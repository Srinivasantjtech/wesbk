<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Mainpage.master" CodeBehind="PayOnline_International.aspx.cs" Inherits="WES.PayOnline_International" %>

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
       
    </style>
     <script type="text/javascript">        window.history.forward(1);

         window.location.hash = "no-back-button";
         window.location.hash = "Again-No-back-button"; //again because google chrome don't insert first hash into history
         window.onhashchange = function () { window.location.hash = "no-back-button"; }
     </script>
<script language="javascript" type="text/javascript">

    function Setinit() {
       
            var x = document.getElementById('<%= btnSP.ClientID %>');
                var y = document.getElementById('<%= BtnProgressSP.ClientID %>');
             
                x.style.display = "none";
                y.style.display = "block";
                y.style.visibility = "visible";
              
           
              document.getElementById("Def").style.display = "none";

        }
    

    var message = "Right click not allowed this page!";

    function creditcardclick() {
        document.getElementById("ctl00_maincontent_divdedault").style.display = 'none';
        document.getElementById("ctl00_maincontent_creditflag2").style.display = 'block';
        document.getElementById("ctl00_maincontent_divpaypal").style.display = 'none';
   
   

        $("#lblcreditcard").addClass("active");
        $("#lbldefaultpayment").removeClass("active");
        $("#lblpaypalcard").removeClass("active");

    }
    function paypalclick() {
      
        document.getElementById("ctl00_maincontent_creditflag2").style.display = 'none';
        document.getElementById("ctl00_maincontent_divpaypal").style.display = 'block';
     
        document.getElementById("ctl00_maincontent_divdedault").style.display = 'none';

        $("#lblcreditcard").removeClass("active");
        $("#lbldefaultpayment").removeClass("active");
        $("#lblpaypalcard").addClass("active");

    }

    function Defaultclick() {
        document.getElementById("ctl00_maincontent_divdedault").style.display = 'block';
        document.getElementById("ctl00_maincontent_creditflag2").style.display = 'none';
        document.getElementById("ctl00_maincontent_divpaypal").style.display = 'none';
      
       

        $("#creditcardclick").removeClass("active");
        $("#lbldefaultpayment").addClass("active");
        $("#paypalclick").removeClass("active");
    }


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
</script>

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
       <div id="divonlinesubmitordererror"  visible="false" runat="server" aria-hidden="false" aria-labelledby="myLargeModalLabel" role="dialog" tabindex="-1" style=" display: block;" class="modal fade bs-example-modal-lg in" >
    <div class="modal-dialog modal-lg">
    <div class="modal-content">

        <div class="modal-header blue_color padding_top padding_btm" >

          <h4 id="H1" class=" white_color font_weight modal-title">Invalid Login Details for Checkout! </h4>
         
        </div>
        <div class="modal-body">

     
         <p>   
                      
                         User Id does not match with online submit order.
                        <br /> <a Href="/logout.aspx" style="color:#15c;" >Please Click Here</a></p>
        

        </div>
      </div>
  </div> 
</div>
     <div id="divmaincontent"  runat="server">
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
<%--          <div class="alert greenbox icon_2" style="padding:30px 10px 30px 60px" runat ="server"  id="greenalert" visible ="false">
                         <h3 style="font-size: 16px;">Your order has been successfully submitted to us for processing by Bank Transfer.Thank You!</h3>
                   </div>--%>


    <div id="divpayonacccontent" runat="server">

            <div class="payment_contents" id="div4" runat="server" style="padding-bottom:20px; float:left; width:50%; display:block;">
                      <h3 style="color:#0099DA; padding-right:22px; font-size: 15px;font-weight:bold  ">Payment Required</h3>
                                     <p style="font-size: 13px;line-height: 1.5;padding-right: 45px;color:#555">
