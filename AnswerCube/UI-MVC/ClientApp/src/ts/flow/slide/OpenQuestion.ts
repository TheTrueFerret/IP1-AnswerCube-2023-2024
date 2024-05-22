import {RemoveLastDirectoryPartOf} from "../../urlDecoder";
import {getCookie} from "../../CookieHandler";

const slideElement: HTMLElement | null = document.getElementById("slide");
var url = window.location.toString();
const baseUrl = "https://storage.cloud.google.com/answer-cube-bucket/";

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
        postAnswer: (CubeId: number, action: 'submit' | 'skip') => void;
    }
}
window.slideType = "OpenQuestion";
window.postAnswer = postAnswer;

