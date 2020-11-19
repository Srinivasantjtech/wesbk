using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Common;

using System.Web.Mail;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CommonServices;
using System.Diagnostics;
public partial class ProductDetails : System.Web.UI.Page
{
    //Stopwatch sw = new Stopwatch();
    ErrorHandler objErrorHandler = new ErrorHandler();
    string stlistprodtitle = string.Empty;
  //  string stlistprod = "";
    string stcategory = string.Empty;
   // string sProd = "";
    string sfamily = string.Empty;
    string stype = string.Empty;
    string stcategorylisttitle = string.Empty;
    string stcategorylistkey = string.Empty;
    string sbrand = string.Empty;
    string smodel = string.Empty;
    string ssize = string.Empty;
    string spower = string.Empty;
    string stitle = string.Empty;
    string skeyword = string.Empty;
    GetmetadataFromEA objgetmetadata = new GetmetadataFromEA();
    HelperServices objHelperServices = new HelperServices();
    ProductServices objProductServices = new ProductServices();

    protected void Page_Load(object sender, EventArgs e)
    {
        //ErrorHandler objErrorHandler = new ErrorHandler();
        //sw.Start();
        Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
        HelperServices objHelperServices = new HelperServices();
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
        string pageurl = "ORGURL_";
        if ((Request.QueryString["printprice"] == null) 
            && (Request.QueryString["printdet"] == null)
            && (Request.QueryString["emailprice"] == null) 
            && (Request.QueryString["emaildet"] == null))
            
            
        {
            if (Request.QueryString["pid"] != null)
            {
                pageurl = pageurl + Request.QueryString["pid"];
                Session[pageurl] = Request.Url.PathAndQuery;
            }
        }
        //sw.Stop();
        //Console.WriteLine("Elapsed={0}", sw.Elapsed);

        //StackTrace st = new StackTrace();
        //StackFrame sf = st.GetFrame(0);

        //objErrorHandler.ExeTimelog = sf.GetMethod().Name + "," + sw.Elapsed.TotalSeconds.ToString();
        //// objErrorHandler.ExeTimelog = sf.GetMethod().Name + "," + sw.Elapsed;
        //objErrorHandler.CreateTimeLog(); 
    }
    protected void Page_SaveStateComplete(object sender, EventArgs e)
    {


        try
        {
            DataSet dscat = new DataSet();
            dscat = (DataSet)HttpContext.Current.Session["LHSAttributes"];
            //Page.Title = "Cellink";
            DataTable dtprod = new DataTable();

            string urlstring = string.Empty;
            string productcode = string.Empty;
            string productid = string.Empty;
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


                    else if (dstblrowitype == "product")
                    {

                        productcode = ds.Tables[0].Rows[i]["Productcode"].ToString();
                        h4.InnerText = productcode;
                        productid = ds.Tables[0].Rows[i]["ItemValue"].ToString();
                    }

                }


                string title_key = objgetmetadata.FetchData(ds);
                string[] StrValues = title_key.Split(new string[] { "|" }, StringSplitOptions.None);
                stitle = StrValues[0];
                skeyword = StrValues[1];
                urlstring = StrValues[2];
                //Page.Title = "Cellink" + "-" + stitle.Replace("<ars>g</ars>", "-").ToString(); ;
                Page.Title = stitle.Replace("<ars>g</ars>", " ").ToString(); ;
                if (Session["FamilyProduct"] != null)
                {
                    DataSet dsdesc = (DataSet)Session["FamilyProduct"];
                    dtprod = dsdesc.Tables[0];
                    //if (dtprod != null && dtprod.Rows.Count > 0)
                    //{
                    //    if (dtprod.Rows[0]["PARENT_KEYWORD"].ToString() != "")
                    //        skeyword = skeyword + "," + dtprod.Rows[0]["PARENT_KEYWORD"].ToString();

                    //    if (dtprod.Rows[0]["SUB_KEYWORD"].ToString() != "")
                    //        skeyword = skeyword + "," + dtprod.Rows[0]["SUB_KEYWORD"].ToString();

                    //}
                }
                if (dscat != null)
                {
                    if (dscat.Tables.Contains("Product Tags") == true && dscat.Tables["Product Tags"].Rows.Count > 0)
                        skeyword = objHelperServices.MetaTagProductkeyword(dscat.Tables["Product Tags"]);
                }

                //if (ds != null && ds.Tables[0].Rows.Count > 2)
                //    productid = ds.Tables[0].Rows[2]["ItemValue"].ToString();

                if (productcode != "")
                    skeyword = skeyword + objProductServices.GetProductSortKeyCode(productid);

                string skeywordRe = objgetmetadata.Replace_SpecialChar(skeyword);
                Page.MetaKeywords = skeywordRe;


            }


            Page.MetaDescription = "List of products from Maincategory";


            Session["prodmodel"] = string.Empty;
            Session["S_FName"] = string.Empty;

