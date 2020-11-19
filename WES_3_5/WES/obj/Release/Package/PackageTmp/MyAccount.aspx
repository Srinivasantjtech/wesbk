<%@ Page Language="C#" MasterPageFile="~/mainpage.master" AutoEventWireup="true" Inherits="MyAccount"  Culture="en-US"
    UICulture="en-US" Codebehind="MyAccount.aspx.cs" %>

<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %> 
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="header" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="leftnav" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" runat="Server">
    <script type="text/javascript" language="javascript">
//        window.history.forward(1);
    </script>
    <%

        if (Session["USER_ID"].ToString() != null && Session["USER_ID"].ToString() != "")
        {
    %>
    <table width="100%" cellspacing="5" cellpadding="0" border="0" align="center">
        <tr>
            <td align="left">
                <a href="home.aspx" style="color: #0099FF" class="tx_3">Home</a> <font style="font-family: Arial, Helvetica, sans-serif;
                    font-weight: bolder; font-size: small; font-style: normal"> 
                / 
                </font>MyAccount
            </td>
        </tr>
        <tr>
            <td align="left">
                <hr />
            </td>
        </tr>
    </table>
    <table width="100%" cellspacing="0" cellpadding="0" border="0" height="440px" align="center">
        <tbody>
            <tr>
                <td width="10" height="10">
                    <img alt="" height="17" src="Images/tbl_topLeft.gif" width="10" />
                </td>
                <td background="Images/tbl_top.gif" height="10">
                    <img alt="" height="17" src="Images/tbl_top.gif" width="10" />
                </td>
                <td width="10" height="10">
                    <img alt="" height="17" src="Images/tbl_topRight.gif" width="10" />
                </td>
            </tr>
            <tr>
                <td width="10" background="Images/tbl_left.gif">
                    <img alt="" height="10" src="Images/tbl_left.gif" width="10" />
                </td>
                <td valign="top">
                    <table align="left" border="0" width="100%" cellpadding="1" cellspacing="0">
                        <tr>
                            <td valign="top" align="left" width="40%">
                             
                                    <% if (System.Convert.ToInt16(Session["USER_ROLE"]) <= 3)
                                       { %>
                                <ul>
                                         <% if (System.Convert.ToInt16(Session["USER_ROLE"]) == 1 || System.Convert.ToInt16(Session["USER_ROLE"]) == 2 || System.Convert.ToInt16(Session["USER_ROLE"]) == 3)
                                                    { %>
                                        <li>
                                       <%-- <asp:LinkButton ID="LinkButton1" Text="Order History" runat="server" Class="CompanylinkSkin"
                                           >--%>
                                            <a href ="OrderHistory.aspx" class="CompanylinkSkin">
                                         Order History
                                            </a>
                                            <%--</asp:LinkButton>--%>
                                        </li>
                                             <%} %>
                                             
                                              <% if (System.Convert.ToInt16(Session["USER_ROLE"]) == 1 && System.Convert.ToString(Session["CUSTOMER_TYPE"]).Contains("Retailer") == false)
                                                    { %>
                                        <li>
                                        <%--<asp:LinkButton ID="LinkButton5" Text="Payment History" runat="server" Class="CompanylinkSkin"
                                            PostBackUrl="~/PaymentHistory.aspx"></asp:LinkButton>--%>
                                             <%-- <a href ="PaymentHistory.aspx" class="CompanylinkSkin">
                                         Payment History
                                            </a>--%>
                                        </li>
                                             <%} %>

                                              <% if (System.Convert.ToString(Session["CUSTOMER_TYPE"]).Contains("Retailer") == false)
                                                    { %>
                                              <li>
                                                  <%--<asp:LinkButton ID="LinkButton3" Text="Orders Pending Approval" runat="server" Class="CompanylinkSkin"
                                                  PostBackUrl="~/PendingOrder.aspx"></asp:LinkButton>--%>
                                                   <a href ="PendingOrder.aspx" class="CompanylinkSkin">
                                         Orders Pending Approval
                                            </a>
                                            </li>
                                             <%} %>
                                      <%} %>
                                  <%--  <li>
                                        <asp:LinkButton ID="lbtnChangeProfile" meta:resourcekey="lbtnChangeProfile" runat="server"
                                            Class="CompanylinkSkin" PostBackUrl="~/EditUserProfile.aspx"></asp:LinkButton></li><li>
                                                <asp:LinkButton ID="lbtnChangePassword" meta:resourcekey="lbtnChangePassword" runat="server"
                                                    Class="CompanylinkSkin" PostBackUrl="~/changePassword.aspx"></asp:LinkButton></li>--%>
                                    <% 
                                        HelperServices objHelperServices = new HelperServices();
                                        if (objHelperServices.GetOptionValues("ECOMMERCEENABLED").ToString() == "YES" && System.Convert.ToInt16(Session["USER_ROLE"]) < 4)
            {
                if (objHelperServices.GetOptionValues("ORDERPURCHASE").ToString().ToUpper() == "YES")
                {
                                    %>
                                    <%--<li>
                                        <asp:LinkButton ID="lbtnOrderHistory" Text="My Orders" runat="server" Class="CompanylinkSkin"
                                            PostBackUrl="~/Orders.aspx?ViewType=HISTORY"></asp:LinkButton></li>--%>
                                    <%      }
                                        if (objHelperServices.GetOptionValues("QUOTEPURCHASE").ToString().ToUpper() == "YES")
                { %>
                                    <%
                }
            } %><%--
                                    <% if (System.Convert.ToInt16(Session["USER_ROLE"]) < 3)
                                       { %>--%>
                                <%--    <li>
                                        <asp:LinkButton ID="LinkButton3" Text="Order History" runat="server" Class="CompanylinkSkin"
                                            PostBackUrl="~/OrderHistory.aspx"></asp:LinkButton></li>--%>
                                    <%--<li><asp:LinkButton ID ="LinkButton1" Text="Company Orders" runat ="server" class="CompanylinkSkin"  PostBackUrl ="~/Orders.aspx?ViewType=COMPANYORDS"></asp:LinkButton></li>--%>
                                   <%-- <li>
                                        <asp:LinkButton ID="LinkButton2" Text="Pending Orders" runat="server" Class="CompanylinkSkin"
                                            PostBackUrl="~/PendingOrder.aspx"></asp:LinkButton></li>--%>
                                  <%--  <%} %>--%>

                                    <% if (System.Convert.ToInt16(Session["USER_ROLE"]) == 1 && System.Convert.ToString(Session["CUSTOMER_TYPE"]).Contains("Retailer")==false) 
                                       { %>

                                       <td valign="top" align="left">
                                      &nbsp
                                    <li>
                                        <%--<asp:LinkButton ID="lbtnCompanyUsers" Text="Company Users" runat="server" Class="CompanylinkSkin"
                                            PostBackUrl="~/MultiUserSetup.aspx"></asp:LinkButton>--%>
                                            <asp:LinkButton ID="lbtnCompanyUsers" Text="Company Users" runat="server" Class="CompanylinkSkin"
                                            OnClick="MultiUserEdit_Click"></asp:LinkButton>
                                           

                                            </li>
                                        <%} %>

                                        <% if (System.Convert.ToString(Session["CUSTOMER_TYPE"]).Contains("Retailer") == true)
                                           {                                               
                                            
                                            %>

                                             <li>
                                       <%-- <asp:LinkButton ID="lbtnChangeProfile" meta:resourcekey="lbtnChangeProfile" runat="server"
                                            Class="CompanylinkSkin" PostBackUrl="~/RetailerEditUserProfile.aspx"></asp:LinkButton>--%>
                                             <a href ="RetailerEditUserProfile.aspx" class="CompanylinkSkin">
                                       View Profile
                                            </a>

                                            </li>
                                          



                                          <%}
                                           else
                                           { %>
                                    
                                              <li>
                                       <%-- <asp:LinkButton ID="LinkButton2" meta:resourcekey="lbtnChangeProfile" runat="server"
                                            Class="CompanylinkSkin" PostBackUrl="~/EditUserProfile.aspx"></asp:LinkButton>--%>
                                            <a href ="EditUserProfile.aspx" class="CompanylinkSkin">
                                        View Profile
                                            </a>
                                            </li>
                                          
                                          <%} %>
                                          <li>
<%--                                                <asp:LinkButton ID="lbtnChangePassword" meta:resourcekey="lbtnChangePassword" runat="server"
                                                    Class="CompanylinkSkin" PostBackUrl="~/changePassword.aspx"></asp:LinkButton>--%>
                                                  <a href ="changePassword.aspx" class="CompanylinkSkin">
                                        Change Password
                                            </a>    
                                                    </li>
                                         <li>
                                                <%--<asp:LinkButton ID="LinkButton4" Text="Change User Name" runat="server"
                                                    Class="CompanylinkSkin" PostBackUrl="~/changeUserName.aspx"></asp:LinkButton>--%>
                                                     <a href ="changeUserName.aspx" class="CompanylinkSkin">
                                        Change User Name
                                            </a>    
                                                    </li>
                                          
                                         <%--  <li>
                                                <asp:LinkButton ID="LinkButton6" Text="New Order Template" runat="server"
                                                    Class="CompanylinkSkin" PostBackUrl="~/OrderTemplate.aspx"></asp:LinkButton></li>--%>
                                            <li>
                                                <%--<asp:LinkButton ID="LinkButton7" Text="Order Templates" runat="server"
                                                    Class="CompanylinkSkin" PostBackUrl="~/OrderTemplateList.aspx"></asp:LinkButton>--%>
                                                    <a href ="OrderTemplateList.aspx" class="CompanylinkSkin">
                                        Order Templates
                                            </a>     
                                                    </li>
                                          </td>
                                    <%--<%} %>--%>
                                </ul>
                            </td>
                        </tr>
                    </table>
                </td>
                <td width="10" background="Images/tbl_right.gif">
                    <img alt="" height="350" src="Images/tbl_right.gif" width="10" />
                </td>
            </tr>
            <tr>
                <td width="10" height="10">
                    <img alt="" height="10" src="Images/tbl_bottomLeft.gif" width="10" />
                </td>
                <td background="Images/tbl_bottom.gif" height="10">
                    <img alt="" height="10" src="Images/tbl_bottom.gif" width="10" />
                </td>
                <td width="10" height="10">
                    <img alt="" height="10" src="Images/tbl_bottomRight.gif" width="10" />
                </td>
            </tr>
        </tbody>
    </table>
    <%  }
        else
        {
            Response.Redirect("Login.aspx");
        }
    %>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="rightnav" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="footer" runat="Server">
</asp:Content>
