import { addAnswerOption, reIndexAnswerOptions, resetAnswerCounters, createPercentageInput } from "./answersOption";
import { setValidationMessages } from "./panelFormValidator";
import { criteriaInputUpdateHandler } from "../apiAutofill/apiDataFiller";
import { createRemoveBtn } from "../../components";
import { createElementWithClassNames, wrapMultipleElementsWithBootstrapRow } from "../../customhelpers/htmlHelper";

let criteriaCount = 0;

export function addCriteria() {
    const criteriaContainer = document.getElementById("criteria-container") as HTMLDivElement;
    const currentCount = criteriaCount++;

    const criteriaElement = createCriteriaElement(currentCount);
    criteriaContainer.appendChild(criteriaElement);

    const nameInput = criteriaElement.querySelector("input[name^='Distributions'][name$='Name']") as HTMLInputElement;
    const nameError = criteriaElement.querySelector(`#${nameInput.id}-msg`) as HTMLSpanElement;
    const questionInput = criteriaElement.querySelector("input[name^='Distributions'][name$='Question']") as HTMLInputElement;
    const questionError = criteriaElement.querySelector(`#${questionInput.id}-msg`) as HTMLSpanElement;
    const answerUl = criteriaElement.querySelector("ul.answer-list") as HTMLUListElement;
    const isDistributionKnownInput = criteriaElement.querySelector("input[name^='Distributions'][name$='IsDistributionKnown']") as HTMLInputElement;

    // Default: add 2 answer options
    addAnswerOption(currentCount, answerUl, true);
    addAnswerOption(currentCount, answerUl, true);

    // Validation for Name
    nameInput.required = true;
    nameInput.minLength = 2;
    nameInput.maxLength = 20;
    setValidationMessages(nameInput.id, nameError.id, {
        valueMissing: "Criteria moet een naam hebben.",
        tooShort: "Criteria naam moet minimaal 2 karakters lang zijn.",
        tooLong: "Criteria naam mag maximaal 20 karakters lang zijn."
    });

    // Validation for Question
    questionInput.minLength = 6;
    questionInput.maxLength = 100;
    setValidationMessages(questionInput.id, questionError.id, {
        tooShort: "Criteria vraag moet minimaal 6 karakters lang zijn.",
        tooLong: "Criteria vraag mag maximaal 100 karakters lang zijn."
    });

    criteriaInputUpdateHandler();
}

function createCriteriaElement(currentCount: number): HTMLDivElement {
    const criteriaId = `criteria-${currentCount}`;

    // Responsive wrapper
    const wrapper = createElementWithClassNames("div", "mt-4", "criteria", "p-3", "rounded", "shadow-sm", "bg-white", "w-100");
    wrapper.id = criteriaId;

    // Head
    const headDiv = createElementWithClassNames("div", "d-flex", "flex-wrap", "flex-row", "align-items-center", "mb-2");
    // Name
    const nameDiv = createElementWithClassNames("div", "d-flex", "me-4", "flex-grow-1");
    const nameLabel = createElementWithClassNames("label", "card-text", "pe-4");
    nameLabel.htmlFor = `criteria-name-${currentCount}`;
    nameLabel.innerHTML = `<strong>Naam:</strong>`;
    const nameInput = createElementWithClassNames("input", "border", "border-1", "rounded-2", "form-control");
    nameInput.id = `criteria-name-${currentCount}`;
    nameInput.type = "text";
    nameInput.placeholder = `Criteria ${currentCount + 1}`;
    nameInput.name = `Distributions[${currentCount}].Name`;
    nameDiv.append(nameLabel, nameInput);

    // isDefault
    const isDefaultDiv = createElementWithClassNames("div", "form-check", "form-switch", "me-4");
    const isDefaultInput = createElementWithClassNames("input", "form-check-input");
    isDefaultInput.id = `is-default-${criteriaId}`;
    isDefaultInput.type = "checkbox";
    isDefaultInput.name = `Distributions[${currentCount}].IsDefault`;
    isDefaultInput.value = "true";
    isDefaultInput.checked = true;
    const isDefaultLabel = createElementWithClassNames("label", "card-text", "ps-3");
    isDefaultLabel.htmlFor = isDefaultInput.id;
    isDefaultLabel.innerHTML = `<strong>Standaard Criteria</strong>`;
    isDefaultDiv.append(isDefaultInput, isDefaultLabel);

    // Remove Button
    const removeBtn = createRemoveBtn(() => removeCriteria(currentCount));

    // Error span for name
    const nameValidationSpan = createElementWithClassNames("span", "text-danger", "ps-1");
    nameValidationSpan.id = `${nameInput.id}-msg`;

    // Head assembly
    headDiv.append(nameDiv, isDefaultDiv, removeBtn, nameValidationSpan);

    // Question
    const questionDiv = createElementWithClassNames("div", "d-flex", "flex-row", "my-2", "align-items-center");
    const questionLabel = createElementWithClassNames("label", "card-text", "pe-4");
    questionLabel.htmlFor = `criteria-question-${currentCount}`;
    questionLabel.innerHTML = `<strong>Vraag:</strong>`;
    const questionInput = createElementWithClassNames("input", "border", "border-1", "rounded-2", "form-control", "criteria-question");
    questionInput.id = `criteria-question-${currentCount}`;
    questionInput.type = "text";
    questionInput.placeholder = `Criteria ${currentCount + 1} vraag`;
    questionInput.name = `Distributions[${currentCount}].Question`;
    // Error span for question
    const questionValidationSpan = createElementWithClassNames("span", "text-danger", "ps-1");
    questionValidationSpan.id = `${questionInput.id}-msg`;
    questionDiv.append(questionLabel, questionInput, questionValidationSpan);

    // Answers heading and distribution known toggle
    const answerHeading = createElementWithClassNames("h6", "pt-2", "me-2");
    answerHeading.innerHTML = `<strong>Mogelijke antwoorden</strong>`;

    const isDistributionKnownDiv = createElementWithClassNames("div", "d-flex", "form-check", "form-switch", "align-items-center", "ms-2");
    const isDistributionKnownInput = createElementWithClassNames("input", "form-check-input");
    isDistributionKnownInput.id = `is-distribution-known-${currentCount}-criteria`;
    isDistributionKnownInput.type = "checkbox";
    isDistributionKnownInput.name = `Distributions[${currentCount}].IsDistributionKnown`;
    isDistributionKnownInput.value = "true";
    isDistributionKnownInput.checked = true;
    isDistributionKnownInput.setAttribute("aria-checked", "true");
    isDistributionKnownInput.addEventListener("change", (e) => {
        toggleAnswerOptionPercentageInput(isDistributionKnownInput.checked, currentCount);
    });
    const isDistributionKnownLabel = createElementWithClassNames("label", "card-text", "ps-2", "mb-0");
    isDistributionKnownLabel.htmlFor = isDistributionKnownInput.id;
    isDistributionKnownLabel.innerText = "Verdeling gekend";
    isDistributionKnownDiv.append(isDistributionKnownInput, isDistributionKnownLabel);

    const answerHeadingRow = wrapMultipleElementsWithBootstrapRow([answerHeading, isDistributionKnownDiv]);

    // Answer options list
    const answerUl = document.createElement("ul");
    answerUl.className = "answer-list list-group w-100 mb-2";

    // Add Answer Option Button
    const addAnswerOptionBtn = createElementWithClassNames("button", "btn", "btn-primary", "add-option-button", "my-2", "w-100");
    addAnswerOptionBtn.type = "button";
    addAnswerOptionBtn.innerText = "Mogelijkheid Toevoegen";
    addAnswerOptionBtn.addEventListener("click", () => addAnswerOption(currentCount, answerUl, isDistributionKnownInput.checked));

    // Assemble all
    wrapper.append(headDiv, questionDiv, answerHeadingRow, answerUl, addAnswerOptionBtn);

    return wrapper;
}

