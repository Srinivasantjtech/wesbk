
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
using System.Configuration;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UC_bybrand : System.Web.UI.UserControl
{
    
    EasyAsk_WES ObjEasyAsk = new EasyAsk_WES();
    ErrorHandler objErrorHandler = new ErrorHandler();
    int iCatalogId;
    int iInventoryLevelCheck;
    int iRecordsPerPage=16;
    bool bIsStartOver = true;
    bool bDoPaging;
    int iPageNo = 1;
    string catID = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["pgno"] != null)
            {
                iPageNo = Convert.ToInt32(Request.QueryString["pgno"]);
            }
           // GetStoreConfig();
            GetPageConfig();
            HiddenField1.Value = "0";
            HiddenField2.Value = "0";
            hfcheckload.Value = "0";
            HFcnt.Value = "1";
            hforgurl.Value = HttpContext.Current.Request.Url.PathAndQuery.ToString();
            hfback.Value = "";
            hfbackdata.Value = "";
        }
        }
    //private void GetStoreConfig()
    //{
    //    iCatalogId = Convert.ToInt32(Session["CATALOG_ID"].ToString());
    //    iInventoryLevelCheck = Convert.ToInt32(Session["INVENTORY_LEVEL_CHECK"].ToString());
    //}

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
                iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE"].ToString());
            }
            else
            {
                iRecordsPerPage = Convert.ToInt32(HidItemPage.Value.ToString());
                Session["RECORDS_PER_PAGE"] = HidItemPage.Value.ToString();
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    public string Bread_Crumbs()
    {
        string breadcrumb = "", paraPID = "", paraFID = "", paraCID = "", byp = "";
        if (Request.QueryString["pid"] != null)
        {
            paraPID = Request.QueryString["pid"].ToString();
        }
        if (Request.QueryString["fid"] != null && Request.QueryString["fid"].ToString() != "List all products")
            paraFID = Request.QueryString["fid"].ToString();
        if (Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "List all models")
            paraCID = Request.QueryString["cid"].ToString();
        if (Request.QueryString["byp"] != null && Request.QueryString["byp"].ToString() != "")
            byp = Request.QueryString["byp"].ToString();
        
        if (Request.QueryString["cid"] != null)
            catID = Request.QueryString["cid"].ToString();
        try
        {
            breadcrumb = ObjEasyAsk.GetBreadCrumb(Server.MapPath("Templates"));
        }
        catch(Exception ex)
        {
            return ex.Message.ToString();
        }
        return breadcrumb;

    }
       
}
