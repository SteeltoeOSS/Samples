from pysteel import cloudfoundry


def setup(context):
    """
    :type context: behave.runner.Context
    """
    cf = cloudfoundry.CloudFoundry(context)
    # remove previous app
    app = 'steeltoe-configuration-sample'
    cf.delete_app(app)
    # create service
    service = 'p.config-server'
    plan = 'standard'
    instance = 'myConfigServer'
    args = ['-c', './config-server.json']
    cf.create_service(service, plan, instance, args)
