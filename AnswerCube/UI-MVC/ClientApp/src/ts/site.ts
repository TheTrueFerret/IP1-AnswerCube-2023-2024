import '@popperjs/core';
import 'bootstrap';
import 'bootstrap-icons/font/bootstrap-icons.css';
import 'bootstrap/dist/css/bootstrap.css';

// Custom JS imports
// ... none at the moment

// Custom CSS imports
import '../scss/site.scss';

console.log('The \'site\' bundle has been loaded!');

document.addEventListener('DOMContentLoaded', () => {
    const themeSelector = document.getElementById('themeSelector') as HTMLSelectElement | null;
    const pageContainer = document.querySelector('.page-container') as HTMLElement | null;
    const slideContainers = document.querySelectorAll('.slide-container') as NodeListOf<HTMLElement>;

    if (!themeSelector || !pageContainer) {
        console.error('Theme selector or page container not found');
        return;
    }

    const applyTheme = () => {
        const selectedTheme = themeSelector.value;
        const isLightTheme = selectedTheme === 'light';

        pageContainer.classList.toggle('light-theme', isLightTheme);
        pageContainer.classList.toggle('dark-theme', !isLightTheme);

        slideContainers.forEach(slideContainer => {
            slideContainer.classList.toggle('light-theme', isLightTheme);
            slideContainer.classList.toggle('dark-theme', !isLightTheme);
        });
    };

    themeSelector.addEventListener('change', applyTheme);

    applyTheme();
});


/*import '@popperjs/core';
import 'bootstrap';
import 'bootstrap-icons/font/bootstrap-icons.css';
import 'bootstrap/dist/css/bootstrap.css';

// Custom JS imports
// ... none at the moment

// Custom CSS imports
import '../scss/site.scss';

console.log('The \'site\' bundle has been loaded!');

document.addEventListener('DOMContentLoaded', async () => {
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
});
*/