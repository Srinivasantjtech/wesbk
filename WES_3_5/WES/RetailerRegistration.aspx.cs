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


public partial class RetailerRegistration : System.Web.UI.Page
{
    Security objcrpengine = new Security(); 
AjaxControlToolkit.ModalPopupExtender modalPop = new AjaxControlToolkit.ModalPopupExtender();
    CountryServices objCountryServices = new CountryServices();
    HelperServices objHelperServices = new HelperServices();
    UserServices objUserServices = new UserServices();
    Security objSecurity = new Security();
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
            this.ModalPanel.Visible = false;
            this.modalPop.Hide();
         
        }
      

        if (!chkterms.Checked)
        {
            lblCheckTerms.Visible = true;
        }

        //chkautomotive.Attributes.Add("onclick", "javascript:validateSelection();");
        //chkengineer.Attributes.Add("onclick", "javascript:validateSelection();");
        //chkgovernment.Attributes.Add("onclick", "javascript:validateSelection();");
        //Chkhobbyist.Attributes.Add("onclick", "javascript:validateSelection();");
        //chkinstallation.Attributes.Add("onclick", "javascript:validateSelection();");
        //chkretailer.Attributes.Add("onclick", "javascript:validateSelection();");
        //chkschool.Attributes.Add("onclick", "javascript:validateSelection();");
        //chkservice.Attributes.Add("onclick", "javascript:validateSelection();");
        //chkother.Attributes.Add("onclick", "javascript:validateSelection();");

        
        txtfname.Attributes.Add("onkeypress", "javascript:return check(event);");
        txtlname.Attributes.Add("onkeypress", "javascript:return check(event);");
      //  txtphone.Attributes.Add("onkeypress", "javascript:return Numbersonly(event);");
      //  txtMobile.Attributes.Add("onkeypress", "javascript:return Numbersonly(event);");
      //  txtfax.Attributes.Add("onkeypress", "javascript:return Numbersonly(event);");
        txtemail.Attributes.Add("onkeypress", "javascript:return Email(event);");
        txtcemail.Attributes.Add("onkeypress", "javascript:return Email(event);");
      //  txtposition.Attributes.Add("onkeypress", "javascript:return Numbersonly(event);");
        //txtcompname.Attributes.Add("onkeypress", "javascript:return check(event);");
        //txtcompno.Attributes.Add("onkeypress", "javascript:return check(event);");
        txtsadd.Attributes.Add("onkeypress", "javascript:return check(event);");
        txtadd2.Attributes.Add("onkeypress", "javascript:return check(event);");
        txttown.Attributes.Add("onkeypress", "javascript:return check(event);");
        txtstate.Attributes.Add("onkeypress", "javascript:return check(event);");
        TxtPassword.Attributes.Add("onkeypress", "javascript:return check(event);");
        TxtConfirmPassword.Attributes.Add("onkeypress", "javascript:return check(event);");
       // txtzip.Attributes.Add("onkeypress", "javascript:return Numbersonly(event);");


        ForgotPassword.Attributes.Add("onmouseover", "javascipt:MouseHover(1);");
        ForgotPassword.Attributes.Add("onmouseout", "javascipt:MouseOut(1);");

        Close.Attributes.Add("onmouseover", "javascipt:MouseHover(2);");
        Close.Attributes.Add("onmouseout", "javascipt:MouseOut(2);");


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
            DataSet tmpds = objUserServices.CheckCustomerRegistrationExists(txtemail.Text.Trim(), "Retailer");

            bool  tmp = objUserServices.CheckUserRegisterEmail(txtemail.Text.Trim(),"Retailer");
            if ((tmpds!=null && tmpds.Tables.Count>0 && tmpds.Tables[0].Rows.Count>0))       
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('Mail address already exists');", true);
                
                //return;
                if (tmp == true)
                {
                        this.ModalPanel.Visible = true;
                        modalPop.ID = "popUp";
                        modalPop.PopupControlID = "ModalPanel";
                        modalPop.BackgroundCssClass = "modalBackground";
                        modalPop.DropShadow = false;
                        modalPop.TargetControlID = "btnHiddenTestPopupExtender";
                        this.ModalPanel.Controls.Add(modalPop);
                        this.modalPop.Show();
                        return;
                }
               
            }
            //if (tmp==true )
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('Mail address already exists');", true);
            //    return;
            //}

            cVerifyMsg.Visible = false;
            int i = SaveUserProfile();
           
            //SendNotification();

            //chkterms.Checked = false;
            //btnsubmit.Enabled = false;

            if (i > 0)
            {
                DataSet dsUser = new DataSet();
                dsUser = objUserServices.GetUserDateSet(i);
                
                 
                
                if (dsUser != null && dsUser.Tables.Count > 0 && dsUser.Tables[0].Rows.Count > 0)
                {
                    SendNewCustomer(i,Convert.ToInt32(dsUser.Tables[0].Rows[0]["USER_ID"]));

                    Session["USER_NAME"] = dsUser.Tables[0].Rows[0]["LOGIN_NAME"].ToString();
                    Session["USER_ID"] = dsUser.Tables[0].Rows[0]["USER_ID"];
                    Session["USER_ROLE"]  = dsUser.Tables[0].Rows[0]["USER_ROLE"];
                    Session["COMPANY_ID"] = dsUser.Tables[0].Rows[0]["COMPANY_ID"];
                    Session["CUSTOMER_TYPE"] = dsUser.Tables[0].Rows[0]["CUSTOMER_TYPE"];
                    Response.Redirect("Home.aspx", true);
                }
                else
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

        }
    }
  
    public int SaveUserProfile()
    {
        try
        {
            //Modified by:Indu
            //Modified on :11-Apr-2013
            //Mofified reason:To Build encrption logic
            oRegInfo.Customer_Type = "Retailer";

            if (txtcompname.Text.Trim().Length > 0)
                oRegInfo.CompanyName = txtcompname.Text.ToString();
            else
                oRegInfo.CompanyName = "";
            if (txtcompno.Text.Trim().Length > 0)
                oRegInfo.AbnAcn = txtcompno.Text.ToString();
            else
                oRegInfo.AbnAcn = "";
            
            
            oRegInfo.Address1 = txtsadd.Text.ToString();
            oRegInfo.Address2 = txtadd2.Text.ToString();
            oRegInfo.SubCity = txttown.Text.ToString();
            oRegInfo.State = txtstate.Text.ToString();
            oRegInfo.PostZipcode = txtzip.Text.ToString();
            //oRegInfo.Country = drpCountry.Text.ToString();
            oRegInfo.Country = drpCountry.SelectedItem.ToString();
            oRegInfo.Fname = txtfname.Text.ToString();
            oRegInfo.Lname = txtlname.Text.ToString();
            oRegInfo.Position = "";
            oRegInfo.Phone = txtphone.Text.ToString();
            oRegInfo.Mobile = txtMobile.Text.ToString();
            oRegInfo.Fax = txtfax.Text.ToString();
            oRegInfo.Email = txtemail.Text.ToString();
            //if (chkautomotive.Checked == true)
            //{
            //    BusinessType = BusinessType + chkautomotive.Text.ToString() + "; ";
            //}
            //if (chkengineer.Checked == true)
            //{
            //    BusinessType = BusinessType + chkengineer.Text.ToString() + "; ";
            //}
            //if (chkgovernment.Checked == true)
            //{
            //    BusinessType = BusinessType + chkgovernment.Text.ToString() + "; ";
            //}
            //if (Chkhobbyist.Checked == true)
            //{
            //    BusinessType = BusinessType + Chkhobbyist.Text.ToString() + "; ";
            //}
            //if (chkinstallation.Checked == true)
            //{
            //    BusinessType = BusinessType + chkinstallation.Text.ToString() + "; ";
            //}
            //if (chkretailer.Checked == true)
            //{
            //    BusinessType = BusinessType + chkretailer.Text.ToString() + "; ";
            //}
            //if (chkschool.Checked == true)
            //{
            //    BusinessType = BusinessType + chkschool.Text.ToString() + "; ";
            //}
            //if (chkservice.Checked == true)
            //{
            //    BusinessType = BusinessType + chkservice.Text.ToString() + "; ";
            //}

            //BusinessType = BusinessType + (txtothers.Text.Trim().Length > 0 ? "Others : " + txtothers.Text.ToString() : "");
            oRegInfo.BusinessType = "NA";
            oRegInfo.BusinessDsc = "";//BusinessType.Length > 50 ? BusinessType.Substring(0, 50).Trim() : BusinessType;
            oRegInfo.SiteID = "1";
            oRegInfo.CustStatus = "False";
            oRegInfo.Status = "I";
            oRegInfo.RegType = "N";
            oRegInfo.Password = TxtPassword.Text;
            string Newpassword = objcrpengine.StringEnCrypt(TxtPassword.Text);
            oRegInfo.Password = Newpassword;
            string strHostName = System.Net.Dns.GetHostName();
             string clientIPAddress="";
             if (System.Net.Dns.GetHostAddresses(strHostName) != null)
             {
                 if (System.Net.Dns.GetHostAddresses(strHostName).Length <= 1)
                     clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
                 else
                     clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(1).ToString();
             }
            oRegInfo.IpAddr = clientIPAddress;   // Request.ServerVariables["REMOTE_ADDR"].ToString()
            //oRegInfo.IpAddr = Request.UserHostAddress.ToString();
            oRegInfo.LastInvNo = "N/A";
            oRegInfo.WesAccNo = "N/A";
           
           return  objUserServices.CreateRegistration(oRegInfo);


        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return -1;
        }
    }
    private string Setdrpdownlistvalue(DropDownList d, string val)
    {
        ListItem li;
        string returnselected = "";
        for (int i = 0; i < d.Items.Count; i++)
        {
            li = d.Items[i];
            if (li.Text.ToUpper() == val.ToUpper())
            {
                d.SelectedIndex = i;
                returnselected = li.Text.ToUpper();
                break;
            }
        }
        return returnselected;
    }
    private void SendNewCustomer( int reg_id,int user_id)
    {
        try
        {
            string url = HttpContext.Current.Request.Url.Authority.ToString();
            string activeLink = string.Format("http://" + url + "/Activation.aspx?id={0}", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(user_id.ToString())));
            objNotificationServices.NotifyConnection = objConnectionDB.GetConnection();
            System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
            //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());                 
            MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());            
            //MessageObj.To.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
            MessageObj.To.Add(txtemail.Text.ToString());
            string message = "";
            string Topmessage = "";
            string bottommessage = "";
            MessageObj.Subject = "Account Sign Up Activation Details";
            MessageObj.IsBodyHtml = true;
            Topmessage = Topmessage + "Hi " + txtfname.Text.ToString() + " " + txtlname.Text.ToString() ;
            Topmessage = Topmessage + "<br/><br/>";
            Topmessage = Topmessage + "Thanks for registering with WES online store";
            Topmessage = Topmessage + "<br/><br/>";
            Topmessage = Topmessage + "To get started shopping please confirm your registration by clicking on the activation link below";
           // Topmessage = Topmessage + "<br/><br/> <font color=\"Red\">Activation Link :</font></br><font color=\"Blue\">" + activeLink + "</font>";
            Topmessage = Topmessage + "<br/><br/> <font color=\"Red\">Activation Link :</font></br><font color=\"Blue\"> <a href=" + activeLink + ">" + activeLink + "</a></font>";
            Topmessage = Topmessage + "<br/><br/>";
            Topmessage = Topmessage + "Your Details: <br/><br/> ";


            

            message = message + "<tr><td>Company Name</td><td>&nbsp;</td><td>" + txtcompname.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>ABN No</td><td>&nbsp;</td><td>" + txtcompno.Text.ToString() + "</td></tr>";

            message = message + "<tr><td>First Name</td><td>&nbsp;</td><td>" + txtfname.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Last Name</td><td>&nbsp;</td><td>" + txtlname.Text.ToString() + "</td></tr>";
          //  message = message + "<tr><td>Position</td><td>&nbsp;</td><td>" + txtposition.Text.ToString() + "</td></tr>";

            message = message + "<tr><td>Phone</td><td>&nbsp;</td><td>" + txtphone.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Mobile/Cell Phone</td><td>&nbsp;</td><td>" + txtMobile.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Fax</td><td>&nbsp;</td><td>" + txtfax.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Email</td><td>&nbsp;</td><td>" + txtemail.Text.ToString() + "</td></tr>";
            

            message = message + "<tr><td>Street Address</td><td>&nbsp;</td><td>" + txtsadd.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Address Line2</td><td>&nbsp;</td><td>" + txtadd2.Text.ToString() + " </td></tr>";
            message = message + "<tr><td>Suburp/Town</td><td>&nbsp;</td><td>" + txttown.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>State/Province</td><td>&nbsp;</td><td>" + txtstate.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Postcode/ZipCode</td><td>&nbsp;</td><td>" + txtzip.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Country</td><td>&nbsp;</td><td>" + drpCountry.SelectedItem.ToString() + "</td></tr>";            
            message = message + "<tr><td>Password</td><td>&nbsp;</td><td>" + TxtPassword.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>LOGIN NAME</td><td>&nbsp;</td><td>" + "WES" + reg_id.ToString().PadLeft(5, '0') +"</td></tr>";

            bottommessage = bottommessage + "<br/><br/>";
            bottommessage = bottommessage + "Best Regards<br/>";
            bottommessage = bottommessage + "WES";

            MessageObj.Body = "<html><body><br/>" + Topmessage+ " <table>" + message + "</table>" + bottommessage+ "</body></html>";
            System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
            smtpclient.UseDefaultCredentials = false;
            smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
            smtpclient.Send(MessageObj);
            Response.Redirect("ConfirmMessage.aspx?Result=MESSAGESENT", false);
        }
        catch (Exception)
        {
            objUserServices.DeleteRegistration(reg_id);
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


    protected void btnForgotPassword_Click(object sender, EventArgs e)
    {       
          this.modalPop.Hide();
          Response.Redirect("ForgotPassWord.aspx");       
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        this.ModalPanel.Visible = false;
        this.modalPop.Hide();
        txtemail.Text = "";
        txtcemail.Text = "";
        cText.Text = "";
        txtemail.Focus();
        return;
    }

}
