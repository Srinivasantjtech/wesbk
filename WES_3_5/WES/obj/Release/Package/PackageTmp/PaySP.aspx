<%@ Page Title="" Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" CodeBehind="PaySP.aspx.cs" Inherits="PaySP"  Culture="en-US" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script language="JavaScript">
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
  <script type="text/ecmascript">
      function ValidCC(sender, args) {

          var cardno = '';
          var CardType = 0;
          AEcardno = /^(?:3[47][0-9]{13})$/;
          Mcardno = /^(?:5[1-5][0-9]{14})$/;
          Vcardno = /^(?:4[0-9]{12}(?:[0-9]{3})?)$/;              

          var dd = document.getElementById('<%= drppaymentmethod.ClientID %>');
          if (dd != null) {
              CardType = dd.value;
          }
          if (CardType == 2) //Amercican Express	   
              cardno = /^(?:3[47][0-9]{13})$/;
          else if (CardType == 5)//Mastercard 	   
              cardno = /^(?:5[1-5][0-9]{14})$/;
          else if (CardType == 6) //Visa	   
              cardno = /^(?:4[0-9]{12}(?:[0-9]{3})?)$/;              
          else
              return args.IsValid = false;

          var txt = document.getElementById('<%= txtCardNumber.ClientID %>');

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
          var ddl = document.getElementById('<%= drppaymentmethod.ClientID %>');
         if (ddl != null) {

             var opts = ddl.options.length;
             for (var i = 0; i < opts; i++) {
                 if (ddl.options[i].value == dval) {
                     ddl.options[i].selected = true;
                     break;
                 }
             }
         }
         
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
        var res = Page_ClientValidate();
        if (res == true) {
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
        else {
            Controlvalidate('dd');
            Controlvalidate('cno');
            Controlvalidate('cn');
            Controlvalidate('cvv');
        }
        
    }
    function Controlvalidate(ctype) {
        if (ctype == "dd") {
            var dd = document.getElementById('<%= drppaymentmethod.ClientID %>');
            if (dd != null && dd.value == 0) {

                dd.style.border = "1px solid #FF0000";
            }
            else {
                dd.style.border = "";

            }
        }
        if (ctype == "cno") {
            var cno = document.getElementById('<%= txtCardNumber.ClientID %>');
            if (cno != null && cno.value == "") {
                cno.style.border = "1px solid #FF0000";
            }
            else {
                cno.style.border = "";
            }
        }
        if (ctype == "cn") {
            var cn = document.getElementById('<%= txtCardName.ClientID %>');
            if (cn != null && cn.value == "") {

                cn.style.border = "1px solid #FF0000";
            }
            else {
                cn.style.border = "";
            }
        }
        if (ctype == "cvv") {
            var cvv = document.getElementById('<%= txtCardCVVNumber.ClientID %>');
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


    <table  width="100%" border="0" 
    align="left">
    <tr>
    <td>
    
   <div id="page-wrap">

   <h3 class="pad10-0" style="margin:0px;">Wes Check Out</h3>
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

<div class="grid12" runat="server" id="divCC"  >
<div class="">
<div class="cl"></div>
   <span id ="r1"  > </span>
 <div runat ="server" id="div2"  class="alert yellowbox icon_4" style="background-color:#FFD52B;height: 33px;padding-top: 26px;">
            </div>
<div runat ="server" id="div1" >
                 <fieldset>                 
          <legend>Payment</legend>
          <div runat ="server" id="div3" >
          <div class="ccforms" style="text-align:left;">

               <div class="form-col-1-8"> <span style="font-size:12px;float: left;margin-top: 2px;">Payment Type</span> </div>

            <div class="form-col-2-8">
           	  <asp:LinkButton ID="LinkButton1" runat="server" OnClick="btnSecurePayLink_Click"  CausesValidation="false" ><label><img style="margin-top: -5px;cursor: pointer;" alt="cc" src="images/pay1ch.png"></label>
</asp:LinkButton>            </div>
              
            <div class="form-col-3-8">
           	    <asp:LinkButton ID="LinkButton2" runat="server" OnClick="btnPayPalPayLink_Click" CausesValidation="false" ><label><img style="margin-top: -5px;cursor: pointer;" alt="cc" src="images/pay2uch.png"></label>
</asp:LinkButton>            </div>
            <div class="cl"></div>
            <div class="form-col-8-8"><p></p>
            </div>
            <div class="cl"></div>

            <div class="form-col-2-8">
            <asp:Label runat="server" ID="Label2"  style="font-size:12px;" >Amount &nbsp;&nbsp;<span class="redspan">*</span>     </asp:Label>
            </div>
            <div class="form-col-2-8">
             $<asp:Label runat="server" ID="lblAmount" style="font-size:12px;" Text="" Width="150px"/> 
            </div>
             <div class="cl"></div>
             <div class="form-col-2-8">
             <asp:Label runat="server" ID="lblpaymentmethod" style="font-size:12px;" >Card Type &nbsp;&nbsp;<span class="redspan">*</span> </asp:Label>    
             </div>
             <div class="form-col-2-8">
              
              <asp:DropDownList ID="drppaymentmethod" runat="server" width="200px" CssClass="cardinput" onchange="Controlvalidate('dd')"  />     
             </div>
             <div class="form-col-2-8">
              
               <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"  InitialValue="0"
                    ControlToValidate="drppaymentmethod" Display="Dynamic" CssClass="error-text" ErrorMessage="Select Card Type" 
                    ></asp:RequiredFieldValidator>     
             </div>
             <div class="cl"></div>
              <div class="form-col-2-8">
                <asp:Label runat="server" ID="lblcardnumber" style="font-size:12px;"   >Card Number &nbsp;&nbsp;<span class="redspan">*</span> </asp:Label>
              </div>
              <div class="form-col-2-8">
               <asp:TextBox runat="server" ID="txtCardNumber" CssClass="cardinput" width="192px"  MaxLength="19" OnBlur="Controlvalidate('cno')" />
              </div>
              <div class="form-col-2-8">
               <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ControlToValidate="txtCardNumber" Display="Dynamic" CssClass="error-text" ErrorMessage="Enter Card Number"  ></asp:RequiredFieldValidator>
                    
                    <asp:customvalidator id="CustomValidator1"   Display="Dynamic"  CssClass="error-text" ClientValidationFunction="ValidCC" errormessage="Please Check Credit Card Number" controltovalidate="txtCardNumber" runat="server">
	             </asp:customvalidator>
              </div>
               <div class="cl"></div>
                <div class="form-col-2-8">
                 <asp:Label runat="server" ID="Label1" style="font-size:12px;" >Name on Card &nbsp;&nbsp;<span class="redspan">*</span> </asp:Label>
                </div>
                <div class="form-col-2-8">
                  <asp:TextBox runat="server" ID="txtCardName"  CssClass="cardinput" width="192px"   MaxLength="50"  OnBlur="Controlvalidate('cn')" />
                </div>
                <div class="form-col-2-8">
                 <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                    ControlToValidate="txtCardName" Display="Dynamic" CssClass="error-text" ErrorMessage="Enter Name on Card"></asp:RequiredFieldValidator>

                </div>
                 <div class="cl"></div>

                  <div class="form-col-2-8">
                   <asp:Label runat="server" ID="lblexpirationdate"  style="font-size:12px;" >Expiration &nbsp;&nbsp;<span class="redspan">*</span> </asp:Label>
                </div>
                <div class="form-col-1-8">
                <asp:DropDownList NAME="drpExpmonth"  ID="drpExpmonth" runat="server"  CssClass="cardinput" style="width: 100%;">           
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

                    
                </div>
                <div class="form-col-1-8">
                <asp:DropDownList NAME="drpExpyear"  ID="drpExpyear" runat="server" CssClass="cardinput" style="width: 100%;">          
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
                 <asp:Label runat="server" ID="lblcardcvvnumber" style="font-size:12px;"  >Card Security Code &nbsp;&nbsp;<span class="redspan">*</span> </asp:Label>
                </div>
                <div class="form-col-2-8">
                  <asp:TextBox runat="server" ID="txtCardCVVNumber" Width="100px" CssClass="cardinput" MaxLength="3" OnBlur="Controlvalidate('cvv')"/>         
                </div>
                <div class="form-col-3-8">
             
                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="txtCardCVVNumber" Display="Dynamic" CssClass="error-text" 
                    ErrorMessage="Enter Card Security Code<Br/>"></asp:RequiredFieldValidator>                    
                           <%--<div class="cvvpaypopup">
                <div class="cvvpopup">
                  <div class="cvvpopupouterdiv1">
                     <div class="cvvpopupaero">              
                    </div>
                    <h1>How to find the security code on a credit card</h1>
                    <p>Find out where to locate the security code on your credit card.</p>
                    <p><strong> Visa, MasterCard, Discover, JCB, and Diners Club</strong></p>
                       <p>     The security code is a three-digit number on the back of your credit card, immediately following your main card number.</p>
                        <img src="images/Mcard.jpg">
                        <p><strong>American Express</strong></p>
                        <p>The security code is a four-digit number located on the front of your credit card, to the right above your main credit card number.</p>
                        <img src="images/Acard.jpg">
                        <p>If your security code is missing or illegible, call the bank or credit card establishment referenced on your card for assistance.</p>
                    </div>
                     <span style="font-size: 10px;color: #0033CC; float: left;"><a class="paypopup" style="cursor:pointer;"><img style="cursor: pointer;" alt="help" src="images/question_blue.png"> 
                     <span style="font-size: 10px;color: #0033CC;">&nbsp;&nbsp; A code that is printed (not imprinted) on the back of <br />
                    &nbsp;&nbsp; a credit card. It consist of 3 or 4 digits.</span></a></span>
                      </div> 
              </div>--%>

              <span style="font-size: 10px;color: #0033CC; float: left;">  
              <asp:LinkButton ID="myLink" CssClass="modal" style="color: #01AEF0;text-decoration: none;"  runat="server"  >
              <img style="cursor: pointer;" alt="help" src="images/question_blue.png"/> 
                     <span style="font-size: 10px;color: #0033CC;">&nbsp;Where to find Security Code? Code is located on either the<br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Front / Back of your credit and consists of 3 or 4 digits</span></asp:LinkButton></span>
                    <asp:ModalPopupExtender ID="TACpopup" PopupControlID="pnlTAC" BackgroundCssClass="modalBackgroundpopup"  BehaviorID="testTACpopup"
    DropShadow="true" runat="server" TargetControlID="myLink" RepositionMode="None">
</asp:ModalPopupExtender>
                </div>
                
                  <div class="cl"></div>
      
                
            <div class="cl"></div>
            <div class="form-col-8-8">              
                <asp:Button runat="server" ID="btnPay" Text="Pay Now" style="width:100px;" class="button normalsiz btngreen fleft"  OnClick="btnSecurePay_Click" OnClientClick="javascript:Setinit()" />       

                 

                 <asp:Button runat="server" ID="BtnProgress" Text="Processing Payment. Please Wait…" style="display:none;visibility:visible;float:left;" class="button normalsiz btngreen fleft" Enabled="false"   />       

                 
                 
                 
                  <%--CssClass="btn1 btn1-success1" ImageUrl="~/images/SecurePayPaynow.jpg"   style=" width:130px;margin-left: 0px;padding-left: 0px;" --%>
         <div id="div4" style="font-size:12px margin-left:30px;color:Red" runat="server"  >
       </div>
            </div>
          
            <div class="cl"></div>
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
                  <td align="left" width="50%"  colspan="3"   rowspan="4" valign="bottom" >
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

<div class="grid12" runat="server" id="divTimeout"  visible="false"    >
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

 <asp:Panel ID="pnlTAC" runat="server" Width="650px"  Style="display:none; "    >
<a href = "javascript:Hidepopup()" class="testbutton" ></a>
<div class="boxfull" style="width:575px;height:500px;text-align:left;">

  <h1>How to find the security code on a credit card</h1>
    <p>Find out where to locate the security code on your credit card.</p>
    <p><strong> Visa, MasterCard, Discover, JCB, and Diners Club</strong></p>
        <p> The security code is a three-digit number on the back of your credit card, immediately following your main card number.</p>
        <img src="images/Mcard.jpg"/>
        <p><strong>American Express</strong></p>
        <p>The security code is a four-digit number located on the front of your credit card, to the right above your main credit card number.</p>
        <img src="images/Acard.jpg"/>
        <p>If your security code is missing or illegible, call the bank or credit card establishment referenced on your card for assistance.</p>
</div>
</asp:Panel>

 

</asp:Content>
<%--<asp:Content ID="Content5" ContentPlaceHolderID="rightnav" runat="server">
</asp:Content>--%>
<asp:Content ID="Content6" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
