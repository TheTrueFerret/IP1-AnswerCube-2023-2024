import {RemoveLastDirectoryPartOf} from "../../urlDecoder";
import {setCookie} from "../../CookieHandler";

const slideElement = document.getElementById("slide");
var url = window.location.toString()
function InitializeFlow() {
    fetch(RemoveLastDirectoryPartOf(url) + "/InitializeFLow/", {
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
        console.log(data)
        if (data && data.token) {
            // Save token in a cookie
            setCookie("jwtToken", data.token, 7); // Change 1 to the number of days you want the cookie to last
        }
    }).catch((error: any) => {
        console.error(error);
        if (slideElement) {
            slideElement.innerHTML = "<em>Problem loading the slide</em>";
        }
    });
}
InitializeFlow()


document.addEventListener('keydown', (event) => {
    switch (event.key) {

        case 'Enter':
            console.log('Enter');
            InitializeFlow()
            break;
        default:
            console.log(event.key, event.keyCode);
            return;
    }
    event.preventDefault();
});