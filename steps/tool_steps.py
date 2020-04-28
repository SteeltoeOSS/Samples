import re

from behave import *

from steeltoe.samples import command


@given(u'you have UAA Client {version} installed')
def step_impl(context, version):
    """
    :type context: behave.runner.Context
    :type version: str
    """
    cmd = command.Command(context, "uaac --version")
    cmd.run()
    actual = cmd.stdout
    actual.should.match(r'{}.*'.format(version))


@given(u'you have CloudFoundry service {service} installed')
def step_impl(context, service):
    """
    :type context: behave.runner.Context
    :type service: str
    """
    cmd = command.Command(context, "cf marketplace")
    cmd.run()
    actual = cmd.stdout
    actual.should.match(r'^{}\s'.format(service), re.MULTILINE)
