import { RemoveLastDirectoryPartOf } from "../../urlDecoder";

// Functie om de schuifvraag-slide te laden
function loadRangeQuestionSlide() {
    const slideElement: HTMLElement | null = document.getElementById("slide");
    var url = window.location.toString();
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
