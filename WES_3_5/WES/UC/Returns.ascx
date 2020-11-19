<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_Returns" Codebehind="Returns.ascx.cs" %>

<%
    Response.Write(ST_Returns());
%>
                
<table width="558" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td height="30" background="images/17.gif"><table width="540" border="0" align="center" cellpadding="0" cellspacing="0">
          <tr>
            <td height="30" class="tx_6" align="left">PAGE INDEX</td>
            <td align="right"><img src="images/ico_11.gif" width="14" height="17"></td>
          </tr>
        </table></td>
      </tr>

      <tr>
        <td class="back_table_center1"><br>
          <table width="540px" border="0" align="center" cellpadding="2" cellspacing="0" class="tx_1">
          <tr>
            <td width="132px" height="25" valign="top" align="left"><strong>Name</strong></td>
            <td valign="top" align="left">
            <input name="txtfname" type="text" id="txtfname" size="50" runat="server" tabindex="0" class="inputbackground"/>
            </td>
            <td align="left" valign="middle" width="142px">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Enter full name" Class="vldRequiredSkin" ValidationGroup="Mandatory" ControlToValidate="txtfname"></asp:RequiredFieldValidator>
            </td>
            </tr>
          <tr>
            <td  height="25"  valign="top" align="left"><strong>Organization Name</strong></td>
            <td valign="top" align="left"><input name="txtorgname" type="text" id="txtorgname" size="50" runat="server" class="inputbackground"/></td>
            <td  align="left" valign="middle" align="left">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Enter organization name" Class="vldRequiredSkin" ValidationGroup="Mandatory" ControlToValidate="txtorgname"></asp:RequiredFieldValidator>
            </td>
          </tr>
          <tr>
            <td height="25"  valign="top" align="left"><strong>Organization Type</strong></td>
            <td valign="top" align="left"><input name="txtorgtype" type="text" id="txtorgtype" size="50" runat="server" class="inputbackground"/></td>
            <td  align="left" valign="middle" align="left">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Enter organization type" Class="vldRequiredSkin" ValidationGroup="Mandatory" ControlToValidate="txtorgtype"></asp:RequiredFieldValidator>
            </td>

          </tr>
          <tr>
            <td height="25"  valign="top" align="left"><strong>Email</strong></td>
            <td valign="top" align="left"><input name="txtemail" type="text" id="txtemail" size="50" runat="server" class="inputbackground"/></td>
             <td  align="left" valign="middle">
            <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtemail"
            ErrorMessage="Enter valid mail id" Class="vldRegExSkin" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
            Width="128px" ValidationGroup="Mandatory"></asp:RegularExpressionValidator>--%>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" Width="128px" ErrorMessage="Enter email id" runat="server" Class="vldRequiredSkin" ValidationGroup="Mandatory" ControlToValidate="txtemail"></asp:RequiredFieldValidator>
            </td>
          </tr>
          <tr>
            <td height="25"  valign="top" align="left"><strong>Address</strong></td>
            <td valign="top" align="left"><input name="txtaddress" type="text" id="txtaddress" size="50" runat="server" class="inputbackground"/></td>
            <td align="left" valign="middle">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Enter address" Class="vldRequiredSkin" ValidationGroup="Mandatory" ControlToValidate="txtaddress"></asp:RequiredFieldValidator>
            </td>

          </tr>
          <tr>
            <td  height="25" valign="top" align="left"><strong>Post Code</strong></td>
            <td valign="top" align="left"><input name="txtpostcode" type="text" id="txtpostcode" size="50" runat="server" class="inputbackground"/></td>
            <td align="left" valign="middle">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Enter postcode" Class="vldRequiredSkin" ValidationGroup="Mandatory" ControlToValidate="txtpostcode"></asp:RequiredFieldValidator>
            </td>
          </tr>
          <tr>
            <td height="25"  valign="top" align="left"><strong>Phone Number</strong></td>
            <td valign="top" align="left"><input name="txtphone" type="text" id="txtphone" size="50" runat="server" class="inputbackground"/></td>
             <td align="left" valign="middle">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Enter phone number" Class="vldRequiredSkin" ValidationGroup="Mandatory" ControlToValidate="txtphone"></asp:RequiredFieldValidator>
            </td>

          </tr>
          <tr>
            <td height="25"  valign="top" align="left"><strong>Mobile Number</strong></td>
            <td valign="top" align="left"><input name="txtmobile" type="text" id="txtmobile" size="50" runat="server" class="inputbackground"/></td>
             <td align="left" valign="middle">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Enter mobile number" Class="vldRequiredSkin" ValidationGroup="Mandatory" ControlToValidate="txtmobile"></asp:RequiredFieldValidator>
            </td>
          </tr>
          <tr>
            <td valign="top">&nbsp;</td>
            <td valign="top">&nbsp;</td>
          </tr>
          <tr>
            <td valign="top">&nbsp;</td>
            <td height="50" align="left">
                <asp:Button ID="Button1" runat="server" CssClass="form_boton" OnClick="btnsend_click" Text="Send" ValidationGroup="Mandatory"/>
            </td>
            <td valign="top">&nbsp;</td>
          </tr>
        </table>
        </td>
      </tr>
</table>