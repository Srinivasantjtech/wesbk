using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using TradingBell.WebCat.EasyAsk;

public partial class UC_searchctrl : System.Web.UI.UserControl
{
    string breadcrumb = string.Empty;
    EasyAsk_WES EasyAsk = new EasyAsk_WES();
    ErrorHandler objErrorHandler = new ErrorHandler();
    public string strsortval = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HiddenField1.Value = "0";
            HiddenField2.Value = "0";
            hfcheckload.Value = "0";
            HFcnt.Value = "1";
            hforgurl.Value = HttpContext.Current.Request.Url.PathAndQuery.ToString();
            hfback.Value = string.Empty;
            hfbackdata.Value = string.Empty;
        }


        if (HttpContext.Current.Session["SortOrder_ps"] != null && HttpContext.Current.Session["SortOrder_ps"] != "")
        {
            if (HttpContext.Current.Session["SortOrder_ps"].ToString() != "SortOrder_ps")
            {
                HttpContext.Current.Session["SortOrder"] = null;
            }
        }


        if (HttpContext.Current.Session["SortOrder"] != null && HttpContext.Current.Session["SortOrder"] != "")
        {

            if (HttpContext.Current.Session["SortOrder"].ToString() == "Latest")
            {
                strsortval = "Latest";
            }
            else if (HttpContext.Current.Session["SortOrder"].ToString() == "ltoh")
            {
                strsortval = "Price Low To High";
            }
            else if (HttpContext.Current.Session["SortOrder"].ToString() == "htol")
            {
                strsortval = "Price High To Low";
            }
            else if (HttpContext.Current.Session["SortOrder"].ToString() == "relevance")
            {
                strsortval = "Relevance";
            }
            else if (HttpContext.Current.Session["SortOrder"].ToString() == "popularity")
            {
                strsortval = "Popular";
            }
            HttpContext.Current.Session["SortOrder_ps"] = "SortOrder_op";
        }
        else
        {
            strsortval = "Relevance";
            HttpContext.Current.Session["SortOrder_ps"] = "SortOrder_op";
        }
  }
   
    public string Bread_Crumbs()
    {

        breadcrumb = EasyAsk.GetBreadCrumb(Server.MapPath("Templates"));
        return breadcrumb;
    }

    public string Spell_Correction()
    {
        try
        {
            string SpellCorrection = "";
            if (HttpContext.Current.Session["Spell_Correction"] != null || HttpContext.Current.Session["Spell_Correction"] == "")
            {
                SpellCorrection = "<strong>" + HttpContext.Current.Session["Spell_Correction"].ToString() + "</strong>";
            }
            return SpellCorrection;
        }
      
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return string.Empty;
        }
    }
}

