from pysteel import cloudfoundry


def setup(context):
    """
    :type context: behave.runner.Context
    """
    cf = cloudfoundry.CloudFoundry(context)
    # remove previous app
    app = 'configuration-providers-sample'
    cf.delete_app(app)
    # create service
    service = 'p.config-server'
    plan = 'standard'
    instance = 'sampleConfigServer'
    args = ['-c', './config-server.json']
    cf.create_service(service, plan, instance, args)

def teardown(context):
    """
    :type context: behave.runner.Context
    """
    cf = cloudfoundry.CloudFoundry(context)
    cf.delete_app('configuration-providers-sample')
    cf.delete_service('sampleConfigServer')
