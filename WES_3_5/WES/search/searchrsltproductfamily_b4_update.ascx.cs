﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
using TradingBell.WebCat.CatalogDB;

using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using System.Reflection;
public partial class search_searchrsltproductfamily : System.Web.UI.UserControl
{
    HelperServices objHelperServices = new HelperServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    CategoryServices objCategoryServices = new CategoryServices();
    Security objSecurity = new Security();
    UserServices objUserServices = new UserServices(); 
    HelperDB objHelperDB = new HelperDB();
    //SqlConnection oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["TBWebCatShoppingCartConnString"].ToString());
    DataSet dscat = new DataSet();
    //ProductFamily oPF = new ProductFamily();
    ProductServices objProductServices = new ProductServices();
    ConnectionDB objConnectionDB = new ConnectionDB();
    int iCatalogId;
    int iInventoryLevelCheck;
    int iRecordsPerPage=16;
    bool bIsStartOver = true;
    string sSortBy = string.Empty;
    bool bDoPaging;
    int iPageNo = 1;
    bool bSortAsc = true;
    int iTotalPages = 0;
    int iTotalProducts = 0;
    int iTmpProductId = 0;
    int iPrevPgNo = 1;
    int iNextPgNo = 1;
    string stemplatepath = string.Empty;
    //string catID = "";
    string _tsb = string.Empty;
    string _tsm = string.Empty;
    string _type = string.Empty;
    string _value = string.Empty;
    string _bname = string.Empty;
    string _searchstr = string.Empty;
    string _byp = "2";
    string _bypcat = null;
    string _pid = string.Empty;
    string _fid = string.Empty;
    string _cid = string.Empty;
    string _pcr = string.Empty;
    string _EAPath = string.Empty;
    string _ParentCatID = string.Empty;
    public string WesNewsCategoryId = System.Configuration.ConfigurationManager.AppSettings["WESNEWS_CATEGORY_ID"].ToString();
    public string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();

    string strPDFFiles1 = HttpContext.Current.Server.MapPath("attachments");
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //added string template path M/A

