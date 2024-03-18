
function getSlideList() {
    fetch("http://localhost:5104/api/Slides",
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
                document.getElementById("page").innerHTML = "<em>Problem!!!</em>";
            }
        })
        .then(slideList => {
            console.log(slideList);
            updateCondition(slideList[1])
        })
}

function updateCondition(newCondition) {
    fetch("http://localhost:5104/api/Slides", {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
        },
        body: JSON.stringify(newCondition)
    }).then(res => {
        if(res.ok) {
            if(res.status === 201) {
                return res.json();
            }
        } else {
            alert("No 2xx code returned")
        }
    }).catch(err => {
        alert("Something went wrong: " + err);
    })
}


