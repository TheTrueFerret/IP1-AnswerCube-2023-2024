import {RemoveLastDirectoryPartOf} from "../../urlDecoder";
import {getCookie} from "../../CookieHandler";

var url = window.location.toString()
const slideElement: HTMLElement | null = document.getElementById("slide");

var checkboxes : any;
var currentCheckedIndex: number;
var totalCheckboxes: number;

function loadSingleChoiceSlide() {
    fetch(RemoveLastDirectoryPartOf(url) + "/GetNextSlide/", {
        method: "GET",
        headers: {
            "Accept": "application/json",
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
                }}
            for (const answer of slide.answerList) {
                slideElement.innerHTML += `<input type="radio" id="input" value="${answer}" name="answer">${answer}<br>`;
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

loadSingleChoiceSlide()


const btn: HTMLElement | null = document.getElementById("submitAnswer");
if (btn) {
    btn.addEventListener('click', function(){
        postAnswer(1)
    });
}

function postAnswer(cubeId: number) {
    let answer = getSelectedAnswer();

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

function getSelectedAnswer() {
    const checkboxes = document.querySelector('input[name="answer"]:checked');
    let selectedAnswers: string[] = [];
    const checkbox = checkboxes as HTMLInputElement; // Assert type to HTMLInputElement
    if (checkbox.value) {
        selectedAnswers.push(checkbox.value); // Use value property instead of nodeValue
    }
    return selectedAnswers;
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
            postAnswer(1)
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



