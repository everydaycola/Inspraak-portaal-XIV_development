import {fetchFromAPI} from "./customhelpers/apihelper";
import {getCurrentBaseUrl} from "./customhelpers/locationHelper";

console.log("Explor concept");

const form: HTMLFormElement | null = document.querySelector("#exploreconcept-form");


if(form){
    form.addEventListener("submit", event => {
        event.preventDefault();
        handleSelectedValues();
    })
}

async function handleSelectedValues(){
    const checkedRadios = document.querySelectorAll<HTMLInputElement>('input[type="radio"]:checked');
    const conclusiefield: HTMLElement | null = document.querySelector("#conclusiefield");
    let totalWeight = 0;
    checkedRadios.forEach(radio => {
        const weight = parseInt(radio.getAttribute('data-answer-weight') || '0', 10);
        totalWeight += weight;
    });

    const baseUrl = getCurrentBaseUrl();
    const data: string = await fetchFromAPI(
        `${baseUrl}/api/ExploreConcepts/SubmitAnswers?totalWeight=${totalWeight}`
    );
    
    if(conclusiefield){
        conclusiefield.innerText = data.suitability;
    }
}