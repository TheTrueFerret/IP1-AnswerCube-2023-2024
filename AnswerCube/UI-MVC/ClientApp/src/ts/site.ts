import '@popperjs/core';
import 'bootstrap';
import 'bootstrap-icons/font/bootstrap-icons.css';
import 'bootstrap/dist/css/bootstrap.css';
import '../scss/site.scss';

import {getControllerNameFromUrl, getDomainFromUrl} from "./urlDecoder";

// Custom JS imports
// ... none at the moment

let url = window.location.toString();
let scssFile: string = '';

console.log('The \'site\' bundle has been loaded!');
console.log(getControllerNameFromUrl(url))

async function getTheme() {
    try {
        const domain = getDomainFromUrl(url);
        const controllerName = getControllerNameFromUrl(url);
        const response = await fetch(`${domain}/api/theme/GetTheme/${controllerName}`, {
            method: "GET",
            headers: {
                'Content-Type': 'application/json',
            },
        });
        if (response.ok) {
            const data = await response.json();
            const theme = data.toString().toLowerCase();
            if (theme === "lighttheme") {
                scssFile = 'lighttheme.scss';
            } else if (theme === "darktheme") {
                scssFile = 'darktheme.scss';
            } else {
                console.error('Unknown theme received:', theme);
            }
            loadTheme(scssFile);
        } else {
            console.error('Failed to fetch theme, code NOT OK'); 
        }
    } catch (err) {
        console.log("Something went wrong: " + err);
    }
}

async function loadTheme(scssFile: string): Promise<void> {
    if (scssFile !== '') {
        try {
            await import(`../scss/${scssFile}`);
            console.log(`${scssFile} has been loaded`);
        } catch (err) {
            console.error(`Failed to load ${scssFile}:`, err);
        }
    }
}

getTheme();
