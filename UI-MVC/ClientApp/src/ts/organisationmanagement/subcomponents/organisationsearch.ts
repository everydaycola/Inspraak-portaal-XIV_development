export function configureSearch() {
    const searchButton: HTMLButtonElement | null = document.querySelector("#searchButton");
    const queryInputField: HTMLInputElement | null = document.querySelector("#searchBar");
    const tableRows: NodeListOf<HTMLTableRowElement> = document.querySelectorAll('table tbody tr');


    if (searchButton && queryInputField) {
        searchButton.addEventListener("click", function () {
            console.log("Searchbutton clicked.")
            const searchQuery = queryInputField.value.toLowerCase();

            tableRows.forEach(row => {
                const organisationName = row.cells[1]?.textContent?.toLowerCase();
                if (organisationName && organisationName.includes(searchQuery)) {
                    row.style.display = '';
                } else {
                    row.style.display = 'none';
                }
            });
        })
    }

}