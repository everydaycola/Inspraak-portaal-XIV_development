let answerWeightTable: HTMLTableElement | null = null;

export function initQuestionCreationFormController() {
    answerWeightTable = document.querySelector("#answerWeightTable");
    const addAnswerOptionButton: HTMLButtonElement | null = document.querySelector("#addAnswerOption");
    const amountOfParticipationMethodInput: HTMLInputElement | null = document.querySelector("#amountOfParticipationMethods");
    const participationMehodsElement: HTMLInputElement | null = document.querySelector("#ParticipationMethods");

    if (answerWeightTable && addAnswerOptionButton && amountOfParticipationMethodInput && participationMehodsElement) {
        addAnswerOptionButton.addEventListener("click", () => {
            const amountOfParticipationMethods = parseInt(amountOfParticipationMethodInput.value);
            addNewAnswerOptionRow(amountOfParticipationMethods, JSON.parse(participationMehodsElement.value));
            console.log("Adding new row to questioncreation form");
        });
    }
}

function addNewAnswerOptionRow(amountOfMethods: number,methodNames: string[]) {
    if (answerWeightTable) {
        const rowIndex = answerWeightTable.tBodies[0].rows.length ;
        const tableBody = answerWeightTable.querySelector("tbody")!!;
        const tableRow = document.createElement("tr");

        // Antwoordtekst cel
        const answerCell = document.createElement("td");
        const answerInput = createInputElement(
            "text",
            `AnswerOptions[${rowIndex}].AnswerText`,
            "Antwoordtekst"
        );
        answerCell.appendChild(answerInput);
        tableRow.appendChild(answerCell);


        // Gewicht cellen voor elke methode
        for (let i = 0; i < amountOfMethods; i++) {
            const weightCell = document.createElement("td");
            const weightInput = createInputElement(
                "number",
                `AnswerOptions[${rowIndex}].Impacts[${i}].Impactweight`,
                "Gewicht"
            );
            weightInput.value="0";

            // Verborgen methodId input
            const hiddenMethodId = document.createElement("input");
            hiddenMethodId.type = "hidden";
            hiddenMethodId.name = `AnswerOptions[${rowIndex}].Impacts[${i}].ParticipationMethodName`;
            hiddenMethodId.value = methodNames[i];

            weightCell.append(weightInput, hiddenMethodId);
            tableRow.appendChild(weightCell);
        };

        // Verwijderknop
        const deleteCell = document.createElement("td");
        const deleteButton = document.createElement("button");
        deleteButton.type = "button";
        deleteButton.className = "btn btn-sm btn-outline-danger";
        deleteButton.textContent = "X";
        deleteButton.onclick = () => {
            tableRow.remove()
            reindexRows();
        };
        deleteCell.appendChild(deleteButton);
        tableRow.appendChild(deleteCell);

        tableBody.appendChild(tableRow);

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
}

function reindexRows() {
    const tableBody = answerWeightTable?.querySelector("tbody");
    if (!tableBody) return;
    
    const rows = tableBody.querySelectorAll("tr");
    rows.forEach((row, newRowIndex) => {
        const answerInput = row.querySelector<HTMLInputElement>(`input[name^="AnswerOptions["][name$="].AnswerText"]`);
        if (answerInput) {
            answerInput.name = `AnswerOptions[${newRowIndex}].AnswerText`;
        }
        const impactInputs = row.querySelectorAll<HTMLInputElement>(`input[name^="AnswerOptions["][name*=".Impacts["]`);
        impactInputs.forEach(input => {
            input.name = input.name.replace(
                /AnswerOptions\[\d+\]/,
                `AnswerOptions[${newRowIndex}]`
            );
        });
    });
}

