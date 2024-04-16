export let currentSlide: any;
const slideElement = document.getElementById("slide");


type slide = {
    Id: number;
    Text: string;
    SlideType: number;
    AnswerList: string[];
}

function getNextSlide() {
    fetch("http://localhost:5104/CircularFlow/GetNextSlide/", {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        },
    })
        .then((response: Response) => {
            if (response.status === 200) {
                return response.json();
            } else {
                if (slideElement) {
                    slideElement.innerHTML = "<em>problem!!!</em>";
                }
            }
        })
        .then((Slide: any) => {
            currentSlide = Slide
        })
        .catch((error: any) => {
            console.error(error);
            if (slideElement) {
                slideElement.innerHTML = "<em>Problem loading the slide</em>";
            }
        });
}

getNextSlide();



function UpdatePage() {
    fetch("http://localhost:5104/CircularFlow/UpdatePage/", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({slideList: currentSlide})
        })
        .then((response: Response) => {
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
        })
        .catch((error: any) => {
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

