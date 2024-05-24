/******/ (function() { // webpackBootstrap
/******/ 	"use strict";
var __webpack_exports__ = {};
/*!*************************************!*\
  !*** ./src/ts/flow/InputHandler.ts ***!
  \*************************************/

const keysPressed = new Set();
document.addEventListener('keydown', (event) => {
    keysPressed.add(event.key);
    const slideType = window.slideType;
    if (keysPressed.has('ArrowDown') && keysPressed.has('1') || keysPressed.has('ArrowDown')) {
        console.log('ArrowDown + CubeId: 1');
        if (slideType == "MultipleChoice" && typeof window.moveSelectedButton === 'function') {
            window.moveSelectedButton('down');
        }
        if (slideType == "SingleChoice" && typeof window.moveCheckedRadioButton === 'function') {
            window.moveCheckedRadioButton('down');
        }
        if (slideType == "RangeQuestion" && typeof window.moveRangeButton === 'function') {
            window.moveRangeButton('down');
        }
    }
    if (keysPressed.has('ArrowUp') && keysPressed.has('1') || keysPressed.has('ArrowUp')) {
        console.log('ArrowUp + CubeId: 1');
        if (slideType == "MultipleChoice" && typeof window.moveSelectedButton === 'function') {
            window.moveSelectedButton('up');
        }
        if (slideType == "SingleChoice" && typeof window.moveCheckedRadioButton === 'function') {
            window.moveCheckedRadioButton('up');
        }
        if (slideType == "RangeQuestion" && typeof window.moveRangeButton === 'function') {
            window.moveRangeButton('up');
        }
    }
    if (keysPressed.has('ArrowLeft') && keysPressed.has('1') || keysPressed.has('ArrowLeft')) {
        if (slideType == "MultipleChoice" && typeof window.selectButton === 'function') {
            window.selectButton();
        }
    }
    if (keysPressed.has('ArrowRight') && keysPressed.has('1') || keysPressed.has('ArrowRight')) {
        if (slideType == "InfoSlide" && typeof window.skipQuestion === 'function') {
            window.skipQuestion();
        }
        else {
            window.postAnswer(1, 'skip');
        }
    }
    if (keysPressed.has('Enter') && keysPressed.has('1') || keysPressed.has('Enter')) {
        console.log('Enter + CubeId: 1');
        if (slideType == "InfoSlide" && typeof window.skipQuestion === 'function') {
            window.skipQuestion();
        }
        else {
            window.postAnswer(1, 'submit');
        }
    }
    switch (event.key) {
        default:
            console.log(event.key, event.keyCode);
            return;
    }
});
document.addEventListener('keyup', (event) => {
    keysPressed.delete(event.key);
});
const submitBtn = document.getElementById("submitAnswer");
if (submitBtn) {
    submitBtn.addEventListener('click', function () {
        if (window.slideType == "InfoSlide" && typeof window.skipQuestion === 'function') {
            window.skipQuestion();
        }
        else {
            window.postAnswer(1, 'submit');
        }
    });
}
const skipBtn = document.getElementById("skipSlide");
if (skipBtn) {
    skipBtn.addEventListener('click', function () {
        if (window.slideType == "InfoSlide" && typeof window.skipQuestion === 'function') {
            window.skipQuestion();
        }
        else {
            window.postAnswer(1, 'skip');
        }
    });
}

/******/ })()
;
//# sourceMappingURL=inputhandler.entry.js.map