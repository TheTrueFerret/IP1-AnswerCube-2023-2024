/******/ (function() { // webpackBootstrap
/******/ 	"use strict";
/******/ 	var __webpack_modules__ = ({

/***/ "./src/ts/urlDecoder.ts":
/*!******************************!*\
  !*** ./src/ts/urlDecoder.ts ***!
  \******************************/
/***/ (function(__unused_webpack_module, __webpack_exports__, __webpack_require__) {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   RemoveLastDirectoryPartOf: function() { return /* binding */ RemoveLastDirectoryPartOf; }
/* harmony export */ });
function RemoveLastDirectoryPartOf(the_url) {
    var the_arr = the_url.split('/');
    the_arr.pop();
    return (the_arr.join('/'));
}


/***/ })

/******/ 	});
/************************************************************************/
/******/ 	// The module cache
/******/ 	var __webpack_module_cache__ = {};
/******/ 	
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/ 		// Check if module is in cache
/******/ 		var cachedModule = __webpack_module_cache__[moduleId];
/******/ 		if (cachedModule !== undefined) {
/******/ 			return cachedModule.exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = __webpack_module_cache__[moduleId] = {
/******/ 			// no module.id needed
/******/ 			// no module.loaded needed
/******/ 			exports: {}
/******/ 		};
/******/ 	
/******/ 		// Execute the module function
/******/ 		__webpack_modules__[moduleId](module, module.exports, __webpack_require__);
/******/ 	
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/ 	
/************************************************************************/
/******/ 	/* webpack/runtime/define property getters */
/******/ 	!function() {
/******/ 		// define getter functions for harmony exports
/******/ 		__webpack_require__.d = function(exports, definition) {
/******/ 			for(var key in definition) {
/******/ 				if(__webpack_require__.o(definition, key) && !__webpack_require__.o(exports, key)) {
/******/ 					Object.defineProperty(exports, key, { enumerable: true, get: definition[key] });
/******/ 				}
/******/ 			}
/******/ 		};
/******/ 	}();
/******/ 	
/******/ 	/* webpack/runtime/hasOwnProperty shorthand */
/******/ 	!function() {
/******/ 		__webpack_require__.o = function(obj, prop) { return Object.prototype.hasOwnProperty.call(obj, prop); }
/******/ 	}();
/******/ 	
/******/ 	/* webpack/runtime/make namespace object */
/******/ 	!function() {
/******/ 		// define __esModule on exports
/******/ 		__webpack_require__.r = function(exports) {
/******/ 			if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 				Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 			}
/******/ 			Object.defineProperty(exports, '__esModule', { value: true });
/******/ 		};
/******/ 	}();
/******/ 	
/************************************************************************/
var __webpack_exports__ = {};
// This entry need to be wrapped in an IIFE because it need to be isolated against other modules in the chunk.
!function() {
/*!********************************************!*\
  !*** ./src/ts/flow/slide/RangeQuestion.ts ***!
  \********************************************/
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _urlDecoder__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../../urlDecoder */ "./src/ts/urlDecoder.ts");

let url = window.location.toString();
var slideElement = document.getElementById("slide");
var sliderElement = document.getElementById("slider");
const baseUrl = "https://storage.cloud.google.com/answer-cube-bucket/";
let rangeInput = document.querySelector('input[type="range"]');
let min = parseInt(rangeInput.min, 10);
let max = parseInt(rangeInput.max, 10);
let step = rangeInput.step ? parseInt(rangeInput.step, 10) : 1;
function postAnswer(cubeId, action) {
    let answer = getRangeAnswer();
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
    fetch((0,_urlDecoder__WEBPACK_IMPORTED_MODULE_0__.RemoveLastDirectoryPartOf)(url) + "/PostAnswer", {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
        },
        body: JSON.stringify(requestBody)
    })
        .then((response) => {
        if (response.status === 200) {
            return response.json();
        }
        else {
            if (slideElement) {
                slideElement.innerHTML = "<em>Problem!!!</em>";
            }
        }
    })
        .then((nextSlideData) => {
        if (nextSlideData.url) {
            window.location.href = nextSlideData.url;
        }
    })
        .catch(err => {
        console.log("Something went wrong: " + err);
    });
}
function getRangeAnswer() {
    let selectedAnswers = [];
    if (sliderElement.value) {
        selectedAnswers.push(sliderElement.value);
    }
    return selectedAnswers;
}
function moveRangeButton(direction) {
    rangeInput.focus();
    if (direction == "up") {
        if (rangeInput.valueAsNumber < max) {
            rangeInput.valueAsNumber += step;
        }
    }
    if (direction == "down") {
        if (rangeInput.valueAsNumber > min) {
            rangeInput.valueAsNumber -= step;
        }
    }
}
window.slideType = "RangeQuestion";
window.moveRangeButton = moveRangeButton;
window.postAnswer = postAnswer;

}();
/******/ })()
;
//# sourceMappingURL=rangequestion.entry.js.map