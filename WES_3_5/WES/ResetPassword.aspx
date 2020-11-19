﻿<%@ Page Language="C#" MasterPageFile="~/mainpage.master" AutoEventWireup="true" Inherits="ResetPassword"  Culture="en-US"
    UICulture="en-US" Codebehind="ResetPassword.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function CheckBeforeUpdate() {
            var retvalue = true;
            var EmptyStatus = false;

            window.document.getElementById("ctl00_maincontent_txtNewPassword").style.borderColor = "ActiveBorder";
            window.document.getElementById("ctl00_maincontent_txtConfirmPassword").style.borderColor = "ActiveBorder";
            
            if (document.getElementById("ctl00_maincontent_txtNewPassword").value == null || document.getElementById("ctl00_maincontent_txtNewPassword").value == '') {
                alert('New Password cannot be empty!');
                window.document.getElementById("ctl00_maincontent_txtNewPassword").style.borderColor = "red";
                document.getElementById("ctl00_maincontent_txtNewPassword").focus();
                EmptyStatus = true;
                retvalue = false;
            }

            if (document.getElementById("ctl00_maincontent_txtConfirmPassword").value == null || document.getElementById("ctl00_maincontent_txtConfirmPassword").value == '') {
                alert('Confirm Password cannot be empty!');
                window.document.getElementById("ctl00_maincontent_txtConfirmPassword").style.borderColor = "red";
                document.getElementById("ctl00_maincontent_txtConfirmPassword").focus();
                EmptyStatus = true;
                retvalue = false;
            }

            if (document.getElementById("ctl00_maincontent_txtNewPassword").value != document.getElementById("ctl00_maincontent_txtConfirmPassword").value && EmptyStatus == false) {
                alert('New Password and Confirm Password do not Match');
                document.getElementById("ctl00_maincontent_txtConfirmPassword").focus();
                retvalue = false;
            }

            return retvalue;
        }
    </script>


    <script type="text/javascript" >

        function keyboardActions(event) {
            if (event.keyCode == 13) {

                eval($("#<%=btnUpdate.ClientID %>").trigger('click'));
                return false;
            }

        }

        $(document).ready(function () {
            if ($.browser.mozilla) {
                $("#<%=txtNewPassword.ClientID %>").keypress(keyboardActions);
                $("#<%=txtConfirmPassword.ClientID %>").keypress(keyboardActions);
                
            } else {
                $("#<%=txtNewPassword.ClientID %>").keydown(keyboardActions);
                $("#<%=txtConfirmPassword.ClientID %>").keydown(keyboardActions);
                
            }
        });

  </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="header" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="leftnav" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" runat="Server">
    <table width="558px" cellspacing="5" cellpadding="5" border="0" style="border-collapse: collapse;
        text-align: left;" align="center">
        <tr>
            <td align="left" class="tx_1">
                <a href="home.aspx" style="color: #0099FF" class="tx_3">Home</a><font style="font-family: Arial, Helvetica, sans-serif;
                    font-weight: bolder; font-size: small; font-style: normal"> / </font>Reset Password
            </td>
        </tr>
        <tr>
            <td class="tx_3">
                <hr />
            </td>
        </tr>
        <tr>
            <td width="558px" align="center">
                <asp:Label ID="lblErrorMessage" runat="server" Class="lblErrorSkin" Visible="false"  style="color:red" ></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="pnlUser" runat="server" DefaultButton="btnSubmit" Visible="false">
        <table id="tblUserID" runat="server" class="BaseTblBorder" width="558px" border="0">
            <tr style="display: none;">
                <td>
                    <asp:Label ID="lblUserID" runat="server" Text="User ID"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtLoginName" runat="server" ReadOnly="true" CssClass="TextBoxStyleRP" />
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblCompanyAccountNo" runat="server" Text="Company Account No"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtCompanyID" runat="server" MaxLength="6" CssClass="TextBoxStyleRP" />
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblEmailAddress" runat="server" Text="EMail Address"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="TextBoxStyleRP" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnSubmit" runat="server" CssClass="ButtonStyleRP" Text="Submit" OnClick="btnSubmit_Click" CausesValidation="false" />
                    <asp:Button ID="btnHidden" runat="server" CssClass="HiddenButtonRP" CausesValidation="true" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <div id="PopDiv" class="containerRP">
        <asp:Panel ID="ResetPWDPopupPanel" runat="server" >

