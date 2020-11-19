<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="UI_InvoiceOrder" Codebehind="InvoiceOrder.ascx.cs" %>
<%@ Import Namespace="System.Data" %>
<%--<%@ Import Namespace="TradingBell.Common" %>
<%@ Import Namespace="TradingBell.WebServices" %>--%>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %>
<style type="text/css">
    .style19
    {
        width: 169px;
        position: absolute;
        left: 12px;
        top: 53px;
    }
    .style20
    {
        position: absolute;
        left: 12px;
        top: 53px;
    }
    .style21
    {
        width: 20%;
    }
</style>
<table id="tblBase" class="" align="center" width="100%" border="0px"
    cellpadding="0" cellspacing="0">
    <tr>
        <td width="100%" colspan="6" align="left" style="background-color: #F2F2F2;">
            <table cellpadding="0" cellspacing="0" width="100%" class="orderdettable">
               <%-- <tr>
                    <td colspan="6" style="font-size: small; color: #0099DA;">
                        <b>Your Order Contents</b>
                    </td>
                </tr>--%>
                <tr>
                    <td bgcolor="#F2F2F2" align="left" style="border-style: none solid solid none; border-width: thin; border-color: #E7E7E7;border: thin solid #E7E7E7;" width="13%">
                        <b>Order Code</b>
                    </td>
                    <td style="border-style: none solid solid none; border-width: thin; border-color: #E7E7E7;border: thin solid #E7E7E7;"
                        bgcolor="#F2F2F2" align="left" width="10%">
                        <b>Quantity</b>
                    </td>
                    <td style="border-style: none solid solid none; border-width: thin; border-color: #E7E7E7;border: thin solid #E7E7E7;"
                        colspan="2" bgcolor="#F2F2F2" align="left" width="25%">
                        <b>Description</b>
                    </td>
                    <%-- <td  
                              style="border-style: none solid solid none; border-width: thin; border-color: #E7E7E7" 
                              bgcolor="White" align="center"  width="10%">
                              <b>Availability</b></td>--%>
                    <td style="border-style: none solid solid none; border-width: thin; border-color: #E7E7E7;border: thin solid #E7E7E7;"
                        bgcolor="#F2F2F2" align="left" width="20%">
                        <b>Cost</b>
                    </td>
                    <td style="border-style: none none solid none; border-width: thin; border-color: #E7E7E7;border: thin solid #E7E7E7;"
                        bgcolor="#F2F2F2" align="left" width="27%">
                        <b>Extension Amount (Ex. GST)</b>
                    </td>
                </tr>
                <%-- <table class ="BaseTable1" cellpadding="3" cellspacing="1" width="558">
                        <tr class="TableRowHead">
                           <td class="tx_6" background="images/17.gif" align ="center" style="width: 210px">Item No.</td>
                           <td class="tx_6" background="images/17.gif" align ="center" width ="100px">
                               Quantity</td>
                               <td class="tx_6" background="images/17.gif" align ="center" width ="210px">
                               Description</td>
                            <td class="tx_6" background="images/17.gif" align ="center" width ="100px">Cost</td>
                            <td class="tx_6" background="images/17.gif" align ="center" width ="100px">Amount</td>
                        </tr>--%>
                <!--- Dynamic content for orderd item details  -->
                <%
                    HelperServices objHelperServices = new HelperServices();
                    
                    OrderServices objOrderServices = new OrderServices();
                    ProductServices objProductServices = new ProductServices();
                    DataSet dsOItem = new DataSet();

                  
                    
                    string CurSymbol = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString();
                    int OrderID = objHelperServices.CI(Request["OrderID"].ToString());
                    string catlogitem = "";
                    //decimal ShippingCost = oHelper.CDEC(Session["ShipCost"]);
                    decimal ProdSubTotal = 0.00M;

                    OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                    oOrderInfo = objOrderServices.GetOrder(OrderID);
                    
                    dsOItem = objOrderServices.GetOrderItems(OrderID);
                    if (dsOItem != null)
                    {
                        foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                        {
                            decimal ProductUnitPrice;
                            int pid;
                            decimal Amt = 0;

                            pid = objHelperServices.CI(rItem["PRODUCT_ID"].ToString());
                            catlogitem = rItem["CATALOG_ITEM_NO"].ToString();
                            //ProductUnitPrice = oHelper.CDEC(oProd.GetProductBasePrice(pid));
                            ProductUnitPrice = objHelperServices.CDEC(rItem["PRICE_EXT_APPLIED"].ToString());
                            ProductUnitPrice = objHelperServices.CDEC(ProductUnitPrice.ToString("N4"));
                           // Amt = objHelperServices.CDEC(objHelperServices.CI(rItem["QTY"].ToString()) * ProductUnitPrice);
                            Amt = Math.Round(objHelperServices.CI(rItem["QTY"].ToString()) * ProductUnitPrice, 2,MidpointRounding.AwayFromZero);
                                    
                %> 
                <tr style="z-index: 1">
                    <td  bgcolor="White" align="left" style="border-style: none solid solid #E7E7E7; border-width: thin;
                        border-color: #E7E7E7" width="13%" class="toplinkatest">
                        <% Response.Write(rItem["CATALOG_ITEM_NO"].ToString()); %></td>
                    <td style="border-style: none solid solid none; border-width: thin; border-color: #E7E7E7"
                        bgcolor="White" align="left" width="10%" >
                        <% Response.Write(rItem["QTY"].ToString()); %>
                    </td>
                    <td style="border-style: none solid solid none; border-width: thin; border-color: #E7E7E7"
                        colspan="2" bgcolor="White" align="left" width="25%">
                        <% Response.Write(rItem["DESCRIPTION"].ToString().Replace("<ars>g</ars>", "&rarr;"));%>
                    </td>
                    <td style="border-style: none solid solid none; border-width: thin; border-color: #E7E7E7"
                        bgcolor="White" align="left" width="20%">
                      <% Response.Write(CurSymbol + " " + objHelperServices.CheckPriceValueDecimal(ProductUnitPrice.ToString()) ); %> </td>
                    <td style="border-style: none solid solid none; border-width: thin; border-color: #E7E7E7"
                        bgcolor="White" align="left" width="27%">
                      <% Response.Write(CurSymbol + " " + Amt); %> </td>
                </tr>
                <%  
                                    
                            //ProdSubTotal =ProdSubTotal + oHelper.CDEC(rItem["PRICE_EXT_APPLIED"].ToString());                                         

                        }//End ForEach

                    }//End dsoItem null
                %>
                <tr>
                     <%if (objOrderServices.IsNativeCountry(OrderID) == 0)
                       {
                                   %>  
                    <td  colspan="4" rowspan="5" height="" class="style21"  bgcolor="white" align="right" valign="top">
                    <% 
                         }
                       else
                       { %>
                       <td  colspan="4" rowspan="3" height="" class="style21"  bgcolor="white" align="right" valign="top">
                               <%} %>      
                        <font color="red">
                            Availability & Cost is only Estimate. Actual Invoice may vary.
                         </font>
                    </td>
                    <td colspan="1" class="NumericField" style="height: 15px; border-style: none solid solid none;text-align:left;
                        border-width: thin; border-color: #E7E7E7;">
                        Sub Total
                    </td>
                    <td class="NumericField" style="height: 15px; border-style: none solid solid none;text-align:left;
                        border-width: thin; border-color: #E7E7E7;">
                        <% 
                            //ProdSubTotal = objOrderServices.GetCurrentProductTotalCost(OrderID);
                            //decimal TaxCst = 0.00M;
                            //decimal Total = 0.00M;
                            //Response.Write(CurSymbol + " " + ProdSubTotal);
                            //if (ProdSubTotal > 0)
                            //{
                            //    TaxCst = Math.Round((ProdSubTotal * 10 / 100), 2, MidpointRounding.AwayFromZero);
                            //}
                            //else
                            //{
                            //    TaxCst = 0;
                            //}
                            //Total = ProdSubTotal + TaxCst;                                    
                            Response.Write(CurSymbol + " " + oOrderInfo.ProdTotalPrice);  
                        %>
                    </td>
                </tr>
                <%
                    if (objOrderServices.IsNativeCountry(OrderID) == 0)
                    {
                 %>
                 <tr>
                    <td colspan="1" class="NumericField" style="height: 15px;text-align:left;">
                        <%   // if (oOrdBillInfo.BillCountry.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oOrdBillInfo.BillCountry.ToLower()).ToLower() != "au" || oOrdShippInfo.ShipCountry.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oOrdShippInfo.ShipCountry.ToLower()).ToLower() != "au") // is other then au
                    if (objOrderServices.IsNativeCountry(OrderID) == 0)
                        Response.Write("Shipping Charge");
                    else
                        Response.Write("Delivery / Handling Charge (Ex GST)");                                  
                                 %>
                    </td>
                    <td class="NumericField" style="height: 15px;text-align:left;">
                        <%
                    //Response.Write(CurSymbol + Session["TaxAmt"].ToString()); 
                    //decimal TaxCst = objOrderServices.GetTaxAmount(OrderID);

                    //Response.Write(CurSymbol + " " + TaxCst);
                    //if (oOrdBillInfo.BillCountry.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oOrdBillInfo.BillCountry.ToLower()).ToLower() != "au" || oOrdShippInfo.ShipCountry.ToLower().Trim() != "australia" || objUserServices.GetUserCountryCode(oOrdShippInfo.ShipCountry.ToLower()).ToLower() != "au") // is other then au
                    if (objOrderServices.IsNativeCountry(OrderID) == 0)
                        Response.Write("To Be Advised");
                    else
                        Response.Write(CurSymbol + " " + oOrderInfo.ShipCost);         
                            
                        %>
                    </td>
                </tr>
                <%} %>
                <tr>
                    <td colspan="1" class="NumericField" style="height: 15px;text-align:left;">
                        TaxAmount(GST)
                    </td>
                    <td class="NumericField" style="height: 15px;text-align:left;">
                        <%
                            //Response.Write(CurSymbol + Session["TaxAmt"].ToString()); 
                            //decimal TaxCst = objOrderServices.GetTaxAmount(OrderID);

                            //Response.Write(CurSymbol + " " + TaxCst);
                            Response.Write(CurSymbol + " " + oOrderInfo.TaxAmount);   
                                   
                        %>
                    </td>
                </tr>
                <%--     <tr>
                            <td colspan="3" class="NumericField" style="height: 25px">Shipping & Handling
                            </td>
                            <td class="NumericField" style="height: 25px">
                                <%
                                    //Response.Write(CurSymbol +  ShippingCost);
                                    Response.Write(CurSymbol + " " + objOrderServices.GetShippingCost(OrderID));
                                %>
                            </td>
                        </tr>--%>
                <tr>
                    <td colspan="1" class="NumericFieldship" style="border-style: none solid solid none;text-align:left;
                        border-width: thin; border-color: #E7E7E7;">
                          <%
                        if (objOrderServices.IsNativeCountry(OrderID) == 0)
                        {                                        
                            %>                                        
                                <strong>Est. Total </strong><br />
                            <%
                        }
                        else
                        {
                                %>
                            <strong>Est. Total Inc GST</strong><br />
                                <%
                        } %>
                        (Freight not included)
                    </td>
                    <td class="NumericFieldship" style="border-style: none solid solid none; border-width: thin;text-align:left;
                        border-color: #E7E7E7;">
                        <strong>
                            <%
                                //decimal ProductTotalCost = ProdSubTotal + oHelper.CDEC(Session["TaxAmt"].ToString()) + ShippingCost;
                                //Response.Write(CurSymbol + ProductTotalCost);
                                //Response.Write(CurSymbol + " " + objOrderServices.GetOrderTotalCost(OrderID));                                    
                               // Response.Write(CurSymbol + " " + Total);
                                Response.Write(CurSymbol + " " + oOrderInfo.TotalAmount);
                            %>
                        </strong>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
