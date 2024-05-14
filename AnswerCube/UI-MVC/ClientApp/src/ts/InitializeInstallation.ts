import { Modal } from 'bootstrap';

document.addEventListener("DOMContentLoaded", function () {
    const createInstallationButton = document.getElementById("createInstallationButton");
    const saveInstallationButton = document.getElementById("saveInstallationButton");
    const closeInstallationModalButton = document.getElementById("closeInstallationModal");
    const createInstallationModalElement = document.getElementById("createInstallationModal");

    if (createInstallationButton && saveInstallationButton && createInstallationModalElement && closeInstallationModalButton) {
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
    }
});
