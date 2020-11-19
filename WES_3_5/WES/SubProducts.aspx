<%@ Page Language="C#" AutoEventWireup="true" Inherits="SubProducts" Codebehind="SubProducts.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
  <%--<script language="javascript" type="text/javascript">
          jQuery(document).ready(function () {
              jQuery("#TB_window").append("<div id='TB_load'><img src='images/closebtn.png' /></div>"); //add loader to the page
              jQuery('#TB_load').show(); //show loader
          })
          </script>--%>
    <title></title>
     <%-- <img  src="Images/closebtn.png" style="margin-left: 395px;" onclick="tb_remove()" />--%>
</head>

<body>
    <form id="form1" runat="server">
     <table  width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
    <td >
   <h3 class="titleblue"><b>Order Clarification/Errors</b>
     <%--<input type="Button" src="Images/closebtn.png" value="CLOSE" style="padding-left:50;"  onclick="tb_remove()" />--%>
    
   </h3>

    <table cellpadding="5" width="100%" cellspacing="0">
        <%--<tr>
            <td colspan="3" class="tx_boheader" align="left">
                Order Clarification/Errors
            </td>
            <td class="tx_boheader" align="right">
                <input type="Button" src="Images/close.png" value="CLOSE" onclick="tb_remove()" />
            </td>
        </tr>--%>
        <tr>
            <td colspan="4">
                Product Item Clarification: <font color="red"><strong>
                    <%=Request["Item"] %></strong></font>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                Please select an item from below to update order with:
            </td>
        </tr>
        <tr>
            <td class="tx_bohead1" width="10%">
                &nbsp;
            </td>
            <td class="tx_bohead1" width="15%">
                Code
            </td>
            <td class="tx_bohead1" width="40%">
                Description
            </td>
            <td class="tx_bohead1" width="35%">
                <% if (Request["Tempid"] != null)
                   { %>
                Add to Order Template
                <%}
                   else
                   { %>
                   Add to Cart
                   <%} %>
            </td>
            <%--<td class="tx_bohead1" width="0%" style="display: none;">
                Product Details
            </td>--%>
        </tr>
        <%=GetProducts() %>
        
    </table>
    </td>
    </tr>
    </table>
    </form>
</body>
</html>
