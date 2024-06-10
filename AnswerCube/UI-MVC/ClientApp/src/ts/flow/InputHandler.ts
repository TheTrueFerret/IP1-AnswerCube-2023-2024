import {max} from "@popperjs/core/lib/utils/math";

const keysPressed = new Set();
const skipBtn: HTMLElement | null = document.getElementById("skipSlide");
const submitBtn: HTMLElement | null = document.getElementById("submitAnswer");
const MAX_USERS: number = 4;
let cubeId: string | null = null;

document.addEventListener('keydown', (event) => {
    keysPressed.add(event.key);
    const slideType: string = window.slideType;
    cubeId = null;
    
    for (let i: number = 0; i <= MAX_USERS; i++) {
        if (keysPressed.has(i.toString())) {
            cubeId = i.toString()
        }
    }
    var cubeNumber: number = Number(cubeId)

    if (cubeId != null && keysPressed.size !== 1) {
        if (keysPressed.has('ArrowDown') && keysPressed.has(cubeId)) {
            move(cubeNumber, 'down', slideType);
        }
        if (keysPressed.has('ArrowUp') && keysPressed.has(cubeId)) {
            move(cubeNumber, 'up', slideType);
        }
        if (keysPressed.has('ArrowLeft') && keysPressed.has(cubeId)) {
            if (slideType === "MultipleChoice") {
                window.selectButton(cubeNumber);
            }
        }
        if (keysPressed.has('ArrowRight') && keysPressed.has(cubeId)) {
            if (slideType === "InfoSlide") {
                window.vote(cubeNumber, 'skip');
            } else {
                window.vote(cubeNumber, 'skip');
            }
        }
        if (keysPressed.has('Enter') && keysPressed.has(cubeId)) {
            if (slideType === "InfoSlide") {
                window.vote(cubeNumber, 'submit');
            } else {
                window.vote(cubeNumber, 'submit');
            }
        }
        if (keysPressed.has('a') && keysPressed.has(cubeId)) {
            if (slideType === "InfoSlide") {
                window.addNewOrDeleteCubeUser(cubeNumber);
            } else {
                window.addNewOrDeleteCubeUser(cubeNumber);
            }
        }
        if (keysPressed.has('s') && keysPressed.has(cubeId)) {
            window.vote(cubeNumber, 'changeSubTheme');
        }
    } /*else if (keysPressed.size === 1) {
        if (keysPressed.has('ArrowDown')) {
            move(0, 'down', slideType);
        }
        if (keysPressed.has('ArrowUp')) {
            move(0, 'up', slideType);
        }
        if (keysPressed.has('ArrowLeft')) {
            if (slideType === "MultipleChoice") {
                window.selectButton(0);
            }
        }
        if (keysPressed.has('ArrowRight')) {
            if (slideType === "InfoSlide") {
                window.vote(0, 'skip');
            } else {
                window.vote(0, 'skip');
            }
        }
        if (keysPressed.has('Enter')) {
            if (slideType === "InfoSlide") {
                window.vote(0, 'submit');
            } else {
                window.vote(0, 'submit');
            }
        }
        if (keysPressed.has('a')) {
            window.addNewOrDeleteCubeUser(0);
        }
        if (keysPressed.has('s')) {
            window.vote(cubeNumber, 'changeSubTheme');
        }
    }*/
});

function move(cubeId: number, direction: 'up' | 'down', slideType: string) {
    console.log(direction + ' + CubeId: ' + cubeId);
    if (slideType == "MultipleChoice" && typeof window.moveSelectedButton === 'function') {
        window.moveSelectedButton(cubeId, direction);
    }
    if (slideType == "SingleChoice" && typeof window.moveCheckedRadioButton === 'function') {
        window.moveCheckedRadioButton(cubeId, direction);
    }
    if (slideType == "RangeQuestion" && typeof window.moveRangeButton === 'function') {
        window.moveRangeButton(cubeId, direction);
    }
    if (slideType == "SubThemes" && typeof window.moveBetweenSlideLists === 'function') {
        window.moveBetweenSlideLists(cubeId, direction);
    }
}


document.addEventListener('keyup', (event) => {
    keysPressed.delete(event.key);
});


if (submitBtn) {
    submitBtn.addEventListener('click', function () {
        if (window.slideType == "InfoSlide" && typeof window.skipQuestion === 'function') {
            window.skipQuestion();
        } else {
            window.vote(0, 'submit');
        }
    });
}

if (skipBtn) {
    skipBtn.addEventListener('click', function () {
        if (window.slideType == "InfoSlide" && typeof window.skipQuestion === 'function') {
            window.skipQuestion();
        } else {
            window.vote(0, 'skip');
        }
    });
}

