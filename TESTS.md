# Steeltoe Sample Applications Tests

## Pre-Requisites

Running the Samples tests requires Python 3 and its associated package installer `pip`.
It's recommended, but not required, that `virtualenv` is used to manage the Python 3 packages.

### Python 3

#### Windows

[Download][pydown] and install Python 3. `pip` is included in the distribution.

Set `PYTHON_HOME` to your installation directory.

Add `%PYTHON_HOME%` and `%PYTHON_HOME%\Scripts` to your `PATH`.

#### Ubuntu

Install Python 3 and `pip`.

```
$ sudo apt install python3 python3-pip
```

#### OS X

Install [`brew`][brew] if needed.

Install Python 3. `pip` is included in the distribution.

```
$ brew install python3
```

### `virtualenv`

#### Windows

Install `virtualenv` using `pip`.

```
$ pip install virtualenv
```

#### Ubuntu

Install `virtualenv` using `pip3`.

```
$ sudo pip3 install virtualenv
```

#### OS X

Install `virtualenv` using `pip3`.

```
$ pip3 install virtualenv
```

## Setup

### Python Environment

Change to the root directory of the Sample repo.
Create a virtual Python 3 environment for the project.
In these instructions, the virtual environment name is `pyenv`, however the name is arbitrary.

#### Windows

```
$ virtualenv pyenv
```

#### Ubuntu, OS X

```
$ virtualenv pyenv --python=python3
```

### Activate the Python 3 virtual environment

Activating the Python 3 virtual environment sets up your shell's path so that it can find the needed packages and tools.  You only need to activate once per shell.

#### Windows

```
$ pyenv\Scripts\activate
```

#### Ubuntu, OS X

```
$ source pyenv/bin/activate
```

Install the Python 3 packages needed to run the Samples project tests.

#### Windows, Ubuntu, OS X

```
$ pip install -r pyenv.pkgs
```

### CloudFoundry Credentials

Testing samples that use CloudFoundry need credentials to access a CloudFoundry instance.

_**Note**: There must be a space named `development` associated with the credentials.
Nothing is created in the `development` space; it's needed solely for the login process._

Create a DOS INI file named `user.ini` in the repository root.

In the file, add a section named `behave.userdata`.

In that section, configure the CloudFoundry credentials using the following attributes:
`cf_apiurl`, `cf_username`, `cf_password`, `cf_org`, `cf_domain`.

_Example: CloudFoundry credentials_
```
[behave.userdata]
cf_apiurl = api.mypcf
cf_username = myusername
cf_password = mypassword
cf_org = myorg
cf_domain = my.domain
```

## Run

_Note: it's assumed your Python 3 virtual environment is activated._

Run all the tests.

```
$ behave
```

Run tests for a specific set of samples.

_Example: Run tests for simple CloudFoundry configuration._
```
$ behave Configuration/src/AspDotNetCore/CloudFoundry
```

Run tests for a specific framework.

_Example: Run tests for the `ubuntu.14.04-x64` framework._
```
$ behave -t ubuntu.14.04-x64
```

Run tests for a specific runtime.

_Example: Run tests for the `win-10-x64` runtime._
```
$ behave -t win-10-x64
```

### Options

Options can be specified (in order of precedence) using command line switches or the user options file.

Command line switches take the form of: `-Dname`.<br>

The user options file is the DOS INI named `user.ini` (created previously when defining CloudFoundry credentials).

To set a boolean option to `true`, specify a value equal to one of `1`, `yes`, `true`, or `on`.

To set a boolean option to `false`, specify a value equals to one of `0`, `no`, `false`, or `off`.

Specifying an option with no value results in a value of `true` regardless of the option type.

_Example: set the option `foo` using a command line switch._
```
$ -Dfoo=bar           # set option foo to bar
```

_Example: set the option `foo` using the user options file._
```
[behave.userdata]
foo = bar
```

The following sections describe available options.  Option defaults are defined in [`behave.ini`](behave.ini).

#### `cf_max_attempts=int`

Specifies how many attempts to try when checking for CloudFoundry artifacts to be available.

By default, 120 attempts are tried before giving up; the delay between retries is 1 second.
Setting this value to a negative number results in an infinite number of attempts.

#### `cf_space=name`

Specifies name to use for scenario spaces.  If not set, generate a random name per scenario.

By default, not set.

#### `debug_on_error=bool`

Specifies whether to enter debugger upon an error, e.g. a scenario step failure.

By default, debugger is not enabled.


#### `cleanup=bool`

Specifies whether to teardown artifacts after the completion of a scenario.

By default, artifacts are torn down after a scenario completes.
Disabling teardown can be useful in developing and debugging scenarios.

#### `max_attempts=int`

Specifies how many attempts to try when checking for local processes to be ready.

By default, 30 attempts are tried before giving up; the delay between retries is 1 second.
Setting this value to a negative number results in an infinite number of attempts.

#### `output=path`

Specifies where to put test output.

By default, output is written to the `test.out` directory in the root directory of the repository.
It may be necessary or desirable to use a directory other then the default.
For example, on Windows, file length names can exceed the supported maxmium, causing tests to fail.

_Example: override output directory on Windows_
```
$ behave -Doutput=C:\MyOut
```

#### `windowed=bool`

Run background processes in their own windows.

By default, output from processes run in the background is displayed in the same shell that ran `behave`.
Background processes instead can be run in their own windows (a new Command shell on Windows, or an XTerm otherwise) by specifiying `-Dwindowed` so that they have a dedicated display for their output.

[pydown]: https://www.python.org/downloads/
[brew]: https://brew.sh/
