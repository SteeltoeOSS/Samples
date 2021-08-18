class LogViewService {

    dynamicLogLevels: IDynamicLogLevels;
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

    public filterNamespaces(namespace: string): void {
        this.filter = namespace;

        this.buildNamespaces();
    }

    public changePage(pageIndex: number): void {
        this.buildNamespaces(pageIndex);
    }

    private buildNamespaces(pageIndex: number = 1): void {
        const tableElement = document.getElementById(`logTable-body`);
        tableElement.innerHTML = "";

        let keys = Object.keys(this.dynamicLogLevels.loggers);
    
        if(this.filter.trim() !== "")
        {
            keys = keys.filter((key: string) => 
                key.toLowerCase().indexOf(this.filter.toLowerCase()) > -1);
        }

        const pageCount = Math.ceil(keys.length / this.maxPageSize);
        this.buildPages(pageIndex, pageCount);

        // since table.insertRow places items on top, perform in reverse order
        keys.slice(pageIndex, pageIndex + this.maxPageSize).reverse().forEach((id: string) => {
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

    private buildPages(currentPage: number, pageCount: number): void {
        let pageMarkup: string = "";

        if (pageCount > 1) {
            for(let i = 1; i <= pageCount; i++) {
                pageMarkup += (i === currentPage) ?
                    this.getActivePageDisplay(i) :
                    this.getNonActivePageDisplay(i);
            }
        }

        const pageListElement = document.getElementById("Page-List");
        pageListElement.innerHTML = pageMarkup;
    }

    private addLogLevels(namespace: string, currentLevel: string): string {
        return this.dynamicLogLevels.levels.map(level => 
            level.toLowerCase() === currentLevel.toLowerCase() ? 
                this.getActiveLogLevelDisplay(namespace, level) :
                this.getNonActiveLogLevelDisplay(namespace, level)).join('');
    }

    private updateLogLevels(namespace: string, level: string): void {
        const keys = Object.keys(this.dynamicLogLevels.loggers);
        
        keys.forEach((id: string) => {
            if(id.indexOf(namespace) > -1) {
                this.dynamicLogLevels.loggers[id].effectiveLevel = level;
                const logLevelListElement = document.getElementById(`${id}-log-levels`);
                logLevelListElement.innerHTML = this.addLogLevels(id, level);
            }
        });
    }

    private getActiveLogLevelDisplay(namespace: string, level: string): string
    {
        return `<li id="${namespace}-${level}" class="inline-list-item active">${level}</li>`;
    }

    private getNonActiveLogLevelDisplay(namespace: string, level:string) : string
    {
        return `<li id="${namespace}-${level}" class="inline-list-item">
                    <a href="" onclick="setLogLevel('${namespace}','${level}'); return false;">${level}</a>
                </li>`
    }

    private getActivePageDisplay(page: number): string
    {
        return `<li class="inline-list-item active">${page}</li>`;
    }

    private getNonActivePageDisplay(page: number) : string
    {
        return `<li class="inline-list-item">
                    <a href="" onclick="changePage(${page}); return false;">${page}</a>
                </li>`
    }
}

const logViewService = new LogViewService();