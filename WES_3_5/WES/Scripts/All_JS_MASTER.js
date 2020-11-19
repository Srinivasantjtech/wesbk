
function Hidepopup() {
    $find('testTACpopup').hide();
    //        document.getElementById('scrollbar').style.display = 'block';
    //        document.body.style.overflow = 'hidden';
    //        $("body").css('overflow', 'hidden');
}

function Showpopup() {
    $find('testTACpopup').hide();
}
function capLock(e) {
    kc = e.keyCode ? e.keyCode : e.which;
    sk = e.shiftKey ? e.shiftKey : ((kc == 16) ? true : false);
    if (((kc >= 65 && kc <= 90) && !sk) || ((kc >= 97 && kc <= 122) && sk))
        document.getElementById('divMayus').style.visibility = 'visible';
    else
        document.getElementById('divMayus').style.visibility = 'hidden';
}



function blockspecialcharacters(e) {
    var key = window.event ? e.keyCode : e.which;
    var keychar = String.fromCharCode(key);
    //var reg = new RegExp("[0-9.]")
    // var reg = new RegExp("[!`@#$%^&*()+=-[]\\\';,./{}|\":<>?~_]")
    var reg = new RegExp("[0-9.a-z.-]")
    //           if (key == 8) {
    //               keychar = String.fromCharCode(key);
    //           }
    //          
    //           if (key == 13) {
    //              // key = 8;
    //               keychar = String.fromCharCode(key);
    //           }
    return reg.test(keychar);
}
function blockspecialcharforOrder(e) {
    var key = window.event ? e.keyCode : e.which;
    var keychar = String.fromCharCode(key);
    //var reg = new RegExp("[0-9.]")
    // var reg = new RegExp("[!`@#$%^&*()+=-[]\\\';,./{}|\":<>?~_]")
    var reg = new RegExp("[0-9.a-z./.\]")
    //           if (key == 8) {
    //               keychar = String.fromCharCode(key);
    //           }
    //          
    //           if (key == 13) {
    //              // key = 8;
    //               keychar = String.fromCharCode(key);
    //           }
    return reg.test(keychar);
}



function check(e) {
    var keynum
    var keychar
    var numcheck
    if (window.event) {
        keynum = e.keyCode
    }
    else if (e.which) {
        keynum = e.which
    }
    keychar = String.fromCharCode(keynum)
    // if (keychar == "@" || keychar == "!" ||  keychar == "#" || keychar == "$" || keychar == "%" || keychar == "*" || keychar == "&" || keychar == "^" || keychar == "(" || keychar == ")" || keychar == "+" || keychar == "=" ){
    if (keychar == "~" || keychar == "`" || keychar == "!" || keychar == "@" || keychar == "#" || keychar == "$" || keychar == "%" || keychar == "^" || keychar == "&" || keychar == "*" || keychar == "(" || keychar == ")" || keychar == "+" || keychar == "=" || keychar == "," || keychar == "<" || keychar == ">" || keychar == "?" || keychar == ";" || keychar == ":" || keychar == "{" || keychar == "}" || keychar == "[" || keychar == "]" || keychar == "|" || keychar == "'" || keychar == "/" || keychar == ".") {
        return false;
    }
    else {
        return true;
    }
}
function checkUserName(e) {
    var keynum
    var keychar
    var numcheck
    if (window.event) {
        keynum = e.keyCode
    }
    else if (e.which) {
        keynum = e.which
    }
    keychar = String.fromCharCode(keynum)
    // if (keychar == "@" || keychar == "!" ||  keychar == "#" || keychar == "$" || keychar == "%" || keychar == "*" || keychar == "&" || keychar == "^" || keychar == "(" || keychar == ")" || keychar == "+" || keychar == "=" ){
    if (keychar == "~" || keychar == "`" || keychar == "!" || keychar == "#" || keychar == "$" || keychar == "%" || keychar == "^" || keychar == "&" || keychar == "*" || keychar == "(" || keychar == ")" || keychar == "+" || keychar == "=" || keychar == "," || keychar == "<" || keychar == ">" || keychar == "?" || keychar == ";" || keychar == ":" || keychar == "{" || keychar == "}" || keychar == "[" || keychar == "]" || keychar == "|" || keychar == "'" || keychar == "/") {
        return false;
    }
    else {
        return true;
    }
}



