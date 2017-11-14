import os
import psutil
import shlex
import subprocess
import threading

class Command(object):

    PROJECT_COMMANDS = ['dotnet', 'cf']
    BATCH_COMMANDS = ['mvn']
    COUNT = 0
    COUNT_LOCK = threading.Lock()

    def __init__(self, context, command, windowed=False, logf=None):
        with Command.COUNT_LOCK:
            Command.COUNT += 1
            self.command_id = Command.COUNT
        self.context = context
        self.command = command
        if self.context.platform == 'windows':
            self.command = self.command.replace('/', '\\\\')
        if self.command.startswith('cf push'):
            self.command += ' --hostname foo-{}'.format(context.cf_space)
        self.windowed = windowed
        self.logf = logf if logf else context.log.info

    def exec(self):
        args = shlex.split(self.command)
        popen_args = []
        if self.context.platform == 'windows':
            if self.windowed or args[0] in Command.BATCH_COMMANDS:
                popen_args += ['CMD', '/C']
        if self.windowed and self.context.options.use_windowed:
            if self.context.platform == 'windows':
                popen_args += ['START', '/WAIT']
            else:
                popen_args += ["xterm", "-e"]
        popen_args += args
        cwd = self.context.project_dir if args[0] in Command.PROJECT_COMMANDS else self.context.sandbox_dir
        env = os.environ.copy()
        env.update(self.context.env)
        pipe = None if self.windowed else subprocess.PIPE
        self.logf("command[{}] cmd: {}".format(self.command_id, ' '.join(popen_args)))
        self.logf("command[{}] cwd: {}".format(self.command_id, cwd))
        self.logf("command[{}] env: {}".format(self.command_id, self.context.env))
        self.proc = subprocess.Popen(popen_args, cwd=cwd, env=env, stdin=pipe, stdout=pipe, stderr=pipe)
        self.logf("command[{}] pid: {}".format(self.command_id, self.proc.pid))

    def wait(self):
        self.rc = self.proc.wait()
        self.logf("command[{}] rc: {}".format(self.command_id, self.rc))
        if self.proc.stdout:
            self.stdout = self.proc.stdout.read().decode('utf-8')
            self.logf("command[{}] stdout:\n | {}".format(self.command_id, self.stdout.strip().replace('\n', '\n | ')))
        if self.proc.stderr:
            self.stderr = self.proc.stderr.read().decode('utf-8')
            self.logf("command[{}] stderr:\n | {}".format(self.command_id, self.stderr.strip().replace('\n', '\n | ')))

    def run(self):
        self.exec()
        self.wait()
        if self.rc:
            raise CommandException('command returned non-zero return code: {}'.format(self.rc))

    def kill(self):
        try:
            parent = psutil.Process(self.proc.pid)
            self.logf("killing pid {}".format(self.proc.pid))
            [ child.kill() for child in parent.children(recursive=True) ]
            parent.kill()
            self.wait()
            self.logf('killed process with pid {}'.format(self.proc.pid))
        except psutil.NoSuchProcess:
            self.logf('process with pid {} no longer exists'.format(self.proc.pid))

class CommandException(Exception):

    pass
