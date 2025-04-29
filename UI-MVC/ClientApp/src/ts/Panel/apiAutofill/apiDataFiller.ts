import {fetchCommunes} from "./apiConnection";
import {attachEventHandlersToSubregionInput} from "./SubRegionHandler";
import {attachEventHandlersToCriteriaInput} from "./CriteriaHandler";

console.log("API IS ACTIVE");
//GLOBAL CONSTANT CATEGORY NAMES FOR SUGGESTING THEM

//FETCH DATA FROM API 
let basicApiData: any[] = [];
fetchCommunes()
    .then((data) => {
        basicApiData = data as any[];
        newSubregionInputAddedHandler();
    })
    .catch((error) => {
        console.error('Error fetching basic communes:', error);
    });
//SUBREGION AUTO FILL HELPERS
const outerDiv = document.querySelector("#subregions-container") as HTMLDivElement;
let subregionInnerDiv = outerDiv.querySelectorAll(".subRegion") as NodeListOf<HTMLDivElement>;
export function newSubregionInputAddedHandler() {
    subregionInnerDiv = outerDiv.querySelectorAll(".subRegion");
    attachEventHandlersToSubregionInput(subregionInnerDiv, basicApiData);
}

//CRITERIA AUTO FILLING USING API
const criteriaOuterDiv = document.querySelector("#criteria-container") as HTMLDivElement;
let criteriaInnerDivs = criteriaOuterDiv.querySelectorAll(".criteria") as NodeListOf<HTMLDivElement>;
export function criteriaInputUpdateHandler(){
    criteriaInnerDivs = criteriaOuterDiv.querySelectorAll(".criteria") as NodeListOf<HTMLDivElement>
    attachEventHandlersToCriteriaInput(criteriaInnerDivs, basicApiData);
}