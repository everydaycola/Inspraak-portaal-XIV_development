import {newSubregionInputAddedHandler} from "../apiAutofill/apiDataFiller";
import {onSubregionChange} from "./panelsizePreviewHandler";
import {createElementWithClassNames} from "../../customhelpers/htmlHelper";
import {addSubregionValidation} from "./panelFormValidator";


let subRegionCount = 1;

export function addSubRegion() {
    const subRegionContainer = document.getElementById("subregions-container") as HTMLDivElement;
    const subRegionElement = createSubRegionElement(subRegionCount, removeSubRegion);
    const nameInput = subRegionElement.querySelectorAll("input")[0];
    const nameError = subRegionElement.querySelectorAll("span")[0];
    const sizeInput = subRegionElement.querySelectorAll("input")[1];
    const sizeError = subRegionElement.querySelectorAll("span")[1];
    subRegionContainer.appendChild(subRegionElement);
    addSubregionValidation(nameInput, nameError, sizeInput, sizeError);
    newSubregionInputAddedHandler();
    onSubregionChange();
    subRegionCount++;
}
function createSubRegionElement(index: number, removeCallback: (id: string) => void): HTMLDivElement {
    const subRegionId = `subregion-${index}`;
    
    const wrapper = createElementWithClassNames("div", "mb-2", "subRegion", `subregion-${index}`, "d-flex", "align-items-start");
    wrapper.id = subRegionId;
    
    const innerWrapper = createElementWithClassNames("div", "w-100");
    
    const nameError = createElementWithClassNames("span", "text-danger", "field-validation-valid", "me-2", "flex-grow-1");
    nameError.id = `subregion-${index}-name-msg`;
    const sizeError = createElementWithClassNames("span", "text-danger", "field-validation-valid", "flex-grow-1");
    sizeError.id = `subregion-${index}-size-msg`;
    
    const inputRow = createElementWithClassNames("div", "d-flex", "align-items-center");
    
    const nameInput = createElementWithClassNames("input", "form-control", "d-inline", "w-50", "me-2") as HTMLInputElement;
    nameInput.id = `subregion-${index}-name`;
    nameInput.name = `SubRegions[${index}].Name`;
    nameInput.placeholder = "Naam";
    nameInput.type = "text";
    nameInput.required = true;
    
    const sizeInput = createElementWithClassNames("input", "form-control", "d-inline", "w-25", "subregion-size-input") as HTMLInputElement;
    sizeInput.id = `subregion-${index}-size`;
    sizeInput.name = `SubRegions[${index}].Size`;
    sizeInput.placeholder = "Grootte";
    sizeInput.type = "number";
    sizeInput.required = true;

    sizeInput.addEventListener("input", () => {
        onSubregionChange();
    });
    
    const removeButton = createElementWithClassNames("button", "btn", "btn-danger", "btn-sm", "ms-2");
    removeButton.type = "button";
    removeButton.innerHTML = `<i class="bi-trash"></i>`;
    removeButton.addEventListener("click", () => removeCallback(subRegionId));
    
    inputRow.append(nameInput, sizeInput, removeButton);
    
    innerWrapper.append(nameError, sizeError, inputRow);
    
    wrapper.append(innerWrapper);

    return wrapper;
}


function removeSubRegion(id: string) {
    const element = document.getElementById(id);
    if (element) {
        element.remove();
        reIndexSubRegions();
        newSubregionInputAddedHandler();
    }
}

function reIndexSubRegions() {
    const subRegionContainer = document.getElementById("subregions-container") as HTMLDivElement;
    const subRegionDivs = subRegionContainer.querySelectorAll(".subRegion") as NodeListOf<HTMLDivElement>;
    
    subRegionCount = 0;
    subRegionDivs.forEach((div, index) => {
        const nameInput = div.querySelector("input[name$='Name']") as HTMLInputElement;

        if (nameInput) {
            nameInput.name = `SubRegions[${index}].Name`;
            
            const sizeInput = div.querySelector("input[name$='Size']")  as HTMLInputElement;
            sizeInput.name = `SubRegions[${index}].Size`;
        }
        
        subRegionCount++;
    });
}