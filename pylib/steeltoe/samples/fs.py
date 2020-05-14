import os
import shutil
import stat


def deltree(dir):
    """
    :type dir: str
    """
    if os.path.exists(dir):
        def remove_readonly(func, path, info):
            os.chmod(path, stat.S_IWRITE)
            func(path)
        shutil.rmtree(dir, onerror=remove_readonly)
