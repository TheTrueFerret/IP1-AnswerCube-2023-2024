import {RemoveLastDirectoryPartOf} from "../../urlDecoder";
import {getCookie} from "../../CookieHandler";
import {types} from "sass";
import Number = types.Number;

var url = window.location.toString()
const slideElement: HTMLElement | null = document.getElementById("slide");



const table = document.getElementById("AnswerTable") as HTMLTableElement | null;
const headerRow = document.getElementById("HeaderRow") as HTMLTableRowElement | null;

var currentCheckedIndexPerUser: number[] = [];
var totalQuestions: number;
var totalActiveCubes: number; // get active cubes

const keyboardPlayer = true;

var voteState: string[];

function generateAnswerColumns(totalActiveCubes: number) {
    if (keyboardPlayer) {
        totalActiveCubes++;
    }
    if (table) {
        if (headerRow) {
            for (let i = 0; i < totalActiveCubes; i++) {
                const newHeaderCell = document.createElement("th");
                newHeaderCell.textContent = "User" + (i); // Adjust this to your needs
                headerRow.insertBefore(newHeaderCell, headerRow.children[i]);
                currentCheckedIndexPerUser[i] = -1;
            }
        }

        // Add new cells to each existing row (excluding the header)
        for (let rowIndex = 1; rowIndex < table.rows.length; rowIndex++) {
            const row = table.rows[rowIndex];
            for (let i = 0; i < totalActiveCubes; i++) {
                const newCell = row.insertCell(i);
                newCell.innerHTML = `<div id="User${(i)}_Row${rowIndex}" data-checked="false" data-user="${(i)}" data-row="${rowIndex}"></div>`;
            }
        }
        totalQuestions = table.rows.length - 1;
    }
}
generateAnswerColumns(2)


function addNewCubeColumn(CubeId: number) {
    if (table) {
        if (headerRow) {
            const newHeaderCell = document.createElement("th");

            switch (CubeId) {
                case(0):
                    newHeaderCell.textContent = "KeyboardPlayer";
                    break;
                case(1):
                    newHeaderCell.textContent = "Fret";
                    break;
                case(2):
                    newHeaderCell.textContent = "Destiny";
                    break;
                case(3):
                    newHeaderCell.textContent = "Rider";
                    break;
                case(4):
                    newHeaderCell.textContent = "Striker";
                    break;
            }
            
            headerRow.insertBefore(newHeaderCell, headerRow.children[CubeId]);
            currentCheckedIndexPerUser[CubeId] = -1;
        }

        // Add new cells to each existing row (excluding the header)
        for (let rowIndex = 1; rowIndex < table.rows.length; rowIndex++) {
            const row = table.rows[rowIndex];
            const newCell = row.insertCell(CubeId);
            newCell.innerHTML = `<div id="User${(CubeId)}_Row${rowIndex}" data-checked="false" data-user="${(CubeId)}" data-row="${rowIndex}"></div>`;
        }
        totalQuestions = table.rows.length - 1;
    }
}



function vote(cubeId: number, action: 'submit' | 'skip') {
    let answer = getSelectedAnswerByCubeId(cubeId)
    
    if (action === 'submit' && answer.length === 0) {
        console.log('No answers selected');
        // Show error to the user, e.g., alert or some UI indication
        alert('Please select at least one answer before submitting <3');
        return;
    }
    
    
    
    
}


function postAnswers() {
    let answers: any[] = []
    
    for (let i = 0; i < totalActiveCubes; i++) {
        answers.push({
            Answer: getSelectedAnswerByCubeId(i),
            CubeId: i
        })
    }

    let requestBody = {
        Answers: answers
    };
    
    console.log(requestBody);
    fetch(RemoveLastDirectoryPartOf(url) + "/PostAnswer", {
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

//TODO
function getSelectedAnswerByCubeId(cubeId: number): string[] {
    const checkboxes = document.querySelectorAll('input[name="answer"]:checked');
    let selectedAnswers: string[] = [];
    checkboxes.forEach((checkbox: Element) => {
        const inputElement = checkbox as HTMLInputElement;
        if (inputElement.value) {
            selectedAnswers.push(inputElement.value);
        }
    });
    return selectedAnswers;
}


function moveCheckedRadioButton(CubeId: number, direction: 'up' | 'down') {
    if (table) {
        for (let i = 1; i <= totalQuestions; i++) {
            let elementId = `User${CubeId}_Row${i}`;
            let element = document.getElementById(elementId);
            if (element) {
                if (currentCheckedIndexPerUser[CubeId] === -1) {
                    element.setAttribute('data-checked', 'true');
                    currentCheckedIndexPerUser[CubeId] = 1;
                    element.innerHTML = 'Selected';
                    return;
                }

                let newIndex;
                if (currentCheckedIndexPerUser[CubeId] === i && direction === 'up') {
                    newIndex = (i - 1);
                    if (newIndex < 1) newIndex = totalQuestions;
                } else if (currentCheckedIndexPerUser[CubeId] === i && direction === 'down') {
                    newIndex = (i + 1);
                    if (newIndex >= totalQuestions) newIndex = 1;
                } else {
                    continue;
                }
                
                let newElementId = `User${CubeId}_Row${newIndex}`;
                let newElement = document.getElementById(newElementId);
                element.setAttribute('data-checked', 'false');
                element.innerHTML = '';
                if (newElement) {
                    newElement.setAttribute('data-checked', 'true');
                    newElement.innerHTML = 'Selected';
                }
                currentCheckedIndexPerUser[CubeId] = newIndex;
                return;
            }
        }
    }
}

declare global {
    interface Window {
        slideType: string;
        moveCheckedRadioButton: (CubeId: number, direction: 'up' | 'down') => void;
        postAnswers: (CubeId: number, action: 'submit' | 'skip') => void;
    }
}

window.slideType = "SingleChoice";
window.moveCheckedRadioButton = moveCheckedRadioButton;
window.postAnswers = postAnswers;

