from pysteel import cloudfoundry


def setup(context):
    """
    :type context: behave.runner.Context
    """
    cf = cloudfoundry.CloudFoundry(context)
    # remove previous app
    app = 'postgresql-connector-sample'
    cf.delete_app(app)
    # create service
    service = 'csb-google-postgres'
    plan = 'gcp-postgres-tiny'
    instance = 'samplePostgreSqlService'
    cf.create_service(service, plan, instance)
