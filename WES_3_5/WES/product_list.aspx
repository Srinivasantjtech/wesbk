<%@ Page Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" Inherits="product_list" Title="Untitled Page" Codebehind="product_list.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="UC/productlist.ascx" TagName="productlist" TagPrefix="uc1" %>


<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" Runat="Server">


   <h3 id="h3_1" runat ="server" class="H3Tag"/> <h3 id="h3_2" runat ="server" class="H3Tag"/><h3 id="h3_3" runat ="server" class="H3Tag"/>
    <uc1:productlist ID="Productlist1" runat="server"></uc1:productlist>    
</asp:Content>


