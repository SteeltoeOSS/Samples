import os

from pysteel import fs


def setup(context):
    """
    :type context: behave.runner.Context
    """
    repo = os.path.join(context.project_dir, 'spring-cloud-config')
    fs.deltree(repo)
