<%@ Page Language="C#" MasterPageFile="~/mainpage.master" AutoEventWireup="true" Inherits="RetailerEditUserProfile" Title="Untitled Page"
     Culture="en-US" UICulture="en-US" Codebehind="RetailerEditUserProfile.aspx.cs" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="header" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="leftnav" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="pnlProfile" runat="server" DefaultButton="btnUpdate">
       <%-- <table align="center" width="558" border="0" cellspacing="0">
            <tr>
                <td align="left" class="tx_1">
                    <a href="home.aspx" style="color: #0099FF" class="tx_3">Home</a><font style="font-family: Arial, Helvetica, sans-serif;
                        font-weight: bolder; font-size: small; font-style: normal"> / </font>View User
                    Profile
                </td>
            </tr>
            <tr>
                <td class="tx_3">
                    <hr>
                </td>
            </tr>
        </table>--%>
         <div class="span9 box1" style="width: 580px;margin-left:5px;">
         <h3 class="title1" align="left">View User Profile</h3>
        <table align="center" width="558">
            <tr>
                <td align="center">
                  <%--  &nbsp;<asp:Label ID="Label2" runat="server" Class="lblRequiredSkin" Visible="false"
                        meta:resourcekey="LblStar" Width="1px"></asp:Label>
                    &nbsp;<asp:Label ID="Label4" runat="server" Visible="false" meta:resourcekey="LblReqField"
                        Class="lblNormalSkin"></asp:Label>--%>
                    <table width="558" cellspacing="0" align="center" >
                       <%-- <tr>
                            <td class="tx_6" height="20px" background="images/17.gif" align="left">
                                <asp:Label ID="lblUserHead" runat="server" meta:resourcekey="lblUserHead"></asp:Label>
                            </td>
                        </tr>--%>
                        <tr>
                            <td align="left">
                             <div class="box1">
                           <h3 class="title3">Contact Details</h3>
                                <table id="user" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                       
                                        <td><span class="form_1">
                                            <asp:Label ID="Label51" Class="lblNormalSkin" runat="server" Text="Company Name"></asp:Label>                                          
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox autocomplete="off" ReadOnly="false" ID="txtcompname" CssClass="input_dr"
                                                runat="server" MaxLength="50" Width="203px"></asp:TextBox></span>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                       
                                        <td style="width: 30%"><span class="form_1">
                                            <asp:Label ID="lblFName" Class="lblNormalSkin" runat="server" meta:resourcekey="lblFName"></asp:Label>
                                            <asp:Label ID="Label10" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label>
                                            </span>
                                        </td>
                                        <td style="width: 30%;">
                                           <span class="form_2">
                                            <asp:TextBox autocomplete="off" ID="txtFname" ReadOnly="false" runat="server" MaxLength="40"
                                                CssClass="input_dr" Width="203px"></asp:TextBox>
                                                </span>
                                            
                                        </td>
                                        <td>
                                        <asp:RequiredFieldValidator ID="rfvFname" runat="server" ControlToValidate="txtFname"
                                                Display="Dynamic" meta:resourcekey="rfvFname" ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                   <%-- <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMName" Class="lblNormalSkin" runat="server" meta:resourcekey="lblMName"></asp:Label>
                                            <asp:Label ID="Label52" runat="server" Text=" "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox autocomplete="off" ReadOnly="true" ID="txtMName" CssClass="input_dr" runat="server"
                                                MaxLength="40" Width="203px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label35" runat="server" Class="lblRequiredSkin" Visible="false" meta:resourcekey="LblStar"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblLName" Class="lblNormalSkin" runat="server" meta:resourcekey="lblLName"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtLName" CssClass="input_dr" runat="server"
                                                MaxLength="40" Width="203px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvLname" runat="server" ControlToValidate="txtLName"
                                                meta:resourcekey="rfvLname" Display="Dynamic" ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSuffix" runat="server" meta:resourcekey="lblSuffix" Class="lblNormalSkin"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtSuffix" runat="server" CssClass="input_dr"
                                                Width="203px" MaxLength="40"></asp:TextBox>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                       
                                        <td><span class="form_1">
                                            <asp:Label ID="lblAdd1" Class="lblNormalSkin" runat="server" meta:resourcekey="lblAdd1"></asp:Label>
                                            <asp:Label ID="Label1" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label>
                                            </span>
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtAdd1" CssClass="input_dr" runat="server"
                                                MaxLength="30" Width="203px"></asp:TextBox></span>
                                            
                                        </td>
                                        <td>
                                        <asp:RequiredFieldValidator ID="rfvAdd1" meta:resourcekey="rfvAdd1" runat="server"
                                                ControlToValidate="txtAdd1" Display="Dynamic" ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        
                                        <td><span class="form_1">
                                            <asp:Label ID="lblAdd2" Class="lblNormalSkin" runat="server" meta:resourcekey="lblAdd2"></asp:Label>
                                            </span>
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtAdd2" CssClass="input_dr" runat="server"
                                                MaxLength="30" Width="203px"></asp:TextBox>
                                             </span>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                      
                                        <td><span class="form_1">
                                            <asp:Label ID="lblAdd3" Class="lblNormalSkin" runat="server" meta:resourcekey="lblAdd3"></asp:Label>
                                            </span>
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtAdd3" CssClass="input_dr" runat="server"
                                                MaxLength="30" Width="203px"></asp:TextBox>
                                                </span>
                                        </td>
                                        <td>
                                        
                                        </td>
                                    </tr>
                                    <tr>
                                      
                                        <td><span class="form_1">
                                            <asp:Label ID="lblCity" Class="lblNormalSkin" runat="server" meta:resourcekey="lblCity"></asp:Label>
                                            <asp:Label ID="Label2" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label>
                                            </span>
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtCity" CssClass="input_dr" runat="server"
                                                MaxLength="30" Width="203px"></asp:TextBox>
                                                </span>
                                            
                                        </td>
                                        <td>
                                        <asp:RequiredFieldValidator ID="rfvCity" meta:resourcekey="rfvCity" runat="server"
                                                ControlToValidate="txtCity" Display="Dynamic" ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                      
                                        <td style="height: 28px"><span class="form_1">
                                            <asp:Label ID="lblState" Class="lblNormalSkin" runat="server" meta:resourcekey="lblState"></asp:Label>
                                            <asp:Label ID="Label3" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label>
                                            </span>
                                        </td>
                                        <td style="height: 28px"><span class="form_2">
                                            <asp:TextBox ReadOnly="false" autocomplete="off" ID="drpState" CssClass="input_dr" runat="server"
                                                MaxLength="20" Width="203px"></asp:TextBox>
                                            </span>
                                             
                                        </td>
                                        <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" meta:resourcekey="rfvState" runat="server"
                                                ControlToValidate="drpState" Display="Dynamic" ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                       
                                        <td><span class="form_1">
                                            <asp:Label ID="lblZip" Class="lblNormalSkin" runat="server" meta:resourcekey="lblZip"></asp:Label>
                                            <asp:Label ID="Label4" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label>
                                            </span>
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtZip" CssClass="input_dr" runat="server"
                                                MaxLength="10" Width="203px"></asp:TextBox></span>  
    
                                           
                                        </td>
                                        <td>
                                         <asp:RequiredFieldValidator ID="rfvZip" meta:resourcekey="rfvZip" runat="server"
                                                ControlToValidate="txtZip" Display="Dynamic" ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars" ValidChars="1234567890"
                                                TargetControlID="txtZip" />
                                        </td>
                                    </tr>
                                    <tr>
                                       
                                        <td style="height: 28px"><span class="form_1">
                                            <asp:Label ID="lblCountry" Class="lblNormalSkin" runat="server" meta:resourcekey="lblCountry"></asp:Label>
                                            </span>
                                        </td>
                                        <td style="height: 28px"><span class="form_2">
                                            <asp:DropDownList ID="drpCountry" runat="server" Width="210px" Class="DropdownlistSkin"
                                                Enabled="true">
                                            </asp:DropDownList>
                                         </span>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                       
                                        <td><span class="form_1">
                                            <asp:Label ID="lblAltEmail" Class="lblNormalSkin" runat="server" meta:resourcekey="lblAltEmail"></asp:Label>
                                            <asp:Label ID="Label6" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label>
                                            </span>
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtAltEmail" CssClass="input_dr"
                                                runat="server" MaxLength="55" Width="203px"></asp:TextBox>
                                                </span>                                           
                                        </td>
                                        <td>
                                          &nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvemail" runat="server" Class="vldRequiredSkin"
                                                ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Email"
                                                ControlToValidate="txtAltEmail"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="valRegEx" runat="server" ControlToValidate="txtAltEmail"
                                                ErrorMessage="Required" Text="Enter Valid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                Class="vldRegExSkin" Display="Dynamic" ValidationGroup="Mandatory"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        
                                        <td><span class="form_1">
                                            <asp:Label ID="lblPhone" Class="lblNormalSkin" runat="server" meta:resourcekey="lblPhone"></asp:Label>
                                            <asp:Label ID="Label5" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label>
                                            </span>
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtPhone" CssClass="input_dr" runat="server"
                                                MaxLength="16" Width="203px"></asp:TextBox>
                                                </span>
                                            
                                        </td>
                                        <td>
                                        <asp:RequiredFieldValidator ID="rfvPhone" meta:resourcekey="rfvPhone" runat="server"
                                                ControlToValidate="txtPhone" Display="Dynamic" ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>

                                                 <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterMode="ValidChars" ValidChars="1234567890"
                                                TargetControlID="txtPhone" />
                                        </td>
                                    </tr>
                                    <tr>
                                        
                                        <td style="height: 30px"><span class="form_1">
                                            <asp:Label ID="lblMobile" Class="lblNormalSkin" runat="server" meta:resourcekey="lblMobile"></asp:Label>
                                            
                                            </span>
                                        </td>
                                        <td style="height: 30px"><span class="form_2">
                                            <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtMobile" CssClass="input_dr"
                                                runat="server" MaxLength="16" Width="203px"></asp:TextBox></span>
                                                      
                                        </td>
                                        <td>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterMode="ValidChars" ValidChars="1234567890"
                                                TargetControlID="txtMobile" />
                                        </td>
                                    </tr>
                                    <tr>
                                       
                                        <td style="width: 30%"><span class="form_1">
                                            <asp:Label ID="lblFax" Class="lblNormalSkin" runat="server" meta:resourcekey="lblFax"></asp:Label>
                                            </span>
                                        </td>
                                        <td style="width: 67%"><span class="form_2">
                                            <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtFax" CssClass="input_dr" runat="server"
                                                MaxLength="16" Width="203px"></asp:TextBox></span>
                                                        
                                        </td>
                                        <td>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterMode="ValidChars" ValidChars="1234567890"
                                                TargetControlID="txtFax" /></td>
                                    </tr>
                                    
                                  


                                 
                                </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                             <div class="box1">
                           <h3 class="title3">Billing information</h3>
                              <table id="Table1" width="100%" cellpadding="0" cellspacing="0">
                              <%--<tr>
                                        <td colspan="3">
                                            <asp:Label ID="lblBillingTitle" runat="server" meta:resourcekey="lblBillingTitle"
                                                Class="lblStaticSkin"></asp:Label>
                                        </td>
                                    </tr>--%>

                              <tr>
                                        <td colspan="3">
                                            <asp:CheckBox ID="ChkBillingAdd" runat="server" Visible="true" AutoPostBack="true"
                                                Class="CheckBoxSkin" meta:resourcekey="ChkBillTitle" Checked="false" OnCheckedChanged="ChkBillingAdd_CheckedChanged1" />
                                        </td>
                                    </tr>
                                    <%--<tr>
                                        <td style="width: 14px">
                                            <asp:Label ID="Label11" runat="server" Class="lblRequiredSkin" Visible="false" meta:resourcekey="LblStar"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label12" runat="server" Text="Company Name"
                                                Class="lblNormalSkin"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ReadOnly="true" autocomplete="off" CssClass="inputtxt" ID="txtbillCompanyName" runat="server" CssClass="input_dr"
                                                MaxLength="40" Width="203px"></asp:TextBox>                                            
                                        </td>
                                    </tr>--%>
                                     <%--<tr>
                                        <td style="width: 14px">
                                            <asp:Label ID="Label13" runat="server" Class="lblRequiredSkin" Visible="false" meta:resourcekey="LblStar"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label18" runat="server" meta:resourcekey="lblFName"
                                                Class="lblNormalSkin"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ReadOnly="false" autocomplete="off" CssClass="inputtxt" ID="txtbillFName" runat="server" CssClass="input_dr"
                                                MaxLength="40" Width="203px"></asp:TextBox>   
                                                  <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Enter Name"  runat="server"
                                                ControlToValidate="txtbillFName" Display="Dynamic" ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>   
                                                                                      
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td style="width: 30%"><span class="form_1">
                                            <asp:Label ID="lblbilladdress1" runat="server" meta:resourcekey="lblbilladdress1"
                                                Class="lblNormalSkin"></asp:Label>
                                                <asp:Label ID="Label7" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label>
                                                </span>
                                        </td>
                                        <td style="width: 30%"><span class="form_2">
                                            <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtbilladd1" runat="server" CssClass="input_dr"
                                                MaxLength="30" Width="203px"></asp:TextBox></span>
                                                
                                            
                                        </td>
                                        <td>
                                        <asp:RequiredFieldValidator ID="rfvbAdd1" runat="server" ControlToValidate="txtbillAdd1"
                                                Display="Dynamic" meta:resourcekey="rfvAdd1" Class="vldRequiredSkin" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                    
                                        <td><span class="form_1">
                                            <asp:Label ID="lblbilladdress2" runat="server" meta:resourcekey="lblbilladdress2"
                                                Class="lblNormalSkin"></asp:Label>
                                                </span>
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtbilladd2" runat="server" CssClass="input_dr"
                                                MaxLength="30" Width="203px"></asp:TextBox>
                                                </span>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                      
                                        <td><span class="form_1">
                                            <asp:Label ID="lblbilladdress3" runat="server" meta:resourcekey="lblbilladdress3"
                                                Class="lblNormalSkin"></asp:Label></span>
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtbilladd3" runat="server" CssClass="input_dr"
                                                MaxLength="30" Width="203px"></asp:TextBox></span>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        
                                        <td><span class="form_1">
                                            <asp:Label ID="lblbillCity" runat="server" meta:resourcekey="lblbillCity" Class="lblNormalSkin"></asp:Label>
                                            <asp:Label ID="Label8" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label>
                                            </span>
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtbillcity" runat="server" CssClass="input_dr"
                                                MaxLength="30" Width="203px"></asp:TextBox></span>
                                            
                                        </td>
                                        <td>
                                        <asp:RequiredFieldValidator ID="rfvbCity" Class="vldRequiredSkin" runat="server"
                                                ControlToValidate="txtbillcity" meta:resourcekey="rfvCity" Display="Dynamic"
                                                ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                       
                                        <td><span class="form_1">
                                            <asp:Label ID="lblbillState" runat="server" meta:resourcekey="lblbillState" Class="lblNormalSkin"></asp:Label>
                                            <asp:Label ID="Label9" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label>
                                            </span>
                                        </td>
                                        <td><span class="form_2">                                        
                                            <asp:TextBox ReadOnly="false" autocomplete="off" ID="drpBillState" runat="server"
                                                CssClass="input_dr" MaxLength="20" Width="203px"></asp:TextBox>
                                                </span>
                                             
                                        </td>
                                        <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="drpBillState"
                                                Display="Dynamic" ErrorMessage="Enter State"  Class="vldRequiredSkin" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><span class="form_1">
                                            <asp:Label ID="lblbillZip" runat="server" meta:resourcekey="lblbillZip" Class="lblNormalSkin"></asp:Label>
                                            <asp:Label ID="Label11" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label>
                                            </span>
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtbillzip" runat="server" CssClass="input_dr"
                                                MaxLength="10" Width="203px"></asp:TextBox></span>
                                            
                                        </td>
                                        <td>
                                        <asp:RequiredFieldValidator ID="rfvbZip" Class="vldRequiredSkin" runat="server"
                                                ControlToValidate="txtbillzip" meta:resourcekey="rfvZip" Display="Dynamic" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>

                                                 <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" FilterMode="ValidChars" ValidChars="1234567890"
                                                TargetControlID="txtbillzip" />
                                        </td>
                                    </tr>
                                    <tr>
                                       
                                        <td><span class="form_1">
                                            <asp:Label ID="lblbillCountry" runat="server" meta:resourcekey="lblbillCountry" Class="lblNormalSkin"></asp:Label>
                                            </span>
                                        </td>
                                        <td><span class="form_2">
                                            <asp:DropDownList ID="drpBillCountry" runat="server" Enabled="true" Width="210px"
                                                Class="DropdownlistSkin">
                                            </asp:DropDownList>
                                           </span>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                       
                                        <td><span class="form_1">
                                            <asp:Label ID="lblbillPhone" runat="server" meta:resourcekey="lblbillPhone" Class="lblNormalSkin"></asp:Label>
                                            <asp:Label ID="Label12" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label>
                                            </span>
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtbillphone" runat="server"
                                                CssClass="input_dr" MaxLength="16" Width="203px"></asp:TextBox></span>
                                        </td>
                                        <td>
                                        <asp:RequiredFieldValidator ID="rfvbPhone" Class="vldRequiredSkin" runat="server"
                                                ControlToValidate="txtbillphone" meta:resourcekey="rfvPhone" Display="Dynamic"
                                                ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                                   <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" FilterMode="ValidChars" ValidChars="1234567890"
                                                TargetControlID="txtbillphone" />
                                        </td>
                                    </tr>
                                    </table>
                                    </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                             <div class="box1">
                           <h3 class="title3">Shipping Information</h3>
                              <table id="Table2" width="100%" cellpadding="0" cellspacing="0">
                                <%-- <tr>
                                        <td colspan="3">
                                            <asp:Label ID="lblshipTitle" runat="server" meta:resourcekey="lblShippingTitle" Class="lblStaticSkin"></asp:Label>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td colspan="3">
                                            <asp:CheckBox ID="ChkShippingAdd" runat="server" Visible="true" AutoPostBack="true" Enabled="true"  
                                                Class="CheckBoxSkin" Checked="false" meta:resourcekey="ChkShipTitle" OnCheckedChanged="ChkShippingAdd_CheckedChanged" />
                                        </td>
                                    </tr>
                                  <%--  <tr>
                                        <td style="width: 14px">
                                            <asp:Label ID="Label7" runat="server" Class="lblRequiredSkin" Visible="false" meta:resourcekey="LblStar"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label8" runat="server"  Text="Company Name" Class="lblNormalSkin"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ReadOnly="true" autocomplete="off" CssClass="inputtxt" ID="txtShipCompName" runat="server" CssClass="input_dr"
                                                MaxLength="40" Width="203px"></asp:TextBox>                                           
                                        </td>
                                    </tr>--%>
                                    <%--<tr>
                                        <td style="width: 14px">
                                            <asp:Label ID="Label9" runat="server" Class="lblRequiredSkin" Visible="false" meta:resourcekey="LblStar"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label10" runat="server"  meta:resourcekey="lblFName" Class="lblNormalSkin"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ReadOnly="false" autocomplete="off" CssClass="inputtxt" ID="txtShipFname" runat="server" CssClass="input_dr"
                                                MaxLength="40" Width="203px"></asp:TextBox>    
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtShipFname"
                                                Display="Dynamic" ErrorMessage="Enter Name"  Class="vldRequiredSkin" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>                                       
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        
                                        <td style="width: 30%"><span class="form_1">
                                            <asp:Label ID="lblshipadd1" runat="server" meta:resourcekey="lblshipaddress1" Class="lblNormalSkin"></asp:Label>
                                            <asp:Label ID="Label13" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label>
                                            </span>
                                        </td>
                                        <td style="width: 30%"><span class="form_2">
                                            <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtshipadd1" runat="server" CssClass="input_dr"
                                                MaxLength="30" Width="203px"></asp:TextBox></span>

                                        </td>
                                        <td>
                                        
                                            <asp:RequiredFieldValidator ID="rfvsAdd1" runat="server" ControlToValidate="txtshipAdd1"
                                                Display="Dynamic" meta:resourcekey="rfvAdd1" Class="vldRequiredSkin" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                        
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><span class="form_1">
                                            <asp:Label ID="lblshipadd2" runat="server" meta:resourcekey="lblshipaddress2" Class="lblNormalSkin"></asp:Label>
                                            </span>
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtshipadd2" runat="server" CssClass="input_dr"
                                                MaxLength="30" Width="203px"></asp:TextBox></span>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                       
                                        <td><span class="form_1">
                                            <asp:Label ID="lblshipadd3" runat="server" meta:resourcekey="lblshipaddress3" Class="lblNormalSkin"></asp:Label>
                                            </span>
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtshipadd3" runat="server" CssClass="input_dr"
                                                MaxLength="30" Width="203px"></asp:TextBox></span>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        
                                        <td><span class="form_1">
                                            <asp:Label ID="lblshipcity" runat="server" meta:resourcekey="lblshipCity" Class="lblNormalSkin"></asp:Label>
                                            <asp:Label ID="Label14" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label>
                                            </span>
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtshipcity" runat="server" CssClass="input_dr"
                                                MaxLength="30" Width="203px"></asp:TextBox></span>
                                            
                                        </td>
                                        <td>
                                        <asp:RequiredFieldValidator ID="rfvsCity" Class="vldRequiredSkin" runat="server"
                                                ControlToValidate="txtshipcity" meta:resourcekey="rfvCity" Display="Dynamic"
                                                ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                       
                                        <td><span class="form_1">
                                            <asp:Label ID="lblshipState" runat="server" meta:resourcekey="lblshipState" Class="lblNormalSkin"></asp:Label>
                                            <asp:Label ID="Label15" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label>
                                            </span>
                                        </td>
                                        <td>
                                            <span class="form_2">
                                            <asp:TextBox ReadOnly="false" autocomplete="off" ID="drpShipState" runat="server"
                                                CssClass="input_dr" MaxLength="20" Width="203px"></asp:TextBox></span>
                                            
                                        </td>
                                        <td>                                        
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="drpShipState"
                                                Display="Dynamic" ErrorMessage="Enter State"  Class="vldRequiredSkin" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>                                       
                                        </td>
                                    </tr>
                                    <tr>
                                        
                                        <td><span class="form_1">
                                            <asp:Label ID="lblshipZip" runat="server" meta:resourcekey="lblshipZip" Class="lblNormalSkin"></asp:Label>
                                            <asp:Label ID="Label16" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label>
                                            </span>
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtshipzip" runat="server" CssClass="input_dr"
                                                MaxLength="10" Width="203px"></asp:TextBox></span>
                                            
                                        </td>
                                        <td>
                                        <asp:RequiredFieldValidator ID="rfvsZip" Class="vldRequiredSkin" runat="server"
                                                ControlToValidate="txtshipzip" meta:resourcekey="rfvZip" Display="Dynamic" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" FilterMode="ValidChars" ValidChars="1234567890"
                                                TargetControlID="txtshipzip" />
                                        </td>
                                    </tr>
                                    <tr>
                                        
                                        <td><span class="form_1">
                                            <asp:Label ID="lblshipCountry" runat="server" meta:resourcekey="lblshipCountry" Class="lblNormalSkin"></asp:Label>
                                            </span>
                                        </td>
                                        <td><span class="form_2">
                                            <asp:DropDownList ID="drpShipCountry" Enabled="true" runat="server" Width="210px"
                                                Class="DropdownlistSkin">
                                            </asp:DropDownList>
                                            </span>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                       
                                        <td><span class="form_1">
                                            <asp:Label ID="lblshipPhone" runat="server" meta:resourcekey="lblshipPhone" Class="lblNormalSkin"></asp:Label>
                                            <asp:Label ID="Label17" runat="server" Class="lblRequiredSkin" Visible="true"
                                                Text="*" Width="1px"></asp:Label>
                                            </span>
                                        </td>
                                        <td><span class="form_2">
                                            <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtshipphone" runat="server"
                                                CssClass="input_dr" MaxLength="16" Width="203px"></asp:TextBox>
                                                </span>
                                            
                                        </td>
                                        <td>
                                        <asp:RequiredFieldValidator ID="rfvsPhone" Class="vldRequiredSkin" runat="server"
                                                ControlToValidate="txtshipphone" meta:resourcekey="rfvPhone" Display="Dynamic"
                                                ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                                
                                                      <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" FilterMode="ValidChars" ValidChars="1234567890"
                                                TargetControlID="txtshipphone" />
                                        </td>
                                    </tr>

                              </table>
                              </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="558" align="center">
                        <tr>
                            <td width="50%" align="center">
                                <asp:Button ID="btnUpdate" Visible="true" runat="server" meta:resourcekey="btnUpdate"
                                    OnClick="btnUpdate_Click" ValidationGroup="Mandatory" class="button normalsiz btngreen btnmain"/>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        </div>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="rightnav" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="footer" runat="Server">
</asp:Content>
