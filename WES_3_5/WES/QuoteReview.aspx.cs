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
using TradingBell.WebCat;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CommonServices;

public partial class QuoteRevie : System.Web.UI.Page
{
    HelperServices objHelperServices = new HelperServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    QuoteServices objQuoteServices = new QuoteServices();
    PaymentServices objPaymentServices = new PaymentServices();
    NotificationServices objNotificationServices = new NotificationServices();
    ConnectionDB objConnectionDB = new ConnectionDB();
    UserServices objUserServices = new UserServices();
    int QuoteID = 0;
    string phone = "";
    double SubTotal = 0.0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
            Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetAllowResponseInBrowserHistory(false);

            if (Session["USER_NAME"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            QuoteID = objHelperServices.CI(Request["QteId"].ToString());
            QteMsg.Text = GetLocalResourceObject("lblMsg").ToString() + "" + QuoteID;
            phone = objHelperServices.Prepare(objHelperServices.GetOptionValues("COMPANY PHONE TEXT").ToString());
            lblConfirmQuote.Text = GetLocalResourceObject("lblConfirmQuote").ToString() + " " + phone;
            //lblConfirmQuote.Text = lblConfirmQuote.Text + phone;

        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
    }
}
