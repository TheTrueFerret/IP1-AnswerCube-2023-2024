import {RemoveLastDirectoryPartOf} from "../../urlDecoder";
import {generateVoteTables, updateVoteUi} from "../VoteTableHandler";
import {getCubeNameByCubeId, postAnswers, stopSession} from "../CircularFlow";

let url = window.location.toString()
const slideElement: HTMLElement | null = document.getElementById("slide");

const table = document.getElementById("AnswerTable") as HTMLTableElement | null;
const headerRow = document.getElementById("HeaderRow") as HTMLTableRowElement | null;

export let currentCheckedIndexPerUser: number[] = [];
export let totalQuestions: number;
export let activeCubes: number[] = []; // get active cubes
export let sessionCube: boolean[] = [];

export let voteStatePerCubeId: string[] = [];


document.addEventListener("DOMContentLoaded", function (){
    fetch(RemoveLastDirectoryPartOf(url) + "/GetActiveSessionsFromInstallation/", {
        method: "GET",
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
        },
    }).then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return response.json();
    }).then(data => {
        if (data.length > 0) {
            for (let i: number = 0; i < data.length; i++) {
                activeCubes[i] = data[i]
                sessionCube[i] = true
            }
            console.log(data);
            generateAnswerColumns();
            generateVoteTables();
        }
    }).catch(err => {
        console.log("Something went wrong: " + err);
        return err; // Return an empty array in case of error
    });
})


function generateAnswerColumns() {
    activeCubes.sort((a, b) => a - b);
    activeCubes.forEach(CubeId => {
        addNewCubeAnswerColumn(CubeId);
    });
}


function addNewOrDeleteCubeUser(cubeId: number) {
    const index = activeCubes.indexOf(cubeId);
    if (index !== -1) {
        activeCubes.splice(index, 1);
        deleteAnswerCubeColumn(cubeId);
        generateVoteTables();
        if (sessionCube[cubeId]) {
            sessionCube[cubeId] = false
            stopSession(cubeId);
        }
    } else {
        activeCubes.push(cubeId); // Add cubeId to activeCubes if it doesn't already exist
        activeCubes.sort((a, b) => a - b);
        addNewCubeAnswerColumn(cubeId);
        generateVoteTables();
    }
}

function deleteAnswerCubeColumn(cubeId: number) {
    if (table && headerRow) {
        // Remove the header cell corresponding to cubeId
        const headerCells = headerRow.getElementsByTagName("th");
        for (let i = 0; i < headerCells.length; i++) {
            if (headerCells[i].innerHTML === getCubeNameByCubeId(cubeId)) {
                headerRow.deleteCell(i);
                break;
            }
        }

        // Remove the cell corresponding to cubeId from each row (excluding header row)
        for (let rowIndex = 1; rowIndex < table.rows.length; rowIndex++) {
            const row = table.rows[rowIndex];
            row.deleteCell(0);
        }

        // Update totalQuestions count
        totalQuestions = table.rows.length - 1;
    }
}


function addNewCubeAnswerColumn(cubeId: number) {
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




function vote(cubeId: number, action: 'submit' | 'skip' | 'changeSubTheme') {
    let answer = getSelectedAnswerByCubeId(cubeId)

    // verander deze naar iets deftig
    if (action === 'submit' && answer.length === 0) {
        console.log('No answers selected');
        // Show error to the user, e.g., alert or some UI indication
        alert('Please select at least one answer before submitting <3');
        return;
    }


    for (let i = 0; i <= activeCubes.length; i++) {
        if (activeCubes[i] == cubeId) {
            if (voteStatePerCubeId[i] == "none") {
                voteStatePerCubeId[i] = action;
            } else {
                if (voteStatePerCubeId[i] != action) {
                    voteStatePerCubeId[i] = action
                }
            }
        }
    }
    updateVoteUi(cubeId, "SubmitTable", false)
    updateVoteUi(cubeId, "SkipTable", false)
    updateVoteUi(cubeId, "", false)

    switch (action) {
        case "submit":
            updateVoteUi(cubeId, "SubmitTable", true)
            break;
        case "skip":
            updateVoteUi(cubeId, "SkipTable", true)
            break;
        case "changeSubTheme":
            updateVoteUi(cubeId, "", true)
            break;
    }

    const allAnswered: boolean = voteStatePerCubeId.every(vote => vote !== "none");
    if (allAnswered) {
        let answers: any[] = [];
        for (let i = 0; i < activeCubes.length; i++) {
            answers.push({
                Answer: getSelectedAnswerByCubeId(activeCubes[i]),
                CubeId: activeCubes[i]
            })
        }
        postAnswers(answers)
        console.log("Everyone voted!");
    } else {
        console.log("Not Everyone Voted Yet");
    }
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
        addNewOrDeleteCubeUser: (cubeId: number) => void;
        generateVoteTables: () => void;
    }
}

window.slideType = "SingleChoice";
window.moveCheckedRadioButton = moveCheckedRadioButton;
window.vote = vote;
window.addNewOrDeleteCubeUser = addNewOrDeleteCubeUser
window.generateVoteTables = generateVoteTables
