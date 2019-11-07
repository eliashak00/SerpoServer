const AUTHHEADER = "Authorization";
const AUTHKEY = "auth";
let deleteUrl;
let deleteFunc;
const http = {
    'get': function(url, success, fail) {
        let xhr = new XMLHttpRequest();
        xhr.open("GET", url, false);
        xhr.setRequestHeader(AUTHHEADER, Cookies.get(AUTHKEY));
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
                dom.setError("Failed to proceed!");
                return null;
            }

        };
        xhr.send();
    },
    'post': function(url, data, success, fail) {
        let xhr = new XMLHttpRequest();
        xhr.open("POST", url);
        xhr.setRequestHeader(AUTHHEADER, Cookies.get(AUTHKEY));
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
                dom.setError("Failed to proceed!");
                return null;
            }
        };
        xhr.send(JSON.stringify(data));
    },
    'delete': function(url, func) {
        $("#masterDelete").modal("toggle");
        deleteUrl = url;
        deleteFunc = func;
        let btn = document.querySelector("#deleteConfirm");
        btn.addEventListener("click",
            function() {
                let xhr = new XMLHttpRequest();
                xhr.open("DELETE", deleteUrl, false);
                xhr.setRequestHeader(AUTHHEADER, Cookies.get(AUTHKEY));
                xhr.onreadystatechange = function() {
                    let result = JSON.parse(this.responseText);
                    if (this.status === 200) {
                        $("#masterDelete").modal("toggle");
                        deleteFunc(this.status, result);

                    } else {
                        console.log(result);
                        dom.setError("Failed to proceed!");

                    }
                    deleteUrl = null;
                    deleteFunc = null;
                    btn = null;
                };
                xhr.send();
            });

    }
};