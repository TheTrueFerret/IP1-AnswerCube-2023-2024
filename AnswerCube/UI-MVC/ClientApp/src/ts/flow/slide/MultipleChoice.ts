import {RemoveLastDirectoryPartOf} from "../../urlDecoder";
import {getCookie} from "../../CookieHandler";

const slideElement: HTMLElement | null = document.getElementById("slide");
var url = window.location.toString();
const jwtToken = getCookie("jwtToken");
const baseUrl = "https://storage.cloud.google.com/answer-cube-bucket/";
function loadMultipleChoiceSlide() {
    fetch(RemoveLastDirectoryPartOf(url) + "/GetNextSlide/", {
        method: "GET",
        headers: {
            "Accept": "application/json",
            "Authorization": `Bearer ${jwtToken}`
        }
    }).then((response: Response) => {
        if (response.status === 200) {
            return response.json();
        } else {
            if (slideElement) {
                slideElement.innerHTML = "<em>problem!!!</em>";
            }
        }
    }).then((slide: any) => {
        console.log(slide);
        if (slideElement) {
            slideElement.innerHTML = `<h3> ${slide.text} </h3> `;
            if (slide.mediaUrl) { // Check if mediaUrl exists
                slideElement.innerHTML += `<img src="${baseUrl}${slide.mediaUrl}" alt="Slide Image">`;
            }
            for (const answers of slide.answerList) {
                slideElement.innerHTML += `<input type="checkbox" id="input" value="${answers}" name="answer">${answers}<br>`;
            }
        }
    }).catch((error: any) => {
        console.error(error);
        if (slideElement) {
            slideElement.innerHTML = "<em>Problem loading the slide</em>";
        }
    });
}
loadMultipleChoiceSlide()



const btn: HTMLElement | null = document.getElementById("submitAnswer");
if (btn) {
    btn.addEventListener('click', postAnswer);
}

function postAnswer() {
    let answer = getSelectedAnswers();
    
    let requestBody = {
        Answer: answer
    };
    console.log(requestBody);
    fetch(RemoveLastDirectoryPartOf(url) + "/PostAnswer", {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
            "Authorization": `Bearer ${jwtToken}`
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

function getSelectedAnswers() {
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

