import {fetchCommunes} from "./apiConnection";
import {attachEventHandlersToSubregionInput} from "./subRegionHandler";

export let basicApiData: any[] = [];
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

