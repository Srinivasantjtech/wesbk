using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CommonServices;

namespace WES.UC
{
    public partial class OnlineCatalogue_price : System.Web.UI.UserControl
    {
        Security objSecurity = new Security();
        ErrorHandler objErrorHandler = new ErrorHandler();
        const string EnDekey = "WAGNER@PayPalPAY@dm1n@123";
        protected void Page_Load(object sender, EventArgs e)
        {
           // btnOpenUrl.Attributes.Add("href", "");
            string encyptedstring = string.Empty;
            if (Session["USER_ID"] != null)
            {
               //encyptedstring = EncryptSP(Session["USER_ID"].ToString());
                encyptedstring = objSecurity.StringEnCrypt(Session["USER_ID"].ToString());
            }
            objErrorHandler.CreateLog(encyptedstring.ToString());
            string str = "?user_id=" + Session["USER_ID"].ToString();
            string url = HttpContext.Current.Request.Url.Authority.ToString();
            objErrorHandler.CreateLog(url.ToString());
            string copyURL = string.Format("http://" + url + "/OnlineCatalogue_price.aspx?UserId=" + encyptedstring);
            objErrorHandler.CreateLog(copyURL.ToString());
            txtUrl.Text = copyURL;
        }


        protected string EncryptSP(string ordid)
        {
            string enc = "";
            enc = objSecurity.StringEnCrypt(ordid, EnDekey);
            enc = objSecurity.StringEnCrypt(enc, EnDekey);
            enc = objSecurity.StringEnCrypt(enc, EnDekey);
            enc = objSecurity.StringEnCrypt(enc, EnDekey);
            enc = objSecurity.StringEnCrypt(enc, EnDekey);
            return HttpUtility.UrlEncode(enc);
        }
    }
}