export function fetchUniqueCodes() {
    let modalContent = document.getElementById("modalContent");
    let panelId = document.getElementById("viewCodesButton")?.getAttribute("data-panel-id");
    if(modalContent && panelId){
    fetch(`/PanelManagement/LoadUniqueCodes?panelId=${panelId}`)
        .then(response => response.text())
        .then(html => {
            if (modalContent) {
                modalContent.innerHTML = html;
            }
        }).catch(error => {
            if (modalContent) {
                modalContent.innerHTML = `
                    <h2>Something went wrong :(</h2>
                `
            }
        })
    }
}

document.addEventListener("DOMContentLoaded", () => {
    const viewCodesButton = document.getElementById("viewCodesButton")
    if(viewCodesButton){
        viewCodesButton.addEventListener("click", fetchUniqueCodes)
    }
});