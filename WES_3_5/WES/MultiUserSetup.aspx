<%@ Page Title="" Language="C#" MasterPageFile="~/FamilyMaster.master" AutoEventWireup="true" Inherits="MultiUserSetup" Codebehind="MultiUserSetup.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" runat="Server">
    <script language="javascript" type="text/javascript">
        function ShowTip(Obj) {
            var pos = $(Obj).offset();
            var width = $(Obj).width();
            var val = $(Obj).attr("alt");
            $("#popup").css({ "left": (pos.left + width) + "px", "top": (pos.top) + "px" });
            $("#popup").show();
            $("#popup div").html(val);
        }

        function HideTip(Obj) {
            $("#popup").hide();
        }

        function DeleteConfirm(UserID, UserName, UserRole, UserRoleDescription, UserLogin) {
            var agree = confirm("Are you sure want Delete this User?\nUser Login: " + UserLogin + "\nUser Name : " + UserName + "\nUser Role : " + UserRoleDescription);
            if (agree) {
                //location.href("MultiUserSetup.aspx?USERID=" + UserID + "&DELETEACTION=SUCCESS");
                var myUrl = "MultiUserSetup.aspx?USERID=" + UserID + "&DELETEACTION=SUCCESS";
                window.location.href = myUrl;
                return true;
            }
        }
    </script>
            <table width="100%" border="0" cellpadding="10" cellspacing="0">
                <tr>
                    <td width="100%" height="115" valign="top">
                        <p style="vertical-align: text-top">
                            <b class="txt_14" style="color:#333333">
                                <%=!EditMode ? "User Setup" : "Edit User Setting"%>
                            </b>
                        </p>
                        <p>
                            Allow members in your organisation to have access to the WES web site using there
                            own login details with<br />
                            the added benefit of setting different user permission levels. (eg. You may wish
                            to give a user access to the<br />
                            web site to view prices but not allow them to place an order)</p>
                        <p class="txt_14 blue1" style="color:#333333">
                            <b>Add New User</b></p>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse">
                            <tr>
                                <td width="100%" align="center">
                                    <asp:Label ID="MessageLabel" runat="server" CssClass="LabelStyle" Visible="false"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td width="254" valign="top">
                                    <table width="100%" border="0" cellpadding="1" cellspacing="0">
                                        <tr>
                                            <td width="100" height="24" valign="top">
                                                User Full Name<span class="red"> *</span>
                                            </td>
                                            <td width="135">
                                                <asp:TextBox ID="txtUserID" Visible="false" runat="server" />
                                                <asp:TextBox runat="server" ID="txtContact" Text="" Class="textSkin" Width="200px"
                                                    MaxLength="40" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="RequiredFieldValidator"
                                                    ControlToValidate="txtContact">Contact cannot be blank</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="24" valign="top">
                                                 Phone
                                                <%--<span class="red"> *</span>--%>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtPhone" Text="" Class="textSkin" Width="200px"
                                                    MaxLength="40" />
                                               <span style="color:white">.</span>
                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtPhone"
                                                    runat="server" ErrorMessage="RequiredFieldValidator">Phone cannot be blank</asp:RequiredFieldValidator>--%>
                                            </td>
                                        </tr>
                                           <tr>
                                            <td height="24" valign="top">
                                                Mobile Phone
                                                <%--<span class="red"> *</span>--%>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtmobilephone" Text="" Class="textSkin" Width="200px"
                                                    MaxLength="40" />
                                                <asp:RegularExpressionValidator ID="reMobileNumber" runat="server" ControlToValidate="txtmobilephone"
                                 ValidationExpression="^(04)\d{8}$"
                                Class="vldRegExSkin" Display="static" ValidationGroup="x" ErrorMessage="Mobile No must start with 04 and must be 10 digit"></asp:RegularExpressionValidator>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterMode="ValidChars" FilterType="Numbers" ValidChars="1234567890" TargetControlID="txtmobilephone" />
                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtPhone"
                                                    runat="server" ErrorMessage="RequiredFieldValidator">Phone cannot be blank</asp:RequiredFieldValidator>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="24" valign="top">
                                                User Email<span class="red"> *</span>
                                                <%--<span class="red"> *</span>--%>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtAEmail" Text="" Class="textSkin" Width="200px"
                                                    MaxLength="60" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="User Email cannot be empty"
                                                    ControlToValidate="txtAEmail">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="24" valign="top">
                                                User Login ID<span class="red"> *</span>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtLogin" Text="" Class="textSkin" AutoPostBack="false"
                                                    Width="200px" MaxLength="10"   />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="User ID cannot be empty"
                                                    ControlToValidate="txtLogin"></asp:RequiredFieldValidator>
                                                <br />
                                                <asp:Label ID="lblMess" runat="server" ForeColor="#FF3300" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="24" valign="top">
                                                User Password<span class="red"> *</span>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtPass" Text="" TextMode="Password" Class="textSkin"
                                                    Width="200px" MaxLength="15" onkeydown="CheckTextPassMaxLength(this,event,'15');" />
                                                      <asp:RegularExpressionValidator ID="RegularExpressionValidator2" Display="Dynamic"
                                                        ControlToValidate="txtPass"  ValidationExpression="(?!^[0-9]*$)(?!^[a-zA-Z]*$)(?!^['!@&#$%^&quot;\\&*_\-\+\=\~\`\,\?\,\>\<\?\(\)\;\:\'\{\[\}\]\|\/.\s]*$)^[a-zA-Z0-9'!@&#$%^&quot;\\&*_\-\+\=\~\`\,\?\,\>\<\?\(\)\;\:\'\{\[\}\]\|\/.\s]{6,15}$"
                                                        runat="server" ErrorMessage="Password must contain letters and numbers. Length needs to be between 6 and 15 characters."></asp:RegularExpressionValidator>
                                                   
                                                  <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Display="Dynamic"
                                                        ControlToValidate="txtPass"   ValidationExpression=".*[0-9].*"
                                                        runat="server" ErrorMessage="Password must contain one number"></asp:RegularExpressionValidator>
                                                   
                                                   <asp:RegularExpressionValidator ID="RegularExpressionValidator3" Display="Dynamic"
                                                        ControlToValidate="txtPass"   ValidationExpression=".*[a-zA-Z].*"
                                                        runat="server" ErrorMessage="Password must contain one letter"></asp:RegularExpressionValidator>--%>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Invalid Password"
                                                    ControlToValidate="txtPass"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="24" valign="top">
                                                Confirm Password<span class="red"> *</span>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtCPass" Text="" TextMode="Password" Class="textSkin"
                                                    Width="200px" MaxLength="15" onkeyDown="CheckTextPassMaxLength(this,event,'15');" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Passwords does not match"
                                                    ControlToValidate="txtCPass"></asp:RequiredFieldValidator><br />
                                                <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Passwords does not match"
                                                    ControlToCompare="txtPass" ControlToValidate="txtCPass" ValueToCompare="String"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td align="center">
                                                <% if (EditMode)
                                                   {%>
                                                <%--<asp:Button ID="Button1" runat="server" Text="Reset Password" onclick="btnRst_Click" />--%>
                                                <asp:ImageButton ID="btnRst" runat="server" Width="140" Height="34" ImageUrl="images/ResetPassword.png"
                                                    OnClick="btnRst_Click" />
                                                <%} 
                                                %>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td width="172" valign="top" valign="top">
                                    <table border="0" cellpadding="0" cellspacing="0" style="margin: auto;">
                                        <tr>
                                            <td height="24" colspan="2" valign="top" style="padding-bottom: 5px;">
                                                <b>User Permission Level</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="23" height="24" style="padding-bottom: 5px;">
                                                <asp:RadioButton ID="radBO" runat="server" GroupName="radRole" Checked="true" AutoPostBack="true"
                                                    OnCheckedChanged="radSA_CheckedChanged" />
                                            </td>
                                            <td style="padding-bottom: 5px;">
                                                Browse Only <a href="#">
                                                    <img alt="<p align=justify>User will be able to browse web site only and view prices.</p>"
                                                        onmouseover="javascript:ShowTip(this)" onmouseout="javascript:HideTip(this)"
                                                        src="images/help.png" width="11" height="11" /></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="23" height="24" style="padding-bottom: 5px;">
                                                <asp:RadioButton ID="radCO" runat="server" GroupName="radRole" AutoPostBack="true"
                                                    OnCheckedChanged="radSA_CheckedChanged" />
                                            </td>
                                            <td style="padding-bottom: 5px;">
                                                Create Order <a href="#">
                                                    <img alt="<p align=justify>User will only be able to browse web site and Create Orders. User cannot submit the order. The order must approved by user with submit order access.</p>"
                                                        onmouseover="javascript:ShowTip(this)" onmouseout="javascript:HideTip(this)"
                                                        src="images/help.png" width="11" height="11" /></a><br />
                                                (User Cannot Submit)
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="23" height="24" style="padding-bottom: 5px;">
                                                <asp:RadioButton ID="radAO" runat="server" GroupName="radRole" AutoPostBack="true"
                                                    OnCheckedChanged="radSA_CheckedChanged" />
                                            </td>
                                            <td style="padding-bottom: 5px;">
                                                Submit / Approve Order <a href="#">
                                                    <img alt="<p align=justify>User will only be able to browse web site, Create Orders and Submit orders for processing. User will also be able submit orders created by users orders waiting to be approved.</p>"
                                                        onmouseover="javascript:ShowTip(this)" onmouseout="javascript:HideTip(this)"
                                                        src="images/help.png" width="11" height="11" /></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="23" height="24" style="padding-bottom: 5px;">
                                                <asp:RadioButton ID="radSA" runat="server" GroupName="radRole" AutoPostBack="true"
                                                    OnCheckedChanged="radSA_CheckedChanged" Enabled="false" />
                                            </td>
                                            <td style="padding-bottom: 5px;">
                                                Admin User <a href="#">
                                                    <img alt="<p align=justify>Give user Admin capilities which include browse, create and submit orders. <br /><br />Admin user will have access to user settings page and be able to add, edit and delete users. <br /><br />This should be enabled for authorised users with the company</p>"
                                                        onmouseover="javascript:ShowTip(this)" onmouseout="javascript:HideTip(this)"
                                                        src="images/help.png" width="11" height="11" /></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding-bottom: 5px;">
                                                <asp:CheckBox runat="server" ID="chkDrop" Enabled="True" />
                                                
                                            </td>
                                            <td style="padding-bottom: 5px;">
                                                Create Drop Shipment <a href="#">
                                                    <img alt="<p align=justify>Grants user the ability to create Drop Shipment orders. This needs to be enabled. Please click here for more details about Drop Shipment ordering.</p>"
                                                        onmouseover="javascript:ShowTip(this)" onmouseout="javascript:HideTip(this)"
                                                        src="images/help.png" width="11" height="11" /></a>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td width="154" valign="top">
                                    <table border="0" cellpadding="0" cellspacing="0" style="margin: auto;">
                                        <tr>
                                            <td height="24" colspan="2" valign="top" style="padding-bottom: 5px;">
                                                <b>Email Notifications</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="23" height="24" style="padding-bottom: 5px;">
                                                <asp:CheckBox runat="server" ID="chkDespatch" />
                                            </td>
                                            <td width="120" style="padding-bottom: 5px;">
                                                Despatch Info <a href="#">
                                                    <img alt="<p align=justify>User will be sent out despatch information once order has been processed.</p>"
                                                        onmouseover="javascript:ShowTip(this)" onmouseout="javascript:HideTip(this)"
                                                        src="images/help.png" width="11" height="11" /></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="24" style="padding-bottom: 5px;">
                                                <asp:CheckBox runat="server" ID="chkAcc" />
                                            </td>
                                            <td style="padding-bottom: 5px;">
                                                Account Info <a href="#">
                                                    <img alt="<p align=justify>User will be sent statements and any other account related information.</p>"
                                                        onmouseover="javascript:ShowTip(this)" onmouseout="javascript:HideTip(this)"
                                                        src="images/help.png" width="11" height="11" /></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="24" style="padding-bottom: 5px;">
                                                <asp:CheckBox runat="server" ID="chkNewsUpd" Checked="true" />
                                            </td>
                                            <td style="padding-bottom: 5px;">
                                                WES News Updates <a href="#">
                                                    <img alt="<p align=justify>User will be sent product updates, special / promotions and highlights</p>"
                                                        onmouseover="javascript:ShowTip(this)" onmouseout="javascript:HideTip(this)"
                                                        src="images/help.png" width="11" height="11" /></a>
                                            </td>
                                        </tr>
                                        <tr runat="server" ID="MailToAdmin" visible="false">
                                           
                                            
                                            <td height="24" style="padding-bottom: 5px;">
                                                <asp:CheckBox runat="server" ID="chkMailToAdmin" Checked="false" />
                                            </td>
                                            <td style="padding-bottom: 5px;">
                                                Recievce Copies of orders placed by all users <a href="#">
                                                    <img alt="<p align=justify>Receive copies of orders submitted placed online by any of the users on your account</p>"
                                                        onmouseover="javascript:ShowTip(this)" onmouseout="javascript:HideTip(this)"
                                                        src="images/help.png" width="11" height="11" /></a>
                                            </td>
                                          
                                        </tr>
                                        <tr>
                                            <td height="60" colspan="2" align="center">
                                                <% if (IsAdminUser() && !EditMode)
                                                   { %>
                                                <asp:ImageButton ID="btnAdd" Width="140" runat="server" Height="34" ImageUrl="images/add_new_user.png"
                                                    OnClick="btnAdd_Click" />
                                                <% }
                                                   else if (EditMode)
                                                   {%>
                                                <%--<asp:Button ID="btnRst" runat="server" Text="Reset Password" 
                    onclick="btnRst_Click" />--%>
                                                <asp:ImageButton ID="btnSave" runat="server" Width="140" Height="34" ImageUrl="images/save_settings.png"
                                                    OnClick="btnSave_Click" />
                                                <%} %>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td height="31" valign="middle">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <% if (IsAdminUser() && !EditMode)
                           { %>
                        <table width="100%" border="0" cellpadding="0" cellspacing="0" style="background-color: Black">
                            <tr style="font-weight: bold;">
                                <td width="19" style="background-color: Gray;color:White;" height="20" valign="top">
                                    <!--DWLayoutEmptyCell-->
                                    &nbsp;
                                </td>
                                <td width="" style="background-color: Gray;color:White;" align="center">
                                    Login name
                                </td>
                                <td width="128" style="background-color: Gray;color:White;" align="center">
                                    Contact Name
                                </td>
                                <td width="188" style="background-color: Gray;color:White;" align="center">
                                    Email Address
                                </td>
                                <td width="94" style="background-color: Gray;color:White;" align="center">
                                    Account Type
                                </td>
                                <td width="72" style="background-color: Gray;color:White;" align="center">
                                    Edit
                                </td>
                                <td width="74" style="background-color: Gray;color:White;" align="center">
                                    Delete
                                </td>
                            </tr>
                            <%=GetUsers()%>
                            <%} %>
                        </table>
                    </td>
                </tr>
            </table>
            <div id="popup" style="position: absolute; display: none; width: 200px; height: 100px;">
                <table border="0" cellpadding="0" cellspacing="2" style="background-color: #189DDC;
                    width: 200px; height: 100px">
                    <tr>
                        <td>
                            <table border="0" cellpadding="5" cellspacing="0">
                                <tr>
                                    <td bgcolor="#ffffff" valign="top">
                                        <img src="images/help.png" width="16" height="16" />
                                    </td>
                                    <td style="width: 200px; height: 100px; background-color: #ffffff;" valign="middle"
                                        align="center">
                                        <div id="content">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <%--<div id="PopDiv" align="center" class="container">
                <asp:Panel ID="ModalPanel" runat="server" CssClass="modalPopup" Visible="false">
                    <asp:UpdatePanel ID="udpInnerUpdatePanel" runat="Server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;
                                font-family: Arial; font-size: 12px; font-weight: bold; color: Green;" align="center">
                                <tr>
                                    <td width="100%" colspan="4">
                                        <p style="text-align: center;">
                                            No Email Address was entered. It is recommended your email address is added for
                                            easy user login and communication. Are you sure you wish to proceed?
                                        </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="30%">
                                        &nbsp;
                                    </td>
                                    <td width="20%">
                                        <asp:Button ID="btnOk" runat="server" Text="Yes" CssClass="ButtonStyle" OnClick="btnOk_Click" />
                                    </td>
                                    <td width="20%" align="center">
                                        <asp:Button ID="btnCancel" runat="server" Text="No" CssClass="ButtonStyle" OnClick="btnCancel_Click" />
                                    </td>
                                    <td width="30%">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100%" colspan="4">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnOk" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </asp:Panel>
            </div>--%>
   <%--     </ContentTemplate>--%>
        <%--<Triggers>--%>
           <%-- <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />--%>
           <%-- <asp:AsyncPostBackTrigger ControlID="txtLogin" EventName="TextChanged" />--%>
           <%-- <asp:AsyncPostBackTrigger ControlID="btnRst" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="radBO" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="radCO" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="radAO" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="radSA" EventName="CheckedChanged" />--%>
      <%--  </Triggers>--%>
  <%--  </asp:UpdatePanel>--%>
</asp:Content>
