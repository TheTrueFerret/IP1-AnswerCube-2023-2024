
let maxSlide = 0;
export let currentSlideIndex = 1;
export let ActiveSlideList;
let activeScript


function GetNextSlideList(slideListId) {
    fetch("http://localhost:5104/CircularFlow/InitializeFlow/",
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ slideListId: slideListId })
        })
        .then(response => {
            if (response.status === 200) {
                return response.json();
            } else {
                document.getElementById("slide").innerHTML = "<em>problem!!!</em>";
            }
        }).then(SlideList => {
        ActiveSlideList = SlideList
        maxSlide = SlideList.slides.length
    })
        .catch(error => {
            console.error(error);
            document.getElementById("slide").innerHTML = "<em>Problem loading the slide</em>";
        });
}

GetNextSlideList(1);


function UpdatePage() {
    fetch("http://localhost:5104/CircularFlow/NextSlide/",
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({currentSlideIndex: currentSlideIndex, slideListId: ActiveSlideList.id})
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


function loadScriptsForCurrentPage() {
    var views = document.querySelectorAll("[data-script]");
    views.forEach(function(view) {
        var scriptUrl = view.getAttribute("data-script");
        loadScript(scriptUrl);
    });
}

function loadScript(scriptUrl) {
    var script = document.createElement('script');
    script.src = scriptUrl;
    script.defer = true;
    document.body.appendChild(script);
    //observer.disconnect();
}

const btn = document.getElementById("nextSlide");
btn.addEventListener('click', nextSlide);


function nextSlide() {
    UpdatePage();
    currentSlideIndex++;
    loadScriptsForCurrentPage()
}
