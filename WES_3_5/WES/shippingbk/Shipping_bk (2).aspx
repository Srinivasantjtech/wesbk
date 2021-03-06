<%@ Page Language="C#" MasterPageFile="~/mainpage.master" AutoEventWireup="true" Inherits="shipping" EnableEventValidation="false"
    Title="Untitled Page" Culture="en-US" UICulture="en-US" CodeBehind="shipping.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace="TradingBell.WebCat.CommonServices" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="UI/InvoiceOrder.ascx" TagName="InvoiceOrder" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">

    <style>
.update_popup {
	    width: 560px;
    height: 180px;
    display: block;
    position: fixed;
    top: 0;
    bottom: 0;
    left: 0;
    right: 0;
    background: #fff;
    margin: auto;
    border: 2px solid #99b4d1;
    font-family: arial;
}
.update_popup_head {
	width:100%;
	height:40px;
	text-align:left;
	background:#3399ff;
}
.update_popup_head img {
padding-top: 5px;
}
.update_popup_body {
	display:block;
	padding:15px;
	text-align:center;
	background:#fff;
	color:#666;
	margin:0 auto;
}
.update_popup_body b {
	font-size:18px;
	text-align:center;
	margin-top:15px;
}
.btnPopup {
    display: inline-block;
	border-radius:6px;
    height: 33px;
    line-height: 33px;
    text-align: center;
    width: 240px;
	margin:30px 0 10px 0;
	text-decoration:none;
	color:#fff !important;
    font-size:16px;
}
.btngreen {
    background: url(../images/greenbtn.jpg);
    border: 1px solid #009900 !important;
}
.btnyellow {
    background: url(../images/yellow_btn.jpg);
    border: 1px solid #e77a10 !important;
}
a.btnPopup:hover { cursor: pointer; }
</style>
    <script type="text/javascript">

        function SetinitSP() {
            try {
                var res = checkcreditmat();
                if (res == true) {

                    var x = document.getElementById('<%= btnSP.ClientID %>');
                    var y = document.getElementById('<%= BtnProgressSP.ClientID %>');

                    x.style.display = "none";
                    y.style.display = "block";
                    y.style.visibility = "visible";
                    //  z.style.display = "block";
                    //  z.style.visibility = "visible";
                    return true;
                }
                else {
                    var x = document.getElementById('<%= btnSP.ClientID %>');
                    var y = document.getElementById('<%= BtnProgressSP.ClientID %>');

                    x.style.display = "block";
                    y.style.display = "none";
                    x.style.visibility = "visible";
                    return false;
                }
            }
            catch (err)
            { }
        }




        $(document).keypress(function (e) {
            if (e.keyCode === 13) {
                e.preventDefault();
                return false;
            }

        });
    
      window.history.forward(1);

        window.location.hash = "no-back-button";
        window.location.hash = "Again-No-back-button"; //again because google chrome don't insert first hash into history
        window.onhashchange = function () { window.location.hash = "no-back-button"; }

        function Onload_drpcheck() {
            if (($("#<%=drpSM1.ClientID%> option:selected").text() == "Shop Counter Pickup")) {
               
                var mblnumber = document.forms[0].elements["<%=txtMobileNumber.ClientID%>"].value;
                if ((mblnumber.substring(0, 2) == "04" && mblnumber.length == 10)) {
                    document.getElementById("ctl00_maincontent_smspopup").style.display = 'inline-block';


                    document.getElementById('<%=lblorderready.ClientID%>').innerHTML = mblnumber;
                    document.getElementById("ctl00_maincontent_divpaymentoption").style.display = 'block';
                    try{
                  document.getElementById("ctl00_maincontent_ImageButton2").style.display = 'none';
                    }
                    catch(error){}
                   

                }
                else {
                    document.getElementById("ctl00_maincontent_smspopup").style.display = 'none';
      //              counterPickup();

                }
                if (($("#<%=drpSM1.ClientID%> option:selected").text() == "International Shipping - TBA")) {
                    try {
                        document.getElementById("ctl00_maincontent_ImageButton2").style.display = 'none';
                        
                    }
                    catch (error) { }
                }
            }
           

            if (document.getElementById("ctl00_maincontent_divdedault").style.display == "block") {
                document.getElementById("<%=RBpaymenttype.ClientID %>").Checked = true;
                document.getElementById("<%=RBpaymenttype.ClientID %>").Checked = true;
//                alert(document.getElementById("ctl00_maincontent_divsubmitordertype").style.display);
//               
//                alert(document.getElementById("<%=RBpaymenttype.ClientID %>").Checked);
              
            }
            var rbdefault = document.getElementById("<%=RBpaymenttype.ClientID %>");
            if (rbdefault.checked) {
                Defaultclick();
            }
            var rbpaypal = document.getElementById("<%=RBPaypal.ClientID %>");
            if (rbpaypal.checked) {
                paypalclick();
            }

            var rbcredit = document.getElementById("<%=RBCreditCard.ClientID %>");
            if (rbcredit.checked) {
                creditcardclick();
            }
        }
        window.onload = Onload_drpcheck;
    </script>

    <%-- Comments / Notes--%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" runat="Server">

     <asp:HiddenField ID="hfuserid" Value="0" runat="server" />
    <!--Add JQuery library reference-->
    <%--  <script src="Scripts/jquery-1.5.1.min.js" type="text/javascript"></script>--%>

    <link href="css/jquery-ui.css" rel="stylesheet" type="text/css" />

    <script src="Scripts/jquery-1.9.1.js" type="text/javascript"></script>

    <script src="Scripts/jquery-ui.js" type="text/javascript"></script>
    <script type="text/javascript">
        function doPostBack(t) {

            __doPostBack();
        }
       
        $(document).ready(function () {
           
                     
            
            $("#ctl00_maincontent_txtSuburb").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "shipping.aspx/GetData",
                        data: "{'DName':'" + document.getElementById('ctl00_maincontent_txtSuburb').value + "'}",
                        dataType: "json",
                        success: function (data) {

                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split('/')[0],
                                    val: item.split('/')[1]
                                }
                            }));
                        },
                        error: function (result) {
                            //alert("Error");
                        }
                    });
                },
                select: function (event, ui) {
                   
                    // document.getElementById('ctl00_maincontent_txtSuburb').value(ui.item.val);

                    var str_esc = escape(ui.item.label);

               
                    $.ajax({
                        type: "POST",
                        url: "shipping.aspx/Loadpostcode",
                        data: "{'DName':'" + str_esc + "'}",

                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            if (data.d != "") {
                                // alert(data.d);
                                var res = data.d.split("-");
                                $('#ctl00_maincontent_txtPostCode').val(res[0])
                                $('#ctl00_maincontent_hfPostCode').val(res[0])
                               
                                $('#ctl00_maincontent_drpstate_txt').val(res[1])
                                $('#ctl00_maincontent_hfdrpstate_txt').val(res[1])
                                $('#ctl00_maincontent_txtSuburb').val(res[2])
                                $('#ctl00_maincontent_txtCountry').focus();
                                __doPostBack();
                             
                            }
                            else {


                            }

                        },
                        error: function (xhr, status, error) {
                            var err = eval("(" + xhr.responseText + ")");
                            console.log(err);
                            // alert(err);
                        }
                    })



                }
            });
        }

        )



        $(document).ready(function () {
            $('#ctl00_maincontent_txtSuburb').attr('autocomplete', 'new-password');
        });  

  
    </script>





    <script type="text/javascript">
        function CheckShippment() {

            switch (document.getElementById("ctl00_maincontent_drpSM1").value) {
                case 'Mail':
                    try {
                        
                        document.getElementById("ctl00_maincontent_ImageButton2").visibilty= 'hidden';
                    }
                    catch (Error)
                 { }
                    $("#smspopup").hide();
                    ShowShipmentPanel();
                   // ShowMailMessage();
                    document.forms[0].elements["<%=drpSM1.ClientID%>"].style.border = 'none';
                    try {
                        document.getElementById("ctl00_maincontent_smspopup").style.display = 'none';
                    }
                    catch (Error) { }
                      __doPostBack();
                    break;
                case 'Courier':
                    try {
                      
                        document.getElementById("ctl00_maincontent_ImageButton2").style.display = 'block';
                    }
                    catch (Error)
                 { }
                    $("#smspopup").hide();
                    ShowShipmentPanel();

                    document.forms[0].elements["<%=drpSM1.ClientID%>"].style.border = 'none';
                    try {
                        document.getElementById("ctl00_maincontent_smspopup").style.display = 'none';
                    }
                    catch (Error) { }
                    __doPostBack();

                    break;
                case 'Courier Pickup':
                    $("#smspopup").hide();
                    ShowShipmentPanel();
                    //ShowCourierMessage();
                    
                    //document.getElementById("ctl00_maincontent_hfispopup").value = 1;
                    document.forms[0].elements["<%=drpSM1.ClientID%>"].style.border = 'none';
                    document.getElementById("ctl00_maincontent_divpaymentoption").style.display = 'block';
                    document.getElementById("ctl00_maincontent_smspopup").style.display = 'none';
                    try {
                        // document.getElementById("ctl00_maincontent_ImageButton2").style.display = 'none';
                    }
                    catch (Error) { }

                   // document.forms[0].elements["<%=TextBox1.ClientID%>"].focus();
                    //checkforpostback_afterclick();
                    __doPostBack();
                    return false;
                    break;
              case 'Counter Pickup':

                  ShowShipmentPanel();

                  //document.forms[0].elements["<%=drpSM1.ClientID%>"].style.border = 'none';
                  var mblnumber1 = document.forms[0].elements["<%=txtMobileNumber.ClientID%>"].value;
              
                  if (mblnumber1.substring(0, 2) == "04" && mblnumber1.length == 10) {

                      document.getElementById("ctl00_maincontent_smspopup").style.display = 'inline-block';

                      document.getElementById('<%=lblorderready.ClientID%>').innerHTML = mblnumber1;

                      //document.getElementById("ctl00_maincontent_lblorderready").val(mblnumber1);

                  }
                  else {

                      counterPickup();
                  }
                  document.getElementById("ctl00_maincontent_divpaymentoption").style.display = 'block';
                  try {
                     // document.getElementById("ctl00_maincontent_ImageButton2").style.display = 'none';
                  }
                  catch (Error)
                 { }
                  checkforpostback();
                  return false;
                  break;
                case 'Drop Shipment Order':
                    $("#smspopup").hide();
                    ShowDropShipmentPanel();
                    document.forms[0].elements["<%=drpSM1.ClientID%>"].style.border = 'none';
                    document.getElementById("ctl00_maincontent_divpaymentoption").style.display = 'block';
                    try {
                       // document.getElementById("ctl00_maincontent_ImageButton2").style.display = 'none';
                    }
                    catch (Error)
                 { }
                   try {
                    document.getElementById("ctl00_maincontent_smspopup").style.display = 'none';
                }
                catch (Error)
                 { }
                     __doPostBack();
                    return false;
                    break;
                default:
                    $("#smspopup").hide();
                    ShowShipmentPanel();
                    document.forms[0].elements["<%=drpSM1.ClientID%>"].style.border = 'none';
                    document.getElementById("ctl00_maincontent_smspopup").style.display = 'none';

                    //__doPostBack();
                    break;
            }
        }
        function checkforpostback_afterclick() {
            try {


                if (document.getElementById('ctl00_maincontent_PnlOrderContents').innerHTML.indexOf('To Be Advised') != -1) {
                 
                        __doPostBack();
                 
                }

                if (document.getElementById("ctl00_maincontent_hfshipcode").value != "") {
                   
                        __doPostBack();
                    
                }
            }
            catch (Error)
                 { }

        }
        function checkforpostback() {
            try {


                if (document.getElementById('ctl00_maincontent_PnlOrderContents').innerHTML.indexOf('To Be Advised') != -1) {
                var mblnumber1 = document.forms[0].elements["<%=txtMobileNumber.ClientID%>"].value;

                if (mblnumber1.substring(0, 2) == "04" && mblnumber1.length == 10) {
                    __doPostBack();
                }
                }

            if (document.getElementById("ctl00_maincontent_hfshipcode").value != "") {
                 var mblnumber1 = document.forms[0].elements["<%=txtMobileNumber.ClientID%>"].value;

                 if (mblnumber1.substring(0, 2) == "04" && mblnumber1.length == 10) {
                     __doPostBack();
                 }
                }
            }
            catch (Error)
                 { }
            
        }
        function ShowDropShipmentPanel() {
            document.getElementById("DropShipmentRow").style.display = '';
            document.getElementById("OtherShipmentRow").style.display = 'none';

        }

        function ShowShipmentPanel() {
            document.getElementById("OtherShipmentRow").style.display = '';
            document.getElementById("DropShipmentRow").style.display = 'none';


        }

        function HidePanels() {
            if (document.getElementById("ctl00_maincontent_drpSM1") != null && document.getElementById("ctl00_maincontent_drpSM1").value != null && document.getElementById("ctl00_maincontent_drpSM1").value == 'Drop Shipment Order') {
                document.getElementById("OtherShipmentRow").style.display = 'none';
                document.getElementById("DropShipmentRow").style.display = '';

            }
            else {
                document.getElementById("OtherShipmentRow").style.display = '';
                document.getElementById("DropShipmentRow").style.display = 'none';

            }
        }

        function ValidationDropShipOrder() {
           
            var isCompanyEmpty = false;
            var isStateEmpty = false;
            var isPostcodeEmpty = false;
            var isSuburbEmpty = false;
            var isadd1key = false;
            var isadd2key = false;
            var isPCkey = false;

         
            if ((document.getElementById("ctl00_maincontent_drpSM1").value == 'Drop Shipment Order') && ((document.getElementById("ctl00_maincontent_txtAttentionTo").value == '') || (document.getElementById("ctl00_maincontent_txtAttentionTo").value == null) || (document.getElementById("ctl00_maincontent_txtAttentionTo").value == 'null'))) {
                document.getElementById("ctl00_maincontent_txtAttentionTo").style.borderColor = "red";
                document.getElementById("ctl00_maincontent_txtAttentionTo").focus();
                isCompanyEmpty = true;
            }
            else {
                document.getElementById("ctl00_maincontent_txtAttentionTo").style.borderColor = "ActiveBorder";
            }
            //                       


            if ((document.getElementById("ctl00_maincontent_drpSM1").value == 'Drop Shipment Order') && ((document.getElementById("ctl00_maincontent_drpstate_txt").value == 'Select Ship To State') || (document.getElementById("ctl00_maincontent_drpstate_txt").value == null) || (document.getElementById("ctl00_maincontent_drpstate_txt").value == 'null'))) {
                document.getElementById("ctl00_maincontent_drpstate_txt").style.borderColor = "red";
                document.getElementById("ctl00_maincontent_drpstate_txt").focus();
                isStateEmpty = true;
            }
            else {
                document.getElementById("ctl00_maincontent_drpstate_txt").style.borderColor = "ActiveBorder";
            }
            if ((document.getElementById("ctl00_maincontent_drpSM1").value == 'Drop Shipment Order') && ((document.getElementById("ctl00_maincontent_txtAddressLine1").value == '') || (document.getElementById("ctl00_maincontent_txtAddressLine1").value == null) || (document.getElementById("ctl00_maincontent_txtAddressLine1").value == 'null'))) {
                document.getElementById("ctl00_maincontent_txtAddressLine1").style.borderColor = "red";
                document.getElementById("ctl00_maincontent_txtAddressLine1").focus();
                isStateEmpty = true;
            }
            else {
                document.getElementById("ctl00_maincontent_txtAddressLine1").style.borderColor = "ActiveBorder";
                //PageMethods.GetDropShipmentKeyExists(document.getElementById("ctl00_maincontent_txtAddressLine1").value,"", OnSuccess1, OnFailure1)
            }


            if ((document.getElementById("ctl00_maincontent_drpSM1").value == 'Drop Shipment Order') && ((document.getElementById("ctl00_maincontent_txtPostCode").value == '') || (document.getElementById("ctl00_maincontent_txtPostCode").value == null) || (document.getElementById("ctl00_maincontent_txtPostCode").value == 'null'))) {
                document.getElementById("ctl00_maincontent_txtPostCode").style.borderColor = "red";
                document.getElementById("ctl00_maincontent_txtPostCode").focus();
                isPostcodeEmpty = true;
            }
            else {
                document.getElementById("ctl00_maincontent_txtPostCode").style.borderColor = "ActiveBorder";
                // PageMethods.GetDropShipmentKeyExists(document.getElementById("ctl00_maincontent_txtPostCode").value, "PostCode", OnSuccess1, OnFailure1)
            }

            if ((document.getElementById("ctl00_maincontent_drpSM1").value == 'Drop Shipment Order') && ((document.getElementById("ctl00_maincontent_txtSuburb").value == '') || (document.getElementById("ctl00_maincontent_txtSuburb").value == null) || (document.getElementById("ctl00_maincontent_txtSuburb").value == 'null'))) {
                document.getElementById("ctl00_maincontent_txtSuburb").style.borderColor = "red";
                document.getElementById("ctl00_maincontent_txtSuburb").focus();
                isSuburbEmpty = true;
            }
            else {
                document.getElementById("ctl00_maincontent_txtSuburb").style.borderColor = "ActiveBorder";
            }

            if ((isCompanyEmpty == true) || (isStateEmpty == true) || (isPostcodeEmpty == true) || (isSuburbEmpty == true)) {
                $find('mpeFirmMessageBox').show();
                document.getElementById('ctl00_maincontent_lblMessage').innerHTML = 'Fill required fields before submit!';


             
                return false;
            }
            else {
             
                return true;
            }




        }
        function counterPickup() {
     
       
          
            var appendthis = ("<div class='modal-overlay js-modal-close'></div>");
            //  e.preventDefault();
            $("body").append(appendthis);
            $(".modal-overlay").fadeTo(500, 0.7);
            $(".modal-box").css({ "display": "block" });
            //$(".js-modalbox").fadeIn(500);
            var modalBox = $(this).attr('data-modal-id');
            $('#' + modalBox).fadeIn($(this).data());
            $("[id=divchangepopup]").hide();
            var input = $("#<%=txtMobileNumber.ClientID%>");
            //         var input = $("#ctl00_MainContent_txtchangemobilenumber");
            
            input[0].focus();
            //        $(".modal-box").attr("display", "block");
            //     $(".modal-box").css({ "display": "block" });
            //new line set visile property to popup
            // document.getElementById("A1").click();

        }
        function savemobilenumer() {
            
            var mblnumber = document.forms[0].elements["<%=txtMobileNumber.ClientID%>"].value;

            if (document.getElementById('ctl00_maincontent_txtMobileNumber').value.trim() == "" ) {
                document.getElementById('lblerror_mo_save').style.display = "block";

                return false;
            }
            else if (mblnumber.substring(0, 2) != "04" || mblnumber.length != 10) {
                document.getElementById('lblerror_mo_save').style.display = "none";

                return false;
            }
            else {
                var chksave = document.getElementById("<%= chksavemobile.ClientID %>").checked;
                $.ajax({
                    type: "POST",
                    url: "/shipping.aspx/MobileNoChange_Click",
                    data: "{'isuserchecked':'" + chksave + "','mobileno':'" + document.getElementById('ctl00_maincontent_txtMobileNumber').value + "'}",

                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        if (data.d != "") {

                            document.getElementById('<%=lblorderready.ClientID%>').innerHTML = data.d;
                            $('#ctl00_maincontent_txtMobileNumber').val(data.d);
                        }
                        document.getElementById("ctl00_maincontent_smspopup").style.display = 'inline-block';
                        $(".modal-box, .modal-overlay").fadeOut(500, function () {

                            $(".modal-overlay").remove();
                        });

                        checkforpostback_afterclick();
                    },
                    error: function (xhr, status, error) {
                        var err = eval("(" + xhr.responseText + ")");
                  //      alert(err);
                    }

                })
                return false;
            }
        }
        function updatemobilenumer() {
            var mblnumber = document.forms[0].elements["<%=txtchangemobilenumber.ClientID%>"].value;
            if (document.getElementById('ctl00_maincontent_txtchangemobilenumber').value.trim() == "") {
              
                document.getElementById('lblerror_mo').style.display = "block";
               
                return false;
            }
            else if (mblnumber.substring(0, 2) != "04" || mblnumber.length != 10) {
                document.getElementById('lblerror_mo').style.display = "none";
                return false;
            }
            else {
             
                document.getElementById('lblerror_mo').style.display = "none";
                var chksave = document.getElementById("<%= cbmobilechange.ClientID %>").checked;
                $.ajax({
                    type: "POST",
                    url: "/shipping.aspx/MobileNoChange_Click",
                    data: "{'isuserchecked':'" + chksave + "','mobileno':'" + document.getElementById('ctl00_maincontent_txtchangemobilenumber').value + "'}",

                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {


                        //  var res = data.d.split("-");
                     
                        
                            document.getElementById("<%=lblorderreadytext.ClientID%>").innerHTML = "SMS Order ready notification message will be sent to:";
                            document.getElementById('<%=lblorderready.ClientID%>').innerHTML = data.d;
                            $('#ctl00_maincontent_txtMobileNumber').val(data.d)
                            $(".modal-box-change, .modal-overlay").fadeOut(500, function () {

                                $(".modal-overlay").remove();
                            });
                            $(".modal-box-change").css({ "display": "none" });

                            document.getElementById("ctl00_maincontent_smspopup").style.display = 'inline-block';


                    },
                    error: function (xhr, status, error) {
                        var err = eval("(" + xhr.responseText + ")");
                   //     alert(err);
                    }
                })

                return false;
            }
        }
        function ShowModal() {
            $("#divchangepopup").show();
           
            var appendthis = ("<div class='modal-overlay js-modal-close'></div>");
            //  e.preventDefault();
            $("body").append(appendthis);
            $(".modal-overlay").fadeTo(500, 0.7);
            $(".modal-box").css({ "display": "none" });
            $(".modal-box-change").css({ "display": "none" });
            
            //$(".js-modalbox").fadeIn(500);
            var modalBox = $(this).attr('data-modal-id');
            $('#' + modalBox).fadeIn($(this).data());
            $("[id=divchangepopup]").show();
            $("[id=popup1]").hide();
            var chmobile = document.getElementById("<%=lblorderready.ClientID%>").innerHTML;
        
            if (chmobile.length !=10) {
               
               document.getElementById("ctl00_maincontent_txtchangemobilenumber").value = document.getElementById("ctl00_maincontent_txtMobileNumber").value;
            }
           
           // document.getElementById("ctl00_maincontent_txtchangemobilenumber").value = chmobile;
            var input = $("#<%=txtchangemobilenumber.ClientID%>");
            //         var input = $("#ctl00_MainContent_txtchangemobilenumber");
            var len = input.val().length;
            input[0].focus();
            input[0].setSelectionRange(len, len);
            document.getElementById("ctl00_maincontent_RegularExpressionValidator1").value = "";
             return true;
        }
        function btnNoThanksChange() {
            var chksave = document.getElementById("<%= cbmobilechange.ClientID %>").checked;
            $.ajax({
                type: "POST",
                url: "/shipping.aspx/NoChange_Click",
                data: "{'isuserchecked':'" + chksave + "','mobileno':''}",

                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    
                       
                        //  var res = data.d.split("-");
                        document.getElementById('<%=lblorderready.ClientID%>').innerHTML = data.d;

//                        $('#ctl00_maincontent_txtMobileNumber').val(data.d)
                       
                   


                },
                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                //    alert(err);
                }
            })
        
            $(".modal-box-change, .modal-overlay").fadeOut(500, function () {

                $(".modal-overlay").remove();
            });
            $(".modal-box-change").css({ "display": "none" });
            document.getElementById("ctl00_maincontent_smspopup").style.display = 'inline-block';
            document.getElementById("ctl00_maincontent_txtchangemobilenumber").value = "";
            document.getElementById('<%=lblorderready.ClientID%>').innerHTML = "";

            document.getElementById("<%=lblorderreadytext.ClientID%>").innerHTML = "SMS Order ready notification will NOT be sent.";
         
            return false;
        }
        function noThanksBtn() {


            $.ajax({
                type: "POST",
                url: "/shipping.aspx/NoChange_Click",
                data: "{'isuserchecked':'false','mobileno':''}",

                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {


                    //  var res = data.d.split("-");
                    document.getElementById("ctl00_maincontent_txtMobileNumber").value = "";
         

                    //                        $('#ctl00_maincontent_txtMobileNumber').val(data.d)


                    checkforpostback_afterclick();

                },
                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
               //     alert(err);
                }
            })
        


            $(".modal-box, .modal-overlay").fadeOut(500, function () {
              
                $(".modal-overlay").remove();

            });
            $(".modal-box").css({ "display": "none" });
            document.getElementById("ctl00_maincontent_txtMobileNumber").value = "";
         
        return false;
            }

