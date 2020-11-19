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
public partial class QuoteView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HelperServices objHelperServices = new HelperServices();
        QuoteServices objQuoteServices = new QuoteServices();
        ErrorHandler objErrorHandler = new ErrorHandler();
        ///User oUser = new User();
        DataSet ds = new DataSet();
        //Page.Title = oHelper.GetOptionValues("BROWSER TITLE").ToString();
        try
        {
        if (!IsPostBack)
        {
            if (Request["ViewType"] == "Confirm")
            {
                decimal CCTotal = objQuoteServices.GetQuoteTotalCost(objHelperServices.CI(Request["QteId"]));
                string CurSymbol = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString();
                lblPageHead.Visible = false;
            }          
        }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }

    }
}
