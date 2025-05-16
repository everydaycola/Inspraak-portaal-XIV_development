import {round} from "@popperjs/core/lib/utils/math";
import {setValidationMessages} from "./panelFormValidator";
import {createElementWithClassNames} from "../../helpers/htmlHelper";
import {createRemoveBtn} from "../../components";

export let answerOptionCounters: Map<number, number> = new Map()

export function createPercentageInput(criteriaId: number, answerCount: number) {
    const answerOptionPercentageInput = createElementWithClassNames("input","border","border-1","rounded-2","me-2","answer-option-distribution");
    answerOptionPercentageInput.id = `answer-option-${criteriaId}-${answerCount}-percentage`
    answerOptionPercentageInput.name = `Distributions[${criteriaId}].AnswerOptions[${answerCount}].DistributionPercentage`;
    answerOptionPercentageInput.type = "number";
    answerOptionPercentageInput.step = "0.01";
    answerOptionPercentageInput.placeholder = `${round(100 / (answerCount + 1))}`
    // Validation
    answerOptionPercentageInput.required = true;
    answerOptionPercentageInput.min = "0";
    answerOptionPercentageInput.max = "100";

    return answerOptionPercentageInput;
}

export function addAnswerOption(criteriaId: number, answersContainer: HTMLUListElement, isDistributionKnown: boolean) {
    const answerCount = answerOptionCounters.get(criteriaId) ?? 0;
    answerOptionCounters.set(criteriaId, answerCount + 1);

    const answerOptionId = `option-${criteriaId}-${answerCount}`

    // Create answerOption li
    const answerOptionLi = createElementWithClassNames("li","list-group-item","d-flex","flex-row","py-2");
    answerOptionLi.id = answerOptionId

    // Create answerOption input
    const answerOptionInput = createElementWithClassNames("input","border","border-1","rounded-2","me-2","answer-option");
    answerOptionInput.id = `answer-option-${criteriaId}-${answerCount}`
    answerOptionInput.name = `Distributions[${criteriaId}].AnswerOptions[${answerCount}].Option`;
    answerOptionInput.type = "text";
    answerOptionInput.placeholder = `Antwoord ${(answerCount + 1)}`

    // Validation span for option
    const answerOptionSpan = createElementWithClassNames("span", "text-danger","field-validation-valid","ps-1");
    answerOptionSpan.id = `${answerOptionInput.id}-msg`
    
    // Create Delete Button
    const removeBtn = createRemoveBtn(() => removeAnswerOption(answerOptionId, criteriaId, answersContainer));
    if (isDistributionKnown) {
        // Create answerOptionPercentage input
        const answerOptionPercentageInput = createPercentageInput(criteriaId, answerCount);

        // Validation span for percentage
        const answerOptionPercentageSpan = createElementWithClassNames("span","text-danger","field-validation-valid","ps-1");
        answerOptionPercentageSpan.id = `${answerOptionPercentageInput.id}-msg`

        answerOptionLi.append(answerOptionInput, answerOptionPercentageInput, removeBtn, answerOptionSpan, answerOptionPercentageSpan);
        answersContainer.append(answerOptionLi);
        // Validation messages
        setValidationMessages(answerOptionPercentageInput.id,answerOptionPercentageSpan.id,{
            valueMissing: "Antwoord optie moet een verdeling waarde hebben.",
            rangeUnderflow: "Percentage moet tussen 0 en 100% zijn.",
            rangeOverflow : "Percentage moet tussen 0 en 100% zijn."
        })
    } else {
        answerOptionLi.append(answerOptionInput, removeBtn, answerOptionSpan);
        answersContainer.append(answerOptionLi);
    }
    
    //Validation
    // Answer Option text
    answerOptionInput.required = true;
    answerOptionInput.minLength = 2;
    answerOptionInput.maxLength = 20;
    setValidationMessages(answerOptionInput.id,answerOptionSpan.id,{
        valueMissing: "Antwoord optie moet een naam hebben.",
        tooShort: "Antwoord optie moet minstens 2 characters lang zijn.",
        tooLong: "Antwoord optie mag maximum maar 20 characters lang zijn."
    })
    

    // Counter ++
    answerOptionCounters.set(criteriaId, answerCount + 1);
}

function removeAnswerOption(id: string, criteriaId: number, answersContainer: HTMLUListElement) {
    let e = event as Event
    const answerOptionLi = (e.currentTarget as HTMLButtonElement).parentElement as HTMLLIElement
    answerOptionLi.remove();
    reIndexAnswerOptions(criteriaId, answersContainer);
}

export function reIndexAnswerOptions(criteriaId: number, answersContainer: HTMLUListElement) {
    const answerLis = answersContainer.querySelectorAll<HTMLLIElement>("li");

    // Update the counter
    answerOptionCounters.set(criteriaId, answerLis.length);

    answerLis.forEach((li, index) => {
        const textInput = li.querySelector<HTMLInputElement>("input[type='text']") as HTMLInputElement;
        const percentInput = li.querySelector<HTMLInputElement>("input[type='number']") as HTMLInputElement;

        li.id = `option-${criteriaId}-${index}`;

        textInput.name = `Distributions[${criteriaId}].AnswerOptions[${index}].Option`;
        textInput.placeholder = `Antwoord ${index + 1}`;

        percentInput.name = `Distributions[${criteriaId}].AnswerOptions[${index}].DistributionPercentage`;
    });
}

function resetAnswerCounter(criteriaId: number) {
    answerOptionCounters.set(criteriaId, 0)
}

export function resetAnswerCounters(criteriaCount: number) {
    answerOptionCounters.forEach((value, key) => {
        if (key >= criteriaCount) {
            resetAnswerCounter(key)
        }
    })
}
