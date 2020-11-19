<%@ Page Language="C#" MasterPageFile="~/mainpage.master" AutoEventWireup="true" Inherits="shipping" EnableEventValidation="false"
    Title="Untitled Page"  Culture="en-US" UICulture="en-US" Codebehind="shipping.aspx.cs" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="UI/InvoiceOrder.ascx" TagName="InvoiceOrder" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">        window.history.forward(1);

        window.location.hash = "no-back-button";
        window.location.hash = "Again-No-back-button"; //again because google chrome don't insert first hash into history
        window.onhashchange = function () { window.location.hash = "no-back-button"; }                                                    
     </script>
     
   <%-- Comments / Notes--%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" runat="Server">
    <!--Add JQuery library reference-->
    <script src="Scripts/jquery-1.5.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        function creditcardclick()
        {
            document.getElementById("divdedault").style.display = 'none';
            document.getElementById("divcreditcard").style.display = 'block';
            document.getElementById("divpaypal").style.display = 'none';
          document.getElementById("ctl00_maincontent_ImageButton2").style.display = 'none';
          
         
            $("#lblcreditcard").addClass("active");
            $("#lbldefaultpayment").removeClass("active");
            $("#lblpaypalcard").removeClass("active");

        }
        function paypalclick() {
            document.getElementById("divdedault").style.display = 'none';
            document.getElementById("divcreditcard").style.display = 'none';
            document.getElementById("divpaypal").style.display = 'block';
            document.getElementById("ctl00_maincontent_ImageButton2").style.display = 'none';
         
         
            $("#lblcreditcard").removeClass("active");
            $("#lbldefaultpayment").removeClass("active");
            $("#lblpaypalcard").addClass("active");
      
        }

        function Defaultclick() {
            document.getElementById("divdedault").style.display = 'block';
            document.getElementById("divcreditcard").style.display = 'none';
            document.getElementById("divpaypal").style.display = 'none';
            document.getElementById("ctl00_maincontent_ImageButton2").style.display = 'none';
         
         
            $("#creditcardclick").removeClass("active");
       
            $("#paypalclick").removeClass("active");
        }

        function Defaultclick_international() {
            document.getElementById("rbinternationaldefault").style.display = 'block';
            document.getElementById("rbinternationalpaypal").style.display = 'none';

      
        }

        function paypalclick_international() {
            document.getElementById("rbinternationaldefault").style.display = 'none';
            document.getElementById("rbinternationalpaypal").style.display = 'block';
         

        }
        function Bgcolorchange() {
            alert("hi");
        }
        function isAlphabetic() {
            
            var ValidChars = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890/\\';
            var sText = document.getElementById("ctl00_maincontent_tt1").value;
            var IsAlphabetic = true;
            var Char;
            var ErrMsg = 'Sorry, but the use of the following special characters is not allowed in the Order No field:' + '\n' + '! ` & ~ ^ * %  $ @ ’ ( “ ) ; [   ] { } ! = < > | * , . -' + '\n' + 'Please update your order no so it longer has any of these restricted characters in it to continue order.';
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

        function checkorderid() {
            var msgCheck = "**** NOTE ****" + '\n' + "Courier Pick Up Service needs to be arranged by you." + '\n' + "Please enter the details of Courier Company that you will be arranging to pick up your parcel from us with.";
//            if (document.forms[0].elements["<%=tt1.ClientID%>"].value.length == 0) {
//                alert('Enter Order No and then proceed');
//                document.forms[0].elements["<%=tt1.ClientID%>"].focus();
//                return false;
//            }
//            else 
            if (document.forms[0].elements["<%=drpSM1.ClientID%>"].value == "Please Select Shipping Method") {
                alert('Please Select Shipping Method');
                document.forms[0].elements["<%=drpSM1.ClientID%>"].focus();
                return false;
            }
            else if ((document.forms[0].elements["<%=TextBox1.ClientID%>"].value == msgCheck || document.forms[0].elements["<%=TextBox1.ClientID%>"].value == '' || document.forms[0].elements["<%=TextBox1.ClientID%>"].value == null) && document.forms[0].elements["<%=drpSM1.ClientID%>"].value == "Courier Pickup") {
                //alert('Please Enter Comments and Submit Order');
                ShowCourierMessage();
                document.forms[0].elements["<%=TextBox1.ClientID%>"].focus();
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
                            alert("Blank space in order no");
                        }
                        else
                            alert("Please enter valid order no, Order no should have alpha-numeric character.");
                        return false;
                    }
                }

                return (ValidationDropShipOrder()); ;
            }

            return (ValidationDropShipOrder());
        }

        $(document).ready(function () {
            if ($find("ShipmentModelPopupExtender").hide() != null) {
                $find("ShipmentModelPopupExtender").hide();
            }
            var msgShipment = "**** NOTE ****" + '\n' + "Courier Pick Up Service needs to be arranged by you." + '\n' + "Please enter the details of Courier Company that you will be" + '\n' + "arranging to pick up your parcel from us with.";
            var msgOthers = "Type Comments Here";
            $("#<%=drpSM1.ClientID %>").change(function (event) {
                if ($("#<%=drpSM1.ClientID%> option:selected").text() == "Courier Pickup") {
                    ShowCourierMessage();
                }
            });
        });

        $(document).ready(function () {

            $("#<%=drpSM1.ClientID %>").change(function (event) {
                if ($("#<%=drpSM1.ClientID%> option:selected").text() == "Mail") {
                    ShowMailMessage();

                }

            });


        });

        function ShowCourierMessage() {
            var msg = "**** NOTE ****" + '\n' + "Courier Pick Up Service needs to be arranged by you." + '\n' + "Please enter into the Comments / Notes box the details of Courier Company that you will be arranging to pick up your parcel from us with.";
            alert(msg);
        }
        function ShowMailMessage() {
            var msg = "**** NOTE ****" + '\n' + " Mail will be used for parcels up to 500 grams including packaging. Parcels over 500 grams will be sent by the most economical way e.g. Courier, Road, etc. ";
            alert(msg);
        }

//        function MouseHover() {
//            $find("ShipmentModelPopupExtender").show();
//        }

