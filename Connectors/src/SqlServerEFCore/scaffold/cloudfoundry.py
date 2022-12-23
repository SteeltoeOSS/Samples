from pysteel import cloudfoundry


def setup(context):
    """
    :type context: behave.runner.Context
    """
    cf = cloudfoundry.CloudFoundry(context)
    # remove previous app
    app = 'sqlserverefcore-connector'
    cf.delete_app(app)
    # create service
    service = 'csb-azure-mssql'
    plan = 'small-v2'
    instance = 'mySqlServerService'
    cf.create_service(service, plan, instance)
