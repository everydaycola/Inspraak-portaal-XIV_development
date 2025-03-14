import '@popperjs/core';
import 'bootstrap';
import 'bootstrap-icons/font/bootstrap-icons.css';
import 'bootstrap/dist/css/bootstrap.css';

// Custom CSS imports
import '../css/site.css'

// Custom Ts
import {defaultPanel} from "./Panel/makeNewPanel";
import {fetchUniqueCodes} from "./PanelManagement/panelManagement";

console.log('The \'site\' bundle has been loaded!');

if (window.location.href.endsWith("MakeNewPanel")) {
    const defaultPanelButton = document.getElementById("DefaultPanelBtn")
    if (defaultPanelButton) {
        defaultPanelButton.addEventListener("click", defaultPanel)
    }
}


if(window.location.href.includes('PanelManagement/Index/')){
    const viewCodesButton = document.getElementById("viewCodesButton")
    const generateAllQrCodes = document.getElementById("generateAllQrCodesButton");
    if(viewCodesButton){
        viewCodesButton.addEventListener("click", fetchUniqueCodes)
    }
}