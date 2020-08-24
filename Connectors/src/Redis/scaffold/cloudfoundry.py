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
    service = 'p-redis'
    plan = 'shared-vm'
    instance = 'myRedisService'
    cf.create_service(service, plan, instance)
