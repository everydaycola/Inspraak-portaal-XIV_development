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
    removeBtn.addEventListener("click", () => removeAnswerOption(answerOptionId, criteriaId, answersContainer));

    // Appending
    answerOptionLi.append(answerOptionInput, answerOptionPercentageInput, removeBtn);
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

export function resetAnswerCounter(criteriaId: number) {
    answerOptionCounters.set(criteriaId, 0)
}

