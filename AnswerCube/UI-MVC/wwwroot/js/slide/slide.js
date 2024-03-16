

function getSlide() {
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
                document.getElementsByClassName("page").innerHTML = "<em>Problem!!!</em>";
            }
        })
        .then(slide => {
            console.log(slide);
            document.getElementById("page").innerHTML += `<h1>${slide.id}</h1><h2>${slide.question}</h2>`;
        })
}

const nextButton = document.getElementById("nextButton");

nextButton.addEventListener('submit', (event) => {
    getSlide();
});



getSlide()