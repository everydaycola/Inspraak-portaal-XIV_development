export function initShareHandler() {
    const copyLinkButtons: NodeListOf<HTMLButtonElement> = document.querySelectorAll('.copy-link-btn');
    copyLinkButtons.forEach((button: HTMLButtonElement): void => {
        button.addEventListener('click', function (this: HTMLButtonElement): void {
            const linkToCopy: string | null = this.getAttribute('data-clipboard-text');
            const originalText: string | null = this.textContent;

            if (linkToCopy) {
                navigator.clipboard.writeText(linkToCopy)
                    .then((): void => {
                        this.textContent = 'Gekopieerd!';
                        setTimeout((): void => {
                            this.textContent = originalText;
                        }, 1500);
                    })
                    .catch((err: unknown): void => {
                        console.error('Fout bij het kopiëren van de link: ', err);
                        alert('De link kon niet automatisch gekopieerd worden. Kopieer hem handmatig: ' + linkToCopy);
                    });
            }
        });
    });
}