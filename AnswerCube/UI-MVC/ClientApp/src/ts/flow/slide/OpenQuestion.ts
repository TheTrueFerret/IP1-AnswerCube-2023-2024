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

