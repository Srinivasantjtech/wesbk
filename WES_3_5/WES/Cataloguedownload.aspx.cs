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
public partial class Cataloguedownload : System.Web.UI.Page
{

   
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);

        //if (!IsPostBack)
        //{
        //    if (Request.QueryString["ActionResult"] != null && Request.QueryString["ActionResult"].ToString() == "CATALOGUE")
        //    {
        //        //multiTabs.ActiveViewIndex = 0;
        //        ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:__doPostBack('ctl00$maincontent$menuTabs','0');", true);
        //    }
        //    else if (Request.QueryString["ActionResult"] != null && Request.QueryString["ActionResult"].ToString() == "NEWS")
        //    {
        //        //multiTabs.ActiveViewIndex = 1;
        //        ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:__doPostBack('ctl00$maincontent$menuTabs','1');",true);
        //    }
        //    else if (Request.QueryString["ActionResult"] != null && Request.QueryString["ActionResult"].ToString() == "FORMS")
        //    {
        //        //multiTabs.ActiveViewIndex = 1;
        //        ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:__doPostBack('ctl00$maincontent$menuTabs','2');", true);
        //    }
        //}
      

    }

    //protected void menuTabs_MenuItemClick(object sender, MenuEventArgs e)
    //{
    //    multiTabs.ActiveViewIndex = Int32.Parse(menuTabs.SelectedValue);
    //}
}
