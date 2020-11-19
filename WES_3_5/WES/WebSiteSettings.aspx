<%@ Page Title="" Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" CodeBehind="WebSiteSettings.aspx.cs" Inherits="WebSiteSettings" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="leftnav" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" runat="server">
 
        <table align ="center"  width="558" border="0" cellspacing="0" cellpadding="5">
        <tr>
          <td align="left" class="tx_1">
            <a href="home.aspx" style="color:#0099FF" class="tx_3">Home</a><font style="font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal"> / </font>Checkout Options
          </td>
        </tr>
        <tr>
           
          <td class="tx_3">
            <hr>
          </td>

        </tr>
      </table>
        
        <br />
         <table cellpadding="2" cellspacing="3" border="0" align="left">
             <tr>
                 <td colspan="3" >
                     <p style="font-weight:bold;margin-left:7px;">Check Out Options</p>
                      
                 </td>
             </tr>
             <tr>
                            <td align="center">
                               <%-- <input id="chkcheckoutoptions1" type="checkbox" runat="server"  />--%>
                                
              &nbsp; <asp:CheckBox id="chkcheckoutoptions" runat="server"
                    AutoPostBack="true"
                    Text=""
                    TextAlign="Right" OnCheckedChanged="btnSubmit_Click"
                    />
                            </td>
                 <td>

                     Set 'Order No' Field as Mandatory at Check Out
                 </td>
                        <%--    <td class="tx_1">
                                <table width="100%" cellspacing="0" cellpadding="0" border="0" style="border-collapse: collapse">
                                    <tr>
                                        <td>
                                           
                                                <div style="line-height: 14px;color:#333">
                                                <span class="txtnorm">
                                                 Set 'Invoice No' Field as Mandatory at Check Out
                                                </span>
                                                </div> 
                                        </td>
                                        <td>
                                            
                                        </td>
                                    </tr>
                                </table>
                            </td>--%>
                        </tr>
            
             <tr>
                            <td align="center">
                            
                                
              &nbsp; <asp:CheckBox id="ChkDuporder" runat="server"
                    AutoPostBack="true"
                    Text=""
                    TextAlign="Right" OnCheckedChanged="btnorderman_Click"
                    />
                            </td>
                 <td>

                    Show warning when Duplicate Order No Used
                 </td>
                       
                        </tr>
             
             
              <tr>
                 <td></td>
                <%-- <td>
                       <asp:Button ID="btnSubmit" runat ="Server" class ="btnNormalSkin"  Text="Submit" Font-Size="11px"  CausesValidation="false" UseSubmitBehavior="true" OnClick="btnSubmit_Click"/>
                 </td>--%>
             </tr>
         </table>
 
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Popupcontent" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="rightnav" runat="server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
