import {fetchCommunes} from "./apiConnection";
import {attachEventHandlersToSubregionInput} from "./SubRegionHandler";
import {attachEventHandlersToCriteriaInput} from "./CriteriaHandler";

let basicApiData: any[] = [];
const outerDiv = document.querySelector("#subregions-container") as HTMLDivElement;
let subregionInnerDiv = outerDiv.querySelectorAll(".subRegion") as NodeListOf<HTMLDivElement>;

export function setupApiAutoFill() {
//FETCH DATA FROM API
    fetchCommunes()
        .then((data) => {
            basicApiData = data as any[];
            newSubregionInputAddedHandler();
        })
        .catch((error) => {
            console.error('Error fetching basic communes:', error);
        });
}

export function newSubregionInputAddedHandler() {
    subregionInnerDiv = outerDiv.querySelectorAll(".subRegion");
    attachEventHandlersToSubregionInput(outerDiv, subregionInnerDiv, basicApiData);
}

//CRITERIA AUTO FILLING USING API
const criteriaOuterDiv = document.querySelector("#criteria-container") as HTMLDivElement;
let criteriaInnerDivs = criteriaOuterDiv.querySelectorAll(".criteria") as NodeListOf<HTMLDivElement>;

export function criteriaInputUpdateHandler() {
    criteriaInnerDivs = criteriaOuterDiv.querySelectorAll(".criteria") as NodeListOf<HTMLDivElement>
    attachEventHandlersToCriteriaInput(criteriaInnerDivs, basicApiData);
    console.log("criteriaInputChanged");
}
