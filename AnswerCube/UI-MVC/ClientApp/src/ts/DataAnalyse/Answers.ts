import {Chart, registerables} from 'chart.js';
import {RemoveLastDirectoryPartOf} from "../urlDecoder";

const canvas: HTMLCanvasElement | null = document.getElementById('barChart') as HTMLCanvasElement;
const titel: HTMLElement | null = document.getElementById("title");
const dataContainer: HTMLElement | null = document.getElementById("data-container");
const left: HTMLElement | null = document.getElementById("left");
const sessions: HTMLElement | null = document.getElementById("sessions");
const onderaan: HTMLElement | null = document.getElementById("onderaan");
const display: HTMLElement | null = document.getElementById("display")
const spinnerHTML = `
                <p class="loader">loading...</p>
                <div class="spinner">
                  <div></div>
                  <div></div>
                  <div></div>
                  <div></div>
                  <div></div>
                  <div></div>
                  <p>Answer Cube</p>
                </div>
            `;
let answerText: string[] = [];
let possAnswerText: string[] = [];
let slideId: number = 0;
let sessionId: number = 0;
let vraag: string = "";
let slideType: number = 0;
let answersFetched: boolean = false; // Flag to track whether answers have been fetched
let aantalSessions: number = 0;

let currentChart: Chart | null = null; // Keep track of the current chart

document.addEventListener("DOMContentLoaded", async () => {
    if (titel) {
        titel.innerHTML = "<h1>select a slide</h1>";
    }
    if (onderaan) {
        onderaan.innerHTML = spinnerHTML;
        console.log("brrrrr thinking")
        console.log(onderaan)
    } else {
        console.log("hmm");
    }

    await startView();
});

async function startView() {
    await GetAllSlides();
    await GetAllSessions();
}

async function GetAllSlides() {
    var url = window.location.toString();
    fetch(RemoveLastDirectoryPartOf(url) + "/DataAnalyse/Slides", {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
        },
    }).then((response: Response) => {
        if (response.status === 200) {
            return response.json();
        } else {
            if (left) {
                left.innerHTML = "<em>problem!!!</em>";
            }
        }
    }).then((data) => {
        if (data && data.length > 0) {
            console.log("by slide id");
            for (let i = 0; i < data.length; i++) {
                let slide: HTMLElement = document.createElement('div');
                slide.innerHTML = "<button class='slideButton' data-slide-id='" + data[i].id + "'>slide: " + data[i].id + "</button>";
                if (left) {
                    left.appendChild(slide);
                }
            }
            attachSlideEventListeners();
        } else {
            console.log("No data received from the API Slides.");
        }
        if (onderaan) {
            console.log("emptying")
            onderaan.innerHTML = "";
        }
    }).catch((error) => {
        console.error("Error fetching data:", error);
        if (left) {
            left.innerHTML = "<em>Error fetching data!</em>";
        }
    });
}
//
async function GetSlideById(slideId: number) {
    var url = window.location.toString();
    fetch(RemoveLastDirectoryPartOf(url) + "/DataAnalyse/SlideById/" + slideId, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
        },
    }).then((response: Response) => {
        if (response.status === 200) {
            return response.json();
        } else {
            if (left) {
                left.innerHTML = "<em>problem!!!</em>";
            }
        }
    }).then((data) => {
        console.log(data);
        slideType = data.slideType;
        vraag = data.text;
        slideId = data.id;
        possAnswerText = data.answerList; // Corrected assignment
        GetAllAnswers(data.id); // Fetch answers and show graph
    }).catch((error) => {
        console.error("Error fetching data:", error);
        if (left) {
            left.innerHTML = "<em>Error fetching data!</em>";
        }
    });
}
async function GetAllAnswers(slideid: number) {
    var url = window.location.toString();
    fetch(RemoveLastDirectoryPartOf(url) + "/DataAnalyse/Answers", {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
        },
    })
        .then((response: Response) => {
            if (response.status === 200) {
                return response.json();
            } else {
                if (onderaan) {
                    onderaan.innerHTML = "<em>problem!!!</em>";
                }
                throw new Error("Failed to fetch data");
            }
        })
        .then((data) => {
            if (data && data.length > 0) {
                console.log("All answers")
                let answerCounts = new Map<string, number>(); // Map to store answer counts
                for (let i = 0; i < data.length; i++) {
                    if (data[i].slide.id === slideid) {
                        const answer = data[i].answerText.join(', '); // Convert array to string
                        if (answerCounts.has(answer)) {
                            answerCounts.set(answer, answerCounts.get(answer)! + 1); // Increment count if answer exists
                            console.log("checked answercounts");
                        } else {
                            answerCounts.set(answer, 1); // Initialize count if answer doesn't exist
                        }
                    }
                }
                // Extract labels and counts for the chart dataset
                const labels = Array.from(answerCounts.keys());
                const counts = Array.from(answerCounts.values());
                // Set the flag to indicate answers have been fetched
                answersFetched = true;
                // Check if the slide ID matches the current slide being displayed
                showGraph(counts, labels, slideid); // Pass counts and labels to showGraph function
            } else {
                console.log("No data received from the API Answers.");
            }
        })
        .catch((error) => {
            console.error("Error fetching data:", error);
            if (onderaan) {
                onderaan.innerHTML = "<em>Error fetching data!</em>";
            }
        });
}

