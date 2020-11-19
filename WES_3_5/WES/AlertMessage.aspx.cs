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
public partial class AlertMessage : System.Web.UI.Page
{
    ErrorHandler objErrorHandler = new ErrorHandler();
    HelperDB objHelperDB = new HelperDB();
    HelperServices objHelperServices = new HelperServices();
    protected void Page_Load(object sender, EventArgs e)
    {     
        try
        {
            Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
            if (Request["Result"] == "NTVALID")
            {
                lblMessage.Text = (string)GetLocalResourceObject("msgUserExists1") + Request["UserName"] + (string)GetLocalResourceObject("msgUserNotValid");
            }
            if (Request["Result"] == "INVALID")
            {
                lblMessage.Text = (string)GetLocalResourceObject("msgUserExists1") + Request["UserName"] + (string)GetLocalResourceObject("msgUserExists2");
            }
            if (Request["Result"] == "VALID")
            {
                lblMessage.Text = (string)GetLocalResourceObject("msgUserExists1") + Request["UserName"] + (string)GetLocalResourceObject("msgUserNotExists");
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
}
