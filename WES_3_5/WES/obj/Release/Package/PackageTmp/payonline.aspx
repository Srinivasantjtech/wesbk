<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="payonline.aspx.cs" Inherits="payonline" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Payonline</title>
      <link href="css/stilos_.css" rel="stylesheet" type="text/css"/> 
<script language=JavaScript>
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
</script>
<script  type="text/javascript">
    function GetPay(Param) {
        var CCType = document.getElementById('<%= drppaymentmethod.ClientID %>').value;
        var ccNo = document.getElementById('<%= txtCardNumber.ClientID %>').value;
        var ccmm = document.getElementById('<%= lblexpirationdate.ClientID %>').value;
        var ccyy = document.getElementById('<%= drpExpyear.ClientID %>').value;
        var cccvv = document.getElementById('<%= txtCardCVVNumber.ClientID %>').value;
        Param = Param+ "#####" + CCType + "#####" + ccNo + "#####" + ccmm + "#####" + ccyy + "#####" + cccvv;
        alert(Param);
        if (Param != "") {
            $.ajax({
                type: "POST",
                url: "GblWebMethods.aspx/GetpayResult",
                data: '{"param":"' + Param + '"}',
                contentType: "application/json",
                dataType: "json",
                success: OnajaxSuccess,
                error: OnajaxFailure
            });
        }
        else {
            var id = document.getElementById("Div1");
            var id1 = document.getElementById("Div2");            
            displayDiv(id, id1, "block", "none");
        }
    }



    function displayDiv(Divid, Divid1, dis,dis1) {
        Divid.style.display = dis;
        Divid1.style.display = dis1;
    }

    function OnajaxSuccess(result) {

        var id = document.getElementById("Div1");
        var id1 = document.getElementById("Div2");
        if (result.d != "") {
                        
            id1.innerHTML = result.d;
            id.innerHTML = "";
            displayDiv(id, id1, "none", "block");
        }
        else {
            
            id.innerHTML = "";
            id1.innerHTML = "Error try again";
            displayDiv(id, id1, "none", "block");
        }


    }
    function OnajaxFailure(result) {

        id.innerHTML = "";
        id1.innerHTML = "Error try again";
        displayDiv(id, id1, "none", "block");
    }
    
  </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ScriptManager>

    <div>
    <table id="tblConfirm" width="100%" border="0px" cellpadding="3" cellspacing="0" style="border-collapse: collapse">
       <tr>
                        <td width="50%">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0" style="border-collapse: collapse">
                                <tr>
                                    <td width="35%" align="justify">
                                        <img src="Images/WesLogo.jpg" id="imgLogo" runat="server" height="85" />
                                    </td>
                                    <td width="10%">
                                        &nbsp;
                                    </td>
                                    <td width="55%">
                                        <p align="left" class="TitleColumnStyle">
                                            WES Australiasia
                                            <br />
                                            138 Liverpool Road,
                                            <br />
                                            Ashfield, NSW, 2131
                                            <br />
                                            Phone: 9797 9866
                                            <br />
                                            Fax: 9716 6015
                                            <br />
                                            Email: sales@wes.net.au
                                        </p>
                                    </td>
                                </tr>
                            </table>
                           
                        </td>
                        <td width="50%">
                            
                        </td>
                        
       </tr>
    </table>
     <table id="tblBase" width="650px" border="0" cellpadding="3" cellspacing="0" style="border-collapse: collapse"
    align="center">
    <tr>
        <td width="100%" bgcolor="#0092c8" style="font-family: Arial; font-size: small; font-weight: bold;
            color: White;">
            ORDER DETAILS
        </td>
    </tr>
    <tr>
        <td width="100%">
            <table width="100%" cellpadding="4" cellspacing="" border="0" style="font-family: Arial;
                font-size: small; font-weight: normal; border-collapse: collapse" bgcolor="#c3d4dd">
                <tr>
                    <td width="98%" class="rowEven">
                        SHIPPING & ORDER DETAILS
                    </td>
                    <td width="2%" style="background-color: White">
                        &nbsp;
                    </td>
                   
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td width="100%">
            <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse">
                <tr>
                    <td width="48%">
                        <table width="100%" cellpadding="2" cellspacing="0" border="0" style="border-collapse: collapse">                          
                            <tr>
                                <td width="40%" align="left" class="TextColumnStylePARO">
                                    ORDER NO
                                </td>
                                <td width="60%" align="left">
                                    <asp:Label ID="lblOrderNo" runat="server" Text="" CssClass="LabelStylePARO"></asp:Label>
                                </td>
                            </tr>                          
                        </table>
                    </td>
                    <td width="4%" style="background-color: White">
                        &nbsp;
                    </td>
                    <td width="48%" align="left">
                        
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td width="100%">
            <table width="100%" cellpadding="4" cellspacing="0" border="0" style="font-family: Arial;
                font-size: small; font-weight: normal; border-collapse: collapse" bgcolor="#c3d4dd">
                <tr>
                    <td width="48%" align="left" class="rowEven">
                        BILL TO:
                    </td>
                    <td width="4%" style="background-color: White">
                        &nbsp;
                    </td>
                    <td width="48%%" align="left" class="rowEven">
                        SHIP TO:
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td width="100%">
            <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse"
                bgcolor="white">
                <tr>
                    <td width="48%" align="left">
                        <asp:Label ID="lblDeliveryTo" runat="server" Text="Delivery Address" CssClass="LabelStylePARO"
                            Font-Bold="false"></asp:Label>
                    </td>
                    <td width="4%">
                        &nbsp;
                    </td>
                    <td width="48%" align="left">
                        <asp:Label ID="lblShipTo" runat="server" Text="Shipping Address" CssClass="LabelStylePARO"
                            Font-Bold="false"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr valign="top">
        <td width="100%" colspan="3" valign="top">
            &nbsp;
        </td>
    </tr>
      <tr>
        <td width="100%" bgcolor="#0092c8" style="font-family: Arial; font-size: small; font-weight: bold;
            color: White;">
            PAYMENT INFORMATION
        </td>
    </tr>
     <tr>
        <td width="100%" bgcolor="#0092c8" style="font-family: Arial; font-size: small; font-weight: bold;color: White;">
   
            <div runat ="server" id="div1">
            <table width="100%" cellpadding="0" cellspacing="0" border="0" id="CardDetails"  >
            <tr>
      <td>         
         <asp:Label runat="server" ID="lblpaymentmethod" Text="Payment Method:" Width="150px"/>
         <asp:DropDownList runat="server" ID="drppaymentmethod" Width="158px" CssClass="cardinput"/>
      </td>
    </tr>
    <tr>
      <td>
         <asp:Label runat="server" ID="lblcardnumber" Text="Card Number:" Width="150px" />
         <asp:TextBox runat="server" ID="txtCardNumber" Width="150px" CssClass="cardinput"  MaxLength="19" />
      </td>
    </tr>
    <tr>
    <tr>
      <td>
         <asp:Label runat="server" ID="Label1" Text="Card Name:" Width="150px" />
         <asp:TextBox runat="server" ID="txtCardName" Width="150px" CssClass="cardinput"  MaxLength="50" />
      </td>
    </tr>
    <tr>
      <td>
          <asp:Label runat="server" ID="lblexpirationdate" Text="Card Number:" Width="150px"/>
           <asp:DropDownList NAME="drpExpmonth" Width="75px" ID="drpExpmonth" runat="server"  CssClass="cardinput">           
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
          <asp:DropDownList NAME="drpExpyear" Width="75px" ID="drpExpyear" runat="server" CssClass="cardinput">          
         </asp:DropDownList>
      </td>
    </tr>
    <tr>
      <td>
         <asp:Label runat="server" ID="lblcardcvvnumber" Text="Card ID Number:" Width="150px"/>
         <asp:TextBox runat="server" ID="txtCardCVVNumber" Width="50px" CssClass="cardinput" MaxLength="3"/>         
      </td>
    </tr>
     <tr>
      <td>         
           
           <asp:Button ID="btnPay" runat="server" Text="Button" OnClick="OnClick_Pay"/>   
                  
      </td>
    </tr>
            </table>
            </div>
               <div runat ="server" id="div2">
               </div>

               
        </td>
    </tr>
    
    <tr>
      <td width="100%">
        
      </td>
    </tr>
    <tr>
        <td width="100%" bgcolor="0092c8" style="font-family: Arial; font-size: small; font-weight: bold;
            color: White;">
            ORDER CONTENTS
        </td>
    </tr>
    <tr>
        <td width="100%">
            <table width="100%" id="test1" border="0" cellpadding="3" cellspacing="0" style="font-family: Arial;
                font-size: small; font-weight: normal; border-collapse: collapse; border-color: Red;"
                bgcolor="#c1d8d9">
         
                <tr  class="rowEven">
                    <td align="left" width="20%">
                        ORDER CODE
                    </td>
                    <td align="left" width="10%">
                        QTY
                    </td>
                    <td align="left" width="20%">
                        Description
                    </td>
                    <td  align="left" width="20%">
                        Cost(Ex. GST)
                    </td>
                    <td align="left" width="30%">
                        Extension Amount (Ex. GST)
                    </td>
                </tr> 
                <asp:Repeater ID="OrderitemdetailRepeater"   runat="server"  > 
                 
                    <ItemTemplate >
                        <tr id="tRow"  runat="server"  class="rowOdd">
                                        <td  id="TD1" runat="server"><%# Eval("CATALOG_ITEM_NO")%></td>
                                        <td><%# Eval("QTY")%></td>
                                        <td > <%# Eval("DESCRIPTION")%></td>
                                         <td style="text-align:right;"> $ <%# Eval("PRICE_EXT_APPLIED")%></td>
                                          <td style="text-align:right;"> $ <%# Eval("PRICE_EXT_APPLIED")%></td>
                                       
                       </tr>  
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr id="tRow"  runat="server" class="rowEven">
                                        <td  id="TD1" runat="server"><%# Eval("CATALOG_ITEM_NO")%></td>
                                        <td><%# Eval("QTY")%></td>
                                        <td > <%# Eval("DESCRIPTION")%></td>
                                         <td style="text-align:right;"> $ <%# Eval("PRICE_EXT_APPLIED")%></td>
                                          <td style="text-align:right;"> $ <%# Eval("PRICE_EXT_APPLIED")%></td>
                                       
                       </tr>  
                    </AlternatingItemTemplate>
               </asp:Repeater> 
         
                  
               <tr style="background-color: white; font-size: 12px;">
                  <td align="left" width="20%">
                        
                    </td>
                    <td align="left" width="10%">
                      
                    </td>
                    <td align="left" width="20%">
                        	
                    </td>
                    <td  align="left" width="20%" class="rowEven">
                        <strong>Sub Total </strong>
                    </td>
                  <td align="left" width="30%" class="rowEven" style="text-align:right;">
                     <strong>  $   <asp:Label runat="server" ID="Product_Total_price"/> </strong>
                  </td>
                </tr>
                 <tr style="background-color: white; font-size: 12px;">
                  <td align="left" width="20%">
                        
                    </td>
                    <td align="left" width="10%">
                      
                    </td>
                    <td align="left" width="20%">
                        	
                    </td>
                    <td  align="left" width="20%" class="rowOdd">
                        <strong>Tax Amount(GST) </strong>
                    </td>
                  <td align="left" width="30%" class="rowOdd" style="text-align:right;">
                      <strong> $  <asp:Label runat="server" ID="Tax_amount"/></strong>
                  </td>
                </tr>
                 <tr style="background-color: white; font-size: 12px;">
                  <td align="left" width="20%">
                        
                    </td>
                    <td align="left" width="10%">
                      
                    </td>
                    <td align="left" width="20%">
                        	
                    </td>
                    <td  align="left" width="20%" class="rowEven">
                        <strong>Total Inc GST</strong>
	
                    </td>
                  <td align="left" width="30%" class="rowEven" style="text-align:right;">
                    <strong>  $  <asp:Label runat="server" ID="Total_Amount"/></strong>
	 
                  </td>
                </tr>
              
              
            </table>
        </td>
    </tr>
</table>
    </div>
    </form>
</body>
</html>
