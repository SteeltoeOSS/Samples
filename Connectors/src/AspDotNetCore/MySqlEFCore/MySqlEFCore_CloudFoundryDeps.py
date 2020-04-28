from steeltoe.samples import cloudfoundry


def deploy(context):
    """
    :type context: behave.runner.Context
    """
    instance = 'myMySqlService'
    context.log.info('deploying "{}"'.format(instance))
    cf = cloudfoundry.CloudFoundry(context)
    cf.create_service('p.mysql', 'db-small', instance)
