<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_CartItems" Codebehind="CartItems.ascx.cs" %>
<%--<%@ Import Namespace  ="TradingBell.Common"  %>--%>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %>
<asp:UpdatePanel runat="server">
<ContentTemplate>
<%
    HelperServices objHelperServices = new HelperServices();
    if (objHelperServices.GetOptionValues("ECOMMERCEENABLED").ToString() == "YES")
    {     
%>
<table id ="tblCart" border ="0" cellpadding ="0" cellspacing="0">	
<tr>
    <td valign="top">
        <div style="margin-left:30px">
        <br />
            <asp:Label ID="lblItem" runat="server" Visible="false" class="lblNormalSkin" Text="<%$ Resources:CategoryNavigator, lblItem %>"></asp:Label>
            <asp:Label ID="lblItemCount" runat="server" class="lblNormalSkin" Text="" Font-Size="X-Small"></asp:Label>
        <br />
        </div>
        <div style="margin-left:30px">
            <asp:Label ID="lblCost" runat="server" Class="lblNormalSkin" Font-Size="X-Small" Text="<%$ Resources:CategoryNavigator, lblCartSubTotal %>"></asp:Label>
            <asp:Label ID="lblCostValue" runat="server" Class="lblNormalSkin" Text="" Font-Size="X-Small"></asp:Label>
        <br />
        </div>
    </td>
</tr>
</table>
<%} %>
</ContentTemplate>
</asp:UpdatePanel>