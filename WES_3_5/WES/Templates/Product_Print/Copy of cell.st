<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
  <head id="ctl00_Head1">
    <link rel="icon" href="images/favicon.ico" type="image/x-icon" />
    <link rel="shortcut icon" href="images/favicon.ico" />
    <link rel="stylesheet" type="text/css" media="screen" href="css/AllCss.css?v=0" />
    <script src="Scripts/All_Js_Top.js?v=0" type="text/javascript"></script>
    <script src="Scripts/All_JS_MASTER.js?v=0" type="text/javascript"></script>
    <link rel="Stylesheet" href="css/thickboxAddtocart.css?v=0" type="text/css" />
    <script language="javascript" src="Scripts/thickboxaddtocart.js?v=0" type="text/javascript" />
    <script type="text/javascript">
      $(document).ready(function () {
      //Default Action
      $(".tab_content").hide();
      $("ul.tabs li:first").addClass("active").show();
      $(".tab_content:first").show();
      //On Click Event
      $("ul.tabs li").click(function () {
      $("ul.tabs li").removeClass("active");
      $(this).addClass("active");
      $(".tab_content").hide();
      var activeTab = $(this).find("a").attr("href");
      $(activeTab).fadeIn();
      return false;
      });
      });
    </script>
    <script language="javascript" type="text/javascript">
      $(function () {
      $("#gallery a").mouseover(function () {
      var mimg = $(this).children("img").attr("src");
      $("#product-image > img").attr("src", mimg.replace("_th50", "_images_200"));
      });
      });

    </script>
    <title>
      WES AUSTRALIA
    </title>
    <script type="text/javascript">

      $(document).ready(function () {
      $("#lmo").hide();
      $("#preview").toggle(function () {
      $("#div1").hide();
      $("#div2").show();
      $("#lmo").show();
      $("#smo").hide();
      }, function () {
      $("#div1").show();
      // $("#div2").hide();
      $("#div2").hide();
      $("#smo").show();
      $("#lmo").hide();
      });
      });

    </script>
  </head>

  <h3 id="ctl00_head_h3_1" class="H3Tag"></h3>
  <h3 id="ctl00_head_h3_2" class="H3Tag"></h3>
  <h3 id="ctl00_head_h3_3" class="H3Tag"></h3>
  <h2 id="ctl00_head_h2" class="H2Tag"></h2>
  <h1 id="ctl00_head_h1" class="H1Tag"></h1>


  <body class="fondo_" style="-webkit-print-color-adjust:exact;">

    <form name="aspnetForm" method="post" action="family.aspx?fid=10793&amp;cid=WES908P&amp;ProductResult=SUCCESS&amp;pcr=WES908N&amp;Path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT0T1bsnnmNMuoL7PWdQglzw" id="aspnetForm">
      <div>
        <input type="hidden" name="__VIEWSTATE" id="__VIEWSTATE" value="" />
      </div>

      <script type="text/javascript">
        //<![CDATA[
