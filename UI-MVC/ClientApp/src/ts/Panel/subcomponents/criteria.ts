import {addAnswerOption, reIndexAnswerOptions, resetAnswerCounters, createPercentageInput} from "./answersOption";
import {setValidationMessages} from "./panelFormValidator"
import {criteriaInputUpdateHandler} from "../apiAutofill/apiDataFiller";
import {createElementWithClassNames} from "../../helpers/htmlHelper";
import {createRemoveBtn} from "../../components";

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
    const answerUl = criteriaElement.querySelector("ul.list-group") as HTMLUListElement;
    const isDistributionKnownInput = criteriaElement.querySelector("input[name^='Distributions'][name$='IsDistributionKnown']") as HTMLInputElement;
    
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

    // Wrapper
    const wrapper = createElementWithClassNames("div","mt-4","criteria");
    wrapper.id = criteriaId;
    // Head
    const headDiv = createElementWithClassNames("div","d-flex","flex-row");
    // Name
    const nameDiv = createElementWithClassNames("div","d-flex","flex-row","col-3");
    const nameLabel = createElementWithClassNames("label","card-text","pe-4");
    nameLabel.htmlFor = "criteria-name";
    nameLabel.innerHTML = `<strong>Naam:</strong>`;
    const nameInput = createElementWithClassNames("input", "border","border-1","rounded-2");
    nameInput.id = `criteria-name-${currentCount}`;
    nameInput.className = "border border-1 rounded-2";
    nameInput.type = "text";
    nameInput.placeholder = `Criteria ${currentCount + 1}`;
    nameInput.name = `Distributions[${currentCount}].Name`;
    nameDiv.append(nameLabel, nameInput);

    // isDefault
    const isDefaultDiv = createElementWithClassNames("div","d-flex","flex-row","col-3","form-check","form-switch");
    const isDefaultInput = createElementWithClassNames("input","border","border-1","rounded-2","form-check-input");
    isDefaultInput.id = `is-default-${criteriaId}`;
    isDefaultInput.type = "checkbox";
    isDefaultInput.name = `Distributions[${currentCount}].IsDefault`;
    isDefaultInput.value = "true";
    isDefaultInput.checked = true;
    const isDefaultLabel = createElementWithClassNames("label","card-text","ps-3");
    isDefaultLabel.htmlFor = "is-default-criteria";
    isDefaultLabel.innerHTML = `<strong>Standaard Criteria</strong>`;
    isDefaultDiv.append(isDefaultInput, isDefaultLabel);

    // Remove Button
    const removeBtn = createRemoveBtn(() => removeCriteria(currentCount))

    // Error span for name
    const nameValidationSpan = createElementWithClassNames("span","text-danger","ps-1");
    nameValidationSpan.id = `${nameInput.id}-msg`;

    // Head assembly
    headDiv.append(nameDiv, isDefaultDiv, removeBtn, nameValidationSpan);

    // Question
    const questionDiv = createElementWithClassNames("div","d-flex","flex-row","my-2");
    const questionLabel = createElementWithClassNames("label","card-text","pe-4");
    questionLabel.htmlFor = "criteria-question";
    questionLabel.innerHTML = `<strong>Vraag:</strong>`;
    const questionInput = createElementWithClassNames("input","border","border-1","rounded-2","col-8","criteria-question");
    questionInput.id = `criteria-question-${currentCount}`;
    questionInput.type = "text";
    questionInput.placeholder = `Criteria ${currentCount + 1} vraag`;
    questionInput.name = `Distributions[${currentCount}].Question`;
    // Error span for question
    const questionValidationSpan = createElementWithClassNames("span","text-danger","ps-1");
    questionValidationSpan.id = `${questionInput.id}-msg`;
    questionDiv.append(questionLabel, questionInput, questionValidationSpan);

    // Answers
    const answerHeading = createElementWithClassNames("h6","card-title","pt-2");
    answerHeading.innerHTML = `<strong>Mogelijke antwoorden</strong>`;

    const answerUl = createElementWithClassNames("ul","list-group","list-group-flush");

    // isDistributionKnown
    const isDistributionKnownDiv = createElementWithClassNames("div","d-flex","flex-row","col-3","form-check","form-switch");
    const isDistributionKnownInput = createElementWithClassNames("input","border","border-1","rounded-2","form-check-input");
    isDistributionKnownInput.id = `is-distribution-known-${currentCount}-criteria`;
    isDistributionKnownInput.type = "checkbox";
    isDistributionKnownInput.name = `Distributions[${currentCount}].IsDistributionKnown`;
    isDistributionKnownInput.value = "true";
    isDistributionKnownInput.checked = true;
    isDistributionKnownInput.addEventListener("click", () => {
        toggleAnswerOptionPercentageInput(isDistributionKnownInput.checked, currentCount);
    });
    const isDistributionKnownLabel = createElementWithClassNames("label","card-text","pe-5");
    isDistributionKnownLabel.htmlFor = isDistributionKnownInput.name;
    isDistributionKnownLabel.innerText = "Verdeling gekend";
    isDistributionKnownDiv.append(isDistributionKnownLabel, isDistributionKnownInput);

    answerUl.append(isDistributionKnownDiv);

    // Add Answer Option Button
    const addAnswerOptionBtn = createElementWithClassNames("button","btn","btn-primary","col-2","ms-3","my-2","add-option-button");
    addAnswerOptionBtn.type = "button";
    addAnswerOptionBtn.innerText = "Mogelijkheid Toevoegen";
    addAnswerOptionBtn.addEventListener("click", () => addAnswerOption(currentCount, answerUl, isDistributionKnownInput.checked));

    // Final assembly
    wrapper.append(headDiv, questionDiv, answerHeading, answerUl, addAnswerOptionBtn);

    return wrapper;
}


