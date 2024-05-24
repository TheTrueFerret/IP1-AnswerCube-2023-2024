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

    if (cubeId != null) {
        if (keysPressed.has('ArrowDown') && keysPressed.has(cubeId)) {
            move(Number(cubeId), 'down', slideType);
        }
        if (keysPressed.has('ArrowUp') && keysPressed.has(cubeId)) {
            move(Number(cubeId), 'up', slideType);
        }
        if (keysPressed.has('ArrowLeft') && keysPressed.has(cubeId)) {
            if (slideType === "MultipleChoice") {
                window.selectButton(Number(cubeId));
            }
        }
        if (keysPressed.has('ArrowRight') && keysPressed.has(cubeId)) {
            if (slideType === "InfoSlide") {
                window.skipQuestion();
            } else {
                window.postAnswer(Number(cubeId), 'skip');
            }
        }
        if (keysPressed.has('Enter') && keysPressed.has(cubeId)) {
            if (slideType === "InfoSlide") {
                window.skipQuestion();
            } else {
                window.postAnswer(Number(cubeId), 'submit');
            }
        }
    } else if (keysPressed.size === 1) {
        if (keysPressed.has('ArrowDown')) {
            move(1, 'down', slideType);
        }
        if (keysPressed.has('ArrowUp')) {
            move(1, 'up', slideType);
        }
        if (keysPressed.has('ArrowLeft')) {
            if (slideType === "MultipleChoice") {
                window.selectButton(1);
            }
        }
        if (keysPressed.has('ArrowRight')) {
            if (slideType === "InfoSlide") {
                window.skipQuestion();
            } else {
                window.postAnswer(1, 'skip');
            }
        }
        if (keysPressed.has('Enter')) {
            if (slideType === "InfoSlide") {
                window.skipQuestion();
            } else {
                window.postAnswer(1, 'submit');
            }
        }
    }
    
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
}



if (submitBtn) {
    submitBtn.addEventListener('click', function () {
        if (window.slideType == "InfoSlide" && typeof window.skipQuestion === 'function') {
            window.skipQuestion();
        } else {
            if (cubeId != null) {
                window.postAnswer(Number(cubeId), 'submit');
            } else {
                window.postAnswer(1, 'submit');
            }
        }
    });
}

if (skipBtn) {
    skipBtn.addEventListener('click', function () {
        if (window.slideType == "InfoSlide" && typeof window.skipQuestion === 'function') {
            window.skipQuestion();
        } else {
            if (cubeId != null) {
                window.postAnswer(Number(cubeId), 'submit');
            } else {
                window.postAnswer(1, 'submit');
            }
        }
    });
}

document.addEventListener('keyup', (event) => {
    keysPressed.delete(event.key);
});