//        function MouseOut() {
//            $find("ShipmentModelPopupExtender").hide();
//        }

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
        function CheckShippment() {

            switch (document.getElementById("ctl00_maincontent_drpSM1").value) {
                case 'Mail':
                    ShowShipmentPanel();
                    break;
                case 'Courier':
                    ShowShipmentPanel();
                    break;
                case 'Courier Pickup':
                    ShowShipmentPanel();
                    break;
                case 'Counter Pickup':
                    ShowShipmentPanel();
                    break;
                case 'Drop Shipment Order':
                    ShowDropShipmentPanel();
                    break;
                default:
                    ShowShipmentPanel();
                    break;
            }
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
            if (document.getElementById("ctl00_maincontent_drpSM1")!=null && document.getElementById("ctl00_maincontent_drpSM1").value!=null && document.getElementById("ctl00_maincontent_drpSM1").value == 'Drop Shipment Order') {
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

            //            // * comment by palani

            //            if ((document.getElementById("ctl00_maincontent_drpSM1").value == 'Drop Shipment Order') && ((document.getElementById("ctl00_maincontent_txtCompany").value == '') || (document.getElementById("ctl00_maincontent_txtCompany").value == null) || (document.getElementById("ctl00_maincontent_txtCompany").value == 'null'))) {
            //                document.getElementById("ctl00_maincontent_txtCompany").style.borderColor = "red";
            //                document.getElementById("ctl00_maincontent_txtCompany").focus();
            //                isCompanyEmpty = true;
            //            }
            //            else {
            //                document.getElementById("ctl00_maincontent_txtCompany").style.borderColor = "ActiveBorder";
            //            }
            //            *//


            //            // * comment by palani

            if ((document.getElementById("ctl00_maincontent_drpSM1").value == 'Drop Shipment Order') && ((document.getElementById("ctl00_maincontent_txtAttentionTo").value == '') || (document.getElementById("ctl00_maincontent_txtAttentionTo").value == null) || (document.getElementById("ctl00_maincontent_txtAttentionTo").value == 'null'))) {
                document.getElementById("ctl00_maincontent_txtAttentionTo").style.borderColor = "red";
                document.getElementById("ctl00_maincontent_txtAttentionTo").focus();
                isCompanyEmpty = true;
            }
            else {
                document.getElementById("ctl00_maincontent_txtAttentionTo").style.borderColor = "ActiveBorder";
            }
            //                       


            if ((document.getElementById("ctl00_maincontent_drpSM1").value == 'Drop Shipment Order') && ((document.getElementById("ctl00_maincontent_drpState").value == 'Select Ship To State') || (document.getElementById("ctl00_maincontent_drpState").value == null) || (document.getElementById("ctl00_maincontent_drpState").value == 'null'))) {
                document.getElementById("ctl00_maincontent_drpState").style.borderColor = "red";
                document.getElementById("ctl00_maincontent_drpState").focus();
                isStateEmpty = true;
            }
            else {
                document.getElementById("ctl00_maincontent_drpState").style.borderColor = "ActiveBorder";
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
                alert('Fill required fields before submit!');
                return false;
            }
            else {
                return true;
            }




        }
    </script>
  <script type="text/javascript">
      function DRPshippment() {
          alert('Non-Standard Delivery Area. We will contact you to confirm costing');
      }
      function couponcodekeypress(e) {

          var bv = document.getElementById("<%=txtCouponCode.ClientID%>");
          var lblerr = document.getElementById("<%=lblcouponerrmsg.ClientID%>");
          //alert(bv);
          bv.setAttribute("style", "border-color:#73ACCF #88CEF9 #88CEF9 !important;")
          //bv.style.border = "1px solid";
          //bv.style.borderColor = "rgb(178, 178, 178)"
          lblerr.setAttribute("style","display:none;")
      }
      function couponcodeError() {
          var bv = document.getElementById("<%=txtCouponCode.ClientID%>");
          bv.style.border = "1px solid";
          bv.style.borderColor = "red"
      }
</script>
 
     <div class="breadcrumb_outer1">
        <a href="home.aspx" style="float: left" class="toplinkatest" style="text-decoration:none!important;" >HOME >&nbsp;</a>
           <div class="breadcrumb1"> 
              <a href="<% =HttpContext.Current.Request.Url.ToString()
                  %>"  class="breadcrumb_txt1" style="text-transform:none;"> Shipping</a>
              <a href="home.aspx" class="breadcrumb_close1" >    
             </a>
          </div>
    </div>
  

     <table width="100%" cellspacing="0" cellpadding="5" align="center" border="0">
        <tr>
            <td align="left" >  
              
                
       <div class="box1" style="width:760px;margin:0 0 0 2px;">    
       <asp:PlaceHolder runat="server" ID="PHOrderConfirm" Visible="false" EnableViewState ="false">
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
                      <asp:Label ID="lblUserRoleName" runat="server" Visible="true" Font-Names="Arial"  Font-Size="11px" ForeColor="Black" Font-Bold="true" Text=""></asp:Label>
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
                   <div class="alert greenbox icon_2" style="padding:30px 10px 30px 60px" runat ="server"  id="greenalert">
                         <h3 style="font-size: 16px;">Your order has been successfully submitted to us for processing. Thank You!</h3>
                   </div>
           
          
   
                 <%--  <div>
                    <img src="images/img_flash/xmas-banner-checkout-01.gif" alt="x-mas" width="760px" height="205px"  />
                   </div>--%>
                <%--</td>--%>
                <% 
                        Session["ORDER_ID"] = "0";
                        Session["Multipleitems"] = null;
                    }
                     
                %>
         
           

          
    </asp:PlaceHolder>   
        
             <div class="alert redbox"  runat ="server" visible="false"  id="divonlinesubmitordererror">
                           <h3 style="font-size: 14px;color:White;font-weight: normal;text-align: center;padding-top: 4px;">
                             
Please note your account is on stop due to outstanding payments.
<br>
Please contact our accounts department ASAP to resolve this.
<br>
<span style=" color: yellow; line-height: 2;">
            EMAIL:accounts@wes.net.au | PHONE:0297979866 
</span>
</h3>
                   </div> 
            <P class="p2 fright"><span class="redx">* </span>Required Fields</P>
            <div class="clear"></div>
           <div class="quickorder4">
                    <table width="100%" runat="server" cellpadding="0" cellspacing="0" border="0" class="shippingtable"  > 
                    <tr>
                    <td style="font-size: medium; color: #0099DA;width:43%">           
                    <b> Enter Your Purchase Order No</b>                                                    
                    </td>
                        <td>
                                
     <%--   <asp:ModalPopupExtender ID="OrderNoPopupExtender" runat="server" BehaviorID="OrderNoModelPopupExtender" TargetControlID="HyperLink1"
            PopupControlID="OrderNoPopupPanel" OkControlID="btnOk" BackgroundCssClass="modalBackground">
        </asp:ModalPopupExtender>--%>
     <%--   <asp:Panel ID="OrderNoPopupPanel" runat="server" CssClass="ModalPopuporder">--%>
            <div class="ModalPopuporder"  id="OrderNoPopupPanel" style="display:none" onclick="MouseOut_orderno()" >
                <table width="100%" cellspacing="0" cellpadding="0" border="0" style="border-collapse: collapse;
                    text-align: left;">
                    <tr>
                        <td>
                           <p class="TableColumnStyle" style="margin-top:0px">
                               
                      Enter your own Order Reference Number.
                                <br/>
Enable or Disable the ‘Order No’ field as being 
                                <br/>
Mandatory during the check out of your order.
                                <br />
This option can only be enabled / disabled
                                <br/>
by your Company Admin in: 
                                <br/>
     
                              <a style="color:#0099DA;" href="WebSiteSettings.aspx">My Account > Web Site Setup</a> 
                            </p>
                         
                        </td>
                    </tr>
                </table>
               
            </div>
        <%--</asp:Panel>--%>
    
                               <div  id="moreinfoorder" runat="server">
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
                    <td >

                    Order No 
                       <%-- <span class="redx"></span>--%>
                         <asp:TextBox style="margin-left:15px;width:210px " MaxLength="12" ID="tt1" runat="server" BackColor="White" CssClass="input_dr" onkeypress="blockspecialcharforOrder(event)" />
                        <div style="display: inline-block; vertical-align: top;margin-top:-6px " id="divmanorder" runat="server">
                              <span class="maroonspan" style="margin-bottom:6px;">*</span>      
                             
                        </div>
                    </td>
                   <%-- <td  width="30%"> 
                   
                    </td>--%>
                        <td width="60%">

                             <div  id="divordermandatory" runat="server">

                  
                      
