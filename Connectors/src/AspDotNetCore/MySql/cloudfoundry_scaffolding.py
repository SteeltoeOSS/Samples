from steeltoe.samples import cloudfoundry


def setup(context):
    """
    :type context: behave.runner.Context
    """
    cf = cloudfoundry.CloudFoundry(context)
    # remove previous app
    app = 'mysql-connector'
    cf.delete_app(app)
    # create service
    service = 'p.mysql'
    plan = 'db-small'
    instance = 'myMySqlService'
    cf.create_service(service, plan, instance)
