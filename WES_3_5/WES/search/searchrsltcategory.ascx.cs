using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradingBell.WebCat;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using StringTemplate = Antlr.StringTemplate.StringTemplate;
using StringTemplateGroup = Antlr.StringTemplate.StringTemplateGroup;

using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class search_searchrsltcategory : System.Web.UI.UserControl
{
    HelperDB oHelper = new HelperDB();
    ErrorHandler oErr = new ErrorHandler();
    DataSet dscat = new DataSet();
    int iCatalogId;
    int iInventoryLevelCheck;
    string stemplatepath = "";
    SqlConnection oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["TBWebCatShoppingCartConnString"].ToString());

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //added string templated path by M/A
             //Request.Url.OriginalString.ToString().Substring(Request.Url.OriginalString.ToString().LastIndexOf("/")+1,Request.Url.OriginalString.ToString().LastIndexOf(".")-1-Request.Url.OriginalString.ToString().LastIndexOf("/"))

            stemplatepath = Server.MapPath(ConfigurationManager.AppSettings["StringTemplatePath"].ToString());
            try
            {
                if(Request.Url.OriginalString.ToString().ToUpper().Contains("PRODUCT_LIST.ASPX"))
                    stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length-1).LastIndexOf('\\')) + "\\product_list\\";
                if (Request.Url.OriginalString.ToString().ToUpper().Contains("BYBRAND.ASPX"))
                    stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\bybrand\\";
                if (Request.Url.OriginalString.ToString().ToUpper().Contains("BYPRODUCT.ASPX"))
                    stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\byproduct\\";
            }
            catch (Exception ec)
            {
            }
            if (IsPostBack)
            {
                GetStoreConfig();
            }
            else
            {
                GetStoreConfig();
            }
        }
        catch (Exception ex)
        {
            oErr.ErrorMsg = ex;
            oErr.CreateLog();
        }
    }

    private void GetStoreConfig()
    {
        try
        {
            //Modify and develop a generic method to get these vars from store config table
            iCatalogId = Convert.ToInt32(Session["CATALOG_ID"].ToString());
            iInventoryLevelCheck = Convert.ToInt32(Session["INVENTORY_LEVEL_CHECK"].ToString());
        }
        catch (Exception ex)
        {
            oErr.ErrorMsg = ex;
            oErr.CreateLog();
        }
    }

    protected string ST_Categories()
    {        
            string category_nameh = "";
            if (Session["PS_SEARCH_RESULTS"] == null)
            {
                return "";
            }
            StringTemplateGroup _stg_container = null;
            StringTemplateGroup _stg_records = null;
            StringTemplate _stmpl_container = null;
            StringTemplate _stmpl_records = null;
            StringTemplate _stmpl_recordsma = null;
            StringTemplate _stmpl_recordsa = null;

            string sHTML = "";
            if (Convert.ToInt32(Session["PS_SEARCH_RESULTS"].ToString()) > 0)
            {
                try
                {
                    oCon.Open();
                    PowerSearch ps = new PowerSearch(oCon);
                    ps.USER_SESSION_ID = Session.SessionID;
                    ps.CATALOG_ID = iCatalogId;
                    ps.INVENTORY_CHECK = iInventoryLevelCheck;
                    string sCategoryIds = "";

                    if (Request.QueryString["__EVENTTARGET"] == "CATEGORYFILTER" && Request.QueryString["__EVENTARGUMENT"] != null)
                    {
                        sCategoryIds = Request.QueryString["__EVENTARGUMENT"].ToString();
                        ps.CATEGORY_ID = sCategoryIds;
                    }
                    else if (Request.QueryString["cyid"] != null)
                    {
                        if (Request.QueryString["cyid"].ToString().Length > 0)
                        {
                            ps.CATEGORY_ID = Request.QueryString["cyid"].ToString();
                        }
                    }
                    else if (Request.QueryString["cid"] != null)
                    {
                        if (Request.QueryString["cid"].ToString().Length > 0)
                        {
                            ps.CATEGORY_ID = Request.QueryString["cid"].ToString();
                            category_nameh = ps.GetCategoryname();
                        }
                    }
                    if (!(Request.Url.OriginalString.ToString().ToUpper().Contains("POWERSEARCH")))
                    ps.ClearFilterproduct("delete tbwc_search_prod_list where product_id in(select distinct spl.product_id from tbwc_search_prod_list spl,tbwc_inventory I where I.product_id<>spl.product_id and spl.user_session_id='" + Session.SessionID + "')");
                    dscat = ps.GetCategories();
                    try
                    {
                        if (Request.Url.OriginalString.ToString().Substring(Request.Url.OriginalString.ToString().LastIndexOf("/") + 1, Request.Url.OriginalString.ToString().LastIndexOf(".") - 1 - Request.Url.OriginalString.ToString().LastIndexOf("/")) == "product_list")
                            dscat = ps.GetCategoriespl();
                        if (Request.Url.OriginalString.ToString().Substring(Request.Url.OriginalString.ToString().LastIndexOf("/") + 1, Request.Url.OriginalString.ToString().LastIndexOf(".") - 1 - Request.Url.OriginalString.ToString().LastIndexOf("/")).Contains("bybrand"))
                            dscat = ps.GetCategoriespl();
                        if (Request.Url.OriginalString.ToString().Substring(Request.Url.OriginalString.ToString().LastIndexOf("/") + 1, Request.Url.OriginalString.ToString().LastIndexOf(".") - 1 - Request.Url.OriginalString.ToString().LastIndexOf("/")) == "byproduct")
                            dscat = ps.GetCategoriespl();
                    }
                    catch (Exception ec)
                    {
                    }

                    TBWDataList[] lstrecords = new TBWDataList[0];
                    TBWDataList[] lstrecordsa = new TBWDataList[0];
                    TBWDataList[] lstrows = new TBWDataList[0];
                    _stg_records = new StringTemplateGroup("searchrsltcategoryrecords", stemplatepath);

                    lstrecords = new TBWDataList[dscat.Tables[0].Rows.Count + 1];
                    int ictrecords = 0;
                    int ictrecordsa = 0;
                    int icolstart = 0;
                    int icol = 1;
                    lstrows = new TBWDataList[icol];
                    if (dscat.Tables[0].Rows.Count < icol)
                    {
                        icol = dscat.Tables[0].Rows.Count;
                    }

                    string attrvalues = "";
                    string allvalues = "";
                    string attrname = "";
                    int attrid = 0;

                    if (Request.QueryString["val"] != null && Request.QueryString["aid"] != null)
                    {
                        //ps.FILTER_STR 4^American Scholar|339^Blue
                        ps.FILTER_STR = Request.QueryString["aid"].ToString() + "^" + Request.QueryString["val"].ToString();
                        ps.ApplyParametricFilters();
                    }
                    DataSet dstp = ps.GetParametricFilters();
                    if (dstp != null && dstp.Tables.Count > 0)
                    {
                        if (dstp != null && dstp.Tables.Count > 0 && dstp.Tables[3].Rows.Count != 0 && stemplatepath.Contains("product_list"))
                        {
                            attrname = dstp.Tables[3].Rows[0].ItemArray[3].ToString();

                            foreach (DataRow dr in dstp.Tables[3].Rows)
                            {
                                if (attrname != dr["ATTRIBUTE_NAME"].ToString())
                                {
                                    _stmpl_recordsma = _stg_records.GetInstanceOf("searchrsltcategory" + "\\" + "mainattribute");
                                    _stmpl_recordsma.SetAttribute("TBT_ECOMENABLED", (Convert.ToInt16(Session["USER_ROLE"]) < 4 ? true : false));
                                    _stmpl_recordsma.SetAttribute("TBWDataList", attrvalues);
                                    _stmpl_recordsma.SetAttribute("TBW_ATTRIBUTE_NAME", attrname);
                                    allvalues = allvalues + _stmpl_recordsma.ToString();
                                    attrvalues = "";

                                    attrname = dr["ATTRIBUTE_NAME"].ToString();
                                    attrid = Convert.ToInt32(dr["ATTRIBUTE_ID"].ToString());
                                    _stmpl_recordsa = _stg_records.GetInstanceOf("searchrsltcategory" + "\\" + "attribute");
                                    _stmpl_recordsa.SetAttribute("TBT_ECOMENABLED", (Convert.ToInt16(Session["USER_ROLE"]) < 4 ? true : false));
                                    _stmpl_recordsa.SetAttribute("TBW_STRING_VALUE", dr["STRING_VALUE"].ToString());
                                    _stmpl_recordsa.SetAttribute("TBW_PRODUCT_COUNT", dr["PRODUCT_COUNT"].ToString());
                                    _stmpl_recordsa.SetAttribute("TBW_CATEGORY_ID", ps.CATEGORY_ID);
                                    _stmpl_recordsa.SetAttribute("TBW_ATTRIBUTE_ID", attrid);
                                    attrvalues = attrvalues + _stmpl_recordsa.ToString();
                                }
                                else
                                {
                                    attrname = dr["ATTRIBUTE_NAME"].ToString();
                                    attrid = Convert.ToInt32(dr["ATTRIBUTE_ID"].ToString());
                                    _stmpl_recordsa = _stg_records.GetInstanceOf("searchrsltcategory" + "\\" + "attribute");
                                    _stmpl_recordsa.SetAttribute("TBT_ECOMENABLED", (Convert.ToInt16(Session["USER_ROLE"]) < 4 ? true : false));
                                    _stmpl_recordsa.SetAttribute("TBW_STRING_VALUE", dr["STRING_VALUE"].ToString());
                                    _stmpl_recordsa.SetAttribute("TBW_PRODUCT_COUNT", dr["PRODUCT_COUNT"].ToString());
                                    _stmpl_recordsa.SetAttribute("TBW_CATEGORY_ID", ps.CATEGORY_ID);
                                    _stmpl_recordsa.SetAttribute("TBW_ATTRIBUTE_ID", attrid);
                                    attrvalues = attrvalues + _stmpl_recordsa.ToString();
                                }
                            }

                            _stmpl_recordsma = _stg_records.GetInstanceOf("searchrsltcategory" + "\\" + "mainattribute");
                            _stmpl_recordsma.SetAttribute("TBT_ECOMENABLED", (Convert.ToInt16(Session["USER_ROLE"]) < 4 ? true : false));
                            _stmpl_recordsma.SetAttribute("TBWDataList", attrvalues);
                            _stmpl_recordsma.SetAttribute("TBW_ATTRIBUTE_NAME", attrname);
                            allvalues = allvalues + _stmpl_recordsma.ToString();
                            attrvalues = "";
                        }
                        foreach (DataRow dr in dscat.Tables[0].Rows)
                        {
                            _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategory" + "\\" + "categories");
                            _stmpl_records.SetAttribute("TBT_ECOMENABLED", (Convert.ToInt16(Session["USER_ROLE"]) < 4 ? true : false));
                            _stmpl_records.SetAttribute("TBW_CATEGORY_NAME", dr["CATEGORY_NAME"].ToString());
                            _stmpl_records.SetAttribute("TBW_CATEGORY_NAME_WITH_COUNT", dr["CATEGORY_NAME_WITH_COUNT"].ToString());
                            _stmpl_records.SetAttribute("TBW_CATEGORY_ID", dr["CATEGORY_ID"].ToString());
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                            ictrecords++;

                            //if (icolstart == icol)
                            //{
                            //    _stg_container = new StringTemplateGroup("searchrsltcategorycontaner", stemplatepath);
                            //    _stmpl_container = _stg_container.GetInstanceOf("searchrsltcategory" + "\\" + "categorycontainer");
                            //    _stmpl_container.SetAttribute("TBWDataList", lstrows);

                            //    lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
                            //    ictrecords++;
                            //    icolstart = 0;
                            //}
                        }

                        _stg_container = new StringTemplateGroup("searchrsltcategorymain", stemplatepath);
                        _stmpl_container = _stg_container.GetInstanceOf("searchrsltcategory" + "\\" + "main");
                        _stmpl_container.SetAttribute("TBT_ECOMENABLED", (Convert.ToInt16(Session["USER_ROLE"]) < 4 ? true : false));
                        _stmpl_container.SetAttribute("TBW_CATEGORY_NAME", category_nameh);
                        _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                        _stmpl_container.SetAttribute("TBWDataListForAttributes", allvalues);

                        if (dscat.Tables[0].Rows.Count == 0 && dstp.Tables[3].Rows.Count == 0)
                        {
                            sHTML = _stmpl_container.ToString().Replace("<br /><strong>Product Category:</strong><br>", "");
                        }
                        else
                        {
                            if (dscat.Tables[0].Rows.Count == 0)
                            {
                                sHTML = _stmpl_container.ToString().Replace("<br /><strong>Product Category:</strong><br>", "");
                            }
                            else
                                sHTML = _stmpl_container.ToString();
                        }
                    }
                    if (dscat.Tables[0].Rows.Count == 0 && Page.Request.Url.ToString().ToLower().Contains("bybrand.aspx") == false && Page.Request.Url.ToString().ToLower().Contains("byproduct.aspx") == false && Page.Request.Url.ToString().ToLower().Contains("product_list.aspx") == false)
                        sHTML = "";
                }
                catch (Exception ex)
                {
                    sHTML = ex.Message;
                }
                finally
                {
                    if (oCon != null)
                    {
                        oCon.Close();
                    }
                }
            }
            return sHTML;
       
    }
}
