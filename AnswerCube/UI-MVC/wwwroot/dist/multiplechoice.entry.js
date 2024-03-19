/******/ (() => { // webpackBootstrap
var __webpack_exports__ = {};
/*!*********************************************!*\
  !*** ./src/ts/flow/slide/MultipleChoice.ts ***!
  \*********************************************/
const tableBody = document.getElementById('MultipleQuestionTableBodyTableBody')
const nextButton = document.getElementById('nextButton')



// deze zijn misschien niet nodig (ge moogt er wel zelf 1 maken <3 jarno)
function getMultipleChoice() {
    fetch("http://localhost:5104/SlideController/Slide",
        {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        })
        .then(response => {
            if (response.status === 200) {
                return response.json();
            } else {
                document.getElementById("MultipleQuestionTableBody").innerHTML = "<em>Problem!!!</em>";
            }
        })
        .then(slide => {
            console.log(slide);
            document.getElementById("MultipleQuestionTableBody").innerHTML += `<h1>${slide.id}</h1><h2>${slide.text}</h2>`;
        })
}



async function retrieveAstronauts() {
    const response = await fetch('/api/astronauts', {
        headers: {
            Accept: 'application/json'
        }
    })
    if (response.status === 200) {
        tableBody.innerHTML = ''
        /**
         * @type {[{id: number, name: string, age: int, dateOfBirth: string, missionsCompleted: int}]}
         */
        const astronauts = await response.json()
        for (const astronaut of astronauts) {
            tableBody.innerHTML += `
                <tr>
                    <td>${astronaut.name}</td>
                    <td>${astronaut.age}</td>
                    <td>${astronaut.dateOfBirth}</td>
                    <td>${astronaut.missionsCompleted}</td>
                </tr>
            `
        }

    } else {
        // :(
    }
}
/*
getMultipleChoice()
nextButton.addEventListener('click', getMultipleChoice)
*/
/******/ })()
;
//# sourceMappingURL=multiplechoice.entry.js.map