async function showGraph(dataSet: Array<number>, labelSet: Array<string>, slideid: number) {
    // Register necessary components
    Chart.register(...registerables);
    if (onderaan) {
        onderaan.innerHTML = ""
    }
    // Get canvas element
    const ctx = canvas?.getContext('2d');
    if (canvas) {
        // Destroy existing chart if it exists
        if (currentChart) {
            currentChart.destroy();
        }

        // Clear the canvas innerHTML in case it contains spinner
        canvas.innerHTML = '';

        // Create new chart
        currentChart = new Chart(canvas, {
            type: 'bar',
            data: {
                labels: labelSet,
                datasets: [{
                    label: vraag,
                    data: dataSet,
                    backgroundColor: 'rgba(54, 162, 235, 0.2)',
                    borderColor: 'rgba(54, 162, 235, 1)',
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });
    } else {
        console.error('Failed to get 2d context for canvas element.');
    }
}


async function GetAllSessions() {
    const url = window.location.toString();
    try {
        const response = await fetch(RemoveLastDirectoryPartOf(url) + "/DataAnalyse/Sessions", {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
            },
        });

        if (response.status !== 200) {
            if (left) {
                left.innerHTML = "<em>problem!!!</em>";
            }
            return;
        }

        const data = await response.json();
        if (data && data.length > 0) {
            console.log("all sessions checked");
            console.log(data.length);
            aantalSessions = data.length;

            // Sort sessions by session ID in descending order
            data.sort((a: { id: number }, b: { id: number }) => b.id - a.id);

            // Fetch and display each session in sorted order
            for (let i = 1; i < aantalSessions+1; i++) {
                let sessionTitle: HTMLElement = document.createElement('div');
                sessionTitle.innerHTML = `<button class='sessionButton' data-session-id='${i}'>session: ${i}</button>`;
                if (sessions) {
                    console.log("appending to sessions");
                    sessions.appendChild(sessionTitle);
                }
                console.log("session: " + data[i]);
            }
            attachSessionEventListeners();
        } else {
            console.log("No data received from the API Sessions.");
        }
    } catch (error) {
        console.error("Error fetching data:", error);
        if (left) {
            left.innerHTML = "<em>Error fetching data!</em>";
        }
    }
}
function GetSessionById(sessionId: number) {
    const url = window.location.toString();
    fetch(RemoveLastDirectoryPartOf(url) + "/DataAnalyse/AnswersBySessionId/" + sessionId, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
        },
    }).then((response: Response) => {
        if (response.status === 200) {
            return response.json();
        } else {
            if (sessions) {
                sessions.innerHTML = "<em>problem!!!</em>";
            }
        }
    }).then((data) => {
        console.log(data);
        if (display) {
            let infoKader: HTMLElement = document.createElement('div');
            infoKader.classList.add('AnswerFromSession');
            console.log(data.length);
            for (let i = 0; i < data.length; i++) {
                console.log("haha werkt lolhaha");
                let itemDiv: HTMLElement = document.createElement('div');
                let id = data[i].slide.id;
                if (data[i].answerText.length > 0) {
                    itemDiv.innerHTML = "<h1>Question: " + data[i].slide.text + "</h1>"
                        + "<h1>answer: " + data[i].answerText + "</h1>" +
                        "<button class='slideButton' data-slide-id='" + id + "'>slide: " + data[i].slide.id + "</button>";
                    infoKader.appendChild(itemDiv);
                }
            }
            console.log("appending");
            display.appendChild(infoKader);
            attachSlideEventListeners();
        } else {
            console.log("No data received from the API GetSessionById.");
        }
    }).catch((error) => {
        console.error("Error fetching data:", error);
        if (sessions) {
            sessions.innerHTML = "<em>Error fetching data!</em>";
        }
    });
}


// EVENT LISTENERS
function attachSessionEventListeners() {
    const sessionButtons = document.querySelectorAll('.sessionButton');
    sessionButtons.forEach(button => {
        button.addEventListener('click', (event) => {
            const target = event.target as HTMLElement;
            const sessionIdString = target.getAttribute('data-session-id');
            if (display) {
                display.innerHTML = '';
            }
            if (sessionIdString) {
                const sessionId = parseInt(sessionIdString, 10);
                console.log('Session button clicked, session ID:', sessionId);
                GetSessionById(sessionId);   

                // Highlight the clicked button and remove highlight from others
                sessionButtons.forEach(btn => btn.classList.remove('active'));
                target.classList.add('active');
            } else {
                console.error('Invalid session ID');
            }
        });
    });
}

function attachSlideEventListeners() {
    const slideButtons = document.querySelectorAll('.slideButton');
    slideButtons.forEach(button => {
        button.addEventListener('click', (event) => {
            if (onderaan) {
                onderaan.innerHTML = spinnerHTML;
            }
            const target = event.target as HTMLElement;
            const slideIdString = target.getAttribute('data-slide-id');
            if (slideIdString) {
                const slideId = parseInt(slideIdString, 10);
                console.log('Slide button clicked, slide ID:', slideId);
                GetSlideById(slideId);
            } else {
                console.error('Invalid slide ID');
            }
        });
    });
}