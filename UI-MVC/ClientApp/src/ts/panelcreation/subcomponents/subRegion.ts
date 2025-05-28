import {addSubregionValidation} from "./panelFormValidator";
import {onSubregionChange} from "./panelsizePreviewHandler";
import {newSubregionInputAddedHandler} from "../apiAutofill/apiDataFiller";
import {createElementWithClassNames} from "../../customHelpers/htmlHelper";


let subRegionCount = 1;

export function initAddSubRegionHandler(){
    const subRegionBtn = document.getElementById("sub-region-btn") as HTMLAnchorElement;
    subRegionBtn.addEventListener("click", addSubRegion);
}

export function addEventHandlerToDefaultSubregionInput(){
    const initialSizeInput = document.querySelector("#subregion-0-size") as HTMLInputElement
    initialSizeInput.addEventListener("input", (e) => {
        onSubregionChange()
    })
}

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

    // Main container
    const wrapper = createElementWithClassNames("div", "mb-2", "subRegion", `subregion-${index}`);
    wrapper.id = subRegionId;

    // Error row
    const errorRow = createElementWithClassNames("div", "row", "mb-1");
    const nameErrorCol = createElementWithClassNames("div", "col-6");
    const sizeErrorCol = createElementWithClassNames("div", "col-6");

    const nameError = createElementWithClassNames("span", "text-danger", "field-validation-valid");
    nameError.id = `subregion-${index}-name-msg`;
    const sizeError = createElementWithClassNames("span", "text-danger", "field-validation-valid");
    sizeError.id = `subregion-${index}-size-msg`;

    nameErrorCol.appendChild(nameError);
    sizeErrorCol.appendChild(sizeError);
    errorRow.append(nameErrorCol, sizeErrorCol);

    // Input row
    const inputRow = createElementWithClassNames("div", "row", "g-2", "align-items-center");

    // Name input column
    const nameCol = createElementWithClassNames("div", "col-12", "col-md-6", "mb-2", "mb-md-0");
    const nameInput = createElementWithClassNames("input", "form-control") as HTMLInputElement;
    nameInput.id = `subregion-${index}-name`;
    nameInput.name = `SubRegions[${index}].Name`;
    nameInput.placeholder = "Naam";
    nameInput.type = "text";
    nameInput.required = true;
    nameCol.appendChild(nameInput);

    // Size input column
    const sizeCol = createElementWithClassNames("div", "col-12", "col-md-4");
    const sizeInput = createElementWithClassNames("input", "form-control", "subregion-size-input") as HTMLInputElement;
    sizeInput.id = `subregion-${index}-size`;
    sizeInput.name = `SubRegions[${index}].Size`;
    sizeInput.placeholder = "Grootte";
    sizeInput.type = "number";
    sizeInput.required = true;
    sizeCol.appendChild(sizeInput);

    // Remove button column
    const buttonCol = createElementWithClassNames("div", "col-auto", "ms-md-2");
    const removeButton = createElementWithClassNames("button", "btn", "btn-danger", "btn-sm");
    removeButton.type = "button";
    removeButton.innerHTML = `<i class="bi-trash"></i>`;
    removeButton.addEventListener("click", () => removeCallback(subRegionId));
    buttonCol.appendChild(removeButton);

    inputRow.append(nameCol, sizeCol, buttonCol);
    wrapper.append(errorRow, inputRow);

    sizeInput.addEventListener("input", () => onSubregionChange());

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