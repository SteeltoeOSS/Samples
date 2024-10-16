from pysteel import cloudfoundry

def setup(context):
    """
    :type context: behave.runner.Context
    """
    cf = cloudfoundry.CloudFoundry(context)
    service = 'p.mysql'
    plan = 'db-small'
    instance = 'sampleMySqlService'
    # remove previous app
    app = 'actuator-api-management-sample'
    cf.delete_app(app)
    # create service
    cf.create_service(service, plan, instance)
