<%@ Page Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" Inherits="QuoteRevie" Title="Untitled Page" Codebehind="QuoteReview.aspx.cs" %>
<%@ Register TagPrefix ="WebCat" TagName ="Invoice" Src ="~/UC/QuoteInvoice.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="leftnav" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" Runat="Server">
<body onload="history.go(+1)">
 <table align="center" width="558" border="0" cellspacing="0" cellpadding="5">
        <tr>
          <td align="left" class="tx_1">
            <a href="home.aspx" style="color:#0099FF" class="tx_3">Home</a> / Quote Review
          </td>
        </tr>
        <tr>
          <td class="tx_3">
            <hr>
          </td>

        </tr>
      </table>
      <br /> 
        <table align="center" width="558" cellpadding ="0" cellspacing ="0" border="0">        
        <tr class="tablerow" align="center" >                            
                        <td align="center">
                        <asp:Label ID="QteMsg" runat="server"  Class="lblErrorSkin"></asp:Label>
                        </td>
                    </tr>
        <tr align="center">
                    <td>
                     <asp:Label ID="Label1" runat="server" meta:resourcekey="lblg" Class="lblErrorSkin"></asp:Label>
                    <asp:Label ID="lblConfirmQuote" runat="server" Class="lblResultSkin" ></asp:Label>
                    </td></tr>       
        <tr>
            <td>                    
              <WebCat:Invoice ID ="ucInvoice" runat ="server" />
            </td>
        </tr>
        <tr>
            <td align="left">
                 <asp:Label ID="CancelMesg" runat="server" meta:resourcekey="lblReferMsg"  Class="lblComPhoneSkin" ></asp:Label><br />
                 <asp:LinkButton ID="LinktoQuoteList" runat="server" Class="CompanylinkSkin" meta:resourcekey="LinktoQuoteList"  PostBackUrl="~/QuoteList.aspx?ViewType=QUOTELIST" onmouseover="this.style.color='red'" onmouseout="this.style.color='black'" ></asp:LinkButton>
         </td>
        </tr>       
    </table>
   </body>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="rightnav" Runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>