<span class="maroonspan">Required Field:This field has been set as Mandatory by your Company Admin </span>
                                 

 </div>
                        </td>
                   
                    </tr>
               <tr> 
                   <td>
                        <asp:Label Width="250px" ID="txterr" runat="server" ForeColor="red" />
                    </td>

               </tr>
                    </table>
             </div>    
                          
  <%--  <br />--%>
           <div class="quickorder4">
           <div class="form-col-8-8" >      
                    <h4 class="padbot10" style="font-size: medium; color: #0099DA;">Shipping</h4>   
                  </div>   
                   <div class="form-col-2-8">
                                      <p class="p2">Select Shipment Method
                                          <span class="redspan">*</span>
                                      </p>
                                </div>
                 <div class="form-col-3-8">
                      <asp:DropDownList NAME="drpSM1" Width="250px" ID="drpSM1" runat="server" CssClass="txtinput1">
                                            <asp:ListItem Text="Please Select Shipping Method" Value="Please Select Shipping Method">Please Select Shipping Method</asp:ListItem>
                                           <asp:ListItem Text="Courier" Value="Courier">Courier</asp:ListItem>
                                            <asp:ListItem Text="Mail" Value="Mail">Mail</asp:ListItem>                                           
                                            <asp:ListItem Text="Courier Pickup" Value="Courier Pickup">Courier Pickup</asp:ListItem>
                                            <asp:ListItem Text="Shop Counter Pickup" Value="Shop Counter Pickup">Shop Counter Pickup</asp:ListItem>
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
                         <textarea id="Ta2" cols="34"   Class="textarea1" readonly="readonly" disabled="disabled" tabindex="-1" runat="server" rows="10" 
                                            name="Ta2"></textarea>
                    </div>
                     <div class="form-col-4-8">
                        <textarea id="Ta3" cols="34"  Class="textarea1" readonly="readonly" disabled="disabled" tabindex="-1" runat="server" rows="10" 
                                            name="Ta3"></textarea>
                     </div>
                 </div>
                 <div class="clear"></div>
                 <div id="DropShipmentRow">
                    <div class="alert yellowbox">
                          <h3 style="font-size:16px;">Please Enter Shipment Delivery Details</h3>
                          <p class="p2 fright">
                            <span class="red"  style="color:#FF0000;">* </span>Required Fields
                          </p>
                          <div class="clear"></div>
                        <div class=" form-col-2-8">
                        <p class="p2">Company Name</p>
                        </div>
                        <div class=" form-col-3-8">
                          <asp:TextBox ID="txtCompany" runat="server" MaxLength="40" Width="242px" CssClass="input_dr" />
                           <asp:Label Width="150px" ID="Label13" runat="server" ForeColor="red"></asp:Label>
                        </div>
                        <div class="clear"></div>
                        <div class=" form-col-2-8">
                            <p class="p2">
                            Attn to / Receivers Code
                            <span class="red" style="color:#FF0000;">*</span>
                            </p>
                        </div>
                        <div class=" form-col-3-8">
                            <asp:TextBox ID="txtAttentionTo" runat="server" MaxLength="40" Width="242px" CssClass="input_dr" />
                            <asp:Label Width="150px" ID="Label27" runat="server" ForeColor="red"></asp:Label>
                        </div>
                        <div class="clear"></div>
                        <div class=" form-col-2-8">
                           <p class="p2">Receivers Contact Number</p>
                        </div>
                        <div class=" form-col-3-8">
                           <asp:TextBox ID="txtShipPhoneNumber" runat="server" Width="242px" MaxLength="40" CssClass="input_dr" />
                           <asp:Label Width="150px" ID="Label26" runat="server" ForeColor="red"></asp:Label>
                        </div>
                        <div class="clear"></div>
                         <div class=" form-col-2-8">
                            <p class="p2">Address Line 1</p>
                         </div>
                          <div class=" form-col-3-8">
                           <asp:TextBox ID="txtAddressLine1" runat="server" MaxLength="40" Width="242px" CssClass="input_dr" />
                            <asp:Label Width="150px" ID="lbladdline1err" runat="server" ForeColor="red"></asp:Label>
                          </div>
                            <div class="clear"></div>
                             <div class=" form-col-2-8">
                               <p class="p2">Address Line 2</p>
                            </div>
                             <div class=" form-col-3-8">
                              <asp:TextBox ID="txtAddressLine2" runat="server" MaxLength="40" Width="242px" CssClass="input_dr" />
                              <asp:Label Width="150px" ID="lbladdline2err" runat="server" ForeColor="red"></asp:Label>
                             </div>
                             <div class="clear"></div>
                             <div class=" form-col-2-8">
                                <p class="p2">
                                Suburb
                                <span class="red" style="color:#FF0000;">*</span>
                                </p>
                             </div>
                               <div class=" form-col-3-8">
                               <asp:TextBox ID="txtSuburb" runat="server" MaxLength="40" Width="242px" CssClass="input_dr" />
                               <asp:Label Width="150px" ID="Label21" runat="server" ForeColor="red"></asp:Label>
                               </div>
                                <div class="clear"></div>
                                <div class=" form-col-2-8">
                                <p class="p2">
                                State
                                <span class="red" style="color:#FF0000;">*</span>
                                </p>
                                </div>
                                <div class=" form-col-3-8">
                                  <asp:DropDownList Visible="true" ID="drpState" runat="server" Width="250px"> 
                                        <asp:ListItem Text="Select Ship To State"></asp:ListItem>
                                            <asp:ListItem Text="ACT"></asp:ListItem>
                                            <asp:ListItem Text="NSW"></asp:ListItem>
                                            <asp:ListItem Text="NT"></asp:ListItem>
                                            <asp:ListItem Text="QLD"></asp:ListItem>
                                            <asp:ListItem Text="SA"></asp:ListItem>
                                            <asp:ListItem Text="TAS"></asp:ListItem>
                                            <asp:ListItem Text="VIC"></asp:ListItem>
                                            <asp:ListItem Text="WA"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label Width="150px" ID="Label22" runat="server" ForeColor="red"></asp:Label>
                                </div>
                                    <div class="clear"></div>
                                    <div class=" form-col-2-8">
                                    <p class="p2">
                                        Post Code
                                        <span class="red" style="color:#FF0000;">*</span>
                                        </p>
                                    </div>
                                    <div class=" form-col-3-8">
                                    <asp:TextBox ID="txtPostCode" runat="server" MaxLength="4" Width="242px" CssClass="input_dr" />
                                     <asp:Label Width="150px" ID="lblpostcode2err" runat="server" ForeColor="red"></asp:Label>
                                     <asp:FilteredTextBoxExtender ID="fteMobile" runat="server" FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtPostCode" />
                                    </div>
                                    <div class="clear"></div>
                                    <div class=" form-col-2-8">
                                    <p class="p2">Country</p>
                                    </div>
                                    <div class=" form-col-3-8">
                                    <asp:TextBox ID="txtCountry" runat="server" CssClass="input_dr" Width="242px" Text="Australia" ReadOnly="True" />
                                        <asp:Label Width="150px" ID="Label24" runat="server" ForeColor="red"></asp:Label>
                                    </div>
                                     <div class="clear"></div>
                                      <div class=" form-col-2-8">
                                        <p class="p2">Delivery Instructions</p>
                                      </div>
                                      <div class=" form-col-3-8">
                                      <asp:TextBox ID="txtDeliveryInstructions"  runat="server" Width="242px"    Class="textSkin" CssClass="input_dr" MaxLength="40"   /> 
                                        <asp:Label Width="150px" ID="Label25" runat="server" ForeColor="red"></asp:Label>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtShipPhoneNumber" />
                                      </div>
                                      <div class="clear"></div>
                        </div>
                        <div class="clear padbot10"></div>
                         <div class="clear"></div>
                        <div class="form-col-8-8">
                                <textarea  id="Ta4" cols="34"   Class="textarea1"  readonly="readonly" disabled="disabled" tabindex="-1" runat="server" rows="10" name="Ta4"></textarea>
                        </div>
                         <div class="clear"></div>
                 </div>
                    
 
           <div id="PopDiv" class="containership">
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" BehaviorID="ShipmentModelPopupExtender" TargetControlID="HyperLink1"
            PopupControlID="ShipmentPopupPanel" OkControlID="btnOk" BackgroundCssClass="modalBackground">
        </asp:ModalPopupExtender>
        <asp:Panel ID="ShipmentPopupPanel" runat="server" CssClass="ModalPopupStyleship">
            <div class="containership">
                <table width="100%" cellspacing="0" cellpadding="0" border="0" style="border-collapse: collapse;
                    text-align: left;">
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
                                    onclick="window.open('Termsandconditions.aspx','popup','width=800,height=600,scrollbars=yes,resizable=no,toolbar=no,directories=no,location=no,menubar=no,modal=yes,status=no,left=150,top=25'); return false">
                                    Terms and Conditions</a>
                            </p>
                        </td>
                    </tr>
                </table>
                <div>
                    <asp:Button ID="btnOk" runat="server" Text="Close" CssClass="ButtonStyleship"  />
                </div>
            </div>
        </asp:Panel>
    </div>
               </div>
           <div class="quickorder4">
    <table id="Table3" width="100%"  runat="server" cellpadding="0" cellspacing="0" border="0" >
        <tr>
          <%--  <td width="2%">
                &nbsp;
            </td>--%>
            <td width="100%" align="left" >
                <table width="100%" runat="server" cellpadding="1" cellspacing="0" border="0" style="border-style: none" id="colo2">
                    <tr>
                        <td colspan="2" style="font-size:16px; color: #0099DA;">
                           <%-- <b>Comments / Notes</b>--%>
                           <div class=" form-col-8-8">
                              <h4 class="padbot10">Comments / Notes</h4>
                           </div>
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="2" width="75%" >
                           <div class="form-col-6-8">
                            <asp:TextBox ID="TextBox1"  runat="server" Rows="5" Columns="30" Font-Size="12px" CssClass="textarea1" Width="535px"  Height="72px" Font-Names="arial"  MaxLength="240"  onkeyDown="return checkMaxLength(this,event,'240');" 
                                TextMode="MultiLine">
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
                        <td align="center">
                        </td>
                    </tr>
                </tbody>
                
                </table>
                
                  

   

           <div class="quickorder4">
     <table id="Table4" width="100%" runat="server" cellpadding="0" cellspacing="0" border="0" style="padding-left:5px" >
        <tr>
            <td >
                   <div class=" form-col-8-8">
                         <h4 class="padbot10" style="font-size:16px; color: #0099DA;">Your Order Contents</h4>
                   </div> 
                                 
  
                       
     </td>
    </tr>
    <tr>
       <td>
           <asp:Panel ID="PnlOrderContents" Visible="true" runat="server">
                    <%
                       HelperServices objHelperServices = new HelperServices();
                       
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
                    
                    <table width="100%"  class="orderdettable"  > 
                             
                        <tr  height="5px">
                            <td bgcolor="#F2F2F2" align="left" style="border-style: none solid solid none; border-width: thin; border-color: #E7E7E7;border: thin solid #E7E7E7; padding: 0px 0 0 9px !important;" width="13%">
                                <b>Order Code</b>
                            </td>
                            <td style="border-style: none solid solid none; border-width: thin; border-color: #E7E7E7;border: thin solid #E7E7E7;padding: 0px 0 0 9px !important;"
                                bgcolor="#F2F2F2" align="left" width="10%">
                                <b>Quantity</b>
                            </td>
                            <td style="border-style: none solid solid none; border-width: thin; border-color: #E7E7E7;border: thin solid #E7E7E7;padding: 0px 0 0 9px !important;"
                                colspan="2" bgcolor="#F2F2F2" align="left" width="30%">
                                <b>Description</b>
                            </td>
                            <%-- <td  
                              style="border-style: none solid solid none; border-width: thin; border-color: #E7E7E7" 
                              bgcolor="White" align="center"  width="10%">
                              <b>Availability</b></td>--%>
                            <td style="border-style: none solid solid none; border-width: thin; border-color: #E7E7E7;border: thin solid #E7E7E7;padding: 0px 0 0 9px !important;"
                                bgcolor="#F2F2F2" align="left" width="20%">
                                <b>Cost (Ex. GST)</b>
                            </td>
                            <td style="border-style: none none solid none; border-width: thin; border-color: #E7E7E7;border: thin solid #E7E7E7;padding: 0px 0 0 9px !important;"
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
                                        int pQty = objOrderServices.GetOrderItemQty(pid, OrderID,OrderItemId1);

                                        maxqty = objHelperServices.CI(rItem["QTY_AVAIL"].ToString());
                                        maxqty = maxqty + objHelperServices.CI(Request.Form["txtQty"] + pQty);
                                        minQty = objHelperServices.CI(rItem["MIN_ORD_QTY"].ToString());
                                        ProductUnitPrice = objHelperServices.CDEC(rItem["PRICE_EXT_APPLIED"].ToString());
                                        ProductUnitPrice = objHelperServices.CDEC(ProductUnitPrice.ToString("N4"));
                                        //ProdShippCost = CalculateShippingCost(OrderID, pid, ProductUnitPrice, pQty);
                                        //TotalShipCost = objHelperServices.CDEC(TotalShipCost + ProdShippCost); 
                                        int Qty = objHelperServices.CI(rItem["QTY"].ToString());
                                        decimal ProdTotal = Math.Round(Qty * ProductUnitPrice, 2,MidpointRounding.AwayFromZero);
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
                        %>
                        <tr>
                            <td  bgcolor="White" align="left" class="">
                                <%Response.Write("<a class=\"toplinkatest\" href =ProductDetails.aspx?&Pid=" + pid + "&fid=" + FId.ToString() + ">" + rItem["CATALOG_ITEM_NO"].ToString() + "</a>");%>
                            </td>
                            <td  bgcolor="White" class="Numeric" align="left">
                                <%Response.Write("<input type =\"Text\" Id=\"txtQtyId" + i + "_" + maxqty + "\" Name =\"txtQty" + i + "\" size=\"5\" style=\"height:15px;width:83px;padding:2px;border:#878787 1px solid\" disabled=\"disabled\" runat =\"server\" onBlur=\"javascript:return Check(" + i + "," + maxqty + "," + minQty + "," + Qty + ");\" value =\"" + Qty + "\">"); %>
                            </td>
                            <td  bgcolor="White" colspan="2">
                                <%Response.Write(Desc);%>
                            </td>
                            <%--   <td <% Response.Write(sty); %>="" 
                                                            bgcolor="White" ="" ><%Response.Write(Available);%></td>--%>
                            <td  bgcolor="White" align="left" class="NumericField"
                                style="width: 130px;text-align:left;">
                                <%Response.Write(cSymbol + " " + objHelperServices.CheckPriceValueDecimal(ProductUnitPrice.ToString("#,#0.0000")));%>
                            </td>
                            <td  bgcolor="White" align="center" class="NumericField" style="text-align:left;">
                                <%Response.Write(cSymbol + " " + ProdTotal.ToString("#,#0.00")); %>
                            </td>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtPid" + i + "\" runat=\"server\" value=\"" + pid + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtCatItem" + i + "\" runat=\"server\" value=\"" + rItem["CATALOG_ITEM_NO"].ToString() + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtMaxQty" + i + "\" runat=\"server\" value=\"" + maxqty + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtMinQty" + i + "\" runat=\"server\" value=\"" + minQty + "\">"); %>
                            <%Response.Write("<input type=\"hidden\" Name=\"txtUntPrice" + i + "\" runat=\"server\" value=\"" + ProductUnitPrice.ToString("#,#0.00") + "\">"); %>
                            <% Response.Write("<input type=\"hidden\" Name=\"txtPrdTprice" + i + "\" runat=\"server\" value=\"" + ProdTotal.ToString("#,#0.00") + "\">"); %>
                        </tr>
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
                                bgcolor="White" class="style22ship" align="left" style="width: 130px;text-align:left;">
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
                                bgcolor="White" class="style23ship" align="left" style="width: 130px;text-align:left;" >
                                <%Response.Write(cSymbol + " " + objHelperServices.CheckPriceValueDecimal(ProductUnitPrice.ToString("#,#0.0000")));%>
                            </td>
                            <%--								                                <td class="NumericField" align="center"><%Response.Write(cSymbol + ProdShippCost.ToString("#,#0.00")); %></td>
                            --%>
                            <td 
                                bgcolor="White" class="NumericField" align="left" style="text-align:left;">
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
                            <%if (objOrderServices.IsNativeCountry(OrderID) == 0)
                              {
                                   %>
                                <td colspan="4" rowspan="5" bgcolor="white" valign="top" align="right">
                            <%}
                              else
                              { %>
                                <td colspan="4" rowspan="3" bgcolor="white" valign="top" align="right">
                                <%} %>
                               <%-- <font color="red">Availability & Cost is only Estimate. Actual Invoice may vary.</font>--%>
                            </td>
                           <%-- <td bgcolor="white"  >
                            </td>--%>
                            <td class="NumericField" colspan="1" bgcolor="white"  align="left" style="text-align:left;">
                                Sub Total
                            </td>
                            <td class="NumericField" bgcolor="white" align="left" style="text-align:left;">
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
                                if (objOrderServices.IsNativeCountry(OrderID) == 0)   
                            {
                                %>
                                 <tr>
                          
                            <td class="NumericField" colspan="1" style="height: 21px;text-align:left;" align="left">
                                Shipping Charge <br />
                                <span style="font-size: 4"></span>
                            </td>
                            <td class="NumericField" style="height: 21px;text-align:left;" align="left">
                                To Be Advised                                
                                          </td>
                                </tr>
                       

                                <%
                           }                                                                 
                                 %>
                            <td class="NumericField" colspan="1" style="height: 21px;text-align:left;" align="left">
                                Tax Amount(GST)<br />
                                <span style="font-size: 4"></span>
                            </td>
                            <td class="NumericField" style="height: 21px;text-align:left;" align="left">
                                <%       
                                    //string sSQL = string.Format("SELECT TAX_AMOUNT FROM TBWC_ORDER WHERE ORDER_ID = {0}", OrderID);
                                    //objHelperServices.SQLString = sSQL;
                                    //taxamt = System.Convert.ToDecimal(objHelperServices.GetValue("TAX_AMOUNT"));
                                    //if (subtot > 0)
                                    //{
                                    //    taxamt = objOrderServices.CalculateTaxAmount(subtot, OrderID.ToString()); //Math.Round((subtot * 10 / 100), 2, MidpointRounding.AwayFromZero);
                                    //}
                                    //else
                                    //{
                                    //    taxamt = 0;
                                    //}
                                    //Total = subtot + taxamt + TotalShipCost;
                                    //Total = objHelperServices.CDEC(objHelperServices.FixDecPlace(Total));%>
                                <%
                                //=objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString() + " " + objHelperServices.CDEC(objHelperServices.FixDecPlace(taxamt))
                                    Response.Write(cSymbol + " " + oOrderInfo.TaxAmount);
                                    %>
                            </td>
                        </tr>
                        <tr>
                           
                            <td class="NumericFieldship" colspan="1" style="height: 21px;border-style: none solid none none; border-width: thin; border-color: #E7E7E7" align="left">
                                 <%
                                    if (objOrderServices.IsNativeCountry(OrderID) == 0)
                                    {                                        
                                     %>                                        
                                         <strong>Est. Total </strong><br />
                                     <%
                                    }
                                    else
                                    {
                                         %>
                                        <strong>Est. Total Inc GST</strong><br />
                                         <%
                                    } %>
                           
                                <span style="font-size: 4">(Freight not included)</span>
                            </td>
                            <td class="NumericFieldship" style="height: 21px;border-style: none solid none none; border-width: thin; border-color: #E7E7E7" align="left" >
                                <strong>
                                    <%
                                    //=objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString() + " " + objHelperServices.CDEC(objHelperServices.FixDecPlace(Total))
                                     Response.Write(cSymbol + " " + oOrderInfo.TotalAmount);
                                        %>
                                </strong>
                            </td>
                        </tr>
                                <tr style="display:none;">
                    
                    <td colspan="6" >
                    <div style=" margin: 10px 10px 10px 10px;text-align:right;">
                    Coupon Code :  <span class="redx"></span>
                    <asp:TextBox Width="150px" MaxLength="20" ID="txtCouponCode" runat="server" BackColor="White" CssClass="input_dr" onkeypress="blockspecialcharacters(event);couponcodekeypress(event);" />
                    <br />
                    <asp:Label Width="250px" ID="lblcouponerrmsg" runat="server" ForeColor="red" Visible=false  />
                    </div>
                    </td>
                    
                    </tr>
                       
                        <tr>
                            <td colspan="6"  style="margin-right:60px"> 
                                <asp:Button runat="server" ID="ImageButton2" Text="Submit Order" style="margin:10px 0 10px 15px;float:right" class="button normalsiz btnblue "  OnClientClick="return checkorderid()" OnClick="ImageButton4_Click" Visible="false" />
                     
                                <asp:Button ID="ImageButton1" runat="server" Text="Edit/Update Order" style="margin:10px 0 10px 15px;float:right" OnClick="ImageButton1_Click" class="buttongray normalsiz btngray "/>
                                
                                   </td>
                    </tr>
                    </table>
                            
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
                        <td colspan="2" style="font-size:16px; color: #0099DA;">
                           <div class="form-col-8-8" >
                              <h4 class="padbot10">Payment Options</h4>
                           </div>
                           
                            
                        </td>
                    </tr>
                
                    
                    <tr>
                        <td rowspan="" width="100%">
                        
                           <div class=" payment_btns">
                          
                                <label for="contactChoice1" class="form-col-2-8 payment_radio " id="lbldefaultpayment" >
                                      <asp:RadioButton ID="RBdefautpaymenttype" runat="server" onclick="Defaultclick()"  Text="Direct Deposit" GroupName="Paymentoption"/>
                              <%--  <input id="contactChoice1" name="contact" value="email" type="radio">--%>
                               
                                    <div runat="server" style="display:none" id="divmastercard">
                                        <span style="height: 37px; display: inline-block;">
  <img style="display:inline-block" src="../images/master-card.png" alt=""/>
                                        </span>
                                      <span style="display: inline-block; height: 30px; vertical-align: middle;">
  <asp:Label ID="lblmastercardno" runat="server" Text="Label" style="height: 15px; display: block;" ></asp:Label>
