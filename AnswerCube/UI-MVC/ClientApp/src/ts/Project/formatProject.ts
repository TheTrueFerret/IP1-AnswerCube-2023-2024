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
