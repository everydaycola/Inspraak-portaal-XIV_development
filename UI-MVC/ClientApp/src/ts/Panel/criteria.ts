import {addAnswerOption} from "./answersOption";

let criteriaCount = 0;

export function addCriteria() {
    const criteriaContainer = document.getElementById("criteria-container") as HTMLDivElement;

    const criteriaId = `criteria-${criteriaCount}`;

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
    nameInput.placeholder = `Criteria ${criteriaCount + 1}`;
    nameInput.name = `Distributions[${criteriaCount}].Name`
    
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
    isDefaultInput.name = `Distributions[${criteriaCount}].isDefault`
    
    // Create Delete Button
    const removeBtn = document.createElement("button");
    removeBtn.type = "button";
    removeBtn.className = "btn btn-danger btn-sm";
    removeBtn.innerHTML = `<i class="bi-trash"></i>`;
    removeBtn.addEventListener("click", () => removeCriteria(criteriaId));
    
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
    questionInput.placeholder = `Criteria ${criteriaCount + 1} vraag`;
    questionInput.name = `Distributions[${criteriaCount}].Question`;
    
    // Create Answer Heading
    const answerHeading = document.createElement("h6");
    answerHeading.className = "card-title pt-2"
    answerHeading.innerHTML = `<strong>Mogelijke antwoorden</strong>`
    
    // Create Answer List
    const answerUl = document.createElement("ul");
    answerUl.id = "answers-container"
    answerUl.className = "list-group list-group-flush";
    
    
    
    // Create addAnswerOption Button
    const addAnswerOptionBtn = document.createElement("button")
    addAnswerOptionBtn.type = "button";
    addAnswerOptionBtn.className = "btn btn-primary col-2 ms-3 my-2";
    addAnswerOptionBtn.innerText = "Mogelijkheid Toevoegen";
    addAnswerOptionBtn.addEventListener("click", () => addAnswerOption(criteriaCount, answerUl));
    
    // Appending
    wrapper.append(headDiv,questionDiv,answerHeading,answerUl,addAnswerOptionBtn)
    
    headDiv.append(nameDiv,isDefaultDiv,removeBtn)
    
    nameDiv.append(nameLabel,nameInput);
    isDefaultDiv.append(isDefaultInput,isDefaultLabel);
    
    questionDiv.append(questionLabel,questionInput);
    
    // add 2 Answer Options
    addAnswerOption(criteriaCount, answerUl);
    addAnswerOption(criteriaCount, answerUl);
    
    criteriaContainer.appendChild(wrapper);
    criteriaCount++
}


function removeCriteria(id: string) {
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

    criteriaCount = 0;
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
        criteriaCount++;
    });
}