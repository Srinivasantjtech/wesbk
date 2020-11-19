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
using TradingBell.WebCat.CommonServices;
public partial class ShippingInfo : System.Web.UI.Page
{
    HelperServices objHelperServices = new HelperServices();
    //Product oProd = new Product();
    //ProductFamily oPF = new ProductFamily();
    //Category oCat = new Category();
    //ErrorHandler oErr = new ErrorHandler();
    //int ProdID = 0;
    //string ImgPath = System.Configuration.ConfigurationManager.AppSettings["ImagePath"];

    protected void Page_Load(object sender, EventArgs e)
    {
        //DataSet dsCat = new DataSet();
        //ProdID = oHelper.CI(Request["Pid"]);
        //Session["PageUrl"] = "ProductDetails.aspx?Pid=" + ProdID;

        Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
        HelperServices objHelperServices = new HelperServices();
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
        txtShipCharge.Text = objHelperServices.GetOptionValues("SHIPPING CHARGE").ToString();
    }
    #region "Control Events"

    #endregion
}
