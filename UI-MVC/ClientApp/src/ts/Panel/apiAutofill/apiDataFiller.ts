import { fetchBasicCommunes } from "./apiConnection";

console.log("API IS ACTIVE");

let basicApiData: any[] = [];

fetchBasicCommunes()
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

function attachEventHandlersToSubregionInput(subregionInnerDiv: NodeListOf<HTMLDivElement>) {
    subregionInnerDiv.forEach(innerDiv => {
        const inputs = innerDiv.querySelectorAll("input") as NodeListOf<HTMLInputElement>;
        const nameInput = inputs[0];
        const sizeInput = inputs[1];

        nameInput.addEventListener("input", () => {
            checkForNameKnownByApi(nameInput.value, sizeInput);
        });
    });
}

function checkForNameKnownByApi(searchedValue: string, sizeInput: HTMLInputElement) {
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
