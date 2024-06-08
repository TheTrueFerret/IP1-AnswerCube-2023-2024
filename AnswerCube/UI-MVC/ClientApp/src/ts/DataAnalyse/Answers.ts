import { RemoveLastDirectoryPartOf } from "../urlDecoder";
import { Chart, registerables } from "chart.js";

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
}

const downloadCSVButton: HTMLElement | null = document.getElementById("download-csv-button");
const canvas: HTMLCanvasElement | null = document.getElementById('barChart') as HTMLCanvasElement;
const titel: HTMLElement | null = document.getElementById("title");
const dataContainer: HTMLElement | null = document.getElementById("data-container");
const left: HTMLElement | null = document.getElementById("left");
const sessions: HTMLElement | null = document.getElementById("sessions");
const onderaan: HTMLElement | null = document.getElementById("onderaan");
const display: HTMLElement | null = document.getElementById("display");

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
        titel.innerHTML = "<h1>Select a session</h1>";
    }
    if (onderaan) {
        onderaan.innerHTML = spinnerHTML;
        console.log("Loading data...");
    } else {
        console.log("Spinner element not found");
    }

    await startView();

    const searchButton: HTMLElement | null = document.getElementById("search-button");
    const searchInput: HTMLInputElement | null = document.getElementById("search-input") as HTMLInputElement;

    if (searchButton && searchInput) {
        console.log("Search button and input found");
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
        console.log("Download CSV button found");
        downloadCSVButton.addEventListener("click", () => {
            downloadCSV();
        });
    }
});

async function startView() {
    console.log("Start view");
    await GetAllSessions();
    await GetAllAnswers(0);
    await processAnswers(1);
    await getSessionById(1);
}

function searchAnswers(query: string) {
    const filteredAnswers = allAnswers.filter(answer => answer.slide.text.toLowerCase().includes(query));
    displayFilteredAnswers(filteredAnswers);
}

function formatTime(dateTimeString: string): string {
    const date = new Date(dateTimeString);
    return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
}

function displayFilteredAnswers(filteredAnswers: Answer[]) {
    window.scrollTo({ top: 100 });
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
                                 <button class='slideButton' data-slide-id='${slideId}'>Slide ${slideId}</button>`;
            infoKader.appendChild(itemDiv);
        });
        display.appendChild(infoKader);

        attachSlideEventListeners();
    } else {
        console.error("Display element not found.");
    }
}

async function GetAllSlides(): Promise<Slide[]> {
    console.log("Get all slides");
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
                left.innerHTML = "<em>Problem!!!</em>";
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
    allAnswers.forEach(answer => {
        csvContent += `${answer.id};${answer.slide.id};${answer.answerText.join('|')}\n`;
    });
    const encodedUri = encodeURI(csvContent);
    return encodedUri;
}

async function GetSlideById(slideId: number) {
    console.log("Get slide by ID");
    const url = window.location.toString();
    try {
        const response = await fetch(RemoveLastDirectoryPartOf(url) + "/DataAnalyse/SlideById/" + slideId, {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
            },
        });

        if (response.status === 200) {
            const data = await response.json();
            slideType = data.slideType;
            vraag = data.text;
            possAnswerText = data.answerList;
            await GetAllAnswers(data.id);
        } else {
            if (left) {
                left.innerHTML = "<em>Problem!!!</em>";
            }
        }
    } catch (error) {
        console.error("Error fetching data:", error);
        if (left) {
            left.innerHTML = "<em>Error fetching data!</em>";
        }
    }
}

async function GetAllAnswers(slideid: number): Promise<Answer[]> {
    console.log("Get all answers");
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
                onderaan.innerHTML = "<em>Problem!!!</em>";
            }
            throw new Error("Failed to fetch data");
        }

        const data: Answer[] = await response.json();
        if (data && data.length > 0) {
            allAnswers = data; // Save the answers to allAnswers
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
    console.log("Process answers");
    let answerCounts = new Map<string, number>();
    for (let i = 0; i < allAnswers.length; i++) {
        if (allAnswers[i].slide.id === slideid) {
            for (let j = 0; j < allAnswers[i].answerText.length; j++) {
                let answer = allAnswers[i].answerText[j];
                answerCounts.set(answer, (answerCounts.get(answer) || 0) + 1);
            }
        }
    }

    let labels = Array.from(answerCounts.keys());
    let counts = Array.from(answerCounts.values());

    if (canvas) {
        const ctx = canvas.getContext('2d');
        if (ctx) {
            if (currentChart) {
                currentChart.destroy();
            }
            currentChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Number of Answers',
                        data: counts,
                        backgroundColor: 'rgba(54, 162, 235, 0.2)',
                        borderColor: 'rgba(54, 162, 235, 1)',
                        borderWidth: 1,
                    }],
                },
                options: {
                    scales: {
                        y: {
                            beginAtZero: true,
                        },
                    },
                },
            });
        }
    } else {
        console.error("Canvas element not found.");
    }
}

async function getSessionById(sessionId: number) {
    const url = window.location.toString();
    try {
        const response = await fetch(RemoveLastDirectoryPartOf(url) + "/DataAnalyse/SessionById/" + sessionId, {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
            },
        });

        if (response.status === 200) {
            const data = await response.json();
            answerText = data.answerList;
        } else {
            if (left) {
                left.innerHTML = "<em>Problem!!!</em>";
            }
        }
    } catch (error) {
        console.error("Error fetching data:", error);
        if (left) {
            left.innerHTML = "<em>Error fetching data!</em>";
        }
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
                left.innerHTML = "<em>Problem!!!</em>";
            }
            return;
        }

        const data: Session[] = await response.json();
        if (data && data.length > 0) {
            aantalSessions = data.length;
            data.forEach(session => {
                let button = document.createElement('button');
                button.textContent = `Session ${session.id}`;
                button.setAttribute('data-session-id', session.id.toString());
                button.classList.add('session-button');
                if (sessions) {
                    sessions.appendChild(button);
                }
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

function attachSessionEventListeners() {
    const sessionButtons = document.querySelectorAll('.session-button');
    sessionButtons.forEach(button => {
        button.addEventListener('click', async (event) => {
            const target = event.target as HTMLElement;
            const sessionId = parseInt(target.getAttribute('data-session-id') || '0');
            if (sessionId) {
                await GetSlideById(sessionId);
            }
        });
    });
}

function attachSlideEventListeners() {
    const slideButtons = document.querySelectorAll('.slideButton');
    slideButtons.forEach(button => {
        button.addEventListener('click', async (event) => {
            const target = event.target as HTMLElement;
            const slideId = parseInt(target.getAttribute('data-slide-id') || '0');
            if (slideId) {
                await GetSlideById(slideId);
            }
        });
    });
}
