<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_Cataloguerequest" Codebehind="Cataloguerequest.ascx.cs" %>



<table width="96%" border="0" cellpadding="0" cellspacing="0" align="center">
<tr>
    <td>
    <table width="100%" border="0" cellspacing="0" cellpadding="5">
        <tr>
          <td class="tx_1" align="left"><a href="home.aspx" style="color:#0099FF" class="tx_3">Home</a><font style="font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal"> / </font>Catalogue Request</td>
        </tr>
        <tr>
          <td class="tx_3"><hr></td>
        </tr>
    </table>
    </td>
</tr>
<tr>
    <td height="30" class="tx_7" align="left">CATALOGUE REQUEST</td>
</tr>
<tr>
    <td class="tx_1" align="left">Please enter where you would like your FREE WES catalogue sent.<br>
    To order your catalogue please complete the following form (* equals required).</td>
</tr>
<tr>
    <td align="center" valign="top" class="tx_1">&nbsp;</td>
</tr>
</table>
<table width="558" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td height="30" background="images/20.gif"><table width="540" border="0" align="center" cellpadding="0" cellspacing="0">
          <tr>
            <td height="30" class="tx_6" align="left">FORMS</td>
            <td align="right"><img src="images/ico_11.gif" width="14" height="17"></td>
          </tr>
        </table></td>
      </tr>

      <tr>
        <td class="back_table_center1"><br>
          <table width="558px" border="0" align="center" cellpadding="2" cellspacing="0" class="tx_1">
          <tr>
            <td width="132px" height="25" valign="top" align="left">&nbsp;&nbsp;&nbsp;Full Name</td>
            <td valign="top" align="left">
            <input name="txtfname" type="text" id="txtfname" size="50" maxlength="50" runat="server" tabindex="0" class="inputbackground"/>
            </td>
            <td align="left" valign="middle" width="142px" align="left">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Enter full name" Class="vldRequiredSkin" ValidationGroup="Mandatory" ControlToValidate="txtfname"></asp:RequiredFieldValidator>
            </td>
            </tr>
          <tr>
            <td valign="top" height="25" align="left">&nbsp;&nbsp;&nbsp;Organization Name</td>
            <td valign="top" align="left"><input name="txtorgname" type="text" id="txtorgname" size="50" maxlength="50" runat="server" class="inputbackground"/></td>
            <td  align="left" valign="middle" align="left">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Enter organization name" Class="vldRequiredSkin" ValidationGroup="Mandatory" ControlToValidate="txtorgname"></asp:RequiredFieldValidator>
            </td>
          </tr>
          <tr>
            <td valign="top" height="25" align="left">&nbsp;&nbsp;&nbsp;Organization Type</td>
            <td valign="top" align="left"><input name="txtorgtype" type="text" id="txtorgtype" size="50" maxlength="50" runat="server" class="inputbackground"/></td>
            <td  align="left" valign="middle">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Enter organization type" Class="vldRequiredSkin" ValidationGroup="Mandatory" ControlToValidate="txtorgtype"></asp:RequiredFieldValidator>
            </td>

          </tr>
          <tr>
            <td valign="top" height="25" align="left">&nbsp;&nbsp;&nbsp;Email</td>
            <td valign="top" align="left"><input name="txtemail" type="text" id="txtemail" maxlength="50" size="50" runat="server" class="inputbackground"/></td>
             <td  align="left" valign="middle">
            <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtemail"
            ErrorMessage="Enter valid mail id" Class="vldRegExSkin" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
            Width="128px" ValidationGroup="Mandatory"></asp:RegularExpressionValidator>--%>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator8"  Width="128px" ErrorMessage="Enter email id" runat="server" Class="vldRequiredSkin" ValidationGroup="Mandatory" ControlToValidate="txtemail"></asp:RequiredFieldValidator>
            </td>
          </tr>
          <tr>
            <td valign="top" height="25" align="left">&nbsp;&nbsp;&nbsp;Address</td>
            <td valign="top" align="left"><input name="txtaddress" type="text" id="txtaddress" size="50" maxlength="250" runat="server" class="inputbackground"/></td>
            <td align="left" valign="middle">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Enter address" Class="vldRequiredSkin" ValidationGroup="Mandatory" ControlToValidate="txtaddress"></asp:RequiredFieldValidator>
            </td>

          </tr>
          <tr>
            <td valign="top" height="25" align="left">&nbsp;&nbsp;&nbsp;Post Code</td>
            <td valign="top" align="left"><input name="txtpostcode" type="text" id="txtpostcode" size="50" maxlength="50" runat="server" class="inputbackground"/></td>
            <td align="left" valign="middle">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Enter postcode" Class="vldRequiredSkin" ValidationGroup="Mandatory" ControlToValidate="txtpostcode"></asp:RequiredFieldValidator>
            </td>
          </tr>
          <tr>
            <td valign="top" height="25" align="left">&nbsp;&nbsp;&nbsp;Phone Number</td>
            <td valign="top" align="left"><input name="txtphone" type="text" id="txtphone" size="50" maxlength="50" runat="server" class="inputbackground"/></td>
             <td align="left" valign="middle">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Enter phone number" Class="vldRequiredSkin" ValidationGroup="Mandatory" ControlToValidate="txtphone"></asp:RequiredFieldValidator>
            </td>

          </tr>
          <tr>
            <td valign="top" height="25" align="left">&nbsp;&nbsp;&nbsp;Mobile Number</td>
            <td valign="top" align="left"><input name="txtmobile" type="text" id="txtmobile" size="50" maxlength="50" runat="server" class="inputbackground"/></td>
             <td align="left" valign="middle" align="left">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Enter mobile number" Class="vldRequiredSkin" ValidationGroup="Mandatory" ControlToValidate="txtmobile"></asp:RequiredFieldValidator>
            </td>
          </tr>
          <tr>
            <td valign="top">&nbsp;</td>
            <td valign="top">&nbsp;</td>

          </tr>
          <tr>
          <td align="right" valign="top" align="left"><label>
            <input type="checkbox" name="radio" id="radio" value="radio" runat="server"/>
            </label></td>
           <td valign="top" colspan="2" align="left">From time to time WES would like to email you details of new products and special offers. If you do not wish to receive these, untick this box
           </td>
          </tr>
          <tr>
            <td valign="top">&nbsp;</td>
            <td valign="top">&nbsp;</td>
          </tr>
          <tr>
            <td valign="top">&nbsp;</td>
            <td height="50" valign="middle" align="left">
                <asp:Button ID="Button1" runat="server" CssClass="form_boton" OnClick="btnsend_click" Text="Request" ValidationGroup="Mandatory"/>
            </td>
          </tr>
        </table>
        </td>
      </tr>
 </table>