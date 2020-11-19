<%@ Page Title="" Language="C#" MasterPageFile="~/HomePage.master" AutoEventWireup="true" Inherits="homepageST" EnableEventValidation="false" Codebehind="home.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="Server">
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="footer" runat="Server">
    <script type="text/javascript" lang="javascript">
        function MouseHover(ID) {
            switch (parseInt(ID)) {
                case 1:
                    document.getElementById("ctl00_footer_ContinueOrder").style.backgroundColor = "#009F00";
                    break;
                case 2:
                    document.getElementById("ctl00_footer_ClearOrder").style.backgroundColor = "red";
                    break;
                case 3:
                    document.getElementById("ctl00_footer_btnCancel").style.backgroundColor = "red";
                    break;
            }
        }

        function MouseOut(ID) {
            switch (parseInt(ID)) {
                case 1:
                    document.getElementById("ctl00_footer_ContinueOrder").style.backgroundColor = "#1589FF";
                    break;
                case 2:
                    document.getElementById("ctl00_footer_ClearOrder").style.backgroundColor = "#1589FF";
                    break;
                case 3:
                    document.getElementById("ctl00_footer_btnCancel").style.backgroundColor = "#1589FF";
                    break;
            }
        }
        
    </script>
    <asp:Button ID="btnHiddenTestPopupExtender" runat="server" Style="display: none;
        visibility: hidden"></asp:Button>
    <div id="PopupOrderMsg" align="center" runat ="server">
        <asp:Panel ID="ModalPanel" runat="server" CssClass="PopUpDisplayStylehpop">
            <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;"
                align="center">
                <tr style="height: 5px">
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td width="100%" align="center" colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td width="100%" align="center" colspan="3" class="TextContentStyle">
                        There has been Product items found still in your shopping cart from your last login,
                        <br />
                        Would you like to continue with this order?
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td width="100%" align="center" colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr style="height: 5px">
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td width="45%" align="right">
                        <asp:Button ID="ContinueOrder" runat="server" Text="Yes, Continue with Previous Order"
                            Width="205px" CssClass="ButtonStylehpop" OnClick="btnContinueOrder_Click" />
                    </td>
                    <td width="10%">
                        &nbsp;
                    </td>
                    <td width="45%" align="left">
                        <asp:Button ID="ClearOrder" runat="server" Text="No, Cancel Previous Order" Width="165px"
                            CssClass="ButtonStylehpop" OnClick="btnClearOrder_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
     <div id="PopupRetailerLoginMsg" align="center" runat ="server">
        <asp:Panel ID="ModalPanel1" runat="server" CssClass="PopUpDisplayStylehpop">
            <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;"
                align="center">
                <tr style="height: 5px">
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td width="100%" align="center" colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td width="100%" align="center" colspan="3" class="TextContentStyle">
                       Your Account Has Not Been Activated!
                        <br />
                        Please check your email for an email from us containing an activation / confirmation link.
                        <br />
                        If you would like us to send you the Activation Email again. <a Href="ConfirmMessage.aspx?Result=REMAILACTIVATION" class="toplinkatest">Please Click Here</a>
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td width="100%" align="center" colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr style="height: 5px">
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr style="height: 10px"> 
                    <td width="100%" align="center" colspan="3">
                        <asp:Button ID="btnCancel" runat="server" Text="Close"
                            Width="205px" CssClass="ButtonStylehpop" OnClick="btnCancel_Click" />
                    </td>                   
                </tr>
            </table>
        </asp:Panel>
    </div>
  
</asp:Content>
