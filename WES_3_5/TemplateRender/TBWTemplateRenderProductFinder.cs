using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
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
namespace TradingBell.WebCat.TemplateRender
{
    /*********************************** J TECH CODE ***********************************/
    public class TBWTemplateRenderProductFinder
    {
        /*********************************** DECLARATION ***********************************/
        EasyAsk_WES EasyAsk = new EasyAsk_WES();
        ErrorHandler objErrorHandler = new ErrorHandler();
        CategoryServices objCategoryServices = new CategoryServices();
        OrderServices objOrderServices = new OrderServices();
        HelperServices objHelperServices = new HelperServices();
        FamilyServices objFamilyServices = new FamilyServices();
        Dictionary<string, TBWDataList[]> _dict_inner_html = new Dictionary<string, TBWDataList[]>();

        private StringTemplateGroup _stg_container = null;
        private StringTemplateGroup _stg_records = null;
        private StringTemplate _stmpl_container = null;
        private StringTemplate _stmpl_records = null;

      
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
        public string paraValue = "";
        public string paraPID = "";
        public string paraFID = "";
        public string paraCID = "";
        private string _cartitem = "";
        private string _CATALOG_ID = "";
        private HelperServices objHelperService = new HelperServices();
        private Security objSecurity = new Security();
        private HelperDB objHelperDB = new HelperDB();
        private ConnectionDB objConnectionDB = new ConnectionDB();
        string _fid = "";
        string downloadST = "";
        bool isdownload = false;
        public string WesNewsCategoryId = System.Configuration.ConfigurationManager.AppSettings["WESNEWS_CATEGORY_ID"].ToString();
        public string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();
        string strProdImages = HttpContext.Current.Server.MapPath("ProdImages");
        string _SkinRootPath = HttpContext.Current.Server.MapPath("Templates");
        //END TBWC_PACKAGE table info
        public string ST_CableL_Load(DataTable datasource, string selectedCable, string selectedCable_image )
        {
            string sHTML = "";

    
            try
            {
                DataSet dsbotmsname = new DataSet();
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                //  StringTemplate _stmpl_records1 = null;
                // StringTemplate _stmpl_recordsrows = null;
                TBWDataList[] lstrecords = new TBWDataList[0];

               // DataSet dsrecords = new DataSet();
                // DataTable dt = null;
                DataRow[] drs = null;

                //dsrecords = EasyAsk.GetCategoryAndBrand("MainCategory");

                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                int ictrows = 0;






                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);

                string Lis = "";

               // lstrecords = new TBWDataList[datasource.Rows.Count + 1];



                int ictrecords = 0;
                if (datasource != null && datasource.Rows.Count > 0)
                {


                  
                    if (selectedCable != "")
                    {
                        _stmpl_container = _stg_container.GetInstanceOf("ProductFinder" + "\\" + "Cable1Leftselect");

                        _stmpl_container.SetAttribute("SELECTED_CABLE", selectedCable);
                        _stmpl_container.SetAttribute("CABLE_IMAGE", selectedCable_image);
                        //_stmpl_container.SetAttribute("CABLE_LI_LIST", Lis);
                    }
                    else
                    {
                        _stmpl_container = _stg_container.GetInstanceOf("ProductFinder" + "\\" + "Cable1Left");

                        _stmpl_container.SetAttribute("SELECTED_CABLE", "select..");
                       // _stmpl_container.SetAttribute("CABLE_LI_LIST", Lis);
                    }
                }
                else
                {
                    _stmpl_container = _stg_container.GetInstanceOf("ProductFinder" + "\\" + "Cable1Left");

                    _stmpl_container.SetAttribute("SELECTED_CABLE", "");
                   // _stmpl_container.SetAttribute("CABLE_LI_LIST", "");
                }          
                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return sHTML;
        }


