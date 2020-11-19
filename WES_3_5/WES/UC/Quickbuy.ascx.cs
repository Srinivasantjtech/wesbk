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

using System.Data.SqlClient;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UI_QuickOrder : System.Web.UI.UserControl
{
    #region "Object Declaration"

    HelperDB objHelperDB = new HelperDB();

    ErrorHandler objErrorHandler = new ErrorHandler();
    HelperServices objHelperService = new HelperServices();
    OrderServices objOrderServices = new OrderServices();
    UserServices objUserServices = new UserServices();
    ConnectionDB objConnectionDB = new ConnectionDB();
    ProductServices objProductServices = new ProductServices();
    OrderServices.OrderInfo oOrdInfo = new OrderServices.OrderInfo();

    #endregion "Object Declaration"

    #region "Variable Declaration"

    int[] ProdID;// = new int[6];
    int[] ProdQnty;// = new int[6];
    int QtyAvail, MinQtyAvail;
    int ProductID, OrderID, CatalogID = 0;
    string SoldOutProds, TempProdQtys, TempProdItems = "";
    string CPItemCode = "";
    string CPItemQty = "";
    int AvlQty = 0;
    int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
    int txtCount = 6;
    int ItemCnt = 0;
    int i = 0;

    #endregion "Variable Declaration"

    protected void Page_Load(object sender, EventArgs e)
    {
        BtnAddToCart.Enabled = IsEcomenabled();

    }

    public bool IsEcomenabled()
    {
        //bool retvalue = false;
        string userid = HttpContext.Current.Session["USER_ID"].ToString();
        //if (!string.IsNullOrEmpty(userid))
        //{
        //    string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
        //    string sSQL = "SELECT USER_ROLE FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " and USER_ID = " + userid;
        //    oHelper.SQLString = sSQL;
        //    int iROLE = oHelper.CI(oHelper.GetValue("USER_ROLE"));
        //    if (iROLE <= 3)
        //        retvalue = true;
        //}
        return objHelperService.GetIsEcomEnabled(userid);            
        //return retvalue;
    }


    public string cartcount()
    {
        HelperServices objHelperServices = new HelperServices();
        ErrorHandler oErr = new ErrorHandler();
        OrderServices objOrderServices = new OrderServices();
        string cartitem = "0";
        int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
        if (Session["USER_ID"] != null && Session["USER_ID"].ToString() != "")
        {
            int OrderID = 0;

            if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ViewOrder"] != null && HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")))
            {
                OrderID = Convert.ToInt32(Session["ORDER_ID"]);
            }
            else
            {
                OrderID = objOrderServices.GetOrderID(objHelperService.CI(Session["USER_ID"]), OpenOrdStatusID);
            }

            string OrderStatus = objOrderServices.GetOrderStatus(OrderID);
            if (OrderID > 0 && (OrderStatus == OrderServices.OrderStatus.OPEN.ToString() || OrderStatus == "CAU_PENDING"))
            {
                if (objOrderServices.GetOrderItemCount(OrderID) == 0)
                    cartitem = "0";
                else
                    cartitem = objOrderServices.GetOrderItemCount(OrderID).ToString();
            }
            else
            {
                cartitem = "0";
            }
        }
        return cartitem;
    }


    protected void btnAddtoCart_ServerClick(object sender, EventArgs e)
    {
        TempProdQtys = HidQty.Value.ToString();
        TempProdItems = HidItemCode.Value.ToString();
        AddToCartP(TempProdQtys, TempProdItems);
    }

    public void AddToCartP(string TempProdQtys, string TempProdItems)
    {

        try
        {
            string[] ProdAQty = new string[txtCount];
            string[] ProdAItem = new string[txtCount];
            string[] ItemQty = new string[txtCount];
            //ProdID = new int[txtCount];
            //ProdQnty = new int[txtCount];

            ProdAQty = Regex.Split(TempProdQtys, ",");
            ProdAItem = Regex.Split(TempProdItems, ",");
            string _notfoundstr = "";
            string _notfoundstrhtml = "";
            string _minqty = "";
            string _minqtyhtml = "";
            string _maxqty = "";
            string _maxqtyhtml = "";
            bool itemcheck = false;
            string _notfoundqtyhtml = "";

            for (int i = 0; i < ProdAItem.Length; i++)
            {
                DataSet checkset = null;
                string _pCode = ProdAItem[i].ToString().Trim();
                decimal _pQty = string.IsNullOrEmpty(ProdAQty[i].ToString().Trim()) ? 1 : Convert.ToDecimal(ProdAQty[i]);
                //string stquery = "SELECT COUNT(*) FROM tbwc_inventory where product_id=(SELECT TOP(1)product_id FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID=1 AND STRING_VALUE='" + _pCode + "')";
                //checkset = GetDataSet(stquery);
                checkset = (DataSet)objHelperDB.GetGenericPageDataDB(_pCode, "GET_QUICKBY_INVENTORY_COUNT", HelperDB.ReturnType.RTDataSet);   
                if (checkset == null || checkset.Tables[0].Rows == null || checkset.Tables[0].Rows[0][0].ToString() == "0")
                {
                    string _substitute = FindSubstitute(_pCode);
                    if (_substitute == "{~MI~}")
                    {
                        _notfoundstrhtml += string.Format("{0},", _pCode);
                        _notfoundqtyhtml += string.Format("{0},", _pQty);
                    }
                    else if (_substitute == "{~N/A~}")
                    {
                        /*if (_notfoundstr.Length > 0)
                        {

                            if (ProdAItem[i].Length >= 1)
                            {
                                _notfoundstr = _notfoundstr + ProdAItem[i].ToString() + ",.";
                                _notfoundstrhtml = _notfoundstrhtml + "<tr><td class=\"tx_boitem\">" + ProdAItem[i].ToString() + "</td><td align=\"right\" class=\"tx_boitem\">" + ProdAQty[i].ToString() + "</td><td>&nbsp;</td></tr>";
                            }
                        }
                        else
                            if (ProdAItem[i].Length >= 1)
                            {
                                _notfoundstr = ProdAItem[i].ToString() + ",.";
                                _notfoundstrhtml = "<tr><td class=\"tx_boitem\">" + ProdAItem[i].ToString() + "</td><td align=\"right\" class=\"tx_boitem\">" + ProdAQty[i].ToString() + "</td><td>&nbsp;</td></tr>";
                            }*/
                        _notfoundstr += string.Format("{0},", _pCode);
                    }
                    else
                    {
                        ProdAItem[i] = _substitute;
                        _pCode = _substitute;
                    }

                }
                //if (_notfoundstr.Length > 0)
                //    _notfoundstr += ". ";
                //stquery = "SELECT MIN_ORD_QTY,QTY_AVAIL FROM tbwc_inventory where product_id=(SELECT TOP(1)product_id FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID=1 AND STRING_VALUE='" + _pCode + "')";
                //checkset = GetDataSet(stquery);
                checkset = (DataSet)objHelperDB.GetGenericPageDataDB(_pCode, "GET_QUICKBY_INVENTORY_QTY_AVAIL", HelperDB.ReturnType.RTDataSet);   
                if (checkset != null && checkset.Tables[0].Rows != null && checkset.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt32(checkset.Tables[0].Rows[0][0]) > Convert.ToInt32(ProdAQty[i]))
                    {
                        itemcheck = true;
                        if (_minqty.Length > 0)
                        {
                            _minqty += "and Minimum quantity for product \"" + ProdAItem[i].ToString() + "\" : " + checkset.Tables[0].Rows[0][0].ToString();
                            _minqtyhtml += "<tr><td class=\"tx_boitem\">" + ProdAItem[i].ToString() + "</td><td align=\"right\" class=\"tx_boitem\">" + ProdAQty[i].ToString() + "</td><td align=\"left\" class=\"tx_boitem\">&nbsp;&nbsp;&nbsp;Minimum quantity is :" + checkset.Tables[0].Rows[0][0].ToString() + "</td></tr>";
                        }
                        else
                        {
                            _minqty += " Minimum quantity for product \"" + ProdAItem[i].ToString() + "\" : " + checkset.Tables[0].Rows[0][0].ToString();
                            _minqtyhtml += "<tr><td class=\"tx_boitem\">" + ProdAItem[i].ToString() + "</td><td align=\"right\" class=\"tx_boitem\">" + ProdAQty[i].ToString() + "</td><td align=\"left\" class=\"tx_boitem\">&nbsp;&nbsp;&nbsp;Minimum quantity is :" + checkset.Tables[0].Rows[0][0].ToString() + "</td></tr>";

                        }
                    }
                    else if (Convert.ToInt32(checkset.Tables[0].Rows[0][1]) < Convert.ToInt32(ProdAQty[i]))
                    {
                        if (_maxqty.Length > 0)
                        {
                            _maxqty += " and Available quantity for product \"" + ProdAItem[i].ToString() + "\" : " + checkset.Tables[0].Rows[0][1].ToString();
                            _maxqtyhtml += "<tr><td class=\"tx_boitem\">" + ProdAItem[i].ToString() + "</td><td align=\"right\" class=\"tx_boitem\">" + ProdAQty[i].ToString() + "</td><td align=\"left\" class=\"tx_boitem\">&nbsp;&nbsp;&nbsp;Available quantity is :" + checkset.Tables[0].Rows[0][1].ToString() + "</td></tr>";
                        }
                        else
                        {
                            _maxqty += " Available quantity for product \"" + ProdAItem[i].ToString() + "\" : " + checkset.Tables[0].Rows[0][1].ToString();
                            _maxqtyhtml += "<tr><td class=\"tx_boitem\">" + ProdAItem[i].ToString() + "</td><td align=\"right\" class=\"tx_boitem\">" + ProdAQty[i].ToString() + "</td><td align=\"left\" class=\"tx_boitem\">&nbsp;&nbsp;&nbsp;Available quantity is :" + checkset.Tables[0].Rows[0][1].ToString() + "</td></tr>";
                        }

                    }
                }

            }
            //if (_notfoundstr.Length > 0)
            //{
            //    erritemId.Text = "Incorrect Codes Found on Order. Please Check! Incorrect Codes :<br/> " + _notfoundstr;
            //    //Session["ITEM_ERROR"] = "Incorrect Codes Found on Order. Please Check! Incorrect Codes :<br/> " + _notfoundstr;
            //}
            //else
            //{
            //    //Session["ITEM_ERROR"] = "";
            //}

            //if (_minqty.Length > 0)
            //{
            //    erritemId.Text = erritemId.Text +"<br/> " +_minqty;
            //    //Session["ITEM_ERROR"] = erritemId.Text + "<br/> " + _minqty;
            //}
            //if (_maxqty.Length > 0)
            //{
            //    erritemId.Text = erritemId.Text + "<br/> " + _maxqty;
            //    //Session["ITEM_ERROR"] = erritemId.Text + "<br/> " + _maxqty;
            //}
            DataSet oProdIDDS = new DataSet();
            //if (erritemId.Text.Length <= 0)
            {

                oProdIDDS = objProductServices.GetProdIDDS(ProdAItem);
                ProdID = new int[txtCount];

                ProdQnty = new int[txtCount];
                for (int t = 0; t < ProdAItem.Length; t++)
                {
                    if (ProdAItem[t].Length > 0 && ProdAQty[t].Length > 0)
                    {
                        //ProdQnty[t] = oHelper.CI(ProdAQty[t]);
                        ItemQty[t] = ProdAItem[t].ToString().Trim() + "<:>" + objHelperService.CI(ProdAQty[t]);
                    }
                }
                if (Session["USER_NAME"] == null)
                {
                    Session["USER"] = "";
                    Session["COUNT"] = "0";
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    //DataSet oProdIDDS = new DataSet();
                    //oProdIDDS = oProd.GetProdIDDS(ProdAItem);

                    int c = 0, NoofProds = 0, TempCount, TempLoc = 0, tempi = 0;
                    if (oProdIDDS != null)
                    {
                        NoofProds = oProdIDDS.Tables[0].Rows.Count;
                        for (c = 0; c < ItemQty.Length; c++)
                        {
                            if (ItemQty[c] != null)
                                foreach (DataRow oDR in oProdIDDS.Tables[0].Select())
                                {
                                    if (oDR["STRING_VALUE"].ToString().ToLower().Contains(ItemQty[c].ToString().ToLower().Substring(0, (ItemQty[c].IndexOf("<:>")))))
                                    {
                                        ProdID[tempi] = objHelperService.CI(oDR["PRODUCT_ID"].ToString());
                                        tempi++;
                                    }
                                }
                        }
                        foreach (DataRow oDRS in oProdIDDS.Tables[0].Select())
                        {
                            for (c = 0; c < ItemQty.Length; c++)
                            {
                                if (ItemQty[c] != null)
                                    if ((oDRS["STRING_VALUE"].ToString().ToUpper().Contains(ItemQty[c].ToString().ToUpper().Substring(0, (ItemQty[c].IndexOf("<:>"))))) && (oDRS["STRING_VALUE"].ToString().ToUpper().Contains(ProdAItem[c].Trim().ToUpper())) && (ProdAItem[c] != string.Empty))
                                    {
                                        TempCount = objHelperService.CI(ItemQty[c].Substring(ItemQty[c].IndexOf("<:>") + 3).ToString());
                                        ProdQnty[TempLoc] = TempCount;
                                        TempLoc = TempLoc + 1;
                                    }
                            }
                        }
                    }
                    for (i = 0; i < NoofProds; i++)
                    {
                        QtyAvail = objOrderServices.GetProductAvilableQty(ProdID[i]);
                        MinQtyAvail = objOrderServices.GetProductMinimumOrderQty(ProdID[i]);
                        int Ord = 0;

                        if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ViewOrder"] != null && HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")))
                        {
                            Ord = Convert.ToInt32(Session["ORDER_ID"]);
                        }
                        else
                        {
                            Ord = objOrderServices.GetOrderID(objHelperService.CI(Session["USER_ID"]), OpenOrdStatusID);
                        }

                        if (Ord != 0)
                            AvlQty = objOrderServices.GetOrderItemQty(ProdID[i], Ord,0);

                        if (ProdID[i] != null && ProdQnty[i] != null)
                        {
                            int p = ProdID[i];
                            if (QtyAvail == 0 && p > 0)
                            {
                                SoldOutProds = SoldOutProds + p + ",";
                            }
                            int txtQty = ProdQnty[i];
                            if ((QtyAvail + AvlQty - txtQty) >= 0)

                                if (MinQtyAvail > 0 && objProductServices.GetProductAvailability(ProdID[i]) == 1)
                                    if (objHelperService.GetOptionValues("ENABLED RESTRICTED PRODUCT").ToString().ToUpper() == "YES")
                                    {
                                        if (objProductServices.GetRestrictedProduct(p).ToString().ToUpper() == "NO")
                                            AddToOrderTable();
                                    }
                                    else
                                    {
                                        AddToOrderTable();
                                    }
                        }
                    }

                    if (Session["pageurl"] != null)
                    {
                        //if (erritemId.Text.Length <= 0)
                        //{
                        //    Session["ITEM_ERROR"] = "";
                        //    Response.Redirect("OrderDetails.aspx");
                        //}
                        //else
                        //{
                        //    string errmsg = " Incorrect Codes Found on Order. Please Check! Incorrect Codes :" + _notfoundstr + _minqty + _maxqty;
                        //    string errmsghtml = "<table cellpadding=\"0\" width=\"598px\" cellspacing=\"0\" style=\"border:solid 3px #B81212;background-color: #F2F2F2;\"><tr><td colspan=\"3\" class=\"tx_bo\" align=\"left\">ITEM CODES NOT FOUND. PLEASE CHECK. ITEMS CAN BE RE-ENTERED BELOW:</td></tr><tr><td width=\"20%\" class=\"tx_bohead\">ITEM CODE</td><td width=\"10%\" class=\"tx_bohead\" align=\"right\">QTY</td><td width=\"70%\" class=\"tx_bohead\">&nbsp;</td></tr>" + _notfoundstrhtml + _minqtyhtml + _maxqtyhtml + "</table>";
                        //    int stringlen = 80;
                        //    while (errmsg.Length > stringlen)
                        //    {
                        //        errmsg = errmsg.Insert(stringlen, "<br/>");
                        //        stringlen = stringlen + 80;
                        //    }
                        //    erritemId.Text = errmsg;

                        //    Session["ITEM_ERROR"] = errmsghtml;
                        //    Response.Redirect("OrderDetails.aspx?&bulkorder=1");
                        //}
                        //  Response.Redirect(Session["PageUrl"].ToString(), false);
                        //txtCopyPaste.Value = "";


                        //string[] _NotAvailableItems;
                        //string TempAvailableItem = "";

                        if (Session["ITEM_ERROR"] == null)
                        {
                            Session["ITEM_ERROR"] = _notfoundstr;
                        }
                        else
                        {
                            Session["ITEM_ERROR"] = Session["ITEM_ERROR"] + _notfoundstr;

                            //_NotAvailableItems = Session["ITEM_ERROR"].ToString().Split(',');
                            //Session["ITEM_ERROR"] = "";

                            //foreach (string _NotAvailableItem in _NotAvailableItems)
                            //{
                            //    if (_NotAvailableItem.Trim() != "" && _NotAvailableItem.Trim() != TempAvailableItem.Trim())
                            //    {
                            //        Session["ITEM_ERROR"] = Session["ITEM_ERROR"] + _NotAvailableItem + ",";
                            //    }

                            //    TempAvailableItem = _NotAvailableItem;
                            //}

                        }

                        //string[] _ClarifyItems;
                        //string TempClarifyItem = "";

                        if (Session["ITEM_CHK"] == null)
                        {
                            Session["ITEM_CHK"] = _notfoundstrhtml;
                        }
                        else
                        {
                            Session["ITEM_CHK"] = Session["ITEM_CHK"] + _notfoundstrhtml;

                            //_ClarifyItems = Session["ITEM_CHK"].ToString().Split(',');
                            //Session["ITEM_CHK"] = "";

                            //foreach (string _NotClarifyItem in _ClarifyItems)
                            //{
                            //    if (_NotClarifyItem.Trim() != "" && _NotClarifyItem.Trim() != TempAvailableItem.Trim())
                            //    {
                            //        Session["ITEM_CHK"] = Session["ITEM_CHK"] + _NotClarifyItem + ",";
                            //    }

                            //    TempClarifyItem = _NotClarifyItem;
                            //}

                        }

                        if (_notfoundstrhtml.Trim() != "")
                        {
                            if (Session["QTY_CHK"] == null || Session["QTY_CHK"].ToString().Trim() == "")
                            {
                                Session["QTY_CHK"] = _notfoundqtyhtml;
                            }
                            else
                            {
                                Session["QTY_CHK"] = Session["QTY_CHK"] + _notfoundqtyhtml;
                            }
                        }
                        else
                        {
                            if (Session["QTY_CHK"] == null || Session["QTY_CHK"].ToString().Trim() == "")
                            {
                                Session["QTY_CHK"] = "";
                            }
                        }

                        if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ViewOrder"] != null && HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")))
                        {
                            Response.Redirect("OrderDetails.aspx?&bulkorder=1&ORDER_ID=" + Session["ORDER_ID"], true);
                        }
                        else
                        {
                            Response.Redirect("OrderDetails.aspx?bulkorder=1", true);
                        }

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


    private string FindSubstitute(string ProductCode)
    {
       
        try
        {
        string _returnProductCode = "";
        string _returnValue = "{~N/A~}";
        DataTable ObjDatatbl=new DataTable() ;
        //string sSQL = string.Format("SELECT PRODUCT_CODE, SORTKEY_LEVEL FROM WESTB_PROD_SORTKEY WHERE SORTKEY = '{0}'", ProductCode);
        //oHelper.SQLString = sSQL;
        //DataSet DSSub = oHelper.GetDataSet("STTABLE");
        DataSet DSSub = (DataSet)objHelperDB.GetGenericPageDataDB(ProductCode, "GET_BULKORDER_WESTB_PROD_SORTKEY", HelperDB.ReturnType.RTDataSet);   

        if (DSSub != null)
        {
            DSSub.Tables[0].TableName = "STTABLE"; 
            if (DSSub.Tables.Count > 0)
            {
                foreach (DataRow oDr in DSSub.Tables[0].Rows)
                {
                    int _SortKeyLevel = System.Convert.ToInt32(oDr["SORTKEY_LEVEL"].ToString());
                    switch (_SortKeyLevel)
                    {
                        case 1:
                            _returnProductCode = oDr["PRODUCT_CODE"].ToString();
                            break;
                        case 2:
                            _returnProductCode = oDr["PRODUCT_CODE"].ToString();
                            break;
                        case 5:
                            //oHelper.SQLString = string.Format("SELECT COUNT(*) CNT FROM WESTB_PROD_SORTKEY WHERE SORTKEY='{0}' AND SORTKEY_LEVEL=5", ProductCode);
                            //if (System.Convert.ToInt32(oHelper.GetValue("CNT").ToString()) == 1)
                            //    _returnProductCode = oDr["PRODUCT_CODE"].ToString();
                            //else
                            //    _returnProductCode = "{~MI~}";
                            ObjDatatbl = (DataTable)objHelperDB.GetGenericPageDataDB(ProductCode, "GET_BULKORDER_WESTB_PROD_SORTKEY_COUNT", HelperDB.ReturnType.RTTable);
                                if (ObjDatatbl != null)
                                {
                                    if (Convert.ToInt32(ObjDatatbl.Rows[0]["CNT"].ToString()) == 1)
                                        _returnProductCode = oDr["PRODUCT_CODE"].ToString();
                                    else
                                        _returnProductCode = "{~MI~}";
                                }
                                else
                                {
                                    _returnProductCode = "{~MI~}";
                                }
                            _returnValue = "{~MI~}";
                            break;
                        case 8:
                            _returnProductCode = "{~MI~}";
                            _returnValue = "{~MI~}";
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        if (_returnProductCode != "{~MI~}" && _returnProductCode != "")
        {
            //sSQL = string.Format("SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE PRODUCT_ID = (SELECT PRODUCT_ID FROM TB_PROD_SPECS WHERE STRING_VALUE = '{0}' AND ATTRIBUTE_ID = 450) AND ATTRIBUTE_ID = 1", _returnProductCode);
            //oHelper.SQLString = sSQL;
            //_returnValue = oHelper.GetValue("STRING_VALUE").ToString();
            _returnValue = (string)objHelperDB.GetGenericPageDataDB(_returnProductCode, "GET_BULKORDER_PROD_SPECS", HelperDB.ReturnType.RTString);
        }
        return _returnValue;
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return string.Empty;
        }
    }

    public void AddToOrderTable()
    {
        try
        {
            int OrderID = 0;
            string OrderStatus = "";

            oOrdInfo.UserID = objHelperService.CI(Session["USER_ID"]);

            if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ViewOrder"] != null && HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")))
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
            ProductPromotionServices objProductPromotionServices = new ProductPromotionServices();
            ProductServices objProductServices = new ProductServices();
            BuyerGroupServices objBuyerGroupServices = new BuyerGroupServices();
            OrderServices.OrderItemInfo oItemInFo = new OrderServices.OrderItemInfo();
            decimal untPrice = 0.00M;
            DataSet dsBgPrice = new DataSet();
            DataSet dsBgDisc = new DataSet();
            OrderServices objOrderServices = new OrderServices();
            int chkExistsItem = 0;
            if (ProdID[i].ToString() != "" && ProdQnty[i].ToString() != "undefined")
            {
                ProductID = ProdID[i];
                chkExistsItem = objOrderServices.GetOrderItemQty(ProductID, OrID,0);
                int ProdQty = ProdQnty[i];

                if ((chkExistsItem == 0 || chkExistsItem != -1) && ProdQty != -1)
                {
                    bool attrcheck = false;
                    //Chceck the promotion table.
                    if (objProductPromotionServices.CheckPromotion(ProductID))
                    {

                        decimal DiscPrice = objHelperService.CDEC(objProductPromotionServices.GetProductPromotionDiscValue(ProductID));

                        // By Jtech
                        //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", Session["USER_ID"]);
                        //oHelper.SQLString = sSQL;
                        //int pricecode = oHelper.CI(oHelper.GetValue("price_code"));

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
                        //oHelper.SQLString = strquery;
                        //untPrice = Convert.ToDecimal(oHelper.GetValue("Numeric_Value"));
                        string user_id=Session["USER_ID"].ToString() ;

                        untPrice=objHelperDB.GetProductPrice(ProductID,ProdQty,user_id);   

                        //string strquery = "select product_id,numeric_value,attribute_id from tb_prod_specs where product_id=" + ProductID + " and numeric_value>0";
                        //DataSet DSprice = new DataSet();
                        //oHelper.SQLString = strquery;
                        //DSprice = oHelper.GetDataSet();
                        //if (DSprice != null && DSprice.Tables[0].Rows.Count > 0)
                        //{

                        //    foreach (DataRow row in DSprice.Tables[0].Rows)
                        //    {
                        //        if (Convert.ToInt32(row["attribute_id"]) == 5111 && ProdQty >= 100)
                        //        {
                        //            untPrice = oHelper.CDEC(row["numeric_value"]);
                        //        }
                        //        else if (Convert.ToInt32(row["attribute_id"]) == 127 && ProdQty >= 50)
                        //        {
                        //            untPrice = oHelper.CDEC(row["numeric_value"]);
                        //        }
                        //        else if (Convert.ToInt32(row["attribute_id"]) == 40 && ProdQty >= 25)
                        //        {
                        //            untPrice = oHelper.CDEC(row["numeric_value"]);
                        //        }
                        //        else if (Convert.ToInt32(row["attribute_id"]) == 5125 && ProdQty >= 25 && ProdQty <= 49)
                        //        {
                        //            untPrice = oHelper.CDEC(row["numeric_value"]);
                        //        }
                        //        else if (Convert.ToInt32(row["attribute_id"]) == 5280 && ProdQty >= 10 && ProdQty <= 49)
                        //        {
                        //            untPrice = oHelper.CDEC(row["numeric_value"]);
                        //        }
                        //        else if (Convert.ToInt32(row["attribute_id"]) == 39 && ProdQty >= 1 && ProdQty <= 24)
                        //        {
                        //            untPrice = oHelper.CDEC(row["numeric_value"]);
                        //        }
                        //        else if (Convert.ToInt32(row["attribute_id"]) == 35 && ProdQty >= 10)
                        //        {
                        //            untPrice = oHelper.CDEC(row["numeric_value"]);
                        //        }
                        //        else if (Convert.ToInt32(row["attribute_id"]) == 43 && ProdQty >= 1 && ProdQty <= 9)
                        //        {
                        //            untPrice = oHelper.CDEC(row["numeric_value"]);
                        //        }
                        //        else if (Convert.ToInt32(row["attribute_id"]) == 5415 && ProdQty >= 8)
                        //        {
                        //            untPrice = oHelper.CDEC(row["numeric_value"]);
                        //        }
                        //        else if (Convert.ToInt32(row["attribute_id"]) == 5007 && ProdQty >= 5)
                        //        {
                        //            untPrice = oHelper.CDEC(row["numeric_value"]);
                        //        }
                        //        else if (Convert.ToInt32(row["attribute_id"]) == 4 && ProdQty >= 1)
                        //        {
                        //            untPrice = oHelper.CDEC(row["numeric_value"]);
                        //        }
                        //    }
                        //}
                        //else if (untPrice <= 0)
                        //{
                        //    untPrice = oHelper.CDEC(oProd.GetProductBasePrice(ProductID));
                        //}
                        DiscPrice = (untPrice * DiscPrice) / 100;
                        untPrice = untPrice - DiscPrice;
                        untPrice = objHelperService.CDEC(untPrice.ToString("N2"));

                    }
                    else
                    {
                        //Check the user default buyer group or custome buyer group.
                        int BGPriceID = objBuyerGroupServices.GetBuyerGroupPriceID(UsrID);
                        string BGName = objBuyerGroupServices.GetBuyerGroup(UsrID);
                        if (BGPriceID == 3 && BGName == "DEFAULTBG")
                        {
                            //jtech
                            //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", Session["USER_ID"]);
                            //oHelper.SQLString = sSQL;
                            //int pricecode = oHelper.CI(oHelper.GetValue("price_code"));

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
                            //oHelper.SQLString = strquery;
                            //untPrice = Convert.ToDecimal(oHelper.GetValue("Numeric_Value"));
                            //jtech

                             string user_id=Session["USER_ID"].ToString() ;

                             untPrice=objHelperDB.GetProductPrice(ProductID,ProdQty,user_id);   


                            //string strquery = "select product_id,numeric_value,attribute_id from tb_prod_specs where product_id=" + ProductID + " and numeric_value>0";
                            //DataSet DSprice = new DataSet();
                            //oHelper.SQLString = strquery;
                            //DSprice = oHelper.GetDataSet();
                            //if (DSprice != null && DSprice.Tables[0].Rows.Count > 0)
                            //{

                            //    foreach (DataRow row in DSprice.Tables[0].Rows)
                            //    {
                            //        if (Convert.ToInt32(row["attribute_id"]) == 5111 && ProdQty >= 100)
                            //        {
                            //            untPrice = oHelper.CDEC(row["numeric_value"]);
                            //        }
                            //        else if (Convert.ToInt32(row["attribute_id"]) == 127 && ProdQty >= 50)
                            //        {
                            //            untPrice = oHelper.CDEC(row["numeric_value"]);
                            //        }
                            //        else if (Convert.ToInt32(row["attribute_id"]) == 40 && ProdQty >= 25)
                            //        {
                            //            untPrice = oHelper.CDEC(row["numeric_value"]);
                            //        }
                            //        else if (Convert.ToInt32(row["attribute_id"]) == 5125 && ProdQty >= 25 && ProdQty <= 49)
                            //        {
                            //            untPrice = oHelper.CDEC(row["numeric_value"]);
                            //        }
                            //        else if (Convert.ToInt32(row["attribute_id"]) == 5280 && ProdQty >= 10 && ProdQty <= 49)
                            //        {
                            //            untPrice = oHelper.CDEC(row["numeric_value"]);
                            //        }
                            //        else if (Convert.ToInt32(row["attribute_id"]) == 39 && ProdQty >= 1 && ProdQty <= 24)
                            //        {
                            //            untPrice = oHelper.CDEC(row["numeric_value"]);
                            //        }
                            //        else if (Convert.ToInt32(row["attribute_id"]) == 35 && ProdQty >= 10)
                            //        {
                            //            untPrice = oHelper.CDEC(row["numeric_value"]);
                            //        }
                            //        else if (Convert.ToInt32(row["attribute_id"]) == 43 && ProdQty >= 1 && ProdQty <= 9)
                            //        {
                            //            untPrice = oHelper.CDEC(row["numeric_value"]);
                            //        }
                            //        else if (Convert.ToInt32(row["attribute_id"]) == 5415 && ProdQty >= 8)
                            //        {
                            //            untPrice = oHelper.CDEC(row["numeric_value"]);
                            //        }
                            //        else if (Convert.ToInt32(row["attribute_id"]) == 5007 && ProdQty >= 5)
                            //        {
                            //            untPrice = oHelper.CDEC(row["numeric_value"]);
                            //        }
                            //        else if (Convert.ToInt32(row["attribute_id"]) == 4 && ProdQty >= 1)
                            //        {
                            //            untPrice = oHelper.CDEC(row["numeric_value"]);
                            //        }
                            //    }
                            //}
                            //else if (untPrice <= 0)
                            //{
                            //    untPrice = oHelper.CDEC(oProd.GetProductBasePrice(ProductID));
                            //}
                        }
                        else
                        {
                            //Jtech
                            //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", Session["USER_ID"]);
                            //oHelper.SQLString = sSQL;
                            //int pricecode = oHelper.CI(oHelper.GetValue("price_code"));

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
                            //oHelper.SQLString = strquery;
                            //untPrice = Convert.ToDecimal(oHelper.GetValue("Numeric_Value"));
                            //Jtech

                             string user_id=Session["USER_ID"].ToString() ;

                             untPrice=objHelperDB.GetProductPrice(ProductID,ProdQty,user_id);   

                            //    string strquery = "select product_id,numeric_value,attribute_id from tb_prod_specs where product_id=" + ProductID + " and numeric_value>0";
                            //DataSet DSprice = new DataSet();
                            //oHelper.SQLString = strquery;
                            //DSprice = oHelper.GetDataSet();
                            //if (DSprice != null && DSprice.Tables[0].Rows.Count > 0)
                            //{

                            //      foreach (DataRow row in DSprice.Tables[0].Rows)
                            //        {
                            //            if (Convert.ToInt32(row["attribute_id"]) == 5111 && ProdQty >= 100)
                            //            {
                            //                untPrice = oHelper.CDEC(row["numeric_value"]);
                            //            }
                            //            else if (Convert.ToInt32(row["attribute_id"]) == 127 && ProdQty >= 50)
                            //            {
                            //                untPrice = oHelper.CDEC(row["numeric_value"]);
                            //            }
                            //            else if (Convert.ToInt32(row["attribute_id"]) == 40 && ProdQty >= 25)
                            //            {
                            //                untPrice = oHelper.CDEC(row["numeric_value"]);
                            //            }
                            //            else if (Convert.ToInt32(row["attribute_id"]) == 5125 && ProdQty >= 25 && ProdQty <= 49)
                            //            {
                            //                untPrice = oHelper.CDEC(row["numeric_value"]);
                            //            }
                            //            else if (Convert.ToInt32(row["attribute_id"]) == 5280 && ProdQty >= 10 && ProdQty <= 49)
                            //            {
                            //                untPrice = oHelper.CDEC(row["numeric_value"]);
                            //            }
                            //            else if (Convert.ToInt32(row["attribute_id"]) == 39 && ProdQty >= 1 && ProdQty <= 24)
                            //            {
                            //                untPrice = oHelper.CDEC(row["numeric_value"]);
                            //            }
                            //            else if (Convert.ToInt32(row["attribute_id"]) == 35 && ProdQty >= 10)
                            //            {
                            //                untPrice = oHelper.CDEC(row["numeric_value"]);
                            //            }
                            //            else if (Convert.ToInt32(row["attribute_id"]) == 43 && ProdQty >= 1 && ProdQty <=9)
                            //            {
                            //                untPrice = oHelper.CDEC(row["numeric_value"]);
                            //            }
                            //            else if (Convert.ToInt32(row["attribute_id"]) == 5415 && ProdQty >= 8)
                            //            {
                            //                untPrice = oHelper.CDEC(row["numeric_value"]);
                            //            }
                            //            else if (Convert.ToInt32(row["attribute_id"]) == 5007 && ProdQty >= 5)
                            //            {
                            //                untPrice = oHelper.CDEC(row["numeric_value"]);
                            //            }
                            //            else if(Convert.ToInt32(row["attribute_id"]) == 4 && ProdQty >= 1)
                            //            {
                            //                untPrice = oHelper.CDEC(row["numeric_value"]);
                            //            }
                            //        }


                            //    ////To calculate the discount price.
                            //    //dsBgDisc = oBG.GetBuyerGroupBasedDiscountDetails(BGName);
                            //    //if (dsBgDisc != null)
                            //    //{
                            //    //    if (dsBgDisc.Tables[0].Rows.Count > 0)
                            //    //    {
                            //    //        decimal DiscVal = oHelper.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                            //    //        DateTime ValidDt = DateTime.Now.Subtract(System.TimeSpan.FromDays(7));//By default set the  previous date.
                            //    //        if (dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString() != "")
                            //    //        {
                            //    //            ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                            //    //        }
                            //    //        string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                            //    //        bool IsBGCatProd = oBG.IsBGCatalogProduct(CatalogID, oBG.GetBuyerGroup(UsrID).ToString());
                            //    //        if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
                            //    //        {
                            //    //            untPrice = oBG.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth);
                            //    //        }
                            //    //    }
                            //    //}
                            //    untPrice = oHelper.CDEC(untPrice.ToString("N2"));

                            //}
                            //else if (untPrice <= 0)
                            //{
                            //    dsBgPrice = oProd.GetProductPriceValue(ProductID, BGPriceID);
                            //    if (dsBgPrice != null)
                            //    {
                            //        untPrice = oHelper.CDEC(dsBgPrice.Tables[0].Rows[0].ItemArray[1].ToString());

                            //        //To calculate the discount price.
                            //        dsBgDisc = oBG.GetBuyerGroupBasedDiscountDetails(BGName);
                            //        if (dsBgDisc != null)
                            //        {
                            //            if (dsBgDisc.Tables[0].Rows.Count > 0)
                            //            {
                            //                decimal DiscVal = oHelper.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                            //                DateTime ValidDt = DateTime.Now.Subtract(System.TimeSpan.FromDays(7));//By default set the  previous date.
                            //                if (dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString() != "")
                            //                {
                            //                    ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                            //                }
                            //                string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                            //                bool IsBGCatProd = oBG.IsBGCatalogProduct(CatalogID, oBG.GetBuyerGroup(UsrID).ToString());
                            //                if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
                            //                {
                            //                    untPrice = oBG.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth);
                            //                }
                            //            }
                            //        }
                            //        untPrice = oHelper.CDEC(untPrice.ToString("N2"));
                            //    }
                            //}
                        }
                    }//Buyergroup price.
                    oItemInFo.ProductID = ProdID[i];
                    oItemInFo.OrderID = OrID;
                    oItemInFo.PriceApplied = untPrice;
                    oItemInFo.UserID = UsrID;
                    if (ProdQty != 0)
                    {
                        int OrderID = objOrderServices.GetOrderID(objHelperService.CI(Session["USER_ID"].ToString()));
                        int maxqty = objOrderServices.GetOrderItemQty(ProductID, OrderID,0);
                        int MinQty = objOrderServices.GetProductMinimumOrderQty(ProductID);
                        int MaxQtyAvl = maxqty + objOrderServices.GetProductAvilableQty(ProductID);
                        oItemInFo.Quantity = objHelperService .CDEC(ProdQty);
                        int Qty = objHelperService.CI(oItemInFo.Quantity);
                        ProdQty = MaxQtyAvl - Qty;
                        if (ProdQty >= 0)
                            objOrderServices.UpdateQuantity(ProductID, ProdQty);
                    }
                    else
                    {
                        oItemInFo.Quantity = 1;
                    }
                    if (chkExistsItem == 0)
                    {
                        if (objOrderServices.AddOrderItem(oItemInFo) != -1)
                        {
                            DataSet dsOrder = new DataSet();
                            dsOrder = objOrderServices.GetOrderPriceValues(OrID);

                            if (dsOrder != null)
                            {
                                decimal ProdTotalPrice;
                                decimal OrderTotal;

                                decimal ExistProdTotal = objHelperService.CDEC(objOrderServices.GetCurrentProductTotalCost(OrID));
                                ProdTotalPrice = ExistProdTotal + (oItemInFo.PriceApplied * oItemInFo.Quantity);
                                decimal Tax = CalculateTaxAmount(ProdTotalPrice);

                                OrderTotal = ProdTotalPrice + Tax;
                                oOrdInfo.OrderID = OrID;
                                oOrdInfo.ProdTotalPrice = objHelperService.CDEC(objHelperService.FixDecPlace(ProdTotalPrice));
                                oOrdInfo.TaxAmount = objHelperService.CDEC(objHelperService.FixDecPlace(Tax));
                                oOrdInfo.TotalAmount = objHelperService.CDEC(objHelperService.FixDecPlace(OrderTotal));
                                objOrderServices.UpdateOrderPrice(oOrdInfo, true);
                            }
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
                            decimal ExistProdTotal = objHelperService.CDEC(objOrderServices.GetCurrentProductTotalCost(OrID));

                            decimal Tax = 0.00M;
                            if (ProdQty >= chkExistsItem)
                            {
                                ProdTotalPrice = (ExistProdTotal + (oItemInFo.PriceApplied * (oItemInFo.Quantity - chkExistsItem)));
                            }
                            else
                            {
                                ProdTotalPrice = (ExistProdTotal - (oItemInFo.PriceApplied * (chkExistsItem - oItemInFo.Quantity)));
                            }

                            Tax = (ProdTotalPrice * Tax) / 100;
                            OrderTotal = ProdTotalPrice + Tax;

                            oOrdInfo.OrderID = OrID;
                            oOrdInfo.ProdTotalPrice = objHelperService.CDEC(objHelperService.FixDecPlace(ProdTotalPrice));
                            oOrdInfo.TaxAmount = objHelperService.CDEC(objHelperService.FixDecPlace(Tax));
                            oOrdInfo.TotalAmount = objHelperService.CDEC(objHelperService.FixDecPlace(OrderTotal));
                            objOrderServices.UpdateOrderPrice(oOrdInfo, true);

                        }
                        objOrderServices.UpdateOrderItem(oItemInFo);
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

    public decimal CalculateTaxAmount(decimal ProdTotalPrice)
    {
        try
        {
            CountryServices objCountryServices = new CountryServices();
            string BillState;
            string BillCountry;
            if (objUserServices.GetTaxExempt(objHelperService .CI(Session["USER_ID"])) == false)
            {
                BillState = objUserServices.GetUserBillStateCode(objHelperService.CI(Session["USER_ID"]));
                BillCountry = objUserServices.GetUserBillCountryCode(objHelperService.CI(Session["USER_ID"]));
                decimal tax = objHelperService.CDEC(objCountryServices.GetStateTax(BillCountry, BillState));
                decimal RetTax = 0.00M;
                RetTax = objHelperService.CDEC(objHelperService.FixDecPlace(ProdTotalPrice * (tax / 100)));
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

    //public void MsgBox(string Msg)
    //{
    //    string script = "<script>alert('" + Msg + "');</script>";
    //    if (!IsClientScriptBlockRegistered("alert"))
    //    {
    //        this.RegisterClientScriptBlock("alert", script);
    //    }
    //}

    public void AddToCart(string TempProdQtys, string TempProdItems)
    {
        try
        {
            //if (Request.QueryString["txtcnt"] != null && ItemCnt == 0)
            //{
            //    txtCount = oHelper.CI(Request.QueryString["txtcnt"].ToString());
            //}
            //else
            //{
            //    txtCount = ItemCnt;
            //}
            string[] ProdAQty = new string[txtCount];
            string[] ProdAItem = new string[txtCount];
            string[] ItemQty = new string[txtCount];
            ProdID = new int[txtCount];
            ProdQnty = new int[txtCount];

            ProdAQty = Regex.Split(TempProdQtys, ",");
            ProdAItem = Regex.Split(TempProdItems, ",");
            for (int t = 0; t < ProdAQty.Length - 1; t++)
            {
                ProdQnty[t] = objHelperService.CI(ProdAQty[t]);
                ItemQty[t] = ProdAItem[t].ToString().Trim() + "<:>" + ProdQnty[t];
            }
            if (Session["USER_NAME"] == null)
            {
                Session["USER"] = "";
                Session["COUNT"] = "0";
                Response.Redirect("Login.aspx", false);
            }
            else
            {
                DataSet oProdIDDS = new DataSet();
                oProdIDDS = objProductServices.GetProdIDDS(ProdAItem);

                int c = 0, NoofProds = 0, TempCount, TempLoc = 0, tempi = 0;
                if (oProdIDDS != null)
                {
                    NoofProds = oProdIDDS.Tables[0].Rows.Count;

                    for (c = 0; c < ProdAQty.Length - 1; c++)
                    {
                        foreach (DataRow oDR in oProdIDDS.Tables[0].Select())
                        {
                            if (oDR["STRING_VALUE"].ToString().ToLower().Contains(ItemQty[c].ToString().ToLower().Substring(0, (ItemQty[c].IndexOf("<:>")))))
                            {
                                ProdID[tempi] = objHelperService.CI(oDR["PRODUCT_ID"].ToString());
                                tempi++;
                            }
                        }
                    }
                    foreach (DataRow oDRS in oProdIDDS.Tables[0].Select())
                    {
                        for (c = 0; c < ProdAQty.Length - 1; c++)
                        {
                            if ((oDRS["STRING_VALUE"].ToString().ToLower().Contains(ItemQty[c].ToString().ToLower().Substring(0, (ItemQty[c].IndexOf("<:>"))))) && (oDRS["STRING_VALUE"].ToString().Contains(ProdAItem[c])) && (ProdAItem[c] != string.Empty))
                            {
                                TempCount = objHelperService.CI(ItemQty[c].Substring(ItemQty[c].IndexOf("<:>") + 3).ToString());
                                ProdQnty[TempLoc] = TempCount;
                                TempLoc = TempLoc + 1;
                            }
                        }
                    }
                }
                for (i = 0; i < NoofProds; i++)
                {
                    QtyAvail = objOrderServices.GetProductAvilableQty(ProdID[i]);
                    MinQtyAvail = objOrderServices.GetProductMinimumOrderQty(ProdID[i]);
                    int Ord = 0;

                    if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ViewOrder"] != null && HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")))
                    {
                        Ord = Convert.ToInt32(Session["ORDER_ID"]);
                    }
                    else
                    {
                        Ord = objOrderServices.GetOrderID(oOrdInfo.UserID, OpenOrdStatusID);
                    }

                    if (Ord != 0)
                        AvlQty = objOrderServices.GetOrderItemQty(ProdID[i], Ord,0);

                    if (ProdID[i] != null && ProdQnty[i] != null)
                    {
                        int p = ProdID[i];
                        if (QtyAvail == 0 && p > 0)
                        {
                            SoldOutProds = SoldOutProds + p + ",";
                        }
                        int txtQty = ProdQnty[i];
                        //if ((QtyAvail + AvlQty - txtQty) >= 0)

                        if (MinQtyAvail > 0 && objProductServices.GetProductAvailability(ProdID[i]) == 1)
                            if (objHelperService.GetOptionValues("ENABLED RESTRICTED PRODUCT").ToString().ToUpper() == "YES")
                            {
                                if (objProductServices.GetRestrictedProduct(p).ToString().ToUpper() == "NO")
                                    AddToOrderTable();
                            }
                            else
                            {
                                AddToOrderTable();
                            }
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

    public void CPSplitItems(string strItems)
    {
        try
        {
            string[] AItems = new string[50];
            string[] TempItems = new string[50];

            if (strItems.Trim() != string.Empty)
            {
                AItems = Regex.Split(strItems, "\r\n");
                for (int i = 0; i < AItems.Length; i++)
                {
                    if (AItems[i].Contains(",") == true)
                    {
                        TempItems = Regex.Split(AItems[i].ToString(), ",");
                    }
                    else if (AItems[i].Contains("\t") == true)
                    {
                        TempItems = Regex.Split(AItems[i].ToString(), "\t");
                    }
                    if ((TempItems != null) && (TempItems.Length >= 2) && (TempItems[0] != null) && (TempItems[1] != null) && (TempItems[0] != string.Empty) && (TempItems[1] != string.Empty) && (TempItems[0] != "") && (TempItems[1] != ""))
                    {
                        CPItemQty = CPItemQty + TempItems[0].ToString().Trim() + ",";
                        CPItemCode = CPItemCode + TempItems[1].ToString().Trim() + ",";
                        TempItems[0] = "";
                        TempItems[1] = "";
                        ItemCnt++;
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
    //private DataSet GetDataSet(string SQLQuery)
    //{
    //    DataSet ds = new DataSet();
    //    SqlDataAdapter da = new SqlDataAdapter(SQLQuery, oCon.ConnectionString.ToString().Substring(oCon.ConnectionString.ToString().IndexOf(';') + 1));
    //    da.Fill(ds, "generictable");
    //    return ds;
    //}
}
