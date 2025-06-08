import {getCurrentBaseUrl} from "../customHelpers/locationHelper";
import {initQuestionCreationFormController} from "./questionCreationFormController";

console.log("Explore concept entrypoint loaded");

const form: HTMLFormElement | null = document.querySelector("#exploreconcept-form");


if (form) {
    form.addEventListener("submit", event => {
        event.preventDefault();
        handleSelectedValues();
    })
}

async function handleSelectedValues() {
    const checkedRadios = document.querySelectorAll<HTMLInputElement>('input[type="radio"]:checked');
    const conclusiefield: HTMLElement | null = document.querySelector("#conclusiefield");
    const conclusieNamefield: HTMLElement | null = document.querySelector("#conclusieName");
    const aanmeldButton: HTMLAnchorElement | null = document.querySelector("#aanmeldbutton");
    const aanbevelingImage: HTMLImageElement | null = document.querySelector("#aanbeveling-image");

    if (aanmeldButton) {
        aanmeldButton.classList.add("d-none");
    }

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
    if (conclusiefield && conclusieNamefield && aanmeldButton && aanbevelingImage) {
        if (data.name.toLowerCase() == "burgerpanel") {
            aanmeldButton.classList.remove("d-none");
        }
        conclusieNamefield.innerText = data.name;
        conclusiefield.innerText = data.suitability;
        if (data.imageUri.startsWith("images/")) {
            aanbevelingImage.src = data.imageUri;
        } else {
            aanbevelingImage.src = `/Storage/GetFile?fileName=${data.imageUri}`;
        }
    }

    const card = document.getElementById("recommendation-card");
    const exploreConceptForm = document.getElementById("explore-concept-form");
    if (card) card.classList.remove("d-none");
    if (exploreConceptForm) {
        exploreConceptForm.classList.remove("col-lg-12")
        exploreConceptForm.classList.add("col-lg-7")
    }
}

initQuestionCreationFormController();