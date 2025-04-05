
const subRegionBtn = document.getElementById("sub-region-btn") as HTMLAnchorElement
subRegionBtn.addEventListener("click", addSubRegion)

let subRegionCount = 2;

function addSubRegion() {
    const subRegionContainer = document.getElementById("subregions-container") as HTMLDivElement;
    const subRegionId = `subregion-${subRegionCount}`;

    // Create the wrapper
    const wrapper = document.createElement("div");
    wrapper.id = subRegionId;
    wrapper.className = "mb-2 d-flex align-items-center";

    // Create inputs
    const nameInput = document.createElement("input");
    nameInput.name = `SubRegions[${subRegionCount}].Name`;
    nameInput.placeholder = "Naam";
    nameInput.type = "text";
    nameInput.className = "form-control d-inline w-50 me-2";

    const sizeInput = document.createElement("input");
    sizeInput.name = `SubRegions[${subRegionCount}].Size`;
    sizeInput.placeholder = "Grootte";
    sizeInput.type = "number";
    sizeInput.className = "form-control d-inline w-25 me-2";

    const removeButton = document.createElement("button");
    removeButton.type = "button";
    removeButton.className = "btn btn-danger btn-sm";
    removeButton.innerText = "✕";
    removeButton.addEventListener("click", () => wrapper.remove());

    // Add elements
    wrapper.append(nameInput, sizeInput, removeButton);
    subRegionContainer.appendChild(wrapper);

    subRegionCount++;
}