import {Modal} from "bootstrap";

console.log('The \'Register\' bundle has been loaded!');

document.addEventListener("DOMContentLoaded", () => {
    const modalElement = document.getElementById("dataModal");
    const shouldShowModal = modalElement?.getAttribute("data-show-hasAnswered") == "False";
    
    if(shouldShowModal && modalElement){
        const bootstrapModal = new Modal(modalElement);
        bootstrapModal.show();
        addEventHandlersToForm();
    }
})

function addEventHandlersToForm(){
    const steps = document.querySelectorAll(".form-step");
    const nextButtons = document.querySelectorAll(".next-step");
    const prevButtons = document.querySelectorAll(".prev-step");

    let currentStep = 0;
    function showStep(step: number) {
        steps.forEach((stepDiv, index) => {
            stepDiv.classList.toggle("active", index === step);
        });
    }

    nextButtons.forEach(button => {
        button.addEventListener("click", () => {
            if (currentStep < steps.length - 1) {
                currentStep++;
                showStep(currentStep);
            }
        });
    });

    prevButtons.forEach(button => {
        button.addEventListener("click", () => {
            if (currentStep > 0) {
                currentStep--;
                showStep(currentStep);
            }
        });
    });

    showStep(currentStep);
}