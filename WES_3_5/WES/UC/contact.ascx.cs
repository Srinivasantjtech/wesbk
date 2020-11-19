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
using System.Web.Mail;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UC_contact : System.Web.UI.UserControl
{
    HelperDB objHelperDB = new HelperDB();
    ErrorHandler objErrorHandler = new ErrorHandler();
    HelperServices objHelperServices = new HelperServices();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            InfoLable.Visible = false;
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    protected void btnsend_Click(object sender, EventArgs e)
    {
        try
        {
            MailMessage MessageObj = new MailMessage();
            MessageObj.From = objHelperServices.GetOptionValues("ADMIN EMAIL").ToString();
            MessageObj.To = txtemail.Text;
            MessageObj.Subject = txtsubject.Text;
            MessageObj.Body = "<html><body><b>" + txtmessage.Text + "</b>" + " <font color=\"red\"></font></body></html>";
            SmtpMail.Send(MessageObj);
            InfoLable.Visible = true;
            Response.Redirect("ConfirmMessage.aspx?Result=MESSAGESENT");
        }
        catch (Exception exc)
        {
            Response.Redirect("ConfirmMessage.aspx?Result=MESSAGENOTSENT");
            InfoLable.Visible = true;
            objErrorHandler.ErrorMsg = exc;
            objErrorHandler.CreateLog();
        }
    }
}
