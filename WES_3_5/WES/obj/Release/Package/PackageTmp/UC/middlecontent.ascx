<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="UC_middlecontent" Codebehind="middlecontent.ascx.cs" %>
<%@ Register Src="newproducts.ascx" TagName="newproducts" TagPrefix="uc1" %>
<%@ Register Src="ProductFilter.ascx" TagName="ProductFilter" TagPrefix="uc4" %>
<%@ Register Src="promotions.ascx" TagName="promotions" TagPrefix="uc2" %>
<%@ Register Src="banner.ascx" TagName="banner" TagPrefix="uc3" %>
<%--<style type="text/css">
    .style1
    {
        height: 175px;
    }
    .style3
    {
        border-bottom: 1px solid #EFEFEF;
        height: 175px;
    }
</style>--%>

<body class="fondo_">
    <table width="970" border="0" cellspacing="0" cellpadding="3">
        <tr>
            <td>
             <%--<table width="970" border="0" cellspacing="0" cellpadding="3">
                    <tr>
                        <td width="970" height="420" valign="top">
                            <uc4:ProductFilter ID="ProductFilter1" runat="server" />
                        </td>
                    </tr>
                </table>--%>
                <table width="970" border="0" cellspacing="0" cellpadding="3">
                    <tr>
                        <td width="970" height="420" valign="top">
                            <uc1:newproducts ID="newproducts1" runat="server" />
                        </td>
                    </tr>
                </table>
                <table width="970" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="100" valign="top">
                            <script type="text/javascript">
                                AC_FL_RunContent('codebase', 'http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,28,0', 'width', '970', 'height', '100', 'src', 'images/swf/slide_logos', 'quality', 'high', 'pluginspage', 'http://www.adobe.com/shockwave/download/download.cgi?P1_Prod_Version=ShockwaveFlash', 'menu', 'false', 'wmode', 'transparent', 'movie', 'images/swf/slide_logos'); //end AC code
                            </script>
                            <noscript>
                                <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,28,0"
                                    width="970" height="100">
                                    <param name="movie" value="images/swf/slide_logos.swf">
                                    <param name="quality" value="high">
                                    <param name="menu" value="false">
                                    <param name="wmode" value="transparent">
                                    <embed src="images/swf/slide_logos.swf" width="970" height="100" quality="high" pluginspage="http://www.adobe.com/shockwave/download/download.cgi?P1_Prod_Version=ShockwaveFlash"
                                        type="application/x-shockwave-flash" menu="false" wmode="transparent"></embed>
                                </object>
                            </noscript>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
