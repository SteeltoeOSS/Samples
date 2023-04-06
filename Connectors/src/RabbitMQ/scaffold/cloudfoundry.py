from pysteel import cloudfoundry


def setup(context):
    """
    :type context: behave.runner.Context
    """
    cf = cloudfoundry.CloudFoundry(context)
    # remove previous app
    app = 'rabbitmq-connector'
    cf.delete_app(app)
    # create service
    service = 'p.rabbitmq'
    plan = 'single-node'
    instance = 'myRabbitMQService'
    cf.create_service(service, plan, instance)
