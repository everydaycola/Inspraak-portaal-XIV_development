import {getCurrentBaseUrl} from "../customhelpers/locationHelper";
import {fetchFromAPI} from "../customhelpers/apihelper";

export function initEditPlanningGroupHandler(){
    document.addEventListener("DOMContentLoaded", () => {
        console.log("Clicked the edit save button");
        const editSaveButtons = document.querySelectorAll(".inline-edit-save-btn");
        console.log("Clicked the edit save button");

        editSaveButtons.forEach(button => {
            button.addEventListener("click", async (event) => {
                const clickedButton = event.currentTarget as HTMLButtonElement;
                const memberId = clickedButton.dataset.memberId;
                const panelId = clickedButton.dataset.panelId;
                let currentMode = clickedButton.dataset.mode;

                if (!memberId || !panelId) {
                    console.error("Missing memberId or panelId on button.");
                    return;
                }

                const emailDisplay = document.getElementById(`email-display-${memberId}`) as HTMLSpanElement;
                const emailEdit = document.getElementById(`email-edit-${memberId}`) as HTMLInputElement;
                const usernameDisplay = document.getElementById(`username-display-${memberId}`) as HTMLSpanElement;
                const usernameEdit = document.getElementById(`username-edit-${memberId}`) as HTMLInputElement;
                const functieDisplay = document.getElementById(`functie-display-${memberId}`) as HTMLSpanElement;
                const functieEdit = document.getElementById(`functie-edit-${memberId}`) as HTMLInputElement;

                if (!emailDisplay || !emailEdit || !usernameDisplay || !usernameEdit || !functieDisplay || !functieEdit) {
                    console.error(`Could not find all required elements for member ID: ${memberId}.`);
                    return;
                }

                if (currentMode === "edit") {
                    emailDisplay.classList.add("d-none");
                    emailEdit.classList.remove("d-none");
                    usernameDisplay.classList.add("d-none");
                    usernameEdit.classList.remove("d-none");
                    functieDisplay.classList.add("d-none");
                    functieEdit.classList.remove("d-none");

                    emailEdit.value = emailDisplay.innerText;
                    usernameEdit.value = usernameDisplay.innerText;
                    functieEdit.value = functieDisplay.innerText;

                    clickedButton.innerText = "Opslaan";
                    clickedButton.classList.remove("btn-info");
                    clickedButton.classList.add("btn-success");
                    clickedButton.dataset.mode = "save";

                } else if (currentMode === "save") {
                    const baseUrl = getCurrentBaseUrl();

                    const updatedEmail = emailEdit.value.trim();
                    const updatedUserName = usernameEdit.value.trim();
                    const updatedFunctie = functieEdit.value.trim();
                    
                    const response: Response = await fetchFromAPI(
                        `${baseUrl}/api/PanelManagements/UpdatePlanningsGroupMember`, {
                            method: "POST",
                            headers: {
                                "Content-Type": "application/json"
                            },
                            body: JSON.stringify({
                                userId: memberId,
                                email: updatedEmail,
                                naam: updatedUserName,
                                functie: updatedFunctie
                            })
                        }
                    );
                    
                    if (!response.ok) {
                        alert("Er zijn velden niet correct ingevuld. Probeer het opnieuw.");
                        return;
                    }
                    
                    emailDisplay.innerText = updatedEmail;
                    usernameDisplay.innerText = updatedUserName;
                    functieDisplay.innerText = updatedFunctie;

                    emailDisplay.classList.remove("d-none");
                    emailEdit.classList.add("d-none");
                    usernameDisplay.classList.remove("d-none");
                    usernameEdit.classList.add("d-none");
                    functieDisplay.classList.remove("d-none");
                    functieEdit.classList.add("d-none");

                    clickedButton.innerText = "Bewerken";
                    clickedButton.classList.remove("btn-success");
                    clickedButton.classList.add("btn-info");
                    clickedButton.dataset.mode = "edit";
                }
            });
        });
    });
}