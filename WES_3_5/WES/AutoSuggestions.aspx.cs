using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.CommonServices;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;

using System.Collections;
using System.Xml;
using Newtonsoft.Json;

    public partial class AutoSuggestions : System.Web.UI.Page
    {
        string searchText = "";
        static int count = 0;
        static string strimgPath = HttpContext.Current.Server.MapPath("ProdImages");
        static string EasyAsk_URL = System.Configuration.ConfigurationManager.AppSettings["EasyAsk_URL"].ToString();
        static int EasyAsk_Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EasyAsk_Port"]);
        static string EasyAsk_WebCatDictionary = System.Configuration.ConfigurationManager.AppSettings["EasyAsk_WebCatDictionary"].ToString();
        static string EasyAsk_WebCatPath = System.Configuration.ConfigurationManager.AppSettings["EA_NEW_PRODUCT_INIT_CATEGORY_PATH"].ToString();
        public string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();
        HelperServices objHelperServices = new HelperServices();
        EasyAsk_WES ObjEasyAsk = new EasyAsk_WES();
        HelperDB objHelperDB = new HelperDB();
        Security objSecurity = new Security();

        DataSet dsAutoComplete = new DataSet();
        DataSet dsAdvisor = new DataSet();
        ErrorHandler objErrorHandler = new ErrorHandler();
        string UserId = "";

        string stemplatepath = "";
        protected void Page_Load(object sender, EventArgs e)
        {
             stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
             if (Request.QueryString["SearchText"] != null)
                 searchText = Request.QueryString["SearchText"].ToString();
            if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
                UserId = HttpContext.Current.Session["USER_ID"].ToString();

            if (UserId == "")
                UserId = "0";

            

            GetAutoCompleteResult(searchText);
            GetAdvisorResult(searchText);
            if (dsAutoComplete != null & dsAutoComplete.Tables.Count >= 1)
            {
                repeaterSearch.DataSource = dsAutoComplete.Tables[0];
                repeaterSearch.DataBind();           
                
            }

           

        }
        protected string GetImagePath(object fPath,object PPath)
        {
            string image_string="";
            string temp_fmly_Image="", temp_product_Image = "";
            if (temp_product_Image != "")
            {
                image_string = temp_product_Image.Substring(42);
                image_string = objHelperServices.SetImageFolderPath(image_string.Replace("\\", "/"), "_th", "_th");
               
            }

            if (temp_fmly_Image != "")
            {
                image_string = temp_fmly_Image.Substring(42);
                image_string = objHelperServices.SetImageFolderPath(image_string.Replace("\\", "/"), "_th", "_th");
                
            }


            System.IO.FileInfo Fil = new System.IO.FileInfo(strimgPath + image_string.ToString());
            if (Fil.Exists)
                image_string = image_string.ToString();
            else
                image_string = "/images/noimage.gif";
            return image_string;
        }
        protected DataTable GetRepeaterAutosuggestion()
        {
            DataTable  rtn=null;
            if (dsAutoComplete != null & dsAutoComplete.Tables.Count >= 2)
            {
                rtn= dsAutoComplete.Tables[1];
            }
            return rtn;
        }
        protected DataTable GetRepeaterProduct()
        {
            DataTable rtn = null;
            if (dsAdvisor != null & dsAdvisor.Tables["Items"] != null)
            {
                rtn = dsAdvisor.Tables["Items"];
            }
            return rtn;
        }
        protected DataTable GetRepeaterAttr()
        {
            DataTable rtn = null;
            if (dsAdvisor != null & dsAdvisor.Tables["MyAttrubute"] != null)
            {
                rtn = dsAdvisor.Tables["MyAttrubute"];
            }
            return rtn;
        }

        protected DataTable GetRepeaterAttrDetail(object Id)
        {
            DataTable rtn = null;
            if (dsAdvisor != null & dsAdvisor.Tables["MyAttrubuteDetail"] != null)
            {
                rtn = dsAdvisor.Tables["MyAttrubuteDetail"].Select("attribute_id='" + Id.ToString() + "'").CopyToDataTable();
            }
            return rtn;
        }
        protected string GetRepeaterAttrFooter(object Id)
        {
            string rtn = "none";
            DataRow[] drs = null;
            if (dsAdvisor != null & dsAdvisor.Tables["MyAttrubute"] != null)
            {
                drs = dsAdvisor.Tables["MyAttrubute"].Select("attribute_id='" + Id.ToString() + "'");
                if (drs.Length > 0)
                {
                    if (Convert.ToInt32(drs[0]["count"].ToString()) > 5)
                        rtn = "block";
                        
                }

            }
            return rtn;
        }
        public void   GetAutoCompleteResult(string Strvalue)
        {
            string resultStr = "";       
            try
            {
                System.Net.WebClient myWebClient = new System.Net.WebClient();
                resultStr = myWebClient.DownloadString("http://" + EasyAsk_URL + ":" + EasyAsk_Port.ToString() + "/EasyAsk/AutoComplete-1.2.1.jsp?fctn=&key=" + Strvalue + "&dct=" + EasyAsk_WebCatDictionary + "&num=5");
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
            
            

        }
         public void   GetAdvisorResult(string Strvalue)
        {
            string resultStr = "";
           
            try
            {
                System.Net.WebClient myWebClient = new System.Net.WebClient();
                resultStr = myWebClient.DownloadString("http://" + EasyAsk_URL + ":" + EasyAsk_Port.ToString() + "/EasyAsk/apps/Advisor.jsp?indexed=1&oneshot=1&ie=UTF-8&disp=json&RequestAction=advisor&RequestData=CA_Search&CatPath="+ HttpUtility.UrlEncode(EasyAsk_WebCatPath)+"&defarrangeby=////NONE////&dct=" + HttpUtility.UrlEncode(EasyAsk_WebCatDictionary) +"&q=" + Strvalue +"&ResultsPerPage=5");
            }
            catch { }
            if (resultStr != "")
            {

                XmlDocument xd = new XmlDocument();
                resultStr = "{ \"rootNode\": {" + resultStr.Trim().TrimStart('{').TrimEnd('}') + "} }";
                xd = (XmlDocument)JsonConvert.DeserializeXmlNode(resultStr);
                dsAdvisor = new DataSet();
                dsAdvisor.ReadXml(new XmlNodeReader(xd));



                DataTable Sqltb = new DataTable();
                DataColumn dc = null;
                string  tmpstr = "";

              if (dsAdvisor.Tables["categoryList"] != null)
              {
                  dc = new DataColumn("input", typeof(string));
                  dc.DefaultValue = Strvalue;
                  dsAdvisor.Tables["categoryList"].Columns.Add(dc);

                  dc = new DataColumn("attribute_id", typeof(string));
                  dc.DefaultValue = "-1";
                  dsAdvisor.Tables["categoryList"].Columns.Add(dc);               
              }
              if (dsAdvisor.Tables["AttributeValueList"] != null)
              {
                  dc = new DataColumn("input", typeof(string));
                  dc.DefaultValue = Strvalue;
                  dsAdvisor.Tables["AttributeValueList"].Columns.Add(dc);

                  dsAdvisor.Tables["AttributeValueList"].Columns["AttributeValue"].ColumnName = "name";
                  
              }
              DataTable Att = new DataTable("MyAttrubute");

              dc = new DataColumn("attribute_id", typeof(string));
              dc.DefaultValue = "";
              Att.Columns.Add(dc);

              dc = new DataColumn("input", typeof(string));
              dc.DefaultValue = Strvalue;
              Att.Columns.Add(dc);

              dc = new DataColumn("name", typeof(string));
              dc.DefaultValue = "";
              Att.Columns.Add(dc);

              dc = new DataColumn("Count", typeof(string));
              dc.DefaultValue = "0";
              Att.Columns.Add(dc);


              DataTable Attdet = new DataTable("MyAttrubuteDetail");

              dc = new DataColumn("attribute_id", typeof(string));
              dc.DefaultValue = "";
              Attdet.Columns.Add(dc);

              dc = new DataColumn("input", typeof(string));
              dc.DefaultValue = Strvalue;
              Attdet.Columns.Add(dc);

              dc = new DataColumn("name", typeof(string));
              dc.DefaultValue = "";
              Attdet.Columns.Add(dc);

                 DataTable dtFiltered =null;

                        DataView view =null;
                        DataTable dtSpecificCols = null;

              if (dsAdvisor.Tables["categoryList"] != null)
              {                 
                      DataRow dr = Att.NewRow();
                      dr["attribute_id"] = "-1";
                      dr["name"] = "Category";
                      if (dsAdvisor.Tables["categoryList"] != null)
                          dr["count"] = dsAdvisor.Tables["categoryList"].Select().Count().ToString();

                        dtFiltered = dsAdvisor.Tables["categoryList"].Select().Take(5).CopyToDataTable();

                         view = new DataView(dtFiltered);
                         dtSpecificCols = view.ToTable(false, new string[] { "attribute_id","input","name"})  ;
                         dtSpecificCols.Select().CopyToDataTable(Attdet, LoadOption.OverwriteChanges); 
                      

                      Att.Rows.Add(dr);
              }

              if (dsAdvisor.Tables["Attribute"] != null)
              {
                 
                  foreach (DataRow dr1 in dsAdvisor.Tables["Attribute"].Rows)
                  {
                      if (dr1["name"].ToString().ToUpper().Contains("BRAND") || dr1["name"].ToString().ToUpper().Contains("KEYWORD") || dr1["name"].ToString().ToUpper().Contains("PRICE"))
                      {
                          DataRow dr = Att.NewRow();
                          dr["attribute_id"] = dr1["attribute_id"].ToString();
                          dr["name"] = dr1["name"].ToString();
                          if (dsAdvisor.Tables["AttributeValueList"] != null)
                              dr["count"] = dsAdvisor.Tables["AttributeValueList"].Select("attribute_id='" + dr1["attribute_id"] + "'").Count().ToString();

                          dsAdvisor.Tables["AttributeValueList"].Select("attribute_id='" + dr1["attribute_id"] + "'").Take(5).CopyToDataTable().DefaultView.ToTable(false, new string[] { "attribute_id", "input", "name" }).Select().CopyToDataTable(Attdet, LoadOption.OverwriteChanges);



                          Att.Rows.Add(dr);
                      }

                  }

              }
              dsAdvisor.Tables.Add(Attdet);
              dsAdvisor.Tables.Add(Att);  
              if (dsAdvisor.Tables["items"] != null)
              {
                   dc=new DataColumn("CATEGORY_PATH",typeof(string));
                  dc.DefaultValue="";
                  dsAdvisor.Tables["items"].Columns.Add( dc);

                  dc = new DataColumn("CATEGORY_ID", typeof(string));
                  dc.DefaultValue="";
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
                  
                  foreach (DataRow dr1 in dsAdvisor.Tables["items"].Rows)
                  {
                      if (tmpstr.Contains(dr1["FAMILY_ID"].ToString().ToUpper()) == false)
                          tmpstr = tmpstr + "'" + dr1["FAMILY_ID"].ToString().ToUpper() + "',";
                  }
              
               
                if (tmpstr != "")
                    tmpstr = tmpstr.Substring(0, tmpstr.Length - 1) + "";

                if (tmpstr != "")
                {
                    //Sqltb = objhelper.GetDataTable(StrSql);
                    Sqltb = (DataTable)objHelperDB.GetGenericDataDB(WesCatalogId, tmpstr, "GET_FAMILY_CATEGORY", HelperDB.ReturnType.RTTable);
                    if (Sqltb != null)
                    {
                        foreach (DataRow dr in Sqltb.Rows)
                        {
                            foreach (DataRow dr1 in dsAdvisor.Tables["items"].Rows)
                            {
                                if (dr["FAMILY_ID"].ToString().ToUpper() == dr1["FAMILY_ID"].ToString().ToUpper())
                                {
                                    dr1["CATEGORY_PATH"] = dr["CATEGORY_PATH"];
                                    dr1["CATEGORY_ID"] = dr["SubCatID"];
                                }
                            }
                        }
                    }
                }
              } 
                //resultStr = resultStr.Replace("\"", "");
                
            }
            
            

        }
         protected string GetEAPath(object CatPath, object Family_id, object Type)
         {
             string eapath = "";

             if (Type.ToString() == "Cat")
             {
                 eapath = "AllProducts////WESAUSTRALASIA////" + CatPath.ToString() ;
             }
             else
                 eapath = "AllProducts////WESAUSTRALASIA////" + CatPath.ToString() + "////UserSearch1=Family Id=" + Family_id.ToString();

             return HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath.ToString()));
         }
    }