function removeCriteria(criteriaId: number) {
    let e = event as Event;
    const wrapper = ((e.currentTarget as HTMLButtonElement)
        .parentElement as HTMLDivElement)
        .parentElement as HTMLDivElement

    // Remove the element from the DOM
    wrapper.remove();
    // Rebuild the subregions to fix the indices
    reIndexCriteria();
    resetAnswerCounters(criteriaId)
    criteriaInputUpdateHandler();
}

function toggleAnswerOptionPercentageInput(isChecked: boolean, criteriaId: number) {
    let e = event as Event;
    const ulChildren = (((e.currentTarget as HTMLInputElement)
        .parentElement as HTMLDivElement)
        .parentElement as HTMLUListElement)
        .childNodes;
    //removing the div form the NodeList
    const lis: HTMLLIElement[] = [].slice.call(ulChildren, 1);

    lis.forEach((li, index) => {
        const inputs = li.childNodes;
        const percentageNode = inputs.item(1)
        if (!isChecked) {
            percentageNode.remove()
        } else {
            const percentageInput = createPercentageInput(criteriaId, index);
            li.insertBefore(percentageInput, li.childNodes[1]);
        }
    })
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

        // Update Question input
        const questionInput = div.querySelector("input[name^='Distributions'][name$='Question']") as HTMLInputElement;
        questionInput.name = `Distributions[${newIndex}].Question`;

        // Update isDefault checkbox
        const isDefaultInput = div.querySelector("input[name^='Distributions'][name$='IsDefault']") as HTMLInputElement;
        isDefaultInput.name = `Distributions[${newIndex}].IsDefault`;

        // Update all answer option inputs
        const answerUl = div.querySelector<HTMLUListElement>("ul.list-group") as HTMLUListElement;
        reIndexAnswerOptions(newIndex, answerUl);

        // Update isDistribution known checkbox
        const isDistributionInput = div.querySelector("input[name^='Distributions'][name$='IsDistributionKnown']") as HTMLInputElement;
        isDistributionInput.name = `Distributions[${newIndex}].IsDefault`

        criteriaCount++;
    });
}