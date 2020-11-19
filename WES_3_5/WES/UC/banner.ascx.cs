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


using TradingBell5.CatalogX;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UC_banner : System.Web.UI.UserControl
{
    ErrorHandler objErrorHandler = new ErrorHandler();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public string ST_banner()
    {
         try
        {
        HelperServices objHelperServices = new HelperServices();
        ConnectionDB objConnectionDB = new ConnectionDB();
        TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("BANNER", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
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
