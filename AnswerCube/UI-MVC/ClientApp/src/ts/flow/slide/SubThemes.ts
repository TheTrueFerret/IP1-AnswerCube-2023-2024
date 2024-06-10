import {RemoveLastDirectoryPartOf} from "../../urlDecoder";
import {createVoteTable, generateVoteTables, updateVoteUi} from "../VoteTableHandler";
import {getCubeIconByCubeId, getCubeNameByCubeId, startSession, stopSession} from "../CircularFlow";
var url: string = window.location.toString()


let currentChosenSlideListPerUser: number[] = [];
let totalSlideLists: number;
let activeCubes: number[] = []; // get active cubes
let sessionCube: boolean[] = [];
let voteStatePerCubeId: string[] = [];


document.addEventListener("DOMContentLoaded", function () {
    getActiveSessions();
    
    for (let i = 0; i < activeCubes.length; i++) {
        currentChosenSlideListPerUser[i] = -1;
    }
    for (let i = 0; i < activeCubes.length; i++) {
        voteStatePerCubeId[activeCubes[i]] = "none";
    }

    const allSlideLists = document.querySelectorAll("#AllSlideLists .card");
    totalSlideLists = allSlideLists.length;

    generateSlideListVoteTables();
    generateVoteTables();
});

function getActiveSessions() {
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

        }
    }).catch(err => {
        console.log("Something went wrong: " + err);
        return err; // Return an empty array in case of error
    });
}


document.querySelectorAll('.ChooseSlideList').forEach((btnSubmit: Element) => {
    btnSubmit.addEventListener('click', function() {
        chooseSlideList(Number((btnSubmit as HTMLElement).getAttribute('data-info')));
    });
});


function addNewOrDeleteCubeUser(cubeId: number) {
    const index = activeCubes.indexOf(cubeId);
    if (index !== -1) {
        voteStatePerCubeId[cubeId] = "removed";
        activeCubes.splice(index, 1);
        updateVoteUi(cubeId, "SubmitTable", false)
        if (sessionCube[cubeId]) {
            sessionCube[cubeId] = false
            stopSession(cubeId);
        }
        moveBetweenSlideLists(cubeId, 'down');
    } else {
        voteStatePerCubeId[cubeId] = "none";
        activeCubes.push(cubeId); // Add cubeId to activeCubes if it doesn't already exist
        activeCubes.sort((a, b) => a - b);
        currentChosenSlideListPerUser[cubeId] = -1;
        if (!sessionCube[cubeId]) {
            sessionCube[cubeId] = true
            startSession(cubeId);
        }
        moveBetweenSlideLists(cubeId, "down");
    }
}


function generateSlideListVoteTables() {
    for (let i = 1; i <= totalSlideLists; i++) {
        let tableId: string = `SlideListUserAnswer ${i}`;
        createVoteTable(2, tableId);
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
    updateVoteUi(cubeId, "SubmitTable", false)
    updateVoteUi(cubeId, "SubmitTable", true)
    
    const allAnswered: boolean = voteStatePerCubeId.every(vote => vote !== "none");
    if (allAnswered) {
        const voteCounts: { [key: number]: number } = {};

        // Count the votes for each slidelist ID
        currentChosenSlideListPerUser.forEach(slideListId => {
            if (voteCounts[slideListId]) {
                voteCounts[slideListId]++;
            } else {
                voteCounts[slideListId] = 1;
            }
        });

        // Find the slidelist ID with the maximum votes
        let mostVotedSlideListId: number | null = null;
        let maxVotes = 0;

        for (const [id, count] of Object.entries(voteCounts)) {
            const numericId = parseInt(id, 10);
            if (count > maxVotes) {
                mostVotedSlideListId = numericId;
                maxVotes = count;
            }
        }
        if (mostVotedSlideListId)
        chooseSlideList(mostVotedSlideListId)
        console.log("Everyone voted!");
    } else if (voteStatePerCubeId){
        
    }
}


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

function getSelectedAnswerByCubeId(cubeId: number): number{
    let selectedAnswer: number = 0;
    for (let i: number = 1; i <= totalSlideLists; i++) {
        //const elementId: string = `SlideList${i}`;
        //const element = document.getElementById(elementId);
        if (i == currentChosenSlideListPerUser[cubeId]) {
            selectedAnswer = currentChosenSlideListPerUser[cubeId]
        }
    }
    return selectedAnswer;
}

function moveBetweenSlideLists(cubeId: number, direction: 'up' | 'down') {
    let cubeName: string = getCubeNameByCubeId(cubeId);
    let moved = false;

    for (let i = 1; i <= totalSlideLists && !moved; i++) {
        let elementId: string = `SlideListUserAnswer ${i}`;
        let element = document.getElementById(elementId);
        if (element) {
            let cells = element.querySelectorAll(`td[data-cube="${cubeId}"]`);
            if (currentChosenSlideListPerUser[cubeId] == i) {
                cells.forEach(cell => {
                    if (cell.getAttribute('data-active') === 'true') {
                        if (voteStatePerCubeId[cubeId] == "removed"){
                            cell.setAttribute('data-active', 'false');
                            cell.innerHTML = '';
                            moved = true;
                        } else {
                            let newIndex: number = 0;
                            if (direction === 'up') {
                                newIndex = (i - 1);
                                if (newIndex < 1) newIndex = totalSlideLists;
                            } else if (direction === 'down') {
                                newIndex = (i + 1);
                                if (newIndex > totalSlideLists) newIndex = 1;
                            }

                            let newElementId = `SlideListUserAnswer ${newIndex}`;
                            let newElement = document.getElementById(newElementId);

                            cell.setAttribute('data-active', 'false');
                            cell.innerHTML = '';

                            if (newElement) {
                                let newCells = newElement.querySelectorAll(`td[data-cube="${cubeId}"]`);
                                newCells.forEach(newCell => {
                                    newCell.setAttribute('data-active', 'true');
                                    let cubeIcon: String = getCubeIconByCubeId(cubeId)
                                    newCell.innerHTML = `<img src="${cubeIcon}" width="50" height="50"/>`;
                                });
                            }
                            currentChosenSlideListPerUser[cubeId] = newIndex;

                            moved = true;
                        }
                    }
                });
            }
            if (currentChosenSlideListPerUser[cubeId] === -1) {
                cells.forEach(cell => {
                    cell.setAttribute('data-active', 'true');
                    let cubeIcon: String = getCubeIconByCubeId(cubeId)
                    cell.innerHTML = `<img src="${cubeIcon}" width="50" height="50"/>`;
                });
                currentChosenSlideListPerUser[cubeId] = 1;
                moved = true;
            }
        }
    }
}


declare global {
    interface Window {
        slideType: string;
        addNewOrDeleteCubeUser: (cubeId: number) => void;
        moveBetweenSlideLists: (CubeId: number, direction: 'up' | 'down') => void;
        vote: (cubeId: number, action: 'submit' | 'skip' | 'changeSubTheme') => void;
    }
}

window.slideType = "SubThemes";
window.addNewOrDeleteCubeUser = addNewOrDeleteCubeUser;
window.moveBetweenSlideLists = moveBetweenSlideLists;
window.vote = vote;