function removeCriteria(criteriaId: number) {
    const e = event as Event;
    const wrapper = ((e.currentTarget as HTMLButtonElement).parentElement as HTMLDivElement).parentElement as HTMLDivElement;
    wrapper.remove();
    reIndexCriteria();
    resetAnswerCounters(criteriaId);
    criteriaInputUpdateHandler();
}

function toggleAnswerOptionPercentageInput(isChecked: boolean, criteriaId: number) {
    // Find the UL for this criteria
    const criteriaDiv = document.getElementById(`criteria-${criteriaId}`) as HTMLDivElement;
    const answerUl = criteriaDiv.querySelector("ul.answer-list") as HTMLUListElement;
    const answerLis = answerUl.querySelectorAll("li");

    answerLis.forEach((li, index) => {
        // Remove existing percentage input if present
        const oldPercentInput = li.querySelector("input[type='number']");
        if (oldPercentInput) {
            oldPercentInput.remove();
        }
        // Add percentage input if checked
        if (isChecked) {
            const percentInput = createPercentageInput(criteriaId, index);
            li.insertBefore(percentInput, li.children[1]);
        }
    });
}

function reIndexCriteria() {
    const criteriaContainer = document.getElementById("criteria-container") as HTMLDivElement;
    const criteriaDivs = criteriaContainer.querySelectorAll<HTMLDivElement>(".criteria");

    criteriaCount = 0;

    criteriaDivs.forEach((div, newIndex) => {
        div.id = `criteria-${newIndex}`;

        // Update Name input
        const nameInput = div.querySelector("input[name^='Distributions'][name$='Name']") as HTMLInputElement;
        nameInput.name = `Distributions[${newIndex}].Name`;
        nameInput.id = `criteria-name-${newIndex}`;

        // Update Question input
        const questionInput = div.querySelector("input[name^='Distributions'][name$='Question']") as HTMLInputElement;
        questionInput.name = `Distributions[${newIndex}].Question`;
        questionInput.id = `criteria-question-${newIndex}`;

        // Update isDefault checkbox
        const isDefaultInput = div.querySelector("input[name^='Distributions'][name$='IsDefault']") as HTMLInputElement;
        isDefaultInput.name = `Distributions[${newIndex}].IsDefault`;
        isDefaultInput.id = `is-default-criteria-${newIndex}`;

        // Update all answer option inputs
        const answerUl = div.querySelector<HTMLUListElement>("ul.answer-list") as HTMLUListElement;
        reIndexAnswerOptions(newIndex, answerUl);

        // Update isDistributionKnown checkbox
        const isDistributionInput = div.querySelector("input[name^='Distributions'][name$='IsDistributionKnown']") as HTMLInputElement;
        isDistributionInput.name = `Distributions[${newIndex}].IsDistributionKnown`;
        isDistributionInput.id = `is-distribution-known-${newIndex}-criteria`;

        criteriaCount++;
    });
}
