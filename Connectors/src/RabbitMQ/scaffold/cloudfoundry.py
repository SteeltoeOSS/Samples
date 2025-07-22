from pysteel import cloudfoundry


def setup(context):
    """
    :type context: behave.runner.Context
    """
    cf = cloudfoundry.CloudFoundry(context)
    # remove previous app
    app = 'rabbitmq-connector-sample'
    cf.delete_app(app)
    # create service
    service = 'p.rabbitmq'
    plan = 'rmq-single-node'
    instance = 'sampleRabbitMQService'
    cf.create_service(service, plan, instance)
