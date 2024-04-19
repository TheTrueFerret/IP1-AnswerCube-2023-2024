import {RemoveLastDirectoryPartOf} from "../site";


function updatePage() {
    const slideElement: HTMLElement | null = document.getElementById("slide");
    console.log(window.location)
    var url = window.location.toString()
    fetch(RemoveLastDirectoryPartOf(url) + "/UpdatePage/", {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        },
    }).then((response: Response) => {
        if (response.status === 200) {
            return response.json();
        } else {
            if (slideElement) {
                slideElement.innerHTML = "<em>problem!!!</em>";
            }
        }
    }).then((slideInfo: any) => {
        if (slideInfo.url) {
            // Redirect to the URL of the next slide
            window.location.href = slideInfo.url;
        } else {
            if (slideElement) {
                slideElement.innerHTML = "<em>Next slide URL not found</em>";
            }
        }
    }).catch((error: any) => {
        console.error(error);
        if (slideElement) {
            slideElement.innerHTML = "<em>Problem loading the next slide</em>";
        }
    });
}


const btn: HTMLElement | null = document.getElementById("nextSlide");
if (btn) {
    btn.addEventListener('click', nextSlide);
}

function nextSlide() {
    postAnswer();
    updatePage();
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
    if (textInput&& textInput.nodeValue) {
        selectedAnswers.push(textInput.nodeValue);
    }
    return selectedAnswers;
}