<asp:Label ID="lblmasterexpirydate" runat="server" Text="Label" style="height: 15px; display: block;"></asp:Label>
                                          </span>
                                        
                                    </div>
                                      <div runat="server" id="divdirectdeposit">
                                        <img style="display:inline-block" src="../images/bank-transfer.png" alt=""/>
   
                                    </div>
                                     <div runat="server" id="divpayoaccount" style="display:none">
                                        <img style="display:inline-block" src="../images/charge-account.png" alt=""/>
   
                                    </div>
                                </label>
               
                                
                                <label for="contactChoice2" class="form-col-2-8 payment_radio"  id="lblcreditcard">
                                     <asp:RadioButton ID="RBCreditCard" runat="server" onclick="creditcardclick()" Text="New Credit Card" GroupName="Paymentoption" ValidationGroup="payonlinepaynow" />
                              <%--  <input id="RBCreditCard" name="contact" value="phone" type="radio">--%>
                                
                                <div><img src="../images/credit_card.png" alt=""/></div>
                                </label>
                                
                                <label for="contactChoice3" class="form-col-1-8 payment_radio" id="lblpaypalcard" >
                                <asp:RadioButton ID="RBPaypal" runat="server"  onclick="paypalclick()" Text="Paypal" GroupName="Paymentoption"/>
                                
                                <div><img src="../images/paypal_pay.png" alt=""/></div>
                                </label> 
                                                           <div class="form-col-3-8">
                                	<p style="margin-top:2px; margin-bottom:8px; text-align:right;padding-right:35px; font-size:13px;">Total Amount: </p>
                                    <h2 style="margin-top:15px;padding-right:35px;color:#0099DA; font-family:Arial;font-weight: bold;font-size: 26px;text-align:right ">
                                       
                                        $ 
                                     
                                        <asp:Label ID="lbltotalamt" runat="server" Text="" CssClass="LabelStyle"  ></asp:Label>
                                    </h2>
                                </div>
                                <div class="clear"></div>
                            </div>
                               <div runat="server" id="divpayonacc" style="display:none">

                                        <img style="display:inline-block" src="../images/charge-account.png" alt=""/>
                                        </div>


                                   <div id="divdedault" style="margin-top:20px " >
                                   
                            
                              <div class="payment_contents"  id="divD1"  runat="server"  style="padding-bottom:20px">
                      <h2 style="color:#0099DA; padding-right:22px; font-size: 13px;  ">Pay By Direct Deposit</h2>
                                     <p style="font-size: 13px;line-height: 1.5;padding-right: 153px;color:#555">
