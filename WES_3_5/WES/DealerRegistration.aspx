﻿<%@ Page Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" Inherits="DealerRegistration"
     $find('testTACpopup').hide();
 }
                                                                                                                                                                                                                                                                                                                                                                                                                                 $find('testTACpopup').hide();
                                                                                                                                                                                                                                                                                                                                                                                                                             }
                                                                                                                                                                                                                                                                                                                                                                                                                                 $('.input_dr').keyup(function (ev) {
                                                                                                                                                                                                                                                                                                                                                                                                                                     var value = $(this).val();
                                                                                                                                                                                                                                                                                                                                                                                                                                 })
                                                                                                                                                                                                                                                                                                                                                                                                                             });</script> <script language="javascript" type="text/javascript">                                                                                                                                                                                                                                                                                                                                                                                                                             function checkEnableSubmit() {
     if (checkselectedlist() == true && (document.getElementById("ctl00_maincontent_chkterms").checked == true)) {
         document.getElementById("ctl00_maincontent_lblCheckTerms").childNodes[0].nodeValue = "*";
     }
         document.getElementById("ctl00_maincontent_chkterms").checked = false;
     }
         document.getElementById("ctl00_maincontent_lblCheckTerms").childNodes[0].nodeValue = "Required";
     }
 }
                                                                                                                                                                                                                                                                                                                                                                                                                                 if ((document.getElementById("ctl00_maincontent_chkother").checked == true) || (document.getElementById("ctl00_maincontent_chkother").checked == 'true') || (document.getElementById("ctl00_maincontent_chkother").checked == 1)) {
                                                                                                                                                                                                                                                                                                                                                                                                                                     document.getElementById("ctl00_maincontent_txtothers").disabled = false;
                                                                                                                                                                                                                                                                                                                                                                                                                                 }
                                                                                                                                                                                                                                                                                                                                                                                                                                     document.getElementById("ctl00_maincontent_txtothers").disabled = true;
                                                                                                                                                                                                                                                                                                                                                                                                                                 }
                                                                                                                                                                                                                                                                                                                                                                                                                             }
                                                                                                                                                                                                                                                                                                                                                                                                                                 if ((document.getElementById("ctl00_maincontent_chkautomotive").checked == true) || (document.getElementById("ctl00_maincontent_chkengineer").checked == true) || (document.getElementById("ctl00_maincontent_chkgovernment").checked == true) ||
                                                                                                                                                                                                                                                                                                                                                                                                                                     document.getElementById("ctl00_maincontent_ErrorStatusHiddenField").value = "1";
                                                                                                                                                                                                                                                                                                                                                                                                                                 }
                                                                                                                                                                                                                                                                                                                                                                                                                                     document.getElementById("ctl00_maincontent_ErrorStatusHiddenField").value = "0";
                                                                                                                                                                                                                                                                                                                                                                                                                                 }
                                                                                                                                                                                                                                                                                                                                                                                                                             }
                                                                                                                                                                                                                                                                                                                                                                                                                                 if (checkselectedlist() == true) {
                                                                                                                                                                                                                                                                                                                                                                                                                                     if (document.getElementById("ctl00_maincontent_chkterms").checked == true) {
                                                                                                                                                                                                                                                                                                                                                                                                                                         document.getElementById("ctl00_maincontent_lblCheckTerms").style.display = '';
                                                                                                                                                                                                                                                                                                                                                                                                                                     }
                                                                                                                                                                                                                                                                                                                                                                                                                                         document.getElementById("ctl00_maincontent_lblCheckTerms").style.display = '';
                                                                                                                                                                                                                                                                                                                                                                                                                                     }
                                                                                                                                                                                                                                                                                                                                                                                                                                 }
                                                                                                                                                                                                                                                                                                                                                                                                                                     document.getElementById("ctl00_maincontent_lblCheckTerms").style.display = '';
                                                                                                                                                                                                                                                                                                                                                                                                                                 }
                                                                                                                                                                                                                                                                                                                                                                                                                             }</script> <script language="javascript" type="text/javascript">                                                                                                                                                                                                                                                                                                                                                                                                                             function check(e) {
     var keynum
         keynum = e.keyCode
     }
         keynum = e.which
     }
     //                var keyCodeValue = (document.all?window.event.keyCode:e.which);                       
         e.keyCode = '';
     }
         //                else if (keynum == 39) {        
         //                        
         //                        if (e.keyCode) { //IE
         //                          if (keyCodeValue == "112") {
         //                            document.onhelp = function() { return (false); }
         //                            window.onhelp = function() { return (false); }
         //                          }
         //                          e.returnValue = false;
         //                          e.keyCode = 0;
         //                        }else{  //not IE
         //                          e.preventDefault();
         //                          e.stopPropagation();
         //                        } 
         //                
         //                    if (e.keyCode) {
         //                      e.keyCode=96;
         //                      return e.keyCode;
         //                    }else if (e.which) {
         //                      e.which=96; 
         //                      return e.which;
         //                    }
         //                }
         return true;
     }
 }</script>
    
    <style>
        .modalBackgroundpopup {
              top: 0 !important;
                bottom: 0 !important;
                left: 0 !important;
                right: 0 !important;
                margin: auto;
        }
    </style>
    
     <script language="javascript" type="text/javascript">                  function Numbersonly(e) {
     var keynum
         keynum = e.keyCode
     }
         keynum = e.which
     }
         return true;
     }
         return false;
     }
 }</script> <script language="javascript" type="text/javascript">                function Email(e) {
     var keynum;
     }
         keynum = e.which;
     }
     }
         return true;
     }
 } </script> <asp:Panel ID="pnlnewProfile" runat="server" DefaultButton="btnsubmit"> <table width="790px" border="0" cellpadding="0" cellspacing="0"> <tr> <td align="left" style="text-align:left;margin-left:0px;"    > <%-- <div class="container">--%> <%--<div class="container12" style="width: 754px;">--%> <div class="span9 box1" style="width: 760px;margin-left:0px;"> <h3 class="title1" align="left">New Customer Sign Up</h3> <div class="img_left"> <img width="106" height="80" alt="img" src="images/page_icon1.png"> </div> <div class="box4"> <p class="p3">New Customer sign form. Please enter all the details below and submit form to us for processing.</p> <p class="p3">After you have submitted the sign up form our Sales Team willl check and verify your details are correct. </p> <p class="p3">Once approved you will receive a confirmation email from us with further information.</p> <p class="p3">Questions? Contact us between the hours of 8:30 a.m. and 5:00 p.m. Monday - Friday<br> Email: <a href="mailto:sales@wes.net.au">sales@wes.net.au</a> or Phone: +61 2 9797-9866 <br> <br> Please Fill Form in Below </p> </div> <table cellpadding="0" cellspacing="0" border="0" width="588px" valign="top"> <%--<tr> <td align="left"> <asp:Label ID="Label2" runat="server" Class="lblRequiredSkin" Visible="true" Text="*"
                                                        <asp:CheckBox ID="chkelectrical" Text="Electrical / Electrician" runat="server"
                                                             Checked="false" />
                                                    </td> <td align="left" style="font-size: 12px;" > <input id="chkother" runat="server" type="checkbox" onchange="javascript:checkEnableOthers()" /> Other, Please Specify </td> <td align="left"> <asp:TextBox runat="server" ID="txtothers" Text=""  class="input_dr" style="width: 266px;height: 18px; "