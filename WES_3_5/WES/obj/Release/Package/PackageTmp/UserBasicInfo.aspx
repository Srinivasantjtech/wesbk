<%@ Page Language="C#" MasterPageFile="~/mainpage.master" AutoEventWireup="true" Inherits="UserBasicInfo" Title="Untitled Page"  Culture ="auto:en-US" UICulture ="auto" Codebehind="UserBasicInfo.aspx.cs" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="header" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="leftnav" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" Runat="Server">
<%--<%@ Register TagPrefix="WebCat" Namespace="TradingBell.WebServices" %>--%>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %>
<
<script type="text/javascript">
  function PwdValidate(oSrc, args)
  {    
   args.IsValid = (args.Value.length >= 6);   
  }
  function ValidMail()
  {
    var txtVal=window.document.forms[0].Elements["<%=txtEmail.ClientID%>"].value;
    var re = /\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/; 
   if(txtVal.search(re) == -1) 
   {
      return false;
   }   
  }
  
  function capLock(e)
  {
     kc = e.keyCode?e.keyCode:e.which;
     sk = e.shiftKey?e.shiftKey:((kc == 16)?true:false);
     if(((kc >= 65 && kc <= 90) && !sk)||((kc >= 97 && kc <= 122) && sk))
        document.getElementById('divMayus').style.visibility = 'visible';
     else
        document.getElementById('divMayus').style.visibility = 'hidden';
    }

</script>
<table align="center" width="100%" border="0" cellspacing="0" cellpadding="5">
        <tr>
          <td align="left" class="tx_1">
            <a href="home.aspx" style="color:#0099FF" class="tx_3">Home</a><font style="font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal"> / </font>UserBasicInfo
          </td>
        </tr>
        <tr>
          <td class="tx_3">
            <hr/>
          </td>

        </tr>
      </table>
      <br />
<table>
<tr>
<td align="left">
<asp:Label ID="Label4" runat="server" Class="lblRequiredSkin"   meta:resourcekey="LblStar"   Width="1px"></asp:Label>&nbsp;
                <asp:Label ID="Label5" runat="server" meta:resourcekey="LblReqField" Class="lblNormalSkin"></asp:Label>
</td>
</tr>
</table>
<table>
<tr><td>
<table align=left class="BaseTblBorder" width="558">
<tr>
    <td  class="tx_6" height="20px" background="images/17.gif" align="left">
       <asp:Label ID="lblHeader" runat="server" meta:resourcekey="lblHeader" Font-Size="Small"></asp:Label>


    </td>
  </tr>
