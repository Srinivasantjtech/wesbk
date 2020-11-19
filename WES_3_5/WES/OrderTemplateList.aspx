<%@ Page Title="" Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" Inherits="OrderTemplateList" Codebehind="OrderTemplateList.aspx.cs" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" runat="Server">

            <script type="text/javascript">
                function DeleteConfirm(Template_id,Template_Name ,Contact) {
                    var agree = confirm("Are you sure want to Delete this order Template?\n Template Name : " + Template_Name + "\nCreated By : " + Contact);
                    
                    if (agree) {
                        var myUrl = "OrderTemplateList.aspx?TempId=" + Template_id + "&Act=D";
                        window.location.href = myUrl;
                        return true;
                    }

                }
                function AddConfirm(Template_id, Template_Name, Contact) {
                    var agree = confirm("Are you sure want to Add  this order Template?\n Template Name : " + Template_Name + "\nCreated By : " + Contact);

                    if (agree) {
                        var myUrl = "OrderTemplateList.aspx?TempId=" + Template_id + "&Act=A";
                        window.location.href = myUrl;
                        return true;
                    }

                }
            </script>
           
            <table align="left">
                <caption>
                    <br />
                    <tr>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td align="left" class="tx_1">
                            <font color="#333333" face="sans-serif" size="4">
                                <asp:Label ID="Label1" runat="server">&nbsp;Order Template List</asp:Label>
                            </font>
                        </td>
                    </tr>
                </caption>
            </table>
            <br />
          
            <br />
             <table width="780px" align="left">
                  <tr >
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                    <td align="left"  >
                     <br />
                     <p style="font-size:11.5px;" >
                       Save time when re-ordering regular or common items by creating an " Order Template". When you need to re-order these items again you 
                        simply select " Order Template" and the products in the template will be added to your shopping cart, no need to type them in again.
                        <br /><br />
                        Two methods for creating "Template Orders"
                        <br /><br />
                        1.Click on "Create New Order Template" button below to enter items in by product codes/part numbers.
                        <br />
                        2.Add items to cart as you would normally when browsing the site then on the cart page click the button that says "Save As Template"
                         <br />
                          <br />
                         </p>
                </td>
                </tr>
                </table>
                
            <table width="209px" align="left">                 
                <tr >
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                    <td align="left">
                        <asp:Button ID="Button1" Text="Create New Order Template" runat="server"  OnClick="btnNewOrderTemplate_Click" class="button normalsiz btngreen btnmain" style="width:200px;" />                        
                         <br />
                      
                </td>
                </tr>
            </table>
         
            <table align="center" width="780">
                        <tr>
                    <td>
                       &nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td align="center">
                        <% 
                            HelperServices ObjHelperServices = new HelperServices();
                            OrderServices objOrderServices = new OrderServices();
                            int userid = ObjHelperServices.CI(Session["USER_ID"]);
                            DataTable oDt = objOrderServices.GetOrderTemplate(userid);
                            if (oDt != null)
                            { %>
                        <table border="0" cellspacing="0" cellpadding="0" width="100%" style="border-width: 1px;
                            border-style: solid; border-color: #c6d3de">
                            <tr style="height: 20px">
                        <%--    <% if (System.Convert.ToInt16(Session["USER_ROLE"]) < 3)
                                 { %>--%>
                                <td style="background-color: #c6d3de; width: 12%;" align="left">
                                    Template Name
                                </td>
                                <td style="background-color: #c6d3de; width: 12%;" align="left">
                                    Notes
                                </td>
                                <td style="background-color: #c6d3de; width: 12%;" align="center">
                                    User
                                </td>
                                <td style="background-color: #c6d3de; width: 17%;" align="center">
                                    Created Date
                                </td>                             
                                <td width="0%" colspan="6" style="background-color: #c6d3de" align="left">
                                    Order Action
                                </td>
                              <%--  <%} %>
                                <% if (System.Convert.ToInt16(Session["USER_ROLE"]) == 3)
                                 { %>--%>
                           <%--     <td style="background-color: #c6d3de; width: 12%;" align="center">
                                    User
                                </td>
                                <td style="background-color: #c6d3de; width: 17%;" align="center">
                                    Order Date
                                </td>
                                <td style="background-color: #c6d3de; width: 15%;" align="center">
                                    Cust.Order No
                                </td>
                                <td width="0%" colspan="6" style="background-color: #c6d3de" align="left">
                                    Order Action
                                </td>--%>
                            <%--    <%} %>--%>
                            </tr>
                            <%  string bgcolor = "";
                                bool bodrow = true;
                                foreach (DataRow oDr in oDt.Rows)
                                {
                                    bgcolor = bodrow ? "white" : "#e7efef";
                                    bodrow = !bodrow;
                            %>
                            
                            <tr>
                           <%-- <% if (System.Convert.ToInt16(Session["USER_ROLE"]) < 3)
                             { %>--%>
                                <td align="left" style="background-color: <%=bgcolor%>; height: 20px; width: 12%;">
                                    <%=oDr["TEMPLATE_NAME"].ToString() %>
                                </td>
                                  <td align="left" style="background-color: <%=bgcolor%>; height: 20px; width: 12%;">
                                    <%=oDr["NOTES"].ToString()%>
                                </td>
                                  <td align="center" style="background-color: <%=bgcolor%>; height: 20px; width: 12%;">
                                    <%=oDr["CONTACT"].ToString()%>
                                </td>
                                <td align="center" style="background-color: <%=bgcolor%>; width: 17%;">
                                    <%=string.Format("{0:dd/MM/yyyy hh:mm:ss}", oDr["CREATED_DATE"])%>
                                </td>
                               
                               
                                <td style="background-color: <%=bgcolor%>" align="left">
                                    <% 
                                        int template_id = ObjHelperServices.CI(oDr["TEMPLATE_ID"].ToString());
                                        DataTable tbErrorItem = objOrderServices.GetOrder_Clarification_Items(template_id, "TEMP_ITEM_ERROR");
                                        DataTable tbErrorChk = objOrderServices.GetOrder_Clarification_Items(template_id, "TEMP_ITEM_CHK");

                                        if ((tbErrorItem != null && tbErrorItem.Rows.Count > 0) || (tbErrorChk != null && tbErrorChk.Rows.Count > 0))
                                        {%>
                                            <font style="color:red;">Clarifications Required</font> 
                                           <%
}
                                        else
                                        {%>
                                          <img src="images/Approve_Order.jpg" height="18px" width="17px" alt="" align="middle" />                                
                                       <a href="javascript:AddConfirm('<%=oDr["TEMPLATE_ID"]%>','<%=oDr["TEMPLATE_NAME"]%>','<%=oDr["CONTACT"]%>')"
                                        style="text-decoration: none; border-bottom: 1px solid none">&nbsp <font color="#3399FF">
                                           Add items to Cart</font></a>
                                        <%
                                            }
                                    
                                     %>                                  
                                </td>
                              
                                <td style="background-color: <%=bgcolor%>" align="left">
                                    <img src="images/View_edit_order.jpg" height="17px" width="16px" alt="" align="middle" /><a
                                        href="ordertemplate.aspx?Tempid=<% = oDr["TEMPLATE_ID"] %>&bulkorder=1"
                                        style="text-decoration: none; border-bottom: 1px solid none"> <font color="#3399FF">
                                            &nbsp Edit Template </font></a>
                                </td>
                                <td align="left" style="background-color: <%=bgcolor%>">
                                    <img src="images/Delete_order.jpg" height="17px" width="16px" alt="" align="middle" />
                                </td>
                                <td  align="left" style="background-color: <%=bgcolor%>">                                    
                                    <a href="javascript:DeleteConfirm('<%=oDr["TEMPLATE_ID"]%>','<%=oDr["TEMPLATE_NAME"]%>','<%=oDr["CONTACT"]%>')"
                                        style="text-decoration: none; border-bottom: 1px solid none">&nbsp <font color="#3399FF">
                                            Delete Template</font></a>
                                </td>
                                <%-- <%} %>
                                  <% if (System.Convert.ToInt16(Session["USER_ROLE"]) == 3)
                                 { %>--%>
                                 <%-- <td align="center" style="background-color: <%=bgcolor%>; height: 20px; width: 12%;">
                                    <%=oDr["CONTACT"].ToString()%>
                                </td>
                                <td align="center" style="background-color: <%=bgcolor%>; width: 17%;">
                                    <%=string.Format("{0:dd/MM/yyyy hh:mm:ss}", oDr["CREATED_DATE"])%>
                                </td>
                                <td align="center" style="background-color: <%=bgcolor%>; width: 15%;">
                                    <%=oDr["Cust.Order No"]%>
                                </td>
                               
                  
                              
                                <td style="background-color: <%=bgcolor%>" align="left">
                                    <img src="images/View_edit_order.jpg" height="17px" width="16px" alt="" align="middle" /><a
                                        href="OrderDetails.aspx?ORDER_ID=<% = oDr["Order"] %>&bulkorder=1&ViewOrder=View"
                                        style="text-decoration: none; border-bottom: 1px solid none"> <font color="#3399FF">
                                            &nbsp View / Edit Order </font></a>
                                </td>
                                <td align="left" style="background-color: <%=bgcolor%>">
                                    <img src="images/Delete_order.jpg" height="17px" width="16px" alt="" align="middle" />
                                </td>
                                <td width="16%" align="left" style="background-color: <%=bgcolor%>">
                                    
                                    <a href="javascript:DeleteConfirm('<%=oDr["ORDER"]%>','<%=oDr["CONTACT"]%>','<%=oDr["CREATED_DATE"]%>','<%=oDr["Cust.Order No"]%>')"
                                        style="text-decoration: none; border-bottom: 1px solid none">&nbsp <font color="#3399FF">
                                            Delete Order</font></a>
                                </td>--%>

                                 
                                <%--  <%} %>--%>
                            </tr>
                            <%} %>
                        </table>
                        <%} %>
                    </td>
                </tr>
            </table>
            <%--<asp:HiddenField ID="HiddenField2" value = "" runat="server" ClientIDMode="Static" />--%>       
</asp:Content>
