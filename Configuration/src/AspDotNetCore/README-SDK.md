# Configuration SDK README

## Using dotnet

### Install

```
$ dotnet new --install Simple
$ dotnet new --install SimpleCloudFoundry
```

There is no good way at present to uninstall.  Closest alternative is to run `dotnet new --debug:reinit` which will reset your templates to the defaults, potentially unintentionally removing other 3rd party templates.

After running the above 2 commands, you'll have 2 new templates:

```
$ dotnet new --list                                                                                                               
...
  Templates                                         Short Name         Language          Tags                                       
  ----------------------------------------------------------------------------------------------------------------------------      
...
  Steeltoe Config Server                            stcfgsrvr          [C#]              Steeltoe/Config                            
  Steeltoe Cloud Foundry Config Server              stcfcfgsrvr        [C#]              Steeltoe/Config                            
...
```

### Create project

Example:

```
$ mkdir MyConfigServer
$ cd MyConfigServer
$ dotnet new stcfgsrvr
```

