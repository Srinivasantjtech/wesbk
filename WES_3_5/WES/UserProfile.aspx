<%@ Page Language="C#" MasterPageFile="~/mainpage.master" AutoEventWireup="true" Inherits="UserProfile" Title="Untitled Page" 
    Culture="en-US" UICulture="en-US" Codebehind="UserProfile.aspx.cs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="header" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="leftnav" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" runat="Server">
    <script type="text/javascript">
        function ValidateAnswer(oSrc, args) {
            args.IsValid = (args.Value.length >= 4);
        }
  
    </script>
    <asp:Panel ID="pnlProfile" runat="server" DefaultButton="btnSubmit">
        <table align="center" width="558" border="0" cellspacing="0" cellpadding="5">
            <tr>
                <td align="left" class="tx_1">
                    <a href="home.aspx" style="color: #0099FF" class="tx_3">Home</a><font style="font-family: Arial, Helvetica, sans-serif;
                        font-weight: bolder; font-size: small; font-style: normal"> / </font>User Profile
                </td>
            </tr>
            <tr>
                <td class="tx_3">
                    <hr>
                </td>
            </tr>
        </table>
        <br />
        &nbsp;<asp:Label ID="Label18" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"
            Width="1px"></asp:Label>
        &nbsp;<asp:Label ID="Label25" runat="server" meta:resourcekey="LblReqField" Class="lblNormalSkin"></asp:Label>
        <table class="BaseTblBorder" align="center" width="558" border="0" cellpadding="0"
            cellspacing="1">
            <tr>
                <td class="tx_6" height="20px" background="images/17.gif" align="left">
                    <asp:Label ID="lblHeader" runat="server" meta:resourcekey="lblHeader" Class="lblBoldHeadSkin"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <table border="0" cellpadding="0" cellspacing="1">
                        <tr>
                            <td width="2%" style="height: 24px" align="left">
                                <asp:Label ID="Label5" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                            </td>
                            <td width="12%" style="height: 24px" align="left">
                                <asp:Label ID="lblFname" runat="server" meta:resourcekey="lblFname" Class="lblNormalSkin"></asp:Label>
                            </td>
                            <td width="35%" style="height: 24px" align="left">
                                <asp:DropDownList ID="cmbPrefix" runat="server" AutoPostBack="false" Class="DropdownlistSkin"
                                    Width="50px">
                                    <asp:ListItem Value="Mr.">Mr.</asp:ListItem>
                                    <asp:ListItem Value="Ms.">Ms.</asp:ListItem>
                                    <asp:ListItem Value="M/s.">M/s.</asp:ListItem>
                                    <asp:ListItem Value="Er.">Er.</asp:ListItem>
                                    <asp:ListItem Value="Dr.">Dr.</asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox autocomplete="off" ID="txtFname" runat="server" Class="textSkin" MaxLength="50"
                                    Width="155px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFname" runat="server" Class="vldRequiredSkin"
                                    ValidationGroup="Mandatory" Display="Dynamic" meta:resourcekey="rfvFname" ControlToValidate="txtFname"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 24px" align="left">
                            </td>
                            <td style="height: 24px" width="12%" align="left">
                                <asp:Label ID="lblMname" runat="server" meta:resourcekey="lblMname" Class="lblNormalSkin"></asp:Label>
                            </td>
                            <td style="height: 24px" width="35%" align="left">
                                <asp:TextBox autocomplete="off" ID="txtMname" runat="server" Class="textSkin" Rows="40"
                                    Width="208px" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 24px" align="left">
                                <asp:Label ID="Label6" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                            </td>
                            <td style="height: 24px" width="12%" align="left">
                                <asp:Label ID="lblLname" runat="server" meta:resourcekey="lblLname" Class="lblNormalSkin"></asp:Label>
                            </td>
                            <td style="height: 24px" width="35%" align="left">
                                <asp:TextBox autocomplete="off" ID="txtLname" runat="server" Class="textSkin" MaxLength="50"
                                    Width="208px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvLname" runat="server" ControlToValidate="txtLName"
                                    Display="Dynamic" meta:resourcekey="rfvLname" Class="vldRequiredSkin" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td width="12%" align="left">
                                <asp:Label ID="lblSuffix" runat="server" meta:resourcekey="lblSuffix" Class="lblNormalSkin"></asp:Label>
                            </td>
                            <td width="35%" align="left">
                                <asp:TextBox autocomplete="off" ID="txtSuffix" runat="server" Class="textSkin" Rows="50"
                                    Width="208px" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left">
                                <asp:Label ID="lblCommunicationTitle" runat="server" meta:resourcekey="lblCommunicationTitle"
                                    Class="lblStaticSkin"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 24px" align="left">
                                <asp:Label ID="Label7" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                            </td>
                            <td width="12%" style="height: 24px" align="left">
                                <asp:Label ID="lblAdd1" runat="server" meta:resourcekey="lblAdd1" Class="lblNormalSkin"></asp:Label>
                            </td>
                            <td width="35%" style="height: 24px" align="left">
                                <asp:TextBox autocomplete="off" ID="txtAdd1" runat="server" Class="textSkin" MaxLength="50"
                                    Width="208px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvAdd1" runat="server" ControlToValidate="txtAdd1"
                                    Display="Dynamic" meta:resourcekey="rfvAdd1" Class="vldRequiredSkin" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td width="12%" align="left">
                                <asp:Label ID="lblAdd2" runat="server" meta:resourcekey="lblAdd2" Class="lblNormalSkin"></asp:Label>
                            </td>
                            <td width="35%" align="left">
                                <asp:TextBox autocomplete="off" ID="txtAdd2" runat="server" Class="textSkin" MaxLength="50"
                                    Width="208px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 125px">
                            </td>
                            <td width="12%" align="left">
                                <asp:Label ID="lblAdd3" runat="server" meta:resourcekey="lblAdd3" Class="lblNormalSkin"></asp:Label>
                            </td>
                            <td width="35%" align="left">
                                <asp:TextBox autocomplete="off" ID="txtAdd3" runat="server" Class="textSkin" MaxLength="50"
                                    Width="208px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 24px" align="left">
                                <asp:Label ID="Label8" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                            </td>
                            <td style="height: 24px" width="12%" align="left">
                                <asp:Label ID="lblCity" runat="server" meta:resourcekey="lblCity" Class="lblNormalSkin"></asp:Label>
                            </td>
                            <td style="height: 24px" width="35%" align="left">
                                <asp:TextBox autocomplete="off" ID="txtCity" runat="server" Class="textSkin" MaxLength="50"
                                    Width="208px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvCity" Class="vldRequiredSkin" runat="server"
                                    ControlToValidate="txtCity" meta:resourcekey="rfvCity" Display="Dynamic" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 22px" align="left">
                                <asp:Label ID="Label9" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                            </td>
                            <td style="height: 22px" width="12%" align="left">
                                <asp:Label ID="lblState" runat="server" meta:resourcekey="lblState" Class="lblNormalSkin"></asp:Label>
                            </td>
                            <td style="height: 22px" width="35%" align="left">
                                <%--   <asp:DropDownList ID="drpState" runat="server" Width="215px" Class="DropdownlistSkin"  >
            </asp:DropDownList>--%>
                                <asp:TextBox autocomplete="off" ID="drpState" runat="server" Class="textSkin" MaxLength="50"
                                    Width="208px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Class="vldRequiredSkin"
                                    runat="server" ControlToValidate="drpState" meta:resourcekey="rfvState" Display="Dynamic"
                                    ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="Label10" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                            </td>
                            <td width="12%" align="left">
                                <asp:Label ID="lblZip" runat="server" meta:resourcekey="lblZip" Class="lblNormalSkin"></asp:Label>
                            </td>
                            <td width="35%" align="left">
                                <asp:TextBox autocomplete="off" ID="txtZip" runat="server" Class="textSkin" Rows="50"
                                    Width="208px" MaxLength="20"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvZip" Class="vldRequiredSkin" runat="server" ControlToValidate="txtZip"
                                    meta:resourcekey="rfvZip" Display="Dynamic" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 22px" align="left">
                                <asp:Label ID="Label11" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                            </td>
                            <td style="height: 22px" width="12%" align="left">
                                <asp:Label ID="lblCountry" runat="server" meta:resourcekey="lblCountry" Class="lblNormalSkin"></asp:Label>
                            </td>
                            <td style="height: 22px" width="35%" align="left">
                                <asp:DropDownList ID="drpCountry" runat="server" Width="215px" Class="DropdownlistSkin">
                                </asp:DropDownList>
                                <%-- AutoPostBack="true" OnSelectedIndexChanged="drpCountry_SelectedIndexChanged" --%>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                            </td>
                            <td width="12%" align="left">
                                <asp:Label ID="lblAltEmail" runat="server" meta:resourcekey="lblAltEmail" Class="lblNormalSkin"></asp:Label>
                            </td>
                            <td width="35%" align="left">
                                <asp:TextBox autocomplete="off" ID="txtAltEmail" runat="server" Class="textSkin"
                                    MaxLength="50" Width="208px"></asp:TextBox>
                                <%
                                    if (txtAltEmail.Text != null || txtAltEmail.Text != "")
                                    {
                                %>
                                <asp:RegularExpressionValidator ID="valRegEx" runat="server" ControlToValidate="txtAltEmail"
                                    meta:resourcekey="rfvAltEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    Class="vldRegExSkin" Display="Dynamic" ValidationGroup="Mandatory"></asp:RegularExpressionValidator>
                                <%} %>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="Label12" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                            </td>
                            <td width="12%" align="left">
                                <asp:Label ID="lblPhone" runat="server" meta:resourcekey="lblPhone" Class="lblNormalSkin"></asp:Label>
                            </td>
                            <td width="35%" align="left">
                                <asp:TextBox autocomplete="off" ID="txtPhone" runat="server" Class="textSkin" MaxLength="50"
                                    Width="208px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPhone" Class="vldRequiredSkin" runat="server"
                                    ControlToValidate="txtPhone" meta:resourcekey="rfvPhone" Display="Dynamic" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 24px" align="left">
                            </td>
                            <td style="height: 24px" width="12%" align="left">
                                <asp:Label ID="lblMobile" runat="server" meta:resourcekey="lblMobile" Class="lblNormalSkin"></asp:Label>
                            </td>
                            <td style="height: 24px" width="35%" align="left">
                                <asp:TextBox autocomplete="off" ID="txtMobile" runat="server" Class="textSkin" MaxLength="50"
                                    Width="208px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td width="12%" align="left">
                                <asp:Label ID="lblFax" runat="server" meta:resourcekey="lblFax" Class="lblNormalSkin"></asp:Label>
                            </td>
                            <td width="35%" align="left">
                                <asp:TextBox autocomplete="off" ID="txtFax" runat="server" Class="textSkin" MaxLength="20"
                                    Width="208px"></asp:TextBox>
                            </td>
                            <!--Billing Details-->
                            <tr>
                                <td colspan="3" align="left">
                                    <asp:Label ID="lblBillingTitle" runat="server" meta:resourcekey="lblBillingTitle"
                                        Class="lblStaticSkin"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    <asp:CheckBox ID="commbillAddress" OnCheckedChanged="checkbillAddress" Class="CheckBoxSkin"
                                        runat="server" meta:resourcekey="lblBillingAddress" Checked="false" AutoPostBack="True" />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 24px" align="left">
                                    <asp:Label ID="Label17" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                                </td>
                                <td style="height: 24px" width="12%" align="left">
                                    <asp:Label ID="lblbilladdress1" runat="server" meta:resourcekey="lblbilladdress1"
                                        Class="lblNormalSkin"></asp:Label>
                                </td>
                                <td style="height: 24px" width="35%" align="left">
                                    <asp:TextBox autocomplete="off" ID="txtbilladd1" runat="server" Class="textSkin"
                                        MaxLength="50" Width="208px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvbAdd1" runat="server" ControlToValidate="txtbillAdd1"
                                        Display="Dynamic" meta:resourcekey="rfvAdd1" Class="vldRequiredSkin" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td width="12%" align="left">
                                    <asp:Label ID="lblbilladdress2" runat="server" meta:resourcekey="lblbilladdress2"
                                        Class="lblNormalSkin"></asp:Label>
                                </td>
                                <td width="35%" align="left">
                                    <asp:TextBox autocomplete="off" ID="txtbilladd2" runat="server" Class="textSkin"
                                        MaxLength="50" Width="208px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td width="12%" align="left">
                                    <asp:Label ID="lblbilladdress3" runat="server" meta:resourcekey="lblbilladdress3"
                                        Class="lblNormalSkin"></asp:Label>
                                </td>
                                <td width="35%" align="left">
                                    <asp:TextBox autocomplete="off" ID="txtbilladd3" runat="server" Class="textSkin"
                                        MaxLength="50" Width="208px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label19" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                                </td>
                                <td width="12%" align="left">
                                    <asp:Label ID="lblbillCity" runat="server" meta:resourcekey="lblbillCity" Class="lblNormalSkin"></asp:Label>
                                </td>
                                <td width="35%" align="left">
                                    <asp:TextBox autocomplete="off" ID="txtbillcity" runat="server" Class="textSkin"
                                        MaxLength="50" Width="208px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvbCity" Class="vldRequiredSkin" runat="server"
                                        ControlToValidate="txtbillcity" meta:resourcekey="rfvCity" Display="Dynamic"
                                        ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label20" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                                </td>
                                <td width="12%" align="left">
                                    <asp:Label ID="lblbillState" runat="server" meta:resourcekey="lblbillState" Class="lblNormalSkin"></asp:Label>
                                </td>
                                <td width="35%" align="left">
                                    <%--<asp:DropDownList ID="drpBillState" runat="server" Width="215px" Class="DropdownlistSkin"  >
            </asp:DropDownList>--%>
                                    <asp:TextBox autocomplete="off" ID="drpBillState" runat="server" Class="textSkin"
                                        MaxLength="50" Width="208px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Class="vldRequiredSkin"
                                        runat="server" ControlToValidate="drpBillState" meta:resourcekey="rfvState" Display="Dynamic"
                                        ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 24px" align="left">
                                    <asp:Label ID="Label21" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                                </td>
                                <td style="height: 24px" width="12%" align="left">
                                    <asp:Label ID="lblbillZip" runat="server" meta:resourcekey="lblbillZip" Class="lblNormalSkin"></asp:Label>
                                </td>
                                <td style="height: 24px" width="35%" align="left">
                                    <asp:TextBox autocomplete="off" ID="txtbillzip" runat="server" Class="textSkin"
                                        MaxLength="20" Width="208px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvbZip" Class="vldRequiredSkin" runat="server"
                                        ControlToValidate="txtbillzip" meta:resourcekey="rfvZip" Display="Dynamic" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 22px" align="left">
                                    <asp:Label ID="Label22" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                                </td>
                                <td width="12%" style="height: 22px" align="left">
                                    <asp:Label ID="lblbillCountry" runat="server" meta:resourcekey="lblbillCountry" Class="lblNormalSkin"></asp:Label>
                                </td>
                                <td width="35%" style="height: 22px" align="left">
                                    <asp:DropDownList ID="drpBillCountry" runat="server" Width="215px" Class="DropdownlistSkin">
                                    </asp:DropDownList>
                                    <%-- AutoPostBack="true" OnSelectedIndexChanged="drpBillCountry_SelectedIndexChanged" --%>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label24" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                                </td>
                                <td width="12%" align="left">
                                    <asp:Label ID="lblbillPhone" runat="server" meta:resourcekey="lblbillPhone" Class="lblNormalSkin"></asp:Label>
                                </td>
                                <td width="35%" align="left">
                                    <asp:TextBox autocomplete="off" ID="txtbillphone" runat="server" Class="textSkin"
                                        MaxLength="50" Width="208px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvbPhone" Class="vldRequiredSkin" runat="server"
                                        ControlToValidate="txtbillphone" meta:resourcekey="rfvPhone" Display="Dynamic"
                                        ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <!-- Shipping Address.. -->
                        </tr>
                        <tr>
                            <td colspan="3" align="left">
                                <asp:Label ID="lblshipTitle" runat="server" meta:resourcekey="lblShippingTitle" Class="lblStaticSkin"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left">
                                <asp:CheckBox ID="commshipAddress" Class="CheckBoxSkin" runat="server" OnCheckedChanged="checkshipAddress"
                                    meta:resourcekey="lblShippingAddress" Width="100%" AutoPostBack="True" />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 24px" align="left">
                                <asp:Label ID="Label1" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                            </td>
                            <td width="12%" style="height: 24px" align="left">
                                <asp:Label ID="lblshipadd1" runat="server" meta:resourcekey="lblshipaddress1" Class="lblNormalSkin"></asp:Label>
                            </td>
                            <td width="35%" style="height: 24px" align="left">
                                <asp:TextBox autocomplete="off" ID="txtshipadd1" runat="server" Class="textSkin"
                                    MaxLength="50" Width="208px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvsAdd1" runat="server" ControlToValidate="txtshipAdd1"
                                    Display="Dynamic" meta:resourcekey="rfvAdd1" Class="vldRequiredSkin" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 24px">
                            </td>
                            <td style="height: 24px" width="12%" align="left">
                                <asp:Label ID="lblshipadd2" runat="server" meta:resourcekey="lblshipaddress2" Class="lblNormalSkin"></asp:Label>
                            </td>
                            <td style="height: 24px" width="35%" align="left">
                                <asp:TextBox autocomplete="off" ID="txtshipadd2" runat="server" Class="textSkin"
                                    MaxLength="50" Width="208px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 27px" align="left">
                            </td>
                            <td width="12%" style="height: 27px" align="left">
                                <asp:Label ID="Label2" runat="server" meta:resourcekey="lblshipaddress3" Class="lblNormalSkin"></asp:Label>
                            </td>
                            <td width="35%" style="height: 27px" align="left">
                                <asp:TextBox autocomplete="off" ID="txtshipadd3" runat="server" Class="textSkin"
                                    MaxLength="50" Width="208px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="Label3" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                            </td>
                            <td width="12%" align="left">
                                <asp:Label ID="lblshipcity" runat="server" meta:resourcekey="lblshipCity" Class="lblNormalSkin"></asp:Label>
                            </td>
                            <td width="35%" align="left">
                                <asp:TextBox autocomplete="off" ID="txtshipcity" runat="server" Class="textSkin"
                                    MaxLength="50" Width="208px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvsCity" Class="vldRequiredSkin" runat="server"
                                    ControlToValidate="txtshipcity" meta:resourcekey="rfvCity" Display="Dynamic"
                                    ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 22px" align="left">
                                <asp:Label ID="Label14" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                            </td>
                            <td width="12%" style="height: 22px" align="left">
                                <asp:Label ID="lblshipState" runat="server" meta:resourcekey="lblshipState" Class="lblNormalSkin"></asp:Label>
                            </td>
                            <td width="35%" style="height: 22px" align="left">
                                <%-- <asp:DropDownList ID="drpShipState" runat="server" Width="215px" Class="DropdownlistSkin"  >
            </asp:DropDownList>--%>
                                <asp:TextBox autocomplete="off" ID="drpShipState" runat="server" Class="textSkin"
                                    MaxLength="50" Width="208px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Class="vldRequiredSkin"
                                    runat="server" ControlToValidate="drpShipState" meta:resourcekey="rfvState" Display="Dynamic"
                                    ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="Label15" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                            </td>
                            <td width="12%" align="left">
                                <asp:Label ID="lblshipZip" runat="server" meta:resourcekey="lblshipZip" Class="lblNormalSkin"></asp:Label>
                            </td>
                            <td width="35%" align="left">
                                <asp:TextBox autocomplete="off" ID="txtshipzip" runat="server" Class="textSkin"
                                    MaxLength="20" Width="208px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvsZip" Class="vldRequiredSkin" runat="server"
                                    ControlToValidate="txtshipzip" meta:resourcekey="rfvZip" Display="Dynamic" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="Label16" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                            </td>
                            <td width="12%" align="left">
                                <asp:Label ID="lblshipCountry" runat="server" meta:resourcekey="lblshipCountry" Class="lblNormalSkin"></asp:Label>
                            </td>
                            <td width="35%" align="left">
                                <asp:DropDownList ID="drpShipCountry" runat="server" Width="215px" Class="DropdownlistSkin">
                                </asp:DropDownList>
                                <%-- AutoPostBack="true" OnSelectedIndexChanged="drpShipCountry_SelectedIndexChanged" --%>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="Label23" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                            </td>
                            <td width="12%" align="left">
                                <asp:Label ID="lblshipPhone" runat="server" meta:resourcekey="lblshipPhone" Class="lblNormalSkin"></asp:Label>
                            </td>
                            <td width="35%" align="left">
                                <asp:TextBox autocomplete="off" ID="txtshipphone" runat="server" Class="textSkin"
                                    MaxLength="50" Width="208px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvsPhone" Class="vldRequiredSkin" runat="server"
                                    ControlToValidate="txtshipphone" meta:resourcekey="rfvPhone" Display="Dynamic"
                                    ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <!-- Security Questions -->
                        <tr>
                            <td colspan="3" style="height: 19px" align="left">
                                <asp:Label ID="lblSecurityTitle" runat="server" meta:resourcekey="lblSecurityTitle"
                                    Class="lblStaticSkin"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 17px">
                            </td>
                            <td style="height: 17px" width="12%">
                            </td>
                            <td style="height: 17px" width="35%" align="left">
                                <asp:Label ID="lblForgetMsg" runat="server" meta:resourcekey="lblForgetMsg" Class="lblGraySkin"
                                    Width="248px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 22px" align="left">
                                <asp:Label ID="Label4" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                            </td>
                            <td width="12%" style="height: 22px" align="left">
                                <asp:Label ID="lblQuestion1" runat="server" meta:resourcekey="lblQuestion1" Class="lblNormalSkin"></asp:Label>
                            </td>
                            <td width="35%" style="height: 22px" align="left">
                                <asp:DropDownList ID="cmbQuestion" runat="server" Width="230px" Class="DropdownlistSkin">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="Label13" runat="server" Class="lblRequiredSkin" meta:resourcekey="LblStar"></asp:Label>
                            </td>
                            <td width="12%" align="left">
                                <asp:Label ID="lblAnswer1" runat="server" meta:resourcekey="lblAnswer1" Class="lblNormalSkin"></asp:Label>
                            </td>
                            <td width="35%" align="left">
                                <asp:TextBox autocomplete="off" ID="txtAnswer1" runat="server" Class="textSkin"
                                    MaxLength="50" Width="225px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvAnswer1" runat="server" meta:resourcekey="rfvAnswer1"
                                    ControlToValidate="txtAnswer1" Class="vldRequiredSkin" ValidationGroup="Mandatory"
                                    Width="128px"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td width="12%">
                                &nbsp;
                            </td>
                            <td width="35%" align="left">
                                &nbsp;<asp:CustomValidator ID="vldCusAns" runat="server" ControlToValidate="txtAnswer1"
                                    ValidationGroup="Mandatory" ClientValidationFunction="ValidateAnswer" meta:resourcekey="cfvAnswer1"
                                    Class="vldCustomSkin" Width="168px"></asp:CustomValidator>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table width="550" align="center">
            <tr>
                <td align="right">
                    <asp:Button ID="btnSubmit" Class="btnNormalSkin" runat="server" meta:resourcekey="btnSubmit"
                        ValidationGroup="Mandatory" OnClick="btnSubmit_Click" />&nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="rightnav" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="footer" runat="Server">
</asp:Content>
