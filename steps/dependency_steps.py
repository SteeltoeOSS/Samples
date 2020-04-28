import importlib
import os
import sys

from behave import *


@given("your Cloud Foundry services have been deployed")
def step_impl(context):
    """
    Delegates deployment to the function 'deploy' defined in the module '{Feature}Deps.py'.
    :type context: behave.runner.Context
    """
    module_name = '{}_CloudFoundryDeps'.format(os.path.splitext(os.path.basename(context.feature.filename))[0])
    module_dir = os.path.join(context.samples_dir, os.path.dirname(context.feature.filename))
    sys.path.append(os.path.join(module_dir))
    try:
        module = importlib.import_module(module_name)
    except ModuleNotFoundError:
        raise Exception("dependency delegation module does not exist: {}".format(module_name))
    sys.path.pop()
    func_name = 'deploy'
    try:
        deploy = getattr(module, func_name)
    except AttributeError:
        raise Exception("dependency delegation function does not exist: {}.{}".format(module_name, func_name))
    context.log.info("delegating dependency deployment to {}.{}".format(module_name, func_name))
    deploy(context)
