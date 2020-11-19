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
public partial class UI_QuoteInvoice : System.Web.UI.UserControl
{
    #region "Declarations"
    HelperServices objHelperServices = new HelperServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    PaymentServices objPaymentServices = new PaymentServices();
    PaymentServices.PayInfo oPayInfo = new PaymentServices.PayInfo();
    OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
    UserServices objUserServices = new UserServices();
    UserServices.UserInfo oUserInfo = new UserServices.UserInfo();
    QuoteServices objQuoteServices = new QuoteServices();
    QuoteServices.QuoteInfo oQuoteInfo = new QuoteServices.QuoteInfo();
    int OrderID = 0;
    int QuoteId = 0;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (QuoteId == 0)
        {
            QuoteId = objHelperServices.CI(Request["QteId"].ToString());
        }
        //oPayInfo = oPay.GetPayment(QuoteId);
        oQuoteInfo = objQuoteServices.GetQuote(QuoteId);
        int UserID = objHelperServices.CI(Session["USER_ID"].ToString());
        oUserInfo = objUserServices.GetUserInfo(UserID);
        lblQuoteID.Text = "Quote Ref # : " + QuoteId;

    }

}
