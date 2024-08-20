from pysteel import cloudfoundry


def setup(context):
    """
    :type context: behave.runner.Context
    """
    cf = cloudfoundry.CloudFoundry(context)
    # remove previous app
    app = 'mongodb-connector-sample'
    cf.delete_app(app)
    # create service
    service = 'csb-azure-mongodb'
    plan = 'small'
    instance = 'sampleMongoDbService'
    cf.create_service(service, plan, instance)
