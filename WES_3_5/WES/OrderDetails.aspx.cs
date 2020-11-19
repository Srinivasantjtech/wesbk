using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Collections.Generic;
//using System.Windows.Forms;
using TradingBell.WebCat;
using GCheckout.Checkout;
using GCheckout.Util;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CommonServices;
public partial class OrderDetails : System.Web.UI.Page
{
    #region "Declarations"
    HelperServices objHelperServices = new HelperServices();
    HelperDB objHelperDB = new HelperDB();
    ErrorHandler objErrorHandler = new ErrorHandler();
    OrderServices objOrderServices = new OrderServices();
    // CustomPrice oCustomPrice = new CustomPrice();
    UserServices objUserServices = new UserServices();
    NotificationServices objNotificationServices = new NotificationServices();
    //ConnectionDB objConnectionDB = new ConnectionDB();
    QuoteServices objQuoteServices = new QuoteServices();
    QuoteServices.QuoteInfo oQuoteInfo = new QuoteServices.QuoteInfo();
    QuoteServices.QuoteItemInfo oQuoteItemInfo = new QuoteServices.QuoteItemInfo();
    OrderServices.OrderItemInfo CobItemInFo = new OrderServices.OrderItemInfo();
    ProductServices objProductServices = new ProductServices();
    OrderServices.OrderInfo oOrdInfo = new OrderServices.OrderInfo();
    int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
    ProductPromotionServices objProductPromotionServices = new ProductPromotionServices();
    BuyerGroupServices objBuyerGroupServices = new BuyerGroupServices();
    CountryServices objCountryServices = new CountryServices();
    DataSet QDs = new DataSet();

