import {addSubRegion} from "./subRegion";
import {addCriteria} from "./criteria";

const subRegionBtn = document.getElementById("sub-region-btn") as HTMLAnchorElement;
subRegionBtn.addEventListener("click", addSubRegion);

const addCriteriaBtn = document.getElementById("criteria-btn") as HTMLAnchorElement;
addCriteriaBtn.addEventListener("click",addCriteria);

const panelForm = document.getElementById("new-panel-form") as HTMLFormElement;
panelForm.addEventListener("submit", (e) => {
    if (!validateCriteriaPercentages()) {
        e.preventDefault(); // Stop submission
    }
});

function validateCriteriaPercentages(): boolean {
    let isValid = true;

    const criteriaBlocks = document.querySelectorAll(".criteria");

    criteriaBlocks.forEach((criteriaDiv, index) => {
        const inputs = criteriaDiv.querySelectorAll<HTMLInputElement>(
            `input[name^="Distributions[${index}].AnswerOptions"][name$="DistributionPercentage"]`
        );

        const sum = Array.from(inputs)
            .map(input => parseFloat(input.value) || 0)
            .reduce((acc, val) => acc + val, 0);

        // Remove existing message if any
        let message = criteriaDiv.querySelector(".distribution-error");
        if (message) message.remove();

        if (Math.abs(sum) != 100) {
            isValid = false;

            const error = document.createElement("div");
            error.className = "text-danger distribution-error";
            error.innerText = `De verdeling van de antwoord opties moet 100% zijn. Nu: ${sum}%`;

            // Place below answer list
            const ul = criteriaDiv.querySelector("ul.list-group");
            ul?.after(error);
        }
    });

    return isValid;
}