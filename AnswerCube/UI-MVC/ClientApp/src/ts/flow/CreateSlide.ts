import {max} from "@popperjs/core/lib/utils/math";

let optionCount = 0;
let maxOptions = 7;
window.onload = () => {
    const slideTypeElement = document.getElementById('slideType') as HTMLSelectElement;
    slideTypeElement.onchange = function (event) {
        const selectElement = event.target as HTMLSelectElement;
        showAnswerOptions(selectElement.value);
    }
}

function showAnswerOptions(slideType: string) {
    const answerOptionsDiv = document.getElementById('answerOptions') as HTMLDivElement;
    const optionsBtnDiv = document.getElementById('optionsButton') as HTMLDivElement;
    answerOptionsDiv.innerHTML = '';
    optionCount = 0;

    if (slideType === 'MultipleChoice' || slideType === 'SingleChoice') {
        
        maxOptions = 7;
        answerOptionsDiv.innerHTML = '<div id="options" class="form-group"><label for="options">Options:</label>' +
            '<div id="option0" class="form-group mb-3"><input type="text" id="options0" name="options" class="form-control"></div></div>';
        const addButton = document.createElement('button');
        addButton.textContent = 'Add Option';
        addButton.onclick = addOption;
        addButton.type = 'button';
        addButton.classList.add('btn', 'btn-primary');
        optionsBtnDiv.appendChild(addButton);
    } else if (slideType === 'RangeQuestion') {
        maxOptions = 4;
        answerOptionsDiv.innerHTML = '<div class="form-group"><label for="range">Range:</label><br><input type="range" id="range" name="range" class="form-control-range"><br><br></div>';
    }
}

function addOption() {
    if (optionCount < maxOptions) {
        const optionsDiv = document.getElementById('options') as HTMLDivElement;
        const newOptionDiv = document.createElement('div');
        newOptionDiv.id = 'option' + optionCount+1;
        newOptionDiv.style.display = 'flex';
        newOptionDiv.style.justifyContent = 'space-between';
        newOptionDiv.style.alignItems = 'center';
        newOptionDiv.classList.add('form-group', 'mb-3');

        const newOption = document.createElement('input');
        newOption.type = 'text';
        newOption.name = 'options';
        newOption.id = 'options' + optionCount+1;
        newOption.className = 'form-control';
        newOption.style.flexGrow = '1';

        const removeButton = document.createElement('button');
        removeButton.onclick = () => removeOption(newOptionDiv.id);
        removeButton.type = 'button';
        removeButton.classList.add('btn', 'btn-small', 'btn-danger');

        const icon = document.createElement('i');
        icon.classList.add('fas', 'fa-trash');
        removeButton.appendChild(icon);

        newOptionDiv.appendChild(newOption);
        newOptionDiv.appendChild(removeButton);
        optionsDiv.appendChild(newOptionDiv);
        optionCount++;
    } else {
        alert('You can only add up to 8 options.');
    }
}

function removeOption(optionId: string) {
    const optionDiv = document.getElementById(optionId);
    if (optionDiv) {
        optionDiv.remove();
        optionCount--;
    }
}