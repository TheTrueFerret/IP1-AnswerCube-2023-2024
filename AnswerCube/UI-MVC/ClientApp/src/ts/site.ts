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
        const domain: string | null = getDomainFromUrl(url);
        const controllerName: string | null = getControllerNameFromUrl(url);
        if (domain != null && controllerName != null) {
            const response = await fetch(`${domain}/api/Theme/GetTheme/${controllerName}`, {
                method: "GET",
                headers: {
                    'Content-Type': 'application/json',
                },
            });
            if (response.ok) {
                const data = await response.text();
                const theme = data.toString().toLowerCase();
                if (theme === "light") {
                    scssFile = 'lighttheme.scss';
                } else if (theme === "dark") {
                    scssFile = 'darktheme.scss';
                } else if (theme === "darkgradation") {
                    scssFile = 'darkgradation.scss';
                } else if (theme === "blue") {
                    scssFile = 'blue.scss';
                } else if (theme === "instagram") {
                    scssFile = 'instagram.scss';
                } else {
                    console.error('Unknown theme received:', theme);
                }
                loadTheme(scssFile);
            } else {
                console.error('Failed to fetch theme, code NOT OK');
            }
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
