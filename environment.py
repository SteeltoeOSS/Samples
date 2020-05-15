import importlib
import logging
import os
import re
import sys
from urllib.parse import urlparse

import behave
import sure
from pysteel import cloudfoundry
from pysteel import fs


#
# hooks
#

def before_all(context):
    """
    behave hook called before running test features
    :type context: behave.runner.Context
    """
    context.samples_dir = os.getcwd()
    context.config.setup_logging(configfile=os.path.join(context.samples_dir, 'logging.ini'))
    context.log = logging.getLogger('pivotal')
    context.log.info("Steeltoe Samples test suite")
    context.log.info("samples directory: {}".format(context.samples_dir))
    setup_options(context)
    setup_output(context)
    setup_platform(context)
    context.counters = {'failed_scenarios': 0, 'failed_features': 0}


def after_all(context):
    """
    behave hook called after running test features
    :type context: behave.runner.Context
    """
    context.log.info("failures:")
    context.log.info("    features : {}".format(context.counters['failed_features']))
    context.log.info("    scenarios: {}".format(context.counters['failed_scenarios']))


def before_feature(context, feature):
    """
    behave hook called before running test feature
    :type context: behave.runner.Context
    :type feature: behave.model.Feature
    """
    context.log.info('[===] feature starting: "{}"'.format(feature.name))
    context.project_dir = os.path.dirname(os.path.join(context.samples_dir, feature.filename))
    context.log.info('project directory: {}'.format(context.project_dir))


def after_feature(context, feature):
    """
    behave hook called before running test feature
    :type context: behave.runner.Context
    :type feature: behave.model.Feature
    """
    if feature.status == behave.model.Status.failed:
        context.counters['failed_features'] += 1
    context.log.info('[===] feature completed: "{}" [{}]'.format(feature.name, feature.status))


def before_scenario(context, scenario):
    """
    behave hook called before running test scenario
    :type context: behave.runner.Context
    :type scenario: behave.model.Scenario
    """
    context.log.info('[---] scenario starting: "{}"'.format(scenario.name))
    sandbox_name = scenario.name.translate({ord(ch): ' ' for ch in '/'})
    context.sandbox_dir = os.path.join(context.sandboxes_dir, sandbox_name)
    context.log.info('sandbox directory: {}'.format(context.sandbox_dir))
    os.makedirs(context.sandbox_dir)
    context.cleanups = []
    setup_env(context)
    tags = scenario.tags + scenario.feature.tags
    for scaffold in list(filter(lambda t: t.endswith('_scaffold'), tags)):
        setup_scaffold(context, scenario, scaffold)


def after_scenario(context, scenario):
    """
    behave hook called after running test scenario
    :type context: behave.runner.Context
    :type scenario: behave.model.Scenario
    """
    if scenario.status == behave.model.Status.failed:
        context.counters['failed_scenarios'] += 1
    if context.options.do_cleanup:
        context.log.info('cleaning up test scenario')
        if hasattr(context, 'cleanups'):
            while context.cleanups:
                context.cleanups.pop()()
    else:
        context.log.info('skipping scenario cleanup')
    context.log.info('[---] scenario completed: "{}" [{}]'.format(scenario.name, scenario.status))


def before_step(context, step):
    """
    behave hook called before running test step
    :type context: behave.runner.Context
    :type step: behave.model.Step
    """
    context.log.info('[...] step starting: "{}"'.format(step.name))


def after_step(context, step):
    """
    behave hook called after running test step
    :type context: behave.runner.Context
    :type step: behave.model.Step
    """
    if context.options.debug_on_error:
        import ipdb
        ipdb.post_mortem(step.exc_traceback)
    context.log.info('[...] step completed: "{}" [{}]'.format(step.name, step.status))


#
# fixture setup helpers
#

