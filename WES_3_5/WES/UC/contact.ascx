<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_contact" EnableTheming="true" Codebehind="contact.ascx.cs" %>
<table cellSpacing="0" cellPadding="0"  align="left" border="0">
      <tbody>
        <tr>
            <td width=10 style="height: 10px"><img height="17" src="Images/tbl_topLeft.gif" width=10>
            </td>
            <td background="Images/tbl_top.gif" style="height: 10px; width: 528px;">
                <img height="17" src="Images/tbl_top.gif" width="10">
            </td>
            <td width="10" style="height: 10px"><img height="17" src="Images/tbl_topRight.gif" width=10>
            </td>
        </tr>
        <tr>
            <td width="10"  background="Images/tbl_left.gif">
                <img height="10" src="Images/tbl_left.gif" width="10">
            </td>
            <td style="width: 528px">
                <table style="width: 174px">
                    <tr>
                        <td align="center"  colspan="5">
                            <strong>  <asp:Label ID="Header"  runat="server" Text="Contact Us" Width="79px"></asp:Label></strong>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 250px">
                            <asp:Label ID="lbFName"  runat="server"  Text ="<%$ Resources:contact,lbFName%>"  Width="160px"></asp:Label></td>
                        <td style="width: 151px">
                            <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
                        </td> 
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFirstName"
                            ErrorMessage="Enter Name" Class="vldRequiredSkin" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                        </td>
                        <td style="width: 3px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 210px">
                            <asp:Label ID="lbCompName" runat="server" Text ="<%$ Resources:contact,lbCompName%>"  Width="104px"></asp:Label></td>
                        <td style="width: 151px">
                            <asp:TextBox ID="txtCompanyName" runat="server"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                        <td style="width: 3px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 210px">
                            <asp:Label ID="lbphone" runat="server"  Text ="<%$ Resources:contact,lbphone%>" ></asp:Label></td>
                        <td style="width: 151px">
                            <asp:TextBox ID="txtphone" runat="server"></asp:TextBox></td>
                        <td>
                        </td>
                        <td style="width: 3px">
                        &nbsp;</td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 210px">
                            <asp:Label ID="lbEmail" runat="server" Text ="<%$ Resources:contact,lbEmail%>" ></asp:Label></td>
                        <td style="width: 151px">
                            <asp:TextBox ID="txtemail" runat="server"></asp:TextBox></td>
                        <td>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtemail"
                            ErrorMessage="Enter Valid Mail Id" Class="vldRegExSkin" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                            Width="128px"></asp:RegularExpressionValidator></td>
                        <td style="width: 3px">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 210px">
                            <asp:Label ID="lbSubject" runat="server" Text ="<%$ Resources:contact,lbSubject%>" Width="73px"></asp:Label></td>
                        <td style="width: 151px">
                            <asp:TextBox ID="txtsubject" runat="server"></asp:TextBox></td>
                        <td>
                        </td>
                        <td style="width: 3px">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lbHeaderText" runat="server" Text ="<%$ Resources:contact,lbHeaderText%>" ></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="height: 26px">
                            <asp:TextBox ID="txtmessage" runat="server" Height="150px" TextMode="MultiLine" Width="495px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 26px; width: 210px;">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtmessage"
                            ErrorMessage="Enter Message Here" Class="vldRequiredSkin" Width="136px" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                        </td>
                        <td style="height: 26px; width: 151px;">
                            <asp:Button ID="btnsend" runat="server" OnClick="btnsend_Click" Text ="<%$ Resources:contact,btnsend%>"  Width="98px" ValidationGroup="Mandatory" />
                        </td>
                        <td style="height: 26px">
                            <asp:Button ID="btnreset" runat="server" Text ="<%$ Resources:contact,btnreset%>" Width="81%" />
                        </td>
                        <td style="width: 3px; height: 26px">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="height: 35px">
                            <asp:Label ID="InfoLable" runat="server" Text="InfoLable" Visible="False" Class="lblResultSkin" Width="472px" Height="24px"></asp:Label>
                        </td>
                        <td style="width: 3px; height: 15px;">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 210px">
                        </td>
                        <td style="width: 151px">
                        </td>
                        <td>
                        </td>
                        <td style="width: 3px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 210px">
                        </td>
                        <td style="width: 151px">
                        </td>
                        <td>
                        </td>
                        <td style="width: 3px">
                        </td>
                    </tr>
            </table>
                <table>
                    <tr>
                        <td colspan ="3"><img src="Images/spacer.gif" height="20"/></td>
                     </tr>
                </table>
            </td>
            <td width="10" background="Images/tbl_right.gif">
                <img height="10" src="Images/tbl_right.gif" width="10">
            </td>
        </tr>
        <tr>
            <td width="10" height="10">
                <img height="10"  src="Images/tbl_bottomLeft.gif" width="10">
            </td>
            <td background="Images/tbl_bottom.gif" height=10 style="width: 528px">
                <img height=10 src="Images/tbl_bottom.gif" width=10>
            </td>
            <td width="10" height="10"><img height="10" src="Images/tbl_bottomRight.gif" width="10">
            </td>
        </tr>
       </tbody>
       </table>

