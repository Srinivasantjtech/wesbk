<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControl_Bottom" Codebehind="Bottom.ascx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">


       <table border ="0" width ="865px">
            <tr>
                <td  align ="center" valign="top" width="850px" style="background-image:url(images/bottom.gif); height: 9px;">
                </td>
            </tr>
        </table>
        <table border="0" width ="850px" >
            <tr>
                <td>
                    <table align ="center">
                        <tr>
                            <td>
                                <asp:LinkButton ID="lbSiteTerms" runat="Server" SkinID ="CompanylinkSkin" Text="<%$ Resources:Bottom, lbSiteTerms %>"  CssClass ="LinkButtonStyle" PostBackUrl ="~/terms.aspx" ></asp:LinkButton>
								   
								    <asp:LinkButton ID="lbTerms" runat="Server" SkinID ="CompanylinkSkin" Text="<%$ Resources:Bottom,lbTerms %>" CssClass ="LinkButtonStyle" PostBackUrl ="~/siteterms.aspx"></asp:LinkButton>
								   
								    <asp:LinkButton ID="lbPrivacy" runat="Server" SkinID ="CompanylinkSkin" Text="<%$ Resources:Bottom,lbPrivacy %>" CssClass ="LinkButtonStyle" PostBackUrl ="~/privacy.aspx"></asp:LinkButton>
								    
								    <asp:LinkButton ID="lbAboutUs" runat="Server" SkinID ="CompanylinkSkin" Text="<%$ Resources:Bottom,lbAboutUs %>" CssClass ="LinkButtonStyle" PostBackUrl ="~/company.aspx"></asp:LinkButton>
								    
								    <asp:LinkButton ID="lbContactUs" runat="Server" SkinID ="CompanylinkSkin" Text="<%$ Resources:Bottom,lbContactUs %>" CssClass ="LinkButtonStyle" PostBackUrl ="~/Contact.aspx"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td class="b1" align ="center" >
                            <asp:Label ID="lblCopyRights" runat="Server" SkinID ="CopyRightsSkin" Text="<%$ Resources:Bottom,lbCopyRights %>" CssClass ="LinkButtonStyle"></asp:Label></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
            <td align="right">
            <asp:ImageButton id="ImageButton2" runat="server" OnClientClick="javascript:window.open('http://tradingbell.com',null, 'status=yes,scrollbars=yes, toolbar=yes, menubar=yes, location=no,resizable=yes'); void('');return false" ImageUrl="~/images/Logo.png">
            </asp:ImageButton><asp:LinkButton id="LinkButton1" runat="server" ForeColor="black" Font-Size="7pt" Font-Names="Verdana" OnClientClick="javascript:window.open('http://tradingbell.com',null, 'status=yes,scrollbars=yes, toolbar=yes, menubar=yes, location=no,resizable=yes'); void('');return false" Text="eCommerce Powered by TradingBell WebCat" Font-Underline="false"></asp:LinkButton>
            </td>
            </tr>
         </table>
