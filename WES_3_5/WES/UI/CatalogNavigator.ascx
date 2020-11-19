<%@ Control Language="C#" AutoEventWireup="true" Inherits="UI_CatalogNavigator"  Codebehind="CatalogNavigator.ascx.cs" %>
<%@ Import Namespace ="System.Data" %>
<%@ Import Namespace ="TradingBell.Common" %>
<%@ Import Namespace ="TradingBell.WebServices" %>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%
    string CategoryLists = BuildCategoryListHTML();
    if (CategoryLists == "")
    {
        CategoryLists = "<TABLE WIDTH=\"100%\" HEIGHT=\"100%\"><TR><TD WIDTH=\"100%\" HEIGHT=\"100%\" ALIGN=\"CENTER\" VALIGN=\"MIDDLE\">";
        CategoryLists = CategoryLists + "<Font Color=\"Red\" face=Verdana Size=\"2\"><BR/><BR/><B>Unable to show Products due to Invalid Layout Specification.</B></Font></TD></TR>";
        Response.Write(CategoryLists);
    }
    else
    {
        Response.Write(CategoryLists);
    }
%>