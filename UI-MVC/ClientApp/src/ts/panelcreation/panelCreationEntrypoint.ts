import {addSubRegion, addSubregionValidation} from "./subcomponents/subRegion";
import {addCriteria} from "./subcomponents/criteria";
import {validateCriteriaPercentages} from "./subcomponents/panelFormValidator";
import {onSubregionChange, setupPanelsizePreviewHandlers} from "./subcomponents/panelsizePreviewHandler";
import {setupApiAutoFill} from "./apiAutofill/apiDataFiller";


document.addEventListener("DOMContentLoaded",() => {
    addSubregionValidation(
        document.getElementById("subregion-0-name") as HTMLInputElement,
        document.getElementById("subregion-0-name-msg") as HTMLSpanElement,
        document.getElementById("subregion-0-size") as HTMLInputElement,
        document.getElementById("subregion-0-size-msg") as HTMLSpanElement)
})

const subRegionBtn = document.getElementById("sub-region-btn") as HTMLAnchorElement;
subRegionBtn.addEventListener("click", addSubRegion);

const addCriteriaBtn = document.getElementById("criteria-btn") as HTMLAnchorElement;
addCriteriaBtn.addEventListener("click",addCriteria);

const panelForm = document.getElementById("new-panel-form") as HTMLFormElement;
panelForm.addEventListener("submit", (e) => {
    if (!validateCriteriaPercentages() || !panelForm.checkValidity()) {
        e.preventDefault(); // Stop submission
    }
});

setupPanelsizePreviewHandlers();
const initialSizeInput = document.querySelector("#subregion-0-size") as HTMLInputElement
initialSizeInput.addEventListener("input", (e) => {
    onSubregionChange()
})

setupApiAutoFill();