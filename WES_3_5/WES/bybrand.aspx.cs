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
using System.Data.SqlClient;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using System.Diagnostics;
public partial class bybrand : System.Web.UI.Page
{
    HelperServices objHelperServices = new HelperServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    string stitle = string.Empty;
    string skeyword = string.Empty;
   
    GetmetadataFromEA objgetmetadata = new GetmetadataFromEA();

    //Stopwatch sw = new Stopwatch();
    protected void Page_Load(object sender, EventArgs e)
    {
        //ErrorHandler objErrorHandler = new ErrorHandler();
        //sw.Start();
        //***********************later want to be update with default page************//
        try
        {
        Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
        Session["CATALOG_ID"] = objHelperServices.GetOptionValues("DEFAULT CATALOG").ToString();
        Session["DO_PAGING"] = objHelperServices.GetOptionValues("SEARCH_DO_PAGING").ToString();
        if(Session["RECORDS_PER_PAGE"] == null || Session["RECORDS_PER_PAGE"] == "")
            Session["RECORDS_PER_PAGE"] = System.Configuration.ConfigurationManager.AppSettings["productcnt"].ToString();// objHelperServices.GetOptionValues("SEARCH_RECS_PER_PAGE").ToString();
        Session["INVENTORY_LEVEL_CHECK"] = objHelperServices.GetOptionValues("INVENTORY_LEVEL_CHECK").ToString();
        Session["SEARCH_CATEGORY_COLS"] = objHelperServices.GetOptionValues("SEARCH_CATEGORY_COLS").ToString();
        Session["iPageNo"] = 1;
            //**********************End**************
       
            if(IsPostBack)        
        {
                if (Request.Form["__EVENTTARGET"].ToString()=="compare,")                
                {
                    string s = Request.Form["__EVENTARGUMENT"].ToString().Substring(0,Request.Form["__EVENTARGUMENT"].ToString().Length-1);//.LastIndexOf("Fid".ToCharArray());
                    string[] str = Request.Form["__EVENTARGUMENT"].Split('$');
                    int FamilyID = objHelperServices.CI(str[0]);
                    Session["CloseWin"] = str[1];
                    Session["FAMILY_ID"] = str[0];
                    Response.Redirect("Compare.aspx",false);
                }           
        }

            //if (!IsPostBack)
            //{
            //    Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //    Response.Cache.SetNoStore();
            //}
        }


        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
           
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

            string prodmodel = string.Empty;
            string prodname = string.Empty;
            string prodlistname = string.Empty;
            string urlstring = string.Empty;



            if (Session["prodlistname"] != null)
            {
                Session["prodlisttitle"] = Session["prodlistname"].ToString();
                prodname = Session["prodlistname"].ToString();
                Page.Title = Session["prodlisttitle"].ToString();
                prodlistname = Session["prodlistname"].ToString();
                h2.InnerText = objgetmetadata.Replace_SpecialChar(prodlistname);
            }
            if (Session["prodmodel"] != null)
            {
                prodmodel = Session["prodmodel"].ToString();
                prodmodel = objgetmetadata.Replace_SpecialChar(prodmodel);
                Page.Title = prodmodel + " " + Session["prodlisttitle"];
                h1.InnerText = prodmodel;
            }
            if (Session["BreadCrumbDS"] != null)
            {
                DataSet dsbc = (DataSet)Session["BreadCrumbDS"];
                string title_key = objgetmetadata.FetchData(dsbc);
                string[] StrValues = title_key.Split(new string[] { "|" }, StringSplitOptions.None);
                stitle = StrValues[0];
                skeyword = StrValues[1];
                urlstring = StrValues[2];
            }

            Page.Title = stitle.Replace("<ars>g</ars>", " ").ToString();
            skeyword = objgetmetadata.Replace_SpecialChar(skeyword);
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
                            skeyword = skeyword + "," + strkeyword1;
                        }

                    }
                }
            }
            Page.MetaKeywords = skeyword;
            string Allmetadesc =  "Cellular Phone Models and Accessories";
            Page.MetaDescription = objgetmetadata.Replace_SpecialChar(Allmetadesc);
            Session["prodmodel"] = string.Empty;
            if (h2.InnerText == "")
            {
                h2.Visible = false;
            }
            if (h1.InnerText == "")
            {
                h1.Visible = false;
            }

        }
        catch
        { }
    }
    [System.Web.Services.WebMethod]
    public static string DynamicPag(string strvalue, int ipageno, int iTotalPages, string eapath, string ViewMode, string irecords)
    {
        try
        {

            HelperServices objHelperServices = new HelperServices();
            try
            {
                string userid = string.Empty;
                if (HttpContext.Current.Session["USER_ID"] != null)
                {

                    userid = HttpContext.Current.Session["USER_ID"].ToString();
                    if (userid == "")
                    {
                        objHelperServices.CheckCredential();
                        if ((HttpContext.Current.Session["USER_ID"] == null) || (HttpContext.Current.Session["USER_ID"].ToString() == ""))
                        {

                            return "LOGIN";
                        }
                    }
                }
                else
                {

                    objHelperServices.CheckCredential();

                    if ((HttpContext.Current.Session["USER_ID"] == null) || (HttpContext.Current.Session["USER_ID"].ToString() == ""))
                    {


                        return "LOGIN";

                    }



                }
            }
            catch
            {

                return "LOGIN";
            }
            if (ipageno <= iTotalPages)
                {



                    string val = strvalue;
                        HttpContext.Current.RewritePath(val, false);
                        search_searchrsltproducts objnew = new search_searchrsltproducts();
                        System.Text.StringBuilder getPostsText = new System.Text.StringBuilder();
                        eapath = eapath.Replace("###", "'");

                        getPostsText.Append(objnew.DynamicPag_BrandJson(val, ipageno, eapath, ViewMode, irecords));
                        return getPostsText.ToString();
                   


                }
                else
                {

                    return "";
                }

           
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }

        //HttpContext.Current.Session["stprodlist"] = getPostsText.ToString();
        // getPostsText.AppendFormat("<div style='height:15px;'></div>");


    }
}
