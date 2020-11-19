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
using System.Data.SqlClient;

using TradingBell5.CatalogX;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
//using System.Diagnostics;
public partial class UC_maincategory : System.Web.UI.UserControl
{
  

    #region "Declarations"    
    
    //Stopwatch stopwatch = new Stopwatch();
    HelperDB objHelperDB = new HelperDB();
    ErrorHandler objErrorHandler = new ErrorHandler();
    HelperServices objHelperServices = new HelperServices();
    Security objSecurity = new Security();
    UserServices objUserServices = new UserServices();
    CategoryServices objCategoryServices = new CategoryServices();
    ConnectionDB objConnectionDB = new ConnectionDB();
    int iCatalogId;
  //  int iInventoryLevelCheck;
    int iRecordsPerPage = 16;
   // bool bIsStartOver = true;
   // bool bDoPaging;
    int iPageNo = 1;
   // bool bSortAsc = true;
   // string _SearchString = "";
    string tempCID = string.Empty;
    string tempCName = string.Empty;

    //TreeView TVCategory = new TreeView();
  
    //string _TvrSkin;
    //String _CategoryText;
   // int _CatalogId=1;
    //string _ImagePath;
    //string _CategoryLogo;
    //string _FamilyLogo;
    //string _DisplayCategoryMode = "FLAT";
    //string _DisplayFamilyCount="NO";
    //string _DisplayProductCount="YES";
    //string _DisplayFamilyLogo="NO";
    //string _DisplayCategoryLogo="NO";
    //string _NaviWidth;
    //string _NaviHeight;
    //string _NodeExpanded;
    //string _CategoryHeaderText;
    //string _HeaderCssClass;
    string MCID = string.Empty;
    //string CID = "";
    //string ParentCatID = "";
 //   string valuepath = "";
    string stemplatepath = string.Empty;
    string _catCid = string.Empty;
    string _parentCatID = string.Empty;
    EasyAsk_WES EasyAsk = new EasyAsk_WES();
    DataSet dscat = new DataSet();
    public string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();
    public string WesNewsCategoryId = System.Configuration.ConfigurationManager.AppSettings["WESNEWS_CATEGORY_ID"].ToString();
    #endregion

    public enum DisplayMode
    {
        No = 0,
        YES = 1
    }
    DisplayMode _CategoryHeaderVisible;

    ConnectionDB conStrr = new ConnectionDB();


    //public string ST_Browsebycategory()
    //{

    //    if (Request.QueryString["fid"] != null && Request.QueryString["fid"].ToString() != "" && Request.QueryString["fid"].ToString() != "List all products")
    //    {
    //        CID = GetCIDD(Request.QueryString["fid"].ToString());
    //    }
    //    if ((Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "List all models") || CID != "" || Request.Url.ToString().ToLower().Contains("powersearch.aspx") == true)
    //    {
    //        HelperDB objHelperDB = new HelperDB();
    //        ConnectionDB objConnectionDB = new ConnectionDB();

    //        TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("BROWSEBYCATEGORY", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
    //        if (Request.QueryString["cid"] == null)
    //            tbwtEngine.paraValue = CID;
    //        else
    //            tbwtEngine.paraValue = Request.QueryString["cid"].ToString();
    //        tbwtEngine.RenderHTML("Row");
    //        return (tbwtEngine.RenderedHTML);
    //    }
    //    return "";
    //}

