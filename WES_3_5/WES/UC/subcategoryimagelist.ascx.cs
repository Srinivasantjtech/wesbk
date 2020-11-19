using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UC_subcategoryimagelist : System.Web.UI.UserControl
{
    ErrorHandler objErrorHandler = new ErrorHandler();
    ConnectionDB objConnectionDB = new ConnectionDB();
    HelperServices objHelperServices = new HelperServices();
    HelperDB objHelperDB = new HelperDB();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string ST_Subcategoryimagelist()
    {
         
        //have to get the id
        try
        {
        string category_id = "1";
        TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("SUBCATEGORYIMAGELIST", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
        tbwtEngine.GDataSet = GetDataSetX(category_id);
        tbwtEngine.RenderHTML("Row");
        return (tbwtEngine.RenderedHTML);
        }

        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return string.Empty;
        }
    }

    private DataSet GetDataSetX(string category_id)
    {
        //jtech
        //DataSet catid = new DataSet();
        //SqlDataAdapter da = new SqlDataAdapter("select convert(int, option_value) as OPTION_VALUE from tbwc_store_options where option_name = 'DEFAULT CATALOG'", conStr.ConnectionString.ToString().Substring(conStr.ConnectionString.ToString().IndexOf(';') + 1));
        //da.Fill(catid, "generictable");
        try
        {
        int Catelog_id = Convert.ToInt32 ( System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString());


        TradingBell5.CatalogX.CSDBProvider.Connection oCXCon = new TradingBell5.CatalogX.CSDBProvider.Connection();
        TradingBell5.CatalogX.CSDBProvider.CSDSTableAdapters.Web_TB_CATEGORYTableAdapter oCatTA = new TradingBell5.CatalogX.CSDBProvider.CSDSTableAdapters.Web_TB_CATEGORYTableAdapter();
        TradingBell5.CatalogX.CSDBProvider.CSDS.WebCat_CategoryDataTable oCatDT = new TradingBell5.CatalogX.CSDBProvider.CSDS.WebCat_CategoryDataTable();        
        //oCXCon.ConnSettings( conStr.ConnectionString.ToString().Substring(conStr.ConnectionString.ToString().IndexOf(';') + 1));
        //oCatTA.Fill(oCatDT, Convert.ToInt32(catid.Tables[0].Rows[0].ItemArray[0].ToString()));
        oCXCon.ConnSettings(objConnectionDB.ConnectionString );
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
