<%@ Page Language="C#" AutoEventWireup="true" Inherits="ShippingInfo"   Codebehind="ShippingInfo.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script  type="text/javascript">
    function close()
    {
    window.close();
    }
    </script>
</head>
<body>
    <form id="form1" runat="server">
   
    <table>
    <tr>
    <td>
    <asp:Label ID="lblShippingCharge" meta:resourcekey="lblShippingCharge" runat="server" class="lblBoldSkin" />
   
    <asp:Label ID="txtShipCharge" runat="server" Class="lblResultSkin" />
    </td>
     </tr>
    <tr>
    <td align="center">
 
<%--<asp:LinkButton ID="lbtnCsc" Text ="Close" class="CompanylinkSkin" runat ="server" OnClientClick ="javascript:window.close()"></asp:LinkButton>
--%>
    </td>
    </tr>
   
    </table>

    
    </form>
</body>
</html>
