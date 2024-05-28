import {activeCubes, voteStatePerCubeId} from "./slide/SingleChoice";
import {getCubeNameByCubeId} from "./CircularFlow";


export function generateVoteTables() {
    const tableIds = ['SubmitTable', 'SkipTable'];
    tableIds.forEach(tableId => {
        const table: HTMLTableElement = document.getElementById(tableId) as HTMLTableElement;
        table.innerHTML = ''; // Clear all content inside the table
    });

    if (activeCubes.length <= 3){
        createVoteTable(1, 'SubmitTable');
        createVoteTable(1, 'SkipTable');
    } else {
        createVoteTable(2, 'SubmitTable');
        createVoteTable(2, 'SkipTable');
    }
    for (let i = 0; i < activeCubes.length; i++) {
        voteStatePerCubeId[i] = "none";
    }
}


// Function to create a table with a specified number of rows and columns
export function createVoteTable(rows: number, tableId: string) {
    const table: HTMLTableElement = document.getElementById(tableId) as HTMLTableElement;
    for (let i = 1; i <= rows; i++) {
        const row = document.createElement('tr');
        for (let i = 1; i <= 3; i++) {
            const cell = document.createElement('td');
            cell.setAttribute("data-cube", (i).toString());
            cell.setAttribute("data-active", "false");
            row.appendChild(cell);
        }
        table.appendChild(row);
    }
}


export function updateVoteUi(cubeId: number, tableId: 'SubmitTable' | 'SkipTable' | '', vote: boolean) {
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