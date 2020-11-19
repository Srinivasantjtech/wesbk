<%@ Page Language="C#" AutoEventWireup="true" Inherits="AlertMessage"  Culture ="auto:en-US" UICulture ="auto" Codebehind="AlertMessage.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<title></title>
  <base target="self" />
</head>
<body>
    <form id="frmAlert" runat="server">
    <div>
    <table>
    <tr>
    <td height="100" valign="top" >
        <asp:Label ID="lblMessage" runat="server" meta:resourcekey="msgUserNotExists"  Class="lblResultSkin"></asp:Label>
    </td>
    </tr>
    <tr>
    <td align="center" height="50"  width="300" valign="bottom" >
  <%-- <asp:Button ID="btnClose" runat="server" meta:resourcekey="btnClose" OnClientClick="window.close()" OnClick="btnClose_Click" />--%>
     </td>
    </tr>
    </table>
    </div>
    </form>
</body>
</html>
