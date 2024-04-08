

function nextSlide() {
    fetch("http://localhost:7272/api/Installations/nextslide",
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
    fetch("http://localhost:7272/Flow/SetCurrentSlide", {
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