    AjaxControlToolkit.ModalPopupExtender modalPop = new AjaxControlToolkit.ModalPopupExtender();
    AjaxControlToolkit.ModalPopupExtender msgPop = new AjaxControlToolkit.ModalPopupExtender();
    AjaxControlToolkit.ModalPopupExtender msgAlert = new AjaxControlToolkit.ModalPopupExtender();
    AjaxControlToolkit.ModalPopupExtender msgOTAlert = new AjaxControlToolkit.ModalPopupExtender();
    public int NewProduct_id=0;
    public int NewQty=0;
    public string FIXED_TAX = System.Configuration.ConfigurationManager.AppSettings["FIXED_TAX"].ToString();
    public string FIXED_TAX_PERCENTAGE = System.Configuration.ConfigurationManager.AppSettings["FIXED_TAX_PERCENTAGE"].ToString();
    string strFile = HttpContext.Current.Server.MapPath("ProdImages");
    int QtyAvail;
    int MinQtyAvail;
    //int ProductID = 0;
    int OrderID = 0;
    int AvlQty = 0;
    int CatalogID = 0;
    bool _IsShippingFree = false;
    string[] ProdID;
    string[] ord_itemID;
    string Restricted_prod;
    DataTable  dsorder_items = null;
    DataTable Order_update = new DataTable();
    // int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
    #endregion "Declarations"
    protected void Page_PreInit()
    {
        if (Request.QueryString["popup"]!=null)
        {
            Page.MasterPageFile = "~/AddtoCardPopup.Master";            
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
       
        try
        {
            
            


            if (Request.QueryString["popup"] != null)
            {                
                pnlEnter.Visible = false;
            }

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        if (Session["PageUrl"] != null && Session["PageUrl"].ToString() != string.Empty)
        {
            
            if (Session["PageUrl"].ToString().Contains("ConfirmMessage.aspx?Result=NOPRICEAMT"))
            {
                Session["PageUrl"] = "ConfirmMessage.aspx?Result=QTEEMPTY";
            }
            else if ((Request.QueryString["ORDER_ID"] != null && !string.IsNullOrEmpty(Request.QueryString["ORDER_ID"])) || (Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0))
            {
              
                //Session["PageUrl"] = "OrderDetails.aspx?ORDER_ID=" + Request.QueryString["ORDER_ID"] + "&bulkorder=1&ViewOrder=View";
            }
            else
            {
                // Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
            }
          
            }
           
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();

        string QuotePurchase = objHelperServices.GetOptionValues("QUOTEPURCHASE").ToString();
        string OrderPurchase = objHelperServices.GetOptionValues("ORDERPURCHASE").ToString();
        if (Session["CATALOGID"] != null && Session["CATALOGID"].ToString() != "")
        {
            CatalogID = objHelperServices.CI(Session["CATALOGID"]);
        }
        else
        {
            Session["CATALOGID"] = objHelperServices.CI(objHelperServices.GetOptionValues("DEFAULT CATALOG").ToString());
            CatalogID = objHelperServices.CI(Session["CATALOGID"]);
        }

        if (Session["ShowPop"] != null)     // DIRECT CHECK OUT OPTION PURPOSE WHEN VIEW CART ITEMS AVAILABLE
        {
            if (Session["ShowPop"].ToString().Trim() == "Yes")
            {
                ShowPopUpMessage();
                Session["ShowPop"] = "";
            }
        }
        if (Request.QueryString["CombainProd_id"] != null && (Request.QueryString["ORDER_ID"] != null))
        {

            string LeaveDuplicateProds = GetLeaveDuplicateProducts();
            int order_id = Convert.ToInt32(Request.QueryString["ORDER_ID"].ToString());
            int PrdId = objHelperServices.CI(Request.QueryString["CombainProd_id"].ToString());
            DataSet dsDuplicateItem = objOrderServices.GetOrderItemsWithDuplicate(order_id, LeaveDuplicateProds);
            if (dsDuplicateItem != null && dsDuplicateItem.Tables.Count > 0 && dsDuplicateItem.Tables[0].Rows.Count > 0)
            {
                DataRow[] Dr = dsDuplicateItem.Tables[0].Select("PRODUCT_ID='" + PrdId + "'");

                if (Dr.Length > 0)
                {
                    DataTable temptbl = Dr.CopyToDataTable();


                    decimal TotQty = 0;

                    foreach (DataRow row in temptbl.Rows)
                    {
                        TotQty = TotQty + objHelperServices.CI(row["QTY"].ToString());
                        int nQty = objHelperServices.CI(row["QTY"].ToString());
                        string order_item_id = row["ORDER_ITEM_ID"].ToString();
                        objOrderServices.RemoveItem(PrdId.ToString(), order_id, objHelperServices.CI(Session["USER_ID"]), order_item_id.ToString());
                    }

                    AddOrderItem(order_id, objHelperServices.CI(Session["USER_ID"]), (int)TotQty, PrdId);
                    oOrdInfo.OrderID = OrderID;
                    objOrderServices.UpdateOrderPrice(oOrdInfo, true);
                }

            }



        }
        if (Request.QueryString["LeaveProd_id"] != null && (Request.QueryString["ORDER_ID"] != null))
        {
            int order_id = Convert.ToInt32(Request.QueryString["ORDER_ID"].ToString());
            int PrdId = objHelperServices.CI(Request.QueryString["LeaveProd_id"].ToString());
            string LeaveDuplicateProds = "";
            if (Session["LeaveDuplicateProds"] != null && Session["LeaveDuplicateProds"].ToString() != "")
            {
                LeaveDuplicateProds = Session["LeaveDuplicateProds"].ToString();

                if (LeaveDuplicateProds.Contains("-" + PrdId + "-") == false && PrdId > 0)
                    LeaveDuplicateProds = LeaveDuplicateProds + ",-" + PrdId + "-";
            }
            else
                LeaveDuplicateProds = "-" + PrdId + "-";

            Session["LeaveDuplicateProds"] = LeaveDuplicateProds;
        }

        if (objHelperServices.GetOptionValues("ECOMMERCEENABLED").ToString().ToUpper() == "YES")
        {
            if (!IsPostBack && (Session["ShowPop"] == null || Session["ShowPop"].ToString().TrimEnd(',') != "" || Session["ShowPop"].ToString().Trim() == ""))
            {

                if (Request.QueryString["ORDER_ID"] != null)
                {
                    if (Request.QueryString["ORDER_ID"].ToString().Trim() != "")
                    {
                        Session["ORDER_ID"] = Request.QueryString["ORDER_ID"];
                        OrderServices objOrderServices = new OrderServices();
                        DataTable oDt1 = objOrderServices.GetPendingOrderProducts();
                        Session["Multipleitems"] = "0";
                        Session["Multipleitems_id"] = "0";
                        if (oDt1 != null && oDt1.Rows.Count > 0)
                        {
                            foreach (DataRow oDr1 in oDt1.Rows)
                            {
                                Session["Multipleitems"] = Session["Multipleitems"] + ", " + oDr1["PRODUCT_ID"];
                                Session["Multipleitems_id"] = Session["Multipleitems_id"] + ", " + oDr1["ORDER_ITEM_ID"];
                            }
                        }
                        else
                        {
                            if (Request.QueryString["ViewOrder"] == null)
                                 Session["ORDER_ID"] = "0";
                            Session["Multipleitems"] = null;
                            Session["Multipleitems_id"] = null;
                        }
                    }
                    else
                    {
                        if (Request.Url.ToString().Contains("OrderDetails.aspx"))
                        {
                            int OrdStatus = (int)OrderServices.OrderStatus.OPEN;
                            int Userid = objHelperServices.CI(Session["USER_ID"]);
                            OrderID = objOrderServices.GetOrderID(Userid, OrdStatus);
                            Session["ORDER_ID"] = OrderID;
                        }
                    }
                }

                // if (ProductID >= 0 && Request["Pid"] != null)
                // {
                //    ProductID = objHelperServices.CI(Request["Pid"].ToString());

                //}
                if (Session["USER_ID"] == null || Session["USER_NAME"] == null || Session["USER_NAME"].ToString() == "" || Convert.ToInt32(Session["USER_ID"].ToString())<0)
                {
                    Session["USER"] = "";
                    Session["COUNT"] = "0";
                    if (Request.QueryString["popup"] == null)
                        Response.Redirect("Login.aspx");
                    else
                    {
                        divAddtocart.Visible = false;
                        divTimeout .Visible =true;
                    }
                }
                else
                {

                    if (objUserServices.IsUserActive(Session["USER_ID"].ToString()))
                    {
                        
                        if (Session["Multipleitems"] != null)
                        {
                            ProdID = Session["Multipleitems"].ToString().Split(',');
                            ord_itemID = Session["Multipleitems_id"].ToString().Split(',');
                            Restricted_prod=objHelperServices.GetOptionValues("ENABLED RESTRICTED PRODUCT").ToString().ToUpper();

                            dsorder_items= objOrderServices.GetOrder_item_Status_all(Session["Multipleitems_id"].ToString());

                            CreateOrder_updateTable();
                            for (int i = 0; i < ProdID.Length; i++)
                            {
                                //ProductID = Convert.ToInt32(ProdID[i]);
                                int product_id = Convert.ToInt32(ProdID[i]);
                                double order_item_id = Convert.ToInt32(ord_itemID[i]);

                                //if (objProductServices.GetProductAvailability(Convert.ToInt32(product_id)) == 1)
                                {
                                    AddMultipleItems(product_id, order_item_id, dsorder_items);
                                }
                            }
                            update_order_table();
                            oOrdInfo.OrderID = OrderID;
                            objOrderServices.UpdateOrderPrice(oOrdInfo, true);

                           // AddMultipleItemsNew(Session["Multipleitems"].ToString(), Session["Multipleitems_id"].ToString());
                            Session["Multipleitems"] = null;
                            //if (Session["pageurl"] != null)
                            //{
                            //    if (!Session["PageUrl"].ToString().Contains("OrderDetails.aspx"))
                            //        if (Session["PageUrl"].ToString().Contains("ConfirmMessage.aspx?Result=QTEEMPTY") || Session["PageUrl"].ToString().Contains("powersearch.aspx"))
                            //        {
                            //            Response.Redirect("OrderDetails.aspx?&bulkorder=1&pid=0");
                            //        }
                            //        else
                            //        {
                            //            Response.Redirect(Session["PageUrl"].ToString());
                            //        }
                            //}
                        }
                        QtyAvail = objOrderServices.GetProductAvilableQty(objHelperServices.CI(Request["Pid"]));
                        MinQtyAvail = objOrderServices.GetProductMinimumOrderQty(objHelperServices.CI(Request["Pid"]));
                        int Productstatus = objProductServices.GetProductAvailability(objHelperServices.CI(Request["Pid"]));
                        int Ord = 0;

                        if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
                        {
                            Ord = Convert.ToInt32(Session["ORDER_ID"]);
                        }
                        else
                        {
                            Ord = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);
                        }

                        if (Ord == 0)
                        {
                            oOrdInfo.UserID = objHelperServices.CI(Session["USER_ID"]);
                            objOrderServices.InitilizeOrder(oOrdInfo);
                            //Ord = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);
                            if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
                            {
                                Ord = Convert.ToInt32(Session["ORDER_ID"]);
                            }
                            else
                            {
                                Ord = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);
                            }
                            lblOrdNo.Text = Ord.ToString();
                        }
                        if (Ord != 0)
                            AvlQty = objOrderServices.GetOrderItemQty(objHelperServices.CI(Request["Pid"]), Ord, 0);
                        lblOrdNo.Text = Ord.ToString();

                        
                        if (Request["Pid"] != null && Request["Qty"] != null && Productstatus != 0)
                        {
                            
                            int p = objHelperServices.CI(Request["Pid"].ToString());
                            if (QtyAvail == 0 && p > 0)
                            {
                                string str;
                                str = "<script  type=\"text/javascript\">";
                                str = str + "alert('Selected Product is sold out')";
                                str = str + "</script>";
                                this.RegisterClientScriptBlock("validate", str);
                            }
                            int txtQty = objHelperServices.CI(Request["Qty"]);
                            //if ((QtyAvail + AvlQty - txtQty) >= 0)
                            
                            if (MinQtyAvail > 0)
                                if (objHelperServices.GetOptionValues("ENABLED RESTRICTED PRODUCT").ToString().ToUpper() == "YES")
                                {
                                   
                                    if (objProductServices.GetRestrictedProduct(p).ToString().ToUpper() == "NO")
                                    {
                                        
                                        AddToOrderTable();
                                    }
                                }
                                else
                                {
                                    
                                    AddToOrderTable();
                                   
                                }

                            if (Session["pageurl"] != null)
                            {
                                if (!Session["PageUrl"].ToString().Contains("OrderDetails.aspx"))
                                    if (Session["PageUrl"].ToString().Contains("ConfirmMessage.aspx?Result=QTEEMPTY"))
                                    {
                                        Response.Redirect("OrderDetails.aspx?&bulkorder=1&pid=0");
                                    }
                            }

                            if (Request.QueryString["popup"] != null && Request["fid"] != null && Request["fid"].ToString() != "")
                            {
                                AddPopUpItem(txtQty, p, Convert.ToInt32(Request["fid"].ToString()));
                                return;
                            }
 
                        }
                    }
                    else
                    {
                        if (Request.QueryString["popup"] == null)
                            Response.Redirect("Login.aspx");
                        else
                        {
                            divAddtocart.Visible = false;
                            divTimeout.Visible = true;
                        }

                    }
                }
                if (Request["SelAll"] == "1")
                {
                    //chkSelectAll.Checked = true;
                }
                else if (Request["SellAll"] == "0")
                {
                    // chkSelectAll.Checked = false;
                }
                if (Request["SelPid"] != null)
                {
                    if (Request["SelPid"] != "" && Request["SelPid"] != "AllProd")
                    {
                        int OrderId = 0;
                        if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
                        {
                            OrderId = Convert.ToInt32(Session["ORDER_ID"]);
                        }
                        else
                        {
                            OrderId = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);
                        }

                        char[] sep = { ',' };
                        string s1 = Request["SelPid"];
                        string s2 = Request["ProdPrice"];
                        string s3 = Request["ORDER_ITEM_ID"].ToString();
                        decimal SelProdPrice = objHelperServices.CDEC(Request["SelProdPrice"].ToString());
                        string[] cnt = new string[30];
                        string[] cnt1 = new string[30];
                        string[] cnt2 = new string[30];
                        cnt = s1.Split(sep);
                        cnt1 = s2.Split(sep);
                        cnt2 = s3.Split(sep);

                        int len = cnt.Length;
                        int flagship = 1;
                        int flagupdate = 0;



                        //for (int j = 1; j <= cnt1.Length; j++)
                        //{
                        //    SelProdPrice = SelProdPrice + objHelperServices.CDEC(cnt1[j - 1].ToString());
                        //}
                        /* if (objOrderServices.GetOrderItemCount(OrderId) > 0)
                         {
                             DataSet oDSOrderItems = objOrderServices.GetOrderItems(OrderId);
                             decimal TempShipCost = 0;
                             for (int i = 1; i <= len; i++)
                             {
                                 int PrdId = objHelperServices.CI(cnt[i - 1].ToString());
                                 double  order_item_id = objHelperServices.CD (cnt2[i - 1].ToString());
                                 int pQty = objOrderServices.GetOrderItemQty(PrdId, OrderId,order_item_id  );
                                 int nQty = objHelperServices.CI(Request.Form["txtQty"] + pQty);
                                 int AvailQty = objOrderServices.GetProductAvilableQty(PrdId);
                                 int n = AvailQty + nQty;

                                 DataRow[] oDR = oDSOrderItems.Tables[0].Select("PRODUCT_ID=" + PrdId + " And ORDER_ITEM_ID=" + order_item_id );
                                 if (oDR != null && oDR.Length > 0)
                                 {

                                     if (objOrderServices.RemoveItem(PrdId.ToString(), OrderId, objHelperServices.CI(Session["USER_ID"]),order_item_id.ToString()   ) != -1)
                                     {
                                         if (n >= 0)
                                         {
                                             objOrderServices.UpdateQuantity(PrdId, n);
                                         }
                                         if (objHelperServices.GetOptionValues("ENABLE ITEM SHIPPING").ToString().ToUpper() == "YES")
                                         {
                                             TempShipCost = TempShipCost + objHelperServices.CDEC(TempShipCost + CalculateShippingCost(OrderId, PrdId, objHelperServices.CDEC(oDR[0]["PRICE_EXT_APPLIED"].ToString()), objHelperServices.CI(oDR[0]["QTY"].ToString())));
                                         }
                                         else
                                         {
                                             if (flagship == 1)
                                             {
                                                 TempShipCost = objHelperServices.CDEC(TempShipCost + CalculateShippingCost(OrderId, PrdId, objHelperServices.CDEC(oDR[0]["PRICE_EXT_APPLIED"].ToString()), objHelperServices.CI(oDR[0]["QTY"].ToString())));
                                                 flagship = 0;
                                             }
                                         }
                                         flagupdate = 1;
                                     }
                                 }
                                 else
                                 {
                                     SelProdPrice = SelProdPrice - objHelperServices.CDEC(cnt1[i - 1].ToString());
                                 }
                             }
                             if (flagupdate == 1)
                             {
                                 decimal Tax = CalculateTaxAmount(SelProdPrice);
                                 if (objOrderServices.GetShippingCost(OrderId) == 0)
                                 {
                                     TempShipCost = -1 * TempShipCost;
                                 }
                                 if (TempShipCost < 0)
                                     TempShipCost = 0;
                                 if (SelProdPrice < 0)
                                     SelProdPrice = 0;
                                 objOrderServices.UpdateRemovedItemsPrice(SelProdPrice, OrderId, objHelperServices.CDEC(objHelperServices.FixDecPlace(Tax)), TempShipCost);
                             }

                         }
                         else
                         {
                             oOrdInfo.OrderID = OrderId;
                             oOrdInfo.ProdTotalPrice = 0.00M;
                             oOrdInfo.TotalAmount = 0.00M;
                             oOrdInfo.TaxAmount = 0.00M;
                             oOrdInfo.ShipCost = 0.00M;
                             objOrderServices.UpdateOrderPrice(oOrdInfo, true);
                             //objOrderServices.UpdateShippingCost(OrderId, 0.00M);
                         }*/
                        if (objOrderServices.GetOrderItemCount(OrderId) > 0)
                        {
                            DataSet oDSOrderItems = objOrderServices.GetOrderItems(OrderId);
                            decimal TempShipCost = 0;
                            for (int i = 1; i <= len; i++)
                            {
                                int PrdId = objHelperServices.CI(cnt[i - 1].ToString());
                                double order_item_id = objHelperServices.CD(cnt2[i - 1].ToString());

                                int pQty = objOrderServices.GetOrderItemQty(PrdId, OrderId, order_item_id);


                                DataRow[] oDR = oDSOrderItems.Tables[0].Select("PRODUCT_ID=" + PrdId + " And ORDER_ITEM_ID=" + order_item_id);
                                if (oDR != null && oDR.Length > 0)
                                {
                                    objOrderServices.RemoveItem(PrdId.ToString(), OrderId, objHelperServices.CI(Session["USER_ID"]), order_item_id.ToString());
                                }
                            }
                        }
                        oOrdInfo.OrderID = OrderId;
                        objOrderServices.UpdateOrderPrice(oOrdInfo, true);

                    }
                    else if (Request["SelPid"] == "AllProd")
                    {
                        int OrderId = 0;
                        if (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View"))
                        {
                            OrderID = Convert.ToInt32(Session["ORDER_ID"]);
                        }
                        else
                        {
                            OrderId = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);
                        }

                        int len = objOrderServices.GetOrderItemCount(OrderId);
                        DataSet ds = new DataSet();
                        ds = objOrderServices.GetOrderItems(OrderId);

                        if (len > 0)
                        {
                            /* int cnt = ds.Tables[0].Rows.Count;
                             foreach (DataRow row in ds.Tables[0].Rows)
                             {
                                 int PrdId = objHelperServices.CI(row["PRODUCT_ID"].ToString());
                                 int pQty = objHelperServices.CI(row["QTY"].ToString());
                                 int nQty = objHelperServices.CI(Request.Form["txtQty"] + pQty);
                                 int AvailQty = objOrderServices.GetProductAvilableQty(PrdId);
                                 string order_item_Cid = row["ORDER_ITEM_ID"].ToString()  ;
                                 int n = AvailQty + nQty;
                                 if (n >= 0)
                                     objOrderServices.UpdateQuantity(PrdId, n);
                             }

                             if (objOrderServices.RemoveItem(Request["SelPid"], OrderId, objHelperServices.CI(Session["USER_ID"]), "") != -1)
                             {
                                 oOrdInfo.OrderID = OrderId;
                                 oOrdInfo.ProdTotalPrice = 0.00M;
                                 oOrdInfo.TotalAmount = 0.00M;
                                 oOrdInfo.TaxAmount = 0.00M;
                                 oOrdInfo.ShipCost = 0.00M;
                                 objOrderServices.UpdateOrderPrice(oOrdInfo, true);
                                 //objOrderServices.UpdateShippingCost(OrderId, 0.00M);
                             }*/
                            objOrderServices.RemoveItem(Request["SelPid"], OrderId, objHelperServices.CI(Session["USER_ID"]), "");
                            oOrdInfo.OrderID = OrderId;
                            objOrderServices.UpdateOrderPrice(oOrdInfo, true);

                        }
                    }
                }
            }
        }
        else
        {
            Response.Redirect("ConfirmMessage.aspx?Result=NOECOMMERCE");
        }
        //lblmsg.Visible = false;
        if (QuotePurchase.ToUpper() == "YES")
        {
            //btnQuoteRequest.Visible = true;
            //btnQuoteRequestTop.Visible = true;
            //Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);        
        }
        else
        {
            //Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
            //btnQuoteRequest.Visible = false;
            // btnQuoteRequestTop.Visible = false;
        }
        if (OrderPurchase.ToUpper() == "YES")
        {
            //Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
            lblOrderProceed.Visible = true;
            //lblOrderProceed1.Visible = true;
        }
        else
        {
            //Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
            //lblOrderProceed1.Visible = false;
            lblOrderProceed.Visible = false;
            //lblmsg.Visible = true;
        }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }

    #region "Functions.."

    private void CreateOrder_updateTable()
    {
        Order_update.TableName = "ORDER_TBL";
        DataColumn dc = new DataColumn("ORDER_ITEM_ID", typeof(Int32));
        dc.DefaultValue = 0;
        Order_update.Columns.Add(dc);
        dc = new DataColumn("PRODUCT_ID", typeof(Int32));
        dc.DefaultValue = 0;
        Order_update.Columns.Add(dc);
        dc = new DataColumn("ORDER_ID", typeof(Int32));
        dc.DefaultValue = 0;
        Order_update.Columns.Add(dc);
        dc = new DataColumn("PRICE",typeof(decimal));
        dc.DefaultValue = 0;
        Order_update.Columns.Add(dc);
        dc = new DataColumn("USER_ID", typeof(Int32));
        dc.DefaultValue = 0;
        Order_update.Columns.Add(dc);
        dc = new DataColumn("QTY", typeof(Int32));
        dc.DefaultValue = 0;
        Order_update.Columns.Add(dc);
        dc = new DataColumn("SHIP_COST", typeof(Decimal));
        dc.DefaultValue = 0;
        Order_update.Columns.Add(dc);
        dc = new DataColumn("TAX_AMOUNT", typeof(Decimal));
        dc.DefaultValue = 0;
        Order_update.Columns.Add(dc);
    }

    public void update_order_table()
    {

        try
        {
            objOrderServices.Update_Order_item_xml(Order_update);
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
          
        
    }
    public void AddMultipleItems(int Productid, double order_item_id,DataTable Order_item_sts)
    {

        string isRestricted="";
        int promotion=0;
        decimal  ispro_disc_price = 0;
        string prod_status = "";
        if (Order_item_sts != null && Order_item_sts.Rows.Count > 0)
        {
            DataRow[] dr=   Order_item_sts.Select("ORDER_ITEM_ID=" + order_item_id);
            if (dr.Length > 0)
            {
                QtyAvail = Convert.ToInt32(dr[0]["QTY_AVAIL"]);
                MinQtyAvail =Convert.ToInt32(   dr[0]["MIN_ORD_QTY"]);
                AvlQty = Convert.ToInt32(dr[0]["Qty"]);
                isRestricted = dr[0]["RESTRICTED"].ToString();
                promotion = Convert.ToInt32(dr[0]["ispromotion"]);
                ispro_disc_price = Convert.ToDecimal(dr[0]["ispro_disc_price"]);
                prod_status = dr[0]["PRODUCT_STATUS"].ToString();


            }

        }
        if (prod_status != "AVAILABLE")
            return;

          //  QtyAvail = objOrderServices.GetProductAvilableQty(Productid);
         //   MinQtyAvail = objOrderServices.GetProductMinimumOrderQty(Productid);
       
        int Ord = 0;

        if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
        {
            Ord = Convert.ToInt32(Session["ORDER_ID"]);
        }
        else
        {
            Ord = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);
        }

        if (Ord == 0)
        {
            oOrdInfo.UserID = objHelperServices.CI(Session["USER_ID"]);
            objOrderServices.InitilizeOrder(oOrdInfo);

            if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
            {
                Ord = Convert.ToInt32(Session["ORDER_ID"]);
            }
            else
            {
                Ord = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);
            }

            lblOrdNo.Text = Ord.ToString();
        }

       // if (Ord != 0)
        //    AvlQty = objOrderServices.GetOrderItemQty(Productid, Ord, order_item_id);


        lblOrdNo.Text = Ord.ToString();
        if (Productid != null)
        {
            int p = Productid;
            if (QtyAvail == 0 && p > 0)
            {
                string str;
                str = "<script type=\"text/javascript\">";
                str = str + "alert('Selected Product is sold out')";
                str = str + "</script>";
                this.RegisterClientScriptBlock("validate", str);
            }
            int txtQty = MinQtyAvail;
            if ((QtyAvail + AvlQty - txtQty) >= 0)
                if (MinQtyAvail > 0)
                    //if (objHelperServices.GetOptionValues("ENABLED RESTRICTED PRODUCT").ToString().ToUpper() == "YES")
                    if (Restricted_prod == "YES")
                    
                    {
                        if (isRestricted == "NO")
                            AddMultipleItemsToOrderTable(Productid, order_item_id, AvlQty, promotion, ispro_disc_price);
                    }
                    else
                    {
                        AddMultipleItemsToOrderTable(Productid, order_item_id, AvlQty, promotion,ispro_disc_price);
                    }
        }

    }



    public void AddMultipleItemsToOrderTable(int Product_id, double order_item_id, int Order_qty, int promotion, decimal  ispro_disc_price)
    {
        try
        {
            int OrderID = 0;
            string OrderStatus = "";

            oOrdInfo.UserID = objHelperServices.CI(Session["USER_ID"]);

            //  OrderID = objOrderServices.GetOrderID(oOrdInfo.UserID, OpenOrdStatusID);

            if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
            {
                OrderID = Convert.ToInt32(Session["ORDER_ID"]);
            }
            else
            {
                OrderID = objOrderServices.GetOrderID(oOrdInfo.UserID, OpenOrdStatusID);
            }

            OrderStatus = objOrderServices.GetOrderStatus(OrderID);

            if (OrderID == 0 || OrderStatus == OrderServices.OrderStatus.PAYMENT.ToString() || OrderStatus == OrderServices.OrderStatus.SHIPPED.ToString() || OrderStatus == OrderServices.OrderStatus.COMPLETED.ToString() || OrderStatus == OrderServices.OrderStatus.CANCELED.ToString() || OrderStatus == OrderServices.OrderStatus.ORDERPLACED.ToString() || OrderStatus == OrderServices.OrderStatus.MANUALPROCESS.ToString())
            {
                objOrderServices.InitilizeOrder(oOrdInfo);

                if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ViewOrder"] != null && HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")))
                {
                    OrderID = Convert.ToInt32(Session["ORDER_ID"]);
                }
                else
                {
                    OrderID = objOrderServices.GetOrderID(oOrdInfo.UserID, OpenOrdStatusID);
                }



                AddOrderMultipleItems(OrderID, oOrdInfo.UserID, Product_id, order_item_id, Order_qty, promotion, ispro_disc_price);
            }
            else if (OrderStatus == OrderServices.OrderStatus.OPEN.ToString() || OrderStatus == "CAU_PENDING")
            {
                AddOrderMultipleItems(OrderID, oOrdInfo.UserID, Product_id, order_item_id, Order_qty, promotion, ispro_disc_price);
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }

    public void AddOrderMultipleItems(int OrID, int UsrID, int Product_id, double order_item_id, int Order_qty, int promotion, decimal ispro_disc_price)
    {
        try
        {

            OrderServices.OrderItemInfo oItemInFo = new OrderServices.OrderItemInfo();
            decimal untPrice = 0.00M;
            DataSet dsBgPrice = new DataSet();
            DataSet dsBgDisc = new DataSet();
            // OrderServices objOrderServices = new OrderServices();
            int chkExistsItem = 0;
            chkExistsItem = Order_qty;// objOrderServices.GetOrderItemQty(Product_id, OrID, order_item_id);
            int ProdQty;
            /*if (MinQtyAvail > 1)
            {
                ProdQty = MinQtyAvail;
            }
            else
            {
                ProdQty = 1;
            }*/
            if (chkExistsItem > 0)
            {
                ProdQty = chkExistsItem;
            }
            else
            {
                ProdQty = 1;
            }
            if (chkExistsItem == 0 || ProdQty != chkExistsItem || ProdQty > 0) // ProdQty > 0
            {
                bool attrcheck = false;
                //Chceck the promotion table.
                //if (objProductPromotionServices.CheckPromotion(Product_id))
                if (promotion>0)
                {

                    //decimal DiscPrice = objHelperServices.CDEC(objProductPromotionServices.GetProductPromotionDiscValue(Product_id));
                    decimal DiscPrice = ispro_disc_price;
                    //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", Session["USER_ID"]);
                    //objHelperServices.SQLString = sSQL;
                    //int pricecode = objHelperServices.CI(objHelperServices.GetValue("price_code"));

                    //string strquery = "";
                    //if (pricecode == 1)
                    //{
                    //    strquery = string.Format("exec GetWESIncProductPrice {0},{1},{2}", ProductID, ProdQty, Session["USER_ID"]);
                    //}
                    //else
                    //{
                    //    strquery = string.Format("exec GetWESProductPrice {0},{1},{2}", ProductID, ProdQty, Session["USER_ID"]);
                    //}

                    //DataSet DSprice = new DataSet();
                    //objHelperServices.SQLString = strquery;
                    //untPrice = Convert.ToDecimal(objHelperServices.GetValue("Numeric_Value"));

                    string user_id = Session["USER_ID"].ToString();

                    untPrice = objHelperDB.GetProductPrice_Exc(Product_id, ProdQty, user_id);


                    /* string strquery = "select product_id,numeric_value,attribute_id from tb_prod_specs where product_id=" + ProductID + " and numeric_value>0";
                     DataSet DSprice = new DataSet();
                     objHelperServices.SQLString = strquery;
                     DSprice = objHelperServices.GetDataSet();
                     if (DSprice != null && DSprice.Tables[0].Rows.Count > 0)
                     {

                         foreach (DataRow row in DSprice.Tables[0].Rows)
                         {
                             if (Convert.ToInt32(row["attribute_id"]) == 5111 && ProdQty >= 100)
                             {
                                 untPrice = objHelperServices.CDEC(row["numeric_value"]);
                             }
                             else if (Convert.ToInt32(row["attribute_id"]) == 127 && ProdQty >= 50)
                             {
                                 untPrice = objHelperServices.CDEC(row["numeric_value"]);
                             }
                             else if (Convert.ToInt32(row["attribute_id"]) == 40 && ProdQty >= 25)
                             {
                                 untPrice = objHelperServices.CDEC(row["numeric_value"]);
                             }
                             else if (Convert.ToInt32(row["attribute_id"]) == 5125 && ProdQty >= 25 && ProdQty <= 49)
                             {
                                 untPrice = objHelperServices.CDEC(row["numeric_value"]);
                             }
                             else if (Convert.ToInt32(row["attribute_id"]) == 5280 && ProdQty >= 10 && ProdQty <= 49)
                             {
                                 untPrice = objHelperServices.CDEC(row["numeric_value"]);
                             }
                             else if (Convert.ToInt32(row["attribute_id"]) == 39 && ProdQty >= 1 && ProdQty <= 24)
                             {
                                 untPrice = objHelperServices.CDEC(row["numeric_value"]);
                             }
                             else if (Convert.ToInt32(row["attribute_id"]) == 35 && ProdQty >= 10)
                             {
                                 untPrice = objHelperServices.CDEC(row["numeric_value"]);
                             }
                             else if (Convert.ToInt32(row["attribute_id"]) == 43 && ProdQty >= 1 && ProdQty <= 9)
                             {
                                 untPrice = objHelperServices.CDEC(row["numeric_value"]);
                             }
                             else if (Convert.ToInt32(row["attribute_id"]) == 5415 && ProdQty >= 8)
                             {
                                 untPrice = objHelperServices.CDEC(row["numeric_value"]);
                             }
                             else if (Convert.ToInt32(row["attribute_id"]) == 5007 && ProdQty >= 5)
                             {
                                 untPrice = objHelperServices.CDEC(row["numeric_value"]);
                             }
                             else if (Convert.ToInt32(row["attribute_id"]) == 5 && ProdQty >= 1)
                             {
                                 untPrice = objHelperServices.CDEC(row["numeric_value"]);
                             }
                         }
                     }
                     else if (untPrice <= 0)
                     {
                         untPrice = objHelperServices.CDEC(objProductServices.GetProductBasePrice(ProductID));
                     }*/
                    DiscPrice = (untPrice * DiscPrice) / 100;
                    untPrice = untPrice - DiscPrice;
                    untPrice = objHelperServices.CDEC(untPrice.ToString("N4"));

                }
                else
                {
                    //Check the user default buyer group or custome buyer group.
                    int BGPriceID = objBuyerGroupServices.GetBuyerGroupPriceID(UsrID);
                    string BGName = objBuyerGroupServices.GetBuyerGroup(UsrID);
                    if (BGPriceID == 4 && BGName == "DEFAULTBG")
                    {
                        //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", Session["USER_ID"]);
                        //objHelperServices.SQLString = sSQL;
                        //int pricecode = objHelperServices.CI(objHelperServices.GetValue("price_code"));


                        //string strquery = "";
                        //if (pricecode == 1)
                        //{
                        //    strquery = string.Format("exec GetWESIncProductPrice {0},{1},{2}", ProductID, ProdQty, Session["USER_ID"]);
                        //}
                        //else
                        //{
                        //    strquery = string.Format("exec GetWESProductPrice {0},{1},{2}", ProductID, ProdQty, Session["USER_ID"]);
                        //}


                        //DataSet DSprice = new DataSet();
                        //objHelperServices.SQLString = strquery;
                        //untPrice = Convert.ToDecimal(objHelperServices.GetValue("Numeric_Value"));

                        string user_id = Session["USER_ID"].ToString();

                        untPrice = objHelperDB.GetProductPrice_Exc(Product_id, ProdQty, user_id);

                        /*string strquery = "select product_id,numeric_value,attribute_id from tb_prod_specs where product_id=" + ProductID + " and numeric_value>0";
                        DataSet DSprice = new DataSet();
                        objHelperServices.SQLString = strquery;
                        DSprice = objHelperServices.GetDataSet();
                        if (DSprice != null && DSprice.Tables[0].Rows.Count > 0)
                        {

                            foreach (DataRow row in DSprice.Tables[0].Rows)
                            {
                                if (Convert.ToInt32(row["attribute_id"]) == 5111 && ProdQty >= 100)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 127 && ProdQty >= 50)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 40 && ProdQty >= 25)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 5125 && ProdQty >= 25 && ProdQty <= 49)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 5280 && ProdQty >= 10 && ProdQty <= 49)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 39 && ProdQty >= 1 && ProdQty <= 24)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 35 && ProdQty >= 10)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 43 && ProdQty >= 1 && ProdQty <= 9)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 5415 && ProdQty >= 8)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 5007 && ProdQty >= 5)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 4 && ProdQty >= 1)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                            }
                        }
                        else if (untPrice <= 0)
                        {
                            untPrice = objHelperServices.CDEC(objProductServices.GetProductBasePrice(ProductID));
                        }*/
                    }
                    else
                    {

                        //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", Session["USER_ID"]);
                        //objHelperServices.SQLString = sSQL;
                        //int pricecode = objHelperServices.CI(objHelperServices.GetValue("price_code"));

                        //string strquery = "";
                        //if (pricecode == 1)
                        //{
                        //    strquery = string.Format("exec GetWESIncProductPrice {0},{1},{2}", ProductID, ProdQty, Session["USER_ID"]);
                        //}
                        //else
                        //{
                        //    strquery = string.Format("exec GetWESProductPrice {0},{1},{2}", ProductID, ProdQty, Session["USER_ID"]);
                        //}

                        //DataSet DSprice = new DataSet();
                        //objHelperServices.SQLString = strquery;
                        //untPrice = Convert.ToDecimal(objHelperServices.GetValue("Numeric_Value"));

                        string user_id = Session["USER_ID"].ToString();

                        untPrice = objHelperDB.GetProductPrice_Exc(Product_id, ProdQty, user_id);

                        /*string strquery = "select product_id,numeric_value,attribute_id from tb_prod_specs where product_id=" + ProductID + " and numeric_value>0";
                     DataSet DSprice = new DataSet();
                     objHelperServices.SQLString = strquery;
                     DSprice = objHelperServices.GetDataSet();
                     if (DSprice != null && DSprice.Tables[0].Rows.Count > 0)
                     {

                           foreach (DataRow row in DSprice.Tables[0].Rows)
                             {
                                 if (Convert.ToInt32(row["attribute_id"]) == 5111 && ProdQty >= 100)
                                 {
                                     untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                 }
                                 else if (Convert.ToInt32(row["attribute_id"]) == 127 && ProdQty >= 50)
                                 {
                                     untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                 }
                                 else if (Convert.ToInt32(row["attribute_id"]) == 40 && ProdQty >= 25)
                                 {
                                     untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                 }
                                 else if (Convert.ToInt32(row["attribute_id"]) == 5125 && ProdQty >= 25 && ProdQty <= 49)
                                 {
                                     untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                 }
                                 else if (Convert.ToInt32(row["attribute_id"]) == 5280 && ProdQty >= 10 && ProdQty <= 49)
                                 {
                                     untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                 }
                                 else if (Convert.ToInt32(row["attribute_id"]) == 39 && ProdQty >= 1 && ProdQty <= 24)
                                 {
                                     untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                 }
                                 else if (Convert.ToInt32(row["attribute_id"]) == 35 && ProdQty >= 10)
                                 {
                                     untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                 }
                                 else if (Convert.ToInt32(row["attribute_id"]) == 43 && ProdQty >= 1 && ProdQty <=9)
                                 {
                                     untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                 }
                                 else if (Convert.ToInt32(row["attribute_id"]) == 5415 && ProdQty >= 8)
                                 {
                                     untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                 }
                                 else if (Convert.ToInt32(row["attribute_id"]) == 5007 && ProdQty >= 5)
                                 {
                                     untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                 }
                                 else if(Convert.ToInt32(row["attribute_id"]) == 4 && ProdQty >= 1)
                                 {
                                     untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                 }
                             }*/



                        //To calculate the discount price.
                        dsBgDisc = objBuyerGroupServices.GetBuyerGroupBasedDiscountDetails(BGName);
                        if (dsBgDisc != null)
                        {
                            if (dsBgDisc.Tables[0].Rows.Count > 0)
                            {
                                decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                                DateTime ValidDt = DateTime.Now.Subtract(System.TimeSpan.FromDays(7));//By default set the  previous date.
                                if (dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString() != "")
                                {
                                    ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                                }
                                string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                                bool IsBGCatProd = objBuyerGroupServices.IsBGCatalogProduct(CatalogID, objBuyerGroupServices.GetBuyerGroup(UsrID).ToString());
                                if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
                                {
                                    untPrice = objBuyerGroupServices.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth);
                                }
                            }
                        }
                        untPrice = objHelperServices.CDEC(untPrice.ToString("N4"));

                        /*}
                        else if (untPrice <= 0)
                        {
                            dsBgPrice = objProductServices.GetProductPriceValue(ProductID, BGPriceID);
                            if (dsBgPrice != null)
                            {
                                untPrice = objHelperServices.CDEC(dsBgPrice.Tables[0].Rows[0].ItemArray[1].ToString());

                                //To calculate the discount price.
                                dsBgDisc = oBG.GetBuyerGroupBasedDiscountDetails(BGName);
                                if (dsBgDisc != null)
                                {
                                    if (dsBgDisc.Tables[0].Rows.Count > 0)
                                    {
                                        decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                                        DateTime ValidDt = DateTime.Now.Subtract(System.TimeSpan.FromDays(7));//By default set the  previous date.
                                        if (dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString() != "")
                                        {
                                            ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                                        }
                                        string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                                        bool IsBGCatProd = oBG.IsBGCatalogProduct(CatalogID, oBG.GetBuyerGroup(UsrID).ToString());
                                        if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
                                        {
                                            untPrice = oBG.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth);
                                        }
                                    }
                                }
                                untPrice = objHelperServices.CDEC(untPrice.ToString("N4"));
                            }
                        }*/
                    }
                }//Buyergroup price.
                oItemInFo.ORDER_ITEM_ID = order_item_id;
                oItemInFo.ProductID = Product_id;
                oItemInFo.OrderID = OrID;

                oItemInFo.PriceApplied = untPrice;
                oItemInFo.UserID = UsrID;
                oItemInFo.Quantity = objHelperServices.CDEC(ProdQty);
                oItemInFo.Ship_Cost = objHelperServices.CDEC(CalculateShippingCost(OrID, Product_id, oItemInFo.PriceApplied, objHelperServices.CI(oItemInFo.Quantity)));
                oItemInFo.Tax_Amount = objOrderServices.CalculateTaxAmount(oItemInFo.Quantity * untPrice, OrID.ToString(), Product_id.ToString());
              //  objOrderServices.UpdateOrderItem(oItemInFo);
                DataRow dr = Order_update.NewRow();
                dr["ORDER_ITEM_ID"]=order_item_id;
                dr["PRODUCT_ID"]=Product_id;
                dr["ORDER_ID"]=OrID;
                dr["PRICE"]=untPrice;
                dr["USER_ID"]=UsrID;
                dr["QTY"]=oItemInFo.Quantity;
                dr["SHIP_COST"]=oItemInFo.Ship_Cost;
                dr["TAX_AMOUNT"]=oItemInFo.Tax_Amount;
                Order_update.Rows.Add(dr);
            }
            else
            {
                //string myscript = "<script type=\"text/javascript\">";
                ////myscript += "var ParmA ='1'; var ParmB = '2';  var ParmC ='3'; var MyArgs = new Array(ParmA, ParmB, ParmC);  var WinSettings = 'center:yes;resizable:no;dialogHeight:304px;dialogWidth:740px;scroll:no'; MyArgs = window.showModalDialog('viewcartmsg.aspx', MyArgs, WinSettings);";
                //myscript += " alert('Product already added in cart');";
                //myscript += "</script>";
                //if (!ClientScript.IsStartupScriptRegistered("PopupScript"))
                //{
                //    ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", myscript, true);
                //}

            }
            //oOrdInfo.OrderID = OrID;
           // objOrderServices.UpdateOrderPrice(oOrdInfo, true);
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }

    public int GetRole()
    {
        string userid = HttpContext.Current.Session["USER_ID"].ToString();
        string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
        //string sSQL = "SELECT USER_ROLE FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " and USER_ID = " + userid;
        //objHelperServices.SQLString = sSQL;
        //int iROLE = objHelperServices.CI(objHelperServices.GetValue("USER_ROLE"));
        //return iROLE;
        int iROLE = 0;
        string tempstr = (string)objHelperDB.GetGenericPageDataDB("", websiteid, userid, "GET_MULTIUSER_COMPANY_BUYERS_USER_ROLE", HelperDB.ReturnType.RTString);
        if (tempstr != null && tempstr != "")
            iROLE = objHelperServices.CI(tempstr);

        return iROLE;

    }


    public void AddToOrderTable()
    {
        try
        {
            int OrderID = 0;
            string OrderStatus = "";
            
            oOrdInfo.UserID = objHelperServices.CI(Session["USER_ID"]);

            if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
            {
                OrderID = Convert.ToInt32(Session["ORDER_ID"]);
            }
            else
            {
                OrderID = objOrderServices.GetOrderID(oOrdInfo.UserID, OpenOrdStatusID);
            }

            OrderStatus = objOrderServices.GetOrderStatus(OrderID);

            if (OrderID == 0 || OrderStatus == OrderServices.OrderStatus.PAYMENT.ToString() || OrderStatus == OrderServices.OrderStatus.SHIPPED.ToString() || OrderStatus == OrderServices.OrderStatus.COMPLETED.ToString() || OrderStatus == OrderServices.OrderStatus.CANCELED.ToString() || OrderStatus == OrderServices.OrderStatus.ORDERPLACED.ToString() || OrderStatus == OrderServices.OrderStatus.MANUALPROCESS.ToString())
            {
                objOrderServices.InitilizeOrder(oOrdInfo);

                if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
                {
                    OrderID = Convert.ToInt32(Session["ORDER_ID"]);
                }
                else
                {
                    OrderID = objOrderServices.GetOrderID(oOrdInfo.UserID, OpenOrdStatusID);
                }
               
                AddOrderItem(OrderID, oOrdInfo.UserID);
            }
            else if (OrderStatus == OrderServices.OrderStatus.OPEN.ToString() || OrderStatus == "CAU_PENDING")
            {
              
                AddOrderItem(OrderID, oOrdInfo.UserID);
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }

    public void AddOrderItem(int OrID, int UsrID)
    {
        try
        {
            //ProductPromotionServices objProductPromotionServices = new ProductPromotionServices();
            //BuyerGroupServices objBuyerGroupServices = new BuyerGroupServices();
            OrderServices.OrderItemInfo oItemInFo = new OrderServices.OrderItemInfo();
            decimal untPrice = 0.00M;
            DataSet dsBgPrice = new DataSet();
            DataSet dsBgDisc = new DataSet();
            int Product_id = 0;
            //OrderServices objOrderServices = new OrderServices();
            int chkExistsItem = 0;

            if ((HttpContext.Current.Request["Pid"]!= null && HttpContext.Current.Request["Qty"] != null) || (NewProduct_id != 0 && NewQty != 0))
            {
                //Modified by indu to check updated stoke status and price
                if (HttpContext.Current.Request["Pid"] != null)
                {
                    Product_id = objHelperServices.CI(HttpContext.Current.Request["Pid"].ToString());
                }
                else {
                    objErrorHandler.CreateLog("Add productid");
                    Product_id = NewProduct_id;
                }
                chkExistsItem = objOrderServices.GetOrderItemQty(Product_id, OrID, 0);

                int ProdQty;
                //if (MinQtyAvail >1)
                //{
                //  ProdQty = MinQtyAvail;
                //}
                //else
                //{
                if (HttpContext.Current.Request["Qty"] != null)
                {
                    ProdQty = objHelperServices.CI(HttpContext.Current.Request["Qty"].ToString());
                }
                else
                {
                    ProdQty = NewQty;
                }
                //}
                /*if (chkExistsItem == 0 || ProdQty != chkExistsItem)
                {*/

                //Chceck the promotion table.
                if (objProductPromotionServices.CheckPromotion(Product_id))
                {
                    decimal DiscPrice = objHelperServices.CDEC(objProductPromotionServices.GetProductPromotionDiscValue(Product_id));
                    //string strquery = "select product_id,numeric_value,attribute_id from tb_prod_specs where product_id=" + ProductID + " and numeric_value>0";

                    //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", Session["USER_ID"]);
                    //objHelperServices.SQLString = sSQL;
                    //int pricecode = objHelperServices.CI(objHelperServices.GetValue("price_code"));

                    //string strquery = "";
                    //if (pricecode == 1)
                    //{
                    //    strquery = string.Format("exec GetWESIncProductPrice {0},{1},{2}", ProductID, ProdQty, Session["USER_ID"]);
                    //}
                    //else
                    //{
                    //    strquery = string.Format("exec GetWESProductPrice {0},{1},{2}", ProductID, ProdQty, Session["USER_ID"]);
                    //}

                    //DataSet DSprice = new DataSet();
                    //objHelperServices.SQLString = strquery;
                    //untPrice = Convert.ToDecimal(objHelperServices.GetValue("Numeric_Value"));


                    string user_id = Session["USER_ID"].ToString();

                    untPrice = objHelperDB.GetProductPrice_Exc(Product_id, ProdQty, user_id);

                    //DSprice = objHelperServices.GetDataSet();
                    //if (DSprice != null && DSprice.Tables[0].Rows.Count > 0)
                    //{

                    //    foreach (DataRow row in DSprice.Tables[0].Rows)
                    //    {
                    //        if (Convert.ToInt32(row["attribute_id"]) == 5111 && ProdQty >= 100)
                    //        {
                    //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                    //        }
                    //        else if (Convert.ToInt32(row["attribute_id"]) == 127 && ProdQty >= 50)
                    //        {
                    //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                    //        }
                    //        else if (Convert.ToInt32(row["attribute_id"]) == 40 && ProdQty >= 25)
                    //        {
                    //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                    //        }
                    //        else if (Convert.ToInt32(row["attribute_id"]) == 5125 && ProdQty >= 25 && ProdQty <= 49)
                    //        {
                    //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                    //        }
                    //        else if (Convert.ToInt32(row["attribute_id"]) == 5280 && ProdQty >= 10 && ProdQty <= 49)
                    //        {
                    //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                    //        }
                    //        else if (Convert.ToInt32(row["attribute_id"]) == 39 && ProdQty >= 1 && ProdQty <= 24)
                    //        {
                    //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                    //        }
                    //        else if (Convert.ToInt32(row["attribute_id"]) == 35 && ProdQty >= 10)
                    //        {
                    //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                    //        }
                    //        else if (Convert.ToInt32(row["attribute_id"]) == 43 && ProdQty >= 1 && ProdQty <= 9)
                    //        {
                    //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                    //        }
                    //        else if (Convert.ToInt32(row["attribute_id"]) == 5415 && ProdQty >= 8)
                    //        {
                    //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                    //        }
                    //        else if (Convert.ToInt32(row["attribute_id"]) == 5007 && ProdQty >= 5)
                    //        {
                    //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                    //        }
                    //        else if (Convert.ToInt32(row["attribute_id"]) == 4 && ProdQty >= 1)
                    //        {
                    //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                    //        }
                    //    }
                    //}
                    //else if (untPrice <= 0)
                    //{
                    //    untPrice = objHelperServices.CDEC(objProductServices.GetProductBasePrice(ProductID));
                    //}
                    DiscPrice = (untPrice * DiscPrice) / 100;
                    untPrice = untPrice - DiscPrice;
                    untPrice = objHelperServices.CDEC(untPrice.ToString("N4"));

                }
                else
                {
                    //Check the user default buyer group or custome buyer group.
                    int BGPriceID = objBuyerGroupServices.GetBuyerGroupPriceID(UsrID);
                    string BGName = objBuyerGroupServices.GetBuyerGroup(UsrID);
                    if (BGPriceID == 4 && BGName == "DEFAULTBG")
                    {
                        //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", Session["USER_ID"]);
                        //objHelperServices.SQLString = sSQL;
                        //int pricecode = objHelperServices.CI(objHelperServices.GetValue("price_code"));

                        //string strquery = "";
                        //if (pricecode == 1)
                        //{
                        //    strquery = string.Format("exec GetWESIncProductPrice {0},{1},{2}", ProductID, ProdQty, Session["USER_ID"]);
                        //}
                        //else
                        //{
                        //    strquery = string.Format("exec GetWESProductPrice {0},{1},{2}", ProductID, ProdQty, Session["USER_ID"]);
                        //}

                        //DataSet DSprice = new DataSet();
                        //objHelperServices.SQLString = strquery;
                        //untPrice = Convert.ToDecimal(objHelperServices.GetValue("Numeric_Value"));


                        string user_id = Session["USER_ID"].ToString();

                        untPrice = objHelperDB.GetProductPrice_Exc(Product_id, ProdQty, user_id);

                        //string strquery = "select product_id,numeric_value,attribute_id from tb_prod_specs where product_id="+ProductID+" and numeric_value>0";
                        //DataSet DSprice = new DataSet();                           
                        //objHelperServices.SQLString = strquery;
                        //DSprice = objHelperServices.GetDataSet();
                        //if (DSprice != null && DSprice.Tables[0].Rows.Count > 0)
                        //{

                        //    foreach (DataRow row in DSprice.Tables[0].Rows)
                        //    {
                        //        if (Convert.ToInt32(row["attribute_id"]) == 5111 && ProdQty >= 100)
                        //        {
                        //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                        //        }
                        //        else if (Convert.ToInt32(row["attribute_id"]) == 127 && ProdQty >= 50)
                        //        {
                        //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                        //        }
                        //        else if (Convert.ToInt32(row["attribute_id"]) == 40 && ProdQty >= 25)
                        //        {
                        //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                        //        }
                        //        else if (Convert.ToInt32(row["attribute_id"]) == 5125 && ProdQty >= 25 && ProdQty <= 49)
                        //        {
                        //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                        //        }
                        //        else if (Convert.ToInt32(row["attribute_id"]) == 5280 && ProdQty >= 10 && ProdQty <= 49)
                        //        {
                        //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                        //        }
                        //        else if (Convert.ToInt32(row["attribute_id"]) == 39 && ProdQty >= 1 && ProdQty <= 24)
                        //        {
                        //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                        //        }
                        //        else if (Convert.ToInt32(row["attribute_id"]) == 35 && ProdQty >= 10)
                        //        {
                        //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                        //        }
                        //        else if (Convert.ToInt32(row["attribute_id"]) == 43 && ProdQty >= 1 && ProdQty <=9)
                        //        {
                        //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                        //        }
                        //        else if (Convert.ToInt32(row["attribute_id"]) == 5415 && ProdQty >= 8)
                        //        {
                        //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                        //        }
                        //        else if (Convert.ToInt32(row["attribute_id"]) == 5007 && ProdQty >= 5)
                        //        {
                        //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                        //        }
                        //        else if(Convert.ToInt32(row["attribute_id"]) == 4 && ProdQty >= 1)
                        //        {
                        //            untPrice = objHelperServices.CDEC(row["numeric_value"]);
                        //        }
                        //    }
                        //}


                    }
                    else
                    {
                        //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", Session["USER_ID"]);
                        //objHelperServices.SQLString = sSQL;
                        //int pricecode = objHelperServices.CI(objHelperServices.GetValue("price_code"));

                        //string strquery = "";
                        //if (pricecode == 1)
                        //{
                        //    strquery = string.Format("exec GetWESIncProductPrice {0},{1},{2}", ProductID, ProdQty, Session["USER_ID"]);
                        //}
                        //else
                        //{
                        //    strquery = string.Format("exec GetWESProductPrice {0},{1},{2}", ProductID, ProdQty, Session["USER_ID"]);
                        //}

                        //DataSet DSprice = new DataSet();
                        //objHelperServices.SQLString = strquery;
                        //untPrice = Convert.ToDecimal(objHelperServices.GetValue("Numeric_Value"));

                        string user_id = Session["USER_ID"].ToString();

                        untPrice = objHelperDB.GetProductPrice_Exc(Product_id, ProdQty, user_id);

                        /*string strquery = "select product_id,numeric_value,attribute_id from tb_prod_specs where product_id="+ProductID+" and numeric_value>0";
                        DataSet DSprice = new DataSet();                           
                        objHelperServices.SQLString = strquery;
                        DSprice = objHelperServices.GetDataSet();
                        if (DSprice != null && DSprice.Tables[0].Rows.Count > 0)
                        {

                              foreach (DataRow row in DSprice.Tables[0].Rows)
                            {
                                if (Convert.ToInt32(row["attribute_id"]) == 5111 && ProdQty >= 100)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 127 && ProdQty >= 50)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 40 && ProdQty >= 25)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 5125 && ProdQty >= 25 && ProdQty <= 49)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 5280 && ProdQty >= 10 && ProdQty <= 49)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 39 && ProdQty >= 1 && ProdQty <= 24)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 35 && ProdQty >= 10)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 43 && ProdQty >= 1 && ProdQty <=9)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 5415 && ProdQty >= 8)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if (Convert.ToInt32(row["attribute_id"]) == 5007 && ProdQty >= 5)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                                else if(Convert.ToInt32(row["attribute_id"]) == 4 && ProdQty >= 1)
                                {
                                    untPrice = objHelperServices.CDEC(row["numeric_value"]);
                                }
                            }
                        }*/


                        //To calculate the discount price.
                        dsBgDisc = objBuyerGroupServices.GetBuyerGroupBasedDiscountDetails(BGName);
                        if (dsBgDisc != null)
                        {
                            if (dsBgDisc.Tables[0].Rows.Count > 0)
                            {
                                decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                                DateTime ValidDt = DateTime.Now.Subtract(System.TimeSpan.FromDays(7));//By default set the  previous date.
                                if (dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString() != "")
                                {
                                    ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                                }
                                string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                                bool IsBGCatProd = objBuyerGroupServices.IsBGCatalogProduct(CatalogID, objBuyerGroupServices.GetBuyerGroup(UsrID).ToString());
                                if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
                                {
                                    untPrice = objBuyerGroupServices.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth);
                                }
                            }
                        }
                        untPrice = objHelperServices.CDEC(untPrice.ToString("N4"));


                        /*else if (untPrice <= 0)
                        {
                            dsBgPrice = objProductServices.GetProductPriceValue(ProductID, BGPriceID);

                            if (dsBgPrice != null)
                            {
                                untPrice = objHelperServices.CDEC(dsBgPrice.Tables[0].Rows[0].ItemArray[1].ToString());

                                //To calculate the discount price.
                                dsBgDisc = oBG.GetBuyerGroupBasedDiscountDetails(BGName);
                                if (dsBgDisc != null)
                                {
                                    if (dsBgDisc.Tables[0].Rows.Count > 0)
                                    {
                                        decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                                        DateTime ValidDt = DateTime.Now.Subtract(System.TimeSpan.FromDays(7));//By default set the  previous date.
                                        if (dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString() != "")
                                        {
                                            ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                                        }
                                        string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                                        bool IsBGCatProd = oBG.IsBGCatalogProduct(CatalogID, oBG.GetBuyerGroup(UsrID).ToString());
                                        if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
                                        {
                                            untPrice = oBG.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth);
                                        }
                                    }
                                }
                                untPrice = objHelperServices.CDEC(untPrice.ToString("N4"));
                            }
                        }*/
                    }
                }//Buyergroup price.




                oItemInFo.ORDER_ITEM_ID = 0;
                //oItemInFo.ProductID = objHelperServices.CI(HttpContext.Current.Request["Pid"]);
                oItemInFo.ProductID = objHelperServices.CI(Product_id);
                oItemInFo.OrderID = OrID;
                oItemInFo.PriceApplied = untPrice;
                oItemInFo.UserID = UsrID;
                oItemInFo.Quantity = ProdQty;
                oItemInFo.Ship_Cost = CalculateShippingCost(OrID, Product_id, oItemInFo.PriceApplied, objHelperServices.CI(oItemInFo.Quantity));
                oItemInFo.Tax_Amount = objOrderServices.CalculateTaxAmount(oItemInFo.Quantity * untPrice, OrID.ToString(), Product_id.ToString());


               // objErrorHandler.CreateLog("GetStockStatus " + oItemInFo.ProductID);





                if (objProductServices.GetStockStatus(oItemInFo.ProductID, UsrID) == 0)
                {
                    OrderServices.Order_Calrification_ItemInfo oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
                    oItemClaItemInfo.Clarification_ID = 0;
                    oItemClaItemInfo.OrderID = OrID;
                    oItemClaItemInfo.ProductDesc = objProductServices.GetProductCode(oItemInFo.ProductID);
                    oItemClaItemInfo.Quantity = ProdQty;
                    oItemClaItemInfo.UserID = UsrID;
                    oItemClaItemInfo.Clarification_Type = "ITEM_REPLACE";
                    
                    objOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);
                }
                else if (untPrice==0 && objProductServices.GetProductPromotion(oItemInFo.ProductID) == "N")
                {
                    OrderServices.Order_Calrification_ItemInfo oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
                    oItemClaItemInfo.Clarification_ID = 0;
                    oItemClaItemInfo.OrderID = OrID;
                    oItemClaItemInfo.ProductDesc = objProductServices.GetProductCode(oItemInFo.ProductID);
                    oItemClaItemInfo.Quantity = ProdQty;
                    oItemClaItemInfo.UserID = UsrID;
                    oItemClaItemInfo.Clarification_Type = "ITEM_PROMOTION";
                 
                    objOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);
                }
             




                else
                {
                  
                    objOrderServices.AddOrderItem(oItemInFo);
                }

  
                /* if (ProdQty != 0 && untPrice != 0)
                 {
                     Session["NOITEMADDED"] = "";
                     int OrderID = OrID;
                     int maxqty = objOrderServices.GetOrderItemQty(Product_id , OrderID,0);
                     int MinQty = objOrderServices.GetProductMinimumOrderQty(Product_id);
                     int MaxQtyAvl = maxqty + objOrderServices.GetProductAvilableQty(Product_id);
                     oItemInFo.Quantity = objHelperServices.CDEC(ProdQty + chkExistsItem);
                     int Qty = objHelperServices.CI(oItemInFo.Quantity);
                     ProdQty = MaxQtyAvl - Qty;
                     if (ProdQty >= 0)
                         objOrderServices.UpdateQuantity(Product_id, ProdQty + chkExistsItem);
                 }
                 else
                 {
                     oItemInFo.Quantity = 1;
                 }*/

                /*  if (chkExistsItem == 0)
                  {
                     if (oItemInFo.PriceApplied != 0)
                      {
                          Session["NOITEMADDED"] = "";
                           if (objOrderServices.AddOrderItem(oItemInFo) != -1)
                          {
                              DataSet dsOrder = new DataSet();
                              dsOrder = objOrderServices.GetOrderPriceValues(OrID);

                              if (dsOrder != null)
                              {
                                  decimal ProdTotalPrice;
                                  decimal OrderTotal;
                                  decimal TotalShipCost;

                                  //Calculate Shipping Cost
                                  decimal ProdShippCost = objHelperServices.CDEC(CalculateShippingCost(OrID, Product_id, oItemInFo.PriceApplied, objHelperServices.CI(oItemInFo.Quantity)));
                                  decimal ExistProdTotal = objHelperServices.CDEC(objOrderServices.GetCurrentProductTotalCost(OrID));
                                  ProdTotalPrice = ExistProdTotal + (oItemInFo.PriceApplied * oItemInFo.Quantity);
                                  decimal Tax = CalculateTaxAmount(ProdTotalPrice);
                                  TotalShipCost = objHelperServices.CDEC(objOrderServices.GetShippingCost(OrID)) + ProdShippCost;

                                  if (_IsShippingFree == true)
                                  {
                                      TotalShipCost = 0;
                                      _IsShippingFree = false;
                                  }

                                  oOrdInfo.ShipCost = objHelperServices.CDEC(objHelperServices.FixDecPlace(TotalShipCost));
                                  OrderTotal = ProdTotalPrice + Tax + TotalShipCost;
                                  oOrdInfo.OrderID = OrID;
                                  oOrdInfo.ProdTotalPrice = objHelperServices.CDEC(objHelperServices.FixDecPlace(ProdTotalPrice));
                                  oOrdInfo.TaxAmount = objHelperServices.CDEC(objHelperServices.FixDecPlace(Tax));
                                  oOrdInfo.TotalAmount = objHelperServices.CDEC(objHelperServices.FixDecPlace(OrderTotal));
                                  objOrderServices.UpdateOrderPrice(oOrdInfo, true);
                              }
                          }
                           
                      }
                      else
                      {
                          Session["NOITEMADDED"] = "NoPrice";
                          //string script = "<script>alert('Invalid price amount.');</script>";
                          //if (!IsClientScriptBlockRegistered("alert"))
                          //{
                          //    this.RegisterClientScriptBlock("alert", script);
                          //}
                      }
                  }
                  else
                  {
                      //update the existing order item.
                      //Update the new product price to exists products price.
                      DataSet dsOrder = new DataSet();
                      dsOrder = objOrderServices.GetOrderPriceValues(OrID);

                      if (dsOrder != null)
                      {
                          decimal ProdTotalPrice;
                          decimal OrderTotal;
                          decimal TotalShipCost;
                          decimal ExistProdTotal = objHelperServices.CDEC(objOrderServices.GetCurrentProductTotalCost(OrID));

                          decimal Tax = 0.00M;
                          //= objHelperServices.CDEC(ConfigurationManager.AppSettings["SalesTax"].ToString());
                          if (ProdQty >= chkExistsItem)
                          {
                              ProdTotalPrice = (ExistProdTotal + (oItemInFo.PriceApplied * (oItemInFo.Quantity - chkExistsItem)));
                          }
                          else
                          {
                              ProdTotalPrice = (ExistProdTotal - (oItemInFo.PriceApplied * (chkExistsItem - oItemInFo.Quantity)));
                          }
                          //Calculate the shipping cost
                          decimal ProdShippCost = objHelperServices.CDEC(CalculateShippingCost(OrID, Product_id , oItemInFo.PriceApplied, objHelperServices.CI(chkExistsItem - oItemInFo.Quantity)));

                          Tax = CalculateTaxAmount(ProdTotalPrice); //(ProdTotalPrice * Tax) / 100;
                          OrderTotal = ProdTotalPrice + Tax;

                          oOrdInfo.OrderID = OrID;
                          oOrdInfo.ProdTotalPrice = objHelperServices.CDEC(objHelperServices.FixDecPlace(ProdTotalPrice));
                          oOrdInfo.TaxAmount = objHelperServices.CDEC(objHelperServices.FixDecPlace(Tax));
                          oOrdInfo.TotalAmount = objHelperServices.CDEC(objHelperServices.FixDecPlace(OrderTotal));
                          objOrderServices.UpdateOrderPrice(oOrdInfo, true);
                      }
                      objOrderServices.UpdateOrderItem(oItemInFo);
                    
                  }
               
                  else
                  {
                      //string myscript = "<script type=\"text/javascript\">";
                      ////myscript += "var ParmA ='1'; var ParmB = '2';  var ParmC ='3'; var MyArgs = new Array(ParmA, ParmB, ParmC);  var WinSettings = 'center:yes;resizable:no;dialogHeight:304px;dialogWidth:740px;scroll:no'; MyArgs = window.showModalDialog('viewcartmsg.aspx', MyArgs, WinSettings);";
                      //myscript += "alert('Product already added in cart');";
                      //myscript += "</script>";
                      //if (!ClientScript.IsStartupScriptRegistered("PopupScript"))
                      //{
                      //    ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", myscript, true);
                      //} 
                  }*/
            }
            oOrdInfo.OrderID = OrID;
            objOrderServices.UpdateOrderPrice(oOrdInfo, true);
            if (HttpContext.Current.Request.QueryString["bulkorder"] != null && HttpContext.Current.Request.QueryString["bulkorder"].ToString() == "1")
            {
                if (HttpContext.Current.Request["rma"] != null)
                {
                    string _rma = HttpContext.Current.Request["rma"].ToString();
                    string _rmitem = HttpContext.Current.Request["Item"].ToString();
                    string _rmqty = "";
                    double CalItem_ID = 0;

                    if (HttpContext.Current.Request.QueryString["DelQty"] != null)
                    {
                        _rmqty = HttpContext.Current.Request["DelQty"].ToString();
                    }
                    if (HttpContext.Current.Request.QueryString["cla_id"] != null)
                    {
                        CalItem_ID = Convert.ToDouble(HttpContext.Current.Request["cla_id"].ToString());

                    }
                    if (_rma == "NF")
                    {
                        //Session["ITEM_ERROR"] = Session["ITEM_ERROR"].ToString().Replace(_rmitem + ",", "");
                        objOrderServices.Remove_Clarification_item(CalItem_ID);
                    }
                    if (_rma == "CI")
                    {
                        //Session["ITEM_CHK"] = Session["ITEM_CHK"].ToString().Replace(_rmitem + ",", "");
                        //Session["QTY_CHK"] = Session["QTY_CHK"].ToString().Replace(_rmqty + ",", "");
                        objOrderServices.Remove_Clarification_item(CalItem_ID);
                    }
                }
            }
            if (HttpContext.Current.Request.QueryString["popup"] == null && NewProduct_id == 0)
            {
                try
                {
                    Response.Redirect("OrderDetails.aspx?ORDER_ID=" + OrID + "&bulkorder=1&ViewOrder=View");
                }
                catch (Exception ex)
                {}
            }
            //else
             //   Response.Redirect("OrderDetails.aspx?ORDER_ID=" + OrID + "&bulkorder=1&ViewOrder=View");

        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    public void AddPopUpItem( int qty, int Product_id,int family_id)
    {
        DataTable dt = objProductServices.GetPopProduct(Product_id, family_id);
        if (dt != null && dt.Rows.Count>0  )
        {
            lblFamilyName.Text = dt.Rows[0]["Family_name"].ToString();
            lblordercode.Text = dt.Rows[0]["Code"].ToString();
            lblQty.Text = qty.ToString();
            lblDesc.Text = dt.Rows[0]["PROD_DSC"].ToString();
            System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dt.Rows[0]["TWeb Image1"].ToString().Replace("\\", "/"));
            if (Fil.Exists)
                lblProImage.ImageUrl = "prodimages"+dt.Rows[0]["TWeb Image1"].ToString().Replace("\\", "/");
            else
                lblProImage.ImageUrl = "/images/noimage.gif";
        }
    }
    public void AddOrderItem(int OrID, int UsrID, int qty, int Product_id)
    {
        try
        {
            OrderServices.OrderItemInfo oItemInFo = new OrderServices.OrderItemInfo();
            decimal untPrice = 0.00M;
            DataSet dsBgPrice = new DataSet();
            DataSet dsBgDisc = new DataSet();

            if (objProductPromotionServices.CheckPromotion(Product_id))
            {
                decimal DiscPrice = objHelperServices.CDEC(objProductPromotionServices.GetProductPromotionDiscValue(Product_id));

                string user_id = Session["USER_ID"].ToString();

                untPrice = objHelperDB.GetProductPrice_Exc(Product_id, qty, user_id);

                DiscPrice = (untPrice * DiscPrice) / 100;
                untPrice = untPrice - DiscPrice;
                untPrice = objHelperServices.CDEC(untPrice.ToString("N4"));

            }
            else
            {
                //Check the user default buyer group or custome buyer group.
                int BGPriceID = objBuyerGroupServices.GetBuyerGroupPriceID(UsrID);
                string BGName = objBuyerGroupServices.GetBuyerGroup(UsrID);
                if (BGPriceID == 4 && BGName == "DEFAULTBG")
                {
                    string user_id = Session["USER_ID"].ToString();

                    untPrice = objHelperDB.GetProductPrice_Exc(Product_id, qty, user_id);




                }
                else
                {


                    string user_id = Session["USER_ID"].ToString();

                    untPrice = objHelperDB.GetProductPrice_Exc(Product_id, qty, user_id);




                    //To calculate the discount price.
                    dsBgDisc = objBuyerGroupServices.GetBuyerGroupBasedDiscountDetails(BGName);
                    if (dsBgDisc != null)
                    {
                        if (dsBgDisc.Tables[0].Rows.Count > 0)
                        {
                            decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                            DateTime ValidDt = DateTime.Now.Subtract(System.TimeSpan.FromDays(7));//By default set the  previous date.
                            if (dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString() != "")
                            {
                                ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                            }
                            string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                            bool IsBGCatProd = objBuyerGroupServices.IsBGCatalogProduct(CatalogID, objBuyerGroupServices.GetBuyerGroup(UsrID).ToString());
                            if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
                            {
                                untPrice = objBuyerGroupServices.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth);
                            }
                        }
                    }
                    untPrice = objHelperServices.CDEC(untPrice.ToString("N4"));



                }
            }//Buyergroup price.
            oItemInFo.ORDER_ITEM_ID = 0;
            oItemInFo.ProductID = Product_id;
            oItemInFo.OrderID = OrID;
            oItemInFo.PriceApplied = untPrice;
            oItemInFo.UserID = UsrID;
            oItemInFo.Quantity = qty;
            oItemInFo.Ship_Cost = CalculateShippingCost(OrID, Product_id, oItemInFo.PriceApplied, objHelperServices.CI(oItemInFo.Quantity));
            oItemInFo.Tax_Amount = objOrderServices.CalculateTaxAmount(oItemInFo.Quantity * untPrice, OrID.ToString(), Product_id.ToString());

            objOrderServices.AddOrderItem(oItemInFo);


        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }

   
    protected decimal CalculateShippingCost(int OID, int ProdId, decimal ProdApplyPrice, int itemQty)
    {
        _IsShippingFree = false;
        DataSet dsOItem = new DataSet();
        decimal ShippingValue = 0;
        decimal ProdShippCost = 0;
        dsOItem = objOrderServices.GetItemDetailsFromInventory(ProdId);
        decimal ProductCost = (ProdApplyPrice * itemQty);
        if (objHelperServices.GetOptionValues("ENABLE ITEM SHIPPING").ToString().ToUpper() == "YES")
        {
            if (dsOItem != null)
            {
                foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                {
                    if (objHelperServices.CB(rItem["IS_SHIPPING"]) == 1)
                    {
                        ProdShippCost = ((ProductCost * objHelperServices.CDEC(rItem["PROD_SHIP_COST"])) / 100);
                    }
                }
            }
        }
        else
        {
            if ((objOrderServices.GetCurrentProductTotalCost(OID) + ProdApplyPrice) < objHelperServices.CDEC(objHelperServices.GetOptionValues("SHIPPING FREE").ToString()))
            {
                if (dsOItem != null)
                {
                    foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                    {
                        ProdShippCost = ((ProductCost * objHelperServices.CI(objHelperServices.GetOptionValues("SHIPPING CHARGE").ToString())) / 100);
                    }
                }
            }
            else
            {
                _IsShippingFree = true;
            }
        }
        return objHelperServices.CDEC(ProdShippCost);
    }

    #endregion

    #region "Control Events.."

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string SelAll = "";
            Session["SellAllClick"] = 0;
            if (Request["SelAll"] == "1")
            {
                SelAll = "0";
            }
            else if (Request["SelAll"] == null)
            {
                SelAll = "1";
                Session["SellAllClick"] = 1;
            }
            else
            {
                SelAll = "1";
            }
            Response.Redirect("OrderDetails.aspx?&bulkorder=1&SelAll=" + SelAll); //+ ",SelPid=" + pid ); //",DelFlag=0");
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }

    protected void btnUpdateCart_Click(object sender, EventArgs e)
    {
        try
        {
            OrderServices.OrderItemInfo oOrdItemInfo = new OrderServices.OrderItemInfo();
            int RowCnt;
            decimal TotalAmt = 0.00M;
            decimal UntPrice = 0.00M;
            decimal ProdTotal = 0.00M;
            decimal tax = 0.00M;
            decimal TaxAmt = 0.00M;
            decimal OrdTotal = 0.00M;
            decimal ProdShipCost = 0.00M;
            decimal TotalShipCost = 0.00M;
            int nQty;
            int PrdId;
            string SelAll = "";
            int OrderID = 0;

            if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
            {
                OrderID = Convert.ToInt32(Session["ORDER_ID"]);
            }
            else
            {
                OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);
            }


            // RowCnt = objOrderServices.GetOrderItemCount(OrderID);
            string LeaveDuplicateProds = GetLeaveDuplicateProducts();
            DataSet dsOItem = objOrderServices.GetOrderItemsWithoutDuplicate(OrderID, LeaveDuplicateProds);

            if (dsOItem != null && dsOItem.Tables.Count > 0 && dsOItem.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                {
                    //bool attrcheck = false;
                    PrdId = objHelperServices.CI(Request.Form["txtPid" + rItem["ORDER_ITEM_ID"].ToString()]);
                    nQty = objHelperServices.CI(Request.Form["txtQty" + rItem["ORDER_ITEM_ID"].ToString()]);

                    if (nQty <= 0)
                    {
                        string ErrorMessage = "Invalid Quantity! Quantity Should be Equal/Greater than 1";
                        //MessageBox.Show(ErrorMessage, "Order Details", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //ShowMessageBoxInformation();

                        //Page page = HttpContext.Current.CurrentHandler as Page;
                        //string script = "<script type='text/javascript' language='javascript'>alert('" + ErrorMessage + "');</script>";
                        //page.ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script);


                        //ErrorMessage = "<script>";
                        //ErrorMessage = ErrorMessage + "alert('Invalid Quantity! Quantity Should be Equal/Greater than 1')";
                        //ErrorMessage = ErrorMessage + "</script>";
                        //this.RegisterClientScriptBlock("alert", ErrorMessage);
                        ShowAlertMessageBox("Invalid Quantity! Quantity Should be Equal/Greater than 1");
                        return;
                    }

                    //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", HttpContext.Current.Session["USER_ID"]);
                    //objHelperServices.SQLString = sSQL;
                    //int pricecode = objHelperServices.CI(objHelperServices.GetValue("price_code"));

                    //string strquery = "";
                    //if (pricecode == 1)
                    //{
                    //    strquery = string.Format("exec GetWESIncProductPrice {0},{1},{2}", PrdId, nQty, HttpContext.Current.Session["USER_ID"]);
                    //}
                    //else
                    //{
                    //    strquery = string.Format("exec GetWESProductPrice {0},{1},{2}", PrdId, nQty, HttpContext.Current.Session["USER_ID"]);
                    //}

                    //DataSet DSprice = new DataSet();
                    //objHelperServices.SQLString = strquery;
                    //UntPrice = Convert.ToDecimal(objHelperServices.GetValue("Numeric_Value"));


                    string user_id = Session["USER_ID"].ToString();

                    UntPrice = objHelperDB.GetProductPrice_Exc(PrdId, nQty, user_id);

                    TotalAmt = UntPrice * nQty;
                    oOrdItemInfo.ORDER_ITEM_ID = objHelperServices.CD(rItem["ORDER_ITEM_ID"].ToString());
                    oOrdItemInfo.UserID = objHelperServices.CI(Session["USER_ID"].ToString());
                    oOrdItemInfo.ProductID = PrdId;
                    oOrdItemInfo.Quantity = nQty;
                    oOrdItemInfo.OrderID = OrderID;
                    oOrdItemInfo.PriceApplied = UntPrice;
                    oOrdItemInfo.Ship_Cost = CalculateShippingCost(OrderID, PrdId, UntPrice, nQty);
                    oOrdItemInfo.Tax_Amount = objOrderServices.CalculateTaxAmount(TotalAmt, OrderID.ToString(), PrdId.ToString());

                    // ProdShipCost = CalculateShippingCost(OrderID, PrdId, UntPrice, nQty);
                    // TotalShipCost = TotalShipCost + ProdShipCost;

                    // ProdTotal = objHelperServices.CDEC(ProdTotal + TotalAmt);

                    // Code updated Padmanaban,JTECH
                    // TO update the qty entered by the user, commented the below to block the available qty verification
                    //
                    // int oQty = objOrderServices.GetOrderItemQty(PrdId, OrderID);
                    // int AvailQty = objOrderServices.GetProductAvilableQty(PrdId);
                    int n = nQty; // AvailQty + oQty - nQty;
                    if (n >= 0)
                    {
                        objOrderServices.UpdateQuantity(PrdId, n);
                        objOrderServices.UpdateOrderItem(oOrdItemInfo);
                    }
                    nQty = objHelperServices.CI(Request.Form["txtQty" + rItem["ORDER_ITEM_ID"].ToString()]);
                    SelAll = "0";
                }
                //TaxAmt = CalculateTaxAmount(ProdTotal);
                //OrdTotal = objHelperServices.CDEC(ProdTotal + TaxAmt + TotalShipCost);
                oOrdInfo.OrderID = OrderID;
                //oOrdInfo.TaxAmount = objHelperServices.CDEC(objHelperServices.FixDecPlace(TaxAmt));
                //oOrdInfo.ProdTotalPrice = objHelperServices.CDEC(objHelperServices.FixDecPlace(ProdTotal));
                //oOrdInfo.ShipCost = objHelperServices.CDEC(objHelperServices.FixDecPlace(TotalShipCost));
                //oOrdInfo.TotalAmount = objHelperServices.CDEC(objHelperServices.FixDecPlace(OrdTotal));
                objOrderServices.UpdateOrderPrice(oOrdInfo, true);
            }
            //Response.Redirect("OrderDetails.aspx?reload=" + SelAll, false);
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }


    protected void btnSaveOrdTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            msgOTAlert.Hide();
            msgAlert.Hide();
            TxtTemplateName.Text = "";
            TxtDesc.Text = "";  
 

            ShowOTAlertMessageBox();

            //OrderServices.OrderTemplateInfo oOrdtemplate = new OrderServices.OrderTemplateInfo();

           
            //int nQty;
            //int PrdId;           
            //int user_id =objHelperServices.CI(Session["USER_ID"].ToString());
            //int OrderID = 0;
            //string ErrorMessage = "";
            //Page page = null;
            //string script = "";
            //if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
            //{
            //    OrderID = Convert.ToInt32(Session["ORDER_ID"]);
            //}
            //else
            //{
            //    OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);
            //}


            //string LeaveDuplicateProds = GetLeaveDuplicateProducts();
            //DataSet dsOItem = objOrderServices.GetOrderItemsWithoutDuplicate(OrderID, LeaveDuplicateProds);


            //objOrderServices.RemoveOrderTemplate(user_id);
            //if (dsOItem != null && dsOItem.Tables.Count > 0 && dsOItem.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow rItem in dsOItem.Tables[0].Rows)
            //    {
            //        //bool attrcheck = false;
            //        PrdId = objHelperServices.CI(Request.Form["txtPid" + rItem["ORDER_ITEM_ID"].ToString()]);
            //        nQty = objHelperServices.CI(Request.Form["txtQty" + rItem["ORDER_ITEM_ID"].ToString()]);

            //        if (nQty <= 0)
            //        {


            //            ShowAlertMessageBox("Invalid Quantity! Quantity Should be Equal/Greater than 1");
            //            return;
            //        }




            //        oOrdtemplate.UserID = user_id;
            //        //oOrdtemplate.ProductID = PrdId;
            //        //oOrdtemplate.Quantity = nQty;
            //        objOrderServices.AddOrderItemTemplate(oOrdtemplate);
            //    }
            //}

            //ShowAlertMessageBox("Successfully saved");
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }


    protected void btnSaveOrdTemplate(object sender, EventArgs e)
    {
        try
        {



            //OrderServices.OrderTemplateInfo oOrdtemplate = new OrderServices.OrderTemplateInfo();


            int nQty;
            int PrdId;
            int user_id = objHelperServices.CI(Session["USER_ID"].ToString());
            int OrderID = 0;
            int Template_id=0;
            string ErrorMessage = "";
            Page page = null;
            string script = "";

            if (TxtTemplateName.Text.Trim() == "")
            {
                msgOTAlert.Hide();
                msgAlert.Hide();
                ShowOTAlertMessageBox();              
                txterr.Text = "Enter Template Name";
                return;
            }
            if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
            {
                OrderID = Convert.ToInt32(Session["ORDER_ID"]);
            }
            else
            {
                OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);
            }


            //string LeaveDuplicateProds = GetLeaveDuplicateProducts();
            DataSet dsOItem = objOrderServices.GetOrderItemsWithoutDuplicate(OrderID, "");

            OrderServices.OrderTemplateInfo oTemInFo = new OrderServices.OrderTemplateInfo();
            OrderServices.OrderTemplateItemInfo oItemInFo = new OrderServices.OrderTemplateItemInfo();
            oTemInFo.TemplateId = 0;
           
            oTemInFo.TemplateName = TxtTemplateName.Text;
            oTemInFo.Description = TxtDesc.Text;
            oTemInFo.UserID = user_id;
            oTemInFo.CompanyID = HttpContext.Current.Session["COMPANY_ID"].ToString();

            int isexists = objOrderServices.GetOrderTemplateNameExists(TxtTemplateName.Text, oTemInFo.UserID, oTemInFo.TemplateId);
            if (isexists > 0)
            {
                msgOTAlert.Hide();
                msgAlert.Hide();
                ShowOTAlertMessageBox();
                txterr.Text = "Template Name Already Exists";
                return;
            }


            Template_id = objOrderServices.AddOrderItemTemplate(oTemInFo);


            if (dsOItem != null && dsOItem.Tables.Count > 0 && dsOItem.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                {
                    //bool attrcheck = false;
                    PrdId = objHelperServices.CI(Request.Form["txtPid" + rItem["ORDER_ITEM_ID"].ToString()]);
                    nQty = objHelperServices.CI(Request.Form["txtQty" + rItem["ORDER_ITEM_ID"].ToString()]);

                    if (nQty <= 0)
                    {


                        ShowAlertMessageBox("Invalid Quantity! Quantity Should be Equal/Greater than 1");
                        return;
                    }

                    oItemInFo.ProductID = PrdId;
                  
                    oItemInFo.TemplateId = Template_id;
                    oItemInFo.Quantity = nQty;                    
                    objOrderServices.AddOrderItemTemplateItem(oItemInFo);
                }
            }

            ShowAlertMessageBox("Successfully saved");
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    private void ShowMessageBoxInformation()
    {
        msgPop.ID = "popUp1";
        msgPop.PopupControlID = "MessageboxPanel";
        msgPop.BackgroundCssClass = "modalBackground";
        msgPop.TargetControlID = "updCart";
        msgPop.DropShadow = true;
        msgPop.CancelControlID = "CloseButton";
        this.MessageboxPanel.Controls.Add(msgPop);
        this.msgPop.Show();
    }

    private void ShowAlertMessageBox(string msg)
    {
        lblAlert.Text = msg;
        msgAlert.ID = "DivAlert";
        msgAlert.PopupControlID = "pnlAlert";
        msgAlert.BackgroundCssClass = "modalBackground";
        msgAlert.TargetControlID = "btnHiddenTestPopupExtender";
        msgAlert.DropShadow = true;
        msgAlert.CancelControlID = "btnok";
        this.pnlAlert.Controls.Add(msgAlert);
        this.msgAlert.Show();
    }
    private void ShowOTAlertMessageBox()
    {
        txterr.Text = ""; 
        msgOTAlert.ID = "divOrderTemplate";
        msgOTAlert.PopupControlID = "plnOrderTemplate";
        msgOTAlert.BackgroundCssClass = "modalBackground";
        msgOTAlert.TargetControlID = "btnHiddenTestPopupExtender";
        msgOTAlert.DropShadow = true;
        msgOTAlert.CancelControlID = "btnOTClose";
        this.plnOrderTemplate.Controls.Add(msgOTAlert);
        this.msgOTAlert.Show();
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        string ErrItems = "", ClrItems = "", ClrQty = "";
        int DuplicateItem_Prod_idCount = 0;
        string LeaveDuplicateProds = GetLeaveDuplicateProducts();

        if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
        {
            OrderID = Convert.ToInt32(Session["ORDER_ID"]);

        }
        else
        {
            OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);

        }
        DataSet dsDuplicateItem_Prod_id = objOrderServices.GetOrderItemsWithDuplicate_Prod_id(OrderID, LeaveDuplicateProds);

        if (dsDuplicateItem_Prod_id != null && dsDuplicateItem_Prod_id.Tables.Count > 0 && dsDuplicateItem_Prod_id.Tables[0].Rows.Count > 0)
        {
            DuplicateItem_Prod_idCount = dsDuplicateItem_Prod_id.Tables[0].Rows.Count;
        }

        //if (Session["ITEM_ERROR"] != null || Session["ITEM_CHK"] != null)
        //{
        //    if (!string.IsNullOrEmpty(Session["ITEM_ERROR"].ToString()))
        //        ErrItems = Session["ITEM_ERROR"].ToString().Trim().Replace(",", "");

        //    if (!string.IsNullOrEmpty(Session["ITEM_CHK"].ToString()))
        //        ClrItems = Session["ITEM_CHK"].ToString().Trim().Replace(",", "");

        //    if (!string.IsNullOrEmpty(Session["QTY_CHK"].ToString()))
        //        ClrQty = Session["QTY_CHK"].ToString().Trim().Replace(",", "");

        //}

        DataTable tbErrorItems = objOrderServices.GetOrder_Clarification_Items(OrderID, "");

        //if (ErrItems == "" && ClrItems == "" && DuplicateItem_Prod_idCount == 0)
        if (tbErrorItems == null && DuplicateItem_Prod_idCount == 0)
        {
            if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
            {
                OrderID = Convert.ToInt32(Session["ORDER_ID"]);
                Response.Redirect("Shipping.aspx?OrderId=" + OrderID + "&ApproveOrder=Approve");
            }
            else
            {
                OrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);
                Response.Redirect("Shipping.aspx?OrderId=" + OrderID);
            }

        }
        else
        {
            //ClientScript.RegisterStartupScript(typeof(string), "alert", "alert(\"Please Review Order Clarifications / Errors before proceeding\")", true);
            //ClientScript.RegisterClientScriptBlock(typeof(string), "alert", "alert(\"Please Review Order Clarifications / Errors before proceeding\")", true);

            //string script = "<script type=\"text/javascript\">alert('Please Review Order Clarifications / Errors before proceeding');</script>";
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);

            ShowPopUpMessage();
        }
    }

    private void ShowPopUpMessage()
    {
        modalPop.ID = "popUp";
        modalPop.PopupControlID = "ModalPanel";
        modalPop.BackgroundCssClass = "modalBackground";
        modalPop.TargetControlID = "lblOrderProceed";
        modalPop.DropShadow = true;
        modalPop.CancelControlID = "btnCancel";
        this.ModalPanel.Controls.Add(modalPop);
        this.modalPop.Show();
    }
    protected void btnOrderDetail_Click(object sender, EventArgs e)
    {       
        Response.Redirect("OrderDetails.aspx?&bulkorder=1&pid=0");       
    }
    protected void btnContinueNext_Click(object sender, EventArgs e)
    {
        if (Session["PageUrl"] == null)
        {
            Response.Redirect("OrderDetails.aspx?&bulkorder=1&pid=0");
        }
        else
        {
            if (Session["PageUrl"].ToString().ToLower().Contains("product_list.aspx") ||
               Session["PageUrl"].ToString().ToLower().Contains("product.aspx") ||
               Session["PageUrl"].ToString().ToLower().Contains("productdetails.aspx") ||
               Session["PageUrl"].ToString().ToLower().Contains("powersearch.aspx") ||
               Session["PageUrl"].ToString().ToLower().Contains("home.aspx") ||
               Session["PageUrl"].ToString().ToLower().Contains("family.aspx") ||
               Session["pageUrl"].ToString().ToLower().Contains("compare.aspx") ||
               Session["pageUrl"].ToString().ToLower().Contains("byproduct.aspx") ||
               Session["pageUrl"].ToString().ToLower().Contains("bybrand.aspx") ||
               Session["pageUrl"].ToString().ToLower().Contains("bulkorder.aspx") ||
               Session["pageUrl"].ToString().ToLower().Contains("cataloguedownload.aspx") ||
               Session["pageUrl"].ToString().ToLower().Contains("newsupdate.aspx") ||
                 Session["pageUrl"].ToString().ToLower().Contains("categorylist.aspx")
               )
                Response.Redirect(Session["PageUrl"].ToString());
            else
                Response.Redirect("home.aspx");
        }
    }

    protected void btnRemove_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void btnQuoteRequest_Click(object sender, EventArgs e)
    {
        int OrdID = 0;

        if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ViewOrder"] != null && HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")))
        {
            OrdID = Convert.ToInt32(Session["ORDER_ID"]);
        }
        else
        {
            OrdID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), OpenOrdStatusID);
        }

        int Upt = 0;
        DataSet DSQteId = new DataSet();

        oOrdInfo = objOrderServices.GetOrder(OrdID); //GetOrderDetails(OrdID);
        oQuoteInfo.TotalAmount = oOrdInfo.ProdTotalPrice;
        oQuoteInfo.QuoteStatus = objHelperServices.CI(QuoteServices.QuoteStatus.OPEN);
        oQuoteInfo.ProdTotalPrice = oOrdInfo.ProdTotalPrice;
        oQuoteInfo.TaxAmount = oOrdInfo.TaxAmount;
        oQuoteInfo.UserID = objHelperServices.CI(Session["USER_ID"].ToString());
        objQuoteServices.CreateQuote(oQuoteInfo);
        QDs = objOrderServices.GetOrderItems(OrdID);
        int QID = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()), objHelperServices.CI(QuoteServices.QuoteStatus.OPEN));
        if (QDs != null)
        {
            foreach (DataRow oDr in QDs.Tables[0].Rows)
            {
                oQuoteItemInfo.ProductID = objHelperServices.CI(oDr["PRODUCT_ID"]);
                string Pid = objHelperServices.CS(oQuoteItemInfo.ProductID);
                oQuoteItemInfo.Quantity = objHelperServices.CI(oDr["QTY"]);
                oQuoteItemInfo.PriceApplied = objHelperServices.CDEC(oDr["PRICE_EXT_APPLIED"]);
                oQuoteItemInfo.QuoteID = QID;
                oQuoteItemInfo.UserID = (objHelperServices.CI(Session["USER_ID"].ToString()));
                objQuoteServices.AddQuoteItem(oQuoteItemInfo);
            }
        }
        int Ordstatus = (int)OrderServices.OrderStatus.QUOTEPLACED;
        Upt = objQuoteServices.UpdateQuoteStatus(QID, (int)QuoteServices.QuoteStatus.REQUESTQUOTE);

        if (Upt > 0)
        {
            objOrderServices.UpdateOrderStatus(OrdID, Ordstatus);
            objOrderServices.UpdateQuoteID(OrdID, QID);
            SendQuoteNotification(QID);
            //btnQuoteRequest.Enabled = false;
            //btnQuoteRequestTop.Enabled = false;
        }
        Response.Redirect("QuoteReview.aspx?QteId=" + QID + "&ViewType=REVIEW");
    }
    #endregion
    private decimal GetMyPrice_Exc(int ProductID)
    {
        decimal retval = 0.00M;
        string userid = HttpContext.Current.Session["USER_ID"].ToString();

        retval = objHelperDB.GetProductPrice_Exc(ProductID, 1, userid);
        return retval;
    }
    public string GetLeaveDuplicateProducts()
    {

        try 
        {
        string LeaveDuplicateProds = "";
        if (Session["LeaveDuplicateProds"] != null && Session["LeaveDuplicateProds"].ToString() != "")
        {
            LeaveDuplicateProds = Session["LeaveDuplicateProds"].ToString();
            LeaveDuplicateProds = LeaveDuplicateProds.Replace("-", "");
            if (LeaveDuplicateProds.StartsWith(",") == true)
                LeaveDuplicateProds = LeaveDuplicateProds.Substring(1);

            if (LeaveDuplicateProds.EndsWith(",") == true)
                LeaveDuplicateProds = LeaveDuplicateProds.Substring(0, LeaveDuplicateProds.Length - 1);

        }
        return LeaveDuplicateProds;
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return string.Empty;  
        }
    }

    #region "Notifications"

    public void SendQuoteNotification(int QuoteID)
    {
        //objNotificationServices.NotifyConnection = objConnectionDB.GetConnection();
        if (objNotificationServices.IsNotificationActive(NotificationVariablesServices.NotificationList.REQUESTEDQUOTE.ToString()))
        {
            DataSet dsOrder = objNotificationServices.BuildNotifyInfo();
            HelperServices objHelperServices = new HelperServices();
            string sTemplate = "";
            string sEmailMessage = "";
            string sUser = "";
            sUser = objUserServices.GetUserEmailAdd(objHelperServices.CI(Session["USER_ID"]));
            try
            {
                DataRow oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.QuoteReceipt.FROMCONTENT.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = objHelperServices.GetOptionValues("COMPANY ADDRESS").ToString();
                dsOrder.Tables[0].Rows.Add(oRow);

                oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.QuoteReceipt.FULLNAME.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = objUserServices.GetUserFullName(objHelperServices.CI(Session["USER_ID"]));
                dsOrder.Tables[0].Rows.Add(oRow);

                oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.QuoteReceipt.QUOTEDATE.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = System.DateTime.Now.ToLongDateString();
                dsOrder.Tables[0].Rows.Add(oRow);

                oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.QuoteReceipt.QUOTENO.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = QuoteID.ToString();
                dsOrder.Tables[0].Rows.Add(oRow);

                oRow = dsOrder.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.QuoteReceipt.CONSTRUCTTABLE.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = ConstructQuoteDetails(QuoteID);
                dsOrder.Tables[0].Rows.Add(oRow);

                sTemplate = objNotificationServices.GetTemplateContent(NotificationVariablesServices.NotificationList.REQUESTEDQUOTE.ToString());
                sEmailMessage = objNotificationServices.ParseTemplateMessage(sTemplate, dsOrder);

                objNotificationServices.SMTPServer = objHelperServices.GetOptionValues("MAIL SERVER").ToString();
                objNotificationServices.NotifyTo.Add(sUser);
                objNotificationServices.NotifyFrom = objHelperServices.GetOptionValues("ADMIN EMAIL").ToString();
                objNotificationServices.NotifySubject = objNotificationServices.GetEmailSubject(NotificationVariablesServices.NotificationList.REQUESTEDQUOTE.ToString());
                objNotificationServices.NotifyMessage = sEmailMessage;
                objNotificationServices.UserName = objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString();
                objNotificationServices.Password = objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString();
                objNotificationServices.NotifyIsHTML = objNotificationServices.IsHTMLNotification(NotificationVariablesServices.NotificationList.REQUESTEDQUOTE.ToString());
                objNotificationServices.SendMessage();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
        }
    }

    public string ConstructQuoteDetails(int QuoteID)
    {
        try
        {
            int Qty = 0;
            double Price = 0.0;
            double TotalPrice = 0.0;
            string CatalogItemNo = "";
            string sQuoteDetails = "";
            DataSet dsOD = new DataSet();
            dsOD = objQuoteServices.GetQuoteDetails(QuoteID);
            string currency = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString();
            sQuoteDetails = @"<TABLE BORDER=1><TR><TD><STRONG>Qty</STRONG></TD><TD><STRONG>Item-Description</STRONG></TD><TD><STRONG>Price</STRONG></TD></TR>";
            foreach (DataRow row in dsOD.Tables[0].Rows)
            {
                CatalogItemNo = row["CATALOG_ITEM_NO"].ToString();
                Qty = objHelperServices.CI(row["QTY"]);
                //CUSTOM PRICE
                Price = objHelperServices.CD(row["PRICE_EXT_APPLIED"]) * Qty;
                TotalPrice = TotalPrice + Price;
                sQuoteDetails = sQuoteDetails + @"<TR><TD>" + Qty.ToString() + "</TD><TD>" + CatalogItemNo + "</TD><TD>" + currency + Price.ToString("#,#0.00") + "</TD></TR>";
            }
            sQuoteDetails = sQuoteDetails + @"<tr><td colspan=3 align=right> SubTotal : " + currency + TotalPrice.ToString("#,#0.00") + "</td></tr></TABLE>";
            return sQuoteDetails;
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return null;
        }

    }
    #endregion

}


