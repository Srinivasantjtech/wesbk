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
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk ;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using System.Reflection;
using System.Linq;
//using System.Diagnostics;
public partial class UC_categorylist : System.Web.UI.UserControl
{
  //  Stopwatch stopwatch = new Stopwatch();
    string stemplatepath = string.Empty;
    ErrorHandler objErrorHandler=new ErrorHandler();
    ConnectionDB objConnectionDB = new ConnectionDB();
    UserServices objUserServices = new UserServices();
    ProductServices objProductServices = new ProductServices();
    Security objSecurity = new Security();
    string _catId = string.Empty;
    string _catName = string.Empty;
    string _catalogid = string.Empty;
  
    CategoryServices objCategoryServices = new CategoryServices();
    int iRecordsPerPage = 16;
    HelperDB objHelperDB = new HelperDB();
    HelperServices objHelperServices = new HelperServices();

    string strFile = HttpContext.Current.Server.MapPath("ProdImages");
    EasyAsk_WES EasyAsk = new EasyAsk_WES();
    public string WesNewsCategoryId = System.Configuration.ConfigurationManager.AppSettings["WESNEWS_CATEGORY_ID"].ToString();


   // int iCatalogId;
   // int iInventoryLevelCheck;
   // bool bIsStartOver = true;
  //  string sSortBy = "";
   // bool bDoPaging;
    int iPageNo = 1;
   // bool bSortAsc = true;
    int iTotalPages = 0;
    int iTotalProducts = 0;
   // int iTmpProductId = 0;
  //  int iPrevPgNo = 1;
   // int iNextPgNo = 1;


    string _tsb = string.Empty;
    string _tsm = string.Empty;
    string _type = string.Empty;
    string _value = string.Empty;
    string _bname = string.Empty;
    string _searchstr = string.Empty;
    string _byp = "2";
   // string _bypcat = null;
   // string _pid = "";
  //  string _fid = "";
    string _cid = string.Empty;
    string _EAPath = string.Empty;
    string _ParentCatID = string.Empty;
    string _pcr = string.Empty;
    public string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        stemplatepath = Server.MapPath("Templates");
       // Page.Title = oHelper.GetOptionValues("BROWSER TITLE").ToString();
        _catalogid = objHelperServices.GetOptionValues("DEFAULT CATALOG").ToString();
        GetPageConfig();
        
