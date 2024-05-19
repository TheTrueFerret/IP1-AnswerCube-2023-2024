import {RemoveLastDirectoryPartOf} from "../../urlDecoder";
import {getCookie} from "../../CookieHandler";

const slideElement: HTMLElement | null = document.getElementById("slide");
var url = window.location.toString();
const jwtToken = getCookie("jwtToken");
const baseUrl = "https://storage.cloud.google.com/answer-cube-bucket/";


function loadOpenQuestionSlide() {
    fetch(RemoveLastDirectoryPartOf(url) + "/GetNextSlide/", {
        method: "GET",
        headers: {
            "Accept": "application/json",
            "Authorization": `Bearer ${jwtToken}`
        }
    }).then((response: Response) => {
        if (response.status === 200) {
            return response.json();
        } else {
            if (slideElement) {
                slideElement.innerHTML = "<em>problem!!!</em>";
            }
        }
    }).then((slide: any) => {
        console.log(slide);
        if (slideElement) {
            slideElement.innerHTML = `<h4> ${slide.text} </h4>`;
            if (slide.mediaUrl) { // Check if mediaUrl exists
                // Extract the filename from the media URL
                let filename = slide.mediaUrl.split('/').pop();
                // Extract the media type from the filename
                let mediaType = filename.split('_')[0];
                console.log(mediaType);
                // Default to "image" if the media type is not "video"
                if (mediaType === "video") {
                    slideElement.innerHTML += `<video width="320" height="240" controls>
                                                  <source src="${slide.mediaUrl}" type="video/mp4">
                                                  Your browser does not support the video tag.
                                                </video><br>`;

                } else if (mediaType === "image") {
                    slideElement.innerHTML += `<img src="${slide.mediaUrl}" alt="Slide Image">`;
                } else {
                    slideElement.innerHTML += `<em>Unsupported media type</em>`;
                }
            }
            slideElement.innerHTML += `<input type="text" id="input" value="" placeholder="Answer the question.">`;
        }
    }).catch((error: any) => {
        console.error(error);
        if (slideElement) {
            slideElement.innerHTML = "<em>Problem loading the slide</em>";
        }
    });
}

loadOpenQuestionSlide()


const btn: HTMLElement | null = document.getElementById("submitAnswer");
if (btn) {
    btn.addEventListener('click', function () {
        postAnswer(1)
    });

    function postAnswer(cubeId: number) {
        let answer = getTextInput();

        let requestBody = {
            Answer: answer,
            CubeId: cubeId
        };
        console.log(requestBody);
        fetch(RemoveLastDirectoryPartOf(url) + "/PostAnswer", {
            method: "POST",
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json',
                "Authorization": `Bearer ${jwtToken}`
            },
            body: JSON.stringify(requestBody)
        }).then((response: Response) => {
            if (response.status === 200) {
                return response.json();
            } else {
                if (slideElement) {
                    slideElement.innerHTML = "<em>problem!!!</em>";
                }
            }
        }).then((nextSlideData: any) => {
            if (nextSlideData.url) {
                // Redirect to the URL of the next slide
                window.location.href = nextSlideData.url;
            }
        }).catch(err => {
            console.log("Something went wrong: " + err);
        })
        console.log(answer);
    }

    function getTextInput() {
        let selectedAnswers: string[] = [];

        //Get the value of the text input
        const textInput = document.querySelector('input[type="text"]#input');
        const textbox = textInput as HTMLInputElement; // Assert type to HTMLInputElement
        if (textbox.value) {
            selectedAnswers.push(textbox.value); // Use value property instead of nodeValue
        }
        return selectedAnswers;
    }


    document.addEventListener('keydown', (event) => {
        switch (event.key) {
            case 'Enter':
                console.log('Enter');
                postAnswer(1)
                break;
            default:
                console.log(event.key, event.keyCode);
                return;
        }
        event.preventDefault();
    });
}
