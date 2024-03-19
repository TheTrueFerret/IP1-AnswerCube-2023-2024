
// function getSlideList() {
//     fetch("http://localhost:5104/api/Installation",
//         {
//             method: "GET",
//             headers: {
//                 "Accept": "application/json"
//             }
//         })
//         .then(response => {
//             if (response.status === 200) {
//                 return response.json();
//             } else {
//                 document.getElementById("page").innerHTML = "<em>Problem!!!</em>";
//             }
//         })
//         .then(slideList => {
//             console.log(slideList);
//             updateCondition(slideList[1])
//         })
// }
//
// function updateCondition(slide) {
//     fetch("http://localhost:5104/Flow/SetCurrentCondition", {
//         method: "POST",
//         headers: {
//             'Content-Type': 'application/json',
//             'Accept': 'application/json',
//         },
//         body: JSON.stringify(slide)
//     }).then(res => {
//         if(res.ok) {
//             if(res.status === 201) {
//                 return res.json();
//             }
//         } else {
//             alert("No 2xx code returned")
//         }
//     }).catch(err => {
//         alert("Something went wrong: " + err);
//     })
// }

