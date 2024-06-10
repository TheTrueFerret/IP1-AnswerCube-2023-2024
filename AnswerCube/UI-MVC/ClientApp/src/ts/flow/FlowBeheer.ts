import * as signalR from '@microsoft/signalr';
import {getDomainFromUrl, RemoveLastDirectoryPartOf} from "../urlDecoder";
import * as url from "node:url";

let connectionStarted = false;
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/flowHub")
    .build();

class CountdownTimer {
    private countdown: number;
    private timeoutId: ReturnType<typeof setTimeout> | null;
    private timerElement: HTMLElement | null;

    constructor() {
        this.countdown = 45;
        this.timeoutId = null;
        this.timerElement = null;
    }

    private countdownFunction = () => {
        try {
            if (this.countdown >= 0) {
                if (this.timerElement) {
                    this.timerElement.innerText = this.countdown.toString();
                }
                this.countdown--;
                this.timeoutId = setTimeout(this.countdownFunction, 1000); // Call the function again after 1 second
            } else {
                // Go To Next Page
                console.log(RemoveLastDirectoryPartOf(window.location.toString()) + "/UpdatePage/")
                fetch(RemoveLastDirectoryPartOf(window.location.toString()) + "/UpdatePage/", {
                    method: "GET",
                    headers: {
                        'Accept': 'application/json',
                    }
                }).then((response: Response) => {
                    if (response.status === 200) {
                        console.log('Network response was ok');
                        return response.json();
                    } else {
                        console.error('Network response was not ok');
                    }
                }).then((url: any) => {
                    if (url.url) {
                        window.location.href = getDomainFromUrl(window.location.toString()) + url.url;
                    }
                }).catch(err => {
                    console.log("Something went wrong: " + err);
                })
            }
        } catch (error) {
            console.error('Error in countdownFunction:', error); // Log any errors in countdownFunction
        }
    };

    public startCountdown = () => {
        if (this.timeoutId === null) {
            console.log('Starting countdown for:', this.timeoutId);
            this.countdownFunction(); // Start the countdown immediately
            console.log('Timeout ID after start:', this.timeoutId);
        } else {
            console.log('Countdown already running with Timeout ID:', this.timeoutId);
        }
    };

    public pauseCountdown = () => {
        if (this.timeoutId !== null) {
            console.log('Pausing countdown');
            clearTimeout(this.timeoutId); // Pause the countdown by clearing the timeout
            console.log('Clearing timeout ID:', this.timeoutId);
            this.timeoutId = null;
            console.log('Timeout ID after pause:', this.timeoutId);
        } else {
            console.log('No active countdown to pause');
        }
    };

    public setTimerElement(element: HTMLElement | null) {
        this.timerElement = element;
    }
    
    public EstablishConnection = () => {
        if (!connectionStarted) {
            console.log('Connection started');
            const connectionUrl = connection.connectionId;
            console.log('ConnectionId: ' + connectionUrl);
            fetch(`/Installation/SetInstallationUrl/${connectionUrl}`, {
                method: 'POST',
            }).then(response => {
                if (!response.ok) {
                    console.error('Failed to start flow');
                }
                console.log('ConnectionId sent to server after starting connection');
            });
            console.log('Starting countdown after connection is established');
            this.startCountdown(); // Start the countdown after the SignalR connection is established
            connectionStarted = true;
        }
    }
}

document.addEventListener('DOMContentLoaded', function () {
    console.log('FlowBeheer loaded');
    const timerElement = document.getElementById('timer');
    const countdownTimer = new CountdownTimer();
    countdownTimer.setTimerElement(timerElement);

    connection.on("StartFlow", function () {
        console.log('Flow resumed');
        countdownTimer.startCountdown();
    });

    connection.on("StopFlow", function () {
        console.log('Flow paused');
        countdownTimer.pauseCountdown();
    });

    connection.on("StopInstallation", function () {
        console.log('Installation stopped');
        window.location.href = getDomainFromUrl(window.location.toString()) + "/Installation/ChooseInstallation";
    });

    connection.onreconnected(connectionId => {
        console.log(`Connection reestablished. Connected with connectionId "${connectionId}".`);
        countdownTimer.EstablishConnection()
    });

    connection.onreconnecting(error => {
        console.log(`Connection lost due to error "${error}". Reconnecting.`);
    });

    connection.onclose(error => {
        console.log(`Connection closed due to error "${error}". Try refreshing this page to restart the connection.`);
        connectionStarted = false; // Reset the connectionStarted variable
        // Attempt to restart the connection
        setTimeout(() => {
            console.log('Attempting to restart connection...');
            connection.start()
                .then(() => {
                    countdownTimer.EstablishConnection();
                    console.log('Connection successfully restarted.');
                })
                .catch(err => console.log('Error while restarting connection: ' + err));
        }, 5000); // Wait 5 seconds before attempting to restart the connection
    });

    connection.start()
        .then(() => {
            countdownTimer.EstablishConnection();
        })
        .catch(err => console.log('Error while starting connection: ' + err));
});


