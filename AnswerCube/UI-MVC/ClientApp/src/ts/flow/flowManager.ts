import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

let connection: HubConnection = new HubConnectionBuilder().withUrl("/flowHub").build();

connection.start().then(() => {
    console.log("connected");
}).catch((err: Error) => console.error(err.toString()));

document.getElementById("startFlow")?.addEventListener("click", function (this: HTMLElement) {
    // Get the installationId from somewhere, e.g. a data attribute on the button
    let installationId: string | null = this.getAttribute("data-installation-id");

    if (installationId) {
        // Send a message to the server to start the flow
        connection.invoke("StartFlow", installationId).catch((err: Error) => console.error(err.toString()));
    }
});

document.getElementById("stopFlow")?.addEventListener("click", function (this: HTMLElement) {
    // Get the installationId from somewhere, e.g. a data attribute on the button
    let installationId: string | null = this.getAttribute("data-installation-id");

    if (installationId) {
        // Send a message to the server to stop the flow
        connection.invoke("StopFlow", installationId).catch((err: Error) => console.error(err.toString()));
    }
});