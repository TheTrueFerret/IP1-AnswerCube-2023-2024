

function getMultipleChoiceSlide() {
    fetch("http://localhost:5104/api/Installations/NextSlide",
        {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        })
        .then(response => {
            if (response.status === 200) {
                return response.json();
            } else {
                document.getElementById("slide").innerHTML = "<em>PENIS!!!</em>";
            }
        })
        .then(slide => {
            console.log(slide);
            document.getElementById("slide").innerHTML += `<h1>${slide.id}</h1><h2>${slide.text}</h2>`;
            for (const answers of slide.answerList.$values) {
                slide.innerHTML += `<input type="checkbox" id="input" value="${answers}" name="answer">${answers}<br>`;
            }
        })
}


getMultipleChoiceSlide()

