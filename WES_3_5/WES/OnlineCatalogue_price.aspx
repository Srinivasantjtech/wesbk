<%@ Page Title="" Language="C#" MasterPageFile="~/OC.Master" AutoEventWireup="true" CodeBehind="OnlineCatalogue_price.aspx.cs" Inherits="WES.OnlineCatalogue_price" %>
<%@ Register Src="UC/OnlineCatalogue_price.ascx" TagName="OnlineCatalogue_price" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link rel="stylesheet" type="text/css" media="screen" href="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>css/onlinecatalogue.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["CSSVersion"].ToString()%>" />   --%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" Runat="Server">
    <uc1:OnlineCatalogue_price ID="OnlineCatalogue_price1" runat="server"></uc1:OnlineCatalogue_price>    
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
