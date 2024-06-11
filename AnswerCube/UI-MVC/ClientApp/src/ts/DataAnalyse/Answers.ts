import {RemoveLastDirectoryPartOf} from "../urlDecoder";
import {Chart, registerables} from "chart.js";

interface Answer {
    id: number;
    slide: {
        id: number;
        text: string;
    };
    answerText: string[];
}

interface Slide {
    id: number;
    text: string;
    slideType: number;
}
interface Session {
    id: number;
    cubeId: number;
    startTime: string;
}

const body: HTMLElement | null = document.querySelector("body");
const downloadCSVButton: HTMLElement | null = document.getElementById("download-csv-button");
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
let answersFetched: boolean = false;
let aantalSessions: number = 0;

let currentChart: Chart | null = null;
let allAnswers: Answer[] = [];

document.addEventListener("DOMContentLoaded", async () => {
    if (titel) {
        titel.innerHTML = "<h1>select a session</h1>";
    }
    if (onderaan) {
        onderaan.innerHTML = spinnerHTML;
        console.log("Loading data...");
    } else {
        console.log("Spinner element not found");
    }
    if (body) {
        body.classList.remove("bg-light");
        body.classList.add("achtergrondkleur");
    }
    
    await startView();
    await createSlideButtons(); // Create slide buttons after loading data
    
    const searchButton: HTMLElement | null = document.getElementById("search-button");
    const searchInput: HTMLInputElement | null = document.getElementById("search-input") as HTMLInputElement;

    if (searchButton && searchInput) {
        searchButton.addEventListener("click", () => {
            const query = searchInput.value.trim().toLowerCase();
            if (query) {
                searchAnswers(query);
            }
        });

        searchInput.addEventListener("keypress", (event) => {
            if (event.key === "Enter") {
                const query = searchInput.value.trim().toLowerCase();
                if (query) {
                    searchAnswers(query);
                }
            }
        });
    }
    if (downloadCSVButton) {
        downloadCSVButton.addEventListener("click", () => {
            downloadCSV();
        });
    }
});


async function startView() {
    await GetAllSessions();
    await GetAllAnswers(0);
    /*
    * toon eerste slide
    * */
    await GetSlideById(1);
    await processAnswers(1)
    await getSessionById(1);
}

function searchAnswers(query: string) {
    console.log("Searching answers with query:", query);
    const filteredAnswers = allAnswers.filter(answer => answer.slide.text.toLowerCase().includes(query));
    console.log("Filtered answers:", filteredAnswers);
    displayFilteredAnswers(filteredAnswers);
}


function formatTime(dateTimeString: string): string {
    const date = new Date(dateTimeString);
    return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
}
function displayFilteredAnswers(filteredAnswers: Answer[]) {
    console.log("Displaying filtered answers:", filteredAnswers);
    window.scrollTo({ top: 100 })
    if (display) {
        display.innerHTML = '';
        if (filteredAnswers.length === 0) {
            display.innerHTML = "<p>No answers found for the given query.</p>";
            return;
        }

        let infoKader: HTMLElement = document.createElement('div');
        infoKader.classList.add('AnswerFromSession');

        filteredAnswers.forEach(answer => {
            let itemDiv: HTMLElement = document.createElement('div');
            const slideId = answer.slide.id;
            const question = answer.slide.text;
            itemDiv.innerHTML = `<h1>Question: ${question}</h1>
                                 <h1>Answer: ${answer.answerText.join(', ')}</h1>
                                 <button class='slideButton' data-slide-id='${slideId}'>slide ${slideId}</button>`;
            infoKader.appendChild(itemDiv);
        });
        display.appendChild(infoKader);

        attachSlideEventListeners();
    } else {
        console.error("Display element not found.");
    }
}

async function GetAllSlides(): Promise<Slide[]> {
    const url = window.location.toString();
    try {
        const response = await fetch(RemoveLastDirectoryPartOf(url) + "/DataAnalyse/Slides", {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
            },
        });

        if (response.status !== 200) {
            if (left) {
                left.innerHTML = "<em>problem!!!</em>";
            }
            return [];
        }

        const data: Slide[] = await response.json();
        if (data && data.length > 0) {
            console.log("Loaded slides");
            return data;
        } else {
            console.log("No data received from the API Slides.");
            return [];
        }
    } catch (error) {
        console.error("Error fetching data:", error);
        if (left) {
            left.innerHTML = "<em>Error fetching data!</em>";
        }
        return [];
    }
}

async function downloadCSV() {
    const encodedUri = await generateCSV();

    const link = document.createElement("a");
    link.setAttribute("href", encodedUri);
    link.setAttribute("download", "data.csv");
    document.body.appendChild(link);

    link.click();
}

