import {fetchFromAPI} from "../customHelpers/apihelper";
import {getCurrentBaseUrl} from "../customHelpers/locationHelper";

export function initExecuteToggleHandler() {
    const executedBtns = document.getElementsByClassName("executed-toggle-btn")
    for (let i = 0; i < executedBtns.length; i++) {
        executedBtns.item(i)!!.addEventListener("click", (event) => {
            toggleExecuted(event)
        })
    }
}

const executedText = "Zet op niet uitgevoerd";
const notExecutedText = "Zet op uitgevoerd";

async function toggleExecuted(event: Event) {
    const button = event.currentTarget as HTMLButtonElement;
    const suggestionId = button.dataset.suggestionId;
    const baseUrl = getCurrentBaseUrl();

    const iconElement = document.getElementById(`suggestion-icon-${suggestionId}`);
    const executedTextElement = document.getElementById(`executed-text-${suggestionId}`);
    const executedSpacerElement = document.getElementById(`executed-spacer-${suggestionId}`);


    const isExecuted = await fetchFromAPI(
        `${baseUrl}/api/PanelProjectPages/executedValue?suggestionId=${suggestionId}`
    );
    console.log(`Current isExecuted status: ${isExecuted}`);

    await fetchFromAPI(
        `${baseUrl}/api/PanelProjectPages/toggleExecuted?suggestionId=${suggestionId}`, {
            method: "POST"
        }
    );

    if (!isExecuted) {
        button.innerText = executedText;
        if (iconElement) {
            iconElement.classList.remove("bi-lightbulb");
            iconElement.classList.remove("text-danger");
            iconElement.classList.add("bi-check-circle-fill", "text-success");
        }
        if (executedTextElement)
            executedTextElement.classList.remove("d-none");

        if (executedSpacerElement)
            executedSpacerElement.classList.remove("d-none");

    } else {
        button.innerText = notExecutedText;
        if (iconElement) {
            iconElement.classList.remove("bi-check-circle-fill", "text-success");
            iconElement.classList.add("bi-lightbulb");
        }
        if (executedTextElement)
            executedTextElement.classList.add("d-none");

        if (executedSpacerElement)
            executedSpacerElement.classList.add("d-none");
    }
}