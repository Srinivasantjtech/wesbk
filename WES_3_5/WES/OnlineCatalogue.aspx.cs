using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradingBell.WebCat.CommonServices;
namespace WES
{
    public partial class OnlineCatalogue : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HelperServices objhelper = new HelperServices();
            string pubpath = System.Configuration.ConfigurationManager.AppSettings["OnlineCatalogue"].ToString();
            string afterloginpubpath = System.Configuration.ConfigurationManager.AppSettings["ALOnlineCatalogue"].ToString();
            if ((Session["USER_ID"] == null) && (objhelper.CheckCredential() == false))
            {
               
                Response.Redirect(pubpath);
            }
            else if (Session["USER_ID"] == "")
            {
                Response.Redirect(pubpath);
            }
            else 
            {

                Response.Redirect(afterloginpubpath);  
            }
        }
    }
}