function restrictUndo(e) {
    var e = window.event;
    var keycode = e.keyCode;
    if (e.ctrlKey == true && keycode == 90) {
        e.returnValue = false;
        e.keyCode = 0;
    }
}

function iekeyDown(e) {
    var e = window.event;
    var keycode = e.keyCode;
    if (e.ctrlKey == true && keycode == 90) {
        e.returnValue = false;
        e.keyCode = 0;
    }
}



// fgt unme


function Email(e) {
    var keynum;
    var keychar;
    var numcheck;
    if (window.event) {

        keynum = e.keyCode;
    }

    else if (e.which) {
        keynum = e.which;
    }

    keychar = String.fromCharCode(keynum);
    if (keychar == "~" || keychar == "`" || keychar == "!" || keychar == "#" || keychar == "$" || keychar == "%" || keychar == "^" || keychar == "&" || keychar == "*" || keychar == "(" || keychar == ")" || keychar == "+" || keychar == "=" || keychar == "," || keychar == "<" || keychar == ">" || keychar == "?" || keychar == ";" || keychar == ":" || keychar == "{" || keychar == "}" || keychar == "[" || keychar == "]" || keychar == "|" || keychar == "'" || keychar == "/") {
        e.keyCode = '';
        return false;
    }
    else {
        return true;

    }
}




function capLock(e) {
    kc = e.keyCode ? e.keyCode : e.which;
    sk = e.shiftKey ? e.shiftKey : ((kc == 16) ? true : false);
    if (((kc >= 65 && kc <= 90) && !sk) || ((kc >= 97 && kc <= 122) && sk))
        document.getElementById('divMayus').style.visibility = 'visible';
    else
        document.getElementById('divMayus').style.visibility = 'hidden';
}
//top


var sessionTimeout;
var sessionstate = "True";
var countflag = 0;

function DisplaySessionTimeout() {
    //assigning minutes left to session timeout to Label
    sessionTimeout = sessionTimeout - 1;
    //if session is not less than 0
    if (sessionTimeout >= 0) {
        //call the function again after 1 minute delay
        window.setTimeout("DisplaySessionTimeout()", 60000);
        sessionstate = "True";
    }
    else {
        //show message box                
        sessionstate = "False";
        //Session["USER_NAME"]="";
        //window.location.href="Login.aspx"
    }
}



function Zoom(ImgFilePath) {

    if (sessionstate == "True") {
        if (ImgFilePath != "prodimages") {
            sessionTimeout = "<%= Session.Timeout %>";
            var is_URL = "Zoom.aspx?ImgUrl=" + ImgFilePath.replace(/&/g, "%26").replace("\\", "%26");
            var is_Features = 'left=300,Top=200,resizable=yes,scrollbars=yes,width=500,height=500';
            var is_win = window.open(is_URL, 'WES', is_Features);
            is_win.focus();
            DisplaySessionTimeout();
        }
    }
    else {
        count = count + 1;
        if (count > 1) {
            window.location.href = "Login.aspx";
        }
    }

}




// mainmaster
function MM_swapImgRestore() { //v3.0
    var i, x, a = document.MM_sr; for (i = 0; a && i < a.length && (x = a[i]) && x.oSrc; i++) x.src = x.oSrc;
}
function MM_preloadImages() { //v3.0
    var d = document; if (d.images) {
        if (!d.MM_p) d.MM_p = new Array();
        var i, j = d.MM_p.length, a = MM_preloadImages.arguments; for (i = 0; i < a.length; i++)
            if (a[i].indexOf("#") != 0) { d.MM_p[j] = new Image; d.MM_p[j++].src = a[i]; }
    }
}