Your order is on hold awaiting payment<br/>
                                      Thank you for your recent order.Payment is required prior to shipping.<br />
                                         Please trasfer funds as per details shown on the right.
                                   </p> 
  <%--<asp:Button runat="server" ID="btndirectdeposit" Text="SUBMIT ORDER" style="margin:10px 32px 10px 10px;float:right" class="normalsiz paynow"  OnClientClick="return checkorderid()" OnClick="ImageButton4_Click" /> --%>
                  
                     </div>
                     
                     
                     
                     <!-- -------  Bank Transfer Div ----------------- --> 
                     <div class="trans_detail" style="padding-bottom:20px; float:left; width:40%; display:block;">
                     	<div class="r_box" style="width:350px;  display:block; border:5px solid red; padding:20px; text-align:left; font-family:Arial;">
                        	<h3 style="color:red; margin:0;">Bank Transfer Details</h3>
                            <p style=" font-size:13px;">
                            	<b>Total Amount :</b>$ 
                                     
                                        <asp:Label ID="lblbttotalamount" runat="server" Text="" CssClass="LabelStyle"  ></asp:Label> <br>
                                <b>Payment Reference :</b> WES-  <asp:Label ID="lblorderid" runat="server" Text="" CssClass="LabelStyle"  ></asp:Label> <br>
                                <b>BSB :</b> 062-105 <br>
                                <b>Account No :</b> 1006-4018 <br>
                                <b>Account Name :</b> WES Components Pty Ltd <br>
                                <b>Swift / BIC Code :</b> CTBAAU2S  <br><br>
                            </p>
                            
                            <p style=" font-size:12px; margin-bottom:0px;">
                            	Important. <br>
                                Please include the Shown Payment Reference <br> 
                                as your payment description for fast processing.
                            </p>
                        </div>
                     </div>
                     



    </div>
<div class="grid12" runat="server" id="divCC" >
<div class="">
<div class="cl"></div>
          <span id ="r1"  > </span>  
 <div runat ="server" id="div2" class="redspan" style="font-size:12px;text-align:center;" >
            </div>
   <div runat="server" id="divgobr" class="alert yellowbox icon_4" style=" height: 25px; padding-top: 26px;font-size:small;display:none ">
                            </div>
<div runat ="server" id="div1" >
          
    <fieldset>
  <legend>Payment</legend>

                     <div class=" payment_btns" style="margin-top:5px">
                           <label style="float:left; width:146px;font-size:12px;margin-left:9px">Payment Type</label>
                                <label for="contactChoice1" class="form-col-2-8 payment_radio " id="lbldefaultpayment" runat="server" visible="false" >
                                      <asp:RadioButton ID="RBdefautpaymenttype" runat="server" onclick="Defaultclick()"  Text="Bank Transfer" GroupName="Paymentoption"/>
                              <%--  <input id="contactChoice1" name="contact" value="email" type="radio">--%>
                               
                               <%--     <div runat="server" style="display:none" id="divmastercard">
                                        <span style="height: 37px; display: inline-block;">
  <img style="display:inline-block" src="../images/master-card.png" alt=""/>
                                        </span>
                                      <span style="display: inline-block; height: 30px; vertical-align: middle;">
  <asp:Label ID="lblmastercardno" runat="server" Text="Label" style="height: 15px; display: block;" ></asp:Label>
<asp:Label ID="lblmasterexpirydate" runat="server" Text="Label" style="height: 15px; display: block;"></asp:Label>
                                          </span>
                                        
                                    </div>--%>
                                      <div runat="server" id="divdirectdeposit">
                                        <img style="display:inline-block" src="../images/bank-transfer.png" alt=""/>
   
                                    </div>
                                   <%--  <div runat="server" id="divpayoaccount" style="display:none">
                                        <img style="display:inline-block" src="../images/charge-account.png" alt=""/>
   
                                    </div>--%>
                                </label>
               
                                
                                <label for="contactChoice2" class="form-col-2-8 payment_radio"  id="lblcreditcard" runat="server">
                                     <asp:RadioButton ID="RBCreditCard" runat="server" onclick="creditcardclick()" Text="" GroupName="Paymentoption" ValidationGroup="payonlinepaynow" style="float:left" />
                           
                                
                                <div style="display:inline-block;float:left;margin: -25px 25px ">
                                    <img src="../images/cards_sm.png" alt=""  runat="server" id="imgsecurepay"/></div>
                                </label>
                                
                                <label for="contactChoice3" class="form-col-1-8 payment_radio" id="lblpaypalcard"  runat="server" visible="false" style="height:45px">
                                <asp:RadioButton ID="RBPaypal" runat="server"  onclick="paypalclick()" Text="" GroupName="Paymentoption" style="float:left"/>
                                
                                <div style="float:left;margin-top:-5px"><img src="../images/paypal_pay.png" alt=""/></div>
                                </label> 
                             <div class="form-col-3-8" style="float:right">
                                	<%--<p style="margin-top:2px; margin-bottom:8px; text-align:right;padding-right:35px; font-size:13px;">Total Amount: </p>--%>
                                    <h2 style="margin-top:5px;margin-right:50px;color:#0099DA; font-family:Arial;font-weight: bold;font-size: 26px;text-align:right ">
                                       
                                        Amount: $ 
                                     
                                        <asp:Label ID="lbltotalamt" runat="server" Text="" CssClass="LabelStyle"  ></asp:Label>
                                    </h2>
                                </div>
                                <div class="clear"></div>
                            </div>
                
                            <div runat="server" id="divpayonacc" style="display:none">

                                        <img style="display:inline-block" src="../images/charge-account.png" alt=""/>
                                        </div>


                                   <div id="divdedault" style="margin-top:20px "  runat="server">
                                   
                            
