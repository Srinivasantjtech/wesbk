<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewPdf_EBook.aspx.cs" Inherits="WES.ViewPdf_EBook" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        #ebook
        {
            height: 900px;
            width: 1000px;
        }
    </style>
</head>
<body style="margin: 0px; padding:0px; height: 100%; overflow:hidden;">
    <form id="form1" runat="server" style="height: 100%">
    <div style=" height: 100%">
     <table style="width: 100%; height: 100%" cellspacing="0"  cellpadding="0">
        <tr>
            <td valign="top" align="left" height="100%">
    <iframe id="ebook" runat="server" visible="false" style="overflow:hidden;height:100%;width:100%;margin: 0px; padding:0px;" 
                     src="attachments\media\wesnews\WN515AMA\WN515AMA.htm"   width="100%" height="100%" frameborder="0" scrolling="no">
    
    </iframe>
    </td>
    </tr>
    </table>
    </div>
    </form>
</body>
</html>