function MM_findObj(n, d) { //v4.01
    var p, i, x; if (!d) d = document; if ((p = n.indexOf("?")) > 0 && parent.frames.length) {
        d = parent.frames[n.substring(p + 1)].document; n = n.substring(0, p);
    }
    if (!(x = d[n]) && d.all) x = d.all[n]; for (i = 0; !x && i < d.forms.length; i++) x = d.forms[i][n];
    for (i = 0; !x && d.layers && i < d.layers.length; i++) x = MM_findObj(n, d.layers[i].document);
    if (!x && d.getElementById) x = d.getElementById(n); return x;
}

function MM_swapImage() { //v3.0
    var i, j = 0, x, a = MM_swapImage.arguments; document.MM_sr = new Array; for (i = 0; i < (a.length - 2); i += 3)
        if ((x = MM_findObj(a[i])) != null) { document.MM_sr[j++] = x; if (!x.oSrc) x.oSrc = x.src; x.src = a[i + 2]; }
}



//ship


function textboxMultilineMaxNumber(txt, maxLen) {
    try {
        if (txt.value.length > (maxLen - 1)) return false;
    } catch (e) {
    }
}
function checkMaxLength(textBox, e, length) {

    var mLen = textBox["MaxLength"];
    if (null == mLen)
        mLen = length;

    var maxLength = parseInt(mLen);
    if (!checkSpecialKeys(e)) {
        if (textBox.value.length > maxLength - 1) {
            if (window.event)//IE
            {
                e.returnValue = false;
                return false;
            }
            else//Firefox
                e.preventDefault();
        }
    }
}