<tr>
<td>
<asp:Panel ID="pnlBasic" runat="server" DefaultButton="btnSubmit">
<table width="572">
      <tr>
        <td align="left">
         <asp:Label ID="Label1" runat="server" Class="lblRequiredSkin"   meta:resourcekey="LblStar"></asp:Label>
        </td>
        <td align="left">
            <asp:Label ID="lblEmail" runat="server" meta:resourcekey="lblEmail" Class="lblStaticSkin"></asp:Label>
        </td>
        <td style="width: 400px" align="left">
            <asp:TextBox  autocomplete="off"  ID="txtEmail" runat="server" Class="textSkin" MaxLength="150" ValidationGroup="EmailValidation" Width="150px"  ></asp:TextBox>
            <asp:LinkButton ID="lbCheckAvail" runat="server" meta:resourcekey="lbCheckAvail" OnClientClick ="return ValidMail();"  OnClick="lbCheckAvail_Click" Class="CommonLinkSkin"></asp:LinkButton>
            <asp:RequiredFieldValidator id="rfvUserId" runat="server" Class="vldRequiredSkin" ValidationGroup="Mandatory" Display="Dynamic" ControlToValidate="txtEmail" meta:resourcekey="rfvUserId" SetFocusOnError="true"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="valRegEx" runat="server" ControlToValidate="txtEmail" meta:resourcekey="valRegEx"  ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"  class="vldRegExSkin" Display="Dynamic" ValidationGroup="Mandatory" SetFocusOnError="true"></asp:RegularExpressionValidator>&nbsp;
            <asp:Label ID="lblExampleId" runat="server" meta:resourcekey="TxtExMail" Class="lblGraySkin"></asp:Label>
        </td>
        </tr>
       <tr>
       <td></td>
       <td></td>
       <td align="left">
            <asp:Label ID="lblchkavail" runat="server" Text="" Class="lblResultSkin"></asp:Label></td>
       </tr>
        <tr>
        <td width="3%" align="left">
          <asp:Label ID="Label2" runat="server" Class="lblRequiredSkin"   meta:resourcekey="LblStar"   ></asp:Label>
        </td>
        <td width="27%" align="left">
            <asp:Label ID="lblPassword" runat="server" meta:resourcekey="lblPassword" Class="lblStaticSkin" ></asp:Label>
        </td>
        <td style="width: 400px" align="left">
        <asp:Label ID="lblErrorMsgUsrID" Class="vldRequiredSkin" ValidationGroup="Mandatory" Display="Dynamic" ForeColor="red" runat="server"></asp:Label>
        <br />
            <asp:TextBox  autocomplete="off"  ID="txtPassword" runat="server" Class="textSkin" TextMode="Password" Width="150px" MaxLength="50" onkeypress="capLock(event)"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvPwd" runat="server" ControlToValidate="txtPassword"
                Display="Dynamic" meta:resourcekey="rfvPwd" Class="vldRequiredSkin"  ValidationGroup="Mandatory" SetFocusOnError="true"></asp:RequiredFieldValidator>
            <asp:CustomValidator ID="vldCustom" runat="server" ControlToValidate="txtPassword"
                ValidationGroup="Mandatory" ClientValidationFunction="PwdValidate" meta:resourcekey ="valPwdMin" Class="vldCustomSkin" SetFocusOnError="true"></asp:CustomValidator>
        </td>
        </tr>
       
        <tr>
        <td style="height: 26px" align="left">
            <asp:Label ID="Label3" runat="server" Class="lblRequiredSkin"   meta:resourcekey="LblStar"   ></asp:Label>
        </td>
        <td style="height: 26px" align="left">
            <asp:Label ID="lblConfirmPassword" runat="server" meta:resourcekey="lblConfirmPassword" Class="lblStaticSkin"></asp:Label>
        </td>
        <td style="height: 26px; width: 400px;" align="left">
            <asp:TextBox  autocomplete="off"  ID="txtConfirmPassword" runat="server" Class="textSkin" TextMode="Password" Width="150px" Rows="50" MaxLength="50" onkeypress="capLock(event)"></asp:TextBox>
            <asp:RequiredFieldValidator id="rfvConfirmPwd" runat="server" Class="vldRequiredSkin" ValidationGroup="Mandatory" Display="Dynamic" meta:resourcekey="rfvConfirmPwd" ControlToValidate="txtConfirmPassword" SetFocusOnError="true"></asp:RequiredFieldValidator>
            <asp:CompareValidator id="cvPwd" runat="server" Class="vldCompareSkin"  meta:resourcekey="cvPwd" ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword" ValidationGroup="Mandatory" SetFocusOnError="true"></asp:CompareValidator>
        </td>
        </tr>
       <tr>
        <td style="height: 26px" >
       </td>
        <td style="height: 26px" align="left">
            <asp:Label ID="lblCompany" runat="server" meta:resourcekey="lblCompany" Class="lblStaticSkin"></asp:Label>
        </td>
        <td style="width: 400px; height: 26px;" align="left">
            <asp:TextBox  autocomplete="off"  ID="txtCompany" runat="server" Class="textSkin" MaxLength="30" CausesValidation="True" Width="150px"   ></asp:TextBox>
            <%
                CompanyGroupServices objCompanyGroupServices = new CompanyGroupServices();
                objCompanyGroupServices.CompID_Type = "auto";
                if (objCompanyGroupServices.CompID_Type.ToLower() == "auto")
                {
            %>
            <asp:RangeValidator ID="rvCompany" runat="server"  meta:resourcekey="rvCompany" ControlToValidate="txtCompany" Display="Dynamic" MaximumValue="9999999999" MinimumValue="0" ValidationGroup="Mandatory" Class="vldRangeSkin" SetFocusOnError="true" ></asp:RangeValidator>
            <%
                }
            %>
         </td>
        </tr>
    <tr>
        <td style="height: 26px">
        </td>
        <td style="height: 26px">
        </td>
        <td style="width: 400px; height: 26px" align="left">
         <asp:Label ID="lbCompanyInfo" runat="server" meta:resourcekey="lbCompanyInfo" Class="lblGraySkin"></asp:Label></td>
    </tr>
       
        
        </table> 
      </asp:Panel>  
        </td>
        </tr>              
   </table>
   </td>
   </tr>   
   <tr><td>
   <table width="558px" align="right">
        <tr>
        <td align ="right">
            <asp:Button ID="btnSubmit" Class="btnNormalSkin"  runat="server" meta:resourcekey="btnSubmit" ValidationGroup="Mandatory" OnClick="btnSubmit_Click" />
        </td>
        </tr> 
    </table>
    </td></tr>
    </table> 
    <div id="divMayus" style="visibility:hidden; color: red;" align="center"><b>Caps Lock is on.</b></div>
    <asp:Label ID="lblcompanycheck" runat="server" text="" Class="lblResultSkin"></asp:Label>     
    </asp:Content>
    

<asp:Content ID="Content7" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>