<%--                              <div class="payment_contents"  id="divpayonacccontent"  runat="server"  style="padding-bottom:20px">
                      <h2 style="color:#0099DA; padding-right:22px; font-size: 13px;  ">Pay By Bank Transfer</h2>
                                     <p style="font-size: 13px;line-height: 1.5;padding-right: 153px;color:#555">
We will email you a proforma invoice with bank trasfer details once your order has been submitted.</br>
                                      When sending funds please include the Incoice No as your payment reference for faster processing 
                                      and notify our accounts department by email  <a style="color:#0099DA;" href="mailto:accounts@wes.net.au">accounts@wes.net.au </a>
                                      with your transfer receipt.
                                   </p> 
  <asp:Button runat="server" ID="btndirectdeposit" Text="SUBMIT ORDER" style="margin:10px 32px 10px 10px;float:right" class="normalsiz paynow"  OnClientClick="return checkorderid()" OnClick="ImageButton4_Click" />
                                   <div class="clear"></div>
                                               </div>--%>
                                             <%--  <div class="payment_contents"  id="divD2"  runat="server"  style="padding-bottom:20px" >
                                                      <h2 style="color:#0099DA; padding-right:22px; font-size:13px; "> Charge Account</h2>
                                                  
                                                   <p>Your Account will be billed for Invoice </p> 
  <asp:Button runat="server" ID="btnpayonacc" Text="SUBMIT ORDER" style="margin:10px 32px 10px 10px;float:right" class="normalsiz paynow"  OnClientClick="return checkorderid()" OnClick="ImageButton4_Click" />
                     <div class ="clearfix"> </div> 
                                               </div>
                                       <div class="payment_contents"  id="divD3"   runat="server"  style="padding-bottom:20px" >
  <asp:Button runat="server" ID="btnmastercard" Text="SUBMIT ORDER" style="margin:10px 32px 10px 10px;" class="normalsiz paynow"  OnClientClick="return checkorderid()" OnClick="ImageButton4_Click" />
                       <div class="clear"></div>
                                               </div>--%>
                               
                                       </div>
                                   
        <div   id="creditflag1" runat="server" style="display:none" >
                            <div class="payment_contents" runat="server" id="divcreditcard" >
                            <div class="form-col-6-8">
                            	<div class="mastercard_pay"></div>
                                
                                
                                <div class="creditcard_pay" >
                                	<div>
                                	<label class="pform_title" style="display:inline-block; width:146px;">Credit Card Number</label>
