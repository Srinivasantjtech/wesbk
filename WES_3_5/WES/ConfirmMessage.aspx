<%@ Page Language="C#" MasterPageFile="~/mainpage.master" AutoEventWireup="true"  Inherits="RegistrationResult"
    Culture="en-US" UICulture="en-US" Codebehind="ConfirmMessage.aspx.cs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
<script type = "text/javascript" >
    window.history.forward(1);
    window.location.hash = "no-back-button";
    window.location.hash = "Again-No-back-button"; //again because google chrome don't insert first hash into history
    window.onhashchange = function () { window.location.hash = "no-back-button"; }
    
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="header" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="leftnav" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" runat="Server" >
    <table align="center" width="558" border="0" cellspacing="0" cellpadding="5">
        <tr>
            <td align="left" class="tx_1">
                <a href="home.aspx" style="color: #0099FF" class="tx_3">Home</a><font style="font-family: Arial, Helvetica, sans-serif;
                    font-weight: bolder; font-size: small; font-style: normal"> / </font>Confirm
                Message
            </td>
        </tr>
        <tr>
            <td class="tx_3">
                <hr>
            </td>
        </tr>
    </table>
    <br />
    <% if (Request.QueryString["Result"] != null)
       {
           if (Request.QueryString["Result"] == "NOECOMMERCE")
           {%>
    <asp:Label ID="lblmsg" runat="server" meta:resourcekey="lblmsg"></asp:Label>
    <% }
             if (Request["Result"] == "REGISTRATION")
             {
                 string regHtml = "<table cellspacing=\"0\" width=\"588px\" cellpadding=\"0\" style=\"background-color: #F2F2F2; border: thin solid #E7E7E7\"><tr><td  class=\"tx_7_blue\" align=\"left\" colspan=\"2\">Registration Confirmation.</td></tr><tr><td  class=\"tx_7_blue\" align=\"left\" colspan=\"2\">&nbsp;</td></tr><tr><td width=\"2%\">&nbsp;</td><td class=\"tx_1\" align=\"left\"><b>Your Details have been received for processing.</b><br /><b>You should receive an email / phone call from us within the next 1-2 business days.</b><br /><br /><b>Should you not hear from us in this time frame please contact us on:</b><br /><br /><b>Email: <a href=\"mailto:sales@wes.net.au\" class=\"tx_3\">sales@wes.net.au</a></b><br /><b>Phone: +61 2 9797-9866</b><br /></td></tr></table>";
                 Response.Write(regHtml);
             }
         }%>
    <asp:Image ID="Imgstat" runat="server" Visible="false" />
    <asp:Label ID="lblConfirmmsg" runat="Server" Class="lblResultSkin" style="font-size: 14px;"></asp:Label>
    <asp:Label ID="lblErrormsg" runat="Server" Class="lblErrorSkin"></asp:Label>
    <asp:Label ID="lblCartEmpty" runat="Server" Class="lblErrorSkin"></asp:Label>
    <asp:HyperLink ID="lnkResult" Class="HLCommonLinkSkin" runat="server" NavigateUrl="Login.aspx"></asp:HyperLink>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="rightnav" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="footer" runat="Server">
</asp:Content>
