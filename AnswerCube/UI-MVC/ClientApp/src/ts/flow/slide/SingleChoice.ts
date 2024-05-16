import {RemoveLastDirectoryPartOf} from "../../urlDecoder";
import {getCookie} from "../../CookieHandler";

var url = window.location.toString()
const slideElement: HTMLElement | null = document.getElementById("slide");
const jwtToken = getCookie("jwtToken");

function loadSingleChoiceSlide() {
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
    btn.addEventListener('click', postAnswer);
}

function postAnswer() {
    let answer = getSelectedAnswer();

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

function getSelectedAnswer() {
    const checkboxes = document.querySelector('input[name="answer"]:checked');
    let selectedAnswers: string[] = [];
    const checkbox = checkboxes as HTMLInputElement; // Assert type to HTMLInputElement
    if (checkbox.value) {
        selectedAnswers.push(checkbox.value); // Use value property instead of nodeValue
    }
    return selectedAnswers;
}
