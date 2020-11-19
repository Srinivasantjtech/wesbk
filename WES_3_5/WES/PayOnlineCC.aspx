<%@ Page Title="" Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" CodeBehind="PayOnlineCC.aspx.cs" Inherits="PayOnlineCC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%--<script language=JavaScript>
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
</script>
   <script language="javascript" type="text/javascript">

     

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
        } ();

        // expose my public methods
        window[EXPOSED_NS] = {
            doSomething: myApp.DoSomething,
            doSomethingElse: myApp.DoSomethingElse
        };

        if (DEBUG) {
            window.MyApp = myApp
        }
    } ());

    window.onload = func1;

    function func1() {
        document.getElementById("r1").scrollIntoView();
    }
    function Setinit() {
        var x = document.getElementById('<%= btnPay.ClientID %>');
        var y = document.getElementById('<%= BtnProgress.ClientID %>');
        var z = document.getElementById('<%= ImgBtnEditShipping.ClientID %>');
        x.style.display = "none";
        y.style.display = "block";
        y.style.visibility = "visible";
        //z.style.display = "block";
        //z.style.visibility = "visible";
        z.style.display = "none";
        z.style.visibility = "hidden";
    }

    function creditcardclick() {
        document.getElementById("ctl00_maincontent_divcreditcard").style.display = 'block';
        document.getElementById("ctl00_maincontent_divdedault").style.display = 'none';

        document.getElementById("ctl00_maincontent_divpaypal").style.display = 'none';
        try {
            document.getElementById("ctl00_maincontent_ImageButton2").style.display = 'none';
        }
        catch (Error)
        { }

      

    }
    function paypalclick() {
        try {
            document.getElementById("ctl00_maincontent_divpaypal").style.display = 'block';

            document.getElementById("ctl00_maincontent_divdedault").style.display = 'none';
            document.getElementById("ctl00_maincontent_divcreditcard").style.display = 'none';
            try {
                document.getElementById("ctl00_maincontent_ImageButton2").style.display = 'none';
            }
            catch (Error)
            { }

            document.getElementById("ctl00_maincontent_div2").style.display = 'none';
        }
        catch (error) {
           
        }
     

    }

    function Defaultclick() {
        try {
            document.getElementById("ctl00_maincontent_divdedault").style.display = 'block';
            document.getElementById("ctl00_maincontent_divcreditcard").style.display = 'none';
            document.getElementById("ctl00_maincontent_divpaypal").style.display = 'none';
            try {
                document.getElementById("ctl00_maincontent_ImageButton2").style.display = 'none';
            }
            catch (Error)
            { }
        
        }
        catch (error) {
            
        }
  
    }

</script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="server">

</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" runat="server">
</asp:Content>--%>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" runat="server">
   <%-- <div class="box1" style="width: 955px;">--%>

    <table  width="100%" border="0" 
    align="left">
    <tr>
    <td>
    
   <div id="page-wrap">
    <%--<H4 style="TEXT-ALIGN: left" class="title3"> SHIPPING & ORDER DETAILS</H4>--%>
   <h3 class="pad10-0" style="margin:0px;">Wes Check Out</h3>
<div class="grid12">
            <ul class="breadcrumb_wag">
            <li>
            <span class="aero">   Shipping / Delivery Details</span>
            </li>
            <li>
            <span class="aero currentpg">Payment Options</span>
            </li>
            <li>
            <span class="aero">Completed</span>
            </li>
            </ul>
</div>

<div class="grid12" runat="server" id="divCC" >
<div class="">
<div class="cl"></div>
          <span id ="r1"  > </span>  
 <div runat ="server" id="div2" class="redspan" style="font-size:12px;text-align:center;" >
            </div>

<div runat ="server" id="div1" >
          

             
      <fieldset>
          <legend>Payment</legend>
          
          <div class="ccforms" style="text-align:left;">

         <div class="form-col-1-8"> <span style="font-size:12px;float: left;margin-top: 2px;">Payment Type</span> </div>

          <div class="form-col-2-8">
           	  <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return creditcardclick()"  CausesValidation="false" ><label>
                     <img style="margin-top: -5px;cursor: pointer;" alt="cc" src="images/pay1uch.png"></label>
