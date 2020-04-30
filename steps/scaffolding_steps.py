import importlib
import os
import sys

from behave import *


@given("your Cloud Foundry scaffolding has been setup")
def step_impl(context):
    """
    Delegates scaffolding setup to the function 'setup' defined in the module 'cloudfoundry_scaffolding'.
    :type context: behave.runner.Context
    """
    module_name = 'cloudfoundry_scaffolding'
    module_dir = os.path.join(context.samples_dir, os.path.dirname(context.feature.filename))
    sys.path.append(os.path.join(module_dir))
    try:
        module = importlib.import_module(module_name)
    except ModuleNotFoundError:
        raise Exception('scaffolding module does not exist: {}'.format(module_name))
    sys.path.pop()
    func_name = 'setup'
    try:
        setup = getattr(module, func_name)
    except AttributeError:
        raise Exception('"{}" "{}" function does not exist'.format(module_name, func_name))
    context.log.info('delegating scaffolding setup deployment to "{}.{}"'.format(module_name, func_name))
    setup(context)
