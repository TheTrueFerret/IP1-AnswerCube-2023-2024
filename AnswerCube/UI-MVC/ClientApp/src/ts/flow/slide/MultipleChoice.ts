import {RemoveLastDirectoryPartOf} from "../../urlDecoder";
import {getCookie} from "../../CookieHandler";

const slideElement: HTMLElement | null = document.getElementById("slide");
var url = window.location.toString();
const baseUrl = "https://storage.cloud.google.com/answer-cube-bucket/";

const checkboxes: any = document.querySelectorAll('input[name="answer"]')
var currentCheckedIndex: number = -1;
const totalCheckboxes: number = checkboxes.length;


function postAnswer(cubeId: number, action: 'submit' | 'skip') {
    let answer: string[] = getSelectedAnswers();

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

function getSelectedAnswers(): string[] {
    const checkboxes = document.querySelectorAll('input[name="answer"]:checked');
    let selectedAnswers: string[] = [];
    for (let i = 0; i < checkboxes.length; i++) {
        const checkbox = checkboxes[i] as HTMLInputElement; // Assert type to HTMLInputElement
        if (checkbox.value) {
            selectedAnswers.push(checkbox.value); // Use value property instead of nodeValue
        }
    }
    return selectedAnswers;
}


function moveSelectedButton(direction: 'up' | 'down') {
    // Check if there's a checkbox checked
    if (currentCheckedIndex === -1) {
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
    currentCheckedIndex = newIndex;
}

function selectButton() {
    checkboxes[currentCheckedIndex].checked = !checkboxes[currentCheckedIndex].checked;
}

declare global {
    interface Window {
        slideType: string;
        moveSelectedButton: (direction: 'up' | 'down') => void;
        selectButton: () => void;
        postAnswer: (CubeId: number, action: 'submit' | 'skip') => void;
    }
}
window.slideType = "MultipleChoice";
window.moveSelectedButton = moveSelectedButton;
window.selectButton = selectButton;
window.postAnswer = postAnswer;
