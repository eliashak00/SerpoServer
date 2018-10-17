
const http = {
    'get': function(url, success, fail) {
        let xhr = new XMLHttpRequest();
        xhr.open("GET", url, false);
        xhr.onreadystatechange = function() {

            let result = null;
            if (this.responseText) {
                result = JSON.parse(this.responseText);
            }

            if (this.status === 200) {
                success(result, this.status);
            } else {
                console.log(result);
                fail(result, this.status);
                return null;
            }

        };
        xhr.send();
    },
    'post': function(url, data, success, fail) {
        let xhr = new XMLHttpRequest();
        xhr.open("POST", url);
        xhr.setRequestHeader("Content-Type", "application/json");
        xhr.onreadystatechange = function() {
            let result = null;
            if (this.responseText) {
                result = JSON.parse(this.responseText);
            }

            if (this.status === 200) {
                success(result, this.status);
            } else {
                console.log(result);
                fail(result, this.status);
                return null;
            }
        };
        xhr.send(JSON.stringify(data));
    },
    'delete': function(url, success, fail) {
        let xhr = new XMLHttpRequest();
        xhr.open("DELETE", url);
        xhr.onreadystatechange = function() {
            let result = JSON.parse(this.responseText);
            if (this.status === 200) {
                success(result, this.status);
            } else {
                console.log(result);
                fail(result, this.status);
                return null;
            }
        };
        xhr.send();
    }
};