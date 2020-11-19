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
public partial class CompanyProfile : System.Web.UI.Page
{
    #region "Declarations..."

    CompanyGroupServices objCompanyGroupServices = new CompanyGroupServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    CountryServices objCountryServices = new CountryServices();
    HelperServices objHelperServices = new HelperServices();
    string compid = "";
    #endregion

    #region "Functions..."
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
}
 catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    public void LoadStates(String conCode)
    {
        DataSet oDs = new DataSet();
        oDs = objCountryServices.GetStates(conCode);
        //drpState.DataSource = oDs;
       // drpState.DataTextField = oDs.Tables[0].Columns["STATE_NAME"].ToString();
       // drpState.DataValueField = oDs.Tables[0].Columns["STATE_CODE"].ToString();
       // drpState.DataBind();
    }
    public void GetCompanyProfile()
    {
        try
{
int _PayMethod = 0;
        if (chkPO.Checked==true)
        {
            _PayMethod = _PayMethod + 1;
        }
        if (chkCC.Checked==true)
        {
            _PayMethod = _PayMethod + 2;
        }
        if (chkCOD.Checked==true)
        {
            _PayMethod = _PayMethod + 4;
        }
        CompanyGroupServices.CompanyInfo oCI = new CompanyGroupServices.CompanyInfo();
        if (objCompanyGroupServices.CompID_Type.ToLower() == "auto")
        {
            oCI.CompanyID = objCompanyGroupServices.GenerateCompanyID().ToString();
        }
        else if (objCompanyGroupServices.CompID_Type.ToLower() == "custom")
        {
            oCI.CompanyID = txtCompanyID.Text;
        }
        compid = oCI.CompanyID;
        oCI.CompanyName = txtCompanyName.Text;
        oCI.TaxID = txtTaxID.Text;
        oCI.Address1 = txtAdd1.Text;
        oCI.Address2 = txtAdd2.Text;
        oCI.Address3 = txtAdd3.Text;
        oCI.City = txtCity.Text;
        oCI.State = drpState.Text;
        oCI.Zip = txtZip.Text;
        oCI.Country = drpCountry.SelectedValue.ToString();
        oCI.Phone1 = txtPhone1.Text;
        oCI.Phone2 = txtPhone2.Text;
        oCI.TollFree = txtTollFree.Text;
        oCI.Fax = txtFax.Text;
        oCI.Email = txtEmail.Text;
        oCI.Web = txtWeb.Text;
        oCI.Status = CompanyGroupServices.CompanyStatus.NEWCOMPANY; //CompanyGroup.CompanyStatus.ACTIVE;
        oCI.CSC = txtSecurityCode.Text;
        oCI.ContactName = txtPrimaryContact.Text;
        oCI.PayMethod = _PayMethod.ToString();
        oCI.BuyerGroup = "DEFAULTBG";
        Session["COMPANYPROFILE"] = oCI;
}
 catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }    
}
    #endregion

    #region "Events..."
    protected void Page_Load(object sender, EventArgs e)
    {
        objCompanyGroupServices.CompID_Type = "auto";
        if (objCompanyGroupServices.CompID_Type == "custom")
        {
            txtCompanyID.Focus();
        }
        else
        {
            txtCompanyName.Focus();
        }
        try
        {
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
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
        if (!IsPostBack)
        {
            LoadCountryList();            
            string countryCode = drpCountry.SelectedValue.ToString();
           // LoadStates(countryCode);
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            if (objCompanyGroupServices.CompID_Type.ToLower() == "custom")
            {
                if (objCompanyGroupServices.isCompanyIDExists(txtCompanyID.Text))
                {
                    Response.Redirect("CompanyProfile.aspx?cNameEx=1", false);
                }
            }
            if (objCompanyGroupServices.isCompanyNameExists(txtCompanyName.Text.Replace("'","''")))
            {
                Response.Redirect("CompanyProfile.aspx?cNameEx=2", false);
            }
            else
            {
                if (chkPO.Checked == true || chkCOD.Checked == true || chkCC.Checked == true)
                {
                    GetCompanyProfile();
                    Response.Redirect("UserProfile.aspx?&ComID=" + compid, false);
                }
                else
                {
                    string scr = @"<script language='javascript' type='text/javascript'>                 
                    alert('" + GetLocalResourceObject("errMsg01").ToString() + @"');          
                    </script>";
                    Page.RegisterClientScriptBlock("chkPayment", scr);
                    chkPO.Focus();
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
    protected void drpCountry_SelectedIndexChanged(object sender, EventArgs e)
    {     
        LoadStates(drpCountry.SelectedValue.ToString());
    }
    protected void txtCompanyName_TextChanged(object sender, EventArgs e)
    {

    }
}