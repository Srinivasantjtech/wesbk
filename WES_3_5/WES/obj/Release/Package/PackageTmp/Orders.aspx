<%@ Page Language="C#" MasterPageFile="~/mainpage.master" AutoEventWireup="true" Inherits="Orders" Title="Untitled Page"   Culture ="auto:en-US" UICulture ="auto" Codebehind="Orders.aspx.cs" %>
<%@ Import Namespace ="System.Data" %>
<%--<%@ Import Namespace ="TradingBell.Common" %>
<%@ Import Namespace ="TradingBell.WebServices" %>--%>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="header" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="leftnav" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" Runat="Server">
<table align="center" width="558" border="0" cellspacing="0" cellpadding="5">
        <tr>
          <td align="left" class="tx_1">
            <a href="home.aspx" style="color:#0099FF" class="tx_3">Home</a><font style="font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal"> / </font>Orders
          </td>
        </tr>
        <tr>
          <td class="tx_3">
            <hr>
          </td>

        </tr>
      </table>
      <br />
<table align=center Width="558">
<tr>
<td align ="left" >
<asp:Panel ID="pnlOrder" runat="server"  Width="558">
</asp:Panel>
</td>
</tr>
</table>
    

</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="rightnav" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>

