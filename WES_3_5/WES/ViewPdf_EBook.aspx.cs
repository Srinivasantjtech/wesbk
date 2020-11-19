using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradingBell.WebCat.CommonServices;
namespace WES
{
    public partial class ViewPdf_EBook : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HelperServices objhelper=new HelperServices();
  
            if ((Session["USER_ID"] != null)||objhelper.CheckCredential()==true)
            
            {
                if (Session["USER_ID"].ToString() != "")
                {
                    string filename=Request.QueryString["FilePath"].ToString();
                    if (filename.ToLower().Contains("www") == false)
                    {
                        //Response.Redirect("attachments" + filename.Replace("/", "\\")); 

                       
                      string fname = "attachments" + filename.Replace("/", "\\");
                      //string fname ="attachments\\media\\wesnews\\WN512IPA\\01A - Power.pdf";
                      //ebook.Attributes.Add("src", fname);
                      ebook.Visible = true; 
                    }
                    else
                    {
                        Response.Redirect(filename); 
                    }
                }

            }
            
        }
    }
}