<%@ Page Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" Inherits="bybrand" Title="Untitled Page" Codebehind="bybrand.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="UC/bybrand.ascx" tagname="bybrand" tagprefix="uc1" %>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" Runat="Server">
    <h1 id="h1" runat ="server" class="H1Tag"/><h2 id="h2" runat ="server" class="H2Tag"/>
    <ContentTemplate>
    <uc1:bybrand ID="bybrand1" runat="server" />
    </ContentTemplate>
</asp:Content>