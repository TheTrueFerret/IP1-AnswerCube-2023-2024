import {RemoveLastDirectoryPartOf} from "../../urlDecoder";
import {getCookie} from "../../CookieHandler";

var url = window.location.toString()
const slideElement: HTMLElement | null = document.getElementById("slide");

/*const checkboxes: any = document.querySelectorAll('input[name="answer"]')
var currentCheckedIndex: number = -1;
const totalCheckboxes: number = checkboxes.length;*/

const table = document.getElementById("AnswerTable") as HTMLTableElement | null;
const headerRow = document.getElementById("HeaderRow") as HTMLTableRowElement | null;

var currentCheckedIndexPerUser: number[] = [];
var totalQuestions: number;

function generateAnswerColumns(numberOfSessions: number) {
    if (table) {
        if (headerRow) {
            for (let i = 0; i < numberOfSessions; i++) {
                const newHeaderCell = document.createElement("th");
                newHeaderCell.textContent = "User" + (1 + i); // Adjust this to your needs
                headerRow.insertBefore(newHeaderCell, headerRow.children[i]);
                currentCheckedIndexPerUser[i] = -1;
            }
        }

        // Add new cells to each existing row (excluding the header)
        for (let rowIndex = 1; rowIndex < table.rows.length; rowIndex++) {
            const row = table.rows[rowIndex];
            for (let i = 0; i < numberOfSessions; i++) {
                const newCell = row.insertCell(i);
                newCell.innerHTML = `<div id="User${(1 + i)}_Row${rowIndex}" data-checked="false" data-user="${(1 + i)}" data-row="${rowIndex}"></div>`;
            }
        }
        totalQuestions = table.rows.length - 1;
    }
}
generateAnswerColumns(2)

function postAnswer(cubeId: number, action: 'submit' | 'skip') {
    let answer = getSelectedAnswer(cubeId);
    
    if (action === 'submit' && answer.length === 0) {
        console.log('No answers selected');
        // Show error to the user, e.g., alert or some UI indication
        alert('Please select at least one answer before submitting <3');
        return;
    }
    
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

function getSelectedAnswer(cubeId: number): string[] {
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
        for (let i = 1; i < totalQuestions; i++) {
            let elementId = `User${CubeId}_Row${i}`;
            let element = document.getElementById(elementId);
            if (element) {
                if (currentCheckedIndexPerUser[CubeId] === -1 && direction === 'up') {
                    element.setAttribute('data-checked', 'true');
                    currentCheckedIndexPerUser[CubeId] = 0;
                    element.innerHTML = 'Selected';
                    return;
                }
                if (currentCheckedIndexPerUser[CubeId] === -1 && direction === 'down') {
                    element.setAttribute('data-checked', 'true');
                    currentCheckedIndexPerUser[CubeId] = totalQuestions;
                    element.innerHTML = 'Selected';
                    return;
                }

                let newIndex;
                if (direction === 'up') {
                    newIndex = i - 1;
                    if (newIndex < 0) newIndex = totalQuestions - 1;
                } else if (direction === 'down') {
                    newIndex = i + 1;
                    if (newIndex >= totalQuestions) newIndex = 0;
                } else {
                    return; // Invalid direction
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
            }
        }
    }
    
    
    
    
    
    
    
    
    
    
    /*// Check if there's a radio button checked
    if (currentCheckedIndex === -1) {
        checkboxes[0].checked = true;
        currentCheckedIndex = 0
        return;
    }
    let newIndex;
    if (direction === 'up') {
        newIndex = currentCheckedIndex - 1;
        if (newIndex < 0) newIndex = totalCheckboxes - 1;
    } else if (direction === 'down') {
        newIndex = currentCheckedIndex + 1;
        if (newIndex >= totalCheckboxes) newIndex = 0;
    } else {
        return; // Invalid direction
    }

    checkboxes[currentCheckedIndex].checked = false;
    checkboxes[newIndex].checked = true;
    currentCheckedIndex = newIndex*/
}

declare global {
    interface Window {
        slideType: string;
        moveCheckedRadioButton: (CubeId: number, direction: 'up' | 'down') => void;
        postAnswer: (CubeId: number, action: 'submit' | 'skip') => void;
    }
}

window.slideType = "SingleChoice";
window.moveCheckedRadioButton = moveCheckedRadioButton;
window.postAnswer = postAnswer;

