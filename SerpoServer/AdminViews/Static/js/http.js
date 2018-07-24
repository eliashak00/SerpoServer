const AUTHHEADER = 'Authorization';
const AUTHKEY = 'auth';
const http = {
    'get': function (url, success) {
        let xhr = new XMLHttpRequest();
        xhr.open('GET', url);
        xhr.setRequestHeader(AUTHHEADER, Cookies.get(AUTHKEY));
        xhr.onreadystatechange = function () {
            let result = null;
            if (this.responseText) {
                result = JSON.parse(this.responseText);
            }
            success(result, this.status);
        };
        xhr.send();
    },
    'post': function (url, data, success, fail) {
        let xhr = new XMLHttpRequest();
        xhr.open('POST', url);
        xhr.setRequestHeader(AUTHHEADER, Cookies.get(AUTHKEY));
        xhr.onreadystatechange = function () {
            let result = null;
            if (this.responseText) {
                result = JSON.parse(this.responseText);
            }
        
            if (this.status === 200){
                success(result, this.status);
            }
            else{
                console.log(result);
                fail(result, this.status);
                return null;
            }
        };
        xhr.send(JSON.stringify(data));
    },
    'delete': function (url) {
        let xhr = new XMLHttpRequest();
        xhr.open('DELETE', url);
        xhr.setRequestHeader(AUTHHEADER, Cookies.get(AUTHKEY));
        xhr.onreadystatechange = function () {
            let result = JSON.parse(this.responseText);
            if (this.status === 200){
                return result
            }
            else{
                console.log(result);
                return null;
            }
        };
        xhr.send();
    }
};