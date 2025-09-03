# Steeltoe Sample Application Tests

## Using the `behave` Wrappers

The Samples tests are run using [behave][behave_url], a Cucumber-style BDD framework written in Python.
This project's `behave` implementation requires Python 3.  See [Installing Python 3](#installing-python-3) for platform-specific instructions.

Two helper scripts, [behave.ps1](behave.ps1) and [behave.sh](behave.sh), are provided to simplify the setup and invocation of `behave`.
These wrappers:

1. install `pipenv` into the user's Python package install directory
1. create a Python virtual environment for the project using `pipenv`
1. install needed Python packages into the virtual environment
1. invoke `behave` in the virtual environment

Steps 1-3 are only run if necessary (typically the first run).  To force the wrapper scripts to re-run steps 1-3, create an empty file named `reinit` in the project root and rerun the wrapper.

Any arguments and parameters passed to a wrapper are subsequently passed to `behave`.

As an example, the following displays the help for `behave`:

```dos
C:> .\behave.ps1 -h
```

Running with no arguments will run every sample for every framework/runtime combination.

```dos
C:> .\behave.ps1
```

To run a specific sample, pass the path to the sample:

```dos
C:> .\behave.ps1 Connectors\src\RabbitMQ
```

To run only a specific framework/runtime combination, use the `--tags` or `-t` parameter:

```dos
C:> .\behave.ps1 Connectors\src\RabbitMQ -t net8.0 -t ubuntu.16.04-x64
```

## Configuring

Create a `user.ini` file in the project root directory.
An example file, [`user.ini.example`](user.ini.example), is provided as a convenience.

```dos
C:> copy user.ini.example user.ini
```

The example file's options are commented with descriptions.

One option you might want to enable is `windowed = yes`.
Setting this option will run background processes in their own dedicated windows, making it easier to follow a test's progress.

### Cloud Foundry

If you don't specify Cloud Foundry credentials, it is assumed you are already logged in to a Cloud Foundry endpoint.

You can configure credentials by setting the following options in `user.ini`:

* `cf_apiurl`
* `cf_username`
* `cf_password`
* `cf_org`

Sample:

```text
[behave.userdata]
cf_apiurl = https://api.run.pcfone.io
cf_username = myuser
cf_password = mypass
cf_org = p-steeltoe
```

It is expected that a Cloud Foundry space named `development` exists for the configured credentials.
The `development` space will be used as the target from which to create additional spaces for running tests.

By default, each sample will use a dedicated space for its applications and services.
The space is named `{feature}-{sample}-{os}` where:

* `{feature}` is the feature name (the top directory node of the sample path)
* `{sample}` is the sample name (the bottom directory node of the sample path)
* `{os}` is one of: `windows`, `osx`, `linux` depending on the platform on which the tests are run

As an example, if running the sample `Connectors\src\RabbitMQ` on Windows, it will use the space `connectors-rabbitmq-windows`.

You can override this behavior by setting the `cf_space` option.

```text
cf_space = myspace
```

## Installing Python 3

Running the Samples tests requires Python 3 and its corresponding `pip` package manager.

### Windows

Install [Chocolatey][choco_url].

Start a PowerShell as Administrator and run:

```dos
C:> choco install -y python3
```

### OS X

Install [Homebrew][brew_url].

Start a terminal and run:

```sh
$ brew install python3
```

### Ubuntu

Start a terminal and run:

```sh
sudo apt install -y python3 python3-pip
```

[choco_url]: https://chocolatey.org/
[brew_url]: https://brew.sh/
[behave_url]: https://github.com/behave/behave
