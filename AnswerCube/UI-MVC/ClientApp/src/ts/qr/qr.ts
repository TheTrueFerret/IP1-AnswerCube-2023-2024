window.addEventListener("load", () => {
    // @ts-ignore
    const uri = document.getElementById("qrCodeData").getAttribute('data-url');
    // @ts-ignore
    new QRCode(document.getElementById("qrCode"),
        {
            text: uri,
            width: 150,
            height: 150
        });
});