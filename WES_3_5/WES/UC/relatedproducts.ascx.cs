using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UC_relatedproducts : System.Web.UI.UserControl
{
    ErrorHandler objErrorHandler = new ErrorHandler();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string ST_Relatedproducts()
    {
        try
        {
        HelperServices objHelperServices = new HelperServices();
        ConnectionDB objConnectionDB = new ConnectionDB();
        TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("RELATEDPRODUCTS", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
        tbwtEngine.RenderHTML("Column");
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
