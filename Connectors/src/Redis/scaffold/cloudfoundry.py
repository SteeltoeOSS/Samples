from pysteel import cloudfoundry


def setup(context):
    """
    :type context: behave.runner.Context
    """
    cf = cloudfoundry.CloudFoundry(context)
    # remove previous app
    app = 'redis-connector'
    cf.delete_app(app)
    # create service
    service = 'p.redis'
    plan = 'on-demand-cache'
    instance = 'myRedisService'
    cf.create_service(service, plan, instance)
