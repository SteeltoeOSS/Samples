from pysteel import cloudfoundry


def setup(context):
    """
    :type context: behave.runner.Context
    """
    cf = cloudfoundry.CloudFoundry(context)
    # remove previous app
    app = 'redis-data-protection-sample'
    cf.delete_app(app)
    # create service
    service = 'p.redis'
    plan = 'vk-plan'
    instance = 'sampleRedisDataProtectionService'
    cf.create_service(service, plan, instance)

def teardown(context):
    """
    :type context: behave.runner.Context
    """
    cf = cloudfoundry.CloudFoundry(context)
    cf.delete_app('redis-data-protection-sample')
    cf.delete_service('sampleRedisDataProtectionService')
