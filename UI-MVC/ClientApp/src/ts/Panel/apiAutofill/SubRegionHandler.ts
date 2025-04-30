import {createSuggestionBox} from "../../Helpers/HtmlHelper";

const outerDiv = document.querySelector("#subregions-container") as HTMLDivElement;
let subregionInnerDiv = outerDiv.querySelectorAll(".subRegion") as NodeListOf<HTMLDivElement>;


export function attachEventHandlersToSubregionInput(subregionInnerDiv: NodeListOf<HTMLDivElement>, basicApiData : any[]) {
    subregionInnerDiv.forEach(innerDiv => {
        const inputs = innerDiv.querySelectorAll("input") as NodeListOf<HTMLInputElement>;
        const nameInput = inputs[0];
        const sizeInput = inputs[1];
        // Create and insert suggestion box
        const suggestionBox = createSuggestionBox();
        innerDiv.insertBefore(suggestionBox, nameInput);
        nameInput.addEventListener("input", () => {
            const inputValue = nameInput.value;
            checkForNameKnownByApi(inputValue, sizeInput,basicApiData);
            updateSuggestions(inputValue, suggestionBox, nameInput,basicApiData);
        });
        document.addEventListener("click", (e) => {
            if (!(e.target as HTMLElement).closest(".suggestion-box")) {
                suggestionBox.style.display = "none";
            }
        });
    });
}
function checkForNameKnownByApi(searchedValue: string, sizeInput: HTMLInputElement,basicApiData: any[]) {
    searchedValue = searchedValue.toLowerCase();
    const match = basicApiData.find(commune => {
        return (commune.communeName as string).toLowerCase() === searchedValue.toLowerCase();
    });
    
    if (match) {
        sizeInput.value = String(match.totalPopulation);
    } else {
        sizeInput.value = '';
    }
}
function updateSuggestions(inputValue: string, suggestionBox: HTMLDivElement, nameInput: HTMLInputElement, basicApiData: any[]) {
    const value = inputValue.toLowerCase();
    if (!value) {
        suggestionBox.style.display = "none";
        return;
    }
    const matches = basicApiData.filter(commune =>
        (commune.communeName as string).toLowerCase().includes(value)
    ).slice(0, 5);
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
            checkForNameKnownByApi(match.communeName, nameInput.nextElementSibling as HTMLInputElement, basicApiData);
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

export function getAllSubRegions(basicApiData : any[]): any[] {
    const subregionElements = document.querySelectorAll(".subRegion") as NodeListOf<HTMLDivElement>;
    let usedData: any[] = [];

    subregionElements.forEach(subregion => {
        const nameInput = subregion.querySelector("input") as HTMLInputElement;
        const found = basicApiData.find(commune => commune.communeName === nameInput.value.trim());
        if (found) {
            usedData.push(found);
        }
    });
    return usedData;
}