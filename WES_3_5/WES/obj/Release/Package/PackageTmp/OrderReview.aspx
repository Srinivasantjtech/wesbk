<%@ Page Language="C#" MasterPageFile="~/mainpage.master" AutoEventWireup="true" Inherits="OrderReview" Title="Untitled Page"  Culture ="auto:en-US" UICulture ="auto"  Codebehind="OrderReview.aspx.cs" %>
<%@ Register TagPrefix ="WebCat" TagName ="Invoice" Src ="~/UI/Invoice.ascx" %>
<%@ Import Namespace ="System.Data" %>
<%--<%@ Import Namespace ="TradingBell.Common" %>
<%@ Import Namespace ="TradingBell.WebServices" %>--%>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="header" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="leftnav" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" Runat="Server">
<script type = "text/javascript" language ="javascript">
    window.history.forward(1);
</script>
<table align="center" width="558" border="0" cellspacing="0" cellpadding="5">
        <tr>
          <td align="left" class="tx_1">
            <a href="home.aspx" style="color:#0099FF" class="tx_3">Home</a><font style="font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal"> / </font>Order Review
          </td>
        </tr>
        <tr>
          <td class="tx_3">
            <hr>
          </td>

        </tr>
      </table>
      <br />
    <table align=center width="558" >
        <!--Site Map-->
            <tr class="tablerow" align="left">
                            <td class="StaticText" style="width:85%; height: 40px;" align="left"  >
                                <b><asp:Label ID="lblCheck" runat ="Server" meta:resourcekey="lblCheck"></asp:Label></b>
                                <asp:Label ID="lblShoppingCart" runat ="Server" meta:resourcekey="lblShoppingCart"></asp:Label>
                               >
                                <asp:Label ID="lblShip" runat ="Server" meta:resourcekey="lblShip"></asp:Label> 
                               >
                                <asp:Label ID="lblBill" runat ="Server" meta:resourcekey="lblBill"></asp:Label>
                               >
                                <b><asp:Label ID="lblReviewOrder" runat ="Server" meta:resourcekey="lblReviewOrder" ForeColor="Blue"></asp:Label></b>
                                >
                                <asp:Label ID="lblConfirm" runat ="Server" meta:resourcekey="lblConfirm" ></asp:Label>
                            </td>
                            <td align="right">
                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<asp:Button ID="btnNextTop" runat ="server" class="btnNormalSkin" meta:resourcekey="btnNext" OnClick="btnNext_Click" Width="69px"/></td>

                        </tr>
        <!--Site Map end up here-->
        
        <!--Content -->
            <tr>
                <td colspan="2">
                  <WebCat:Invoice ID ="ucInvoice" runat ="server" />
                </td>
            </tr>
        <!--Content End up here-->    
        <!--Proceed to next Page-->
            <tr>
                <td align="right" colspan="4">
                    <table>
		                <tr class="TableRow" >
		                     <td style="width: 102px; height: 26px">
                                 <asp:Button ID="btnCancel" runat="server" class="btnNormalSkin"   meta:resourcekey="btnCancel" OnClick="btnCancel_Click" />
		                     </td>   
					            <td style="height: 26px" align="right">
					                <asp:Button ID="btnNext" runat ="Server" class="btnNormalSkin" meta:resourcekey="btnNext" OnClick="btnNext_Click" Width="69px"/>&nbsp;</td>
				        </tr>
                        <tr class="TableRow">
                            <td align="left" colspan="2" style="height: 26px">
					                <asp:Label ID="lblmsghere" runat="server" Class="lblGraySkin" meta:resourcekey="lblMessage" Width="176px"></asp:Label></td>
                        </tr>
		            </table>
                </td>
            </tr>
        <!--Proceed to next page end up here-->
    </table>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="rightnav" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>
