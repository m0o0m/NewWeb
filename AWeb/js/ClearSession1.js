var xmlhttp;
function createXmlhttp() {
    if (window.XMLHttpRequest) {
        xmlhttp = new XMLHttpRequest();
    }
    else if (window.ActiveXObject) {
        try {
            xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");
        } catch (e) {
            xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
        }
    }
    return xmlhttp;

}

function ClearSession() {
    createXmlhttp();
    var queryString = '';
    var url = "Service.asmx/ClearSession";
    xmlhttp.open("POST", url, true);
    xmlhttp.onreadystatechange = handleStateChange;
    xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded;");
    xmlhttp.send(queryString);
   
}
function handleStateChange() {
    if (xmlhttp.readyState == 4) {
        if (xmlhttp.status == 200) {
            //清空成功
        }
    }
}