import {RemoveLastDirectoryPartOf} from "../../urlDecoder";
import {getCookie} from "../../CookieHandler";

const slideElement: HTMLElement | null = document.getElementById("slide");
var url = window.location.toString()
const baseUrl = "https://storage.cloud.google.com/answer-cube-bucket/";

function loadInfoSlide() {
    const jwtToken = getCookie("jwtToken");
    fetch(RemoveLastDirectoryPartOf(url) + "/GetNextSlide/", {
        method: "GET",
        headers: {
            "Accept": "application/json",
            "Authorization": `Bearer ${jwtToken}`
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
            if (slide.mediaUrl) { // Check if mediaUrl exists
                slideElement.innerHTML += `<img src="${baseUrl}${slide.mediaUrl}" alt="Slide Image">`;
            }
        }
    }).catch((error: any) => {
        console.error(error);
        if (slideElement) {
            slideElement.innerHTML = "<em>Problem loading the slide</em>";
        }
    });
}
loadInfoSlide()


