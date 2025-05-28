
import {getAllSubRegions} from "./subRegionHandler";
import {createSuggestionBox, wrapElementWithBootstrapRow} from "../../customhelpers/htmlHelper";

const available_criteria_categories : string[] = ["Geslacht","Werkend", "Opleidingsgraad"]

export function attachEventHandlersToCriteriaInput(subregionInnerDiv: NodeListOf<HTMLDivElement>, basicApiData : any[]) {
    var currentlyUsedSubregions = getAllSubRegions(basicApiData);
    subregionInnerDiv.forEach(innerDiv => {
        const inputs = innerDiv.querySelectorAll("input") as NodeListOf<HTMLInputElement>;
        const nameInput = inputs[0];
        const suggestionBox = createSuggestionBox();
        const suggestionBoxWithRow= wrapElementWithBootstrapRow(suggestionBox);
        const firstRowOfInnerDiv = innerDiv.children[0]
        innerDiv.insertBefore(suggestionBoxWithRow, firstRowOfInnerDiv);
        nameInput.addEventListener("input", () => {
            const inputValue = nameInput.value.toLowerCase();
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
    const vraagInput : HTMLInputElement | null = innerDiv.querySelector<HTMLInputElement>(".criteria-question");
    let answerOption = innerDiv.querySelectorAll<HTMLInputElement>(".answer-option");
    let antwoord1Input : HTMLInputElement = answerOption[0];
    let antwoord2Input : HTMLInputElement = answerOption[1];
    let answerOptionInputs = innerDiv.querySelectorAll<HTMLInputElement>(".answer-option-distribution");
    let distributionInput1 : HTMLInputElement = answerOptionInputs[0];
    let distributionInput2 :HTMLInputElement = answerOptionInputs[1];
    if(antwoord1Input == null){
        mogelijkheidToevoegenButton?.click();
        answerOption = innerDiv.querySelectorAll<HTMLInputElement>(".answer-option");
        answerOptionInputs = innerDiv.querySelectorAll<HTMLInputElement>(".answer-option-distribution");
        antwoord1Input = answerOption[0];
        distributionInput1 = answerOptionInputs[0];
    }
    if(antwoord2Input == null){
        mogelijkheidToevoegenButton?.click();
        answerOption = innerDiv.querySelectorAll<HTMLInputElement>(".answer-option");
        answerOptionInputs = innerDiv.querySelectorAll<HTMLInputElement>(".answer-option-distribution");
        antwoord2Input = answerOption[1];
        distributionInput2 = answerOptionInputs[1];
    }
    
    if(vraagInput){
        if(selectedItem === available_criteria_categories[0]){
            vraagInput.value = "Wat is uw geslacht?";
            antwoord1Input.value = "Man";
            antwoord2Input.value = "Vrouw";
            const average = usedData.reduce((sum, dataEntry) => sum + parseFloat(dataEntry.percentageMen), 0) / usedData.length;
            distributionInput1.value = String(average)
            distributionInput2.value = String(100 - average);
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
            const totalPopulation = usedData.reduce((sum, dataEntry) => sum + parseFloat(dataEntry.totalPopulation), 0)
            const totalPeopleWorking = usedData.reduce((sum, dataEntry) => sum + parseFloat(dataEntry.totalPeopleWorking), 0)
            const totalPeopleLookingForWork = usedData.reduce((sum, dataEntry) => sum + parseFloat(dataEntry.totalPeopleLookingForWork), 0)
            
            var peopleWorkingPercentage = calculatePercentage(totalPeopleWorking, totalPopulation);
            var peopleLookingForWorkPercentage = calculatePercentage(totalPeopleLookingForWork, totalPopulation);
            
            distributionInput1.value = String(peopleWorkingPercentage.toFixed(2))
            distributionInput2.value = String(peopleLookingForWorkPercentage.toFixed(2))
            distributionInput3.value = String((100 - parseFloat(distributionInput1.value) - parseFloat(distributionInput2.value)).toFixed(2))
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
            vraagInput.value = "Studeerd u momenteel?";
            antwoord1Input.value = "Secundair onderwijs";
            antwoord2Input.value = "Hoger onderwijs";
            antwoord3Input.value = "Anders"

            const totalPopulation = usedData.reduce((sum, dataEntry) => sum + parseFloat(dataEntry.totalPopulation), 0)
            const secondarySchoolTotal = usedData.reduce((sum, dataEntry) => sum + parseFloat(dataEntry.secondarySchoolStudents), 0)
            const higherSchooledTotal = usedData.reduce((sum, dataEntry) => sum + parseFloat(dataEntry.higherEducation), 0)

            var peopleSecondarySchool = calculatePercentage(secondarySchoolTotal, totalPopulation);
            var peopleInHigherSchool = calculatePercentage(higherSchooledTotal, totalPopulation);
            
            distributionInput1.value = String(peopleSecondarySchool.toFixed(2))
            distributionInput2.value = String(peopleInHigherSchool.toFixed(2))
            distributionInput3.value = String((100 - parseFloat(distributionInput1.value) - parseFloat(distributionInput2.value)).toFixed(2))
        }
    }
}


function calculatePercentage(part: number, total: number): number {
    if (total === 0) {
        throw new Error("Total amount of citizens cannot be zero.");
    }
    return (part / total) * 100;
}