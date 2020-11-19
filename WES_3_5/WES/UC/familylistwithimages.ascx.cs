using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradingBell.Common;
using System.Data;
using TradingBell.WebServices;
using System.Data.SqlClient;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UC_familylistwithimages : System.Web.UI.UserControl
{
    ConnectionDB conStr = new ConnectionDB();

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string ST_Familylistwithimages()
    {
        HelperDB oHelper = new HelperDB();
        string category_id = "1";      
        TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("FAMILYLISTWITHIMAGES", Server.MapPath(oHelper.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), conStr.ConnectionString);
        tbwtEngine.GDataSet = GetDataSetFX(category_id);
        tbwtEngine.RenderHTML("Row");        
        return (tbwtEngine.RenderedHTML);
    }
    private DataSet GetDataSetFX(string category_id)
    {
        DataSet catid = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter("select convert(int, option_value) as OPTION_VALUE from tbwc_store_options where option_name = 'DEFAULT CATALOG'", conStr.ConnectionString.ToString().Substring(conStr.ConnectionString.ToString().IndexOf(';')+1));
        da.Fill(catid, "generictable");
        FamilyListRender oFamList = new FamilyListRender();
        oFamList.CatalogID = Convert.ToInt32(catid.Tables[0].Rows[0].ItemArray[0].ToString());
        oFamList.CategoryID = category_id;
        return oFamList.GetFamilyDetailsDS();
    } 
}
