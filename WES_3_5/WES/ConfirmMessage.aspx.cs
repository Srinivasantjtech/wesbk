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
using TradingBell.WebCat;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class RegistrationResult : System.Web.UI.Page
{
    ErrorHandler objErrorHandler = new ErrorHandler();
    HelperServices objHelperServices = new HelperServices();
    NotificationServices objNotificationServices = new NotificationServices();
    ConnectionDB objConnectionDB = new ConnectionDB();
    UserServices objUserServices = new UserServices();


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        string sb;
        sb = "<script language=javascript>\n";
        sb += "window.history.forward(1);\n";
        sb += "\n</script>";
        ClientScript.RegisterClientScriptBlock(Page.GetType(), "clientScript", sb);


        sb = "";
        sb = "<script type=javascript>\n";
        sb += "window.onload = function () { Clear(); }\n";
        sb += "function Clear() { \n";
        sb += " var Backlen=history.length; \n";
        sb += " if (Backlen > 0) history.go(-Backlen); \n";
        sb += "\n}</script>";
        ClientScript.RegisterClientScriptBlock(Page.GetType(), "clientScript", sb);



    }
    protected void Page_Load(object sender, EventArgs e)
    {


        Response.Buffer = true;
        //Response.ExpiresAbsolute = DateTime.Now;
        Response.ExpiresAbsolute = DateTime.Now.AddHours(-1);


        Response.Expires = 0;
        Response.AddHeader("Expires", "-1");
        Response.AddHeader("pragma", "no-cache");
        Response.AddHeader("cache-control", "private");
        Response.CacheControl = "no-cache";
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
        Response.Cache.SetNoStore();
        Response.ClearHeaders();
        Response.AppendHeader("Cache-Control", "no-cache"); //HTTP 1.1
        Response.AppendHeader("Cache-Control", "private"); // HTTP 1.1
        Response.AppendHeader("Cache-Control", "no-store"); // HTTP 1.1
        Response.AppendHeader("Cache-Control", "must-revalidate"); // HTTP 1.1
        Response.AppendHeader("Cache-Control", "max-stale=0"); // HTTP 1.1
        Response.AppendHeader("Cache-Control", "post-check=0"); // HTTP 1.1
        Response.AppendHeader("Cache-Control", "pre-check=0"); // HTTP 1.1
        Response.AppendHeader("Pragma", "no-cache"); // HTTP 1.1
        Response.AppendHeader("Keep-Alive", "timeout=3, max=993"); // HTTP 1.1
        Response.AppendHeader("Expires", "Mon, 26 Jul 1997 05:00:00 GMT"); // HTTP 1.1

        Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
        try
        {
            if (Request["Result"] == "NOPRICEAMT")
            {
                lblCartEmpty.Text = (string)GetLocalResourceObject("msgnoprice");
                lblCartEmpty.Visible = true;
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "UPDATE")
            {
                lblConfirmmsg.Text = (string)GetLocalResourceObject("msgUpdate");
                lblErrormsg.Visible = false;
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "SUCCESS")
            {
                lblConfirmmsg.Text = (string)GetLocalResourceObject("msgSuccess");
                lnkResult.Text = (string)GetLocalResourceObject("lnkResult.Text");
                lnkResult.Visible = false;
                lblErrormsg.Visible = false;
            }
            if (Request["Result"] == "FORGOTPASSWORD")
            {
                lblConfirmmsg.Text = (string)GetLocalResourceObject("msgForgotPassWord");
                //Session["FORGOTPWD_FORGOT"] = (string)GetLocalResourceObject("msgForgotPassWord");
                //Session["FORGOTPWD"] = (string)GetLocalResourceObject("msgForgotPassWord");
                Session["FORGOTPWD"] = "Your password has been reset and sent to your email address. Check your email address and continue to login";
                //Session["FORGOTPWD_FORGOT"] = "Your password has been reset and sent to your email address. Check your email address and continue to login";
                Response.Redirect("login.aspx?Result=FORGOTPWD", false);
                //lblErrormsg.Visible = false;
                //lnkResult.Visible = false;
            }
            if (Request["Result"] == "FORGOTUSERID")
            {
                lblConfirmmsg.Text = (string)GetLocalResourceObject("msgForgotuserid");
               //Session["FORGOTPWD_FORGOT"] = (string)GetLocalResourceObject("msgForgotuserid");
                //Session["FORGOTPWD"] = (string)GetLocalResourceObject("msgForgotuserid");
                Session["FORGOTPWD"] = "Your User Id(s) sent to your email address. Check your email address and continue to login";
                Response.Redirect("login.aspx?Result=FORGOTUSERID", false);
                // Session["FORGOTPWD_FORGOT"] = "Your User Id(s) sent to your email address. Check your email address and continue to login";
               //Response.Redirect("login.aspx?Result=FORGOTPASSWORD", false);
                //lblErrormsg.Visible = false;
                //lnkResult.Visible = false;
            }
            if (Request["Result"] == "RESETPASSWORD")
            {
                lblConfirmmsg.Text = (string)GetLocalResourceObject("msgResetPassword");
                Session["RESETPWD"] = (string)GetLocalResourceObject("msgResetPassword");
                Response.Redirect("login.aspx?Result=RESETPASSWORD", false);
                //lblErrormsg.Visible = false;
                //lnkResult.Visible = false;
            }
            if (Request["Result"] == "PASSWORDCHANGED")
            {
                lblConfirmmsg.Text = (string)GetLocalResourceObject("msgPasswordChanged");
                lblErrormsg.Visible = false;
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "LOGINNAMECHANGED")
            {
                lblConfirmmsg.Text = (string)GetLocalResourceObject("msgloginnameChanged");
                lblErrormsg.Visible = false;
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "ACTIVATED")
            {
                lblConfirmmsg.Text = (string)GetLocalResourceObject("msgActivation");
                lblErrormsg.Visible = false;
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "ACTIVATION_FAILED")
            {
                lblConfirmmsg.Visible = false;
                lblErrormsg.Text = (string)GetLocalResourceObject("msgAvtivationFailed");                
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "REMAILACTIVATION")
            {
                if (HttpContext.Current.Session["USER_ID"]!=null)
                {
                    ReMailActivation(Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()));
                }
                
                lblConfirmmsg.Text = (string)GetLocalResourceObject("MsgReMailActivation");
                lblErrormsg.Visible = false;
                lnkResult.Visible = false;
                
            }
            
            if (Request["Result"] == "CARTEMPTY")
            {
                lblConfirmmsg.Visible = false;
                lblErrormsg.Text = (string)GetLocalResourceObject("msgCartEmpty");
                lnkResult.Visible = false;
            }

            if (Request["Result"] == "MESSAGESENT")
            {
                lblConfirmmsg.Text = (string)GetLocalResourceObject("lbConfirmMsg.Text");
                lblErrormsg.Visible = false;
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "ADDRUPDATEREQUESTSENT")
            {
                lblConfirmmsg.Text = (string)GetLocalResourceObject("lbAddrUpdateConfirmMsg.Text");
                lblErrormsg.Visible = false;
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "ADMINADDRUPDATEREQUESTSENT")
            {
                lblConfirmmsg.Text = (string)GetLocalResourceObject("lbAdminAddrUpdateRequestMsg.Text");
                lblErrormsg.Visible = false;
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "ADDRUPDATEREQUESTNOTSENT")
            {
                lblConfirmmsg.Visible = false;
                lblErrormsg.Text = (string)GetLocalResourceObject("lbAddrUpdateRequestMsgFailed.Text");
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "MESSAGENOTSENT")
            {
                lblConfirmmsg.Visible = false;
                lblErrormsg.Text = (string)GetLocalResourceObject("lbConfirmMsgFailed.Text");
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "QUOTERESULT")
            {
                lblConfirmmsg.Text = (string)GetLocalResourceObject("msgQuoteConfirmation");
                lblErrormsg.Visible = false;
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "QTEEMPTY")
            {
                lblCartEmpty.Text = (string)GetLocalResourceObject("msgCartEmpty");
                lblCartEmpty.Visible = true;
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "QTECANCEL")
            {
                lblCartEmpty.Text = (string)GetLocalResourceObject("msgQuoteCancel1") + " " + Session["QUOTEID"].ToString() + " " + (string)GetLocalResourceObject("msgQuoteCancel");
                lblCartEmpty.Visible = true;
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "NOQUOTELIST")
            {
                lblCartEmpty.Text = (string)GetLocalResourceObject("msgNoQuoteList");
                lblCartEmpty.Visible = true;
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "NOORDERS")
            {
                lblCartEmpty.Text = (string)GetLocalResourceObject("msgNoOrder");
                lblCartEmpty.Visible = true;
                lnkResult.Visible = false;
            }
            else if (Request["Result"] == "WSLISTEMPTY")
            {
                lblCartEmpty.Text = (string)GetLocalResourceObject("msgWsListEmpty");
                lblCartEmpty.Visible = true;
                lnkResult.Visible = false;
            }
            else if (Request["Result"] == "QUICKEMPTY")
            {
                string qmin = "";
                string qmax = "";
                string msg = "";
                if (Request["qmax"] != null)
                {
                    qmax = Request["qmax"].ToString();
                }
                if (Request["qmin"] != null)
                {
                    qmin = Request["qmin"].ToString();
                }
                if (Request["msg"] != null)
                {
                    msg = Request["msg"].ToString();
                }
                if (msg.Length > 0)
                {
                    msg += ". ";
                }
                string errmsg = "Incorrect Codes Found on Order. Please Check! Incorrect Codes : " + msg.Replace("%22", "\"").Replace("%26", "&").Replace("%23", "#") + qmin + qmax;
                int stringlen = 80;
                while (errmsg.Length > stringlen)
                {
                    errmsg = errmsg.Insert(stringlen, "<br/>");
                    stringlen = stringlen + 80;
                }
                lblCartEmpty.Text = errmsg;
                lblCartEmpty.Visible = true;
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "CATALOGREQUEST")
            {
                string HtmlNotification = "<table width=\"400px\"  cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tr><td width=\"400px\">Please send me the requested catalog</td></tr></table>";
                if (SendNotification(HtmlNotification) == 1)
                {
                    Imgstat.ImageUrl = "images/submitted_cat.jpg";
                    Imgstat.Visible = true;
                }
            }
            if (Request["Result"] == "REGISTRATION")
            {
                Session["PageUrl"] = "Login.aspx";
            }
            if (Request["Result"] == "CHECKOUTOPTIONCHANGED")
            {
                lblConfirmmsg.Text = "CheckOut Option Changed Successfully.";
                lblErrormsg.Visible = false;
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "UPDATE_DUPLICATE")
            {
                lblConfirmmsg.Text = "Duplicate Order No Option Changed Successfully.";
                lblErrormsg.Visible = false;
                lnkResult.Visible = false;
            }
            if (Request["Result"] == "NOTADMIN")
            {
                lblConfirmmsg.Text = "Only Admin User Can Modify Account Settings.";
                lblErrormsg.Visible = false;
                lnkResult.Visible = false;
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }

    private void ReMailActivation(int User_id)
    {
        try
        {
            UserServices ObjUserServices = new UserServices();
            UserServices.UserInfo ObjUserInfo = new UserServices.UserInfo();
            Security ObjSecurity = new Security();

            ObjUserInfo = ObjUserServices.GetUserInfo(User_id);

            string url = HttpContext.Current.Request.Url.Authority.ToString();
            string activeLink = string.Format("http://" + url + "/Activation.aspx?id={0}", HttpUtility.UrlEncode(ObjSecurity.StringEnCrypt(User_id.ToString())));
            objNotificationServices.NotifyConnection = objConnectionDB.GetConnection();
            System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();

            //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());                 
            MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
            //MessageObj.To.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
            MessageObj.To.Add(ObjUserInfo.AlternateEmail.ToString().Trim () );
            string message = "";
            MessageObj.Subject = "WESonline-Form-Re-Mail Activation";
            MessageObj.IsBodyHtml = true;
            MessageObj.Body = "<html><body>" + "</br> <font color=\"Red\">Activation Link :</font></br><font color=\"Blue\"> <a href=" + activeLink + ">" + activeLink + "</a></font></body></html>";
            System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
            smtpclient.UseDefaultCredentials = false;
            smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
            smtpclient.Send(MessageObj);
            //Response.Redirect("ConfirmMessage.aspx?Result=MESSAGESENT", false);
        }
        catch (Exception)
        {
            Response.Redirect("ConfirmMessage.aspx?Result=MESSAGENOTSENT");
        }
    }

    public int SendNotification(string mailmessage)
    {
        objNotificationServices.NotifyConnection = objConnectionDB.GetConnection();
        int i;
        string sEmailMessage = "";
        string sUser = "";
        sUser = objUserServices.GetUserEmailAdd(objHelperServices.CI(Session["USER_ID"]));
        try
        {
            //sTemplate = oNot.GetTemplateContent(NotificationVariables.NotificationList.NEWORDER.ToString());
            //sEmailMessage = oNot.ParseTemplateMessage(sTemplate, dsOrder);
            sEmailMessage = mailmessage;
            objNotificationServices.SMTPServer = objHelperServices.GetOptionValues("MAIL SERVER").ToString();
            ArrayList CCList = new ArrayList();
            CCList.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
            objNotificationServices.NotifyCC = CCList;
            objNotificationServices.NotifyTo.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
            objNotificationServices.NotifyFrom = sUser;
            string EmailSubject = "Catalog Request"; //= oNot.GetEmailSubject(NotificationVariables.NotificationList.NEWORDER.ToString());
            //EmailSubject = EmailSubject.Replace("{ORDERID}", OrderID.ToString());
            objNotificationServices.NotifySubject = EmailSubject;
            objNotificationServices.NotifyMessage = sEmailMessage;
            objNotificationServices.UserName = objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString();
            objNotificationServices.Password = objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString();
            objNotificationServices.NotifyIsHTML = true; //oNot.IsHTMLNotification(NotificationVariables.NotificationList.NEWORDER.ToString());
            i = objNotificationServices.SendMessage();
            return i;
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return -1;
        }

    }
}