        if (IsPostBack)
        {

        }
        else
        {
            if (Request.QueryString["pgno"] != null)
            {
                iPageNo = Convert.ToInt32(Request.QueryString["pgno"]);
            }

            hforgurl.Value = HttpContext.Current.Request.Url.PathAndQuery.ToString();
            HiddenField1.Value = "0";
            HiddenField2.Value = "0";
            hfcheckload.Value = "0";
            HFcnt.Value = "1";
            hfback.Value = "";
            hfbackdata.Value = "";
        }
    }
    private void GetPageConfig()
    {
        try
        {
            //if (Session["PS_IS_START_OVER"].ToString() == "YES")
            //{
            //    bIsStartOver = true;
            //}
            //else
            //{
            //    bIsStartOver = false;
            //}
        
            if (HidItemPage.Value.ToString() == string.Empty || HidItemPage.Value.ToString() == null)
            {
                if (Session["RECORDS_PER_PAGE_CATEGORY_LIST"] != null)
                    iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_CATEGORY_LIST"].ToString());
            }
            else
            {
                iRecordsPerPage = Convert.ToInt32(HidItemPage.Value.ToString());
                Session["RECORDS_PER_PAGE_CATEGORY_LIST"] = HidItemPage.Value.ToString();
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    protected string ST_CategoryList()
    {

        return (Category_RenderHTML("CATEGORY", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString())));
    }
    protected string ST_CategoryProductList()
    {
        return (CategoryProductList_RenderHTMLJson("CATEGORYPRODUCTLIST", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString())));
       // TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("CATEGORYPRODUCTLIST", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
        

    }
    protected string ST_newproductsnav()
    {
        ConnectionDB objConnectionDB = new ConnectionDB();
        HelperServices objHelperServices = new HelperServices();

      TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("NEWPRODUCTHIGHLIGHTSCATLIST", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);

      //tbwtEngine.RenderHTML("Column");

       // return (tbwtEngine.RenderedHTML);
        return tbwtEngine.ST_NewProduct_Highlights_cat_list_Load();
        //return (newproductsnav_RenderHTML("NEWPRODUCTHIGHLIGHTSCATLIST", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString())));
    }
    private void SubCategoryDisplay( StringTemplate bodyST_subcategorylist, DataRow drow)
    {
                
        if (WesNewsCategoryId == _catId)
            bodyST_subcategorylist.SetAttribute("TBT_CATEGORY_WITH_CAT_ID", drow["CATEGORY_ID"].ToString());



        bodyST_subcategorylist.SetAttribute("TBT_CATEGORY_NAME", drow["CATEGORY_NAME"].ToString());
        bodyST_subcategorylist.SetAttribute("TBT_CATEGORY_ID", HttpUtility.UrlEncode(drow["CATEGORY_ID"].ToString()));
        if (drow["CUSTOM_NUM_FIELD3"] != System.DBNull.Value)
        {
            bodyST_subcategorylist.SetAttribute("TBT_CUSTOM_NUM_FIELD3", Convert.ToInt32(drow["CUSTOM_NUM_FIELD3"]).ToString());
        }
        else
        {
            bodyST_subcategorylist.SetAttribute("TBT_CUSTOM_NUM_FIELD3", "");
        }
        bodyST_subcategorylist.SetAttribute("TBT_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(drow["PARENT_CATEGORY_ID"].ToString()));

        bodyST_subcategorylist.SetAttribute("TBT_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(""));
        bodyST_subcategorylist.SetAttribute("TBT_ATTRIBUTE_TYPE", "Category");
        bodyST_subcategorylist.SetAttribute("TBT_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(drow["CATEGORY_NAME"].ToString()));
        bodyST_subcategorylist.SetAttribute("TBT_ATTRIBUTE_BRAND", "");

        bodyST_subcategorylist.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(drow["EA_PATH"].ToString())));
        bodyST_subcategorylist.SetAttribute("divid", drow["CATEGORY_ID"].ToString());

             
    }
    private void MainCategoryDisplay(StringTemplate bodyST_categorylist, DataRow _dsrow)
    {

        
        if (WesNewsCategoryId == _catId)
            bodyST_categorylist.SetAttribute("TBT_CATEGORY_WITH_CAT_ID", _dsrow["CATEGORY_ID"].ToString());
        
            bodyST_categorylist.SetAttribute("TBT_CATEGORY_NAME", _dsrow["CATEGORY_NAME"].ToString());

        bodyST_categorylist.SetAttribute("TBT_CATEGORY_ID", HttpUtility.UrlEncode(_dsrow["CATEGORY_ID"].ToString()));
        if (_dsrow["CUSTOM_NUM_FIELD3"] != System.DBNull.Value)
        {
            bodyST_categorylist.SetAttribute("TBT_CUSTOM_NUM_FIELD3", Convert.ToInt32(_dsrow["CUSTOM_NUM_FIELD3"]).ToString());
        }
        else
        {
            bodyST_categorylist.SetAttribute("TBT_CUSTOM_NUM_FIELD3", "");
        }
        bodyST_categorylist.SetAttribute("TBT_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(_dsrow["PARENT_CATEGORY_ID"].ToString()));

        bodyST_categorylist.SetAttribute("TBT_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(""));
        bodyST_categorylist.SetAttribute("TBT_ATTRIBUTE_TYPE", "Category");
        bodyST_categorylist.SetAttribute("TBT_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(_dsrow["CATEGORY_NAME"].ToString()));
        bodyST_categorylist.SetAttribute("TBT_ATTRIBUTE_BRAND", "");

        bodyST_categorylist.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_dsrow["EA_PATH"].ToString())));                                                    
                        


        //bodyST_categorylist.SetAttribute("TBT_CATEGORY_NAME", _dsrow["CATEGORY_NAME"].ToString());
        //bodyST_categorylist.SetAttribute("TBT_CATEGORY_ID", _dsrow["CATEGORY_ID"].ToString());





        //FileInfo Fil = new FileInfo(strFile + _dsrow["IMAGE_FILE"].ToString());
        //if (Fil.Exists)
        //{
        //    bodyST_categorylist.SetAttribute("TBT_IMAGE_FILE", _dsrow["IMAGE_FILE"].ToString().Replace("\\", "/"));
        //}
        //else
        //{
        //    bodyST_categorylist.SetAttribute("TBT_IMAGE_FILE", "");
        //}

        //if (_dsrow["CUSTOM_NUM_FIELD3"] != null && _dsrow["CUSTOM_NUM_FIELD3"].ToString() != "")
        //{
        //    if (Convert.ToInt32(_dsrow["CUSTOM_NUM_FIELD3"]) == 2)
        //    {
        //        //bodyST_categorylist.SetAttribute("TBT_URL", "byproduct.aspx");
        //        bodyST_categorylist.SetAttribute("TBT_URL", "product_list.aspx");
        //        bodyST_categorylist.SetAttribute("TBT_CUSTOM_NUM_FIELD3", Convert.ToInt32(_dsrow["CUSTOM_NUM_FIELD3"]).ToString() + "&pcr=" + _catId);
        //    }
        //    else
        //    {
        //        bodyST_categorylist.SetAttribute("TBT_URL", "product_list.aspx");
        //        bodyST_categorylist.SetAttribute("TBT_CUSTOM_NUM_FIELD3", Convert.ToInt32(_dsrow["CUSTOM_NUM_FIELD3"]).ToString());
        //    }

        //}
        //else
        //{
        //    bodyST_categorylist.SetAttribute("TBT_URL", "product_list.aspx");
        //    bodyST_categorylist.SetAttribute("TBT_CUSTOM_NUM_FIELD3", "1");
        //}
    }
    public string CategoryProductList_RenderHTML(string package, string SkinRootPath)
    {
        //if (Session["PS_SEARCH_RESULTS"] == null)
        //{
        //    return "";
        //}
        FamilyServices objFamilyServices = new FamilyServices();
        StringTemplateGroup _stg_container = null;
        StringTemplateGroup _stg_records = null;
        StringTemplate _stmpl_container = null;
        StringTemplate _stmpl_records = null;
        StringTemplate _stmpl_pages = null;
        DataSet dsprod = new DataSet();
        DataSet dsprodspecs = new DataSet();
        DataSet dscat = new DataSet();
        string strFile = HttpContext.Current.Server.MapPath("ProdImages");

        int oe = 0;
        string category_nameh = string.Empty;
        string sHTML = string.Empty;
        string _pcr = string.Empty;
        string _ViewType = string.Empty;
     
    //string catID = "";
   
   

        //if (Convert.ToInt32(Session["PS_SEARCH_RESULTS"].ToString()) > 0)
        //{
        try
        {

            if (Request.QueryString["tsm"] != null && Request.QueryString["tsm"].ToString() != "")
                _tsm = Request.QueryString["tsm"];

            if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "")
                _tsb = Request.QueryString["tsb"];

            if (Request.QueryString["type"] != null)
                _type = Request.QueryString["type"];

            if (Request.QueryString["value"] != null)
                _value = Request.QueryString["value"];

            if (Request.QueryString["bname"] != null)
                _bname = Request.QueryString["bname"];
            if (Request.QueryString["searchstr"] != null)
                _searchstr = Request.QueryString["searchstr"];
            if (Request.QueryString["srctext"] != null)
                _searchstr = Request.QueryString["srctext"];

            if (Request.QueryString["cid"] != null)
                _cid = Request.QueryString["cid"];

            if (Request.QueryString["pcr"] != null)
                _pcr = Request.QueryString["pcr"];


            if (Request.QueryString["ViewMode"] != null)
            {
                _ViewType = Request.QueryString["ViewMode"];
                Session["PL_VIEW_MODE"] = _ViewType;
            }
            else if (Session["PL_VIEW_MODE"] != null && Session["PL_VIEW_MODE"].ToString() != "")
                _ViewType = Session["PL_VIEW_MODE"].ToString();
            else
                _ViewType = "GV";

            //if (Request.QueryString["path"] != null)
            //    _EAPath = HttpUtility.UrlDecode(objSecurity.StringDeCrypt(Request.QueryString["path"].ToString()));

            if (HttpContext.Current.Session["EA"] != null)
                _EAPath = HttpContext.Current.Session["EA"].ToString();


            stemplatepath = Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());


                         
                dscat = (DataSet)HttpContext.Current.Session["FamilyProduct"];
        


            if (dscat != null)
            {

                if (Request.QueryString["pcr"] != null)
                    _pcr = Request.QueryString["pcr"];
                if (Request.QueryString["type"] == null || Request.QueryString["type"] == "")
                {
                    //GetDataSet("SELECT CATEGORY_NAME  FROM	TB_CATEGORY WHERE	CATEGORY_ID ='" + catID + "'").Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
                    string tempstr = (string)objHelperDB.GetGenericDataDB(_cid, "GET_CATEGORY_NAME", HelperDB.ReturnType.RTString);
                    if (tempstr != null && tempstr != "")
                        category_nameh = tempstr;
                }
                else
                    category_nameh = Request.QueryString["value"].ToString();

                DataRow drpagect = dscat.Tables[0].Rows[0];

                if (drpagect.Table.Rows.Count > 0)
                {
                    iTotalPages = Convert.ToInt32(drpagect["TOTAL_PAGES"]);
                }

                if (iPageNo > iTotalPages)
                {
                    iPageNo = iTotalPages;
                    //ps.PAGE_NO = iPageNo;
                }
                Session["iTotalPages"] = iTotalPages;
                DataRow drproductsct = dscat.Tables[1].Rows[0];
                iTotalProducts = Convert.ToInt32(drproductsct["TOTAL_PRODUCTS"]);
                if (_cid != "")
                    _ParentCatID = GetParentCatID(_cid);

                if ((iTotalPages <= 0 || iTotalProducts <= 0) && WesNewsCategoryId == _ParentCatID)
                {


                    _stg_container = new StringTemplateGroup("CategoryProductListmain", stemplatepath);
                    _stmpl_container = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "advmain");

                   // SetEbookAndPDFLink(_stmpl_container);
                    DataTable tmpDataTable = (DataTable)objHelperDB.GetGenericDataDB(WesCatalogId, "'" + _cid + "'", "GET_CATEGORY", HelperDB.ReturnType.RTTable);
                    if (tmpDataTable != null && tmpDataTable.Rows.Count > 0)
                    {
                        //modified by:indu
                        string ebookpath = tmpDataTable.Rows[0]["CUSTOM_TEXT_FIELD3"].ToString();

                        if (ebookpath.Contains("www."))
                        {

                            ebookpath = ebookpath.ToLower().Replace("attachments", "").Replace("\\", "").Replace("http://", "").Replace("https://", "");
                            ebookpath = "http://" + ebookpath;
                        }
                        _stmpl_container.SetAttribute("TBT_ADV_URL_LINK", ebookpath);
                    }
                    _stmpl_container.SetAttribute("TBW_CATEGORY_NAME", category_nameh);
                    return _stmpl_container.ToString();
                }



                TBWDataList[] lstrecords = new TBWDataList[0];
                TBWDataList[] lstrows = new TBWDataList[0];
                _stg_records = new StringTemplateGroup("CategoryProductList", stemplatepath);


                int ictrecords = 0;
                int icolstart = 0;
                string trmpstr = string.Empty;
                int icol = 1;
                lstrows = new TBWDataList[icol];

                if (_ViewType == "GV")
                {
                    icol = 4;
                    lstrows = new TBWDataList[icol];
                }
                else
                {
                    icol = 1;
                    lstrows = new TBWDataList[icol];
                }

                //if (dscat.Tables[0].Rows.Count < icol)
                //{
                //    icol = dscat.Tables[0].Rows.Count;
                //}
                string soddevenrow = "odd";

                DataRow[] drprodcoll = dscat.Tables[2].Select("ATTRIBUTE_ID = 1");


                string userid = string.Empty;
                string PriceTable = string.Empty;
                int pricecode = 0;
                string tmpProds = string.Empty;
                DataSet dsBgDisc = new DataSet();
                DataSet dsPriceTableAll = new DataSet();
                if (Session["USER_ID"] != null)
                    userid = Session["USER_ID"].ToString();
                if (userid == string.Empty)
                    userid = "0";

                string _Buyer_Group = objFamilyServices.GetBuyerGroup(Convert.ToInt32(userid));
                if (Convert.ToInt32(userid) > 0)
                {

                    dsBgDisc = objFamilyServices.GetBuyerGroupBasedDiscountDetails(_Buyer_Group);
                }
                else
                {
                    dsBgDisc = objFamilyServices.GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
                }
                pricecode = objHelperDB.GetPriceCode(userid);

                if (Session["RECORDS_PER_PAGE_CATEGORY_LIST"] != null)
                    iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_CATEGORY_LIST"].ToString());
        
                tmpProds = "";
                if (Convert.ToInt32(userid) > 0)
                {
                    foreach (DataRow drpid in drprodcoll)
                    {
                        tmpProds = tmpProds + drpid["PRODUCT_ID"].ToString() + ",";
                    }
                    if (tmpProds != "")
                    {
                        tmpProds = tmpProds.Substring(0, tmpProds.Length - 1);
                      //  dsPriceTableAll = objHelperDB.GetProductPriceTableAll(tmpProds, Convert.ToInt32(userid));
                    }
                }


                lstrecords = new TBWDataList[drprodcoll.Length + 1];
                string CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(userid));
                foreach (DataRow drpid in drprodcoll)
                {
                    oe++;
                    if ((oe % 2) == 0)
                    {
                        soddevenrow = "even";
                    }
                    else
                    {
                        soddevenrow = "odd";
                    }

                    if (_ViewType == "GV")
                        _stmpl_records = _stg_records.GetInstanceOf("CategoryProductList" + "\\" + "productlist_GridView");
                    else
                        _stmpl_records = _stg_records.GetInstanceOf("CategoryProductList" + "\\" + "productlist_WES" + soddevenrow);

                  //  string urlDesc = "";

                    _stmpl_records.SetAttribute("FAMILY_ID", drpid["FAMILY_ID"].ToString());
                    _stmpl_records.SetAttribute("CATEGORY_ID", drpid["CATEGORY_ID"].ToString());
                    _stmpl_records.SetAttribute("PARENT_CATEGORY_ID", drpid["PARENT_CATEGORY_ID"].ToString());

                    _stmpl_records.SetAttribute("FAMILY_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath)));

                    _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath + "////UserSearch1=Family Id=" + drpid["FAMILY_ID"].ToString())));

                    _stmpl_records.SetAttribute("TBT_PRODUCT_ID", drpid["PRODUCT_ID"].ToString());
                    _stmpl_records.SetAttribute("PRODUCT_CODE", drpid["PRODUCT_CODE"].ToString());
                    _stmpl_records.SetAttribute("TBT_FAMILY_ID", drpid["FAMILY_ID"].ToString());
                    trmpstr = drpid["FAMILY_NAME"].ToString().Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
                    if (_ViewType == "GV")
                    {

                        if (trmpstr.Length > 60)
                            trmpstr = trmpstr.Substring(0, 60) + "...";
                    }

                    _stmpl_records.SetAttribute("FAMILY_NAME", trmpstr);

                    _stmpl_records.SetAttribute("FAMILY_PRODUCT_COUNT", drpid["FAMILY_PRODUCT_COUNT"].ToString());
                    _stmpl_records.SetAttribute("PRODUCT_COUNT", drpid["PRODUCT_COUNT"].ToString());

                    _stmpl_records.SetAttribute("TBT_ECOMENABLED", objHelperServices.GetIsEcomEnabled(userid));
                    if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
                        _stmpl_records.SetAttribute("TBT_WITH_LOGIN", true);
                    //  string ValueFortag = string.Empty;
                    //ValueFortag = "<div id=\"pid" + dscat.Tables[2].Rows[0]["PRODUCT_ID"].ToString() + "\" style=\"background-color:#ffffff;visibility:hidden;position:absolute\">" + AssemblePriceTable(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus) + "</div><div onMouseOver=\"javascript:ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onMouseOut=\"javascript:ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" style=\"position:relative\">" + ValueFortag + "</div>";
                    else
                        _stmpl_records.SetAttribute("TBT_WITH_LOGIN", false);

                    if (Convert.ToInt32(drpid["PRODUCT_COUNT"].ToString()) > 1)
                    {
                        _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", false);
                        _stmpl_records.SetAttribute("TBT_SUB_FAMILY", true);
                        _stmpl_records.SetAttribute("TBT_MIN_PRICE", drpid["MIN_PRICE"].ToString());
                    }
                    else
                    {
                        _stmpl_records.SetAttribute("TBT_SUB_FAMILY", false);
                        _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", true);



                        if (dsBgDisc != null)
                        {
                            if (dsBgDisc.Tables[0].Rows.Count > 0)
                            {
                                decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                                DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                                string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                                //untPrice = objHelperServices.CDEC(DsPreview.Tables[_familyID].Rows[i][j].ToString());
                                bool IsBGCatProd = objFamilyServices.IsBGCatalogProduct(Convert.ToInt32(WesCatalogId), _Buyer_Group);
                                if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && (IsBGCatProd))
                                {
                                    //ValueFortag = objFamilyServices.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth).ToString();

                                }
                            }
                        }
                        //  commented for product price table
                        //if (Convert.ToInt32(userid) > 0)
                        //{

                        //    PriceTable = objFamilyServices.AssemblePriceTable(objHelperServices.CI(drpid["PRODUCT_ID"].ToString()), pricecode, drpid["PRODUCT_CODE"].ToString(), drpid["STOCK_STATUS_DESC"].ToString(), drpid["PROD_STOCK_STATUS"].ToString(), CustomerType, Convert.ToInt32(userid), drpid["PROD_STOCK_FLAG"].ToString(), drpid["ETA"].ToString(), dsPriceTableAll);
                        //}
                        //_stmpl_records.SetAttribute("TBT_PRODUCT_PRICE_TABLE", PriceTable);


                        // _stmpl_records.SetAttribute("TBT_ECOMENABLED", (Convert.ToInt16(Session["USER_ROLE"]) < 4 ? true : false));

                        if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
                            _stmpl_records.SetAttribute("TBT_WITH_LOGIN", true);
                        //  string ValueFortag = string.Empty;
                        //ValueFortag = "<div id=\"pid" + dscat.Tables[2].Rows[0]["PRODUCT_ID"].ToString() + "\" style=\"background-color:#ffffff;visibility:hidden;position:absolute\">" + AssemblePriceTable(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus) + "</div><div onMouseOver=\"javascript:ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onMouseOut=\"javascript:ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" style=\"position:relative\">" + ValueFortag + "</div>";
                        else
                            _stmpl_records.SetAttribute("TBT_WITH_LOGIN", false);

                        DataRow[] drow = dscat.Tables[2].Select("PRODUCT_ID='" + drpid["PRODUCT_ID"].ToString() + "' AND ATTRIBUTE_TYPE=0");
                        if (drow.Length > 0) // Data Rows must return 1 row 
                        {
                            DataTable td = drow.CopyToDataTable();
                            _stmpl_records.SetAttribute("TBT_USER_PRICE", td.Rows[0]["NUMERIC_VALUE"].ToString());

                            if (Convert.ToInt32(td.Rows[0]["QTY_AVAIL"].ToString()) != -1)
                            {
                                if (Convert.ToInt32(td.Rows[0]["QTY_AVAIL"].ToString()) > 0)
                                {
                                    _stmpl_records.SetAttribute("TBT_QTY_AVAIL", td.Rows[0]["QTY_AVAIL"].ToString());
                                    _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", td.Rows[0]["MIN_ORD_QTY"].ToString());
                                }
                                else
                                {
                                    _stmpl_records.SetAttribute("TB_DISPLAY", "none");
                                }
                                //_stmpl_records.SetAttribute("TBT_PRODUCT_ID", td.Rows[0]["PRODUCT_ID"].ToString());
                            }
                        }



                    }
                    dsprodspecs = new DataSet();
                    DataRow[] drow1 = dscat.Tables[2].Select("PRODUCT_ID='" + drpid["PRODUCT_ID"].ToString() + "' AND (ATTRIBUTE_TYPE=1 OR ATTRIBUTE_TYPE=4 OR ATTRIBUTE_TYPE=3) And FAMILY_ID='" + drpid["FAMILY_ID"].ToString() + "'");
                    if (drow1.Length > 0)
                        dsprodspecs.Tables.Add(drow1.CopyToDataTable());
                    else
                        dsprodspecs = null;



                    if (dsprodspecs != null && dsprodspecs.Tables.Count >= 1 && dsprodspecs.Tables[0].Rows.Count >= 1)
                    {
                        foreach (DataRow dr in dsprodspecs.Tables[0].Rows)
                        {
                            if (dr["ATTRIBUTE_TYPE"].ToString() == "1")
                            {
                                if (dr["ATTRIBUTE_ID"].ToString() == "1")
                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), objHelperServices.StringTrim(dr["STRING_VALUE"].ToString(), 16, true));
                                else
                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString());
                            }
                            else if (dr["ATTRIBUTE_TYPE"].ToString() == "4")
                            {
                                if (dr["NUMERIC_VALUE"].ToString().Length > 0)
                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), Math.Round(Convert.ToDecimal(dr["NUMERIC_VALUE"]), 2).ToString());
                            }
                            else if (dr["ATTRIBUTE_TYPE"].ToString() == "3")
                            {
                                //System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dr["STRING_VALUE"].ToString().Replace("/", "\\"));
                                //if (Fil.Exists)
                                //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));
                                //else
                                //{
                                //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), "/images/noimage.gif");
                                //}

                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));

                            }
                            else
                            {
                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString());
                            }

                        }

                    }
                    dsprodspecs = new DataSet();
                    drow1 = dscat.Tables[2].Select("PRODUCT_ID='" + drpid["PRODUCT_ID"].ToString() + "' AND (ATTRIBUTE_TYPE=7 OR ATTRIBUTE_TYPE=9) And FAMILY_ID='" + drpid["FAMILY_ID"].ToString() + "'");
                    if (drow1.Length > 0)
                        dsprodspecs.Tables.Add(drow1.CopyToDataTable());
                    else
                        dsprodspecs = null;

                    if (dsprodspecs != null && dsprodspecs.Tables.Count >= 1 && dsprodspecs.Tables[0].Rows.Count >= 1)
                    {
                        string desc = string.Empty;
                        string descattr = string.Empty;
                        foreach (DataRow dr in dsprodspecs.Tables[0].Rows)
                        {

                            if (dr["ATTRIBUTE_TYPE"].ToString() == "9")
                            {
                                //System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dr["STRING_VALUE"].ToString());
                                //if (Fil.Exists)
                                //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));
                                //else
                                //{
                                //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), "/images/noimage.gif");
                                //}

                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));

                            }
                            else if (dr["ATTRIBUTE_TYPE"].ToString() == "7")
                            {
                                //string desc = dr["STRING_VALUE"].ToString().Replace("\r", "&nbsp;").Replace("\n", "<br/>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                               // string desc = dr["STRING_VALUE"].ToString().Replace("\r", "&nbsp;");
                                desc = dr["STRING_VALUE"].ToString().Replace("\r", "").Replace("\n\r","").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                                if (dr["ATTRIBUTE_ID"].ToString() == "13" && desc.Length > 0)
                                    desc = desc + "<br/>";
                                
                                descattr = descattr + desc;
                                if (desc.Length > 230 && _ViewType == "LV")
                                {
                                    _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
                                    desc = desc.Substring(0, 230).ToString();
                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
                                }
                                //else if (desc.Length > 30 && _ViewType == "GV")
                                //{
                                //    _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
                                //    desc = desc.Substring(0, 30).ToString();
                                //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
                                //}
                                else
                                {
                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
                                    _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), false);
                                }
                            }
                            else
                            {

                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\r", "&nbsp;").Replace("\n", "<br/>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>"));
                            }

                        }
                        if (descattr.Length > 140 && _ViewType == "GV")
                        {
                            int count = 0;
                            count = descattr.Count(c => char.IsUpper(c));
                            if (count >= 35)
                            {
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                                descattr = descattr.Substring(0, 120).ToString();
                                descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                            }
                            else if (descattr.Length > 140 && count < 35)
                            {
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                                descattr = descattr.Substring(0, 140).ToString();
                                descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                            }
                           
                        }
                        else
                            _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                    }



                    lstrows[icolstart] = new TBWDataList(_stmpl_records.ToString());

                    
                    icolstart++;
                    if (icolstart >= icol || oe == drprodcoll.Length)
                    {
                        _stg_container = new StringTemplateGroup("CategoryProductListcontainer", stemplatepath);
                        _stmpl_container = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "producttable_" + soddevenrow);
                        _stmpl_container.SetAttribute("TBWDataList", lstrows);
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
                        ictrecords++;
                        icolstart = 0;
                        lstrows = new TBWDataList[icol];

                    }
                }

                //   }

                _stg_container = new StringTemplateGroup("CategoryProductListmain", stemplatepath);





                if (Request.QueryString["ViewMode"] != null)
                {
                    PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                    isreadonly.SetValue(this.Request.QueryString, false, null);
                    this.Request.QueryString.Remove("ViewMode");
                }

                string productlistURL = Request.QueryString.ToString();
                string powersearchURListView = string.Empty;
                string powersearchURLGridView = string.Empty;
                powersearchURListView = productlistURL + "&ViewMode=LV";
                powersearchURLGridView = productlistURL + "&ViewMode=GV";


                _stmpl_container = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "main");
                _stmpl_container.SetAttribute("TBW_CATEGORY_ID", _cid);
                _stmpl_container.SetAttribute("TBW_PRODUCT_COUNT", iTotalProducts);
                _stmpl_container.SetAttribute("TBW_CATEGORY_NAME", category_nameh);

                _stmpl_container.SetAttribute("TBW_URL", powersearchURListView);
                _stmpl_container.SetAttribute("TBW_URL1", powersearchURLGridView);


 

                if (_ViewType == "LV")
                    _stmpl_container.SetAttribute("TBW_VIEWTYPE", true);
                else
                    _stmpl_container.SetAttribute("TBW_VIEWTYPE", false);


                // comment start

                //_stmpl_container.SetAttribute("SELECT_" + iRecordsPerPage, "SELECTED=\"SELECTED\" ");

                //if (iTotalPages > 1)
                //{
                //    if (iPageNo != iTotalPages)
                //    {
                //        _stmpl_container.SetAttribute("TBW_TO_PAGE", true);
                //    }
                //    else if (iPageNo == iTotalPages)
                //    {
                //        _stmpl_container.SetAttribute("TBW_TOTAL_PAGE", true);
                //    }
                //}
                //else
                //{
                //    _stmpl_container.SetAttribute("TBW_TO_PAGE", false);
                //}


                ////if (WesNewsCategoryId == _ParentCatID) // WES NEW ONLy
                ////    SetEbookAndPDFLink(_stmpl_container);
                ////else
                ////    _stmpl_container.SetAttribute("TBT_DISPLAY_LINK", "none");



                //if (iPageNo < iTotalPages)
                //{
                //    if (iPageNo > 1)
                //    {
                //        iPrevPgNo = iPageNo - 1;
                //    }
                //    else
                //    {
                //        iPrevPgNo = 1;
                //    }
                //    iNextPgNo = iPageNo + 1;
                //}
              
                //else
                //{
                //    iNextPgNo = 1;
                //    iPrevPgNo = 1;
                //    iPageNo = iTotalPages;
                //}
              
                //try
                //{

                //    _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                //    if (iPageNo > 2 && (iTotalPages >= (iPageNo + 2)))
                //    {
                       

                //        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iPageNo - 2);
                //        SetQueryString(_stmpl_pages);


                //        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());

                //        _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");

                //        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iPageNo - 1);
                //        SetQueryString(_stmpl_pages);
                //        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());

                //        _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");

                //        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iPageNo);
                //        SetQueryString(_stmpl_pages);
                //        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));

                //        _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");

                //        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iPageNo + 1);
                //        SetQueryString(_stmpl_pages);
                //        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());

                //        _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                        
                //        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iPageNo + 2);
                //        SetQueryString(_stmpl_pages);
                //        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());//.Replace("-</td>", "</td>"));
                //    }
                //    else if (iPageNo > 0 && iPageNo < 4 && iPageNo < iTotalPages)
                //    {

                //        _stmpl_pages.SetAttribute("TBW_PAGE_NO", 1);
                //        SetQueryString(_stmpl_pages);

                //        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //        if (iPageNo == 1)
                //        {
                //            if (1 == iTotalPages)
                               
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
                //            else
                //            {
                                
                //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));

                //            }
                //        }
                //        else
                //        {
                //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
                //        }

                //        if (2 <= iTotalPages)
                //        {
                //            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");

                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", 2);
                //            SetQueryString(_stmpl_pages);
                           
                //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //            if (iPageNo == 2)
                //            {
                //                if (2 == iTotalPages)
                //                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
                //                else
                //                {
                //                    _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));
                //                   // _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //                }
                //            }
                //            else
                //            {
                               

                //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
                //            }
                //        }

                //        if (3 <= iTotalPages)
                //        {
                //            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");

                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", 3);
                //            SetQueryString(_stmpl_pages);
                            
                //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //            if (iPageNo == 3)
                //            {
                //                if (3 == iTotalPages)
                //                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
                //                else
                //                {
                //                    _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));
                //                }
                //            }
                //            else
                //            {
                               
                //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
                //            }
                //        }
                //        if (4 <= iTotalPages)
                //        {
                //            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");

                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", 4);
                //            SetQueryString(_stmpl_pages);
                            
                //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //            if (iPageNo == 4)
                //            {
                //                if (4 == iTotalPages)
                //                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
                //                else
                //                {
                //                    _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));

                //                }
                //            }
                //            else
                //            {
                               
                //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
                //            }

                //        }
                //        if (5 <= iTotalPages)
                //        {
                //            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                           
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", 5);
                //            SetQueryString(_stmpl_pages);
                           
                //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //            if (iPageNo == 5)
                //            {
                                
                //                if (4 == iTotalPages)
                //                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("\r", "").Replace("\n", "").Replace("-</td>", "</td>"));
                //                else
                //                {
                //                    _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("\r", "").Replace("\n", "").Replace("-</td>", "</td>"));
                //                }
                //            }
                //            else
                //            {
                //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());//.Replace("-</td>", "</td>"));
                //            }

                //        }
                //    }
                //    else
                //        if (iPageNo == iTotalPages && 1 <= iTotalPages - 4 && iPageNo < iTotalPages)
                //        {
                           
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 4);
                //            SetQueryString(_stmpl_pages);
                           
                //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


                //            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                           
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 3);
                //            SetQueryString(_stmpl_pages);
                           
                //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


                //            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                           
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 2);
                //            SetQueryString(_stmpl_pages);
                           
                //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


                //            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                           
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 1);
                //            SetQueryString(_stmpl_pages);
                //            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


                //            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                           
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages);
                //            SetQueryString(_stmpl_pages);
                           
                //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //            if (iPageNo == iTotalPages)
                //            {
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
                //            }
                //            else
                //            {
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());//.Replace("-</td>", "</td>"));
                //            }

                //        }
                //        else if (iPageNo == iTotalPages && iPageNo < iTotalPages)
                //        {
                           
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 3);
                //            SetQueryString(_stmpl_pages);
                           
                //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


                //            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");

                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 2);
                //            SetQueryString(_stmpl_pages);

                //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


                //            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                            
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 1);
                //            SetQueryString(_stmpl_pages);
                           
                //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


                //            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                           
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages);
                //            SetQueryString(_stmpl_pages);
                            
                //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //            if (iPageNo == iTotalPages)
                //            {
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
                //            }
                //            else
                //            {
                //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());//.Replace("-</td>", "</td>"));
                //            }

                //        }
                //        else if ((1 <= iTotalPages - 4 && iPageNo < iTotalPages) || (1 <= iTotalPages - 4 && iPageNo == iTotalPages))
                //        {
                //            if (iTotalPages - 4 > 0)
                //            {
                               
                //                _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 4);
                //                SetQueryString(_stmpl_pages);

                //                _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
                //            }


                //            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                            
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 3);
                //            SetQueryString(_stmpl_pages);

                //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());




                //            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                           
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 2);

                //            SetQueryString(_stmpl_pages);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());



                //            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                           
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 1);
                //            SetQueryString(_stmpl_pages);
                           
                //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                           
                //            if (iPageNo != iTotalPages)
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now").Replace("\n", "").Replace("\r", ""));//.Replace("-</td>", "</td>"));
                //            else
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());

                //            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                            

                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages);
                //            SetQueryString(_stmpl_pages);
                //            if (iPageNo == iTotalPages)
                //            {
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
                //            }
                //            else
                //            {
                               
                //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());//.Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
                //            }


                //        }


                //        else if (iPageNo == iTotalPages)
                //        {

                //            if (iTotalPages - 3 > 0)
                //            {
                                
                //                _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 3);
                //                SetQueryString(_stmpl_pages);
                                
                //                _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
                //            }
                //            if (iTotalPages - 2 > 0)
                //            {
                //                _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                                
                //                _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 2);
                //                SetQueryString(_stmpl_pages);
                               
                //                _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
                //            }
                //            if (iTotalPages - 1 > 0)
                //            {
                //                _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                                
                //                _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 1);
                //                SetQueryString(_stmpl_pages);
                                
                //                _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
                //            }
                //            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpageno");
                            
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages);
                //            SetQueryString(_stmpl_pages);
                           
                //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //            if (iPageNo == iTotalPages)
                //            {
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
                //            }
                //            else
                //            {
                //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());//.Replace("-</td>", "</td>"));
                //            }

                //        }
                //    if (iTotalPages > 1 && iPageNo != iTotalPages && iPageNo < iTotalPages)
                //    {
                //         _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpagenoNext");
                //        _stmpl_pages.SetAttribute("TBW_PAGE_NO", (iPageNo + 1));
                //        SetQueryString(_stmpl_pages);
                       
                //        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);

                //        _stmpl_container.SetAttribute("TBW_NEXT_PAGE", _stmpl_pages.ToString());




                //    }
                //    else
                //    {

                //        if (iPageNo != iTotalPages)
                //        {
                           
                //            _stmpl_pages = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "productpagenoNext");
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", (iPageNo));
                //            SetQueryString(_stmpl_pages);
                //            _stmpl_container.SetAttribute("TBW_NEXT_PAGE", _stmpl_pages.ToString());
                //        }
                //        else
                //        {
                //            _stmpl_container.SetAttribute("TBW_NEXT_PAGE", "");
                //        }
                //    }
                //}
                //catch (Exception ex)
                //{

                //}
               
                //if (iRecordsPerPage == 32767) //View All
                //{
                //    iRecordsPerPage = iTotalProducts;
                //}
                //_stmpl_container.SetAttribute("TBW_START_PAGE_NO", (iPageNo * iRecordsPerPage) - (iRecordsPerPage - 1));
                //if (((iPageNo * iRecordsPerPage) > iTotalProducts))
                //    _stmpl_container.SetAttribute("TBW_END_PAGE_NO", iTotalProducts);
                //else
                //    _stmpl_container.SetAttribute("TBW_END_PAGE_NO", (iPageNo * iRecordsPerPage));
                // comment end

                if (iTotalPages > 1 && iPageNo != iTotalPages && iPageNo < iTotalPages)
                {
                    //_stmpl_container.SetAttribute("TBW_TOTAL_PAGES", iTotalPages);
                    //_stmpl_container.SetAttribute("TBW_PREVIOUS_PG_NO", iPrevPgNo);
                    //_stmpl_container.SetAttribute("TBW_CURRENT_PAGE_NO", iPageNo);
                    //_stmpl_container.SetAttribute("TBW_NEXT_PG_NO", iNextPgNo);
                    _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                    sHTML = _stmpl_container.ToString().Replace("<option value=\"" + iRecordsPerPage.ToString() + "\">", "<option value=\"" + iRecordsPerPage.ToString() + "\" selected =selected>");
                }
                else
                {
                    iPageNo = iTotalPages;
                    //_stmpl_container.SetAttribute("TBW_TOTAL_PAGES", iTotalPages);
                    //_stmpl_container.SetAttribute("TBW_PREVIOUS_PG_NO", iPrevPgNo);
                    //_stmpl_container.SetAttribute("TBW_CURRENT_PAGE_NO", iPageNo);
                    //_stmpl_container.SetAttribute("TBW_NEXT_PG_NO", iNextPgNo);
                    _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                    sHTML = _stmpl_container.ToString().Replace("<option value=\"" + iRecordsPerPage.ToString() + "\">", "<option value=\"" + iRecordsPerPage.ToString() + "\" selected =selected>");
                }
               
                if (sHTML.ToString().Contains("data-original=\"prodimages\""))
                {
                    sHTML = sHTML.Replace("data-original=\"prodimages\"", "data-original=\"images/noimage.gif\"");
                    sHTML = sHTML.Replace("href=\"javascript:Zoom('prodimages')\"", "href=\"#\"");
                }
                if (sHTML.ToString().Contains("data-original=\"\""))
                {
                    sHTML = sHTML.ToString().Replace("data-original=\"\"", "data-original=\"images/noimage.gif\"");
                    sHTML = sHTML.Replace("href=\"javascript:Zoom('')\"", "href=\"#\"");
                }
                if (dscat.Tables[1].Rows[0].ItemArray[0].ToString() == "0")
                    sHTML = "";
                if (dscat.Tables[0].Rows[0].ItemArray[0].ToString() == "1")
                {
                    sHTML = sHTML.Replace("<a href=\"powersearch.aspx?pgno=1\">Previous</a>", "");
                    sHTML = sHTML.Replace("<a href=\"powersearch.aspx?pgno=1\">Next</a>", "");
                }
                DataSet DSnaprod = new DataSet();

            }
            string eapath = _EAPath.Replace("'", "###");


            htmleapath.Value = eapath.ToString();

            htmltotalpages.Value = iTotalPages.ToString();
            htmlviewmode.Value = _ViewType;
            //if (Session["iRecordsPerPage"] != null)
            //{
            htmlirecords.Value = iRecordsPerPage.ToString();
           
        }
        catch (Exception ex)
        {
            sHTML = ex.Message;
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
        finally
        {
           
        }
       

        //return objHelperServices.StripWhitespace( sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>"));
        return sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
    }


    public string CategoryProductList_RenderHTMLJson(string package, string SkinRootPath)
    {
        //if (Session["PS_SEARCH_RESULTS"] == null)
        //{
        //    return "";
        //}
        
        FamilyServices objFamilyServices = new FamilyServices();
        StringTemplateGroup _stg_container = null;
        StringTemplateGroup _stg_records = null;
        StringTemplate _stmpl_container = null;
        StringTemplate _stmpl_records = null;
     //   StringTemplate _stmpl_pages = null;
        DataSet dsprod = new DataSet();
        DataSet dsprodspecs = new DataSet();
        DataSet dscat = new DataSet();
        string strFile = HttpContext.Current.Server.MapPath("ProdImages");

        int oe = 0;
        string category_nameh = string.Empty;
        string sHTML = string.Empty;
        string _pcr = string.Empty;
        string _ViewType = string.Empty;
        bool BindToST = true;
        //string catID = "";

        string tmpstrPid = string.Empty;
        string tmpdivcount = string.Empty;

        //if (Convert.ToInt32(Session["PS_SEARCH_RESULTS"].ToString()) > 0)
        //{
        try
        {
            //stopwatch.Start();
            if (Request.QueryString["tsm"] != null && Request.QueryString["tsm"].ToString() != "")
                _tsm = Request.QueryString["tsm"];

            if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "")
                _tsb = Request.QueryString["tsb"];

            if (Request.QueryString["type"] != null)
                _type = Request.QueryString["type"];

            if (Request.QueryString["value"] != null)
                _value = Request.QueryString["value"];

            if (Request.QueryString["bname"] != null)
                _bname = Request.QueryString["bname"];
            if (Request.QueryString["searchstr"] != null)
                _searchstr = Request.QueryString["searchstr"];
            if (Request.QueryString["srctext"] != null)
                _searchstr = Request.QueryString["srctext"];

            if (Request.QueryString["cid"] != null)
                _cid = Request.QueryString["cid"];

            if (Request.QueryString["pcr"] != null)
                _pcr = Request.QueryString["pcr"];


            if (Request.QueryString["ViewMode"] != null)
            {
                _ViewType = Request.QueryString["ViewMode"];
                Session["PL_VIEW_MODE"] = _ViewType;
            }
            else if (Session["PL_VIEW_MODE"] != null && Session["PL_VIEW_MODE"].ToString() != "")
                _ViewType = Session["PL_VIEW_MODE"].ToString();
            else
                _ViewType = "GV";

            //if (Request.QueryString["path"] != null)
            //    _EAPath = HttpUtility.UrlDecode(objSecurity.StringDeCrypt(Request.QueryString["path"].ToString()));

            if (HttpContext.Current.Session["EA"] != null)
                _EAPath = HttpContext.Current.Session["EA"].ToString();


            stemplatepath = Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());



            dscat = (DataSet)HttpContext.Current.Session["FamilyProduct"];



            if (dscat != null)
            {

                if (Request.QueryString["pcr"] != null)
                    _pcr = Request.QueryString["pcr"];
                if (Request.QueryString["type"] == null || Request.QueryString["type"] == string.Empty)
                {
                    //GetDataSet("SELECT CATEGORY_NAME  FROM	TB_CATEGORY WHERE	CATEGORY_ID ='" + catID + "'").Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
                    string tempstr = (string)objHelperDB.GetGenericDataDB(_cid, "GET_CATEGORY_NAME", HelperDB.ReturnType.RTString);
                    if (tempstr != null && tempstr != "")
                        category_nameh = tempstr;
                }
                else
                    category_nameh = Request.QueryString["value"].ToString();

                DataRow drpagect = dscat.Tables[0].Rows[0];

                if (drpagect.Table.Rows.Count > 0)
                {
                    iTotalPages = Convert.ToInt32(drpagect["TOTAL_PAGES"]);
                }

                if (iPageNo > iTotalPages)
                {
                    iPageNo = iTotalPages;
                    //ps.PAGE_NO = iPageNo;
                }
                Session["iTotalPages"] = iTotalPages;
                DataRow drproductsct = dscat.Tables[1].Rows[0];
                iTotalProducts = Convert.ToInt32(drproductsct["TOTAL_PRODUCTS"]);
                if (_cid != "")
                    _ParentCatID = GetParentCatID(_cid);

                if ((iTotalPages <= 0 || iTotalProducts <= 0) && WesNewsCategoryId == _ParentCatID)
                {


                    _stg_container = new StringTemplateGroup("CategoryProductListmain", stemplatepath);
                    _stmpl_container = _stg_container.GetInstanceOf("CategoryProductList\\advmain");

                    // SetEbookAndPDFLink(_stmpl_container);
                    DataTable tmpDataTable = (DataTable)objHelperDB.GetGenericDataDB(WesCatalogId, "'" + _cid + "'", "GET_CATEGORY", HelperDB.ReturnType.RTTable);
                    if (tmpDataTable != null && tmpDataTable.Rows.Count > 0)
                    {
                        //modified by:indu
                        string ebookpath = tmpDataTable.Rows[0]["CUSTOM_TEXT_FIELD3"].ToString();

                        if (ebookpath.Contains("www."))
                        {

                            ebookpath = ebookpath.ToLower().Replace("attachments", "").Replace("\\", "").Replace("http://", "").Replace("https://", "");
                            ebookpath = "http://" + ebookpath;
                        }
                        _stmpl_container.SetAttribute("TBT_ADV_URL_LINK", ebookpath);
                    }
                    _stmpl_container.SetAttribute("TBW_CATEGORY_NAME", category_nameh);
                    return _stmpl_container.ToString();
                }



                TBWDataList[] lstrecords = new TBWDataList[0];
                TBWDataList[] lstrows = new TBWDataList[0];
                _stg_records = new StringTemplateGroup("CategoryProductList", stemplatepath);


                int ictrecords = 0;
                int icolstart = 0;
                string trmpstr = string.Empty;
                int icol = 1;
                lstrows = new TBWDataList[icol];

                if (_ViewType == "GV")
                {
                    icol = 4;
                    lstrows = new TBWDataList[icol];
                }
                else
                {
                    icol = 1;
                    lstrows = new TBWDataList[icol];
                }

                //if (dscat.Tables[0].Rows.Count < icol)
                //{
                //    icol = dscat.Tables[0].Rows.Count;
                //}
                string soddevenrow = "odd";

               // DataRow[] drprodcoll = dscat.Tables[2].Select("ATTRIBUTE_ID = 1");


                string userid = string.Empty;
                string PriceTable = string.Empty;
                int pricecode = 0;
                string tmpProds = string.Empty;
                DataSet dsBgDisc = new DataSet();
                DataSet dsPriceTableAll = new DataSet();
                if (Session["USER_ID"] != null)
                    userid = Session["USER_ID"].ToString();
                if (userid == string.Empty)
                    userid = "0";

                //string _Buyer_Group = objFamilyServices.GetBuyerGroup(Convert.ToInt32(userid));
                //if (Convert.ToInt32(userid) > 0)
                //{

                //    dsBgDisc = objFamilyServices.GetBuyerGroupBasedDiscountDetails(_Buyer_Group);
                //}
                //else
                //{
                //    dsBgDisc = objFamilyServices.GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
                //}
                pricecode = objHelperDB.GetPriceCode(userid);

                if (Session["RECORDS_PER_PAGE_CATEGORY_LIST"] != null)
                    iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_CATEGORY_LIST"].ToString());

                tmpProds = "";
                if (Convert.ToInt32(userid) > 0)
                {
                    foreach (DataRow drpid in dscat.Tables["FamilyPro"].Rows)
                    {
                        tmpProds = tmpProds + drpid["PRODUCT_ID"] + ",";
                    }
                    if (tmpProds != string.Empty)
                    {
                        tmpProds = tmpProds.Substring(0, tmpProds.Length - 1);
                        dsPriceTableAll = objHelperDB.GetProductPriceTableAll(tmpProds, Convert.ToInt32(userid));
                    }
                }

                bool IsEcomEnabled = objHelperServices.GetIsEcomEnabled(userid);
                lstrecords = new TBWDataList[dscat.Tables["FamilyPro"].Rows.Count  + 1];
                string CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(userid));

                string cellGV = string.Empty;
                string cellLV = string.Empty;
                cellGV = "CategoryProductList\\productlist_GridView";
                cellLV = "CategoryProductList\\productlist_WES";

                int icnt = 0;
                foreach (DataRow drpid in dscat.Tables["FamilyPro"].Rows)
                {
                    oe++;
                    icnt++;

                    if ((oe % 2) == 0)
                    {
                        soddevenrow = "even";
                    }
                    else
                    {
                        soddevenrow = "odd";
                    }

                    if (_ViewType == "GV")
                        _stmpl_records = _stg_records.GetInstanceOf(cellGV);
                    else
                        _stmpl_records = _stg_records.GetInstanceOf(cellLV + soddevenrow);

                    //string urlDesc = "";
                    if (tmpstrPid == "")
                    {
                        tmpstrPid = drpid["PRODUCT_ID"].ToString();
                        tmpdivcount = drpid["PRODUCT_ID"] + "_" + icnt;
                    }
                    else
                    {
                        tmpstrPid = tmpstrPid + "," + drpid["PRODUCT_ID"].ToString();
                        tmpdivcount = tmpdivcount + "," + drpid["PRODUCT_ID"] + "_" + icnt;
                    }
                    _stmpl_records.SetAttribute("FAMILY_ID", drpid["FAMILY_ID"]);
                    _stmpl_records.SetAttribute("CATEGORY_ID", drpid["CATEGORY_ID"]);
                    _stmpl_records.SetAttribute("PARENT_CATEGORY_ID", drpid["PARENT_CATEGORY_ID"]);

                    _stmpl_records.SetAttribute("FAMILY_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath)));

                    _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath + "////UserSearch1=Family Id=" + drpid["FAMILY_ID"])));

                    _stmpl_records.SetAttribute("TBT_PRODUCT_ID", drpid["PRODUCT_ID"]);
                    _stmpl_records.SetAttribute("PRODUCT_CODE", drpid["PRODUCT_CODE"].ToString());
                    _stmpl_records.SetAttribute("TBT_FAMILY_ID", drpid["FAMILY_ID"]);
                    _stmpl_records.SetAttribute("divcount", drpid["PRODUCT_ID"] + "_" + icnt);

                    trmpstr = drpid["FAMILY_NAME"].ToString().Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
                    if (_ViewType == "GV")
                    {

                        if (trmpstr.Length > 60)
                            trmpstr = trmpstr.Substring(0, 60) + "...";
                    }

                    _stmpl_records.SetAttribute("FAMILY_NAME", trmpstr);

                    _stmpl_records.SetAttribute("FAMILY_PRODUCT_COUNT", drpid["FAMILY_PRODUCT_COUNT"]);
                    _stmpl_records.SetAttribute("PRODUCT_COUNT", drpid["PRODUCT_COUNT"]);

                    _stmpl_records.SetAttribute("TBT_ECOMENABLED", IsEcomEnabled);
                    if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
                        _stmpl_records.SetAttribute("TBT_WITH_LOGIN", true);
                    //  string ValueFortag = string.Empty;
                    //ValueFortag = "<div id=\"pid" + dscat.Tables[2].Rows[0]["PRODUCT_ID"].ToString() + "\" style=\"background-color:#ffffff;visibility:hidden;position:absolute\">" + AssemblePriceTable(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus) + "</div><div onMouseOver=\"javascript:ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onMouseOut=\"javascript:ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" style=\"position:relative\">" + ValueFortag + "</div>";
                    else
                        _stmpl_records.SetAttribute("TBT_WITH_LOGIN", false);

