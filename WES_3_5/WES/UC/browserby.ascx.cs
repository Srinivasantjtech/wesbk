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
public partial class UC_browserby : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
    //    if (Request.Form["__EVENTARGUMENT"] != null)
    //    {
    //        //if (Request.Form["__EVENTARGUMENT"].ToString().ToLower() == "browsebyproduct")
    //        //{
    //        //    Control ctl = LoadControl("~/UC/browsebyproduct.ascx");
    //        //    this.Controls.Add(ctl);
    //        //}
    //        //else 
    //        if (Request.Form["__EVENTARGUMENT"].ToString().ToLower() == "browsebybrand," || Request.Form["__EVENTARGUMENT"].ToString().ToLower() == "browsebybrand")
    //        {
    //            Control ctl = LoadControl("~/UC/browsebybrandWES.ascx");
    //                      //string script = "<script language='javascript'>\n";
    //            //script += "alert('hey there');\n";
    //            //script += "selectproduct();";
    //            //script += "</script>";
    //            //Page.RegisterStartupScript("StartUp", script);
            
    //            this.Controls.Add(ctl);
    //        }
    //        else
    //        {
    //            //if (Request.QueryString["byp"] !=null && Request.QueryString["byp"].ToString() == "1")
    //            //{
    //            //    Control ctl = LoadControl("~/UC/browsebyproduct.ascx");
    //            //    this.Controls.Add(ctl);
    //            //}
    //            //else 
    //            if (Request.QueryString["byp"] != null && (Request.QueryString["byp"].ToString() == "2" || Request.QueryString["byp"].ToString() == "3") && Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "")//&& Request.QueryString["cid"].ToString() == "WES210582")
    //            {
    //                Control ctl = LoadControl("~/UC/browsebybrandWES.ascx");
    //                this.Controls.Add(ctl);
    //            }
    //            else if (Request.QueryString["byp"] != null && (Request.QueryString["byp"].ToString() == "2" || Request.QueryString["byp"].ToString() == "3") && Request.QueryString["pcr"] != null && Request.QueryString["pcr"].ToString() != "")//&& Request.QueryString["pcr"].ToString() == "WES210582")                
    //            {
    //                Control ctl = LoadControl("~/UC/browsebybrandWES.ascx");
    //                this.Controls.Add(ctl);
    //            }
                
    //            //else
    //            //{
    //            //    Control ctl = LoadControl("~/UC/browsebyproduct.ascx");
    //            //    this.Controls.Add(ctl);
    //            //}
    //        }
    //    }
    //    else
    //    {
    //        if (Request.QueryString["byp"] != null)
    //        {
    //            if ((Request.QueryString["byp"].ToString() == "2" || Request.QueryString["byp"].ToString() == "3") && Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "")//&& Request.QueryString["cid"].ToString() == "WES210582")
    //            {
    //                Control ctl = LoadControl("~/UC/browsebybrandWES.ascx");
    //                this.Controls.Add(ctl);
    //            }
    //            else if ((Request.QueryString["byp"].ToString() == "2" || Request.QueryString["byp"].ToString() == "3") && Request.QueryString["pcr"] != null && Request.QueryString["pcr"].ToString() != "")// && Request.QueryString["pcr"].ToString() == "WES210582")
    //            {
    //                Control ctl = LoadControl("~/UC/browsebybrandWES.ascx");
    //                this.Controls.Add(ctl);
    //            }
    //            //else if (Request.QueryString["byp"].ToString() == "1")
    //            //{
    //            //    //Control ctl = LoadControl("~/UC/browsebybrand.ascx");
    //            //    //this.Controls.Add(ctl);
    //            // Control ctl = LoadControl("~/UC/browsebyproduct.ascx");
    //            //    this.Controls.Add(ctl);
    //            //}
    //        }
    //        //else
    //        //{
    //        //    Control ctl = LoadControl("~/UC/browsebyproduct.ascx");
    //        //    this.Controls.Add(ctl);
    //        //}
    //    }
    }
}
