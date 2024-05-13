import {RemoveLastDirectoryPartOf} from "../../urlDecoder";
import {getCookie} from "../../CookieHandler";

let url = window.location.toString()
const slideElement: HTMLElement | null = document.getElementById("slide");
const jwtToken = getCookie("jwtToken");
const sliderElement: HTMLInputElement | null = document.getElementById("slider") as HTMLInputElement;

var checkboxes : any;
var currentCheckedIndex: number;
var totalCheckboxes: number;


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

document.addEventListener('keydown', (event) => {
    switch (event.key) {
        case 'ArrowDown':
            console.log('ArrowDown');
            moveCheckedRadioButton('down')
            break;
        case 'ArrowUp':
            console.log('ArrowUp');
            moveCheckedRadioButton('up')
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
