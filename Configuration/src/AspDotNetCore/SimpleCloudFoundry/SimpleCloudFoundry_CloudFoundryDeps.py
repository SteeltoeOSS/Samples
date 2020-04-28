from steeltoe.samples import cloudfoundry


def deploy(context):
    """
    :type context: behave.runner.Context
    """
    instance = 'myConfigServer'
    context.log.info('deploying "{}"'.format(instance))
    cf = cloudfoundry.CloudFoundry(context)
    cf.create_service('p-config-server', 'standard', instance, ['-c', './config-server.json'])

