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
public partial class UC_contactus : System.Web.UI.UserControl
{
    ErrorHandler objErrorHandler = new ErrorHandler();
    HelperServices objHelperServices = new HelperServices();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string ST_contactus()
    {
        try
        {
        ConnectionDB objConnectionDB = new ConnectionDB();

        TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("CONTACTUS", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
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

    protected void btnsend_click(object sender, EventArgs e)
    {
       // try
        //{
       //     Notification oNot = new Notification();
       //     oNot.SMTPServer = oHelper.GetOptionValues("MAIL SERVER").ToString();
       //     oNot.NotifyTo.Add(oHelper.GetOptionValues("ADMIN EMAIL").ToString());
       //     oNot.NotifyFrom = txtemail.Value.ToString() + "<support@tradingbell.com>";
       //     oNot.NotifySubject = "Mail from " + txtorgname.Value.ToString();
       //     string message = "";
       //     message = message + "Full Name               : " + txtfname.Value.ToString() + Environment.NewLine;
       //     message = message + "Organization Name  : " + txtorgname.Value.ToString() + Environment.NewLine;
       //     message = message + "Organization Type   : " + txtorgtype.Value.ToString() + Environment.NewLine;
       //     message = message + "Address                  : " + txtaddress.Value.ToString() + Environment.NewLine;
       //     message = message + "Post Code              : " + txtpostcode.Value.ToString() + Environment.NewLine;
       //     message = message + "Phone Number       : " + txtphone.Value.ToString() + Environment.NewLine;
       //     message = message + "Mobile Number      : " + txtmobile.Value.ToString() + Environment.NewLine;

       //     oNot.NotifyMessage = message;
       //     oNot.UserName = "support@tradingbell.com";
       //     oNot.Password = "catalog@5";
       //     int chkmail = oNot.SendMessage();
       //     if (chkmail > 0)
       //     {
       //         Response.Redirect("ConfirmMessage.aspx?Result=MESSAGESENT", false);
       //     }
       //     else
       //     {
       //         Response.Redirect("ConfirmMessage.aspx?Result=MESSAGENOTSENT");

       //     }

       // }
       // catch (Exception ex)
       //{
       //    Response.Redirect("ConfirmMessage.aspx?Result=MESSAGENOTSENT");

       //    oErr.CreateLog();
       // }
        //try
        //{
            
        //    System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage(oHelper.GetOptionValues("ADMIN EMAIL").ToString(), txtemail.Value.ToString());
        //    string message = "";
        //    MessageObj.Subject = "Mail from " + txtorgname.Value.ToString();
        //    MessageObj.IsBodyHtml = true;
        //    message = message + "Full Name&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;:&nbsp;" + txtfname.Value.ToString() + "<br/>";
        //    message = message + "Organization Name&nbsp; &nbsp;:&nbsp;" + txtorgname.Value.ToString() + "<br/>";
        //    message = message + "Organization Type&nbsp;&nbsp;&nbsp;&nbsp;:&nbsp;" + txtorgtype.Value.ToString() + "<br/>";
        //    message = message + "Address&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:&nbsp;" + txtaddress.Value.ToString() + "<br/>";
        //    message = message + "Post Code&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;:&nbsp;" + txtpostcode.Value.ToString() + "<br/>";
        //    message = message + "Phone Number&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;:&nbsp;" + txtphone.Value.ToString() + "<br/>";
        //    message = message + "Mobile Number&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;:&nbsp;" + txtmobile.Value.ToString() + "<br/>";
        //    MessageObj.Body = "<html><body><b>" + message + "</b>" + " <font color=\"red\"></font></body></html>";
        //    System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(oHelper.GetOptionValues("MAIL SERVER").ToString());
        //    smtpclient.UseDefaultCredentials =false;
        //    smtpclient.Send(MessageObj);
        //    Response.Redirect("ConfirmMessage.aspx?Result=MESSAGESENT",false);
        //}
        //catch (Exception exc)
        //{
        //    Response.Redirect("ConfirmMessage.aspx?Result=MESSAGENOTSENT");
        //}

    }
}
