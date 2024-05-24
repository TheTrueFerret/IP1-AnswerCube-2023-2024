/******/ (function() { // webpackBootstrap
/******/ 	"use strict";
var __webpack_exports__ = {};
/*!***********************************!*\
  !*** ./src/ts/users/searchbar.ts ***!
  \***********************************/

document.addEventListener('DOMContentLoaded', (event) => {
    const searchQueryElement = document.getElementById('searchQuery');
    searchQueryElement.addEventListener('input', function (e) {
        var _a;
        let searchQuery = searchQueryElement.value.toLowerCase();
        const userRows = document.getElementsByClassName('user-row');
        for (let i = 0; i < userRows.length; i++) {
            const emailElement = userRows[i].querySelector('td[data-email]');
            const email = ((_a = emailElement === null || emailElement === void 0 ? void 0 : emailElement.getAttribute('data-email')) === null || _a === void 0 ? void 0 : _a.toLowerCase()) || '';
            if (email.includes(searchQuery)) {
                userRows[i].style.display = '';
            }
            else {
                userRows[i].style.display = 'none';
            }
        }
    });
});

/******/ })()
;
//# sourceMappingURL=searchBar.entry.js.map