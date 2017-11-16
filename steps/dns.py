import command
import re

def hostname(context, name):
    '''
    if simple name, return name + space
    if FQDN, then return name unless the domain is x.y.x
    if domain is x.y.z, return the hostname per the CloudFoundry route
    '''
    if '.' in name:
        host, domain = name.split('.', 1)
        domain = domainname(context, domain)
    else:
        host, domain = name, None
    return '{}-{}{}{}'.format(host, context.cf_space, '.' if domain else '', domain if domain else '')

def domainname(context, name):
    if name != 'x.y.z':
        return name
    return context.options.cf_domain

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
