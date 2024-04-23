import {RemoveLastDirectoryPartOf} from "../urlDecoder";


function SkipQuestion() {
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


document.addEventListener('keydown', (event) => {
    switch (event.key) {
        case 'ArrowDown':
            console.log('ArrowDown');
            break;
        case 'ArrowUp':
            console.log('ArrowUp');
            break;
        case 'ArrowLeft':
            console.log('ArrowLeft');
            break;
        case 'ArrowRight':
            console.log('ArrowRight');
            break;
        default:
            console.log(event.key, event.keyCode);
            return;
    }
    event.preventDefault();
});


