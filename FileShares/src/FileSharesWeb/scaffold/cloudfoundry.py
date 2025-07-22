from pysteel import cloudfoundry
import os;

def setup(context):
    """
    :type context: behave.runner.Context
    """
    cf = cloudfoundry.CloudFoundry(context)
    # remove previous app
    app = 'fileshares-sample'
    cf.delete_app(app)
    # create service
    service = 'credhub'
    plan = 'default'
    instance = 'sampleNetworkShare'
    args = ['-c', '{}'.format(os.getenv("CUSTOM_VARIABLE"))]
    cf.create_service(service, plan, instance, args, True)