<asp:TextBox ID="txtcreditcardno" runat="server" class="payment_input" style="width:368px; display:inline-block;" MaxLength="19" ValidationGroup="payonlinepaynow" >

                                </asp:TextBox>
                                      <div style="margin-left:146px; margin-bottom:5px">  <asp:RequiredFieldValidator ID="RBcreditcardno" runat="server"  class="vldRequiredSkin_creditcard" ErrorMessage="Please Enter Card No" ControlToValidate ="txtcreditcardno" ValidationGroup="payonlinepaynow"></asp:RequiredFieldValidator> </div>

                                    <div class="clear"></div>
                                    </div>

                                    <div class="clear"></div>

                                    <div>
                                        <div style="width:246px; float:left;">
                                    	<label class="pform_title" style="display:inline-block; width:146px;">CVV</label>
                                        <asp:TextBox ID="txtCVV" runat="server" class="payment_input" style="width:80px; display:inline-block;"  MaxLength="4" ValidationGroup="payonlinepaynow" ></asp:TextBox>
                                    	<div style="margin-left:146px; margin-bottom:5px">  <asp:RequiredFieldValidator ID="RBCVV" runat="server"  class="vldRequiredSkin_creditcard" ErrorMessage="Please Enter CVV" ControlToValidate ="txtCVV" ValidationGroup="payonlinepaynow"></asp:RequiredFieldValidator></div>
                                      
                                    </div>
                                    <div style="width:320px; float:left;">
                                 
                                    	<label class="pform_title" style="width:90px; display:inline-block;">Expiry Date</label>
                                             <asp:DropDownList NAME="drpExpmonth"  ID="drpExpmonth" runat="server"   class="payment_select" style="width:80px; display:inline-block;">           
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
                    <asp:DropDownList NAME="drpExpyear"  ID="drpExpyear" runat="server" class="payment_select" style="width:80px; display:inline-block;">          
                    </asp:DropDownList>
                                       <div style="margin-left:146px; margin-bottom:5px"><asp:RequiredFieldValidator ID="RBexpirydate" runat="server"   class="vldRequiredSkin_creditcard" ErrorMessage="Please Enter Expiry Date" ControlToValidate ="drpExpyear" ValidationGroup="payonlinepaynow"></asp:RequiredFieldValidator></div>
                                         </div>

                                         <div class="clear"></div>
                                    </div>
                                    
                                    
                                    <div class="clear"></div>
                             
                                    </div>
                                    <div class="clear"></div>
                                    
                                    <div>
                                    <label class="pform_title" style="width:146px; display:inline-block;">Name On Card</label>
                                          <asp:TextBox ID="txtnamecard" runat="server" class="payment_input" style="width:368px; display:inline-block;" MaxLength="150" ValidationGroup="payonlinepaynow"></asp:TextBox>
                                          <div style="margin-left:146px; margin-bottom:5px"><asp:RequiredFieldValidator ID="RBCardname" runat="server"  class="vldRequiredSkin_creditcard" ErrorMessage="Please Enter Name On Card" ControlToValidate ="txtnamecard" ValidationGroup="payonlinepaynow"></asp:RequiredFieldValidator></div>
                                    </div>
                                    
                                    <div>
                                            <asp:Button ID="btnSP" runat="server" Text="Pay Now"  ValidationGroup="payonlinepaynow" class="paynow" OnClick="btnSecurePay_Click" style="margin-right:388px;width:20%;font-size:12px"  OnClientClick="javascript:Setinit()"/>
                                    <%--    <asp:Button runat="server" ID="BtnProgressSP" Text="Processing Payment. Please Wait…" style="display:none;visibility:visible;float:left; " class="btn btn-primary" Enabled="false"   />     --%>
                  <%--<asp:Button runat="server" ID="BtnProgressSP" Text="Processing Payment. Please Wait…" Style="display: none; visibility: visible; float: left;" class="button normalsiz btngreen fleft"  Enabled="false"  />--%>
                                                        <asp:Image ID="BtnProgressSP" runat="server" style="display:none;float:left;" ImageUrl="../images/Processing_Payment.png" />
                                    </div>
                                    
                                    <div class="clear"></div>
                                </div>
                                  <div id="divContent" style="font-size:12px margin-left:30px;color:Red" runat="server"></div>
                                
                                
                                <div class="paypal_pay"></div>
                                
                                
                                <div class="clear"></div>
                            </div> 
                           
        </div>
        <div id="creditflag2" runat="server">
                                                    <div id="dropin-container" style="width:">
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
        <div class="payment_contents form-col-8-8"  id="divpaypal" runat="server" style="display:none">
                                              <div class="payment_contents creditcard_pay">
                                                    <img style="display:inline-block" src="../images/paypal.png" alt=""/>
                                                   <div id="paypalpaybtn" class="form-col-4-8" style="float:right;margin-top:20px">
                                                          <asp:Button runat="server" ID="btnPay" Text="Pay Now" class="button normalsiz btngreen" OnClick="btnPay_Click"
                                                              OnClientClick="return checkorderid()" Style="margin-right: 45px;width:100px;float:right"/>
                                                          <asp:Button runat="server" ID="BtnProgress" Text="Processing Payment. Please Wait…"
                                                              Style="display: none; visibility: visible; float: left;" class="btn btn-primary "
                                                              Enabled="false" />
                                                      </div>
                          <p style="margin-top:8px;margin-bottom:40px;font-size:12px;line-height:2" >
                              Pay using your Credit Card or PayPal Account </br>
                              You will be redirected to PayPal website to complete payment transaction.</p>
                               <div class="col-sm-20 nolftpadd">
                                <div class="form-group col-lg-8 nolftpadd">
                                 <h3 class="green_clr"><asp:Label runat="server" ID="lblpaypaltotamt" CssClass="totalamt" Visible="false" /> 
                                </h3>           
                                </div>
                            </div>

            <%--  <asp:Button runat="server" ID="btnPay" Text="Pay Now" class="paynow" OnClick="btnPay_Click" OnClientClick="return checkorderid()"  style="margin-right:28px " />       

                  

                 
                 <asp:Button runat="server" ID="BtnProgress" Text="Processing Payment. Please Wait…" style="display:none;visibility:visible;float:left;" class="btn btn-primary " Enabled="false"   />  --%>     
  <%--                <input id="btnpaypal" value="Pay Now" class="paynow" type="button" runat="server" />--%>
 <div class="clear"></div>
