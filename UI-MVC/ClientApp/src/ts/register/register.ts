import {Modal} from "bootstrap";

console.log('The \'Register\' bundle has been loaded!');

document.addEventListener("DOMContentLoaded", () => {
    const modalElement = document.getElementById("dataModal");
    const shouldShowModal = modalElement?.getAttribute("data-show-hasAnswered") == "False";
    
    if(shouldShowModal && modalElement){
        const bootstrapModal = new Modal(modalElement);
        bootstrapModal.show();
    }
})