            if (Session["FamilyProduct"] != null)
            {
                DataSet dsdesc = (DataSet)Session["FamilyProduct"];
                dtprod = dsdesc.Tables[0];
                string expression = "attribute_Name in ('Description','Prod_Desc','Descriptions','Short Description','Description 1','Features','Notes','Note') ";
                DataRow[] foundRows;
                foundRows = dtprod.Select(expression);
                string desc = string.Empty;
                string DescForh2 = string.Empty;
                string frattr_name = string.Empty;
                for (int j = 0; j < foundRows.Length; j++)
                {
                    frattr_name = foundRows[j]["attribute_Name"].ToString();
                    if ((frattr_name == "Description") || (frattr_name == "Descriptions"))
                    {
                        DescForh2 = foundRows[j][1].ToString();

                        h2.InnerText = objgetmetadata.Replace_SpecialChar(DescForh2);
                    }
                    if (j == 0)
                    {
                        if (frattr_name == "Short Description")
                        {
                            DescForh2 = foundRows[j][1].ToString();
                            h2.InnerText = objgetmetadata.Replace_SpecialChar(DescForh2);
                        }
                        desc = foundRows[j][1].ToString();
                    }
                    else
                    {
                        if ((h2.InnerText == string.Empty) && (frattr_name == "Short Description"))
                        {
                            DescForh2 = foundRows[j][1].ToString();
                            h2.InnerText = objgetmetadata.Replace_SpecialChar(DescForh2);
                        }

                        if (foundRows[j - 1]["STRING_VALUE"].ToString() != foundRows[j]["STRING_VALUE"].ToString())
                        {
                            if (desc != string.Empty)
                            {
                                desc = desc + ". " + foundRows[j][1].ToString();
                            }
                            else
                            {
                                desc = foundRows[j][1].ToString();

                            }
                        }
                    }

                }
                if (h2.InnerText == string.Empty)
                {
                    h2.InnerText = objgetmetadata.Replace_SpecialChar(DescForh2);
                }


                Page.MetaDescription = objgetmetadata.Replace_SpecialChar(desc);

            }

            if (h1.InnerText == "")
            {
                h1.Visible = false;
            }
            if (h2.InnerText == "")
            {
                h2.Visible = false;
            }
            if (h3_2.InnerText == "")
            {
                h3_2.Visible = false;
            }
            if (h3_3.InnerText == "")
            {
                h3_3.Visible = false;
            }


        }
        catch
        { }


    }

    [System.Web.Services.WebMethod]
    public static string Strval(string WithPrice, string Details, string cellname, string pid, string fid, string cid,string path)
    {
        ErrorHandler objErrorHandler = new ErrorHandler();
        try
        {
            UC_Products prod = new UC_Products();
            string str = prod.Construct_ST(WithPrice, Details, "cell", pid, fid, cid,path);

            return str;
        }
        catch (Exception ex)
        {
            objErrorHandler.CreateLog(ex.ToString());
            return ex.ToString() ;
        }

    }
    //[System.Web.Services.WebMethod]
    //public static string Strpdf(string WithPrice, string Details, string cellname)
    //{
    //    UC_Products prod = new UC_Products();
    //   prod.ST_PDF(WithPrice, Details, cellname);

    //    return "true";
    //}
    [System.Web.Services.WebMethod]
    public static string StrEmail(string WithPrice, string Details, string cellname, string emailid,string notes,string pid,string fid,string cid,string path)
    {

        string Email="Email_"+pid;
        HttpContext.Current.Session[Email] = emailid;
        string Notes = "Notes_" + pid;
        HttpContext.Current.Session[Notes] = notes ;
      //  UC_Products prod = new UC_Products();
      //string res=  prod.ST_Email(WithPrice, Details, cellname,emailid,notes,pid,fid,cid);

      return "true";
    }

    [System.Web.Services.WebMethod]
    public static string SendAskQuestionMail(string fromid, string fname, string phone, string qustion, string productcode)
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

            string message = "";
            MessageObj.Subject = "WESOnline-Form-Product-Enquiry";
            MessageObj.IsBodyHtml = true;
            message = message + "<tr><td  style=\"width:112px\">Product Code </td><td>&nbsp;</td><td>" + productcode + "</td></tr>";
            message = message + "<tr><td>Product Link </td><td>&nbsp;</td><td><a href='" + HttpContext.Current.Request.UrlReferrer.OriginalString + "'>" + HttpContext.Current.Request.UrlReferrer.OriginalString + " </a> </td></tr>";
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
    public static string SendBulkBuyProjectPricing(string productcode, string fullname, string qty, string fromid, string deliverytime, string phone, string targetprice, string notesandaddtionalinfo)
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

            string message = "";
            MessageObj.Subject = "WESOnline-Form-BulkBuy-Enquiry";
            MessageObj.IsBodyHtml = true;
            message = message + "<tr><td style=\"width:112px\">ProductCode </td><td>&nbsp;</td><td>" + productcode + "</td></tr>";
            message = message + "<tr><td>Product Link </td><td>&nbsp;</td><td><a href='" + HttpContext.Current.Request.UrlReferrer.OriginalString + "'>" + HttpContext.Current.Request.UrlReferrer.OriginalString + " </a> </td></tr>";
            message = message + "<tr><td></td><td>&nbsp;</td><td></td></tr>";
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
    public static string DownloadUpdate(string fullname, string fromid, string phone, string downloadrequire, string productcode)
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

            string message = "";
            MessageObj.Subject = "WESOnline-Form-Download-Request";
            MessageObj.IsBodyHtml = true;
            message = message + "<tr><td style=\"width:112px\">ProductCode </td><td>&nbsp;</td><td>" + productcode + "</td></tr>";
            message = message + "<tr><td>Product Link </td><td>&nbsp;</td><td><a href='" + HttpContext.Current.Request.UrlReferrer.OriginalString + "'>" + HttpContext.Current.Request.UrlReferrer.OriginalString + " </a> </td></tr>";
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

}


