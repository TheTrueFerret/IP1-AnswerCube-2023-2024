

function initialise() {
    fetch("http://localhost:5104/api/Installation/initialiseSlideList",
        {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        })
        .then(response => {
            if (response.status === 200) {
                nextSlide()
                return response.json();
            } else {
                document.getElementById("slide").innerHTML = "<em>Problem!!!</em>";
            }
        })
}

function nextSlide() {
    fetch("http://localhost:5104/api/Installation/nextSlide",
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
}

const btn = document.getElementById("nextSlide").addEventListener('click', initialise);
