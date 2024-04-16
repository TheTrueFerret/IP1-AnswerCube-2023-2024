import {currentSlide} from "../CircularFlow";

const slideElement = document.getElementById("slide");

let currentCurrentSlide: any = currentSlide;
async function LoadSingleChoiceSlide() {
    console.log(currentCurrentSlide);
    if (slideElement) {
        slideElement.innerHTML = `<h3>${currentCurrentSlide.Text}</h3>`;
        for (const answer of currentCurrentSlide.AnswerList) {
            slideElement.innerHTML += `<input type="radio" id="input" value="${answer}" name="answer">${answer}<br>`;
        }
    }
}

LoadSingleChoiceSlide()


