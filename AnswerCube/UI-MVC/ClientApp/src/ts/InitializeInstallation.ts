import {Modal} from 'bootstrap';
import {RemoveLastDirectoryPartOf} from "./urlDecoder";
import {getCookie, setCookie} from "./CookieHandler";

let url = window.location.toString()
const jwtToken = getCookie("jwtToken");

let installationSelectElement: HTMLSelectElement | null = document.getElementById("inactiveInstallations") as HTMLSelectElement;

let installationName: HTMLInputElement | null = document.getElementById("installationName") as HTMLInputElement;
let installationLocation: HTMLInputElement | null = document.getElementById("installationLocation") as HTMLInputElement;

let organisationSelectElement: HTMLSelectElement | null = document.getElementById("organisations") as HTMLSelectElement;


const createInstallationButton: HTMLElement | null = document.getElementById("createInstallationButton");
const saveInstallationButton: HTMLElement | null = document.getElementById("saveInstallationButton");
const closeInstallationModalButton: HTMLElement | null = document.getElementById("closeInstallationModal");
const createInstallationModalElement: HTMLElement | null = document.getElementById("createInstallationModal");
const submitInstallationButton: HTMLElement | null = document.getElementById("confirmButton");


if (createInstallationButton && saveInstallationButton && createInstallationModalElement &&
    closeInstallationModalButton && submitInstallationButton) {
    const createInstallationModal = new Modal(createInstallationModalElement);
    console.log("Buttons and modal found"); // Check if this message appears in the console

    createInstallationButton.addEventListener("click", function () {
        createInstallationModal.show();
    });
    saveInstallationButton.addEventListener("click", function () {
        CreateNewInstallation();
        createInstallationModal.hide();
    });
    closeInstallationModalButton.addEventListener("click", function () {
        createInstallationModal.hide();
    });
    submitInstallationButton.addEventListener("click", function () {
        submitInstallation()
    });
}

function submitInstallation() {
    let installationId = getSelectedInstallationValue()
    let requestBody = {
        Id: installationId
    };
    console.log(requestBody);
    fetch(RemoveLastDirectoryPartOf(url) + "/SetInstallationToActive", {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
        },
        body: JSON.stringify(requestBody)
    }).then((response: Response) => {
        if (response.status === 200) {
            return response.json();
        } else {

        }
    }).then((data: any) => {
        if (data && data.token) {
            setCookie("jwtToken", data.token, 7);
        }
        if (data.url) {
            // Redirect to the URL of the next slide
            window.location.href = data.url;
        }
    }).catch(err => {
        console.log("Something went wrong: " + err);
    })
}


function getSelectedInstallationValue() {
    if (installationSelectElement) {
        const selectedOption = installationSelectElement.options[installationSelectElement.selectedIndex];
        const selectedInstallationValue = selectedOption.value;
        console.log('Selected value:', selectedInstallationValue);
        return selectedInstallationValue;
    }
}


function CreateNewInstallation() {
    const name = getNameNewInstallation();
    const location = getLocationNewInstallation();
    const id = getOrganizationId()
    
    let requestBody = {
        Name: name,
        Location: location,
        Id: id
    };
    console.log(requestBody);
    fetch(RemoveLastDirectoryPartOf(url) + "/CreateInstallation", {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
        },
        body: JSON.stringify(requestBody)
    }).then((response: Response) => {
        if (response.status === 200) {
            return response.json();
        }
    }).then((newInstallation: any) => {
        if (newInstallation.success && installationSelectElement) {
            const newOption = document.createElement('option');
            newOption.value = newInstallation.id;
            newOption.textContent = `${newInstallation.name} ${newInstallation.location}`;
            installationSelectElement.append(newOption);
        }
    }).catch(err => {
        console.log("Something went wrong: " + err);
    })
}

function getNameNewInstallation() {
    if (installationName) {
        return installationName.value
    } else {
        console.error("installationName input not found");
        return null
    }
}

function getLocationNewInstallation() {
    if (installationLocation) {
        return installationLocation.value
    } else {
        console.error("installationLocation input not found");
        return null
    }
}

function getOrganizationId() {
    if (organisationSelectElement) {
        const selectedOption = organisationSelectElement.options[organisationSelectElement.selectedIndex];
        const selectedInstallationValue = selectedOption.value;
        console.log('Selected value:', selectedInstallationValue);
        return selectedInstallationValue;
    }
}

