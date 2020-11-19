using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using TradingBell.Common;
using TradingBell.WebServices;
using TradingBell5.CatalogX;
using StringTemplate = Antlr.StringTemplate.StringTemplate;
using StringTemplateGroup = Antlr.StringTemplate.StringTemplateGroup;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class search_searchbybrand : System.Web.UI.UserControl
{
    string stemplatepath = "";
    string tempCID = "";
    string tempCName = "";
    ConnectionDB conStr = new ConnectionDB();
    ErrorHandler oErr;
    Category oCat;
    ProductFamily oPF;
    ProductRender oPR;
    HelperDB oHelper;
    int ictrecords = 0;
    EasyAsk_WES EasyAsk = new EasyAsk_WES(); 
    protected void Page_Load(object sender, EventArgs e)
    {
        stemplatepath = Server.MapPath(ConfigurationManager.AppSettings["StringTemplatePath"].ToString());
        try
        {
            if (Request.Url.OriginalString.ToString().ToUpper().Contains("PRODUCT_LIST"))
                stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\product_list\\";
            if (Request.Url.OriginalString.ToString().ToUpper().Contains("BYBRAND.ASPX") || Request.Url.OriginalString.ToString().ToUpper().Contains("CATEGORYLIST.ASPX") || Request.Url.OriginalString.ToString().ToUpper().Contains("PRODUCT_LIST.ASPX"))
                stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\bybrand\\";
            //if (Request.Url.OriginalString.ToString().ToUpper().Contains("BYPRODUCT.ASPX") || Request.Url.OriginalString.ToString().ToUpper().Contains("CATEGORYLIST.ASPX") || Request.Url.OriginalString.ToString().ToUpper().Contains("PRODUCT_LIST.ASPX"))
                //stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\byproduct\\";

              
        }
        catch (Exception ex)
        {
        }
    }

    protected void LoadBrandMode()
    {
        string _catID = Request["cid"].ToString();
        if (_catID == "WES0389")
        {
            dd1.ToolTip = "Categories";
            dd1.Items.Add(new ListItem("Select Category", ""));
            GetCategories(dd1);
            dd2.ToolTip = "Brands";
            dd2.Items.Add(new ListItem("Select Brand"));
            GetBrands(dd2);
            dd3.ToolTip = "Models";
            dd3.Items.Add(new ListItem("List All Models"));
            GetModels(dd3);
        }
        else
        {
            dd1.Items.Add(new ListItem("Select Brand"));
            dd1.ToolTip = "Brands";
            GetBrands(dd1);
            dd2.Items.Add(new ListItem("List All Models"));
            dd2.ToolTip = "Models";
            GetModels(dd2);
            //dd3.Items.Add(new ListItem("List All Products"));
            //dd3.ToolTip = "Categories";
            //GetCategories(dd3);
            dd3.Visible = false;
        }
    }
    private void GetCategories(DropDownList oDropdown)
    {
        oHelper = new HelperDB();
        string _catID = Request["cid"].ToString();
        string _SelValue = "";
        DataSet oDs = null;
        if (_catID == "WES0389")
        {
            if (!(string.IsNullOrEmpty(Request["sl1"]) && string.IsNullOrEmpty(Request["sl2"])))
            {
                _SelValue = Request["sl1"].ToString();
                if (Request["sl2"].ToString().Trim() != "0")
                _SelValue += "~" + Request["sl2"];
            }
            //string sSQL = string.Format("select distinct subcatid_l1, subcatname_l1, subcatid_l2, subcatname_l2 from VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT where CATEGORY_ID = '{0}'", _catID);
            string sSQL = string.Format("select distinct tsbp.subcatid_l1, tsbp.subcatname_l1, tsbp.subcatid_l2, tsbp.subcatname_l2 from VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT tsbp, tb_category tc where tsbp.subcatid_l1 = tc.CATEGORY_ID and ISNULL(TC.CUSTOM_NUM_FIELD3,0)<> 3 and tsbp.CATEGORY_ID ='{0}'", _catID);
            oHelper.SQLString = sSQL;
            oDs = oHelper.GetDataSet();
        }
        else
        {
            if (!(string.IsNullOrEmpty(Request["tsb"]) && string.IsNullOrEmpty(Request["tsm"])))
            {
                //string sSQL = string.Format("select distinct subcatid_l1, subcatname_l1, subcatid_l2, subcatname_l2 from VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT where CATEGORY_ID = '{0}' and tosuite_brand='{1}' and tosuite_model='{2}' order by subcatname_l1", _catID, Request["tsb"].ToString(), Request["tsm"].ToString());
                string sSQL = string.Format("select distinct tsbp.subcatid_l1, tsbp.subcatname_l1, tsbp.subcatid_l2, tsbp.subcatname_l2 from VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT tsbp, tb_category tc where tsbp.CATEGORY_ID = '{0}' and tsbp.tosuite_brand='{1}' and tsbp.tosuite_model='{2}' and tsbp.subcatid_l1 = tc.CATEGORY_ID and ISNULL(TC.CUSTOM_NUM_FIELD3,0)<> 3 order by tsbp.subcatname_l1", _catID, Request["tsb"].ToString(), Request["tsm"].ToString());
                oHelper.SQLString = sSQL;
                oDs = oHelper.GetDataSet();
                if (!(string.IsNullOrEmpty(Request["sl1"]) && string.IsNullOrEmpty(Request["sl2"])))
                {
                    _SelValue = Request["sl1"].ToString();
                    if (Request["sl2"].ToString().Trim() != "0")
                        _SelValue += "~" + Request["sl2"];
                }
            }
        }
        if (oDs.Tables.Count > 0)
        {
            foreach (DataRow oDr in oDs.Tables[0].Rows)
            {
                string sValue = oDr["subcatid_l1"].ToString();
                if (!string.IsNullOrEmpty(oDr["subcatid_l2"].ToString().Trim()))
                {
                    sValue += "~" + oDr["subcatid_l2"];
                    oDropdown.Items.Add(new ListItem(string.Format("{0} - {1}", oDr["subcatname_l1"].ToString(), oDr["subcatname_l2"].ToString().Trim()), sValue));
                }
                else
                {
                    oDropdown.Items.Add(new ListItem(string.Format("{0}", oDr["subcatname_l1"].ToString()), sValue));
                }
            }

            if (oDropdown.Items.FindByValue(_SelValue) != null && !IsPostBack)
            {
                ListItem oList = oDropdown.Items.FindByValue(_SelValue);
                oList.Selected = true;
            }
        }
    }

    private void GetBrands(DropDownList oDropDown)
    {
        oHelper = new HelperDB();
        string tempCID = Request["cid"].ToString();
        int CatalogID = Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString());
        //string sSQL = "";
        //if (tempCID != "WES0389")
        //    sSQL = "SELECT  DISTINCT TOSUITE_BRAND FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE SUBCATID_L1 <> '' AND CATEGORY_ID='" + tempCID + "' AND CATALOG_ID=" + CatalogID + " ORDER BY TOSUITE_BRAND";
        //else
        //{
        //    string sl1 = "";
        //    if (!string.IsNullOrEmpty(Request["sl1"]))
        //        sl1 = Request["sl1"].ToString();
        //    string sl2 = "";
        //    if (!string.IsNullOrEmpty(Request["sl2"]))
        //        if (Request["sl2"].ToString() != "0")
        //        sl2 = Request["sl2"].ToString();

        //    sSQL = "SELECT  DISTINCT TOSUITE_BRAND FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE CATEGORY_ID='" + tempCID + "' AND CATALOG_ID=" + CatalogID + "";
        //    if (sl1.Trim() != "")
        //     sSQL += " AND SUBCATID_L1 = '" + sl1 + "'"; 
        //    if (sl2.Trim() != "") sSQL += " AND SUBCATID_L2 = '" +  sl2 + "'";
            
        //    sSQL +=" ORDER BY TOSUITE_BRAND";
        //}
        //oHelper.SQLString = sSQL;
        //DataSet oDs = oHelper.GetDataSet();
        DataSet oDs = new DataSet();
        //HttpContext.Current.Session["MainMenuClick"] = null;
        //oDs.Tables.Add(EasyAsk.GetMainMenuClickDetail(tempCID, "Brand").Copy());
        oDs.Tables.Add((DataTable)((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["Brand"].Copy());
       

        foreach (DataRow oDr in oDs.Tables[0].Rows)
        {
            oDropDown.Items.Add(new ListItem(oDr["TOSUITE_BRAND"].ToString()));
        }
        if (!string.IsNullOrEmpty(Request["tsb"]) && !IsPostBack)
        {
            oDropDown.Text = Request["tsb"].ToString();
        }
    }

    private void GetModels(DropDownList oDropDown)
    {
        oHelper = new HelperDB();
        string _CatID = Request["cid"].ToString();

        int CatalogID = Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString());
        if (!string.IsNullOrEmpty(Request["tsb"])) 
        {
            string tosuite_brand = Request["tsb"].ToString();
            //string sSQL = "SELECT DISTINCT TOSUITE_MODEL FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE SUBCATID_L1 <> '' AND CATEGORY_ID='" + _CatID + "' AND CATALOG_ID=" + CatalogID + " AND TOSUITE_BRAND = '" + tosuite_brand + "' ORDER BY TOSUITE_MODEL";
            //oHelper.SQLString = sSQL;
            //DataSet oDs = oHelper.GetDataSet();
            tempCName = GetCName(_CatID);
            //DataSet oDs = EasyAsk.GetWESModel(tempCName, CatalogID, Server.UrlDecode(Request.QueryString["tsb"].ToString())); 
            DataSet oDs = (DataSet)HttpContext.Current.Session["WESBrand_Model"];
            foreach (DataRow oDr in oDs.Tables[0].Rows)
            {
                oDropDown.Items.Add(new ListItem(oDr["TOSUITE_MODEL"].ToString()));
            }
            if (!string.IsNullOrEmpty(Request["tsm"]) && !IsPostBack)
            {
                oDropDown.Text = Request["tsm"].ToString();
            }

        }
    }

    protected string ST_bybrand()
    {
        StringTemplateGroup _stg_main_container = null;
        StringTemplateGroup _stg_records_container = null;
        StringTemplateGroup _stg_records = null;
        StringTemplate _stmpl_main_container_tmpl = null;
        StringTemplate _stmpl_records_container_tmpl = null;
        StringTemplate _stmpl_records_tmpl = null;
        //StringTemplate _stmpl_records_tmpl2 = null;
        //StringTemplate _stmpl_records_tmpl3 = null;
        TBWDataList[] lstrecords = new TBWDataList[0];
        TBWDataList[] lstrows = new TBWDataList[0];
        TBWDataList[] lstcontainers = new TBWDataList[3];
        DataSet dsCat;
        string[] filterval = null;
        string[] filterval1 = null;
        string[] filterval2 = null;
        oHelper = new HelperDB();
        oErr = new ErrorHandler();
        oCat = new Category();
        oPF = new ProductFamily();
        oPR = new ProductRender();
        string sHTML = "";
        string dropdowncatid = "";
        string _catid = "";
        string _fid = "";
        try
        {
           // LoadBrandMode();

            if (Request.QueryString["cid"] != null && Request.QueryString["sldummy"] != null)//sldummy is to skip
            {
                #region old
                if (Request.QueryString["cid"].ToString().Length > 0)
                {
                    dropdowncatid = Getdropdwoncatid(Request.QueryString["cid"].ToString());
                    _catid = Request.QueryString["cid"].ToString();
                    tempCName = GetCName(Request.QueryString["cid"].ToString());
                    if (Request.QueryString["pcid"] != null)
                    {
                        tempCID = Request.QueryString["pcid"].ToString();
                        if (tempCID != GetCID(Request.QueryString["cid"].ToString()))
                        {
                            tempCID = GetCID(Request.QueryString["cid"].ToString());
                        }
                    }
                    else
                    {
                        tempCID = GetCID(Request.QueryString["cid"].ToString());
                    }
                    //if (Request.QueryString["cid"].ToString() == "WES598" || Request.QueryString["cid"].ToString() == "WES503")
                    //{
                    //    tempCID = GetCID(Request.QueryString["cid"].ToString());
                    //}
                    //else
                    //{
                    //    tempCID = Request.QueryString["cid"].ToString();
                    //}
                }
                if (Request.QueryString["fid"] != null && Request.QueryString["fid"].ToString() != "")
                {
                    _fid = Request.QueryString["fid"].ToString();
                }

                if (hidcatIds.Value != string.Empty && hidcatIds.Value != null)
                {
                    filterval = hidcatIds.Value.Split('^');
                }
                if (HidsubcatIds.Value != string.Empty && HidsubcatIds.Value != null)
                {
                    filterval1 = HidsubcatIds.Value.Split('^');
                }

                dsCat = new DataSet();
                dsCat = oCat.GetSubCategoriesBrand(tempCID, Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()));
                if (dsCat != null && dsCat.Tables.Count > 0 && (dsCat.Tables[0].Rows.Count > 0))
                {
                    ictrecords = 0;
                    lstrecords = new TBWDataList[dsCat.Tables[0].Rows.Count + 1];
                    _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                    _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                    _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "Select Brand");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "Select Brand");
                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                    ictrecords++;
                    bool selstate = false;
                    foreach (DataRow _drow in dsCat.Tables[0].Rows)
                    {
                        _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["CATEGORY_ID"].ToString());
                        if (filterval != null && _drow["CATEGORY_ID"].ToString() == filterval[0].ToString())
                        {
                            _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                            selstate = true;
                            Response.Redirect("categorylist.aspx?&ld=0&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2&bypcat=1", false);
                        }
                        else if (filterval == null && _drow["CATEGORY_ID"].ToString() == dropdowncatid && selstate == false)
                        {
                            filterval = new string[2];
                            filterval[0] = _drow["CATEGORY_ID"].ToString();
                            filterval[1] = _drow["CATEGORY_NAME"].ToString();
                            _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                            //Response.Redirect("categorylist.aspx?ld=0&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2&bypcat=1", false);
                        }
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["CATEGORY_NAME"].ToString());
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                        ictrecords++;

                    }
                    _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                    _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                    _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "1");
                    lstcontainers[0] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                }
                if (filterval != null && filterval[0].ToString().Length > 0)
                {
                    tempCID = filterval[0].ToString();
                    //if (tempCID != Request.QueryString["pcid"].ToString())
                    bool selstate1 = false;
                    DataSet dsCat1 = new DataSet();
                    //dsCat1 = GetDataSet("SELECT category_id,category_name,HIERARCHY_LEVEL FROM Category_Function (" + oHelper.GetOptionValues("DEFAULT CATALOG").ToString() + ",'" + tempCID + "')");
                    dsCat1 = GetDataSet(string.Format("SELECT category_id,category_name,HIERARCHY_LEVEL FROM Category_Function ({0},'{1}') where ISNULL(CUSTOM_NUM_FIELD3,0)<> 3", oHelper.GetOptionValues("DEFAULT CATALOG").ToString(), tempCID));
                    if (dsCat1 != null && dsCat1.Tables.Count > 0 && (dsCat1.Tables[0].Rows.Count > 0))
                    {
                        ictrecords = 0;
                        DataRow[] _DCRow = null;
                        _DCRow = dsCat1.Tables[0].Select("HIERARCHY_LEVEL>0");
                        //if (filterval == null)
                        //    _DCRow = dsCat1.Tables[0].Select("HIERARCHY_LEVEL>1");
                        //else
                        //    _DCRow = dsCat1.Tables[0].Select("HIERARCHY_LEVEL>0");
                        if (_DCRow != null && _DCRow.Length > 0)
                        {
                            lstrecords = new TBWDataList[_DCRow.Count() + 1];
                            _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                            _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                            _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all models");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all models");
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                            ictrecords++;
                            foreach (DataRow _drow in _DCRow)
                            {
                                _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["CATEGORY_ID"].ToString());
                                //if (_catid == _drow["CATEGORY_ID"].ToString())
                                //{
                                //  _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                //}
                                //else 
                                if (filterval1 != null && _drow["CATEGORY_ID"].ToString() == filterval1[0].ToString())
                                {
                                    _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                    selstate1 = true;
                                    //Response.Redirect("product_list.aspx?&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2", false);
                                    Response.Redirect("bybrand.aspx?&id=0&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2", false);
                                }
                                else if (filterval1 == null && _drow["CATEGORY_ID"].ToString() == _catid.ToString() && selstate1 == false)
                                {
                                    filterval1 = new string[2];
                                    filterval1[0] = _drow["CATEGORY_ID"].ToString();
                                    filterval1[1] = _drow["CATEGORY_NAME"].ToString();
                                    _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");

                                }
                                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["CATEGORY_NAME"].ToString());
                                lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                                ictrecords++;

                            }
                            _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                            _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                            _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
                            lstcontainers[1] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                        }
                    }
                    dsCat1.Dispose();
                }
                else
                {
                    lstrecords = new TBWDataList[1];
                    _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                    _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                    _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all models");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all models");
                    lstrecords[0] = new TBWDataList(_stmpl_records_tmpl.ToString());
                    _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                    _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                    _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
                    lstcontainers[1] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                }
                string catIDLiSt = "'0'";
                if (filterval1 != null && filterval1[0].ToString().Length > 0)
                {
                    DataSet dsCattemp = new DataSet();
                    //dsCattemp = GetDataSet("SELECT CATEGORY_ID FROM Category_Function (" + oHelper.GetOptionValues("DEFAULT CATALOG").ToString() + ",'" + filterval1[0].ToString() + "')");
                    dsCattemp = GetDataSet(string.Format("SELECT CATEGORY_ID FROM Category_Function ({0},'{1}') where ISNULL(CUSTOM_NUM_FIELD3,0)<> 3'", oHelper.GetOptionValues("DEFAULT CATALOG").ToString(), filterval1[0].ToString()));
                    foreach (DataRow _drow in dsCattemp.Tables[0].Rows)
                    {
                        catIDLiSt = catIDLiSt + ",'" + _drow["category_Id"].ToString() + "'";
                    }

                    //else
                    //{

                    //    foreach (DataRow _drow in dsCat1.Tables[0].Rows)
                    //    {
                    //        catIDLiSt = catIDLiSt + ",'" + _drow["category_Id"].ToString() + "'";
                    //    }
                    //}
                    //catIDLiSt = catIDLiSt + ",'" + _catid + "'";
                    DataSet dsCat2 = new DataSet();
                    bool selstate2 = false;
                    string sqlQuery = "(SELECT DISTINCT F.FAMILY_ID,F.FAMILY_NAME,F.CATEGORY_ID,(SELECT COUNT(PRODUCT_ID) FROM TB_PROD_FAMILY WHERE FAMILY_ID=F.FAMILY_ID) AS COUNT FROM TB_FAMILY F,TB_CATALOG_FAMILY CF where CF.FAMILY_ID=F.FAMILY_ID AND F.CATEGORY_ID IN(" + catIDLiSt + ") AND CF.CATALOG_ID=" + oHelper.GetOptionValues("DEFAULT CATALOG").ToString() + " AND F.PARENT_FAMILY_ID=0 AND (SELECT COUNT(PRODUCT_ID) FROM TB_PROD_FAMILY WHERE FAMILY_ID=F.FAMILY_ID) > 0) ";
                    sqlQuery += "UNION ";
                    sqlQuery += "(SELECT DISTINCT F.FAMILY_ID,(F1.FAMILY_NAME + ' - ' + F.FAMILY_NAME) AS FAMILY_NAME,F.CATEGORY_ID,(SELECT COUNT(PRODUCT_ID) FROM TB_PROD_FAMILY WHERE FAMILY_ID=F.FAMILY_ID) AS COUNT FROM TB_FAMILY F,TB_FAMILY F1,TB_CATALOG_FAMILY CF where CF.FAMILY_ID=F.FAMILY_ID AND F.CATEGORY_ID IN(" + catIDLiSt + ") AND CF.CATALOG_ID=" + oHelper.GetOptionValues("DEFAULT CATALOG").ToString() + " AND F.PARENT_FAMILY_ID IN(SELECT DISTINCT F.FAMILY_ID FROM TB_FAMILY F,TB_CATALOG_FAMILY CF where CF.FAMILY_ID=F.FAMILY_ID AND F.CATEGORY_ID IN(" + catIDLiSt + ") AND CF.CATALOG_ID=" + oHelper.GetOptionValues("DEFAULT CATALOG").ToString() + " AND F.PARENT_FAMILY_ID=0) AND F1.FAMILY_ID=F.PARENT_FAMILY_ID AND (SELECT COUNT(PRODUCT_ID) FROM TB_PROD_FAMILY WHERE FAMILY_ID=F.FAMILY_ID) > 0 ) ORDER BY FAMILY_NAME";
                    dsCat2 = GetDataSet(sqlQuery);
                    if (dsCat2 != null && dsCat2.Tables.Count > 0 && (dsCat2.Tables[0].Rows.Count > 0))
                    {
                        ictrecords = 0;

                        lstrecords = new TBWDataList[dsCat2.Tables[0].Rows.Count + 1];
                        _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                        _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                        _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all products");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all products");
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                        ictrecords++;
                        foreach (DataRow _drow in dsCat2.Tables[0].Rows)
                        {
                            _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["FAMILY_ID"].ToString());//_drow["CATEGORY_ID"].ToString() + "|" + _drow["FAMILY_ID"].ToString());
                            if (filterval2 != null && _drow["CATEGORY_ID"].ToString() + "|" + _drow["FAMILY_ID"].ToString() == filterval2[0].ToString())
                            {
                                _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                selstate2 = true;
                            }
                            else if (filterval2 == null && _drow["CATEGORY_ID"].ToString() == _catid.ToString() && _drow["FAMILY_ID"].ToString() == _fid.ToString() && selstate2 == false)
                            {
                                filterval2 = new string[2];
                                filterval2[0] = _drow["FAMILY_ID"].ToString();
                                filterval2[1] = _drow["FAMILY_NAME"].ToString();
                                _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                //Response.Redirect("categorylist.aspx?ld=0&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2&bypcat=1", false);
                            }
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["FAMILY_NAME"].ToString());
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                            ictrecords++;

                        }
                        _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                        _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                        _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "3");
                        lstcontainers[2] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                    }
                }
                else
                {

                    lstrecords = new TBWDataList[2];
                    _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                    _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                    _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all products");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all products");
                    lstrecords[0] = new TBWDataList(_stmpl_records_tmpl.ToString());
                    _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                    _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                    _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "3");
                    lstcontainers[2] = new TBWDataList(_stmpl_records_container_tmpl.ToString());

                }

                /* _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                 _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                 _stmpl_records_tmpl2 = _stg_records.GetInstanceOf("searchbyproduct" + "\\" + "multilistitem");
                 _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbyproduct" + "\\" + "multilistcontainer");

                // _stmpl_records_container_tmpl = LoadModel(dsCat, _stmpl_records_tmpl2, _stmpl_records_container_tmpl);
                 lstcontainers[1] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                 */



                _stg_main_container = new StringTemplateGroup("searchrsltmultilistmain", stemplatepath);
                _stmpl_main_container_tmpl = _stg_main_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistmain");
                _stmpl_main_container_tmpl.SetAttribute("TBW_CATEGORY_NAME", tempCName);
                _stmpl_main_container_tmpl.SetAttribute("TBWDataList", lstcontainers);

                sHTML = _stmpl_main_container_tmpl.ToString();
                //if (dspfilters.Tables[0].Rows.Count == 0)
                //sHTML = "";
                #endregion
            }
            else
            {
                if (hidcatIds.Value != string.Empty && hidcatIds.Value != null)
                {
                    filterval = hidcatIds.Value.Split('^');
                }
                if (HidsubcatIds.Value != string.Empty && HidsubcatIds.Value != null)
                {
                    filterval1 = HidsubcatIds.Value.Split('^');
                }
                if (HidsubcatIds1.Value != string.Empty && HidsubcatIds1.Value != null)
                {
                    filterval2 = HidsubcatIds1.Value.Split('^');
                }

                if (Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "")//&& Request.QueryString["cid"].ToString() == "WES210582")
                {
                    string cid = Request.QueryString["cid"].ToString();

                    tempCID = Request.QueryString["cid"].ToString();
                    tempCName = GetCName(tempCID);
                    dsCat = new DataSet();
                    if (cid == "WES0389")
                    {
                        ictrecords = 0;
                        lstrecords = new TBWDataList[1];
                        _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                        _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                        _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "Select Category");
                        string sSQL = "select distinct subcatname_l1 from VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT where CATEGORY_ID = 'WES0389'";
                        oHelper.SQLString = sSQL;
                        DataSet oDs = oHelper.GetDataSet();
                        _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "Select Category");
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                        ictrecords++;
                        if (oDs != null)
                        {
                            lstrecords = new TBWDataList[oDs.Tables[0].Rows.Count + 1];
                            foreach (DataRow _drow in oDs.Tables[0].Rows)
                            {
                                _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["subcatname_l1"].ToString());
                                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["subcatname_l1"].ToString());
                                lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                                ictrecords++;
                            }
                        }
                        _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                        _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                        _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
                        lstcontainers[0] = new TBWDataList(_stmpl_records_container_tmpl.ToString());

                        ictrecords = 0;
                        lstrecords = new TBWDataList[1];
                        _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                        _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                        _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "Select Brand");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "Select Brand");
                        lstrecords[0] = new TBWDataList(_stmpl_records_tmpl.ToString());

                        _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                        _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                        _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
                        lstcontainers[1] = new TBWDataList(_stmpl_records_container_tmpl.ToString());

                        ictrecords = 0;
                        lstrecords = new TBWDataList[1];
                        _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                        _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                        _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "Select Model");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "Select Model");
                        lstrecords[0] = new TBWDataList(_stmpl_records_tmpl.ToString());
                        _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                        _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                        _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
                        lstcontainers[2] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                    }
                    else
                    {
                        //dsCat = oCat.GetWESBrand(tempCID, Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()));
                        //dsCat.Tables.Add (EasyAsk.GetMainMenuClickDetail(tempCID, "Brand").Copy()) ;
                        dsCat.Tables.Add((DataTable)((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["Brand"].Copy());


                        if (dsCat != null && dsCat.Tables.Count > 0 && (dsCat.Tables[0].Rows.Count > 0))
                        {
                            ictrecords = 0;
                            lstrecords = new TBWDataList[dsCat.Tables[0].Rows.Count + 1];
                            _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                            _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                            _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");

                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "Select Brand");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "Select Brand");
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                            ictrecords++;
                            bool selstate = false;
                            foreach (DataRow _drow in dsCat.Tables[0].Rows)
                            {
                                _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["TOSUITE_BRAND"].ToString());
                                if (filterval != null && _drow["TOSUITE_BRAND"].ToString() == filterval[0].ToString())
                                {
                                    _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                    selstate = true;
                                    Response.Redirect("categorylist.aspx?&ld=0&cid=" + tempCID + "&tsb=" + Server.UrlEncode(_drow["TOSUITE_BRAND"].ToString()) + "&byp=2&bypcat=1", false);
                                }
                                else if (filterval == null && Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "" && _drow["TOSUITE_BRAND"].ToString() == Server.UrlDecode(Request.QueryString["tsb"].ToString()) && selstate == false)
                                {
                                    filterval = new string[2];
                                    filterval[0] = _drow["TOSUITE_BRAND"].ToString();
                                    filterval[1] = _drow["TOSUITE_BRAND"].ToString();
                                    _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                    //Response.Redirect("categorylist.aspx?ld=0&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2&bypcat=1", false);
                                }
                                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["TOSUITE_BRAND"].ToString());
                                lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                                ictrecords++;

                            }
                            _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                            _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                            _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "1");
                            lstcontainers[0] = new TBWDataList(_stmpl_records_container_tmpl.ToString());

                        }
                        if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "")
                        {
                            tempCID = Request.QueryString["cid"].ToString();
                            bool selstate1 = false;
                            DataSet dsCat1 = new DataSet();
                            //dsCat1 = oCat.GetWESModel(tempCID, Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()), Server.UrlDecode(Request.QueryString["tsb"].ToString()));
                            //dsCat1 = EasyAsk.GetWESModel(tempCName, Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()), Server.UrlDecode(Request.QueryString["tsb"].ToString())); 
                            dsCat1 = (DataSet)HttpContext.Current.Session["WESBrand_Model"];
                            if (dsCat1 != null && dsCat1.Tables.Count > 0 && (dsCat1.Tables[0].Rows.Count > 0))
                            {
                                ictrecords = 0;
                                DataRow[] _DCRow = null;
                                _DCRow = dsCat1.Tables[0].Select();
                                if (_DCRow != null && _DCRow.Length > 0)
                                {
                                    lstrecords = new TBWDataList[_DCRow.Count() + 1];
                                    _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                                    _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                                    _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all models");
                                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all models");
                                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                                    ictrecords++;
                                    foreach (DataRow _drow in _DCRow)
                                    {
                                        _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["TOSUITE_MODEL"].ToString());
                                        if (filterval1 != null && _drow["TOSUITE_MODEL"].ToString() == filterval1[0].ToString())
                                        {
                                            _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                            selstate1 = true;
                                            //Response.Redirect("product_list.aspx?&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2", false);
                                            Response.Redirect("bybrand.aspx?&id=0&cid=" + tempCID + "&tsb=" + Server.UrlEncode(Request.QueryString["tsb"].ToString()) + "&tsm=" + Server.UrlEncode(_drow["TOSUITE_MODEL"].ToString()) + "&byp=2&bypcat=1", false);
                                        }
                                        else if (filterval1 == null && Request.QueryString["tsm"] != null && _drow["TOSUITE_MODEL"].ToString() == Server.UrlDecode(Request.QueryString["tsm"].ToString()) && selstate1 == false)
                                        {
                                            filterval1 = new string[2];
                                            filterval1[0] = _drow["TOSUITE_MODEL"].ToString();
                                            filterval1[1] = _drow["TOSUITE_MODEL"].ToString();
                                            _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");

                                        }
                                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["TOSUITE_MODEL"].ToString());
                                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                                        ictrecords++;

                                    }
                                    _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                                    _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                                    _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
                                    lstcontainers[1] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                                }
                            }
                            dsCat1.Dispose();
                        }
                        else
                        {
                            lstrecords = new TBWDataList[1];
                            _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                            _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                            _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all models");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all models");
                            lstrecords[0] = new TBWDataList(_stmpl_records_tmpl.ToString());
                            _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                            _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                            _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
                            lstcontainers[1] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                        }

                        //if (Request.QueryString["tsm"] != null && Request.QueryString["tsm"].ToString() != "")
                        //{
                        //    /* DataSet dsCat2 = new DataSet();
                        //     bool selstate2 = false;
                        //     dsCat2 = oCat.GetWESFamily(tempCID, Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()), Request.QueryString["tsb"].ToString(), Server.UrlDecode(Request.QueryString["tsm"].ToString()));
                        //     if (dsCat2 != null && dsCat2.Tables.Count > 0 && (dsCat2.Tables[0].Rows.Count > 0))
                        //     {
                        //         ictrecords = 0;

                        //         lstrecords = new TBWDataList[dsCat2.Tables[0].Rows.Count + 1];
                        //         _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                        //         _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                        //         _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrand" + "\\" + "multilistitem");
                        //         _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all products");
                        //         _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all products");
                        //         lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                        //         ictrecords++;
                        //         foreach (DataRow _drow in dsCat2.Tables[0].Rows)
                        //         {
                        //             _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrand" + "\\" + "multilistitem");
                        //             _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["SUBCATID_L1"].ToString() + "|" + _drow["SUBCATID_L2"].ToString());
                        //             string byproduct = "";
                        //             if (_drow["BYPRODUCT"].ToString().EndsWith("- ") == true)
                        //             {
                        //                 byproduct = _drow["BYPRODUCT"].ToString().Replace("- ", "");
                        //             }
                        //             else
                        //             {
                        //                 byproduct = _drow["BYPRODUCT"].ToString();
                        //             }
                        //             if (filterval2 != null && _drow["SUBCATID_L1"].ToString() + "|" + _drow["SUBCATID_L2"].ToString() == filterval2[0].ToString() && filterval2[0].EndsWith("|") == false)
                        //             {
                        //                 _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                        //                 selstate2 = true;
                        //                 Response.Redirect("bybrand.aspx?&id=0&cid=" + tempCID + "&tsb=" + Server.UrlEncode(Request.QueryString["tsb"].ToString()) + "&tsm=" + Server.UrlEncode(Request.QueryString["tsm"].ToString()) + "&sl1=" + _drow["SUBCATID_L1"].ToString() + "&sl2=" + _drow["SUBCATID_L2"].ToString() + "&byp=2&bypcat=1", false);
                        //             }
                        //             else if (filterval2 == null && Request.QueryString["sl1"] != null && Request.QueryString["sl1"].ToString() != "" && Request.QueryString["sl2"] != null && Request.QueryString["sl2"].ToString() != "" && _drow["SUBCATID_L1"].ToString() == Request.QueryString["sl1"].ToString() && _drow["SUBCATID_L2"].ToString() == Request.QueryString["sl2"].ToString() && selstate2 == false)
                        //             {
                        //                 filterval2 = new string[2];
                        //                 filterval2[0] = _drow["SUBCATID_L1"].ToString() + "|" + _drow["SUBCATID_L2"].ToString();
                        //                 filterval2[1] = _drow["BYPRODUCT"].ToString();
                        //                 _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                        //             }
                        //             else if (filterval2 != null && _drow["SUBCATID_L2"].ToString() == "" && _drow["SUBCATID_L1"].ToString() + "|" == filterval2[0].ToString())
                        //             {
                        //                 _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                        //                 selstate2 = true;
                        //                 Response.Redirect("bybrand.aspx?&id=0&cid=" + tempCID + "&tsb=" + Server.UrlEncode(Request.QueryString["tsb"].ToString()) + "&tsm=" + Server.UrlEncode(Request.QueryString["tsm"].ToString()) + "&sl1=" + _drow["SUBCATID_L1"].ToString() + "&sl2=0&byp=2&bypcat=1", false);
                        //             }
                        //             else if (filterval2 == null && Request.QueryString["sl1"] != null && Request.QueryString["sl1"].ToString() != "" && Request.QueryString["sl2"] != null && Request.QueryString["sl2"].ToString() != "" && Request.QueryString["sl2"].ToString() == "0" && _drow["SUBCATID_L1"].ToString() == Request.QueryString["sl1"].ToString() && _drow["SUBCATID_L2"].ToString() == "" && selstate2 == false)
                        //             {
                        //                 filterval2 = new string[2];
                        //                 filterval2[0] = _drow["SUBCATID_L1"].ToString() + "|" + _drow["SUBCATID_L2"].ToString();
                        //                 filterval2[1] = byproduct;
                        //                 _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                        //             }
                        //             _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", byproduct);
                        //             lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                        //             ictrecords++;

                        //         }
                        //         _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrand" + "\\" + "multilistcontainer");
                        //         _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                        //         _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "3");
                        //         lstcontainers[2] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                        //     }*/
                        //}
                        //else
                        //{
                        //    lstrecords = new TBWDataList[2];
                        //    _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                        //    _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                        //    _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                        //    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "Select Category");
                        //    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "Select Category");
                        //    lstrecords[0] = new TBWDataList(_stmpl_records_tmpl.ToString());
                        //    _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                        //    _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                        //    _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "3");
                        //    lstcontainers[2] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                        //}

                    }
                }
                _stg_main_container = new StringTemplateGroup("searchrsltmultilistmain", stemplatepath);
                _stmpl_main_container_tmpl = _stg_main_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistmain");
                _stmpl_main_container_tmpl.SetAttribute("TBW_CATEGORY_NAME", tempCName);
                _stmpl_main_container_tmpl.SetAttribute("TBWDataList", lstcontainers);
                sHTML = _stmpl_main_container_tmpl.ToString();
            }
        }
        catch (Exception ex)
        {
            sHTML = ex.Message;
        }
        return sHTML;
    }



    private string Getdropdwoncatid(string catID)
    {
        DataSet DSBC = null;
        //DataSet DSUBC = null;
        string catIDtemp = catID;
        do
        {
            DSBC = GetDataSet("SELECT CATEGORY_NAME,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID IN(SELECT PARENT_CATEGORY FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "')");

            if (DSBC != null && DSBC.Tables[0] != null && DSBC.Tables[0].Rows != null && DSBC.Tables[0].Rows.Count > 0)
                foreach (DataRow DR in DSBC.Tables[0].Rows)
                {
                    if (DR["CATEGORY_NAME"].ToString().ToUpper() == "PRODUCT" || DR["CATEGORY_NAME"].ToString().ToUpper() == "BRAND")
                    {
                        return catIDtemp;

                    }
                    else
                    {
                        catIDtemp = DR["CATEGORY_ID"].ToString();
                    }
                }
            else
                return "0";
        } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
        return DSBC.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
    }
    private string GetCID(string catID)
    {
        DataSet DSBC = null;
        DataSet DSUBC = null;
        string catIDtemp = catID;
        do
        {
            DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
            foreach (DataRow DR in DSBC.Tables[0].Rows)
            {
                catIDtemp = DR["PARENT_CATEGORY"].ToString();
                if (catIDtemp == "0")
                {
                    DSUBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY = '" + DR["CATEGORY_ID"].ToString() + "' AND CATEGORY_NAME Like 'brand'");
                    return DSUBC.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                }
            }
        } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
        return DSBC.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
    }
    private string GetCName(string catID)
    {
        DataSet DSBC = null;
        string catIDtemp = catID;
        do
        {
            DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
            foreach (DataRow DR in DSBC.Tables[0].Rows)
            {
                catIDtemp = DR["PARENT_CATEGORY"].ToString();
                if (catIDtemp == "0")
                {
                    // DSUBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY = '" + DR["CATEGORY_ID"].ToString() + "' AND CATEGORY_NAME Like 'Product'");
                    return DR["CATEGORY_NAME"].ToString();
                }
            }
        } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
        return DSBC.Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
    }
    private DataSet GetDataSet(string SQLQuery)
    {
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(SQLQuery, conStr.ConnectionString.ToString().Substring(conStr.ConnectionString.ToString().IndexOf(';') + 1));
        da.Fill(ds, "generictable");
        return ds;
    }

    protected void dd1_TextChanged(object sender, EventArgs e)
    {
        if (dd1.ToolTip == "Brands")
        {
            string sURL = string.Format("categorylist.aspx?ld=0&cid={0}&tsb={1}&bypcat=1&byp=2", Request["cid"], Server.UrlEncode(dd1.Text));
            Response.Redirect(sURL);
        }
        if (dd1.ToolTip == "Categories")
        {
            string[] sCatS = dd1.SelectedValue.Split('~');
            string _SubCat1 = "";
            string _SubCat2 = "0";
            _SubCat1 = sCatS[0];
            if (sCatS.Length > 1)
            {
                _SubCat2 = sCatS[1];
            }
            string sURL = string.Format("categorylist.aspx?ld=0&cid={0}&byp=2&sl1={1}&sl2={2}", Request["cid"], Server.UrlEncode(_SubCat1), Server.UrlEncode(_SubCat2));
            Response.Redirect(sURL);

        }
    }

    protected void dd2_TextChanged(object sender, EventArgs e)
    {
        if (dd2.ToolTip == "Brands")
        {
            string[] sCatS = dd1.SelectedValue.Split('~');
            string _SubCat1 = "";
            string _SubCat2 = "0";
            _SubCat1 = sCatS[0];
            if (sCatS.Length > 1)
            {
                _SubCat2 = sCatS[1];
            }
            string sURL = string.Format("categorylist.aspx?ld=0&cid={0}&tsb={1}&bypcat=1&byp=2&sl1={2}&sl2={3}", Request["cid"], Server.UrlEncode(dd2.Text).Trim(), Server.UrlEncode(_SubCat1), Server.UrlEncode(_SubCat2));
            Response.Redirect(sURL);
        }
        else if (dd2.ToolTip == "Models")
        {
            string sURL = string.Format("bybrand.aspx?ld=0&cid={0}&tsb={1}&tsm={2}&bypcat=1&byp=2", Request["cid"], Server.UrlEncode(dd1.Text), Server.UrlEncode(dd2.Text));
            Response.Redirect(sURL);
        }
    }

    protected void dd3_TextChanged(object sender, EventArgs e)
    {
        if (dd3.ToolTip == "Brands")
        {
            string sURL = string.Format("categorylist.aspx?ld=0&cid={0}&tsb={1}&bypcat=1&byp=2", Request["cid"], Server.UrlEncode(dd3.Text));
            Response.Redirect(sURL);
        }
        else if (dd3.ToolTip == "Models")
        {
            if (dd1.SelectedValue.ToString().Trim() == "")
            {
                string sURL = string.Format("bybrand.aspx?ld=0&cid={0}&tsb={1}&tsm={2}&bypcat=1&byp=2", Request["cid"], Server.UrlEncode(dd2.Text), Server.UrlEncode(dd3.Text));
                Response.Redirect(sURL);
            }
            else
            {
                string[] sCatS = dd1.SelectedValue.Split('~');
                string _SubCat1 = "";
                string _SubCat2 = "0";
                _SubCat1 = sCatS[0];
                if (sCatS.Length > 1)
                {
                    _SubCat2 = sCatS[1];
                }
                string sURL = string.Format("bybrand.aspx?ld=0&cid={0}&tsb={1}&tsm={2}&bypcat=1&byp=2&sl1={3}&sl2={4}", Request["cid"], Server.UrlEncode(dd2.Text), Server.UrlEncode(dd3.Text), Server.UrlEncode(_SubCat1), Server.UrlEncode(_SubCat2));
                Response.Redirect(sURL);
            }
        }
        else if (dd3.ToolTip == "Categories")
        {
            string[] sCatS = dd3.SelectedValue.Split('~');
            string _SubCat1 = "";
            string _SubCat2 = "0";
            _SubCat1 = sCatS[0];
            if (sCatS.Length > 1)
            {
                _SubCat2 = sCatS[1];
            }
            string sURL = string.Format("bybrand.aspx?ld=0&cid={0}&tsb={1}&tsm={2}&bypcat=1&byp=2&sl1={3}&sl2={4}", Request["cid"], Server.UrlEncode(dd1.Text),Server.UrlEncode(dd2.Text),Server.UrlEncode(_SubCat1), Server.UrlEncode(_SubCat2));
            Response.Redirect(sURL);
        }
    }

}
