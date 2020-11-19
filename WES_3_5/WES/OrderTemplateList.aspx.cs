using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.SqlClient;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.CommonServices;
public partial class OrderTemplateList : System.Web.UI.Page
{
    public DataTable oDt;
    //ConnectionDB objConnectionDB = new ConnectionDB();
    //UserServices objUserServices = new UserServices();
    HelperServices objHelperServices = new HelperServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    UserServices objUserServices=new UserServices();
    OrderServices ObjOrderServices = new OrderServices();
    HelperDB objHelperDB = new HelperDB();
    bool _IsShippingFree = false;
    public string FIXED_TAX = System.Configuration.ConfigurationManager.AppSettings["FIXED_TAX"].ToString();
    public string FIXED_TAX_PERCENTAGE = System.Configuration.ConfigurationManager.AppSettings["FIXED_TAX_PERCENTAGE"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
             int TempId=0;
            if (Request["Act"] != null)
            {
                String _Action = Request["Act"].ToString();
                if (_Action == "D")
                {
                    if (Request["TempId"] != null)
                    {
                        TempId = objHelperServices.CI(Request["TempId"].ToString());
                        if (TempId > 0)
                            ObjOrderServices.RemoveOrderTemplate(TempId);
                    }



                }
                else if (_Action == "A")
                {
                    if (Request["TempId"] != null)
                    {
                        AddToCart();
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

    protected void btnNewOrderTemplate_Click(object sender, EventArgs e)
    {
        Response.Redirect("OrderTemplate.aspx");  
    }
    public void AddToCart()
    {
        try
        {
            
            string _notfoundstr = "";
            string _notfoundstrhtml = "";
            //string _minqty = "";
            //string _minqtyhtml = "";
            //string _maxqty = "";
            //string _maxqtyhtml = "";
            string _notfoundqtyhtml = "";
            //bool itemcheck = false;
            string PCodeAll = "";
            DataTable protbl = null;

            decimal ProdTotalPrice = 0;
            //decimal OrderTotal = 0;
            decimal TotalShipCost = 0;
            DataTable dt = null;
            int tmpcount = 0;
            string T_id = "";
            int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
            OrderServices.OrderInfo oOrdInfo = new OrderServices.OrderInfo();

            if (HttpContext.Current.Request.QueryString["Tempid"] != null)
                T_id = HttpContext.Current.Request.QueryString["Tempid"].ToString();
            else
                T_id = "0";

            int userid = objHelperServices.CI(HttpContext.Current.Session["USER_ID"]);

            DataSet ds = new DataSet();
            ds=ObjOrderServices.GetOrderTemplateItem(userid, T_id);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                Response.Redirect("orderTemplateList.aspx", true);
            else
                dt = ds.Tables[0].Copy(); 


            for (int i = 0; i <= dt.Rows.Count-1   ; i++)
            {
                PCodeAll = PCodeAll + "'" + dt.Rows[i]["PCode"].ToString().Trim() + "',";
            }

            if (PCodeAll != "")
            {
                PCodeAll = PCodeAll.Substring(0, PCodeAll.Length - 1) + "";
                protbl = (DataTable)objHelperDB.GetGenericPageDataDB(PCodeAll, "GET_BULKORDER_INVENTORY_COUNT_ALL", HelperDB.ReturnType.RTTable);

            }
            tmpcount = PCodeAll.Split(',').Length - 1;

     

            int OrderID = 0;
            string OrderStatus = "";
            OrderServices.OrderItemInfo oItemInFo = new OrderServices.OrderItemInfo();
            OrderServices.Order_Calrification_ItemInfo oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
            oOrdInfo.UserID = userid;
            //OrderID = objOrderServices.GetOrderID(oOrdInfo.UserID, OpenOrdStatusID);

            if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ViewOrder"] != null && HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")))
            {
                OrderID = Convert.ToInt32(Session["ORDER_ID"]);
            }
            else
            {
                OrderID = ObjOrderServices.GetOrderID(oOrdInfo.UserID, OpenOrdStatusID);
            }

            OrderStatus = ObjOrderServices.GetOrderStatus(OrderID);

            if (OrderID == 0 || OrderStatus == OrderServices.OrderStatus.PAYMENT.ToString() || OrderStatus == OrderServices.OrderStatus.SHIPPED.ToString() || OrderStatus == OrderServices.OrderStatus.COMPLETED.ToString() || OrderStatus == OrderServices.OrderStatus.CANCELED.ToString() || OrderStatus == OrderServices.OrderStatus.ORDERPLACED.ToString() || OrderStatus == OrderServices.OrderStatus.MANUALPROCESS.ToString())
            {
                ObjOrderServices.InitilizeOrder(oOrdInfo);

                if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0))
                {
                    OrderID = Convert.ToInt32(Session["ORDER_ID"]);
                }
                else
                {
                    OrderID = ObjOrderServices.GetOrderID(oOrdInfo.UserID, OpenOrdStatusID);
                }
            }

            decimal ExistProdTotal = objHelperServices.CDEC(ObjOrderServices.GetCurrentProductTotalCost(OrderID));
            decimal ExistShipCostTotal = objHelperServices.CDEC(ObjOrderServices.GetShippingCost(OrderID));
            ProdTotalPrice = ExistProdTotal;
            TotalShipCost = ExistShipCostTotal;
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                int checkset = -1;
                string _pCode = dt.Rows[i]["PCode"].ToString().Trim();
                decimal _pQty = string.IsNullOrEmpty(_pCode.Trim()) ? 0 : Convert.ToDecimal(dt.Rows[i]["QTY"].ToString());

                if (_pCode != "")
                {
                 

                    if (protbl != null && protbl.Rows.Count > 0)
                    {
                        DataRow[] dr = protbl.Select("PCode='" + _pCode + "'");
                        if (dr.Length > 0)
                            checkset = 1;
                        else
                            checkset = 0;

                    }
                    else
                        checkset = 0;

                    if (checkset == 0)
                    {
                        // _notfoundstr += string.Format("{0},", _pCode);

                        string _substitute = FindSubstitute(_pCode);
                        if (_substitute == "{~MI~}")
                        {
                            _notfoundstrhtml += string.Format("{0},", _pCode);
                            _notfoundqtyhtml += string.Format("{0},", _pQty);
                            oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
                            oItemClaItemInfo.Clarification_ID = 0;
                            oItemClaItemInfo.OrderID = OrderID;
                            oItemClaItemInfo.ProductDesc = _pCode;
                            oItemClaItemInfo.Quantity = _pQty;
                            oItemClaItemInfo.UserID = oOrdInfo.UserID;
                            oItemClaItemInfo.Clarification_Type = "ITEM_CHK";
                            ObjOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);
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
                            oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
                            oItemClaItemInfo.Clarification_ID = 0;
                            oItemClaItemInfo.OrderID = OrderID;
                            oItemClaItemInfo.ProductDesc = _pCode;
                            oItemClaItemInfo.Quantity = _pQty;
                            oItemClaItemInfo.UserID = oOrdInfo.UserID;
                            oItemClaItemInfo.Clarification_Type = "ITEM_ERROR";
                            ObjOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);
                        }
                        else
                        {
                            //ProdAItem[i] = _substitute;
                            //_pCode = _substitute;

                            oItemInFo.ProductID = objHelperServices.CI(dt.Rows[i]["PRODUCT_ID"].ToString());
  
                                
                            oItemInFo.OrderID = OrderID;
                            oItemInFo.PriceApplied = GetMyPrice_Exc(oItemInFo.ProductID, objHelperServices.CI(dt.Rows[i]["QTY"].ToString()));

                            oItemInFo.UserID = oOrdInfo.UserID;
                            oItemInFo.Quantity = objHelperServices.CI(dt.Rows[i]["QTY"].ToString());
                            oItemInFo.Ship_Cost = CalculateShippingCost(OrderID, oItemInFo.ProductID, oItemInFo.PriceApplied, objHelperServices.CI(oItemInFo.Quantity));
                            oItemInFo.Tax_Amount = CalculateTaxAmount(oItemInFo.Quantity * oItemInFo.PriceApplied);
                            //int chkExistsItem = objOrderServices.GetOrderItemQty(oItemInFo.ProductID, OrderID);
                            /*if (chkExistsItem > 0)
                            {
                                oItemInFo.Quantity = ObjHelperServices.CI(ProdAQty[i]) + chkExistsItem;
                                objOrderServices.UpdateOrderItem(oItemInFo);
                           }
                            else
                            {*/
                            ObjOrderServices.AddOrderItem(oItemInFo);
                            //}

                        }

                    }
                    if (checkset > 0)
                    {
                        oItemInFo.ORDER_ITEM_ID = 0;
                        oItemInFo.ProductID = objHelperServices.CI(dt.Rows[i]["PRODUCT_ID"].ToString());
                        oItemInFo.OrderID = OrderID;
                        oItemInFo.PriceApplied = GetMyPrice_Exc(oItemInFo.ProductID, objHelperServices.CI(dt.Rows[i]["QTY"].ToString()));
                        oItemInFo.UserID = oOrdInfo.UserID;
                        oItemInFo.Quantity = objHelperServices.CI(dt.Rows[i]["QTY"].ToString());
                        oItemInFo.Ship_Cost = CalculateShippingCost(OrderID, oItemInFo.ProductID, oItemInFo.PriceApplied, objHelperServices.CI(oItemInFo.Quantity));
                        oItemInFo.Tax_Amount = CalculateTaxAmount(oItemInFo.Quantity * oItemInFo.PriceApplied);
                        // int chkExistsItem = objOrderServices.GetOrderItemQty(oItemInFo.ProductID, OrderID);
                        /*if (chkExistsItem > 0)
                        {
                            oItemInFo.Quantity = ObjHelperServices.CI(ProdAQty[i]) + chkExistsItem;
                            objOrderServices.UpdateOrderItem(oItemInFo);
                        }
                        else
                        {*/
                        ObjOrderServices.AddOrderItem(oItemInFo);

                        //}


                        if (OrderID != null)
                        {


                            decimal ProdShippCost = objHelperServices.CDEC(CalculateShippingCost(OrderID, oItemInFo.ProductID, oItemInFo.PriceApplied, objHelperServices.CI(oItemInFo.Quantity)));

                            ProdTotalPrice = ProdTotalPrice + (oItemInFo.PriceApplied * oItemInFo.Quantity);

                            TotalShipCost = TotalShipCost + ProdShippCost;


                           
                        }
                    }


                }

            }

           
            oOrdInfo.OrderID = OrderID;
            ObjOrderServices.UpdateOrderPrice(oOrdInfo, true);

           
            string enc = System.DateTime.Now.Millisecond.ToString();

            if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) )
            {
                Response.Redirect("OrderDetails.aspx?bulkorder=1&Pid=0&ORDER_ID=" + Convert.ToInt32(Session["ORDER_ID"])  , true);
            }
            else
            {
                Response.Redirect("OrderDetails.aspx?bulkorder=1", true);

            }


        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            //objErrorHandler.CreateLog();
        }
    }
    private decimal GetMyPrice_Exc(int ProductID, int qty)
    {
        decimal retval = 0.00M;
        string userid = HttpContext.Current.Session["USER_ID"].ToString();

        retval = objHelperDB.GetProductPrice_Exc(ProductID, qty, userid);
        return retval;
    }
    private string FindSubstitute(string ProductCode)
    {
        string _returnProductCode = "";
        string _returnValue = "{~N/A~}";
        DataTable ObjDatatbl = new DataTable();

        try
        {
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
                                //{
                                //returnProductCode = oDr["PRODUCT_CODE"].ToString();
                                //else
                                // _returnProductCode = "{~MI~}";
                                // }
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
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
        return _returnValue;
    }
    protected decimal CalculateShippingCost(int OID, int ProdId, decimal ProdApplyPrice, int itemQty)
    {
        _IsShippingFree = false;
        DataSet dsOItem = new DataSet();
        decimal ShippingValue = 0;
        decimal ProdShippCost = 0;
        dsOItem = ObjOrderServices.GetItemDetailsFromInventory(ProdId);
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
            if ((ObjOrderServices.GetCurrentProductTotalCost(OID) + ProdApplyPrice) < objHelperServices.CDEC(objHelperServices.GetOptionValues("SHIPPING FREE").ToString()))
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
        return objHelperServices.CDEC(objHelperServices.FixDecPlace(ProdShippCost));
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
                decimal RetTax = 0.00M;
                if (FIXED_TAX.ToUpper() == "TRUE")
                {

                    decimal tax = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
                    RetTax = objHelperServices.CDEC(objHelperServices.FixDecPlace(ProdTotalPrice * (tax / 100)));
                }
                else
                {
                    BillState = objUserServices.GetUserBillStateCode(objHelperServices.CI(Session["USER_ID"]));
                    BillCountry = objUserServices.GetUserBillCountryCode(objHelperServices.CI(Session["USER_ID"]));
                    decimal tax = objHelperServices.CDEC(objCountryServices.GetStateTax(BillCountry, BillState));

                    RetTax = objHelperServices.CDEC(objHelperServices.FixDecPlace(ProdTotalPrice * (tax / 100)));
                }
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
}

