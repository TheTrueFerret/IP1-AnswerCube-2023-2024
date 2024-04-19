import {RemoveLastDirectoryPartOf} from "../../urlDecoder";

function loadSingleChoiceSlide() {
    const slideElement: HTMLElement | null = document.getElementById("slide");
    var url = window.location.toString()
    fetch(RemoveLastDirectoryPartOf(url) + "/GetNextSlide/", {
        method: "GET",
        headers: {
            "Accept": "application/json"
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
    const slideElement: HTMLElement | null = document.getElementById("slide");
    let answer = getSelectedAnswers();

    let requestBody = {
        Answer: answer
    };
    var url = window.location.toString()
    console.log(requestBody);
    fetch(RemoveLastDirectoryPartOf(url) + "/CircularFlow/PostAnswer", {
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
    }).then((slideData: any) => {
        if (slideData.url) {
            // Redirect to the URL of the next slide
            window.location.href = slideData.url;
        } else {
            if (slideElement) {
                slideElement.innerHTML = "<em>Next slide URL not found</em>";
            }
        }
    }).catch(err => {
        console.log("Something went wrong: " + err);
    })
    console.log(answer);
}

function getSelectedAnswers() {
    const checkboxes = document.querySelectorAll('input[name="answer"]:checked');
    let selectedAnswers = [];
    if (checkboxes && checkboxes.length > 0) {
        checkboxes.forEach((checkbox) => {
                selectedAnswers.push(checkbox.nodeValue);
            }
        );
    }

    //Get the value of the text input
    const textInput = document.querySelector('input[type="text"]#input');
    if (textInput && textInput.nodeValue) {
        selectedAnswers.push(textInput.nodeValue);
    }
    return selectedAnswers;
}