<<<<<<< .mine
                    //if (HttpContext.Current.Session["EBAY_BLOCK"] != null && HttpContext.Current.Session["EBAY_BLOCK"].ToString() == "True" && drpid["EBAY_BLOCK"] != null && drpid["EBAY_BLOCK"].ToString() == "True")
                    //{
                    //    _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", true);
                    //    _stmpl_records.SetAttribute("TBT_SUB_FAMILY", false);
                    //    _stmpl_records.SetAttribute("TBT_HIDE_BUY", true);
                    //    _stmpl_records.SetAttribute("PRODUCT_STATUS", "UNAVAILABLE");


                    //    if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
                    //        _stmpl_records.SetAttribute("TBT_WITH_LOGIN", true);
                    //    else
                    //        _stmpl_records.SetAttribute("TBT_WITH_LOGIN", false);


                    //    _stmpl_records.SetAttribute("TBT_USER_PRICE", drpid["PRODUCT_PRICE"]);

                    //    if (Convert.ToInt32(drpid["QTY_AVAIL"].ToString()) != -1)
                    //    {
                    //        if (Convert.ToInt32(drpid["QTY_AVAIL"].ToString()) > 0)
                    //        {
                    //            _stmpl_records.SetAttribute("TBT_QTY_AVAIL", drpid["QTY_AVAIL"]);
                    //            _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", drpid["MIN_ORD_QTY"]);
                    //        }
                    //        else
                    //        {
                    //            _stmpl_records.SetAttribute("TB_DISPLAY", "none");
                    //        }

                    //    }
                    //}
                     if (Convert.ToInt32(drpid["PRODUCT_COUNT"].ToString()) > 1)
||||||| .r594
                    if (Convert.ToInt32(drpid["PRODUCT_COUNT"].ToString()) > 1)
=======
                    if (HttpContext.Current.Session["EBAY_BLOCK"] != null && HttpContext.Current.Session["EBAY_BLOCK"].ToString() == "True" && drpid["EBAY_BLOCK"] != null && drpid["EBAY_BLOCK"].ToString() == "True")
