import {RemoveLastDirectoryPartOf} from "../site";


function UpdatePage() {
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
    UpdatePage();
}

