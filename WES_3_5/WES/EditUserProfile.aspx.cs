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
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;

public partial class EditUserProfile : System.Web.UI.Page
{
    #region "Declarations..."

    HelperServices objHelperServices = new HelperServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    UserServices objUserServices = new UserServices();
    UserServices.UserInfo oUserinfo = new UserServices.UserInfo();
    CountryServices objCountryServices = new CountryServices();

    ConnectionDB objConnectionDB = new ConnectionDB();
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
                    LoadStates("AU");
                    AssignUserData(Session["USER_ID"].ToString());

                    //string countryCode = drpCountry.SelectedValue.ToString();
                    //LoadStates(countryCode);
                    string ShipcountryCode = drpShipCountry.SelectedValue.ToString();
                    //ShipLoadStates(ShipcountryCode);
                    string BillcountryCode = drpBillCountry.SelectedValue.ToString();
                    //BillLoadStates(BillcountryCode);
                }
                else
                {
                    Response.Redirect("login.aspx", false);
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
        if (Session["USER_ID"] != null && Session["USER_ID"].ToString() != "")
        {
            if (UpdateUserData() > 0)
            {
                Response.Redirect("ConfirmMessage.aspx?Result=UPDATE");
            }
        }
        else
        {
            Response.Redirect("login.aspx", false);
        }

    }

    protected void btnSubmitAddress_Click(object sender, EventArgs e)
    {
        SendMailUpdateAddress();
    }
    public void LoadShippingInfo(string sUserID)
    {
        try
        {

            int _UserID;
            _UserID = objHelperServices.CI(sUserID);
            oUserinfo = objUserServices.GetUserInfo(_UserID);
            txtShipCompName.Text = objUserServices.GetCompanyName(_UserID);
            //cmbPrefix.Text = oUserinfo.Prefix;
            txtShipFname.Text = oUserinfo.Contact ;
            txtshipadd1.Text = oUserinfo.ShipAddress1;
            txtshipadd2.Text = oUserinfo.ShipAddress2;
            //txtshipadd3.Text = oUserinfo.ShipAddress3;
            txtshipcity.Text = oUserinfo.ShipCity;
            drpShipState.Text = oUserinfo.ShipState;
            txtshipzip.Text = oUserinfo.ShipZip;
            drpShipCountry.SelectedValue = oUserinfo.ShipCountry;
            Setdrpdownlistvalue(drpShipCountry, oUserinfo.ShipCountry.ToString());
            txtshipphone.Text = oUserinfo.ShipPhone;
            //txtshipemail.Text = oUserinfo.AlternateEmail;
            txtshipFax.Text = oUserinfo.Fax;
            //txtshipDeliveryInst.Text =oUserinfo.
            //Modified by indu while updating for send sms
           // txtshipphone.Text = oUserinfo.Ship_Phone;
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

            txtbillCompanyName.Text = objUserServices.GetBillToCompanyName(_UserID);
            txtbillFName.Text = oUserinfo.FirstName  ;
            txtbilladd1.Text = oUserinfo.BillAddress1;
            txtbilladd2.Text = oUserinfo.BillAddress2;
            //txtbilladd3.Text = oUserinfo.BillAddress3;
            txtbillcity.Text = oUserinfo.BillCity;
            drpBillState.Text = oUserinfo.BillState;
            txtbillzip.Text = oUserinfo.BillZip;
            drpBillCountry.SelectedValue = oUserinfo.BillCountry;
            Setdrpdownlistvalue(drpBillCountry, oUserinfo.BillCountry.ToString());
            txtbillphone.Text = oUserinfo.BillPhone;
            txtbillFax.Text = oUserinfo.BillFax;
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
            //txtshipemail.Text = oUserinfo.AlternateEmail;
            txtshipFax.Text = oUserinfo.Fax;
            txtshipDeliveryInst.Text = oUserinfo.DeliveryInst;
            //txtbillemail.Text = oUserinfo.AlternateEmail;
            txtbillDeliveryInst.Text = oUserinfo.DeliveryInst;
            txtDELIVERYINST.Text = oUserinfo.DeliveryInst;
            //txtcompname.Text = objUserServices.GetCompanyName(_UserID);
            ////cmbPrefix.Text = oUserinfo.Prefix;
            //txtFname.Text = oUserinfo.Contact ;
            ////txtLName.Text = oUserinfo.LastName;
            ////txtSuffix.Text = oUserinfo.Suffix;
            ////txtMName.Text = oUserinfo.MiddleName;
            //drpCountry.SelectedValue = objUserServices.GetUserCountryCode(oUserinfo.Country);
            //txtFax.Text = oUserinfo.Fax;

            //txtMobile.Text = oUserinfo.MobilePhone;
            //txtPhone.Text = oUserinfo.Phone;
            //drpState.Text = oUserinfo.State;

            //txtAdd1.Text = oUserinfo.Address1;
            //txtAdd2.Text = oUserinfo.Address2;
            //txtAdd3.Text = oUserinfo.Address3;
            //txtCity.Text = oUserinfo.City;
            //txtAltEmail.Text = oUserinfo.AlternateEmail;
            //txtZip.Text = oUserinfo.Zip;
            //For Shipping Details
            LoadShippingInfo(Session["USER_ID"].ToString());

         

            txtComname.Text = txtShipCompName.Text.Trim();
            txt_attnto.Text = txtShipFname.Text.Trim();
            txtphonenumber.Text = oUserinfo.ShipPhone.ToString();
            //drpupdatecountry.SelectedValue = drpShipCountry.SelectedValue;
            objErrorHandler.CreateLog("drpShipCountry " + drpShipCountry.SelectedValue);
            drpupdatecountry.SelectedValue = oUserinfo.ShipCountry;
            Setdrpdownlistvalue(drpupdatecountry, oUserinfo.ShipCountry.ToString());
            txtupdateadd.Text = txtshipadd1.Text.Trim();
            txtupdateadd2.Text = txtshipadd2.Text.Trim();
            txtupdatetown.Text = txtshipcity.Text.Trim();
            txtupdatezip.Text = txtshipzip.Text.Trim();
            txtupdatestate.Text = drpShipState.Text.Trim();

            if (drpupdatecountry.SelectedValue.ToString().ToUpper() == "AU")
            {
                drpupdatestate.Text = drpShipState.Text.Trim();
                drpupdatestate.Style.Add("display", "block");
                txtupdatestate.Style.Add("display", "none");
            }
            else
            {
                txtupdatestate.Text = drpShipState.Text.Trim();
                txtupdatestate.Style.Add("display", "block");
                drpupdatestate.Style.Add("display", "none");
            }

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
        //txtshipadd3.Text = "";
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
        //txtbilladd3.Text = "";
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
        //txtshipadd3.Text = "";
        txtshipcity.Text = "";
        //txtshipcountry.Text = "";
        txtshipphone.Text = "";
        //txtshipstate.Text = "";
        txtshipzip.Text = "";
    }
    public void GetExistingBillAddr()
    {
    }
    public int UpdateUserData()
    {
        try
        {
            oUserinfo.UserID = objHelperServices.CI(Session["USER_ID"].ToString());
            ////oUserinfo.Prefix = cmbPrefix.Text;
            //oUserinfo.FirstName = txtFname.Text;
            ////oUserinfo.LastName = txtLName.Text;
            ////oUserinfo.MiddleName = txtMName.Text;
            ////oUserinfo.Suffix = txtSuffix.Text;
            //oUserinfo.Address1 = txtAdd1.Text;
            //oUserinfo.Address2 = txtAdd2.Text;
            //oUserinfo.Address3 = txtAdd3.Text;
            //oUserinfo.AlternateEmail = txtAltEmail.Text;
            //oUserinfo.City = txtCity.Text;

            //oUserinfo.State = drpState.Text;
            //oUserinfo.Country = drpCountry.Text;
            //oUserinfo.Zip = txtZip.Text;
            //oUserinfo.Fax = txtFax.Text;
            //oUserinfo.Phone = txtPhone.Text;
            //oUserinfo.MobilePhone = txtMobile.Text;
            //For Shipping Details
            oUserinfo.ShipAddress1 = txtshipadd1.Text;
            oUserinfo.ShipAddress2 = txtshipadd2.Text;
            //oUserinfo.ShipAddress3 = txtshipadd3.Text;
            oUserinfo.ShipCity = txtshipcity.Text;
            oUserinfo.ShipState = drpShipState.Text;
            oUserinfo.ShipZip = txtshipzip.Text;
            oUserinfo.ShipCountry = drpShipCountry.Text;
            oUserinfo.ShipPhone = txtshipphone.Text;
            //For Billing Details
            oUserinfo.BillAddress1 = txtbilladd1.Text;
            oUserinfo.BillAddress2 = txtbilladd2.Text;
            //oUserinfo.BillAddress3 = txtbilladd3.Text;
            oUserinfo.BillCity = txtbillcity.Text;
            oUserinfo.BillState = drpBillState.Text;
            oUserinfo.BillZip = txtbillzip.Text;
            oUserinfo.BillCountry = drpBillCountry.Text;
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
            //txtshipadd3.Text = txtbilladd3.Text;
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
            //txtbilladd1.Text = txtAdd1.Text;
            //txtbilladd2.Text = txtAdd2.Text;
            //txtbilladd3.Text = txtAdd3.Text;
            //txtbillcity.Text = txtCity.Text;
            //drpBillState.Text = drpState.Text;
            //drpBillCountry.Text = drpCountry.SelectedValue;
            //txtbillphone.Text = txtPhone.Text;
            //txtbillzip.Text = txtZip.Text;
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
            //oDs = objCountryServices.GetCountries();
            //drpCountry.Items.Clear();
            //drpCountry.DataSource = oDs.Tables[0];
            //drpCountry.DataValueField = oDs.Tables[0].Columns["COUNTRY_CODE"].ToString();
            //drpCountry.DataTextField = oDs.Tables[0].Columns["COUNTRY_NAME"].ToString();
            //drpCountry.DataBind();
            //drpCountry.SelectedIndex = drpCountry.Items.IndexOf(new ListItem("Australia", "AU"));

            oDs = new DataSet();
            oDs = objCountryServices.GetCountries();
            drpShipCountry.Items.Clear();
            drpShipCountry.DataSource = oDs;
            drpShipCountry.DataValueField = oDs.Tables[0].Columns["COUNTRY_CODE"].ToString();
            drpShipCountry.DataTextField = oDs.Tables[0].Columns["COUNTRY_NAME"].ToString();
            drpShipCountry.DataBind();
            drpShipCountry.SelectedIndex = drpShipCountry.Items.IndexOf(new ListItem("Australia", "AU"));

            oDs = new DataSet();
            oDs = objCountryServices.GetCountries();
            drpBillCountry.Items.Clear();
            drpBillCountry.DataSource = oDs;
            drpBillCountry.DataValueField = oDs.Tables[0].Columns["COUNTRY_CODE"].ToString();
            drpBillCountry.DataTextField = oDs.Tables[0].Columns["COUNTRY_NAME"].ToString();
            drpBillCountry.DataBind();
            drpBillCountry.SelectedIndex = drpBillCountry.Items.IndexOf(new ListItem("Australia", "AU"));

            oDs = new DataSet();
            oDs = objCountryServices.GetCountries();
            drpupdatecountry.Items.Clear();
            drpupdatecountry.DataSource = oDs;
            drpupdatecountry.DataValueField = oDs.Tables[0].Columns["COUNTRY_CODE"].ToString();
            drpupdatecountry.DataTextField = oDs.Tables[0].Columns["COUNTRY_NAME"].ToString();
            drpupdatecountry.DataBind();
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
            drpupdatestate.Items.Clear();
            drpupdatestate.DataSource = oDs;
            drpupdatestate.DataTextField = oDs.Tables[0].Columns["STATE_CODE"].ToString();
            drpupdatestate.DataValueField = oDs.Tables[0].Columns["STATE_CODE"].ToString();
            drpupdatestate.DataBind();
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

    private void SendMailUpdateAddress()
    {
        UserServices.UserInfo oUserInfo = new UserServices.UserInfo();
        try
        {
            int _UserrID;
            _UserrID = objHelperServices.CI(Session["USER_ID"].ToString());
            oUserInfo = objUserServices.GetUserInfo(_UserrID);
             string sHTML = "";
             try
             {
                 StringTemplateGroup _stg_container = null;
                 StringTemplate _stmpl_container = null;
                 string stemplatepath;
                 string state = string.Empty;
                 if (drpupdatecountry.SelectedItem.ToString().ToLower() == "australia")
                 {
                     state = drpupdatestate.SelectedItem.ToString().Trim();
                 }
                 else
                 {
                     state = txtupdatestate.Text.Trim();
                 }

                 stemplatepath = Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());

                 _stg_container = new StringTemplateGroup("main", stemplatepath);

                 _stmpl_container = _stg_container.GetInstanceOf("mail" + "\\" + "UpdateAddressRequest");

                 _stmpl_container.SetAttribute("CUSTOMER_NO", oUserInfo.CompanyID.ToString());
                 _stmpl_container.SetAttribute("REQUESTED_BY", oUserInfo.Contact.ToString());
                 _stmpl_container.SetAttribute("USER_ID", _UserrID.ToString());
                 _stmpl_container.SetAttribute("BUSINESS_NAME", txtComname.Text.ToString());
                 _stmpl_container.SetAttribute("RECEIVER_NAME", txt_attnto.Text.ToString());
                 _stmpl_container.SetAttribute("STREET_ADDRESS1", txtupdateadd.Text.ToString());

                 if (txtupdateadd2.Text.ToString().Trim() != "")
                 {
                     _stmpl_container.SetAttribute("STREET_ADDRESS2", txtupdateadd2.Text.ToString());
                     _stmpl_container.SetAttribute("STREET_ADDRESS2_SHOW", true);
                 }
                 else
                 {
                     _stmpl_container.SetAttribute("STREET_ADDRESS2_SHOW", false);
                 }
                 _stmpl_container.SetAttribute("TOWN", txtupdatetown.Text.ToString());
                 _stmpl_container.SetAttribute("STATE", state);
                 _stmpl_container.SetAttribute("ZIPCODE", txtupdatezip.Text.ToString());
                 _stmpl_container.SetAttribute("COUNTRY", drpupdatecountry.SelectedItem.ToString());
                 _stmpl_container.SetAttribute("PHONE_NUMBER", txtphonenumber.Text.ToString());
                 _stmpl_container.SetAttribute("DELIVERY_INST", txtDELIVERYINST.Text.ToString());
                 sHTML = _stmpl_container.ToString();

                 if (sHTML != "")
                 {
                     objNotificationServices.NotifyConnection = objConnectionDB.GetConnection();
                     System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
                     MessageObj.From = new System.Net.Mail.MailAddress(oUserInfo.AlternateEmail);
                    //MessageObj.From = new System.Net.Mail.MailAddress("accounts@wes.net.au");
                    //MessageObj.To.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
                    //MessageObj.To.Add(oUserInfo.AlternateEmail);
                    //MessageObj.To.Add("accounts@wes.net.au");
                    //MessageObj.Bcc.Add("naresh@jtechindia.com");
                    MessageObj.To.Add(System.Configuration.ConfigurationManager.AppSettings["Accountsemail"].ToString());
                    MessageObj.Bcc.Add(System.Configuration.ConfigurationManager.AppSettings["Reviewemail"].ToString());
                    string message = "";
                     MessageObj.Subject = "Wes User Address Update Request Form";
                     MessageObj.IsBodyHtml = true;
                     //message = message + "<tr><td>User Id</td><td>&nbsp;</td><td>" + _UserrID.ToString() + "</td></tr>";
                     //message = message + "<tr><td>Bussiness Name</td><td>&nbsp;</td><td>" + txtComname.Text.ToString() + "</td></tr>";
                     //message = message + "<tr><td>Receivers Name</td><td>&nbsp;</td><td>" + txt_attnto.Text.ToString() + "</td></tr>";
                     //message = message + "<tr><td>Street Address</td><td>&nbsp;</td><td>" + txtupdateadd.Text.ToString() + "</td></tr>";
                     //if (txtupdateadd2.Text.ToString() != "")
                     //{
                     //    message = message + "<tr><td>Address Line2</td><td>&nbsp;</td><td>" + txtupdateadd2.Text.ToString() + " </td></tr>";
                     //}
                     //message = message + "<tr><td>Suburb/Town</td><td>&nbsp;</td><td>" + txtupdatetown.Text.ToString() + "</td></tr>";
                     //message = message + "<tr><td>State/Province</td><td>&nbsp;</td><td>" + txtupdatestate.Text.ToString() + "</td></tr>";
                     //message = message + "<tr><td>Postcode/ZipCode</td><td>&nbsp;</td><td>" + txtupdatezip.Text.ToString() + "</td></tr>";
                     //message = message + "<tr><td>Country</td><td>&nbsp;</td><td>" + drpupdatecountry.SelectedItem.ToString() + "</td></tr>";
                     //message = message + "<tr><td>Delivery Instruction</td><td>&nbsp;</td><td>" + txtDELIVERYINST.Text.ToString() + "</td></tr>";
                     //MessageObj.Body = "<html><body><table>" + message + "</table>" + " <font color=\"red\"></font></body></html>";
                     MessageObj.Body = sHTML;
                     System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
                     smtpclient.UseDefaultCredentials = false;
                     smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
                     smtpclient.Send(MessageObj);
                     Response.Redirect("ConfirmMessage.aspx?Result=ADDRUPDATEREQUESTSENT", false);
       
                 }
             }
             catch (Exception e)
             {
                 Response.Redirect("ConfirmMessage.aspx?Result=ADDRUPDATEREQUESTNOTSENT");
             }
        }
        catch (Exception)
        {
            Response.Redirect("ConfirmMessage.aspx?Result=ADDRUPDATEREQUESTNOTSENT");
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
