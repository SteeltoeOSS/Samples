from steeltoe.samples import cloudfoundry


def deploy(context):
    """
    :type context: behave.runner.Context
    """
    instance = 'myRabbitMQService'
    context.log.info('deploying "{}"'.format(instance))
    cf = cloudfoundry.CloudFoundry(context)
    cf.create_service('p-rabbitmq', 'standard', instance)
