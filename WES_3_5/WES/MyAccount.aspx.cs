using System;
using System.Data;
using System.Data.SqlClient;
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
using TradingBell.WebCat.CommonServices; 

public partial class MyAccount : System.Web.UI.Page
{
    UserServices objUserServices = new UserServices();
    HelperServices objHelperServices = new HelperServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();                                             
    }



    private bool GetRole()
    {
        bool retvalue = false;
        ConnectionDB oConStr = new ConnectionDB();
        SqlConnection oCon = new SqlConnection();
        return retvalue;
    }
    public void MultiUserEdit_Click(object sender, EventArgs e)
    {

        try
        {
        string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
        string UserID = HttpContext.Current.Session["USER_ID"].ToString();
        int UserLogWebsiteId = objUserServices.GetUserWebSite_id(UserID);
        string webTitle = objUserServices.GetWebTitle(UserLogWebsiteId.ToString());
        Response.Redirect("MultiUserSetup.aspx",false);
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
        //if (websiteid == UserLogWebsiteId.ToString() && UserLogWebsiteId > 0 && UserLogWebsiteId != null)
        //{
        //    Response.Redirect("MultiUserSetup.aspx");
        //}
        //else
        //{
        //    ClientScript.RegisterStartupScript(typeof(Page), "WESAlert", "<script type='text/javascript'>alert('User account can be created on " + webTitle + " website only');</script>", false);
        //}


    }
}
