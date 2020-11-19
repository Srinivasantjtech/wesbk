<%@ Page Language="C#" MasterPageFile="~/Mainpage.Master" AutoEventWireup="true" Inherits="ProductFilter" Title="Untitled Page"
     Culture="en-US" UICulture="en-US" Codebehind="ProductFilter.aspx.cs" %>
     
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %> 
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="uc\ProductFilter.ascx" TagName="ProductFilter" TagPrefix="uc4" %>

<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" runat="Server" >
  
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Popupcontent" runat="Server" >
<%--<link rel="stylesheet" type="text/css" media="screen" href="css/AllCss.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["CSSVersion"].ToString()%>" />
--%>
<link rel="stylesheet" type="text/css" media="screen" href="css/productfilter.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["CSSVersion"].ToString()%>" />
   <table width="970" border="0" cellspacing="0" cellpadding="3">
                    <tr>
                        <td width="970" height="420" valign="top">
                            <uc4:ProductFilter ID="ProductFilter1" runat="server" />
                        </td>
                    </tr>
                </table>
                
<script type="text/javascript">
    var fby = fby || [];
    fby.push(['showTab', { id: '7210', position: 'right', color: '#FF1F3A'}]);
    (function () {
        var f = document.createElement('script'); f.type = 'text/javascript'; f.async = true;
        f.src = '//cdn.feedbackify.com/f.js';
        var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(f, s);
    })();
    jQuery(document).ready(function () {
        //Tabs
        $('#horizontalTab').responsiveTabs({
            startCollapsed: 'accordion',
            collapsible: 'accordion',
            rotate: false,
            setHash: true
        });
        SyntaxHighlighter.all();
    });
</script>
</asp:Content>
