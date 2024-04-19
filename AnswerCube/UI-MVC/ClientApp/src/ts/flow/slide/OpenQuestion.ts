import {RemoveLastDirectoryPartOf} from "../../urlDecoder";

function loadOpenQuestionSlide() {
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
            slideElement.innerHTML = `<h4> ${slide.text} </h4>`;
            slideElement.innerHTML += `<input type="text" id="input" value="" placeholder="Answer the question.">`;
        }
    }).catch((error: any) => {
        console.error(error);
        if (slideElement) {
            slideElement.innerHTML = "<em>Problem loading the slide</em>";
        }
    });
}
loadOpenQuestionSlide()



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
    fetch("http://localhost:5104/CircularFlow/PostAnswer", {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
        },
        body: JSON.stringify(requestBody)
    }).then(res => {
        console.log(res)
        if (res.ok) {
            if (res.status === 201) {
                return res.json();
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

