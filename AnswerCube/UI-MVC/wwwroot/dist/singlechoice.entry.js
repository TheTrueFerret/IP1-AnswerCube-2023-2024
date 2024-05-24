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
/*!*******************************************!*\
  !*** ./src/ts/flow/slide/SingleChoice.ts ***!
  \*******************************************/
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _urlDecoder__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../../urlDecoder */ "./src/ts/urlDecoder.ts");

var url = window.location.toString();
const slideElement = document.getElementById("slide");
const checkboxes = document.querySelectorAll('input[name="answer"]');
var currentCheckedIndex = -1;
const totalCheckboxes = checkboxes.length;
function postAnswer(cubeId, action) {
    let answer = getSelectedAnswer();
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
    }).then((response) => {
        if (response.status === 200) {
            return response.json();
        }
        else {
            if (slideElement) {
                slideElement.innerHTML = "<em>problem!!!</em>";
            }
        }
    }).then((nextSlideData) => {
        if (nextSlideData.url) {
            // Redirect to the URL of the next slide
            window.location.href = nextSlideData.url;
        }
    }).catch(err => {
        console.log("Something went wrong: " + err);
    });
    console.log(answer);
}
function getSelectedAnswer() {
    const checkboxes = document.querySelectorAll('input[name="answer"]:checked');
    let selectedAnswers = [];
    checkboxes.forEach((checkbox) => {
        const inputElement = checkbox;
        if (inputElement.value) {
            selectedAnswers.push(inputElement.value);
        }
    });
    return selectedAnswers;
}
function moveCheckedRadioButton(direction) {
    // Check if there's a radio button checked
    if (currentCheckedIndex === -1) {
        checkboxes[0].checked = true;
        currentCheckedIndex = 0;
        return;
    }
    let newIndex;
    if (direction === 'up') {
        newIndex = currentCheckedIndex - 1;
        if (newIndex < 0)
            newIndex = totalCheckboxes - 1;
    }
    else if (direction === 'down') {
        newIndex = currentCheckedIndex + 1;
        if (newIndex >= totalCheckboxes)
            newIndex = 0;
    }
    else {
        return; // Invalid direction
    }
    checkboxes[currentCheckedIndex].checked = false;
    checkboxes[newIndex].checked = true;
    currentCheckedIndex = newIndex;
}
window.slideType = "SingleChoice";
window.moveCheckedRadioButton = moveCheckedRadioButton;
window.postAnswer = postAnswer;

}();
/******/ })()
;
//# sourceMappingURL=singlechoice.entry.js.map