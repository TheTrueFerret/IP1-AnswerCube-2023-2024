//function getSlideList() {
//    fetch("http://localhost:5104/api/Slides",
//        {
//            method: "GET",
//            headers: {
//                "Accept": "application/json"
//            }
//        })
//        .then(response => {
//            if (response.status === 200) {
//                return response.json();
//            } else {
//                document.getElementById("page").innerHTML = "<em>Problem!!!</em>";
//            }
//        })
//        .then(slideList => {
//            console.log(slideList);
//            updateCondition(slideList[1])
//        })
//}
//
//function updateCondition(newCondition) {
//    fetch("http://localhost:5104/api/Slides", {
//        method: "POST",
//        headers: {
//            'Content-Type': 'application/json',
//            'Accept': 'application/json',
//        },
//        body: JSON.stringify(newCondition)
//    }).then(res => {
//        if(res.ok) {
//            if(res.status === 201) {
//                return res.json();
//            }
//        } else {
//            alert("No 2xx code returned")
//        }
//    }).catch(err => {
//        alert("Something went wrong: " + err);
//    })
//}

document.addEventListener("DOMContentLoaded", async function () {
    const nextBtn = document.getElementById("next");
    const slide = document.getElementById("slide");
    let maxSlide = await getMaxSlides()
    let currentSlide = 1;
    //let timer = setInterval(nextSlide, 15000);
    addListeners()
    console.log(maxSlide)
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
                case "multiplechoice":
                    console.log("multiple choice slide");
                    slide.innerHTML = `<h3> ${data.text} </h3> `;
                    for (const answers of data.answerList.$values) {
                        slide.innerHTML += `<input type="checkbox" id="input" value="${answers}" name="answer">${answers}<br>`;
                    }
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
        updateProgressBar();
        currentSlide++;
    }

    function updateProgressBar() {
        let totalQuestions = maxSlide; // total number of questions
        let answeredQuestions = currentSlide; // number of answered questions

        let progress = (answeredQuestions / totalQuestions) * 100;
        console.log(progress)

        let progressBar = document.getElementById("progressBar");
        progressBar.style.width = progress + "%";
        progressBar.style.backgroundColor = "limegreen"; // Add this line
    }

    async function getMaxSlides() {
        let numberOfSlides = 0;
        await fetch(`https://localhost:7272/api/flow/getMaxNumberOfSlides`, {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        }).then(async response => {
            if (response.status === 200) {

                numberOfSlides = await response.json().then(data => {
                    console.log(data)
                    return data.count;
                });
            }
        })
        return numberOfSlides;
    }
});