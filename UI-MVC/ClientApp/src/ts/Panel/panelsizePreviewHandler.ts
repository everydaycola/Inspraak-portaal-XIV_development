import {fetchFromAPI, getCurrentBaseUrl} from "./apiAutofill/apiConnection";


export function setupPanelsizePreviewHandlers() {
    const reservePercentInput: HTMLInputElement | null = document.querySelector("#reserve");
    const sampleRateInput: HTMLInputElement | null = document.querySelector("#sample-rate");
    if (reservePercentInput && sampleRateInput) {
        reservePercentInput.addEventListener("input", () => {
            handleInputChange(reservePercentInput, sampleRateInput)
        })
        sampleRateInput.addEventListener("input", () => {
            handleInputChange(reservePercentInput, sampleRateInput)
        })
    }
}

export function onSubregionChange() {
    console.log("subregion change!")
    const reservePercentInput: HTMLInputElement | null = document.querySelector("#reserve");
    const sampleRateInput: HTMLInputElement | null = document.querySelector("#sample-rate");
    if (reservePercentInput && sampleRateInput) {
        const subregSizeInputs: NodeListOf<HTMLInputElement> = document.querySelectorAll(".subregion-size-input");
        subregSizeInputs.forEach(subreg => {
            subreg.addEventListener("change", (e) => {
                handleInputChange(reservePercentInput, sampleRateInput)
            })
        })
    }
}

async function handleInputChange(
    reservePercentInput: HTMLInputElement,
    sampleRateInput: HTMLInputElement
) {
    const baseUrl = getCurrentBaseUrl();
    const citizenCount = getSelectedCommunesTotalCount();
    const reservePercentage = parseFloat(reservePercentInput.value);
    const samplePercentage = parseFloat(sampleRateInput.value);

    const reserveCount: number = await fetchFromAPI(
        `${baseUrl}/api/Calculations/reservesize?citizenCount=${citizenCount}&reservePercentage=${reservePercentage}`
    );

    const totalCitizenCount = citizenCount + reserveCount;
    const estimatedPanelSize: number = await fetchFromAPI(
        `${baseUrl}/api/Calculations/panelsize?citizenCount=${totalCitizenCount}&samplePercentage=${samplePercentage}`
    );

    const outputField = document.querySelector(".panel-size-output");
    if (outputField) {
        outputField.innerHTML = "panel grootte: " + estimatedPanelSize;
    }
}

function getSelectedCommunesTotalCount() {
    const subregSizeInputs: NodeListOf<HTMLInputElement> = document.querySelectorAll(".subregion-size-input");
    let totalCount = Array.from(subregSizeInputs).reduce((sum, input) => sum + (parseFloat(input.value) || 0), 0);
    return totalCount;
}