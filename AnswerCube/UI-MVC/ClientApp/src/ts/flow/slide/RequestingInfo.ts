import {generateVoteTables, updateVoteUi} from "../VoteTableHandler";
import {postAnswers, startSession, stopSession} from "../CircularFlow";
import {getDomainFromUrl, RemoveLastDirectoryPartOf} from "../../urlDecoder";


const slideElement: HTMLElement | null = document.getElementById("slide");
let url = window.location.toString()

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
            generateVoteTables();
            for (let i: number = 0; i < activeCubes.length; i++) {
                voteStatePerCubeId[activeCubes[i]] = "none";
            }
        }
    }).catch(err => {
        console.log("Something went wrong: " + err);
        return err; // Return an empty array in case of error
    });
})

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