def setup_options(context):
    """
    setup/configure user-supplied options, or those dictated by the environment
    :type context: behave.runner.Context
    """
    user_opts = os.path.join(context.samples_dir, "user.ini")
    if os.path.exists(user_opts):
        import configparser
        parser = configparser.SafeConfigParser()
        parser.read(user_opts)
        section = context.config.userdata.get("config_section", "behave.userdata")
        if parser.has_section(section):
            options = parser.items(section)
            context.config.userdata.update(options)
        else:
            context.log.info("user options file found but does not contain section [{}]".format(section))
    context.options = type("", (), {})()
    context.options.output_dir = context.config.userdata.get('output')
    context.log.info("option: output directory -> {}".format(context.options.output_dir))
    try:
        context.options.use_windowed = context.config.userdata.getbool('windowed')
    except ValueError as e:
        context.log.error("invalid config option: windowed -> {}".format(context.config.userdata.get('windowed')))
        raise e
    context.log.info("option: windowed? -> {}".format(context.options.use_windowed))
    try:
        context.options.do_cleanup = context.config.userdata.getbool('cleanup')
    except ValueError as e:
        context.log.error("invalid config option: cleanup -> {}".format(context.config.userdata.get('cleanup')))
        raise e
    context.log.info("option: cleanup? -> {}".format(context.options.do_cleanup))
    try:
        context.options.max_attempts = context.config.userdata.getint('max_attempts')
    except ValueError as e:
        context.log.error("invalid config option: max_attempts -> {}".format(
            context.config.userdata.get('max_attempts')))
        raise e
    context.log.info("option: max attempts -> {}".format(context.options.max_attempts))
    try:
        context.options.debug_on_error = context.config.userdata.getbool('debug_on_error')
    except ValueError as e:
        context.log.error("invalid config option: debug_on_error -> {}".format(
            context.config.userdata.get('debug_on_error')))
        raise e
    context.log.info("option: debug on error? -> {}".format(context.options.debug_on_error))
    context.options.cf = type("", (), {})()
    context.options.cf.apiurl = context.config.userdata.get('cf_apiurl')
    context.log.info("option: CloudFoundry API URL -> {}".format(context.options.cf.apiurl))
    context.options.cf.username = context.config.userdata.get('cf_username')
    context.log.info("option: CloudFoundry username -> {}".format(context.options.cf.username))
    context.options.cf.password = context.config.userdata.get('cf_password')
    context.log.info("option: CloudFoundry password -> {}".format('*' if context.options.cf.password else None))
    context.options.cf.org = context.config.userdata.get('cf_org')
    context.log.info("option: CloudFoundry org -> {}".format(context.options.cf.org))
    context.options.cf.domain = context.config.userdata.get('cf_domain')
    context.log.info("option: CloudFoundry domain -> {}".format(context.options.cf.domain))
    context.options.cf.space = context.config.userdata.get('cf_space')
    context.log.info("option: CloudFoundry space -> {}".format(context.options.cf.space))
    try:
        context.options.cf.max_attempts = context.config.userdata.getint('cf_max_attempts')
    except ValueError as e:
        context.log.error(
            "invalid config option: cf_max_attempts -> {}".format(context.config.userdata.get('cf_max_attempts')))
        raise e
    context.log.info("option: CloudFoundry max attempts -> {}".format(context.options.cf.max_attempts))


def setup_platform(context):
    """
    determine the underlying platform and whether it's supported
    :type context: behave.runner.Context
    """
    try:
        context.platform = {
            'darwin': 'osx',
            'linux': 'linux',
            'win32': 'windows',
        }[sys.platform]
    except KeyError:
        assert False, "unknown platform: {}".format(sys.platform)
    context.log.info("platform: {}".format(context.platform))


def setup_output(context):
    """
    setup test output directories
    :type context: behave.runner.Context
    """
    context.log.info("output directory: {}".format(context.options.output_dir))
    context.options.output_dir = os.path.abspath(context.options.output_dir)
    fs.deltree(context.options.output_dir)
    context.sandboxes_dir = os.path.join(context.options.output_dir, 'sandboxes')
    context.config.junit_directory = os.path.join(context.options.output_dir, 'reports')


def setup_env(context):
    """
    set up command execution environment
    :type context: behave.runner.Context
    """
    context.env = {}
    if context.platform == 'windows':
        context.env['CF_COLOR'] = 'false'


def setup_scaffold(context, scenario, scaffold):
    """
    set up scenario scaffolding
    :type context: behave.runner.Context
    :type scenario: behave.model.Scenario
    :type scaffold: str
    """
    # general scaffolding
    eval('setup_{}(context, scenario)'.format(scaffold))
    # sample scaffolding
    sample_module_dir = os.path.join(context.samples_dir, os.path.dirname(context.feature.filename))
    sys.path.append(os.path.join(sample_module_dir))
    try:
        sample_module = importlib.import_module(scaffold)
    except ModuleNotFoundError:
        raise Exception('sample scaffolding module does not exist: {}'.format(scaffold))
    finally:
        sys.path.pop()
    sample_setup_func_name = 'setup'
    try:
        sample_setup_func = getattr(sample_module, sample_setup_func_name)
    except AttributeError:
        raise Exception('sample scaffolding "{}" function does not exist'.format(scaffold, sample_setup_func_name))
    context.log.info(
        'delegating sample scaffolding setup deployment to "{}.{}"'.format(scaffold, sample_setup_func_name))
    sample_setup_func(context)


def setup_local_scaffold(context, scenario):
    """
    set up scenario scaffolding
    :type context: behave.runner.Context
    :type scenario: behave.model.Scenario
    """
    pass


def setup_cloudfoundry_scaffold(context, scenario):
    """
    set up scenario scaffolding
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
        tld = re.split('/|\\\\', scenario.filename)[0]
        feature_file = os.path.basename(scenario.filename)
        context.cf_space = "{}-{}-{}".format(
            tld,
            os.path.splitext(feature_file)[0],
            context.platform
        ).lower()
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
