import {max} from "@popperjs/core/lib/utils/math";

let optionCount = 0;
let maxOptions = 8;
window.addEventListener("DOMContentLoaded", ev => {
    const slideTypeElement = document.getElementById('slideType') as HTMLSelectElement;
    slideTypeElement.onchange = function (event) {
        const selectElement = event.target as HTMLSelectElement;
        showAnswerOptions(selectElement.value);
    }
});

function showAnswerOptions(slideType: string) {
    const answerOptionsDiv = document.getElementById('answerOptions') as HTMLDivElement;
    const questionDiv = document.getElementById('questionDiv') as HTMLDivElement;
    const label = document.getElementById("questionLabel") as HTMLLabelElement;
    const input = document.getElementById("question") as HTMLInputElement;
    answerOptionsDiv.innerHTML = '';
    optionCount = 0;

    if (slideType === 'MultipleChoice' || slideType === 'SingleChoice') {
        questionDiv.hidden = false;
        label.innerText = "Question:";
        input.placeholder = "Question";
        maxOptions = 8;
        CreateOptionField(answerOptionsDiv);
    } else if (slideType === 'RangeQuestion') {
        questionDiv.hidden = false;
        label.innerText = "Question:";
        input.placeholder = "Question";
        maxOptions = 5;
        CreateOptionField(answerOptionsDiv);
    } else if (slideType === 'InfoSlide') {
        questionDiv.hidden = false;
        label.innerText = "Info title:";
        input.placeholder = "info title";
        CreateInfoField(answerOptionsDiv);
    } else if (slideType === 'OpenQuestion') {
        questionDiv.hidden = false;
        label.innerText = "Question:";
        input.placeholder = "Question";
    } else if (slideType === '') {
        questionDiv.hidden = true;
    }
}

function CreateInfoField(optionsDiv: HTMLDivElement) {
    optionsDiv.innerHTML = '<div id="options" class="form-group"><label for="options">Info text:</label>' +
        '<div id="options0" class="form-group mb-3"><input type="text" id="options0" name="options" class="form-control" placeholder="Info" required></div></div>';
}

function CreateOptionField(optionsDiv: HTMLDivElement) {
    optionsDiv.innerHTML = '<div id="options" class="form-group"><label for="options">Options:</label>' +
        '<div id="option0" class="form-group mb-3"><input type="text" id="options0" name="options" class="form-control" placeholder="option" required></div>' +
        '<div id="option1" class="form-group mb-3"><input type="text" id="options1" name="options" class="form-control" placeholder="option" required></div></div>';
    optionCount = 2;
    const addButton = document.createElement('button');
    addButton.textContent = 'Add Option';
    addButton.onclick = addOption;
    addButton.type = 'button';
    addButton.classList.add('btn', 'btn-primary');
    optionsDiv.appendChild(addButton);
}

function addOption() {
    if (optionCount < maxOptions) {
        const optionsDiv = document.getElementById('options') as HTMLDivElement;
        const newOptionDiv = document.createElement('div');
        newOptionDiv.id = 'option' + optionCount + 1;
        newOptionDiv.style.display = 'flex';
        newOptionDiv.style.justifyContent = 'space-between';
        newOptionDiv.style.alignItems = 'center';
        newOptionDiv.classList.add('form-group', 'mb-3');

        const newOption = document.createElement('input');
        newOption.type = 'text';
        newOption.name = 'options';
        newOption.id = 'options' + optionCount + 1;
        newOption.className = 'form-control';
        newOption.style.flexGrow = '1';
        newOption.placeholder = 'option';
        newOption.required = true;

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
        alert('You can only add up to ' + (maxOptions + 1) + ' options.');
    }
}

function removeOption(optionId: string) {
    const optionDiv = document.getElementById(optionId);
    if (optionDiv) {
        optionDiv.remove();
        optionCount--;
    }
}