import {fetchFromAPI} from "../customhelpers/apihelper";
import {getCurrentBaseUrl} from "../customhelpers/locationHelper";

const suggestionVisibilityBtns = document.getElementsByClassName("suggestion-visibility-toggle-btn")
for (let i = 0; i < suggestionVisibilityBtns.length; i++) {
    suggestionVisibilityBtns.item(i)!!.addEventListener("click",(event) => {toggleVisibility(event)})
}
const privateText = "Maak publiek"
const publicText = "Maak privé"

async function toggleVisibility(event: Event) {
    const button = event.currentTarget as HTMLButtonElement
    const suggestionId = button.dataset.suggestionId;
    const baseUrl = getCurrentBaseUrl()
    const visibility = await fetchFromAPI(
        `${baseUrl}/api/PanelProjectPages/visibility?suggestionId=${suggestionId}`
    )
    console.log(visibility)
    await fetchFromAPI(
        `${baseUrl}/api/PanelProjectPages/toggleVisibility?suggestionId=${suggestionId}`, {
            method: "POST"
        }
    )
    
    
    if (!visibility){
        button.innerText = publicText
    } else {
        button.innerText = privateText
    }
}