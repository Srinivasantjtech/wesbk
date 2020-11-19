<%@ Page Language="C#" MasterPageFile="~/Mainpage.Master" AutoEventWireup="true" Inherits="ProductFinder" Title="Untitled Page"
     Culture="en-US" UICulture="en-US" Codebehind="ProductFinder.aspx.cs" %>
     
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %> 
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="uc\ProductFinder.ascx" TagName="ProductFinder" TagPrefix="uc4" %>

<asp:Content ID="Content5" ContentPlaceHolderID="Popupcontent" runat="Server" >
  
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server" >

<meta http-equiv="X-UA-Compatible" content="IE=edge"/>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
<%--<script src="/Scripts/AC_RunActiveContent.js" type="text/javascript"></script>
 <script src="Scripts/All_JQ_Mainpage.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>" type="text/javascript"></script>
<script src="Scripts/All_Js_Top.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>" type="text/javascript"></script>
<script src="Scripts/All_JS_MASTER.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>" type="text/javascript"></script>--%>
<link href='http://fonts.googleapis.com/css?family=Oswald:400,700,300' rel='stylesheet' type='text/css'>
<link href='http://fonts.googleapis.com/css?family=Open+Sans:400,600' rel='stylesheet' type='text/css'>

  <link rel="Stylesheet" href="css/thickboxAddtocart.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["CSSVersion"].ToString()%>" type="text/css" />
     <%--<script language="javascript" src="Scripts/thickboxaddtocart.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>" type="text/javascript" />--%>
              <script type="text/javascript"  >
                  (function () {
                      var s = document.createElement('script');
                      s.type = 'text/javascript';
                      s.async = true;
                      s.src = 'Scripts/thickboxaddtocart.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>';
                      var x = document.getElementsByTagName('script')[0];
                      x.parentNode.insertBefore(s, x);
                  })();
</script>
<%--
  <script src="Scripts/Productfilter.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["JSVersion"].ToString()%>" type="text/javascript"></script>
--%>
<%--<link rel="stylesheet" type="text/css" media="screen" href="css/AllCss.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["CSSVersion"].ToString()%>" />
--%>
<link rel="stylesheet" type="text/css" media="screen" href="css/productfilter.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["CSSVersion"].ToString()%>" />

<%--   <table width="970" border="0" cellspacing="0" cellpadding="3" align="center" >
                    <tr>
                        <td width="970" valign="top">--%>
                         <uc4:ProductFinder ID="ProductFinder1" runat="server" />
<%--
                        </td>
                    </tr>
                </table>--%>
         
<%--<script type="text/javascript">
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
</script>--%>
</asp:Content>


