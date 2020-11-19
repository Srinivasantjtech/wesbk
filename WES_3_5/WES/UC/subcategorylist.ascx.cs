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
using System.Data.SqlClient;

using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UC_subcategorylist : System.Web.UI.UserControl
{
    ConnectionDB objConnectionDB = new ConnectionDB();
    HelperServices objHelperServices = new HelperServices();
    HelperDB objHelperDB = new HelperDB();
    ErrorHandler objErrorHandler = new ErrorHandler();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public string ST_Subcategorylist()
    {

        try
        {
        string category_id = "1";        
        //have to get the id        
        TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("SUBCATEGORYLIST", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
        tbwtEngine.GDataSet = GetDataSetFX(category_id);
        tbwtEngine.RenderHTML("Row");
        return (tbwtEngine.RenderedHTML);
        }

        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return null;
        }
    }

    private DataSet GetDataSetFX(string category_id)
    {
        try{
        DataSet catid = new DataSet();
        //SqlDataAdapter da = new SqlDataAdapter("select convert(int, option_value) as OPTION_VALUE from tbwc_store_options where option_name = 'DEFAULT CATALOG'", conStr.ConnectionString.ToString().Substring(conStr.ConnectionString.ToString().IndexOf(';') + 1));
        //da.Fill(catid, "generictable");

        int Catelog_id = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString());


        TradingBell5.CatalogX.CSDBProvider.Connection oCXCon = new TradingBell5.CatalogX.CSDBProvider.Connection();
        TradingBell5.CatalogX.CSDBProvider.CSDSTableAdapters.Web_TB_CATEGORYTableAdapter oCatTA = new TradingBell5.CatalogX.CSDBProvider.CSDSTableAdapters.Web_TB_CATEGORYTableAdapter();
        TradingBell5.CatalogX.CSDBProvider.CSDS.WebCat_CategoryDataTable oCatDT = new TradingBell5.CatalogX.CSDBProvider.CSDS.WebCat_CategoryDataTable();
        //oCXCon.ConnSettings(conStr.ConnectionString.ToString().Substring(conStr.ConnectionString.ToString().IndexOf(';') + 1));
        //oCatTA.Fill(oCatDT, Convert.ToInt32(catid.Tables[0].Rows[0].ItemArray[0].ToString()));
        oCXCon.ConnSettings(objConnectionDB.ConnectionString);
        oCatTA.Fill(oCatDT, Catelog_id);
        if (oCatDT != null)
        {
            DataRow[] oDR = oCatDT.Select("PARENT_CATEGORY='" + category_id + "'", "CATEGORY_NAME");
            DataTable dt = oCatDT.Clone();
            foreach (DataRow dr in oDR)
                dt.ImportRow(dr);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            return ds;
        }
        return null;
    } 
        

        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return null;
        }
    }
}
