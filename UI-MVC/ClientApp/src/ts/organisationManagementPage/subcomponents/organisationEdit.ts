export function configureEdit() {
    const table = document.querySelector("table");
    if (!table) return;
    table.addEventListener("click", async (event) => {
        const target = event.target as HTMLElement;

        if (target.classList.contains("edit-button")) {
            const row = target.closest("tr");
            if (!row) return;

            const nameCell = row.querySelector('[data-name]');
            const id = row.getAttribute("data-id");

            if (!nameCell || !id) return;

            const isEditing = target.textContent === "Save";

            if (!isEditing) {
                const currentName = nameCell.textContent?.trim() || "";
                nameCell.innerHTML = `<input type="text" class="form-control" value="${currentName}" />`;
                target.textContent = "Save";
                target.classList.remove("btn-primary");
                target.classList.add("btn-success");
            } else {
                //INPUT FIELD MET GEUPDATE WAARDE
                const input = nameCell.querySelector("input") as HTMLInputElement;
                const newName = input.value;
                
                try {
                    const response = await fetch("/OrganisationManagement/AdminOrganisationUpdate", {
                        method: "POST",
                        headers: {
                            "Content-Type": "application/x-www-form-urlencoded",
                        },
                        body: new URLSearchParams({
                            organisationId: id,
                            name: newName
                        }),
                    });
                    if (!response.ok) {
                        console.error("Er liep iets fout bij het updated van een organisatie...")
                    }
                    const result = await response.json();
                    nameCell.textContent = result.name;
                    target.textContent = "Edit";
                    target.classList.remove("btn-success");
                    target.classList.add("btn-primary");
                } catch (error) {
                    console.error("Fout bij het opslaan van organisatie:", error);
                    alert("Opslaan mislukt.");
                }
            }
        }
    });
}
