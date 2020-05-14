import re
import socket
import time

from behave import *

from steeltoe.samples.command import Command, CommandException
from steeltoe.samples.cloudfoundry import CloudFoundry, CloudFoundryObjectDoesNotExistError


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


@when(u'you wait until CloudFoundry app {app} is started')
def step_impl(context, app):
    """
    :type context: behave.runner.Context
    :type app: str
    """

    def app_started():
        try:
           return CloudFoundry(context).get_app_status(app) == 'running'
        except CloudFoundryObjectDoesNotExistError:
            return False

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
