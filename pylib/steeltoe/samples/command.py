import os
import re
import shlex
import subprocess
import threading

import psutil

from . import dns


class Command(object):
    PROJECT_COMMANDS = ['dotnet', 'cf']
    BATCH_COMMANDS = ['gradlew', 'mvn', 'uaac']
    COUNT = 0
    COUNT_LOCK = threading.Lock()

    def __init__(self, context, command, windowed=False, log_func=None):
        """
        :type context: behave.runner.Context
        :type command: str
        :type windowed: bool
        :type log_func: func
        """
        with Command.COUNT_LOCK:
            Command.COUNT += 1
            self.command_id = Command.COUNT
        self.context = context
        self.env = self.context.env.copy()
        self.command = command
        self.sandbox_dir = context.sandbox_dir
        self.args = shlex.split(command)
        self.cwd = infer_cwd(context, self.args)
        self.args = resolve_args(context, self.args, self.cwd)
        self.windowed = windowed
        self.log_func = log_func if log_func else context.log.info
        self.proc = None
        self.rc = None
        self.stdout = None
        self.stderr = None
        self.stdout_f = None
        self.stderr_f = None
        self.stdout_path = None
        self.stderr_path = None

    def exec(self):
        popen_args = []
        if self.context.platform == 'windows':
            if self.windowed or self.args[0].split('/')[-1] in Command.BATCH_COMMANDS:
                popen_args += ['CMD', '/C']
            self.args[0] = self.args[0].replace('/', '\\')
        if self.windowed and self.context.options.use_windowed:
            if self.context.platform == 'windows':
                popen_args += ['START', '/WAIT']
            else:
                popen_args += ["xterm", "-T", "Samples: {}".format(self.command), "-geom", "120x32", "-e"]
        popen_args += self.args
        env = os.environ.copy()
        env.update(self.env)
        if self.windowed:
            stdout = None
            stderr = None
        else:
            cmd = os.path.split(self.args[0])[-1]
            self.stdout_path = os.path.join(self.sandbox_dir, '{}-{}.out'.format(self.command_id, cmd))
            self.stdout_f = open(self.stdout_path, 'w')
            stdout = self.stdout_f
            self.stderr_path = os.path.join(self.sandbox_dir, '{}-{}.err'.format(self.command_id, cmd))
            self.stderr_f = open(self.stderr_path, 'w')
            stderr = self.stderr_f
        self.log_func("command[{}] cmd: {}".format(self.command_id, ' '.join(popen_args)))
        self.log_func("command[{}] cwd: {}".format(self.command_id, self.cwd))
        self.log_func("command[{}] env: {}".format(self.command_id, self.env))
        self.proc = subprocess.Popen(popen_args, cwd=self.cwd, env=env, stdin=None, stdout=stdout, stderr=stderr)
        self.log_func("command[{}] pid: {}".format(self.command_id, self.proc.pid))

    def wait(self):
        self.rc = self.proc.wait()
        self.log_func("command[{}] rc: {}".format(self.command_id, self.rc))
        if not self.windowed:
            self._format_output('stdout')
            self._format_output('stderr')

    def run(self):
        self.exec()
        self.wait()
        if self.rc:
            raise CommandException(
                'command returned non-zero return code: {}, stderr:\n{}'.format(self.rc, self.stderr))

    def kill(self):
        try:
            parent = psutil.Process(self.proc.pid)
            self.log_func("killing pid {}".format(self.proc.pid))
            [child.kill() for child in parent.children(recursive=True)]
            parent.kill()
            self.wait()
            self.log_func('killed process with pid {}'.format(self.proc.pid))
        except psutil.NoSuchProcess:
            self.log_func('process with pid {} no longer exists'.format(self.proc.pid))

    def _format_output(self, output_name):
        """
        :type output_name: str
        """
        getattr(self, '{}_f'.format(output_name)).close()
        output = open(getattr(self, '{}_path'.format(output_name))).read()
        setattr(self, output_name, output)
        if output:
            self.log_func("command[{}] {}: ↓↓↓\n{}".format(self.command_id, output_name, output.strip()))
        else:
            self.log_func("command[{}] {}: <none>".format(self.command_id, output_name))


class CommandException(Exception):

    def __init(self, message):
        super(CommandException, self).__init__(message)


def infer_cwd(context, args):
    """
    all dotnet commands are run in project dir
    cf commands are run in project dir if they refer to a project file, else run in sandbox
    all other commands are run in sandbox
    :type context: behave.runner.Context
    :type args: list
    """
    if args[0] == 'dotnet':
        return context.project_dir
    if args[0] == 'cf':
        for flag in ['-c', '-f']:
            if flag in args:
                path = args[args.index(flag) + 1]
                if os.path.exists(os.path.join(context.project_dir, path)):
                    return context.project_dir
        return context.sandbox_dir
    return context.sandbox_dir


def resolve_args(context, args, cwd):
    """
    resolve app names, hostnames, etc
    :type context: behave.runner.Context
    :type args: list
    :type cwd: str
    """
    cmd = os.path.split(args[0])[-1]
    if cmd == 'cf':
        resolve_cf_args(context, args, cwd)
    return args


def resolve_cf_args(context, args, cwd):
    """
    :type context: behave.runner.Context
    :type args: list
    :type cwd: str
    """
    if args[1] == 'push':
        if '-f' in args:
            manifest = os.path.join(cwd, args[args.index('-f') + 1])
            with open(manifest, 'r', encoding='utf-8-sig') as f:
                doc = f.read()
            match = re.search(r'- name:\s+(\S+)', doc)
            if match:
                app = match.group(1)
                args += ['--hostname', dns.resolve_hostname(context, app)]
