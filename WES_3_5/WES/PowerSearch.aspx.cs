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
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.CommonServices;
using System.Web.Configuration;
using System.Web.Services;
using System.Text.RegularExpressions;
using System.Collections.Generic;

using System.Data.SqlClient;
using System.Diagnostics;





public partial class PowerSearchPage : System.Web.UI.Page
{
    HelperServices objHelperServices = new HelperServices();
    ErrorHandler objErrorHandler = new ErrorHandler();
    EasyAsk_WES EasyAsk = new EasyAsk_WES();
    string _AttType = string.Empty;
    string _AttValue = string.Empty;
    int _resultpage = 10;
    int _PageNo = 1;
    string ParentCatID = string.Empty;
    string _Brand = string.Empty;
    string _searchstr = string.Empty;
    Stopwatch sw = new Stopwatch();
    protected void Page_Load(object sender, EventArgs e)
    {
        //ErrorHandler objErrorHandler = new ErrorHandler();
        //sw.Start();

        try
        {
        if (Session["USER_NAME"] == null)
        {
            Session["USER"] = "";
            Session["COUNT"] = "0";
            Response.Redirect("Login.aspx");
        }
        //***********************later want to be update with default page************//
        Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
        HelperServices objHelperServices = new HelperServices();
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
        Session["CATALOG_ID"] = objHelperServices.GetOptionValues("DEFAULT CATALOG").ToString();
        Session["DO_PAGING"] = objHelperServices.GetOptionValues("SEARCH_DO_PAGING").ToString();
        Session["RECORDS_PER_PAGE"] = System.Configuration.ConfigurationManager.AppSettings["productcnt"].ToString(); //objHelperServices.GetOptionValues("SEARCH_RECS_PER_PAGE").ToString();
        Session["INVENTORY_LEVEL_CHECK"] = objHelperServices.GetOptionValues("INVENTORY_LEVEL_CHECK").ToString();
        Session["SEARCH_CATEGORY_COLS"] = objHelperServices.GetOptionValues("SEARCH_CATEGORY_COLS").ToString();
        //**********************End**************
        Session["iPageNo"] = 1;
        if(IsPostBack)        
        {
            if (Request.Form["ctl00$maincontent$searchctrl1$__EVENTTARGET1"].ToString() == "OrderDetails")                
                {
                    string MultipleItems = Request.Form["ctl00$maincontent$searchctrl1$__EVENTARGUMENT1"].ToString().Substring(0, Request.Form["ctl00$maincontent$searchctrl1$__EVENTARGUMENT1"].ToString().Length - 1);//.LastIndexOf("Fid".ToCharArray());
                    Session["Multipleitems"] = MultipleItems;
                    Response.Redirect("OrderDetails.aspx");
                }           
        }

        //if (!IsPostBack)
        //{
        //    Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    Response.Cache.SetNoStore();
        //}


        //if (Request.QueryString["type"] != null)
        //    _AttType = Request.QueryString["Type"];
        //if (Request.QueryString["value"] != null)
        //    _AttValue = Request.QueryString["Value"];
        //if (Request.QueryString["bname"] != null)
        //    _Brand = Request.QueryString["bname"];
        //if (Request.QueryString["pgno"] != null)
        //    _PageNo = Convert.ToInt32(Request.QueryString["pgno"]);

        //if (Request.QueryString["searchstr"] != null)
        //    _searchstr = Request.QueryString["searchstr"].Trim();

        //if (Request.QueryString["srctext"] != null)
        //    _searchstr = Request.QueryString["srctext"].Trim();

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
        //EasyAsk.GetAttributeProducts(_searchstr, _AttType, _AttValue, _Brand, _resultpage.ToString(), (_PageNo - 1).ToString(), "Next");

    }
    [WebMethod]
    public static List<string> WestestAutoCompleteData(string strvalue)
    {
        List<string> result = new List<string>();
        ConnectionDB objConnectionDB = new ConnectionDB();
        OrderDB objOrderDB = new OrderDB();
        /* //SqlCommand objSqlCommand;
     
       //  using (objSqlCommand = new SqlCommand("select top 10 A.PRODUCT_ID,B.STRING_VALUE from TB_PRODUCT A,TB_PROD_SPECS B,TB_ATTRIBUTE C where A.PRODUCT_ID=B.PRODUCT_ID And B.ATTRIBUTE_ID=C.ATTRIBUTE_ID And C.PUBLISH2WEB=1 And C.ATTRIBUTE_ID=1  And  B.STRING_VALUE LIKE '%'+@STRING_VALUE+'%'", objConnectionDB.GetConnection()));
             using (objSqlCommand = new SqlCommand("select top 15 A.PRODUCT_ID,B.STRING_VALUE from TB_PRODUCT A,TB_PROD_SPECS B,TB_ATTRIBUTE C where A.PRODUCT_ID=B.PRODUCT_ID And B.ATTRIBUTE_ID=C.ATTRIBUTE_ID And C.PUBLISH2WEB=1 And C.ATTRIBUTE_ID=1  And  B.STRING_VALUE LIKE '%'+@STRING_VALUE+'%'", objConnectionDB.GetConnection())) ;
             {
                //objSqlCommand.CommandType = CommandType.Text;
                 objSqlCommand.Parameters.Add("@STRING_VALUE", strvalue);
                 SqlDataReader dr = objSqlCommand.ExecuteReader();
                 while (dr.Read())
                 {
                     result.Add(dr["STRING_VALUE"].ToString());
                 }
                 return result;
           //  }
         }
         * */
        string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();

        DataTable Tbl = (DataTable)objOrderDB.GetGenericDataDB(WesCatalogId, strvalue, "GET_POWER_SEARCH_AUTO_COMPLETE_PRODUCT_TEST", OrderDB.ReturnType.RTTable);
        if (Tbl != null)
        {
            foreach (DataRow dr in Tbl.Rows)
            {
                result.Add(dr["ProductCode"].ToString());
            }
        }

        return result;

    }

    [System.Web.Services.WebMethod]
    public static string DynamicPag(string strvalue, int ipageno, int iTotalPages, string eapath, string ViewMode, string irecords)
    {
        try
        {
            HelperServices objHelperServices = new HelperServices();
            try
            {
                string userid = "";
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
                        getPostsText.Append(objnew.DynamicPagJson(val, ipageno, eapath, ViewMode, irecords));
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

      


    }
}
