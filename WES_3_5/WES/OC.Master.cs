using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.IO;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;

namespace WES
{
    public partial class OC : System.Web.UI.MasterPage
    {
        HelperServices objHelperServices = new HelperServices();
        protected void Page_Load(object sender, EventArgs e)
        {
            string requrl = Request.Url.ToString().ToLower();
            loadheader();
            loadmaincontent();
            loadfooter();
        }
        private void loadheader()
        {
            FileStream fileStream = new FileStream(Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString() + "\\homepage\\header.st"), FileMode.Open);
            System.IO.StreamReader streamWriter = new System.IO.StreamReader(fileStream);
            string dataString = streamWriter.ReadToEnd();
            streamWriter.Close();
            fileStream.Close();
            string[] str = dataString.Split('$');
            for (int strc = 1; strc < str.Length; strc = strc + 2)
            {
                if (str[strc].ToUpper() == "TOP")
                {
                    if (Session["USER_ID"] == null || Session["USER_ID"].ToString() == "")
                    {
                        str[strc] = "toplog";
                    }
                }

                Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");

                header.Controls.Add(ctl);
            }
        }
        private void loadmaincontent()
        {
            FileStream fileStream = new FileStream(Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString() + "\\homepage\\maincontent.st"), FileMode.Open);
            System.IO.StreamReader streamWriter = new System.IO.StreamReader(fileStream);
            string dataString = streamWriter.ReadToEnd();
            streamWriter.Close();
            fileStream.Close();
            string[] str = dataString.Split('$');
            string requrllwr = Request.Url.ToString().ToLower();
            string requrlupr = Request.Url.ToString().ToUpper();
            if (!requrllwr.Contains("requestlogin"))
            {
                for (int strc = 1; strc < str.Length; strc = strc + 2)
                {
                    // comment by palani
                    //if (requrlupr.Contains(str[strc].ToUpper() + ".as".ToUpper()))
                    if (requrlupr.Contains(str[strc].ToUpper() + ".AS"))
                    {
                        Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");
                        maincontent.Controls.Add(ctl);
                    }
                }


            }
        }
        private void loadfooter()
        {
            FileStream fileStream = new FileStream(Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString() + "\\homepage\\footer.st"), FileMode.Open);
            System.IO.StreamReader streamWriter = new System.IO.StreamReader(fileStream);
            string dataString = streamWriter.ReadToEnd();
            streamWriter.Close();
            fileStream.Close();
            string[] str = dataString.Split('$');
            for (int strc = 1; strc < str.Length; strc = strc + 2)
            {
                Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");

                footer.Controls.Add(ctl);
            }
        }
    }
}