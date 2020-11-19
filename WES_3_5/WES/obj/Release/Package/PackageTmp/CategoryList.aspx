<%@ Page Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" Inherits="CategoryList" Title="Untitled Page" Codebehind="CategoryList.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="UC/categorylist.ascx" TagName="categorylist" TagPrefix="uc1" %>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" Runat="Server">

  <h3 id="h3_1" runat ="server" class="H3Tag" /> <h3 id="h3_2" runat ="server" class="H3Tag"/><h3 id="h3_3" runat ="server" class="H3Tag"/><h2 id="h2" runat ="server" class="H2Tag"/>
    <uc1:categorylist ID="Categorylist1" runat="server" />   
</asp:Content>

