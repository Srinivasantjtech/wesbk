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
public partial class search_searchbyproduct : System.Web.UI.UserControl
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
    int ictrecords=0;

    protected void Page_Load(object sender, EventArgs e)
    {
        stemplatepath = Server.MapPath(ConfigurationManager.AppSettings["StringTemplatePath"].ToString());
        try
        {
            if (Request.Url.OriginalString.ToString().ToUpper().Contains("PRODUCT_LIST"))
               stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\product_list\\";
            if (Request.Url.OriginalString.ToString().ToUpper().Contains("BYBRAND.ASPX"))
                stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\bybrand\\";
            if (Request.Url.OriginalString.ToString().ToUpper().Contains("BYPRODUCT.ASPX") || Request.Url.OriginalString.ToString().ToUpper().Contains("CATEGORYLIST.ASPX") || Request.Url.OriginalString.ToString().ToUpper().Contains("PRODUCT_LIST.ASPX"))
                stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\byproduct\\";
        }
        catch (Exception ec)
        {
        }
    }

    protected string ST_byproduct()
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
        string[] filterval = null;
        string[] filterval1 = null;
        string[] filterval2 = null;
        oHelper = new HelperDB();
        oErr = new ErrorHandler();
        oCat = new Category();
        oPF = new ProductFamily();
        oPR = new ProductRender();        
        string sHTML = "";
        string _catid = "";
        string _catname="";
        int _familyid=0;
        try
        {
            if (hidcatIds.Value != string.Empty && hidcatIds.Value != null)
            {
                filterval = hidcatIds.Value.Split('^');
            }
            if (HidsubcatIds.Value != string.Empty && HidsubcatIds.Value != null)
            {
                filterval1 = HidsubcatIds.Value.Split('^');
            }
            if (HidfamIds.Value != string.Empty && HidfamIds.Value != null)
            {
                filterval2 = HidfamIds.Value.Split('^');
            }
            if (Request.QueryString["cid"] != null && Request.QueryString["sldummy"] != null)//sldummy is to skip
            {
                if (Request.QueryString["cid"].ToString().Length > 0)
                {
                    if (Request.Url.OriginalString.ToString().ToUpper().Contains("BYPRODUCT.ASPX"))
                        _catid = Getdropdwoncatid(Request.QueryString["cid"].ToString());
                    else
                        _catid = Request.QueryString["cid"].ToString();
                    DataSet _dscatname = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + _catid + "'");
                    if (_dscatname != null && _dscatname.Tables[0].Rows.Count > 0)
                        _catname = _dscatname.Tables[0].Rows[0][0].ToString();
                    tempCName = GetCName(Request.QueryString["cid"].ToString());
                    tempCID = GetCID(Request.QueryString["cid"].ToString());
                    if (tempCID == "0")
                    {
                        return "";
                    }
                }
                if (Request.QueryString["fid"] != null)
                {
                    _familyid = Convert.ToInt32(Request.QueryString["fid"]);
                }

               
               

                DataSet dsCat = new DataSet();
                dsCat = oCat.GetSubCategories(tempCID.ToString(), Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()));

                if (dsCat != null && dsCat.Tables.Count > 0 && (dsCat.Tables[0].Rows.Count > 0))
                {
                    ictrecords = 0;
                    lstrecords = new TBWDataList[dsCat.Tables[0].Rows.Count + 1];
                    _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                    _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                    _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbyproduct" + "\\" + "multilistitem");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "Select Type");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "Select Type");
                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                    ictrecords++;
                    bool selstate = false;
                    foreach (DataRow _drow in dsCat.Tables[0].Rows)
                    {
                        _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbyproduct" + "\\" + "multilistitem");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["CATEGORY_ID"].ToString());
                        if (filterval != null && _drow["CATEGORY_ID"].ToString() == filterval[0].ToString())
                        {
                            _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                            selstate = true;
                            Response.Redirect("byproduct.aspx?&id=0&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=3&qf=1", false);
                        }
                        else if (filterval == null && _catname.ToUpper() == _drow["CATEGORY_NAME"].ToString().ToUpper() && selstate == false)
                        {
                            filterval = new string[2];
                            filterval[0] = _drow["CATEGORY_ID"].ToString();
                            filterval[1] = _drow["CATEGORY_NAME"].ToString();
                            _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                            selstate = true;
                        }
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["CATEGORY_NAME"].ToString());
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                        ictrecords++;

                    }
                    _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbyproduct" + "\\" + "multilistcontainer");
                    _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                    _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "1");
                    lstcontainers[0] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                    if (selstate == false && !(Request.Url.OriginalString.ToString().ToUpper().Contains("CATEGORYLIST")))
                    {
                        return "";
                    }
                }
                /*
                DataSet dsCat1 = new DataSet();
                if (filterval != null && filterval[0].ToString().Length > 0)
                {
                    tempCID = filterval[0].ToString();

                    dsCat1 = GetDataSet("SELECT * FROM Category_Function (" + oHelper.GetOptionValues("DEFAULT CATALOG").ToString() + ",'" + tempCID + "')");
                    if (dsCat1 != null && dsCat1.Tables.Count > 0 && (dsCat1.Tables[0].Rows.Count > 0))
                    {
                        ictrecords = 0;
                        DataRow[] _DCRow = null;
                        if (filterval == null)
                            _DCRow = dsCat1.Tables[0].Select("HIERARCHY_LEVEL>1");
                        else
                            _DCRow = dsCat1.Tables[0].Select("HIERARCHY_LEVEL>0");
                        lstrecords = new TBWDataList[_DCRow.Count() + 1];
                        _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                        _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                        _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbyproduct" + "\\" + "multilistitem");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "Select Brand");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "Select Brand");
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                        ictrecords++;
                        bool selstate = false;
                        foreach (DataRow _drow in _DCRow)
                        {
                            _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbyproduct" + "\\" + "multilistitem");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["CATEGORY_ID"].ToString());
                            if (filterval1 != null && _drow["CATEGORY_ID"].ToString() == filterval1[0].ToString())
                            {
                                _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                selstate = true;
                            }
                            else if (Request.Url.OriginalString.ToString().ToUpper().Contains("BYPRODUCT.ASPX"))
                            {
                                if (filterval1 == null && Request.QueryString["cid"].ToString().ToUpper() == _drow["CATEGORY_ID"].ToString().ToUpper() && selstate == false)
                                {
                                    filterval1 = new string[2];
                                    filterval1[0] = _drow["CATEGORY_ID"].ToString();
                                    filterval1[1] = _drow["CATEGORY_NAME"].ToString();
                                    _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");

                                }
                            }
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["CATEGORY_NAME"].ToString());
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                            ictrecords++;

                        }
                        _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbyproduct" + "\\" + "multilistcontainer");
                        _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                        _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
                        lstcontainers[1] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                    }
                }
                else
                {
                    lstrecords = new TBWDataList[ 1];
                    _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                    _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                    _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbyproduct" + "\\" + "multilistitem");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "Select Brand");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "Select Brand");
                    lstrecords[0] = new TBWDataList(_stmpl_records_tmpl.ToString());
                    _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbyproduct" + "\\" + "multilistcontainer");
                    _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                    _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
                    lstcontainers[1] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                }
                */
                string catIDLiSt = "'0'";
                if (filterval != null && filterval[0].ToString().Length > 0)
                {
                    DataSet dsCattemp = new DataSet();
                    dsCattemp = GetDataSet("SELECT CATEGORY_ID FROM Category_Function (" + oHelper.GetOptionValues("DEFAULT CATALOG").ToString() + ",'" + filterval[0].ToString() + "')");
                    foreach (DataRow _drow in dsCattemp.Tables[0].Rows)
                    {
                        catIDLiSt = catIDLiSt + ",'" + _drow["category_Id"].ToString() + "'";
                    }

                    /*else
                    {
                    
                        foreach (DataRow _drow in dsCat1.Tables[0].Rows)
                        {
                            catIDLiSt = catIDLiSt + ",'" + _drow["category_Id"].ToString() + "'";
                        }
                    }*/

                    DataSet dsCat2 = new DataSet();
                    dsCat2 = GetDataSet("SELECT DISTINCT F.FAMILY_ID,F.FAMILY_NAME,F.CATEGORY_ID FROM TB_FAMILY F,TB_CATALOG_FAMILY CF where CF.FAMILY_ID=F.FAMILY_ID AND F.CATEGORY_ID IN(" + catIDLiSt + ") AND CF.CATALOG_ID=" + oHelper.GetOptionValues("DEFAULT CATALOG").ToString());
                    if (dsCat2 != null && dsCat2.Tables.Count > 0 && (dsCat2.Tables[0].Rows.Count > 0))
                    {
                        ictrecords = 0;

                        lstrecords = new TBWDataList[dsCat2.Tables[0].Rows.Count + 1];
                        _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                        _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                        _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbyproduct" + "\\" + "multilistitem");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "All Models");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "All Models");
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                        ictrecords++;
                        foreach (DataRow _drow in dsCat2.Tables[0].Rows)
                        {
                            _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbyproduct" + "\\" + "multilistitem");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["CATEGORY_ID"].ToString() + "|" + _drow["FAMILY_ID"].ToString());
                            if (Convert.ToInt32(_drow["FAMILY_ID"]) == _familyid)
                            {
                                _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                            }
                            else if (filterval2 != null && _drow["CATEGORY_ID"].ToString() + "|" + _drow["FAMILY_ID"].ToString() == filterval2[0].ToString())
                                _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["FAMILY_NAME"].ToString());
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                            ictrecords++;

                        }
                        _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbyproduct" + "\\" + "multilistcontainer");
                        _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                        _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
                        lstcontainers[2] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                    }
                }
                else
                {
                    lstrecords = new TBWDataList[1];
                    _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                    _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                    _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbyproduct" + "\\" + "multilistitem");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "All Models");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "All Models");
                    lstrecords[0] = new TBWDataList(_stmpl_records_tmpl.ToString());
                    _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbyproduct" + "\\" + "multilistcontainer");
                    _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                    _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
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
                _stmpl_main_container_tmpl = _stg_main_container.GetInstanceOf("searchbyproduct" + "\\" + "multilistmain");
                _stmpl_main_container_tmpl.SetAttribute("TBW_CATEGORY_NAME", tempCName);
                _stmpl_main_container_tmpl.SetAttribute("TBWDataList", lstcontainers);

                sHTML = _stmpl_main_container_tmpl.ToString();
                //if (dspfilters.Tables[0].Rows.Count == 0)
                //sHTML = "";
            }
            else
            {

                tempCName = GetCName(Request.QueryString["cid"].ToString());
                tempCID = Request.QueryString["cid"].ToString();
                if (Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "")
                {
                    DataSet dsCat = new DataSet();
                    dsCat = null; //oCat.GetSubCategoriesL2ProductWES(tempCID.ToString(), Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()));                   
                    if (dsCat != null && dsCat.Tables.Count > 0 && (dsCat.Tables[0].Rows.Count > 0))
                    {
                        ictrecords = 0;
                        lstrecords = new TBWDataList[dsCat.Tables[0].Rows.Count + 1];
                        _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                        _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                        _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbyproduct" + "\\" + "multilistitem");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "Select Type");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "Select Type");
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                        ictrecords++;
                        bool selstate = false;
                        foreach (DataRow _drow in dsCat.Tables[0].Rows)
                        {
                            _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbyproduct" + "\\" + "multilistitem");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["CATEGORY_ID"].ToString());
                            if (filterval != null && _drow["CATEGORY_ID"].ToString() == filterval[0].ToString())
                            {
                                _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                selstate = true;
                                Response.Redirect("byproduct.aspx?&id=0&cid=" + tempCID + "&sl2=" + _drow["CATEGORY_ID"].ToString() + "&byp=2", false);
                            }
                            else if (filterval == null && Request.QueryString["sl2"] != null && Request.QueryString["sl2"].ToString() != "" && _drow["CATEGORY_ID"].ToString() == Request.QueryString["sl2"].ToString() && selstate == false)
                            {
                                filterval = new string[2];
                                filterval[0] = _drow["CATEGORY_ID"].ToString();
                                filterval[1] = _drow["CATEGORY_NAME"].ToString();
                                _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                selstate = true;
                            }
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["CATEGORY_NAME"].ToString());
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                            ictrecords++;

                        }
                        _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbyproduct" + "\\" + "multilistcontainer");
                        _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                        _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "1");
                        lstcontainers[0] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                        //if (selstate == false && !(Request.Url.OriginalString.ToString().ToUpper().Contains("CATEGORYLIST")))
                        //{
                        //    return "";
                        //}
                    }
                }                
                
                if (Request.QueryString["sl2"] != null && Request.QueryString["sl2"].ToString() != "")
                {
                    DataSet dsCat2 = new DataSet();                    
                    bool selstate1 = false;
                    string sqlQuery = "SELECT DISTINCT FAMILY_ID,FAMILY_NAME,SUBCATID_L2 AS CATEGORY_ID FROM WESTB_TOSUITE_DATA WHERE CATEGORY_ID='" + tempCID + "' AND SUBCATNAME_L1='PRODUCT' AND SUBCATID_L2='" + Request.QueryString["sl2"].ToString() + "'" +
                                    " AND FAMILY_ID IS NOT NULL ORDER BY FAMILY_NAME";
                    dsCat2 = GetDataSet(sqlQuery);
                    if (dsCat2 != null && dsCat2.Tables.Count > 0 && (dsCat2.Tables[0].Rows.Count > 0))
                    {
                        ictrecords = 0;

                        lstrecords = new TBWDataList[dsCat2.Tables[0].Rows.Count + 1];
                        _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                        _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                        _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbyproduct" + "\\" + "multilistitem");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "All Models");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "All Models");
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                        ictrecords++;
                        foreach (DataRow _drow in dsCat2.Tables[0].Rows)
                        {
                            _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbyproduct" + "\\" + "multilistitem");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["FAMILY_ID"].ToString());//_drow["CATEGORY_ID"].ToString() + "|" + _drow["FAMILY_ID"].ToString());
                            if (filterval1 != null && _drow["FAMILY_ID"].ToString() == filterval1[0].ToString())
                            {
                                _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                selstate1 = true;                                
                                //Response.Redirect("byproduct.aspx?&id=0&cid=" + tempCID + "&sl2=" + Request.QueryString["sl2"].ToString() + "&fid=" + _drow["FAMILY_ID"].ToString() + "&byp=2&bypcat=1", false);
                            }
                            else if (filterval1 == null && Request.QueryString["fid"] != null && Request.QueryString["fid"].ToString() != "" && _drow["FAMILY_ID"].ToString() == Request.QueryString["fid"].ToString() && selstate1 == false)
                            {
                                filterval1 = new string[2];
                                filterval1[0] = _drow["FAMILY_ID"].ToString();
                                filterval1[1] = _drow["FAMILY_NAME"].ToString();
                                _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");

                            }                    
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["FAMILY_NAME"].ToString());
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                            ictrecords++;

                        }
                        _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbyproduct" + "\\" + "multilistcontainer");
                        _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                        _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
                        lstcontainers[2] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                    }
                }
                else
                {
                    lstrecords = new TBWDataList[2];
                    _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                    _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                    _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbyproduct" + "\\" + "multilistitem");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "All Models");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "All Models");
                    lstrecords[0] = new TBWDataList(_stmpl_records_tmpl.ToString());
                    _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbyproduct" + "\\" + "multilistcontainer");
                    _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                    _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
                    lstcontainers[2] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                }

                _stg_main_container = new StringTemplateGroup("searchrsltmultilistmain", stemplatepath);
                _stmpl_main_container_tmpl = _stg_main_container.GetInstanceOf("searchbyproduct" + "\\" + "multilistmain");
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
    private StringTemplate LoadModel(DataSet dsCat, StringTemplate _stmpl_records_tmpl2, StringTemplate _stmpl_records_container_tmpl)
    {
        foreach (DataRow _drow in dsCat.Tables[0].Rows)
        {
          _stmpl_records_container_tmpl= Loadsubmodel(_drow["CATEGORY_ID"].ToString(), _stmpl_records_tmpl2, _stmpl_records_container_tmpl);
        }
       
        

        return _stmpl_records_container_tmpl; 
    }
    private StringTemplate Loadsubmodel(string _catid,StringTemplate _stmpl_records_tmpl2, StringTemplate _stmpl_records_container_tmpl)
    {
        DataSet dsCat1 = new DataSet();
        dsCat1 = oCat.GetSubCategories(tempCID.ToString(), Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()));

        ictrecords = 0;
        TBWDataList[] lstrecords=new TBWDataList[dsCat1.Tables[0].Rows.Count];
        StringTemplateGroup _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
        StringTemplateGroup _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
        if (dsCat1 != null && dsCat1.Tables.Count > 0 && (dsCat1.Tables[0].Rows.Count > 0))
        {
            foreach (DataRow _drow in dsCat1.Tables[0].Rows)
            {                

                _stmpl_records_tmpl2.SetAttribute("TBW_LIST_VAL1", _drow["CATEGORY_ID"].ToString());

                _stmpl_records_tmpl2.SetAttribute("TBW_LIST_VAL", _drow["CATEGORY_NAME"].ToString());
                lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl2.ToString());
                ictrecords++;            
              // _stmpl_records_container_tmpl= Loadsubmodel(_drow["CATEGORY_ID"].ToString(), _stmpl_records_tmpl2,_stmpl_records_container_tmpl);
            }
            _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
        }
        return _stmpl_records_container_tmpl;

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
                    DSUBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY = '" + DR["CATEGORY_ID"].ToString() + "' AND CATEGORY_NAME Like 'Product'");
                    if (DSUBC.Tables[0].Rows.Count > 0)
                        return DSUBC.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                    else
                        return "0";
                }               
            }
        } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");       
        return DSBC.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
    }
    private string CheckCatID(string catID)
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
                if (DR["CATEGORY_NAME"].ToString() == "Product")
                {
                    return DR["CATEGORY_ID"].ToString();
                }
            }
        } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
        return "0";
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
}
