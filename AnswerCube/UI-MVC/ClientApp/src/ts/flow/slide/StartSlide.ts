

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
                fetchPartialView()
            } else {
                throw new Error("Failed to update current slide");
            }
        })
        .catch(error => {
            console.error("Error updating current slide:", error);
        });
}


function fetchPartialView() {
    fetch("http://localhost:5104/Flow/CircularFlow/", { // Update the URL accordingly
        method: "GET",
        headers: {
            "Accept": "text/html" // Expect HTML response
        }
    })
        .then(response => {
            if (response.ok) {
                return response.text();
            } else {
                throw new Error("Failed to fetch partial view");
            }
        })
        .then(html => {
            // Update the DOM with the fetched partial view
            document.getElementById("page").innerHTML = html;
        })
        .catch(error => {
            console.error("Error fetching partial view:", error);
        });
}

const btn = document.getElementById("nextSlide").addEventListener('click', nextSlide);