>>>>>>> .r640
                    {
<<<<<<< .mine
                        BindToST = objHelperDB.CheckFamily_Discontinued(drpid["FAMILY_ID"].ToString());
||||||| .r594
=======
                        _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", true);
                        _stmpl_records.SetAttribute("TBT_SUB_FAMILY", false);
                        _stmpl_records.SetAttribute("TBT_HIDE_BUY", true);
                        _stmpl_records.SetAttribute("PRODUCT_STATUS", "UNAVAILABLE");

                        
                        if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
                            _stmpl_records.SetAttribute("TBT_WITH_LOGIN", true);
                        else
                            _stmpl_records.SetAttribute("TBT_WITH_LOGIN", false);


                        _stmpl_records.SetAttribute("TBT_USER_PRICE", drpid["PRODUCT_PRICE"]);

                        if (Convert.ToInt32(drpid["QTY_AVAIL"].ToString()) != -1)
                        {
                            if (Convert.ToInt32(drpid["QTY_AVAIL"].ToString()) > 0)
                            {
                                _stmpl_records.SetAttribute("TBT_QTY_AVAIL", drpid["QTY_AVAIL"]);
                                _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", drpid["MIN_ORD_QTY"]);
                            }
                            else
                            {
                                _stmpl_records.SetAttribute("TB_DISPLAY", "none");
                            }

                        }
                    }
                    else if (Convert.ToInt32(drpid["PRODUCT_COUNT"].ToString()) > 1)
                    {
>>>>>>> .r640
                        _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", false);
                        _stmpl_records.SetAttribute("TBT_SUB_FAMILY", true);
                        _stmpl_records.SetAttribute("TBT_MIN_PRICE", drpid["MIN_PRICE"]);
                    }
                    else
                    {
                        _stmpl_records.SetAttribute("TBT_SUB_FAMILY", false);
                        _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", true);

                        string stk_sta_desc = "";
                        stk_sta_desc = drpid["STOCK_STATUS_DESC"].ToString().Trim();
                        //   objErrorHandler.CreateLog(stk_sta_desc);
                        BindToST = true;
                        if ((stk_sta_desc == "DISCONTINUED NO LONGER AVAILABLE" || stk_sta_desc == "TEMPORARY UNAVAILABLE NO ETA" || stk_sta_desc == "TEMPORARY_UNAVAILABLE NO ETA"))
                        {
                            if (stk_sta_desc == "DISCONTINUED NO LONGER AVAILABLE")
                            {
                                _stmpl_records.SetAttribute("PRODUCT_STATUS", "Product No Longer Available.</br> Please contact Us");
                                BindToST = false;
                            }
                            else if (stk_sta_desc == "TEMPORARY UNAVAILABLE NO ETA" || stk_sta_desc == "TEMPORARY_UNAVAILABLE NO ETA")
                            {
                                _stmpl_records.SetAttribute("PRODUCT_STATUS", "Product Temporarily Unavailable.<br/>Please Contact Us for more details");
                            }
                            if (drpid["PROD_SUBSTITUTE"].ToString().Trim() != "" &&  drpid["PROD_STOCK_FLAG"].ToString().Trim() == "-2")
                            {
                              //  DataTable rtntbl = objProductServices.GetSubstituteProductDetails(drpid["PROD_SUBSTITUTE"].ToString().Trim(), Convert.ToInt32(userid));
                                DataTable rtntbl = objProductServices.GetSubstituteProductDetails(drpid["PRODUCT_CODE"].ToString(), Convert.ToInt32(userid));
                                if (rtntbl != null && rtntbl.Rows.Count > 0)
                                {

                                    bool samecodesubproduct = (bool)rtntbl.Rows[0]["samecodesubproduct"];

                                    bool samecodenotFound = (bool)rtntbl.Rows[0]["samecodenotFound"];

                                    if (samecodenotFound == false && rtntbl.Rows[0]["ea_path"].ToString() != "")
                                    {


                                        if (stk_sta_desc.ToUpper() == "DISCONTINUED NO LONGER AVAILABLE")
                                        {

                                            DataSet Sqltbs = objHelperDB.GetProductPriceEA("", rtntbl.Rows[0]["SubstuyutePid"].ToString(), userid);
                                            if (Sqltbs != null)
                                            {

                                                string stockstaus = Sqltbs.Tables[0].Rows[0]["PROD_STK_STATUS_DSC"].ToString().Trim().ToUpper();
                                              //  objErrorHandler.CreateLog("Sqltbs" + stockstaus);
                                                if ((stockstaus == "DISCONTINUED NO LONGER AVAILABLE"))
                                                {

                                                    BindToST = false;
                                                }
                                                else
                                                {
                                                    BindToST = true;
                                                }
                                            }
                                        }
                                        _stmpl_records.SetAttribute("TBT_HIDE_BUY", false);
                                        _stmpl_records.SetAttribute("TBT_SUB_PRODUCT", true);
                                        _stmpl_records.SetAttribute("TBT_REP_NIL_CODE", rtntbl.Rows[0]["wag_product_code"].ToString());
                                        string strurl = "ProductDetails.aspx?Pid=" + rtntbl.Rows[0]["SubstuyutePid"].ToString() + "&amp;fid=" + rtntbl.Rows[0]["Pfid"].ToString() + "&amp;Cid=" + rtntbl.Rows[0]["CatId"].ToString() + "&amp;path=" + rtntbl.Rows[0]["ea_path"].ToString();
                                        _stmpl_records.SetAttribute("TBT_REP_EA_PATH", strurl);
                                       
                                    }
                                    else
                                    {
                                        _stmpl_records.SetAttribute("TBT_HIDE_BUY", true);

                                    }
                                }
                                else
                                {
                                    _stmpl_records.SetAttribute("TBT_HIDE_BUY", true);
                                }
                            }
                            else
                            {
                                _stmpl_records.SetAttribute("TBT_HIDE_BUY", true);

                            }

                        }
                        else
                        {
                            _stmpl_records.SetAttribute("TBT_HIDE_BUY", false);
                            if ((stk_sta_desc.ToUpper().Contains("OUT_OF_STOCK") == true || stk_sta_desc.ToUpper().Contains("SPECIAL_ORDER") == true ) && drpid["PROD_SUBSTITUTE"].ToString().Trim() != "" && drpid["PROD_STOCK_FLAG"].ToString().Trim() == "-2")
                            {
                                //DataTable rtntbl = objProductServices.GetSubstituteProductDetails(drpid["PROD_SUBSTITUTE"].ToString().Trim(), Convert.ToInt32(userid));
                                DataTable rtntbl = objProductServices.GetSubstituteProductDetails(drpid["PRODUCT_CODE"].ToString(), Convert.ToInt32(userid));
                                if (rtntbl != null && rtntbl.Rows.Count > 0)
                                {

                                    bool samecodesubproduct = (bool)rtntbl.Rows[0]["samecodesubproduct"];

                                    bool samecodenotFound = (bool)rtntbl.Rows[0]["samecodenotFound"];
                                   // objErrorHandler.CreateLog("OUT_OF_STOCK" + samecodenotFound + "--" + rtntbl.Rows[0]["ea_path"].ToString());
                                    if (samecodenotFound == false && rtntbl.Rows[0]["ea_path"].ToString() != "")
                                    {

                                        _stmpl_records.SetAttribute("TBT_SUB_PRODUCT", true);
                                        _stmpl_records.SetAttribute("TBT_REP_NIL_CODE", rtntbl.Rows[0]["wag_product_code"].ToString());
                                        string strurl = "ProductDetails.aspx?Pid=" + rtntbl.Rows[0]["SubstuyutePid"].ToString() + "&amp;fid=" + rtntbl.Rows[0]["Pfid"].ToString() + "&amp;Cid=" + rtntbl.Rows[0]["CatId"].ToString() + "&amp;path=" + rtntbl.Rows[0]["ea_path"].ToString();
                                        _stmpl_records.SetAttribute("TBT_REP_EA_PATH", strurl);
                                    }
                                }

                            }
                            else if (HttpContext.Current.Session["EBAY_BLOCK"] != null && HttpContext.Current.Session["EBAY_BLOCK"].ToString() == "True" && drpid["EBAY_BLOCK"] != null && drpid["EBAY_BLOCK"].ToString() == "True")
                            {
                                _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", true);

                                _stmpl_records.SetAttribute("TBT_HIDE_BUY", true);
                                _stmpl_records.SetAttribute("PRODUCT_STATUS", "UNAVAILABLE");

                            } 
                        }

                        //if (dsBgDisc != null)
                        //{
                        //    if (dsBgDisc.Tables[0].Rows.Count > 0)
                        //    {
                        //        decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                        //        DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                        //        string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                        //        //untPrice = objHelperServices.CDEC(DsPreview.Tables[_familyID].Rows[i][j].ToString());
                        //        bool IsBGCatProd = objFamilyServices.IsBGCatalogProduct(Convert.ToInt32(WesCatalogId), _Buyer_Group);
                        //        if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
                        //        {
                        //            //ValueFortag = objFamilyServices.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth).ToString();

                        //        }
                        //    }
                        //}
                        //  commented for product price table
                        //if (Convert.ToInt32(userid) > 0)
                        //{

                        //    PriceTable = objFamilyServices.AssemblePriceTable(objHelperServices.CI(drpid["PRODUCT_ID"].ToString()), pricecode, drpid["PRODUCT_CODE"].ToString(), drpid["STOCK_STATUS_DESC"].ToString(), drpid["PROD_STOCK_STATUS"].ToString(), CustomerType, Convert.ToInt32(userid), drpid["PROD_STOCK_FLAG"].ToString(), drpid["ETA"].ToString(), dsPriceTableAll);
                        //}
                        //_stmpl_records.SetAttribute("TBT_PRODUCT_PRICE_TABLE", PriceTable);


                        // _stmpl_records.SetAttribute("TBT_ECOMENABLED", (Convert.ToInt16(Session["USER_ROLE"]) < 4 ? true : false));

                        if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
                            _stmpl_records.SetAttribute("TBT_WITH_LOGIN", true);
                        //  string ValueFortag = string.Empty;
                        //ValueFortag = "<div id=\"pid" + dscat.Tables[2].Rows[0]["PRODUCT_ID"].ToString() + "\" style=\"background-color:#ffffff;visibility:hidden;position:absolute\">" + AssemblePriceTable(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus) + "</div><div onMouseOver=\"javascript:ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onMouseOut=\"javascript:ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" style=\"position:relative\">" + ValueFortag + "</div>";
                        else
                            _stmpl_records.SetAttribute("TBT_WITH_LOGIN", false);



                        _stmpl_records.SetAttribute("TBT_USER_PRICE", drpid["PRODUCT_PRICE"]);

                        if (Convert.ToInt32(drpid["QTY_AVAIL"].ToString()) != -1)
                        {
                            if (Convert.ToInt32(drpid["QTY_AVAIL"].ToString()) > 0)
                            {
                                _stmpl_records.SetAttribute("TBT_QTY_AVAIL", drpid["QTY_AVAIL"]);
                                _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", drpid["MIN_ORD_QTY"]);
                            }
                            else
                            {
                                _stmpl_records.SetAttribute("TB_DISPLAY", "none");
                            }

                        }

                        //DataRow[] drow = dscat.Tables[2].Select("PRODUCT_ID='" + drpid["PRODUCT_ID"].ToString() + "' AND ATTRIBUTE_TYPE=0");
                        //if (drow.Length > 0) // Data Rows must return 1 row 
                        //{
                        //    DataTable td = drow.CopyToDataTable();
                        //    _stmpl_records.SetAttribute("TBT_USER_PRICE", td.Rows[0]["NUMERIC_VALUE"].ToString());

                        //    if (Convert.ToInt32(td.Rows[0]["QTY_AVAIL"].ToString()) != -1)
                        //    {
                        //        if (Convert.ToInt32(td.Rows[0]["QTY_AVAIL"].ToString()) > 0)
                        //        {
                        //            _stmpl_records.SetAttribute("TBT_QTY_AVAIL", td.Rows[0]["QTY_AVAIL"].ToString());
                        //            _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", td.Rows[0]["MIN_ORD_QTY"].ToString());
                        //        }
                        //        else
                        //        {
                        //            _stmpl_records.SetAttribute("TB_DISPLAY", "none");
                        //        }
                        //        //_stmpl_records.SetAttribute("TBT_PRODUCT_ID", td.Rows[0]["PRODUCT_ID"].ToString());
                        //    }
                        //}



                    }

                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_1", drpid["PRODUCT_CODE"]);

                    //System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + drpid["Prod_Thumbnail"].ToString().Replace("/", "\\"));
                    //if (Fil.Exists)
                    //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", drpid["Prod_Thumbnail"]);
                    //else
                    //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", "/images/noimage.gif");

                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", drpid["Prod_Thumbnail"]);


                    string desc = string.Empty;
                    string descattr = string.Empty;
                    string prod_desc_alt = string.Empty;

                    if (_ViewType == "LV")
                    {
                        desc = drpid["Family_ShortDescription"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");

                        prod_desc_alt = drpid["Prod_Description"].ToString();
                        if (prod_desc_alt.Length > 0)
                            _stmpl_records.SetAttribute("PROD_DESC_ALT", prod_desc_alt);
                        else
                            _stmpl_records.SetAttribute("PROD_DESC_ALT", drpid["FAMILY_NAME"]);

                        if (desc.Length > 230 && _ViewType == "LV")
                        {
                            _stmpl_records.SetAttribute("TBT_MORE_13", true);
                            desc = desc.Substring(0, 230).ToString();
                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_13", desc);
                        }
                        else
                        {
                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_13", desc);
                            _stmpl_records.SetAttribute("TBT_MORE_13", false);
                        }
                        desc = drpid["Family_Description"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");

                        if (desc.Length > 230 && _ViewType == "LV")
                        {
                            _stmpl_records.SetAttribute("TBT_MORE_90", true);
                            desc = desc.Substring(0, 230).ToString();
                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_90", desc);
                        }
                        else
                        {
                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_90", desc);
                            _stmpl_records.SetAttribute("TBT_MORE_90", false);
                        }


                    }
                    else
                    {
                        // descattr = drpid["Family_ShortDescription"].ToString() + " " + drpid["Family_Description"].ToString() + " " + drpid["Prod_Description"].ToString();

                        descattr = drpid["Family_ShortDescription"].ToString();
                        if (descattr.Length > 0)
                            descattr = descattr + "<br/>";
                        descattr = descattr + drpid["Family_Description"] + " " + drpid["Prod_Description"];

                        prod_desc_alt = drpid["Prod_Description"].ToString();
                        if (prod_desc_alt.Length > 0)
                            _stmpl_records.SetAttribute("PROD_DESC_ALT", prod_desc_alt);
                        else
                            _stmpl_records.SetAttribute("PROD_DESC_ALT", drpid["FAMILY_NAME"]);

                        if (descattr.Length > 140 && _ViewType == "GV")
                        {
                            int count = 0;
                            count = descattr.Count(c => char.IsUpper(c));
                            int count1 = 0;
                            count1 = descattr.Count(c => char.IsSymbol(c)) + descattr.Count(c => char.IsNumber(c));
                            if (count >= 35)
                            {
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                                descattr = descattr.Substring(0, 120).ToString();
                                descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                            }
                            else if (descattr.Length > 140 && count < 35 && count1 > 10)
                            {
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                                descattr = descattr.Substring(0, 135).ToString();
                                descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                            }
                            else if (descattr.Length > 140 && count < 35 && count1 < 10)
                            {
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                                descattr = descattr.Substring(0, 140).ToString();
                                descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);

                            }

                        }
                        else
                        {
                            _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                        }
                    }
                


                    //dsprodspecs = new DataSet();
                    //DataRow[] drow1 = dscat.Tables[2].Select("PRODUCT_ID='" + drpid["PRODUCT_ID"].ToString() + "' AND (ATTRIBUTE_TYPE=1 OR ATTRIBUTE_TYPE=4 OR ATTRIBUTE_TYPE=3) And FAMILY_ID='" + drpid["FAMILY_ID"].ToString() + "'");
                    //if (drow1.Length > 0)
                    //    dsprodspecs.Tables.Add(drow1.CopyToDataTable());
                    //else
                    //    dsprodspecs = null;



                    //if (dsprodspecs != null && dsprodspecs.Tables.Count >= 1 && dsprodspecs.Tables[0].Rows.Count >= 1)
                    //{
                    //    foreach (DataRow dr in dsprodspecs.Tables[0].Rows)
                    //    {
                    //        if (dr["ATTRIBUTE_TYPE"].ToString() == "1")
                    //        {
                    //            if (dr["ATTRIBUTE_ID"].ToString() == "1")
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), objHelperServices.StringTrim(dr["STRING_VALUE"].ToString(), 16, true));
                    //            else
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString());
                    //        }
                    //        else if (dr["ATTRIBUTE_TYPE"].ToString() == "4")
                    //        {
                    //            if (dr["NUMERIC_VALUE"].ToString().Length > 0)
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), Math.Round(Convert.ToDecimal(dr["NUMERIC_VALUE"]), 2).ToString());
                    //        }
                    //        else if (dr["ATTRIBUTE_TYPE"].ToString() == "3")
                    //        {
                    //            System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dr["STRING_VALUE"].ToString().Replace("/", "\\"));
                    //            if (Fil.Exists)
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));
                    //            else
                    //            {
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), "/images/noimage.gif");
                    //            }

                    //        }
                    //        else
                    //        {
                    //            _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString());
                    //        }

                    //    }

                    //}
                    //dsprodspecs = new DataSet();
                    //drow1 = dscat.Tables[2].Select("PRODUCT_ID='" + drpid["PRODUCT_ID"].ToString() + "' AND (ATTRIBUTE_TYPE=7 OR ATTRIBUTE_TYPE=9) And FAMILY_ID='" + drpid["FAMILY_ID"].ToString() + "'");
                    //if (drow1.Length > 0)
                    //    dsprodspecs.Tables.Add(drow1.CopyToDataTable());
                    //else
                    //    dsprodspecs = null;

                    //if (dsprodspecs != null && dsprodspecs.Tables.Count >= 1 && dsprodspecs.Tables[0].Rows.Count >= 1)
                    //{
                    //    string desc = "";
                    //    string descattr = "";
                    //    foreach (DataRow dr in dsprodspecs.Tables[0].Rows)
                    //    {

                    //        if (dr["ATTRIBUTE_TYPE"].ToString() == "9")
                    //        {
                    //            System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dr["STRING_VALUE"].ToString());
                    //            if (Fil.Exists)
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));
                    //            else
                    //            {
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), "/images/noimage.gif");
                    //            }

                    //        }
                    //        else if (dr["ATTRIBUTE_TYPE"].ToString() == "7")
                    //        {
                    //            //string desc = dr["STRING_VALUE"].ToString().Replace("\r", "&nbsp;").Replace("\n", "<br/>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                    //            // string desc = dr["STRING_VALUE"].ToString().Replace("\r", "&nbsp;");
                    //            desc = dr["STRING_VALUE"].ToString().Replace("\r", "").Replace("\n\r", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                    //            if (dr["ATTRIBUTE_ID"].ToString() == "13" && desc.Length > 0)
                    //                desc = desc + "<br/>";

                    //            descattr = descattr + desc;
                    //            if (desc.Length > 230 && _ViewType == "LV")
                    //            {
                    //                _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
                    //                desc = desc.Substring(0, 230).ToString();
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
                    //            }
                    //            //else if (desc.Length > 30 && _ViewType == "GV")
                    //            //{
                    //            //    _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
                    //            //    desc = desc.Substring(0, 30).ToString();
                    //            //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
                    //            //}
                    //            else
                    //            {
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
                    //                _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), false);
                    //            }
                    //        }
                    //        else
                    //        {

                    //            _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\r", "&nbsp;").Replace("\n", "<br/>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>"));
                    //        }

                    //    }
                    //    if (descattr.Length > 140 && _ViewType == "GV")
                    //    {
                    //        int count = 0;
                    //        count = descattr.Count(c => char.IsUpper(c));
                    //        if (count >= 35)
                    //        {
                    //            _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                    //            descattr = descattr.Substring(0, 120).ToString();
                    //            descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                    //            _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                    //        }
                    //        else if (descattr.Length > 140 && count < 35)
                    //        {
                    //            _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                    //            descattr = descattr.Substring(0, 140).ToString();
                    //            descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                    //            _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                    //        }

                    //    }
                    //    else
                    //        _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                    //}


                    if (BindToST == true)
                    {
                        lstrows[icolstart] = new TBWDataList(_stmpl_records.ToString());
                    }


                    icolstart++;
                    if (icolstart >= icol || oe == dscat.Tables["FamilyPro"].Rows.Count)
                    {
                        _stg_container = new StringTemplateGroup("CategoryProductListcontainer", stemplatepath);
                        _stmpl_container = _stg_container.GetInstanceOf("CategoryProductList\\producttable_" + soddevenrow);
                        _stmpl_container.SetAttribute("TBWDataList", lstrows);
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
                        ictrecords++;
                        icolstart = 0;
                        lstrows = new TBWDataList[icol];

                    }
                }

                //   }
                hfproductids.Value = tmpstrPid;
                hftmpdivcount.Value = tmpdivcount;
                _stg_container = new StringTemplateGroup("CategoryProductListmain", stemplatepath);

                if (Request.QueryString["ViewMode"] != null)
                {
                    PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                    isreadonly.SetValue(this.Request.QueryString, false, null);
                    this.Request.QueryString.Remove("ViewMode");
                }

                string productlistURL = Request.QueryString.ToString();
                string powersearchURListView = string.Empty;
                string powersearchURLGridView = string.Empty;
                powersearchURListView = productlistURL + "&ViewMode=LV";
                powersearchURLGridView = productlistURL + "&ViewMode=GV";


                _stmpl_container = _stg_container.GetInstanceOf("CategoryProductList\\main");
                _stmpl_container.SetAttribute("TBW_CATEGORY_ID", _cid);
                _stmpl_container.SetAttribute("TBW_PRODUCT_COUNT", iTotalProducts);
                _stmpl_container.SetAttribute("TBW_CATEGORY_NAME", category_nameh);

                _stmpl_container.SetAttribute("TBW_URL", powersearchURListView);
                _stmpl_container.SetAttribute("TBW_URL1", powersearchURLGridView);

                if (_ViewType == "LV")
                    _stmpl_container.SetAttribute("TBW_VIEWTYPE", true);
                else
                    _stmpl_container.SetAttribute("TBW_VIEWTYPE", false);


                if (HttpContext.Current.Session["SortOrder"] != null && HttpContext.Current.Session["SortOrder"] != "")
                {

                    if (HttpContext.Current.Session["SortOrder"].ToString() == "Latest")
                    {
                        _stmpl_container.SetAttribute("SortBy", "Latest");
                    }

                    else if (HttpContext.Current.Session["SortOrder"].ToString() == "ltoh")
                    {
                        _stmpl_container.SetAttribute("SortBy", "Price Low To High");
                    }
                    else if (HttpContext.Current.Session["SortOrder"].ToString() == "htol")
                    {
                        _stmpl_container.SetAttribute("SortBy", "Price High To Low");
                    }
                    else if (HttpContext.Current.Session["SortOrder"].ToString() == "popularity")
                    {
                        _stmpl_container.SetAttribute("SortBy", "Popular");
                    }
                    else
                        _stmpl_container.SetAttribute("SortBy", "Latest");
                }
                else
                    _stmpl_container.SetAttribute("SortBy", "Latest");

                if (iTotalPages > 1 && iPageNo != iTotalPages && iPageNo < iTotalPages)
                {
                    //_stmpl_container.SetAttribute("TBW_TOTAL_PAGES", iTotalPages);
                    //_stmpl_container.SetAttribute("TBW_PREVIOUS_PG_NO", iPrevPgNo);
                    //_stmpl_container.SetAttribute("TBW_CURRENT_PAGE_NO", iPageNo);
                    //_stmpl_container.SetAttribute("TBW_NEXT_PG_NO", iNextPgNo);
                    _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                    sHTML = _stmpl_container.ToString().Replace("<option value=\"" + iRecordsPerPage.ToString() + "\">", "<option value=\"" + iRecordsPerPage.ToString() + "\" selected =selected>");
                }
                else
                {
                    iPageNo = iTotalPages;
                    //_stmpl_container.SetAttribute("TBW_TOTAL_PAGES", iTotalPages);
                    //_stmpl_container.SetAttribute("TBW_PREVIOUS_PG_NO", iPrevPgNo);
                    //_stmpl_container.SetAttribute("TBW_CURRENT_PAGE_NO", iPageNo);
                    //_stmpl_container.SetAttribute("TBW_NEXT_PG_NO", iNextPgNo);
                    _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                    sHTML = _stmpl_container.ToString().Replace("<option value=\"" + iRecordsPerPage.ToString() + "\">", "<option value=\"" + iRecordsPerPage.ToString() + "\" selected =selected>");
                }

                if (sHTML.ToString().Contains("data-original=\"prodimages\""))
                {
                    sHTML = sHTML.Replace("data-original=\"prodimages\"", "data-original=\"images/noimage.gif\"");
                    sHTML = sHTML.Replace("href=\"javascript:Zoom('prodimages')\"", "href=\"#\"");
                }
                if (sHTML.ToString().Contains("data-original=\"\""))
                {
                    sHTML = sHTML.ToString().Replace("data-original=\"\"", "data-original=\"images/noimage.gif\"");
                    sHTML = sHTML.Replace("href=\"javascript:Zoom('')\"", "href=\"#\"");
                }
                if (dscat.Tables[1].Rows[0].ItemArray[0].ToString() == "0")
                    sHTML = "";
                if (dscat.Tables[0].Rows[0].ItemArray[0].ToString() == "1")
                {
                    sHTML = sHTML.Replace("<a href=\"powersearch.aspx?pgno=1\">Previous</a>", "");
                    sHTML = sHTML.Replace("<a href=\"powersearch.aspx?pgno=1\">Next</a>", "");
                }
                DataSet DSnaprod = new DataSet();

            }
            string eapath = _EAPath.Replace("'", "###");


            htmleapath.Value = eapath.ToString();

            htmltotalpages.Value = iTotalPages.ToString();
            htmlviewmode.Value = _ViewType;
            //if (Session["iRecordsPerPage"] != null)
            //{
            htmlirecords.Value = iRecordsPerPage.ToString();
         //   stopwatch.Stop();
          //  objErrorHandler.CreateLog("categoryproductlist renderhtmljson function load time:" + "=" + stopwatch.Elapsed);
        }
        catch (Exception ex)
        {
            sHTML = ex.Message;
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
        finally
        {

        }


       // return objHelperServices.StripWhitespace(sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>"));
        return sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
    }
    private void SetQueryString(StringTemplate _stmpl_pages)
    {
        _stmpl_pages.SetAttribute("TBW_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(_pcr));
        _stmpl_pages.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(_cid));
        _stmpl_pages.SetAttribute("TBW_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(_searchstr));
        _stmpl_pages.SetAttribute("TBW_ATTRIBUTE_TYPE", _type);
        _stmpl_pages.SetAttribute("TBW_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(_value));
        _stmpl_pages.SetAttribute("TBW_ATTRIBUTE_BRAND", HttpUtility.UrlEncode(_bname));
        _stmpl_pages.SetAttribute("TBW_CUSTOM_NUM_FIELD3", _byp);
        _stmpl_pages.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath)));

    }
    private string GetParentCatID(string catID)
    {
        try
        {
            DataSet DSBC = null;
            string catIDtemp = catID;
            do
            {
                //DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
                DSBC = (DataSet)objHelperDB.GetGenericPageDataDB(catIDtemp, "GET_CATEGORYLIST_CAREGORY", HelperDB.ReturnType.RTDataSet);
                if (DSBC != null)
                {
                    foreach (DataRow DR in DSBC.Tables[0].Rows)
                    {
                        catIDtemp = DR["PARENT_CATEGORY"].ToString();
                        if (catIDtemp == "0")
                        {
                            // DSUBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY = '" + DR["CATEGORY_ID"].ToString() + "' AND CATEGORY_NAME Like 'Product'");
                            return DR["CATEGORY_ID"].ToString();
                        }
                    }
                }
            } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
            return DSBC.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
        }
        catch (Exception ex)
        {

        }
        return "";
    }
    public string Category_RenderHTML(string Package, string SkinRootPath)
{
         
        string skin_container = null;
        int grid_cols = 0;
        int grid_rows = 0;
        string skin_sql_container = null;
        string skin_sql_param_container = null;
        string skin_records = null;

        TBWDataList[] lstrecords = new TBWDataList[0];
        TBWDataList[] lstrecords1 = new TBWDataList[0];
        TBWDataList[] lstrecords2 = new TBWDataList[0];
        StringTemplateGroup stg_records = null;
        StringTemplate bodyST_categorylist = null;
        StringTemplate bodyST_subcategorylist = null;
        StringTemplate bodyST = null;
        DataSet dspkg = new DataSet();
        string _wcat=null;

        DataTable SubCategory = new DataTable();


        if (Request.QueryString["wcat"] != null)
          _wcat = Request.QueryString["wcat"];

        try
        {

            //stopwatch.Start();
            string strPDFFiles1 = HttpContext.Current.Server.MapPath("attachments");
            string strPDFFiles2 = HttpContext.Current.Server.MapPath("News update");


            if (Request.QueryString["bypcat"] == null)
            {
                if (Request.QueryString["cid"] != null)
                {

                    //string sqlpkginfo = " SELECT * FROM TBW_PACKAGE ";
                    //sqlpkginfo = sqlpkginfo + " WHERE PACKAGE_NAME = '" + Package + "'";
                    //dspkg = GetDataSet(sqlpkginfo);
                    dspkg = (DataSet)objHelperDB.GetGenericDataDB(Package, "GET_PACKAGE_WITHOUT_ISROOT", HelperDB.ReturnType.RTDataSet);
                    if (dspkg != null)
                    {
                        if (dspkg.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dspkg.Tables[0].Rows)
                            {
                                skin_container = dr["SKIN_NAME"].ToString();
                                grid_cols = Convert.ToInt32(dr["GRID_COLS"]);
                                grid_rows = Convert.ToInt32(dr["GRID_ROWS"]);
                                skin_sql_container = dr["SKIN_SQL"].ToString();
                                skin_sql_param_container = dr["SKIN_SQL_PARAM"].ToString();
                                skin_records = dr["SKIN_NAME"].ToString();
                            }
                        }
                    }
                    if (Request.QueryString["cid"]!=null)
                    _catId = Request.QueryString["cid"].ToString();
                  //  string sqlcatquery = "";
                    //if (Request.QueryString["byp"] != null && (Request.QueryString["byp"].ToString() == "2" || Request.QueryString["byp"].ToString() == "3"))
                    //{
                    //    sqlcatquery = "select tc.category_id,tc.category_name,tc.image_file,tc.custom_num_field3 from tb_category tc, tb_catalog_sections tcs where tc.category_id=tcs.category_id and tc.category_name <>'Brand' and tc.Category_name <>'Product' AND ISNULL(TC.SHORT_DESC,'')<>'NOT FOR WEB' and tcs.catalog_id=" + _catalogid + " and tc.parent_category=(SELECT CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_NAME='PRODUCT' AND PARENT_CATEGORY='" + _catId + "') AND tc.custom_num_field3 in(1,3) order by tc.category_name";
                    //    stg_records = new StringTemplateGroup(skin_records + "listbrand", SkinRootPath + "\\" + skin_records + "listbrand");
                    //}
                    //else
                    //{
                    //sqlcatquery = "select tc.category_id,tc.category_name,tc.image_file,tc.image_file2,tc.custom_num_field3 from tb_category tc, tb_catalog_sections tcs where tc.category_id=tcs.category_id and tc.category_name <>'Brand' and tc.Category_name <>'Product' AND ISNULL(TC.SHORT_DESC,'')<>'NOT FOR WEB' and tcs.catalog_id=" + _catalogid + " and tc.parent_category='" + _catId + "' order by tc.category_name";
                    // By Jtech Mohan
                    //sqlcatquery = "select tc.category_id,tc.category_name,tc.image_file,tc.image_file2,tc.custom_num_field3 from tb_category tc, tb_catalog_sections tcs where tc.category_id=tcs.category_id and tc.category_name <>'Brand' and tc.Category_name <>'Product' AND ISNULL(TC.CUSTOM_NUM_FIELD3,0)<> 3 and tcs.catalog_id=" + _catalogid + " and tc.parent_category='" + _catId + "' order by tc.category_name";
                    // By Jtech Mohan

                    stg_records = new StringTemplateGroup(skin_records + "list", SkinRootPath + "\\" + skin_records + "list");
                    //}

                    //DataSet dscatname = GetDataSet(sqlcatquery);                

                    //SubCategory = EasyAsk.GetMainMenuClickDetail(_catId, "SubCategory");
                    DataTable dscatname = new DataTable();
                    SubCategory = (DataTable)((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["SubCategory"];
                       
                    //dscatname = EasyAsk.GetMainMenuClickDetail(_catId, "MainCategory");
                    dscatname = (DataTable)((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["MainCategory"];
                    
                    if (dscatname == null)
                        return "";

                    int tdrow = Convert.ToInt32(dscatname.Rows.Count / 2);

                    if ((dscatname.Rows.Count % 2) != 0)
                    {
                        tdrow++;
                    }
                    if (Request.QueryString["bypcat"] != null && Request.QueryString["bypcat"].ToString() == "1")
                    {
                        tdrow = Convert.ToInt32(dscatname.Rows.Count / 3);

                        if ((dscatname.Rows.Count % 3) != 0)
                        {
                            tdrow++;
                        }
                    }
                    lstrecords = new TBWDataList[tdrow];
                    lstrecords1 = new TBWDataList[tdrow];
                    lstrecords2 = new TBWDataList[tdrow];
                    if (dscatname.Rows.Count > 0)
                    {
                        int bodyValue = 0;
                        int colcount = 0;
                        int colcount1 = 0;
                      

                        foreach (DataRow _dsrow in dscatname.Rows)
                        {
                            string cellstr = string.Empty;
                            if (_dsrow["CATEGORY_NAME"].ToString().ToUpper() != "BRAND" && _dsrow["CATEGORY_NAME"].ToString().ToUpper() != "PRODUCT")
                            {
                                bodyST_categorylist = stg_records.GetInstanceOf("cell");
                                if (Request.QueryString["bypcat"] != null && Request.QueryString["bypcat"].ToString() == "1")
                                {
                                    bodyST_categorylist = stg_records.GetInstanceOf("cell2");
                                }
                                MainCategoryDisplay(bodyST_categorylist, _dsrow);
                                //bodyST_categorylist.SetAttribute("TBT_CATEGORY_NAME", _dsrow["CATEGORY_NAME"].ToString());
                                //bodyST_categorylist.SetAttribute("TBT_CATEGORY_ID", _dsrow["CATEGORY_ID"].ToString());
                                //FileInfo Fil = new FileInfo(strFile + _dsrow["IMAGE_FILE"].ToString());
                                //if (Fil.Exists)
                                //{
                                //    bodyST_categorylist.SetAttribute("TBT_IMAGE_FILE", _dsrow["IMAGE_FILE"].ToString().Replace("\\", "/"));
                                //}
                                //else
                                //{
                                //    bodyST_categorylist.SetAttribute("TBT_IMAGE_FILE", "");
                                //}

                                //if (_dsrow["CUSTOM_NUM_FIELD3"] != null && _dsrow["CUSTOM_NUM_FIELD3"].ToString() != "")
                                //{
                                //    if (Convert.ToInt32(_dsrow["CUSTOM_NUM_FIELD3"]) == 2)
                                //    {
                                //        //bodyST_categorylist.SetAttribute("TBT_URL", "byproduct.aspx");
                                //        bodyST_categorylist.SetAttribute("TBT_URL", "product_list.aspx");
                                //        bodyST_categorylist.SetAttribute("TBT_CUSTOM_NUM_FIELD3", Convert.ToInt32(_dsrow["CUSTOM_NUM_FIELD3"]).ToString() + "&pcr=" + _catId);
                                //    }
                                //    else
                                //    {
                                //        bodyST_categorylist.SetAttribute("TBT_URL", "product_list.aspx");
                                //        bodyST_categorylist.SetAttribute("TBT_CUSTOM_NUM_FIELD3", Convert.ToInt32(_dsrow["CUSTOM_NUM_FIELD3"]).ToString());
                                //    }

                                //}
                                //else
                                //{
                                //    bodyST_categorylist.SetAttribute("TBT_URL", "product_list.aspx");
                                //    bodyST_categorylist.SetAttribute("TBT_CUSTOM_NUM_FIELD3", "1");
                                //}

                                if (Request.QueryString["bypcat"] == null)
                                {

                                    // sqlcatquery = "select c.category_id,c.category_name,c.custom_num_field3 from tb_category c,TB_catalog_sections cs where c.category_id=cs.category_id and cs.catalog_id=" + _catalogid + " AND ISNULL(C.SHORT_DESC,'')<>'NOT FOR WEB'and c.parent_category='" + _dsrow["CATEGORY_ID"].ToString() + "' order by c.category_name";

                                    // By Jtech Mohan
                                    //sqlcatquery = "select c.category_id,c.category_name,c.custom_num_field3 as custom_num_field3 from tb_category c,TB_catalog_sections cs where c.category_id=cs.category_id and cs.catalog_id=" + _catalogid + " AND ISNULL(C.CUSTOM_NUM_FIELD3,0)<> 3 and c.parent_category='" + _dsrow["CATEGORY_ID"].ToString() + "' order by c.category_name";
                                    //DataTable dssubcatname = GetDataSet(sqlcatquery).Tables[0] ;


                                    DataRow[] Datarows = SubCategory.Select("PARENT_CATEGORY_ID='" + _dsrow["CATEGORY_ID"].ToString() + "'", "CATEGORY_NAME ASC");

                                   // comment by palani
                                    // By Jtech Mohan
                                    int subcount = 1;
                                    string subcat = string.Empty;
                                    string hidncat = string.Empty;

                                    if (Datarows.Length > 0)
                                    {
                                        DataTable dssubcatname = Datarows.CopyToDataTable();
                                        foreach (DataRow drow in dssubcatname.Rows)
                                        {
                                            bodyST_subcategorylist = stg_records.GetInstanceOf("cell1");

                                            if (subcount <= 3)
                                            {
                                                //bodyST_subcategorylist.SetAttribute("TBT_CATEGORY_NAME", drow["CATEGORY_NAME"].ToString());
                                                //bodyST_subcategorylist.SetAttribute("TBT_CATEGORY_ID", drow["CATEGORY_ID"].ToString());
                                                //if (drow["CUSTOM_NUM_FIELD3"] != null && drow["CUSTOM_NUM_FIELD3"].ToString() != "")
                                                //{
                                                //    if (Convert.ToInt32(drow["CUSTOM_NUM_FIELD3"]) == 2)
                                                //    {
                                                //        //bodyST_subcategorylist.SetAttribute("TBT_URL", "byproduct.aspx");
                                                //        bodyST_subcategorylist.SetAttribute("TBT_URL", "product_list.aspx");
                                                //        bodyST_subcategorylist.SetAttribute("TBT_CUSTOM_NUM_FIELD3", Convert.ToInt32(drow["CUSTOM_NUM_FIELD3"]).ToString() + "&pcr=" + _catId);
                                                //    }
                                                //    else
                                                //    {
                                                //        bodyST_subcategorylist.SetAttribute("TBT_URL", "product_list.aspx");
                                                //        bodyST_subcategorylist.SetAttribute("TBT_CUSTOM_NUM_FIELD3", Convert.ToInt32(drow["CUSTOM_NUM_FIELD3"]).ToString());

                                                //    }

                                                //}
                                                //else
                                                //{
                                                //    bodyST_subcategorylist.SetAttribute("TBT_URL", "product_list.aspx");
                                                //    bodyST_subcategorylist.SetAttribute("TBT_CUSTOM_NUM_FIELD3", "1");
                                                //}
                                                //bodyST_subcategorylist.SetAttribute("divid", drow["CATEGORY_ID"].ToString());
                                                SubCategoryDisplay(bodyST_subcategorylist, drow);
                                                bodyST_subcategorylist.SetAttribute("divdisplay", "Block");
                                                subcat += bodyST_subcategorylist.ToString();
                                                subcount++;
                                            }
                                            else
                                            {
                                                if (subcount == 4)
                                                {
                                                    hidncat += drow["CATEGORY_ID"].ToString();
                                                }
                                                else
                                                {
                                                    hidncat += "," + drow["CATEGORY_ID"].ToString();
                                                }
                                                //bodyST_subcategorylist.SetAttribute("TBT_CATEGORY_NAME", drow["CATEGORY_NAME"].ToString());
                                                //bodyST_subcategorylist.SetAttribute("TBT_CATEGORY_ID", drow["CATEGORY_ID"].ToString());
                                                //if (drow["CUSTOM_NUM_FIELD3"] != null && drow["CUSTOM_NUM_FIELD3"].ToString() != "")
                                                //{
                                                //    if (Convert.ToInt32(drow["CUSTOM_NUM_FIELD3"]) == 2)
                                                //    {
                                                //        //bodyST_subcategorylist.SetAttribute("TBT_URL", "byproduct.aspx");
                                                //        bodyST_subcategorylist.SetAttribute("TBT_URL", "product_list.aspx");
                                                //        bodyST_subcategorylist.SetAttribute("TBT_CUSTOM_NUM_FIELD3", Convert.ToInt32(drow["CUSTOM_NUM_FIELD3"]).ToString() + "&pcr=" + _catId);
                                                //    }
                                                //    else
                                                //    {
                                                //        bodyST_subcategorylist.SetAttribute("TBT_URL", "product_list.aspx");
                                                //        bodyST_subcategorylist.SetAttribute("TBT_CUSTOM_NUM_FIELD3", Convert.ToInt32(drow["CUSTOM_NUM_FIELD3"]).ToString());

                                                //    }

                                                //}
                                                //else
                                                //{
                                                //    bodyST_subcategorylist.SetAttribute("TBT_URL", "product_list.aspx");
                                                //    bodyST_subcategorylist.SetAttribute("TBT_CUSTOM_NUM_FIELD3", "1");
                                                //}
                                                //bodyST_subcategorylist.SetAttribute("divid", drow["CATEGORY_ID"].ToString());
                                                SubCategoryDisplay(bodyST_subcategorylist, drow);
                                                bodyST_subcategorylist.SetAttribute("divdisplay", "block");
                                                subcat += bodyST_subcategorylist.ToString();
                                                subcount++;
                                            }
                                        }
                                        //if (subcount >= 5)
                                        //{
                                        //    subcat += "<div id=\"divclick" + dssubcatname.Rows[0][0].ToString() + "\" style=\"display:Block; cursor :pointer;\" class=\"tx_3\" onclick=\"categoryclick('" + hidncat + ",divclick" + dssubcatname.Rows[0][0].ToString() + "')\"><b>> Click Here To See More of This Category</b></div>";
                                        //}

                                    }
                                    bodyST_categorylist.SetAttribute("TBWDataList", subcat);
                                }

                                cellstr += bodyST_categorylist.ToString();
                                bodyST = stg_records.GetInstanceOf("row");
                                if (Request.QueryString["bypcat"] != null && Request.QueryString["bypcat"].ToString() == "1")
                                {
                                    bodyST = stg_records.GetInstanceOf("row1");
                                }
                                bodyST.SetAttribute("TBWDataList", cellstr);
                                if (bodyValue < tdrow)
                                {
                                    lstrecords[bodyValue] = new TBWDataList(bodyST.ToString());
                                }
                                else
                                {
                                    if (colcount < tdrow)
                                    {
                                        lstrecords1[colcount] = new TBWDataList(bodyST.ToString());
                                        colcount++;
                                    }
                                    else
                                    {
                                        lstrecords2[colcount1] = new TBWDataList(bodyST.ToString());
                                        colcount1++;
                                    }

                                }
                                bodyValue++;
                            }
                        }
                    }

                    //string sqlquery = "select category_id,category_name,short_desc,image_file, image_file2 from tb_category where parent_category='0' AND ISNULL(SHORT_DESC,'')<>'NOT FOR WEB' and category_id='" + _catId + "'";
                    // Jtech Mohan
                    //string sqlquery = "select category_id,category_name,short_desc,image_file, image_file2 from tb_category where parent_category='0' AND ISNULL(CUSTOM_NUM_FIELD3,0)<> 3 and category_id='" + _catId + "'";
                    //DataSet dscat = GetDataSet(sqlquery);
                    DataSet dscat = new DataSet();

                    DataRow[] row = EasyAsk.GetCategoryAndBrand("MainCategory").Tables[0].Select("CATEGORY_ID='" + _catId + "'");
                    if (row.Length > 0)
                    {
                        dscat.Tables.Add(row.CopyToDataTable());
                    }
                    // Jtech Mohan
                    StringTemplate bodyST_main = stg_records.GetInstanceOf("main");


                    if (dscat.Tables[0].Rows[0]["CUSTOM_TEXT_FIELD2"].ToString() != "")
                    {
                      


                   string ebookpath=objHelperServices.viewebook(dscat.Tables[0].Rows[0]["CUSTOM_TEXT_FIELD2"].ToString());
                   if (ebookpath.Contains("www."))
                   {

                       ebookpath = ebookpath.ToLower().Replace("attachments", "").Replace("\\", "").Replace("http://", "").Replace("https://", "");
                       ebookpath = "http://" + ebookpath;
                   }
                        bodyST_main.SetAttribute("TBT_EBOOK_LINK", ebookpath.Replace("wes_secure_files/", ""));

                        bodyST_main.SetAttribute("TBT_DISPALY_EBOOK_LINK", "block");
                    }
                    else
                    {
                        bodyST_main.SetAttribute("TBT_EBOOK_LINK", "");
                        bodyST_main.SetAttribute("TBT_DISPALY_EBOOK_LINK", "none");
                    }

                    if (WesNewsCategoryId == _catId)
                    {
                       
                      

                        bodyST_main.SetAttribute("TBT_PDF_STATUS", false);
                        bodyST_main.SetAttribute("TBT_WESNEWS_LINK", true);
                    }
                    else
                    {
                        bodyST_main.SetAttribute("TBT_WESNEWS_LINK", false);
                        string Ebook_pdf_FileRef = System.Configuration.ConfigurationManager.AppSettings["Ebook_pdf_FileRef"].ToString();    
                        string newfile = dscat.Tables[0].Rows[0]["IMAGE_FILE2"].ToString();
                        if (!(newfile.ToLower().Contains(Ebook_pdf_FileRef)))
                        {
                            newfile = newfile.Replace("/media/", "/media/wes_secure_files/").Replace("\\media\\pdf\\", "\\media\\wes_secure_files\\pdf\\");
                        }

                        FileInfo Fil2 = new FileInfo(strPDFFiles1 + newfile);
                        FileInfo Fil3 = new FileInfo(strPDFFiles2 + newfile);
                         //Modified By :indu :Added replace function to remove  catelogdowload

                        bodyST_main.SetAttribute("PDF", newfile.Replace("\\", "/").Replace("wes_secure_files/", ""));
                       // bodyST_main.SetAttribute("PDF", dscat.Tables[0].Rows[0]["IMAGE_FILE2"].ToString());
                        if (Fil2.Exists || Fil3.Exists)
                        {
                         
                            bodyST_main.SetAttribute("TBT_PDF_STATUS", true);
                            
                            //bodyST_main.SetAttribute("TBT_PATH", strPDFFiles1);
                        }
                        else
                        {
                            bodyST_main.SetAttribute("TBT_PDF_STATUS", false);
                            //bodyST_main.SetAttribute("TBT_PATH", strPDFFiles1);
                        }


                    }

                    if (Request.QueryString["bypcat"] != null && Request.QueryString["bypcat"].ToString() == "1")
                    {
                        bodyST_main = stg_records.GetInstanceOf("main1");
                    }

                    bodyST_main.SetAttribute("TBWDataList", lstrecords);
                    bodyST_main.SetAttribute("TBWDataList1", lstrecords1);
                    bodyST_main.SetAttribute("TBWDataList3", ST_newproductsnav());
                    if (Request.QueryString["bypcat"] != null && Request.QueryString["bypcat"].ToString() == "1")
                    {
                        bodyST_main.SetAttribute("TBWDataList2", lstrecords2);
                    }
                    foreach (DataRow dsrow in dscat.Tables[0].Rows)
                    {
                        foreach (DataColumn dscol in dscat.Tables[0].Columns)
                        {
                            string dscolnameup = dscol.ColumnName.ToUpper();
                            if (dscolnameup == "IMAGE_FILE")
                            {
                                bodyST_main.SetAttribute("TBT_" + dscolnameup, dsrow[dscolnameup].ToString().Replace("\\", "/"));
                            }
                            else
                            {
                                bodyST_main.SetAttribute("TBT_" + dscolnameup, dsrow[dscolnameup].ToString());
                            }
                        }
                    }
                    string sHtmls = bodyST_main.ToString();
                    //if (sHtmls.Contains("src=\"prodimages\""))
                    //    sHtmls = sHtmls.Replace("src=\"prodimages\"", "src=\"images/noimage.gif\"");
                    //if (sHtmls.Contains("src=\"\""))
                    //{
                    //    sHtmls = sHtmls.Replace("src=\"\"", "src=\"images/noimage.gif\"");
                    //}


                    if (sHtmls.Contains("data-original=\"prodimages\""))
                        sHtmls = sHtmls.Replace("data-original=\"prodimages\"", "data-original=\"images/noimage.gif\"");
                    if (sHtmls.Contains("data-original=\"\""))
                    {
                        sHtmls = sHtmls.Replace("data-original=\"\"", "data-original=\"images/noimage.gif\"");
                    }

                    // sHtmls += ST_PDFDownload();
                   // return objHelperServices.StripWhitespace(sHtmls);
                 //   stopwatch.Stop();
                 //   objErrorHandler.CreateLog("category renderhtml function load time:" + "=" + stopwatch.Elapsed);
                    return sHtmls;
                }
            }
            else
            {
               // stopwatch.Start();
                if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "")
                {
                    string tosuite_brand = Server.UrlDecode(Request.QueryString["tsb"].ToString());
                    _catId = Request.QueryString["cid"].ToString();
                    _catName = GetCName(_catId);
                    //string sqlcatquery = "SELECT DISTINCT TOSUITE_MODEL,'/Section17_th/' + TOSUITE_MODEL_IMAGE AS IMAGE_FILE FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE SUBCATID_L1 <> '' AND CATEGORY_ID=N'" + _catId + "' AND CATALOG_ID=" + _catalogid + " AND TOSUITE_BRAND = '" + tosuite_brand + "' ORDER BY TOSUITE_MODEL";
                    //DataSet dscatname = GetDataSet(sqlcatquery);

                    HelperServices objHelperServices = new HelperServices();
                    //DataSet dscatname = EasyAsk.GetWESModel(_catName, Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()), tosuite_brand); 
                    DataSet dscatname = (DataSet)HttpContext.Current.Session["WESBrand_Model"];
                    TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("CATEGORYLISTIMG", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
                    tbwtEngine.paraValue = _catId;
                    tbwtEngine.GDataSet = dscatname;
                    tbwtEngine.RenderHTML("Row");
                    return (tbwtEngine.RenderedHTML);
                }
                else if (Request.QueryString["tsb"] == null && Request.QueryString["cid"] != null && Request.QueryString["cid"] != "")
                {
                    _catId = Request.QueryString["cid"].ToString();
                    _catName = GetCName(_catId);
                    //string sqlcatquery = "SELECT  DISTINCT TOSUITE_BRAND,TOSUITE_BRAND_IMAGE AS IMAGE_FILE FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE SUBCATID_L1 <> '' AND CATEGORY_ID=N'" + _catId + "' AND CATALOG_ID=" + _catalogid + " ORDER BY TOSUITE_BRAND";
                    //DataSet dscatname = GetDataSet(sqlcatquery);
                    DataSet dscatname = new DataSet();
                    HelperServices objHelperServices = new HelperServices();
                    //dscatname.Tables.Add (EasyAsk.GetMainMenuClickDetail(_catalogid ,"Brand").Copy());//
                    dscatname.Tables.Add((DataTable)((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["Brand"].Copy());


                    TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("CATEGORYLISTIMG", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
                    tbwtEngine.paraValue = _catId;
                    tbwtEngine.GDataSet = dscatname;
                    tbwtEngine.RenderHTML("Row");
                    return (tbwtEngine.RenderedHTML);
                }
                else
                {
                    _catId = Request.QueryString["cid"].ToString();
                    //old string sqlcatquery = "select tc.CATEGORY_ID,tc.CATEGORY_NAME,tc.IMAGE_FILE from tb_category tc, tb_catalog_sections tcs where tc.category_id=tcs.category_id and tc.category_name <>'Brand' and tc.Category_name <>'Product' AND ISNULL(TC.SHORT_DESC,'')<>'NOT FOR WEB' and tcs.catalog_id=" + _catalogid + " and tc.parent_category='" + _catId + "' order by tc.category_name";
                    //string sqlcatquery = "select tc.CATEGORY_ID,tc.CATEGORY_NAME,tc.IMAGE_FILE from tb_category tc, tb_catalog_sections tcs where tc.category_id=tcs.category_id and tc.category_name <>'Brand' and tc.Category_name <>'Product' AND ISNULL(TC.CUSTOM_NUM_FIELD3,0)<> 3 and tcs.catalog_id=" + _catalogid + " and tc.parent_category='" + _catId + "' order by tc.category_name";
                    //DataSet dscatname = GetDataSet(sqlcatquery);
                    DataSet dscatname = (DataSet)objHelperDB.GetGenericPageDataDB("", _catalogid, _catId, "GET_CATEGORYLIST_CATEGORY_NAME", HelperDB.ReturnType.RTDataSet);

                    HelperServices objHelperServices = new HelperServices();
                    TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("CATEGORYLISTIMG", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
                    tbwtEngine.paraValue = _catId;
                    tbwtEngine.GDataSet = dscatname;
                    tbwtEngine.RenderHTML("Row");
                    return (tbwtEngine.RenderedHTML);
                }
            }
           
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg =ex;
            objErrorHandler.CreateLog();
        }

        return "";

    }

    public string newproductsnav_RenderHTML(string Package, string SkinRootPath)
    {
        return "";
    }
    private string GetCName(string catID)
    {
        try
        {
            DataSet DSBC = null;
            string catIDtemp = catID;
            do
            {
                //DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
                DSBC = (DataSet)objHelperDB.GetGenericPageDataDB(catIDtemp, "GET_CATEGORYLIST_CAREGORY", HelperDB.ReturnType.RTDataSet);
                if (DSBC != null)
                {
                    foreach (DataRow DR in DSBC.Tables[0].Rows)
                    {
                        catIDtemp = DR["PARENT_CATEGORY"].ToString();
                        if (catIDtemp == "0")
                        {
                            // DSUBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY = '" + DR["CATEGORY_ID"].ToString() + "' AND CATEGORY_NAME Like 'Product'");
                            return DR["CATEGORY_NAME"].ToString();
                        }
                    }
                }
            } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
            return DSBC.Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
        }
        catch (Exception ex)
        {
        }
        return "";
    }
    public string Bread_Crumbs()
    {

        string breadcrumb = "", paraPID = "", paraFID = "", paraCID = "", byp = "";
        if (Request.QueryString["pid"] != null)
        {
            paraPID = Request.QueryString["pid"].ToString();
        }
        if (Request.QueryString["fid"] != null)
            paraFID = Request.QueryString["pid"].ToString();
        if (Request.QueryString["cid"] != null)
            paraCID = Request.QueryString["cid"].ToString();
        if (Request.QueryString["byp"] != null)
            byp = Request.QueryString["byp"].ToString();
        // by Jech
        //if (paraPID != "")
        //{
        //    DataSet DSBC = null;

        //    DSBC = GetDataSet("SELECT String_value FROM TB_prod_specs WHERE product_ID = " + paraPID + "and attribute_id=1");
        //    foreach (DataRow DR in DSBC.Tables[0].Rows)
        //    {
        //        breadcrumb = DR[0].ToString();
        //    }
        //    if (paraFID != "")
        //    {
        //        string catIDtemp = "";
        //        DSBC = GetDataSet("SELECT family_name,category_id FROM TB_family WHERE family_ID = " + paraFID);
        //        foreach (DataRow DR in DSBC.Tables[0].Rows)
        //        {
        //            breadcrumb = DR[0].ToString() + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
        //            catIDtemp = DR[1].ToString();
        //        }
        //        do
        //        {
        //            DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
        //            foreach (DataRow DR in DSBC.Tables[0].Rows)
        //            {
        //                breadcrumb = DR["CATEGORY_NAME"].ToString() + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
        //                catIDtemp = DR["PARENT_CATEGORY"].ToString();
        //            }
        //        } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
        //    }
        //}
        //else if (paraFID != "")
        //{
        //    DataSet DSBC = null;
        //    string catIDtemp = "";
        //    DSBC = GetDataSet("SELECT family_NAME,category_id FROM TB_family WHERE family_ID = " + paraFID);
        //    foreach (DataRow DR in DSBC.Tables[0].Rows)
        //    {
        //        breadcrumb = DR[0].ToString();
        //        catIDtemp = DR[1].ToString();
        //    }
        //    do
        //    {
        //        DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
        //        foreach (DataRow DR in DSBC.Tables[0].Rows)
        //        {
        //            if (breadcrumb == "")
        //                breadcrumb = DR["CATEGORY_NAME"].ToString();
        //            else
        //                breadcrumb = DR["CATEGORY_NAME"].ToString() + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
        //            catIDtemp = DR["PARENT_CATEGORY"].ToString();
        //        }
        //    } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
        //}
        //else if (paraCID != "")
        //{
        //    DataSet DSBC = null;
        //    string catIDtemp = paraCID;
        //    do
        //    {
        //        // Jtech mohan
        //        DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
        //        //DataRow[] row = EasyAsk.GetCategoryAndBrand("MainCategory").Tables[0].Select("CATEGORY_ID='" + catIDtemp + "'");
        //        //if (row.Length > 0)
        //        //{
        //        //    DSBC.Tables.Add(row.CopyToDataTable());
        //        //}
        //        //else
        //        //{
        //        //    DataRow[] row = EasyAsk.GetCategoryAndBrand("MainCategory").Tables[0].Select("CATEGORY_ID='" + catIDtemp + "'");
        //        //}

        //        //Jech Mohan
        //        foreach (DataRow DR in DSBC.Tables[0].Rows)
        //        {
        //            if (DR["PARENT_CATEGORY"].ToString() != "0")
        //            {
        //                if (breadcrumb == "")
        //                    breadcrumb = "<a href=\"product_list.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
        //                else
        //                    breadcrumb = "<a href=\"product_list.aspx?&ld=0&cid=" + DR["CATEGORY_ID"].ToString() + "\"  style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
        //            }
        //            else
        //            {
        //                if (breadcrumb == "")
        //                {
        //                    if (Request.QueryString["bypcat"] != null && Request.QueryString["bypcat"].ToString() != "")
        //                    {
        //                        //breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + catIDtemp + "&byp=" + byp + "&bypcat=" + Request.QueryString["bypcat"].ToString() + "\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
        //                        breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + catIDtemp + "&byp=" + byp + "\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
        //                    }
        //                    else
        //                    {
        //                        breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + catIDtemp + "&byp=" + byp + "\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
        //                    }
        //                    if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "")
        //                    {
        //                        //breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font><a href=\"categorylist.aspx?&ld=0&cid=" + catIDtemp + "&byp=" + byp + "&bypcat=" + Request.QueryString["bypcat"].ToString() + "&tsb=" + Request.QueryString["tsb"].ToString() + "\" style=\"color:Black;\">" + Request.QueryString["tsb"].ToString() + "</a>";
        //                        breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font><a href=\"categorylist.aspx?&ld=0&cid=" + catIDtemp + "&byp=" + byp + "&tsb=" + Server.UrlEncode(Request.QueryString["tsb"].ToString()) + "\" style=\"color:Black;\">" + Server.UrlDecode(Request.QueryString["tsb"].ToString()) + "</a>";
        //                    }
        //                }
        //                else
        //                {
        //                    if (Request.QueryString["bypcat"] != null && Request.QueryString["bypcat"].ToString() != "")
        //                    {
        //                        //breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + DR["CATEGORY_ID"].ToString() + "&byp=" + byp + "&bypcat=" + Request.QueryString["bypcat"].ToString() + "\"  style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
        //                        breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + DR["CATEGORY_ID"].ToString() + "&byp=" + byp + " \"  style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
        //                    }
        //                    else
        //                    {
        //                        breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + DR["CATEGORY_ID"].ToString() + "&byp=" + byp + "\"  style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
        //                    }
        //                    if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "")
        //                    {
        //                        //breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + catIDtemp + "&byp=" + byp + "&bypcat=" + Request.QueryString["bypcat"].ToString() + "&tsb=" + Request.QueryString["tsb"].ToString() + "\" style=\"color:Black;\">" + Request.QueryString["tsb"].ToString() + "</a><font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
        //                        breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + catIDtemp + "&byp=" + byp + "&tsb=" + Server.UrlEncode(Request.QueryString["tsb"].ToString()) + "\" style=\"color:Black;\">" + Server.UrlDecode(Request.QueryString["tsb"].ToString()) + "</a><font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
        //                    }
        //                }
        //            }
        //            catIDtemp = DR["PARENT_CATEGORY"].ToString();
        //        }
        //    } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");

        //}

       // stopwatch.Start();
        breadcrumb= EasyAsk.GetBreadCrumb(Server.MapPath("Templates"));
       // stopwatch.Stop();
      //  objErrorHandler.CreateLog("Bread_Crumbs function load time:" + "=" + stopwatch.Elapsed);
        return breadcrumb;
    }
    //private DataSet GetDataSet(string SQLQuery)
    //{
    //    DataSet ds = new DataSet();
    //    SqlDataAdapter da = new SqlDataAdapter(SQLQuery,  conStr.ConnectionString.Substring(conStr.ConnectionString.IndexOf(';') + 1));
    //    da.Fill(ds, "generictable");
    //    return ds;
    //}
    public string ST_PDFDownload()
    {
        StringTemplateGroup _stg_container = null;
        StringTemplateGroup _stg_records = null;
        StringTemplate _stmpl_container = null;
        StringTemplate _stmpl_records = null;
        TBWDataList[] lstrecords = new TBWDataList[0];


        string shtml = string.Empty;
        int counter = 0;

        if (Directory.Exists(Server.MapPath("attachments")))
        {

            //string[] fileEntries = Directory.GetFiles(Server.MapPath("attachments"), "*.pdf");
            //lstrecords = new TBWDataList[fileEntries.Length];
            //filenames = new string[fileEntries.Length];
            //if (fileEntries.Length > 0)

            DataSet dsPDFCount = new DataSet();
            dsPDFCount = objCategoryServices.GetCatalogPDFCount(2);

            //if (dsPDFCount != null)
            //{
            //    foreach (DataRow rPDF in dsPDFCount.Tables[0].Rows)
            //    {
            //        lstrecords = new TBWDataList[Convert.ToInt32(rPDF["CountFiles"].ToString())];
            //    }
            //}
            lstrecords = new TBWDataList[1];
            if (lstrecords.Length > 0)
            {

                DataSet dsCatalog = new DataSet();
                try
                {
                    dsCatalog = objCategoryServices.GetCatalogPDFDownload1(_catId);
                    if (dsCatalog != null)
                    {
                        foreach (DataRow rCat in dsCatalog.Tables[0].Rows)
                        {
                            string MyFile = Server.MapPath(string.Format("attachments/{0}", rCat["IMAGE_FILE2"].ToString()));

                            _stg_records = new StringTemplateGroup("Categorylist", stemplatepath);
                            _stmpl_records = _stg_records.GetInstanceOf("Categorylist" + "\\" + "cell3");
                            _stg_container = new StringTemplateGroup("cataloguedownload", stemplatepath);
                            _stmpl_container = _stg_container.GetInstanceOf("cataloguedownload" + "\\" + "row");

                            if (System.IO.File.Exists(MyFile))
                            {
                                _stmpl_records.SetAttribute("PDF", rCat["IMAGE_FILE2"].ToString());
                              //  _stmpl_records.SetAttribute("PDFFILEDESCRIPTION", rCat["IMAGE_NAME2"].ToString());

                                FileInfo finfo = new FileInfo(Server.MapPath(string.Format("attachments/{0}", rCat["IMAGE_FILE2"].ToString())));
                                long FileInBytes = finfo.Length;
                                long FileInKB = finfo.Length / 1024;

                               // _stmpl_records.SetAttribute("PDF_SIZE", FileInKB + " KB");
                              //  _stmpl_records.SetAttribute("PDF_DATE", rCat["MODIFIED_DATE"].ToString());

                                _stmpl_container.SetAttribute("TBWDataList", _stmpl_records.ToString());
                                lstrecords[counter] = new TBWDataList(_stmpl_container.ToString());
                                counter++;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    objErrorHandler.ErrorMsg = e;
                    objErrorHandler.CreateLog(); 
                }

                _stg_container = new StringTemplateGroup("Categorylist", stemplatepath);
                _stmpl_container = _stg_container.GetInstanceOf("Categorylist" + "\\" + "main2");
                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                shtml = _stmpl_container.ToString();
            }
            else
                return "<table height=\"100\"><tr><td valign=\"bottom\">No PDF Catalogue found</td></tr></table>";
        }
        else
            return "<table height=\"100\"><tr><td valign=\"bottom\">No PDF catalogue found</td></tr></table>";
        return shtml;

    }


    public string DynamicPag_RenderHTML(string URL, int ipageno, string eapath, string ViewMode, string irecords)
    {
        //if (HttpContext.Current.Session["PS_SEARCH_RESULTS"] == null)
        //{
        //    return "";
        //}
        string stemplatepath = string.Empty;
        ErrorHandler objErrorHandler = new ErrorHandler();
        ConnectionDB objConnectionDB = new ConnectionDB();
        Security objSecurity = new Security();
       // string _catId = "";
       // string _catName = "";
       // string _catalogid = "";

        CategoryServices objCategoryServices = new CategoryServices();
       // int iRecordsPerPage = 24;
        HelperDB objHelperDB = new HelperDB();
        HelperServices objHelperServices = new HelperServices();

        string strFile = HttpContext.Current.Server.MapPath("ProdImages");
        EasyAsk_WES EasyAsk = new EasyAsk_WES();
        string WesNewsCategoryId = System.Configuration.ConfigurationManager.AppSettings["WESNEWS_CATEGORY_ID"].ToString();

        string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();

      //  string package = "CATEGORYPRODUCTLIST";
        string SkinRootPath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());


        FamilyServices objFamilyServices = new FamilyServices();
        StringTemplateGroup _stg_container = null;
        StringTemplateGroup _stg_records = null;
        StringTemplate _stmpl_container = null;
        StringTemplate _stmpl_records = null;
        StringTemplate _stmpl_pages = null;
      //  DataSet dsprod = new DataSet();
        DataSet dsprodspecs = new DataSet();
        DataSet dscat = new DataSet();


        int oe = 0;
        string category_nameh = string.Empty;
        string sHTML = string.Empty;
        string _pcr = string.Empty;
        string _ViewType = string.Empty;

        //string catID = "";



        //if (Convert.ToInt32(HttpContext.Current.Session["PS_SEARCH_RESULTS"].ToString()) > 0)
        //{
        try
        {
            //objHelperServices.CheckCredential();
            string userid = string.Empty;
            if (HttpContext.Current.Session["USER_ID"] != null)
            {
                userid = HttpContext.Current.Session["USER_ID"].ToString();
            }
            //else
            //{
             
            //// HttpContext.Current.Response.Redirect("Login.aspx",false);
            //}
            dscat = Get_Value_Breadcrum(ipageno, eapath, irecords);
            if (HttpContext.Current.Request.QueryString["tsm"] != null && HttpContext.Current.Request.QueryString["tsm"].ToString() != "")
                _tsm = HttpContext.Current.Request.QueryString["tsm"];

            if (HttpContext.Current.Request.QueryString["tsb"] != null && HttpContext.Current.Request.QueryString["tsb"].ToString() != "")
                _tsb = HttpContext.Current.Request.QueryString["tsb"];

            if (HttpContext.Current.Request.QueryString["type"] != null)
                _type = HttpContext.Current.Request.QueryString["type"];

            if (HttpContext.Current.Request.QueryString["value"] != null)
                _value = HttpContext.Current.Request.QueryString["value"];

            if (HttpContext.Current.Request.QueryString["bname"] != null)
                _bname = HttpContext.Current.Request.QueryString["bname"];
            if (HttpContext.Current.Request.QueryString["searchstr"] != null)
                _searchstr = HttpContext.Current.Request.QueryString["searchstr"];
            if (HttpContext.Current.Request.QueryString["srctext"] != null)
                _searchstr = HttpContext.Current.Request.QueryString["srctext"];

            if (HttpContext.Current.Request.QueryString["cid"] != null)
                _cid = HttpContext.Current.Request.QueryString["cid"];

            if (HttpContext.Current.Request.QueryString["pcr"] != null)
                _pcr = HttpContext.Current.Request.QueryString["pcr"];


            if (HttpContext.Current.Request.QueryString["ViewMode"] != null)
            {
                _ViewType = HttpContext.Current.Request.QueryString["ViewMode"];               
            }
            else 
            {
                _ViewType = ViewMode;
            }
            //else if (HttpContext.Current.Session["PL_VIEW_MODE"] != null && HttpContext.Current.Session["PL_VIEW_MODE"].ToString() != "")
            //    _ViewType = HttpContext.Current.Session["PL_VIEW_MODE"].ToString();
            //else
            //    _ViewType = "GV";

            //if (HttpContext.Current.Request.QueryString["path"] != null)
            //    _EAPath = HttpUtility.UrlDecode(objSecurity.StringDeCrypt(HttpContext.Current.Request.QueryString["path"].ToString()));


            //if (HttpContext.Current.Session["iPageNo"] != null)
            //{
            //    iPageNo = Convert.ToInt32(HttpContext.Current.Session["iPageNo"]);
            //}









          



          
                _EAPath = dscat.Tables["eapath"].Rows[0][0].ToString();


            stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());



         //   dscat = (DataSet)HttpContext.Current.Session["FamilyProduct"];



            if (dscat != null)
            {

                if (HttpContext.Current.Request.QueryString["pcr"] != null)
                    _pcr = HttpContext.Current.Request.QueryString["pcr"];
                if (HttpContext.Current.Request.QueryString["type"] == null || HttpContext.Current.Request.QueryString["type"] == "")
                {
                    //GetDataSet("SELECT CATEGORY_NAME  FROM	TB_CATEGORY WHERE	CATEGORY_ID ='" + catID + "'").Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
                    string tempstr = (string)objHelperDB.GetGenericDataDB(_cid, "GET_CATEGORY_NAME", HelperDB.ReturnType.RTString);
                    if (tempstr != null && tempstr != string.Empty)
                        category_nameh = tempstr;
                }
                else
                    category_nameh = HttpContext.Current.Request.QueryString["value"].ToString();

                DataRow drpagect = dscat.Tables[0].Rows[0];
                iTotalPages = Convert.ToInt32(drpagect["TOTAL_PAGES"]);

                if (iPageNo > iTotalPages)
                {
                    iPageNo = iTotalPages;
                    //ps.PAGE_NO = iPageNo;
                }

                DataRow drproductsct = dscat.Tables[1].Rows[0];
                iTotalProducts = Convert.ToInt32(drproductsct["TOTAL_PRODUCTS"]);
                if (_cid != "")
                    _ParentCatID = GetParentCatID(_cid);

                if ((iTotalPages <= 0 || iTotalProducts <= 0) && WesNewsCategoryId == _ParentCatID)
                {


                    _stg_container = new StringTemplateGroup("CategoryProductListmain", stemplatepath);
                    _stmpl_container = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "advmain");

                    // SetEbookAndPDFLink(_stmpl_container);
                    DataTable tmpDataTable = (DataTable)objHelperDB.GetGenericDataDB(WesCatalogId, "'" + _cid + "'", "GET_CATEGORY", HelperDB.ReturnType.RTTable);
                    if (tmpDataTable != null && tmpDataTable.Rows.Count > 0)
                    {

                        //modified by:indu
                        string ebookpath = tmpDataTable.Rows[0]["CUSTOM_TEXT_FIELD3"].ToString();

                        if (ebookpath.Contains("www."))
                        {

                            ebookpath = ebookpath.ToLower().Replace("attachments", "").Replace("\\", "").Replace("http://", "").Replace("https://", "");
                            ebookpath = "http://" + ebookpath;
                        }
                        _stmpl_container.SetAttribute("TBT_ADV_URL_LINK", ebookpath);
                    }
                    _stmpl_container.SetAttribute("TBW_CATEGORY_NAME", category_nameh);
                    return _stmpl_container.ToString();
                }



                TBWDataList[] lstrecords = new TBWDataList[0];
                TBWDataList[] lstrows = new TBWDataList[0];
                _stg_records = new StringTemplateGroup("CategoryProductList", stemplatepath);


                int ictrecords = 0;
                int icolstart = 0;
                string trmpstr = string.Empty;
                int icol = 1;
                lstrows = new TBWDataList[icol];

                if (_ViewType == "GV")
                {
                    icol = 4;
                    lstrows = new TBWDataList[icol];
                }
                else
                {
                    icol = 1;
                    lstrows = new TBWDataList[icol];
                }

                //if (dscat.Tables[0].Rows.Count < icol)
                //{
                //    icol = dscat.Tables[0].Rows.Count;
                //}
                string soddevenrow = "odd";

                DataRow[] drprodcoll = dscat.Tables[2].Select("ATTRIBUTE_ID = 1");



                string PriceTable = string.Empty;
                int pricecode = 0;
                string tmpProds = string.Empty;
                DataSet dsBgDisc = new DataSet();
                DataSet dsPriceTableAll = new DataSet();
               

                string _Buyer_Group = objFamilyServices.GetBuyerGroup(Convert.ToInt32(userid));
                if (Convert.ToInt32(userid) > 0)
                {

                    dsBgDisc = objFamilyServices.GetBuyerGroupBasedDiscountDetails(_Buyer_Group);
                }
                else
                {
                    dsBgDisc = objFamilyServices.GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
                }
                pricecode = objHelperDB.GetPriceCode(userid);

                //if (HttpContext.Current.Session["RECORDS_PER_PAGE_CATEGORY_LIST"] != null)
                //    iRecordsPerPage = Convert.ToInt32(HttpContext.Current.Session["RECORDS_PER_PAGE_CATEGORY_LIST"].ToString());

                tmpProds = "";
                if (Convert.ToInt32(userid) > 0)
                {
                    foreach (DataRow drpid in drprodcoll)
                    {
                        tmpProds = tmpProds + drpid["PRODUCT_ID"].ToString() + ",";
                    }
                    if (tmpProds != "")
                    {
                        tmpProds = tmpProds.Substring(0, tmpProds.Length - 1);
                      //  dsPriceTableAll = objHelperDB.GetProductPriceTableAll(tmpProds, Convert.ToInt32(userid));
                    }
                }

                string CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(userid));
                lstrecords = new TBWDataList[drprodcoll.Length + 1];

                foreach (DataRow drpid in drprodcoll)
                {
                    oe++;
                    if ((oe % 2) == 0)
                    {
                        soddevenrow = "even";
                    }
                    else
                    {
                        soddevenrow = "odd";
                    }

                    if (_ViewType == "GV")
                        _stmpl_records = _stg_records.GetInstanceOf("CategoryProductList" + "\\" + "productlist_GridView");
                    else
                        _stmpl_records = _stg_records.GetInstanceOf("CategoryProductList" + "\\" + "productlist_WES" + soddevenrow);

                    //string urlDesc = "";

                    _stmpl_records.SetAttribute("FAMILY_ID", drpid["FAMILY_ID"].ToString());
                    _stmpl_records.SetAttribute("CATEGORY_ID", drpid["CATEGORY_ID"].ToString());
                    _stmpl_records.SetAttribute("PARENT_CATEGORY_ID", drpid["PARENT_CATEGORY_ID"].ToString());

                    _stmpl_records.SetAttribute("FAMILY_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath)));

                    _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath + "////UserSearch1=Family Id=" + drpid["FAMILY_ID"].ToString())));

                    _stmpl_records.SetAttribute("TBT_PRODUCT_ID", drpid["PRODUCT_ID"].ToString());
                    _stmpl_records.SetAttribute("PRODUCT_CODE", drpid["PRODUCT_CODE"].ToString());
                    _stmpl_records.SetAttribute("TBT_FAMILY_ID", drpid["FAMILY_ID"].ToString());
                    trmpstr = drpid["FAMILY_NAME"].ToString().Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
                    if (_ViewType == "GV")
                    {

                        if (trmpstr.Length > 60)
                            trmpstr = trmpstr.Substring(0, 60) + "...";
                    }

                    _stmpl_records.SetAttribute("FAMILY_NAME", trmpstr);

                    _stmpl_records.SetAttribute("FAMILY_PRODUCT_COUNT", drpid["FAMILY_PRODUCT_COUNT"].ToString());
                    _stmpl_records.SetAttribute("PRODUCT_COUNT", drpid["PRODUCT_COUNT"].ToString());

                    _stmpl_records.SetAttribute("TBT_ECOMENABLED", objHelperServices.GetIsEcomEnabled(userid));
                    if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
                        _stmpl_records.SetAttribute("TBT_WITH_LOGIN", true);
                    //  string ValueFortag = string.Empty;
                    //ValueFortag = "<div id=\"pid" + dscat.Tables[2].Rows[0]["PRODUCT_ID"].ToString() + "\" style=\"background-color:#ffffff;visibility:hidden;position:absolute\">" + AssemblePriceTable(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus) + "</div><div onMouseOver=\"javascript:ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onMouseOut=\"javascript:ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" style=\"position:relative\">" + ValueFortag + "</div>";
                    else
                        _stmpl_records.SetAttribute("TBT_WITH_LOGIN", false);

                    if (Convert.ToInt32(drpid["PRODUCT_COUNT"].ToString()) > 1)
                    {
                        _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", false);
                        _stmpl_records.SetAttribute("TBT_SUB_FAMILY", true);
                        _stmpl_records.SetAttribute("TBT_MIN_PRICE", drpid["MIN_PRICE"].ToString());
                    }
                    else
                    {
                        _stmpl_records.SetAttribute("TBT_SUB_FAMILY", false);
                        _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", true);



                        if (dsBgDisc != null)
                        {
                            if (dsBgDisc.Tables[0].Rows.Count > 0)
                            {
                                decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                                DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                                string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                                //untPrice = objHelperServices.CDEC(DsPreview.Tables[_familyID].Rows[i][j].ToString());
                                bool IsBGCatProd = objFamilyServices.IsBGCatalogProduct(Convert.ToInt32(WesCatalogId), _Buyer_Group);
                                if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && (IsBGCatProd))
                                {
                                    //ValueFortag = objFamilyServices.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth).ToString();

                                }
                            }
                        }
                        //  commented for product price table
                        //if (Convert.ToInt32(userid) > 0)
                        //{

                        //    PriceTable = objFamilyServices.AssemblePriceTable(objHelperServices.CI(drpid["PRODUCT_ID"].ToString()), pricecode, drpid["PRODUCT_CODE"].ToString(), drpid["STOCK_STATUS_DESC"].ToString(), drpid["PROD_STOCK_STATUS"].ToString(), CustomerType, Convert.ToInt32(userid), drpid["PROD_STOCK_FLAG"].ToString(), drpid["ETA"].ToString(), dsPriceTableAll);
                        //}
                        //_stmpl_records.SetAttribute("TBT_PRODUCT_PRICE_TABLE", PriceTable);


                        // _stmpl_records.SetAttribute("TBT_ECOMENABLED", (Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]) < 4 ? true : false));

                        if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
                            _stmpl_records.SetAttribute("TBT_WITH_LOGIN", true);
                        //  string ValueFortag = string.Empty;
                        //ValueFortag = "<div id=\"pid" + dscat.Tables[2].Rows[0]["PRODUCT_ID"].ToString() + "\" style=\"background-color:#ffffff;visibility:hidden;position:absolute\">" + AssemblePriceTable(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus) + "</div><div onMouseOver=\"javascript:ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onMouseOut=\"javascript:ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" style=\"position:relative\">" + ValueFortag + "</div>";
                        else
                            _stmpl_records.SetAttribute("TBT_WITH_LOGIN", false);

                        DataRow[] drow = dscat.Tables[2].Select("PRODUCT_ID='" + drpid["PRODUCT_ID"].ToString() + "' AND ATTRIBUTE_TYPE=0");
                        if (drow.Length > 0) // Data Rows must return 1 row 
                        {
                            DataTable td = drow.CopyToDataTable();
                            _stmpl_records.SetAttribute("TBT_USER_PRICE", td.Rows[0]["NUMERIC_VALUE"].ToString());

                            if (Convert.ToInt32(td.Rows[0]["QTY_AVAIL"].ToString()) != -1)
                            {
                                if (Convert.ToInt32(td.Rows[0]["QTY_AVAIL"].ToString()) > 0)
                                {
                                    _stmpl_records.SetAttribute("TBT_QTY_AVAIL", td.Rows[0]["QTY_AVAIL"].ToString());
                                    _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", td.Rows[0]["MIN_ORD_QTY"].ToString());
                                }
                                else
                                {
                                    _stmpl_records.SetAttribute("TB_DISPLAY", "none");
                                }
                                //_stmpl_records.SetAttribute("TBT_PRODUCT_ID", td.Rows[0]["PRODUCT_ID"].ToString());
                            }
                        }



                    }
                    dsprodspecs = new DataSet();
                    DataRow[] drow1 = dscat.Tables[2].Select("PRODUCT_ID='" + drpid["PRODUCT_ID"].ToString() + "' AND (ATTRIBUTE_TYPE=1 OR ATTRIBUTE_TYPE=4 OR ATTRIBUTE_TYPE=3) And FAMILY_ID='" + drpid["FAMILY_ID"].ToString() + "'");
                    if (drow1.Length > 0)
                        dsprodspecs.Tables.Add(drow1.CopyToDataTable());
                    else
                        dsprodspecs = null;



                    if (dsprodspecs != null && dsprodspecs.Tables.Count >= 1 && dsprodspecs.Tables[0].Rows.Count >= 1)
                    {
                        foreach (DataRow dr in dsprodspecs.Tables[0].Rows)
                        {
                            if (dr["ATTRIBUTE_TYPE"].ToString() == "1")
                            {
                                if (dr["ATTRIBUTE_ID"].ToString() == "1")
                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), objHelperServices.StringTrim(dr["STRING_VALUE"].ToString(), 16, true));
                                else
                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString());
                            }
                            else if (dr["ATTRIBUTE_TYPE"].ToString() == "4")
                            {
                                if (dr["NUMERIC_VALUE"].ToString().Length > 0)
                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), Math.Round(Convert.ToDecimal(dr["NUMERIC_VALUE"]), 2).ToString());
                            }
                            else if (dr["ATTRIBUTE_TYPE"].ToString() == "3")
                            {
                                //System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dr["STRING_VALUE"].ToString().Replace("/", "\\"));
                                //if (Fil.Exists)
                                //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));
                                //else
                                //{
                                //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), "/images/noimage.gif");
                                //}

                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));

                            }
                            else
                            {
                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString());
                            }

                        }

                    }
                    dsprodspecs = new DataSet();
                    drow1 = dscat.Tables[2].Select("PRODUCT_ID='" + drpid["PRODUCT_ID"].ToString() + "' AND (ATTRIBUTE_TYPE=7 OR ATTRIBUTE_TYPE=9) And FAMILY_ID='" + drpid["FAMILY_ID"].ToString() + "'");
                    if (drow1.Length > 0)
                        dsprodspecs.Tables.Add(drow1.CopyToDataTable());
                    else
                        dsprodspecs = null;

                    if (dsprodspecs != null && dsprodspecs.Tables.Count >= 1 && dsprodspecs.Tables[0].Rows.Count >= 1)
                    {
                        string desc = string.Empty;
                        string descattr = string.Empty;
                        foreach (DataRow dr in dsprodspecs.Tables[0].Rows)
                        {

                            if (dr["ATTRIBUTE_TYPE"].ToString() == "9")
                            {

                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));

                                //System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dr["STRING_VALUE"].ToString());
                                //if (Fil.Exists)
                                //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));
                                //else
                                //{
                                //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), "/images/noimage.gif");
                                //}

                            }
                            else if (dr["ATTRIBUTE_TYPE"].ToString() == "7")
                            {
                                //string desc = dr["STRING_VALUE"].ToString().Replace("\r", "&nbsp;").Replace("\n", "<br/>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                                //string desc = dr["STRING_VALUE"].ToString().Replace("\r", "&nbsp;");
                                desc = dr["STRING_VALUE"].ToString().Replace("\r", "").Replace("\n\r","").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                                if (dr["ATTRIBUTE_ID"].ToString() == "13" && desc.Length > 0)
                                    desc = desc + "<br/>";
                                descattr = descattr + desc;
                                if (desc.Length > 230 && _ViewType == "LV")
                                {
                                    _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
                                    desc = desc.Substring(0, 230).ToString();
                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
                                }
                                //else if (desc.Length > 30 && _ViewType == "GV")
                                //{
                                //    _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
                                //    desc = desc.Substring(0, 30).ToString();
                                //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
                                //}
                                else
                                {
                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
                                    _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), false);
                                }
                            }
                            else
                            {

                                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\r", "&nbsp;").Replace("\n", "<br/>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>"));
                            }

                        }
                        if (descattr.Length > 140 && _ViewType == "GV")
                        {
                            int count = 0;
                            count = descattr.Count(c => char.IsUpper(c));
                            if (count >= 35)
                            {
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                                descattr = descattr.Substring(0, 120).ToString();
                                descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                            }
                            else if (descattr.Length > 140 && count < 35)
                            {
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                                descattr = descattr.Substring(0, 140).ToString();
                                descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                            }
                      
                        }
                        else
                            _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                    }



                    lstrows[icolstart] = new TBWDataList(_stmpl_records.ToString());


                    icolstart++;
                    if (icolstart >= icol || oe == drprodcoll.Length)
                    {
                        _stg_container = new StringTemplateGroup("CategoryProductListcontainer", stemplatepath);
                        _stmpl_container = _stg_container.GetInstanceOf("CategoryProductList" + "\\" + "producttable_" + soddevenrow);
                        _stmpl_container.SetAttribute("TBWDataList", lstrows);
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
                        sHTML = sHTML + _stmpl_container.ToString();
                        ictrecords++;
                        icolstart = 0;
                        lstrows = new TBWDataList[icol];

                    }
                }

                //   }

                _stg_container = new StringTemplateGroup("CategoryProductListmain", stemplatepath);

                if (HttpContext.Current.Request.QueryString["ViewMode"] != null)
                {
                    PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                    isreadonly.SetValue(HttpContext.Current.Request.QueryString, false, null);
                    HttpContext.Current.Request.QueryString.Remove("ViewMode");
                }

                string productlistURL = HttpContext.Current.Request.QueryString.ToString();
                string powersearchURListView = string.Empty;
                string powersearchURLGridView = string.Empty;
                powersearchURListView = productlistURL + "&ViewMode=LV";
                powersearchURLGridView = productlistURL + "&ViewMode=GV";




            }
        }
        catch (Exception ex)
        {
            objErrorHandler.CreateLog(ex.ToString() );  
            sHTML = ex.Message;
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
        finally
        {

        }

        //return objHelperServices.StripWhitespace(sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>"));
        return sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
    }

    public string DynamicPag_RenderHTMLJson(string URL, int ipageno, string eapath, string ViewMode, string irecords)
    {
        //if (HttpContext.Current.Session["PS_SEARCH_RESULTS"] == null)
        //{
        //    return "";
        //}
        string stemplatepath = string.Empty;
        ErrorHandler objErrorHandler = new ErrorHandler();
        ConnectionDB objConnectionDB = new ConnectionDB();
        Security objSecurity = new Security();
       // string _catId = "";
       // string _catName = "";
       // string _catalogid = "";

        CategoryServices objCategoryServices = new CategoryServices();
      //  int iRecordsPerPage = 24;
        HelperDB objHelperDB = new HelperDB();
        HelperServices objHelperServices = new HelperServices();

        string strFile = HttpContext.Current.Server.MapPath("ProdImages");
        EasyAsk_WES EasyAsk = new EasyAsk_WES();
        string WesNewsCategoryId = System.Configuration.ConfigurationManager.AppSettings["WESNEWS_CATEGORY_ID"].ToString();

        string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();

       // string package = "CATEGORYPRODUCTLIST";
        string SkinRootPath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());


        FamilyServices objFamilyServices = new FamilyServices();
        StringTemplateGroup _stg_container = null;
        StringTemplateGroup _stg_records = null;
        StringTemplate _stmpl_container = null;
        StringTemplate _stmpl_records = null;
       // StringTemplate _stmpl_pages = null;
        DataSet dsprod = new DataSet();
        DataSet dsprodspecs = new DataSet();
        DataSet dscat = new DataSet();
        bool BindToST = true;

        int oe = 0;
        string category_nameh = string.Empty;
        string sHTML = string.Empty;
        string _pcr = string.Empty;
        string _ViewType = string.Empty;

        //string catID = "";



        //if (Convert.ToInt32(HttpContext.Current.Session["PS_SEARCH_RESULTS"].ToString()) > 0)
        //{
        try
        {
            //objHelperServices.CheckCredential();
            string userid = string.Empty;
            if (HttpContext.Current.Session["USER_ID"] != null)
            {
                userid = HttpContext.Current.Session["USER_ID"].ToString();
            }
            //else
            //{

            //// HttpContext.Current.Response.Redirect("Login.aspx",false);
            //}
            dscat = Get_Value_Breadcrum(ipageno, eapath, irecords);
            if (HttpContext.Current.Request.QueryString["tsm"] != null && HttpContext.Current.Request.QueryString["tsm"].ToString() != string.Empty)
                _tsm = HttpContext.Current.Request.QueryString["tsm"];

            if (HttpContext.Current.Request.QueryString["tsb"] != null && HttpContext.Current.Request.QueryString["tsb"].ToString() != string.Empty)
                _tsb = HttpContext.Current.Request.QueryString["tsb"];

            if (HttpContext.Current.Request.QueryString["type"] != null)
                _type = HttpContext.Current.Request.QueryString["type"];

            if (HttpContext.Current.Request.QueryString["value"] != null)
                _value = HttpContext.Current.Request.QueryString["value"];

            if (HttpContext.Current.Request.QueryString["bname"] != null)
                _bname = HttpContext.Current.Request.QueryString["bname"];
            if (HttpContext.Current.Request.QueryString["searchstr"] != null)
                _searchstr = HttpContext.Current.Request.QueryString["searchstr"];
            if (HttpContext.Current.Request.QueryString["srctext"] != null)
                _searchstr = HttpContext.Current.Request.QueryString["srctext"];

            if (HttpContext.Current.Request.QueryString["cid"] != null)
                _cid = HttpContext.Current.Request.QueryString["cid"];

            if (HttpContext.Current.Request.QueryString["pcr"] != null)
                _pcr = HttpContext.Current.Request.QueryString["pcr"];


            if (HttpContext.Current.Request.QueryString["ViewMode"] != null)
            {
                _ViewType = HttpContext.Current.Request.QueryString["ViewMode"];
            }
            else
            {
                _ViewType = ViewMode;
            }
            //else if (HttpContext.Current.Session["PL_VIEW_MODE"] != null && HttpContext.Current.Session["PL_VIEW_MODE"].ToString() != "")
            //    _ViewType = HttpContext.Current.Session["PL_VIEW_MODE"].ToString();
            //else
            //    _ViewType = "GV";

            //if (HttpContext.Current.Request.QueryString["path"] != null)
            //    _EAPath = HttpUtility.UrlDecode(objSecurity.StringDeCrypt(HttpContext.Current.Request.QueryString["path"].ToString()));


            //if (HttpContext.Current.Session["iPageNo"] != null)
            //{
            //    iPageNo = Convert.ToInt32(HttpContext.Current.Session["iPageNo"]);
            //}














            _EAPath = dscat.Tables["eapath"].Rows[0][0].ToString();


            stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());



            //   dscat = (DataSet)HttpContext.Current.Session["FamilyProduct"];



            if (dscat != null)
            {

                if (HttpContext.Current.Request.QueryString["pcr"] != null)
                    _pcr = HttpContext.Current.Request.QueryString["pcr"];
                if (HttpContext.Current.Request.QueryString["type"] == null || HttpContext.Current.Request.QueryString["type"] == string.Empty)
                {
                    //GetDataSet("SELECT CATEGORY_NAME  FROM	TB_CATEGORY WHERE	CATEGORY_ID ='" + catID + "'").Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
                    string tempstr = (string)objHelperDB.GetGenericDataDB(_cid, "GET_CATEGORY_NAME", HelperDB.ReturnType.RTString);
                    if (tempstr != null && tempstr != string.Empty)
                        category_nameh = tempstr;
                }
                else
                    category_nameh = HttpContext.Current.Request.QueryString["value"].ToString();

                DataRow drpagect = dscat.Tables[0].Rows[0];
                iTotalPages = Convert.ToInt32(drpagect["TOTAL_PAGES"]);

                if (iPageNo > iTotalPages)
                {
                    iPageNo = iTotalPages;
                    //ps.PAGE_NO = iPageNo;
                }

                DataRow drproductsct = dscat.Tables[1].Rows[0];
                iTotalProducts = Convert.ToInt32(drproductsct["TOTAL_PRODUCTS"]);
                if (_cid != "")
                    _ParentCatID = GetParentCatID(_cid);

                if ((iTotalPages <= 0 || iTotalProducts <= 0) && WesNewsCategoryId == _ParentCatID)
                {


                    _stg_container = new StringTemplateGroup("CategoryProductListmain", stemplatepath);
                    _stmpl_container = _stg_container.GetInstanceOf("CategoryProductList\\advmain");

                    // SetEbookAndPDFLink(_stmpl_container);
                    DataTable tmpDataTable = (DataTable)objHelperDB.GetGenericDataDB(WesCatalogId, "'" + _cid + "'", "GET_CATEGORY", HelperDB.ReturnType.RTTable);
                    if (tmpDataTable != null && tmpDataTable.Rows.Count > 0)
                    {

                        //modified by:indu
                        string ebookpath = tmpDataTable.Rows[0]["CUSTOM_TEXT_FIELD3"].ToString();

                        if (ebookpath.Contains("www."))
                        {

                            ebookpath = ebookpath.ToLower().Replace("attachments", "").Replace("\\", "").Replace("http://", "").Replace("https://", "");
                            ebookpath = "http://" + ebookpath;
                        }
                        _stmpl_container.SetAttribute("TBT_ADV_URL_LINK", ebookpath);
                    }
                    _stmpl_container.SetAttribute("TBW_CATEGORY_NAME", category_nameh);
                    return _stmpl_container.ToString();
                }



                TBWDataList[] lstrecords = new TBWDataList[0];
                TBWDataList[] lstrows = new TBWDataList[0];
                _stg_records = new StringTemplateGroup("CategoryProductList", stemplatepath);


                int ictrecords = 0;
                int icolstart = 0;
                string trmpstr = string.Empty;
                int icol = 1;
                lstrows = new TBWDataList[icol];

                if (_ViewType == "GV")
                {
                    icol = 4;
                    lstrows = new TBWDataList[icol];
                }
                else
                {
                    icol = 1;
                    lstrows = new TBWDataList[icol];
                }

                //if (dscat.Tables[0].Rows.Count < icol)
                //{
                //    icol = dscat.Tables[0].Rows.Count;
                //}
                string soddevenrow = "odd";

               // DataRow[] drprodcoll = dscat.Tables[2].Select("ATTRIBUTE_ID = 1");



                string PriceTable = string.Empty;
                int pricecode = 0;
                string tmpProds = string.Empty;
                DataSet dsBgDisc = new DataSet();
                DataSet dsPriceTableAll = new DataSet();


                //string _Buyer_Group = objFamilyServices.GetBuyerGroup(Convert.ToInt32(userid));
                //if (Convert.ToInt32(userid) > 0)
                //{

                //    dsBgDisc = objFamilyServices.GetBuyerGroupBasedDiscountDetails(_Buyer_Group);
                //}
                //else
                //{
                //    dsBgDisc = objFamilyServices.GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
                //}
                pricecode = objHelperDB.GetPriceCode(userid);

                //if (HttpContext.Current.Session["RECORDS_PER_PAGE_CATEGORY_LIST"] != null)
                //    iRecordsPerPage = Convert.ToInt32(HttpContext.Current.Session["RECORDS_PER_PAGE_CATEGORY_LIST"].ToString());

                tmpProds = "";
                if (Convert.ToInt32(userid) > 0)
                {
                    foreach (DataRow drpid in dscat.Tables["FamilyPro"].Rows)
                    {
                        tmpProds = tmpProds + drpid["PRODUCT_ID"].ToString() + ",";
                    }
                    if (tmpProds != string.Empty)
                    {
                        tmpProds = tmpProds.Substring(0, tmpProds.Length - 1);
                        //dsPriceTableAll = objHelperDB.GetProductPriceTableAll(tmpProds, Convert.ToInt32(userid));
                    }
                }

                bool IsEcomEnabled = objHelperServices.GetIsEcomEnabled(userid);
                lstrecords = new TBWDataList[dscat.Tables["FamilyPro"].Rows.Count + 1];
                string CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(userid));
                string cellGV = string.Empty;
                string cellLV = string.Empty;
                cellGV = "CategoryProductList\\productlist_GridView";
                cellLV = "CategoryProductList\\productlist_WES";

                int icnt = 0;
                foreach (DataRow drpid in dscat.Tables["FamilyPro"].Rows)
                {
                    oe++;
                    icnt++;
                    if ((oe % 2) == 0)
                    {
                        soddevenrow = "even";
                    }
                    else
                    {
                        soddevenrow = "odd";
                    }

                    if (_ViewType == "GV")
                        _stmpl_records = _stg_records.GetInstanceOf(cellGV);
                    else
                        _stmpl_records = _stg_records.GetInstanceOf(cellLV + soddevenrow);

                    //string urlDesc = "";

                    _stmpl_records.SetAttribute("FAMILY_ID", drpid["FAMILY_ID"]);
                    _stmpl_records.SetAttribute("CATEGORY_ID", drpid["CATEGORY_ID"]);
                    _stmpl_records.SetAttribute("PARENT_CATEGORY_ID", drpid["PARENT_CATEGORY_ID"]);

                    _stmpl_records.SetAttribute("FAMILY_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath)));

                    _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath + "////UserSearch1=Family Id=" + drpid["FAMILY_ID"].ToString())));

                    _stmpl_records.SetAttribute("TBT_PRODUCT_ID", drpid["PRODUCT_ID"]);
                    _stmpl_records.SetAttribute("PRODUCT_CODE", drpid["PRODUCT_CODE"].ToString());
                    _stmpl_records.SetAttribute("TBT_FAMILY_ID", drpid["FAMILY_ID"]);
                    _stmpl_records.SetAttribute("divcount", drpid["PRODUCT_ID"] + "_" + icnt); //div count for grid loads popup display issue resolved

                    trmpstr = drpid["FAMILY_NAME"].ToString().Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
                    if (_ViewType == "GV")
                    {

                        if (trmpstr.Length > 60)
                            trmpstr = trmpstr.Substring(0, 60) + "...";
                    }

                    _stmpl_records.SetAttribute("FAMILY_NAME", trmpstr);

                    _stmpl_records.SetAttribute("FAMILY_PRODUCT_COUNT", drpid["FAMILY_PRODUCT_COUNT"]);
                    _stmpl_records.SetAttribute("PRODUCT_COUNT", drpid["PRODUCT_COUNT"]);

                    _stmpl_records.SetAttribute("TBT_ECOMENABLED", IsEcomEnabled);
                    if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
                        _stmpl_records.SetAttribute("TBT_WITH_LOGIN", true);
                    //  string ValueFortag = string.Empty;
                    //ValueFortag = "<div id=\"pid" + dscat.Tables[2].Rows[0]["PRODUCT_ID"].ToString() + "\" style=\"background-color:#ffffff;visibility:hidden;position:absolute\">" + AssemblePriceTable(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus) + "</div><div onMouseOver=\"javascript:ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onMouseOut=\"javascript:ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" style=\"position:relative\">" + ValueFortag + "</div>";
                    else
                        _stmpl_records.SetAttribute("TBT_WITH_LOGIN", false);

