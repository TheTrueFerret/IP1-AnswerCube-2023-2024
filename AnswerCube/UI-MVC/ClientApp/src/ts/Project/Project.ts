window.document.addEventListener('DOMContentLoaded', function () {
    // Get all the project divs
    const projectDivs = document.querySelectorAll('.project-div');
    console.log(projectDivs.length);
    // Add click event listener to each project div
    // @ts-ignore
    projectDivs.forEach((div: HTMLDivElement) => {
        div.addEventListener('click', function (this: HTMLDivElement, event: Event) {
            // Get the project id from the edit button's onclick attribute
            const projectId = this.dataset.projectId;
            const organizationId = this.dataset.organizationId;

            // Prevent the event from propagating if a button was clicked
            if ((event.target as Element).tagName === 'BUTTON' || (event.target as Element).tagName === 'I') {
                event.stopPropagation();
            } else {
                // Redirect to the project's view
                window.location.href = `/Project/Project?projectid=${projectId}&organizationid=${organizationId}`;
            }
        });
    });

    // Get the search input field
    const searchInput = document.getElementById('searchInput') as HTMLInputElement;
    const infoDiv = document.getElementById('infoDiv') as HTMLDivElement;
    infoDiv.textContent = 'No projects found';
    infoDiv.style.display = 'none';
    // Add an input event listener to the search input field
    searchInput.addEventListener('input', function () {
        // Get the current input value
        const inputValue = this.value.toLowerCase();

        // Filter the project divs
        // @ts-ignore
        projectDivs.forEach((div: HTMLDivElement) => {
            // Get the project name from the div's data attribute
            const projectName = div.dataset.projectName?.toLowerCase();

            // If the project name includes the input value, show the div, otherwise hide it
            if (inputValue === '' || projectName?.includes(inputValue)) {
                div.style.display = 'block';
                infoDiv.style.display = 'none';
            } else {
                infoDiv.style.display = 'block';
                div.style.display = 'none';
            }
        });
    });
});