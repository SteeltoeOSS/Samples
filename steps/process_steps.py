import re
import socket
import time

from behave import *

from steeltoe.samples.command import Command, CommandException


@when(u'you set env var {name} to ""')
def step_impl(context, name):
    """
    :type context: behave.runner.Context
    :type name: str
    """
    try:
        del (context.env[name])
    except KeyError:
        pass


@when(u'you set env var {name} to "{value}"')
def step_impl(context, name, value):
    """
    :type context: behave.runner.Context
    :type name: str
    :type value: str
    """
    context.env[name] = value


@when(u'you run: {command}')
def step_impl(context, command):
    """
    :type context: behave.runner.Context
    :type command: str
    """
    Command(context, command).run()


@when(u'you run in the background: {command}')
def step_impl(context, command):
    """
    :type context: behave.runner.Context
    :type command: str
    """
    cmd = Command(context, command, windowed=True)
    cmd.exec()

    def cleanup():
        cmd.kill()

    context.cleanups.append(cleanup)


# noinspection PyBDDParameters
@when(u'you wait until process listening on port {port:d}')
def step_impl(context, port):
    """
    :type context: behave.runner.Context
    :type port: int
    """

    def port_listening():
        try:
            sock = socket.socket()
            sock.connect(('localhost', port))
            sock.close()
            return True
        except ConnectionRefusedError:
            return False

    try_until(context, port_listening, context.options.max_attempts)


@when(u'you wait until CloudFoundry service {service} is created')
def step_impl(context, service):
    """
    :type context: behave.runner.Context
    :type service: str
    """

    def service_available():
        cmd = Command(context, 'cf services', logf=context.log.debug)
        cmd.run()
        if not re.search(r'^{}\s'.format(service), cmd.stdout, re.MULTILINE):
            context.log.info('waiting for service {} deployment to start'.format(service))
            return False
        cmd = Command(context, 'cf service {}'.format(service), logf=context.log.debug)
        cmd.run()
        match = re.search(r'^status:\s+(.*)', cmd.stdout, re.MULTILINE)
        if not match:
            context.log.info('service "{}" status not yet available'.format(service))
            return False
        status = match.group(1)
        context.log.info('service "{}" status: "{}"'.format(service, status))
        return status == 'create succeeded'

    try_until(context, service_available, context.options.cf.max_attempts)


@when(u'you wait until CloudFoundry app {app} is started')
def step_impl(context, app):
    """
    :type context: behave.runner.Context
    :type app: str
    """

    def app_started():
        cmd = Command(context, 'cf apps', log_func=context.log.debug)
        cmd.run()
        if not re.search(r'^{}\s'.format(app), cmd.stdout, re.MULTILINE):
            context.log.info('waiting for app {} deployment to start'.format(app))
            return False
        cmd = Command(context, 'cf app {}'.format(app), log_func=context.log.debug)
        try:
            cmd.run()
        except CommandException as e:
            context.log.info('unexpected exception: {}'.format(e))
            return False
        match = re.search(r'^#0\s+(\S+)', cmd.stdout, re.MULTILINE)
        if not match:
            context.log.info('app "{}" status not yet available'.format(app))
            return False
        status = match.group(1)
        if status == 'crashed':
            raise Exception('CloudFoundry app {} has crashed'.format(app))
        context.log.info('app "{}" status: "{}"'.format(app, status))
        return status == 'running'

    try_until(context, app_started, context.options.cf.max_attempts)


def try_until(context, function, max_attempts):
    """
    :type context: behave.runner.Context
    :type function: function
    :type max_attempts: int
    """
    attempts = 0
    while True:
        attempts += 1
        if max_attempts >= 0:
            if attempts > max_attempts:
                assert False, "maximum attemps exceeded ({})".format(max_attempts)
            context.log.info("attempt {}/{}".format(attempts, max_attempts))
        else:
            context.log.info("attempt {}".format(attempts))
        if function():
            break
        time.sleep(1)
