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
using System.Collections.Generic; 
using System.Xml.Linq;
using System.Web.Mail;
using System.Net.Mime;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UC_Returns : System.Web.UI.UserControl
{
    HelperServices objHelperServices = new HelperServices();
   
    ConnectionDB objConnectionDB = new ConnectionDB();
    ErrorHandler objErrorHandler = new ErrorHandler();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string ST_Returns()
    {
        TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("RETURNS", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
        tbwtEngine.RenderHTML("Row");
        return (tbwtEngine.RenderedHTML);
    }

    protected void btnsend_click(object sender, EventArgs e)
    {
        try
        {
            NotificationServices objNotificationServices = new NotificationServices();
            objNotificationServices.SMTPServer = objHelperServices.GetOptionValues("MAIL SERVER").ToString();
            objNotificationServices.NotifyTo.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
            objNotificationServices.NotifyFrom = txtemail.Value.ToString() + "<support@tradingbell.com>";
            objNotificationServices.NotifySubject = "Mail from " + txtorgname.Value.ToString();
            string message = "";
            message = message + "Full Name               : " + txtfname.Value.ToString() + Environment.NewLine;
            message = message + "Organization Name  : " + txtorgname.Value.ToString() + Environment.NewLine;
            message = message + "Organization Type   : " + txtorgtype.Value.ToString() + Environment.NewLine;
            message = message + "Address                  : " + txtaddress.Value.ToString() + Environment.NewLine;
            message = message + "Post Code              : " + txtpostcode.Value.ToString() + Environment.NewLine;
            message = message + "Phone Number       : " + txtphone.Value.ToString() + Environment.NewLine;
            message = message + "Mobile Number      : " + txtmobile.Value.ToString() + Environment.NewLine;

            objNotificationServices.NotifyMessage = message;
            objNotificationServices.UserName = "support@tradingbell.com";
            objNotificationServices.Password = "catalog@5";
            int chkmail = objNotificationServices.SendMessage();
            if (chkmail > 0)
            {
                Response.Redirect("ConfirmMessage.aspx?Result=MESSAGESENT", false);
            }
            else
            {
                Response.Redirect("ConfirmMessage.aspx?Result=MESSAGENOTSENT");

            }

        }
        catch (Exception ex)
       {
           Response.Redirect("ConfirmMessage.aspx?Result=MESSAGENOTSENT");            
           objErrorHandler.CreateLog();

        }

    }
}
