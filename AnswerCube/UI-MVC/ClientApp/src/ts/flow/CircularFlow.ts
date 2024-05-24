import {RemoveLastDirectoryPartOf} from "../urlDecoder";

const slideElement: HTMLElement | null = document.getElementById("slide");
var url = window.location.toString();


export function getSessions() {
    fetch(RemoveLastDirectoryPartOf(url) + "/PostAnswer", {
        method: "GET",
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
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
        return data;
    }).catch(err => {
        console.log("Something went wrong: " + err);
    })
}
