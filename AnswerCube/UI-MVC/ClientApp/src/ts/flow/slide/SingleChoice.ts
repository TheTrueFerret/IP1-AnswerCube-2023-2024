import {RemoveLastDirectoryPartOf} from "../../urlDecoder";
import {getCookie} from "../../CookieHandler";

var url = window.location.toString()
const slideElement: HTMLElement | null = document.getElementById("slide");

var checkboxes: any;
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
                }
            }
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

function postAnswer(cubeId: number, action: 'submit' | 'skip') {
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



