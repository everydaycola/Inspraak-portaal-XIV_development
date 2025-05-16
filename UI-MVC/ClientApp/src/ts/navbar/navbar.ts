
export function setupNavBarInteraction() {
    const expandNavbarButton: HTMLAnchorElement = document.querySelector(".expand-navbar")!!;
    const nav: HTMLElement = document.querySelector("nav")!!
    const largeContainer: HTMLElement = document.querySelector(".large-grid-container")!!
    const navLinks: NodeListOf<HTMLElement> | null = document.querySelectorAll(".nav-item")
    let isOpened: Boolean = false;
    expandNavbarButton.addEventListener("click", (event: MouseEvent) => {
        isOpened = !isOpened;
        if (isOpened) {
            expandNavbar(nav, largeContainer, navLinks, expandNavbarButton)
        } else {
            collapseNavbar(nav, largeContainer, navLinks, expandNavbarButton)
        }
    })
}

function expandNavbar(
    nav: HTMLElement,
    largeContainer: HTMLElement,
    navLinks: NodeListOf<HTMLElement>,
    expandNavbarButton: HTMLAnchorElement
) {
    largeContainer.classList.remove("closeGridAnimation");
    largeContainer.classList.add("enlargeGridAnimation");
    const onAnimationEnd = () => {
        largeContainer.classList.add("opened-large-grid-container");
        largeContainer.classList.remove("enlargeGridAnimation");

        if (navLinks) {
            navLinks.forEach(link => {
                link.querySelector(".nav-link-description")?.classList.remove("l-invisible");
            });
        }
        expandNavbarButton.innerText = "<";

        largeContainer.removeEventListener("animationend", onAnimationEnd);
    };

    largeContainer.addEventListener("animationend", onAnimationEnd);
}

function collapseNavbar(
    nav: HTMLElement,
    largeContainer: HTMLElement,
    navLinks: NodeListOf<HTMLElement>,
    expandNavbarButton: HTMLAnchorElement
) {
    largeContainer.classList.remove("closeGridAnimation");
    largeContainer.classList.remove("enlargeGridAnimation");
    largeContainer.classList.add("closeGridAnimation");
    largeContainer.classList.remove("opened-large-grid-container");
    const onAnimationEnd = () => {
        largeContainer.classList.remove("closeGridAnimation");
        
        if (navLinks) {
            navLinks.forEach(link => {
                link.querySelector(".nav-link-description")?.classList.add("l-invisible");
            });
        }
        expandNavbarButton.innerText = ">";

        largeContainer.removeEventListener("animationend", onAnimationEnd);
    };

    largeContainer.addEventListener("animationend", onAnimationEnd);
}