    private string GetCIDD(string familyid)
    {
        try
        {
            DataSet prodtable = new DataSet();
            prodtable = (DataSet)objHelperDB.GetGenericPageDataDB(familyid, "GET_MAINCATEGORY_CAREGORY_ID", HelperDB.ReturnType.RTDataSet);
            //SqlDataAdapter da = new SqlDataAdapter("select CATEGORY_ID from tb_family where family_id=" + familyid, conStrr.ConnectionString.ToString().Substring(conStrr.ConnectionString.ToString().IndexOf(';') + 1));
            //da.Fill(prodtable, "Producttable");
            //return prodtable.Tables[0].Rows[0].ItemArray[0].ToString();

            if (prodtable != null && prodtable.Tables.Count > 0)
                return prodtable.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
            else
                return "";
        }
        catch (Exception e)
        {
            objErrorHandler.ErrorMsg = e;
            objErrorHandler.CreateLog();
            return string.Empty;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {

        
            iCatalogId = Convert.ToInt32(WesCatalogId);

            stemplatepath = Server.MapPath(ConfigurationManager.AppSettings["StringTemplatePath"].ToString());

         
            if (Request.QueryString["cid"] != null)
            {
                _catCid = Request.QueryString["cid"];
            }

             if (Request.QueryString["pgno"] != null)
            {
                iPageNo = Convert.ToInt32(Request.QueryString["pgno"]);
            }
            if (!IsPostBack)
            {
                
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
           
        }
    }

    protected void logoutsession(object sender, EventArgs e)
    {
        objUserServices.OnLineFlag(false, objHelperServices.CI(Session["USER_ID"]));
        Session.RemoveAll();
        Session.Clear();
        Session.Abandon();
        Session["USER_ID"] = "";
        Response.Redirect("Login.aspx");
    }

    
     //private DataSet GetDataSet(string SQLQuery)
     //{
     //    DataSet ds = new DataSet();
     //    SqlDataAdapter da = new SqlDataAdapter(SQLQuery, conStr.ConnectionString.ToString().Substring(conStr.ConnectionString.ToString().IndexOf(';') + 1));
     //    da.Fill(ds, "generictable");
     //    return ds;
     //}

     private string GetCID(string familyid)
     {
         try
         {


             DataSet DSBC = null;
             string catIDtemp = string.Empty;
             
             DSBC = (DataSet)objHelperDB.GetGenericPageDataDB(familyid, "GET_MAINCATEGORY_CAREGORY_ID", HelperDB.ReturnType.RTDataSet);

             if (DSBC != null && DSBC.Tables.Count > 0)
                 catIDtemp = DSBC.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
             else
                 catIDtemp = string.Empty;
             do
             {
                 //DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
                 DSBC = (DataSet)objHelperDB.GetGenericPageDataDB(catIDtemp, "GET_CATEGORYLIST_CAREGORY", HelperDB.ReturnType.RTDataSet);
                 if (DSBC != null)
                 {
                     foreach (DataRow DR in DSBC.Tables[0].Rows)
                     {
                         catIDtemp = DR["PARENT_CATEGORY"].ToString();
                         MCID = DR["CATEGORY_ID"].ToString() + ">" + MCID;
                     }
                 }
             } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
             MCID = DSBC.Tables[0].Rows[0]["CATEGORY_ID"].ToString() + ">" + MCID;
             return DSBC.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
         }
         catch (Exception ex)
         {

         }
         return "";

     }
     private string GetParentCatID(string catID)
     {
         try
         {
             DataSet DSBC = null;
             string catIDtemp = catID;
             do
             {
                 //DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
                 DSBC = (DataSet)objHelperDB.GetGenericPageDataDB(catIDtemp, "GET_CATEGORYLIST_CAREGORY", HelperDB.ReturnType.RTDataSet);
                 if (DSBC != null)
                 {
                     foreach (DataRow DR in DSBC.Tables[0].Rows)
                     {
                         catIDtemp = DR["PARENT_CATEGORY"].ToString();
                         if (catIDtemp == "0")
                         {
                             // DSUBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY = '" + DR["CATEGORY_ID"].ToString() + "' AND CATEGORY_NAME Like 'Product'");
                             return DR["CATEGORY_ID"].ToString();
                         }
                     }
                 }
             } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
             return DSBC.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
         }
         catch (Exception ex)
         {

         }
         return "";
     }
     private string GetMCatID(string MainCategoryid)
     {
         DataSet DSBC = null;
         string catIDtemp = string.Empty;
         catIDtemp = MainCategoryid;
         MCID = MainCategoryid + ">" + MCID;
         do
         {
             //DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
             DSBC = (DataSet)objHelperDB.GetGenericPageDataDB(catIDtemp, "GET_CATEGORYLIST_CAREGORY", HelperDB.ReturnType.RTDataSet);
             if (DSBC != null)
             {
                 foreach (DataRow DR in DSBC.Tables[0].Rows)
                 {
                     catIDtemp = DR["PARENT_CATEGORY"].ToString();
                     MCID = DR["PARENT_CATEGORY"].ToString() + ">" + MCID;
                 }
                 if (DSBC.Tables[0].Rows.Count <= 0)
                     return "0";
             }
         } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
         return DSBC.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
     }



     protected string Get_Value_Breadcrum()
     {

         string sHTML = string.Empty;
      //  string sBrandAndModelHTML = "";
       // string sModelListHTML = "";
         try
            {

                //stopwatch.Start();
                //StringTemplateGroup _stg_container = null;
                //StringTemplateGroup _stg_records = null;
                //StringTemplate _stmpl_container = null;
                //StringTemplate _stmpl_records = null;
                //StringTemplate _stmpl_records1 = null;
                //StringTemplate _stmpl_recordsrows = null;
                TBWDataList[] lstrecords = new TBWDataList[0];
                TBWDataList[] lstrows = new TBWDataList[0];

               // StringTemplateGroup _stg_container1 = null;
               // StringTemplateGroup _stg_records1 = null;                
                TBWDataList1[] lstrecords1 = new TBWDataList1[0];                
                TBWDataList1[] lstrows1 = new TBWDataList1[0];
               // int ictrows = 0;
                string _tsb = string.Empty;
                string _tsm = string.Empty;
                string _type = string.Empty;
                string _value = string.Empty;
                string _bname = string.Empty;
                string _searchstr = string.Empty;
                 string _byp = "2";
                string _bypcat=null;


                string _pid = string.Empty;
                string _fid = string.Empty;
                string _seeall = string.Empty;
                _bypcat = Request.QueryString["bypcat"];
                
      
                     

                if (Request.QueryString["tsm"] != null && Request.QueryString["tsm"].ToString() != "")
                    _tsm = Request.QueryString["tsm"];

                if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "")
                    _tsb = Request.QueryString["tsb"];

                if (Request.QueryString["type"]!=null)
                    _type=Request.QueryString["type"];

                if (Request.QueryString["value"]!=null)
                    _value=Request.QueryString["value"];

                if (Request.QueryString["bname"]!=null)
                    _bname=Request.QueryString["bname"];
                if (Request.QueryString["searchstr"] != null)
                    _searchstr = Request.QueryString["searchstr"];
                if (Request.QueryString["srctext"] != null)
                    _searchstr = Request.QueryString["srctext"];

                if (Request.QueryString["fid"] != null)
                    _fid = Request.QueryString["fid"];
                if (Request.QueryString["pid"] != null)
                    _pid = Request.QueryString["pid"];

                if (Request.QueryString["seeall"] != null)
                    _seeall = Request.QueryString["seeall"];


                if(_catCid!="")
                   _parentCatID = GetParentCatID(_catCid);
                 // exception handle for repository category

                string requrl = HttpContext.Current.Request.Url.ToString().ToLower();

                try
                {
                    if ((requrl.Contains("productdetails.aspx")))

                    {
                        if (HttpContext.Current.Session["Category_Attributes"] == null && _parentCatID != "")
                        {
                            EasyAsk.GetMainMenuClickDetail(_parentCatID, "");
                        }
                    }
                }
                catch (Exception ex)
                {

                }




                if (Request.QueryString["path"] != null)
           
                    HttpContext.Current.Session["EA"] = objSecurity.StringDeCrypt(Request.QueryString["path"].ToString());
               

                //if (HttpContext.Current.Session["MainCategory"]!=null)
                //{
                //    DataRow[] dr = ((DataSet)HttpContext.Current.Session["MainCategory"]).Tables[0].Select("CATEGORY_ID='" + _parentCatID + "'");
                //    if (dr.Length>0)
                //     _byp=dr[0]["CUSTOM_NUM_FIELD3"].ToString();
                //}

                if (HttpContext.Current.Application["key_MainCategory"] != null)
                {
                    DataRow[] dr = ((DataSet)HttpContext.Current.Application["key_MainCategory"]).Tables[0].Select("CATEGORY_ID='" + _parentCatID + "'");
                    if (dr.Length>0)
                     _byp=dr[0]["CUSTOM_NUM_FIELD3"].ToString();
                }
            
                if ((requrl.Contains("categorylist.aspx")))
                {
                    if (_bypcat == null)
                    {
                        
                        EasyAsk.GetMainMenuClickDetail(_catCid, "");


                        string CatName = string.Empty;
                        DataTable tmptbl=null;
                        if (HttpContext.Current.Session["MainMenuClick"]!=null)
                        {
                          //  tmptbl = ((DataSet)HttpContext.Current.Session["MainCategory"]).Tables[0] ;
                            tmptbl = ((DataSet)HttpContext.Current.Application["key_MainCategory"]).Tables[0];

                             tmptbl = tmptbl.Select("CATEGORY_ID='" + _catCid + "'").CopyToDataTable();
                                                      
                            if (tmptbl != null && tmptbl.Rows.Count > 0)
                            {                              
                                CatName = tmptbl.Rows[0]["CATEGORY_NAME"].ToString();
                            }


                        }           


                        if (Session["RECORDS_PER_PAGE_CATEGORY_LIST"] != null)
                            iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_CATEGORY_LIST"].ToString());
                      //  stopwatch.Start();
                        
                        EasyAsk.GetAttributeProducts("CategoryProductList", "", "Category", CatName, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                      //  stopwatch.Stop();
                      //  objErrorHandler.CreateLog("maincategory CategoryProductList function return time:" + "=" + stopwatch.Elapsed);
                      //  objErrorHandler.CreateLog("maincategory function ea path" + HttpContext.Current.Session["EA"].ToString());
                    }
                    else if (_tsb != "")
                    {
                        string parentCatName = GetCName(_catCid);
                        EasyAsk.GetWESModel(parentCatName, iCatalogId, _tsb);
                    }

                }
                if ((requrl.Contains("bybrand.aspx")))
                {
                   // int SubCatCount=0;
                    if (Request.QueryString["type"] == null )
                    {
                        if (_tsb != null && _tsb != "" && _tsm != null && _tsm != null)
                        {
                          
                            //string parentCatName = GetCName(ParentCatID);
                            //EasyAsk.GetBrandAndModelProducts(parentCatName, _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                            if (Session["RECORDS_PER_PAGE_BYBRAND"] != null)
                                iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_BYBRAND"].ToString());

                            EasyAsk.GetAttributeProducts("ByBrand", "", "Model", _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                          
                        }
                    }
                    else
                    {
                        if (Session["RECORDS_PER_PAGE_BYBRAND"] != null)
                            iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_BYBRAND"].ToString());

                        if (_type != "")
                        {

                            EasyAsk.GetAttributeProducts("ByBrand", "", _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                            
                        }
                        else
                        { //new open

                            EasyAsk.GetAttributeProducts("ByBrand", "", "Model", _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                            
                        }
                    }
                }
                if ((requrl.Contains("product_list.aspx")))
                {
                    if (Session["RECORDS_PER_PAGE_PRODUCT_LIST"] != null)
                         iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_PRODUCT_LIST"].ToString());

                    
                     EasyAsk.GetAttributeProducts("ProductList", "", _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                    
                }
                if ((requrl.Contains("powersearch.aspx")))
                {
                    if (Session["RECORDS_PER_PAGE_POWERSEARCH"] != null)
                        iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_POWERSEARCH"].ToString());
                  
                    EasyAsk.GetAttributeProducts("PowerSearch", _searchstr, _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                   
                }
                if ((requrl.Contains("family.aspx")))
                {
                    if (Request.QueryString["type"] == null)
                    {
                       
                        EasyAsk.GetAttributeProducts("FamilyPage", "", "FamilyId", _fid, "", "0", "0", "");
                       
                    }
                    else
                    {
                       
                        EasyAsk.GetAttributeProducts("FamilyPage", _searchstr, _type, _value, _bname, "0", "0", "");
                       
                    }
                }
                if ((requrl.Contains("productdetails.aspx")))
                {
                    if (Request.QueryString["type"] == null)
                    {
                        
                        EasyAsk.GetAttributeProducts("ProductPage", "", "ProductId", _pid, "", "0", "0", "");
                        
                    }
                    else
                    {
                       
                        EasyAsk.GetAttributeProducts("ProductPage", _searchstr, _type, _value, _bname, "0", "0", "");
                        
                    }
                }
               // stopwatch.Stop();
               // objErrorHandler.CreateLog("Get_Value_Breadcrum function load time:" + "=" + stopwatch.Elapsed);
            }
         catch (Exception ex)
         {

             sHTML = ex.Message;
         }

         return sHTML;
     }

    //-------------------------------------------------------
     protected void Page_PreRender(object sender, EventArgs e)
     {
         Get_Value_Breadcrum();
     }
    protected string ST_Categories()
    {
        //stopwatch.Start();
        string sHTML = string.Empty;
        string sBrandAndModelHTML = string.Empty;
        string sModelListHTML = string.Empty;
     //   string sCablefilterHTML = "";
         try
            {

                
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
               // StringTemplate _stmpl_records1 = null;
                StringTemplate _stmpl_recordsrows = null;
                TBWDataList[] lstrecords = new TBWDataList[0];
                TBWDataList[] lstrows = new TBWDataList[0];

                //StringTemplateGroup _stg_container1 = null;
               // StringTemplateGroup _stg_records1 = null;                
                TBWDataList1[] lstrecords1 = new TBWDataList1[0];                
                TBWDataList1[] lstrows1 = new TBWDataList1[0];
                int ictrows = 0;
                string _tsb = string.Empty;
                string _tsm = string.Empty;
                string _type = string.Empty;
                string _value = string.Empty;
                string _bname = string.Empty;
                string _searchstr = string.Empty;
                 string _byp = "2";
                string _bypcat=null;


                string _pid = string.Empty;
                string _fid = string.Empty;
                string _seeall = string.Empty;
                _bypcat = Request.QueryString["bypcat"];
                
      
                     

                if (Request.QueryString["tsm"] != null && Request.QueryString["tsm"].ToString() != "")
                    _tsm = Request.QueryString["tsm"];

                if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "")
                    _tsb = Request.QueryString["tsb"];

                if (Request.QueryString["type"]!=null)
                    _type=Request.QueryString["type"];

                if (Request.QueryString["value"]!=null)
                    _value=Request.QueryString["value"];

                if (Request.QueryString["bname"]!=null)
                    _bname=Request.QueryString["bname"];
                if (Request.QueryString["searchstr"] != null)
                    _searchstr = Request.QueryString["searchstr"];
                if (Request.QueryString["srctext"] != null)
                    _searchstr = Request.QueryString["srctext"];

                if (Request.QueryString["fid"] != null)
                    _fid = Request.QueryString["fid"];
                if (Request.QueryString["pid"] != null)
                    _pid = Request.QueryString["pid"];

                if (Request.QueryString["seeall"] != null)
                    _seeall = Request.QueryString["seeall"];


                if(_catCid!="")
                   _parentCatID = GetParentCatID(_catCid);
               
             //Start
             //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("productdetails.aspx")==true)
                //{
                //    if (HttpContext.Current.Session["Category_Attributes"] == null)
                //    {
                //        EasyAsk.GetMainMenuClickDetail(_parentCatID, "");
                //    }
                //}




                //if (Request.QueryString["path"] != null)
                //    HttpContext.Current.Session["EA"] = objSecurity.StringDeCrypt(Request.QueryString["path"].ToString());

                //if (HttpContext.Current.Session["MainCategory"]!=null)
                //{
                //    DataRow[] dr = ((DataSet)HttpContext.Current.Session["MainCategory"]).Tables[0].Select("CATEGORY_ID='" + _parentCatID + "'");
                //    if (dr.Length>0)
                //     _byp=dr[0]["CUSTOM_NUM_FIELD3"].ToString();
                //}
                
                //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("categorylist.aspx") == true )
                //{
                //    if (_bypcat == null)
                //    {
                        
                //        EasyAsk.GetMainMenuClickDetail(_catCid, "");


                //        string CatName = "";
                //        DataTable tmptbl=null;
                //        if (HttpContext.Current.Session["MainMenuClick"]!=null)
                //        {
                //            tmptbl = ((DataSet)HttpContext.Current.Session["MainCategory"]).Tables[0] ;

                //             tmptbl = tmptbl.Select("CATEGORY_ID='" + _catCid + "'").CopyToDataTable();
                                                      
                //            if (tmptbl != null && tmptbl.Rows.Count > 0)
                //            {                              
                //                CatName = tmptbl.Rows[0]["CATEGORY_NAME"].ToString();
                //            }


                //        }           


                //        if (Session["RECORDS_PER_PAGE_CATEGORY_LIST"] != null)
                //            iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_CATEGORY_LIST"].ToString());
                        
                //        EasyAsk.GetAttributeProducts("CategoryProductList", "", "Category", CatName, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                        
                //    }
                //    else if (_tsb != "")
                //    {
                //        string parentCatName = GetCName(_catCid);
                //        EasyAsk.GetWESModel(parentCatName, iCatalogId, _tsb);
                //    }

                //}
                //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("bybrand.aspx") == true)
                //{
                //    int SubCatCount=0;
                //    if (Request.QueryString["type"] == null )
                //    {
                //        if (_tsb != null && _tsb != "" && _tsm != null && _tsm != null)
                //        {
                          
                //            //string parentCatName = GetCName(ParentCatID);
                //            //EasyAsk.GetBrandAndModelProducts(parentCatName, _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                //            if (Session["RECORDS_PER_PAGE_BYBRAND"] != null)
                //                iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_BYBRAND"].ToString());

                //            EasyAsk.GetAttributeProducts("ByBrand", "", "Model", _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                          
                //        }
                //    }
                //    else
                //    {
                //        if (Session["RECORDS_PER_PAGE_BYBRAND"] != null)
                //            iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_BYBRAND"].ToString());

                //        if (_type != "")
                //        {

                //            EasyAsk.GetAttributeProducts("ByBrand", "", _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                            
                //        }
                //        else
                //        { //new open

                //            EasyAsk.GetAttributeProducts("ByBrand", "", "Model", _tsm, _tsb, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                            
                //        }
                //    }
                //}
                //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("product_list.aspx") == true)
                //{
                //    if (Session["RECORDS_PER_PAGE_PRODUCT_LIST"] != null)
                //         iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_PRODUCT_LIST"].ToString());

                    
                //     EasyAsk.GetAttributeProducts("ProductList", "", _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                    
                //}
                //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("powersearch.aspx") == true)
                //{
                //    if (Session["RECORDS_PER_PAGE_POWERSEARCH"] != null)
                //        iRecordsPerPage = Convert.ToInt32(Session["RECORDS_PER_PAGE_POWERSEARCH"].ToString());
                  
                //    EasyAsk.GetAttributeProducts("PowerSearch", _searchstr, _type, _value, _bname, iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                   
                //}
                //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("family.aspx") == true)
                //{
                //    if (Request.QueryString["type"] == null)
                //    {
                       
                //        EasyAsk.GetAttributeProducts("FamilyPage", "", "FamilyId", _fid, "", "0", "0", "");
                       
                //    }
                //    else
                //    {
                       
                //        EasyAsk.GetAttributeProducts("FamilyPage", _searchstr, _type, _value, _bname, "0", "0", "");
                       
                //    }
                //}
                //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("productdetails.aspx") == true)
                //{
                //    if (Request.QueryString["type"] == null)
                //    {
                        
                //        EasyAsk.GetAttributeProducts("ProductPage", "", "ProductId", _pid, "", "0", "0", "");
                        
                //    }
                //    else
                //    {
                       
                //        EasyAsk.GetAttributeProducts("ProductPage", _searchstr, _type, _value, _bname, "0", "0", "");
                        
                //    }
                //}
            ////For Meta Tag
                //if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "" && (Request.QueryString["tsm"].ToString() != null && Request.QueryString["tsm"] != null))
                //{
                //    string category_nameh = "";
                //    DataSet tmp = GetDataSet("SELECT CATEGORY_NAME  FROM	TB_CATEGORY WHERE	CATEGORY_ID ='" + catID + "'");
                //    if (tmp != null && tmp.Tables.Count >= 1 && tmp.Tables[0].Rows.Count >= 1)
                //    {

                //        category_nameh = tmp.Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
                //    }

                //    EasyAsk.GetBrandAndModelProducts(category_nameh, Request.QueryString["tsm"].ToString(), Request.QueryString["tsb"].ToString(), iRecordsPerPage.ToString(), (iPageNo - 1).ToString(), "Next");
                //}

                string requrl = HttpContext.Current.Request.Url.ToString().ToLower();
                if ((requrl.Contains("categorylist.aspx")))
                {

                    _byp = Request.QueryString["byp"].ToString();  
                    if (_bypcat == null)
                    {
                        dscat = (DataSet)HttpContext.Current.Session["Category_Attributes"];
                       // DataTable result = null; //Declare a dataSet to be filled.

                        // Sort data
                       if (HttpContext.Current.Request.QueryString["cid"].ToString().ToUpper() == "WESNEWS")
                       {

                           DataRow[] R1, R2;
                           DataTable Tmptbl  = dscat.Tables[0].Clone();  
                           R1 = dscat.Tables[0].Select("CUSTOM_TEXT_FIELD1_VALUE<>''", "CUSTOM_TEXT_FIELD1 desc", DataViewRowState.CurrentRows);

                           R2 = dscat.Tables[0].Select("CUSTOM_TEXT_FIELD1_VALUE=''", "CATEGORY_ID desc", DataViewRowState.CurrentRows);

                           foreach (DataRow  dr in R1)
                           {
                               Tmptbl.Rows.Add(dr.ItemArray)  ;
                           }
                            foreach (DataRow  dr in R2)
                           {
                               Tmptbl.Rows.Add(dr.ItemArray)  ;
                           }

                            //dscat.Tables[0].DefaultView.Sort = "CUSTOM_TEXT_FIELD1 desc";
                       
                        // Store in new Dataset
                       // result.Tables.Add(dscat.Tables[0].DefaultView.ToTable());

                       // result = dscat.Tables[0].DefaultView.ToTable();
                        
    
                        dscat.Tables.Remove("Category");
                        dscat.Tables.Add(Tmptbl);
                       }
                    }
                    else if (_tsb != "")
                    {
                        dscat = null;

                        //dscat = (DataSet)HttpContext.Current.Session["WESBrand_Model_Attributes"];

                    }
                    
                }
                else if ((requrl.Contains("productdetails.aspx")))
                {                    
                    //dscat = (DataSet)HttpContext.Current.Session["Category_Attributes"];
                    //if (dscat == null)
                    //{
                    //    EasyAsk.GetMainMenuClickDetail(_parentCatID, "");
                    //    dscat = (DataSet)HttpContext.Current.Session["Category_Attributes"];
                    //}
                    dscat = null;
  
                }
                else
                {
                    dscat = (DataSet)HttpContext.Current.Session["LHSAttributes"];
                }

                if ((requrl.Contains("powersearch.aspx")) && dscat != null && dscat.Tables.Count > 0)
                {
                    if (HttpContext.Current.Session["BreadCrumbDS"] != null)
                    {
                        DataSet breadcrumbPS = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];

                        if (breadcrumbPS != null && breadcrumbPS.Tables.Count > 0 && breadcrumbPS.Tables[0].Rows.Count == 1)
                        {
                            Session["LHScatPS"] = dscat;
                            Session["LHSsubcatPS"] = null;

                        }
                        else if (breadcrumbPS != null && breadcrumbPS.Tables.Count > 0 && breadcrumbPS.Tables[0].Rows.Count == 2 && breadcrumbPS.Tables[0].Rows[1]["ItemType"].ToString().ToLower().Contains("category"))
                        {
                            Session["LHSsubcatPS"] = dscat;
                            Session["subcatshow"] = "true";
                        }
                        else if (breadcrumbPS != null && breadcrumbPS.Tables.Count > 0 && breadcrumbPS.Tables[0].Rows.Count == 3 && breadcrumbPS.Tables[0].Rows[1]["ItemType"].ToString().ToLower().Contains("category") && breadcrumbPS.Tables[0].Rows[2]["ItemType"].ToString().ToLower().Contains("category"))
                        {
                            Session["LHSsubcatPS"] = (DataSet)HttpContext.Current.Session["LHSsubcatPS"];
                           // Session["LHSsubcatPS"] = dscat;
                            Session["subcatshow"] = "true";
                        }
                        else
                        {
                            // Session["LHSsubcatPS"] = null;  
                            Session["subcatshow"] = "false";
                        }
                       
                    }
                }

               // modify by palani
                if ((requrl.Contains("bybrand.aspx")) && dscat != null)
                {
                    if (HttpContext.Current.Session["BreadCrumbDS"] != null)
                    {
                        DataSet breadcrumb = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];
                        string bcitemtype = breadcrumb.Tables[0].Rows[0]["ItemType"].ToString().ToLower();
                        if (bcitemtype.Contains("brand"))
                        {
                            if (breadcrumb != null && breadcrumb.Tables.Count > 0 && breadcrumb.Tables[0].Rows.Count == 2 && bcitemtype.Contains("brand"))                        
                            {
                                Session["dscatBrandModel"] = _tsb + "," + _tsm;
                                Session["dscatname"] = _value;
                                Session["dscatbybrand"] = dscat;
                                Session["dssubcatbybrand"] = null;
                            }
                            if (breadcrumb != null && breadcrumb.Tables.Count > 0 && breadcrumb.Tables[0].Rows.Count == 3 && bcitemtype.Contains("brand"))
                            {
                                if (breadcrumb.Tables[0].Rows[2]["ItemType"].ToString() == "Category")
                                    Session["dssubcatbybrand"] = dscat;
                            }
                        }
                        else
                        {
                            if (breadcrumb != null && breadcrumb.Tables.Count > 0 && breadcrumb.Tables[0].Rows.Count == 3)                        
                            {
                                Session["dscatBrandModel"] = _tsb + "," + _tsm;
                                Session["dscatname"] = _value;
                                Session["dscatbybrand"] = dscat;
                                Session["dssubcatbybrand"] = null;
                            }
                            if (breadcrumb != null && breadcrumb.Tables.Count > 0 && breadcrumb.Tables[0].Rows.Count == 4 )
                            {
                                if (breadcrumb.Tables[0].Rows[3]["ItemType"].ToString() == "Category")
                                    Session["dssubcatbybrand"] = dscat;
                            }
                        }
                    }


                }
                _stg_records = new StringTemplateGroup("searchrsltcategoryleftrecords", stemplatepath);
                _stg_container = new StringTemplateGroup("searchrsltcategoryleftmain", stemplatepath);

               //string requrl = HttpContext.Current.Request.Url.ToString().ToLower();
                string stmplrecords = string.Empty;
                if (dscat != null)
                {
                    if ((requrl.Contains("categorylist.aspx")))
                    {
                        stmplrecords="searchrsltcategoryleft\\cell";
                    }
                    else if ((requrl.Contains("bybrand.aspx")))
                    {
                        stmplrecords="searchrsltcategoryleft\\cell1";
                    }
                    else if ((requrl.Contains("powersearch.aspx")))
                    {
                        stmplrecords="searchrsltcategoryleft\\cell2";
                    }
                    else if ((requrl.Contains("family.aspx")))
                    {
                        stmplrecords="searchrsltcategoryleft\\cell3";
                    }
                    else
                    {
                       stmplrecords="searchrsltcategoryleft\\cell";
                    }
                    if (dscat.Tables.Count > 0)
                        lstrows = new TBWDataList[dscat.Tables.Count + 1];

                    for (int i = 0; i < dscat.Tables.Count; i++)
                    {
                        Boolean tmpallow = true;
                        //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("categorylist.aspx") == true && Request.QueryString["tsb"]!=null  && Request.QueryString["tsb"].ToString()!="")
                        //{ 
                        //    if (dscat.Tables[i].TableName.Contains("Model"))
                        //        tmpallow = true;
                        //    else
                        //        tmpallow = false;
                        //}
                        //else 
                        if ((requrl.Contains("categorylist.aspx")) || (requrl.Contains("productdetails.aspx")))
                        {
                            if (dscat.Tables[i].TableName.Contains("Category"))
                                tmpallow = true;
                            else if (dscat.Tables[i].TableName.Contains("Brand"))
                                tmpallow = false;
                                //Commented by indu on 6/6/2016 Reason:dispalying all filter option in category page
                            //else if (Request.QueryString["byp"] == "2")
                            //    tmpallow = true;
                            else
                                tmpallow = false;
                        }
                        if ((tmpallow))
                        {
                            
                            if (dscat.Tables[i].Rows.Count > 0)
                            {
                                lstrecords = new TBWDataList[dscat.Tables[i].Rows.Count + 1];
                                lstrecords1 = new TBWDataList1[dscat.Tables[i].Rows.Count + 1];
                                int ictrecords = 0;

                                int j = 0;
                                foreach (DataRow dr in dscat.Tables[i].Rows)//For Records
                                {

                                    _stmpl_records = _stg_records.GetInstanceOf(stmplrecords);
                                    bool bindtost = false;
                                    if (dscat.Tables[i].TableName.Contains("Category"))
                                    {
                                        if (dr["CATEGORY_ID"].ToString().Contains("SPF-") == false)
                                        {
                                            bindtost = true;
                                            _stmpl_records.SetAttribute("TBW_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(_parentCatID.ToString()));
                                            _stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(dr["CATEGORY_ID"].ToString()));
                                            //_stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(ParentCatID.ToString()));

                                            _stmpl_records.SetAttribute("TBW_ATTRIBUTE_NAME", dr["Category_Name"]);
                                            _stmpl_records.SetAttribute("TBW_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(dr["Category_Name"].ToString()));
                                            _stmpl_records.SetAttribute("TBW_ATTRIBUTE_BRAND", HttpUtility.UrlEncode(dr["brandvalue"].ToString()));
                                            _stmpl_records.SetAttribute("TBW_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(dr["SearchString"].ToString()));
                                            _stmpl_records.SetAttribute("TBW_PRO_CNT", dr["Product_Count"]);
                                            if (_parentCatID == WesNewsCategoryId)
                                                _stmpl_records.SetAttribute("TBW_OPTION_CATEGORY_ID", "<br/>" + dr["CATEGORY_ID"]);
                                            else
                                                _stmpl_records.SetAttribute("TBT_CATEGORY_ID_DIV", "");
                                        }
                                    }
                                    else
                                    {
                                        //_stmpl_records.SetAttribute("TBW_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(ParentCatID.ToString()));
                                        //_stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(dr["CATEGORY_ID"].ToString()));
                                        _stmpl_records.SetAttribute("TBW_PRO_CNT", dr["Product_Count"]);
                                        _stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(_catCid.ToString()));
                                        _stmpl_records.SetAttribute("TBW_ATTRIBUTE_NAME", dr[0].ToString());
                                        _stmpl_records.SetAttribute("TBW_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(dr[0].ToString()));
                                        _stmpl_records.SetAttribute("TBW_ATTRIBUTE_BRAND", HttpUtility.UrlEncode(dr["brandvalue"].ToString()));
                                        _stmpl_records.SetAttribute("TBW_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(dr["SearchString"].ToString()));


                                    }
                                    

                                    _stmpl_records.SetAttribute("TBW_CUSTOM_NUM_FIELD3", _byp);
                                    _stmpl_records.SetAttribute("TBW_BRAND", HttpUtility.UrlEncode(_tsb));
                                    _stmpl_records.SetAttribute("TBW_MODEL", HttpUtility.UrlEncode(_tsm));
                                    _stmpl_records.SetAttribute("TBW_FAMILY_ID", _fid);

                                    _stmpl_records.SetAttribute("TBW_ATTRIBUTE_TYPE", dscat.Tables[i].TableName.ToString());
                                    if (HttpContext.Current.Session["EA"] != null)
                                    {
                                        _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(HttpContext.Current.Session["EA"].ToString())));
                                    }
                                    if ((requrl.Contains("categorylist.aspx")) || (requrl.Contains("productdetails.aspx")))
                                    {
                                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                                    }
                                    else
                                    {
                                        if ((dscat.Tables[i].TableName.Contains("Category")))
                                        {
                                            if (bindtost == true)
                                            {
                                                lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                                            }
                                        }
                                        else
                                        {
                                            if (ictrecords <= 4)
                                            {
                                                lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                                            }
                                            else
                                            {
                                                lstrecords1[ictrecords] = new TBWDataList1(_stmpl_records.ToString());
                                            }
                                        }

                                    }
                                    ictrecords++;
                                }

                                j++;
                                //if (dscat_full.Tables[i].Rows.Count > 0)
                                //{
                                //    _stmpl_recordsrows.SetAttribute("TBW_LINK", "<h3 class=expand id='" + dscat.Tables[i].TableName.ToString().Replace("/", "").Replace(" ", "") + "1' onclick=showHide('" + dscat.Tables[i].TableName.ToString().Replace("/", "").Replace(" ", "") + "');return false;>Show More Options</h3>");
                                //}
                                if ((requrl.Contains("categorylist.aspx")) || (requrl.Contains("productdetails.aspx")))
                                    _stmpl_recordsrows = _stg_container.GetInstanceOf("searchrsltcategoryleft\\row1");
                                else
                                {
                                    if ((dscat.Tables[i].TableName.Contains("Category")))
                                    {
                                        _stmpl_recordsrows = _stg_container.GetInstanceOf("searchrsltcategoryleft\\row1");
                                    }
                                    else
                                    {
                                        if (dscat.Tables[i].Rows.Count > 5)
                                            _stmpl_recordsrows = _stg_container.GetInstanceOf("searchrsltcategoryleft\\row");
                                        else
                                            _stmpl_recordsrows = _stg_container.GetInstanceOf("searchrsltcategoryleft\\row1");
                                    }
                                }

                                //}
                                //else
                                //{
                                //    _stmpl_recordsrows = _stg_container.GetInstanceOf("searchrsltcategoryleft" + "\\" + "row1");
                                //}
                                _stmpl_recordsrows.SetAttribute("TBT_ATTRIBUTE_TITLE", dscat.Tables[i].TableName.ToString());
                                _stmpl_recordsrows.SetAttribute("TBT_ATTRIBUTE_ID", dscat.Tables[i].TableName.ToString().Replace("/", "").Replace(" ", ""));
                                _stmpl_recordsrows.SetAttribute("TBT_MENU_ID", (i + 1).ToString());
                                _stmpl_recordsrows.SetAttribute("TBWDataList", lstrecords);
                                _stmpl_recordsrows.SetAttribute("TBWDataList1", lstrecords1);
                                lstrows[ictrows] = new TBWDataList(_stmpl_recordsrows.ToString());
                                ictrows++;
                            }
                        }
                    }
                }
                // You Have Select
                DataSet ds = new DataSet();
                int cnt = 0;
                ds = null;
                if (HttpContext.Current.Session["BreadCrumbDS"] != null)
                {
                    ds = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];

                }
                string YHSCell = "searchrsltcategoryleft\\YHSCell";
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    lstrows1 = new TBWDataList1[ds.Tables[0].Rows.Count+1];

                    string rowitemtypelwr = string.Empty;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        _stmpl_records = _stg_records.GetInstanceOf(YHSCell);
                        _stmpl_records.SetAttribute("TBT_REMOVEEAPATH",  HttpUtility.UrlEncode(objSecurity.StringEnCrypt(row["RemoveEAPath"].ToString())));
                        _stmpl_records.SetAttribute("TBT_REMOVEURL", row["RemoveUrl"]);
                        
                        _stmpl_records.SetAttribute("TBW_ITEM_TYPE", "Item " + row["ItemType"]);
                         rowitemtypelwr = row["ItemType"].ToString().ToLower();

                        if (rowitemtypelwr == "family")
                            _stmpl_records.SetAttribute("TBW_ITEM_NAME", row["FamilyName"]);
                        else if (rowitemtypelwr == "product")
                        {
                            if (row["ProductCode"].ToString() != "")
                            {
                                _stmpl_records.SetAttribute("TBW_ITEM_NAME", row["ProductCode"]);
                            }
                            else
                            {
                                _stmpl_records.SetAttribute("TBW_ITEM_NAME", row["ItemValue"]);
                            }
                        }
                        else
                            _stmpl_records.SetAttribute("TBW_ITEM_NAME", row["ItemValue"]);

                        _stmpl_records.SetAttribute("TBT_URL", row["Url"]);
                        _stmpl_records.SetAttribute("TBT_EAPATH",  HttpUtility.UrlEncode(objSecurity.StringEnCrypt(row["EAPath"].ToString())));

                        lstrows1[cnt] = new TBWDataList1(_stmpl_records.ToString());
                        cnt = cnt + 1;
                        
                    }

                }
                sBrandAndModelHTML = string.Empty;
                string requrlorg = Request.Url.OriginalString.ToLower();
                if (requrlorg.Contains("categorylist.aspx"))
                {
                    if (_parentCatID != WesNewsCategoryId && _byp=="2") // Not WES News
                        sBrandAndModelHTML=ST_bybrand();

                }
                //if (Request.Url.OriginalString.ToLower().Contains("categorylist.aspx"))
                //{
                //    //if (_parentCatID != WesNewsCategoryId && _byp == "2") // Not WES News
                //        sCablefilterHTML  = ST_CablePlug1();

                //}
                if (requrlorg.Contains("categorylist.aspx") && _tsb != "" && _bypcat != null)
                {
                    if (_parentCatID != WesNewsCategoryId && _byp == "2") // Not WES News
                        sModelListHTML = ST_BrandAndModel();
                }

                

                // You Have Select
                _stmpl_container = _stg_container.GetInstanceOf("searchrsltcategoryleft\\main");
                //_stmpl_container.SetAttribute("Selection", updateNavigation());
                _stmpl_container.SetAttribute("TBWDataList", lstrows);
                _stmpl_container.SetAttribute("BRAND_AND_MODEL_HTML", sBrandAndModelHTML);
                //_stmpl_container.SetAttribute("CABLE_FILTER", sCablefilterHTML);
                _stmpl_container.SetAttribute("MODEL_HTML", sModelListHTML);
                _stmpl_container.SetAttribute("TBWDataList1", lstrows1);  //youer current Selection  
                sHTML += _stmpl_container.ToString();

                //stopwatch.Stop();
               // objErrorHandler.CreateLog("ST_categories function load time:" + "=" + stopwatch.Elapsed);
            }
            
            catch (Exception ex)
            {
                sHTML = ex.Message;
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            finally
            {
                
            }
       
         
       // return objHelperServices.StripWhitespace(  sHTML);
         return sHTML;
    }
    

   
    private string GetCName(string catID)
    {
        try
        {
            DataSet DSBC = null;
            string catIDtemp = catID;
            do
            {
                //DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
                DSBC = (DataSet)objHelperDB.GetGenericPageDataDB(catIDtemp, "GET_CATEGORYLIST_CAREGORY", HelperDB.ReturnType.RTDataSet);
                if (DSBC != null)
                {
                    foreach (DataRow DR in DSBC.Tables[0].Rows)
                    {
                        catIDtemp = DR["PARENT_CATEGORY"].ToString();
                        if (catIDtemp == "0")
                        {
                            // DSUBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY = '" + DR["CATEGORY_ID"].ToString() + "' AND CATEGORY_NAME Like 'Product'");
                            return DR["CATEGORY_NAME"].ToString();
                        }
                    }
                }
            } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
            return DSBC.Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
        }
        catch(Exception ex)
        {
        }
         return "";
    }


    protected string ST_BrandAndModel()
    {

        string sHTML = string.Empty;
        try
        {


            StringTemplateGroup _stg_container = null;
            StringTemplateGroup _stg_records = null;
            StringTemplate _stmpl_container = null;
            StringTemplate _stmpl_records = null;
         //   StringTemplate _stmpl_records1 = null;
            StringTemplate _stmpl_recordsrows = null;
            TBWDataList[] lstrecords = new TBWDataList[0];
            TBWDataList[] lstrows = new TBWDataList[0];

           // StringTemplateGroup _stg_container1 = null;
           // StringTemplateGroup _stg_records1 = null;
            TBWDataList1[] lstrecords1 = new TBWDataList1[0];
            TBWDataList1[] lstrows1 = new TBWDataList1[0];

            stemplatepath = Server.MapPath(ConfigurationManager.AppSettings["StringTemplatePath"].ToString());
            int ictrows = 0;
            string _bypcat = null;
            _bypcat = Request.QueryString["bypcat"];
            DataSet dscat = new DataSet();
            DataTable dt = null;
            //if ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("categorylist.aspx") == true && _bypcat == null) || HttpContext.Current.Request.Url.ToString().ToLower().Contains("product_list.aspx") == true)
            //{
            //    dt = (DataTable)((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["Brand"];
            //    if (dt == null)
            //        dscat = null;
            //    else
            //        dscat.Tables.Add(dt.Copy());
            //}
            //else
            //{
                dscat = (DataSet)HttpContext.Current.Session["WESBrand_Model"];
            //}






            if (dscat == null)
                return "";
            _stg_records = new StringTemplateGroup("searchrsltcategoryleftrecords", stemplatepath);
            _stg_container = new StringTemplateGroup("searchrsltcategoryleftmain", stemplatepath);
            if (dscat.Tables.Count > 0)
                lstrows = new TBWDataList[dscat.Tables.Count + 1];

            for (int i = 0; i < dscat.Tables.Count; i++)
            {
                Boolean tmpallow = true;
                //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("categorylist.aspx") == true)
                //{
                //if (dscat.Tables[i].TableName.Contains("Brand") == true && Request.QueryString["byp"] == "2")
                //    tmpallow = true;
                //else
                //    tmpallow = false;
                //}
                if ((dscat.Tables[i].TableName.Contains("Model")))
                    tmpallow = true;
                else
                    tmpallow = false;

                if ((tmpallow))
                {
                    if (dscat.Tables[i].Rows.Count > 0)
                    {
                        lstrecords = new TBWDataList[dscat.Tables[i].Rows.Count + 1];

                        int ictrecords = 0;

                        int j = 0;
                        foreach (DataRow dr in dscat.Tables[i].Rows)//For Records
                        {


                            //if ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("categorylist.aspx") == true && _bypcat == null) || HttpContext.Current.Request.Url.ToString().ToLower().Contains("product_list.aspx") == true)
                            //{
                            //    _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryright" + "\\" + "cell");
                            //    _stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(ParentCatID.ToString()));
                            //    _stmpl_records.SetAttribute("TBW_BRAND", dr["TOSUITE_BRAND"].ToString());

                            //}
                            //else
                            //{
                                _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryright" + "\\" + "cell2");
                                _stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(_parentCatID.ToString()));
                                _stmpl_records.SetAttribute("TBW_BRAND", Server.UrlEncode(Request.QueryString["tsb"]));
                                _stmpl_records.SetAttribute("TBW_MODEL", HttpUtility.UrlEncode(dr["TOSUITE_MODEL"].ToString()));
                                _stmpl_records.SetAttribute("TBW_MODEL_NAME", dr["TOSUITE_MODEL"].ToString());

                            //}
                            if (HttpContext.Current.Session["EA"] != null)
                            {
                                _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(HttpContext.Current.Session["EA"].ToString())));
                            }

                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                            ictrecords++;
                        }

                        j++;

                        _stmpl_recordsrows = _stg_container.GetInstanceOf("searchrsltcategoryright" + "\\" + "row1");
                        //if ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("categorylist.aspx") == true && _bypcat == null) || HttpContext.Current.Request.Url.ToString().ToLower().Contains("product_list.aspx") == true)
                         //   _stmpl_recordsrows.SetAttribute("TBT_ATTRIBUTE_TITLE", dscat.Tables[i].TableName.ToString());
                        //else
                            _stmpl_recordsrows.SetAttribute("TBT_ATTRIBUTE_TITLE", "MODEL");

                        _stmpl_recordsrows.SetAttribute("TBWDataList", lstrecords);
                        lstrows[ictrows] = new TBWDataList(_stmpl_recordsrows.ToString());
                        ictrows++;
                    }
                }
            }

            _stmpl_container = _stg_container.GetInstanceOf("searchrsltcategoryright" + "\\" + "main");
            //_stmpl_container.SetAttribute("Selection", updateNavigation());
            _stmpl_container.SetAttribute("TBWDataList", lstrows);
            sHTML += _stmpl_container.ToString();
        }

        catch (Exception ex)
        {
            sHTML = ex.Message;
        }
        finally
        {

        }


        return sHTML;
    }

