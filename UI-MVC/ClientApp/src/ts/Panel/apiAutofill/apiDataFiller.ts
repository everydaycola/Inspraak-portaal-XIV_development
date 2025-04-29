import {fetchBasicCommunes, fetchCommunes} from "./apiConnection";

console.log("API IS ACTIVE");

let basicApiData: any[] = [];

fetchCommunes()
    .then((data) => {
        basicApiData = data as any[];
        newSubregionInputAddedHandler();
    })
    .catch((error) => {
        console.error('Error fetching basic communes:', error);
    });

const outerDiv = document.querySelector("#subregions-container") as HTMLDivElement;
let subregionInnerDiv = outerDiv.querySelectorAll(".subRegion") as NodeListOf<HTMLDivElement>;

export function newSubregionInputAddedHandler() {
    subregionInnerDiv = outerDiv.querySelectorAll(".subRegion");
    attachEventHandlersToSubregionInput(subregionInnerDiv);
}


const criteriaOuterDiv = document.querySelector("#criteria-container") as HTMLDivElement;
let criteriaInnerDivs = criteriaOuterDiv.querySelectorAll(".criteria") as NodeListOf<HTMLDivElement>;
console.log(criteriaInnerDivs);
export function criteriaInputUpdateHandler(){
    criteriaInnerDivs = criteriaOuterDiv.querySelectorAll(".criteria") as NodeListOf<HTMLDivElement>
    console.log(criteriaInnerDivs);
}

function attachEventHandlersToSubregionInput(subregionInnerDiv: NodeListOf<HTMLDivElement>) {
    subregionInnerDiv.forEach(innerDiv => {
        const inputs = innerDiv.querySelectorAll("input") as NodeListOf<HTMLInputElement>;
        const nameInput = inputs[0];
        const sizeInput = inputs[1];

        // Create and insert suggestion box
        const suggestionBox = createSuggestionBox();
        innerDiv.insertBefore(suggestionBox, nameInput);

        nameInput.addEventListener("input", () => {
            const inputValue = nameInput.value;
            checkForNameKnownByApi(inputValue, sizeInput);
            updateSuggestions(inputValue, suggestionBox, nameInput);
        });

        document.addEventListener("click", (e) => {
            if (!(e.target as HTMLElement).closest(".suggestion-box")) {
                suggestionBox.style.display = "none";
            }
        });
    });
}
function createSuggestionBox(): HTMLDivElement {
    const box = document.createElement("div");
    box.className = "suggestion-box";
    box.style.position = "absolute";
    box.style.background = "white";
    box.style.border = "1px solid #ccc";
    box.style.padding = "4px";
    box.style.fontSize = "0.9em";
    box.style.zIndex = "1000";
    box.style.display = "none";
    return box;
}

function updateSuggestions(inputValue: string, suggestionBox: HTMLDivElement, nameInput: HTMLInputElement) {
    const value = inputValue.toLowerCase();
    if (!value) {
        suggestionBox.style.display = "none";
        return;
    }

    const matches = basicApiData.filter(commune =>
        (commune.communeName as string).toLowerCase().includes(value)
    ).slice(0, 5); // limit to 5 suggestions

    if (matches.length === 0) {
        suggestionBox.style.display = "none";
        return;
    }

    suggestionBox.innerHTML = "";
    matches.forEach(match => {
        const item = document.createElement("div");
        item.textContent = match.communeName;
        item.style.cursor = "pointer";
        item.style.padding = "2px 4px";

        item.addEventListener("click", () => {
            nameInput.value = match.communeName;
            suggestionBox.style.display = "none";
            checkForNameKnownByApi(match.communeName, nameInput.nextElementSibling as HTMLInputElement);
        });

        suggestionBox.appendChild(item);
    });

    const inputRect = nameInput.getBoundingClientRect();
    const scrollTop = window.scrollY || document.documentElement.scrollTop;
    suggestionBox.style.left = `${inputRect.left}px`;
    suggestionBox.style.top = `${inputRect.top - suggestionBox.offsetHeight + scrollTop - 4}px`;
    suggestionBox.style.width = `${inputRect.width}px`;
    suggestionBox.style.display = "block";
}


function checkForNameKnownByApi(searchedValue: string, sizeInput: HTMLInputElement) {
    searchedValue = searchedValue.toLowerCase();
    const match = basicApiData.find(commune => {
        return (commune.communeName as string).toLowerCase() === searchedValue.toLowerCase();
    });

    console.log(basicApiData);
    if (match) {
        sizeInput.value = String(match.totalPopulation); 
    } else {
        sizeInput.value = '';
    }
}