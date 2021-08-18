function initializeLogging() {
    logViewService.getNamespaces();
}

function setLogLevel(namespace: string, level: string): void {
    logViewService.setLogLevel(namespace, level);
}

function filterNamespaces(): void {
    const filter = (document.getElementById("searchFilter") as HTMLInputElement).value;
    logViewService.filterNamespaces(filter);
}

function changePage(pageNumber: number) {
    logViewService.changePage(pageNumber);
}
