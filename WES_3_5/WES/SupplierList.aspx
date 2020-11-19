<%@ Page Language="C#" MasterPageFile="~/mainpage.master" AutoEventWireup="true" Inherits="SupplierList" Title="Untitled Page"  Culture = "auto:en-US" UICulture = "auto" Codebehind="SupplierList.aspx.cs" %>
<%@ Register TagPrefix ="WebCat" TagName ="SupplierList" Src ="UI/SupplierList.ascx" %>


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
            <a href="home.aspx" style="color:#0099FF" class="tx_3">Home</a> / Supplier list
          </td>
        </tr>
        <tr>
          <td class="tx_3">
            <hr>
          </td>

        </tr>
      </table>
      <br />

<div>
    <WebCat:SupplierList ID ="ucSupplierList" runat ="server" SupplierListVisible="Yes" HeadCssClass="TableHead" CssClass="TableText" HeadLinkCssClass="HeadLink" LinkCssClass="Link" />
</div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="rightnav" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>


