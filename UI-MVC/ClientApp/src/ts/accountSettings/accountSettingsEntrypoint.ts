import QRCode from "qrcode"

const qrCodeDiv : HTMLDivElement | null = document.querySelector("#qrCode")
const qrCodeDataDiv: HTMLDivElement | null = document.querySelector("#qrCodeData")

if (qrCodeDiv && qrCodeDataDiv) {
    const authenticatorUri = qrCodeDataDiv.getAttribute("data-url")!!
    QRCode.toDataURL(authenticatorUri, { width: 180 }, (err, url) => {
        if (err) {
            console.error('Failed to generate QR code:', err);
            return;
        }
        const img = document.createElement('img');
        img.src = url;
        img.alt = 'Authenticator QR code';
        qrCodeDiv.innerHTML = '';
        qrCodeDiv.appendChild(img);
    });
}