export function configureDelete() {
    document.querySelectorAll(".delete-button").forEach(button => {
        button.addEventListener("click", async (event) => {
            const row = (event.currentTarget as HTMLElement).closest("tr");
            if (!row) return;

            const organisationId = row.dataset.id; // Directly get string ID
            try {
                const response = await fetch(`/OrganisationManagement/AdminOrganisationDelete/${organisationId}`, {
                    method: "DELETE",
                });

                if (response.ok) {
                    window.location.href = response.url;
                } else if (!response.ok) {
                    console.error("Deletion failed:", await response.text());
                }
            } catch (error) {
                console.error("Network error:", error);
            }
        });
    });
}