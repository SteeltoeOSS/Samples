import os
import re
import time

import yaml

from . import command


class CloudFoundryObjectDoesNotExistError(Exception):
    pass


class CloudFoundryRouteError(Exception):
    pass


class CloudFoundry(object):

    def __init__(self, context):
        self._context = context

    def login(self, api_url, username, password, org, space):
        """
        Login to Cloud Foundry
        :type api_url: str
        :type username: str
        :type password: str
        :type org: str
        :type space: str
        """
        self._context.log.info('logging into Cloud Foundry')
        cmd_s = 'cf login -a {} -u {} -p {} -o {} -s {}'.format(api_url, username, password, org, space)
        command.Command(self._context, cmd_s).run()

    def get_api_endpoint(self):
        self._context.log.info('getting Cloud Foundry target')
        cmd_s = 'cf target'
        cmd = command.Command(self._context, cmd_s)
        cmd.run()
        m = re.search(r'^(API|api) endpoint:\s*(.*)', cmd.stdout, re.MULTILINE)
        if not m:
            raise Exception("couldn't guess domain; cf target did not return api endpoint")
        return m.group(2)

    def space_exists(self, name):
        """
        Return True if the space exists in the targeted org.
        :type name: str
        """
        cmd_s = 'cf space {}'.format(name)
        cmd = command.Command(self._context, cmd_s)
        cmd.run(raise_on_error=False)
        return cmd.rc == 0

    def create_space(self, name):
        """
        Create the space if it does not exist; continue when it already exists.
        Returns True if the space was created, False if it already existed.
        :type name: str
        :rtype: bool
        """
        self._context.log.info('ensuring Cloud Foundry space "{}"'.format(name))
        if self.space_exists(name):
            self._context.log.info('space "{}" already exists'.format(name))
            return False
        cmd_s = 'cf create-space {}'.format(name)
        cmd = command.Command(self._context, cmd_s)
        cmd.run(raise_on_error=False)
        if cmd.rc == 0:
            return True
        combined = ((cmd.stdout or '') + '\n' + (cmd.stderr or '')).lower()
        if 'already exists' in combined:
            self._context.log.info('space "{}" already exists (create-space)'.format(name))
            return False
        raise command.CommandException(
            'cf create-space failed: rc={}, stderr:\n{}'.format(cmd.rc, cmd.stderr))

    def delete_space(self, name):
        """
        :type name: str
        """
        self._context.log.info('deleting Cloud Foundry space "{}"'.format(name))
        cmd_s = 'cf delete-space {} -f'.format(name)
        command.Command(self._context, cmd_s).run()

    def target_space(self, name):
        """
        :type name: str
        """
        self._context.log.info('targeting Cloud Foundry space "{}"'.format(name))
        cmd_s = 'cf target -s {}'.format(name)
        command.Command(self._context, cmd_s).run()

    def create_service(self, service_offering, service_plan, service_instance, args=None, skip_logs=False, alternatives=None):
        """
        Provision a service instance, retrying with additional marketplace pairs if ``cf create-service`` fails.
        :param service_offering: primary service offering name.
        :param service_plan: plan name for the primary offering.
        :param service_instance: name for the new service instance.
        :param args: optional extra arguments appended to ``cf create-service`` (e.g. ``-c`` config).
        :param skip_logs: when True, suppress command stdout/stderr for the create step.
        :param alternatives: optional list of ``(service_offering, service_plan)`` tuples tried in order after the primary pair fails.
        :type service_offering: str
        :type service_plan: str
        :type service_instance: str
        :type args: list or None
        :type skip_logs: bool
        :type alternatives: list or None
        """
        candidates = [(service_offering, service_plan)]
        if alternatives:
            for alternative_offering, alternative_plan in alternatives:
                candidates.append((alternative_offering, alternative_plan))
        errors = []
        for i, (attempt_offering, attempt_plan) in enumerate(candidates):
            try:
                self._create_service_impl(attempt_offering, attempt_plan, service_instance, args, skip_logs)
                return
            except Exception as e:
                errors.append((attempt_offering, attempt_plan, e))
                if i < len(candidates) - 1:
                    self._context.log.warning('create_service "{}:{} {}" failed ({}), trying next marketplace candidate'.format(attempt_offering, attempt_plan, service_instance, e))
        details = '; '.join(
            '[{}] {}:{} - {}'.format(i + 1, o, p, e)
            for i, (o, p, e) in enumerate(errors)
        )
        raise Exception(
            'create_service exhausted all {} candidate(s) for instance "{}": {}'.format(
                len(errors), service_instance, details))

    def _create_service_impl(self, service_offering, service_plan, service_instance, args=None, skip_logs=False):
        """
        Run ``cf create-service`` once and poll until the instance is ready or fails.
        :type service_offering: str
        :type service_plan: str
        :type service_instance: str
        :type args: list or None
        :type skip_logs: bool
        """
        self.wait_until_service_ready_to_create(service_instance)
        if args is None:
            cmd_base = 'cf create-service {} {} {}'.format(service_offering, service_plan, service_instance)
        else:
            cmd_base = ['cf', 'create-service', service_offering, service_plan, service_instance] + args
        self._context.log.info('creating Cloud Foundry service "{}:{}" instance "{}"'.format(service_offering, service_plan, service_instance))
        if skip_logs:
            cmd = command.Command(self._context, cmd_base, log_func=self._context.log.nolog)
        else:
            cmd = command.Command(self._context, cmd_base)
        cmd.run()
        if cmd.rc != 0:
            raise Exception('create service instance failed: {}'.format(service_instance))
        self._context.log.info('waiting for service instance "{}" to become available'.format(service_instance))
        attempts = 0
        while True:
            attempts += 1
            if self._context.options.cf.max_attempts >= 0:
                if attempts > self._context.options.cf.max_attempts:
                    assert False, "maximum attempts exceeded ({})".format(self._context.options.cf.max_attempts)
                self._context.log.info("attempt {}/{}".format(attempts, self._context.options.cf.max_attempts))
            else:
                self._context.log.info("attempt {}".format(attempts))
            try:
                status = self.get_service_status(service_instance)
            except CloudFoundryObjectDoesNotExistError:
                status = None

            if (status == 'create succeeded') or (status == 'update succeeded'):
                break
            if status in ['create failed', 'update failed']:
                assert False, "service instance {} creation/update failed".format(service_instance)
            if status is None:
                self._context.log.info('service instance "{}" status not yet available'.format(service_instance))
            else:
                self._context.log.info('service instance "{}" status: "{}"'.format(service_instance, status))
            time.sleep(self._context.options.cmd.loop_wait)

    def create_user_provided_service(self, service_instance, credentials=None):
        """
        :type service_instance: str
        :type credentials: str
        """
        self.wait_until_service_ready_to_create(service_instance)
        cmd_s = 'cf create-user-provided-service {}'.format(service_instance)
        if credentials:
            cmd_s += ' -p {}'.format(credentials)
        cmd = command.Command(self._context, cmd_s)
        cmd.run()
        if cmd.rc != 0:
            raise Exception('create user provided service instance failed: {}'.format(service_instance))

    def wait_until_service_ready_to_delete(self, service_instance):
        """
        Before ``cf delete-service``, wait out any in-progress operation that would cause CF to
        reject the delete (``create in progress``, ``update in progress``).
        :type service_instance: str
        """
        blocking_statuses = frozenset({'create in progress', 'update in progress'})
        attempts = 0
        while True:
            attempts += 1
            if self._context.options.cf.max_attempts >= 0:
                if attempts > self._context.options.cf.max_attempts:
                    assert False, (
                        'maximum attempts exceeded ({}) waiting for service instance "{}" '
                        'to become ready to delete'
                    ).format(self._context.options.cf.max_attempts, service_instance)
                self._context.log.info(
                    'wait create-clear {}/{}'.format(attempts, self._context.options.cf.max_attempts),
                )
            else:
                self._context.log.info('wait create-clear attempt {}'.format(attempts))
            try:
                status = self.get_service_status(service_instance)
            except CloudFoundryObjectDoesNotExistError:
                return
            normalized = (status or '').strip().lower()
            if normalized in blocking_statuses:
                self._context.log.info(
                    'service instance "{}" status "{}"; waiting before delete'.format(
                        service_instance,
                        status,
                    ),
                )
                time.sleep(self._context.options.cmd.loop_wait)
                continue
            return

    def delete_service(self, service_instance):
        """
        :type service_instance: str
        """
        self.wait_until_service_ready_to_delete(service_instance)
        self._context.log.info('deleting Cloud Foundry service instance "{}"'.format(service_instance))
        cmd_s = 'cf delete-service -f {}'.format(service_instance)
        cmd = command.Command(self._context, cmd_s)
        cmd.run()
        self._context.log.info('waiting for service instance "{}" to be removed'.format(service_instance))
        attempts = 0
        while True:
            attempts += 1
            if self._context.options.cf.max_attempts >= 0:
                if attempts > self._context.options.cf.max_attempts:
                    assert False, "maximum attempts exceeded ({})".format(self._context.options.cf.max_attempts)
                self._context.log.info("attempt {}/{}".format(attempts, self._context.options.cf.max_attempts))
            else:
                self._context.log.info("attempt {}".format(attempts))
            
            try:
                status = self.get_service_status(service_instance)
                if status == 'delete failed':
                    assert False, "service instance {} deletion failed".format(service_instance)
                self._context.log.info('service instance still exists')
            except CloudFoundryObjectDoesNotExistError:
                break
                
            time.sleep(self._context.options.cmd.loop_wait)

    def service_exists(self, service_instance):
        """
        :type service_instance: str
        """
        cmd_s = 'cf service {}'.format(service_instance)
        cmd = command.Command(self._context, cmd_s)
        try:
            cmd.run()
            return True
        except command.CommandException as e:
            if 'Service instance not found' in str(e) or 'Service instance \'{}\' not found'.format(service_instance) in str(e):
                return False
            raise e

    def get_service_status(self, service_instance):
        """
        :type service_instance: str
        """
        cmd_s = 'cf service {}'.format(service_instance)
        cmd = command.Command(self._context, cmd_s)
        try:
            cmd.run()
        except command.CommandException as e:
            if 'Service instance not found' in str(e) or 'Service instance \'{}\' not found'.format(service_instance) in str(e):
                raise CloudFoundryObjectDoesNotExistError()
            raise e
        match = re.search(r'\s*status:\s+(.*)', cmd.stdout, re.MULTILINE)
        if match:
            return match.group(1).strip()

    def wait_until_service_ready_to_create(self, service_instance):
        """
        Before ``cf create-service``, wait out a leftover instance from a prior run that is still
        deleting (``delete in progress``). Once the instance is gone, creation can proceed.
        Fails immediately if the instance is in ``delete failed`` — CF admin purge is required.
        :type service_instance: str
        """
        blocking_statuses = frozenset({'delete in progress'})
        attempts = 0
        while True:
            attempts += 1
            if self._context.options.cf.max_attempts >= 0:
                if attempts > self._context.options.cf.max_attempts:
                    assert False, (
                        'maximum attempts exceeded ({}) waiting for service instance "{}" '
                        'to finish deleting'
                    ).format(self._context.options.cf.max_attempts, service_instance)
                self._context.log.info(
                    'wait delete-clear {}/{}'.format(attempts, self._context.options.cf.max_attempts),
                )
            else:
                self._context.log.info('wait delete-clear attempt {}'.format(attempts))
            try:
                status = self.get_service_status(service_instance)
            except CloudFoundryObjectDoesNotExistError:
                return
            normalized = (status or '').strip().lower()
            if normalized == 'delete failed':
                assert False, (
                    'service instance "{}" is stuck in "delete failed" state; '
                    'CF admin must run: cf purge-service-instance -f {}'
                ).format(service_instance, service_instance)
            if normalized in blocking_statuses:
                self._context.log.info(
                    'service instance "{}" status "{}"; waiting before create'.format(
                        service_instance,
                        status,
                    ),
                )
                time.sleep(self._context.options.cmd.loop_wait)
                continue
            return

    def push_app(self, manifest, extra_args=""):
        """
        :type manifest: str
        :type extra_args: str
        """
        manifest_yaml = yaml.safe_load(open(os.path.join(self._context.project_dir, manifest), 'r'))
        app_name = manifest_yaml['applications'][0]['name']
        self._context.log.info('pushing Cloud Foundry app "{}" ({})'.format(app_name, manifest))
        cmd_s = f'cf push -t 120 -f {manifest} {extra_args}'.strip()
        cmd = command.Command(self._context, cmd_s)
        cmd.run(raise_on_error=False)

        self._context.log.info('cf push is complete, allow some stabilization time')
        time.sleep(30)
        self._context.log.info('sleep complete, proceeding with test...')

        attempts = 0
        crashCount = 0
        while True:
            attempts += 1
            if self._context.options.cf.max_attempts >= 0:
                if attempts > self._context.options.cf.max_attempts:
                    assert False, "maximum attempts exceeded ({})".format(self._context.options.cf.max_attempts)
                self._context.log.info("attempt {}/{}".format(attempts, self._context.options.cf.max_attempts))
            else:
                self._context.log.info("attempt {}".format(attempts))

            try:
                status = self.get_app_status(app_name)
            except CloudFoundryObjectDoesNotExistError:
                status = None

            if status is None:
                self._context.log.info('app "{}" status not yet available'.format(app_name))
            else:
                self._context.log.info('app "{}" status: "{}"'.format(app_name, status))

            if status == 'running':
                break
            if status == 'crashed':
                crashCount += 1
                if crashCount > 10:
                    assert False, "app {} crashed".format(app_name)

            time.sleep(self._context.options.cmd.loop_wait)

    def delete_app(self, app_name):
        """
        :type app_name: str
        """
        self._context.log.info('deleting Cloud Foundry app "{}"'.format(app_name))
        cmd_s = 'cf delete -f -r {}'.format(app_name)
        command.Command(self._context, cmd_s).run()

    def app_exists(self, app_name):
        """
        :type app_name: str
        """
        cmd_s = 'cf app {}'.format(app_name)
        cmd = command.Command(self._context, cmd_s)
        try:
            cmd.run()
            return True
        except command.CommandException as e:
            if "App '{}' not found".format(app_name) in str(e):
                return False
            raise e

    def get_app_status(self, app_name):
        """
        :type app_name: str
        """
        cmd_s = 'cf app {}'.format(app_name)
        cmd = command.Command(self._context, cmd_s)
        try:
            cmd.run()
        except command.CommandException as e:
            if "App '{}' not found".format(app_name) in str(e):
                raise CloudFoundryObjectDoesNotExistError()
            if "Requested route" in str(e) and "does not exist" in str(e):
                self._context.log.error('routing error: {}'.format(e))
                raise CloudFoundryRouteError()
            raise e
        match = re.search(r'^#0\s+(\S+)', cmd.stdout, re.MULTILINE)
        if not match:
            return None
        return match.group(1)

    def get_task_status(self, app_name, task_name):
        """
        :type app_name: str
        :type task_name: str
        """
        cmd_s = 'cf tasks {}'.format(app_name)
        cmd = command.Command(self._context, cmd_s)
        try:
            cmd.run()
        except command.CommandException as e:
            if "App '{}' not found".format(app_name) in str(e):
                raise CloudFoundryObjectDoesNotExistError()
            raise e
        match = re.search('(.*?--name {})'.format(task_name), cmd.stdout, re.MULTILINE)
        return match.group(1).split()[2]

    def get_recent_logs(self, app_name, source_filter=None, tail=50):
        """
        Retrieve recent log output for an app via 'cf logs --recent'.
        When source_filter is provided, returns only lines whose CF source tag contains it
        (e.g. 'APP/TASK/' for task containers, 'APP/PROC/WEB/' for the web process).
        Falls back to the last `tail` lines when the filter matches nothing.
        :type app_name: str
        :type source_filter: str or None
        :type tail: int
        """
        cmd = command.Command(self._context, 'cf logs --recent {}'.format(app_name))
        cmd.run(raise_on_error=False)
        output = (cmd.stdout or '').strip()
        if not output:
            return '(no recent logs available)'
        lines = output.splitlines()
        if source_filter:
            filtered = [l for l in lines if source_filter in l]
            if filtered:
                return '\n'.join(filtered)
        return '\n'.join(lines[-tail:])

    def get_recent_task_logs(self, app_name):
        """
        Get recent logs filtered to CF task output (lines tagged [APP/TASK/...]).
        CF auto-generates the task name so we match the source prefix rather than the name.
        :type app_name: str
        """
        return self.get_recent_logs(app_name, source_filter='APP/TASK/')

    def get_app_route(self, app_name):
        """
        :type app_name: str
        """
        cmd_s = 'cf app {}'.format(app_name)
        cmd = command.Command(self._context, cmd_s)
        try:
            cmd.run()
        except command.CommandException as e:
            if "App '{}' not found".format(app_name) in str(e):
                raise CloudFoundryObjectDoesNotExistError()
            raise e
        match = re.search(r'^routes:\s+(\S+)', cmd.stdout, re.MULTILINE)
        if not match:
            return None
        return match.group(1)
