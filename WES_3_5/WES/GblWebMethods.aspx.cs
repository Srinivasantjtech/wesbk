using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data ;
using System.IO;
using System.Web.Services;
using System.Collections;
using System.Web.Configuration;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.CommonServices;
using TradingBell.WebCat.Helpers ;
using TradingBell.WebCat.EasyAsk;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
using TradingBell.WebCat.TemplateRender;
using System.Xml;
using Newtonsoft.Json;
using System.Text ;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
public partial class GblWebMethods : System.Web.UI.Page
{

    
    
    
    
    static string strimgPath = HttpContext.Current.Server.MapPath("ProdImages");
    static string EasyAsk_URL = System.Configuration.ConfigurationManager.AppSettings["EasyAsk_URL"].ToString();
    static int EasyAsk_Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EasyAsk_Port"]);    
    static string EasyAsk_WebCatDictionary = System.Configuration.ConfigurationManager.AppSettings["EasyAsk_WebCatDictionary"].ToString();
    static string EasyAsk_WebCatPath = System.Configuration.ConfigurationManager.AppSettings["EA_NEW_PRODUCT_INIT_CATEGORY_PATH"].ToString();
    static string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();
    static int count=0;
    string _parentCatID = "";
     DataSet dsAutoComplete = new DataSet();
        DataSet dsAdvisor = new DataSet();

