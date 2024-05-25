import '@popperjs/core';
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
