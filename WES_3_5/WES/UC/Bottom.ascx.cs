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
public partial class UC_Bottom : System.Web.UI.UserControl
{
    ErrorHandler objErrorHandler = new ErrorHandler();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string ST_bottom()
    {
          try
        {
        //HelperServices objHelperServices = new HelperServices();
        //ConnectionDB objConnectionDB = new ConnectionDB();
        //TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("BOTTOM", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
        ////tbwtEngine.RenderHTML("Row");
        ////return (tbwtEngine.RenderedHTML);
        //return tbwtEngine.ST_Bottom_Load();
      string html = (string)Cache["Cache_bottom"];
      if (HttpContext.Current.Session["CUSTOMER_TYPE"] != null && HttpContext.Current.Session["CUSTOMER_TYPE"].ToString().Equals("Retailer"))
      {
          html = html.Replace("EditUserProfile.aspx", "RetailerEditUserProfile.aspx");
      
      }
      return html;
        }
          catch (Exception ex)
          {
              objErrorHandler.ErrorMsg = ex;
              objErrorHandler.CreateLog();
              return string.Empty;
          }
    }
}
