
function getMultipleChoice() {
    fetch("http://localhost:5104/api/Slides",
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
                document.getElementById("slide").innerHTML = "<em>Problem!!!</em>";
            }
        })
        .then(slide => {
            console.log(slide);
            document.getElementById("slide").innerHTML += `<h1>${slide.id}</h1><h2>${slide.text}</h2>`;
        })
}

getMultipleChoice()