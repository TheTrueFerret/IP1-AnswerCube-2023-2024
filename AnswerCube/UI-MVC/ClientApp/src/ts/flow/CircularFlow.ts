import {RemoveLastDirectoryPartOf} from "../urlDecoder";

const slideElement: HTMLElement | null = document.getElementById("slide");

export function getCubeNameByCubeId(CubeId: number): string {
    switch (CubeId) {
        case(0):
            return "Cuber1";
        case(1):
            return "Cuber2";
        case(2):
            return "Cuber3";
        case(3):
            return "Cuber4";
        case(4):
            return "Cuber5";
    }
    return "null"
}


export function stopSession(cubeId: number) {
    let url: string = window.location.toString()

    fetch(RemoveLastDirectoryPartOf(url) + "/EndSession/", {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
        },
        body: JSON.stringify(cubeId)
    }).then((response: Response) => {
        if (response.status === 200) {
            return response.json();
        } else {
            if (slideElement) {
                slideElement.innerHTML = "<em>problem!!!</em>";
            }
        }
    }).catch(err => {
        console.log("Something went wrong: " + err);
    })
}


export function postAnswers(answers: any[]) {
    let requestBody = {
        Answers: answers
    };

    console.log(requestBody);
    let url: string = window.location.toString()
    fetch(RemoveLastDirectoryPartOf(url) + "/PostAnswer/", {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
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
}
