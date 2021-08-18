interface IDynamicLogLevels {
    levels: Array<string>;
    loggers: IDynamicLogLevel;
}

interface IDynamicLogLevel {
    [id: string]: ILogConfiguration;
}

interface ILogConfiguration {
    effectiveLevel: string;
}

interface IActuatorLoggerRequest {
    configuredLevel: string;
}
