import {RemoveLastDirectoryPartOf} from "../../urlDecoder";
import {getCookie} from "../../CookieHandler";
var url: string = window.location.toString()


function chooseSlideList(slideListId: number) {
    let requestBody = {
        Id: slideListId
    };
    console.log(requestBody);
    fetch(RemoveLastDirectoryPartOf(url) + "/ChooseSlideList", {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
        },
        body: JSON.stringify(requestBody)
    }).then((response: Response) => {
        if (response.status === 200) {
            return response.json();
        }
    }).then((nextSlideData: any) => {
        if (nextSlideData.url) {
            // Redirect to the URL of the next slide
            window.location.href = nextSlideData.url;
        }
    }).catch(err => {
        console.log("Something went wrong: " + err);
    })
}

document.querySelectorAll('.ChooseSlideList').forEach((btnSubmit: Element) => {
    btnSubmit.addEventListener('click', function() {
        chooseSlideList(Number((btnSubmit as HTMLElement).getAttribute('data-info')));
    });
});




