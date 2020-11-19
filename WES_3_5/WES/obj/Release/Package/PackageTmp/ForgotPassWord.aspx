<%@ Page Language="C#" MasterPageFile="~/mainpage.master" AutoEventWireup="true" Inherits="ForgotPassWord"  Culture="en-US"
    UICulture="en-US" Codebehind="ForgotPassWord.aspx.cs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
  <script type="text/javascript">
    function CheckCusType() {

        switch (document.getElementById("ctl00_maincontent_CusType").value) {
            case 'Retailer':
                ShowRetailer();
                break;
            case 'Dealer':
                ShowDealer();
                break;
           
        }
    }

    function ShowDealer() {
        document.getElementById("ctl00_maincontent_txtLoginName").style.display = "block";
        document.getElementById("ctl00_maincontent_lblLoginName").style.display = "block";
    }

    function ShowRetailer() {
        document.getElementById("ctl00_maincontent_txtLoginName").style.display = "none";
        document.getElementById("ctl00_maincontent_lblLoginName").style.display = "none";
    }
   
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="header" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="leftnav" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" runat="Server">
    <table align="center" width="558" border="0" cellspacing="0" cellpadding="5">
        <tr>
            <td align="left" class="tx_1">
                <a href="home.aspx" style="color: #0099FF" class="tx_3">Home</a><font style="font-family: Arial, Helvetica, sans-serif;
                    font-weight: bolder; font-size: small; font-style: normal"> / </font>Forgot
                Password
            </td>
        </tr>
        <tr>
            <td class="tx_3">
                <hr>
            </td>
        </tr>
        <tr>
            <td>
                <table id="tblError" width="558" runat="server" align="left">
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblError" runat="server" Class="lblErrorSkin"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="pnlUser" runat="server" DefaultButton="btnSubmit">
        <table id="tblUserID" runat="server" class="BaseTblBorder" width="558" border="0">
            <tr>
                <td class="tx_6" height="20px" background="images/17.gif" align="left">
                    <asp:Label ID="lblAssistance" runat="server" meta:resourcekey="lblAssistance" ></asp:Label>
                </td>
            </tr>
            <%--<tr>
           
             <td align="left">
                 <asp:Label ID="lblcustype" runat="server" Text="CustomerType :"  Width="145px" Class="lblStaticSkin"></asp:Label>
                <asp:DropDownList NAME="CusType" Width="165px" ID="CusType" runat="server" CssClass="inputtxt">
                    <asp:ListItem Text="Dealer" Value="Dealer" Selected="True" >Dealer</asp:ListItem>
                    <asp:ListItem Text="Retailer" Value="Retailer">Retailer</asp:ListItem>                                           
                </asp:DropDownList>
             </td>
            </tr>--%>
            <tr>
                <td height="40" align="left">
                    <asp:Label ID="lblAssistanceMessage" runat="server" Class="lblNormalSkin" meta:resourcekey="lblAssistanceMessage"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="558">
                            <tr>
           
                                <td align="left" style="width: 77px">
                                <asp:Label ID="lblcustype" runat="server" Text="CustomerType :"   Class="lblStaticSkin"></asp:Label>
                                </td>
                                <td align="left">
                                  <asp:DropDownList NAME="CusType" Width="165px" ID="CusType" runat="server" CssClass="inputtxt" >
                                <asp:ListItem Text="Dealer" Value="Dealer" Selected="True" >Dealer</asp:ListItem>
                                <asp:ListItem Text="Retailer" Value="Retailer">Retailer</asp:ListItem>                                           
                                </asp:DropDownList>
                                </td>
                            </tr>
                        <tr>
                            <td style="width: 77px" align="left">
                                <asp:Label ID="lblLoginName" runat="server" meta:resourcekey="lblLoginName" Class="lblStaticSkin"
                                    Width="140px"></asp:Label>
                            </td>
                            <td align="left">
                                &nbsp;<asp:TextBox autocomplete="off" ID="txtLoginName" runat="server" Width="160px"  MaxLength="10"  
                                    Class="textSkin" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 77px" align="left">
                                <asp:Label ID="lblUserID" runat="server" meta:resourcekey="lblUserID" Class="lblStaticSkin"
                                    Width="140px"></asp:Label>
                            </td>
                            <td align="left">
                                &nbsp;<asp:TextBox autocomplete="off" ID="txtUserMail" runat="server" Width="160px"
                                    Class="textSkin" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table width="558" border="0" id="tblusedidbtn" runat="server">
            <tr>
                <td align="right" colspan="2">
                    <asp:Button ID="btnUser" runat="server" meta:resourcekey="btnUser" OnClick="btnUser_Click"
                        Class="btnNormalSkin" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlSQ" runat="server" DefaultButton="btnSubmit">
        <table id="tblSecurityQuestion" runat="Server" visible="false" class="BaseTblBorder"
            width="558" cellspacing="1">
            <tr>
                <td colspan="2" class="TableRowHead" align="left">
                    <asp:Label ID="lblSecurityHeader" runat="server" meta:resourcekey="lblSecurityHeader"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" height="40" align="left">
                    <asp:Label ID="lblSecurityQuestion" runat="server" Class="lblNormalSkin"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 107px" align="left">
                    <asp:Label ID="lblYourAnswer" runat="server" meta:resourcekey="lblYourAnswer" Class="lblStaticSkin"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox autocomplete="off" ID="txtYourAnswer" runat="server" Class="textSkin"
                        AutoCompleteType="Disabled"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table id="tblSecurityQuestionbtn" runat="server" width="558px" border="0" visible="false">
            <tr>
                <td colspan="2" align="right">
                    <asp:Button ID="btnSubmit" runat="server" meta:resourcekey="btnSubmit" OnClick="btnSubmit_Click"
                        Class="btnNormalSkin" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="rightnav" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="footer" runat="Server">
</asp:Content>
