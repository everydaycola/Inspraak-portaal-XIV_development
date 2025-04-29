import {addSubRegion} from "./subRegion";
import {addCriteria, validateCriteriaPercentages} from "./criteria";

const subRegionBtn = document.getElementById("sub-region-btn") as HTMLAnchorElement;
subRegionBtn.addEventListener("click", addSubRegion);

const addCriteriaBtn = document.getElementById("criteria-btn") as HTMLAnchorElement;
addCriteriaBtn.addEventListener("click",addCriteria);

const panelForm = document.getElementById("new-panel-form") as HTMLFormElement;
panelForm.addEventListener("submit", (e) => {
    if (!validateCriteriaPercentages() || panelForm.checkValidity()) {
        e.preventDefault(); // Stop submission
    }
});