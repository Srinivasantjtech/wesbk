using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;

using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UC_category : System.Web.UI.UserControl
{
    ConnectionDB objConnectionDB = new ConnectionDB();
    ErrorHandler objErrorHandler = new ErrorHandler();
    HelperDB objHelperDB = new HelperDB();
    HelperServices objHelperServices = new HelperServices();
    Security objSecurity = new Security();
    string strFile = HttpContext.Current.Server.MapPath("ProdImages");
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string ST_Category()
    {
        HelperServices objHelperServices = new HelperServices();
        //return (Category_RenderHTML("CATEGORY", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString())));
        return ST_Category_Load("CATEGORY", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()));
    }
    public string ST_Category_Load(string Package, string _SkinRootPath)
    {
        string sHTML = string.Empty;

        try
        {
            StringTemplateGroup _stg_container = null;
            StringTemplateGroup _stg_records = null;
            StringTemplate _stmpl_container = null;
            StringTemplate _stmpl_records = null;
            StringTemplate _stmpl_recordstemp = new StringTemplate();
            //  StringTemplate _stmpl_records1 = null;
            // StringTemplate _stmpl_recordsrows = null;
            TBWDataList[] lstrecords = new TBWDataList[0];
            //  TBWDataList[] lstrows = new TBWDataList[0];

            //StringTemplateGroup _stg_container1 = null;
            // StringTemplateGroup _stg_records1 = null;
            //TBWDataList1[] lstrecords1 = new TBWDataList1[0];
            // TBWDataList1[] lstrows1 = new TBWDataList1[0];

            //   DataSet dscat = new DataSet();
            EasyAsk_WES EasyAsk = new EasyAsk_WES();
          

            DataSet dsrecordsM = new DataSet();
            DataSet dsrecordsS = new DataSet();
            //DataTable dt = null;
            //DataRow[] drs = null;

            dsrecordsM = EasyAsk.GetCategoryAndBrand("MainCategory");
            dsrecordsS = EasyAsk.GetCategoryAndBrand("SubCategory");
            // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
           // int ictrows = 0;

            //if (dsrecordsM != null && dsrecordsM.Tables.Count > 0 && dsrecordsM.Tables[0].Rows.Count > 0)
            //{

            //}

         


            _stg_records = new StringTemplateGroup("row", _SkinRootPath);
            _stg_container = new StringTemplateGroup("main", _SkinRootPath);


            lstrecords = new TBWDataList[dsrecordsM.Tables[0].Rows.Count + 1];




            int ictrecords = 0;
          // int ictrecords1 = 0;
            string catcell = Package + "\\cell";
            foreach (DataRow dr in dsrecordsM.Tables[0].Rows)//For Records
            {
                if (Convert.ToInt32(dr["SUB_COUNT"].ToString()) > 0)
                {
                    _stmpl_records = _stg_records.GetInstanceOf(catcell);

                    
                    _stmpl_records.SetAttribute("TBT_PARENT_CATEGORY_NAME", dr["CATEGORY_NAME"]);
                    _stmpl_records.SetAttribute("TBT_PARENT_CATEGORY_ID", dr["CATEGORY_ID"]);
                    _stmpl_records.SetAttribute("TBT_CUSTOM_NUM_FIELD3", dr["CUSTOM_NUM_FIELD3"]);
                        _stmpl_records.SetAttribute("EA_PATH", dr["EA_PATH"]);
                    if ( dr["SHORT_DESC"].ToString().Length>150) 
                        _stmpl_records.SetAttribute("TBT_SHORT_DESC", dr["SHORT_DESC"].ToString().Substring(0,150));
                    else
                        _stmpl_records.SetAttribute("TBT_SHORT_DESC", dr["SHORT_DESC"]);

                        _stmpl_records.SetAttribute("TBT_PARENT_CATEGORY_IMAGE", GetImagePath(dr["IMAGE_FILE"].ToString(),""));
                            
                            
                    _stmpl_records.SetAttribute("TBT_SUB_CATEGORY_LIST", getSubcategoryList(dsrecordsS, dr["CATEGORY_ID"].ToString(), 1, _SkinRootPath));
                    
                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                    ictrecords++;
                }
            }


            _stmpl_container = _stg_container.GetInstanceOf(Package + "\\main");



            _stmpl_container.SetAttribute("TBWDataList", lstrecords);
            sHTML = _stmpl_container.ToString();
            if (sHTML.Contains("data-original=\"prodimages\""))
            sHTML = sHTML.Replace("data-original=\"prodimages\"", "data-original=\"images/noimage.gif\"");
            if (sHTML.Contains("data-original=\"\""))
                sHTML = sHTML.Replace("data-original=\"\"", "data-original=\"images/noimage.gif\"");


            //return objHelperServices.StripWhitespace(sHTML);
            return sHTML;


        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            sHTML = "";
        }
        return sHTML;
    }
    protected string GetImagePath(object Path, string tosize)
    {
        string retpath;
        //System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + Path.ToString());
        //if (Fil.Exists)
        //{
        //    if (tosize == "TH50")
        //        retpath = objHelperServices.SetImageFolderPath(Path.ToString().Replace("\\", "/"), "_th", "_th50");
        //    else
        //        retpath = Path.ToString().Replace("\\", "/");
        //    //retpath = objHelperServices.SetImageFolderPath(Path.ToString().Replace("\\", "/"), "_th", "_Images_200");
        //}
        //else
        //    retpath = "/images/noimage.gif";

        if (tosize == "TH50")
            retpath = objHelperServices.SetImageFolderPath(Path.ToString().Replace("\\", "/"), "_th", "_th50");
        else
            retpath = Path.ToString().Replace("\\", "/");

        return retpath;
    }
    public string getSubcategoryList(DataSet subdata, string catid, int part, string _SkinRootPath)
    {
      //  StringTemplateGroup _stg_container = null;
        StringTemplateGroup _stg_records = null;
      //  StringTemplate _stmpl_container = null;
        StringTemplate _stmpl_records = null;
        StringTemplate _stmpl_recordstemp = new StringTemplate();
        //  StringTemplate _stmpl_records1 = null;
        // StringTemplate _stmpl_recordsrows = null;
        TBWDataList[] lstrecords = new TBWDataList[0];
        string _tsb = string.Empty;
        string _tsm = string.Empty;
        string _searchstr = string.Empty;
       // string _byp = "2";
      //  string _bypcat = null;
       // string _pid = "";
      //  string _fid = "";
      //  string _Ea_path = "";

        if (Request.QueryString["tsm"] != null)
            _tsm = Request.QueryString["tsm"];

        if (Request.QueryString["tsb"] != null)
            _tsb = Request.QueryString["tsb"];

        if (Request.QueryString["searchstr"] != null)
            _searchstr = Request.QueryString["searchstr"];
        if (Request.QueryString["srctext"] != null)
            _searchstr = Request.QueryString["srctext"];

        _stg_records = new StringTemplateGroup("cell", _SkinRootPath);
        string subhtml = string.Empty;
        //DataTable dt = null;
        try
        {
            DataRow[] drs = subdata.Tables[0].Select("TBT_PARENT_CATEGORY_ID='" + catid + "'");
            if (drs.Length > 0)
            {
                string catcell1 = "Category\\cell1";
                foreach (DataRow dr in drs.Take(6).CopyToDataTable().Rows)
                {
                    _stmpl_records = _stg_records.GetInstanceOf(catcell1);
                    
                     _stmpl_records.SetAttribute("TBT_CATEGORY_ID",  HttpUtility.UrlEncode(dr["CATEGORY_ID"].ToString()));

                        if (dr["TBT_CUSTOM_NUM_FIELD3"]!=System.DBNull.Value)
                        {
                            _stmpl_records.SetAttribute("TBT_CUSTOM_NUM_FIELD3", Convert.ToInt32(dr["TBT_CUSTOM_NUM_FIELD3"]).ToString());
                        }
                        else
                        {
                            _stmpl_records.SetAttribute("TBT_CUSTOM_NUM_FIELD3", "");
                        }

                     _stmpl_records.SetAttribute("TBT_PARENT_CATEGORY_ID",  HttpUtility.UrlEncode(dr["TBT_PARENT_CATEGORY_ID"].ToString()));
                   // _stmpl_records.SetAttribute("TBT_CATEGORY_ID", dr["CATEGORY_ID"].ToString());
                    
                    
                     _stmpl_records.SetAttribute("TBT_BRAND", HttpUtility.UrlEncode(_tsb));
                        _stmpl_records.SetAttribute("TBT_MODEL", HttpUtility.UrlEncode(_tsm));
                        _stmpl_records.SetAttribute("TBT_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(_searchstr));
                        _stmpl_records.SetAttribute("TBT_ATTRIBUTE_TYPE","Category");
                          _stmpl_records.SetAttribute("TBT_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(dr["CATEGORY_NAME"].ToString()));
                        _stmpl_records.SetAttribute("TBT_ATTRIBUTE_BRAND",_tsb);
                           
                          _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(dr["EA_PATH"] + "////" + dr["TBT_PARENT_CATEGORY_NAME"])));                                                    
                                
                               
                                 
                                 
                    
                    _stmpl_records.SetAttribute("TBT_CATEGORY_NAME", dr["CATEGORY_NAME"]);
                    subhtml = subhtml + _stmpl_records.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            return "";
        }
        return subhtml;
    }

    public string Category_RenderHTML(string Package, string SkinRootPath)
    {
        try
        {
        string skin_container = null;

        int grid_cols = 0;
        int grid_rows = 0;
        string skin_sql_container = null;
        string skin_sql_param_container = null;
        string skin_records = null;
        TBWDataList[] lstrecords = new TBWDataList[0];
        StringTemplateGroup stg_records = null;
        StringTemplate bodyST = null;
        StringTemplate bodyST_categorylist = null;
        StringTemplate bodyST_head = null;
        StringTemplate bodyST_list1 = null;
        string firstval = null;
        //List<string> name = new List<string>();
        StringBuilder name = new StringBuilder();
        System.Text.StringBuilder categorylist = new StringBuilder();
        System.Text.StringBuilder categoryrowlist = new StringBuilder();
        int indV = 0;
        int bodyValue = 0;
        EasyAsk_WES EasyAsk=new EasyAsk_WES();
        DataSet dspkg = new DataSet();
        string _tsb = string.Empty;
        string _tsm = string.Empty;
        string _searchstr = string.Empty;
       // string _byp = "2";
       // string _bypcat=null;
      //  string _pid = "";
      //  string _fid = "";
      //  string _Ea_path = "";
                
                     

                if (Request.QueryString["tsm"] != null)
                    _tsm = Request.QueryString["tsm"];

                if (Request.QueryString["tsb"] != null)
                    _tsb = Request.QueryString["tsb"];

                if (Request.QueryString["searchstr"] != null)
                    _searchstr = Request.QueryString["searchstr"];
                if (Request.QueryString["srctext"] != null)
                    _searchstr = Request.QueryString["srctext"];

                

        // old Code commant by Jtech
        //string sqlpkginfo = " SELECT * FROM TBW_PACKAGE ";
        //sqlpkginfo = sqlpkginfo + " WHERE PACKAGE_NAME = '" + Package + "'";

        //dspkg = GetDataSet(sqlpkginfo);
                // old Code commant by Jtech
                dspkg = (DataSet)objHelperDB.GetGenericDataDB(Package, "GET_PACKAGE_WITHOUT_ISROOT", HelperDB.ReturnType.RTDataSet);

        if (dspkg != null)
        {
            if (dspkg.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dspkg.Tables[0].Rows)
                {
                    skin_container = dr["SKIN_NAME"].ToString();
                    grid_cols = Convert.ToInt32(dr["GRID_COLS"]);
                    grid_rows = Convert.ToInt32(dr["GRID_ROWS"]);
                    skin_sql_container = dr["SKIN_SQL"].ToString();
                    skin_sql_param_container = dr["SKIN_SQL_PARAM"].ToString();
                    skin_records = dr["SKIN_NAME"].ToString();
                }
            }
        }
        
        //Build the inner body of the HTML
        stg_records = new StringTemplateGroup(skin_records, SkinRootPath + "\\" + skin_records);
        //DataSet dsrecords = GetDataSet(skin_sql_container);
        DataSet dsrecords = EasyAsk.GetCategoryAndBrand("SubCategory"); 
        if (dsrecords != null && dsrecords.Tables[0].Rows.Count != 0)
        {
        lstrecords = new TBWDataList[dsrecords.Tables[0].Rows.Count * 2];
        int catno = 0;
        if (dsrecords.Tables[0].Rows[0]["TBT_PARENT_CATEGORY_NAME"].ToString() != null && dsrecords.Tables[0].Rows[0]["TBT_PARENT_CATEGORY_NAME"].ToString() != string.Empty)
        {
            //Build the Sub heading 
            firstval = dsrecords.Tables[0].Rows[0]["TBT_PARENT_CATEGORY_NAME"].ToString().ToUpper();
            bodyST_categorylist = stg_records.GetInstanceOf("cell");
            DataRow ddr = dsrecords.Tables[0].Rows[0];
            foreach (DataColumn dc in dsrecords.Tables[0].Columns)
            {
                if (dc.ColumnName.ToString().Contains("CUSTOM_NUM"))
                    if (ddr[dc.ColumnName].ToString().Length > 0)
                        bodyST_categorylist.SetAttribute(dc.ColumnName, Convert.ToDouble(ddr[dc.ColumnName].ToString()));
                    else
                        bodyST_categorylist.SetAttribute(dc.ColumnName, "0");
                else
                {
                    if ("TBT_PARENT_CATEGORY_IMAGE" == dc.ColumnName)
                    {
                        // System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + ddr[dc.ColumnName].ToString().Replace("/","\\" ));
                        //if (Fil.Exists)
                        //{
                        //    bodyST_categorylist.SetAttribute(dc.ColumnName,  ddr[dc.ColumnName].ToString().Replace("\\", "/"));
                        //}
                        //else
                        //{
                        //    bodyST_categorylist.SetAttribute(dc.ColumnName, "/Images/Noimage.gif");

                        //}
                        if(ddr[dc.ColumnName].ToString() != "")
                             bodyST_categorylist.SetAttribute(dc.ColumnName, ddr[dc.ColumnName].ToString().Replace("\\", "/"));
                        else
                            bodyST_categorylist.SetAttribute(dc.ColumnName, "/Images/Noimage.gif");

                        //bodyST_categorylist.SetAttribute(dc.ColumnName, ddr[dc.ColumnName].ToString().Replace('\\', '/'));
                    }                        
                    else if ("EA_PATH" == dc.ColumnName)
                        bodyST_categorylist.SetAttribute(dc.ColumnName, HttpUtility.UrlEncode(objSecurity.StringEnCrypt(ddr[dc.ColumnName].ToString())));
                    else
                        bodyST_categorylist.SetAttribute(dc.ColumnName, ddr[dc.ColumnName].ToString());
                }
            }
            
        }
        int i = 1;
        foreach (DataRow dr in dsrecords.Tables[0].Rows)
        {
            if (dr["TBT_PARENT_CATEGORY_NAME"].ToString() != null && dr["TBT_PARENT_CATEGORY_NAME"].ToString() != string.Empty)
            {
               
                if (firstval != dr["TBT_PARENT_CATEGORY_NAME"].ToString().ToUpper())
                {
                    
                    //Build the category 
                    bodyST_categorylist.SetAttribute("TBT_CATEGORY_ORDER", i);
                    i++;
                    bodyST_categorylist.SetAttribute("TBT_SUB_CATEGORY_LIST", categorylist.ToString());                    
                    name.Append(bodyST_categorylist.ToString());
                    indV++; catno = 0;
                    if (indV == grid_cols)
                    {
                        bodyST = stg_records.GetInstanceOf("row");
                        bodyST.SetAttribute("TBWDataList", name);
                        lstrecords[bodyValue] = new TBWDataList(bodyST.ToString());
                        bodyValue++;
                        indV = 0;
                        name = new StringBuilder();
                    }

                    //Build the sub heading
                    categorylist = new StringBuilder();
                    bodyST_categorylist = stg_records.GetInstanceOf("cell");
                    foreach (DataColumn dc in dsrecords.Tables[0].Columns)
                    {
                        if (dc.ColumnName.ToString().Contains("CUSTOM_NUM"))
                            if (dr[dc.ColumnName].ToString().Length > 0)
                                bodyST_categorylist.SetAttribute(dc.ColumnName, Convert.ToDouble(dr[dc.ColumnName].ToString()));
                            else
                                bodyST_categorylist.SetAttribute(dc.ColumnName, "0");
                        else
                        {
                            if ("TBT_PARENT_CATEGORY_IMAGE" == dc.ColumnName)
                            {
                                if(dr[dc.ColumnName].ToString() != "")
                                    bodyST_categorylist.SetAttribute(dc.ColumnName, dr[dc.ColumnName].ToString().Replace("\\", "/"));
                                else
                                    bodyST_categorylist.SetAttribute(dc.ColumnName, "/Images/Noimage.gif");
                                //System.IO.FileInfo Fil = new System.IO.FileInfo(strFile + dr[dc.ColumnName].ToString().Replace("/", "\\"));
                                //if (Fil.Exists)
                                //{
                                //    bodyST_categorylist.SetAttribute(dc.ColumnName, dr[dc.ColumnName].ToString().Replace("\\", "/"));
                                //}
                                //else
                                //{
                                //    bodyST_categorylist.SetAttribute(dc.ColumnName, "/Images/Noimage.gif");

                                //}

                                // bodyST_categorylist.SetAttribute(dc.ColumnName, dr[dc.ColumnName].ToString().Replace('\\', '/'));
                            }
                            else if ("EA_PATH" == dc.ColumnName)
                                bodyST_categorylist.SetAttribute(dc.ColumnName, HttpUtility.UrlEncode(objSecurity.StringEnCrypt(dr[dc.ColumnName].ToString())));
                            else
                                bodyST_categorylist.SetAttribute(dc.ColumnName, dr[dc.ColumnName].ToString());
                        }
                    }                    
                    firstval = dr[1].ToString().ToUpper();
                  

                }
                //Build the Content
                if (catno < 6)
                {
                    if (dr["TBT_SHORT_DESC"].ToString().ToLower() != "not for web")
                    {
                        bodyST_list1 = stg_records.GetInstanceOf("cell1");
                        bodyST_list1.SetAttribute("TBT_CATEGORY_NAME", dr["CATEGORY_NAME"].ToString());
                        bodyST_list1.SetAttribute("TBT_CATEGORY_ID",  HttpUtility.UrlEncode(dr["CATEGORY_ID"].ToString()));
                        if (dr["TBT_CUSTOM_NUM_FIELD3"]!=System.DBNull.Value)
                        {
                            bodyST_list1.SetAttribute("TBT_CUSTOM_NUM_FIELD3", Convert.ToInt32(dr["TBT_CUSTOM_NUM_FIELD3"]).ToString());
                        }
                        else
                        {
                            bodyST_list1.SetAttribute("TBT_CUSTOM_NUM_FIELD3", "");
                        }
                        bodyST_list1.SetAttribute("TBT_PARENT_CATEGORY_ID",  HttpUtility.UrlEncode(dr["TBT_PARENT_CATEGORY_ID"].ToString()));

                        bodyST_list1.SetAttribute("TBT_BRAND", HttpUtility.UrlEncode(_tsb));
                        bodyST_list1.SetAttribute("TBT_MODEL", HttpUtility.UrlEncode(_tsm));
                        bodyST_list1.SetAttribute("TBT_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(_searchstr));
                        bodyST_list1.SetAttribute("TBT_ATTRIBUTE_TYPE","Category");
                        bodyST_list1.SetAttribute("TBT_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(dr["CATEGORY_NAME"].ToString()));
                        bodyST_list1.SetAttribute("TBT_ATTRIBUTE_BRAND",_tsb);

                        bodyST_list1.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(dr["EA_PATH"].ToString() + "////" + dr["TBT_PARENT_CATEGORY_NAME"].ToString())));                                                    
                        categorylist.Append(bodyST_list1.ToString()); catno++;
                    }
                }

            }
        }
        }
        if (dsrecords == null || dsrecords.Tables[0].Rows.Count == 0)
        {
            bodyST_categorylist = stg_records.GetInstanceOf("cell");
        }
        if (indV < grid_cols)
        {
            bodyST_categorylist.SetAttribute("TBT_CATEGORY_ORDER", "21");
            bodyST_categorylist.SetAttribute("TBT_SUB_CATEGORY_LIST", categorylist.ToString());
            name.Append(bodyST_categorylist.ToString());
            bodyST = stg_records.GetInstanceOf("row");
            bodyST.SetAttribute("TBWDataList", name);
            if (lstrecords.Length != 0)
            {
                lstrecords[bodyValue] = new TBWDataList(bodyST.ToString());
            }
            bodyValue++;
        }
        StringTemplate bodyST_main = stg_records.GetInstanceOf("main");
        bodyST_main.SetAttribute("TBWDataList", lstrecords);
        string sHtmls = bodyST_main.ToString();
        if (sHtmls.Contains("src=\"prodimages\""))
            sHtmls = sHtmls.Replace("src=\"prodimages\"", "src=\"images/noimage.gif\"");
        if (sHtmls.Contains("src=\"\""))
            sHtmls = sHtmls.Replace("src=\"\"", "src=\"images/noimage.gif\"");

        //return objHelperServices.StripWhitespace(sHtmls);
        return sHtmls;
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return string.Empty  ;
        }
        //if (dsrecords.Tables[0].Rows[0][1].ToString() != null && dsrecords.Tables[0].Rows[0][1].ToString() != string.Empty)
        //{
        //    //Build the Sub heading 
        //    firstval = dsrecords.Tables[0].Rows[0][1].ToString().ToUpper();
        //    bodyST_head = stg_records.GetInstanceOf("cell_head");
        //    bodyST_head.SetAttribute("TBT_PARENT_CATEGORY_NAME", firstval);
        //    categorylist.Append(bodyST_head.ToString());
          
        //}
        //foreach (DataRow dr in dsrecords.Tables[0].Rows)
        //{
        //    if (dr[1].ToString() != null && dr[1].ToString() != string.Empty)
        //    {
        //        if (firstval != dr[1].ToString().ToUpper())
        //        {

        //            //Build the category 
        //            bodyST_categorylist = stg_records.GetInstanceOf("cell");
        //            bodyST_categorylist.SetAttribute("TBT_SUB_CATEGORY_LIST", categorylist.ToString());
        //            name.Append(bodyST_categorylist.ToString());
        //            indV++;
        //            if (indV == grid_cols)
        //            {
        //                bodyST = stg_records.GetInstanceOf("row");
        //                bodyST.SetAttribute("TBWDataList", name);
        //                lstrecords[bodyValue] = new TBWDataList(bodyST.ToString());
        //                bodyValue++;
        //                indV = 0;
        //                name = new StringBuilder();
        //            }

        //            //Build the sub heading
        //            categorylist = new StringBuilder();
        //            bodyST_head = stg_records.GetInstanceOf("cell_head");
        //            bodyST_head.SetAttribute("TBT_PARENT_CATEGORY_NAME", dr[1].ToString().ToUpper());
        //            categorylist.Append(bodyST_head.ToString());
        //            firstval = dr[1].ToString().ToUpper();
                   
        //        }
        //        //Build the Content
        //        bodyST_list1 = stg_records.GetInstanceOf("cell1");
        //        bodyST_list1.SetAttribute(dr.Table.Columns[3].ColumnName.ToString(), dr[3].ToString());
        //        categorylist.Append(bodyST_list1.ToString());
                
        //    }
        //}      
        
        //if (indV < grid_cols)
        //{
        //    bodyST = stg_records.GetInstanceOf("row");
        //    bodyST.SetAttribute("TBWDataList", name);
        //    lstrecords[bodyValue] = new TBWDataList(bodyST.ToString());
        //    bodyValue++;            
        //}
        //StringTemplate bodyST_main = stg_records.GetInstanceOf("main");
        //bodyST_main.SetAttribute("TBWDataList", lstrecords);
        //return bodyST_main.ToString();
    }



    //private DataSet GetDataSet(string SQLQuery)
    //{
    //    DataSet ds = new DataSet();
    //    SqlDataAdapter da = new SqlDataAdapter(SQLQuery, conStr.ConnectionString.Substring(conStr.ConnectionString.IndexOf(";") + 1));
    //    da.Fill(ds, "generictable");
    //    return ds;
    //}
}
