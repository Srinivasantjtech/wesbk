<%@ Page Language="C#" MasterPageFile="~/mainpage.master" ViewStateMode="Enabled" AutoEventWireup="true" Inherits="OrderTemplate" Title="Untitled Page"  Culture="en-US" UICulture="en-US" ValidateRequest="false" Codebehind="OrderTemplate.aspx.cs" %>
<%--<%@ Import Namespace ="TradingBell.Common" %>--%>
<%@ Import Namespace="TradingBell.WebCat.Helpers" %>

<%@ Register src="UC/BulkOrder.ascx" tagname="BulkOrder" tagprefix="uc1" %>


<asp:Content ID="Content2" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="header" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="leftnav" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" Runat="Server">    
 <uc1:BulkOrder ID="BulkOrder1" runat="server" /> 
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="rightnav" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>


