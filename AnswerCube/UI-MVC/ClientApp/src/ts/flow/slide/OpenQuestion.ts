import {RemoveLastDirectoryPartOf} from "../../urlDecoder";
import {getCookie} from "../../CookieHandler";
import {updateVoteUi} from "../VoteTableHandler";
import {postAnswers} from "../CircularFlow";

const slideElement: HTMLElement | null = document.getElementById("slide");
var url = window.location.toString();
const baseUrl = "https://storage.cloud.google.com/answer-cube-bucket/";

let currentCheckedIndexPerUser: number[] = [];
let totalQuestions: number;
let activeCubes: number[] = []; // get active cubes
let sessionCube: boolean[] = [];
let voteStatePerCubeId: string[] = [];


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
        // Show error to the user, e.g., alert or some UI indication
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
        vote: (cubeId: number, action: 'submit' | 'skip' | 'changeSubTheme') => void;
        postAnswer: (CubeId: number, action: 'submit' | 'skip') => void;
    }
}
window.slideType = "OpenQuestion";
window.vote = vote;
window.postAnswer = postAnswer;

