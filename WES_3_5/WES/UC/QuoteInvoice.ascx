<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_QuoteInvoice" Codebehind="QuoteInvoice.ascx.cs" %>
<%@ Import Namespace ="System.Data" %>
<%--<%@ Import Namespace ="TradingBell.Common" %>
<%@ Import Namespace ="TradingBell.WebServices" %>--%>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %>
<table id ="tblBase"  class="BaseTblBorder" align ="left" width="100%" border ="0px" cellpadding="3" cellspacing="0">
                 
       <tr>
            <td class="TableRowHead" colspan ="7" align="left"><asp:Label id="lblQuoteID" runat="server" Text="" > ></asp:Label></td>
       </tr>
			   
        <tr>
            <td colspan ="7">
                <table class ="BaseTable1" cellpadding="3" cellspacing="1" width="558">
                        <tr class="TableRowHead">
                           <td class="TableRowHead" align ="center" style="width: 210px">Item No.</td>
                           <td class="TableRowHead" align ="center" width ="100px">Quantity</td>
                            <td class="TableRowHead" align ="center" width ="100px">Cost</td>
                            <td class="TableRowHead" align ="center" width ="100px">Amount</td>
                        </tr>
                        <!--- Dynamic content for orderd item details  -->
                        <%
                            HelperDB objHelperDB = new HelperDB();
                            HelperServices objHelperServices = new HelperServices();
                            ErrorHandler objErrorHandler = new ErrorHandler();
                            QuoteServices objQuoteServices = new QuoteServices();
                            ProductServices objProductServices = new ProductServices();
                            //ProductFamily oProdFam = new ProductFamily();
                            DataSet dsOItem = new DataSet();


                            string CurSymbol = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString();

                            int QuoteID = objHelperServices.CI(Request["Qteid"].ToString());
                            
                            //decimal ShippingCost = oHelper.CDEC(Session["ShipCost"]);
                            decimal ProdSubTotal = 0.00M;
                            dsOItem = objQuoteServices.GetQuoteItems(QuoteID);
                            if (dsOItem != null)
                            {
                                foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                                {
                                    decimal ProductUnitPrice;
                                    int pid;
                                    decimal Amt = 0;
                                    pid = objHelperServices.CI(rItem["PRODUCT_ID"].ToString());
                                    int FId = objProductServices.GetFamilyID(pid); 
                                    //ProductUnitPrice = oHelper.CDEC(oProd.GetProductBasePrice(pid));
                                    ProductUnitPrice = objHelperServices.CDEC(rItem["PRICE_APPLIED"].ToString());
                                    ProductUnitPrice = objHelperServices.CDEC(ProductUnitPrice.ToString("N2"));
                                    Amt = objHelperServices.CDEC(objHelperServices.CI(rItem["QTY"].ToString()) * ProductUnitPrice); 
                                    
                        %>
                          <tr class="TableRow">
                                <td class="TableRow" style="width: 210px" align="left"><%Response.Write("<a href =productdetails.aspx?&Pid="+ pid + "&fid=" + FId.ToString() + ">" + rItem["CATALOG_ITEM_NO"].ToString() + "</a>"); %></td>
                                <td class="NumericField"><% Response.Write(rItem["QTY"].ToString()); %></td>
                                <td class="NumericField"><% Response.Write(CurSymbol + " " + ProductUnitPrice); %></td>
                                <td class="NumericField"><% Response.Write(CurSymbol + " " + Amt); %></td>
                            </tr>   
                       <%   
                                    
                            //ProdSubTotal =ProdSubTotal + oHelper.CDEC(rItem["PRICE_APPLIED"].ToString());                                         
                            
                                    }//End ForEach
                                    
                            }//End dsoItem null
                       %>
                                               
                        <tr>
                            <td colspan="3" class="NumericField" style="height: 25px">Sub Total
                            </td>
                            <td class="NumericField" style="height: 25px">
                                <% 
                                    ProdSubTotal = objQuoteServices.GetQuoteTotalCost(QuoteID);
                                    Response.Write(CurSymbol + " " + ProdSubTotal); %>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" class="NumericField"><strong>Total</strong>
                            </td>
                            <td class="NumericField">
                                <strong>
                                    <%
                                        //decimal ProductTotalCost = ProdSubTotal + oHelper.CDEC(Session["TaxAmt"].ToString()) + ShippingCost;
                                        //Response.Write(CurSymbol + ProductTotalCost);
                                        Response.Write(CurSymbol + " " + objQuoteServices.GetCurrentProductTotalCost(QuoteID));                                    %>
                                </strong>
                            </td>
                        </tr>
                 </table> 
            </td>
       </tr>
</table> 