

const slide = document.getElementById("slide");

function LoadSlideData() {
    fetch("http://localhost:5104/LinearFlow/GetSlideData/",
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
                slide.innerHTML = "<em>problem!!!</em>";
            }
        }).then(slideData => {
        console.log("multiple choice slide");
        slide.innerHTML = `<h3> ${slideData.text} </h3> `;
        for (const answers of slideData.answerList.$values) {
            slide.innerHTML += `<input type="checkbox" id="input" value="${answers}" name="answer">${answers}<br>`;
        }
    })
        .catch(error => {
            console.error(error);
            slide.innerHTML = "<em>Problem loading the slide</em>";
        });
}

LoadSlideData()


