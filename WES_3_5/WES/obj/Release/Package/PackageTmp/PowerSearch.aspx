<%@ Page Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" Inherits="PowerSearchPage" Title="Untitled Page" Codebehind="PowerSearch.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register src="UC/searchctrl.ascx" tagname="searchctrl" tagprefix="uc1" %>

<%--<%@ Register Src="~/search/searchparametricfilter.ascx" TagName="searchparametricfilter" TagPrefix="uc5" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" Runat="Server">
<link href="search/templates/type1/searchrsltproducts/searchrsltproducts_files/base.css" type="text/css" rel="Stylesheet" />
<script type = "text/javascript" language ="javascript">
//    window.history.forward(1);
</script>

    <uc1:searchctrl ID="searchctrl1" runat="server" />
</asp:Content>


<asp:Content ID="Content6" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>

