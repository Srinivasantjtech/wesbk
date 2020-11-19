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
using System.Web.Services;
using System.Web.Configuration;
using System.Data.OleDb;
using System.IO;
using Microsoft.Office.Interop;
using System.Drawing;
using Newtonsoft.Json;
using System.Globalization;
using System.Reflection;

public partial class UC_byproduct : System.Web.UI.UserControl
{
    TBWTemplateEngine objtemp = new TBWTemplateEngine("", "", "");
    ConnectionDB conStr = new ConnectionDB();
    DataTable dtdata = new DataTable();
    DataTable dtCommon = new DataTable();
    int i = 0;
    AjaxControlToolkit.ModalPopupExtender modalPop = new AjaxControlToolkit.ModalPopupExtender();

    readonly PagedDataSource _pgsource = new PagedDataSource();
    int _firstIndex, _lastIndex;
    private int _pageSize = 10;

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

    string myString = "";
    string dupString = "";
    string duplicateString = "";
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
            return ProdImgPath + "/" + path.ToString().Replace("\\", "/");

        }
        else
        {
            return ProdImgPath + "/images/noimage.gif";
        }
    }
    protected void IBClose_Click(object sender, EventArgs e)
    {
        ClarifyAlert.Hide();

    }
    private int CurrentPage
    {
        get
        {
            if (ViewState["CurrentPage"] == null)
            {
                return 0;
            }
            return ((int)ViewState["CurrentPage"]);
        }
        set
        {
            ViewState["CurrentPage"] = value;
        }
    }

    protected void dgtemplate_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "PathUpdate")
        {
            HttpContext.Current.Session["dtProdList"] = null;

            string id = e.CommandArgument.ToString();
            HttpContext.Current.Session["bindid"] = id;
           
            GetDataFromDb(id);
            ConnectionDB objconn = new ConnectionDB();
            var con = new SqlConnection(objconn.ConnectionString);
            con.Open();
            var da = new SqlDataAdapter("select FileName  from [TBWC_MYPRODUCT] where Id=" + id + " ", con);
            var dt = new DataTable();
            da.Fill(dt);
            con.Close();
            lbltempname.Text = dt.Rows[0][0].ToString();
            HttpContext.Current.Session["TemplateName"] = lbltempname.Text;
        }



    }
    protected void Page_Load(object sender, EventArgs e)
    {
        
        cmdAction.ServerClick += new EventHandler(BtnAddtoCart1_Click);
       
        if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
        {
            OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]);
        }
        if (Request.QueryString["GetData"] == null)
        {


            if (!IsPostBack)
            {
                Literal1.Text = "";
                Literal2.Text = "";
                Literal3.Text = "";
                StatusLabel1.Visible = false;
                GetData_DDL(user_id);
                 //BindDataIntoRepeater(); 

            }
        }
        else
        {
            if (Request.QueryString["GetData"] == "true")
            {

                GetDataFromDb(HttpContext.Current.Session["bindid"].ToString());

                PropertyInfo isreadonly =
    typeof(System.Collections.Specialized.NameValueCollection).GetProperty(
    "IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                // make collection editable
                isreadonly.SetValue(this.Request.QueryString, false, null);
                // remove
                Request.QueryString.Remove("GetData");

                lbltempname.Text = HttpContext.Current.Session["TemplateName"].ToString();
            }
           else if (!IsPostBack)
            {
                Literal1.Text = "";
                Literal2.Text = "";
                Literal3.Text = "";
                GetData_DDL(user_id);
                //   BindDataIntoRepeater(); 

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
                        objOrderServices.RemoveItem(PrdId.ToString(), order_id, objHelperServices.CI(HttpContext.Current.Session["USER_ID"]), order_item_id.ToString());
                    }
                    OrderDetails frmord = new OrderDetails();
                    frmord.AddOrderItem(order_id, objHelperServices.CI(HttpContext.Current.Session["USER_ID"]), (int)TotQty, PrdId);
                    oOrdInfo.OrderID = OrderID;
                    objOrderServices.UpdateOrderPrice(oOrdInfo, true);
                }

            }

            BindDataIntoRepeater();

        }
        if (Request.QueryString["LeaveProd_id"] != null && (Request.QueryString["ORDER_ID"] != null))
        {
            int order_id = Convert.ToInt32(Request.QueryString["ORDER_ID"].ToString());
            int PrdId = objHelperServices.CI(Request.QueryString["LeaveProd_id"].ToString());
            string LeaveDuplicateProds = "";
            if (HttpContext.Current.Session["LeaveDuplicateProds"] != null && HttpContext.Current.Session["LeaveDuplicateProds"].ToString() != "")
            {
                LeaveDuplicateProds = HttpContext.Current.Session["LeaveDuplicateProds"].ToString();

                if (LeaveDuplicateProds.Contains("-" + PrdId + "-") == false && PrdId > 0)
                    LeaveDuplicateProds = LeaveDuplicateProds + ",-" + PrdId + "-";
            }
            else
                LeaveDuplicateProds = "-" + PrdId + "-";

            HttpContext.Current.Session["LeaveDuplicateProds"] = LeaveDuplicateProds;
            BindDataIntoRepeater();
        }
        if (Request["SelPid"] != null)
        {
            if (Request["SelPid"] != "" && Request["SelPid"] != "AllProd")
            {
                int OrderId = 0;
                if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
                {
                    OrderId = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]);
                }
                else
                {
                    OrderId = objOrderServices.GetOrderID(objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString()), OpenOrdStatusID);
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
                            objOrderServices.RemoveItem(PrdId.ToString(), OrderId, objHelperServices.CI(HttpContext.Current.Session["USER_ID"]), order_item_id.ToString());
                        }
                    }
                }
                oOrdInfo.OrderID = OrderId;
                objOrderServices.UpdateOrderPrice(oOrdInfo, true);
                BindDataIntoRepeater();
            }
            else if (Request["SelPid"] == "AllProd")
            {
                int OrderId = 0;
                if (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View"))
                {
                    OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]);
                }
                else
                {
                    OrderId = objOrderServices.GetOrderID(objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString()), OpenOrdStatusID);
                }

                int len = objOrderServices.GetOrderItemCount(OrderId);
                DataSet ds = new DataSet();
                ds = objOrderServices.GetOrderItems(OrderId);

                if (len > 0)
                {

                    objOrderServices.RemoveItem(Request["SelPid"], OrderId, objHelperServices.CI(HttpContext.Current.Session["USER_ID"]), "");
                    oOrdInfo.OrderID = OrderId;
                    objOrderServices.UpdateOrderPrice(oOrdInfo, true);

                }
                BindDataIntoRepeater();
            }
        }

    }
    protected void BindGrid_Click(object sender, EventArgs e)
    {
        HyperLink btnbindgrid = (HyperLink)sender;
        GridViewRow row = (GridViewRow)btnbindgrid.NamingContainer;
        string id = Gridprodlst.DataKeys[row.RowIndex].Value.ToString();
        GetDataFromDb(id);
    }
    protected void BtnAddtoCart_Click(object sender, EventArgs e)
    {
        ImageButton lbtn = (ImageButton)sender;
        GridViewRow row = (GridViewRow)lbtn.NamingContainer;
        TextBox txtqty = Gridprodlst.Rows[row.RowIndex].FindControl("txtqty") as TextBox;


        if (row != null)
        {
            if (HttpContext.Current.Session["txtqty"] != null)
            {
                HttpContext.Current.Session["txtqty"] = HttpContext.Current.Session["txtqty"] + "," + txtqty.Text;
            }
            else
            {

                HttpContext.Current.Session["txtqty"] = txtqty.Text;
            }

            if (HttpContext.Current.Session["txtcode"] != null)
            {
                HttpContext.Current.Session["txtcode"] = HttpContext.Current.Session["txtcode"] + "," + Gridprodlst.DataKeys[row.RowIndex].Value.ToString();
            }
            else
            {

                HttpContext.Current.Session["txtcode"] = Gridprodlst.DataKeys[row.RowIndex].Value.ToString();
            }

            AddToCart(txtqty.Text, Gridprodlst.DataKeys[row.RowIndex].Value.ToString());

        }

    }

    protected void BtnAddtoCart1_Click(object sender, EventArgs e)
    {
        try
        {
            myString = "";
            Literal1.Text = Literal2.Text = Literal3.Text = myString;
            foreach (GridViewRow row in Gridprodlst.Rows)
            {
                TextBox txtqty = Gridprodlst.Rows[row.RowIndex].FindControl("txtqty") as TextBox;
                AddToCart(txtqty.Text, Gridprodlst.DataKeys[row.RowIndex].Value.ToString());

            }
            myString = "<span id=\"rupspan\" style=\"font-family:rupee;font-size:16px;font-wieght:bold;\"> " +"" + "  </span>";

            Literal1.Text = Literal2.Text = Literal3.Text = myString;
        }
        catch (Exception ex)
        {


        }

    }
    private decimal GetMyPrice_Exc(int ProductID, int qty)
    {
        decimal retval = 0.00M;
        string userid = HttpContext.Current.Session["USER_ID"].ToString();

        retval = objHelperDB.GetProductPrice_Exc(ProductID, qty, userid);
        return retval;
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
                    } ////// i call you swichh off 
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
    public void AddToCart(string TempProdQtys, string TempProdItems) //srini add 10/15
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
            oOrdInfo.UserID = ObjHelperServices.CI(HttpContext.Current.Session["USER_ID"]);
            //OrderID = objOrderServices.GetOrderID(oOrdInfo.UserID, OpenOrdStatusID);

            //string CustomerType = objUserServices.GetCustomerType(oOrdInfo.UserID);
            if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ViewOrder"] != null && HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")))
            {
                OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]);
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
                    OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]);
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
                                    StrStockStatus = drow["STOCK_STATUS"].ToString().Replace("_", " ");
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
                                        StrProductStatusSub = objProductServices.GetStockStatusDesc(StrStockStatus, Convert.ToInt32((drow["PROD_STOCK_STATUS"].ToString())), Convert.ToInt32((drow["PROD_STOCK_FLAG"].ToString())), oOrdInfo.UserID, ProdCodeId);
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
                        else if (checksetsub == 1 && StrProductStatusSub == 0)
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
                        else if (checksetsub == 1 && StrProductStatusSub == 1)
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

                                    objErrorHandler.CreateLog("pCode" + _pCode + "OrderID" + OrderID + "userid" + oOrdInfo.UserID + "EbayBlock");
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
            //string enc = System.DateTime.Now.Millisecond.ToString();

            //if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
            //{
            //    Response.Redirect("OrderDetails.aspx?ORDER_ID=" + Convert.ToInt32(Session["ORDER_ID"]) + "&bulkorder=1&ViewOrder=View&" + enc, true);
            //}
            //else
            //{
            //    Response.Redirect("OrderDetails.aspx?bulkorder=1&" + enc, true);

            //}


        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            //objErrorHandler.CreateLog();
        }
    }
    public void UploadButton_Click(object sender, EventArgs e)
    {
        HttpContext.Current.Session["dtProdList"] = null;
        lbldiverror.Text = "";
        // RegularExpressionValidator1.ErrorMessage = null;
        if (txtName.Text == "")
        {
            StatusLabel.Text = "Please Enter Template Name";
            StatusLabel.ForeColor = System.Drawing.Color.Red;
            StatusLabel.Visible = true;
            txtName.Focus();
            return;
        }
        StatusLabel.Visible = false;
        string fileName = Path.GetFileName(FileUploadControl.PostedFile.FileName);
        HttpContext.Current.Session["fileName"] = fileName;
        string fileExtension = Path.GetExtension(FileUploadControl.PostedFile.FileName);
        double dblMaxFileSize = Convert.ToDouble(ConfigurationManager.AppSettings["MaxFileSize"]);
        int intFileSize = FileUploadControl.PostedFile.ContentLength;  // Here the file size is obtained in bytes
        double dblFileSizeinKB = intFileSize / 1024.0; // We convert the file size into kilobytes
        int QuickOrderBoxCount = Convert.ToInt16(ConfigurationManager.AppSettings["QuickOrderBoxCount"]);

        if ((FileUploadControl.HasFile))
        {
            if (FileUploadControl.PostedFile.ContentLength < 20728650)
            {
                string strFileName = Path.GetFileName(FileUploadControl.PostedFile.FileName);
                string strFileType = System.IO.Path.GetExtension(FileUploadControl.FileName).ToString().ToLower();
                HttpContext.Current.Session["strFileName"] = strFileName;
                HttpContext.Current.Session["strFileType"] = strFileType;
                if (strFileType == ".xls" || strFileType == ".xlsx")
                {


                    FileUploadControl.SaveAs(Server.MapPath("~/Import/ExcelImport_" + user_id + strFileType));

                }


                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Only excel files allowed');", true);
                    return;
                }



                int isexists = objOrderServices.GetExcelTemplateNameExists(txtName.Text, Convert.ToInt32(user_id));
                if (isexists > 0)
                {
                    msgOTAlert.Hide();
                    msgAlert.Hide();

                   // ShowOTAlertMessageBox();
                    lbldiverror.Text = "Template Name Already Exists";
                    return;
                }
                else
                {
                    lbldiverror.Text = "";
                    upload_excel();


                }
            }
            else
            {
                StatusLabel.Text = "Maximun file size allowed 20MB";
                StatusLabel.ForeColor = System.Drawing.Color.Red;
                StatusLabel.Visible = true;
            }


        }

        else
        {
            StatusLabel.Text = "Please choose a .xls/.xlsx file";
            StatusLabel.ForeColor = System.Drawing.Color.Red;
            StatusLabel.Visible = true;
        }


    }

    public void UploadButtonProdlist_Click(object sender, EventArgs e)
    {
        myString = "";
        Literal1.Text = Literal2.Text = Literal3.Text = myString;
        StatusLabel1.Visible = false;
        HttpContext.Current.Session["dtProdList"] = null;
        // RegularExpressionValidator1.ErrorMessage = null;
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
        StatusLabel.Visible = false;
        string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
        HttpContext.Current.Session["fileName"] = fileName;
        string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName);
        double dblMaxFileSize = Convert.ToDouble(ConfigurationManager.AppSettings["MaxFileSize"]);
        int intFileSize = FileUpload1.PostedFile.ContentLength;  // Here the file size is obtained in bytes
        double dblFileSizeinKB = intFileSize / 1024.0; // We convert the file size into kilobytes
        int QuickOrderBoxCount = Convert.ToInt16(ConfigurationManager.AppSettings["QuickOrderBoxCount"]);

        if ((FileUpload1.HasFile))
        {
            if (dblFileSizeinKB < dblMaxFileSize)
            {
                string strFileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                string strFileType = System.IO.Path.GetExtension(FileUpload1.FileName).ToString().ToLower();
                HttpContext.Current.Session["strFileName"] = strFileName;
                HttpContext.Current.Session["strFileType"] = strFileType;
                if (strFileType == ".xls" || strFileType == ".xlsx")
                {


                    FileUpload1.SaveAs(Server.MapPath("~/Import/ExcelImport_" + user_id + strFileType));

                }


                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Only excel files allowed');", true);
                    return;
                }



                //int isexists = objOrderServices.GetExcelTemplateNameExists(fileName, Convert.ToInt32(user_id));
                //if (isexists > 0)
                //{
                //    msgOTAlert.Hide();
                //    msgAlert.Hide();

                //    ShowOTAlertMessageBox();
                //    // lbldiverror.Text = "Template Name Already Exists";
                //    return;
                //}
                //else
                //{
                    upload_excelsub();


                //}
            }
            else
            {
                StatusLabel1.Text = "Maximun file size allowed 20MB";
                StatusLabel1.ForeColor = System.Drawing.Color.Red;
                StatusLabel1.Visible = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "addexcelcode();", true);
            }

        }

        else
        {
            StatusLabel1.Text = "Please choose file";
            StatusLabel1.ForeColor = System.Drawing.Color.Red;
            StatusLabel1.Visible = true;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "addexcelcode();", true);
        }


    }


    private void upload_excel()
    {

        string strFileName = string.Empty;
        string strFileType = string.Empty;
        if (HttpContext.Current.Session["strFileName"] != null)
        {

            strFileName = HttpContext.Current.Session["strFileName"].ToString();
            strFileName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(strFileName.ToLower());
        }

        if (HttpContext.Current.Session["strFileType"] != null)
        {
            strFileType = HttpContext.Current.Session["strFileType"].ToString();
        }


        try
        {

            OleDbConnection conn = new OleDbConnection();
            OleDbCommand cmd = new OleDbCommand();
            OleDbDataAdapter da = new OleDbDataAdapter();
            DataSet ds = new DataSet();
            string query = null;
            string connString = "";



            string strNewPath = Server.MapPath("~/Import/ExcelImport_" + user_id + strFileType);

            connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";

            query = "SELECT code FROM [Sheet1$]";

            conn = new OleDbConnection(connString);
            if (conn.State == ConnectionState.Closed)
                conn.Open();
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
                ProductServices objProductServices = new ProductServices();
                HelperDB objHelperDB = new HelperDB();
                HelperServices objHelperServices = new HelperServices();
                int rowcount = ds.Tables[0].Rows.Count;
                int rowcountnew = ds.Tables.Count;

                //dtdata.Columns.Add("Product_code");
                //dtdata.Columns.Add("ID");
                //dtdata.Columns.Add("FileName");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //  for (i = 0; i <= ds.Tables[0].Rows.Count-1; i++)
                    //  {
                    //      string sqlwordcol1 = ds.Tables[0].Rows[i].ItemArray[0].ToString().Trim();
                    //      string sqlwordcol2 = ds.Tables[0].Rows[i].ItemArray[1].ToString().Trim();
                    //      string containword = sqlwordcol1;



                    //      string product_code = ds.Tables[0].Rows[i].ItemArray[0].ToString().Trim();
                    // //     int product_id = objHelperServices.CI(objProductServices.GetProductID_code(product_code));
                    ////      decimal untPrice = objHelperDB.GetProductPrice_Exc(product_id, 1, user_id);
                    //      DataRow workRow = dtdata.NewRow();

                    //      workRow[0] = product_code;
                    //      workRow[1] = user_id;

                    //      workRow[2] = strFileName;
                    //      dtdata.Rows.Add(workRow); 

                    //  }
                    Insert_Master(user_id, txtName.Text, ds.Tables[0]);
                    GetData_DDL(user_id);
                    //     bulkcopy(ds.Tables[0]);
                    // BindDataIntoRepeater();
                }
                else
                {
                    StatusLabel.Text = "Invalid File.Please Download the Sample File";
                    StatusLabel.ForeColor = System.Drawing.Color.Red;
                    StatusLabel.Visible = true;

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


        //}
        //else
        //{
        //    StatusLabel.Text = "Please select an excel file first";
        //    StatusLabel.ForeColor = System.Drawing.Color.Red;
        //    StatusLabel.Visible = true;
        //}




    }
    public void upload_excelsub()
    {

        string strFileName = string.Empty;
        string strFileType = string.Empty;
        if (HttpContext.Current.Session["strFileName"] != null)
        {
            strFileName = HttpContext.Current.Session["strFileName"].ToString();
        }

        if (HttpContext.Current.Session["strFileType"] != null)
        {
            strFileType = HttpContext.Current.Session["strFileType"].ToString();
        }


        try
        {

            OleDbConnection conn = new OleDbConnection();
            OleDbCommand cmd = new OleDbCommand();
            OleDbDataAdapter da = new OleDbDataAdapter();
            DataSet ds = new DataSet();
            string query = null;
            string connString = "";



            string strNewPath = Server.MapPath("~/Import/ExcelImport_" + user_id + strFileType);

            connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";

            query = "SELECT code FROM [Sheet1$]";

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
                ProductServices objProductServices = new ProductServices();
                HelperDB objHelperDB = new HelperDB();
                HelperServices objHelperServices = new HelperServices();
                int rowcount = ds.Tables[0].Rows.Count;
                int rowcountnew = ds.Tables.Count;

                //dtdata.Columns.Add("Product_code");
                //dtdata.Columns.Add("ID");
                //dtdata.Columns.Add("FileName");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //  for (i = 0; i <= ds.Tables[0].Rows.Count-1; i++)
                    //  {
                    //      string sqlwordcol1 = ds.Tables[0].Rows[i].ItemArray[0].ToString().Trim();
                    //      string sqlwordcol2 = ds.Tables[0].Rows[i].ItemArray[1].ToString().Trim();
                    //      string containword = sqlwordcol1;



                    //      string product_code = ds.Tables[0].Rows[i].ItemArray[0].ToString().Trim();
                    // //     int product_id = objHelperServices.CI(objProductServices.GetProductID_code(product_code));
                    ////      decimal untPrice = objHelperDB.GetProductPrice_Exc(product_id, 1, user_id);
                    //      DataRow workRow = dtdata.NewRow();

                    //      workRow[0] = product_code;
                    //      workRow[1] = user_id;

                    //      workRow[2] = strFileName;
                    //      dtdata.Rows.Add(workRow); 

                    //  }
                    if (Request.QueryString["GetData"] == null)
                    {
                        Insert_MasterSub(user_id, txtName.Text, ds.Tables[0]);
                        GetData_DDL(user_id);
                    }
                    //else if (Request.QueryString["GetData"] == "true")
                    //{
                    //    GetDataFromDb(HttpContext.Current.Session["bindid"].ToString());
                    //}
                    //else
                    //{ Request.QueryString["GetData"] = "false"; }
                    
                   
                    //     bulkcopy(ds.Tables[0]);
                    // BindDataIntoRepeater();
                }
                else
                {
                    StatusLabel.Text = "Invalid File.Please Download the Sample File";
                    StatusLabel.ForeColor = System.Drawing.Color.Red;
                    StatusLabel.Visible = true;

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


        //}
        //else
        //{
        //    StatusLabel.Text = "Please select an excel file first";
        //    StatusLabel.ForeColor = System.Drawing.Color.Red;
        //    StatusLabel.Visible = true;
        //}




    }
    private string GetStockStatus(DataRow row)
    {

        string PROD_STOCK_STATUS = row["PROD_STOCK_STATUS"].ToString();
        // dr["Stock"] = dtitems.Rows[i]["prod_stk_status_dsc"].ToString();
        int stockflag = Convert.ToInt16(row["PROD_STOCK_FLAG"].ToString());
        string ETA = row["ETA"].ToString();

        bool validETA = false;

        if (ETA != string.Empty && ETA != "PLEASE CALL")
        {

            return "Pre-Order. ETA -"+ ETA+"";

        }
        string strpid = row["Product_id"].ToString();
        int SOH = 0;
        if (row["SOH"] != null && row["SOH"].ToString() != "")
        {
            SOH = Convert.ToInt16(row["SOH"].ToString());
        }

        int Supplier_ID = 0;
        if (row["Supplier_ID"] != null && row["Supplier_ID"].ToString() != "")
        {
            Supplier_ID = Convert.ToInt16(row["Supplier_ID"].ToString());
        }
        int SUPPLIER_SHIP_DAYS = 0;
        if (row["SUPPLIER_SHIP_DAYS"] != null && row["SUPPLIER_SHIP_DAYS"].ToString() != "")
        {
            SUPPLIER_SHIP_DAYS = Convert.ToInt16(row["SUPPLIER_SHIP_DAYS"].ToString());
        }
        //int Supplier_ID =Convert.ToInt16( dtitems.Rows[i]["Supplier_ID"].ToString());


       // string PROD_SUBSTITUTE = row["PROD_SUBSTITUTE"].ToString().Trim();
        string PROD_LEGISTATED_STATE = row["PROD_LEGISTATED_STATE"].ToString();

        //bool insert = true;


        if (stockflag == -2 && validETA ==true )
        {
            return "Pre-Order";
       
        }
      else  if (stockflag <= -2 && (PROD_LEGISTATED_STATE == "DDDDDDD" || Supplier_ID == 999) && PROD_LEGISTATED_STATE != "UUUUUUU")
        {

            return "Discontinued No Longer Available";
        }
        else if ((stockflag == -2 && PROD_STOCK_STATUS == "False" && row["PROD_STK_STATUS_DSC"].ToString().Trim() == "OUT_OF_STOCK ITEM WILL BE BACK ORDERED") || (stockflag == 0 && PROD_STOCK_STATUS == "True" && row["PROD_STK_STATUS_DSC"].ToString().Trim() == "Please_Call") || (stockflag == -2 && PROD_STOCK_STATUS == "False" && row["PROD_STK_STATUS_DSC"].ToString().Trim() == "SPECIAL_ORDER PRICE & AVAILABILITY TO BE CONFIRMED"))
        {
            string isSameLogic = GetStockDetails(row["PRODUCT_ID"].ToString(), ETA);
            if (isSameLogic == "true")
            {
                return row["PROD_STK_STATUS_DSC"].ToString().Trim();
            }
            else
            {
                return isSameLogic;
            }
        }
        else
        {



            return row["PROD_STK_STATUS_DSC"].ToString().Trim();
            //dr["soh"] = SOH;
            //dr["prod_stock_flag"] =stockflag;

            //dr["prod_legistated_state"] = PROD_LEGISTATED_STATE.Trim();

        }





    }

    private string GetStockDetails(string Pid,string eta)
    {
        HelperDB objhelperDb = new HelperDB();
        try
        {
            //string user_id = string.Empty;
            //string order_id = string.Empty;
            //int no = 0;
            int availabilty = 0;
            string availabilty1 = string.Empty;
            string sqlexec = "exec SP_CHECK_STOCK_STATUS '" + Pid.ToString() + "' ";

            DataSet Dsall = objhelperDb.GetDataSetDB(sqlexec);
            if (eta != string.Empty && eta != "PLEASE CALL")
            {

                return "Pre-Order. ETA :  " + eta + "";

            }
            if (Dsall.Tables[0].Rows[0]["AVAILABILTY_TOTAL"] != null && Dsall.Tables[0].Rows[0]["AVAILABILTY_TOTAL"].ToString() != "" && Dsall.Tables[0].Rows[0]["AVAILABILTY_TOTAL"].ToString().ToUpper().Trim() != "CALL")
            {
                availabilty1 = Dsall.Tables[0].Rows[0]["AVAILABILTY_TOTAL"].ToString();
                availabilty1 = availabilty1.Replace("+", "");
                availabilty = Convert.ToInt32(availabilty1.ToString());
            }
            if ((Dsall.Tables[0].Rows[0]["SUPPLIER_ITEM_CODE"] == null || Dsall.Tables[0].Rows[0]["SUPPLIER_ITEM_CODE"].ToString().Trim() == "") || (Dsall.Tables[0].Rows[0]["SUPPLIER_ID"] == null || Dsall.Tables[0].Rows[0]["SUPPLIER_ID"].ToString().Trim() == ""))
            {
                return "true";
            }
            if (Dsall.Tables[0].Rows[0]["product_id"] != null && Dsall.Tables[0].Rows[0]["product_id"].ToString() == string.Empty && Dsall.Tables[0].Rows[0]["PRODUCT_CODE"] != null && Dsall.Tables[0].Rows[0]["PRODUCT_CODE"].ToString() == string.Empty)
            {
                if (Dsall.Tables[0].Rows[0]["SUPPLIER_SHIP_DAYS"] != null && Dsall.Tables[0].Rows[0]["SUPPLIER_SHIP_DAYS"].ToString() != "")
                {
                    int shipping_time = Convert.ToInt32(Dsall.Tables[0].Rows[0]["SUPPLIER_SHIP_DAYS"].ToString());

                    string supplier_shipping_time = string.Empty;

                    if (shipping_time > 1)
                    {
                        supplier_shipping_time = "1 - " + shipping_time + " Days";
                    }
                    else if (shipping_time == 1)
                    {
                        supplier_shipping_time = " 1 Day";

                    }

                    if (shipping_time > 0 && shipping_time <= 14)
                    {
                        return "Please Call ships out within " + supplier_shipping_time;
                        //st.SetAttribute("TBT_STOCK_STATUS_2", true);
                        //st.SetAttribute("TBT_ISINSTOCK", true);
                        //st.SetAttribute("TBT_ISINSTOCK_STAUS", "Please Call");
                        //st.SetAttribute("TBT_SHIPPINGDAYS", supplier_shipping_time);
                        //st.SetAttribute("TBT_SHOW_SHIPPINGDAYS", true);
                        //objErrorHandler.CreateLog("supplier_shipping_time " + supplier_shipping_time);
                        //return false;
                    }
                    else
                    {
                        return "true";
                    }
                }
                return "true";
            }
            else if (availabilty > 0)
            {

                int avail_total = Convert.ToInt32(availabilty);
                int stock_cutoff = Convert.ToInt32(Dsall.Tables[0].Rows[0]["WEB_STOCK_CUTOFF"]);
                int shipping_time = Convert.ToInt32(Dsall.Tables[0].Rows[0]["SUPPLIER_SHIPPING_TIME"].ToString());

                string supplier_shipping_time = string.Empty;
                if (shipping_time > 1)
                {
                    supplier_shipping_time = "1 - " + shipping_time + " Days";
                }
                else if (shipping_time == 1)
                {
                    supplier_shipping_time = " 1 Day";
                }
                if (avail_total >= stock_cutoff)
                {

                    return "In Stock Ships out within"+"<br>" + supplier_shipping_time;
                    //st.SetAttribute("TBT_SHOW_SHIPPINGDAYS", true);
                    //st.SetAttribute("TBT_ISINSTOCK", true);
                    //st.SetAttribute("TBT_ISINSTOCK_STAUS", "In Stock");
                    //st.SetAttribute("TBT_SHIPPINGDAYS", supplier_shipping_time);
                }
                else if (avail_total < stock_cutoff)
                {
                    return "Limited Stock Ships out within " + supplier_shipping_time;
                    //st.SetAttribute("TBT_SHOW_SHIPPINGDAYS", true);
                    //st.SetAttribute("TBT_PLEASE_CALL", true);
                    //st.SetAttribute("TBT_SHIPPINGDAYS", supplier_shipping_time);
                }

            }
        }
        catch (Exception ex)
        {

        }
        return "true";
    }



  







    private string Get_Substitute_productCode(string PROD_SUBSTITUTE)
    {
        ErrorHandler objErrorHandler = new ErrorHandler();
        SqlCommand objSqlCommand = new SqlCommand();
        SqlDataAdapter objDataAdapter = new SqlDataAdapter();
        SqlConnection conn = new SqlConnection();
        objErrorHandler.CreateLog("inside getsubs" + PROD_SUBSTITUTE);
        try
        {
            string sqlexec = "select string_value from tb_prod_specs where product_id in (  select  product_id  from tb_prod_specs where string_value = '" + PROD_SUBSTITUTE + "' and attribute_id=450 ) and attribute_id=1 ";
            objSqlCommand = new SqlCommand(sqlexec, conn);
            objDataAdapter = new SqlDataAdapter(objSqlCommand);
            DataSet dsattr = new DataSet();
            objDataAdapter.Fill(dsattr);
            if (dsattr != null && dsattr.Tables[0].Rows.Count > 0)
            {
                return dsattr.Tables[0].Rows[0][0].ToString();
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.CreateLog(ex.ToString());

        }
        return PROD_SUBSTITUTE;

    }


    private string New_GetStockDetails(decimal days)
    {

        string comments = string.Empty;


        if (days < 3 && days >= 1)
        {
            comments = "Ships out Within 1 to 3 Days";
        }

        else if (days > 14)
        {
            int week = (int)Math.Ceiling(days / 7);
            int week1 = ((int)Math.Ceiling(days / 7)) + 1;
            comments = "Available Within " + week + "-" + week1 + " weeks from order";
        }
        else
        {

            comments = "Available Within " + days + "days from order";
        }
        return comments;

    }


    private void GetData_DDL(string id)
    {
        ConnectionDB objconn = new ConnectionDB();

        var con = new SqlConnection(objconn.ConnectionString);
        con.Open();
        var da = new SqlDataAdapter("select * from [TBWC_MYPRODUCT] where user_id='" + user_id + "' ", con);
        var dt = new DataTable();
        da.Fill(dt);

        con.Close();
        if (dt.Rows.Count > 0)
        {
            dgtemplate.DataSource = dt;
            dgtemplate.DataBind();
            divsavetemp.Visible = true;
        }
        else
        {
            divsavetemp.Visible = false;
        }
        //ddlsavetemp.DataTextField = "Filename";
        //ddlsavetemp.DataValueField = "id";

    }

    DataTable dtGetDataFromDb = new DataTable();
    public void GetDataFromDb(string id)
    {
        try
        {

            if (HttpContext.Current.Session["dtProdList"] == null)
            {
                HttpContext.Current.Session["bindid"] = id;


                ConnectionDB objconn = new ConnectionDB();

                var con = new SqlConnection(objconn.ConnectionString);
                con.Open();
                //var da = new SqlDataAdapter("select a.Id,a.Product_Id,b.STRING_VALUE,PROD_SUBSTITUTE,PROD_STOCK_FLAG, code,PROD_STK_STATUS_DSC ,DBO.GetETA(c.PROD_STOCK_STATUS,c.PROD_NEXT_SHIP) AS eta,a.Stock_Status,Comments,'$'+CONVERT(varchar, Price) as Price,QTY from [TBWC_MYPRODUCT_details] a left join TB_PROD_SPECS b on b.Product_Id=a.PRODUCT_ID and ATTRIBUTE_ID=7 left join westb_product_item c on c.Product_Id=a.product_id where  a.Id = '" + id + "' order by Code desc ", con);
                var da = new SqlDataAdapter("select a.Id,a.Product_Id,a.Id,d.STRING_VALUE,PROD_SUBSTITUTE,PROD_STOCK_FLAG, code,PROD_STK_STATUS_DSC ,DBO.GetETA(c.PROD_STOCK_STATUS,c.PROD_NEXT_SHIP) AS eta,a.Stock_Status,Comments,'$'+CONVERT(varchar, Price) as Price,QTY from [TBWC_MYPRODUCT_details] a  Left join TB_PROD_SPECS_img d on d.Product_Id = a.Product_Id  left join westb_product_item c on c.Product_Id=a.product_id where  a.Id = '" + id + "' order by Code desc ", con);
                da.Fill(dtCommon);
                // var da2 = new SqlDataAdapter("select QTY from TBWC_ORDER_ITEM where PRODUCT_ID =3",con);
                con.Close();
                dtCommon.Columns.Add("FlagVisible_Invisible", typeof(bool));
                if (dtCommon.Rows.Count > 0)
                {
                    if (divMain != null)
                        divMain.Visible = false;
                    if (lblnoprod != null)
                        lblnoprod.Visible = false;
                    // dt.Columns.Add("QTY");

                    int i = 0;
                   
                    string strfile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
                    foreach (DataRow row in dtCommon.Rows)
                    {
                        if (divmyproduct != null)
                            divmyproduct.Visible = true;
                        //    string  StockStatus= GetStockStatus(row);
                        //   row["Stock_Status"] = StockStatus;
                        //getqty(Convert.ToInt32(row["Product_id"]));


                        //    row["QTY"] = dtGetDataFromDb.Rows[i]["QTY"];
                        // objHelperServices.SetImageFolderPath(row["STRING_VALUE"].ToString().Replace("\\", "/"), "_th", "_th50");
                        //string strfile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
                        ////string ordImage = strfile + objHelperServices.SetImageFolderPath(row["STRING_VALUE"].ToString().Replace("\\", "/"), "_images", "_th50");
                        ///
                        //row["STRING_VALUE"] = ordImage;
                        string ordImage = strfile + objHelperServices.SetImageFolderPath(row["STRING_VALUE"].ToString().Replace("\\", "/"), "_images", "_th50");

                        if (row["STRING_VALUE"].ToString() == "" || row["STRING_VALUE"].ToString() == "NULL" || row["STRING_VALUE"].ToString() == null)
                        {
                            ordImage = "../images/Noimage.Gif";
                            row["STRING_VALUE"] = ordImage;
                        }
                        else
                        {
                            row["STRING_VALUE"] = ordImage;
                        }

                        //objErrorHandler.CreateLog(row["Stock_Status"].ToString());
                        string stk_sta_desc = row["Stock_Status"].ToString().Trim().Replace("_", " ");
                        row["Stock_Status"] = stk_sta_desc.ToUpper();
                        string PROD_STOCK_FLAG = row["PROD_STOCK_FLAG"].ToString();
                        string PROD_SUBSTITUTE = row["PROD_SUBSTITUTE"].ToString();

                        if (stk_sta_desc != "IN STOCK")
                        {
                            string x = GetStockDetails(row["Product_Id"].ToString(), row["eta"].ToString());
                            //  if (stk_sta_desc.ToUpper() == "DISCONTINUED NO LONGER AVAILABLE" || stk_sta_desc.ToUpper() == "TEMPORARY UNAVAILABLE NO ETA" || stk_sta_desc.ToUpper().Contains("OUT_OF_STOCK") == true || stk_sta_desc.ToUpper().Contains("SPECIAL_ORDER") == true)
                            if (stk_sta_desc == "DISCONTINUED NO LONGER AVAILABLE" || stk_sta_desc == "TEMPORARY UNAVAILABLE NO ETA" || stk_sta_desc.Contains("OUT OF STOCK") == true || stk_sta_desc.Contains("SPECIAL ORDER") == true || stk_sta_desc.ToUpper().Contains("LIMITED STOCK") == true || stk_sta_desc.ToUpper().Contains("PLEASE CALL") == true || stk_sta_desc.ToUpper().Contains("IN STOCK") == true)
                            {

                                if (stk_sta_desc == "DISCONTINUED NO LONGER AVAILABLE" || stk_sta_desc == "TEMPORARY UNAVAILABLE NO ETA")

                                {
                                    if (x != "true")
                                    {
                                        row["Stock_Status"] = x.ToUpper();
                                    }
                                    else
                                    {
                                        row["Stock_Status"] = stk_sta_desc.ToUpper();
                                        row["FlagVisible_Invisible"] = false;
                                    }


                                }
                                if (stk_sta_desc.Contains("OUT OF STOCK") == true || stk_sta_desc.Contains("SPECIAL ORDER") == true || stk_sta_desc.ToUpper().Contains("LIMITED STOCK") == true || stk_sta_desc.ToUpper().Contains("PLEASE CALL") == true)
                                {
                                    if (x != "true")
                                    {
                                        row["Stock_Status"] = x.ToUpper();
                                        row["FlagVisible_Invisible"] = true;
                                    }
                                    else
                                    {
                                        row["Stock_Status"] = stk_sta_desc.ToUpper();
                                        row["FlagVisible_Invisible"] = true;
                                    }
                                }
                                if (stk_sta_desc.ToUpper().Contains("IN STOCK") == true)
                                {
                                    if (x != "true")
                                    {
                                        row["Stock_Status"] = x.ToUpper();
                                        row["FlagVisible_Invisible"] = true;
                                    }
                                    else
                                    {
                                        row["Stock_Status"] = stk_sta_desc.ToUpper();
                                        row["FlagVisible_Invisible"] = true;
                                    }
                                }
                                if (PROD_SUBSTITUTE.Trim() != "" && PROD_STOCK_FLAG == "-2")
                                {
                                    // DataTable rtntbl = objProductServices.GetSubstituteProductDetails(drpid["PROD_SUBSTITUTE"].ToString().Trim(), Convert.ToInt32(userid));
                                    objErrorHandler.CreateLog(row["code"] + "ps");
                                    DataTable rtntbl = objProductServices.GetSubstituteProductDetails(row["code"].ToString(), Convert.ToInt32(user_id));

                                    if (rtntbl != null && rtntbl.Rows.Count > 0)
                                    {

                                        bool samecodesubproduct = (bool)rtntbl.Rows[0]["samecodesubproduct"];

                                        bool samecodenotFound = (bool)rtntbl.Rows[0]["samecodenotFound"];
                                        //objErrorHandler.CreateLog("samecodenotFound" + samecodenotFound);
                                        //objErrorHandler.CreateLog("ea_path" + rtntbl.Rows[0]["ea_path"].ToString());
                                        if (samecodenotFound == false && rtntbl.Rows[0]["ea_path"].ToString() != "")
                                        {
                                            string strurl = "ProductDetails.aspx?Pid=" + rtntbl.Rows[0]["SubstuyutePid"].ToString() + "&amp;fid=" + rtntbl.Rows[0]["Pfid"].ToString() + "&amp;Cid=" + rtntbl.Rows[0]["CatId"].ToString() + "&amp;path=" + rtntbl.Rows[0]["ea_path"].ToString();
                                            //objErrorHandler.CreateLog(strurl);
                                            //strurl = strurl.Trim();
                                            //Rows[0]["Pfid"].ToString() + "&amp;Cid=" + rtntbl.Rows[0]["CatId"].ToString() + "&amp;path=" + rtntbl.Rows[0]["ea_path"].ToString();
                                            row["Stock_Status"] = "SUBSTITUTED TO" + "</br> " + "<a style='font:bold;font-weight:600' class='blue1' href = '" + strurl + "'>" + rtntbl.Rows[0]["wag_product_code"].ToString() + "</a>";

                                            row["FlagVisible_Invisible"] = true;
                                            // _stmpl_records.SetAttribute("TBT_REP_EA_PATH", strurl);

                                        }
                                        else if (stk_sta_desc == "Discontinued No Longer Available" || stk_sta_desc == "DISCONTINUED NO LONGER AVAILABLE" || stk_sta_desc.ToUpper() == "TEMPORARY UNAVAILABLE NO ETA")

                                        {

                                            //string x = GetStockDetails(row["Product_Id"].ToString(), row["eta"].ToString());
                                            if (x != "true")
                                            {
                                                row["Stock_Status"] = x.ToUpper();
                                            }
                                            else
                                            {
                                                row["Stock_Status"] = stk_sta_desc.ToUpper();
                                                row["FlagVisible_Invisible"] = false;
                                            }


                                        }
                                        else
                                        {

                                            row["Stock_Status"] = GetStockDetails(row["Product_Id"].ToString(), row["eta"].ToString());
                                            row["FlagVisible_Invisible"] = true;
                                        }
                                    }
                                    else if (stk_sta_desc == "Discontinued No Longer Available" || stk_sta_desc.ToUpper() == "TEMPORARY UNAVAILABLE NO ETA")
                                    {
                                        //row["Stock_Status"] = GetStockDetails(row["Product_Id"].ToString());
                                        //row["FlagVisible_Invisible"] = false;

                                        //string x = GetStockDetails(row["Product_Id"].ToString(), row["eta"].ToString());
                                        if (x != "true")
                                        {
                                            row["Stock_Status"] = x.ToUpper();
                                        }
                                        else
                                        {
                                            row["Stock_Status"] = stk_sta_desc.ToUpper();
                                            row["FlagVisible_Invisible"] = false;
                                        }

                                    }
                                    else
                                    {

                                        row["Stock_Status"] = GetStockDetails(row["Product_Id"].ToString(), row["eta"].ToString()).ToUpper();
                                        row["FlagVisible_Invisible"] = true;
                                    }
                                    //objErrorHandler.CreateLog("true");


                                }
                                else
                                {
                                    objErrorHandler.CreateLog(stk_sta_desc + "__4");
                                    objErrorHandler.CreateLog("false");
                                    if (stk_sta_desc.ToUpper() == "DISCONTINUED NO LONGER AVAILABLE" || stk_sta_desc.ToUpper() == "TEMPORARY UNAVAILABLE NO ETA")
                                    {
                                        //string x = GetStockDetails(row["Product_Id"].ToString(), row["eta"].ToString());
                                        if (x != "true")
                                        {
                                            row["Stock_Status"] = x.ToUpper();
                                        }
                                        else
                                        {
                                            row["Stock_Status"] = stk_sta_desc.ToUpper();
                                            row["FlagVisible_Invisible"] = false;
                                        }
                                    }
                                    //row["Stock_Status"] = GetStockDetails(row["Product_Id"].ToString());
                                    //row["FlagVisible_Invisible"] = true;
                                }
                                
                            }
                            else
                            {
                                row["FlagVisible_Invisible"] = false;
                            }
                        }
                        else
                        {

                            row["Stock_Status"] = stk_sta_desc;
                            row["FlagVisible_Invisible"] = true;
                        }
                        i++;
                        dtCommon.AcceptChanges();
                    }
                }
                else
                {
                    lblnoprod.Visible = true;
                }
                divmyproduct.Visible = true;
                lbltempname.Visible = true;
                HttpContext.Current.Session["dtProdList"] = dtCommon;
                Gridprodlst.DataSource = dtCommon;
                Gridprodlst.DataBind();

            }
            else
            {

                dtCommon =(DataTable)  HttpContext.Current.Session["dtProdList"];
                Gridprodlst.DataSource = dtCommon;
                Gridprodlst.DataBind();

            }
          
          
        }
        catch (Exception ex)
        {

            objErrorHandler.CreateLog(ex.ToString());
        }
    }

    

    private void getqty(int product_id)
    {


        ConnectionDB objconn1 = new ConnectionDB();

        var con1 = new SqlConnection(objconn1.ConnectionString);
        con1.Open();
        var da2 = new SqlDataAdapter("select b.QTY from TB_PROD_SPECS a left join TBWC_ORDER_ITEM b on b.PRODUCT_ID = a.PRODUCT_ID where a.PRODUCT_ID =" + Convert.ToInt32(product_id) + " and a.ATTRIBUTE_ID=1", con1);

        da2.Fill(dtGetDataFromDb);

        con1.Close();
    }




    private void GetDataFromDb1(string code, int id)
    {
        try
        {
            var dt = new DataTable();

            ConnectionDB objconn = new ConnectionDB();

            var con = new SqlConnection(objconn.ConnectionString);
            con.Open();
           // var da = new SqlDataAdapter("select a.Id,a.Product_Id,b.STRING_VALUE, code,Stock_Status,Comments,'$'+CONVERT(varchar, Price) as Price,QTY from [TBWC_MYPRODUCT_details] a join TB_PROD_SPECS b on a.Product_Id=b.PRODUCT_ID and ATTRIBUTE_ID=7 where code !='" + code + "'  ", con); //and a.Id = " + id + "
            var da = new SqlDataAdapter("select a.Id,a.Product_Id,d.STRING_VALUE,DBO.GetETA(c.PROD_STOCK_STATUS,c.PROD_NEXT_SHIP) AS eta,a.Stock_Status,PROD_SUBSTITUTE,PROD_STOCK_FLAG, code,PROD_STK_STATUS_DSC,Comments,'$'+CONVERT(varchar, Price) as Price,QTY from [TBWC_MYPRODUCT_details] a  Left join TB_PROD_SPECS_img d on d.Product_Id = a.Product_Id  left join westb_product_item c on c.Product_Id=a.product_id where  code !='" + code + "' and a.Id ="+id+ " order by Code desc", con);
           // var da = new SqlDataAdapter("select a.Id,a.Product_Id,b.STRING_VALUE,PROD_SUBSTITUTE,PROD_STOCK_FLAG, code,PROD_STK_STATUS_DSC ,DBO.GetETA(c.PROD_STOCK_STATUS,c.PROD_NEXT_SHIP) AS eta,a.Stock_Status,Comments,'$'+CONVERT(varchar, Price) as Price,QTY from [TBWC_MYPRODUCT_details] a left join TB_PROD_SPECS_Code b on b.Product_Id=a.PRODUCT_ID Left join TB_PROD_SPECS_img d on d.Product_Id = a.Product_Id and d.ATTRIBUTE_ID=7 left join westb_product_item c on c.Product_Id=a.product_id where  a.Id = '" + id + "' order by Code desc ", con);
            da.Fill(dt);
            // var da2 = new SqlDataAdapter("select QTY from TBWC_ORDER_ITEM where PRODUCT_ID =3",con);
            con.Close();
            dt.Columns.Add("FlagVisible_Invisible", typeof(bool));
            if (dt.Rows.Count > 0)
            {
                if (divMain != null)
                    divMain.Visible = false;
                if (lblnoprod != null)
                    lblnoprod.Visible = false;
                // dt.Columns.Add("QTY");

                int i = 0;

                string strfile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
                foreach (DataRow row in dt.Rows)
                {
                    if (divmyproduct != null)
                        divmyproduct.Visible = true;
                    //    string  StockStatus= GetStockStatus(row);
                    //   row["Stock_Status"] = StockStatus;
                    //getqty(Convert.ToInt32(row["Product_id"]));


                    //    row["QTY"] = dtGetDataFromDb.Rows[i]["QTY"];
                    // objHelperServices.SetImageFolderPath(row["STRING_VALUE"].ToString().Replace("\\", "/"), "_th", "_th50");
                    //string strfile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
                    ////string ordImage = strfile + objHelperServices.SetImageFolderPath(row["STRING_VALUE"].ToString().Replace("\\", "/"), "_images", "_th50");
                    ///
                    //row["STRING_VALUE"] = ordImage;
                    string ordImage = strfile + objHelperServices.SetImageFolderPath(row["STRING_VALUE"].ToString().Replace("\\", "/"), "_images", "_th50");

                    if (row["STRING_VALUE"].ToString() == "" || row["STRING_VALUE"].ToString() == "NULL" || row["STRING_VALUE"].ToString() == null)
                    {
                        ordImage = "../images/Noimage.Gif";
                        row["STRING_VALUE"] = ordImage;
                    }
                    else
                    {
                        row["STRING_VALUE"] = ordImage;
                    }

                    //objErrorHandler.CreateLog(row["Stock_Status"].ToString());
                    string stk_sta_desc = row["Stock_Status"].ToString().Trim().Replace("_", " ");
                    row["Stock_Status"] = stk_sta_desc.ToUpper();
                    string PROD_STOCK_FLAG = row["PROD_STOCK_FLAG"].ToString();
                    string PROD_SUBSTITUTE = row["PROD_SUBSTITUTE"].ToString();
                    if (stk_sta_desc != "IN STOCK")
                    {
                        string x = GetStockDetails(row["Product_Id"].ToString(), row["eta"].ToString());
                        //  if (stk_sta_desc.ToUpper() == "DISCONTINUED NO LONGER AVAILABLE" || stk_sta_desc.ToUpper() == "TEMPORARY UNAVAILABLE NO ETA" || stk_sta_desc.ToUpper().Contains("OUT_OF_STOCK") == true || stk_sta_desc.ToUpper().Contains("SPECIAL_ORDER") == true)
                        //if (stk_sta_desc == "DISCONTINUED NO LONGER AVAILABLE" || stk_sta_desc == "TEMPORARY UNAVAILABLE NO ETA" || stk_sta_desc.Contains("OUT OF STOCK") == true || stk_sta_desc.Contains("SPECIAL ORDER") == true || stk_sta_desc.ToUpper().Contains("LIMITED STOCK") == true || stk_sta_desc.ToUpper().Contains("PLEASE CALL") == true)
                            if (stk_sta_desc == "DISCONTINUED NO LONGER AVAILABLE" || stk_sta_desc == "TEMPORARY UNAVAILABLE NO ETA" || stk_sta_desc.Contains("OUT OF STOCK") == true || stk_sta_desc.Contains("SPECIAL ORDER") == true || stk_sta_desc.ToUpper().Contains("LIMITED STOCK") == true || stk_sta_desc.ToUpper().Contains("PLEASE CALL") == true || stk_sta_desc.ToUpper().Contains("IN STOCK") == true)
                            {

                            if (stk_sta_desc == "DISCONTINUED NO LONGER AVAILABLE" || stk_sta_desc == "TEMPORARY UNAVAILABLE NO ETA")

                            {
                                if (x != "true")
                                {
                                    row["Stock_Status"] = x.ToUpper();
                                }
                                else
                                {
                                    row["Stock_Status"] = stk_sta_desc.ToUpper();
                                    row["FlagVisible_Invisible"] = false;
                                }


                            }
                            if (stk_sta_desc.Contains("OUT OF STOCK") == true || stk_sta_desc.Contains("SPECIAL ORDER") == true || stk_sta_desc.ToUpper().Contains("LIMITED STOCK") == true || stk_sta_desc.ToUpper().Contains("PLEASE CALL") == true)
                            {
                                if (x != "true")
                                {
                                    row["Stock_Status"] = x.ToUpper();
                                    row["FlagVisible_Invisible"] = true;
                                }
                                else
                                {
                                    row["Stock_Status"] = stk_sta_desc.ToUpper();
                                    row["FlagVisible_Invisible"] = true;
                                }
                            }
                            if (stk_sta_desc.ToUpper().Contains("IN STOCK") == true)
                            {
                                if (x != "true")
                                {
                                    row["Stock_Status"] = x.ToUpper();
                                    row["FlagVisible_Invisible"] = true;
                                }
                                else
                                {
                                    row["Stock_Status"] = stk_sta_desc.ToUpper();
                                    row["FlagVisible_Invisible"] = true;
                                }
                            }

                            if (PROD_SUBSTITUTE.Trim() != "" && PROD_STOCK_FLAG == "-2")
                            {
                                // DataTable rtntbl = objProductServices.GetSubstituteProductDetails(drpid["PROD_SUBSTITUTE"].ToString().Trim(), Convert.ToInt32(userid));
                                objErrorHandler.CreateLog(row["code"] + "ps");
                                DataTable rtntbl = objProductServices.GetSubstituteProductDetails(row["code"].ToString(), Convert.ToInt32(user_id));

                                if (rtntbl != null && rtntbl.Rows.Count > 0)
                                {

                                    bool samecodesubproduct = (bool)rtntbl.Rows[0]["samecodesubproduct"];

                                    bool samecodenotFound = (bool)rtntbl.Rows[0]["samecodenotFound"];
                                    //objErrorHandler.CreateLog("samecodenotFound" + samecodenotFound);
                                    //objErrorHandler.CreateLog("ea_path" + rtntbl.Rows[0]["ea_path"].ToString());
                                    if (samecodenotFound == false && rtntbl.Rows[0]["ea_path"].ToString() != "")
                                    {
                                        string strurl = "ProductDetails.aspx?Pid=" + rtntbl.Rows[0]["SubstuyutePid"].ToString() + "&amp;fid=" + rtntbl.Rows[0]["Pfid"].ToString() + "&amp;Cid=" + rtntbl.Rows[0]["CatId"].ToString() + "&amp;path=" + rtntbl.Rows[0]["ea_path"].ToString();
                                        //objErrorHandler.CreateLog(strurl);
                                        //strurl = strurl.Trim();
                                        //Rows[0]["Pfid"].ToString() + "&amp;Cid=" + rtntbl.Rows[0]["CatId"].ToString() + "&amp;path=" + rtntbl.Rows[0]["ea_path"].ToString();
                                        row["Stock_Status"] = "SUBSTITUTED TO" + "</br> " + "<a style='font:bold;font-weight:600' class='blue1' href = '" + strurl + "'>" + rtntbl.Rows[0]["wag_product_code"].ToString() + "</a>";

                                        row["FlagVisible_Invisible"] = true;
                                        // _stmpl_records.SetAttribute("TBT_REP_EA_PATH", strurl);

                                    }
                                    else if (stk_sta_desc == "Discontinued No Longer Available" || stk_sta_desc == "DISCONTINUED NO LONGER AVAILABLE" || stk_sta_desc.ToUpper() == "TEMPORARY UNAVAILABLE NO ETA")

                                    {

                                        //string x = GetStockDetails(row["Product_Id"].ToString(), row["eta"].ToString());
                                        if (x != "true")
                                        {
                                            row["Stock_Status"] = x.ToUpper();
                                        }
                                        else
                                        {
                                            row["Stock_Status"] = stk_sta_desc.ToUpper();
                                            row["FlagVisible_Invisible"] = false;
                                        }


                                    }
                                    else
                                    {

                                        row["Stock_Status"] = GetStockDetails(row["Product_Id"].ToString(), row["eta"].ToString());
                                        row["FlagVisible_Invisible"] = true;
                                    }
                                }
                                else if (stk_sta_desc == "Discontinued No Longer Available" || stk_sta_desc.ToUpper() == "TEMPORARY UNAVAILABLE NO ETA")
                                {
                                    //row["Stock_Status"] = GetStockDetails(row["Product_Id"].ToString());
                                    //row["FlagVisible_Invisible"] = false;

                                    //string x = GetStockDetails(row["Product_Id"].ToString(), row["eta"].ToString());
                                    if (x != "true")
                                    {
                                        row["Stock_Status"] = x;
                                    }
                                    else
                                    {
                                        row["Stock_Status"] = stk_sta_desc;
                                        row["FlagVisible_Invisible"] = false;
                                    }

                                }
                                else
                                {

                                    row["Stock_Status"] = GetStockDetails(row["Product_Id"].ToString(), row["eta"].ToString()).ToUpper();
                                    row["FlagVisible_Invisible"] = true;
                                }
                                //objErrorHandler.CreateLog("true");


                            }
                            else
                            {
                                objErrorHandler.CreateLog(stk_sta_desc + "__4");
                                objErrorHandler.CreateLog("false");
                                if (stk_sta_desc.ToUpper() == "DISCONTINUED NO LONGER AVAILABLE" || stk_sta_desc.ToUpper() == "TEMPORARY UNAVAILABLE NO ETA")
                                {
                                    //string x = GetStockDetails(row["Product_Id"].ToString(), row["eta"].ToString());
                                    if (x != "true")
                                    {
                                        row["Stock_Status"] = x.ToUpper();
                                    }
                                    else
                                    {
                                        row["Stock_Status"] = stk_sta_desc.ToUpper();
                                        row["FlagVisible_Invisible"] = false;
                                    }
                                }
                                //row["Stock_Status"] = GetStockDetails(row["Product_Id"].ToString());
                                //row["FlagVisible_Invisible"] = true;
                            }

                        }
                        else
                        {
                            row["FlagVisible_Invisible"] = false;
                        }
                    }
                    else
                    {

                        row["Stock_Status"] = stk_sta_desc.ToUpper();
                        row["FlagVisible_Invisible"] = true;
                    }
                    i++;
                    dt.AcceptChanges();
                }
            }
            else
            {
                lblnoprod.Visible = true;
            }
            divmyproduct.Visible = true;
            lbltempname.Visible = true;
            HttpContext.Current.Session["dtProdList"] = dt;
            Gridprodlst.DataSource = dt;
            Gridprodlst.DataBind();

        }
        catch (Exception ex)
        { }
    }



    protected void LinkButton_Click(object sender, EventArgs e)
    {

        ////Response.ContentType = "xls";
        ////Response.AppendHeader("Content-Disposition", "attachment; filename=Sample.xls");
        ////Response.TransmitFile(Server.MapPath("~/Export/Sample.xls"));
        ////Response.End();


        try
        {

            string fileName = ConfigurationManager.AppSettings["MyProdFileName"].ToString();
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
                if (productcodes[i].Trim() != "" && objHelperServices.CI(quantitys[i]) > 0)
                    tmpdatatbl.Rows.Add(productcodes[i], quantitys[i]);

            }
            tmpds.Tables.Add(tmpdatatbl);
            if (tmpds != null && tmpds.Tables.Count > 0)
                dscodes = tmpds;
            else
                dscodes = null;

        }
    }
    protected void btnSaveasOrdTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            msgOTAlert.Hide();
            msgAlert.Hide();

            SettextData(TQtys, TProds);

            //TxtdivTemplateName.Text = "";
            //TxtdivDesc.Text = "";


            ShowOTAlertMessageBox();
        }
        catch (Exception ex)
        {
        }
    }

    private void ShowOTAlertMessageBox()
    {
        //  lbldiverror.Text = "";
        divOrderTemplate1.Visible = true;
        msgOTAlert.ID = "divOrderTemplate";
        msgOTAlert.PopupControlID = "plnOrderTemplate";
        msgOTAlert.BackgroundCssClass = "modalBackground";
        msgOTAlert.TargetControlID = "btnHiddenTestPopupExtender";
        msgOTAlert.DropShadow = true;
        msgOTAlert.CancelControlID = "btnOTClose";
        this.plnOrderTemplate.Controls.Add(msgOTAlert);
        this.msgOTAlert.Show();
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

        ClarifyAlert.ID = "DivItemClarify";
        ClarifyAlert.PopupControlID = "PnlItemClarify";
        ClarifyAlert.BackgroundCssClass = "modalBackground";
        ClarifyAlert.TargetControlID = "btnHiddenTestPopupExtender";
        ClarifyAlert.DropShadow = true;
        ClarifyAlert.CancelControlID = "btnok";
        ClarifyAlert.BehaviorID = "BtnItemClarify";
        //this.PnlItemClarify.Controls.Add(ClarifyAlert);

        this.ClarifyAlert.Show();
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

    protected void btnSaveOrdTemplate_excel(object sender, EventArgs e)
    {
        msgAlert.Hide();
        msgOTAlert.Hide();
        plnOrderTemplate.Visible = false;
        pnlAlert.Visible = false;
        upload_excel();

    }

    protected void btnSaveOrdTemplate(object sender, EventArgs e)
    {
        //  try
        //  {
        //      isSave = true;

        //DataSet ds=      objOrderServices.GetOrderItems(OrderID);

        //      for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
        //      {
        //          if (i == 0)
        //          {
        //              TQtys =ds.Tables[0].Rows[i]["qty"].ToString();
        //              TProds = ds.Tables[0].Rows[i]["catalog_item_no"].ToString();
        //          }
        //          else
        //          {
        //              TQtys = TQtys+","+ ds.Tables[0].Rows[i]["qty"].ToString();
        //              TProds = TProds+","+ ds.Tables[0].Rows[i]["catalog_item_no"].ToString();
        //          }
        //      }
        //      int userid = 0;
        //      //if (txtcnt != null && txtcnt != "" && ItemCnt == 0)
        //      //{
        //          txtCount = ds.Tables[0].Rows.Count;
        //      //}
        //      //else
        //      //{
        //      //    txtCount = ItemCnt;
        //      //}
        //      string[] ProdAQty = new string[txtCount];
        //      string[] ProdAItem = new string[txtCount];
        //      string[] ItemQty = new string[txtCount];
        //      TempProdQtys = TQtys;
        //      TempProdItems = TProds;
        //      if (TempProdQtys == null || TempProdItems == null)
        //          return;

        //      SettextData(TQtys, TProds);


        //      if (TxtdivTemplateName.Text.Trim() == "" && TxtTemplateName.Text.Trim()=="")
        //      {
        //          msgOTAlert.Hide();
        //          msgAlert.Hide();

        //          ShowOTAlertMessageBox();
        //          lbldiverror.Text = "Enter Template Name";
        //          return;
        //      }

        //      ProdAQty = Regex.Split(TempProdQtys, ",");
        //      ProdAItem = Regex.Split(TempProdItems, ",");


        //      string Pqtycheck = "";
        //      Int32 Num;
        //      for (int i = 0; i < ProdAQty.Length - 1; i++)
        //      {
        //          Pqtycheck = ProdAQty[i].ToString().Trim();
        //          bool isNum = Int32.TryParse(Pqtycheck, out Num);

        //          if (isNum == false)
        //          {
        //              msgOTAlert.Hide();
        //              msgAlert.Hide();

        //              ShowOTAlertMessageBox();
        //              lbldiverror.Text = "Incorrect Code or Invalid QTY";
        //              return;
        //          }
        //      }

        //      string _notfoundstr = "";
        //      string _notfoundstrhtml = "";
        //      string _minqty = "";
        //      string _minqtyhtml = "";
        //      string _maxqty = "";
        //      string _maxqtyhtml = "";
        //      string _notfoundqtyhtml = "";
        //      bool itemcheck = false;
        //      string PCodeAll = "";
        //      DataTable protbl = null;

        //      decimal ProdTotalPrice = 0;
        //      decimal OrderTotal = 0;
        //      decimal TotalShipCost = 0;
        //      DataTable dt = null;
        //      int tmpcount = 0;
        //      for (int i = 0; i < ProdAItem.Length; i++)
        //      {
        //          PCodeAll = PCodeAll + "'" + ProdAItem[i].ToString().Trim() + "',";

        //      }
        //      if (PCodeAll != "")
        //      {
        //          PCodeAll = PCodeAll.Substring(0, PCodeAll.Length - 1) + "";
        //          protbl = (DataTable)objHelperDB.GetGenericPageDataDB(PCodeAll, "GET_BULKORDER_INVENTORY_COUNT_ALL", HelperDB.ReturnType.RTTable);

        //      }
        //      tmpcount = PCodeAll.Split(',').Length - 1;

        //      //if (flgcprowcheck == true)
        //      //{

        //      //    if (protbl == null && flgcprowcheck == true || tmpcount != protbl.Rows.Count && flgcprowcheck == true)
        //      //    {
        //      //        msgOTAlert.Hide();
        //      //        msgAlert.Hide();

        //      //        ShowOTAlertMessageBox();
        //      //        lbldiverror.Text = "Incorrect Code or Invalid QTY";

        //      //        return;   

        //      //    }
        //      //}


        //      int Template_id = 0;
        //      string OrderStatus = "";
        //      OrderServices.OrderTemplateInfo oTemInFo = new OrderServices.OrderTemplateInfo();
        //      OrderServices.OrderTemplateItemInfo oItemInFo = new OrderServices.OrderTemplateItemInfo();
        //      OrderServices.Order_Calrification_ItemInfo oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
        //      userid = ObjHelperServices.CI(Session["USER_ID"]);
        //      string proids = "";
        //      if (TxtTemplateName.Text == "")
        //      {
        //          TxtTemplateName.Text = TxtdivTemplateName.Text;
        //      }
        //      if (TxtDesc.Text == "")
        //      {
        //          TxtDesc.Text = TxtdivDesc.Text;
        //      }
        //      oTemInFo.TemplateName = TxtTemplateName.Text;
        //      oTemInFo.Description = TxtDesc.Text;
        //      oTemInFo.UserID = userid;
        //      oTemInFo.CompanyID = HttpContext.Current.Session["COMPANY_ID"].ToString();
        //      oTemInFo.TemplateId = 0;
        //      int isexists = objOrderServices.GetOrderTemplateNameExists(TxtTemplateName.Text, oTemInFo.UserID, oTemInFo.TemplateId);
        //      if (isexists > 0)
        //      {
        //          msgOTAlert.Hide();
        //          msgAlert.Hide();

        //          ShowOTAlertMessageBox();
        //          lbldiverror.Text = "Template Name Already Exists";
        //          return;
        //      }

        //      DataTable tbErrorItem = objOrderServices.GetOrder_Clarification_Items(Template_id, "TEMP_ITEM_ERROR");
        //      DataTable tbErrorChk = objOrderServices.GetOrder_Clarification_Items(Template_id, "TEMP_ITEM_CHK");

        //      if ((tbErrorItem != null && tbErrorItem.Rows.Count > 0) || (tbErrorChk != null && tbErrorChk.Rows.Count > 0))
        //      {
        //          string Script = "alert('Please review and correct Order Clarifications / Errors before proceeding to Save Template!');";
        //          Page.ClientScript.RegisterStartupScript(this.GetType(), "Tempscript2", Script, true);
        //          return;
        //      }

        //      //if ((HttpContext.Current.Session["TEMPLATE_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["TEMPLATE_ID"]) > 0))
        //      //{
        //      //    oTemInFo.TemplateId = Convert.ToInt32(Session["TEMPLATE_ID"]);
        //      //}
        //      //else
        //      //    oTemInFo.TemplateId = 0;

        //      Template_id = objOrderServices.AddOrderItemTemplate(oTemInFo);


        //      //objOrderServices.RemoveOrderTemplate(userid); 

        //      for (int i = 0; i < ProdAItem.Length; i++)
        //      {
        //          int checkset = -1;
        //          string _pCode = ProdAItem[i].ToString().Trim();
        //          decimal _pQty = string.IsNullOrEmpty(_pCode.Trim()) ? 0 : Convert.ToDecimal(ProdAQty[i]);

        //          if (_pCode != "")
        //          {

        //              if (protbl != null && protbl.Rows.Count > 0)
        //              {
        //                  DataRow[] dr = protbl.Select("PCode='" + _pCode + "'");
        //                  if (dr.Length > 0)
        //                      checkset = 1;
        //                  else
        //                      checkset = 0;

        //              }
        //              else
        //                  checkset = 0;


        //              if (checkset == 0)
        //              {


        //                  string _substitute = FindSubstitute(_pCode);


        //                  if (_substitute == "{~MI~}")
        //                  {
        //                      _notfoundstrhtml += string.Format("{0},", _pCode);
        //                      _notfoundqtyhtml += string.Format("{0},", _pQty);

        //                      oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
        //                      oItemClaItemInfo.Clarification_ID = 0;
        //                      oItemClaItemInfo.OrderID = Template_id;
        //                      oItemClaItemInfo.ProductDesc = _pCode;
        //                      oItemClaItemInfo.Quantity = _pQty;
        //                      oItemClaItemInfo.UserID = userid;
        //                      oItemClaItemInfo.Clarification_Type = "TEMP_ITEM_CHK";
        //                      objOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);


        //                  }
        //                  else if (_substitute == "{~N/A~}")
        //                  {

        //                      _notfoundstr += string.Format("{0},", _pCode);
        //                      oItemClaItemInfo = new OrderServices.Order_Calrification_ItemInfo();
        //                      oItemClaItemInfo.Clarification_ID = 0;
        //                      oItemClaItemInfo.OrderID = Template_id;
        //                      oItemClaItemInfo.ProductDesc = _pCode;
        //                      oItemClaItemInfo.Quantity = _pQty;
        //                      oItemClaItemInfo.UserID = userid;
        //                      oItemClaItemInfo.Clarification_Type = "TEMP_ITEM_ERROR";
        //                      objOrderServices.AddOrder_ClarificationItem(oItemClaItemInfo);

        //                  }
        //                  else
        //                  {
        //                      ProdAItem[i] = _substitute;
        //                      _pCode = _substitute;

        //                      oItemInFo.ProductID = GetProductID(ProdAItem[i]);

        //                      oItemInFo.TemplateId = Template_id;
        //                      oItemInFo.Quantity = ObjHelperServices.CI(ProdAQty[i]);


        //                      objOrderServices.AddOrderItemTemplateItem(oItemInFo);


        //                  }

        //              }
        //              if (checkset > 0)
        //              {

        //                  oItemInFo.ProductID = GetProductID(ProdAItem[i]);


        //                  oItemInFo.TemplateId = Template_id;
        //                  oItemInFo.Quantity = ObjHelperServices.CI(ProdAQty[i]);

        //                  objOrderServices.AddOrderItemTemplateItem(oItemInFo);

        //              }


        //          }

        //      }





        //      //string enc = System.DateTime.Now.Millisecond.ToString();

        //      //if ((Session["ORDER_ID"] != null && Convert.ToInt32(Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
        //      //{
        //      //    Response.Redirect("OrderDetails.aspx?ORDER_ID=" + Convert.ToInt32(Session["ORDER_ID"]) + "&bulkorder=1&ViewOrder=View&" + enc, true);
        //      //}
        //      //else
        //      //{
        //      //    Response.Redirect("OrderDetails.aspx?bulkorder=1&" + enc, true);

        //      //}

        //      tbErrorItem = objOrderServices.GetOrder_Clarification_Items(Template_id, "TEMP_ITEM_ERROR");
        //      tbErrorChk = objOrderServices.GetOrder_Clarification_Items(Template_id, "TEMP_ITEM_CHK");


        //      if ((tbErrorItem != null && tbErrorItem.Rows.Count > 0) || (tbErrorChk != null && tbErrorChk.Rows.Count > 0))
        //          Response.Redirect("ordertemplate.aspx?bulkorder=1&Tempid=" + Template_id, true);
        //      else
        //          ShowAlertMessageBox("Successfully saved");


        //  }
        //  catch (Exception ex)
        //  {
        //      objErrorHandler.ErrorMsg = ex;
        //      //objErrorHandler.CreateLog();
        //  }

    }


    protected void btnViewTemplate(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("OrderTemplateList.aspx", true);



        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            //objErrorHandler.CreateLog();
        }

    }

    public void Insert_MasterSub(string USER_ID, string Filename, DataTable dt)
    {
        Literal1.Text = "";
        
        try
            {
                string ID = HttpContext.Current.Session["bindid"].ToString();
                //string ID = Session["ID"].ToString();

                ConnectionDB objconn = new ConnectionDB();
                SqlConnection conn = new SqlConnection(objconn.ConnectionString);
                DataSet ds = new DataSet();
                string sqlCmd = "SELECT count(Code) from TBWC_MYPRODUCT_DETAILS where ID='" + ID + "' ";
                SqlCommand cmd = new SqlCommand(sqlCmd, conn);

                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                            int countds = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                if (ds != null)
                {
                    sqlCmd = "SELECT Id,Product_Id,Code,Stock_Status,Comments,Price,Qty from TBWC_MYPRODUCT_DETAILS where ID='" + ID + "'";
                    cmd = new SqlCommand(sqlCmd, conn);

                    da = new SqlDataAdapter(cmd);
                    DataSet ds1 = new DataSet();
                    da.Fill(ds1);

                DateTime dtdatetime = DateTime.Now;
                if (ds1 != null)
                    {
                        dt.Columns.Add("Id", typeof(Int32));
                        dt.Columns.Add("Product_Id", typeof(Int32));
                        dt.Columns.Add("Stock_Status", typeof(String));
                        dt.Columns.Add("Comments", typeof(String));
                        dt.Columns.Add("Price", typeof(decimal));
                        dt.Columns.Add("Qty", typeof(String));
                    dt.Columns.Add("CreatedOn", typeof(DateTime));
                    dt.Columns.Add("ModifiedOn", typeof(DateTime));
                    string id = ds1.Tables[0].Rows[0][0].ToString();
                        string chkCode = "";
                        foreach (DataRow row in dt.Rows)
                        {
                            string code = row[0].ToString();
                            objErrorHandler.CreateLog(code);
                        //string str = "select b.PRODUCT_ID,c.PROD_STOCK_STATUS,c.PROD_STOCK_FLAG,DBO.GetETA(c.PROD_STOCK_STATUS,c.PROD_NEXT_SHIP) AS eta,c.Product_id,c.SOH,Supplier_ID,c.SUPPLIER_SHIP_DAYS,c.PROD_SUBSTITUTE,c.PROD_LEGISTATED_STATE,c.PROD_STK_STATUS_DSC ,dbo.fnGetWESProductPriceNew(b.PRODUCT_ID,1," + user_id + ",'') as Cost  from  TB_PROD_SPECS b    join WESTB_PRODUCT_ITEM c on b.PRODUCT_ID = c.PRODUCT_ID where string_value='" + code + "' and attribute_id=1";
                        string str = "select b.PRODUCT_ID,c.PROD_STOCK_STATUS,c.PROD_STOCK_FLAG,DBO.GetETA(c.PROD_STOCK_STATUS,c.PROD_NEXT_SHIP) AS eta,c.Product_id,c.SOH,Supplier_ID,c.SUPPLIER_SHIP_DAYS,c.PROD_SUBSTITUTE,c.PROD_LEGISTATED_STATE,c.PROD_STK_STATUS_DSC ,dbo.fnGetWESProductPriceNew(b.PRODUCT_ID,1," + user_id + ",'') as Cost  from  TB_PROD_SPECS_Code b    join WESTB_PRODUCT_ITEM c on b.PRODUCT_ID = c.PRODUCT_ID where string_value='" + code + "' ";
                            objErrorHandler.CreateLog(str);
                            SqlDataAdapter da1 = new SqlDataAdapter(str, conn);
                            DataTable dt1 = new DataTable();
                            da1.Fill(dt1);

                            //dt.Columns.Add("FlagVisible_Invisible", typeof(bool));
                            //dt.Columns.Add("StockStatus");
                            if (dt != null)
                            {
                                //string StockStatus = GetStockStatus(dt1.Rows[0]);//srini manually added below 15-10-20

                                if (dt1.Rows.Count > 0)
                                {
                                    string StockStatus = dt1.Rows[0]["PROD_STK_STATUS_DSC"].ToString().Trim().Replace("_", " ");// "IN_STOCK";
                                row["Stock_Status"] = StockStatus.Replace("_", " ");
                                    row["Id"] = ID;
                                    row["Product_Id"] = dt1.Rows[0]["PRODUCT_ID"].ToString();

                                    row["Qty"] = 1;// dt1.Rows[0]["Qty"].ToString();
                                    row["Comments"] = "";
                                    row["Price"] = dt1.Rows[0]["Cost"].ToString();
                                row["CreatedOn"] = dtdatetime;
                                row["ModifiedOn"] = dtdatetime;
                            }
                                else
                                {

                                    string codeChekInvalid = row["Code"].ToString().Replace("\n", String.Empty);

                                    if (codeChekInvalid == code.Replace("\n", String.Empty))
                                    {
                                        row.Delete();
                                        chkCode += code + ',';
                                        myString = "<span id=\"rupspan\" style=\"font-family:rupee;font-size:16px;font-wieght:bold;\">Invalid ordercode '" + chkCode + "' </span>";

                                        Literal1.Text = myString;
                                    }
                                }

                            }
                        }
                        dt.AcceptChanges();


                        DataTable dtnew = (DataTable)ds1.Tables[0];

                        foreach (DataRow drn in dtnew.Rows)
                        {
                            string strCode = drn["Code"].ToString().Replace("\n", String.Empty);

                            foreach (DataRow dr2 in dt.Rows)
                            {
                                string strdr2 = dr2["Code"].ToString().Replace("\n", String.Empty);
                                if (strCode == strdr2)
                                {
                                    dupString += strCode + ',';
                                    dr2.Delete();
                                    duplicateString = "<span id=\"rupspan\" style=\"font-family:rupee;font-size:16px;font-wieght:bold;\">Order code '" + dupString + "' duplicate occurs </span>";
                                    if (chkCode != "")
                                        Literal1.Text = myString + " " + duplicateString;
                                    else Literal1.Text = duplicateString;
                                }

                            }
                            dt.AcceptChanges();
                        }


                        if (dt.Rows.Count > 0)
                        {


                            bulkcopyProd(dt);
                            HttpContext.Current.Session["bindid"] = id;
                            GetDataFromDb(id);
                        }
                        else
                        {
                            myString = "<span id=\"rupspan\" style=\"font-family:rupee;font-size:16px;font-wieght:bold;\">Invalid Order code '" + chkCode + "'   </span>";
                            if (chkCode != "")
                                Literal1.Text = myString + " " + duplicateString;
                            else Literal1.Text = duplicateString;

                            if (dupString == "" && chkCode != "")
                                Literal1.Text = myString;
                            if (dupString == "" && chkCode == "")
                                Literal1.Text = "";
                            GetDataFromDb(id);
                        }
                        //bulkcopyProd(dt);

                        ////  divmyproduct.Visible = true;
                        //HttpContext.Current.Session["bindid"] = id;
                        //GetDataFromDb(id);
                    }


                }

                conn.Close();
            }
            catch (Exception ex)
            {
                objErrorHandler.CreateLog(ex.ToString());
            }
        
    }




    private void Insert_Master(string USER_ID, string Filename, DataTable dt)
    {
        try
        {
            Filename = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Filename.ToLower());
            ConnectionDB objconn = new ConnectionDB();
            SqlConnection conn = new SqlConnection(objconn.ConnectionString);
            DataSet ds = new DataSet();
            string sqlCmd = "SELECT count(FileName) from TBWC_MYProduct where USER_ID='" + USER_ID + "' and FileName='" + Filename + "'";
            SqlCommand cmd = new SqlCommand(sqlCmd, conn);

            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            string id = "";
            if (ds != null)
            {
                if (ds.Tables[0].Rows[0][0].ToString() == "1")
                {
                    // ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "confirm('File Already Exsist, Do you want to Override " + Filename.ToString() + " ?');", true);
                    Overwrite();

                }
                else
                {

                    cmd.CommandText = "INSERT INTO TBWC_MYProduct(USER_ID, FileName)   VALUES(@param1,@param2)";

                    cmd.Parameters.AddWithValue("@param1", USER_ID);
                    cmd.Parameters.AddWithValue("@param2", Filename);


                    cmd.ExecuteNonQuery();
                }

                sqlCmd = "SELECT id from TBWC_MYProduct where USER_ID='" + USER_ID + "' and FileName='" + Filename + "'";
                cmd = new SqlCommand(sqlCmd, conn);


                da = new SqlDataAdapter(cmd);
                DataSet ds1 = new DataSet();
                da.Fill(ds1);
                DateTime dtdatetime = DateTime.Now;
                if (ds1 != null)
                {
                    dt.Columns.Add("Id", typeof(Int32));
                    dt.Columns.Add("Product_Id", typeof(Int32));
                    dt.Columns.Add("Stock_Status", typeof(String));
                    dt.Columns.Add("Comments", typeof(String));
                    dt.Columns.Add("Price", typeof(decimal));
                    dt.Columns.Add("Qty", typeof(Int32));
                    dt.Columns.Add("CreatedOn", typeof(DateTime));
                    dt.Columns.Add("ModifiedOn", typeof(DateTime));
                    string chkCode = "";
                     id = ds1.Tables[0].Rows[0][0].ToString();


                    foreach (DataRow row in dt.Rows)
                    {
                        string code = row[0].ToString();
                        //objErrorHandler.CreateLog(code);
                        //string str = "select b.PRODUCT_ID,c.PROD_STOCK_STATUS,c.PROD_STOCK_FLAG,DBO.GetETA(c.PROD_STOCK_STATUS,c.PROD_NEXT_SHIP) AS eta,c.Product_id,c.SOH,Supplier_ID,c.SUPPLIER_SHIP_DAYS,c.PROD_SUBSTITUTE,c.PROD_LEGISTATED_STATE,c.PROD_STK_STATUS_DSC ,dbo.fnGetWESProductPriceNew(b.PRODUCT_ID,1," + user_id + ",'') as Cost  from  TB_PROD_SPECS b join WESTB_PRODUCT_ITEM c on b.PRODUCT_ID = c.PRODUCT_ID where string_value='" + code + "' and attribute_id=1";
                       string str = "select b.PRODUCT_ID,c.PROD_STOCK_STATUS,c.PROD_STOCK_FLAG,DBO.GetETA(c.PROD_STOCK_STATUS,c.PROD_NEXT_SHIP) AS eta,c.Product_id,c.SOH,Supplier_ID,c.SUPPLIER_SHIP_DAYS,c.PROD_SUBSTITUTE,c.PROD_LEGISTATED_STATE,c.PROD_STK_STATUS_DSC ,dbo.fnGetWESProductPriceNew(b.PRODUCT_ID,1," + user_id + ",'') as Cost  from  TB_PROD_SPECS_Code b join WESTB_PRODUCT_ITEM c on b.PRODUCT_ID = c.PRODUCT_ID where string_value='" + code + "' ";
                        
                        SqlDataAdapter da1 = new SqlDataAdapter(str, conn);
                        DataTable dt1 = new DataTable();
                        da1.Fill(dt1);

                        //dt.Columns.Add("FlagVisible_Invisible", typeof(bool));
                        //dt.Columns.Add("StockStatus");
                        if (dt != null)
                        {
                            if (dt1.Rows.Count > 0)
                            {
                                //  string StockStatus = GetStockStatus(dt1.Rows[0]);  GetStockStatus(dt1.Rows[0]).Trim().Replace("_", " ");
                                row["Stock_Status"] = dt1.Rows[0]["PROD_STK_STATUS_DSC"].ToString().Trim().Replace("_", " "); //dt1.Rows[0]["PROD_STK_STATUS_DSC"].ToString();
                                row["Id"] = id;
                                row["Product_Id"] = dt1.Rows[0]["PRODUCT_ID"].ToString();
                                row["Qty"] = 1;//dt1.Rows[0]["Qty"].ToString();
                                row["Comments"] = "";
                                row["Price"] = dt1.Rows[0]["Cost"].ToString();
                                row["CreatedOn"] = dtdatetime;
                                row["ModifiedOn"] = dtdatetime;
                            }
                            else
                            {
                                chkCode += code + ',';
                                string codeChekInvalid = row["Code"].ToString();
                                myString = "<span id=\"rupspan\" style=\"font-family:rupee;font-size:16px;font-weight:bold;\">Order code('s) '" + chkCode + "'  not inserted  </span>";

                                Literal2.Text = myString;
                                if (codeChekInvalid == code)
                                {
                                    row.Delete();
                                }
                            }
                        }
                    }
                    dt.AcceptChanges();
                    if (dt.Rows.Count > 0)
                    {
                        

                        bulkcopy(dt);
                        HttpContext.Current.Session["bindid"] = id;
                        GetDataFromDb(id);
                    }
                    else 
                    {
                       string strsqlCom = "Delete from TBWC_MYProduct where USER_ID='" + USER_ID + "' and Id=" + id + " ";
                       SqlCommand scmd = new SqlCommand(strsqlCom, conn);
                       scmd.ExecuteNonQuery();

                        myString = "<span id=\"rupspan\" style=\"font-family:rupee;font-size:16px;font-wieght:bold;\">Invalid Order code '" + chkCode + "'  </span>";
                        Literal3.Text = myString;
                        Literal2.Text = "";
                    }

                  
                    
                    //  divmyproduct.Visible = true;
                   
                }


            }

            conn.Close();


            ConnectionDB objconn1 = new ConnectionDB();
            var con1 = new SqlConnection(objconn1.ConnectionString);
            con1.Open();
            var da1c = new SqlDataAdapter("select FileName  from [TBWC_MYPRODUCT] where Id=" + id + " ", con1);
            var dt1c = new DataTable();
            da1c.Fill(dt1c);
            con1.Close();
            lbltempname.Text = dt1c.Rows[0][0].ToString();
            HttpContext.Current.Session["TemplateName"] = lbltempname.Text;
        }

        catch (Exception ex)
        {
            objErrorHandler.CreateLog(ex.ToString());
        }
    }
    private void Overwrite()
    {
        try
        {
            ConnectionDB objconn = new ConnectionDB();
            SqlConnection conn = new SqlConnection(objconn.ConnectionString);
            conn.Open();
            string Filename = HttpContext.Current.Session["fileName"].ToString();
            string str = " update TBWC_MYProduct set ModifiedOn = getdate() where user_id = '" + user_id + "' and FileName = '" + Filename + "'";
            objErrorHandler.CreateLog(str);
            SqlCommand cmd = new SqlCommand(str, conn);




            cmd.ExecuteNonQuery();

        }
        catch (Exception ex)
        {
            objErrorHandler.CreateLog(ex.ToString());
        }
    }
    private void bulkcopyProd(DataTable dt)
    {
        try
        {
            ConnectionDB objconn = new ConnectionDB();
            SqlConnection conn = new SqlConnection(objconn.ConnectionString);
            conn.Open();

            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(objconn.ConnectionString))
            {
                //Set the database table name
                sqlBulkCopy.DestinationTableName = "dbo.TBWC_MYPRODUCT_DETAILS";




                //[OPTIONAL]: Map the DataTable columns with that of the database table
                sqlBulkCopy.ColumnMappings.Add("Id", "Id");


                sqlBulkCopy.ColumnMappings.Add("Product_Id", "Product_Id");
                sqlBulkCopy.ColumnMappings.Add("Code", "Code");
                sqlBulkCopy.ColumnMappings.Add("Stock_Status", "Stock_Status");
                sqlBulkCopy.ColumnMappings.Add("Comments", "Comments");
                sqlBulkCopy.ColumnMappings.Add("Price", "Price");
                sqlBulkCopy.ColumnMappings.Add("QTY", "Qty");
                sqlBulkCopy.ColumnMappings.Add("CreatedOn", "CreatedOn");
                sqlBulkCopy.ColumnMappings.Add("ModifiedOn", "ModifiedOn");

                sqlBulkCopy.WriteToServer(dt);
                conn.Close();

            }
        }
        catch (Exception ex)
        {
            objErrorHandler.CreateLog(ex.ToString());
        }
    }
    private void bulkcopy(DataTable dt)
    {
        try
        {
            ConnectionDB objconn = new ConnectionDB();
            SqlConnection conn = new SqlConnection(objconn.ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Delete from TBWC_MYProduct_details where id='" + dt.Rows[0]["ID"].ToString() + "'", conn);




            cmd.ExecuteNonQuery();
            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(objconn.ConnectionString))
            {
                //Set the database table name
                sqlBulkCopy.DestinationTableName = "dbo.TBWC_MYPRODUCT_DETAILS";




                //[OPTIONAL]: Map the DataTable columns with that of the database table
                sqlBulkCopy.ColumnMappings.Add("Id", "Id");


                sqlBulkCopy.ColumnMappings.Add("Product_Id", "Product_Id");
                sqlBulkCopy.ColumnMappings.Add("Code", "Code");
                sqlBulkCopy.ColumnMappings.Add("Stock_Status", "Stock_Status");
                sqlBulkCopy.ColumnMappings.Add("Comments", "Comments");
                sqlBulkCopy.ColumnMappings.Add("Price", "Price");
                sqlBulkCopy.ColumnMappings.Add("QTY", "Qty");
                sqlBulkCopy.ColumnMappings.Add("CreatedOn", "CreatedOn");
                sqlBulkCopy.ColumnMappings.Add("ModifiedOn", "ModifiedOn");


                sqlBulkCopy.WriteToServer(dt);
                conn.Close();

            }
        }
        catch (Exception ex)
        {
            objErrorHandler.CreateLog(ex.ToString());
        }
    }
    // Bind PagedDataSource into Repeater
    private void BindDataIntoRepeater()
    {
        
        Literal1.Text = Literal2.Text = Literal3.Text = "";
        if (HttpContext.Current.Session["bindid"] != null)
        {
            GetDataFromDb(HttpContext.Current.Session["bindid"].ToString());
        }


        _pgsource.AllowPaging = true;
        // Number of items to be displayed in the Repeater
        _pgsource.PageSize = _pageSize;
        _pgsource.CurrentPageIndex = CurrentPage;
        // Keep the Total pages in View State
        ViewState["TotalPages"] = _pgsource.PageCount;
        // Example: "Page 1 of 10"
        //lblpage.Text = "Page " + (CurrentPage + 1) + " of " + _pgsource.PageCount;
        //// Enable First, Last, Previous, Next buttons
        //lbPrevious.Enabled = !_pgsource.IsFirstPage;
        //lbNext.Enabled = !_pgsource.IsLastPage;
        //lbFirst.Enabled = !_pgsource.IsFirstPage;
        //lbLast.Enabled = !_pgsource.IsLastPage;

        // Bind data into repeaterftrue
        //rptResult.DataSource = _pgsource;
        //rptResult.DataBind();


        // Call the function to do paging
        //      HandlePaging();
    }

    private void HandlePaging()
    {
        var dt = new DataTable();
        dt.Columns.Add("PageIndex"); //Start from 0
        dt.Columns.Add("PageText"); //Start from 1

        _firstIndex = CurrentPage - 5;
        if (CurrentPage > 5)
            _lastIndex = CurrentPage + 5;
        else
            _lastIndex = 10;

        // Check last page is greater than total page then reduced it to total no. of page is last index
        if (_lastIndex > Convert.ToInt32(ViewState["TotalPages"]))
        {
            _lastIndex = Convert.ToInt32(ViewState["TotalPages"]);
            _firstIndex = _lastIndex - 10;
        }

        if (_firstIndex < 0)
            _firstIndex = 0;

        // Now creating page number based on above first and last page index
        for (var i = _firstIndex; i < _lastIndex; i++)
        {
            var dr = dt.NewRow();
            dr[0] = i;
            dr[1] = i + 1;
            dt.Rows.Add(dr);
        }

        //rptPaging.DataSource = dt;
        //rptPaging.DataBind();
    }

    protected void lbFirst_Click(object sender, EventArgs e)
    {
        CurrentPage = 0;
        BindDataIntoRepeater();
    }
    protected void lbLast_Click(object sender, EventArgs e)
    {
        CurrentPage = (Convert.ToInt32(ViewState["TotalPages"]) - 1);
        BindDataIntoRepeater();
    }
    protected void lbPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        BindDataIntoRepeater();
    }
    protected void lbNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        BindDataIntoRepeater();
    }

    protected void rptPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (!e.CommandName.Equals("newPage")) return;
        CurrentPage = Convert.ToInt32(e.CommandArgument.ToString());
        BindDataIntoRepeater();
    }



    protected void Gridprodlst_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Gridprodlst.PageIndex = e.NewPageIndex;
        BindDataIntoRepeater();
    }

    protected void Gridprodlst_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void Gridprodlst_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.Pager)
        //{
        //    Table pagerTable = (Table)e.Row.Cells[0].Controls[0];
        //    TableRow pagerRow = pagerTable.Rows[0];
        //    PagerSettings pagerSettings = ((GridView)sender).PagerSettings;
        //    int cellsCount = pagerRow.Cells.Count;



        //    if (pagerSettings.Mode == PagerButtons.Numeric
        //                     || pagerSettings.Mode == PagerButtons.NumericFirstLast)
        //    {
        //        int prevButtonIndex = pagerSettings.Mode == PagerButtons.Numeric ? 0 : 1;
        //        int nextButtonIndex = pagerSettings.Mode == PagerButtons.Numeric ? cellsCount - 1 : cellsCount - 2;


        //        if (prevButtonIndex < cellsCount)
        //        {
        //            //check whether previous button exists 
        //            LinkButton btnPrev = pagerRow.Cells[prevButtonIndex].Controls[0] as LinkButton;
        //            if (btnPrev != null && btnPrev.Text.IndexOf("...") != -1)
        //            {
        //                btnPrev.Text = pagerSettings.PreviousPageText;
        //                //btnPrev.CommandName = "Page";
        //                //btnPrev.CommandArgument = "Prev";
        //            }
        //        }
        //        if (nextButtonIndex > 0 && nextButtonIndex < cellsCount)
        //        {
        //            //check whether next button exists 
        //            LinkButton btnNext = pagerRow.Cells[nextButtonIndex].Controls[0] as LinkButton;
        //            if (btnNext != null && btnNext.Text.IndexOf("...") != -1)
        //            {
        //                btnNext.Text = pagerSettings.NextPageText;
        //                //btnNext.CommandName = "Page";
        //                //btnNext.CommandArgument = "Next";
        //            }
        //        }

        //    }

        //    foreach (TableCell cell in pagerRow.Cells)
        //    {


        //        Control lb = cell.Controls[0];
        //        if (lb is Label)
        //        {

        //            cell.Style.Value = "padding-left:5px;padding-right:5px;background-color:#C6E8F5";

        //        }
        //        else if (lb is LinkButton)
        //        {

        //            cell.Style.Value = "background-color: #E0E3E8;padding-left:5px;padding-right:5px;";
        //            cell.Attributes.Add("onmouseover", "this.style.cursor='hand';this.style.backgroundColor='#C6E8F5';");
        //            cell.Attributes.Add("onmouseout", "this.style.backgroundColor='#E6E7EC';");
        //            ((LinkButton)lb).Style.Value = "text-decoration:none;";
        //        }
        //    }

        //}

        if (e.Row.RowType == DataControlRowType.Pager)
        {
            Table pagerTable = (Table)e.Row.Cells[0].Controls[0];
            TableRow pagerRow = pagerTable.Rows[0];
            PagerSettings pagerSettings = ((GridView)sender).PagerSettings;
            int cellsCount = pagerRow.Cells.Count;



            if (pagerSettings.Mode == PagerButtons.Numeric
                             || pagerSettings.Mode == PagerButtons.NumericFirstLast)
            {
                int prevButtonIndex = pagerSettings.Mode == PagerButtons.Numeric ? 0 : 1;
                int nextButtonIndex = pagerSettings.Mode == PagerButtons.Numeric ? cellsCount - 1 : cellsCount - 2;


                if (prevButtonIndex < cellsCount)
                {
                    //check whether previous button exists 
                    LinkButton btnPrev = pagerRow.Cells[prevButtonIndex].Controls[0] as LinkButton;
                    if (btnPrev != null && btnPrev.Text.IndexOf("...") != -1)
                    {
                        btnPrev.Text = pagerSettings.PreviousPageText;
                        //btnPrev.CommandName = "Page";
                        //btnPrev.CommandArgument = "Prev";
                    }
                }
                if (nextButtonIndex > 0 && nextButtonIndex < cellsCount)
                {
                    //check whether next button exists 
                    LinkButton btnNext = pagerRow.Cells[nextButtonIndex].Controls[0] as LinkButton;
                    if (btnNext != null && btnNext.Text.IndexOf("...") != -1)
                    {
                        btnNext.Text = pagerSettings.NextPageText;
                        //btnNext.CommandName = "Page";
                        //btnNext.CommandArgument = "Next";
                    }
                }

            }

            foreach (TableCell cell in pagerRow.Cells)
            {


                Control lb = cell.Controls[0];
                if (lb is Label)
                {

                    cell.Style.Value = "padding-left:5px;padding-right:5px;background-color:transparent;color:#58d1ff;";

                }
                else if (lb is LinkButton)
                {

                    cell.Style.Value = "background-color: transparent;padding-left:5px;padding-right:5px;";
                    cell.Attributes.Add("onmouseover", "this.style.cursor='hand';this.style.backgroundColor='transparent';");
                    cell.Attributes.Add("onmouseout", "this.style.backgroundColor='transparent';");
                    ((LinkButton)lb).Style.Value = "text-decoration:none;";
                }
            }

        }
    }

    protected void rptPaging_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        var lnkPage = (LinkButton)e.Item.FindControl("lbPaging");
        if (lnkPage.CommandArgument != CurrentPage.ToString()) return;
        lnkPage.Enabled = false;
        lnkPage.BackColor = Color.FromName("#00FF00");
    }

    protected void btnOTClose_click(object sender, EventArgs e)
    {
        msgAlert.Hide();
        msgOTAlert.Hide();
        plnOrderTemplate.Visible = false;
        pnlAlert.Visible = false;
    }


    private void AddOrderTemplateProductItem(int prod_id, int qty, int template_id)
    {
        OrderServices.OrderTemplateItemInfo oItemInFo = new OrderServices.OrderTemplateItemInfo();
        oItemInFo.ProductID = prod_id;


        oItemInFo.TemplateId = template_id;
        oItemInFo.Quantity = qty;

        objOrderServices.AddOrderItemTemplateItem(oItemInFo);
    }
    public string GetLeaveDuplicateProducts()
    {

        try
        {
            string LeaveDuplicateProds = "";
            if (HttpContext.Current.Session["LeaveDuplicateProds"] != null && HttpContext.Current.Session["LeaveDuplicateProds"].ToString() != "")
            {
                LeaveDuplicateProds = HttpContext.Current.Session["LeaveDuplicateProds"].ToString();
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



    protected void ImageButtonDelete_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btn = sender as ImageButton;
        GridViewRow row = btn.NamingContainer as GridViewRow;
        string orderCode = Gridprodlst.DataKeys[row.RowIndex].Values["CODE"].ToString();
        int id = Convert.ToInt32(HttpContext.Current.Session["bindid"]);
        string query = "delete from TBWC_MYPRODUCT_DETAILS where Code= '" + orderCode + "' and Id = " + id + "";
        try
        {
            ConnectionDB objconn = new ConnectionDB();
            SqlConnection conn = new SqlConnection(objconn.ConnectionString);
            conn.Open();
            objErrorHandler.CreateLog(query);
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();

          //  GetDataFromDb(Convert.ToString(id));
            GetDataFromDb1(orderCode, id);
        }
        catch (Exception ex)
        {

            objErrorHandler.CreateLog(ex.ToString());
        }


    }

    protected void Imagedelete_mainProduct_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btn = sender as ImageButton;
        GridViewRow row = btn.NamingContainer as GridViewRow;
        string ID = dgtemplate.DataKeys[row.RowIndex].Values["ID"].ToString();

        HttpContext.Current.Session["ID"] = ID;
        string queryMain = "delete from TBWC_MyProduct where ID= '" + ID + "' ";
        string querySub = "delete from TBWC_MYPRODUCT_DETAILS where ID= '" + ID + "' ";

        try
        {
            ConnectionDB objconn = new ConnectionDB();
            SqlConnection conn = new SqlConnection(objconn.ConnectionString);
            conn.Open();
            objErrorHandler.CreateLog(queryMain);
            SqlCommand cmd = new SqlCommand(queryMain, conn);
            cmd.ExecuteNonQuery();

            //
            ConnectionDB objconnS = new ConnectionDB();
            SqlConnection connS = new SqlConnection(objconn.ConnectionString);
            connS.Open();
            objErrorHandler.CreateLog(querySub);
            SqlCommand cmdS = new SqlCommand(querySub, connS);
            cmdS.ExecuteNonQuery();

            Response.Redirect("/byProduct.aspx");
            //GetDataFromDb2(ID);

        }
        catch (Exception ex)
        {

            objErrorHandler.CreateLog(ex.ToString());
        }
    }

    protected void btnBuytoadd_Click(object sender, EventArgs e)
    {
        Button lbtn = (Button)sender;
        GridViewRow row = (GridViewRow)lbtn.NamingContainer;
        TextBox txtqty = Gridprodlst.Rows[row.RowIndex].FindControl("txtqty") as TextBox;


        if (row != null)
        {
            if (HttpContext.Current.Session["txtqty"] != null)
            {
                HttpContext.Current.Session["txtqty"] = HttpContext.Current.Session["txtqty"] + "," + txtqty.Text;
            }
            else
            {

                HttpContext.Current.Session["txtqty"] = txtqty.Text;
            }

            if (HttpContext.Current.Session["txtcode"] != null)
            {
                HttpContext.Current.Session["txtcode"] = HttpContext.Current.Session["txtcode"] + "," + Gridprodlst.DataKeys[row.RowIndex].Value.ToString();
            }
            else
            {

                HttpContext.Current.Session["txtcode"] = Gridprodlst.DataKeys[row.RowIndex].Value.ToString();
            }

            AddToCart(txtqty.Text, Gridprodlst.DataKeys[row.RowIndex].Value.ToString());

        }
    }

    protected void LinkButton3_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)HttpContext.Current.Session["dtProdList"];
        DataTable dtCode_ = new DataTable();
        dtCode_.Columns.Add("Code");
        dtCode_.Columns.Add("Cost");
        dtCode_.Columns.Add("StoctStatus");
        foreach (DataRow dr in dt.Rows)
        {
            dtCode_.Rows.Add(dr["Code"], dr["Price"], dr["Stock_Status"]);
        }


        if (dtCode_.Rows.Count > 0)
        {
            string filename = "ProductList.xls";
            System.IO.StringWriter tw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
            DataGrid dgGrid = new DataGrid();
            dgGrid.DataSource = dtCode_;
            dgGrid.DataBind();


            dgGrid.RenderControl(hw);

            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
            this.EnableViewState = false;
            Response.Write(tw.ToString());
            Response.End();
        }



    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        string ErrItems = "", ClrItems = "", ClrQty = "";
        int DuplicateItem_Prod_idCount = 0;
        string LeaveDuplicateProds = GetLeaveDuplicateProducts();

        if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (Request.QueryString["ViewOrder"] != null && Request.QueryString["ViewOrder"].ToString().Equals("View")))
        {
            OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]);

        }
        else
        {
            OrderID = objOrderServices.GetOrderID(objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString()), OpenOrdStatusID);

        }
        DataSet dsDuplicateItem_Prod_id = objOrderServices.GetOrderItemsWithDuplicate_Prod_id(OrderID, LeaveDuplicateProds);

        if (dsDuplicateItem_Prod_id != null && dsDuplicateItem_Prod_id.Tables.Count > 0 && dsDuplicateItem_Prod_id.Tables[0].Rows.Count > 0)
        {
            DuplicateItem_Prod_idCount = dsDuplicateItem_Prod_id.Tables[0].Rows.Count;
        }

        //if (HttpContext.Current.Session["ITEM_ERROR"] != null || Session["ITEM_CHK"] != null)
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
                OrderID = objOrderServices.GetOrderID(objHelperServices.CI(HttpContext.Current.Session["USER_ID"].ToString()), OpenOrdStatusID);
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

}
