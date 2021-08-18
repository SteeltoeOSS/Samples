class LogViewService {

    dynamicLogLevels: IDynamicLogLevels;
    pageIndex: number = 0;
    filter: string = "";
    readonly maxPageSize: number = 10;

    public getNamespaces(): void {
        LogService.getLogLevelsAndNamespaces().then(data => {
            this.dynamicLogLevels = data;
            
            this.buildNamespaces();
        },
        error => console.error(error));
    };

    public setLogLevel(namespace: string, newLevel: string): void {
        LogService.setLogLevel(namespace, newLevel).then(async (data: ILogConfiguration) => {
            this.updateLogLevels(namespace, newLevel);
        },
        error => console.error(error));
    }

    private buildNamespaces(): void {
        const tableElement = document.getElementById(`logTable-body`);
        tableElement.innerHTML = "";

        let keys = Object.keys(this.dynamicLogLevels.loggers);
    
            if(this.filter.trim() !== "")
            {
                keys = keys.filter((key: string) => 
                    key.toLowerCase().indexOf(this.filter.toLowerCase()) > -1);
            }
    
            keys.slice().reverse().forEach((id: string) => {
                let table = document.getElementById("logTable-body") as HTMLTableElement;
                var row = table.insertRow(0);
                var cell = row.insertCell(0);
    
                cell.innerHTML = `<span class="logger">${id}</span>
                    <ul id="${id}-log-levels" class="inline-list">
                        ${this.addLogLevels(
                            id, 
                            this.dynamicLogLevels.loggers[id].effectiveLevel)}
                    </ul>`;
            });
    }

    public filterNamespaces(namespace: string): void {
        this.filter = namespace;

        this.buildNamespaces();
    }

    private addLogLevels(namespace: string, currentLevel: string): string
    {
        return this.dynamicLogLevels.levels.map(level => 
            level.toLowerCase() === currentLevel.toLowerCase() ? 
                this.getActiveLogLevelDisplay(namespace, level) :
                this.getNonActiveLogLevelDisplay(namespace, level)).join('');
    }

    private updateLogLevels(namespace: string, level: string)
    {
        const keys = Object.keys(this.dynamicLogLevels.loggers);
        
        keys.forEach((id: string) => {
            if(id.indexOf(namespace) > -1) {
                this.dynamicLogLevels.loggers[id].effectiveLevel = level;
                const logLevelListElement = document.getElementById(`${id}-log-levels`);
                logLevelListElement.innerHTML = this.addLogLevels(id, level);
            }
        });
    }

    private getActiveLogLevelDisplay(namespace: string, level: string)
    {
        return `<li id="${namespace}-${level}" class="log-level inline-list-item active">${level}</li>`;
    }

    private getNonActiveLogLevelDisplay(namespace: string, level:string) : string
    {
        return `<li id="${namespace}-${level}" class="log-level inline-list-item">
                    <a href="" onclick="setLogLevel('${namespace}','${level}'); return false;">${level}</a>
                </li>`
    }
}

const logViewService = new LogViewService();