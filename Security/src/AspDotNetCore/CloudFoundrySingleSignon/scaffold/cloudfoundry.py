import os
import shutil
import stat

from pysteel import cloudfoundry, dns
from pysteel.command import Command


def setup(context):
    """
    :type context: behave.runner.Context
    """
    cf = cloudfoundry.CloudFoundry(context)
    # remove previous apps
    cf.delete_app('single-signon')
    cf.delete_app('uaa')
    # create UAA service and app
    if not cf.service_exists('myOAuthService'):
        credentials = '\'{{"client_id":"myTestApp", "client_secret":"myTestApp", "uri":"{}"}}\''.format(
            dns.resolve_url(context, 'uaa://uaa.x.y.z'))
        cf.create_user_provided_service('myOAuthService', credentials)
    if not cf.app_exists('uaa'):
        hostname = dns.resolve_hostname(context, 'uaa')
        domainname = dns.resolve_domainname(context, 'x.y.z')
        uaa_repo = os.path.join(context.project_dir, 'uaa')
        if os.path.exists(uaa_repo):
            def remove_readonly(func, path, excinfo):
                os.chmod(path, stat.S_IWRITE)
                func(path)
            shutil.rmtree(uaa_repo, onerror=remove_readonly)
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
        ]:
            Command(context, cmd_s).run()