We will email you a proforma invoice with bank trasfer details once your order has been submitted.</br></br>
                                      When sending funds please include the Incoice No as your payment reference for faster processing 
                                      and notify our accounts department by email  <a style="color:#0099DA;" href="mailto:accounts@wes.net.au">accounts@wes.net.au </a>
                                      with your transfer receipt.
                                   </p> 
  <asp:Button runat="server" ID="btndirectdeposit" Text="SUBMIT ORDER" style="margin:10px 32px 10px 10px;float:right" class="normalsiz paynow"  OnClientClick="return checkorderid()" OnClick="ImageButton4_Click" />
                                   <div class="clear"></div>
                                               </div>
                                               <div class="payment_contents"  id="divD2"  runat="server"  style="padding-bottom:20px" >
                                                      <h2 style="color:#0099DA; padding-right:22px; font-size:13px; "> Charge Account</h2>
                                                  
                                                   <p>Your Account will be billed for Invoice </p> 
  <asp:Button runat="server" ID="btnpayonacc" Text="SUBMIT ORDER" style="margin:10px 32px 10px 10px;float:right" class="normalsiz paynow"  OnClientClick="return checkorderid()" OnClick="ImageButton4_Click" />
                     <div class ="clearfix"> </div> 
                                               </div>
                                       <div class="payment_contents"  id="divD3"   runat="server"  style="padding-bottom:20px" >
  <asp:Button runat="server" ID="btnmastercard" Text="SUBMIT ORDER" style="margin:10px 32px 10px 10px;" class="normalsiz paynow"  OnClientClick="return checkorderid()" OnClick="ImageButton4_Click" />
                       <div class="clear"></div>
                                               </div>
                                      
                                       </div>
                                   

                            <div class="payment_contents" style="display:none;margin-top:20px" id="divcreditcard"  >
                            <div class="form-col-6-8">
                            	<div class="mastercard_pay"></div>
                                
                                
                                <div class="creditcard_pay" >
                                	<div>
                                	<label class="pform_title">Credit Card Number</label>
