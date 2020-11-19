<%@ Page Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" Inherits="Cataloguedownload" Title="Untitled Page" Codebehind="Cataloguedownload.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="UC/CatalogueDownload.ascx" TagName="CatalogueDownload" TagPrefix="uc1" %>
<%@ Register Src="UC/Newsupdate.ascx" TagName="Newsupdate" TagPrefix="uc1" %>

<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %>
<%@ Import Namespace ="System.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" runat="Server">
    <%--<style type="text/css">
        .fancy .ajax__tab_header
        {
            cursor: pointer;
            font-size: 10px;
            font-weight: bold;
            color: Gray;
            font-family: sans-serif;
            background: url(img/blue_bg.jpg) repeat-x;
            border-top: 1px solid black;
            border-left: 1px solid black;
            border-right: 1px solid black;
            border-bottom: none;
        }
        .fancy .ajax__tab_active .ajax__tab_outer, .fancy .ajax__tab_header .ajax__tab_outer, .fancy .ajax__tab_hover .ajax__tab_outer
        {
            height: 46px;
            background: url(img/blue_left.jpg) no-repeat left top;
        }
        .fancy .ajax__tab_active .ajax__tab_inner, .fancy .ajax__tab_header .ajax__tab_inner, .fancy .ajax__tab_hover .ajax__tab_inner
        {
            height: 30px;
            margin-left: 10px; /* offset the width of the left image */
            background: url(img/blue_right.jpg) no-repeat right top;
        }
        .fancy .ajax__tab_active .ajax__tab_tab, .fancy .ajax__tab_hover .ajax__tab_tab, .fancy .ajax__tab_header .ajax__tab_tab
        {
            margin: 16px 16px 0px 0px;
        }
        
        .fancy .ajax__tab_hover .ajax__tab_tab, .fancy .ajax__tab_active .ajax__tab_tab
        {
            color: #0077cc;
        }
        .fancy .ajax__tab_body
        {
            font-family: verdana,tahoma,helvetica;
            font-size: 10pt;
            border: 1px solid #999999;
            border-top: 0;
            padding: 2px;
            background-color: White;
        }
        
        html
        {
            background-color: White;
            color: #0099ff;
        }
        .menuTabs
        {
            position: relative;
            top: 1px;
            left: 10px;
            height:25px;
        }
        .tab
        {
            border: Solid 1px #cccccc;
            border-bottom: none;
            padding: 0px 10px;
            background-color: Gray;
            color: #0099ff;
             height:25px;
        }
        .selectedTab
        {
            border: Solid 1px #cccccc;
            border-bottom: none;
            padding: 0px 10px;
            background-color: #0099ff;
            color: #0099ff;
            font-weight: bold;
             height:25px;
        }
        .tabBody
        {
            border: Solid 1px #cccccc;
            padding: 20px;
            background-color: white;
        }
        
        .tabHeader
        {
            position: absolute;
            right: 0px;
            width: 300px;
            background-color: #b0e0e6;
        }
    </style>--%>
    <div>
         <uc1:CatalogueDownload ID="CatalogueDownload2" runat="server"   />
   
       <%-- <asp:Menu ID="menuTabs" CssClass="menuTabs" StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab"
            Orientation="Horizontal" OnMenuItemClick="menuTabs_MenuItemClick" runat="server">
              
            <Items >
                <asp:MenuItem Text="CATALOGUE" Value="0" />
            
                <asp:MenuItem Text="WES NEWS" Value="1" />
                 
                <asp:MenuItem Text="FORMS / OTHERS" Value="2"  />
            </Items>
        </asp:Menu>
        <div class="tabBody">
            <asp:MultiView ID="multiTabs" runat="server">
                <asp:View ID="view1" runat="server">
                    <uc1:CatalogueDownload ID="CatalogueDownload2" runat="server"   />
                </asp:View>
                <asp:View ID="view2" runat="server">
                    <uc1:Newsupdate ID="Newsupdate2" runat="server" />
                </asp:View>
                <asp:View ID="view3" runat="server">
                    <uc1:Newsupdate ID="Newsupdate1" runat="server" />
                </asp:View>
            </asp:MultiView>
        </div>
    </div>--%>
    <%--<div>
        <asp:TabContainer ID="TabContainer1" runat="server" CssClass="fancy" TabStripPlacement="Top"
            UseVerticalStripPlacement="true">
            <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="CATALOGUE DOWNLOAD" Enabled="true" CssClass="tabHeader"
                ScrollBars="Auto" OnDemandMode="Once">
                <ContentTemplate>
                <uc1:CatalogueDownload ID="CatalogueDownload1" runat="server" />
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="WES NEWS DOWNLOAD" Enabled="true"
                ScrollBars="Auto" OnDemandMode="Once">
                <ContentTemplate>
                <uc1:Newsupdate ID="Newsupdate1" runat="server" />
                </ContentTemplate>
            </asp:TabPanel>
        </asp:TabContainer>
    </div>--%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="rightnav" runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="footer" runat="Server">
</asp:Content>
