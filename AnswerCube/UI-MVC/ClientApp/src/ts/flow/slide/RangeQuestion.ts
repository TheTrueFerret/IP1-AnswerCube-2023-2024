import {RemoveLastDirectoryPartOf} from "../../urlDecoder";
import {getCookie} from "../../CookieHandler";

let url = window.location.toString()
const slideElement: HTMLElement | null = document.getElementById("slide");
const jwtToken = getCookie("jwtToken");
const sliderElement: HTMLInputElement | null = document.getElementById("slider") as HTMLInputElement;
const baseUrl = "https://storage.cloud.google.com/answer-cube-bucket/";


function loadRangeQuestionSlide() {
    fetch(RemoveLastDirectoryPartOf(url) + "/GetNextSlide/", {
        method: "GET",
        headers: {
            "Accept": "application/json",
            "Authorization": `Bearer ${jwtToken}`
        }
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
        .then((slide: any) => {
            console.log(slide);
            if (slideElement) {
                slideElement.innerHTML = `<h3>${slide.text}</h3>`;
                if (slide.mediaUrl) { // Check if mediaUrl exists
                    slideElement.innerHTML += `<img src="${baseUrl}${slide.mediaUrl}" alt="Slide Image">`;
                }
                const answersContainer = document.querySelector(".answers-container");
                if (answersContainer) {
                    fillSliderOptions(slide.answerList);
                }
            }
        })
        .catch((error: any) => {
            console.error(error);
            if (slideElement) {
                slideElement.innerHTML = "<em>Problem loading the slide</em>";
            }
        });
}

loadRangeQuestionSlide();

const btnSubmit: HTMLElement | null = document.getElementById("submitAnswer");
if (btnSubmit) {
    btnSubmit.addEventListener('click', postAnswer);
}

function postAnswer() {
    
    let answer = getSelectedAnswer()
    
    let requestBody = {
        Answer: answer
    };
    console.log(requestBody);
    fetch(RemoveLastDirectoryPartOf(url) + "/PostAnswer", {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json', 
            'Authorization': `Bearer ${jwtToken}`
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

function getSelectedAnswer() {
    if (sliderElement) {
        let selectedAnswers: string[] = [];
        if (sliderElement.value) {
            selectedAnswers.push(sliderElement.value); 
        }
        return selectedAnswers;
    }
}

function fillSliderOptions(options: any) {
    const tickmarkLabelsContainer = document.querySelector(".tickmark-labels");
    if (tickmarkLabelsContainer) {
        tickmarkLabelsContainer.innerHTML = ""; // Container leegmaken

        if (Array.isArray(options)) {
            options.forEach((option: any) => {
                const tickmarkLabel = document.createElement("div");
                tickmarkLabel.classList.add("tickmark-label");
                tickmarkLabel.textContent = option;
                tickmarkLabelsContainer.appendChild(tickmarkLabel);
            });
        }
    }
}