<<<<<<< .mine
                    //if (HttpContext.Current.Session["EBAY_BLOCK"] != null && HttpContext.Current.Session["EBAY_BLOCK"].ToString() == "True" && drpid["EBAY_BLOCK"] != null && drpid["EBAY_BLOCK"].ToString() == "True")
                    //{
                    //    _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", true);
                    //    _stmpl_records.SetAttribute("TBT_SUB_FAMILY", false);
                    //    _stmpl_records.SetAttribute("TBT_HIDE_BUY", true);
                    //    _stmpl_records.SetAttribute("PRODUCT_STATUS", "UNAVAILABLE");
                    //    if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
                    //        _stmpl_records.SetAttribute("TBT_WITH_LOGIN", true);
                    //    else
                    //        _stmpl_records.SetAttribute("TBT_WITH_LOGIN", false);


                    //    _stmpl_records.SetAttribute("TBT_USER_PRICE", drpid["PRODUCT_PRICE"]);

                    //    if (Convert.ToInt32(drpid["QTY_AVAIL"].ToString()) != -1)
                    //    {
                    //        if (Convert.ToInt32(drpid["QTY_AVAIL"].ToString()) > 0)
                    //        {
                    //            _stmpl_records.SetAttribute("TBT_QTY_AVAIL", drpid["QTY_AVAIL"]);
                    //            _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", drpid["MIN_ORD_QTY"]);
                    //        }
                    //        else
                    //        {
                    //            _stmpl_records.SetAttribute("TB_DISPLAY", "none");
                    //        }

                    //    }
                    //}
                     if (Convert.ToInt32(drpid["PRODUCT_COUNT"].ToString()) > 1)
