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
public partial class UC_topnext : System.Web.UI.UserControl
{
    HelperDB objHelperDB = new HelperDB();
    HelperServices objHelperServices = new HelperServices();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataSet dslink = new DataSet();
            dslink = (DataSet)objHelperDB.GetGenericDataDB("", "GET_BANNER_LINK", HelperDB.ReturnType.RTDataSet);
            //if(dslink!=null)
            //Session["dsblink"] = dslink;
            if (dslink != null && dslink.Tables[0].Rows.Count > 0)
            {
                if (dslink.Tables[0].Rows[0]["BANNER1_LINK_NEW"].ToString() != null)
                {
                    banner1a.HRef ="../" + dslink.Tables[0].Rows[0]["BANNER1_LINK_NEW"].ToString();
                    if (dslink.Tables[0].Rows[0]["ISNEWWINDOW_BANNER1"].ToString() == "True")
                        banner1a.Target = "_blank";
                }
                 if (dslink.Tables[0].Rows[0]["BANNER1_LINK_NEW"].ToString() == null)
                {
                    banner1a.HRef = "../" + "#";
                }
                 if (dslink.Tables[0].Rows[0]["BANNER2_LINK_NEW"].ToString() != null)
                {
                    banner2a.HRef = "../" + dslink.Tables[0].Rows[0]["BANNER2_LINK_NEW"].ToString();
                    if (dslink.Tables[0].Rows[0]["ISNEWWINDOW_BANNER2"].ToString() == "True")
                        banner2a.Target = "_blank";
                }
                 if (dslink.Tables[0].Rows[0]["BANNER2_LINK_NEW"].ToString() == null)
                {
                    banner2a.HRef = "../" + "#";
                }
                 if (dslink.Tables[0].Rows[0]["BANNER3_LINK_NEW"].ToString() != null)
                {
                    banner3a.HRef = "../" + dslink.Tables[0].Rows[0]["BANNER3_LINK_NEW"].ToString();
                    if (dslink.Tables[0].Rows[0]["ISNEWWINDOW_BANNER3"].ToString() == "True")
                        banner3a.Target = "_blank";
                }
                 if (dslink.Tables[0].Rows[0]["BANNER3_LINK_NEW"].ToString() == null)
                {
                    banner3a.HRef = "../" + "#";
                }
                 if (dslink.Tables[0].Rows[0]["BANNER4_LINK_NEW"].ToString() != null)
                {
                    banner4a.HRef = "../" + dslink.Tables[0].Rows[0]["BANNER4_LINK_NEW"].ToString();
                    if (dslink.Tables[0].Rows[0]["ISNEWWINDOW_BANNER4"].ToString() == "True")
                        banner4a.Target = "_blank";
                }
                 if (dslink.Tables[0].Rows[0]["BANNER4_LINK_NEW"].ToString() == null)
                {
                    banner4a.HRef = "../" + "#";
                }
            }
        }

    }
}
