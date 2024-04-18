
function loadRangeQuestionSlide() {
    const slideElement: HTMLElement | null = document.getElementById("slide");
    fetch("http://localhost:5104/CircularFlow/GetNextSlide/", {
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
                slideElement.innerHTML += `<input type="radio" id="input" value="${answers}" name="answer">${answers}<br>`;
            }

            // paste the html generating part here
        }
    }).catch((error: any) => {
        console.error(error);
        if (slideElement) {
            slideElement.innerHTML = "<em>Problem loading the slide</em>";
        }
    });
}
loadRangeQuestionSlide()