||||||| .r594
                    if (Convert.ToInt32(drpid["PRODUCT_COUNT"].ToString()) > 1)
=======
                    if (HttpContext.Current.Session["EBAY_BLOCK"] != null && HttpContext.Current.Session["EBAY_BLOCK"].ToString() == "True" && drpid["EBAY_BLOCK"] != null && drpid["EBAY_BLOCK"].ToString() == "True")
>>>>>>> .r640
                    {
<<<<<<< .mine
                        BindToST = objHelperDB.CheckFamily_Discontinued(drpid["FAMILY_ID"].ToString());
||||||| .r594
=======
                        _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", true);
                        _stmpl_records.SetAttribute("TBT_SUB_FAMILY", false);
                        _stmpl_records.SetAttribute("TBT_HIDE_BUY", true);
                        _stmpl_records.SetAttribute("PRODUCT_STATUS", "UNAVAILABLE");
                        if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
                            _stmpl_records.SetAttribute("TBT_WITH_LOGIN", true);
                        else
                            _stmpl_records.SetAttribute("TBT_WITH_LOGIN", false);


                        _stmpl_records.SetAttribute("TBT_USER_PRICE", drpid["PRODUCT_PRICE"]);

                        if (Convert.ToInt32(drpid["QTY_AVAIL"].ToString()) != -1)
                        {
                            if (Convert.ToInt32(drpid["QTY_AVAIL"].ToString()) > 0)
                            {
                                _stmpl_records.SetAttribute("TBT_QTY_AVAIL", drpid["QTY_AVAIL"]);
                                _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", drpid["MIN_ORD_QTY"]);
                            }
                            else
                            {
                                _stmpl_records.SetAttribute("TB_DISPLAY", "none");
                            }

                        }
                    }
                    else if (Convert.ToInt32(drpid["PRODUCT_COUNT"].ToString()) > 1)
                    {
>>>>>>> .r640
                        _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", false);
                        _stmpl_records.SetAttribute("TBT_SUB_FAMILY", true);
                        _stmpl_records.SetAttribute("TBT_MIN_PRICE", drpid["MIN_PRICE"]);
                    }
                    else
                    {
                        _stmpl_records.SetAttribute("TBT_SUB_FAMILY", false);
                        _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", true);

                        string stk_sta_desc = "";
                        stk_sta_desc = drpid["STOCK_STATUS_DESC"].ToString().Trim();
                        //   objErrorHandler.CreateLog(stk_sta_desc);
                        BindToST = true;
                        if ((stk_sta_desc == "DISCONTINUED NO LONGER AVAILABLE" || stk_sta_desc == "TEMPORARY UNAVAILABLE NO ETA" || stk_sta_desc == "TEMPORARY_UNAVAILABLE NO ETA"))
                        {
                            if (stk_sta_desc == "DISCONTINUED NO LONGER AVAILABLE")
                            {
                                _stmpl_records.SetAttribute("PRODUCT_STATUS", "Product No Longer Available.</br> Please contact Us");
                                BindToST = false;
                            }
                            else if (stk_sta_desc == "TEMPORARY UNAVAILABLE NO ETA" || stk_sta_desc == "TEMPORARY_UNAVAILABLE NO ETA")
                            {
                                _stmpl_records.SetAttribute("PRODUCT_STATUS", "Product Temporarily Unavailable.<br/>Please Contact Us for more details");
                            }
                            if (drpid["PROD_SUBSTITUTE"].ToString().Trim() != "" &&  drpid["PROD_STOCK_FLAG"].ToString().Trim() == "-2")
                            {
                                //DataTable rtntbl = objProductServices.GetSubstituteProductDetails(drpid["PROD_SUBSTITUTE"].ToString().Trim(), Convert.ToInt32(userid));
                                DataTable rtntbl = objProductServices.GetSubstituteProductDetails(drpid["PRODUCT_CODE"].ToString(), Convert.ToInt32(userid));
                                if (rtntbl != null && rtntbl.Rows.Count > 0)
                                {

                                    bool samecodesubproduct = (bool)rtntbl.Rows[0]["samecodesubproduct"];

                                    bool samecodenotFound = (bool)rtntbl.Rows[0]["samecodenotFound"];

                                    if (samecodenotFound == false && rtntbl.Rows[0]["ea_path"].ToString() != "")
                                    {
                                        if (stk_sta_desc.ToUpper() == "DISCONTINUED NO LONGER AVAILABLE")
                                        {

                                            DataSet Sqltbs = objHelperDB.GetProductPriceEA("", rtntbl.Rows[0]["SubstuyutePid"].ToString(), userid);
                                            if (Sqltbs != null)
                                            {

                                                string stockstaus = Sqltbs.Tables[0].Rows[0]["PROD_STK_STATUS_DSC"].ToString().Trim().ToUpper();
                                                //  objErrorHandler.CreateLog("Sqltbs" + stockstaus);
                                                if ((stockstaus == "DISCONTINUED NO LONGER AVAILABLE"))
                                                {

                                                    BindToST = false;
                                                }
                                                else
                                                {
                                                    BindToST = true;
                                                }
                                            }
                                        }
                                        _stmpl_records.SetAttribute("TBT_HIDE_BUY", false);
                                        _stmpl_records.SetAttribute("TBT_SUB_PRODUCT", true);
                                        _stmpl_records.SetAttribute("TBT_REP_NIL_CODE", rtntbl.Rows[0]["wag_product_code"].ToString());
                                        string strurl = "ProductDetails.aspx?Pid=" + rtntbl.Rows[0]["SubstuyutePid"].ToString() + "&amp;fid=" + rtntbl.Rows[0]["Pfid"].ToString() + "&amp;Cid=" + rtntbl.Rows[0]["CatId"].ToString() + "&amp;path=" + rtntbl.Rows[0]["ea_path"].ToString();
                                        _stmpl_records.SetAttribute("TBT_REP_EA_PATH", strurl);
                                        BindToST = true;
                                    }
                                    else
                                    {
                                        _stmpl_records.SetAttribute("TBT_HIDE_BUY", true);

                                    }
                                }
                                else
                                {
                                    _stmpl_records.SetAttribute("TBT_HIDE_BUY", true);
                                }
                            }
                            else
                            {
                                _stmpl_records.SetAttribute("TBT_HIDE_BUY", true);

                            }

                        }
                        else
                        {
                            _stmpl_records.SetAttribute("TBT_HIDE_BUY", false);
                            if ((stk_sta_desc.ToUpper().Contains("OUT_OF_STOCK") == true || stk_sta_desc.ToUpper().Contains("SPECIAL_ORDER") == true ) && drpid["PROD_SUBSTITUTE"].ToString().Trim() != "" && drpid["PROD_STOCK_FLAG"].ToString().Trim() == "-2")
                            {
                                //DataTable rtntbl = objProductServices.GetSubstituteProductDetails(drpid["PROD_SUBSTITUTE"].ToString().Trim(), Convert.ToInt32(userid));
                                DataTable rtntbl = objProductServices.GetSubstituteProductDetails(drpid["PRODUCT_CODE"].ToString(), Convert.ToInt32(userid));
                                if (rtntbl != null && rtntbl.Rows.Count > 0)
                                {

                                    bool samecodesubproduct = (bool)rtntbl.Rows[0]["samecodesubproduct"];

                                    bool samecodenotFound = (bool)rtntbl.Rows[0]["samecodenotFound"];
                                   // objErrorHandler.CreateLog("OUT_OF_STOCK" + samecodenotFound + "--" + rtntbl.Rows[0]["ea_path"].ToString());
                                    if (samecodenotFound == false && rtntbl.Rows[0]["ea_path"].ToString() != "")
                                    {

                                        _stmpl_records.SetAttribute("TBT_SUB_PRODUCT", true);
                                        _stmpl_records.SetAttribute("TBT_REP_NIL_CODE", rtntbl.Rows[0]["wag_product_code"].ToString());
                                        string strurl = "ProductDetails.aspx?Pid=" + rtntbl.Rows[0]["SubstuyutePid"].ToString() + "&amp;fid=" + rtntbl.Rows[0]["Pfid"].ToString() + "&amp;Cid=" + rtntbl.Rows[0]["CatId"].ToString() + "&amp;path=" + rtntbl.Rows[0]["ea_path"].ToString();
                                        _stmpl_records.SetAttribute("TBT_REP_EA_PATH", strurl);
                                    }
                                }

                            }
                            else if (HttpContext.Current.Session["EBAY_BLOCK"] != null && HttpContext.Current.Session["EBAY_BLOCK"].ToString() == "True" && drpid["EBAY_BLOCK"] != null && drpid["EBAY_BLOCK"].ToString() == "True")
                            {
                                _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", true);

                                _stmpl_records.SetAttribute("TBT_HIDE_BUY", true);
                                _stmpl_records.SetAttribute("PRODUCT_STATUS", "UNAVAILABLE");

                            } 
                        }

                        //if (dsBgDisc != null)
                        //{
                        //    if (dsBgDisc.Tables[0].Rows.Count > 0)
                        //    {
                        //        decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                        //        DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                        //        string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                        //        //untPrice = objHelperServices.CDEC(DsPreview.Tables[_familyID].Rows[i][j].ToString());
                        //        bool IsBGCatProd = objFamilyServices.IsBGCatalogProduct(Convert.ToInt32(WesCatalogId), _Buyer_Group);
                        //        if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
                        //        {
                        //            //ValueFortag = objFamilyServices.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth).ToString();

                        //        }
                        //    }
                        //}

                        //  commented for product price table
                        //if (Convert.ToInt32(userid) > 0)
                        //{

                        //    PriceTable = objFamilyServices.AssemblePriceTable(objHelperServices.CI(drpid["PRODUCT_ID"].ToString()), pricecode, drpid["PRODUCT_CODE"].ToString(), drpid["STOCK_STATUS_DESC"].ToString(), drpid["PROD_STOCK_STATUS"].ToString(), CustomerType, Convert.ToInt32(userid), drpid["PROD_STOCK_FLAG"].ToString(), drpid["ETA"].ToString(), dsPriceTableAll);
                        //}
                        //_stmpl_records.SetAttribute("TBT_PRODUCT_PRICE_TABLE", PriceTable);


                        // _stmpl_records.SetAttribute("TBT_ECOMENABLED", (Convert.ToInt16(HttpContext.Current.Session["USER_ROLE"]) < 4 ? true : false));

                        if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
                            _stmpl_records.SetAttribute("TBT_WITH_LOGIN", true);
                        //  string ValueFortag = string.Empty;
                        //ValueFortag = "<div id=\"pid" + dscat.Tables[2].Rows[0]["PRODUCT_ID"].ToString() + "\" style=\"background-color:#ffffff;visibility:hidden;position:absolute\">" + AssemblePriceTable(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus) + "</div><div onMouseOver=\"javascript:ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onMouseOut=\"javascript:ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" style=\"position:relative\">" + ValueFortag + "</div>";
                        else
                            _stmpl_records.SetAttribute("TBT_WITH_LOGIN", false);




                        _stmpl_records.SetAttribute("TBT_USER_PRICE", drpid["PRODUCT_PRICE"]);

                        if (Convert.ToInt32(drpid["QTY_AVAIL"].ToString()) != -1)
                        {
                            if (Convert.ToInt32(drpid["QTY_AVAIL"].ToString()) > 0)
                            {
                                _stmpl_records.SetAttribute("TBT_QTY_AVAIL", drpid["QTY_AVAIL"]);
                                _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", drpid["MIN_ORD_QTY"]);
                            }
                            else
                            {
                                _stmpl_records.SetAttribute("TB_DISPLAY", "none");
                            }

                        }


                        //DataRow[] drow = dscat.Tables[2].Select("PRODUCT_ID='" + drpid["PRODUCT_ID"].ToString() + "' AND ATTRIBUTE_TYPE=0");
                        //if (drow.Length > 0) // Data Rows must return 1 row 
                        //{
                        //    DataTable td = drow.CopyToDataTable();
                        //    _stmpl_records.SetAttribute("TBT_USER_PRICE", td.Rows[0]["NUMERIC_VALUE"].ToString());

                        //    if (Convert.ToInt32(td.Rows[0]["QTY_AVAIL"].ToString()) != -1)
                        //    {
                        //        if (Convert.ToInt32(td.Rows[0]["QTY_AVAIL"].ToString()) > 0)
                        //        {
                        //            _stmpl_records.SetAttribute("TBT_QTY_AVAIL", td.Rows[0]["QTY_AVAIL"].ToString());
                        //            _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", td.Rows[0]["MIN_ORD_QTY"].ToString());
                        //        }
                        //        else
                        //        {
                        //            _stmpl_records.SetAttribute("TB_DISPLAY", "none");
                        //        }
                        //        //_stmpl_records.SetAttribute("TBT_PRODUCT_ID", td.Rows[0]["PRODUCT_ID"].ToString());
                        //    }
                        //}



                    }


                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_1", drpid["PRODUCT_CODE"]);

                    //System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + drpid["Prod_Thumbnail"].ToString().Replace("/", "\\"));
                    //if (Fil.Exists)
                    //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", drpid["Prod_Thumbnail"]);
                    //else
                    //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", "/images/noimage.gif");

                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", drpid["Prod_Thumbnail"]);

                    string desc = string.Empty;
                    string descattr = string.Empty;
                    string prod_desc_alt = string.Empty;

                    if (_ViewType == "LV")
                    {
                        desc = drpid["Family_ShortDescription"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");

                        prod_desc_alt = drpid["Prod_Description"].ToString();
                        if (prod_desc_alt.Length > 0)
                            _stmpl_records.SetAttribute("PROD_DESC_ALT", prod_desc_alt);
                        else
                            _stmpl_records.SetAttribute("PROD_DESC_ALT", drpid["FAMILY_NAME"]);

                        if (desc.Length > 230 && _ViewType == "LV")
                        {
                            _stmpl_records.SetAttribute("TBT_MORE_13", true);
                            desc = desc.Substring(0, 230).ToString();
                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_13", desc);
                        }
                        else
                        {
                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_13", desc);
                            _stmpl_records.SetAttribute("TBT_MORE_13", false);
                        }
                        desc = drpid["Family_Description"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");

                        if (desc.Length > 230 && _ViewType == "LV")
                        {
                            _stmpl_records.SetAttribute("TBT_MORE_90", true);
                            desc = desc.Substring(0, 230).ToString();
                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_90", desc);
                        }
                        else
                        {
                            _stmpl_records.SetAttribute("ATTRIBUTE_ID_90", desc);
                            _stmpl_records.SetAttribute("TBT_MORE_90", false);
                        }


                    }
                    else
                    {
                        // descattr = drpid["Family_ShortDescription"].ToString() + " " + drpid["Family_Description"].ToString() + " " + drpid["Prod_Description"].ToString();

                        descattr = drpid["Family_ShortDescription"].ToString();
                        if (descattr.Length > 0)
                            descattr = descattr + "<br/>";
                        descattr = descattr + drpid["Family_Description"] + " " + drpid["Prod_Description"];

                        prod_desc_alt = drpid["Prod_Description"].ToString();
                        if (prod_desc_alt.Length > 0)
                            _stmpl_records.SetAttribute("PROD_DESC_ALT", prod_desc_alt);
                        else
                            _stmpl_records.SetAttribute("PROD_DESC_ALT", drpid["FAMILY_NAME"]);

                        if (descattr.Length > 140 && _ViewType == "GV")
                        {
                            int count = 0;
                            count = descattr.Count(c => char.IsUpper(c));
                            int count1 = 0;
                            count1 = descattr.Count(c => char.IsSymbol(c)) + descattr.Count(c => char.IsNumber(c));
                            if (count >= 35)
                            {
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                                descattr = descattr.Substring(0, 120).ToString();
                                descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                            }
                            else if (descattr.Length > 140 && count < 35 && count1 > 10)
                            {
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                                descattr = descattr.Substring(0, 135).ToString();
                                descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                            }
                            else if (descattr.Length > 140 && count < 35 && count1 < 10)
                            {
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                                descattr = descattr.Substring(0, 140).ToString();
                                descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                                _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);

                            }

                        }
                        else
                        {
                            _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                        }
                    }
                
                    //dsprodspecs = new DataSet();
                    //DataRow[] drow1 = dscat.Tables[2].Select("PRODUCT_ID='" + drpid["PRODUCT_ID"].ToString() + "' AND (ATTRIBUTE_TYPE=1 OR ATTRIBUTE_TYPE=4 OR ATTRIBUTE_TYPE=3) And FAMILY_ID='" + drpid["FAMILY_ID"].ToString() + "'");
                    //if (drow1.Length > 0)
                    //    dsprodspecs.Tables.Add(drow1.CopyToDataTable());
                    //else
                    //    dsprodspecs = null;



                    //if (dsprodspecs != null && dsprodspecs.Tables.Count >= 1 && dsprodspecs.Tables[0].Rows.Count >= 1)
                    //{
                    //    foreach (DataRow dr in dsprodspecs.Tables[0].Rows)
                    //    {
                    //        if (dr["ATTRIBUTE_TYPE"].ToString() == "1")
                    //        {
                    //            if (dr["ATTRIBUTE_ID"].ToString() == "1")
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), objHelperServices.StringTrim(dr["STRING_VALUE"].ToString(), 16, true));
                    //            else
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString());
                    //        }
                    //        else if (dr["ATTRIBUTE_TYPE"].ToString() == "4")
                    //        {
                    //            if (dr["NUMERIC_VALUE"].ToString().Length > 0)
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), Math.Round(Convert.ToDecimal(dr["NUMERIC_VALUE"]), 2).ToString());
                    //        }
                    //        else if (dr["ATTRIBUTE_TYPE"].ToString() == "3")
                    //        {
                    //            System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dr["STRING_VALUE"].ToString().Replace("/", "\\"));
                    //            if (Fil.Exists)
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));
                    //            else
                    //            {
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), "/images/noimage.gif");
                    //            }

                    //        }
                    //        else
                    //        {
                    //            _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString());
                    //        }

                    //    }

                    //}
                    //dsprodspecs = new DataSet();
                    //drow1 = dscat.Tables[2].Select("PRODUCT_ID='" + drpid["PRODUCT_ID"].ToString() + "' AND (ATTRIBUTE_TYPE=7 OR ATTRIBUTE_TYPE=9) And FAMILY_ID='" + drpid["FAMILY_ID"].ToString() + "'");
                    //if (drow1.Length > 0)
                    //    dsprodspecs.Tables.Add(drow1.CopyToDataTable());
                    //else
                    //    dsprodspecs = null;

                    //if (dsprodspecs != null && dsprodspecs.Tables.Count >= 1 && dsprodspecs.Tables[0].Rows.Count >= 1)
                    //{
                    //    string desc = "";
                    //    string descattr = "";
                    //    foreach (DataRow dr in dsprodspecs.Tables[0].Rows)
                    //    {

                    //        if (dr["ATTRIBUTE_TYPE"].ToString() == "9")
                    //        {
                    //            System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dr["STRING_VALUE"].ToString());
                    //            if (Fil.Exists)
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));
                    //            else
                    //            {
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), "/images/noimage.gif");
                    //            }

                    //        }
                    //        else if (dr["ATTRIBUTE_TYPE"].ToString() == "7")
                    //        {
                    //            //string desc = dr["STRING_VALUE"].ToString().Replace("\r", "&nbsp;").Replace("\n", "<br/>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                    //            //string desc = dr["STRING_VALUE"].ToString().Replace("\r", "&nbsp;");
                    //            desc = dr["STRING_VALUE"].ToString().Replace("\r", "").Replace("\n\r", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                    //            if (dr["ATTRIBUTE_ID"].ToString() == "13" && desc.Length > 0)
                    //                desc = desc + "<br/>";
                    //            descattr = descattr + desc;
                    //            if (desc.Length > 230 && _ViewType == "LV")
                    //            {
                    //                _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
                    //                desc = desc.Substring(0, 230).ToString();
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
                    //            }
                    //            //else if (desc.Length > 30 && _ViewType == "GV")
                    //            //{
                    //            //    _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
                    //            //    desc = desc.Substring(0, 30).ToString();
                    //            //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
                    //            //}
                    //            else
                    //            {
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
                    //                _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), false);
                    //            }
                    //        }
                    //        else
                    //        {

                    //            _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\r", "&nbsp;").Replace("\n", "<br/>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>"));
                    //        }

                    //    }
                    //    if (descattr.Length > 140 && _ViewType == "GV")
                    //    {
                    //        int count = 0;
                    //        count = descattr.Count(c => char.IsUpper(c));
                    //        if (count >= 35)
                    //        {
                    //            _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                    //            descattr = descattr.Substring(0, 120).ToString();
                    //            descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                    //            _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                    //        }
                    //        else if (descattr.Length > 140 && count < 35)
                    //        {
                    //            _stmpl_records.SetAttribute("ATTRIBUTEDESC_GV", true);
                    //            descattr = descattr.Substring(0, 140).ToString();
                    //            descattr = descattr.Substring(0, descattr.LastIndexOf(" "));
                    //            _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                    //        }

                    //    }
                    //    else
                    //        _stmpl_records.SetAttribute("ATTRIBUTEDESC", descattr);
                    //}


                    if (BindToST == true)
                    {
                        lstrows[icolstart] = new TBWDataList(_stmpl_records.ToString());
                    }

                    icolstart++;
                    if (icolstart >= icol || oe == dscat.Tables["FamilyPro"].Rows.Count)
                    {
                        _stg_container = new StringTemplateGroup("CategoryProductListcontainer", stemplatepath);
                        _stmpl_container = _stg_container.GetInstanceOf("CategoryProductList\\producttable_" + soddevenrow);
                        _stmpl_container.SetAttribute("TBWDataList", lstrows);
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
                        sHTML = sHTML + _stmpl_container.ToString();
                        ictrecords++;
                        icolstart = 0;
                        lstrows = new TBWDataList[icol];

                    }
                }

                //   }

                _stg_container = new StringTemplateGroup("CategoryProductListmain", stemplatepath);

                if (HttpContext.Current.Request.QueryString["ViewMode"] != null)
                {
                    PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                    isreadonly.SetValue(HttpContext.Current.Request.QueryString, false, null);
                    HttpContext.Current.Request.QueryString.Remove("ViewMode");
                }

                string productlistURL = HttpContext.Current.Request.QueryString.ToString();
                string powersearchURListView = string.Empty;
                string powersearchURLGridView = string.Empty;
                powersearchURListView = productlistURL + "&ViewMode=LV";
                powersearchURLGridView = productlistURL + "&ViewMode=GV";




            }
        }
        catch (Exception ex)
        {
            objErrorHandler.CreateLog(ex.ToString());
            sHTML = ex.Message;
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
        finally
        {

        }

        //return objHelperServices.StripWhitespace(sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>"));
        return sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
    }






    protected DataSet Get_Value_Breadcrum(int ipageno, string eapath, string irecords)
    {

        string sHTML = string.Empty;
       // string sBrandAndModelHTML = "";
       // string sModelListHTML = "";
        DataSet dscat = new DataSet();
        EasyAsk_WES EasyAsk = new EasyAsk_WES();
        try
        {


            string _catCid = string.Empty;
            string _parentCatID = string.Empty;
            //int ictrows = 0;
            string _tsb = string.Empty;
            string _tsm = string.Empty;
            string _type = string.Empty;
            string _value = string.Empty;
            string _bname = string.Empty;
            string _searchstr = string.Empty;
            string url = string.Empty;
           // string _byp = "2";
            string _bypcat = null;
            string _pid = string.Empty;
            string _fid = string.Empty;
            string _seeall = string.Empty;
            _bypcat = HttpContext.Current.Request.QueryString["bypcat"];

            if (HttpContext.Current.Request.QueryString["tsm"] != null && HttpContext.Current.Request.QueryString["tsm"].ToString() != "")
                _tsm = HttpContext.Current.Request.QueryString["tsm"];

            if (HttpContext.Current.Request.QueryString["tsb"] != null && HttpContext.Current.Request.QueryString["tsb"].ToString() != "")
                _tsb = HttpContext.Current.Request.QueryString["tsb"];

            if (HttpContext.Current.Request.QueryString["type"] != null)
                _type = HttpContext.Current.Request.QueryString["type"];

            if (HttpContext.Current.Request.QueryString["value"] != null)
                _value = HttpContext.Current.Request.QueryString["value"];

            if (HttpContext.Current.Request.QueryString["bname"] != null)
                _bname = HttpContext.Current.Request.QueryString["bname"];
            if (HttpContext.Current.Request.QueryString["searchstr"] != null)
                _searchstr = HttpContext.Current.Request.QueryString["searchstr"];
            if (HttpContext.Current.Request.QueryString["srctext"] != null)
                _searchstr = HttpContext.Current.Request.QueryString["srctext"];

            if (HttpContext.Current.Request.QueryString["fid"] != null)
                _fid = HttpContext.Current.Request.QueryString["fid"];
            if (HttpContext.Current.Request.QueryString["pid"] != null)
                _pid = HttpContext.Current.Request.QueryString["pid"];

            if (HttpContext.Current.Request.QueryString["seeall"] != null)
                _seeall = HttpContext.Current.Request.QueryString["seeall"];
            if (HttpContext.Current.Request.QueryString["cid"] != null)
                _cid = HttpContext.Current.Request.QueryString["cid"];


            if (_catCid != string.Empty)
                _parentCatID = GetParentCatID(_catCid);
          
            url = HttpContext.Current.Request.Url.OriginalString.ToLower();
           
            string EA = string.Empty;
            EA = eapath;
           
            //if (HttpContext.Current.Session["MainCategory"] != null)
            //{
            //    DataRow[] dr = ((DataSet)HttpContext.Current.Session["MainCategory"]).Tables[0].Select("CATEGORY_ID='" + _parentCatID + "'");
            //    if (dr.Length > 0)
            //        _byp = dr[0]["CUSTOM_NUM_FIELD3"].ToString();
            //}


            iPageNo = ipageno;

            if ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("categorylist.aspx")))
            {
                if (_bypcat == null)
                {

                    EasyAsk.GetMainMenuClickDetail(_cid, "");


                    string CatName = string.Empty;
                    DataTable tmptbl = null;
                    //if (HttpContext.Current.Session["MainMenuClick"] != null)
                    //{
                    //    tmptbl = ((DataSet)HttpContext.Current.Session["MainCategory"]).Tables[0];
                    tmptbl = EasyAsk.GetCategoryAndBrand("MainCategory").Tables[0];
                        tmptbl = tmptbl.Select("CATEGORY_ID='" + _cid + "'").CopyToDataTable();

                        if (tmptbl != null && tmptbl.Rows.Count > 0)
                        {
                            CatName = tmptbl.Rows[0]["CATEGORY_NAME"].ToString();
                        }


                    //}


                    //if (HttpContext.Current.Session["RECORDS_PER_PAGE_CATEGORY_LIST"] != null)
                    //    iRecordsPerPage = Convert.ToInt32(HttpContext.Current.Session["RECORDS_PER_PAGE_CATEGORY_LIST"].ToString());

                   dscat= EasyAsk.GetAttributeProducts("CategoryProductList", "", "Category", CatName, _bname,irecords, (iPageNo - 1).ToString(), "Next",EA);

                }
                //else if (_tsb != "")
                //{
                //    string parentCatName = GetCName(_cid);
                //  dscat=  EasyAsk.GetWESModel(parentCatName, iCatalogId, _tsb);
                //}

            }

            //end
        }

        catch (Exception ex)
        {
            sHTML = ex.Message;
        }




        return dscat;
    }

     
}
