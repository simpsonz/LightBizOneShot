function doSetStorage(c_name, value) {//ex : doSetStorage('변수명','변수값');
    if (!!window.localStorage == true) {
        localStorage.setItem(c_name, value);
    } else {
        var exdate = new Date();
        exdate.setDate(exdate.getDate() + 7);
        document.cookie = c_name + "=" + escape(value) + "; expires=" + exdate.toUTCString();
        return c_name + "=" + c_value;
    }
}

function doGetStorage(c_name) {//ex : doGetStorage('변수명');
    if (!!window.localStorage == true) {
        return localStorage.getItem(c_name);
    } else {
        //return "error : storage not working";
        var c_value = document.cookie;
        var c_start = c_value.indexOf(" " + c_name + "=");
        if (c_start == -1) {
            c_start = c_value.indexOf(c_name + "=");
        }
        if (c_start == -1) {
            c_value = null;
        }
        else {
            c_start = c_value.indexOf("=", c_start) + 1;
            var c_end = c_value.indexOf(";", c_start);
            if (c_end == -1) {
                c_end = c_value.length;
            }
            c_value = unescape(c_value.substring(c_start, c_end));
        }
        return c_value;

    }
}

function doRemoveStorage(c_name) {//해당 변수를 완전히 제거 ex : doRemoveStorage('변수명');
    if (!!window.localStorage == true) {
        localStorage.removeItem(c_name);
    } else {
        var expireDate = new Date();
        //어제 날짜를 쿠키 소멸 날짜로 설정한다.
        expireDate.setDate(expireDate.getDate() - 1);
        document.cookie = c_name + "= " + "; expires=" + expireDate.toGMTString() + "; path=/";

    }
}

function doClearStorage() {// 모든 localStorage 제거 ex : localStorage();
    if (!!window.localStorage == true) {
        localStorage.clear();
    } else {
        var cookies = document.cookie.split(";");
        for (var i = 0; i < cookies.length; i++) {
            var cookie = cookies[i];
            var eqPos = cookie.indexOf("=");
            var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
            document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT";
        }
    }
}
