const dom = {
    'getValue': function(id) {
        let element = document.getElementById(id);
        if (element === null) {
            console.log(id);
            return null;
        }
        return element.value;
    },
    'setValue': function(id, value) {
        let element = document.getElementById(id);
        if (element === null) {
            console.log(id);
        } else {
            if (element.nodeName === "BUTTON") {
                element.innerText = value;
            } else {
                element.value = value;
            }
        }
    },
    'setError': function(msg) {
        const element = document.getElementById("errMsg");
        element.innerText = msg;
        element.style.display = "block";
    },
    'removeError': function() {
        const element = document.getElementById("errMsg");
        element.innerText = "";
        element.style.display = "none";
    },
    'remove': function(id, parent) {
        let element = document.getElementById(id);
        if (parent) {
            element.parentNode.style.display = "none";
        }
        element.style.display = "none";
    },
    'show': function(id, parent) {
        let element = document.getElementById(id);
        if (parent) {
            element.parentNode.style.display = "block";
        }
        element.style.display = "block";
    },
    'disable': function(id) {
        let element = document.getElementById(id);
        element.disabled = true;
    },
    'enable': function(id) {
        let element = document.getElementById(id);
        element.disabled = false;
    }

};
let urlParams;
(window.onpopstate = function() {
    let match,
        pl = /\+/g,
        search = /([^&=]+)=?([^&]*)/g,
        decode = function(s) { return decodeURIComponent(s.replace(pl, " ")); },
        query = window.location.search.substring(1);

    urlParams = {};
    while (match = search.exec(query))
        urlParams[decode(match[1])] = decode(match[2]);
})();