async function generateCSV() {
    let csvContent = "data:text/csv;charset=utf-8";
    const allSlides = await GetAllSlides();
    csvContent += "\n\n\nAnswer ID;Slide ID;Answer Text\n";
    const allAnswers = await GetAllAnswers(0);
    console.log("allAnswers");
    console.log(JSON.stringify(allAnswers));
    allAnswers.forEach(answer => {
        csvContent += `${answer.id};${answer.slide.id};${answer.answerText.join('|')}\n`;
    });
    const encodedUri = encodeURI(csvContent);
    return encodedUri;
}

async function GetSlideById(slideId: number) {
    const url = window.location.toString();
    try {
        const response = await fetch(RemoveLastDirectoryPartOf(url) + "/DataAnalyse/SlideById/" + slideId, {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
            },
        });

        if (response.status !== 200) {
            if (left) {
                left.innerHTML = "<em>problem!!!</em>";
            }
            throw new Error("Failed to fetch slide data");
        }

        const data = await response.json();
        console.log(data);
        slideType = data.slideType;
        vraag = data.text;
        slideId = data.id;
        possAnswerText = data.answerList;

        const answers = await GetAllAnswers(slideId);
        processAnswers(slideId);
    } catch (error) {
        console.error("Error fetching data:", error);
        if (left) {
            left.innerHTML = "<em>Error fetching data!</em>";
        }
    }
}

async function GetAllAnswers(slideid: number): Promise<Answer[]> {
    const url = window.location.toString();
    try {
        const response = await fetch(RemoveLastDirectoryPartOf(url) + "/DataAnalyse/Answers", {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
            },
        });

        if (response.status !== 200) {
            if (onderaan) {
                onderaan.innerHTML = "<em>problem!!!</em>";
            }
            throw new Error("Failed to fetch data");
        }

        const data: Answer[] = await response.json();
        if (data && data.length > 0) {
            allAnswers = data; // Ensure allAnswers is populated
            console.log("Fetched answers:", allAnswers);
            if (slideid !== 0) {
                processAnswers(slideid);
            }
            return data;
        } else {
            console.log("No data received from the API Answers.");
            return [];
        }
    } catch (error) {
        console.error("Error fetching data:", error);
        if (onderaan) {
            onderaan.innerHTML = "<em>Error fetching data!</em>";
        }
        return [];
    }
}

function processAnswers(slideid: number) {
    let answerCounts = new Map<string, number>();
    for (let i = 0; i < allAnswers.length; i++) {
        if (allAnswers[i].slide.id === slideid) {
            const answer = allAnswers[i].answerText.join(', ');
            if (answerCounts.has(answer)) {
                answerCounts.set(answer, answerCounts.get(answer)! + 1);
                console.log("checked answercounts");
            } else {
                answerCounts.set(answer, 1);
            }
        }
    }
    const labels = Array.from(answerCounts.keys());
    const counts = Array.from(answerCounts.values());
    answersFetched = true;
    if (onderaan) {
        onderaan.innerHTML = "";
    }
    showGraph(counts, labels, slideid);
}

