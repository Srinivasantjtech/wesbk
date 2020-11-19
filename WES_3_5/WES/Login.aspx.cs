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
using System.Diagnostics;
using System.Text;
using System.IO;
public partial class Login : System.Web.UI.Page
{
    Stopwatch sw = new Stopwatch();
   // ErrorHandler objErrorHandler = new ErrorHandler();

    private bool CheckCredential()
    {
        bool returnvalue = false;
        bool validUser;
        string username;
        string password;
        string login;

        int UserID;
        TradingBell.WebCat.Helpers.Security objSecurity = new TradingBell.WebCat.Helpers.Security();
        UserServices objUserServices = new UserServices();
        //TradingBell.WebCat.Helpers.ErrorHandler objErrorHandler = new TradingBell.WebCat.Helpers.ErrorHandler();
        CompanyGroupServices objCompanyGroupServices = new CompanyGroupServices();

        if (Request.Cookies["LoginInfo"] != null && Request.Cookies["LoginInfo"].Value.ToString().Trim() != "")
        {
            try
            {
                login = "False";
                HttpCookie LoginInfoCookie = Request.Cookies["LoginInfo"];
                username = objSecurity.StringDeCrypt(LoginInfoCookie["UserName"].ToString());
                password = objSecurity.StringDeCrypt(LoginInfoCookie["Password"].ToString());
                if (LoginInfoCookie["Login"] != null)
                    login = objSecurity.StringDeCrypt(LoginInfoCookie["Login"].ToString());
                validUser = objUserServices.CheckUserName(username);
                UserID = objUserServices.GetUserID(username);
                if (UserID != -1 && username != string.Empty && login == "True")
                {
                    if (objCompanyGroupServices.CheckCompanyStatus(UserID) == CompanyGroupServices.CompanyStatus.ACTIVE.ToString())
                    {
                        if ((validUser))
                        {
                            bool HasAdminUser = objUserServices.HasAdmin(UserID);
                            if (objUserServices.IsUserActive(UserID.ToString()))
                            {
                                password = objSecurity.StringEnCrypt(password);
                                if (objUserServices.CheckUser(username, password))
                                {
                                    string Role;
                                    Role = objUserServices.GetRole(UserID);

                                    if (Role != null)
                                    {
                                        returnvalue = true;
                                        objUserServices.OnLineFlag(true, UserID);
                                        Session["USER_NAME"] = username;
                                        Session["USER_ID"] = UserID;
                                        Session["USER_ROLE"] = Role;
                                        Session["COMPANY_ID"] = objUserServices.GetCompanyID(UserID);
                                        Session["CUSTOMER_TYPE"] = objUserServices.GetCustomerType(UserID);
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                // objErrorHandler.ErrorMsg = ex;
                // objErrorHandler.CreateLog();
            }

        }
        else
            returnvalue = false;



        return returnvalue;

    }

    protected void Page_Load(object sender, EventArgs e)
    {

       
        
        HelperServices objHelperServices = new HelperServices();
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
        Security objSecurity=new Security();
        //string e1 = objSecurity.StringDeCrypt("");

       // string e2  = objSecurity.StringDeCrypt("");
        
  
        if ((CheckCredential()))
            HttpContext.Current.Response.Redirect("home.aspx");
       
            
           
        

        //HtmlMeta meta = new HtmlMeta();
        //meta.Name = "keywords";
        //meta.Content = oHelper.GetOptionValues("Meta keyword").ToString();
        //this.Header.Controls.Add(meta);

        //// Render: <meta name="Description" content="noindex" />
        //meta = new HtmlMeta();
        //meta.Name = "Description";
        //meta.Content = oHelper.GetOptionValues("Meta Description").ToString();
        //this.Header.Controls.Add(meta);

        //// Render: <meta name="Abstraction" content="Some words listed here" />

        //meta.Name = "Abstraction";
        //meta.Content = oHelper.GetOptionValues("Meta Abstraction").ToString();
        //this.Header.Controls.Add(meta);

        //// Render: <meta name="Distribution" content="noindex" />
        //meta = new HtmlMeta();
        //meta.Name = "Distribution";
        //meta.Content = oHelper.GetOptionValues("Meta Distribution").ToString();
        //this.Header.Controls.Add(meta);
      
        //meta.Name = "keywords";
        //meta.Content = oHelper.GetOptionValues("Meta keyword").ToString();
        //this.Header.Controls.Add(meta);

        //// Render: <meta name="Description" content="noindex" />
        //meta = new HtmlMeta();
        //meta.Name = "Description";
        //meta.Content = oHelper.GetOptionValues("Meta Description").ToString();
        //this.Header.Controls.Add(meta);

        //// Render: <meta name="Abstraction" content="Some words listed here" />

        //meta.Name = "Abstraction";
        //meta.Content = oHelper.GetOptionValues("Meta Abstraction").ToString();
        //this.Header.Controls.Add(meta);

        //// Render: <meta name="Distribution" content="noindex" />
        //meta = new HtmlMeta();
        //meta.Name = "Distribution";
        //meta.Content = oHelper.GetOptionValues("Meta Distribution").ToString();
        //this.Header.Controls.Add(meta);
        //InitLoad();
        //txtUserName.Focus();
        //if (Request["Result"] == "SUCCESS")
        //    RegSucess.Visible = true;
        //int i = 1;
        //txtUserName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + cmdLogin.UniqueID + "').click();return false;}} else {return true}; ");
        //txtPassword.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + cmdLogin.UniqueID + "').click();return false;}} else {return true}; ");
        
        //if (!IsPostBack)
        //{
        //    try
        //    {
        //        sReferralURL = Request.ServerVariables["HTTP_REFERER"].ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
    }
    
}
