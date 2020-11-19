<%@ Page Title="" Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" CodeBehind="PaySP.aspx.cs" Inherits="PaySP" Culture="en-US" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
       <style>
       .tryagain {
               float: none;
   width: 132px;
    height: 37px;
    border: none;
    display: inline-block;
background: #48B335;
text-transform: initial;
color: #fff;
font-size: 16px;
font-weight: bold;
text-align: center;
border-radius: 6px;
float: right;
cursor: pointer;
padding-bottom: 2px;
}
       


.creditcard_wrap {
    background-color: #fff;
    border: 1px solid #b5b5b5;
    border-radius: 4px;
    display: block;
    margin: 0;
    max-height: 500px;
    transition: transform .3s,opacity .3s,max-height .3s ease;
    width: 100%;
	font-family: Arial;
}
.creditcard_wrap_header {
    align-items: center;
    border-bottom: 1px solid #b5b5b5;
    display: flex;
    flex-wrap: wrap;
    padding: 12px 15px 0 12px;
    position: relative;
}
.creditcard_wrap_header .creditcard_header-label {
    align-items: center;
    display: flex;
    flex-grow: 1;
    padding-bottom: 12px;
}
.creditcard_wrap_header .creditcard_header_cardIcon {
    align-items: center;
    display: flex;
    text-align: center;
    width: 50px;
}
.creditcard_header_cardIcon img, .creditcard_header_rgtIcons img {
	max-height:26px;
}
.creditcard_wrap_header .creditcard_header_title {
    color: #000;
    font-size: 18px;
    margin-left: 20px;
}
.creditcard_wrap_header .creditcard_header_rgtIcons {
    padding-bottom: 10px;
}
.creditcard_form_body {
    padding: 10px 15px 10px 10px;
	width:60%;
}
.creditcard_form_body .creditcard_form_group {
    margin-bottom: 12px;
    padding-left: 5px;
}
.creditcard_form_body .creditcard_form_group .creditcard_form_label {
    color: #000;
    display: block;
    font-size: 14px;
    font-weight: 400;
    line-height: 1.4;
    margin: 0;
    padding: 0;
    text-align: left;
}

.creditcard_form_body .creditcard_form_group .creditcard_form_field {
    position: relative;
}
.creditcard_form_body .creditcard_form_group .creditcard_form_field .creditcard_form_text_field {
    border: 1px solid #bfbfbf;
    height: 44px;
    margin: 4px 0 0;
    padding: 0 0px;
}
.creditcard_form_body .creditcard_form_group .creditcard_form_field .creditcard_form_text_field.bbcol {
    border-color:#ca2a2a !important;
}
.creditcard_form_body .creditcard_form_group .creditcard_form_field .creditcard_form_text_field.bbcol:focus, .creditcard_form_body .creditcard_form_group .creditcard_form_field .creditcard_form_text_field.bbcol:hover {
    border-color:#ca2a2a !important;
}
.creditcard_form_body .creditcard_form_group .creditcard_form_field .creditcard_form_text_field:hover, .creditcard_form_body .creditcard_form_group .creditcard_form_field .creditcard_form_text_field:focus {
    border-color: #7d7d7d;
}
.creditcard_form_body .creditcard_form_group .creditcard_form_field .creditcard_form_text_field input.creditcard_form_input {
    border: none;
    background-image: none;
    background-color: transparent;
    box-shadow: none;
    width: 98%;
    height: 100%;
    font-size: 16px;
    font-family:"Arial",sans-serif;
    color: #000;
    padding:0 1%;
}
.creditcard_form_body .creditcard_form_group .creditcard_form_error {
    color: #ca2a2a;
   
    font-size: 13px;
    line-height: 1.4;
    margin: 5px 0;
    padding: 0;
}
.creditcard_form_body .creditcard_form_multiColumn {
    display: flex;
    flex-wrap: wrap;
    justify-content: space-between;
}
.creditcard_form_body .creditcard_form_group .creditcard_form_descriptor {
    color: #b5b5b5;
    font-size: 13px;
    margin-left: 6px;
}
.creditcard_form_body .creditcard_form_multiColumn .creditcard_form_group {
    flex-basis: 190px;
    flex-grow: 1;
}

.creditcard_form_body .creditcard_form_group .creditcard_form_field .creditcard_form_icon_container {
    margin-top: -14px;
    position: absolute;
    right: 11px;
    top: 50%;
}
.creditcard_form_body .creditcard_form_field .creditcard_form_icon_container .creditcard_form_field-error-icon {
	display:none;
}

</style>
     <style type="text/css">
        .MessageBoxPopUp
        {
            background-color: White;
            border: solid 2px #99B4D1;
        }
        
        
        
        .MessageBoxButton
        {
            background-color: White;
            border: solid 2px #99B4D1;
            font-weight: bold;
            font-family: Verdana;
            font-size: 9pt;
            cursor: pointer;
            height: 20px;
            display: block;
        }
        
        
        
        .MessageBoxHeader
        {
            height: 17px;
            font-size: 10pt;
            color: White;
            font-weight: bold;
            font-family: Verdana;
            text-align: Left;
            vertical-align: middle;
            padding: 3px 3px 3px 3px;
            background-color: #3399FF;
            border-bottom: 2px solid #0099DA;
        }
        
        
        
        .MessageBoxData
        {
            height: 20px;
            font-size: 10pt;
            font-family: Verdana;
            color: #3A4349;
            text-align: Left;
            vertical-align: top;
        }
    </style>
    <%--<script language="JavaScript">
        var message = "Right click not allowed this page!";
        function clickIE4() {
            if (event.button == 2) {
                alert(message);
                return false;
            }
        }
        function clickNS4(e) {
            if (document.layers || document.getElementById && !document.all) {
                if (e.which == 2 || e.which == 3) {
                    alert(message);
                    return false;
                }
            }
        }

        if (document.layers) {
            document.captureEvents(Event.MOUSEDOWN);
            document.onmousedown = clickNS4;
        }
        else if (document.all && !document.getElementById) {
            document.onmousedown = clickIE4;
        }

        document.oncontextmenu = new Function("alert(message);return false")

    </script>--%>

    <script language="JavaScript">
        document.onkeypress = function (event) {
            event = (event || window.event);
            if (event.keyCode == 123) {
                return false;
            }
        }
        document.onmousedown = function (event) {
            event = (event || window.event);
            if (event.keyCode == 123) {
                return false;
            }
        }
        document.onkeydown = function (event) {
            event = (event || window.event);
            if (event.keyCode == 123) {
                return false;
            }
        }


        function Setinit_paypal() {
            var x = document.getElementById('<%= btnPay_paypal.ClientID %>');
             var y = document.getElementById('<%= BtnProgress_paypal.ClientID %>');
            // var z = document.getElementById('<%= ImgBtnEditShipping.ClientID %>');
             x.style.display = "none";
             y.style.display = "block";
             y.style.visibility = "visible";            
            // z.style.display = "none";
            // z.style.visibility = "hidden";
             return true;
         }

        function creditcardclick() {
            document.getElementById("divcreditcard").style.display = 'block';
            document.getElementById("divdedault").style.display = 'none';

            document.getElementById("divpaypal").style.display = 'none';

            return false;


        }
        function paypalclick() {
            try {
                document.getElementById("divpaypal").style.display = 'block';

                document.getElementById("divdedault").style.display = 'none';
                document.getElementById("divcreditcard").style.display = 'none';
           
            }
            catch (error) {

            }
            return false;

        }

        function Defaultclick() {
            try {
                document.getElementById("divdedault").style.display = 'block';
                document.getElementById("divcreditcard").style.display = 'none';
                document.getElementById("divpaypal").style.display = 'none';
              

            }
            catch (error) {

            }
            return false;
        }
    </script>
    <%--var dd = document.getElementById('<%= drppaymentmethod.ClientID %>');
          if (dd != null) {
              CardType = dd.value;
          }
          if (CardType == 2)    
              cardno = /^(?:3[47][0-9]{13})$/;
          else if (CardType == 5)   
              cardno = /^(?:5[1-5][0-9]{14})$/;
          else if (CardType == 6) 
              cardno = /^(?:4[0-9]{12}(?:[0-9]{3})?)$/;              
          else
              return args.IsValid = false;--%>

     <%-- var ddl = document.getElementById('<%= drppaymentmethod.ClientID %>');
          if (ddl != null) {

              var opts = ddl.options.length;
              for (var i = 0; i < opts; i++) {
                  if (ddl.options[i].value == dval) {
                      ddl.options[i].selected = true;
                      break;
                  }
              }
          }--%>
