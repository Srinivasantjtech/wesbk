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
public partial class RetailerEditUserProfile : System.Web.UI.Page
{
    #region "Declarations..."

    HelperServices objHelperServices = new HelperServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    UserServices objUserServices = new UserServices();
    UserServices.UserInfo oUserinfo = new UserServices.UserInfo();
    CountryServices objCountryServices = new CountryServices();
    ConnectionDB objConnectionDB = new ConnectionDB();
    Security objSecurity = new Security();
    string oldmail = "";
    int userid = 0;
    NotificationServices objNotificationServices = new NotificationServices();
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
        HtmlMeta meta = new HtmlMeta();
        meta.Name = "keywords";
        meta.Content = objHelperServices.GetOptionValues("Meta keyword").ToString();
        this.Header.Controls.Add(meta);

        // Render: <meta name="Description" content="noindex" />
        meta = new HtmlMeta();
        meta.Name = "Description";
        meta.Content = objHelperServices.GetOptionValues("Meta Description").ToString();
        this.Header.Controls.Add(meta);

        // Render: <meta name="Abstraction" content="Some words listed here" />

        meta.Name = "Abstraction";
        meta.Content = objHelperServices.GetOptionValues("Meta Abstraction").ToString();
        this.Header.Controls.Add(meta);