</script>
    <script  type="text/javascript">
        function creditcardclick() {
            document.getElementById("ctl00_maincontent_divcreditcard").style.display = 'block';
            document.getElementById("ctl00_maincontent_divdedault").style.display = 'none';

            document.getElementById("ctl00_maincontent_divpaypal").style.display = 'none';
            try
            {
    //  document.getElementById("ctl00_maincontent_ImageButton2").style.display = 'none';
  }
  catch (Error)
                 { }
                 //var sectionID = "ctl00_maincontent_divcreditcard";
//                 alert(sectionID);
//                 $('html, body').animate({
//        scrollTop: $('#' + sectionID ).offset().top}, 500)
//});

                 document.getElementById("ctl00_maincontent_divcreditcard").focus();
            //$("#lblcreditcard").addClass("active");
            //$("#lbldefaultpayment").removeClass("active");
            //$("#lblpaypalcard").removeClass("active");

        }
        function paypalclick() {
            try {
                document.getElementById("ctl00_maincontent_divpaypal").style.display = 'block';

                document.getElementById("ctl00_maincontent_divdedault").style.display = 'none';
                document.getElementById("ctl00_maincontent_divcreditcard").style.display = 'none';
                try
                {
               // document.getElementById("ctl00_maincontent_ImageButton2").style.display = 'none';
            }
            catch (Error)
                 { }

                document.getElementById("ctl00_maincontent_div2").style.display = 'none';
            }
            catch (error) {
               // document.getElementById("ctl00_maincontent_div2").style.display = 'none';
            }
            document.getElementById("ctl00_maincontent_btnPay").focus();
            //$("#lblcreditcard").removeClass("active");
            //$("#lbldefaultpayment").removeClass("active");
            //$("#lblpaypalcard").addClass("active");

        }

        function Defaultclick() {
            try {
                document.getElementById("ctl00_maincontent_divdedault").style.display = 'block';
                document.getElementById("ctl00_maincontent_divcreditcard").style.display = 'none';
                document.getElementById("ctl00_maincontent_divpaypal").style.display = 'none';
                document.getElementById("<%=RBpaymenttype.ClientID %>").Checked = true;
                try{
               // document.getElementById("ctl00_maincontent_ImageButton2").style.display = 'none';
                    document.getElementById("ctl00_maincontent_div2").style.display = 'none';
            }
            catch (Error)
                 { }
               // document.getElementById("ctl00_maincontent_div2").style.display = 'none';
            }
            catch (error) {
               // document.getElementById("ctl00_maincontent_div2").style.display = 'none';
            }
            //$("#creditcardclick").removeClass("active");

            //$("#paypalclick").removeClass("active");
        }
//        function Defaultclick_int() {
//          //  document.getElementById("ctl00_maincontent_divintdirdep").style.display = 'block';
//            document.getElementById("ctl00_maincontent_divinternationalpaypal").style.display = 'none';
//            document.getElementById("divremotesecurepay").style.display = 'none';

//        }
//        function paypalclick_int() {
//            document.getElementById("ctl00_maincontent_divinternationalpaypal").style.display = 'block';
//           // document.getElementById("ctl00_maincontent_divintdirdep").style.display = 'none';
//            document.getElementById("divremotesecurepay").style.display = 'none';
//        }
//        function creditcardclick_remote() {
//            document.getElementById("divinternationalpaypal").style.display = 'none';
//          //  document.getElementById("divintdirdep").style.display = 'none';
//            document.getElementById("divremotesecurepay").style.display = 'block';

