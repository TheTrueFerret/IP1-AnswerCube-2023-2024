import {RemoveLastDirectoryPartOf} from "../../urlDecoder";
import {getCookie} from "../../CookieHandler";

var url = window.location.toString()
const slideElement: HTMLElement | null = document.getElementById("slide");

const checkboxes: any = document.querySelectorAll('input[name="answer"]')
var currentCheckedIndex: number = -1;
const totalCheckboxes: number = checkboxes.length;

function postAnswer(cubeId: number, action: 'submit' | 'skip') {
    let answer = getSelectedAnswer();
    
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

function getSelectedAnswer(): string[] {
    const checkboxes = document.querySelectorAll('input[name="answer"]:checked');
    let selectedAnswers: string[] = [];
    checkboxes.forEach((checkbox: Element) => {
        const inputElement = checkbox as HTMLInputElement;
        if (inputElement.value) {
            selectedAnswers.push(inputElement.value);
        }
    });
    return selectedAnswers;
}

function moveCheckedRadioButton(direction: 'up' | 'down') {
    // Check if there's a radio button checked
    if (currentCheckedIndex === -1) {
        checkboxes[0].checked = true;
        currentCheckedIndex = 0
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

    checkboxes[currentCheckedIndex].checked = false;
    checkboxes[newIndex].checked = true;
    currentCheckedIndex = newIndex
}

declare global {
    interface Window {
        slideType: string;
        moveCheckedRadioButton: (direction: 'up' | 'down') => void;
        postAnswer: (CubeId: number, action: 'submit' | 'skip') => void;
    }
}

window.slideType = "SingleChoice";
window.moveCheckedRadioButton = moveCheckedRadioButton;
window.postAnswer = postAnswer;