<%--   if (ctype == "dd") {
            var dd = document.getElementById('<%= drppaymentmethod.ClientID %>');
            if (dd != null && dd.value == 0) {

                dd.style.border = "1px solid #FF0000";
            }
            else {
                dd.style.border = "";

            }
        }--%>
  <script type="text/ecmascript">
      function ValidCC(sender, args) {

          var cardno = '';
          var CardType = 0;
          AEcardno = /^(?:3[47][0-9]{13})$/;
          Mcardno = /^(?:5[1-5][0-9]{14})$/;
          Vcardno = /^(?:4[0-9]{12}(?:[0-9]{3})?)$/;


          var txt = document.getElementById('<%= txtnamecard.ClientID %>');

          if (txt.value.match(AEcardno) && mod10_check(txt.value) == true) {

              if (txt != null) {
                  txt.style.border = "";
              }
              if (CardType != 2) {
                  SetDropDownValue(2);
              }
              return args.IsValid = true;
          }
          else if (txt.value.match(Mcardno) && mod10_check(txt.value) == true) {

              if (txt != null) {
                  txt.style.border = "";
              }
              if (CardType != 5) {
                  SetDropDownValue(5);
              }
              return args.IsValid = true;
          }
          else if (txt.value.match(Vcardno) && mod10_check(txt.value) == true) {

              if (txt != null) {
                  txt.style.border = "";
              }
              if (CardType != 6) {
                  SetDropDownValue(6);
              }
              return args.IsValid = true;
          }
          else {
              if (txt != null) {
                  txt.style.border = "1px solid #FF0000";
              }

              if (CardType == 2) //Amercican Express	
              {
                  sender.innerHTML = "Not a valid Amercican Express credit card number!";
              }
              else if (CardType == 5)//Mastercard 	   
              {
                  sender.innerHTML = "Not a valid Mastercard card number!";
              }
              else if (CardType == 6) //Visa	 
              {
                  sender.innerHTML = "Not a valid Visa card number!";
              }
              else {
                  sender.innerHTML = "Not a valid card number!";
              }

              return args.IsValid = false;
          }
      }
      function SetDropDownValue(dval) {
        

      }
      function mod10_check(val) {
          var nondigits = new RegExp(/[^0-9]+/g);
          var number = val.replace(nondigits, '');
          var pos, digit, i, sub_total, sum = 0;
          var strlen = number.length;
          if (strlen < 13) { return false; }
          for (i = 0; i < strlen; i++) {
              pos = strlen - i;
              digit = parseInt(number.substring(pos - 1, pos));
              if (i % 2 == 1) {
                  sub_total = digit * 2;
                  if (sub_total > 9) {
                      sub_total = 1 + (sub_total - 10);
                  }
              } else {
                  sub_total = digit;
              }
              sum += sub_total;
          }
          if (sum > 0 && sum % 10 == 0) {
              return true;
          }
          return false;
      }
  </script>
    <script language="javascript" type="text/javascript">
         function validatenumerics_cc(key) {
           //getting key code of pressed key
           var keycode = (key.which) ? key.which : key.keyCode;
           //comparing pressed keycodes
           
                if (keycode > 31 && (keycode < 48 || keycode > 57)) {
                    console.log(" You can enter only characters 0 to 9 ");
                    return false;
                }
          
           else return true;


       }
        function validatenumerics(key) {
           //getting key code of pressed key
           var keycode = (key.which) ? key.which : key.keyCode;
           //comparing pressed keycodes
            if (keycode != 47) {
                if (keycode > 31 && (keycode < 48 || keycode > 57)) {
                    console.log(" You can enter only characters 0 to 9 ");
                    return false;
                }
            }
           else return true;


       }
        function Insertslash(e) {
            
        var x=    $('#<%=txtExpyear.ClientID%>').val();  
          
            if (x.length == 4 && x.includes("/") == false) {
           var y=x.substring(0,2)+"/"+x.substring(4,2)
                $('#<%=txtExpyear.ClientID%>').val(y);
            }
            
        }

  function InsertSpace_cc (e) {
            
         var x=    $('#<%=txtcreditcardno.ClientID%>').val();  
          
            if (x.length ==16 && x.includes(" ") == false) {
                var y = x.substring(0, 4) + " " + x.substring(4, 8) + " " + x.substring(8, 12) + " " + x.substring(12, 16);
                $('#<%=txtcreditcardno.ClientID%>').val(y);
      }
       else if (x.length == 15 && x.includes(" ") == false) {
                 var y = x.substring(0, 4) + " " + x.substring(4, 8) + " " + x.substring(8, 12) + " " + x.substring(12, 15);
                $('#<%=txtcreditcardno.ClientID%>').val(y);
            }
        }

      function InsertSpace (e) {
            
         var x=    $('#<%=txtcreditcardno.ClientID%>').val();  
          
            if (x.length == 4 || x.length==9 ||x.length==14 ) {
                $('#<%=txtcreditcardno.ClientID%>').val(x + " ");
          }
         
        }
        function Numbersonly(e) {
            var keynum
            var keychar
            var numcheck
            // For Internet Explorer
            if (window.event) {
                keynum = e.keyCode
            }
                // For Netscape/Firefox/Opera
            else if (e.which) {
                keynum = e.which
            }
            keychar = String.fromCharCode(keynum)
            //List of special characters you want to restrict
            if (keychar == "1" || keychar == "2" || keychar == "3" || keychar == "4" || keychar == "5" || keychar == "6" || keychar == "7" || keychar == "8" || keychar == "9" || keychar == "0") {

                return true;
            }
            else {
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        (function () {

            var DEBUG = true,
        EXPOSED_NS = 'ForTheCosumer';

            var myApp = function () {

                return {
                    DoSomething: function () { },
                    DoSomethingElse: function () { }
                }
            }();

            // expose my public methods
            window[EXPOSED_NS] = {
                doSomething: myApp.DoSomething,
                doSomethingElse: myApp.DoSomethingElse
            };

            if (DEBUG) {
                window.MyApp = myApp
            }
        }());
        //window.onload = func1;

        //function func1() {
        //    document.getElementById("r1").scrollIntoView();
        //}

        function Setinit() {
            var res = Page_ClientValidate();
            if (res == true) {

                document.getElementById("diexpimg").style.display = "none";
                  if (document.getElementById("ctl00_maincontent_txtExpyear").value.includes("/") == false) {
                      document.getElementById("divexpyear").classList.add("bbcol");
                     alert("Please Enter a valid expiry date");
                      return false;
                }
                var txtf = document.getElementById("ctl00_maincontent_txtExpyear").value.split("/");

                var today, someday;
                var exMonth = txtf[0];
                var exYear = "20" + txtf[1];
                today = new Date();
                someday = new Date();
              
                // alert(exYear);
                if (exMonth == "00"||exMonth == "0" || exMonth >12) {
                    alert("Please Enter a valid expiry date");
                     document.getElementById("divexpyear").classList.add("bbcol");
                      return false;
                }
                  else {
                     document.getElementById("divexpyear").classList.remove("bbcol");
                }
                someday.setFullYear(exYear, exMonth, 1);
                //alert(someday);
                // alert(today);
                if (someday < today) {
                    alert("Please Enter a valid expiry date");
                    document.getElementById("divexpyear").classList.add("bbcol");
                    return false;
                }
                else {
                     document.getElementById("divexpyear").classList.remove("bbcol");
                }
                  var x = document.getElementById('<%= btnSP.ClientID %>');
                   <%-- var y = document.getElementById('<%= BtnProgressSP.ClientID %>');--%>
                x.value = "Processing..";
                    document.getElementById("ctl00_maincontent_Def").style.display = "none";
               // __doPostBack('btnSP','OnClick');

               // x.click();
               // alert("x");
              // document.getElementById('<%= btnSP.ClientID %>').onclick();
                return true;
            <%--    var x = document.getElementById('<%= btnPay.ClientID %>');
            var y = document.getElementById('<%= BtnProgress.ClientID %>');
            var z = document.getElementById('<%= ImgBtnEditShipping.ClientID %>');--%>
            //x.style.display = "none";
            //y.style.display = "block";
            //y.style.visibility = "visible";
            //z.style.display = "block";
            //z.style.visibility = "visible";
            //z.style.display = "none";
            //    z.style.visibility = "hidden";
            //    document.getElementById("Def").style.display = "none";
        }
        else {
            //Controlvalidate('dd');
            //Controlvalidate('cno');
            //Controlvalidate('cn');
            //Controlvalidate('cvv');
                  var isv = 1;
              if (document.getElementById("ctl00_maincontent_txtcreditcardno").value == "") {

                document.getElementById("divcardno").classList.add("bbcol");
                  document.getElementById("divimgcardno").style.display = "block";
                     $("#<%=RBcreditcardno.ClientID%>").css("visibility", "visible");
                isv = 0;
            }
            else
            {
                document.getElementById("divcardno").classList.remove("bbcol");
                 document.getElementById("divimgcardno").style.display = "none";
                
                }
                if (document.getElementById("ctl00_maincontent_txtCVV").value == "") {

                document.getElementById("divcvv").classList.add("bbcol");
                    document.getElementById("diccvvimg").style.display = "block";
                         $("#<%=RBCVV.ClientID%>").css("visibility", "visible");
                isv = 0;
            }
            else
            {
                document.getElementById("divcvv").classList.remove("bbcol");
                 document.getElementById("diccvvimg").style.display = "none";
                
            }
            if (document.getElementById("ctl00_maincontent_txtExpyear").value == "") {

                document.getElementById("divexpyear").classList.add("bbcol");
                 document.getElementById("diexpimg").style.display = "block";
                isv = 0;
            }
            else
            {
               
              
                document.getElementById("diexpimg").style.display = "none";
                  if (document.getElementById("ctl00_maincontent_txtExpyear").value.includes("/") == false) {

                     alert("Please Enter a valid expiry date with /");
                      return false;
                }
                var txtf = document.getElementById("ctl00_maincontent_txtExpyear").value.split("/");

                var today, someday;
                var exMonth = txtf[0];
                var exYear = "20" + txtf[1];
                today = new Date();
                someday = new Date();
            
                // alert(exYear);
                if (exMonth == "00"||exMonth == "0" || exMonth >12) {
                    alert("Please Enter a valid expiry date");
                      return false;
                }
                someday.setFullYear(exYear, exMonth, 1);
                //alert(someday);
                // alert(today);
                if (someday < today) {
                    alert("Please Enter a valid expiry date");
                    document.getElementById("divexpyear").classList.add("bbcol");
                    return false;
                }
                else {
                     document.getElementById("divexpyear").classList.remove("bbcol");
                }

            }
          
                
           
            
          
            if (isv == 0) {
                document.getElementById("divccwrap").style.borderColor = "#ca2a2a";
                document.getElementById("divmainerror").style.display = "block";
               
            }
            else
            {

                 document.getElementById("divccwrap").style.borderColor = "#b5b5b5";
                document.getElementById("divmainerror").style.display = "none";
                document.getElementById("diccvvimg").style.display = "none";
            }
        }

    }
    function Controlvalidate(ctype) {
        
        if (ctype == "cno") {
            var cno = document.getElementById('<%= txtnamecard.ClientID %>');
            if (cno != null && cno.value == "") {
                cno.style.border = "1px solid #FF0000";
            }
            else {
                cno.style.border = "";
            }
        }
        if (ctype == "cn") {
            var cn = document.getElementById('<%= txtnamecard.ClientID %>');
            if (cn != null && cn.value == "") {

                cn.style.border = "1px solid #FF0000";
            }
            else {
                cn.style.border = "";
            }
        }
        if (ctype == "cvv") {
            var cvv = document.getElementById('<%= txtCVV.ClientID %>');
            if (cvv != null && cvv.value == "") {
                cvv.style.border = "1px solid #FF0000";
            }
            else {
                cvv.style.border = "";
            }
        }
    }

    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" runat="server">
</asp:Content>--%>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" runat="server">


    <table width="100%" border="0"
        align="left">
        <tr>
            <td>

                <div id="page-wrap">

                    <h3 class="pad10-0" style="margin: 0px;">Wes Check Out</h3>
                    <div class="grid12">
                        <ul class="breadcrumb_wag">
                            <li>
                                <span class="aero">Shipping / Delivery Details</span>
                            </li>
                            <li>
                                <span class="aero currentpg">Payment Options</span>
                            </li>
                            <li>
                                <span class="aero">Completed</span>
                            </li>
                        </ul>
                    </div>

                    <div class="grid12" runat="server" id="divCC">
                        <div class="">
                            <div class="cl"></div>
                            <span id="r1"></span>
                            <div runat="server" id="div2" class="alert yellowbox icon_4" style="background-color: #FFD52B; height: 33px; padding-top: 26px;">
                            </div>
                             <div runat="server" id="divgobr" class="alert yellowbox icon_4" style="height: 25px; padding-top: 26px;font-size:small;display:none">
                            </div>
                            <div runat="server" id="div1">
                                <fieldset>
                                    <legend>Payment</legend>
                                    <div runat="server" id="div3">
                                       
                                           
                                            
                                    <div style="width:100%; display:block; position:relative;float: left;margin-bottom:10px">
                                        <div style="width:65%; display:block;">
                                            <div class="form-col-2-8"><span style="font-size: 12px; float: left; margin-top: 2px;">Payment Type</span> </div>
                                            <div style="width:250px;float:left; display:inline-block;">
                                                <asp:RadioButton ID="RadioButton1" runat="server" onclick="creditcardclick()"  GroupName="Paymentoption" ValidationGroup="payonlinepaynow" Checked="true" />
                                                   <img src="../images/cards_sm.png" alt="" runat="server" id="imgsecurepay"/>
                                            </div>
                                            <div style="width:120px;float:left; display:none;">
                                                <asp:RadioButton ID="RadioButton2" runat="server" onclick="paypalclick()"  GroupName="Paymentoption" />
                                                <img src="../images/paypal_pay.png" alt="" />
                                            </div>
                                            <div style="width:120px;float:left; display:inline-block;" id="Def" runat="server">
                                                <asp:RadioButton ID="RadioButton3" runat="server" onclick="Defaultclick()"  GroupName="Paymentoption" Checked="false" />
                                                <img style="display: inline-block" src="../images/bank-transfer.png" alt="" />
                                            </div>

                                        </div>
                                        <div style="width:32%; display:block;float: left;">
                                            <h2 style="margin-top: 15px; padding-right: 35px; color: #0099DA; font-family: Arial; font-weight: bold; font-size: 26px; text-align: right">Amount: $ 
            <asp:Label runat="server" ID="lblAmount" Text="" CssClass="" />
                                             </h2>
                                        </div>

                                        <div class="cl"></div>
                                    </div>

                                        

                                            <div id="divcreditcard">
                                           <%--  <div class="form-col-3-8">
                                                <h2 style="margin-top: 15px; padding-right: 35px; color: #0099DA; font-family: Arial; font-weight: bold; font-size: 26px; text-align: right">Amount: $ 
            <asp:Label runat="server" ID="lblAmount" Text="" CssClass="" />
                                                </h2>
                                            </div>--%>

                                            <div class="cl"></div>
                                            <div class="form-col-8-8">
                                                <p></p>
                                            </div>
                                            <div class="cl"></div>
                                      <div   id="creditflag1" runat="server" style="margin-top: 15px;">

                                           <div class="creditcard_pay" id="ccfocus">
	<div class="creditcard_wrap" id="divccwrap">
		<div class="creditcard_wrap_header">
			<div class="creditcard_header-label">
				<div class="creditcard_header_cardIcon">
				   <img src="../images/card_iconLeft.jpg"/>
				</div>
				<div class="creditcard_header_title">Pay with card</div>
			</div>
			<div class="creditcard_header_rgtIcons">
				<img src="../images/PayWithCard_icon.jpg"/>
			</div>
		</div>
		<div class="creditcard_form_body">
			<div class="creditcard_form_group">
				<label for="txtnamecard">
                      <asp:Label ID="lbltypesp" runat="server" Visible="true" font-color="white" Font-Names="Arial"
                        Font-Size="10px" Font-Bold="true" Text="SP" style="color:#f2f2f2"></asp:Label>
					<div class="creditcard_form_label">Cardholder Name</div>
					<div class="creditcard_form_field">
                       
						<div class="creditcard_form_text_field">

                            <asp:TextBox ID="txtnamecard" runat="server" class="creditcard_form_input" MaxLength="150" ValidationGroup="payonlinepaynow" placeholder="Cardholder Name"></asp:TextBox>
                                                         
						</div>
					</div>
				</label>
				<div class="creditcard_form_error">
                      <asp:Label ID="lbldummy" runat="server" Visible="true" font-color="white" Font-Names="Arial"
                        Font-Size="10px" Font-Bold="true" Text="." style="color:#f2f2f2"></asp:Label>
				</div>
			</div>
			<div class="creditcard_form_group">
				<label for="txtcreditcardno">
					<div class="creditcard_form_label">Card Number</div>
					<div class="creditcard_form_field">
						<div class="creditcard_form_text_field" id="divcardno">
							
						 <asp:TextBox ID="txtcreditcardno" runat="server" class="creditcard_form_input" MaxLength="19" placeholder="" onkeypress="return validatenumerics_cc(event);"   onkeyup="InsertSpace(event)" onblur="InsertSpace_cc(event)">

                                                                </asp:TextBox>
                                
                        </div>
                         <div class="creditcard_form_icon_container">
							<div class="creditcard_form_field-error-icon" id="divimgcardno">
								<img src="../images/error_alert_icon.jpg" />
							</div>
						</div>                              
                        
					</div>
				</label>
				<div class="creditcard_form_error">
                     <asp:RequiredFieldValidator ID="RBcreditcardno" runat="server" class="creditcard_form_error"
                                                                    ErrorMessage="Please fill out a card number." ControlToValidate="txtcreditcardno" ValidationGroup="payonlinepaynow"
                                                                    InitialValue=""></asp:RequiredFieldValidator>
                        

				</div>
			</div>
			<div class="creditcard_form_multiColumn">
				<div class="creditcard_form_group">
					<label for="txtExpyear">
						<div class="creditcard_form_label">Expiration Date <span class="creditcard_form_descriptor">(MM/YY)</span></div>
						<div class="creditcard_form_field">
							<div class="creditcard_form_text_field" id="divexpyear">
                                <asp:TextBox ID="txtExpyear" runat="server" placeholder="MM / YY" class="creditcard_form_input" MaxLength="5"  onkeypress="return validatenumerics(event);" onblur="Insertslash(event)">

                                                                </asp:TextBox>
								
						
                                           
                            </div>
                             <div class="creditcard_form_icon_container">
							<div class="creditcard_form_field-error-icon"  id="diexpimg">
								<img src="../images/error_alert_icon.jpg" />
							</div>
						</div>         
						</div>
					</label>
					<div class="creditcard_form_error">
                                            <asp:RequiredFieldValidator ID="RBexpirydate" runat="server" class="creditcard_form_error"
                                                                    ErrorMessage="Please fill out an expiration date.." ControlToValidate="txtExpyear" ValidationGroup="payonlinepaynow"></asp:RequiredFieldValidator>     
					</div>
				</div>
				<div class="creditcard_form_group">
					<label for="txtCVV">
						<div class="creditcard_form_label">CVV <span class="creditcard_form_descriptor">(3 digits)</span></div>
						<div class="creditcard_form_field">
							<div class="creditcard_form_text_field" id="divcvv">
                                <asp:TextBox ID="txtCVV" runat="server" class="creditcard_form_input" MaxLength="4" placeholder="" onkeypress="return validatenumerics(event);">

                                                                </asp:TextBox>
								
						
                                           
                            </div>
                                  <div class="creditcard_form_icon_container">
							<div class="creditcard_form_field-error-icon" id="diccvvimg">
								<img src="../images/error_alert_icon.jpg" />
							</div>
						
							</div>
						</div>
					</label>
					
                   
                    <div class="creditcard_form_error">
                   <asp:RequiredFieldValidator ID="RBCVV" runat="server" class="creditcard_form_errord"
                            ErrorMessage="Please fill out a CVV." InitialValue="" ControlToValidate="txtCVV" ValidationGroup="payonlinepaynow"></asp:RequiredFieldValidator>
                            </div>        

				</div>
                 
			</div>
			
			
		</div>
	</div>
 <div class="creditcard_form_error" id="divmainerror" style="display:none;text-align:center;margin-top:10px;vertical-align:middle   ">
     <span>	<img src="../images/error_alert_icon.jpg" /></span>
     <span style="color:#ca2a2a;font-size:13px; ">Please check your information and try again.</span>
     </div>
<asp:Button ID="btnSP" runat="server" Text="Pay Now" ValidationGroup="payonlinepaynow" style="margin-top: 15px;"
                                                                class="normalsiz paynow" OnClientClick="return Setinit()" UseSubmitBehavior="true" OnClick="btnSecurePay_Click"  />
                                                            
                                                            <asp:Image ID="BtnProgressSP" runat="server" Style="display: none; float: right;"
                                                                ImageUrl="../images/Processing_Payment.png" />
</div>
	  <div runat="server" id="divfailed_sp" style="display: none; text-align: center; background: #FFF200;
                                                        font-size: 15px;height:100px" class="alert yellowbox">
                                                        <p style="font-weight: bold;height:20px; margin:7px 0; font-size:15px;">Transaction Failed!</p>
                                                        <div id="diverrorno1" style="height:30px;font-weight:300">
                                                            Please Try again or use a different card / payment method.
                                                             </div>
                                                        <div style="text-align: center">
                                                             <input  id="tryagain" class="tryagain"  style="float:none"  onclick="hide_SP();return false;" type="submit"
                                                            value="Try Again">

                                                        </div>
          </div>



                                            <%--<div class="form-col-2-8">
                                                <asp:Label runat="server" ID="lblcardnumber" Style="font-size: 12px;">Card Number &nbsp;&nbsp;<span class="redspan">*</span> </asp:Label>
                                            </div>
                                            <div class="form-col-2-8">
                                                <asp:TextBox runat="server" ID="txtCardNumber" CssClass="cardinput" Width="192px" MaxLength="19" OnBlur="Controlvalidate('cno')" />
                                            </div>
                                            <div class="form-col-2-8">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                    ControlToValidate="txtCardNumber" Display="Dynamic" CssClass="error-text" ErrorMessage="Enter Card Number"></asp:RequiredFieldValidator>

                                                <asp:CustomValidator ID="CustomValidator1" Display="Dynamic" CssClass="error-text" ClientValidationFunction="ValidCC" ErrorMessage="Please Check Credit Card Number" ControlToValidate="txtCardNumber" runat="server">
                                                </asp:CustomValidator>
                                            </div>
                                            <div class="cl"></div>
                                            <div class="form-col-2-8">
                                                <asp:Label runat="server" ID="Label1" Style="font-size: 12px;">Name on Card &nbsp;&nbsp;<span class="redspan">*</span> </asp:Label>
                                            </div>
                                            <div class="form-col-2-8">
                                                <asp:TextBox runat="server" ID="txtCardName" CssClass="cardinput" Width="192px" MaxLength="50" OnBlur="Controlvalidate('cn')" />
                                            </div>
                                            <div class="form-col-2-8">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                                    ControlToValidate="txtCardName" Display="Dynamic" CssClass="error-text" ErrorMessage="Enter Name on Card"></asp:RequiredFieldValidator>

                                            </div>
                                            <div class="cl"></div>

                                            <div class="form-col-2-8">
                                                <asp:Label runat="server" ID="lblexpirationdate" Style="font-size: 12px;">Expiration &nbsp;&nbsp;<span class="redspan">*</span> </asp:Label>
                                            </div>
                                            <div class="form-col-1-8">
                                                <asp:DropDownList NAME="drpExpmonth" ID="drpExpmonth" runat="server" CssClass="cardinput" Style="width: 100%;">
                                                    <asp:ListItem Selected="true" Text="01" Value="01"></asp:ListItem>
                                                    <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                                    <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                                    <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                                    <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                    <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                                    <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                                    <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                                    <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                    <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                    <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                </asp:DropDownList>


                                            </div>
                                            <div class="form-col-1-8">
                                                <asp:DropDownList NAME="drpExpyear" ID="drpExpyear" runat="server" CssClass="cardinput" Style="width: 100%;">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-col-2-8">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                                    ControlToValidate="drpExpmonth" Display="Dynamic" CssClass="error-text"
                                                    ErrorMessage="Select Month"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                                    ControlToValidate="drpExpyear" Display="Dynamic" CssClass="error-text"
                                                    ErrorMessage="Select Year"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="cl"></div>

                                            <div class="form-col-2-8">
                                                <asp:Label runat="server" ID="lblcardcvvnumber" Style="font-size: 12px;">Card Security Code &nbsp;&nbsp;<span class="redspan">*</span> </asp:Label>
                                            </div>
                                            <div class="form-col-2-8">
                                                <asp:TextBox runat="server" ID="txtCardCVVNumber" Width="100px" CssClass="cardinput" MaxLength="4" OnBlur="Controlvalidate('cvv')" />
                                            </div>
                                            <div class="form-col-3-8">

                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                    ControlToValidate="txtCardCVVNumber" Display="Dynamic" CssClass="error-text"
                                                    ErrorMessage="Enter Card Security Code<Br/>"></asp:RequiredFieldValidator>


                                                <span style="font-size: 10px; color: #0033CC; float: left;">
                                                    <asp:LinkButton ID="myLink" CssClass="modal" Style="color: #01AEF0; text-decoration: none;" runat="server">
              <img style="cursor: pointer;" alt="help" src="images/question_blue.png"/> 
                     <span style="font-size: 10px;color: #0033CC;">&nbsp;Where to find Security Code? Code is located on either the<br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Front / Back of your credit and consists of 3 or 4 digits</span></asp:LinkButton></span>
                                                <asp:ModalPopupExtender ID="TACpopup" PopupControlID="pnlTAC" BackgroundCssClass="modalBackgroundpopup" BehaviorID="testTACpopup"
                                                    DropShadow="true" runat="server" TargetControlID="myLink" RepositionMode="None">
                                                </asp:ModalPopupExtender>
                                            </div>

                                            <div class="cl"></div>


                                            <div class="cl"></div>
                                            <div class="form-col-8-8">
                                                <asp:Button runat="server" ID="btnPay" Text="Pay Now" Style="width: 100px;" class="button normalsiz btngreen fleft" OnClick="btnSecurePay_Click" OnClientClick="javascript:Setinit()" />



                                 
 <asp:Image ID="BtnProgress" runat="server" style="display:none;float:left;" ImageUrl="../images/Processing_Payment.png" />



                                                                                              <div id="div4" style="font-size: 12px margin-left:30px; color: Red" runat="server">
                                                </div>
                                            </div>

                                            <div class="cl"></div>--%>
                                        </div>


<div id="creditflag2" runat="server">
                                                    <div id="dropin-container" style="margin-top: -10px">
                                                    </div>
                                                    <div id="divsucess" style="display: none; text-align: center" class="alert greenbox icon_2">
                                                        Transaction Approved!
                                                        <br />
                                                        Your Order will now be processed, Thanks for shopping at Wagner Online!
                                                        <br />
                                                        Payment Method: Credit Card
                                                        <div id="divrefno" class="accordion_head_green clear">
                                                        </div>
                                                    </div>
                                                   <div id="divFailed" style="display: none; text-align: center; background: #FFF200;
                                                        font-size: 15px;height:100px" class="alert yellowbox">
                                                        <p style="font-weight: bold;height:20px; margin:7px 0; font-size:15px;">Transaction Failed!</p>
                                                        <div id="diverrorno" style="height:30px;font-weight:300">
                                                            Please Try again or use a different card / payment method.
                                                             </div>
                                                        <div style="text-align: center">
                                                             <input  id="tryagain" class="tryagain"  style="float:none"  onclick="hide();return false;" type="submit"
                                                            value="Try Again">

                                                        </div>
                                                            
                                                       
                                                        
                                                    </div>
                                                    <div runat="server" id="loading" align="center">
                                                        <input disabled id="pay-btn" class="normalsiz paynow" onclick="return false;" type="submit" style="width:350px"
                                                            value="Loading...">
                                                    </div>
                                                </div>
<div id="creditflag3" runat="server">
                                                </div>

</div>

                                                <div id="divdedault" style="margin-top:20px;display:none" >       
                            
                   <div class="payment_contents" id="divpayonacccontent" runat="server" style="padding-bottom:20px; float:left; width:50%; display:block;">
                      <h2 style="color:#0099DA; padding-right:22px; font-size: 13px;font-weight:bold   ">Payment Required</h2>
                                     <p style="font-size: 13px;line-height: 1.5;padding-right: 45px;color:#555">
Your order is on hold awaiting payment<br/>
                                      Thank you for your recent order.Payment is required prior to shipping.<br />
                                         Please trasfer funds as per details shown on the right.
                                   </p> 
  <%--<asp:Button runat="server" ID="btndirectdeposit" Text="SUBMIT ORDER" style="margin:10px 32px 10px 10px;float:left" class="normalsiz paynow"  OnClientClick="return checkorderid()" OnClick="ImageButton4_Click" /> --%>
                       <div class="clear"></div>
                     </div>
                     
                     
                     
                     <!-- -------  Bank Transfer Div ----------------- --> 
                     <div class="trans_detail" style="padding-bottom:20px; float:left; width:40%; display:block;">
                     	<div class="r_box" style="width:350px;  display:block; border:5px solid red; padding:20px; text-align:left; font-family:Arial;">
                        	<h3 style="color:red; margin:0;">Bank Transfer Details</h3>
                            <p style=" font-size:13px;">
                            	<b>Total Amount :</b> $ 
                                     
                                        <asp:Label ID="lbltotalamt" runat="server" Text="" CssClass="LabelStyle"  ></asp:Label> <br>
                                <b>Payment Reference :</b> WES-<asp:Label ID="lblorderid" runat="server" Text="" CssClass="LabelStyle"  ></asp:Label> <br>
                                <b>BSB :</b> 062-105 <br>
                                <b>Account No :</b> 1006-4018  <br>
                                <b>Account Name :</b>WES Components Pty Ltd  <br>
                                <b>Swift /  BIC Code :</b> CTBAAU2S <br><br>
                            </p>
                            
                            <p style=" font-size:12px; margin-bottom:0px;">
                            	Important. <br>
                                Please include the Shown Payment Reference <br> 
                                as your payment description for fast processing.
                            </p>
                        </div>
                     </div>
                     
                     <!-- -------  Bank Transfer Div ----------------- --> 
                                        
                           </div>    

              <div id="divpaypal" style="display:none">


             
             
            <div class="cl"></div>
            <div class="form-col-8-8">
              <img style="margin-bottom:15px" alt="cc" src="images/paypal.png">
              <p class="para pad10-0">Pay using your Credit Card or Paypal Account.</p>
              <p class="para pad10-0">You wil be redirected to PayPal website to complete payment transaction.<br>
                </p>
              
            </div>
            <div class="cl"></div>
           

            <div class="form-col-8-8">   
              
                 
                 <asp:Button runat="server" ID="btnPay_paypal" Text="Pay Now" style="width:100px;" class="button normalsiz btngreen fleft"  OnClick="btnPay_Click" OnClientClick="return Setinit_paypal()" />       


                 
                 <asp:Button runat="server" ID="BtnProgress_paypal" Text="Processing Payment. Please Wait…" style="display:none;visibility:visible;float:left;" class="button normalsiz btngreen fleft" Enabled="false"   />       
                 
                  
         <div id="divContent" style="font-size:12px margin-left:30px;color:Red" runat="server"  >
       </div>
            </div>
                  </div>

                                            </div>
                                    </div>
                                    <div class="cl"></div>
                           
                            </div>
                            <fieldset>
                                <legend>Shipping & Order Details</legend>
                                <table width="100%" border="0" cellpadding="0" cellspacing="0">

                                    <tr>
                                        <td>
                                            <fieldset>
                                                <legend>Bill To</legend>
                                                <p class="para pad10">
                                                    <asp:Label ID="lblDeliveryTo" runat="server" Text="Delivery Address" CssClass="LabelStyle"
                                                        Font-Bold="false" Style="font-weight: normal;"></asp:Label>
                                                </p>
                                                <div class="cl"></div>
                                            </fieldset>
                                        </td>
                                        <td>
                                            <fieldset>
                                                <legend>Ship To</legend>
                                                <p class="para pad10">
                                                    <asp:Label ID="lblShipTo" runat="server" Text="Shipping Address" CssClass="LabelStyle"
                                                        Font-Bold="false" Style="font-weight: normal;"></asp:Label>
                                                </p>
                                                <div class="cl"></div>
                                            </fieldset>
                                        </td>
                                    </tr>



                                    <tr>
                                        <td colspan="2">
                                            <fieldset>
                                                <legend>Order Contents</legend>
                                                <div class="form-col-8-8">
                                                    <table width="100%" border="0">


                                                        <tr>
                                                            <td width="100%" colspan="2">
                                                                <table width="100%" id="test1" border="0" cellpadding="3" cellspacing="0" class="orderdettable">

                                                                    <tr class="" style="background-color: #BCD0E2;">
                                                                        <td align="left" width="20%">ORDER CODE
                                                                        </td>
                                                                        <td align="left" width="10%">QTY
                                                                        </td>
                                                                        <td align="left" width="25%">Description
                                                                        </td>
                                                                        <td align="right" width="20%">Cost(Ex. GST)
                                                                        </td>
                                                                        <td align="left" width="30%">Extension Amount (Ex. GST)
                                                                        </td>
                                                                    </tr>
                                                                    <asp:Repeater ID="OrderitemdetailRepeater" runat="server">

                                                                        <ItemTemplate>

                                                                            <tr id="tRow" runat="server" class="rowOdd">
                                                                                <td id="TD1" runat="server" style="text-align: left;"><%# Eval("CATALOG_ITEM_NO")%></td>
                                                                                <td style="text-align: left;"><%# Eval("QTY")%></td>
                                                                                <td style="text-align: left;"><%# Eval("DESCRIPTION")%></td>
                                                                                <td style="text-align: right;">$ <%# Convert.ToDecimal(Eval("PRICE_EXT_APPLIED")).ToString("#,#0.00")%></td>
                                                                                <td style="text-align: right;">$ <%# Convert.ToDecimal (Eval("TOTAL_EXT")).ToString("#,#0.00") %></td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                        <AlternatingItemTemplate>
                                                                            <tr id="tRow" runat="server" class="">
                                                                                <td id="TD1" runat="server" style="text-align: left;"><%# Eval("CATALOG_ITEM_NO")%></td>
                                                                                <td style="text-align: left;"><%# Eval("QTY")%></td>
                                                                                <td style="text-align: left;"><%# Eval("DESCRIPTION")%></td>
                                                                                <td style="text-align: right;">$ <%# Convert.ToDecimal(Eval("PRICE_EXT_APPLIED")).ToString("#,#0.00")%></td>
                                                                                <td style="text-align: right;">$ <%# Convert.ToDecimal (Eval("TOTAL_EXT")).ToString("#,#0.00") %></td>
                                                                            </tr>
                                                                        </AlternatingItemTemplate>
                                                                    </asp:Repeater>


                                                                    <tr style="background-color: white; font-size: 12px;">
                                                                        <td align="left" width="50%" colspan="3" rowspan="4" valign="bottom">
                                                                            <asp:Button ID="ImgBtnEditShipping" runat="server" Text="Edit / Update Order" Style="float: left !important; font-size: 12px; text-shadow: none; display: none;"
                                                                                OnClick="ImgBtnEditShipping_Click" class="buttongray normalsiz btngray fleft" CausesValidation="false" />
                                                                        </td>

                                                                        <td align="left" width="20%" class="">
                                                                            <strong>Sub Total (Ex GST)</strong>
                                                                        </td>
                                                                        <td align="left" width="30%" class="" style="text-align: right;">
                                                                            <strong>$  
                                                                                <asp:Label runat="server" ID="Product_Total_price" />
                                                                            </strong>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="background-color: white; font-size: 12px;">
                                                                        <%-- <td align="left" width="50%"  >
                        
                        </td>--%>

                                                                        <td align="left" width="20%" class="rowOdd">
                                                                            <strong>Delivery / Handling Charge (Ex GST) </strong>
                                                                        </td>
                                                                        <td align="left" width="30%" class="rowOdd" style="text-align: right;">
                                                                            <strong>$ 
                                                                                <asp:Label runat="server" ID="lblCourier" /></strong>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="background-color: white; font-size: 12px;">
                                                                        <%-- <td align="left" width="50%"  >
                        
                    </td>--%>

                                                                        <td align="left" width="20%" class="rowOdd">
                                                                            <strong>Total Tax Amount (GST) </strong>
                                                                        </td>
                                                                        <td align="left" width="30%" class="rowOdd" style="text-align: right;">
                                                                            <strong>$ 
                                                                                <asp:Label runat="server" ID="Tax_amount" /></strong>
                                                                        </td>
                                                                    </tr>

                                                                    <tr style="background-color: white; font-size: 12px;">
                                                                        <%--<td align="left" width="50%"  >
                        
                    </td>--%>

                                                                        <td align="left" width="20%" class="Rsucess">
                                                                            <strong>
                                                                                <asp:Label runat="server" ID="lblTotalCap" /></strong>

                                                                        </td>
                                                                        <td align="left" width="30%" class="Rsucess" style="text-align: right;">
                                                                            <strong>$ 
                                                                                <asp:Label runat="server" ID="Total_Amount" /></strong>

                                                                        </td>
                                                                    </tr>


                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </fieldset>
                                        </td>
                                    </tr>


                                </table>

                            </fieldset>




                        </div>
                    </div>

                    <div class="grid12" runat="server" id="divTimeout" visible="false">
                        <fieldset>
                            <div style="text-align: center; padding: 130px;">
                                <span style="font-size: 21px;">Your session has timed out</span><br />
                                <span style="font-size: 14px;"><a href="Login.aspx" class="para pad10-0" style="font-size: 11px; color: #0033cc; font-weight: bold;">Click here</a> to log in again </span>
                            </div>
                        </fieldset>
                    </div>

               

            </td>
        </tr>

    </table>
      <asp:Button ID="btnTemp" runat="server" Style="display: none;" />
     <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" DynamicServicePath="" Enabled="True"
        TargetControlID="btnTemp" PopupControlID="pnlMessageBox_BR" BackgroundCssClass="modal"
        PopupDragHandleControlID="pnlMessageBox_BR" CancelControlID="btnclose_br" BehaviorID="mpeFirmMessageBox_br">
    </asp:ModalPopupExtender>
     <div style="position:fixed;width:100%;height:100%;display:none;background:rgba(0,0,0,0.7);z-index:9999 ;   top: 0; bottom: 0;left: 0;right: 0;" id="p1"  runat="server" > </div>
    
     <asp:Panel ID="pnlMessageBox_BR" runat="server" Style="display: none; width: 360px;
        text-align: center; max-height: 250px; overflow: auto; padding-bottom: 20px"
        class="MessageBoxPopUp">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr class="MessageBoxHeader" style="height: 17px;">
                <td colspan="2">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/info_msg.png" />
                    <%--<asp:Label ID="lblHeader" runat="server" Text="i" style="font-size:14px;color:White;width:22px;height:15px;display:inline-block;background-color:#6eb621;border:2px solid #fff;text-align:center"   ></asp:Label>
                    --%>
                </td>
                <td align="right" style="padding: 2px 2px 0px 0px;">
                    <%--   <asp:ImageButton ID="imgBtnClose" runat="server" ImageUrl="~/Images/close11.png"

                             OnClientClick="closeModelPopup(this)" />--%>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 5px;">
                </td>
            </tr>
            <tr>
                <td class="MessageBoxData" colspan="2" style="width: 100%; padding: 15px; text-align: center;">
                    <asp:Label ID="lblMessage_br" runat="server"></asp:Label>
                </td>
            </tr>
            <tr style="vertical-align: bottom; height: 20px; padding: 0px 5px 5px 0px;">
                <td align="right" style="width: 56px; text-align: center; margin: 0 auto; margin-top: -15px;
                    display: block;">
                    <input type="button" id="btnclose_br" class="btnbuy2 button btngreen" onclick="closeModelPopup_BR()"
                        value="Ok" />
                </td>
            </tr>
        </table>
    </asp:Panel>
   
    <asp:Panel ID="pnlTAC" runat="server" Width="650px" Style="display: none;">
        <a href="javascript:Hidepopup()" class="testbutton"></a>
        <div class="boxfull" style="width: 575px; height: 500px; text-align: left;">

            <h1>How to find the security code on a credit card</h1>
            <p>Find out where to locate the security code on your credit card.</p>
            <p><strong>Visa, MasterCard, Discover, JCB, and Diners Club</strong></p>
            <p>The security code is a three-digit number on the back of your credit card, immediately following your main card number.</p>
            <img src="images/Mcard.jpg" />
            <p><strong>American Express</strong></p>
            <p>The security code is a four-digit number located on the front of your credit card, to the right above your main credit card number.</p>
            <img src="images/Acard.jpg" />
            <p>If your security code is missing or illegible, call the bank or credit card establishment referenced on your card for assistance.</p>
        </div>
    </asp:Panel>
    <style>

        .braintree-form__field {
            width:60%!important;
        }
          

div[data-braintree-id="cvv-field-group"] .braintree-form__field {
   width: 50% !important;
   margin-left:-122px!important;
}
div[data-braintree-id="cvv-field-group"] > label > .braintree-form__label {
   width: 50% !important;
   margin-left:-122px!important;
}

    </style>
     
     <script src="https://js.braintreegateway.com/web/dropin/1.22.0/js/dropin.min.js"></script>
    <script src="https://js.braintreegateway.com/web/3.47.0/js/three-d-secure.min.js"></script>
    <%-- <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.js"></script>--%>
    <script src="https://js.braintreegateway.com/web/3.59.0/js/client.min.js"></script>
    <script src="https://js.braintreegateway.com/web/3.59.0/js/data-collector.min.js"></script>
    <script>
         function closeModelPopup_BR()
        {
           
             document.getElementById('ctl00_maincontent_p1').style.display = "none";
          document.getElementById('ctl00_maincontent_creditflag2').Style.display = "block";
            document.getElementById('pay-btn').focus();
             $find('mpeFirmMessageBox_br').hide();
          
        }
      
            var payBtn = document.getElementById('pay-btn');
var nonceGroup = document.querySelector('.nonce-group');
var nonceInput = document.querySelector('.nonce-group input');
var payGroup = document.querySelector('.pay-group');
         var dropin;
         var clienttoken = "<%= this.ClientToken %>";
        var totamt = document.getElementById('ctl00_maincontent_lblAmount');
                 //   alert(totamt);
         var amt1 = totamt.innerHTML.replace('<b>', '').replace('</b>', '').replace('$', '').replace(',','');
       //  alert(totamt.innerHTML.replace('<b>','').replace('</b>','').replace('$',''));
         var amt = parseFloat(amt1);
             braintree.dropin.create({
                 authorization: clienttoken,
                 container: '#dropin-container',
                 card: {
                     cardholderName: {
                         required: false
                         // to make cardholder name required
                         // required: true
                     }
                 },
                 fields: {
                     number: {
                         selector: '#card-number',
                         placeholder: '1111 1111 1111 1111'
                     },
                    
                     expirationDate: {
                         selector: '#expiration-date',
                         placeholder: 'MM/YY'
                     }
                 },


                 threeDSecure: {
                     amount: amt
                 }

                
             }, function (err, instance) {
                 if (err) {


                     console.log('component error:', err);
                     return;
                 }
                 var divOk = document.getElementById('divFailed');
                 divOk.style.display = "none";

                 dropin = instance;

                 setupForm();
             });

                   
             function setupForm() {
                 //alert("inside setip form");
                 enablePayNow();
             }
            
             function enablePayNow() {
                 payBtn.value = 'Pay Now';
                 payBtn.removeAttribute('disabled');
                 console.log("enable paynow");
               // document.getElementById('cancel-order').style.display =  "inline-block";
                   document.getElementById('divFailed').style.display = "none";
                            document.getElementById('diverrorno').style.display = "none";
                          
             }

             function showNonce(payload) {
                 nonceInput.value = payload.nonce;

                // document.getElementById("HiddenField1").value = payload.nonce;
                 payGroup.classList.add('hidden');
                 payGroup.style.display = 'none';
                 nonceGroup.classList.remove('hidden');



             }

             payBtn.addEventListener('click', function (event) {
                 payBtn.value = 'Processing...';
                 payBtn.setAttribute('disabled', 'disabled');
                 //                 document.getElementById('cancel-order').style.display = "none";
                 //                 document.getElementById('divdis').style.display = "block";
                 //  
                 
                 document.getElementById("ctl00_maincontent_Def").style.display = "none";
                 //                 document.getElementById("lblsp").disabled = true;
                 try {
                     var y = document.getElementsByClassName("braintree-heading");
                     y[0].innerHTML = '';
                     y[0].outerHTML = '';
                 }
                 catch (err)
                   { }
                 dropin.requestPaymentMethod(function (err, payload) {
                   
                     //   alert(err);
                     if (err) {
                         
                         console.log('tokenization error:', err);
                         // alert(err);
                         dropin.clearSelectedPaymentMethod();
                         payBtn.value = 'Pay Now';
                         payBtn.removeAttribute('disabled');
                         document.getElementById("ctl00_maincontent_Def").style.display = "inline-block";
                       
                         return;
                     } else {

                         payBtn.setAttribute('disabled', 'disabled');
                         payBtn.value = 'Processing...';
                         console.log('initial tokenization success:', payload);
                        
                     }

                     if (payload.type !== 'CreditCard') {

                         // if not a credit card, skip 3ds and send nonce to server
                         return;
                     }

                     // console.log(payload.status.threeDSecure);
                     if (!payload.liabilityShifted) {


                         //dropin.clearSelectedPaymentMethod();
                         console.log('Liability did not shift', payload);


                         //enablePayNow();
                         payBtn.visibility = "hidden";
                         //  document.getElementById('cancel-order').style.display = "none";
                         SaleTrans(payload.nonce);
                         
                         //  document.getElementById('divdis').style.display = "none";
                         document.getElementById("ctl00_maincontent_Def").style.display = "inline-block";
                     }
                     else {

                         SaleTrans(payload.nonce);
                         document.getElementById('divFailed').style.display = "none";
                         // document.getElementById('cancel-order').style.display = "none";
                         document.getElementById('diverrorno').style.display = "none";
                         payBtn.value = 'Card Verification Successful..Please Wait..';

                         $("#btnnewsecurepay").attr("disabled", "disabled");

                         // document.getElementById("braintreesecurepay").disabled = true;
                         //   document.getElementById("lblpp").style.display = "none";
                         //
                         var x = document.getElementsByClassName("braintree-large-button");
                         x[0].innerHTML = '';
                         x[0].outerHTML = '';
                         enableFrm();
                     }

                     console.log('verification success:', payload);




                     // send nonce and verification data to your server
                 });
             });

       
    </script> 
    <script type="text/javascript" language="javascript">


           function SaleTrans(a) {
               try { 
                      try {
                    var x = document.getElementsByClassName("braintree-large-button");
                     x[0].outerHTML = "";
                }
                catch (err) { }
                   document.getElementById('ctl00_maincontent_divgobr').innerHTML = "";
                    document.getElementById('ctl00_maincontent_divgobr').style.display = "none";
                    var totamt = document.getElementById('ctl00_maincontent_lblAmount');
                   // alert(totamt);
         var amt1 = totamt.innerHTML.replace('<b>', '').replace('</b>', '').replace('$', '').replace(',','');
       //  alert(totamt.innerHTML.replace('<b>','').replace('</b>','').replace('$',''));
         var amt = parseFloat(amt1);

                $.ajax({
                    type: "POST",
                    url: "PaySP.aspx/SaleTrans",
                    data: "{'nounce':'" + a + "','Amount':'"+ amt +"'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                       if (data.d.includes("Error") == false) {

                 window.location.href=data.d;
                 }
                 else if (data.d.includes("QTEEMPTY") == true) {
                     window.location.href = "ConfirmMessage.aspx?Result=QTEEMPTY";
                 }
                 else if (data.d.includes("Session Timed out") == true) {
                     alert("Session Timed out.Please Login..")
                     window.location.href = "Login.aspx";
                 }


                 else {

                     // enablePayNow();
//                     document.getElementById("btnnewsecurepay").disabled = true;
//                     document.getElementById("braintreesecurepay").disabled = false;
                     var divOk = document.getElementById('divFailed');
                     document.getElementById('diverrorno').style.display = "block";
                     // document.getElementById('diverrorno').innerHTML = data.d
                     divOk.style.display = "block";
                     document.getElementById('divsucess').style.display = "none";

                     document.getElementById('divrefno').style.display = "none";
                     // dropin.clearSelectedPaymentMethod();
                           document.getElementById('dropin-container').style.display = "block";
                            document.getElementById('pay-btn').style.display = "none";
//                           document.getElementById("lblpp").style.display = "inline-block";
//                           document.getElementById('PayType1').style.display = "block";
                            try {
                               var x = document.getElementsByClassName("braintree-large-button");
                               x[0].outerHTML = "";
                           }
                           catch (error) { }
                           //   x[0].outerHTML = '<div data-braintree-id="toggle" class="braintree-large-button braintree-toggle" tabindex="0" style="background:white;border:none" onclick="hide()"><span onclick="hide()">Choose another way to pay</span></div>';
                     //document.getElementById('cancel-order').style.display = "inline-block";
//                     document.getElementById("lblsp").disabled = true;
                     
                    //x[0].innerHTML = '<span  onclick=hide()>Choose another way to pay</span>';
                     //alert(x[0].outerHTML);
                   
                      
                 }
                 //   alert("x");
                 // showNonce(payload);
             },
             error: function (xhr, status, error) {

                 var err = eval("(" + xhr.responseText + ")");
                 // alert(err);
             }
         })
            }
            catch (err) {
//alert(err)
            }
      }
        function hide_SP() {

            $("#<%=btnSP.ClientID%>").show();
            $("#<%=btnSP.ClientID%>").val("Pay Now");
             var divOk = document.getElementById('ctl00_maincontent_divfailed_sp');
            divOk.style.display = "none";
            return false;
        }

        function hide() {
            //   alert("x");
            var divOk = document.getElementById('divFailed');
            divOk.style.display = "none";
            //            var x = document.getElementsByClassName("braintree-methods braintree-methods-initial");
            //            x[0].innerHTML = '';
            document.getElementById("ctl00_maincontent_loading").style.display = "block";
            enablePayNow();
            dropin.clearSelectedPaymentMethod();
            document.getElementById('pay-btn').style.display = "block";
            //var lblsp = document.getElementById('lblsp');
            //lblsp.removeAttribute('disabled');
            //document.getElementById("lblsp").disabled = false;
            //document.getElementById("lblpp").style.display = "inline-block";
            //var x = document.getElementsByClassName("braintree-large-button");

            ////  x[0].innerHTML = '<span  onclick=hide()>Choose another way to pay</span>';
            //x[0].outerHTML = '<div  data-braintree-id="toggle" class="braintree-large-button braintree-toggle braintree-hidden" tabindex="0" style="background:white;border:none" onclick="hide()"><span onclick="hide()">Choose another way to pay</span></div>';

        }



    </script>

</asp:Content>
<%--<asp:Content ID="Content5" ContentPlaceHolderID="rightnav" runat="server">
</asp:Content>--%>
<asp:Content ID="Content6" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
