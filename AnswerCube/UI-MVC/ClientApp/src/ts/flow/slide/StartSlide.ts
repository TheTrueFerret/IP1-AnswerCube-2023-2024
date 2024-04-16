import {currentSlide} from "../CircularFlow";
const slideElement = document.getElementById("slide");


function InitializeFlow() {
    fetch("http://localhost:5104/CircularFlow/InitializeFLow/", {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        },
    })
        .then((response: Response) => {
            if (response.status === 200) {
                return response.json();
                console.log(currentSlide)
            } else {
                if (slideElement) {
                    slideElement.innerHTML = "<em>problem!!!</em>";
                }
            }
        })
        .then((data: any) => {
            
        })
        .catch((error: any) => {
            console.error(error);
            if (slideElement) {
                slideElement.innerHTML = "<em>Problem loading the slide</em>";
            }
        });
}

InitializeFlow()