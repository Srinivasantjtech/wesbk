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
using TradingBell.WebCat.CommonServices;


public partial class UserProfile : System.Web.UI.Page
{
    #region "Declarations..."

    ErrorHandler objErrorHandler = new ErrorHandler();
    UserServices objUserServices = new UserServices();
    HelperServices objHelsperServices = new HelperServices();
    CompanyGroupServices objCompanyGroupServices = new CompanyGroupServices();
    UserServices.UserInfo oUI;
    CompanyGroupServices.CompanyInfo oCI;
    ConnectionDB objConnectionDB = new ConnectionDB();
    NotificationServices objNotificationServices = new NotificationServices();
    CountryServices objCountryServices = new CountryServices();

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Page.Title = objHelsperServices.GetOptionValues("BROWSER TITLE").ToString();
            if (!IsPostBack)
            {
                LoadCountryList();
                txtFname.Focus();
                LoadSecurityQuestions();

                string countryCode = drpCountry.SelectedValue.ToString();
                //LoadStates(countryCode);
                string ShipcountryCode = drpShipCountry.SelectedValue.ToString();
                // ShipLoadStates(ShipcountryCode);
                string BillcountryCode = drpBillCountry.SelectedValue.ToString();
                // BillLoadStates(BillcountryCode);


                if (Session["COMPANYPROFILE"] != null || Session["COMPANYPROFILE"].ToString() != null)
                {
                    txtFname.Focus();
                    LoadSecurityQuestions();
                    LoadCountryList();
                    countryCode = drpCountry.SelectedValue.ToString();
                    // LoadStates(countryCode);
                    ShipcountryCode = drpShipCountry.SelectedValue.ToString();
                    // ShipLoadStates(ShipcountryCode);
                    BillcountryCode = drpBillCountry.SelectedValue.ToString();
                    // BillLoadStates(BillcountryCode);

                    oCI = (CompanyGroupServices.CompanyInfo)Session["COMPANYPROFILE"];

                    txtbilladd1.Text = oCI.Address1.ToString();
                    txtbilladd2.Text = oCI.Address2.ToString();
                    txtbilladd3.Text = oCI.Address3.ToString();
                    txtbillcity.Text = oCI.City.ToString();
                    drpBillState.Text = oCI.State.ToString();
                    drpBillCountry.SelectedValue = oCI.Country.ToString();
                    txtbillzip.Text = oCI.Zip.ToString();
                    txtbillphone.Text = oCI.Phone1.ToString();

                    txtshipadd1.Text = oCI.Address1.ToString();
                    txtshipadd2.Text = oCI.Address2.ToString();
                    txtshipadd3.Text = oCI.Address3.ToString();
                    txtshipcity.Text = oCI.City.ToString();
                    drpShipState.Text = oCI.State.ToString();
                    drpShipCountry.SelectedValue = oCI.Country.ToString();
                    txtshipzip.Text = oCI.Zip.ToString();
                    txtshipphone.Text = oCI.Phone1.ToString();
                }
                else
                {
                    Response.Redirect("UserBasicInfo.aspx", false);
                }
                HtmlMeta meta = new HtmlMeta();
                meta.Name = "keywords";
                meta.Content = objHelsperServices.GetOptionValues("Meta keyword").ToString();
                this.Header.Controls.Add(meta);

                // Render: <meta name="Description" content="noindex" />
                meta = new HtmlMeta();
                meta.Name = "Description";
                meta.Content = objHelsperServices.GetOptionValues("Meta Description").ToString();
                this.Header.Controls.Add(meta);

                // Render: <meta name="Abstraction" content="Some words listed here" />
                meta.Name = "Abstraction";
                meta.Content = objHelsperServices.GetOptionValues("Meta Abstraction").ToString();
                this.Header.Controls.Add(meta);

                // Render: <meta name="Distribution" content="noindex" />
                meta = new HtmlMeta();
                meta.Name = "Distribution";
                meta.Content = objHelsperServices.GetOptionValues("Meta Distribution").ToString();
                this.Header.Controls.Add(meta);

            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    #region "Control Events"
    protected void checkshipAddress(object sender, EventArgs e)
    {
        // Response.Write("check");
        if (commshipAddress.Checked == true)
        {
            txtshipadd1.Text = txtbilladd1.Text;
            txtshipadd2.Text = txtbilladd2.Text;
            txtshipadd3.Text = txtbilladd3.Text;
            txtshipcity.Text = txtbillcity.Text;
            drpShipState.Text = drpBillState.Text;
            txtshipzip.Text = txtbillzip.Text;
            drpShipCountry.SelectedValue = drpBillCountry.Text;
            txtshipphone.Text = txtbillphone.Text;
        }
        else
        {
            txtshipadd1.Text = "";
            txtshipadd2.Text = "";
            txtshipadd3.Text = "";
            txtshipcity.Text = "";
            // txtshipstate.Text = "";
            txtshipzip.Text = "";
            // txtshipcountry.Text = "";
            txtshipphone.Text = "";


        }
    }

    protected void checkbillAddress(object sender, EventArgs e)
    {
        //Is communication addr is same as Billing address
        if (commbillAddress.Checked == true)
        {
            txtbilladd1.Text = txtAdd1.Text;
            txtbilladd2.Text = txtAdd2.Text;
            txtbilladd3.Text = txtAdd3.Text;
            txtbillcity.Text = txtCity.Text;
            drpBillState.Text = drpState.Text;
            txtbillzip.Text = txtZip.Text;
            drpBillCountry.SelectedValue = drpCountry.Text;
            txtbillphone.Text = txtPhone.Text;
        }
        else
        {
            //For Billing details
            txtbilladd1.Text = "";
            txtbilladd2.Text = "";
            txtbilladd3.Text = "";
            txtbillcity.Text = "";
            txtbillzip.Text = "";
            txtbillphone.Text = "";
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Session["USERPROFILE"] != null)
        {
            oUI = (UserServices.UserInfo)Session["USERPROFILE"];
        }
        if (Session["COMPANYPROFILE"] != null && Session["COMPANYPROFILE"].ToString() != null)
        {
            try
            {
                if (oUI.CompanyID == null)
                {
                    oCI = (CompanyGroupServices.CompanyInfo)Session["COMPANYPROFILE"];
                    if (SaveCompanyProfile() > 0)
                    {
                        oUI.CompanyID = objCompanyGroupServices.GetCompanyID(oCI.CompanyName).ToString();
                        Session["COMPANYPROFILE"] = null;
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            if (SaveUserProfile() > 0)
            {
                if (objUserServices.InsertCompanyUser(objUserServices.GetUserID(oUI.Email), oUI.CompanyID, UserServices._StoreAdminID) > 0)
                {
                    //Send the notification to store admin to initmate the user is registered.
                    SendNotification();
                    Response.Redirect("Login.aspx?Result=SUCCESS");
                }
            }
        }
        else
        {
            Response.Redirect("UserBasicInfo.aspx", false);
        }
    }
    #endregion
    #region "Functions..."

    public int SaveCompanyProfile()
    {
        try
        {
            objCompanyGroupServices.CompanyUser =UserServices._StoreAdminID;
            return objCompanyGroupServices.CreateProfile(oCI);
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return -1;
        }
    }
    public int SaveUserProfile()
    {
        try
        {
            oUI.Prefix = cmbPrefix.Text;
            oUI.FirstName = txtFname.Text;
            oUI.MiddleName = txtMname.Text;
            oUI.LastName = txtLname.Text;
            oUI.Suffix = txtSuffix.Text;
            oUI.Address1 = txtAdd1.Text;
            oUI.Address2 = txtAdd2.Text;
            oUI.Address3 = txtAdd3.Text;
            oUI.City = txtCity.Text;
            oUI.State = drpState.Text;
            oUI.Zip = txtZip.Text;
            oUI.Country = drpCountry.Text;
            oUI.Phone = txtPhone.Text;
            oUI.MobilePhone = txtMobile.Text;
            oUI.Fax = txtFax.Text;
            oUI.AlternateEmail = txtAltEmail.Text;

            oUI.Pwd_Question1 = objHelsperServices.Prepare(cmbQuestion.Text);
            oUI.Pwd_Answer1 = txtAnswer1.Text;
            oUI.isCC_Payment = 0;
            oUI.isCOD_Payment = 0;
            oUI.isPO_Payment = 0;


            //For shipping Details
            oUI.ShipAddress1 = txtshipadd1.Text;
            oUI.ShipAddress2 = txtshipadd2.Text;
            oUI.ShipAddress3 = txtshipadd3.Text;
            oUI.ShipCity = txtshipcity.Text;
            oUI.ShipState = drpShipState.Text;
            oUI.ShipZip = txtshipzip.Text;
            oUI.ShipCountry = drpShipCountry.Text;
            oUI.ShipPhone = txtshipphone.Text;

            //For Billing details
            oUI.BillAddress1 = txtbilladd1.Text;
            oUI.BillAddress2 = txtbilladd2.Text;
            oUI.BillAddress3 = txtbilladd3.Text;
            oUI.BillCity = txtbillcity.Text;
            oUI.BillState = drpBillState.Text;
            oUI.BillZip = txtbillzip.Text;
            oUI.BillCountry = drpBillCountry.Text;
            oUI.BillPhone = txtbillphone.Text;

            if (objHelsperServices.GetOptionValues("DEFAULT USER STATUS").ToString() == "ACTIVE")
            {

                oUI.Status = (int)UserServices.UserStatus.ACTIVE;
                GetCompanyPaymentStatus();
            }
            else if (objHelsperServices.GetOptionValues("DEFAULT USER STATUS").ToString() == "INACTIVE")
            {
                oUI.Status = (int)UserServices.UserStatus.INACTIVE;
            }
            if (!objCompanyGroupServices.IsCompanyUserExist(oUI.CompanyID))
            {
                oUI.oUserRole = UserServices.UserRole.COMPANYADMIN;
            }
            else
            {
                oUI.oUserRole = UserServices.UserRole.SHOPPER;
            }
            oUI.isOnline = 0;
            oUI.UserID = UserServices._StoreAdminID;
            return objUserServices.CreateUser(oUI);
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return -1;
        }

    }

    public void LoadSecurityQuestions()
    {
        cmbQuestion.Items.Clear();
        cmbQuestion.Items.Add((string)GetLocalResourceObject("Question1"));
        cmbQuestion.Items.Add((string)GetLocalResourceObject("Question2"));
        cmbQuestion.Items.Add((string)GetLocalResourceObject("Question3"));
        cmbQuestion.Items.Add((string)GetLocalResourceObject("Question4"));
        cmbQuestion.Items.Add((string)GetLocalResourceObject("Question5"));
        cmbQuestion.Items.Add((string)GetLocalResourceObject("Question6"));
        cmbQuestion.Items.Add((string)GetLocalResourceObject("Question7"));
        cmbQuestion.Items.Add((string)GetLocalResourceObject("Question8"));
        cmbQuestion.Items.Add((string)GetLocalResourceObject("Question9"));
    }

    public void SendNotification()
    {
        objNotificationServices.NotifyConnection = objConnectionDB.GetConnection();
        if (objNotificationServices.IsNotificationActive(NotificationVariablesServices.NotificationList.USERREGISTRATION.ToString()))
        {
            DataSet dsUser = objNotificationServices.BuildNotifyInfo();
            DataRow oRow = dsUser.Tables[0].NewRow();
            string TmplCont = "";
            string EmailCont = "";

            try
            {
                //Construct the unique symbol with replace variables.
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.UserRegistration.USERID.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = oUI.Email;
                dsUser.Tables[0].Rows.Add(oRow);

                oRow = dsUser.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.UserRegistration.PASSWORD.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = oUI.Password;
                dsUser.Tables[0].Rows.Add(oRow);

                oRow = dsUser.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.UserRegistration.FIRSTNAME.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = oUI.FirstName;
                dsUser.Tables[0].Rows.Add(oRow);

                oRow = dsUser.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.UserRegistration.LASTNAME.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = oUI.LastName;
                dsUser.Tables[0].Rows.Add(oRow);

                oRow = dsUser.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.UserRegistration.COMPANYURL.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = objHelsperServices.GetOptionValues("COMPANY URL").ToString();
                dsUser.Tables[0].Rows.Add(oRow);

                oRow = dsUser.Tables[0].NewRow();
                oRow["ColumnKey"] = objNotificationServices.UniqueStartSymbol + NotificationVariablesServices.UserRegistration.COMPANYADDRESS.ToString() + objNotificationServices.UniqueEndSymbol;
                oRow["ColumnValue"] = objHelsperServices.GetOptionValues("COMPANY ADDRESS").ToString();
                dsUser.Tables[0].Rows.Add(oRow);

                TmplCont = objNotificationServices.GetTemplateContent(NotificationVariablesServices.NotificationList.USERREGISTRATION.ToString());
                EmailCont = objNotificationServices.ParseTemplateMessage(TmplCont, dsUser);

                //Notification for newly register user
                objNotificationServices.SMTPServer = objHelsperServices.GetOptionValues("MAIL SERVER").ToString();
                objNotificationServices.UserName = objHelsperServices.GetOptionValues("MAIL SERVER USERNAME").ToString();
                objNotificationServices.Password = objHelsperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString();
                objNotificationServices.NotifyBCC.Add(objHelsperServices.GetOptionValues("ADMIN EMAIL").ToString());
                objNotificationServices.NotifyBCC.Add(objCompanyGroupServices.GetCompanyAdmin(oUI.CompanyID));
                objNotificationServices.NotifyTo.Add(oUI.Email);
                objNotificationServices.NotifyFrom = objHelsperServices.GetOptionValues("ADMIN EMAIL").ToString();
                objNotificationServices.NotifySubject = objNotificationServices.GetEmailSubject(NotificationVariablesServices.NotificationList.USERREGISTRATION.ToString());
                objNotificationServices.NotifyMessage = EmailCont;
                objNotificationServices.NotifyIsHTML = objNotificationServices.IsHTMLNotification(NotificationVariablesServices.NotificationList.USERREGISTRATION.ToString());
                objNotificationServices.SendMessage();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
        }
    }
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

            oDs = new DataSet();
            oDs = objCountryServices.GetCountries();
            drpShipCountry.Items.Clear();
            drpShipCountry.DataSource = oDs;
            drpShipCountry.DataValueField = oDs.Tables[0].Columns["COUNTRY_CODE"].ToString();
            drpShipCountry.DataTextField = oDs.Tables[0].Columns["COUNTRY_NAME"].ToString();
            drpShipCountry.DataBind();
            drpShipCountry.SelectedIndex = drpCountry.Items.IndexOf(new ListItem("Australia", "AU"));

            oDs = new DataSet();
            oDs = objCountryServices.GetCountries();
            drpBillCountry.Items.Clear();
            drpBillCountry.DataSource = oDs;
            drpBillCountry.DataValueField = oDs.Tables[0].Columns["COUNTRY_CODE"].ToString();
            drpBillCountry.DataTextField = oDs.Tables[0].Columns["COUNTRY_NAME"].ToString();
            drpBillCountry.DataBind();
            drpBillCountry.SelectedIndex = drpCountry.Items.IndexOf(new ListItem("Australia", "AU"));
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    public void LoadStates(String conCode)
    {
        try
        {
            DataSet oDs = new DataSet();
            oDs = objCountryServices.GetStates(conCode);
            //   drpState.DataSource = oDs;
            // drpState.DataTextField = oDs.Tables[0].Columns["STATE_NAME"].ToString();
            // drpState.DataValueField = oDs.Tables[0].Columns["STATE_CODE"].ToString();
            // drpState.DataBind();
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    public void ShipLoadStates(String conCode)
    {
        DataSet oDs = new DataSet();
        oDs = objCountryServices.GetStates(conCode);
        //drpShipState.DataSource = oDs;
        // drpShipState.DataTextField = oDs.Tables[0].Columns["STATE_NAME"].ToString();
        // drpShipState.DataValueField = oDs.Tables[0].Columns["STATE_CODE"].ToString();
        // drpShipState.DataBind();
    }
    public void BillLoadStates(String conCode)
    {
        try
        {
            DataSet oDs = new DataSet();
            oDs = objCountryServices.GetStates(conCode);
            // drpBillState.DataSource = oDs;
            // drpBillState.DataTextField = oDs.Tables[0].Columns["STATE_NAME"].ToString();
            // drpBillState.DataValueField = oDs.Tables[0].Columns["STATE_CODE"].ToString();
            // drpBillState.DataBind();
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    public void GetCompanyPaymentStatus()
    {
        try
        {
            string ComID = "";
            if (Request["ComID"] != null)
            {
                ComID = (Request["ComID"].ToString());
                oCI = objCompanyGroupServices.GetCompanyInfo(objCompanyGroupServices.GetCompanyName(ComID));

                if (oCI.PayMethod == "7")
                {
                    oUI.isPO_Payment = 1;
                    oUI.isCC_Payment = 1;
                    oUI.isCOD_Payment = 1;
                }
                else if (oCI.PayMethod == "1")
                {
                    oUI.isPO_Payment = 1;
                    oUI.isCC_Payment = 0;
                    oUI.isCOD_Payment = 0;

                }
                else if (oCI.PayMethod == "2")
                {
                    oUI.isPO_Payment = 0;
                    oUI.isCC_Payment = 1;
                    oUI.isCOD_Payment = 0;


                }
                else if (oCI.PayMethod == "4")
                {
                    oUI.isPO_Payment = 0;
                    oUI.isCC_Payment = 0;
                    oUI.isCOD_Payment = 1;

                }
                else if (oCI.PayMethod == "3")
                {
                    oUI.isPO_Payment = 1;
                    oUI.isCC_Payment = 1;
                    oUI.isCOD_Payment = 0;


                }
                else if (oCI.PayMethod == "6")
                {
                    oUI.isPO_Payment = 0;
                    oUI.isCC_Payment = 1;
                    oUI.isCOD_Payment = 1;


                }
                else if (oCI.PayMethod == "5")
                {
                    oUI.isPO_Payment = 1;
                    oUI.isCC_Payment = 0;
                    oUI.isCOD_Payment = 1;

                }
                else
                {
                    oUI.isPO_Payment = 0;
                    oUI.isCC_Payment = 0;
                    oUI.isCOD_Payment = 0;
                }
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
    }

    #endregion

    //protected void drpCountry_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    string countryCode = drpCountry.SelectedValue.ToString();
    //    LoadStates(countryCode);
    //}
    //protected void drpShipCountry_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    string ShipcountryCode = drpShipCountry.SelectedValue.ToString();
    //    ShipLoadStates(ShipcountryCode);
    //}
    //protected void drpBillCountry_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    string BillcountryCode = drpBillCountry.SelectedValue.ToString();
    //   // BillLoadStates(BillcountryCode);
    //}
    protected void vldCusAns_ServerValidate(object source, ServerValidateEventArgs args)
    {

    }
}
