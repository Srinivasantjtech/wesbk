<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControl_Top"
    EnableTheming="true" Codebehind="Top.ascx.cs" %>
<%@ Register Src="CartItems.ascx" TagName="CartItems" TagPrefix="uc1" %>
<script language="javascript" type="text/jscript">
    //window.history.forward(1);
    function Redirect() {
        var is_URL = "http://www.tradingbell.com";
        var is_Features = "left=0,Top=0,scrollbars=yes,width=990,height=750";
        window.open(is_URL, "WEBCAT", is_Features);
    }
    
    
</script>
<meta http-equiv="content-type" content="text/html; charset=UTF-8" />
<asp:Panel ID="pnlSearch" runat="server">
    <script language="javascript" type="text/javascript">
        function ValidateSearchText(elemt) {
            if (trim(window.document.getElementById("ctl00_TopPage_txtSearch").value) == "") {
                alert('Search text cannot be empty');
                window.document.getElementById("ctl00_TopPage_txtSearch").focus();
                return false;
            }
        }
        function trim(s) {
            while (s.substring(0, 1) == ' ') {
                s = s.substring(1, s.length);
            }
            while (s.substring(s.length - 1, s.length) == ' ') {
                s = s.substring(0, s.length - 1);
            }
            return s;
        }

    </script>
    <table id="tblTop" cellpadding="0" cellspacing="0" border="0" style="width: 862px;
        height: 20px" runat="server">
        <tr id="rowImg">
            <td valign="top" id="cellImg">
                <table id="tblOuter" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td valign="top" style="width: 862px; height: 10px">
                            <table id="tblInner" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td valign="top" height="90" style="width: 400px">
                                        <div style="margin-left: 30px; margin-top: 01px; margin-right: 9px; line-height: 12px"
                                            class="b">
                                            <asp:LinkButton ID="lbtncompany" Text="" runat="server" SkinID="CompanylinkSkin"
                                                PostBackUrl="~/Default.aspx"></asp:LinkButton></div>
                                        <div style="margin-left: 30px; margin-top: 20px">
                                        </div>
                                    </td>
                                    <td valign="top" height="90" style="width: 695px">
                                        <div style="margin-left: 101px; margin-top: 01px; margin-right: 4px; line-height: 12px"
                                            class="b1">
                                            <asp:Image ID="ImgPhone" runat="server" Style="margin-right: 7px" ImageUrl="~/Images/z1.gif"
                                                BorderWidth="0" /><asp:Label ID="lblPhone" SkinID="lblComPhoneSkin" runat="server"></asp:Label>
                                            <asp:Label ID="lblUser" runat="server"></asp:Label></div>
                                        <div style="margin-left: 101px; margin-top: 18px; margin-right: 4px; line-height: 12px">
                                            &nbsp;</div>
                                        <div style="margin-left: 17px; margin-top: 25px; margin-right: 4px; line-height: 12px"
                                            class="w">
                                            <asp:Image ID="imgArrow1" runat="server" ImageUrl="~/Images/top_arrow.gif" Style="margin-right: 4px;" />
                                            <asp:LinkButton ID="lbAccount" runat="server" Text="<%$ Resources:Top,lbAccount %>"
                                                SkinID="TopLevelSkin" PostBackUrl="~/Login.aspx"></asp:LinkButton><asp:Image ID="imgArrow2"
                                                    runat="server" ImageUrl="~/Images/top_arrow.gif" Style="margin-right: 4px; margin-left: 9px" />
                                            <%
                                                string tOrderID = "";
                                                tOrderID = string.IsNullOrEmpty(Session["ORDER_ID"].ToString()) ? "0" : Session["ORDER_ID"].ToString();
                                            %>
                                            <asp:LinkButton ID="lbViewCart" runat="server" SkinID="TopLevelSkin" Text="<%$ Resources:Top,lbViewCart %>"
                                                PostBackUrl="~/orderDetails.aspx?&bulkorder=1&Pid=0&ORDER_ID=<% tOrderID %>"></asp:LinkButton><asp:Image
                                                    ID="imgArrow3" runat="server" ImageUrl="~/Images/top_arrow.gif" Style="margin-right: 4px;
                                                    margin-left: 12px" />
                                            <asp:LinkButton ID="lbWishlist" runat="server" Text="<%$ Resources:Top,lbWishlist%>"
                                                SkinID="TopLevelSkin" PostBackUrl="~/UserBasicInfo.aspx"></asp:LinkButton><asp:Image
                                                    ID="imgArrow4" runat="server" ImageUrl="~/Images/top_arrow.gif" Style="margin-right: 4px;
                                                    margin-left: 12px" />
                                            <asp:LinkButton ID="lborderstatus" runat="server" Text="<%$ Resources:Top,lborderstatus%>"
                                                SkinID="TopLevelSkin" PostBackUrl="~/Orders.aspx?ViewType=STATUS"></asp:LinkButton><asp:Image
                                                    ID="imgArrow5" runat="server" ImageUrl="~/Images/top_arrow.gif" Style="margin-right: 4px;
                                                    margin-left: 12px" />
                                            <asp:LinkButton ID="lbViewWishList" runat="server" SkinID="TopLevelSkin" Text="<%$ Resources:Top,lbViewWishList %>"
                                                PostBackUrl="~/WishListDetails.aspx"></asp:LinkButton><asp:Image ID="imgArrow" runat="server"
                                                    ImageUrl="~/Images/top_arrow.gif" Style="margin-right: 4px; margin-left: 12px" />
                                            <asp:LinkButton ID="lbtnSignOut" runat="server" SkinID="TopLevelSkin" Text="<%$ Resources:Top,lbtnSignOut %>"
                                                OnClick="lbtnSignOut_Click"></asp:LinkButton>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" height="15" style="width: 847px;">
                            <table cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td valign="top" width="565" style="height: 45px">
                                        <div style="margin-left: 45px; margin-top: 25px; margin-right: 4px; line-height: 12px"
                                            class="w">
                                            <strong>
                                                <asp:LinkButton ID="lblHome" runat="server" Text="<%$ Resources:Top,lblHome %>" SkinID="MainLinkSkin"
                                                    PostBackUrl="~/Default.aspx"></asp:LinkButton>
                                                <asp:LinkButton ID="lbCompany" runat="server" Text="<%$ Resources:Top,lbCompany %>"
                                                    SkinID="MainLinkSkin" PostBackUrl="~/Company.aspx"></asp:LinkButton>
                                                <asp:LinkButton ID="lbService" runat="server" Text="<%$ Resources:Top,lbService %>"
                                                    SkinID="MainLinkSkin" PostBackUrl="~/Services.aspx"></asp:LinkButton>
                                                <asp:LinkButton ID="lbSupplier" runat="server" Text="<%$ Resources:Top,lbSupplier %>"
                                                    SkinID="MainLinkSkin" PostBackUrl="~/SupplierList.aspx"></asp:LinkButton>
                                                <asp:LinkButton ID="lbPromotion" runat="server" Text="<%$ Resources:Top,lbPromotion %>"
                                                    SkinID="MainLinkSkin" PostBackUrl="~/Promotion.aspx"></asp:LinkButton>
                                                <asp:LinkButton ID="lbContacts" runat="server" Text="<%$ Resources:Top,lbContacts %>"
                                                    SkinID="MainLinkSkin" PostBackUrl="~/Contact.aspx"></asp:LinkButton>
                                            </strong>
                                        </div>
                                    </td>
                                    <td valign="top" width="121" style="height: 45px" align="right">
                                        <div style="margin-left: 2px; margin-top: 23px">
                                            <asp:Label ID="lblSearch" runat="server" SkinID="lblSearchSkin" Text="<%$ Resources:Top,lblSearch%>"></asp:Label>
                                        </div>
                                    </td>
                                    <td valign="top" width="121" style="height: 45px">
                                        <div style="margin-left: 2px; margin-top: 23px" align="center">
                                            <asp:TextBox autocomplete="off" ID="txtSearch" runat="server" Height="12px" Width="115px"
                                                MaxLength="85" Font-Size="10px" Font-Names="Arial Unicode MS, Arial" CausesValidation="True"></asp:TextBox><br />
                                            <asp:LinkButton ID="lbAdvSearch" SkinID="SearchLinkSkin" PostBackUrl="~/search.aspx"
                                                runat="server" Font-Underline="False" meta:resourcekey="lbAdvSearch"></asp:LinkButton>
                                        </div>
                                    </td>
                                    <td valign="top" width="80" style="height: 45px">
                                        <div style="margin-left: 0px; margin-top: 23px">
                                            <asp:ImageButton ID="cmdSearch" OnClientClick="return ValidateSearchText(this);"
                                                runat="server" ImageUrl="~/Images/go.gif" /><%--  OnClick="cmdSearch_Click"    OnClientClick="return ValidateSearchText();" --%>
                                            <%--<input type="image" src="../Images/go.gif" name="cmdSearch" id="cmdSearch" value="Submit" alt="Submit" onclick="javascript:return cmdSearch_onclick();" />--%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
