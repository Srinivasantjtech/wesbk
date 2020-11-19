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

public partial class DealerRegistration : System.Web.UI.Page
{
    CountryServices objCountryServices = new CountryServices();
    HelperServices objHelperServices = new HelperServices();
    UserServices objUserServices = new UserServices();
    UserServices.RegistrationInfo oRegInfo = new UserServices.RegistrationInfo();
    ErrorHandler objErrorHandler = new ErrorHandler();

    UserServices.RegistrationInfo oRI;

    ConnectionDB objConnectionDB = new ConnectionDB();
    NotificationServices objNotificationServices = new NotificationServices();

    string BusinessType = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();

        if (!IsPostBack)
        {
            txtcompname.Focus();
            LoadCountryList();
            string countryCode = drpCountry.SelectedValue.ToString();

            ErrorStatusHiddenField.Value = "0";

            //btnsubmit.Enabled = false;
            cVerifyMsg.Visible = false;
            chkterms.Checked = false;
            // ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript: checkEnableSubmit(1); ", true);
         
        }

        if (!chkterms.Checked)
        {
            lblCheckTerms.Visible = true;
        }

        chkautomotive.Attributes.Add("onclick", "javascript:validateSelection();");
        chkengineer.Attributes.Add("onclick", "javascript:validateSelection();");
        chkgovernment.Attributes.Add("onclick", "javascript:validateSelection();");
        Chkhobbyist.Attributes.Add("onclick", "javascript:validateSelection();");
        chkinstallation.Attributes.Add("onclick", "javascript:validateSelection();");
        chkretailer.Attributes.Add("onclick", "javascript:validateSelection();");
        chkschool.Attributes.Add("onclick", "javascript:validateSelection();");
        chkelectrical.Attributes.Add("onclick", "javascript:validateSelection();");
        chkservice.Attributes.Add("onclick", "javascript:validateSelection();");
        chkother.Attributes.Add("onclick", "javascript:validateSelection();");

        
        txtfname.Attributes.Add("onkeypress", "javascript:return check(event);");
        txtlname.Attributes.Add("onkeypress", "javascript:return check(event);");
      //  txtphone.Attributes.Add("onkeypress", "javascript:return Numbersonly(event);");
      //  txtMobile.Attributes.Add("onkeypress", "javascript:return Numbersonly(event);");
      //  txtfax.Attributes.Add("onkeypress", "javascript:return Numbersonly(event);");
        txtemail.Attributes.Add("onkeypress", "javascript:return Email(event);");
        txtcemail.Attributes.Add("onkeypress", "javascript:return Email(event);");
      //  txtposition.Attributes.Add("onkeypress", "javascript:return Numbersonly(event);");
        txtcompname.Attributes.Add("onkeypress", "javascript:return check(event);");
        txtcompno.Attributes.Add("onkeypress", "javascript:return check(event);");
        txtsadd.Attributes.Add("onkeypress", "javascript:return check(event);");
        txtadd2.Attributes.Add("onkeypress", "javascript:return check(event);");
        txttown.Attributes.Add("onkeypress", "javascript:return check(event);");
        txtstate.Attributes.Add("onkeypress", "javascript:return check(event);");
       // txtzip.Attributes.Add("onkeypress", "javascript:return Numbersonly(event);");
    }

    //protected void drpCountry_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    string BillcountryCode = drpCountry.SelectedValue.ToString();
    //    // BillLoadStates(BillcountryCode);
    //}

    public void LoadCountryList()
    {
        try
        {
            DataSet oDs = new DataSet();

            oDs = objCountryServices.GetCountries();
            drpCountry.Items.Clear();
            drpCountry.DataSource = oDs;
            drpCountry.DataValueField = oDs.Tables[0].Columns["COUNTRY_CODE"].ToString();
            drpCountry.DataTextField = oDs.Tables[0].Columns["COUNTRY_NAME"].ToString();
            drpCountry.DataBind();
            drpCountry.SelectedIndex = drpCountry.Items.IndexOf(new ListItem("Australia", "AU"));

        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
    }
    //protected void CustomerType_Click(object sender, EventArgs e)
    //{
    //    if (CustomerType.Text == "Retailer")
    //        Response.Redirect("RetailerRegistration.aspx");  
    //}
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
        cVerify.ValidateCaptcha(cText.Text);        //Validate Captcha Control Text
        if (cText.Text.Trim() == "")
        {
            //chkterms.Checked = false;
            cText.Text = "";
            cVerifyMsg.Text = "Required";
            cVerifyMsg.Visible = true;
            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript: checkEnableSubmit(); ", true);
        }
        else if (cVerify.UserValidated)
        {
            if (!Page.IsValid)
            {
                //chkterms.Checked = false;
                cVerifyMsg.Visible = false;
                cText.Text = "";
                ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript: checkEnableSubmit(); ", true);
                return;
            }

            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript: checkselectedlist(); ", true);

            if (Convert.ToInt16(ErrorStatusHiddenField.Value) == 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('Please complete the field What Describes you and/or your company the best!');", true);
                cText.Text = "";
                cVerifyMsg.Text = "Required";
                cVerifyMsg.Visible = true;
                return;
            }

            if (chkterms.Checked == false)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('To Submit form you must agree to the Sales Terms and Conditions');", true);
                cText.Text = "";
                cVerifyMsg.Text = "Required";
                cVerifyMsg.Visible = true;
                return;
            }

            cVerifyMsg.Visible = false;
            double i = SaveUserProfile();
            
            //SendNotification();

            //chkterms.Checked = false;
            //btnsubmit.Enabled = false;

            if (i > 0)
            {
                SendNewCustomer();
                Response.Redirect("ConfirmMessage.aspx?Result=REGISTRATION", true);
            }
        }
        else
        {
            //cVerify.CustomValidatorErrorMessage = "Invalid Verification Code";
            cVerifyMsg.Text = "Invalid Verification Code";
            //btnsubmit.Enabled = false;
            cVerifyMsg.Visible = true;
            //chkterms.Checked = false;
            cText.Text = "";
            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript: checkEnableSubmit(); ", true);
            return;
        }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return;
        }
    }

    public double SaveUserProfile()
    {
        try
        {
            oRegInfo.Customer_Type = "Delear";// CustomerType.Text;   
            oRegInfo.CompanyName = txtcompname.Text.ToString();
            oRegInfo.AbnAcn = txtcompno.Text.ToString();
            oRegInfo.Address1 = txtsadd.Text.ToString();
            oRegInfo.Address2 = txtadd2.Text.ToString();
            oRegInfo.SubCity = txttown.Text.ToString();
            oRegInfo.State = txtstate.Text.ToString();
            oRegInfo.PostZipcode = txtzip.Text.ToString();
         //   oRegInfo.Country = drpCountry.SelectedValue.ToString();
            oRegInfo.Country = drpCountry.SelectedItem.ToString();
            oRegInfo.Fname = txtfname.Text.ToString();
            oRegInfo.Lname = txtlname.Text.ToString();
            oRegInfo.Position = txtposition.Text.ToString();
            oRegInfo.Phone = txtphone.Text.ToString();
            oRegInfo.Mobile = txtMobile.Text.ToString();
            oRegInfo.Fax = txtfax.Text.ToString();
            oRegInfo.Email = txtemail.Text.ToString();
            oRegInfo.Company_webSite = txtCompanyWebSite.Text.ToString();     
            if (chkautomotive.Checked == true)
            {
                BusinessType = BusinessType + chkautomotive.Text.ToString() + "; ";
            }
            if (chkengineer.Checked == true)
            {
                BusinessType = BusinessType + chkengineer.Text.ToString() + "; ";
            }
            if (chkgovernment.Checked == true)
            {
                BusinessType = BusinessType + chkgovernment.Text.ToString() + "; ";
            }
            if (Chkhobbyist.Checked == true)
            {
                BusinessType = BusinessType + Chkhobbyist.Text.ToString() + "; ";
            }
            if (chkinstallation.Checked == true)
            {
                BusinessType = BusinessType + chkinstallation.Text.ToString() + "; ";
            }
            if (chkretailer.Checked == true)
            {
                BusinessType = BusinessType + chkretailer.Text.ToString() + "; ";
            }
            if (chkschool.Checked == true)
            {
                BusinessType = BusinessType + chkschool.Text.ToString() + "; ";
            }
            if (chkelectrical.Checked == true)
            {
                BusinessType = BusinessType + chkelectrical.Text.ToString() + "; ";
            }
            if (chkservice.Checked == true)
            {
                BusinessType = BusinessType + chkservice.Text.ToString() + "; ";
            }

            BusinessType = BusinessType + (txtothers.Text.Trim().Length > 0 ? "Others : " + txtothers.Text.ToString() : "");
            oRegInfo.BusinessType = "NA";
            oRegInfo.BusinessDsc = BusinessType.Length > 50 ? BusinessType.Substring(0, 50).Trim() : BusinessType;
            oRegInfo.SiteID = "1";
            oRegInfo.CustStatus = "False";
            oRegInfo.Status = "I";
            oRegInfo.RegType = "N";
            oRegInfo.Password = "";
           // string strHostName = System.Net.Dns.GetHostName();
             string clientIPAddress="";
             if (Session["IP_ADDR"] != null && Session["IP_ADDR"].ToString() != "")
                 clientIPAddress = Session["IP_ADDR"].ToString();
             else
                 clientIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();

            oRegInfo.IpAddr = clientIPAddress;   // Request.ServerVariables["REMOTE_ADDR"].ToString()
            //oRegInfo.IpAddr = Request.UserHostAddress.ToString();
            oRegInfo.LastInvNo = "N/A";
            oRegInfo.WesAccNo = "N/A";

            return objUserServices.CreateRegistration(oRegInfo);
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return -1;
        }
    }

    private void SendNewCustomer()
    {
        try
        {
            objNotificationServices.NotifyConnection = objConnectionDB.GetConnection();
            System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
            MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());                 
            //MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());            
            MessageObj.To.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
            string message = "";
            MessageObj.Subject = "WESonline-Form-NewCustomer";
            MessageObj.IsBodyHtml = true;
            message = message + "<tr><td>Company / Account Name</td><td>&nbsp;</td><td>" + txtcompname.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>ABN / ACN / Company Number</td><td>&nbsp;</td><td>" + txtcompno.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Company WebSite</td><td>&nbsp;</td><td>" + txtCompanyWebSite.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Street Address</td><td>&nbsp;</td><td>" + txtsadd.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Address Line2</td><td>&nbsp;</td><td>" + txtadd2.Text.ToString() + " </td></tr>";
            message = message + "<tr><td>Suburp/Town</td><td>&nbsp;</td><td>" + txttown.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>State/Province</td><td>&nbsp;</td><td>" + txtstate.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Postcode/ZipCode</td><td>&nbsp;</td><td>" + txtzip.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Country</td><td>&nbsp;</td><td>" + drpCountry.SelectedItem.ToString() + "</td></tr>";
            message = message + "<tr><td>First Name</td><td>&nbsp;</td><td>" + txtfname.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Last Name</td><td>&nbsp;</td><td>" + txtlname.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Position</td><td>&nbsp;</td><td>" + txtposition.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Phone</td><td>&nbsp;</td><td>" + txtphone.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Mobile/Cell Phone</td><td>&nbsp;</td><td>" + txtMobile.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Fax</td><td>&nbsp;</td><td>" + txtfax.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Email</td><td>&nbsp;</td><td>" + txtemail.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Business Type</td><td>&nbsp;</td><td>" + BusinessType.ToString() + "</td></tr>";
            MessageObj.Body = "<html><body><table>" + message + "</table>" + " <font color=\"red\"></font></body></html>";
            System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
            smtpclient.UseDefaultCredentials = false;
            smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
            smtpclient.Send(MessageObj);
            Response.Redirect("ConfirmMessage.aspx?Result=MESSAGESENT", false);
        }
        catch (Exception)
        {
            Response.Redirect("ConfirmMessage.aspx?Result=MESSAGENOTSENT");
        }
    }

    public void SendNotification()
    {
        objNotificationServices.NotifyConnection = objConnectionDB.GetConnection();
        if (objNotificationServices.IsNotificationActive(NotificationVariablesServices.NotificationList.CUSTOMERREGISTRATION.ToString()))
        {
            DataSet dsUser = objNotificationServices.BuildNotifyInfo();
            DataRow oRow = dsUser.Tables[0].NewRow();
            string TmplCont = "";
            string EmailCont = "";

            oRI = objUserServices.GetRegistrationInfo(txtemail.Text);
            try
            {
                //Construct the unique symbol with replace variables.

                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.CustomerRegistration.COMPANYNAME.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = oRI.CompanyName.Trim();
                dsUser.Tables[0].Rows.Add(oRow);

                oRow = dsUser.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.CustomerRegistration.CUSTOMEREMAIL.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = oRI.Email.Trim();
                dsUser.Tables[0].Rows.Add(oRow);

                oRow = dsUser.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.CustomerRegistration.CUSTOMERADDRESS.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = oRI.Address1.Trim();
                dsUser.Tables[0].Rows.Add(oRow);

                TmplCont = objNotificationServices.GetTemplateContent(NotificationVariablesServices.NotificationList.CUSTOMERREGISTRATION.ToString());
                EmailCont = objNotificationServices.ParseTemplateMessage(TmplCont, dsUser);

                //Notification for newly register user
                objNotificationServices.SMTPServer = objHelperServices.GetOptionValues("MAIL SERVER").ToString();
                objNotificationServices.UserName = objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString();
                objNotificationServices.Password = objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString();
                objNotificationServices.NotifyBCC.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
                objNotificationServices.NotifyBCC.Add(txtemail.Text);
                objNotificationServices.NotifyTo.Add(oRI.Email.Trim());
                objNotificationServices.NotifyFrom = objHelperServices.GetOptionValues("ADMIN EMAIL").ToString();
                objNotificationServices.NotifySubject = objNotificationServices.GetEmailSubject(NotificationVariablesServices.NotificationList.CUSTOMERREGISTRATION.ToString());
                objNotificationServices.NotifyMessage = EmailCont;
                objNotificationServices.NotifyIsHTML = objNotificationServices.IsHTMLNotification(NotificationVariablesServices.NotificationList.CUSTOMERREGISTRATION.ToString());
                objNotificationServices.SendMessage();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
        }
    }

}
