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
//using System.Diagnostics;
public partial class Mainpage : System.Web.UI.MasterPage
{
   
    HelperServices objHelperServices = new HelperServices();
   
    protected void Page_Load(object sender, EventArgs e)
    {
        string requrl = Request.Url.ToString().ToLower();
        loadheader();
        loadmaincontent();
        if (!(requrl.Contains("resetchangepassword.aspx")) && !(requrl.Contains("login.aspx")))
        {
            loadleftnav();
            if (!(requrl.Contains("categorylist.aspx")) && !(requrl.Contains("bybrand.aspx")) && !(requrl.Contains("productdetails.aspx")))
            {
                loadrightnav();
            }
        }
        loadfooter();
        //if (Session["Notification"] == null)
        //{
        //    Page.ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript: checksubscription(); ", true);
        //    Session["Notification"] = "Yes";
        //}
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

    private void loadleftnav()
    {
        string requrlln = Request.Url.ToString().ToLower();
        if (!requrlln.Contains("orderhistory") && !requrlln.Contains("byproduct"))
        {
            FileStream fileStream = new FileStream(Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString() + "\\homepage\\leftnav.st"), FileMode.Open);
            System.IO.StreamReader streamWriter = new System.IO.StreamReader(fileStream);
            string dataString = streamWriter.ReadToEnd();
            streamWriter.Close();
            fileStream.Close();
            if (requrlln.Contains("categorylist.aspx"))
            {
                dataString = dataString.Replace("$newproductsnav$", "");
            }
            if (requrlln.Contains("paymenthistory.aspx")
                || requrlln.Contains("billinfo.aspx")
                || requrlln.Contains("billinfosp.aspx")
                || requrlln.Contains("paysp.aspx")
                || requrlln.Contains("payonlinecc.aspx")
                || requrlln.Contains("productfinder.aspx")
                  || requrlln.Contains("payonline_international.aspx")
                 || requrlln.Contains("payonline_banktrasfer.aspx")
                )
            {
                dataString = dataString.Replace("$newproductsnav$", "");
                dataString = dataString.Replace("$newproductlognav$", "");
            }
            string[] str = dataString.Split('$');
            string strtename = string.Empty;
            string strtelwr = string.Empty;
            for (int strc = 1; strc < str.Length; strc = strc + 2)
            {
                strtename = string.Empty;
                strtename = str[strc].ToUpper();
                strtelwr = string.Empty;
                strtelwr = str[strc];
                if (strtename == "NEWPRODUCTSNAV" && !requrlln.Contains("categorylist.aspx"))
                {
                    if (Session["USER_ID"] == null || Session["USER_ID"].ToString() == "")
                    {
                        //str[strc] = "NEWPRODUCTLOGNAV";
                        strtelwr = "NEWPRODUCTLOGNAV";

                    }
                    
                }
                else if (strtename == "NEWPRODUCTLOGNAV")
                {

                    if (strtename == "NEWPRODUCTLOGNAV" && !requrlln.Contains("payonlinecc.aspx") && !requrlln.Contains("login.aspx") && !requrlln.Contains("createanaccount.aspx") && !requrlln.Contains("dealerregistration.aspx") && !requrlln.Contains("existcustomerregistration.aspx") && !requrlln.Contains("categorylist.aspx") && !requrlln.Contains("retailerregistration.aspx"))
                    {
                        Control ctl = LoadControl("~/UC/" + strtelwr + ".ascx");
                        leftnav.Controls.Add(ctl);
                    }


                }
                else if (strtename == "NEWPRODUCTLOGNAV" && !requrlln.Contains("categorylist.aspx"))
                {
                    Control ctl = LoadControl("~/UC/" + strtelwr + ".ascx");
                  leftnav.Controls.Add(ctl);
                }
                else if (strtelwr != "maincategory" && strtelwr != "browsebybrandWES" && strtelwr != "browsebyproductWES")
                {
                    Control ctl = LoadControl("~/UC/" + strtelwr + ".ascx");
                   leftnav.Controls.Add(ctl);
                }
                else if (strtelwr == "maincategory" && (requrlln.Contains("product_list.aspx")))
                {
                    Control ctl = LoadControl("~/UC/" + strtelwr + ".ascx");
                   leftnav.Controls.Add(ctl);
                }
                else if (strtelwr == "maincategory" && (requrlln.Contains("productdetails.aspx")))
                {
                    Control ctl = LoadControl("~/UC/" + strtelwr + ".ascx");
                    leftnav.Controls.Add(ctl);
                }
                else if (strtelwr == "maincategory" && (requrlln.Contains("family.aspx")))
                {
                    Control ctl = LoadControl("~/UC/" + strtelwr + ".ascx");
                    leftnav.Controls.Add(ctl);
                }
                else if (strtelwr == "maincategory" && (requrlln.Contains("bybrand.aspx")))
                {
                    Control ctl = LoadControl("~/UC/" + strtelwr + ".ascx");
                    leftnav.Controls.Add(ctl);
                }
                else if (strtelwr == "maincategory" && (requrlln.Contains("byproduct.aspx")))
                {
                    Control ctl = LoadControl("~/UC/" + strtelwr + ".ascx");
                   leftnav.Controls.Add(ctl);
                }
                else if (strtelwr == "maincategory" && (requrlln.Contains("categorylist.aspx")))
                {
                    Control ctl = LoadControl("~/UC/" + strtelwr + ".ascx");
                    leftnav.Controls.Add(ctl);
                }
                else if (strtelwr == "maincategory" && (requrlln.Contains("powersearch.aspx")))
                {
                    Control ctl = LoadControl("~/UC/" + strtelwr + ".ascx");
                   leftnav.Controls.Add(ctl);
                }
               
            }
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
        if (!requrllwr.Contains("requestlogin")  && !requrllwr.Contains("byproduct"))
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

    private void loadrightnav()
    {
        string requrl = Request.Url.ToString().ToLower();
        if (!requrl.Contains("bulkorder") && !requrl.Contains("orderdetail") && !requrl.Contains("orderhistory") && !requrl.Contains("pendingorder") && !requrl.Contains("byproduct"))
        {
            FileStream fileStream = new FileStream(Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString() + "\\homepage\\rightnav.st"), FileMode.Open);
            System.IO.StreamReader streamWriter = new System.IO.StreamReader(fileStream);
            string dataString = streamWriter.ReadToEnd();
            streamWriter.Close();
            fileStream.Close();
            if (requrl.Contains("categorylist.aspx"))     //|| Request.Url.ToString().ToLower().Contains("productdetails.aspx") || Request.Url.ToString().ToLower().Contains("product_list.aspx")
            {
                dataString = dataString.Replace("$quickbuy$", "$newproductsnav$");
                
               
                if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "")
                {
                    dataString = dataString.Replace("$advertisement$", "");
                }
                dataString = dataString.Replace("$announcements$", "");
            }
            if (requrl.Contains("productdetails.aspx") || requrl.Contains("shipping.aspx")
                || requrl.Contains("payonlinecc.aspx") || requrl.Contains("paymenthistory.aspx")
                || requrl.Contains("ordertemplate.aspx") || requrl.Contains("ordertemplatelist.aspx")
                || requrl.Contains("billinfo.aspx")
                || requrl.Contains("billinfosp.aspx")
                || requrl.Contains("paysp.aspx")
                || requrl.Contains("productfinder.aspx")
                || requrl.Contains("payonline_international.aspx")
                 || requrl.Contains("payonline_banktrasfer.aspx")
                ) 
            {
                dataString = dataString.Replace("$advertisement$", "");
            }
            string[] str = dataString.Split('$');
            string strtempname = string.Empty;
            for (int strc = 1; strc < str.Length; strc = strc + 2)
            {
                strtempname = string.Empty;
                strtempname = str[strc];
                if (!(strtempname.Contains("quickbuy") && requrl.Contains("bybrand.aspx")))
                    if (!requrl.Contains("powersearch.aspx"))
                    {

                        if (!(strtempname == "browserby" && requrl.Contains("productdetails")) && strtempname != "browsebycategory" && strtempname != "browsebybrandWES" && strtempname != "browsebyproductWES" && (!(str[strc] == "quickbuy" && requrl.Contains("shipping"))) && (!(strtempname == "quickbuy" && requrl.Contains("orderdetails"))) && (!(strtempname == "announcements" && requrl.Contains("shipping"))) && (!(strtempname == "announcements" && requrl.Contains("bulkorder"))))
                        {
                            if (requrl.Contains("bybrand.aspx"))
                            {
                                if (strtempname != "browserby" && strtempname != "advertisement")
                                {

                                    Control ctl = LoadControl("~/UC/" + strtempname + ".ascx");
                             
                                        rightnav.Controls.Add(ctl);
                                    
                                }

                                
                            }
                            else
                            {
                                if (strtempname != "browserby")
                                {

                                    if (strtempname.ToUpper() == "browserby" && !requrl.Contains("login.aspx"))
                                    {
                                        Control ctl = LoadControl("~/UC/" + strtempname + ".ascx");
                                        rightnav.Controls.Add(ctl);
                                    }

                                    //if (strtempname == "advertisement"
                                    if (strtempname == "specproduct"
                                        && !requrl.Contains("login.aspx")
                                        && !requrl.Contains("categorylist.aspx")
                                        && !requrl.Contains("product_list.aspx")
                                          && !requrl.Contains("productspl.aspx")
                                         &&  !requrl.Contains("billinfo.aspx")
               && !requrl.Contains("billinfosp.aspx")
               && !requrl.Contains("paysp.aspx")
              && !requrl.Contains("productfinder.aspx")
               && !requrl.Contains("payonline_international.aspx")
                 && !requrl.Contains("payonline_banktrasfer.aspx")
                                        
                                        //Added by indu Defect:633
                                        && !requrl.Contains("cataloguedownload.aspx")) //&& !Request.Url.ToString().ToLower().Contains("product_list.aspx")
                                    {
                                        Control ctl = LoadControl("~/UC/" + strtempname + ".ascx");
                                        rightnav.Controls.Add(ctl);
                                    }
                                   
                                }
                            }

                        }
                        else if (strtempname == "browsebybrandWES" && ((requrl.Contains("product_list.aspx")) || (requrl.Contains("bybrand.aspx"))) && !requrl.Contains("bulkorder"))
                        {
                            Control ctl = LoadControl("~/UC/" + strtempname + ".ascx");

                            rightnav.Controls.Add(ctl);
                        }
                        else if (strtempname == "browsebyproductWES" && ((requrl.Contains("product_list.aspx")) || (requrl.Contains("byproduct.aspx"))) && !requrl.Contains("bulkorder"))
                        {
                            Control ctl = LoadControl("~/UC/" + strtempname + ".ascx");

                            rightnav.Controls.Add(ctl);
                        }
                        else if (requrl.Contains("shipping_info.aspx") && !requrl.Contains("bulkorder"))
                        {
                            Control ctl = LoadControl("~/UC/" + strtempname + ".ascx");
                            rightnav.Controls.Add(ctl);
                        }
                      
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
