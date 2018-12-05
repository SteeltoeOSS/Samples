import command
import re

def hostname(context, name):
    '''
    if simple name, return name + space
    if FQDN, then return name unless the domain is x.y.x
    if domain is x.y.z, return the hostname per the CloudFoundry route
    '''
    context.log.info('resolving hostname for {}'.format(name))
    if re.search('^localhost(:\d+)$', name):
        return name
    if '.' in name:
        host, domain = name.split('.', 1)
    else:
        host, domain = name, None
    host = '{}-{}'.format(host, context.cf_space)
    context.log.info('host -> {}'.format(host))
    if domain:
        domain = domainname(context, domain)
        context.log.info('domain -> {}'.format(domain))
    resolved = '{}.{}'.format(host, domain) if domain else host
    context.log.info('resolved name -> {}'.format(resolved))
    return resolved

def domainname(context, name):
    resolved = name if name != 'x.y.z' else 'apps.pcfone.io'
    context.log.info('resolved domain -> {}'.format(resolved))
    return resolved


def url(context, url):
    '''
    return the url with a resolved hostname
    '''
    pseudo = re.match('(\S+)://([^/]+)(.*)', url)
    if not pseudo:
        return url
    scheme = pseudo.group(1)
    fqdn = pseudo.group(2)
    path = pseudo.group(3)
    return '{}://{}{}'.format(scheme, hostname(context, fqdn), path)
