<%@ Page Language="C#" MasterPageFile="~/mainpage.master"AutoEventWireup="true" Inherits="CompanyProfile" Title="Untitled Page"   Culture ="auto:en-US" UICulture ="auto" Codebehind="CompanyProfile.aspx.cs" %>
<%--<%@ Import Namespace ="TradingBell.Common" %>--%>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %>

<%@ Register TagPrefix="WebCat" Namespace="TradingBell.WebServices" Assembly="WES" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="header" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="leftnav" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" Runat="Server">
<asp:Panel ID ="pnlProfile" runat="server" DefaultButton="btnSubmit">
 <table align=center width="558" border="0" cellspacing="0" cellpadding="5">
        <tr>
          <td align="left" class="tx_1">
            <a href="home.aspx" style="color:#0099FF" class="tx_3">Home</a><font style="font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal"> / </font>Company Profile
          </td>
        </tr>
        <tr>
          <td class="tx_3">
            <hr>
          </td>

        </tr>
      </table>
      <br />
 <asp:Label ID="Label13" runat="server" Class="lblRequiredSkin"   meta:resourcekey="LblStar"></asp:Label><asp:Label ID="Label14" runat="server" meta:resourcekey="LblReqField" Class="lblNormalSkin"></asp:Label></asp:Panel>
                
   <table class="BaseTblBorder" width="568" align="center">
        <tr>
            <td  class="tx_6" height="20px" background="images/17.gif" align="left">
                <asp:Label ID="lblTitle" Class="lblBoldHeadSkin" runat="server" meta:resourcekey="lblTitle" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <table width="568" align="left">
                    <tr>
                        <td colspan = "2" align="left">
                            <asp:Label ID="lblCompanyDetails" runat="server" meta:resourcekey="lblCompanyDetails" Class="lblStaticSkin"></asp:Label>
                        </td>
                        <td align="left">
                         <% 
                            //Comapny name already exists.
                            if(Request["cNameEx"].ToString()=="1")
                            {
                            %>
                            <asp:Label ID ="lblErrorMsg" runat ="server" meta:resourcekey="lblErrorMsg" Class="lblErrorSkin" ></asp:Label>
                            <% }
                            else if(Request["cNameEx"].ToString()=="2")
                            {
                            %>
                            <asp:Label ID ="Label10" runat ="server" meta:resourcekey="lblErrorMsg1" Class="lblErrorSkin" ></asp:Label>
                            <% }
                         %>
                        </td>
                    </tr>
                    <%
                        CompanyGroupServices objCompanyGroupServices = new CompanyGroupServices();
                        objCompanyGroupServices.CompID_Type = "auto";
                        if (objCompanyGroupServices.CompID_Type.ToLower() == "custom")
                        {
                    %>
                    <tr>
                        <td align="center">
                            <asp:Label ID="Label2" runat="server" Class="lblRequiredSkin"   meta:resourcekey="LblStar"   ></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblCompanyID" runat="server"  meta:resourcekey="lblCompanyID" Class="lblNormalSkin" ></asp:Label>
                        </td>
                        <td align="left">
                        <%--<input name="txtCompanyID" type="text" id="txtCompanyID" maxlength="30" runat="server" tabindex="0" width="210px" class="inputbackground"/>--%>
                            <asp:TextBox autocomplete="off"   ID="txtCompanyID" runat="server" Width="210px" MaxLength="30" Class="textSkin"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvCompanyID"  meta:resourcekey="rfvCompanyID" runat="server"  ControlToValidate="txtCompanyID" ValidationGroup="Company" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ControlToValidate="txtCompanyID" meta:resourcekey="CompIDRegEx" ValidationExpression="^[0-9a-zA-Z]+$" Class="vldRegExSkin"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <%
                        }
                    %>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label1" runat="server" Class="lblRequiredSkin"   meta:resourcekey="LblStar"   ></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblCompanyName" runat="server"  meta:resourcekey="lblCompanyName" Class="lblNormalSkin" ></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox  autocomplete="off"   ID="txtCompanyName" runat="server" Width="210px" MaxLength="50" OnTextChanged="txtCompanyName_TextChanged" Class="textSkin"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvCompanyName"  meta:resourcekey="rfvCompanyName" runat="server"  ControlToValidate="txtCompanyName" ValidationGroup="Company" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:3%" align="left">
                           
                        </td>
                        <td style="width:30%" align="left">
                            <asp:Label ID="lblTaxID" runat="server"  meta:resourcekey="lblTaxID" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td style="width:67%" align="left">
                            <asp:TextBox  autocomplete="off"   ID="txtTaxID" runat="server" Width="210px" MaxLength="50" Class="textSkin"></asp:TextBox>
                            <%--<asp:RequiredFieldValidator ID="rfvTaxID" runat="server" meta:resourcekey="rfvTaxID" ControlToValidate="txtTaxID" ValidationGroup="Company" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                        --%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan ="3" align="left">
                            <asp:Label ID="lblCommunicationDetails" runat="server" meta:resourcekey="lblCommunicationDetails" Class="lblStaticSkin"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label3" runat="server" Class="lblRequiredSkin"   meta:resourcekey="LblStar"   ></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblAdd1" runat="server"  meta:resourcekey="lblAdd1" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox  autocomplete="off"   ID="txtAdd1" runat="server" Width="210px" MaxLength="50" Class="textSkin"></asp:TextBox>
                            <asp:RequiredFieldValidator  ID="rfvAdd1" runat="server" meta:resourcekey="rfvAdd1" ControlToValidate="txtAdd1" ValidationGroup="Company" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px" align="left">
                        </td>
                        <td style="height: 10px" align="left">
                            <asp:Label ID="lblAdd2" runat="server"  meta:resourcekey="lblAdd2" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td style="height: 10px" align="left">
                            <asp:TextBox  autocomplete="off"   ID="txtAdd2" runat="server" Width="210px" MaxLength="50" Class="textSkin"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblAdd3" runat="server"  meta:resourcekey="lblAdd3" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox  autocomplete="off"   ID="txtAdd3" runat="server" Width="210px" MaxLength="50" Class="textSkin"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label4" runat="server" Class="lblRequiredSkin"   meta:resourcekey="LblStar"   ></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblCity" runat="server"  meta:resourcekey="lblCity" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox  autocomplete="off"   ID="txtCity" runat="server" Width="210px" MaxLength="50" Class="textSkin"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvCity" runat="server"  meta:resourcekey="rfvCity" ControlToValidate="txtCity" ValidationGroup="Company" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label5" runat="server" Class="lblRequiredSkin"   meta:resourcekey="LblStar"   ></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblState" runat="server"  meta:resourcekey="lblState" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td align="left">
                           <%-- <asp:DropDownList ID="drpState" Class="DropdownlistSkin" Width="220px" runat="server">
                            </asp:DropDownList>--%>
                            <asp:TextBox  autocomplete="off"   ID="drpState" runat="server" Width="210px" MaxLength="50" Class="textSkin"></asp:TextBox>
                          <asp:RequiredFieldValidator ID="rfvState" runat="server" meta:resourcekey="rfvState" ControlToValidate="drpState" ValidationGroup="Company" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                      </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label6" runat="server" Class="lblRequiredSkin"   meta:resourcekey="LblStar"   ></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblZip" runat="server"  meta:resourcekey="lblZip" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox  autocomplete="off"   ID="txtZip" runat="server" Width="210px" MaxLength="20" Class="textSkin"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvZip" runat="server" meta:resourcekey="rfvZip" ControlToValidate="txtZip" ValidationGroup="Company" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label7" runat="server" Class="lblRequiredSkin"   meta:resourcekey="LblStar"   ></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblCountry" runat="server"  meta:resourcekey="lblCountry" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="drpCountry"  Class="DropdownlistSkin" Width="220px" runat="server" AutoPostBack="false">
                            </asp:DropDownList>
                            <%-- <asp:TextBox  autocomplete="off"   ID="txtCountry" runat="server"></asp:TextBox>--%>
                            <%--<asp:RequiredFieldValidator ID="rfvCountry" runat="server"  meta:resourcekey="rfvCountry" ControlToValidate="txtCountry" ValidationGroup="Company" Class="vldRequiredSkin"></asp:RequiredFieldValidator>--%>
                        </td>
                    </tr>
                    
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label8" runat="server" Class="lblRequiredSkin"   meta:resourcekey="LblStar"   ></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblPhone1" runat="server"   meta:resourcekey="lblPhone1" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox  autocomplete="off"   ID="txtPhone1" runat="server" Width="210px" MaxLength="50" Class="textSkin"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPhone1" runat="server" meta:resourcekey="rfvPhone1"  ControlToValidate="txtPhone1" ValidationGroup="Company" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblPhone2" runat="server"  meta:resourcekey="lblPhone2"  Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox  autocomplete="off"   ID="txtPhone2" runat="server" Width="210px" MaxLength="50" Class="textSkin"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblTollFree" runat="server"  meta:resourcekey="lblTollFree" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox  autocomplete="off"   ID="txtTollFree" runat="server" Width="210px" MaxLength="50" Class="textSkin"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblFax" runat="server"  meta:resourcekey="lblFax" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox  autocomplete="off"   ID="txtFax" runat="server" Width="210px" MaxLength="20" Class="textSkin"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label9" runat="server" Class="lblRequiredSkin"   meta:resourcekey="LblStar"   ></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblEmail" runat="server"  meta:resourcekey="lblEmail" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox  autocomplete="off"   ID="txtEmail" runat="server" Width="210px" MaxLength="50" Class="textSkin"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" meta:resourcekey="rfvEmail" ControlToValidate="txtEmail" ValidationGroup="Company" Class="vldRequiredSkin" TabIndex="564"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="valRegEx" runat="server" ControlToValidate="txtEmail" meta:resourcekey="valRegEx"  ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Class="vldRegExSkin" ValidationGroup="Company" TabIndex="455"></asp:RegularExpressionValidator></td>
                    </tr>
                    <tr>
                        <td>
                            
                        </td>
                        <td align="left">
                            <asp:Label ID="lblSecurityCode" Visible="false" runat="server"  meta:resourcekey="lblSecurityCode"  Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox  autocomplete="off" Visible="false"  ID="txtSecurityCode" runat="server" Width="210px" MaxLength="50" Class="textSkin"></asp:TextBox>
                            </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblWeb" runat="server"  meta:resourcekey="lblWeb" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox  autocomplete="off"   ID="txtWeb" runat="server" Width="210px" MaxLength="50" Class="textSkin"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="Regweb" runat="server" ControlToValidate="txtWeb" meta:resourcekey="rfvinvalidUrl"  ValidationExpression="^((ht|f)tp(s?)\:\/\/|~/|/)?([\w]+:\w+@)?([a-zA-Z]{1}([\w\-]+\.)+([\w]{2,5}))(:[\d]{1,5})?((/?\w+/)+|/?)(\w+\.[\w]{3,4})?((\?\w+=\w+)?(&\w+=\w+)*)?" Class="vldRegExSkin" ValidationGroup="Company" ></asp:RegularExpressionValidator>

                        </td>
                    </tr>
                    <tr>
                        <td style="height: 26px" align="left">
                            <asp:Label ID="Label11" runat="server" Class="lblRequiredSkin"   meta:resourcekey="LblStar"   ></asp:Label>
                        </td>
                        <td style="height: 26px" align="left">
                            <asp:Label ID="lblPrimaryContact" runat="server"  meta:resourcekey="lblPrimaryContact" Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td style="height: 26px" align="left">
                            <asp:TextBox  autocomplete="off"   ID="txtPrimaryContact" runat="server" Width="210px" MaxLength="50" Class="textSkin"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvContact" runat="server"  meta:resourcekey="rfvContact" ControlToValidate="txtPrimaryContact" ValidationGroup="Company" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <%
                    //  Helper oHelper = new Helper();
                    //  if (oHelper.GetOptionValues("ECOMMERCEENABLED").ToString() == "YES")
                    // {  
                    %>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label12" runat="server" Class="lblRequiredSkin"   meta:resourcekey="LblStar"   ></asp:Label>
                        </td>
                        <td colspan = "3" align="left">
                            <asp:Label ID="lblPaymentDetails" runat="server" meta:resourcekey="lblPaymentDetails" Class="lblStaticSkin"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblPayMethod" runat="server"  meta:resourcekey="lblPayMethod"  Class="lblNormalSkin"></asp:Label>
                        </td>
                        <td align="left"> 
                            <table>
                                <tr>
                                    <td align="left"><asp:CheckBox ID="chkPO" meta:resourcekey="chkPO"  runat="server" Class="CheckBoxSkin" /></td>
                                    <td align="left"><asp:CheckBox ID="chkCC" meta:resourcekey="chkCC" runat="server" Class="CheckBoxSkin" /></td>
                                    <td align="left"><asp:CheckBox ID="chkCOD" meta:resourcekey="chkCOD" runat="server" Class="CheckBoxSkin" Checked ="true" /></td>
                                </tr>
                            </table>
                            <asp:Label ID="lblError" runat="server" Class="lblErrorSkin"></asp:Label></td>
                    </tr>
                    <% //} %>
            </table>
            </td>
        </tr>
        </table>
        <table width ="568" align="center">
        <tr>
            <td  align ="right" >
            <asp:Button ID="btnSubmit" runat="server" meta:resourcekey="btnSubmit"  OnClick="btnSubmit_Click" ValidationGroup="Company" Class="btnNormalSkin" />
            </td>
        </tr> 
        </table>      
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="rightnav" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>



