import {newSubregionInputAddedHandler} from "./apiAutofill/apiDataFiller";

import {setValidationMessages} from "./FormValidator"
import {onSubregionChange} from "./panelsizePreviewHandler";

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
    nameInput.id = `subregion-${subRegionCount}-name` 
    nameInput.name = `SubRegions[${subRegionCount}].Name`;  // Bind to SubRegions[index].Name
    nameInput.placeholder = "Naam";
    nameInput.type = "text";
    nameInput.className = "form-control d-inline w-50 me-2";
    

    // Create the Size input
    const sizeInput = document.createElement("input");
    sizeInput.id = `subregion-${subRegionCount}-size`
    sizeInput.name = `SubRegions[${subRegionCount}].Size`;  // Bind to SubRegions[index].Size
    sizeInput.placeholder = "Grootte";
    sizeInput.type = "number";
    sizeInput.className = "form-control d-inline w-25 me-2 subregion-size-input";

    sizeInput.addEventListener("input",() => {
        onSubregionChange()
    })
    
    // Create the Remove button
    const removeButton = document.createElement("button");
    removeButton.type = "button";
    removeButton.className = "btn btn-danger btn-sm";
    removeButton.innerHTML = `<i class="bi-trash"></i>`;
    removeButton.addEventListener("click", () => removeSubRegion(subRegionId));

    const nameError = document.createElement("span");
    nameError.id = `${nameInput.id}-msg`
    nameError.className = "text-danger field-validation-valid";
    
    const sizeError = document.createElement("span");
    sizeError.id = `${sizeInput.id}-msg`
    sizeError.className = "text-danger field-validation-valid";
    
    // Append the inputs and button to the wrapper
    wrapper.append(nameInput, sizeInput, removeButton, nameError, sizeError);

    // Add the wrapper to the subregion container
    subRegionContainer.appendChild(wrapper);
    //Call apiDataFillerScript to repopulate.
    newSubregionInputAddedHandler()
    onSubregionChange()

    addSubregionValidation(nameInput, nameError, sizeInput, sizeError);

    // Increment the count for the next subregion
    subRegionCount++;
}

export function addSubregionValidation(nameInput: HTMLInputElement, nameError: HTMLSpanElement, sizeInput: HTMLInputElement, sizeError: HTMLSpanElement) {
    //Name
    if (!nameInput.required){
        nameInput.required = true;
    }
    setValidationMessages(nameInput.id, nameError.id, {
        valueMissing: "De naam van een (deel)gemeente of wijk is verplicht."
    })
    //Size
    if (!sizeInput.required) {
        sizeInput.required = true;
    }
    setValidationMessages(sizeInput.id, sizeError.id, {
        valueMissing: "De grootte van een (deel)gemeente of wijk is verplicht."
    })
}

function removeSubRegion(id: string) {
    const element = document.getElementById(id);
    if (element) {
        // Remove the element from the DOM
        element.remove();
        // Rebuild the subregions to fix the indices
        reIndexSubRegions();
        //Call apiDataFillerScript
        newSubregionInputAddedHandler();
    }
}

function reIndexSubRegions() {
    const subRegionContainer = document.getElementById("subregions-container") as HTMLDivElement;
    const subRegionDivs = subRegionContainer.querySelectorAll(".subRegion") as NodeListOf<HTMLDivElement>;

    // Re-index the remaining subregions
    subRegionCount = 0;
    subRegionDivs.forEach((div, index) => {
        const nameInput = div.querySelector("input[name$='Name']") as HTMLInputElement;

        if (nameInput) {
            // Re-index Name input
            nameInput.name = `SubRegions[${index}].Name`;

            // Re-index Size input
            const sizeInput = div.querySelector("input[name$='Size']")  as HTMLInputElement;
            sizeInput.name = `SubRegions[${index}].Size`;
        }

        // Update subregionCount to the correct next index
        subRegionCount++;
    });
}