<asp:TextBox ID="txtcreditcardno" runat="server" class="input_100" MaxLength="19" ValidationGroup="payonlinepaynow" >

                                </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RBcreditcardno" runat="server"  class="vldRequiredSkin_creditcard" ErrorMessage="Please Enter Card No" ControlToValidate ="txtcreditcardno" ValidationGroup="payonlinepaynow"></asp:RequiredFieldValidator>

                                    <%--<input class="input_100" type="text">--%>
                                    </div>
                                    
                                    <div class="form-col-4-8">
                                    	<label class="pform_title">CVV</label>
                                        <asp:TextBox ID="txtCVV" runat="server" class="input_50" MaxLength="3" ValidationGroup="payonlinepaynow" ></asp:TextBox>
                                    	  <asp:RequiredFieldValidator ID="RBCVV" runat="server"  class="vldRequiredSkin_creditcard" ErrorMessage="Please Enter CVV" ControlToValidate ="txtCVV" ValidationGroup="payonlinepaynow"></asp:RequiredFieldValidator>
                                        <%--<input class="input_50" type="text">--%>
                                    </div>
                                    <div class="form-col-4-8">
                                 
                                    	<label class="pform_title">Expiry Date</label>
                                             <asp:DropDownList NAME="drpExpmonth"  ID="drpExpmonth" runat="server"   class="payment_select">           
                    <asp:ListItem Selected ="true" Text ="01" Value="01"></asp:ListItem>
                    <asp:ListItem  Text ="02" Value="02"></asp:ListItem>
                    <asp:ListItem  Text ="03" Value="03"></asp:ListItem>
                    <asp:ListItem Text ="04" Value="04"></asp:ListItem>
                    <asp:ListItem  Text ="05" Value="05"></asp:ListItem>
                    <asp:ListItem  Text ="06" Value="06"></asp:ListItem>
                    <asp:ListItem  Text ="07" Value="07"></asp:ListItem>
                    <asp:ListItem Text ="08" Value="08"></asp:ListItem>
                    <asp:ListItem  Text ="09" Value="09"></asp:ListItem>
                    <asp:ListItem  Text ="10" Value="10"></asp:ListItem>
                    <asp:ListItem  Text ="11" Value="11"></asp:ListItem>
                    <asp:ListItem  Text ="12" Value="12"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList NAME="drpExpyear"  ID="drpExpyear" runat="server" class="payment_select">          
                    </asp:DropDownList>
                                             <asp:RequiredFieldValidator ID="RBexpirydate" runat="server"   class="vldRequiredSkin_creditcard" ErrorMessage="Please Enter Expiry Date" ControlToValidate ="drpExpyear" ValidationGroup="payonlinepaynow"></asp:RequiredFieldValidator>
                                         </div>
                 
                             
                                    </div>
                                    <div class="clear"></div>
                                    
                                    <div>
                                    <label class="pform_title">Name On Card</label>
                                          <asp:TextBox ID="txtnamecard" runat="server" class="input_100" MaxLength="150" ValidationGroup="payonlinepaynow"></asp:TextBox>
                                                  <asp:RequiredFieldValidator ID="RBCardname" runat="server"  class="vldRequiredSkin_creditcard" ErrorMessage="Please Enter Name On Card" ControlToValidate ="txtnamecard" ValidationGroup="payonlinepaynow"></asp:RequiredFieldValidator>
                                    </div>
                                    
                                    <div>
                                            <asp:Button ID="btnSP" runat="server" Text="Pay Now"  ValidationGroup="payonlinepaynow" class="paynow" OnClick="btnSecurePay_Click"   OnClientClick="return checkorderid()"/>
                                        <asp:Button runat="server" ID="BtnProgressSP" Text="Processing Payment. Please Wait…" style="display:none;visibility:visible;float:left;" class="btn btn-primary" Enabled="false"   />     
                 
                                    </div>
                                    
                                    <div class="clear"></div>
                                </div>
                                  <div id="divContent" style="font-size:12px margin-left:30px;color:Red" runat="server"></div>
                                
                                
                                <div class="paypal_pay"></div>
                                
                                
                                <div class="clear"></div>
                            </div> 
                            <div class="payment_contents form-col-8-8" style="display:none;margin-top:20px;padding-bottom:20px " id="divpaypal">
                                              <div class="payment_contents creditcard_pay">
                                                    <h2 style="color:#0099DA; padding-right:22px; font-size: 13px;  ">Pay with Paypal</h2>

              <p style="margin-top:8px;margin-bottom:40px;font-size:12px;line-height:2" >
                  Pay using your PayPal Account </br>
                  You will be redirected to PayPal site to complete your purchase.</p>
                   <div class="col-sm-20 nolftpadd">
                    <div class="form-group col-lg-8 nolftpadd">
                     <h3 class="green_clr"><asp:Label runat="server" ID="lblpaypaltotamt" CssClass="totalamt" Visible="false" /> 
                    </h3>           
                    </div>
                </div>

  <asp:Button runat="server" ID="btnPay" Text="Pay Now" class="paynow"  OnClick="btnPay_Click" OnClientClick="return checkorderid()"  style="margin-right:28px " />       

                    <asp:Button runat="server" ID="btnPayApi" Text="Pay Now" style="width:100px;" class="paynow" Visible="false"  OnClick="btnPayApi_Click" OnClientClick="Setinit(this.id)" />       


                 
                 <asp:Button runat="server" ID="BtnProgress" Text="Processing Payment. Please Wait…" style="display:none;visibility:visible;float:left;" class="btn btn-primary " Enabled="false"   />       
  <%--                <input id="btnpaypal" value="Pay Now" class="paynow" type="button" runat="server" />--%>
 <div class="clear"></div>
