<%@ Page Title="" Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" CodeBehind="ResetChangePassword.aspx.cs" Inherits="WES.ResetChangePassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" runat="server">
  <asp:Panel ID ="pnlLogin" runat ="server" DefaultButton="btnChange" Height="500px">
    <table align =center  width="950" border="0" cellspacing="0" cellpadding="5">
        <tr>
          <td align="left" class="tx_1">
            <a href="home.aspx" style="color:#0099FF" class="tx_3">Home</a><font style="font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal"> / </font>Change Password
          </td>
        </tr>
        <tr>
          <td class="tx_3">
            <hr>
          </td>

        </tr>
      </table>
      <br />
      <table width="600px">
<tr>
    <td colspan="4" align="center"><span class="col-green">Welcome to WES Online Web Site</span></td>
  </tr>
  <tr><td style="height:10px" >
  </td></tr>
  <tr>
    <td colspan="4" align="center" >
     <span class="col-black">To get started please create a new login password </span>
    
    </td>
  </tr>
   
</table>  
<table style="text-align:right" width="600px"   >
<tr style="text-align:right" >
<td style="text-align:right" >
  <asp:Label ID="Label3" runat="server" Class="lblRequiredSkin"   Text="*"  Width="1px"></asp:Label>
                &nbsp;<asp:Label ID="Label4" runat="server"  Text="Required Fields" Class="lblNormalSkin"></asp:Label>
   
</td>
</tr>

</table>
    
   <%-- <Table cellSpacing="0" cellPadding="0" width="558" align="center"  border="0">
    <tbody>--%>
        <%--<tr>
            <td>--%>
                <table ID="tblBase" align="center" border="0" cellpadding="3" cellspacing="0" 
                    width="100%">
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblError" runat="server" class="lblErrorSkin" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" valign="middle">
                            <table ID="LoginTable" align="center" border="0" cellpadding="3" 
                                cellspacing="0" style="height:200px">
                                
                                <tr>
                                   
                                    <td align="right" style="width: 123px">
                                        <asp:Label ID="lblNewPassword" runat="server" Class="lblStaticSkin_new" 
                                            text="New Password:"></asp:Label>
                                             <asp:Label ID="Label1" runat="server" Class="lblRequiredSkin" text="*"></asp:Label>
                                            </td>
                                             <td>
                                       
                                    </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtNewPassword" runat="server" autocomplete="off" style="Width:225px; Height:20px"
                                                Class="textSkin" Font-Underline="False" MaxLength="15" onkeydown="CheckTextPassMaxLength(this,event,'15');" 
                                                 TextMode="Password"></asp:TextBox>
                                            <%--  <asp:RequiredFieldValidator ID="rfvNewPwd" Class="vldRequiredSkin" ControlToValidate ="txtNewPassword" runat="server" meta:resourcekey="rfvNewPwd"  ValidationGroup="Mandatory"></asp:RequiredFieldValidator>--%>
                                        </td>
                                        <td align="left" width="163px">
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                                ControlToValidate="txtNewPassword" Display="Dynamic" 
                                                ErrorMessage="Password must contain letters and numbers. Length needs to be between 6 and 15 characters." 
                                                 ValidationExpression="(?!^[0-9]*$)(?!^[a-zA-Z]*$)(?!^['!@&#$%^&quot;\\&*_\-\+\=\~\`\,\?\,\>\<\?\(\)\;\:\'\{\[\}\]\|\/.\s]*$)^[a-zA-Z0-9'!@&#$%^&quot;\\&*_\-\+\=\~\`\,\?\,\>\<\?\(\)\;\:\'\{\[\}\]\|\/.\s]{6,15}$"></asp:RegularExpressionValidator>
                                           <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                                ControlToValidate="txtNewPassword" Display="Dynamic" 
                                                ErrorMessage="Password must contain one number" 
                                                ValidationExpression=".*[0-9].*"></asp:RegularExpressionValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                                                ControlToValidate="txtNewPassword" Display="Dynamic" 
                                                ErrorMessage="Password must contain one letter" 
                                                ValidationExpression=".*[a-zA-Z].*"></asp:RegularExpressionValidator>--%>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                                ControlToValidate="txtNewPassword" ErrorMessage="New Password Required"></asp:RequiredFieldValidator>
                                        </td>
                                    
                                </tr>
                                <tr>
                                   
                                    <td align="right" style="height: 30px; width: 123px;">
                                        <asp:Label ID="lblConfirmPassword" runat="server" Class="lblStaticSkin_new" 
                                            Text="Confirm Password:"></asp:Label>
                                              <asp:Label ID="Label2" runat="server" Class="lblRequiredSkin" text="*"></asp:Label>
                                    </td>
                                     <td style="height: 30px">
                                      
                                    </td>
                                    <td align="left" style="height: 30px">
                                        <asp:TextBox ID="txtConfirmPassword" runat="server" autocomplete="off" style="Width:225px; Height:20px"
                                            Class="textSkin" MaxLength="15" onkeyDown="CheckTextPassMaxLength(this,event,'15');"  TextMode="Password"></asp:TextBox>
                                    </td>
                                    <td align="left">
                                        <%-- <asp:RequiredFieldValidator ID="rfvConPwd"  Class="vldRequiredSkin" ControlToValidate ="txtConfirmPassword" runat="server" meta:resourcekey="rfvConPwd" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>--%>
                                        <%--<asp:RequiredFieldValidator ID="rfvConPwd"  Class="vldRequiredSkin" ControlToValidate ="txtConfirmPassword" runat="server" meta:resourcekey="rfvConPwd" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>--%>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                            ControlToValidate="txtConfirmPassword" ErrorMessage="Confirm Password Required"></asp:RequiredFieldValidator>
                                        <br />
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" 
                                            ControlToCompare="txtNewPassword" ControlToValidate="txtConfirmPassword" 
                                            ErrorMessage="Passwords does not match" ValueToCompare="String"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="4">
                                        <table align="center" width="200px">
                                            <tr>
                                                <td>
                                                    <br />
                                                    <asp:Button ID="btnChange" runat="Server" CausesValidation="true" 
                                                        class="btn-updatepwd" OnClick="btnChange_Click" Text="Update Password" 
                                                        UseSubmitBehavior="true" />
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div ID="divMayus" align="center" style="visibility:hidden; color: red;">
                                                        <b>Caps Lock is on.</b></div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
           
                
               <%-- </td>
           
            
           
        </tr>--%>
        
      <%--   </tbody>
     </Table>    --%> 
    </asp:Panel>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
