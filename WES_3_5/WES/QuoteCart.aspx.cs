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
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.CommonServices;
public partial class QuoteCart : System.Web.UI.Page
{ 
    #region "Declarations"
    HelperServices objHelperServices = new HelperServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    QuoteServices objQuoteServices = new QuoteServices();
    //CustomPrice oCustomPrice = new CustomPrice();
    OrderServices objOrderServices = new OrderServices();
    UserServices objUserServices = new UserServices();
    NotificationServices objNotificationServices = new NotificationServices();
    ConnectionDB objConnectionDB = new ConnectionDB();
    ProductServices objProductServices = new ProductServices();
    QuoteServices.QuoteInfo oQuoteInfo = new QuoteServices.QuoteInfo();

    OrderServices.OrderInfo oOrdInfo = new OrderServices.OrderInfo();
    OrderServices.OrderItemInfo oOrdItemInfo = new OrderServices.OrderItemInfo();
    DataSet QDs = new DataSet();
    int QtyAvail;
    int MinQtyAvail;
    int ProductID = 0;
    int QuoteID = 0;
    int OrderID = 0;
    int QteFlag;
    int QteStatus;    
    int UserId;
    int CatalogID = 0;
    int QuoteStatusID = (int)QuoteServices.QuoteStatus.OPEN;  
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
            Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
            UpdateMsg.Visible = true;
            if (Session["CATALOGID"] != null && Session["CATALOGID"].ToString() != "")
            {
                CatalogID = objHelperServices.CI(Session["CATALOGID"]);
            }
            else
            {
                Session["CATALOGID"] = objHelperServices.CI(objHelperServices.GetOptionValues("DEFAULT CATALOG").ToString());
                CatalogID = objHelperServices.CI(Session["CATALOGID"]);
            }
            if( objHelperServices.CI(Server.HtmlEncode(Request.QueryString["Quote_ID"]))!=0)
                QuoteID = objHelperServices.CI(Server.HtmlEncode(Request.QueryString["Quote_ID"]));
            else if (Session["QuoteID"] != null)
                QuoteID = objHelperServices.CI(Session["QuoteID"]);
            lblQteNo.Text = QuoteID.ToString();
            Session["QuoteID"] = QuoteID;     
            UserId = objHelperServices.CI(Session["USER_ID"].ToString());       
            if (!IsPostBack)
            {
                int ExistingQuote = 0;
                QteFlag = objHelperServices.CI(Server.HtmlEncode(Request.QueryString["QteFlag"]));
                QteStatus = objHelperServices.CI(Server.HtmlEncode(Request.QueryString["QuoteStatus"]));
                string Status = objQuoteServices.GetQuoteStatus(QuoteID);
                if (Status == "QUOTEUPDATEFLAG")
                {
                    btnPlaceOrder.Visible = false;
                    btnPlaceQuote.Visible = true;
                    btnPlaceOrder1.Visible = false;
                    btnPlaceQuote1.Visible = true;
                }
                else
                {
                    btnPlaceOrder.Visible = true;
                    btnPlaceQuote.Visible = false;
                    btnPlaceOrder1.Visible = true;
                    btnPlaceQuote1.Visible = false;
                }
                if (QteFlag == 1)
                {
                    string Flag = "";                    
                    UserId = objHelperServices.CI(Session["USER_ID"].ToString());
                    ExistingQuote = objQuoteServices.GetQuoteID(UserId, objHelperServices.CI(QuoteServices.QuoteStatus.OPEN));
                    if (ExistingQuote != 0)
                        objQuoteServices.UpdateQuoteStatus(ExistingQuote, objHelperServices.CI(QuoteServices.QuoteStatus.RESPONSEQUOTE));
                    if (objQuoteServices.GetQuoteStatus(QuoteID) == "RESPONSEQUOTE")
                    {
                        btnPlaceOrder.Visible = true;
                        btnPlaceQuote.Visible = false;
                        btnPlaceOrder1.Visible = true;
                        btnPlaceQuote1.Visible = false;
                        objQuoteServices.UpdateQuoteStatus(QuoteID, QuoteStatusID);
                    }
                    else
                    {
                        btnPlaceOrder.Visible = false;
                        btnPlaceQuote.Visible = true;
                        btnPlaceOrder1.Visible = false;
                        btnPlaceQuote1.Visible = true;
                        objQuoteServices.UpdateQuoteStatus(QuoteID, QuoteStatusID);
                    }  
                }               
                if (ProductID > 0)
                {
                    ProductID = objHelperServices.CI(Request["Pid"].ToString());
                }
                if (Session["USER_NAME"] == null)
                {
                    Session["USER"] = "";
                    Session["COUNT"] = "0";
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    QtyAvail = objProductServices.GetMaxAvailQty(objHelperServices.CI(Request["Pid"]));
                    MinQtyAvail = objOrderServices.GetProductMinimumOrderQty(objHelperServices.CI(Request["Pid"]));
                    int AvlQty = objOrderServices.GetProductAvilableQty(objHelperServices.CI(Request["Pid"]));                
                   
                    if (QuoteID != 0)
                        AvlQty = objQuoteServices.GetQuoteItemQty(objHelperServices.CI(Request["Pid"]), QuoteID);

                    if (Request["Pid"] != null && Request["Qty"] != null)
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
                        if ((QtyAvail + AvlQty - txtQty) >= 0)

                            if (MinQtyAvail > 0)
                                if (objHelperServices.GetOptionValues("ENABLED RESTRICTED PRODUCT").ToString().ToUpper() == "YES")
                                {
                                    if (objProductServices.GetRestrictedProduct(p).ToString().ToUpper() == "NO")
                                        AddToQuoteTable();
                                }
                                else
                                {
                                    AddToQuoteTable();
                                }
                        if (Session["pageurl"] != null)
                        {
                            Response.Redirect(Session["PageUrl"].ToString(), false);
                        }
                    }
                }
                if (Request["SelAll"] != null)
                {
                    if (Request["SelAll"] == "1")
                    {
                        chkSelectAll.Checked = true;
                    }
                    else if (Request["SellAll"] == "0")
                    {
                        chkSelectAll.Checked = false;
                    }
                }
                else
                {
                    chkSelectAll.Checked = false;
                }
                if (Request["SelPid"] != null)
                {
                    if (Request["SelPid"] != "" && Request["SelPid"] != "AllProd")
                    {
                        if (Session["QuoteID"] != null)
                            QuoteID = objHelperServices.CI(Session["QuoteID"]);
                        else
                           QuoteID = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()), QuoteStatusID);
                        char[] sep = { ',' };
                        string s1 = Request["SelPid"];
                        string s2 = Request["ProdPrice"];
                        decimal SelProdPrice = objHelperServices.CDEC(Request["SelProdPrice"].ToString());
                        string[] cnt1 = new string[30];
                        string[] cnt = new string[30];
                        cnt = s1.Split(sep);
                        cnt1 = s2.Split(sep);
                        int len = cnt.Length;
                        int chk = objQuoteServices.GetQuoteItemCount(QuoteID);
                        if (chk == 1)
                        {
                            btnBotRemove.Visible = false;
                            btnPlaceOrder.Visible = false;
                            btnPlaceQuote.Visible = true;
                            btnBotRemove1.Visible = false;
                            btnPlaceOrder1.Visible = false;
                            btnPlaceQuote1.Visible = true;
                        }
                        else
                        {
                            btnPlaceOrder.Visible = false;
                            btnPlaceQuote.Visible = true;
                            btnBotRemove.Visible = true;
                            btnPlaceOrder1.Visible = false;
                            btnPlaceQuote1.Visible = true;
                            btnBotRemove1.Visible = true;
                        }

