using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using StringTemplate = Antlr.StringTemplate.StringTemplate;
using StringTemplateGroup = Antlr.StringTemplate.StringTemplateGroup;

using System.Text;
/// <summary>
/// Summary description for EasyAsk_Cellink
/// </summary>
/// 
namespace TradingBell.Common
{
    public class EasyAsk_WES
    {
        #region "Common Declaration"
        Helper objhelper = new Helper();
        Connection Gcon = new Connection();
        ErrorHandler objErrorhandler = new ErrorHandler();
        
        string StrSql;
        const String COOKIE_NAME = "EasyAsk-eCommerce-Demo";
        String m_rpp = "10";
        String m_sort = "";
        String m_grp = "";
        int j;
        public string EasyAsk_URL = System.Configuration.ConfigurationManager.AppSettings["EasyAsk_URL"].ToString();
        public int EasyAsk_Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EasyAsk_Port"]);
        public string EasyAsk_WesCatBrandDictionary = System.Configuration.ConfigurationManager.AppSettings["EasyAsk_WesCatBrandDictionary"].ToString();
        public string EasyAsk_WebCatDictionary = System.Configuration.ConfigurationManager.AppSettings["EasyAsk_WebCatDictionary"].ToString();



        IRemoteEasyAsk getRemote()
        {
            IRemoteEasyAsk ea =Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WebCatDictionary);
            IOptions opts = ea.getOptions();
            opts.setResultsPerPage(m_rpp); // ea_rpp.Value);   // use current settings
            opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
            opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);
            opts.setToplevelProducts(true);                   
            return ea;
        }
        

        #endregion
        # region   "you have select & BreadCrumb"
        public void CreateYouHaveSelectAndBreadCrumb()
        {
            string[] StrValues = null;
            string EA ="";
            string temp="";
            string breadcrumb = "";
            string[] tmpsplit;
            string CatId="";
            string byp="0";
            DataSet Ds=new DataSet();
            HttpContext.Current.Session["BreadCrumbDS"] = null;
            DataSet YHSAndBC = new DataSet();
            string TempPath = "";
            string tsb = "";
            string tsm = "";
            string tempstr = "";
            string scrtext = "";
            string familyid = "";
            string Productid = "";
            bool isFamily = false;
            bool isProduct = false;
            YHSAndBC.Tables.Add("YHSAndBC");
            YHSAndBC.Tables["YHSAndBC"].Columns.Add("EAPath", typeof(string));
            YHSAndBC.Tables["YHSAndBC"].Columns.Add("RemoveEAPath", typeof(string));
            YHSAndBC.Tables["YHSAndBC"].Columns.Add("ActualValue", typeof(string));
            YHSAndBC.Tables["YHSAndBC"].Columns.Add("ItemType", typeof(string));
            YHSAndBC.Tables["YHSAndBC"].Columns.Add("ItemValue", typeof(string));
            YHSAndBC.Tables["YHSAndBC"].Columns.Add("Url", typeof(string));
            YHSAndBC.Tables["YHSAndBC"].Columns.Add("RemoveUrl", typeof(string));
            YHSAndBC.Tables["YHSAndBC"].Columns.Add("TempPath", typeof(string));
            YHSAndBC.Tables["YHSAndBC"].Columns.Add("FamilyName", typeof(string));
            YHSAndBC.Tables["YHSAndBC"].Columns.Add("ProductCode", typeof(string));

            if (HttpContext.Current.Session["EA"] != null)
            {
                EA = HttpContext.Current.Session["EA"].ToString();
 
            }

            if (EA != "")
            {
                StrValues = EA.Split(new string[] { "////" }, StringSplitOptions.None);
                if (StrValues.Length > 0)
                {
                    for (int i = 2; i < StrValues.Length; i++)
                    {
                        
                        

                        temp = "";                            
                        for (int j = 0; j <= i; j++)
                        {
                            if (j == 0)
                            {
                                temp = temp + StrValues[j];
                            }
                            else
                            {
                                temp = temp + "////" + StrValues[j];
                            }
                        }
                        DataRow row = YHSAndBC.Tables["YHSAndBC"].NewRow();
                        row["EAPath"] = temp;
                        row["RemoveEAPath"] = temp.Replace("////" + StrValues[i].ToString(), "");
                        row["ActualValue"] = StrValues[i].ToString();

                        if (StrValues[i].ToUpper().Contains("ATTRIBSEL") )
                        {
                            tmpsplit = StrValues[i].Split('=');
                            if (tmpsplit.Length >=2)
                            {
                                row["ItemType"] = tmpsplit[1].Trim();
                                TempPath =TempPath + "/"+tmpsplit[1].Trim();
                                if (tmpsplit[2].Contains(":"))
                                {
                                    tmpsplit = tmpsplit[2].Split(':');
                                    row["ItemValue"] = tmpsplit[1].Trim().Replace("'","");

                                }
                                else
                                {
                                    row["ItemValue"] = tmpsplit[2].Trim().Replace("'", "");
                                }

                            }                          
                        }
                        else if (StrValues[i].ToUpper().Contains("SEARCH"))
                        {
                            if (StrValues[i].ToUpper().Contains("FAMILY"))
                            {
                                tmpsplit = StrValues[i].Split('=');
                                if (tmpsplit.Length >= 2)
                                {
                                        row["ItemType"] = "Family";
                                        TempPath = TempPath + "/" + "Family";
                                        row["ItemValue"] = tmpsplit[2].ToString();
                                        DataSet tmpds=GetDataSet("Select FAMILY_NAME from TB_FAMILY WHERE FAMILY_ID='" + tmpsplit[2].ToString() + "'");
                                        if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                                            row["FamilyName"] = tmpds.Tables[0].Rows[0]["FAMILY_NAME"];
                                        
                                    
                                }
                            }
                            else if (StrValues[i].ToUpper().Contains("PROD"))
                            {
                                tmpsplit = StrValues[i].Split('=');
                                if (tmpsplit.Length >= 2)
                                {
                                    row["ItemType"] = "Product";
                                    TempPath = TempPath + "/" + "Product";
                                    row["ItemValue"] = tmpsplit[2].ToString();
                                    DataSet tmpds = GetDataSet("Select isnull(STRING_VALUE,'') as STRING_VALUE from TB_PROD_SPECS Where ATTRIBUTE_ID=1 And  PRODUCT_ID=" + tmpsplit[2].ToString() + "");
                                    if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                                        row["ProductCode"] = tmpds.Tables[0].Rows[0]["STRING_VALUE"];


                                }
                            }
                            else
                            {
                                tmpsplit = StrValues[i].Split('=');
                                if (tmpsplit.Length >= 1)
                                {
                                    row["ItemType"] = tmpsplit[0].Trim();
                                    TempPath = TempPath + "/" + tmpsplit[0].Trim();
                                    row["ItemValue"] = tmpsplit[1].Trim().Replace("'", "");
                                }
                            }
                        }
                        else
                        {                           
                                row["ItemType"] = "Category";
                                TempPath = TempPath + "/" + "Category";
                                row["ItemValue"] = StrValues[i].Trim().Replace("'", "");                         
                        }

                        row["TempPath"] = TempPath;


                        YHSAndBC.Tables["YHSAndBC"].Rows.Add(row);                        
                    }

                }
                isFamily = false;
                isProduct = false;
                if (YHSAndBC.Tables["YHSAndBC"].Rows.Count > 0)
                {
                    if (YHSAndBC.Tables["YHSAndBC"].Rows[0]["ItemType"].ToString().ToLower().Contains("search"))
                    {
                        for (int i = 0; i <= YHSAndBC.Tables["YHSAndBC"].Rows.Count - 1; i++)
                        {
                            using (DataTable td = YHSAndBC.Tables["YHSAndBC"])
                            {

                                if (td.Rows[i]["TempPath"].ToString().ToLower().Contains("search") && i==0)
                                {
                                    scrtext = td.Rows[i]["Itemvalue"].ToString();
                                    tempstr = "?srctext=" + HttpUtility.UrlEncode(td.Rows[i]["Itemvalue"].ToString());
                                    YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "powersearch.aspx" + tempstr;
                                    YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = "home.aspx?";
                                }
                                else if (td.Rows[i]["TempPath"].ToString().ToLower().Contains("search"))
                                {
                                    if (td.Rows[i]["ItemType"].ToString().ToLower()=="brand")
                                        tsb = HttpUtility.UrlEncode(td.Rows[i]["Itemvalue"].ToString());
                                    if (td.Rows[i]["ItemType"].ToString().ToLower() == "model")
                                        tsm = HttpUtility.UrlEncode(td.Rows[i]["Itemvalue"].ToString());
                                    if (td.Rows[i]["ItemType"].ToString().ToLower() == "family")
                                    {
                                        familyid = HttpUtility.UrlEncode(td.Rows[i]["Itemvalue"].ToString());
                                        isFamily = true;
                                    }
                                    if (td.Rows[i]["ItemType"].ToString().ToLower() == "product")
                                    {
                                        Productid = HttpUtility.UrlEncode(td.Rows[i]["Itemvalue"].ToString());
                                        isProduct = true;
                                    }
                                    if (isProduct == true)
                                    {
                                        if (td.Rows[i]["ItemType"].ToString().ToLower() == "product")
                                        {
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "Productdetails.aspx" + tempstr + "&fid=" + familyid + "&pid=" + Productid + "&searchstr=" + scrtext;
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"];  //"categorylist.aspx" + tempstr;
                                        }
                                    }
                                    else if (isFamily == true)
                                    {
                                        if (td.Rows[i]["ItemType"].ToString().ToLower() == "family")
                                        {
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "Family.aspx" + tempstr + "&fid=" + HttpUtility.UrlEncode(td.Rows[i]["Itemvalue"].ToString()) + "&searchstr=" + scrtext;
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"];  //"categorylist.aspx" + tempstr;
                                        }
                                        else
                                        {
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "Family.aspx" + tempstr + "&fid=" + familyid + "&tsb=" + tsb + "&tsm=" + tsm + "&type=" + td.Rows[i]["ItemType"].ToString() + "&value=" + HttpUtility.UrlEncode(td.Rows[i]["Itemvalue"].ToString()) + "&bname=" + tsb + "&searchstr=" + scrtext;
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"];  //"categorylist.aspx" + tempstr;
                                        }
                                    }

                                    else
                                    {
                                        YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "powersearch.aspx" + tempstr + "&tsb=" + tsb + "&tsm=" + tsm + "&type=" + td.Rows[i]["ItemType"].ToString() + "&value=" + HttpUtility.UrlEncode(td.Rows[i]["Itemvalue"].ToString()) + "&bname=" + tsb + "&searchstr=" + scrtext;
                                        YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"];  //"categorylist.aspx" + tempstr;
                                    }
                                }                                
                            }
                        }
                    }
                    else
                    {
                        isFamily =false;
                        isProduct = false;
                        for (int i = 0; i <= YHSAndBC.Tables["YHSAndBC"].Rows.Count - 1; i++)
                        {
                            using (DataTable td = YHSAndBC.Tables["YHSAndBC"])
                            {

                                if (td.Rows[i]["ItemType"].ToString().ToLower() == "family")
                                {
                                    familyid = HttpUtility.UrlEncode(td.Rows[i]["Itemvalue"].ToString());
                                    isFamily = true;
                                }
                                if (td.Rows[i]["ItemType"].ToString().ToLower() == "product")
                                {
                                    Productid = HttpUtility.UrlEncode(td.Rows[i]["Itemvalue"].ToString());
                                    isProduct = true;
                                }
                                if (td.Rows[i]["TempPath"].ToString() == "/Category")
                                {
                                    Ds = GetDataSet("Select Category_ID,isnull(CUSTOM_NUM_FIELD3,0) as CUSTOM_NUM_FIELD3 from tb_Category where Category_Name='" + td.Rows[i]["ItemValue"].ToString() + "'");
                                    if (Ds != null && Ds.Tables[0].Rows.Count > 0)
                                    {
                                        CatId = Ds.Tables[0].Rows[0][0].ToString();
                                        byp = ((int)float.Parse(Ds.Tables[0].Rows[0]["CUSTOM_NUM_FIELD3"].ToString())).ToString();
                                    }
                                    tempstr = "?id=0&cid=" + CatId + "&byp=" + byp;
                                    YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "categorylist.aspx" + tempstr;
                                    YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = "home.aspx?";
                                }
                                
                                else if (td.Rows[i]["TempPath"].ToString() == "/Category/Category")
                                {
                                    YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "product_list.aspx" + tempstr + "&type=" + td.Rows[i]["ItemType"].ToString() + "&value=" + HttpUtility.UrlEncode(td.Rows[i]["Itemvalue"].ToString()) + "&bname=" + "&searchstr=";
                                    YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"];  //"categorylist.aspx" + tempstr;
                                }
                                else if (td.Rows[i]["TempPath"].ToString() == "/Category/Brand")
                                {
                                    tsb = td.Rows[i]["Itemvalue"].ToString();
                                    YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "categorylist.aspx" + tempstr + "&bypcat=1&tsb=" + HttpUtility.UrlEncode(tsb);
                                    YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"]; //"categorylist.aspx" + tempstr;
                                }
                                else if (td.Rows[i]["TempPath"].ToString() == "/Category/Brand/Model")
                                {
                                    tsm = td.Rows[i]["Itemvalue"].ToString();
                                    YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "bybrand.aspx" + tempstr + "&tsb=" + HttpUtility.UrlEncode(tsb) + "&tsm=" + HttpUtility.UrlEncode(tsm);
                                    YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"];//"categorylist.aspx" + tempstr + "&bypcat=1&tsb=" + HttpUtility.UrlEncode(tsb);
                                }
                                else if (td.Rows[i]["TempPath"].ToString().StartsWith("/Category/Brand/Model"))
                                {   if (isProduct == true)
                                    {
                                        if (td.Rows[i]["ItemType"].ToString().ToLower() == "product")
                                        {
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "Productdetails.aspx" + tempstr + "&fid=" + familyid +"&pid=" +Productid;
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"]; //"bybrand.aspx" + tempstr + "&tsb=" + HttpUtility.UrlEncode(tsb) + "&tsm=" + HttpUtility.UrlEncode(tsb);
                                        }                                       
                                    }
                                    else if (isFamily == true)
                                    {
                                        if (td.Rows[i]["ItemType"].ToString().ToLower() == "family")
                                        {
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "Family.aspx" + tempstr + "&fid=" + familyid;
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"]; //"bybrand.aspx" + tempstr + "&tsb=" + HttpUtility.UrlEncode(tsb) + "&tsm=" + HttpUtility.UrlEncode(tsb);
                                        }
                                        else
                                        {
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "Family.aspx" + tempstr + "&fid=" + familyid + "&tsb=" + HttpUtility.UrlEncode(tsb) + "&tsm=" + HttpUtility.UrlEncode(tsm) + "&type=" + td.Rows[i]["ItemType"].ToString() + "&value=" + HttpUtility.UrlEncode(td.Rows[i]["Itemvalue"].ToString()) + "&bname=" + HttpUtility.UrlEncode(tsb) + "&searchstr=";
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"]; //"bybrand.aspx" + tempstr + "&tsb=" + HttpUtility.UrlEncode(tsb) + "&tsm=" + HttpUtility.UrlEncode(tsb);
                                        }
                                    }                                     
                                    else
                                    {
                                        YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "bybrand.aspx" + tempstr + "&tsb=" + HttpUtility.UrlEncode(tsb) + "&tsm=" + HttpUtility.UrlEncode(tsm) + "&type=" + td.Rows[i]["ItemType"].ToString() + "&value=" + HttpUtility.UrlEncode(td.Rows[i]["Itemvalue"].ToString()) + "&bname=" + HttpUtility.UrlEncode(tsb) + "&searchstr=";
                                        YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"]; //"bybrand.aspx" + tempstr + "&tsb=" + HttpUtility.UrlEncode(tsb) + "&tsm=" + HttpUtility.UrlEncode(tsb);
                                    }
                                }
                                else if (td.Rows[i]["TempPath"].ToString().StartsWith("/Category/Category"))
                                {
                                    if (isProduct == true)
                                    {
                                        if (td.Rows[i]["ItemType"].ToString().ToLower() == "product")
                                        {
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "Productdetails.aspx" + tempstr + "&fid=" + familyid + "&pid=" + Productid;
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"]; //"bybrand.aspx" + tempstr + "&tsb=" + HttpUtility.UrlEncode(tsb) + "&tsm=" + HttpUtility.UrlEncode(tsb);
                                        }
                                    }
                                    else if (isFamily == true)
                                    {
                                        if (td.Rows[i]["ItemType"].ToString().ToLower() == "family")
                                        {
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "Family.aspx" + tempstr + "&fid=" + familyid;
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"]; //"bybrand.aspx" + tempstr + "&tsb=" + HttpUtility.UrlEncode(tsb) + "&tsm=" + HttpUtility.UrlEncode(tsb);
                                        }
                                        else
                                        {
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "Family.aspx" + tempstr + "&fid=" + familyid + "&type=" + td.Rows[i]["ItemType"].ToString() + "&value=" + HttpUtility.UrlEncode(td.Rows[i]["Itemvalue"].ToString()) + "&bname=" + HttpUtility.UrlEncode(tsb) + "&searchstr=";
                                            YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"]; //"bybrand.aspx" + tempstr + "&tsb=" + HttpUtility.UrlEncode(tsb) + "&tsm=" + HttpUtility.UrlEncode(tsb);
                                        }
                                    }
                                    
                                    else
                                    {
                                        YHSAndBC.Tables["YHSAndBC"].Rows[i]["Url"] = "product_list.aspx" + tempstr + "&type=" + td.Rows[i]["ItemType"].ToString() + "&value=" + HttpUtility.UrlEncode(td.Rows[i]["Itemvalue"].ToString()) + "&bname=" + HttpUtility.UrlEncode(tsb) + "&searchstr=";
                                        YHSAndBC.Tables["YHSAndBC"].Rows[i]["RemoveUrl"] = YHSAndBC.Tables["YHSAndBC"].Rows[i - 1]["Url"]; //"categorylist.aspx" + tempstr;
                                    }
                                }
                            }
                        }
                    }
                }
                

                HttpContext.Current.Session["BreadCrumbDS"] = YHSAndBC;
            }


        }
        private DataSet GetDataSet(string SQLQuery)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(SQLQuery, Gcon.ConnectionString.ToString().Substring(Gcon.ConnectionString.ToString().IndexOf(';') + 1));
            da.Fill(ds, "generictable");
            return ds;
        }
        public string GetBreadCrumb(string templatepath)
        {
            StringTemplateGroup _stg_records = null;
            StringTemplateGroup _stg_records1 = null;
            StringTemplate _stmpl_records = null;
            StringTemplate _stmpl_records1 = null;
            string breadcrumb = "";
            string stemplatepath = templatepath;
            _stg_records = new StringTemplateGroup("Cell", stemplatepath);
            _stg_records1 = new StringTemplateGroup("main", stemplatepath);

            DataSet ds = new DataSet();
            ds = null;
            if (HttpContext.Current.Session["BreadCrumbDS"] != null)
            {
                ds = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];

            }
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                _stmpl_records1 = _stg_records1.GetInstanceOf("BreadCrumb" + "\\" + "home");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    _stmpl_records = _stg_records.GetInstanceOf("BreadCrumb" + "\\" + "cell");
                    
                    if(row["ItemType"].ToString().ToLower()=="family")
                        _stmpl_records.SetAttribute("TBT_LINKNAME", row["FamilyName"].ToString());
                    else if (row["ItemType"].ToString().ToLower() == "product")
                        _stmpl_records.SetAttribute("TBT_LINKNAME", row["ProductCode"].ToString());    
                    else
                    _stmpl_records.SetAttribute("TBT_LINKNAME", row["ItemValue"].ToString());

                    _stmpl_records.SetAttribute("TBT_REMOVEEAPATH",  HttpUtility.UrlEncode(objhelper.StringEnCrypt(row["RemoveEAPath"].ToString())));
                    _stmpl_records.SetAttribute("TBT_REMOVEURL", row["RemoveUrl"].ToString());
                    _stmpl_records.SetAttribute("TBT_URL", row["Url"].ToString());
                    _stmpl_records.SetAttribute("TBT_EAPATH",HttpUtility.UrlEncode(objhelper.StringEnCrypt( row["EAPath"].ToString())));
                    breadcrumb = breadcrumb + _stmpl_records.ToString();
                }

            }
            return _stmpl_records1+ breadcrumb+"</div>";
        }

        #endregion
        # region   "Main Category_Menu_Click"
        DataSet Menu_Category = new DataSet();
        DataTable Menu_Parent = new DataTable("ParentCategory");
        DataTable Menu_Main_Category = new DataTable("MainCategory");
        DataTable Menu_Sub_Category = new DataTable("SubCategory");
        DataTable Menu_Brand = new DataTable("Brand");

        public DataTable GetMainMenuClickDetail(string Cid, string ReturnType)
        {
            Boolean blnGetData=false;
            if (HttpContext.Current.Session["MainMenuClick"] == null)
            {
                blnGetData = true;
            }
            else if (((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["ParentCategory"].Rows[0]["CATEGORY_ID"].ToString().ToUpper() != Cid.ToUpper())
            {
                blnGetData = true;
            }

            DataTable tmptbl = new DataTable();
                
            string CatName = "";
            tmptbl = GetCategoryAndBrand("MainCategory").Tables[0].Select("CATEGORY_ID='" + Cid + "'").CopyToDataTable();
            if (tmptbl != null && tmptbl.Rows.Count > 0)
            {
                CatName = tmptbl.Rows[0]["CATEGORY_NAME"].ToString();
            }

            if (blnGetData == true)
            {
                
                if (CatName != "")
                {
                    IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WesCatBrandDictionary);
                    IOptions opts = ea.getOptions();
                    opts.setResultsPerPage("1");
                    opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);
                    opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);
                    opts.setSubCategories(true);
                    opts.setNavigateHierarchy(false);
                    opts.setReturnSKUs(false);
                    INavigateResults res = ea.userBreadCrumbClick("AllProducts////WESAUSTRALASIA////" + CatName);
                    String path = res.getCatPath();
                    HttpContext.Current.Session["EA"] = res.getCatPath();
                    CreateYouHaveSelectAndBreadCrumb();
                    IList<INavigateCategory> list = res.getDetailedCategories();
                    //IList<INavigateCategory> li = res.getDetailedAttributeValues(  ();
                    //For Brand Table.
                    Menu_Category.Tables.Add(Menu_Parent);
                    Menu_Parent.Columns.Add("CATEGORY_ID", typeof(string));
                    //For Main_Category Table.
                    Menu_Category.Tables.Add(Menu_Main_Category);
                    Menu_Main_Category.Columns.Add("CATEGORY_NAME", typeof(string));
                    Menu_Main_Category.Columns.Add("CATEGORY_ID", typeof(string));
                    Menu_Main_Category.Columns.Add("PARENT_CATEGORY_NAME", typeof(string));
                    Menu_Main_Category.Columns.Add("PARENT_CATEGORY_ID", typeof(string));
                    Menu_Main_Category.Columns.Add("SHORT_DESC", typeof(string));
                    Menu_Main_Category.Columns.Add("IMAGE_FILE", typeof(string));
                    Menu_Main_Category.Columns.Add("IMAGE_FILE2", typeof(string));
                    Menu_Main_Category.Columns.Add("CUSTOM_NUM_FIELD3", typeof(string));
                    // Sub_Category
                    Menu_Category.Tables.Add(Menu_Sub_Category);
                    Menu_Sub_Category.Columns.Add("PARENT_CATEGORY_ID", typeof(string));
                    Menu_Sub_Category.Columns.Add("PARENT_CATEGORY_NAME", typeof(string));
                    Menu_Sub_Category.Columns.Add("CATEGORY_NAME", typeof(string));
                    Menu_Sub_Category.Columns.Add("CATEGORY_ID", typeof(string));
                    Menu_Sub_Category.Columns.Add("SHORT_DESC", typeof(string));
                    Menu_Sub_Category.Columns.Add("CUSTOM_NUM_FIELD3", typeof(string));
                    //For Brand Table.
                    Menu_Category.Tables.Add(Menu_Brand);
                    Menu_Brand.Columns.Add("TOSUITE_BRAND", typeof(string));


                    DataRow row2 = Menu_Parent.NewRow();
                    row2["CATEGORY_ID"] = Cid.ToUpper();
                    Menu_Parent.Rows.Add(row2);
                    try
                    {
                        foreach (INavigateCategory item in list)
                        {
                            DataRow row = Menu_Main_Category.NewRow();
                            row["CATEGORY_NAME"] = item.getName();
                            IList<string> li = item.getIDs();
                            row["CATEGORY_ID"] = li[0].ToString().Substring(2);
                            row["PARENT_CATEGORY_Name"] = CatName;
                            row["PARENT_CATEGORY_ID"] = Cid.ToString();

                            row["SHORT_DESC"] = string.Empty;
                            row["IMAGE_FILE"] = string.Empty;
                            row["IMAGE_FILE2"] = string.Empty;
                            row["CUSTOM_NUM_FIELD3"] = "2";
                            IList<INavigateCategory> SubCat_List = item.getSubCategories();
                            foreach (INavigateCategory item1 in SubCat_List)
                            {
                                DataRow row1 = Menu_Sub_Category.NewRow();
                                row1["CATEGORY_NAME"] = item1.getName();
                                IList<string> SUB_CATEGORY_ID = item1.getIDs();
                                row1["CATEGORY_ID"] = SUB_CATEGORY_ID[0].ToString().Substring(2);

                                row1["PARENT_CATEGORY_NAME"] = item.getName();
                                row1["PARENT_CATEGORY_ID"] = li[0].ToString().Substring(2);
                                row1["SHORT_DESC"] = string.Empty;
                                row1["CUSTOM_NUM_FIELD3"] = "2";
                                Menu_Sub_Category.Rows.Add(row1);
                            }
                            Menu_Main_Category.Rows.Add(row);
                        }
                        IList<INavigateAttribute> Brand_list = res.getDetailedAttributeValues("Brand");
                        foreach (INavigateAttribute item in Brand_list)
                        {
                            DataRow row = Menu_Brand.NewRow();
                            row["TOSUITE_BRAND"] = item.getValue();
                            Menu_Brand.Rows.Add(row);
                        }
                        HttpContext.Current.Session["MainMenuClick"] = Menu_Category;
                        IList<string> Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
                        HttpContext.Current.Session["Category_Attributes"] = GetCategoryAttribute(Attributes, res, "", "");


                    }
                    catch (Exception)
                    {
                    }
                }
            }
            else if (HttpContext.Current.Session["EA"].ToString().ToUpper() ==("AllProducts////WESAUSTRALASIA////" +CatName).ToUpper())
            {
                HttpContext.Current.Session["EA"] = "AllProducts////WESAUSTRALASIA////" +CatName;
                    CreateYouHaveSelectAndBreadCrumb();
            }
            if (ReturnType == "MainCategory")
                return ((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["MainCategory"] ;
            else if (ReturnType == "SubCategory")
                return ((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["SubCategory"];
            else if (ReturnType == "Brand")
                return ((DataSet)HttpContext.Current.Session["MainMenuClick"]).Tables["Brand"];
            else
                return null;
             
        }
        DataSet Category_Attributes_DS = new DataSet();
        DataSet Category_Attributes_DS_Full = new DataSet();
        public DataSet GetCategoryAttribute(IList<string> Attributes, INavigateResults res, string temptext,string searchString )
        {
           
            try
            {             
                    Category_Attributes_DS.Tables.Add("Category");
                    Category_Attributes_DS.Tables["Category"].Columns.Add("CATEGORY_ID", typeof(string));
                    Category_Attributes_DS.Tables["Category"].Columns.Add("Category_Name", typeof(string));
                    Category_Attributes_DS.Tables["Category"].Columns.Add("Product_Count", typeof(int));
                    Category_Attributes_DS.Tables["Category"].Columns.Add("brandvalue", typeof(string));
                    Category_Attributes_DS.Tables["Category"].Columns.Add("SearchString", typeof(string));
                IList<INavigateCategory> category = null;
                category = res.getDetailedCategories();               
                if (category.Count > 0) //For Searching Category Values
                {
                    foreach (INavigateCategory categoryItem in category)
                    {
                        DataRow row = Category_Attributes_DS.Tables["Category"].NewRow();
                        IList<string> Id = categoryItem.getIDs();
                        row["CATEGORY_ID"] = Id[0].ToString().Substring(2);
                        row["Category_Name"] = categoryItem.getName();
                        row["Product_Count"] = categoryItem.getProductCount();
                        row["brandvalue"] = temptext;
                        row["SearchString"] = searchString;
                        Category_Attributes_DS.Tables["Category"].Rows.Add(row);
                    }
                }
             
               
                for (int i = 0; i < Attributes.Count; i++)
                {
                    String attrName = (String)Attributes[i];

                    if (!attrName.Contains("Long Description")) //For do not display Long Description
                    {
                        Category_Attributes_DS.Tables.Add(attrName);
                        Category_Attributes_DS.Tables[i+1].Columns.Add(attrName, typeof(string));
                        Category_Attributes_DS.Tables[i+1].Columns.Add("Product_Count", typeof(int));
                        Category_Attributes_DS.Tables[i + 1].Columns.Add("brandvalue", typeof(string)); //  Store Actual EA brand Value
                        Category_Attributes_DS.Tables[i + 1].Columns.Add("SearchString", typeof(string));
                        IList<INavigateAttribute> AttributeValue = res.getDetailedAttributeValues(attrName, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
                        foreach (INavigateAttribute AttributeItem in AttributeValue)
                        {
                            DataRow row = Category_Attributes_DS.Tables[i+1].NewRow();
                            if (attrName.Equals("Model"))//For Model Name will split
                            {
                                
                                if (AttributeItem.getValue().Contains(temptext))
                                {
                                    string[] model = AttributeItem.getValue().Split(':');
                                    if (model[0].ToString().Contains(temptext))
                                    {
                                        //if (temptext == "" && HttpContext.Current.Session["Brand"] == null || HttpContext.Current.Session["Brand"] == string.Empty)
                                        //{
                                        //    HttpContext.Current.Session["Brand"] = model[0].ToString();
                                        //    temptext = model[0].ToString();
                                        //}
                                        row[0] = model[1].ToString();
                                        row[1] = AttributeItem.getProductCount();
                                        row["brandvalue"] = temptext;
                                        row["SearchString"] = searchString;
                                        Category_Attributes_DS.Tables[i+1].Rows.Add(row);
                                    }
                                }
                            }
                            else
                            {
                                row[0] = AttributeItem.getValue();
                                row[1] = AttributeItem.getProductCount();
                                row["brandvalue"] = "";
                                row["SearchString"] = searchString;
                                Category_Attributes_DS.Tables[i+1].Rows.Add(row);
                            }
                        }
                       
                    }

                }

               
            
               
            }
            catch (Exception ex)
            {
            }

            return Category_Attributes_DS;
        }



          #endregion



        
        public int GetPriceCode()
        {
            int pc=-1;
            DataTable Sqltb = new DataTable();
            string userid = HttpContext.Current.Session["USER_ID"].ToString();
            if (!string.IsNullOrEmpty(userid))
            {
                string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
                Sqltb = objhelper.GetDataTable(sSQL);
                if (Sqltb != null && Sqltb.Rows.Count>0)
                    pc = Convert.ToInt16(Sqltb.Rows[0]["price_code"]);
            }
            return pc;
        }

        

        #region "For the Menu,Sub Menu"

        DataSet SubCategory = new DataSet();
        DataSet MainCategory = new DataSet();
        DataSet Brand = new DataSet();
        DataSet Category, Brand_Model;
        DataTable Main_Category = new DataTable();
        DataTable Sub_Category = new DataTable();
        DataTable tbl_Brand = new DataTable();

        public DataSet GetCategoryAndBrand(string ReturnType)
        {
            if (HttpContext.Current.Session["MainCategory"] == null || HttpContext.Current.Session["SubCategory"] == null || HttpContext.Current.Session["WESBrand"] == null)
            {
                IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WesCatBrandDictionary);
                IOptions opts = ea.getOptions();
                opts.setResultsPerPage("1"); // ea_rpp.Value);   // use current settings
                opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
                opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);
                opts.setSubCategories(true);
                opts.setNavigateHierarchy(false);
                opts.setReturnSKUs(false);                
                Category = new DataSet();
                INavigateResults res = ea.userBreadCrumbClick("AllProducts////WESAUSTRALASIA");
                HttpContext.Current.Session["EA"] = res.getCatPath();
                CreateYouHaveSelectAndBreadCrumb();
                String path = res.getCatPath();
                IList<INavigateCategory> list = res.getDetailedCategories();
                Create_DataTable_Columns();

                try
                {
                    foreach (INavigateCategory item in list)
                    {
                        DataRow row = Main_Category.NewRow();
                        row["CATEGORY_NAME"] = item.getName();
                        IList<string> li = item.getIDs();
                        row["CATEGORY_ID"] = li[0].ToString().Substring(2);
                        row["PARENT_CATEGORY"] = "0";
                        row["SHORT_DESC"] = string.Empty;
                        row["IMAGE_FILE"] = string.Empty;
                        row["IMAGE_FILE2"] = string.Empty;
                        row["CUSTOM_NUM_FIELD3"] = "2";
                        row["EA_PATH"] = HttpContext.Current.Session["EA"].ToString() + "////"+item.getName();

                        IList<INavigateCategory> SubCat_List = item.getSubCategories();
                        foreach (INavigateCategory item1 in SubCat_List)
                        {
                            DataRow row1 = Sub_Category.NewRow();
                            row1["TBT_PARENT_CATEGORY_NAME"] = item.getName();

                           //PARENT_CATEGORY
                            row1["TBT_PARENT_CATEGORY_ID"] = li[0].ToString().Substring(2);
                            row1["CATEGORY_NAME"] = item1.getName();
                            IList<string> SUB_CATEGORY_ID = item1.getIDs();

                            row1["CATEGORY_ID"] = SUB_CATEGORY_ID[0].ToString().Substring(2);
                            row1["TBT_SHORT_DESC"] = string.Empty;
                            row1["TBT_CUSTOM_NUM_FIELD3"] = "2";
                            row1["TBT_PARENT_CATEGORY_IMAGE"] = string.Empty;                                                        
                            row1["EA_PATH"] = HttpContext.Current.Session["EA"].ToString();
                            Sub_Category.Rows.Add(row1);
                        }
                        Main_Category.Rows.Add(row);
                    }
                    //foreach (INavigateAttribute item in Brand_list)
                    //{
                    //    DataRow row = Brand.NewRow();
                    //    row["TOSUITE_BRAND"] = item.getValue();
                    //    Brand.Rows.Add(row);
                    //}

                }
                catch (Exception)
                {
                }
                GetDataSet1(res);
                GetDBCategoryDetails();
                HttpContext.Current.Session["SubCategory"] = SubCategory;
                HttpContext.Current.Session["MainCategory"] = MainCategory;             
            }
            if (ReturnType == "MainCategory")
                return (DataSet)HttpContext.Current.Session["MainCategory"];
            else if (ReturnType == "SubCategory")
                return (DataSet)HttpContext.Current.Session["SubCategory"];
            //else if (ReturnType == "Brand")
            //    return (DataSet)HttpContext.Current.Session["WESBrand"];
            else
                return null;
        }

        public void GetDBCategoryDetails()
        {
            DataTable Sqltb = new DataTable();
            StrSql = "Select CATEGORY_ID,SHORT_DESC,IMAGE_FILE,IMAGE_FILE2,isnull(CUSTOM_NUM_FIELD3,0) as CUSTOM_NUM_FIELD3 from tb_Category where Category_ID in (";
            foreach (DataRow Dr in MainCategory.Tables[0].Rows)
            {
                StrSql = StrSql + "'" + Dr["CATEGORY_ID"].ToString() + "',";
            }
            if (StrSql != "")
                StrSql = StrSql.Substring(0, StrSql.Length - 1) + ")";

            Sqltb = objhelper.GetDataTable(StrSql);
            if (Sqltb != null)
            {

                foreach (DataRow Dr in MainCategory.Tables[0].Rows)
                {
                    DataRow[] row = Sqltb.Select("CATEGORY_ID='" + Dr["CATEGORY_ID"] + "'");
                    if (row.Length>0) 
                    {
                        DataTable Dt=row.CopyToDataTable();
                         Dr["SHORT_DESC"] = Dt.Rows[0]["SHORT_DESC"];
                            Dr["IMAGE_FILE"] = Dt.Rows[0]["IMAGE_FILE"];
                            Dr["IMAGE_FILE2"] = Dt.Rows[0]["IMAGE_FILE2"];                              
                            Dr["CUSTOM_NUM_FIELD3"] = ((int)float.Parse(Dt.Rows[0]["CUSTOM_NUM_FIELD3"].ToString())) ;
                            foreach (DataRow Dr1 in SubCategory.Tables[0].Rows)
                            {
                                if (Dr1["TBT_PARENT_CATEGORY_ID"].ToString().ToUpper() == Dr["CATEGORY_ID"].ToString().ToUpper())
                                {
                                    Dr1["TBT_SHORT_DESC"] = Dt.Rows[0]["SHORT_DESC"];
                                    Dr1["TBT_PARENT_CATEGORY_IMAGE"] = Dt.Rows[0]["IMAGE_FILE"];
                                    Dr1["TBT_CUSTOM_NUM_FIELD3"] = ((int)float.Parse(Dt.Rows[0]["CUSTOM_NUM_FIELD3"].ToString()));
                                }

                            }
                    }
                    
                }
              
                //foreach (DataRow dbDr in Sqltb.Rows)
                //{


                //    foreach (DataRow Dr in MainCategory.Tables[0].Rows)
                //    {
                //        if (Dr["CATEGORY_ID"] == dbDr["CATEGORY_ID"])
                //        {
                //            Dr["SHORT_DESC"] = dbDr["SHORT_DESC"];
                //            Dr["IMAGE_FILE"] = dbDr["IMAGE_FILE"];
                //            Dr["IMAGE_FILE2"] = dbDr["IMAGE_FILE2"];
                //            Dr["CUSTOM_NUM_FIELD3"] = dbDr["CUSTOM_NUM_FIELD3"];
                //            break;
                //        }
                //    }
                //}
            }


        }
        DataSet BrandModelPro = new DataSet();
        DataTable BrandModelFamPro = new DataTable("FamilyPro");
        public DataSet GetBrandAndModelProducts(string ParentCatName, string Model, string Brand,string resultPerPage, string CurrentPageNo,string NextPage  )
        {           
            try
            {
                string tmpCatPath = "";
                if (ParentCatName != "")
                {
                    tmpCatPath = "////" + ParentCatName + "////AttribSelect=Brand='"+Brand+"'";
                }

                IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WebCatDictionary);
                IOptions opts = ea.getOptions();
                opts.setResultsPerPage(resultPerPage); // ea_rpp.Value);   // use current settings
                opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
                opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);//"Category;1000"
                opts.setSubCategories(false);
                opts.setNavigateHierarchy(false);
                opts.setReturnSKUs(true);
                int PriceCode=GetPriceCode();
                if (PriceCode != -1)
                    opts.setCallOutParam("&eap_PriceCode=" + PriceCode.ToString());
                
               // Create_Brand_Product_Table_Columns();

                BrandModelPro.Tables.Add("TOTAL_PAGES");
                BrandModelPro.Tables["TOTAL_PAGES"].Columns.Add("TOTAL_PAGES", typeof(int));

                //For Total Products.
                BrandModelPro.Tables.Add("TOTAL_PRODUCTS");
                BrandModelPro.Tables["TOTAL_PRODUCTS"].Columns.Add("TOTAL_PRODUCTS", typeof(string));



                BrandModelPro.Tables.Add(BrandModelFamPro);
                BrandModelFamPro.Columns.Add("FAMILY_ID", typeof(string));
                BrandModelFamPro.Columns.Add("CATEGORY_ID", typeof(string));
                BrandModelFamPro.Columns.Add("PARENT_CATEGORY_ID", typeof(string));
                BrandModelFamPro.Columns.Add("FAMILY_NAME", typeof(string));
                BrandModelFamPro.Columns.Add("PRODUCT_ID", typeof(string));
                BrandModelFamPro.Columns.Add("PRODUCT_CODE", typeof(string));
                BrandModelFamPro.Columns.Add("PRODUCT_PRICE", typeof(string));
                BrandModelFamPro.Columns.Add("ATTRIBUTE_ID", typeof(string));
                BrandModelFamPro.Columns.Add("STRING_VALUE", typeof(string));
                BrandModelFamPro.Columns.Add("NUMERIC_VALUE", typeof(string));
                BrandModelFamPro.Columns.Add("OBJECT_TYPE", typeof(string));
                BrandModelFamPro.Columns.Add("OBJECT_NAME", typeof(string));
                BrandModelFamPro.Columns.Add("ATTRIBUTE_NAME", typeof(string));
                BrandModelFamPro.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
                BrandModelFamPro.Columns.Add("SUBCATNAME_L1", typeof(string));
                BrandModelFamPro.Columns.Add("SUBCATNAME_L2", typeof(string));
                BrandModelFamPro.Columns.Add("CATEGORY_NAME", typeof(string));
                BrandModelFamPro.Columns.Add("PARENT_CATEGORY_NAME", typeof(string));
                BrandModelFamPro.Columns.Add("CUSTOM_NUM_FIELD3", typeof(string));
                BrandModelFamPro.Columns.Add("SUB_FAMILY_COUNT", typeof(string));
                BrandModelFamPro.Columns.Add("PRODUCT_COUNT", typeof(string));                
                BrandModelFamPro.Columns.Add("FAMILY_PRODUCT_COUNT", typeof(string));
                BrandModelFamPro.Columns.Add("QTY_AVAIL", typeof(string));
                BrandModelFamPro.Columns.Add("MIN_ORD_QTY", typeof(string));
                
                BrandModelPro.Tables.Add("Category");
                //BrandModelPro.Tables["Category"].Columns.Add("SUBCATID_L1", typeof(string));
                //BrandModelPro.Tables["Category"].Columns.Add("SUBCATNAME_L1", typeof(string));
                 BrandModelPro.Tables["Category"].Columns.Add("CATALOG_ID", typeof(string));
                 BrandModelPro.Tables["Category"].Columns.Add("PRODUCT_ID", typeof(string));
                 BrandModelPro.Tables["Category"].Columns.Add("STATUS", typeof(string));

                INavigateResults res = null;
 
                //updateBreadCrumb(res.getBreadCrumbTrail());


                if (int.Parse(CurrentPageNo) <= 0)
                {
                    res = ea.userAttributeClick_Brand("AllProducts////WESAUSTRALASIA" + tmpCatPath, "Model = '" + Brand + ":" +Model+ "'");
                    HttpContext.Current.Session["EA"] = res.getCatPath();
                   
                }
                else
                {
                    res = ea.userPageOp(HttpContext.Current.Session["EA"].ToString(), CurrentPageNo, NextPage);
                    
                }

                HttpContext.Current.Session["EA"] = res.getCatPath();
                CreateYouHaveSelectAndBreadCrumb();
                DataRow dr = BrandModelPro.Tables["TOTAL_PAGES"].NewRow();
                dr[0] = res.getPageCount();
                BrandModelPro.Tables["TOTAL_PAGES"].Rows.Add(dr);

                DataRow dr1 = BrandModelPro.Tables["TOTAL_PRODUCTS"].NewRow();
                dr1[0] = res.getTotalItems();
                BrandModelPro.Tables["TOTAL_PRODUCTS"].Rows.Add(dr1);
              
                //Get_Family_Product_Values("ByBrand",b BrandModelFamPro, res, null, null);

                IList<string> Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
                HttpContext.Current.Session["LHSAttributes"] = GetCategoryAttribute(Attributes, res, "", "");
                            

                HttpContext.Current.Session["Brand_Model_Product_DS"] = BrandModelPro;
                return BrandModelPro;
            }
            catch (Exception ex)
            {
                objErrorhandler.ErrorMsg = ex;
                objErrorhandler.CreateLog();
                return null;
            }

        }


        private DataTable Get_Family_Product_Values(string DataType, DataTable ds, INavigateResults res, IResultRow item1, String name)
        {
           
            int last = res.getLastItem();
            int colFmlyID = res.getColumnIndex("Family Id");
            int colFmlyName = res.getColumnIndex("Family Name");
            int colFmlyDesc = res.getColumnIndex("Family ShortDescription");
            int colFmlylongDesc = res.getColumnIndex("Family Description");
            int colFmlyImg = res.getColumnIndex("Family Thumbnail");

            int colProductID = res.getColumnIndex("Prod Id");
            int colProductCode = res.getColumnIndex("Prod Code");
            int colProductPrice = res.getColumnIndex("Price");
            int colProductDesc = res.getColumnIndex("Prod Description");
            int colProductImg = res.getColumnIndex("Prod Thumbnail");

            int colProductCount = res.getColumnIndex("Prod Count");
            int colFamilyProdCount = res.getColumnIndex("Family Prod Count");
            int colSubFamilyCount = res.getColumnIndex("SubFamily Count");
            string _ProCode ="";
            string _ProductPrice ="";
            string _FmlyDesc="";
            string _ProductDesc="";
            string _FmlylongDesc="";
            string image_string = "";
            //IList<INavigateCategory> item = res.getDetailedCategories();


           


            DataRow dRow;
            try
            {
                if (last >= 0)
                {
                    
                        for (int i = res.getFirstItem() - 1, col = 0; i < last; i++, col++)
                        {
                            dRow = ds.NewRow();

                            dRow["FAMILY_ID"] = res.getCellData(i, colFmlyID);
                            dRow["FAMILY_NAME"] = res.getCellData(i, colFmlyName);
                            //dRow["DESCRIPTION1"] = res.getCellData(i, colFmlyDesc);
                            //dRow["LongDESCRIPTION"] = res.getCellData(i, colFmlylongDesc);
                            dRow["PRODUCT_ID"] = res.getCellData(i, colProductID);
                            dRow["PRODUCT_CODE"] = res.getCellData(i, colProductCode);
                            dRow["PRODUCT_PRICE"] = res.getCellData(i, colProductPrice);

                            string temp_family_count = res.getCellData(i, colFamilyProdCount);

                            string temp_product_count = res.getCellData(i, colProductCount);
                            string temp_subfamily_count = res.getCellData(i, colSubFamilyCount);
                            string temp_fmly_Image = res.getCellData(i, colFmlyImg).ToString();
                            string temp_product_Image = res.getCellData(i, colProductImg).ToString();

                            dRow["SUB_FAMILY_COUNT"] = (temp_subfamily_count == null || temp_subfamily_count == "") ? "0" : temp_subfamily_count;
                            dRow["PRODUCT_COUNT"] = (temp_product_count == null || temp_product_count == "") ? "0" : temp_product_count;
                            dRow["FAMILY_PRODUCT_COUNT"] = (temp_family_count == null || temp_family_count == "") ? "0" : temp_family_count;


                            image_string = "";
                            if (temp_product_count.ToString() == "1")
                            {
                                if (temp_product_Image != "")
                                    image_string = temp_product_Image.Substring(42);
                                else if (temp_fmly_Image != "")
                                    image_string = temp_fmly_Image.Substring(42);
                                else
                                    image_string = "noimage.gif";
                            }
                            else
                            {
                                if (temp_fmly_Image != "")
                                    image_string = temp_fmly_Image.Substring(42);
                                else
                                    image_string = "noimage.gif";

                            }
                            //if (temp_fmly_Image != "" && temp_product_Image != "")
                            //{
                            //    if (temp_product_count.ToString() == "1")
                            //    {
                            //        image_string = temp_product_Image.Substring(42);
                            //    }
                            //    else
                            //    {
                            //        image_string = temp_fmly_Image.Substring(42);
                            //    }
                            //}
                            //else
                            //{
                            //    image_string = "noimage.gif";
                            //}
                            _ProCode = res.getCellData(i, colProductCode);
                            _ProductPrice = res.getCellData(i, colProductPrice);

                            _FmlyDesc = res.getCellData(i, colFmlyDesc);
                            _ProductDesc = res.getCellData(i, colProductDesc);
                            _FmlylongDesc = res.getCellData(i, colFmlylongDesc);
                            dRow["SUBCATNAME_L1"] = "";
                            dRow["SUBCATNAME_L2"] = "";
                            dRow["CATEGORY_NAME"] = "";
                            dRow["PARENT_CATEGORY_NAME"] = "";

                            dRow["CATEGORY_ID"] = "";
                            dRow["PARENT_CATEGORY_ID"] = "";
                            dRow["CUSTOM_NUM_FIELD3"] = "";
                            dRow["QTY_AVAIL"] = "-1";
                            dRow["MIN_ORD_QTY"] = "-1";

                            if (DataType == "ByBrand" || DataType == "PowerSearch")
                            {
                                for (int k = 0; k < 9; k++)
                                {
                                    if (k == 0)
                                    {
                                        dRow["ATTRIBUTE_ID"] = "1";
                                        dRow["STRING_VALUE"] = _ProCode; //For the Product Code
                                        dRow["NUMERIC_VALUE"] = "0";
                                        dRow["OBJECT_TYPE"] = "NULL";
                                        dRow["OBJECT_NAME"] = "NULL";
                                        dRow["ATTRIBUTE_NAME"] = "Code";
                                        dRow["ATTRIBUTE_TYPE"] = "1";
                                        ds.Rows.Add(dRow.ItemArray);
                                    }
                                    if (k == 1)
                                    {


                                        dRow["ATTRIBUTE_ID"] = "5";
                                        dRow["STRING_VALUE"] = "";
                                        if (_ProductPrice == "" || _ProductPrice == string.Empty)
                                        {
                                            dRow["NUMERIC_VALUE"] = "0";
                                        }
                                        else
                                        {
                                            dRow["NUMERIC_VALUE"] = _ProductPrice.Substring(1);//For Cost
                                        }
                                        dRow["OBJECT_TYPE"] = "NULL";
                                        dRow["OBJECT_NAME"] = "NULL";
                                        dRow["ATTRIBUTE_NAME"] = "Cost";
                                        dRow["ATTRIBUTE_TYPE"] = "4";
                                        ds.Rows.Add(dRow.ItemArray);
                                    }
                                    if (k == 2)
                                    {
                                        dRow["ATTRIBUTE_ID"] = "62";
                                        dRow["STRING_VALUE"] = _FmlyDesc;//Family Description
                                        dRow["NUMERIC_VALUE"] = "0";
                                        dRow["OBJECT_TYPE"] = "NULL";
                                        dRow["OBJECT_NAME"] = "NULL";
                                        dRow["ATTRIBUTE_NAME"] = "Description";
                                        dRow["ATTRIBUTE_TYPE"] = "1";
                                        ds.Rows.Add(dRow.ItemArray);
                                    }
                                    if (k == 3)
                                    {
                                        dRow["ATTRIBUTE_ID"] = "449";
                                        dRow["STRING_VALUE"] = _ProductDesc;//Product Description.
                                        dRow["NUMERIC_VALUE"] = "0";
                                        dRow["OBJECT_TYPE"] = "NULL";
                                        dRow["OBJECT_NAME"] = "NULL";
                                        dRow["ATTRIBUTE_NAME"] = "PROD_DSC";
                                        dRow["ATTRIBUTE_TYPE"] = "1";
                                        ds.Rows.Add(dRow.ItemArray);
                                    }
                                    if (k == 4)
                                    {
                                        dRow["ATTRIBUTE_ID"] = "492";
                                        dRow["STRING_VALUE"] = "";
                                        if (_ProductPrice == "" || _ProductPrice == string.Empty)
                                        {
                                            dRow["NUMERIC_VALUE"] = "0";
                                        }
                                        else
                                        {
                                            dRow["NUMERIC_VALUE"] = _ProductPrice.Substring(1);//For Cost
                                        }
                                        dRow["OBJECT_TYPE"] = "NULL";
                                        dRow["OBJECT_NAME"] = "NULL";
                                        dRow["ATTRIBUTE_NAME"] = "PROD_EXT_PRI_3";
                                        dRow["ATTRIBUTE_TYPE"] = "4";
                                        ds.Rows.Add(dRow.ItemArray);
                                    }
                                    if (k == 5)
                                    {
                                        dRow["ATTRIBUTE_ID"] = "453";
                                        dRow["STRING_VALUE"] = image_string;
                                        dRow["NUMERIC_VALUE"] = "0";
                                        dRow["OBJECT_TYPE"] = "jpg";
                                        dRow["OBJECT_NAME"] = image_string;
                                        dRow["ATTRIBUTE_NAME"] = "Web Image1";
                                        dRow["ATTRIBUTE_TYPE"] = "3";
                                        ds.Rows.Add(dRow.ItemArray);
                                    }
                                    if (k == 6)
                                    {
                                        dRow["ATTRIBUTE_ID"] = "7";
                                        dRow["STRING_VALUE"] = image_string;
                                        dRow["NUMERIC_VALUE"] = "0";
                                        dRow["OBJECT_TYPE"] = "jpg";
                                        dRow["OBJECT_NAME"] = image_string;
                                        dRow["ATTRIBUTE_NAME"] = "Product Image1";
                                        dRow["ATTRIBUTE_TYPE"] = "3";
                                        ds.Rows.Add(dRow.ItemArray);
                                    }
                                    if (k == 7)
                                    {
                                        dRow["ATTRIBUTE_ID"] = "452";
                                        dRow["STRING_VALUE"] = image_string;
                                        dRow["NUMERIC_VALUE"] = "0";
                                        dRow["OBJECT_TYPE"] = "jpg";
                                        dRow["OBJECT_NAME"] = image_string;
                                        dRow["ATTRIBUTE_NAME"] = "TWeb Image1";
                                        dRow["ATTRIBUTE_TYPE"] = "3";
                                        ds.Rows.Add(dRow.ItemArray);
                                    }
                                    if (k == 8)
                                    {
                                        dRow["ATTRIBUTE_ID"] = "4";
                                        dRow["STRING_VALUE"] = _FmlylongDesc;//Family Description
                                        dRow["NUMERIC_VALUE"] = "0";
                                        dRow["OBJECT_TYPE"] = "NULL";
                                        dRow["OBJECT_NAME"] = "NULL";
                                        dRow["ATTRIBUTE_NAME"] = "DESCRIPTIONS";
                                        dRow["ATTRIBUTE_TYPE"] = "1";
                                        ds.Rows.Add(dRow.ItemArray);
                                    }

                                }
                            }
                            else if (DataType == "ProductList")
                            {

                                dRow["ATTRIBUTE_ID"] = "1";
                                dRow["STRING_VALUE"] = _ProCode; //For the Product Code
                                dRow["NUMERIC_VALUE"] = "0";
                                dRow["OBJECT_TYPE"] = "NULL";
                                dRow["OBJECT_NAME"] = "NULL";
                                dRow["ATTRIBUTE_NAME"] = "Code";
                                dRow["ATTRIBUTE_TYPE"] = "1";
                                ds.Rows.Add(dRow.ItemArray);

                                //------------------

                                dRow["ATTRIBUTE_ID"] = "0";
                                dRow["STRING_VALUE"] = "";
                                if (_ProductPrice == "" || _ProductPrice == string.Empty)
                                {
                                    dRow["NUMERIC_VALUE"] = "0";
                                }
                                else
                                {
                                    dRow["NUMERIC_VALUE"] = _ProductPrice.Substring(1);//For Cost
                                }
                                dRow["OBJECT_TYPE"] = "NULL";
                                dRow["OBJECT_NAME"] = "NULL";
                                dRow["ATTRIBUTE_NAME"] = "Cost";
                                dRow["ATTRIBUTE_TYPE"] = "0";
                                ds.Rows.Add(dRow.ItemArray);

                                //------------------
                                dRow["ATTRIBUTE_ID"] = "747";
                                dRow["STRING_VALUE"] = image_string;
                                dRow["NUMERIC_VALUE"] = "0";
                                dRow["OBJECT_TYPE"] = "jpg";
                                dRow["OBJECT_NAME"] = image_string;
                                dRow["ATTRIBUTE_NAME"] = "Product Image1";
                                dRow["ATTRIBUTE_TYPE"] = "9";
                                ds.Rows.Add(dRow.ItemArray);
                                //------------------
                                dRow["ATTRIBUTE_ID"] = "90";
                                dRow["STRING_VALUE"] = _FmlylongDesc;//Family Description
                                dRow["NUMERIC_VALUE"] = "0";
                                dRow["OBJECT_TYPE"] = "NULL";
                                dRow["OBJECT_NAME"] = "NULL";
                                dRow["ATTRIBUTE_NAME"] = "DESCRIPTIONS";
                                dRow["ATTRIBUTE_TYPE"] = "7";
                                ds.Rows.Add(dRow.ItemArray);


                            }
                            j++;
                        }
                        // Get Family Category & Sub Category Name from DB
                        DataTable Sqltb = new DataTable();
                        StrSql = "Select A.FAMILY_ID,isnull(B.CATEGORY_NAME,'') as SubCat , isnull(C.CATEGORY_NAME,'') as ParentCat,isnull(B.CATEGORY_ID,'') as SubCatID, isnull(C.CATEGORY_ID,'') as ParentCatID,isnull(B.CUSTOM_NUM_FIELD3,0) as CUSTOM_NUM_FIELD3 from TB_CATALOG_FAMILY A " +
                                 " Left Outer Join  TB_CATEGORY B on A.CATEGORY_ID=B.CATEGORY_ID " +
                                 " Left Outer Join TB_CATEGORY C   on B.PARENT_CATEGORY=C.CATEGORY_ID " +
                                 " Where A.CATALOG_ID=2 And FAMILY_ID in (";
                        foreach (DataRow dr1 in ds.Rows)
                        {
                            StrSql = StrSql + "'" + dr1["FAMILY_ID"].ToString().ToUpper() + "',";
                        }
                        if (StrSql != "")
                            StrSql = StrSql.Substring(0, StrSql.Length - 1) + ")";

                        if (StrSql != "")
                        {
                            Sqltb = objhelper.GetDataTable(StrSql);

                            if (Sqltb != null)
                            {
                                foreach (DataRow dr in Sqltb.Rows)
                                {
                                    foreach (DataRow dr1 in ds.Rows)
                                    {
                                        if (dr["FAMILY_ID"].ToString().ToUpper() == dr1["FAMILY_ID"].ToString().ToUpper())
                                        {
                                            dr1["SUBCATNAME_L1"] = dr["ParentCat"];
                                            dr1["PARENT_CATEGORY_NAME"] = dr["ParentCat"];

                                            dr1["SUBCATNAME_L2"] = dr["SubCat"];
                                            dr1["CATEGORY_NAME"] = dr["SubCat"];

                                            dr1["CATEGORY_ID"] = dr["SubCatID"];
                                            dr1["PARENT_CATEGORY_ID"] = dr["ParentCatID"];
                                            dr1["CUSTOM_NUM_FIELD3"] = ((int)float.Parse(dr["CUSTOM_NUM_FIELD3"].ToString())).ToString();
                                        }
                                    }
                                }
                            }
                        }
                        // Get QTY_AVAIL,MIN_ORD_QTY from DB -- TBWC_INVENTORY

                        StrSql = "SELECT PRODUCT_ID,QTY_AVAIL,MIN_ORD_QTY FROM TBWC_INVENTORY " +
                                 " Where PRODUCT_ID in (";
                        foreach (DataRow dr1 in ds.Rows)
                        {
                            StrSql = StrSql + dr1["PRODUCT_ID"].ToString().ToUpper() + ",";
                        }
                        if (StrSql != "")
                            StrSql = StrSql.Substring(0, StrSql.Length - 1) + ")";

                        if (StrSql != "")
                        {
                            Sqltb = objhelper.GetDataTable(StrSql);

                            if (Sqltb != null)
                            {
                                foreach (DataRow dr in Sqltb.Rows)
                                {
                                    foreach (DataRow dr1 in ds.Rows)
                                    {
                                        if (dr["PRODUCT_ID"].ToString().ToUpper() == dr1["PRODUCT_ID"].ToString().ToUpper())
                                        {
                                            dr1["QTY_AVAIL"] = dr["QTY_AVAIL"].ToString();
                                            dr1["MIN_ORD_QTY"] = dr["MIN_ORD_QTY"].ToString();
                                        }
                                    }
                                }
                            }
                        }                    
                    }
               

            }
            catch (Exception ex)
            {
            }
            return ds;
       }

        private DataTable Get_FamilyPage_Product_Values(string DataType, DataTable Fds, DataTable ds, INavigateResults res, IResultRow item1, String name)
        {

            int last = res.getLastItem();
            int colFmlyID = res.getColumnIndex("Family Id");
            int colFmlyName = res.getColumnIndex("Family Name");
            int colFmlyDesc = res.getColumnIndex("Family ShortDescription");
            int colFmlylongDesc = res.getColumnIndex("Family Description");
            int colFmlyImg = res.getColumnIndex("Family Thumbnail");

            int colsubFmlyID = res.getColumnIndex("SubFamily Id");
            int colsubFmlyName = res.getColumnIndex("SubFamily Name");            
            int colsubFmlyImg = res.getColumnIndex("SubFamily Thumbnail");            
            int colsubFmlylongDesc = res.getColumnIndex("SubFamily Description");

            int colProductID = res.getColumnIndex("Prod Id");
            int colProductCode = res.getColumnIndex("Prod Code");
            int colProductPrice = res.getColumnIndex("Price");
            int colProductDesc = res.getColumnIndex("Prod Description");
            int colProductImg = res.getColumnIndex("Prod Thumbnail");

            int colProductCount = res.getColumnIndex("Prod Count");
            int colFamilyProdCount = res.getColumnIndex("Family Prod Count");
            int colSubFamilyCount = res.getColumnIndex("SubFamily Count");
            string _ProCode = "";
            string _ProductPrice = "";
            string _FmlyDesc = "";
            string _ProductDesc = "";
            string _FmlylongDesc = "";
            string image_string = "";
            string _fmly_Image=null;
            //IList<INavigateCategory> item = res.getDetailedCategories();





            DataRow dRow;
            try
            {
                if (last >= 0)
                {

                    _ProCode = res.getCellData(0, colProductCode);
                    _ProductPrice = res.getCellData(0, colProductPrice);

                    _FmlyDesc = res.getCellData(0, colFmlyDesc);
                    _FmlylongDesc = res.getCellData(0, colFmlylongDesc);
                    _fmly_Image = res.getCellData(0, colFmlyImg).ToString();

                    string FamCount = res.getCellData(0, colFamilyProdCount);
                    string ProCount = last.ToString();
                    string Status = "false";
                    if (ProCount == "1")
                    {
                        if (FamCount != ProCount)
                        {
                            Status = "One Product";
                        }
                        else
                        {
                            Status = "true";
                        }
                    }
                    else if (FamCount == ProCount)
                    {
                        Status = "true";
                    }
                    else
                    {
                        Status = "false";
                    }

                    //string temp_product_Image = res.getCellData(0, colProductImg).ToString();
                    image_string = "";
                    if (_fmly_Image != "" && _fmly_Image != null)
                        image_string = _fmly_Image.Substring(42);
                    else
                        image_string = "noimage.gif";

                    dRow = Fds.NewRow();

                    dRow["FAMILY_ID"] = res.getCellData(0, colFmlyID);
                    dRow["FAMILY_NAME"] = res.getCellData(0, colFmlyName);
                    dRow["ATTRIBUTE_DATA_TYPE"] = "TEXT";
                    dRow["FAMILY_PROD_COUNT"]=FamCount;
                    dRow["PROD_COUNT"]=ProCount;
                    dRow["STATUS"] = Status;


                    dRow["ATTRIBUTE_ID"] = "1";
                    dRow["STRING_VALUE"] = _ProCode; //For the Product Code
                    dRow["NUMERIC_VALUE"] = "0";
                    dRow["OBJECT_TYPE"] = "NULL";
                    dRow["OBJECT_NAME"] = "NULL";
                    dRow["ATTRIBUTE_NAME"] = "Code";
                    dRow["ATTRIBUTE_TYPE"] = "1";
                    Fds.Rows.Add(dRow.ItemArray);

                    //------------------
                    dRow["ATTRIBUTE_ID"] = "13";
                    dRow["STRING_VALUE"] = _FmlyDesc;//Family Description
                    dRow["NUMERIC_VALUE"] = "0";
                    dRow["OBJECT_TYPE"] = "NULL";
                    dRow["OBJECT_NAME"] = "NULL";
                    dRow["ATTRIBUTE_NAME"] = "SHORT_DESCRIPTION";
                    dRow["ATTRIBUTE_TYPE"] = "7";
                    Fds.Rows.Add(dRow.ItemArray);

                    //------------------
                    dRow["ATTRIBUTE_ID"] = "4";
                    dRow["STRING_VALUE"] = _FmlylongDesc;//Family Description
                    dRow["NUMERIC_VALUE"] = "0";
                    dRow["OBJECT_TYPE"] = "NULL";
                    dRow["OBJECT_NAME"] = "NULL";
                    dRow["ATTRIBUTE_NAME"] = "DESCRIPTION";
                    dRow["ATTRIBUTE_TYPE"] = "7";
                    Fds.Rows.Add(dRow.ItemArray);



                    //------------------
                    dRow["ATTRIBUTE_ID"] = "746";
                    dRow["STRING_VALUE"] = image_string;
                    dRow["NUMERIC_VALUE"] = "0";
                    dRow["OBJECT_TYPE"] = "jpg";
                    dRow["OBJECT_NAME"] = image_string;
                    dRow["ATTRIBUTE_NAME"] = "FWeb Image1";
                    dRow["ATTRIBUTE_TYPE"] = "9";
                    Fds.Rows.Add(dRow.ItemArray);


                    //------------------
                    dRow["ATTRIBUTE_ID"] = "747";
                    dRow["STRING_VALUE"] = image_string;
                    dRow["NUMERIC_VALUE"] = "0";
                    dRow["OBJECT_TYPE"] = "jpg";
                    dRow["OBJECT_NAME"] = image_string;
                    dRow["ATTRIBUTE_NAME"] = "TFWeb Image1";
                    dRow["ATTRIBUTE_TYPE"] = "9";
                    Fds.Rows.Add(dRow.ItemArray);

                    // Get Family Category & Sub Category Name from DB
                    DataTable Sqltb = new DataTable();
                    StrSql = "select FS.STRING_VALUE,FS.ATTRIBUTE_ID,FS.NUMERIC_VALUE,FS.OBJECT_TYPE,FS.OBJECT_NAME,A.ATTRIBUTE_NAME,A.ATTRIBUTE_TYPE " +
                             " from TB_FAMILY F " +
                             " Inner Join TB_FAMILY_SPECS FS On fs.FAMILY_ID=F.FAMILY_ID" +
                             " Inner Join TB_ATTRIBUTE A On A.ATTRIBUTE_ID=FS.ATTRIBUTE_ID " +
                             " where A.ATTRIBUTE_TYPE=9 And A.PUBLISH2WEB = 1 and A.ATTRIBUTE_ID not in (746, 747) And F.FAMILY_ID=" + res.getCellData(0, colFmlyID).ToString();

                    if (StrSql != "")
                    {
                        Sqltb = objhelper.GetDataTable(StrSql);

                        if (Sqltb != null)
                        {
                            foreach (DataRow dr in Sqltb.Rows)
                            {
                                dRow["ATTRIBUTE_ID"] = dr["ATTRIBUTE_ID"];
                                dRow["STRING_VALUE"] = dr["STRING_VALUE"];
                                dRow["NUMERIC_VALUE"] = dr["NUMERIC_VALUE"];
                                dRow["OBJECT_TYPE"] = dr["OBJECT_TYPE"];
                                dRow["OBJECT_NAME"] = dr["OBJECT_NAME"];
                                dRow["ATTRIBUTE_NAME"] = dr["ATTRIBUTE_NAME"];
                                dRow["ATTRIBUTE_TYPE"] = dr["ATTRIBUTE_TYPE"];
                                Fds.Rows.Add(dRow.ItemArray);
                            }
                        }
                    }

                    for (int i = res.getFirstItem() - 1, col = 0; i < last; i++, col++)
                    {
                     

                        dRow = ds.NewRow();

                        string temp_family_count = res.getCellData(i, colFamilyProdCount);

                        string temp_product_count = res.getCellData(i, colProductCount);
                        string temp_subfamily_count = res.getCellData(i, colSubFamilyCount);
                        string temp_fmly_Image = res.getCellData(i, colFmlyImg).ToString();

                        string temp_subfmly_Image = res.getCellData(i, colsubFmlyImg).ToString();
                        string temp_product_Image = res.getCellData(i, colProductImg).ToString();

                        if (temp_fmly_Image != null && temp_fmly_Image != "")
                            dRow["FAMILY_TH_IMAGE"] = temp_fmly_Image.Substring(42);
                        else
                            dRow["FAMILY_TH_IMAGE"] = "noimage.gif";

                        //if (temp_subfmly_Image != null && temp_subfmly_Image != "")
                        //    dRow["SUB_FAMILY_TH_IMAGE"] = temp_subfmly_Image.Substring(42);
                        //else
                        //    dRow["SUB_FAMILY_TH_IMAGE"] = "noimage.gif";

                       

                        if (res.getCellData(i, colsubFmlyID).ToString()!="" && res.getCellData(i, colsubFmlyName).ToString()!="")
                        {
                            dRow["FAMILY_ID"] = res.getCellData(i, colsubFmlyID);
                            dRow["FAMILY_NAME"] = res.getCellData(i, colsubFmlyName);                            
                            dRow["FAMILY_SHORT_DESC"] = "";
                            dRow["FAMILY_DESC"] = res.getCellData(i, colsubFmlylongDesc);
                            

                            if (temp_subfmly_Image != null && temp_subfmly_Image != "")
                                dRow["FAMILY_TH_IMAGE"] = temp_subfmly_Image.Substring(42);
                            else
                                dRow["FAMILY_TH_IMAGE"] = "noimage.gif";
                        }
                        else
                        {

                            dRow["FAMILY_ID"] = res.getCellData(i, colFmlyID);
                            dRow["FAMILY_NAME"] = res.getCellData(i, colFmlyName);
                            dRow["FAMILY_SHORT_DESC"] = res.getCellData(i, colFmlyDesc);
                            dRow["FAMILY_DESC"] = res.getCellData(i, colFmlylongDesc);

                            if (temp_fmly_Image != null && temp_fmly_Image != "")
                                dRow["FAMILY_TH_IMAGE"] = temp_fmly_Image.Substring(42);
                            else
                                dRow["FAMILY_TH_IMAGE"] = "noimage.gif";
                        }

                        //dRow["FAMILY_ID"] = res.getCellData(i, colFmlyID);
                        //dRow["FAMILY_NAME"] = res.getCellData(i, colFmlyName);
                        ////dRow["FAMILY_TH_IMAGE"] = res.getCellData(i, colFmlyImg);
                        //dRow["FAMILY_SHORT_DESC"] = res.getCellData(i, colFmlyDesc);
                        //dRow["FAMILY_DESC"] = res.getCellData(i, colFmlylongDesc);
                        //dRow["SUB_FAMILY_ID"] = res.getCellData(i, colsubFmlyID);
                        //dRow["SUB_FAMILY_NAME"] = res.getCellData(i, colsubFmlyID);
                        ////dRow["SUB_FAMILY_TH_IMAGE"] = res.getCellData(i, colsubFmlyImg);
                        
                        //dRow["SUB_FAMILY_DESC"] = res.getCellData(i, colsubFmlylongDesc);
                        dRow["PRODUCT_ID"] = res.getCellData(i, colProductID);
                        dRow["PRODUCT_CODE"] = res.getCellData(i, colProductCode);
                        dRow["PRODUCT_PRICE"] = res.getCellData(i, colProductPrice);
                        dRow["PRODUCT_DESC"] = res.getCellData(i, colProductDesc);
                        //dRow["PRODUCT_TH_IMAGE"] = res.getCellData(i, colProductImg);





                        if (temp_product_Image != null && temp_product_Image != "")
                            dRow["PRODUCT_TH_IMAGE"] = temp_product_Image.Substring(42);
                        else
                            dRow["PRODUCT_TH_IMAGE"] = "noimage.gif";


                        dRow["SUB_FAMILY_COUNT"] = (temp_subfamily_count == null || temp_subfamily_count == "") ? "0" : temp_subfamily_count;
                        dRow["PRODUCT_COUNT"] = (temp_product_count == null || temp_product_count == "") ? "0" : temp_product_count;
                        dRow["FAMILY_PRODUCT_COUNT"] = (temp_family_count == null || temp_family_count == "") ? "0" : temp_family_count;


                        

                        dRow["QTY_AVAIL"] = "-1";
                        dRow["MIN_ORD_QTY"] = "-1";
                        ds.Rows.Add(dRow);

                    }


                    // Get QTY_AVAIL,MIN_ORD_QTY from DB -- TBWC_INVENTORY

                    StrSql = "SELECT PRODUCT_ID,QTY_AVAIL,MIN_ORD_QTY FROM TBWC_INVENTORY " +
                             " Where PRODUCT_ID in (";
                    foreach (DataRow dr1 in ds.Rows)
                    {
                        StrSql = StrSql + dr1["PRODUCT_ID"].ToString().ToUpper() + ",";
                    }
                    if (StrSql != "")
                        StrSql = StrSql.Substring(0, StrSql.Length - 1) + ")";

                    if (StrSql != "")
                    {
                        Sqltb = objhelper.GetDataTable(StrSql);

                        if (Sqltb != null)
                        {
                            foreach (DataRow dr in Sqltb.Rows)
                            {
                                foreach (DataRow dr1 in ds.Rows)
                                {
                                    if (dr["PRODUCT_ID"].ToString().ToUpper() == dr1["PRODUCT_ID"].ToString().ToUpper())
                                    {
                                        dr1["QTY_AVAIL"] = dr["QTY_AVAIL"].ToString();
                                        dr1["MIN_ORD_QTY"] = dr["MIN_ORD_QTY"].ToString();
                                    }
                                }
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
            }
            return ds;
        }

        private DataTable Get_ProductPage_Product_Values(string DataType, DataTable Fds, DataTable ds, INavigateResults res, IResultRow item1, String name)
        {

            int last = res.getLastItem();
            int colFmlyID = res.getColumnIndex("Family Id");
            int colFmlyName = res.getColumnIndex("Family Name");
            int colFmlyDesc = res.getColumnIndex("Family ShortDescription");
            int colFmlylongDesc = res.getColumnIndex("Family Description");
            int colFmlyImg = res.getColumnIndex("Family Thumbnail");

            int colsubFmlyID = res.getColumnIndex("SubFamily Id");
            int colsubFmlyName = res.getColumnIndex("SubFamily Name");
            int colsubFmlyImg = res.getColumnIndex("SubFamily Thumbnail");
            int colsubFmlylongDesc = res.getColumnIndex("SubFamily Description");

            int colProductID = res.getColumnIndex("Prod Id");
            int colProductCode = res.getColumnIndex("Prod Code");
            int colProductPrice = res.getColumnIndex("Price");
            int colProductDesc = res.getColumnIndex("Prod Description");
            int colProductImg = res.getColumnIndex("Prod Thumbnail");

            int colProductCount = res.getColumnIndex("Prod Count");
            int colFamilyProdCount = res.getColumnIndex("Family Prod Count");
            int colSubFamilyCount = res.getColumnIndex("SubFamily Count");
            string _ProCode = "";
            string _ProductPrice = "";
            string _FmlyDesc = "";
            string _ProductDesc = "";
            string _FmlylongDesc = "";
            string image_string = "";
            string _fmly_Image = null;
            
            //IList<INavigateCategory> item = res.getDetailedCategories();





            DataRow dRow;
            try
            {
                if (last >= 0)
                {

                    _ProCode = res.getCellData(0, colProductCode);
                    _ProductPrice = res.getCellData(0, colProductPrice);
                    _ProductDesc = res.getCellData(0, colProductDesc);

                    _FmlyDesc = res.getCellData(0, colFmlyDesc);
                    _FmlylongDesc = res.getCellData(0, colFmlylongDesc);
                    _fmly_Image = res.getCellData(0, colFmlyImg).ToString();
                    string FamCount = res.getCellData(0, colFamilyProdCount);
                    string ProCount = last.ToString();
                    string temp_subfamily_count = res.getCellData(0, colSubFamilyCount);
                    string temp_fmly_Image = res.getCellData(0, colFmlyImg).ToString();
                    string _subFamID = res.getCellData(0, colsubFmlyID);
                    string _FamID = res.getCellData(0, colFmlyID);
                    string temp_subfmly_Image = res.getCellData(0, colsubFmlyImg).ToString();
                    
                    //string temp_product_Image = res.getCellData(0, colProductImg).ToString();
                    image_string = "";
                    if (_fmly_Image != "" && _fmly_Image != null)
                        image_string = _fmly_Image.Substring(42);
                    else
                        image_string = "noimage.gif";


                    dRow = ds.NewRow();

                    dRow["PRODUCT_ID"] = res.getCellData(0, colProductID);

                    if (res.getCellData(0, colsubFmlyID).ToString() != "" && res.getCellData(0, colsubFmlyName).ToString() != "")
                    {
                        dRow["FAMILY_ID"] = res.getCellData(0, colsubFmlyID);
                        dRow["FAMILY_NAME"] = res.getCellData(0, colsubFmlyName);
                        dRow["FAMILY_SHORT_DESC"] = "";
                        dRow["FAMILY_DESC"] = res.getCellData(0, colsubFmlylongDesc);                        
                        if (temp_subfmly_Image != null && temp_subfmly_Image != "")
                            dRow["FAMILY_TH_IMAGE"] = temp_subfmly_Image.Substring(42);
                        else
                            dRow["FAMILY_TH_IMAGE"] = "noimage.gif";
                    }
                    else
                    {

                        dRow["FAMILY_ID"] = res.getCellData(0, colFmlyID);
                        dRow["FAMILY_NAME"] = res.getCellData(0, colFmlyName);
                        dRow["FAMILY_SHORT_DESC"] = res.getCellData(0, colFmlyDesc);
                        dRow["FAMILY_DESC"] = res.getCellData(0, colFmlylongDesc);

                        if (temp_fmly_Image != null && temp_fmly_Image != "")
                            dRow["FAMILY_TH_IMAGE"] = temp_fmly_Image.Substring(42);
                        else
                            dRow["FAMILY_TH_IMAGE"] = "noimage.gif";
                    }

                    dRow["QTY_AVAIL"] = "-1";
                    dRow["MIN_ORD_QTY"] = "-1";
                    dRow["STOCK_STATUS_DESC"] = "";
                    dRow["FAMILY_PROD_COUNT"] = FamCount;
                    dRow["PROD_COUNT"] = ProCount;

                    // Get QTY_AVAIL,MIN_ORD_QTY from DB -- TBWC_INVENTORY
                    DataTable Sqltb = new DataTable();
                    StrSql = "SELECT A.PRODUCT_ID,A.QTY_AVAIL,A.MIN_ORD_QTY,Isnull(B.PROD_STK_STATUS_DSC,'') as STOCK_STATUS " +
                             " FROM TBWC_INVENTORY A LEFT OUTER JOIN WESTB_PRODUCT_ITEM B ON A.PRODUCT_ID=B.PRODUCT_ID" +
                             " Where A.PRODUCT_ID =" + res.getCellData(0, colProductID).ToString();                    
                    if (StrSql != "")
                    {
                        Sqltb = objhelper.GetDataTable(StrSql);

                        if (Sqltb != null)
                        {
                            foreach (DataRow dr in Sqltb.Rows)
                            {
                                dRow["QTY_AVAIL"] = dr["QTY_AVAIL"].ToString();
                                dRow["MIN_ORD_QTY"] = dr["MIN_ORD_QTY"].ToString();
                                dRow["STOCK_STATUS_DESC"] = dr["STOCK_STATUS"].ToString();
                            }
                        }
                    }

                
                

                    dRow["ATTRIBUTE_ID"] = "1";
                    dRow["STRING_VALUE"] = _ProCode; 
                    dRow["NUMERIC_VALUE"] = "0";
                    dRow["OBJECT_TYPE"] = "NULL";
                    dRow["OBJECT_NAME"] = "NULL";
                    dRow["ATTRIBUTE_NAME"] = "Code";
                    dRow["ATTRIBUTE_TYPE"] = "1";
                    dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
                    ds.Rows.Add(dRow.ItemArray);

                    //------------------
                    dRow["ATTRIBUTE_ID"] = "449";
                    dRow["STRING_VALUE"] = _ProductDesc;
                    dRow["NUMERIC_VALUE"] = "0";
                    dRow["OBJECT_TYPE"] = "NULL";
                    dRow["OBJECT_NAME"] = "NULL";
                    dRow["ATTRIBUTE_NAME"] = "PROD_DSC";
                    dRow["ATTRIBUTE_TYPE"] = "1";
                    dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
                    ds.Rows.Add(dRow.ItemArray);

                  



                    //------------------
                    dRow["ATTRIBUTE_ID"] = "452";
                    dRow["STRING_VALUE"] = image_string;
                    dRow["NUMERIC_VALUE"] = "0";
                    dRow["OBJECT_TYPE"] = "jpg";
                    dRow["OBJECT_NAME"] = image_string;
                    dRow["ATTRIBUTE_NAME"] = "TWeb Image1";
                    dRow["ATTRIBUTE_TYPE"] = "3";
                    dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
                    ds.Rows.Add(dRow.ItemArray);


                    //------------------
                    dRow["ATTRIBUTE_ID"] = "453";
                    dRow["STRING_VALUE"] = image_string;
                    dRow["NUMERIC_VALUE"] = "0";
                    dRow["OBJECT_TYPE"] = "jpg";
                    dRow["OBJECT_NAME"] = image_string;
                    dRow["ATTRIBUTE_NAME"] = "Web Image1";
                    dRow["ATTRIBUTE_TYPE"] = "3";
                    dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
                    ds.Rows.Add(dRow.ItemArray);

                    // Get product Spec
                    Sqltb = new DataTable();
                    StrSql = "select A.STRING_VALUE,A.ATTRIBUTE_ID,A.NUMERIC_VALUE,A.OBJECT_TYPE,A.OBJECT_NAME,B.ATTRIBUTE_NAME,B.ATTRIBUTE_TYPE,B.ATTRIBUTE_DATATYPE " +
                             " from TB_PROD_SPECS A " +                             
                             " Inner Join TB_ATTRIBUTE B On A.ATTRIBUTE_ID=B.ATTRIBUTE_ID " +
                             " where B.PUBLISH2WEB = 1 and A.ATTRIBUTE_ID not in (1,453,452, 7) And A.PRODUCT_ID=" + res.getCellData(0, colProductID).ToString();

                    if (StrSql != "")
                    {
                        Sqltb = objhelper.GetDataTable(StrSql);

                        if (Sqltb != null)
                        {
                            foreach (DataRow dr in Sqltb.Rows)
                            {
                                dRow["ATTRIBUTE_ID"] = dr["ATTRIBUTE_ID"];
                                dRow["STRING_VALUE"] = dr["STRING_VALUE"];
                                dRow["NUMERIC_VALUE"] = dr["NUMERIC_VALUE"];
                                dRow["OBJECT_TYPE"] = dr["OBJECT_TYPE"];
                                dRow["OBJECT_NAME"] = dr["OBJECT_NAME"];
                                dRow["ATTRIBUTE_NAME"] = dr["ATTRIBUTE_NAME"];
                                dRow["ATTRIBUTE_TYPE"] = dr["ATTRIBUTE_TYPE"];
                                dRow["ATTRIBUTE_DATATYPE"] = dr["ATTRIBUTE_DATATYPE"];                                
                                ds.Rows.Add(dRow.ItemArray);
                            }
                        }
                    }
                    //this Query ref STP->GetProductImages
                    Sqltb = new DataTable();
                    StrSql = "select FS.STRING_VALUE,FS.ATTRIBUTE_ID,FS.NUMERIC_VALUE,FS.OBJECT_TYPE,FS.OBJECT_NAME,A.ATTRIBUTE_NAME,A.ATTRIBUTE_TYPE,A.ATTRIBUTE_DATATYPE " +
                             " from TB_FAMILY F " +
                             " Inner Join TB_FAMILY_SPECS FS On fs.FAMILY_ID=F.FAMILY_ID" +
                             " Inner Join TB_ATTRIBUTE A On A.ATTRIBUTE_ID=FS.ATTRIBUTE_ID " +
                             " where A.PUBLISH2WEB = 1 and A.ATTRIBUTE_ID not in (746, 747) And F.FAMILY_ID=" + ((_subFamID == "") ? _FamID : _subFamID).ToString();

                    DataRow[] Dr = ds.Select("ATTRIBUTE_TYPE=7");
                    if (Dr.Length <= 0)                    
                       StrSql=StrSql+" And A.ATTRIBUTE_TYPE in (7,9)";                    
                    else
                       StrSql = StrSql + " And A.ATTRIBUTE_TYPE=9";

                    if (StrSql != "")
                    {
                        Sqltb = objhelper.GetDataTable(StrSql);

                        if (Sqltb != null)
                        {
                            foreach (DataRow dr in Sqltb.Rows)
                            {
                                dRow["ATTRIBUTE_ID"] = dr["ATTRIBUTE_ID"];
                                dRow["STRING_VALUE"] = dr["STRING_VALUE"];
                                dRow["NUMERIC_VALUE"] = dr["NUMERIC_VALUE"];
                                dRow["OBJECT_TYPE"] = dr["OBJECT_TYPE"];
                                dRow["OBJECT_NAME"] = dr["OBJECT_NAME"];
                                dRow["ATTRIBUTE_NAME"] = dr["ATTRIBUTE_NAME"];
                                dRow["ATTRIBUTE_TYPE"] = dr["ATTRIBUTE_TYPE"];
                                dRow["ATTRIBUTE_DATATYPE"] = dr["ATTRIBUTE_DATATYPE"];
                                ds.Rows.Add(dRow.ItemArray);
                            }
                        }
                    }
                  



                }
                  

            }
            catch (Exception ex)
            {
            }
            return ds;
        }
        DataSet AttributePro = new DataSet();
        DataTable AttributeFamPro = new DataTable("FamilyPro");
        DataTable AttributeFam = new DataTable("Family");
        //DataTable AttributeSubFam = new DataTable("SubFamily");

        public DataSet GetAttributeProducts(string DataPage, string SearchStr, string AttributeType, string AttributeValue, string Brand, string resultPerPage, string CurrentPageNo, string NextPage)
        {
            try
            {

                string temp;
                string temp1;
                IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WebCatDictionary);
                IOptions opts = ea.getOptions();
                opts.setResultsPerPage(resultPerPage); // ea_rpp.Value);   // use current settings
                opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
                opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);//"Category;1000"

                if (DataPage == "FamilyPage" || DataPage == "ProductPage")
                {
                    opts.setSubCategories(false);
                    opts.setReturnSKUs(true);
                }
                else
                {
                    opts.setSubCategories(true);
                    opts.setReturnSKUs(false);
                }
                opts.setNavigateHierarchy(false);

                int PriceCode = GetPriceCode();
                if (PriceCode != -1)
                    opts.setCallOutParam("&eap_PriceCode=" + PriceCode.ToString());


                
                if (DataPage == "FamilyPage")
                {
                    AttributePro.Tables.Add(AttributeFam);
                    AttributeFam.Columns.Add("FAMILY_ID", typeof(string));
                    AttributeFam.Columns.Add("FAMILY_NAME", typeof(string));
                    AttributeFam.Columns.Add("STRING_VALUE", typeof(string));
                    AttributeFam.Columns.Add("NUMERIC_VALUE", typeof(string));
                    AttributeFam.Columns.Add("ATTRIBUTE_ID", typeof(string));
                    AttributeFam.Columns.Add("ATTRIBUTE_NAME", typeof(string));
                    AttributeFam.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
                    AttributeFam.Columns.Add("OBJECT_TYPE", typeof(string));
                    AttributeFam.Columns.Add("OBJECT_NAME", typeof(string));
                    AttributeFam.Columns.Add("ATTRIBUTE_DATA_TYPE", typeof(string));
                    AttributeFam.Columns.Add("FAMILY_PROD_COUNT", typeof(string));
                    AttributeFam.Columns.Add("PROD_COUNT", typeof(string));
                    AttributeFam.Columns.Add("STATUS", typeof(string));

                    AttributePro.Tables.Add(AttributeFamPro);
                    AttributeFamPro.Columns.Add("FAMILY_ID", typeof(string));
                    AttributeFamPro.Columns.Add("FAMILY_NAME", typeof(string));
                    AttributeFamPro.Columns.Add("FAMILY_TH_IMAGE", typeof(string));
                    AttributeFamPro.Columns.Add("FAMILY_SHORT_DESC", typeof(string));
                    AttributeFamPro.Columns.Add("FAMILY_DESC", typeof(string));
                    AttributeFamPro.Columns.Add("PRODUCT_ID", typeof(string));
                    AttributeFamPro.Columns.Add("PRODUCT_CODE", typeof(string));
                    AttributeFamPro.Columns.Add("PRODUCT_PRICE", typeof(string));
                    AttributeFamPro.Columns.Add("PRODUCT_DESC", typeof(string));
                    AttributeFamPro.Columns.Add("PRODUCT_TH_IMAGE", typeof(string));
                    AttributeFamPro.Columns.Add("SUB_FAMILY_COUNT", typeof(string));
                    AttributeFamPro.Columns.Add("PRODUCT_COUNT", typeof(string));
                    AttributeFamPro.Columns.Add("FAMILY_PRODUCT_COUNT", typeof(string));
                    AttributeFamPro.Columns.Add("QTY_AVAIL", typeof(string));
                    AttributeFamPro.Columns.Add("MIN_ORD_QTY", typeof(string));


                }
                else if (DataPage == "ProductPage")
                {
                    AttributePro.Tables.Add(AttributeFamPro);
                    AttributeFamPro.Columns.Add("PRODUCT_ID", typeof(string));
                    AttributeFamPro.Columns.Add("STRING_VALUE", typeof(string));
                    AttributeFamPro.Columns.Add("NUMERIC_VALUE", typeof(string));
                    AttributeFamPro.Columns.Add("ATTRIBUTE_ID", typeof(string));
                    AttributeFamPro.Columns.Add("ATTRIBUTE_NAME", typeof(string));
                    AttributeFamPro.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
                    AttributeFamPro.Columns.Add("OBJECT_TYPE", typeof(string));
                    AttributeFamPro.Columns.Add("OBJECT_NAME", typeof(string));
                    AttributeFamPro.Columns.Add("ATTRIBUTE_DATATYPE", typeof(string));
                    AttributeFamPro.Columns.Add("QTY_AVAIL", typeof(string));
                    AttributeFamPro.Columns.Add("MIN_ORD_QTY", typeof(string));
                    AttributeFamPro.Columns.Add("FAMILY_ID", typeof(string));
                    AttributeFamPro.Columns.Add("FAMILY_NAME", typeof(string));
                    AttributeFamPro.Columns.Add("FAMILY_TH_IMAGE", typeof(string));
                    AttributeFamPro.Columns.Add("FAMILY_SHORT_DESC", typeof(string));
                    AttributeFamPro.Columns.Add("FAMILY_DESC", typeof(string));
                    AttributeFamPro.Columns.Add("STOCK_STATUS_DESC", typeof(string));
                    AttributeFamPro.Columns.Add("FAMILY_PROD_COUNT", typeof(string));
                    AttributeFamPro.Columns.Add("PROD_COUNT", typeof(string));          
                    //AttributePro.Tables.Add(AttributeFam);
                    //AttributeFam.Columns.Add("FAMILY_ID", typeof(string));
                    //AttributeFam.Columns.Add("FAMILY_NAME", typeof(string));
                    //AttributeFam.Columns.Add("FAMILY_TH_IMAGE", typeof(string));
                    //AttributeFam.Columns.Add("FAMILY_SHORT_DESC", typeof(string));
                    //AttributeFam.Columns.Add("FAMILY_DESC", typeof(string));
                    


                }
                else
                {
                    AttributePro.Tables.Add("TOTAL_PAGES");
                    AttributePro.Tables["TOTAL_PAGES"].Columns.Add("TOTAL_PAGES", typeof(int));

                    //For Total Products.
                    AttributePro.Tables.Add("TOTAL_PRODUCTS");
                    AttributePro.Tables["TOTAL_PRODUCTS"].Columns.Add("TOTAL_PRODUCTS", typeof(string));



                    AttributePro.Tables.Add(AttributeFamPro);
                    AttributeFamPro.Columns.Add("FAMILY_ID", typeof(string));
                    AttributeFamPro.Columns.Add("CATEGORY_ID", typeof(string));
                    AttributeFamPro.Columns.Add("PARENT_CATEGORY_ID", typeof(string));
                    AttributeFamPro.Columns.Add("FAMILY_NAME", typeof(string));
                    AttributeFamPro.Columns.Add("PRODUCT_ID", typeof(string));
                    AttributeFamPro.Columns.Add("PRODUCT_CODE", typeof(string));
                    AttributeFamPro.Columns.Add("PRODUCT_PRICE", typeof(string));
                    AttributeFamPro.Columns.Add("ATTRIBUTE_ID", typeof(string));
                    AttributeFamPro.Columns.Add("STRING_VALUE", typeof(string));
                    AttributeFamPro.Columns.Add("NUMERIC_VALUE", typeof(string));
                    AttributeFamPro.Columns.Add("OBJECT_TYPE", typeof(string));
                    AttributeFamPro.Columns.Add("OBJECT_NAME", typeof(string));
                    AttributeFamPro.Columns.Add("ATTRIBUTE_NAME", typeof(string));
                    AttributeFamPro.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
                    AttributeFamPro.Columns.Add("SUBCATNAME_L1", typeof(string));
                    AttributeFamPro.Columns.Add("SUBCATNAME_L2", typeof(string));
                    AttributeFamPro.Columns.Add("CATEGORY_NAME", typeof(string));
                    AttributeFamPro.Columns.Add("PARENT_CATEGORY_NAME", typeof(string));
                    AttributeFamPro.Columns.Add("CUSTOM_NUM_FIELD3", typeof(string));
                    AttributeFamPro.Columns.Add("SUB_FAMILY_COUNT", typeof(string));
                    AttributeFamPro.Columns.Add("PRODUCT_COUNT", typeof(string));
                    AttributeFamPro.Columns.Add("FAMILY_PRODUCT_COUNT", typeof(string));
                    AttributeFamPro.Columns.Add("QTY_AVAIL", typeof(string));
                    AttributeFamPro.Columns.Add("MIN_ORD_QTY", typeof(string));

                }


                INavigateResults res = null;


                if (int.Parse(CurrentPageNo) <= 0)
                {
                    if (SearchStr != "" && AttributeValue == "" && AttributeType == "")
                    {
                        res = ea.userSearch("AllProducts////WESAUSTRALASIA", SearchStr);
                    }
                    else
                    {
                        if (AttributeType == "Category")
                        {
                            if (HttpContext.Current.Session["EA"] != null)
                            {
                                if (HttpContext.Current.Session["EA"].ToString().EndsWith("////" + AttributeValue))
                                    res = ea.userBreadCrumbClick(HttpContext.Current.Session["EA"].ToString());
                                else
                                    res = ea.userBreadCrumbClick(HttpContext.Current.Session["EA"].ToString() + "////" + AttributeValue);
                                //HttpContext.Current.Session["EA"] = res.getCatPath();
                            }

                        }
                        else if (AttributeType == "FamilyId")
                        {
                            if (HttpContext.Current.Session["EA"] != null)
                            {
                                if (HttpContext.Current.Session["EA"].ToString().EndsWith("Family Id=" + AttributeValue))
                                {
                                    HttpContext.Current.Session["EA"] = HttpContext.Current.Session["EA"].ToString().Replace("Family Id=" + AttributeValue, "");
                                    res = ea.userSearch(HttpContext.Current.Session["EA"].ToString(), "Family Id=" + AttributeValue);
                                }
                                else
                                    res = ea.userSearch(HttpContext.Current.Session["EA"].ToString(), "Family Id=" + AttributeValue);
                            }

                        }
                        else if (AttributeType == "ProductId")
                        {
                            if (HttpContext.Current.Session["EA"] != null)
                            {
                                if (HttpContext.Current.Session["EA"].ToString().EndsWith("Prod Id=" + AttributeValue))
                                {
                                    HttpContext.Current.Session["EA"] = HttpContext.Current.Session["EA"].ToString().Replace("Prod Id=" + AttributeValue, "");
                                    res = ea.userSearch(HttpContext.Current.Session["EA"].ToString(), "Prod Id=" + AttributeValue);
                                }
                                else
                                    res = ea.userSearch(HttpContext.Current.Session["EA"].ToString(), "Prod Id=" + AttributeValue);
                            }

                        }
                        else
                        {
                            if (AttributeType == "Model")
                            {

                                temp = "" + AttributeType + " = '" + HttpUtility.UrlEncode(Brand) + ":" + HttpUtility.UrlEncode(AttributeValue) + "'";
                                temp1 = "" + AttributeType + " = ";
                            }
                            else
                            {
                                temp = "" + AttributeType + " = '" + HttpUtility.UrlEncode(AttributeValue) + "'";
                                temp1 = "" + AttributeType + " = ";
                            }
                            if (HttpContext.Current.Session["EA"] != null)
                            {
                                if (HttpContext.Current.Session["EA"].ToString().Contains(temp1))
                                {
                                    int t = HttpContext.Current.Session["EA"].ToString().IndexOf(temp1) - 17;
                                    string t1 = HttpContext.Current.Session["EA"].ToString().Remove(t);
                                    HttpContext.Current.Session["EA"] = t1;

                                }

                                res = ea.userAttributeClick(HttpContext.Current.Session["EA"].ToString(), temp);
                                //HttpContext.Current.Session["EA"] = res.getCatPath();

                            }
                        }
                    }

                }
                else
                {
                    if (HttpContext.Current.Session["EA"] != null)
                    {
                        res = ea.userPageOp(HttpContext.Current.Session["EA"].ToString(), CurrentPageNo, NextPage);
                    }
                }

                HttpContext.Current.Session["EA"] = res.getCatPath();
                CreateYouHaveSelectAndBreadCrumb();
                if (DataPage != "FamilyPage" && DataPage != "ProductPage")
                {
                    DataRow dr = AttributePro.Tables["TOTAL_PAGES"].NewRow();
                    dr[0] = res.getPageCount();
                    AttributePro.Tables["TOTAL_PAGES"].Rows.Add(dr);

                    DataRow dr1 = AttributePro.Tables["TOTAL_PRODUCTS"].NewRow();
                    dr1[0] = res.getTotalItems();
                    AttributePro.Tables["TOTAL_PRODUCTS"].Rows.Add(dr1);
                }

                if (DataPage == "FamilyPage")
                    Get_FamilyPage_Product_Values(DataPage, AttributeFam, AttributeFamPro, res, null, null);
                else if (DataPage == "ProductPage")
                    Get_ProductPage_Product_Values(DataPage, AttributeFam, AttributeFamPro, res, null, null);                        
                else
                    Get_Family_Product_Values(DataPage, AttributeFamPro, res, null, null);


                IList<string> Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
                if (AttributeType == "Brand")
                    HttpContext.Current.Session["LHSAttributes"] = GetCategoryAttribute(Attributes, res, AttributeValue, SearchStr);
                else
                    HttpContext.Current.Session["LHSAttributes"] = GetCategoryAttribute(Attributes, res, "", SearchStr);

                if (DataPage == "ProductList" || DataPage == "PowerSearch")
                {
                    AttributeFam = AttributeFamPro.DefaultView.ToTable(true, "Family_Id", "Family_Name", "FAMILY_PRODUCT_COUNT", "SUB_FAMILY_COUNT", "PRODUCT_COUNT").Copy();
                    AttributeFam.TableName = "Family";
                    AttributePro.Tables.Add(AttributeFam);
                }
                HttpContext.Current.Session["FamilyProduct"] = AttributePro;



                return AttributePro;
            }
            catch (Exception ex)
            {
                objErrorhandler.ErrorMsg = ex;
                objErrorhandler.CreateLog();
                return null;
            }
            finally
            {
                AttributePro = null;
                AttributeFamPro = null;
                AttributeFam = null;
            }
        }


      
        public void GetDataSet1(INavigateResults res)
        {
            try
            {
                IList<INavigateAttribute> Brand_list = res.getDetailedAttributeValues("Brand");
                foreach (INavigateAttribute item in Brand_list)
                {
                    DataRow row = tbl_Brand.NewRow();
                    row["TOSUITE_BRAND"] = item.getValue();
                    tbl_Brand.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
            }
            HttpContext.Current.Session["WESBrand"] = Brand;
        }

        //Store EasyAsk Values to Session.
        //public void Get_SessionData()
        //{
        //    GetDataSet();
        //    if (Category.Tables.Count > 0)
        //    {
        //        HttpContext.Current.Session["Category"] = Category;
        //    }
        //}

        void Create_DataTable_Columns()
        {
            try
            {
                //For SubCategory Table.
                SubCategory.Tables.Add(Sub_Category);
                Sub_Category.Columns.Add("TBT_PARENT_CATEGORY_ID", typeof(string));
                Sub_Category.Columns.Add("TBT_PARENT_CATEGORY_NAME", typeof(string));
                Sub_Category.Columns.Add("TBT_PARENT_CATEGORY_IMAGE", typeof(string));
                Sub_Category.Columns.Add("CATEGORY_NAME", typeof(string));
                Sub_Category.Columns.Add("CATEGORY_ID", typeof(string));
                Sub_Category.Columns.Add("TBT_SHORT_DESC", typeof(string));
                Sub_Category.Columns.Add("TBT_CUSTOM_NUM_FIELD3", typeof(string));
                Sub_Category.Columns.Add("EA_PATH", typeof(string));
                
                //For Category Table.
                MainCategory.Tables.Add(Main_Category);
                Main_Category.Columns.Add("CATEGORY_NAME", typeof(string));
                Main_Category.Columns.Add("CATEGORY_ID", typeof(string));
                Main_Category.Columns.Add("PARENT_CATEGORY", typeof(string));
                Main_Category.Columns.Add("SHORT_DESC", typeof(string));
                Main_Category.Columns.Add("IMAGE_FILE", typeof(string));
                Main_Category.Columns.Add("IMAGE_FILE2", typeof(string));
                Main_Category.Columns.Add("CUSTOM_NUM_FIELD3", typeof(string));
                Main_Category.Columns.Add("EA_PATH", typeof(string));

                //For Brand Table.
                Brand.Tables.Add(tbl_Brand);
                tbl_Brand.Columns.Add("TOSUITE_BRAND", typeof(string));
            }
            catch (Exception ex)
            {
            }
        }

        public DataSet GetWESModel(string parentCategoryName, int CatalogID, string tosuite_brand)
        {
            string tmpEaPath ="";
            string tmpCatPath ="";
             Boolean blnGetData=false;  
            if(parentCategoryName!="")
            {
                tmpCatPath ="////" +parentCategoryName;
            }
            
            if (HttpContext.Current.Session["WESBrand_Model"] == null)
            {
                blnGetData = true; 
            }
            else
            {
                if (((DataSet)HttpContext.Current.Session["WESBrand_Model"]).Tables["ParentCategory"].Rows[0]["CATNAME"].ToString().ToUpper() == parentCategoryName.ToUpper() && ((DataSet)HttpContext.Current.Session["WESBrand_Model"]).Tables["ParentCategory"].Rows[0]["BRANDNAME"].ToString().ToUpper() == tosuite_brand.ToUpper())
                {
                    blnGetData = false;
                }               
                else
                {
                    blnGetData = true;
                }

            
            }
            tmpEaPath = "AllProducts////WESAUSTRALASIA" + tmpCatPath + "////AttribSelect=Brand='" + tosuite_brand + "'";
            if (blnGetData == true)
            {



                IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WesCatBrandDictionary);
                IOptions opts = ea.getOptions();
                opts.setResultsPerPage("0"); // ea_rpp.Value);   // use current settings
                opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
                opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);
                opts.setSubCategories(false);
                opts.setNavigateHierarchy(false);
                opts.setReturnSKUs(false);
                Brand_Model = new DataSet();

                DataTable dTable = new DataTable("Model");
                DataTable dTable1 = new DataTable("ParentCategory");


                INavigateResults res = ea.userAttributeClick("AllProducts////WESAUSTRALASIA" + tmpCatPath, "Brand='" + tosuite_brand + "'");
                //updateBreadCrumb(res.getBreadCrumbTrail());
                HttpContext.Current.Session["Brand_Path"] = res.getCatPath();
                HttpContext.Current.Session["EA"] = res.getCatPath();
                CreateYouHaveSelectAndBreadCrumb();
                try
                {
                    int last = res.getLastItem();
                    int Model_Name = res.getColumnIndex("ModelValue");
                    int Model_Image = res.getColumnIndex("Model Thumbnail");
                    int Brand_Name = res.getColumnIndex("Brand");
                    // IList Model = res.getDetailedAttributeValues("Model");
                    Brand_Model.Tables.Add(dTable);
                    dTable.Columns.Add("TOSUITE_MODEL", typeof(string));
                    dTable.Columns.Add("IMAGE_FILE", typeof(string));
                    dTable.Columns.Add("Brand", typeof(string));

                    Brand_Model.Tables.Add(dTable1);
                    dTable1.Columns.Add("CATNAME", typeof(string));
                    dTable1.Columns.Add("BRANDNAME", typeof(string));


                    DataRow row2 = dTable1.NewRow();
                    row2["CATNAME"] = parentCategoryName.ToUpper();
                    row2["BRANDNAME"] = tosuite_brand.ToUpper();

                    dTable1.Rows.Add(row2);


                    string image_string = "";
                    DataRow dRow;
                    for (int i = res.getFirstItem() - 1; i < last; i++)
                    {
                        dRow = dTable.NewRow();
                        dRow["TOSUITE_MODEL"] = res.getCellData(i, Model_Name);
                        // dRow["IMAGE_FILE"] = 
                        string Model_Image_Name = res.getCellData(i, Model_Image).ToString();
                        if (Model_Image_Name != "" && Model_Image_Name != null)
                        {
                            image_string = Model_Image_Name.Substring(42);
                        }
                        else
                        {
                            image_string = "noimage.gif";
                        }
                        dRow["IMAGE_FILE"] = image_string;
                        dRow["Brand"] = res.getCellData(i, Brand_Name);
                        dTable.Rows.Add(dRow);
                    }

                    IList<string> Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
                    HttpContext.Current.Session["WESBrand_Model_Attributes"] = GetCategoryAttribute(Attributes, res, "", "");
                 
                }
                

                catch (Exception)
                {
                    return null;
                }
                HttpContext.Current.Session["WESBrand_Model"] = Brand_Model;
                return Brand_Model;
            }
            else if (HttpContext.Current.Session["EA"].ToString().ToUpper() == tmpEaPath.ToUpper())
            {
                HttpContext.Current.Session["EA"] = tmpEaPath;
                CreateYouHaveSelectAndBreadCrumb();
            }
            
            return (DataSet) HttpContext.Current.Session["WESBrand_Model"] ;
        


        }

        //--------------------------------------------------------------------------------

        //public DataSet getCategory_List(string _catId)
        //{
        //    DataSet ds = new DataSet();
        //    DataTable Category_List = new DataTable();
        //    Category_List.Columns.Add("category_id", typeof(string));
        //    Category_List.Columns.Add("category_name", typeof(string));
        //    Category_List.Columns.Add("image_file", typeof(string));
        //    Category_List.Columns.Add("custom_num_field3", typeof(string));
        //    Category_List.Columns.Add("parent_category", typeof(string));
        //    ds.Tables.Add(Category_List);

        //    if (HttpContext.Current.Session["Category"] != null)
        //    {
        //        DataSet tempdata = (DataSet)HttpContext.Current.Session["Category"];
        //        foreach (DataRow dr in tempdata.Tables[0].Rows)
        //        {
        //            if (dr.ItemArray.GetValue(0).ToString() == _catId)
        //            {
        //                DataRow row = Category_List.NewRow();
        //                row["category_id"] = dr.ItemArray.GetValue(4).ToString();
        //                row["category_name"] = dr.ItemArray.GetValue(3).ToString();
        //                row["image_file"] = string.Empty;
        //                row["custom_num_field3"] = "2";
        //                row["parent_category"] = dr.ItemArray.GetValue(0).ToString();
        //                Category_List.Rows.Add(row);
        //            }
        //        }
        //    }
        //    return ds;
        //}

        //public DataSet Category_Menu_Click_BreadCrumbs(string _catId)
        //{
        //    DataSet ds = new DataSet();
        //    DataTable Bread_Crumbs = new DataTable();
        //    Bread_Crumbs.Columns.Add("CATEGORY_NAME", typeof(string));
        //    Bread_Crumbs.Columns.Add("PARENT_CATEGORY", typeof(string));
        //    Bread_Crumbs.Columns.Add("CATEGORY_ID", typeof(string));
        //    ds.Tables.Add(Bread_Crumbs);

        //    if (HttpContext.Current.Session["Category"] != null)
        //    {
        //        DataSet tempdata = (DataSet)HttpContext.Current.Session["Category"];
        //        foreach (DataRow dr in tempdata.Tables[1].Rows)
        //        {
        //            if (dr.ItemArray.GetValue(1).ToString() == _catId)
        //            {
        //                DataRow row = Bread_Crumbs.NewRow();
        //                row["CATEGORY_ID"] = dr.ItemArray.GetValue(1).ToString();
        //                row["PARENT_CATEGORY"] = "WES0830";
        //                row["CATEGORY_NAME"] = dr.ItemArray.GetValue(0).ToString();
        //                Bread_Crumbs.Rows.Add(row);
        //            }
        //        }
        //    }
        //    return ds;
        //}

        #endregion

        //#region "For the homepage search"

        //DataSet HomeSearch = new DataSet();
        //DataTable Table = new DataTable();//
        //DataTable Table1 = new DataTable();//Total Count
        //DataTable Table2 = new DataTable();
        //DataTable Table3 = new DataTable();
        //ErrorHandler objErrorhandler = new ErrorHandler();

        //void Create_Search_Table_Columns()
        //{
        //    //For Total Pages.
        //    HomeSearch.Tables.Add(Table);
        //    Table.Columns.Add("TOTAL_PAGES", typeof(int));

        //    //For Total Products.
        //    HomeSearch.Tables.Add(Table1);
        //    Table1.Columns.Add("TOTAL_PRODUCTS", typeof(string));

        //    //For Display Sarch Result.
        //    HomeSearch.Tables.Add(Table2);
        //    Table2.Columns.Add("FAMILY_ID", typeof(string));
        //    Table2.Columns.Add("FAMILY_NAME", typeof(string));
        //    Table2.Columns.Add("DESCRIPTION1", typeof(string));
        //    Table2.Columns.Add("LongDESCRIPTION", typeof(string));
        //    Table2.Columns.Add("PRODUCT_ID", typeof(string));
        //    Table2.Columns.Add("ATTRIBUTE_ID", typeof(string));
        //    Table2.Columns.Add("STRING_VALUE", typeof(string));
        //    Table2.Columns.Add("NUMERIC_VALUE", typeof(string));
        //    Table2.Columns.Add("OBJECT_TYPE", typeof(string));
        //    Table2.Columns.Add("OBJECT_NAME", typeof(string));
        //    Table2.Columns.Add("ATTRIBUTE_NAME", typeof(string));
        //    Table2.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
        //    Table2.Columns.Add("CATEGORY_ID", typeof(string));
        //    Table2.Columns.Add("CATEGORY_NAME", typeof(string));
        //    Table2.Columns.Add("PARENT_CATEGORY_NAME", typeof(string));
        //    Table2.Columns.Add("PARENT_CATEGORY_ID", typeof(string));
        //    Table2.Columns.Add("CUSTOM_NUM_FIELD3", typeof(string));
        //    Table2.Columns.Add("PARENT_FAMILY_ID", typeof(string));
        //    Table2.Columns.Add("(No column name)", typeof(string));
        //    Table2.Columns.Add("LFID", typeof(string));
        //    Table2.Columns.Add("Family_Prod_Count", typeof(int));
        //    Table2.Columns.Add("Prod_Count", typeof(int));
        //    Table2.Columns.Add("STATUS", typeof(bool));

        //    //For Sarch Result Related.
        //    HomeSearch.Tables.Add(Table3);
        //    Table3.Columns.Add("CATALOG_ID", typeof(string));
        //    Table3.Columns.Add("PRODUCT_ID", typeof(string));
        //    Table3.Columns.Add("STATUS", typeof(string));
        //}

        ////public DataSet Get_Homepage_UserSearch(string srctxt, int curPage, string pageOp)
        ////{
        ////    try
        ////    {
        ////        RemoteEasyAsk();
        ////        DataSet ds = new DataSet();
        ////        Create_Search_Table_Columns();

        ////        if (curPage < 1)
        ////        {
        ////            INavigateResults res = ea.userSearch("AllProducts////WESAUSTRALASIA////Cellular Accessories", srctxt);
        ////            ds = Get_DataSet_Values(res);
        ////        }
        ////        else
        ////        {
        ////            INavigateResults res1 = ea.userPageOp("AllProducts////WESAUSTRALASIA////Cellular Accessories////UserSearch1=" + srctxt + "", curPage.ToString(), pageOp);
        ////            ds = Get_DataSet_Values(res1);
        ////        }
        ////        return ds;
        ////    }
        ////    catch (Exception)
        ////    {
        ////        return null;
        ////    }
        ////}

        //DataSet Get_DataSet_Values(INavigateResults res)
        //{
        //    DataRow dr = Table.NewRow();
        //    dr[0] = res.getPageCount();
        //    HomeSearch.Tables[0].Rows.Add(dr);

        //    DataRow dr1 = Table1.NewRow();
        //    dr1[0] = res.getTotalItems();
        //    HomeSearch.Tables[1].Rows.Add(dr1);

        //    int last = res.getLastItem();
        //    int colFmlyID = res.getColumnIndex("Family Id");
        //    int colFmlyName = res.getColumnIndex("Family Name");
        //    int colFmlyDesc = res.getColumnIndex("Family ShortDescription");
        //    int colFmlylongDesc = res.getColumnIndex("Family Description");
        //    int colFmlyImg = res.getColumnIndex("Family Thumbnail");

        //    int colProductID = res.getColumnIndex("Prod Id");
        //    int colProductCode = res.getColumnIndex("Prod Code");
        //    int colProductPrice = res.getColumnIndex("Price");
        //    int colProductDesc = res.getColumnIndex("Prod Description");
        //    int colProductImg = res.getColumnIndex("Prod Thumbnail");

        //    int colProductCount = res.getColumnIndex("Prod Count");
        //    int colFamilyProdCount = res.getColumnIndex("Family Prod Count");

        //    IList<INavigateCategory> item = res.getDetailedCategories();
        //    DataRow dRow;
        //    try
        //    {
        //        if (last >= 0)
        //        {
        //            for (int i = res.getFirstItem() - 1, col = 0; i < last; i++, col++)
        //            {
        //                for (int k = 0; k < 9; k++)
        //                {
        //                    dRow = Table2.NewRow();
        //                    dRow["FAMILY_ID"] = res.getCellData(i, colFmlyID);
        //                    dRow["FAMILY_NAME"] = res.getCellData(i, colFmlyName);
        //                    dRow["DESCRIPTION1"] = res.getCellData(i, colFmlyDesc);
        //                    dRow["LongDESCRIPTION"] = res.getCellData(i, colFmlylongDesc);
        //                    dRow["PRODUCT_ID"] = res.getCellData(i, colProductID);

        //                    string temp_family_count = res.getCellData(i, colFamilyProdCount);

        //                    string temp_product_count = res.getCellData(i, colProductCount);
        //                    string temp_fmly_Image = res.getCellData(i, colFmlyImg).ToString();
        //                    string temp_product_Image = res.getCellData(i, colProductImg).ToString();
        //                    string image_string = "";

        //                    if (temp_fmly_Image != "" && temp_product_Image != "")
        //                    {
        //                        if (temp_product_count.ToString() == "1")
        //                        {
        //                            image_string = temp_product_Image.Substring(42);
        //                        }
        //                        else
        //                        {
        //                            image_string = temp_fmly_Image.Substring(42);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        image_string = "noimage.gif";
        //                    }

        //                    if (k == 0)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "1";
        //                        dRow["STRING_VALUE"] = res.getCellData(i, colProductCode);//For the Product Code
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "Code";
        //                        dRow["ATTRIBUTE_TYPE"] = "1";
        //                    }
        //                    if (k == 1)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "5";
        //                        dRow["STRING_VALUE"] = "";
        //                        if (res.getCellData(i, colProductPrice) == "" || res.getCellData(i, colProductPrice) == string.Empty)
        //                        {
        //                            dRow["NUMERIC_VALUE"] = "0";
        //                        }
        //                        else
        //                        {
        //                            dRow["NUMERIC_VALUE"] = res.getCellData(i, colProductPrice).Substring(1);//For Cost
        //                        }
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "Cost";
        //                        dRow["ATTRIBUTE_TYPE"] = "4";
        //                    }
        //                    if (k == 2)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "62";
        //                        dRow["STRING_VALUE"] = res.getCellData(i, colFmlyDesc);//Family Description
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "Description";
        //                        dRow["ATTRIBUTE_TYPE"] = "1";
        //                    }
        //                    if (k == 3)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "449";
        //                        dRow["STRING_VALUE"] = res.getCellData(i, colProductDesc);//Product Description.
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "PROD_DSC";
        //                        dRow["ATTRIBUTE_TYPE"] = "1";
        //                    }
        //                    if (k == 4)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "492";
        //                        dRow["STRING_VALUE"] = "";
        //                        if (res.getCellData(i, colProductPrice) == "" || res.getCellData(i, colProductPrice) == string.Empty)
        //                        {
        //                            dRow["NUMERIC_VALUE"] = "0";
        //                        }
        //                        else
        //                        {
        //                            dRow["NUMERIC_VALUE"] = res.getCellData(i, colProductPrice).Substring(1);//For Cost
        //                        }
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "PROD_EXT_PRI_3";
        //                        dRow["ATTRIBUTE_TYPE"] = "4";
        //                    }
        //                    if (k == 5)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "453";
        //                        dRow["STRING_VALUE"] = image_string;
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "jpg";
        //                        dRow["OBJECT_NAME"] = image_string;
        //                        dRow["ATTRIBUTE_NAME"] = "Web Image1";
        //                        dRow["ATTRIBUTE_TYPE"] = "3";
        //                    }
        //                    if (k == 6)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "7";
        //                        dRow["STRING_VALUE"] = image_string;
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "jpg";
        //                        dRow["OBJECT_NAME"] = image_string;
        //                        dRow["ATTRIBUTE_NAME"] = "Product Image1";
        //                        dRow["ATTRIBUTE_TYPE"] = "3";
        //                    }
        //                    if (k == 7)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "452";
        //                        dRow["STRING_VALUE"] = image_string;
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "jpg";
        //                        dRow["OBJECT_NAME"] = image_string;
        //                        dRow["ATTRIBUTE_NAME"] = "TWeb Image1";
        //                        dRow["ATTRIBUTE_TYPE"] = "3";
        //                    }
        //                    if (k == 8)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "4";
        //                        dRow["STRING_VALUE"] = res.getCellData(i, colFmlylongDesc);//Family Description
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "DESCRIPTIONS";
        //                        dRow["ATTRIBUTE_TYPE"] = "1";
        //                    }

        //                    dRow["CATEGORY_ID"] = "";
        //                    dRow["CATEGORY_NAME"] = string.Empty;
        //                    dRow["PARENT_CATEGORY_NAME"] = "";
        //                    dRow["PARENT_CATEGORY_ID"] = "";
        //                    dRow["CUSTOM_NUM_FIELD3"] = "2";
        //                    dRow["PARENT_FAMILY_ID"] = "0";
        //                    dRow["(No column name)"] = "";
        //                    dRow["LFID"] = res.getCellData(i, colFmlyID);
        //                    dRow["Family_Prod_Count"] = temp_family_count;
        //                    dRow["Prod_Count"] = temp_product_count;
        //                  //  HttpContext.Current.Session["FamilyCount"] = temp_family_count;
        //                    if (temp_family_count == temp_product_count)
        //                    {
        //                        dRow["STATUS"] = true;
        //                    }
        //                    else
        //                    {
        //                        dRow["STATUS"] = false;
        //                    }
        //                    Table2.Rows.Add(dRow);
        //                }
        //                j++;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    HttpContext.Current.Session["HomeSearch"] = HomeSearch;
        //    return HomeSearch;
        //}

        //#endregion

        //#region "For Menu Click Results"

        //DataSet Menu_Click = new DataSet();
        //DataTable Mnu_Temp = new DataTable();
        //DataTable Mnu_Table = new DataTable();
        //DataTable Mnu_Table1 = new DataTable();
        //DataTable Mnu_Table2 = new DataTable();
        //DataTable Mnu_Table3 = new DataTable();

        //public DataSet Category_Menu_Click(string pid, string cname, int curPage, string temptext, string pageOp)
        //{
        //    try
        //    {
        //        IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WebCatDictionary);
        //        IOptions opts = ea.getOptions();
        //        opts.setResultsPerPage(m_rpp); // ea_rpp.Value);   // use current settings
        //        opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
        //        opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);
        //        opts.setSubCategories(true);
        //        opts.setNavigateHierarchy(false);
        //        opts.setReturnSKUs(false);  
        //        Create_Menu_Table_Columns();
        //        INavigateResults res = null;
        //        if (curPage < 1)
        //        {
        //            string Encode_Path = "AllProducts////WESAUSTRALASIA////" + cname;
        //            res = ea.userCategoryClick(Encode_Path);
        //            if (HttpContext.Current.Request["pgno"] == null)
        //            {
        //                updateBreadCrumb(res.getBreadCrumbTrail(), "", "");
        //            }
        //            HttpContext.Current.Session["EA"] = res.getCatPath();
        //            IList<INavigateCategory> list = res.getDetailedCategories();

        //            foreach (INavigateCategory item in list)
        //            {
        //                DataRow row = Mnu_Temp.NewRow();
        //                IList<string> li = item.getIDs();
        //                row["category_id"] = li[0].ToString().Substring(2);
        //                row["category_name"] = item.getName();
        //                row["image_file"] = "NULL";
        //                row["custom_num_field3"] = "2";
        //                row["parent_category"] = pid;
        //                Mnu_Temp.Rows.Add(row);
        //            }
        //        }
        //        else
        //        {
        //            res = ea.userPageOp("AllProducts////WESAUSTRALASIA////Cellular Accessories////" + cname, curPage.ToString(), pageOp);
        //            IList<INavigateCategory> list = res.getDetailedCategories();
        //            updateBreadCrumb(res.getBreadCrumbTrail(), "", "");
        //            //if (HttpContext.Current.Request.Url.OriginalString.Contains("categorylist.aspx"))
        //            //{
        //            //    Category_updateBreadCrumb(res.getBreadCrumbTrail());
        //            //}
        //            //else
        //            //{
        //            //    product_updateBreadCrumb(res.getBreadCrumbTrail());
        //            //}
        //            foreach (INavigateCategory item in list)
        //            {
        //                DataRow row = Mnu_Temp.NewRow();
        //                IList<string> li = item.getIDs();
        //                row["category_id"] = li[0].ToString().Substring(2);
        //                row["category_name"] = item.getName();
        //                row["image_file"] = "NULL";
        //                row["custom_num_field3"] = "2";
        //                row["parent_category"] = pid;
        //                Mnu_Temp.Rows.Add(row);
        //            }
        //        }
        //        Get_FamilyDS_Values(res);

        //        //Menu_Click.Tables.Add(Mnu_Temp);
        //        //Menu_Click.Tables.Add(Mnu_Table);
        //        //Menu_Click.Tables.Add(Mnu_Table1);
        //        //Menu_Click.Tables.Add(Mnu_Table2);
        //        //Menu_Click.Tables.Add(Mnu_Table3);

        //        if (Menu_Click.Tables.Count > 0)
        //        {
        //            HttpContext.Current.Session["Click_Menu_Results"] = Menu_Click;
        //        }

        //        IList<string> Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
        //        EasyAsk_UserSearch_Attributes_DS(Attributes, res, temptext);
        //        HttpContext.Current.Session["Menu"] = Menu_Click;
        //      //  objErrorhandler._LogMsg = Environment.NewLine + "After Category List & Product List Page EasyAsk Result -" + DateTime.Now.ToLongTimeString();
        //      //  objErrorhandler.CreateLogTest();
        //        return Menu_Click;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        //void Create_Menu_Table_Columns()
        //{
        //    Menu_Click.Tables.Add(Mnu_Temp);
        //    //For Category
        //    Mnu_Temp.Columns.Add("category_id", typeof(string));
        //    Mnu_Temp.Columns.Add("category_name", typeof(string));
        //    Mnu_Temp.Columns.Add("image_file", typeof(string));
        //    Mnu_Temp.Columns.Add("custom_num_field3", typeof(string));
        //    Mnu_Temp.Columns.Add("parent_category", typeof(string));

        //    Menu_Click.Tables.Add(Mnu_Table);
        //    //For Total Pages.
        //    Mnu_Table.Columns.Add("TOTAL_PAGES", typeof(int));

        //    Menu_Click.Tables.Add(Mnu_Table1);
        //    //For Total Products.
        //    Mnu_Table1.Columns.Add("TOTAL_PRODUCTS", typeof(string));

        //    Menu_Click.Tables.Add(Mnu_Table2);
        //    //For Display Family Result.
        //    Mnu_Table2.Columns.Add("FAMILY_ID", typeof(string));
        //    Mnu_Table2.Columns.Add("FAMILY_NAME", typeof(string));
        //    Mnu_Table2.Columns.Add("ATTRIBUTE_ID", typeof(string));
        //    Mnu_Table2.Columns.Add("STRING_VALUE", typeof(string));
        //    Mnu_Table2.Columns.Add("NUMERIC_VALUE", typeof(string));
        //    Mnu_Table2.Columns.Add("OBJECT_TYPE", typeof(string));
        //    Mnu_Table2.Columns.Add("OBJECT_NAME", typeof(string));
        //    Mnu_Table2.Columns.Add("ATTRIBUTE_NAME", typeof(string));
        //    Mnu_Table2.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
        //    //Mnu_Table2.Columns.Add("Family_Prod_Count", typeof(string));
        //    //Mnu_Table2.Columns.Add("Prod_Count", typeof(string));

        //    Menu_Click.Tables.Add(Mnu_Table3);
        //    //For Family Result Related.
        //    Mnu_Table3.Columns.Add("FAMILY_ID", typeof(string));
        //    Mnu_Table3.Columns.Add("FAMILY_NAME", typeof(string));
        //    Mnu_Table3.Columns.Add("Family_Prod_Count", typeof(string));
        //    Mnu_Table3.Columns.Add("Prod_Count", typeof(string));
        //}

        //DataSet Get_FamilyDS_Values(INavigateResults res)
        //{
        //    DataRow dr = Mnu_Table.NewRow();
        //    dr[0] = res.getPageCount();
        //    Mnu_Table.Rows.Add(dr);

        //    DataRow dr1 = Mnu_Table1.NewRow();
        //    dr1[0] = res.getTotalItems();
        //    Mnu_Table1.Rows.Add(dr1);

        //    int last = res.getLastItem();
        //    int colFmlyID = res.getColumnIndex("Family Id");
        //    int colFmlyName = res.getColumnIndex("Family Name");
        //    int colFmlyDesc = res.getColumnIndex("Family Description");
        //    int colShortFmlyDesc = res.getColumnIndex("Family ShortDescription");
        //    int colFmlyImg = res.getColumnIndex("Family Thumbnail");
        //    int colFamilyCount = res.getColumnIndex("Family Prod Count");
        //    int colProductCode = res.getColumnIndex("Prod Code");
        //    int colProductImg = res.getColumnIndex("Prod Thumbnail");
        //    int colProductCount = res.getColumnIndex("Prod Count");

        //    DataRow dRow;
        //    try
        //    {
        //        if (last >= 0)
        //        {
        //            for (int i = res.getFirstItem() - 1, col = 0; i < last; i++, col++)
        //            {
        //                DataRow row1 = Mnu_Table3.NewRow();
        //                row1["FAMILY_ID"] = res.getCellData(i, colFmlyID);
        //                row1["FAMILY_NAME"] = res.getCellData(i, colFmlyName);
        //                row1["Family_Prod_Count"] = res.getCellData(i, colFamilyCount);
        //                row1["Prod_Count"] = res.getCellData(i, colProductCount);
        //               // HttpContext.Current.Session["FamilyCount"] = res.getCellData(i, colFamilyCount);
        //                Mnu_Table3.Rows.Add(row1);

        //                for (int k = 0; k < 5; k++)
        //                {
        //                    dRow = Mnu_Table2.NewRow();
        //                    dRow["FAMILY_ID"] = res.getCellData(i, colFmlyID);
        //                    dRow["FAMILY_NAME"] = res.getCellData(i, colFmlyName);

        //                    string temp_family_count = res.getCellData(i, colFamilyCount);
        //                    string temp_product_count = res.getCellData(i, colProductCount);
        //                    string temp_fmly_Image = res.getCellData(i, colFmlyImg).ToString();
        //                    string temp_product_Image = res.getCellData(i, colProductImg).ToString();
        //                    string image_string = "";

        //                    if (temp_fmly_Image != "" && temp_product_Image != "")
        //                    {
        //                        if (temp_product_count.ToString() == "1")
        //                        {
        //                            image_string = temp_product_Image.Substring(42);
        //                        }
        //                        else
        //                        {
        //                            image_string = temp_fmly_Image.Substring(42);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        image_string = "noimage.gif";
        //                    }


        //                    if (k == 0)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "4";
        //                        dRow["STRING_VALUE"] = res.getCellData(i, colFmlyDesc);
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "Descriptions";
        //                        dRow["ATTRIBUTE_TYPE"] = "7";
        //                    }
        //                    if (k == 1)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "8";
        //                        if (res.getCellData(i, colFmlyImg) != "")
        //                        {
        //                            dRow["STRING_VALUE"] = image_string;// res.getCellData(i, colFmlyImg).Substring(42).ToString();
        //                        }
        //                        else
        //                        {
        //                            dRow["STRING_VALUE"] = @"\NoImage.gif";
        //                        }
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "jpg";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "Family Image1";
        //                        dRow["ATTRIBUTE_TYPE"] = "9";
        //                    }
        //                    if (k == 2)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "329";
        //                        dRow["STRING_VALUE"] = res.getCellData(i, colFmlyDesc);
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "Descriptions1";
        //                        dRow["ATTRIBUTE_TYPE"] = "7";
        //                    }
        //                    if (k == 3)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "13";
        //                        dRow["STRING_VALUE"] = res.getCellData(i, colShortFmlyDesc);
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "Short Description";
        //                        dRow["ATTRIBUTE_TYPE"] = "7";
        //                    }
        //                    if (k == 4)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "1";
        //                        dRow["STRING_VALUE"] = res.getCellData(i, colProductCode);
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "Code";
        //                        dRow["ATTRIBUTE_TYPE"] = "1";
        //                    }
        //                    Mnu_Table2.Rows.Add(dRow);
        //                }
        //                j++;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return Menu_Click;
        //}

        //#endregion

        //#region "For the Family Page Products"

        //DataSet ProductSearch = new DataSet();
        //DataTable Producttable = new DataTable();
        //DataTable Familytable = new DataTable();

        //DataSet Sub_Family_DS = new DataSet();
        //DataTable Sub_Family_Products = new DataTable();
        //DataTable Sub_Family_Details = new DataTable();

        //public DataSet EasyAsk_Family_Products(string srctxt, string familyid, string temptext, string familyshow)
        //{
        //    try
        //    {
        //        IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WebCatDictionary);
        //        IOptions opts = ea.getOptions();
        //        opts.setResultsPerPage(m_rpp); // ea_rpp.Value);   // use current settings
        //        opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
        //        opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);
        //        opts.setSubCategories(false);
        //        opts.setNavigateHierarchy(false);
        //        opts.setReturnSKUs(true);
        //        INavigateResults res = null;
        //        DataSet ds = new DataSet();
        //        DataSet subfamily_ds = new DataSet();
        //        Create_Product_Table_Columns();
        //        Create_SubFamily_Table_Columns();
        //        //ea.setReturnSKUS(true);
        //        if (familyshow == "1")
        //        {
        //            opts.setResultsPerPage("0");
        //            //ea.setResultsPerPage("0");
        //            INavigateResults res1 = ea.userSearch("AllProducts////WESAUSTRALASIA////Cellular Accessories", "Family Id=" + familyid);
        //            ds = GetProductDetails(res1);
        //            subfamily_ds = GetSubfamilyDetails(res1);

        //            //For Leftnavigation New UI
        //            IList<string> Attributes = res1.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
        //            EasyAsk_UserSearch_Attributes_DS(Attributes, res1, temptext);

        //            //Store Family details in session
        //            HttpContext.Current.Session["Family"] = ds;
        //            HttpContext.Current.Session["Sub_Family"] = subfamily_ds;
        //            HttpContext.Current.Session["EA"] = res1.getCatPath();
        //            updateBreadCrumb(res1.getBreadCrumbTrail(), familyshow, familyid);
        //        }
        //        else
        //        {
        //            opts.setResultsPerPage("0");
        //         //   ea.setResultsPerPage("0");
        //            res = ea.userSearch(HttpContext.Current.Session["EA"].ToString(), "Family Id=" + familyid);
        //            ds = GetProductDetails(res);
        //            subfamily_ds = GetSubfamilyDetails(res);

        //            //For Leftnavigation New UI
        //            IList<string> Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
        //            EasyAsk_UserSearch_Attributes_DS(Attributes, res, temptext);

        //            //Store Family details in session
        //            HttpContext.Current.Session["Family"] = ds;
        //            HttpContext.Current.Session["Sub_Family"] = subfamily_ds;
        //            HttpContext.Current.Session["EA"] = res.getCatPath();
        //            //  string SEO_PATH = "";
        //            updateBreadCrumb(res.getBreadCrumbTrail(), familyshow, familyid);
        //        }
        //      //  objErrorhandler._LogMsg = Environment.NewLine + "After EasyAsk API Call For Selecting Family Details -" + DateTime.Now.ToLongTimeString();
        //      //  objErrorhandler.CreateLogTest();
        //        return ds;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        //private void Create_Product_Table_Columns()
        //{
        //    ProductSearch.Tables.Add(Familytable);

        //    Familytable.Columns.Add("FAMILY_ID", typeof(string));
        //    Familytable.Columns.Add("FAMILY_NAME", typeof(string));
        //    Familytable.Columns.Add("STRING_VALUE", typeof(string));
        //    Familytable.Columns.Add("NUMERIC_VALUE", typeof(string));
        //    Familytable.Columns.Add("ATTRIBUTE_ID", typeof(string));
        //    Familytable.Columns.Add("ATTRIBUTE_NAME", typeof(string));
        //    Familytable.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
        //    Familytable.Columns.Add("ATTRIBUTE_DATA_TYPE", typeof(string));
        //    Familytable.Columns.Add("Family_Prod_Count", typeof(string));
        //    Familytable.Columns.Add("Prod_Count", typeof(string));
        //    Familytable.Columns.Add("STATUS", typeof(string));

        //    ProductSearch.Tables.Add(Producttable);//family products

        //    Producttable.Columns.Add("PRODUCT_ID", typeof(string));
        //    Producttable.Columns.Add("FAMILY_ID", typeof(string));
        //    Producttable.Columns.Add("STRING_VALUE", typeof(string));
        //    Producttable.Columns.Add("NUMERIC_VALUE", typeof(string));
        //    Producttable.Columns.Add("ATTRIBUTE_NAME", typeof(string));
        //    Producttable.Columns.Add("ATTRIBUTE_ID", typeof(string));
        //    Producttable.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
        //    Producttable.Columns.Add("ATTRIBUTE_DATATYPE", typeof(string));
        //}

        //private void Create_SubFamily_Table_Columns()
        //{
        //    Sub_Family_DS.Tables.Add(Sub_Family_Products);//subfamily products

        //    Sub_Family_Products.Columns.Add("PRODUCT_ID", typeof(string));
        //    Sub_Family_Products.Columns.Add("FAMILY_ID", typeof(string));
        //    Sub_Family_Products.Columns.Add("SUB_FAMILY_ID", typeof(string));
        //    Sub_Family_Products.Columns.Add("STRING_VALUE", typeof(string));
        //    Sub_Family_Products.Columns.Add("NUMERIC_VALUE", typeof(string));
        //    Sub_Family_Products.Columns.Add("ATTRIBUTE_NAME", typeof(string));
        //    Sub_Family_Products.Columns.Add("ATTRIBUTE_ID", typeof(string));
        //    Sub_Family_Products.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
        //    Sub_Family_Products.Columns.Add("ATTRIBUTE_DATATYPE", typeof(string));

        //    Sub_Family_DS.Tables.Add(Sub_Family_Details);//subfamily products

        //    Sub_Family_Details.Columns.Add("FAMILY_ID", typeof(string));
        //    Sub_Family_Details.Columns.Add("FAMILY_NAME", typeof(string));
        //    Sub_Family_Details.Columns.Add("STRING_VALUE", typeof(string));
        //    Sub_Family_Details.Columns.Add("NUMERIC_VALUE", typeof(string));
        //    Sub_Family_Details.Columns.Add("ATTRIBUTE_ID", typeof(string));
        //    Sub_Family_Details.Columns.Add("ATTRIBUTE_NAME", typeof(string));
        //    Sub_Family_Details.Columns.Add("ATTRIBUTE_DATATYPE", typeof(string));
        //    Sub_Family_Details.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
        //}

        //private DataSet GetProductDetails(INavigateResults res)
        //{
        //    int last = res.getLastItem();
        //    int first = res.getFirstItem();
        //    int colFmlyID = res.getColumnIndex("Family Id");
        //    int colFmlyName = res.getColumnIndex("Family Name");
        //    int colFmlyDesc = res.getColumnIndex("Family Description");//ShortDescription
        //    int colFmlyImg = res.getColumnIndex("Family Thumbnail");

        //    int subFmlyID = res.getColumnIndex("SubFamily Id");
        //    int subFmlyName = res.getColumnIndex("SubFamily Name");
        //    int subFmlyDesc = res.getColumnIndex("SubFamily Description");

        //    int colProductID = res.getColumnIndex("Prod Id");
        //    int colProductCode = res.getColumnIndex("Prod Code");
        //    int colProductPrice = res.getColumnIndex("Price");
        //    int colProductDesc = res.getColumnIndex("Prod Description");
        //    int colProductImg = res.getColumnIndex("Prod Thumbnail");

        //    int colProductCount = res.getColumnIndex("Prod Count");
        //    int colFamilyProdCount = res.getColumnIndex("Family Prod Count");

        //    DataRow dr, dRow;
        //    try
        //    {
        //        if (last >= 0)
        //        {
        //            string F_Id = res.getCellData(0, colFmlyID);
        //            string F_Name = res.getCellData(0, colFmlyName);
        //            string F_Count = res.getCellData(0, colFamilyProdCount);
        //            string P_Count = last.ToString();
        //            string Status = "false";
        //            if (P_Count == "1")
        //            {
        //                if (F_Count != P_Count)
        //                {
        //                    Status = "One Product";
        //                }
        //                else
        //                {
        //                    Status = "true";
        //                }
        //            }
        //            else if (F_Count == P_Count)
        //            {
        //                Status = "true";
        //            }
        //            else
        //            {
        //                Status = "false";
        //            }

        //            //For Family Details
        //            for (int i = 0; i < 7; i++)
        //            {
        //                dr = Familytable.NewRow();
        //                if (i == 0)
        //                {
        //                    dr["FAMILY_ID"] = F_Id;
        //                    dr["FAMILY_NAME"] = F_Name;
        //                    dr["STRING_VALUE"] = "";
        //                    dr["NUMERIC_VALUE"] = "";
        //                    dr["ATTRIBUTE_ID"] = "";
        //                    dr["ATTRIBUTE_NAME"] = "Code";
        //                    dr["ATTRIBUTE_TYPE"] = "";
        //                    dr["ATTRIBUTE_DATA_TYPE"] = "TEXT";
        //                    dr["Family_Prod_Count"] = F_Count;
        //                    dr["Prod_Count"] = P_Count.ToString();
        //                    dr["STATUS"] = Status;
        //                }
        //                if (i == 1)
        //                {
        //                    dr["FAMILY_ID"] = F_Id;
        //                    dr["FAMILY_NAME"] = F_Name;
        //                    dr["STRING_VALUE"] = res.getCellData(0, colFmlyDesc);
        //                    dr["NUMERIC_VALUE"] = "";
        //                    dr["ATTRIBUTE_ID"] = "329";
        //                    dr["ATTRIBUTE_NAME"] = "Descriptions1";
        //                    dr["ATTRIBUTE_TYPE"] = "7";
        //                    dr["ATTRIBUTE_DATA_TYPE"] = "TEXT";
        //                    dr["Family_Prod_Count"] = F_Count;
        //                    dr["Prod_Count"] = P_Count.ToString();
        //                    dr["STATUS"] = Status;
        //                }
        //                if (i == 2)
        //                {
        //                    dr["FAMILY_ID"] = F_Id;
        //                    dr["FAMILY_NAME"] = F_Name;
        //                    dr["STRING_VALUE"] = res.getCellData(0, colFmlyDesc);
        //                    dr["NUMERIC_VALUE"] = "";
        //                    dr["ATTRIBUTE_ID"] = "4";
        //                    dr["ATTRIBUTE_NAME"] = "Descriptions";
        //                    dr["ATTRIBUTE_TYPE"] = "7";
        //                    dr["ATTRIBUTE_DATA_TYPE"] = "TEXT";
        //                    dr["Family_Prod_Count"] = F_Count;
        //                    dr["Prod_Count"] = P_Count.ToString();
        //                    dr["STATUS"] = Status;
        //                }
        //                if (i == 3)
        //                {
        //                    dr["FAMILY_ID"] = F_Id;
        //                    dr["FAMILY_NAME"] = F_Name;
        //                    if (res.getCellData(0, colFmlyImg) != "")
        //                    {
        //                        dr["STRING_VALUE"] = res.getCellData(0, colFmlyImg).Substring(42).ToString();
        //                    }
        //                    else
        //                    {
        //                        dr["STRING_VALUE"] = @"\NoImage.gif";
        //                    }
        //                    dr["NUMERIC_VALUE"] = "";
        //                    dr["ATTRIBUTE_ID"] = "8";
        //                    dr["ATTRIBUTE_NAME"] = "Family Image1";
        //                    dr["ATTRIBUTE_TYPE"] = "9";
        //                    dr["ATTRIBUTE_DATA_TYPE"] = "TEXT";
        //                    dr["Family_Prod_Count"] = F_Count;
        //                    dr["Prod_Count"] = P_Count.ToString();
        //                    dr["STATUS"] = Status;
        //                }
        //                if (i == 4)
        //                {
        //                    dr["FAMILY_ID"] = F_Id;
        //                    dr["FAMILY_NAME"] = F_Name;
        //                    if (res.getCellData(0, colFmlyImg) != "")
        //                    {
        //                        dr["STRING_VALUE"] = res.getCellData(0, colFmlyImg).Substring(42).ToString();
        //                    }
        //                    else
        //                    {
        //                        dr["STRING_VALUE"] = @"\NoImage.gif";
        //                    }
        //                    dr["NUMERIC_VALUE"] = "";
        //                    dr["ATTRIBUTE_ID"] = "746";
        //                    dr["ATTRIBUTE_NAME"] = "FWeb Image1";
        //                    dr["ATTRIBUTE_TYPE"] = "9";
        //                    dr["ATTRIBUTE_DATA_TYPE"] = "TEXT";
        //                    dr["Family_Prod_Count"] = F_Count;
        //                    dr["Prod_Count"] = P_Count.ToString();
        //                    dr["STATUS"] = Status;
        //                }
        //                if (i == 5)
        //                {
        //                    dr["FAMILY_ID"] = F_Id;
        //                    dr["FAMILY_NAME"] = F_Name;
        //                    if (res.getCellData(0, colFmlyImg) != "")
        //                    {
        //                        dr["STRING_VALUE"] = res.getCellData(0, colFmlyImg).Substring(42).ToString();
        //                        //    if (res.getCellData(0, colProductImg) != "")
        //                        //    {
        //                        //        dr["STRING_VALUE"] = res.getCellData(0, colProductImg).Substring(42).ToString();
        //                        //    }
        //                        //    else
        //                        //    {
        //                        //        dr["STRING_VALUE"] = @"\NoImage.gif";
        //                        //    }
        //                    }
        //                    else
        //                    {
        //                        dr["STRING_VALUE"] = @"\NoImage.gif";
        //                    }
        //                    dr["NUMERIC_VALUE"] = "";
        //                    dr["ATTRIBUTE_ID"] = "747";
        //                    dr["ATTRIBUTE_NAME"] = "TFWeb Image1";
        //                    dr["ATTRIBUTE_TYPE"] = "9";
        //                    dr["ATTRIBUTE_DATA_TYPE"] = "TEXT";
        //                    dr["Family_Prod_Count"] = F_Count;
        //                    dr["Prod_Count"] = P_Count.ToString();
        //                    dr["STATUS"] = Status;
        //                }
        //                if (i == 6)
        //                {
        //                    dr["FAMILY_ID"] = F_Id;
        //                    dr["FAMILY_NAME"] = F_Name;
        //                    dr["ATTRIBUTE_ID"] = "452";
        //                    if (res.getCellData(0, colProductImg) != "")
        //                    {
        //                        dr["STRING_VALUE"] = res.getCellData(0, colProductImg).Substring(42).ToString();
        //                    }
        //                    else
        //                    {
        //                        dr["STRING_VALUE"] = @"\NoImage.gif";
        //                    }
        //                    dr["NUMERIC_VALUE"] = "";
        //                    dr["ATTRIBUTE_NAME"] = "TWeb Image1";
        //                    dr["ATTRIBUTE_TYPE"] = "3";
        //                    dr["ATTRIBUTE_DATA_TYPE"] = "TEXT";
        //                    dr["Family_Prod_Count"] = F_Count;
        //                    dr["Prod_Count"] = P_Count.ToString();
        //                    dr["STATUS"] = Status;
        //                }
        //                Familytable.Rows.Add(dr);
        //            }
        //         //   HttpContext.Current.Session["FamilyCount"] = F_Count;
        //            //For family page products
        //            for (int i = res.getFirstItem() - 1, col = 0; i < last; i++, col++) //For Product details
        //            {
        //                if (res.getCellData(i, subFmlyID) == "" && res.getCellData(i, subFmlyName) == "")
        //                {
        //                    for (int k = 0; k < 6; k++)
        //                    {
        //                        dRow = Producttable.NewRow();
        //                        dRow["PRODUCT_ID"] = res.getCellData(i, colProductID);
        //                        dRow["FAMILY_ID"] = res.getCellData(i, colFmlyID);
        //                        if (k == 0)
        //                        {
        //                            dRow["ATTRIBUTE_ID"] = "1";
        //                            dRow["STRING_VALUE"] = res.getCellData(i, colProductCode);//For the Product Code
        //                            dRow["NUMERIC_VALUE"] = "0";
        //                            dRow["ATTRIBUTE_NAME"] = "Code";
        //                            dRow["ATTRIBUTE_TYPE"] = "1";
        //                            dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
        //                        }
        //                        if (k == 1)
        //                        {
        //                            dRow["ATTRIBUTE_ID"] = "5";
        //                            dRow["STRING_VALUE"] = "";
        //                            if (res.getCellData(i, colProductPrice) == "")
        //                            {
        //                                dRow["NUMERIC_VALUE"] = "0";
        //                            }
        //                            else
        //                            {
        //                                dRow["NUMERIC_VALUE"] = res.getCellData(i, colProductPrice).Substring(1);//For Cost
        //                            }
        //                            dRow["ATTRIBUTE_NAME"] = "Cost";
        //                            dRow["ATTRIBUTE_TYPE"] = "4";
        //                            dRow["ATTRIBUTE_DATATYPE"] = "NUMBER";
        //                        }
        //                        if (k == 2)
        //                        {
        //                            dRow["ATTRIBUTE_ID"] = "449";
        //                            dRow["STRING_VALUE"] = res.getCellData(i, colProductDesc);
        //                            dRow["NUMERIC_VALUE"] = "0";
        //                            dRow["ATTRIBUTE_NAME"] = "PROD_DSC";
        //                            dRow["ATTRIBUTE_TYPE"] = "1";
        //                            dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
        //                        }
        //                        if (k == 3)
        //                        {
        //                            dRow["ATTRIBUTE_ID"] = "449";
        //                            dRow["STRING_VALUE"] = res.getCellData(i, colProductDesc);
        //                            dRow["NUMERIC_VALUE"] = "0";
        //                            dRow["ATTRIBUTE_NAME"] = "DESCRIPTION";
        //                            dRow["ATTRIBUTE_TYPE"] = "1";
        //                            dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
        //                        }
        //                        if (k == 4)
        //                        {
        //                            dRow["ATTRIBUTE_ID"] = "7";
        //                            if (res.getCellData(0, colProductImg) != "")
        //                            {
        //                                dRow["STRING_VALUE"] = res.getCellData(0, colProductImg).Substring(42).ToString();
        //                            }
        //                            else
        //                            {
        //                                dRow["STRING_VALUE"] = @"\NoImage.gif";
        //                            }
        //                            dRow["NUMERIC_VALUE"] = "";
        //                            dRow["ATTRIBUTE_NAME"] = "Product Image1";
        //                            dRow["ATTRIBUTE_TYPE"] = "3";
        //                            dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
        //                        }
        //                        if (k == 5)
        //                        {
        //                            dRow["ATTRIBUTE_ID"] = "452";
        //                            if (res.getCellData(0, colProductImg) != "")
        //                            {
        //                                dRow["STRING_VALUE"] = res.getCellData(0, colProductImg).Substring(42).ToString();
        //                            }
        //                            else
        //                            {
        //                                dRow["STRING_VALUE"] = @"\NoImage.gif";
        //                            }
        //                            dRow["NUMERIC_VALUE"] = "";
        //                            dRow["ATTRIBUTE_NAME"] = "TWeb Image1";
        //                            dRow["ATTRIBUTE_TYPE"] = "3";
        //                            dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
        //                        }
        //                        Producttable.Rows.Add(dRow);
        //                    }
        //                }
        //                else
        //                {
        //                    //For subfamily product details
        //                    for (int k = 0; k < 5; k++)
        //                    {
        //                        dRow = Sub_Family_Products.NewRow();
        //                        dRow["PRODUCT_ID"] = res.getCellData(i, colProductID);
        //                        dRow["FAMILY_ID"] = res.getCellData(i, colFmlyID);
        //                        dRow["SUB_FAMILY_ID"] = res.getCellData(i, subFmlyID);
        //                        if (k == 0)
        //                        {
        //                            dRow["ATTRIBUTE_ID"] = "1";
        //                            dRow["STRING_VALUE"] = res.getCellData(i, colProductCode);//For the Product Code
        //                            dRow["NUMERIC_VALUE"] = "0";
        //                            dRow["ATTRIBUTE_NAME"] = "Code";
        //                            dRow["ATTRIBUTE_TYPE"] = "1";
        //                            dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
        //                        }
        //                        if (k == 1)
        //                        {
        //                            dRow["ATTRIBUTE_ID"] = "5";
        //                            dRow["STRING_VALUE"] = "";
        //                            if (res.getCellData(i, colProductPrice) == "")
        //                            {
        //                                dRow["NUMERIC_VALUE"] = "0";
        //                            }
        //                            else
        //                            {
        //                                dRow["NUMERIC_VALUE"] = res.getCellData(i, colProductPrice).Substring(1);//For Cost
        //                            }
        //                            dRow["ATTRIBUTE_NAME"] = "Cost";
        //                            dRow["ATTRIBUTE_TYPE"] = "4";
        //                            dRow["ATTRIBUTE_DATATYPE"] = "NUMBER";
        //                        }
        //                        if (k == 2)
        //                        {
        //                            dRow["ATTRIBUTE_ID"] = "449";
        //                            dRow["STRING_VALUE"] = res.getCellData(i, colProductDesc);
        //                            dRow["NUMERIC_VALUE"] = "0";
        //                            dRow["ATTRIBUTE_NAME"] = "PROD_DSC";
        //                            dRow["ATTRIBUTE_TYPE"] = "1";
        //                            dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
        //                        }
        //                        if (k == 3)
        //                        {
        //                            dRow["ATTRIBUTE_ID"] = "449";
        //                            dRow["STRING_VALUE"] = res.getCellData(i, colProductDesc) + " Testing";
        //                            dRow["NUMERIC_VALUE"] = "0";
        //                            dRow["ATTRIBUTE_NAME"] = "DESCRIPTION";
        //                            dRow["ATTRIBUTE_TYPE"] = "1";
        //                            dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
        //                        }
        //                        if (k == 4)
        //                        {
        //                            dRow["ATTRIBUTE_ID"] = "7";
        //                            if (res.getCellData(i, colProductImg) != "")
        //                            {
        //                                dRow["STRING_VALUE"] = res.getCellData(i, colProductImg).Substring(42).ToString();
        //                            }
        //                            else
        //                            {
        //                                dRow["STRING_VALUE"] = @"\NoImage.gif";
        //                            }
        //                            dRow["NUMERIC_VALUE"] = "";
        //                            dRow["ATTRIBUTE_NAME"] = "TWEB_IMAGE123";
        //                            dRow["ATTRIBUTE_TYPE"] = "3";
        //                            dRow["ATTRIBUTE_DATATYPE"] = "TEXT";
        //                        }
        //                        Sub_Family_Products.Rows.Add(dRow);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return ProductSearch;
        //}

        //private DataSet GetSubfamilyDetails(INavigateResults res)
        //{
        //    int last = res.getLastItem();
        //    int first = res.getFirstItem();
        //    int colFmlyID = res.getColumnIndex("Family Id");
        //    int colFmlyName = res.getColumnIndex("Family Name");
        //    int colFmlyDesc = res.getColumnIndex("Family Description");//ShortDescription
        //    int colFmlyImg = res.getColumnIndex("Family Thumbnail");

        //    int subFmlyID = res.getColumnIndex("SubFamily Id");
        //    int subFmlyName = res.getColumnIndex("SubFamily Name");
        //    int subFmlyDesc = res.getColumnIndex("SubFamily Description");
        //    int subFmlyImg = res.getColumnIndex("SubFamily Thumbnail");

        //    int colProductID = res.getColumnIndex("Prod Id");
        //    int colProductCode = res.getColumnIndex("Prod Code");
        //    int colProductPrice = res.getColumnIndex("Price");
        //    int colProductDesc = res.getColumnIndex("Prod Description");
        //    int colProductImg = res.getColumnIndex("Prod Thumbnail");

        //    int colProductCount = res.getColumnIndex("Prod Count");
        //    int colFamilyProdCount = res.getColumnIndex("Family Prod Count");

        //    DataRow dr;
        //    try
        //    {
        //        if (last >= 0)
        //        {
        //            for (int i = res.getFirstItem() - 1, col = 0; i < last; i++, col++) //For Product details
        //            {
        //                if (res.getCellData(i, subFmlyID) != "" && res.getCellData(i, subFmlyName) != "")
        //                {
        //                    //For Sub Family Details
        //                    for (int k = 0; k < 3; k++)
        //                    {
        //                        dr = Sub_Family_Details.NewRow();
        //                        if (k == 0)
        //                        {
        //                            dr["FAMILY_ID"] = res.getCellData(i, subFmlyID);
        //                            dr["FAMILY_NAME"] = res.getCellData(i, subFmlyName);
        //                            dr["STRING_VALUE"] = "";
        //                            dr["NUMERIC_VALUE"] = "";
        //                            dr["ATTRIBUTE_ID"] = "";
        //                            dr["ATTRIBUTE_NAME"] = "";
        //                            dr["ATTRIBUTE_DATATYPE"] = "";
        //                            dr["ATTRIBUTE_TYPE"] = "";
        //                        }
        //                        if (k == 1)
        //                        {
        //                            dr["FAMILY_ID"] = res.getCellData(i, subFmlyID);
        //                            dr["FAMILY_NAME"] = res.getCellData(i, subFmlyName);
        //                            dr["STRING_VALUE"] = res.getCellData(i, subFmlyDesc);
        //                            //dr["STRING_VALUE"] = res.getCellData(i, subFmlyDesc) + "" + "Testing";
        //                            dr["NUMERIC_VALUE"] = "";
        //                            dr["ATTRIBUTE_ID"] = "329";
        //                            dr["ATTRIBUTE_NAME"] = "Descriptions1";
        //                            dr["ATTRIBUTE_DATATYPE"] = "TEXT";
        //                            dr["ATTRIBUTE_TYPE"] = "7";
        //                        }
        //                        if (k == 2)
        //                        {
        //                            dr["FAMILY_ID"] = res.getCellData(i, subFmlyID);
        //                            dr["FAMILY_NAME"] = res.getCellData(i, subFmlyName);
        //                            if (res.getCellData(i, subFmlyImg) != "")
        //                            {
        //                                dr["STRING_VALUE"] = res.getCellData(i, subFmlyImg).Substring(42).ToString();
        //                            }
        //                            else
        //                            {
        //                                dr["STRING_VALUE"] = @"\NoImage.gif";
        //                            }
        //                            dr["NUMERIC_VALUE"] = "";
        //                            dr["ATTRIBUTE_ID"] = "747";
        //                            dr["ATTRIBUTE_NAME"] = "TFWeb Image1";
        //                            dr["ATTRIBUTE_DATATYPE"] = "TEXT";
        //                            dr["ATTRIBUTE_TYPE"] = "9";
        //                        }
        //                        Sub_Family_Details.Rows.Add(dr);
        //                    }

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    if (Sub_Family_DS.Tables[0].Rows.Count > 0)
        //    {
        //        return Sub_Family_DS;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //#endregion

        //#region "For the Attributes"

        //public DataSet Get_UserSearch_Attr(string srctxt, string attribute_Type, string attribute_value, int curPage, string pageOp, string rpp)
        //{
        //    try
        //    {
        //      //   objErrorhandler._LogMsg =  Environment.NewLine + "Before EasyAsk Result -" + DateTime.Now.ToLongTimeString();
        //       //  objErrorhandler.CreateLogTest();
        //     //   IRemoteEasyAsk ea = getRemote();
        //      //  RemoteEasyAsk();
        //      //  ea.setReturnSKUS(false);
        //      //  ea.setResultsPerPage(rpp);
        //        IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WebCatDictionary);
        //        IOptions opts = ea.getOptions();
        //        opts.setResultsPerPage(m_rpp); // ea_rpp.Value);   // use current settings
        //        opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
        //        opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);
        //        opts.setSubCategories(false);
        //        opts.setNavigateHierarchy(false);
        //        opts.setReturnSKUs(false);
        //        Create_Search_Table_Columns();
        //        DataSet ds = new DataSet();
        //        INavigateResults res = null;
        //        if (curPage < 1)
        //        {
        //            if (HttpContext.Current.Request["pgno"] == null)
        //            {
        //                res = ea.userSearch("AllProducts////WESAUSTRALASIA////Cellular Accessories", srctxt);
        //                updateBreadCrumb(res.getBreadCrumbTrail(), "", "");
        //            }
        //            else
        //            {
        //                res = ea.userSearch(HttpContext.Current.Session["EA"].ToString(), "");
        //                updateBreadCrumb(res.getBreadCrumbTrail(), "", "");
        //            }
        //            HttpContext.Current.Session["EA"] = res.getCatPath();
        //            Get_DataSet_Values(res);//For Main Content Values.

        //            IList<String> attrNamesFull = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
        //            EasyAsk_UserSearch_Attributes_DS(attrNamesFull, res, attribute_value);

        //            updateCommentary(res);
        //           //  objErrorhandler._LogMsg = Environment.NewLine + "After EasyAsk Result -"+ DateTime.Now.ToLongTimeString();
        //             //  objErrorhandler.CreateLogTest();
        //        }
        //        else
        //        {
        //            res = ea.userPageOp(HttpContext.Current.Session["EA"].ToString(), curPage.ToString(), pageOp);
        //            ds = Get_DataSet_Values(res);
        //            updateBreadCrumb(res.getBreadCrumbTrail(), "", "");
        //            HttpContext.Current.Session["EA"] = res.getCatPath();
        //            //   HttpContext.Current.Response.Write("<script>   function addZero(i) { if (i < 10) { i = '0' + i;} return i;}  var date = new Date(); var h = addZero(date.getHours()); var m = addZero(date.getMinutes()); var s = addZero(date.getSeconds()); alert('After EasyAsk Result: '+h + ':' + m + ':' + s);</script>"); 
        //            //Get_DataSet_Values(res1);
        //            //IList Attributes = res1.getAttributeNames();
        //            //EasyAsk_UserSearch_Attributes_DS(Attributes, res1, attribute_value);
        //        }
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorhandler.ErrorMsg = ex;
        //        objErrorhandler.CreateLog();
        //        return null;
        //    }
        //}

        ////public DataSet Get_Brand_Product(string srctxt, string attribute_Type, string attribute_value, int curPage, string pageOp, bool SKUS)
        ////{
        ////    try
        ////    {
        ////       // IRemoteEasyAsk ea = getRemote();
        ////       // RemoteEasyAsk();
        ////       // ea.setReturnSKUS(SKUS);
        ////        IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WebCatDictionary);
        ////        IOptions opts = ea.getOptions();
        ////        opts.setResultsPerPage(m_rpp); // ea_rpp.Value);   // use current settings
        ////        opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
        ////        opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);
        ////        opts.setSubCategories(false);
        ////        opts.setNavigateHierarchy(false);
        ////        opts.setReturnSKUs(SKUS);
        ////        DataSet ds = new DataSet();
        ////        DataSet Family = new DataSet();
        ////        INavigateResults res = null;
        ////        Create_Search_Table_Columns();
        ////        Create_Product_Table_Columns();
        ////        Create_SubFamily_Table_Columns();
        ////        if (curPage < 1)
        ////        {
        ////            string temp = "";
        ////            string temp1 = "";
        ////            if (attribute_Type.Contains("Category"))
        ////            {
        ////                //res = ea.userBreadCrumbClick("AllProducts////WESAUSTRALASIA////Cellular Accessories////UserSearch1=" + srctxt + "////" + attribute_value.Replace("-", "&") + "");
        ////                if (HttpContext.Current.Session["EA"] != null)
        ////                {
        ////                    string s = HttpUtility.UrlEncode(HttpContext.Current.Session["EA"].ToString());
        ////                    res = ea.userBreadCrumbClick1(s + "////" + attribute_value + "");
        ////                    HttpContext.Current.Session["EA"] = res.getCatPath();
        ////                    updateBreadCrumb(res.getBreadCrumbTrail(), "", "");
        ////                }
        ////            }
        ////            else
        ////            {
        ////                if (attribute_Type == "Model")
        ////                {
        ////                    string brand = HttpContext.Current.Session["Brand"].ToString();
        ////                    temp = "" + attribute_Type + " = '" + brand + ":" + attribute_value + "'";//For Apple:Ipad2...
        ////                    temp1 = "" + attribute_Type + " = ";
        ////                }
        ////                else
        ////                {
        ////                    temp = "" + attribute_Type + " = '" + attribute_value + "'";//For default value....
        ////                    temp1 = "" + attribute_Type + " = ";
        ////                }
        ////                if (HttpContext.Current.Session["EA"] != null)
        ////                {
        ////                    if (HttpContext.Current.Session["EA"].ToString().Contains(temp1))
        ////                    {
        ////                        int t = HttpContext.Current.Session["EA"].ToString().IndexOf(temp1) - 17;
        ////                        string t1 = HttpContext.Current.Session["EA"].ToString().Remove(t);
        ////                        HttpContext.Current.Session["EA"] = t1;

        ////                    }
        ////                    //res = ea.attrClick("AllProducts////WESAUSTRALASIA////Cellular Accessories////UserSearch1=" + srctxt + "", temp);
        ////                    res = ea.userAttributeClick(HttpContext.Current.Session["EA"].ToString(), temp);
        ////                    // res = ea.attrClick("AllProducts////WESAUSTRALASIA////Cellular Accessories////UserSearch="+srctxt+"////UserSearch1=" + HttpContext.Current.Request.QueryString["fid"] + "", temp);
        ////                    HttpContext.Current.Session["EA"] = res.getCatPath();
        ////                    updateBreadCrumb(res.getBreadCrumbTrail(), "", "");
        ////                }
        ////            }
        ////            ds = Get_DataSet_Values(res);
        ////            Family = GetProductDetails(res);
        ////            DataSet subfamily_ds = GetSubfamilyDetails(res);
        ////            IList<string> Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
        ////            EasyAsk_UserSearch_Attributes_DS(Attributes, res, attribute_value);
        ////            HttpContext.Current.Session["Sub_Family"] = subfamily_ds;

        ////        }
        ////        else
        ////        {
        ////            //AllProducts////WESAUSTRALASIA////Cellular Accessories////UserSearch=Audio////AttribSelect=Brand = 'Motorola'
        ////            //INavigateResults res1 = ea.userPageOp("AllProducts////WESAUSTRALASIA////Cellular Accessories////UserSearch1=" + srctxt + "////AttribSelect=" + attribute_value + "", curPage.ToString(), pageOp);
        ////            if (HttpContext.Current.Session["EA"] != null)
        ////            {
        ////                INavigateResults res1 = ea.userPageOp(HttpContext.Current.Session["EA"].ToString(), curPage.ToString(), pageOp);
        ////                ds = Get_DataSet_Values(res1);
        ////                //  updateBreadCrumb(res1.getBreadCrumbTrail());
        ////            }
        ////            //IList Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
        ////            //EasyAsk_UserSearch_Attributes_DS(Attributes, res, attribute_value);
        ////        }
        ////        HttpContext.Current.Session["Family"] = Family;
        ////        return ds;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        return null;
        ////    }
        ////}

        ////public DataSet Get_Category_UserSearch(string srctxt, string Category, int curPage, string pageOp)
        ////{
        ////    try
        ////    {
        ////        RemoteEasyAsk();

        ////        DataSet ds = new DataSet();
        ////        Create_Search_Table_Columns();

        ////        if (curPage < 1)
        ////        {
        ////            INavigateResults res = ea.userBreadCrumbClick("AllProducts////WESAUSTRALASIA////Cellular Accessories////UserSearch1=" + srctxt + "////" + Category);
        ////            ds = Get_DataSet_Values(res);
        ////        }
        ////        else
        ////        {
        ////            INavigateResults res1 = ea.userPageOp("AllProducts////WESAUSTRALASIA////Cellular Accessories////UserSearch1=" + srctxt + "////" + Category, curPage.ToString(), pageOp);
        ////            ds = Get_DataSet_Values(res1);
        ////        }
        ////        return ds;
        ////    }
        ////    catch (Exception)
        ////    {
        ////        return null;
        ////    }
        ////}
        ////created by boopathi&sengottuvel.


        //DataSet EA_UserSearch_Attributes_DS = new DataSet();
        //DataSet EA_UserSearch_Attributes_DS_Full = new DataSet();
        //void EasyAsk_UserSearch_Attributes_DS(IList<string> Attributes, INavigateResults res, string temptext)
        //{
        //    int s = 0;
        //    try
        //    {
        //        if (s == 0)
        //        {
        //            EA_UserSearch_Attributes_DS_Full.Tables.Add("Category");
        //            EA_UserSearch_Attributes_DS_Full.Tables["Category"].Columns.Add("CATEGORY_ID", typeof(string));
        //            EA_UserSearch_Attributes_DS_Full.Tables["Category"].Columns.Add("Category_Name", typeof(string));
        //            EA_UserSearch_Attributes_DS_Full.Tables["Category"].Columns.Add("Product_Count", typeof(int));
        //            EA_UserSearch_Attributes_DS.Tables.Add("Category");
        //            EA_UserSearch_Attributes_DS.Tables["Category"].Columns.Add("CATEGORY_ID", typeof(string));
        //            EA_UserSearch_Attributes_DS.Tables["Category"].Columns.Add("Category_Name", typeof(string));
        //            EA_UserSearch_Attributes_DS.Tables["Category"].Columns.Add("Product_Count", typeof(int));
        //            s++;
        //        }
        //        IList<INavigateCategory> category = null;

        //        if (res.getDetailedCategories(EasyAskConstants.ATTR_DISPLAY_MODE_INITIAL) != null)
        //        {
        //            category = res.getDetailedCategories();
        //        }
        //        else
        //        {
        //            category = res.getDetailedCategories();
        //        }
        //        if (category.Count > 0) //For Searching Category Values
        //        {
        //            //EA_UserSearch_Attributes_DS.Tables.Add("Category");
        //            //EA_UserSearch_Attributes_DS.Tables["Category"].Columns.Add("CATEGORY_ID", typeof(string));
        //            //EA_UserSearch_Attributes_DS.Tables["Category"].Columns.Add("Category_Name", typeof(string));
        //            //EA_UserSearch_Attributes_DS.Tables["Category"].Columns.Add("Product_Count", typeof(int));
        //            foreach (INavigateCategory categoryItem in category)
        //            {
        //                DataRow row = EA_UserSearch_Attributes_DS.Tables["Category"].NewRow();
        //                IList<string> Id = categoryItem.getIDs();
        //                row["CATEGORY_ID"] = Id[0].ToString().Substring(2);
        //                row["Category_Name"] = categoryItem.getName();
        //                row["Product_Count"] = categoryItem.getProductCount();
        //                EA_UserSearch_Attributes_DS.Tables["Category"].Rows.Add(row);
        //            }
        //        }
        //        if (res.getDetailedCategories(EasyAskConstants.ATTR_DISPLAY_MODE_INITIAL) != null)
        //        {
        //            IList<INavigateCategory> category1 = res.getDetailedCategories();
        //            if (res.getDetailedCategories().Count > 0) //For Searching Category Values
        //            {
        //                //foreach (INavigateCategory categoryItem in category1)
        //                //{
        //                //    DataRow row = EA_UserSearch_Attributes_DS_Full.Tables["Category"].NewRow();
        //                //    IList<string> Id = categoryItem.getIDs();
        //                //    row["CATEGORY_ID"] = Id[0].ToString().Substring(2);
        //                //    row["Category_Name"] = categoryItem.getName();
        //                //    row["Product_Count"] = categoryItem.getProductCount();
        //                //    EA_UserSearch_Attributes_DS_Full.Tables["Category"].Rows.Add(row);
        //                //}
        //            }
        //        }

        //        int temp_Count;
        //        if (EA_UserSearch_Attributes_DS.Tables.Count == 0)
        //        {
        //            temp_Count = 0;
        //        }
        //        else
        //        {
        //            temp_Count = 1;
        //        }

        //        for (int i = 0; i < Attributes.Count; i++)
        //        {
        //            String attrName = (String)Attributes[i];

        //            if (!attrName.Contains("Long Description")) //For do not display Long Description
        //            {
        //                EA_UserSearch_Attributes_DS.Tables.Add(attrName);
        //                EA_UserSearch_Attributes_DS.Tables[temp_Count].Columns.Add(attrName, typeof(string));
        //                EA_UserSearch_Attributes_DS.Tables[temp_Count].Columns.Add("Product_Count", typeof(int));

        //                IList<INavigateAttribute> AttributeValue = res.getDetailedAttributeValues(attrName, EasyAskConstants.ATTR_DISPLAY_MODE_INITIAL);
        //                foreach (INavigateAttribute AttributeItem in AttributeValue)
        //                {
        //                    DataRow row = EA_UserSearch_Attributes_DS.Tables[temp_Count].NewRow();
        //                    if (attrName.Equals("Model"))//For Model Name will split
        //                    {
        //                        if (HttpContext.Current.Session["Brand"] != null)
        //                        {
        //                            temptext = HttpContext.Current.Session["Brand"].ToString();
        //                        }
        //                        else
        //                        {
        //                            temptext = HttpUtility.UrlDecode(temptext);
        //                        }
        //                        if (AttributeItem.getValue().Contains(temptext))
        //                        {
        //                            string[] model = AttributeItem.getValue().Split(':');
        //                            if (model[0].ToString().Contains(temptext))
        //                            {
        //                                if (temptext == "" && HttpContext.Current.Session["Brand"] == null || HttpContext.Current.Session["Brand"] == string.Empty)
        //                                {
        //                                    HttpContext.Current.Session["Brand"] = model[0].ToString();
        //                                    temptext = model[0].ToString();
        //                                }
        //                                row[0] = model[1].ToString();
        //                                row[1] = AttributeItem.getProductCount();
        //                                EA_UserSearch_Attributes_DS.Tables[temp_Count].Rows.Add(row);
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        row[0] = AttributeItem.getValue();
        //                        row[1] = AttributeItem.getProductCount();
        //                        EA_UserSearch_Attributes_DS.Tables[temp_Count].Rows.Add(row);
        //                    }
        //                }
        //                temp_Count++;
        //            }

        //        }

        //        //modification for show only four models
        //        //
        //        if (EA_UserSearch_Attributes_DS.Tables.Count > 1)
        //        {
        //            for (int k = 1; k < EA_UserSearch_Attributes_DS.Tables.Count; k++)
        //            {
        //                if (EA_UserSearch_Attributes_DS.Tables[k].TableName == "Model")
        //                {
        //                    if (EA_UserSearch_Attributes_DS.Tables["Model"].Rows.Count == 0)
        //                    {
        //                        IList<INavigateAttribute> AttributeValue = res.getDetailedAttributeValues("Model", EasyAskConstants.ATTR_DISPLAY_MODE_FULL);

        //                        foreach (INavigateAttribute AttributeItem in AttributeValue)
        //                        {
        //                            if (EA_UserSearch_Attributes_DS.Tables["Model"].Rows.Count < 4)
        //                            {
        //                                DataRow row = EA_UserSearch_Attributes_DS.Tables["Model"].NewRow();
        //                                // if (attrName.Equals("Model"))//For Model Name will split
        //                                //  {
        //                                string[] model = AttributeItem.getValue().Split(':');
        //                                if (model[0].ToString().Contains(temptext))
        //                                {//problem here

        //                                    if (HttpContext.Current.Session["Brand"] == null || HttpContext.Current.Session["Brand"] == string.Empty)
        //                                    {
        //                                        HttpContext.Current.Session["Brand"] = model[0].ToString();
        //                                    }
        //                                    row[0] = model[1].ToString();
        //                                    row[1] = AttributeItem.getProductCount();
        //                                    EA_UserSearch_Attributes_DS.Tables["Model"].Rows.Add(row);
        //                                }
        //                                // }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        //
        //        //end

        //        int temp_Count1;
        //        if (EA_UserSearch_Attributes_DS_Full.Tables.Count == 0)
        //        {
        //            temp_Count1 = 0;
        //        }
        //        else
        //        {
        //            temp_Count1 = 1;
        //        }
        //        for (int j = 0; j < Attributes.Count; j++)
        //        {
        //            String attrName1 = (String)Attributes[j];

        //            if (!attrName1.Contains("Long Description")) //For do not display Long Description
        //            {
        //                EA_UserSearch_Attributes_DS_Full.Tables.Add(attrName1);
        //                EA_UserSearch_Attributes_DS_Full.Tables[temp_Count1].Columns.Add(attrName1, typeof(string));
        //                EA_UserSearch_Attributes_DS_Full.Tables[temp_Count1].Columns.Add("Product_Count", typeof(int));
        //                Boolean limited = res.isInitialDispLimitedForAttrValues(attrName1);
        //                if (limited)
        //                {
        //                    IList<INavigateAttribute> AttributeValue = res.getDetailedAttributeValues(attrName1, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
        //                    foreach (INavigateAttribute AttributeItem in AttributeValue)
        //                    {
        //                        DataRow row = EA_UserSearch_Attributes_DS_Full.Tables[temp_Count1].NewRow();
        //                        if (attrName1.Equals("Model"))//For Model Name will split
        //                        {
        //                            string[] model = AttributeItem.getValue().Split(':');
        //                            if (model[0].ToString().Contains(temptext))
        //                            {//problem here

        //                                if (HttpContext.Current.Session["Brand"] == null || HttpContext.Current.Session["Brand"] == string.Empty)
        //                                {
        //                                    HttpContext.Current.Session["Brand"] = model[0].ToString();
        //                                }
        //                                row[0] = model[1].ToString();
        //                                row[1] = AttributeItem.getProductCount();
        //                                EA_UserSearch_Attributes_DS_Full.Tables[temp_Count1].Rows.Add(row);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            row[0] = AttributeItem.getValue();
        //                            row[1] = AttributeItem.getProductCount();
        //                            EA_UserSearch_Attributes_DS_Full.Tables[temp_Count1].Rows.Add(row);
        //                        }
        //                    }
        //                }
        //                else
        //                {

        //                }
        //                temp_Count1++;
        //            }
        //        }
        //        if (EA_UserSearch_Attributes_DS.Tables.Count > 1)
        //        {
        //            for (int k = 1; k < EA_UserSearch_Attributes_DS.Tables.Count; k++)
        //            {
        //                if (EA_UserSearch_Attributes_DS.Tables[k].TableName == "Model")
        //                {
        //                    if (EA_UserSearch_Attributes_DS.Tables["Model"].Rows.Count == EA_UserSearch_Attributes_DS_Full.Tables["Model"].Rows.Count)
        //                    {
        //                            EA_UserSearch_Attributes_DS_Full.Tables["Model"].Rows.Clear();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    //return EA_UserSearch_Attributes_DS;
        //    HttpContext.Current.Session["Search_Attributes_Full"] = EA_UserSearch_Attributes_DS_Full;
        //    HttpContext.Current.Session["Search_Attributes"] = EA_UserSearch_Attributes_DS;
        //}


        ////Modified New 
        //public void BreadCrumbClick(string rpp, string attribute_value, string familyshow, string familyid, string count)
        //{
        //    try
        //    {
        //       // IRemoteEasyAsk ea = getRemote();
        //       // RemoteEasyAsk();
        //        IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WebCatDictionary);
        //        IOptions opts = ea.getOptions();
        //        opts.setResultsPerPage(m_rpp); // ea_rpp.Value);   // use current settings
        //        opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
        //        opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);
        //        opts.setSubCategories(false);
        //        opts.setNavigateHierarchy(false);
        //        opts.setReturnSKUs(false);
        //        INavigateResults res = null;
        //        DataSet ds = new DataSet();
        //        DataSet subfamily_ds = new DataSet();
        //        Create_Product_Table_Columns();
        //        Create_SubFamily_Table_Columns();
        //       // ea.setReturnSKUS(true);
        //        //if (familyshow == "1")
        //        //{
        //       // ea.setResultsPerPage("0");
        //        INavigateResults res1 = ea.userBreadCrumbClick(HttpContext.Current.Session["EA"].ToString());
        //        ds = GetProductDetails(res1);
        //        subfamily_ds = GetSubfamilyDetails(res1);

        //        //For Leftnavigation New UI
        //        IList<string> Attributes = res1.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
        //        EasyAsk_UserSearch_Attributes_DS(Attributes, res1, attribute_value);

        //        //Store Family details in session
        //        HttpContext.Current.Session["Family"] = ds;
        //        HttpContext.Current.Session["Sub_Family"] = subfamily_ds;
        //        HttpContext.Current.Session["EA"] = res1.getCatPath();
        //        updateBreadCrumb(res1.getBreadCrumbTrail(), familyshow, familyid);
        //        //}
        //        //else
        //        //{
        //        //    ea.setResultsPerPage("0");
        //        //    res = ea.userBreadCrumbClick(HttpContext.Current.Session["EA"].ToString());
        //        //    ds = GetProductDetails(res);
        //        //    subfamily_ds = GetSubfamilyDetails(res);

        //        //    //For Leftnavigation New UI
        //        //    IList Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
        //        //    EasyAsk_UserSearch_Attributes_DS(Attributes, res, attribute_value);

        //        //    //Store Family details in session
        //        //    HttpContext.Current.Session["Family"] = ds;
        //        //    HttpContext.Current.Session["Sub_Family"] = subfamily_ds;
        //        //    HttpContext.Current.Session["EA"] = res.getCatPath();
        //        //    //  string SEO_PATH = "";
        //        //    updateBreadCrumb(res.getBreadCrumbTrail(), familyshow, familyid);
        //        //}

        //    }
        //    catch (Exception ex)
        //    {
        //        // return null;
        //    }
        //}

        //public void BreadCrumbClick(string rpp, string attribute_value, int curPage, string pageOp)
        //{
        //    try
        //    {
        //        IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WebCatDictionary);
        //        IOptions opts = ea.getOptions();
        //        opts.setResultsPerPage(rpp); // ea_rpp.Value);   // use current settings
        //        opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
        //        opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);
        //        opts.setSubCategories(false);
        //        opts.setNavigateHierarchy(false);
        //        opts.setReturnSKUs(false);
        //       // ea.setResultsPerPage(rpp);
        //        Create_Search_Table_Columns();
        //        DataSet ds = new DataSet();
        //        if (curPage < 1)
        //        {
        //            INavigateResults res = ea.userBreadCrumbClick(HttpContext.Current.Session["EA"].ToString());
        //            if (HttpContext.Current.Request.Url.OriginalString.Contains("categorylist.aspx"))
        //            {
        //                Create_Menu_Table_Columns();
        //                updateBreadCrumb(res.getBreadCrumbTrail(), "", "");
        //                IList<INavigateCategory> list = res.getDetailedCategories();

        //                foreach (INavigateCategory item in list)
        //                {
        //                    DataRow row = Mnu_Temp.NewRow();
        //                    IList<string> li = item.getIDs();
        //                    row["category_id"] = li[0].ToString().Substring(2);
        //                    row["category_name"] = item.getName();
        //                    row["image_file"] = "NULL";
        //                    row["custom_num_field3"] = "2";
        //                    row["parent_category"] = "";
        //                    Mnu_Temp.Rows.Add(row);
        //                }
        //                Get_FamilyDS_Values(res);

        //                if (Menu_Click.Tables.Count > 0)
        //                {
        //                    HttpContext.Current.Session["Click_Menu_Results"] = Menu_Click;
        //                }

        //            }
        //            else if (HttpContext.Current.Request.Url.OriginalString.Contains("product_list.aspx"))
        //            {
        //                Create_Menu_Table_Columns();
        //                updateBreadCrumb(res.getBreadCrumbTrail(), "", "");
        //                IList<INavigateCategory> list = res.getDetailedCategories();

        //                foreach (INavigateCategory item in list)
        //                {
        //                    DataRow row = Mnu_Temp.NewRow();
        //                    IList<string> li = item.getIDs();
        //                    row["category_id"] = li[0].ToString().Substring(2);
        //                    row["category_name"] = item.getName();
        //                    row["image_file"] = "NULL";
        //                    row["custom_num_field3"] = "2";
        //                    row["parent_category"] = "";
        //                    Mnu_Temp.Rows.Add(row);
        //                }
        //                Get_FamilyDS_Values(res);
        //                if (Menu_Click.Tables.Count > 0)
        //                {
        //                    HttpContext.Current.Session["Click_Menu_Results"] = Menu_Click;
        //                }
        //            }
        //            else
        //            {
        //                updateBreadCrumb(res.getBreadCrumbTrail(), "", "");
        //                Get_DataSet_Values(res);//For Main Content Values.
        //            }
        //            HttpContext.Current.Session["EA"] = res.getCatPath();
        //            //Get_DataSet_Values(res);//For Main Content Values.
        //            IList<string> Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
        //            EasyAsk_UserSearch_Attributes_DS(Attributes, res, attribute_value);
        //        }
        //        else
        //        {
        //            INavigateResults res = ea.userPageOp(HttpContext.Current.Session["EA"].ToString(), curPage.ToString(), pageOp);
        //            if (HttpContext.Current.Request.Url.OriginalString.Contains("categorylist.aspx"))
        //            {
        //                Create_Menu_Table_Columns();
        //                updateBreadCrumb(res.getBreadCrumbTrail(), "", "");
        //                IList<INavigateCategory> list = res.getDetailedCategories();

        //                foreach (INavigateCategory item in list)
        //                {
        //                    DataRow row = Mnu_Temp.NewRow();
        //                    IList<string> li = item.getIDs();
        //                    row["category_id"] = li[0].ToString().Substring(2);
        //                    row["category_name"] = item.getName();
        //                    row["image_file"] = "NULL";
        //                    row["custom_num_field3"] = "2";
        //                    row["parent_category"] = "";
        //                    Mnu_Temp.Rows.Add(row);
        //                }
        //                Get_FamilyDS_Values(res);

        //                if (Menu_Click.Tables.Count > 0)
        //                {
        //                    HttpContext.Current.Session["Click_Menu_Results"] = Menu_Click;
        //                }
        //            }
        //            else if (HttpContext.Current.Request.Url.OriginalString.Contains("product_list.aspx"))
        //            {
        //                Create_Menu_Table_Columns();
        //                updateBreadCrumb(res.getBreadCrumbTrail(), "", "");
        //                IList<INavigateCategory> list = res.getDetailedCategories();

        //                foreach (INavigateCategory item in list)
        //                {
        //                    DataRow row = Mnu_Temp.NewRow();
        //                    IList<string> li = item.getIDs();
        //                    row["category_id"] = li[0].ToString().Substring(2);
        //                    row["category_name"] = item.getName();
        //                    row["image_file"] = "NULL";
        //                    row["custom_num_field3"] = "2";
        //                    row["parent_category"] = "";
        //                    Mnu_Temp.Rows.Add(row);
        //                }
        //                Get_FamilyDS_Values(res);
        //                if (Menu_Click.Tables.Count > 0)
        //                {
        //                    HttpContext.Current.Session["Click_Menu_Results"] = Menu_Click;
        //                }
        //            }
        //            else
        //            {
        //                Get_DataSet_Values(res);//For Main Content Values.
        //                updateBreadCrumb(res.getBreadCrumbTrail(), "", "");
        //            }
        //            //  return ds;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //   return null;
        //    }
        //}

        //protected void updateBreadCrumb(IBreadCrumbTrail bct, string familyshow, string familyid)
        //{
        //    try
        //    {
        //        HttpContext.Current.Session["Link"] = null;
        //        HttpContext.Current.Session["Selected"] = null;
        //        HttpContext.Current.Session["BreadCrumbName"] = null;
        //        string separator = "";
        //        List<String> Htmlitems1 = new List<String>();
        //        String htmlItems = "";
        //        String href = "";
        //        string GetValue = "";
        //        foreach (INavigateNode node in bct.getSearchPath())
        //        {
        //          // string separator1= node.getSEOPath();
        //            IList<INavigateNode> nodes = bct.getSearchPath();

        //            string separator1 = node.getPath();
        //            String label = node.getLabel();
        //            if (node.getLabel() == "Model")
        //            {
        //                string[] ModelName = node.getValue().Split(':');
        //                GetValue = ModelName[1].ToString();
        //            }
        //            else
        //            {
        //                GetValue = node.getValue();
        //            }
        //            if (node.getPath().Contains("AllProducts////WESAUSTRALASIA////Cellular Accessories////UserSearch1=") || node.getPath().Contains("AllProducts////WESAUSTRALASIA////Cellular Accessories////UserSearch="))
        //            {
        //                if (!node.getPath().Contains("Family Id="))
        //                {
        //                    separator = "&nbsp;/&nbsp;";
        //                    HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"powersearch.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">" + GetValue + "</a>";
        //                    href = "<a href=\"powersearch.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                    if (HttpContext.Current.Session["Link"] == null)
        //                    {
        //                        HttpContext.Current.Session["Link"] = "<a href=\"powersearch.aspx?&amp;Path=" + HttpUtility.UrlEncode("ppaNFtmS8Au7qIvaOCRHUp5RGlmGw65lKAOdRc+AWE7wD1EsnO+ebUWpKbZWV/Nuik1daBT3bK1yvp40MZ9Cig==") + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                    }
        //                    else
        //                    {
        //                        //    HttpContext.Current.Session["Link"] = "<a href=\"powersearch.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                        //    href = "<a href=\"powersearch.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                    }
        //                    htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV(label, GetValue) + "</a></td></tr></table></li>";
        //                    HttpContext.Current.Session["Link"] = "<a href=\"powersearch.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                    Htmlitems1.Add(htmlItems);
        //                }
        //                else
        //                {
        //                    string BreadCrumb_replace = node.getPath().Replace("Family Id=", "#");
        //                    string[] BreadCrumb_Name = BreadCrumb_replace.Split('#');
        //                    if (!BreadCrumb_Name[1].Contains("AttribSelect="))
        //                    {
        //                        DataSet d = (DataSet)HttpContext.Current.Session["Family"];
        //                        string path_value = d.Tables[0].Rows[0][1].ToString();
        //                        string Family_Id = d.Tables[0].Rows[0][0].ToString();
        //                        separator = "&nbsp;/&nbsp;";
        //                        if (HttpContext.Current.Request.QueryString["fcnt"] != null)
        //                        {
        //                            HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">" + path_value + "</a>";
        //                            href = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">";
        //                            if (HttpContext.Current.Session["Link"] == null)
        //                            {
        //                                HttpContext.Current.Session["Link"] = "<a href=\"product_list.aspx?&amp;Path=" + HttpUtility.UrlEncode("ppaNFtmS8Au7qIvaOCRHUp5RGlmGw65lKAOdRc+AWE7wD1EsnO+ebUWpKbZWV/Nuik1daBT3bK1yvp40MZ9Cig==") + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            }
        //                            htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV("Family Name", path_value) + "</a></td></tr></table></li>";
        //                            HttpContext.Current.Session["Link"] = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">";
        //                            Htmlitems1.Add(htmlItems);
        //                        }
        //                        else
        //                        {
        //                            HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">" + path_value + "</a>";
        //                            href = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">";
        //                            if (HttpContext.Current.Session["Link"] == null)
        //                            {
        //                                HttpContext.Current.Session["Link"] = "<a href=\"product_list.aspx?&amp;Path=" + HttpUtility.UrlEncode("ppaNFtmS8Au7qIvaOCRHUp5RGlmGw65lKAOdRc+AWE7wD1EsnO+ebUWpKbZWV/Nuik1daBT3bK1yvp40MZ9Cig==") + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                                // href = "<a href=\"powersearch.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            }
        //                            htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV("Family Name", path_value) + "</a></td></tr></table></li>";
        //                            HttpContext.Current.Session["Link"] = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">";
        //                            Htmlitems1.Add(htmlItems);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        DataSet d = (DataSet)HttpContext.Current.Session["Family"];
        //                        string Family_Id = d.Tables[0].Rows[0][0].ToString();
        //                        separator = "&nbsp;/&nbsp;";
        //                        if (HttpContext.Current.Request.QueryString["fcnt"] != null)
        //                        {
        //                            HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">" + GetValue + "</a>";
        //                            href = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV(label, GetValue) + "</a></td></tr></table></li>";
        //                            HttpContext.Current.Session["Link"] = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            Htmlitems1.Add(htmlItems);
        //                        }
        //                        else
        //                        {
        //                            HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">" + GetValue + "</a>";
        //                            href = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV(label, GetValue) + "</a></td></tr></table></li>";
        //                            HttpContext.Current.Session["Link"] = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            Htmlitems1.Add(htmlItems);
        //                        }
        //                    }

        //                }
        //                //HttpContext.Current.Session["BreadCrumbCount"] = i + 1;
        //            }
        //            else if (node.getPath().Contains("AllProducts////WESAUSTRALASIA////Cellular Accessories////"))
        //            {
        //                string Category_breadcrumb = node.getPath().Replace("////", "#");
        //                string[] Category_breadcrumb_name = Category_breadcrumb.Split('#');
        //                int count_breadcrumb = Category_breadcrumb_name.Count();
        //                if (count_breadcrumb == 4)
        //                {
        //                    if (HttpContext.Current.Request.Url.OriginalString.Contains("categorylist.aspx"))
        //                    {
        //                        separator = "&nbsp;/&nbsp;";
        //                        HttpContext.Current.Session["BreadCrumbName"] = separator + "<a href=\"categorylist.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">" + GetValue + "</a>";
        //                        if (HttpContext.Current.Session["Link"] == null)
        //                        {
        //                            HttpContext.Current.Session["Link"] = "<a href=\"categorylist.aspx?&amp;Path=" + HttpUtility.UrlEncode("ppaNFtmS8Au7qIvaOCRHUp5RGlmGw65lKAOdRc+AWE7wD1EsnO+ebUWpKbZWV/Nuik1daBT3bK1yvp40MZ9Cig==") + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            // href = "<a href=\"powersearch.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                        }
        //                        href = "<a href=\"categorylist.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                        htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV(label, GetValue) + "</a></td></tr></table></li>";
        //                        HttpContext.Current.Session["Link"] = "<a href=\"categorylist.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                        Htmlitems1.Add(htmlItems);
        //                    }
        //                    else
        //                    {
        //                        separator = "&nbsp;/&nbsp;";
        //                        HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"product_list.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">" + GetValue + "</a>";
        //                        if (HttpContext.Current.Session["Link"] == null)
        //                        {
        //                            HttpContext.Current.Session["Link"] = "<a href=\"product_list.aspx?&amp;Path=" + HttpUtility.UrlEncode("ppaNFtmS8Au7qIvaOCRHUp5RGlmGw65lKAOdRc+AWE7wD1EsnO+ebUWpKbZWV/Nuik1daBT3bK1yvp40MZ9Cig==") + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            // href = "<a href=\"powersearch.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                        }
        //                        href = "<a href=\"product_list.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                        htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV(label, GetValue) + "</a></td></tr></table></li>";
        //                        HttpContext.Current.Session["Link"] = "<a href=\"product_list.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">"; 
        //                        Htmlitems1.Add(htmlItems);
        //                    }
        //                }
        //                else
        //                {
        //                    if (!node.getPath().Contains("AttribSelect="))
        //                    {
        //                        if (!node.getPath().Contains("Family Id="))
        //                        {
        //                            //if (HttpContext.Current.Request.QueryString["type"] == null)
        //                            //{
        //                            separator = "&nbsp;/&nbsp;";
        //                            HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"product_list.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">" + GetValue + "</a>";
        //                            if (HttpContext.Current.Session["Link"] == null)
        //                            {
        //                                HttpContext.Current.Session["Link"] = "<a href=\"product_list.aspx?&amp;Path=" + HttpUtility.UrlEncode("ppaNFtmS8Au7qIvaOCRHUp5RGlmGw65lKAOdRc+AWE7wD1EsnO+ebUWpKbZWV/Nuik1daBT3bK1yvp40MZ9Cig==") + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                                // href = "<a href=\"powersearch.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            }
        //                            href = "<a href=\"product_list.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV(label, GetValue) + "</a></td></tr></table></li>";
        //                            HttpContext.Current.Session["Link"] = "<a href=\"product_list.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            Htmlitems1.Add(htmlItems);
        //                        }
        //                        else
        //                        {
        //                            DataSet d = (DataSet)HttpContext.Current.Session["Family"];
        //                            string path_value = d.Tables[0].Rows[0][1].ToString();
        //                            string Family_Id = d.Tables[0].Rows[0][0].ToString();
        //                            separator = "&nbsp;/&nbsp;";
        //                            if (HttpContext.Current.Request.QueryString["fcnt"] != null)
        //                            {
        //                                HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">" + path_value + "</a>";
        //                                href = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">";
        //                                htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV("Family Name", path_value) + "</a></td></tr></table></li>";
        //                                HttpContext.Current.Session["Link"] = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">";
        //                                Htmlitems1.Add(htmlItems);
        //                            }
        //                            else
        //                            {
        //                                HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">" + path_value + "</a>";
        //                                href = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">";
        //                                htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV("Family Name", path_value) + "</a></td></tr></table></li>";
        //                                HttpContext.Current.Session["Link"] = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">";
        //                                Htmlitems1.Add(htmlItems);
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (!node.getPath().Contains("Family Id="))
        //                        {
        //                            separator = "&nbsp;/&nbsp;";
        //                            HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"powersearch.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">" + GetValue + "</a>";
        //                            href = "<a href=\"powersearch.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV(label, GetValue) + "</a></td></tr></table></li>";
        //                            HttpContext.Current.Session["Link"] = "<a href=\"powersearch.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            Htmlitems1.Add(htmlItems);
        //                        }
        //                        else
        //                        {
        //                            string BreadCrumb_replace = node.getPath().Replace("Family Id=", "#");
        //                            string[] BreadCrumb_Name = BreadCrumb_replace.Split('#');
        //                            if (!BreadCrumb_Name[1].Contains("AttribSelect="))
        //                            {
        //                                DataSet d = (DataSet)HttpContext.Current.Session["Family"];
        //                                string path_value = d.Tables[0].Rows[0][1].ToString();
        //                                string Family_Id = d.Tables[0].Rows[0][0].ToString();
        //                                separator = "&nbsp;/&nbsp;";
        //                                if (HttpContext.Current.Request.QueryString["fcnt"] != null)
        //                                {
        //                                    HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">" + path_value + "</a>";
        //                                    href = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">";
        //                                    htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV("Family Name", path_value) + "</a></td></tr></table></li>";
        //                                    HttpContext.Current.Session["Link"] = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">";
        //                                    Htmlitems1.Add(htmlItems);
        //                                }
        //                                else
        //                                {
        //                                    HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">" + path_value + "</a>";
        //                                    href = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">";
        //                                    htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV("Family Name", path_value) + "</a></td></tr></table></li>";
        //                                    HttpContext.Current.Session["Link"] = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(path_value) + "\">";
        //                                    Htmlitems1.Add(htmlItems);
        //                                }
        //                            }
        //                            else
        //                            {
        //                                DataSet d = (DataSet)HttpContext.Current.Session["Family"];
        //                                string Family_Id = d.Tables[0].Rows[0][0].ToString();
        //                                separator = "&nbsp;/&nbsp;";
        //                                if (HttpContext.Current.Request.QueryString["fcnt"] != null)
        //                                {
        //                                    HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">" + GetValue + "</a>";
        //                                    href = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                                    htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV(label, GetValue) + "</a></td></tr></table></li>";
        //                                    HttpContext.Current.Session["Link"] = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;fcnt=" + HttpContext.Current.Request.QueryString["fcnt"].ToString() + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                                    Htmlitems1.Add(htmlItems);
        //                                }
        //                                else
        //                                {
        //                                    HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">" + GetValue + "</a>";
        //                                    href = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                                    htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV(label, GetValue) + "</a></td></tr></table></li>";
        //                                    HttpContext.Current.Session["Link"] = "<a href=\"family.aspx?&amp;fid=" + Family_Id + "&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                                    Htmlitems1.Add(htmlItems);
        //                                }
        //                            }
        //                        }

        //                    }
        //                }
        //            }

        //        }

        //        int h = Htmlitems1.Count;
        //        string HtmlItem_order = "";
        //        for (int i = 0; i < h; i++)
        //        {
        //            HtmlItem_order += Htmlitems1[i].ToString();
        //        }
        //        String html = "";

        //        if (null != HtmlItem_order && 0 < HtmlItem_order.Length)
        //        {
        //             html += "<td class='ea-nav-block ea-clickable'><div class='ea-nav-block-header'><div class='ea-nav-title'>Your current selection:</div></div>";
        //          //  html += "<td class='ea-nav-block ea-clickable'><div class='headimage'> Product Filter Options<br /><span>Your current selection :</span></div>";
        //            html += "<ul class='ea-remove-nav-block-values' >";
        //            html += HtmlItem_order;
        //            html += "</ul></td>";
        //        }
        //        HttpContext.Current.Session["Selected"] = html;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //protected void updateBreadCrumb(IBreadCrumbTrail bct)
        //{
        //    try
        //    {
        //        HttpContext.Current.Session["Link"] = null;
        //        HttpContext.Current.Session["Selected"] = null;
        //        HttpContext.Current.Session["BreadCrumbName"] = null;
        //        string separator = "";
        //        List<String> Htmlitems1 = new List<String>();
        //        String htmlItems = "";
        //        String href = "";
        //        string GetValue = "";
        //        foreach (INavigateNode node in bct.getSearchPath())
        //        {
        //            // string separator1= node.getSEOPath();
        //            IList<INavigateNode> nodes = bct.getSearchPath();

        //            string separator1 = node.getPath();
        //            String label = node.getLabel();
        //            if (node.getLabel() == "Brand")
        //            {
        //                HttpContext.Current.Session["Brand_Name_Dispaly"] = node.getValue();
        //            }
        //            if (node.getLabel() == "Model")
        //            {
        //                string[] ModelName = node.getValue().Split(':');
        //                GetValue = ModelName[1].ToString();
        //                HttpContext.Current.Session["Brand_Model_Name_Dispaly"] = ModelName[1].ToString();
        //            }
        //            else
        //            {
        //                GetValue = node.getValue();
        //            }
        //            if (node.getPath().Contains("AllProducts////WESAUSTRALASIA////Cellular Accessories////"))
        //            {
        //                string Category_breadcrumb = node.getPath().Replace("////", "#");
        //                string[] Category_breadcrumb_name = Category_breadcrumb.Split('#');
        //                int count_breadcrumb = Category_breadcrumb_name.Count();
        //                if (count_breadcrumb == 4)
        //                {
        //                        separator = "&nbsp;/&nbsp;";
        //                        HttpContext.Current.Session["BreadCrumbName"] = separator + "<a href=\"categorylist.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "&amp;tsb="+HttpUtility.UrlEncode(node.getValue())+"\">" + GetValue + "</a>";
        //                        if (HttpContext.Current.Session["Link"] == null)
        //                        {
        //                            HttpContext.Current.Session["Link"] = "<a href=\"Home.aspx\">";
        //                        }
        //                        href = "<a href=\"categorylist.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "&amp;tsb="+HttpUtility.UrlEncode(node.getValue())+"\">";
        //                        htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV(label, GetValue) + "</a></td></tr></table></li>";
        //                        HttpContext.Current.Session["Link"] = "<a href=\"categorylist.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "&amp;tsb=" + HttpUtility.UrlEncode(node.getValue()) + "\">"; 
        //                        Htmlitems1.Add(htmlItems);
        //                }
        //                else
        //                {
        //                            separator = "&nbsp;/&nbsp;";
        //                            HttpContext.Current.Session["BreadCrumbName"] += separator + "<a href=\"brandlist.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">" + GetValue + "</a>";
        //                            href = "<a href=\"brandlist.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            htmlItems = "<li class='ea-remove-nav-value' ><table><tr><td valign='middle'; align='left'>" + HttpContext.Current.Session["Link"].ToString() + "<img src='images/remove.png' style='border:none;'></a></td><td align='left'>" + href + hCAV(label, GetValue) + "</a></td></tr></table></li>";
        //                            HttpContext.Current.Session["Link"] = "<a href=\"brandlist.aspx?&amp;Path=" + HttpUtility.UrlEncode(objhelper.StringEnCrypt(node.getPath())) + "&amp;BreadCrumb_Name=" + HttpUtility.UrlEncode(node.getValue()) + "\">";
        //                            Htmlitems1.Add(htmlItems);
        //                }
        //            }

        //        }

        //        int h = Htmlitems1.Count;
        //        string HtmlItem_order = "";
        //        for (int i = 0; i < h; i++)
        //        {
        //            HtmlItem_order += Htmlitems1[i].ToString();
        //        }
        //        String html = "";

        //        if (null != HtmlItem_order && 0 < HtmlItem_order.Length)
        //        {
        //            html += "<td class='ea-nav-block ea-clickable' align='left'><div class='ea-nav-block-header'><div class='ea-nav-title'>Your current selection:</div></div>";
        //            //  html += "<td class='ea-nav-block ea-clickable'><div class='headimage'> Product Filter Options<br /><span>Your current selection :</span></div>";
        //            html += "<ul class='ea-remove-nav-block-values'>";
        //            html += HtmlItem_order;
        //            html += "</ul></td>";
        //        }
        //        HttpContext.Current.Session["Selected"] = html;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //protected void updateCommentary(INavigateResults res)
        //{
        //    if (res.getLastItem() == -1)
        //    {
        //        HttpContext.Current.Session["Spell_Correction"] = "<font  style='line-height: 30px; color: #FF0000; font-size: small; font-weight:100;'>No data was found matching your query.</font>";
        //    }
        //    else
        //    { 
        //        String commentary = res.getCommentary();
        //        String prettycomment = "Sorry. There are no results for '";
        //        String outcomment = "";
        //        string Search_Word = "";

        //        if (0 < commentary.Length)
        //        {
        //            if (-1 != commentary.IndexOf("Ignored"))
        //            {
        //                IBreadCrumbTrail bct = res.getBreadCrumbTrail();
        //                IList<INavigateNode> i = bct.getSearchPath();
        //                foreach (INavigateNode node in bct.getSearchPath())
        //                {
        //                    if (node.getValue() != "Cellular Accessories" && node.getValue() != "WESAUSTRALASIA" && node.getValue() != "AllProducts")
        //                    {
        //                        Search_Word = node.getValue();
        //                        outcomment = prettycomment + res.getQuestion() + "'.";
        //                        outcomment += " Search Found Results for '" + Search_Word + "'";
        //                    }
        //                    else
        //                    {
        //                        outcomment = prettycomment + res.getQuestion() + "'.";
        //                    }
        //                }

        //            }
        //            else if (-1 != commentary.IndexOf("Corrected Word"))
        //            {
        //                outcomment = res.getCommentary();
        //                string[] Outcomment = outcomment.Split(';');
        //                outcomment = Outcomment[0].ToString() + ";";
        //            }
        //            //Corrected Word
        //            HttpContext.Current.Session["Spell_Correction"] = "<font  style='line-height: 30px; color: #FF0000; font-size: small; font-weight:100;'>" + outcomment + "</font>";

        //        }
        //        else
        //        {
        //            HttpContext.Current.Session["Spell_Correction"] = null;
        //        }
        //    }
        //}

        //String hCAV(String label, String value)
        //{
        //    String html = "";
        //    if (null != label && 0 < label.Length)
        //    {
        //        html += "<span class='ea-remove-nav-value-label'  style='text-align:left';>" + label + ": </span><br>";
        //    }
        //    html += "<span class='ea-remove-nav-value-value'  style='text-align:left';>" + value + "</span>";
        //    return html;
        //}
        ////String createURITo(String path)
        ////{
        ////    return "Default.aspx?" + path;
        ////}

        //#endregion

        #region for Brand List
        //DataSet Brand_Product = new DataSet();
        //DataTable Brand_Product_Values = new DataTable();

        //public DataSet Get_Brand_Product(string Model,string Brand)
        //{
        //    try
        //    {
        //        IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WebCatDictionary);
        //        IOptions opts = ea.getOptions();
        //        opts.setResultsPerPage("0"); // ea_rpp.Value);   // use current settings
        //        opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
        //        opts.setGrouping("Category;1000");
        //        opts.setSubCategories(false);
        //        opts.setNavigateHierarchy(false);
        //        opts.setReturnSKUs(true);
        //        Create_Brand_Product_Table_Columns();
        //        INavigateResults res = null;
        //        if (HttpContext.Current.Request["Type"] != null)
        //        {
        //            if (HttpContext.Current.Request["Type"] != "Category")
        //            {
        //                res = ea.userAttributeClick(HttpContext.Current.Session["EA"].ToString(), "" + Model + "='" + Brand + "'");
        //            }
        //            else
        //            {
        //                res = ea.userCategoryClick(HttpContext.Current.Session["EA"].ToString()+"////"+Brand);
        //            }
        //        }
        //        else
        //        {
        //            //Model = Model.Replace(" ", "-").Replace("-+", "").Replace("&", "-").Replace("(", "-").Replace(")", "-").Replace("/", "-").Replace(",", "-");
        //           // Model = Model.Replace("+", "%2b");
        //            res = ea.userAttributeClick_Brand(HttpContext.Current.Session["EA"].ToString(), "Model = '" + Brand + ":" + Model + "'");
        //        }
        //        updateBreadCrumb(res.getBreadCrumbTrail());
        //        HttpContext.Current.Session["EA"] = res.getCatPath();
        //        IList<INavigateCategory> Category = res.getDetailedCategories();
        //        if (Category.Count > 0)
        //        {
        //            foreach (INavigateCategory categoryItem in Category)
        //            {
        //                DataRow row = Brand_Product.Tables["Category"].NewRow();
        //                IList<string> Id = categoryItem.getIDs();
        //                row["SUBCATID_L1"] = Id[0].ToString().Substring(2);
        //                row["SUBCATNAME_L1"] = categoryItem.getName();
        //                Brand_Product.Tables["Category"].Rows.Add(row);
        //            }
        //        }
        //        layoutGroups(res);
        //       //For Main Content Values.
        //        // IList<string> Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
        //        // EasyAsk_UserSearch_Attributes_DS(Attributes, res, Brand);
        //        // updateCommentary(res);
        //        HttpContext.Current.Session["Brand_Product_DS"] = Brand_Product;
        //        return Brand_Product;
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorhandler.ErrorMsg = ex;
        //        objErrorhandler.CreateLog();
        //        return null;
        //    }
        //}

        //void Get_Brand_DataSet_Values(INavigateResults res)
        //{
        //    int last = res.getLastItem();
        //    int colFmlyID = res.getColumnIndex("Family Id");
        //    int colFmlyName = res.getColumnIndex("Family Name");
        //    int colFmlyDesc = res.getColumnIndex("Family ShortDescription");
        //    int colFmlylongDesc = res.getColumnIndex("Family Description");
        //    int colFmlyImg = res.getColumnIndex("Family Thumbnail");

        //    int colProductID = res.getColumnIndex("Prod Id");
        //    int colProductCode = res.getColumnIndex("Prod Code");
        //    int colProductPrice = res.getColumnIndex("Price");
        //    int colProductDesc = res.getColumnIndex("Prod Description");
        //    int colProductImg = res.getColumnIndex("Prod Thumbnail");

        //    int colProductCount = res.getColumnIndex("Prod Count");
        //    int colFamilyProdCount = res.getColumnIndex("Family Prod Count");

        //    IList<INavigateCategory> item = res.getDetailedCategories();
        //    DataRow dRow;
        //    try
        //    {
        //        if (last >= 0)
        //        {
        //            for (int i = res.getFirstItem() - 1, col = 0; i < last; i++, col++)
        //            {
        //                for (int k = 0; k < 9; k++)
        //                {
        //                    dRow = Brand_Product_Values.NewRow();
        //                    dRow["FAMILY_ID"] = res.getCellData(i,colFmlyID);
        //                    dRow["FAMILY_NAME"] = res.getCellData(i,colFmlyName);
        //                    // dRow["DESCRIPTION1"] = res.getCellData(i, colFmlyDesc);
        //                    // dRow["LongDESCRIPTION"] = res.getCellData(i, colFmlylongDesc);
        //                    dRow["PRODUCT_ID"] = res.getCellData(i,colProductID);

        //                    string temp_family_count = res.getCellData(i,colFamilyProdCount);
        //                    string temp_product_count = res.getCellData(i,colProductCount);
        //                    string temp_fmly_Image = res.getCellData(i,colFmlyImg).ToString();
        //                    string temp_product_Image = res.getCellData(i,colProductImg).ToString();
        //                    string image_string = "";

        //                    if (temp_fmly_Image != "" && temp_product_Image != "")
        //                    {
        //                        if (temp_product_count.ToString() == "1")
        //                        {
        //                            image_string = temp_product_Image.Substring(42);
        //                        }
        //                        else
        //                        {
        //                            image_string = temp_fmly_Image.Substring(42);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        image_string = "noimage.gif";
        //                    }

        //                    if (k == 0)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "1";
        //                        dRow["STRING_VALUE"] = res.getCellData(i,colProductCode);//For the Product Code
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "Code";
        //                        dRow["ATTRIBUTE_TYPE"] = "1";
        //                    }
        //                    if (k == 1)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "5";
        //                        dRow["STRING_VALUE"] = "";
        //                        if (res.getCellData(i,colProductPrice) == "" || res.getCellData(i,colProductPrice) == string.Empty)
        //                        {
        //                            dRow["NUMERIC_VALUE"] = "0";
        //                        }
        //                        else
        //                        {
        //                            dRow["NUMERIC_VALUE"] = res.getCellData(i,colProductPrice).Substring(1);//For Cost
        //                        }
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "Cost";
        //                        dRow["ATTRIBUTE_TYPE"] = "4";
        //                    }
        //                    if (k == 2)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "62";
        //                        dRow["STRING_VALUE"] = res.getCellData(i,colProductDesc);//Product Description.
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "Description";
        //                        dRow["ATTRIBUTE_TYPE"] = "1";
        //                    }
        //                    if (k == 3)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "449";
        //                        dRow["STRING_VALUE"] = res.getCellData(i,colFmlyDesc);//Family Description
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "PROD_DSC";
        //                        dRow["ATTRIBUTE_TYPE"] = "1";
        //                    }
        //                    if (k == 4)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "492";
        //                        dRow["STRING_VALUE"] = "";
        //                        if (res.getCellData(i,colProductPrice) == "" || res.getCellData(i,colProductPrice) == string.Empty)
        //                        {
        //                            dRow["NUMERIC_VALUE"] = "0";
        //                        }
        //                        else
        //                        {
        //                            dRow["NUMERIC_VALUE"] = res.getCellData(i,colProductPrice).Substring(1);//For Cost
        //                        }
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "PROD_EXT_PRI_3";
        //                        dRow["ATTRIBUTE_TYPE"] = "4";
        //                    }
        //                    if (k == 5)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "453";
        //                        dRow["STRING_VALUE"] = image_string;
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "jpg";
        //                        dRow["OBJECT_NAME"] = image_string;
        //                        dRow["ATTRIBUTE_NAME"] = "Web Image1";
        //                        dRow["ATTRIBUTE_TYPE"] = "3";
        //                    }
        //                    if (k == 6)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "7";
        //                        dRow["STRING_VALUE"] = image_string;
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "jpg";
        //                        dRow["OBJECT_NAME"] = image_string;
        //                        dRow["ATTRIBUTE_NAME"] = "Product Image1";
        //                        dRow["ATTRIBUTE_TYPE"] = "3";
        //                    }
        //                    if (k == 7)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "452";
        //                        dRow["STRING_VALUE"] = image_string;
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "jpg";
        //                        dRow["OBJECT_NAME"] = image_string;
        //                        dRow["ATTRIBUTE_NAME"] = "TWeb Image1";
        //                        dRow["ATTRIBUTE_TYPE"] = "3";
        //                    }
        //                    if (k == 8)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "4";
        //                        dRow["STRING_VALUE"] = res.getCellData(i,colFmlylongDesc);//Family Description
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "DESCRIPTIONS";
        //                        dRow["ATTRIBUTE_TYPE"] = "1";
        //                    }

        //                    dRow["CATEGORY_ID"] = "";
        //                    // dRow["CATEGORY_NAME"] = string.Empty;
        //                    // dRow["PARENT_CATEGORY_NAME"] = "";
        //                    dRow["PARENT_CATEGORY_ID"] = "";
        //                    dRow["SUBCATNAME_L1"] = "";
        //                    dRow["SUBCATNAME_L2"] = "";
        //                    //  dRow["CUSTOM_NUM_FIELD3"] = "2";
        //                    //  dRow["PARENT_FAMILY_ID"] = "0";
        //                    // dRow["(No column name)"] = "";
        //                    // dRow["LFID"] = res.getCellData(i, colFmlyID);
        //                    //  dRow["Family_Prod_Count"] = temp_family_count;
        //                    //  dRow["Prod_Count"] = temp_product_count;

        //                    if (temp_family_count == temp_product_count)
        //                    {
        //                       // dRow["STATUS"] = true;
        //                    }
        //                    else
        //                    {
        //                      //  dRow["STATUS"] = false;
        //                    }
        //                    Brand_Product_Values.Rows.Add(dRow);
        //                }
        //            }
        //                j++;
        //            }
        //        }
           
        //    catch (Exception ex)
        //    {
        //    }
        //    HttpContext.Current.Session["Brand_Product_Value"] = Brand_Product_Values;
        //    //return HomeSearch;
        //}

        //private void Get_Brand_DataSet_Values(INavigateResults res,IResultRow item1,String name)
        //{
        //     int last = res.getLastItem();
        //    int colFmlyID = res.getColumnIndex("Family Id");
        //    int colFmlyName = res.getColumnIndex("Family Name");
        //    int colFmlyDesc = res.getColumnIndex("Family ShortDescription");
        //    int colFmlylongDesc = res.getColumnIndex("Family Description");
        //    int colFmlyImg = res.getColumnIndex("Family Thumbnail");

        //    int colProductID = res.getColumnIndex("Prod Id");
        //    int colProductCode = res.getColumnIndex("Prod Code");
        //    int colProductPrice = res.getColumnIndex("Price");
        //    int colProductDesc = res.getColumnIndex("Prod Description");
        //    int colProductImg = res.getColumnIndex("Prod Thumbnail");

        //    int colProductCount = res.getColumnIndex("Prod Count");
        //    int colFamilyProdCount = res.getColumnIndex("Family Prod Count");

        //    IList<INavigateCategory> item = res.getDetailedCategories();
        //    DataRow dRow;
        //    try
        //    {

        //       // if (last >= 0)
        //      //  {
        //        //    for (int i = res.getFirstItem() - 1, col = 0; i < last; i++, col++)
        //          //  {
        //                for (int k = 0; k < 9; k++)
        //                {
        //                    dRow = Brand_Product_Values.NewRow();
        //                    dRow["FAMILY_ID"] = item1.getCellData(colFmlyID);
        //                    dRow["FAMILY_NAME"] = item1.getCellData(colFmlyName);
        //                    // dRow["DESCRIPTION1"] = res.getCellData(i, colFmlyDesc);
        //                    // dRow["LongDESCRIPTION"] = res.getCellData(i, colFmlylongDesc);
        //                    dRow["PRODUCT_ID"] = item1.getCellData(colProductID);

        //                    string temp_family_count = item1.getCellData(colFamilyProdCount);
        //                    string temp_product_count = item1.getCellData(colProductCount);
        //                    string temp_fmly_Image = item1.getCellData(colFmlyImg).ToString();
        //                    string temp_product_Image = item1.getCellData(colProductImg).ToString();
        //                    string image_string = "";

        //                    if (temp_fmly_Image != "" && temp_product_Image != "")
        //                    {
        //                        if (temp_product_count.ToString() == "1")
        //                        {
        //                            image_string = temp_product_Image.Substring(42);
        //                        }
        //                        else
        //                        {
        //                            image_string = temp_fmly_Image.Substring(42);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        image_string = "noimage.gif";
        //                    }

        //                    if (k == 0)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "1";
        //                        dRow["STRING_VALUE"] = item1.getCellData(colProductCode);//For the Product Code
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "Code";
        //                        dRow["ATTRIBUTE_TYPE"] = "1";
        //                    }
        //                    if (k == 1)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "5";
        //                        dRow["STRING_VALUE"] = "";
        //                        if (item1.getCellData(colProductPrice) == "" || item1.getCellData(colProductPrice) == string.Empty)
        //                        {
        //                            dRow["NUMERIC_VALUE"] = "0";
        //                        }
        //                        else
        //                        {
        //                            dRow["NUMERIC_VALUE"] = item1.getCellData(colProductPrice).Substring(1);//For Cost
        //                        }
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "Cost";
        //                        dRow["ATTRIBUTE_TYPE"] = "4";
        //                    }
        //                    if (k == 2)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "62";
        //                        dRow["STRING_VALUE"] = item1.getCellData(colProductDesc);//Product Description.
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "Description";
        //                        dRow["ATTRIBUTE_TYPE"] = "1";
        //                    }
        //                    if (k == 3)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "449";
        //                        dRow["STRING_VALUE"] = item1.getCellData(colFmlyDesc);//Family Description
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "PROD_DSC";
        //                        dRow["ATTRIBUTE_TYPE"] = "1";
        //                    }
        //                    if (k == 4)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "492";
        //                        dRow["STRING_VALUE"] = "";
        //                        if (item1.getCellData(colProductPrice) == "" || item1.getCellData(colProductPrice) == string.Empty)
        //                        {
        //                            dRow["NUMERIC_VALUE"] = "0";
        //                        }
        //                        else
        //                        {
        //                            dRow["NUMERIC_VALUE"] = item1.getCellData(colProductPrice).Substring(1);//For Cost
        //                        }
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "PROD_EXT_PRI_3";
        //                        dRow["ATTRIBUTE_TYPE"] = "4";
        //                    }
        //                    if (k == 5)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "453";
        //                        dRow["STRING_VALUE"] = image_string;
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "jpg";
        //                        dRow["OBJECT_NAME"] = image_string;
        //                        dRow["ATTRIBUTE_NAME"] = "Web Image1";
        //                        dRow["ATTRIBUTE_TYPE"] = "3";
        //                    }
        //                    if (k == 6)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "7";
        //                        dRow["STRING_VALUE"] = image_string;
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "jpg";
        //                        dRow["OBJECT_NAME"] = image_string;
        //                        dRow["ATTRIBUTE_NAME"] = "Product Image1";
        //                        dRow["ATTRIBUTE_TYPE"] = "3";
        //                    }
        //                    if (k == 7)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "452";
        //                        dRow["STRING_VALUE"] = image_string;
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "jpg";
        //                        dRow["OBJECT_NAME"] = image_string;
        //                        dRow["ATTRIBUTE_NAME"] = "TWeb Image1";
        //                        dRow["ATTRIBUTE_TYPE"] = "3";
        //                    }
        //                    if (k == 8)
        //                    {
        //                        dRow["ATTRIBUTE_ID"] = "4";
        //                        dRow["STRING_VALUE"] = item1.getCellData(colFmlylongDesc);//Family Description
        //                        dRow["NUMERIC_VALUE"] = "0";
        //                        dRow["OBJECT_TYPE"] = "NULL";
        //                        dRow["OBJECT_NAME"] = "NULL";
        //                        dRow["ATTRIBUTE_NAME"] = "DESCRIPTIONS";
        //                        dRow["ATTRIBUTE_TYPE"] = "1";
        //                    }

        //                    dRow["CATEGORY_ID"] = "";
        //                    // dRow["CATEGORY_NAME"] = string.Empty;
        //                    // dRow["PARENT_CATEGORY_NAME"] = "";
        //                    dRow["PARENT_CATEGORY_ID"] = "";
        //                    dRow["SUBCATNAME_L1"] = name;
        //                    dRow["SUBCATNAME_L2"] = "";
        //                    //  dRow["CUSTOM_NUM_FIELD3"] = "2";
        //                    //  dRow["PARENT_FAMILY_ID"] = "0";
        //                    // dRow["(No column name)"] = "";
        //                    // dRow["LFID"] = res.getCellData(i, colFmlyID);
        //                    //  dRow["Family_Prod_Count"] = temp_family_count;
        //                    //  dRow["Prod_Count"] = temp_product_count;

        //                    if (temp_family_count == temp_product_count)
        //                    {
        //                       // dRow["STATUS"] = true;
        //                    }
        //                    else
        //                    {
        //                      //  dRow["STATUS"] = false;
        //                    }
        //                    Brand_Product_Values.Rows.Add(dRow);
        //                }
        //            }
        //       // }
        //    //}
        //    catch (Exception ex)
        //    {
        //    }
        //    HttpContext.Current.Session["Brand_Product_Value"] = Brand_Product_Values;
        //   // return HomeSearch;
        //}

        //private void Create_Brand_Product_Table_Columns()
        //{
        //    Brand_Product.Tables.Add("Category");
        //    Brand_Product.Tables["Category"].Columns.Add("SUBCATID_L1", typeof(string));
        //    Brand_Product.Tables["Category"].Columns.Add("SUBCATNAME_L1", typeof(string));

        //    Brand_Product.Tables.Add(Brand_Product_Values);
        //    Brand_Product_Values.Columns.Add("FAMILY_ID", typeof(string));
        //    Brand_Product_Values.Columns.Add("CATEGORY_ID", typeof(string));
        //    Brand_Product_Values.Columns.Add("PARENT_CATEGORY_ID", typeof(string));
        //    Brand_Product_Values.Columns.Add("FAMILY_NAME", typeof(string));
        //    Brand_Product_Values.Columns.Add("PRODUCT_ID", typeof(string));
        //    Brand_Product_Values.Columns.Add("ATTRIBUTE_ID", typeof(string));
        //    Brand_Product_Values.Columns.Add("STRING_VALUE", typeof(string));
        //    Brand_Product_Values.Columns.Add("NUMERIC_VALUE", typeof(string));
        //    Brand_Product_Values.Columns.Add("OBJECT_TYPE", typeof(string));
        //    Brand_Product_Values.Columns.Add("OBJECT_NAME", typeof(string));
        //    Brand_Product_Values.Columns.Add("ATTRIBUTE_NAME", typeof(string));
        //    Brand_Product_Values.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
        //    Brand_Product_Values.Columns.Add("SUBCATNAME_L1", typeof(string));
        //    Brand_Product_Values.Columns.Add("SUBCATNAME_L2", typeof(string));

        //    //Brand_Product_Values.Columns.Add("DESCRIPTION1", typeof(string));
        //    //Brand_Product_Values.Columns.Add("LongDESCRIPTION", typeof(string));
        //    //Brand_Product_Values.Columns.Add("CATEGORY_NAME", typeof(string));
        //    //Brand_Product_Values.Columns.Add("PARENT_CATEGORY_NAME", typeof(string));
        //    //Brand_Product_Values.Columns.Add("CUSTOM_NUM_FIELD3", typeof(string));
        //    //Brand_Product_Values.Columns.Add("PARENT_FAMILY_ID", typeof(string));
        //    //Brand_Product_Values.Columns.Add("(No column name)", typeof(string));
        //    //Brand_Product_Values.Columns.Add("LFID", typeof(string));
        //    //Brand_Product_Values.Columns.Add("Family_Prod_Count", typeof(int));
        //    //Brand_Product_Values.Columns.Add("Prod_Count", typeof(int));
        //    //Brand_Product_Values.Columns.Add("STATUS", typeof(bool));

        //}

        //void layoutGroups(INavigateResults res)
        //{
        //    if (res.isGroupedResult())
        //    {
        //        String html = "";
        //        IGroupedResultSet groups = res.getGroupedResult();
        //        int groupType = groups.getGroupCriteriaType();
        //        String path = res.getCatPath();
        //        for (int i = groups.getStartGroup(), len = groups.getEndGroup(); i <= len; i++)
        //        {
        //            IGroupedResult group = groups.getGroup(i);
        //            String name = group.getGroupValue();
        //            if (null == name || 0 == name.Length)
        //            {
        //                continue;  // skip empty
        //            }
        //            String nodeString = groups.getNodeString(group);
        //            int startRow = group.getStartRow();
        //            int endRow = group.getEndRow();
        //            html += "<div style='border:1px solid #D1D3D4'><table border='0' cellpadding='3' cellspacing='1' width='100%' class='EAMTitle'>";
        //            html += "<tr><td width='100%' class='EA_EStoreGroupTitle'><div style='float:left;'>" + group.getGroupValue();
        //            html += "</div><div style='float:right'>" + ((endRow - startRow + 1) + " of " + group.getTotalNumberOfRows());
        //            if (null != nodeString && 0 < nodeString.Length)
        //            {  // need to have some criteria to drill into this is the 'unknown' group
        //                //    html += "<a class='EA_EStoreGroupTitle' href='" + formURLToGroup(path, groupType, nodeString) + "'> more</a>";
        //            }
        //            html += "</div></td></tr></table></div>";
        //            html += "<table cellpadding='2' cellspacing='1' border='0' class='EA_EStoreProductGridViewTable' style='table-layout:fixed;'>";
        //            html += "<tr>";
        //            for (int j = startRow - 1; j < endRow; j++)
        //            {
        //                IResultRow item = group.getItem(j);
        //                Get_Brand_DataSet_Values(res, item, name);
        //                //  html += "<td valign='top' class='resultscell' style='width:125px;'><img src='" + resolveImageURL(item.getCellData(colImg)) + "' /><br /><div class='resultscelltext'>" + item.getCellData(colName) + "<br />" + item.getCellData(colPrice) + "</div></td>";
        //            }
        //            html += "</tr></table>"; //</div></div>";
        //        }
        //        // searchresultsPH.Controls.Add(new LiteralControl(html));
        //    }
        //    else
        //    { 
        //        Get_Brand_DataSet_Values(res);
        //    }
        //}

        //public void BreadCrumbClick1(string rpp, string attribute_value, int curPage, string pageOp)
        //{
        //    IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, "wesCatBrand");
        //    IOptions opts = ea.getOptions();
        //    opts.setResultsPerPage("0"); // ea_rpp.Value);   // use current settings
        //    opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
        //    opts.setGrouping(m_grp.Equals("-default-") ? "" : m_grp);
        //    opts.setSubCategories(false);
        //    opts.setNavigateHierarchy(false);
        //    opts.setReturnSKUs(true);
        //    Brand_Model = new DataSet();
        //    DataTable dTable = new DataTable();
        //    INavigateResults res = ea.userBreadCrumbClick1(HttpContext.Current.Session["EA"].ToString());
        //    updateBreadCrumb(res.getBreadCrumbTrail());
        //    try
        //    {
        //        int last = res.getLastItem();
        //        int Model_Name = res.getColumnIndex("ModelValue");
        //        int Model_Image = res.getColumnIndex("Model Thumbnail");
        //        int Brand_Name = res.getColumnIndex("Brand");
        //        // IList Model = res.getDetailedAttributeValues("Model");
        //        Brand_Model.Tables.Add(dTable);
        //        dTable.Columns.Add("TOSUITE_MODEL", typeof(string));
        //        dTable.Columns.Add("IMAGE_FILE", typeof(string));
        //        dTable.Columns.Add("Brand", typeof(string));
        //        string image_string = "";
        //        DataRow dRow;
        //        for (int i = res.getFirstItem() - 1; i < last; i++)
        //        {
        //            dRow = dTable.NewRow();
        //            dRow["TOSUITE_MODEL"] = res.getCellData(i, Model_Name);
        //            // dRow["IMAGE_FILE"] = 
        //            string Model_Image_Name = res.getCellData(i, Model_Image).ToString();
        //            if (Model_Image_Name != "" && Model_Image_Name != null)
        //            {
        //                image_string = Model_Image_Name.Substring(42);
        //            }
        //            else
        //            {
        //                image_string = "noimage.gif";
        //            }
        //            dRow["IMAGE_FILE"] = image_string;
        //            dRow["Brand"] = res.getCellData(i, Brand_Name);
        //            dTable.Rows.Add(dRow);
        //        }
        //    }

        //    catch (Exception)
        //    {
        //    }
        //    HttpContext.Current.Session["WESBrand_Model"] = Brand_Model;
        //}

        //public void BreadCrumbClick_Brand(string rpp, string attribute_value, int curPage, string pageOp)
        //{
        //    try
        //    {
        //        IRemoteEasyAsk ea = Impl.RemoteFactory.create(EasyAsk_URL, EasyAsk_Port, EasyAsk_WebCatDictionary);
        //        IOptions opts = ea.getOptions();
        //        opts.setResultsPerPage("0"); // ea_rpp.Value);   // use current settings
        //        opts.setSortOrder(m_sort.Equals("-default-") ? "" : m_sort);       // use current settings
        //        opts.setGrouping("Category;1000");
        //        opts.setSubCategories(false);
        //        opts.setNavigateHierarchy(false);
        //        opts.setReturnSKUs(true);
        //        Create_Brand_Product_Table_Columns();
        //        DataSet ds = new DataSet();
        //        INavigateResults res = null;
        //        if (curPage < 1)
        //        {
        //            res = ea.userBreadCrumbClick(HttpContext.Current.Session["EA"].ToString());
        //            HttpContext.Current.Session["EA"] = res.getCatPath();
        //            //Get_DataSet_Values(res);//For Main Content Values.
        //        }
        //        else
        //        {
        //           res = ea.userPageOp(HttpContext.Current.Session["EA"].ToString(), curPage.ToString(), pageOp);
        //            //  return ds;
        //        }

        //        IList<INavigateCategory> Category = res.getDetailedCategories();
        //        if (Category.Count > 0)
        //        {
        //            foreach (INavigateCategory categoryItem in Category)
        //            {
        //                DataRow row = Brand_Product.Tables["Category"].NewRow();
        //                IList<string> Id = categoryItem.getIDs();
        //                row["SUBCATID_L1"] = Id[0].ToString().Substring(2);
        //                row["SUBCATNAME_L1"] = categoryItem.getName();
        //                Brand_Product.Tables["Category"].Rows.Add(row);
        //            }
        //        }
        //        updateBreadCrumb(res.getBreadCrumbTrail());
        //        layoutGroups(res);
        //        IList<string> Attributes = res.getAttributeNames(EasyAskConstants.ATTR_FILTER_NORMAL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
        //        EasyAsk_UserSearch_Attributes_DS(Attributes, res, attribute_value);
        //        HttpContext.Current.Session["Brand_Product_DS"] = Brand_Product;
            
        //    }
        //    catch (Exception ex)
        //    {
        //        //   return null;
        //    }
        //}


        #endregion


    }

}