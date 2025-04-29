import {createSuggestionBox} from "../../Helpers/HtmlHelper";
import {getAllSubRegions} from "./SubRegionHandler";

const available_criteria_categories : string[] = ["Geslacht","Werkend", "Opleidingsgraad"]

export function attachEventHandlersToCriteriaInput(subregionInnerDiv: NodeListOf<HTMLDivElement>, basicApiData : any[]) {
    var currentlyUsedSubregions = getAllSubRegions(basicApiData);
    console.log(currentlyUsedSubregions);
    subregionInnerDiv.forEach(innerDiv => {
        const inputs = innerDiv.querySelectorAll("input") as NodeListOf<HTMLInputElement>;
        const nameInput = inputs[0];
        // Create and insert suggestion box
        const suggestionBox = createSuggestionBox();
        innerDiv.append(suggestionBox);
        nameInput.addEventListener("input", () => {
            const inputValue = nameInput.value.toLowerCase();
            // Filter suggestions based on input value
            const matches = available_criteria_categories.filter(category =>
                category.toLowerCase().includes(inputValue)
            ).slice(0, 5);
            if (matches.length === 0) {
                suggestionBox.style.display = "none";
                return;
            }
            suggestionBox.innerHTML = "";
            matches.forEach(match => {
                const item = document.createElement("div");
                item.textContent = match;
                item.style.cursor = "pointer";
                item.style.padding = "2px 4px";

                item.addEventListener("click", () => {
                    nameInput.value = match;
                    suggestionBox.style.display = "none";
                    handleSelectedCriteriaFromHelper(match,innerDiv,currentlyUsedSubregions);
                });
                suggestionBox.appendChild(item);
            });

            const inputRect = nameInput.getBoundingClientRect();
            const scrollTop = window.scrollY || document.documentElement.scrollTop;
            suggestionBox.style.left = `${inputRect.left}px`;
            suggestionBox.style.top = `${inputRect.top - suggestionBox.offsetHeight + scrollTop - 4}px`;
            suggestionBox.style.width = `${inputRect.width}px`;
            suggestionBox.style.display = "block";
        });
        document.addEventListener("click", (e) => {
            if (!(e.target as HTMLElement).closest(".suggestion-box")) {
                suggestionBox.style.display = "none";
            }
        });
    });
}

function handleSelectedCriteriaFromHelper(selectedItem: string, innerDiv : HTMLDivElement, usedData : any[]) {
    const mogelijkheidToevoegenButton : HTMLButtonElement | null = innerDiv.querySelector(".add-option-button");
    const vraagInput : HTMLInputElement | null = innerDiv.querySelector<HTMLInputElement>("#criteria-question");
    let answerOption = innerDiv.querySelectorAll<HTMLInputElement>(".answer-option");
    let antwoord1Input = answerOption[0];
    let antwoord2Input = answerOption[1];
    let answerOptionInputs = innerDiv.querySelectorAll<HTMLInputElement>(".answer-option-distribution");
    let distributionInput1 = answerOptionInputs[0];
    let distributionInput2 = answerOptionInputs[1];
    
    if(vraagInput){
        if(selectedItem === available_criteria_categories[0]){
            vraagInput.value = "Wat is uw geslacht?";
            antwoord1Input.value = "Man";
            antwoord2Input.value = "Vrouw";
            //TODO:Calculate acual percentage based on API.
            distributionInput1.value = String(49.5)
            distributionInput2.value = String(51.5)
        }
        if(selectedItem === available_criteria_categories[1]){
            mogelijkheidToevoegenButton?.click();
            answerOption = innerDiv.querySelectorAll<HTMLInputElement>(".answer-option");
            antwoord1Input = answerOption[0];
            antwoord2Input = answerOption[1];
            answerOptionInputs = innerDiv.querySelectorAll<HTMLInputElement>(".answer-option-distribution");
            distributionInput1 = answerOptionInputs[0];
            distributionInput2 = answerOptionInputs[1];
            const antwoord3Input = answerOption[2];
            const distributionInput3 = answerOptionInputs[2];
            vraagInput.value = "Welke staat beschrijft u bet beste?";
            antwoord1Input.value = "Niet werkend";
            antwoord2Input.value = "Werkzoekend";
            antwoord3Input.value = "Werkend"

            distributionInput1.value = String(50)
            distributionInput2.value = String(30)
            distributionInput3.value = String(20)
        }
        if(selectedItem === available_criteria_categories[2]){
            mogelijkheidToevoegenButton?.click();
            answerOption = innerDiv.querySelectorAll<HTMLInputElement>(".answer-option");
            antwoord1Input = answerOption[0];
            antwoord2Input = answerOption[1];
            answerOptionInputs = innerDiv.querySelectorAll<HTMLInputElement>(".answer-option-distribution");
            distributionInput1 = answerOptionInputs[0];
            distributionInput2 = answerOptionInputs[1];
            const antwoord3Input = answerOption[2];
            const distributionInput3 = answerOptionInputs[2];
            vraagInput.value = "Wat is uw hoogst behaalde diploma?";
            antwoord1Input.value = "Secundair onderwijs";
            antwoord2Input.value = "Hoger onderwijs of universiteit";
            antwoord3Input.value = "Anders"

            distributionInput1.value = String(20)
            distributionInput2.value = String(30)
            distributionInput3.value = String(50)
        }
    }
}
