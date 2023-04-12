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
    domain = 'apps.{}'.format(context.options.cf.apiurl.removeprefix('https://api.sys.'))
    oauth = 'myOAuthService'
    uaa = 'uaa'
    uaa_url = 'uaa://{}.{}'.format(uaa, domain)
    sso = 'single-signon'
    sso_url = 'https://{}.{}'.format(sso, domain)
    # remove previous apps
    cf.delete_app(sso)
    cf.delete_app(uaa)
    # remove previous services
    cf.delete_service(oauth)
    # create UAA service
    credentials = '\'{{"client_id":"myTestApp", "client_secret":"myTestApp", "uri":"{}"}}\''.format(uaa_url)
    cf.create_user_provided_service(oauth, credentials)
    # create UAA app
    uaa_repo = os.path.join(context.project_dir, uaa)
    if os.path.exists(uaa_repo):
        def remove_readonly(func, path, excinfo):
            os.chmod(path, stat.S_IWRITE)
            func(path)

        shutil.rmtree(uaa_repo, onerror=remove_readonly)
    for cmd_s in [
        'git clone https://github.com/cloudfoundry/uaa.git',
        'git -C uaa checkout 4.7.1',
        'uaa/gradlew -p uaa -Dapp={} -Dapp-domain={} manifests -Dorg.gradle.daemon=false'.format(uaa,
                                                                                                 domain),
    ]:
        Command(context, cmd_s).run()
    cf.push_app('uaa/build/sample-manifests/uaa-cf-application.yml')
    # configure UAA app
    for cmd_s in [
        'uaac target {}'.format(dns.resolve_url(context, 'https://uaa')),
        'uaac token client get admin -s adminsecret',
        'uaac contexts',
        'uaac group add testgroup',
        'uaac user add testuser --given_name Test --family_name User --emails testuser@domain.com --password Password1!',
        'uaac member add testgroup testuser',
        'uaac client add myTestApp --scope cloud_controller.read,cloud_controller_service_permissions.read,openid,testgroup --authorized_grant_types authorization_code,refresh_token --authorities uaa.resource --redirect_uri {}/signin-cloudfoundry --autoapprove cloud_controller.read,cloud_controller_service_permissions.read,openid,testgroup --secret myTestApp'.format(
            sso_url),
    ]:
        Command(context, cmd_s).run()
