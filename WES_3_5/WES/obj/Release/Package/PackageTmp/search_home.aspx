<%@ Page Language="C#" AutoEventWireup="true" Inherits="search_home" Codebehind="search_home.aspx.cs" %>

<%@ Register Src="search/search.ascx" TagName="search" TagPrefix="uc1" %>
<%--<%@ Register Src="search/searchrsltcategory.ascx" TagName="searchrsltcategory" TagPrefix="uc2" %>
<%@ Register Src="search/searchrsltfamily.ascx" TagName="searchrsltfamily" TagPrefix="uc3" %>
--%>
<%@ Register Src="search/searchrsltproducts.ascx" TagName="searchrsltproducts" TagPrefix="uc4" %>
<%--<%@ Register Src="search/searchparametricfilter.ascx" TagName="searchparametricfilter"--%>
    TagPrefix="uc5" %>
   
   <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>

    <script type="text/javascript">
        function __doPostBack(eventTarget, eventArgument) {
            document.getElementById("__EVENTTARGET").value = eventTarget;
            document.getElementById("__EVENTARGUMENT").value = eventArgument;
            document.forms[0].submit();
        }


function GetSelItems(field) {

        var count=0;
        var sCategoryIds='';
        for (var j = 0; j < document.getElementsByName(field).length; j++)
        {            
            if(document.getElementsByName(field).item(j).checked==true)
            {
                sCategoryIds = sCategoryIds + document.getElementsByName(field).item(j).value + ",";
            }
        }

        sCategoryIds = sCategoryIds.substring(0, sCategoryIds.length - 1)
        if (sCategoryIds.length > 0) 
            __doPostBack('CATEGORYFILTER', sCategoryIds);
        else
            alert('Please select atleast one category for filtering');
 }

    </script>

    <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
    <title>Search</title>
    <link rel="stylesheet" type="text/css" href="/search.css" />
    <link href="search/search.css" rel="stylesheet" type="text/css" />
    <link href="search/templates/type1/searchrsltproducts/searchrsltproducts_files/base.css" type="text/css" rel="Stylesheet" />
  <%--  <link href="App_Themes/WebCat/WebCatStyle.css" type="text/css" rel="stylesheet" />--%>
</head>

<body>
    <form id="Form1">
    <input type="hidden" name="__EVENTTARGET" value="">
    <input type="hidden" name="__EVENTARGUMENT" value="">
    <table align="center" style="border: 0.5px solid #808080; width: 100%">
        <tr>
            <td>
                <table style=" width: 100%;">
                    <tr>
                        <td ">
				<img alt="" src="http://www.remichel.com/Home/homehdr3.gif"/></td>
                    </tr>
                        <tr>
        <td valign="top">
            <table cellpadding="4" cellspacing="0" width="100%" border=0 style="height: 1px" bordercolor="red">
                <tr>
                    <td class="MenuHead">
                        <a href="Default.aspx" style="text-decoration:none"><font color="white">Home</font></a>
                    </td>
                    <td bgcolor="#000066">
                    <a style='font-size: 15px; font-family: Arial Unicode MS, Arial; color:Red; font-weight:bold;'>| </a>
                    </td>
                    <td class="MenuHead" align="center">
                        <a href="Login.aspx" style="text-decoration:none"><font color="white">My Account</font></a>
                    </td>
                    <td bgcolor="#000066">
                    <a style='font-size: 15px; font-family: Arial Unicode MS, Arial; color:Red; font-weight:bold;'>| </a>
                    </td>
                    <td class="MenuHead" align="center">
                        <a href="Company.aspx" style="text-decoration:none"><font color="white">Company</font></a>
                    </td>
                    <td bgcolor="#000066">
                    <a style='font-size: 15px; font-family: Arial Unicode MS, Arial; color:Red; font-weight:bold;'>| </a>
                    </td>
                    <td class="MenuHead" align="center">
                        <a href="search_home.aspx" style="text-decoration:none"><font color="white">Product Search</font></a>
                    </td>
                    <td bgcolor="#000066">
                    <a style='font-size: 15px; font-family: Arial Unicode MS, Arial; color:Red; font-weight:bold;'>| </a>
                    </td>
                    <td class="MenuHead" align="right">
                        <a href="Login.aspx" style="text-decoration:none"><font color="white">Login</font></a>
                        
                    </td>
                    <td bgcolor="#000066" align="right">
                    <a style='font-size: 15px; font-family: Arial Unicode MS, Arial; color:Red; font-weight:bold;'>&nbsp;&nbsp;&nbsp;| </a>
                    </td>
                    <td class="MenuHead" align="right">
                        <a href="Contact.aspx" style="text-decoration:none"><font color="white">Contact Us</font></a>
                    </td>
                    <%--<td class="TableHead"> 
                        <a href="HelpFAQ.aspx#MailLink" style="text-decoration:none"><font color="white">&nbsp;&nbsp;Subscribe to email</font></a>
                    </td>--%>
                </tr>
            </table>
        </td>
    </tr>
                    <tr>
                        <td>
                            <div style="width: 100%; border: 0.1px solid #808080">
                                <table style="width: 100%">
                                    <tr>
                                        <td>
                                            <uc1:search ID="search1" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <%--<uc5:searchparametricfilter ID="searchparametricfilter1" runat="server" />--%>
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style="width: 100%" align="left">
                                               <%-- <uc2:searchrsltcategory ID="searchrsltcategory1" runat="server" />--%>
                                            </div>
                                            <div style="width: 100%" align="left">
                                                <uc4:searchrsltproducts ID="searchrsltproducts1" runat="server" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
