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

interface ILogRequest {
    configuredLevel: string;
}

interface ILogLevel {
    namespace: string;
    level: string;
}