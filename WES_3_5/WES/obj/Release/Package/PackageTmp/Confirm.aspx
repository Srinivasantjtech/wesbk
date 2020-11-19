<%@ Page Language="C#" MasterPageFile="~/mainpage.master" AutoEventWireup="true" Inherits="Confirm" Title="Untitled Page"  Culture ="auto:en-US" UICulture ="auto" Codebehind="Confirm.aspx.cs" %>
<%@ Register TagPrefix ="WebCat" TagName ="Invoice" Src ="~/UI/Invoice.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="header" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="leftnav" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" Runat="Server">

<script type = "text/javascript" >

</script>
<table align=center width="568" border="0" cellspacing="0" cellpadding="5">
        <tr>
          <td align="left" class="tx_1">
            <a href="home.aspx" style="color:#0099FF" class="tx_3">Home</a><font style="font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal"> / </font>Confirm
          </td>
        </tr>
        <tr>
          <td class="tx_3">
            <hr>
          </td>

        </tr>
      </table>    
<table class="popupbody" id ="tblBase" align ="center" width="558" border ="0px" cellpadding="3" cellspacing="0">    
      <tr>
            <td>
                <table class="popupbody" id ="tblConfirm"  align ="left" width="558" border ="0px" cellpadding="3" cellspacing="0">
                    <tr class="tablerow">
                        <td class="StaticText" align="left">
                            <b><asp:Label ID="lblCheck" runat ="Server" meta:resourcekey="lblCheck" 
                                Visible="False"></asp:Label></b>
                            <asp:Label ID="lblShoppingCart" runat ="Server" 
                                meta:resourcekey="lblShoppingCart" Visible="False"></asp:Label>
                            <asp:Label ID="lblShip" runat ="Server" meta:resourcekey="lblShip" 
                                Visible="False"></asp:Label> 
                            <asp:Label ID="lblBill" runat ="Server" meta:resourcekey="lblBill" 
                                Visible="False"></asp:Label>
                            <asp:Label ID="lblReviewOrder" runat ="Server" meta:resourcekey="lblReviewOrder" Visible="False"></asp:Label>
                            <b><asp:Label ID="lblConfirmtt" runat ="Server" meta:resourcekey="lblConfirm" 
                                ForeColor="Blue" Visible="False" ></asp:Label></b>
                            <b><asp:Label ID ="lblPageHead" runat ="server" meta:resourcekey="lblPageHead" 
                                Visible="False"></asp:Label></b>
                        </td>
                        <td align ="left">
                            <img  src="Images/Print.gif" id="imgPrint" runat ="server" onclick="OpenInvoice();" style="cursor:pointer;"/><asp:LinkButton ID="lbtnPrint" meta:resourcekey="btnPrint" runat ="Server" class="CompanylinkSkin" OnClientClick ="OpenInvoice();"></asp:LinkButton>
                        </td>
                    </tr>   	           	       
                    <tr>
                        <td align="left">                    
                            <asp:Label ID="lblConfirm1" runat="Server" class="lblResultSkin" ></asp:Label>	
                            <asp:Label ID="lblConfirmValue" runat="Server" class="lblErrorSkin" ></asp:Label>	
                            <asp:Label ID="lblConfirm2" runat="Server" class="lblResultSkin" ></asp:Label>	
                        </td>
                    </tr>
                    </table>  
            </td>
        </tr>
        <tr>
            <td> 
                <WebCat:Invoice ID ="ucInvoice" runat ="server"  />
            </td>
        </tr>
</table>
       
</asp:Content>
      <asp:Content ID="Content6" ContentPlaceHolderID="rightnav" Runat="Server">
      </asp:Content>
      <asp:Content ID="Content7" ContentPlaceHolderID="footer" Runat="Server">
      </asp:Content>

