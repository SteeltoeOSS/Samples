import re
import time

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

    def create_space(self, space):
        """
        :type space: str
        """
        self._context.log.info('creating Cloud Foundry space "{}"'.format(space))
        cmd_s = 'cf create-space {}'.format(space)
        command.Command(self._context, cmd_s).run()

    def target_space(self, space):
        """
        :type space: str
        """
        self._context.log.info('targeting Cloud Foundry space "{}"'.format(space))
        cmd_s = 'cf target -s {}'.format(space)
        command.Command(self._context, cmd_s).run()

    def create_service(self, service, plan, instance, args=[]):
        """
        :type service: str
        :type plan: str
        :type instance: str
        :type args: list
        """
        self._context.log.info('creating Cloud Foundry service "{}"'.format(instance))
        cmd_s = 'cf create-service {} {} {}'.format(service, plan, instance)
        if args:
            cmd_s += ' ' + ' '.join(args)
        cmd = command.Command(self._context, cmd_s)
        cmd.run()
        if cmd.rc != 0:
            raise Exception('create service failed: {}'.format(instance))
        self._context.log.info('waiting for service "{}" to become available'.format(instance))
        attempts = 0
        while True:
            attempts += 1
            if self._context.options.cf.max_attempts >= 0:
                if attempts > self._context.options.cf.max_attempts:
                    assert False, "maximum attempts exceeded ({})".format(self._context.options.cf.max_attempts)
                self._context.log.info("attempt {}/{}".format(attempts, self._context.options.cf.max_attempts))
            else:
                self._context.log.info("attempt {}".format(attempts))
            cmd_s = 'cf services'
            cmd = command.Command(self._context, cmd_s, log_func=self._context.log.debug)
            cmd.run()
            if not re.search(r'^{}\s'.format(instance), cmd.stdout, re.MULTILINE):
                self._context.log.info('waiting for service "{}" deployment to start'.format(instance))
                continue
            cmd_s = 'cf service {}'.format(instance)
            cmd = command.Command(self._context, cmd_s, log_func=self._context.log.debug)
            cmd.run()
            match = re.search(r'^status:\s+(.*)', cmd.stdout, re.MULTILINE)
            if not match:
                self._context.log.info('service "{}" status not yet available'.format(instance))
                continue
            status = match.group(1)
            self._context.log.info('service "{}" status: "{}"'.format(instance, status))
            if status == 'create succeeded':
                break
            time.sleep(1)
