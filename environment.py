import logging
import os
import shutil
import stat
import sure
import sys

PLATFORM_SUPPORT = {
        'netcoreapp2.0': ['windows', 'linux', 'osx'],
        'net461': ['windows'],
        }

def before_all(context):
    '''
    behave hook called before running test suite
    '''
    context.repo_dir = os.getcwd()
    setup_logging(context)
    context.log.info("TEST START")
    setup_options(context)
    context.log.info("repo directory: {}".format(context.repo_dir))
    setup_platform(context)
    setup_output(context)

def after_all(context):
    '''
    behave hook called after running test suite
    '''
    context.log.info("TEST END")

def before_feature(context, feature):
    '''
    behave hook called before running testfeaturescenario
    '''
    context.log.info('[===] {}'.format(feature.name))
    context.project_dir = os.path.dirname(os.path.join(context.repo_dir, feature.filename))
    context.log.info('project directory: {}'.format(context.project_dir))

def before_scenario(context, scenario):
    '''
    behave hook called before running test scenario
    '''
    context.log.info('[---] {}'.format(scenario.name))
    for tag in scenario.tags + scenario.feature.tags:
        if tag in PLATFORM_SUPPORT:
            if context.platform not in PLATFORM_SUPPORT[tag]:
                context.log.info("{} not supported on {}".format(tag, context.platform))
                scenario.mark_skipped()
                return
    sandbox_name = scenario.name.translate({ord(ch): None for ch in ' -'})
    context.sandbox_dir = os.path.join(context.sandboxes_dir, sandbox_name)
    context.log.info('sandbox directory: {}'.format(context.sandbox_dir))
    os.makedirs(context.sandbox_dir)
    context.cleanups = []
    setup_env(context, scenario)
    context.cf_space = context.options.cf_space

def setup_env(context, scenario):
    context.env = {}
    context.env['CF_HOME'] = context.sandbox_dir
    context.env['CF_COLOR'] = 'false'


def after_scenario(context, scenario):
    '''
    behave hook called after running test scenario
    '''
    if context.options.do_cleanup:
        context.log.info('cleaning up scenario')
        if hasattr(context, 'cleanups'):
            while context.cleanups:
                context.cleanups.pop()()
    else:
        context.log.info('skipping scenario cleanup')

def before_step(context, step):
    '''
    behave hook called before running test step
    '''
    context.log.info('[...] {}'.format(step.name))

def after_step(context, step):
    '''
    behave hook called after running test step
    '''
    if context.options.debug_on_error:
        import ipdb
        ipdb.post_mortem(step.exc_traceback)

def setup_options(context):
    '''
    setup/configure user-supplied options, or those dictated by the environment
    '''
    user_opts = os.path.join(context.repo_dir, "user.ini")
    if os.path.exists(user_opts):
        import configparser
        parser = configparser.SafeConfigParser()
        parser.read(user_opts)
        section = context.config.userdata.get("config_section", "behave.userdata")
        if parser.has_section(section):
            options = parser.items(section)
            context.log.info("user options {}".format(options))
            context.config.userdata.update(options)
        else:
            context.log.info("user options file found but does not contain section [{}]".format(section))
    context.options = type("", (), {})()
    context.options.output_dir = context.config.userdata.get('output')
    context.log.info("option: output directory -> {}".format(context.options.output_dir))
    context.options.use_windowed = context.config.userdata.getbool('windowed')
    context.options.do_cleanup = context.config.userdata.getbool('cleanup')
    context.log.info("option: cleanup? -> {}".format(context.options.do_cleanup))
    context.log.info("option: use windowed? -> {}".format(context.options.use_windowed))
    context.options.max_attempts = context.config.userdata.getint('max_attempts')
    context.log.info("option: max attempts -> {}".format(context.options.max_attempts))
    context.options.debug_on_error = context.config.userdata.getbool('debug_on_error')
    context.log.info("option: debug on error? -> {}".format(context.options.debug_on_error))
    context.options.cf_apiurl = context.config.userdata.get('cf_apiurl')
    assert context.options.cf_apiurl, 'CloudFoundry API URL not set (option: cf_apiurl)'
    context.log.info("option: CloudFoundry API URL -> {}".format(context.options.cf_apiurl))
    context.options.cf_username = context.config.userdata.get('cf_username')
    assert context.options.cf_username, 'CloudFoundry username not set (option: cf_username)'
    context.log.info("option: CloudFoundry username -> {}".format(context.options.cf_username))
    context.options.cf_password = context.config.userdata.get('cf_password')
    assert context.options.cf_password, 'CloudFoundry password not set (option: cf_password)'
    context.log.info("option: CloudFoundry password -> *")
    context.options.cf_org = context.config.userdata.get('cf_org')
    assert context.options.cf_org, 'CloudFoundry org not set (option: cf_org)'
    context.log.info("option: CloudFoundry org -> {}".format(context.options.cf_org))
    context.options.cf_domain = context.config.userdata.get('cf_domain')
    assert context.options.cf_domain, 'CloudFoundry domain not set (option: cf_domain)'
    context.log.info("option: CloudFoundry domain -> {}".format(context.options.cf_domain))
    context.options.cf_space = context.config.userdata.get('cf_space')
    context.log.info("option: CloudFoundry space -> {}".format(context.options.cf_space))
    context.options.cf_max_attempts = context.config.userdata.getint('cf_max_attempts')
    context.log.info("option: CloudFoundry max attempts -> {}".format(context.options.cf_max_attempts))

def setup_logging(context):
    '''
    load logging config
    '''
    context.config.setup_logging(configfile=os.path.join(context.repo_dir, 'logging.cfg'))
    context.log = logging.getLogger('pivotal')

def setup_platform(context):
    '''
    determine the underlying platform and whether it's supported
    '''
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
    '''
    setup test output directories
    '''
    context.log.info("output directory: {}".format(context.options.output_dir))
    context.options.output_dir = os.path.abspath(context.options.output_dir)
    if os.path.exists(context.options.output_dir):
        def remove_readonly(func, path, info):
            os.chmod(path, stat.S_IWRITE)
            func(path)
        shutil.rmtree(context.options.output_dir, onerror=remove_readonly)
    context.sandboxes_dir = os.path.join(context.options.output_dir, 'sandboxes')
    context.config.junit_directory = os.path.join(context.options.output_dir, 'reports')

