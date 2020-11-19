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
using System.Diagnostics;     
public partial class family : System.Web.UI.Page
{
    
    HelperServices objHelperServices = new HelperServices();
    GetmetadataFromEA objgetmetadata = new GetmetadataFromEA();
    string stlistprod = string.Empty;
    string stcategory = string.Empty;
    string stcategorylisttitle = string.Empty;
    string stcategorylistkey = string.Empty;
    string sfamily = string.Empty;
    string stype = string.Empty;

    string sbrand = string.Empty;
    string smodel = string.Empty;
    string ssize = string.Empty;
    string spower = string.Empty;
    string stitle = string.Empty;
    string skeyword = string.Empty;
    //  Stopwatch sw = new Stopwatch();
    protected void Page_Load(object sender, EventArgs e)
    {
      //  ErrorHandler objErrorHandler = new ErrorHandler();
       // sw.Start();
        Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();        
       // Session["PageUrl"] = "Family.aspx?fid=" + Request["fid"];
       // sw.Stop();
       // Console.WriteLine("Elapsed={0}", sw.Elapsed);

      //  StackTrace st = new StackTrace();
       // StackFrame sf = st.GetFrame(0);

        //objErrorHandler.ExeTimelog = sf.GetMethod().Name + "," + sw.Elapsed.TotalSeconds.ToString();
        // objErrorHandler.ExeTimelog = sf.GetMethod().Name + "," + sw.Elapsed;
       // objErrorHandler.CreateTimeLog(); 
    }

