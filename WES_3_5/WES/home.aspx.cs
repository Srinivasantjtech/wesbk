using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using System.Diagnostics;
using System.Data;

public partial class homepageST : System.Web.UI.Page
{
    AjaxControlToolkit.ModalPopupExtender modalPop = new AjaxControlToolkit.ModalPopupExtender();
    OrderServices objOrderServices = new OrderServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    HelperServices objHelperServices = new HelperServices();
    ProductServices objProductServices = new ProductServices();
       HelperDB objHelperDB = new HelperDB();
    protected void Page_Load(object sender, EventArgs e)
    {
        //Stopwatch sw = new Stopwatch();
        //ErrorHandler objErrorHandler = new ErrorHandler();

        try
        {
        HelperServices objHelperServices = new HelperServices();
        //sw.Start();
        Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
        string sPageExtention = Convert.ToString(objHelperServices.GetOptionValues("PAGE EXTENTION"));
        Session["PageExtention"] = sPageExtention == "-1" ? "" : sPageExtention;
        int cOrderID = 0;

        if (!IsPostBack)
        {
            if (HttpContext.Current.Session["AUTOLOGIN"] != null && HttpContext.Current.Session["AUTOLOGIN"].ToString() == "1")
            {
                OrderServices objOrderServices = new OrderServices();
                int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
                int OrderID = 0;


                cOrderID = objOrderServices.GetOrderID(objHelperServices.CI(Session["USER_ID"]), OpenOrdStatusID);

                if (cOrderID > 0)
                {
                    if (objOrderServices.GetOrderItemCount(cOrderID) > 0)
                    {
                        Session["PrevOrderID"] = cOrderID;
                    }
                    else
                    {
                        Session["PrevOrderID"] = "0";
                    }

                }
                else
                {
                    Session["PrevOrderID"] = "0";
                }
            }
           
            if (HttpContext.Current.Session["CUSTOMER_TYPE"] != null && HttpContext.Current.Session["CUSTOMER_TYPE"].ToString() == "Retailer")
            {
                if (HttpContext.Current.Session["USER_ROLE"] != null && Convert.ToInt32(HttpContext.Current.Session["USER_ROLE"].ToString()) == 4)
                {
                  
                    CallRetailerMsg();
                }
                else
                {
                   
                    CallOrderMsg();
                }
            }
            else
            {
              
                CallOrderMsg();

            }

        }
        
        ContinueOrder.Attributes.Add("onmouseover","javascipt:MouseHover(1);");
        ContinueOrder.Attributes.Add("onmouseout","javascipt:MouseOut(1);");

        ClearOrder.Attributes.Add("onmouseover", "javascipt:MouseHover(2);");
        ClearOrder.Attributes.Add("onmouseout", "javascipt:MouseOut(2);");

        btnCancel.Attributes.Add("onmouseover", "javascipt:MouseHover(3);");
        btnCancel.Attributes.Add("onmouseout", "javascipt:MouseOut(3);");

        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
        //sw.Stop();
        //Console.WriteLine("Elapsed={0}", sw.Elapsed);

        //StackTrace st = new StackTrace();
        //StackFrame sf = st.GetFrame(0);

        //objErrorHandler.ExeTimelog = sf.GetMethod().Name + "," + sw.Elapsed.TotalSeconds.ToString();
        //// objErrorHandler.ExeTimelog = sf.GetMethod().Name + "," + sw.Elapsed;
        //objErrorHandler.CreateTimeLog(); 
    }
    private void CallOrderMsg()
    {
        this.ModalPanel1.Visible = false;
        this.modalPop.Hide();
        modalPop = new AjaxControlToolkit.ModalPopupExtender();
        if (Session["PrevOrderID"] != null && Convert.ToInt32(Session["PrevOrderID"]) > 0)
        {
            ModalPanel.Visible = true;
            modalPop.ID = "popUp";
            modalPop.PopupControlID = "ModalPanel";
            modalPop.BackgroundCssClass = "modalBackground";
            modalPop.DropShadow = false;
            modalPop.TargetControlID = "btnHiddenTestPopupExtender";
            this.ModalPanel.Controls.Add(modalPop);
            this.modalPop.Show();
        }
        else
        {
            this.ModalPanel.Visible = false;            
            this.modalPop.Hide();
        }
    }
    private void CallRetailerMsg()
    {
        this.ModalPanel.Visible = false;
        this.modalPop.Hide();
        modalPop = new AjaxControlToolkit.ModalPopupExtender();
        if (Session["RetailerMsg"] == null)
        {
            Session["RetailerMsg"] = "true";
            ModalPanel1.Visible = true;
            modalPop.ID = "popUp1";
            modalPop.PopupControlID = "ModalPanel1";
            modalPop.BackgroundCssClass = "modalBackground";
            modalPop.DropShadow = false;
            modalPop.TargetControlID = "btnHiddenTestPopupExtender";
            this.ModalPanel1.Controls.Add(modalPop);
            this.modalPop.Show();
        }
        else
        {
            this.ModalPanel1.Visible = false;
            this.modalPop.Hide();
        }
    }
    protected void btnContinueOrder_Click(object sender, EventArgs e)
    {
        try
        {
        if (Session["PrevOrderID"] != null && Convert.ToInt32(Session["PrevOrderID"]) > 0)
        {
            this.modalPop.Hide();
            Session["ORDER_ID"] = Session["PrevOrderID"];
            int OrderId =Convert.ToInt32( Session["ORDER_ID"]);
            Session["PrevOrderID"] = "0";

//Added By indu to get upfdated price and stock in orderItem
            try
            {
                if (objOrderServices.GetOrderItemCount(OrderId) > 0)
                {
                    ErrorHandler objErrorHandler = new ErrorHandler();
                    objErrorHandler.CreateLog("inside continue");
                    DataSet oDSOrderItems = objOrderServices.GetOrderItems(OrderId);
                    decimal TempShipCost = 0;
                    for (int i = 0; i < oDSOrderItems.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            int PrdId = objHelperServices.CI(oDSOrderItems.Tables[0].Rows[i]["product_id"].ToString());
                            double order_item_id = objHelperServices.CD(oDSOrderItems.Tables[0].Rows[i]["order_item_id"].ToString());
                          
                            int pQty = objHelperServices.CI(oDSOrderItems.Tables[0].Rows[i]["QTY"].ToString());

                            objErrorHandler.CreateLog("Remove continue" + PrdId);

                            objOrderServices.RemoveItem(PrdId.ToString(), OrderId, objHelperServices.CI(Session["USER_ID"]), order_item_id.ToString());

                            OrderDetails frmord = new OrderDetails();
                            frmord.NewProduct_id = PrdId;
                            frmord.NewQty = pQty;

                            frmord.AddOrderItem(OrderId, objHelperServices.CI(Session["USER_ID"]));
                        }
                        catch (Exception ex)
                        {

                            objErrorHandler.CreateLog(ex.ToString());
                        }

                    }


                    DataTable tbErrorItem = objOrderServices.GetOrder_Clarification_Items(OrderId, "");
                    if (tbErrorItem != null)
                    {
                        for (int i = 0; i < tbErrorItem.Rows.Count; i++)
                        {
                            try
                            {
                                string product_code = tbErrorItem.Rows[i]["product_desc"].ToString();
                                int PrdId = objHelperServices.CI(objProductServices.GetProductID_code(product_code));
                                double CalItem_ID = objHelperServices.CD(tbErrorItem.Rows[i]["CLARIFICATION_ID"].ToString());

                                int pQty = objHelperServices.CI(tbErrorItem.Rows[i]["QTY"].ToString());

                                objErrorHandler.CreateLog("Remove continue inside order clarification" + PrdId);

                                objOrderServices.Remove_Clarification_item(CalItem_ID);
                                //cA.AddToCart(pQty, PrdId);

                                OrderDetails frmord = new OrderDetails();
                                frmord.NewProduct_id = PrdId;
                                frmord.NewQty = pQty;

                                frmord.AddOrderItem(OrderId, objHelperServices.CI(Session["USER_ID"]));
                            }
                            catch (Exception ex)
                            {

                                objErrorHandler.CreateLog(ex.ToString());
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                objErrorHandler.CreateLog(ex.ToString());
            }
                  
            
            Response.Redirect("OrderDetails.aspx?ORDER_ID=" + Convert.ToInt32(Session["ORDER_ID"]) + "&bulkorder=1&ViewOrder=View", false);
        }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {

            this.modalPop.Hide();
            PopupRetailerLoginMsg.Visible = false;
            
            CallOrderMsg();
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }


    protected void btnClearOrder_Click(object sender, EventArgs e)
    {
        try
        {
        this.modalPop.Hide();
        PopupOrderMsg.Visible = false;
      if (Session["PrevOrderID"] != null && Convert.ToInt32(Session["PrevOrderID"]) > 0)
        {
            OrderServices objOrderServices = new OrderServices();
            objOrderServices.RemoveItem("AllProd", Convert.ToInt32(Session["PrevOrderID"]), Convert.ToInt32(Session["USER_ID"]),"");
            objOrderServices.RemoveOrder(Convert.ToInt32(Session["PrevOrderID"]), Convert.ToInt32(Session["USER_ID"]));

            Session["PrevOrderID"] = "0";
            Session["ORDER_ID"] = "0";
        }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
     
    }

    //public void AddToCart(int Qty, int ProductID, int OrderID)
    //{
    //    try
    //    {
    //        OrderServices.OrderItemInfo oItemInFo = new OrderServices.OrderItemInfo();
    //        OrderServices.Order_Calrification_ItemInfo oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
    //        string _notfoundstr = "";
    //        string _notfoundstrhtml = "";
    //        string _minqty = "";
    //        string _minqtyhtml = "";
    //        string _maxqty = "";
    //        string _maxqtyhtml = "";
    //        string _notfoundqtyhtml = "";
    //        bool itemcheck = false;
    //        string PCodeAll = "";
    //        DataTable protbl = null;
    //        DataTable protblsub = null;

    //        decimal ProdTotalPrice = 0;
    //        decimal OrderTotal = 0;
    //        decimal TotalShipCost = 0;
    //        DataTable dt = null;
    //        int tmpcount = 0;
    //        int StrProductStatus = -1;
    //        int StrProductStatusSub = -1;
    //        string StrStockStatus = "";
    //        int ProdCodeId=0;
    //        string product_status=string.Empty;
    //        int checkset=0;
    //        //bool Prod_Stock_Status_sub = false;
    //        //bool Prod_Stock_Status = false;
           

    //        //if (flgcprowcheck == true)
    //        //{

    //        //    if (protbl == null && flgcprowcheck == true || tmpcount != protbl.Rows.Count && flgcprowcheck == true)
    //        //    {
    //        //        string Script = "alert('Incorrect Code or Invalid QTY');";
    //        //        Page.ClientScript.RegisterStartupScript(this.GetType(), "Myscript", Script, true);
    //        //        return;
    //        //    }
    //        //}


          

    //        decimal ExistProdTotal = objHelperServices.CDEC(objOrderServices.GetCurrentProductTotalCost(OrderID));
    //        decimal ExistShipCostTotal = objHelperServices.CDEC(objOrderServices.GetShippingCost(OrderID));
    //        ProdTotalPrice = ExistProdTotal;
    //        TotalShipCost = ExistShipCostTotal;
                 
    //            protbl = (DataTable)objHelperDB.GetGenericDataDB(ProductID.ToString(), "GET_SINGLE_PRODUCT_INVENTORY_BULKORDER", HelperDB.ReturnType.RTTable);
    //                //str     DataTable objrbl = (DataTable)objHelperDB.GetGenericDataDB(ProductID.ToString(), "GET_SINGLE_PRODUCT_INVENTORY_BULKORDER", HelperDB.ReturnType.RTTable);ing stquery = "SELECT COUNT(*) CNT FROM tbwc_inventory where product_id=(SELECT TOP(1)product_id FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID=1 AND STRING_VALUE='" + _pCode + "')";
    //                //oHelper.SQLString = stquery;
    //                //checkset = oHelper.CI(oHelper.GetValue("CNT")); // GetDataSet(stquery);
    //                //string cnt = (string)objHelperDB.GetGenericPageDataDB(_pCode, "GET_BULKORDER_INVENTORY_COUNT", HelperDB.ReturnType.RTString);
    //                //if (cnt != null && cnt != "")
    //                //checkset = ObjHelperServices.CI(cnt);   
    //                ProdCodeId = 0;
    //                //product_status = "";
    //                StrProductStatusSub = -1;
    //                StrStockStatus = "";
    //                if (protbl != null && protbl.Rows.Count > 0)
    //                {
    //                    DataRow[] dr = protbl.Select("Product_ID='" + ProductID + "'");
    //                    if (dr.Length > 0)
    //                    {
    //                        foreach (DataRow drow in dr)
    //                        {
    //                            if (drow["product_status"].ToString().ToUpper() == "AVAILABLE")
    //                            {
    //                                checkset = 1;
    //                                ProdCodeId = Convert.ToInt32(drow["PRODUCT_ID"].ToString());
    //                                StrStockStatus = drow["STOCK_STATUS"].ToString().Replace("_", " ");
    //                                // objErrorHandler.CreateLog("BULK1" + ProdCodeId);
    //                                StrProductStatusSub = objProductServices.GetStockStatusDesc(StrStockStatus, Convert.ToInt32((drow["PROD_STOCK_STATUS"].ToString())), Convert.ToInt32((drow["PROD_STOCK_FLAG"].ToString())), oOrdInfo.UserID, ProdCodeId);

    //                                //if (objProductServices.GetStockStatusDesc(StrStockStatus, oOrdInfo.UserID) == 1)
    //                                //    StrProductStatusSub = 1;
    //                                //else
    //                                //    StrProductStatusSub = Convert.ToInt32((drow["PROD_STOCK_STATUS"].ToString()));

    //                                product_status = drow["product_status"].ToString();

    //                                break;
    //                            }
    //                        }
    //                        if (product_status == "")
    //                        {
    //                            foreach (DataRow drow in dr)
    //                            {
    //                                if (drow["product_status"].ToString().ToUpper() != "AVAILABLE")
    //                                {
    //                                    checkset = 2;
    //                                    ProdCodeId = Convert.ToInt32(drow["PRODUCT_ID"].ToString());
    //                                    StrStockStatus = drow["STOCK_STATUS"].ToString().Replace("_", " ");
    //                                    // objErrorHandler.CreateLog("BULK2" + ProdCodeId);
    //                                    StrProductStatusSub = objProductServices.GetStockStatusDesc(StrStockStatus, Convert.ToInt32((drow["PROD_STOCK_STATUS"].ToString())), Convert.ToInt32((drow["PROD_STOCK_FLAG"].ToString())), oOrdInfo.UserID, ProdCodeId);
    //                                    //if (objProductServices.GetStockStatusDesc(StrStockStatus, oOrdInfo.UserID) == 1)
    //                                    //    StrProductStatusSub = 1;
    //                                    //else
    //                                    //    StrProductStatusSub = Convert.ToInt32((drow["PROD_STOCK_STATUS"].ToString()));

    //                                    product_status = drow["product_status"].ToString();

    //                                    break;
    //                                }
    //                            }
    //                        }

    //                    }
    //                    else
    //                    {
    //                        checkset = 0;
    //                        ProdCodeId = 0;
    //                        StrProductStatusSub = -1;
    //                        product_status = "";
    //                    }

    //                }
    //                else
    //                {
    //                    checkset = 0;
    //                    ProdCodeId = 0;
    //                    StrProductStatusSub = -1;
    //                    product_status = "";
    //                }


    //                if (checkset == 0)
    //                {
    //                    // _notfoundstr += string.Format("{0},", _pCode);

    //                    string _substitute = FindSubstitute(_pCode);

    //                    if (_substitute != "{~MI~}" && _substitute != "{~N/A~}")
    //                    {
    //                        protblsub = (DataTable)objHelperDB.GetGenericPageDataDB("'" + _substitute + "'", "GET_BULKORDER_INVENTORY_COUNT_ALL", HelperDB.ReturnType.RTTable);

    //                        if (protblsub != null && protblsub.Rows.Count > 0)
    //                        {

    //                            foreach (DataRow drow in protblsub.Rows)
    //                            {
    //                                if (drow["product_status"].ToString().ToUpper() == "AVAILABLE")
    //                                {
    //                                    checksetsub = 1;
    //                                    ProdCodeId = Convert.ToInt32(drow["PRODUCT_ID"].ToString());
    //                                    StrStockStatus = drow["STOCK_STATUS"].ToString().Replace("_", " ");
    //                                    // objErrorHandler.CreateLog("BULK2" + ProdCodeId);
    //                                    StrProductStatusSub = objProductServices.GetStockStatusDesc(StrStockStatus, Convert.ToInt32((drow["PROD_STOCK_STATUS"].ToString())), Convert.ToInt32((drow["PROD_STOCK_FLAG"].ToString())), oOrdInfo.UserID, ProdCodeId);
    //                                    //if (objProductServices.GetStockStatusDesc(StrStockStatus, oOrdInfo.UserID) == 1)
    //                                    //    StrProductStatusSub = 1;
    //                                    //else
    //                                    //    StrProductStatusSub = Convert.ToInt32((drow["PROD_STOCK_STATUS"].ToString()));

    //                                    product_status = drow["product_status"].ToString();

    //                                    break;
    //                                }
    //                            }
    //                            if (product_status == "")
    //                            {
    //                                foreach (DataRow drow in protblsub.Rows)
    //                                {
    //                                    if (drow["product_status"].ToString().ToUpper() != "AVAILABLE")
    //                                    {
    //                                        checksetsub = 2;
    //                                        ProdCodeId = Convert.ToInt32(drow["PRODUCT_ID"].ToString());
    //                                        StrStockStatus = drow["STOCK_STATUS"].ToString().Replace("_", " ");
    //                                        // objErrorHandler.CreateLog("BULK3" + ProdCodeId);
    //                                        StrProductStatusSub = objProductServices.GetStockStatusDesc(StrStockStatus, Convert.ToInt32((drow["PROD_STOCK_STATUS"].ToString())), Convert.ToInt32((drow["PROD_STOCK_FLAG"].ToString())), oOrdInfo.UserID, ProdCodeId);
    //                                        //if (objProductServices.GetStockStatusDesc(StrStockStatus,oOrdInfo.UserID)==1)
    //                                        //    StrProductStatusSub = 1;
    //                                        //else
    //                                        //    StrProductStatusSub = Convert.ToInt32((drow["PROD_STOCK_STATUS"].ToString()));

    //                                        product_status = drow["product_status"].ToString();

    //                                        break;
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }

    //                    if (_substitute == "{~MI~}")
    //                    {
    //                        _notfoundstrhtml += string.Format("{0},", _pCode);
    //                        _notfoundqtyhtml += string.Format("{0},", _pQty);
    //                        oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
    //                        oItemClaItemInfo.Clarification_ID = 0;
    //                        oItemClaItemInfo.OrderID = OrderID;
    //                        oItemClaItemInfo.ProductDesc = _pCode;
    //                        oItemClaItemInfo.Quantity = _pQty;
    //                        oItemClaItemInfo.UserID = oOrdInfo.UserID;
    //                        oItemClaItemInfo.Clarification_Type = "ITEM_CHK";
    //                        objOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);
    //                    }
    //                    else if (_substitute == "{~N/A~}" || checksetsub == 2)
    //                    {

    //                        _notfoundstr += string.Format("{0},", _pCode);
    //                        oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
    //                        oItemClaItemInfo.Clarification_ID = 0;
    //                        oItemClaItemInfo.OrderID = OrderID;
    //                        oItemClaItemInfo.ProductDesc = _pCode;
    //                        oItemClaItemInfo.Quantity = _pQty;
    //                        oItemClaItemInfo.UserID = oOrdInfo.UserID;
    //                        oItemClaItemInfo.Clarification_Type = "ITEM_ERROR";
    //                        objOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);
    //                    }
    //                    else if (checksetsub == 1 && StrProductStatusSub == 0)
    //                    {
    //                        _notfoundstr += string.Format("{0},", _pCode);
    //                        oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
    //                        oItemClaItemInfo.Clarification_ID = 0;
    //                        oItemClaItemInfo.OrderID = OrderID;
    //                        oItemClaItemInfo.ProductDesc = _pCode;
    //                        oItemClaItemInfo.Quantity = _pQty;
    //                        oItemClaItemInfo.UserID = oOrdInfo.UserID;
    //                        oItemClaItemInfo.Clarification_Type = "ITEM_REPLACE";
    //                        objOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);
    //                    }
    //                    else if (checksetsub == 1 && StrProductStatusSub == 1)
    //                    {


    //                        ProdAItem[i] = _substitute;
    //                        _pCode = _substitute;

    //                        oItemInFo.ProductID = ProdCodeId;//GetProductID(ProdAItem[i]);
    //                        oItemInFo.OrderID = OrderID;
    //                        oItemInFo.PriceApplied = GetMyPrice_Exc(oItemInFo.ProductID, ObjHelperServices.CI(ProdAQty[i]));

    //                        oItemInFo.UserID = oOrdInfo.UserID;
    //                        oItemInFo.Quantity = ObjHelperServices.CI(ProdAQty[i]);
    //                        oItemInFo.Ship_Cost = CalculateShippingCost(OrderID, oItemInFo.ProductID, oItemInFo.PriceApplied, ObjHelperServices.CI(oItemInFo.Quantity));
    //                        oItemInFo.Tax_Amount = objOrderServices.CalculateTaxAmount(oItemInFo.Quantity * oItemInFo.PriceApplied, OrderID.ToString(
    //                            ));

    //                        if (oItemInFo.PriceApplied == 0 && objProductServices.GetProductPromotion(oItemInFo.ProductID) == "N")
    //                        {
    //                            _notfoundstr += string.Format("{0},", _pCode);
    //                            oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
    //                            oItemClaItemInfo.Clarification_ID = 0;
    //                            oItemClaItemInfo.OrderID = OrderID;
    //                            oItemClaItemInfo.ProductDesc = _pCode;
    //                            oItemClaItemInfo.Quantity = _pQty;
    //                            oItemClaItemInfo.UserID = oOrdInfo.UserID;
    //                            oItemClaItemInfo.Clarification_Type = "ITEM_PROMOTION";
    //                            objOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);
    //                        }
    //                        else
    //                            objOrderServices.AddOrderItem(oItemInFo);



    //                    }
    //                    else
    //                    {

    //                        _notfoundstr += string.Format("{0},", _pCode);
    //                        oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
    //                        oItemClaItemInfo.Clarification_ID = 0;
    //                        oItemClaItemInfo.OrderID = OrderID;
    //                        oItemClaItemInfo.ProductDesc = _pCode;
    //                        oItemClaItemInfo.Quantity = _pQty;
    //                        oItemClaItemInfo.UserID = oOrdInfo.UserID;
    //                        oItemClaItemInfo.Clarification_Type = "ITEM_ERROR";
    //                        objOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);

    //                    }

    //                }
    //                else if (checkset == 2)
    //                {
    //                    _notfoundstr += string.Format("{0},", _pCode);
    //                    oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
    //                    oItemClaItemInfo.Clarification_ID = 0;
    //                    oItemClaItemInfo.OrderID = OrderID;
    //                    oItemClaItemInfo.ProductDesc = _pCode;
    //                    oItemClaItemInfo.Quantity = _pQty;
    //                    oItemClaItemInfo.UserID = oOrdInfo.UserID;
    //                    oItemClaItemInfo.Clarification_Type = "ITEM_ERROR";
    //                    objOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);
    //                }


    //                if (checkset == 1 && StrProductStatusSub == 0)
    //                {
    //                    _notfoundstr += string.Format("{0},", _pCode);
    //                    oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
    //                    oItemClaItemInfo.Clarification_ID = 0;
    //                    oItemClaItemInfo.OrderID = OrderID;
    //                    oItemClaItemInfo.ProductDesc = _pCode;
    //                    oItemClaItemInfo.Quantity = _pQty;
    //                    oItemClaItemInfo.UserID = oOrdInfo.UserID;
    //                    oItemClaItemInfo.Clarification_Type = "ITEM_REPLACE";
    //                    objOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);
    //                }
    //                else if (checkset == 1 && StrProductStatusSub == 1)
    //                {



    //                    oItemInFo.ORDER_ITEM_ID = 0;
    //                    oItemInFo.ProductID = ProdCodeId;// GetProductID(ProdAItem[i]);
    //                    oItemInFo.OrderID = OrderID;
    //                    oItemInFo.PriceApplied = GetMyPrice_Exc(oItemInFo.ProductID, ObjHelperServices.CI(ProdAQty[i]));
    //                    oItemInFo.UserID = oOrdInfo.UserID;
    //                    oItemInFo.Quantity = ObjHelperServices.CI(ProdAQty[i]);
    //                    oItemInFo.Ship_Cost = CalculateShippingCost(OrderID, oItemInFo.ProductID, oItemInFo.PriceApplied, ObjHelperServices.CI(oItemInFo.Quantity));
    //                    oItemInFo.Tax_Amount = objOrderServices.CalculateTaxAmount(oItemInFo.Quantity * oItemInFo.PriceApplied, OrderID.ToString());

    //                    if (oItemInFo.PriceApplied == 0 && objProductServices.GetProductPromotion(oItemInFo.ProductID) == "N")
    //                    {
    //                        _notfoundstr += string.Format("{0},", _pCode);
    //                        oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
    //                        oItemClaItemInfo.Clarification_ID = 0;
    //                        oItemClaItemInfo.OrderID = OrderID;
    //                        oItemClaItemInfo.ProductDesc = _pCode;
    //                        oItemClaItemInfo.Quantity = _pQty;
    //                        oItemClaItemInfo.UserID = oOrdInfo.UserID;
    //                        oItemClaItemInfo.Clarification_Type = "ITEM_PROMOTION";
    //                        objOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);
    //                    }
    //                    else
    //                        objOrderServices.AddOrderItem(oItemInFo);




    //                    if (OrderID != null)
    //                    {


    //                        decimal ProdShippCost = ObjHelperServices.CDEC(CalculateShippingCost(OrderID, ProductID, oItemInFo.PriceApplied, ObjHelperServices.CI(oItemInFo.Quantity)));

    //                        ProdTotalPrice = ProdTotalPrice + (oItemInFo.PriceApplied * oItemInFo.Quantity);

    //                        TotalShipCost = TotalShipCost + ProdShippCost;


                         
    //                    }

    //                }


    //            }

         

       
    //        oOrdInfo.OrderID = OrderID;
    //        objOrderServices.UpdateOrderPrice(oOrdInfo, true);

         
    //        //string enc = System.DateTime.Now.Millisecond.ToString();

    //        //if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
    //        //{
    //        //    Response.Redirect("OrderDetails.aspx?ORDER_ID=" + Convert.ToInt32(Session["ORDER_ID"]) + "&bulkorder=1&ViewOrder=View&" + enc, true);
    //        //}
    //        //else
    //        //{
    //        //    Response.Redirect("OrderDetails.aspx?bulkorder=1&" + enc, true);

    //        //}


    //    }
    //    catch (Exception ex)
    //    {
    //        objErrorHandler.ErrorMsg = ex;
    //        //objErrorHandler.CreateLog();
    //    }
    //}
}