</div>

                                          </div>
</fieldset>
             
      <fieldset>
          <legend>Order Details</legend>

       
          
                             <div class="quickorder4" id="divpaymentoption" runat="server">
            <table id="ctl00_maincontent_Table3" border="0" cellpadding="0" cellspacing="0" width="100%">
            <tbody>
            <tr>
                <td align="left" width="100%">
                        <table id="ctl00_maincontent_colo2" style="border-style: none" border="0" cellpadding="1" cellspacing="0" width="100%">
                    <tbody>
                    <%--<tr>
                        <td colspan="2" style="font-size:16px; color: #0099DA;">
                           <div class="form-col-8-8" >
                              <h4 class="padbot10">Payment Options</h4>
                           </div>
                           
                            
                        </td>
                    </tr>--%>
                
                    
                    <tr>
                        <td rowspan="" width="100%">
                        
                             <p class="para pad10">Order No.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;: <asp:Label ID="lblOrderNo" runat="server" Text="" CssClass="LabelStyle" style="font-weight:normal;" ></asp:Label></p>
                             <p class="para pad10">Shipping Method : <asp:Label ID="lblShippingMethod" runat="server" Text="" CssClass="LabelStyle" style="font-weight:normal;" ></asp:Label></p>
     
                              <div class="clear"></div>
                                <div runat ="server" id="div3" class="accordion_head_yellow gray_40" style="font-size:12px;text-align:center; font-weight:bold;margin-bottom:12px;background: #FFF200; padding: 12px 17px;" visible ="false" >
                </div>
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
                        <tr id="tR1"  runat="server" class="">
                        <td  id="TD2" runat="server" style="text-align:left;"><%# Eval("CATALOG_ITEM_NO")%></td>
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
        </div>
 </td>
 </tr>
 
 </table>
  
         
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

      
            var payBtn = document.getElementById('pay-btn');
var nonceGroup = document.querySelector('.nonce-group');
var nonceInput = document.querySelector('.nonce-group input');
var payGroup = document.querySelector('.pay-group');
         var dropin;
         var clienttoken = "<%= this.ClientToken %>";
        var totamt = document.getElementById('ctl00_maincontent_lbltotalamt');
                 //   alert(totamt);
         var amt1 = totamt.innerHTML.replace('<b>', '').replace('</b>', '').replace('$', '');
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
                 try {
                     document.getElementById("ctl00_maincontent_lblpaypalcard").style.display = "none";
                 }
                 catch (error) { }
                 document.getElementById("ctl00_maincontent_divdedault").style.display = "none";
                   document.getElementById("ctl00_maincontent_divpaypal").style.display = 'none';
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
                          try {
                     document.getElementById("ctl00_maincontent_lblpaypalcard").style.display = "inline-block";
                 }
                 catch (error) { }
                         
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
                            try {
                     document.getElementById("ctl00_maincontent_lblpaypalcard").style.display = "inline-block";
                 }
                 catch (error) { }
                        
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
                     document.getElementById('ctl00_maincontent_divgobr').style.display = "none";
                   document.getElementById('ctl00_maincontent_divgobr').innerHTML = "";
                    var totamt = document.getElementById('ctl00_maincontent_lbltotalamt');
                   // alert(totamt);
         var amt1 = totamt.innerHTML.replace('<b>', '').replace('</b>', '').replace('$', '');
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

