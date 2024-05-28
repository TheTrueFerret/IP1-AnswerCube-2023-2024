import {RemoveLastDirectoryPartOf} from "../../urlDecoder";
import {generateVoteTables, updateVoteUi} from "../VoteTableHandler";
import {postAnswers} from "../CircularFlow";

const slideElement: HTMLElement | null = document.getElementById("slide");
let url = window.location.toString()
const baseUrl = "https://storage.cloud.google.com/answer-cube-bucket/";


let activeCubes: number[] = []; // get active cubes
let sessionCube: boolean[] = [];
let voteStatePerCubeId: string[] = [];


document.addEventListener("DOMContentLoaded", function (){
    fetch(RemoveLastDirectoryPartOf(url) + "/GetActiveSessionsFromInstallation/", {
        method: "GET",
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
        },
    }).then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return response.json();
    }).then(data => {
        if (data.length > 0) {
            for (let i: number = 0; i < data.length; i++) {
                activeCubes[i] = data[i]
                sessionCube[i] = true
            }
            console.log(data);
            generateVoteTables(activeCubes, voteStatePerCubeId);
        }
    }).catch(err => {
        console.log("Something went wrong: " + err);
        return err; // Return an empty array in case of error
    });
})


function vote(cubeId: number, action: 'submit' | 'skip' | 'changeSubTheme') {
    
    for (let i = 0; i <= activeCubes.length; i++) {
        if (activeCubes[i] == cubeId) {
            if (voteStatePerCubeId[i] == "none") {
                voteStatePerCubeId[i] = action;
            } else {
                if (voteStatePerCubeId[i] != action) {
                    voteStatePerCubeId[i] = action
                }
            }
        }
    }
    updateVoteUi(cubeId, "SkipTable", false)
    updateVoteUi(cubeId, "", false)

    switch (action) {
        case "submit":
            updateVoteUi(cubeId, "SkipTable", true)
            break;
        case "skip":
            updateVoteUi(cubeId, "SkipTable", true)
            break;
        case "changeSubTheme":
            updateVoteUi(cubeId, "", true)
            break;
    }

    const allAnswered: boolean = voteStatePerCubeId.every(vote => vote !== "none");
    if (allAnswered) {
        skipQuestion();
        console.log("Everyone voted!");
    } else {
        console.log("Not Everyone Voted Yet");
    }
}


function skipQuestion() {
    fetch(RemoveLastDirectoryPartOf(url) + "/UpdatePage/", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
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



declare global {
    interface Window {
        slideType: string;
        vote: (cubeId: number, action: 'submit' | 'skip' | 'changeSubTheme') => void;
        skipQuestion: () => void;
    }
}

window.slideType = "InfoSlide";
window.vote = vote;
window.skipQuestion = skipQuestion;

