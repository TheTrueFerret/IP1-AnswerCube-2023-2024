import {getDomainFromUrl} from "../urlDecoder";

window.addEventListener("load", function() {
    setTimeout(() => {
        window.location.href = getDomainFromUrl(window.location.toString())+ "/CircularFlow/Subthemes";
    }, 15000);
});