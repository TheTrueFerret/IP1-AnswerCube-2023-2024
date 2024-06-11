import {generateVoteTables, updateVoteUi} from "../VoteTableHandler";
import {postAnswers, startSession, stopSession} from "../CircularFlow";
import {getDomainFromUrl, RemoveLastDirectoryPartOf} from "../../urlDecoder";


const slideElement: HTMLElement | null = document.getElementById("slide");
let url = window.location.toString()

let activeCubes: number[] = []; // get active cubes
let sessionCube: boolean[] = [];
let voteStatePerCubeId: string[] = [];


function addNewOrDeleteCubeUser(cubeId: number) {
    const index = activeCubes.indexOf(cubeId);
    if (index !== -1) {
        voteStatePerCubeId[cubeId] = "removed";
        activeCubes.splice(index, 1);
        updateVoteUi(cubeId, "SubmitTable", false);
        updateVoteUi(cubeId, "SkipTable", false);
        updateVoteUi(cubeId, "SubthemeTable", false);
        if (sessionCube[cubeId]) {
            sessionCube[cubeId] = false
            stopSession(cubeId);
        }
    } else {
        voteStatePerCubeId[cubeId] = "none";
        activeCubes.push(cubeId); // Add cubeId to activeCubes if it doesn't already exist
        activeCubes.sort((a, b) => a - b);
        if (!sessionCube[cubeId]) {
            sessionCube[cubeId] = true
            startSession(cubeId);
        }
    }
}


function vote(cubeId: number, action: 'submit' | 'skip' | 'changeSubTheme') {
    
    for (let i = 0; i <= activeCubes.length; i++) {
        if (activeCubes[i] == cubeId) {
            if (voteStatePerCubeId[cubeId] == "none") {
                voteStatePerCubeId[cubeId] = action;
            } else {
                if (voteStatePerCubeId[cubeId] != action) {
                    voteStatePerCubeId[cubeId] = action
                }
            }
        }
    }
    updateVoteUi(cubeId, "SkipTable", false)
    updateVoteUi(cubeId, "SubthemeTable", false)

    switch (action) {
        case "submit":
            updateVoteUi(cubeId, "SkipTable", true)
            break;
        case "skip":
            updateVoteUi(cubeId, "SkipTable", true)
            break;
        case "changeSubTheme":
            updateVoteUi(cubeId, "SubthemeTable", true)
            break;
    }

    if (action == "changeSubTheme") {
        window.location.href = getDomainFromUrl(window.location.toString()) + "/CircularFlow/Subthemes"
    } else {
        const allAnswered: boolean = voteStatePerCubeId.every(vote => vote !== "none");
        if (allAnswered) {
            skipQuestion();
            console.log("Everyone voted!");
        } else {
            console.log("Not Everyone Voted Yet");
        }
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
        moveCheckedRadioButton: (CubeId: number, direction: 'up' | 'down') => void;
        vote: (cubeId: number, action: 'submit' | 'skip' | 'changeSubTheme') => void;
        addNewOrDeleteCubeUser: (cubeId: number) => void;
        generateVoteTables: (activeCubes: number[], voteStatePerCubeId: string[]) => void;
    }
}

window.slideType = "LeaveContactInfo";
window.vote = vote;
window.addNewOrDeleteCubeUser = addNewOrDeleteCubeUser
window.generateVoteTables = generateVoteTables