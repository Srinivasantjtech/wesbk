using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CommonServices;
namespace WES
{
    public partial class InterLogin : System.Web.UI.Page
    {
        Security objSecurity = new Security();
        HelperServices objHelperServices = new HelperServices();
        protected void Page_Load(object sender, EventArgs e)
        {
            string strMsg = string.Empty;
          

            try
            {
               
                string username;
                string password;
                int UserID;


                username = Request.QueryString["username"];
                UserID = Convert.ToInt32(Request.QueryString["userid"]);

                password = Request.QueryString["password"];  

                Session["USER_NAME"] = username;
                Session["USER_ID"] = UserID;
                SetCookie(username, password);
         //   http://staging.wes.com.au/media/wes_sercure_files/pdf/wescat2017/PriceList_2017.pdf

                string url = "http://staging.wes.com.au/" + Request.QueryString["Queryurl"].Replace("wes_sercure_files/","") ;
               // objHelperServices.writelog(url);
                Response.Redirect(url);

            }
            catch
            { }

        }


        private void SetCookie(string UserName, string Password)
        {
          
                HttpCookie LoginInfoCookie = new HttpCookie("LoginInfo");
                LoginInfoCookie["UserName"] = objSecurity.StringEnCrypt(UserName);
                LoginInfoCookie["Password"] = objSecurity.StringEnCrypt(Password);
                LoginInfoCookie["Expires"] = DateTime.Now.AddDays(1).ToString();
                LoginInfoCookie["Login"] = objSecurity.StringEnCrypt("True");
                LoginInfoCookie.Expires = DateTime.Now.AddDays(1);
                HttpContext.Current.Response.AppendCookie(LoginInfoCookie);
          

        }
    }
}