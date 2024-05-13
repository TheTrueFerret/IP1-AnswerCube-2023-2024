import {RemoveLastDirectoryPartOf} from "../../urlDecoder";
import {getCookie} from "../../CookieHandler";

const slideElement: HTMLElement | null = document.getElementById("slide");
var url = window.location.toString();
const jwtToken = getCookie("jwtToken");

var checkboxes : any;
var currentCheckedIndex: number;
var totalCheckboxes: number;

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


document.addEventListener('keydown', (event) => {
    switch (event.key) {
        case 'ArrowDown':
            console.log('ArrowDown');
            moveSelectedButton('down');
            break;
        case 'ArrowUp':
            console.log('ArrowUp');
            moveSelectedButton('up');
            break;
        case 'ArrowLeft':
            console.log('ArrowLeft');
            checkButton();
            break;
        case 'ArrowRight':
            console.log('ArrowRight');
            checkButton();
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


function moveSelectedButton(direction: 'up' | 'down') {
    // Check if there's a checkbox checked
    if (currentCheckedIndex === -1) {
        checkboxes[0].checked = true;
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

    checkboxes[currentCheckedIndex].checked = false;
    checkboxes[newIndex].checked = true;
    currentCheckedIndex = newIndex;
}

function checkButton() {
    checkboxes[currentCheckedIndex].checked = !checkboxes[currentCheckedIndex].checked;
}

