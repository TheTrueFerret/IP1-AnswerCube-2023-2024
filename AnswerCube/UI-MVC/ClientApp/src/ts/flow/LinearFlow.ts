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

document.addEventListener("DOMContentLoaded", function () {
    const nextBtn = document.getElementById("next");
    const slide = document.getElementById("slide");
    let maxSlide = 7
    let currentSlide = 1
    addListeners()
    //getMaxSlides()
    nextSlide()
    
    function addListeners() {
        if (nextBtn) {
            nextBtn.addEventListener("click", nextSlide);
        }
    }
    
    function getSlide() {
        if (currentSlide >= maxSlide) {
            currentSlide = 1
            console.log("OUT OF SLIDES")
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
            console.log(data)
            let slideText = data.text;
            slide.innerHTML = `<h3> ${slideText} </h3> `;
            if (data.question !== null && data.question !== undefined){
                console.log("info slide");
                slide.innerHTML = `<h4> ${data.question} </h4> <br> <h3> ${slideText} </h3>`;
                return
            }
            if (data.isMultipleChoice !== null && data.isMultipleChoice !== undefined ) {
                if (data.isMultipleChoice === true) {
                    console.log("multiple choice slide");
                    slide.innerHTML = `<h3> ${slideText} </h3> `;
                    for (const answers of data.answerList.$values) {
                        slide.innerHTML += `<input type="checkbox" id="input" value="${answers}" name="answer">${answers}<br>`;
                    }
                    return;
                } else{
                    console.log("Single choice slide");
                    for (const answers of data.answerList.$values) {
                        slide.innerHTML += `<input type="radio" id="input" value="${answers}" name="answer">${answers}<br>`;
                    }
                    return;
                }
            }
            slide.innerHTML += `<input type="text" id="input" value="" placeholder="Answer the question.">`;
        })
    }

    function nextSlide() {
        getSlide()
        currentSlide++;
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