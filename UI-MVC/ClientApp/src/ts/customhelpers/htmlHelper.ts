export function createSuggestionBox(): HTMLDivElement {
    const box = document.createElement("div");
    box.className = "suggestion-box";
    box.style.background = "white";
    box.style.border = "1px solid #ccc";
    box.style.padding = "4px";
    box.style.fontSize = "0.9em";
    box.style.zIndex = "1000";
    box.style.display = "none";
    return box;
}

export function wrapElementWithBootstrapRow(element: HTMLElement): HTMLDivElement{
    const row = document.createElement("div");
    row.className="row";
    row.appendChild(element);
    return row;
}

export function createElementWithClassNames<K extends keyof HTMLElementTagNameMap>(tagName:K, ...classNames: string[]):HTMLElementTagNameMap[K]{
    const element = document.createElement(tagName)
    element.classList.add(...classNames);
    return element;
}