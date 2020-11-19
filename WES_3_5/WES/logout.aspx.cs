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
using System.IO;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class logout : System.Web.UI.Page
{
    ErrorHandler objErrorHandler = new ErrorHandler();
    Security objSecurity = new Security();
    protected void Page_Load(object sender, EventArgs e)
    {     
   
        try
        {
        HelperServices objHelperServices = new HelperServices();
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
        UserServices objUserServices = new UserServices();
        objUserServices.OnLineFlag(false, objHelperServices.CI(Session["USER_ID"]));
        if (Session["pdffile"] != null && Session["pdffile"].ToString() != string.Empty)
        {
            string filename = Session["PdfFileName"].ToString();
            System.IO.FileInfo filin = new System.IO.FileInfo(Server.MapPath("~/Invoices\\" + "In" + filename + ".pdf"));
            bool fileExists = filin.Exists;
            if ((fileExists))
            {
                filin.Delete();
            }
            else
            {
                HttpContext.Current.Session["PdfFileName"] = null;
            }
        }
 
        Session.RemoveAll();
        Session.Clear();
        Session.Abandon();
        Session["USER_ID"] = "";
        if (Request.Cookies["LoginInfo"] != null && Request.Cookies["LoginInfo"].Value.ToString().Trim() != "")
        {
            HttpCookie LoginInfoCookie = Request.Cookies["LoginInfo"];
            LoginInfoCookie["Login"] = objSecurity.StringEnCrypt("False");
            HttpContext.Current.Response.AppendCookie(LoginInfoCookie);
        }         
        Response.Redirect("Login.aspx",false);

        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
}
