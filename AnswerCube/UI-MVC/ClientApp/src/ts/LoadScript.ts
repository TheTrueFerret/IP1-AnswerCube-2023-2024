

document.addEventListener("DOMContentLoaded", function() {
    loadScriptsForCurrentPage();
});

// Select the target node
var targetNode = document.getElementById('page');

// Options for the observer (which mutations to observe)
var config = { childList: true, subtree: true };

// Callback function to execute when mutations are observed
var callback = function(mutationsList, observer) {
    for(var mutation of mutationsList) {
        if (mutation.type === 'childList') {
            // Check if innerHTML has changed
            loadScriptsForCurrentPage();
        }
    }
};

// Create an observer instance linked to the callback function
var observer = new MutationObserver(callback);

function loadScriptsForCurrentPage() {
    var views = document.querySelectorAll("[data-script]");
    views.forEach(function(view) {
        var scriptUrl = view.getAttribute("data-script");
        loadScript(scriptUrl);
    });
}

function loadScript(scriptUrl) {
    var script = document.createElement('script');
    script.src = scriptUrl;
    script.defer = true;
    document.body.appendChild(script);
    observer.disconnect();
}

document.getElementById("nextSlide").addEventListener("click", function() {
    connectObserver()
});

function connectObserver() {
    // Start observing the target node for configured mutations
    observer.observe(targetNode, config);
}