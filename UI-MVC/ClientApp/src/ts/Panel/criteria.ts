import {addAnswerOption, reIndexAnswerOptions} from "./answersOption";
import {resetAnswerCounter} from "./answersOption";
import {answerOptionCounters} from "./answersOption";

let criteriaCount = 0;


export function addCriteria() {
    const criteriaContainer = document.getElementById("criteria-container") as HTMLDivElement;
    const currentCount = criteriaCount++

    const criteriaId = `criteria-${currentCount}`;

    // Create the wrapper div
    const wrapper = document.createElement("div");
    wrapper.id = criteriaId;
    wrapper.className = "mt-4 criteria";

    // Create head div
    const headDiv = document.createElement("div");
    headDiv.className = "d-flex flex-row";

    // Create name div
    const nameDiv = document.createElement("div");
    nameDiv.className = "d-flex flex-row col-3";

    // Create name label
    const nameLabel = document.createElement("label")
    nameLabel.className = "card-text pe-4";
    nameLabel.htmlFor = "criteria-name";
    nameLabel.innerHTML = `<strong>Naam:</strong>`

    // Create name input
    const nameInput = document.createElement("input")
    nameInput.id = "criteria-name";
    nameInput.className = "border border-1 rounded-2";
    nameInput.type = "text";
    nameInput.placeholder = `Criteria ${currentCount + 1}`;
    nameInput.name = `Distributions[${currentCount}].Name`

    // Create isDefault div
    const isDefaultDiv = document.createElement("div");
    isDefaultDiv.className = "d-flex flex-row col-3 form-check form-switch"

    // Create isDefault label
    const isDefaultLabel = document.createElement("label")
    isDefaultLabel.className = "card-text ps-3";
    isDefaultLabel.htmlFor = "is-default-criteria";
    isDefaultLabel.innerHTML = `<strong>Standaard Criteria</strong>`

    // Create isDefault input
    const isDefaultInput = document.createElement("input")
    isDefaultInput.id = "is-default-criteria";
    isDefaultInput.className = "border border-1 rounded-2 form-check-input";
    isDefaultInput.type = "checkbox";
    isDefaultInput.name = `Distributions[${currentCount}].isDefault`

    // Create Delete Button
    const removeBtn = document.createElement("button");
    removeBtn.type = "button";
    removeBtn.className = "btn btn-danger btn-sm";
    removeBtn.innerHTML = `<i class="bi-trash"></i>`;
    removeBtn.addEventListener("click", () => removeCriteria(currentCount));

    // Create Question div
    const questionDiv = document.createElement("div");
    questionDiv.className = "d-flex flex-row my-2";

    // Create Question label
    const questionLabel = document.createElement("label");
    questionLabel.className = "card-text pe-4";
    questionLabel.htmlFor = "criteria-question";
    questionLabel.innerHTML = `<strong>Vraag:</strong>`;

    // Create Question input
    const questionInput = document.createElement("input");
    questionInput.id = "criteria-question";
    questionInput.className = "border border-1 rounded-2 col-8";
    questionInput.type = "text";
    questionInput.placeholder = `Criteria ${currentCount + 1} vraag`;
    questionInput.name = `Distributions[${currentCount}].Question`;

    // Create Answer Heading
    const answerHeading = document.createElement("h6");
    answerHeading.className = "card-title pt-2"
    answerHeading.innerHTML = `<strong>Mogelijke antwoorden</strong>`

    // Create Answer List
    const answerUl = document.createElement("ul");
    answerUl.className = "list-group list-group-flush";


    // Create addAnswerOption Button
    const addAnswerOptionBtn = document.createElement("button")
    addAnswerOptionBtn.type = "button";
    addAnswerOptionBtn.className = "btn btn-primary col-2 ms-3 my-2";
    addAnswerOptionBtn.innerText = "Mogelijkheid Toevoegen";
    addAnswerOptionBtn.addEventListener("click", () => addAnswerOption(currentCount, answerUl));

    // Appending
    wrapper.append(headDiv, questionDiv, answerHeading, answerUl, addAnswerOptionBtn)

    headDiv.append(nameDiv, isDefaultDiv, removeBtn)

    nameDiv.append(nameLabel, nameInput);
    isDefaultDiv.append(isDefaultInput, isDefaultLabel);

    questionDiv.append(questionLabel, questionInput);

    // add 2 Answer Options
    addAnswerOption(currentCount, answerUl);
    addAnswerOption(currentCount, answerUl);

    criteriaContainer.appendChild(wrapper);
}


function removeCriteria(criteriaId: number) {
    let e = event as Event
    const btn = e.currentTarget as HTMLButtonElement;
    const headDiv = btn.parentElement as HTMLDivElement;
    const wrapper = headDiv.parentElement as HTMLDivElement;
    
    // Remove the element from the DOM
    wrapper.remove();
    resetAnswerCounter(criteriaId)
    // Rebuild the subregions to fix the indices
    reIndexCriteria();
    resetAnswerCounters()
}

function reIndexCriteria() {
    const criteriaContainer = document.getElementById("criteria-container") as HTMLDivElement;
    const criteriaDivs = criteriaContainer.querySelectorAll<HTMLDivElement>(".criteria");

    criteriaCount = 0;

    criteriaDivs.forEach((div, newIndex) => {
        div.id = `criteria-${newIndex}`;

        // Update Name input
        const nameInput = div.querySelector("input[name^='Distributions'][name$='Name']") as HTMLInputElement;
        if (nameInput) {
            nameInput.name = `Distributions[${newIndex}].Name`;
        }

        // Update Question input
        const questionInput = div.querySelector("input[name^='Distributions'][name$='Question']") as HTMLInputElement;
        if (questionInput) {
            questionInput.name = `Distributions[${newIndex}].Question`;
        }

        // Update isDefault checkbox
        const isDefaultInput = div.querySelector("input[name^='Distributions'][name$='isDefault']") as HTMLInputElement;
        if (isDefaultInput) {
            isDefaultInput.name = `Distributions[${newIndex}].isDefault`;
        }

        // Update all answer option inputs
        const answerUl = div.querySelector<HTMLUListElement>("ul.list-group") as HTMLUListElement;
        reIndexAnswerOptions(newIndex, answerUl);
        
        const removeBtn = div.querySelector<HTMLButtonElement>("button.btn-danger.btn-sm") as HTMLButtonElement;

        // Add the event listener for the remove button
        removeBtn.addEventListener('click', () => removeCriteria(newIndex));

        criteriaCount++;
    });
}

function resetAnswerCounters() {
    answerOptionCounters.forEach((value, key) => {
        if (key >= criteriaCount) {
            resetAnswerCounter(key)
        }
    })
}