import {createElementWithClassNames} from "./customHelpers/htmlHelper";

export function createRemoveBtn(removalFunction: (() => void) | ((event: MouseEvent) => void)): HTMLButtonElement {
    const removeBtn = createElementWithClassNames("button", "btn", "btn-danger", "btn-sm") as HTMLButtonElement;
    removeBtn.type = "button";
    removeBtn.innerHTML = `<i class="bi-trash"></i>`;
    removeBtn.addEventListener("click", (event) => {
        if (removalFunction.length > 0) {
            (removalFunction as (event: MouseEvent) => void)(event);
        } else {
            (removalFunction as () => void)();
        }
    });
    return removeBtn;
}