import {round} from "@popperjs/core/lib/utils/math";

export let answerOptionCounters: Map<number, number> = new Map()


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
    answerOptionInput.className = "border border-1 rounded-2 me-2 answer-option";
    answerOptionInput.type = "text";
    answerOptionInput.placeholder = `Antwoord ${(answerCount + 1)}`
    
    // Add validation attributes
    answerOptionInput.setAttribute("data-val", "true");
    answerOptionInput.setAttribute("data-val-required", "Antwoord optie moet een naam hebben.");
    answerOptionInput.setAttribute("data-val-minlength", "2");
    answerOptionInput.setAttribute("data-val-minlength-min", "2");
    answerOptionInput.setAttribute("data-val-maxlength", "20");
    answerOptionInput.setAttribute("data-val-maxlength-max", "20");

    // Validation span for option
    const answerOptionSpan = document.createElement("span");
    answerOptionSpan.className = "text-danger field-validation-valid";
    answerOptionSpan.setAttribute("data-valmsg-for", answerOptionInput.name);
    answerOptionSpan.setAttribute("data-valmsg-replace", "true");

    // Create answerOptionPercentage input
    const answerOptionPercentageInput = document.createElement("input");
    answerOptionPercentageInput.name = `Distributions[${criteriaId}].AnswerOptions[${answerCount}].DistributionPercentage`;
    answerOptionPercentageInput.className = "border border-1 rounded-2 me-2 answer-option-distribution";
    answerOptionPercentageInput.type = "number";
    answerOptionPercentageInput.placeholder = `${round(100 / (answerCount + 1))}`
    
    // Add validation attributes
    answerOptionPercentageInput.setAttribute("data-val", "true");
    answerOptionPercentageInput.setAttribute("data-val-required", "Antwoord optie moet een verdeling waarde hebben.");
    answerOptionPercentageInput.setAttribute("data-val-range", "Percentage moet tussen 0 en 100% zijn.");
    answerOptionPercentageInput.setAttribute("data-val-range-min", "0");
    answerOptionPercentageInput.setAttribute("data-val-range-max", "1");

// Validation span for percentage
    const answerOptionPercentageSpan = document.createElement("span");
    answerOptionPercentageSpan.className = "text-danger field-validation-valid";
    answerOptionPercentageSpan.setAttribute("data-valmsg-for", answerOptionPercentageInput.name);
    answerOptionPercentageSpan.setAttribute("data-valmsg-replace", "true");
    
    // Create Delete Button
    const removeBtn = document.createElement("button");
    removeBtn.type = "button";
    removeBtn.className = "btn btn-danger btn-sm";
    removeBtn.innerHTML = `<i class="bi-trash"></i>`;
    removeBtn.addEventListener("click", () => removeAnswerOption(answerOptionId, criteriaId, answersContainer));
    

    // Appending
    answerOptionLi.append(answerOptionInput, answerOptionPercentageInput, removeBtn, answerOptionSpan, answerOptionPercentageSpan);
    answersContainer.append(answerOptionLi);

    // Counter ++
    answerOptionCounters.set(criteriaId, answerCount + 1);
}

function removeAnswerOption(id: string, criteriaId: number, answersContainer: HTMLUListElement) {
    let e = event as Event
    const btn = e.currentTarget as HTMLButtonElement;
    const answerOptionLi = btn.parentElement as HTMLLIElement
    answerOptionLi.remove();
    reIndexAnswerOptions(criteriaId, answersContainer);
}

export function reIndexAnswerOptions(criteriaId: number, answersContainer: HTMLUListElement) {
    const answerLis = answersContainer.querySelectorAll<HTMLLIElement>("li");

    // Update the counter
    answerOptionCounters.set(criteriaId, answerLis.length);

    answerLis.forEach((li, index) => {
        const textInput = li.querySelector<HTMLInputElement>("input[type='text']") as HTMLInputElement;
        const percentInput = li.querySelector<HTMLInputElement>("input[type='number']") as HTMLInputElement;
        const removeBtn = li.querySelector<HTMLButtonElement>("button.btn-danger") as HTMLButtonElement;

        li.id = `option-${criteriaId}-${index}`;

        textInput.name = `Distributions[${criteriaId}].AnswerOptions[${index}].Option`;
        textInput.placeholder = `Antwoord ${index + 1}`;

        percentInput.name = `Distributions[${criteriaId}].AnswerOptions[${index}].DistributionPercentage`;

        // Add the event listener for remove button
        removeBtn.addEventListener('click', () => removeAnswerOption(li.id, criteriaId, answersContainer));

    });
}

function resetAnswerCounter(criteriaId: number) {
    answerOptionCounters.set(criteriaId, 0)
}

export function resetAnswerCounters(criteriaCount:number) {
    answerOptionCounters.forEach((value, key) => {
        if (key >= criteriaCount) {
            resetAnswerCounter(key)
        }
    })
}
