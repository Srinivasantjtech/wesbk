<%@ Page Language="C#" MasterPageFile="~/mainpage.master" AutoEventWireup="true" Inherits="ForgotUserName"  Culture="en-US"
    UICulture="en-US" Codebehind="ForgotUserName.aspx.cs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="header" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="leftnav" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" runat="Server">
    <table  width="558" border="0" cellspacing="0" cellpadding="0">
     <tr>
            <td align="left" class="tx_1">
                <a href="home.aspx" style="color: #0099FF" class="tx_3">Home</a><font style="font-family: Arial, Helvetica, sans-serif;
                    font-weight: bolder; font-size: small; font-style: normal"> / </font>Forgot
                Username
            </td>
        </tr>
           <tr>
            <td class="tx_3">
                <hr>
            </td>
        </tr>
 
     
     <tr>
    <td >
      
         
        
   <%-- <table width="100%" border="0" cellspacing="0" cellpadding="0" >
      
        <tr>
            <td>
                <table id="tblError" width="100%" runat="server" align="left">
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblError" runat="server" Class="lblErrorSkin"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>--%>
   
    <br />
    <asp:Panel ID="pnlUser" runat="server" DefaultButton="btnSubmit">
        <table id="tblUserID" runat="server"  width="100%" border="0" style="border-width: 2px;border-style: solid;border-color: #ECE9D8;height: 75px;"  >
            <tr>
        <td class="tx_6" height="20px" background="images/17.gif" align="left">
           <asp:Label ID="lblAssistance" runat="server" Text="User ID Assistance" ></asp:Label>
        </td>
     </tr>
       <%--<tr>         
             <td align="left">
                 <asp:Label ID="lblcustype" runat="server" Text="CustomerType :"  Width="145px" Class="lblStaticSkin" ></asp:Label>
                <asp:DropDownList NAME="CusType" Width="165px" ID="CusType" runat="server" CssClass="inputtxt">
                    <asp:ListItem Text="Dealer" Value="Dealer" Selected="True" >Dealer</asp:ListItem>
                    <asp:ListItem Text="Retailer" Value="Retailer">Retailer</asp:ListItem>                                           
                </asp:DropDownList>
             </td>
            </tr>--%>
     <tr>
            <td>
                <table id="tblError" width="100%" runat="server" align="left">
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblError" runat="server" Class="lblErrorSkin"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
            <tr>
                <td>
                    <table width="100%" >
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
                                <asp:Label ID="lblUserID" runat="server" runat="server" meta:resourcekey="lblUserID" Class="lblStaticSkin"
                                    Width="140px"></asp:Label>
                            </td>
                            <td align="left">
                                &nbsp;<asp:TextBox autocomplete="off" ID="txtUserID" runat="server"  CssClass="inputtxt" Width="160px"  MaxLength="50"  
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
                    <asp:Button ID="btnUser" runat="server"  meta:resourcekey="btnUser" OnClick="btnUser_Click"   class="btnNormalSkin" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlSQ" runat="server" DefaultButton="btnSubmit">
        <table id="tblSecurityQuestion" runat="Server" visible="false" class="BaseTblBorder"
            width="100%" cellspacing="1">
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
                    <asp:TextBox autocomplete="off" ID="txtYourAnswer" runat="server" CssClass="inputtxt" Class="textSkin"
                        AutoCompleteType="Disabled"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table id="tblSecurityQuestionbtn" runat="server" width="100%" border="0" visible="false">
            <tr>
                <td colspan="2" align="right">
                    <asp:Button ID="btnSubmit" runat="server" meta:resourcekey="btnSubmit" OnClick="btnSubmit_Click"
                        Class="btnNormalSkin" />
                </td>
            </tr>
        </table>
    </asp:Panel>    
    
              </td>
        </tr>
    </table>
    
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="rightnav" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="footer" runat="Server">
</asp:Content>
