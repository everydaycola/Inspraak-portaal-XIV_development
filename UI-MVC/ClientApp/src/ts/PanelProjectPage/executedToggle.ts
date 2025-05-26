import {fetchFromAPI} from "../customhelpers/apihelper";
import {getCurrentBaseUrl} from "../customhelpers/locationHelper";

const executedBtns = document.getElementsByClassName("executed-toggle-btn")
for (let i = 0; i < executedBtns.length; i++) {
    executedBtns.item(i)!!.addEventListener("click",(event) => {toggleExecuted(event)})
}

const executedText = "Zet op niet uitgevoerd";
const notExecutedText = "Zet op uitgevoerd";

async function toggleExecuted(event: Event) {
    const button = event.currentTarget as HTMLButtonElement;
    const suggestionId = button.dataset.suggestionId;
    const baseUrl = getCurrentBaseUrl();

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
    } else { 
        button.innerText = notExecutedText;
    }
}