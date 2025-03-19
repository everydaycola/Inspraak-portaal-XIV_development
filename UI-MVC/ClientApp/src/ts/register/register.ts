import {Modal} from "bootstrap";

console.log('The \'Register\' bundle has been loaded!');

document.addEventListener("DOMContentLoaded", () => {
    const modalElement = document.getElementById("dataModal");
    const shouldShowModal = modalElement?.getAttribute("data-show-hasAnswered") == "False";
    
    if(shouldShowModal && modalElement){
        const bootstrapModal = new Modal(modalElement);
        bootstrapModal.show();
        addEventHandlersToForm();
        visualiseNextButtonsWhenQuestionIsAnswered();
    }
})
function visualiseNextButtonsWhenQuestionIsAnswered(){
    const formSteps = document.querySelectorAll('.form-step');
    formSteps.forEach((step, index) => {
        if (index === 0) { 
            const emailInput = step.querySelector('input[type="email"]') as HTMLInputElement;
            const nextButton = step.querySelector('.next-step') as HTMLElement;
            if (nextButton) {
                nextButton.style.display = 'none';
                
                emailInput.addEventListener('input', function() {
                    if (emailInput.value.trim() !== '' && emailInput.value.includes("@")) {
                        nextButton.style.display = 'initial';
                        nextButton.classList.add('cstm-btn-anim')
                    } else {
                        nextButton.style.display = 'none';
                        nextButton.classList.remove('cstm-btn-anim')
                    }
                });
            }
        }
        if (index > 0) { 
            const radioButtons = step.querySelectorAll('input[type="radio"]');
            const nextButton = step.querySelector('.next-step') as HTMLElement;
            nextButton.style.display = 'none';

            radioButtons.forEach(radio => {
                const radioInput = radio as HTMLInputElement;
                radioInput.addEventListener('change', function() {
                    const inputName = radioInput.name;
                    toggleNextButton(inputName, nextButton);
                });
            });
        }
    });
}

function toggleNextButton(inputName: string, nextButton: HTMLElement) {
    const input = document.querySelector(`input[name="${inputName}"]:checked`);
    if (input) {
        nextButton.style.display = 'initial';
        nextButton.classList.add('cstm-btn-anim')
    } else {
        nextButton.style.display = 'none';
        nextButton.classList.remove('cstm-btn-anim')
    }
}

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