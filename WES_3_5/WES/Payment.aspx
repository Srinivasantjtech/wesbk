<%@ Page Language="C#" MasterPageFile="~/mainpage.master" AutoEventWireup="true" Inherits="payment" Title="Untitled Page"  Culture ="auto:en-US" UICulture ="auto"  Codebehind="payment.aspx.cs" %>
<%@ Register Assembly="GCheckout" Namespace="GCheckout.Checkout" TagPrefix="GCCheckout" %>
<%--<%@ Import Namespace ="TradingBell.Common" %>
<%@ Import Namespace ="TradingBell.WebServices" %>--%>
<%@ Import Namespace="System.Net" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %> 
<asp:Content ID="Content2" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="header" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="leftnav" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" Runat="Server">

<script type="text/javascript" >
function OpenCsc()
{
    var is_Url = "Csc.aspx";
    var is_Features = "left=300,top=300,height=400,width=600";
    window.open(is_Url,"WEBCAT",is_Features);
}
 </script>
    
<%
            //Helper oHelper = new Helper();
            //User oUsr = new User(); 
           // int UsrManualVerifyStatus = (int)TradingBell.Common.User.UserStatus.MANUALVERIFY; 
           // if (oUsr.GetUserStatus(oHelper.CI(Session["USER_ID"].ToString())) == UsrManualVerifyStatus)
                { 
%>
<table align="center" width="558" border="0" cellspacing="0" cellpadding="5">
        <tr>
          <td align="left" class="tx_1">
            <a href="home.aspx" style="color:#0099FF" class="tx_3">Home</a><font style="font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal"> / </font>Payment
          </td>
        </tr>
        <tr>
          <td class="tx_3">
            <hr>
          </td>

        </tr>
      </table>
      <br />
        <table align="center" width="558" cellpadding ="0" cellspacing = "0" border = "0">
            <!--Site Map-->
           <tr class="tablerow">
                            <td class="StaticText" align="left">
                                <b><asp:Label ID="lblCheck" runat ="Server" meta:resourcekey="lblCheck"></asp:Label>:</b>
                                <asp:Label ID="lblShoppingCart" runat ="Server" meta:resourcekey="lblShoppingCart"></asp:Label>
                                >
                                <asp:Label ID="lblShip" runat ="Server" meta:resourcekey="lblShip"></asp:Label> 
                                >
                                <b><asp:Label ID="lblBill" runat ="Server" meta:resourcekey="lblBill" ForeColor="Blue"></asp:Label></b>
                                >
                                <asp:Label ID="lblReviewOrder" runat ="Server" meta:resourcekey="lblReviewOrder"></asp:Label>
                                > 
                                <asp:Label ID="lblConfirm" runat ="Server" meta:resourcekey="lblConfirm" ></asp:Label>
                            </td>                           
                        </tr>
                        </table>                    
                        <table width="558" border="1">		                
		                 </table>
                      <%}
                     //   else
                        { %>
		                    <table id ="tblBase" align ="center" width="558" class="BaseTblBorder" border ="0px" cellpadding="3" cellspacing="0">
			                <tr>
			                    <td background = "images/17.gif" class="TableRowHead" colspan = "2" align="left"><asp:Label id="lblCardDetails" runat="server" Text = "Payment Options" ></asp:Label></td>
			                </tr>
			                <tr>
			                    <td align="left"><asp:Label ID ="lblPONumber" runat ="server" meta:resourcekey ="lblPONumber" Class="lblStaticSkin"></asp:Label></td>
			                    <td align="left"><asp:TextBox ID ="txtPoNo" runat ="server" MaxLength="40" Class="textSkin"></asp:TextBox></td>
			                </tr>
			                <tr>
			                    <td align="left"><asp:Label ID ="lblPOReleaseNumber" runat ="server" meta:resourcekey ="lblPOReleaseNumber" Class="lblStaticSkin" ></asp:Label></td>
			                    <td align="left"><asp:TextBox ID ="txtPoRNo"  MaxLength="40" runat ="server" Class="textSkin"></asp:TextBox></td>
			                </tr>
			                <tr>
			                    <td align="left" style="height: 28px"><asp:Label ID ="lblPaymentType" runat ="server" meta:resourcekey ="lblPaymentType" Class="lblStaticSkin"></asp:Label></td>
			                    <td align="left" style="height: 28px"><asp:DropDownList ID ="cmdPaymentOptions" runat ="server" AutoPostBack ="true" OnSelectedIndexChanged="cmdPaymentOptions_SelectedIndexChanged" Width="155px" >
			                        </asp:DropDownList>
			                    </td>
			                </tr>
			                <tr>
			                    <td width="200px">			                    
			                    </td>
			                    <td align = "right" colspan ="2" width="200px"> &nbsp;<GCCheckout:GCheckoutButton ID ="btnGCheckout" runat = "server" height="43px" OnClick="btnGCheckout_Click" Width="160px" />
			                     </td>
			                </tr>
			                <tr>
			                    <td colspan="2" align="center">&nbsp;<asp:Label ID ="lblNoPayoptions" runat ="server" Class="lblResultSkin"  meta:resourcekey ="lblNoPayoptions"></asp:Label>    </td>
			                </tr>			                
			                </table>
			                 <%
                                if (cmdPaymentOptions.SelectedValue == "CC")
                                { %>
			                &nbsp;
<%}
                 %><%} %>
                 <table width="558" cellpadding ="0" cellspacing = "0" border = "0">
                 <tr>
			        <td align="right">   
			            <asp:Button ID ="btnNext" Class="btnNormalSkin"  runat ="server" meta:resourcekey="btnNext" OnClick="btnProceed_Click" />
			        </td>
			     </tr>
                 </table>       
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="rightnav" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>