</div>

                                          </div>
                              <div class="clear"></div>
                                <div runat ="server" id="div2" class="accordion_head_yellow gray_40" style="font-size:12px;text-align:center; font-weight:bold;margin-bottom:12px;background: #FFF200; padding: 12px 17px;" visible ="false" >
                </div>
                            </div>
  
    
    
    
    
    
       <div class="quickorder4" id="divInternationalpayoption" runat="server" style="display:none">
            <table id="Table1" border="0" cellpadding="0" cellspacing="0" width="100%">
            <tbody>
            <tr>
                <td align="left" width="100%">
                        <table id="Table2" style="border-style: none" border="0" cellpadding="1" cellspacing="0" width="100%">
                    <tbody>
                    <tr>
                        <td colspan="2" style="font-size:16px; color: #0099DA;">
                           <div class="form-col-8-8" >
                              <h4 class="padbot10">Payment Options</h4>
                           </div>
                           
                            
                        </td>
                    </tr>
                
                    
                    <tr>
                        <td rowspan="" width="100%">
                        
                           <div class=" payment_btns">
                          
                                <label for="contactChoice1" class="form-col-2-8 payment_radio " id="Label9" >
                                      <asp:RadioButton ID="rbinternationaldefault" runat="server" onclick="Defaultclick_int()"  Text="Direct Deposit" GroupName="Paymentoption"/>
                          
                               
                                   
                                      <div>
                                        <img style="display:inline-block" src="../images/bank-transfer.png" alt=""/>
   
                                    </div>
                                    
                                </label>
               
                                
                              
                                <label for="contactChoice3" class="form-col-1-8 payment_radio" id="Label28" >
                                <asp:RadioButton ID="rbinternationalpaypal" runat="server"  onclick="paypalclick_int()" Text="Paypal" GroupName="Paymentoption"/>
                                
                                <div><img src="../images/paypal_pay.png" alt=""/></div>
                                </label> 
                                                         <%--  <div class="form-col-3-8">
                                	<p style="margin-top:2px; margin-bottom:8px; text-align:right;padding-right:35px; font-size:13px;">Total Amount: </p>
                                    <h2 style="margin-top:15px;padding-right:35px;color:#0099DA; font-family:Arial;font-weight: bold;font-size: 26px;text-align:right ">
                                       
                                        $ 
                                     
                                        <asp:Label ID="Label29" runat="server" Text="" CssClass="LabelStyle"  ></asp:Label>
                                    </h2>
                                </div>--%>
                                <div class="clear"></div>
                            </div>
                             


                                   <div id="divintdirdep" style="margin-top:20px " >
                                   
                            
                              <div class="payment_contents"  id="div8"  runat="server"  style="padding-bottom:20px">
                      <h2 style="color:#0099DA; padding-right:22px; font-size: 13px;  ">Pay By Direct Deposit</h2>
                                     <p style="font-size: 13px;line-height: 1.5;padding-right: 153px;color:#555">
