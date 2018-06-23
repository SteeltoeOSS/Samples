# Configuration SDK README

## Using dotnet

### Install

Run `dotnet new --install <PATH>`.

Example:
```
$ dotnet new --install Configuration\src\AspDotNetCore\Simple
```

There is no good way at present to uninstall.  Closest alternative is to run `dotnet new --debug:reinit` which will reset your templates to the defaults, potentially unintentionally removing other 3rd party templates.

Run `dotnet new --list` to see installed templates.


```
$ dotnet new --list                                                                                                               
...
  Templates                                         Short Name         Language          Tags                                       
  ----------------------------------------------------------------------------------------------------------------------------      
...
Steeltoe Cloud Foundry                            stcf               [C#]              Steeltoe/Configuration
Steeltoe Config Server                            stcfgsrvr          [C#]              Steeltoe/Configuration
Steeltoe Cloud Foundry Config Server              stcfcfgsrvr        [C#]              Steeltoe/Configuration
Steeltoe SQL Server EFCore Connector              stmssqlefcore      [C#]              Steeltoe/Connector
Steeltoe Redis Connector                          stredis            [C#]              Steeltoe/Connector
Steeltoe RabbitMQ Connector                       strabbitmq         [C#]              Steeltoe/Connector
Steeltoe PostgreSQL EFCore Connector              stpgsqlefcore      [C#]              Steeltoe/Connector
Steeltoe PostgreSQL Connector                     stpgsql            [C#]              Steeltoe/Connector
Steeltoe MySQL EFCore Connector                   stmysqlefcore      [C#]              Steeltoe/Connector
Steeltoe MySQL EF6 Connector                      stmysqlef6         [C#]              Steeltoe/Connector
Steeltoe MySQL Connector                          stmysql            [C#]              Steeltoe/Connector
...
```

### Create project

Example:

```
$ mkdir MyConfigServer
$ cd MyConfigServer
$ dotnet new stcfgsrvr
```

## Using Visual Studio 2017

Open and run the [TemplatePack](TemplatePack.sln) solution.
