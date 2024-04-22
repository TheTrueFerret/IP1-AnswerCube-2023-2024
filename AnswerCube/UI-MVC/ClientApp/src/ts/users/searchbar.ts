document.addEventListener('DOMContentLoaded', (event) => {
    const searchQueryElement = document.getElementById('searchQuery') as HTMLInputElement;
    searchQueryElement.addEventListener('input', function (e) {
        console.log("Search query: ", searchQueryElement.value);
        let searchQuery = searchQueryElement.value.toLowerCase();
        const userRows = document.getElementsByClassName('user-row');

        for (let i = 0; i < userRows.length; i++) {
            console.log("er wordt gezocht in row: " + i)
            const emailElement = (userRows[i] as HTMLElement).querySelector('td[data-email]') as HTMLElement;
            const email = emailElement?.getAttribute('data-email')?.toLowerCase() || '';
            console.log("email: " + email);
            if (email.includes(searchQuery)) {
                console.log("Match found: ", email);
                (userRows[i] as HTMLElement).style.display = '';
            } else {
                console.log("No match found: ", email);
                (userRows[i] as HTMLElement).style.display = 'none';
            }
        }
        console.log("klaar met zoeken\n")
    });
});