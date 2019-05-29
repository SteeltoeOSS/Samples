from behave import *
from command import Command, CommandException
import re
import socket
import time

@when(u'you set env var {name} to ""')
def step_impl(context, name):
    try:
        del(context.env[name])
    except KeyError:
        pass

@when(u'you set env var {name} to "{value}"')
def step_impl(context, name, value):
    context.env[name] = value

@when(u'you run: {command}')
def step_impl(context, command):
    Command(context, command).run()

@when(u'you run in the background: {command}')
def step_impl(context, command):
    cmd = Command(context, command, windowed=True)
    cmd.exec()
    def cleanup():
        cmd.kill()
    context.cleanups.append(cleanup)

@when(u'you wait until process listening on port {port:d}')
def step_impl(context, port):
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
    def app_started():
        cmd = Command(context, 'cf apps', logf=context.log.debug)
        cmd.run()
        if not re.search(r'^{}\s'.format(app), cmd.stdout, re.MULTILINE):
            context.log.info('waiting for app {} deployment to start'.format(app))
            return False
        cmd = Command(context, 'cf app {}'.format(app), logf=context.log.debug)
        try:
            cmd.run()
        except CommandException as e:
            context.log.info('unexpected exception: {}'.format(e))
            return False
        match = re.search(r'^\#0\s+(\S+)', cmd.stdout, re.MULTILINE)
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
