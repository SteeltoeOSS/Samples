from pysteel import cloudfoundry


def setup(context):
    """
    :type context: behave.runner.Context
    """
    cf = cloudfoundry.CloudFoundry(context)
    # remove previous app
    app = 'oauth-connector'
    cf.delete_app(app)
    # create user-provided service
    instance = 'myOAuthService'
    cf.create_user_provided_service(instance, 'oauth.json')  # TODO... just guessing here; should run: `cf cups myOAuthService -p oauth.json`
