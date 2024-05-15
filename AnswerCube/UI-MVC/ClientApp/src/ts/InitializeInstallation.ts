import {Modal} from 'bootstrap';
import {RemoveLastDirectoryPartOf} from "./urlDecoder";
import {getCookie, setCookie} from "./CookieHandler";

let url = window.location.toString()
const jwtToken = getCookie("jwtToken");
let selectElement: HTMLSelectElement | null = document.getElementById("inactiveInstallations") as HTMLSelectElement

const createInstallationButton = document.getElementById("createInstallationButton");
const saveInstallationButton = document.getElementById("saveInstallationButton");
const closeInstallationModalButton = document.getElementById("closeInstallationModal");
const createInstallationModalElement = document.getElementById("createInstallationModal");
const submitInstallationButton = document.getElementById("confirmButton");


if (createInstallationButton && saveInstallationButton && createInstallationModalElement &&
    closeInstallationModalButton && submitInstallationButton) {
    const createInstallationModal = new Modal(createInstallationModalElement);
    console.log("Buttons and modal found"); // Check if this message appears in the console

    // Show the modal when the "Create New Installation" button is clicked
    createInstallationButton.addEventListener("click", function () {
        createInstallationModal.show();
    });
    // Handle form submission within the modal
    saveInstallationButton.addEventListener("click", function () {
        // Here you can add your logic to handle the form submission
        // For example, you can retrieve form data and perform AJAX request to save the installation
        // After saving the installation, you can close the modal
        createInstallationModal.hide();
    });

    // Handle form submission within the modal
    closeInstallationModalButton.addEventListener("click", function () {
        createInstallationModal.hide();
    });

    // When pressing the submit button
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
    if (selectElement) {
        const selectedOption = selectElement.options[selectElement.selectedIndex];
        const selectedInstallationValue = selectedOption.value;
        console.log('Selected value:', selectedInstallationValue);
        return selectedInstallationValue;
    }
}