<div class="fpwd_panel" runat="server" id="divmain"> 
                    <h3 class="title1 alaincenter">Reset Your Password</h3> 
     <p style="font-size:12px;font-weight:bold">Please enter your new password using a mix of letters and numbers with a length of 6 to 15 characters.</p>
 <div class="clear"></div>
					<div class="center_block">
                       
                        <p><span class="label16">New Password</span> 
                            
                           <asp:TextBox ID="txtNewPassword" runat="server" CssClass="input_fpwd" TextMode="Password" MaxLength="15" onkeydown="CheckTextPassMaxLength(this,event,'15');"  />
                               
                             <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Display="Dynamic" style="color:red"
                                                        ControlToValidate="txtNewPassword"   ValidationExpression="(?!^[0-9]*$)(?!^[a-zA-Z]*$)(?!^['!@&#$%^&quot;\\&*_\-\+\=\~\`\,\?\,\>\<\?\(\)\;\:\'\{\[\}\]\|\/.\s]*$)^[a-zA-Z0-9'!@&#$%^&quot;\\&*_\-\+\=\~\`\,\?\,\>\<\?\(\)\;\:\'\{\[\}\]\|\/.\s]{6,15}$"
                                                        runat="server" ErrorMessage="Password must contain letters and numbers. Length needs to be between 6 and 15 characters."></asp:RegularExpressionValidator>
                                    <div class="clear"></div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"  style="color:red" ErrorMessage="New Password Required"
                 ControlToValidate="txtNewPassword" ></asp:RequiredFieldValidator> 
                        </p> 
                        <p><span class="label16">Confirm Password</span>    

  <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="input_fpwd" TextMode="Password" MaxLength="15" onkeyDown="CheckTextPassMaxLength(this,event,'15');"  />   
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Confirm Password Required"
                 ControlToValidate="txtConfirmPassword" style="color:red"  ></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Passwords does not match"
                  ControlToCompare="txtNewPassword" ControlToValidate="txtConfirmPassword" ValueToCompare="String" style="color:red" ></asp:CompareValidator>            
                       


                        </p> 
                    </div>
                        
                    <p style="height:30px; width:200px; margin:0 auto; text-align:center;">  
                

                           <asp:Button ID="btnUpdate" runat="server" Text="UPDATE" CssClass="buttonlogin" OnClick="btnUpdate_Click" CausesValidation="true" />
                    </p>  
  
    <p  style="height:30px; width:200px; margin:0 auto; text-align:center;">

          <asp:Label ID="lblPwdErrorMessage" runat="server" Class="lblErrorSkin"  style="color:red" Visible="false"></asp:Label>
    </p>
                </div>

          <table width="558" height="30" border="0" id="tblusedidbtn" runat="server" visible="false" style="text-align:center" >
          
            <tr>
                <td>
                <asp:Label ID="lblsucess" runat="server" Text="Your password has been reset successfully" Font-Size ="14px" ForeColor="green"></asp:Label>

         </td>
</tr>
              <tr><td>
                  <p style="color:#0071CF;font-size:12px" >
                  <a id="hflogin" class="HyperLinkStyle" href="Login.aspx" style="color:#0071CF;font-size:12px" >Continue here to continue to login with your new password. </a>
                      </p>
               
            </td>
                  </tr>
            
        </table>

               </asp:Panel>
        </div>

           <%--   <div class="containerRP">
                <table width="100%" cellspacing="0" cellpadding="5" border="0" style="border-collapse: collapse;
                    text-align: left;">
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Label ID="lblPwdErrorMessage" runat="server" Class="lblErrorSkin" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TableColumnStyleRP">
                            <asp:Label ID="lblNewPassword" runat="server" Text="New Password" CssClass="LabelStyleRP"></asp:Label>
                        </td>
                        <td class="TableColumnStyleRP">
                           
                        </td>
                        
                    </tr>
                    <tr>
                    <td class="TableColumnStyleRP">
                    </td>
                    <td  colspan="2">
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" Display="Dynamic"
                                                        ControlToValidate="txtNewPassword"   ValidationExpression="(?!^[0-9]*$)(?!^[a-zA-Z]*$)(?!^['!@&#$%^&quot;\\&*_\-\+\=\~\`\,\?\,\>\<\?\(\)\;\:\'\{\[\}\]\|\/.\s]*$)^[a-zA-Z0-9'!@&#$%^&quot;\\&*_\-\+\=\~\`\,\?\,\>\<\?\(\)\;\:\'\{\[\}\]\|\/.\s]{6,15}$"
                                                        runat="server" ErrorMessage="Password must contain letters and numbers. Length needs to be between 6 and 15 characters."></asp:RegularExpressionValidator>
                                                   
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Display="Dynamic"
                                                        ControlToValidate="txtNewPassword"   ValidationExpression=".*[0-9].*"
                                                        runat="server" ErrorMessage="Password must contain one number"></asp:RegularExpressionValidator>
                                                   
                                                   <asp:RegularExpressionValidator ID="RegularExpressionValidator3" Display="Dynamic"
                                                        ControlToValidate="txtNewPassword"   ValidationExpression=".*[a-zA-Z].*"
                                                        runat="server" ErrorMessage="Password must contain one letter"></asp:RegularExpressionValidator>
                                                       
                        </td>
                    </tr>
                    <tr>
                        <td class="TableColumnStyleRP">
                            <asp:Label ID="lblConfirmPassword" runat="server" Text="Confirm Password" CssClass="LabelStyleRP"></asp:Label>
                        </td>
                        <td class="TableColumnStyleRP">
                                    
                        </td>
                 

                    </tr>
                  <tr>
                     <td class="TableColumnStyle" height="10px">
                    </td>
                      <td  colspan="2"  height="10px">
                       <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Confirm Password Required"
                 ControlToValidate="txtConfirmPassword" ></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Passwords does not match"
                  ControlToCompare="txtNewPassword" ControlToValidate="txtConfirmPassword" ValueToCompare="String"></asp:CompareValidator>
                      </td>
                    </tr>
                <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="ButtonStyleRP" OnClick="btnUpdate_Click" CausesValidation="true" />
                      
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
    </div>--%>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="rightnav" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="footer" runat="Server">
</asp:Content>