            stemplatepath = Server.MapPath(ConfigurationManager.AppSettings["StringTemplatePath"].ToString());
            try
            {
                string requrl = Request.Url.OriginalString.ToString().ToUpper();
                if (requrl.Contains("PRODUCT_LIST.ASPX"))
                    stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\product_list\\";
                if (requrl.Contains("BYBRAND.ASPX"))
                    stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\bybrand\\";
                if (requrl.Contains("BYPRODUCT.ASPX"))
                    stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\byproduct\\";
            }
            catch (Exception ec)
            {
            }
            if (IsPostBack)
            {

            }
            else
            {
                if (Request.QueryString["pgno"] != null)
                {
                    iPageNo = Convert.ToInt32(Request.QueryString["pgno"]);
                }
            }
            GetStoreConfig();
            GetPageConfig();

        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }

    private void GetStoreConfig()
    {
        iCatalogId = Convert.ToInt32(Session["CATALOG_ID"].ToString());
        iInventoryLevelCheck = Convert.ToInt32(Session["INVENTORY_LEVEL_CHECK"].ToString());
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
            bDoPaging = Convert.ToBoolean(Session["DO_PAGING"].ToString());
            if (HidItemPage.Value.ToString() == string.Empty || HidItemPage.Value.ToString() == null)
            {
                if(Session["RECORDS_PER_PAGE_PRODUCT_LIST"]!=null)
                 iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_PRODUCT_LIST"].ToString());
            }
            else
            {
                iRecordsPerPage = Convert.ToInt32(HidItemPage.Value.ToString());
                Session["RECORDS_PER_PAGE_PRODUCT_LIST"] = HidItemPage.Value.ToString();
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
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

    public string ST_ProductList()
    {
      
        FamilyServices objFamilyServices = new FamilyServices();
        StringTemplateGroup _stg_container = null;
        StringTemplateGroup _stg_records = null;
        StringTemplate _stmpl_container = null;
        StringTemplate _stmpl_records = null;
        //StringTemplate _stmpl_pages = null;
        DataSet dsprod = new DataSet();
        DataSet dsprodspecs = new DataSet();
        DataSet subcatds = new DataSet();
        string strFile = HttpContext.Current.Server.MapPath("ProdImages");

        int oe = 0;
        string category_nameh = string.Empty;
        string sHTML = string.Empty;
        string _pcr = string.Empty;
        string _ViewType = string.Empty;
        string lstcarecord = string.Empty;
        string lstprerecord = string.Empty;
        string lstvperrecord = string.Empty;
        DataSet breadcrumb = new DataSet();
        HelperServices objHelperService = new HelperServices();
        //string CID_new = "";
        //string PCID_new = "";
     
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
            {
                _cid = Request.QueryString["cid"];
                Session["SS__cid"] = _cid;
            }
            if (Request.QueryString["pcr"] != null)
            {
                _pcr = Request.QueryString["pcr"];
                Session["SS___pcr"] = _pcr;
            }


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

            if (HttpContext.Current.Session["EA"]!=null)
                _EAPath = HttpContext.Current.Session["EA"].ToString();   


          

           
            if (Request.Url.ToString().ToLower().Contains("product_list.aspx"))
            {
                //ps.ClearFilterproduct("DELETE TBWC_SEARCH_PROD_LIST WHERE PRODUCT_ID IN(SELECT DISTINCT F.FAMILY_ID FROM TB_FAMILY F,TB_FAMILY_SPECS FS WHERE F.FAMILY_ID=FS.FAMILY_ID AND ISNULL(FS.STRING_VALUE,'')='NOT FOR WEB' AND F.CATEGORY_ID='" + ps.CATEGORY_ID + "') AND USER_SESSION_ID='" + Session.SessionID + "'");
                // dscat = ps.GetFamilies();
                dscat = (DataSet)HttpContext.Current.Session["FamilyProduct"];
            }
            else
            {
                // dscat = ps.GetProducts();
            }
          


            if (dscat != null)
            {

                if (Request.QueryString["pcr"] != null)
                    _pcr = Request.QueryString["pcr"];
                if (Request.QueryString["type"] == null)
                {
                    //GetDataSet("SELECT CATEGORY_NAME  FROM	TB_CATEGORY WHERE	CATEGORY_ID ='" + catID + "'").Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
                    string tempstr = (string)objHelperDB.GetGenericDataDB(_cid, "GET_CATEGORY_NAME", HelperDB.ReturnType.RTString);
                    if (tempstr != null && tempstr != "")
                        category_nameh = tempstr;
                }
                else
                    category_nameh = Request.QueryString["value"].ToString();

                DataRow drpagect = dscat.Tables[0].Rows[0];
               // if (drpagect.Table.Rows.Count > 0)
               // {
                    iTotalPages = Convert.ToInt32(drpagect["TOTAL_PAGES"]);
              //  }
                Session["iTotalPages"] = iTotalPages;
                if (iPageNo > iTotalPages)
                {
                    iPageNo = iTotalPages;
                   
                }

                DataRow drproductsct = dscat.Tables[1].Rows[0];
                iTotalProducts = Convert.ToInt32(drproductsct["TOTAL_PRODUCTS"]);
                if (_cid!="")
                _ParentCatID = GetParentCatID(_cid);
               
                if ((iTotalPages <= 0 || iTotalProducts <= 0) && WesNewsCategoryId == _ParentCatID)
                {


                    _stg_container = new StringTemplateGroup("searchrsltproductmain", stemplatepath);
                    _stmpl_container = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "advmain");

                    SetEbookAndPDFLink(_stmpl_container);
                    DataTable tmpDataTable = (DataTable)objHelperDB.GetGenericDataDB(WesCatalogId,"'"+_cid+"'", "GET_CATEGORY", HelperDB.ReturnType.RTTable);
                    if (tmpDataTable != null && tmpDataTable.Rows.Count>0 )
                    {
                        string ebookpath = tmpDataTable.Rows[0]["CUSTOM_TEXT_FIELD3"].ToString();
                                           
                                            if (ebookpath.Contains("www."))
                                            {

                                                ebookpath = ebookpath.ToLower().Replace("attachments", ""). Replace("\\", "").Replace("http://", "").Replace("https://", "");
                                                ebookpath = "http://" + ebookpath;
                                            }
                                            _stmpl_container.SetAttribute("TBT_ADV_URL_LINK", ebookpath);                        
                    }
                    _stmpl_container.SetAttribute("TBW_CATEGORY_NAME", category_nameh);
                   
                    return _stmpl_container.ToString();
                }
                


                TBWDataList[] lstrecords = new TBWDataList[0];
                TBWDataList[] lstrows = new TBWDataList[0];
                _stg_records = new StringTemplateGroup("searchrsltproductrecords", stemplatepath);


                int ictrecords = 0;
                int icolstart = 0;
                string trmpstr="";
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

                DataRow[] drprodcoll = dscat.Tables[2].Select("ATTRIBUTE_ID = 1","SNO");


                string userid = string.Empty;
                string PriceTable = string.Empty;
                int pricecode = 0;
                string tmpProds = string.Empty;
                DataSet dsBgDisc = new DataSet();
                DataSet dsPriceTableAll = new DataSet();
                if (Session["USER_ID"] != null)
                    userid = Session["USER_ID"].ToString();
                if (userid == "")
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

                        if (_ViewType=="GV")
                            _stmpl_records = _stg_records.GetInstanceOf("searchrsltproducts" + "\\" + "productlist_GridView");
                        else
                           _stmpl_records = _stg_records.GetInstanceOf("searchrsltproducts" + "\\" + "productlist_WES" + soddevenrow);

                       // string urlDesc = "";

                        _stmpl_records.SetAttribute("FAMILY_ID", drpid["FAMILY_ID"].ToString());
                        _stmpl_records.SetAttribute("CATEGORY_ID", drpid["CATEGORY_ID"].ToString());
                        _stmpl_records.SetAttribute("PARENT_CATEGORY_ID", drpid["PARENT_CATEGORY_ID"].ToString());

                        _stmpl_records.SetAttribute("FAMILY_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath)));

                        _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath + "////UserSearch1=Family Id=" + drpid["FAMILY_ID"].ToString())));

                        _stmpl_records.SetAttribute("TBT_PRODUCT_ID", drpid["PRODUCT_ID"].ToString());
                        _stmpl_records.SetAttribute("PRODUCT_CODE", drpid["PRODUCT_CODE"].ToString());
                        _stmpl_records.SetAttribute("TBT_FAMILY_ID", drpid["FAMILY_ID"].ToString());
                        trmpstr = drpid["FAMILY_NAME"].ToString().Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
                       if(_ViewType == "GV")
                       {
                         
                           if(trmpstr.Length>60)
                               trmpstr=trmpstr.Substring(0,60)+"...";
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

                            //    PriceTable = objFamilyServices.AssemblePriceTable(objHelperServices.CI(drpid["PRODUCT_ID"].ToString()), pricecode, drpid["PRODUCT_CODE"].ToString(), drpid["STOCK_STATUS_DESC"].ToString(), drpid["PROD_STOCK_STATUS"].ToString(), CustomerType, Convert.ToInt32(userid),  drpid["PROD_STOCK_FLAG"].ToString(),drpid["ETA"].ToString(), dsPriceTableAll);
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
                                   if( dr["ATTRIBUTE_ID"].ToString()=="1")
                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), objHelperServices.StringTrim(dr["STRING_VALUE"].ToString(), 16, true));
                                else
                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(),dr["STRING_VALUE"].ToString());
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
                                    System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dr["STRING_VALUE"].ToString().Replace("/", "\\"));
                                    if (Fil.Exists)
                                        _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));
                                    else
                                    {
                                        _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), "/images/noimage.gif");
                                    }

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
                            string descattr = string.Empty;
                            string desc = string.Empty;
                            foreach (DataRow dr in dsprodspecs.Tables[0].Rows)
                            {

                                if (dr["ATTRIBUTE_TYPE"].ToString() == "9")
                                {
                                    System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dr["STRING_VALUE"].ToString());
                                    if (Fil.Exists)
                                        _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));
                                    else
                                    {
                                        _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), "/images/noimage.gif");
                                    }

                                }
                                else if (dr["ATTRIBUTE_TYPE"].ToString() == "7")
                                {
                                    //string desc = dr["STRING_VALUE"].ToString().Replace("\r", "&nbsp;").Replace("\n", "<br/>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                                   // string desc = dr["STRING_VALUE"].ToString().Replace("\r", "&nbsp;");
                                    desc = dr["STRING_VALUE"].ToString().Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                                    if (dr["ATTRIBUTE_ID"].ToString() == "13" && desc.Length > 0)
                                        desc = desc + "<br/>";

                                   // descattr = descattr +" "+ desc;
                                    descattr = descattr + desc;
                                    if (desc.Length > 250 && _ViewType== "LV")
                                    {
                                        _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
                                        desc = desc.Substring(0, 250).ToString();
                                        _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
                                    }
                                    //else if (desc.Length > 30 && _ViewType == "GV")
                                    //{
                                    //    _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
                                    //    desc = desc.Substring(0,30).ToString();
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

                        //lstrows[icolstart] = new TBWDataList(_stmpl_records.ToString());
                        icolstart++;
                        if (icolstart >= icol || oe==drprodcoll.Length  )
                        {
                            _stg_container = new StringTemplateGroup("searchrsltproductcontainer", stemplatepath);
                            _stmpl_container = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "producttable_" + soddevenrow);
                            _stmpl_container.SetAttribute("TBWDataList", lstrows);
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
                            ictrecords++;
                            icolstart = 0;
                            lstrows = new TBWDataList[icol];
                            
                        }
                    }

             //   }

                _stg_container = new StringTemplateGroup("searchrsltproductmain", stemplatepath);

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

             
                _stmpl_container = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "main");
                _stmpl_container.SetAttribute("TBW_CATEGORY_ID", _cid);
                _stmpl_container.SetAttribute("TBW_PRODUCT_COUNT", iTotalProducts);
                _stmpl_container.SetAttribute("TBW_CATEGORY_NAME", category_nameh);

                
                breadcrumb = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];
                DataRow[] _DCRow = null;

                if (breadcrumb != null && breadcrumb.Tables.Count > 0 && breadcrumb.Tables[0].Rows.Count > 0)
                {
                    if (breadcrumb.Tables[0].Rows[0]["ItemType"].ToString().ToLower().Contains("cablel"))
                    {
                        string c1=breadcrumb.Tables[0].Rows[0]["Itemvalue"].ToString();
                        string c2="anything";
                       if (breadcrumb.Tables[0].Rows.Count>1 && breadcrumb.Tables[0].Rows[1]["ItemType"].ToString().ToLower().Contains("cablelr"))
                            c2=breadcrumb.Tables[0].Rows[1]["Itemvalue"].ToString();

                        _stmpl_container.SetAttribute("TBTC_IMAGE_FILE", "/images/noimage.gif");
                        _stmpl_container.SetAttribute("TBTC_SHORT_DESC_CAT", "Your cable finder results for " + c1 + " to "+ c2);
                        _stmpl_container.SetAttribute("TBTC_CATEGORY_NAME", "Cable Finder Results");
                    }
                    else
                    {
                        _DCRow = breadcrumb.Tables[0].Select("ItemType='Category'");
                        if (_DCRow != null && _DCRow.Length > 0)
                        {
                            foreach (DataRow _drow in _DCRow)
                            {
                                if (_DCRow.Length == 3)
                                {
                                    if (_drow == _DCRow[_DCRow.Length - 1])
                                    {
                                        lstcarecord = _drow["ItemValue"].ToString();
                                    }
                                    if (_drow == _DCRow[_DCRow.Length - 2])
                                    {
                                        lstprerecord = _drow["ItemValue"].ToString();
                                    }

                                    if (_drow == _DCRow[_DCRow.Length - 3])
                                    {
                                        lstvperrecord = _drow["ItemValue"].ToString();
                                    }
                                }
                                else if (_DCRow.Length == 2)
                                {
                                    if (_drow == _DCRow[_DCRow.Length - 1])
                                    {
                                        lstprerecord = _drow["ItemValue"].ToString();
                                    }
                                    if (_DCRow[_DCRow.Length - 2] != null)
                                    {
                                        if (_drow == _DCRow[_DCRow.Length - 2])
                                        {
                                            lstvperrecord = _drow["ItemValue"].ToString();
                                        }
                                    }
                                    else
                                    {
                                        lstvperrecord = "0";
                                    }

                                }
                                else
                                {
                                    if (_drow == _DCRow[_DCRow.Length - 1])
                                    {
                                        lstprerecord = _drow["ItemValue"].ToString();
                                    }
                                    lstvperrecord = "0";


                                }
                            }
                        }
                        if (_DCRow.Length != 1)
                        {
                            if (lstcarecord != null && lstcarecord != "" && lstprerecord != null && lstprerecord != "" && lstvperrecord != null && lstvperrecord != "")
                                subcatds = (DataSet)objHelperDB.GetGenericDataDB("", lstvperrecord, lstprerecord, lstcarecord, "GET_CATEGORY_DETAILS", HelperDB.ReturnType.RTDataSet);
                            else
                                subcatds = (DataSet)objHelperDB.GetGenericDataDB("", lstvperrecord, lstprerecord, lstcarecord, "GET_CATEGORY_DETAILS_BR", HelperDB.ReturnType.RTDataSet);
                        }
                        else
                            subcatds = null;

                        if (subcatds == null)
                            subcatds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, lstprerecord, "0", "GET_CAT_DETAILS", HelperDB.ReturnType.RTDataSet);

                        if (subcatds != null && subcatds.Tables.Count >= 1)
                        {
                            string catdesc = string.Empty;
                            string catename = string.Empty;
                            catdesc = subcatds.Tables[0].Rows[0]["SHORT_DESC"].ToString();
                            catename = subcatds.Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
                            _stmpl_container.SetAttribute("TBTC_SHORT_DESC_CAT", catdesc);
                            _stmpl_container.SetAttribute("TBTC_CATEGORY_NAME", catename);
                            // System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + subcatds.Tables[0].Rows[0]["IMAGE_FILE"].ToString());
                            string imgpath = subcatds.Tables[0].Rows[0]["IMAGE_FILE"].ToString().Replace("\\", "/");

                            if (!(subcatds.Tables[0].Rows[0]["IMAGE_FILE"].ToString().ToLower().Contains("_th")))
                            {
                                imgpath = objHelperService.SetImageFolderPath(imgpath, "_images", "_th");
                            }
                            System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + imgpath);

                            if (Fil.Exists)
                                // _stmpl_container.SetAttribute("TBTC_IMAGE_FILE", subcatds.Tables[0].Rows[0]["IMAGE_FILE"].ToString().Replace("\\", "/"));
                                _stmpl_container.SetAttribute("TBTC_IMAGE_FILE", imgpath);
                            else
                            {
                                _stmpl_container.SetAttribute("TBTC_IMAGE_FILE", "/images/noimage.gif");
                            }

                        }
                        else
                        {
                            _stmpl_container.SetAttribute("TBTC_IMAGE_FILE", "/images/noimage.gif");
                            _stmpl_container.SetAttribute("TBTC_SHORT_DESC_CAT", "");
                            _stmpl_container.SetAttribute("TBTC_CATEGORY_NAME", "");
                        }
                    }
                }
                else
                {
                    _stmpl_container.SetAttribute("TBTC_IMAGE_FILE", "/images/noimage.gif");
                    _stmpl_container.SetAttribute("TBTC_SHORT_DESC_CAT", "");
                    _stmpl_container.SetAttribute("TBTC_CATEGORY_NAME", "");
                }
        
                _stmpl_container.SetAttribute("TBW_URL", powersearchURListView);
                _stmpl_container.SetAttribute("TBW_URL1", powersearchURLGridView);

                if (_ViewType == "LV")
                    _stmpl_container.SetAttribute("TBW_VIEWTYPE", true);
                else
                    _stmpl_container.SetAttribute("TBW_VIEWTYPE", false);

              
                //if (iTotalPages > 1)
                //    _stmpl_container.SetAttribute("TBW_TO_PAGE", true);
                //else
                //    _stmpl_container.SetAttribute("TBW_TO_PAGE", false);

                //_stmpl_container.SetAttribute("SELECT_"+ iRecordsPerPage, "SELECTED=\"SELECTED\" ");
              
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


               // if (WesNewsCategoryId == _ParentCatID) // WES NEW ONLy
              //  {
                    SetEbookAndPDFLink(_stmpl_container);
                   // _stmpl_container.SetAttribute("TBT_DISPLAY_DIV_PAGE", false);
             //   }
                //else
                //{
                //    _stmpl_container.SetAttribute("TBT_DISPLAY_LINK", "none");
                //    _stmpl_container.SetAttribute("TBT_DISPLAY_DIV_PAGE", true);
                //}


              

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
                ////else if ( iPageNo < iTotalPages)
                ////{

                ////}
                //else
                //{
                //    iNextPgNo = 1;
                //    iPrevPgNo = 1;
                //    iPageNo = iTotalPages;
                //}
                //try
                //{
                    
                //    _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");
                //    if (iPageNo > 2 && (iTotalPages >= (iPageNo + 2)))
                //    {
                    
                //        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iPageNo - 2);
                //        SetQueryString(_stmpl_pages);


                //        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());

                //        _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

                //        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iPageNo - 1);
                //        SetQueryString(_stmpl_pages);
                //        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());

                //        _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

                //        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iPageNo);
                //        SetQueryString(_stmpl_pages);
                //        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));

                //        _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");

                //        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iPageNo + 1);
                //        SetQueryString(_stmpl_pages);
                //        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());

                //        _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");
                //        //_stmpl_pages.SetAttribute("TBW_CATEGORY_ID", _cid);
                //        _stmpl_pages.SetAttribute("TBW_PAGE_NO", iPageNo + 2);
                //        SetQueryString(_stmpl_pages);
                //        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //        _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //        _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());//.Replace("-</td>", "</td>"));
                //    }
                //    else if (iPageNo > 0 && iPageNo < 4 && iPageNo < iTotalPages)
                //    {
                //        //_stmpl_pages.SetAttribute("TBW_CATEGORY_ID", catID);
                //        _stmpl_pages.SetAttribute("TBW_PAGE_NO", 1);
                //        SetQueryString(_stmpl_pages);
                //        //if (_ViewType == "LV")
                //        //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", true);
                //        //else
                //        //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", false);
                //        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //        if (iPageNo == 1)
                //        {
                //            if (1 == iTotalPages)
                //                // _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "tx_17A").Replace("-</td>", "</td>"));
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
                //            else
                //            {
                //                // _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "tx_17A"));
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
                //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");
                //            // _stmpl_pages.SetAttribute("TBW_CATEGORY_ID", catID);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", 2);
                //            SetQueryString(_stmpl_pages);
                //            //if (_ViewType == "LV")
                //            //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", true);
                //            //else
                //            //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", false);
                //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //            if (iPageNo == 2)
                //            {
                //                if (2 == iTotalPages)
                //                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
                //                else
                //                {
                //                    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));
                //                    _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //                }
                //            }
                //            else
                //            {
                //                //if (2 == iTotalPages)
                //                //    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("-</td>", "</td>"));
                //                //else
                //                //    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());

                //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
                //            }
                //        }

                //        if (3 <= iTotalPages)
                //        {
                //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");
                //            //_stmpl_pages.SetAttribute("TBW_CATEGORY_ID", catID);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", 3);
                //            SetQueryString(_stmpl_pages);
                //            //if (_ViewType == "LV")
                //            //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", true);
                //            //else
                //            //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", false);
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
                //                //if (3 == iTotalPages)
                //                //    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("-</td>", "</td>"));
                //                //else
                //                //    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
                //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
                //            }
                //        }
                //        if (4 <= iTotalPages)
                //        {
                //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");
                //            //_stmpl_pages.SetAttribute("TBW_CATEGORY_ID", catID);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", 4);
                //            SetQueryString(_stmpl_pages);
                //            //if (_ViewType == "LV")
                //            //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", true);
                //            //else
                //            //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", false);
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
                //                //if (4 == iTotalPages)
                //                //    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("-</td>", "</td>"));
                //                //else
                //                //    _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
                //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
                //            }

                //        }
                //        if (5 <= iTotalPages)
                //        {
                //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");
                //            //_stmpl_pages.SetAttribute("TBW_CATEGORY_ID", catID);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", 5);
                //            SetQueryString(_stmpl_pages);
                //            //if (_ViewType == "LV")
                //            //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", true);
                //            //else
                //            //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", false);
                //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //            if (iPageNo == 5)
                //            {
                //               // _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now").Replace("-</td>", "</td>"));
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
                //            //_stmpl_pages.SetAttribute("TBW_CATEGORY_ID", catID);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 4);
                //            SetQueryString(_stmpl_pages);
                //            //if (_ViewType == "LV")
                //            //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", true);
                //            //else
                //            //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", false);
                //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


                //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");
                //            //_stmpl_pages.SetAttribute("TBW_CATEGORY_ID", catID);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 3);
                //            SetQueryString(_stmpl_pages);
                //            //if (_ViewType == "LV")
                //            //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", true);
                //            //else
                //            //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", false);
                //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


                //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");
                //            //_stmpl_pages.SetAttribute("TBW_CATEGORY_ID", catID);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 2);
                //            SetQueryString(_stmpl_pages);
                //            //if (_ViewType == "LV")
                //            //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", true);
                //            //else
                //            //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", false);
                //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


                //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");
                //            //_stmpl_pages.SetAttribute("TBW_CATEGORY_ID", catID);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 1);
                //            SetQueryString(_stmpl_pages);
                //            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


                //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");
                //            //_stmpl_pages.SetAttribute("TBW_CATEGORY_ID", catID);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages);
                //            SetQueryString(_stmpl_pages);
                //            //if (_ViewType == "LV")
                //            //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", true);
                //            //else
                //            //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", false);
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
                //            //_stmpl_pages.SetAttribute("TBW_CATEGORY_ID", catID);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 3);
                //            SetQueryString(_stmpl_pages);
                //            //if (_ViewType == "LV")
                //            //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", true);
                //            //else
                //            //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", false);
                //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


                //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");
                //            //_stmpl_pages.SetAttribute("TBW_CATEGORY_ID", catID);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 2);
                //            SetQueryString(_stmpl_pages);
                //            //if (_ViewType == "LV")
                //            //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", true);
                //            //else
                //            //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", false);
                //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


                //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");
                //            //_stmpl_pages.SetAttribute("TBW_CATEGORY_ID", catID);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 1);
                //            SetQueryString(_stmpl_pages);
                //            //if (_ViewType == "LV")
                //            //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", true);
                //            //else
                //            //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", false);
                //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());


                //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");
                //            //_stmpl_pages.SetAttribute("TBW_CATEGORY_ID", catID);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages);
                //            SetQueryString(_stmpl_pages);
                //            //if (_ViewType == "LV")
                //            //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", true);
                //            //else
                //            //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", false);
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
                //                //check  comment by palani
                //                //_stmpl_pages.SetAttribute("TBW_CATEGORY_ID", _cid);
                //                _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 4);
                //                SetQueryString(_stmpl_pages);
                            
                //                _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
                //            }


                //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");
                //            //_stmpl_pages.SetAttribute("TBW_CATEGORY_ID", catID);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 3);
                //            SetQueryString(_stmpl_pages);
                         
                //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());




                //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");
                //            //_stmpl_pages.SetAttribute("TBW_CATEGORY_ID", catID);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 2);

                //            SetQueryString(_stmpl_pages);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //            _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());



                //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");
                //            //_stmpl_pages.SetAttribute("TBW_CATEGORY_ID", catID);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 1);
                //            SetQueryString(_stmpl_pages);
                //            //if (_ViewType == "LV")
                //            //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", true);
                //            //else
                //            //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", false);
                //            _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //           // _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
                //            if (iPageNo != iTotalPages)
                //                 _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now").Replace("\n", "").Replace("\r", ""));//.Replace("-</td>", "</td>"));
                //            else
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());

                //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");
                //            //_stmpl_pages.SetAttribute("TBW_CATEGORY_ID", catID);

                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages);
                //            SetQueryString(_stmpl_pages);
                //            if (iPageNo == iTotalPages)
                //            {
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
                //            }
                //            else
                //            {
                //               // _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString().Replace("-</td>", "</td>"));
                //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());//.Replace("tx_10A", "now"));//.Replace("-</td>", "</td>"));
                //            }


                //        }


                //        else if (iPageNo == iTotalPages)
                //        {

                //            if (iTotalPages - 3 > 0)
                //            {
                //                //_stmpl_pages.SetAttribute("TBW_CATEGORY_ID", catID);
                //                _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 3);
                //                SetQueryString(_stmpl_pages);
                //                //if (_ViewType == "LV")
                //                //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", true);
                //                //else
                //                //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", false);
                //                _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
                //            }
                //            if (iTotalPages - 2 > 0)
                //            {
                //                _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");
                //                //_stmpl_pages.SetAttribute("TBW_CATEGORY_ID", catID);
                //                _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 2);
                //                SetQueryString(_stmpl_pages);
                //                //if (_ViewType == "LV")
                //                //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", true);
                //                //else
                //                //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", false);
                //                _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
                //            }
                //            if (iTotalPages - 1 > 0)
                //            {
                //                _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");
                //                //_stmpl_pages.SetAttribute("TBW_CATEGORY_ID", catID);
                //                _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages - 1);
                //                SetQueryString(_stmpl_pages);
                //                //if (_ViewType == "LV")
                //                //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", true);
                //                //else
                //                //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", false);
                //                _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);
                //                _stmpl_pages.SetAttribute("TBW_PAGE_Minus", "-");
                //                _stmpl_container.SetAttribute("TBW_DISPLAY_PAGE_NO", _stmpl_pages.ToString());
                //            }
                //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");
                //            //_stmpl_pages.SetAttribute("TBW_CATEGORY_ID", catID);
                //            _stmpl_pages.SetAttribute("TBW_PAGE_NO", iTotalPages);
                //            SetQueryString(_stmpl_pages);
                //            //if (_ViewType == "LV")
                //            //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", true);
                //            //else
                //            //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", false);
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
                //        // _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpageno");
                //        //_stmpl_pages.SetAttribute("TBW_CATEGORY_ID", catID);
                //        // _stmpl_pages.SetAttribute("TBW_PAGE_NO", (iPageNo+1));
                //        //_stmpl_container.SetAttribute("TBW_NEXT_PAGE", "<td align=\"left\" class=\"tx_10A\"><a href=\"product_list.aspx?pgno=" + (iPageNo + 1) + "&cid=" + catID + "\" >Next Page</a></td>");
                //        _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpagenoNext");
                //        _stmpl_pages.SetAttribute("TBW_PAGE_NO", (iPageNo + 1));
                //        SetQueryString(_stmpl_pages);
                //        //if (_ViewType == "LV")
                //        //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", true);
                //        //else
                //        //    _stmpl_pages.SetAttribute("TBW_VIEWTYPE1", false);
                //        _stmpl_pages.SetAttribute("TBW_VIEWTYPE", _ViewType);

                //        _stmpl_container.SetAttribute("TBW_NEXT_PAGE", _stmpl_pages.ToString());                      
                        



                //    }
                //    else
                //    {

                //        if (iPageNo != iTotalPages)
                //        {
                //            ////_stmpl_container.SetAttribute("TBW_NEXT_PAGE", "<td align=\"left\" class=\"tx_10A\"><a href=\"product_list.aspx?pgno=" + (iPageNo) + "&cid=" + catID + "\" >Next Page</a></td>");
                //            //_stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpagenoNext");
                //            //_stmpl_pages.SetAttribute("TBW_PAGE_NO", (iPageNo));
                //            //SetQueryString(_stmpl_pages);
                //            //_stmpl_container.SetAttribute("TBW_NEXT_PAGE", _stmpl_pages.ToString());   
                //            _stmpl_pages = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "productpagenoNext");
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
                //if (iRecordsPerPage==32767) //View All
                //{
                //    iRecordsPerPage = iTotalProducts;
                //}
                //_stmpl_container.SetAttribute("TBW_START_PAGE_NO", (iPageNo * iRecordsPerPage) - (iRecordsPerPage - 1));
                //if (((iPageNo * iRecordsPerPage) > iTotalProducts))
                //    _stmpl_container.SetAttribute("TBW_END_PAGE_NO", iTotalProducts);
                //else
                //    _stmpl_container.SetAttribute("TBW_END_PAGE_NO", (iPageNo * iRecordsPerPage));

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
                //sHTML = _stmpl_container.ToString().Replace("<option value=\"" + iRecordsPerPage.ToString() + "\">", "<option value=\"" + iRecordsPerPage.ToString() + "\" selected =selected>");
                //if (DSfp.Tables[0].Rows.Count == 0)
                //{
                //    try
                //    {
                //        sHTML = sHTML.Remove(sHTML.IndexOf("<%--Featured Product--%>"), sHTML.IndexOf("<%--/Featured Product--%>") - sHTML.IndexOf("<%--Featured Product--%>"));
                //    }
                //    catch (Exception ex)
                //    { }
                //}
                //if (sHTML.ToString().Contains("src=\"prodimages\""))
                //{
                //    sHTML = sHTML.Replace("src=\"prodimages\"", "src=\"images/noimage.gif\"");
                //    sHTML = sHTML.Replace("href=\"javascript:Zoom('prodimages')\"", "href=\"#\"");
                //}
                //if (sHTML.ToString().Contains("src=\"\""))
                //{
                //    sHTML = sHTML.ToString().Replace("src=\"\"", "src=\"images/noimage.gif\"");
                //    sHTML = sHTML.Replace("href=\"javascript:Zoom('')\"", "href=\"#\"");
                //}
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

        string orgurl = HttpContext.Current.Request.Url.PathAndQuery.ToString();
        string eapath = _EAPath.Replace("'", "###");
       

        htmleapath.Value = eapath.ToString();
       
        htmltotalpages.Value = iTotalPages.ToString();
        htmlviewmode.Value = _ViewType;

        htmlirecords.Value = iRecordsPerPage.ToString();
       
        sHTML = sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
       // return objHelperServices.StripWhitespace(sHTML);
        return sHTML;
    }


    public string ST_ProductListJson()
    {

        FamilyServices objFamilyServices = new FamilyServices();
        StringTemplateGroup _stg_container = null;
        StringTemplateGroup _stg_records = null;
        StringTemplateGroup _stg_records_sub = null;
        StringTemplate _stmpl_container = null;
        StringTemplate _stmpl_container_sub = null;
        StringTemplate _stmpl_records = null;
        StringTemplate _stmpl_records_sub = null;
      //  StringTemplate _stmpl_pages = null;
        DataSet dsprod = new DataSet();
        DataSet dsprodspecs = new DataSet();
        DataSet subcatds = new DataSet();
        string strFile = HttpContext.Current.Server.MapPath("ProdImages");

        bool BindToST = true;
        int oe = 0;
        int oe_sub = 0;
        string category_nameh = string.Empty;
        string sHTML = string.Empty;
        string _pcr = string.Empty;
        string _ViewType = string.Empty;
        string lstcarecord = string.Empty;
        string lstprerecord = string.Empty;
        string lstvperrecord = string.Empty;
        DataSet breadcrumb = new DataSet();
        HelperServices objHelperService = new HelperServices();
       // string CID_new = "";
       // string PCID_new = "";
        string tmpstrPid = string.Empty;
        string tmpdivcount = string.Empty;
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
            {
                _cid = Request.QueryString["cid"];
                Session["SS__cid"] = _cid;
            }
            if (Request.QueryString["pcr"] != null)
            {
                _pcr = Request.QueryString["pcr"];
                Session["SS___pcr"] = _pcr;
            }


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





            if (Request.Url.ToString().ToLower().Contains("product_list.aspx"))
            {
                //ps.ClearFilterproduct("DELETE TBWC_SEARCH_PROD_LIST WHERE PRODUCT_ID IN(SELECT DISTINCT F.FAMILY_ID FROM TB_FAMILY F,TB_FAMILY_SPECS FS WHERE F.FAMILY_ID=FS.FAMILY_ID AND ISNULL(FS.STRING_VALUE,'')='NOT FOR WEB' AND F.CATEGORY_ID='" + ps.CATEGORY_ID + "') AND USER_SESSION_ID='" + Session.SessionID + "'");
                // dscat = ps.GetFamilies();
                dscat = (DataSet)HttpContext.Current.Session["FamilyProduct"];
            }
            else
            {
                // dscat = ps.GetProducts();
            }

            DataSet dsLHS = new DataSet();
            if ((DataSet)HttpContext.Current.Session["LHSAttributes"] != null)
            {
                dsLHS = (DataSet)HttpContext.Current.Session["LHSAttributes"];
            }


            if (dscat != null)
            {

                if (Request.QueryString["pcr"] != null)
                    _pcr = Request.QueryString["pcr"];
                if (Request.QueryString["type"] == null)
                {
                    //GetDataSet("SELECT CATEGORY_NAME  FROM	TB_CATEGORY WHERE	CATEGORY_ID ='" + catID + "'").Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
                    string tempstr = (string)objHelperDB.GetGenericDataDB(_cid, "GET_CATEGORY_NAME", HelperDB.ReturnType.RTString);
                    if (tempstr != null && tempstr != "")
                        category_nameh = tempstr;
                }
                else
                    category_nameh = Request.QueryString["value"].ToString();

                DataRow drpagect = dscat.Tables[0].Rows[0];
                // if (drpagect.Table.Rows.Count > 0)
                // {
                iTotalPages = Convert.ToInt32(drpagect["TOTAL_PAGES"]);
                //  }
                Session["iTotalPages"] = iTotalPages;
                if (iPageNo > iTotalPages)
                {
                    iPageNo = iTotalPages;

                }

                DataRow drproductsct = dscat.Tables[1].Rows[0];
                iTotalProducts = Convert.ToInt32(drproductsct["TOTAL_PRODUCTS"]);
                if (_cid != "")
                    _ParentCatID = GetParentCatID(_cid);

                if ((iTotalPages <= 0 || iTotalProducts <= 0) && WesNewsCategoryId == _ParentCatID)
                {


                    _stg_container = new StringTemplateGroup("searchrsltproductmain", stemplatepath);
                    _stmpl_container = _stg_container.GetInstanceOf("searchrsltproducts\\advmain");

                    SetEbookAndPDFLink(_stmpl_container);
                    DataTable tmpDataTable = (DataTable)objHelperDB.GetGenericDataDB(WesCatalogId, "'" + _cid + "'", "GET_CATEGORY", HelperDB.ReturnType.RTTable);
                    if (tmpDataTable != null && tmpDataTable.Rows.Count > 0)
                    {
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
                _stg_records = new StringTemplateGroup("searchrsltproductrecords", stemplatepath);

                _stg_records_sub = new StringTemplateGroup("searchrsltproductrecords", stemplatepath);

                TBWDataList1[] lstrecords_sub = new TBWDataList1[0];
                TBWDataList1[] lstrows_sub = new TBWDataList1[0];

                int ictrecords = 0;
                int icolstart = 0;

                int ictrecords_sub = 0;
                int icolstart_sub = 0;

                string trmpstr = string.Empty;
                int icol = 1;
                int icol_sub = 0;
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

              //  DataRow[] drprodcoll = dscat.Tables[2].Select("ATTRIBUTE_ID = 1", "SNO");


                string userid = string.Empty;
                string PriceTable = string.Empty;
                int pricecode = 0;
                string tmpProds = string.Empty;
                DataSet dsBgDisc = new DataSet();
                DataSet dsPriceTableAll = new DataSet();
                if (Session["USER_ID"] != null)
                    userid = Session["USER_ID"].ToString();
                if (userid == "")
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


                tmpProds = "";
                if (Convert.ToInt32(userid) > 0)
                {
                    foreach (DataRow drpid in dscat.Tables["FamilyPro"].Rows)
                    {
                        tmpProds = tmpProds + drpid["PRODUCT_ID"].ToString() + ",";
                    }
                    if (tmpProds != "")
                    {
                        tmpProds = tmpProds.Substring(0, tmpProds.Length - 1);
                       // dsPriceTableAll = objHelperDB.GetProductPriceTableAll(tmpProds, Convert.ToInt32(userid));
                    }
                }

                bool IsEcomEnabled = objHelperServices.GetIsEcomEnabled(userid);
                lstrecords = new TBWDataList[dscat.Tables["FamilyPro"].Rows.Count + 1];
                string CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(userid));
                string cellGV = string.Empty;
                string cellLV = string.Empty;
                
                cellGV = "searchrsltproducts\\productlist_GridView";
                cellLV = "searchrsltproducts\\productlist_WES";

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
                    BindToST = true;
                    if (_ViewType == "GV")
                        _stmpl_records = _stg_records.GetInstanceOf(cellGV);
                    else
                        _stmpl_records = _stg_records.GetInstanceOf(cellLV + soddevenrow);

                  //  string urlDesc = "";
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
                    _stmpl_records.SetAttribute("FAMILY_ID", drpid["FAMILY_ID"].ToString());
                    _stmpl_records.SetAttribute("CATEGORY_ID", drpid["CATEGORY_ID"].ToString());
                    _stmpl_records.SetAttribute("PARENT_CATEGORY_ID", drpid["PARENT_CATEGORY_ID"].ToString());

                    _stmpl_records.SetAttribute("FAMILY_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath)));

                    _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath + "////UserSearch1=Family Id=" + drpid["FAMILY_ID"].ToString())));

                    _stmpl_records.SetAttribute("TBT_PRODUCT_ID", drpid["PRODUCT_ID"].ToString());
                    _stmpl_records.SetAttribute("PRODUCT_CODE", drpid["PRODUCT_CODE"].ToString());
                    _stmpl_records.SetAttribute("TBT_FAMILY_ID", drpid["FAMILY_ID"].ToString());
                    _stmpl_records.SetAttribute("divcount", drpid["PRODUCT_ID"] + "_" + icnt);

                    trmpstr = drpid["FAMILY_NAME"].ToString().Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");

              

                    if (_ViewType == "GV")
                    {

                        if (trmpstr.Length > 60)
                            trmpstr = trmpstr.Substring(0, 60) + "...";
                    }

                    _stmpl_records.SetAttribute("FAMILY_NAME", trmpstr);

                    _stmpl_records.SetAttribute("FAMILY_PRODUCT_COUNT", drpid["FAMILY_PRODUCT_COUNT"].ToString());
                    _stmpl_records.SetAttribute("PRODUCT_COUNT", drpid["PRODUCT_COUNT"].ToString());

                    _stmpl_records.SetAttribute("TBT_ECOMENABLED", IsEcomEnabled);
                    if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
                        _stmpl_records.SetAttribute("TBT_WITH_LOGIN", true);
                    //  string ValueFortag = string.Empty;
                    //ValueFortag = "<div id=\"pid" + dscat.Tables[2].Rows[0]["PRODUCT_ID"].ToString() + "\" style=\"background-color:#ffffff;visibility:hidden;position:absolute\">" + AssemblePriceTable(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus) + "</div><div onMouseOver=\"javascript:ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onMouseOut=\"javascript:ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" style=\"position:relative\">" + ValueFortag + "</div>";
                    else
                        _stmpl_records.SetAttribute("TBT_WITH_LOGIN", false);
<<<<<<< .mine

                 
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



                        _stmpl_records.SetAttribute("TBT_USER_PRICE", drpid["PRODUCT_PRICE"].ToString());
                        _stmpl_records.SetAttribute("ATTRIBUTE_ID_5", drpid["PRODUCT_PRICE"].ToString());

                        if (Convert.ToInt32(drpid["QTY_AVAIL"].ToString()) != -1)
                        {
                            if (Convert.ToInt32(drpid["QTY_AVAIL"].ToString()) > 0)
                            {
                                _stmpl_records.SetAttribute("TBT_QTY_AVAIL", drpid["QTY_AVAIL"].ToString());
                                _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", drpid["MIN_ORD_QTY"].ToString());
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
                        _stmpl_records.SetAttribute("TBT_MIN_PRICE", drpid["MIN_PRICE"].ToString());
                    }
                    else
                    {
                        _stmpl_records.SetAttribute("TBT_SUB_FAMILY", false);
                        _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", true);

                        string stk_sta_desc = "";
                        stk_sta_desc = drpid["STOCK_STATUS_DESC"].ToString().Trim();
                        //   objErrorHandler.CreateLog(stk_sta_desc);
                        
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
                            if ((stk_sta_desc.ToUpper().Contains("OUT_OF_STOCK") == true ||stk_sta_desc.ToUpper().Contains("SPECIAL_ORDER") == true  ) && drpid["PROD_SUBSTITUTE"].ToString().Trim() != "" && drpid["PROD_STOCK_FLAG"].ToString().Trim() == "-2")
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

                            else  if (HttpContext.Current.Session["EBAY_BLOCK"] != null && HttpContext.Current.Session["EBAY_BLOCK"].ToString() == "True" && drpid["EBAY_BLOCK"] != null && drpid["EBAY_BLOCK"].ToString() == "True")
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



                        _stmpl_records.SetAttribute("TBT_USER_PRICE", drpid["PRODUCT_PRICE"].ToString());
                        _stmpl_records.SetAttribute("ATTRIBUTE_ID_5", drpid["PRODUCT_PRICE"].ToString());
                        
                        if (Convert.ToInt32(drpid["QTY_AVAIL"].ToString()) != -1)
                        {
                            if (Convert.ToInt32(drpid["QTY_AVAIL"].ToString()) > 0)
                            {
                                _stmpl_records.SetAttribute("TBT_QTY_AVAIL", drpid["QTY_AVAIL"].ToString());
                                _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", drpid["MIN_ORD_QTY"].ToString());
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

                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_1", drpid["PRODUCT_CODE"].ToString());

                    System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + drpid["Prod_Thumbnail"].ToString().Replace("/", "\\"));
                    if (Fil.Exists)
                        _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", drpid["Prod_Thumbnail"].ToString());
                    else
                        _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", "/images/noimage.gif");



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
                            _stmpl_records.SetAttribute("PROD_DESC_ALT", drpid["FAMILY_NAME"].ToString());
                        if (desc.Length > 250 && _ViewType == "LV")
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

                        if (desc.Length > 250 && _ViewType == "LV")
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

                        descattr = drpid["Family_ShortDescription"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                        if (descattr.Length > 0)
                            descattr = descattr + "<br/>";
                        descattr = descattr + drpid["Family_Description"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                        descattr = descattr + " " + drpid["Prod_Description"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");

                        prod_desc_alt = drpid["Prod_Description"].ToString();
                        if (prod_desc_alt.Length > 0)
                            _stmpl_records.SetAttribute("PROD_DESC_ALT", prod_desc_alt);
                        else
                            _stmpl_records.SetAttribute("PROD_DESC_ALT", drpid["FAMILY_NAME"].ToString());
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
                    //            //System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dr["STRING_VALUE"].ToString().Replace("/", "\\"));
                    //            //if (Fil.Exists)
                    //            //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));
                    //            //else
                    //            //{
                    //            //    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), "/images/noimage.gif");
                    //            //}
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
                    //    string descattr = "";
                    //    string desc = "";
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
                    //            desc = dr["STRING_VALUE"].ToString().Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                    //            if (dr["ATTRIBUTE_ID"].ToString() == "13" && desc.Length > 0)
                    //                desc = desc + "<br/>";

                    //            // descattr = descattr +" "+ desc;
                    //            descattr = descattr + desc;
                    //            if (desc.Length > 250 && _ViewType == "LV")
                    //            {
                    //                _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
                    //                desc = desc.Substring(0, 250).ToString();
                    //                _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), desc);
                    //            }
                    //            //else if (desc.Length > 30 && _ViewType == "GV")
                    //            //{
                    //            //    _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
                    //            //    desc = desc.Substring(0,30).ToString();
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

                    //lstrows[icolstart] = new TBWDataList(_stmpl_records.ToString());
                    icolstart++;
                    if (icolstart >= icol || oe == dscat.Tables["FamilyPro"].Rows.Count)
                    {
                        _stg_container = new StringTemplateGroup("searchrsltproductcontainer", stemplatepath);
                        _stmpl_container = _stg_container.GetInstanceOf("searchrsltproducts\\producttable_" + soddevenrow);
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
                _stg_container = new StringTemplateGroup("searchrsltproductmain", stemplatepath);

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


                _stmpl_container = _stg_container.GetInstanceOf("searchrsltproducts\\main");
                _stmpl_container.SetAttribute("TBW_CATEGORY_ID", _cid);
                _stmpl_container.SetAttribute("TBW_PRODUCT_COUNT", iTotalProducts);
                _stmpl_container.SetAttribute("TBW_CATEGORY_NAME", category_nameh);

                _stmpl_container.SetAttribute("TBW_SUB_CAT_IMAGE", false);

                try
                {
                    if (dsLHS != null && dsLHS.Tables.Count > 0)
                    {
                        if (dsLHS.Tables["category"].Rows.Count > 0)
                        {
                            string _parentCatID = string.Empty;
                            string _catCid = string.Empty;
                            if (Request.QueryString["pcr"] != null)
                            {
                                _catCid = Request.QueryString["pcr"];
                            }
                            if (_catCid != "")
                            {
                                _parentCatID = GetParentCatID(_catCid);
                            }
                            icol_sub = 6;
                            lstrows_sub = new TBWDataList1[icol_sub];
                            lstrecords_sub = new TBWDataList1[dsLHS.Tables["category"].Rows.Count + 1];
                            bool SubcatST = false;
                            foreach (DataRow dr in dsLHS.Tables["category"].Rows)
                            {
                                string img_url = string.Empty;
                                img_url = GetCategoryImageFile(dr["CATEGORY_ID"].ToString());
                                SubcatST = false;
                                if (img_url != "" && img_url != null)
                                {
                                    SubcatST = true;
                                    _stmpl_container.SetAttribute("TBW_SUB_CAT_IMAGE", true);
                                    img_url = objHelperServices.SetImageFolderPath(img_url.Replace("\\", "/"), "_Images", "_th");
                                }

                           
                                 oe_sub++;
                                _stmpl_records_sub = _stg_records_sub.GetInstanceOf("searchrsltproducts\\productlist_subcategory");
                                _stmpl_records_sub.SetAttribute("SUB_CAT_NAME", dr["CATEGORY_NAME"].ToString());
                                _stmpl_records_sub.SetAttribute("TBW_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(_parentCatID.ToString()));
                                _stmpl_records_sub.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(dr["CATEGORY_ID"].ToString()));
                                _stmpl_records_sub.SetAttribute("TBW_ATTRIBUTE_NAME", dr["Category_Name"]);
                                _stmpl_records_sub.SetAttribute("TBW_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(dr["Category_Name"].ToString()));
                                _stmpl_records_sub.SetAttribute("TBW_ATTRIBUTE_BRAND", HttpUtility.UrlEncode(dr["brandvalue"].ToString()));
                                _stmpl_records_sub.SetAttribute("TBW_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(dr["SearchString"].ToString()));
                                _stmpl_records_sub.SetAttribute("TBW_ATTRIBUTE_TYPE", "Category"); //
                                _stmpl_records_sub.SetAttribute("TBT_IMAGE_FILE", img_url);
                                if (HttpContext.Current.Session["EA"] != null)
                                {
                                    _stmpl_records_sub.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(HttpContext.Current.Session["EA"].ToString())));
                                }

                                if (SubcatST == true)
                                {
                                    lstrows_sub[icolstart_sub] = new TBWDataList1(_stmpl_records_sub.ToString());
                                }
                                icolstart_sub++;
                                if (icolstart_sub >= icol_sub || oe_sub == dsLHS.Tables["category"].Rows.Count)
                                {
                                    _stg_container = new StringTemplateGroup("searchrsltproductcontainer", stemplatepath);
                                    _stmpl_container_sub = _stg_container.GetInstanceOf("searchrsltproducts\\producttable_sub");
                                    _stmpl_container_sub.SetAttribute("TBWDataList1", lstrows_sub);
                                    lstrecords_sub[ictrecords_sub] = new TBWDataList1(_stmpl_container_sub.ToString());
                                    ictrecords_sub++;
                                    icolstart_sub = 0;
                                    lstrows_sub = new TBWDataList1[icol_sub];

                                }
                             // }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    objErrorHandler.CreateLog(ex.ToString()); 
                }


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

                breadcrumb = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];
                DataRow[] _DCRow = null;

                if (breadcrumb != null && breadcrumb.Tables.Count > 0 && breadcrumb.Tables[0].Rows.Count > 0)
                {
                    if (breadcrumb.Tables[0].Rows[0]["ItemType"].ToString().ToLower().Contains("cablel"))
                    {
                        string c1 = breadcrumb.Tables[0].Rows[0]["Itemvalue"].ToString();
                        string c2 = "anything";
                        if (breadcrumb.Tables[0].Rows.Count > 1 && breadcrumb.Tables[0].Rows[1]["ItemType"].ToString().ToLower().Contains("cablelr"))
                            c2 = breadcrumb.Tables[0].Rows[1]["Itemvalue"].ToString();

                        _stmpl_container.SetAttribute("TBTC_IMAGE_FILE", "/images/noimage.gif");
                        _stmpl_container.SetAttribute("TBTC_SHORT_DESC_CAT", "Your cable finder results for " + c1 + " to " + c2);
                        _stmpl_container.SetAttribute("TBTC_CATEGORY_NAME", "Cable Finder Results");
                    }
                    else
                    {
                        _DCRow = breadcrumb.Tables[0].Select("ItemType='Category'");
                        if (_DCRow != null && _DCRow.Length > 0)
                        {
                            foreach (DataRow _drow in _DCRow)
                            {
                                if (_DCRow.Length == 3)
                                {
                                    if (_drow == _DCRow[_DCRow.Length - 1])
                                    {
                                        lstcarecord = _drow["ItemValue"].ToString();
                                    }
                                    if (_drow == _DCRow[_DCRow.Length - 2])
                                    {
                                        lstprerecord = _drow["ItemValue"].ToString();
                                    }

                                    if (_drow == _DCRow[_DCRow.Length - 3])
                                    {
                                        lstvperrecord = _drow["ItemValue"].ToString();
                                    }
                                }
                                else if (_DCRow.Length == 2)
                                {
                                    if (_drow == _DCRow[_DCRow.Length - 1])
                                    {
                                        lstprerecord = _drow["ItemValue"].ToString();
                                    }
                                    if (_DCRow[_DCRow.Length - 2] != null)
                                    {
                                        if (_drow == _DCRow[_DCRow.Length - 2])
                                        {
                                            lstvperrecord = _drow["ItemValue"].ToString();
                                        }
                                    }
                                    else
                                    {
                                        lstvperrecord = "0";
                                    }

                                }
                                else
                                {
                                    if (_drow == _DCRow[_DCRow.Length - 1])
                                    {
                                        lstprerecord = _drow["ItemValue"].ToString();
                                    }
                                    lstvperrecord = "0";


                                }
                            }
                        }
                        if (_DCRow.Length != 1)
                        {
                            if (lstcarecord != null && lstcarecord != "" && lstprerecord != null && lstprerecord != "" && lstvperrecord != null && lstvperrecord != "")
                                subcatds = (DataSet)objHelperDB.GetGenericDataDB("", lstvperrecord, lstprerecord, lstcarecord, "GET_CATEGORY_DETAILS", HelperDB.ReturnType.RTDataSet);
                            else
                                subcatds = (DataSet)objHelperDB.GetGenericDataDB("", lstvperrecord, lstprerecord, lstcarecord, "GET_CATEGORY_DETAILS_BR", HelperDB.ReturnType.RTDataSet);
                        }
                        else
                            subcatds = null;

                        if (subcatds == null)
                            subcatds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, lstprerecord, "0", "GET_CAT_DETAILS", HelperDB.ReturnType.RTDataSet);

                        if (subcatds != null && subcatds.Tables.Count >= 1)
                        {
                            string catdesc = string.Empty;
                            string catename = string.Empty;
                            catdesc = subcatds.Tables[0].Rows[0]["SHORT_DESC"].ToString();
                            catename = subcatds.Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
                            _stmpl_container.SetAttribute("TBTC_SHORT_DESC_CAT", catdesc);
                            _stmpl_container.SetAttribute("TBTC_CATEGORY_NAME", catename);
                            // System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + subcatds.Tables[0].Rows[0]["IMAGE_FILE"].ToString());
                            string imgpath = subcatds.Tables[0].Rows[0]["IMAGE_FILE"].ToString().Replace("\\", "/");

                            if (!(subcatds.Tables[0].Rows[0]["IMAGE_FILE"].ToString().ToLower().Contains("_th")))
                            {
                                imgpath = objHelperService.SetImageFolderPath(imgpath, "_images", "_th");
                            }
                            System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + imgpath);

                            if (Fil.Exists)
                                // _stmpl_container.SetAttribute("TBTC_IMAGE_FILE", subcatds.Tables[0].Rows[0]["IMAGE_FILE"].ToString().Replace("\\", "/"));
                                _stmpl_container.SetAttribute("TBTC_IMAGE_FILE", imgpath);
                            else
                            {
                                _stmpl_container.SetAttribute("TBTC_IMAGE_FILE", "/images/noimage.gif");
                            }

                        }
                        else
                        {
                            _stmpl_container.SetAttribute("TBTC_IMAGE_FILE", "/images/noimage.gif");
                            _stmpl_container.SetAttribute("TBTC_SHORT_DESC_CAT", "");
                            _stmpl_container.SetAttribute("TBTC_CATEGORY_NAME", "");
                        }
                    }
                }
                else
                {
                    _stmpl_container.SetAttribute("TBTC_IMAGE_FILE", "/images/noimage.gif");
                    _stmpl_container.SetAttribute("TBTC_SHORT_DESC_CAT", "");
                    _stmpl_container.SetAttribute("TBTC_CATEGORY_NAME", "");
                }

                _stmpl_container.SetAttribute("TBW_URL", powersearchURListView);
                _stmpl_container.SetAttribute("TBW_URL1", powersearchURLGridView);

                if (_ViewType == "LV")
                    _stmpl_container.SetAttribute("TBW_VIEWTYPE", true);
                else
                    _stmpl_container.SetAttribute("TBW_VIEWTYPE", false);


                SetEbookAndPDFLink(_stmpl_container);
            
                if (iTotalPages > 1 && iPageNo != iTotalPages && iPageNo < iTotalPages)
                {
                    //_stmpl_container.SetAttribute("TBW_TOTAL_PAGES", iTotalPages);
                    //_stmpl_container.SetAttribute("TBW_PREVIOUS_PG_NO", iPrevPgNo);
                    //_stmpl_container.SetAttribute("TBW_CURRENT_PAGE_NO", iPageNo);
                    //_stmpl_container.SetAttribute("TBW_NEXT_PG_NO", iNextPgNo);
                    _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                    _stmpl_container.SetAttribute("TBWDataList1", lstrecords_sub);
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
                    _stmpl_container.SetAttribute("TBWDataList1", lstrecords_sub);
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

        string orgurl = HttpContext.Current.Request.Url.PathAndQuery.ToString();
        string eapath = _EAPath.Replace("'", "###");


        htmleapath.Value = eapath.ToString();

        htmltotalpages.Value = iTotalPages.ToString();
        htmlviewmode.Value = _ViewType;

        htmlirecords.Value = iRecordsPerPage.ToString();

        sHTML = sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
      //  return objHelperServices.StripWhitespace(sHTML);
        return sHTML;
    }

    //private string GetParentCatID(string catID)
    //{
    //    try
    //    {
    //        DataSet DSBC = null;
    //        string catIDtemp = catID;
    //        do
    //        {
    //            //DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
    //            DSBC = (DataSet)objHelperDB.GetGenericPageDataDB(catIDtemp, "GET_CATEGORYLIST_CAREGORY", HelperDB.ReturnType.RTDataSet);
    //            if (DSBC != null)
    //            {
    //                foreach (DataRow DR in DSBC.Tables[0].Rows)
    //                {
    //                    catIDtemp = DR["PARENT_CATEGORY"].ToString();
    //                    if (catIDtemp == "0")
    //                    {
    //                        // DSUBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY = '" + DR["CATEGORY_ID"].ToString() + "' AND CATEGORY_NAME Like 'Product'");
    //                        return DR["CATEGORY_ID"].ToString();
    //                    }
    //                }
    //            }
    //        } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
    //        return DSBC.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //    return "";
    //}
    //private decimal GetMyPrice(int ProductID)
    //{
    //    decimal retval = 0.00M;
    //    string userid = HttpContext.Current.Session["USER_ID"].ToString();
    //    if (!string.IsNullOrEmpty(userid))
    //    {

    //        string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
    //        oHelper.SQLString = sSQL;
    //        int pricecode = oHelper.CI(oHelper.GetValue("price_code"));

    //        string strquery = "";
    //        if (pricecode == 1)
    //        {
    //            strquery = string.Format("exec GetWESIncProductPrice {0},{1},{2}", ProductID, 1, HttpContext.Current.Session["USER_ID"]);
    //        }
    //        else
    //        {
    //            strquery = string.Format("exec GetWESProductPrice {0},{1},{2}", ProductID, 1, HttpContext.Current.Session["USER_ID"]);
    //        }

    //        DataSet DSprice = new DataSet();
    //        oHelper.SQLString = strquery;
    //        retval = Math.Round(Convert.ToDecimal(oHelper.GetValue("Numeric_Value")), 2);
    //    }
    //    return retval;
    //}
    //private DataSet GetDataSet(string SQLQuery)
    //{
    //    DataSet ds = new DataSet();
    //    SqlDataAdapter da = new SqlDataAdapter(SQLQuery, conStr.ConnectionString.Substring(conStr.ConnectionString.IndexOf(';') + 1));
    //    da.Fill(ds, "generictable");
    //    return ds;
    //}

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
           // objErrorHandler.CreateLog(ex.ToString());
            return "";
        }
    }

    private string GetCategoryImageFile(string catID)
    {
        DataSet DSBC = null;
        string retVal = string.Empty;
        string catIDtemp = catID;

        try
        {
            DSBC = (DataSet)objHelperDB.GetGenericPageDataDB(catIDtemp, "GET_CATEGORY_IMAGE_PATH", HelperDB.ReturnType.RTDataSet);
            DSBC.Tables[0].TableName = "Image";
            if (DSBC != null)
            {
                foreach (DataRow oDR in DSBC.Tables["Image"].Rows)
                {
                    retVal = oDR["IMAGE_FILE"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            retVal = null;
        }

        return retVal;
        
    }

    private void SetEbookAndPDFLink(StringTemplate _stmpl_container)
    {
        DataSet dsCatalog = objCategoryServices.GetCatalogPDFDownload1(_cid);

        if (dsCatalog != null && dsCatalog.Tables.Count > 0 && dsCatalog.Tables[0].Rows.Count > 0)
        {
            string newfile = dsCatalog.Tables[0].Rows[0]["IMAGE_FILE2"].ToString();
            string Ebook_pdf_FileRef = System.Configuration.ConfigurationManager.AppSettings["Ebook_pdf_FileRef"].ToString();    
            if (!(newfile.ToLower().Contains(Ebook_pdf_FileRef)))
            {
                newfile = newfile.Replace("/media/", "/media/wes_secure_files/").Replace("\\media\\pdf\\", "\\media\\wes_secure_files\\pdf\\");
            }

            FileInfo Fil1 = new FileInfo(strPDFFiles1 + newfile);

            // if (dsCatalog.Tables[0].Rows[0]["IMAGE_NAME"].ToString() != "" || Fil1.Exists)
            // {
                 _stmpl_container.SetAttribute("TBT_DISPLAY_LINK", "block");
                 


                 if (dsCatalog.Tables[0].Rows[0]["CUSTOM_TEXT_FIELD2"].ToString() != "")
                 {
                     //modified by:indu
                    
                     string ebookpath = objHelperServices.viewebook(dsCatalog.Tables[0].Rows[0]["CUSTOM_TEXT_FIELD2"].ToString());
                     if (ebookpath.Contains("www."))
                     {

                         ebookpath = ebookpath.ToLower().Replace("attachments", "").Replace("\\", "").Replace("http://", "").Replace("https://", "");
                         ebookpath = "http://" + ebookpath;
                     }
                    // objErrorHandler.CreateLog("Inside if  ebook");  
                     _stmpl_container.SetAttribute("TBT_EBOOK_LINK", ebookpath.Replace("wes_secure_files/", ""));
                     _stmpl_container.SetAttribute("TBT_DISPLAY_EBOOK_LINK", "block");
                 }
                 else
                 {
                    // objErrorHandler.CreateLog("Inside else  ebook");  
                     _stmpl_container.SetAttribute("TBT_EBOOK_LINK", "");
                     _stmpl_container.SetAttribute("TBT_DISPLAY_EBOOK_LINK", "none");
                 }

                 if (dsCatalog.Tables[0].Rows[0]["CUSTOM_TEXT_FIELD3"].ToString() != "")
                 {
                     //modified by:indu
                     string ebookpath = dsCatalog.Tables[0].Rows[0]["CUSTOM_TEXT_FIELD3"].ToString();

                     if (ebookpath.Contains("www."))
                     {

                         ebookpath = ebookpath.ToLower().Replace("attachments", "").Replace("\\", "").Replace("http://", "").Replace("https://", "");
                         ebookpath = "http://" + ebookpath;
                     }
                     _stmpl_container.SetAttribute("TBT_HTML_LINK", ebookpath.Replace("wes_secure_files/", ""));
                     _stmpl_container.SetAttribute("TBT_DISPLAY_HTML_LINK", "block");
                 }
                 else
                 {
                     _stmpl_container.SetAttribute("TBT_HTML_LINK", "");
                     _stmpl_container.SetAttribute("TBT_DISPLAY_HTML_LINK", "none");
                 }




                 if (Fil1.Exists)
                 {
                     //Modified By :indu :Added replace function to remove  catelogdowload
                     _stmpl_container.SetAttribute("PDF", newfile.Replace("\\", "/").Replace("wes_secure_files/", ""));
                     _stmpl_container.SetAttribute("TBT_DISPLAY_PDF_LINK", "block");
                 }
                 else
                     _stmpl_container.SetAttribute("TBT_DISPLAY_PDF_LINK", "none");

            // }
            // else
            //     _stmpl_container.SetAttribute("TBT_DISPLAY_LINK", "none");


        }
        else
            _stmpl_container.SetAttribute("TBT_DISPLAY_LINK", "none");
    }


    public string DynamicPag(string url, int ipageno, string eapath, string ViewMode, string irecords)
    {
        //if (HttpContext.Current.Session["PS_SEARCH_RESULTS"] == null)
        //{
        //    return "";
        //}
      

        HelperServices objHelperServices = new HelperServices();
        ErrorHandler objErrorHandler = new ErrorHandler();
        CategoryServices objCategoryServices = new CategoryServices();
        Security objSecurity = new Security();
        HelperDB objHelperDB = new HelperDB();
        FamilyServices objFamilyServices = new FamilyServices();
        StringTemplateGroup _stg_container = null;
        StringTemplateGroup _stg_records = null;
        StringTemplate _stmpl_container = null;
        StringTemplate _stmpl_records = null;
        StringTemplate _stmpl_pages = null;
        DataSet dsprod = new DataSet();
        DataSet dsprodspecs = new DataSet();
        EasyAsk_WES EasyAsk = new EasyAsk_WES();
        ProductServices objProductServices = new ProductServices();
        string strFile = HttpContext.Current.Server.MapPath("ProdImages");
        string WesNewsCategoryId = System.Configuration.ConfigurationManager.AppSettings["WESNEWS_CATEGORY_ID"].ToString();
        string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();
        stemplatepath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["StringTemplatePath"].ToString());
        stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\product_list\\";
        string strPDFFiles1 = HttpContext.Current.Server.MapPath("attachments");
        int oe = 0;
        string category_nameh = string.Empty;
        string sHTML = string.Empty;
        string _pcr = string.Empty;
        string _ViewType = string.Empty;

        string userid = string.Empty;
        if (HttpContext.Current.Session["USER_ID"] != null)
        {
            userid = HttpContext.Current.Session["USER_ID"].ToString();
        }
        
        //if (userid == "")
        //    userid = "0";
        //if (Convert.ToInt32(HttpContext.Current.Session["PS_SEARCH_RESULTS"].ToString()) > 0)
        //{
        try
        {
            dscat = Get_Value_Breadcrum(ipageno, eapath,irecords);   
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
                HttpContext.Current.Session["PL_VIEW_MODE"] = _ViewType;
            }

            else
            {
                _ViewType = ViewMode;
            }


            //if (HttpContext.Current.Session["iPageNo"] != null)
            //{
            //    iPageNo = Convert.ToInt32(HttpContext.Current.Session["iPageNo"]);
            //}


            //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("product_list.aspx") == true)
            //{
            //    if (HttpContext.Current.Session["RECORDS_PER_PAGE_PRODUCT_LIST"] != null)
            //        iRecordsPerPage = Convert.ToInt32(HttpContext.Current.Session["RECORDS_PER_PAGE_PRODUCT_LIST"].ToString());


            //    EasyAsk.GetAttributeProducts("ProductList", "", _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

            //}

            //if (HttpContext.Current.Request.QueryString["path"] != null)
            //    _EAPath = HttpUtility.UrlDecode(objSecurity.StringDeCrypt(HttpContext.Current.Request.QueryString["path"].ToString()));

            //if (HttpContext.Current.Session["EA"] != null)
            //    _EAPath = HttpContext.Current.Session["EA"].ToString();


            _EAPath = dscat.Tables["eapath"].Rows[0][0].ToString();


            //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("product_list.aspx"))
            //{
            //    //ps.ClearFilterproduct("DELETE TBWC_SEARCH_PROD_LIST WHERE PRODUCT_ID IN(SELECT DISTINCT F.FAMILY_ID FROM TB_FAMILY F,TB_FAMILY_SPECS FS WHERE F.FAMILY_ID=FS.FAMILY_ID AND ISNULL(FS.STRING_VALUE,'')='NOT FOR WEB' AND F.CATEGORY_ID='" + ps.CATEGORY_ID + "') AND USER_SESSION_ID='" + HttpContext.Current.Session.HttpContext.Current.SessionID + "'");
            //    // dscat = ps.GetFamilies();
            //    dscat = (DataSet)HttpContext.Current.Session["FamilyProduct"];
            //}
            //else
            //{
            //    // dscat = ps.GetProducts();
            //}



            if (dscat != null)
            {

                if (HttpContext.Current.Request.QueryString["pcr"] != null)
                    _pcr = HttpContext.Current.Request.QueryString["pcr"];
                if (HttpContext.Current.Request.QueryString["type"] == null)
                {
                    //GetDataSet("SELECT CATEGORY_NAME  FROM	TB_CATEGORY WHERE	CATEGORY_ID ='" + catID + "'").Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
                    string tempstr = (string)objHelperDB.GetGenericDataDB(_cid, "GET_CATEGORY_NAME", HelperDB.ReturnType.RTString);
                    if (tempstr != null && tempstr != "")
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


                    _stg_container = new StringTemplateGroup("searchrsltproductmain", stemplatepath);
                    _stmpl_container = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "advmain");

                    SetEbookAndPDFLink(_stmpl_container);
                    DataTable tmpDataTable = (DataTable)objHelperDB.GetGenericDataDB(WesCatalogId, "'" + _cid + "'", "GET_CATEGORY", HelperDB.ReturnType.RTTable);
                    if (tmpDataTable != null && tmpDataTable.Rows.Count > 0)
                    {
                        //modified by:indu
                        string ebookpath =tmpDataTable.Rows[0]["CUSTOM_TEXT_FIELD3"].ToString();

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
                _stg_records = new StringTemplateGroup("searchrsltproductrecords", stemplatepath);


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

                DataRow[] drprodcoll = dscat.Tables[2].Select("ATTRIBUTE_ID = 1", "SNO");



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
                        _stmpl_records = _stg_records.GetInstanceOf("searchrsltproducts" + "\\" + "productlist_GridView");
                    else
                        _stmpl_records = _stg_records.GetInstanceOf("searchrsltproducts" + "\\" + "productlist_WES" + soddevenrow);

                   // string urlDesc = "";

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
                                System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dr["STRING_VALUE"].ToString().Replace("/", "\\"));
                                if (Fil.Exists)
                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));
                                else
                                {
                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), "/images/noimage.gif");
                                }

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
                        string descattr = string.Empty;
                        string desc = string.Empty;
                        foreach (DataRow dr in dsprodspecs.Tables[0].Rows)
                        {

                            if (dr["ATTRIBUTE_TYPE"].ToString() == "9")
                            {
                                System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dr["STRING_VALUE"].ToString());
                                if (Fil.Exists)
                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));
                                else
                                {
                                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_" + dr["ATTRIBUTE_ID"].ToString(), "/images/noimage.gif");
                                }

                            }
                            else if (dr["ATTRIBUTE_TYPE"].ToString() == "7")
                            {
                                //string desc = dr["STRING_VALUE"].ToString().Replace("\r", "&nbsp;").Replace("\n", "<br/>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                               // string desc = dr["STRING_VALUE"].ToString().Replace("\r", "&nbsp;");
                                desc = dr["STRING_VALUE"].ToString().Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");

                                if (dr["ATTRIBUTE_ID"].ToString() == "13" && desc.Length > 0)
                                    desc = desc + "<br/>";
                               // descattr = descattr +" "+ desc;
                                descattr = descattr + desc;
                                if (desc.Length > 250 && _ViewType == "LV")
                                {
                                    _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
                                    desc = desc.Substring(0, 250).ToString();
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
                            else if( descattr.Length > 140 && count < 35)
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

                    //lstrows[icolstart] = new TBWDataList(_stmpl_records.ToString());
                    icolstart++;
                    if (icolstart >= icol || oe == drprodcoll.Length)
                    {
                        _stg_container = new StringTemplateGroup("searchrsltproductcontainer", stemplatepath);
                        _stmpl_container = _stg_container.GetInstanceOf("searchrsltproducts" + "\\" + "producttable_" + soddevenrow);
                        _stmpl_container.SetAttribute("TBWDataList", lstrows);


                        lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
                        sHTML = sHTML + _stmpl_container.ToString();
                        ictrecords++;
                        icolstart = 0;
                        lstrows = new TBWDataList[icol];

                    }
                }

                //   }

                _stg_container = new StringTemplateGroup("searchrsltproductmain", stemplatepath);

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




                if (_ViewType == "LV")
                    _stmpl_container.SetAttribute("TBW_VIEWTYPE", true);
                else
                    _stmpl_container.SetAttribute("TBW_VIEWTYPE", false);








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

                DataSet DSnaprod = new DataSet();

            }
        }
        catch (Exception ex)
        {
            sHTML = ex.Message;
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
        finally
        {
            //if (oCon != null)
            //{
            //oCon.Close();
            //}
        }

        //}
        string strtophtml = " <table align=\"left\" width=\"775px\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" ><tr><td height=\"126\"  align=left ><TABLE border=0 cellSpacing=0 cellPadding=0 width=\"775px\"><tr><td>";
        string strbottom = "</td></tr></TABLE></td></tr><tr><td></td></tr></table>";
        sHTML = strtophtml + sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>") + strbottom;

       // return objHelperServices.StripWhitespace(sHTML);
        return sHTML;
    }

    public string DynamicPagJson(string url, int ipageno, string eapath, string ViewMode, string irecords)
    {
        //if (HttpContext.Current.Session["PS_SEARCH_RESULTS"] == null)
        //{
        //    return "";
        //}


        HelperServices objHelperServices = new HelperServices();
        ErrorHandler objErrorHandler = new ErrorHandler();
        CategoryServices objCategoryServices = new CategoryServices();
        Security objSecurity = new Security();
        HelperDB objHelperDB = new HelperDB();
        FamilyServices objFamilyServices = new FamilyServices();
        StringTemplateGroup _stg_container = null;
        StringTemplateGroup _stg_records = null;
        StringTemplate _stmpl_container = null;
        StringTemplate _stmpl_records = null;
        StringTemplate _stmpl_pages = null;
        DataSet dsprod = new DataSet();
        DataSet dsprodspecs = new DataSet();
        EasyAsk_WES EasyAsk = new EasyAsk_WES();
        ProductServices objProductServices = new ProductServices();
        bool BindToST = true;
        string strFile = HttpContext.Current.Server.MapPath("ProdImages");
        string WesNewsCategoryId = System.Configuration.ConfigurationManager.AppSettings["WESNEWS_CATEGORY_ID"].ToString();
        string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();
        stemplatepath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["StringTemplatePath"].ToString());
        stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\product_list\\";
        string strPDFFiles1 = HttpContext.Current.Server.MapPath("attachments");
        int oe = 0;
        string category_nameh = string.Empty;
        string sHTML = string.Empty;
        string _pcr = string.Empty;
        string _ViewType = string.Empty;

        string userid = string.Empty;
        if (HttpContext.Current.Session["USER_ID"] != null)
        {
            userid = HttpContext.Current.Session["USER_ID"].ToString();
        }

        //if (userid == "")
        //    userid = "0";
        //if (Convert.ToInt32(HttpContext.Current.Session["PS_SEARCH_RESULTS"].ToString()) > 0)
        //{
        try
        {
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
                HttpContext.Current.Session["PL_VIEW_MODE"] = _ViewType;
            }

            else
            {
                _ViewType = ViewMode;
            }


            //if (HttpContext.Current.Session["iPageNo"] != null)
            //{
            //    iPageNo = Convert.ToInt32(HttpContext.Current.Session["iPageNo"]);
            //}


            //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("product_list.aspx") == true)
            //{
            //    if (HttpContext.Current.Session["RECORDS_PER_PAGE_PRODUCT_LIST"] != null)
            //        iRecordsPerPage = Convert.ToInt32(HttpContext.Current.Session["RECORDS_PER_PAGE_PRODUCT_LIST"].ToString());


            //    EasyAsk.GetAttributeProducts("ProductList", "", _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");

            //}

            //if (HttpContext.Current.Request.QueryString["path"] != null)
            //    _EAPath = HttpUtility.UrlDecode(objSecurity.StringDeCrypt(HttpContext.Current.Request.QueryString["path"].ToString()));

            //if (HttpContext.Current.Session["EA"] != null)
            //    _EAPath = HttpContext.Current.Session["EA"].ToString();


            _EAPath = dscat.Tables["eapath"].Rows[0][0].ToString();


            //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("product_list.aspx"))
            //{
            //    //ps.ClearFilterproduct("DELETE TBWC_SEARCH_PROD_LIST WHERE PRODUCT_ID IN(SELECT DISTINCT F.FAMILY_ID FROM TB_FAMILY F,TB_FAMILY_SPECS FS WHERE F.FAMILY_ID=FS.FAMILY_ID AND ISNULL(FS.STRING_VALUE,'')='NOT FOR WEB' AND F.CATEGORY_ID='" + ps.CATEGORY_ID + "') AND USER_SESSION_ID='" + HttpContext.Current.Session.HttpContext.Current.SessionID + "'");
            //    // dscat = ps.GetFamilies();
            //    dscat = (DataSet)HttpContext.Current.Session["FamilyProduct"];
            //}
            //else
            //{
            //    // dscat = ps.GetProducts();
            //}



            if (dscat != null)
            {

                if (HttpContext.Current.Request.QueryString["pcr"] != null)
                    _pcr = HttpContext.Current.Request.QueryString["pcr"];
                if (HttpContext.Current.Request.QueryString["type"] == null)
                {
                    //GetDataSet("SELECT CATEGORY_NAME  FROM	TB_CATEGORY WHERE	CATEGORY_ID ='" + catID + "'").Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
                    string tempstr = (string)objHelperDB.GetGenericDataDB(_cid, "GET_CATEGORY_NAME", HelperDB.ReturnType.RTString);
                    if (tempstr != null && tempstr != "")
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


                    _stg_container = new StringTemplateGroup("searchrsltproductmain", stemplatepath);
                    _stmpl_container = _stg_container.GetInstanceOf("searchrsltproducts\\advmain");

                    SetEbookAndPDFLink(_stmpl_container);
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
                _stg_records = new StringTemplateGroup("searchrsltproductrecords", stemplatepath);


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

                //DataRow[] drprodcoll = dscat.Tables[2].Select("ATTRIBUTE_ID = 1", "SNO");



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


                tmpProds = "";
                if (Convert.ToInt32(userid) > 0)
                {
                    foreach (DataRow drpid in dscat.Tables["FamilyPro"].Rows)
                    {
                        tmpProds = tmpProds + drpid["PRODUCT_ID"].ToString() + ",";
                    }
                    if (tmpProds != "")
                    {
                        tmpProds = tmpProds.Substring(0, tmpProds.Length - 1);
                       // dsPriceTableAll = objHelperDB.GetProductPriceTableAll(tmpProds, Convert.ToInt32(userid));
                    }
                }
                bool IsEcomEnabled = objHelperServices.GetIsEcomEnabled(userid);

                lstrecords = new TBWDataList[dscat.Tables["FamilyPro"].Rows.Count + 1];
                string CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(userid));
                string cellGV = string.Empty;
                string cellLV = string.Empty;
                cellGV = "searchrsltproducts\\productlist_GridView";
                cellLV = "searchrsltproducts\\productlist_WES";

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
                    BindToST = true;
                    if (_ViewType == "GV")
                        _stmpl_records = _stg_records.GetInstanceOf(cellGV);
                    else
                        _stmpl_records = _stg_records.GetInstanceOf(cellLV + soddevenrow);

                   // string urlDesc = "";

                    _stmpl_records.SetAttribute("FAMILY_ID", drpid["FAMILY_ID"].ToString());
                    _stmpl_records.SetAttribute("CATEGORY_ID", drpid["CATEGORY_ID"].ToString());
                    _stmpl_records.SetAttribute("PARENT_CATEGORY_ID", drpid["PARENT_CATEGORY_ID"].ToString());

                    _stmpl_records.SetAttribute("FAMILY_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath)));

                    _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EAPath + "////UserSearch1=Family Id=" + drpid["FAMILY_ID"].ToString())));

                    _stmpl_records.SetAttribute("TBT_PRODUCT_ID", drpid["PRODUCT_ID"].ToString());
                    _stmpl_records.SetAttribute("PRODUCT_CODE", drpid["PRODUCT_CODE"].ToString());
                    _stmpl_records.SetAttribute("TBT_FAMILY_ID", drpid["FAMILY_ID"].ToString());
                    _stmpl_records.SetAttribute("divcount", drpid["PRODUCT_ID"] + "_" + icnt);
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

<<<<<<< .mine
               
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


                        _stmpl_records.SetAttribute("TBT_USER_PRICE", drpid["PRODUCT_PRICE"].ToString());
                        _stmpl_records.SetAttribute("ATTRIBUTE_ID_5", drpid["PRODUCT_PRICE"].ToString());


                        if (Convert.ToInt32(drpid["QTY_AVAIL"].ToString()) != -1)
                        {
                            if (Convert.ToInt32(drpid["QTY_AVAIL"].ToString()) > 0)
                            {
                                _stmpl_records.SetAttribute("TBT_QTY_AVAIL", drpid["QTY_AVAIL"].ToString());
                                _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", drpid["MIN_ORD_QTY"].ToString());
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
                        _stmpl_records.SetAttribute("TBT_MIN_PRICE", drpid["MIN_PRICE"].ToString());
                    }
                    else
                    {
                        _stmpl_records.SetAttribute("TBT_SUB_FAMILY", false);
                        _stmpl_records.SetAttribute("TBT_BUY_PRODUCT", true);

                        string stk_sta_desc = "";
                        stk_sta_desc = drpid["STOCK_STATUS_DESC"].ToString().Trim();
                        //   objErrorHandler.CreateLog(stk_sta_desc);
                      
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
                            if ((stk_sta_desc.ToUpper().Contains("OUT_OF_STOCK") == true ||stk_sta_desc.ToUpper().Contains("SPECIAL_ORDER") == true  ) && drpid["PROD_SUBSTITUTE"].ToString().Trim() != "" && drpid["PROD_STOCK_FLAG"].ToString().Trim() == "-2")
                            {
                               // DataTable rtntbl = objProductServices.GetSubstituteProductDetails(drpid["PROD_SUBSTITUTE"].ToString().Trim(), Convert.ToInt32(userid));
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



                        _stmpl_records.SetAttribute("TBT_USER_PRICE", drpid["PRODUCT_PRICE"].ToString());
                        _stmpl_records.SetAttribute("ATTRIBUTE_ID_5", drpid["PRODUCT_PRICE"].ToString());
                        

                        if (Convert.ToInt32(drpid["QTY_AVAIL"].ToString()) != -1)
                        {
                            if (Convert.ToInt32(drpid["QTY_AVAIL"].ToString()) > 0)
                            {
                                _stmpl_records.SetAttribute("TBT_QTY_AVAIL", drpid["QTY_AVAIL"].ToString());
                                _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", drpid["MIN_ORD_QTY"].ToString());
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

                    _stmpl_records.SetAttribute("ATTRIBUTE_ID_1", drpid["PRODUCT_CODE"].ToString());

                    System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + drpid["Prod_Thumbnail"].ToString().Replace("/", "\\"));
                    if (Fil.Exists)
                        _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", drpid["Prod_Thumbnail"].ToString());
                    else
                        _stmpl_records.SetAttribute("ATTRIBUTE_ID_747", "/images/noimage.gif");


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
                            _stmpl_records.SetAttribute("PROD_DESC_ALT", drpid["FAMILY_NAME"].ToString());
                        if (desc.Length > 250 && _ViewType == "LV")
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

                        if (desc.Length > 250 && _ViewType == "LV")
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

                        descattr = drpid["Family_ShortDescription"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                        if (descattr.Length > 0)
                            descattr = descattr + "<br/>";
                        descattr = descattr + drpid["Family_Description"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                        descattr = descattr + " " + drpid["Prod_Description"].ToString().Replace("\r", "").Replace("\r\n", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");

                        prod_desc_alt = drpid["Prod_Description"].ToString();
                        if (prod_desc_alt.Length > 0)
                            _stmpl_records.SetAttribute("PROD_DESC_ALT", prod_desc_alt);
                        else
                            _stmpl_records.SetAttribute("PROD_DESC_ALT", drpid["FAMILY_NAME"].ToString());
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
                    //    string descattr = "";
                    //    string desc = "";
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
                    //            desc = dr["STRING_VALUE"].ToString().Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");

                    //            if (dr["ATTRIBUTE_ID"].ToString() == "13" && desc.Length > 0)
                    //                desc = desc + "<br/>";
                    //            // descattr = descattr +" "+ desc;
                    //            descattr = descattr + desc;
                    //            if (desc.Length > 250 && _ViewType == "LV")
                    //            {
                    //                _stmpl_records.SetAttribute("TBT_MORE_" + dr["ATTRIBUTE_ID"].ToString(), true);
                    //                desc = desc.Substring(0, 250).ToString();
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


                    if ( BindToST == true)
                    {
                    lstrows[icolstart] = new TBWDataList(_stmpl_records.ToString());
                    }

                    //lstrows[icolstart] = new TBWDataList(_stmpl_records.ToString());
                    icolstart++;
                    if (icolstart >= icol || oe == dscat.Tables["FamilyPro"].Rows.Count)
                    {
                        _stg_container = new StringTemplateGroup("searchrsltproductcontainer", stemplatepath);
                        _stmpl_container = _stg_container.GetInstanceOf("searchrsltproducts\\producttable_" + soddevenrow);
                        _stmpl_container.SetAttribute("TBWDataList", lstrows);


                        lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
                        sHTML = sHTML + _stmpl_container.ToString();
                        ictrecords++;
                        icolstart = 0;
                        lstrows = new TBWDataList[icol];

                    }
                }

                //   }

                _stg_container = new StringTemplateGroup("searchrsltproductmain", stemplatepath);

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




                if (_ViewType == "LV")
                    _stmpl_container.SetAttribute("TBW_VIEWTYPE", true);
                else
                    _stmpl_container.SetAttribute("TBW_VIEWTYPE", false);








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

                DataSet DSnaprod = new DataSet();

            }
        }
        catch (Exception ex)
        {
            sHTML = ex.Message;
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
        finally
        {
            //if (oCon != null)
            //{
            //oCon.Close();
            //}
        }

        //}
        //string strtophtml = " <table align=\"left\" width=\"775px\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" ><tr><td height=\"126\"  align=left ><TABLE border=0 cellSpacing=0 cellPadding=0 width=\"775px\"><tr><td>";
        //string strbottom = "</td></tr></TABLE></td></tr><tr><td></td></tr></table>";
        sHTML =  sHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>") ;

        //return objHelperServices.StripWhitespace(sHTML);
        return sHTML;
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
            int ictrows = 0;
            string _tsb = string.Empty;
            string _tsm = string.Empty;
            string _type = string.Empty;
            string _value = string.Empty;
            string _bname = string.Empty;
            string _searchstr = string.Empty;
            string url = string.Empty;
            string _byp = "2";
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


            if (_catCid != "")
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
          
            if ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("product_list.aspx")))
            {
                //if (HttpContext.Current.Session["RECORDS_PER_PAGE_PRODUCT_LIST"] != null)
                //    iRecordsPerPage = Convert.ToInt32(HttpContext.Current.Session["RECORDS_PER_PAGE_PRODUCT_LIST"].ToString());
              
                EA = eapath;

                dscat = EasyAsk.GetAttributeProducts("ProductList", "", _type, _value, _bname, irecords, (iPageNo - 1).ToString(), "Next", EA);
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
