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
public partial class UserControl_Top : System.Web.UI.UserControl
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
        oHTC.Attributes.Add("style", @"background-image:url(" + ImgPath + "); width: 862px; height:20px;");
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //Helper oHelper = new Helper();
        //txtSearch.Attributes.Add("onKeyUp", "return btnKeyPress(event,this);");        
        lbtncompany.Text =  oHelper.GetOptionValues("COMPANY URL").ToString();
        lblPhone.Text =  oHelper.GetOptionValues("COMPANY PHONE TEXT").ToString();
        Page.Title = oHelper.GetOptionValues("BROWSER TITLE").ToString();
        if (Session["USER_ID"] == null || Session["USER_ID"].ToString() == "")
        {
            lbtnSignOut.Visible = false;
            imgArrow.Visible = false;
            //imgLogin.Visible = false;
            lblUser.Text = ".";
        }
        else if (Session["USER_ID"] != null || Session["USER_ID"].ToString() != "")
        {
            lbtnSignOut.Visible = true;
            imgArrow.Visible = true;
            //imgLogin.Visible = true;
            lblUser.Text = (string)GetGlobalResourceObject("Top", "WelcomeNote") + " " + oUser.GetUserName(oHelper.CI(Session["USER_ID"]));
            lblUser.CssClass = "HeadLink";
        }
        //Helper oHelper = new Helper();
        if (oHelper.GetOptionValues("ECOMMERCEENABLED").ToString() == "NO")
        {
            imgArrow2.Visible = false;
            lborderstatus.Visible = false;
            imgArrow4.Visible = false;
            lbViewCart.Visible = false;
            imgArrow5.Visible = false;
            lbViewWishList.Visible = false;
        }
        if (IsPostBack)
        {
            if (txtSearch.Text != "")
            {
                Response.Redirect("SearchResult.aspx?txtSearch=" + txtSearch.Text);
            }
        }
    }

    //protected void cmdSearch_Click(object sender, ImageClickEventArgs e)
    //{

    //    if (txtSearch.Text != "")
    //    {
    //        Response.Redirect("SearchResult.aspx?txtSearch=" + txtSearch.Text);
    //    }
    //    else
    //    {
    //        string str;
    //        str = "<script>";
    //        str = str + "alert('Search Text Can not be empty')";
    //        str = str + "</script>";
    //        Page.RegisterClientScriptBlock("Validate", str);
    //        txtSearch.Focus();
    //    }
    //}

    protected void lbtnSignOut_Click(object sender, EventArgs e)
    {
        oUser.OnLineFlag(false, oHelper.CI(Session["USER_ID"]));
        Session.RemoveAll();
        Session.Clear();
        Session.Abandon();
        Session["USER_ID"] = "";
        Response.Redirect("Login.aspx");
    }
}
