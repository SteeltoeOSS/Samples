from pysteel import cloudfoundry


def setup(context):
    """
    :type context: behave.runner.Context
    """
    cf = cloudfoundry.CloudFoundry(context)
    # remove previous app
    app = 'postgresefcore-connector'
    cf.delete_app(app)
    # create service
    service = 'postgresql-10-odb'
    plan = 'standalone'
    instance = 'myPostgres'
    args = ['-c',
            '\'{"db_name":"postgresample", "db_username":"steeltoe", "owner_name":"Steeltoe Demo", "owner_email":"demo@steeltoe.io"}\'']
    cf.create_service(service, plan, instance, args)
