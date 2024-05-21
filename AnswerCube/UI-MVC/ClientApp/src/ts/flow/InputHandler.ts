const keysPressed = new Set();

document.addEventListener('keydown', (event) => {
    keysPressed.add(event.key);
    const slideType: string = window.slideType;

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
            window.moveRangeButton('up')
        }
    }
    if (keysPressed.has('ArrowLeft') && keysPressed.has('1') || keysPressed.has('ArrowLeft')) {
        if (slideType == "MultipleChoice" && typeof window.selectButton === 'function') {
            window.selectButton();
        }
    }
    if (keysPressed.has('ArrowRight') && keysPressed.has('1') || keysPressed.has('ArrowRight')) {
        window.postAnswer(1, 'skip')
    }
    if (keysPressed.has('Enter') && keysPressed.has('1') || keysPressed.has('Enter')) {
        console.log('Enter + CubeId: 1')
        window.postAnswer(1, 'submit')
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


const submitBtn: HTMLElement | null = document.getElementById("submitAnswer");
if (submitBtn) {
    submitBtn.addEventListener('click', function () {
        window.postAnswer(1, 'submit')
    });
}

const skipBtn: HTMLElement | null = document.getElementById("skipSlide");
if (skipBtn) {
    skipBtn.addEventListener('click', function() {
        if (window.slideType == "InfoSlide") {
            window.skipQuestion
        }
        window.postAnswer(1, 'skip')
    });
}