        // Render: <meta name="Distribution" content="noindex" />
        meta = new HtmlMeta();
        meta.Name = "Distribution";
        meta.Content = objHelperServices.GetOptionValues("Meta Distribution").ToString();
        this.Header.Controls.Add(meta);
        try
        {


            if (!IsPostBack)
            {
                if (Session["USER_ID"] != null && Session["USER_ID"].ToString() != "")
                {
                    LoadCountryList();
                    AssignUserData(Session["USER_ID"].ToString());

                    string countryCode = drpCountry.SelectedValue.ToString();
                    //LoadStates(countryCode);
                    string ShipcountryCode = drpShipCountry.SelectedValue.ToString();
                    //ShipLoadStates(ShipcountryCode);
                    string BillcountryCode = drpBillCountry.SelectedValue.ToString();
                    //BillLoadStates(BillcountryCode);
                }
                else
                {
                  // Response.Redirect("login.aspx", false);
                    string strActionType = objSecurity.StringEnCrypt("RequireLogin");
                    HttpContext.Current.Response.Redirect("login.aspx?ActionType=" + strActionType);
                }

            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    #region "Events..."
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
       try
       {
        
        DataSet dsMail =new DataSet(); 
        if (Session["USER_ID"] != null && Session["USER_ID"].ToString() != "")
        {
           userid = Convert.ToInt32(Session["USER_ID"].ToString().Trim());
           oldmail=objUserServices.GetUserEmailAdd(userid);

            if (oldmail != txtAltEmail.Text.Trim().ToString())   
            {
                dsMail = objUserServices.CheckCustomerRegistrationExists(txtAltEmail.Text.Trim().ToString(), "Retailer");
                   if (dsMail!=null && dsMail.Tables.Count>0 && dsMail.Tables[0].Rows.Count>0)
                   {
                       ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('Mail address already exists');", true);                
                       return;
                   }
            }

            if (UpdateUserData() > 0)
            {
                if (oldmail != txtAltEmail.Text.Trim().ToString())
                {
                    objUserServices.SetUserRole(userid.ToString(), 4);
                    SendUpdateCustomer(userid);
                }
                else
                {
                    Response.Redirect("ConfirmMessage.aspx?Result=UPDATE");
                }
            }
        }
        else
        {
            Response.Redirect("login.aspx", false);
        }
       }
       catch (Exception ex)
       {
           objErrorHandler.ErrorMsg = ex;
           objErrorHandler.CreateLog();

       }
    }
    public void LoadShippingInfo(string sUserID)
    {
        try
        {

            int _UserID;
            _UserID = objHelperServices.CI(sUserID);
            oUserinfo = objUserServices.GetUserInfo(_UserID);
            //txtShipCompName.Text = objUserServices.GetCompanyName(_UserID);
            //cmbPrefix.Text = oUserinfo.Prefix;
           // txtShipFname.Text = oUserinfo.FirstName;
            txtshipadd1.Text = oUserinfo.ShipAddress1;
            txtshipadd2.Text = oUserinfo.ShipAddress2;
            txtshipadd3.Text = oUserinfo.ShipAddress3;
            txtshipcity.Text = oUserinfo.ShipCity;
            drpShipState.Text = oUserinfo.ShipState;
            txtshipzip.Text = oUserinfo.ShipZip;
            drpShipCountry.SelectedValue = oUserinfo.ShipCountry;
            Setdrpdownlistvalue(drpShipCountry, oUserinfo.ShipCountry.ToString());
            txtshipphone.Text = oUserinfo.ShipPhone;
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
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
    public void LoadBillInfo(string sUserID)
    {
        try
        {
            int _UserID;
            _UserID = objHelperServices.CI(sUserID);
            oUserinfo = objUserServices.GetUserBillInfo(_UserID);

            //txtbillCompanyName.Text = objUserServices.GetBillToCompanyName(_UserID);
            //txtbillFName.Text = oUserinfo.FirstName;
            txtbilladd1.Text = oUserinfo.BillAddress1;
            txtbilladd2.Text = oUserinfo.BillAddress2;
            txtbilladd3.Text = oUserinfo.BillAddress3;
            txtbillcity.Text = oUserinfo.BillCity;
            drpBillState.Text = oUserinfo.BillState;
            txtbillzip.Text = oUserinfo.BillZip;
            drpBillCountry.SelectedValue = oUserinfo.BillCountry;
            Setdrpdownlistvalue(drpBillCountry, oUserinfo.BillCountry.ToString());
            txtbillphone.Text = oUserinfo.BillPhone;
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    public void AssignUserData(string sUserID)
    {
        try
        {
            int _UserID;
            _UserID = objHelperServices.CI(sUserID);
            oUserinfo = objUserServices.GetUserInfo(_UserID);
            txtcompname.Text = objUserServices.GetCompanyName(_UserID);
            //cmbPrefix.Text = oUserinfo.Prefix;
            txtFname.Text = oUserinfo.FirstName;
            //txtLName.Text = oUserinfo.LastName;
            //txtSuffix.Text = oUserinfo.Suffix;
            //txtMName.Text = oUserinfo.MiddleName;
            drpCountry.SelectedValue = objUserServices.GetUserCountryCode(oUserinfo.Country);
            txtFax.Text = oUserinfo.Fax;

            txtMobile.Text = oUserinfo.MobilePhone;
            txtPhone.Text = oUserinfo.Phone;
            drpState.Text = oUserinfo.State;

            txtAdd1.Text = oUserinfo.Address1;
            txtAdd2.Text = oUserinfo.Address2;
            txtAdd3.Text = oUserinfo.Address3;
            txtCity.Text = oUserinfo.City;
            txtAltEmail.Text = oUserinfo.AlternateEmail;
            txtZip.Text = oUserinfo.Zip;
            //For Shipping Details
            LoadShippingInfo(Session["USER_ID"].ToString());
            //For Billing Details
            LoadBillInfo(Session["USER_ID"].ToString());


        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    public void ClearShippingInfo()
    {
        txtshipadd1.Text = "";
        txtshipadd2.Text = "";
        txtshipadd3.Text = "";
        txtshipcity.Text = "";
        //txtshipcountry.Text = "";
        txtshipphone.Text = "";
        //txtshipstate.Text = "";
        txtshipzip.Text = "";
    }
    public void ClearBillingInfo()
    {
        txtbilladd1.Text = "";
        txtbilladd2.Text = "";
        txtbilladd3.Text = "";
        txtbillcity.Text = "";
        //txtbillcountry.Text = "";
        txtbillphone.Text = "";
        //txtbillstate.Text = "";
        txtbillzip.Text = "";
    }
    public void GetExistingShipAddr()
    {
        txtshipadd1.Text = "";
        txtshipadd2.Text = "";
        txtshipadd3.Text = "";
        txtshipcity.Text = "";
        //txtshipcountry.Text = "";
        txtshipphone.Text = "";
        //txtshipstate.Text = "";
        txtshipzip.Text = "";
    }
    public void GetExistingBillAddr()
    {
    }
    private void SendUpdateCustomer(int User_id)
    {
        try
        {
            string url = HttpContext.Current.Request.Url.Authority.ToString();
            string activeLink = string.Format("http://" + url + "/Activation.aspx?id={0}", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(User_id.ToString())));
            objNotificationServices.NotifyConnection = objConnectionDB.GetConnection();
            System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
            //MessageObj.From = new System.Net.Mail.MailAddress(txtemail.Text.ToString());                 
            MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
            //MessageObj.To.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
            MessageObj.To.Add(txtAltEmail.Text.ToString());
            string message = "";
            MessageObj.Subject = "WESonline-Form-Update Profile";
            MessageObj.IsBodyHtml = true;
            message = message + "<tr><td>Name</td><td>&nbsp;</td><td>" + txtFname.Text.ToString() + "</td></tr>";                        
            message = message + "<tr><td>Phone</td><td>&nbsp;</td><td>" + txtPhone.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Mobile/Cell Phone</td><td>&nbsp;</td><td>" + txtMobile.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Fax</td><td>&nbsp;</td><td>" + txtFax.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Email</td><td>&nbsp;</td><td>" + txtAltEmail.Text.ToString() + "</td></tr>";


            message = message + "<tr><td>Street Address</td><td>&nbsp;</td><td>" + txtAdd1.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Address Line2</td><td>&nbsp;</td><td>" + txtAdd2.Text.ToString() + " </td></tr>";
            message = message + "<tr><td>Suburp/Town</td><td>&nbsp;</td><td>" + txtCity.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>State/Province</td><td>&nbsp;</td><td>" + drpState.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Postcode/ZipCode</td><td>&nbsp;</td><td>" + txtZip.Text.ToString() + "</td></tr>";
            message = message + "<tr><td>Country</td><td>&nbsp;</td><td>" + drpCountry.SelectedItem.ToString() + "</td></tr>";            
            MessageObj.Body = "<html><body><table>" + message + "</table>" + "</br> <font color=\"Red\">Activation Link :</font></br><font color=\"Blue\">" + activeLink + "</font></body></html>";
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

    public int UpdateUserData()
    {
        try
        {
            oUserinfo.UserID = objHelperServices.CI(Session["USER_ID"].ToString());
            //oUserinfo.Prefix = cmbPrefix.Text;   
            oUserinfo.COMPANY_NAME = txtcompname.Text; 
            oUserinfo.FirstName = txtFname.Text;
            //oUserinfo.LastName = txtLName.Text;
            //oUserinfo.MiddleName = txtMName.Text;
            //oUserinfo.Suffix = txtSuffix.Text;
            oUserinfo.Address1 = txtAdd1.Text;
            oUserinfo.Address2 = txtAdd2.Text;
            oUserinfo.Address3 = txtAdd3.Text;
            oUserinfo.Email  = txtAltEmail.Text;
            oUserinfo.City = txtCity.Text;

            oUserinfo.State = drpState.Text;
           // oUserinfo.Country = drpCountry.Text;
            oUserinfo.Country = drpCountry.SelectedItem.ToString();
            oUserinfo.Zip = txtZip.Text;
            oUserinfo.Fax = txtFax.Text;
            oUserinfo.Phone = txtPhone.Text;
            oUserinfo.MobilePhone = txtMobile.Text;
            //For Shipping Details
            oUserinfo.ShipAddress1 = txtshipadd1.Text;
            oUserinfo.ShipAddress2 = txtshipadd2.Text;
            oUserinfo.ShipAddress3 = txtshipadd3.Text;
            oUserinfo.ShipCity = txtshipcity.Text;
            oUserinfo.ShipState = drpShipState.Text;
            oUserinfo.ShipZip = txtshipzip.Text;
           // oUserinfo.ShipCountry = drpShipCountry.Text;
            oUserinfo.ShipCountry = drpShipCountry.SelectedItem.ToString();
            oUserinfo.ShipPhone = txtshipphone.Text;
            //For Billing Details
            oUserinfo.BillAddress1 = txtbilladd1.Text;
            oUserinfo.BillAddress2 = txtbilladd2.Text;
            oUserinfo.BillAddress3 = txtbilladd3.Text;
            oUserinfo.BillCity = txtbillcity.Text;
            oUserinfo.BillState = drpBillState.Text;
            oUserinfo.BillZip = txtbillzip.Text;
            oUserinfo.BillCountry = drpBillCountry.SelectedItem.ToString();
            oUserinfo.BillPhone = txtbillphone.Text;

            return objUserServices.UpdateUserInfo(oUserinfo);
              
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return -1;
        }
    }
    protected void ChkShippingAdd_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkShippingAdd.Checked == true)
        {
            txtshipadd1.Text = txtbilladd1.Text;
            txtshipadd2.Text = txtbilladd2.Text;
            txtshipadd3.Text = txtbilladd3.Text;
            txtshipcity.Text = txtbillcity.Text;
            drpShipState.Text = drpBillState.Text;
            drpShipCountry.Text = drpBillCountry.SelectedValue;
            txtshipphone.Text = txtbillphone.Text;
            txtshipzip.Text = txtbillzip.Text;
        }
        else
        {
            LoadShippingInfo(Session["USER_ID"].ToString());
        }
    }
    protected void ChkBillingAdd_CheckedChanged1(object sender, EventArgs e)
    {
        if (ChkBillingAdd.Checked == true)
        {
            txtbilladd1.Text = txtAdd1.Text;
            txtbilladd2.Text = txtAdd2.Text;
            txtbilladd3.Text = txtAdd3.Text;
            txtbillcity.Text = txtCity.Text;
            drpBillState.Text = drpState.Text;
            drpBillCountry.Text = drpCountry.SelectedValue;
            txtbillphone.Text = txtPhone.Text;
            txtbillzip.Text = txtZip.Text;
        }
        else
        {
            LoadBillInfo(Session["USER_ID"].ToString());
        }
    }
    public void LoadCountryList()
    {
        try
        {
            DataSet oDs = new DataSet();
            oDs = objCountryServices.GetCountries();
            drpCountry.Items.Clear();
            drpCountry.DataSource = oDs.Tables[0];
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
            //DataSet oDs = new DataSet();
            //oDs = oCountry.GetStates(conCode);
            //drpState.Items.Clear();
            //drpState.DataSource = oDs;
            //drpState.DataTextField = oDs.Tables[0].Columns["STATE_NAME"].ToString();
            //drpState.DataValueField = oDs.Tables[0].Columns["STATE_CODE"].ToString();
            drpState.DataBind();
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    public void ShipLoadStates(String conCode)
    {
        try
        {

            //DataSet oDs = new DataSet();
            //    oDs = oCountry.GetStates(conCode);
            //    drpShipState.Items.Clear();
            //    drpShipState.DataSource = oDs;
            //    drpShipState.DataTextField = oDs.Tables[0].Columns["STATE_NAME"].ToString();
            //    drpShipState.DataValueField = oDs.Tables[0].Columns["STATE_CODE"].ToString();
            //   drpShipState.DataBind();
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    public void BillLoadStates(String conCode)
    {
        try
        {
            //DataSet oDs = new DataSet();
            //oDs = oCountry.GetStates(conCode);
            //drpBillState.Items.Clear();
            //drpBillState.DataSource = oDs;
            //drpBillState.DataTextField = oDs.Tables[0].Columns["STATE_NAME"].ToString();
            //drpBillState.DataValueField = oDs.Tables[0].Columns["STATE_CODE"].ToString();
            //drpBillState.DataBind();
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }

    //protected void drpBillCountry_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    string BillcountryCode = drpBillCountry.SelectedValue.ToString();
    //    //BillLoadStates(BillcountryCode);
    //}
    //protected void drpShipCountry_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    string ShipcountryCode = drpShipCountry.SelectedValue.ToString();
    //   //ShipLoadStates(ShipcountryCode);

    //}

    //protected void drpCountry_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    string countryCode = drpCountry.SelectedValue.ToString();
    //    //LoadStates(countryCode);

    //}
    #endregion
}
