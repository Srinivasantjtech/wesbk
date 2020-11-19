using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using TradingBell.WebCat.CatalogDB ;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;

public partial class MasterPage : System.Web.UI.MasterPage
{
   
    HelperServices objHelperServices = new HelperServices();

    protected void Page_Load(object sender, EventArgs e)
    {
      
        loadheader();
       // loadleftnav();
        loadmaincontentblock();
       // loadrightnav();
        loadfooter();
        //if (Session["Notification"] == null)
        //{
        //    Page.ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript: checksubscription(); ", true);
        //    Session["Notification"] = "Yes";
        //}

    }
    private void loadheader()
    {
        FileStream fileStream = new FileStream(Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString() + "\\home\\header.st"), FileMode.Open);
        System.IO.StreamReader streamWriter = new System.IO.StreamReader(fileStream);
        string dataString = streamWriter.ReadToEnd();
        streamWriter.Close();
        fileStream.Close();
        string[] str = dataString.Split('$');
        for (int strc = 1; strc < str.Length; strc = strc + 2)
        {
            if (str[strc].ToUpper() == "TOP")
            {
                if (Session["USER_ID"].ToString() == null || Session["USER_ID"].ToString() == "")
                {
                    str[strc] = "toplog";
                }
            }
            Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");

            header.Controls.Add(ctl);
        }
    }

    private void loadleftnav()
    {
        FileStream fileStream = new FileStream(Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString() + "\\home\\leftnav.st"), FileMode.Open);
        System.IO.StreamReader streamWriter = new System.IO.StreamReader(fileStream);
        string dataString = streamWriter.ReadToEnd();
        streamWriter.Close();
        fileStream.Close();
        string[] str = dataString.Split('$');
        for (int strc = 1; strc < str.Length; strc=strc+2){
            if ((str[strc] != "browsebycategory"))
            {
                Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");
            }

            
           // leftnavigator.Controls.Add(ctl);
        }
    }

    private void loadmaincontentblock()
    {
        FileStream fileStream = new FileStream(Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString() + "\\home\\maincontentblock.st"), FileMode.Open);
        System.IO.StreamReader streamWriter = new System.IO.StreamReader(fileStream);
        string dataString = streamWriter.ReadToEnd();
        streamWriter.Close();
        fileStream.Close();
        string[] str = dataString.Split('$');
        for (int strc = 1; strc < str.Length; strc = strc + 2)
        {
            Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");

            //maincontentblock.Controls.Add(ctl);
        }
    }

    private void loadrightnav()
    {
        FileStream fileStream = new FileStream(Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString() + "\\home\\rightnav.st"), FileMode.Open);
        System.IO.StreamReader streamWriter = new System.IO.StreamReader(fileStream);
        string dataString = streamWriter.ReadToEnd();
        streamWriter.Close();
        fileStream.Close();
        string[] str = dataString.Split('$');
        for (int strc = 1; strc < str.Length; strc = strc + 2)
        {
            Control ctl = LoadControl("~/UC/" + str[strc] + ".ascx");

            //rightnavigator.Controls.Add(ctl);
        }
    }

    private void loadfooter()
    {
        FileStream fileStream = new FileStream(Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString() + "\\home\\footer.st"), FileMode.Open);
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
