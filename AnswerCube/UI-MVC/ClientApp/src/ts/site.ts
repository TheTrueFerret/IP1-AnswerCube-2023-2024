import '@popperjs/core';
import 'bootstrap';
import 'bootstrap-icons/font/bootstrap-icons.css';
import 'bootstrap/dist/css/bootstrap.css';
import '../scss/site.scss';

import { RemoveLastDirectoryPartOf } from "./urlDecoder";

// Custom JS imports
// ... none at the moment

let url = window.location.toString();
let scssFile: string = '';

console.log('The \'site\' bundle has been loaded!');

async function getTheme() {
    try {
        const response = await fetch(RemoveLastDirectoryPartOf(url) + "/api/theme", {
            method: "GET",
            headers: {
                'Content-Type': 'application/json',
            },
        });
        if (response.ok) {
            const data = await response.json();
            const theme = data.toString().toLowerCase();
            if (theme === "lighttheme") {
                scssFile = '../scss/lighttheme.scss';
            } else if (theme === "darktheme") {
                scssFile = '../scss/darktheme.scss';
            } else {
                console.error('Unknown theme received:', theme);
            }
            loadTheme(scssFile);
        } else {
            //exception? 
        }
    } catch (err) {
        console.log("Something went wrong: " + err);
    }
}

async function loadTheme(scssFile: string): Promise<void> {
    if (scssFile !== '') {
        try {
            await import(scssFile);
            console.log(`${scssFile} has been loaded`);
        } catch (err) {
            console.error(`Failed to load ${scssFile}:`, err);
        }
    }
}

getTheme();
