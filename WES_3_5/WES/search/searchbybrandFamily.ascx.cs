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
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class search_searchbybrandFamily : System.Web.UI.UserControl
{
    string stemplatepath = "";
    string tempCID = "";
    string pcid = "";
    string tempCName = "";
    ConnectionDB conStr = new ConnectionDB();    
    ErrorHandler oErr;
    Category oCat;
    ProductFamily oPF;
    ProductRender oPR;
    HelperDB oHelper;
    int ictrecords = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        stemplatepath = Server.MapPath(ConfigurationManager.AppSettings["StringTemplatePath"].ToString());
        try
        {
            if (Request.Url.OriginalString.ToString().ToUpper().Contains("PRODUCT_LIST"))
                stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\product_list\\";
            if (Request.Url.OriginalString.ToString().ToUpper().Contains("FAMILY"))
                stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\bybrand\\";
            if (Request.Url.OriginalString.ToString().ToUpper().Contains("BYBRAND.ASPX") || Request.Url.OriginalString.ToString().ToUpper().Contains("CATEGORYLIST.ASPX") || Request.Url.OriginalString.ToString().ToUpper().Contains("PRODUCT_LIST.ASPX"))
                stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\bybrand\\";
            //if (Request.Url.OriginalString.ToString().ToUpper().Contains("BYPRODUCT.ASPX") || Request.Url.OriginalString.ToString().ToUpper().Contains("CATEGORYLIST.ASPX") || Request.Url.OriginalString.ToString().ToUpper().Contains("PRODUCT_LIST.ASPX"))
                //stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\byproduct\\";
        }
        catch (Exception ec)
        {
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
        StringTemplate _stmpl_records_tmpl2 = null;
        StringTemplate _stmpl_records_tmpl3 = null;
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
        bool DisplayCompatible = false;
        bool nodisplay = false;
        try
        {
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
                    _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrand" + "\\" + "multilistitem");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "Select Brand");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "Select Brand");
                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                    ictrecords++;
                    bool selstate = false;
                    foreach (DataRow _drow in dsCat.Tables[0].Rows)
                    {
                        _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrand" + "\\" + "multilistitem");
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
                    _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrand" + "\\" + "multilistcontainer");
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
                    dsCat1 = GetDataSet("SELECT category_id,category_name,HIERARCHY_LEVEL FROM Category_Function (" + oHelper.GetOptionValues("DEFAULT CATALOG").ToString() + ",'" + tempCID + "')");
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
                            _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrand" + "\\" + "multilistitem");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all models");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all models");
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                            ictrecords++;
                            foreach (DataRow _drow in _DCRow)
                            {
                                _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrand" + "\\" + "multilistitem");
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
                            _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrand" + "\\" + "multilistcontainer");
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
                    _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrand" + "\\" + "multilistitem");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all models");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all models");
                    lstrecords[0] = new TBWDataList(_stmpl_records_tmpl.ToString());
                    _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrand" + "\\" + "multilistcontainer");
                    _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                    _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
                    lstcontainers[1] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                }
                string catIDLiSt = "'0'";
                if (filterval1 != null && filterval1[0].ToString().Length > 0)
                {
                    DataSet dsCattemp = new DataSet();
                    dsCattemp = GetDataSet("SELECT CATEGORY_ID FROM Category_Function (" + oHelper.GetOptionValues("DEFAULT CATALOG").ToString() + ",'" + filterval1[0].ToString() + "')");
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
                        _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrand" + "\\" + "multilistitem");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all products");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all products");
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                        ictrecords++;
                        foreach (DataRow _drow in dsCat2.Tables[0].Rows)
                        {
                            _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrand" + "\\" + "multilistitem");
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
                        _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrand" + "\\" + "multilistcontainer");
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
                    _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrand" + "\\" + "multilistitem");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all products");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all products");
                    lstrecords[0] = new TBWDataList(_stmpl_records_tmpl.ToString());
                    _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrand" + "\\" + "multilistcontainer");
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
                _stmpl_main_container_tmpl = _stg_main_container.GetInstanceOf("searchbybrand" + "\\" + "multilistmain");
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

                if (Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "" && Request.QueryString["pcr"] != null && Request.QueryString["pcr"].ToString() != "" && Request.Url.ToString().ToLower().Contains("family.aspx") == false)//&& Request.QueryString["pcr"].ToString() == "WES210582"
                {
                    tempCID = Request.QueryString["cid"].ToString();
                    pcid = Request.QueryString["pcr"].ToString();
                    tempCName = GetCName(tempCID);
                    dsCat = new DataSet();
                    dsCat = oCat.GetWESBrandFamily(pcid, Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()), tempCID);
                    if (dsCat != null && dsCat.Tables.Count > 0 && (dsCat.Tables[0].Rows.Count > 0))
                    {
                        ictrecords = 0;
                        lstrecords = new TBWDataList[dsCat.Tables[0].Rows.Count + 1];
                        _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                        _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                        _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistitem");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "Select Brand");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "Select Brand");
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                        ictrecords++;
                        bool selstate = false;
                        foreach (DataRow _drow in dsCat.Tables[0].Rows)
                        {
                            _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistitem");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["TOSUITE_BRAND"].ToString());
                            if (filterval != null && _drow["TOSUITE_BRAND"].ToString() == filterval[0].ToString())
                            {
                                _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                selstate = true;
                                Response.Redirect("product_list.aspx?&ld=0&cid=" + tempCID + "&pcr=" + pcid + "&sb=" + Server.UrlEncode(_drow["TOSUITE_BRAND"].ToString()) + "&byp=2&qf=1", false);
                            }
                            else if (filterval == null && Request.QueryString["sb"] != null && Request.QueryString["sb"].ToString() != "" && _drow["TOSUITE_BRAND"].ToString() == Server.UrlDecode(Request.QueryString["sb"].ToString()) && selstate == false)
                            {
                                filterval = new string[2];
                                filterval[0] = _drow["TOSUITE_BRAND"].ToString();
                                filterval[1] = _drow["TOSUITE_BRAND"].ToString();
                                _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                //Response.Redirect("categorylist.aspx?ld=0&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2&bypcat=1", false);
                            }
                            else if (filterval != null && filterval[0].ToString().ToLower() == "select brand")
                            {
                                Response.Redirect("product_list.aspx?&ld=0&cid=" + tempCID + "&pcr=" + pcid + "&byp=2&qf=1", false);

                            }
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["TOSUITE_BRAND"].ToString());
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                            ictrecords++;

                        }
                        _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistcontainer");
                        _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                        _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "1");
                        lstcontainers[0] = new TBWDataList(_stmpl_records_container_tmpl.ToString());

                    }
                    if (Request.QueryString["sb"] != null && Request.QueryString["sb"].ToString() != "")
                    {
                        tempCID = Request.QueryString["cid"].ToString();
                        pcid = Request.QueryString["pcr"].ToString();
                        bool selstate1 = false;
                        DataSet dsCat1 = new DataSet();
                        dsCat1 = oCat.GetWESModelFamily(pcid, Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()), Server.UrlDecode(Request.QueryString["sb"].ToString()), tempCID);
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
                                _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistitem");
                                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all models");
                                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all models");
                                lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                                ictrecords++;
                                foreach (DataRow _drow in _DCRow)
                                {
                                    _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistitem");
                                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["TOSUITE_MODEL"].ToString());
                                    if (filterval1 != null && _drow["TOSUITE_MODEL"].ToString() == filterval1[0].ToString())
                                    {
                                        _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                        selstate1 = true;
                                        //Response.Redirect("product_list.aspx?&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2", false);
                                        Response.Redirect("product_list.aspx?&id=0&cid=" + tempCID + "&pcr=" + pcid + "&sb=" + Server.UrlEncode(Request.QueryString["sb"].ToString()) + "&sm=" + Server.UrlEncode(_drow["TOSUITE_MODEL"].ToString()) + "&byp=2&qf=1", false);
                                    }
                                    else if (filterval1 == null && Request.QueryString["sm"] != null && _drow["TOSUITE_MODEL"].ToString() == Server.UrlDecode(Request.QueryString["sm"].ToString()) && selstate1 == false)
                                    {
                                        filterval1 = new string[2];
                                        filterval1[0] = _drow["TOSUITE_MODEL"].ToString();
                                        filterval1[1] = _drow["TOSUITE_MODEL"].ToString();
                                        _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");

                                    }
                                    else if (filterval1 != null && filterval1[0].ToString().ToLower() == "list all models")
                                    {
                                        Response.Redirect("product_list.aspx?&id=0&cid=" + tempCID + "&pcr=" + pcid + "&sb=" + Server.UrlEncode(Request.QueryString["sb"].ToString()) + "&byp=2&qf=1", false);
                                    }
                                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["TOSUITE_MODEL"].ToString());
                                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                                    ictrecords++;

                                }
                                _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistcontainer");
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
                        _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistitem");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all models");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all models");
                        lstrecords[0] = new TBWDataList(_stmpl_records_tmpl.ToString());
                        _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistcontainer");
                        _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                        _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
                        lstcontainers[1] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                    }

                    //if (Request.QueryString["tsm"] != null && Request.QueryString["tsm"].ToString() != "")
                    //{
                    //    DataSet dsCat2 = new DataSet();
                    //    bool selstate2 = false;
                    //    dsCat2 = oCat.GetWESFamily(tempCID,Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()),Request.QueryString["tsb"].ToString(),Server.UrlDecode(Request.QueryString["tsm"].ToString()));
                    //    if (dsCat2 != null && dsCat2.Tables.Count > 0 && (dsCat2.Tables[0].Rows.Count > 0))
                    //    {
                    //        ictrecords = 0;

                    //        lstrecords = new TBWDataList[dsCat2.Tables[0].Rows.Count + 1];
                    //        _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                    //        _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                    //        _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistitem");
                    //        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all products");
                    //        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all products");
                    //        lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                    //        ictrecords++;
                    //        foreach (DataRow _drow in dsCat2.Tables[0].Rows)
                    //        {
                    //            _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistitem");
                    //            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["SUBCATID_L1"].ToString() + "|" + _drow["SUBCATID_L2"].ToString());
                    //            if (filterval2 != null && _drow["SUBCATID_L1"].ToString() + "|" + _drow["SUBCATID_L2"].ToString() == filterval2[0].ToString())
                    //            {
                    //                _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                    //                selstate2 = true;
                    //            }                                
                    //            else if (filterval2 == null && Request.QueryString["sl1"] != null && Request.QueryString["sl1"].ToString() != "" && Request.QueryString["sl2"] != null && Request.QueryString["sl2"].ToString() != "" && _drow["SUBCATID_L1"].ToString() == Request.QueryString["sl1"].ToString() && _drow["SUBCATID_L2"].ToString() == Request.QueryString["sl2"].ToString() && selstate2 == false)
                    //            {
                    //                filterval2 = new string[2];
                    //                filterval2[0] = _drow["SUBCATID_L1"].ToString() + "|" + _drow["SUBCATID_L2"].ToString();
                    //                filterval2[1] = _drow["BYPRODUCT"].ToString();
                    //                _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");                                    
                    //            }
                    //            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["BYPRODUCT"].ToString());
                    //            lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                    //            ictrecords++;

                    //        }
                    //        _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistcontainer");
                    //        _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                    //        _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "3");
                    //        lstcontainers[2] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                    //    }
                    //}
                    //else
                    //{
                    //    lstrecords = new TBWDataList[2];
                    //    _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                    //    _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                    //    _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistitem");
                    //    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all products");
                    //    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all products");
                    //    lstrecords[0] = new TBWDataList(_stmpl_records_tmpl.ToString());
                    //    _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistcontainer");
                    //    _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                    //    _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "3");
                    //    lstcontainers[2] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                    //}

                }
                else if (Request.Url.ToString().ToLower().Contains("family.aspx") == true && Request.QueryString["pcr"] != null && Request.QueryString["pcr"].ToString() != "")
                {
                    string fid = Request.QueryString["fid"].ToString();
                    DataSet ds = GetDataSetFX(fid);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 1)
                    {
                        pcid = Request.QueryString["pcr"].ToString();
                        tempCID = Request.QueryString["cid"].ToString();
                        dsCat = new DataSet();
                        dsCat = GetDataSet("SELECT DISTINCT TOSUITE_BRAND FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE FAMILY_ID=" + fid + " AND CATEGORY_ID='" + pcid + "' AND CATALOG_ID=" + oHelper.GetOptionValues("DEFAULT CATALOG").ToString() + " ORDER BY TOSUITE_BRAND");
                        if (dsCat != null && dsCat.Tables.Count > 0 && (dsCat.Tables[0].Rows.Count > 0))
                        {
                            ictrecords = 0;
                            lstrecords = new TBWDataList[dsCat.Tables[0].Rows.Count + 1];
                            _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                            _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                            _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistitem");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "Select Brand");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "Select Brand");
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                            ictrecords++;
                            bool selstate = false;
                            foreach (DataRow _drow in dsCat.Tables[0].Rows)
                            {
                                _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistitem");
                                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["TOSUITE_BRAND"].ToString());
                                if (filterval != null && _drow["TOSUITE_BRAND"].ToString() == filterval[0].ToString())
                                {
                                    _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                    selstate = true;
                                    Response.Redirect("family.aspx?&fid=" + fid + "&cid=" + tempCID + "&pcr=" + pcid + "&sb=" + Server.UrlEncode(_drow["TOSUITE_BRAND"].ToString()) + "&byp=2&qf=1", false);
                                }
                                else if (filterval == null && Request.QueryString["sb"] != null && Request.QueryString["sb"].ToString() != "" && _drow["TOSUITE_BRAND"].ToString() == Server.UrlDecode(Request.QueryString["sb"].ToString()) && selstate == false)
                                {
                                    filterval = new string[2];
                                    filterval[0] = _drow["TOSUITE_BRAND"].ToString();
                                    filterval[1] = _drow["TOSUITE_BRAND"].ToString();
                                    _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                    //Response.Redirect("categorylist.aspx?ld=0&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2&bypcat=1", false);
                                }
                                else if (filterval != null && filterval[0].ToString().ToLower() == "select brand")
                                {
                                    Response.Redirect("family.aspx?&fid=" + fid + "&cid=" + tempCID + "&pcr=" + pcid + "&byp=2&qf=1", false);
                                }
                                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["TOSUITE_BRAND"].ToString());
                                lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                                ictrecords++;

                            }
                            _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistcontainer");
                            _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                            _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "1");
                            lstcontainers[0] = new TBWDataList(_stmpl_records_container_tmpl.ToString());

                            if (Request.QueryString["sb"] != null && Request.QueryString["sb"].ToString() != "")
                            {
                                tempCID = Request.QueryString["cid"].ToString();
                                pcid = Request.QueryString["pcr"].ToString();
                                fid = Request.QueryString["fid"].ToString();
                                bool selstate1 = false;
                                DataSet dsCat1 = new DataSet();
                                dsCat1 = GetDataSet("SELECT DISTINCT TOSUITE_MODEL FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE FAMILY_ID=" + fid + " AND CATEGORY_ID='" + pcid + "' AND TOSUITE_BRAND='" + Server.UrlDecode(Request.QueryString["sb"].ToString()) + "' AND CATALOG_ID=" + oHelper.GetOptionValues("DEFAULT CATALOG").ToString() + " ORDER BY TOSUITE_MODEL");
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
                                        _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistitem");
                                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all models");
                                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all models");
                                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                                        ictrecords++;
                                        foreach (DataRow _drow in _DCRow)
                                        {
                                            _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistitem");
                                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["TOSUITE_MODEL"].ToString());
                                            if (filterval1 != null && _drow["TOSUITE_MODEL"].ToString() == filterval1[0].ToString())
                                            {
                                                _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                                selstate1 = true;
                                                //Response.Redirect("product_list.aspx?&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2", false);
                                                Response.Redirect("family.aspx?&fid=" + fid + "&cid=" + tempCID + "&pcr=" + pcid + "&sb=" + Server.UrlEncode(Request.QueryString["sb"].ToString()) + "&sm=" + Server.UrlEncode(_drow["TOSUITE_MODEL"].ToString()) + "&byp=2&qf=1", false);
                                            }
                                            else if (filterval1 == null && Request.QueryString["sm"] != null && _drow["TOSUITE_MODEL"].ToString() == Server.UrlDecode(Request.QueryString["sm"].ToString()) && selstate1 == false)
                                            {
                                                filterval1 = new string[2];
                                                filterval1[0] = _drow["TOSUITE_MODEL"].ToString();
                                                filterval1[1] = _drow["TOSUITE_MODEL"].ToString();
                                                _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");

                                            }
                                            else if (filterval1 != null && filterval1.ToString().ToLower() == "list all models")
                                            {
                                                Response.Redirect("family.aspx?&fid=" + fid + "&cid=" + tempCID + "&pcr=" + pcid + "&sb=" + Server.UrlEncode(Request.QueryString["sb"].ToString()) + "&byp=2&qf=1", false);

                                            }
                                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["TOSUITE_MODEL"].ToString());
                                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                                            ictrecords++;

                                        }
                                        _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistcontainer");
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
                                _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistitem");
                                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all models");
                                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all models");
                                lstrecords[0] = new TBWDataList(_stmpl_records_tmpl.ToString());
                                _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistcontainer");
                                _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                                _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
                                lstcontainers[1] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                            }
                        }
                        else
                        {
                            DisplayCompatible = false;
                            nodisplay = true;
                        }

                    }
                    else if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                    {
                        DisplayCompatible = true;
                        pcid = Request.QueryString["pcr"].ToString();
                        tempCID = Request.QueryString["cid"].ToString();
                        dsCat = new DataSet();
                        dsCat = GetDataSet("SELECT DISTINCT TOSUITE_BRAND FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE FAMILY_ID=" + fid + " AND CATEGORY_ID='" + pcid + "' AND CATALOG_ID=" + oHelper.GetOptionValues("DEFAULT CATALOG").ToString() + " ORDER BY TOSUITE_BRAND");
                        if (dsCat != null && dsCat.Tables.Count > 0 && (dsCat.Tables[0].Rows.Count > 0))
                        {
                            ictrecords = 0;
                            lstrecords = new TBWDataList[dsCat.Tables[0].Rows.Count + 1];
                            _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                            _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                            _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistitem");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "Select Brand");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "Select Brand");
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                            ictrecords++;
                            bool selstate = false;
                            foreach (DataRow _drow in dsCat.Tables[0].Rows)
                            {
                                _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistitem");
                                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["TOSUITE_BRAND"].ToString());
                                if (filterval != null && _drow["TOSUITE_BRAND"].ToString() == filterval[0].ToString())
                                {
                                    _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                    selstate = true;
                                    Response.Redirect("family.aspx?&fid=" + fid + "&cid=" + tempCID + "&pcr=" + pcid + "&sb=" + Server.UrlEncode(_drow["TOSUITE_BRAND"].ToString()) + "&byp=2&qf=1", false);
                                }
                                else if (filterval == null && Request.QueryString["sb"] != null && Request.QueryString["sb"].ToString() != "" && _drow["TOSUITE_BRAND"].ToString() == Server.UrlDecode(Request.QueryString["sb"].ToString()) && selstate == false)
                                {
                                    filterval = new string[2];
                                    filterval[0] = _drow["TOSUITE_BRAND"].ToString();
                                    filterval[1] = _drow["TOSUITE_BRAND"].ToString();
                                    _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                    //Response.Redirect("categorylist.aspx?ld=0&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2&bypcat=1", false);
                                }
                                else if (filterval != null && filterval[0].ToString().ToLower() == "select brand")
                                {
                                    Response.Redirect("family.aspx?&fid=" + fid + "&cid=" + tempCID + "&pcr=" + pcid + "&byp=2&qf=1", false);
                                }
                                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["TOSUITE_BRAND"].ToString());
                                lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                                ictrecords++;

                            }
                            _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistcontainer");
                            _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                            _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "1");
                            lstcontainers[0] = new TBWDataList(_stmpl_records_container_tmpl.ToString());

                            if (Request.QueryString["sb"] != null && Request.QueryString["sb"].ToString() != "")
                            {
                                tempCID = Request.QueryString["cid"].ToString();
                                pcid = Request.QueryString["pcr"].ToString();
                                fid = Request.QueryString["fid"].ToString();
                                bool selstate1 = false;
                                DataSet dsCat1 = new DataSet();
                                dsCat1 = GetDataSet("SELECT DISTINCT TOSUITE_MODEL FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE FAMILY_ID=" + fid + " AND CATEGORY_ID='" + pcid + "' AND TOSUITE_BRAND='" + Server.UrlDecode(Request.QueryString["sb"].ToString()) + "' AND CATALOG_ID=" + oHelper.GetOptionValues("DEFAULT CATALOG").ToString() + " ORDER BY TOSUITE_MODEL");
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
                                        _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistitem");
                                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all models");
                                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all models");
                                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                                        ictrecords++;
                                        foreach (DataRow _drow in _DCRow)
                                        {
                                            _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistitem");
                                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["TOSUITE_MODEL"].ToString());
                                            if (filterval1 != null && _drow["TOSUITE_MODEL"].ToString() == filterval1[0].ToString())
                                            {
                                                _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                                selstate1 = true;
                                                //Response.Redirect("product_list.aspx?&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2", false);
                                                //Response.Redirect("family.aspx?&id=0&cid=" + tempCID + "&pcr=" + pcid + "&sb=" + Request.QueryString["sb"].ToString() + "&sm=" + Server.UrlEncode(_drow["TOSUITE_MODEL"].ToString()) + "&byp=2&qf=1", false);
                                            }
                                            else if (filterval1 == null && Request.QueryString["sm"] != null && _drow["TOSUITE_MODEL"].ToString() == Server.UrlDecode(Request.QueryString["sm"].ToString()) && selstate1 == false)
                                            {
                                                filterval1 = new string[2];
                                                filterval1[0] = _drow["TOSUITE_MODEL"].ToString();
                                                filterval1[1] = _drow["TOSUITE_MODEL"].ToString();
                                                _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");

                                            }
                                            else if (filterval1 != null && filterval1.ToString().ToLower() == "list all models")
                                            {
                                                Response.Redirect("family.aspx?&fid=" + fid + "&cid=" + tempCID + "&pcr=" + pcid + "&sb=" +Server.UrlEncode(Request.QueryString["sb"].ToString()) + "&byp=2&qf=1", false);

                                            }
                                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["TOSUITE_MODEL"].ToString());
                                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                                            ictrecords++;

                                        }
                                        _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistcontainer");
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
                                _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistitem");
                                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all models");
                                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all models");
                                lstrecords[0] = new TBWDataList(_stmpl_records_tmpl.ToString());
                                _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistcontainer");
                                _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                                _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
                                lstcontainers[1] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                            }
                        }
                        else
                        {
                            DisplayCompatible = false;
                            nodisplay = true;
                        }
                    }
                    else
                    {
                        pcid = Request.QueryString["pcr"].ToString();
                        tempCID = Request.QueryString["cid"].ToString();
                        dsCat = new DataSet();
                        dsCat = GetDataSet("SELECT DISTINCT TOSUITE_BRAND FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE FAMILY_ID=" + fid + " AND CATEGORY_ID='" + pcid + "' AND CATALOG_ID=" + oHelper.GetOptionValues("DEFAULT CATALOG").ToString() + " ORDER BY TOSUITE_BRAND");
                        if (dsCat != null && dsCat.Tables.Count > 0 && (dsCat.Tables[0].Rows.Count > 0))
                        {
                            ictrecords = 0;
                            lstrecords = new TBWDataList[dsCat.Tables[0].Rows.Count + 1];
                            _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                            _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                            _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistitem");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "Select Brand");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "Select Brand");
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                            ictrecords++;
                            bool selstate = false;
                            foreach (DataRow _drow in dsCat.Tables[0].Rows)
                            {
                                _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistitem");
                                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["TOSUITE_BRAND"].ToString());
                                if (filterval != null && _drow["TOSUITE_BRAND"].ToString() == filterval[0].ToString())
                                {
                                    _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                    selstate = true;
                                    Response.Redirect("family.aspx?&fid=" + fid + "&cid=" + tempCID + "&pcr=" + pcid + "&sb=" + Server.UrlEncode(_drow["TOSUITE_BRAND"].ToString())+ "&byp=2&qf=1", false);
                                }
                                else if (filterval == null && Request.QueryString["sb"] != null && Request.QueryString["sb"].ToString() != "" && _drow["TOSUITE_BRAND"].ToString() == Server.UrlDecode(Request.QueryString["sb"].ToString()) && selstate == false)
                                {
                                    filterval = new string[2];
                                    filterval[0] = _drow["TOSUITE_BRAND"].ToString();
                                    filterval[1] = _drow["TOSUITE_BRAND"].ToString();
                                    _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                    //Response.Redirect("categorylist.aspx?ld=0&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2&bypcat=1", false);
                                }
                                else if (filterval != null && filterval[0].ToString().ToLower() == "select brand")
                                {
                                    Response.Redirect("family.aspx?&fid=" + fid + "&cid=" + tempCID + "&pcr=" + pcid + "&byp=2&qf=1", false);
                                }
                                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["TOSUITE_BRAND"].ToString());
                                lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                                ictrecords++;

                            }
                            _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistcontainer");
                            _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                            _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "1");
                            lstcontainers[0] = new TBWDataList(_stmpl_records_container_tmpl.ToString());

                            if (Request.QueryString["sb"] != null && Request.QueryString["sb"].ToString() != "")
                            {
                                tempCID = Request.QueryString["cid"].ToString();
                                pcid = Request.QueryString["pcr"].ToString();
                                fid = Request.QueryString["fid"].ToString();
                                bool selstate1 = false;
                                DataSet dsCat1 = new DataSet();
                                dsCat1 = GetDataSet("SELECT DISTINCT TOSUITE_MODEL FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE FAMILY_ID=" + fid + " AND CATEGORY_ID='" + pcid + "' AND TOSUITE_BRAND='" + Server.UrlDecode(Request.QueryString["sb"].ToString()) + "' AND CATALOG_ID=" + oHelper.GetOptionValues("DEFAULT CATALOG").ToString() + " ORDER BY TOSUITE_MODEL");
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
                                        _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistitem");
                                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all models");
                                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all models");
                                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                                        ictrecords++;
                                        foreach (DataRow _drow in _DCRow)
                                        {
                                            _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistitem");
                                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["TOSUITE_MODEL"].ToString());
                                            if (filterval1 != null && _drow["TOSUITE_MODEL"].ToString() == filterval1[0].ToString())
                                            {
                                                _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                                selstate1 = true;
                                                //Response.Redirect("product_list.aspx?&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2", false);
                                                Response.Redirect("family.aspx?&fid=" + fid + "&cid=" + tempCID + "&pcr=" + pcid + "&sb=" + Server.UrlEncode(Request.QueryString["sb"].ToString()) + "&sm=" + Server.UrlEncode(_drow["TOSUITE_MODEL"].ToString()) + "&byp=2&qf=1", false);
                                            }
                                            else if (filterval1 == null && Request.QueryString["sm"] != null && _drow["TOSUITE_MODEL"].ToString() == Server.UrlDecode(Request.QueryString["sm"].ToString()) && selstate1 == false)
                                            {
                                                filterval1 = new string[2];
                                                filterval1[0] = _drow["TOSUITE_MODEL"].ToString();
                                                filterval1[1] = _drow["TOSUITE_MODEL"].ToString();
                                                _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");

                                            }
                                            else if (filterval1 != null && filterval1.ToString().ToLower() == "list all models")
                                            {
                                                Response.Redirect("family.aspx?&fid=" + fid + "&cid=" + tempCID + "&pcr=" + pcid + "&sb=" +Server.UrlEncode(Request.QueryString["sb"].ToString()) + "&byp=2&qf=1", false);

                                            }
                                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["TOSUITE_MODEL"].ToString());
                                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                                            ictrecords++;

                                        }
                                        _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistcontainer");
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
                                _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistitem");
                                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all models");
                                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all models");
                                lstrecords[0] = new TBWDataList(_stmpl_records_tmpl.ToString());
                                _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistcontainer");
                                _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                                _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
                                lstcontainers[1] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                            }
                        }
                        else
                        {
                            DisplayCompatible = false;
                            nodisplay = true;
                        }
                    }


                }
                _stg_main_container = new StringTemplateGroup("searchrsltmultilistmain", stemplatepath);
                _stmpl_main_container_tmpl = _stg_main_container.GetInstanceOf("searchbybrandFamily" + "\\" + "multilistmain");
                _stmpl_main_container_tmpl.SetAttribute("TBW_CATEGORY_NAME", tempCName);
                if (Request.Url.ToString().ToLower().Contains("product_list.aspx") == true)
                {
                    _stmpl_main_container_tmpl.SetAttribute("TOSUITE_DISPLAY", "Filter Results By Brand and Model:");
                }
                else if (Request.Url.ToString().ToLower().Contains("family.aspx") == true)
                {
                    if (DisplayCompatible == true)
                        _stmpl_main_container_tmpl.SetAttribute("TOSUITE_DISPLAY", "Compatible With:");
                    else
                        if (nodisplay == true)
                        {
                            _stmpl_main_container_tmpl.SetAttribute("TOSUITE_DISPLAY", "");
                        }
                        else
                        {
                            _stmpl_main_container_tmpl.SetAttribute("TOSUITE_DISPLAY", "Compatible With:");
                        }

                }
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
        DataSet DSUBC = null;
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
    private DataSet GetDataSetFX(string familyid)
    {
        DataSet prodtable = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter("select distinct tps.PRODUCT_ID from tb_prod_specs tps,tb_prod_family tpf where tps.product_id=tpf.product_id and tpf.family_id=" + familyid, conStr.ConnectionString.ToString().Substring(conStr.ConnectionString.ToString().IndexOf(';') + 1));
        da.Fill(prodtable, "Producttable");       
        return prodtable;
    }

}
