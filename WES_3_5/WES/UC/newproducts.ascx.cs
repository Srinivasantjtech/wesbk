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
public partial class UC_newpeoducts : System.Web.UI.UserControl
{
    ErrorHandler objErrorHandler = new ErrorHandler();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public string ST_Newproduct()
    {

        try
        {
        ConnectionDB objConnectionDB = new ConnectionDB();
        HelperServices objHelperServices = new HelperServices();

        TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("NEWPRODUCT", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
        //tbwtEngine.RenderHTML("Column");
        //return (tbwtEngine.RenderedHTML);
        return tbwtEngine.ST_NewProduct_Load();
         }
         catch (Exception e)
         {
             objErrorHandler.ErrorMsg = e;
             objErrorHandler.CreateLog();
             return string.Empty;
         }
    }
}
