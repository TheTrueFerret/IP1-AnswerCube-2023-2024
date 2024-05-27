import {RemoveLastDirectoryPartOf} from "../../urlDecoder";
import {getCookie} from "../../CookieHandler";
import {types} from "sass";
import Number = types.Number;
import {Tab} from "bootstrap";

var url = window.location.toString()
const slideElement: HTMLElement | null = document.getElementById("slide");

const table = document.getElementById("AnswerTable") as HTMLTableElement | null;
const headerRow = document.getElementById("HeaderRow") as HTMLTableRowElement | null;

var currentCheckedIndexPerUser: number[] = [];
var totalQuestions: number;
var activeCubes: number[]; // get active cubes
var voteStatePerCubeId: string[] = [];


document.addEventListener("DOMContentLoaded", function (){
    activeCubes = [2,1];
    generateAnswerColumns(activeCubes);
    generateVoteTables(activeCubes);
})

function generateAnswerColumns(cubeIdList: number[]) {
    cubeIdList.sort((a, b) => b - a);
    activeCubes.forEach(CubeId => {
        addNewCubeColumn(CubeId);
    });
}


function addNewCubeColumn(cubeId: number) {
    if (table) {
        if (headerRow) {
            const newHeaderCell = document.createElement("th");
            newHeaderCell.innerHTML = getCubeNameByCubeId(cubeId);
            headerRow.insertBefore(newHeaderCell, headerRow.firstChild);
            currentCheckedIndexPerUser[cubeId] = -1;
        }
        // Add new cells to each existing row (excluding the header)
        for (let rowIndex = 1; rowIndex < table.rows.length; rowIndex++) {
            const row = table.rows[rowIndex];
            const newCell = row.insertCell(0);
            newCell.innerHTML = `<div id="Cube${(cubeId)}_Row${rowIndex}" data-checked="false" data-cube="${(cubeId)}" data-row="${rowIndex}"></div>`;
        }
        totalQuestions = table.rows.length - 1;
    }
}


function generateVoteTables(activeCubes: number[]) {
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
function createVoteTable(rows: number, tableId: string) {
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


function getCubeNameByCubeId(CubeId: number): string {
    switch (CubeId) {
        case(0):
            return "Keyboard";
        case(1):
            return "Fret";
        case(2):
            return "Destiny";
        case(3):
            return "Rider";
        case(4):
            return "Striker";
    }
    return "null"
}


function vote(cubeId: number, action: 'submit' | 'skip' | 'changeSubTheme') {
    let answer = getSelectedAnswerByCubeId(cubeId)
    
    // verander deze naar iets deftig
    if (action === 'submit' && answer.length === 0) {
        console.log('No answers selected');
        // Show error to the user, e.g., alert or some UI indication
        alert('Please select at least one answer before submitting <3');
        return;
    }

    // make it so you can only vote on one!!!
    // for example when you press skip and your already pressed submit (and voted submit)
    // your vote changes to submit
    if (voteStatePerCubeId[cubeId-1] == "none") {
        voteStatePerCubeId[cubeId-1] = action;
    }
    
    switch (action) {
        case "submit":
            updateVoteUi(cubeId, "SubmitTable")
            break;
        case "skip":
            updateVoteUi(cubeId, "SkipTable")
            break;
        case "changeSubTheme":
            updateVoteUi(cubeId, "")
            break;
    }
    
    const allAnswered: boolean = voteStatePerCubeId.every(vote => vote !== "none");
    if (allAnswered) {
        postAnswers()
        console.log("Everyone voted!");
    } else {
        console.log("Not Everyone Voted Yet");
    }
}


function updateVoteUi(cubeId: number, tableId: 'SubmitTable' | 'SkipTable' | '') {
    if (!tableId) return;
    const table: HTMLTableElement = document.getElementById(tableId) as HTMLTableElement;
    const cells = table.querySelectorAll(`td[data-cube='${cubeId}']`);
    cells.forEach(cell => {
        cell.setAttribute('data-active', 'true');
        cell.innerHTML = getCubeNameByCubeId(cubeId);
    });
}


function postAnswers() {
    let answers: any[] = []
    
    for (let i = 0; i < activeCubes.length; i++) {
        answers.push({
            Answer: getSelectedAnswerByCubeId(activeCubes[i]),
            CubeId: activeCubes[i]
        })
    }

    let requestBody = {
        Answers: answers
    };
    
    console.log(requestBody);
    fetch(RemoveLastDirectoryPartOf(url) + "/PostAnswer/", {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
        },
        body: JSON.stringify(requestBody)
    }).then((response: Response) => {
        if (response.status === 200) {
            return response.json();
        } else {
            if (slideElement) {
                slideElement.innerHTML = "<em>problem!!!</em>";
            }
        }
    }).then((nextSlideData: any) => {
        if (nextSlideData.url) {
            // Redirect to the URL of the next slide
            window.location.href = nextSlideData.url;
        }
    }).catch(err => {
        console.log("Something went wrong: " + err);
    })
}

function getSelectedAnswerByCubeId(cubeId: number): string[] {
    let selectedAnswers: string[] = [];
    for (let i: number = 1; i <= totalQuestions; i++) {
        const elementId = `Cube${cubeId}_Row${i}`;
        const element = document.getElementById(elementId);
        if (element && element.getAttribute('data-checked') === 'true') {
            const answerColumn = document.getElementById(`AnswerRow ${i}`);
            if (answerColumn)
            selectedAnswers.push(answerColumn.innerHTML);
        }
    }
    return selectedAnswers;
}


function moveCheckedRadioButton(CubeId: number, direction: 'up' | 'down') {
    if (table) {
        for (let i = 1; i <= totalQuestions; i++) {
            let elementId = `Cube${CubeId}_Row${i}`;
            let element = document.getElementById(elementId);
            if (element) {
                if (currentCheckedIndexPerUser[CubeId] === -1) {
                    element.setAttribute('data-checked', 'true');
                    currentCheckedIndexPerUser[CubeId] = 1;
                    element.innerHTML = 'Selected';
                    return;
                }

                let newIndex;
                if (currentCheckedIndexPerUser[CubeId] === i && direction === 'up') {
                    newIndex = (i - 1);
                    if (newIndex < 1) newIndex = totalQuestions;
                } else if (currentCheckedIndexPerUser[CubeId] === i && direction === 'down') {
                    newIndex = (i + 1);
                    if (newIndex > totalQuestions) newIndex = 1;
                } else {
                    continue;
                }
                
                let newElementId = `Cube${CubeId}_Row${newIndex}`;
                let newElement = document.getElementById(newElementId);
                element.setAttribute('data-checked', 'false');
                element.innerHTML = '';
                if (newElement) {
                    newElement.setAttribute('data-checked', 'true');
                    newElement.innerHTML = 'Selected';
                }
                currentCheckedIndexPerUser[CubeId] = newIndex;
                return;
            }
        }
    }
}


declare global {
    interface Window {
        slideType: string;
        moveCheckedRadioButton: (CubeId: number, direction: 'up' | 'down') => void;
        vote: (cubeId: number, action: 'submit' | 'skip' | 'changeSubTheme') => void;
    }
}

window.slideType = "SingleChoice";
window.moveCheckedRadioButton = moveCheckedRadioButton;
window.vote = vote;

