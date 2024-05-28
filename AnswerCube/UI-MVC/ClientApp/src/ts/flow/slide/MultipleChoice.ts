import {RemoveLastDirectoryPartOf} from "../../urlDecoder";
import {generateVoteTables, updateVoteUi} from "../VoteTableHandler";
import {getCubeNameByCubeId, postAnswers, stopSession} from "../CircularFlow";

const slideElement: HTMLElement | null = document.getElementById("slide");
var url: string = window.location.toString();
const baseUrl = "https://storage.cloud.google.com/answer-cube-bucket/";

const table = document.getElementById("AnswerTable") as HTMLTableElement | null;
const headerRow = document.getElementById("HeaderRow") as HTMLTableRowElement | null;

let currentCheckedIndexPerUser: number[] = [];
let totalQuestions: number;
let activeCubes: number[] = []; // get active cubes
let sessionCube: boolean[] = [];
let voteStatePerCubeId: string[] = [];


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
            generateVoteTables(activeCubes, voteStatePerCubeId);
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
        generateVoteTables(activeCubes, voteStatePerCubeId);
        if (sessionCube[cubeId]) {
            sessionCube[cubeId] = false
            stopSession(cubeId);
        }
    } else {
        activeCubes.push(cubeId); // Add cubeId to activeCubes if it doesn't already exist
        activeCubes.sort((a, b) => a - b);
        addNewCubeAnswerColumn(cubeId);
        generateVoteTables(activeCubes, voteStatePerCubeId);
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
            newCell.innerHTML = `<div id="Cube${(cubeId)}_Row${rowIndex}" data-checked="false" data-highlighted="false" data-cube="${(cubeId)}" data-row="${rowIndex}"></div>`;
        }
        totalQuestions = table.rows.length - 1;
    }
}


function vote(cubeId: number, action: 'submit' | 'skip' | 'changeSubTheme') {
    let answer: string[] = getSelectedAnswers(cubeId)

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
                Answer: answer,
                CubeId: activeCubes[i]
            })
        }
        postAnswers(answers)
        console.log("Everyone voted!");
    } else {
        console.log("Not Everyone Voted Yet");
    }
}

function getSelectedAnswers(cubeId: number): string[] {
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


function moveSelectedButton(cubeId: number, direction: 'up' | 'down') {
    if (table) {
        for (let i = 1; i <= totalQuestions; i++) {
            let elementId = `Cube${cubeId}_Row${i}`;
            let element = document.getElementById(elementId);
            if (element) {
                if (currentCheckedIndexPerUser[cubeId] === -1) {
                    currentCheckedIndexPerUser[cubeId] = 1;
                    element.setAttribute('data-highlighted', 'true')
                    element.innerHTML = 'Highlighted';
                    return;
                }

                let newIndex;
                if (currentCheckedIndexPerUser[cubeId] === i && direction === 'up') {
                    newIndex = (i - 1);
                    if (newIndex < 1) newIndex = totalQuestions;
                } else if (currentCheckedIndexPerUser[cubeId] === i && direction === 'down') {
                    newIndex = (i + 1);
                    if (newIndex > totalQuestions) newIndex = 1;
                } else {
                    continue;
                }

               
                if (element.getAttribute('data-checked') == 'false') {
                    element.setAttribute('data-highlighted', 'false')
                    element.innerHTML = '';
                } else {
                    element.setAttribute('data-highlighted', 'false')
                    element.innerHTML = 'Selected';
                }
                
                let newElementId = `Cube${cubeId}_Row${newIndex}`;
                let newElement = document.getElementById(newElementId);
                if (newElement) {
                    newElement.setAttribute('data-highlighted', 'true')
                    newElement.innerHTML = 'Highlighted';
                }
                currentCheckedIndexPerUser[cubeId] = newIndex;
                return;
            }
        }
    }
    
    // Check if there's a checkbox checked
    /*if (currentCheckedIndex === -1) {
        checkboxes[0].focus();
        currentCheckedIndex = 0;
        return;
    }

    let newIndex;
    if (direction === 'up') {
        newIndex = currentCheckedIndex - 1;
        if (newIndex < 0) newIndex = totalCheckboxes - 1;
    } else if (direction === 'down') {
        newIndex = currentCheckedIndex + 1;
        if (newIndex >= totalCheckboxes) newIndex = 0;
    } else {
        return; // Invalid direction
    }
    checkboxes[newIndex].focus();
    currentCheckedIndex = newIndex;*/
}

function selectButton(cubeId: number) {
    if (table) {
        for (let i = 1; i <= totalQuestions; i++) {
            let elementId = `Cube${cubeId}_Row${i}`;
            let element = document.getElementById(elementId);
            if (element) {
                if (element.getAttribute('data-highlighted') == 'true') {
                    if (element.getAttribute('data-checked') == 'false') {
                        element.setAttribute('data-checked', 'true');
                        element.innerHTML = 'Selected';
                        return;
                    } else if (element.getAttribute('data-checked') == 'true') {
                        element.setAttribute('data-checked', 'false');
                        element.innerHTML = 'Highlighted';
                        return;
                    }
                }
            }
        }
    }
    
    //checkboxes[currentCheckedIndex].checked = !checkboxes[currentCheckedIndex].checked;
}

declare global {
    interface Window {
        slideType: string;
        addNewOrDeleteCubeUser: (cubeId: number) => void;
        vote: (cubeId: number, action: 'submit' | 'skip' | 'changeSubTheme') => void;
        moveSelectedButton: (cubeId: number, direction: 'up' | 'down') => void;
        selectButton: (cubeId: number) => void;
    }
}
window.slideType = "MultipleChoice";
window.addNewOrDeleteCubeUser = addNewOrDeleteCubeUser;
window.vote = vote;
window.moveSelectedButton = moveSelectedButton;
window.selectButton = selectButton;
