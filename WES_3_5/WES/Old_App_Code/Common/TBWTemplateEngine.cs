using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using StringTemplate = Antlr.StringTemplate.StringTemplate;
using StringTemplateGroup = Antlr.StringTemplate.StringTemplateGroup;
using TradingBell.Common;
using TradingBell.WebServices;
using System.Xml;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;
namespace TradingBell.Common
{
    public class TBWTemplateEngine
    {

        EasyAsk_WES EasyAsk = new EasyAsk_WES();
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
        public string paraValue = "";
        public string paraPID = "";
        public string paraFID = "";
        public string paraCID = "";
        private string _cartitem = "";
        private string _CATALOG_ID = "";
        private Helper oHelper = new Helper();
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
       
        public TBWTemplateEngine(string Package, string SkinRootPath, string DBConnectionString)
        {
            _Package = Package;
            _SkinRootPath = SkinRootPath;
            _DBConnectionString = DBConnectionString.Substring(DBConnectionString.IndexOf(';') + 1);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter("select convert(int, option_value) as OPTION_VALUE from tbwc_store_options where option_name = 'DEFAULT CATALOG'", _DBConnectionString);
            da.Fill(ds, "generictable");
            _CATALOG_ID = ds.Tables[0].Rows[0].ItemArray[0].ToString();

        }

