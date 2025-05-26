import {newSubregionInputAddedHandler} from "../apiAutofill/apiDataFiller";
import {onSubregionChange} from "./panelsizePreviewHandler";
import {createElementWithClassNames} from "../../customhelpers/htmlHelper";
import {setValidationMessages} from "./panelFormValidator";


let subRegionCount = 1;

export function addSubRegion() {
    const subRegionContainer = document.getElementById("subregions-container") as HTMLDivElement;
    const subRegionElement = createSubRegionElement(subRegionCount, removeSubRegion);
    const nameInput = subRegionElement.querySelectorAll("input")[0];
    const nameError = subRegionElement.querySelectorAll("span")[0];
    const sizeInput = subRegionElement.querySelectorAll("input")[1];
    const sizeError = subRegionElement.querySelectorAll("span")[0];
    subRegionContainer.appendChild(subRegionElement);
    addSubregionValidation(nameInput, nameError, sizeInput, sizeError);
    newSubregionInputAddedHandler();
    onSubregionChange();
    subRegionCount++;
}
function createSubRegionElement(index: number, removeCallback: (id: string) => void): HTMLDivElement {
    const subRegionId = `subregion-${index}`;

    // Create the wrapper div
    const wrapper = createElementWithClassNames("div", "mb-2","row","subRegion")
    wrapper.id = subRegionId;

    // Create the Name input
    const nameInput = createElementWithClassNames("input","form-control", "col","d-inline", "w-50","me-2");
    nameInput.id = `subregion-${index}-name`;
    nameInput.name = `SubRegions[${index}].Name`;
    nameInput.placeholder = "Naam";
    nameInput.type = "text";

    // Create the Size input
    const sizeInput = createElementWithClassNames("input","form-control","d-inline","col","w-25","me-2","subregion-size-input");
    sizeInput.id = `subregion-${index}-size`;
    sizeInput.name = `SubRegions[${index}].Size`;
    sizeInput.placeholder = "Grootte";
    sizeInput.type = "number";

    sizeInput.addEventListener("input", () => {
        onSubregionChange();
    });

    // Create the Remove button
    const removeButton = createElementWithClassNames("button","btn","btn-danger","btn-sm");
    removeButton.type = "button";
    removeButton.innerHTML = `<i class="bi-trash"></i>`;
    removeButton.addEventListener("click", () => removeCallback(subRegionId));

    // Error messages
    const nameError = createElementWithClassNames("span","text-danger","col","field-validation-valid");
    nameError.id = `${nameInput.id}-msg`;
    nameError.className = "text-danger field-validation-valid";
    
    const sizeError = createElementWithClassNames("span","text-danger","col","field-validation-valid");
    sizeError.id = `${sizeInput.id}-msg`;

    // Append the inputs and button to the wrapper
    wrapper.append(nameInput, sizeInput, removeButton, nameError, sizeError);
    
    return wrapper;
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