import {fetchFromAPI} from "./customhelpers/apihelper";
import {getCurrentBaseUrl} from "./customhelpers/locationHelper";
import {initQuestionCreationFormController} from "./exploreConcept/questionCreationFormController";

console.log("Explore concept entrypoint loaded");

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
    
    const totalWeightsByMethod: Record<string, number> = {};
    checkedRadios.forEach(radio => {
        const dataImpacts = radio.getAttribute("data-impacts");
        if (!dataImpacts) return;
        const impacts = JSON.parse(dataImpacts);
        impacts.forEach((impact: { ParticipationMethodName: string; ImpactWeight: number }) => {
            const name = impact.ParticipationMethodName;
            const weight = impact.ImpactWeight;

            if (!totalWeightsByMethod[name]) {
                totalWeightsByMethod[name] = 0;
            }
            totalWeightsByMethod[name] += weight;
        });
    });

    const baseUrl = getCurrentBaseUrl();
    const response = await fetch(`${baseUrl}/api/ExploreConcepts/SubmitAnswers`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(totalWeightsByMethod),
    });

    const data = await response.json();
    if (conclusiefield) {
        conclusiefield.innerText = data.suitability;
    }
}
initQuestionCreationFormController();