We will email you a proforma invoice with bank trasfer details once your order has been submitted.<br/><br/>
                                      When sending funds please include the Incoice No as your payment reference for faster processing 
                                      and notify our accounts department by email  <a style="color:#0099DA;" href="mailto:accounts@wes.net.au">accounts@wes.net.au </a>
                                      with your transfer receipt.
                                   </p> 
                                       <%
                                           OrderServices objOrderServices = new OrderServices();            
                            OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                            oOrderInfo = objOrderServices.GetOrder(OrderID);
                            
                                    if ( (oOrderInfo.TotalAmount<1000))
                                    {   %>                                      
                          <div class="alert yellowbox icon_1">
                  
                       <p class="p2">
                      Note. For orders under AUD $1000 an additional bank fee of AUD $28.00 will be added
                       
                    
                      </p>
                      </div>
                               
                                 <% }%> 
                                                
  <asp:Button runat="server" ID="btndristdepositsuborder" Text="SUBMIT ORDER" style="margin:10px 32px 10px 10px;float:right" class="normalsiz paynow"  OnClientClick="return checkorderid()" OnClick="ImageButton4_Click" />
                                   <div class="clear"></div>
                                               </div>
                                           
                                   
                                      
                                       </div>
                                   

                    
                            <div class="payment_contents form-col-8-8" style="display:none;margin-top:20px;padding-bottom:20px " id="divinternationalpaypal">
                                              <div class="payment_contents creditcard_pay">
                                                    <h2 style="color:#0099DA; padding-right:22px; font-size: 13px;  ">Pay with Paypal</h2>

              <p style="margin-top:8px;margin-bottom:40px;font-size:12px;line-height:2" >
                  Pay using your PayPal Account </br>
                  You will be redirected to PayPal site to complete your purchase.</p>
                   <div class="col-sm-20 nolftpadd">
                    <div class="form-group col-lg-8 nolftpadd">
                     <h3 class="green_clr"><asp:Label runat="server" ID="Label30" CssClass="totalamt" Visible="false" /> 
                    </h3>           
                    </div>
                </div>

  <asp:Button runat="server" ID="btnpaypalsubmitorder" Text="Pay Now" class="paynow"  OnClick="ImageButton4_Click" OnClientClick="return checkorderid()"  style="margin-right:28px " />       

                  
  <%--                <input id="btnpaypal" value="Pay Now" class="paynow" type="button" runat="server" />--%>
 <div class="clear"></div>
</div>

                                          </div>
                              <div class="clear"></div>
                                <div runat ="server" id="div14" class="accordion_head_yellow gray_40" style="font-size:12px;text-align:center; font-weight:bold;margin-bottom:12px;background: #FFF200; padding: 12px 17px;" visible ="false" >
                </div>
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
            <td align="center">
                &nbsp;<asp:Label ID="LblStar" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"
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
                        <td style="width: 10px">
                        </td>
                        <td style="width: 144px">
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10px">
                        </td>
                        <td style="width: 144px">
                        </td>
                        <td>
                            &nbsp;
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
                        <td>
                            &nbsp;
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
                        <td>
                            &nbsp;
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
                                    <td style="width: 10px">
                                    </td>
                                    <td style="width: 135px">
                                        <asp:Label ID="lblProvider" runat="server" meta:resourcekey="lblProvider" Class="lblNormalSkin"></asp:Label>
                                    </td>
                                    <td style="width: 376px">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px">
                                    </td>
                                    <td style="width: 135px">
                                        <asp:Label ID="lblMethod" runat="server" meta:resourcekey="lblMethod" Class="lblNormalSkin"></asp:Label>
                                    </td>
                                    <td style="width: 376px">
                                        &nbsp;
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
                                    <td style="width: 10px">
                                    </td>
                                    <td style="width: 135px">
                                    </td>
                                    <td style="width: 376px">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px">
                                    </td>
                                    <td style="width: 135px">
                                    </td>
                                    <td style="width: 376px">
                                        &nbsp;
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
                                    <td style="width: 376px">
                                        &nbsp;
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
                                    <td style="width: 376px">
                                        &nbsp;
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
                            <td class="tablerow" align="right" style="height: 26px">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
            </td>
        </tr>
        <!-- Proceed to NExt Page end up here-->
    </table>
    <% } %>

   <asp:Button ID="btnHiddenTestPopupExtender" runat="server" Style="display: none;
        visibility: hidden"></asp:Button>
    <div id="PopupOrderMsg" align="center" runat ="server">
        <asp:Panel ID="ModalPanel" runat="server" CssClass="PopUpDisplayStyleship">
            <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;"
                align="center">
                <tr style="height: 5px">
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td width="100%" align="center" colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td width="100%" align="center" colspan="3" class="TextContentStyleship">
                       Account Activation Required before you can proceed to check out.
                        <br />
                        Please check your email for account activation link as this was emailed to you when you registered your account.
                        <br />
                        If you would like us to send you the Activation Email again. <a Href="ConfirmMessage.aspx?Result=REMAILACTIVATION" class="toplinkatest">Please Click Here</a>
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td width="100%" align="center" colspan="3">
                        &nbsp;
                    </td>
                </tr>
            
                <tr style="height: 10px">
                    <td width="35%" align="right">
                       <%-- <asp:Button ID="ForgotPassword" runat="server" Text="Close"
                            Width="205px"  CssClass="ButtonStyle" OnClick="btnForgotPassword_Click" />--%>
                    </td>
                    <td width="30%">
                         <asp:Button ID="ForgotPassword" runat="server" Text="Close"
                            Width="205px"  CssClass="button normalsiz btnblue" OnClick="btnForgotPassword_Click" />
                    </td>
                    <td width="35%" align="left">
                        <%--<asp:Button ID="Close" runat="server" Text="Close" Width="165px"
                            CssClass="ButtonStyle" OnClick="btnClose_Click" />--%>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>   
     <div id="ResetPassAlertMsg" align="center" runat ="server">
        <asp:Panel ID="pnlResetPassAlert" runat="server" CssClass="PopUpDisplayStyleship">
            <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;"
                align="center">
                <tr style="height: 5px">
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td width="100%" align="center" colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td width="100%" align="center" colspan="3" class="TextContentStyleship">
                       Your password was reseted,Change password action required before you can proceed to check out.
                        <br />
                         Please check your email for reseted password as this was emailed to you.
                        <br />
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td width="100%" align="center" colspan="3">
                        &nbsp;
                    </td>
                </tr>
            
                <tr style="height: 10px">
                    <td width="35%" align="right">
                       <%-- <asp:Button ID="ForgotPassword" runat="server" Text="Close"
                            Width="205px"  CssClass="ButtonStyle" OnClick="btnForgotPassword_Click" />--%>
                    </td>
                    <td width="30%">
                         <asp:Button ID="btnGoHome" runat="server" Text="Close"
                            Width="205px"  CssClass="button normalsiz btnblue" OnClick="btnGoHome_Click" PostBackUrl="~/Shipping.aspx?RPWD=true" />
                    </td>
                    <td width="35%" align="left">
                        <%--<asp:Button ID="Close" runat="server" Text="Close" Width="165px"
                            CssClass="ButtonStyle" OnClick="btnClose_Click" />--%>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>   
 
     
   
 
   
   
   
    
   
   
   
 
   
   
   
   
   
   
   
 
   
   
   
   
   
   
   
 
   
   
   
    
   
   
   
 
   
   
   
   
   
   
   
 
   
   
   
    
   
   
   
 
   
   
   
    
   
   
   
 
   
   
   
   
   
   
   
 
   
   
   
</asp:Content>
