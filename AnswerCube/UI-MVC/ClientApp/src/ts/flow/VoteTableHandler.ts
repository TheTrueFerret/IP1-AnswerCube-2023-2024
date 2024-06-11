import {getCubeNameByCubeId} from "./CircularFlow";

export function generateVoteTables() {
    var tableIds: string[];
    if (window.slideType == "InfoSlide" || window.slideType == "LeaveContactInfo") {
        tableIds = ['SkipTable', 'SubthemeTable'];
    } else {
        tableIds = ['SubmitTable', 'SkipTable', 'SubthemeTable'];
    }
    tableIds.forEach(tableId => {
        const table: HTMLTableElement = document.getElementById(tableId) as HTMLTableElement;
        if (table) {
            table.innerHTML = ''; // Clear all content inside the table
        }
    });
    if (window.slideType == "InfoSlide" || window.slideType == "LeaveContactInfo") {
        createVoteTable(2, 'SkipTable');
        createVoteTable(2, 'SubthemeTable');
    } else {
        createVoteTable(2, 'SubmitTable');
        createVoteTable(2, 'SkipTable');
        createVoteTable(2, 'SubthemeTable');
    }
}


// Function to create a table with a specified number of rows and columns
export function createVoteTable(rows: number, tableId: string) {
    const table: HTMLTableElement = document.getElementById(tableId) as HTMLTableElement;

    let cubeCounter = 1;
    for (let i = 1; i <= rows; i++) {
        const row = document.createElement('tr');
        for (let j = 1; j <= 3; j++) {
            const cell = document.createElement('td');
            cell.setAttribute("data-cube", cubeCounter.toString());
            cell.setAttribute("data-active", "false");
            cubeCounter++;
            
            row.appendChild(cell);
        }
        table.appendChild(row);
    }
}


export function updateVoteUi(cubeId: number, tableId: string, vote: boolean) {
    if (!tableId) return;
    const table: HTMLTableElement = document.getElementById(tableId) as HTMLTableElement;
    const cells = table.querySelectorAll(`td[data-cube='${cubeId}']`);
    if (vote) {
        cells.forEach(cell => {
            if (cell.getAttribute('data-active') == 'true') {
                cell.setAttribute('data-active', 'false');
                cell.innerHTML = "";
            } else {
                cell.setAttribute('data-active', 'true');
                cell.innerHTML = getCubeNameByCubeId(cubeId);
            }
        });
    } else {
        cells.forEach(cell => {
            cell.setAttribute('data-active', 'false');
            cell.innerHTML = '';
        });
    }
}