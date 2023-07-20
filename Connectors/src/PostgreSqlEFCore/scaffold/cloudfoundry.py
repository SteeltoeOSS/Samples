from pysteel import cloudfoundry


def setup(context):
    """
    :type context: behave.runner.Context
    """
    cf = cloudfoundry.CloudFoundry(context)
    # remove previous app
    app = 'postgresqlefcore-connector'
    cf.delete_app(app)
    # create service
    service = 'csb-google-postgres'
    plan = 'default'
    instance = 'myPostgreSqlService'
    cf.create_service(service, plan, instance)
