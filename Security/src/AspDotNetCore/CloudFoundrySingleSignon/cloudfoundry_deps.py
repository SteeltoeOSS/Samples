from steeltoe.samples import cloudfoundry
from steeltoe.samples import dns
from steeltoe.samples.command import Command


def deploy(context):
    """
    :type context: behave.runner.Context
    """
    cf = cloudfoundry.CloudFoundry(context)
    uaa_app_status = cf.get_app_status('uaa')
    if uaa_app_status is None:
        hostname = dns.resolve_hostname(context, 'uaa')
        domainname = dns.resolve_domainname(context, 'x.y.z')
        for cmd_s in [
            'git clone https://github.com/cloudfoundry/uaa.git',
            'git -C uaa checkout 4.7.1',
            'uaa/gradlew -p uaa -Dapp={} -Dapp-domain={} manifests -Dorg.gradle.daemon=false'.format(hostname,
                                                                                                     domainname),
        ]:
            Command(context, cmd_s).run()
        cf.push_app('uaa/build/sample-manifests/uaa-cf-application.yml')
        for cmd_s in [
            'uaac target {}'.format(dns.resolve_url(context, 'https://uaa.x.y.z')),
            'uaac token client get admin -s adminsecret',
            'uaac contexts',
            'uaac group add testgroup',
            'uaac user add testuser --given_name Test --family_name User --emails testuser@domain.com --password Password1!',
            'uaac member add testgroup testuser',
            'uaac client add myTestApp --scope cloud_controller.read,cloud_controller_service_permissions.read,openid,testgroup --authorized_grant_types authorization_code,refresh_token --authorities uaa.resource --redirect_uri {} --autoapprove cloud_controller.read,cloud_controller_service_permissions.read,openid,testgroup --secret myTestApp'.format(
                'https://single-signon.x.y.z/signin-cloudfoundry'),
            'cf create-user-provided-service myOAuthService -p \'{{"client_id":"myTestApp", "client_secret":"myTestApp", "uri":"{}"}}\''.format(
                dns.resolve_url(context, 'uaa://uaa.x.y.z')),
        ]:
            Command(context, cmd_s).run()
