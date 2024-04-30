window.document.addEventListener('DOMContentLoaded', function () {
    // Get all the project divs
    const projectDivs = document.querySelectorAll('.project-div');

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
});