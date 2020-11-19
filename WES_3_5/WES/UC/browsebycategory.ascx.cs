using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradingBell.Common;
using System.Data;
using System.Data.SqlClient;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UC_browsebycategory : System.Web.UI.UserControl
{
    ConnectionDB conStr = new ConnectionDB();

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public string ST_Browsebycategory()
    {
        string CID = "";
        if (Request.QueryString["fid"] != null)
        {
            CID = GetCID(Request.QueryString["fid"].ToString());
        }
        if (Request.QueryString["cid"] != null || CID!="")
        {          
            HelperDB oHelper = new HelperDB();
            ConnectionDB conStr = new ConnectionDB();

            TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("BROWSEBYCATEGORY", Server.MapPath(oHelper.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), conStr.ConnectionString);
            if (Request.QueryString["cid"] == null)
                tbwtEngine.paraValue = CID;
            else
                tbwtEngine.paraValue = Request.QueryString["cid"].ToString();
            tbwtEngine.RenderHTML("Row");
            return (tbwtEngine.RenderedHTML);
        }
        return "";
    }

    private string GetCID(string familyid)
    {
        DataSet prodtable = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter("select CATEGORY_ID from tb_family where family_id=" + familyid, conStr.ConnectionString.ToString().Substring(conStr.ConnectionString.ToString().IndexOf(';') + 1));
        da.Fill(prodtable, "Producttable");
        return prodtable.Tables[0].Rows[0].ItemArray[0].ToString();
    }
}
