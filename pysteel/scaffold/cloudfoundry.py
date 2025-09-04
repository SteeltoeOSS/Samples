import os
import re
from urllib.parse import urlparse

from pysteel import cloudfoundry


def setup(context, scenario):
    """
    set up scenario scaffolding for Cloud Foundry samples
    :type context: behave.runner.Context
    :type scenario: behave.model.Scenario
    """
    cf = cloudfoundry.CloudFoundry(context)

    # CloudFoundry options
    creds = [context.options.cf.apiurl, context.options.cf.username, context.options.cf.password,
             context.options.cf.org]
    if [cred for cred in creds if cred]:
        if None in creds:
            raise Exception(
                'if setting CloudFoundry credentials, all of cf_apiurl, cf_username, cf_password, cf_org must be set')
        context.env['CF_HOME'] = context.sandbox_dir
        cf.login(
            context.options.cf.apiurl,
            context.options.cf.username,
            context.options.cf.password,
            context.options.cf.org,
            'development'
        )
    else:
        context.log.info('CloudFoundry credentials not provided, assuming already logged in')
    context.cf_space = context.options.cf.space
    if not context.cf_space:
        feature = context.feature.name.replace(' ', '-')
        # Strip out everything after the first '(' (including the parens)
        scenarioName = scenario.name.split('(')[0].strip()
        if not scenarioName:
            raise ValueError(f"Cannot extract base name from scenario: '{scenario.name}'")

        scenarioName = scenarioName.replace(' ', '-')
        context.cf_space = f"sample-local_{feature}-{scenarioName}"
    context.log.info('CloudFoundry space -> {}'.format(context.cf_space))
    context.cf_domain = context.options.cf.domain
    if not context.cf_domain:
        context.cf_domain = cf.get_api_endpoint()
        context.log.info('guessing CloudFoundry domain')
        endpoint = cf.get_api_endpoint()
        context.cf_domain = urlparse(endpoint).hostname.replace('api.run', 'apps')
    context.log.info('CloudFoundry domain -> {}'.format(context.cf_domain))

    # CloudFoundry sandbox
    cf.create_space(context.cf_space)
    cf.target_space(context.cf_space)

def teardown(context, scenario):
    """
    :type context: behave.runner.Context
    """
    cf = cloudfoundry.CloudFoundry(context)
    if context.cf_space:
        cf.delete_space(context.cf_space)
    else:
        context.log.warn("No cf_space defined in context; skipping space deletion")
