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
using TradingBell.Common;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UI_Defulttop : System.Web.UI.UserControl
{
    HelperDB oHelper = new HelperDB();
    User oUser = new User();

    protected override void OnInit(EventArgs e)
    {
        Panel oPannel = (Panel)FindControl("pnlSearch");
        HtmlTable oHT = (HtmlTable)oPannel.FindControl("tblTop");
        HtmlTableRow oHTR = (HtmlTableRow)oHT.FindControl("rowImg");
        HtmlTableCell oHTC = (HtmlTableCell)oHTR.FindControl("cellImg");
        string ImgPath = @"Images" + oHelper.GetOptionValues("TOP LOGO").ToString();
        //oHTC.Attributes.Add("style", @"background-image:url(" + ImgPath + "); width: 862px; height:20px;");
        oHTC.Attributes.Add("style", @"background-image:url(Images/bg_header.jpg);");
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

         protected void cmdSearch_Click(object sender, ImageClickEventArgs e)
    {

        if(txtSearch.Text!="")

    {
        Response.Redirect("SearchResult.aspx?txtSearch=" + txtSearch.Text);
        }
        else
        {
            string str;
            str = "<script>";
            str = str + "alert('Search Text Can not be empty')";
            str = str + "</script>";
            Page.RegisterClientScriptBlock("validate", str);

            
            txtSearch.Focus();
        }
    }
   
    
    protected void lbtnSignOut_Click1(object sender, EventArgs e)
    {
{
        oUser.OnLineFlag(false, oHelper.CI(Session["USER_ID"]));
        Session.RemoveAll();
        Session["USER_ID"] = "";
        Response.Redirect("Login.aspx");
    }

    }
}
