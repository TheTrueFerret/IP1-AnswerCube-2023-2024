import {RemoveLastDirectoryPartOf} from "../../site";

function InitializeFlow() {
    const slideElement = document.getElementById("slide");
    var url = window.location.toString()
    console.log(url + '|' + RemoveLastDirectoryPartOf(url) )
    //fetch(RemoveLastDirectoryPartOf(url) + "/GetNextSlide/", {
    fetch("http://localhost:5104/CircularFlow/InitializeFLow/", {
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
    }).then((data: any) => {
        
    }).catch((error: any) => {
        console.error(error);
        if (slideElement) {
            slideElement.innerHTML = "<em>Problem loading the slide</em>";
        }
    });
}
InitializeFlow()