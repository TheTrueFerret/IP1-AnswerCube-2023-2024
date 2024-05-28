document.addEventListener("DOMContentLoaded", function () {
    const answersList = document.getElementById("answersList") as HTMLDivElement | null;
    const addAnswerButton = document.getElementById("add-answer") as HTMLButtonElement | null;
    const slideTypeSelect = document.getElementById("slideType") as HTMLSelectElement | null;

    if (addAnswerButton && answersList && slideTypeSelect) {
        addAnswerButton.addEventListener("click", function () {
            const answerCount = answersList.querySelectorAll('.answer-item').length;
            const newAnswer = document.createElement("div");
            newAnswer.classList.add("input-group", "mb-3", "answer-item");
            newAnswer.innerHTML = `
                <span class="input-group-text">Optie ${answerCount + 1}</span>
                <input type="text" name="answersList" class="form-control" />
                <button type="button" class="btn btn-danger delete-answer"><i class="fas fa-trash"></i></button>
            `;
            answersList.appendChild(newAnswer);
        });

        answersList.addEventListener("click", function (event) {
            const target = event.target as HTMLElement;
            if (target.closest('.delete-answer')) {
                const answerItem = target.closest('.answer-item');
                if (answerItem) {
                    answerItem.remove();
                    const answerItems = answersList.querySelectorAll('.answer-item');
                    answerItems.forEach((item, index) => {
                        item.querySelector('.input-group-text')!.textContent = `Optie ${index + 1}`;
                    });
                }
            }
        });

        slideTypeSelect.addEventListener("change", function () {
            const slideTypeInput = document.getElementById("slideType") as HTMLInputElement | null;
            const addAnswerSection = document.getElementById("answersList") as HTMLDivElement | null;

            if (slideTypeInput && addAnswerSection) {
                slideTypeInput.value = slideTypeSelect.value;
                if (slideTypeSelect.value === "InfoSlide" || slideTypeSelect.value === "OpenQuestion") {
                    addAnswerSection.style.display = "none";
                } else {
                    addAnswerSection.style.display = "block";
                }
            }
        });
    }
});
