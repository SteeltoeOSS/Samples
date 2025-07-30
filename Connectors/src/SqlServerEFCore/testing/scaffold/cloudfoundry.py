from pysteel import cloudfoundry


def setup(context):
    """
    :type context: behave.runner.Context
    """
    cf = cloudfoundry.CloudFoundry(context)
    # remove previous app
    app = 'sqlserver-efcore-connector-sample'
    cf.delete_app(app)
    # create service
    service = 'csb-azure-mssql-db'
    plan = 'small-v2'
    instance = 'sampleSqlServerService'
    cf.create_service(service, plan, instance)

def teardown(context):
    """
    :type context: behave.runner.Context
    """
    cf = cloudfoundry.CloudFoundry(context)
    cf.delete_app('sqlserver-efcore-connector-sample')
    cf.delete_service('sampleSqlServerService')
