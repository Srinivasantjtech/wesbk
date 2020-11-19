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
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.Configuration;
using System.Data.OleDb;
using System.IO;

public partial class UI_BulkOrder : System.Web.UI.UserControl
{
    #region "Object Declaration"

    HelperDB objHelperDB = new HelperDB();
    HelperServices ObjHelperServices = new HelperServices();
    OrderDB objOrderDB = new OrderDB();
    ErrorHandler objErrorHandler = new ErrorHandler();
    OrderServices objOrderServices = new OrderServices();
    UserServices objUserServices = new UserServices();
    //ConnectionDB objConnectionDB = new ConnectionDB();
    ProductServices objProductServices = new ProductServices();
    OrderServices.OrderInfo oOrdInfo = new OrderServices.OrderInfo();


    public string FIXED_TAX = System.Configuration.ConfigurationManager.AppSettings["FIXED_TAX"].ToString();
    public string FIXED_TAX_PERCENTAGE = System.Configuration.ConfigurationManager.AppSettings["FIXED_TAX_PERCENTAGE"].ToString();
    bool _IsShippingFree = false;
    #endregion "Object Declaration"

    #region "Variable Declaration"

    int QtyAvail, MinQtyAvail;
    int ProductID, OrderID, CatalogID = 0;
    string SoldOutProds, TempProdQtys, TempProdItems = "";
    string CPItemCode = "";
    string CPItemQty = "";
    string TProds = "";
    string TQtys = "";
    string BProds = "";
    string BQtys = "";
     
