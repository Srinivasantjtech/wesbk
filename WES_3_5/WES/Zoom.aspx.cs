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
using TradingBell.WebCat.CommonServices;
public partial class Zoom : System.Web.UI.Page
{
    //public int chk;
    protected void Page_Load(object sender, EventArgs e)
    {
        HelperServices objHelperServices = new HelperServices();
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
        //if (chk == 0)
        //{
            imgPF.ImageUrl = Request["ImgUrl"].ToString();
        //}   
            if (Session["USER_NAME"] == null || Session["USER_NAME"] == "")
            {
                Response.Redirect("Login.aspx",false);
            }
       
    }
    
}