                        if (len == chk)
                        {
                            lblRemoveMsg.Visible = true;
                        }
                        else
                        {
                            if (objQuoteServices.GetQuoteItemCount(QuoteID) > 0)
                            {
                                DataSet qDRItems = objQuoteServices.GetQuoteItems(QuoteID);
                                for (int i = 1; i <= len; i++)
                                {
                                    int PrdId = objHelperServices.CI(cnt[i - 1].ToString());
                                    int pQty = objQuoteServices.GetQuoteItemQty(PrdId, QuoteID);
                                    int nQty = objHelperServices.CI(Request.Form["txtQty"] + pQty);
                                    int AvailQty = objOrderServices.GetProductAvilableQty(PrdId);
                                    int n = AvailQty + nQty;
                                    DataRow[] oDR = qDRItems.Tables[0].Select("PRODUCT_ID=" + PrdId);
                                    if (oDR != null && oDR.Length > 0)
                                    {
                                        if (n >= 0)
                                            objOrderServices.UpdateQuantity(PrdId, n);
                                    }
                                    else
                                    {
                                        SelProdPrice = SelProdPrice - objHelperServices.CDEC(cnt1[i - 1].ToString()); 
                                    }
                                }
                                OrderID=objHelperServices.CI(objOrderServices.GetOrderIDForQuote(QuoteID));
                                if (objQuoteServices.RemoveQuoteItem(Request["SelPid"], QuoteID) != -1)
                                {
                                    decimal Tax = CalculateTaxAmount(SelProdPrice);
                                    objQuoteServices.UpdateRemovedItemsPrice(SelProdPrice, QuoteID, Tax);
                                    if (objOrderServices.RemoveItem(Request["SelPid"], OrderID, objHelperServices.CI(Session["USER_ID"]),"") != -1)
                                        objOrderServices.UpdateRemovedItemsPrice(SelProdPrice, OrderID, Tax, 0);
                                    int Qutitems = objQuoteServices.GetQuoteItemCount(QuoteID);
                                    if (Qutitems == 1)
                                    {
                                        btnBotRemove.Visible = false;
                                        btnBotRemove1.Visible = false;
                                    }
                                    else
                                    {
                                        btnBotRemove.Visible = true;
                                        btnBotRemove1.Visible = true;
                                    }

                                }
                                else
                                {
                                    oQuoteInfo.QuoteID = QuoteID;
                                    oQuoteInfo.ProdTotalPrice = 0.00M;
                                    oQuoteInfo.TotalAmount = 0.00M;
                                    oQuoteInfo.TaxAmount = 0.00M;
                                    objQuoteServices.UpdateQuotePrice(oQuoteInfo);

                                }
                            }
                        }
                    }
                    else if (Request["SelPid"] == "AllProd")
                    {
                        if (Session["QuoteID"] != null)
                            QuoteID = objHelperServices.CI(Session["QuoteID"]);
                        else
                            QuoteID = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()), QuoteStatusID);
                        int len = objQuoteServices.GetQuoteItemCount(QuoteID);
                        DataSet ds = new DataSet();
                        ds = objQuoteServices.GetQuoteItems(QuoteID);                        
                        int chk = objQuoteServices.GetQuoteItemCount(QuoteID);
                        if (len == chk)
                        {
                            lblRemoveMsg.Visible = true;
                        }
                        else
                        {
                            if (len > 0)
                            {
                                int cnt = ds.Tables[0].Rows.Count;
                                foreach (DataRow row in ds.Tables[0].Rows)
                                {
                                    int PrdId = objHelperServices.CI(row["PRODUCT_ID"].ToString());
                                    int pQty = objHelperServices.CI(row["QTY"].ToString());
                                    int nQty = objHelperServices.CI(Request.Form["txtQty"] + pQty);
                                    int AvailQty = objOrderServices.GetProductAvilableQty(PrdId);
                                    int n = AvailQty + nQty;
                                    if (n >= 0)
                                        objOrderServices.UpdateQuantity(PrdId, n);
                                }
                                if (objQuoteServices.RemoveQuoteItem(Request["SelPid"], QuoteID) != -1)
                                {
                                    oQuoteInfo.QuoteID = QuoteID;
                                    oQuoteInfo.ProdTotalPrice = 0.00M;
                                    objQuoteServices.UpdateQuotePrice(oQuoteInfo);
                                }
                            }
                        }
                    }
                }

                if (objQuoteServices.GetQuoteItemCount(QuoteID) == 0)
                {                    
                    btnPlaceQuote.Enabled = false;
                    btnBotRemove.Visible = false;
                    btnUpdateCartBot.Enabled = false;
                    btnQuoteCanceled.Enabled = false;
                    btnPlaceOrder.Enabled = false;
                    btnPlaceQuote1.Enabled = false;
                    btnBotRemove1.Visible = false;
                    btnUpdateCartBot1.Enabled = false;
                    btnQuoteCanceled1.Enabled = false;
                    btnPlaceOrder1.Enabled = false;
                }        
                decimal totalcost = objQuoteServices.GetQuoteTotalCost(QuoteID);
                if (totalcost <= 0)
                {
                    btnPlaceOrder.Enabled = false;
                    btnPlaceQuote.Enabled = false;
                    btnPlaceOrder1.Enabled = false;
                    btnPlaceQuote1.Enabled = false;
                }
                else
                {
                    btnPlaceOrder.Enabled = true;
                    btnPlaceQuote.Enabled = true;
                    btnPlaceOrder1.Enabled = true;
                    btnPlaceQuote1.Enabled = true;
                }                
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }

    #region "Add to Quote : Functions.."
    public void AddToQuoteTable()
    {
        try
        {
            int QuoteID = 0;
            string QuoteStatus = "";

            oQuoteInfo.UserID = objHelperServices.CI(Session["USER_ID"]);
            QuoteID = objQuoteServices.GetQuoteID(oQuoteInfo.UserID, QuoteStatusID);
            QuoteStatus = objQuoteServices.GetQuoteStatus(QuoteID);

            if (QuoteID == 0 || QuoteStatus == QuoteServices.QuoteStatus.REQUESTQUOTE.ToString() || QuoteStatus == QuoteServices.QuoteStatus.RESPONSEQUOTE.ToString() || QuoteStatus == QuoteServices.QuoteStatus.CLOSED.ToString() || QuoteStatus == QuoteServices.QuoteStatus.CANCELED.ToString())
            {
                objQuoteServices.InitilizeQuote(oQuoteInfo);
                QuoteID = objQuoteServices.GetQuoteID(oQuoteInfo.UserID, QuoteStatusID);
                AddQuoteItem(QuoteID, oQuoteInfo.UserID);
            }
            else if (QuoteStatus == QuoteServices.QuoteStatus.OPEN.ToString())
            {
                AddQuoteItem(QuoteID, oQuoteInfo.UserID);
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    public void AddQuoteItem(int QuoID, int UsrID)
    {
        try
        {
            ProductPromotionServices objProductPromotionServices = new ProductPromotionServices();
            ProductServices objProductServices = new ProductServices();
            BuyerGroupServices objBuyerGroupServices = new BuyerGroupServices();           
            QuoteServices.QuoteItemInfo qItemInFo = new QuoteServices.QuoteItemInfo();
            decimal untPrice = 0.00M;
            DataSet dsBgPrice = new DataSet();
            DataSet dsBgDisc = new DataSet();
            QuoteServices objQuoteServices = new QuoteServices();
            OrderServices objOrderServices = new OrderServices();
            int chkExistsItem = 0;
            if (Request["Pid"].ToString() != "" && Request["Qty"].ToString() != "undefined")
            {
                ProductID = objHelperServices.CI(Request["Pid"].ToString());
                chkExistsItem = objQuoteServices.GetQuoteItemQty(ProductID, QuoID);
                int ProdQty = objHelperServices.CI(Request["Qty"].ToString());

                if (chkExistsItem == 0 || ProdQty != chkExistsItem)
                {
                    //Check Custom price Option.
                    if (objHelperServices.GetOptionValues("ENABLED CUSTOM PRICE").ToString() == "YES")
                    {
                        //oCustomPrice.Provider = "sqloledb";
                        //oCustomPrice.DataSource = "tbrnd1\\rnd2";
                        //oCustomPrice.InitialCatalog = "T1NEE_V5";
                        //oCustomPrice.DBUserId = "tbadmin";
                        //oCustomPrice.DBPassword = "data2go";
                        //oCustomPrice.CustomAttributeIdColumnName = "ATTRIBUTE_ID";
                        //oCustomPrice.CustomAttributeId = 3;
                        //oCustomPrice.CustomPriceColumnName = "NUMERIC_VALUE";
                        //oCustomPrice.CustomTableName = "TB_PROD_SPECS";
                        //oCustomPrice.CustomProductIdColumnName = "PRODUCT_ID";
                        //oCustomPrice.CustomProductID = ProductID;
                        //untPrice = objHelperServices.CDEC(oCustomPrice.GetCustomPrice());

                    }
                    else
                    {
                        //Chceck the promotion table.
                        if (objProductPromotionServices.CheckPromotion(ProductID))
                        {

                            decimal DiscPrice = objHelperServices.CDEC(objProductPromotionServices.GetProductPromotionDiscValue(ProductID));
                            untPrice = objHelperServices.CDEC(objProductServices.GetProductBasePrice(ProductID));
                            DiscPrice = (untPrice * DiscPrice) / 100;
                            untPrice = untPrice - DiscPrice;
                            untPrice = objHelperServices.CDEC(untPrice.ToString("N2"));

                        }
                        else
                        {
                            //Check the user default buyer group or custome buyer group.
                            int BGPriceID = objBuyerGroupServices.GetBuyerGroupPriceID(UsrID);
                            string BGName = objBuyerGroupServices.GetBuyerGroup(UsrID);
                            if (BGPriceID == 3 && BGName == "DEFAULTBG")
                            {
                                untPrice = objHelperServices.CDEC(objProductServices.GetProductBasePrice(ProductID));
                            }
                            else
                            {
                                dsBgPrice = objProductServices.GetProductPriceValue(ProductID, BGPriceID);
                                if (dsBgPrice != null)
                                {
                                    untPrice = objHelperServices.CDEC(dsBgPrice.Tables[0].Rows[0].ItemArray[1].ToString());

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
                                            bool IsBGCatProd = objBuyerGroupServices.IsBGCatalogProduct(CatalogID, objBuyerGroupServices.GetBuyerGroup(UserId).ToString());
                                            if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
                                            {
                                                untPrice = objBuyerGroupServices.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth);
                                            }
                                        }
                                    }
                                    untPrice = objHelperServices.CDEC(untPrice.ToString("N2"));
                                }
                            }
                        }//Buyergroup price.
                    }

                    qItemInFo.ProductID = objHelperServices.CI(Request["Pid"]);
                    qItemInFo.QuoteID = QuoID;
                    qItemInFo.PriceApplied = untPrice;
                    qItemInFo.UserID = UsrID;
                    if (ProdQty != 0)
                    {
                        int QuoteID = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()));
                        int maxqty = objQuoteServices.GetQuoteItemQty(ProductID, QuoteID);
                        int MinQty = objOrderServices.GetProductMinimumOrderQty(ProductID);
                        int MaxQtyAvl = maxqty + objOrderServices.GetProductAvilableQty(ProductID);
                        qItemInFo.Quantity = objHelperServices.CDEC(ProdQty);
                        int Qty = objHelperServices.CI(qItemInFo.Quantity);
                        ProdQty = MaxQtyAvl - Qty;
                        if (ProdQty >= 0)
                            objOrderServices.UpdateQuantity(ProductID, ProdQty);
                    }
                    else
                    {
                        qItemInFo.Quantity = 1;
                    }
                    if (chkExistsItem == 0)
                    {
                        if (objQuoteServices.AddQuoteItem(qItemInFo) != -1)
                        {
                            DataSet dsOrder = new DataSet();
                            dsOrder = objQuoteServices.GetQuotePriceValues(QuoID);

                            if (dsOrder != null)
                            {
                                decimal ProdTotalPrice;
                                decimal OrderTotal;
                                decimal ExistProdTotal = objHelperServices.CDEC(objQuoteServices.GetCurrentProductTotalCost(QuoID));

                                decimal Tax = CalculateTaxAmount(ExistProdTotal);
                                ProdTotalPrice = ExistProdTotal + (qItemInFo.PriceApplied * qItemInFo.Quantity);
                                //Tax = objHelperServices.CDEC((ProdTotalPrice * Tax) / 100);
                                OrderTotal = ProdTotalPrice; //+Tax;
                                oQuoteInfo.QuoteID = QuoID;
                                oQuoteInfo.ProdTotalPrice = ProdTotalPrice;
                                oQuoteInfo.TaxAmount = Tax;
                                oQuoteInfo.TotalAmount = OrderTotal;
                                objQuoteServices.UpdateQuotePrice(oQuoteInfo);
                            }
                        }
                    }
                    else
                    {                        
                        DataSet dsOrder = new DataSet();
                        dsOrder = objQuoteServices.GetQuotePriceValues(QuoID);

                        if (dsOrder != null)
                        {
                            decimal ProdTotalPrice;
                            decimal OrderTotal;
                            decimal ExistProdTotal = objHelperServices.CDEC(objQuoteServices.GetCurrentProductTotalCost(QuoID));

                            decimal Tax = 0.00M;
                            if (ProdQty >= chkExistsItem)
                            {
                                ProdTotalPrice = (ExistProdTotal + (qItemInFo.PriceApplied * (qItemInFo.Quantity - chkExistsItem)));
                            }
                            else
                            {
                                ProdTotalPrice = (ExistProdTotal - (qItemInFo.PriceApplied * (chkExistsItem - qItemInFo.Quantity)));
                            }
                            OrderTotal = ProdTotalPrice;
                            oQuoteInfo.QuoteID = QuoID;
                            oQuoteInfo.ProdTotalPrice = ProdTotalPrice;
                            oQuoteInfo.TaxAmount = Tax;
                            oQuoteInfo.TotalAmount = OrderTotal;
                            objQuoteServices.UpdateQuotePrice(oQuoteInfo);
                        }
                        objQuoteServices.UpdateQuoteItem(qItemInFo);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    #endregion

    #region "Add to Order : Functions.."
    public void AddOrderItem(DataSet oDs, int OrID, int Uid)
    {
        try
        {
            foreach (DataRow orow in oDs.Tables[0].Rows)
            {
                oOrdItemInfo.ProductID = objHelperServices.CI(orow["PRODUCT_ID"]);
                oOrdItemInfo.Quantity = objHelperServices.CI(orow["QTY"]);
                oOrdItemInfo.PriceApplied = objHelperServices.CDEC(orow["PRICE_APPLIED"]);
                oOrdItemInfo.OrderID = OrID;
                oOrdItemInfo.UserID = Uid;
                objOrderServices.AddOrderItem(oOrdItemInfo);
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    #endregion

    #region "Control Events.."
    protected void btnPlaceOrder_Click(object sender, EventArgs e)
    {
        int OrderID = 0;
        try
        {
            if (Session["USER_NAME"] == null || Session["USER_NAME"].ToString() == string.Empty)
            {
                Response.Redirect("login.aspx");
            }
            else
            {
                if (QteFlag == 1)
                {
                    QuoteID = objHelperServices.CI(Server.HtmlEncode(Request.QueryString["Quote_ID"]));
                }
                else
                {
                    QuoteID = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()), QuoteStatusID);
                }
                if (QuoteID != 0)
                {
                    DataSet oQDs = new DataSet();
                    int upt = 0;
                    oQuoteInfo = objQuoteServices.GetQuote(QuoteID);
                    oOrdInfo.TotalAmount = oQuoteInfo.TotalAmount;
                    oOrdInfo.ProdTotalPrice = oQuoteInfo.ProdTotalPrice;
                    oOrdInfo.TaxAmount = oQuoteInfo.TaxAmount;
                    oOrdInfo.OrderStatus = objHelperServices.CI(OrderServices.OrderStatus.OPEN);
                    int s_oderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"].ToString()), objHelperServices.CI(OrderServices.OrderStatus.OPEN));
                    oOrdInfo.UserID = oQuoteInfo.UserID;
                    oOrdInfo.OrderID = objOrderServices.GetOrderIDForQuote(QuoteID);
                    objOrderServices.UpdateOrderPrice(oOrdInfo, false);
                    int Uid = oOrdInfo.UserID;
                    int OrdStatus = oOrdInfo.OrderStatus;
                    int OrdId = 0;
                    OrdId = objOrderServices.GetOrderIDForQuote(QuoteID);
                    oQDs = objQuoteServices.GetQuoteItems(QuoteID);
                    AddOrderItem(oQDs, OrdId, Uid);
                    oQuoteInfo.QuoteID = QuoteID;
                    objQuoteServices.UpdateQuote(oQuoteInfo);
                    upt = objOrderServices.UpdateOrderStatus(OrdId, objHelperServices.CI(OrderServices.OrderStatus.QUOTEPLACED));
                    if (upt > 0)
                        Response.Redirect("Shipping.aspx?QteFlag=1&QteId=" + QuoteID, false);
                }
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    protected void btnRemoveCart_Click(object sender, EventArgs e)
    {
        try
        {

            if (Request["SelPid"] != null)
            {
                if (Session["QuoteID"] != null)
                    QuoteID = objHelperServices.CI(Session["QuoteID"]);
                else
                    QuoteID = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()), QuoteStatusID);
                Session["QuoteID"] = QuoteID;

                int items = objQuoteServices.GetQuoteItemCount(QuoteID);
                if (items == 1)
                {
                    btnPlaceOrder.Visible = false;
                    btnBotRemove.Visible = false;
                    btnPlaceOrder1.Visible = false;
                    btnBotRemove1.Visible = false;
                }
                else
                {
                    btnPlaceOrder.Visible = false;
                    btnBotRemove.Visible = true;
                    btnPlaceOrder1.Visible = false;
                    btnBotRemove1.Visible = true;
                }
                btnPlaceQuote.Visible = true;
                btnQuoteCanceled.Visible = true;
                btnUpdateCartBot.Visible = true;
                btnPlaceQuote1.Visible = true;
                btnQuoteCanceled1.Visible = true;
                btnUpdateCartBot1.Visible = true;

            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();            
        }
    }
    protected void btnUpdateQuote_Click(object sender, EventArgs e)
    {
        try
        {
            lblRemoveMsg.Visible = false;
            if (Session["QuoteID"] != null)
                QuoteID = objHelperServices.CI(Session["QuoteID"]);
            else
                QuoteID = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()), QuoteStatusID);

            int items = objQuoteServices.GetQuoteItemCount(QuoteID);
            if (items == 1)
            {
                btnBotRemove.Visible = false;
                btnBotRemove1.Visible = false;
            }
            else
            {
                btnBotRemove.Visible = true;
                btnBotRemove1.Visible = true;
            }
            btnPlaceOrder.Visible = false;
            btnUpdateCartBot.Visible = true;
            btnPlaceQuote.Visible = true;
            btnQuoteCanceled.Visible = true;
            btnPlaceOrder1.Visible = false;
            btnUpdateCartBot1.Visible = true;
            btnPlaceQuote1.Visible = true;
            btnQuoteCanceled1.Visible = true;

            QuoteServices.QuoteItemInfo qItemInFo = new QuoteServices.QuoteItemInfo();
            decimal TaxAmt = 0.00M;
            int RowCnt;
            decimal TotalAmt = 0.00M;
            decimal UntPrice;
            decimal ProdTotal = 0.00M;
            decimal tax = 0.00M;
            decimal QuoteTotal = 0.00M;
            int nQty;
            int PrdId;
            string SelAll = "";

            RowCnt = objQuoteServices.GetQuoteItemCount(QuoteID);
            if (RowCnt > 0)
            {
                for (int i = 0; i < RowCnt; i++)
                {
                    PrdId = objHelperServices.CI(Request.Form["txtPid" + i]);
                    nQty = objHelperServices.CI(Request.Form["txtQty" + i]);
                    UntPrice = objHelperServices.CDEC(objProductServices.GetProductBasePrice(PrdId));
                    TotalAmt = UntPrice * nQty;

                    qItemInFo.UserID = objHelperServices.CI(Session["USER_ID"].ToString());
                    qItemInFo.ProductID = PrdId;
                    qItemInFo.Quantity = nQty;
                    qItemInFo.QuoteID = QuoteID;
                    qItemInFo.PriceApplied = UntPrice;

                    ProdTotal = objHelperServices.CDEC(ProdTotal + TotalAmt);
                    int oQty = objQuoteServices.GetQuoteItemQty(PrdId, QuoteID);

                    int AvailQty = objOrderServices.GetProductAvilableQty(PrdId);
                    int n = AvailQty + oQty - nQty;
                    if (n >= 0)
                    {
                        objOrderServices.UpdateQuantity(PrdId, n);
                        objQuoteServices.UpdateQuoteItem(qItemInFo);
                    }
                    nQty = objHelperServices.CI(Request.Form["txtQty" + i]);
                    SelAll = "0";
                }
                TaxAmt = objHelperServices.CDEC(ProdTotal * (tax / 100));
                QuoteTotal = objHelperServices.CDEC(ProdTotal);
                oQuoteInfo.QuoteID = QuoteID;
                oQuoteInfo.TaxAmount = TaxAmt;
                oQuoteInfo.ProdTotalPrice = ProdTotal;
                oQuoteInfo.TotalAmount = QuoteTotal;
                objQuoteServices.UpdateQuotePrice(oQuoteInfo);
                Session["QuoteID"] = QuoteID;
                objQuoteServices.UpdateQuoteStatus(QuoteID, objHelperServices.CI(QuoteServices.QuoteStatus.QUOTEUPDATEFLAG));
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    protected void btnPlaceQuote_Click(object sender, EventArgs e)
    {
        try
        {
            int UpdRst = 0;
            int InsQhistory = 0;
            if (Session["USER_NAME"] != null)
            {
                if (Session["QuoteID"] != null)
                    QuoteID = objHelperServices.CI(Session["QuoteID"]);
                else
                    QuoteID = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()), QuoteStatusID);
                oQuoteInfo.ProdTotalPrice = objQuoteServices.GetCurrentProductTotalCost(QuoteID);
                oQuoteInfo.TotalAmount = objQuoteServices.GetCurrentProductTotalCost(QuoteID);
                oQuoteInfo.UserID = objHelperServices.CI(Session["USER_ID"].ToString());
                oQuoteInfo.QuoteStatus = (int)QuoteServices.QuoteStatus.REQUESTQUOTE;
                oQuoteInfo.QuoteID = QuoteID;
                UpdRst = objQuoteServices.UpdateQuote(oQuoteInfo);
                if (UpdRst > 0)
                {
                    Response.Redirect("QuoteReview.aspx?QteId=" + QuoteID + "&ViewType=REVIEW");
                }
            }
            else
            {
                Response.Redirect("Login.aspx",false);
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }

    }
    protected void btnQuoteCancel_Click(object sender, EventArgs e)
    {
        
        try
        {
            int UpdRst = 0;
            int InsQhistory = 0;
            int QteUpdated = (int)QuoteServices.QuoteStatus.QUOTEUPDATEFLAG;
            string QteStatus = objQuoteServices.GetQuoteStatus(QuoteID);
            if (QteStatus == "OPEN")
            {
                QuoteID = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()), QuoteStatusID);
            }
            else
            {
                QuoteID = objQuoteServices.GetQuoteID(objHelperServices.CI(Session["USER_ID"].ToString()), QteUpdated);
            }
            DataSet ds = new DataSet();
            ds = objQuoteServices.GetQuoteItems(QuoteID);
            if (objQuoteServices.GetQuoteItemCount(QuoteID) > 0)
            {
                foreach (DataRow orow in ds.Tables[0].Rows)
                {
                    int PrdId = objHelperServices.CI(orow["PRODUCT_ID"]);
                    int pQty = objQuoteServices.GetQuoteItemQty(PrdId, QuoteID);
                    int nQty = objHelperServices.CI(Request.Form["txtQty"] + pQty);
                    int AvailQty = objOrderServices.GetProductAvilableQty(PrdId);
                    int n = AvailQty + nQty;
                    if (n >= 0)
                        objOrderServices.UpdateQuantity(PrdId, n);
                }
            }
            UpdRst = objQuoteServices.CancelQuote(QuoteID);
            if (UpdRst > 0)
            {
                Session["QUOTEID"] = QuoteID;
                Response.Redirect("ConfirmMessage.aspx?Result=QTECANCEL");
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }


    }
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
            Response.Redirect("QuoteCart.aspx?SelAll=" + SelAll); 
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }

    }   
    public decimal CalculateTaxAmount(decimal ProdTotalPrice)
    {
        try
        {
            CountryServices objCountryServices = new CountryServices();
            string BillState;
            string BillCountry;
            if (objUserServices.GetTaxExempt(objHelperServices.CI(Session["USER_ID"])) == false)
            {
                BillState = objUserServices.GetUserBillStateCode(objHelperServices.CI(Session["USER_ID"]));
                BillCountry = objUserServices.GetUserBillCountryCode(objHelperServices.CI(Session["USER_ID"]));
                decimal tax = objHelperServices.CDEC(objCountryServices.GetStateTax(BillCountry, BillState));
                decimal RetTax = 0.00M;
                RetTax = objHelperServices.CDEC(objHelperServices.FixDecPlace(ProdTotalPrice * (tax / 100)));
                return RetTax;
            }
            return 0;
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return -1;
        }
    }        
    #endregion
}
















