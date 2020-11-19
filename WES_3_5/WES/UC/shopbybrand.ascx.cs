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

using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UC_shopbybrand : System.Web.UI.UserControl
{
    ErrorHandler objErrorHandler = new ErrorHandler();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public string ST_Shopbybrand()
    {
        try
        {
        HelperServices objHelperServices = new HelperServices();
        ConnectionDB objConnectionDB = new ConnectionDB();
        TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("SHOPBYBRAND", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
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
}
