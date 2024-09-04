from pysteel import cloudfoundry


def setup(context):
    """
    :type context: behave.runner.Context
    """
    cf = cloudfoundry.CloudFoundry(context)
    # remove previous app
    app = 'cosmosdb-connector-sample'
    cf.delete_app(app)
    # create service
    service = 'csb-azure-cosmosdb-sql'
    plan = 'mini'
    instance = 'sampleCosmosDbService'
    cf.create_service(service, plan, instance)
