<%@ Page Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" Inherits="QuoteList" Title="Untitled Page" Codebehind="QuoteList.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" Runat="Server">
<table align="center" width="558" border="0" cellspacing="0" cellpadding="5">
<tr>
<td align="left" class="tx_1">
    <a href="home.aspx" style="color:#0099FF" class="tx_3">Home</a> / Quote List
</td>
</tr>
<tr>
<td class="tx_3">
    <hr>
</td>
</tr>
</table>
 <br /> 
<table width="558" >
<tr>
<td align ="left" >
<asp:Panel ID="pnlQuote" runat="server" >
</asp:Panel>
<asp:Label ID="Message" runat="server" meta:resourcekey="lblQteListMsg" Class="lblBoldHeadSkin"></asp:Label>
</td>
</tr>
</table>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="rightnav" Runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>

