function nextSlide() {
    fetch("http://localhost:5104/api/Installations",
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
                document.getElementById("slide").innerHTML = "<em>problem!!!</em>";
            }
        }).then(slide => {
        console.log(slide)
        UpdateFlowPage(slide)
    })
        .catch(error => {
            console.error(error);
            document.getElementById("slide").innerHTML = "<em>Problem loading the slide</em>";
        });
}

function UpdateFlowPage(slide) {
    fetch("http://localhost:5104/Flow/SetCurrentSlide", {
        method: "POST",
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        },
        body: JSON.stringify(slide)
    })
        .then(response => {
            if (response.ok) {
                console.log("Current slide updated successfully");
                return response.text();
            } else {
                throw new Error("Failed to update current slide");
            }
        })
        .then(partialPageHtml  => {
            console.log(partialPageHtml );
            document.getElementById("page").innerHTML = partialPageHtml;
        })
        .catch(error => {
            console.error("Error updating current slide:", error);
        });
}


const btn = document.getElementById("nextSlide").addEventListener('click', nextSlide);


document.addEventListener("DOMContentLoaded", function () {
    const nextBtn = document.getElementById("next");
    const slide = document.getElementById("slide");
    let maxSlide = 0;
    let currentSlide = 1
    //let timer = setInterval(nextSlide, 15000);
    addListeners()
    updateProgressBar()
    getMaxSlides()
    nextSlide()

    function addListeners() {
        if (nextBtn) {
            nextBtn.addEventListener("click", nextSlide);
        }
    }

    function getSlide() {
        if (currentSlide > maxSlide) {
            currentSlide = 1
            console.log("OUT OF SLIDES")
            //clearInterval(timer)
        }
        fetch(`https://localhost:7272/api/flow/getSlideFromList/${currentSlide}`,
            {
                method: "GET",
                headers: {
                    "Accept": "application/json"
                }
            })
            .then(response => {
                if (response.status === 200) {
                    slide.innerHTML = `<em>IT WORKS!!!! + ${currentSlide} </em>`;
                    return response.json();
                } else {
                    slide.innerHTML = `<em>OUT OF SLIDES!!! + ${currentSlide} </em>`;
                }
            }).then(data => {
            console.log(data);
            switch (data.slideType?.toLowerCase()) {
                case null:
                    slide.innerHTML = `<em>No slides found!</em>`;
                    break;
                case "openquestion":
                    console.log("open question slide");
                    slide.innerHTML = `<h4> ${data.text} </h4>`;
                    slide.innerHTML += `<input type="text" id="input" value="" placeholder="Answer the question.">`;
                    break;
                
                case "singlechoice":
                    console.log("Single choice slide");
                    slide.innerHTML = `<h3> ${data.text} </h3> `;
                    for (const answers of data.answerList.$values) {
                        slide.innerHTML += `<input type="radio" id="input" value="${answers}" name="answer">${answers}<br>`;
                    }
                    break;
                case "info":
                    console.log("Single choice slide");
                    slide.innerHTML = `<h3> ${data.text} </h3> `;
                    break;
                case "rangequestion":
                    console.log("Range Question slide");
                    slide.innerHTML = `<h3> ${data.text} </h3> `;
                    for (const answers of data.answerList.$values) {
                        slide.innerHTML += `<input type="radio" id="input" value="${answers}" name="answer">${answers}<br>`;
                    }
                    break;
                default:
                    console.log("problem loading data");
                    slide.innerHTML = `<h4>Problem with loading the data</h4>`;
                    break;
            }
        })
    }

    function nextSlide() {
        getSlide()
        currentSlide++;
        updateProgressBar();
    }

    function updateProgressBar() {
        let totalQuestions = maxSlide; // total number of questions
        let answeredQuestions = currentSlide; // number of answered questions

        let progress = (answeredQuestions / totalQuestions) * 100;

        let progressBar = document.getElementById("progressBar");
        progressBar.style.width = progress + "%";
        progressBar.style.backgroundColor = "limegreen"; // Add this line
    }

    function getMaxSlides() {
        fetch(`https://localhost:7272/api/flow/getMaxNumberOfSlides`, {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        })
            .then(async response => {
                if (response.status === 200) {
                    return response.json();
                } else {
                    ;
                }
            }).then(data => {
            maxSlide = data.count;
        })
    }
});
