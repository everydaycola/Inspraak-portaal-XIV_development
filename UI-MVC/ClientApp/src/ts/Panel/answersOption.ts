import {round} from "@popperjs/core/lib/utils/math";

let answerOptionCounters: Map<number, number> = new Map()


export function addAnswerOption(criteriaId: number, answersContainer: HTMLUListElement) {
    const answerCount = answerOptionCounters.get(criteriaId) ?? 0;
    answerOptionCounters.set(criteriaId, answerCount + 1);
    
    const answerOptionId = `option-${criteriaId}-${answerCount}`

    // Create answerOption li
    const answerOptionLi = document.createElement("li");
    answerOptionLi.id = answerOptionId
    answerOptionLi.className = "list-group-item d-flex flex-row py-2";

    // Create answerOption input
    const answerOptionInput = document.createElement("input");
    answerOptionInput.name = `Distributions[${criteriaId}].AnswerOptions[${answerCount}].Option`;
    answerOptionInput.className = "border border-1 rounded-2 me-2";
    answerOptionInput.type = "text";
    answerOptionInput.placeholder = `Antwoord ${(answerCount + 1)}`

    // Create answerOptionPercentage input
    const answerOptionPercentageInput = document.createElement("input");
    answerOptionPercentageInput.name = `Distributions[${criteriaId}].AnswerOptions[${answerCount}].DistributionPercentage`;
    answerOptionPercentageInput.className = "border border-1 rounded-2 me-2";
    answerOptionPercentageInput.type = "number";
    answerOptionPercentageInput.placeholder = `${round(100 / (answerCount + 1))}`

    // Create Delete Button
    const removeBtn = document.createElement("button");
    removeBtn.type = "button";
    removeBtn.className = "btn btn-danger btn-sm";
    removeBtn.innerHTML = `<i class="bi-trash"></i>`;
    removeBtn.addEventListener("click", () => removeAnswerOption(answerOptionId));

    // Appending
    answerOptionLi.append(answerOptionInput, answerOptionPercentageInput, removeBtn);
    answersContainer.append(answerOptionLi);

    // Counter ++
    answerOptionCounters.set(criteriaId, answerCount + 1);
}

function removeAnswerOption(id: string) {
    const element = document.getElementById(id);
    if (element) {
        // Remove the element from the DOM
        element.remove();
        // Rebuild the subregions to fix the indices
        //reIndexCriteria();
    }
}

function reIndexCriteria() {
    const subRegionContainer = document.getElementById("subregions-container") as HTMLDivElement;
    const subRegionDivs = subRegionContainer.querySelectorAll("");

    // Re-index the remaining subregions

    //answerOptionCount = 0;
    subRegionDivs.forEach((div, index) => {
        const nameInput = div.querySelector("input[name$='Name']") as HTMLInputElement;

        if (nameInput) {
            // Re-index Name input
            nameInput.name = `SubRegions[${index + 1}].Name`;

            // Re-index Size input
            const sizeInput = div.querySelector("input[name$='Size']") as HTMLInputElement;
            sizeInput.name = `SubRegions[${index + 1}].Size`;
        }

        // Update subregionCount to the correct next index
        // answerOptionCount++;
    });
}