       protected string ST_bybrand()
    {
        StringTemplateGroup _stg_main_container = null;
        StringTemplateGroup _stg_records_container = null;
        StringTemplateGroup _stg_records = null;
        StringTemplate _stmpl_main_container_tmpl = null;
        StringTemplate _stmpl_records_container_tmpl = null;
        StringTemplate _stmpl_records_tmpl = null;
        //StringTemplate _stmpl_records_tmpl2 = null;
        //StringTemplate _stmpl_records_tmpl3 = null;
        TBWDataList[] lstrecords = new TBWDataList[0];
        TBWDataList[] lstrows = new TBWDataList[0];
        TBWDataList[] lstcontainers = new TBWDataList[3];
        DataSet dsCat;
        string[] filterval = null;
        string[] filterval1 = null;
        string[] filterval2 = null;
        
        //oPR = new ProductRender();
        string sHTML = string.Empty;
      //  string dropdowncatid = "";
      //  string _catid = "";
      //  string _fid = "";
        int ictrecords = 0;


        try
        {
            stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\bybrand\\";


            if (hidcatIds.Value != string.Empty && hidcatIds.Value != null)
            {
                filterval = hidcatIds.Value.Split('^');
            }
            if (HidsubcatIds.Value != string.Empty && HidsubcatIds.Value != null)
            {
                filterval1 = HidsubcatIds.Value.Split('^');
            }
            if (HidsubcatIds1.Value != string.Empty && HidsubcatIds1.Value != null)
            {
                filterval2 = HidsubcatIds1.Value.Split('^');
            }

           // if (Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "")//&& Request.QueryString["cid"].ToString() == "WES210582")
            //{
                //string cid = Request.QueryString["cid"].ToString();

                //tempCID = Request.QueryString["cid"].ToString();
                //tempCName = GetCName(tempCID);
                dsCat = new DataSet();

                if (((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["Brand"] == null )
                    return "";
  
                dsCat.Tables.Add((DataTable)((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["Brand"].Copy());
                
                
                if (dsCat == null || dsCat.Tables.Count == 0 || dsCat.Tables[0].Rows.Count == 0)
                    return "";


                if (dsCat != null && dsCat.Tables.Count > 0 && (dsCat.Tables[0].Rows.Count > 0))
                {
                    tempCID = Request.QueryString["cid"].ToString();
                    ictrecords = 0;
                    lstrecords = new TBWDataList[dsCat.Tables[0].Rows.Count + 1];
                    _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                    _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                    _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");

                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "Select Brand");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "Select Brand");
                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                    ictrecords++;
                    bool selstate = false;
                    foreach (DataRow _drow in dsCat.Tables[0].Rows)
                    {
                        _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["TOSUITE_BRAND"]);
                        if (filterval != null && _drow["TOSUITE_BRAND"].ToString() == filterval[0].ToString())
                        {
                            _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                            selstate = true;
                            string eapath="";
                            //eapath = HttpContext.Current.Session["EA"].ToString();
                            //if (eapath.Contains("////AttribSelect=Brand"))
                            //{
                            //    int inx = eapath.IndexOf("////AttribSelect=Brand");

                            //    eapath = eapath.ToString().Substring(0,inx);

                                
                            //}                            
                            //eapath = eapath + "////AttribSelect=Brand='" + _drow["TOSUITE_BRAND"].ToString() + "'";

                            eapath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_drow["EA_PATH"].ToString()));
                            Response.Redirect("categorylist.aspx?&ld=0&cid=" + tempCID + "&tsb=" + Server.UrlEncode(_drow["TOSUITE_BRAND"].ToString()) + "&byp=2&bypcat=1&path=" + eapath, false);
                        }
                        else if (filterval == null && Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "" && _drow["TOSUITE_BRAND"].ToString() == Server.UrlDecode(Request.QueryString["tsb"].ToString()) && !(selstate))
                        {
                            filterval = new string[2];
                            filterval[0] = _drow["TOSUITE_BRAND"].ToString();
                            filterval[1] = _drow["TOSUITE_BRAND"].ToString();
                            _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                            //Response.Redirect("categorylist.aspx?ld=0&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2&bypcat=1", false);
                        }
                        _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["TOSUITE_BRAND"]);
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                        ictrecords++;

                    }
                    _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                    _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                    _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "1");
                    lstcontainers[0] = new TBWDataList(_stmpl_records_container_tmpl.ToString());

                }
                if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "")
                {
                    tempCID = Request.QueryString["cid"].ToString();
                    bool selstate1 = false;
                    DataSet dsCat1 = new DataSet();
                    //dsCat1 = oCat.GetWESModel(tempCID, Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()), Server.UrlDecode(Request.QueryString["tsb"].ToString()));
                    //dsCat1 = EasyAsk.GetWESModel(tempCName, Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()), Server.UrlDecode(Request.QueryString["tsb"].ToString())); 
                    dsCat1 = (DataSet)HttpContext.Current.Session["WESBrand_Model"];
                    if (dsCat1 != null && dsCat1.Tables.Count > 0 && (dsCat1.Tables[0].Rows.Count > 0))
                    {
                        ictrecords = 0;
                        DataRow[] _DCRow = null;
                        _DCRow = dsCat1.Tables[0].Select();
                        if (_DCRow != null && _DCRow.Length > 0)
                        {
                            lstrecords = new TBWDataList[_DCRow.Length + 1];
                            _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                            _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                            _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all models");
                            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all models");
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                            ictrecords++;
                            foreach (DataRow _drow in _DCRow)
                            {
                                _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["TOSUITE_MODEL"].ToString());
                                if (filterval1 != null && _drow["TOSUITE_MODEL"].ToString() == filterval1[0].ToString())
                                {
                                    _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
                                    selstate1 = true;
                                    //Response.Redirect("product_list.aspx?&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2", false);
                                    string eapath = string.Empty;
                                    //eapath = HttpContext.Current.Session["EA"].ToString();
                                    //if (eapath.Contains("////AttribSelect=Model"))
                                    //{
                                    //    int inx = eapath.IndexOf("////AttribSelect=Model");

                                    //    eapath = eapath.ToString().Substring(0, inx );

                                        
                                    //}
                                    //eapath = eapath + "////AttribSelect=Model = '" + Request.QueryString["tsb"].ToString() + ":" + _drow["TOSUITE_MODEL"].ToString() + "'";

                                    eapath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_drow["EA_PATH"].ToString()));
                                    Response.Redirect("bybrand.aspx?&id=0&cid=" + tempCID + "&tsb=" + Server.UrlEncode(Request.QueryString["tsb"].ToString()) + "&tsm=" + Server.UrlEncode(_drow["TOSUITE_MODEL"].ToString()) + "&byp=2&bypcat=1&path=" + eapath, false);
                                }
                                else if (filterval1 == null && Request.QueryString["tsm"] != null && _drow["TOSUITE_MODEL"].ToString() == Server.UrlDecode(Request.QueryString["tsm"].ToString()) && !(selstate1))
                                {
                                    filterval1 = new string[2];
                                    filterval1[0] = _drow["TOSUITE_MODEL"].ToString();
                                    filterval1[1] = _drow["TOSUITE_MODEL"].ToString();
                                    _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");

                                }
                                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["TOSUITE_MODEL"]);
                                lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                                ictrecords++;

                            }
                            _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                            _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                            _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
                            lstcontainers[1] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                        }
                    }
                    dsCat1.Dispose();
                }
                else
                {
                    lstrecords = new TBWDataList[1];
                    _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                    _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                    _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all models");
                    _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all models");
                    lstrecords[0] = new TBWDataList(_stmpl_records_tmpl.ToString());
                    _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
                    _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                    _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
                    lstcontainers[1] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
                }

              

           // }
            //}
            _stg_main_container = new StringTemplateGroup("searchrsltmultilistmain", stemplatepath);
            _stmpl_main_container_tmpl = _stg_main_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistmain");
            _stmpl_main_container_tmpl.SetAttribute("TBW_CATEGORY_NAME", tempCName);
            _stmpl_main_container_tmpl.SetAttribute("TBWDataList", lstcontainers);
            sHTML = _stmpl_main_container_tmpl.ToString();
        }

        catch (Exception ex)
        {
            sHTML = ex.Message;
        }
        return sHTML;
    }

       protected string ST_CablePlug1()
       {
           StringTemplateGroup _stg_main_container = null;
           StringTemplateGroup _stg_records_container = null;
           StringTemplateGroup _stg_records = null;
           StringTemplate _stmpl_main_container_tmpl = null;
           StringTemplate _stmpl_records_container_tmpl = null;
           StringTemplate _stmpl_records_tmpl = null;
           //StringTemplate _stmpl_records_tmpl2 = null;
           //StringTemplate _stmpl_records_tmpl3 = null;
           TBWDataList[] lstrecords = new TBWDataList[0];
           TBWDataList[] lstrows = new TBWDataList[0];
           TBWDataList[] lstcontainers = new TBWDataList[3];
           DataSet dsCat;
         //  string[] filterval = null;
         //  string[] filterval1 = null;
        //   string[] filterval2 = null;

           //oPR = new ProductRender();
           string sHTML = string.Empty;
         //  string dropdowncatid = "";
          // string _catid = "";
          // string _fid = "";
           int ictrecords = 0;


           try
           {
               stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) + "\\bybrand\\";


            
               dsCat = new DataSet();

               dsCat = (DataSet)HttpContext.Current.Session["LHSAttributes"];

               if (dsCat == null || dsCat.Tables.Count == 0 || dsCat.Tables["Plug 1"] == null || dsCat.Tables["Plug 1"].Rows.Count ==0)
                   return "";


               if (dsCat != null && dsCat.Tables.Count > 0 && (dsCat.Tables["Plug 1"].Rows.Count > 0))
               {
                   
                   ictrecords = 0;
                   lstrecords = new TBWDataList[dsCat.Tables["Plug 1"].Rows.Count + 1];
                   _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
                   _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
                   _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchCable" + "\\" + "multilistitem");

                   _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "Select Plug1");
                   _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "Select Plug1");
                   lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                   ictrecords++;
                   bool selstate = false;
                   foreach (DataRow _drow in dsCat.Tables["Plug 1"].Rows)
                   {
                       _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchCable" + "\\" + "multilistitem");
                       _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow[0].ToString());                                              
                       _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow[0].ToString());
                       lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
                       ictrecords++;

                   }
                   _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchCable" + "\\" + "multilistcontainer");
                   _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
                   _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "Cable1");
                   lstcontainers[0] = new TBWDataList(_stmpl_records_container_tmpl.ToString());

               }
               //if (Request.QueryString["tsb"] != null && Request.QueryString["tsb"].ToString() != "")
               //{
               //    tempCID = Request.QueryString["cid"].ToString();
               //    bool selstate1 = false;
               //    DataSet dsCat1 = new DataSet();
               //    //dsCat1 = oCat.GetWESModel(tempCID, Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()), Server.UrlDecode(Request.QueryString["tsb"].ToString()));
               //    //dsCat1 = EasyAsk.GetWESModel(tempCName, Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG").ToString()), Server.UrlDecode(Request.QueryString["tsb"].ToString())); 
               //    dsCat1 = (DataSet)HttpContext.Current.Session["WESBrand_Model"];
               //    if (dsCat1 != null && dsCat1.Tables.Count > 0 && (dsCat1.Tables[0].Rows.Count > 0))
               //    {
               //        ictrecords = 0;
               //        DataRow[] _DCRow = null;
               //        _DCRow = dsCat1.Tables[0].Select();
               //        if (_DCRow != null && _DCRow.Length > 0)
               //        {
               //            lstrecords = new TBWDataList[_DCRow.Length + 1];
               //            _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
               //            _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
               //            _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
               //            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "List all models");
               //            _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "List all models");
               //            lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
               //            ictrecords++;
               //            foreach (DataRow _drow in _DCRow)
               //            {
               //                _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistitem");
               //                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", _drow["TOSUITE_MODEL"].ToString());
               //                if (filterval1 != null && _drow["TOSUITE_MODEL"].ToString() == filterval1[0].ToString())
               //                {
               //                    _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");
               //                    selstate1 = true;
               //                    //Response.Redirect("product_list.aspx?&cid=" + _drow["CATEGORY_ID"].ToString() + "&byp=2", false);
               //                    string eapath = "";
               //                    //eapath = HttpContext.Current.Session["EA"].ToString();
               //                    //if (eapath.Contains("////AttribSelect=Model"))
               //                    //{
               //                    //    int inx = eapath.IndexOf("////AttribSelect=Model");

               //                    //    eapath = eapath.ToString().Substring(0, inx );


               //                    //}
               //                    //eapath = eapath + "////AttribSelect=Model = '" + Request.QueryString["tsb"].ToString() + ":" + _drow["TOSUITE_MODEL"].ToString() + "'";

               //                    eapath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_drow["EA_PATH"].ToString()));
               //                    Response.Redirect("bybrand.aspx?&id=0&cid=" + tempCID + "&tsb=" + Server.UrlEncode(Request.QueryString["tsb"].ToString()) + "&tsm=" + Server.UrlEncode(_drow["TOSUITE_MODEL"].ToString()) + "&byp=2&bypcat=1&path=" + eapath, false);
               //                }
               //                else if (filterval1 == null && Request.QueryString["tsm"] != null && _drow["TOSUITE_MODEL"].ToString() == Server.UrlDecode(Request.QueryString["tsm"].ToString()) && selstate1 == false)
               //                {
               //                    filterval1 = new string[2];
               //                    filterval1[0] = _drow["TOSUITE_MODEL"].ToString();
               //                    filterval1[1] = _drow["TOSUITE_MODEL"].ToString();
               //                    _stmpl_records_tmpl.SetAttribute("TBW_CATEGORY_ID_SELECTED", "SELECTED=\"SELECTED\" ");

               //                }
               //                _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", _drow["TOSUITE_MODEL"].ToString());
               //                lstrecords[ictrecords] = new TBWDataList(_stmpl_records_tmpl.ToString());
               //                ictrecords++;

               //            }
               //            _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchbybrandcategory" + "\\" + "multilistcontainer");
               //            _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
               //            _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "2");
               //            lstcontainers[1] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
               //        }
               //    }
               //    dsCat1.Dispose();
               //}
               //else
               //{
               lstrecords = new TBWDataList[1];
               _stg_records = new StringTemplateGroup("searchrsltmultilistitem", stemplatepath);
               _stg_records_container = new StringTemplateGroup("searchrsltmultilistcontainer", stemplatepath);
               _stmpl_records_tmpl = _stg_records.GetInstanceOf("searchCable" + "\\" + "multilistitem");
               _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL1", "Select All");
               _stmpl_records_tmpl.SetAttribute("TBW_LIST_VAL", "Select All");
               lstrecords[0] = new TBWDataList(_stmpl_records_tmpl.ToString());
               _stmpl_records_container_tmpl = _stg_records_container.GetInstanceOf("searchCable" + "\\" + "multilistcontainer");
               _stmpl_records_container_tmpl.SetAttribute("TBWDataList", lstrecords);
               _stmpl_records_container_tmpl.SetAttribute("TBW_SELECTED_ID", "Cable2");
               lstcontainers[1] = new TBWDataList(_stmpl_records_container_tmpl.ToString());
              // }



               // }
               //}
               _stg_main_container = new StringTemplateGroup("searchrsltmultilistmain", stemplatepath);
               _stmpl_main_container_tmpl = _stg_main_container.GetInstanceOf("searchCable" + "\\" + "multilistmain");
               _stmpl_main_container_tmpl.SetAttribute("TBW_CATEGORY_NAME", tempCName);
               _stmpl_main_container_tmpl.SetAttribute("TBWDataList", lstcontainers);
               sHTML = _stmpl_main_container_tmpl.ToString();
           }

           catch (Exception ex)
           {
               sHTML = ex.Message;
           }
           return sHTML;
       }
}
