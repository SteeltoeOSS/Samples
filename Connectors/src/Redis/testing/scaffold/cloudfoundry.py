from pysteel import cloudfoundry


def setup(context):
    """
    :type context: behave.runner.Context
    """
    cf = cloudfoundry.CloudFoundry(context)
    # remove previous app
    app = 'redis-connector-sample'
    cf.delete_app(app)
    # create service
    service = 'p-redis'
    plan = 'shared-vm'
    instance = 'sampleRedisService'
    cf.create_service(service, plan, instance, alternatives=[('p.redis', 'vk-plan')])

def teardown(context):
    """
    :type context: behave.runner.Context
    """
    cf = cloudfoundry.CloudFoundry(context)
    cf.delete_app('redis-connector-sample')
    cf.delete_service('sampleRedisService')
