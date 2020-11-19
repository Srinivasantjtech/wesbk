using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WES
{
    public partial class sessionabort : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Session.Abandon();

            string querystring = "username=" + Request.QueryString["username"].ToString() + "&password=" + Request.QueryString["password"] + "&login=" + Request.QueryString["login"];
             Response.Redirect("home.aspx?" + querystring, false);
           
        }
    }
}