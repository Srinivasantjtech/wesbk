<%@ Control Language="C#" AutoEventWireup="true" Inherits="UI_CategoryNavigator" Codebehind="CategoryNavigator.ascx.cs" %>
<%@ Import Namespace  ="TradingBell.Common"  %>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%
    Helper oHelper = new Helper();
    if (oHelper.GetOptionValues("ECOMMERCEENABLED").ToString() == "YES")
    {  
    
%>
 <table>
        <tr>
            <td valign ="top" align ="left" style="width: 200px;">
                <table cellpadding="0" cellspacing="0" border="0" style="height:100% ">
					<tr>
						<td valign="top" width="197" height="7" style="background-image:url(images/line_top.gif) "></td>
					</tr>
					<tr>
					    <td valign="top" width="197" style="height:50px; background-image:url(images/1_line2.gif) ">
					        <div style="margin-left:05px">
                                <asp:Label ID="lblCart" runat="server" SkinID ="lblBoldSkin" Text="<%$ Resources:CategoryNavigator, lblCart %>"></asp:Label><br />
                             </div>
                             <div style="margin-left:30px">
                                <asp:Label ID="lblItem" runat="server" SkinID ="lblNormalSkin" Text="<%$ Resources:CategoryNavigator, lblItem %>"></asp:Label>
                                <asp:Label ID="lblItemCount" runat="server" SkinID ="lblNormalSkin" Text=""></asp:Label>
                                <br />
                             </div>
                             <div style="margin-left:43px">
                                <asp:Label ID="lblCost" runat="server" SkinID="lblNormalSkin" Text="<%$ Resources:CategoryNavigator, lblCartSubTotal %>"></asp:Label>
                                <asp:Label ID="lblCostValue" runat="server" SkinID="lblNormalSkin" Text=""></asp:Label>
                                <br />
					         </div>
						</td>
					</tr>
				    <tr>
				        <td valign="top" width="197" height="6" style="background-image:url(images/1_line3.gif) "></td>
				    </tr>
				</table>
			    </td>
			   </tr>
		</table>
		<%} %>
		<table>
		    <tr>
		        <td height="0px"></td>
		    </tr>
		</table>
		<table>
        <tr>
            <td valign ="top" align ="left" style="width: 200px" >
                <table cellpadding="0" cellspacing="0" border="0" style="height:100% ">
					<tr>
						<td valign="top" width="197" style="background-image:url(images/line_top.gif) ; height: 7px;"></td>
					</tr>
					<tr>
					    <td valign="top" width="197" style="height:110px; background-image:url(images/1_line2.gif) ">
					        <div style="margin-left:2px; margin-top:0px;overflow:auto;width:189px;height:500px;">
               <asp:TreeView ID="TVCategory" SkinID="TrvSkin" runat="server" ShowLines="TRUE"  >
               </asp:TreeView>
             </div>
						</td>
					</tr>
				    <tr>
				        <td valign="top" width="197" height="6" style="background-image:url(images/1_line3.gif) "></td>
				    </tr>
				</table>
			    </td>
			   </tr>
		</table>