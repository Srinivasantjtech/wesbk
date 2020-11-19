using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradingBell.WebCat;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using StringTemplate = Antlr.StringTemplate.StringTemplate;
using StringTemplateGroup = Antlr.StringTemplate.StringTemplateGroup;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;

public class DataSetHelper
{
    public DataSet ds;
    ErrorHandler oErr = new ErrorHandler();
    public DataSetHelper(ref DataSet DataSet)
    {
        ds = DataSet;
    }
    public DataSetHelper()
    {
        ds = null;
    }
    private bool ColumnEqual(object A, object B)
    {

        // Compares two values to see if they are equal. Also compares DBNULL.Value.
        // Note: If your DataTable contains object fields, then you must extend this
        // function to handle them in a meaningful way if you intend to group on them.

        if (A == DBNull.Value && B == DBNull.Value) //  both are DBNull.Value
            return true;
        if (A == DBNull.Value || B == DBNull.Value) //  only one is DBNull.Value
            return false;
        return (A.Equals(B));  // value type standard comparison
    }
    public DataTable SelectDistinct(string TableName, DataTable SourceTable, string FieldName)
    {
        DataTable dt = new DataTable(TableName);
        try
        {
            dt.Columns.Add(FieldName, SourceTable.Columns[FieldName].DataType);

            object LastValue = null;
            foreach (DataRow dr in SourceTable.Select("", FieldName))
            {
                if (LastValue == null || !(ColumnEqual(LastValue, dr[FieldName])))
                {
                    LastValue = dr[FieldName];
                    dt.Rows.Add(new object[] { LastValue });
                }
            }
            if (ds != null)
            {
                ds.Tables.Add(dt);
            }
        }
        catch (Exception ex)
        {
            oErr.ErrorMsg = ex;
            oErr.CreateLog();
        }
        return dt;
    }

}

public partial class search_searchparametricfilter : System.Web.UI.UserControl
{
    public string sAttrIds = "";
    HelperDB oHelper = new HelperDB();
    SqlConnection oCon = null;

    DataSet dspfilters = new DataSet();
    int iCatalogId = 1;
    int iInventoryLevelCheck = 1;
    int ictrecords = 0;
    int ilistcontainers = 0;
    string sAttributeName = "";
    string stemplatepath = "";
    bool bFilterApplied = false;

