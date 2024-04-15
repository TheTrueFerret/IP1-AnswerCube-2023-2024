import {ActiveSlideList, currentSlideIndex} from "../CircularFlow";

const slide = document.getElementById("slide");

async function GetSingleChoiceSlide() {
    const currentSlide = ActiveSlideList[currentSlideIndex]; // Haal de huidige dia op basis van de huidige index
    fetch(`http://localhost:5104/api/SingleChoices/${currentSlide}`,
        {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        })
        .then(response => {
            if (response.status === 200) {
                if (slide) {
                    slide.innerHTML = `<em>IT WORKS!!!! + ${currentSlide} </em>`;
                } else {
                    console.error("Slide element not found!");
                }
                return response.json();
            } else {
                if (slide) {
                    slide.innerHTML = `<em>OUT OF SLIDES!!! + ${currentSlide} </em>`;
                } else {
                    console.error("Slide element not found!");
                }
            }
        })
        .then(data => {
            console.log(data);
            const slideElement = document.getElementById("slide");
            if (slideElement) {
                slideElement.innerHTML = `<h3>${data.text}</h3>`;
                for (const answer of data.answerList) {
                    slideElement.innerHTML += `<input type="radio" id="input" value="${answer}" name="answer">${answer}<br>`;
                }
            }
        })
}

GetSingleChoiceSlide()