        public bool RenderHTML(string rType)
        {
            bool _status = false;
            string _sqlpkginfo;
            _sqlpkginfo = " SELECT * FROM TBW_PACKAGE ";
            _sqlpkginfo = _sqlpkginfo + " WHERE IS_ROOT = 0 AND PACKAGE_NAME = '" + _Package + "' ORDER BY PROCESS_ORDER ASC ";

            DataSet dspkg = new DataSet();
            try
            {
                dspkg = GetDataSet(_sqlpkginfo);
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

                    DataSet DSnaprod = new DataSet();
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

                    }
                }
                _status = true;
            }
            catch (Exception ex)
            {
                _status = false;
            }
            return _status;
        }

        private bool BuildMainContainer()
        {
            bool _status = false;
            string strFile = HttpContext.Current.Server.MapPath("ProdImages");
            string strPDFFiles1 = HttpContext.Current.Server.MapPath("Catalogue Download");
            string strPDFFiles2 = HttpContext.Current.Server.MapPath("News update");
            string _sqlpkginfo;
            _sqlpkginfo = " SELECT TOP 1 * FROM TBW_PACKAGE ";
            _sqlpkginfo = _sqlpkginfo + " WHERE IS_ROOT = 1 AND PACKAGE_NAME = '" + _Package + "' ORDER BY PROCESS_ORDER ASC ";

            DataSet dspkg = new DataSet();
            try
            {
                dspkg = GetDataSet(_sqlpkginfo);
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

                //Build the outer body of the HTML - for main container
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
                    dscontainer=null;
                    DataSet tempDs = (DataSet)HttpContext.Current.Session["FamilyProduct"];
                    if (tempDs != null && tempDs.Tables["SubFamily"]!=null)
                    {
                        DataRow[] dr = tempDs.Tables["SubFamily"].Select("FAMILY_ID='" + paraValue + "'");
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
                        foreach (DataRow dr in dscontainer.Tables[0].Rows)
                        {
                            if (_Package == "CSFAMILYPAGE")
                            {   
                                if(dr["STATUS"].ToString().ToUpper()=="TRUE")
                                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + _skin_container + "1");
                                else if(dr["STATUS"].ToString().ToUpper()=="FALSE")
                                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + _skin_container );
                                else
                                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + _skin_container+"2");

                            }
                            else
                                _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + _skin_container);

                            _stmpl_container.SetAttribute("TBT_ECOMENABLED", IsEcomenabled());
                            _stmpl_container.SetAttribute("TBT_COMPANY_NAME", GetCompanyName() + ",");
                            _stmpl_container.SetAttribute("TBT_LOGIN_NAME", GetLoginName());
                            HttpContext.Current.Session["LOGIN_NAME"] = GetLoginName();

                            foreach (DataColumn dc in dr.Table.Columns)
                            {
                                _stmpl_container.SetAttribute("TBT_" + dc.ColumnName.ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr[dc.ColumnName.ToString()].ToString().Replace("\r", "&nbsp;").Replace("\n", "<br/>"));
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
                            foreach (DataRow dr in dscontainer.Tables[0].Rows)
                            {
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
                                    _stmpl_container.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"].ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;"));
                                }
                            }
                        }

                    }
                }
                if (_Package == "BROWSEBYCATEGORY" || _Package == "BROWSEBYBRAND" || _Package == "BROWSEBYPRODUCT")
                {
                    string bbvalue = "", catName = ""; int recvalue = 0; string cidvalue = "";
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
                    }
                }
                _stmpl_container.SetAttribute("TBW_CART_ITEM", cartitem);
                //Lets get the block inner elements from the dictionary and finish building the main container

                if (_Package == "CATEGORYLISTIMG")
                {
                    DataSet DSDR = new DataSet();
                    //DSDR = GetDataSet("SELECT CATEGORY_ID,CATEGORY_NAME,IMAGE_FILE,SHORT_DESC,IMAGE_FILE2 FROM TB_CATEGORY WHERE CATEGORY_ID = '" + paraValue.ToString() + "'");
                    DataRow[] row = EasyAsk.GetCategoryAndBrand("MainCategory").Tables[0].Select("CATEGORY_ID='" + paraValue.ToString() +"'")   ;
                    if (row.Length > 0)
                    {
                        DSDR.Tables.Add(row.CopyToDataTable());
                    }   
                    
                    
                    if (DSDR!=null)
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

                string _Tbt_Order_Id = "";
                string _Tbt_Ship_URL = "";

                if (HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0)
                {
                    _Tbt_Order_Id = HttpContext.Current.Session["ORDER_ID"].ToString();
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

                string breadcrumb = "";
                if (HttpContext.Current.Request.Url.ToString().Contains("productdetails.aspx") == true && HttpContext.Current.Request.QueryString["tsb"] != null && HttpContext.Current.Request["tsb"].ToString() != "" && HttpContext.Current.Request["tsm"] != null && HttpContext.Current.Request["tsm"].ToString() != "" && HttpContext.Current.Request["sl1"] != null && HttpContext.Current.Request["sl1"].ToString() != "" && HttpContext.Current.Request["sl2"] != null && HttpContext.Current.Request["sl2"].ToString() != "")
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
                }
                _stmpl_container.SetAttribute("TBT_BREAD_CRUMBS", breadcrumb.Replace("<ars>g</ars>", "&rarr;"));

                foreach (KeyValuePair<string, TBWDataList[]> kvp in _dict_inner_html)
                {
                    _stmpl_container.SetAttribute(kvp.Key, kvp.Value);
                }

                _status = true;
                if (_Package == "CSFAMILYPAGE")
                {
                    GetFamilyMultipleImages(Convert.ToInt32(paraFID), _stmpl_container);
                }
            }
            catch (Exception ex)
            {
                _status = false;
            }
            return _status;
        }

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
                    int ictrecords = 0, ictcol = 1; string strValue = "";
                    foreach (DataRow cdr in cellrow)
                    {
                        _stmpl_records = _stg_records.GetInstanceOf(_Package + "\\" + _skin_records);
                        
                        _stmpl_records.SetAttribute("TBT_ECOMENABLED", IsEcomenabled());
                        if(_Package != "PRODUCT") _stmpl_records.SetAttribute("TBT_YOURCOST", GetMyPrice(System.Convert.ToInt32(cdr["PRODUCT_ID"])));

                        if (_Package == "PRODUCT") GetMultipleImages(System.Convert.ToInt32(cdr["PRODUCT_ID"]), System.Convert.ToInt32(paraFID), _stmpl_records);

                        foreach (DataColumn dc in dsrecords.Tables[0].Columns)
                        {
                            //_stmpl_records.SetAttribute(dc.ColumnName, dr[dc.ColumnName].ToString());                                                       
                            _stmpl_records.SetAttribute("TBT_" + dc.ColumnName.ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), cdr[dc.ColumnName.ToString()].ToString());
                            if (_Package.ToString() == "COMPARE" && dc.ColumnName.ToString().ToUpper() == "PRODUCT_ID")
                            {
                                DataSet Dsfamilyname = GetDataSet("SELECT TOP(1) FAMILY_ID FROM TB_PROD_FAMILY WHERE PRODUCT_ID =" + cdr["Product_ID"].ToString() + " AND FAMILY_ID IN(SELECT DISTINCT FAMILY_ID FROM TB_CATALOG_FAMILY WHERE CATEGORY_ID IN (SELECT CATEGORY_ID FROM CATEGORY_FUNCTION(" + CATALOG_ID + ", '" + cdr["CATEGORY_ID"].ToString() + "')))");
                                if (Dsfamilyname != null && Dsfamilyname.Tables[0].Rows.Count > 0)
                                {
                                    _stmpl_records.SetAttribute("TBT_FAMILY_ID", Dsfamilyname.Tables[0].Rows[0][0].ToString());
                                }
                            }
                            else if ((_Package.ToString() == "NEWPRODUCT" || _Package.ToString() == "PROMOTIONS" || _Package.ToString() == "MOREPRODUCTS") && dc.ColumnName.ToString().ToUpper() == "PRODUCT_ID")
                            {
                                DataSet Dsfamilyname = GetDataSet("select f.family_id,f.family_name,fs.string_value,a.attribute_id,a.attribute_name from tb_family_specs fs,tb_Family f,tb_catalog_family cf,tb_attribute a where f.family_id =fs.family_id and f.family_id=cf.family_id and fs.attribute_id=a.attribute_id and f.family_id in(SELECT TOP(1) FAMILY_ID FROM TB_PROD_FAMILY WHERE PRODUCT_ID =" + cdr["Product_ID"].ToString() + " ) and cf.catalog_id=" + CATALOG_ID);
                                if (Dsfamilyname != null && Dsfamilyname.Tables[0].Rows.Count > 0)
                                {
                                    _stmpl_records.SetAttribute("TBT_FAMILY_NAME", Dsfamilyname.Tables[0].Rows[0]["FAMILY_NAME"].ToString().Replace("<ars>g</ars>", "&rarr;"));
                                    // _stmpl_records.SetAttribute("TBT_FAMILY_ID", Dsfamilyname.Tables[0].Rows[0]["FAMILY_ID"].ToString());
                                    foreach (DataRow Drow in Dsfamilyname.Tables[0].Rows)
                                    {
                                        string desc = "";
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
                        foreach (DataRow dr in dsrecords.Tables[0].Select("PRODUCT_ID = " + cdr["PRODUCT_ID"].ToString()))
                        {
                            //_stmpl_records.SetAttribute(dc.ColumnName, dr[dc.ColumnName].ToString());                            
                            if (dr["ATTRIBUTE_DATATYPE"].ToString().ToUpper().StartsWith("TEX"))
                            {
                                if (dr["ATTRIBUTE_TYPE"].ToString() == "3" || dr["ATTRIBUTE_TYPE"].ToString() == "9")
                                {
                                    FileInfo Fil;
                                    if (_Package == "PRODUCT")
                                    {
                                        Fil = new FileInfo(strFile + dr["STRING_VALUE"].ToString().Replace("_TH", "_Images_200"));
                                    }
                                    else
                                    {
                                        Fil = new FileInfo(strFile + dr["STRING_VALUE"].ToString());
                                    }
                                    if (Fil.Exists)
                                    {
                                        if (_Package == "PRODUCT")
                                        {
                                            //_stmpl_records.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"].ToString().Replace("_TH", "_Images_200"));
                                        }
                                        else
                                        {
                                            _stmpl_records.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"]);
                                        }
                                    }
                                    else
                                    {
                                        _stmpl_records.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), "");
                                    }
                                }
                                else
                                {
                                    _stmpl_records.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"]);
                                }
                            }
                            else if (dr["ATTRIBUTE_DATATYPE"].ToString().ToUpper().StartsWith("NUM"))
                            {
                                if (dr["NUMERIC_VALUE"].ToString().Length > 0)
                                    _stmpl_records.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), oHelper.FixDecPlace(Convert.ToDecimal(dr["NUMERIC_VALUE"])).ToString());
                                else
                                    _stmpl_records.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), "");
                            }
                        }
                        if (_Package.ToString() == "PRODUCT")
                        {
                            bool descflag = false;
                            int familyrows = 0;
                            DataSet dsfamily = new DataSet();
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
                        } 
                        else
                        {
                            _stmpl_records.SetAttribute("TBT_FAMILY_ID", paraFID);
                        }
                        if (_Package.ToString() == "PRODUCT" || _Package.ToString() == "COMPARE" || _Package.ToString() == "NEWPRODUCT" || _Package.ToString() == "PROMOTIONS" || _Package.ToString() == "NEWPRODUCTNAV" || _Package.ToString() == "MOREPRODUCTS")
                        {
                            if (_Package.ToString() == "PRODUCT")
                            {
                                if (cdr["QTY_AVAIL"] != null && cdr["MIN_ORD_QTY"] != null)
                                {
                                    if (Convert.ToInt32(cdr["QTY_AVAIL"].ToString()) > 0)
                                    {
                                       // _stmpl_records.SetAttribute("TBT_QTY_AVAIL", cdr["QTY_AVAIL"].ToString());
                                       // _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", cdr["MIN_ORD_QTY"].ToString());
                                    }
                                    else
                                    {
                                        _stmpl_records.SetAttribute("TB_DISPLAY", "none");
                                    }
                                }
                                else
                                {
                                    _stmpl_records.SetAttribute("TB_DISPLAY", "none");                                   
                                }

                                if (cdr["FAMILY_PROD_COUNT"] != null && cdr["PROD_COUNT"] != null)
                                {
                                    if (cdr["FAMILY_PROD_COUNT"].ToString() != cdr["PROD_COUNT"].ToString())                                    
                                        _stmpl_records.SetAttribute("TBT_DISPLAY_TIP", "block");                                    
                                    else
                                        _stmpl_records.SetAttribute("TBT_DISPLAY_TIP", "none");
                                }
                                else                                
                                    _stmpl_records.SetAttribute("TBT_DISPLAY_TIP", "none");                                       

                            }
                            else
                            {
                                DataSet dsProduct = new DataSet();
                                dsProduct = GetDataSet("SELECT PRODUCT_ID,QTY_AVAIL,MIN_ORD_QTY FROM TBWC_INVENTORY WHERE PRODUCT_ID=" + cdr["PRODUCT_ID"].ToString());
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
                                    if (_Package.ToString() == "PRODUCT")
                                    {
                                        _stmpl_records.SetAttribute("TB_DISPLAY", "none");
                                    }
                                }
                            }
                            string prodValues = "";
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
                                            prodValues = prodValues + "<TR><TD bgcolor=\"white\" style=\"border-color:Black;\" align=\"center\" width=\"250px\"><strong>" + dr["attribute_name"] + ": </strong></TD><TD bgcolor=\"white\" align=\"center\" style=\"border-color:Black;\" width=\"250px\">" + " " + oHelper.GetOptionValues("CURRENCYFORMAT").ToString() + " " + Math.Round(Convert.ToDecimal(dr["NUMERIC_VALUE"]), 2).ToString() + "</TD></TR>";
                                        }
                                        else
                                        {
                                            prodValues = prodValues + "<TR><TD bgcolor=\"white\" style=\"border-color:Black;\" align=\"center\" width=\"250px\"><strong>" + dr["attribute_name"] + ": </strong></TD><TD bgcolor=\"white\" align=\"center\" style=\"border-color:Black;\" width=\"250px\">" + Math.Round(Convert.ToDecimal(dr["NUMERIC_VALUE"]), 2).ToString() + "</TD></TR>";
                                        }
                                }

                            }*/
                            foreach (DataRow dr in dsrecords.Tables[0].Select("ATTRIBUTE_TYPE <> 3 AND ATTRIBUTE_ID <> 1 and  PRODUCT_ID = " + cdr["PRODUCT_ID"].ToString(), "ATTRIBUTE_NAME"))
                            {
                                if (dr["attribute_name"].ToString().ToUpper() != "SUIT" && dr["attribute_name"].ToString().ToUpper() != "BRAND")
                                    if (dr["ATTRIBUTE_DATATYPE"].ToString().ToUpper().StartsWith("TEX"))
                                    {
                                        if (dr["STRING_VALUE"] != null && dr["STRING_VALUE"].ToString() != "" && dr["ATTRIBUTE_NAME"].ToString().ToUpper() != "NEW PRODUCTS")
                                            prodValues = prodValues + "<TR><TD bgcolor=\"white\" align=\"center\" style=\"border-color:Black;\" width=\"250px\"><strong>" + dr["attribute_name"] + ": </strong></TD><TD bgcolor=\"white\" align=\"center\" style=\"border-color:Black;\" width=\"250px\">" + dr["STRING_VALUE"] + "</TD></TR>";
                                    }
                            }
                            string _sPriceTable ="";
                            string _StockStatus="";
                            if (_Package.ToString() == "PRODUCT")
                            {
                                _sPriceTable = GetProductPriceTable(Convert.ToInt32(cdr["PRODUCT_ID"]));
                                _StockStatus = cdr["STOCK_STATUS_DESC"].ToString().Replace("_", " ");
                            }
                            else
                            {
                                _sPriceTable = GetProductPriceTable(Convert.ToInt32(cdr["PRODUCT_ID"]));
                                _StockStatus = GetStockStatus(Convert.ToInt32(cdr["PRODUCT_ID"]));
                            }
                            string _Tbt_Stock_Status = "";
                            string _Tbt_Stock_Status_1 = "";
                            bool _Tbt_Stock_Status_2 = false;
                            string _Tbt_Stock_Status_3 = "";
                            string _Colorcode1 = "";
                            string _Colorcode;
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

                            switch (_StockStatusTrim)
                            {
                                case "IN STOCK":
                                    _Colorcode = "#43A246";
                                    _Tbt_Stock_Status = "INSTOCK";
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
                                    _Tbt_Stock_Status_2 = true;
                                    //_Tbt_Stock_Status = "<span style=\"color:" + _Colorcode + "\">TEMPORARY UNAVAILABLE NO ETA</span>";
                                    _Tbt_Stock_Status_3 = "TEMPORARY UNAVAILABLE NO ETA";
                                    break;
                                case "TEMPORARY UNAVAILABLE NO ETA":
                                    _Colorcode = "#F9A023";
                                    _Tbt_Stock_Status_2 = true;
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
                                default:
                                    _Colorcode = "Black";
                                    _Tbt_Stock_Status = _StockStatusTrim;
                                    break;
                            }




                            _stmpl_records.SetAttribute("TBT_COLOR_CODE", _Colorcode);

                            if (_Tbt_Stock_Status != "")
                            {
                                _stmpl_records.SetAttribute("TBT_STOCK_STATUS", _Tbt_Stock_Status);
                            }
                            else
                            {
                                _stmpl_records.SetAttribute("TBT_STOCK_STATUS_3", _Tbt_Stock_Status_3);
                            }
                            _stmpl_records.SetAttribute("TBT_COLOR_CODE_1", _Colorcode1);
                            _stmpl_records.SetAttribute("TBT_STOCK_STATUS_1", _Tbt_Stock_Status_1);
                            _stmpl_records.SetAttribute("TBT_STOCK_STATUS_2", _Tbt_Stock_Status_2);
                            //_stmpl_records.SetAttribute("TBT_STOCK_STATUS", GetStockStatus(Convert.ToInt32(cdr["PRODUCT_ID"])));
                            _stmpl_records.SetAttribute("TBT_PRICE_TABLE", _sPriceTable);
                            _stmpl_records.SetAttribute("TBT_PDISPLAY_MODE", _sPriceTable == "" ? "none" : "");
                            if (prodValues != "")
                            {
                                if (_Package == "PRODUCT")
                                {
                                    GetProductDetails(Convert.ToInt32(cdr["PRODUCT_ID"]), Convert.ToInt32(paraFID), _stmpl_records);
                                    //GetProductDesc(Convert.ToInt32(cdr["PRODUCT_ID"]), Convert.ToInt32(paraFID), _stmpl_records);
                                }
                                _stmpl_records.SetAttribute("TBT_ALL_PRODUCTVALUES", prodValues);
                                _stmpl_records.SetAttribute("TBT_DISPLAY_MODE", "");
                            }
                            else
                            {
                                _stmpl_records.SetAttribute("TBT_DISPLAY_MODE", "none");
                            }
                        }
                        if (_Package.ToString().Contains("FAMILYPAGE"))
                        {
                            DataSet dsProduct = new DataSet();
                            dsProduct = GetDataSet("SELECT PRODUCT_ID,QTY_AVAIL,MIN_ORD_QTY FROM TBWC_INVENTORY WHERE PRODUCT_ID=" + cdr["PRODUCT_ID"].ToString());
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

        private decimal GetMyPrice(int ProductID)
        {
            decimal retval = 0.00M;
            try
            {
                string userid = HttpContext.Current.Session["USER_ID"].ToString();
                if (!string.IsNullOrEmpty(userid))
                {
                    string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
                    oHelper.SQLString = sSQL;
                    int pricecode = oHelper.CI(oHelper.GetValue("price_code"));

                    if (!string.IsNullOrEmpty(userid))
                    {
                        string strquery = "";
                        if (pricecode == 1)
                        {
                            strquery = string.Format("exec GetWESIncProductPrice {0},{1},{2}", ProductID, 1, HttpContext.Current.Session["USER_ID"]);
                        }
                        else
                        {
                            strquery = string.Format("exec GetWESProductPrice {0},{1},{2}", ProductID, 1, HttpContext.Current.Session["USER_ID"]);
                        }

                        DataSet DSprice = new DataSet();
                        oHelper.SQLString = strquery;
                        retval = Math.Round(Convert.ToDecimal(oHelper.GetValue("Numeric_Value")), 2);
                    }
                }
            }
            catch
            {
            }
            return retval;
        }

        private string GetStockStatus(int ProductID)
        {
            string Retval = "NO STATUS AVAILABLE";
            try
            {
                string sSQL = string.Format("select PROD_STK_STATUS_DSC from WESTB_PRODUCT_ITEM WHERE PRODUCT_ID = {0}", ProductID);
                oHelper.SQLString = sSQL;
                Retval = oHelper.GetValue("PROD_STK_STATUS_DSC").ToString().Replace("_", " ");
            }
            catch
            {
            }
            return Retval;
        }

        private string GetProductPriceTable(int ProductID)
        {
            string _sPriceTable = "";
            string userid = HttpContext.Current.Session["USER_ID"].ToString();
            if (!string.IsNullOrEmpty(userid))
            {
                string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
                oHelper.SQLString = sSQL;
                int pricecode = oHelper.CI(oHelper.GetValue("price_code"));
                DataSet dsPriceTable = new DataSet();
                SqlDataAdapter oDa = new SqlDataAdapter();
                oDa.SelectCommand = new SqlCommand();
                oDa.SelectCommand.CommandText = "GetPriceTable";
                oDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                oDa.SelectCommand.Connection = new SqlConnection(_DBConnectionString);
                oDa.SelectCommand.Parameters.Clear();
                oDa.SelectCommand.Parameters.AddWithValue("@ProductID", ProductID);
                oDa.SelectCommand.Parameters.AddWithValue("@UserID", userid);
                oDa.Fill(dsPriceTable, "Price");
                _sPriceTable = "";

                int TotalCount = 0;
                int RowCount = 0;

                if (pricecode == 3)
                    foreach (DataRow oDr in dsPriceTable.Tables["Price"].Rows)
                    {
                        _sPriceTable += string.Format("<tr><td class=\"bg_grey3\"><b>{0}</b></td><td align=\"center\">${1:0.00}</td><td align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);
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
                        _sPriceTable += string.Format("<tr><td class=\"{3}\"><b>{0}</b></td><td  class=\"{3}\" align=\"center\">${1:0.00}</td><td class=\"{3}\" align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                    }
                }
            }
            return _sPriceTable;
        }

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
            DataSet tmp=(DataSet)HttpContext.Current.Session["FamilyProduct"];
            if (tmp!=null)
            {
                DataRow[] Dr = tmp.Tables[0].Select("ATTRIBUTE_TYPE=1");
                if (Dr.Length > 0)
                {
                    oDs.Tables.Add(Dr.CopyToDataTable());
                    oDs.Tables[0].TableName = "PrdDetails";
                }
            }
            
            if (oDs != null & oDs.Tables.Count>0)
            {
               // DataRow[] Familyspec2 = oDs.Tables[0].Select("ATTRIBUTE_ID in (13,4,240,241,2,51,18)");

                foreach (DataRow oDr in oDs.Tables["PrdDetails"].Rows)
                {
                    if (oDr["ATTRIBUTE_NAME"].ToString() != "Long Description")
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

        private void GetProductDesc(int ProductID, int FamilyID, StringTemplate st)
        {
            //SqlConnection oCon = new SqlConnection(_DBConnectionString);
            //SqlDataAdapter oDa = new SqlDataAdapter("GetFamilyDetails", oCon);
            //oDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            //oDa.SelectCommand.Parameters.Clear();
            //oDa.SelectCommand.Parameters.AddWithValue("@FamilyID", FamilyID);
            DataSet oDs = new DataSet();

            DataSet tmp = (DataSet)HttpContext.Current.Session["FamilyProduct"];
            if (tmp != null)
            {
                DataRow[] Dr = tmp.Tables[0].Select("ATTRIBUTE_TYPE=7");
                if (Dr.Length > 0)
                {
                    oDs.Tables.Add(Dr.CopyToDataTable());
                    oDs.Tables[0].TableName = "PrdDetails" ;
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


        private void GetMultipleImages(int ProductID, int FamilyID, StringTemplate st)
        {
            string strfile = HttpContext.Current.Server.MapPath("ProdImages");
            SqlConnection oCon = new SqlConnection(_DBConnectionString);
            SqlDataAdapter oDa = new SqlDataAdapter("GetProductImages", oCon);
            oDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            oDa.SelectCommand.Parameters.Clear();
            oDa.SelectCommand.Parameters.AddWithValue("@ProductID", ProductID);
            oDa.SelectCommand.Parameters.AddWithValue("@FamilyID", FamilyID);
            DataSet oDs = new DataSet();            
            oDa.Fill(oDs, "Images");                        
            bool firstImg = true;
            if (oDs != null)
            {
                foreach (DataRow oDr in oDs.Tables["Images"].Rows)
                {
                    ProductImage oPrd = new ProductImage();
                    if (File.Exists(string.Format(@"{0}/{1}", strfile, oDr["STRING_VALUE"].ToString().Replace("\\", "/"))))
                    {
                        oPrd.LargeImage = oDr["STRING_VALUE"].ToString().Replace("\\", "/");
                        oPrd.Thumpnail = oPrd.LargeImage.ToLower().Replace("_images", "_th50");
                        oPrd.SmallImage = oPrd.LargeImage.ToLower().Replace("_images", "_th");
                        oPrd.MediumImage = oPrd.LargeImage.ToLower().Replace("_images", "_images_200");
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


        private void GetFamilyMultipleImages(int FamilyID, StringTemplate st)
        {
            string strfile = HttpContext.Current.Server.MapPath("ProdImages");
            //SqlConnection oCon = new SqlConnection(_DBConnectionString);
            //SqlDataAdapter oDa = new SqlDataAdapter("GetFamilyImages", oCon);
            //oDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            //oDa.SelectCommand.Parameters.Clear();
            //oDa.SelectCommand.Parameters.AddWithValue("@FamilyID", FamilyID);
            DataSet oDs = new DataSet();
            //oDa.Fill(oDs, "Images");            
            DataTable dt=new DataTable();
            DataRow[] dr;
            oDs = (DataSet)HttpContext.Current.Session["FamilyProduct"];
            bool firstImg = true;
            if (oDs != null)
            {
                dr = oDs.Tables[0].Select("ATTRIBUTE_TYPE=9 And ATTRIBUTE_ID Not in (746, 747)");
                if (dr.Length > 0)
                {
                    dt = dr.CopyToDataTable();
                    foreach (DataRow oDr in dt.Rows)
                    {
                        ProductImage oPrd = new ProductImage();
                        if (File.Exists(string.Format(@"{0}/{1}", strfile, oDr["STRING_VALUE"].ToString().Replace("\\", "/"))))
                        {
                            oPrd.LargeImage = oDr["STRING_VALUE"].ToString().Replace("\\", "/");
                            oPrd.Thumpnail = oPrd.LargeImage.ToLower().Replace("_images", "_th50");
                            oPrd.SmallImage = oPrd.LargeImage.ToLower().Replace("_images", "_th");
                            oPrd.MediumImage = oPrd.LargeImage.ToLower().Replace("_images", "_images_200");
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
                    dsrecords =EasyAsk.GetCategoryAndBrand("MainCategory");              
            }
            else  if (_Package == "BROWSEBYCATEGORY" || _Package == "BROWSEBYBRAND" || _Package == "BROWSEBYPRODUCT") // unwanted db calls
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
                            _stmpl_records.SetAttribute("TBT_CAT_ID", HttpContext.Current.Request.QueryString["cid"].ToString());
                        }
                        if (HttpContext.Current.Request.QueryString["tsb"] != null)
                        {
                            _stmpl_records.SetAttribute("TBT_TOSUITE_BRAND", HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()));
                            _stmpl_records.SetAttribute("TBT_TOSUITE_BRAND1", HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsb"].ToString()));
                        }
                        if (HttpContext.Current.Request.QueryString["sl1"] != null)
                            _stmpl_records.SetAttribute("TBT_TOSUITE_SL1", HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["sl1"].ToString()));
                        if (HttpContext.Current.Request.QueryString["sl2"] != null)
                            _stmpl_records.SetAttribute("TBT_TOSUITE_SL2", HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["sl2"].ToString()));
                        foreach (DataColumn dc in dsrecords.Tables[0].Columns)
                        {
                            //_stmpl_records.SetAttribute(dc.ColumnName, dr[dc.ColumnName].ToString());                                                       
                            if ((_Package == "TOP" || _Package == "TOPLOG") &&  dc.ColumnName.ToString().ToUpper()=="CATEGORY_NAME" &&  dr[dc.ColumnName.ToString()].ToString().Length > 8)
                            {
                                string sttr = dr[dc.ColumnName.ToString()].ToString();
                                int indx = sttr.IndexOf(" ", 7);
                                if (indx >= 7)
                                    sttr = sttr.Substring(0, indx) + "<br/>" + sttr.Substring(indx + 1);
                                else if (sttr == "VCR COMPONENTS")
                                    sttr = "VCR<br/>COMPONENTS";
                                _stmpl_records.SetAttribute("TBT_" + dc.ColumnName.ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), sttr);
                            }
                            else
                            {
                                if (dc.ColumnName.ToString().ToUpper() == "IMAGE_FILE")
                                {
                                    FileInfo Fil = new FileInfo(strFile + dr[dc.ColumnName.ToString()].ToString());
                                    if (Fil.Exists)
                                    {
                                        _stmpl_records.SetAttribute("TBT_" + dc.ColumnName.ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr[dc.ColumnName.ToString()].ToString());
                                    }
                                    else
                                    {
                                        _stmpl_records.SetAttribute("TBT_" + dc.ColumnName.ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), "");
                                    }
                                }
                                else
                                {
                                    if (dc.ColumnName.ToString().ToUpper() == "CUSTOM_NUM_FIELD3" && dr[dc.ColumnName.ToString()].ToString() == "2")
                                    {
                                        _stmpl_records.SetAttribute("TBT_" + dc.ColumnName.ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr[dc.ColumnName.ToString()].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"));// + "&bypcat=1");
                                    }
                                    else if (dc.ColumnName.ToUpper().ToString() == "EA_PATH" &&(_Package == "TOP" || _Package == "TOPLOG"))
                                    {
                                        _stmpl_records.SetAttribute("TBT_EAPATH", HttpUtility.UrlEncode(oHelper.StringEnCrypt(dr["EA_PATH"].ToString())));
                                    }
                                    else
                                    {
                                        if (dc.ColumnName.ToString().ToUpper() == "TOSUITE_MODEL" || dc.ColumnName.ToString().ToUpper() == "TOSUITE_BRAND")
                                        {
                                            _stmpl_records.SetAttribute("TBT_" + dc.ColumnName.ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), HttpUtility.UrlEncode(dr[dc.ColumnName.ToString()].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")));
                                            _stmpl_records.SetAttribute("TBT_" + dc.ColumnName.ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper() + "1", dr[dc.ColumnName.ToString()].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"));
                                        }
                                        else
                                        {
                                            _stmpl_records.SetAttribute("TBT_" + dc.ColumnName.ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr[dc.ColumnName.ToString()].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"));
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

        private DataSet GetDataSet(string SQLQuery)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(SQLQuery, _DBConnectionString);
            da.Fill(ds, "generictable");
            return ds;
        }

        private DataSet GetDataSet(string SQLQuery, string SQLType, string SQLParam)
        {
            DataSet ds = new DataSet();
            if (paraValue != "" && SQLParam != "")
                SQLQuery = SQLQuery.Replace(SQLParam, paraValue);
            if (_Package == "BROWSEBYCATEGORY" && HttpContext.Current.Session["PARAFILTER"] == "Value")
            {
                if (SQLQuery.Contains("order by tc.category_id"))
                    SQLQuery = SQLQuery.Replace("order by tc.category_id", " and tC.CATEGORY_ID in (select distinct category_id from tb_family where family_id in (select family_id from tb_prod_family where product_id in  (select product_id from TBWC_SEARCH_PROD_LIST where user_session_id = '" + HttpContext.Current.Session.SessionID + "')))" + " order by tc.category_id");
            }
            SqlDataAdapter da = new SqlDataAdapter(SQLQuery, _DBConnectionString);
            da.Fill(ds, "generictable");
            return ds;
        }
        private DataSet ProductFilterFlatTable(DataSet flatDataset)
        {
            {
                StringBuilder SQLstring = new StringBuilder();
                DataSet oDsProductFilter = new DataSet();
                string SQLString = " SELECT PRODUCT_FILTERS FROM TB_CATALOG WHERE  CATALOG_ID = " + CATALOG_ID + " ";
                SqlDataAdapter da = new SqlDataAdapter(SQLString, _DBConnectionString);
                da.Fill(oDsProductFilter);
                string sProductFilter = string.Empty;
                if (oDsProductFilter.Tables[0].Rows.Count > 0 && oDsProductFilter.Tables[0].Rows[0].ItemArray[0].ToString() != string.Empty)
                {
                    sProductFilter = oDsProductFilter.Tables[0].Rows[0].ItemArray[0].ToString();
                    XmlDocument xmlDOc = new XmlDocument();
                    xmlDOc.LoadXml(sProductFilter);
                    XmlNode rNode = xmlDOc.DocumentElement;

                    if (rNode.ChildNodes.Count > 0)
                    {
                        for (int i = 0; i < rNode.ChildNodes.Count; i++)
                        {
                            XmlNode TableDataSetNode = rNode.ChildNodes[i];

                            if (TableDataSetNode.HasChildNodes)
                            {
                                if (TableDataSetNode.ChildNodes[2].InnerText == " ")
                                {
                                    TableDataSetNode.ChildNodes[2].InnerText = "=";
                                }
                                if (TableDataSetNode.ChildNodes[0].InnerText == " ")
                                {
                                    TableDataSetNode.ChildNodes[0].InnerText = "0";
                                }
                                string stringval = TableDataSetNode.ChildNodes[3].InnerText.Replace("'", "''");
                                DataSet attribuetypeDS = new DataSet();
                                string sSQLString = " SELECT ATTRIBUTE_DATATYPE FROM TB_ATTRIBUTE WHERE  ATTRIBUTE_ID = " + Convert.ToInt32(TableDataSetNode.ChildNodes[0].InnerText) + " ";
                                SqlDataAdapter das = new SqlDataAdapter(sSQLString, _DBConnectionString);
                                das.Fill(attribuetypeDS);
                                if (attribuetypeDS.Tables[0].Rows[0].ItemArray[0].ToString().ToUpper().Contains("TEX") == true || attribuetypeDS.Tables[0].Rows[0].ItemArray[0].ToString().ToUpper().Contains("DATE") == true)
                                {

                                    if (TableDataSetNode.ChildNodes[4].InnerText != "NONE")
                                    {
                                        SQLstring.Append("SELECT DISTINCT PRODUCT_ID FROM [PRODUCT SPECIFICATION](" + CATALOG_ID + ") WHERE (STRING_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ") " + "\n");
                                    }
                                    else
                                    {
                                        SQLstring.Append("SELECT DISTINCT PRODUCT_ID FROM [PRODUCT SPECIFICATION](" + CATALOG_ID + ") WHERE (STRING_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ")" + "\n");
                                    }
                                }
                                else if (attribuetypeDS.Tables[0].Rows[0].ItemArray[0].ToString().ToUpper().Contains("DECI") == true || attribuetypeDS.Tables[0].Rows[0].ItemArray[0].ToString().ToUpper().Contains("NUM") == true)
                                {
                                    if (TableDataSetNode.ChildNodes[4].InnerText != "NONE")
                                    {
                                        SQLstring.Append("SELECT DISTINCT PRODUCT_ID FROM [PRODUCT SPECIFICATION](" + CATALOG_ID + ") WHERE  (NUMERIC_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ") " + "\n");
                                    }
                                    else
                                    {
                                        SQLstring.Append("SELECT DISTINCT PRODUCT_ID FROM [PRODUCT SPECIFICATION](" + CATALOG_ID + ") WHERE (NUMERIC_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ")" + "\n");
                                    }
                                }


                            }
                            if (TableDataSetNode.ChildNodes[4].InnerText == "NONE")
                            {
                            }
                            if (TableDataSetNode.ChildNodes[4].InnerText == "AND")
                            {
                                SQLstring.Append(" INTERSECT \n");
                            }
                            if (TableDataSetNode.ChildNodes[4].InnerText == "OR")
                            {
                                SQLstring.Append(" UNION \n");
                            }

                        }

                    }

                }
                string productFiltersql = SQLstring.ToString();
                // Boolean variableFilter = false;
                if (productFiltersql.Length > 0)
                {
                    string s = "SELECT PRODUCT_ID FROM [PRODUCT FAMILY](" + CATALOG_ID + ") WHERE CATALOG_ID=" + CATALOG_ID + " AND PRODUCT_ID IN\n" +
                          "(\n";// +
                    //"SELECT DISTINCT PRODUCT_ID\n" +
                    //"FROM [PRODUCT SPECIFICATION](" + CATALOG_ID + ") \n" +
                    //"WHERE\n";
                    productFiltersql = s + productFiltersql + "\n)";
                    SqlDataAdapter dad = new SqlDataAdapter(productFiltersql, _DBConnectionString);
                    dad.Fill(oDsProductFilter);

                    bool available = false;

                    for (int rowCount = 0; rowCount < flatDataset.Tables[0].Rows.Count; rowCount++)
                    {//foreach (DataRow odr in flatDataset.Tables[0].Rows)
                        DataRow odr = flatDataset.Tables[0].Rows[rowCount];
                        available = false;
                        foreach (DataRow dr in oDsProductFilter.Tables[0].Rows)
                        {
                            if (dr["PRODUCT_ID"].ToString() == odr["PRODUCT_ID"].ToString())
                            {
                                available = true;
                            }

                        }
                        if (available == false)
                        {
                            string cmdd = " DELETE FROM TBWC_SEARCH_PROD_LIST WHERE PRODUCT_ID = " + odr["PRODUCT_ID"].ToString() + " AND USER_SESSION_ID='" + HttpContext.Current.Session.SessionID + "'";
                            SqlConnection _SQLConn = new SqlConnection(_DBConnectionString);
                            SqlCommand pscmd = new SqlCommand(cmdd, _SQLConn);
                            pscmd.CommandType = CommandType.Text;
                            int valr = pscmd.ExecuteNonQuery();
                            odr.Delete();
                            flatDataset.AcceptChanges();
                            rowCount--;
                        }

                    }

                }
            }
            return flatDataset;
        }

        private bool IsPDFAttached()
        {
            bool retvalue = false;

            if (paraCID != null && !string.IsNullOrEmpty(paraCID.Trim()))
            {
                string sSQL = "SELECT IMAGE_FILE2 FROM TB_CATEGORY WHERE CATEGORY_ID = '" + paraCID + "'";
                oHelper.SQLString = sSQL;


            }

            return retvalue;
        }

        private bool IsEcomenabled()
        {
            bool retvalue = false;
            string userid = HttpContext.Current.Session["USER_ID"].ToString();
            if (!string.IsNullOrEmpty(userid))
            {
                string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                string sSQL = "SELECT USER_ROLE FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " and USER_ID = " + userid;
                oHelper.SQLString = sSQL;
                int iROLE = oHelper.CI(oHelper.GetValue("USER_ROLE"));
                if (iROLE <= 3)
                    retvalue = true;
            }
            return retvalue;
        }

        private string GetLoginName()
        {
            string retvalue = string.Empty;
            string userid = HttpContext.Current.Session["USER_ID"].ToString();
            if (!string.IsNullOrEmpty(userid))
            {
                string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                string sSQL = "SELECT CONTACT FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " and USER_ID = " + userid;
                oHelper.SQLString = sSQL;
                string iLoginName = oHelper.GetValue("CONTACT");
                retvalue = iLoginName;
            }
            return retvalue;
        }

        private string GetCompanyName()
        {
            string retvalue = string.Empty;
            string userid = HttpContext.Current.Session["USER_ID"].ToString();
            if (!string.IsNullOrEmpty(userid))
            {
                string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                string sSQL = "SELECT COMPANY_ID FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " and USER_ID = " + userid;
                oHelper.SQLString = sSQL;
                int iCompanyID = oHelper.CI(oHelper.GetValue("COMPANY_ID"));
                string sSQL1 = "SELECT COMPANY_NAME FROM TBWC_COMPANY WHERE WEBSITE_ID = " + websiteid + " and COMPANY_ID = " + iCompanyID;
                oHelper.SQLString = sSQL1;
                retvalue = oHelper.GetValue("COMPANY_NAME").ToString().Trim();
            }
            return retvalue;
        }

        private DataSet FamilyFilterFlatTable(DataSet flatDataset)
        {

            {
                StringBuilder SQLstring = new StringBuilder();
                DataSet oDsFamilyFilter = new DataSet();
                string SQLString = " SELECT FAMILY_FILTERS FROM TB_CATALOG WHERE  CATALOG_ID = " + CATALOG_ID + " ";
                SqlDataAdapter da = new SqlDataAdapter(SQLString, _DBConnectionString);
                da.Fill(oDsFamilyFilter);
                string sFamilyFilter = string.Empty;
                if (oDsFamilyFilter.Tables[0].Rows.Count > 0 && oDsFamilyFilter.Tables[0].Rows[0].ItemArray[0].ToString() != string.Empty)
                {
                    sFamilyFilter = oDsFamilyFilter.Tables[0].Rows[0].ItemArray[0].ToString();
                    XmlDocument xmlDOc = new XmlDocument();
                    xmlDOc.LoadXml(sFamilyFilter);
                    XmlNode rNode = xmlDOc.DocumentElement;

                    if (rNode.ChildNodes.Count > 0)
                    {
                        for (int i = 0; i < rNode.ChildNodes.Count; i++)
                        {
                            XmlNode TableDataSetNode = rNode.ChildNodes[i];

                            if (TableDataSetNode.HasChildNodes)
                            {
                                if (TableDataSetNode.ChildNodes[2].InnerText == " ")
                                {
                                    TableDataSetNode.ChildNodes[2].InnerText = "=";
                                }
                                if (TableDataSetNode.ChildNodes[0].InnerText == " ")
                                {
                                    TableDataSetNode.ChildNodes[0].InnerText = "0";
                                }
                                string stringval = TableDataSetNode.ChildNodes[3].InnerText.Replace("'", "''");
                                if (TableDataSetNode.ChildNodes[4].InnerText != "NONE")
                                {
                                    SQLstring.Append("SELECT DISTINCT FAMILY_ID FROM [FAMILY DESCRIPTION](" + CATALOG_ID + ") WHERE (STRING_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ") " + "\n");
                                }
                                else
                                {
                                    SQLstring.Append("SELECT DISTINCT FAMILY_ID FROM [FAMILY DESCRIPTION](" + CATALOG_ID + ") WHERE  (STRING_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ")" + "\n");
                                }


                            }
                            if (TableDataSetNode.ChildNodes[4].InnerText == "NONE")
                            {
                            }
                            if (TableDataSetNode.ChildNodes[4].InnerText == "AND")
                            {
                                SQLstring.Append(" INTERSECT \n");
                            }
                            if (TableDataSetNode.ChildNodes[4].InnerText == "OR")
                            {
                                SQLstring.Append(" UNION \n");
                            }

                        }

                    }

                }
                string familyFiltersql = SQLstring.ToString();

                if (familyFiltersql.Length > 0)
                {
                    string s = "SELECT FAMILY_ID FROM FAMILY(" + CATALOG_ID + ") WHERE CATALOG_ID=" + CATALOG_ID + " AND FAMILY_ID IN\n" +
                          "(\n";// +
                    //"SELECT DISTINCT FAMILY_ID\n" +
                    //"FROM [FAMILY DESCRIPTION](" + CATALOG_ID + ")\n" +
                    //"WHERE\n";
                    familyFiltersql = s + familyFiltersql + "\n)";

                    SqlDataAdapter dda = new SqlDataAdapter(familyFiltersql, _DBConnectionString);
                    dda.Fill(oDsFamilyFilter);

                    bool available = false;
                    DataSet AvailableDs = flatDataset;
                    for (int rowCount = 0; rowCount < flatDataset.Tables[0].Rows.Count; rowCount++)
                    {//foreach (DataRow odr in flatDataset.Tables[0].Rows)
                        DataRow odr = flatDataset.Tables[0].Rows[rowCount];
                        available = false;
                        foreach (DataRow dr in oDsFamilyFilter.Tables[0].Rows)
                        {
                            if (dr["FAMILY_ID"].ToString() == odr["FAMILY_ID"].ToString() || dr["FAMILY_ID"].ToString() == odr["SUBFAMILY_ID"].ToString())
                            {
                                available = true;
                            }

                        }
                        if (available == false)
                        {
                            //string cmdd = " DELETE FROM TBWC_SEARCH_PROD_LIST WHERE FAMILY_ID = " + odr["FAMILY_ID"].ToString() + " OR FAMILY_ID = " + odr["SUBFAMILY_ID"].ToString() + " AND  USER_SESSION_ID='" + HttpContext.Current.Session.SessionID + "'";
                            //SqlConnection _SQLConn = new SqlConnection(_DBConnectionString);
                            //SqlCommand pscmd = new SqlCommand(cmdd, _SQLConn);
                            //pscmd.CommandType = CommandType.Text;
                            //int valr = pscmd.ExecuteNonQuery();
                            odr.Delete();
                            flatDataset.AcceptChanges();
                            rowCount--;
                        }

                    }


                }

            }
            //ProductFilterFlatTable(flatDataset);
            return flatDataset;
        }

    }

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
}
