using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using StringTemplate = Antlr3.ST.StringTemplate;  
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
//using TradingBell.Common;
//using TradingBell.WebServices;
using System.Xml;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.CommonServices;
using TradingBell.WebCat.EasyAsk ;
//using System.Diagnostics;
namespace TradingBell.WebCat.TemplateRender
{
    /*********************************** J TECH CODE ***********************************/
    public class TBWTemplateEngine
    {
        /*********************************** DECLARATION ***********************************/
        //Stopwatch stopwatch = new Stopwatch();
        EasyAsk_WES EasyAsk = new EasyAsk_WES();
        ErrorHandler objErrorHandler = new ErrorHandler();
        CategoryServices objCategoryServices = new CategoryServices();
        OrderServices objOrderServices = new OrderServices();
        HelperServices objHelperServices = new HelperServices();
        FamilyServices objFamilyServices = new FamilyServices();
        UserServices objUserServices = new UserServices();
        ProductServices objProductServices = new ProductServices();
        Dictionary<string, TBWDataList[]> _dict_inner_html = new Dictionary<string, TBWDataList[]>();

        private StringTemplateGroup _stg_container = null;
        private StringTemplateGroup _stg_records = null;
        private StringTemplate _stmpl_container = null;
        private StringTemplate _stmpl_records = null;

        private string _SkinRootPath = null;
        private string _SkinRoot_container = null;
        private string _SkinRoot_records = null;

        private string _RenderedHTML = null; //for this use the big string variable
        private string _DBConnectionString = null;
        private string _Package = null;

        //Get the following from TBWC_PACKAGE table 
        private string _skin_container = null;
        private int _grid_cols = 1;
        private int _grid_rows = 1;
        private string _skin_sql_container = null;
        private string _skin_sql_type_container = null;
        private string _skin_sql_param_container = null;
        private int _package_order = 0;
        private string _skin_body_attribute = null;

        private string _skin_records = null;
        private string _skin_sql_records = null;
        private string _skin_sql_type_records = null;
        private string _skin_sql_param_records = null;
        private DataSet _GDataSet = null;
        public string paraValue = string.Empty;
        public string paraPID = string.Empty;
        public string paraFID = string.Empty;
        public string paraCID = string.Empty;
        private string _cartitem = string.Empty;
        private string _CATALOG_ID = string.Empty;
        private HelperServices objHelperService = new HelperServices();
        private Security objSecurity = new Security();
        private HelperDB objHelperDB = new HelperDB();
        private ConnectionDB objConnectionDB = new ConnectionDB();
        string _fid = string.Empty;
        string downloadST = string.Empty;
        bool isdownload=false;
        public string WesNewsCategoryId = System.Configuration.ConfigurationManager.AppSettings["WESNEWS_CATEGORY_ID"].ToString();
        public string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();
        string strProdImages = HttpContext.Current.Server.MapPath("ProdImages");
        //END TBWC_PACKAGE table info

        public string cartitem
        {
            get
            {
                return _cartitem;
            }
            set
            {
                _cartitem = value;
            }
        }

        public string CATALOG_ID
        {
            get
            {
                return _CATALOG_ID;
            }
            set
            {
                _CATALOG_ID = value;
            }
        }

        public string DBConnectionString
        {
            get
            {
                return _DBConnectionString;
            }
            set
            {
                _DBConnectionString = value;
            }
        }

        public string Package
        {
            get
            {
                return _Package;
            }
            set
            {
                _Package = value;
            }
        }

        public string SkinRootPath
        {
            get
            {
                return _SkinRootPath;
            }
            set
            {
                _SkinRootPath = value;
            }
        }

        public string RenderedHTML
        {
            get
            {
               // return objHelperService.StripWhitespace(_RenderedHTML);
                return _RenderedHTML;
            }
        }

        public DataSet GDataSet
        {
            get
            {
                return _GDataSet;
            }
            set
            {
                _GDataSet = value;
            }
        }
        /*********************************** DECLARATION ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE PACKAGE,SKIN NAME AND DATABASE CONNECTION STRING  DETAILS ***/
        /********************************************************************************/
        public TBWTemplateEngine(string Package, string SkinRootPath, string DBConnectionString)
        {
            _Package = Package;
            _SkinRootPath = SkinRootPath;
            //_DBConnectionString = DBConnectionString.Substring(DBConnectionString.IndexOf(';') + 1);
            //DataSet ds = new DataSet();
            //SqlDataAdapter da = new SqlDataAdapter("select convert(int, option_value) as OPTION_VALUE from tbwc_store_options where option_name = 'DEFAULT CATALOG'", _DBConnectionString);
            //da.Fill(ds, "generictable");
            //_CATALOG_ID = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            _CATALOG_ID = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();

        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK HTML TEMPLATE PAGE IS GENERATED OR NOT  ***/
        /********************************************************************************/
        public bool RenderHTML(string rType)
        {
            bool _status = false;
            string _sqlpkginfo;
            
            //_sqlpkginfo = " SELECT * FROM TBW_PACKAGE ";
            //_sqlpkginfo = _sqlpkginfo + " WHERE IS_ROOT = 0 AND PACKAGE_NAME = '" + _Package + "' ORDER BY PROCESS_ORDER ASC ";

            DataSet dspkg = new DataSet();
            try
            {
                //dspkg = GetDataSet(_sqlpkginfo);
                dspkg = (DataSet)objHelperDB.GetGenericDataDB(_Package, "GET_PACKAGE", HelperDB.ReturnType.RTDataSet);
                if (dspkg != null)
                {
                    if (dspkg.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dspkg.Tables[0].Rows)
                        {
                            _skin_container = dr["SKIN_NAME"].ToString();
                            _grid_cols = Convert.ToInt32(dr["GRID_COLS"]);
                            _grid_rows = Convert.ToInt32(dr["GRID_ROWS"]);
                            _skin_sql_container = dr["SKIN_SQL"].ToString();
                            _skin_sql_type_container = dr["SKIN_SQL_TYPE"].ToString();
                            _skin_sql_param_container = dr["SKIN_SQL_PARAM"].ToString();
                            _package_order = Convert.ToInt32(dr["PROCESS_ORDER"]);
                            _skin_body_attribute = dr["SKIN_BODY_ATTRIBUTE"].ToString();
                            _skin_records = dr["LIST_SKIN_NAME"].ToString();
                            _skin_sql_records = dr["LIST_SKIN_SQL"].ToString();
                            _skin_sql_type_records = dr["LIST_SKIN_SQL_TYPE"].ToString();
                            _skin_sql_param_records = dr["LIST_SKIN_SQL_PARAM"].ToString();

                            if (rType == "Column")
                                BuildRecordsTemplateColumn();
                            else
                                BuildRecordsTemplateRow();
                        }
                    }
                }

                if (BuildMainContainer())
                {
                    _RenderedHTML = _stmpl_container.ToString().Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");


                    if (_Package == "NEWPRODUCT" || _Package == "CATEGORYLISTIMG" || _Package == "CSFAMILYPAGE" || _Package == "CSFAMILYPAGEWITHSUBFAMILY" || _Package == "PRODUCT" || _Package == "NEWPRODUCTNAV" ) //|| _Package == "CSFAMILYPAGE" || _Package == "CSFAMILYPAGEWITHSUBFAMILY" || _Package == "PRODUCT"
                    {

                        if (_stmpl_container.ToString().Contains("data-original=\"prodimages\""))
                        {
                            if (_Package == "CSFAMILYPAGE")
                                _RenderedHTML = _stmpl_container.ToString().Replace("data-original=\"prodimages\"", "data-original=\"images/noimage.gif\"");
                            else
                                _RenderedHTML = _stmpl_container.ToString().Replace("data-original=\"prodimages\"", "data-original=\"images/noimage.gif\"");
                            _RenderedHTML = _RenderedHTML.Replace("href=\"javascript:Zoom('prodimages')\"", "href=\"#\"");
                        }
                        if (_stmpl_container.ToString().Contains("data-original=\"\""))
                        {
                            _RenderedHTML = _stmpl_container.ToString().Replace("data-original=\"\"", "data-original=\"images/noimage.gif\"");
                            _RenderedHTML = _RenderedHTML.Replace("href=\"javascript:Zoom('')\"", "href=\"#\"");
                        }
                    }
                    else
                    {
                        if (_stmpl_container.ToString().Contains("src=\"prodimages\""))
                        {
                            if (_Package == "CSFAMILYPAGE")
                                _RenderedHTML = _stmpl_container.ToString().Replace("src=\"prodimages\"", "src=\"images/noimage.gif\" style=\"display:none;\"");
                            else
                                _RenderedHTML = _stmpl_container.ToString().Replace("src=\"prodimages\"", "src=\"images/noimage.gif\"");
                            _RenderedHTML = _RenderedHTML.Replace("href=\"javascript:Zoom('prodimages')\"", "href=\"#\"");
                        }
                        if (_stmpl_container.ToString().Contains("src=\"\""))
                        {
                            _RenderedHTML = _stmpl_container.ToString().Replace("src=\"\"", "src=\"images/noimage.gif\"");
                            _RenderedHTML = _RenderedHTML.Replace("href=\"javascript:Zoom('')\"", "href=\"#\"");
                        }
                    }

                    /*DataSet DSnaprod = new DataSet();
                    DSnaprod = GetDataSet("SELECT PRODUCT_ID,QTY_AVAIL,MIN_ORD_QTY FROM TBWC_INVENTORY WHERE PRODUCT_STATUS <> 'AVAILABLE'");
                    foreach (DataRow DDR in DSnaprod.Tables[0].Rows)
                    {
                        if (_RenderedHTML.Contains("<img src=\"images/but_buy1.gif\" name=\"Image" + DDR["PRODUCT_ID"].ToString() + "_"))
                            _RenderedHTML = _RenderedHTML.Replace("<img src=\"images/but_buy1.gif\" name=\"Image" + DDR["PRODUCT_ID"].ToString() + "_", " <img src=\"images/but_buy1.gif\" style=\"display:none\" name=\"Image" + DDR["PRODUCT_ID"].ToString() + "_");

                        if (_RenderedHTML.Contains("<img src=\"images/but_buyitem1.gif\" name=\"Image" + DDR["PRODUCT_ID"].ToString() + "_"))
                            _RenderedHTML = _RenderedHTML.Replace("<img src=\"images/but_buyitem1.gif\" name=\"Image" + DDR["PRODUCT_ID"].ToString() + "_", " <img src=\"images/but_buyitem1.gif\" style=\"display:none\" name=\"Image" + DDR["PRODUCT_ID"].ToString() + "_");

                        if (_RenderedHTML.Contains("<strong>QTY</strong><input name=\"txt" + DDR["PRODUCT_ID"].ToString() + "\" style=\"background-color:#FFFFFF;color: #000000;\""))
                            _RenderedHTML = _RenderedHTML.Replace("<strong>QTY</strong><input name=\"txt" + DDR["PRODUCT_ID"].ToString() + "\" style=\"background-color:#FFFFFF;color: #000000;\"", "<strong style=\" display:none\">QTY</strong><input name=\"txt" + DDR["PRODUCT_ID"].ToString() + "\" style=\" display:none\"");

                        if (_RenderedHTML.Contains("<strong>QTY</strong><input name=\"txt" + DDR["PRODUCT_ID"].ToString() + "_" + DDR["QTY_AVAIL"].ToString() + "_" + DDR["MIN_ORD_QTY"].ToString() + "\" style=\"background-color:#FFFFFF;color: #000000;\" size=\"5\" type=\"text\" id=\"txt" + DDR["PRODUCT_ID"].ToString() + "_" + DDR["QTY_AVAIL"].ToString() + "_" + DDR["MIN_ORD_QTY"].ToString() + "\"/>"))
                            _RenderedHTML = _RenderedHTML.Replace("<strong>QTY</strong><input name=\"txt" + DDR["PRODUCT_ID"].ToString() + "_" + DDR["QTY_AVAIL"].ToString() + "_" + DDR["MIN_ORD_QTY"].ToString() + "\" style=\"background-color:#FFFFFF;color: #000000;\" size=\"5\" type=\"text\" id=\"txt" + DDR["PRODUCT_ID"].ToString() + "_" + DDR["QTY_AVAIL"].ToString() + "_" + DDR["MIN_ORD_QTY"].ToString() + "\"/>", "&nbsp;");

                    }*/
                }
                _status = true;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                _status = false;
            }
           
            return _status;
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK WEATHER THE MAIN CONTENT IS GET BUILD OR NOT  ***/
        /********************************************************************************/
        private bool BuildMainContainer()
        {
            bool _status = false;
            string strFile = HttpContext.Current.Server.MapPath("ProdImages");
            string strPDFFiles1 = HttpContext.Current.Server.MapPath("attachments");
            string strPDFFiles2 = HttpContext.Current.Server.MapPath("News update");
            string _sqlpkginfo;
            //_sqlpkginfo = " SELECT TOP 1 * FROM TBW_PACKAGE ";
            //_sqlpkginfo = _sqlpkginfo + " WHERE IS_ROOT = 1 AND PACKAGE_NAME = '" + _Package + "' ORDER BY PROCESS_ORDER ASC ";

            DataSet dspkg = new DataSet();
            try
            {
                //dspkg = GetDataSet(_sqlpkginfo);
                dspkg = (DataSet)objHelperDB.GetGenericDataDB(_Package, "GET_MAIN_PACKAGE", HelperDB.ReturnType.RTDataSet);
                if (dspkg != null)
                {
                    if (dspkg.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dspkg.Tables[0].Rows)
                        {
                            _skin_container = dr["SKIN_NAME"].ToString();
                            _grid_cols = Convert.ToInt32(dr["GRID_COLS"]);
                            _grid_rows = Convert.ToInt32(dr["GRID_ROWS"]);
                            _skin_sql_container = dr["SKIN_SQL"].ToString();
                            _skin_sql_type_container = dr["SKIN_SQL_TYPE"].ToString();
                            _skin_sql_param_container = dr["SKIN_SQL_PARAM"].ToString();
                            _package_order = Convert.ToInt32(dr["PROCESS_ORDER"]);
                            _skin_body_attribute = dr["SKIN_BODY_ATTRIBUTE"].ToString();
                        }
                    }
                }


                if (_Package == "CSFAMILYPAGEWITHSUBFAMILY_PRINT")
                {
                    _skin_container = "main_print";
                }
                //Build the outer body of the HTML - for main container

                if (_skin_container == "" || _skin_container == null || _SkinRootPath == "" || _SkinRootPath == null)
                {
                    return false;
                }

                _stg_container = new StringTemplateGroup(_skin_container, _SkinRootPath);
                DataSet dscontainer = null;
                if (_GDataSet != null && _GDataSet.Tables.Count > 1)
                {
                    dscontainer = _GDataSet;
                }
                else if (_Package == "CSFAMILYPAGE")
                {
                    dscontainer = (DataSet)HttpContext.Current.Session["FamilyProduct"];
                }
                else if (_Package == "CSFAMILYPAGEWITHSUBFAMILY")
                {
                    dscontainer = null;
                    DataSet tempDs = (DataSet)HttpContext.Current.Session["FamilyProduct"];
                   if (tempDs != null && tempDs.Tables["SubFamily"] != null)
                   
                    {
                        DataRow[] dr = tempDs.Tables["SubFamily"].Select("FAMILY_ID='" + paraValue + "'");
                        //DataRow[] dr = tempDs.Tables["FamilyPro"].Select("FAMILY_ID='" + paraValue + "'");
                        if (dr.Length > 0)
                        {
                            dscontainer = new DataSet();
                            dscontainer.Tables.Add(dr.CopyToDataTable().Copy());
                        }

                    }
                }
                else if (_Package == "CSFAMILYPAGEWITHSUBFAMILY_PRINT")
                {
                    dscontainer = null;
                    DataSet tempDs = (DataSet)HttpContext.Current.Session["FamilyProduct"];
                   // if (tempDs != null && tempDs.Tables["SubFamily"] != null)
                    if (tempDs != null && tempDs.Tables["FamilyPro"] != null)
                    {
                       // DataRow[] dr = tempDs.Tables["SubFamily"].Select("FAMILY_ID='" + paraValue + "'");
                        DataRow[] dr = tempDs.Tables["FamilyPro"].Select("FAMILY_ID='" + paraValue + "'");
                        if (dr.Length > 0)
                        {
                            dscontainer = new DataSet();
                            dscontainer.Tables.Add(dr.CopyToDataTable().Copy());
                        }

                    }
                }
                else
                {
                    dscontainer = GetDataSet(_skin_sql_container, _skin_sql_type_container, _skin_sql_param_container);
                }
                if (dscontainer != null)
                {
                    if (dscontainer.Tables[0].Rows.Count > 0)
                    {
                        string pkgskincontainer = _Package + "\\" + _skin_container;
                        foreach (DataRow dr in dscontainer.Tables[0].Rows)
                        {
                            if (_Package == "CSFAMILYPAGE")
                            {
                                if (dr["STATUS"].ToString().ToUpper() == "TRUE")
                                    _stmpl_container = _stg_container.GetInstanceOf(pkgskincontainer + "1");
                                else if (dr["STATUS"].ToString().ToUpper() == "FALSE")
                                    _stmpl_container = _stg_container.GetInstanceOf(pkgskincontainer);
                                else
                                    _stmpl_container = _stg_container.GetInstanceOf(pkgskincontainer + "2");

                            }
                            else

                                _stmpl_container = _stg_container.GetInstanceOf(pkgskincontainer);
                            if (HttpContext.Current.Session["USER_ID"]== null || HttpContext.Current.Session["USER_ID"] == "")
                            {
                            }
                            else
                            {

                                if (_Package == "BOTTOM")
                                {
                                    if (HttpContext.Current.Session["CUSTOMER_TYPE"] != null && HttpContext.Current.Session["CUSTOMER_TYPE"].ToString() == "Retailer")
                                        _stmpl_container.SetAttribute("TBT_RET_PRO", false);
                                    else
                                        _stmpl_container.SetAttribute("TBT_RET_PRO", true);
                                }
                            }
                                

                            if (_Package == "CSFAMILYPAGE")
                            {
                                
                                DataSet tempdscat = (DataSet)HttpContext.Current.Session["LHSAttributes"];
                                if (tempdscat != null && tempdscat.Tables.Count > 0)
                                {
                                    if (tempdscat.Tables.Count == 1 && tempdscat.Tables[0].Rows.Count > 0)
                                        _stmpl_container.SetAttribute("TBT_LHS_TIP", true);
                                    else if (tempdscat.Tables.Count > 1 && tempdscat.Tables[1].Rows.Count > 0)
                                        _stmpl_container.SetAttribute("TBT_LHS_TIP", true);
                                    else
                                        _stmpl_container.SetAttribute("TBT_LHS_TIP", false);
                                }
                                else
                                    _stmpl_container.SetAttribute("TBT_LHS_TIP",false);


                                if (HttpContext.Current.Request.QueryString["fid"] != null)
                                {
                                    _fid = HttpContext.Current.Request.QueryString["fid"].ToString();

                               
                                    //new code for family cloning
                                    string eapath = EasyAsk.GetFamilyEAPATH();
                                    if (eapath != "")
                                    {
                                        string[] strs = eapath.Split(new string[] { "////" }, StringSplitOptions.None);
                                        if (strs.Length > 0)
                                        {
                                            DataSet tmpds = objCategoryServices.GetCategotyID(strs[strs.Length - 1].ToString());
                                            if (tmpds != null && tmpds.Tables.Count > 0)
                                                _stmpl_container.SetAttribute("TBT_CATEGORY_ID", tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString());

                                        }
                                        eapath = "AllProducts////WESAUSTRALASIA" + eapath;
                                        _stmpl_container.SetAttribute("TBT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath)));
                                    }
                                    else
                                    {
                                        DataSet tmpds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, _fid, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);

                                        if (tmpds != null && tmpds.Tables.Count > 0)
                                        {
                                            _stmpl_container.SetAttribute("TBT_CATEGORY_ID", tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString());
                                             eapath = "AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString();
                                            _stmpl_container.SetAttribute("TBT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath)));
                                        }
                                    }
                                }
                               
                            }

                            _stmpl_container.SetAttribute("TBT_ECOMENABLED", IsEcomenabled());

                           

                            _stmpl_container.SetAttribute("TBT_LOGIN_NAME", GetLoginName());

                             if (HttpContext.Current.Session["CUSTOMER_TYPE"] != null && HttpContext.Current.Session["CUSTOMER_TYPE"].ToString() == "Retailer")
                            {
                                if (HttpContext.Current.Session["USER_ROLE"] != null && Convert.ToInt32(HttpContext.Current.Session["USER_ROLE"].ToString())==4)
                                {
                                    string ReMailLink = "<a Href=ConfirmMessage.aspx?Result=REMAILACTIVATION class=\"toplinkatest\">Re-Email Activation Link Now</a>";

                                  _stmpl_container.SetAttribute("TBT_LOGIN_NAME", " Your Account Has Not Been Activated! " + ReMailLink);                                              
                                }
                                else
                                _stmpl_container.SetAttribute("TBT_COMPANY_NAME", "");
                            }
                            else
                            {
                                _stmpl_container.SetAttribute("TBT_COMPANY_NAME", GetCompanyName() + ",");
                            }

                            
                            HttpContext.Current.Session["LOGIN_NAME"] = GetLoginName();

                            foreach (DataColumn dc in dr.Table.Columns)
                            {
                                if (_Package == "CSFAMILYPAGEWITHSUBFAMILY_PRINT")
                                {
                                    string fam_th_image = string.Empty;
                                    FileInfo Fil;

                                    if (dc.ColumnName.ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper() == "FAMILY_TH_IMAGE")
                                    {
                                        fam_th_image = dr[dc.ColumnName.ToString()].ToString().Replace("\r", "&nbsp;").Replace("\n", " ");
                                        Fil = new FileInfo(strFile + fam_th_image);
                                        if (Fil.Exists)
                                        {
                                            _stmpl_container.SetAttribute("TBT_" + dc.ColumnName.ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr[dc.ColumnName.ToString()].ToString().Replace("\r", "&nbsp;").Replace("\n", " "));
                                        }
                                        else
                                            _stmpl_container.SetAttribute("TBT_" + dc.ColumnName.ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), "/images/noimage.gif");

                                    }
                                    else
                                    {
                                        _stmpl_container.SetAttribute("TBT_" + dc.ColumnName.ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr[dc.ColumnName.ToString()].ToString().Replace("\r", "&nbsp;").Replace("\n", " "));
                                    }
                                }
                                else
                                    _stmpl_container.SetAttribute("TBT_" + dc.ColumnName.ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr[dc.ColumnName.ToString()].ToString().Replace("\r", "&nbsp;").Replace("\n", " "));
                                
                                
                               

                               
                            }
                            // code by palani
                             if (_Package == "")
                            {
                                if (HttpContext.Current.Request.QueryString["tsm"] == null && HttpContext.Current.Request.QueryString["Path"] == null && HttpContext.Current.Request.QueryString["tsb"] == null)
                                {
                                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\Tsb");
                                }
                                else if (HttpContext.Current.Request.QueryString["tsm"] != null && HttpContext.Current.Request.QueryString["tsm"] != "" && HttpContext.Current.Request.QueryString["tsb"] != null)
                                {
                                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\Tsb");
                                }
                            }
                            //*********************
                        }

                        
                        if (dscontainer.Tables[0].Columns.Contains("attribute_name"))
                        {
                            string descall = string.Empty;
                           // string descalltrim = "";
                            string desc1 = string.Empty;
                            string descallstring = string.Empty; ;
                            string Att_name = string.Empty;
                            foreach (DataRow dr in dscontainer.Tables[0].Rows)
                            {
                                desc1 = "";
                                if (dr["ATTRIBUTE_TYPE"].ToString() == "3" || dr["ATTRIBUTE_TYPE"].ToString() == "9")
                                {
                                    FileInfo Fil;
                                    if (_Package == "CSFAMILYPAGE")
                                    {
                                        Fil = new FileInfo(strFile + dr["STRING_VALUE"].ToString().Replace("_TH", "_Images_200"));
                                    }
                                    else
                                    {
                                        Fil = new FileInfo(strFile + dr["STRING_VALUE"].ToString());
                                    }
                                    if (Fil.Exists)
                                    {
                                        if (_Package == "CSFAMILYPAGE")
                                        {
                                            //_stmpl_container.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"].ToString().Replace("_TH", "_Images_200"));
                                        }
                                        else
                                            _stmpl_container.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));
                                    }
                                    else
                                    {
                                        _stmpl_container.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), "");
                                    }

                                }
                                else
                                {
                                   // 
                                    Att_name = dr["ATTRIBUTE_NAME"].ToString().ToUpper();

                                    if (_Package == "CSFAMILYPAGE")
                                    {
                                        if (Att_name == "DESCRIPTIONS" || Att_name == "FEATURES" || Att_name == "SPECIFICATION" || Att_name == "SPECIFICATIONS" || Att_name == "APPLICATIONS" || Att_name == "NOTES")
                                        {
                                            desc1 = dr["STRING_VALUE"].ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                                            desc1 = desc1.ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;");
                                        }
                                        else
                                            _stmpl_container.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"].ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;"));
                                    }
                                    else
                                    {
                                        _stmpl_container.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"].ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;"));

                                    }
                                    //if (Att_name == "short description" || Att_name == "short description1" || Att_name == "note" || Att_name == "Notes" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "APPLICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATION" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "DESCRIPTIONS")
                                    //{

