import {RemoveLastDirectoryPartOf} from "../../urlDecoder";


function loadMultipleChoiceSlide() {
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
