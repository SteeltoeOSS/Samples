import os
import psutil
import re
import resolve
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
        self.env = self.context.env.copy()
        self.command = command
        if self.context.platform == 'windows':
            self.command = self.command.replace('/', '\\\\')
        self.args = shlex.split(command)
        self.cwd = infer_cwd(context, self.args)
        self.args = resolve_args(context, self.args, self.cwd)
        self.windowed = windowed
        self.logf = logf if logf else context.log.info

    def exec(self):
        popen_args = []
        if self.context.platform == 'windows':
            if self.windowed or self.args[0] in Command.BATCH_COMMANDS:
                popen_args += ['CMD', '/C']
        if self.windowed and self.context.options.use_windowed:
            if self.context.platform == 'windows':
                popen_args += ['START', '/WAIT']
            else:
                popen_args += ["xterm", "-T", "Samples: {}".format(self.command), "-geom", "120x32", "-e"]
        popen_args += self.args
        env = os.environ.copy()
        env.update(self.env)
        pipe = None if self.windowed else subprocess.PIPE
        self.logf("command[{}] cmd: {}".format(self.command_id, ' '.join(popen_args)))
        self.logf("command[{}] cwd: {}".format(self.command_id, self.cwd))
        self.logf("command[{}] env: {}".format(self.command_id, self.env))
        self.proc = subprocess.Popen(popen_args, cwd=self.cwd, env=env, stdin=pipe, stdout=pipe, stderr=pipe)
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

def infer_cwd(context, args):
    '''
    all dotnet commands are run in project dir
    cf commands are run in project dir if they refer to a project file, else run in sandbox
    all other commands are run in sandbox
    '''
    if args[0] == 'dotnet':
        return context.project_dir
    if args[0] == 'cf':
        for flag in ['-c', '-f']:
            if flag in args:
                path = args[ args.index(flag) + 1 ]
                if os.path.exists(os.path.join(context.project_dir, path)):
                    return context.project_dir
        return context.sandbox_dir
    return context.sandbox_dir

def resolve_args(context, args, cwd):
    '''
    resolve app names, hostnames, etc
    '''
    resolved = args.copy()
    # scope 'cf push' hostname
    if resolved[0:2] == ['cf', 'push'] and '-f' in resolved:
        manifest = os.path.join(cwd, resolved[ resolved.index('-f') + 1 ])
        with open(manifest, 'r', encoding='utf-8-sig') as f:
            doc = f.read()
        match =re.search('- name:\s+(\S+)', doc)
        if match:
            app = match.group(1)
            resolved += ['--hostname', resolve.hostname(context, app)]
    # scope 'cf cups' embedded URLs in payload
    if resolved[0:2] == ['cf', 'cups'] and '-p' in resolved:
        payload_idx = resolved.index('-p') + 1
        payload = resolved[payload_idx]
        match = re.search('"(uaa://[^"]+)', payload)
        if match:
            url = match.group(1)
            resolved[payload_idx] = payload.replace(url, resolve.url(context, url))
    # scope 'uaac target' URL
    if resolved[0:2] == ['uaac', 'target']:
        resolved[2] = resolve.url(context, resolved[2])
    # scope 'uaac client' redirect URI
    if resolved[0:2] == ['uaac', 'client'] and '--redirect_uri' in resolved:
        uri_idx = resolved.index('--redirect_uri') + 1
        uri = resolved[uri_idx]
        resolved[uri_idx] = resolve.url(context, uri)
    # scope gradle arguments -Dapp= and -Dapp-domain=
    for i in range(len(resolved)):
        if resolved[i].startswith('-Dapp='):
            app = resolved[i].split('=', 1)[1]
            resolved[i] = '-Dapp={}'.format(resolve.hostname(context, app))
        if resolved[i].startswith('-Dapp-domain='):
            domain = resolved[i].split('=', 1)[1]
            resolved[i] = '-Dapp-domain={}'.format(resolve.domainname(context, domain))
    return resolved
