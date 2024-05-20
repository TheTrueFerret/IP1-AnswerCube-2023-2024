document.addEventListener("DOMContentLoaded", function () {
    const answersList = document.getElementById("answersList");

    document.getElementById("add-answer")?.addEventListener("click", function () {
        const answerCount = answersList?.querySelectorAll('.answer-item').length ?? 0;
        const newAnswer = document.createElement("div");
        newAnswer.classList.add("input-group", "mb-3", "answer-item");
        newAnswer.innerHTML = `
            <span class="input-group-text">Optie ${answerCount + 1}</span>
            <input type="text" name="answersList" class="form-control" />
            <button type="button" class="btn btn-danger delete-answer"><i class="fas fa-trash"></i></button>
        `;
        answersList?.appendChild(newAnswer);
    });

    answersList?.addEventListener("click", function (event) {
        const target = event.target as HTMLElement;
        if (target.closest('.delete-answer')) {
            const answerItem = target.closest('.answer-item');
            if (answerItem && answersList) {
                answersList.removeChild(answerItem);
                
                const answerItems = answersList.querySelectorAll('.answer-item');
                answerItems.forEach((item, index) => {
                    item.querySelector('.input-group-text')!.textContent = `Optie ${index + 1}`;
                });
            }
        }
    });
});