//        }


        function Bgcolorchange() {
           
        }
        function isAlphabetic() {

            var ValidChars = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890/\\';
            var sText = document.getElementById("ctl00_maincontent_tt1").value;
            var IsAlphabetic = true;
            var Char;
            var ErrMsg = 'Sorry, but the use of the following special characters is not allowed in the Order No field:' + '\n' + '! ` & ~ ^ * %  $ @ � ( � ) ; [   ] { } ! = < > | * , . -' + '\n' + 'Please update your order no so it longer has any of these restricted characters in it to continue order.';
            var err = document.getElementById("ctl00_maincontent_txterr");
            if (err != null) {
                err.innerHTML = '';
            }
            for (i = 0; i < sText.length; i++) {
                Char = sText.charAt(i);

                if (ValidChars.indexOf(Char) == -1) {
                    alert(ErrMsg);
                    document.getElementById("ctl00_maincontent_tt1").value = '';
                    document.forms[0].elements["<%=tt1.ClientID%>"].focus();
                    return false;
                }
            }

            return isAlphabetic;
        }
        function checkcreditmat() {
        
        if (document.forms[0].elements["<%=tt1.ClientID%>"].value.length == 0 && document.forms[0].elements["<%=hftt1.ClientID%>"].value == "1") {
        $find('mpeFirmMessageBox').show();
           // document.getElementById('ctl00_maincontent_lblMessage').style.display = "block";
        document.getElementById('ctl00_maincontent_lblMessage').innerHTML = 'Please Enter Order No';
            document.forms[0].elements["<%=tt1.ClientID%>"].focus();

            return false;
        }
//        if ((document.forms[0].elements["<%=TextBox1.ClientID%>"].value == msgCheck || document.forms[0].elements["<%=TextBox1.ClientID%>"].value == "" || document.forms[0].elements["<%=TextBox1.ClientID%>"].value == null) && document.forms[0].elements["<%=drpSM1.ClientID%>"].value == "Courier Pickup") {
//            //alert('Please Enter Comments and Submit Order');
//            //ShowCourierMessage();
//            document.forms[0].elements["<%=TextBox1.ClientID%>"].focus();
//            document.getElementById("ctl00_maincontent_hfispopup").value = 0;

//            return false;
//        }

        if (document.getElementById("ctl00_maincontent_txtcreditcardno").value == "") {

            document.getElementById("ctl00_maincontent_txtcreditcardno").focus();
            document.getElementById("ctl00_maincontent_RBcreditcardno").style.visibility = "visible";
            return false;
        }
        if (document.getElementById("ctl00_maincontent_txtCVV").value == "") {

            document.getElementById("ctl00_maincontent_txtCVV").focus();
            document.getElementById("ctl00_maincontent_RBCVV").style.visibility = "visible";
            return false;
        }
        if (document.getElementById("ctl00_maincontent_txtnamecard").value == "") {

            document.getElementById("ctl00_maincontent_txtnamecard").focus();
            document.getElementById("ctl00_maincontent_RBCardname").style.visibility = "visible";
            return false;
        }
    return checkorderid();
       
        
        }
        function checkorderid() {
            var msgCheck = "**** NOTE ****" + '<br>' + "Courier Pick Up Service needs to be arranged by you." + '\n' + "Please enter the details of Courier Company that you will be arranging to pick up your parcel from us with.";
            if (document.forms[0].elements["<%=tt1.ClientID%>"].value.length == 0 && document.forms[0].elements["<%=hftt1.ClientID%>"].value == "1") {
                $find('mpeFirmMessageBox').show();
                document.getElementById('ctl00_maincontent_lblMessage').innerHTML = 'Please Enter Order No';
                document.forms[0].elements["<%=tt1.ClientID%>"].focus();

                return false;
            }
           else if (document.forms[0].elements["<%=hfduporderproceed.ClientID%>"].value == "1") {
                return true;
            }
            else {

                var x = checkordeidexist(document.forms[0].elements["<%=tt1.ClientID%>"].value);
               
                 if (x == false) {
                    return false;
                }
              
               
            }
            //            else 
            if (document.forms[0].elements["<%=drpSM1.ClientID%>"].value == "Please Select Shipping Method") {
              
                // $("#pnlMessageBox").disp();
                document.forms[0].elements["<%=drpSM1.ClientID%>"].style.border= 'thin solid red';
              
                $find('mpeFirmMessageBox').show();
                document.getElementById('ctl00_maincontent_lblMessage').innerHTML = 'Please Select Shipping Method';
                
               // document.forms[0].elements["<%=lblMessage.ClientID%>"].value = "Please Select Shipping Method";
                document.forms[0].elements["<%=drpSM1.ClientID%>"].focus();
               
                return false;
            }
            else if ((document.forms[0].elements["<%=TextBox1.ClientID%>"].value == msgCheck || document.forms[0].elements["<%=TextBox1.ClientID%>"].value == "" || document.forms[0].elements["<%=TextBox1.ClientID%>"].value == null) && document.forms[0].elements["<%=drpSM1.ClientID%>"].value == "Courier Pickup") {
                //alert('Please Enter Comments and Submit Order');
                ShowCourierMessage();
                document.forms[0].elements["<%=TextBox1.ClientID%>"].focus();
                document.getElementById("ctl00_maincontent_hfispopup").value = 0;
                
                return false;
            }
            else {
             
                var numaric = document.forms[0].elements["<%=tt1.ClientID%>"].value;
                for (var j = 0; j < numaric.length; j++) {
                    var alphaa = numaric.charAt(j);
                    var hh = alphaa.charCodeAt(0);
                    //hh == 95 || hh == 45
                    if ((hh > 47 && hh < 58) || (hh > 64 && hh < 91) || (hh > 96 && hh < 123) || hh == 92 || hh == 47) {
                    }
                    else {
                        if (hh == 32) {
                            //alert("Blank space in order no");
                            $find('mpeFirmMessageBox').show();
                            document.getElementById('ctl00_maincontent_lblMessage').innerHTML = 'Blank space in order no';

                        }
                        else {
                            alert("Please enter valid order no, Order no should have alpha-numeric character.");
                            $find('mpeFirmMessageBox').show();
                            document.getElementById('ctl00_maincontent_lblMessage').innerHTML = 'Please enter valid order no, Order no should have alpha-numeric character.';

                        }
                        return (ValidationDropShipOrder());
                    }
                }
                   
            }

            return (ValidationDropShipOrder());
    }
     function checkordeidexist(refid) {
         var l=false;
         $.ajax({
             type: "POST",
             async:false,
             url: "/shipping.aspx/CheckOrderID",
             data: "{'refid':'" + refid + "'}",
             
             contentType: "application/json; charset=utf-8",
             dataType: "json",
             success: function (data) {

                 if (data.d == "0") {

                    
                     $find('mpeFirmMessageBox').show();
                     document.getElementById('ctl00_maincontent_lblMessage').innerHTML = 'Order No already exists, please Re-enter Order No';
                     document.forms[0].elements["<%=tt1.ClientID%>"].focus();

                     l = false;
                 }

               else  if (data.d == "2") {

                   document.getElementById('duppopup').style.display = "block";
                  
                 }
                 else {

                     l = true;
                     //return (ValidationDropShipOrder());
                 }

             },
             error: function (xhr, status, error) {
                 var err = eval("(" + xhr.responseText + ")");
       //          alert(err);
             }

         })
         return l;     
     }
        function EnterNeworderno()
        {
            
            document.forms[0].elements["<%=tt1.ClientID%>"].value="";
            document.forms[0].elements["<%=tt1.ClientID%>"].focus();
            document.getElementById('duppopup').style.display = "none";
        }
        function ordernoproceed() {
            document.forms[0].elements["<%=hfduporderproceed.ClientID%>"].value = "1";
            document.getElementById('duppopup').style.display = "none";

        }
    $(document).ready(function () {
        if ($find("ShipmentModelPopupExtender").hide() != null) {
            $find("ShipmentModelPopupExtender").hide();
        }
    });
        

        function ShowCourierMessage() {
            var msg = "**** NOTE ****" + '<br>' + "Courier Pick Up Service needs to be arranged by you." + '\n' + "Please enter into the Comments / Notes box the details of Courier Company that you will be arranging to pick up your parcel from us with.";
            $find('mpeFirmMessageBox').show();
            document.getElementById('ctl00_maincontent_lblMessage').innerHTML = msg;
            document.getElementById("ctl00_maincontent_hfispopup").value = 1;
            return false;
          //  alert(msg);
        }
        function ShowMailMessage() {
            var msg = "**** NOTE ****" + '<br>' + " Mail will be used for parcels up to 500 grams including packaging. Parcels over 500 grams will be sent by the most economical way e.g. Courier, Road, etc. ";
            $find('mpeFirmMessageBox').show();
            document.getElementById('ctl00_maincontent_lblMessage').innerHTML = msg;
            document.getElementById("ctl00_maincontent_hfispopup").value = 1;
            return false;
            //  alert(msg);
        }

    

        function MouseHover() {
            $find("ShipmentModelPopupExtender").show();
            //new line set visile property to popup
            $(".ModalPopupStyleshi").css({ "visibility": "visible" });


        }

        function MouseOut() {
            $find("ShipmentModelPopupExtender").hide();
        }

        function MouseHover_orderno() {

            document.getElementById("OrderNoPopupPanel").style.display = 'block';
            //$find("OrderNoPopupPanel").show();
            ////new line set visile property to popup
            //$(".ModalPopupStyleshi").css({ "visibility": "visible" });
            //$(".ModalPopuporder").css({ "visibility": "visible" });
        }

        function MouseOut_orderno() {
            document.getElementById("OrderNoPopupPanel").style.display = 'none';
        }
    
    </script>

    <script type="text/javascript">
        //Added by smith to remove special characters in comments box on 13-09-2018
        function keyboardup(id) {
            var value = id.value
            // id.value = value.replace(/'/, '`');
            var e = e || window.event;

            if (e.keyCode == '37' || e.keyCode == '38' || e.keyCode == '39' || e.keyCode == '40' || e.keyCode == '8') {
            }
            else {
                id.value = value.replace(/'/g, "`")
            }
            return Validate(id);
        }
        function Validate(txt) {

            //if (txt != null && txt.value != "" && !txt.value.match(/^[a-zA-Z0-9?=.,:*!@#$%^&*_\-\s]+$/)) {

            if (txt != null && txt.value != "" && !txt.value.match(/^[a-zA-Z0-9.,:;\s]+$/)) {
                var lastChar = txt.value[txt.value.length - 1];
                if (!lastChar.match(/^[a-zA-Z0-9.,:;\s]+$/)) {

                    if (!txt.value.match("/")) {
                        txt.value = txt.value.replace(lastChar, '');
                        if (!txt.value.match(/^[a-zA-Z0-9.,:;\s]+$/)) {
                            alert("Invalid Text : '" + lastChar + " '");
                            txt.value = '';
                        }
                        else {
                            alert("Invalid Text : ' " + lastChar + " '");
                        }
                    }
                }
                else {
                    if (!txt.value.match("/")) {
                        alert("Invalid Text : '" + txt.value + " '");
                        txt.value = '';
                    }
                }
            }
            else {
                return true;
            }
        }
    </script>


    <script type="text/javascript">
        function DRPshippment() {
           // alert('Non-Standard Delivery Area. We will contact you to confirm costing');
            $find('mpeFirmMessageBox').show();
            document.getElementById('ctl00_maincontent_lblMessage').innerHTML = 'Non-Standard Delivery Area. We will contact you to confirm costing';

        }
        function couponcodekeypress(e) {

            var bv = document.getElementById("<%=txtCouponCode.ClientID%>");
          var lblerr = document.getElementById("<%=lblcouponerrmsg.ClientID%>");
          //alert(bv);
          bv.setAttribute("style", "border-color:#73ACCF #88CEF9 #88CEF9 !important;")
          //bv.style.border = "1px solid";
          //bv.style.borderColor = "rgb(178, 178, 178)"
          lblerr.setAttribute("style", "display:none;")
      }
      function couponcodeError() {
          var bv = document.getElementById("<%=txtCouponCode.ClientID%>");
          bv.style.border = "1px solid";
          bv.style.borderColor = "red"
      }
    </script>
    <style type="text/css">

.MessageBoxPopUp

{

    background-color:White;

    border:solid 2px #99B4D1;

}

 

.MessageBoxButton

{


 background-color:White;
border: solid 2px #99B4D1;



font-weight:bold;

font-family:Verdana;

font-size:9pt;

cursor:pointer;

height: 20px; 



display:block;

}

 

.MessageBoxHeader

{

 height:17px;

 font-size:10pt;

 color:White; 

 font-weight:bold;

 font-family:Verdana; 

 text-align:Left;

 vertical-align:middle;

 padding:3px 3px 3px 3px;

 background-color:#3399FF;

 border-bottom:2px solid #0099DA;

}

 

.MessageBoxData

{

 height:20px;

 font-size:10pt;

 font-family:Verdana;

 color:#3A4349; 

 text-align:Left;

 vertical-align:top;

}

 

 

</style>

<script type="text/javascript">

    function pageLoad(sender, args) {
        if (!args.get_isPartialLoad()) {
            //  add our handler to the document's
            //  keydown event
            $addHandler(document, "keydown", onKeyDown);
        }
    }

    function onKeyDown(e) {
        if (e && e.keyCode == Sys.UI.Key.esc) {
           
            // if the key pressed is the escape key, dismiss the dialog
            $find('mpeFirmMessageBox').hide();
            //alert(document.getElementById("ctl00_maincontent_hfispopup").value);
            console.log(document.getElementById("ctl00_maincontent_hfispopup").value);
            if (document.getElementById("ctl00_maincontent_hfispopup").value == '1') {
                // alert(document.getElementById("ctl00_maincontent_hfispopup").value);
                console.log(document.getElementById("ctl00_maincontent_hfispopup").value);
                document.getElementById("ctl00_maincontent_hfispopup").value = 0;
                _doPostBack();
               // window.reload();
                //location.reload();
                //return true;
            }


//            if (document.getElementById("ctl00_maincontent_drpSM1").value == 'Mail' || document.getElementById("ctl00_maincontent_drpSM1").value == 'Courier Pickup') {
//                __doPostBack();
//                //return true;
//            }
       
        }
    }

    function closeModelPopup(btn) {

//        if (document.getElementById("ctl00_maincontent_drpSM1").value == 'Mail' || document.getElementById("ctl00_maincontent_drpSM1").value == 'Courier Pickup') {
//            __doPostBack();
//            //return true;
//        }
       
        if (document.getElementById("ctl00_maincontent_hftt1").value == '1' && document.getElementById("ctl00_maincontent_tt1").value == '') {
          
            var bv = document.getElementById("<%=tt1.ClientID%>");
         
            bv.setAttribute("style", "margin-left: 15px; width: 210px;border-color:red !important;")
            //        document.forms[0].elements["<%=tt1.ClientID%>"].removeAttribute("class");
            //        document.forms[0].elements["<%=tt1.ClientID%>"].style.border = 'thin solid red';
            document.forms[0].elements["<%=tt1.ClientID%>"].focus();
        }
        else if (document.getElementById("ctl00_maincontent_hftt1").value == '1') {
            document.forms[0].elements["<%=tt1.ClientID%>"].focus();
        }
        else if (document.getElementById("ctl00_maincontent_TextBox1").value == '')
        {
            document.forms[0].elements["<%=TextBox1.ClientID%>"].focus();
        }

    
            $find('mpeFirmMessageBox').hide();
          
            if (document.getElementById("ctl00_maincontent_hfispopup").value == '1' && document.getElementById("ctl00_maincontent_drpSM1").value != 'Mail') {
                document.getElementById("ctl00_maincontent_hfispopup").value = 0;
                __doPostBack();
                //return true;
            }



//                if (document.getElementById("ctl00_maincontent_drpSM1").value == 'Mail' || document.getElementById("ctl00_maincontent_drpSM1").value == 'Courier Pickup') {
//                    __doPostBack();
//                    //return true;
//                }
       
    }
    function ttleave() {
        if (document.getElementById("ctl00_maincontent_tt1").value != '') {
            var bv = document.getElementById("<%=tt1.ClientID%>");

            //alert(bv);
            bv.setAttribute("style", "margin-left: 15px; width: 210px;border-color:#73ACCF #88CEF9 #88CEF9 !important;")
        }
       // checkorderid();
     }
//    function blockspecialcharforOrder() {
//        if (document.getElementById("ctl00_maincontent_tt1").value != '') {
//            document.forms[0].elements["<%=tt1.ClientID%>"].addclass('input_dr');
//        }
//    }

</script>
    <asp:Button ID="btnTemp" runat="server" Style="display: none;" />
    <cc1:ModalPopupExtender ID="mpeMessageBox" runat="server" DynamicServicePath="" Enabled="True"

    TargetControlID="btnTemp" PopupControlID="pnlMessageBox" BackgroundCssClass="modal"

    PopupDragHandleControlID="pnlMessageBox" CancelControlID="btnCancel" BehaviorID="mpeFirmMessageBox">

</cc1:ModalPopupExtender>

<%--<asp:Panel ID="pnlMessageBox" runat="server" Style="display: none; width: 300px;

    height: 80px;" class="MessageBoxPopUp">

    <table border="0" cellpadding="0" cellspacing="0" width="100%">

        <tr class="MessageBoxHeader" style="height: 17px;">

            <td colspan="2">

                <asp:Label ID="lblHeader" runat="server"></asp:Label>

            </td>

            <td align="right" style="padding: 2px 2px 0px 0px;">

            

           

            </td>

        </tr>

        <tr>

            <td colspan="2" style="height: 5px;">

            </td>

        </tr>

        <tr >

            <td style=" padding-left: 5px;">

                <asp:Image ID="imgInfo" runat="server" ImageUrl="~/Images/info1.png" />

            </td>

            <td class="MessageBoxData" colspan="2" style=" width:200px;padding: 10px 5px 5px 5px;">

                <asp:Label ID="lblMessage" runat="server"></asp:Label>

            </td>

        </tr>
  <tr style="vertical-align: bottom; height: 20px; padding: 0px 5px 5px 0px;">

            <td style="width: 40px;">

            </td>

       <td align="right" style="width: 180px">

                <asp:Button ID="btnok" runat="server" CssClass="MessageBoxButton"  OnClientClick="closeModelPopup(this)"  />

            </td>

            <td align="right">

          
            </td>

        </tr>

    </table>

</asp:Panel>--%>
   
    
    <asp:Panel ID="pnlMessageBox" runat="server" Style="display: none; width: 360px; text-align:center;

    max-height: 250px; overflow:auto;padding-bottom:20px" class="MessageBoxPopUp">

    <table border="0" cellpadding="0" cellspacing="0" width="100%">

        <tr class="MessageBoxHeader" style="height: 17px;">

            <td colspan="2">
            
                 <asp:Image ID="imgInfo" runat="server" ImageUrl="~/Images/info_msg.png"  />
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

        <tr >

          

            <td class="MessageBoxData" colspan="2" style=" width:100%;padding: 15px; text-align:center;" >


                <asp:Label ID="lblMessage" runat="server"></asp:Label>

            </td>

        </tr>
  <tr style="vertical-align: bottom; height: 20px; padding: 0px 5px 5px 0px;">

         

       <td align="right" style="width:56px;
text-align: center;
margin: 0 auto;
    margin-top: -15px;
display: block;"
>

                <input type="button" id="Button1"  class="btnbuy2 button btngreen" onclick="closeModelPopup(this)"  value="Ok" />

            </td>

        

        </tr>

    </table>

</asp:Panel>

    
     <div class="breadcrumb_outer1">
        <a href="home.aspx" style="float: left" class="toplinkatest" style="text-decoration: none!important;">HOME >&nbsp;</a>
        <div class="breadcrumb1">
            <a href="<% =HttpContext.Current.Request.Url.ToString()
                  %>"
                class="breadcrumb_txt1" style="text-transform: none;">Shipping</a>
            <a href="home.aspx" class="breadcrumb_close1"></a>
        </div>
    </div>


    <table width="100%" cellspacing="0" cellpadding="5" align="center" border="0">
        <tr>
            <td align="left">


                <div class="box1" style="width: 760px; margin: 0 0 0 2px;">
                    <asp:PlaceHolder runat="server" ID="PHOrderConfirm" Visible="false" EnableViewState="false">
                        <%-- <table cellpadding="0" cellspacing="0" border="0" width="765" style="background: none repeat scroll 0 0 #319731;border-radius: 5px 5px 5px 5px;">
            <tr>--%>
                        <% 
                    
                            if (Convert.ToInt16(Session["USER_ROLE"]) == 3)
                            {
                        
                        
                        %>
                        <%--<td align="center" style="text-align: center; padding-top: 5px; padding-right: 30px;padding-bottom: 10px;">--%>
                        <div class="alert yellowbox icon_1">
                            <h3 style="font-size: 16px;">Order Now Pending Approval</h3>
                            <p class="p2">
                                Your order is now pending approval form your company supervisor/s before it can be submitted to us for processing.
                       <br>
                                The following member/s in your company will be able to authorise and submite your order:
                      <br>
                                <asp:Label ID="lblUserRoleName" runat="server" Visible="true" Font-Names="Arial" Font-Size="11px" ForeColor="Black" Font-Bold="true" Text=""></asp:Label>
                            </p>
                        </div>

                        <%--  </td>--%>
                        <%-- <td style="height: 135px; background-repeat: no-repeat;  background-color:#ffdb01;"
                    valign="top">
                    <img src="images/Test1.jpg" alt="" width="80px" />
                </td>
                <td style="width: 5px; height: 135px;  background-color:#ffdb01; border: 0;
                    background-repeat: no-repeat;" align="left" valign="top">
                </td>
                <td style="width: 565px; height: 135px; background-color:#ffdb01;
                    border: 0; background-repeat: no-repeat;" align="left" valign="top">
                    <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;
                        font-family: Arial; font-size: 12px; text-align: center;" >
                        <tr>
                            <td align="center" style="font-size: 16px; padding-right: 10px; padding-top: 10px;">
                                <b>Order Now Pending Approval </b>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="text-align: center; padding-top: 10px; padding-right: 15px;
                                font-family: Arial; font-size: 13px">
                                <b>Your Order is now pending approval from your company supervisor/s before it
                                    <br />
                                    can be submitted to us for processing. The following member/s in your company
                                    <br />
                                    will be able to authorise and submit your order: </b>
                                <br />
                            </td>
                        </tr>

                        <tr>
                            <td align="center" style="text-align: center; padding-top: 5px; padding-right: 30px;
                                padding-bottom: 10px;">
                                <asp:Label ID="lblUserRoleName" runat="server" Visible="true" Font-Names="Arial"
                                    Font-Size="14px" ForeColor="Black" Font-Bold="true" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 135px; height: 135px;  background-color:#ffdb01;border: 0;
                    background-repeat: no-repeat;" align="left" valign="top">
                </td>--%>



                        <% 
                        Session["ORDER_ID"] = "0";
                        Session["Multipleitems"] = null;
                    }
                    else if (Convert.ToInt16(Session["USER_ROLE"]) == 1 || Convert.ToInt16(Session["USER_ROLE"]) == 2)
                    {
                        %>
                        <%-- <td width="100%" style=" height: 49px; background-image: url(images/logoOrder.jpg);
                    background-repeat: no-repeat; background-position: 12px center;" align="center" valign="middle"  >
                    <asp:Label ID="Label9" runat="server" Visible="true" Font-Names="Arial" Font-Size="14px"
                        ForeColor="White" Text="Your Order Has Been Sent To Us For Processing.Thank You!"></asp:Label>
                </td>--%>
                        <%-- <td>--%>
                        <div class="alert greenbox icon_2" style="padding: 30px 10px 30px 60px" runat="server" id="greenalert">
                            <h3 style="font-size: 16px;">

                                <%--  Your order has been successfully submitted to us for processing. Thank You!</h3>--%>
                                <asp:Label ID="lblgreenalert" runat="server" Text="Your order has been successfully submitted to us for processing. Thank You!" />
                        </div>



                        <div style="display:none">
                            <img src="images/img_flash/Banner-Order-Confirmation.gif" alt="" width="760px" height="300px" />
                        </div>
                        <%--</td>--%>
                        <% 
                        Session["ORDER_ID"] = "0";
                        Session["Multipleitems"] = null;
                    }
                     
                        %>
         
           

          
                    </asp:PlaceHolder>

                    <div class="alert redbox " runat="server" visible="false" id="divonlinesubmitordererror">
                        <div style="display: inline-block; float: left; margin-right: 30px;">
                            <img src="/images/Payment_Alert.jpg">
                        </div>
                        <div style="float: left; color: #fff; font-size: 13px;">
                            <b style="font-size: 15px; line-height: 2;">Your Account has been Stopped! </b>
                            <p style="line-height: 1.5; margin: 0;">
                                There are outstanding payments on your account that needs to be resolved ASAP!
                                <br>
                                Please contact our accounts department by email
                <a style="color: #fff; text-decoration: underline;" href="mailto:accounts@wes.com.au">accounts@wes.com.au</a>
                                or Phone on 02-9797-9866
                            </p>
                        </div>
                        <div class="clear"></div>
                    </div>
                    <p class="p2 fright"><span class="redx">* </span>Required Fields</p>
                    <div class="clear"></div>
                    <div class="quickorder4">
                        <table width="100%" runat="server" cellpadding="0" cellspacing="0" border="0" class="shippingtable">
                            <tr>
                                <td style="font-size: medium; color: #0099DA; width: 43%">
                                    <b>Enter Your Purchase Order No</b>
                                </td>
                                <td>

                                    <%--   <asp:ModalPopupExtender ID="OrderNoPopupExtender" runat="server" BehaviorID="OrderNoModelPopupExtender" TargetControlID="HyperLink1"
            PopupControlID="OrderNoPopupPanel" OkControlID="btnOk" BackgroundCssClass="modalBackground">
        </asp:ModalPopupExtender>--%>
                                    <%--   <asp:Panel ID="OrderNoPopupPanel" runat="server" CssClass="ModalPopuporder">--%>
                                    <div class="ModalPopuporder" id="OrderNoPopupPanel" style="display: none" onclick="MouseOut_orderno()">
                                        <table width="100%" cellspacing="0" cellpadding="0" border="0" style="border-collapse: collapse; text-align: left;">
                                            <tr>
                                                <td>
                                                    <p class="TableColumnStyle" style="margin-top: 0px">
                                                        Enter your own Order Reference Number.
                                <br />
                                                        Enable or Disable the �Order No� field as being 
                                <br />
                                                        Mandatory during the check out of your order.
                                <br />
                                                        This option can only be enabled / disabled
                                <br />
                                                        by your Company Admin in: 
                                <br />

                                                        <a style="color: #0099DA;" href="WebSiteSettings.aspx">My Account > Web Site Setup</a>
                                                    </p>

                                                </td>
                                            </tr>
                                        </table>

                                    </div>
                                    <%--</asp:Panel>--%>

                                    <div id="moreinfoorder" runat="server">
                                        <a class="link" href="#">

                                            <asp:HyperLink ID="HyperLink2" runat="server" onclick="MouseHover_orderno();" CssClass="HyperLinkStyleship">

                                                <img id="Img1" runat="server" src="~/images/info.jpg" alt="" onclick="MouseHover_orderno();" style="cursor: pointer;" />
                                                Learn More 
                                            </asp:HyperLink>
                                        </a>


                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>Order No 
                       <%-- <span class="redx"></span>--%>
                                    <asp:TextBox Style="margin-left: 15px; width: 210px" MaxLength="12" ID="tt1" runat="server" BackColor="White" CssClass="input_dr" onkeypress="blockspecialcharforOrder(event)"  onfocusout="ttleave()" />
                                 
                                    <div style="display: inline-block; vertical-align: top; margin-top: -6px" id="divmanorder" runat="server">
                                        <span class="maroonspan" style="margin-bottom: 6px;">*</span>
                                        
                                    </div>
                                </td>
                                <%-- <td  width="30%"> 
                   
                    </td>--%>
                                <td width="60%">

                                    <div id="divordermandatory" runat="server">



                                        <span class="maroonspan">Required Field:This field has been set as Mandatory by your Company Admin </span>


                                    </div>
                                </td>

                            </tr>
                            <tr>
                                <td>
                       
                                     <asp:HiddenField ID="hftt1" runat="server" value="0"/>
                                    <asp:Label Width="250px" ID="txterr" runat="server" ForeColor="red" />
                                    <asp:Label Width="250px" ID="txterr_1" runat="server" ForeColor="red" />
                                      

                                </td>

                            </tr>
                        </table>
                    </div>

                    <%--  <br />--%>
                    <div class="quickorder4">
                        <div class="form-col-8-8">
                            <h4 class="padbot10" style="font-size: medium; color: #0099DA;">Shipping</h4>
                        </div>
                        <div class="form-col-2-8">
                            <p class="p2">
                                Select Shipment Method
                                          <span class="redspan">*</span>
                            </p>
                        </div>
                        <div class="form-col-3-8">
                            <%--OnSelectedIndexChanged="drpSM1_SelectedIndexChanged"--%>
                            <asp:DropDownList NAME="drpSM1" Width="250px" ID="drpSM1" runat="server" CssClass="txtinput1"  onchange="return CheckShippment();"  OnSelectedIndexChanged="drpSM1_SelectedIndexChanged" AutoPostBack="true" >
                                <asp:ListItem Text="Please Select Shipping Method" Value="Please Select Shipping Method">Please Select Shipping Method</asp:ListItem>
                                <asp:ListItem Text="Courier" Value="Courier">Courier</asp:ListItem>
                                <asp:ListItem Text="Mail" Value="Mail">Mail</asp:ListItem>
                                <asp:ListItem Text="Courier Pickup" Value="Courier Pickup">Courier Pickup</asp:ListItem>
                                <asp:ListItem Text="Shop Counter Pickup" Value="Counter Pickup">Shop Counter Pickup</asp:ListItem>
                          
                                  </asp:DropDownList>
                           
                        </div>
                        <div class="form-col-3-8">
                            <a class="link" href="#">

                                <asp:HyperLink ID="HyperLink1" runat="server" onclick="MouseHover();" CssClass="HyperLinkStyleship">
                                    <img id="Img11" runat="server" src="~/images/info.jpg" alt="" onclick="MouseHover();" style="cursor: pointer;" />
                                    Learn More About Shipment Method Options 
                                </asp:HyperLink>
                            </a>
                        </div>
                        <div class="clear padbot10"></div>
                        <div id="OtherShipmentRow">
                            <div class="form-col-4-8">
                                <textarea id="Ta2" cols="34" class="textarea1" readonly="readonly" disabled="disabled" tabindex="-1" runat="server" rows="10"
                                    name="Ta2"></textarea>
                            </div>
                            <div class="form-col-4-8">
                                <textarea id="Ta3" cols="34" class="textarea1" readonly="readonly" disabled="disabled" tabindex="-1" runat="server" rows="10"
                                    name="Ta3"></textarea>
                            </div>
                        </div>
                        <div class="clear"></div>
                        <div id="DropShipmentRow" style="display:none">
                            <div class="alert yellowbox">
                                <h3 style="font-size: 16px;">Please Enter Shipment Delivery Details</h3>
                                <p class="p2 fright">
                                    <span class="red" style="color: #FF0000;">* </span>Required Fields
                                </p>
                                <div class="clear"></div>
                                <div class=" form-col-2-8">
                                    <p class="p2">Company Name</p>
                                </div>
                                <div class=" form-col-3-8">
                                    <asp:TextBox ID="txtCompany" runat="server" MaxLength="40" Width="242px" CssClass="input_dr" autocomplete="new-password"/>
                                    <asp:Label Width="150px" ID="Label13" runat="server" ForeColor="red"></asp:Label>
                                </div>
                                <div class="clear"></div>
                                <div class=" form-col-2-8">
                                    <p class="p2">
                                        Attn to / Receivers Code
                            <span class="red" style="color: #FF0000;">*</span>
                                    </p>
                                </div>
                                <div class=" form-col-3-8">
                                    <asp:TextBox ID="txtAttentionTo" runat="server" MaxLength="40" Width="242px" CssClass="input_dr" autocomplete="new-password" />
                                    <asp:Label Width="150px" ID="Label27" runat="server" ForeColor="red"></asp:Label>
                                </div>
                                <div class="clear"></div>
                                <div class=" form-col-2-8">
                                    <p class="p2">Receivers Contact Number</p>
                                </div>
                                <div class=" form-col-3-8">
                                    <asp:TextBox ID="txtShipPhoneNumber" runat="server" Width="242px" MaxLength="40" CssClass="input_dr"  autocomplete="new-password"/>
                                    <asp:Label Width="150px" ID="Label26" runat="server" ForeColor="red"></asp:Label>
                                </div>
                                <div class="clear"></div>
                                <div class=" form-col-2-8">
                                    <p class="p2">Address Line 1
                                      <span class="red" style="color: #FF0000;">*</span></p>
                                </div>
                                <div class=" form-col-3-8">
                                    <asp:TextBox ID="txtAddressLine1" runat="server" MaxLength="40" Width="242px" CssClass="input_dr" autocomplete="new-password" />
                                    <asp:Label Width="150px" ID="lbladdline1err" runat="server" ForeColor="red"></asp:Label>
                                </div>
                                <div class="clear"></div>
                                <div class=" form-col-2-8">
                                    <p class="p2">Address Line 2</p>
                                </div>
                                <div class=" form-col-3-8">
                                    <asp:TextBox ID="txtAddressLine2" runat="server" MaxLength="40" Width="242px" CssClass="input_dr"  autocomplete="new-password"/>
                                    <asp:Label Width="150px" ID="lbladdline2err" runat="server" ForeColor="red"></asp:Label>
                                </div>
                                <div class="clear"></div>
                                <div class=" form-col-2-8">
                                    <p class="p2">
                                        Suburb
                                <span class="red" style="color: #FF0000;">*</span>
                                    </p>
                                </div>
                                <div class=" form-col-3-8">
                                    <asp:TextBox ID="txtSuburb" runat="server" MaxLength="40" Width="242px" CssClass="input_dr" AutoCompleteType="Disabled" />
                                    <asp:Label Width="150px" ID="Label21" runat="server" ForeColor="red"></asp:Label>
                                </div>
                                <div class="clear"></div>
                                <div class=" form-col-2-8">
                                    <p class="p2">
                                        State
                                <span class="red" style="color: #FF0000;">*</span>
                                    </p>
                                </div>
                                <div class=" form-col-3-8">
                                    <asp:HiddenField ID="hfdrpstate_txt" runat="server" Value=""></asp:HiddenField>
                                       <asp:TextBox ID="drpstate_txt" runat="server" MaxLength="4" Width="242px" CssClass="input_dr"  Enabled="true" ReadOnly="true" />
                                  <%--  <asp:DropDownList Visible="true" ID="drpState" runat="server" Width="250px" Enabled="true">
                                        <asp:ListItem Text="Select Ship To State"></asp:ListItem>
                                        <asp:ListItem Text="ACT"></asp:ListItem>
                                        <asp:ListItem Text="NSW"></asp:ListItem>
                                        <asp:ListItem Text="NT"></asp:ListItem>
                                        <asp:ListItem Text="QLD"></asp:ListItem>
                                        <asp:ListItem Text="SA"></asp:ListItem>
                                        <asp:ListItem Text="TAS"></asp:ListItem>
                                        <asp:ListItem Text="VIC"></asp:ListItem>
                                        <asp:ListItem Text="WA"></asp:ListItem>
                                    </asp:DropDownList>--%>
                                    <asp:Label Width="150px" ID="Label22" runat="server" ForeColor="red"></asp:Label>
                                </div>
                                <div class="clear"></div>
                                <div class=" form-col-2-8">
                                    <p class="p2">
                                        Post Code
                                        <span class="red" style="color: #FF0000;">*</span>
                                    </p>
                                </div>
                                <div class=" form-col-3-8">
                                      <asp:HiddenField ID="hfPostCode" runat="server" Value=""></asp:HiddenField>
                                    <asp:TextBox ID="txtPostCode" runat="server" MaxLength="4" Width="242px" CssClass="input_dr" ReadOnly="true"  />
                                    <asp:Label Width="150px" ID="lblpostcode2err" runat="server" ForeColor="red"></asp:Label>
                                    <asp:FilteredTextBoxExtender ID="fteMobile" runat="server" FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtPostCode" />
                                </div>
                                <div class="clear"></div>
                                <div class=" form-col-2-8">
                                    <p class="p2">Country</p>
                                </div>
                                <div class=" form-col-3-8">
                                    <asp:TextBox ID="txtCountry" runat="server" CssClass="input_dr" Width="242px" Text="Australia" ReadOnly="True" autocomplete="new-password" />
                                    <asp:Label Width="150px" ID="Label24" runat="server" ForeColor="red"></asp:Label>
                                </div>
                                <div class="clear"></div>
                                <div class=" form-col-2-8">
                                    <p class="p2">Delivery Instructions</p>
                                </div>
                                <div class=" form-col-3-8">
                                    <asp:TextBox ID="txtDeliveryInstructions" runat="server" Width="242px" Class="textSkin" CssClass="input_dr" MaxLength="40" autocomplete="new-password"/>
                                    <asp:Label Width="150px" ID="Label25" runat="server" ForeColor="red"></asp:Label>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtShipPhoneNumber" />
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="clear padbot10"></div>
                            <div class="clear"></div>
                            <div class="form-col-8-8">
                                <textarea id="Ta4" cols="34" class="textarea1" readonly="readonly" disabled="disabled" tabindex="-1" runat="server" rows="10" name="Ta4"></textarea>
                            </div>
                            <div class="clear"></div>
                        </div>


                        <div id="PopDiv" class="containership" style="display:none">
                            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" BehaviorID="ShipmentModelPopupExtender" TargetControlID="HyperLink1"
                                PopupControlID="ShipmentPopupPanel" OkControlID="btnOk" BackgroundCssClass="modalBackground">
                            </asp:ModalPopupExtender>
                            <asp:Panel ID="ShipmentPopupPanel" runat="server" CssClass="ModalPopupStyleship">
                                <div class="containership">
                                    <table width="100%" cellspacing="0" cellpadding="0" border="0" style="border-collapse: collapse; text-align: left;">
                                        <tr>
                                            <td>
                                                <p class="TableColumnStyle">
                                                    <b>Mail</b>
                                                    <br />
                                                    Goods will be sent using Mail Service.
                                <br />
                                                    Invoice will be included with Parcel Note.
                                <br />
                                                    Dangerous goods cannot be sent by mail, road service only.
                                                </p>
                                                <p class="TableColumnStyle">
                                                    <b>Courier Service</b>
                                                    <br />
                                                    Goods will be sent using Courier Service if under Gross / cubic weight of 3Kg.
                                <br />
                                                    Parcels over 3Kg will be sent by most economical means, eg Road, E-Parcel Service
                                etc.
                                <br />
                                                    Invoice will be included with Parcel
                                <br />
                                                    Note. Dangerous goods cannot be sent by air, road service only.
                                                </p>
                                                <p class="TableColumnStyle">
                                                    <b>Courier Pick Up</b>
                                                    <br />
                                                    You will need to organise your own courier to pick up parcel from us.
                                <br />
                                                    Please provide details of the courier company who will be
                                <br />
                                                    picking up in the comments box below.
                                <br />
                                                    Goods will be packaged and Invoice included with Parcel
                                                </p>
                                                <p class="TableColumnStyle">
                                                    <b>Counter Pick Up</b>
                                                    <br />
                                                    Your or your company representative will pick up the goods from our trade counter.
                                <br />
                                                    Invoice will supplied at pick up.
                                                </p>
                                                <p class="TableColumnStyle">
                                                    <b>Drop Shipment</b>
                                                    <br />
                                                    We will send goods directly to your customer.
                                <br />
                                                    No invoice will enclosed in the Parcel , a picking slip only will be included.
                                <br />
                                                    Invoice will be sent to you by email.
                                                </p>
                                                <p class="TableColumnStyle">
                                                    <b>SHIPPING COSTS</b>
                                                    <br />
                                                    Please refer to our <a href="Termsandconditions.aspx" style="color: #0099da; text-decoration: blink;"
                                                        onclick="window.open('Termsandconditions.aspx','popup','width=800,height=600,scrollbars=yes,resizable=no,toolbar=no,directories=no,location=no,menubar=no,modal=yes,status=no,left=150,top=25'); return false">Terms and Conditions</a>
                                                </p>
                                            </td>
                                        </tr>
                                    </table>
                                    <div>
                                        <asp:Button ID="btnOk" runat="server" Text="Close" CssClass="ButtonStyleship" />
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                        <div class="quickorder4">
                            <table id="Table3" width="100%" runat="server" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <%--  <td width="2%">
                &nbsp;
            </td>--%>
                                    <td width="100%" align="left">
                                        <table width="100%" runat="server" cellpadding="1" cellspacing="0" border="0" style="border-style: none" id="colo2">
                                            <tr>
                                                <td colspan="2" style="font-size: 16px; color: #0099DA;">
                                                    <%-- <b>Comments / Notes</b>--%>
                                                    <div class=" form-col-8-8">
                                                        <h4 class="padbot10">Comments / Notes</h4>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td rowspan="2" width="75%">
                                                    <div class="form-col-6-8">
                                                        <asp:TextBox ID="TextBox1" runat="server" Rows="5" Columns="30" Font-Size="12px" CssClass="textarea1" Width="535px" Height="72px" Font-Names="arial" MaxLength="240" onkeyDown="return checkMaxLength(this,event,'240');"
                                                            TextMode="MultiLine"  onkeyup="javascript:keyboardup(this);">
                                                        </asp:TextBox>
                                                    </div>
                                                </td>
                                                <td align="right">
                                                    <%--<asp:ImageButton ID="ImageButton5" runat="server"  AlternateText="Edit/Update Order" CssClass="button normalsiz btngray fleft"
                                OnClick="ImageButton5_Click" />--%>
                                                    <%-- <asp:Button ID="ImageButton5" runat="server" Text="Edit/Update Order" CssClass="buttongray normalsiz btngray fleft" OnClick="ImageButton5_Click" style="margin: 0px 0px 0px 10px;" />--%>
                            
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <%-- <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/images/submit_order4.jpg"
                                OnClick="ImageButton4_Click" OnClientClick="return checkorderid()" />--%>
                                                    <%--<asp:Button ID="ImageButton4" runat="server" Text="Submit Order" OnClick="ImageButton4_Click" OnClientClick="return checkorderid()" CssClass="button normalsiz btnblue fleft" style="margin: 0px 0px 0px 10px;" />--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <%-- <td width="2%">
                &nbsp;
            </td>--%>
                                </tr>
                            </table>
                        </div>
            </td>

        </tr>

        <tr>
            <td align="center"></td>
        </tr>
        </tbody>
                
    </table>





    <div class="quickorder4">
        <table id="Table4" width="100%" runat="server" cellpadding="0" cellspacing="0" border="0" style="padding-left: 5px">
            <tr>
                <td>
                    <div class=" form-col-8-8">
                        <h4 class="padbot10" style="font-size: 16px; color: #0099DA;">Your Order Contents</h4>
                        <asp:Label ID="lblppp" runat="server" Text="" Style="float: right; color: grey"></asp:Label>
                    </div>



                </td>
                </tr>
                <tr>
                <td>
              
                 <asp:Label ID="lblweight" runat="server" Visible="true" font-color="gray" Font-Names="Arial" Font-Size="20px" Font-Bold="true" Text=""></asp:Label>
                 <asp:Label ID="lblzone" runat="server" Visible="true"  font-color="gray" Font-Names="Arial" Font-Size="20px"  Font-Bold="true" Text=""></asp:Label>

                   <asp:Label ID="lblzonezip" runat="server" Visible="true" font-color="gray" Font-Names="Arial" Font-Size="20px" Font-Bold="true" Text=""></asp:Label>
                </td>
                </tr>
            
            <tr>
                <td>
                    <asp:Panel ID="PnlOrderContents" Visible="true" runat="server">
                        <%
                            HelperServices objHelperServices = new HelperServices();
                            HelperDB objHelperDB = new HelperDB();
                            OrderServices objOrderServices = new OrderServices();
                            ProductServices objProductServices = new ProductServices();
                            //ProductFamily oProdFam = new ProductFamily();
                            DataSet dsOItem = new DataSet();

                            int OrderID = 0;
                            int Userid;
                            int ProductId;
                            decimal subtot = 0.00M;
                            decimal taxamt = 0.00M;
                            decimal Total = 0.00M;

                            string SelProductId = "";
                            string OrdStatus = "";

                            int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;

                            Userid = objHelperServices.CI(Session["USER_ID"]);

                            if (!string.IsNullOrEmpty(Request["OrderID"]))
                            {
                                OrderID = Convert.ToInt32(Request["OrderID"].ToString());
                            }
                            else
                            {
                                OrderID = objOrderServices.GetOrderID(Userid, OpenOrdStatusID);
                            }

                            //if (Request.Cookies["OrderInfo"] == null)
                            //{
                            //    //Handling cookies for when close IE before submit order

                            //    HttpCookie OrderInformation = new HttpCookie("OrderInfo");
                            //    OrderInformation["LoginName"] = Session["LOGIN_NAME"].ToString();
                            //    OrderInformation["OrderID"] = OrderID.ToString();
                            //    OrderInformation["UserID"] = Userid.ToString();
                            //    OrderInformation.Expires = DateTime.Now.AddDays(3);
                            //    Response.Cookies.Add(OrderInformation);
                            //}

                            OrdStatus = objOrderServices.GetOrderStatus(OrderID);
                            ProductId = objHelperServices.CI(Request.QueryString["Pid"]);
                        %>

                        <table width="100%" class="orderdettable">

                            <tr height="5px">
                                <td bgcolor="#F2F2F2" align="left" style="border-style: none solid solid none; border-width: thin; border-color: #E7E7E7; border: thin solid #E7E7E7; padding: 0px 0 0 9px !important;" width="13%">
                                    <b>Order Code</b>
                                </td>
                                <td style="border-style: none solid solid none; border-width: thin; border-color: #E7E7E7; border: thin solid #E7E7E7; padding: 0px 0 0 9px !important;"
                                    bgcolor="#F2F2F2" align="left" width="10%">
                                    <b>Quantity</b>
                                </td>
                                <td style="border-style: none solid solid none; border-width: thin; border-color: #E7E7E7; border: thin solid #E7E7E7; padding: 0px 0 0 9px !important;"
                                    colspan="2" bgcolor="#F2F2F2" align="left" width="30%">
                                    <b>Description</b>
                                </td>
                                <%-- <td  
                              style="border-style: none solid solid none; border-width: thin; border-color: #E7E7E7" 
                              bgcolor="White" align="center"  width="10%">
                              <b>Availability</b></td>--%>
                                <td style="border-style: none solid solid none; border-width: thin; border-color: #E7E7E7; border: thin solid #E7E7E7; padding: 0px 0 0 9px !important;"
                                    bgcolor="#F2F2F2" align="left" width="20%">
                                    <b>Cost (Ex. GST)</b>
                                </td>
                                <td style="border-style: none none solid none; border-width: thin; border-color: #E7E7E7; border: thin solid #E7E7E7; padding: 0px 0 0 9px !important;"
                                    bgcolor="#F2F2F2" align="left" width="19%">
                                    <b>Extension Amount (Ex. GST)</b>
                                </td>
                            </tr>
                            <%   	  
                                OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                                oOrderInfo = objOrderServices.GetOrder(OrderID);

                                dsOItem = objOrderServices.GetOrderItems(OrderID);
                                string cSymbol = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString();
                                decimal ProdShippCost = 0.00M;
                                decimal TotalShipCost = 0.00M;
                                decimal TotalTaxAmount = 0.00M;
                                SelProductId = "";
                                if (OrdStatus == OrderServices.OrderStatus.OPEN.ToString() || OrdStatus == "CAU_PENDING")
                                {
                                    if (dsOItem != null)
                                    {
                                        int i = 0;
                                        foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                                        {
                                            decimal ProductUnitPrice;
                                            int pid;
                                            int maxqty;
                                            int minQty;
                                            int FId = 0;
                                            double OrderItemId1 = 0;
                                            string sty = "style=\"border-style: none solid none none; border-width: thin; border-color: #E7E7E7\" ";
                                            string styl = "style=\"border-style: none none none none; border-width: thin; border-color: #E7E7E7\" ";
                                            if (rItem["PRODUCT_ID"].ToString() == dsOItem.Tables[0].Rows[dsOItem.Tables[0].Rows.Count - 1]["PRODUCT_ID"].ToString())
                                            {
                                                sty = "style=\"border-style: none solid none none; border-width: thin; border-color: #E7E7E7\" ";
                                                styl = "style=\"border-style: none none none none; border-width: thin; border-color: #E7E7E7\" ";
                                            }
                                            pid = objHelperServices.CI(rItem["PRODUCT_ID"].ToString());
                                            OrderItemId1 = objHelperServices.CD(rItem["ORDER_ITEM_ID"].ToString());
                                            //FId = objHelperServices.CI(rItem["FAMILY_ID"].ToString()); 
                                            FId = objProductServices.GetFamilyID(pid);
                                            int pQty = objOrderServices.GetOrderItemQty(pid, OrderID, OrderItemId1);

                                            maxqty = objHelperServices.CI(rItem["QTY_AVAIL"].ToString());
                                            maxqty = maxqty + objHelperServices.CI(Request.Form["txtQty"] + pQty);
                                            minQty = objHelperServices.CI(rItem["MIN_ORD_QTY"].ToString());
                                            ProductUnitPrice = objHelperServices.CDEC(rItem["PRICE_EXT_APPLIED"].ToString());
                                            ProductUnitPrice = objHelperServices.CDEC(ProductUnitPrice.ToString("N4"));
                                            TotalTaxAmount = TotalTaxAmount + objHelperServices.CDEC(rItem["TAX_AMOUNT"].ToString());
                                            string chkShipping_Method = string.Empty;
                                            DataSet dsCSM = (DataSet)objHelperDB.GetGenericDataDB("", pid.ToString(), "GET_ZIP_SHIPPING_METHOD", HelperDB.ReturnType.RTDataSet);
                                            if (dsCSM != null)
                                            {
                                                if (dsCSM.Tables.Count > 0 && dsCSM.Tables[0].Rows.Count > 0)
                                                {
                                                    chkShipping_Method = dsCSM.Tables[0].Rows[0]["PROD_LEGISTATED_STATE"].ToString();
                                                    if (chkShipping_Method.Trim() == "PPPPPPP")
                                                    {

                                                        hfisppp.Value = "1";
                                                    }
                                                }
                                            }
                                            //ProdShippCost = CalculateShippingCost(OrderID, pid, ProductUnitPrice, pQty);
                                            //TotalShipCost = objHelperServices.CDEC(TotalShipCost + ProdShippCost); 
                                            int Qty = objHelperServices.CI(rItem["QTY"].ToString());
                                            decimal ProdTotal = Math.Round(Qty * ProductUnitPrice, 2, MidpointRounding.AwayFromZero);
                                            //decimal ProdTotal = Qty * ProductUnitPrice;
                                            subtot = subtot + ProdTotal;
                                            string Desc = rItem["DESCRIPTION"].ToString().Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
                                            string Available = rItem["PRODUCT_STATUS"].ToString();
                                            // int FId=oProd.getpr
                                            if (Request["SelAll"] != "1")
                                            {
                                                SelProductId = "";
                                                Session["SelProduct"] = null;
                                                CheckBox chk = new CheckBox();
                                                string rowspan_csm = string.Empty;
                                            //if (chkShipping_Method.Trim() == "PPPPPPP")
                                            //{
                                            //    rowspan_csm = "rowspan=\"2\"";
                                            //}
           
                            %>
                            <tr>
                                <td bgcolor="White" align="left" class="" <%=rowspan_csm%>>
                                    <%Response.Write("<a class=\"toplinkatest\" href =ProductDetails.aspx?&Pid=" + pid + "&fid=" + FId.ToString() + ">" + rItem["CATALOG_ITEM_NO"].ToString() + "</a>");%>
                                </td>
                                <td bgcolor="White" class="Numeric" align="left">
                                    <%Response.Write("<input type =\"Text\" Id=\"txtQtyId" + i + "_" + maxqty + "\" Name =\"txtQty" + i + "\" size=\"5\" style=\"height:15px;width:83px;padding:2px;border:#878787 1px solid\" disabled=\"disabled\" runat =\"server\" onBlur=\"javascript:return Check(" + i + "," + maxqty + "," + minQty + "," + Qty + ");\" value =\"" + Qty + "\">"); %>
                                </td>
                                <td bgcolor="White" colspan="2">
                                    <%Response.Write(Desc);%>
                                </td>
                                <%--   <td <% Response.Write(sty); %>="" 
                                                            bgcolor="White" ="" ><%Response.Write(Available);%></td>--%>
                                <td bgcolor="White" align="left" class="NumericField"
                                    style="width: 130px; text-align: left;">
                                    <%Response.Write(cSymbol + " " + objHelperServices.CheckPriceValueDecimal(ProductUnitPrice.ToString("#,#0.0000")));%>
                                </td>
                                <td bgcolor="White" align="center" class="NumericField" style="text-align: left;">
                                    <%Response.Write(cSymbol + " " + ProdTotal.ToString("#,#0.00")); %>
                                </td>
                                <%Response.Write("<input type=\"hidden\" Name=\"txtPid" + i + "\" runat=\"server\" value=\"" + pid + "\">"); %>
                                <%Response.Write("<input type=\"hidden\" Name=\"txtCatItem" + i + "\" runat=\"server\" value=\"" + rItem["CATALOG_ITEM_NO"].ToString() + "\">"); %>
                                <%Response.Write("<input type=\"hidden\" Name=\"txtMaxQty" + i + "\" runat=\"server\" value=\"" + maxqty + "\">"); %>
                                <%Response.Write("<input type=\"hidden\" Name=\"txtMinQty" + i + "\" runat=\"server\" value=\"" + minQty + "\">"); %>
                                <%Response.Write("<input type=\"hidden\" Name=\"txtUntPrice" + i + "\" runat=\"server\" value=\"" + ProductUnitPrice.ToString("#,#0.00") + "\">"); %>
                                <% Response.Write("<input type=\"hidden\" Name=\"txtPrdTprice" + i + "\" runat=\"server\" value=\"" + ProdTotal.ToString("#,#0.00") + "\">"); %>
                            </tr>
                            <% if (chkShipping_Method.Trim() == "PPPPPPP")
                               { %>
                            <%--<tr>
                                <td colspan="6">

                                    <div style="background: #FFD405; color: black; padding: 4px 2px; font-size: 11px">
                                        <div style="float: left; margin: 1px 7px 0 5px;">
                                            <img style="max-height: 30px;" src="images/Alert_yellow.png" /></div>
                                        <div>
                                            <b><%=rItem["CATALOG_ITEM_NO"].ToString()%> </b>is a non standard shipping item and no shipping is available for this product.<br />
                                            Product can be picked uped in store or an alternative pick up method can be arranged with our sales team.
                                        </div>
                                    </div>

                                </td>
                            </tr>--%>
                            <% } %>

                            <%  
                               i = i + 1;
                                        }
                                        else if (Request["SelAll"] == "0")
                                        {
                                            SelProductId = "";
                                            Session["SelProduct"] = null;
                            %>
                            <tr>
                                <td
                                    bgcolor="White" align="left" class="style20ship">
                                    <%Response.Write("<a href =ProductFeatures.aspx?Fid=" + FId + "&Pid" + pid + "&Min=" + minQty + "&Max" + maxqty + ");>" + rItem["CATALOG_ITEM_NO"].ToString() + "</a>");%>
                                </td>
                                <td
                                    bgcolor="White" class="style21ship" align="left">
                                    <%Response.Write("<input type =\"Text\" Id=\"txtQtyId" + i + "_" + maxqty + "\" Name =\"txtQty" + i + "\" size=\"7\" disabled=\"disabled\" runat =\"server\" onBlur=\"javascript:Check(" + i + "," + maxqty + ");\" value =\"" + Qty + "\">"); %>
                                </td>
                                <td
                                    colspan="2" bgcolor="White" class="style21ship">
                                    <%Response.Write(Desc); %>AN2
                                </td>
                                <%--<td style="border-style: none none none solid; border-width: thin; border-color: #E7E7E7"
                                bgcolor="White" class="style20">
                                <%Response.Write(Available); %>
                            </td>--%>
                                <td
                                    bgcolor="White" class="style22ship" align="left" style="width: 130px; text-align: left;">
                                    <%Response.Write(cSymbol + " " + objHelperServices.CheckPriceValueDecimal(ProductUnitPrice.ToString("#,#0.0000")));%>
                                </td>
                                <%--								                                <td class="NumericField" align="center"><%Response.Write(cSymbol + ProdShippCost.ToString("#,#0.00")); %></td>
                                --%>
                                <td
                                    bgcolor="White" class="style23ship" align="left" width="20%">
                                    <%Response.Write(cSymbol + " " + ProdTotal.ToString("#,#0.00")); %>
                                </td>
                                <%Response.Write("<input type=\"hidden\" Name=\"txtPid" + i + "\" runat=\"server\" value=\"" + pid + "\">"); %>
                                <%Response.Write("<input type=\"hidden\" Name=\"txtCatItem" + i + "\" runat=\"server\" value=\"" + rItem["CATALOG_ITEM_NO"].ToString() + "\">"); %>
                                <%Response.Write("<input type=\"hidden\" Name=\"txtMaxQty" + i + "\" runat=\"server\" value=\"" + maxqty + "\">"); %>
                                <%Response.Write("<input type=\"hidden\" Name=\"txtsPrdId" + i + "\" runat=\"server\" value=\"" + pid + "\">"); %>
                                <%Response.Write("<input type=\"hidden\" Name=\"txtUntPrice" + i + "\" runat=\"server\" value=\"" + ProductUnitPrice.ToString("#,#0.00") + "\">"); %>
                                <%Response.Write("<input type=\"hidden\" Name=\"txtPrdTprice" + i + "\" runat=\"server\" value=\"" + ProdTotal.ToString("#,#0.00") + "\">"); %>
                            </tr>
                            <%
                                  i = i + 1;
                                        }
                                        else
                                        { 
                            %>
                            <tr>
                                <td
                                    bgcolor="White" align="left" class="">
                                    <%Response.Write("<a href =ProductFeatures.aspx?Fid=" + FId + "&Pid=" + pid + "&Min=" + minQty + "&Max=" + maxqty + ");>" + rItem["CATALOG_ITEM_NO"].ToString() + "</a>");%>
                                </td>
                                <td
                                    bgcolor="White" class="Numeric" align="left">
                                    <%Response.Write("<input type =\"Text\" Id=\"txtQtyId" + i + "_" + maxqty + "\" Name =\"txtQty" + i + "\" size=\"7\"  disabled=\"disabled\" runat =\"server\" onBlur=\"javascript:Check(" + i + "," + maxqty + ");\" value =\"" + Qty + "\">"); %>
                                </td>
                                <td
                                    colspan="2" bgcolor="White" class="style21ship">
                                    <%Response.Write(Desc); %>AN1
                                </td>
                                <%-- <td style="border-style: none none none solid; border-width: thin; border-color: #E7E7E7"
                                bgcolor="White" class="style20">
                                <%Response.Write(Available); %>
                            </td>--%>
                                <td
                                    bgcolor="White" class="style23ship" align="left" style="width: 130px; text-align: left;">
                                    <%Response.Write(cSymbol + " " + objHelperServices.CheckPriceValueDecimal(ProductUnitPrice.ToString("#,#0.0000")));%>
                                </td>
                                <%--								                                <td class="NumericField" align="center"><%Response.Write(cSymbol + ProdShippCost.ToString("#,#0.00")); %></td>
                                --%>
                                <td
                                    bgcolor="White" class="NumericField" align="left" style="text-align: left;">
                                    <%Response.Write(cSymbol + " " + ProdTotal.ToString("#,#0.00")); %>
                                </td>
                                <%Response.Write("<input type=\"hidden\" Name=\"txtPid" + i + "\" runat=\"server\" value=\"" + pid + "\">"); %>
                                <%Response.Write("<input type=\"hidden\" Name=\"txtCatItem" + i + "\" runat=\"server\" value=\"" + rItem["CATALOG_ITEM_NO"].ToString() + "\">"); %>
                                <%Response.Write("<input type=\"hidden\" Name=\"txtMaxQty" + i + "\" runat=\"server\" value=\"" + maxqty + "\">"); %>
                                <%Response.Write("<input type=\"hidden\" Name=\"txtsPrdId" + i + "\" runat=\"server\" value=\"" + pid + "\">"); %>
                                <%Response.Write("<input type=\"hidden\" Name=\"txtUntPrice" + i + "\" runat=\"server\" value=\"" + ProductUnitPrice.ToString("#,#0.00") + "\">"); %>
                                <% Response.Write("<input type=\"hidden\" Name=\"txtPrdTprice" + i + "\" runat=\"server\" value=\"" + ProdTotal.ToString("#,#0.00") + "\">"); %>
                            </tr>
                            <%   
                                   SelProductId = SelProductId + "," + pid;
                                   i = i + 1;
                                        } //End of SelAll
                                    } //End of for each.
                                    dsOItem.Dispose();
                                }//End of dataset empty. 
                            } // End Of Order Status Check
                            if (SelProductId != "")
                            {
                                SelProductId = SelProductId.Substring(1, SelProductId.Length - 1);
                                Session["SelProduct"] = SelProductId;
                            }
                            %>
                            <!-- End Up Here-->
                            <%-- <tr>
                            <td height="2px">
                            </td>
                        </tr>--%>
                            <tr>
                                <%if ((objOrderServices.IsNativeCountry(OrderID) == 0))
                                  {
                                %>
                                <td colspan="4" rowspan="5" bgcolor="white" valign="top" align="right">
                                    <%}

                                  else if ((drpSM1.SelectedValue == "Courier") || (drpSM1.SelectedValue == "Mail") || (drpSM1.SelectedValue == "Drop Shipment Order"))
                                  {                                                           
                                    %>
                                    <td colspan="4" rowspan="5" bgcolor="white" valign="top" align="right">

                                        <%} %>
                                        <%  else
                                  { %>
                                        <td colspan="4" rowspan="4" bgcolor="white" valign="top" align="right">
                                            <%} %>
                            
                                        </td>
                                        <%-- <td bgcolor="white"  >
                            </td>--%>
                                        <td class="NumericField" colspan="1" bgcolor="white" align="left" style="text-align: left;">Sub Total
                                        </td>
                                        <td class="NumericField" bgcolor="white" align="left" style="text-align: left;">
                                            <%
                                                //Response.Write(objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString() + " " + objHelperServices.CDEC(objHelperServices.FixDecPlace(subtot)));//.ToString("#,#0.00")
                                                Response.Write(cSymbol + " " + oOrderInfo.ProdTotalPrice);
                                  
                                            %>
                                        </td>
                            </tr>
                            <%--<tr>
								<td class="NumericField" colspan ="2">Tax
								</td>
								<td class="NumericField">
									<%                                        
taxamt = CalculateTaxAmount(subtot);
Session["TaxAmt"] = objHelperServices.CDEC(taxamt);
Response.Write(objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString() + " " + objHelperServices.CDEC(objHelperServices.FixDecPlace(taxamt))); //objHelperServices.FixDecPlace(objHelperServices.CDEC(taxamt)));  					    
							         %>
								</td>
							</tr>--%>
                            <%--<tr>
							    <td class="NumericField" colspan ="4">Total ShippingCost
								</td>
								<td class="NumericField">
								<%
								    Response.Write(objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString() + objHelperServices.CDEC(objHelperServices.FixDecPlace(TotalShipCost)));  					    
								%>
								</td>
							</tr>--%>
                            <tr>
                                <%-- <td bgcolor="white" >
                            </td>--%>

                                <%
                                    //if (oOrdBillInfo1.BillCountry.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oOrdBillInfo1.BillCountry.ToLower()).ToLower() != "au" || oOrdShippInfo1.ShipCountry.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oOrdShippInfo1.ShipCountry.ToLower()).ToLower() != "au") // is other then au                      
                                    if ((objOrderServices.IsNativeCountry(OrderID) == 0))
                                    {
                                %>
                                <tr>

                                    <td class="NumericField" colspan="1" style="height: 21px; text-align: left;" align="left">Shipping Charge
                                        <br />
                                        <span style="font-size: 4"></span>
                                    </td>
                                    <td class="NumericField" style="height: 21px; text-align: left;" align="left">To Be Advised                                
                                    </td>
                                </tr>


                                <%
                            }     
                                                                                       
                                %>
                                <%-- <%
                             else    if(    (ispickuponly_zone(OrderID)==true) && (( drpSM1.SelectedValue == "Courier")   ||   ( drpSM1.SelectedValue == "Mail")))  
                              {                                                           
                                 %>

                             <tr>
                          
                            <td class="NumericField" colspan="1" style="height: 21px;text-align:left;" align="left">
                                Shipping Charge <br />
                                <span style="font-size: 4"></span>
                            </td>
                                 <% if (emailconfimation =="Remote"){ %>
                            <td class="NumericField" style="height: 21px;text-align:left;" align="left">
                                To Be Advised                                
                                          </td>
                               
                             <%
                           }     
                                else   
                                {                                                     
                                 %>
                                     <td align="left" class="NumericField" style="height: 21px;text-align:left;">
                                
                           
                                   <%   Response.Write(cSymbol + " " + objHelperServices.CDEC(lblshipingcost.Text)); %>
                                    </td>
                                  <%
                           }     
                                                                                       
                                 %>

                             <%
                           }     
                                                                                       
                                 %>

                             </tr>--%>
                                <%
                                else if ((drpSM1.SelectedValue == "Courier") || (drpSM1.SelectedValue == "Mail") || (drpSM1.SelectedValue == "Drop Shipment Order"))
                                {                                                           
                                %>
                                <tr>
                                    <td align="left" class="NumericField" colspan="1" style="height: 21px; text-align: left;">Shipping Charge
                                    <br />
                                        <span style="font-size: 4"></span></td>
                                    <td align="left" class="NumericField" style="height: 21px; text-align: left;">
                                  <asp:HiddenField ID="hfshipcode" runat="server" />
                                        <asp:Label ID="lblshipingcost" runat="server" Visible="false" />
                                        <%  if (objHelperServices.CDEC(lblshipingcost.Text) > 0)
                                            {

                                                Response.Write(cSymbol + " " + objHelperServices.FixDecPlace(objHelperServices.CDEC(lblshipingcost.Text)));
                                            }
                                            else
                                            {
                                               
                                                    Response.Write("To Be Advised");
                                             
                                            } %>
                                    </td>
                                </tr>
                                <% } %>

                                <tr>
                                    <td align="left" class="NumericField" colspan="1" style="height: 21px; text-align: left;">Tax Amount(GST)<br />
                                        <span style="font-size: 4"></span></td>
                                    <td align="left" class="NumericField" style="height: 21px; text-align: left;"><%       
                                                                                                                      if (((drpSM1.SelectedValue == "Courier") || (drpSM1.SelectedValue == "Mail") || (drpSM1.SelectedValue ==  "Drop Shipment Order") ) && (objHelperServices.CDEC(lblshipingcost.Text) > 0))
                                                                                                                      {
                                                                                                                          decimal taxamtwithshipping =TotalTaxAmount+ objOrderServices.CalculateTaxAmount( objHelperServices.CDEC(lblshipingcost.Text), OrderID.ToString());

                                                                                                                          Response.Write(cSymbol + " " + objHelperServices.FixDecPlace(taxamtwithshipping));
                                                                                                                      }
                                                                                                                      else
                                                                                                                      {
                                                                                                                          Response.Write(cSymbol + " " + oOrderInfo.TaxAmount);

                                                                                                                      }
                                    %></td>
                                </tr>

                                <tr>
                                    <td align="left" class="NumericFieldship" colspan="1" style="height: 21px; border-style: none solid none none; border-width: thin; border-color: #E7E7E7"><%
                                                                                                                                                                                                  if (objOrderServices.IsNativeCountry(OrderID) == 0)
                                                                                                                                                                                                  {                                        
                                    %><strong>Est. Total </strong>
                                        <br />
                                        <%
                                    }
                                    else
                                    {
                                        %><strong>Est. Total Inc GST</strong><br />
                                        <%
                                    } %>




                                        <%       
                                            if (((drpSM1.SelectedValue == "Courier") || (drpSM1.SelectedValue == "Mail") || (drpSM1.SelectedValue ==  "Drop Shipment Order")) && (ispickuponly_zone(OrderID) == false))
                                            {

                                            }
                                            else
                                            {    %>
                                        <span style="font-size: 4">(Freight not included)</span>

                                        <%  }   %>
                                 
                                    </td>
                                    <td align="left" class="NumericFieldship" style="height: 21px; border-style: none solid none none; border-width: thin; border-color: #E7E7E7"><strong>
                                        <asp:Label ID="lbltotalamount" runat="server" Visible="false" Width="250px" Text="" />
                                        <%--   <%
                                   
                                     Response.Write(cSymbol + " " + oOrderInfo.TotalAmount);
                                        %>--%>
                                        <% if (((drpSM1.SelectedValue == "Courier") || (drpSM1.SelectedValue == "Mail") || (drpSM1.SelectedValue == "Drop Shipment Order")) && (objHelperServices.CDEC(lblshipingcost.Text) > 0))
                                           {
                                               decimal taxamtwithshipping =TotalTaxAmount+ objOrderServices.CalculateTaxAmount( objHelperServices.CDEC(lblshipingcost.Text), OrderID.ToString());
                                               decimal Totalamt = oOrderInfo.ProdTotalPrice + taxamtwithshipping + objHelperServices.CDEC(lblshipingcost.Text);
                                               Response.Write(cSymbol + " " + objHelperServices.FixDecPlace(Totalamt));

                                           }
                                           else
                                           {

                                               Response.Write(cSymbol + " " + oOrderInfo.TotalAmount);
                                           }
                                        %>

                                    </strong></td>
                                </tr>
                                <tr style="display: none;">
                                    <td colspan="6">
                                        <div style="margin: 10px 10px 10px 10px; text-align: right;">
                                            Coupon Code : <span class="redx"></span>
                                            <asp:TextBox ID="txtCouponCode" runat="server" BackColor="White" CssClass="input_dr" MaxLength="20" onkeypress="blockspecialcharacters(event);couponcodekeypress(event);" Width="150px" />
                                            <br />
                                            <asp:Label ID="lblcouponerrmsg" runat="server" ForeColor="red" Visible="false" Width="250px" />
                                        </div>
                                    </td>
                                </tr>
                                <%--  class="form-col-8-8"--%>
                            </tr>
                            <tr runat="server" visible="false" id="trpm">


                                <td colspan="2" style="font-size: 16px; color: #0099DA; border-right: 0px">
                                    <div>
                                        <h4 class="padbot10">Payment Method</h4>
                                    </div>


                                </td>
                                <td colspan="6" style="border-left: 0px">
                                    <div id="divsubmitordertype" runat="server" visible="false" style="text-align: right">
                                        <label for="contactChoice2" class="payment_radio" style="display: inline-block; position: relative; margin-right: 40px; height: 92px" id="Label19">
                                            <asp:RadioButton ID="RBOffpay" runat="server" Checked="false" GroupName="payop" Style="position: absolute; left: -20px; top: 18px;" />

                                            <div runat="server" style="display: none; text-align: left;" id="divofflinemastercard">
                                                <span style="display: block; text-align: left; margin-bottom: 16px">
                                                    <img style="display: inline-block" src="../images/master-card.png" alt="" id="imgofmaster" runat="server" />
                                                    <img style="display: inline-block" src="../images/visa.png" alt="" id="imgofflinevisa" runat="server" visible="false" />
                                                    <img style="display: inline-block" src="../images/amex.png" alt="" id="imgofflineimgamax" runat="server" visible="false" />
                                                </span>
                                                <span style="display: block; text-align: left; margin-left: -20px; margin-bottom: 16px; font-size: 12px; height: 30px; vertical-align: middle;">
                                                    <asp:Label ID="lblofflinemastercard" runat="server" Text="Label" Style="height: 15px; display: block;"></asp:Label>
                                                    <asp:Label ID="lblofflinecarddate" runat="server" Text="Label" Style="height: 15px; display: block;"></asp:Label>
                                                </span>

                                            </div>
                                            <div runat="server" id="divofflineonacc" style="display: none; text-align: left;">
                                                <span style="display: block; text-align: left; margin-bottom: 16px">
                                                    <img style="display: inline-block; margin-bottom: 13px" src="../images/charge-account.png" alt="" />
                                                    <div style="font-size: 12px">
                                                        Charge to Account
                                                          <div style="display: block; visibility: hidden;">Pay On Account</div>

                                                    </div>

                                                </span>
                                            </div>
                                        </label>

                                        <label for="contactChoice2" class="payment_radio" id="Label23" style="height: 92px; margin: 5px 0 13px 8px; text-align: left; display: inline-block; position: relative; font-size: 12px; margin-right: 10px;">
                                            <asp:RadioButton ID="RBOnPay" runat="server" GroupName="payop" Style="position: absolute; left: -20px; top: 18px;" />
                                            <div>
                                                <img src="../images/online_button_1.png" alt="" /></div>
                                            <div style="text-align: left; margin-bottom: 10px">Pay Online via Credit
                                                <br />
                                                Card or Paypal</div>


                                        </label>


                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" style="margin-right: 60px">
                                       <div id="smspopup" class="chechout_notify" runat="server" style="display:none;margin-top: 12px;">
                                        <img src="/images/images.png" />
                                        <%--<p>
                                            SMS Order ready notification message will be sent to:</p>--%>
                                         <asp:Label ID="lblorderreadytext" runat="server" Text="SMS Order ready notification message will be sent to:"></asp:Label>
                                        <asp:Label ID="lblorderready" runat="server" Text="" 
                                            Style="font-weight:bolder;font-size: 14px;vertical-align: middle;color: #666; "></asp:Label>
                                      
                                        <a class="js-open-modal btn" onclick="ShowModal();" style="cursor:pointer"  >Change</a>
                                      
                                        <%-- <asp:LinkButton ID="btnchangemobile" runat="server" OnClientClick="counterPickup()" >Change</asp:LinkButton>--%>
                                    </div>

                                    <asp:Button ID="ImageButton2" runat="server" class="button normalsiz btnblue " OnClick="ImageButton4_Click" OnClientClick="return checkorderid()" Style="margin: 10px 0 10px 15px; float: right" Text="Submit Order" Visible="false" />

                                    <asp:Button ID="ImageButton1" runat="server" class="buttongray normalsiz btngray " OnClick="ImageButton1_Click" Style="margin: 10px 0 10px 15px; float: right" Text="Edit/Update Order" />
                                </td>
                            </tr>

                           
                        </table>
                          <div id="popup1" class="modal-box"  style="left:0;right:0;top:0;bottom:0;margin:auto;height:300px">

 <div class="modal-body">
                       <h2 class="pophead">Get notified by SMS when your </h2>  <h2 class="pophead">Order is Ready for Pick Up</h2>
                       <div class="entermobile">
                           <label>Enter Your Mobile Number:</label>
                           <%--<input type="text" class="pp_input" />--%>
                           <asp:TextBox ID="txtMobileNumber" runat="server" CssClass="pp_input" MaxLength="10"></asp:TextBox>
                           <div class="clearfix"></div>
                            <label id="lblerror_mo_save" style="display:none;color:red">Enter Mobile Number</label>
                           
                        <%--   <asp:RequiredFieldValidator ID="rfMobileNumber" runat="server" Class="vldRequiredSkin"
                                                ValidationGroup="x" Display="Dynamic" ErrorMessage="Enter Mobile Number" ControlToValidate="txtMobileNumber" style="color:red"></asp:RequiredFieldValidator>--%>
                            <asp:RegularExpressionValidator ID="reMobileNumber" runat="server" ControlToValidate="txtMobileNumber"
                                 ValidationExpression="^(04)\d{8}$"
                                Class="vldRegExSkin" Display="Dynamic" ValidationGroup="x" ErrorMessage="Mobile No. must start with 04 and must be 10 digit" style="color:red"></asp:RegularExpressionValidator>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtMobileNumber" />

                       </div>
                       
                       <div class="btns">
                           <asp:CheckBox ID="chksavemobile" runat="server" Checked="true" />
                            <label class="lbl">Save number for future orders</label>
                       <%--    <input type="button" value="Ok, Notify Me" onclick="msg()"> --%>
                           <asp:Button ID="notifymeBtn" runat="server" Text="Ok, Notify Me" CssClass="notifyme" OnClientClick="return savemobilenumer();" ValidationGroup="x"  />
                           <asp:Button ID="noThanksBtn" runat="server" Text="No, Thanks" CssClass="nothanks" OnClientClick="return noThanksBtn();"  />
                       </div>
                   </div>
  
  
</div>
               
             <div id="divchangepopup" class="modal-box-change" style="left:465.5px;top:152.5px;">

 <div class="modal-body"  style="padding: 1.5em 1.8em 1.1em;">
                       <h2 class="pophead">Get notified by SMS when your </h2>  <h2 class="pophead">Order is Ready for Pick Up</h2>
                       <div class="entermobile">
                           <label>Enter New Number:</label>
                           <%--<input type="text" class="pp_input" />--%>
                           <asp:TextBox ID="txtchangemobilenumber" runat="server" CssClass="pp_input" MaxLength="10" ></asp:TextBox>
                           <div class="clearfix"></div>
                            <label id="lblerror_mo" style="display:none;color:red">Enter Mobile Number</label>
                           
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtchangemobilenumber"
                                 ValidationExpression="^(04)\d{8}$"
                                Class="vldRegExSkin" Display="Dynamic" ValidationGroup="y" ErrorMessage="Mobile No. must start with 04 and must be 10 digit" style="color:red"></asp:RegularExpressionValidator>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtchangemobilenumber" />

                       </div>
                       
                       <div class="btns">
                           <asp:CheckBox ID="cbmobilechange" runat="server" Checked="true" />
                            <label class="lbl">Save number for future orders</label>
                           <asp:Button ID="btnMobileNoChange" runat="server" Text="Ok, Notify Me" CssClass="notifyme"  ValidationGroup="y" OnClientClick="return updatemobilenumer();"  />
                           <asp:Button ID="btnNoThanksChange" runat="server" Text="No, Thanks" CssClass="nothanks" OnClientClick="return btnNoThanksChange()" />
                       </div>
                   </div>
  
  
</div>
   <div class="update_popup" id="duppopup" style="display:none">
	<div class="update_popup_head">
		<img src="i_icon.png" >
	</div>
	<div class="update_popup_body">
		<b>Please Note: <br> Order No has been used with a previous order</b>
		
		<a href="#" class="btnPopup btngreen" onclick="EnterNeworderno()">Enter New Order No</a>
		<a href="#" class="btnPopup btnyellow" onclick="ordernoproceed()">Submit with Duplicate No</a>
	</div>
	
</div>
                         <asp:HiddenField ID="hfduporderproceed" Value="0" runat="server" />
                    </asp:Panel>
                    
                    <asp:Panel ID="PnlOrderInvoice" runat="server" Visible="false">
                        <uc1:InvoiceOrder ID="InvoiceOrder2" runat="server" />
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    <div class="quickorder4" id="divpaymentoption" runat="server">
        <table id="ctl00_maincontent_Table3" border="0" cellpadding="0" cellspacing="0" width="100%">
            <tbody>
                <tr>
                    <td align="left" width="100%">
                        <table id="ctl00_maincontent_colo2" style="border-style: none" border="0" cellpadding="1" cellspacing="0" width="100%">
                            <tbody>
                                <tr>
                                    <td colspan="2" style="font-size: 16px; color: #0099DA;">
                                        <div class="form-col-8-8">
                                            <h4 class="padbot10">Payment Options</h4>
                                        </div>


                                    </td>
                                </tr>


                                <tr>
                                    <td rowspan="" width="100%">

                                        <div class="payment_btns">
                                            <div style="width: 70%; display: block; float: left">
                                                <label for="contactChoice1" class="form-col-3-8 payment_radio " id="lbldefaultpayment" runat="server">
                                                    <asp:RadioButton ID="RBpaymenttype" runat="server" onclick="Defaultclick()" Text="Bank Transfer"  Checked="true" GroupName="Paymentoption"  />

                                                    <div runat="server" style="display: none" id="divmastercard">
                                                        <span style="height: 37px; display: inline-block;">
                                                            <img style="display: inline-block" src="../images/master-card.png" alt="" id="imgmc" runat="server" />
                                                            <img style="display: inline-block" src="../images/visa.png" alt="" id="imgvisa" runat="server" visible="false" />
                                                            <img style="display: inline-block" src="../images/amex.png" alt="" id="imgamex" runat="server" visible="false" />
                                                        </span>
                                                        <span style="display: inline-block; height: 30px; vertical-align: middle;">
                                                            <asp:Label ID="lblmastercardno" runat="server" Text="Label" Style="height: 15px; display: block;"></asp:Label>
                                                            <asp:Label ID="lblmasterexpirydate" runat="server" Text="Label" Style="height: 15px; display: block;"></asp:Label>
                                                        </span>

                                                    </div>
                                                    <div runat="server" id="divdirectdeposit">
                                                        <img style="display: inline-block" src="../images/bank-transfer.png" alt="" />

                                                    </div>
                                                    <div runat="server" id="divpayoaccount" style="display: none">
                                                        <img style="display: inline-block" src="../images/charge-account.png" alt="" />

                                                    </div>
                                                </label>


                                                <label for="contactChoice2" class="form-col-3-8 payment_radio" id="lblcreditcard">
                                                    <asp:RadioButton ID="RBCreditCard" runat="server" onclick="creditcardclick()" Text="Credit Card" GroupName="Paymentoption" ValidationGroup="payonlinepaynow" />


                                                    <div>
                                                        <img src="../images/credit_card.png" alt="" /></div>
                                                </label>

                                                <label for="contactChoice3" class="form-col-1-8 payment_radio" id="lblpaypalcard"  style="display:none">
                                                    <asp:RadioButton ID="RBPaypal" runat="server" onclick="paypalclick()" Text="Paypal" GroupName="Paymentoption" />

                                                    <div>
                                                        <img src="../images/paypal_pay.png" alt="" /></div>
                                                </label>
                                            </div>
                                            <div style="width: 30%; display: block; float: left">
                                                <div id="divtotalamt" runat="server" visible="false">
                                                    <p style="margin-top: 2px; margin-bottom: 8px; text-align: right; padding-right: 35px; font-size: 13px;">Total Amount: </p>
                                                    <h2 style="margin-top: 15px; padding-right: 35px; color: #0099DA; font-family: Arial; font-weight: bold; font-size: 26px; text-align: right">$ 
                                     
                                        <asp:Label ID="lbltotalamt" runat="server" Text="" CssClass="LabelStyle"></asp:Label>
                                                    </h2>
                                                </div>
                                            </div>
                                            <div class="clear"></div>


                                            <div runat="server" id="divpayonacc" style="display: none">

                                                <img style="display: inline-block" src="../images/charge-account.png" alt="" />
                                            </div>


                                            <div id="divdedault" style="margin-top: 20px; display: none;" runat="server">


                                                <div class="payment_contents" id="divD1" runat="server" style="padding-bottom: 20px">
                                                    <h2 style="color: #0099DA; padding-right: 22px; font-size: 13px;">Pay By Bank Transfer</h2>
                                                    <p style="font-size: 13px; line-height: 1.5; padding-right: 153px; color: #555">
                                                        We will email you a proforma invoice with bank trasfer details once your order has been submitted.</br></br>
                                      When sending funds please include the Incoice No as your payment reference for faster processing 
                                      and notify our accounts department by email  <a style="color: #0099DA;" href="mailto:accounts@wes.net.au">accounts@wes.net.au </a>
                                                        with your transfer receipt.
                                                    </p>


                                                    <asp:Button runat="server" ID="btndirectdeposit" Text="SUBMIT ORDER" Style="margin: 10px 32px 10px 10px; float: right" class="normalsiz paynow" OnClientClick="return checkorderid()" OnClick="ImageButton4_Click"  />
                                                    <div class="clear"></div>
                                                </div>
                                                <div class="payment_contents" id="divD2" runat="server" style="padding-bottom: 20px">
                                                    <h2 style="color: #0099DA; padding-right: 22px; font-size: 13px;">Charge Account</h2>

                                                    <p>Your Account will be billed for Invoice </p>
                                                    <asp:Button runat="server" ID="btnpayonacc" Text="SUBMIT ORDER" Style="margin: 10px 32px 10px 10px; float: right" class="normalsiz paynow" OnClientClick="return checkorderid()" OnClick="ImageButton4_Click"  />
                                                    <div class="clearfix"></div>
                                                </div>
                                                <div class="payment_contents" id="divD3" runat="server" style="padding-bottom: 20px">
                                                    <asp:Button runat="server" ID="btnmastercard" Text="SUBMIT ORDER" Style="margin: 10px 32px 10px 10px;" class="normalsiz paynow" OnClientClick="return checkorderid()" OnClick="ImageButton4_Click" />
                                                    <div class="clear"></div>
                                                </div>

                                            </div>


                                            <div class="payment_contents" style="display: none; margin-top: 20px" id="divcreditcard" runat="server">
                                                <div class="form-col-6-8">
                                                    <div class="mastercard_pay"></div>


                                                    <div class="creditcard_pay" id="ccfocus">
                                                        <div>
                                                            <label class="pform_title">Credit Card Number</label>
                                                            <asp:TextBox ID="txtcreditcardno" runat="server" class="input_100" MaxLength="19">

                                                            </asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RBcreditcardno" runat="server" class="vldRequiredSkin_creditcard" ErrorMessage="Please Enter Card No"
                                                                ControlToValidate="txtcreditcardno"
                                                                ValidationGroup="payonlinepaynow" InitialValue=""></asp:RequiredFieldValidator>


                                                        </div>

                                                        <div class="form-col-4-8">
                                                            <label class="pform_title">CVV</label>
                                                            <asp:TextBox ID="txtCVV" runat="server" class="input_50" MaxLength="4"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RBCVV" runat="server" class="vldRequiredSkin_creditcard" ErrorMessage="Please Enter CVV" InitialValue=""
                                                                ControlToValidate="txtCVV" ValidationGroup="payonlinepaynow"></asp:RequiredFieldValidator>

                                                        </div>
                                                        <div class="form-col-4-8">

                                                            <label class="pform_title">Expiry Date</label>
                                                            <asp:DropDownList NAME="drpExpmonth" ID="drpExpmonth" runat="server" class="payment_select">
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
                                                            <asp:DropDownList NAME="drpExpyear" ID="drpExpyear" runat="server" class="payment_select">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="RBexpirydate" runat="server"
                                                                class="vldRequiredSkin_creditcard" ErrorMessage="Please Enter Expiry Date"
                                                                ControlToValidate="drpExpyear" ValidationGroup="payonlinepaynow"></asp:RequiredFieldValidator>
                                                        </div>


                                                    </div>
                                                    <div class="clear"></div>

                                                    <div>
                                                        <label class="pform_title">Name On Card</label>
                                                        <asp:TextBox ID="txtnamecard" runat="server" class="input_100" MaxLength="150" ValidationGroup="payonlinepaynow"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RBCardname" runat="server" class="vldRequiredSkin_creditcard" ErrorMessage="Please Enter Name On Card"
                                                            ControlToValidate="txtnamecard" InitialValue="" ValidationGroup="payonlinepaynow"></asp:RequiredFieldValidator>
                                                    </div>

                                                    <div>
                                                        <asp:Button ID="btnSP" runat="server" Text="Pay Now" ValidationGroup="payonlinepaynow" class="paynow" OnClick="btnSecurePay_Click" OnClientClick="return SetinitSP();"  />
                                                         <%--<asp:Button runat="server" ID="BtnProgressSP" Text="Processing Payment. Please Wait�" Style="display: none; visibility: visible; float: left;" class="button normalsiz btngreen fleft"  Enabled="false"  />--%>
                                                        <asp:Image ID="BtnProgressSP" runat="server" style="display:none;float:right;" ImageUrl="../images/Processing_Payment.png" />
                                                    </div>

                                                    <div class="clear"></div>
                                                </div>
                                                <div id="divContent" style="font-size: 12px margin-left:30px; color: Red" runat="server"></div>


                                                <div class="paypal_pay"></div>


                                                <div class="clear"></div>
                                            </div>
                                            <div class="payment_contents form-col-8-8" style="display: none; margin-top: 20px; padding-bottom: 20px" id="divpaypal" runat="server">
                                                <div class="payment_contents creditcard_pay">
                                                    <h2 style="color: #0099DA; padding-right: 22px; font-size: 13px;">Pay with Paypal</h2>

                                                    <p style="margin-top: 8px; margin-bottom: 40px; font-size: 12px; line-height: 2">
                                                        Pay using your PayPal Account </br>
                  You will be redirected to PayPal site to complete your purchase.
                                                    </p>
                                                    <div class="col-sm-20 nolftpadd">
                                                        <div class="form-group col-lg-8 nolftpadd">
                                                            <h3 class="green_clr">
                                                                <asp:Label runat="server" ID="lblpaypaltotamt" CssClass="totalamt" Visible="false" />
                                                            </h3>
                                                        </div>
                                                    </div>

                                                    <asp:Button runat="server" ID="btnPay"   Text="Pay Now" class="paynow"  OnClick="btnPay_Click" OnClientClick="return checkorderid()" Style="margin-right: 28px"  />
                                                    <asp:HiddenField ID="isordersubmited" Value="false" runat="server" />
                                                    <asp:Button runat="server" ID="btnPayApi" Text="Pay Now" Style="width: 100px;" class="paynow" Visible="false" OnClick="btnPayApi_Click" OnClientClick="Setinit(this.id)" />



                                                    <asp:Button runat="server" ID="BtnProgress" Text="Processing Payment. Please Wait�" Style="display: none; visibility: visible; float: left;" class="btn btn-primary " Enabled="false" />
                                                    <%--                <input id="btnpaypal" value="Pay Now" class="paynow" type="button" runat="server" />--%>
                                                    <div class="clear"></div>
                                                </div>

                                            </div>
                                            <div class="clear"></div>
                                            <div runat="server" id="div2" class="accordion_head_yellow gray_40" style="font-size: 12px; text-align: center; font-weight: bold; margin-bottom: 12px; background: #FFF200; padding: 12px 17px;" visible="false">
                                            </div>
                                        </div>
                                    </td>



                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>







    <div class="quickorder4" id="divInternationalpayoption" runat="server" style="display: none">
        <table id="Table1" border="0" cellpadding="0" cellspacing="0" width="100%">
            <tbody>
                <tr>
                    <td align="left" width="100%">
                        <table id="Table2" style="border-style: none" border="0" cellpadding="1" cellspacing="0" width="100%">
                            <tbody>
                                <tr>
                                    <td colspan="2" style="font-size: 16px; color: #0099DA;">
                                        <div class="form-col-8-8">
                                            <h4 class="padbot10">Payment Options</h4>
                                        </div>


                                    </td>
                                </tr>


                                <tr>
                                    <td rowspan="" width="100%">

                                        <div class=" payment_btns">

                                            <label for="contactChoice1" class="form-col-2-8 payment_radio " id="Label9">
                                                <asp:RadioButton ID="rbinternationaldefault" runat="server" onclick="Defaultclick_int()" Text="Bank Transfer" GroupName="Paymentoption" />



                                                <div>
                                                    <img style="display: inline-block" src="../images/bank-transfer.png" alt="" />

                                                </div>

                                            </label>
                                           
                                            <label for="contactChoice2" class="form-col-2-8 payment_radio" id="lblintsp" runat="server">
                                                <asp:RadioButton ID="rbremotezone_sp" runat="server" onclick="creditcardclick_remote()" Text="Credit Card" GroupName="Paymentoption" ValidationGroup="payonlinepaynow" />


                                                <div>
                                                    <img src="../images/credit_card.png" alt="" /></div>
                                            </label>
                                         
                                            <label for="contactChoice3" class="form-col-1-8 payment_radio" id="lblintpp" runat="server">
                                                <asp:RadioButton ID="rbinternationalpaypal" runat="server" onclick="paypalclick_int()" Text="" GroupName="Paymentoption" />
                                                <div>
                                                    <img src="../images/paypal_pay.png" alt="" />
                                                </div>
                                            </label>

                                            <div class="clear"></div>
                                        </div>


<%--
                                        <div id="divintdirdep" style="margin-top: 20px" runat="server">


                                            <div class="payment_contents" id="div8" runat="server" style="padding-bottom: 20px">
                                                <h2 style="color: #0099DA; padding-right: 22px; font-size: 13px;">Pay By Bank Transfer</h2>
                                                <p style="font-size: 13px; line-height: 1.5; padding-right: 153px; color: #555">
                                                    We will email you a proforma invoice with bank trasfer details once your order has been submitted.<br />
                                                    <br />
                                                    When sending funds please include the Incoice No as your payment reference for faster processing
                                      and notify our accounts department by email  <a style="color: #0099DA;" href="mailto:accounts@wes.net.au">accounts@wes.net.au </a>
                                                    with your transfer receipt.
                                                </p>
                                                <%
                                                    OrderServices objOrderServices = new OrderServices();
                                                    OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                                                    oOrderInfo = objOrderServices.GetOrder(OrderID);

                                                    if ((oOrderInfo.TotalAmount < 1000) && (objOrderServices.IsNativeCountry(OrderID) == 0))
                                                    {   %>
                                                <div class="alert shippingbox">

                                                    <p class="p2">
                                                        Note. For orders under AUD $1000 an additional bank fee of AUD $28.00 will be added
                     
                  
                                                    </p>
                                                </div>

                                                <% }%>

                                                <asp:Button runat="server" ID="btndristdepositsuborder" Text="SUBMIT ORDER" Style="margin: 10px 32px 10px 10px; float: right" class="normalsiz paynow" OnClientClick="return checkorderid()" OnClick="ImageButton4_Click"  />
                                                <div class="clear"></div>
                                            </div>



                                        </div>
--%>
                                        <div class="payment_contents form-col-8-8" style="display: none; margin-top: 20px; padding-bottom: 20px" id="divremotesecurepay" runat="server">


                                            <asp:Button runat="server" ID="btnremotesecurepay" Text="SUBMIT ORDER" class="paynow" OnClick="ImageButton4_Click" OnClientClick="return checkorderid()"  Style="margin-right: 28px" />


                                        </div>

                                        <div class="payment_contents form-col-8-8" style="display: block; margin-top: 20px; padding-bottom: 20px" id="divinternationalpaypal" runat="server">
                                            <div class="payment_contents creditcard_pay">
                                                <h2 style="color: #0099DA; padding-right: 22px; font-size: 13px;">Pay with Paypal</h2>

                                                <p style="margin-top: 8px; margin-bottom: 40px; font-size: 12px; line-height: 2">
                                                    Pay using your PayPal Account
                                                    <br />
                                                    You will be redirected to PayPal site to complete your purchase.
                                                </p>
                                                <div class="col-sm-20 nolftpadd">
                                                    <div class="form-group col-lg-8 nolftpadd">
                                                        <h3 class="green_clr">
                                                            <asp:Label runat="server" ID="Label30" CssClass="totalamt" Visible="false" />
                                                        </h3>
                                                    </div>
                                                </div>
                                                <%--<%
                                             
                            OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                            oOrderInfo = objOrderServices.GetOrder(OrderID);
                          
                                    if ( (oOrderInfo.TotalAmount<1000) && (objOrderServices.IsNativeCountry(OrderID) == 0)) 
                                    {   %>                                    
                          <div class="alert shippingbox">
                
                       <p class="p2">
                      Note. For orders under AUD $1000 an additional bank fee of AUD $28.00 will be added
                     
                  
                      </p>
                      </div>
                             
                                 <% }%>--%>
                                                <asp:Button runat="server" ID="btnpaypalsubmitorder" Text="SUBMIT ORDER" class="paynow" OnClick="ImageButton4_Click" OnClientClick="return checkorderid()"  Style="margin-right: 28px" />



                                                <div class="clear"></div>
                                            </div>

                                        </div>
                                        <div class="clear"></div>
                                        <div runat="server" id="div14" class="accordion_head_yellow gray_40" style="font-size: 12px; text-align: center; font-weight: bold; margin-bottom: 12px; background: #FFF200; padding: 12px 17px;" visible="false">
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
                                 
    
       

    <%
        if (1 == 2)
        { %>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtbillFName" runat="server"
        MaxLength="50" Class="textSkin" Width="225px"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtbillLName" runat="server"
        MaxLength="50" Class="textSkin" Width="225px"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtbillMName" runat="server"
        MaxLength="50" Class="textSkin" Width="225px"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtbilladd1" runat="server" MaxLength="50"
        Class="textSkin" Width="225px"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtbilladd2" runat="server" MaxLength="50"
        Class="textSkin" Width="225px"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtbilladd3" runat="server" MaxLength="50"
        Class="textSkin" Width="225px"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtbillcity" runat="server" MaxLength="50"
        Class="textSkin" Width="225px"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="drpBillState" runat="server"
        MaxLength="50" Class="textSkin" Width="225px"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtbillzip" runat="server" MaxLength="20"
        Class="textSkin" Width="225px"></asp:TextBox>
    <asp:DropDownList Visible="false" ID="drpBillCountry" runat="server" Width="230px"
        AutoPostBack="true" Class="DropdownlistSkin">
    </asp:DropDownList>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtbillphone" runat="server"
        MaxLength="50" Class="textSkin" Width="225px"></asp:TextBox>
    <asp:DropDownList Visible="false" ID="cmbProvider" runat="server" Width="150px">
         <asp:ListItem Text="Please Select"></asp:ListItem>
        <asp:ListItem Text="UPS"></asp:ListItem>
        <asp:ListItem Text="DHL"></asp:ListItem>
        <asp:ListItem Text="FedEX"></asp:ListItem>
        <asp:ListItem Text="USPS"></asp:ListItem>
    </asp:DropDownList>
    <asp:DropDownList Visible="false" ID="cmbShipMethod" runat="server" Width="150px">
        <asp:ListItem Text="Ground" Value="Ground"></asp:ListItem>
        <asp:ListItem Text="SecondDay" Value="SecondDay"></asp:ListItem>
        <asp:ListItem Text="Overnight" Value="Overnight"></asp:ListItem>
    </asp:DropDownList>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtSFName" Class="textSkin"
        Width="225px" runat="server" MaxLength="50"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtSMName" Class="textSkin"
        Width="225px" runat="server" MaxLength="50"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtSLName" runat="server" MaxLength="50"
        Class="textSkin" Width="225px"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtSAdd1" runat="server" MaxLength="50"
        Class="textSkin" Width="225px"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtSAdd2" runat="server" MaxLength="50"
        Class="textSkin" Width="225px"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtSAdd3" runat="server" MaxLength="50"
        Class="textSkin" Width="225px"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtSCity" runat="server" MaxLength="50"
        Class="textSkin" Width="225px"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="drpShipState" runat="server"
        MaxLength="50" Class="textSkin" Width="225px"></asp:TextBox>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtSZip" runat="server" MaxLength="20"
        Class="textSkin" Width="225px" OnTextChanged="txtSZip_TextChanged"></asp:TextBox>
    <asp:DropDownList Visible="false" ID="drpShipCountry" runat="server" Width="230px"
        AutoPostBack="true" Class="DropdownlistSkin">
    </asp:DropDownList>
    <asp:TextBox Visible="false" autocomplete="off" ID="txtSPhone" runat="server" MaxLength="50"
        Class="textSkin" Width="225px"></asp:TextBox>
    &nbsp;<table align="center" width="558">
        <!--Site Map-->
        <tr class="tablerow">
            <td class="StaticText" align="left">
                <b>
                    <asp:Label ID="lblCheck" runat="Server" meta:resourcekey="lblCheck"></asp:Label></b>
                <asp:Label ID="lblShoppingCart" runat="Server" meta:resourcekey="lblShoppingCart"></asp:Label>
                > <b>
                    <asp:Label ID="lblShip" runat="Server" meta:resourcekey="lblShip" ForeColor="Blue"></asp:Label></b>
                >
                <asp:Label ID="lblBill" runat="Server" meta:resourcekey="lblBill"></asp:Label>
                >
                <asp:Label ID="lblReviewOrder" runat="Server" meta:resourcekey="lblReviewOrder"></asp:Label>
                >
                <asp:Label ID="lblConfirm" runat="Server" meta:resourcekey="lblConfirm"></asp:Label>
            </td>
        </tr>
        <tr runat="server" id="tbNoItems">
            <td style="height: 21px">
                <asp:LinkButton ID="ShippingLink" runat="server" Class="ErrorLinkSkin" meta:resourcekey="lbllinkcart"
                    ForeColor="Blue" PostBackUrl="~/OrderDetails.aspx" />
            </td>
        </tr>
        <!--Shipping Details-->
        <tr>
            <td align="center">&nbsp;<asp:Label ID="LblStar" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"
                Width="1px"></asp:Label>&nbsp;
                <asp:Label ID="lblRequired" runat="server" meta:resourcekey="lblRequired" Class="lblNormalSkin"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left">
                <!--- Billing Informations-->
                <table id="tblBasebill" width="560" class="BaseTblBorder" align="left" border="0"
                    cellpadding="3" cellspacing="0">
                    <tr>
                        <td colspan="100%" background="images/17.gif" class="TableRowHead">
                            <asp:Label ID="BillingHeader" runat="Server" meta:resourcekey="lblBillingDetails"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:CheckBox ID="ChkbillingAdd" runat="server" meta:resourcekey="ChkbillingAdd"
                                Class="CheckBoxSkin" AutoPostBack="True" Checked="True" OnCheckedChanged="ChkbillingAdd_CheckedChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                        </td>
                        <td style="width: 144px;">
                            <asp:Label ID="lblBillFName" runat="server" meta:resourcekey="lblBillFName" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="ReqFName" ControlToValidate="txtbillFName" Class="vldRequiredSkin"
                                runat="server" meta:resourcekey="rfvFName" ValidationGroup="ShipBillGroup"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegFname" runat="server" ControlToValidate="txtbillFName"
                                meta:resourcekey="rgExname" ValidationExpression="[a-zA-z]+([ '-][a-zA-Z]+)*"
                                Class="vldRegExSkin" Display="static" ValidationGroup="Mandatory"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10px">
                            <asp:Label ID="Label5" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                        </td>
                        <td style="width: 144px">
                            <asp:Label ID="lblBillLName" runat="server" meta:resourcekey="lblBillLName" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" ControlToValidate="txtbillLName"
                                Class="vldRequiredSkin" runat="server" meta:resourcekey="rfvLName" ValidationGroup="ShipBillGroup"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegLName" runat="server" ControlToValidate="txtbillLName"
                                meta:resourcekey="rgExname" ValidationExpression="[a-zA-z]+([ '-][a-zA-Z]+)*"
                                Class="vldRegExSkin" Display="static" ValidationGroup="Mandatory"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10px">
                            <asp:Label ID="Label2" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                        </td>
                        <td style="width: 144px">
                            <asp:Label ID="BillAdd1" runat="server" meta:resourcekey="lblSAdd" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ControlToValidate="txtbilladd1"
                                Class="vldRequiredSkin" runat="server" meta:resourcekey="rfvAdd1" ValidationGroup="ShipBillGroup"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10px"></td>
                        <td style="width: 144px"></td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10px"></td>
                        <td style="width: 144px"></td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10px">
                            <asp:Label ID="Label4" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                        </td>
                        <td style="width: 144px">
                            <asp:Label ID="BillCity" runat="Server" meta:resourcekey="lblCity" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ControlToValidate="txtbillcity"
                                Class="vldRequiredSkin" runat="server" meta:resourcekey="rfvCity" ValidationGroup="ShipBillGroup"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10px">
                            <asp:Label ID="Label6" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                        </td>
                        <td style="width: 144px">
                            <asp:Label ID="BillState" runat="Server" meta:resourcekey="lblState" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10px">
                            <asp:Label ID="Label15" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                        </td>
                        <td style="width: 144px">
                            <asp:Label ID="BillZip" runat="Server" meta:resourcekey="lblZip" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" ControlToValidate="txtbillzip"
                                Class="vldRequiredSkin" runat="server" meta:resourcekey="rfvZip" ValidationGroup="ShipBillGroup"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10px">
                            <asp:Label ID="Label10" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                        </td>
                        <td style="width: 144px">
                            <asp:Label ID="BillCountry" runat="Server" meta:resourcekey="lblCountry" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10px">
                            <asp:Label ID="Label18" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                        </td>
                        <td style="width: 144px">
                            <asp:Label ID="BillPhone" runat="Server" meta:resourcekey="lblPhone" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" ControlToValidate="txtbillphone"
                                Class="vldRequiredSkin" runat="server" meta:resourcekey="rfvPhone" ValidationGroup="ShipBillGroup"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:CheckBox ID="ChkBillDefaultaddr" runat="server" Class="CheckBoxSkin" meta:resourcekey="ChkBillingDefaultAddr"
                                AutoPostBack="True" OnCheckedChanged="ChkDefaultBillAdd_CheckedChanged" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="left">
                <table id="tblBase" width="560" class="BaseTblBorder" border="0px" cellpadding="3"
                    cellspacing="0">
                    <tr>
                        <td class="TableRowHead" background="images/17.gif">
                            <asp:Label ID="lblShippingDetails" runat="Server" meta:resourcekey="lblShippingDetails"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table id="TblInner" border="0" cellpadding="3" cellspacing="0" width="100%">
                                <tr>
                                    <td colspan="3">
                                        <asp:CheckBox ID="ChkShippingAdd" runat="server" Class="CheckBoxSkin" meta:resourcekey="ChkShippingAdd"
                                            AutoPostBack="True" Checked="True" OnCheckedChanged="ChkShippingAdd_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px"></td>
                                    <td style="width: 135px">
                                        <asp:Label ID="lblProvider" runat="server" meta:resourcekey="lblProvider" Class="lblNormalSkin"></asp:Label>
                                    </td>
                                    <td style="width: 376px">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px"></td>
                                    <td style="width: 135px">
                                        <asp:Label ID="lblMethod" runat="server" meta:resourcekey="lblMethod" Class="lblNormalSkin"></asp:Label>
                                    </td>
                                    <td style="width: 376px">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px">
                                        <asp:Label ID="Label20" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                                    </td>
                                    <td style="width: 135px">
                                        <asp:Label ID="lblSFName" runat="server" meta:resourcekey="lblSFName" Class="lblNormalSkin"></asp:Label>
                                    </td>
                                    <td style="width: 376px">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtSFName"
                                            Class="vldRequiredSkin" runat="server" meta:resourcekey="rfvFName" ValidationGroup="ShipBillGroup"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px">
                                        <asp:Label ID="Label7" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                                    </td>
                                    <td style="width: 135px">
                                        <asp:Label ID="lblSLName" runat="server" meta:resourcekey="lblSLName" Class="lblNormalSkin"></asp:Label>
                                    </td>
                                    <td style="width: 376px">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtSLName"
                                            Class="vldRequiredSkin" runat="server" meta:resourcekey="rfvLastName" ValidationGroup="ShipBillGroup"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px">
                                        <asp:Label ID="Label8" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                                    </td>
                                    <td style="width: 135px">
                                        <asp:Label ID="lblSAdd" runat="server" meta:resourcekey="lblSAdd" Class="lblNormalSkin"></asp:Label>
                                    </td>
                                    <td style="width: 376px">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtSAdd1"
                                            Class="vldRequiredSkin" runat="server" meta:resourcekey="rfvAdd1" ValidationGroup="ShipBillGroup"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px"></td>
                                    <td style="width: 135px"></td>
                                    <td style="width: 376px">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px"></td>
                                    <td style="width: 135px"></td>
                                    <td style="width: 376px">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px">
                                        <asp:Label ID="Label11" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                                    </td>
                                    <td style="width: 135px">
                                        <asp:Label ID="lblCity" runat="Server" meta:resourcekey="lblCity" Class="lblNormalSkin"></asp:Label>
                                    </td>
                                    <td style="width: 376px">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txtSCity"
                                            Class="vldRequiredSkin" runat="server" meta:resourcekey="rfvCity" ValidationGroup="ShipBillGroup"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px">
                                        <asp:Label ID="Label12" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                                    </td>
                                    <td style="width: 135px">
                                        <asp:Label ID="lblState" runat="Server" meta:resourcekey="lblState" Class="lblNormalSkin"></asp:Label>
                                    </td>
                                    <td style="width: 376px">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px">
                                        <asp:Label ID="Label14" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                                    </td>
                                    <td style="width: 135px">
                                        <asp:Label ID="lblZip" runat="Server" meta:resourcekey="lblZip" Class="lblNormalSkin"></asp:Label>
                                    </td>
                                    <td style="width: 376px">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ControlToValidate="txtSZip"
                                            Class="vldRequiredSkin" runat="server" meta:resourcekey="rfvZip" ValidationGroup="ShipBillGroup"></asp:RequiredFieldValidator>
                                        <asp:Label ID="lblFDMsg" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px">
                                        <asp:Label ID="Label16" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                                    </td>
                                    <td style="width: 135px">
                                        <asp:Label ID="lblCountry" runat="Server" meta:resourcekey="lblCountry" Class="lblNormalSkin"></asp:Label>
                                    </td>
                                    <td style="width: 376px">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px">
                                        <asp:Label ID="Label1" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                                    </td>
                                    <td style="width: 135px">
                                        <asp:Label ID="lblPhone" runat="Server" meta:resourcekey="lblPhone" Class="lblNormalSkin"></asp:Label>
                                    </td>
                                    <td style="width: 376px">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ControlToValidate="txtSPhone"
                                            Class="vldRequiredSkin" runat="server" meta:resourcekey="rfvPhone" ValidationGroup="ShipBillGroup"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:CheckBox ID="ChkShipDefaultaddr" runat="server" Class="CheckBoxSkin" meta:resourcekey="ChkShippingDefaultAddr"
                                            AutoPostBack="True" OnCheckedChanged="ChkDefaultShipAdd_CheckedChanged" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <!--Shipping Details end here-->
        <!--Proceded to Next Page-->
        <tr>
            <td align="right">
                <asp:Button ID="btnShipProceed" Class="btnNormalSkin" runat="server" meta:resourcekey="btnShipProceed"
                    OnClick="btnShipProceed_Click" ValidationGroup="ShipBillGroup" />&nbsp;<table border="0">
                        <tr>
                            <td class="tablerow" align="right" style="height: 26px">&nbsp;
                            </td>
                        </tr>
                    </table>
            </td>
        </tr>
        <!-- Proceed to NExt Page end up here-->
    </table>
    <% } %>

    <asp:Button ID="btnHiddenTestPopupExtender" runat="server" Style="display: none; visibility: hidden"></asp:Button>
    <div id="PopupOrderMsg" align="center" runat="server">
        <asp:Panel ID="ModalPanel" runat="server" CssClass="PopUpDisplayStyleship">
            <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;"
                align="center">
                <tr style="height: 5px">
                    <td colspan="3">&nbsp;
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td width="100%" align="center" colspan="3">&nbsp;
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td width="100%" align="center" colspan="3" class="TextContentStyleship">Account Activation Required before you can proceed to check out.
                        <br />
                        Please check your email for account activation link as this was emailed to you when you registered your account.
                        <br />
                        If you would like us to send you the Activation Email again. <a href="ConfirmMessage.aspx?Result=REMAILACTIVATION" class="toplinkatest">Please Click Here</a>
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td width="100%" align="center" colspan="3">&nbsp;
                    </td>
                </tr>

                <tr style="height: 10px">
                    <td width="35%" align="right">
                        <%-- <asp:Button ID="ForgotPassword" runat="server" Text="Close"
                            Width="205px"  CssClass="ButtonStyle" OnClick="btnForgotPassword_Click" />--%>
                    </td>
                    <td width="30%">
                        <asp:Button ID="ForgotPassword" runat="server" Text="Close"
                            Width="205px" CssClass="button normalsiz btnblue" OnClick="btnForgotPassword_Click" />
                    </td>
                    <td width="35%" align="left">
                        <%--<asp:Button ID="Close" runat="server" Text="Close" Width="165px"
                            CssClass="ButtonStyle" OnClick="btnClose_Click" />--%>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <div id="ResetPassAlertMsg" align="center" runat="server">
        <asp:Panel ID="pnlResetPassAlert" runat="server" CssClass="PopUpDisplayStyleship">
            <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;"
                align="center">
                <tr style="height: 5px">
                    <td colspan="3">&nbsp;
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td width="100%" align="center" colspan="3">&nbsp;
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td width="100%" align="center" colspan="3" class="TextContentStyleship">Your password was reseted,Change password action required before you can proceed to check out.
                        <br />
                        Please check your email for reseted password as this was emailed to you.
                        <br />
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td width="100%" align="center" colspan="3">&nbsp;
                    </td>
                </tr>

                <tr style="height: 10px">
                    <td width="35%" align="right">
                        <%-- <asp:Button ID="ForgotPassword" runat="server" Text="Close"
                            Width="205px"  CssClass="ButtonStyle" OnClick="btnForgotPassword_Click" />--%>
                    </td>
                    <td width="30%">
                        <asp:Button ID="btnGoHome" runat="server" Text="Close"
                            Width="205px" CssClass="button normalsiz btnblue" OnClick="btnGoHome_Click" PostBackUrl="~/Shipping.aspx?RPWD=true" />
                    </td>
                    <td width="35%" align="left">
                        <%--<asp:Button ID="Close" runat="server" Text="Close" Width="165px"
                            CssClass="ButtonStyle" OnClick="btnClose_Click" />--%>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    
    <asp:HiddenField ID="hfisppp" Value="0" runat="server" />
    <asp:HiddenField ID="hfispopup" Value="0" runat="server" />
    <asp:HiddenField ID="hfshowmailpopup" Value="0" runat="server" />
    </table>
     
   
 
   
   
   
    
   
   
   
 
   
   
   
   
   
   
   
 
   
   
   
    </div>
   
   
   
 
   
   
   
    
   
   
   
 
   
   
   
   
   
   
   
 
   
   
   
    </div>
   
   
   

                      </p>
   
   
   
    
   
   
   
 
   
   
   
   
   
   
   
 
   
   
</asp:Content>