        [WebMethod]
        public static string GetSearchResultNew1(string Strvalue)
        {
            string  sHTML = "";
            StringBuilder  strhtml =new StringBuilder(50000);

            //HelperServices objHelperServices = new HelperServices();
            //EasyAsk_WES ObjEasyAsk = new EasyAsk_WES();
            //HelperDB objHelperDB = new HelperDB();
            Security objSecurity = new Security();
            HelperDB objHelperDB = new HelperDB();
            DataSet dsAutoComplete = new DataSet();
            DataSet dsAdvisor = new DataSet();
            ErrorHandler objErrorHandler = new ErrorHandler();
             string temp_product_Image = "";
        string temp_fmly_Image = "";
        string image_string = "";
            try
            {
                

                string resultStr = "";
                string eapath="";
                string UserId = "";
                int pricecode = -1;
                if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
                    UserId = HttpContext.Current.Session["USER_ID"].ToString();

                if (UserId == "")
                    UserId = "0";
                pricecode= objHelperDB.GetPriceCode(UserId);
                try
                {
                    System.Net.WebClient myWebClient = new System.Net.WebClient();
                    resultStr = myWebClient.DownloadString("http://" + EasyAsk_URL + ":" + EasyAsk_Port.ToString() + "/EasyAsk/AutoComplete-1.2.1.jsp?fctn=&key=" + HttpUtility.UrlEncode(Strvalue) + "&dct=" + EasyAsk_WebCatDictionary + "&num=10");
                }
                catch { }
                if (resultStr != "")
                {

                    XmlDocument xd = new XmlDocument();
                    resultStr = "{ \"rootNode\": {" + resultStr.Trim().TrimStart('{').TrimEnd('}') + "} }";
                    xd = (XmlDocument)JsonConvert.DeserializeXmlNode(resultStr);
                    dsAutoComplete = new DataSet();
                    dsAutoComplete.ReadXml(new XmlNodeReader(xd));

                    //resultStr = resultStr.Replace("\"", "");

                }

                try
                {
                    System.Net.WebClient myWebClient = new System.Net.WebClient();
                    resultStr = myWebClient.DownloadString("http://" + EasyAsk_URL + ":" + EasyAsk_Port.ToString() + "/EasyAsk/apps/Advisor.jsp?indexed=1&oneshot=1&ie=UTF-8&disp=json&RequestAction=advisor&RequestData=CA_Search&CatPath=" + HttpUtility.UrlEncode(EasyAsk_WebCatPath) + "&defarrangeby=////NONE////&dct=" + HttpUtility.UrlEncode(EasyAsk_WebCatDictionary) + "&q=" + HttpUtility.UrlEncode(Strvalue) + "&ResultsPerPage=1&eap_PriceCode=" +pricecode.ToString() );
                }
                catch { }
                if (resultStr != "")
                {

                    XmlDocument xd = new XmlDocument();
                    resultStr = "{ \"rootNode\": {" + resultStr.Trim().TrimStart('{').TrimEnd('}') + "} }";
                    xd = (XmlDocument)JsonConvert.DeserializeXmlNode(resultStr);
                    dsAdvisor = new DataSet();
                    dsAdvisor.ReadXml(new XmlNodeReader(xd));
                }
                DataTable Sqltb = new DataTable();
                DataColumn dc = null;
                string tmpstr = "";
                Boolean datanotfound=true;
                if (dsAdvisor.Tables["items"] != null)
                {
                    dc = new DataColumn("CATEGORY_PATH", typeof(string));
                    dc.DefaultValue = "";
                    dsAdvisor.Tables["items"].Columns.Add(dc);

                    dc = new DataColumn("CATEGORY_ID", typeof(string));
                    dc.DefaultValue = "";
                    dsAdvisor.Tables["items"].Columns.Add(dc);

                    if (dsAdvisor.Tables["items"].Columns["Prod_Thumbnail"] == null)
                    {
                        dc = new DataColumn("Prod_Thumbnail", typeof(string));
                        dc.DefaultValue = "";
                        dsAdvisor.Tables["items"].Columns.Add(dc);
                    }
                    if (dsAdvisor.Tables["items"].Columns["Family_Thumbnail"] == null)
                    {
                        dc = new DataColumn("Family_Thumbnail", typeof(string));
                        dc.DefaultValue = "";
                        dsAdvisor.Tables["items"].Columns.Add(dc);
                    }
                    if (dsAdvisor.Tables["items"].Columns["Prod_Description"] == null)
                    {
                        dc = new DataColumn("Prod_Description", typeof(string));
                        dc.DefaultValue = "";
                        dsAdvisor.Tables["items"].Columns.Add(dc);
                    }

                    //foreach (DataRow dr1 in dsAdvisor.Tables["items"].Rows)
                    //{
                    //    if (tmpstr.Contains(dr1["FAMILY_ID"].ToString().ToUpper()) == false)
                    //        tmpstr = tmpstr + "'" + dr1["FAMILY_ID"].ToString().ToUpper() + "',";
                    //}


                    //if (tmpstr != "")
                    //    tmpstr = tmpstr.Substring(0, tmpstr.Length - 1) + "";

                    //if (tmpstr != "")
                    //{
                    //    //Sqltb = objhelper.GetDataTable(StrSql);
                    //    Sqltb = (DataTable)objHelperDB.GetGenericDataDB(WesCatalogId, tmpstr, "GET_FAMILY_CATEGORY", HelperDB.ReturnType.RTTable);
                    //    if (Sqltb != null)
                    //    {
                    //        foreach (DataRow dr in Sqltb.Rows)
                    //        {
                    //            foreach (DataRow dr1 in dsAdvisor.Tables["items"].Rows)
                    //            {
                    //                if (dr["FAMILY_ID"].ToString().ToUpper() == dr1["FAMILY_ID"].ToString().ToUpper())
                    //                {
                    //                    dr1["CATEGORY_PATH"] = dr["CATEGORY_PATH"];
                    //                    dr1["CATEGORY_ID"] = dr["SubCatID"];
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                } 

                strhtml.Append("<table><tr>");
                if (dsAdvisor.Tables["categoryList"] != null || dsAdvisor.Tables["Attribute"] != null)               
                    strhtml.Append("<td valign='top' style='width:75%'>");                
                else
                    strhtml.Append("<td valign='top' style='width:100%'>");
                




                
                if (dsAutoComplete != null & dsAutoComplete.Tables.Count >= 2)
                {
                     strhtml.Append("<div class='clear'></div>");
                    strhtml.Append("<div class='viewmoresmall'><strong>Search suggestions for "+ Strvalue + "</strong></div>");
                    strhtml.Append("<div class='clear'></div>");
                    strhtml.Append("<ul>");
                    foreach (DataRow dr in dsAutoComplete.Tables[1].Rows)
                    {
                        datanotfound=false;
                        tmpstr=dr["val"].ToString().ToUpper();                        
                        tmpstr =tmpstr.Replace(Strvalue.ToUpper(), "<strong>" + Strvalue.ToUpper() + "</strong>");

                        strhtml.Append("<li><a href='/powersearch.aspx?srctext=" + HttpUtility.UrlEncode(Strvalue) + "' >" + tmpstr + "</a></li>");
                    }
                    strhtml.Append("</ul>");
                }


                //if (dsAdvisor.Tables["items"] != null)
                //{

                //     strhtml.Append("<div class='viewmoresmall'><strong>Products</strong></div>");
                    

                //    foreach (DataRow dr in dsAdvisor.Tables["items"].Rows)
                //    {

                //         eapath = "AllProducts////WESAUSTRALASIA////" + dr["CATEGORY_PATH"].ToString() + "////UserSearch1=Family Id=" + dr["Family_Id"].ToString();
                //        eapath=HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath.ToString()));
                //    temp_product_Image = dr["Prod_Thumbnail"].ToString();
                //    temp_fmly_Image = dr["Family_Thumbnail"].ToString();
                //    if (temp_product_Image != "")
                //        image_string = temp_product_Image.Substring(42);
                //    else if (temp_fmly_Image != "")
                //        image_string = temp_fmly_Image.Substring(42);
                //    else
                //        image_string = "noimage.gif";
                //    //Fil = new FileInfo(strimgPath + image_string);


                //       strhtml.Append("<div class='drop_products'>");
                //       strhtml.Append("<img width='80' height='80' alt='img' src='prodimages\\" + image_string+ "'>");
                //       strhtml.Append("<a href='/productdetails.aspx?pid=" + dr["Prod_Id"].ToString() + "&amp;fid=" + dr["Family_Id"].ToString() + "&amp;cid=" + dr["CATEGORY_ID"].ToString() + "&amp;path=" + eapath + "'><strong>" + dr["Family_name"].ToString() + "</strong></a>");
                //    strhtml.Append("<p>"+  dr["Prod_Description"].ToString() +"</p>");
                //     strhtml.Append("<div style='Color:red'>Code :<strong>" + dr["Prod_Code"].ToString()+"</strong> &nbsp;&nbsp;&nbsp;&nbsp;Price :<strong style='red'> " + dr["Price"].ToString()+"</strong>");
                //     strhtml.Append("</div><div class='clear'></div></div>");

                //    }
                //}
                strhtml.Append("</td>");
                if (dsAdvisor.Tables["categoryList"] != null || dsAdvisor.Tables["Attribute"] != null)
                {
                    strhtml.Append("<td valign='top' style='width:60%'>");

                    if(dsAdvisor.Tables["categoryList"] != null)
                    {

                        eapath = "AllProducts////WESAUSTRALASIA////UserSearch1=" + Strvalue;
                        eapath=HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath.ToString()));
                         strhtml.Append("<div class='viewmoresmall'><strong>Category</strong></div>");
                         strhtml.Append("<ul>");
                         int i = 0;
                         foreach (DataRow dr in dsAdvisor.Tables["categoryList"].Rows)
                         {
                             datanotfound=false;
                             i = i + 1;

                             if (i > 5)
                                 break;
                             strhtml.Append("<li><a href='powersearch.aspx?&amp;id=0&amp;searchstr=" + HttpUtility.UrlEncode(Strvalue) + "&amp;type=Category&amp;value=" + HttpUtility.UrlEncode(   dr["name"].ToString() )+ "&amp;bname=&amp;byp=2&amp;Path=" + eapath + "' style='font-size:10px'>" + dr["name"].ToString() + "</a></li>");
                         }
                         strhtml.Append("</ul>");
                        if (dsAdvisor.Tables["categoryList"].Rows.Count>5)  
                            strhtml.Append("<a style='color:#0099FF;text-decoration:none;font-size:9px;' href='PowerSearch.aspx?srctext="+ HttpUtility.UrlEncode(Strvalue) +"'>View All Results</a>");

                    }

                    if (dsAdvisor.Tables["Attribute"] != null)
                    {
                        foreach (DataRow dr1 in dsAdvisor.Tables["Attribute"].Rows)
                        {
                            if (dr1["name"].ToString().ToUpper().Contains("PRODUCT TAGS") )
                            {
                                DataTable  Datar =dsAdvisor.Tables["AttributeValueList"].Select("attribute_id='" + dr1["attribute_id"] + "'").CopyToDataTable();
                                if (Datar!=null && Datar.Rows.Count>0 )
                                {
                                    eapath = "AllProducts////WESAUSTRALASIA////UserSearch1=" + Strvalue;
                                    eapath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath.ToString()));
                                    strhtml.Append("<div class='viewmoresmall'><strong>" + dr1["name"].ToString() + "</strong></div>");
                                    strhtml.Append("<ul>");
                                    int i = 0;
                                    foreach (DataRow dr in Datar.Rows)
                                    {
                                        datanotfound=false;
                                        i = i + 1;

                                        if (i>5)
                                            break;

                                        strhtml.Append("<li><a href='powersearch.aspx?&amp;id=0&amp;searchstr=" + HttpUtility.UrlEncode(Strvalue) + "&amp;type=" + dr1["name"].ToString() + "&amp;value=" + HttpUtility.UrlEncode(dr["Attributevalue"].ToString()) + "&amp;bname=&amp;byp=2&amp;Path=" + eapath + "' style='font-size:10px'>" + dr["Attributevalue"].ToString() + "</a></li>");
                                        
                                    }

                                    strhtml.Append("</ul>");
                                    if (i > 5)
                                        strhtml.Append("<a style='color:#0099FF;text-decoration:none;font-size:9px;' href='PowerSearch.aspx?srctext=" +  HttpUtility.UrlEncode(Strvalue) + "'>View All Results</a>");

                                }
                               
                                
                                
                              
                            }
                        }
                    }
                }
                strhtml.Append("</td></tr><tr>");
                if (dsAdvisor.Tables["categoryList"] != null || dsAdvisor.Tables["Attribute"] != null)
                    strhtml.Append("<td colspan='2'>");                
                else
                    strhtml.Append("<td>");

                 
                 strhtml.Append("<div class='clear'></div>");
                 strhtml.Append("<a class='viewmore' href='PowerSearch.aspx?srctext=" + HttpUtility.UrlEncode(Strvalue) + "'>View All Results</a>");
                 
                strhtml.Append("<div class='clear'></div>");
                strhtml.Append("</td></tr></table>");

                 if (datanotfound ==true)
                     strhtml =strhtml.Clear();
            }
            catch { }
           
               
            return strhtml.ToString().Trim();
        }
    [WebMethod]
    public static string GetSearchResultNew(string Strvalue)
    {
        string sHTML = "";


        try
        {
            System.Net.WebClient myWebClient = new System.Net.WebClient();
            
            sHTML = myWebClient.DownloadString("http://" + HttpContext.Current.Request.Url.Authority + "/AutoSuggestions.aspx?searchtext=" + Strvalue);
           
        }
        catch { }

        return sHTML;
    }
    
    [WebMethod]
    public static string GetSearchResult(string Strvalue)
    {
        DataSet ResultDs = new DataSet();
        DataSet ResultAttDs = new DataSet();
        string sHTML = "";
        
        string temp_product_Image = "";
        string temp_fmly_Image = "";
        string image_string = "";
        FileInfo Fil;
        string UserId="";
        string tmpstr="";
        string tmpstrCode = "";
        
        if( HttpContext.Current.Session["USER_ID"]!=null && HttpContext.Current.Session["USER_ID"]!="")
            UserId= HttpContext.Current.Session["USER_ID"].ToString();

        if (UserId == "")
            UserId = "0";

        HelperServices objHelperServices = new HelperServices();
        EasyAsk_WES ObjEasyAsk = new EasyAsk_WES(); 
        HelperDB objHelperDB = new HelperDB();
        Security objSecurity = new Security();
        ErrorHandler objErrorHandler = new ErrorHandler();
        string stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
        // Db calls
        //int priceCode = objHelperDB.GetPriceCode(UserId.ToString());
        //ResultDs = objHelperServices.GetPowerSearchProducts(Strvalue, priceCode, Convert.ToInt32(UserId));
        // Db calls
        Hashtable Autosuggestion = GetAutoCompleteResult(Strvalue);
       
        //EA Calls
        ResultDs = ObjEasyAsk.GetDropDownPowerSearchProducts(Strvalue,"5");
        ResultAttDs = (DataSet)HttpContext.Current.Session["SearchAttributes"];
        //EA Calls
        if (ResultDs != null && ResultDs.Tables.Count >= 2 && (ResultDs.Tables[0].Rows.Count > 0 || ResultDs.Tables[1].Rows.Count > 0))
        {
            try
            {
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_recordsRow = null;
                StringTemplateGroup _stg_recordsCell = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                StringTemplate _stmpl_records1 = null;
                StringTemplate _stmpl_recordsrows = null;
                TBWDataList[] lstrecords = new TBWDataList[0];


                StringTemplateGroup _stg_container1 = null;
                StringTemplateGroup _stg_records1 = null;
                TBWDataList1[] lstrecords1 = new TBWDataList1[0];

                
                string SearchAttr = "";
                
                _stg_recordsRow = new StringTemplateGroup("row", stemplatepath);
                _stg_recordsCell = new StringTemplateGroup("cell", stemplatepath);
                _stg_container = new StringTemplateGroup("main", stemplatepath);


              



                int ictrecords = 0;
                int ictrecords1 = 0;
                DataRow dRow;
                //foreach (DataRow dr in ResultDs.Tables[1].Rows)//For Records
                //{
                //    tmpstrCode = "";
                //    tmpstr = dr["FAMILY_NAME"].ToString().ToUpper();
                //    if (tmpstrCode == "" && tmpstr.Contains(Strvalue.ToUpper()) == true)
                //        tmpstrCode = tmpstr;

                //    tmpstr = dr["ProdCode"].ToString().ToUpper();
                //    if (tmpstrCode == "" && tmpstr.Contains(Strvalue.ToUpper()) == true)
                //        tmpstrCode = tmpstr;

                //    tmpstr = dr["ShortDesc"].ToString().ToUpper();
                //    if (tmpstrCode == "" && tmpstr.Contains(Strvalue.ToUpper()) == true)
                //        tmpstrCode = tmpstr;

                //    if (tmpstrCode != "")
                //    {
                //        dRow = ResultDs.Tables[0].NewRow();
                //        dRow["Code"] = tmpstrCode;
                //        ResultDs.Tables[0].Rows.Add(dRow);
                //    }


                //}
                SearchAttr = GetSearchResultAttr(Strvalue, stemplatepath);
                foreach (string str in Autosuggestion.Values)//For Records
                {
                    
                   
                        dRow = ResultDs.Tables[0].NewRow();
                        dRow["Code"] = str;
                        ResultDs.Tables[0].Rows.Add(dRow);
                   


                }
                lstrecords = new TBWDataList[ResultDs.Tables[0].Rows.Count + 1];
                lstrecords1 = new TBWDataList1[ResultDs.Tables[1].Rows.Count + 1];

               


                foreach (DataRow dr in ResultDs.Tables[0].Rows)//For Records
                {

                    _stmpl_records = _stg_recordsCell.GetInstanceOf("TopSearch" + "\\" + "cell");
                    if (Strvalue != "")
                    {
                        tmpstr=dr["Code"].ToString().ToUpper();                        
                        tmpstr =tmpstr.Replace(Strvalue.ToUpper(), "<strong>" + Strvalue.ToUpper() + "</strong>");
                        _stmpl_records.SetAttribute("TBT_SEARCH_TEXT_FORMAT", tmpstr);
                    }
                    else
                        _stmpl_records.SetAttribute("TBT_SEARCH_TEXT_FORMAT", dr["Code"].ToString().ToUpper());

                    _stmpl_records.SetAttribute("TBT_SEARCH_TEXT", dr["Code"].ToString().ToUpper());

                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                    ictrecords++;
                }
                foreach (DataRow dr in ResultDs.Tables[1].Rows)//For Records
                {

                    _stmpl_records1 =  _stg_recordsRow.GetInstanceOf("TopSearch" + "\\" + "row");

                    temp_product_Image = dr["ProdTh"].ToString();

                    temp_fmly_Image = dr["FamilyTh"].ToString();



                    if (temp_product_Image != "")
                        image_string = temp_product_Image;
                    else if (temp_fmly_Image != "")
                        image_string = temp_fmly_Image;
                    else
                        image_string = "noimage.gif";

                    Fil = new FileInfo(strimgPath + image_string);

                    if (Fil.Exists)
                        _stmpl_records1.SetAttribute("TBT_PRODUCT_TH_IMAGE", image_string);
                    else
                        _stmpl_records1.SetAttribute("TBT_PRODUCT_TH_IMAGE", "/noimage.gif");


                    _stmpl_records1.SetAttribute("TBT_PRODUCT_ID", dr["PRODUCT_ID"].ToString());
                    _stmpl_records1.SetAttribute("TBT_FAMILY_ID", dr["FAMILY_ID"].ToString());
                    _stmpl_records1.SetAttribute("TBT_CATEGORY_ID", dr["CATEGORY_ID"].ToString());

                    string eapath = "AllProducts////WESAUSTRALASIA////" + dr["CATEGORY_PATH"].ToString() + "////UserSearch1=Family Id=" + dr["FAMILY_ID"].ToString();

                    _stmpl_records1.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath)));
                    _stmpl_records1.SetAttribute("TBT_FAMILY_NAME", dr["FAMILY_NAME"].ToString());

                    string desc =  dr["FamilyDesc"].ToString();
                    if (desc.Length > 150)
                    {
                        _stmpl_records1.SetAttribute("TBT_SHOW_MORE", true);
                        desc = desc.Substring(0, 150).ToString();
                        _stmpl_records1.SetAttribute("FAMILY_DESC", desc);
                    }
                    else
                    {
                        _stmpl_records1.SetAttribute("FAMILY_DESC", desc);
                        _stmpl_records1.SetAttribute("TBT_SHOW_MORE", false);
                    }

                    
                    _stmpl_records1.SetAttribute("SHORT_DESC", dr["ShortDesc"].ToString());
                    _stmpl_records1.SetAttribute("PRODUCT_CODE", dr["ProdCode"].ToString());
                    _stmpl_records1.SetAttribute("PRODUCT_PRICE", dr["cost"].ToString());
                    lstrecords1[ictrecords1] = new TBWDataList1(_stmpl_records1.ToString());
                    ictrecords1++;
                }
                _stmpl_container = _stg_container.GetInstanceOf("TopSearch" + "\\" + "main");

                if (ResultDs.Tables[0].Rows.Count > 0)
                    _stmpl_container.SetAttribute("TBT_SEARCH_DISPLAY", true);
                else
                    _stmpl_container.SetAttribute("TBT_SEARCH_DISPLAY", false);
                _stmpl_container.SetAttribute("TBT_SEARCH_TEXT", Strvalue);
                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                _stmpl_container.SetAttribute("TBWDataList1", lstrecords1);
                _stmpl_container.SetAttribute("TBT_SEARCH_ATTR", SearchAttr);
                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }

        }

        return sHTML;
    }

    public static string GetSearchResultAttr(string Strvalue, string stemplatepath)
    {
        DataSet ResultDs = new DataSet();
       
        string sHTML = "";

     
       
        //string UserId = "";
   
      
        //if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
        //    UserId = HttpContext.Current.Session["USER_ID"].ToString();

        //if (UserId == "")
        //    UserId = "0";

        HelperServices objHelperServices = new HelperServices();
        EasyAsk_WES ObjEasyAsk = new EasyAsk_WES();
        HelperDB objHelperDB = new HelperDB();
        Security objSecurity = new Security();
        ErrorHandler objErrorHandler = new ErrorHandler();
      
        ResultDs = (DataSet)HttpContext.Current.Session["SearchAttributes"];
        //EA Calls
        if (ResultDs != null && ResultDs.Tables.Count >0 )
        {
            try
            {
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_recordsRow = null;
                StringTemplateGroup _stg_recordsCell = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                StringTemplate _stmpl_records1 = null;
                StringTemplate _stmpl_recordsrows = null;
                TBWDataList[] lstrows = new TBWDataList[0];


                StringTemplateGroup _stg_container1 = null;
                StringTemplateGroup _stg_records1 = null;
                TBWDataList1[] lstrecords1 = new TBWDataList1[0];



                _stg_recordsRow = new StringTemplateGroup("row", stemplatepath);
                _stg_recordsCell = new StringTemplateGroup("cell", stemplatepath);
                _stg_container = new StringTemplateGroup("main", stemplatepath);






                 int ictrows = 0;

              
              
              

                if (ResultDs != null)
                {

                    if (ResultDs.Tables.Count > 0)
                        lstrows = new TBWDataList[ResultDs.Tables.Count + 1];

                    for (int i = 0; i < ResultDs.Tables.Count; i++)
                    {
                        Boolean tmpallow = true;
                      
                            if (ResultDs.Tables[i].TableName.Contains("Category"))
                                tmpallow = true;
                            else if (ResultDs.Tables[i].TableName.Contains("PRODUCT TAGS"))
                                tmpallow = true;
                           // else if (ResultDs.Tables[i].TableName.Contains("Model"))
                           //     tmpallow = true;
                            else if (ResultDs.Tables[i].TableName.Contains("Price"))
                                tmpallow = true;                          
                            else
                                tmpallow = false;
                        
                        if (tmpallow == true)
                        {
                            if (ResultDs.Tables[i].Rows.Count > 0)
                            {

                                lstrecords1 = new TBWDataList1[ResultDs.Tables[i].Rows.Count + 1];
                                int ictrecords = 0;

                                int j = 0;
                                foreach (DataRow dr in ResultDs.Tables[i].Rows)//For Records
                                {
                                    if (ictrecords <= 6)
                                    {
                                        _stmpl_records = _stg_recordsCell.GetInstanceOf("TopSearch" + "\\" + "cell1");

                                        if (ResultDs.Tables[i].TableName.Contains("Category"))
                                        {
                                            _stmpl_records.SetAttribute("TBW_ATTRIBUTE_NAME", dr["Category_Name"].ToString());
                                            _stmpl_records.SetAttribute("TBW_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(dr["Category_Name"].ToString()));
                                            _stmpl_records.SetAttribute("TBW_ATTRIBUTE_BRAND", HttpUtility.UrlEncode(dr["brandvalue"].ToString()));
                                            _stmpl_records.SetAttribute("TBW_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(dr["SearchString"].ToString()));
                                        }
                                        else
                                        {
                                            _stmpl_records.SetAttribute("TBW_ATTRIBUTE_NAME", dr[0].ToString());
                                            _stmpl_records.SetAttribute("TBW_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(dr[0].ToString()));
                                            _stmpl_records.SetAttribute("TBW_ATTRIBUTE_BRAND", HttpUtility.UrlEncode(dr["brandvalue"].ToString()));
                                            _stmpl_records.SetAttribute("TBW_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(dr["SearchString"].ToString()));


                                        }


                                        _stmpl_records.SetAttribute("TBW_CUSTOM_NUM_FIELD3", 2);

                                        _stmpl_records.SetAttribute("TBW_ATTRIBUTE_TYPE", ResultDs.Tables[i].TableName.ToString());
                                        if (HttpContext.Current.Session["EASearch"] != null)
                                        {
                                            _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(HttpContext.Current.Session["EASearch"].ToString())));
                                        }

                                        lstrecords1[ictrecords] = new TBWDataList1(_stmpl_records.ToString());
                                    }
                                    ictrecords++;

                                }

                                j++;

                                _stmpl_recordsrows = _stg_recordsRow.GetInstanceOf("TopSearch" + "\\" + "row1");
                                
                                _stmpl_recordsrows.SetAttribute("TBT_ATTRIBUTE_TITLE", ResultDs.Tables[i].TableName.ToString());
                                _stmpl_recordsrows.SetAttribute("TBT_SEARCH_TEXT", Strvalue);
                                _stmpl_recordsrows.SetAttribute("TBWDataList1", lstrecords1);

                                if (ictrecords > 6)
                                    _stmpl_recordsrows.SetAttribute("TBT_DISPLAY_LINK", true);
                                else
                                    _stmpl_recordsrows.SetAttribute("TBT_DISPLAY_LINK", false);

                                lstrows[ictrows] = new TBWDataList(_stmpl_recordsrows.ToString());
                                ictrows++;
                            }
                        }
                    }
                    _stmpl_container = _stg_container.GetInstanceOf("TopSearch" + "\\" + "main1");
                    
                    _stmpl_container.SetAttribute("TBWDataList", lstrows); 
                    sHTML = _stmpl_container.ToString();
                }

                
                
              
                
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }

        }

        return sHTML;
    }
    private static string GetParentCatID(string catID)
    {
        try
        {
            HelperDB objHelperDB = new HelperDB();
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
    public static Hashtable  GetAutoCompleteResult(string Strvalue )
    {
        string resultStr = "";
        Hashtable Autosuggestion = new Hashtable();
        try
        {
            System.Net.WebClient myWebClient = new System.Net.WebClient();
             resultStr = myWebClient.DownloadString("http://" + EasyAsk_URL + ":" + EasyAsk_Port.ToString() + "/EasyAsk/AutoComplete.jsp?fctn=&key=" + Strvalue + "&dct=" + EasyAsk_WebCatDictionary + "&num=5");
        }
        catch { }
        if (resultStr != "")
        {
           
            resultStr = resultStr.Replace("\"", "");
            BuildResult(resultStr, Autosuggestion);
        }
         return Autosuggestion;
    }
     public  static void BuildResult(string resultStr ,Hashtable Autosuggestion )
        {
            int s = 0;
            int e = 0;
            int s1 = 0;
            int e1 = 0;
           
            string tmpstr = "";
            if (resultStr.Contains("[{"))
                s = resultStr.IndexOf("[{");
            else
                s = resultStr.IndexOf(",{");

            if (resultStr.Contains("},"))
                e = resultStr.IndexOf("},");
            else
                e = resultStr.IndexOf("}]");
            if (s>=0 && e>=0)
            {
                count = count + 1;
                s = s + 2;

                
                tmpstr=resultStr.Substring(s, e - s);                    
                 if (tmpstr.Contains("val:"))
                    s1=tmpstr.IndexOf("val:")+4;
                 if (tmpstr.Contains(",start"))
                     e1 = tmpstr.IndexOf(",start");

                 Autosuggestion.Add(count, tmpstr.Substring(s1, e1 - s1));



                if (resultStr.Substring(e + 1).Length > 0)
                    BuildResult(resultStr.Substring(e + 1), Autosuggestion);
            }
           


        }

     [System.Web.Services.WebMethod()]
     public static string SetURL(string url)
     {
         System.Uri urlx = new System.Uri(url);
         string f = urlx.Fragment.ToString();

         if (f != null && f != "")
         {

             if (HttpContext.Current.Session["PageUrl"]!=null )
             {
                 url = HttpContext.Current.Session["PageUrl"].ToString();
                 if (url.ToLower().Contains("index.htm") == true && url.ToLower().Contains("#") ==false)
                 {
                     HttpContext.Current.Session["PageUrl"] = url  + f;
                 }
             }
         }


         return "s";
     }

     [System.Web.Services.WebMethod()]
     public static string cartcount(string Strvalue)
     {
         try
         {
             HelperServices objHelperServices = new HelperServices();
             ErrorHandler oErr = new ErrorHandler();
             OrderServices objOrderServices = new OrderServices();
             string cartitem = "0";
             string rtnOrderid= "0";
             int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
             if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"].ToString() != "")
             {

                 int OrderID = 0;

                 if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ViewOrder"] != null && HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")))
                 {
                     OrderID = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]);
                 }
                 else
                 {
                     OrderID = objOrderServices.GetOrderID(objHelperServices.CI(HttpContext.Current.Session["USER_ID"]), OpenOrdStatusID);
                 }

                 string OrderStatus = objOrderServices.GetOrderStatus(OrderID);
                 if (OrderID > 0 && OrderStatus == OrderServices.OrderStatus.OPEN.ToString())    // || OrderStatus=="CAU_PENDING")
                 {
                     if (objOrderServices.GetOrderItemCount(OrderID) == 0)
                         cartitem = "0";
                     else
                         cartitem = objOrderServices.GetOrderItemCount(OrderID).ToString();

                     rtnOrderid = OrderID.ToString();
                 }
                 else
                 {
                     rtnOrderid = "0";
                     cartitem = "0";
                 }
             }
             return cartitem + "," + rtnOrderid;
         }


         catch 
         {             
             return "0,0";
         }
     }
     [System.Web.Services.WebMethod]
     public static string FindCableL(string strfindvalue)
     {

         DataSet rtnds = new DataSet();
         DataTable tblCable = new DataTable();
         string rtnstr = "";
         TradingBell.WebCat.EasyAsk.EasyAsk_WES EasyAsk = new TradingBell.WebCat.EasyAsk.EasyAsk_WES();


         
         DataSet dsCat = new DataSet();

         dsCat = EasyAsk.GetCategoryAndBrand("Filter");

         string strSQL = "Exec STP_TBWC_PICK_CABLE_FILTER";
         HelperDB objHelperDB = new HelperDB();
         DataSet dsCables = objHelperDB.GetDataSetDB(strSQL);

         if (dsCables != null && dsCables.Tables.Count > 0 && dsCables.Tables[0] != null && dsCables.Tables[0].Rows.Count > 0)
         {
             tblCable = dsCables.Tables[0].Copy();
         }

         if (dsCat == null || dsCat.Tables.Count == 0 || dsCat.Tables["CableL"] == null || dsCat.Tables["CableL"].Rows.Count == 0)
             rtnds.GetXml();
         else
         {
             rtnds.Tables.Add(dsCat.Tables["CableL"].Copy());
         }

         if (rtnds != null && rtnds.Tables[0] != null && rtnds.Tables[0].Rows.Count > 0)
         {
             for (int i = 0; i <= rtnds.Tables[0].Rows.Count - 1; i++)
             {
                 if (strfindvalue != "")
                 {
                     if (rtnds.Tables[0].Rows[i]["CableL"].ToString().ToLower().Contains(strfindvalue.ToLower()))
                         rtnstr = rtnstr + rtnds.Tables[0].Rows[i]["CableL"].ToString() + "&&&&&" + GetCableFilterImagePath(tblCable,rtnds.Tables[0].Rows[i]["CableL"].ToString()) + "#####";
                 }
                 else
                 {
                     rtnstr = rtnstr + rtnds.Tables[0].Rows[i]["CableL"].ToString() + "&&&&&" + GetCableFilterImagePath(tblCable, rtnds.Tables[0].Rows[i]["CableL"].ToString()) + "#####";
                 }

             }
         }
         if (rtnstr != "")
             rtnstr = rtnstr.Substring(0, rtnstr.Length - 5);

         return rtnstr;
     }
     [System.Web.Services.WebMethod]
     public static string FindCableLR(string strfindvalue, string strCable1value)
     {

         DataSet rtnds = new DataSet();
         DataTable tblCable = new DataTable();
         string rtnstr = "";
         string tempst = "";
         TradingBell.WebCat.EasyAsk.EasyAsk_WES EasyAsk = new TradingBell.WebCat.EasyAsk.EasyAsk_WES();

         if (strCable1value == "")
             return "";

         DataSet dsCat = new DataSet();

         dsCat = EasyAsk.GetCategoryAndBrand("Filter");

         string strSQL = "Exec STP_TBWC_PICK_CABLE_FILTER";
         HelperDB objHelperDB = new HelperDB();
         DataSet dsCables = objHelperDB.GetDataSetDB(strSQL);

         if (dsCables != null && dsCables.Tables.Count > 0 && dsCables.Tables[0] != null && dsCables.Tables[0].Rows.Count > 0)
         {
             tblCable = dsCables.Tables[0].Copy();
         }

         if (dsCat == null || dsCat.Tables.Count == 0 || dsCat.Tables["CableLR"] == null || dsCat.Tables["CableLR"].Rows.Count == 0)
             rtnds.GetXml();
         else
         {
             rtnds.Tables.Add(dsCat.Tables["CableLR"].Copy());
         }
         rtnstr = "All Cable"+ "&&&&&" + GetCableFilterImagePath(tblCable, "All Cable") + "#####";
         if (rtnds != null && rtnds.Tables[0] != null && rtnds.Tables[0].Rows.Count > 0)
         {
             for (int i = 0; i <= rtnds.Tables[0].Rows.Count - 1; i++)
             {
                 tempst = rtnds.Tables[0].Rows[i]["CableLR"].ToString();
                 if (tempst.Contains(":"))
                 {
                     string[] str = tempst.Split(':');
                     if (str.Length > 0)
                     {
                         if (str[0].ToString().ToLower() == strCable1value.ToLower())
                         {
                             if (strfindvalue != "")
                             {
                                 if (str[1].ToLower().Contains(strfindvalue.ToLower()))
                                     rtnstr = rtnstr + str[1].ToString() + "&&&&&" + GetCableFilterImagePath(tblCable, str[1].ToString()) + "#####";
                             }
                             else
                             {
                                     rtnstr = rtnstr + str[1].ToString() + "&&&&&" + GetCableFilterImagePath(tblCable, str[1].ToString()) + "#####";
                             }

                          
                         }
                     }

                 }
                
             }
         }
         if (rtnstr != "")
             rtnstr = rtnstr.Substring(0, rtnstr.Length - 5);

         return rtnstr;
     }

     public static String GetCableFilterImagePath(DataTable dt, string cableName)
     {
         FileInfo Fil;
         try
         {
             if (dt != null && dt.Rows.Count > 0)
             {
                 DataRow[] dr = dt.Select("Cable_Name='" + cableName + "'");
                 if (dr.Length > 0)
                 {
                     Fil = new FileInfo(strimgPath + dr[0]["Cable_image"].ToString());

                     if (Fil.Exists)
                         return "/prodimages"+dr[0]["Cable_image"].ToString();
                     else
                         return "/images/noimage.gif";
                 
                 }

             }
             else
             {
                 return "/images/noimage.gif";
             }
         }
         catch(Exception ex)
         {
         }
         return "/images/noimage.gif";
     }
     [System.Web.Services.WebMethod]
     public static string GetFindMyCableURL(string Cable1, string Cable2)
     {

         DataSet rtnds = new DataSet();
         string rtnstr = "";
         TradingBell.WebCat.EasyAsk.EasyAsk_WES EasyAsk = new TradingBell.WebCat.EasyAsk.EasyAsk_WES();
         string Ea = "AllProducts////WESAUSTRALASIA";
         Security objSecurity = new Security();
         string cid = "";
         if (HttpContext.Current.Request["cid"] != null)
             cid = HttpContext.Current.Request["cid"].ToString();

         if (Cable1 != "" && Cable2 != "")
         {
             Ea = Ea + "////AttribSelect=CableL = '" + Cable1 + "'";
             rtnstr = "product_list.aspx?&id=0&pcr=&cid=" + cid + "&tsb=&tsm=&searchstr=&type=CableLR&value=" + HttpUtility.UrlDecode(Cable1 +":" +  Cable2) + "&bname=&byp=2&Path=" + HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea));
         }
         else if (Cable1 != "" && Cable2 == "")
         {
             rtnstr = "product_list.aspx?&id=0&pcr=&cid=" + cid + "&tsb=&tsm=&searchstr=&type=CableL&value=" + HttpUtility.UrlDecode(Cable1) + "&bname=&byp=2&Path=" + HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea));
         }
         //HttpContext.Current.Response.Redirect(rtnstr);
         return rtnstr;
     }

    //////////////////////////////////////////////  Find my Brand/////////////////////////////////
     [System.Web.Services.WebMethod]
     public static string FindMyBrand(string strfindvalue)
     {

         DataSet rtnds = new DataSet();
         DataTable tblbrand = new DataTable();
         DataTable tblmodel = new DataTable();
         string rtnstr = "";
         string tmpstr = "";
         string tmpstrimage = "";
         TradingBell.WebCat.EasyAsk.EasyAsk_WES EasyAsk = new TradingBell.WebCat.EasyAsk.EasyAsk_WES();
         HelperDB objHelperDB = new HelperDB();


         DataSet dsCat = new DataSet();
        
         dsCat = EasyAsk.GetCategoryAndBrand("Filter");


        DataSet dsbrands = (DataSet)objHelperDB.GetGenericPageDataDB("Brand", "GET_BRAND_MODEL", HelperDB.ReturnType.RTDataSet);
        DataSet dsmodels = (DataSet)objHelperDB.GetGenericPageDataDB("Model", "GET_BRAND_MODEL", HelperDB.ReturnType.RTDataSet);






         if (dsbrands != null && dsbrands.Tables.Count > 0 && dsbrands.Tables[0] != null && dsbrands.Tables[0].Rows.Count > 0)
         {
             tblbrand = dsbrands.Tables[0].Copy();
         }
         if (dsmodels != null && dsmodels.Tables.Count > 0 && dsmodels.Tables[0] != null && dsmodels.Tables[0].Rows.Count > 0)
         {
             tblmodel = dsmodels.Tables[0].Copy();
         }

         if (dsCat == null || dsCat.Tables.Count == 0 || dsCat.Tables["Brand"] == null || dsCat.Tables["Brand"].Rows.Count == 0)
             rtnds.GetXml();
         else
         {
             rtnds.Tables.Add(dsCat.Tables["Brand"].Copy());
         }

         if (rtnds != null && rtnds.Tables[0] != null && rtnds.Tables[0].Rows.Count > 0)
         {

             for (int i = 0; i <= rtnds.Tables[0].Rows.Count - 1; i++)
             {

                 tmpstr = rtnds.Tables[0].Rows[i]["Brand"].ToString();
                 tmpstrimage = GetBrandFilterImagePath(tblbrand, tmpstr);

                 if (strfindvalue != "")
                 {
                     if (tmpstr.ToLower().Contains(strfindvalue.ToLower()))
                         rtnstr = rtnstr + tmpstr + "&&&&&" + tmpstrimage + "#####";
                 }
                 else
                 {
                     rtnstr = rtnstr + tmpstr + "&&&&&" + tmpstrimage + "#####";
                 }

             }
         }

       

         if (rtnstr != "")
             rtnstr = rtnstr.Substring(0, rtnstr.Length - 5);

         return rtnstr;
     }
     public static void SetBrandModelSessions()
     {


         TradingBell.WebCat.EasyAsk.EasyAsk_WES EasyAsk = new TradingBell.WebCat.EasyAsk.EasyAsk_WES();
         HelperDB objHelperDB = new HelperDB();
         DataTable tblBimg=null;
         DataTable tblMimg=null;
         DataSet dsCat = EasyAsk.GetCategoryAndBrand("Filter");


         DataSet dsbrands = (DataSet)objHelperDB.GetGenericPageDataDB("Brand", "GET_BRAND_MODEL", HelperDB.ReturnType.RTDataSet);
       //  DataSet dsmodels = (DataSet)objHelperDB.GetGenericPageDataDB("Model", "GET_BRAND_MODEL", HelperDB.ReturnType.RTDataSet);

         if (dsbrands != null && dsbrands.Tables.Count > 0 && dsbrands.Tables[0] != null && dsbrands.Tables[0].Rows.Count > 0)
         {
             tblBimg = dsbrands.Tables[0].Copy();
         }
         //if (dsmodels != null && dsmodels.Tables.Count > 0 && dsmodels.Tables[0] != null && dsmodels.Tables[0].Rows.Count > 0)
         //{
         //    tblMimg = dsmodels.Tables[0].Copy();
         //}


          DataSet Brand_Model = new DataSet();         
           DataTable tblModel = new DataTable("Model");
         DataTable tblBrand = new DataTable("Brand");
         DataTable tblcat = new DataTable();
         DataTable tblTempB = new DataTable();
         DataTable tblTempM = new DataTable();
         string tmpstr = "";
         string tmpstrimage = "";
         string tempst = "";


         DataSet Menu_Category = new DataSet();
       
         DataTable Menu_Parent = new DataTable("ParentCategory");
         DataTable Menu_Main_Category = new DataTable("MainCategory");
         DataTable Menu_Sub_Category = new DataTable("SubCategory");
         DataTable Menu_Brand = new DataTable("Brand");

          
         

         string Ea = "AllProducts////WESAUSTRALASIA";
         
         tblModel.Columns.Add("TOSUITE_MODEL", typeof(string));
         tblModel.Columns.Add("IMAGE_FILE", typeof(string));
         tblModel.Columns.Add("Brand", typeof(string));
         tblModel.Columns.Add("EA_PATH", typeof(string));
         
         //Brand_Model.Tables.Add(tblModel);

         
         tblBrand.Columns.Add("TOSUITE_BRAND", typeof(string));        
         tblBrand.Columns.Add("EA_PATH", typeof(string));

         //Brand_Model.Tables.Add(tblBrand);

         
         tblcat.Columns.Add("CATNAME", typeof(string));
         tblcat.Columns.Add("BRANDNAME", typeof(string));
         //Brand_Model.Tables.Add(tblcat);


         if (dsCat != null && dsCat.Tables.Count != 0 && dsCat.Tables["Brand"] != null && dsCat.Tables["Brand"].Rows.Count > 0)
         {
             tblTempB = dsCat.Tables["Brand"].Copy();

             for (int i = 0; i <= tblTempB.Rows.Count - 1; i++)
             {

                 tmpstr = tblTempB.Rows[i]["Brand"].ToString();
                 tmpstrimage = GetBrandFilterImagePath(tblBimg, tmpstr);
                 DataRow row = tblBrand.NewRow();
                 row["TOSUITE_BRAND"] = tmpstr;
                 row["EA_PATH"] = Ea;
               
                 tblBrand.Rows.Add(row);
             }
             
             if (HttpContext.Current.Session["MainMenuClick"] != null && ((DataSet) HttpContext.Current.Session["MainMenuClick"]).Tables["Brand"]!=null)
             {
                 Menu_Category = ((DataSet)HttpContext.Current.Session["MainMenuClick"]);
                 Menu_Category.Tables.Remove("Brand");  
                 Menu_Category.Tables.Add (tblBrand);
                 HttpContext.Current.Session["MainMenuClick"] = Menu_Category;
             }
             else
             {
                 Menu_Category.Tables.Add(tblBrand.Copy());
                 HttpContext.Current.Session["MainMenuClick"] = Menu_Category;
             }

         }
         //if (dsCat != null && dsCat.Tables.Count != 0 && dsCat.Tables["Models"] != null && dsCat.Tables["Models"].Rows.Count > 0)
         //{
         //    tblTempB = dsCat.Tables["Models"].Copy();

         //    for (int i = 0; i <= tblTempB.Rows.Count - 1; i++)
         //    {

         //        tempst = tblTempB.Rows[i]["Models"].ToString();
         //         if (tempst.Contains(":"))
         //         {
         //             string[] str = tempst.Split(':');
         //             if (str.Length > 0)
         //             {
         //                 tmpstr = str[1].ToString();
         //                 tmpstrimage = GetBrandFilterImagePath(tblMimg, tmpstr);
         //                 DataRow row = tblModel.NewRow();
         //                 row["TOSUITE_MODEL"] = str[1].ToString();
         //                 row["IMAGE_FILE"] = tmpstrimage;
         //                 row["Brand"] = str[0].ToString();
         //                 row["EA_PATH"] = Ea + "////AttribSelect=Brand = '" + str[0].ToString() + "'"; ;
         //                 tblModel.Rows.Add(row);
         //             }
         //         }
         //    }
         //    Brand_Model.Tables.Add(tblModel);
         //    HttpContext.Current.Session["WESBrand_Model"] = Brand_Model;

         //}

     }

     [System.Web.Services.WebMethod]
     public static string FindMyModel(string strfindvalue, string strBrandvalue)
     {

         DataSet rtnds = new DataSet();
         DataTable tblmodel = new DataTable();
         string rtnstr = "";
         string tempst = "";
         TradingBell.WebCat.EasyAsk.EasyAsk_WES EasyAsk = new TradingBell.WebCat.EasyAsk.EasyAsk_WES();
         HelperDB objHelperDB = new HelperDB();
         if (strBrandvalue == "")
             return "";

         DataSet dsCat = new DataSet();

         dsCat = EasyAsk.GetCategoryAndBrand("Filter");

         DataSet dsmodels = (DataSet)objHelperDB.GetGenericPageDataDB("Model", "GET_BRAND_MODEL", HelperDB.ReturnType.RTDataSet);

         if (dsmodels != null && dsmodels.Tables.Count > 0 && dsmodels.Tables[0] != null && dsmodels.Tables[0].Rows.Count > 0)
         {
             tblmodel = dsmodels.Tables[0].Copy();
         }

         if (dsCat == null || dsCat.Tables.Count == 0 || dsCat.Tables["Models"] == null || dsCat.Tables["Models"].Rows.Count == 0)
             rtnds.GetXml();
         else
         {
             rtnds.Tables.Add(dsCat.Tables["Models"].Copy());
         }
         rtnstr = "All Model" + "&&&&&" + "/images/AllModel.png" + "#####";
         if (rtnds != null && rtnds.Tables[0] != null && rtnds.Tables[0].Rows.Count > 0)
         {
             for (int i = 0; i <= rtnds.Tables[0].Rows.Count - 1; i++)
             {
                 tempst = rtnds.Tables[0].Rows[i]["Models"].ToString();
                 if (tempst.Contains(":"))
                 {
                     string[] str = tempst.Split(':');
                     if (str.Length > 0)
                     {
                         if (str[0].ToString().ToLower() == strBrandvalue.ToLower())
                         {
                             if (strfindvalue != "")
                             {
                                 if (str[1].ToLower().Contains(strfindvalue.ToLower()))
                                     rtnstr = rtnstr + str[1].ToString() + "&&&&&" + GetBrandFilterImagePath(tblmodel, str[1].ToString()) + "#####";
                             }
                             else
                             {
                                 rtnstr = rtnstr + str[1].ToString() + "&&&&&" + GetBrandFilterImagePath(tblmodel, str[1].ToString()) + "#####";
                             }


                         }
                     }

                 }

             }
         }
         if (rtnstr != "")
             rtnstr = rtnstr.Substring(0, rtnstr.Length - 5);

         return rtnstr;
     }
     public static String GetBrandFilterImagePath(DataTable dt, string bmName)
     {
         FileInfo Fil;
         try
         {
             if (dt != null && dt.Rows.Count > 0)
             {
                 DataRow[] dr = dt.Select("BMName='" + bmName + "'");
                 if (dr.Length > 0)
                 {
                     Fil = new FileInfo(strimgPath + dr[0]["BM_Image"].ToString());

                     if (Fil.Exists)
                         return "/prodimages" + dr[0]["BM_Image"].ToString();
                     else
                         return "/images/noimage.gif";

                 }

             }
             else
             {
                 return "/images/noimage.gif";
             }
         }
         catch (Exception ex)
         {
         }
         return "/images/noimage.gif";
     }



     [System.Web.Services.WebMethod]
     public static string SetSortOrder(string orderVal, string url)
     {

         string rtn = "-1";
         if (orderVal.ToLower() == "latest")
         {
             if (HttpContext.Current.Session["SortOrder"] == null || HttpContext.Current.Session["SortOrder"] != "Latest")
             {
                 HttpContext.Current.Session["SortOrder"] = "Latest";
                 rtn = "1";
             }

         }
         else if (orderVal.ToLower() == "ltoh")
         {
             if (HttpContext.Current.Session["SortOrder"] == null || HttpContext.Current.Session["SortOrder"] != "ltoh")
             {
                 HttpContext.Current.Session["SortOrder"] = "ltoh";
                 rtn = "1";
             }

         }
         else if (orderVal.ToLower() == "htol")
         {
             if (HttpContext.Current.Session["SortOrder"] == null || HttpContext.Current.Session["SortOrder"] != "htol")
             {

                 HttpContext.Current.Session["SortOrder"] = "htol";
                 rtn = "1";
             }
         }

         else if (orderVal.ToLower() == "relevance")
         {
             if (HttpContext.Current.Session["SortOrder"] == null || HttpContext.Current.Session["SortOrder"] != "relevance")
             {

                 HttpContext.Current.Session["SortOrder"] = "relevance";
                 rtn = "1";
             }
         }

         else if (orderVal.ToLower() == "popularity")
         {
             if (HttpContext.Current.Session["SortOrder"] == null || HttpContext.Current.Session["SortOrder"] != "popularity")
             {
                 HttpContext.Current.Session["SortOrder"] = "popularity";
                 rtn = "1";
             }
         }
         else if (orderVal.ToLower() == "catalog")
         {
             if (HttpContext.Current.Session["SortOrder"] == null || HttpContext.Current.Session["SortOrder"] != "catalog")
             {
                 HttpContext.Current.Session["SortOrder"] = "catalog";
                 rtn = "1";
             }
         }

         if (url.ToLower().Contains("powersearch.aspx") == true)
         {
             HttpContext.Current.Session["SortOrder_ps"] = "SortOrder_ps";
         }
         else
         {
             HttpContext.Current.Session["SortOrder_ps"] = "SortOrder_op";
         }

         return rtn;
     }

     [System.Web.Services.WebMethod]
     public static string GetFindMyBrandURL(string Brand, string Model)
     {

         DataSet rtnds = new DataSet();
         string rtnstr = "";
         TradingBell.WebCat.EasyAsk.EasyAsk_WES EasyAsk = new TradingBell.WebCat.EasyAsk.EasyAsk_WES();
         string Ea = "AllProducts////WESAUSTRALASIA";
         Security objSecurity = new Security();
         string cid = "";
         if (HttpContext.Current.Request["cid"] != null)
             cid = HttpContext.Current.Request["cid"].ToString();


         SetBrandModelSessions();


         if (Brand != "" && Model != "")
         {
             Ea = Ea + "////AttribSelect=Brand = '" + Brand + "'";
             //rtnstr = "product_list.aspx?&id=0&pcr=&cid=" + cid + "&tsb=" + HttpUtility.UrlDecode(Brand) + "&tsm=" + HttpUtility.UrlDecode(Model) + "&searchstr=&type=Model&value=" + HttpUtility.UrlDecode(Model) + "&bname=" + HttpUtility.UrlDecode(Brand) + "&byp=2&Path=" + HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea));
             rtnstr = "bybrand.aspx?id=0&pcr=&cid=" + cid + "&tsb=" + HttpUtility.UrlEncode(Brand) + "&tsm=" + HttpUtility.UrlEncode(Model) + "&Path=" + HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea)); 
         }
         else if (Brand != "" && Model == "")
         {
             //rtnstr = "product_list.aspx?&id=0&pcr=&cid=" + cid + "&tsb=" + HttpUtility.UrlDecode(Brand) + "&tsm=&searchstr=&type=Brand&value=" + HttpUtility.UrlDecode(Brand) + "&bname=" + HttpUtility.UrlDecode(Brand) + "&byp=2&Path=" + HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea));
             rtnstr = "categorylist.aspx?id=0&pcr=&cid=" + cid + "&bypcat=1&tsb=" + HttpUtility.UrlEncode(Brand) +"&Path=" + HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea));
         }
         //HttpContext.Current.Response.Redirect(rtnstr);
         return rtnstr;
     }
   
    
    
    
    
    


    //////////////////////////////////////////////  Find my Brand/////////////////////////////////
     public class Marker
     {
         public string endpoint { get; set; }
         // public string keys { get; set; }

     }

     [System.Web.Services.WebMethod]
     public static string endpoint(object Markers)
     {

         try
         {
             string subscription = JsonConvert.SerializeObject(Markers);
             string[] x = subscription.Split(new string[] { "," }, StringSplitOptions.None);
             string endpointjs = x[0] + "}";
             //   Marker x = JsonConvert.SerializeObject( Markers);
             Marker user = JsonConvert.DeserializeObject<Marker>(endpointjs);

             string endpoint = user.endpoint;
             ConnectionDB objConnection = new ConnectionDB();
             DataSet ds = new DataSet();
             HelperServices objHelperService = new HelperServices();
             string querystr = "insert into TB_NOTIFICATION_SUB_DETAILS (currentSubscription,[ENDPOINT],[MessageStatus],Website_id) ";
             querystr = querystr + "values ('" + subscription + "','" + endpoint + "',0,1)";

             SqlCommand pscmd = new SqlCommand(querystr, objConnection.GetConnection());
             pscmd.ExecuteNonQuery();
             //string strxml = HttpContext.Current.Server.MapPath("Notification") + "\\" + "Notification.txt";
             ////      System.IO.File.WriteAllText(strxml + "\\" + "Mainds.txt",  JsonConvert.SerializeObject(Markers));

             //if (Markers != null)
             //{


             //    StreamWriter writer2 = new StreamWriter(strxml, true);
             //    writer2.WriteLine(JsonConvert.SerializeObject(Markers));
             //    writer2.Flush();
             //    writer2.Close();
             //}

             //ConnectionDB objConnection = new ConnectionDB();      


             //string str = "insert into TB_NOTIFICATION_SUB_DETAILS values('"+ Markers +"','endpoint')";

             //SqlCommand objSqlCommand = new SqlCommand(str,   objConnection.GetConnection());
             //int r = objSqlCommand.ExecuteNonQuery();
             //if (r > 0)
             //{

             //}
             return "";
         }
         catch (Exception ex)
         {

             return "";

         }
     }

     [System.Web.Services.WebMethod]  
     public static string getMousehoverData(string strpid, string strpcode, string divcount)
     {
         FamilyServices objFamilyServices = new FamilyServices();
         HelperServices objHelperServices = new HelperServices();
         HelperDB objHelperDB = new HelperDB();
         string userid = string.Empty;
         string PriceTable = string.Empty;
         int pricecode = 0;
         string tmpProds = string.Empty;
         DataSet dsBgDisc = new DataSet();
         DataSet dsPriceTableAll = new DataSet();
         DataSet dscat = new DataSet();
         UserServices objUserServices = new UserServices();
         Security objSecurity = new Security();
         HelperDB objhelperDb = new HelperDB();
         dscat = (DataSet)HttpContext.Current.Session["FamilyProduct"];


         string CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()));
         //string CustomerType = objUserServices.GetCustomerType(Convert.ToInt32("999"));

         if (HttpContext.Current.Session["USER_ID"] != null)
             userid = HttpContext.Current.Session["USER_ID"].ToString();
         if (userid == string.Empty || userid == "999")
             userid = "0";

         tmpProds = "";

         //  DataRow[] drprodcoll = dscat.Tables[2].Select();
         //foreach (DataRow drpid in drprodcoll)
         //{
         //    tmpProds = tmpProds + drpid["PRODUCT_ID"].ToString() + ",";
         //}
         dsPriceTableAll = objHelperDB.GetProductPriceTableAll(strpid, Convert.ToInt32(userid));
         string sqlexec = "exec STP_TBWC_PICKFPRODUCTPRICE_EA '','" + strpid + "','" + userid + "'";
         dscat = objhelperDb.GetDataSetDB(sqlexec);

         try
         {
             if (dscat != null)
             {
                 foreach (DataRow drpid in dscat.Tables[0].Rows)
                 {
                     if (Convert.ToInt32(userid) > 0)
                     {
                         //PriceTable = objFamilyServices.AssemblePriceTable(objHelperServices.CI(drpid["PRODUCT_ID"].ToString()), pricecode, drpid["PRODUCT_CODE"].ToString(), drpid["STOCK_STATUS_DESC"].ToString(), drpid["PROD_STOCK_STATUS"].ToString(), CustomerType, Convert.ToInt32(userid), drpid["PROD_STOCK_FLAG"].ToString(), drpid["ETA"].ToString(), dsPriceTableAll);

                         PriceTable = objFamilyServices.AssemblePriceTable(Convert.ToInt32(strpid), pricecode, strpcode, drpid["PROD_STK_STATUS_DSC"].ToString(), drpid["PROD_STOCK_STATUS"].ToString(), CustomerType, Convert.ToInt32(userid), drpid["PROD_STOCK_FLAG"].ToString(), drpid["ETA"].ToString(), dsPriceTableAll);
                     }
                     // _stmpl_records.SetAttribute("TBT_PRODUCT_PRICE_TABLE", PriceTable);

                 }//srini
             }
         }
         catch (Exception ex)
         {

             throw ex;
         }
         //if (PriceTable != "")
         //{
         //    PriceTable = "<div class='popupaero'></div>" + PriceTable;
         //}
         return PriceTable;




     }
     public class orderItems
     {
         public int pid { get; set; }
         public string price { get; set; }
         public string divcount { get; set; }
         public string ebayBlock { get; set; }

     }
     [System.Web.Services.WebMethod]
     public static object getprice(string strpid, string divcount)
     {
         DataSet tmpDs = new DataSet();
         HelperDB objhelperDb = new HelperDB();
         UserServices objUserServices = new UserServices();
          HelperServices objHelperServices = new HelperServices();
         string CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()));
         //string CustomerType = objUserServices.GetCustomerType(Convert.ToInt32("999"));
         string userid = string.Empty;
         if (HttpContext.Current.Session["USER_ID"] != null)
             userid = HttpContext.Current.Session["USER_ID"].ToString();
         if (userid == string.Empty || userid == "999")
             userid = "0";
         tmpDs = objhelperDb.GetProductPriceEA("", strpid, userid);

          List<orderItems> orderitemslist = new List<orderItems>();
                    //               orderItems orderitems = new orderItems();



          string[] x = divcount.Split(',');
      
          for (int i = 0; i < x.Length; i++)
          {
              orderItems orderitems = new orderItems();
               string[] y = x[i].Split('_');
              DataTable Datar = tmpDs.Tables[0].Select("product_id='" + y[0] + "'").CopyToDataTable();
        if(Datar!=null)
        {
           
              
              orderitems.pid =Convert.ToInt32( Datar.Rows[0]["product_id"].ToString());
              orderitems.price = objHelperServices.FixDecPlace(objHelperServices.CDEC(Datar.Rows[0]["price"].ToString())).ToString();
              orderitems.ebayBlock = Datar.Rows[0]["EBAY_BLOCK"].ToString();
             
                  orderitems.divcount = x[i];
                 
            
              //for(x[0])
        }
             
              orderitemslist.Add(orderitems);
              
          }
         
          JavaScriptSerializer js = new JavaScriptSerializer();
          //                HttpContext.Current.Response.Write();
          return js.Serialize(orderitemslist);
   
     
     }
    
}
