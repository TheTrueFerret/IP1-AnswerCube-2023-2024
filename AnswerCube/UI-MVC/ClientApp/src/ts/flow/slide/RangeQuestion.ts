import {RemoveLastDirectoryPartOf} from "../../urlDecoder";
import {generateVoteTables, updateVoteUi} from "../VoteTableHandler";
import {getCubeNameByCubeId, postAnswers, startSession, stopSession} from "../CircularFlow";

let url: string = window.location.toString()
let slideElement: HTMLElement | null = document.getElementById("slide");
let  sliderElement: HTMLInputElement = document.getElementById("slider") as HTMLInputElement;
const baseUrl = "https://storage.cloud.google.com/answer-cube-bucket/";


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
            generateVoteTables();
            for (let i: number = 0; i < activeCubes.length; i++) {
                voteStatePerCubeId[activeCubes[i]] = "none";
            }
        }
    }).catch(err => {
        console.log("Something went wrong: " + err);
        return err; // Return an empty array in case of error
    });
})



function generateAnswerColumns() {
    activeCubes.sort((a, b) => a - b);
    activeCubes.forEach(CubeId => {
        addNewAnswerCubeRange(CubeId);
    });
}


function addNewOrDeleteCubeUser(cubeId: number) {
    const index = activeCubes.indexOf(cubeId);
    if (index !== -1) {
        voteStatePerCubeId[cubeId] = "removed";
        activeCubes.splice(index, 1);
        updateVoteUi(cubeId, "SubmitTable", false);
        updateVoteUi(cubeId, "SkipTable", false);
        updateVoteUi(cubeId, "SubthemeTable", false);
        deleteAnswerCubeRange(cubeId);
        if (sessionCube[cubeId]) {
            sessionCube[cubeId] = false
            stopSession(cubeId);
        }
    } else {
        voteStatePerCubeId[cubeId] = "none";
        activeCubes.push(cubeId); // Add cubeId to activeCubes if it doesn't already exist
        activeCubes.sort((a, b) => a - b);
        addNewAnswerCubeRange(cubeId);
        if (!sessionCube[cubeId]) {
            sessionCube[cubeId] = true
            startSession(cubeId);
        }
    }
}

function deleteAnswerCubeRange(cubeId: number) {
    let rangeElement: HTMLElement | null = document.getElementById(`Slider${cubeId}`);
    let cubeName: HTMLElement | null = document.getElementById(`CubeName${cubeId}`);

    if (rangeElement) {
        rangeElement.remove();
    }
    if (cubeName) {
        cubeName.remove();
    }
}


function addNewAnswerCubeRange(cubeId: number) {
    let ranges: HTMLElement | null = document.getElementById('AllRanges');
    let newCubeRange: string =`<div id="CubeName${cubeId}">${getCubeNameByCubeId(cubeId)}</div><input type="range" id="Slider${cubeId}" class="slider custom-slider" list="tickmarks" step="10" min="0" max="100"/>`;
    if (ranges) ranges.insertAdjacentHTML('beforeend', newCubeRange);
}


function vote(cubeId: number, action: 'submit' | 'skip' | 'changeSubTheme') {
    let answer: string[] = getRangeAnswer(cubeId);

    /*// verander deze naar iets deftig
    if (action === 'submit' && answer.length === 0) {
        console.log('No answers selected');
        // Show error to the user, e.g., alert or some UI indication
        alert('Please select at least one answer before submitting <3');
        return;
    }*/
    

    for (let i = 0; i <= activeCubes.length; i++) {
        if (activeCubes[i] == cubeId) {
            if (voteStatePerCubeId[cubeId] == "none") {
                voteStatePerCubeId[cubeId] = action;
            } else {
                if (voteStatePerCubeId[cubeId] != action) {
                    voteStatePerCubeId[cubeId] = action
                }
            }
        }
    }
    updateVoteUi(cubeId, "SubmitTable", false)
    updateVoteUi(cubeId, "SkipTable", false)
    updateVoteUi(cubeId, "SubthemeTable", false)

    switch (action) {
        case "submit":
            updateVoteUi(cubeId, "SubmitTable", true)
            break;
        case "skip":
            updateVoteUi(cubeId, "SkipTable", true)
            break;
        case "changeSubTheme":
            updateVoteUi(cubeId, "SubthemeTable", true)
            break;
    }

    const allAnswered: boolean = voteStatePerCubeId.every(vote => vote !== "none");
    if (allAnswered) {
        let answers: any[] = [];
        for (let i: number = 0; i < activeCubes.length; i++) {
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


function getRangeAnswer(cubeId: number): string[] {
    let rangeElement: HTMLInputElement | null = document.getElementById(`Slider${cubeId}`) as HTMLInputElement;
    let answerValue: string[] = [];
    if (rangeElement?.value) {
        answerValue[0] = rangeElement.value.toString();
    }
    return answerValue;
}

function moveRangeButton(cubeId: number, direction: 'up' | 'down') {
    let rangeInput: HTMLInputElement = document.getElementById('Slider' + cubeId.toString()) as HTMLInputElement;
    let min: number = parseInt(rangeInput.min, 10);
    let max: number = parseInt(rangeInput.max, 10);
    let step: number = parseInt(rangeInput.step, 10);
    if (direction == "up") {
        if (rangeInput.valueAsNumber < max) {
            rangeInput.valueAsNumber += step;
        }
    }
    if (direction == "down") {
        if (rangeInput.valueAsNumber > min) {
            rangeInput.valueAsNumber -= step;
        }
    }
}


declare global {
    interface Window {
        slideType: string;
        addNewOrDeleteCubeUser: (cubeId: number) => void;
        moveRangeButton: (cubeId: number, direction: 'up' | 'down') => void;
        vote: (cubeId: number, action: 'submit' | 'skip' | 'changeSubTheme') => void;
    }
}

window.slideType = "RangeQuestion";
window.addNewOrDeleteCubeUser = addNewOrDeleteCubeUser;
window.moveRangeButton = moveRangeButton;
window.vote = vote;

