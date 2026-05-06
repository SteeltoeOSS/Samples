from pysteel import cloudfoundry


def setup(context):
    """
    :type context: behave.runner.Context
    """
    cf = cloudfoundry.CloudFoundry(context)
    # remove previous app
    app = 'postgresql-efcore-connector-sample'
    cf.delete_app(app)
    # create service
    service = 'postgres'
    plan = 'small'
    instance = 'samplePostgreSqlEFCoreService'
    cf.create_service(service, plan, instance, alternatives=[('postgres', 'db-small')])

def teardown(context):
    """
    :type context: behave.runner.Context
    """
    cf = cloudfoundry.CloudFoundry(context)
    cf.delete_app('postgresql-efcore-connector-sample')
    cf.delete_service('samplePostgreSqlEFCoreService')
