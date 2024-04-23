import { RemoveLastDirectoryPartOf } from "../../urlDecoder";

const slideElement: HTMLElement | null = document.getElementById("slide");
var url = window.location.toString();

function loadRangeQuestionSlide() {
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
                slideElement.innerHTML = "<em>Problem!!!</em>";
            }
        }
    }).then((slide: any) => {
        console.log(slide);
        if (slideElement) {
            slideElement.innerHTML = `<h3>${slide.text}</h3>`;
            const answersContainer = document.querySelector(".answers-container");
            if (answersContainer) {
                answersContainer.innerHTML = "";
            }
            fillSliderOptions(slide.answerList);
        }
    }).catch((error: any) => {
        console.error(error);
        if (slideElement) {
            slideElement.innerHTML = "<em>Problem loading the slide</em>";
        }
    });
}
loadRangeQuestionSlide()



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


// Functie om de opties van de schuifregelaar te vullen
function fillSliderOptions(options: any) {
    const tickmarkLabelsContainer = document.querySelector(".tickmark-labels");
    if (tickmarkLabelsContainer) {
        tickmarkLabelsContainer.innerHTML = ""; // Eerst de container leegmaken

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

// Eventlistener voor het laden van de DOM
document.addEventListener("DOMContentLoaded", function() {
    loadRangeQuestionSlide();
});
