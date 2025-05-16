interface ShareData {
    title?: string;
    text?: string;
    url?: string;
}

document.addEventListener('DOMContentLoaded', (): void => {
    const shareButtons: NodeListOf<HTMLButtonElement> = document.querySelectorAll('.share-btn');
    shareButtons.forEach((button: HTMLButtonElement): void => {
        button.addEventListener('click', function (this: HTMLButtonElement): void {
            const url: string | null = this.getAttribute('data-url');
            const title: string | null = this.getAttribute('data-title');
            const text: string | null = title;

            if (navigator.share) {
                const shareData: ShareData = {
                    title: title ?? undefined,
                    text: text ?? undefined,
                    url: url ?? undefined
                };

                navigator.share(shareData)
                    .then((): void => console.log('Succesvol gedeeld'))
                    .catch((error: unknown): void => console.log('Fout bij delen', error));
            } else {
                alert('Web sharing API wordt niet ondersteund in deze browser. Kopieer de link handmatig.');
            }
        });
    });

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
});