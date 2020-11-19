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
using System.Text.RegularExpressions;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CommonServices;
public partial class UserBasicInfo : System.Web.UI.Page
{
    #region "Declarations..."
    UserServices objUserServices = new UserServices();
    CompanyGroupServices objCompanyGroupServices = new CompanyGroupServices();
    CompanyGroupServices.CompanyInfo CompInfo = new CompanyGroupServices.CompanyInfo();
    HelperServices objHelperServices = new HelperServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    public bool isValidCompany = false;
    public string _mCompanyID = "";
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
            txtCompany.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSubmit.UniqueID + "').click();return false;}} else {return true}; ");
            txtConfirmPassword.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSubmit.UniqueID + "').click();return false;}} else {return true}; ");
            txtEmail.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSubmit.UniqueID + "').click();return false;}} else {return true}; ");
            txtPassword.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSubmit.UniqueID + "').click();return false;}} else {return true}; ");
            if (lblErrorMsgUsrID.Text != "")
            {
                lblErrorMsgUsrID.Text = "";
            }
            lblchkavail.Text = "";
            Session["COUNT"] = "0";
      
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
    #region "Control Events"
    protected void lbCheckAvail_Click(object sender, EventArgs e)
    {
        //string pattern;
        string pattern = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|" +
                @"0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z]" +
                @"[a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";
        System.Text.RegularExpressions.Match match = Regex.Match(txtEmail.Text.Trim(), pattern, RegexOptions.IgnoreCase);
        if (match.Success)
        {
            if (txtEmail.Text.Trim().Length > 0)
            {
                try
                {
                    string sUserStatus = "";
                    if (objUserServices.CheckUserName(txtEmail.Text.Trim()))
                    {
                        sUserStatus = "INVALID";
                        lblchkavail.Text = (string)GetLocalResourceObject("msgUserExists1") + txtEmail.Text.Trim() + (string)GetLocalResourceObject("msgUserExists2");
                    }
                    else
                    {
                        string errmsg = txtEmail.Text.Trim();
                        int stringlen = 40;
                        while (errmsg.Length > stringlen)
                        {
                            errmsg = errmsg.Insert(stringlen, "<br/>");
                            stringlen = stringlen + 40;
                        }
                        sUserStatus = "VALID";
                        lblchkavail.Text = (string)GetLocalResourceObject("msgUserExists1") + errmsg + (string)GetLocalResourceObject("msgUserNotExists");
                    }
                    ////string sScript = "<script defer>\n" +

                    ////              "  var strReturn = window.open('AlertMessage.aspx?UserName=" + txtEmail.Text.Trim() + "&Result=" + sUserStatus + "','desc','left=300,Top=200,scrollbars=no,width=250,height=200');\n" +

                    ////              " __doPostBack('btnSetup',strReturn);" +

                    ////              "</script>";


                    ////if (!Page.IsClientScriptBlockRegistered("OpenDialog"))
                    ////{
                    ////    Page.RegisterStartupScript("OpenDialog", sScript);
                    ////}
                   
                }
                 
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }

            }
        }
        else
        {
            try
            {
                //string sScript = "<script>";
                //sScript = sScript + "alert('" + (string)GetLocalResourceObject("msgUserIDEmpty") + "')";
                //sScript = sScript + "</script>";
                //Page.RegisterClientScriptBlock("UserID", sScript);
                lblErrorMsgUsrID.Text = (string)GetLocalResourceObject("msgCheckValidUser");
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }

        }

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        try
        {
        lblchkavail.Text = "";
        lblcompanycheck.Text = "";
        UserServices .UserInfo oUI = new UserServices.UserInfo();
        //if (txtPassword.Text != txtPassword.Text.ToUpper())
        {
            if (!objUserServices.CheckUserName(txtEmail.Text))
            {

                oUI.Email = txtEmail.Text;
                oUI.Password = txtPassword.Text;
                if (txtCompany.Text.Trim().Length > 0)
                {
                    isValidCompany = objCompanyGroupServices.IsCompanyExist(txtCompany.Text.Trim().ToString());
                    if (isValidCompany)
                    {
                        if (!objUserServices.CheckUserName(_mCompanyID))
                        {
                            oUI.CompanyID = txtCompany.Text;
                            CompInfo = objCompanyGroupServices.GetCompanyInfo(objCompanyGroupServices.GetCompanyName(oUI.CompanyID));
                            Session["COMPANYPROFILE"] = CompInfo;
                            Session["USERPROFILE"] = oUI;
                            Response.Redirect("UserProfile.aspx?ComID=" + oUI.CompanyID);
                        }
                    }
                    else
                    {
                        //string sScript = "<script>";
                        //sScript = sScript + "alert('" + (string)GetLocalResourceObject("msgInValidCompany") + "')";
                        //sScript = sScript + "</script>";
                        //Page.RegisterClientScriptBlock("InvalidCompany", sScript);
                        lblcompanycheck.Text = (string)GetLocalResourceObject("msgInValidCompany").ToString();
                    }
                }
                else
                {
                    Session["USERPROFILE"] = oUI;
                    Response.Redirect("CompanyProfile.aspx?cNameEx=0");
                }
            }
            else
            {
                //string sScript = "<script>";
                //sScript = sScript + "alert('" + (string)GetLocalResourceObject("msgExistingUser") + " ');";
                ////sScript = sScript + "javascript:window.history.forward(1);)";
                //sScript = sScript + "</script>";
                //Page.RegisterClientScriptBlock("ExistingUser", sScript);
                lblErrorMsgUsrID.Text = (string)GetLocalResourceObject("msgExistingUser");
                lblcompanycheck.Text = "";

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
}
