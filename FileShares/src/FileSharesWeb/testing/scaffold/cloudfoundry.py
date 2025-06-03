import json, os
from pysteel import cloudfoundry

def setup(context):
    """
    :type context: behave.runner.Context
    """
    location = os.environ.get('FILESHARE_LOCATION')
    username = os.environ.get('FILESHARE_USERNAME')
    password = os.environ.get('FILESHARE_PASSWORD')

    missing = [name for name, value in {
        "FILESHARE_LOCATION": location,
        "FILESHARE_USERNAME": username,
        "FILESHARE_PASSWORD": password
    }.items() if not value]

    if missing:
        raise RuntimeError(f"Missing required environment variables: {', '.join(missing)}")

    service_config = {
        "location": location,
        "username": username,
        "password": password
    }

    cf = cloudfoundry.CloudFoundry(context)
    app = 'fileshares-sample'
    cf.delete_app(app)

    service = 'credhub'
    plan = 'default'
    instance = 'sampleNetworkShare'
    args = ['-c', json.dumps(service_config)]
    cf.create_service(service, plan, instance, args, True)