Sys.WebForms.PageRequestManager._initialize('ctl00$ScriptManager1', 'aspnetForm', [], [], [], 90, 'ctl00');
//]]>
      </script>

      <table class="back_sitio" width="990" align="center" cellpadding="0" cellspacing="0">
        <tr>
          <td height="2" valign="top" align="center">
          </td>
        </tr>
        <tr>
          <td valign="top" align="center">



            <script type='text/javascript'>





              /* function init() {
              key_count_global = 0;
              var vartime = null;

              document.getElementById("srcfield").onkeypress = function (e) {
              if (e == undefined)
              e = event;

              if (e != null) {

              key_count_global++;
              var id = document.getElementById("PSearchDiv");
              id.innerHTML = '<div >
                <img src="images/Invloading.gif"/ >
              </div>';
              id.style.display = "block";
              vartime = setTimeout("lookup(" + key_count_global + "," + e.keyCode + ")", 1000);
              }
              }
              }
              window.onload = init;

              function lookup(key_count, keyCode) {

              if (key_count == key_count_global) {

              if (keyCode != 13) {
              var SearchId = document.getElementById("srcfield")
              GetSearchProducts(SearchId)
              }
              else {
              urlredirect();
              }
              }

              }*/

            </script>
            <script>
              function GetSearchProducts(SearchId) {
              if (SearchId.value != "") {


              $.ajax({
              type: "POST",
              url: "GblWebMethods.aspx/GetSearchResultNew1",
              data: '{"Strvalue":"' + SearchId.value + '"}',
              contentType: "application/json;charset=utf-8",
              dataType: "json",
              success: OnajaxSuccess,
              error: OnajaxFailure


              });
              }
              else {
              var id = document.getElementById("PSearchDiv");
              toggleDiv(id, "none");
              }
              }



              function toggleDiv(Divid, dis) {
              Divid.style.display = dis;
              }

              function OnajaxSuccess(result) {

              var id = document.getElementById("PSearchDiv");
              if (result.d != "") {

              var sid = document.getElementById("srcfield");
              var s = result.d;
              id.innerHTML = result.d;

              toggleDiv(id, "block")
              }
              else {
              id.innerHTML = "No Result";
              toggleDiv(id, "block")
              }


              }
              function OnajaxFailure(result) {

              id.innerHTML = "Please try again later";
              }

            </script>

            <script type="text/javascript">

              function ttrim(stringToTrim) {
              return stringToTrim.replace(" ","");
              }

              function urlredirect()
              {
              var ddlattrvalue=document.getElementById('srcfield').value;
              if(ddlattrvalue!="")
              {
              if(ddlattrvalue!="Search WES! Enter Keywords or Part No's")
              {
              if(ttrim(ddlattrvalue)!="")
              {
              var i=0;
              for(i=0;i<=ddlattrvalue.length;i++)
              ddlattrvalue= ddlattrvalue.replace(/#/,"%23").replace(/&/g,"%26").replace(/ /g,"%20").replace("+","%2B").replace(/\"/g, "%22");

              window.document.location="powersearch.aspx?&srctext="+ ddlattrvalue.trim() + "";
              }
              }
              }
              }

              function FillVal(ctl)
              {

              if(ctl.value=='' || ctl.value==null || ttrim(ctl.value) == '')
              {
              ctl.value="Search WES! Enter Keywords or Part No's";

              var id = document.getElementById("PSearchDiv");
              id.style.display = "none";
              }
              }
              function Foc(ctl)
              {
              if(ctl.value=="Search WES! Enter Keywords or Part No's")
              {
              ctl.value='';
              }
              if(ctl.value!='')
              {

              var id = document.getElementById("PSearchDiv");
              //remove temp
              //id.style.display = "block";
              }
              }
              var hovered = false;
              function divMouseover(ctl)
              {
              hovered=true;
              }
              function divmouseout(ctl)
              {
              hovered=false;
              }
              function Focout(ctl)
              {
              if (!hovered) {
              var id = document.getElementById("PSearchDiv");
              id.style.display = "none";
              }


              }
              function Focout1(ctl)
              {

              var id = document.getElementById("PSearchDiv");
              id.style.display = "none";



              }

              function urlredirectK(e)
              {

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
              /* if (keychar == "@" || keychar == "!" || keychar == "~" || keychar == "`"  || keychar == "%" || keychar == "^"  || keychar == "(" || keychar == ")" || keychar == "+" || keychar == "=" || keychar == ";" || keychar == ":" || keychar == "," || keychar == "?" || keychar == "/" || keychar == "[" || keychar == "{" || keychar == "]" || keychar == "}" || keychar == "." || keychar == "*" || keychar == "\#" || keychar == "$" || keychar == "\%" )
              {
              e.keyCode = '';
              return false;
              }
              else if (e.which == 64 || e.which == 126 || e.which == 33 || e.which == 64 || e.which == 35  || e.which == 36 || e.which == 37 || e.which == 94 || e.which == 38 || e.which == 42 || e.which == 40 || e.which == 39 || e.which == 44 || e.which == 45 || e.which == 46 || e.which == 47 || e.which == 58 || e.which == 61 || e.which == 91 || e.which == 92 || e.which == 93 || e.which == 96 || e.which == 95 || e.which == 43 || e.which == 123 || e.which == 124 || e.which == 125 || e.which == 59 || e.which == 34 || e.which == 60 || e.which == 62 || e.which == 63 || e.which == 126)
              {

              return false;
              }*/

              var ddlattrvalue=document.getElementById('srcfield').value;

              if(ddlattrvalue!="")
              {
              if(ddlattrvalue!="Search WES! Enter Keywords or Part No's")
              {

              if(e.keyCode==13)
              {
              window.document.location="powersearch.aspx?&srctext=" + ddlattrvalue.replace(/#/,"%23").replace(/&/g,"%26").replace(/ /g,"%20").replace("+","%2B").replace(/\"/g, "%22");
              return false;
              }
              }
              }
              }

            </script>
            <table width="970px" height="100%"  border="0" cellpadding="0" cellspacing="0">
              <tr>
                <td width="970px" align="center" valign="top" style="font-family: Arial, Helvetica, sans-serif; font-size: 100%; color: #666;">
                  <div id="header">
                    <div class="bg1">
                      <div class="bg1inner">
                        <div class="welcommsg">Welcome, &nbsp; Wagner Electronics            , &nbsp; nathan smith &nbsp;</div>
                        <div class="toplinks">
                          <a href="OnlineCatalogue.aspx" class="toplinkatest" >Online Catalogue</a>
                          <a href="Agencies.aspx" class="toplinkatest" >Agencies</a>
                          <a href="aboutus.aspx" class="toplinkatest" >About Us</a>
                          <a href="contactus.aspx" class="toplinkatest" >Contact Us</a>
                          <a href="logout.aspx" class="toplinkatest" >Log Out</a>
                        </div>
                      </div>
                    </div >
                    <div class="bg1">
                      <div class="bg2inner">
                        <div class="logo_outer" >
                          <h1 class="brand" align="left">
                            <a href="home.aspx" class="logomerge">
                              <!--<img alt="" src="images/logo.png" />-->
                            </a>
                          </h1>
                        </div>
                        <div class="search_outer" style="float:left; margin: 30px 0 0 60px;">
                          <input name="srcfield" type="text" style="font-size:12px;font-family:arial;"  id="srcfield" class="topsearch"  value="Search WES! Enter Keywords or Part No's" onblur="FillVal(srcfield);Focout(srcfield)" onfocus="Foc(srcfield);"  onkeypress="return urlredirectK(event);"   autocomplete="off"/>
                          <a style="cursor:pointer;"  class="hoverbtn searchbtn" onclick="urlredirect()"></a>
                          <div class="searchdrop" id="PSearchDiv" style="Display:none" onmouseover="divMouseover();" onmouseout="divmouseout();" onmouseup="Focout()" >
                          </div>
                        </div>
                        <div class="user_outer">
                          <a href="BulkOrder.aspx?txtcnt=27" class="iconbtn hoverbtn ico1" ></a>
                          <a href="myaccount.aspx" class="iconbtn hoverbtn ico2" ></a>
                          <a href="CatalogueDownload.aspx?ActionResult=CATALOGUE" class="iconbtn hoverbtn ico3" ></a>
                          <div class="clear"></div>
                          <a class="hoverbtn chkout" id="cartcheckout"  href="shipping.aspx" ></a>
                          <a class="cart2 hoverbtn" id="cartviewcart" href="orderDetails.aspx?&bulkorder=1&Pid=0&ORDER_ID=101601">VIEW CART</a>
                          <a class="cart1 hoverbtn"  href="orderDetails.aspx?&bulkorder=1&Pid=0&ORDER_ID=101601">
                            <span id="cartqty">(2)</span>&nbsp;ITEM/S
                          </a>
                        </div>
                        <div class="clear"></div>
                      </div >
                    </div >
                    <div id="navcontainer">
                      <ul>

                        <li class="default">
                          <a href="categorylist.aspx?&ld=0&cid=WES-04D&byp=2&path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT1UA57a3MFttoguxpuJpJdQ2Lpafl3GGM0%3d" target="_top">Audio Components</a>
                        </li>
                        <li class="default">
                          <a href="categorylist.aspx?&ld=0&cid=WES-08H&byp=2&path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT2OJsMaIIAX6TYutxOQb%2blW" target="_top">Automotive</a>
                        </li>
                        <li class="default">
                          <a href="categorylist.aspx?&ld=0&cid=WES-02B&byp=2&path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT375YXMq%2bmoclW95UJKxZQGShnRSQB5fio%3d" target="_top">
                            AV TV SAT<br/>Installation
                          </a>
                        </li>
                        <li class="default">
                          <a href="categorylist.aspx?&ld=0&cid=WES-03C&byp=2&path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT0Nz%2fOF9uCpC8JGNDu4likJzyRwe2ZEcG4%3d" target="_top">
                            Cables &<br/>Connectors
                          </a>
                        </li>
                        <li class="default">
                          <a href="categorylist.aspx?&ld=0&cid=WES0830&byp=2&path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT3PRWzEEvqUxJDP4QQ1zyupqbNlwtoAqRs%3d" target="_top">
                            Cellular<br/>Accessories
                          </a>
                        </li>
                        <li class="default">
                          <a href="categorylist.aspx?&ld=0&cid=WES-06F&byp=2&path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT21eHD1mW8Xi4a3kLns%2boNRvdezat1NhpQ7KLVEsRIkuw%3d%3d" target="_top">
                            Communication<br/>Accessories
                          </a>
                        </li>
                        <li class="default">
                          <a href="categorylist.aspx?&ld=0&cid=WES-05E&byp=2&path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT0MIR57wXzlJ4pvXfTb5WhveYOIYN2kI1k%3d" target="_top">DATA & Computer</a>
                        </li>
                        <li class="default">
                          <a href="categorylist.aspx?&ld=0&cid=WES-11K&byp=2&path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT0COcya87%2b5qnUnQZn4JIGleNSLe%2bHqqNM%3d" target="_top">
                            Electronic<br/>Components
                          </a>
                        </li>
                        <li class="default">
                          <a href="categorylist.aspx?&ld=0&cid=WES-01A&byp=2&path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT3%2faxCH8UYK12zWnxguXmSRoHicdHZeLVo%3d" target="_top">
                            Power &<br/>Lighting
                          </a>
                        </li>
                        <li class="default">
                          <a href="categorylist.aspx?&ld=0&cid=RC1&byp=2&path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT0kWcD3CMzKGdJIAKOK3PhQ" target="_top">ROOTCAT1</a>
                        </li>
                        <li class="default">
                          <a href="categorylist.aspx?&ld=0&cid=RC2&byp=2&path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT0kWcD3CMzKGaJWGuxVzJxl" target="_top">ROOTCAT2</a>
                        </li>
                        <li class="default">
                          <a href="categorylist.aspx?&ld=0&cid=RC4&byp=2&path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT0kWcD3CMzKGY1ZTSThjoev" target="_top">ROOTCAT4</a>
                        </li>
                        <li class="default">
                          <a href="categorylist.aspx?&ld=0&cid=WES-07G&byp=2&path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT0T1bsnnmNMuoL7PWdQglzw" target="_top">Security</a>
                        </li>
                        <li class="default">
                          <a href="categorylist.aspx?&ld=0&cid=WES-12L&byp=2&path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT1mMsafcFOwJzukqs5wEXE3gngQgLmgh20%3d" target="_top">
                            Service<br/>& Repair Parts
                          </a>
                        </li>
                        <li class="default">
                          <a href="categorylist.aspx?&ld=0&cid=WES-10J&byp=2&path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT0XoEFwPAuCXcRUB%2ftlD4u1" target="_top">
                            Tools &<br/>Test
                          </a>
                        </li>
                        <li class="default">
                          <a href="categorylist.aspx?&ld=0&cid=WESNEWS&byp=2&path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT1jeqpQhfYbVHmDiGDdpCNZ" class="last redbtn">
                            WES NEWS
                            <br/>
                            NEW PRODUCTS
                          </a>
                        </li >
                      </ul>
                    </div>
                    <div class="clear"></div>
                  </div >
                </td>
              </tr>
            </table>
          </td>
        </tr>
        <tr>
          <td class="style2" style="border-style: none" valign="top" align="center">
            <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
              <tr>
                <td height="10" align="left" valign="top">
                </td>
              </tr>
              <tr>
                <td align="center" valign="top">
                  <table border="0" width="970" cellpadding="0" cellspacing="0">
                    <tr>
                      <td valign="top">
                        <table border="0" cellpadding="0" cellspacing="0">
                          <tr>
                            <td width="180" valign="top">

                              <input name="ctl00$leftnav$ctl00$hidcatIds" type="hidden" id="ctl00_leftnav_ctl00_hidcatIds" />
                              <input name="ctl00$leftnav$ctl00$HidsubcatIds" type="hidden" id="ctl00_leftnav_ctl00_HidsubcatIds" />
                              <input name="ctl00$leftnav$ctl00$HidsubcatIds1" type="hidden" id="ctl00_leftnav_ctl00_HidsubcatIds1" />
                              <input name="ctl00$leftnav$ctl00$HidItemPage" type="hidden" id="ctl00_leftnav_ctl00_HidItemPage" />
                              <input name="ctl00$leftnav$ctl00$Hidcat" type="hidden" id="ctl00_leftnav_ctl00_Hidcat" />
                              <script language="javascript">
                                function GetSelectedItems(field) {
                                var SelAttrStr = '';
                                for (var j = 0; j < document.getElementById(field).options.length; j++) {
             if (document.getElementById(field).options[j].selected) {
                 if (document.getElementById(field).options[j].value != 'Select Brand' && document.getElementById(field).options[j].value != 'List all models' && document.getElementById(field).options[j].value != 'List all products') {
                                SelAttrStr = document.getElementById(field).options[j].value + '^' + field;
                                if (field == 1)
                                document.getElementById("ctl00_leftnav_ctl00_hidcatIds").value = SelAttrStr;
                                else if (field == 2) {
                                document.getElementById("ctl00_leftnav_ctl00_HidsubcatIds").value = SelAttrStr;
                                }
                                else if (field == 3) {
                                document.getElementById("ctl00_leftnav_ctl00_HidsubcatIds1").value = SelAttrStr;
                                }

                                if (field <= 3) {
                                document.forms[0].submit();
                                }
                                }
                                }

                                }
                                }

                              </script>

                              <table width="180" border="0" cellspacing="0" cellpadding="0" >
                                <tr>
                                  <td>





                                  </td>
                                </tr>
                              </table>
                              </br>


                            </td>
                            <td width="790" align="left" valign="top">



                              <script language="javascript" >
                                function productbuy(buyvalue, pid) {
                                var qtyval = document.forms[0].elements[buyvalue].value;
                                var qtyavail = document.forms[0].elements[buyvalue].name;
                                qtyavail = qtyavail.toString().split('_')[1];
                                var minordqty = document.forms[0].elements[buyvalue].name;
                                minordqty = minordqty.toString().split('_')[2];

                                var fid = document.forms[0].elements[buyvalue].name;
                                fid = fid.toString().split('_')[3];

                                var orgurl = "http" + "://" + "localhost:1664" + "/";


                                if (isNaN(qtyval) || qtyval == "" || qtyval <= 0 || qtyval.indexOf(".") != -1) {
                                alert('Invalid Quantity!');
                                window.document.forms[0].elements[buyvalue].style.borderColor = "red";
                                document.forms[0].elements[buyvalue].focus();
                                return false;
                                }
                                else {
                                // window.document.location = 'OrderDetails.aspx?&bulkorder=1&Pid=' + pid + '&Qty=' + qtyval;
                                CallProductPopup(orgurl,buyvalue, pid, qtyval, 0, fid);
                                }
                                }

                                function keyct(e) {
                                var keyCode = (e.keyCode ? e.keyCode : e.which);
                                if (keyCode == 8 || (keyCode >= 48 && keyCode <= 57) || (keyCode >= 96 && keyCode <= 105)) {

                                }
                                else {
                                e.preventDefault();
                                }
                                }
                              </script>
                              <script type="text/javascript">
                                function test(id) {
                                var testid = "popupouterdiv" + id;
                                var objDiv = document.getElementById(testid);
                                var pidr = objDiv.id;
                                document.getElementById(pidr).style.visibility = 'hidden';
                                var podv = $(objDiv).html();
                                // var objDiva = document.getElementById(id);
                                var d = document.getElementById(pidr);
                                var menuDiv1 = document.getElementById('testdiv');
                                var menuDiv = document.getElementById('testdiv');
                                $(menuDiv).html('');
                                $(menuDiv).css("width", "220px");
                                $(menuDiv).css("visibility", "visible");
                                $(menuDiv).css("padding", "5px");
                                $(menuDiv).append(podv);
                                var IE = document.all ? true : false;
                                if (IE) {
                                tempX = event.clientX + document.documentElement.scrollLeft;
                                tempY = event.clientY + document.documentElement.scrollTop;
                                MouseX = tempX;
                                MouseY = tempY;
                                $(menuDiv).css(
                                {
                                position: "absolute",
                                left: MouseX - (menuDiv.clientWidth - 120) + 'px',
                                top: MouseY - (menuDiv.clientHeight + 35) + 'px'
                                }
                                );
                                }
                                else {
                                $(menuDiv).css(
                                {
                                position: "absolute",
                                left: MouseX - (menuDiv.clientWidth - 120) + 'px',
                                top: (MouseY - (menuDiv.offsetHeight + 35)) + 'px'
                                }
                                );
                                }
                                }
                                function Mouseout(id) {
                                var testid = "popupouterdiv" + id;
                                var objDiv = document.getElementById(testid);
                                var pidr = objDiv.id;
                                document.getElementById(pidr).style.visibility = 'hidden';
                                document.getElementById('testdiv').style.visibility = 'hidden';
                                }
                                function createMenuDiv() {
                                var menuDiv = document.createElement("div");
                                $(menuDiv).css("background-color", "yellow");
                                $(menuDiv).css("z-index", "1");
                                $(menuDiv).css("width", "220px");
                                $(menuDiv).css("height", "70px");
                                $(menuDiv).css("visibility", "visible");
                                return menuDiv;
                                }
                                function Moverstockstatus(id) {
                                var testid = "pid" + id;
                                var objDiv = document.getElementById(testid);
                                var pidr = objDiv.id;
                                document.getElementById(pidr).style.visibility = 'hidden';
                                var podv = $(objDiv).html();
                                var objDiva = document.getElementById(id);
                                // var pida = objDiva.id;
                                var d = document.getElementById(pidr);
                                var menuDiv1 = document.getElementById('testdiv');
                                var menuDiv = document.getElementById('testdiv');
                                $(menuDiv).html('');
                                $(menuDiv).css("width", "220px");
                                $(menuDiv).css("visibility", "visible");
                                $(menuDiv).css("padding", "5px");
                                $(menuDiv).append(podv);
                                var IE = document.all ? true : false;
                                if (IE) {
                                tempX = event.clientX + document.documentElement.scrollLeft;
                                tempY = event.clientY + document.documentElement.scrollTop;
                                MouseX = tempX;
                                MouseY = tempY;
                                $(menuDiv).css(
                                {
                                position: "absolute",
                                left: MouseX - (menuDiv.clientWidth - 120) + 'px',
                                top: MouseY - (menuDiv.clientHeight + 35) + 'px'
                                }
                                );
                                }
                                else {
                                $(menuDiv).css(
                                {
                                position: "absolute",
                                left: MouseX - (menuDiv.clientWidth - 120) + 'px',
                                top: (MouseY - (menuDiv.offsetHeight + 35)) + 'px'
                                }
                                );
                                }
                                }
                                function Moutstockstatus(id) {
                                var testid = "pid" + id;
                                var objDiv = document.getElementById(testid);
                                var pidr = objDiv.id;
                                document.getElementById(pidr).style.visibility = 'hidden';
                                document.getElementById('testdiv').style.visibility = 'hidden';
                                }
                                //    new script
                                function Moverimgtag(id) {
                                var testid = "pro_img_popup" + id;
                                var objDiv = document.getElementById(testid);
                                var pidrnew = objDiv.id;
                                document.getElementById(pidrnew).style.visibility = 'hidden';
                                var podvimg = $(objDiv).html();
                                //var objDiva = document.getElementById(id);

                                // var pida = objDiva.id;
                                var d = document.getElementById(pidrnew);
                                var menuDiv1 = document.getElementById('testdivimg');
                                var menuDiv = document.getElementById('testdivimg');
                                $(menuDiv).html('');
                                $(menuDiv).css("visibility", "visible");
                                $(menuDiv).append(podvimg);
                                var IE = document.all ? true : false;
                                if (IE) {
                                tempX = event.clientX + document.documentElement.scrollLeft;
                                tempY = event.clientY + document.documentElement.scrollTop;
                                MouseX = tempX;
                                MouseY = tempY;
                                $(menuDiv).css(
                                {
                                position: "absolute",
                                left: MouseX - (menuDiv.clientWidth - 200) + 'px',
                                top: MouseY - (menuDiv.clientHeight + 35) + 'px'
                                }
                                );
                                }
                                else {

                                $(menuDiv).css(
                                {
                                position: "absolute",
                                left: MouseX - (menuDiv.clientWidth - 200) + 'px',
                                top: (MouseY - (menuDiv.offsetHeight + 35)) + 'px'
                                }
                                );
                                }
                                }
                                function Moutimgtag(id) {
                                var testid = "pro_img_popup" + id;
                                var objDivnew = document.getElementById(testid);
                                var pidrnew = objDivnew.id;
                                document.getElementById(pidrnew).style.visibility = 'hidden';
                                document.getElementById('testdivimg').style.visibility = 'hidden';
                                }
                              </script>


                              <script language="javascript" type = "text/javascript">




                                function webmethodprint() {
                                try {
                                //             alert("yes");

                                //             var printContents = document.getElementById("familyprint_div").innerHTML;
                                //             var originalContents = document.body.innerHTML;

                                //             document.body.innerHTML = printContents;

                                //             window.print();

                                //             document.body.innerHTML = originalContents;


                                //            html2canvas($('body'),{
                                //                onrendered: function (canvas) {
                                //                            var imgString = canvas.toDataURL("image/png");
                                //                            window.open(imgString);
                                //            }
                                //

                                var printContent = document.getElementById("ctl00_maincontent_ctl00_chkpriceprint");
                                // var chkprintprice = document.getElementById("ctl00_maincontent_ctl00_chkDetailprint");

                                var hffid = document.getElementById("ctl00_maincontent_ctl00_hffid");

                                var printprice = false;
                                var printdet = true;
                                var cellx = "cell";
                                if (printContent.checked) {
                                printprice = true;
                                }
                                // if (chkprintprice.checked) {
                                //    printdet = true;
                                //  }

                                $.ajax({
                                type: "POST",
                                url: "family.aspx/Strval",
                                data: "{'WithPrice':'" + printprice + "','Details':'" + printdet + "','cellname':'" + cellx + "','fid':'" + hffid.value + "'  }",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (data) {

                                var windowUrl = 'http://staging2.wesonline.com.au/productdetails';

                                var windowName = 'Print Product Details';
                                var printWindow = ''
                                // window.open(windowUrl, windowName, 'left=50000,top=50000,width=0,height=0');

                                var ua = window.navigator.userAgent;
                                var msie = ua.indexOf("MSIE ");

                                if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {    // If Internet Explorer, return version number
                                alert(parseInt(ua.substring(msie + 5, ua.indexOf(".", msie))));
                                printWindow = window.open('', '', 'left=50000,top=50000,width=0,height=0');
                                }
                                else {              // If another browser, return 0
                                //alert('otherbrowser');
                                printWindow = window.open(windowUrl, windowName, 'left=50000,top=50000,width=0,height=0');
                                }

                                printWindow.document.write(data.d);

                                printWindow.document.close();

                                if (navigator.userAgent.toLowerCase().indexOf('chrome') >= 64) {
                                // Chrome Browser Detected?

                                printWindow.PPClose = false;

                                printWindow.onbeforeunload = function () {

                                // Before Window Close Event
                                if (printWindow.PPClose == false) {
                                // Close not OK?

                                return 'Leaving this page will block the parent window!\nPlease select "Stay on this Page option" and use the\nCancel button instead to close the Print Preview Window.\n';

                                }
                                }
                                printWindow.focus();

                                printWindow.print();

                                alert("Leaving Print page will block the parent window!\Use Cancel button to close the Print Preview Window.\n");                              // Clear Close Flag                                          // Print preview
                                printWindow.PPClose = true;                                      // Set Close Flag to OK.
                                }
                                else {

                                printWindow.print();
                                printWindow.focus();
                                printWindow.close();
                                }



                                },
                                error: function (xhr, status, error) {
                                var err = eval("(" + xhr.responseText + ")");
                                alert(err.Message);
                                }
                                })
                                printmethodclose();
                                }
                                catch (Error) {
                                alert(Error);
                                }
                                }


                                function webmethodemail() {
                                try {

                                var emailContent = document.getElementById("chkPriceemail");
                                //var chkemailprice = document.getElementById("chkDetialsemail");
                                var txtemailid = document.getElementById("txtemail");

                                var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

                                if (!filter.test(txtemailid.value)) {
                                alert('Please provide a valid email address');
                                txtemailid.focus;
                                return false;
                                }
                                var notes = document.getElementById("txtnotes");


                                var hffid = document.getElementById("ctl00_maincontent_family_hffid");

                                var printprice = false;
                                var printdet = true;
                                var cellx = "cell";
                                var emailid = txtemailid.value;
                                if (emailContent.checked) {
                                printprice = true;
                                }
                                // if (chkemailprice.checked) {
                                //   printdet = true;
                                // }

                                var hurl = "family.aspx?emailprice=" + printprice + "&emaildet=" + printdet + "&fid=" + hffid.value ;


                                $.ajax({
                                type: "POST",
                                url: "ProductDetails.aspx/StrEmail",
                                data: "{'WithPrice':'" + printprice + "','Details':'" + printdet + "','cellname':'" + cellx + "','emailid':'" + emailid + "','notes':'" + notes.value + "', 'pid':'" + hfpid.value + "','fid':'" + hffid.value + "','cid':'" + hfcid.value + "' }",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (data) {
                                var d = data.d;
                                emailmethodclose();
                                window.location.href = hurl;
                                },
                                error: function (xhr, status, error) {
                                var err = eval("(" + xhr.responseText + ")");
                                alert(err.Message);
                                }
                                })

                                }
                                catch (Error) {
                                alert(Error);
                                }
                                }
                                function webmethodpdf() {
                                try {

                                var chkpdfContent = document.getElementById("chkpricepdf");
                                // var chkpdfprice = document.getElementById("chkDetailpdf");

                                var printprice = "false";
                                var printdet = "true";
                                var cellx = "cell_pdf";
                                var hfpid = document.getElementById("ctl00_maincontent_Product1_hfpid");
                                var hffid = document.getElementById("ctl00_maincontent_Product1_hffid");
                                var hfcid = document.getElementById("ctl00_maincontent_Product1_hfcid");
                                if (chkpdfContent.checked) {
                                printprice = "true";
                                document.getElementById("ctl00_maincontent_Product1_hfpricepdf").value = printprice;

                                }
                                // if (chkpdfprice.checked) {
                                //     printdet = "true";
                                //      document.getElementById("ctl00_maincontent_Product1_hfdetailspdf").value = printdet;
                                //  }
                                var hurl = "productdetails.aspx?printprice=" + printprice + "&printdet=" + printdet + "&pid=" + hfpid.value + "&fid=" + hffid.value + "&cid=" + hfcid.value;
                                pdfmethodclose();
                                window.location.href = hurl;


                                }
                                catch (Error) {
                                alert(Error);
                                }
                                }



                              </script>
                              <script language="javascript" type = "text/javascript">
                                function showpopup_print() {

                                try
                                {
                                var objDiv = document.getElementById("divprintpop");
                                objDiv.style.visibility = 'visible';
                                var objDiv = document.getElementById("divpdfpop").style.visibility = 'hidden';
                                var objDiv = document.getElementById("divemailpop").style.visibility = 'hidden';
                                var chkprice = document.getElementById("ctl00_maincontent_ctl00_chkpriceprint");
                                chkprice.checked = true;
                                // var chkDetialsemail = document.getElementById("ctl00_maincontent_ctl00_chkDetailprint");
                                //chkDetialsemail.checked = true;
                                // chkDetialsemail.focus();

                                }
                                catch (Error)
                                {
                                alert(Error);
                                }
                                }


                                function showpopup_pdf() {
                                var objDiv = document.getElementById("divpdfpop");
                                objDiv.style.visibility = 'visible';
                                var objDiv = document.getElementById("divprintpop").style.visibility = 'hidden';
                                var objDiv = document.getElementById("divemailpop").style.visibility = 'hidden';
                                var chkprice = document.getElementById("ctl00_maincontent_ctl00_chkpricepdf");
                                chkprice.checked = true;
                                //var chkDetialsemail = document.getElementById("ctl00_maincontent_ctl00_chkDetailpdf");
                                // chkDetialsemail.checked = true;
                                }
                                function showpopup_email() {
                                var objDiv = document.getElementById("divemailpop");
                                objDiv.style.visibility = 'visible';
                                var chkprice = document.getElementById("ctl00_maincontent_ctl00_chkPriceemail");
                                chkprice.checked = true;
                                // var chkDetialsemail = document.getElementById("ctl00_maincontent_ctl00_chkDetialsemail");
                                //chkDetialsemail.checked = true;

                                var objDiv = document.getElementById("divprintpop").style.visibility = 'hidden';
                                var objDiv = document.getElementById("divpdfpop").style.visibility = 'hidden';

                                }

                                function printmethodclose() {

                                var objDiv = document.getElementById('divprintpop');
                                objDiv.style.visibility = 'hidden';

                                }
                                function pdfmethodclose() {

                                var objDiv = document.getElementById('divpdfpop');
                                objDiv.style.visibility = 'hidden';

                                }
                                function emailmethodclose() {

                                var objDiv = document.getElementById('divemailpop');
                                objDiv.style.visibility = 'hidden';

                                }
                                function emailmethodclear() {

                                var objDiv = document.getElementById('divemailpop');
                                objDiv.style.visibility = 'hidden';
                                var txtemail = document.getElementById("ctl00_maincontent_ctl00_txtemail");
                                txtemail.value = "";
                                var txtnotes = document.getElementById("ctl00_maincontent_ctl00_txtnotes");
                                txtnotes.value = "";

                                }
                                function buttonmouseout() {


                                printmethodclose();
                                pdfmethodclose();
                                emailmethodclose();
                                }
                                function buttonmouseout_clear() {


                                printmethodclose();
                                pdfmethodclose();
                                emailmethodclear();
                                }
                              </script>
                              <div class="br" style="clear:both;">

                              </div>
                              <div class="clear"></div>

                              <div class="box1" style="width:764px;margin:0 0 0 10px;"  id="familyprint_div">
                                <div onmouseover="buttonmouseout()" >
                                  <!--<div id="div1" style="display:none;"></div>-->
                                  <div class="product_top">
                                    <div class="family_img">
                                      <div id="product-image" class="gallery-view" style="height:210px:vertical-align: middle;" >
                                        <img style="display: block;max-height:256px;max-width:256px;" src="images/noimage.gif" style="display:none;"  alt=""/>
                                      </div>

                                      <div class="product-info">
                                        <div class="brand"></div>
                                        <div class="gallery-thumbnails">
                                          <div id="gallery" style="margin-top:25px">
                                            <ul style="list-style: none outside none;">
                                            </ul>
                                          </div>
                                        </div>
                                      </div>
                                    </div>
                                    <div class="family_txt">
                                      <h1 class="pad10" style=" color: #666666;font-size: 16px;text-align:left;">REQUIRES CABLE</h1>
                                      <p>
                                      </p>
                                      <div id="div1" style="color: #222222;font-size: 11px;line-height: 14px;margin: 5px 0;min-height:180px;">
                                        <p>
                                          Cable is NOT included in any of the Look-C kits. 4 Core Low voltage “Bell wire” is required. eg. Telephone, CAT5 or security cable, etc.<br/>For runs up to 20M we recommend SEC414 (ACA Approved). For longer runs, CAT5 twisted pair type cable is quite suitable up to 100M.<br/>For electronically noisy environments or to ensure maximum picture quality we have available shielded coax cable with control wires.<br/><br/>CAT5 cable is available in Blue, Yellow, Grey, Red, Green and Pink.<br/>Other lengths are also available. See Cable Section for CAT5 Cable.<br/><br/><bol>DOOR STRIKE EL971</bol>&nbsp;<br/>Door-Strike Requires 12Vdc/500mA source, local or remote<br/>
                                        </p>
                                      </div>
                                      <div id="div2" style="display:none;color: #222222;font-size: 11px;line-height: 14px;margin: 5px 0;min-height:180px;">
                                        <p>
                                          Cable is NOT included in any of the Look-C kits. 4 Core Low voltage “Bell wire” is required. eg. Telephone, CAT5 or security cable, etc.<br/>For runs up to 20M we recommend SEC414 (ACA Approved). For longer runs, CAT5 twisted pair type cable is quite suitable up to 100M.<br/>For electronically noisy environments or to ensure maximum picture quality we have available shielded coax cable with control wires.<br/><br/>CAT5 cable is available in Blue, Yellow, Grey, Red, Green and Pink.<br/>Other lengths are also available. See Cable Section for CAT5 Cable.<br/><br/><bol>DOOR STRIKE EL971</bol>&nbsp;<br/>Door-Strike Requires 12Vdc/500mA source, local or remote<br/>
                                        </p>
                                      </div>
                                      <br/>



                                    </div>
                                    <div class="clear"></div>

                                  </div>
                                  <script language="javascript">

                                    var IE = document.all?true:false;
                                    var MouseX = 0;
                                    var MouseY = 0;

                                    if (!IE)
                                    {
                                    document.captureEvents(Event.MOUSEMOVE);
                                    document.onmousemove = getMouseXY;
                                    }


                                    function ShowPriceTable(objectid)
                                    {


                                    var objDiv = document.getElementById(objectid);
                                    objDiv.style.visibility = "visible";
                                    if (IE)
                                    {
                                    getMouseXY();
                                    objDiv.style.left = MouseX - (objDiv.clientWidth + 5) + 'px';
                                    objDiv.style.top = MouseY - (objDiv.clientHeight + 25) + 'px';
                                    }
                                    else
                                    {

                                    objDiv.style.left = MouseX - (objDiv.clientWidth -100) + 'px';
                                    objDiv.style.top = (MouseY - (objDiv.offsetHeight + 50)) + 'px';
                                    }
                                    objDiv.style.zIndex = 1;
                                    objDiv.style.padding = "5px";

                                    }

                                    function getMouseXY(e)
                                    {
                                    if (IE) { // grab the x-y pos.s if browser is IE
                                    tempX = event.clientX + document.documentElement.scrollLeft;
                                    tempY = event.clientY + document.documentElement.scrollTop;
                                    } else {  // grab the x-y pos.s if browser is NS
                                    tempX = e.pageX;
                                    tempY = e.pageY;

                                    }
                                    MouseX = tempX;
                                    MouseY = tempY;
                                    }

                                    function ClosePriceTable(objectid)
                                    {
                                    var objDiv = document.getElementById(objectid);
                                    objDiv.style.visibility = "hidden";
                                    }
                                  </script>


                                </div>
                                <div>
                                  <table  width="320px" height="20px" align="right" style="border-color:grey">
                                    <tr>
                                      <td>
                                        <div class="costable">

                                          <div class="pricepopup">
                                            <div class="popupouterdiv3" id="divprintpop" >
                                              <div class="popupaero15_1"></div>
                                              <table>
                                                <tr>
                                                  <td>
                                                    <div class="row-mid">
                                                      <div class="poptitle">
                                                        <span class="titleimg-pop">
                                                          <img src="/images/print.png" />
                                                        </span>
                                                        <span class="title-pop">Print Product Details Page Options</span>
                                                      </div>
                                                    </div>
                                                    <div class="row-mid">
                                                      <p>Available options when creating Page</p>
                                                    </div>
                                                    <div class="row-mid">
                                                      <div class="opochoice">
                                                        <table align="center">
                                                          <tr>
                                                            <td>
                                                              <input name="ctl00$maincontent$ctl00$chkpriceprint" type="checkbox" id="ctl00_maincontent_ctl00_chkpriceprint" value="false" />
                                                            </td>
                                                            <td align="left">

                                                              <label>Show price</label>
                                                            </td>
                                                          </tr>

                                                        </table>




                                                      </div>
                                                    </div>

                                                    <div class="row-center">
                                                      <table align="center">
                                                        <tr>
                                                          <td>
                                                            <a class="btnbuy2 button psmallsiz btngreen" href="#"  onclick="webmethodprint();">Submit</a>


                                                          </td>
                                                        </tr>
                                                      </table >
                                                    </div>
                                                  </td>
                                                </tr>
                                              </table>
                                            </div>
                                          </div>
                                        </div>
                                        <a id="print" class="aprint costable" href="#" onmouseover="showpopup_print();" style="visibility:visible;"></a>
                                        <div class="costable">

                                          <div class="pricepopup">
                                            <div class="popupouterdiv4" id="divpdfpop">
                                              <div class="popupaero15_2"></div>
                                              <table>
                                                <tr>
                                                  <td>
                                                    <div class="row-mid">
                                                      <div class="poptitle">
                                                        <span class="titleimg-pop">
                                                          <img src="/images/pdf.png" />
                                                        </span>
                                                        <span class="title-pop">PDF Product Details Page Options</span>
                                                      </div>
                                                    </div>
                                                    <div class="row-mid">
                                                      <p>Available options when creating Page</p>
                                                    </div>
                                                    <div class="row-mid">
                                                      <div class="opochoice">
                                                        <table align="center">
                                                          <tr>
                                                            <td>
                                                              <input name="ctl00$maincontent$ctl00$chkpricepdf" type="checkbox" id="ctl00_maincontent_ctl00_chkpricepdf" value="false" checked="checked" />
                                                            </td>
                                                            <td align="left">
                                                              <label>Show price</label>
                                                            </td>
                                                          </tr>

                                                        </table>




                                                      </div>
                                                    </div>
                                                    <div class="row-center">
                                                      <table align="center">
                                                        <tr>
                                                          <td>

                                                            <input type="submit" name="ctl00$maincontent$ctl00$pdfbtn" value="Submit" id="ctl00_maincontent_ctl00_pdfbtn" class="btnbuy2 button psmallsiz btngreen" />
                                                          </td>
                                                        </tr>
                                                      </table >



                                                    </div>

                                                  </td>
                                                </tr>
                                              </table>
                                            </div>
                                          </div>
                                        </div>
                                        <a class="apdf" href="#" onmouseover="showpopup_pdf();"> </a>
                                        <div class="costable">
                                          <div class="pricepopup">
                                            <div class="popupouterdiv5" id="divemailpop">
                                              <div class="popupaero15_3"></div>
                                              <table>
                                                <tr>
                                                  <td>
                                                    <div class="row-mid">
                                                      <div class="poptitle">
                                                        <span class="titleimg-pop">
                                                          <img src="/images/email.png" />
                                                        </span>
                                                        <span class="title-pop">Email Product Details Page Options</span>

                                                      </div>
                                                    </div>
                                                    <div class="row-mid">
                                                      <table align="center">
                                                        <tr>
                                                          <td style="width:20px">

                                                            <label>Email Id</label>

                                                            <textarea name="ctl00$maincontent$ctl00$txtemail" id="ctl00_maincontent_ctl00_txtemail" rows="1" cols="50" style="width:200px;height:20px">                              </textarea>
                                                          </td>
                                                        </tr>
                                                        <tr>
                                                          <td style="width:20px">
                                                            <label>Notes</label>
                                                            <textarea name="ctl00$maincontent$ctl00$txtnotes" id="ctl00_maincontent_ctl00_txtnotes" rows="5" cols="50" style="width:200px;height:40px">                              </textarea>

                                                          </td>
                                                        </tr>

                                                      </table>
                                                    </div>
                                                    <div class="row-mid">
                                                      <p>Available options when creating Page</p>
                                                    </div>
                                                    <div class="row-mid">
                                                      <div class="opochoice">
                                                        <table>
                                                          <tr>
                                                            <td>
                                                              <input name="ctl00$maincontent$ctl00$chkPriceemail" type="checkbox" id="ctl00_maincontent_ctl00_chkPriceemail" value="false" />
                                                            </td>
                                                            <td align="left">
                                                              <label>Show price</label>

                                                            </td>
                                                          </tr>

                                                        </table>




                                                      </div>
                                                    </div>

                                                    <div class="row-center">
                                                      <table align="center">
                                                        <tr>
                                                          <td>

                                                            <input type="submit" name="ctl00$maincontent$ctl00$emailbtn" value="Submit" id="ctl00_maincontent_ctl00_emailbtn" class="btnbuy2 button psmallsiz btngreen" />
                                                          </td>
                                                        </tr>
                                                      </table>

                                                    </div>
                                                    <label id="ctl00_maincontent_ctl00_lblmessage" text="">
                                                    </label>
                                                  </td>
                                                </tr>
                                              </table>
                                            </div>
                                          </div>
                                        </div>

                                        <a class="aemail" href="#" onmouseover="showpopup_email(); "> </a>

                                      </td>


                                    </tr>

                                  </table>


                                </div>

                                <div class="tabbable" onmouseover="buttonmouseout_clear()">
                                  <ul class="tabs">
                                    <li>
                                      <a href="#tab1" >Products</a>
                                    </li>
                                    <li style="display:none"; >
                                      <a href="#tab2" >Downloads</a>
                                    </li>
                                  </ul>
                                  <div class="clear"></div>
                                  <div class="tab-content">
                                    <div id="tab1" class="tab_content" >
                                      <div style=" width:100%; height:100%;">
                                        <div class="fpscroll">
                                          <table width="100%" border="0" style=" overflow:hidden;" cellpadding="0" cellspacing="0">
                                            <tr>
                                              <td align="left" >
                                                <div Class="testscroll">
                                                  <TABLE width="99%" border="0" cellspacing="1" Class="FamilyPageTable" cellpadding="3">
                                                    <TR>
                                                      <TD ALIGN="Center" VALIGN="Middle" Class="FamilyPageTableHead" style="width: 200;   " ></TD>
                                                      <TD ALIGN="Center" VALIGN="Middle" Class="FamilyPageTableHead" style="width: 200;  " >Code</TD>
                                                      <TD ALIGN="Center" VALIGN="Middle" Class="FamilyPageTableHead" style="width: 200;  " >COLOUR</TD>
                                                      <TD ALIGN="Center" VALIGN="Middle" Class="FamilyPageTableHead" style="width: 200;  " >DESCRIPTION</TD>
                                                      <TD ALIGN="Center" VALIGN="Middle" Class="FamilyPageTableHead" style="width: 200;  " >LENGTH</TD>
                                                      <TD ALIGN="Center" VALIGN="Middle" Class="FamilyPageTableHead" style="width: 200px;   " >More Info</TD>
                                                      <TD ALIGN="Center" VALIGN="Middle" Class="FamilyPageTableHead" style="width: 200;  " >COST Ex GST</TD>
                                                      <TD ALIGN="Center" VALIGN="Middle" Class="FamilyPageTableHead" style="width:120px;    " >Cart</TD>
                                                    </TR>
                                                    <TR>
                                                      <TD  ALIGN="center" VALIGN="Middle" Class="FamilyPageTableCell" style="width: 200;   ">
                                                        <div class="pro_thum_outer">
                                                          <img src="Images/NoImage.gif" onMouseOut="javascript:Moutimgtag('10228');" onMouseOver="javascript:Moverimgtag('10228');" style="max-height:50px;max-width:50px" />
                                                        </div>
                                              </td>
                                              <TD ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width: 200;   " > SEC7505 </TD>
                                              <TD ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width: 200;   " > Cream </TD>
                                              <TD ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width: 200;   " > Coax + 5 Control Wires </TD>
                                              <TD ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width: 200;   " > 100M </TD>
                                              <TD  ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width:200;   ">
                                                <a href="productdetails.aspx?pid=10228&fid=10793&cid=WES908P&path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT0T1bsnnmNMugVJxFQPC6pgHzEp0wrrdomihnz0Py6cTfaJ7M51bMHCeYOIYN2kI1k%3d" class="tx_3">See More Details </a>
                                              </TD>
                                              <TD ALIGN="center" VALIGN="Middle"  Class="costable" style="width: 200;cursor:pointer;border-color: -moz-use-text-color #E8E8E8 #E8E8E8 -moz-use-text-color;border-style: none solid solid none;border-width: medium 1px 1px medium;border-color:#E8E8E8;   " >
                                                <div class="pricepopup">
                                                  <div id="pid10228" class="popupouterdiv2none">
                                                    <div class="popupaero"></div>
                                                    <table  class="table table-striped  table-bordered table-condensed" style="margin:0; background:#FFF">
                                                      <tr class="success">
                                                        <td width="28%">ORDER CODE:</td>
                                                        <td colspan="2">STOCK STATUS</td>
                                                      </tr>
                                                      <tr>
                                                        <td width="28%">SEC7505</td>
                                                        <td colspan="2">
                                                          <span >INSTOCK</span>
                                                          <br>
                                                        </td>
                                                      </tr>
                                                      <tr class="success">
                                                        <td>Qty</td>
                                                        <td width="38%">Cost Inc GST</td>
                                                        <td width="34%">Cost Ex GST</td>
                                                      </tr>
                                                      <tr>
                                                        <td class="bg_grey3">1+</td>
                                                        <td  class="bg_grey3" align="center">$95.15</td>
                                                        <td class="bg_grey3" align="center">$86.50</td>
                                                      </tr>
                                                      <tr>
                                                        <td class="bg_grey3">5+</td>
                                                        <td  class="bg_grey3" align="center">$85.64</td>
                                                        <td class="bg_grey3" align="center">$77.85</td>
                                                      </tr>
                                                      <tr>
                                                        <td class="bg_grey3">RRP</td>
                                                        <td  class="bg_grey3" align="center">$139.00</td>
                                                        <td class="bg_grey3" align="center">$126.36</td>
                                                      </tr>
                                                      <tr>
                                                        <td class="bg_grey31">Your Price</td>
                                                        <td  class="bg_grey31" align="center">$85.64</td>
                                                        <td class="bg_grey31" align="center">$77.85</td>
                                                      </tr>
                                                    </table>
                                                  </div>
                                                  <a class="poppricenone" onMouseOut="javascript:Moutstockstatus('10228');" onMouseOver="javascript:Moverstockstatus('10228');" style="text-decoration:none;">
                                                    $ 77.85  <br/> Price / Stock Status
                                                  </a>
                                                  </Div>
                                              </TD>
                                              <TD ALIGN="Center" VALIGN="Middle"  Class="FamilyPageTableCell"  style="width: 120px; ">
                                                <table width="100px" border="0" cellPadding="0" cellSpacing="0">
                                                  <tr>
                                                    <td>
                                                      <div>
                                                        <input valign="middle" name="txt10228_100_1_10793" onkeydown="return keyct(event)"  maxlength="6" type="text" size="1" id="txt10228_100_1_10793" style="background-color:#FFFFFF;color: #000000;width: 30px;height:21px;float:left;"/>
                                                        <div class="costable">
                                                          <div class="pricepopup">
                                                            <div class="popupouterdivnone" id="popupouterdiv10228">
                                                              <div class="popupaero"></div>
                                                              <table  class="table table-striped  table-bordered table-condensed" style="margin:0; background:#FFF">
                                                                <tr class="success">
                                                                  <td width="28%">ORDER CODE:</td>
                                                                  <td colspan="2">STOCK STATUS</td>
                                                                </tr>
                                                                <tr>
                                                                  <td width="28%">SEC7505</td>
                                                                  <td colspan="2">
                                                                    <span >INSTOCK</span>
                                                                    <br>
                                                                  </td>
                                                                </tr>
                                                                <tr class="success">
                                                                  <td>Qty</td>
                                                                  <td width="38%">Cost Inc GST</td>
                                                                  <td width="34%">Cost Ex GST</td>
                                                                </tr>
                                                                <tr>
                                                                  <td class="bg_grey3">1+</td>
                                                                  <td  class="bg_grey3" align="center">$95.15</td>
                                                                  <td class="bg_grey3" align="center">$86.50</td>
                                                                </tr>
                                                                <tr>
                                                                  <td class="bg_grey3">5+</td>
                                                                  <td  class="bg_grey3" align="center">$85.64</td>
                                                                  <td class="bg_grey3" align="center">$77.85</td>
                                                                </tr>
                                                                <tr>
                                                                  <td class="bg_grey3">RRP</td>
                                                                  <td  class="bg_grey3" align="center">$139.00</td>
                                                                  <td class="bg_grey3" align="center">$126.36</td>
                                                                </tr>
                                                                <tr>
                                                                  <td class="bg_grey31">Your Price</td>
                                                                  <td  class="bg_grey31" align="center">$85.64</td>
                                                                  <td class="bg_grey31" align="center">$77.85</td>
                                                                </tr>
                                                              </table>
                                                            </div>
                                                            <a style="cursor:pointer;margin: 0 0 0 5px;" onMouseOut="javascript:Mouseout('10228');" onMouseOver="javascript:test('10228');"  id="10228" valign="middle" class="btnbuy2 button smallsiz btngreen costable"  onClick="productbuy('txt10228_100_1_10793','10228');">Buy  </a>
                                                          </div>
                                                        </div>
                                                      </div>
                                                    </td>
                                                  </tr>
                                                </table>
                                              </TD>
                                              </TR>
                                              <TR>
                                                <TD  ALIGN="center" VALIGN="Middle" Class="FamilyPageTableCell" style="width: 200;   ">
                                                  <div class="pro_thum_outer">
                                                    <img src="Images/NoImage.gif" onMouseOut="javascript:Moutimgtag('10229');" onMouseOver="javascript:Moverimgtag('10229');"  style="max-height:50px;max-width:50px" />
                                                  </div>
                            </td>
                            <TD ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width:200;   " >SEC17505</TD>
                            <TD ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width:200;   " >Cream</TD>
                            <TD ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width:200;   " >Coax + 5 Control Wires</TD>
                            <TD ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width:200;   " >305M</TD>
                            <TD  ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width:200;  ">
                              <a href="productdetails.aspx?pid=10229&fid=10793&cid=WES908P&path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT0T1bsnnmNMugVJxFQPC6pgHzEp0wrrdomihnz0Py6cTfaJ7M51bMHCeYOIYN2kI1k%3d" class="tx_3">See More Details </a>
                            </TD>
                            <TD ALIGN="center" VALIGN="Middle"  Class="costable" style="width: 200px;cursor:pointer;border-color: -moz-use-text-color #E8E8E8 #E8E8E8 -moz-use-text-color;border-style: none solid solid none;border-width: medium 1px 1px medium;border-color:#E8E8E8;   " >
                              <div class="pricepopup">
                                <div id="pid10229" class="popupouterdiv2none">
                                  <div class="popupaero"></div>
                                  <table  class="table table-striped  table-bordered table-condensed" style="margin:0; background:#FFF">
                                    <tr class="success">
                                      <td width="28%">ORDER CODE:</td>
                                      <td colspan="2">STOCK STATUS</td>
                                    </tr>
                                    <tr>
                                      <td width="28%">SEC17505</td>
                                      <td colspan="2">
                                        <span >INSTOCK</span>
                                        <br>
                                      </td>
                                    </tr>
                                    <tr class="success">
                                      <td>Qty</td>
                                      <td width="38%">Cost Inc GST</td>
                                      <td width="34%">Cost Ex GST</td>
                                    </tr>
                                    <tr>
                                      <td class="bg_grey3">1+</td>
                                      <td  class="bg_grey3" align="center">$273.90</td>
                                      <td class="bg_grey3" align="center">$249.00</td>
                                    </tr>
                                    <tr>
                                      <td class="bg_grey3">5+</td>
                                      <td  class="bg_grey3" align="center">$246.51</td>
                                      <td class="bg_grey3" align="center">$224.10</td>
                                    </tr>
                                    <tr>
                                      <td class="bg_grey3">RRP</td>
                                      <td  class="bg_grey3" align="center">$349.00</td>
                                      <td class="bg_grey3" align="center">$317.27</td>
                                    </tr>
                                    <tr>
                                      <td class="bg_grey31">Your Price</td>
                                      <td  class="bg_grey31" align="center">$246.51</td>
                                      <td class="bg_grey31" align="center">$224.10</td>
                                    </tr>
                                  </table>
                                </div>
                                <a class="poppricenone" onMouseOut="javascript:Moutstockstatus('10229');" onMouseOver="javascript:Moverstockstatus('10229');" style="text-decoration:none;">
                                  $ 224.10  <br/> Price / Stock Status
                                </a>
                                </Div>
                            </TD>
                            <TD ALIGN="Center" VALIGN="Middle"  Class="FamilyPageTableCell"  style="width: 120px;  ">
                              <table width="100px" border="0" cellPadding="0" cellSpacing="0">
                                <tr>
                                  <td>
                                    <div>
                                      <input valign="middle" name="txt10229_100_1_10793" onkeydown="return keyct(event)"  maxlength="6" type="text" size="1" id="txt10229_100_1_10793" style="background-color:#FFFFFF;color: #000000;width: 30px;height:21px;float:left;"/>
                                      <div class="costable">
                                        <div class="pricepopup">
                                          <div class="popupouterdivnone" id="popupouterdiv10229">
                                            <div class="popupaero"></div>
                                            <table  class="table table-striped  table-bordered table-condensed" style="margin:0; background:#FFF">
                                              <tr class="success">
                                                <td width="28%">ORDER CODE:</td>
                                                <td colspan="2">STOCK STATUS</td>
                                              </tr>
                                              <tr>
                                                <td width="28%">SEC17505</td>
                                                <td colspan="2">
                                                  <span >INSTOCK</span>
                                                  <br>
                                                </td>
                                              </tr>
                                              <tr class="success">
                                                <td>Qty</td>
                                                <td width="38%">Cost Inc GST</td>
                                                <td width="34%">Cost Ex GST</td>
                                              </tr>
                                              <tr>
                                                <td class="bg_grey3">1+</td>
                                                <td  class="bg_grey3" align="center">$273.90</td>
                                                <td class="bg_grey3" align="center">$249.00</td>
                                              </tr>
                                              <tr>
                                                <td class="bg_grey3">5+</td>
                                                <td  class="bg_grey3" align="center">$246.51</td>
                                                <td class="bg_grey3" align="center">$224.10</td>
                                              </tr>
                                              <tr>
                                                <td class="bg_grey3">RRP</td>
                                                <td  class="bg_grey3" align="center">$349.00</td>
                                                <td class="bg_grey3" align="center">$317.27</td>
                                              </tr>
                                              <tr>
                                                <td class="bg_grey31">Your Price</td>
                                                <td  class="bg_grey31" align="center">$246.51</td>
                                                <td class="bg_grey31" align="center">$224.10</td>
                                              </tr>
                                            </table>
                                          </div>
                                          <a style="cursor:pointer;margin: 0 0 0 5px;" onMouseOut="javascript:Mouseout('10229');" onMouseOver="javascript:test('10229');"  id="10229" valign="middle" class="btnbuy2 button smallsiz btngreen costable"  onClick="productbuy('txt10229_100_1_10793','10229');">Buy  </a>
                                        </div>
                                      </div>
                                    </div>
                                  </td>
                                </tr>
                              </table>
                            </TD>
                            </TR>
                            <TR>
                              <TD  ALIGN="center" VALIGN="Middle" Class="FamilyPageTableCell" style="width: 200;   ">
                                <div class="pro_thum_outer">
                                  <img src="Images/NoImage.gif" onMouseOut="javascript:Moutimgtag('10292');" onMouseOver="javascript:Moverimgtag('10292');" style="max-height:50px;max-width:50px" />
                                </div>
                      </td>
                      <TD ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width: 200;   " >CNC5100BU</TD>
                      <TD ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width: 200;   " >Blue</TD>
                      <TD ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width: 200;   " >8 Core Category-5e Cable</TD>
                      <TD ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width: 200;   " >100M</TD>
                      <TD  ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width:200;   ">
                        <a href="productdetails.aspx?pid=10292&fid=10793&cid=WES908P&path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT0T1bsnnmNMugVJxFQPC6pgHzEp0wrrdomihnz0Py6cTfaJ7M51bMHCeYOIYN2kI1k%3d" class="tx_3">See More Details </a>
                      </TD>
                      <TD ALIGN="center" VALIGN="Middle"  Class="costable" style="width: 200;cursor:pointer;border-color: -moz-use-text-color #E8E8E8 #E8E8E8 -moz-use-text-color;border-style: none solid solid none;border-width: medium 1px 1px medium;border-color:#E8E8E8;   " >
                        <div class="pricepopup">
                          <div id="pid10292" class="popupouterdiv2none">
                            <div class="popupaero"></div>
                            <table  class="table table-striped  table-bordered table-condensed" style="margin:0; background:#FFF">
                              <tr class="success">
                                <td width="28%">ORDER CODE:</td>
                                <td colspan="2">STOCK STATUS</td>
                              </tr>
                              <tr>
                                <td width="28%">CNC5100BU</td>
                                <td colspan="2">
                                  <span >INSTOCK</span>
                                  <br>
                                </td>
                              </tr>
                              <tr class="success">
                                <td>Qty</td>
                                <td width="38%">Cost Inc GST</td>
                                <td width="34%">Cost Ex GST</td>
                              </tr>
                              <tr>
                                <td class="bg_grey3">1+</td>
                                <td  class="bg_grey3" align="center">$43.67</td>
                                <td class="bg_grey3" align="center">$39.70</td>
                              </tr>
                              <tr>
                                <td class="bg_grey3">5+</td>
                                <td  class="bg_grey3" align="center">$39.30</td>
                                <td class="bg_grey3" align="center">$35.73</td>
                              </tr>
                              <tr>
                                <td class="bg_grey3">RRP</td>
                                <td  class="bg_grey3" align="center">$54.55</td>
                                <td class="bg_grey3" align="center">$49.59</td>
                              </tr>
                              <tr>
                                <td class="bg_grey31">Your Price</td>
                                <td  class="bg_grey31" align="center">$39.30</td>
                                <td class="bg_grey31" align="center">$35.73</td>
                              </tr>
                            </table>
                          </div>
                          <a class="poppricenone" onMouseOut="javascript:Moutstockstatus('10292');" onMouseOver="javascript:Moverstockstatus('10292');" style="text-decoration:none;">
                            $ 35.73  <br/> Price / Stock Status
                          </a>
                          </Div>
                      </TD>
                      <TD ALIGN="Center" VALIGN="Middle"  Class="FamilyPageTableCell"  style="width: 120px; ">
                        <table width="100px" border="0" cellPadding="0" cellSpacing="0">
                          <tr>
                            <td>
                              <div>
                                <input valign="middle" name="txt10292_100_1_10793" onkeydown="return keyct(event)"  maxlength="6" type="text" size="1" id="txt10292_100_1_10793" style="background-color:#FFFFFF;color: #000000;width: 30px;height:21px;float:left;"/>
                                <div class="costable">
                                  <div class="pricepopup">
                                    <div class="popupouterdivnone" id="popupouterdiv10292">
                                      <div class="popupaero"></div>
                                      <table  class="table table-striped  table-bordered table-condensed" style="margin:0; background:#FFF">
                                        <tr class="success">
                                          <td width="28%">ORDER CODE:</td>
                                          <td colspan="2">STOCK STATUS</td>
                                        </tr>
                                        <tr>
                                          <td width="28%">CNC5100BU</td>
                                          <td colspan="2">
                                            <span >INSTOCK</span>
                                            <br>
                                          </td>
                                        </tr>
                                        <tr class="success">
                                          <td>Qty</td>
                                          <td width="38%">Cost Inc GST</td>
                                          <td width="34%">Cost Ex GST</td>
                                        </tr>
                                        <tr>
                                          <td class="bg_grey3">1+</td>
                                          <td  class="bg_grey3" align="center">$43.67</td>
                                          <td class="bg_grey3" align="center">$39.70</td>
                                        </tr>
                                        <tr>
                                          <td class="bg_grey3">5+</td>
                                          <td  class="bg_grey3" align="center">$39.30</td>
                                          <td class="bg_grey3" align="center">$35.73</td>
                                        </tr>
                                        <tr>
                                          <td class="bg_grey3">RRP</td>
                                          <td  class="bg_grey3" align="center">$54.55</td>
                                          <td class="bg_grey3" align="center">$49.59</td>
                                        </tr>
                                        <tr>
                                          <td class="bg_grey31">Your Price</td>
                                          <td  class="bg_grey31" align="center">$39.30</td>
                                          <td class="bg_grey31" align="center">$35.73</td>
                                        </tr>
                                      </table>
                                    </div>
                                    <a style="cursor:pointer;margin: 0 0 0 5px;" onMouseOut="javascript:Mouseout('10292');" onMouseOver="javascript:test('10292');"  id="10292" valign="middle" class="btnbuy2 button smallsiz btngreen costable"  onClick="productbuy('txt10292_100_1_10793','10292');">Buy  </a>
                                  </div>
                                </div>
                              </div>
                            </td>
                          </tr>
                        </table>
                      </TD>
                      </TR>
                      <TR>
                        <TD  ALIGN="center" VALIGN="Middle" Class="FamilyPageTableCell" style="width: 200;   ">
                          <div class="pro_thum_outer">
                            <img src="Images/NoImage.gif" onMouseOut="javascript:Moutimgtag('12733');" onMouseOver="javascript:Moverimgtag('12733');"  style="max-height:50px;max-width:50px" />
                          </div>
                </td>
                <TD ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width:200;   " >CL158BU</TD>
                <TD ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width:200;   " >Blue</TD>
                <TD ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width:200;   " >8 Core Category-5e Cable</TD>
                <TD ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width:200;   " >25M</TD>
                <TD  ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width:200;  ">
                  <a href="productdetails.aspx?pid=12733&fid=10793&cid=WES908P&path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT0T1bsnnmNMugVJxFQPC6pgHzEp0wrrdomihnz0Py6cTfaJ7M51bMHCeYOIYN2kI1k%3d" class="tx_3">See More Details </a>
                </TD>
                <TD ALIGN="center" VALIGN="Middle"  Class="costable" style="width: 200px;cursor:pointer;border-color: -moz-use-text-color #E8E8E8 #E8E8E8 -moz-use-text-color;border-style: none solid solid none;border-width: medium 1px 1px medium;border-color:#E8E8E8;   " >
                  <div class="pricepopup">
                    <div id="pid12733" class="popupouterdiv2none">
                      <div class="popupaero"></div>
                      <table  class="table table-striped  table-bordered table-condensed" style="margin:0; background:#FFF">
                        <tr class="success">
                          <td width="28%">ORDER CODE:</td>
                          <td colspan="2">STOCK STATUS</td>
                        </tr>
                        <tr>
                          <td width="28%">CL158BU</td>
                          <td colspan="2">
                            <span >INSTOCK</span>
                            <br>
                          </td>
                        </tr>
                        <tr class="success">
                          <td>Qty</td>
                          <td width="38%">Cost Inc GST</td>
                          <td width="34%">Cost Ex GST</td>
                        </tr>
                        <tr>
                          <td class="bg_grey3">1+</td>
                          <td  class="bg_grey3" align="center">$18.43</td>
                          <td class="bg_grey3" align="center">$16.75</td>
                        </tr>
                        <tr>
                          <td class="bg_grey3">10+</td>
                          <td  class="bg_grey3" align="center">$16.58</td>
                          <td class="bg_grey3" align="center">$15.07</td>
                        </tr>
                        <tr>
                          <td class="bg_grey3">100+</td>
                          <td  class="bg_grey3" align="center">$14.74</td>
                          <td class="bg_grey3" align="center">$13.40</td>
                        </tr>
                        <tr>
                          <td class="bg_grey3">500+</td>
                          <td  class="bg_grey3" align="center">$13.82</td>
                          <td class="bg_grey3" align="center">$12.56</td>
                        </tr>
                        <tr>
                          <td class="bg_grey3">RRP</td>
                          <td  class="bg_grey3" align="center">$27.65</td>
                          <td class="bg_grey3" align="center">$25.14</td>
                        </tr>
                        <tr>
                          <td class="bg_grey31">Your Price</td>
                          <td  class="bg_grey31" align="center">$16.58</td>
                          <td class="bg_grey31" align="center">$15.07</td>
                        </tr>
                      </table>
                    </div>
                    <a class="poppricenone" onMouseOut="javascript:Moutstockstatus('12733');" onMouseOver="javascript:Moverstockstatus('12733');" style="text-decoration:none;">
                      $ 15.07  <br/> Price / Stock Status
                    </a>
                    </Div>
                </TD>
                <TD ALIGN="Center" VALIGN="Middle"  Class="FamilyPageTableCell"  style="width: 120px;  ">
                  <table width="100px" border="0" cellPadding="0" cellSpacing="0">
                    <tr>
                      <td>
                        <div>
                          <input valign="middle" name="txt12733_100_1_10793" onkeydown="return keyct(event)"  maxlength="6" type="text" size="1" id="txt12733_100_1_10793" style="background-color:#FFFFFF;color: #000000;width: 30px;height:21px;float:left;"/>
                          <div class="costable">
                            <div class="pricepopup">
                              <div class="popupouterdivnone" id="popupouterdiv12733">
                                <div class="popupaero"></div>
                                <table  class="table table-striped  table-bordered table-condensed" style="margin:0; background:#FFF">
                                  <tr class="success">
                                    <td width="28%">ORDER CODE:</td>
                                    <td colspan="2">STOCK STATUS</td>
                                  </tr>
                                  <tr>
                                    <td width="28%">CL158BU</td>
                                    <td colspan="2">
                                      <span >INSTOCK</span>
                                      <br>
                                    </td>
                                  </tr>
                                  <tr class="success">
                                    <td>Qty</td>
                                    <td width="38%">Cost Inc GST</td>
                                    <td width="34%">Cost Ex GST</td>
                                  </tr>
                                  <tr>
                                    <td class="bg_grey3">1+</td>
                                    <td  class="bg_grey3" align="center">$18.43</td>
                                    <td class="bg_grey3" align="center">$16.75</td>
                                  </tr>
                                  <tr>
                                    <td class="bg_grey3">10+</td>
                                    <td  class="bg_grey3" align="center">$16.58</td>
                                    <td class="bg_grey3" align="center">$15.07</td>
                                  </tr>
                                  <tr>
                                    <td class="bg_grey3">100+</td>
                                    <td  class="bg_grey3" align="center">$14.74</td>
                                    <td class="bg_grey3" align="center">$13.40</td>
                                  </tr>
                                  <tr>
                                    <td class="bg_grey3">500+</td>
                                    <td  class="bg_grey3" align="center">$13.82</td>
                                    <td class="bg_grey3" align="center">$12.56</td>
                                  </tr>
                                  <tr>
                                    <td class="bg_grey3">RRP</td>
                                    <td  class="bg_grey3" align="center">$27.65</td>
                                    <td class="bg_grey3" align="center">$25.14</td>
                                  </tr>
                                  <tr>
                                    <td class="bg_grey31">Your Price</td>
                                    <td  class="bg_grey31" align="center">$16.58</td>
                                    <td class="bg_grey31" align="center">$15.07</td>
                                  </tr>
                                </table>
                              </div>
                              <a style="cursor:pointer;margin: 0 0 0 5px;" onMouseOut="javascript:Mouseout('12733');" onMouseOver="javascript:test('12733');"  id="12733" valign="middle" class="btnbuy2 button smallsiz btngreen costable"  onClick="productbuy('txt12733_100_1_10793','12733');">Buy  </a>
                            </div>
                          </div>
                        </div>
                      </td>
                    </tr>
                  </table>
                </TD>
                </TR>
                <TR>
                  <TD  ALIGN="center" VALIGN="Middle" Class="FamilyPageTableCell" style="width: 200;   ">
                    <div class="pro_thum_outer">
                      <img src="Images/NoImage.gif" onMouseOut="javascript:Moutimgtag('12736');" onMouseOver="javascript:Moverimgtag('12736');" style="max-height:50px;max-width:50px" />
                    </div>
          </td>
          <TD ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width: 200;   " >CL161BU</TD>
          <TD ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width: 200;   " >Blue</TD>
          <TD ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width: 200;   " >8 Core Category-5e Cable</TD>
          <TD ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width: 200;   " >50M</TD>
          <TD  ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width:200;   ">
            <a href="productdetails.aspx?pid=12736&fid=10793&cid=WES908P&path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT0T1bsnnmNMugVJxFQPC6pgHzEp0wrrdomihnz0Py6cTfaJ7M51bMHCeYOIYN2kI1k%3d" class="tx_3">See More Details </a>
          </TD>
          <TD ALIGN="center" VALIGN="Middle"  Class="costable" style="width: 200;cursor:pointer;border-color: -moz-use-text-color #E8E8E8 #E8E8E8 -moz-use-text-color;border-style: none solid solid none;border-width: medium 1px 1px medium;border-color:#E8E8E8;   " >
            <div class="pricepopup">
              <div id="pid12736" class="popupouterdiv2none">
                <div class="popupaero"></div>
                <table  class="table table-striped  table-bordered table-condensed" style="margin:0; background:#FFF">
                  <tr class="success">
                    <td width="28%">ORDER CODE:</td>
                    <td colspan="2">STOCK STATUS</td>
                  </tr>
                  <tr>
                    <td width="28%">CL161BU</td>
                    <td colspan="2">
                      <span >INSTOCK</span>
                      <br>
                    </td>
                  </tr>
                  <tr class="success">
                    <td>Qty</td>
                    <td width="38%">Cost Inc GST</td>
                    <td width="34%">Cost Ex GST</td>
                  </tr>
                  <tr>
                    <td class="bg_grey3">1+</td>
                    <td  class="bg_grey3" align="center">$30.75</td>
                    <td class="bg_grey3" align="center">$27.95</td>
                  </tr>
                  <tr>
                    <td class="bg_grey3">10+</td>
                    <td  class="bg_grey3" align="center">$27.66</td>
                    <td class="bg_grey3" align="center">$25.15</td>
                  </tr>
                  <tr>
                    <td class="bg_grey3">100+</td>
                    <td  class="bg_grey3" align="center">$24.60</td>
                    <td class="bg_grey3" align="center">$22.36</td>
                  </tr>
                  <tr>
                    <td class="bg_grey3">500+</td>
                    <td  class="bg_grey3" align="center">$23.06</td>
                    <td class="bg_grey3" align="center">$20.96</td>
                  </tr>
                  <tr>
                    <td class="bg_grey3">RRP</td>
                    <td  class="bg_grey3" align="center">$46.09</td>
                    <td class="bg_grey3" align="center">$41.90</td>
                  </tr>
                  <tr>
                    <td class="bg_grey31">Your Price</td>
                    <td  class="bg_grey31" align="center">$27.66</td>
                    <td class="bg_grey31" align="center">$25.15</td>
                  </tr>
                </table>
              </div>
              <a class="poppricenone" onMouseOut="javascript:Moutstockstatus('12736');" onMouseOver="javascript:Moverstockstatus('12736');" style="text-decoration:none;">
                $ 29.25  <br/> Price / Stock Status
              </a>
              </Div>
          </TD>
          <TD ALIGN="Center" VALIGN="Middle"  Class="FamilyPageTableCell"  style="width: 120px; ">
            <table width="100px" border="0" cellPadding="0" cellSpacing="0">
              <tr>
                <td>
                  <div>
                    <input valign="middle" name="txt12736_100_1_10793" onkeydown="return keyct(event)"  maxlength="6" type="text" size="1" id="txt12736_100_1_10793" style="background-color:#FFFFFF;color: #000000;width: 30px;height:21px;float:left;"/>
                    <div class="costable">
                      <div class="pricepopup">
                        <div class="popupouterdivnone" id="popupouterdiv12736">
                          <div class="popupaero"></div>
                          <table  class="table table-striped  table-bordered table-condensed" style="margin:0; background:#FFF">
                            <tr class="success">
                              <td width="28%">ORDER CODE:</td>
                              <td colspan="2">STOCK STATUS</td>
                            </tr>
                            <tr>
                              <td width="28%">CL161BU</td>
                              <td colspan="2">
                                <span >INSTOCK</span>
                                <br>
                              </td>
                            </tr>
                            <tr class="success">
                              <td>Qty</td>
                              <td width="38%">Cost Inc GST</td>
                              <td width="34%">Cost Ex GST</td>
                            </tr>
                            <tr>
                              <td class="bg_grey3">1+</td>
                              <td  class="bg_grey3" align="center">$30.75</td>
                              <td class="bg_grey3" align="center">$27.95</td>
                            </tr>
                            <tr>
                              <td class="bg_grey3">10+</td>
                              <td  class="bg_grey3" align="center">$27.66</td>
                              <td class="bg_grey3" align="center">$25.15</td>
                            </tr>
                            <tr>
                              <td class="bg_grey3">100+</td>
                              <td  class="bg_grey3" align="center">$24.60</td>
                              <td class="bg_grey3" align="center">$22.36</td>
                            </tr>
                            <tr>
                              <td class="bg_grey3">500+</td>
                              <td  class="bg_grey3" align="center">$23.06</td>
                              <td class="bg_grey3" align="center">$20.96</td>
                            </tr>
                            <tr>
                              <td class="bg_grey3">RRP</td>
                              <td  class="bg_grey3" align="center">$46.09</td>
                              <td class="bg_grey3" align="center">$41.90</td>
                            </tr>
                            <tr>
                              <td class="bg_grey31">Your Price</td>
                              <td  class="bg_grey31" align="center">$27.66</td>
                              <td class="bg_grey31" align="center">$25.15</td>
                            </tr>
                          </table>
                        </div>
                        <a style="cursor:pointer;margin: 0 0 0 5px;" onMouseOut="javascript:Mouseout('12736');" onMouseOver="javascript:test('12736');"  id="12736" valign="middle" class="btnbuy2 button smallsiz btngreen costable"  onClick="productbuy('txt12736_100_1_10793','12736');">Buy  </a>
                      </div>
                    </div>
                  </div>
                </td>
              </tr>
            </table>
          </TD>
          </TR>
          <TR>
            <TD  ALIGN="center" VALIGN="Middle" Class="FamilyPageTableCell" style="width: 200;   ">
              <div class="pro_thum_outer">
                <img src="Images/NoImage.gif" onMouseOut="javascript:Moutimgtag('15435');" onMouseOver="javascript:Moverimgtag('15435');"  style="max-height:50px;max-width:50px" />
              </div>
              </td>
              <TD ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width:200;   " >EL971</TD>
              <TD ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width:200;   " ></TD>
              <TD ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width:200;   " >Door Strike</TD>
              <TD ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width:200;   " ></TD>
              <TD  ALIGN="center" VALIGN="Middle"  Class="FamilyPageTableCell" style="width:200;  ">
                <a href="productdetails.aspx?pid=15435&fid=10793&cid=WES908P&path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT0T1bsnnmNMugVJxFQPC6pgHzEp0wrrdomihnz0Py6cTfaJ7M51bMHCeYOIYN2kI1k%3d" class="tx_3">See More Details </a>
              </TD>
              <TD ALIGN="center" VALIGN="Middle"  Class="costable" style="width: 200px;cursor:pointer;border-color: -moz-use-text-color #E8E8E8 #E8E8E8 -moz-use-text-color;border-style: none solid solid none;border-width: medium 1px 1px medium;border-color:#E8E8E8;   " >
                <div class="pricepopup">
                  <div id="pid15435" class="popupouterdiv2none">
                    <div class="popupaero"></div>
                    <table  class="table table-striped  table-bordered table-condensed" style="margin:0; background:#FFF">
                      <tr class="success">
                        <td width="28%">ORDER CODE:</td>
                        <td colspan="2">STOCK STATUS</td>
                      </tr>
                      <tr>
                        <td width="28%">EL971</td>
                        <td colspan="2">
                          <span >INSTOCK</span>
                          <br>
                        </td>
                      </tr>
                      <tr class="success">
                        <td>Qty</td>
                        <td width="38%">Cost Inc GST</td>
                        <td width="34%">Cost Ex GST</td>
                      </tr>
                      <tr>
                        <td class="bg_grey3">1+</td>
                        <td  class="bg_grey3" align="center">$25.85</td>
                        <td class="bg_grey3" align="center">$23.50</td>
                      </tr>
                      <tr>
                        <td class="bg_grey3">10+</td>
                        <td  class="bg_grey3" align="center">$23.27</td>
                        <td class="bg_grey3" align="center">$21.15</td>
                      </tr>
                      <tr>
                        <td class="bg_grey3">RRP</td>
                        <td  class="bg_grey3" align="center">$39.00</td>
                        <td class="bg_grey3" align="center">$35.45</td>
                      </tr>
                      <tr>
                        <td class="bg_grey31">Your Price</td>
                        <td  class="bg_grey31" align="center">$23.27</td>
                        <td class="bg_grey31" align="center">$21.15</td>
                      </tr>
                    </table>
                  </div>
                  <a class="poppricenone" onMouseOut="javascript:Moutstockstatus('15435');" onMouseOver="javascript:Moverstockstatus('15435');" style="text-decoration:none;">
                    $ 21.15  <br/> Price / Stock Status
                  </a>
                  </Div>
              </TD>
              <TD ALIGN="Center" VALIGN="Middle"  Class="FamilyPageTableCell"  style="width: 120px;  ">
                <table width="100px" border="0" cellPadding="0" cellSpacing="0">
                  <tr>
                    <td>
                      <div>
                        <input valign="middle" name="txt15435_100_1_10793" onkeydown="return keyct(event)"  maxlength="6" type="text" size="1" id="txt15435_100_1_10793" style="background-color:#FFFFFF;color: #000000;width: 30px;height:21px;float:left;"/>
                        <div class="costable">
                          <div class="pricepopup">
                            <div class="popupouterdivnone" id="popupouterdiv15435">
                              <div class="popupaero"></div>
                              <table  class="table table-striped  table-bordered table-condensed" style="margin:0; background:#FFF">
                                <tr class="success">
                                  <td width="28%">ORDER CODE:</td>
                                  <td colspan="2">STOCK STATUS</td>
                                </tr>
                                <tr>
                                  <td width="28%">EL971</td>
                                  <td colspan="2">
                                    <span >INSTOCK</span>
                                    <br>
                                  </td>
                                </tr>
                                <tr class="success">
                                  <td>Qty</td>
                                  <td width="38%">Cost Inc GST</td>
                                  <td width="34%">Cost Ex GST</td>
                                </tr>
                                <tr>
                                  <td class="bg_grey3">1+</td>
                                  <td  class="bg_grey3" align="center">$25.85</td>
                                  <td class="bg_grey3" align="center">$23.50</td>
                                </tr>
                                <tr>
                                  <td class="bg_grey3">10+</td>
                                  <td  class="bg_grey3" align="center">$23.27</td>
                                  <td class="bg_grey3" align="center">$21.15</td>
                                </tr>
                                <tr>
                                  <td class="bg_grey3">RRP</td>
                                  <td  class="bg_grey3" align="center">$39.00</td>
                                  <td class="bg_grey3" align="center">$35.45</td>
                                </tr>
                                <tr>
                                  <td class="bg_grey31">Your Price</td>
                                  <td  class="bg_grey31" align="center">$23.27</td>
                                  <td class="bg_grey31" align="center">$21.15</td>
                                </tr>
                              </table>
                            </div>
                            <a style="cursor:pointer;margin: 0 0 0 5px;" onMouseOut="javascript:Mouseout('15435');" onMouseOver="javascript:test('15435');"  id="15435" valign="middle" class="btnbuy2 button smallsiz btngreen costable"  onClick="productbuy('txt15435_100_1_10793','15435');">Buy  </a>
                          </div>
                        </div>
                      </div>
                    </td>
                  </tr>
                </table>
              </TD>
          </TR>
          </TABLE>
          </div>
          </td>
        </tr>
      </table>
      </div>
      </div>
      <p align="center">
      </p>
      </div>

      </div>
      </div >

      </div>
      <div id="testdiv" class="ppdivtest" ></div>
      <div id="testdivimg" class="testdivimgcss" ></div>
      <input type="hidden" name="ctl00$maincontent$ctl00$hffid" id="ctl00_maincontent_ctl00_hffid" value="10793" />
      </td>
      </tr>
      </table>
      </td>
      </tr>
      </table>
      </td>
      </tr>
      </table>
      </td>
      </tr>
      <tr>
        <td style="border-style: none">

          <div class="container_cmn" >
            <div class="container12">
              <div class="span12 box2" style="margin-left:6px;width: 946px;">
                <div class="fourbox">
                  <h3 class="title5">PRODUCT CATEGORIES</h3>
                  <ul class="list1" style="list-style: none outside none;border: 0 none;font-size: 100%;margin: 0;padding: 0;">

                    <li class="default">
                      <a href="categorylist.aspx?&ld=0&cid=WES-04D&byp=1"  target="_top">Audio Components</a>
                    </li>
                    <li class="default">
                      <a href="categorylist.aspx?&ld=0&cid=WES-08H&byp=1"  target="_top">Automotive</a>
                    </li>
                    <li class="default">
                      <a href="categorylist.aspx?&ld=0&cid=WES-02B&byp=1"  target="_top">AV TV SAT Installation</a>
                    </li>
                    <li class="default">
                      <a href="categorylist.aspx?&ld=0&cid=WES-03C&byp=1"  target="_top">Cables & Connectors</a>
                    </li>
                    <li class="default">
                      <a href="categorylist.aspx?&ld=0&cid=WES0830&byp=1"  target="_top">Cellular Accessories</a>
                    </li>
                    <li class="default">
                      <a href="categorylist.aspx?&ld=0&cid=WES-06F&byp=1"  target="_top">Communication Accessories</a>
                    </li>
                    <li class="default">
                      <a href="categorylist.aspx?&ld=0&cid=WES-05E&byp=1"  target="_top">DATA & Computer</a>
                    </li>
                    <li class="default">
                      <a href="categorylist.aspx?&ld=0&cid=WES-11K&byp=1"  target="_top">Electronic Components</a>
                    </li>
                    <li class="default">
                      <a href="categorylist.aspx?&ld=0&cid=WES-01A&byp=1"  target="_top">Power & Lighting</a>
                    </li>
                    <li class="default">
                      <a href="categorylist.aspx?&ld=0&cid=RC1&byp=1"  target="_top">ROOTCAT1</a>
                    </li>
                    <li class="default">
                      <a href="categorylist.aspx?&ld=0&cid=RC2&byp=1"  target="_top">ROOTCAT2</a>
                    </li>
                    <li class="default">
                      <a href="categorylist.aspx?&ld=0&cid=RC4&byp=1"  target="_top">ROOTCAT4</a>
                    </li>
                    <li class="default">
                      <a href="categorylist.aspx?&ld=0&cid=WES-07G&byp=1"  target="_top">Security</a>
                    </li>
                    <li class="default">
                      <a href="categorylist.aspx?&ld=0&cid=WES-12L&byp=1"  target="_top">Service & Repair Parts</a>
                    </li>
                    <li class="default">
                      <a href="categorylist.aspx?&ld=0&cid=WES-10J&byp=1"  target="_top">Tools & Test</a>
                    </li>
                  </ul>
                </div>
                <div class="fourbox">
                  <h3 class="title5">MY ACCOUNT</h3>
                  <ul class="list1" style="list-style: none outside none;border: 0 none;font-size: 100%;margin: 0;padding: 0;">
                    <li >
                      <a href="EditUserProfile.aspx" >View Profile</a>
                    </li>
                    <li >
                      <a href="changePassword.aspx" >Change Password</a>
                    </li>
                    <li >
                      <a href="changeUsername.aspx" >Change Username</a>
                    </li>
                    <li >
                      <!--<a href="Orders.aspx?ViewType=HISTORY" >Order History</a>-->
                      <a href="OrderHistory.aspx" >Order History</a>

                    </li>

                  </ul>
                </div>
                <div class="fourbox">
                  <h3 class="title5">COMPANY INFO</h3>
                  <ul class="list1" style="list-style: none outside none;border: 0 none;font-size: 100%;margin: 0;padding: 0;">
                    <li >
                      <a href="aboutus.aspx" class="tx_4">About Us</a>
                    </li>
                    <li >
                      <a href="contactus.aspx" class="tx_4">Contact Us</a>
                    </li>
                    <li >
                      <a href="Termsandconditions.aspx" class="tx_4">Terms and Conditions</a>
                    </li>
                    <li >
                      <a href="privacepolicy.aspx" class="tx_4">Privacy Policy</a>
                    </li>
                    <li >
                      <a href="CatalogueDownload.aspx?ActionResult=CATALOGUE" class="tx_4">Catalogue Download</a>
                    </li>
                    <li >
                      <a href="CatalogueDownload.aspx?ActionResult=NEWS" class="tx_4">News Letter Download</a>
                    </li>
                  </ul>
                </div>
              </div>
              <div style="color: rgb(241, 241, 241);" align="left">&nbsp;&nbsp;V4.7</div>
            </div>

          </div >
        </td>
      </tr>
      </table>
      <script src="Scripts/All_JQ_FamilyPage.js" type="text/javascript"></script>
      <script src="Scripts/All_K_button.js" type="text/javascript"></script>

      <script type="text/javascript">
        var _gaq = _gaq || [];
        _gaq.push(['_setAccount', 'UA-38373387-1']);
        _gaq.push(['_trackPageview']);

        (function () {
        var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
        ga.src = ('https:' == document.location.protocol ? 'https://' : 'http://') + 'stats.g.doubleclick.net/dc.js';
        var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
        })();
      </script>
      <script type="text/javascript">
        var fby = fby || [];
        fby.push(['showTab', { id: '7210', position: 'right', color: '#FF1F3A'}]);
        (function () {
        var f = document.createElement('script'); f.type = 'text/javascript'; f.async = true;
        f.src = '//cdn.feedbackify.com/f.js';
        var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(f, s);
        })();

      </script>
    </form>
  </body>
</html>
}