

export function setupPanelsizePreviewHandlers(){
    const reservePercentInput: HTMLInputElement | null = document.querySelector("#reserve");
    const sampleRateInput: HTMLInputElement | null = document.querySelector("#sample-rate");
    const responseRateInput: HTMLInputElement | null = document.querySelector("#response-rate");
    if(reservePercentInput && sampleRateInput && responseRateInput){
        reservePercentInput.addEventListener("input", handleReservePercentInput)
        sampleRateInput.addEventListener("input", handleSampleRateInput)
        responseRateInput.addEventListener("input", handleResponseRateInput)
    }
}

function handleReservePercentInput() {
    console.log("reserveInput")
}
function handleSampleRateInput() {
    console.log("sampleRate")
}
function handleResponseRateInput() {
    console.log("responseRate")
}
function getSelectedCommunesTotalCount(){
    const subregSizeInputs :NodeListOf<HTMLInputElement> = document.querySelectorAll(".subregion-size-input");
    let totalCount = Array.from(subregSizeInputs).reduce((sum, input) => sum + (parseFloat(input.value) || 0), 0);
    return totalCount;
}