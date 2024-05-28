import {getCubeNameByCubeId} from "./CircularFlow";


export function generateVoteTables(activeCubes: number[], voteStatePerCubeId: string[]) {
    const tableIds = ['SubmitTable', 'SkipTable'];
    tableIds.forEach(tableId => {
        const table: HTMLTableElement = document.getElementById(tableId) as HTMLTableElement;
        table.innerHTML = ''; // Clear all content inside the table
    });
    createVoteTable(2, 'SubmitTable');
    createVoteTable(2, 'SkipTable');
    
    for (let i = 0; i < activeCubes.length; i++) {
        if (voteStatePerCubeId[i] != "none") {
            updateVoteUi(activeCubes[i], voteStatePerCubeId[i], true);
        }
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
            cell.setAttribute('data-active', 'true');
            cell.innerHTML = getCubeNameByCubeId(cubeId);
        });
    } else {
        cells.forEach(cell => {
            cell.setAttribute('data-active', 'false');
            cell.innerHTML = '';
        });
    }
}