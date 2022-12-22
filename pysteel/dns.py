import re
from pysteel.cloudfoundry import CloudFoundry


def resolve_hostname(context, host):
    """
    :type context: behave.runner.Context
    :type host: str
    """
    context.log.info('resolving hostname for {}'.format(host))
    resolved = host if re.search(r'^localhost(:\d+)$', host) else CloudFoundry(context).get_app_route(host)
    context.log.info('resolved name -> {}'.format(resolved))
    return resolved


def resolve_url(context, url):
    """
    return the url with a resolved hostname
    :type context: behave.runner.Context
    :type url: str
    """
    pseudo = re.match(r'(\S+)://([^/]+)(.*)', url)
    if not pseudo:
        return url
    scheme = pseudo.group(1)
    fqdn = pseudo.group(2)
    path = pseudo.group(3)
    return '{}://{}{}'.format(scheme, resolve_hostname(context, fqdn), path)
