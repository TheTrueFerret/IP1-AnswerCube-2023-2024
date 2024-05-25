import '@popperjs/core';
import 'bootstrap';
import 'bootstrap-icons/font/bootstrap-icons.css';
import 'bootstrap/dist/css/bootstrap.css';

import {RemoveLastDirectoryPartOf} from "./urlDecoder";

// Custom JS imports
// ... none at the moment

var url = window.location.toString();
var scssFile: string = '';

console.log('The \'site\' bundle has been loaded!');

/*document.addEventListener('DOMContentLoaded', async () => {
    const projectIdElement = document.querySelector<HTMLInputElement>('input[name="selectedTheme"]');
    const projectId = projectIdElement?.getAttribute('data-project-id');

    if (!projectId) {
        console.error('Project ID not found');
        return;
    }
    
    try {
        const response = await fetch(`/api/theme/${projectId}`);
        if (!response.ok) {
            throw new Error('Failed to fetch theme');
        }
        const theme = await response.text();
        document.body.classList.add(theme);

        const pageContainer = document.querySelector('.page-container');
        if (pageContainer) {
            pageContainer.classList.add(theme);
        }
    } catch (error) {
        console.error('Error fetching or applying theme:', error);
    }

    // Event listener to update hidden input when theme is selected
    const themeSelector = document.querySelector<HTMLSelectElement>('#themeSelector');
    themeSelector?.addEventListener('change', (event) => {
        const selectedTheme = (event.target as HTMLSelectElement).value;
        if (projectIdElement) {
            projectIdElement.value = selectedTheme;
        }
    });
});*/


function getTheme() {
    fetch(RemoveLastDirectoryPartOf(url) + "/GetTheme", {
        method: "GET",
        headers: {
            'Content-Type': 'application/json',
        },
    }).then((response: Response) => {
        if (response.status === 200) {
            return response.text();
        } else {
            
        }
    }).then((data: any) => {
        // hier de sccsFile instellen
        // maybe een switch case idk
        scssFile = data.Theme.toString()
    }).catch(err => {
        console.log("Something went wrong: " + err);
    })
}

if (scssFile != '') {
    // Custom CSS imports
    // import scssFile;
}

