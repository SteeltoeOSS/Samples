from behave import *
import command
import re
import sure
from version import Version

@given(u'you have at least .NET Core SDK {version} installed')
def step_impl(context, version):
    expected = Version(version)
    cmd = command.Command(context, "dotnet --version")
    cmd.run()
    actual = Version(cmd.stdout)
    (actual >= expected).should.be.true

@given(u'you have Java {version} installed')
def step_impl(context, version):
    cmd = command.Command(context, "java -version")
    cmd.run()
    actual = cmd.stderr
    actual.should.match(r'{}.*'.format(version))

@given(u'you have UAA Client {version} installed')
def step_impl(context, version):
    cmd = command.Command(context, "uaac --version")
    cmd.run()
    actual = cmd.stdout
    actual.should.match(r'{}.*'.format(version))

@given(u'you have Apache Maven {version} installed')
def step_impl(context, version):
    cmd = command.Command(context, "mvn --version")
    cmd.run()
    actual = cmd.stdout
    actual.should.match(r'{}.*'.format(version))

@given(u'you have CloudFoundry service {service} installed')
def step_impl(context, service):
    cmd = command.Command(context, "cf marketplace")
    cmd.run()
    actual = cmd.stdout
    actual.should.match(r'^{}\s'.format(service), re.MULTILINE)
