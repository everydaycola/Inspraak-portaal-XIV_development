export function initQuestionCreationFormController() {
    const answerWeightTable: HTMLTableElement | null = document.querySelector("#answerWeightTable");
    const addAnswerOptionButton: HTMLButtonElement | null = document.querySelector("#addAnswerOption");
    const amountOfParticipationMethodInput : HTMLInputElement | null = document.querySelector("#amountOfParticipationMethods");
    
    if (answerWeightTable && addAnswerOptionButton && amountOfParticipationMethodInput) {
        addAnswerOptionButton.addEventListener("click", () => {
            const amountOfParticipationMethods = parseInt(amountOfParticipationMethodInput.value);
            addNewAnswerOptionRow(answerWeightTable,amountOfParticipationMethods);
            console.log("Adding new row to questioncreation form");
        });
    }
}

function addNewAnswerOptionRow(answerWeightTable: HTMLTableElement, amountOfMethods: number) {
    const rowIndex = answerWeightTable.tBodies[0].rows.length;
    const tableRow = document.createElement("tr");

    // Antwoordtekst cel
    const answerCell = document.createElement("td");
    const answerInput = createInputElement(
        "text",
        `QuestionToEdit.AnswerOptions[${rowIndex}].AnswerOptionText`,
        "Antwoordtekst"
    );
    answerCell.appendChild(answerInput);
    tableRow.appendChild(answerCell);
    

    // Gewicht cellen voor elke methode
    for(let i =0; i< amountOfMethods; i++){
        const weightCell = document.createElement("td");
        const weightInput = createInputElement(
            "number",
            `QuestionToEdit.AnswerOptions[${rowIndex}].WeightsPerMethod[}].Weight`,
            "Gewicht"
        );

        // Verborgen methodId input
        const hiddenMethodId = document.createElement("input");
        hiddenMethodId.type = "hidden";
        hiddenMethodId.name = `QuestionToEdit.AnswerOptions[${rowIndex}].WeightsPerMethod[].MethodId`;
        //hiddenMethodId.value = method.id.toString();

        weightCell.append(weightInput, hiddenMethodId);
        tableRow.appendChild(weightCell);
    };

    // Verwijderknop
    const deleteCell = document.createElement("td");
    const deleteButton = document.createElement("button");
    deleteButton.type = "button";
    deleteButton.className = "btn btn-sm btn-outline-danger";
    deleteButton.textContent = "X";
    deleteButton.onclick = () => tableRow.remove();
    deleteCell.appendChild(deleteButton);
    tableRow.appendChild(deleteCell);

    answerWeightTable.appendChild(tableRow);

    // Hulpfunctie voor input creatie
    function createInputElement(type: string, name: string, placeholder: string): HTMLInputElement {
        const input = document.createElement("input");
        input.type = type;
        input.className = "form-control me-2";
        input.name = name;
        input.placeholder = placeholder;
        input.required = true;
        return input;
    }
}
