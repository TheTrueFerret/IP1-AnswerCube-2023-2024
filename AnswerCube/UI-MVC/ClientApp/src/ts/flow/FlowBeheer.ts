import * as signalR from "@microsoft/signalr";

document.addEventListener('DOMContentLoaded', function () {

    console.log('FlowBeheer loaded')

    let countdown: number = 20;
    let intervalId: ReturnType<typeof setInterval> | null = null;
    const timerElement = document.getElementById('timer');

    const countdownFunction = () => {
        try {
            if (countdown >= 0) {
                if (timerElement) {
                    timerElement.innerText = countdown.toString();
                }
                countdown--;
            } else {
                if (intervalId !== null) {
                    clearInterval(intervalId);
                    intervalId = null;
                }
                // Reset the countdown and start again
                countdown = 20;
                intervalId = setInterval(countdownFunction, 1000);
            }
        } catch (error) {
            console.error('Error in countdownFunction:', error); // Log any errors in countdownFunction
        }
    };

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/flowHub")
        .build();

    connection.on("StartFlow", function () {
        if (intervalId === null) {
            intervalId = setInterval(countdownFunction, 1000);
            console.log('Flow started')
        }
    });

    connection.on("StopFlow", function () {
        if (intervalId !== null) {
            clearInterval(intervalId);
            intervalId = null;
            console.log('Flow stopped')
        }
    });

    connection.onreconnected(connectionId => {
        console.log(`Connection reestablished. Connected with connectionId "${connectionId}".`);
    });

    connection.onreconnecting(error => {
        console.log(`Connection lost due to error "${error}". Reconnecting.`);
    });

    connection.onclose(error => {
        console.log(`Connection closed due to error "${error}". Try refreshing this page to restart the connection.`);
    });

    connection.start()
        .then(() => {
                console.log('Connection started');
                const connectionUrl = connection.connectionId
                console.log('ConnectionId: ' + connectionUrl)
                fetch(`/Installation/SetInstallationUrl/${connectionUrl}`, {
                    method: 'POST',
                }).then(response => {
                    if (!response.ok) {
                        console.error('Failed to start flow');
                    }
                    console.log('ConnectionId sent to server')
                });
            }
        )

        .catch(err => console.log('Error while starting connection: ' + err));

    intervalId = setInterval(countdownFunction, 1000);

});