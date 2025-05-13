export function setupNavBarHover() {
    const expandNavbarButton: HTMLAnchorElement = document.querySelector(".expand-navbar")!!;
    const nav: HTMLElement = document.querySelector("nav")!!
    const largeContainer: HTMLElement = document.querySelector(".large-grid-container")!!
    const navLinks: NodeListOf<HTMLElement> | null = document.querySelectorAll(".nav-item")
    let isOpened: Boolean = false;
    expandNavbarButton.addEventListener("click", (event: MouseEvent) => {
        isOpened = !isOpened;
        if(isOpened) {
            expandNavbar(nav, largeContainer, navLinks, expandNavbarButton)
        }else{
            collapseNavbar(nav, largeContainer, navLinks, expandNavbarButton)
        }
    })
}

function expandNavbar(nav: HTMLElement, largeContainer: HTMLElement, navLinks: NodeListOf<HTMLElement>, expandNavbarButton: HTMLAnchorElement){
    nav.classList.add("show");
    largeContainer.classList.add("opened-large-grid-container");
    if (navLinks != null) {
        navLinks.forEach(link => {
            link.querySelector(".nav-link-description")?.classList.remove("invisible")
        })
    }
    expandNavbarButton.innerText = "<"
}
function collapseNavbar(nav: HTMLElement, largeContainer: HTMLElement, navLinks: NodeListOf<HTMLElement>, expandNavbarButton: HTMLAnchorElement){
    nav.classList.remove("show");
    largeContainer.classList.remove("opened-large-grid-container");
    if (navLinks != null) {
        navLinks.forEach(link => {
            link.querySelector(".nav-link-description")?.classList.add("invisible")
        })
    }
    expandNavbarButton.innerText = ">"
}