async function showGraph(dataSet: Array<number>, labelSet: Array<string>, slideid: number) {
    window.scrollTo({top: 0})
    Chart.register(...registerables);
    if (onderaan) {
        onderaan.innerHTML = ""
    }
    const ctx = canvas?.getContext('2d');
    if (canvas) {
        if (currentChart) {
            currentChart.destroy();
        }
        canvas.innerHTML = '';

        const backgroundColors = [
            'rgba(255, 99, 132, 0.5)',
            'rgba(54, 162, 235, 0.5)',
            'rgba(255, 206, 86, 0.5)',
            'rgba(75, 192, 192, 0.5)',
            'rgba(153, 102, 255, 0.5)',
            'rgba(255, 159, 64, 0.5)',
            'rgba(199, 199, 199, 0.5)',
            'rgba(83, 102, 255, 0.5)',
            'rgba(255, 159, 255, 0.5)',
            'rgba(132, 255, 132, 0.5)'
        ];

        const borderColors = [
            'rgba(255, 99, 132, 1)',
            'rgba(54, 162, 235, 1)',
            'rgba(255, 206, 86, 1)',
            'rgba(75, 192, 192, 1)',
            'rgba(153, 102, 255, 1)',
            'rgba(255, 159, 64, 1)',
            'rgba(199, 199, 199, 1)',
            'rgba(83, 102, 255, 1)',
            'rgba(255, 159, 255, 1)',
            'rgba(132, 255, 132, 1)'
        ];

        const colorsCount = dataSet.length;
        const backgroundColorsArray = backgroundColors.slice(0, colorsCount);
        const borderColorsArray = borderColors.slice(0, colorsCount);

        currentChart = new Chart(canvas, {
            type: 'bar',
            data: {
                labels: labelSet,
                datasets: [{
                    label: vraag,
                    data: dataSet,
                    backgroundColor: backgroundColorsArray,
                    borderColor: borderColorsArray,
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    y: {
                        ticks: {
                            font: {
                                size: 16
                            },
                            stepSize: 1
                        },
                        title: {
                            display: true,
                            text: 'Number of Responses' // Label for the y-axis
                        }
                    },
                    x: {
                        ticks: {
                            font: {
                                size: 16
                            },
                        },
                        title: {
                            display: true,
                            text: 'Possible Answers' // Label for the x-axis
                        }
                    }
                },
                plugins: {
                    title: {
                        font: {
                            size: 20
                        },
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

        const data: Session[] = await response.json();
        if (data && data.length > 0) {
            console.log("all sessions checked");
            console.log(data.length);
            aantalSessions = data.length;

            data.sort((a: Session, b: Session) => b.id - a.id);
            data.forEach((session: Session) => {
                const startTime = new Date(session.startTime);
                const formattedStartTime = startTime.toLocaleString();

                let sessionTitle: HTMLElement = document.createElement('div');
                sessionTitle.innerHTML = `<button class='sessionButton' data-session-id='${session.id}'>User ${session.id}
                                <br>Started the flow at <br>${formattedStartTime}
                                <br>And used cube: ${session.cubeId}</button>`;
                if (sessions) {
                    console.log("appending to sessions");
                    sessions.appendChild(sessionTitle);
                }
                console.log("session: " + session.id);
            });
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

function getSessionById(sessionId: number) {
    const url = window.location.toString();
    fetch(RemoveLastDirectoryPartOf(url) + "/DataAnalyse/SessionById/" + sessionId, {
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
        console.log("SessionByIdData");
        console.log(data);
        let installation = data.installation;
        const formattedStartTime = formatTime(data.startTime);
        const formattedEndTime = formatTime(data.endTime);
        let session: HTMLElement = document.createElement('div');
        session.innerHTML = "<p>cube: " + data.cubeId + "</p>" +
            "<p>startTime: <p>" + formattedStartTime + "</p></p>" +
            "<p>endTime: <p>" + formattedEndTime + "</p></p>" + 
            "<p>Installation: <p>" + installation.name + "</p></p>" +
            "<p>At location: <p>" + installation.location + "</p></p>";
        if (left) {
            left.innerHTML = "";
            left.appendChild(session);
        }
    }).catch((error) => {
        console.error("Error fetching data:", error);
        if (sessions) {
            sessions.innerHTML = "<em>Error fetching data!</em>";
        }
    });
}


function AnswersBySessionId(sessionId: number) {
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
        if (Array.isArray(data) && data.length > 0) {
            console.log(data);
            if (display) {
                let infoKader: HTMLElement = document.createElement('div');
                infoKader.classList.add('AnswerFromSession');
                const firstAnswer = data[0];
                const firstSlideId = firstAnswer.slide.id;
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
                processAnswers(firstSlideId);
                console.log("appending");
                display.appendChild(infoKader);
                attachSlideEventListeners();
            } else {
                console.log("No data received from the API AnswersBySessionId.");
            }
        } else {
            console.log("Invalid data received from API AnswersBySessionId.");
        }
    }).catch((error) => {
        console.error("Error fetching data:", error);
        if (sessions) {
            sessions.innerHTML = "<em>Error fetching data!</em>";
        }
    });
}
async function createSlideButtons() {
    const slides = await GetAllSlides();
    const slidesContainer: HTMLElement | null = document.getElementById("slides-container");

    if (slidesContainer) {
        slidesContainer.innerHTML = ''; 
        slides.forEach(slide => {
            const slideButton: HTMLElement = document.createElement('button');
            slideButton.className = 'slideButton';
            slideButton.innerText = `Vraag ${slide.id}`;
            slideButton.setAttribute('data-slide-id', slide.id.toString());
            slidesContainer.appendChild(slideButton);
        });

        attachSlideButtonEventListeners();
    } else {
        console.error("Slides container not found.");
    }
}

function attachSlideButtonEventListeners() {
    const slideButtons = document.querySelectorAll('.slideButton');
    slideButtons.forEach(button => {
        button.addEventListener('click', (event) => {
            const target = event.target as HTMLElement;
            const slideIdString = target.getAttribute('data-slide-id');
            if (slideIdString) {
                const slideId = parseInt(slideIdString, 10);
                console.log('Slide button clicked, slide ID:', slideId);
                GetSlideById(slideId);
                processAnswers(slideId); 
            } else {
                console.error('Invalid slide ID');
            }
        });
    });
}

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
                AnswersBySessionId(sessionId);
                getSessionById(sessionId);
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