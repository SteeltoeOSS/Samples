import os
import re
import time

import yaml

from . import command


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
        m = re.search(r'^api endpoint:\s*(.*)', cmd.stdout, re.MULTILINE)
        if not m:
            raise Exception("couldn't guess domain; cf target did not return api endpoint")
        return m.group(1)

    def create_space(self, name):
        """
        :type name: str
        """
        self._context.log.info('creating Cloud Foundry space "{}"'.format(name))
        cmd_s = 'cf create-space {}'.format(name)
        command.Command(self._context, cmd_s).run()

    def target_space(self, name):
        """
        :type name: str
        """
        self._context.log.info('targeting Cloud Foundry space "{}"'.format(name))
        cmd_s = 'cf target -s {}'.format(name)
        command.Command(self._context, cmd_s).run()

    def create_service(self, service, plan, name, args=[]):
        """
        :type service: str
        :type plan: str
        :type name: str
        :type args: list
        """
        self._context.log.info('creating Cloud Foundry service "{}"'.format(name))
        cmd_s = 'cf create-service {} {} {}'.format(service, plan, name)
        if args:
            cmd_s += ' ' + ' '.join(args)
        cmd = command.Command(self._context, cmd_s)
        cmd.run()
        if cmd.rc != 0:
            raise Exception('create service failed: {}'.format(name))
        self._context.log.info('waiting for service "{}" to become available'.format(name))
        attempts = 0
        while True:
            attempts += 1
            if self._context.options.cf.max_attempts >= 0:
                if attempts > self._context.options.cf.max_attempts:
                    assert False, "maximum attempts exceeded ({})".format(self._context.options.cf.max_attempts)
                self._context.log.info("attempt {}/{}".format(attempts, self._context.options.cf.max_attempts))
            else:
                self._context.log.info("attempt {}".format(attempts))
            status = self.get_service_status(name)
            if status == 'create succeeded':
                break
            if status is None:
                self._context.log.info('service "{}" status not yet available'.format(name))
            else:
                self._context.log.info('service "{}" status: "{}"'.format(name, status))
            time.sleep(1)

    def get_service_status(self, name):
        """
        :type name: str
        """
        cmd_s = 'cf service {}'.format(name)
        cmd = command.Command(self._context, cmd_s, log_func=self._context.log.debug)
        try:
            cmd.run()
        except command.CommandException as e:
            if 'Service instance {} not found'.format(instance) in str(e):
                return None
            raise e
        match = re.search(r'^status:\s+(.*)', cmd.stdout, re.MULTILINE)
        if not match:
            return None
        return match.group(1)

    def push_app(self, manifest):
        """
        :type manifest: str
        """
        cmd_s = 'cf push -f {}'.format(manifest)
        cmd = command.Command(self._context, cmd_s)
        cmd.run()
        if cmd.rc != 0:
            raise Exception('push app failed: {}'.format(manifest))
        manifest_yaml = yaml.load(open(os.path.join(self._context.sandbox_dir, manifest), 'r'))
        name = manifest_yaml['applications'][0]['name']
        attempts = 0
        while True:
            attempts += 1
            if self._context.options.cf.max_attempts >= 0:
                if attempts > self._context.options.cf.max_attempts:
                    assert False, "maximum attempts exceeded ({})".format(self._context.options.cf.max_attempts)
                self._context.log.info("attempt {}/{}".format(attempts, self._context.options.cf.max_attempts))
            else:
                self._context.log.info("attempt {}".format(attempts))
            status = self.get_app_status(name)
            if status == 'running':
                break
            if status is None:
                self._context.log.info('app "{}" status not yet available'.format(name))
            else:
                self._context.log.info('app "{}" status: "{}"'.format(name, status))
            time.sleep(1)

    def get_app_status(self, name):
        """
        :type name: str
        """
        cmd_s = 'cf app {}'.format(name)
        cmd = command.Command(self._context, cmd_s, log_func=self._context.log.debug)
        try:
            cmd.run()
        except command.CommandException as e:
            if "App '{}' not found".format(name) in str(e):
                return None
            raise e
        match = re.search(r'^#0\s+(\S+)', cmd.stdout, re.MULTILINE)
        if not match:
            return None
        return match.group(1)