    int AvlQty = 0;
    int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
    int[] ProdID;
    int[] ProdQnty;
    int i = 0;
    int txtCount = 20;
    int ItemCnt = 0;
    string txtcnt;
    string itemcode;
    string itemqty;
    string user_id = HttpContext.Current.Session["USER_ID"].ToString();
    protected int intTxtCount = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["QuickOrderBoxCount"]);
    protected bool flgcprowcheck = false;
    protected bool isSave = false;
    protected DataSet dscodes = null;
    HelperServices objHelperServices = new HelperServices();
    AjaxControlToolkit.ModalPopupExtender msgOTAlert = new AjaxControlToolkit.ModalPopupExtender();
    AjaxControlToolkit.ModalPopupExtender msgAlert = new AjaxControlToolkit.ModalPopupExtender();
    AjaxControlToolkit.ModalPopupExtender ClarifyAlert = new AjaxControlToolkit.ModalPopupExtender();
    string strFile = HttpContext.Current.Server.MapPath("ProdImages");
    #endregion


    protected string GetString(DataSet dstemplate, string f)
    {
        if (dstemplate != null && dstemplate.Tables.Count > 0 && dstemplate.Tables[0].Rows.Count > 0)
        {
            return dstemplate.Tables[0].Rows[0][f].ToString();

        }
        return "";
    }
    protected string GetFilePath(string path)
    {
        string ProdImgPath = "/ProdImages";
        FileInfo Fil = new FileInfo(strFile + path);
        if (Fil.Exists)
        {
            return ProdImgPath+"/" +path.ToString().Replace("\\", "/");

        }
        else
        {
            return ProdImgPath+"/images/noimage.gif";
        }
    }
    protected void IBClose_Click(object sender, EventArgs e)
    {
        ClarifyAlert.Hide();
        divclrify.Visible = false;  
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            msgOTAlert.Hide();
            msgAlert.Hide();
 
            isSave = false;
            //erritemId.Text = "";
            TempProdQtys = "";
            TempProdItems = "";
            TProds = "";
            TQtys = "";
            lnkbtnmore.PostBackUrl = "../" + HttpContext.Current.Request.Url.AbsoluteUri.ToString().Substring(HttpContext.Current.Request.Url.AbsoluteUri.ToString().LastIndexOf('/') + 1);
            if (Request.Form["ctl00$maincontent$ctl00$HidtxtCnt"] != null)
            {
                txtcnt = Request.Form["ctl00$maincontent$ctl00$HidtxtCnt"].ToString();
            }
            else
            {
                if (Request.Form["ctl00$maincontent$BulkOrder1$HidtxtCnt"] != null)
                {
                    txtcnt = Request.Form["ctl00$maincontent$BulkOrder1$HidtxtCnt"].ToString();
                }
                else
                {
                    HidtxtCnt.Value = "10";
                }

            }
            if (txtcnt != null && txtcnt != "")//if (HidtxtCnt.Value != "")
            {
                txtCopyPaste.Rows = ObjHelperServices.CI(txtcnt.ToString()) - 7; //oHelper.CI(Session["TXTCOUNT"].ToString());
            }
            else
            {
                txtCopyPaste.Rows = txtCount - 5;
            }
            if (IsPostBack)
            {
                if (Request.Form["ctl00$maincontent$ctl00$HidItemCode1"] != null && Request.Form["ctl00$maincontent$ctl00$HidItemCode1"].ToString() != "")
                {
                    itemcode = Request.Form["ctl00$maincontent$ctl00$HidItemCode1"].ToString().Substring(0, Request.Form["ctl00$maincontent$ctl00$HidItemCode1"].ToString().Length - 1);
                    itemqty = Request.Form["ctl00$maincontent$ctl00$HidQty1"].ToString().Substring(0, Request.Form["ctl00$maincontent$ctl00$HidQty1"].ToString().Length - 1);

                    CPItemCode = CPItemCode + itemcode + ",";
                    CPItemQty = CPItemQty + itemqty + ",";
                    flgcprowcheck = false;
                }
                if (Request.Form["ctl00$maincontent$BulkOrder1$HidItemCode1"] != null && Request.Form["ctl00$maincontent$BulkOrder1$HidItemCode1"].ToString() != "")
                {
                    itemcode = Request.Form["ctl00$maincontent$BulkOrder1$HidItemCode1"].ToString().Substring(0, Request.Form["ctl00$maincontent$BulkOrder1$HidItemCode1"].ToString().Length - 1);
                    itemqty = Request.Form["ctl00$maincontent$BulkOrder1$HidQty1"].ToString().Substring(0, Request.Form["ctl00$maincontent$BulkOrder1$HidQty1"].ToString().Length - 1);

                    CPItemCode = CPItemCode + itemcode + ",";
                    CPItemQty = CPItemQty + itemqty + ",";
                    flgcprowcheck = false;
                }
                if (txtcnt != null && txtcnt != "")
                {
                    txtCount = ObjHelperServices.CI(txtcnt.ToString());
                }
                TempProdQtys = itemqty;// HidQty1.Value.ToString();
                TempProdItems = itemcode;//HidItemCode1.Value.ToString();
                TQtys = CPItemQty;
                TProds = CPItemCode;
                
                if (Request.Form["ctl00$maincontent$ctl00$txtCopyPaste"] != null && Request.Form["ctl00$maincontent$ctl00$txtCopyPaste"].ToString() != string.Empty)
                {
                    CPSplitItems(Request.Form["ctl00$maincontent$ctl00$txtCopyPaste"].ToString());
                    TempProdQtys = CPItemQty;
                    TempProdItems = CPItemCode;
                    flgcprowcheck = true;
                }
                if (Request.Form["ctl00$maincontent$BulkOrder1$txtCopyPaste"] != null && Request.Form["ctl00$maincontent$BulkOrder1$txtCopyPaste"].ToString() != string.Empty)
                {
                    CPSplitItems(Request.Form["ctl00$maincontent$BulkOrder1$txtCopyPaste"].ToString());
                    TempProdQtys = CPItemQty;
                    TempProdItems = CPItemCode;
                    flgcprowcheck = true;
                }

                if (TempProdItems != string.Empty && TempProdQtys != string.Empty)
                {
                    if (Request.Url.ToString().ToLower().Contains("bulkorder.aspx"))
                    {
                        string CtrlID = string.Empty;
                           if (Request.Form[hidSourceID.UniqueID] != null && Request.Form[hidSourceID.UniqueID] != string.Empty)
                           {
                               CtrlID = Request.Form[hidSourceID.UniqueID];
                          }
                          // if (CtrlID.Contains("btnSaveasTemplate") == false)
                              // AddToCart(TempProdQtys, TempProdItems);

                    }

                    //if (Request.Url.ToString().ToLower().Contains("ordertemplate.aspx"))
                    //    AddToOrderTemplate(TempProdQtys, TempProdItems);
                    //else
                    //{
                    //    string CtrlID = string.Empty;
                    //    if (Request.Form[hidSourceID.UniqueID] != null && Request.Form[hidSourceID.UniqueID] != string.Empty)
                    //    {
                    //        CtrlID = Request.Form[hidSourceID.UniqueID];
                    //    }
                    //    if (CtrlID.Contains("btnSaveasTemplate") == false)
                    //        AddToCart(TempProdQtys, TempProdItems);
                    //}



                }
                else
                {
                    hidSourceID.Value = null;
                }
                HidItemCode1.Value = "";
                HidQty1.Value = "";
                /*                if (erritemId.Text.Length > 0)
                                {
                                    string Script = "alert('Incorrect Codes Found. Please Check order');";
                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Myscript", Script, true);
                                    //Script = this.Attributes.Count.ToString();
                                   // Request.Form.Set("ctl00$maincontent$ctl00$HidItemCode1", "");
                                }*/
            }
            if (Page.IsPostBack == false)
            {
                SetErrorClarify();
              
            }
            //if (Request["Pid"] != null && Request["Qty"] != null && Request["tempid"]!=null)
            //{
            //    if (Request["rma"] != null && Request["cla_id"] != null)
            //    {
            //        if (Request["rma"].ToString() == "NF")
            //        {

            //            objOrderServices.Remove_Clarification_item(objHelperServices.CI(Request["cla_id"].ToString()));
            //        }
            //        if (Request["rma"].ToString() == "CI")
            //        {

            //            objOrderServices.Remove_Clarification_item(objHelperServices.CI(Request["cla_id"].ToString()));
            //        }
            //    }
            //    AddOrderTemplateProductItem(objHelperServices.CI(Request["Pid"].ToString()), objHelperServices.CI(Request["Qty"].ToString()), objHelperServices.CI(Request["tempid"].ToString()));
            //    SetErrorClarify();
            //}

        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            //objErrorHandler.CreateLog();
        }
        HttpContext.Current.Session["fileName"] = null;
        HttpContext.Current.Session["fileData"] = null;
        StatusLabel.Text = "";
        HttpContext.Current.Session["linkmoredata"] = null;
        HttpContext.Current.Session["linkmoredatatxtcount"] = null;

        
    }
    private void SetErrorClarify()
    {
        divclrify.Visible = false;
        if (Request.QueryString["bulkorder"] != null && Request.QueryString["bulkorder"].ToString() == "1")
        {
            int T_id = 0;
            if (HttpContext.Current.Request.QueryString["Tempid"] != null)
                T_id = objHelperServices.CI(HttpContext.Current.Request.QueryString["Tempid"].ToString());
            else
                T_id = 0;

            DataTable tbErrorItem = objOrderServices.GetOrder_Clarification_Items(T_id, "TEMP_ITEM_ERROR");
            DataTable tbErrorChk = objOrderServices.GetOrder_Clarification_Items(T_id, "TEMP_ITEM_CHK");
           
            
            //if (tbErrorItem != null && tbErrorItem.Rows.Count > 0)
            //{
               
                RepeaterError.DataSource = tbErrorItem;
                RepeaterError.DataBind();
            //}
            //if (tbErrorChk != null && tbErrorChk.Rows.Count > 0)
            //{
               
                RepeaterClarify.DataSource = tbErrorChk;
                RepeaterClarify.DataBind();
            //}
                if ((tbErrorItem != null && tbErrorItem.Rows.Count > 0) || (tbErrorChk != null && tbErrorChk.Rows.Count > 0))
                {
                    divclrify.Visible = true;
                }

        }
    }
    protected void RepeaterItemCommand_click(object sender, RepeaterCommandEventArgs e)
    {


        SettextData(TQtys, TProds);
        string[] args=null;
        if (e.CommandName  == "NF")
        {
            if (objHelperServices.CI(e.CommandArgument.ToString()) > 0)            
                objOrderServices.Remove_Clarification_item(objHelperServices.CI(  e.CommandArgument.ToString()));
            SetErrorClarify();
        }
        else if (e.CommandName == "CI")
        {
            if (objHelperServices.CI(e.CommandArgument.ToString()) > 0)  
                objOrderServices.Remove_Clarification_item(objHelperServices.CI(e.CommandArgument.ToString()));
            SetErrorClarify();
        }
        else if (e.CommandName == "ItemCI")
        {
            args = Regex.Split(e.CommandArgument.ToString(),",");
            if (args.Length == 4)
            {
                DataTable dt = (DataTable)objHelperDB.GetGenericPageDataDB(args[2], "GET_SUBPRODUCT_DETAILS", HelperDB.ReturnType.RTTable);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataColumn dc = new DataColumn("CIQty");
                    dc.DefaultValue = args[1];
                    dt.Columns.Add(dc);

                    dc = new DataColumn("TEMP_ID");
                    dc.DefaultValue = args[3];
                    dt.Columns.Add(dc);

                    dc = new DataColumn("CLARIFICATION_ID");
                    dc.DefaultValue = args[0];
                    dt.Columns.Add(dc);
                }
                RepeaterItemClarify.DataSource = dt;
                RepeaterItemClarify.DataBind();
                ShowClarifyAlertMessageBox(args[2]);


            }
        }
        else if (e.CommandName == "CIInsert")
            {
                args = Regex.Split(e.CommandArgument.ToString(), ",");
                 if (args.Length >= 4)
                 {
                    AddOrderTemplateProductItem(objHelperServices.CI(args[0]),objHelperServices.CI(args[1]),objHelperServices.CI(args[2]));
                     if (dscodes!=null && dscodes.Tables.Count>0) 
                     {
                     DataRow dr= dscodes.Tables[0].NewRow();
                         dr["productcode"]=args[4].ToString();
                         dr["quantity"]=args[1].ToString();
                         dscodes.Tables[0].Rows.Add(dr);  
                     }
                    if (objHelperServices.CI(args[3]) > 0)
                        objOrderServices.Remove_Clarification_item(objHelperServices.CI(args[3]));
                    SetErrorClarify();
                    ClarifyAlert.Hide();
 
                 }
            }


            
        
       
        
    }
    public void AddToOrderTable()
    {
        //try
        //{
        //    int OrderID = 0;
        //    string OrderStatus = "";

        //    oOrdInfo.UserID = oHelper.CI(Session["USER_ID"]);
        //    OrderID = oOrder.GetOrderID(oOrdInfo.UserID, OpenOrdStatusID);
        //    OrderStatus = oOrder.GetOrderStatus(OrderID);

        //    if (OrderID == 0 || OrderStatus == OrderServices.OrderStatus.PAYMENT.ToString() || OrderStatus == OrderServices.OrderStatus.SHIPPED.ToString() || OrderStatus == OrderServices.OrderStatus.COMPLETED.ToString() || OrderStatus == OrderServices.OrderStatus.CANCELED.ToString() || OrderStatus == OrderServices.OrderStatus.ORDERPLACED.ToString() || OrderStatus == OrderServices.OrderStatus.MANUALPROCESS.ToString())
        //    {
        //        oOrder.InitilizeOrder(oOrdInfo);
        //        OrderID = oOrder.GetOrderID(oOrdInfo.UserID, OpenOrdStatusID);
        //        AddOrderItem(OrderID, oOrdInfo.UserID);
        //    }
        //    else if (OrderStatus == OrderServices.OrderStatus.OPEN.ToString())
        //    {
        //        AddOrderItem(OrderID, oOrdInfo.UserID);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    oErr.ErrorMsg = ex;
        //    oErr.CreateLog();
        //}
    }

    //public void AddOrderItem(int OrID, int UsrID)
    //{
    //    try
    //    {
    //        TradingBell.WebServices.ProductPromotion oPP = new TradingBell.WebServices.ProductPromotion();
    //        Product oProd = new Product();
    //        TradingBell.WebServices.BuyerGroup oBG = new TradingBell.WebServices.BuyerGroup();
    //        OrderServices.OrderItemInfo oItemInFo = new OrderServices.OrderItemInfo();
    //        decimal untPrice = 0.00M;
    //        DataSet dsBgPrice = new DataSet();
    //        DataSet dsBgDisc = new DataSet();
    //        Order oOrder = new Order();
    //        int chkExistsItem = 0;
    //        if (ProdID[i].ToString() != "" && ProdQnty[i].ToString() != "undefined")
    //        {
    //            ProductID = ProdID[i];
    //            chkExistsItem = oOrder.GetOrderItemQty(ProductID, OrID);
    //            int ProdQty = ProdQnty[i];

    //            if ((chkExistsItem == 0 || ProdQty != chkExistsItem) && ProdQty != -1)
    //            {
    //                bool attrcheck = false;
    //                //Chceck the promotion table.
    //                if (oPP.CheckPromotion(ProductID))
    //                {

    //                    decimal DiscPrice = oHelper.CDEC(oPP.GetProductPromotionDiscValue(ProductID));
    //                    string strquery = "select product_id,numeric_value,attribute_id from tb_prod_specs where product_id=" + ProductID + " and numeric_value>0";
    //                    DataSet DSprice = new DataSet();
    //                    oHelper.SQLString = strquery;
    //                    DSprice = oHelper.GetDataSet();
    //                    if (DSprice != null && DSprice.Tables[0].Rows.Count > 0)
    //                    {

    //                        foreach (DataRow row in DSprice.Tables[0].Rows)
    //                        {
    //                            if (Convert.ToInt32(row["attribute_id"]) == 5111 && ProdQty >= 100)
    //                            {
    //                                untPrice = oHelper.CDEC(row["numeric_value"]);
    //                            }
    //                            else if (Convert.ToInt32(row["attribute_id"]) == 127 && ProdQty >= 50)
    //                            {
    //                                untPrice = oHelper.CDEC(row["numeric_value"]);
    //                            }
    //                            else if (Convert.ToInt32(row["attribute_id"]) == 40 && ProdQty >= 25)
    //                            {
    //                                untPrice = oHelper.CDEC(row["numeric_value"]);
    //                            }
    //                            else if (Convert.ToInt32(row["attribute_id"]) == 5125 && ProdQty >= 25 && ProdQty <= 49)
    //                            {
    //                                untPrice = oHelper.CDEC(row["numeric_value"]);
    //                            }
    //                            else if (Convert.ToInt32(row["attribute_id"]) == 5280 && ProdQty >= 10 && ProdQty <= 49)
    //                            {
    //                                untPrice = oHelper.CDEC(row["numeric_value"]);
    //                            }
    //                            else if (Convert.ToInt32(row["attribute_id"]) == 39 && ProdQty >= 1 && ProdQty <= 24)
    //                            {
    //                                untPrice = oHelper.CDEC(row["numeric_value"]);
    //                            }
    //                            else if (Convert.ToInt32(row["attribute_id"]) == 35 && ProdQty >= 10)
    //                            {
    //                                untPrice = oHelper.CDEC(row["numeric_value"]);
    //                            }
    //                            else if (Convert.ToInt32(row["attribute_id"]) == 43 && ProdQty >= 1 && ProdQty <= 9)
    //                            {
    //                                untPrice = oHelper.CDEC(row["numeric_value"]);
    //                            }
    //                            else if (Convert.ToInt32(row["attribute_id"]) == 5415 && ProdQty >= 8)
    //                            {
    //                                untPrice = oHelper.CDEC(row["numeric_value"]);
    //                            }
    //                            else if (Convert.ToInt32(row["attribute_id"]) == 5007 && ProdQty >= 5)
    //                            {
    //                                untPrice = oHelper.CDEC(row["numeric_value"]);
    //                            }
    //                            else if (Convert.ToInt32(row["attribute_id"]) == 4 && ProdQty >= 1)
    //                            {
    //                                untPrice = oHelper.CDEC(row["numeric_value"]);
    //                            }
    //                        }
    //                    }
    //                    else if (untPrice <= 0)
    //                    {
    //                        untPrice = oHelper.CDEC(oProd.GetProductBasePrice(ProductID));
    //                    }
    //                    DiscPrice = (untPrice * DiscPrice) / 100;
    //                    untPrice = untPrice - DiscPrice;
    //                    untPrice = oHelper.CDEC(untPrice.ToString("N2"));

    //                }
    //                else
    //                {
    //                    //Check the user default buyer group or custome buyer group.
    //                    int BGPriceID = oBG.GetBuyerGroupPriceID(UsrID);
    //                    string BGName = oBG.GetBuyerGroup(UsrID);
    //                    if (BGPriceID == 4 && BGName == "DEFAULTBG")
    //                    {
    //                        string strquery = "select product_id,numeric_value,attribute_id from tb_prod_specs where product_id=" + ProductID + " and numeric_value>0";
    //                        DataSet DSprice = new DataSet();
    //                        oHelper.SQLString = strquery;
    //                        DSprice = oHelper.GetDataSet();
    //                        if (DSprice != null && DSprice.Tables[0].Rows.Count > 0)
    //                        {

    //                            foreach (DataRow row in DSprice.Tables[0].Rows)
    //                            {
    //                                if (Convert.ToInt32(row["attribute_id"]) == 5111 && ProdQty >= 100)
    //                                {
    //                                    untPrice = oHelper.CDEC(row["numeric_value"]);
    //                                }
    //                                else if (Convert.ToInt32(row["attribute_id"]) == 127 && ProdQty >= 50)
    //                                {
    //                                    untPrice = oHelper.CDEC(row["numeric_value"]);
    //                                }
    //                                else if (Convert.ToInt32(row["attribute_id"]) == 40 && ProdQty >= 25)
    //                                {
    //                                    untPrice = oHelper.CDEC(row["numeric_value"]);
    //                                }
    //                                else if (Convert.ToInt32(row["attribute_id"]) == 5125 && ProdQty >= 25 && ProdQty <= 49)
    //                                {
    //                                    untPrice = oHelper.CDEC(row["numeric_value"]);
    //                                }
    //                                else if (Convert.ToInt32(row["attribute_id"]) == 5280 && ProdQty >= 10 && ProdQty <= 49)
    //                                {
    //                                    untPrice = oHelper.CDEC(row["numeric_value"]);
    //                                }
    //                                else if (Convert.ToInt32(row["attribute_id"]) == 39 && ProdQty >= 1 && ProdQty <= 24)
    //                                {
    //                                    untPrice = oHelper.CDEC(row["numeric_value"]);
    //                                }
    //                                else if (Convert.ToInt32(row["attribute_id"]) == 35 && ProdQty >= 10)
    //                                {
    //                                    untPrice = oHelper.CDEC(row["numeric_value"]);
    //                                }
    //                                else if (Convert.ToInt32(row["attribute_id"]) == 43 && ProdQty >= 1 && ProdQty <= 9)
    //                                {
    //                                    untPrice = oHelper.CDEC(row["numeric_value"]);
    //                                }
    //                                else if (Convert.ToInt32(row["attribute_id"]) == 5415 && ProdQty >= 8)
    //                                {
    //                                    untPrice = oHelper.CDEC(row["numeric_value"]);
    //                                }
    //                                else if (Convert.ToInt32(row["attribute_id"]) == 5007 && ProdQty >= 5)
    //                                {
    //                                    untPrice = oHelper.CDEC(row["numeric_value"]);
    //                                }
    //                                else if (Convert.ToInt32(row["attribute_id"]) == 4 && ProdQty >= 1)
    //                                {
    //                                    untPrice = oHelper.CDEC(row["numeric_value"]);
    //                                }
    //                            }
    //                        }
    //                        else if (untPrice <= 0)
    //                        {
    //                            untPrice = oHelper.CDEC(oProd.GetProductBasePrice(ProductID));
    //                        }
    //                    }
    //                    else
    //                    {
    //                        string strquery = "select product_id,numeric_value,attribute_id from tb_prod_specs where product_id=" + ProductID + " and numeric_value>0";
    //                    DataSet DSprice = new DataSet();
    //                    oHelper.SQLString = strquery;
    //                    DSprice = oHelper.GetDataSet();
    //                    if (DSprice != null && DSprice.Tables[0].Rows.Count > 0)
    //                    {

    //                         foreach (DataRow row in DSprice.Tables[0].Rows)
    //                            {
    //                                if (Convert.ToInt32(row["attribute_id"]) == 5111 && ProdQty >= 100)
    //                                {
    //                                    untPrice = oHelper.CDEC(row["numeric_value"]);
    //                                }
    //                                else if (Convert.ToInt32(row["attribute_id"]) == 127 && ProdQty >= 50)
    //                                {
    //                                    untPrice = oHelper.CDEC(row["numeric_value"]);
    //                                }
    //                                else if (Convert.ToInt32(row["attribute_id"]) == 40 && ProdQty >= 25)
    //                                {
    //                                    untPrice = oHelper.CDEC(row["numeric_value"]);
    //                                }
    //                                else if (Convert.ToInt32(row["attribute_id"]) == 5125 && ProdQty >= 25 && ProdQty <= 49)
    //                                {
    //                                    untPrice = oHelper.CDEC(row["numeric_value"]);
    //                                }
    //                                else if (Convert.ToInt32(row["attribute_id"]) == 5280 && ProdQty >= 10 && ProdQty <= 49)
    //                                {
    //                                    untPrice = oHelper.CDEC(row["numeric_value"]);
    //                                }
    //                                else if (Convert.ToInt32(row["attribute_id"]) == 39 && ProdQty >= 1 && ProdQty <= 24)
    //                                {
    //                                    untPrice = oHelper.CDEC(row["numeric_value"]);
    //                                }
    //                                else if (Convert.ToInt32(row["attribute_id"]) == 35 && ProdQty >= 10)
    //                                {
    //                                    untPrice = oHelper.CDEC(row["numeric_value"]);
    //                                }
    //                                else if (Convert.ToInt32(row["attribute_id"]) == 43 && ProdQty >= 1 && ProdQty <=9)
    //                                {
    //                                    untPrice = oHelper.CDEC(row["numeric_value"]);
    //                                }
    //                                else if (Convert.ToInt32(row["attribute_id"]) == 5415 && ProdQty >= 8)
    //                                {
    //                                    untPrice = oHelper.CDEC(row["numeric_value"]);
    //                                }
    //                                else if (Convert.ToInt32(row["attribute_id"]) == 5007 && ProdQty >= 5)
    //                                {
    //                                    untPrice = oHelper.CDEC(row["numeric_value"]);
    //                                }
    //                                else if(Convert.ToInt32(row["attribute_id"]) == 4 && ProdQty >= 1)
    //                                {
    //                                    untPrice = oHelper.CDEC(row["numeric_value"]);
    //                                }
    //                            }


    //                            //To calculate the discount price.
    //                            dsBgDisc = oBG.GetBuyerGroupBasedDiscountDetails(BGName);
    //                            if (dsBgDisc != null)
    //                            {
    //                                if (dsBgDisc.Tables[0].Rows.Count > 0)
    //                                {
    //                                    decimal DiscVal = oHelper.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
    //                                    DateTime ValidDt = DateTime.Now.Subtract(System.TimeSpan.FromDays(7));//By default set the  previous date.
    //                                    if (dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString() != "")
    //                                    {
    //                                        ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
    //                                    }
    //                                    string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
    //                                    bool IsBGCatProd = oBG.IsBGCatalogProduct(CatalogID, oBG.GetBuyerGroup(UsrID).ToString());
    //                                    if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
    //                                    {
    //                                        untPrice = oBG.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth);
    //                                    }
    //                                }
    //                            }
    //                            untPrice = oHelper.CDEC(untPrice.ToString("N2"));

    //                        }
    //                        else if (untPrice <= 0)
    //                        {
    //                            dsBgPrice = oProd.GetProductPriceValue(ProductID, BGPriceID);
    //                            if (dsBgPrice != null)
    //                            {
    //                                untPrice = oHelper.CDEC(dsBgPrice.Tables[0].Rows[0].ItemArray[1].ToString());

    //                                //To calculate the discount price.
    //                                dsBgDisc = oBG.GetBuyerGroupBasedDiscountDetails(BGName);
    //                                if (dsBgDisc != null)
    //                                {
    //                                    if (dsBgDisc.Tables[0].Rows.Count > 0)
    //                                    {
    //                                        decimal DiscVal = oHelper.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
    //                                        DateTime ValidDt = DateTime.Now.Subtract(System.TimeSpan.FromDays(7));//By default set the  previous date.
    //                                        if (dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString() != "")
    //                                        {
    //                                            ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
    //                                        }
    //                                        string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
    //                                        bool IsBGCatProd = oBG.IsBGCatalogProduct(CatalogID, oBG.GetBuyerGroup(UsrID).ToString());
    //                                        if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
    //                                        {
    //                                            untPrice = oBG.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth);
    //                                        }
    //                                    }
    //                                }
    //                                untPrice = oHelper.CDEC(untPrice.ToString("N2"));
    //                            }
    //                        }
    //                    }
    //                }//Buyergroup price.
    //                oItemInFo.ProductID = ProdID[i];
    //                oItemInFo.OrderID = OrID;
    //                oItemInFo.PriceApplied = untPrice;
    //                oItemInFo.UserID = UsrID;
    //                if (ProdQty != 0)
    //                {
    //                    int OrderID = oOrder.GetOrderID(oHelper.CI(Session["USER_ID"].ToString()));
    //                    int maxqty = oOrder.GetOrderItemQty(ProductID, OrderID);
    //                    int MinQty = oOrder.GetProductMinimumOrderQty(ProductID);
    //                    int MaxQtyAvl = maxqty + oOrder.GetProductAvilableQty(ProductID);
    //                    oItemInFo.Quantity = oHelper.CDEC(ProdQty);
    //                    int Qty = oHelper.CI(oItemInFo.Quantity);
    //                    ProdQty = MaxQtyAvl - Qty;
    //                    if (ProdQty >= 0)
    //                        oOrder.UpdateQuantity(ProductID, ProdQty);
    //                }
    //                else
    //                {
    //                    oItemInFo.Quantity = 1;
    //                }
    //                if (chkExistsItem == 0)
    //                {
    //                    if (oOrder.AddOrderItem(oItemInFo) != -1)
    //                    {
    //                        DataSet dsOrder = new DataSet();
    //                        dsOrder = oOrder.GetOrderPriceValues(OrID);

    //                        if (dsOrder != null)
    //                        {
    //                            decimal ProdTotalPrice;
    //                            decimal OrderTotal;

    //                            decimal ExistProdTotal = oHelper.CDEC(oOrder.GetCurrentProductTotalCost(OrID));
    //                            ProdTotalPrice = ExistProdTotal + (oItemInFo.PriceApplied * oItemInFo.Quantity);
    //                            decimal Tax = CalculateTaxAmount(ProdTotalPrice);

    //                            OrderTotal = ProdTotalPrice + Tax;
    //                            oOrdInfo.OrderID = OrID;
    //                            oOrdInfo.ProdTotalPrice = oHelper.CDEC(oHelper.FixDecPlace(ProdTotalPrice));
    //                            oOrdInfo.TaxAmount = oHelper.CDEC(oHelper.FixDecPlace(Tax));
    //                            oOrdInfo.TotalAmount = oHelper.CDEC(oHelper.FixDecPlace(OrderTotal));
    //                            oOrder.UpdateOrderPrice(oOrdInfo, true);
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    //update the existing order item.
    //                    //Update the new product price to exists products price.
    //                    DataSet dsOrder = new DataSet();
    //                    dsOrder = oOrder.GetOrderPriceValues(OrID);

    //                    if (dsOrder != null)
    //                    {
    //                        decimal ProdTotalPrice;
    //                        decimal OrderTotal;
    //                        decimal TotalShipCost;
    //                        decimal ExistProdTotal = oHelper.CDEC(oOrder.GetCurrentProductTotalCost(OrID));

    //                        decimal Tax = 0.00M;
    //                        if (ProdQty >= chkExistsItem)
    //                        {
    //                            ProdTotalPrice = (ExistProdTotal + (oItemInFo.PriceApplied * (oItemInFo.Quantity - chkExistsItem)));
    //                        }
    //                        else
    //                        {
    //                            ProdTotalPrice = (ExistProdTotal - (oItemInFo.PriceApplied * (chkExistsItem - oItemInFo.Quantity)));
    //                        }

    //                        Tax = (ProdTotalPrice * Tax) / 100;
    //                        OrderTotal = ProdTotalPrice + Tax;

    //                        oOrdInfo.OrderID = OrID;
    //                        oOrdInfo.ProdTotalPrice = oHelper.CDEC(oHelper.FixDecPlace(ProdTotalPrice));
    //                        oOrdInfo.TaxAmount = oHelper.CDEC(oHelper.FixDecPlace(Tax));
    //                        oOrdInfo.TotalAmount = oHelper.CDEC(oHelper.FixDecPlace(OrderTotal));
    //                        oOrder.UpdateOrderPrice(oOrdInfo, true);

    //                    }
    //                    oOrder.UpdateOrderItem(oItemInFo);
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        oErr.ErrorMsg = ex;
    //        oErr.CreateLog();
    //    }
    //}

    //public decimal CalculateTaxAmount(decimal ProdTotalPrice)
    //{
    //    try
    //    {
    //        CountryServices objCountryServices = new CountryServices();
    //        string BillState;
    //        string BillCountry;
    //        if (objUserServices.GetTaxExempt(ObjHelperServices.CI(Session["USER_ID"])) == false)
    //        {
    //            decimal RetTax = 0.00M;
    //            if (FIXED_TAX.ToUpper() == "TRUE")
    //            {

    //                decimal tax = ObjHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString());
    //                RetTax = ObjHelperServices.CDEC(ObjHelperServices.FixDecPlace(ProdTotalPrice * (tax / 100)));
    //            }
    //            else
    //            {
    //                BillState = objUserServices.GetUserBillStateCode(ObjHelperServices.CI(Session["USER_ID"]));
    //                BillCountry = objUserServices.GetUserBillCountryCode(ObjHelperServices.CI(Session["USER_ID"]));
    //                decimal tax = ObjHelperServices.CDEC(objCountryServices.GetStateTax(BillCountry, BillState));

    //                RetTax = ObjHelperServices.CDEC(ObjHelperServices.FixDecPlace(ProdTotalPrice * (tax / 100)));
    //            }
    //            return RetTax;
    //        }
    //        return 0;
    //    }
    //    catch (Exception ex)
    //    {
    //        objErrorHandler.ErrorMsg = ex;
    //        objErrorHandler.CreateLog();
    //        return -1;
    //    }
    //}

    //public void MsgBox(string Msg)
    //{
    //    string script = "<script>alert('" + Msg + "');</script>";
    //    if (!IsClientScriptBlockRegistered("alert"))
    //    {
    //        this.RegisterClientScriptBlock("alert", script);
    //    }
    //}
    protected DataSet GetOrderTemplateDetails()
    {

        try
        {

            if (isSave ==true) return null;

            string T_id = "";
            if (HttpContext.Current.Request.QueryString["Tempid"] != null)
            {
                T_id = HttpContext.Current.Request.QueryString["Tempid"].ToString();
            }
            else
            {
                T_id = "0";

                
            }

            if (objHelperServices.CI(T_id) > 0)
            {
                OrderServices objOrderService = new OrderServices();
                int userid = objHelperServices.CI(HttpContext.Current.Session["USER_ID"]);

                return objOrderService.GetOrderTemplateItem(userid, T_id);


            }
        }
        catch (Exception ex)
        {
            return null;
        }
        return null;
    }
    private int GetProductID(string Code)
    {
        object retval = "-1";
        //string sql = string.Format("SELECT PRODUCT_ID FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID = 1 AND STRING_VALUE = '{0}'", Code);
        //oHelper.SQLString = sql;
        //retval = oHelper.GetValue("PRODUCT_ID");
        //return oHelper.CI(retval);
        retval = (string)objHelperDB.GetGenericDataDB(Code, "GET_BULLORDER_PRODUCT_ID", HelperDB.ReturnType.RTString);
        if (retval != null && retval != "")
            retval = ObjHelperServices.CI(retval);

        return (int)retval;

    }
    protected void BtnAddtoCart_Click(object sender, EventArgs e)
    {
        AddToCart(TempProdQtys, TempProdItems);
    }
    public void AddToCart(string TempProdQtys, string TempProdItems)
    {
        try
        {
            //erritemId.Text = "";
            if (txtcnt != null && txtcnt != "" && ItemCnt == 0)
            {
                txtCount = ObjHelperServices.CI(txtcnt.ToString());
            }
            else
            {
                txtCount = ItemCnt;
            }
            string[] ProdAQty = new string[txtCount];
            string[] ProdAItem = new string[txtCount];
            string[] ItemQty = new string[txtCount];
            //ProdID = new int[txtCount];
            //ProdQnty = new int[txtCount];

            if (TempProdQtys == null || TempProdItems == null)
                return;

            ProdAQty = Regex.Split(TempProdQtys, ",");
            ProdAItem = Regex.Split(TempProdItems, ",");


            string Pqtycheck = "";
            Int32 Num;
            for (int i = 0; i < ProdAQty.Length - 1; i++)
            {
                Pqtycheck = ProdAQty[i].ToString().Trim();
                bool isNum = Int32.TryParse(Pqtycheck, out Num);

                if (isNum == false)
                {
                    string Script = "alert('Incorrect Code or Invalid QTY');";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Myscript", Script, true);
                    return;
                }
            }

            string _notfoundstr = "";
            string _notfoundstrhtml = "";
            string _minqty = "";
            string _minqtyhtml = "";
            string _maxqty = "";
            string _maxqtyhtml = "";
            string _notfoundqtyhtml = "";
            bool itemcheck = false;
            string PCodeAll = "";
            DataTable protbl = null;
            DataTable protblsub = null;

            decimal ProdTotalPrice = 0;
            decimal OrderTotal = 0;
            decimal TotalShipCost = 0;
            DataTable dt = null;
            int tmpcount = 0;
            int StrProductStatus = -1;
            int StrProductStatusSub = -1;
            string StrStockStatus = "";
            //bool Prod_Stock_Status_sub = false;
            //bool Prod_Stock_Status = false;
            for (int i = 0; i < ProdAItem.Length; i++)
            {
                PCodeAll = PCodeAll + "'" + ProdAItem[i].ToString().Trim() + "',";

            }
            if (PCodeAll != "")
            {
                PCodeAll = PCodeAll.Substring(0, PCodeAll.Length - 1) + "";
                protbl = (DataTable)objHelperDB.GetGenericPageDataDB(PCodeAll, "GET_BULKORDER_INVENTORY_COUNT_ALL", HelperDB.ReturnType.RTTable);

            }
            tmpcount = PCodeAll.Split(',').Length - 1;

            //if (flgcprowcheck == true)
            //{

            //    if (protbl == null && flgcprowcheck == true || tmpcount != protbl.Rows.Count && flgcprowcheck == true)
            //    {
            //        string Script = "alert('Incorrect Code or Invalid QTY');";
            //        Page.ClientScript.RegisterStartupScript(this.GetType(), "Myscript", Script, true);
            //        return;
            //    }
            //}


            int OrderID = 0;
            string OrderStatus = "";
                     
            int ProdCodeId = 0;
            OrderServices.OrderItemInfo oItemInFo = new OrderServices.OrderItemInfo();
            OrderServices.Order_Calrification_ItemInfo oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
            oOrdInfo.UserID = ObjHelperServices.CI(Session["USER_ID"]);
            //OrderID = objOrderServices.GetOrderID(oOrdInfo.UserID, OpenOrdStatusID);

            //string CustomerType = objUserServices.GetCustomerType(oOrdInfo.UserID);
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
            }

            decimal ExistProdTotal = ObjHelperServices.CDEC(objOrderServices.GetCurrentProductTotalCost(OrderID));
            decimal ExistShipCostTotal = ObjHelperServices.CDEC(objOrderServices.GetShippingCost(OrderID));
            ProdTotalPrice = ExistProdTotal;
            TotalShipCost = ExistShipCostTotal;
            for (int i = 0; i < ProdAItem.Length; i++)
            {
                
                int checkset = -1;
                int checksetsub = -1;
                string product_status = "";
                string ebay_block = null;
                string _pCode = ProdAItem[i].ToString().Trim();
                decimal _pQty = string.IsNullOrEmpty(_pCode.Trim()) ? 0 : Convert.ToDecimal(ProdAQty[i]);

                if (_pCode != "")
                {
                    //string stquery = "SELECT COUNT(*) CNT FROM tbwc_inventory where product_id=(SELECT TOP(1)product_id FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID=1 AND STRING_VALUE='" + _pCode + "')";
                    //oHelper.SQLString = stquery;
                    //checkset = oHelper.CI(oHelper.GetValue("CNT")); // GetDataSet(stquery);
                    //string cnt = (string)objHelperDB.GetGenericPageDataDB(_pCode, "GET_BULKORDER_INVENTORY_COUNT", HelperDB.ReturnType.RTString);
                    //if (cnt != null && cnt != "")
                    //checkset = ObjHelperServices.CI(cnt);   
                    ProdCodeId = 0;
                    product_status = "";
                    StrProductStatusSub = -1;
                    StrStockStatus = "";
                    if (protbl != null && protbl.Rows.Count > 0)
                    {
                        DataRow[] dr = protbl.Select("PCode='" + _pCode + "'");
                        if (dr.Length > 0)
                        {
                            foreach (DataRow drow in dr)
                            {
                                if (drow["product_status"].ToString().ToUpper() == "AVAILABLE")
                                {
                                    checkset = 1;
                                    ProdCodeId = Convert.ToInt32(drow["PRODUCT_ID"].ToString());
                                    StrStockStatus = drow["STOCK_STATUS"].ToString().Replace("_"," ");
                                   // objErrorHandler.CreateLog("BULK1" + ProdCodeId);
                                    StrProductStatusSub = objProductServices.GetStockStatusDesc(StrStockStatus, Convert.ToInt32((drow["PROD_STOCK_STATUS"].ToString())), Convert.ToInt32((drow["PROD_STOCK_FLAG"].ToString())), oOrdInfo.UserID, ProdCodeId);

                                    //if (objProductServices.GetStockStatusDesc(StrStockStatus, oOrdInfo.UserID) == 1)
                                    //    StrProductStatusSub = 1;
                                    //else
                                    //    StrProductStatusSub = Convert.ToInt32((drow["PROD_STOCK_STATUS"].ToString()));

                                    product_status = drow["product_status"].ToString();
                                    ebay_block = drow["EBAY_BLOCK"].ToString();
                                    break;
                                }
                            }
                            if (product_status == "")
                            {
                                foreach (DataRow drow in dr)
                                {
                                    if (drow["product_status"].ToString().ToUpper() != "AVAILABLE")
                                    {
                                        checkset = 2;
                                        ProdCodeId = Convert.ToInt32(drow["PRODUCT_ID"].ToString());
                                        StrStockStatus = drow["STOCK_STATUS"].ToString().Replace("_", " ");
                                       // objErrorHandler.CreateLog("BULK2" + ProdCodeId);
                                        StrProductStatusSub = objProductServices.GetStockStatusDesc(StrStockStatus, Convert.ToInt32((drow["PROD_STOCK_STATUS"].ToString())), Convert.ToInt32((drow["PROD_STOCK_FLAG"].ToString())), oOrdInfo.UserID,ProdCodeId);
                                        //if (objProductServices.GetStockStatusDesc(StrStockStatus, oOrdInfo.UserID) == 1)
                                        //    StrProductStatusSub = 1;
                                        //else
                                        //    StrProductStatusSub = Convert.ToInt32((drow["PROD_STOCK_STATUS"].ToString()));

                                        product_status = drow["product_status"].ToString();
                                       
                                        break;
                                    }
                                }
                            }

                        }
                        else
                        {
                            checkset = 0;
                            ProdCodeId = 0;
                            StrProductStatusSub = -1;
                            product_status = "";
                        }

                    }
                    else
                    {
                        checkset = 0;
                        ProdCodeId = 0;
                        StrProductStatusSub = -1;
                        product_status = "";
                    }
                  

                    if (checkset == 0)
                    {
                        // _notfoundstr += string.Format("{0},", _pCode);

                        string _substitute = FindSubstitute(_pCode);

                        if (_substitute != "{~MI~}" && _substitute != "{~N/A~}")
                        {
                            protblsub = (DataTable)objHelperDB.GetGenericPageDataDB("'" + _substitute + "'", "GET_BULKORDER_INVENTORY_COUNT_ALL", HelperDB.ReturnType.RTTable);

                            if (protblsub != null && protblsub.Rows.Count > 0)
                            {

                                foreach (DataRow drow in protblsub.Rows)
                                {
                                    if (drow["product_status"].ToString().ToUpper() == "AVAILABLE")
                                    {
                                        checksetsub = 1;
                                        ProdCodeId = Convert.ToInt32(drow["PRODUCT_ID"].ToString());
                                        StrStockStatus = drow["STOCK_STATUS"].ToString().Replace("_", " ");
                                       // objErrorHandler.CreateLog("BULK2" + ProdCodeId);
                                        StrProductStatusSub = objProductServices.GetStockStatusDesc(StrStockStatus, Convert.ToInt32((drow["PROD_STOCK_STATUS"].ToString())), Convert.ToInt32((drow["PROD_STOCK_FLAG"].ToString())), oOrdInfo.UserID, ProdCodeId);
                                        //if (objProductServices.GetStockStatusDesc(StrStockStatus, oOrdInfo.UserID) == 1)
                                        //    StrProductStatusSub = 1;
                                        //else
                                        //    StrProductStatusSub = Convert.ToInt32((drow["PROD_STOCK_STATUS"].ToString()));

                                        product_status = drow["product_status"].ToString();
                                        
                                        break;
                                    }
                                }
                                if (product_status == "")
                                {
                                    foreach (DataRow drow in protblsub.Rows)
                                    {
                                        if (drow["product_status"].ToString().ToUpper() != "AVAILABLE")
                                        {
                                            checksetsub = 2;
                                            ProdCodeId = Convert.ToInt32(drow["PRODUCT_ID"].ToString());
                                            StrStockStatus = drow["STOCK_STATUS"].ToString().Replace("_", " ");
                                           // objErrorHandler.CreateLog("BULK3" + ProdCodeId);
                                            StrProductStatusSub = objProductServices.GetStockStatusDesc(StrStockStatus, Convert.ToInt32((drow["PROD_STOCK_STATUS"].ToString())), Convert.ToInt32((drow["PROD_STOCK_FLAG"].ToString())), oOrdInfo.UserID, ProdCodeId);
                                            //if (objProductServices.GetStockStatusDesc(StrStockStatus,oOrdInfo.UserID)==1)
                                            //    StrProductStatusSub = 1;
                                            //else
                                            //    StrProductStatusSub = Convert.ToInt32((drow["PROD_STOCK_STATUS"].ToString()));

                                            product_status = drow["product_status"].ToString();
                                            
                                            break;
                                        }
                                    }
                                }
                            }
                        }

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
                            objOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);
                        }
                        else if (_substitute == "{~N/A~}" || checksetsub == 2)
                        {
                          
                            _notfoundstr += string.Format("{0},", _pCode);
                            oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
                            oItemClaItemInfo.Clarification_ID = 0;
                            oItemClaItemInfo.OrderID = OrderID;
                            oItemClaItemInfo.ProductDesc = _pCode;
                            oItemClaItemInfo.Quantity = _pQty;
                            oItemClaItemInfo.UserID = oOrdInfo.UserID;
                            oItemClaItemInfo.Clarification_Type = "ITEM_ERROR";
                            objOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);
                        }
                        else if (checksetsub == 1 && StrProductStatusSub == 0 )
                        {
                            _notfoundstr += string.Format("{0},", _pCode);
                            oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
                            oItemClaItemInfo.Clarification_ID = 0;
                            oItemClaItemInfo.OrderID = OrderID;
                            oItemClaItemInfo.ProductDesc = _pCode;
                            oItemClaItemInfo.Quantity = _pQty;
                            oItemClaItemInfo.UserID = oOrdInfo.UserID;
                            oItemClaItemInfo.Clarification_Type = "ITEM_REPLACE";
                            objOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);
                        } 
                      else if(checksetsub == 1 && StrProductStatusSub == 1)
                        {

                              
                                ProdAItem[i] = _substitute;
                                _pCode = _substitute;

                                oItemInFo.ProductID = ProdCodeId;//GetProductID(ProdAItem[i]);
                                oItemInFo.OrderID = OrderID;
                                oItemInFo.PriceApplied = GetMyPrice_Exc(oItemInFo.ProductID, ObjHelperServices.CI(ProdAQty[i]));

                                oItemInFo.UserID = oOrdInfo.UserID;
                                oItemInFo.Quantity = ObjHelperServices.CI(ProdAQty[i]);
                                oItemInFo.Ship_Cost = CalculateShippingCost(OrderID, oItemInFo.ProductID, oItemInFo.PriceApplied, ObjHelperServices.CI(oItemInFo.Quantity));
                                oItemInFo.Tax_Amount = objOrderServices.CalculateTaxAmount(oItemInFo.Quantity * oItemInFo.PriceApplied, OrderID.ToString(), ProdCodeId.ToString());


                                if (oItemInFo.PriceApplied == 0 && objProductServices.GetProductPromotion(oItemInFo.ProductID) == "N")
                                {
                                    _notfoundstr += string.Format("{0},", _pCode);
                                    oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
                                    oItemClaItemInfo.Clarification_ID = 0;
                                    oItemClaItemInfo.OrderID = OrderID;
                                    oItemClaItemInfo.ProductDesc = _pCode;
                                    oItemClaItemInfo.Quantity = _pQty;
                                    oItemClaItemInfo.UserID = oOrdInfo.UserID;
                                    oItemClaItemInfo.Clarification_Type = "ITEM_PROMOTION";
                                    objOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);
                                }
                                else
                                {

                                    if (HttpContext.Current.Session["EBAY_BLOCK"] != null && HttpContext.Current.Session["EBAY_BLOCK"].ToString() == "True" && ebay_block != null && ebay_block == "True")
                                    {

                                        objErrorHandler.CreateLog("pCode" + _pCode + "OrderID" + OrderID + "userid" + oOrdInfo.UserID+"EbayBlock");
                                        _notfoundstr += string.Format("{0},", _pCode);
                                        oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
                                        oItemClaItemInfo.Clarification_ID = 0;
                                        oItemClaItemInfo.OrderID = OrderID;
                                        oItemClaItemInfo.ProductDesc = _pCode;
                                        oItemClaItemInfo.Quantity = _pQty;
                                        oItemClaItemInfo.UserID = oOrdInfo.UserID;
                                        oItemClaItemInfo.Clarification_Type = "ITEM_ERROR";
                                        objOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);
                                    }
                                    else
                                    {

                                        objOrderServices.AddOrderItem(oItemInFo);
                                    }
                                }
                             

                            
                        }
                        else
                        {

                            _notfoundstr += string.Format("{0},", _pCode);
                            oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
                            oItemClaItemInfo.Clarification_ID = 0;
                            oItemClaItemInfo.OrderID = OrderID;
                            oItemClaItemInfo.ProductDesc = _pCode;
                            oItemClaItemInfo.Quantity = _pQty;
                            oItemClaItemInfo.UserID = oOrdInfo.UserID;
                            oItemClaItemInfo.Clarification_Type = "ITEM_ERROR";
                            objOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);

                        }

                    }
                    else if (checkset == 2)
                    {
                        _notfoundstr += string.Format("{0},", _pCode);
                        oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
                        oItemClaItemInfo.Clarification_ID = 0;
                        oItemClaItemInfo.OrderID = OrderID;
                        oItemClaItemInfo.ProductDesc = _pCode;
                        oItemClaItemInfo.Quantity = _pQty;
                        oItemClaItemInfo.UserID = oOrdInfo.UserID;
                        oItemClaItemInfo.Clarification_Type = "ITEM_ERROR";
                        objOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);
                    }


                    if (checkset == 1 && StrProductStatusSub == 0)
                    {
                        _notfoundstr += string.Format("{0},", _pCode);
                        oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
                        oItemClaItemInfo.Clarification_ID = 0;
                        oItemClaItemInfo.OrderID = OrderID;
                        oItemClaItemInfo.ProductDesc = _pCode;
                        oItemClaItemInfo.Quantity = _pQty;
                        oItemClaItemInfo.UserID = oOrdInfo.UserID;
                        oItemClaItemInfo.Clarification_Type = "ITEM_REPLACE";
                        objOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);
                    }
                    else if (checkset == 1 && StrProductStatusSub == 1)
                    {


                    
                            oItemInFo.ORDER_ITEM_ID = 0;
                            oItemInFo.ProductID = ProdCodeId;// GetProductID(ProdAItem[i]);
                            oItemInFo.OrderID = OrderID;
                            oItemInFo.PriceApplied = GetMyPrice_Exc(oItemInFo.ProductID, ObjHelperServices.CI(ProdAQty[i]));
                            oItemInFo.UserID = oOrdInfo.UserID;
                            oItemInFo.Quantity = ObjHelperServices.CI(ProdAQty[i]);
                            oItemInFo.Ship_Cost = CalculateShippingCost(OrderID, oItemInFo.ProductID, oItemInFo.PriceApplied, ObjHelperServices.CI(oItemInFo.Quantity));
                            oItemInFo.Tax_Amount = objOrderServices.CalculateTaxAmount(oItemInFo.Quantity * oItemInFo.PriceApplied, OrderID.ToString(), ProdCodeId.ToString());

                            if (oItemInFo.PriceApplied == 0 && objProductServices.GetProductPromotion(oItemInFo.ProductID) == "N")
                            {
                                _notfoundstr += string.Format("{0},", _pCode);
                                oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
                                oItemClaItemInfo.Clarification_ID = 0;
                                oItemClaItemInfo.OrderID = OrderID;
                                oItemClaItemInfo.ProductDesc = _pCode;
                                oItemClaItemInfo.Quantity = _pQty;
                                oItemClaItemInfo.UserID = oOrdInfo.UserID;
                                oItemClaItemInfo.Clarification_Type = "ITEM_PROMOTION";
                                objOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);
                            }
                            else
                            {
                                if (HttpContext.Current.Session["EBAY_BLOCK"] != null && HttpContext.Current.Session["EBAY_BLOCK"].ToString() == "True" && ebay_block != null && ebay_block == "True")
                                {


                                    _notfoundstr += string.Format("{0},", _pCode);
                                    oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
                                    oItemClaItemInfo.Clarification_ID = 0;
                                    oItemClaItemInfo.OrderID = OrderID;
                                    oItemClaItemInfo.ProductDesc = _pCode;
                                    oItemClaItemInfo.Quantity = _pQty;
                                    oItemClaItemInfo.UserID = oOrdInfo.UserID;
                                    oItemClaItemInfo.Clarification_Type = "ITEM_ERROR";
                                    objOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);
                                }
                                else
                                {

                                    objOrderServices.AddOrderItem(oItemInFo);
                                }
                            }



                          


                            if (OrderID != null)
                            {


                                decimal ProdShippCost = ObjHelperServices.CDEC(CalculateShippingCost(OrderID, ProductID, oItemInFo.PriceApplied, ObjHelperServices.CI(oItemInFo.Quantity)));

                                ProdTotalPrice = ProdTotalPrice + (oItemInFo.PriceApplied * oItemInFo.Quantity);

                                TotalShipCost = TotalShipCost + ProdShippCost;


                                //Calculate Shipping Cost

                                // decimal ExistProdTotal = ObjHelperServices.CDEC(objOrderServices.GetCurrentProductTotalCost(OrderID));

                                //decimal ProdShippCost = ObjHelperServices.CDEC(CalculateShippingCost(OrderID, ProductID, oItemInFo.PriceApplied, ObjHelperServices.CI(oItemInFo.Quantity)));

                                //ProdTotalPrice = ExistProdTotal + (oItemInFo.PriceApplied * oItemInFo.Quantity);

                                //decimal Tax = CalculateTaxAmount(ProdTotalPrice);

                                //TotalShipCost = ObjHelperServices.CDEC(objOrderServices.GetShippingCost(OrderID)) + ProdShippCost;

                                //if (_IsShippingFree == true)
                                //{
                                //    TotalShipCost = 0;
                                //    _IsShippingFree = false;
                                //}

                                //oOrdInfo.ShipCost = ObjHelperServices.CDEC(ObjHelperServices.FixDecPlace(TotalShipCost));
                                //OrderTotal = ProdTotalPrice + Tax + TotalShipCost;
                                //oOrdInfo.OrderID = OrderID;
                                //oOrdInfo.ProdTotalPrice = ObjHelperServices.CDEC(ObjHelperServices.FixDecPlace(ProdTotalPrice));
                                //oOrdInfo.TaxAmount = ObjHelperServices.CDEC(ObjHelperServices.FixDecPlace(Tax));
                                //oOrdInfo.TotalAmount = ObjHelperServices.CDEC(ObjHelperServices.FixDecPlace(OrderTotal));
                                //objOrderServices.UpdateOrderPrice(oOrdInfo, true);
                            }
                        
                    }


                }

            }

            /*if (OrderID != null)
            {
                decimal Tax = CalculateTaxAmount(ProdTotalPrice);


                if (_IsShippingFree == true)
                {
                    TotalShipCost = 0;
                    _IsShippingFree = false;
                }

                oOrdInfo.ShipCost = ObjHelperServices.CDEC(ObjHelperServices.FixDecPlace(TotalShipCost));
                OrderTotal = ProdTotalPrice + Tax + TotalShipCost;
                oOrdInfo.OrderID = OrderID;
                oOrdInfo.ProdTotalPrice = ObjHelperServices.CDEC(ObjHelperServices.FixDecPlace(ProdTotalPrice));
                oOrdInfo.TaxAmount = ObjHelperServices.CDEC(ObjHelperServices.FixDecPlace(Tax));
                oOrdInfo.TotalAmount = ObjHelperServices.CDEC(ObjHelperServices.FixDecPlace(OrderTotal));
                objOrderServices.UpdateOrderPrice(oOrdInfo, true);
            }*/
            oOrdInfo.OrderID = OrderID;
            objOrderServices.UpdateOrderPrice(oOrdInfo, true);

           /* if (Session["ITEM_ERROR"] == null)
            {
                Session["ITEM_ERROR"] = _notfoundstr;
            }
            else
            {
                Session["ITEM_ERROR"] = Session["ITEM_ERROR"] + _notfoundstr;

            }



            if (Session["ITEM_CHK"] == null)
            {
                Session["ITEM_CHK"] = _notfoundstrhtml;
            }
            else
            {
                Session["ITEM_CHK"] = Session["ITEM_CHK"] + _notfoundstrhtml;

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
            */
            string enc = System.DateTime.Now.Millisecond.ToString();

            if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
            {
                Response.Redirect("OrderDetails.aspx?ORDER_ID=" + Convert.ToInt32(Session["ORDER_ID"]) + "&bulkorder=1&ViewOrder=View&" + enc, true);
            }
            else
            {
                Response.Redirect("OrderDetails.aspx?bulkorder=1&" + enc, true);

            }


        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            //objErrorHandler.CreateLog();
        }
    }
    protected DataSet GetEntrySaveData()
    {
        return dscodes;
    }
    public void SettextData(string Qtys, string Items)
    {
        string hidproductcodes = Items;
        string hidquantitys = Qtys;
            if (hidproductcodes != null && hidproductcodes != "" && hidquantitys != null && hidquantitys != "")
            {

                DataSet tmpds = new DataSet();
                DataTable tmpdatatbl = new DataTable();
                tmpdatatbl.Columns.Add("productcode", typeof(string));
                tmpdatatbl.Columns.Add("quantity", typeof(string));

                string productcode = hidproductcodes;
                string quantity = hidquantitys;

                string[] productcodes = productcode.Split(',');
                string[] quantitys = quantity.Split(',');

                for (int i = 0; i <= productcodes.Length - 1; i++)
                {
                    if (productcodes[i].Trim()!="" && objHelperServices.CI(quantitys[i])>0 )
                    tmpdatatbl.Rows.Add(productcodes[i], quantitys[i]);

                }
                tmpds.Tables.Add(tmpdatatbl);
                if (tmpds != null && tmpds.Tables.Count > 0)
                    dscodes = tmpds;
                else
                    dscodes = null;

            }
    }
    protected void BtnSaveOrderTemplate_Click(object sender, EventArgs e)
    {
        AddToOrderTemplate(TempProdQtys, TempProdItems);
    }
    protected void SetRepeterData(DataTable dt, string type)
    {
        if (type == "Error")
        {
            RepeaterError.DataSource = dt;
            RepeaterError.DataBind();
        }


    }

    protected void AddToOrderTemplate(string TempProdQtys, string TempProdItems)
    {
        try
        {
            isSave = true;



          

            //erritemId.Text = "";
            int userid = 0;
            if (txtcnt != null && txtcnt != "" && ItemCnt == 0)
            {
                txtCount = ObjHelperServices.CI(txtcnt.ToString());
            }
            else
            {
                txtCount = ItemCnt;
            }
            string[] ProdAQty = new string[txtCount];
            string[] ProdAItem = new string[txtCount];
            string[] ItemQty = new string[txtCount];
            //ProdID = new int[txtCount];
            //ProdQnty = new int[txtCount];

            if (TempProdQtys == null || TempProdItems == null)
                return;

            SettextData(TQtys, TProds);

            if (TxtTemplateName.Text.Trim() == "")
            {
                txterr.Text = "Enter Template Name";
                return;
            }
            

            ProdAQty = Regex.Split(TempProdQtys, ",");
            ProdAItem = Regex.Split(TempProdItems, ",");


            string Pqtycheck = "";
            Int32 Num;
            for (int i = 0; i < ProdAQty.Length - 1; i++)
            {
                Pqtycheck = ProdAQty[i].ToString().Trim();
                bool isNum = Int32.TryParse(Pqtycheck, out Num);

                if (isNum == false)
                {
                    string Script = "alert('Incorrect Code or Invalid QTY');";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Myscript", Script, true);
                    return;
                }
            }

            string _notfoundstr = "";
            string _notfoundstrhtml = "";
            string _minqty = "";
            string _minqtyhtml = "";
            string _maxqty = "";
            string _maxqtyhtml = "";
            string _notfoundqtyhtml = "";
            bool itemcheck = false;
            string PCodeAll = "";
            DataTable protbl = null;

            decimal ProdTotalPrice = 0;
            decimal OrderTotal = 0;
            decimal TotalShipCost = 0;
            DataTable dt = null;
            int tmpcount = 0;
            for (int i = 0; i < ProdAItem.Length; i++)
            {
                PCodeAll = PCodeAll + "'" + ProdAItem[i].ToString().Trim() + "',";

            }
            if (PCodeAll != "")
            {
                PCodeAll = PCodeAll.Substring(0, PCodeAll.Length - 1) + "";
                protbl = (DataTable)objHelperDB.GetGenericPageDataDB(PCodeAll, "GET_BULKORDER_INVENTORY_COUNT_ALL", HelperDB.ReturnType.RTTable);

            }
            tmpcount = PCodeAll.Split(',').Length - 1;

            //if (flgcprowcheck == true)
            //{

            //    if (protbl == null && flgcprowcheck == true || tmpcount != protbl.Rows.Count && flgcprowcheck == true)
            //    {
            //        string Script = "alert('Incorrect Code or Invalid QTY');";
            //        Page.ClientScript.RegisterStartupScript(this.GetType(), "Myscript", Script, true);
            //        return;

            //    }
            //}


            int Template_id = 0;
            string OrderStatus = "";
            OrderServices.OrderTemplateInfo oTemInFo = new OrderServices.OrderTemplateInfo();
            OrderServices.OrderTemplateItemInfo  oItemInFo = new OrderServices.OrderTemplateItemInfo();
            OrderServices.Order_Calrification_ItemInfo oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
            userid = ObjHelperServices.CI(Session["USER_ID"]);
            string proids = "";

            oTemInFo.TemplateName = TxtTemplateName.Text;
            oTemInFo.Description=TxtDesc.Text;
            oTemInFo.UserID =userid;
            oTemInFo.CompanyID = HttpContext.Current.Session["COMPANY_ID"].ToString();
            if (HttpContext.Current.Request.QueryString["TempId"] != null)
            {
                oTemInFo.TemplateId = Convert.ToInt32(HttpContext.Current.Request.QueryString["TempId"]);
                DataSet  tmpds= objOrderServices.GetOrderTemplateItem(oTemInFo.UserID,oTemInFo.TemplateId.ToString());
                if (tmpds==null || tmpds.Tables.Count==0 || tmpds.Tables[0].Rows.Count==0  )
                {
                     string Script = "alert('Invalid Template Id');";
                     Page.ClientScript.RegisterStartupScript(this.GetType(), "Tempscript", Script, true);
                     return;
                }
               

            }
            else
                oTemInFo.TemplateId = 0;

            int isexists=objOrderServices.GetOrderTemplateNameExists(TxtTemplateName.Text, oTemInFo.UserID, oTemInFo.TemplateId);
            if (isexists > 0)
            {
                string Script = "alert('Template Name Already Exists');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Tempscript1", Script, true);
                return;
            }

            DataTable tbErrorItem = objOrderServices.GetOrder_Clarification_Items(oTemInFo.TemplateId, "TEMP_ITEM_ERROR");
            DataTable tbErrorChk = objOrderServices.GetOrder_Clarification_Items(oTemInFo.TemplateId, "TEMP_ITEM_CHK");

            if ((tbErrorItem != null && tbErrorItem.Rows.Count > 0) || (tbErrorChk != null && tbErrorChk.Rows.Count > 0))
            {
                string Script = "alert('Please review and correct Order Clarifications / Errors before proceeding to Save Template!');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Tempscript2", Script, true);
                return;
            }
            
            //if ((HttpContext.Current.Session["TEMPLATE_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["TEMPLATE_ID"]) > 0))
            //{
            //    oTemInFo.TemplateId = Convert.ToInt32(Session["TEMPLATE_ID"]);
            //}
            //else
            //    oTemInFo.TemplateId = 0;

            Template_id = objOrderServices.AddOrderItemTemplate(oTemInFo);

           
            //objOrderServices.RemoveOrderTemplate(userid); 

            for (int i = 0; i < ProdAItem.Length; i++)
            {
                int checkset = -1;
                string _pCode = ProdAItem[i].ToString().Trim();
                decimal _pQty = string.IsNullOrEmpty(_pCode.Trim()) ? 0 : Convert.ToDecimal(ProdAQty[i]);

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
                        

                        string _substitute = FindSubstitute(_pCode);
                        if (_substitute == "{~MI~}")
                        {
                            _notfoundstrhtml += string.Format("{0},", _pCode);
                            _notfoundqtyhtml += string.Format("{0},", _pQty);
                            oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
                            oItemClaItemInfo.Clarification_ID = 0;
                            oItemClaItemInfo.OrderID = Template_id;
                            oItemClaItemInfo.ProductDesc = _pCode;
                            oItemClaItemInfo.Quantity = _pQty;
                            oItemClaItemInfo.UserID = userid;
                            oItemClaItemInfo.Clarification_Type = "TEMP_ITEM_CHK";
                            objOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);
                      
                        }
                        else if (_substitute == "{~N/A~}")
                        {
                          
                            _notfoundstr += string.Format("{0},", _pCode);
                            oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
                            oItemClaItemInfo.Clarification_ID = 0;
                            oItemClaItemInfo.OrderID = Template_id;
                            oItemClaItemInfo.ProductDesc = _pCode;
                            oItemClaItemInfo.Quantity = _pQty;
                            oItemClaItemInfo.UserID = userid;
                            oItemClaItemInfo.Clarification_Type = "TEMP_ITEM_ERROR";
                            objOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);
                   
                        }
                        else
                        {
                            ProdAItem[i] = _substitute;
                            _pCode = _substitute;
                          
                            oItemInFo.ProductID = GetProductID(ProdAItem[i]);
          
                            oItemInFo.TemplateId = Template_id;
                            oItemInFo.Quantity = ObjHelperServices.CI(ProdAQty[i]);

                            
                            objOrderServices.AddOrderItemTemplateItem(oItemInFo);
                            

                        }

                    }
                    if (checkset > 0)
                    {
                        
                        oItemInFo.ProductID = GetProductID(ProdAItem[i]);
                        
                        
                        oItemInFo.TemplateId  = Template_id;
                        oItemInFo.Quantity = ObjHelperServices.CI(ProdAQty[i]);

                        objOrderServices.AddOrderItemTemplateItem(oItemInFo);
  
                    }


                }

            }

          
    

          
            //string enc = System.DateTime.Now.Millisecond.ToString();

            //if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
            //{
            //    Response.Redirect("OrderDetails.aspx?ORDER_ID=" + Convert.ToInt32(Session["ORDER_ID"]) + "&bulkorder=1&ViewOrder=View&" + enc, true);
            //}
            //else
            //{
            //    Response.Redirect("OrderDetails.aspx?bulkorder=1&" + enc, true);

            //}
             tbErrorItem = objOrderServices.GetOrder_Clarification_Items(Template_id, "TEMP_ITEM_ERROR");
             tbErrorChk = objOrderServices.GetOrder_Clarification_Items(Template_id, "TEMP_ITEM_CHK");


            if ((tbErrorItem != null && tbErrorItem.Rows.Count > 0) || (tbErrorChk != null && tbErrorChk.Rows.Count > 0) )
                Response.Redirect("ordertemplate.aspx?bulkorder=1&Tempid=" + Template_id, true);            
            else
                Response.Redirect("OrderTemplateList.aspx", true);
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            //objErrorHandler.CreateLog();
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
        if (ObjHelperServices.GetOptionValues("ENABLE ITEM SHIPPING").ToString().ToUpper() == "YES")
        {
            if (dsOItem != null)
            {
                foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                {
                    if (ObjHelperServices.CB(rItem["IS_SHIPPING"]) == 1)
                    {
                        ProdShippCost = ((ProductCost * ObjHelperServices.CDEC(rItem["PROD_SHIP_COST"])) / 100);
                    }
                }
            }
        }
        else
        {
            if ((objOrderServices.GetCurrentProductTotalCost(OID) + ProdApplyPrice) < ObjHelperServices.CDEC(ObjHelperServices.GetOptionValues("SHIPPING FREE").ToString()))
            {
                if (dsOItem != null)
                {
                    foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                    {
                        ProdShippCost = ((ProductCost * ObjHelperServices.CI(ObjHelperServices.GetOptionValues("SHIPPING CHARGE").ToString())) / 100);
                    }
                }
            }
            else
            {
                _IsShippingFree = true;
            }
        }
        return ObjHelperServices.CDEC(ObjHelperServices.FixDecPlace(ProdShippCost));
    }
    private decimal GetMyPrice(int ProductID)
    {
        decimal retval = 0.00M;
        string userid = HttpContext.Current.Session["USER_ID"].ToString();
        //if (!string.IsNullOrEmpty(userid))
        //{
        //    string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
        //    oHelper.SQLString = sSQL;
        //    int pricecode = ObjHelperServices.CI(oHelper.GetValue("price_code"));

        //    string strquery = "";
        //    if (pricecode == 1)
        //    {
        //        strquery = string.Format("exec GetWESIncProductPrice {0},{1},{2}", ProductID, 1, HttpContext.Current.Session["USER_ID"]);
        //    }
        //    else
        //    {
        //        strquery = string.Format("exec GetWESProductPrice {0},{1},{2}", ProductID, 1, HttpContext.Current.Session["USER_ID"]);
        //    }

        //    DataSet DSprice = new DataSet();
        //    oHelper.SQLString = strquery;
        //    retval = Math.Round(Convert.ToDecimal(oHelper.GetValue("Numeric_Value")), 2);
        //}
        retval = objHelperDB.GetProductPrice(ProductID, 1, userid);
        return retval;
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
                    if (AItems[i] != "")
                    {
                        if (AItems[i].Contains(",") == true)
                        {
                            TempItems = Regex.Split(AItems[i].ToString(), ",");
                        }
                        else if (AItems[i].Contains("\t") == true)
                        {
                            TempItems = Regex.Split(AItems[i].ToString(), "\t");
                        }
                        else if (AItems[i].Contains(" ") == true)
                        {
                            while (AItems[i].ToString().Contains("  "))
                                AItems[i] = AItems[i].Replace("  ", " ");
                            TempItems = Regex.Split(AItems[i].ToString(), " ");
                        }
                        if ((TempItems != null) && (TempItems.Length >= 2) && (TempItems[0] != null) && (TempItems[1] != null) && (TempItems[0] != string.Empty) && (TempItems[1] != string.Empty) && (TempItems[0] != "") && (TempItems[1] != ""))
                        {
                            CPItemCode = CPItemCode + TempItems[0].ToString().Trim() + ",";
                            CPItemQty = CPItemQty + TempItems[1].ToString().Trim() + ",";

                            BProds = BProds + TempItems[0].ToString().Trim() + ",";
                            BQtys = BQtys + TempItems[1].ToString().Trim() + ",";


                            TempItems[0] = "";
                            TempItems[1] = "";
                            ItemCnt++;
                        }
                        else
                        {
                            HidtxtCnt.Value = "20";
                            txtCopyPaste.Rows = 20;
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

    protected void btnAddCart1_ServerClick(object sender, EventArgs e)
    {

    }


    protected void lnkbtnmore_Click(object sender, EventArgs e)
    {
        string hidproductcodes = HidItemCode2.Value;
        string hidquantitys = HidQty2.Value;
        if (hidproductcodes != null && hidproductcodes != "" && hidquantitys != null && hidquantitys != "")
        {

            itemcode = hidproductcodes.Substring(0, hidproductcodes.Length - 1);
            itemqty = hidquantitys.Substring(0, hidquantitys.Length - 1);
            CPItemCode = CPItemCode + itemcode + ",";
            CPItemQty = CPItemQty + itemqty + ",";

            DataSet tmpds = new DataSet();
            DataTable tmpdatatbl = new DataTable();
            tmpdatatbl.Columns.Add("productcode", typeof(string));
            tmpdatatbl.Columns.Add("quantity", typeof(string));

            string productcode = CPItemCode;
            string quantity = CPItemQty;

            string[] productcodes = productcode.Split(',');
            string[] quantitys = quantity.Split(',');

            for (int i = 0; i < productcodes.Length - 1; i++)
            {
                tmpdatatbl.Rows.Add(productcodes[i], quantitys[i]);

            }
            tmpds.Tables.Add(tmpdatatbl);
            if (tmpds != null && tmpds.Tables.Count > 0)
                HttpContext.Current.Session["linkmoredata"] = tmpds;
            else
                HttpContext.Current.Session["linkmoredata"] = null;

        }

        if (HidtxtCnt.Value == "")
            HidtxtCnt.Value = "20";
        else
        {
            if (Convert.ToInt32(HidtxtCnt.Value.ToString()) >= 50)
            {
                HidtxtCnt.Value = "50";
                txtCopyPaste.Rows = ObjHelperServices.CI(HidtxtCnt.Value.ToString()) - 8;
            }
            else
            {
                HidtxtCnt.Value = (Convert.ToInt32(HidtxtCnt.Value.ToString()) + 10).ToString();
                if (HidtxtCnt.Value == "40")
                {
                    txtCopyPaste.Rows = ObjHelperServices.CI(HidtxtCnt.Value.ToString()) - 7;
                }
                else if (HidtxtCnt.Value == "30")
                {
                    txtCopyPaste.Rows = ObjHelperServices.CI(HidtxtCnt.Value.ToString()) - 6;
                }
                else if (HidtxtCnt.Value == "50")
                {
                    txtCopyPaste.Rows = ObjHelperServices.CI(HidtxtCnt.Value.ToString()) - 8;
                }
            }
        }

        if (HidtxtCnt.Value != "")
            HttpContext.Current.Session["linkmoredatatxtcount"] = HidtxtCnt.Value.ToString();
    }
    //protected void lnkBtnLoadTemplate_Click(object sender, EventArgs e)
    //{
    //     DataSet tmpordds = new DataSet();  
    //    DataSet  tmpds = new DataSet();
    //        DataTable tmpdatatbl = new DataTable();
    //        decimal rc = 0;
    //        decimal tmp = 0;
            
    //        decimal finval =0;
    //    int userid = ObjHelperServices.CI(HttpContext.Current.Session["USER_ID"]);
    //    tmpordds = objOrderServices.GetOrderTemplateItem(userid);

            
    //        if (tmpordds!=null && tmpordds.Tables[0].Rows.Count>0)  
    //        {
    //            tmpdatatbl = tmpordds.Tables[0].DefaultView.ToTable(false, "PCode","Qty"); 
                
    //            tmpds.Tables.Add(tmpdatatbl);
    //        }

    //        if (tmpds != null && tmpds.Tables.Count > 0)
    //        {
    //              rc=tmpdatatbl.Rows.Count-1;
    //              if (rc % 10 != 0)
    //                  rc = (rc - rc % 10) + 10;
    //              else
    //                  rc = 10;
    //            HidtxtCnt.Value=rc.ToString();
    //            HttpContext.Current.Session["linkmoredata"] = tmpds;
    //        }
    //        else
    //        {
    //            HttpContext.Current.Session["linkmoredata"] = null;
    //            HidtxtCnt.Value="10";
    //        }

            


    //        //if (HidtxtCnt.Value == "")
    //        //    HidtxtCnt.Value = "20";
    //        //else
    //        //{
    //        //    if (Convert.ToInt32(HidtxtCnt.Value.ToString()) >= 50)
    //        //    {
    //        //        HidtxtCnt.Value = "50";
    //        //        txtCopyPaste.Rows = ObjHelperServices.CI(HidtxtCnt.Value.ToString()) - 8;
    //        //    }
    //        //    else
    //        //    {
    //        //        HidtxtCnt.Value = (Convert.ToInt32(HidtxtCnt.Value.ToString()) + 10).ToString();
    //        //        if (HidtxtCnt.Value == "40")
    //        //        {
    //        //            txtCopyPaste.Rows = ObjHelperServices.CI(HidtxtCnt.Value.ToString()) - 7;
    //        //        }
    //        //        else if (HidtxtCnt.Value == "30")
    //        //        {
    //        //            txtCopyPaste.Rows = ObjHelperServices.CI(HidtxtCnt.Value.ToString()) - 6;
    //        //        }
    //        //        else if (HidtxtCnt.Value == "50")
    //        //        {
    //        //            txtCopyPaste.Rows = ObjHelperServices.CI(HidtxtCnt.Value.ToString()) - 8;
    //        //        }
    //        //    }
    //        //}
        

    //    if (HidtxtCnt.Value != "")
    //        HttpContext.Current.Session["linkmoredatatxtcount"] = HidtxtCnt.Value.ToString();
    //}
    //private DataSet GetDataSet(string SQLQuery)
    //{
    //    DataSet ds = new DataSet();
    //    SqlDataAdapter da = new SqlDataAdapter(SQLQuery, oCon.ConnectionString.ToString().Substring(oCon.ConnectionString.ToString().IndexOf(';') + 1));
    //    da.Fill(ds, "generictable");
    //    return ds;
    //}

    protected void btnResetCart_Click(object sender, EventArgs e)
    {
        this.Server.Transfer("BulkOrder.aspx");

    }

    public void UploadButton_Click(object sender, EventArgs e)
    {
        // RegularExpressionValidator1.ErrorMessage = null;
        string fileName = Path.GetFileName(FileUploadControl.PostedFile.FileName);
        HttpContext.Current.Session["fileName"] = fileName;
        string fileExtension = Path.GetExtension(FileUploadControl.PostedFile.FileName);
        double dblMaxFileSize = Convert.ToDouble(ConfigurationManager.AppSettings["MaxFileSize"]);
        int intFileSize = FileUploadControl.PostedFile.ContentLength;  // Here the file size is obtained in bytes
        double dblFileSizeinKB = intFileSize / 1024.0; // We convert the file size into kilobytes
        int QuickOrderBoxCount = Convert.ToInt16(ConfigurationManager.AppSettings["QuickOrderBoxCount"]);


        if (fileExtension == ".csv")
        {
            ConnectCSV();
            return;
        }
        else
        {
            if ((FileUploadControl.HasFile))
            {
                if (dblFileSizeinKB < 1024)
                {
                    try
                    {

                        OleDbConnection conn = new OleDbConnection();
                        OleDbCommand cmd = new OleDbCommand();
                        OleDbDataAdapter da = new OleDbDataAdapter();
                        DataSet ds = new DataSet();
                        string query = null;
                        string connString = "";
                        string strFileName = Path.GetFileName(FileUploadControl.PostedFile.FileName);
                        string strFileType = System.IO.Path.GetExtension(FileUploadControl.FileName).ToString().ToLower();

                        if (strFileType == ".xls" || strFileType == ".xlsx")
                        {


                            FileUploadControl.SaveAs(Server.MapPath("~/Import/ExcelImport_" + user_id + strFileType));

                        }
                        //else if (strFileType == ".csv")
                        //{
                        //    //FileUploadControl.SaveAs(Server.MapPath("~/ExcelImport_" + user_id + strFileType ));
                        //    ConnectCSV();
                        //    return;
                        //}

                        else
                        {
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Only excel files allowed');", true);
                            return;
                        }



                        string strNewPath = Server.MapPath("~/Import/ExcelImport_" + user_id + strFileType);
                        // string strNewPath1 = Server.MapPath("~/ExcelImport1_" + user_id + " + strFileType +");

                        //  if (strFileType.Trim() == ".csv")
                        //  {
                        // connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                        //     connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                        //   }
                        //  if (strFileType.Trim() == ".xlsx" || strFileType.Trim() == ".xls")
                        //  {
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        //  }

                        //  if (strFileType == ".xls" || strFileType == ".xlsx")
                        //  {
                        query = "SELECT * FROM [Sheet1$]";
                        //  }
                        //if (strFileType == ".csv")
                        //{
                        //    query = "SELECT * FROM [Sheet1$]";
                        //}
                        conn = new OleDbConnection(connString);
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        cmd = new OleDbCommand(query, conn);
                        da = new OleDbDataAdapter(cmd);

                        System.Data.DataTable dt = null;
                        dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        string sheetname = dt.Rows[0][2].ToString();
                        if (sheetname == "Sheet1$")
                        {
                            ds = new DataSet();
                            da.Fill(ds);
                            da.Dispose();
                            conn.Close();
                            conn.Dispose();

                            int rowcount = ds.Tables[0].Rows.Count;
                            int rowcountnew = ds.Tables.Count;
                            if (rowcount <= QuickOrderBoxCount)
                            {
                                for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                                {
                                    string sqlwordcol1 = ds.Tables[0].Rows[i].ItemArray[0].ToString().Trim();
                                    string sqlwordcol2 = ds.Tables[0].Rows[i].ItemArray[1].ToString().Trim();
                                    string containword = sqlwordcol1;
                                    if (sqlwordcol1.Contains("select") == true || sqlwordcol1.Contains("delete") == true || sqlwordcol1.Contains("insert") == true || sqlwordcol1.Contains("update") == true || sqlwordcol1 == "select *" || sqlwordcol1 == "select*" || sqlwordcol1 == "select * from" || sqlwordcol1 == "delete  from" || sqlwordcol1 == "insert into")
                                    {
                                        Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert(' Excel File Not Valid Format.');", true);
                                        ds = null;
                                        HttpContext.Current.Session["fileData"] = null;
                                        return;
                                    }
                                    else if (sqlwordcol2.Contains("select") == true || sqlwordcol2.Contains("delete") == true || sqlwordcol2.Contains("insert") == true || sqlwordcol2.Contains("update") == true || sqlwordcol2 == "select *" || sqlwordcol2 == "select*" || sqlwordcol2 == "select * from" || sqlwordcol2 == "delete  from" || sqlwordcol2 == "insert into")
                                    {
                                        Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert(' Excel File Not Valid Format.');", true);
                                        ds = null;
                                        HttpContext.Current.Session["fileData"] = null;
                                        return;
                                    }
                                    else
                                    {
                                        if (ds != null && ds.Tables.Count > 0)
                                            HttpContext.Current.Session["fileData"] = ds;
                                        else
                                            HttpContext.Current.Session["fileData"] = null;
                                    }
                                }
                            }
                            else
                            {
                                //StatusLabel.Text = "Maximun Excel File Allowed 30 Rows.";
                                //StatusLabel.ForeColor = System.Drawing.Color.Red;
                                //StatusLabel.Visible = true;
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Maximun Excel File Allowed 99 Rows.');", true);
                                return;
                            }
                        }
                        else
                        {
                            //StatusLabel.Text = "First Sheet Name must contain Sheet1";
                            //StatusLabel.ForeColor = System.Drawing.Color.Red;
                            //StatusLabel.Visible = true;
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('First Sheet Name must contain Sheet1.');", true);
                            ds = null;
                            HttpContext.Current.Session["fileData"] = null;
                            return;
                        }




                        da.Dispose();
                        conn.Close();
                        conn.Dispose();
                    }
                    catch (System.NullReferenceException ex)
                    {
                        StatusLabel.Text = "Error: " + ex.Message;
                        StatusLabel.ForeColor = System.Drawing.Color.Red;
                        StatusLabel.Visible = true;
                    }
                }

                else
                {
                    StatusLabel.Text = "Maximun file size allowed 1024KB";
                    StatusLabel.ForeColor = System.Drawing.Color.Red;
                    StatusLabel.Visible = true;
                }


            }
            else
            {
                StatusLabel.Text = "Please select an excel file first";
                StatusLabel.ForeColor = System.Drawing.Color.Red;
                StatusLabel.Visible = true;
            }

        }
        btnAddCart1.Focus();
    }


    private void ConnectCSV()
    {
        string fileName = Path.GetFileName(FileUploadControl.PostedFile.FileName);
        HttpContext.Current.Session["fileName"] = fileName;
        string fileExtension = Path.GetExtension(FileUploadControl.PostedFile.FileName);
        double dblMaxFileSize = Convert.ToDouble(ConfigurationManager.AppSettings["MaxFileSize"]);
        int intFileSize = FileUploadControl.PostedFile.ContentLength;  // Here the file size is obtained in bytes
        double dblFileSizeinKB = intFileSize / 1024.0; // We convert the file size into kilobytes
        int QuickOrderBoxCount = Convert.ToInt16(ConfigurationManager.AppSettings["QuickOrderBoxCount"]);

        try
        {
            string strFileType = System.IO.Path.GetExtension(FileUploadControl.FileName).ToString().ToLower();
            string target = Server.MapPath("~/Import/ExcelImport_" + user_id + strFileType);







            if (FileUploadControl.HasFile)
            {

                FileUploadControl.SaveAs(target);

                ////string connString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Text;", System.IO.Path.GetDirectoryName(target + "\\" + FileUploadControl.FileName));
                // string connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Text;", System.IO.Path.GetDirectoryName(target + "\\" + FileUploadControl.FileName));
                // string cmdString = string.Format("SELECT * FROM {0}", System.IO.Path.GetFileName(target + "\\" + FileUploadControl.FileName));
                string connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Text;", System.IO.Path.GetDirectoryName(target));
                string cmdString = string.Format("SELECT * FROM {0}", System.IO.Path.GetFileName(target));
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(cmdString, connString);

                DataSet ds = new DataSet();

                dataAdapter.Fill(ds);
                dataAdapter.Dispose();
                int rowcount = ds.Tables[0].Rows.Count;
                int rowcountnew = ds.Tables.Count;
                if (rowcount <= QuickOrderBoxCount)
                {
                    for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                    {

                        string sqlwordcol1 = ds.Tables[0].Rows[i].ItemArray[0].ToString().Trim();
                        string sqlwordcol2 = ds.Tables[0].Rows[i].ItemArray[1].ToString().Trim();
                        if (sqlwordcol1.Contains("select") == true || sqlwordcol1.Contains("delete") == true || sqlwordcol1.Contains("insert") == true || sqlwordcol1.Contains("update") == true || sqlwordcol1 == "select *" || sqlwordcol1 == "select*" || sqlwordcol1 == "select * from" || sqlwordcol1 == "delete  from" || sqlwordcol1 == "insert into")
                        {
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert(' Excel File Not Valid Format.');", true);
                            ds = null;
                            HttpContext.Current.Session["fileData"] = null;
                            return;
                        }
                        else if (sqlwordcol2.Contains("select") == true || sqlwordcol2.Contains("delete") == true || sqlwordcol2.Contains("insert") == true || sqlwordcol2.Contains("update") == true || sqlwordcol2 == "select *" || sqlwordcol2 == "select*" || sqlwordcol2 == "select * from" || sqlwordcol2 == "delete  from" || sqlwordcol2 == "insert into")
                        {
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert(' Excel File Not Valid Format.');", true);
                            ds = null;
                            HttpContext.Current.Session["fileData"] = null;
                            return;
                        }
                        else
                        {
                            if (ds != null && ds.Tables.Count > 0)
                                HttpContext.Current.Session["fileData"] = ds;
                            else
                                HttpContext.Current.Session["fileData"] = null;
                        }
                    }

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Maximun Excel File Allowed 99 Rows.');", true);
                    return;
                }
            }
        }
        catch (Exception e)
        {

        }


    }
    protected void LinkButton_Click(object sender, EventArgs e)
    {

        ////Response.ContentType = "xls";
        ////Response.AppendHeader("Content-Disposition", "attachment; filename=Sample.xls");
        ////Response.TransmitFile(Server.MapPath("~/Export/Sample.xls"));
        ////Response.End();


        try
        {

            string fileName = ConfigurationManager.AppSettings["MyFileName"].ToString();
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            //Response.AddHeader("Content-Type", "application/Excel");
            //Response.ContentType = "application/vnd.xls";
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            Response.AddHeader("Content-Type", "application/Excel");
            Response.ContentType = "application/vnd.xls";
            //  Response.TransmitFile(Server.MapPath("~/Export/Sample.xls"));
            Response.TransmitFile(Server.MapPath("~/Export/" + fileName));
            Response.End();
        }
        catch (Exception ex)
        {
            // objErrorHandler.ErrorMsg = ex;
        }



    }

    protected void btndownload_Click(object sender, EventArgs e)
    {


        ////Response.ContentType = "xls";
        ////Response.AppendHeader("Content-Disposition", "attachment; filename=Sample.xls");
        ////Response.TransmitFile(Server.MapPath("~/Export/Sample.xls"));
        ////Response.End();


        try
        {

            string fileName = ConfigurationManager.AppSettings["MyFileName"].ToString();
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            //Response.AddHeader("Content-Type", "application/Excel");
            //Response.ContentType = "application/vnd.xls";
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            Response.AddHeader("Content-Type", "application/Excel");
            Response.ContentType = "application/vnd.xls";
            //  Response.TransmitFile(Server.MapPath("~/Export/Sample.xls"));
            Response.TransmitFile(Server.MapPath("~/Export/" + fileName));
            Response.End();
        }
        catch (Exception ex)
        {
            // objErrorHandler.ErrorMsg = ex;
        }



    }

    protected void btnSaveasOrdTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            msgOTAlert.Hide();
            msgAlert.Hide();

            SettextData(TQtys , TProds);

            TxtdivTemplateName.Text = "";
            TxtdivDesc.Text = "";


            ShowOTAlertMessageBox();
        }
        catch (Exception ex)
        {
        }
    }

    private void ShowOTAlertMessageBox()
    {
        lbldiverror.Text  = "";
        msgOTAlert.ID = "divOrderTemplate";
        msgOTAlert.PopupControlID = "plnOrderTemplate";
        msgOTAlert.BackgroundCssClass = "modalBackground";
        msgOTAlert.TargetControlID = "btnHiddenTestPopupExtender";
        msgOTAlert.DropShadow = true;
        msgOTAlert.CancelControlID = "btnOTClose";
        this.plnOrderTemplate.Controls.Add(msgOTAlert);
        this.msgOTAlert.Show();
    }
    protected void btnSaveOrdTemplate(object sender, EventArgs e)
    {
        try
        {
            isSave = true;

          
            int userid = 0;
            if (txtcnt != null && txtcnt != "" && ItemCnt == 0)
            {
                txtCount = ObjHelperServices.CI(txtcnt.ToString());
            }
            else
            {
                txtCount = ItemCnt;
            }
            string[] ProdAQty = new string[txtCount];
            string[] ProdAItem = new string[txtCount];
            string[] ItemQty = new string[txtCount];

            if (TempProdQtys == null || TempProdItems == null)
                return;

            SettextData(TQtys , TProds );


            if (TxtdivTemplateName.Text.Trim() == "")
            {
                msgOTAlert.Hide();
                msgAlert.Hide();

                ShowOTAlertMessageBox();
                lbldiverror.Text = "Enter Template Name";
                return;
            }

            ProdAQty = Regex.Split(TempProdQtys, ",");
            ProdAItem = Regex.Split(TempProdItems, ",");


            string Pqtycheck = "";
            Int32 Num;
            for (int i = 0; i < ProdAQty.Length - 1; i++)
            {
                Pqtycheck = ProdAQty[i].ToString().Trim();
                bool isNum = Int32.TryParse(Pqtycheck, out Num);

                if (isNum == false)
                {
                    msgOTAlert.Hide();
                    msgAlert.Hide();
                  
                    ShowOTAlertMessageBox();
                    lbldiverror.Text = "Incorrect Code or Invalid QTY";
                    return;                  
                }
            }

            string _notfoundstr = "";
            string _notfoundstrhtml = "";
            string _minqty = "";
            string _minqtyhtml = "";
            string _maxqty = "";
            string _maxqtyhtml = "";
            string _notfoundqtyhtml = "";
            bool itemcheck = false;
            string PCodeAll = "";
            DataTable protbl = null;

            decimal ProdTotalPrice = 0;
            decimal OrderTotal = 0;
            decimal TotalShipCost = 0;
            DataTable dt = null;
            int tmpcount = 0;
            for (int i = 0; i < ProdAItem.Length; i++)
            {
                PCodeAll = PCodeAll + "'" + ProdAItem[i].ToString().Trim() + "',";

            }
            if (PCodeAll != "")
            {
                PCodeAll = PCodeAll.Substring(0, PCodeAll.Length - 1) + "";
                protbl = (DataTable)objHelperDB.GetGenericPageDataDB(PCodeAll, "GET_BULKORDER_INVENTORY_COUNT_ALL", HelperDB.ReturnType.RTTable);

            }
            tmpcount = PCodeAll.Split(',').Length - 1;

            //if (flgcprowcheck == true)
            //{

            //    if (protbl == null && flgcprowcheck == true || tmpcount != protbl.Rows.Count && flgcprowcheck == true)
            //    {
            //        msgOTAlert.Hide();
            //        msgAlert.Hide();
                  
            //        ShowOTAlertMessageBox();
            //        lbldiverror.Text = "Incorrect Code or Invalid QTY";
                    
            //        return;   

            //    }
            //}


            int Template_id = 0;
            string OrderStatus = "";
            OrderServices.OrderTemplateInfo oTemInFo = new OrderServices.OrderTemplateInfo();
            OrderServices.OrderTemplateItemInfo oItemInFo = new OrderServices.OrderTemplateItemInfo();
            OrderServices.Order_Calrification_ItemInfo oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
            userid = ObjHelperServices.CI(Session["USER_ID"]);
            string proids = "";

            oTemInFo.TemplateName = TxtdivTemplateName.Text;
            oTemInFo.Description = TxtdivDesc.Text;
            oTemInFo.UserID = userid;
            oTemInFo.CompanyID = HttpContext.Current.Session["COMPANY_ID"].ToString();   
            oTemInFo.TemplateId = 0;
            int isexists = objOrderServices.GetOrderTemplateNameExists(TxtdivTemplateName.Text, oTemInFo.UserID, oTemInFo.TemplateId);
            if (isexists > 0)
            {          
                msgOTAlert.Hide();
                msgAlert.Hide();
              
                ShowOTAlertMessageBox();
                lbldiverror.Text = "Template Name Already Exists";
                return;   
            }

            DataTable tbErrorItem = objOrderServices.GetOrder_Clarification_Items(Template_id, "TEMP_ITEM_ERROR");
            DataTable tbErrorChk = objOrderServices.GetOrder_Clarification_Items(Template_id, "TEMP_ITEM_CHK");

            if ((tbErrorItem != null && tbErrorItem.Rows.Count > 0) || (tbErrorChk != null && tbErrorChk.Rows.Count > 0))
            {
                string Script = "alert('Please review and correct Order Clarifications / Errors before proceeding to Save Template!');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Tempscript2", Script, true);
                return;
            }

            //if ((HttpContext.Current.Session["TEMPLATE_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["TEMPLATE_ID"]) > 0))
            //{
            //    oTemInFo.TemplateId = Convert.ToInt32(Session["TEMPLATE_ID"]);
            //}
            //else
            //    oTemInFo.TemplateId = 0;

            Template_id = objOrderServices.AddOrderItemTemplate(oTemInFo);


            //objOrderServices.RemoveOrderTemplate(userid); 

            for (int i = 0; i < ProdAItem.Length; i++)
            {
                int checkset = -1;
                string _pCode = ProdAItem[i].ToString().Trim();
                decimal _pQty = string.IsNullOrEmpty(_pCode.Trim()) ? 0 : Convert.ToDecimal(ProdAQty[i]);

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


                        string _substitute = FindSubstitute(_pCode);


                        if (_substitute == "{~MI~}")
                        {
                            _notfoundstrhtml += string.Format("{0},", _pCode);
                            _notfoundqtyhtml += string.Format("{0},", _pQty);

                            oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
                            oItemClaItemInfo.Clarification_ID = 0;
                            oItemClaItemInfo.OrderID = Template_id;
                            oItemClaItemInfo.ProductDesc = _pCode;
                            oItemClaItemInfo.Quantity = _pQty;
                            oItemClaItemInfo.UserID = userid;
                            oItemClaItemInfo.Clarification_Type = "TEMP_ITEM_CHK";
                            objOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);
                      

                        }
                        else if (_substitute == "{~N/A~}")
                        {

                            _notfoundstr += string.Format("{0},", _pCode);
                            oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
                            oItemClaItemInfo.Clarification_ID = 0;
                            oItemClaItemInfo.OrderID = Template_id;
                            oItemClaItemInfo.ProductDesc = _pCode;
                            oItemClaItemInfo.Quantity = _pQty;
                            oItemClaItemInfo.UserID = userid;
                            oItemClaItemInfo.Clarification_Type = "TEMP_ITEM_ERROR";
                            objOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);

                        }
                        else
                        {
                            ProdAItem[i] = _substitute;
                            _pCode = _substitute;

                            oItemInFo.ProductID = GetProductID(ProdAItem[i]);

                            oItemInFo.TemplateId = Template_id;
                            oItemInFo.Quantity = ObjHelperServices.CI(ProdAQty[i]);


                            objOrderServices.AddOrderItemTemplateItem(oItemInFo);


                        }

                    }
                    if (checkset > 0)
                    {

                        oItemInFo.ProductID = GetProductID(ProdAItem[i]);


                        oItemInFo.TemplateId = Template_id;
                        oItemInFo.Quantity = ObjHelperServices.CI(ProdAQty[i]);

                        objOrderServices.AddOrderItemTemplateItem(oItemInFo);

                    }


                }

            }





            //string enc = System.DateTime.Now.Millisecond.ToString();

            //if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
            //{
            //    Response.Redirect("OrderDetails.aspx?ORDER_ID=" + Convert.ToInt32(Session["ORDER_ID"]) + "&bulkorder=1&ViewOrder=View&" + enc, true);
            //}
            //else
            //{
            //    Response.Redirect("OrderDetails.aspx?bulkorder=1&" + enc, true);

            //}

             tbErrorItem = objOrderServices.GetOrder_Clarification_Items(Template_id, "TEMP_ITEM_ERROR");
             tbErrorChk = objOrderServices.GetOrder_Clarification_Items(Template_id, "TEMP_ITEM_CHK");


            if ((tbErrorItem != null && tbErrorItem.Rows.Count > 0) || (tbErrorChk != null && tbErrorChk.Rows.Count > 0))
                Response.Redirect("ordertemplate.aspx?bulkorder=1&Tempid=" + Template_id, true);
            else
                ShowAlertMessageBox("Successfully saved");

           
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            //objErrorHandler.CreateLog();
        }
        
    }

    //private int GetStockStatus(int ProductID)
    //{
    //    int Retval = -1;
    //    try
    //    {
    //        //string sSQL = string.Format("select PROD_STK_STATUS_DSC from WESTB_PRODUCT_ITEM WHERE PRODUCT_ID = {0}", ProductID);
    //        //objHelperService.SQLString = sSQL;
    //        //Retval = objHelperService.GetValue("PROD_STK_STATUS_DSC").ToString().Replace("_", " ");
    //        DataTable objrbl = (DataTable)objHelperDB.GetGenericDataDB(ProductID.ToString(), "GET_SINGLE_PRODUCT_INVENTORY_BULKORDER", HelperDB.ReturnType.RTTable);
    //        if (objrbl != null)
    //        {
    //            string strstockstatus = objrbl.Rows[0]["PROD_STOCK_STATUS"].ToString();

    //            if (strstockstatus == "false" || strstockstatus == "False")
    //                Retval = 0;
    //            else
    //                Retval = -1;
    //        }
    //        else
    //            Retval = -1;
    //    }
    //    catch (Exception e)
    //    {
    //        objErrorHandler.ErrorMsg = e;
    //        objErrorHandler.CreateLog();
    //    }
    //    return Retval;
    //}

    private void AddOrderTemplateProductItem(int prod_id,int qty,int template_id)
    {
        OrderServices.OrderTemplateItemInfo oItemInFo = new OrderServices.OrderTemplateItemInfo();
        oItemInFo.ProductID = prod_id;


        oItemInFo.TemplateId = template_id;
        oItemInFo.Quantity = qty;

        objOrderServices.AddOrderItemTemplateItem(oItemInFo);
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
    private void ShowClarifyAlertMessageBox(string msg)
    {
        lblitemClarify.Text = msg;
        ClarifyAlert.ID = "DivItemClarify";
        ClarifyAlert.PopupControlID = "PnlItemClarify";
        ClarifyAlert.BackgroundCssClass = "modalBackground";
        ClarifyAlert.TargetControlID = "btnHiddenTestPopupExtender";
        ClarifyAlert.DropShadow = true;
        ClarifyAlert.CancelControlID = "btnok";
        ClarifyAlert.BehaviorID = "BtnItemClarify";
        this.PnlItemClarify.Controls.Add(ClarifyAlert);

        this.ClarifyAlert.Show();
    }
    protected void btnOTClose_click(object sender, EventArgs e)
    {
        msgAlert.Hide();
        msgOTAlert.Hide();
        plnOrderTemplate.Visible = false;
        pnlAlert.Visible = false;
    }
   
}

