import {RemoveLastDirectoryPartOf} from "../../urlDecoder";
import {getCookie} from "../../CookieHandler";

const slideElement: HTMLElement | null = document.getElementById("slide");
var url = window.location.toString();
const jwtToken = getCookie("jwtToken");
const baseUrl = "https://storage.cloud.google.com/answer-cube-bucket/";

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
            for (const answers of slide.answerList) {
                slideElement.innerHTML += `<input type="checkbox" id="input" value="${answers}" name="answer">${answers}<br>`;
            }
        }
        checkboxes = document.querySelectorAll('input[name="answer"]');
        currentCheckedIndex = -1;
        totalCheckboxes = checkboxes.length;
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
    btn.addEventListener('click', function() {
        postAnswer(1)
    });
}

function postAnswer(cubeId: number) {
    let answer = getSelectedAnswers();
    
    let requestBody = {
        Answer: answer,
        Session: cubeId
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
            
            postAnswer(1)
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

function checkButton() {
    checkboxes[currentCheckedIndex].checked = !checkboxes[currentCheckedIndex].checked;
}

