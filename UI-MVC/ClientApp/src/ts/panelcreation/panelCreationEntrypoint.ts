import {addSubregionValidation, handlePanelFormSubmission} from "./subcomponents/panelFormValidator";
import {addEventHandlerToDefaultSubregionInput, initAddSubRegionHandler} from "./subcomponents/subRegion";
import {initAddCriteriaHandler} from "./subcomponents/criteria";
import {setupPanelsizePreviewHandlers} from "./subcomponents/panelsizePreviewHandler";
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