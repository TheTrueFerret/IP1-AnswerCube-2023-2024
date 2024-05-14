/******/ (() => { // webpackBootstrap
/******/ 	"use strict";
var __webpack_exports__ = {};
/*!***********************************!*\
  !*** ./src/ts/Project/Project.ts ***!
  \***********************************/

window.document.addEventListener('DOMContentLoaded', function () {
    // Get all the project divs
    const projectDivs = document.querySelectorAll('.project-div');
    console.log(projectDivs.length);
    // Add click event listener to each project div
    // @ts-ignore
    projectDivs.forEach((div) => {
        div.addEventListener('click', function (event) {
            // Get the project id from the edit button's onclick attribute
            const projectId = this.dataset.projectId;
            const organizationId = this.dataset.organizationId;
            // Prevent the event from propagating if a button was clicked
            if (event.target.tagName === 'BUTTON' || event.target.tagName === 'I') {
                event.stopPropagation();
            }
            else {
                // Redirect to the project's view
                window.location.href = `/Project/Project?projectid=${projectId}&organizationid=${organizationId}`;
            }
        });
    });
    // Get the search input field
    const searchInput = document.getElementById('searchInput');
    const infoDiv = document.getElementById('infoDiv');
    infoDiv.textContent = 'No projects found';
    infoDiv.style.display = 'none';
    // Add an input event listener to the search input field
    searchInput.addEventListener('input', function () {
        // Get the current input value
        const inputValue = this.value.toLowerCase();
        // Filter the project divs
        // @ts-ignore
        projectDivs.forEach((div) => {
            var _a;
            // Get the project name from the div's data attribute
            const projectName = (_a = div.dataset.projectName) === null || _a === void 0 ? void 0 : _a.toLowerCase();
            // If the project name includes the input value, show the div, otherwise hide it
            if (inputValue === '' || (projectName === null || projectName === void 0 ? void 0 : projectName.includes(inputValue))) {
                div.style.display = 'block';
                infoDiv.style.display = 'none';
            }
            else {
                infoDiv.style.display = 'block';
                div.style.display = 'none';
            }
        });
    });
});

/******/ })()
;
//# sourceMappingURL=project.entry.js.map