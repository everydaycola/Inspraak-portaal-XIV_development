import {addEventHandlerToDefaultSubregionInput, addSubRegion, initAddSubRegionHandler} from "./subcomponents/subRegion";
import {addCriteria, initAddCriteriaHandler} from "./subcomponents/criteria";
import {
    addSubregionValidation,
    handlePanelFormSubmission,
    validateCriteriaPercentages
} from "./subcomponents/panelFormValidator";
import {onSubregionChange, setupPanelsizePreviewHandlers} from "./subcomponents/panelsizePreviewHandler";
import {setupApiAutoFill} from "./apiAutofill/apiDataFiller";

document.addEventListener("DOMContentLoaded",() => {
    addSubregionValidation(
        document.getElementById("subregion-0-name") as HTMLInputElement,
        document.getElementById("subregion-0-name-msg") as HTMLSpanElement,
        document.getElementById("subregion-0-size") as HTMLInputElement,
        document.getElementById("subregion-0-size-msg") as HTMLSpanElement)
initAddSubRegionHandler();
initAddCriteriaHandler();
handlePanelFormSubmission();
setupPanelsizePreviewHandlers();
addEventHandlerToDefaultSubregionInput();
setupApiAutoFill();
})