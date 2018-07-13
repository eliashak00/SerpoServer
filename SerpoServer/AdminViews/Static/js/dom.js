const dom = {
    'getValue': function (id) {
        let element = document.getElementById(id);
        if (element === null){
            console.log(id);
            return null;
        } 
        return element.value;
    },
    'setValue':function (id, value) {
        let element = document.getElementById(id);
        if (element === null){
            console.log(id);
        }else{
            if (element.nodeName === 'BUTTON'){
                element.innerText = value;
            } 
            else {
                element.value = value;
            }
        }
    },
    'setError': function (msg) {
        const element = document.getElementById("errMsg");
        element.innerText = msg;
        element.style.display = 'block';
    },
    'removeError': function () {
        const element = document.getElementById("errMsg");
        element.innerText = "";
        element.style.display = 'none';
    },
    'remove': function (id, parent) {
        const element = document.getElementById(id);
        if (parent) {
            element.parentNode.style.display = 'none';
        }
        element.style.display = 'none';
    },
    'show': function (id, parent) {
        const element = document.getElementById(id);
        if (parent) {
            element.parentNode.style.display = 'block';
        }
        element.style.display = 'block';
    },
    'disable': function (id) {
        const element = document.getElementById(id);
        element.disabled = true;
    },
    'enable': function (id) {
        const element = document.getElementById(id);
        element.disabled = false;
    }
    
};