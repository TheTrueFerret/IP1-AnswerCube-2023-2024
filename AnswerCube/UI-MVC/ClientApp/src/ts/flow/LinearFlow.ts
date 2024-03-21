//function getSlideList() {
//    fetch(appUrl + "/api/Slides",
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
//    fetch(appUrl + "/api/Slides", {
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
    const skipBtn = document.getElementById("skip");
    const slide = document.getElementById("slide");
    let input = HTMLElement;
    let maxSlide = await getMaxSlides()
    let currentSlide = 1;
    addListeners()
    console.log(maxSlide)
    getSlide()
    updateProgressBar();

    function addListeners() {
        console.log(nextBtn)
        console.log(skipBtn)
        if (nextBtn) {
            nextBtn.addEventListener("click", nextSlide);
        }
        if (skipBtn) {
            skipBtn.addEventListener("click", nextSlide);
        }
    }

    function getSlide() {
        if (currentSlide > maxSlide) {
            currentSlide = 1
            console.log("OUT OF SLIDES")
            //clearInterval(timer)
        }
        fetch(appUrl + `/api/flow/getSlideFromList/${currentSlide}`,
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
                case "rangequestion":
                    console.log("Range Question slide");
                    slide.innerHTML = `<h3> ${data.text} </h3> `;
                    for (const answers of data.answerList.$values) {
                        slide.innerHTML += `<input type="radio" id="input" value="${answers}" name="answer">${answers}<br>`;
                    }
                    break;
                case "info":
                    console.log("Single choice slide");
                    slide.innerHTML = `<h3> ${data.text} </h3> `;
                    break;
                default:
                    console.log("problem loading data");
                    slide.innerHTML = `<h4>Problem with loading the data</h4>`;
                    break;
            }
        })
    }

    function nextSlide() {
        currentSlide++;
        getSlide()
        updateProgressBar();
        PostAnswer();
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
        await fetch(appUrl + `/api/flow/getMaxNumberOfSlides`, {
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

    function PostAnswer() {
        let answer = getSelectedAnswers();
        let slideId = currentSlide - 1;

        let requestBody = {
            Id: slideId,
            Answer: answer
        };
        console.log(requestBody);
        fetch(appUrl + `/api/flow/PostAnswer`, {
            method: "POST",
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json',
            },
            body: JSON.stringify(requestBody)
        }).then(res => {
            console.log(res)
            if (res.ok) {
                if (res.status === 201) {
                    return res.json();
                }
            }
        }).catch(err => {
            console.log("Something went wrong: " + err);
        })
        console.log(answer);
    }

    function getSelectedAnswers() {
        const checkboxes = document.querySelectorAll('input[name="answer"]:checked');
        let selectedAnswers = [];
        if (checkboxes && checkboxes.length > 0) {
            checkboxes.forEach((checkbox) => {
                    // @ts-ignore
                    selectedAnswers.push(checkbox.value);
                }
            );
        }

        //Get the value of the text input
        const textInput = document.querySelector('input[type="text"]#input');
        // @ts-ignore
        if (textInput && textInput.value) {
            // @ts-ignore
            selectedAnswers.push(textInput.value);
        }
        return selectedAnswers;
    }
});