function checkSpecialKeys(e) {
    if (e.keyCode !=9 && e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 35 && e.keyCode != 36 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
        return false;
    else
        return true;
}


//catelist

function dsp(loc) {
    if (document.getElementById) {
        var foc = loc.firstChild;
        foc = loc.firstChild.innerHTML ?
         loc.firstChild :
         loc.firstChild.nextSibling;
      
        foc.innerHTML = foc.innerHTML == '+ Show more options' ? '- Show less options' : '+ Show more options';
        foc = loc.parentNode.nextSibling.style ?
         loc.parentNode.nextSibling :
         loc.parentNode.nextSibling.nextSibling;
        foc.style.display = foc.style.display == 'block' ? 'none' : 'block';
    }
}

if (!document.getElementById)
    document.write('<style type="text/css"><!--\n' +
      '.dspcont{display:block;}\n' +
      '//--></style>');


$(document).ready(function () {

    $(".submenuhide").hide();
    $(".show_hide").show();
});
function showsubmenu(id) {
    $("#submenuhide" + id).slideToggle();
}



//familymaster

//function MM_swapImgRestore() { //v3.0
//    var i, x, a = document.MM_sr; for (i = 0; a && i < a.length && (x = a[i]) && x.oSrc; i++) x.src = x.oSrc;
//}
//function MM_preloadImages() { //v3.0
//    var d = document; if (d.images) {
//        if (!d.MM_p) d.MM_p = new Array();
//        var i, j = d.MM_p.length, a = MM_preloadImages.arguments; for (i = 0; i < a.length; i++)
//            if (a[i].indexOf("#") != 0) { d.MM_p[j] = new Image; d.MM_p[j++].src = a[i]; }
//    }
//}

//function MM_findObj(n, d) { //v4.01
//    var p, i, x; if (!d) d = document; if ((p = n.indexOf("?")) > 0 && parent.frames.length) {
//        d = parent.frames[n.substring(p + 1)].document; n = n.substring(0, p);
//    }
//    if (!(x = d[n]) && d.all) x = d.all[n]; for (i = 0; !x && i < d.forms.length; i++) x = d.forms[i][n];
//    for (i = 0; !x && d.layers && i < d.layers.length; i++) x = MM_findObj(n, d.layers[i].document);
//    if (!x && d.getElementById) x = d.getElementById(n); return x;
//}

//function MM_swapImage() { //v3.0
//    var i, j = 0, x, a = MM_swapImage.arguments; document.MM_sr = new Array; for (i = 0; i < (a.length - 2); i += 3)
//        if ((x = MM_findObj(a[i])) != null) { document.MM_sr[j++] = x; if (!x.oSrc) x.oSrc = x.src; x.src = a[i + 2]; }
//}


//catelist

function categoryclick(catid) {
    var myattr = new Array();
    if (catid.toString().indexOf(",") >= 0) {
        myattr = catid.split(",");
        var i = 0;
        for (i = 0; i < myattr.length; i++) {
            if (i != (myattr.length - 1)) {
                document.getElementById(myattr[i]).style.display = "block";
            }
            else
                document.getElementById(myattr[i]).style.display = "none";
        }
    }
    else {
        document.getElementById(catid).style.display = "block";
    }
}



//prodlist


function GetSelItems(field) {
    var count = 0;
    var sCategoryIds = '';
    for (var j = 0; j < document.getElementsByName(field).length; j++) {
        if (document.getElementsByName(field).item(j).checked == true) {
            sCategoryIds = sCategoryIds + document.getElementsByName(field).item(j).value + ",";
        }
    }

    sCategoryIds = sCategoryIds.substring(0, sCategoryIds.length - 1)
    if (sCategoryIds.length > 0)
        __doPostBack('CATEGORYFILTER', sCategoryIds);
    else
        alert('Please select atleast one category for filtering');
}

function GetCompareItems(field, fid) {
    var count = 0;
    var st = "";
    for (var j = 0; j < field.length; j++) {
        if (field[j].checked) {
            st += field[j].value + ",";
            count = count + 1;
        }
    }
    if (count > 1) {
        __doPostBack1('Compare', fid + "$" + st);
        return false;
    }
    else {
        alert('Please select atleast two items to compare');
    }
    return false;
}
function CheckCompareCount(field, ctlid) {
    var count = 1;
    for (var j = 0; j < field.length; j++) {
        if (field[j].checked) {
            if (count > 5) {
                document.forms[0].elements[ctlid].checked = false;
                alert('A maximum of 5 products can be compared at one time');
            }
            count = count + 1;
        }
    }
}

function Geturlqstring(st) {
    var urls = new String();
    urls = window.location.href;
    alert(urls.lastIndexOf("#"))
    if (urls.lastIndexOf("#") > 0) {
        alert(urls.substring(0, urls.lastIndexOf("#")))
    }
    else {
        __doPostBack('Compare', st);
        return false;
    }
    return false;
}


/*
function RestrictPassLenth(txtcontent) {
    if (txtcontent.length >= 15) {
        alert('Password Length should not be greater than 15');
        return false;
    }
}
*/

var pasinit;
function CheckTextPassMaxLength(textBox,e, length)
{

    var mLen = textBox["MaxLength"];
    var tsellen = 0;
    var text = document.getElementById(textBox.id);
    var t = text.value.substr(text.selectionStart, text.selectionEnd - text.selectionStart);



   
    if (t != null) {
        tsellen = t.length;
    }  
        if(null==mLen)
            mLen=length;

        var maxLength = parseInt(mLen);
        if (!checkSpecialKeys(e)) {
            if (textBox.value.length - tsellen > maxLength - 1) {
            if(window.event)//IE
            {
              alert("Password Length should not be greater than 15");
              e.returnValue = false;
              }
            else//Firefox
            {
                alert("Password Length should not be greater than 15");
                e.preventDefault();
            }
         }
    }   
}
function checkSpecialKeys(e) {
    var keynum;
    if (window.event) {

        keynum = e.keyCode;
    }

    else if (e.which) {
        keynum = e.which;
    }
 
    if(e.keyCode !=9 && e.keyCode !=8 && e.keyCode!=46 && e.keyCode!=37 && e.keyCode!=38 && e.keyCode!=39 && e.keyCode!=40)
        return false;
    else
        return true;
}

