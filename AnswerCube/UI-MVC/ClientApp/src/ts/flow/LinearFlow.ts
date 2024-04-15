


function nextSlide() {
    fetch("http://localhost:5104/LinearFlow/NextSlide/",
        {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        })
        .then(response => {
            if (response.status === 200) {
                return response.text();
            } else {
                document.getElementById("slide").innerHTML = "<em>problem!!!</em>";
            }
        }).then(slidePage => {
        document.getElementById("page").innerHTML = slidePage;
    })
        .catch(error => {
            console.error(error);
            document.getElementById("slide").innerHTML = "<em>Problem loading the slide</em>";
        });
}

const btn = document.getElementById("nextSlide").addEventListener('click', nextSlide);

