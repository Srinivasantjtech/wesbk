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

using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class CategoryList : System.Web.UI.Page
{
    ErrorHandler objErrorHandler = new ErrorHandler();
    HelperDB objhelperDb = new HelperDB();
    string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();
    string stcategory = string.Empty;
    string stcategorylisttitle = string.Empty;
    string stcategorylistkey = string.Empty;
    string stitle = string.Empty;
    string skeyword = string.Empty;
    GetmetadataFromEA objgetmetadata = new GetmetadataFromEA();
    protected void Page_Load(object sender, EventArgs e)
    {
         try
        {
        HelperServices objHelperServices = new HelperServices();
        Session["PageUrl"] = Request.Url.PathAndQuery.ToString().Substring(Request.Url.PathAndQuery.IndexOf("/", 1) + 1);
        Session["CATALOG_ID"] = objHelperServices.GetOptionValues("DEFAULT CATALOG").ToString();
        Session["DO_PAGING"] = objHelperServices.GetOptionValues("SEARCH_DO_PAGING").ToString();
        if (Session["RECORDS_PER_PAGE"] == null || Session["RECORDS_PER_PAGE"] == "")
            Session["RECORDS_PER_PAGE"] = System.Configuration.ConfigurationManager.AppSettings["productcnt"].ToString();//objHelperServices.GetOptionValues("SEARCH_RECS_PER_PAGE").ToString();
        Session["INVENTORY_LEVEL_CHECK"] = objHelperServices.GetOptionValues("INVENTORY_LEVEL_CHECK").ToString();
        Session["SEARCH_CATEGORY_COLS"] = objHelperServices.GetOptionValues("SEARCH_CATEGORY_COLS").ToString();
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
        if (!IsPostBack)
        {
            if (Request.QueryString["ActionResult"] != null && Request.QueryString["ActionResult"].ToString() == "CATALOGUE")
            {
                //multiTabs.ActiveViewIndex = 0;
                ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:__doPostBack('ctl00$maincontent$menuTabs','0');", true);
            }
            else if (Request.QueryString["ActionResult"] != null && Request.QueryString["ActionResult"].ToString() == "NEWS")
            {
                //multiTabs.ActiveViewIndex = 1;
                ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:__doPostBack('ctl00$maincontent$menuTabs','1');", true);
            }
            //Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetNoStore();
        }
     
        Session["iPageNo"] = 1;
        }


         catch (Exception ex)
         {
             objErrorHandler.ErrorMsg = ex;
             objErrorHandler.CreateLog();

         }
    }

    //[System.Web.Services.WebMethod]
    //public static string FindAnotherEnd(string strvalue )
    //{
        
    //    DataSet rtnds=new DataSet();
    //    string rtnstr = "";
    //    TradingBell.WebCat.EasyAsk.EasyAsk_WES   EasyAsk= new TradingBell.WebCat.EasyAsk.EasyAsk_WES();
    //    string Ea=HttpContext.Current.Session["EA"].ToString();
   
    //    EasyAsk.GetAttributeProducts("ProductList", "", "Plug 1", strvalue, "", "1", "0", "Next",Ea);
    //     DataSet dsCat = new DataSet();

    //           dsCat = (DataSet)HttpContext.Current.Session["LHSAttributes"];

    //    if (dsCat == null || dsCat.Tables.Count == 0 || dsCat.Tables["Plug 2"] == null || dsCat.Tables["Plug 2"].Rows.Count ==0)
    //                rtnds.GetXml(); 
    //    else
    //    {
    //        rtnds.Tables.Add(dsCat.Tables["Plug 2"].Copy());
    //    }
    //    if (rtnds!=null && rtnds.Tables[0]!=null  && rtnds.Tables[0].Rows.Count>0)   
    //    {
    //        for (int i = 0; i <= rtnds.Tables[0].Rows.Count - 1; i++)
    //        {
    //            rtnstr = rtnstr + rtnds.Tables[0].Rows[i]["Plug 2"].ToString() + "#####";
                
    //        }
    //    }
    //    if (rtnstr != "")
    //        rtnstr = rtnstr.Substring(0, rtnstr.Length - 5);

    //    return rtnstr;
    //}
  

    [System.Web.Services.WebMethod]
    public static string DynamicPag(string strvalue, int ipageno, int iTotalPages, string eapath, string ViewMode, string irecords)
    {
        try
        {
            HelperServices objHelperServices = new HelperServices();
         
            try
            {
                string userid = string.Empty;
            if  (HttpContext.Current. Session["USER_ID"] != null) 
            {              
                
                userid = HttpContext.Current.Session["USER_ID"].ToString();
                if (userid == "")
                {
                    objHelperServices.CheckCredential();
                    if ((HttpContext.Current.Session["USER_ID"] == null)||(HttpContext.Current.Session["USER_ID"].ToString()==""))
                    {
                       
                        return "LOGIN";
                    }
                }
            }
            else
            {
               
                objHelperServices.CheckCredential();
               
                if( (HttpContext.Current.Session["USER_ID"] == null)||(HttpContext.Current.Session["USER_ID"].ToString()==""))
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
                        UC_categorylist objnew = new UC_categorylist();
                        System.Text.StringBuilder getPostsText = new System.Text.StringBuilder();


                        getPostsText.Append(objnew.DynamicPag_RenderHTMLJson(val, ipageno, eapath, ViewMode, irecords));
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

    private string Get_MetaDescription(string stcategory, string parentcatid)
    {

        string catid = string.Empty;
        try
        {


            DataTable Sqltb = (DataTable)objhelperDb.GetGenericDataDB(WesCatalogId, stcategory, parentcatid, "GET_CAT_DETAILS", HelperDB.ReturnType.RTTable);
            if (Sqltb != null)
            {
                if (Sqltb.Rows[0][1] != null)
                {
                    string metades = Sqltb.Rows[0][1].ToString();

                    if (metades != string.Empty)
                    {
                        metades = objgetmetadata.Replace_SpecialChar(metades);
                        Page.MetaDescription = metades;
                    }
                }
                if (Sqltb.Rows[0][0] != null)
                {
                    catid = Sqltb.Rows[0][0].ToString();
                    return catid;
                }
            }
        }
        catch
        {

        }

        return catid;
    }

    protected void Page_SaveStateComplete(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["tsb"] == null)
            {

                string urlstring = string.Empty;
                string pcatid = string.Empty;
                //Page.Title = "Cellink";
                if (Session["EA"] != null)
                {
                    string EA = Session["EA"].ToString();
                    DataSet ds = (DataSet)Session["BreadCrumbDS"];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {


                        if (ds.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "category")
                        {

                            if (i != 0)
                            {

                                stcategory = ds.Tables[0].Rows[i]["Itemvalue"].ToString();
                                pcatid = Get_MetaDescription(stcategory, pcatid);
                                stcategory = objgetmetadata.Replace_SpecialChar(stcategory);
                                Session["prodlisttitle"] = stcategory;
                                Session["prodlistname"] = stcategory;
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
                            else
                            {
                                stcategory = ds.Tables[0].Rows[i]["Itemvalue"].ToString();
                                pcatid = Get_MetaDescription(stcategory, "0");
                                stcategory = objgetmetadata.Replace_SpecialChar(stcategory);
                                h3_1.InnerText = stcategory;
                            }
                        }

                    }

                    string title_key = objgetmetadata.FetchData(ds);
                    if (title_key != "|")
                    {
                        string[] StrValues = title_key.Split(new string[] { "|" }, StringSplitOptions.None);
                        stitle = StrValues[0];
                        skeyword = StrValues[1];
                        urlstring = StrValues[2];
                        //Page.Title = "Cellink" + "-" + stitle.Replace("<ars>g</ars>", "-").ToString(); ;
                        Page.Title = stitle.Replace("<ars>g</ars>", " ").ToString(); ;
                        Page.MetaKeywords = objgetmetadata.Replace_SpecialChar(skeyword);



                    }

                }

                // Page.MetaDescription = "List of products from Maincategory";

                if (Session["prodmodel"] != null)
                {
                    string prodmodel = Session["prodmodel"].ToString();

                    h3_3.InnerText = objgetmetadata.Replace_SpecialChar(prodmodel);
                }


                if (h3_2.InnerText == string.Empty)
                {

                    h3_2.Visible = false;
                }
                if (h3_3.InnerText == string.Empty)
                {

                    h3_3.Visible = false;
                }
            }
            else
            {

                string pagetitle = string.Empty;

                if ((DataSet)Session["BreadCrumbDS"] != null)
                {
                    DataSet ds = (DataSet)Session["BreadCrumbDS"];
                    string dsrItemType = ds.Tables[0].Rows[0]["ItemType"].ToString().ToLower();

                    if (dsrItemType == "category")
                    {

                        h3_1.InnerText = ds.Tables[0].Rows[0]["Itemvalue"].ToString();
                        pagetitle = objgetmetadata.Replace_SpecialChar(ds.Tables[0].Rows[0]["ItemType"].ToString());
                    }
                    if (dsrItemType == "brand")
                    {



                        stcategory = ds.Tables[0].Rows[1]["Itemvalue"].ToString();
                        stcategory = objgetmetadata.Replace_SpecialChar(stcategory);
                        h2.InnerText = ds.Tables[0].Rows[1]["Itemvalue"].ToString();
                        Page.Title = stcategory + " " + pagetitle;
                        Page.MetaKeywords = pagetitle + "," + stcategory;
                        // Page.MetaDescription = stcategory + "," + "Cellular Phone Models and Accessories";
                    }

                }
            }
            if ((Page.Title == "")||(Page.Title =="Untitled Page") )
            {
                Page.Title ="WES AUSTRALIA" ;
            }
        }
        catch
        { }

    }





}