</asp:LinkButton>            </div>
              
            <div class="form-col-1-8">
               <%-- OnClick="btnPayPalPayLink_Click"--%>
           	    <asp:LinkButton ID="LinkButton2" runat="server" OnClientClick="return paypalclick()" CausesValidation="false" ><label>
                    
                            
                       <img style="margin-top: -5px;cursor: pointer;" alt="cc" src="images/pay2ch.png"></label></asp:LinkButton>
            </div>
                  <div class="form-col-1-8">
                        
                                                <asp:LinkButton ID="LinkButton3" runat="server" OnClientClick="return Defaultclick()"  CausesValidation="false"><label>
                                                      <img style="display:inline-block" src="images/bank-transfer_notcheck.png" alt=""/>
</asp:LinkButton>
                                            </div>

                <div id="divdedault" style="margin-top:20px;display:none;margin-top:10px"  runat="server">       
                            
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


             
               <div class="form-col-3-8">
                                                <h2 style="margin-top: 15px; padding-right: 35px; color: #0099DA; font-family: Arial; font-weight: bold; font-size: 26px; text-align: right">Amount: $ 
            <asp:Label runat="server" ID="lblAmount" Text="" CssClass="" />
                                                </h2>
                                            </div>
            <div class="cl"></div>
            <div class="form-col-8-8">
              <img style="margin-bottom:15px" alt="cc" src="images/paypal.png">
              <p class="para pad10-0">Pay using your Credit Card or Paypal Account.</p>
              <p class="para pad10-0">You wil be redirected to PayPal website to complete payment transaction.<br>
                </p>
              
            </div>
            <div class="cl"></div>
           

            <div class="form-col-8-8">   
              
                 
                 <asp:Button runat="server" ID="btnPay" Text="Pay Now" style="width:100px;" class="button normalsiz btngreen fleft"  OnClick="btnPay_Click" OnClientClick="Setinit()" />       


                 
                 <asp:Button runat="server" ID="BtnProgress" Text="Processing Payment. Please Wait…" style="display:none;visibility:visible;float:left;" class="button normalsiz btngreen fleft" Enabled="false"   />       
                 
                  
         <div id="divContent" style="font-size:12px margin-left:30px;color:Red" runat="server"  >
       </div>
            </div>
                  </div>

            <%--<div class="form-col-8-8">
              <p class="para pad10-0">You can review this order before it’s final.</p>
            </div>--%>
            <div class="cl"></div>
          </div>
          
          <div class="cl"></div>
        </fieldset>       
      

</div>
<fieldset>
 <legend>Shipping & Order Details</legend>
 <table  width="100%" border="0" cellpadding="0" cellspacing="0"  >         
         

                    <tr>
                     <td>
                     <fieldset>
<legend>Bill To</legend>
<p class="para pad10"> <asp:Label ID="lblDeliveryTo" runat="server" Text="Delivery Address" CssClass="LabelStyle"
            Font-Bold="false" style="font-weight:normal;"   ></asp:Label></p>
<div class="cl"></div>
</fieldset>
</td>
<td>
                     <fieldset>
<legend>Ship To</legend>
<p class="para pad10"> <asp:Label ID="lblShipTo" runat="server" Text="Shipping Address" CssClass="LabelStyle"
                Font-Bold="false"  style="font-weight:normal;"></asp:Label>
</p>
<div class="cl"></div>
</fieldset>
                     </td>
                    </tr>
                  


                    <tr>
                    <td colspan="2" >
<fieldset>
<legend>Order Contents</legend>
<div class="form-col-8-8">
<table width="100%" border="0" >
    
 <%--   <tr>
      <td width="100%" colspan="2" >
        
      </td>
    </tr>--%>
