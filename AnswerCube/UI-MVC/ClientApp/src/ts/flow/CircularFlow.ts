import {Obj} from "@popperjs/core";

let maxSlide: number;
export let currentSlideIndex: number = 1;
export let ActiveSlideList: any[];

function GetNextSlideList(slideListId: number) {
    fetch("http://localhost:5104/CircularFlow/InitializeFlow/", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ slideListId: slideListId })
    })
        .then((response: Response) => {
            if (response.status === 200) {
                return response.json();
            } else {
                const slideElement = document.getElementById("slide");
                if (slideElement) {
                    slideElement.innerHTML = "<em>problem!!!</em>";
                }
            }
        }).then((SlideList: any) => {
        ActiveSlideList = SlideList;
        maxSlide = SlideList.slides.length;
    })
        .catch((error: any) => {
            console.error(error);
            const slideElement = document.getElementById("slide");
            if (slideElement) {
                slideElement.innerHTML = "<em>Problem loading the slide</em>";
            }
        });
}

GetNextSlideList(1);

function UpdatePage() {
    fetch("http://localhost:5104/CircularFlow/NextSlide/", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({currentSlideIndex: currentSlideIndex, slideListId: ActiveSlideList})
    })
        .then((response: Response) => {
            if (response.status === 200) {
                return response.json();
            } else {
                const slideElement = document.getElementById("slide");
                if (slideElement) {
                    slideElement.innerHTML = "<em>problem!!!</em>";
                }            }
        }).then((slideInfo: any) => {
        if (slideInfo.url) {
            // Redirect to the URL of the next slide
            window.location.href = slideInfo.url;
        } else {
            const slideElement = document.getElementById("slide");
            if (slideElement) {
                slideElement.innerHTML = "<em>Next slide URL not found</em>";
            }        }
    })
        .catch((error: any) => {
            console.error(error);
            const slideElement = document.getElementById("slide");
            if (slideElement) {
                slideElement.innerHTML = "<em>Problem loading the slide</em>";
            }        });
}

const btn: HTMLElement | null = document.getElementById("nextSlide");
if (btn) {
    btn.addEventListener('click', nextSlide);
}

function nextSlide() {
    UpdatePage();
    currentSlideIndex++;
}

