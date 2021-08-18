function initializeLogging()
{
    getNamespaces();
}

function getNamespaces(filter: string = "")
{
    var logService = new LogService();

    logService.GetLogLevelsAndNamespaces().then(data =>{

        let keys = Object.keys(data.loggers);

        if(filter.trim() !== "")
        {
            keys = keys.filter((key: string) => key.toLowerCase().indexOf(filter.toLowerCase()) > -1);
        }

        keys.forEach((id: string, index: number) => {
            let table = document.getElementById("logTable-body") as HTMLTableElement;
            var row = table.insertRow(0);
            var cell = row.insertCell(0);

            cell.innerHTML = `<span class="logger">${id}</span>
                <ul class="inline-list">
                    ${addLogLevels(id, data.loggers[id].effectiveLevel, data.levels)}
                </ul>`;
        });
    },
    error => console.error(error));
}

function addLogLevels(name: string, currentLevel: string, logLevels: string[]): string
{
    return logLevels.map(level => level.toLowerCase() === currentLevel.toLowerCase() ? 
        activeLevelDisplay(name, level) :
        nonActiveLevelDisplay(name, currentLevel, level)).join('');
}

function setLogLevel(name: string, currentLevel: string, newLevel: string): void
{
    var logService = new LogService();

    logService.SetLogLevel(name, newLevel).then(async (data: ILogConfiguration) => {
      
        await resetAllLogLevels(name, newLevel);

        const newLevelElement = document.getElementById(`${name}-${newLevel}`);
        newLevelElement.outerHTML = activeLevelDisplay(name, newLevel);
    },
    error => console.error(error));
}

function filterNamespaces(): void
{
    resetNamespaces();

    const filterValue = (document.getElementById("searchFilter") as HTMLInputElement).value;

    getNamespaces(filterValue);
}

function resetNamespaces(): void
{
    const tableElement = document.getElementById(`logTable-body`);
    tableElement.innerHTML = "";
}

async function resetAllLogLevels(name: string, newLevel: string): Promise<void>
{
    var logService = new LogService();

    await logService.GetLogLevelsAndNamespaces().then(data =>{

        data.levels.forEach((level: string) => {
            const oldLevelElement = document.getElementById(`${name}-${level}`);
            oldLevelElement.outerHTML = nonActiveLevelDisplay(name, newLevel, level);
        });
    },
    error => console.error(error));
}

function activeLevelDisplay(name: string, level: string)
{
    return `<li id="${name}-${level}" class="inline-list-item active">${level}</li>`;
}

function nonActiveLevelDisplay(name: string, currentLevel: string, level:string) : string
{
    return `<li id="${name}-${level}" class="inline-list-item">
                <a href="" onclick="setLogLevel('${name}','${currentLevel}','${level}'); return false;">${level}</a>
            </li>`
}


