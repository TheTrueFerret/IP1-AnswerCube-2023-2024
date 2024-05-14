/******/ (() => { // webpackBootstrap
/******/ 	"use strict";
/******/ 	var __webpack_modules__ = ({

/***/ "./src/ts/CookieHandler.ts":
/*!*********************************!*\
  !*** ./src/ts/CookieHandler.ts ***!
  \*********************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   getCookie: () => (/* binding */ getCookie),
/* harmony export */   setCookie: () => (/* binding */ setCookie)
/* harmony export */ });
// Function to set the cookie by name
function setCookie(name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "") + expires + "; path=/";
}
// Function to retrieve cookie by name
function getCookie(name) {
    const cookies = document.cookie.split(';');
    for (let i = 0; i < cookies.length; i++) {
        const cookie = cookies[i].trim();
        if (cookie.startsWith(name + '=')) {
            return cookie.substring(name.length + 1);
        }
    }
    return null;
}


/***/ }),

/***/ "./src/ts/urlDecoder.ts":
/*!******************************!*\
  !*** ./src/ts/urlDecoder.ts ***!
  \******************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   RemoveLastDirectoryPartOf: () => (/* binding */ RemoveLastDirectoryPartOf)
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
/******/ 	(() => {
/******/ 		// define getter functions for harmony exports
/******/ 		__webpack_require__.d = (exports, definition) => {
/******/ 			for(var key in definition) {
/******/ 				if(__webpack_require__.o(definition, key) && !__webpack_require__.o(exports, key)) {
/******/ 					Object.defineProperty(exports, key, { enumerable: true, get: definition[key] });
/******/ 				}
/******/ 			}
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/hasOwnProperty shorthand */
/******/ 	(() => {
/******/ 		__webpack_require__.o = (obj, prop) => (Object.prototype.hasOwnProperty.call(obj, prop))
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/make namespace object */
/******/ 	(() => {
/******/ 		// define __esModule on exports
/******/ 		__webpack_require__.r = (exports) => {
/******/ 			if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 				Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 			}
/******/ 			Object.defineProperty(exports, '__esModule', { value: true });
/******/ 		};
/******/ 	})();
/******/ 	
/************************************************************************/
var __webpack_exports__ = {};
// This entry need to be wrapped in an IIFE because it need to be isolated against other modules in the chunk.
(() => {
/*!********************************************!*\
  !*** ./src/ts/flow/slide/RangeQuestion.ts ***!
  \********************************************/
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _urlDecoder__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../../urlDecoder */ "./src/ts/urlDecoder.ts");
/* harmony import */ var _CookieHandler__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../../CookieHandler */ "./src/ts/CookieHandler.ts");


let url = window.location.toString();
const slideElement = document.getElementById("slide");
const jwtToken = (0,_CookieHandler__WEBPACK_IMPORTED_MODULE_1__.getCookie)("jwtToken");
const sliderElement = document.getElementById("slider");
function loadRangeQuestionSlide() {
    fetch((0,_urlDecoder__WEBPACK_IMPORTED_MODULE_0__.RemoveLastDirectoryPartOf)(url) + "/GetNextSlide/", {
        method: "GET",
        headers: {
            "Accept": "application/json",
            "Authorization": `Bearer ${jwtToken}`
        }
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
        .then((slide) => {
        console.log(slide);
        if (slideElement) {
            slideElement.innerHTML = `<h3>${slide.text}</h3>`;
            const answersContainer = document.querySelector(".answers-container");
            if (answersContainer) {
                fillSliderOptions(slide.answerList);
            }
        }
    })
        .catch((error) => {
        console.error(error);
        if (slideElement) {
            slideElement.innerHTML = "<em>Problem loading the slide</em>";
        }
    });
}
loadRangeQuestionSlide();
const btnSubmit = document.getElementById("submitAnswer");
if (btnSubmit) {
    btnSubmit.addEventListener('click', postAnswer);
}
function postAnswer() {
    let answer = getSelectedAnswer();
    let requestBody = {
        Answer: answer
    };
    console.log(requestBody);
    fetch((0,_urlDecoder__WEBPACK_IMPORTED_MODULE_0__.RemoveLastDirectoryPartOf)(url) + "/PostAnswer", {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
            'Authorization': `Bearer ${jwtToken}`
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
function getSelectedAnswer() {
    if (sliderElement) {
        let selectedAnswers = [];
        if (sliderElement.value) {
            selectedAnswers.push(sliderElement.value);
        }
        return selectedAnswers;
    }
}
function fillSliderOptions(options) {
    const tickmarkLabelsContainer = document.querySelector(".tickmark-labels");
    if (tickmarkLabelsContainer) {
        tickmarkLabelsContainer.innerHTML = ""; // Container leegmaken
        if (Array.isArray(options)) {
            options.forEach((option) => {
                const tickmarkLabel = document.createElement("div");
                tickmarkLabel.classList.add("tickmark-label");
                tickmarkLabel.textContent = option;
                tickmarkLabelsContainer.appendChild(tickmarkLabel);
            });
        }
    }
}

})();

/******/ })()
;
//# sourceMappingURL=rangequestion.entry.js.map