
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient; 
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UC_productlist : System.Web.UI.UserControl
{
    ConnectionDB objConnectionDB = new ConnectionDB();
     EasyAsk_WES objEasyAsk = new EasyAsk_WES();
     HelperDB objHelperDB = new HelperDB();
    DataSet dscat = new DataSet();
    string stemplatepath = string.Empty;
    string _AttType = string.Empty;
    string _AttValue = string.Empty;
    int _resultpage=12;
    int _PageNo=1;
    string ParentCatID = string.Empty;
    string _Brand = string.Empty;
    string _searchstr = string.Empty;
    ErrorHandler objErrorHandler = new ErrorHandler();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            
         stemplatepath = Server.MapPath(ConfigurationManager.AppSettings["StringTemplatePath"].ToString());
        
        //    if (Request.QueryString["type"]!=null)
        //   _AttType= Request.QueryString["Type"];
        //if (Request.QueryString["value"]!=null)
        //   _AttValue= Request.QueryString["Value"];
        //if (Request.QueryString["bname"] != null)
        //    _Brand = Request.QueryString["bname"];
        //if (Request.QueryString["pgno"]!=null)
        //   _PageNo= Convert.ToInt32(Request.QueryString["pgno"]);

        //if (Request.QueryString["searchstr"] != null)
        //    _searchstr = Request.QueryString["searchstr"];

        //if (Session["RECORDS_PER_PAGE"]!= null)
        //    _resultpage=Convert.ToInt32(HttpContext.Current.Session["RECORDS_PER_PAGE"]) ;

        // if (Request.QueryString["pcr"] != null)
        // {
        //    ParentCatID=Request.QueryString["pcr"] ;             
        // }

         if (!IsPostBack)
         {
             hforgurl.Value = HttpContext.Current.Request.Url.PathAndQuery.ToString();
             HiddenField1.Value = "0";
             HiddenField2.Value = "0";
             hfcheckload.Value = "0";
             HFcnt.Value = "1";

             hfback.Value = string.Empty;
             hfbackdata.Value = string.Empty;
         }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
          
        }
       // if (!IsPostBack)    
         //EasyAsk.GetAttributeProducts(_searchstr,_AttType, _AttValue, _Brand, _resultpage.ToString(), (_PageNo - 1).ToString(), "Next");

    }
    public string Bread_Crumbs()
    {
      try
      {
        string breadcrumb = "", paraPID = "", paraFID = "", paraCID = "";
        if (Request.QueryString["pid"] != null)
        {
            paraPID = Request.QueryString["pid"].ToString();
        }
        if (Request.QueryString["fid"] != null)
            paraFID = Request.QueryString["pid"].ToString();
        if (Request.QueryString["cid"] != null)
            paraCID = Request.QueryString["cid"].ToString();
        if (paraCID=="")
            paraCID = ParentCatID;

        // jtech
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
        //            if(breadcrumb=="")
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
        //        DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
        //        foreach (DataRow DR in DSBC.Tables[0].Rows)
        //        {
        //            if (DR["PARENT_CATEGORY"].ToString() != "0")
        //            {
        //                if (breadcrumb == "")
        //                {
        //                    if (Request.QueryString["sl1"] != null && Request.QueryString["sl1"].ToString() != "" && Request.QueryString["sl2"] != null && Request.QueryString["sl2"].ToString() != "")
        //                    {
        //                        //breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + catIDtemp + "&byp=2&bypcat=1\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
        //                        breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + catIDtemp + "&byp=2\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
        //                        string sql = "SELECT DISTINCT SUBCATNAME_L1 + ' - ' + SUBCATNAME_L2 FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE CATEGORY_ID='" + Request.QueryString["cid"].ToString() + "' AND SUBCATID_L1='" + Request.QueryString["sl1"].ToString() + "' AND SUBCATID_L2='" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "'";
        //                        breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font><a href=\"product_list.aspx?&ld=0&cid=" + catIDtemp + "&sl1=" + Request.QueryString["sl1"].ToString() + "&sl2=" + Request.QueryString["sl2"].ToString() + "&byp=2&qf=1\" style=\"color:Black;\">" + GetDataSet(sql).Tables[0].Rows[0][0].ToString()  + "</a>";
        //                    }
        //                    else
        //                    {
        //                        if (Request.QueryString["pcr"] != null && Request.QueryString["pcr"].ToString() != "")
        //                        {
        //                            breadcrumb = "<a href=\"product_list.aspx?&ld=0&cid=" + catIDtemp + "&pcr=" + Request.QueryString["pcr"].ToString() + "&byp=2&qf=1\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
        //                        }
        //                        else
        //                        {
        //                            breadcrumb = "<a href=\"product_list.aspx?&ld=0&cid=" + catIDtemp + "\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    if (Request.QueryString["pcr"] != null && Request.QueryString["pcr"].ToString() != "")
        //                    {
        //                        breadcrumb = "<a href=\"product_list.aspx?&ld=0&cid=" + DR["CATEGORY_ID"].ToString() + "&pcr=" + Request.QueryString["pcr"].ToString() + "&byp=2&qf=1\"  style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
        //                    }
        //                    else
        //                    {
        //                        breadcrumb = "<a href=\"product_list.aspx?&ld=0&cid=" + DR["CATEGORY_ID"].ToString() + "\"  style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (breadcrumb == "")
        //                    if (Request.QueryString["sl1"] != null && Request.QueryString["sl1"].ToString() != "" && Request.QueryString["sl2"] != null && Request.QueryString["sl2"].ToString() != "")
        //                    {
        //                        //breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + catIDtemp + "&byp=2&bypcat=1\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
        //                        breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + catIDtemp + "&byp=2\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
        //                        string sql = "SELECT DISTINCT SUBCATNAME_L1 + ' - ' + SUBCATNAME_L2 FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE CATEGORY_ID='" + Request.QueryString["cid"].ToString() + "' AND SUBCATID_L1='" + Request.QueryString["sl1"].ToString() + "' AND SUBCATID_L2='" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "'";
        //                        breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font><a href=\"product_list.aspx?&ld=0&cid=" + catIDtemp + "&sl1=" + Request.QueryString["sl1"].ToString() + "&sl2=" + Request.QueryString["sl2"].ToString() + "&byp=2&qf=1\" style=\"color:Black;\">" + GetDataSet(sql).Tables[0].Rows[0][0].ToString() + "</a>";
        //                    }
        //                    else
        //                    {
        //                        if (Request.QueryString["pcr"] != null && Request.QueryString["pcr"].ToString() != "")
        //                        {
        //                            //breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + catIDtemp + "&pcr=" + Request.QueryString["pcr"].ToString() + "&byp=2&bypcat=1\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
        //                            breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + catIDtemp + "&pcr=" + Request.QueryString["pcr"].ToString() + "&byp=2\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
        //                        }
        //                        else
        //                        {
        //                            breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
        //                        }
        //                    }
        //                else
        //                {
        //                    if (Request.QueryString["pcr"] != null && Request.QueryString["pcr"].ToString() != "")
        //                    {
        //                        //breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + DR["CATEGORY_ID"].ToString() + "&pcr=" + Request.QueryString["pcr"].ToString() + "&byp=2&bypcat=1\"  style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
        //                        breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + DR["CATEGORY_ID"].ToString() + "&pcr=" + Request.QueryString["pcr"].ToString() + "&byp=2\"  style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
        //                    }
        //                    else
        //                    {
        //                        breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + DR["CATEGORY_ID"].ToString() + "\"  style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
        //                    }
        //                }

        //            }
        //            catIDtemp = DR["PARENT_CATEGORY"].ToString();
        //        }
        //    } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
        //}
        breadcrumb = objEasyAsk.GetBreadCrumb(Server.MapPath("Templates"));       
        return breadcrumb;
      }
      catch (Exception ex)
      {
          objErrorHandler.ErrorMsg = ex;
          objErrorHandler.CreateLog();
          return string.Empty; 
      }
    }

    //private DataSet GetDataSet(string SQLQuery)
    //{
    //    DataSet ds = new DataSet();
    //    SqlDataAdapter da = new SqlDataAdapter(SQLQuery, conStr.ConnectionString.Substring(conStr.ConnectionString.IndexOf(';') + 1));
    //    da.Fill(ds, "generictable");
    //    return ds;
    //}


    //  protected string ST_Categories()
    //{

    //    string sHTML = "";
    //     try
    //        {
  
              
    //            StringTemplateGroup _stg_container = null;
    //            StringTemplateGroup _stg_records = null;
    //            StringTemplate _stmpl_container = null;
    //            StringTemplate _stmpl_records = null;
    //            StringTemplate _stmpl_records1 = null;
    //            StringTemplate _stmpl_recordsrows = null;
    //            TBWDataList[] lstrecords = new TBWDataList[0];
    //            TBWDataList[] lstrows = new TBWDataList[0];

    //            StringTemplateGroup _stg_container1 = null;
    //            StringTemplateGroup _stg_records1 = null;                
    //            TBWDataList1[] lstrecords1 = new TBWDataList1[0];
    //            TBWDataList1[] lstrows1 = new TBWDataList1[0];
    //            int ictrows = 0;

                

    //            dscat = (DataSet)HttpContext.Current.Session["LHSAttributes"];
     
    //            _stg_records = new StringTemplateGroup("searchrsltcategoryleftrecords", stemplatepath);
    //            _stg_container = new StringTemplateGroup("searchrsltcategoryleftmain", stemplatepath);
    //            lstrows = new TBWDataList[dscat.Tables.Count + 1];
                
    //            for (int i = 0; i < dscat.Tables.Count; i++)
    //            {
    //                Boolean tmpallow = true;
    //                if (dscat.Tables[i].TableName.Contains("Category"))
    //                    tmpallow = true;
    //                else if (dscat.Tables[i].TableName.Contains("Brand") )
    //                    tmpallow =false;
    //                else if (Request.QueryString["byp"] == "2")                        
    //                    tmpallow = true;
    //                else
    //                    tmpallow =false ;

    //                if(tmpallow==true )
    //                {
    //                    if (dscat.Tables[i].Rows.Count > 0)
    //                    {
    //                        lstrecords = new TBWDataList[dscat.Tables[i].Rows.Count + 1];
    //                        lstrecords1 = new TBWDataList1[dscat.Tables[i].Rows.Count + 1];
    //                        int ictrecords = 0;

    //                        int j = 0;
    //                        foreach (DataRow dr in dscat.Tables[i].Rows)//For Records
    //                        {



    //                            _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryleft" + "\\" + "cell");
    //                            if (dscat.Tables[i].TableName.Contains("Category"))
    //                            {
    //                                _stmpl_records.SetAttribute("TBW_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(ParentCatID.ToString()));
    //                                _stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(dr["CATEGORY_ID"].ToString()));
    //                                _stmpl_records.SetAttribute("TBW_ATTRIBUTE_NAME", dr["Category_Name"].ToString());

    //                            }
    //                            else
    //                            {
    //                                _stmpl_records.SetAttribute("TBW_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(ParentCatID.ToString()));
    //                                _stmpl_records.SetAttribute("TBW_CATEGORY_ID", "");
    //                                _stmpl_records.SetAttribute("TBW_ATTRIBUTE_NAME", dr[0].ToString());
    //                            }
    //                            _stmpl_records.SetAttribute("TBW_ATTRIBUTE_TYPE", dscat.Tables[i].TableName.ToString());
    //                            if (HttpContext.Current.Session["EA"] != null)
    //                            {
    //                                _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(oHelper.StringEnCrypt(HttpContext.Current.Session["EA"].ToString())));
    //                            }
    //                            if (dscat.Tables[i].TableName.Contains("Category"))
    //                            {
    //                                if (ictrecords <= 9)
    //                                {
    //                                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
    //                                }
    //                                else
    //                                {
    //                                    lstrecords1[ictrecords] = new TBWDataList1(_stmpl_records.ToString());
    //                                }
    //                            }
    //                            else
    //                            {
    //                                lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
    //                            }
    //                            ictrecords++;
    //                        }

    //                        j++;
    //                        //if (dscat_full.Tables[i].Rows.Count > 0)
    //                        //{
    //                        //    _stmpl_recordsrows.SetAttribute("TBW_LINK", "<h3 class=expand id='" + dscat.Tables[i].TableName.ToString().Replace("/", "").Replace(" ", "") + "1' onclick=showHide('" + dscat.Tables[i].TableName.ToString().Replace("/", "").Replace(" ", "") + "');return false;>Show More Options</h3>");
    //                        //}
    //                        if (dscat.Tables[i].TableName.Contains("Category"))
    //                        {
    //                            if (ictrecords > 9)
    //                                _stmpl_recordsrows = _stg_container.GetInstanceOf("searchrsltcategoryleft" + "\\" + "row");
    //                            else
    //                                _stmpl_recordsrows = _stg_container.GetInstanceOf("searchrsltcategoryleft" + "\\" + "row1");
    //                        }
    //                        else
    //                        {
    //                            _stmpl_recordsrows = _stg_container.GetInstanceOf("searchrsltcategoryleft" + "\\" + "row1");
    //                        }
    //                        _stmpl_recordsrows.SetAttribute("TBT_ATTRIBUTE_TITLE", dscat.Tables[i].TableName.ToString());
    //                        _stmpl_recordsrows.SetAttribute("TBT_ATTRIBUTE_ID", dscat.Tables[i].TableName.ToString().Replace("/", "").Replace(" ", ""));
    //                        _stmpl_recordsrows.SetAttribute("TBWDataList", lstrecords);
    //                        _stmpl_recordsrows.SetAttribute("TBWDataList1", lstrecords1);
    //                        lstrows[ictrows] = new TBWDataList(_stmpl_recordsrows.ToString());
    //                        ictrows++;
    //                    }
    //                }
    //            }
    //            _stmpl_container = _stg_container.GetInstanceOf("searchrsltcategoryleft" + "\\" + "main");
    //            //_stmpl_container.SetAttribute("Selection", updateNavigation());
    //            _stmpl_container.SetAttribute("TBWDataList", lstrows);                
    //            sHTML += _stmpl_container.ToString();
    //        }
    //        catch (Exception ex)
    //        {
    //            sHTML = ex.Message;
    //        }
    //        finally
    //        {
                
    //        }
               
    //    return sHTML;
    //}


}
