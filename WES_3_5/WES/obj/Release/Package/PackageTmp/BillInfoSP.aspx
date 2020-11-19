<%@ Page Title="" Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" CodeBehind="BillInfoSP.aspx.cs" Inherits="BillInfoSP" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="server">

</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" runat="server">
</asp:Content>--%>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" runat="server">
    <div id="BodyContentDiv" runat="server">
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
            <span class="aero">Shipping / Delivery Details</span>
            </li>
            <li>
            <span class="aero">Payment Options</span>
            </li>
            <li>
            <span class="aero currentpg">Completed</span>
            </li>
            </ul>
</div>
<div class="grid12">
<div class="">
<div class="cl"></div>
         <div id="divOk" runat="server"  class="alert greenbox icon_2"  style="padding:30px 10px 30px 60px;font-family: arial;font-size: 13px;font-weight: bold;background-position: 292px center;text-align: center;">
        </div>
<div class="grid6" style="width:469px;" >
<fieldset style="height:100px;">
  <legend>Order Details</legend>
<p class="para pad10">Order No.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;: <asp:Label ID="lblOrderNo" runat="server" Text="" CssClass="LabelStyle" style="font-weight:normal;" ></asp:Label></p>
<p class="para pad10">Shipping Method : <asp:Label ID="lblShippingMethod" runat="server" Text="" CssClass="LabelStyle" style="font-weight:normal;" ></asp:Label></p>
</fieldset>
</div>
<div class="grid6" style="width:469px;" >
<fieldset style="height:100px;">
<legend>Payment Information</legend>
     <table id="tblBase" width="100%" border="0" cellpadding="3" cellspacing="0" style="border-collapse: collapse"
    align="left">
     <tr>
        <td width="100%"   style="font-family: Arial; font-size: small; font-weight:normal;" colspan="2" align="left">
            <div runat ="server" id="div1">
            <table  cellpadding="2" cellspacing="0" border="0" id="CardDetails" >
    <tr>
    <td>         
    <div id="divContent"  runat="server" >
     <div id="divError" runat="server" style="color:Red;" >
        </div>

        <div id="divlink" runat="server" >
        </div>
    </div>
    </td>
    </tr>            
            </table>
             </div> 
        </td>
        </tr>
    </table>
</fieldset>
</div>


<div class="cl"></div>
<div class="grid6" style="width:469px;" >
<fieldset style="height:150px;">
<legend>Bill To</legend>
<p class="para pad10"> <asp:Label ID="lblDeliveryTo" runat="server" Text="Delivery Address" CssClass="LabelStyle"
            Font-Bold="false"  style="font-weight:normal;"></asp:Label></p>
<div class="cl"></div>
</fieldset>
</div>
<div class="grid6" style="width:469px;" >
<fieldset style="height:150px;">
<legend>Ship To</legend>
<p class="para pad10"> <asp:Label ID="lblShipTo" runat="server" Text="Shipping Address" CssClass="LabelStyle"
                Font-Bold="false"  style="font-weight:normal;"></asp:Label>
</p>
<div class="cl"></div>
</fieldset>
</div>
<div class="cl"></div>

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
                                         <td style="text-align:right;"> $ <%# Convert.ToDecimal(Eval("TOTAL_EXT")).ToString("#,#0.00")%></td>
                                       
                       </tr>  
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr id="tRow"  runat="server" class="">
                                        <td  id="TD1" runat="server" style="text-align:left;"><%# Eval("CATALOG_ITEM_NO")%></td>
                                        <td style="text-align:left;"><%# Eval("QTY")%></td>
                                        <td style="text-align:left;"> <%# Eval("DESCRIPTION")%></td>
                                         <td style="text-align:right;"> $ <%# Convert.ToDecimal(Eval("PRICE_EXT_APPLIED")).ToString("#,#0.00")%></td>
                                          <td style="text-align:right;"> $ <%# Convert.ToDecimal(Eval("TOTAL_EXT")).ToString("#,#0.00")%></td>
                                       
                       </tr>  
                    </AlternatingItemTemplate>
               </asp:Repeater> 
         
                  
               <tr style="background-color: white; font-size: 12px;">
                  <td align="left" width="50%"  colspan="3"   rowspan="4">
                        
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
                        <strong>Total Tax Amount(GST) </strong>
                    </td>
                  <td align="left" width="30%" class="rowOdd" style="text-align:right;">
                      <strong> $  <asp:Label runat="server" ID="Tax_amount"/></strong>
                  </td>
                </tr>
                  
                 <tr style="background-color: white; font-size: 12px;">
                  <%--<td align="left" width="50%"  >
                        
                    </td>--%>

                    <td  align="left" width="20%" class="Rsucess">
                        <strong>Total Inc GST</strong>
	
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


   
</div>
</div>
 </div>


 </td>
 </tr>
 </table>
 </div>
</asp:Content>
<%--<asp:Content ID="Content5" ContentPlaceHolderID="rightnav" runat="server">
</asp:Content>--%>
<asp:Content ID="Content6" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