        public string ST_CableLImage_Load(DataTable datasource, string strsearchvalue, string Connector_type)
        {
            string sHTML = "";

        
            try
            {
                DataSet dsbotmsname = new DataSet();
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                //  StringTemplate _stmpl_records1 = null;
                // StringTemplate _stmpl_recordsrows = null;
                TBWDataList[] lstrecords = new TBWDataList[0];

                // DataSet dsrecords = new DataSet();
                // DataTable dt = null;
                DataRow[] drs = null;

                //dsrecords = EasyAsk.GetCategoryAndBrand("MainCategory");

                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                int ictrows = 0;






                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);

                string Lis = "";

                // lstrecords = new TBWDataList[datasource.Rows.Count + 1];


                Connector_type = Connector_type.ToLower();
                int ictrecords = 0;
                if (datasource != null && datasource.Rows.Count > 0)
                {


                    foreach (DataRow dr in datasource.Rows)//For Records
                    {

                        if (strsearchvalue != "")
                        {
                            if (dr["Connector_type"].ToString().ToLower() == Connector_type)
                            {
                                if (dr["CableL"].ToString().ToLower().Contains(strsearchvalue.ToLower()))
                                {

                                    _stmpl_records = _stg_records.GetInstanceOf("ProductFinder" + "\\" + "Cable1LeftImage");

                                    _stmpl_records.SetAttribute("CABLE_IMAGE", dr["Image_Path"].ToString());
                                    _stmpl_records.SetAttribute("CABLE_NAME", dr["CableL"].ToString());
                                    _stmpl_records.SetAttribute("CONNECTOR_TYPE", Connector_type);
                                    _stmpl_records.SetAttribute("URL_CABLE_NAME", HttpUtility.UrlEncode(dr["CableL"].ToString()));

                                    Lis = Lis + _stmpl_records.ToString();
                                }
                            }
                        }
                        else
                        {
                            if (dr["Connector_type"].ToString().ToLower() == Connector_type)
                            {
                                _stmpl_records = _stg_records.GetInstanceOf("ProductFinder" + "\\" + "Cable1LeftImage");

                                _stmpl_records.SetAttribute("CABLE_IMAGE", dr["Image_Path"].ToString());
                                _stmpl_records.SetAttribute("CABLE_NAME", dr["CableL"].ToString());
                                _stmpl_records.SetAttribute("CONNECTOR_TYPE", Connector_type);
                                _stmpl_records.SetAttribute("URL_CABLE_NAME", HttpUtility.UrlEncode(dr["CableL"].ToString()));
                                Lis = Lis + _stmpl_records.ToString();
                            }
                        }
                    }
                     sHTML=Lis;
                }
                else
                {
                    sHTML="";
                }
                
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return sHTML;
        }
        public string ST_CableRImage_Load(DataTable datasource, string Cable_left, string Ea, string strsearchvalue, string Connector_type)
        {
            string sHTML = "";

           
            try
            {
                DataSet dsbotmsname = new DataSet();
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                //  StringTemplate _stmpl_records1 = null;
                // StringTemplate _stmpl_recordsrows = null;
                TBWDataList[] lstrecords = new TBWDataList[0];

                // DataSet dsrecords = new DataSet();
                // DataTable dt = null;
                DataRow[] drs = null;

                //dsrecords = EasyAsk.GetCategoryAndBrand("MainCategory");

                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                int ictrows = 0;






                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);

                string Lis = "";

                // lstrecords = new TBWDataList[datasource.Rows.Count + 1];

                Connector_type = Connector_type.ToLower();

                int ictrecords = 0;
                if (datasource != null && datasource.Rows.Count > 0)
                {


                    foreach (DataRow dr in datasource.Rows)//For Records
                    {

                        if (strsearchvalue != "")
                        {
                            if (dr["Connector_type"].ToString().ToLower() == Connector_type)
                            {
                                if (dr["CableLR"].ToString().ToLower().Contains(strsearchvalue.ToLower()))
                                {
                                    _stmpl_records = _stg_records.GetInstanceOf("ProductFinder" + "\\" + "Cable2RightImage");
                                    _stmpl_records.SetAttribute("CABLE_LEFT", HttpUtility.UrlEncode(Cable_left));
                                    _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea)));
                                    _stmpl_records.SetAttribute("CABLE_IMAGE", dr["Image_Path"].ToString());
                                    _stmpl_records.SetAttribute("URL_CABLE_VALUE", HttpUtility.UrlEncode(Cable_left + ":" + dr["CableLR"].ToString()));
                                    _stmpl_records.SetAttribute("URL_CABLE_TYPE", "CableLR");
                                    _stmpl_records.SetAttribute("CONNECTOR_TYPE", Connector_type);
                                    _stmpl_records.SetAttribute("URL_CABLE_NAME", HttpUtility.UrlEncode(dr["CableLR"].ToString()));
                                    _stmpl_records.SetAttribute("CABLE_NAME", dr["CableLR"].ToString());
                                    Lis = Lis + _stmpl_records.ToString();
                                }
                            }
                        }
                        else
                        {

                            if (dr["Connector_type"].ToString().ToLower() == Connector_type)
                            {
                                _stmpl_records = _stg_records.GetInstanceOf("ProductFinder" + "\\" + "Cable2RightImage");
                                _stmpl_records.SetAttribute("CABLE_LEFT", HttpUtility.UrlEncode(Cable_left));
                                _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea)));
                                _stmpl_records.SetAttribute("CABLE_IMAGE", dr["Image_Path"].ToString());
                                _stmpl_records.SetAttribute("URL_CABLE_VALUE", HttpUtility.UrlEncode(Cable_left + ":" + dr["CableLR"].ToString()));
                                _stmpl_records.SetAttribute("URL_CABLE_TYPE", "CableLR");
                                _stmpl_records.SetAttribute("CONNECTOR_TYPE", Connector_type);
                                _stmpl_records.SetAttribute("URL_CABLE_NAME", HttpUtility.UrlEncode(dr["CableLR"].ToString()));
                                _stmpl_records.SetAttribute("CABLE_NAME", dr["CableLR"].ToString());
                                Lis = Lis + _stmpl_records.ToString();
                            }
                        }
                    }
                    sHTML = Lis;
                }
                else
                {
                    sHTML = "";
                }

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return sHTML;
        }
        public string ST_CableR_Load(DataTable datasource, string selectedCable, string selectedCable_image, string Cable_left, string Ea)
        {
            string sHTML = "";

        
            try
            {
                DataSet dsbotmsname = new DataSet();
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                //  StringTemplate _stmpl_records1 = null;
                // StringTemplate _stmpl_recordsrows = null;
                TBWDataList[] lstrecords = new TBWDataList[0];

                // DataSet dsrecords = new DataSet();
                // DataTable dt = null;
                DataRow[] drs = null;

                //dsrecords = EasyAsk.GetCategoryAndBrand("MainCategory");

                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                int ictrows = 0;






                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);

                string Lis = "";

               // lstrecords = new TBWDataList[datasource.Rows.Count + 1];



                int ictrecords = 0;
                if (datasource != null && datasource.Rows.Count > 0)
                {
                    //foreach (DataRow dr in datasource.Rows)//For Records
                    //{
                    //    _stmpl_records = _stg_records.GetInstanceOf("ProductFinder" + "\\" + "Cable2RightLi");
             

                    //    _stmpl_records.SetAttribute("CABLE_LEFT", HttpUtility.UrlEncode(Cable_left));
                    //    _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea)));
                    //    _stmpl_records.SetAttribute("CABLE_IMAGE", dr["Image_Path"].ToString());
                    //    _stmpl_records.SetAttribute("URL_CABLE_VALUE", HttpUtility.UrlEncode(Cable_left+":"+ dr["CableLR"].ToString()));
                    //    _stmpl_records.SetAttribute("URL_CABLE_TYPE", "CableLR");
                    //    _stmpl_records.SetAttribute("URL_CABLE_NAME", HttpUtility.UrlEncode(dr["CableLR"].ToString()));
                    //    _stmpl_records.SetAttribute("CABLE_NAME", dr["CableLR"].ToString());

                    //    Lis = Lis + _stmpl_records.ToString();
                    //}

                    if (selectedCable != "")
                    {
                        _stmpl_container = _stg_container.GetInstanceOf("ProductFinder" + "\\" + "Cable2Rightselect");

                        _stmpl_container.SetAttribute("SELECTED_CABLE", selectedCable);
                        _stmpl_container.SetAttribute("CABLE_IMAGE", selectedCable_image);
                       // _stmpl_container.SetAttribute("CABLE_LI_LIST", Lis);
                    }
                    else
                    {
                        _stmpl_container = _stg_container.GetInstanceOf("ProductFinder" + "\\" + "Cable2Right");

                        _stmpl_container.SetAttribute("SELECTED_CABLE", "select..");
                        if (Cable_left!="")
                            _stmpl_container.SetAttribute("SELECT_CLASS", "selected");
                        
                        //_stmpl_container.SetAttribute("CABLE_LI_LIST", Lis);
                    }

                }
                else
                {
                   
                        _stmpl_container = _stg_container.GetInstanceOf("ProductFinder" + "\\" + "Cable2Right");

                        _stmpl_container.SetAttribute("SELECTED_CABLE", "");
                        if (Cable_left != "")
                            _stmpl_container.SetAttribute("SELECT_CLASS", "selected");
                        
                       // _stmpl_container.SetAttribute("CABLE_LI_LIST", "");
                   
                }

                
                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return sHTML;
        }



        public string ST_Get_Attr_Filter()
        {
            DataSet dscat = new DataSet();

            StringTemplateGroup _stg_container = null;
            StringTemplateGroup _stg_records = null;
            StringTemplate _stmpl_container = null;
            StringTemplate _stmpl_records = null;
            StringTemplate _stmpl_records1 = null;
            StringTemplate _stmpl_recordsrows = null;
            TBWDataList[] lstrecords = new TBWDataList[0];
            TBWDataList[] lstrows = new TBWDataList[0];

            StringTemplateGroup _stg_container1 = null;
            StringTemplateGroup _stg_records1 = null;
            TBWDataList1[] lstrecords1 = new TBWDataList1[0];
            TBWDataList1[] lstrows1 = new TBWDataList1[0];
            int ictrows = 0;
            string _tsb = "";
            string _tsm = "";
            string _type = "";
            string _value = "";
            string _bname = "";
            string _searchstr = "";
            string _byp = "2";
            string _bypcat = null;


            string _pid = "";
            string _fid = "";
            string _seeall = "";
            string sHtml = "";
            //dscat = (DataSet)HttpContext.Current.Session["LHSAttributes"];
            dscat = (DataSet)HttpContext.Current.Session["TOPAttributes"];
            string Cable1 = "";
            string Cable2 = "";
            string AttName = "";
            string selectedname = "";
            if (HttpContext.Current.Request.QueryString["l"] != null)
            {
                Cable1 = HttpContext.Current.Request.QueryString["l"].ToString();
            }
            if (HttpContext.Current.Request.QueryString["r"] != null)
            {
                Cable2 = HttpContext.Current.Request.QueryString["r"].ToString();
            }

            _stg_records = new StringTemplateGroup("cell", _SkinRootPath);
            _stg_container = new StringTemplateGroup("main", _SkinRootPath);
            if (dscat != null)
            {

                if (dscat.Tables.Count > 0)
                    lstrows = new TBWDataList[dscat.Tables.Count + 1];

                for (int i = 0; i < dscat.Tables.Count; i++)
                {
                    Boolean tmpallow = true;


                    //if (dscat.Tables[i].TableName.Contains("Category"))
                    //    tmpallow = true;
                    //else if (dscat.Tables[i].TableName.Contains("Brand"))
                    //    tmpallow = false;
                    //else if (Request.QueryString["byp"] == "2")
                    //    tmpallow = true;
                    //else
                    //    tmpallow = false;
                    selectedname = "";
                    if (tmpallow == true)
                    {
                        if (dscat.Tables[i].Rows.Count > 0)
                        {
                            lstrecords = new TBWDataList[dscat.Tables[i].Rows.Count + 1];
                            lstrecords1 = new TBWDataList1[dscat.Tables[i].Rows.Count + 1];
                            int ictrecords = 0;

                            int j = 0;
                            selectedname = "";
                            AttName = dscat.Tables[i].TableName.ToString().Replace("_Table", "");
                            foreach (DataRow dr in dscat.Tables[i].Rows)//For Records
                            {



                                _stmpl_records = _stg_records.GetInstanceOf("ProductFinderAtt" + "\\" + "cell");

                                if (dscat.Tables[i].TableName.Contains("Category"))
                                {
                                    //_stmpl_records.SetAttribute("TBW_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(_parentCatID.ToString()));
                                    //_stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(dr["CATEGORY_ID"].ToString()));
                                    //_stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(ParentCatID.ToString()));


                                    _stmpl_records.SetAttribute("URL_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(dr["Category_Name"].ToString()));
                                    _stmpl_records.SetAttribute("ATTRIBUTE_NAME", dr["Category_Name"].ToString());
                                    _stmpl_records.SetAttribute("URL_CABLE_LEFT",  HttpUtility.UrlEncode(Cable1));
                                    _stmpl_records.SetAttribute("URL_CABLE_RIGHT", HttpUtility.UrlEncode(Cable2));

                                    if (dr["select"].ToString() == "1")
                                    {
                                        selectedname = dr["Category_Name"].ToString();
                                        _stmpl_records.SetAttribute("OPT_SELECTED", "SELECTED");
                                      
                                        
                                    }
                                }
                                else
                                {
                                    //_stmpl_records.SetAttribute("TBW_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(ParentCatID.ToString()));
                                    //_stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(dr["CATEGORY_ID"].ToString()));
                                    // _stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(_catCid.ToString()));

                                    _stmpl_records.SetAttribute("URL_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(dr[0].ToString()));
                                    //if (dr[0].ToString().Length>15)
                                    //    _stmpl_records.SetAttribute("ATTRIBUTE_NAME", dr[0].ToString().Substring(0,14)  );
                                    //else
                                        _stmpl_records.SetAttribute("ATTRIBUTE_NAME", dr[0].ToString());

                                    _stmpl_records.SetAttribute("URL_CABLE_LEFT",  HttpUtility.UrlEncode(Cable1));
                                    _stmpl_records.SetAttribute("URL_CABLE_RIGHT", HttpUtility.UrlEncode(Cable2));

                                    if (dr["select"].ToString() == "1")
                                    {
                                        selectedname = dr[0].ToString();
                                        _stmpl_records.SetAttribute("OPT_SELECTED", "SELECTED");
                                       
                                    }

                                }


                                //_stmpl_records.SetAttribute("TBW_CUSTOM_NUM_FIELD3", _byp);
                                //_stmpl_records.SetAttribute("TBW_BRAND", HttpUtility.UrlEncode(_tsb));
                                //_stmpl_records.SetAttribute("TBW_MODEL", HttpUtility.UrlEncode(_tsm));
                                //_stmpl_records.SetAttribute("TBW_FAMILY_ID", _fid);

                                if (AttName.ToLower().Contains("category"))
                                    _stmpl_records.SetAttribute("URL_ATTRIBUTE_TYPE", HttpUtility.UrlEncode("Category"));
                                else
                                    _stmpl_records.SetAttribute("URL_ATTRIBUTE_TYPE", HttpUtility.UrlEncode(AttName));

                                //if (HttpContext.Current.Request.QueryString["ea"] != null)
                                //{
                                //    _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(HttpContext.Current.Request.QueryString["ea"].ToString())));
                                //}

                                if (dr["eapath"].ToString() == "")
                                {
                                    if (HttpContext.Current.Session["EA"] != null)
                                        dr["eapath"] = HttpContext.Current.Session["EA"].ToString();
                                }

                                //if (HttpContext.Current.Session["EA"] != null)
                                if (dr["eapath"] != "")
                                {
                                    _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(dr["eapath"].ToString())));
                                }

                                lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());

                                ictrecords++;
                            }

                            j++;

                            _stmpl_recordsrows = _stg_container.GetInstanceOf("ProductFinderAtt" + "\\" + "row");



                            if (AttName.Length > 13)
                            {
                                _stmpl_recordsrows.SetAttribute("SELECTED_ATT", "&#60;" + AttName.Substring(0, 13) + ".." + "&#62;");
                            }
                            else
                            {
                                _stmpl_recordsrows.SetAttribute("SELECTED_ATT", "&#60;" + AttName + "&#62;");
                            }

                            if (selectedname == "")
                            {
                                if (AttName.Length > 13)
                                {
                                    _stmpl_recordsrows.SetAttribute("SELECTED_NAME", "&#60;" + AttName.Substring(0, 13) + ".." + "&#62;");
                                }
                                else
                                {
                                    _stmpl_recordsrows.SetAttribute("SELECTED_NAME", "&#60;" + AttName + "&#62;");
                                }
                            }
                            else
                            {
                                _stmpl_recordsrows.SetAttribute("SELECT_COLOR", "color: #2cbbfe;");
                                if (selectedname.Length > 13)
                                {
                                    _stmpl_recordsrows.SetAttribute("SELECTED_NAME", selectedname.Substring(0, 13) + "..");
                                }
                                else
                                {
                                    _stmpl_recordsrows.SetAttribute("SELECTED_NAME", selectedname);
                                }
                            }

                            //_stmpl_recordsrows.SetAttribute("TBT_ATTRIBUTE_ID", dscat.Tables[i].TableName.ToString().Replace("/", "").Replace(" ", ""));
                            //_stmpl_recordsrows.SetAttribute("TBT_MENU_ID", (i + 1).ToString());
                            _stmpl_recordsrows.SetAttribute("TBWDataList", lstrecords);
                            // _stmpl_recordsrows.SetAttribute("TBWDataList1", lstrecords1);
                            sHtml = sHtml + _stmpl_recordsrows.ToString();
                        }
                    }
                }
                if (sHtml!="")
                {
                  _stmpl_container = _stg_container.GetInstanceOf("ProductFinderAtt" + "\\" + "main");
                
                


                _stmpl_container.SetAttribute("MENU_LIST", sHtml);
                sHtml = _stmpl_container.ToString();
                }
                HttpContext.Current.Session["TOPAttributes"] = dscat;
            }

            return sHtml;
        }


        public string ST_Get_BrandModel_Attr_Filter()
        {
            DataSet dscat = new DataSet();

            StringTemplateGroup _stg_container = null;
            StringTemplateGroup _stg_records = null;
            StringTemplate _stmpl_container = null;
            StringTemplate _stmpl_records = null;
            StringTemplate _stmpl_records1 = null;
            StringTemplate _stmpl_recordsrows = null;
            TBWDataList[] lstrecords = new TBWDataList[0];
            TBWDataList[] lstrows = new TBWDataList[0];

            StringTemplateGroup _stg_container1 = null;
            StringTemplateGroup _stg_records1 = null;
            TBWDataList1[] lstrecords1 = new TBWDataList1[0];
            TBWDataList1[] lstrows1 = new TBWDataList1[0];
            int ictrows = 0;
            string _tsb = "";
            string _tsm = "";
            string _type = "";
            string _value = "";
            string _bname = "";
            string _searchstr = "";
            string _byp = "2";
            string _bypcat = null;


            string _pid = "";
            string _fid = "";
            string _seeall = "";
            string sHtml = "";
            //dscat = (DataSet)HttpContext.Current.Session["LHSAttributes"];
            dscat = (DataSet)HttpContext.Current.Session["TOPAttributes"];

            string brandleft = "";
            string modelright = "";
            string AttName = "";
            string selectedname = "";
            if (HttpContext.Current.Request.QueryString["l"] != null)
            {
                brandleft = HttpContext.Current.Request.QueryString["l"].ToString();
            }
            if (HttpContext.Current.Request.QueryString["r"] != null)
            {
                modelright = HttpContext.Current.Request.QueryString["r"].ToString();
            }

            _stg_records = new StringTemplateGroup("cell", _SkinRootPath);
            _stg_container = new StringTemplateGroup("main", _SkinRootPath);
            if (dscat != null)
            {

                if (dscat.Tables.Count > 0)
                    lstrows = new TBWDataList[dscat.Tables.Count + 1];

                for (int i = 0; i < dscat.Tables.Count; i++)
                {
                    Boolean tmpallow = true;


                    //if (dscat.Tables[i].TableName.Contains("Category"))
                    //    tmpallow = true;
                    //else if (dscat.Tables[i].TableName.Contains("Brand"))
                    //    tmpallow = false;
                    //else if (Request.QueryString["byp"] == "2")
                    //    tmpallow = true;
                    //else
                    //    tmpallow = false;
                    selectedname = "";
                    if (tmpallow == true)
                    {
                        if (dscat.Tables[i].Rows.Count > 0)
                        {
                            lstrecords = new TBWDataList[dscat.Tables[i].Rows.Count + 1];
                            lstrecords1 = new TBWDataList1[dscat.Tables[i].Rows.Count + 1];
                            int ictrecords = 0;
                            selectedname = "";
                            int j = 0;
                            AttName = dscat.Tables[i].TableName.ToString().Replace("_Table", "");
                            foreach (DataRow dr in dscat.Tables[i].Rows)//For Records
                            {



                                _stmpl_records = _stg_records.GetInstanceOf("ProductFinderAtt" + "\\" + "cell_mob");

                                if (dscat.Tables[i].TableName.Contains("Category"))
                                {
                                    //_stmpl_records.SetAttribute("TBW_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(_parentCatID.ToString()));
                                    //_stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(dr["CATEGORY_ID"].ToString()));
                                    //_stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(ParentCatID.ToString()));


                                    _stmpl_records.SetAttribute("URL_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(dr["Category_Name"].ToString()));
                                    _stmpl_records.SetAttribute("ATTRIBUTE_NAME", dr["Category_Name"].ToString());
                                    _stmpl_records.SetAttribute("URL_BRAND_LEFT", HttpUtility.UrlEncode(brandleft ));
                                    _stmpl_records.SetAttribute("URL_MODEL_RIGHT", HttpUtility.UrlEncode(modelright ));

                                    if (dr["select"].ToString() == "1")
                                    {
                                        selectedname = dr["Category_Name"].ToString();
                                        _stmpl_records.SetAttribute("OPT_SELECTED", "SELECTED");
                                       
                                    }
                                }
                                else
                                {
                                    //_stmpl_records.SetAttribute("TBW_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(ParentCatID.ToString()));
                                    //_stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(dr["CATEGORY_ID"].ToString()));
                                    // _stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(_catCid.ToString()));

                                    _stmpl_records.SetAttribute("URL_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(dr[0].ToString()));
                                    //if (dr[0].ToString().Length>15)
                                    //    _stmpl_records.SetAttribute("ATTRIBUTE_NAME", dr[0].ToString().Substring(0,14)  );
                                    //else
                                    _stmpl_records.SetAttribute("ATTRIBUTE_NAME", dr[0].ToString());

                                    _stmpl_records.SetAttribute("URL_BRAND_LEFT", HttpUtility.UrlEncode(brandleft ));
                                    _stmpl_records.SetAttribute("URL_MODEL_RIGHT", HttpUtility.UrlEncode(modelright));

                                    if (dr["select"].ToString() == "1")
                                    {
                                        selectedname = dr[0].ToString();
                                        _stmpl_records.SetAttribute("OPT_SELECTED", "SELECTED");
                                       
                                    }
                                }


                                //_stmpl_records.SetAttribute("TBW_CUSTOM_NUM_FIELD3", _byp);
                                //_stmpl_records.SetAttribute("TBW_BRAND", HttpUtility.UrlEncode(_tsb));
                                //_stmpl_records.SetAttribute("TBW_MODEL", HttpUtility.UrlEncode(_tsm));
                                //_stmpl_records.SetAttribute("TBW_FAMILY_ID", _fid);

                                if (AttName.ToLower().Contains("category"))
                                    _stmpl_records.SetAttribute("URL_ATTRIBUTE_TYPE", HttpUtility.UrlEncode("Category"));
                                else
                                    _stmpl_records.SetAttribute("URL_ATTRIBUTE_TYPE", HttpUtility.UrlEncode(AttName));

                                
                                //if (HttpContext.Current.Request.QueryString["ea"] != null)
                                //{
                                //    _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(HttpContext.Current.Request.QueryString["ea"].ToString())));
                                //}

                                if (dr["eapath"].ToString() == "")
                                {
                                    if (HttpContext.Current.Session["EA"] != null)
                                        dr["eapath"] = HttpContext.Current.Session["EA"].ToString();
                                }

                                //if (HttpContext.Current.Session["EA"] != null)
                                if (dr["eapath"] != "")
                                {
                                    _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(dr["eapath"].ToString())));
                                }
                                lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());

                                ictrecords++;
                            }

                            j++;

                            _stmpl_recordsrows = _stg_container.GetInstanceOf("ProductFinderAtt" + "\\" + "row_mob");


                            if (AttName.Length > 13)
                            {
                                _stmpl_recordsrows.SetAttribute("SELECTED_ATT", "&#60;" + AttName.Substring(0, 13) + ".." + "&#62;");
                            }
                            else
                            {
                                _stmpl_recordsrows.SetAttribute("SELECTED_ATT", "&#60;" + AttName + "&#62;");
                            }

                            if (selectedname == "")
                            {
                                if (AttName.Length > 13)
                                {
                                    _stmpl_recordsrows.SetAttribute("SELECTED_NAME", "&#60;" + AttName.Substring(0, 13) + ".." + "&#62;");
                                }
                                else
                                {
                                    _stmpl_recordsrows.SetAttribute("SELECTED_NAME", "&#60;" + AttName + "&#62;");
                                }
                            }
                            else
                            {
                                _stmpl_recordsrows.SetAttribute("SELECT_COLOR", "color: #2cbbfe;");
                                if (selectedname.Length > 13)
                                {
                                    _stmpl_recordsrows.SetAttribute("SELECTED_NAME", selectedname.Substring(0, 13) + "..");
                                }
                                else
                                {
                                    _stmpl_recordsrows.SetAttribute("SELECTED_NAME", selectedname);
                                }
                            }

                            //_stmpl_recordsrows.SetAttribute("TBT_ATTRIBUTE_ID", dscat.Tables[i].TableName.ToString().Replace("/", "").Replace(" ", ""));
                            //_stmpl_recordsrows.SetAttribute("TBT_MENU_ID", (i + 1).ToString());
                            _stmpl_recordsrows.SetAttribute("TBWDataList", lstrecords);
                            // _stmpl_recordsrows.SetAttribute("TBWDataList1", lstrecords1);
                            sHtml = sHtml + _stmpl_recordsrows.ToString();
                        }
                    }
                }
                if (sHtml != "")
                {
                    _stmpl_container = _stg_container.GetInstanceOf("ProductFinderAtt" + "\\" + "main_mob");




                    _stmpl_container.SetAttribute("MENU_LIST", sHtml);
                    sHtml = _stmpl_container.ToString();
                }
                HttpContext.Current.Session["TOPAttributes"] = dscat;
            }

            return sHtml;
        }


        public string ST_Get_VechicleBrandModel_Attr_Filter()
        {
            DataSet dscat = new DataSet();

            StringTemplateGroup _stg_container = null;
            StringTemplateGroup _stg_records = null;
            StringTemplate _stmpl_container = null;
            StringTemplate _stmpl_records = null;
            StringTemplate _stmpl_records1 = null;
            StringTemplate _stmpl_recordsrows = null;
            TBWDataList[] lstrecords = new TBWDataList[0];
            TBWDataList[] lstrows = new TBWDataList[0];

            StringTemplateGroup _stg_container1 = null;
            StringTemplateGroup _stg_records1 = null;
            TBWDataList1[] lstrecords1 = new TBWDataList1[0];
            TBWDataList1[] lstrows1 = new TBWDataList1[0];
            int ictrows = 0;
            string _tsb = "";
            string _tsm = "";
            string _type = "";
            string _value = "";
            string _bname = "";
            string _searchstr = "";
            string _byp = "2";
            string _bypcat = null;


            string _pid = "";
            string _fid = "";
            string _seeall = "";
            string sHtml = "";
            string selectedname = "";
            //dscat = (DataSet)HttpContext.Current.Session["LHSAttributes"];
            dscat = (DataSet)HttpContext.Current.Session["TOPAttributes"];


            string brandleft = "";
            string modelright = "";
            string AttName = "";

          

            if (HttpContext.Current.Request.QueryString["l"] != null)
            {
                brandleft = HttpContext.Current.Request.QueryString["l"].ToString();
            }
            if (HttpContext.Current.Request.QueryString["r"] != null)
            {
                modelright = HttpContext.Current.Request.QueryString["r"].ToString();
            }

            _stg_records = new StringTemplateGroup("cell", _SkinRootPath);
            _stg_container = new StringTemplateGroup("main", _SkinRootPath);
            if (dscat != null)
            {

                if (dscat.Tables.Count > 0)
                    lstrows = new TBWDataList[dscat.Tables.Count + 1];

                for (int i = 0; i < dscat.Tables.Count; i++)
                {
                    Boolean tmpallow = true;


                    //if (dscat.Tables[i].TableName.Contains("Category"))
                    //    tmpallow = true;
                    //else if (dscat.Tables[i].TableName.Contains("Brand"))
                    //    tmpallow = false;
                    //else if (Request.QueryString["byp"] == "2")
                    //    tmpallow = true;
                    //else
                    //    tmpallow = false;
                    selectedname = "";
                    if (tmpallow == true)
                    {
                        if (dscat.Tables[i].Rows.Count > 0)
                        {
                            lstrecords = new TBWDataList[dscat.Tables[i].Rows.Count + 1];
                            lstrecords1 = new TBWDataList1[dscat.Tables[i].Rows.Count + 1];
                            int ictrecords = 0;
                            selectedname = "";
                            int j = 0;
                             AttName = dscat.Tables[i].TableName.ToString().Replace("_Table", "");
                            foreach (DataRow dr in dscat.Tables[i].Rows)//For Records
                            {



                                _stmpl_records = _stg_records.GetInstanceOf("ProductFinderAtt" + "\\" + "cell_veh");

                                if (dscat.Tables[i].TableName.Contains("Category"))
                                {
                                    //_stmpl_records.SetAttribute("TBW_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(_parentCatID.ToString()));
                                    //_stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(dr["CATEGORY_ID"].ToString()));
                                    //_stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(ParentCatID.ToString()));


                                    _stmpl_records.SetAttribute("URL_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(dr["Category_Name"].ToString()));
                                    _stmpl_records.SetAttribute("ATTRIBUTE_NAME", dr["Category_Name"].ToString());
                                    _stmpl_records.SetAttribute("URL_BRAND_LEFT", HttpUtility.UrlEncode(brandleft));
                                    _stmpl_records.SetAttribute("URL_MODEL_RIGHT", HttpUtility.UrlEncode(modelright));

                                    if (dr["select"].ToString() == "1")
                                    {
                                        selectedname = dr["Category_Name"].ToString();
                                        _stmpl_records.SetAttribute("OPT_SELECTED", "SELECTED");
                                       
                                    }
                                }
                                else
                                {
                                    //_stmpl_records.SetAttribute("TBW_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(ParentCatID.ToString()));
                                    //_stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(dr["CATEGORY_ID"].ToString()));
                                    // _stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(_catCid.ToString()));

                                    _stmpl_records.SetAttribute("URL_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(dr[0].ToString()));
                                    //if (dr[0].ToString().Length>15)
                                    //    _stmpl_records.SetAttribute("ATTRIBUTE_NAME", dr[0].ToString().Substring(0,14)  );
                                    //else
                                    _stmpl_records.SetAttribute("ATTRIBUTE_NAME", dr[0].ToString());

                                    _stmpl_records.SetAttribute("URL_BRAND_LEFT", HttpUtility.UrlEncode(brandleft));
                                    _stmpl_records.SetAttribute("URL_MODEL_RIGHT", HttpUtility.UrlEncode(modelright));

                                    if (dr["select"].ToString() == "1")
                                    {
                                        selectedname = dr[0].ToString();
                                        _stmpl_records.SetAttribute("OPT_SELECTED", "SELECTED");
                                       
                                    }

                                }


                                //_stmpl_records.SetAttribute("TBW_CUSTOM_NUM_FIELD3", _byp);
                                //_stmpl_records.SetAttribute("TBW_BRAND", HttpUtility.UrlEncode(_tsb));
                                //_stmpl_records.SetAttribute("TBW_MODEL", HttpUtility.UrlEncode(_tsm));
                                //_stmpl_records.SetAttribute("TBW_FAMILY_ID", _fid);

                                if (AttName.ToLower().Contains("category"))
                                    _stmpl_records.SetAttribute("URL_ATTRIBUTE_TYPE", HttpUtility.UrlEncode("Category"));
                                else
                                    _stmpl_records.SetAttribute("URL_ATTRIBUTE_TYPE", HttpUtility.UrlEncode(AttName));
                                //if (HttpContext.Current.Request.QueryString["ea"] != null)
                                //{
                                //    _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(HttpContext.Current.Request.QueryString["ea"].ToString())));
                                //}
                                if (dr["eapath"].ToString() == "")
                                {
                                    if (HttpContext.Current.Session["EA"] != null)
                                        dr["eapath"] = HttpContext.Current.Session["EA"].ToString();
                                }

                                //if (HttpContext.Current.Session["EA"] != null)
                                if (dr["eapath"] != "")
                                {
                                    _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(dr["eapath"].ToString())));
                                }

                                lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());

                                ictrecords++;
                            }

                            j++;

                            _stmpl_recordsrows = _stg_container.GetInstanceOf("ProductFinderAtt" + "\\" + "row_veh");

                          

                            if (AttName.Length > 13)
                            {
                                _stmpl_recordsrows.SetAttribute("SELECTED_ATT", "&#60;" + AttName.Substring(0, 13) + ".." + "&#62;");
                            }
                            else
                            {
                                _stmpl_recordsrows.SetAttribute("SELECTED_ATT", "&#60;" + AttName + "&#62;");
                            }

                            if (selectedname == "")
                            {
                                if (AttName.Length > 13)
                                {
                                    _stmpl_recordsrows.SetAttribute("SELECTED_NAME", "&#60;" + AttName.Substring(0, 13) + ".." + "&#62;");
                                }
                                else
                                {
                                    _stmpl_recordsrows.SetAttribute("SELECTED_NAME", "&#60;" + AttName + "&#62;");
                                }
                            }
                            else
                            {
                                _stmpl_recordsrows.SetAttribute("SELECT_COLOR", "color: #2cbbfe;");
                                if (selectedname.Length > 13)
                                {
                                    _stmpl_recordsrows.SetAttribute("SELECTED_NAME",   selectedname.Substring(0, 13) + ".." );
                                }
                                else
                                {
                                    _stmpl_recordsrows.SetAttribute("SELECTED_NAME",  selectedname );
                                }
                            }



                            //_stmpl_recordsrows.SetAttribute("TBT_ATTRIBUTE_ID", dscat.Tables[i].TableName.ToString().Replace("/", "").Replace(" ", ""));
                            //_stmpl_recordsrows.SetAttribute("TBT_MENU_ID", (i + 1).ToString());
                            _stmpl_recordsrows.SetAttribute("TBWDataList", lstrecords);
                            // _stmpl_recordsrows.SetAttribute("TBWDataList1", lstrecords1);
                            sHtml = sHtml + _stmpl_recordsrows.ToString();
                        }
                    }
                }
                if (sHtml != "")
                {
                    _stmpl_container = _stg_container.GetInstanceOf("ProductFinderAtt" + "\\" + "main_veh");




                    _stmpl_container.SetAttribute("MENU_LIST", sHtml);
                    sHtml = _stmpl_container.ToString();
                }
                HttpContext.Current.Session["TOPAttributes"] = dscat;
            }

            return sHtml;
        }
        public string ST_BrandL_Load(DataTable datasource, string selectedBrand, string selectedBrand_image)
        {
            string sHTML = "";


            try
            {
                DataSet dsbotmsname = new DataSet();
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                //  StringTemplate _stmpl_records1 = null;
                // StringTemplate _stmpl_recordsrows = null;
                TBWDataList[] lstrecords = new TBWDataList[0];

                // DataSet dsrecords = new DataSet();
                // DataTable dt = null;
                DataRow[] drs = null;

                //dsrecords = EasyAsk.GetCategoryAndBrand("MainCategory");

                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                int ictrows = 0;






                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);

                string Lis = "";

                // lstrecords = new TBWDataList[datasource.Rows.Count + 1];



                int ictrecords = 0;
                if (datasource != null && datasource.Rows.Count > 0)
                {


                    foreach (DataRow dr in datasource.Rows)//For Records
                    {
                        if (dr["Category_id"].ToString().ToUpper() != "WES-08H")
                        {
                            _stmpl_records = _stg_records.GetInstanceOf("ProductFinder" + "\\" + "BrandLeftLi");


                            _stmpl_records.SetAttribute("URL_BRAND_NAME", HttpUtility.UrlEncode(dr["Brand"].ToString()));
                            _stmpl_records.SetAttribute("BRAND_NAME", dr["Brand"].ToString());
                            Lis = Lis + _stmpl_records.ToString();
                        }
                    }
                    if (selectedBrand != "")
                    {
                        _stmpl_container = _stg_container.GetInstanceOf("ProductFinder" + "\\" + "BrandLeftselect");

                        _stmpl_container.SetAttribute("SELECTED_BRAND", selectedBrand );
                        _stmpl_container.SetAttribute("BRAND_IMAGE", selectedBrand_image);
                        _stmpl_container.SetAttribute("BRAND_LI_LIST", Lis);
                    }
                    else
                    {
                        _stmpl_container = _stg_container.GetInstanceOf("ProductFinder" + "\\" + "BrandLeft");

                        _stmpl_container.SetAttribute("SELECTED_BRAND", "Select Brand");
                        _stmpl_container.SetAttribute("BRAND_LI_LIST", Lis);
                    }
                }
                else
                {
                    _stmpl_container = _stg_container.GetInstanceOf("ProductFinder" + "\\" + "BrandLeft");

                    _stmpl_container.SetAttribute("SELECTED_BRAND", "Select Brand");
                    _stmpl_container.SetAttribute("BRAND_LI_LIST", "");
                }
                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return sHTML;
        }


        public string ST_BrandLImage_Load(DataTable datasource, string strsearchvalue)
        {
            string sHTML = "";


            try
            {
                DataSet dsbotmsname = new DataSet();
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                //  StringTemplate _stmpl_records1 = null;
                // StringTemplate _stmpl_recordsrows = null;
                TBWDataList[] lstrecords = new TBWDataList[0];

                // DataSet dsrecords = new DataSet();
                // DataTable dt = null;
                DataRow[] drs = null;

                //dsrecords = EasyAsk.GetCategoryAndBrand("MainCategory");

                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                int ictrows = 0;






                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);

                string Lis = "";

                // lstrecords = new TBWDataList[datasource.Rows.Count + 1];



                int ictrecords = 0;
                if (datasource != null && datasource.Rows.Count > 0)
                {


                    foreach (DataRow dr in datasource.Rows)//For Records
                    {


                        if (dr["Category_id"].ToString().ToUpper() != "WES-08H")
                        {

                            if (strsearchvalue != "")
                            {
                                if (dr["Brand"].ToString().ToLower().Contains(strsearchvalue.ToLower()))
                                {
                                    _stmpl_records = _stg_records.GetInstanceOf("ProductFinder" + "\\" + "BrandLeftImage");

                                    // _stmpl_records.SetAttribute("BRAND_IMAGE", dr["Image_Path"].ToString());
                                    _stmpl_records.SetAttribute("BRAND_NAME", dr["Brand"].ToString());
                                    _stmpl_records.SetAttribute("URL_BRAND_NAME", HttpUtility.UrlEncode(dr["Brand"].ToString()));
                                    Lis = Lis + _stmpl_records.ToString();
                                }
                            }
                            else
                            {
                                _stmpl_records = _stg_records.GetInstanceOf("ProductFinder" + "\\" + "BrandLeftImage");

                                // _stmpl_records.SetAttribute("BRAND_IMAGE", dr["Image_Path"].ToString());
                                _stmpl_records.SetAttribute("BRAND_NAME", dr["Brand"].ToString());
                                _stmpl_records.SetAttribute("URL_BRAND_NAME", HttpUtility.UrlEncode(dr["Brand"].ToString()));
                                Lis = Lis + _stmpl_records.ToString();
                            }
                        }
                    }
                    sHTML = Lis;
                }
                else
                {
                    sHTML = "";
                }

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return sHTML;
        }



        public string ST_ModelR_Load(DataTable datasource, string selectedModel, string selectedModel_image, string Brand_left, string Ea)
        {
            string sHTML = "";


            try
            {
                DataSet dsbotmsname = new DataSet();
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                //  StringTemplate _stmpl_records1 = null;
                // StringTemplate _stmpl_recordsrows = null;
                TBWDataList[] lstrecords = new TBWDataList[0];

                // DataSet dsrecords = new DataSet();
                // DataTable dt = null;
                DataRow[] drs = null;

                //dsrecords = EasyAsk.GetCategoryAndBrand("MainCategory");

                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                int ictrows = 0;






                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);

                string Lis = "";

                // lstrecords = new TBWDataList[datasource.Rows.Count + 1];



                int ictrecords = 0;
                if (datasource != null && datasource.Rows.Count > 0)
                {
                    foreach (DataRow dr in datasource.Rows)//For Records
                    {
                        if (dr["Category_id"].ToString().ToUpper() != "WES-08H")
                        {
                            _stmpl_records = _stg_records.GetInstanceOf("ProductFinder" + "\\" + "ModelRightLi");


                            _stmpl_records.SetAttribute("BRAND_LEFT", HttpUtility.UrlEncode(Brand_left));
                            _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea)));
                            //_stmpl_records.SetAttribute("CABLE_IMAGE", dr["Image_Path"].ToString());
                            _stmpl_records.SetAttribute("URL_MODEL_VALUE", HttpUtility.UrlEncode(dr["Model"].ToString()));
                            _stmpl_records.SetAttribute("URL_MODEL_TYPE", "Model");
                            _stmpl_records.SetAttribute("URL_MODEL_NAME", HttpUtility.UrlEncode(dr["Model"].ToString()));
                            _stmpl_records.SetAttribute("MODEL_NAME", dr["Model"].ToString());

                            Lis = Lis + _stmpl_records.ToString();
                        }
                    }

                    if (selectedModel  != "")
                    {
                        _stmpl_container = _stg_container.GetInstanceOf("ProductFinder" + "\\" + "ModelRightselect");

                        _stmpl_container.SetAttribute("SELECTED_Model", selectedModel );
                        _stmpl_container.SetAttribute("MODEL_IMAGE", selectedModel_image);
                         _stmpl_container.SetAttribute("MODEL_LI_LIST", Lis);
                    }
                    else
                    {
                        _stmpl_container = _stg_container.GetInstanceOf("ProductFinder" + "\\" + "ModelRight");

                        _stmpl_container.SetAttribute("SELECTED_MODEL", "select Model");
                        if (Brand_left  != "")
                            _stmpl_container.SetAttribute("SELECT_CLASS", "selected");

                        _stmpl_container.SetAttribute("MODEL_LI_LIST", Lis);
                    }

                }
                else
                {

                    _stmpl_container = _stg_container.GetInstanceOf("ProductFinder" + "\\" + "ModelRight");

                    _stmpl_container.SetAttribute("SELECTED_MODEL", "select Model");
                    if (Brand_left != "")
                        _stmpl_container.SetAttribute("SELECT_CLASS", "selected");

                     _stmpl_container.SetAttribute("MODEL_LI_LIST", "");

                }


                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return sHTML;
        }

        public string ST_ModelRImage_Load(DataTable datasource, string Brand_left, string Ea, string strsearchvalue)
        {
            string sHTML = "";


            try
            {
                DataSet dsbotmsname = new DataSet();
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                //  StringTemplate _stmpl_records1 = null;
                // StringTemplate _stmpl_recordsrows = null;
                TBWDataList[] lstrecords = new TBWDataList[0];

                // DataSet dsrecords = new DataSet();
                // DataTable dt = null;
                DataRow[] drs = null;

                //dsrecords = EasyAsk.GetCategoryAndBrand("MainCategory");

                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                int ictrows = 0;






                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);

                string Lis = "";

                // lstrecords = new TBWDataList[datasource.Rows.Count + 1];



                int ictrecords = 0;
                if (datasource != null && datasource.Rows.Count > 0)
                {


                    foreach (DataRow dr in datasource.Rows)//For Records
                    {
                        if (dr["Category_id"].ToString().ToUpper() != "WES-08H")
                        {
                            if (strsearchvalue != "")
                            {
                                if (dr["model"].ToString().ToLower().Contains(strsearchvalue.ToLower()))
                                {
                                    _stmpl_records = _stg_records.GetInstanceOf("ProductFinder" + "\\" + "ModelRightImage");
                                    _stmpl_records.SetAttribute("BRAND_LEFT", HttpUtility.UrlEncode(Brand_left));
                                    _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea)));
                                    _stmpl_records.SetAttribute("MODEL_IMAGE", dr["Image_Path"].ToString());
                                    _stmpl_records.SetAttribute("URL_MODEL_VALUE", HttpUtility.UrlEncode(dr["model"].ToString()));
                                    _stmpl_records.SetAttribute("URL_MODEL_TYPE", "model");
                                    _stmpl_records.SetAttribute("URL_MODEL_NAME", HttpUtility.UrlEncode(dr["model"].ToString()));
                                    _stmpl_records.SetAttribute("MODEL_NAME", dr["model"].ToString());
                                    Lis = Lis + _stmpl_records.ToString();
                                }
                            }
                            else
                            {

                                _stmpl_records = _stg_records.GetInstanceOf("ProductFinder" + "\\" + "ModelRightImage");
                                _stmpl_records.SetAttribute("BRAND_LEFT", HttpUtility.UrlEncode(Brand_left));
                                _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea)));
                                _stmpl_records.SetAttribute("MODEL_IMAGE", dr["Image_Path"].ToString());
                                _stmpl_records.SetAttribute("URL_MODEL_VALUE", HttpUtility.UrlEncode(dr["model"].ToString()));
                                _stmpl_records.SetAttribute("URL_MODEL_TYPE", "model");
                                _stmpl_records.SetAttribute("URL_MODEL_NAME", HttpUtility.UrlEncode(dr["model"].ToString()));
                                _stmpl_records.SetAttribute("MODEL_NAME", dr["model"].ToString());
                                Lis = Lis + _stmpl_records.ToString();
                            }
                        }
                    }
                    sHTML = Lis;
                }
                else
                {
                    sHTML = "";
                }

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return sHTML;
        }



        public string ST_VechicleBrandL_Load(DataTable datasource, string selectedBrand, string selectedBrand_image)
        {
            string sHTML = "";


            try
            {
                DataSet dsbotmsname = new DataSet();
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                //  StringTemplate _stmpl_records1 = null;
                // StringTemplate _stmpl_recordsrows = null;
                TBWDataList[] lstrecords = new TBWDataList[0];

                // DataSet dsrecords = new DataSet();
                // DataTable dt = null;
                DataRow[] drs = null;

                //dsrecords = EasyAsk.GetCategoryAndBrand("MainCategory");

                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                int ictrows = 0;






                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);

                string Lis = "";

                // lstrecords = new TBWDataList[datasource.Rows.Count + 1];



                int ictrecords = 0;
                if (datasource != null && datasource.Rows.Count > 0)
                {


                    foreach (DataRow dr in datasource.Rows)//For Records
                    {
                        if (dr["Category_id"].ToString().ToUpper() == "WES-08H")
                        {
                            _stmpl_records = _stg_records.GetInstanceOf("ProductFinder" + "\\" + "VBrandLeftLi");


                            _stmpl_records.SetAttribute("URL_BRAND_NAME", HttpUtility.UrlEncode(dr["Brand"].ToString()));
                            _stmpl_records.SetAttribute("BRAND_NAME", dr["Brand"].ToString());
                            Lis = Lis + _stmpl_records.ToString();
                        }
                    }
                    if (selectedBrand != "")
                    {
                        _stmpl_container = _stg_container.GetInstanceOf("ProductFinder" + "\\" + "VBrandLeftselect");

                        _stmpl_container.SetAttribute("SELECTED_BRAND", selectedBrand);
                        _stmpl_container.SetAttribute("BRAND_IMAGE", selectedBrand_image);
                        _stmpl_container.SetAttribute("BRAND_LI_LIST", Lis);
                    }
                    else
                    {
                        _stmpl_container = _stg_container.GetInstanceOf("ProductFinder" + "\\" + "VBrandLeft");

                        _stmpl_container.SetAttribute("SELECTED_BRAND", "Select Make");
                        _stmpl_container.SetAttribute("BRAND_LI_LIST", Lis);
                    }
                }
                else
                {
                    _stmpl_container = _stg_container.GetInstanceOf("ProductFinder" + "\\" + "VBrandLeft");

                    _stmpl_container.SetAttribute("SELECTED_BRAND", "Select Make");
                    _stmpl_container.SetAttribute("BRAND_LI_LIST", "");
                }
                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return sHTML;
        }


        public string ST_VechicleBrandLImage_Load(DataTable datasource, string strsearchvalue)
        {
            string sHTML = "";


            try
            {
                DataSet dsbotmsname = new DataSet();
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                //  StringTemplate _stmpl_records1 = null;
                // StringTemplate _stmpl_recordsrows = null;
                TBWDataList[] lstrecords = new TBWDataList[0];

                // DataSet dsrecords = new DataSet();
                // DataTable dt = null;
                DataRow[] drs = null;

                //dsrecords = EasyAsk.GetCategoryAndBrand("MainCategory");

                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                int ictrows = 0;






                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);

                string Lis = "";

                // lstrecords = new TBWDataList[datasource.Rows.Count + 1];



                int ictrecords = 0;
                if (datasource != null && datasource.Rows.Count > 0)
                {


                    foreach (DataRow dr in datasource.Rows)//For Records
                    {


                        if (dr["Category_id"].ToString().ToUpper() == "WES-08H")
                        {

                            if (strsearchvalue != "")
                            {
                                if (dr["Brand"].ToString().ToLower().Contains(strsearchvalue.ToLower()))
                                {
                                    _stmpl_records = _stg_records.GetInstanceOf("ProductFinder" + "\\" + "VBrandLeftImage");

                                    // _stmpl_records.SetAttribute("BRAND_IMAGE", dr["Image_Path"].ToString());
                                    _stmpl_records.SetAttribute("BRAND_NAME", dr["Brand"].ToString());
                                    _stmpl_records.SetAttribute("URL_BRAND_NAME", HttpUtility.UrlEncode(dr["Brand"].ToString()));
                                    Lis = Lis + _stmpl_records.ToString();
                                }
                            }
                            else
                            {
                                _stmpl_records = _stg_records.GetInstanceOf("ProductFinder" + "\\" + "VBrandLeftImage");

                                // _stmpl_records.SetAttribute("BRAND_IMAGE", dr["Image_Path"].ToString());
                                _stmpl_records.SetAttribute("BRAND_NAME", dr["Brand"].ToString());
                                _stmpl_records.SetAttribute("URL_BRAND_NAME", HttpUtility.UrlEncode(dr["Brand"].ToString()));
                                Lis = Lis + _stmpl_records.ToString();
                            }
                        }
                    }
                    sHTML = Lis;
                }
                else
                {
                    sHTML = "";
                }

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return sHTML;
        }



        public string ST_VechicleModelR_Load(DataTable datasource, string selectedModel, string selectedModel_image, string Brand_left, string Ea)
        {
            string sHTML = "";


            try
            {
                DataSet dsbotmsname = new DataSet();
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                //  StringTemplate _stmpl_records1 = null;
                // StringTemplate _stmpl_recordsrows = null;
                TBWDataList[] lstrecords = new TBWDataList[0];

                // DataSet dsrecords = new DataSet();
                // DataTable dt = null;
                DataRow[] drs = null;

                //dsrecords = EasyAsk.GetCategoryAndBrand("MainCategory");

                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                int ictrows = 0;






                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);

                string Lis = "";

                // lstrecords = new TBWDataList[datasource.Rows.Count + 1];



                int ictrecords = 0;
                if (datasource != null && datasource.Rows.Count > 0)
                {
                    foreach (DataRow dr in datasource.Rows)//For Records
                    {
                        if (dr["Category_id"].ToString().ToUpper() == "WES-08H")
                        {
                            _stmpl_records = _stg_records.GetInstanceOf("ProductFinder" + "\\" + "VModelRightLi");


                            _stmpl_records.SetAttribute("BRAND_LEFT", HttpUtility.UrlEncode(Brand_left));
                            _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea)));
                            //_stmpl_records.SetAttribute("CABLE_IMAGE", dr["Image_Path"].ToString());
                            _stmpl_records.SetAttribute("URL_MODEL_VALUE", HttpUtility.UrlEncode(dr["Model"].ToString()));
                            _stmpl_records.SetAttribute("URL_MODEL_TYPE", "Model");
                            _stmpl_records.SetAttribute("URL_MODEL_NAME", HttpUtility.UrlEncode(dr["Model"].ToString()));
                            _stmpl_records.SetAttribute("MODEL_NAME", dr["Model"].ToString());

                            Lis = Lis + _stmpl_records.ToString();
                        }
                    }

                    if (selectedModel != "")
                    {
                        _stmpl_container = _stg_container.GetInstanceOf("ProductFinder" + "\\" + "VModelRightselect");

                        _stmpl_container.SetAttribute("SELECTED_Model", selectedModel);
                        _stmpl_container.SetAttribute("MODEL_IMAGE", selectedModel_image);
                        _stmpl_container.SetAttribute("MODEL_LI_LIST", Lis);
                    }
                    else
                    {
                        _stmpl_container = _stg_container.GetInstanceOf("ProductFinder" + "\\" + "VModelRight");

                        _stmpl_container.SetAttribute("SELECTED_MODEL", "select Model");
                        if (Brand_left != "")
                            _stmpl_container.SetAttribute("SELECT_CLASS", "selected");

                        _stmpl_container.SetAttribute("MODEL_LI_LIST", Lis);
                    }

                }
                else
                {

                    _stmpl_container = _stg_container.GetInstanceOf("ProductFinder" + "\\" + "VModelRight");

                    _stmpl_container.SetAttribute("SELECTED_MODEL", "select Model");
                    if (Brand_left != "")
                        _stmpl_container.SetAttribute("SELECT_CLASS", "selected");

                    _stmpl_container.SetAttribute("MODEL_LI_LIST", "");

                }


                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return sHTML;
        }

        public string ST_VechicleModelRImage_Load(DataTable datasource, string Brand_left, string Ea, string strsearchvalue)
        {
            string sHTML = "";


            try
            {
                DataSet dsbotmsname = new DataSet();
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                //  StringTemplate _stmpl_records1 = null;
                // StringTemplate _stmpl_recordsrows = null;
                TBWDataList[] lstrecords = new TBWDataList[0];

                // DataSet dsrecords = new DataSet();
                // DataTable dt = null;
                DataRow[] drs = null;

                //dsrecords = EasyAsk.GetCategoryAndBrand("MainCategory");

                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                int ictrows = 0;






                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);

                string Lis = "";

                // lstrecords = new TBWDataList[datasource.Rows.Count + 1];



                int ictrecords = 0;
                if (datasource != null && datasource.Rows.Count > 0)
                {


                    foreach (DataRow dr in datasource.Rows)//For Records
                    {
                        if (dr["Category_id"].ToString().ToUpper() == "WES-08H")
                        {
                            if (strsearchvalue != "")
                            {
                                if (dr["model"].ToString().ToLower().Contains(strsearchvalue.ToLower()))
                                {
                                    _stmpl_records = _stg_records.GetInstanceOf("ProductFinder" + "\\" + "VModelRightImage");
                                    _stmpl_records.SetAttribute("BRAND_LEFT", HttpUtility.UrlEncode(Brand_left));
                                    _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea)));
                                    _stmpl_records.SetAttribute("MODEL_IMAGE", dr["Image_Path"].ToString());
                                    _stmpl_records.SetAttribute("URL_MODEL_VALUE", HttpUtility.UrlEncode(dr["model"].ToString()));
                                    _stmpl_records.SetAttribute("URL_MODEL_TYPE", "model");
                                    _stmpl_records.SetAttribute("URL_MODEL_NAME", HttpUtility.UrlEncode(dr["model"].ToString()));
                                    _stmpl_records.SetAttribute("MODEL_NAME", dr["model"].ToString());
                                    Lis = Lis + _stmpl_records.ToString();
                                }
                            }
                            else
                            {

                                _stmpl_records = _stg_records.GetInstanceOf("ProductFinder" + "\\" + "VModelRightImage");
                                _stmpl_records.SetAttribute("BRAND_LEFT", HttpUtility.UrlEncode(Brand_left));
                                _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea)));
                                _stmpl_records.SetAttribute("MODEL_IMAGE", dr["Image_Path"].ToString());
                                _stmpl_records.SetAttribute("URL_MODEL_VALUE", HttpUtility.UrlEncode(dr["model"].ToString()));
                                _stmpl_records.SetAttribute("URL_MODEL_TYPE", "model");
                                _stmpl_records.SetAttribute("URL_MODEL_NAME", HttpUtility.UrlEncode(dr["model"].ToString()));
                                _stmpl_records.SetAttribute("MODEL_NAME", dr["model"].ToString());
                                Lis = Lis + _stmpl_records.ToString();
                            }
                        }
                    }
                    sHTML = Lis;
                }
                else
                {
                    sHTML = "";
                }

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return sHTML;
        }
    }
   
       
        /*********************************** J TECH CODE ***********************************/
}
