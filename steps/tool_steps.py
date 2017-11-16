import command
import re
import uuid

@given(u'you have .NET Core SDK {version} installed')
def step_impl(context, version):
    cmd = command.Command(context, "dotnet --version")
    cmd.run()
    actual = cmd.stdout
    actual.should.match(r'{}.*'.format(version))

@given(u'you have Java {version} installed')
def step_impl(context, version):
    cmd = command.Command(context, "java -version")
    cmd.run()
    actual = cmd.stderr
    actual.should.match(r'{}.*'.format(version))

@given(u'you have Apache Maven {version} installed')
def step_impl(context, version):
    cmd = command.Command(context, "mvn --version")
    cmd.run()
    actual = cmd.stdout
    actual.should.match(r'{}.*'.format(version))

@given(u'you are logged into CloudFoundry')
def step_impl(context):
    apiurl = context.options.cf_apiurl
    assert apiurl, 'CloudFoundry API URL not set (option: cf_apiurl)'
    username = context.options.cf_username
    assert username, 'CloudFoundry username not set (option: cf_username)'
    password = context.options.cf_password
    assert password, 'CloudFoundry password not set (option: cf_password)'
    org = context.options.cf_org
    assert org, 'CloudFoundry org not set (option: cf_org)'
    context.cf_space = context.options.cf_space
    if not context.cf_space:
        context.cf_space = uuid.uuid4()
    command.Command(context, 'cf login -a {} -u {} -p {} -o {} -s development'.format(
        apiurl, username, password, org)).run()
    command.Command(context, 'cf create-space {}'.format(context.cf_space)).run()
    command.Command(context, 'cf target -s {}'.format(context.cf_space)).run()
    def cleanup():
        cmd = command.Command(context, 'cf delete-space -f {}'.format(context.cf_space))
        cmd.exec()
        cmd.wait()
    context.cleanups.append(cleanup)

@given(u'you have CloudFoundry service {service} installed')
def step_impl(context, service):
    cmd = command.Command(context, "cf marketplace")
    cmd.run()
    actual = cmd.stdout
    actual.should.match(r'^{}\s'.format(service), re.MULTILINE)
