import re


def resolve_hostname(context, name):
    """
    if simple name, return name + space
    if FQDN, then return name unless the domain is x.y.x
    if domain is x.y.z, return the hostname per the CloudFoundry route
    :type context: behave.runner.Context
    :type name: str
    """
    context.log.info('resolving hostname for {}'.format(name))
    if re.search(r'^localhost(:\d+)$', name):
        return name
    if '.' in name:
        host, domain = name.split('.', 1)
    else:
        host, domain = name, None
    host = '{}-{}'.format(host, context.cf_space.replace('_', ''))
    context.log.info('host -> {}'.format(host))
    if domain:
        domain = resolve_domainname(context, domain)
        context.log.info('domain -> {}'.format(domain))
    resolved = '{}.{}'.format(host, domain) if domain else host
    context.log.info('resolved name -> {}'.format(resolved))
    return resolved


def resolve_domainname(context, name):
    """
    :type context: behave.runner.Context
    :type name: str
    """
    resolved = name if name != 'x.y.z' else 'apps.pcfone.io'
    context.log.info('resolved domain -> {}'.format(resolved))
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
