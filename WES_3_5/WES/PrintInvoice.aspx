<%@ Page Language="C#" AutoEventWireup="true" Inherits="PrintInvoice"  Codebehind="PrintInvoice.aspx.cs" %>
<%@ Register TagPrefix ="WebCat" TagName ="Invoice" Src ="~/UI/Invoice.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<link href="css/stilos_.css" rel="stylesheet" type="text/css" />

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>WESAUSTRALASIA</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table align="center" width="558px">
        <tr>
            <td width="100%">
            <WebCat:Invoice ID="ucInvoice" runat ="server" />
            </td>
        </tr>
        <tr>
            <td align ="right">            
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
<script language="javascript" type="text/ecmascript">
window.print();
</script>
