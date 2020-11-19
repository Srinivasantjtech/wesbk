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
public partial class UC_mfgbylogo : System.Web.UI.UserControl
{
    ConnectionDB objConnectionDB = new ConnectionDB();
    HelperServices objHelperServices = new HelperServices();
    protected void Page_Load(object sender, EventArgs e)
    {        
    }
    public string ST_MFGbyLogo()
    {
        TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("MFGBYLOGO", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
        tbwtEngine.RenderHTML("row");
        return (tbwtEngine.RenderedHTML);
    }
}
