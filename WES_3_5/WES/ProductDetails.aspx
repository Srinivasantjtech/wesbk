<%@ Page Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" Inherits="ProductDetails" Title="Untitled Page" Codebehind="ProductDetails.aspx.cs"  ValidateRequest="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="UC/Product.ascx" tagname="Product" tagprefix="uc1" %>
<%@ Register src="UC/moreproducts.ascx" tagname="moreproducts" tagprefix="uc2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<h3 id="h3_1" runat ="server" class="H3Tag"/> 
<h3 id="h3_2" runat ="server" class="H3Tag"/>
<h3 id="h3_3" runat ="server" class="H3Tag"/>
<h2 id="h2" runat ="server" class="H2Tag"/>
<h1 id="h1" runat ="server"  class="H1Tag"/>
<h4 id="h4" runat ="server"  class="H4Tag"/>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" Runat="Server">
    <uc1:Product ID="Product1" runat="server" />
   
    <%--<uc2:moreproducts ID="moreproducts1" runat="server" />--%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="rightnav" Runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>