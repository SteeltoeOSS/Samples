import os
import shutil
import stat


def deltree(dirpath):
    """
    :type dirpath: str
    """
    if os.path.exists(dirpath):
        def remove_readonly(func, path, _):
            os.chmod(path, stat.S_IWRITE)
            func(path)
        shutil.rmtree(dirpath, onerror=remove_readonly)
