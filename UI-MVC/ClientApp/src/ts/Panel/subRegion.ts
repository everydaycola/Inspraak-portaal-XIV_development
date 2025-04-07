let subRegionCount = 1;

export function addSubRegion() {
    const subRegionContainer = document.getElementById("subregions-container") as HTMLDivElement;

    // Create a unique ID for this subregion block
    const subRegionId = `subregion-${subRegionCount}`;

    // Create the wrapper div
    const wrapper = document.createElement("div");
    wrapper.id = subRegionId;
    wrapper.className = "mb-2 d-flex align-items-center subRegion";

    // Create the Name input
    const nameInput = document.createElement("input");
    nameInput.name = `SubRegions[${subRegionCount}].Name`;  // Bind to SubRegions[index].Name
    nameInput.placeholder = "Naam";
    nameInput.type = "text";
    nameInput.className = "form-control d-inline w-50 me-2";

    // Create the Size input
    const sizeInput = document.createElement("input");
    sizeInput.name = `SubRegions[${subRegionCount}].Size`;  // Bind to SubRegions[index].Size
    sizeInput.placeholder = "Grootte";
    sizeInput.type = "number";
    sizeInput.className = "form-control d-inline w-25 me-2";

    // Create the Remove button
    const removeButton = document.createElement("button");
    removeButton.type = "button";
    removeButton.className = "btn btn-danger btn-sm";
    removeButton.innerHTML = `<i class="bi-trash"></i>`;
    removeButton.addEventListener("click", () => removeSubRegion(subRegionId));

    // Append the inputs and button to the wrapper
    wrapper.append(nameInput, sizeInput, removeButton);

    // Add the wrapper to the subregion container
    subRegionContainer.appendChild(wrapper);

    // Increment the count for the next subregion
    subRegionCount++;
}

function removeSubRegion(id: string) {
    const element = document.getElementById(id);
    if (element) {
        // Remove the element from the DOM
        element.remove();
        // Rebuild the subregions to fix the indices
        reIndexSubRegions();
    }
}

function reIndexSubRegions() {
    const subRegionContainer = document.getElementById("subregions-container") as HTMLDivElement;
    const subRegionDivs = subRegionContainer.querySelectorAll(".subRegion");

    // Re-index the remaining subregions
    subRegionCount = 1;
    subRegionDivs.forEach((div, index) => {
        const nameInput = div.querySelector("input[name$='Name']") as HTMLInputElement;

        if (nameInput) {
            // Re-index Name input
            nameInput.name = `SubRegions[${index+1}].Name`;

            // Re-index Size input
            const sizeInput = div.querySelector("input[name$='Size']")  as HTMLInputElement;
            sizeInput.name = `SubRegions[${index+1}].Size`;
        }

        // Update subregionCount to the correct next index
        subRegionCount++;
    });
}