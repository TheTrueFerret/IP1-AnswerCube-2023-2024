import {RemoveLastDirectoryPartOf} from "../../urlDecoder";
import {getCookie} from "../../CookieHandler";
import {generateVoteTables, updateVoteUi} from "../VoteTableHandler";
import {postAnswers, stopSession} from "../CircularFlow";

const slideElement: HTMLElement | null = document.getElementById("slide");
var url = window.location.toString();
const baseUrl = "https://storage.cloud.google.com/answer-cube-bucket/";

let chosenSlideList: number[] = [];
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
            generateVoteTables(activeCubes, voteStatePerCubeId);
            for (let i: number = 0; i < activeCubes.length; i++) {
                voteStatePerCubeId[i] = "none";
            }
        }
    }).catch(err => {
        console.log("Something went wrong: " + err);
        return err; // Return an empty array in case of error
    });
})


function addNewOrDeleteCubeUser(cubeId: number) {
    const index = activeCubes.indexOf(cubeId);
    if (index !== -1) {
        activeCubes.splice(index, 1);
        generateVoteTables(activeCubes, voteStatePerCubeId);
        if (sessionCube[cubeId]) {
            sessionCube[cubeId] = false
            stopSession(cubeId);
        }
    } else {
        activeCubes.push(cubeId); // Add cubeId to activeCubes if it doesn't already exist
        activeCubes.sort((a, b) => a - b);
        generateVoteTables(activeCubes, voteStatePerCubeId);
    }
}

function vote(cubeId: number, action: 'submit' | 'skip' | 'changeSubTheme') {
    let answer: string[] = getTextInput()

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
                Answer: getTextInput(),
                CubeId: activeCubes[i]
            })
        }
        postAnswers(answers)
        console.log("Everyone voted!");
    } else {
        console.log("Not Everyone Voted Yet");
    }
}


function postAnswer(cubeId: number, action: 'submit' | 'skip') {
    let answer = getTextInput();

    if (action === 'submit' && answer.length === 0) {
        console.log('No answers selected');
        alert('Please select at least one answer before submitting <3');
        return;
    }
    
    let requestBody = {
        Answer: answer,
        CubeId: cubeId
    };
    console.log(requestBody);
    fetch(RemoveLastDirectoryPartOf(url) + "/PostAnswer", {
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
    console.log(answer);
}

function getTextInput() {
    let selectedAnswers: string[] = [];

    //Get the value of the text input
    const textInput = document.querySelector('input[type="text"]#input');
    const textbox = textInput as HTMLInputElement; // Assert type to HTMLInputElement
    if (textbox.value) {
        selectedAnswers.push(textbox.value); // Use value property instead of nodeValue
    }
    return selectedAnswers;
}

declare global {
    interface Window {
        slideType: string;
        addNewOrDeleteCubeUser: (cubeId: number) => void;
        vote: (cubeId: number, action: 'submit' | 'skip' | 'changeSubTheme') => void;
        postAnswer: (CubeId: number, action: 'submit' | 'skip') => void;
    }
}

window.slideType = "OpenQuestion";
window.addNewOrDeleteCubeUser = addNewOrDeleteCubeUser;
window.vote = vote;
window.postAnswer = postAnswer;

