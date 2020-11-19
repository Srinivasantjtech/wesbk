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
public partial class UC_searchby : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Request.QueryString["byp"] != null && Request.QueryString["byp"].ToString() != "" && Request.QueryString["byp"].ToString() == "2")
        //{
        //    if (Request.QueryString["qf"] != null && Request.QueryString["qf"].ToString() != "" && Request.QueryString["qf"].ToString() == "1")
        //    {
        //        Control ctl = LoadControl("~/search/searchbybrandFamily.ascx");                
        //        this.Controls.Add(ctl);
        //    }
        //    else 
        //    {
        //        Control ctl = LoadControl("~/search/searchbybrand.ascx");
        //        this.Controls.Add(ctl);
        //    }

        //}
        
    }
}