                                    //    desc1 = dr["STRING_VALUE"].ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                                    //    _stmpl_container.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"].ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;"));
                                    //   // _stmpl_container.SetAttribute("TBT_MORE", desc1);
                                        
                                        
                                    //}
                                    //else if (dr["ATTRIBUTE_NAME"].ToString() == "Descriptions1" || dr["ATTRIBUTE_NAME"].ToString() == "DescriptionTemp")
                                    //{
                                    //    desc1 = dr["STRING_VALUE"].ToString().Replace("*", "").Replace("\n", "<br/>").Replace("\r", "&nbsp;").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                                    //}
                                    //else
                                    //{
                                    //    _stmpl_container.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"].ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;"));
                                    //}
                                }
                               
                               // if (dr["ATTRIBUTE_NAME"].ToString() == "Short Description" || dr["ATTRIBUTE_NAME"].ToString() == "Short Description1" || dr["ATTRIBUTE_NAME"].ToString() == "Note" || dr["ATTRIBUTE_NAME"].ToString() == "Notes" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "APPLICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATION" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "DESCRIPTIONS" || dr["ATTRIBUTE_NAME"].ToString() == "Descriptions1" || dr["ATTRIBUTE_NAME"].ToString() == "DescriptionTemp")
                                if (_Package == "CSFAMILYPAGE")
                                {                                   
                                        if (desc1 != "")
                                            descall = descall + desc1 + "<br/><br/>";                                  
                                }
                             
                            }
                            if (_Package == "CSFAMILYPAGE")
                            {
                            if (descall != "")
                            {
                                descall = descall.Trim();
                                descall = descall.Substring(0, descall.Length - 5);
                            }
                            
                          
                            if (descall.Length > 1080)
                            {
                              //  _stmpl_records.SetAttribute("TBT_MORE_SHOW", true);
                                descallstring = descall.Substring(0, 1080).ToString();
                                _stmpl_container.SetAttribute("TBT_MORE", descallstring);
                                _stmpl_container.SetAttribute("TBT_MENU_ID", "2");
                               descall = descall.Substring(0,descall.Length).ToString();
                               // descall = descall.Substring(300).ToString();

                                _stmpl_container.SetAttribute("TBT_DESCALL", descall);

                            }
                            else
                            {
                                _stmpl_container.SetAttribute("TBT_DESCALL", descall);
                                _stmpl_container.SetAttribute("TBT_MENU_ID", "2");
                               // _stmpl_container.SetAttribute("TBT_MORE_SHOW", false);
                                _stmpl_container.SetAttribute("TBT_MORE", descall);
                            }
                            if (descall.Length > 1080)
                                _stmpl_container.SetAttribute("TBT_MORE_SHOW", true);
                            else
                                _stmpl_container.SetAttribute("TBT_MORE_SHOW", false);

                            
                            }
                            
                        }

                    }
                }
                if (_Package == "ADVERTISEMENT")
                {
                   DataSet dsbannerlink = new DataSet();
                    string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                    dsbannerlink = (DataSet)objHelperDB.GetGenericDataDB("", "GET_BANNER_LINK", HelperDB.ReturnType.RTDataSet);
                    string Banner5_Link = string.Empty;
                     if (dsbannerlink != null && dsbannerlink.Tables[0].Rows.Count > 0)
                    {
                        Banner5_Link = dsbannerlink.Tables[0].Rows[0]["BANNER5_LINK_NEW"].ToString();
                        _stmpl_container.SetAttribute("TBT_RHS_LINK", Banner5_Link);
                    }
                }
                if (_Package == "BROWSEBYCATEGORY" || _Package == "BROWSEBYBRAND" || _Package == "BROWSEBYPRODUCT")
                {
                    /*string bbvalue = "", catName = ""; int recvalue = 0; string cidvalue = "";
                    DataSet DSDR = null;
                    catName = paraValue;
                    DSDR = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catName + "'");
                    if (DSDR != null && DSDR.Tables[0].Rows.Count>=1   )
                    {
                        while (DSDR.Tables[0].Rows[0].ItemArray[1].ToString() != "0")
                        {
                            DSDR = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + DSDR.Tables[0].Rows[0].ItemArray[1].ToString() + "'");
                        }
                        bbvalue = "<h3 class=\"headerbar\"> " + DSDR.Tables[0].Rows[0]["CATEGORY_NAME"].ToString() + "</h3>";
                        cidvalue = DSDR.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                        #region comments
                        ////do
                        //{
                        //    DSDR = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + paraValue + "'");
                        //    foreach (DataRow DR in DSDR.Tables[0].Rows)
                        //    {
                        //        if (DR["PARENT_CATEGORY"].ToString() == "0")
                        //            if (recvalue > 0)
                        //            {
                        //                bbvalue = "<h3 class=\"headerbar\"><img src=\"images/ico_menu_back.gif\" width=\"15\" height=\"15\" align=\"absmiddle\"> " + DR["CATEGORY_NAME"].ToString() + "</h3>" + bbvalue;
                        //                catName = DR["CATEGORY_NAME"].ToString();
                        //            }
                        //            else
                        //            {
                        //                bbvalue = "<h3 class=\"headerbar\">&raquo; " + DR["CATEGORY_NAME"].ToString() + "</h3>" + bbvalue;
                        //                catName = DR["CATEGORY_NAME"].ToString();
                        //            }
                        //        else
                        //        {
                        //            bbvalue = "<h3 class=\"headerbar\"><img src=\"images/ico_menu_back.gif\" width=\"15\" height=\"15\" align=\"absmiddle\"> " + DR["CATEGORY_NAME"].ToString() + "</h3>" + bbvalue;
                        //            catName = DR["CATEGORY_NAME"].ToString();
                        //        }
                        //        //if (DR["PARENT_CATEGORY"].ToString() == "0")
                        //        //    if(recvalue >0)
                        //        //        bbvalue = "<h3 class=\"headerbar\"><img src=\"images/ico_menu_back.gif\" width=\"15\" height=\"15\" align=\"absmiddle\"> " + DR["CATEGORY_NAME"].ToString() + "</h3>" + bbvalue;
                        //        //    else
                        //        //        bbvalue= "<h3 class=\"headerbar\">&raquo; " + DR["CATEGORY_NAME"].ToString() + "</h3>" + bbvalue;
                        //        //else
                        //        //    bbvalue = "<h3 class=\"headerbar2\">" + DR["CATEGORY_NAME"].ToString() + "</h3>" + bbvalue;
                        //    }
                        //    paraValue = DSDR.Tables[0].Rows[0].ItemArray[1].ToString(); recvalue++;
                        //    DSDR = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + paraValue + "'");
                        //    foreach (DataRow DR in DSDR.Tables[0].Rows)
                        //    {
                        //        string Convalue = "product_list";
                        //        if (_Package == "BROWSEBYBRAND")
                        //            Convalue = "bybrand";
                        //        if (_Package == "BROWSEBYPRODUCT")
                        //            Convalue = "byproduct";

                        //        if (catName.ToLower() == "brand" || catName.ToLower() == "product")
                        //        {
                        //            bbvalue = "<h3 class=\"headerbar\">&raquo; " + catName + "</h3>";

                        //         //   bbvalue = "<h3 class=\"headerbar\"><img src=\"images/ico_menu_back.gif\" width=\"15\" height=\"15\" align=\"absmiddle\"> " + catName + "</h3>";
                        //        }
                        //        else
                        //            bbvalue = "<h3 class=\"headerbar\"><A HREF=\"" + Convalue + ".aspx?&ld=0&&cid=" + DR["CATEGORY_ID"].ToString() + "\"><img src=\"images/ico_menu_back.gif\" width=\"15\" height=\"15\" align=\"absmiddle\"> " + catName + "</a></h3>";
                        //        //bbvalue = bbvalue.Replace(catName, "<A HREF=\"product_list.aspx?&ld=0&&cid=" + DR["CATEGORY_ID"].ToString() + "\">" + catName + "</a>");                            
                        //    }
                        //}//while(DSDR.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
                        #endregion
                        _stmpl_container.SetAttribute("TBT_SELECTED_CATEGORY_NAME", bbvalue);
                        _stmpl_container.SetAttribute("TBT_SELECTED_CATEGORY_ID", cidvalue);
                    }
                    else
                    {
                        _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + _skin_container);
                        _stmpl_container.SetAttribute("TBT_SELECTED_CATEGORY_NAME", "");
                        _stmpl_container.SetAttribute("TBT_SELECTED_CATEGORY_ID", "");
                    }*/
                }
                _stmpl_container.SetAttribute("TBW_CART_ITEM", cartitem);
                //Lets get the block inner elements from the dictionary and finish building the main container

                if (_Package == "CATEGORYLISTIMG")
                {
                    DataSet DSDR = new DataSet();
                    //DSDR = GetDataSet("SELECT CATEGORY_ID,CATEGORY_NAME,IMAGE_FILE,SHORT_DESC,IMAGE_FILE2 FROM TB_CATEGORY WHERE CATEGORY_ID = '" + paraValue.ToString() + "'");
                    DataRow[] row = EasyAsk.GetCategoryAndBrand("MainCategory").Tables[0].Select("CATEGORY_ID='" + paraValue.ToString() + "'");
                    if (row.Length > 0)
                    {
                        DSDR.Tables.Add(row.CopyToDataTable());
                    }


                    if (DSDR != null && DSDR.Tables.Count>0)
                    {
                    

                            
                        foreach (DataRow _dsrow in DSDR.Tables[0].Rows)
                        {
                            _stmpl_container.SetAttribute("TBT_CATEGORY_NAME", _dsrow["CATEGORY_NAME"].ToString());
                            _stmpl_container.SetAttribute("TBT_CATEGORY_ID", _dsrow["CATEGORY_ID"].ToString());
                            _stmpl_container.SetAttribute("TBT_SHORT_DESC", _dsrow["SHORT_DESC"].ToString());
                            FileInfo Fil = new FileInfo(strFile + _dsrow["IMAGE_FILE"].ToString());
                            if (Fil.Exists)
                            {
                                _stmpl_container.SetAttribute("TBT_IMAGE_FILE1", _dsrow["IMAGE_FILE"].ToString().Replace("\\", "/"));
                            }
                            else
                            {
                                _stmpl_container.SetAttribute("TBT_IMAGE_FILE1", "");
                            }

                            /* For Category PDF file attachment (checking)  */
                            FileInfo Fil2 = new FileInfo(strPDFFiles1 + _dsrow["IMAGE_FILE2"].ToString());
                            FileInfo Fil3 = new FileInfo(strPDFFiles2 + _dsrow["IMAGE_FILE2"].ToString());
                            if (Fil2.Exists || Fil3.Exists)
                            {
                                _stmpl_container.SetAttribute("TBT_PDF_STATUS", true);
                            }
                            else
                            {
                                _stmpl_container.SetAttribute("TBT_PDF_STATUS", false);
                            }

                        }
                    }

                }

                string _Tbt_Order_Id = string.Empty;
                string _Tbt_Ship_URL = string.Empty;
                int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;

                if (HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0)
                {
                    _Tbt_Order_Id = HttpContext.Current.Session["ORDER_ID"].ToString();
                }
                else
                {
                    _Tbt_Order_Id = objOrderServices.GetOrderID(objHelperServices.CI(HttpContext.Current.Session["USER_ID"]), OpenOrdStatusID).ToString();
                }
               // Modified by:Indu:For back button problem
     //           if (HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0)
     //           {

     //               int orderid = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]);
     //               string orderdstatus = string.Empty;
     //               orderdstatus = objOrderServices.GetOrderStatus(orderid);
     //               //Modified by:Indu
     //               //for back button prb
     //               if ((orderdstatus == "OPEN" || orderdstatus == "CAU_PENDING"))
     //               {
     //                   _Tbt_Order_Id = HttpContext.Current.Session["ORDER_ID"].ToString();

     //               }
     //               else
     //               {
     //                   if (
     //                   HttpContext.Current.Request.Url.ToString().ToUpper().Contains("PRODUCT_LIST") ||
     //HttpContext.Current.Request.Url.ToString().ToUpper().Contains("BYBRAND") ||
     //        HttpContext.Current.Request.Url.ToString().ToUpper().Contains("POWERSEARCH") ||
     //        HttpContext.Current.Request.Url.ToString().ToUpper().Contains("FAMILY") ||
     //         HttpContext.Current.Request.Url.ToString().ToUpper().Contains("PRODUCTDETAILS") ||
     //          HttpContext.Current.Request.Url.ToString().ToUpper().Contains("ORDERDETAILS")||
     //                       HttpContext.Current.Request.Url.ToString().ToUpper().Contains("CATEGORYLIST"))
     //                   {
     //                       _Tbt_Order_Id = objOrderServices.GetOrderID(objHelperServices.CI(HttpContext.Current.Session["USER_ID"]), OpenOrdStatusID).ToString();
     //                       HttpContext.Current.Session["ORDER_ID"] = _Tbt_Order_Id;
     //                   }
     //                   else
     //                   {
     //                       _Tbt_Order_Id = HttpContext.Current.Session["ORDER_ID"].ToString();
     //                   }
     //               }
     //           }
     //           else
     //           {
     //               _Tbt_Order_Id = objOrderServices.GetOrderID(objHelperServices.CI(HttpContext.Current.Session["USER_ID"]), OpenOrdStatusID).ToString();
     //           }
                //End

                if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ViewOrder"] != null && HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")) || (HttpContext.Current.Request.QueryString["ApproveOrder"] != null && HttpContext.Current.Request.QueryString["ApproveOrder"].Equals("Approve")))                
                {
                    _Tbt_Ship_URL = "shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
                }
                else
                {
                    _Tbt_Ship_URL = "shipping.aspx";
                }

                _stmpl_container.SetAttribute("TBT_ORDER_ID", _Tbt_Order_Id);
                _stmpl_container.SetAttribute("TBT_SHIP_URL", _Tbt_Ship_URL);

                if (HttpContext.Current.Session["USER_ID"] == null || HttpContext.Current.Session["USER_ID"] == "")
                    _stmpl_container.SetAttribute("TBT_LOGIN", false);
                else
                    _stmpl_container.SetAttribute("TBT_LOGIN", true);

                #region "breadcrumb"
                string breadcrumb = string.Empty;
                /*if (HttpContext.Current.Request.Url.ToString().Contains("productdetails.aspx") == true && HttpContext.Current.Request.QueryString["tsb"] != null && HttpContext.Current.Request["tsb"].ToString() != "" && HttpContext.Current.Request["tsm"] != null && HttpContext.Current.Request["tsm"].ToString() != "" && HttpContext.Current.Request["sl1"] != null && HttpContext.Current.Request["sl1"].ToString() != "" && HttpContext.Current.Request["sl2"] != null && HttpContext.Current.Request["sl2"].ToString() != "")
                {
                    DataSet DSBC = null;
                    if (paraCID != "")
                    {
                        DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + paraCID + "'");
                        foreach (DataRow DR in DSBC.Tables[0].Rows)
                        {
                            //breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + paraCID + "&byp=2&bypcat=1\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                            breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + paraCID + "&byp=2\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                        }
                        breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"bybrand.aspx?&ld=0&cid=" + paraCID + "&tsb=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "&byp=2&bypcat=1\">" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "</a>";
                        breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"bybrand.aspx?&ld=0&cid=" + paraCID + "&tsb=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "&tsm=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "&byp=2\">" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "</a>";
                        string sql = "";
                        if (HttpContext.Current.Request["sl2"].ToString() != "0")
                        {
                            sql = "SELECT SUBCATNAME_L1 + ' - ' + SUBCATNAME_L2 FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE TOSUITE_BRAND='" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "' AND TOSUITE_MODEL='" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "' AND CATEGORY_ID='" + paraCID + "' AND SUBCATID_L1='" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "' AND SUBCATID_L2='" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "' AND PRODUCT_ID=" + paraPID.ToString();
                        }
                        else
                        {
                            sql = "SELECT SUBCATNAME_L1 FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE TOSUITE_BRAND='" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "' AND TOSUITE_MODEL='" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "' AND CATEGORY_ID='" + paraCID + "' AND SUBCATID_L1='" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "' AND SUBCATID_L2='' AND PRODUCT_ID=" + paraPID.ToString();
                        }
                        breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"bybrand.aspx?&ld=0&cid=" + paraCID + "&tsb=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "&tsm=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "&sl1=" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "&sl2=" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "&byp=2\">" + GetDataSet(sql).Tables[0].Rows[0][0].ToString() + "</a>";
                    }
                    if (paraPID != "")
                    {
                        DSBC = GetDataSet("SELECT String_value FROM TB_prod_specs WHERE product_ID = " + paraPID + "and attribute_id=1");
                        foreach (DataRow DR in DSBC.Tables[0].Rows)
                        {
                            breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"productdetails.aspx?&pid=" + paraPID + "&fid=" + paraFID + "&cid=" + paraCID + "&tsb=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "&tsm=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "&sl1=" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "&sl2=" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "&byp=2&bypcat=1\" style=\"color:Black;\">" + DR[0].ToString() + "</a>";
                        }
                    }
                }
                else if (HttpContext.Current.Request.Url.ToString().Contains("productdetails.aspx") == true && HttpContext.Current.Request.QueryString["tsb"] != null && HttpContext.Current.Request["tsb"].ToString() != "" && HttpContext.Current.Request["tsm"] != null && HttpContext.Current.Request["tsm"].ToString() != "")
                {
                    DataSet DSBC = null;
                    if (paraCID != "")
                    {
                        DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + paraCID + "'");
                        foreach (DataRow DR in DSBC.Tables[0].Rows)
                        {
                            //breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + paraCID + "&byp=2&bypcat=1\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                            breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + paraCID + "&byp=2\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                        }
                        breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"bybrand.aspx?&ld=0&cid=" + paraCID + "&tsb=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "&byp=2&bypcat=1\">" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "</a>";
                        breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"bybrand.aspx?&ld=0&cid=" + paraCID + "&tsb=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "&tsm=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "&byp=2\">" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "</a>";
                    }
                    if (paraPID != "")
                    {
                        DSBC = GetDataSet("SELECT String_value FROM TB_prod_specs WHERE product_ID = " + paraPID + "and attribute_id=1");
                        foreach (DataRow DR in DSBC.Tables[0].Rows)
                        {
                            breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"productdetails.aspx?&pid=" + paraPID + "&fid=" + paraFID + "&cid=" + paraCID + "&tsb=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "&tsm=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "&byp=2&bypcat=1\" style=\"color:Black;\">" + DR[0].ToString() + "</a>";
                        }
                    }

                }
                else if (HttpContext.Current.Request.Url.ToString().Contains("productdetails.aspx") == true && HttpContext.Current.Request.QueryString["tsb"] != null && HttpContext.Current.Request["tsb"].ToString() != "")
                {
                    DataSet DSBC = null;
                    if (paraCID != "")
                    {
                        DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + paraCID + "'");
                        foreach (DataRow DR in DSBC.Tables[0].Rows)
                        {
                            //breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + paraCID + "&byp=2&bypcat=1\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                            breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + paraCID + "&byp=2\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                        }
                        breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"bybrand.aspx?&ld=0&cid=" + paraCID + "&tsb=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "&byp=2\">" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "</a>";
                    }
                    if (paraPID != "")
                    {
                        DSBC = GetDataSet("SELECT String_value FROM TB_prod_specs WHERE product_ID = " + paraPID + " and attribute_id=1");
                        foreach (DataRow DR in DSBC.Tables[0].Rows)
                        {
                            breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"productdetails.aspx?&pid=" + paraPID + "&fid=" + paraFID + "&cid=" + paraCID + "&tsb=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "&byp=2&bypcat=1\" style=\"color:Black;\">" + DR[0].ToString() + "</a>";
                        }
                    }
                }
                else if (HttpContext.Current.Request.Url.ToString().Contains("family.aspx") == true && _Package == "CSFAMILYPAGE" && HttpContext.Current.Request.QueryString["sl1"] != null && HttpContext.Current.Request.QueryString["sl1"].ToString() != "" && HttpContext.Current.Request.QueryString["sl2"] != null && HttpContext.Current.Request.QueryString["sl2"].ToString() != "")
                {
                    DataSet DSBC = null;
                    DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + HttpContext.Current.Request.QueryString["cid"].ToString() + "'");
                    foreach (DataRow DR in DSBC.Tables[0].Rows)
                    {
                        //breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + DR["CATEGORY_ID"].ToString() +"&byp=2&bypcat=1\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                        breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + DR["CATEGORY_ID"].ToString() + "&byp=2\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                    }
                    string sql = "SELECT DISTINCT SUBCATNAME_L1 + ' - ' + SUBCATNAME_L2 FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE CATEGORY_ID='" + HttpContext.Current.Request.QueryString["cid"].ToString() + "' AND SUBCATID_L1='" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "' AND SUBCATID_L2='" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "'";
                    breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font><a href=\"product_list.aspx?&ld=0&cid=" + HttpContext.Current.Request.QueryString["cid"].ToString() + "&sl1=" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "&sl2=" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "&byp=2&qf=1\" style=\"color:Black;\">" + GetDataSet(sql).Tables[0].Rows[0][0].ToString() + "</a>";

                    DSBC = GetDataSet("SELECT family_NAME,category_id FROM TB_family WHERE family_ID = " + paraFID);
                    foreach (DataRow DR in DSBC.Tables[0].Rows)
                    {
                        breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font><a href=\"family.aspx?&fid=" + paraFID + "&cid=" + HttpContext.Current.Request.QueryString["cid"].ToString() + "&sl1=" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "&sl2=" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "&by=2&qf=1\" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a>";
                    }

                }
                else if (HttpContext.Current.Request.Url.ToString().Contains("productdetails.aspx") == true && _Package == "PRODUCT" && HttpContext.Current.Request.QueryString["sl1"] != null && HttpContext.Current.Request.QueryString["sl1"].ToString() != "" && HttpContext.Current.Request.QueryString["sl2"] != null && HttpContext.Current.Request.QueryString["sl2"].ToString() != "")
                {
                    DataSet DSBC = null;
                    DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + HttpContext.Current.Request.QueryString["cid"].ToString() + "'");
                    foreach (DataRow DR in DSBC.Tables[0].Rows)
                    {
                        //breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + DR["CATEGORY_ID"].ToString() + "&byp=2&bypcat=1\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                        breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + DR["CATEGORY_ID"].ToString() + "&byp=2\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                    }
                    string sql = "SELECT DISTINCT SUBCATNAME_L1 + ' - ' + SUBCATNAME_L2 FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE CATEGORY_ID='" + HttpContext.Current.Request.QueryString["cid"].ToString() + "' AND SUBCATID_L1='" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "' AND SUBCATID_L2='" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "'";
                    breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font><a href=\"product_list.aspx?&ld=0&cid=" + HttpContext.Current.Request.QueryString["cid"].ToString() + "&sl1=" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "&sl2=" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "&byp=2&qf=1\" style=\"color:Black;\">" + GetDataSet(sql).Tables[0].Rows[0][0].ToString() + "</a>";
                    if (HttpContext.Current.Request.QueryString["tf"] != null && HttpContext.Current.Request.QueryString["tf"].ToString() != "")
                    {
                        DSBC = GetDataSet("SELECT family_NAME,category_id FROM TB_family WHERE family_ID = " + paraFID);
                        foreach (DataRow DR in DSBC.Tables[0].Rows)
                        {
                            breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font><a href=\"family.aspx?&fid=" + paraFID + "&cid=" + HttpContext.Current.Request.QueryString["cid"].ToString() + "&sl1=" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "&sl2=" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "&by=2&qf=1\" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a>";
                        }
                    }
                    DSBC = GetDataSet("SELECT String_value FROM TB_prod_specs WHERE product_ID = " + paraPID + " and attribute_id=1");
                    foreach (DataRow DR in DSBC.Tables[0].Rows)
                    {
                        breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"productdetails.aspx?&pid=" + paraPID + "&fid=" + paraFID + "&cid=" + paraCID + "&sl1=" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "&sl2=" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "&byp=2&bypcat=1\" style=\"color:Black;\">" + DR[0].ToString() + "</a>";
                    }
                }
                else if (HttpContext.Current.Request.Url.ToString().Contains("family.aspx") == true && HttpContext.Current.Request.QueryString["sl1"] != null && HttpContext.Current.Request.QueryString["sl1"].ToString() != "" && HttpContext.Current.Request.QueryString["sl2"] != null && HttpContext.Current.Request.QueryString["sl2"].ToString() != "")
                {

                }
                else
                {
                    if (paraPID != "")
                    {
                        DataSet DSBC = null;
                        DSBC = GetDataSet("SELECT String_value FROM TB_prod_specs WHERE product_ID = " + paraPID + "and attribute_id=1");
                        foreach (DataRow DR in DSBC.Tables[0].Rows)
                        {
                            if (HttpContext.Current.Request.QueryString["pcr"] != null && HttpContext.Current.Request.QueryString["pcr"].ToString() != "")
                            {
                                if (HttpContext.Current.Request.QueryString["byp"] != null && HttpContext.Current.Request.QueryString["byp"].ToString() != "")
                                {
                                    if (HttpContext.Current.Request.QueryString["cid"] != null && HttpContext.Current.Request.QueryString["cid"].ToString() != "")
                                    {
                                        breadcrumb = "<a href=\"productdetails.aspx?&pid=" + paraPID + "&fid=" + paraFID + "&cid=" + HttpContext.Current.Request.QueryString["cid"].ToString() + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=" + HttpContext.Current.Request.QueryString["byp"].ToString() + "&qf=1 \" style=\"color:Black;\">" + DR[0].ToString() + "</a>";
                                    }
                                    else
                                        breadcrumb = "<a href=\"productdetails.aspx?&pid=" + paraPID + "&fid=" + paraFID + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=" + HttpContext.Current.Request.QueryString["byp"].ToString() + "&qf=1 \" style=\"color:Black;\">" + DR[0].ToString() + "</a>";
                                }
                                else
                                {
                                    breadcrumb = "<a href=\"productdetails.aspx?&pid=" + paraPID + "&fid=" + paraFID + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&qf=1 \" style=\"color:Black;\">" + DR[0].ToString() + "</a>";
                                }
                            }
                            else
                            {
                                breadcrumb = "<a href=\"productdetails.aspx?&pid=" + paraPID + "&fid=" + paraFID + " \" style=\"color:Black;\">" + DR[0].ToString() + "</a>";
                            }
                        }
                        if (paraFID != "")
                        {
                            string catIDtemp = "";
                            DSBC = GetDataSet("SELECT family_name,category_id FROM TB_family WHERE family_ID = " + paraFID);
                            foreach (DataRow DR in DSBC.Tables[0].Rows)
                            {
                                if (HttpContext.Current.Request.QueryString["pcr"] != null && HttpContext.Current.Request.QueryString["pcr"].ToString() != "")
                                {
                                    if (HttpContext.Current.Request.QueryString["byp"] != null && HttpContext.Current.Request.QueryString["byp"].ToString() != "")
                                    {
                                        if (HttpContext.Current.Request.QueryString["cid"] != null && HttpContext.Current.Request.QueryString["cid"].ToString() != "")
                                            breadcrumb = "<a href=\"family.aspx?&fid=" + paraFID + "&cid=" + HttpContext.Current.Request.QueryString["cid"].ToString() + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=" + HttpContext.Current.Request.QueryString["byp"].ToString() + "&qf=1 \" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                                        else
                                            breadcrumb = "<a href=\"family.aspx?&fid=" + paraFID + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=" + HttpContext.Current.Request.QueryString["byp"].ToString() + "&qf=1 \" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                                    }
                                    else
                                    {
                                        breadcrumb = "<a href=\"family.aspx?&fid=" + paraFID + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + " \" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                                    }
                                }
                                else
                                {
                                    breadcrumb = "<a href=\"family.aspx?&fid=" + paraFID + " \" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                                }
                                catIDtemp = DR[1].ToString();
                            }
                            do
                            {
                                DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
                                foreach (DataRow DR in DSBC.Tables[0].Rows)
                                {
                                    if (DR["PARENT_CATEGORY"].ToString() != "0")
                                    {
                                        if (HttpContext.Current.Request.QueryString["pcr"] != null && HttpContext.Current.Request.QueryString["pcr"].ToString() != "")
                                        {
                                            breadcrumb = "<a href=\"product_list.aspx?&ld=0&cid=" + catIDtemp + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=2&qf=1\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                                        }
                                        else
                                        {
                                            breadcrumb = "<a href=\"product_list.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                                        }
                                    }
                                    else
                                    {
                                        if (HttpContext.Current.Request.QueryString["pcr"] != null && HttpContext.Current.Request.QueryString["pcr"].ToString() != "")
                                        {
                                            breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + catIDtemp + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=2&qf=1\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                                        }
                                        else
                                        {
                                            breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                                        }
                                    }
                                    catIDtemp = DR["PARENT_CATEGORY"].ToString();
                                }
                            } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
                        }
                    }
                    else if (paraFID != "")
                    {
                        //DataSet DSBC = null;
                        //string catIDtemp = "";
                        //DSBC = GetDataSet("SELECT family_NAME,category_id FROM TB_family WHERE family_ID = " + paraFID);
                        //foreach (DataRow DR in DSBC.Tables[0].Rows)
                        //{
                        //    if (HttpContext.Current.Request.QueryString["pcr"] != null && HttpContext.Current.Request.QueryString["pcr"].ToString() != "")
                        //    {
                        //        if (HttpContext.Current.Request.QueryString["byp"] != null && HttpContext.Current.Request.QueryString["byp"].ToString() != "")
                        //        {
                        //            if (HttpContext.Current.Request.QueryString["cid"] != null && HttpContext.Current.Request.QueryString["cid"].ToString() != "")
                        //                breadcrumb = "<a href=\"family.aspx?&fid=" + paraFID + "&cid=" + HttpContext.Current.Request.QueryString["cid"].ToString() + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=" + HttpContext.Current.Request.QueryString["byp"].ToString() + "&qf=1 \" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a>";
                        //            else
                        //                breadcrumb = "<a href=\"family.aspx?&fid=" + paraFID + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=" + HttpContext.Current.Request.QueryString["byp"].ToString() + "&qf=1 \" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a>";
                        //        }
                        //        else
                        //        {
                        //            breadcrumb = "<a href=\"family.aspx?&fid=" + paraFID + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + " \" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a>";
                        //        }
                        //    }
                        //    else
                        //    {
                        //        breadcrumb = "<a href=\"family.aspx?&fid=" + paraFID + " \" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a>";
                        //    }
                        //    catIDtemp = DR[1].ToString();
                        //}
                        //do
                        //{
                        //    DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
                        //    foreach (DataRow DR in DSBC.Tables[0].Rows)
                        //    {
                        //        if (DR["PARENT_CATEGORY"].ToString() != "0")
                        //        {
                        //            if (HttpContext.Current.Request.QueryString["pcr"] != null && HttpContext.Current.Request.QueryString["pcr"].ToString() != "")
                        //            {
                        //                breadcrumb = "<a href=\"product_list.aspx?&ld=0&cid=" + catIDtemp + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=2&qf=1 \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                        //            }
                        //            else
                        //            {
                        //                breadcrumb = "<a href=\"product_list.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            if (HttpContext.Current.Request.QueryString["pcr"] != null && HttpContext.Current.Request.QueryString["pcr"].ToString() != "")
                        //            {
                        //                //breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + catIDtemp + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=2&bypcat=1\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                        //                breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + catIDtemp + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=2\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                        //            }
                        //            else
                        //            {
                        //                breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                        //            }
                        //        }
                        //        catIDtemp = DR["PARENT_CATEGORY"].ToString();
                        //    }
                        //} while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
                    }
                    else if (paraCID != "")
                    {
                        DataSet DSBC = null;
                        string catIDtemp = paraCID;
                        do
                        {
                            DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
                            foreach (DataRow DR in DSBC.Tables[0].Rows)
                            {
                                if (DR["PARENT_CATEGORY"].ToString() != "0")
                                {
                                    if (breadcrumb == "")
                                        breadcrumb = "<a href=\"product_list.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                                    else
                                        breadcrumb = "<a href=\"product_list.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                                }
                                else
                                {
                                    if (breadcrumb == "")
                                        breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                                    else
                                        breadcrumb = "<a href=\"categorylist.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;

                                }
                                catIDtemp = DR["PARENT_CATEGORY"].ToString();
                            }
                        } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
                    }
                }*/
                #endregion
                _stmpl_container.SetAttribute("TBT_BREAD_CRUMBS", breadcrumb.Replace("<ars>g</ars>", "&rarr;"));

                foreach (KeyValuePair<string, TBWDataList[]> kvp in _dict_inner_html)
                {
                    _stmpl_container.SetAttribute(kvp.Key, kvp.Value);
                }

                _status = true;
                if (_Package == "CSFAMILYPAGE")
                {
                    //GetFamilyMultipleImages(Convert.ToInt32(paraFID), _stmpl_container);
                }
                
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                _status = false;
            }
            return _status;
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO BUILD TEMPLATE RECORDS IN COLUMN WISE  ***/
        /********************************************************************************/
        private void BuildRecordsTemplateColumn()
        {
            TBWDataList[] lstrecords = new TBWDataList[0];
            string strFile = HttpContext.Current.Server.MapPath("ProdImages");
            //Build the cell inner body of the HTML
            _stg_records = new StringTemplateGroup(_skin_records, _SkinRootPath);
            DataSet dsrecords = null;
            if (_GDataSet != null && _skin_sql_records.Length == 0)
            {
                dsrecords = _GDataSet;
            }
            else if (_Package.ToString() == "PRODUCT")
            {
                dsrecords = (DataSet)HttpContext.Current.Session["FamilyProduct"];
                string sessionname="FamilyProduct_"+paraPID;
                HttpContext.Current.Session[sessionname] = dsrecords;
            } 
            else if (_Package.ToString() == "NEWPRODUCTLOGNAV")
            {
                dsrecords = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_PRODUCT_LOG_NAV");  
            }
            else if (_Package.ToString() == "NEWPRODUCT HIGHLIGHTSCATLIST")
            {
                dsrecords = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_PRO_HIGHLIGHTS_CAT_LIST");
            }
            else if (_Package.ToString() == "NEWPRODUCT")
            {
                dsrecords = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_PRODUCT 8");
            }
            else if (_Package.ToString() == "NEWPRODUCTNAV")
            {
                dsrecords = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_PRODUCT_NAV");
            }
            else
            {
                dsrecords = GetDataSet(_skin_sql_records, _skin_sql_type_records, _skin_sql_param_records);
            }
            _stg_container = new StringTemplateGroup(_skin_container, _SkinRootPath);
            if (dsrecords != null)
            {


                if (dsrecords.Tables[0].Rows.Count > 0)
                {
                    DataRow[] cellrow = dsrecords.Tables[0].Select("ATTRIBUTE_ID = 1");
                    lstrecords = new TBWDataList[cellrow.Length];
                    int ictrecords = 0, ictcol = 1;
                    string strValue = string.Empty;
                    string pacskinrec = _Package + "\\" + _skin_records;
                    foreach (DataRow cdr in cellrow)
                    {
                        _stmpl_records = _stg_records.GetInstanceOf(pacskinrec);

                        _stmpl_records.SetAttribute("TBT_ECOMENABLED", IsEcomenabled());
                        if (_Package != "PRODUCT") _stmpl_records.SetAttribute("TBT_YOURCOST", GetMyPrice(System.Convert.ToInt32(cdr["PRODUCT_ID"])));

                        if (_Package == "PRODUCT")
                        {
                            GetMultipleImages(System.Convert.ToInt32(cdr["PRODUCT_ID"]), System.Convert.ToInt32(paraFID),System.Convert.ToInt32(cdr["Family_ID"]), _stmpl_records);
                            if (HttpContext.Current.Request.QueryString["fid"] != null)
                            {
                                _fid = HttpContext.Current.Request.QueryString["fid"].ToString();


                                 string eapath = EasyAsk.GetFamilyEAPATH();
                                 if (eapath != "")
                                 {
                                     string[] strs = eapath.Split(new string[] { "////" }, StringSplitOptions.None);
                                     if (strs.Length > 0)
                                     {
                                         DataSet tmpds = objCategoryServices.GetCategotyID(strs[strs.Length - 1].ToString());
                                         if (tmpds != null && tmpds.Tables.Count > 0)
                                             _stmpl_records.SetAttribute("TBT_CATEGORY_ID", tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString());
                                     }
                                     eapath = "AllProducts////WESAUSTRALASIA" + eapath;
                                     _stmpl_records.SetAttribute("TBT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath)));
                                 }
                                 else
                                 {
                                     DataSet tmpds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, _fid, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
                                     if (tmpds != null && tmpds.Tables.Count > 0)
                                     {
                                         _stmpl_records.SetAttribute("TBT_CATEGORY_ID", tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString());
                                          eapath = "AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString();
                                         _stmpl_records.SetAttribute("TBT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath)));
                                     }
                                 }
                            }

                        }


                        if (_Package == "NEWPRODUCTLOGNAV" || _Package == "NEWPRODUCTNAV" || _Package == "NEWPRODUCTHIGHLIGHTSCATLIST")
                        {
                            string espath="AllProducts////WESAUSTRALASIA////" + cdr["CATEGORY_PATH"].ToString();
                            _stmpl_records.SetAttribute("FAMILY_EA_PATH" , HttpUtility.UrlEncode(objSecurity.StringEnCrypt(espath)));

                            espath = "AllProducts////WESAUSTRALASIA////" + cdr["CATEGORY_PATH"].ToString() + "////UserSearch1=Family Id=" + cdr["FAMILY_ID"].ToString();
                            _stmpl_records.SetAttribute("PRODUCT_EA_PATH",  HttpUtility.UrlEncode(objSecurity.StringEnCrypt(espath)));                                     

                        }

                       
                        //if (_Package == "NEWPRODUCT")
                        //{

                        //    string espath = "AllProducts////WESAUSTRALASIA////" + cdr["CATEGORY_PATH"].ToString() + "////UserSearch1=Family Id=" + cdr["FAMILY_ID"].ToString();
                        //    _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(espath)));

                        //}
                        string dccolnameupper = string.Empty;
                        foreach (DataColumn dc in dsrecords.Tables[0].Columns)
                        {
                            dccolnameupper = dc.ColumnName.ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper();
                            //_stmpl_records.SetAttribute(dc.ColumnName, dr[dc.ColumnName].ToString());                                                       
                            if (dccolnameupper != "FAMILY_NAME")
                            {
                                _stmpl_records.SetAttribute("TBT_" + dccolnameupper, cdr[dc.ColumnName.ToString()].ToString());
                            }
                            else
                            {
                                if (_Package != "NEWPRODUCTNAV")
                                    _stmpl_records.SetAttribute("TBT_" + dccolnameupper, cdr[dc.ColumnName.ToString()].ToString());
                                //if (_Package == "NEWPRODUCTNAV")
                                //{
                                //    string familyname = cdr[dc.ColumnName.ToString()].ToString();
                                //    if (familyname.Length <= 45)
                                //    {
                                //        _stmpl_records.SetAttribute("TBT_" + dccolnameupper, cdr[dc.ColumnName.ToString()].ToString());
                                //    }
                                //    else
                                //    {
                                //        familyname = familyname.Substring(0, 45) + "..";
                                //        _stmpl_records.SetAttribute("TBT_" + dccolnameupper, familyname);
                                //    }
                                //}
                                //else
                                //{
                                //    _stmpl_records.SetAttribute("TBT_" + dccolnameupper, cdr[dc.ColumnName.ToString()].ToString());
                                //}
                            
                            }
                            if (_Package == "COMPARE" && dc.ColumnName.ToString().ToUpper() == "PRODUCT_ID")
                            {
                                //DataSet Dsfamilyname = GetDataSet("SELECT TOP(1) FAMILY_ID FROM TB_PROD_FAMILY WHERE PRODUCT_ID =" + cdr["Product_ID"].ToString() + " AND FAMILY_ID IN(SELECT DISTINCT FAMILY_ID FROM TB_CATALOG_FAMILY WHERE CATEGORY_ID IN (SELECT CATEGORY_ID FROM CATEGORY_FUNCTION(" + CATALOG_ID + ", '" + cdr["CATEGORY_ID"].ToString() + "')))");
                                DataSet Dsfamilyname = (DataSet)objHelperDB.GetGenericDataDB(_CATALOG_ID, cdr["Product_ID"].ToString(), cdr["CATEGORY_ID"].ToString(), "GET_FAMILY_ID_COMPARE_PACKAGE", HelperDB.ReturnType.RTDataSet);
                                if (Dsfamilyname != null && Dsfamilyname.Tables[0].Rows.Count > 0)
                                {
                                    _stmpl_records.SetAttribute("TBT_FAMILY_ID", Dsfamilyname.Tables[0].Rows[0][0].ToString());
                                }
                            }
                            else if ((_Package == "NEWPRODUCT" || _Package == "PROMOTIONS" || _Package == "MOREPRODUCTS") && dc.ColumnName.ToString().ToUpper() == "PRODUCT_ID")
                            {
                                //DataSet Dsfamilyname = GetDataSet("select f.family_id,f.family_name,fs.string_value,a.attribute_id,a.attribute_name from tb_family_specs fs,tb_Family f,tb_catalog_family cf,tb_attribute a where f.family_id =fs.family_id and f.family_id=cf.family_id and fs.attribute_id=a.attribute_id and f.family_id in(SELECT TOP(1) FAMILY_ID FROM TB_PROD_FAMILY WHERE PRODUCT_ID =" + cdr["Product_ID"].ToString() + " ) and cf.catalog_id=" + CATALOG_ID);
                                DataSet Dsfamilyname = (DataSet)objHelperDB.GetGenericDataDB(_CATALOG_ID, cdr["Product_ID"].ToString(), "GET_FAMILY_ATTRIBUTE_NEWPRODUCT_PACKAGE", HelperDB.ReturnType.RTDataSet);
                                if (Dsfamilyname != null && Dsfamilyname.Tables[0].Rows.Count > 0)
                                {
                                    if (_Package != "NEWPRODUCT")
                                        _stmpl_records.SetAttribute("TBT_FAMILY_NAME", Dsfamilyname.Tables[0].Rows[0]["FAMILY_NAME"].ToString().Replace("<ars>g</ars>", "&rarr;"));
                                    // _stmpl_records.SetAttribute("TBT_FAMILY_ID", Dsfamilyname.Tables[0].Rows[0]["FAMILY_ID"].ToString());
                                    foreach (DataRow Drow in Dsfamilyname.Tables[0].Rows)
                                    {
                                        string desc = string.Empty;
                                        if (Drow["STRING_VALUE"].ToString().Length > 80)
                                        {
                                            desc = Drow["STRING_VALUE"].ToString().Substring(0, 80) + "...";
                                        }
                                        else
                                        {
                                            desc = Drow["STRING_VALUE"].ToString();
                                        }
                                        _stmpl_records.SetAttribute("TBT_" + Drow["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), desc.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("\n", "<br/>").Replace("\r", "&nbsp;"));

                                    }

                                }
                            }
                        }
                        //_stmpl_records.SetAttribute("TBT_FAMILY_ID", paraFID);
                        string descall = string.Empty;
                        string desc1 = string.Empty;
                        string descallstring = string.Empty;
                        string attName = string.Empty;
                       // int setatttr = 0;
                        string attrnamerple = string.Empty;
                        string stringvalue = string.Empty;
                        foreach (DataRow dr in dsrecords.Tables[0].Select("PRODUCT_ID = " + cdr["PRODUCT_ID"].ToString()))
                        {
                            //_stmpl_records.SetAttribute(dc.ColumnName, dr[dc.ColumnName].ToString());  
                            desc1 = "";
                            attrnamerple = dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper();
                            //When there is no product image then refer family image
                            stringvalue = dr["STRING_VALUE"].ToString();
                            ////try
                            ////{
                            ////    if ((_Package.Equals("NEWPRODUCTLOGNAV") || _Package.Equals("NEWPRODUCTNAV")) && setatttr == 0)
                            ////    {

                            ////        string Product_ID = dr["Product_ID"].ToString();
                            ////        string Attrname = "TWEB IMAGE1";
                            ////        DataRow[] dr1 = dsrecords.Tables[0].Select("PRODUCT_ID='" + Product_ID + "' AND ATTRIBUTE_NAME='" + Attrname + "'");
                            ////        string sSQL = string.Empty;

                            ////        if (dr1.Length <= 0)
                            ////        {
                            ////            string family_id = dr["Family_ID"].ToString();
                            ////            sSQL = "Exec Get_FamilyImage " + family_id + "," + Product_ID;
                            ////            HelperDB objHelperDB = new HelperDB();
                            ////            DataSet dsfamily = objHelperDB.GetDataSetDB(sSQL);

                            ////            setatttr = 1;
                            ////            if (dsfamily.Tables[0] != null)
                            ////            {
                            ////                FileInfo Fil1;
                            ////                Fil1 = new FileInfo(strFile + dsfamily.Tables[0].Rows[0]["string_value"].ToString().Replace("\\", "/"));
                            ////                if (Fil1.Exists)
                            ////                {
                            ////                    _stmpl_records.SetAttribute("TBT_TWEB_IMAGE1", dsfamily.Tables[0].Rows[0]["string_value"].ToString().Replace("\\", "/"));
                            ////                }
                            ////                else
                            ////                {
                            ////                    _stmpl_records.SetAttribute("TBT_TWEB_IMAGE1", "/images/noimage.gif");
                            ////                }
                            ////            }
                            ////            else
                            ////            {
                            ////                _stmpl_records.SetAttribute("TBT_TWEB_IMAGE1", "/images/noimage.gif");
                            ////            }

                            ////        }
                            ////    }
                            ////}
                            ////catch
                            ////{ }
                            //end
                            if (dr["ATTRIBUTE_DATATYPE"].ToString().ToUpper().StartsWith("TEX"))
                            {
                                if (dr["ATTRIBUTE_TYPE"].ToString() == "3" || dr["ATTRIBUTE_TYPE"].ToString() == "9")
                                {
                                    FileInfo Fil;
                                    if (_Package == "PRODUCT")
                                    {
                                        Fil = new FileInfo(strFile + stringvalue.Replace("_TH", "_Images_200"));
                                    }
                                    else
                                    {
                                        Fil = new FileInfo(strFile + stringvalue);
                                    }
                                    if (Fil.Exists)
                                    {
                                        if(_Package != "PRODUCT")
                                            _stmpl_records.SetAttribute("TBT_" + attrnamerple, stringvalue.Replace("\\", "/"));
                                        //if (_Package == "PRODUCT")
                                        //{
                                        //    //_stmpl_records.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"].ToString().Replace("_TH", "_Images_200"));
                                        //}
                                        //else
                                        //{
                                        //    _stmpl_records.SetAttribute("TBT_" + attrnamerple, stringvalue.Replace("\\", "/"));
                                        //}
                                    }
                                    else
                                    {
                                        _stmpl_records.SetAttribute("TBT_" + attrnamerple, "");
                                    }
                                }
                                //else if (dr["ATTRIBUTE_NAME"].ToString().ToLower() == " SHORT_DESCRIPTION" || dr["ATTRIBUTE_NAME"].ToString() == "Descriptions" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "FEATURES" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATION" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "APPLICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "NOTES")
                                //{
                                    //desc1 = dr["STRING_VALUE"].ToString().Replace("*", "").Replace("\n", "<br/>").Replace("\r", "&nbsp;");
                                //}
                                else
                                {
                                    attName = dr["ATTRIBUTE_NAME"].ToString().ToUpper();
                                    if (_Package == "PRODUCT")
                                    {
                                        if (attName == "DESCRIPTIONS" || attName == "FEATURES" || attName == "SPECIFICATION" || attName == "SPECIFICATIONS" || attName == "APPLICATIONS" || attName == "NOTES")
                                        {

                                            desc1 = stringvalue.Replace("*", "").Replace("\n", "<br/>").Replace("\r", "&nbsp;");
                                        }
                                        else
                                            _stmpl_records.SetAttribute("TBT_" + attrnamerple, stringvalue);
                                    }
                                    else
                                        _stmpl_records.SetAttribute("TBT_" + attrnamerple, stringvalue);
                                }
                                //if (dr["ATTRIBUTE_NAME"].ToString().ToLower() == " SHORT_DESCRIPTION" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "DESCRIPTIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "FEATURES" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATION" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "APPLICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "NOTES")
                                //{
                                //    descall = descall + desc1;
                                //}
                            }
                            //if (dr["ATTRIBUTE_NAME"].ToString().ToLower() == " SHORT_DESCRIPTION" || dr["ATTRIBUTE_NAME"].ToString() == "Descriptions" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "FEATURES" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATION" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "APPLICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "NOTES")
                            if (_Package == "PRODUCT")
                            {
                                if (desc1!="")
                                    descall = descall + desc1 + "<br/><br/>";
                            }
           
                            else if (dr["ATTRIBUTE_DATATYPE"].ToString().ToUpper().StartsWith("NUM"))
                            {
                                if (dr["NUMERIC_VALUE"].ToString().Length > 0)
                                    _stmpl_records.SetAttribute("TBT_" + attrnamerple, objHelperService.FixDecPlace(Convert.ToDecimal(dr["NUMERIC_VALUE"])).ToString());
                                else
                                    _stmpl_records.SetAttribute("TBT_" + attrnamerple, "");
                            }

                         

                        }
                        // if (dr["ATTRIBUTE_NAME"].ToString().ToLower() == " SHORT_DESCRIPTION" || dr["ATTRIBUTE_NAME"].ToString() == "Descriptions" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "FEATURES" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATION" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "APPLICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "NOTES")
                        if (_Package == "PRODUCT")
                        {

                            if (descall == "")
                                _stmpl_records.SetAttribute("TBT_PROD_DESC_SHOW", true);
                            else
                                _stmpl_records.SetAttribute("TBT_PROD_DESC_SHOW", false);

                            if (descall.Length > 400)
                                _stmpl_records.SetAttribute("TBT_MORE_SHOW", true);
                            else
                                _stmpl_records.SetAttribute("TBT_MORE_SHOW", false);

                            if (descall.Length > 400)
                            {
                                //  _stmpl_records.SetAttribute("TBT_MORE_SHOW", true);
                                descallstring = descall.Substring(0, 400).ToString();
                                _stmpl_records.SetAttribute("TBT_MORE", descallstring);
                                _stmpl_records.SetAttribute("TBT_MENU_ID", "2");
                                descall = descall.Substring(0, descall.Length).ToString();
                                // descall = descall.Substring(300).ToString();

                                _stmpl_records.SetAttribute("TBT_DESCALL", descall);

                            }
                            else
                            {
                                _stmpl_records.SetAttribute("TBT_DESCALL", descall);
                                _stmpl_records.SetAttribute("TBT_MENU_ID", "2");
                                // _stmpl_container.SetAttribute("TBT_MORE_SHOW", false);
                                _stmpl_records.SetAttribute("TBT_MORE", descall);
                            }
                        }
                       // if (_Package == "PRODUCT")
                      //  {
                            //bool descflag = false;
                           // int familyrows = 0;
                           // DataSet dsfamily = new DataSet();
                            //dsfamily = GetDataSet("select f.family_id,f.family_name,fs.string_value,a.attribute_id,a.attribute_name from tb_family f,tb_family_specs fs,tb_attribute a where f.family_id=fs.family_id and f.family_id =" + paraFID + " and fs.attribute_id=a.attribute_id and a.attribute_type=7 and a.attribute_id in(90,91,377,379,4)");
                            //if (dsfamily != null && dsfamily.Tables[0] != null && dsfamily.Tables[0].Rows.Count > 0)
                            //{
                            //    _stmpl_records.SetAttribute("TBT_FAMILY_NAME", dsfamily.Tables[0].Rows[0]["FAMILY_NAME"].ToString().Replace("<ars>g</ars>", "&rarr;"));
                            //    foreach (DataRow _rows in dsfamily.Tables[0].Rows)
                            //    {
                            //        if (_rows["STRING_VALUE"].ToString().Trim() == "")
                            //        {
                            //            familyrows++;
                            //        }
                            //        _stmpl_records.SetAttribute("TBT_" + _rows["ATTRIBUTE_NAME"].ToString().ToUpper().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_"), _rows["STRING_VALUE"].ToString().Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("\r", "&nbsp;").Replace("\n", "<br/>"));
                            //    }
                            //    if (familyrows == dsfamily.Tables[0].Rows.Count)
                            //    {
                            //        _stmpl_records.SetAttribute("TBT_DESHEADER", "none");
                            //    }
                            //}
                            //else
                            //{
                            //    dsfamily = GetDataSet("select family_id,family_name from tb_family where family_id=" + paraFID);
                            //    _stmpl_records.SetAttribute("TBT_FAMILY_NAME", dsfamily.Tables[0].Rows[0]["FAMILY_NAME"].ToString().Replace("<ars>g</ars>", "&rarr;"));
                            //    _stmpl_records.SetAttribute("TBT_DESHEADER", "none");
                            //}                                     

                            // _stmpl_records.SetAttribute("TBT_FAMILY_ID", cdr["FAMILY_ID"]);
                            //_stmpl_records.SetAttribute("TBT_FAMILY_NAME", cdr["FAMILY_NAME"].ToString().Replace("<ars>g</ars>", "&rarr;"));
                            //_stmpl_records.SetAttribute("TBT_DESHEADER", "none");
                       // }
                       // else
                       // {
                      //      _stmpl_records.SetAttribute("TBT_FAMILY_ID", paraFID);
                      //  }
                        if ( _Package != "PRODUCT")
                            _stmpl_records.SetAttribute("TBT_FAMILY_ID", paraFID);

                        if (_Package == "PRODUCT" || _Package == "COMPARE" || _Package == "NEWPRODUCT" || _Package == "PROMOTIONS" || _Package == "NEWPRODUCTNAV" || _Package == "MOREPRODUCTS" || _Package == "NEWPRODUCTHIGHLIGHTSCATLIST")
                        {
                            if (_Package == "PRODUCT")
                            {
                                ////if (cdr["QTY_AVAIL"] != null && cdr["MIN_ORD_QTY"] != null)
                                ////{
                                ////    if (Convert.ToInt32(cdr["QTY_AVAIL"].ToString()) > 0)
                                ////    {
                                ////        // _stmpl_records.SetAttribute("TBT_QTY_AVAIL", cdr["QTY_AVAIL"].ToString());
                                ////        // _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", cdr["MIN_ORD_QTY"].ToString());
                                ////    }
                                ////    else
                                ////    {
                                ////        _stmpl_records.SetAttribute("TB_DISPLAY", "none");
                                ////    }
                                ////}
                                ////else
                                ////{
                                ////    _stmpl_records.SetAttribute("TB_DISPLAY", "none");
                                ////}

                                if (cdr["FAMILY_PROD_COUNT"] != null && cdr["PROD_COUNT"] != null)
                                {
                                    if (cdr["FAMILY_PROD_COUNT"].ToString() != cdr["PROD_COUNT"].ToString())
                                    {
                                        DataSet _parentFamilyds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, cdr["FAMILY_ID"].ToString(), "GET_FAMILY_ID_AND_PARENT", HelperDB.ReturnType.RTDataSet);
                                        string _parentFamily_Id = "0";
                                        if (_parentFamilyds != null && _parentFamilyds.Tables.Count > 0 && _parentFamilyds.Tables[0].Rows.Count > 0)
                                            _parentFamily_Id = _parentFamilyds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();

                                        if (_parentFamily_Id == "0")
                                            _parentFamily_Id = cdr["FAMILY_ID"].ToString();

                                        _stmpl_records.SetAttribute("TBT_PARENT_FAMILY_ID", _parentFamily_Id);

                                        _stmpl_records.SetAttribute("TBT_DISPLAY_TIP", "block");
                                    }
                                    else
                                        _stmpl_records.SetAttribute("TBT_DISPLAY_TIP", "none");
                                }
                                else
                                    _stmpl_records.SetAttribute("TBT_DISPLAY_TIP", "none");


                                

                            }
                            else
                            {
                                DataSet dsProduct = new DataSet();
                                //dsProduct = GetDataSet("SELECT PRODUCT_ID,QTY_AVAIL,MIN_ORD_QTY FROM TBWC_INVENTORY WHERE PRODUCT_ID=" + cdr["PRODUCT_ID"].ToString());
                                dsProduct = (DataSet)objHelperDB.GetGenericDataDB(cdr["PRODUCT_ID"].ToString(), "GET_SINGLE_PRODUCT_INVENTORY", HelperDB.ReturnType.RTDataSet);
                                if (dsProduct != null && dsProduct.Tables[0] != null && dsProduct.Tables[0].Rows.Count > 0)
                                {
                                    if (Convert.ToInt32(dsProduct.Tables[0].Rows[0]["QTY_AVAIL"]) > 0)
                                    {
                                        _stmpl_records.SetAttribute("TBT_QTY_AVAIL", dsProduct.Tables[0].Rows[0]["QTY_AVAIL"].ToString());
                                        _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", dsProduct.Tables[0].Rows[0]["MIN_ORD_QTY"].ToString());
                                    }
                                    else
                                    {
                                        _stmpl_records.SetAttribute("TB_DISPLAY", "none");
                                    }
                                }
                                else
                                {
                                    if (_Package == "PRODUCT")
                                    {
                                        _stmpl_records.SetAttribute("TB_DISPLAY", "none");
                                    }
                                }
                            }
                            string prodValues = string.Empty;
                            /*foreach (DataRow dr in dsrecords.Tables[dsrecords.Tables.Count - 1].Select("ATTRIBUTE_TYPE <> 3 AND ATTRIBUTE_ID NOT IN (1,5,450,449,481,482,483,484,485,486,487,488,489,490,491,492,493,494,495,496,497,498,499,500,501,502,503) AND PRODUCT_ID = " + cdr["PRODUCT_ID"].ToString(), "ATTRIBUTE_NAME"))
                            {
                                
                                //_stmpl_records.SetAttribute(dc.ColumnName, dr[dc.ColumnName].ToString());                            
                                if( dr["attribute_name"].ToString().ToUpper()!="SUIT" &&  dr["attribute_name"].ToString().ToUpper()!="BRAND")
                                if (dr["ATTRIBUTE_DATATYPE"].ToString().ToUpper().StartsWith("TEX"))
                                {
                                    if(dr["STRING_VALUE"]!=null && dr["STRING_VALUE"].ToString() !="" && dr["ATTRIBUTE_NAME"].ToString().ToUpper() != "NEW PRODUCTS")
                                        prodValues = prodValues + "<TR><TD bgcolor=\"white\" align=\"center\" style=\"border-color:Black;\" width=\"250px\"><strong>" + dr["attribute_name"] + ": </strong></TD><TD bgcolor=\"white\" align=\"center\" style=\"border-color:Black;\" width=\"250px\">" + dr["STRING_VALUE"] + "</TD></TR>"; 
                                }
                                else if (dr["ATTRIBUTE_DATATYPE"].ToString().ToUpper().StartsWith("NUM"))
                                {
                                    if (dr["NUMERIC_VALUE"].ToString().Length > 0)
                                        if (dr["ATTRIBUTE_TYPE"].ToString() == "4")
                                        {
                                            prodValues = prodValues + "<TR><TD bgcolor=\"white\" style=\"border-color:Black;\" align=\"center\" width=\"250px\"><strong>" + dr["attribute_name"] + ": </strong></TD><TD bgcolor=\"white\" align=\"center\" style=\"border-color:Black;\" width=\"250px\">" + " " + objHelperService.GetOptionValues("CURRENCYFORMAT").ToString() + " " + Math.Round(Convert.ToDecimal(dr["NUMERIC_VALUE"]), 2).ToString() + "</TD></TR>";
                                        }
                                        else
                                        {
                                            prodValues = prodValues + "<TR><TD bgcolor=\"white\" style=\"border-color:Black;\" align=\"center\" width=\"250px\"><strong>" + dr["attribute_name"] + ": </strong></TD><TD bgcolor=\"white\" align=\"center\" style=\"border-color:Black;\" width=\"250px\">" + Math.Round(Convert.ToDecimal(dr["NUMERIC_VALUE"]), 2).ToString() + "</TD></TR>";
                                        }
                                }

                            }*/
                            /*
                            foreach (DataRow dr in dsrecords.Tables[0].Select("ATTRIBUTE_TYPE <> 3 AND ATTRIBUTE_ID <> 1 and  PRODUCT_ID = " + cdr["PRODUCT_ID"].ToString(), "ATTRIBUTE_NAME"))
                            {
                                if (dr["attribute_name"].ToString().ToUpper() != "SUIT" && dr["attribute_name"].ToString().ToUpper() != "BRAND")
                                    if (dr["ATTRIBUTE_DATATYPE"].ToString().ToUpper().StartsWith("TEX"))
                                    {
                                        if (dr["STRING_VALUE"] != null && dr["STRING_VALUE"].ToString() != "" && dr["ATTRIBUTE_NAME"].ToString().ToUpper() != "NEW PRODUCTS")
                                           prodValues = prodValues + "<TR><TD bgcolor=\"white\" align=\"center\" style=\"border-color:Black;\" width=\"250px\"><strong>" + dr["attribute_name"] + ": </strong></TD><TD bgcolor=\"white\" align=\"center\" style=\"border-color:Black;\" width=\"250px\">" + dr["STRING_VALUE"] + "</TD></TR>";
                                           
                                        }
                            }*/
                            string _sPriceTable = string.Empty;
                            string _StockStatus = string.Empty;
                            string _Prod_Stock_Status = "0";
                            string _Prod_Stock_Flag = "0";
                            string Issubstitute = "";
                            string _Eta = string.Empty;
                            bool isProductReplace = true;
                            string strReplacedProduct = "";
                            string CustomerType = "";
                            if (_Package == "PRODUCT")
                            {
                               // objErrorHandler.CreateLog("inside product load"); 
                                DataSet dsPriceTable1 = new DataSet();
                                
                                string userid = "0";

                                _StockStatus = cdr["STOCK_STATUS_DESC"].ToString().Replace("_", " ");
                                _Prod_Stock_Status = cdr["PROD_STOCK_STATUS"].ToString();
                                _Prod_Stock_Flag = cdr["PROD_STOCK_FLAG"].ToString();
                              Issubstitute=  cdr["PROD_SUBSTITUTE"].ToString().Trim();
                                  if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
                                    userid = HttpContext.Current.Session["USER_ID"].ToString();
                                CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(userid));



                                _StockStatus = _StockStatus.Trim().Replace("_" ," ");


                                //if ((_StockStatus.ToUpper().Contains("OUT OF STOCK ITEM WILL BE BACK ORDERED") || _StockStatus.ToUpper().Contains("SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED")))//&& CustomerType.ToLower() == "dealer"
                                //     isProductReplace = false;
                                // else
                                // {
                                //     if (_Prod_Stock_Status.ToLower() == "true" || _Prod_Stock_Status.ToLower() == "1")
                                //         isProductReplace = false;
                                //     else if (_Prod_Stock_Flag=="0")
                                //         isProductReplace = false;
                                // }
                               // objErrorHandler.CreateLog("_Prod_Stock_Flag" + _Prod_Stock_Flag );
                                if (_Prod_Stock_Flag == "0") 
                                {
                                    isProductReplace = false;
                                }
                               // objErrorHandler.CreateLog("isProductReplace---" + isProductReplace + "Issubstitute" + Issubstitute);
                                 if ((isProductReplace == true ) && (Issubstitute!=""))
                                 {
                                    // objErrorHandler.CreateLog("b4 Replacement product---" + cdr["STRING_VALUE"].ToString() + "stockstaus" + _StockStatus);
                                     strReplacedProduct = GetProductReplacementDetails(_stmpl_records, cdr["STRING_VALUE"].ToString(), Convert.ToInt32(userid), _StockStatus);
                                // objErrorHandler.CreateLog("isProductReplace---" + isProductReplace + "strReplacedProduct" + strReplacedProduct);
                                 }
                                 _sPriceTable = GetProductPriceTable(Convert.ToInt32(cdr["PRODUCT_ID"]));
                                 //if (isProductReplace == false || strReplacedProduct=="Not Replaced")
                                 //{
                                 //    objErrorHandler.CreateLog("inside product load" + isProductReplace + strReplacedProduct); 
                                 
                                     if (cdr["ETA"].ToString() != "")
                                     {
                                         _Eta = string.Format("<tr><td><b>ETA</b></td><td colspan=\"2\"><b>" + cdr["ETA"].ToString() + "</b></td></tr>");
                                     }
                                 //}





                                
                             
                        

                            }
                            else
                            {
                               // objErrorHandler.CreateLog("GetProductPriceTable" + cdr["PRODUCT_ID"]); 
                                _sPriceTable = GetProductPriceTable(Convert.ToInt32(cdr["PRODUCT_ID"]));
                                _StockStatus = GetStockStatus(Convert.ToInt32(cdr["PRODUCT_ID"]));
                            }
                            string _Tbt_Stock_Status = string.Empty;
                            string _Tbt_Stock_Status_1 = string.Empty;
                            bool _Tbt_Stock_Status_2 = false;
                            string _Tbt_Stock_Status_3 = string.Empty;
                            string _Colorcode1 = string.Empty;
                            string _Colorcode = string.Empty; 
                            string _StockStatusTrim = _StockStatus.Trim();




                            //switch (_StockStatusTrim)
                            //{
                            //    case "IN STOCK":
                            //        _Tbt_Stock_Status = "INSTOCK";
                            //        _Tbt_Stock_Status_2 = true;
                            //        _Colorcode = "#43A246";
                            //        break;
                            //    case "SPECIAL ORDER":
                            //        _Tbt_Stock_Status = "SPECIAL ORDER PRICE & AVAILBAILTY TO BE CONFIRMED";
                            //        _Tbt_Stock_Status_2 = true;
                            //        _Colorcode = "#43A246";
                            //        break;
                            //    case "DISCONTINUED":
                            //        _Tbt_Stock_Status = "DISCONTINUED";
                            //        _Tbt_Stock_Status_2 = false;
                            //        _Colorcode = "#ED1C24";
                            //        break;
                            //    case "TEMPORARY UNAVAILBLE":
                            //        _Tbt_Stock_Status = "TEMPORARY UNAVAILABLE NO ETA";
                            //        _Tbt_Stock_Status_2 = false;
                            //        _Colorcode = "#F9A023";
                            //        break;
                            //    case "OUT OF STOCK":
                            //        _Tbt_Stock_Status = "OUT OF STOCK";
                            //        _Tbt_Stock_Status_2 = false;
                            //        _Colorcode = "#F9A023";
                            //        _Tbt_Stock_Status_1 = "ITEM WILL BE BACK ORDERED";
                            //        _Colorcode1 = "#43A246";
                            //        break;
                            //    default:
                            //        _Tbt_Stock_Status = _StockStatus;
                            //        _Tbt_Stock_Status_2 = true;
                            //        _Colorcode = "Black";
                            //        break;
                            //}
                           // objErrorHandler.CreateLog("Product Details" + _StockStatusTrim);
                            switch (_StockStatusTrim)
                            {
                                case "IN STOCK":
                                    _Colorcode = "#43A246";
                                    _Tbt_Stock_Status = "INSTOCK";
                                    _Tbt_Stock_Status_2 = true;
                                    break;
                                case "Limited Stock, Please Call":
                                    _Colorcode = "#f69e1b";
                                    _Tbt_Stock_Status = "Limited Stock - Please Call";
                                    _Tbt_Stock_Status_2 = true;
                                    break;
                                case "SPECIAL ORDER":
                                    _Colorcode = "#43A246";
                                    _Tbt_Stock_Status_2 = true;
                                    _Tbt_Stock_Status = "SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED";
                                    break;
                                case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
                                    _Colorcode = "#43A246";
                                    _Tbt_Stock_Status_2 = true;
                                    _Tbt_Stock_Status = "SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED";
                                    break;
                                case "SPECIAL ORDER PRICE &":
                                    _Colorcode = "#43A246";
                                    _Tbt_Stock_Status_2 = true;
                                    _Tbt_Stock_Status = "SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED";
                                    break;
                                case "DISCONTINUED":
                                    _Colorcode = "#ED1C24";
                                    _Tbt_Stock_Status_2 = false;
                                    _Tbt_Stock_Status_3 = "DISCONTINUED NO LONGER AVAILABLE";
                                    break;
                                case "DISCONTINUED NO LONGER AVAILABLE":
                                    _Colorcode = "#ED1C24";
                                    _Tbt_Stock_Status_2 = false;
                                    _Tbt_Stock_Status_3 = "DISCONTINUED NO LONGER AVAILABLE";
                                    break;
                                case "DISCONTINUED NO LONGER":
                                    _Colorcode = "#ED1C24";
                                    _Tbt_Stock_Status_2 = false;
                                    _Tbt_Stock_Status_3 = "DISCONTINUED NO LONGER AVAILABLE";
                                    break;
                                case "TEMPORARY UNAVAILABLE":
                                    _Colorcode = "#F9A023";
                               //modified by Indu
                                    _Tbt_Stock_Status_2 = false;

                                    //_Tbt_Stock_Status = "<span style=\"color:" + _Colorcode + "\">TEMPORARY UNAVAILABLE NO ETA</span>";
                                    _Tbt_Stock_Status_3 = "TEMPORARY UNAVAILABLE NO ETA";
                                    break;
                                case "TEMPORARY UNAVAILABLE NO ETA":
                                    _Colorcode = "#F9A023";
                                    //modified by Indu
                                    _Tbt_Stock_Status_2 = false;
                                    _Tbt_Stock_Status_3 = "TEMPORARY UNAVAILABLE NO ETA";
                                    break;
                                case "OUT OF STOCK":
                                    _Tbt_Stock_Status_3 = "OUT OF STOCK";
                                    _Tbt_Stock_Status_2 = true;
                                    _Colorcode = "#F9A023";
                                    _Tbt_Stock_Status_1 = "ITEM WILL BE BACK ORDERED";
                                    _Colorcode1 = "#43A246";
                                    break;
                                case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
                                    _Tbt_Stock_Status_3 = "OUT OF STOCK";
                                    _Tbt_Stock_Status_2 = true;
                                    _Colorcode = "#F9A023";
                                    _Tbt_Stock_Status_1 = "ITEM WILL BE BACK ORDERED";
                                    _Colorcode1 = "#43A246";
                                    break;
                                case "OUT OF STOCK ITEM WILL":
                                    _Tbt_Stock_Status_3 = "OUT OF STOCK";
                                    _Tbt_Stock_Status_2 = true;
                                    _Colorcode = "#F9A023";
                                    _Tbt_Stock_Status_1 = "ITEM WILL BE BACK ORDERED";
                                    _Colorcode1 = "#43A246";
                                    break;

                                case "Please Call":
                                  
                                    _Tbt_Stock_Status_2 = true;
                                    _Tbt_Stock_Status = "Please Call";
                                    _Colorcode = "#43A246";
                                    break;
                                case "Please_Call":
                                    _Tbt_Stock_Status_2 = true;
                                    _Tbt_Stock_Status = "Please Call";
                                    _Colorcode = "#43A246";
                                    break;
                                default:
                                    _Colorcode = "Black";
                                    _Tbt_Stock_Status = _StockStatusTrim;
                                    break;
                            }



                            if (isProductReplace == true &&  strReplacedProduct=="Replaced" )
                            {
                                _stmpl_records.SetAttribute("TBT_REPLACED", true);
                               // _stmpl_records.SetAttribute("TBT_REPLACED_DETAIL", strReplacedProduct);
                            }
                            else
                            {

                                _stmpl_records.SetAttribute("TBT_REPLACED", false);
                                _stmpl_records.SetAttribute("TBT_COLOR_CODE", _Colorcode);

                             //   objErrorHandler.CreateLog("PROD_STOCK_FLAG - " + cdr["PROD_STOCK_FLAG"].ToString());
                              //  objErrorHandler.CreateLog("STOCK_STATUS_DESC - " + cdr["STOCK_STATUS_DESC"].ToString());
                               // objErrorHandler.CreateLog("PROD_STOCK_STATUS - " + cdr["PROD_STOCK_STATUS"].ToString());

                                bool isSameLogic = true;

                                if (HttpContext.Current.Session["EBAY_BLOCK"] != null && HttpContext.Current.Session["EBAY_BLOCK"].ToString() == "True" && cdr["EBAY_BLOCK"] != null && cdr["EBAY_BLOCK"].ToString() == "True")
                                {
                                    isSameLogic = true;
                                }
                                else if ((cdr["PROD_STOCK_FLAG"].ToString() == "-2" && cdr["PROD_STOCK_STATUS"].ToString() == "False" && cdr["STOCK_STATUS_DESC"].ToString().Trim() == "OUT_OF_STOCK ITEM WILL BE BACK ORDERED") || (cdr["PROD_STOCK_FLAG"].ToString() == "0" && cdr["PROD_STOCK_STATUS"].ToString() == "True" && cdr["STOCK_STATUS_DESC"].ToString().Trim() == "Please_Call") || (cdr["PROD_STOCK_FLAG"].ToString() == "-2" && cdr["PROD_STOCK_STATUS"].ToString() == "False" && cdr["STOCK_STATUS_DESC"].ToString().Trim() == "SPECIAL_ORDER PRICE & AVAILABILITY TO BE CONFIRMED"))
                                {
                                    isSameLogic = GetStockDetails(_stmpl_records, cdr["PRODUCT_ID"].ToString());
                                }
                                else
                                {
                                    isSameLogic = true;
                                    //_stmpl_records.SetAttribute("TBT_STOCK_STATUS_3", _Tbt_Stock_Status_3);
                                }
                                //_stmpl_records.SetAttribute("TBT_COLOR_CODE_1", _Colorcode1);
                                //_stmpl_records.SetAttribute("TBT_STOCK_STATUS_1", _Tbt_Stock_Status_1);
                                //_stmpl_records.SetAttribute("TBT_STOCK_STATUS_2", _Tbt_Stock_Status_2);
                                //_stmpl_records.SetAttribute("TBT_STOCK_STATUS_ETA", _Eta);

                                //objErrorHandler.CreateLog("isSameLogic " + isSameLogic);
                                _stmpl_records.SetAttribute("TBT_EXISTING_LOGIC", isSameLogic);
                                _stmpl_records.SetAttribute("TBT_PRICE_TABLE", _sPriceTable);
                                _stmpl_records.SetAttribute("TBT_PDISPLAY_MODE", _sPriceTable == "" ? "none" : "");

                                if (isSameLogic)
                                {

                                    if (HttpContext.Current.Session["EBAY_BLOCK"] != null && HttpContext.Current.Session["EBAY_BLOCK"].ToString() == "True" && cdr["EBAY_BLOCK"] != null && cdr["EBAY_BLOCK"].ToString() == "True")
                                    {
                                        _stmpl_records.SetAttribute("TBT_STOCK_STATUS", "UNAVAILABLE");
                                        _stmpl_records.SetAttribute("TBT_STOCK_STATUS_2", false);
                                    }
                                    else if (_Tbt_Stock_Status != "")
                                    {
                                        _stmpl_records.SetAttribute("TBT_STOCK_STATUS", _Tbt_Stock_Status);
                                        _stmpl_records.SetAttribute("TBT_STOCK_STATUS_2", _Tbt_Stock_Status_2);
                                    }
                                    else
                                    {
                                        _stmpl_records.SetAttribute("TBT_STOCK_STATUS_3", _Tbt_Stock_Status_3);
                                        _stmpl_records.SetAttribute("TBT_STOCK_STATUS_2", _Tbt_Stock_Status_2);
                                    }
                                    _stmpl_records.SetAttribute("TBT_COLOR_CODE_1", _Colorcode1);
                                    _stmpl_records.SetAttribute("TBT_STOCK_STATUS_1", _Tbt_Stock_Status_1);
                                    //_stmpl_records.SetAttribute("TBT_STOCK_STATUS_2", _Tbt_Stock_Status_2);
                                    _stmpl_records.SetAttribute("TBT_STOCK_STATUS_ETA", _Eta);


                                    ////_stmpl_records.SetAttribute("TBT_STOCK_STATUS", GetStockStatus(Convert.ToInt32(cdr["PRODUCT_ID"])));
                                    //_stmpl_records.SetAttribute("TBT_PRICE_TABLE", _sPriceTable);
                                    //_stmpl_records.SetAttribute("TBT_PDISPLAY_MODE", _sPriceTable == "" ? "none" : "");
                                }
                            }


                           // if (prodValues != "")
                           // {
                                if (_Package == "PRODUCT")
                                {
                                    GetProductDetails(Convert.ToInt32(cdr["PRODUCT_ID"]), Convert.ToInt32(paraFID), _stmpl_records);
                                    //GetProductDesc(Convert.ToInt32(cdr["PRODUCT_ID"]), Convert.ToInt32(paraFID), _stmpl_records);
                                }
                          //      _stmpl_records.SetAttribute("TBT_ALL_PRODUCTVALUES", prodValues);
                          //      _stmpl_records.SetAttribute("TBT_DISPLAY_MODE", "");
                          //  }
                          //  else
                          //  {
                         //       _stmpl_records.SetAttribute("TBT_DISPLAY_MODE", "none");
                          //  }
                        }
                        if (_Package == "PRODUCT")
                        {
                            string vpchref = string.Empty;
                            string eapath = string.Empty;
                            string ctname = string.Empty;
                            DataSet bcvpc = new DataSet();
                            if (HttpContext.Current.Session["BreadCrumbDS"] != null)
                            {
                                bcvpc = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];
                            }
                            if (bcvpc != null && bcvpc.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow dr in bcvpc.Tables[0].Rows)
                                {
                                    if (dr["ItemType"].ToString().ToLower() == "category")
                                    {
                                        vpchref = ""; ctname = "";
                                        eapath = "";
                                        vpchref = dr["Url"].ToString();
                                        eapath = dr["EAPath"].ToString();
                                        ctname = dr["ItemValue"].ToString();
                                    }
                                }
                                eapath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath));
                                vpchref = vpchref + "&path=" + eapath;
                                _stmpl_records.SetAttribute("TBT_VPCHREF", vpchref);
                                _stmpl_records.SetAttribute("TBT_CAT_NAME_VPC", ctname.Replace("$",""));
                            }
                        }
                        if (_Package == "PRODUCT") // for download tab
                        {
                            ST_Product_Download(cdr["PRODUCT_ID"].ToString());

                            _stmpl_records.SetAttribute("TBT_DOWNLOAD_DATA", downloadST);
                            _stmpl_records.SetAttribute("TBT_DOWNLOAD", isdownload);
                            downloadST = "";


                        }
                        if (_Package.Contains("FAMILYPAGE"))
                        {
                            DataSet dsProduct = new DataSet();
                            //dsProduct = GetDataSet("SELECT PRODUCT_ID,QTY_AVAIL,MIN_ORD_QTY FROM TBWC_INVENTORY WHERE PRODUCT_ID=" + cdr["PRODUCT_ID"].ToString());
                            dsProduct = (DataSet)objHelperDB.GetGenericDataDB(cdr["PRODUCT_ID"].ToString(), "GET_SINGLE_PRODUCT_INVENTORY", HelperDB.ReturnType.RTDataSet);
                            _stmpl_records.SetAttribute("TBT_QTY_AVAIL", dsProduct.Tables[0].Rows[0]["QTY_AVAIL"].ToString());
                            _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", dsProduct.Tables[0].Rows[0]["MIN_ORD_QTY"].ToString());
                        }
                        strValue = strValue + _stmpl_records.ToString();
                        if (ictcol == _grid_cols)
                        {
                            _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + _skin_container);
                            _stmpl_container.SetAttribute("TBWDataList", strValue);
                            if (ictrecords % 2 == 0)
                                _stmpl_container.SetAttribute("BGCOLOR", "TableEvenRow");
                            else
                                _stmpl_container.SetAttribute("BGCOLOR", "TableOddRow");
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
                            ictrecords++; ictcol = 1; strValue = "";
                        }
                        else
                        {
                            ictcol++;
                        }
                    }
                    if (strValue != "")
                    {
                        _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + _skin_container);
                        _stmpl_container.SetAttribute("TBWDataList", strValue);
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
                        ictrecords++; ictcol = 1; strValue = "";
                    }
                   
                }
            }

            _dict_inner_html[_skin_body_attribute] = lstrecords;
        }
        private string GetProductReplacementDetails(StringTemplate _stmpl_records, string _CODE, int user_id, string _StockStatus)
        {
            string _catid = "", pfid = "", Ea_Path = "", wag_product_code = "", SubstuyutePid = "";
            string _sPriceTable = "";
            bool samecodesubproduct = false;
            bool samecodenotFound = false;
            DataTable rtntbl = objProductServices.GetSubstituteProductDetails(_CODE, user_id);
            if (rtntbl != null && rtntbl.Rows.Count > 0)
            {

                _catid = rtntbl.Rows[0]["CatId"].ToString();
                pfid = rtntbl.Rows[0]["Pfid"].ToString();
                Ea_Path = rtntbl.Rows[0]["Ea_Path"].ToString();
                samecodesubproduct = (bool)rtntbl.Rows[0]["samecodesubproduct"];
                samecodenotFound = (bool)rtntbl.Rows[0]["samecodenotFound"];
                wag_product_code = rtntbl.Rows[0]["wag_product_code"].ToString();
                SubstuyutePid = rtntbl.Rows[0]["SubstuyutePid"].ToString();
            }
            else
            {
                samecodesubproduct = true;
                samecodenotFound = false;
            }
            if (samecodenotFound == false && samecodesubproduct == false)
            {
                //_sPriceTable += string.Format("<tr><td style=\"color:red;\"width=\"28%\">{0}</td>", _CODE);
                //_sPriceTable += string.Format("<td style=\"color:red;\" colspan=\"2\">{0}</td></tr>", "Product Not Available.");
                //_sPriceTable += "<tr class=\"success\"><td colspan=\"3\"><br>RECOMMENDED REPLACEMENT<br><br></td></tr>";
                //_sPriceTable += "<tr><td colspan=\"3\">";
                //_sPriceTable += "<br>Order Code : " + "<span  style=\"color:green;font-weight: bold;\">" + wag_product_code + "</span> <br>";
                //string strurl = "ProductDetails.aspx?Pid=" + SubstuyutePid + "&amp;fid=" + pfid + "&amp;Cid=" + _catid + "&amp;path=" + Ea_Path;
                //_sPriceTable += "<br><a href =\"" + strurl + "\" style=\"font-weight: bold; text-decoration: none; color: #1589FF;\" > View Replacement Product </a>";
                //_sPriceTable += "<br><br></td></tr>";
               
                string strurl = "ProductDetails.aspx?Pid=" + SubstuyutePid + "&amp;fid=" + pfid + "&amp;Cid=" + _catid + "&amp;path=" + Ea_Path;
                _stmpl_records.SetAttribute("TBT_NIL_REPLACED", false);
                _stmpl_records.SetAttribute("TBT_REP_NIL_CODE", wag_product_code);
                _stmpl_records.SetAttribute("TBT_REP_EA_PATH", strurl);
              //  objErrorHandler.CreateLog("_sPriceTable" + _sPriceTable);
                _sPriceTable = "Replaced";
            }
            else //    if (samecodenotFound == false && samecodesubproduct == true)
            {
                _sPriceTable = "Not Replaced";
                return _sPriceTable;
                //_stmpl_records.SetAttribute("TBT_NIL_REPLACED", true);
                //_stmpl_records.SetAttribute("TBT_REP_NIL_CODE", _CODE);
                //_stmpl_records.SetAttribute("TBT_REP_STATUS", _StockStatus);
                //_sPriceTable += string.Format("<tr><td style=\"color:red;\"width=\"28%\">{0}</td>", _CODE);
               // _sPriceTable += string.Format("<td style=\"color:red;\" colspan=\"2\">{0}</td></tr>", "Product Temporarily Unavailable <br>Please Contact Us for more details");
            }
            
            //else
            //{
            //    _stmpl_records.SetAttribute("TBT_NIL_REPLACED", true);
            //    _stmpl_records.SetAttribute("TBT_REP_NIL_CODE", _CODE);
            //    _stmpl_records.SetAttribute("TBT_REP_STATUS", _StockStatus);
            //    //_sPriceTable += string.Format("<tr><td style=\"color:red;\"width=\"28%\">{0}</td>", _CODE);
            //    //_sPriceTable += string.Format("<td style=\"color:red;\" colspan=\"2\">{0}</td></tr>", "Product Temporarily Unavailable <br>Please Contact Us for more details");
            //}

            return _sPriceTable;

        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE USERS PRICE IN ROUND VALUE ***/
        /********************************************************************************/
        private decimal GetMyPrice(int ProductID)
        {
            decimal retval = 0.00M;
            try
            {
                string userid = HttpContext.Current.Session["USER_ID"].ToString();
                //if (!string.IsNullOrEmpty(userid))
                //{
                //    string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
                //    objHelperService.SQLString = sSQL;
                //    int pricecode = objHelperService.CI(objHelperService.GetValue("price_code"));

                //    if (!string.IsNullOrEmpty(userid))
                //    {
                //        string strquery = "";
                //        if (pricecode == 1)
                //        {
                //            strquery = string.Format("exec GetWESIncProductPrice {0},{1},{2}", ProductID, 1, HttpContext.Current.Session["USER_ID"]);
                //        }
                //        else
                //        {
                //            strquery = string.Format("exec GetWESProductPrice {0},{1},{2}", ProductID, 1, HttpContext.Current.Session["USER_ID"]);
                //        }

                //        DataSet DSprice = new DataSet();
                //        objHelperService.SQLString = strquery;
                //        retval = Math.Round(Convert.ToDecimal(objHelperService.GetValue("Numeric_Value")), 2);
                //    }
                //}
                retval = objHelperDB.GetProductPrice(ProductID, 1, userid);
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();

            }
            return retval;
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE STOCK STATUS USING PRODUCT ID ***/
        /********************************************************************************/
        private string GetStockStatus(int ProductID)
        {
            string Retval = "NO STATUS AVAILABLE";
            try
            {
                //string sSQL = string.Format("select PROD_STK_STATUS_DSC from WESTB_PRODUCT_ITEM WHERE PRODUCT_ID = {0}", ProductID);
                //objHelperService.SQLString = sSQL;
                //Retval = objHelperService.GetValue("PROD_STK_STATUS_DSC").ToString().Replace("_", " ");
		        DataTable objrbl =(DataTable)objHelperDB.GetGenericDataDB(ProductID.ToString(), "GET_SINGLE_PRODUCT_INVENTORY", HelperDB.ReturnType.RTTable);
                if (objrbl != null )
                {
                    Retval = objrbl.Rows[0]["STOCK_STATUS"].ToString().Replace("_", " ");                    
                }
                else
                    Retval = "NO STATUS AVAILABLE";
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
            }
            return Retval;
        }




        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRODUCT PRICE TABLE DETAILS USING PRODUCT ID ***/
        /********************************************************************************/
        private string GetProductPriceTable(int ProductID)
        {

            string _sPriceTable = string.Empty;
            try
            {
                string userid = HttpContext.Current.Session["USER_ID"].ToString();
                if (!string.IsNullOrEmpty(userid))
                {
                    //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
                    //objHelperService.SQLString = sSQL;
                    //int pricecode = objHelperService.CI(objHelperService.GetValue("price_code"));
                    int pricecode = objHelperDB.GetPriceCode(userid);
                    DataSet dsPriceTable = new DataSet();
                   
                    //SqlDataAdapter oDa = new SqlDataAdapter();
                    //oDa.SelectCommand = new SqlCommand();
                    //oDa.SelectCommand.CommandText = "GetPriceTable";
                    //oDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                    //oDa.SelectCommand.Connection = new SqlConnection(  );
                    //oDa.SelectCommand.Parameters.Clear();
                    //oDa.SelectCommand.Parameters.AddWithValue("@ProductID", ProductID);
                    //oDa.SelectCommand.Parameters.AddWithValue("@UserID", userid);
                    //oDa.Fill(dsPriceTable, "Price");
                   

                    dsPriceTable = objHelperDB.GetProductPriceTable(ProductID, Convert.ToInt32(userid.ToString()));
                    _sPriceTable = "";

                    int TotalCount = 0;
                    int RowCount = 0;


                    string[] P1 = null;
                    string[] P2 = null;
                   // objErrorHandler.CreateLog(pricecode + "--" + ProductID);
                    if (pricecode == 3)
                        foreach (DataRow oDr in dsPriceTable.Tables["Price"].Rows)
                        {
                            //_sPriceTable += string.Format("<tr><td class=\"bg_grey3\"><b>{0}</b></td><td align=\"center\">${1:0.00}</td><td align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);
                            P1 = oDr["Price1"].ToString().Split('.');
                            P2 = oDr["Price2"].ToString().Split('.');
                            if (P1[1].Length >= 4 && P2[1].Length >= 4)
                            {
                                if ((P1.Length > 0 && Convert.ToInt32(P1[1].Substring(2, 2)) > 1) || P2.Length > 0 && Convert.ToInt32(P2[1].Substring(2, 2)) > 1)
                                    _sPriceTable += string.Format("<tr><td class=\"bg_grey3\">{0}</td><td align=\"center\">${1:0.0000}</td><td align=\"center\">${2:0.0000}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);
                                else
                                    _sPriceTable += string.Format("<tr><td class=\"bg_grey3\">{0}</td><td align=\"center\">${1:0.00}</td><td align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);
                            }
                            else
                                _sPriceTable += string.Format("<tr><td class=\"bg_grey3\">{0}</td><td align=\"center\">${1:0.00}</td><td align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);

                        }
                    else
                    {
                        bool bLastRow = false;

                        TotalCount = dsPriceTable.Tables["Price"].Rows.Count;
                        RowCount = 0;

                        foreach (DataRow oDr in dsPriceTable.Tables["Price"].Rows)
                        {
                            RowCount = RowCount + 1;
                            if (RowCount == TotalCount && (pricecode >= 4) && oDr["QTY"].Equals("Your Price"))   // check whether it is Last Row
                            {
                                bLastRow = true;
                            }

                            string _color = bLastRow ? "bg_grey31" : "bg_grey3";
                            //_sPriceTable += string.Format("<tr><td class=\"{3}\"><b>{0}</b></td><td  class=\"{3}\" align=\"center\">${1:0.00}</td><td class=\"{3}\" align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                            P1 = oDr["Price1"].ToString().Split('.');
                            P2 = oDr["Price2"].ToString().Split('.');
                            if (P1[1].Length >= 4 && P2[1].Length >= 4)
                            {
                                if ((P1.Length > 0 && Convert.ToInt32(P1[1].Substring(2, 2)) > 1) || P2.Length > 0 && Convert.ToInt32(P2[1].Substring(2, 2)) > 1)
                                    _sPriceTable += string.Format("<tr><td class=\"{3}\">{0}</td><td  class=\"{3}\" align=\"center\">${1:0.0000}</td><td class=\"{3}\" align=\"center\">${2:0.0000}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                                else
                                    _sPriceTable += string.Format("<tr><td class=\"{3}\">{0}</td><td  class=\"{3}\" align=\"center\">${1:0.00}</td><td class=\"{3}\" align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                            }
                            else
                                _sPriceTable += string.Format("<tr><td class=\"{3}\">{0}</td><td  class=\"{3}\" align=\"center\">${1:0.00}</td><td class=\"{3}\" align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                        }
                    }

                }
            }


            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                _sPriceTable="-1";
            }
            //objErrorHandler.CreateLog(_sPriceTable);
            return _sPriceTable;
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRODUCT DETAILS BASED ON FAMILY ID AND PRODUCT ID ***/
        /********************************************************************************/

        private void GetProductDetails(int ProductID, int FamilyID, StringTemplate st)
        {
            //SqlConnection oCon = new SqlConnection(_DBConnectionString);
            //SqlDataAdapter oDa = new SqlDataAdapter("GetProductDetails", oCon);
            //oDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            //oDa.SelectCommand.Parameters.Clear();
            //oDa.SelectCommand.Parameters.AddWithValue("@ProductID", ProductID);
            //oDa.SelectCommand.Parameters.AddWithValue("@FamilyID", FamilyID);
            DataSet oDs = new DataSet();
            //oDa.Fill(oDs, "PrdDetails");
            DataSet tmp = (DataSet)HttpContext.Current.Session["FamilyProduct"];
            try
            {
                if (tmp != null)
                {
                    DataRow[] Dr = tmp.Tables[0].Select("ATTRIBUTE_TYPE=1");
                    if (Dr.Length > 0)
                    {
                        oDs.Tables.Add(Dr.CopyToDataTable());
                        oDs.Tables[0].TableName = "PrdDetails";
                    }
                }

                if (oDs != null & oDs.Tables.Count > 0)
                {
                    // DataRow[] Familyspec2 = oDs.Tables[0].Select("ATTRIBUTE_ID in (13,4,240,241,2,51,18)");

                    foreach (DataRow oDr in oDs.Tables["PrdDetails"].Rows)
                    {
                        if (oDr["ATTRIBUTE_NAME"].ToString() != "Long Description")
                        {
                            if (!string.IsNullOrEmpty(oDr["STRING_VALUE"].ToString()))
                            {
                                ProductDetails oPrdDet = new ProductDetails();
                                oPrdDet.AttributeID = Convert.ToInt32(oDr["ATTRIBUTE_ID"]);
                                oPrdDet.AttributeName = oDr["ATTRIBUTE_NAME"].ToString();
                                oPrdDet.SpecValue = oDr["STRING_VALUE"].ToString();
                                st.SetAttribute("TBT_PRODDETAILS", oPrdDet);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();

            }
            
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE DESCRIPTION DETAILS OF PRODUCTS ***/
        /********************************************************************************/
        private void GetProductDesc(int ProductID, int FamilyID, StringTemplate st)
        {
            //SqlConnection oCon = new SqlConnection(_DBConnectionString);
            //SqlDataAdapter oDa = new SqlDataAdapter("GetFamilyDetails", oCon);
            //oDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            //oDa.SelectCommand.Parameters.Clear();
            //oDa.SelectCommand.Parameters.AddWithValue("@FamilyID", FamilyID);
            DataSet oDs = new DataSet();

            DataSet tmp = (DataSet)HttpContext.Current.Session["FamilyProduct"];
            try
            {
                if (tmp != null)
                {
                    DataRow[] Dr = tmp.Tables[0].Select("ATTRIBUTE_TYPE=7");
                    if (Dr.Length > 0)
                    {
                        oDs.Tables.Add(Dr.CopyToDataTable());
                        oDs.Tables[0].TableName = "PrdDetails";
                    }
                }
                if (oDs != null & oDs.Tables.Count > 0)
                {
                    // DataRow[] Familyspec2 = oDs.Tables[0].Select("ATTRIBUTE_ID in (13,4,240,241,2,51,18)");

                    foreach (DataRow oDr in oDs.Tables["PrdDetails"].Rows)
                    {
                        ProductDetails oPrdDet = new ProductDetails();
                        oPrdDet.SpecValue = oDr["STRING_VALUE"].ToString();
                        //string attr = "TBT_" + oPrdDet.AttributeName;
                        st.SetAttribute("TBT_PRODDESC", oPrdDet);
                    }
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg=e;
                objErrorHandler.CreateLog();
            }
            
        
       
            //oDa.Fill(oDs, "PrdDetails");
            //if (oDs != null && oDs.Tables["PrdDetails"].Rows.Count > 0 && oDs.Tables.Count > 0)
            //{
            //    StringBuilder sBuilder = new StringBuilder();
            //    DataRow[] Familyspec2 = oDs.Tables[0].Select("ATTRIBUTE_ID in (13,4,240,241,2,51,18)");
            //    int[] AttributeIdList = new int[7];
            //    AttributeIdList[0] = 13;
            //    AttributeIdList[1] = 4;
            //    AttributeIdList[2] = 240;
            //    AttributeIdList[3] = 241;
            //    AttributeIdList[4] = 2;
            //    AttributeIdList[5] = 51;
            //    AttributeIdList[6] = 18;

            //    if (Familyspec2.Length > 0)
            //    {
            //        for (int i = 0; i < AttributeIdList.Length; i++)
            //        {
            //            foreach (DataRow oDr in oDs.Tables["PrdDetails"].Rows)
            //            {
            //                if (oDr["STRING_VALUE"].ToString().Length > 0)
            //                {

            //                    string t = oDr["ATTRIBUTE_ID"].ToString();
            //                    if (oDr["ATTRIBUTE_ID"].ToString() == AttributeIdList[i].ToString())
            //                    {
            //                        ProductDetails oPrdDet = new ProductDetails();
            //                        oPrdDet.AttributeID = Convert.ToInt32(oDr["ATTRIBUTE_ID"]);
            //                        oPrdDet.AttributeName = oDr["ATTRIBUTE_NAME"].ToString();

            //                        oPrdDet.SpecValue = oDr["STRING_VALUE"].ToString().Replace("\r", "&nbsp;").Replace("\n", "<br/>");
            //                        //string attr = "TBT_" + oPrdDet.AttributeName;
            //                        st.SetAttribute("TBT_PRODDESC", oPrdDet);
            //                    }
            //                }
            //            }
            //        }
            //    }

            //    //foreach (DataRow oDr in oDs.Tables["PrdDetails"].Rows)
            //    //{
            //    //    ProductDetails oPrdDet = new ProductDetails();
            //    //    oPrdDet.AttributeID = Convert.ToInt32(oDr["ATTRIBUTE_ID"]);
            //    //    oPrdDet.AttributeName = oDr["ATTRIBUTE_NAME"].ToString();

            //    //    oPrdDet.SpecValue = oDr["STRING_VALUE"].ToString();
            //    //    //string attr = "TBT_" + oPrdDet.AttributeName;
            //    //    st.SetAttribute("TBT_PRODDESC", oPrdDet);
            //    //}
            //}
            //else
            //{
            //    string sql_query = "select string_value from TB_PROD_SPECS where ATTRIBUTE_ID=(select ATTRIBUTE_ID from TB_ATTRIBUTE where ATTRIBUTE_NAME='Description') and product_id=" + ProductID;
            //    SqlDataAdapter oDa1 = new SqlDataAdapter(sql_query, oCon);

            //    DataSet oDs1 = new DataSet();
            //    oDa1.Fill(oDs1, "Productdetails");
            //    if (oDs1 != null)
            //    {
            //        foreach (DataRow oDr in oDs1.Tables["Productdetails"].Rows)
            //        {
            //            ProductDetails oPrdDet = new ProductDetails();
            //            oPrdDet.SpecValue = oDr["STRING_VALUE"].ToString();
            //            //string attr = "TBT_" + oPrdDet.AttributeName;
            //            st.SetAttribute("TBT_PRODDESC", oPrdDet);
            //        }
            //    }
            //}
               
            }


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRODUCT IMAGES IN DIFFERENT FORMATS BASED ON PARAMETERS ***/
        /********************************************************************************/

        private void GetMultipleImages(int ProductID, int FamilyID,int SubFamilyId, StringTemplate st)
        {
            ConnectionDB objConnectionDB = new ConnectionDB();
            string strfile = HttpContext.Current.Server.MapPath("ProdImages");
            //SqlConnection oCon = new SqlConnection(_DBConnectionString);
            SqlDataAdapter oDa = new SqlDataAdapter("GetProductImages", objConnectionDB.GetConnection());
            oDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            oDa.SelectCommand.Parameters.Clear();
            oDa.SelectCommand.Parameters.AddWithValue("@ProductID", ProductID);
            oDa.SelectCommand.Parameters.AddWithValue("@FamilyID", FamilyID);
            oDa.SelectCommand.Parameters.AddWithValue("@SubFamilyId", SubFamilyId);
            DataSet oDs = new DataSet();
            oDa.Fill(oDs, "Images");
            bool firstImg = true;
            objConnectionDB.CloseConnection();
            try
            { 
            if (oDs != null)
            {
                foreach (DataRow oDr in oDs.Tables["Images"].Rows)
                {
                    ProductImage oPrd = new ProductImage();
                    if (File.Exists(string.Format(@"{0}/{1}", strfile, oDr["STRING_VALUE"].ToString().Replace("\\", "/"))))
                    {
                        oPrd.LargeImage = oDr["STRING_VALUE"].ToString().Replace("\\", "/");                        

                        //oPrd.Thumpnail = oPrd.LargeImage.ToLower().Replace("_images", "_th50");
                        //oPrd.SmallImage = oPrd.LargeImage.ToLower().Replace("_images", "_th");
                        //oPrd.MediumImage = oPrd.LargeImage.ToLower().Replace("_images", "_images_200");
                        if ((oPrd.LargeImage.ToLower().Contains("_th")))
                        {
                            string tmpimg = oDr["STRING_VALUE"].ToString().Replace("\\", "/");
                            oPrd.LargeImage = objHelperService.SetImageFolderPath(tmpimg, "_th", "_images");
                            oPrd.Thumpnail = objHelperService.SetImageFolderPath(tmpimg, "_th", "_th50");
                            oPrd.SmallImage = objHelperService.SetImageFolderPath(tmpimg, "_th", "_th");
                            oPrd.MediumImage = objHelperService.SetImageFolderPath(tmpimg, "_th", "_images_200");
                        }                        
                        else
                        {
                            oPrd.Thumpnail = objHelperService.SetImageFolderPath(oPrd.LargeImage.ToLower(), "_Images", "_th50");
                            oPrd.SmallImage = objHelperService.SetImageFolderPath(oPrd.LargeImage.ToLower(), "_Images", "_th");
                            oPrd.MediumImage = objHelperService.SetImageFolderPath(oPrd.LargeImage.ToLower(), "_Images", "_images_200");
                        }
                        
                        st.SetAttribute("TBT_MULTIIMAGES", oPrd);
                        if (firstImg)
                        {
                            st.SetAttribute("TBT_TWEB_IMAGE1", oPrd.MediumImage);
                            firstImg = false;
                        }
                    }
                }
            }
            }
            
                catch (Exception e)
                {
                    objErrorHandler.ErrorMsg=e;
                    objErrorHandler.CreateLog();
                }
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE FAMILY PAGE PRODUCT IMAGES IN DIFFERENT FORMATS ***/
        /********************************************************************************/
        private void GetFamilyMultipleImages(int FamilyID, StringTemplate st, DataSet oDs)
        {
            string strfile = HttpContext.Current.Server.MapPath("ProdImages");
            //SqlConnection oCon = new SqlConnection(_DBConnectionString);
            //SqlDataAdapter oDa = new SqlDataAdapter("GetFamilyImages", oCon);
            //oDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            //oDa.SelectCommand.Parameters.Clear();
            //oDa.SelectCommand.Parameters.AddWithValue("@FamilyID", FamilyID);
          //  DataSet oDs = new DataSet();
            //oDa.Fill(oDs, "Images");            
            DataTable dt = new DataTable();
            DataRow[] dr;
          //  oDs = (DataSet)HttpContext.Current.Session["FamilyProduct"];
            bool firstImg = true;
            try
            {
                if (oDs != null)
                {
                    dr = oDs.Tables[0].Select("ATTRIBUTE_TYPE=9 And ATTRIBUTE_ID Not in (746, 747)", "ATTRIBUTE_ID");
                    if (dr.Length > 0)
                    {
                        dt = dr.CopyToDataTable();
                        foreach (DataRow oDr in dt.Rows)
                        {
                            ProductImage oPrd = new ProductImage();
                            if (File.Exists(string.Format(@"{0}/{1}", strfile, oDr["STRING_VALUE"].ToString().Replace("\\", "/"))))
                            {
                                oPrd.LargeImage = oDr["STRING_VALUE"].ToString().Replace("\\", "/");
                                oPrd.Thumpnail = objHelperService.SetImageFolderPath(oPrd.LargeImage.ToLower(), "_Images", "_th50");
                                oPrd.SmallImage = objHelperService.SetImageFolderPath(oPrd.LargeImage.ToLower(), "_Images", "_th");
                                oPrd.MediumImage = objHelperService.SetImageFolderPath(oPrd.LargeImage.ToLower(), "_Images", "_images_200");

                                //oPrd.Thumpnail = oPrd.LargeImage.ToLower().Replace("_images", "_th50");
                                //oPrd.SmallImage = oPrd.LargeImage.ToLower().Replace("_images", "_th");
                                //oPrd.MediumImage = oPrd.LargeImage.ToLower().Replace("_images", "_images_200");
                                st.SetAttribute("TBT_MULTIIMAGES", oPrd);
                                if (firstImg)
                                {
                                    st.SetAttribute("TBT_TFWEB_IMAGE1", oPrd.MediumImage);
                                    st.SetAttribute("TBT_TFWEB_LIMAGE", oPrd.LargeImage);
                                    firstImg = false;
                                }
                            }
                        }

                    }
                    else
                    {
                      //  string tempstr = "";
                       // string tempstr1 = "";
                     //   string[] tempstrs = null;
                        dr = oDs.Tables[0].Select("ATTRIBUTE_TYPE=9 And ATTRIBUTE_ID in (746) And STRING_VALUE<>'noimage.gif'", "ATTRIBUTE_ID");
                        if (dr.Length > 0)
                        {
                            dt = dr.CopyToDataTable();
                            foreach (DataRow oDr in dt.Rows)
                            {

                                ProductImage oPrd = new ProductImage();
                                if (File.Exists(string.Format(@"{0}/{1}", strfile, oDr["STRING_VALUE"].ToString().Replace("\\", "/"))))
                                {
                                    string img = oDr["STRING_VALUE"].ToString().Replace("\\", "/");
                                    oPrd.LargeImage = objHelperService.SetImageFolderPath(img, "_th", "_images");

                                    oPrd.Thumpnail = objHelperService.SetImageFolderPath(img, "_th", "_th50");
                                    oPrd.SmallImage = objHelperService.SetImageFolderPath(img, "_th", "_th");
                                    oPrd.MediumImage = objHelperService.SetImageFolderPath(img, "_Th", "_images_200");

                                    //oPrd.Thumpnail = oPrd.LargeImage.ToLower().Replace("_images", "_th50");
                                    //oPrd.SmallImage = oPrd.LargeImage.ToLower().Replace("_images", "_th");
                                    //oPrd.MediumImage = oPrd.LargeImage.ToLower().Replace("_images", "_images_200");
                                    st.SetAttribute("TBT_MULTIIMAGES", oPrd);
                                    if (firstImg)
                                    {
                                        st.SetAttribute("TBT_TFWEB_IMAGE1", oPrd.MediumImage);
                                        st.SetAttribute("TBT_TFWEB_LIMAGE", oPrd.LargeImage);
                                        firstImg = false;
                                    }
                                }

                            }

                        }
                    }

                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
            }
          
            }



        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK WEATHER AN URL ALREADY EXIST OR NOT  ***/
        /********************************************************************************/
        private static bool UrlExists(string url)
        {
            bool retval = false;
            try
            {
                new System.Net.WebClient().DownloadData(url);
                retval = true;
            }
            catch (System.Net.WebException)
            {


            }
            return retval;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO BUILD ROW WISE TEMPLATE RECORDS   ***/
        /********************************************************************************/
        private void BuildRecordsTemplateRow()
        {
            TBWDataList[] lstrecords = new TBWDataList[0];
            //Build the cell inner body of the HTML
            _stg_records = new StringTemplateGroup(_skin_records, _SkinRootPath);
            DataSet dsrecords = null;
            string strFile = HttpContext.Current.Server.MapPath("ProdImages");
            if (_GDataSet != null && _skin_sql_records.Length == 0)
            {
                dsrecords = _GDataSet;
            }
            else if (_Package == "TOP" || _Package == "TOPLOG")
            {         
                DataSet tmpds=EasyAsk.GetCategoryAndBrand("MainCategory") ;
                if(tmpds!=null)
                { // remove WES NEWS MENU 
                  DataRow[] dr= tmpds.Tables[0].Select("CATEGORY_ID<>'" + WesNewsCategoryId + "'","CATEGORY_NAME" );
                    if (dr.Length>0)
                    {
                      dsrecords  =new DataSet();
                      dsrecords.Tables.Add(dr.CopyToDataTable().Copy());   
                    }
                }
                else
                    dsrecords=null;                 
                  
            }

            else if (_Package == "BOTTOM" )
            {
                DataSet tmpds = EasyAsk.GetCategoryAndBrand("MainCategory");
                if (tmpds != null)
                { // remove WES NEWS MENU 
                    DataRow[] dr = tmpds.Tables[0].Select("CATEGORY_ID<>'" + WesNewsCategoryId + "'", "CATEGORY_NAME");
                    if (dr.Length > 0)
                    {
                        dsrecords = new DataSet();
                        dsrecords.Tables.Add(dr.CopyToDataTable().Copy());
                    }
                }
                else
                    dsrecords = null;

            }

            else if (_Package == "BROWSEBYCATEGORY" || _Package == "BROWSEBYBRAND" || _Package == "BROWSEBYPRODUCT") // unwanted db calls
            {
                dsrecords = null;
            }
            else
            {
                dsrecords = GetDataSet(_skin_sql_records, _skin_sql_type_records, _skin_sql_param_records);
            }
            _stg_container = new StringTemplateGroup(_skin_container, _SkinRootPath);

            if (dsrecords != null)
            {//dsrecords.Tables.Count - 1
                if (dsrecords.Tables[0].Rows.Count > 0)
                {

                    lstrecords = new TBWDataList[dsrecords.Tables[0].Rows.Count];
                    int ictrecords = 0, ictcol = 1; string strValue = "";
                    foreach (DataRow dr in dsrecords.Tables[0].Rows)
                    {
                        if (_Package == "CATEGORYLISTIMG")
                        {
                            if (HttpContext.Current.Request.QueryString["tsb"] == null && HttpContext.Current.Request.QueryString["cid"] != null && HttpContext.Current.Request.QueryString["cid"] != "")
                                _stmpl_records = _stg_records.GetInstanceOf(_Package + "\\" + _skin_records.ToString() + "1");
                            else
                                _stmpl_records = _stg_records.GetInstanceOf(_Package + "\\" + _skin_records);
                        }
                        else
                            
                            _stmpl_records = _stg_records.GetInstanceOf(_Package + "\\" + _skin_records);
                       

                        _stmpl_records.SetAttribute("TBT_ECOMENABLED", IsEcomenabled());
                        if (HttpContext.Current.Request.QueryString["cid"] != null)
                        {
                            _stmpl_records.SetAttribute("TBT_CAT_ID", HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["cid"].ToString()));                            
                        }
                        if (HttpContext.Current.Request.QueryString["tsb"] != null)
                        {
                            _stmpl_records.SetAttribute("TBT_TOSUITE_BRAND", HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()));
                            _stmpl_records.SetAttribute("TBT_TOSUITE_BRAND1", HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsb"].ToString()));
                        }
                        if (_Package == "CATEGORYLISTIMG")
                        {
                            if (HttpContext.Current.Session["EA"] != null)
                            {
                                _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(HttpContext.Current.Session["EA"].ToString())));
                            }
                        }
                        if (HttpContext.Current.Request.QueryString["sl1"] != null)
                            _stmpl_records.SetAttribute("TBT_TOSUITE_SL1", HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["sl1"].ToString()));
                        if (HttpContext.Current.Request.QueryString["sl2"] != null)
                            _stmpl_records.SetAttribute("TBT_TOSUITE_SL2", HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["sl2"].ToString()));
                        string dccolname = string.Empty;
                        string dccolrplce = string.Empty;
                        string drcolname = string.Empty;
                        string dccolnameupp = string.Empty;
                        foreach (DataColumn dc in dsrecords.Tables[0].Columns)
                        {
                            //_stmpl_records.SetAttribute(dc.ColumnName, dr[dc.ColumnName].ToString());     
                            dccolname = string.Empty;
                            dccolname = dc.ColumnName.ToString();
                            dccolrplce = string.Empty;
                            dccolrplce = dccolname.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper();
                            drcolname = dr[dccolname].ToString();
                            dccolnameupp = dccolname.ToUpper();
                            if ((_Package == "TOP" || _Package == "TOPLOG") && dccolnameupp == "CATEGORY_NAME" && drcolname.Length > 8)
                            {
                                string sttr = dr[dccolname].ToString();
                                int indx = sttr.IndexOf(" ", 7);
                                if (indx >= 7)
                                    sttr = sttr.Substring(0, indx) + "<br/>" + sttr.Substring(indx + 1);
                                else if (sttr == "VCR COMPONENTS")
                                    sttr = "VCR<br/>COMPONENTS";
                                _stmpl_records.SetAttribute("TBT_" + dccolrplce, sttr);
                            }
                            else
                            {
                                if (dccolnameupp == "IMAGE_FILE")
                                {
                                    FileInfo Fil = new FileInfo(strFile + drcolname);
                                    if (Fil.Exists)
                                    {
                                        _stmpl_records.SetAttribute("TBT_" + dccolrplce, drcolname);
                                    }
                                    else
                                    {
                                        _stmpl_records.SetAttribute("TBT_" + dccolrplce, "");
                                    }
                                }
                                else
                                {
                                    if (dccolnameupp == "CUSTOM_NUM_FIELD3" && drcolname == "2")
                                    {
                                        _stmpl_records.SetAttribute("TBT_" + dccolrplce, drcolname.Replace("\r", "<br/>").Replace("\n", "&nbsp;"));// + "&bypcat=1");
                                    }
                                    else if (dccolnameupp == "EA_PATH" && (_Package == "TOP" || _Package == "TOPLOG"))
                                    {
                                        _stmpl_records.SetAttribute("TBT_EAPATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(dr["EA_PATH"].ToString())));
                                    }
                                    else
                                    {
                                        if (dccolnameupp == "TOSUITE_MODEL" || dccolnameupp == "TOSUITE_BRAND")
                                        {
                                            _stmpl_records.SetAttribute("TBT_" + dccolrplce, HttpUtility.UrlEncode(drcolname.Replace("\r", "<br/>").Replace("\n", "&nbsp;")));
                                            _stmpl_records.SetAttribute("TBT_" + dccolrplce + "1", drcolname.Replace("\r", "<br/>").Replace("\n", "&nbsp;"));
                                        }
                                        else
                                        {
                                            _stmpl_records.SetAttribute("TBT_" + dccolrplce, drcolname.Replace("\r", "<br/>").Replace("\n", "&nbsp;"));
                                        }
                                    }
                                }
                            }

                        }
                        if (_Package == "CSFAMILYPAGE")
                        {
                            _stmpl_records.SetAttribute("TBT_FAMILYIMAGE", "My Family");
                        }
                        strValue = strValue + _stmpl_records.ToString();
                        if (ictrecords == 10 && _Package == "TOP")
                        {
                            // strValue += "</ul></div></td><tr><td><div id=\"navcontainer\">";
                        }
                        if (ictcol == _grid_cols)
                        {
                            _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + _skin_container);
                            _stmpl_container.SetAttribute("TBWDataList", strValue);
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
                            ictrecords++; ictcol = 1; strValue = "";
                        }
                        else
                        {
                            ictcol++;
                        }
                    }
                    if (strValue != "")
                    {
                        _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + _skin_container);
                        _stmpl_container.SetAttribute("TBWDataList", strValue);
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
                        ictrecords++; ictcol = 1; strValue = "";
                    }
                }
            }
            _dict_inner_html[_skin_body_attribute] = lstrecords;
        }

        //private DataSet GetDataSet(string SQLQuery)
        //{
        //    DataSet ds = new DataSet();
        //    SqlDataAdapter da = new SqlDataAdapter(SQLQuery, _DBConnectionString);
        //    da.Fill(ds, "generictable");
        //    return ds;
        //}

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE DATASET DETAILS BASED ON PARAMETERS  ***/
        /********************************************************************************/
        private DataSet GetDataSet(string SQLQuery, string SQLType, string SQLParam)
        {
            DataSet ds = new DataSet();
            try
            {
                if (paraValue != "" && SQLParam != "")
                    SQLQuery = SQLQuery.Replace(SQLParam, paraValue);
                if (_Package == "BROWSEBYCATEGORY" && HttpContext.Current.Session["PARAFILTER"] == "Value")
                {
                    if (SQLQuery.Contains("order by tc.category_id"))
                        SQLQuery = SQLQuery.Replace("order by tc.category_id", " and tC.CATEGORY_ID in (select distinct category_id from tb_family where family_id in (select family_id from tb_prod_family where product_id in  (select product_id from TBWC_SEARCH_PROD_LIST where user_session_id = '" + HttpContext.Current.Session.SessionID + "')))" + " order by tc.category_id");
                }
                ConnectionDB objConnectionDB = new ConnectionDB();
                SqlDataAdapter da = new SqlDataAdapter(SQLQuery, objConnectionDB.GetConnection());
                da.Fill(ds, "generictable");
                objConnectionDB.CloseConnection();
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg=e;
                objErrorHandler.CreateLog();
            }          
            return ds;
        }
        //private DataSet ProductFilterFlatTable(DataSet flatDataset)
        //{
        //    {
        //        StringBuilder SQLstring = new StringBuilder();
        //        DataSet oDsProductFilter = new DataSet();
        //        string SQLString = " SELECT PRODUCT_FILTERS FROM TB_CATALOG WHERE  CATALOG_ID = " + CATALOG_ID + " ";
        //        SqlDataAdapter da = new SqlDataAdapter(SQLString, _DBConnectionString);
        //        da.Fill(oDsProductFilter);
        //        string sProductFilter = string.Empty;
        //        if (oDsProductFilter.Tables[0].Rows.Count > 0 && oDsProductFilter.Tables[0].Rows[0].ItemArray[0].ToString() != string.Empty)
        //        {
        //            sProductFilter = oDsProductFilter.Tables[0].Rows[0].ItemArray[0].ToString();
        //            XmlDocument xmlDOc = new XmlDocument();
        //            xmlDOc.LoadXml(sProductFilter);
        //            XmlNode rNode = xmlDOc.DocumentElement;

        //            if (rNode.ChildNodes.Count > 0)
        //            {
        //                for (int i = 0; i < rNode.ChildNodes.Count; i++)
        //                {
        //                    XmlNode TableDataSetNode = rNode.ChildNodes[i];

        //                    if (TableDataSetNode.HasChildNodes)
        //                    {
        //                        if (TableDataSetNode.ChildNodes[2].InnerText == " ")
        //                        {
        //                            TableDataSetNode.ChildNodes[2].InnerText = "=";
        //                        }
        //                        if (TableDataSetNode.ChildNodes[0].InnerText == " ")
        //                        {
        //                            TableDataSetNode.ChildNodes[0].InnerText = "0";
        //                        }
        //                        string stringval = TableDataSetNode.ChildNodes[3].InnerText.Replace("'", "''");
        //                        DataSet attribuetypeDS = new DataSet();
        //                        string sSQLString = " SELECT ATTRIBUTE_DATATYPE FROM TB_ATTRIBUTE WHERE  ATTRIBUTE_ID = " + Convert.ToInt32(TableDataSetNode.ChildNodes[0].InnerText) + " ";
        //                        SqlDataAdapter das = new SqlDataAdapter(sSQLString, _DBConnectionString);
        //                        das.Fill(attribuetypeDS);
        //                        if (attribuetypeDS.Tables[0].Rows[0].ItemArray[0].ToString().ToUpper().Contains("TEX") == true || attribuetypeDS.Tables[0].Rows[0].ItemArray[0].ToString().ToUpper().Contains("DATE") == true)
        //                        {

        //                            if (TableDataSetNode.ChildNodes[4].InnerText != "NONE")
        //                            {
        //                                SQLstring.Append("SELECT DISTINCT PRODUCT_ID FROM [PRODUCT SPECIFICATION](" + CATALOG_ID + ") WHERE (STRING_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ") " + "\n");
        //                            }
        //                            else
        //                            {
        //                                SQLstring.Append("SELECT DISTINCT PRODUCT_ID FROM [PRODUCT SPECIFICATION](" + CATALOG_ID + ") WHERE (STRING_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ")" + "\n");
        //                            }
        //                        }
        //                        else if (attribuetypeDS.Tables[0].Rows[0].ItemArray[0].ToString().ToUpper().Contains("DECI") == true || attribuetypeDS.Tables[0].Rows[0].ItemArray[0].ToString().ToUpper().Contains("NUM") == true)
        //                        {
        //                            if (TableDataSetNode.ChildNodes[4].InnerText != "NONE")
        //                            {
        //                                SQLstring.Append("SELECT DISTINCT PRODUCT_ID FROM [PRODUCT SPECIFICATION](" + CATALOG_ID + ") WHERE  (NUMERIC_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ") " + "\n");
        //                            }
        //                            else
        //                            {
        //                                SQLstring.Append("SELECT DISTINCT PRODUCT_ID FROM [PRODUCT SPECIFICATION](" + CATALOG_ID + ") WHERE (NUMERIC_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ")" + "\n");
        //                            }
        //                        }


        //                    }
        //                    if (TableDataSetNode.ChildNodes[4].InnerText == "NONE")
        //                    {
        //                    }
        //                    if (TableDataSetNode.ChildNodes[4].InnerText == "AND")
        //                    {
        //                        SQLstring.Append(" INTERSECT \n");
        //                    }
        //                    if (TableDataSetNode.ChildNodes[4].InnerText == "OR")
        //                    {
        //                        SQLstring.Append(" UNION \n");
        //                    }

        //                }

        //            }

        //        }
        //        string productFiltersql = SQLstring.ToString();
        //        // Boolean variableFilter = false;
        //        if (productFiltersql.Length > 0)
        //        {
        //            string s = "SELECT PRODUCT_ID FROM [PRODUCT FAMILY](" + CATALOG_ID + ") WHERE CATALOG_ID=" + CATALOG_ID + " AND PRODUCT_ID IN\n" +
        //                  "(\n";// +
        //            //"SELECT DISTINCT PRODUCT_ID\n" +
        //            //"FROM [PRODUCT SPECIFICATION](" + CATALOG_ID + ") \n" +
        //            //"WHERE\n";
        //            productFiltersql = s + productFiltersql + "\n)";
        //            SqlDataAdapter dad = new SqlDataAdapter(productFiltersql, _DBConnectionString);
        //            dad.Fill(oDsProductFilter);

        //            bool available = false;

        //            for (int rowCount = 0; rowCount < flatDataset.Tables[0].Rows.Count; rowCount++)
        //            {//foreach (DataRow odr in flatDataset.Tables[0].Rows)
        //                DataRow odr = flatDataset.Tables[0].Rows[rowCount];
        //                available = false;
        //                foreach (DataRow dr in oDsProductFilter.Tables[0].Rows)
        //                {
        //                    if (dr["PRODUCT_ID"].ToString() == odr["PRODUCT_ID"].ToString())
        //                    {
        //                        available = true;
        //                    }

        //                }
        //                if (available == false)
        //                {
        //                    string cmdd = " DELETE FROM TBWC_SEARCH_PROD_LIST WHERE PRODUCT_ID = " + odr["PRODUCT_ID"].ToString() + " AND USER_SESSION_ID='" + HttpContext.Current.Session.SessionID + "'";
        //                    SqlConnection _SQLConn = new SqlConnection(_DBConnectionString);
        //                    SqlCommand pscmd = new SqlCommand(cmdd, _SQLConn);
        //                    pscmd.CommandType = CommandType.Text;
        //                    int valr = pscmd.ExecuteNonQuery();
        //                    odr.Delete();
        //                    flatDataset.AcceptChanges();
        //                    rowCount--;
        //                }

        //            }

        //        }
        //    }
        //    return flatDataset;
        //}

        //private bool IsPDFAttached()
        //{
        //    bool retvalue = false;

        //    if (paraCID != null && !string.IsNullOrEmpty(paraCID.Trim()))
        //    {
        //        string sSQL = "SELECT IMAGE_FILE2 FROM TB_CATEGORY WHERE CATEGORY_ID = '" + paraCID + "'";
        //        objHelperService.SQLString = sSQL;


        //    }

        //    return retvalue;
        //}
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK ECOM  IS ENABLED OR NOT  ***/
        /********************************************************************************/
        private bool IsEcomenabled()
        {

            string userid = HttpContext.Current.Session["USER_ID"].ToString();
            return objHelperService.GetIsEcomEnabled(userid);
        }

        public void ST_Product_Download(string _pid)
        {
            string rtnstr = string.Empty;
            StringTemplateGroup _stg_container = null;
          //  StringTemplateGroup _stg_records = null;
            StringTemplate _stmpl_container = null;
           // StringTemplate _stmpl_records = null;
           // StringTemplate _stmpl_records1 = null;
           // StringTemplate _stmpl_recordsrows = null;
            TBWDataList[] lstrecords = new TBWDataList[0];
            TBWDataList[] lstrows = new TBWDataList[0];

            StringTemplateGroup _stg_container1 = null;
            StringTemplateGroup _stg_records1 = null;
            TBWDataList1[] lstrecords1 = new TBWDataList1[0];
            TBWDataList1[] lstrows1 = new TBWDataList1[0];

            

            DataTable dt = new DataTable();
            DataRow[] dr = null;

            int ictrecords = 0;
            downloadST = "";
            isdownload = false;
            if (_pid != "")
            {
                _stg_container = new StringTemplateGroup("main", SkinRootPath);
                lstrecords = new TBWDataList[1];

                DataSet TempEADs = objFamilyServices.GetFamilyPageProduct(_pid, "PRODUCT_ATTACHMENT");
                if (TempEADs != null && TempEADs.Tables.Count > 0 && TempEADs.Tables[0].Rows.Count > 0)
                {
                    //TempEADs.Tables[0].Columns.Add("FAMILY_NAME");



                   
                      
                        _stmpl_container = _stg_container.GetInstanceOf(_Package  + "\\" + "DownloadMain");
                        
                          
                                rtnstr = ST_Productpage_Download( TempEADs.Tables[0]);
                                if (rtnstr != "")
                                {
                                    lstrecords[ictrecords] = new TBWDataList(rtnstr.ToString());
                                    ictrecords = ictrecords + 1;
                                }

                       
                       
                        _stmpl_container.SetAttribute("TBWDataList", lstrecords);

                        if (ictrecords > 0)
                        {
                            _stmpl_container.SetAttribute("DOWNLOAD_MAIL", ST_Downloads_Update(true));
                            downloadST = _stmpl_container.ToString();
                            isdownload = true;

                        }
                        else
                        {
                            _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "DownloadMain");
                            _stmpl_container.SetAttribute("DOWNLOAD_MAIL", ST_Downloads_Update(false));
                            downloadST = _stmpl_container.ToString();
                            isdownload = true;
                        }

                 
                }
                else
                {
                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "DownloadMain");
                    _stmpl_container.SetAttribute("DOWNLOAD_MAIL", ST_Downloads_Update(false));
                    downloadST = _stmpl_container.ToString();
                    isdownload = true;
                }
            }
            
            


        }

        public string ST_Productpage_Download(DataTable Adt)
        {
            StringTemplateGroup _stg_container = null;
            StringTemplateGroup _stg_records = null;
            StringTemplate _stmpl_container = null;
            StringTemplate _stmpl_records = null;

            TBWDataList[] lstrecords = new TBWDataList[0];
            TBWDataList[] lstrows = new TBWDataList[0];


            TBWDataList1[] lstrecords1 = new TBWDataList1[0];
            TBWDataList1[] lstrows1 = new TBWDataList1[0];


            string strPDFFiles1 = HttpContext.Current.Server.MapPath("attachments");
            string strImgFiles1 = HttpContext.Current.Server.MapPath("ProdImages");
            long FileInKB;
            string[] file = null;
            string strfile = string.Empty;
            if (Adt != null && Adt.Rows.Count > 0)
            {




                DataSet dscat = new DataSet();


                try
                {
                    _stg_records = new StringTemplateGroup("cell", _SkinRootPath);
                    _stg_container = new StringTemplateGroup("row", _SkinRootPath);


                    lstrecords = new TBWDataList[Adt.Rows.Count + 1];



                    int ictrecords = 0;

                    foreach (DataRow dr in Adt.Rows)//For Records
                    {
                        strfile = dr["PRODUCT_ATT_FILE"].ToString().Replace(@"\\", "/");


                        FileInfo Fil;

                       

                        if ((dr["PRODUCT_ATT_FILE"].ToString().ToLower().Contains(".jpg")))
                            Fil = new FileInfo(strImgFiles1 + dr["PRODUCT_ATT_FILE"].ToString());
                        else
                            Fil = new FileInfo(strPDFFiles1 + dr["PRODUCT_ATT_FILE"].ToString());


                        if (Fil.Exists)
                        {
                            _stmpl_records = _stg_records.GetInstanceOf(_Package + "\\" + "DownloadCell");

                            strfile = dr["PRODUCT_ATT_FILE"].ToString().Replace(@"\\", "/");
                            strfile = strfile.Replace(@"\", "/");

                            file = strfile.Split(new string[] { @"/" }, StringSplitOptions.None);
                            if (file.Length > 0)
                            {
                                _stmpl_records.SetAttribute("TBT_ATTACH_FILE_NAME", file[file.Length - 1].ToString());
                                if ((file[file.Length - 1].ToString().ToLower().Contains(".jpg")))
                                    _stmpl_records.SetAttribute("TBT_ATTACH_INIT_PATH", "prodimages");
                                else
                                    _stmpl_records.SetAttribute("TBT_ATTACH_INIT_PATH", "");
                            }

                            //  FileInBytes = Fil.Length;
                            FileInKB = Fil.Length / 1024;

                            _stmpl_records.SetAttribute("TBT_PRODUCT_ATT_DESC", dr["PRODUCT_ATT_DESC"].ToString());

                            _stmpl_records.SetAttribute("TBT_ATTACH_FILE_PATH", strfile.Replace(".PDF", ".pdf"));
                            _stmpl_records.SetAttribute("TBT_ATTACH_SIZE", FileInKB.ToString() + " KB");


                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                            ictrecords++;
                        }
                    }

                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "DownloadRow");
                   

                    _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                    if (ictrecords > 0)
                        return _stmpl_container.ToString();

                }
                catch (Exception ex)
                {
                    objErrorHandler.ErrorMsg = ex;
                    objErrorHandler.CreateLog();
                    return "";
                }

                return "";
            }
            return "";
        }

        public string ST_Downloads_Update(bool chkdwld)
        {
            StringTemplateGroup _stg_container = null;
            StringTemplate _stmpl_container = null;
            try
            {

                _stg_container = new StringTemplateGroup("main", _SkinRootPath);

                //if (chkdwld == false)
                _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "DowloadUpdate");
                //else
                //_stmpl_container = _stg_container.GetInstanceOf("Familypage" + "\\" + "WithDowloadUpdate");

                if (HttpContext.Current.Session["AQ_FL_CAPTCH_IMAGE"] != null)
                {
                    _stmpl_container.SetAttribute("CAPTCHA_IMAGE", HttpContext.Current.Session["AQ_FL_CAPTCH_IMAGE"].ToString());
                    _stmpl_container.SetAttribute("CC_CODE", HttpContext.Current.Session["AQ_FL_CAPTCH_VALUE"].ToString());
                }
                if (!(chkdwld))
                    _stmpl_container.SetAttribute("IS_NO_DOWNLOAD", true);
                else
                    _stmpl_container.SetAttribute("IS_NO_DOWNLOAD", false);

                return _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();

            }
            return "";
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE USER ECOM LOGIN NAME ***/
        /********************************************************************************/
        private string GetLoginName()
        {
            string retvalue = string.Empty;
            string userid = HttpContext.Current.Session["USER_ID"].ToString();
            DataTable objDt = new DataTable();
            try
            {
            if (!string.IsNullOrEmpty(userid))
            {
                string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                //string sSQL = "SELECT CONTACT FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " and USER_ID = " + userid;
                //objHelperService.SQLString = sSQL;
                //string iLoginName = objHelperService.GetValue("CONTACT");
                string iLoginName = string.Empty;
                if (HttpContext.Current.Session["ECOM_LOGIN_COMP"] == null)
                {
                    objDt = (DataTable)objHelperDB.GetGenericDataDB(_CATALOG_ID, websiteid, userid, "GET_ECOM_ENABLED", HelperDB.ReturnType.RTTable);
                    if (objDt != null && objDt.Rows.Count > 0)
                    {
                        iLoginName = objDt.Rows[0]["CONTACT"].ToString();
                        HttpContext.Current.Session["ECOM_LOGIN_COMP"] = objDt;
                    }

                }
                else
                {
                    objDt = (DataTable)HttpContext.Current.Session["ECOM_LOGIN_COMP"];
                    if (objDt != null && objDt.Rows.Count > 0)
                        iLoginName = objDt.Rows[0]["CONTACT"].ToString();
                }
                retvalue = iLoginName;
            }
            }
                catch (Exception e)
            {
                    objErrorHandler.ErrorMsg=e;
                    objErrorHandler.CreateLog();
                    retvalue="-1";
                }
            finally
            {
                objDt.Dispose();
                objDt=null;
            }
            return retvalue;
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE ECOM COMPANY NAME ***/
        /********************************************************************************/
        private string GetCompanyName()
        {
            string retvalue = string.Empty;
            DataTable objDt = new DataTable();
            try
            {
                string userid = HttpContext.Current.Session["USER_ID"].ToString();
                if (!string.IsNullOrEmpty(userid))
                {
                    string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                    //string sSQL = "SELECT COMPANY_ID FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " and USER_ID = " + userid;
                    //objHelperService.SQLString = sSQL;
                    //int iCompanyID = objHelperService.CI(objHelperService.GetValue("COMPANY_ID"));
                    //string sSQL1 = "SELECT COMPANY_NAME FROM TBWC_COMPANY WHERE WEBSITE_ID = " + websiteid + " and COMPANY_ID = " + iCompanyID;
                    //objHelperService.SQLString = sSQL1;
                    //retvalue = objHelperService.GetValue("COMPANY_NAME").ToString().Trim();
                    if (HttpContext.Current.Session["ECOM_LOGIN_COMP"] == null)
                    {
                        objDt = (DataTable)objHelperDB.GetGenericDataDB(_CATALOG_ID, websiteid, userid, "GET_ECOM_ENABLED", HelperDB.ReturnType.RTTable);
                        if (objDt != null && objDt.Rows.Count > 0)
                        {
                            HttpContext.Current.Session["ECOM_LOGIN_COMP"] = objDt;
                            retvalue = objDt.Rows[0]["COMPANY_NAME"].ToString();

                        }
                    }
                    else
                    {
                        objDt = (DataTable)HttpContext.Current.Session["ECOM_LOGIN_COMP"];
                        if (objDt != null && objDt.Rows.Count > 0)
                        {
                            retvalue = objDt.Rows[0]["COMPANY_NAME"].ToString();

                        }
                    }
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg=e;
                objErrorHandler.CreateLog();
                retvalue="-1";
            }
            finally
            {
                objDt.Dispose();
                objDt=null;
            }
                return retvalue;
            }

        public string ST_Top_Load_cache()
        {
            string sHTML = string.Empty;

            try
            {
              
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;               
                TBWDataList[] lstrecords = new TBWDataList[0];
                DataSet dsrecords = new DataSet();              

                dsrecords = EasyAsk.GetCategoryAndBrand_Applicationstart("MainCategory");


                if (dsrecords == null || dsrecords.Tables.Count <= 0 || dsrecords.Tables[0].Rows.Count <= 0)
                    return "";


               





                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);


                lstrecords = new TBWDataList[dsrecords.Tables[0].Rows.Count + 1];



                int ictrecords = 0;
                string chkpackage = string.Empty;
                string chkpacmain = string.Empty;
             
                    chkpackage = "TopCache\\cell";
                    chkpacmain = "TopCache\\Main";
               

                string dr_cateid_upr = string.Empty;

                foreach (DataRow dr in dsrecords.Tables[0].Rows)//For Records
                {
                    dr_cateid_upr = dr["Category_id"].ToString().ToUpper();
                    if (dr_cateid_upr != WesNewsCategoryId && !(dr_cateid_upr.Contains("SPF-"))  && !(dr_cateid_upr.Contains("WES-CLR")))
                    {
               
                        _stmpl_records = _stg_records.GetInstanceOf(chkpackage);

                        //added by indu
                        string BlockCategory = System.Configuration.ConfigurationManager.AppSettings["BlockCategory"].ToString();
                        if (dr_cateid_upr != BlockCategory)
                        {
                            _stmpl_records.SetAttribute("TBT_CATEGORY_ID", dr["CATEGORY_ID"]);
                            _stmpl_records.SetAttribute("TBT_CUSTOM_NUM_FIELD3", dr["CUSTOM_NUM_FIELD3"]);
                            _stmpl_records.SetAttribute("TBT_EAPATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(dr["EA_PATH"].ToString())));
                            _stmpl_records.SetAttribute("TBT_CATEGORY_NAME", dr["CATEGORY_NAME_TOP"]);

                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                            ictrecords++;
                        }
                    }

                }

                _stmpl_container = _stg_container.GetInstanceOf(chkpacmain);

           _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                sHTML = _stmpl_container.ToString();
             
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return ProdimageRreplaceImages(sHTML);
        }

        public string ST_Top_Load()
        {
            string sHTML = string.Empty;

            try
            {
                //stopwatch.Start();
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                //  StringTemplate _stmpl_records1 = null;
                // StringTemplate _stmpl_recordsrows = null;
                TBWDataList[] lstrecords = new TBWDataList[0];
           
                DataSet dsrecords = new DataSet();
                // DataTable dt = null;
               // DataRow[] drs = null;

                dsrecords = EasyAsk.GetCategoryAndBrand("MainCategory");
             

                if (dsrecords == null || dsrecords.Tables.Count <= 0 || dsrecords.Tables[0].Rows.Count <= 0)
                    return "";


                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
               // int ictrows = 0;






                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);


                lstrecords = new TBWDataList[dsrecords.Tables[0].Rows.Count + 1];



                int ictrecords = 0;
                string chkpackage = string.Empty;
                string chkpacmain = string.Empty;
                //if (_Package == "TOP")
                //     chkpackage = "Top";
                //else
                //     chkpackage = "TopLog";
                if (_Package == "TOP")
                {
                    chkpackage = "Top\\cell";
                    chkpacmain = "Top\\Main";
                }
                else
                {
                    chkpackage = "TopLog\\cell";
                    chkpacmain = "TopLog\\Main";
                }

                string dr_cateid_upr = string.Empty;
             //Commented by indu introduced application cache concept
                //foreach (DataRow dr in dsrecords.Tables[0].Rows)//For Records
                //{
                //    dr_cateid_upr = dr["Category_id"].ToString().ToUpper();
                //    if (dr_cateid_upr != WesNewsCategoryId && !(dr_cateid_upr.Contains("SPF-")))
                   
                //    {
                //        //if (_Package == "TOP")
                //        //    _stmpl_records = _stg_records.GetInstanceOf("Top" + "\\" + "cell");
                //        //else
                //        //    _stmpl_records = _stg_records.GetInstanceOf("TopLog" + "\\" + "cell");

                //        _stmpl_records = _stg_records.GetInstanceOf(chkpackage);

                //        //added by indu
                //        string BlockCategory = System.Configuration.ConfigurationManager.AppSettings["BlockCategory"].ToString();
                //        if (dr_cateid_upr != BlockCategory)
                //        {
                //            _stmpl_records.SetAttribute("TBT_CATEGORY_ID", dr["CATEGORY_ID"]);
                //            _stmpl_records.SetAttribute("TBT_CUSTOM_NUM_FIELD3", dr["CUSTOM_NUM_FIELD3"]);
                //            _stmpl_records.SetAttribute("TBT_EAPATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(dr["EA_PATH"].ToString())));
                //            _stmpl_records.SetAttribute("TBT_CATEGORY_NAME", dr["CATEGORY_NAME_TOP"]);
                            
                //            lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                //            ictrecords++;
                //        }
                //    }
                    
                //}
        
                //if (_Package == "TOP")
                //    _stmpl_container = _stg_container.GetInstanceOf("Top" + "\\" + "Main");
                //else
                //    _stmpl_container = _stg_container.GetInstanceOf("TopLog" + "\\" + "Main");

                _stmpl_container = _stg_container.GetInstanceOf(chkpacmain);

                _stmpl_container.SetAttribute("TBW_CART_ITEM", cartitem);

                if (_Package == "TOP")
                {

                    _stmpl_container.SetAttribute("TBT_LOGIN_NAME", GetLoginName());

                    if (HttpContext.Current.Session["CUSTOMER_TYPE"] != null && HttpContext.Current.Session["CUSTOMER_TYPE"].ToString() == "Retailer")
                    {
                        if (HttpContext.Current.Session["USER_ROLE"] != null && Convert.ToInt32(HttpContext.Current.Session["USER_ROLE"].ToString()) == 4)
                        {
                            string ReMailLink = "<a Href=ConfirmMessage.aspx?Result=REMAILACTIVATION class=\"toplinkatest\">Re-Email Activation Link Now</a>";

                            _stmpl_container.SetAttribute("TBT_LOGIN_NAME", " Your Account Has Not Been Activated! " + ReMailLink);
                        }
                        else
                            _stmpl_container.SetAttribute("TBT_COMPANY_NAME", "");
                    }
                    else
                    {
                        _stmpl_container.SetAttribute("TBT_COMPANY_NAME", GetCompanyName() + ",");
                    }


                    HttpContext.Current.Session["LOGIN_NAME"] = GetLoginName();



                }
                if ((HttpContext.Current.Session["USER_ID"] != null) && (HttpContext.Current.Session["USER_ID"].ToString() !=""))
                {
                    DataSet ds = objUserServices.GetPaymentoption(Convert.ToInt32(HttpContext.Current.Session["USER_ID"]));
                    int i = 0;
                    if (ds != null)
                    {
                        i = Convert.ToInt16(ds.Tables[0].Rows[0]["PAYMENT_TERM"].ToString());

                    }
                    if (i == 9)
                    {
                        _stmpl_container.SetAttribute("TBT_STOP_ACCOUNT", true);
                    }
                }
                string _Tbt_Order_Id = string.Empty;
                string _Tbt_Ship_URL = string.Empty;
                int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;

                if (HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0)
                {
                    _Tbt_Order_Id = HttpContext.Current.Session["ORDER_ID"].ToString();
                }
                else
                {
                    _Tbt_Order_Id = objOrderServices.GetOrderID(objHelperServices.CI(HttpContext.Current.Session["USER_ID"]), OpenOrdStatusID).ToString();
                }             

                if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ViewOrder"] != null && HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")) || (HttpContext.Current.Request.QueryString["ApproveOrder"] != null && HttpContext.Current.Request.QueryString["ApproveOrder"].Equals("Approve")))
                {
                    _Tbt_Ship_URL = "shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
                }
                else
                {
                    _Tbt_Ship_URL = "shipping.aspx";
                }

                _stmpl_container.SetAttribute("TBT_ORDER_ID", _Tbt_Order_Id);
                _stmpl_container.SetAttribute("TBT_SHIP_URL", _Tbt_Ship_URL);

                //if (HttpContext.Current.Session["USER_ID"] == null || HttpContext.Current.Session["USER_ID"] == "")
                //    _stmpl_container.SetAttribute("TBT_LOGIN", false);
                //else
                //    _stmpl_container.SetAttribute("TBT_LOGIN", true);

               // _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                sHTML = _stmpl_container.ToString();
               // stopwatch.Stop();
               // objErrorHandler.CreateLog("ST_Top_Load function load time:" + "=" + stopwatch.Elapsed);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return ProdimageRreplaceImages(sHTML);
        }
        public string ST_Bottom_Load()
        {
            string sHTML = string.Empty;

            try
            {
                //stopwatch.Start();
                DataSet dsbotmsname = new DataSet();
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                //  StringTemplate _stmpl_records1 = null;
                // StringTemplate _stmpl_recordsrows = null;
                TBWDataList[] lstrecords = new TBWDataList[0];
               
                DataSet dsrecords = new DataSet();
                // DataTable dt = null;
              //  DataRow[] drs = null;

                dsrecords = EasyAsk.GetCategoryAndBrand_Applicationstart("MainCategory");
              
                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
              //  int ictrows = 0;






                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);


                lstrecords = new TBWDataList[dsrecords.Tables[0].Rows.Count + 1];


                string dr_cat_id_upper = string.Empty;
                int ictrecords = 0;
                string cellbottom = "Bottom\\cell";
                foreach (DataRow dr in dsrecords.Tables[0].Rows)//For Records
                {

                    _stmpl_records = _stg_records.GetInstanceOf(cellbottom);
                    //added by indu
                    //string BlockCategory = System.Configuration.ConfigurationManager.AppSettings["BlockCategory"].ToString();     
                    //if (dr["CATEGORY_ID"].ToString().ToUpper() != BlockCategory)
                    dr_cat_id_upper = dr["Category_id"].ToString().ToUpper();
                    if (dr_cat_id_upper != WesNewsCategoryId && !(dr_cat_id_upper.Contains("SPF-")))
                    {
                        _stmpl_records.SetAttribute("TBT_CATEGORY_ID", dr["CATEGORY_ID"]);
                        _stmpl_records.SetAttribute("TBT_CUSTOM_NUM_FIELD3", dr["CUSTOM_NUM_FIELD3"]);
                        _stmpl_records.SetAttribute("TBT_EAPATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(dr["EA_PATH"].ToString())));
                        _stmpl_records.SetAttribute("TBT_CATEGORY_NAME", dr["CATEGORY_NAME"]);



                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                        ictrecords++;
                    }

                }

                _stmpl_container = _stg_container.GetInstanceOf("Bottom\\Main");

                //if (HttpContext.Current.Session["CUSTOMER_TYPE"] != null && HttpContext.Current.Session["CUSTOMER_TYPE"].ToString().Equals("Retailer"))
                //    _stmpl_container.SetAttribute("TBT_RET_PRO", false);
                //else
                //    _stmpl_container.SetAttribute("TBT_RET_PRO", true);

                //if (!HttpContext.Current.Session["USER_ID"].Equals(""))
                //{
                //    if (HttpContext.Current.Session["USER_ID"].ToString().Equals(System.Configuration.ConfigurationManager.AppSettings["DUM_USER_ID"].ToString()))
                //        _stmpl_container.SetAttribute("TBT_DUMUSER_CHECK", false);
                //    else
                //        _stmpl_container.SetAttribute("TBT_DUMUSER_CHECK", true);
                //}

                //dsbotmsname = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_MICROSITE_NAME");
                //string combinelihref = "";
                //if (dsbotmsname != null)
                //{
                //    if (dsbotmsname.Tables[0].Rows.Count > 0)
                //    {
                //        foreach (DataRow dr in dsbotmsname.Tables[0].Rows)
                //        {
                //            string href ="";
                //            string lihref="";

                //            href= "/SupplierList/Supplier1/Contactuspage_Supplier1.aspx?supplier_name=" + dr["MICROSITE_NAME"].ToString();
                //            lihref = "<li><a href=\"" + href + "\" class=\"tx_4\">" + dr["MICROSITE_NAME"].ToString() + "</a></li>";
                //            combinelihref = combinelihref + lihref;
                //        }
                //    }
                //}
                //    _stmpl_container.SetAttribute("MICSITNAME", combinelihref);

                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                sHTML = _stmpl_container.ToString();
                //stopwatch.Stop();
               // objErrorHandler.CreateLog("ST_Bottom_Load function load time:" + "=" + stopwatch.Elapsed);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return ProdimageRreplaceImages(sHTML);
        }
        public string ST_NewProduct_Nav_Load()
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
            
                DataSet dsrecords = new DataSet();
                // DataTable dt = null;
               // DataRow[] drs = null;
                string userid ="0";
                if(  HttpContext.Current.Session["USER_ID"]!=null &&  HttpContext.Current.Session["USER_ID"]!="")
                    userid = HttpContext.Current.Session["USER_ID"].ToString();



                DataSet tmpds = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_PRODUCT_WES 12,0," + userid);

                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    //tmpds.Tables[0].Columns.Add("URL_RW_PATH", typeof(string));
                    //foreach (DataRow dr in tmpds.Tables[0].Rows)
                    //{
                    //    dr["URL_RW_PATH"] = objHelperServices.Cons_NewURlASPX("", "AllProducts////WESAUSTRALASIA////" + "////" + dr["CATEGORY_PATH"].ToString() + "////" + dr["FAMILY_ID"].ToString() + "=" + dr["FAMILY_name"].ToString() + "////" + dr["product_ID"].ToString() + "=" + dr["Code"].ToString(), "pd.aspx", true, "");

                    //}

                }
                else
                    return "";

                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
              //  int ictrows = 0;






                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);


                lstrecords = new TBWDataList[tmpds.Tables[0].Rows.Count + 1];

                bool isenable = IsEcomenabled();

                int ictrecords = 0;
                string npnl = _Package + "\\cell";
                foreach (DataRow dr in tmpds.Tables[0].Rows)//For Records
                {
                    _stmpl_records = _stg_records.GetInstanceOf(npnl);


                    _stmpl_records.SetAttribute("TBT_PRODUCT_ID", dr["PRODUCT_ID"]);

                    _stmpl_records.SetAttribute("TBT_FAMILY_ID", dr["FAMILY_ID"]);

                    _stmpl_records.SetAttribute("TBT_CATEGORY_ID", dr["CATEGORY_ID"]);

                    _stmpl_records.SetAttribute("TBT_FAMILY_NAME", dr["FAMILY_NAME"]);

                    string espath = "AllProducts////WESAUSTRALASIA////" + dr["CATEGORY_PATH"];
                    _stmpl_records.SetAttribute("FAMILY_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(espath)));

                    espath = "AllProducts////WESAUSTRALASIA////" + dr["CATEGORY_PATH"] + "////UserSearch1=Family Id=" + dr["FAMILY_ID"];
                    _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(espath)));

                  
                    _stmpl_records.SetAttribute("TBT_CODE", dr["CODE"]);


            _stmpl_records.SetAttribute("TBT_STOCK_STATUS_2", true);
            _stmpl_records.SetAttribute("TBT_TWEB_IMAGE1", GetImagePath(dr["TWEB Image1"].ToString(),""));
            _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", dr["MIN_ORD_QTY"]);
            _stmpl_records.SetAttribute("TBT_QTY_AVAIL", dr["QTY_AVAIL"]);

                    _stmpl_records.SetAttribute("TBT_ECOMENABLED", isenable);
                   
            


                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                    ictrecords++;
                }


                _stmpl_container = _stg_container.GetInstanceOf(_Package  + "\\" + "Main");



                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return ProdimageRreplaceImages(sHTML);
        }


        public string ST_NewProduct_log_Nav_Load()
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

                DataSet dsrecords = new DataSet();
                // DataTable dt = null;
               // DataRow[] drs = null;
                string userid = "0";
                if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
                    userid = HttpContext.Current.Session["USER_ID"].ToString();



                DataSet tmpds = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_PRODUCT_WES 3,0," + userid);

                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    //tmpds.Tables[0].Columns.Add("URL_RW_PATH", typeof(string));
                    //foreach (DataRow dr in tmpds.Tables[0].Rows)
                    //{
                    //    dr["URL_RW_PATH"] = objHelperServices.Cons_NewURlASPX("", "AllProducts////WESAUSTRALASIA////" + "////" + dr["CATEGORY_PATH"].ToString() + "////" + dr["FAMILY_ID"].ToString() + "=" + dr["FAMILY_name"].ToString() + "////" + dr["product_ID"].ToString() + "=" + dr["Code"].ToString(), "pd.aspx", true, "");

                    //}

                }
                else
                    return "";

                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
            //    int ictrows = 0;






                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);


                lstrecords = new TBWDataList[tmpds.Tables[0].Rows.Count + 1];

                bool isenable = IsEcomenabled();

                int ictrecords = 0;
                string nplnl = _Package + "\\cell";
                foreach (DataRow dr in tmpds.Tables[0].Rows)//For Records
                {
                    _stmpl_records = _stg_records.GetInstanceOf(nplnl);


                    _stmpl_records.SetAttribute("TBT_PRODUCT_ID", dr["PRODUCT_ID"]);

                    _stmpl_records.SetAttribute("TBT_FAMILY_ID", dr["FAMILY_ID"]);

                    _stmpl_records.SetAttribute("TBT_CATEGORY_ID", dr["CATEGORY_ID"]);

                    _stmpl_records.SetAttribute("TBT_FAMILY_NAME", dr["FAMILY_NAME"]);

                    string espath = "AllProducts////WESAUSTRALASIA////" + dr["CATEGORY_PATH"];
                    _stmpl_records.SetAttribute("FAMILY_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(espath)));

                    espath = "AllProducts////WESAUSTRALASIA////" + dr["CATEGORY_PATH"] + "////UserSearch1=Family Id=" + dr["FAMILY_ID"];
                    _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(espath)));


                    _stmpl_records.SetAttribute("TBT_CODE", dr["CODE"]);

                    //if (dr["DESCRIPTION"].ToString().Length > 40)
                    //    _stmpl_records.SetAttribute("TBT_DESCRIPTIONS1", dr["DESCRIPTION"].ToString().Substring(0, 40) + "...");
                    //else
                    //    _stmpl_records.SetAttribute("TBT_DESCRIPTIONS1", dr["DESCRIPTION"].ToString());


                  //  _stmpl_records.SetAttribute("TBT_STOCK_STATUS_2", true);
                    _stmpl_records.SetAttribute("TBT_TWEB_IMAGE1", GetImagePath(dr["TWEB Image1"].ToString(), ""));
                   // _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", dr["MIN_ORD_QTY"].ToString());
                  //  _stmpl_records.SetAttribute("TBT_QTY_AVAIL", dr["QTY_AVAIL"].ToString());

                  //  _stmpl_records.SetAttribute("TBT_ECOMENABLED", isenable);




                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                    ictrecords++;
                }


                _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\Main");



                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return ProdimageRreplaceImages(sHTML);
        }

        public string ST_right_specialproduct()
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

                DataSet dsrecords = new DataSet();
                // DataTable dt = null;
                // DataRow[] drs = null;
                string userid = "0";
                if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
                    userid = HttpContext.Current.Session["USER_ID"].ToString();



                DataSet tmpds = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_Specials_WES 3,0," + userid);

                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    //tmpds.Tables[0].Columns.Add("URL_RW_PATH", typeof(string));
                    //foreach (DataRow dr in tmpds.Tables[0].Rows)
                    //{
                    //    dr["URL_RW_PATH"] = objHelperServices.Cons_NewURlASPX("", "AllProducts////WESAUSTRALASIA////" + "////" + dr["CATEGORY_PATH"].ToString() + "////" + dr["FAMILY_ID"].ToString() + "=" + dr["FAMILY_name"].ToString() + "////" + dr["product_ID"].ToString() + "=" + dr["Code"].ToString(), "pd.aspx", true, "");

                    //}

                }
                else
                    return "";

                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                //    int ictrows = 0;






                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);
                int rowcount = 0;

                lstrecords = new TBWDataList[5 + 1];

                bool isenable = IsEcomenabled();

                int ictrecords = 0;
                string nplnl = _Package + "\\cell";

                foreach (DataRow dr in tmpds.Tables[0].Rows)//For Records
                {
                    _stmpl_records = _stg_records.GetInstanceOf(nplnl);


                    _stmpl_records.SetAttribute("TBT_PRODUCT_ID", dr["PRODUCT_ID"]);

                    _stmpl_records.SetAttribute("TBT_FAMILY_ID", dr["FAMILY_ID"]);

                    _stmpl_records.SetAttribute("TBT_CATEGORY_ID", dr["CATEGORY_ID"]);

                    _stmpl_records.SetAttribute("TBT_FAMILY_NAME", dr["FAMILY_NAME"]);

                    string espath = "AllProducts////WESAUSTRALASIA////" + dr["CATEGORY_PATH"];
                    _stmpl_records.SetAttribute("FAMILY_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(espath)));

                    espath = "AllProducts////WESAUSTRALASIA////" + dr["CATEGORY_PATH"] + "////UserSearch1=Family Id=" + dr["FAMILY_ID"];
                    _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(espath)));


                    _stmpl_records.SetAttribute("TBT_CODE", dr["CODE"]);

                    //if (dr["DESCRIPTION"].ToString().Length > 40)
                    //    _stmpl_records.SetAttribute("TBT_DESCRIPTIONS1", dr["DESCRIPTION"].ToString().Substring(0, 40) + "...");
                    //else
                    //    _stmpl_records.SetAttribute("TBT_DESCRIPTIONS1", dr["DESCRIPTION"].ToString());


                    //  _stmpl_records.SetAttribute("TBT_STOCK_STATUS_2", true);
                    _stmpl_records.SetAttribute("TBT_TWEB_IMAGE1", GetImagePath(dr["TWEB Image1"].ToString(), ""));
                    // _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", dr["MIN_ORD_QTY"].ToString());
                    //  _stmpl_records.SetAttribute("TBT_QTY_AVAIL", dr["QTY_AVAIL"].ToString());

                    //  _stmpl_records.SetAttribute("TBT_ECOMENABLED", isenable);




                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                    ictrecords++;
                    rowcount = rowcount + 1;
                    if (rowcount == 5)
                    {

                        break;

                    }

                }


                _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\Main");



                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return ProdimageRreplaceImages(sHTML);
        }

        public string ST_NewProduct_Load()
        {
            string sHTML = string.Empty;

            try
            {
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                StringTemplate _stmpl_records_c = null;
                StringTemplate _stmpl_records_s = null;
                StringTemplate _stmpl_recordstemp = new StringTemplate();
                //  StringTemplate _stmpl_records1 = null;
                // StringTemplate _stmpl_recordsrows = null;
                TBWDataList[] lstrecords = new TBWDataList[0];
                TBWDataList[] lstrecords_clearance = new TBWDataList[0];
                TBWDataList[] lstrecords_specials = new TBWDataList[0];
                DataSet dsrecords = new DataSet();
                // DataTable dt = null;
                DataRow[] drs = null;
                string userid = "0";
                bool isenable = IsEcomenabled();

                int ictrecords = 0;
                string cellnp = _Package + "\\cell";

                if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
                    userid = HttpContext.Current.Session["USER_ID"].ToString();

                try
                {

                  //  DataSet tmpds = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_PRODUCT_WES 20,0," + userid);
                    DataSet tmpds = (DataSet)HttpContext.Current.Cache["Cache_NEWPRODUCT"];  
                    if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                    {
                        //tmpds.Tables[0].Columns.Add("URL_RW_PATH", typeof(string));
                        //foreach (DataRow dr in tmpds.Tables[0].Rows)
                        //{ 
                        //    dr["URL_RW_PATH"] = objHelperServices.Cons_NewURlASPX("", "AllProducts////WESAUSTRALASIA////" + "////" + dr["CATEGORY_PATH"].ToString() + "////" + dr["FAMILY_ID"].ToString() + "=" + dr["FAMILY_name"].ToString() + "////" + dr["product_ID"].ToString() + "=" + dr["Code"].ToString(), "pd.aspx", true, "");

                        //}

                    }
                    //else
                    //    return "";

                    // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                    //  int ictrows = 0;






                    _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                    _stg_container = new StringTemplateGroup("main", _SkinRootPath);


                    lstrecords = new TBWDataList[tmpds.Tables[0].Rows.Count + 1];

                   
                    foreach (DataRow dr in tmpds.Tables[0].Rows)//For Records
                    {
                        _stmpl_records = _stg_records.GetInstanceOf(cellnp);

                        //objErrorHandler.CreateLog("new product ebay block ---> " + tmpds.Tables[0].Rows.Count);
                        _stmpl_records.SetAttribute("TBT_PRODUCT_ID", dr["PRODUCT_ID"]);

                        _stmpl_records.SetAttribute("TBT_FAMILY_ID", dr["FAMILY_ID"]);

                        _stmpl_records.SetAttribute("TBT_CATEGORY_ID", dr["CATEGORY_ID"]);

                        _stmpl_records.SetAttribute("TBT_FAMILY_NAME", dr["FAMILY_NAME"]);

                        string espath = "AllProducts////WESAUSTRALASIA////" + dr["CATEGORY_PATH"];
                        _stmpl_records.SetAttribute("FAMILY_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(espath)));

                        espath = "AllProducts////WESAUSTRALASIA////" + dr["CATEGORY_PATH"] + "////UserSearch1=Family Id=" + dr["FAMILY_ID"];
                        _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(espath)));

                        //string sss = HttpUtility.UrlDecode(objSecurity.StringDeCrypt("K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT0qj5dNwgdSlMUS%2fCFrvtBbBsxmiH4H4c8zrux6WqQRHPewdDM24DI95rC7VuvDaJQS5yxSwZ08Tu%2bd1YoDeXlh1DNFePmp7MPiIpl%2fXb69sB8xKdMK63aJooZ89D8unE0QBAZ1hU2LTNHfzS3%2fFnyH"));
                        //string ss1 = objSecurity.StringDeCrypt("K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT0qj5dNwgdSlMUS%2fCFrvtBbBsxmiH4H4c8zrux6WqQRHPewdDM24DI95rC7VuvDaJQS5yxSwZ08Tu%2bd1YoDeXlh1DNFePmp7MPiIpl%2fXb69sB8xKdMK63aJooZ89D8unE0QBAZ1hU2LTNHfzS3%2fFnyH");

                        _stmpl_records.SetAttribute("TBT_CODE", dr["CODE"]);
                        if (dr["DESCRIPTION"].ToString().Length > 80)
                            _stmpl_records.SetAttribute("TBT_DESCRIPTIONS", dr["DESCRIPTION"].ToString().Substring(0, 80) + "...");
                        else
                            _stmpl_records.SetAttribute("TBT_DESCRIPTIONS", dr["DESCRIPTION"]);

                        _stmpl_records.SetAttribute("TBT_YOURCOST", dr["COSt"]);

                        if (HttpContext.Current.Session["EBAY_BLOCK"] != null && HttpContext.Current.Session["EBAY_BLOCK"].ToString() == "True" && dr["EBAY_BLOCK"] != null && dr["EBAY_BLOCK"].ToString() == "True")
                        {
                            _stmpl_records.SetAttribute("TBT_STOCK_STATUS_2", false);
                            _stmpl_records.SetAttribute("PRODUCT_STATUS", "UNAVAILABLE");
                        }
                        else
                        {
                            _stmpl_records.SetAttribute("TBT_STOCK_STATUS_2", true);
                        }
                           // _stmpl_records.SetAttribute("TBT_STOCK_STATUS_2", true);
                        _stmpl_records.SetAttribute("TBT_TWEB_IMAGE1", GetImagePath(dr["TWEB Image1"].ToString(), ""));
                      //  _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", dr["MIN_ORD_QTY"]);
                        _stmpl_records.SetAttribute("TBT_QTY_AVAIL", dr["QTY_AVAIL"]);

                        _stmpl_records.SetAttribute("TBT_ECOMENABLED", isenable);




                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                        ictrecords++;
                    }


                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\Main");



                    _stmpl_container.SetAttribute("TBWDataList", lstrecords);

                }
                catch (Exception ex)
                {
                    objErrorHandler.CreateLog(ex.ToString());
                }





                //Clearance

                try
                {
                    _stg_records = new StringTemplateGroup("row_clearance", _SkinRootPath);

                  //  DataSet tmpds_c = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_CLEARANCE_WES 20,0," + userid);
                    DataSet tmpds_c = (DataSet)HttpContext.Current.Cache["Cache_ClearanceProducts"];  
                    
                    lstrecords_clearance = new TBWDataList[20];
                    ictrecords = 0;
                    foreach (DataRow dr in tmpds_c.Tables[0].Rows)//For Records
                    {
                        _stmpl_records_c = _stg_records.GetInstanceOf(cellnp);


                        _stmpl_records_c.SetAttribute("TBT_PRODUCT_ID", dr["PRODUCT_ID"]);

                        _stmpl_records_c.SetAttribute("TBT_FAMILY_ID", dr["FAMILY_ID"]);

                        _stmpl_records_c.SetAttribute("TBT_CATEGORY_ID", dr["CATEGORY_ID"]);

                        _stmpl_records_c.SetAttribute("TBT_FAMILY_NAME", dr["FAMILY_NAME"]);

                        string espath = "AllProducts////WESAUSTRALASIA////" + dr["CATEGORY_PATH"];
                        _stmpl_records_c.SetAttribute("FAMILY_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(espath)));

                        espath = "AllProducts////WESAUSTRALASIA////" + dr["CATEGORY_PATH"] + "////UserSearch1=Family Id=" + dr["FAMILY_ID"];
                        _stmpl_records_c.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(espath)));

                        //string sss = HttpUtility.UrlDecode(objSecurity.StringDeCrypt("K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT0qj5dNwgdSlMUS%2fCFrvtBbBsxmiH4H4c8zrux6WqQRHPewdDM24DI95rC7VuvDaJQS5yxSwZ08Tu%2bd1YoDeXlh1DNFePmp7MPiIpl%2fXb69sB8xKdMK63aJooZ89D8unE0QBAZ1hU2LTNHfzS3%2fFnyH"));
                        //string ss1 = objSecurity.StringDeCrypt("K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT0qj5dNwgdSlMUS%2fCFrvtBbBsxmiH4H4c8zrux6WqQRHPewdDM24DI95rC7VuvDaJQS5yxSwZ08Tu%2bd1YoDeXlh1DNFePmp7MPiIpl%2fXb69sB8xKdMK63aJooZ89D8unE0QBAZ1hU2LTNHfzS3%2fFnyH");

                        _stmpl_records_c.SetAttribute("TBT_CODE", dr["CODE"]);
                        if (dr["DESCRIPTION"].ToString().Length > 80)
                            _stmpl_records_c.SetAttribute("TBT_DESCRIPTIONS", dr["DESCRIPTION"].ToString().Substring(0, 80) + "...");
                        else
                            _stmpl_records_c.SetAttribute("TBT_DESCRIPTIONS", dr["DESCRIPTION"]);

                        _stmpl_records_c.SetAttribute("TBT_YOURCOST", dr["COSt"]);

                      //  _stmpl_records_c.SetAttribute("TBT_STOCK_STATUS_2", true);
                        if (HttpContext.Current.Session["EBAY_BLOCK"] != null && HttpContext.Current.Session["EBAY_BLOCK"].ToString() == "True" && dr["EBAY_BLOCK"] != null && dr["EBAY_BLOCK"].ToString() == "True")
                        {
                            _stmpl_records_c.SetAttribute("TBT_STOCK_STATUS_2", false);
                            _stmpl_records_c.SetAttribute("PRODUCT_STATUS", "UNAVAILABLE");
                        }
                        else
                        {
                            _stmpl_records_c.SetAttribute("TBT_STOCK_STATUS_2", true);
                        }
                        _stmpl_records_c.SetAttribute("TBT_TWEB_IMAGE1", GetImagePath(dr["TWEB Image1"].ToString(), ""));
                      //  _stmpl_records_c.SetAttribute("TBT_MIN_ORD_QTY", dr["MIN_ORD_QTY"]);
                        _stmpl_records_c.SetAttribute("TBT_QTY_AVAIL", dr["QTY_AVAIL"]);

                        _stmpl_records_c.SetAttribute("TBT_ECOMENABLED", isenable);




                        lstrecords_clearance[ictrecords] = new TBWDataList(_stmpl_records_c.ToString());
                        ictrecords++;
                        if (ictrecords == 20)
                        {
                            break;
                        }
                    }



                    _stmpl_container.SetAttribute("TBWDataList_c", lstrecords_clearance);


                }
                catch (Exception ex)
                {
                    objErrorHandler.CreateLog(ex.ToString());
                }


                try
                {
                    _stg_records = new StringTemplateGroup("row_Specials", _SkinRootPath);

                    //DataSet tmpds_S = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_Specials_WES 20,0," + userid);
                    DataSet tmpds_S = (DataSet)HttpContext.Current.Cache["Cache_SpecialProducts"];
                    lstrecords_specials = new TBWDataList[20];
                    ictrecords = 0;
                    foreach (DataRow dr in tmpds_S.Tables[0].Rows)//For Records
                    {
                        _stmpl_records_s = _stg_records.GetInstanceOf(cellnp);


                        _stmpl_records_s.SetAttribute("TBT_PRODUCT_ID", dr["PRODUCT_ID"]);

                        _stmpl_records_s.SetAttribute("TBT_FAMILY_ID", dr["FAMILY_ID"]);

                        _stmpl_records_s.SetAttribute("TBT_CATEGORY_ID", dr["CATEGORY_ID"]);

                        _stmpl_records_s.SetAttribute("TBT_FAMILY_NAME", dr["FAMILY_NAME"]);

                        string espath = "AllProducts////WESAUSTRALASIA////" + dr["CATEGORY_PATH"];
                        _stmpl_records_s.SetAttribute("FAMILY_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(espath)));

                        espath = "AllProducts////WESAUSTRALASIA////" + dr["CATEGORY_PATH"] + "////UserSearch1=Family Id=" + dr["FAMILY_ID"];
                        _stmpl_records_s.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(espath)));

                        //string sss = HttpUtility.UrlDecode(objSecurity.StringDeCrypt("K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT0qj5dNwgdSlMUS%2fCFrvtBbBsxmiH4H4c8zrux6WqQRHPewdDM24DI95rC7VuvDaJQS5yxSwZ08Tu%2bd1YoDeXlh1DNFePmp7MPiIpl%2fXb69sB8xKdMK63aJooZ89D8unE0QBAZ1hU2LTNHfzS3%2fFnyH"));
                        //string ss1 = objSecurity.StringDeCrypt("K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT0qj5dNwgdSlMUS%2fCFrvtBbBsxmiH4H4c8zrux6WqQRHPewdDM24DI95rC7VuvDaJQS5yxSwZ08Tu%2bd1YoDeXlh1DNFePmp7MPiIpl%2fXb69sB8xKdMK63aJooZ89D8unE0QBAZ1hU2LTNHfzS3%2fFnyH");

                        _stmpl_records_s.SetAttribute("TBT_CODE", dr["CODE"]);
                        if (dr["DESCRIPTION"].ToString().Length > 80)
                            _stmpl_records_s.SetAttribute("TBT_DESCRIPTIONS", dr["DESCRIPTION"].ToString().Substring(0, 80) + "...");
                        else
                            _stmpl_records_s.SetAttribute("TBT_DESCRIPTIONS", dr["DESCRIPTION"]);

                        _stmpl_records_s.SetAttribute("TBT_YOURCOST", dr["COSt"]);

                      //  _stmpl_records_s.SetAttribute("TBT_STOCK_STATUS_2", true);
                        if (HttpContext.Current.Session["EBAY_BLOCK"] != null && HttpContext.Current.Session["EBAY_BLOCK"].ToString() == "True" && dr["EBAY_BLOCK"] != null && dr["EBAY_BLOCK"].ToString() == "True")
                        {
                            _stmpl_records_s.SetAttribute("TBT_STOCK_STATUS_2", false);
                            _stmpl_records_s.SetAttribute("PRODUCT_STATUS", "UNAVAILABLE");
                        }
                        else
                        {
                            _stmpl_records_s.SetAttribute("TBT_STOCK_STATUS_2", true);
                        }
                        _stmpl_records_s.SetAttribute("TBT_TWEB_IMAGE1", GetImagePath(dr["TWEB Image1"].ToString(), ""));
                        //_stmpl_records_s.SetAttribute("TBT_MIN_ORD_QTY", dr["MIN_ORD_QTY"]);
                        _stmpl_records_s.SetAttribute("TBT_QTY_AVAIL", dr["QTY_AVAIL"]);

                        _stmpl_records_s.SetAttribute("TBT_ECOMENABLED", isenable);




                        lstrecords_specials[ictrecords] = new TBWDataList(_stmpl_records_s.ToString());
                        ictrecords++;
                        if (ictrecords == 20)
                        {
                            break;
                        }
                    }



                    _stmpl_container.SetAttribute("TBWDataList_s", lstrecords_specials);

                }
                catch (Exception ex)
                {
                    objErrorHandler.CreateLog(ex.ToString());
                }


                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return ProdimageRreplaceImages(sHTML);
        }


        public string ST_SpecialProduct()
        {
            string sHTML = string.Empty;

            try
            {
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;

                StringTemplate _stmpl_records_s = null;
                StringTemplate _stmpl_recordstemp = new StringTemplate();
                //  StringTemplate _stmpl_records1 = null;
                // StringTemplate _stmpl_recordsrows = null;
                string PriceTable = string.Empty;
                int pricecode = 0;
                string tmpProds = string.Empty;
                DataSet dsBgDisc = new DataSet();
                DataSet dsPriceTableAll = new DataSet();
                TBWDataList[] lstrecords_specials = new TBWDataList[0];
                DataSet dsrecords = new DataSet();
                // DataTable dt = null;
                DataRow[] drs = null;
                string userid = "0";
                if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
                    userid = HttpContext.Current.Session["USER_ID"].ToString();


                DataSet tmpds_S = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_Specials_WES 20,1," + userid);
                lstrecords_specials = new TBWDataList[tmpds_S.Tables[0].Rows.Count + 1];






                _stg_records = new StringTemplateGroup("row_Specials", _SkinRootPath);

                _stg_container = new StringTemplateGroup("main", _SkinRootPath);


                HelperDB objhelperDb = new HelperDB();
                int ictrecords = 0;
                string cellnp = _Package + "\\cell";
                bool isenable = IsEcomenabled();
                pricecode = objHelperDB.GetPriceCode(userid);
                tmpProds = "";
                if (Convert.ToInt32(userid) > 0)
                {
                    foreach (DataRow drpid in tmpds_S.Tables[0].Rows)
                    {
                        tmpProds = tmpProds + drpid["PRODUCT_ID"].ToString() + ",";
                    }
                    if (tmpProds != "")
                    {
                        tmpProds = tmpProds.Substring(0, tmpProds.Length - 1);
                     
                        dsPriceTableAll = objHelperDB.GetProductPriceTableAll(tmpProds, Convert.ToInt32(userid));
                     
                    }
                }

                string CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(userid));
                foreach (DataRow dr in tmpds_S.Tables[0].Rows)//For Records
                {
                    _stmpl_records_s = _stg_records.GetInstanceOf(cellnp);


                    _stmpl_records_s.SetAttribute("TBT_PRODUCT_ID", dr["PRODUCT_ID"]);

                    _stmpl_records_s.SetAttribute("TBT_FAMILY_ID", dr["FAMILY_ID"]);

                    _stmpl_records_s.SetAttribute("TBT_CATEGORY_ID", dr["CATEGORY_ID"]);

                    _stmpl_records_s.SetAttribute("TBT_FAMILY_NAME", dr["FAMILY_NAME"]);

                    string espath = "AllProducts////WESAUSTRALASIA////" + dr["CATEGORY_PATH"];
                    _stmpl_records_s.SetAttribute("FAMILY_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(espath)));

                    espath = "AllProducts////WESAUSTRALASIA////" + dr["CATEGORY_PATH"] + "////UserSearch1=Family Id=" + dr["FAMILY_ID"];
                    _stmpl_records_s.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(espath)));

                    //string sss = HttpUtility.UrlDecode(objSecurity.StringDeCrypt("K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT0qj5dNwgdSlMUS%2fCFrvtBbBsxmiH4H4c8zrux6WqQRHPewdDM24DI95rC7VuvDaJQS5yxSwZ08Tu%2bd1YoDeXlh1DNFePmp7MPiIpl%2fXb69sB8xKdMK63aJooZ89D8unE0QBAZ1hU2LTNHfzS3%2fFnyH"));
                    //string ss1 = objSecurity.StringDeCrypt("K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT0qj5dNwgdSlMUS%2fCFrvtBbBsxmiH4H4c8zrux6WqQRHPewdDM24DI95rC7VuvDaJQS5yxSwZ08Tu%2bd1YoDeXlh1DNFePmp7MPiIpl%2fXb69sB8xKdMK63aJooZ89D8unE0QBAZ1hU2LTNHfzS3%2fFnyH");

                    _stmpl_records_s.SetAttribute("TBT_CODE", dr["CODE"]);
                    if (dr["DESCRIPTION"].ToString().Length > 80)
                        _stmpl_records_s.SetAttribute("TBT_DESCRIPTIONS", dr["DESCRIPTION"].ToString().Substring(0, 80) + "...");
                    else
                        _stmpl_records_s.SetAttribute("TBT_DESCRIPTIONS", dr["DESCRIPTION"]);

                    _stmpl_records_s.SetAttribute("TBT_YOURCOST", dr["COSt"]);

                    _stmpl_records_s.SetAttribute("TBT_STOCK_STATUS_2", true);
                    _stmpl_records_s.SetAttribute("TBT_TWEB_IMAGE1", GetImagePath(dr["TWEB Image1"].ToString(), ""));
                 //   _stmpl_records_s.SetAttribute("TBT_MIN_ORD_QTY", dr["MIN_ORD_QTY"]);
                    _stmpl_records_s.SetAttribute("TBT_QTY_AVAIL", dr["QTY_AVAIL"]);

                    _stmpl_records_s.SetAttribute("TBT_ECOMENABLED", isenable);

                    if (Convert.ToInt32(userid) > 0)
                    {
                        try
                        {
                            DataSet Sqltbs = objhelperDb.GetProductPriceEA("", dr["PRODUCT_ID"].ToString(), userid);
                            if (Sqltbs != null)
                            {

                                //  objErrorHandler.CreateLog(dr["PRODUCT_CODE"].ToString() + dr["STOCK_STATUS_DESC"].ToString() + dr["PROD_STOCK_STATUS"].ToString() + CustomerType + dr["PROD_STOCK_FLAG"].ToString() + dr["ETA"].ToString());
                                PriceTable = objFamilyServices.AssemblePriceTable(objHelperServices.CI(dr["PRODUCT_ID"].ToString()), pricecode, dr["CODE"].ToString(), Sqltbs.Tables[0].Rows[0]["PROD_STK_STATUS_DSC"].ToString(), Sqltbs.Tables[0].Rows[0]["PROD_STOCK_STATUS"].ToString(), CustomerType, Convert.ToInt32(userid), Sqltbs.Tables[0].Rows[0]["PROD_STOCK_FLAG"].ToString(), Sqltbs.Tables[0].Rows[0]["ETA"].ToString(), dsPriceTableAll);
                            }
                        }
                        catch (Exception ex)
                        {
                            objErrorHandler.CreateLog(ex.ToString());
                        }
                    }
                    _stmpl_records_s.SetAttribute("TBT_PRODUCT_PRICE_TABLE", PriceTable);



                    lstrecords_specials[ictrecords] = new TBWDataList(_stmpl_records_s.ToString());
                    ictrecords++;
                }

                _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\Main");

                _stmpl_container.SetAttribute("TBWDataList_s", lstrecords_specials);




                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return ProdimageRreplaceImages(sHTML);
        }

        
        //public string ST_NewProduct_Load()
        //{
        //    string sHTML = string.Empty;

        //    try
        //    {
        //        StringTemplateGroup _stg_container = null;
        //        StringTemplateGroup _stg_records = null;
        //        StringTemplate _stmpl_container = null;
        //        StringTemplate _stmpl_records = null;
        //        StringTemplate _stmpl_recordstemp = new StringTemplate();
        //        //  StringTemplate _stmpl_records1 = null;
        //        // StringTemplate _stmpl_recordsrows = null;
        //        TBWDataList[] lstrecords = new TBWDataList[0];

        //        DataSet dsrecords = new DataSet();
        //        // DataTable dt = null;
        //        DataRow[] drs = null;
        //        string userid = "0";
        //        if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
        //            userid = HttpContext.Current.Session["USER_ID"].ToString();



        //        DataSet tmpds = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_PRODUCT_WES 20,0," + userid);

        //        if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
        //        {
        //            //tmpds.Tables[0].Columns.Add("URL_RW_PATH", typeof(string));
        //            //foreach (DataRow dr in tmpds.Tables[0].Rows)
        //            //{
        //            //    dr["URL_RW_PATH"] = objHelperServices.Cons_NewURlASPX("", "AllProducts////WESAUSTRALASIA////" + "////" + dr["CATEGORY_PATH"].ToString() + "////" + dr["FAMILY_ID"].ToString() + "=" + dr["FAMILY_name"].ToString() + "////" + dr["product_ID"].ToString() + "=" + dr["Code"].ToString(), "pd.aspx", true, "");

        //            //}

        //        }
        //        else
        //            return "";

        //        // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
        //      //  int ictrows = 0;






        //        _stg_records = new StringTemplateGroup("row", _SkinRootPath);
        //        _stg_container = new StringTemplateGroup("main", _SkinRootPath);


        //        lstrecords = new TBWDataList[tmpds.Tables[0].Rows.Count + 1];

        //        bool isenable = IsEcomenabled();

        //        int ictrecords = 0;
        //        string cellnp = _Package + "\\cell";

        //        foreach (DataRow dr in tmpds.Tables[0].Rows)//For Records
        //        {
        //            _stmpl_records = _stg_records.GetInstanceOf(cellnp);


        //            _stmpl_records.SetAttribute("TBT_PRODUCT_ID", dr["PRODUCT_ID"]);

        //            _stmpl_records.SetAttribute("TBT_FAMILY_ID", dr["FAMILY_ID"]);

        //            _stmpl_records.SetAttribute("TBT_CATEGORY_ID", dr["CATEGORY_ID"]);

        //            _stmpl_records.SetAttribute("TBT_FAMILY_NAME", dr["FAMILY_NAME"]);

        //            string espath = "AllProducts////WESAUSTRALASIA////" + dr["CATEGORY_PATH"];
        //            _stmpl_records.SetAttribute("FAMILY_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(espath)));

        //            espath = "AllProducts////WESAUSTRALASIA////" + dr["CATEGORY_PATH"] + "////UserSearch1=Family Id=" + dr["FAMILY_ID"];
        //            _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(espath)));

        //            //string sss = HttpUtility.UrlDecode(objSecurity.StringDeCrypt("K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT0qj5dNwgdSlMUS%2fCFrvtBbBsxmiH4H4c8zrux6WqQRHPewdDM24DI95rC7VuvDaJQS5yxSwZ08Tu%2bd1YoDeXlh1DNFePmp7MPiIpl%2fXb69sB8xKdMK63aJooZ89D8unE0QBAZ1hU2LTNHfzS3%2fFnyH"));
        //            //string ss1 = objSecurity.StringDeCrypt("K6WOvrM7yDGJsbKYFM1l%2b53cP8HUT0Dxg%2bPOAn2YvT0qj5dNwgdSlMUS%2fCFrvtBbBsxmiH4H4c8zrux6WqQRHPewdDM24DI95rC7VuvDaJQS5yxSwZ08Tu%2bd1YoDeXlh1DNFePmp7MPiIpl%2fXb69sB8xKdMK63aJooZ89D8unE0QBAZ1hU2LTNHfzS3%2fFnyH");

        //            _stmpl_records.SetAttribute("TBT_CODE", dr["CODE"]);
        //            if (dr["DESCRIPTION"].ToString().Length>80)
        //                _stmpl_records.SetAttribute("TBT_DESCRIPTIONS", dr["DESCRIPTION"].ToString().Substring(0,80)+"...");
        //            else
        //            _stmpl_records.SetAttribute("TBT_DESCRIPTIONS", dr["DESCRIPTION"]);

        //            _stmpl_records.SetAttribute("TBT_YOURCOST", dr["COSt"]);
                    
        //            _stmpl_records.SetAttribute("TBT_STOCK_STATUS_2", true);
        //            _stmpl_records.SetAttribute("TBT_TWEB_IMAGE1", GetImagePath(dr["TWEB Image1"].ToString(),""));
        //            _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", dr["MIN_ORD_QTY"]);
        //            _stmpl_records.SetAttribute("TBT_QTY_AVAIL", dr["QTY_AVAIL"]);

        //            _stmpl_records.SetAttribute("TBT_ECOMENABLED", isenable);




        //            lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
        //            ictrecords++;
        //        }


        //        _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\Main");



        //        _stmpl_container.SetAttribute("TBWDataList", lstrecords);
        //        sHTML = _stmpl_container.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //        sHTML = "";
        //    }
        //    return ProdimageRreplaceImages(sHTML);
        //}

        public string ST_NewProduct_Highlights_cat_list_Load()
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

                DataSet dsrecords = new DataSet();
                // DataTable dt = null;
                DataRow[] drs = null;
                string userid = "0";
                if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
                    userid = HttpContext.Current.Session["USER_ID"].ToString();



                DataSet tmpds = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_PRODUCT_WES 6,0," + userid);

                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    //tmpds.Tables[0].Columns.Add("URL_RW_PATH", typeof(string));
                    //foreach (DataRow dr in tmpds.Tables[0].Rows)
                    //{
                    //    dr["URL_RW_PATH"] = objHelperServices.Cons_NewURlASPX("", "AllProducts////WESAUSTRALASIA////" + "////" + dr["CATEGORY_PATH"].ToString() + "////" + dr["FAMILY_ID"].ToString() + "=" + dr["FAMILY_name"].ToString() + "////" + dr["product_ID"].ToString() + "=" + dr["Code"].ToString(), "pd.aspx", true, "");

                    //}

                }
                else
                    return "";

                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                int ictrows = 0;






                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);


                lstrecords = new TBWDataList[tmpds.Tables[0].Rows.Count + 1];

                bool isenable = IsEcomenabled();

                int ictrecords = 0;
                string nphcl = _Package + "\\cell";
                foreach (DataRow dr in tmpds.Tables[0].Rows)//For Records
                {
                    _stmpl_records = _stg_records.GetInstanceOf(nphcl);


                    _stmpl_records.SetAttribute("TBT_PRODUCT_ID", dr["PRODUCT_ID"].ToString());

                    _stmpl_records.SetAttribute("TBT_FAMILY_ID", dr["FAMILY_ID"].ToString());

                    _stmpl_records.SetAttribute("TBT_CATEGORY_ID", dr["CATEGORY_ID"].ToString());

                    _stmpl_records.SetAttribute("TBT_FAMILY_NAME", dr["FAMILY_NAME"].ToString());

                    string espath = "AllProducts////WESAUSTRALASIA////" + dr["CATEGORY_PATH"].ToString();
                    _stmpl_records.SetAttribute("FAMILY_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(espath)));

                    espath = "AllProducts////WESAUSTRALASIA////" + dr["CATEGORY_PATH"].ToString() + "////UserSearch1=Family Id=" + dr["FAMILY_ID"].ToString();
                    _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(espath)));


                    _stmpl_records.SetAttribute("TBT_CODE", dr["CODE"].ToString());
                    if (dr["DESCRIPTION"].ToString().Length > 80)
                        _stmpl_records.SetAttribute("TBT_DESCRIPTIONS", dr["DESCRIPTION"].ToString().Substring(0, 80) + "...");
                    else
                        _stmpl_records.SetAttribute("TBT_DESCRIPTIONS", dr["DESCRIPTION"].ToString());

                    _stmpl_records.SetAttribute("TBT_YOURCOST", dr["COSt"].ToString());

                    _stmpl_records.SetAttribute("TBT_STOCK_STATUS_2", true);
                    _stmpl_records.SetAttribute("TBT_TWEB_IMAGE1", GetImagePath(dr["TWEB Image1"].ToString(), ""));
                    _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", dr["MIN_ORD_QTY"].ToString());
                    _stmpl_records.SetAttribute("TBT_QTY_AVAIL", dr["QTY_AVAIL"].ToString());

                    _stmpl_records.SetAttribute("TBT_ECOMENABLED", isenable);




                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                    ictrecords++;
                }


                _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\Main");



                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return ProdimageRreplaceImages(sHTML);
        }


        public string ST_Family_Load(DataSet dsrecords)
        {
            string sHTML = string.Empty;

            try
            {
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;

                TBWDataList[] lstrecords = new TBWDataList[0];

                //DataSet dsrecords = new DataSet();

                DataRow[] drs = null;




                // dsrecords = (DataSet)HttpContext.Current.Session["FamilyProduct"];

                if (dsrecords == null || dsrecords.Tables.Count <= 0 || dsrecords.Tables[0].Rows.Count <= 0)
                    return "";

                int ictrows = 0;





                string attname = "";

                _stg_container = new StringTemplateGroup("main", _SkinRootPath);

                if (dsrecords.Tables[0].Rows[0]["STATUS"].ToString().ToUpper() == "TRUE")
                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\main1");
                else if (dsrecords.Tables[0].Rows[0]["STATUS"].ToString().ToUpper() == "FALSE")
                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\main");
                else
                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\main2");




                _stmpl_container.SetAttribute("TBT_FAMILY_ID", dsrecords.Tables[0].Rows[0]["FAMILY_ID"].ToString());
                _stmpl_container.SetAttribute("TBT_FAMILY_NAME", dsrecords.Tables[0].Rows[0]["FAMILY_NAME"].ToString());
                HttpContext.Current.Session["TBT_FAMILY_NAME_ALT"] = "";
                HttpContext.Current.Session["TBT_FAMILY_NAME_ALT"] = dsrecords.Tables[0].Rows[0]["FAMILY_NAME"].ToString();
                _stmpl_container.SetAttribute("TBT_PROD_COUNT", dsrecords.Tables[0].Rows[0]["PROD_COUNT"].ToString());
                _stmpl_container.SetAttribute("TBT_FAMILY_PROD_COUNT", dsrecords.Tables[0].Rows[0]["FAMILY_PROD_COUNT"].ToString());
                string desc1 = string.Empty;
                string descall = string.Empty;
                string descallstring = string.Empty;
                foreach (DataRow dr in dsrecords.Tables[0].Rows)//For Records
                {

                    attname = dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper();


                    desc1 = "";
                    if (attname.Equals("DESCRIPTIONS") || attname.Equals("FEATURES") || attname.Equals("SPECIFICATION") || attname.Equals("SPECIFICATIONS") || attname.Equals("APPLICATIONS") || attname.Equals("NOTES"))
                    {
                        desc1 = dr["STRING_VALUE"].ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                        desc1 = desc1.ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;");
                    }
                    //modified by indu to display short description in family page
       else if (attname.Contains("SHORT_DESCRIPTION"))
                    {
                        _stmpl_container.SetAttribute("TBT_" + attname, dr["STRING_VALUE"].ToString());
                    }

                    if (!desc1.Equals(""))
                        descall = descall + desc1 + "<br/><br/>";
                    // _stmpl_container.SetAttribute(attname, dr["STRING_VALUE"].ToString());



                }

                if (!desc1.Equals(""))
                {
                    descall = descall.Trim();
                    descall = descall.Substring(0, descall.Length - 5);
                }
                if (descall.Length > 1080)
                {
                    descallstring = descall.Substring(0, 1080).ToString();
                    _stmpl_container.SetAttribute("TBT_MORE", descallstring);
                    _stmpl_container.SetAttribute("TBT_MENU_ID", "2");
                    descall = descall.Substring(0, descall.Length).ToString();
                    _stmpl_container.SetAttribute("TBT_DESCALL", descall);

                }
                else
                {
                    _stmpl_container.SetAttribute("TBT_DESCALL", descall);
                    _stmpl_container.SetAttribute("TBT_MENU_ID", "2");

                    _stmpl_container.SetAttribute("TBT_MORE", descall);
                }
                if (descall.Length > 1080)
                    _stmpl_container.SetAttribute("TBT_MORE_SHOW", true);
                else
                    _stmpl_container.SetAttribute("TBT_MORE_SHOW", false);




                DataSet tempdscat = (DataSet)HttpContext.Current.Session["LHSAttributes"];
                if (tempdscat != null && tempdscat.Tables.Count > 0)
                {
                    if (tempdscat.Tables.Count == 1 && tempdscat.Tables[0].Rows.Count > 0)
                        _stmpl_container.SetAttribute("TBT_LHS_TIP", true);
                    else if (tempdscat.Tables.Count > 1 && tempdscat.Tables[1].Rows.Count > 0)
                        _stmpl_container.SetAttribute("TBT_LHS_TIP", true);
                    else
                        _stmpl_container.SetAttribute("TBT_LHS_TIP", false);
                }
                else
                    _stmpl_container.SetAttribute("TBT_LHS_TIP", false);

                if (HttpContext.Current.Request.QueryString["fid"] != null)
                {
                    _fid = HttpContext.Current.Request.QueryString["fid"].ToString();


                    //new code for family cloning
                    string eapath = EasyAsk.GetFamilyEAPATH();
                    if (eapath != "")
                    {
                        string[] strs = eapath.Split(new string[] { "////" }, StringSplitOptions.None);
                        if (strs.Length > 0)
                        {
                            DataSet tmpds = objCategoryServices.GetCategotyID(strs[strs.Length - 1].ToString());
                            if (tmpds != null && tmpds.Tables.Count > 0)
                                _stmpl_container.SetAttribute("TBT_CATEGORY_ID", tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString());

                        }
                        eapath = "AllProducts////WESAUSTRALASIA" + eapath;
                        _stmpl_container.SetAttribute("TBT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath)));
                    }
                    else
                    {
                        DataSet tmpds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, _fid, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);

                        if (tmpds != null && tmpds.Tables.Count > 0)
                        {
                            _stmpl_container.SetAttribute("TBT_CATEGORY_ID", tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString());
                            eapath = "AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString();
                            _stmpl_container.SetAttribute("TBT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath)));
                        }
                    }
                }

                GetFamilyMultipleImages(Convert.ToInt32(paraFID), _stmpl_container, dsrecords);
                //GetFamilyMultipleImages(Convert.ToInt32(paraFID), _stmpl_container);

                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return ProdimageRreplaceImages(sHTML);
        }
        public string ST_SubFamily_Load(DataSet tempDs)
        {
            string sHTML = string.Empty;

            try
            {
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;

                TBWDataList[] lstrecords = new TBWDataList[0];

                DataSet dsrecords = new DataSet();

                DataRow[] drs = null;




                //DataSet tempDs = (DataSet)HttpContext.Current.Session["FamilyProduct"];
                if (tempDs != null && tempDs.Tables["FamilyPro"] != null)
                {
                    drs = tempDs.Tables["FamilyPro"].Select("FAMILY_ID='" + paraValue + "'");
                    //if (dr.Length > 0)
                    //{
                    //    dsrecords = new DataSet();
                    //    dsrecords.Tables.Add(dr.CopyToDataTable().Copy());
                    //}

                }
                if (drs == null || drs.Length <= 0)
                    return "";

               // int ictrows = 0;





               // string attname = "";

                _stg_container = new StringTemplateGroup("main", _SkinRootPath);
                _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\Main");

                _stmpl_container.SetAttribute("TBT_FAMILY_NAME", drs[0]["FAMILY_NAME"].ToString());
                _stmpl_container.SetAttribute("TBT_SHORT_DESCRIPTION", drs[0]["FAMILY_SHORT_DESC"].ToString());
                _stmpl_container.SetAttribute("TBT_DESCRIPTIONS", drs[0]["FAMILY_DESC"].ToString());

                FileInfo Fil = new FileInfo(strProdImages + drs[0]["FAMILY_TH_IMAGE"].ToString().Replace("\\", "/"));
                if (Fil.Exists)
                {
                    _stmpl_container.SetAttribute("TBT_TFWEB_IMAGE1", drs[0]["FAMILY_TH_IMAGE"].ToString().Replace("\\", "/"));
                    _stmpl_container.SetAttribute("TBT_FWEB_IMAGE1",  objHelperService.SetImageFolderPath(drs[0]["FAMILY_TH_IMAGE"].ToString().Replace("\\", "/"), "_th", "_Images_200") );

                }
                else
                {
                    _stmpl_container.SetAttribute("TBT_TFWEB_IMAGE1", "/images/noimage.gif");
                    _stmpl_container.SetAttribute("TBT_FWEB_IMAGE1", "");

                }


                //foreach (DataRow dr in dsrecords.Tables[0].Rows)//For Records
                //{

                //    attname = "TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper();

                //     if (dr["ATTRIBUTE_TYPE"].ToString().Equals("3") || dr["ATTRIBUTE_TYPE"].ToString().Equals("9"))
                //     {
                //         FileInfo Fil = new FileInfo(strProdImages + dr["STRING_VALUE"].ToString().Replace("\\", "/"));

                //         if (Fil.Exists)
                //         {
                //           _stmpl_container.SetAttribute(attname , dr["STRING_VALUE"].ToString().Replace("\\", "/"));
                //         }
                //         else
                //             _stmpl_container.SetAttribute(attname, "/images/noimage.gif");

                //     }
                //     else
                //       _stmpl_container.SetAttribute(attname, dr["STRING_VALUE"].ToString());



                //}

                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return ProdimageRreplaceImages(sHTML);
        }

        protected string GetImagePath(object Path, string tosize )
        {
            string retpath;
            System.IO.FileInfo Fil = new System.IO.FileInfo(strProdImages + Path.ToString());
            if (Fil.Exists)
            {   if (tosize=="TH50")
                    retpath = objHelperServices.SetImageFolderPath(Path.ToString().Replace("\\", "/"),"_th","_th50");
                else
                    retpath = Path.ToString().Replace("\\", "/");
                //retpath = objHelperServices.SetImageFolderPath(Path.ToString().Replace("\\", "/"), "_th", "_Images_200");
            }
            else
                retpath = "/images/noimage.gif";

            return retpath;
        }
        public string ProdimageRreplaceImages(string shtml)
        {

            _RenderedHTML = shtml.ToString().Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");


            if (_Package == "NEWPRODUCT" || _Package == "CATEGORYLISTIMG" || _Package == "CSFAMILYPAGE" || _Package == "CSFAMILYPAGEWITHSUBFAMILY" || _Package == "PRODUCT" || _Package == "NEWPRODUCTNAV") //|| _Package == "CSFAMILYPAGE" || _Package == "CSFAMILYPAGEWITHSUBFAMILY" || _Package == "PRODUCT"
            {

                if (shtml.ToString().Contains("data-original=\"prodimages\""))
                {
                    if (_Package == "CSFAMILYPAGE")
                        _RenderedHTML = shtml.ToString().Replace("data-original=\"prodimages\"", "data-original=\"images/noimage.gif\"");
                    else
                        _RenderedHTML = shtml.ToString().Replace("data-original=\"prodimages\"", "data-original=\"images/noimage.gif\"");
                    _RenderedHTML = _RenderedHTML.Replace("href=\"javascript:Zoom('prodimages')\"", "href=\"#\"");
                }
                if (shtml.ToString().Contains("data-original=\"\""))
                {
                    _RenderedHTML = shtml.ToString().Replace("data-original=\"\"", "data-original=\"images/noimage.gif\"");
                    _RenderedHTML = _RenderedHTML.Replace("href=\"javascript:Zoom('')\"", "href=\"#\"");
                }
            }
            else
            {
                if (shtml.ToString().Contains("src=\"prodimages\""))
                {
                    if (_Package == "CSFAMILYPAGE")
                        _RenderedHTML = shtml.ToString().Replace("src=\"prodimages\"", "src=\"images/noimage.gif\" style=\"display:none;\"");
                    else
                        _RenderedHTML = shtml.ToString().Replace("src=\"prodimages\"", "src=\"images/noimage.gif\"");
                    _RenderedHTML = _RenderedHTML.Replace("href=\"javascript:Zoom('prodimages')\"", "href=\"#\"");
                }
                if (shtml.ToString().Contains("src=\"\""))
                {
                    _RenderedHTML = shtml.ToString().Replace("src=\"\"", "src=\"images/noimage.gif\"");
                    _RenderedHTML = _RenderedHTML.Replace("href=\"javascript:Zoom('')\"", "href=\"#\"");
                }
            }

            return _RenderedHTML;
        }

        private bool GetStockDetails(StringTemplate st, string Pid)
        {
            HelperDB objhelperDb = new HelperDB();
            try
            {
                string user_id = string.Empty;
                string order_id = string.Empty;
                int no = 0;
                int availabilty = 0;
                string availabilty1 = string.Empty;
                string sqlexec = "exec SP_CHECK_STOCK_STATUS '" + Pid.ToString() + "' ";
                objErrorHandler.CreateLog("sqlexec " + sqlexec);
                DataSet Dsall = objhelperDb.GetDataSetDB(sqlexec);

                if (Dsall.Tables[0].Rows[0]["AVAILABILTY_TOTAL"] != null && Dsall.Tables[0].Rows[0]["AVAILABILTY_TOTAL"].ToString() != "" && Dsall.Tables[0].Rows[0]["AVAILABILTY_TOTAL"].ToString().ToUpper().Trim() != "CALL")
                {
                    availabilty1 = Dsall.Tables[0].Rows[0]["AVAILABILTY_TOTAL"].ToString();
                    availabilty1 = availabilty1.Replace("+", "");
                    availabilty = Convert.ToInt32(availabilty1.ToString());
                }
                if ((Dsall.Tables[0].Rows[0]["SUPPLIER_ITEM_CODE"] == null || Dsall.Tables[0].Rows[0]["SUPPLIER_ITEM_CODE"].ToString().Trim() == "") || (Dsall.Tables[0].Rows[0]["SUPPLIER_ID"] == null || Dsall.Tables[0].Rows[0]["SUPPLIER_ID"].ToString().Trim() == ""))
                {
                    return true;
                }
                if (Dsall.Tables[0].Rows[0]["product_id"] != null && Dsall.Tables[0].Rows[0]["product_id"].ToString() == string.Empty && Dsall.Tables[0].Rows[0]["PRODUCT_CODE"] != null && Dsall.Tables[0].Rows[0]["PRODUCT_CODE"].ToString() == string.Empty)
                {
                    if (Dsall.Tables[0].Rows[0]["SUPPLIER_SHIP_DAYS"] != null && Dsall.Tables[0].Rows[0]["SUPPLIER_SHIP_DAYS"].ToString() != "")
                    {
                        int shipping_time = Convert.ToInt32(Dsall.Tables[0].Rows[0]["SUPPLIER_SHIP_DAYS"].ToString());
                        objErrorHandler.CreateLog("SUPPLIER_SHIP_DAYS " + shipping_time + "Pid:" + Pid);
                        string supplier_shipping_time = string.Empty;

                        if (shipping_time > 1)
                        {
                            supplier_shipping_time = "1 - " + shipping_time + " Days";
                        }
                        else if (shipping_time == 1)
                        {
                            supplier_shipping_time = " 1 Day";
                        }
                        
                        if (shipping_time > 0 && shipping_time <= 14)
                        {
                            st.SetAttribute("TBT_STOCK_STATUS_2", true);
                            st.SetAttribute("TBT_ISINSTOCK", true);
                            st.SetAttribute("TBT_ISINSTOCK_STAUS", "Please Call");
                            st.SetAttribute("TBT_SHIPPINGDAYS", supplier_shipping_time);
                            st.SetAttribute("TBT_SHOW_SHIPPINGDAYS", true);
                            objErrorHandler.CreateLog("supplier_shipping_time " + supplier_shipping_time);
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    return true;
                }
                else if (availabilty > 0)
                {
                    objErrorHandler.CreateLog("availabilty " + availabilty);
                    int avail_total = Convert.ToInt32(availabilty);
                    int stock_cutoff = Convert.ToInt32(Dsall.Tables[0].Rows[0]["WEB_STOCK_CUTOFF"]);
                    int shipping_time = Convert.ToInt32(Dsall.Tables[0].Rows[0]["SUPPLIER_SHIPPING_TIME"].ToString());
                    st.SetAttribute("TBT_STOCK_STATUS_2", true);
                    string supplier_shipping_time = string.Empty;
                    if (shipping_time > 1)
                    {
                        supplier_shipping_time = "1 - " + shipping_time + " Days";
                    }
                    else if (shipping_time == 1)
                    {
                        supplier_shipping_time = " 1 Day";
                    }
                    if (avail_total >= stock_cutoff)
                    {
                        st.SetAttribute("TBT_SHOW_SHIPPINGDAYS", true);
                        st.SetAttribute("TBT_ISINSTOCK", true);
                        st.SetAttribute("TBT_ISINSTOCK_STAUS", "In Stock");
                        st.SetAttribute("TBT_SHIPPINGDAYS", supplier_shipping_time);
                    }
                    else if (avail_total < stock_cutoff)
                    {
                        st.SetAttribute("TBT_SHOW_SHIPPINGDAYS", true);
                        st.SetAttribute("TBT_PLEASE_CALL", true);
                        st.SetAttribute("TBT_SHIPPINGDAYS", supplier_shipping_time);
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                objErrorHandler.CreateLog(ex.ToString());
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            return true;
        }
    
    }

            //private DataSet FamilyFilterFlatTable(DataSet flatDataset)
            //{

            //    {
            //        StringBuilder SQLstring = new StringBuilder();
            //        DataSet oDsFamilyFilter = new DataSet();
            //        string SQLString = " SELECT FAMILY_FILTERS FROM TB_CATALOG WHERE  CATALOG_ID = " + CATALOG_ID + " ";
            //        SqlDataAdapter da = new SqlDataAdapter(SQLString, _DBConnectionString);
            //        da.Fill(oDsFamilyFilter);
            //        string sFamilyFilter = string.Empty;
            //        if (oDsFamilyFilter.Tables[0].Rows.Count > 0 && oDsFamilyFilter.Tables[0].Rows[0].ItemArray[0].ToString() != string.Empty)
            //        {
            //            sFamilyFilter = oDsFamilyFilter.Tables[0].Rows[0].ItemArray[0].ToString();
            //            XmlDocument xmlDOc = new XmlDocument();
            //            xmlDOc.LoadXml(sFamilyFilter);
            //            XmlNode rNode = xmlDOc.DocumentElement;

            //            if (rNode.ChildNodes.Count > 0)
            //            {
            //                for (int i = 0; i < rNode.ChildNodes.Count; i++)
            //                {
            //                    XmlNode TableDataSetNode = rNode.ChildNodes[i];

            //                    if (TableDataSetNode.HasChildNodes)
            //                    {
            //                        if (TableDataSetNode.ChildNodes[2].InnerText == " ")
            //                        {
            //                            TableDataSetNode.ChildNodes[2].InnerText = "=";
            //                        }
            //                        if (TableDataSetNode.ChildNodes[0].InnerText == " ")
            //                        {
            //                            TableDataSetNode.ChildNodes[0].InnerText = "0";
            //                        }
            //                        string stringval = TableDataSetNode.ChildNodes[3].InnerText.Replace("'", "''");
            //                        if (TableDataSetNode.ChildNodes[4].InnerText != "NONE")
            //                        {
            //                            SQLstring.Append("SELECT DISTINCT FAMILY_ID FROM [FAMILY DESCRIPTION](" + CATALOG_ID + ") WHERE (STRING_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ") " + "\n");
            //                        }
            //                        else
            //                        {
            //                            SQLstring.Append("SELECT DISTINCT FAMILY_ID FROM [FAMILY DESCRIPTION](" + CATALOG_ID + ") WHERE  (STRING_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ")" + "\n");
            //                        }


            //                    }
            //                    if (TableDataSetNode.ChildNodes[4].InnerText == "NONE")
            //                    {
            //                    }
            //                    if (TableDataSetNode.ChildNodes[4].InnerText == "AND")
            //                    {
            //                        SQLstring.Append(" INTERSECT \n");
            //                    }
            //                    if (TableDataSetNode.ChildNodes[4].InnerText == "OR")
            //                    {
            //                        SQLstring.Append(" UNION \n");
            //                    }

            //                }

            //            }

            //        }
            //        string familyFiltersql = SQLstring.ToString();

            //        if (familyFiltersql.Length > 0)
            //        {
            //            string s = "SELECT FAMILY_ID FROM FAMILY(" + CATALOG_ID + ") WHERE CATALOG_ID=" + CATALOG_ID + " AND FAMILY_ID IN\n" +
            //                  "(\n";// +
            //            //"SELECT DISTINCT FAMILY_ID\n" +
            //            //"FROM [FAMILY DESCRIPTION](" + CATALOG_ID + ")\n" +
            //            //"WHERE\n";
            //            familyFiltersql = s + familyFiltersql + "\n)";

            //            SqlDataAdapter dda = new SqlDataAdapter(familyFiltersql, _DBConnectionString);
            //            dda.Fill(oDsFamilyFilter);

            //            bool available = false;
            //            DataSet AvailableDs = flatDataset;
            //            for (int rowCount = 0; rowCount < flatDataset.Tables[0].Rows.Count; rowCount++)
            //            {//foreach (DataRow odr in flatDataset.Tables[0].Rows)
            //                DataRow odr = flatDataset.Tables[0].Rows[rowCount];
            //                available = false;
            //                foreach (DataRow dr in oDsFamilyFilter.Tables[0].Rows)
            //                {
            //                    if (dr["FAMILY_ID"].ToString() == odr["FAMILY_ID"].ToString() || dr["FAMILY_ID"].ToString() == odr["SUBFAMILY_ID"].ToString())
            //                    {
            //                        available = true;
            //                    }

            //                }
            //                if (available == false)
            //                {
            //                    //string cmdd = " DELETE FROM TBWC_SEARCH_PROD_LIST WHERE FAMILY_ID = " + odr["FAMILY_ID"].ToString() + " OR FAMILY_ID = " + odr["SUBFAMILY_ID"].ToString() + " AND  USER_SESSION_ID='" + HttpContext.Current.Session.SessionID + "'";
            //                    //SqlConnection _SQLConn = new SqlConnection(_DBConnectionString);
            //                    //SqlCommand pscmd = new SqlCommand(cmdd, _SQLConn);
            //                    //pscmd.CommandType = CommandType.Text;
            //                    //int valr = pscmd.ExecuteNonQuery();
            //                    odr.Delete();
            //                    flatDataset.AcceptChanges();
            //                    rowCount--;
            //                }

            //            }


            //        }

            //    }
            //    //ProductFilterFlatTable(flatDataset);
            //    return flatDataset;
            //}


        public class ProductImage
        {
            public string LargeImage { get; set; }
            public string Thumpnail { get; set; }
            public string MediumImage { get; set; }
            public string SmallImage { get; set; }
        }

        public class ProductDetails
        {
            public string AttributeName { get; set; }
            public string SpecValue { get; set; }
            public int AttributeID { get; set; }
            public int SortOrder { get; set; }
        }

        public class ProductDesc
        {
            public string AttributeName { get; set; }
            public string SpecValue { get; set; }
            public int AttributeID { get; set; }
        }
        /*********************************** J TECH CODE ***********************************/
}