    protected void Page_SaveStateComplete(object sender, EventArgs e)
    {

        try
        {

           // string Fname = "";
           // string CFname = "";
            string urlstring = string.Empty;
            //Page.Title = "Cellink";


            if (Session["BreadCrumbDS"] != null)
            {

                DataSet ds = (DataSet)Session["BreadCrumbDS"];
                string dstblrowitype = string.Empty;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    dstblrowitype = ds.Tables[0].Rows[i]["ItemType"].ToString().ToLower();
                    if (dstblrowitype == "category")
                    {
                        if (i != 0)
                        {

                            stcategory = ds.Tables[0].Rows[i]["Itemvalue"].ToString();

                            stcategory = objgetmetadata.Replace_SpecialChar(stcategory);
                            if (stcategorylisttitle == string.Empty)
                            {
                                stcategorylisttitle = stcategory;
                                h3_2.InnerText = stcategory;
                            }
                            else
                            {
                                h3_3.InnerText = stcategory;

                            }
                        }
                        if (i == 0)
                        {
                            h3_1.InnerText = ds.Tables[0].Rows[0]["Itemvalue"].ToString();

                        }
                    }
                    else if (dstblrowitype == "family")
                    {

                        sfamily = ds.Tables[0].Rows[i]["FamilyName"].ToString();

                        h1.InnerText = objgetmetadata.Replace_SpecialChar(sfamily);

                    }


                }

                string title_key = objgetmetadata.FetchData(ds);
                string[] StrValues = title_key.Split(new string[] { "|" }, StringSplitOptions.None);
                stitle = StrValues[0];
                skeyword = StrValues[1];
                urlstring = StrValues[2];
                //Page.Title = "Cellink" + "-" + stitle.Replace("<ars>g</ars>", "-").ToString();
                Page.Title = stitle.Replace("<ars>g</ars>", "-").ToString();
                string skeywordRe = objgetmetadata.Replace_SpecialChar(skeyword);
                if (HttpContext.Current.Session["LHSAttributes"] != null)
                {
                    DataSet dsproductattr = (DataSet)HttpContext.Current.Session["LHSAttributes"];
                    if (dsproductattr != null)
                    {
                        if (dsproductattr.Tables.Contains("Product Tags"))
                        {
                            if (dsproductattr.Tables["Product Tags"].Rows.Count > 0)
                            {
                                string strkeyword1 = objHelperServices.MetaTagProductkeyword(dsproductattr.Tables["Product Tags"]);
                                skeywordRe = skeywordRe + "," + strkeyword1;
                            }

                        }
                    }
                }
                Page.MetaKeywords = skeywordRe;
                string shtdesc = string.Empty;
                string desc = string.Empty;
                if (Session["FamilyProduct"] != null)
                {
                    DataSet dsdesc = (DataSet)Session["FamilyProduct"];

                    DataTable dtprod = new DataTable();

                    dtprod = dsdesc.Tables[0];
                    string expression = "attribute_Name in ('Short Description','Description','Features','Descriptions','prod_dsc','Notes','Note') ";
                    DataRow[] foundRows;
                    foundRows = dtprod.Select(expression);
                    string frattr_name = string.Empty;
                    for (int j = 0; j < foundRows.Length; j++)
                    {
                        frattr_name = foundRows[j]["attribute_Name"].ToString();
                        if (j == 0)
                        {
                            if (frattr_name == "Short Description")
                            {
                                string h2desc = foundRows[j]["STRING_VALUE"].ToString();
                                h2.InnerText = objgetmetadata.Replace_SpecialChar(h2desc);
                            }
                            desc = foundRows[j]["STRING_VALUE"].ToString();

                        }
                        else
                        {
                            if ((h2.InnerText == string.Empty) && (frattr_name == "Short Description"))
                            {
                                string h2desc = foundRows[j]["STRING_VALUE"].ToString();

                                h2.InnerText = objgetmetadata.Replace_SpecialChar(h2desc);
                            }
                            if (foundRows[j - 1]["STRING_VALUE"].ToString() != foundRows[j]["STRING_VALUE"].ToString())
                            {
                                if (desc != string.Empty)
                                {
                                    desc = desc + ". " + foundRows[j]["STRING_VALUE"].ToString();
                                }
                                else
                                {
                                    desc = foundRows[j]["STRING_VALUE"].ToString();
                                }
                            }
                        }

                    }

                    Page.MetaDescription = objgetmetadata.Replace_SpecialChar(desc);


                }
                if (h2.InnerText == string.Empty)
                {
                    h2.InnerText = Page.MetaDescription;
                }


                if (h3_2.InnerText == "")
                {

                    h3_2.Visible = false;
                }
                if (h3_3.InnerText == "")
                {

                    h3_3.Visible = false;
                }
                if (h2.InnerText == string.Empty)
                {

                    h2.Visible = false;
                }
            }
            //stlistprod = objHelperServices.URLRewriteToAddressBar("Family.aspx?" ,urlstring.ToUpper(), Request.Url.PathAndQuery.ToString(), Server.MapPath("URL_rewrite_Family.ini"),false);

            //if (Page.Request.RawUrl.ToString().Contains("="))
            //{
            //    stlistprod = objHelperServices.URLRewriteToAddressBar("Family.aspx?" + urlstring.ToUpper(), Request.Url.PathAndQuery.ToString(), Server.MapPath("URL_rewrite_Family.ini"));
            //    ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "processAjaxData( '" + stlistprod + "');", true);
            //}
            //else
            //{
            //  string[]  PARENTFAMILY= Page.Request.RawUrl.Split(new string[] { "?" }, StringSplitOptions.None);
            //  Session["PARENTFAMILY"] = PARENTFAMILY[1];

            //}
            string[] PARENTFAMILY = Page.Request.RawUrl.Split(new string[] { "?" }, StringSplitOptions.None);
            Session["PARENTFAMILY"] = PARENTFAMILY[1];
        }
        catch
        { }







    }

    [System.Web.Services.WebMethod]
    public static string Strval(string WithPrice, string Details, string cellname, string fid,string path)
    {
        ErrorHandler objErrorHandler = new ErrorHandler();
        try
        {
            UC_family prod = new UC_family();
            string str = prod.Construct_ST(WithPrice, Details, "cell", fid,path);
           string imgpath=HttpContext.Current.Server.MapPath("") ;
           str = str.Replace(imgpath, "");    
            return str;
        }
        catch (Exception ex)
        {
            objErrorHandler.CreateLog(ex.ToString());
            return ex.ToString();
        }

    }

    [System.Web.Services.WebMethod]
    public static string SendAskQuestionMail(string fromid, string fname, string phone, string qustion, string familyName)
    {
        ErrorHandler objErrorHandler = new ErrorHandler();
        try
        {
            HelperServices objHelperServices = new HelperServices();
            UserServices objUserServices = new UserServices();
            UserServices.UserInfo oUserinfo = new UserServices.UserInfo();
            string _UserID = "0";
            System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();


            MessageObj.From = new System.Net.Mail.MailAddress(fromid);
            MessageObj.To.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());

            //MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
            //MessageObj.To.Add(fromid);

            string message = string.Empty;
            MessageObj.Subject = "WESOnline-Form-Product-Enquiry";
            MessageObj.IsBodyHtml = true;
            message = message + "<tr><td style=\"width:112px\">Family Name </td><td>&nbsp;</td><td>" + familyName + "</td></tr>";
            message = message + "<tr><td>Family Link </td><td>&nbsp;</td><td><a href='" + HttpContext.Current.Request.UrlReferrer.OriginalString + "'>" + HttpContext.Current.Request.UrlReferrer.OriginalString + " </a> </td></tr>";
            message = message + "<tr><td></td><td>&nbsp;</td><td></td></tr>";
            message = message + "<tr><td>Full Name </td><td>&nbsp;</td><td>" + fname + "</td></tr>";
            message = message + "<tr><td>Email id </td><td>&nbsp;</td><td>" + fromid + "</td></tr>";
            message = message + "<tr><td>Phone </td><td>&nbsp;</td><td>" + phone + " </td></tr>";
            message = message + "<tr><td>Question</td><td>&nbsp;</td><td>" + qustion + "</td></tr>";
            message = message + "<tr><td></td><Br/><td>&nbsp;</td><td><Br/></td></tr>";
            if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
            {
                _UserID = HttpContext.Current.Session["USER_ID"].ToString();
                oUserinfo = objUserServices.GetUserInfo(Convert.ToInt32(_UserID));
                if (oUserinfo.CUSTOMER_TYPE.ToString().Equals("Retailer"))
                {
                    message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Retail" + "</td></tr>";
                }
                else if (oUserinfo.CUSTOMER_TYPE.ToString().Equals("Dealer"))
                {
                    message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Trade" + "</td></tr>";
                    message = message + "<tr><td>WES Customer No</td><td>&nbsp;</td><td>" + oUserinfo.CompanyID + "</td></tr>";
                }
                else
                    message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Retail" + "</td></tr>";

            }
            else
            {
                message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Retail" + "</td></tr>";
            }

            MessageObj.Body = "<html><body><table>" + message + "</table>" + " <font color=\"red\"></font></body></html>";
            System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
            smtpclient.UseDefaultCredentials = false;
            smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
            smtpclient.Send(MessageObj);
            return "1".ToString();
        }
        catch (Exception ex)
        {
            objErrorHandler.CreateLog(ex.ToString());
            return "-1".ToString();
        }
    }

    [System.Web.Services.WebMethod]
    public static string SendBulkBuyProjectPricing(string productcode, string fullname, string qty, string fromid, string deliverytime, string phone, string targetprice, string notesandaddtionalinfo, string familyName)
    {
        ErrorHandler objErrorHandler = new ErrorHandler();
        try
        {
            HelperServices objHelperServices = new HelperServices();
            UserServices objUserServices = new UserServices();
            UserServices.UserInfo oUserinfo = new UserServices.UserInfo();
            System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
            string _UserID = "0";

            MessageObj.From = new System.Net.Mail.MailAddress(fromid);
            MessageObj.To.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());

            //MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
            //MessageObj.To.Add(fromid);

            string message = string.Empty;
            MessageObj.Subject = "WESOnline-Form-BulkBuy-Enquiry";
            MessageObj.IsBodyHtml = true;
            message = message + "<tr><td style=\"width:112px\">Family Name </td><td>&nbsp;</td><td>" + familyName + "</td></tr>";
            // message = message + "<tr><td style=\"width:112px\">ProductCode </td><td>&nbsp;</td><td>" + productcode + "</td></tr>";
            message = message + "<tr><td>Family Link </td><td>&nbsp;</td><td><a href='" + HttpContext.Current.Request.UrlReferrer.OriginalString + "'>" + HttpContext.Current.Request.UrlReferrer.OriginalString + " </a> </td></tr>";
            message = message + "<tr><td></td><td>&nbsp;</td><td></td></tr>";
            message = message + "<tr><td>ProductCode </td><td>&nbsp;</td><td>" + productcode + "</td></tr>";
            message = message + "<tr><td>Full Name </td><td>&nbsp;</td><td>" + fullname + "</td></tr>";
            message = message + "<tr><td>QTY </td><td>&nbsp;</td><td>" + qty + "</td></tr>";
            message = message + "<tr><td>Email id </td><td>&nbsp;</td><td>" + fromid + "</td></tr>";
            message = message + "<tr><td>Delivery Time </td><td>&nbsp;</td><td>" + deliverytime + "</td></tr>";
            message = message + "<tr><td>Phone </td><td>&nbsp;</td><td>" + phone + " </td></tr>";
            message = message + "<tr><td>Target Price</td><td>&nbsp;</td><td>" + targetprice + "</td></tr>";
            message = message + "<tr><td>Notes / Addtional Info</td><td>&nbsp;</td><td>" + notesandaddtionalinfo + "</td></tr>";
            message = message + "<tr><td></td><Br/><td>&nbsp;</td><td><Br/></td></tr>";
            if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
            {
                _UserID = HttpContext.Current.Session["USER_ID"].ToString();
                oUserinfo = objUserServices.GetUserInfo(Convert.ToInt32(_UserID));
                if (oUserinfo.CUSTOMER_TYPE.ToString().Equals("Retailer"))
                {
                    message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Retail" + "</td></tr>";
                }
                else if (oUserinfo.CUSTOMER_TYPE.ToString().Equals("Dealer"))
                {
                    message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Trade" + "</td></tr>";
                    message = message + "<tr><td>WES Customer No</td><td>&nbsp;</td><td>" + oUserinfo.CompanyID + "</td></tr>";
                }
                else
                    message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Retail" + "</td></tr>";

            }
            else
            {
                message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Retail" + "</td></tr>";
            }

            MessageObj.Body = "<html><body><table>" + message + "</table>" + " <font color=\"red\"></font></body></html>";
            System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
            smtpclient.UseDefaultCredentials = false;
            smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
            smtpclient.Send(MessageObj);
            return "1".ToString();
        }
        catch (Exception ex)
        {
            objErrorHandler.CreateLog(ex.ToString());
            return "-1".ToString();
        }
    }

    [System.Web.Services.WebMethod]
    public static string DownloadUpdate(string fullname, string fromid, string phone, string downloadrequire, string familyName)
    {
        ErrorHandler objErrorHandler = new ErrorHandler();
        try
        {
            HelperServices objHelperServices = new HelperServices();
            UserServices objUserServices = new UserServices();
            System.Net.Mail.MailMessage MessageObj = new System.Net.Mail.MailMessage();
            UserServices.UserInfo oUserinfo = new UserServices.UserInfo();
            string _UserID = "0";
            MessageObj.From = new System.Net.Mail.MailAddress(fromid);
            MessageObj.To.Add(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());

            //MessageObj.From = new System.Net.Mail.MailAddress(objHelperServices.GetOptionValues("ADMIN EMAIL").ToString());
            //MessageObj.To.Add(fromid);

            string message = string.Empty;
            MessageObj.Subject = "WESOnline-Form-Download-Request";
            MessageObj.IsBodyHtml = true;
            message = message + "<tr><td style=\"width:112px\">Family Name </td><td>&nbsp;</td><td>" + familyName + "</td></tr>";
            message = message + "<tr><td>Family Link </td><td>&nbsp;</td><td><a href='" + HttpContext.Current.Request.UrlReferrer.OriginalString + "'>" + HttpContext.Current.Request.UrlReferrer.OriginalString + " </a> </td></tr>";
            message = message + "<tr><td></td><td>&nbsp;</td><td></td></tr>";
            message = message + "<tr><td>Full Name </td><td>&nbsp;</td><td>" + fullname + "</td></tr>";
            message = message + "<tr><td>Email id </td><td>&nbsp;</td><td>" + fromid + "</td></tr>";
            message = message + "<tr><td>Phone </td><td>&nbsp;</td><td>" + phone + " </td></tr>";
            message = message + "<tr><td>Download Required / Comments</td><td>&nbsp;</td><td>" + downloadrequire + "</td></tr>";
            message = message + "<tr><td></td><Br/><td>&nbsp;</td><td><Br/></td></tr>";
            if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
            {
                _UserID = HttpContext.Current.Session["USER_ID"].ToString();
                oUserinfo = objUserServices.GetUserInfo(Convert.ToInt32(_UserID));
                if (oUserinfo.CUSTOMER_TYPE.ToString().Equals("Retailer"))
                {
                    message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Retail" + "</td></tr>";
                }
                else if (oUserinfo.CUSTOMER_TYPE.ToString().Equals("Dealer"))
                {
                    message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Trade" + "</td></tr>";
                    message = message + "<tr><td>WES Customer No</td><td>&nbsp;</td><td>" + oUserinfo.CompanyID + "</td></tr>";
                }
                else
                    message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Retail" + "</td></tr>";

            }
            else
            {
                message = message + "<tr><td>Customer Type</td><td>&nbsp;</td><td>" + "Retail" + "</td></tr>";
            }
            MessageObj.Body = "<html><body><table>" + message + "</table>" + " <font color=\"red\"></font></body></html>";
            System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(objHelperServices.GetOptionValues("MAIL SERVER").ToString());
            smtpclient.UseDefaultCredentials = false;
            smtpclient.Credentials = new System.Net.NetworkCredential(objHelperServices.GetOptionValues("MAIL SERVER USERNAME").ToString(), objHelperServices.GetOptionValues("MAIL SERVER PASSWORD").ToString());
            smtpclient.Send(MessageObj);
            return "1".ToString();
        }
        catch (Exception ex)
        {
            objErrorHandler.CreateLog(ex.ToString());
            return "-1".ToString();
        }
    }

    [System.Web.Services.WebMethod]

    public static string DynamicPag(string ipageno, string _Fid, string eapath, string Rawurl, string Pagecnt)
    {
        try
        {
            UC_family fm = new UC_family();
            //int i = fm.GetFamilyAllData(_Fid);

            //DataSet dsPriceTableAll = fm.dsPriceTableAll;
            //DataSet EADs = (DataSet)HttpContext.Current.Session["FamilyPro    duct"];
            //DataSet Ds = fm.Ds;
            //string CScontentvalue = FamilyServices.Dynamic_GenerateHorizontalHTMLJson(CNT, _Fid, Ds, dsPriceTableAll, EADs);
            if (Convert.ToInt16(ipageno) < Convert.ToInt16(Pagecnt))
            {
                string CScontentvalue = fm.Dynamic_pagination(ipageno, _Fid, eapath, Rawurl);
                if (Convert.ToInt16(ipageno) + 1 < Convert.ToInt16(Pagecnt))
                {
                    string lod = "lodder" + ipageno;
                    CScontentvalue = CScontentvalue + " <div  class='" + lod + "' align=\"center\"> </div>";
                }
                return CScontentvalue;
            }
            else
            {
                HttpContext.Current.Session["hfprevfid"] = null;
                return "";
            }
        }
        catch (Exception ex)
        {
            return "";
        }

    }



}
