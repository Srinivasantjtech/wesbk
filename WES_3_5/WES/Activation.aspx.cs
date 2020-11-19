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
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class Activation : System.Web.UI.Page
{
    #region "Declarations"
   // int OrderId = 0;
    HelperServices objHelperServices = new HelperServices();
    
    ErrorHandler objErrorHandler = new ErrorHandler();
    UserServices objUserServices = new UserServices();
    Security objSecurity = new Security();
    
    
    
    
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string reg_id = "";
            int i=0;
            if( Request["id"]!=null)
            {
                reg_id = Request["id"].ToString();
                reg_id= objSecurity.StringDeCrypt(reg_id);
                i = objUserServices.SetUserRole(reg_id, 2);
                if (i > 0)
                {
                    Session["USER_ROLE"] = objUserServices.GetRole(Convert.ToInt32(reg_id));
                    Response.Redirect("ConfirmMessage.aspx?Result=ACTIVATED");
                }
                else
                    Response.Redirect("ConfirmMessage.aspx?Result=ACTIVATION_FAILED");
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
        }
    }
  
}
