import os
import psutil
import re
import resolve
import shlex
import subprocess
import threading

class Command(object):

    PROJECT_COMMANDS = ['dotnet', 'cf']
    BATCH_COMMANDS = ['mvn', 'uaac']
    COUNT = 0
    COUNT_LOCK = threading.Lock()

    def __init__(self, context, command, windowed=False, logf=None):
        with Command.COUNT_LOCK:
            Command.COUNT += 1
            self.command_id = Command.COUNT
        self.context = context
        self.env = self.context.env.copy()
        self.command = command
        self.sandbox_dir = context.sandbox_dir
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
        self.logf("command[{}] cmd: {}".format(self.command_id, ' '.join(popen_args)))
        self.logf("command[{}] cwd: {}".format(self.command_id, self.cwd))
        self.logf("command[{}] env: {}".format(self.command_id, self.env))
        self.proc = subprocess.Popen(popen_args, cwd=self.cwd, env=env, stdin=None, stdout=stdout, stderr=stderr)
        self.logf("command[{}] pid: {}".format(self.command_id, self.proc.pid))

    def wait(self):
        self.rc = self.proc.wait()
        self.logf("command[{}] rc: {}".format(self.command_id, self.rc))
        if not self.windowed:
            self.stdout_f.close()
            self.stdout = open(self.stdout_path).read()
            self.logf("command[{}] stdout:\n | {}".format(self.command_id, self.stdout.strip().replace('\n', '\n | ')))
            self.stderr_f.close()
            self.stderr = open(self.stderr_path).read()
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
    arg_resolvers = {
            'cf': resolve_cf_args,
            'gradle': resolve_gradle_args,
            'gradlew': resolve_gradle_args,
            'uaac': resolve_uaac_args,
            }
    cmd = os.path.split(args[0])[-1]
    try:
        arg_resolvers[cmd](context, args, cwd)
    except KeyError:
        pass
    return args

def resolve_cf_args(context, args, cwd):
    if args[1] == 'push':
        if '-f' in args:
            manifest = os.path.join(cwd, args[args.index('-f') + 1 ])
            with open(manifest, 'r', encoding='utf-8-sig') as f:
                doc = f.read()
            match =re.search('- name:\s+(\S+)', doc)
            if match:
                app = match.group(1)
                args += ['--hostname', resolve.hostname(context, app)]
    elif args[1] == 'cups':
        if '-p' in args:
            creds_idx = args.index('-p') + 1
            creds = args[creds_idx]
            match = re.search('"(uaa://[^"]+)', creds)
            if match:
                url = match.group(1)
                args[creds_idx] = creds.replace(url, resolve.url(context, url))

def resolve_gradle_args(context, args, cwd):
    args.append('-Dorg.gradle.daemon=false')
    for i in range(len(args)):
        if args[i].startswith('-Dapp='):
            host = args[i].split('=', 1)[1]
            args[i] = '-Dapp={}'.format(resolve.hostname(context, host))
        if args[i].startswith('-Dapp-domain='):
            domain = args[i].split('=', 1)[1]
            args[i] = '-Dapp-domain={}'.format(resolve.domainname(context, domain))

def resolve_uaac_args(context, args, cwd):
    if args[1] == 'target':
        args[2] = resolve.url(context, args[2])
    elif args[1] == 'client':
        if '--redirect_uri' in args:
            uri_idx = args.index('--redirect_uri') + 1
            uri = args[uri_idx]
            args[uri_idx] = resolve.url(context, uri)
