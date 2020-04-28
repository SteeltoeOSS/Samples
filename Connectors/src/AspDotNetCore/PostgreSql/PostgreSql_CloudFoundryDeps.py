from steeltoe.samples import cloudfoundry


def deploy(context):
    """
    :type context: behave.runner.Context
    """
    instance = 'myPostgres'
    context.log.info('deploying "{}"'.format(instance))
    cf = cloudfoundry.CloudFoundry(context)
    cf.create_service(
        'postgresql-10-odb',
        'standalone',
        instance,
        ['-c', '\'{"db_name":"postgresample", "db_username":"steeltoe", "owner_name":"Steeltoe Demo", "owner_email":"demo@steeltoe.io"}\'']
    )
