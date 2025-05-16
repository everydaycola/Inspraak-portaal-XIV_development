type ValidationMessages = Partial<Record< 'valueMissing' |
    'tooShort' | 'tooLong' | 'rangeUnderflow' |
    'rangeOverflow' | 'typeMismatch' | 'patternMismatch',
    string>>;

export function setValidationMessages(ctrlID: string, msgEleID: string,
    messages: ValidationMessages) {
    let element = document.getElementById(ctrlID) as HTMLInputElement
    element.addEventListener("focusout", () => {

        let flag: boolean = false;

        if (element.validity.valueMissing) {
            if (typeof messages.valueMissing !== "undefined") {
                element.setCustomValidity(messages.valueMissing);
                flag = true;
            }
        }
        if (element.validity.tooShort) {
            if (typeof messages.tooShort !== "undefined") {
                element.setCustomValidity(messages.tooShort);
                flag = true;
            }
        }

        if (element.validity.tooLong) {
            if (typeof messages.tooLong !== "undefined") {
                element.setCustomValidity(messages.tooLong);
                flag = true;
            }
        }

        if (element.validity.rangeUnderflow) {
            if (typeof messages.rangeUnderflow !== "undefined") {
                element.setCustomValidity(messages.rangeUnderflow);
                flag = true;
            }
        }

        if (element.validity.rangeOverflow) {
            if (typeof messages.rangeOverflow !== "undefined") {
                element.setCustomValidity(messages.rangeOverflow);
                flag = true;
            }
        }


        if (element.validity.patternMismatch) {
            if (typeof messages.patternMismatch !== "undefined") {
                element.setCustomValidity(messages.patternMismatch);
                flag = true;
            }
        }

        if (element.validity.typeMismatch) {
            if (typeof messages.typeMismatch !== "undefined") {
                element.setCustomValidity(messages.typeMismatch);
                flag = true;
            }
        }

        if (flag) {
            (document.querySelector("#" + msgEleID) as HTMLElement).innerHTML = element.validationMessage;
        }
        else {
            element.setCustomValidity("");
            (document.querySelector("#" + msgEleID) as HTMLElement).innerHTML = "";
        }
    });
}

export function addSubregionValidation(nameInput: HTMLInputElement, nameError: HTMLSpanElement, sizeInput: HTMLInputElement, sizeError: HTMLSpanElement) {
    //Name
    if (!nameInput.required){
        nameInput.required = true;
    }
    setValidationMessages(nameInput.id, nameError.id, {
        valueMissing: "De naam van een (deel)gemeente of wijk is verplicht."
    })
    //Size
    if (!sizeInput.required) {
        sizeInput.required = true;
    }
    setValidationMessages(sizeInput.id, sizeError.id, {
        valueMissing: "De grootte van een (deel)gemeente of wijk is verplicht."
    })
}

export function validateCriteriaPercentages(): boolean {
    let isValid = true;

    const criteriaBlocks = document.querySelectorAll(".criteria");

    criteriaBlocks.forEach((criteriaDiv, index) => {
        const distKnownCheck = criteriaDiv.querySelector(`input[name$="IsDistributionKnown"]`) as HTMLInputElement
        const defaultCheck = criteriaDiv.querySelector(`input[name$="IsDefault"]`) as HTMLInputElement
        const error = document.createElement("div");
        if (defaultCheck.checked && !distKnownCheck.checked) {
            isValid = false

            error.className = "text-danger distribution-error";
            error.innerText = `Een standaard criteria moet een verdeling hebben.`;
        } else if (distKnownCheck.checked) {
            const inputs = criteriaDiv.querySelectorAll<HTMLInputElement>(
                `input[name^="Distributions[${index}].AnswerOptions"][name$="DistributionPercentage"]`
            );

            const sum = Array.from(inputs)
                .map(input => parseFloat(input.value) || 0)
                .reduce((acc, val) => acc + val, 0);

            // Remove existing message if any
            let message = criteriaDiv.querySelector(".distribution-error");
            if (message) message.remove();

            if (Math.abs(sum) != 100) {
                isValid = false;

                error.className = "text-danger distribution-error";
                error.innerText = `De verdeling van de antwoord opties moet 100% zijn. Nu: ${sum}%`;
            }
        }
        // Place below answer list
        const ul = criteriaDiv.querySelector("ul.list-group");
        ul?.after(error);
    });

    return isValid;
}

