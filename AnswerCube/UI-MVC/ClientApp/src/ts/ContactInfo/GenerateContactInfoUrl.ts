let url = window.location.origin + "/ContactInfo/ContactInfo";

addEventListener("DOMContentLoaded", () => {
    let qrDiv = document.getElementById("qrCodeData") as HTMLDivElement;
    // @ts-ignore
    qrDiv.setAttribute("data-url", url)
});