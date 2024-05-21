import {RemoveLastDirectoryPartOf} from "../urlDecoder";
import {getCookie} from "../CookieHandler";

const slideElement: HTMLElement | null = document.getElementById("slide");
var url: string = window.location.toString()

function SkipQuestion() {
    fetch(RemoveLastDirectoryPartOf(url) + "/UpdatePage/", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
        },
    }).then((response: Response) => {
        if (response.status === 200) {
            return response.json();
        } else {
            if (slideElement) {
                slideElement.innerHTML = "<em>problem!!!</em>";
            }
        }
    }).then((slideData: any) => {
        if (slideData.url) {
            // Redirect to the URL of the next slide
            window.location.href = slideData.url;
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

const btn: HTMLElement | null = document.getElementById("skipSlide");
if (btn) {
    btn.addEventListener('click', SkipQuestion);
}


