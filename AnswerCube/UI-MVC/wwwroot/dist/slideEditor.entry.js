/******/ (function() { // webpackBootstrap
/******/ 	"use strict";
var __webpack_exports__ = {};
/*!******************************************!*\
  !*** ./src/ts/flow/slide/SlideEditor.ts ***!
  \******************************************/

document.addEventListener("DOMContentLoaded", function () {
    var _a;
    const answersList = document.getElementById("answersList");
    (_a = document.getElementById("add-answer")) === null || _a === void 0 ? void 0 : _a.addEventListener("click", function () {
        var _a;
        const answerCount = (_a = answersList === null || answersList === void 0 ? void 0 : answersList.querySelectorAll('.answer-item').length) !== null && _a !== void 0 ? _a : 0;
        const newAnswer = document.createElement("div");
        newAnswer.classList.add("input-group", "mb-3", "answer-item");
        newAnswer.innerHTML = `
            <span class="input-group-text">Optie ${answerCount + 1}</span>
            <input type="text" name="answersList" class="form-control" />
            <button type="button" class="btn btn-danger delete-answer"><i class="fas fa-trash"></i></button>
        `;
        answersList === null || answersList === void 0 ? void 0 : answersList.appendChild(newAnswer);
    });
    answersList === null || answersList === void 0 ? void 0 : answersList.addEventListener("click", function (event) {
        const target = event.target;
        if (target.closest('.delete-answer')) {
            const answerItem = target.closest('.answer-item');
            if (answerItem && answersList) {
                answersList.removeChild(answerItem);
                const answerItems = answersList.querySelectorAll('.answer-item');
                answerItems.forEach((item, index) => {
                    item.querySelector('.input-group-text').textContent = `Optie ${index + 1}`;
                });
            }
        }
    });
});

/******/ })()
;
//# sourceMappingURL=slideEditor.entry.js.map