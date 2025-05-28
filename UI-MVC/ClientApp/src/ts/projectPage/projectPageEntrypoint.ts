import {initShareHandler} from "./share";
import {initSuggestionVisiblityHandler} from "./suggestionVisibility";
import {initExecuteToggleHandler} from "./executedToggle";
import {initVoteHandler} from "./suggestionVoting";


console.log("Project page entrypoint loaded.")

initShareHandler();
initSuggestionVisiblityHandler();
initExecuteToggleHandler();
initVoteHandler();