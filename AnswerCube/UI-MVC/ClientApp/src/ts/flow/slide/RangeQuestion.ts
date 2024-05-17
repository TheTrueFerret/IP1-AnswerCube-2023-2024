import {RemoveLastDirectoryPartOf} from "../../urlDecoder";
import {getCookie} from "../../CookieHandler";

let url = window.location.toString()
const slideElement: HTMLElement | null = document.getElementById("slide");
const jwtToken = getCookie("jwtToken");
const sliderElement: HTMLInputElement | null = document.getElementById("slider") as HTMLInputElement;
const baseUrl = "https://storage.cloud.google.com/answer-cube-bucket/";

let rangeInput: any;
let min: number;
let max: number;
let step: number;

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
                    // Extract the filename from the media URL
                    let filename = slide.mediaUrl.split('/').pop();
                    // Extract the media type from the filename
                    let mediaType = filename.split('_')[0];
                    console.log(mediaType);
                    // Default to "image" if the media type is not "video"
                    if (mediaType === "video") {
                        slideElement.innerHTML += `<video width="320" height="240" controls>
                                                  <source src="${slide.mediaUrl}" type="video/mp4">
                                                  Your browser does not support the video tag.
                                                </video><br>`;

                    } else if (mediaType === "image") {
                        slideElement.innerHTML += `<img src="${slide.mediaUrl}" alt="Slide Image">`;
                    } else {
                        slideElement.innerHTML += `<em>Unsupported media type</em>`;
                    }
                }
                const answersContainer = document.querySelector(".answers-container");
                if (answersContainer) {
                    fillSliderOptions(slide.answerList);
                }
            }
            rangeInput = document.querySelector<HTMLInputElement>('input[type="range"]');
            min = parseInt(rangeInput.min, 10);
            max = parseInt(rangeInput.max, 10);
            step = rangeInput.step ? parseInt(rangeInput.step, 10) : 1;
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

document.addEventListener('keydown', (event) => {
    switch (event.key) {
        case 'ArrowDown':
            console.log('ArrowDown');
            moveRangeButton('down')
            break;
        case 'ArrowUp':
            console.log('ArrowUp');
            moveRangeButton('up')
            break;
        case 'ArrowLeft':
            console.log('ArrowLeft');
            break;
        case 'ArrowRight':
            console.log('ArrowRight');
            break;
        case 'a' || 'A':
            console.log('a');
            break;
        case 's' || 'S':
            console.log('s');
            break;
        case 'd' || 'D':
            console.log('d');
            break;
        case 'f' || 'F':
            console.log('f');
            break;
        case 'g' || 'G':
            console.log('g');
            break;
        case 'h' || 'H':
            console.log('h');
            break;
        case 'Enter':
            console.log('Enter');
            postAnswer()
            break;
        default:
            console.log(event.key, event.keyCode);
            return;
    }
    event.preventDefault();
});


function moveRangeButton(direction: 'up' | 'down') {
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