    protected void Page_Load(object sender, EventArgs e)
    {
       stemplatepath = Server.MapPath(ConfigurationManager.AppSettings["StringTemplatePath"].ToString());
       try
       {
           if (Request.Url.OriginalString.ToString().ToUpper().Contains("PRODUCT_LIST"))
               stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\product_list\\";
           if (Request.Url.OriginalString.ToString().ToUpper().Contains("BYBRAND.ASPX"))
               stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\bybrand\\";
           if (Request.Url.OriginalString.ToString().ToUpper().Contains("BYPRODUCT.ASPX"))
               stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\byproduct\\";
       }
       catch (Exception ec)
       {
       }
       try
       {
          // if (Request.QueryString["__EVENTTARGET"] == "PARAMETRICFILTER" && Request.QueryString["__EVENTARGUMENT"] != null)
           if (hdnFilterType.Value == "PARAMETRICFILTER" && hdnFilterIds.Value != "")
           {
               string sFilterStr = hdnFilterIds.Value;// Request.QueryString["__EVENTARGUMENT"].ToString();
               oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["TBWebCatShoppingCartConnString"].ToString());
               oCon.Open();
               //PowerSearch ps = new PowerSearch(oCon);
               PowerSearchServices ps = new PowerSearchServices();
               ps.USER_SESSION_ID = Session.SessionID;
               ps.FILTER_STR = sFilterStr;
               ps.ApplyParametricFilters();
               bFilterApplied = true;
               hdnFilterType.Value = "";
               hdnFilterIds.Value = "";
               HttpContext.Current.Session["PARAFILTER"] = "Value";
           }
           else
           {
               HttpContext.Current.Session["PARAFILTER"] = "";
           }
       }
       catch (Exception ex)
       {
           string s = ex.Message;
       }
    }


    protected string ST_ParametricFilters()
    {
        if (bFilterApplied == false)
        {
            if (Session["PS_SEARCH_RESULTS"] == null)
            {
                return "";
            }

            if (Request.QueryString["cyid"] != null)
            {
                if (Request.QueryString["cyid"].ToString().Length <= 0)
                {
                    return "";
                }
            }
            //else
            //{
            //    return "";
            //}
        }
        StringTemplateGroup _stg_main_container = null;
        StringTemplateGroup _stg_records_container = null;
        StringTemplateGroup _stg_records = null;
        StringTemplate _stmpl_main_container_tmpl = null;
        StringTemplate _stmpl_records_container_tmpl = null;
        StringTemplate _stmpl_records_tmpl = null;
        StringTemplate _stmpl_records_tmpl2 = null;
        StringTemplate _stmpl_records_tmpl3 = null;
        string sHTML = "";
        if (Convert.ToInt32(Session["PS_SEARCH_RESULTS"].ToString()) > 0)
        {
            try
            {
                oCon = new SqlConnection(ConfigurationManager.ConnectionStrings["TBWebCatShoppingCartConnString"].ToString());
                oCon.Open();
                PowerSearch ps = new PowerSearch(oCon);
                ps.USER_SESSION_ID = Session.SessionID;
                ps.CATALOG_ID = iCatalogId;
                ps.INVENTORY_CHECK = iInventoryLevelCheck;
                ps.CATEGORY_ID = "";
                if (Request.QueryString["cyid"] != null)
                {
                    if (Request.QueryString["cyid"].ToString().Length > 0)
                    {
                        ps.CATEGORY_ID = Request.QueryString["cyid"].ToString();
                    }
                }

                TBWDataList[] lstrecords = new TBWDataList[0];
                TBWDataList[] lstrows = new TBWDataList[0];
                TBWDataList[] lstcontainers = new TBWDataList[0];
                ilistcontainers = 0;
                
                dspfilters = ps.GetParametricFilters();
                if (dspfilters != null && dspfilters.Tables.Count > 0 && (dspfilters.Tables[0].Rows.Count > 0 || dspfilters.Tables[1].Rows.Count > 0 || dspfilters.Tables[2].Rows.Count > 0 || dspfilters.Tables[3].Rows.Count > 0))
                {
                    DataTable dts = new DataTable();
                    DataSetHelper dsHelper = new DataSetHelper(ref dspfilters);
                    dts = dsHelper.SelectDistinct("DistinctAttributes", dspfilters.Tables[0], "ATTRIBUTE_ID");

                    DataTable dts2 = new DataTable();
                    DataSetHelper dsHelper2 = new DataSetHelper(ref dspfilters);
                    dts2 = dsHelper2.SelectDistinct("DistinctAttributes2", dspfilters.Tables[1], "ATTRIBUTE_ID");

                    DataSetHelper dsHelper3 = new DataSetHelper(ref dspfilters);
                    DataTable dts3 = new DataTable();
                    dts3 = dsHelper3.SelectDistinct("DistinctAttributes3", dspfilters.Tables[2], "ATTRIBUTE_ID");

                    _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                    _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);

                    lstcontainers = new TBWDataList[dts.Rows.Count + dts2.Rows.Count + dts3.Rows.Count + 1];
                    DataSet DSNFields = ps.GetNarrowFields();
                    foreach (DataRow dnf in DSNFields.Tables[0].Rows)
                    {                            
                        foreach (DataRow dr in dts.Rows)
                            if (dnf["ATTRIBUTE_ID"].ToString() == dr["ATTRIBUTE_ID"].ToString())
                        {
                            ictrecords = 0;
                            DataRow[] drtemp = dspfilters.Tables[0].Select("ATTRIBUTE_ID = " + dr["ATTRIBUTE_ID"].ToString());
                            lstrecords = new TBWDataList[drtemp.Length + 1];
                            foreach (DataRow drs in dspfilters.Tables[0].Select("ATTRIBUTE_ID = " + dr["ATTRIBUTE_ID"].ToString()))
                            {
                                _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchrsltparametricfilter" + "\\" + "multilistitem");

                                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", drs["STRING_VALUE"].ToString());

                                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", drs["STRING_VALUE"].ToString());
                                if (drs["TITLE"].ToString().Trim().Length == 0)
                                    sAttributeName = drs["ATTRIBUTE_NAME"].ToString();
                                else
                                    sAttributeName = drs["TITLE"].ToString();
                                lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                                ictrecords++;
                            }
                            _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchrsltparametricfilter" + "\\" + "multilistcontainer");
                            _stmpl_records_container_tmpl.SetAttribute("TBW_ATTRIBUTE_NAME", sAttributeName);
                            _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                            //to set drildown control ids and needed to generate parametric list-M/A
                            _stmpl_records_container_tmpl.SetAttribute("TBW_ATTRIBUTE_ID", dr["ATTRIBUTE_ID"].ToString());
                            sAttrIds += dr["ATTRIBUTE_ID"].ToString() + ",";
                            //END
                            lstcontainers[ilistcontainers] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                            ilistcontainers++;
                        }

                        foreach (DataRow dr in dts2.Rows)
                            if (dnf["ATTRIBUTE_ID"].ToString() == dr["ATTRIBUTE_ID"].ToString())
                        {
                            ictrecords = 0;
                            DataRow[] drtemp = dspfilters.Tables[1].Select("ATTRIBUTE_ID = " + dr["ATTRIBUTE_ID"].ToString());
                            lstrecords = new TBWDataList[drtemp.Length + 1];
                            foreach (DataRow drs in dspfilters.Tables[1].Select("ATTRIBUTE_ID = " + dr["ATTRIBUTE_ID"].ToString()))
                            {
                                _stmpl_records_tmpl2 = _stg_records.GetInstanceOf("searchrsltparametricfilter" + "\\" + "multilistitem");
                                _stmpl_records_tmpl2.SetAttribute("TBW_LIST_VAL1", Math.Round(Convert.ToDecimal(drs["NUMERIC_VALUE"]), 2).ToString());
                                _stmpl_records_tmpl2.SetAttribute("TBW_LIST_VAL", Math.Round(Convert.ToDecimal(drs["NUMERIC_VALUE"]), 2).ToString());
                                if (drs["TITLE"].ToString().Trim().Length == 0)
                                    sAttributeName = drs["ATTRIBUTE_NAME"].ToString();
                                else
                                    sAttributeName = drs["TITLE"].ToString();
                                lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl2.ToString());
                                ictrecords++;
                            }
                            _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchrsltparametricfilter" + "\\" + "multilistcontainer");
                            _stmpl_records_container_tmpl.SetAttribute("TBW_ATTRIBUTE_NAME", sAttributeName);
                            _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                            _stmpl_records_container_tmpl.SetAttribute("TBW_ATTRIBUTE_ID", dr["ATTRIBUTE_ID"].ToString());
                            sAttrIds += dr["ATTRIBUTE_ID"].ToString() + ",";
                            lstcontainers[ilistcontainers] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                            ilistcontainers++;
                        }

                        foreach (DataRow dr in dts3.Rows)
                            if (dnf["ATTRIBUTE_ID"].ToString() == dr["ATTRIBUTE_ID"].ToString())
                        {
                            ictrecords = 0;
                            DataRow[] drtemp = dspfilters.Tables[2].Select("ATTRIBUTE_ID = " + dr["ATTRIBUTE_ID"].ToString());
                            lstrecords = new TBWDataList[drtemp.Length + 1];
                            foreach (DataRow drs in dspfilters.Tables[2].Select("ATTRIBUTE_ID = " + dr["ATTRIBUTE_ID"].ToString()))
                            {
                                _stmpl_records_tmpl3 = _stg_records.GetInstanceOf("searchrsltparametricfilter" + "\\" + "multilistitem");
                                _stmpl_records_tmpl3.SetAttribute("TBW_LIST_VAL1", drs["RANGE_NAME"].ToString());
                                _stmpl_records_tmpl3.SetAttribute("TBW_LIST_VAL", drs["RANGE_NAME"].ToString());
                                if (drs["TITLE"].ToString().Trim().Length == 0)
                                    sAttributeName = drs["ATTRIBUTE_NAME"].ToString();
                                else
                                    sAttributeName = drs["TITLE"].ToString();
                                lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl3.ToString());
                                ictrecords++;
                            }
                            _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchrsltparametricfilter" + "\\" + "multilistcontainer");
                            _stmpl_records_container_tmpl.SetAttribute("TBW_ATTRIBUTE_NAME", sAttributeName);
                            _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                            //to set drildown control ids and needed to generate parametric list-M/A
                            _stmpl_records_container_tmpl.SetAttribute("TBW_ATTRIBUTE_ID", dr["ATTRIBUTE_ID"].ToString());
                            sAttrIds += dr["ATTRIBUTE_ID"].ToString() + ",";
                            //END
                            lstcontainers[ilistcontainers] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                            ilistcontainers++;
                        }
                    }
                    _stg_main_container = new StringTemplateGroup("searchrsltmultilistmain", stemplatepath);
                    _stmpl_main_container_tmpl = _stg_main_container.GetInstanceOf("searchrsltparametricfilter" + "\\" + "multilistmain");
                    _stmpl_main_container_tmpl.SetAttribute("TBWDataList", lstcontainers);

                     sHTML = _stmpl_main_container_tmpl.ToString();
                    //if (dspfilters.Tables[0].Rows.Count == 0)
                        //sHTML = "";
                }

                string sql = string.Empty;
                string UserSession = HttpContext.Current.Session.SessionID;
                sql = "Delete From TBWC_SEARCH_PROD_LIST WHERE USER_SESSION_ID ='" + UserSession + "'";
                SqlCommand cmd = new SqlCommand(sql, oCon);
                cmd.ExecuteNonQuery();
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
