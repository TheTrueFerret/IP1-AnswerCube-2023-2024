document.addEventListener('DOMContentLoaded', (event) => {
    const searchQueryElement = document.getElementById('searchQuery') as HTMLInputElement;
    searchQueryElement.addEventListener('input', function (e) {

        let searchQuery = searchQueryElement.value.toLowerCase();
        const userRows = document.getElementsByClassName('user-row');

        for (let i = 0; i < userRows.length; i++) {
            const emailElement = (userRows[i] as HTMLElement).querySelector('td[data-email]') as HTMLElement;
            const email = emailElement?.getAttribute('data-email')?.toLowerCase() || '';
            if (email.includes(searchQuery)) {
                (userRows[i] as HTMLElement).style.display = '';
            } else {
                (userRows[i] as HTMLElement).style.display = 'none';
            }
        }
    });
});