import {RemoveLastDirectoryPartOf} from "../../urlDecoder";

let url = window.location.toString()
var slideElement: HTMLElement | null = document.getElementById("slide");
var  sliderElement: HTMLInputElement = document.getElementById("slider") as HTMLInputElement;
const baseUrl = "https://storage.cloud.google.com/answer-cube-bucket/";

let rangeInput: any = document.querySelector<HTMLInputElement>('input[type="range"]');
let min: number = parseInt(rangeInput.min, 10);
let max: number = parseInt(rangeInput.max, 10);
let step: number = rangeInput.step ? parseInt(rangeInput.step, 10) : 1;


function postAnswer(cubeId: number, action: 'submit' | 'skip') {
    let answer: string[] = getRangeAnswer();

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
    })
        .then((response: Response) => {
            if (response.status === 200) {
                return response.json();
            } else {
                if (slideElement) {
                    slideElement.innerHTML = "<em>Problem!!!</em>";
                }
            }
        })
        .then((nextSlideData: any) => {
            if (nextSlideData.url) {
                window.location.href = nextSlideData.url;
            }
        })
        .catch(err => {
            console.log("Something went wrong: " + err);
        });
}

function getRangeAnswer(): string[] {
    let selectedAnswers: string[] = [];
    if (sliderElement.value) {
        selectedAnswers.push(sliderElement.value);
    }
    return selectedAnswers;
}

function moveRangeButton(cubeId: number, direction: 'up' | 'down') {
    rangeInput.focus()
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
        moveRangeButton: (cubeId: number, direction: 'up' | 'down') => void;
        postAnswer: (CubeId: number, action: 'submit' | 'skip') => void;
    }
}
window.slideType = "RangeQuestion";
window.moveRangeButton = moveRangeButton;
window.postAnswer = postAnswer;
