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
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class search_search : System.Web.UI.UserControl
{
    string _searchStr = string.Empty;
    HelperServices objHelperServices = new HelperServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    ConnectionDB objConnectionDB = new ConnectionDB();
    //SqlConnection oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["TBWebCatShoppingCartConnString"].ToString());
    SqlDataReader psrdr = null;
    int iCatalogId;
    string sCategoryId = string.Empty;
    int iInventoryLevelCheck;
    int iRecordsPerPage;
    bool bIsStartOver;
    string sSortBy = string.Empty;
    bool bDoPaging;
    int iPageNo = 1;
    bool ExistStrChk = false;
    string breadcrumb = string.Empty;
    EasyAsk_WES EasyAsk = new EasyAsk_WES();
    Security objSecurity = new Security();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            try
            {
                try
                {
                    if (Request.QueryString["ld"] != null && Request.QueryString["ld"].ToString() != "")
                    {
                        Session["filter"] = null;
                    }
                }
                catch (Exception ex)
                {

                }

                try
                {
                    if (Request.QueryString["src"] != null && Request.QueryString["src"].ToString() != "" && !IsPostBack)
                    {
                        txtSearch.Text = HttpUtility.HtmlDecode(Request.QueryString["src"].ToString());

                    }
                }
                catch (Exception) { }


                if (Request.QueryString["srctext"] != null && Request.QueryString["srctext"].ToString() != "" && !IsPostBack)
                {
                    txtSearch.Text = HttpUtility.HtmlDecode(Request.QueryString["srctext"].ToString());

                }


            }
            catch (Exception ex)
            {

            }
            GetStoreConfig();
            GetPageConfig();
            //LoadCatList();
            //ddlcategory_DataBound();
            //ddlsubcategory_DataBound();
            if (IsPostBack)
            {
                //this checking is used to ovoid unneccesary tasks if the user clicked clear button
                if (hdnForClear.Value != "CLEAR")
                {
                    //chkSearchWithin.Visible = true;
                    //Added by M/A on sep 27 09
                    Session["PS_SEARCH_STR"] = txtSearch.Text;
                    lblSearchError.Text = "";
                    //bIsStartOver = !chkSearchWithin.Checked;
                   // Session["PS_IS_START_OVER"] = chkSearchWithin.Checked ? "NO" : "YES";
                }
            }
            else
            {

                if (txtSearch.Text != "")
                {
                    //chkSearchWithin.Visible = false;
                    //Added by M/A on sep 27 09
                    Session["PS_SEARCH_STR"] = txtSearch.Text;
                    lblSearchError.Text = "";
                    //bIsStartOver = !chkSearchWithin.Checked;
                    //Session["PS_IS_START_OVER"] = chkSearchWithin.Checked ? "NO" : "YES";
                }
                else
                {
                    Session["PS_SEARCH_STR"] = "";
                    //bIsStartOver = chkSearchWithin.Checked;
                    //Session["PS_IS_START_OVER"] = chkSearchWithin.Checked ? "NO" : "YES";
                    //chkSearchWithin.Visible = false;
                }
            }
          // ExecutePowerSearch();
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }

        txtSearch.Attributes.Add("onkeypress", "javascript:return blockspecialcharacters(event);");
        
    }

    private void ddlcategory_DataBound()
    {
        DataSet dscat = new DataSet();
        DataSet breadcrumb_ct = new DataSet();
        DataRow[] _DCRow = null;
        string catname = string.Empty;
        string href = string.Empty;
        string _searchstr = string.Empty;
        string eapath = string.Empty;
        int i = 0;

        //dscat = (DataSet)HttpContext.Current.Session["LHSAttributes"];
        //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("powersearch.aspx") == true && dscat != null)
        //{
        //    if (HttpContext.Current.Session["BreadCrumbDS"] != null)
        //    {
        //        breadcrumb_ct = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];
        //    }
        //    _DCRow = dscat.Tables[0].Select();
        //    if (_DCRow != null && _DCRow.Length > 0)
        //    {
        //        foreach (DataRow _drow in _DCRow)
        //        {
        //            if (breadcrumb_ct != null)
        //                eapath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(breadcrumb_ct.Tables[0].Rows[0]["EAPath"].ToString() + "////" + _drow["Category_Name"].ToString()));
        //            href = "powersearch.aspx?&id=0&searchstr=" + HttpUtility.UrlEncode(_drow["SearchString"].ToString()) + "&type=" + dscat.Tables[0].TableName.ToString() + "&value=" + HttpUtility.UrlEncode(_drow["Category_Name"].ToString()) + "&byp=2&Path=" + eapath;
        //            catname = _drow["Category_Name"].ToString();
        //            ddlcategory.Items.Insert(i, new ListItem(catname, href));
        //            i++;
        //        }
        //    }
        //}
        //else
        //{
        string _type = string.Empty;
        string _value = string.Empty;
        string _bname = string.Empty;
         

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


            iRecordsPerPage = 16;
            string iRecordsPerPage_lhs="16";
            //EasyAsk.GetAttributeProducts("PowerSearch", _searchstr, _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
           // dscat = (DataSet)HttpContext.Current.Session["LHSAttributes"];


             dscat = (DataSet)HttpContext.Current.Session["LHSCMAttributes"];
             if (dscat != null)
                 dscat = (DataSet)HttpContext.Current.Session["LHSCMAttributes"];
             else
             {
                 EasyAsk.GetDDLCategorymain(_searchstr, iRecordsPerPage_lhs);
                 dscat = (DataSet)HttpContext.Current.Session["LHSCMAttributes"];
             }
            //EasyAsk.GetDDLCategory("PowerSearch", _searchstr, _type, _value, _bname, iRecordsPerPage_lhs, (iPageNo - 1).ToString(), "Next", "");
           
            if (dscat != null)
            {
                if (HttpContext.Current.Session["BreadCrumbDS"] != null)
                {
                    breadcrumb_ct = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];
                }
          
               //powersearch.aspx?&amp;id=0&amp;searchstr=$TBW_ATTRIBUTE_SEARCH$&amp;type=$TBW_ATTRIBUTE_TYPE$&amp;value=$TBW_ATTRIBUTE_VALUE$&amp;bname=$TBW_ATTRIBUTE_BRAND$&amp;byp=$TBW_CUSTOM_NUM_FIELD3$&amp;Path=$EA_PATH$"
                _DCRow = dscat.Tables[0].Select();
                
                if (_DCRow != null && _DCRow.Length > 0)
                {
                    ddlcategory.Items.Insert(0, new ListItem("Select Category", "Select Category"));
                   // ddlcategory.SelectedIndex = ddlcategory.Items.IndexOf(new ListItem("Select Category", "Select Category"));
                    foreach (DataRow _drow in _DCRow)
                    {
                        if (breadcrumb_ct != null && breadcrumb_ct.Tables[0].Rows.Count > 0)
                            eapath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(breadcrumb_ct.Tables[0].Rows[0]["EAPath"].ToString() + "////" + _drow["Category_Name"].ToString()));
                        else
                         eapath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt( "AllProducts////WESAUSTRALASIA////UserSearch1=" + _searchstr + "////" + _drow["Category_Name"].ToString()));
                        href = "powersearch.aspx?&id=0&searchstr=" + HttpUtility.UrlEncode(_drow["SearchString"].ToString()) + "&type=" + dscat.Tables[0].TableName.ToString() + "&value=" + HttpUtility.UrlEncode(_drow["Category_Name"].ToString()) + "&byp=2&Path=" + eapath;
                        catname = _drow["Category_Name"].ToString();
                       // if (breadcrumb_ct.Tables[0].Rows.Count > 1)
                      //  {
                            string catvalue = ddlcategory.SelectedItem.Text;
                            string hiddvalue = cv.Value;
                            if (catvalue == _drow["Category_Name"].ToString())
                            {
                                ddlcategory.Items.Insert(0, new ListItem(catname, href));
                            }
                            else
                            {
                                ddlcategory.Items.Insert(i, new ListItem(catname, href));
                            }
                       // }
                        //else
                        //{
                        //    ddlcategory.Items.Insert(i, new ListItem(catname, href));
                        //}
                        i++;
                    }
                }

               // ddlcategory.Items.Insert(0, new ListItem("Select Category", "0"));
               // ddlcategory.Items.Insert(1, new ListItem("home", "home.aspx"));
                //powersearch.aspx?&id=0&searchstr=mp3&type=Category&value=Audio+Speakers+%26+PA&bname=&byp=2&Path=K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0DxMJ2DOPpWgOU%3d
                //string href = "bybrand.aspx?&Id=0&pcr=" + HttpUtility.UrlEncode(tempCID) + "&cid=" + HttpUtility.UrlEncode(_drow["CATEGORY_ID"].ToString()) + "&searchstr=&bname=" + HttpUtility.UrlEncode(_drow["brandvalue"].ToString()) + "&tsb=" + Server.UrlEncode(Request.QueryString["tsb"].ToString()) + "&value=" + HttpUtility.UrlEncode(_drow["Category_Name"].ToString()) + "&type=Category&tsm=" + HttpUtility.UrlEncode(_tsm) + "&byp=2&path=" + eapath;
            }
       // }
    }

    protected void itemSelected(object sender, EventArgs e)
    {
        string catvalue = ddlcategory.SelectedItem.Text;
        string valuecat = ddlcategory.SelectedValue;
        ddlsubcategory_DataBound();
        Response.Redirect(valuecat);
    }

    private void ddlsubcategory_DataBound()
    {

        DataSet dscat = new DataSet();
        DataSet breadcrumb_ct = new DataSet();
        DataRow[] _DCRow = null;
        string catname = string.Empty;
        string href = string.Empty;
        string _searchstr = string.Empty;
        string eapath = string.Empty;
        int i = 0;
        //dscat = (DataSet)HttpContext.Current.Session["LHSAttributes"];
        //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("powersearch.aspx") == true && dscat != null)
        //{
        //    if (HttpContext.Current.Session["BreadCrumbDS"] != null)
        //    {
        //        breadcrumb_ct = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];
        //    }
        //    _DCRow = dscat.Tables[0].Select();
        //    if (_DCRow != null && _DCRow.Length > 0)
        //    {
                
        //        if (_DCRow != null && _DCRow.Length > 0)
        //        {
        //            foreach (DataRow _drow in _DCRow)
        //            {
        //                if (breadcrumb_ct != null)
        //                    eapath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(breadcrumb_ct.Tables[0].Rows[0]["EAPath"].ToString() + "////" + _drow["Category_Name"].ToString()));
        //                href = "powersearch.aspx?&id=0&searchstr=" + HttpUtility.UrlEncode(_drow["SearchString"].ToString()) + "&type=" + dscat.Tables[0].TableName.ToString() + "&value=" + HttpUtility.UrlEncode(_drow["Category_Name"].ToString()) + "&byp=2&Path=" + eapath;
        //                catname = _drow["Category_Name"].ToString();
        //                ddlsubcategory.Items.Insert(i, new ListItem(catname, href));
        //                i++;
        //            }
        //        }
        //    }
        //}
       // else
       // {

        string _type = string.Empty;
        string _value = string.Empty;
        string _bname = string.Empty;
        

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


            iRecordsPerPage = 16;
            string iRecordsPerPage_lhs = "16";
            //EasyAsk.GetAttributeProducts("PowerSearch", _searchstr, _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
            // dscat = (DataSet)HttpContext.Current.Session["LHSAttributes"];
       // "PowerSearch", _searchstr, _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
            EasyAsk.GetDDLCategory("PowerSearch", _searchstr, _type, _value, _bname, iRecordsPerPage_lhs, (iPageNo - 1).ToString(),"Next","");
            dscat = (DataSet)HttpContext.Current.Session["LHSCAttributes"];
            if (dscat != null)
            {
                if (HttpContext.Current.Session["BreadCrumbDS"] != null)
                {
                    breadcrumb_ct = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];
                }
                //powersearch.aspx?&amp;id=0&amp;searchstr=$TBW_ATTRIBUTE_SEARCH$&amp;type=$TBW_ATTRIBUTE_TYPE$&amp;value=$TBW_ATTRIBUTE_VALUE$&amp;bname=$TBW_ATTRIBUTE_BRAND$&amp;byp=$TBW_CUSTOM_NUM_FIELD3$&amp;Path=$EA_PATH$"
                _DCRow = dscat.Tables[0].Select();
           
                if (_DCRow != null && _DCRow.Length > 0)
                {
                    ddlsubcategory.Items.Insert(0, new ListItem("Select Category", "Select Category"));
                    foreach (DataRow _drow in _DCRow)
                    {
                        if (breadcrumb_ct != null)
                            eapath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(breadcrumb_ct.Tables[0].Rows[0]["EAPath"].ToString() + "////" + _drow["Category_Name"].ToString()));
                        href = "powersearch.aspx?&id=0&searchstr=" + HttpUtility.UrlEncode(_drow["SearchString"].ToString()) + "&type=" + dscat.Tables[0].TableName.ToString() + "&value=" + HttpUtility.UrlEncode(_drow["Category_Name"].ToString()) + "&byp=2&Path=" + eapath;
                        catname = _drow["Category_Name"].ToString();
                        ddlsubcategory.Items.Insert(i, new ListItem(catname, href));
                        i++;
                    }
                }

               }
       // }
    }

    //private void ExecutePowerSearch()
    //{
    //    try
    //    {
    //        //oCon.Open();
    //       // PowerSearch ps = new PowerSearch(oCon);
    //        PowerSearchServices ps = new PowerSearchServices();
    //        ps.USER_SESSION_ID = Session.SessionID;
    //        ps.CATALOG_ID = iCatalogId;
    //        ps.CATEGORY_ID = sCategoryId;
    //        ps.CATEGORY_ID = "";
    //        if (!IsPostBack)
    //            if (Request.QueryString["cyid"] != null)
    //            {
    //                if (Request.QueryString["cyid"].ToString().Length > 0)
    //                {
    //                    ps.CATEGORY_ID = Request.QueryString["cyid"].ToString();
    //                }
    //            }
    //            else if (Request.QueryString["cid"] != null)
    //            {
    //                if (Request.QueryString["cid"].ToString().Length > 0)
    //                {
    //                    ps.CATEGORY_ID = Request.QueryString["cid"].ToString();
    //                }
    //            }
    //        ps.SEARCH_STR = Session["PS_SEARCH_STR"].ToString();
    //        ps.FILTER_STR = "";
    //        ps.INVENTORY_CHECK = iInventoryLevelCheck;
    //        ps.USE_PARAMETRIC_FILTER = false;
    //        ps.IS_START_OVER = bIsStartOver;
    //        Session["PS_SEARCH_RESULTS"] = Convert.ToString(ps.ExecutePowerSearch());

    //        if (Request.QueryString["cyid"] != null)
    //        {
    //            if (Request.QueryString["cyid"].ToString().Length > 0)
    //            {
    //                ps.CATEGORY_ID = Request.QueryString["cyid"].ToString();
    //                Session["PS_SEARCH_RESULTS"] = Convert.ToString(ps.ExecutePowerSearch());
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        objErrorHandler.ErrorMsg = ex;
    //        objErrorHandler.CreateLog();
    //    }
    //    finally
    //    {
    //        //if (oCon != null)
    //        //{
    //        //    oCon.Close();
    //        //}
    //    }
    //}

    //public string Bread_Crumbs()
    //{
    //    string breadcrumb = "", paraPID = "", paraFID = "", paraCID = "";
    //    if (Request.QueryString["pid"] != "null")
    //    {
    //        paraPID = Request.QueryString["pid"].ToString();
    //    }
    //    if (Request.QueryString["fid"] != "null")
    //        paraFID = Request.QueryString["pid"].ToString();
    //    if (Request.QueryString["cid"] != null)
    //        paraCID = Request.QueryString["cid"].ToString();

    //    if (paraPID != "")
    //    {
    //        DataSet DSBC = null;

    //        DSBC = GetDataSet("SELECT String_value FROM TB_prod_specs WHERE product_ID = " + paraPID + "and attribute_id=1");
    //        foreach (DataRow DR in DSBC.Tables[0].Rows)
    //        {
    //            breadcrumb = DR[0].ToString();
    //        }
    //        if (paraFID != "")
    //        {
    //            string catIDtemp = "";
    //            DSBC = GetDataSet("SELECT family_name,category_id FROM TB_family WHERE family_ID = " + paraFID);
    //            foreach (DataRow DR in DSBC.Tables[0].Rows)
    //            {
    //                breadcrumb = DR[0].ToString() + " / " + breadcrumb;
    //                catIDtemp = DR[1].ToString();
    //            }
    //            do
    //            {
    //                DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
    //                foreach (DataRow DR in DSBC.Tables[0].Rows)
    //                {
    //                    breadcrumb = DR["CATEGORY_NAME"].ToString() + " / " + breadcrumb;
    //                    catIDtemp = DR["PARENT_CATEGORY"].ToString();
    //                }
    //            } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
    //        }
    //    }
    //    else if (paraFID != "")
    //    {
    //        DataSet DSBC = null;
    //        string catIDtemp = "";
    //        DSBC = GetDataSet("SELECT family_NAME,category_id FROM TB_family WHERE family_ID = " + paraFID);
    //        foreach (DataRow DR in DSBC.Tables[0].Rows)
    //        {
    //            breadcrumb = DR[0].ToString();
    //            catIDtemp = DR[1].ToString();
    //        }
    //        do
    //        {
    //            DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
    //            foreach (DataRow DR in DSBC.Tables[0].Rows)
    //            {
    //                breadcrumb = DR["CATEGORY_NAME"].ToString() + " / " + breadcrumb;
    //                catIDtemp = DR["PARENT_CATEGORY"].ToString();
    //            }
    //        } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
    //    }
    //    else if (paraCID != "")
    //    {
    //        DataSet DSBC = null;
    //        string catIDtemp = paraCID;
    //        do
    //        {
    //            DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
    //            foreach (DataRow DR in DSBC.Tables[0].Rows)
    //            {
    //                if (breadcrumb == "")
    //                    breadcrumb = DR["CATEGORY_NAME"].ToString();
    //                else
    //                    breadcrumb = DR["CATEGORY_NAME"].ToString() + " / " + breadcrumb;
    //                catIDtemp = DR["PARENT_CATEGORY"].ToString();
    //            }
    //        } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
    //    }
    //    return breadcrumb;
    //}

    //private DataSet GetDataSet(string SQLQuery)
    //{
    //    DataSet ds = new DataSet();
    //    SqlDataAdapter da = new SqlDataAdapter(SQLQuery, oCon.ConnectionString.Substring(oCon.ConnectionString.IndexOf(';') + 1));
    //    da.Fill(ds, "generictable");
    //    return ds;
    //}
    private void GetStoreConfig()
    {
        try
        {
            //Modify and develop a generic method to get these vars from store config table
            //iCatalogId = Convert.ToInt32(Session["CATALOG_ID"].ToString());
            //iInventoryLevelCheck = Convert.ToInt32(Session["INVENTORY_LEVEL_CHECK"].ToString());

        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }

    private void GetPageConfig()
    {
        try
        {
            //Changes this to get from page settings
            //iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE"].ToString());

            //if (HidItemPage.Value.ToString() == string.Empty || HidItemPage.Value.ToString() == null)
            //{
            //    if (Session["RECORDS_PER_PAGE_POWERSEARCH"] != null)
            //        iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_POWERSEARCH"].ToString());
            //}
            //else
            //{
            //    iRecordsPerPage = Convert.ToInt32(HidItemPage.Value.ToString());
            //    Session["RECORDS_PER_PAGE_POWERSEARCH"] = HidItemPage.Value.ToString();
            //}
            //bDoPaging = Convert.ToBoolean(Session["DO_PAGING"].ToString());
            //iPageNo = 1;
            //sSortBy = "";
            //iCatalogId = Convert.ToInt32(Session["CATALOG_ID"].ToString());
        }
        catch (Exception ex)
        {
            //objErrorHandler.ErrorMsg = ex;
           // objErrorHandler.CreateLog();
        }
    }

    protected void btnsearch_Click(object sender, EventArgs e)
    {
        try
        {
           
            if (txtSearch.Text.Trim() != "")
            {
                Response.Redirect("powersearch.aspx?&srctext=" + txtsearchhidden.Value.ToString().Replace("#","%23").Replace("&","%26").Replace(" ","%20").Replace("+","%2B").Replace("#", "%23").Replace("&", "%26"), false);
                
            }
            else
            {
                brnClear_Click(sender, e);
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }

    protected void brnClear_Click(object sender, EventArgs e)
    {
        try
        {
            //clearing search result
            txtSearch.Text = "";
            //oCon.Open();
            PowerSearchServices ps = new PowerSearchServices();
            ps.USER_SESSION_ID = Session.SessionID;
            int iNoOfEffectedRecords = ps.ClearSearchResults();
            hdnForClear.Value = "";
            Response.Redirect("powersearch.aspx?&srctext=");
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    //public string Bread_Crumbs()
    //{

    //    breadcrumb = EasyAsk.GetBreadCrumb(Server.MapPath("Templates"));
    //    return breadcrumb;
    //}
    public string Spell_Correction()
    {
        string SpellCorrection = string.Empty;
        if (HttpContext.Current.Session["Spell_Correction"] != null || HttpContext.Current.Session["Spell_Correction"] == "")
        {
            SpellCorrection = "<div class='alert yellowbox icon_3' style='background-color:#FFD52B;margin-top:-1px;' >" + HttpContext.Current.Session["Spell_Correction"].ToString() + "</div>";
        }
        return SpellCorrection;
    }
}