<%--    <tr>
        <td  width="100%" colspan="2"  align="left" >
        <H3 class="title1" style="TEXT-ALIGN: left">ORDER CONTENTS</H3>
            
        </td>
    </tr>--%>
    <tr>
        <td width="100%" colspan="2" >
            <table  width="100%" id="test1" border="0" cellpadding="3" cellspacing="0"  class="orderdettable" >
         
                <tr  class="" style="background-color:#BCD0E2;">
                    <td align="left" width="20%">
                        ORDER CODE
                    </td>
                    <td align="left" width="10%">
                        QTY
                    </td>
                    <td align="left" width="25%">
                        Description
                    </td>
                    <td  align="right" width="20%">
                        Cost(Ex. GST)
                    </td>
                    <td align="left" width="30%">
                        Extension Amount (Ex. GST)
                    </td>
                </tr> 
                <asp:Repeater ID="OrderitemdetailRepeater"   runat="server"  > 
                 
                    <ItemTemplate >
                        
                        <tr id="tRow"  runat="server"  class="rowOdd">
                        <td  id="TD1" runat="server" style="text-align:left;"><%# Eval("CATALOG_ITEM_NO")%></td>
                        <td style="text-align:left;"><%# Eval("QTY")%></td>
                        <td style="text-align:left;"> <%# Eval("DESCRIPTION")%></td>
                        <td style="text-align:right;"> $ <%# Convert.ToDecimal(Eval("PRICE_EXT_APPLIED")).ToString("#,#0.00")%></td>
                        <td style="text-align:right;"> $ <%# Convert.ToDecimal (Eval("TOTAL_EXT")).ToString("#,#0.00") %></td>                                       
                       </tr>  
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr id="tRow"  runat="server" class="">
                        <td  id="TD1" runat="server" style="text-align:left;"><%# Eval("CATALOG_ITEM_NO")%></td>
                        <td style="text-align:left;"><%# Eval("QTY")%></td>
                        <td style="text-align:left;"> <%# Eval("DESCRIPTION")%></td>
                        <td style="text-align:right;"> $ <%# Convert.ToDecimal(Eval("PRICE_EXT_APPLIED")).ToString("#,#0.00")%></td>
                        <td style="text-align:right;"> $ <%# Convert.ToDecimal (Eval("TOTAL_EXT")).ToString("#,#0.00") %></td>
                       </tr>  
                    </AlternatingItemTemplate>
               </asp:Repeater> 
         
       
               <tr style="background-color: white; font-size: 12px;">
                  <td align="left" width="50%"  colspan="3"   rowspan="4" valign="bottom">
                        <asp:Button ID="ImgBtnEditShipping" runat="server" Text="Edit / Update Order" style="float: left !important;font-size: 12px;
    text-shadow: none;display:none;" OnClick="ImgBtnEditShipping_Click" class="buttongray normalsiz btngray fleft" CausesValidation="false" />
                    </td>
 
                    <td  align="left" width="20%" class="">
                        <strong>Sub Total (Ex GST)</strong>
                    </td>
                  <td align="left" width="30%" class="" style="text-align:right;">
                     <strong>  $   <asp:Label runat="server" ID="Product_Total_price"/> </strong>
                  </td>
                </tr>
                <tr style="background-color: white; font-size: 12px;">
                        <%-- <td align="left" width="50%"  >
                        
                        </td>--%>

                        <td  align="left" width="20%" class="rowOdd">
                            <strong>Delivery / Handling Charge (Ex GST) </strong>
                        </td>
                        <td align="left" width="30%" class="rowOdd" style="text-align:right;">
                            <strong> $  <asp:Label runat="server" ID="lblCourier"/></strong>
                        </td>
                    </tr>
                 <tr style="background-color: white; font-size: 12px;">
                 <%-- <td align="left" width="50%"  >
                        
                    </td>--%>

                    <td  align="left" width="20%" class="rowOdd">
                        <strong>Total Tax Amount (GST) </strong>
                    </td>
                  <td align="left" width="30%" class="rowOdd" style="text-align:right;">
                      <strong> $  <asp:Label runat="server" ID="Tax_amount"/></strong>
                  </td>
                </tr>
                 
                 <tr style="background-color: white; font-size: 12px;">
                  <%--<td align="left" width="50%"  >
                        
                    </td>--%>

                    <td  align="left" width="20%" class="Rsucess">
                        <strong> <asp:Label runat="server" ID="lblTotalCap"/></strong>
	
                    </td>
                  <td align="left" width="30%" class="Rsucess" style="text-align:right;">
                    <strong>  $  <asp:Label runat="server" ID="Total_Amount"/></strong>
	 
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
<div class="grid12" runat="server" id="divTimeout" visible="false"    >
    <fieldset>
    <div style="text-align:center;padding:130px;" >
    <span style="font-size:21px;"  > Your session has timed out</span><br />
    <span style="font-size:14px;"> <a href="Login.aspx" class="para pad10-0" style="font-size:11px; color:#0033cc; font-weight:bold;">Click here</a> to log in again </span>
    </div>
    </fieldset>
</div>
 </div>

 </td>
 </tr>
 
 </table>
  
</asp:Content>
<%--<asp:Content ID="Content5" ContentPlaceHolderID="rightnav" runat="server">
</asp:Content>--%>
<asp:Content ID="Content6" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
