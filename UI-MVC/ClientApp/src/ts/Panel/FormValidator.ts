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