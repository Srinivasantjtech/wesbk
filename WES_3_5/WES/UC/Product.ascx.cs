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
using StringTemplate = Antlr3.ST.StringTemplate;  
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Data.SqlClient;
using System.Text;
using Pechkin;
using System.Net.Mail;
using System.Globalization;

    public partial class UC_Products : System.Web.UI.UserControl
    {
        // ConnectionDB objConnectionDB = new ConnectionDB();
        HelperServices objHelperServices = new HelperServices();
        string breadcrumb = string.Empty;
        EasyAsk_WES objEasyAsk = new EasyAsk_WES();
        ErrorHandler objErrorHandler = new ErrorHandler();

        //private HelperDB objHelperDB = new HelperDB();
        //private Security objSecurity = new Security();
       // string strprint;

        CategoryServices objCategoryServices = new CategoryServices();
        EasyAsk_WES EasyAsk = new EasyAsk_WES();
        public string paraFID = string.Empty;
        public string Productcode = string.Empty;
        public string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                hfpath.Value = Request.QueryString["path"].ToString();
                if ((HttpContext.Current.Request.QueryString["printprice"] != null) && (HttpContext.Current.Request.QueryString["printdet"] != null))
                {


                    if (Request.QueryString["path"] != null)
                    {

                    hfpath.Value = Request.QueryString["path"].ToString().Replace(" ","+") ;
                    }
                    //objErrorHandler.CreateLog(hfpath.Value);
                    //if (Request.QueryString["fid"] != null)
                    //{

                    //    hffid.Value = Request.QueryString["fid"].ToString();
                    //}
                    //if (Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "")
                    //{
                    //    hfcid.Value = Request.QueryString["cid"].ToString();
                    //}
                    //hfdetailspdf.Value = Request.QueryString["printdet"].ToString();
                    //hfpricepdf.Value = Request.QueryString["printprice"].ToString();
                    //ST_PDF(hfpricepdf.Value, hfdetailspdf.Value, "cell_pdf", hfpid.Value, hffid.Value, hfcid.Value);

                    if (HttpContext.Current.Request.QueryString["pid"] != null)
                    {

                        hfpid.Value = HttpContext.Current.Request.QueryString["pid"];
                    }
                    if (HttpContext.Current.Request.QueryString["fid"] != null)
                    {

                        hffid.Value = HttpContext.Current.Request.QueryString["fid"];
                    }
                    if (HttpContext.Current.Request.QueryString["cid"] != null && HttpContext.Current.Request.QueryString["cid"].ToString() != "")
                    {
                        hfcid.Value = HttpContext.Current.Request.QueryString["cid"];
                    }
                    hfdetailspdf.Value = HttpContext.Current.Request.QueryString["printdet"];
                    hfpricepdf.Value = HttpContext.Current.Request.QueryString["printprice"];

                   // objErrorHandler.CreateLog("pid_fid_" + hfpid.Value+"_" + hffid.Value);
                    ST_PDF(hfpricepdf.Value, hfdetailspdf.Value, "cell_pdf", hfpid.Value, hffid.Value, hfcid.Value,hfpath.Value);

                }

                if ((HttpContext.Current.Request.QueryString["emailprice"] != null) && (HttpContext.Current.Request.QueryString["emaildet"] != null))
                {

                    if (Request.QueryString["path"] != null)
                    {

                        hfpath.Value = Request.QueryString["path"].ToString().Replace(" ", "+");
                    }
                    //if (Request.QueryString["pid"] != null)
                    //{

                    //    hfpid.Value = Request.QueryString["pid"].ToString();
                    //}
                    //if (Request.QueryString["fid"] != null)
                    //{

                    //    hffid.Value = Request.QueryString["fid"].ToString();
                    //}
                    //if (Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "")
                    //{
                    //    hfcid.Value = Request.QueryString["cid"].ToString();
                    //}
                    //hfdetailspdf.Value = Request.QueryString["emaildet"].ToString();
                    //hfpricepdf.Value = Request.QueryString["emailprice"].ToString();


                    if (HttpContext.Current.Request.QueryString["pid"] != null)
                    {

                        hfpid.Value = HttpContext.Current.Request.QueryString["pid"].ToString();
                    }
                    if (HttpContext.Current.Request.QueryString["fid"] != null)
                    {

                        hffid.Value = HttpContext.Current.Request.QueryString["fid"].ToString();
                    }
                    if (HttpContext.Current.Request.QueryString["cid"] != null && HttpContext.Current.Request.QueryString["cid"].ToString() != "")
                    {
                        hfcid.Value = HttpContext.Current.Request.QueryString["cid"].ToString();
                    }
                    hfdetailspdf.Value = HttpContext.Current.Request.QueryString["emaildet"].ToString();
                    hfpricepdf.Value = HttpContext.Current.Request.QueryString["emailprice"].ToString();

                    string Email = "Email_" + hfpid.Value;
                    string txtemail = string.Empty;
                    if (Session[Email] != null)
                    {

                        txtemail = Session[Email].ToString();
                    }
                    string Notes = "Notes_" + hfpid.Value;
                    string txtnotes = string.Empty;
                    if (Session[Notes] != null)
                    {
                        txtnotes = Session[Notes].ToString();
                    }


                    ST_Email_PDF(hfpricepdf.Value, hfdetailspdf.Value, "cell_pdf", txtemail, txtnotes, hfpid.Value, hffid.Value, hfcid.Value, hfpath.Value);
                }
               
            }
            catch (Exception ex)
            {
                //objErrorHandler.ErrorMsg = ex;
               // objErrorHandler.CreateLog(); 
            }
        }
        public string Bread_Crumbs()
        {

            breadcrumb = objEasyAsk.GetBreadCrumb(Server.MapPath("Templates"));
            return breadcrumb;
        }
       


        public string ST_Product()
        {
            try
            {
                ConnectionDB objConnectionDB = new ConnectionDB();
                TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("PRODUCT", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
                if (Request.QueryString["pid"] != null)
                {
                    tbwtEngine.paraValue = Request.QueryString["pid"].ToString();
                    tbwtEngine.paraPID = Request.QueryString["pid"].ToString();
                    Session["p_pid"] = Request.QueryString["pid"].ToString();
                    hfpid.Value = Request.QueryString["pid"].ToString(); 
                }
                if (Request.QueryString["fid"] != null)
                {
                    tbwtEngine.paraFID = Request.QueryString["fid"].ToString();
                    Session["p_fid"] = Request.QueryString["fid"].ToString();
                    hffid.Value = Request.QueryString["fid"].ToString();
                }
                if (Request.QueryString["cid"] != null && Request.QueryString["cid"].ToString() != "")
                {
                    tbwtEngine.paraCID = Request.QueryString["cid"].ToString();
                    Session["p_cid"] = Request.QueryString["cid"].ToString();
                    hfcid.Value = Request.QueryString["cid"].ToString(); 
                }
                
                tbwtEngine.RenderHTML("Column");
                objConnectionDB.CloseConnection();
                return (tbwtEngine.RenderedHTML);
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return string.Empty;
            }
        }

        public string ST_Product_print(string WithPrice, string Details, string cellname, string pid, string fid, string cid, DataSet DsProductDetails,string EA)
        {
            try
            {

              
                HelperServices objHelperServices = new HelperServices();
                EasyAsk_WES objEasyAsk = new EasyAsk_WES();
        ErrorHandler objErrorHandler = new ErrorHandler();
        CategoryServices objCategoryServices = new CategoryServices();
        EasyAsk_WES EasyAsk = new EasyAsk_WES();
        HelperDB objHelperDB = new HelperDB();
        Security objSecurity = new Security();
        string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();                // lblbreadcrum.Text = breadcrumb;
                //lblbreadcrum1.Text = breadcrumb;
     breadcrumb=  objEasyAsk.GetBreadCrumb_print();
     string strValue = string.Empty;
                string paraFID = "0";
                ConnectionDB objConnectionDB = new ConnectionDB();
                TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("PRODUCT_PRINT",HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
                if (pid!= null)
                {
                    tbwtEngine.paraValue = pid;
                    tbwtEngine.paraPID = pid;
                }
                if (fid != null)
                {
                    tbwtEngine.paraFID = fid;
                    paraFID = fid;
                }
                if (cid!= null && cid != "")
                {
                    tbwtEngine.paraCID = cid;
                }
                tbwtEngine.RenderHTML("Column");
                objConnectionDB.CloseConnection();
                string sessionname = "FamilyProduct_" + pid;
                 DataSet dsrecords=null;
                 //if (HttpContext.Current.Session[sessionname] != null)
                 //{
                 //    dsrecords = (DataSet)HttpContext.Current.Session[sessionname];
                 //}
                 //else if (HttpContext.Current.Session["FamilyProduct"] != null)
                 //{
                 //    dsrecords = (DataSet)HttpContext.Current.Session["FamilyProduct"];
                 //}
                 //else
                 //{
                 //    Response.Redirect("home.aspx"); 
                 //}

                 if (DsProductDetails != null)
                 {
                    // objErrorHandler.CreateLog("Inside DsProductDetails");
                     dsrecords = DsProductDetails;
                 }
                 else
                 {
                    // objErrorHandler.CreateLog("Inside easyask");
                     dsrecords = EasyAsk.GetAttributeProducts("ProductPage", "", "ProductId", pid, "", "0", "0", "",EA);
                    // dsrecords = (DataSet)HttpContext.Current.Session["FamilyProduct"];
                 }
                    StringTemplate _stmpl_records = null;
                StringTemplateGroup _stg_records = null;
                StringTemplateGroup _stg_container = null;
                StringTemplate _stmpl_container = null;

                _stg_records = new StringTemplateGroup(cellname, HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()));
                _stmpl_records = _stg_records.GetInstanceOf("PRODUCT_PRINT" + "\\" + cellname);
                DataRow[] cellrow = dsrecords.Tables[0].Select("ATTRIBUTE_ID = 1");
                TBWDataList[] lstrecords = new TBWDataList[0];
                lstrecords = new TBWDataList[cellrow.Length];
                int ictrecords = 0, ictcol = 1;

                foreach (DataRow cdr in cellrow)
                {
                    _stmpl_records.SetAttribute("TBT_YOURCOST", GetMyPrice(System.Convert.ToInt32(cdr["PRODUCT_ID"])));


                    _stmpl_records.SetAttribute("TBT_YOURCOST", GetMyPrice(System.Convert.ToInt32(cdr["PRODUCT_ID"])));

                    if (cellname == "cell")
                    {
                        GetMultipleImages(System.Convert.ToInt32(cdr["PRODUCT_ID"]), System.Convert.ToInt32(paraFID), _stmpl_records);
                    }
                    else
                    {
                        GetMultipleImages_pdf(System.Convert.ToInt32(cdr["PRODUCT_ID"]), System.Convert.ToInt32(paraFID), _stmpl_records);
                    }
                    if (fid != null)
                    {
                        string _fid = fid;

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


                    string _Package = "PRODUCT_PRINT";
                    foreach (DataColumn dc in dsrecords.Tables[0].Columns)
                    {
                        //_stmpl_records.SetAttribute(dc.ColumnName, dr[dc.ColumnName].ToString());                                                       
                        if (dc.ColumnName.ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper() != "FAMILY_NAME")
                        {
                            _stmpl_records.SetAttribute("TBT_" + dc.ColumnName.ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), cdr[dc.ColumnName.ToString()].ToString());
                        }
                        else
                        {

                            _stmpl_records.SetAttribute("TBT_" + dc.ColumnName.ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), cdr[dc.ColumnName.ToString()].ToString());
                            if(cellname == "cell_pdf")
                            {
                                hfFN.Value = cdr[dc.ColumnName.ToString()].ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;");
                            }

                        }

                    }
                    //_stmpl_records.SetAttribute("TBT_FAMILY_ID", paraFID);
                    string descall = string.Empty;
                    string desc1 = string.Empty;
                    string descallstring = string.Empty;
                    string attName = string.Empty;
                   // int setatttr = 0;
                    foreach (DataRow dr in dsrecords.Tables[0].Select("PRODUCT_ID = " + cdr["PRODUCT_ID"].ToString()))
                    {
                        //_stmpl_records.SetAttribute(dc.ColumnName, dr[dc.ColumnName].ToString());  
                        desc1 = "";
                        //When there is no product image then refer family image
                        try
                        {
                            string strFile = HttpContext.Current.Server.MapPath("ProdImages");
                            //end
                            if (dr["ATTRIBUTE_TYPE"].ToString() == "1" && dr["ATTRIBUTE_NAME"].ToString().ToUpper() == "CODE")
                            {
                                Productcode = dr["STRING_VALUE"].ToString();
                            }
                            if (dr["ATTRIBUTE_DATATYPE"].ToString().ToUpper().StartsWith("TEX"))
                            {
                                if (dr["ATTRIBUTE_TYPE"].ToString() == "3" || dr["ATTRIBUTE_TYPE"].ToString() == "9")
                                {
                                    FileInfo Fil;
                                    if (_Package == "PRODUCT_PRINT")
                                    {
                                        Fil = new FileInfo(strFile + dr["STRING_VALUE"].ToString().Replace("_th", "_Images_200"));
                                    }
                                    else
                                    {
                                        Fil = new FileInfo(strFile + dr["STRING_VALUE"].ToString());
                                    }
                                    if (Fil.Exists)
                                    {
                                        
                                        if(cellname =="cell_pdf")
                                        {
                                            _stmpl_records.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), Server.MapPath("/prodimages"+dr["STRING_VALUE"].ToString().Replace("_th", "_Images_200").Replace("/","\\")));
                                        }
                                        else{
                                         _stmpl_records.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"].ToString().Replace("_th", "_Images_200"));
                                        }
                                    }
                                    else
                                    {
                                        _stmpl_records.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(),  Server.MapPath("/prodimages/noimage.gif"));
                                    }
                                }

                                else
                                {
                                    attName = dr["ATTRIBUTE_NAME"].ToString().ToUpper();
                                    if (_Package.ToString() == "PRODUCT_PRINT")
                                    {
                                        if (attName == "DESCRIPTIONS" || attName == "FEATURES" || attName == "SPECIFICATION" || attName == "SPECIFICATIONS" || attName == "APPLICATIONS" || attName == "NOTES")
                                        {

                                            desc1 = dr["STRING_VALUE"].ToString().Replace("*", "").Replace("\n", "<br/>").Replace("\r", "&nbsp;");
                                        }
                                        else
                                            _stmpl_records.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"].ToString());
                                    }
                                    else
                                        _stmpl_records.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"].ToString());
                                }

                            }

                            if (_Package.ToString() == "PRODUCT_PRINT")
                            {
                                if (desc1 != "")
                                    descall = descall + desc1 + "<br/><br/>";
                            }





                        }
                        catch
                        {
                            
                        }
                    }
                    // if (dr["ATTRIBUTE_NAME"].ToString().ToLower() == " SHORT_DESCRIPTION" || dr["ATTRIBUTE_NAME"].ToString() == "Descriptions" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "FEATURES" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATION" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "APPLICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "NOTES")

                    if (descall.Length > 400)
                        _stmpl_records.SetAttribute("TBT_MORE_SHOW", true);
                    else
                        _stmpl_records.SetAttribute("TBT_MORE_SHOW", false);

                 //   _stmpl_records.SetAttribute("TBT_DESCALL", descall);

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

                    //bool descflag = false;
                   // int familyrows = 0;
                    DataSet dsfamily = new DataSet();




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






                    string prodValues = string.Empty;
                    /*
                    foreach (DataRow dr in dsrecords.Tables[0].Select("ATTRIBUTE_TYPE <> 3 AND ATTRIBUTE_ID <> 1 and  PRODUCT_ID = " + cdr["PRODUCT_ID"].ToString(), "ATTRIBUTE_NAME"))
                    {
                        if (dr["attribute_name"].ToString().ToUpper() != "SUIT" && dr["attribute_name"].ToString().ToUpper() != "BRAND")
                            if (dr["ATTRIBUTE_DATATYPE"].ToString().ToUpper().StartsWith("TEX"))
                            {
                                if (dr["STRING_VALUE"] != null && dr["STRING_VALUE"].ToString() != "" && dr["ATTRIBUTE_NAME"].ToString().ToUpper() != "NEW PRODUCTS")
                                    prodValues = prodValues + "<TR><TD bgcolor=\"white\" style='border-left: 1px solid #c8c8c8;' align=\"center\" style=\"border-color:Black;\" width=\"250px\"><strong>" + dr["attribute_name"] + ": </strong></TD><TD bgcolor=\"white\" style='border-left: 1px solid #c8c8c8;' align=\"center\" style=\"border-color:Black;\" width=\"250px\">" + dr["STRING_VALUE"] + "</TD></TR>";

                            }
                    }*/
                    string _sPriceTable = string.Empty;
                    string _StockStatus = string.Empty;
                    string _Eta = string.Empty;
                    string _Prod_Stock_Status = "0";
                    string _Prod_Stock_Flag = "0";
                    bool isProductReplace = true;
                    string strReplacedProduct = "";
                    string CustomerType = "";
                    UserServices objUserServices = new UserServices();
                    if (_Package == "PRODUCT_PRINT")
                    {
                        DataSet dsPriceTable1 = new DataSet();

                        string userid = "0";

                        _StockStatus = cdr["STOCK_STATUS_DESC"].ToString().Replace("_", " ");
                        _Prod_Stock_Status = cdr["PROD_STOCK_STATUS"].ToString();
                        _Prod_Stock_Flag = cdr["PROD_STOCK_FLAG"].ToString();
                        if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
                            userid = HttpContext.Current.Session["USER_ID"].ToString();
                        CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(userid));



                        _StockStatus = _StockStatus.Trim().Replace("_", " ");


                        //if ((_StockStatus.ToUpper().Contains("OUT OF STOCK ITEM WILL BE BACK ORDERED") || _StockStatus.ToUpper().Contains("SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED"))) //&& CustomerType.ToLower() == "dealer"
                        //    isProductReplace = false;
                        //else
                        //{
                        //    if (_Prod_Stock_Status.ToLower() == "true" || _Prod_Stock_Status.ToLower() == "1")
                        //        isProductReplace = false;
                        //    else if (_Prod_Stock_Flag == "0")
                        //        isProductReplace = false;
                        //}

                        if (_Prod_Stock_Flag == "0")
                            isProductReplace = false;

                        if (isProductReplace == true)
                        {
                            strReplacedProduct = GetProductReplacementDetails_pdf(_stmpl_records, cdr["STRING_VALUE"].ToString(), Convert.ToInt32(userid));

                        }
                        else
                        {
                         //   _sPriceTable = GetProductPriceTable(Convert.ToInt32(cdr["PRODUCT_ID"]));
                            if (WithPrice == "true")
                            {
                                _sPriceTable = GetProductPriceTable_print(Convert.ToInt32(cdr["PRODUCT_ID"]));

                            }

                            if (cdr["ETA"].ToString() != "")
                            {
                                //_Eta = string.Format("<tr><td><b>ETA</b></td><td colspan=\"2\"><b>" + cdr["ETA"].ToString() + "</b></td></tr>");
                        
                                    _stmpl_records.SetAttribute("TBT_ETA_PRINT", true);
                                    _stmpl_records.SetAttribute("TBT_STOCK_STATUS_ETA", cdr["ETA"].ToString());                           

                            }
                            else
                                _stmpl_records.SetAttribute("TBT_ETA_PRINT", false);
                        }









                    }
                    else
                    {
                        _sPriceTable = GetProductPriceTable_print(Convert.ToInt32(cdr["PRODUCT_ID"]));
                        _StockStatus = GetStockStatus(Convert.ToInt32(cdr["PRODUCT_ID"]));
                    }

                    //if (WithPrice == "true")
                    //{
                    //    _sPriceTable = GetProductPriceTable_print(Convert.ToInt32(cdr["PRODUCT_ID"]));

                    //}

                    //if (cdr["ETA"].ToString() != "")
                    //{
                    //    _Eta = cdr["ETA"].ToString();
                    //}
                    //_StockStatus = cdr["STOCK_STATUS_DESC"].ToString().Replace("_", " ");


                    //if (_Eta != "")
                    //{
                    //    _stmpl_records.SetAttribute("TBT_ETA_PRINT", true);
                    //    _stmpl_records.SetAttribute("TBT_STOCK_STATUS_ETA", _Eta);
                    //}
                    //else
                    //    _stmpl_records.SetAttribute("TBT_ETA_PRINT", false);

                
                  
                 //   _StockStatus = cdr["STOCK_STATUS_DESC"].ToString().Replace("_", " ");

                    string _Tbt_Stock_Status = string.Empty;
                    string _Tbt_Stock_Status_1 = string.Empty;
                    bool _Tbt_Stock_Status_2 = false;
                    string _Tbt_Stock_Status_3 = string.Empty;
                    string _Colorcode1 = string.Empty;
                    string _Colorcode;
                    string _StockStatusTrim = _StockStatus.Trim();

                   // objErrorHandler.CreateLog(_StockStatusTrim);

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
                          //  _Tbt_Stock_Status_2 = true;
                            //modified by indu Requirement Stock Status update date 22-Apr-2017
                            _Tbt_Stock_Status_2 = false;
                            //_Tbt_Stock_Status = "<span style=\"color:" + _Colorcode + "\">TEMPORARY UNAVAILABLE NO ETA</span>";
                            _Tbt_Stock_Status_3 = "TEMPORARY UNAVAILABLE NO ETA";
                            break;
                        case "TEMPORARY UNAVAILABLE NO ETA":
                            _Colorcode = "#F9A023";
                           // _Tbt_Stock_Status_2 = true;
                            //modified by indu Requirement Stock Status update date 22-Apr-2017
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
                        default:
                            _Colorcode = "Black";
                            _Tbt_Stock_Status = _StockStatusTrim;
                            break;
                    }




                    if (isProductReplace == true)
                    {
                        _stmpl_records.SetAttribute("TBT_REPLACED", true);
                        // _stmpl_records.SetAttribute("TBT_REPLACED_DETAIL", strReplacedProduct);
                    }
                    else
                    {
                        _stmpl_records.SetAttribute("TBT_REPLACED", false);
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
                       // _stmpl_records.SetAttribute("TBT_STOCK_STATUS_ETA", _Eta);
                        //_stmpl_records.SetAttribute("TBT_STOCK_STATUS", GetStockStatus(Convert.ToInt32(cdr["PRODUCT_ID"])));
                        _stmpl_records.SetAttribute("TBT_PRICE_TABLE", _sPriceTable);
                        _stmpl_records.SetAttribute("TBT_PDISPLAY_MODE", _sPriceTable == "" ? "none" : "");

                        if ((_StockStatusTrim == "DISCONTINUED NO LONGER AVAILABLE") || (_StockStatusTrim == "TEMPORARY UNAVAILABLE NO ETA") ||  (_StockStatusTrim == "TEMPORARY_UNAVAILABLE NO ETA"))
                        {
                           // objErrorHandler.CreateLog(_StockStatusTrim + "inside TBT_HIDE_BUY" + Convert.ToInt32(cdr["PRODUCT_ID"]));
                            _stmpl_records.SetAttribute("TBT_HIDE_BUY", true);
                        }
                        else
                        {
                            _stmpl_records.SetAttribute("TBT_HIDE_BUY", false);
                        }

                    }


             
                    _stmpl_records.SetAttribute("TBT_COLOR_CODE", _Colorcode);
                    /* comment
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
                    _stmpl_records.SetAttribute("TBT_PRICE_TABLE", _sPriceTable);
                    _stmpl_records.SetAttribute("TBT_PDISPLAY_MODE", _sPriceTable == "" ? "none" : "");*/
                   // if (prodValues != "")
                   // {
                       
                            GetProductDetails(Convert.ToInt32(cdr["PRODUCT_ID"]), Convert.ToInt32(paraFID), _stmpl_records);
                           
                      
                   //     _stmpl_records.SetAttribute("TBT_ALL_PRODUCTVALUES", prodValues);
                   //     _stmpl_records.SetAttribute("TBT_DISPLAY_MODE", "");
                   // }

                            _stmpl_records.SetAttribute("Imagepath_icon", HttpContext.Current.Server.MapPath("/images/subproduct_icon.jpg"));
                    if (WithPrice == "true")
                    {
                        _stmpl_records.SetAttribute("TBT_WITHPRICE", true);

                    }
                    else
                    {
                        _stmpl_records.SetAttribute("TBT_WITHPRICE", false);
                    }
                    //if (Details == "true")
                    //{
                    //    _stmpl_records.SetAttribute("TBT_WITHDETAILS", true);
                        
                    //}
                    //else
                    //{
                    //    _stmpl_records.SetAttribute("TBT_WITHDETAILS", false);
                    //}


                    strValue = strValue + _stmpl_records.ToString();

                }
                return strValue;
            }



            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return string.Empty;
            }
        }
        private string GetProductReplacementDetails_pdf(StringTemplate _stmpl_records, string _CODE, int user_id)
        {
            string _catid = "", pfid = "", Ea_Path = "", wag_product_code = "", SubstuyutePid = "";
            string _sPriceTable = "";
            bool samecodesubproduct = false;
            bool samecodenotFound = false;
            ProductServices objProductServices = new ProductServices();
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
            if (samecodenotFound == false && samecodesubproduct == true)
            {
                _stmpl_records.SetAttribute("TBT_NIL_REPLACED", true);
                _stmpl_records.SetAttribute("TBT_REP_NIL_CODE", _CODE);
                _stmpl_records.SetAttribute("TBT_REP_STATUS", "Product Temporarily Unavailable <br>Please Contact Us for more details");
                //_sPriceTable += string.Format("<tr><td style=\"color:red;\"width=\"28%\">{0}</td>", _CODE);
                // _sPriceTable += string.Format("<td style=\"color:red;\" colspan=\"2\">{0}</td></tr>", "Product Temporarily Unavailable <br>Please Contact Us for more details");
            }
            else if (samecodenotFound == false && samecodesubproduct == false)
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

            }
            else
            {
                _stmpl_records.SetAttribute("TBT_NIL_REPLACED", true);
                _stmpl_records.SetAttribute("TBT_REP_NIL_CODE", _CODE);
                _stmpl_records.SetAttribute("TBT_REP_STATUS", "Product Temporarily Unavailable <br>Please Contact Us for more details");
                //_sPriceTable += string.Format("<tr><td style=\"color:red;\"width=\"28%\">{0}</td>", _CODE);
                //_sPriceTable += string.Format("<td style=\"color:red;\" colspan=\"2\">{0}</td></tr>", "Product Temporarily Unavailable <br>Please Contact Us for more details");
            }

            return _sPriceTable;

        }
        private void GetMultipleImages_pdf(int ProductID, int FamilyID, StringTemplate st)
        {
            ConnectionDB objConnectionDB = new ConnectionDB();
            HelperServices objHelperService = new HelperServices();
            string strfile = HttpContext.Current.Server.MapPath("ProdImages");
            //SqlConnection oCon = new SqlConnection(_DBConnectionString);
            SqlDataAdapter oDa = new SqlDataAdapter("GetProductImages", objConnectionDB.GetConnection());
            oDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            oDa.SelectCommand.Parameters.Clear();
            oDa.SelectCommand.Parameters.AddWithValue("@ProductID", ProductID);
            oDa.SelectCommand.Parameters.AddWithValue("@FamilyID", FamilyID);
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
                             oPrd.LargeImage =  oDr["STRING_VALUE"].ToString().Replace("\\", "/");

                           // oPrd.LargeImage = oDr["STRING_VALUE"].ToString().Replace("\\", "/");

                            if ((oPrd.LargeImage.ToLower().Contains("_th")))
                            {
                                string tmpimg = oDr["STRING_VALUE"].ToString().Replace("\\", "/");
                                oPrd.LargeImage = objHelperService.SetImageFolderPath(tmpimg, "_th", "_images");
                                oPrd.Thumpnail = objHelperService.SetImageFolderPath(tmpimg, "_th", "_th50");
                                oPrd.SmallImage = objHelperService.SetImageFolderPath(tmpimg, "_th", "_th");
                                // oPrd.MediumImage = objHelperService.SetImageFolderPath(tmpimg, "_th", "_images_200");
                            }
                            else
                            {
                                oPrd.Thumpnail = objHelperService.SetImageFolderPath(oPrd.LargeImage.ToLower(), "_Images", "_th50");
                                oPrd.Thumpnail = strfile+oPrd.Thumpnail;// e:/catalogstudio/wes_2_5/wes/prodimages/section21_th50/bwm68.jpg"
                                oPrd.SmallImage = objHelperService.SetImageFolderPath(oPrd.LargeImage.ToLower(), "_Images", "_th");
                                // oPrd.MediumImage = objHelperService.SetImageFolderPath(oPrd.LargeImage.ToLower(), "_Images", "_images_200");
                            }

                            st.SetAttribute("TBT_MULTIIMAGES", oPrd);
                            if (firstImg)
                            {
                                // st.SetAttribute("TBT_TWEB_IMAGE1", oPrd.MediumImage);
                                firstImg = false;
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

        private void GetMultipleImages(int ProductID, int FamilyID, StringTemplate st)
        {
            ConnectionDB objConnectionDB = new ConnectionDB();
            HelperServices objHelperService = new HelperServices();
            string strfile = HttpContext.Current.Server.MapPath("ProdImages");
            //SqlConnection oCon = new SqlConnection(_DBConnectionString);
            SqlDataAdapter oDa = new SqlDataAdapter("GetProductImages", objConnectionDB.GetConnection());
            oDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            oDa.SelectCommand.Parameters.Clear();
            oDa.SelectCommand.Parameters.AddWithValue("@ProductID", ProductID);
            oDa.SelectCommand.Parameters.AddWithValue("@FamilyID", FamilyID);
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
                           // oPrd.LargeImage = strfile + oDr["STRING_VALUE"].ToString().Replace("\\", "/");

                            oPrd.LargeImage = oDr["STRING_VALUE"].ToString().Replace("\\", "/");

                            if ((oPrd.LargeImage.ToLower().Contains("_th")))
                            {
                                string tmpimg = oDr["STRING_VALUE"].ToString().Replace("\\", "/");
                                oPrd.LargeImage = objHelperService.SetImageFolderPath(tmpimg, "_th", "_images");
                                oPrd.Thumpnail = objHelperService.SetImageFolderPath(tmpimg, "_th", "_th50");
                                oPrd.SmallImage = objHelperService.SetImageFolderPath(tmpimg, "_th", "_th");
                               // oPrd.MediumImage = objHelperService.SetImageFolderPath(tmpimg, "_th", "_images_200");
                            }
                            else
                            {
                                oPrd.Thumpnail = objHelperService.SetImageFolderPath(oPrd.LargeImage.ToLower(), "_Images", "_th50");
                                oPrd.SmallImage = objHelperService.SetImageFolderPath(oPrd.LargeImage.ToLower(), "_Images", "_th");
                               // oPrd.MediumImage = objHelperService.SetImageFolderPath(oPrd.LargeImage.ToLower(), "_Images", "_images_200");
                            }

                            st.SetAttribute("TBT_MULTIIMAGES", oPrd);
                            if (firstImg)
                            {
                               // st.SetAttribute("TBT_TWEB_IMAGE1", oPrd.MediumImage);
                                firstImg = false;
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


        private string GetStockStatus(int ProductID)
        {
            string Retval = "NO STATUS AVAILABLE";
            try
            {               
                HelperDB objHelperDB = new HelperDB();
                DataTable objrbl = (DataTable)objHelperDB.GetGenericDataDB(ProductID.ToString(), "GET_SINGLE_PRODUCT_INVENTORY", HelperDB.ReturnType.RTTable);
                if (objrbl != null)
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

        private string GetProductPriceTable_print(int ProductID)
        {

            string _sPriceTable = string.Empty;
            try
            {
                string userid = HttpContext.Current.Session["USER_ID"].ToString();
                if (!string.IsNullOrEmpty(userid))
                {

                    HelperDB objHelperDB = new HelperDB();
                    int pricecode = objHelperDB.GetPriceCode(userid);
                    DataSet dsPriceTable = new DataSet();

                    dsPriceTable = objHelperDB.GetProductPriceTable(ProductID, Convert.ToInt32(userid.ToString()));
                    _sPriceTable = "";

                    int TotalCount = 0;
                    int RowCount = 0;

                    if (pricecode == 3)
                        foreach (DataRow oDr in dsPriceTable.Tables["Price"].Rows)
                        {
                            _sPriceTable += string.Format("<tr style='border-left: 1px solid #c8c8c8;'><td style='border-bottom: 1px solid #c8c8c8;border-left: 1px solid #c8c8c8;font-size:12px;color: #666666;font-family: Arial;' align='center' height='27px'>{0}</td><td height='27px' style='border-left: 1px solid #c8c8c8;border-bottom: 1px solid #c8c8c8;font-size:12px;color: #666666;font-family: Arial;' align='center'>${1:0.00}</td><td style='border-left: 1px solid #c8c8c8;border-bottom: 1px solid #c8c8c8;font-size:12px;color: #666666;font-family: Arial;' height='27px' align='center'>${2:0.00}</td></tr>", oDr["QTY"].ToString(), oDr["Price1"].ToString(), oDr["Price2"].ToString());

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

                            string _color = bLastRow ? "background-color: #D3F890;font-weight:bold;" : "font-weight:normal;";


                            _sPriceTable += string.Format("<tr style='border-left: 1px solid #c8c8c8;border-bottom: 1px solid #c8c8c8;'><td height='27px' style='border-left: 1px solid #c8c8c8;border-bottom: 1px solid #c8c8c8;font-size:12px;color: #666666;font-family: Arial;' align='center'>{0}</td><td height='27px' style='border-left: 1px solid #c8c8c8;border-bottom: 1px solid #c8c8c8;font-size:12px;color: #666666;font-family: Arial;' align='center'>${1:0.00}</td><td height='27px' style='border-left: 1px solid #c8c8c8;font-size:12px;color: #666666;font-family: Arial;' align='center'>${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);

                        }
                    }

                }
            }


            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                _sPriceTable = "-1";
            }
            return _sPriceTable;
        }
        private string GetProductPriceTable(int ProductID)
        {

            string _sPriceTable = string.Empty;
            try
            {
                string userid = HttpContext.Current.Session["USER_ID"].ToString();
                if (!string.IsNullOrEmpty(userid))
                {
                   
                    HelperDB objHelperDB = new HelperDB();
                    int pricecode = objHelperDB.GetPriceCode(userid);
                    DataSet dsPriceTable = new DataSet();
                   
                    dsPriceTable = objHelperDB.GetProductPriceTable(ProductID, Convert.ToInt32(userid.ToString()));
                    _sPriceTable = "";

                    int TotalCount = 0;
                    int RowCount = 0;

                    if (pricecode == 3)
                        foreach (DataRow oDr in dsPriceTable.Tables["Price"].Rows)
                        {   
                                _sPriceTable += string.Format("<tr><td>{0}</td><td>${1:0.00}</td><td>${2:0.00}</td></tr>", oDr["QTY"].ToString(), oDr["Price1"].ToString(), oDr["Price2"].ToString());
                            
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
                         
                            
                                _sPriceTable += string.Format("<tr><td>{0}</td><td>${1:0.00}</td><td>${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                            
                           }
                    }

                }
            }


            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                _sPriceTable = "-1";
            }
            return _sPriceTable;
        }
        private decimal GetMyPrice(int ProductID)
        {
            decimal retval = 0.00M;
            try
            {
                string userid = HttpContext.Current.Session["USER_ID"].ToString();
               
                HelperDB objHelperDB = new HelperDB();
                retval = objHelperDB.GetProductPrice(ProductID, 1, userid);
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();

            }
            return retval;
        }

        public class ProductDetails
        {
            public string AttributeName { get; set; }
            public string SpecValue { get; set; }
            public int AttributeID { get; set; }
            public int SortOrder { get; set; }
        }

        private string ST_Email_PDF(string WithPrice, string Details, string cellname, string emailid, string notes, string pid, string fid, string cid,string path)
        {
            string result = string.Empty;
            string str = Construct_ST(WithPrice, Details, cellname, pid, fid, cid,path);
            //Assign Html content in a string to write in PDF 
            string strContent = "<html><body style='-webkit-print-color-adjust:exact;'>" + str + "</body></html>";
           // Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);

            string user_id = string.Empty;
            if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
            {
                user_id = HttpContext.Current.Session["USER_ID"].ToString();
            }

            try
            {
                var pechkin = Factory.Create(new GlobalConfig());
                var pdf = pechkin.Convert(new ObjectConfig()
                                        .SetLoadImages(true).SetZoomFactor(1.5)
                                        .SetPrintBackground(true)
                                        .SetScreenMediaType(true)
                                        .SetCreateExternalLinks(true), strContent);


                string familyname = string.Empty;
                if (hfFN.Value != "")
                {
                    familyname = hfFN.Value;
                    familyname = familyname.Replace(" ", "-").Replace("  ", "-").Replace("\\", "_").Replace("/", "_").Replace(":", "_").Replace("\"", "_").Replace("?", "_").Replace("<", "_").Replace(">", "_").Replace("|", "_").ToString();
                }
                else
                    familyname = fid;

                string titlecase = string.Empty;
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                titlecase = textInfo.ToTitleCase(familyname.ToLower());

                string new_filname = string.Empty;
                new_filname = titlecase;

                string product_code = Productcode;
                product_code = textInfo.ToTitleCase(product_code.ToLower());
                product_code = product_code.Replace(" ", "-").Replace("  ", "-").Replace("\\", "_").Replace("/", "_").Replace(":", "_").Replace("\"", "_").Replace("?", "_").Replace("<", "_").Replace(">", "_").Replace("|", "_").ToString();

                if (File.Exists(Server.MapPath("EmailPdfFiles/" + product_code + "-" + new_filname + "_" + user_id + ".pdf")))
                {
                    File.Delete(Server.MapPath("EmailPdfFiles/" + product_code + "-" + new_filname + "_" + user_id + ".pdf"));
                    using (FileStream file = System.IO.File.Create(Server.MapPath("EmailPdfFiles/" + product_code + "-" + new_filname + "_" + user_id + ".pdf")))
                    {
                        file.Write(pdf, 0, pdf.Length);
                    }
                }
                else
                {
                    using (FileStream file = System.IO.File.Create(Server.MapPath("EmailPdfFiles/" + product_code + "-" + new_filname + "_" + user_id + ".pdf")))
                    {
                        file.Write(pdf, 0, pdf.Length);
                    }
                }

                //using (FileStream file = System.IO.File.Create(Server.MapPath("EmailPdfFiles/" + product_code + "-" + new_filname +"_"+ user_id+ ".pdf")))
                //{
                //    file.Write(pdf, 0, pdf.Length);
                //}

                if (!File.Exists(Server.MapPath("EmailPdfFiles/" + product_code + "-" + new_filname + ".pdf")))
                     File.Copy(Server.MapPath("EmailPdfFiles/" + product_code + "-" + new_filname + "_" + user_id + ".pdf"), Server.MapPath("EmailPdfFiles/" + product_code + "-" + new_filname + ".pdf"));
                
                result = objHelperServices.sendmail("Product Information." + " " + product_code+"-" + new_filname, notes, emailid, Server.MapPath("EmailPdfFiles/" + product_code + "-" + new_filname + ".pdf"));

                if (File.Exists(Server.MapPath("EmailPdfFiles/" + product_code + "-" + new_filname + ".pdf")))
                    File.Delete(Server.MapPath("EmailPdfFiles/" + product_code + "-" + new_filname + ".pdf"));
                if (File.Exists(Server.MapPath("EmailPdfFiles/" + product_code + "-" + new_filname + "_" + user_id + ".pdf")))
                    File.Delete(Server.MapPath("EmailPdfFiles/" + product_code + "-" + new_filname + "_" + user_id + ".pdf"));

                string pageurl = "ORGURL_";
                pageurl = pageurl + hfpid.Value;
                if (Session[pageurl] != null)
                {
                    Response.Redirect(Session[pageurl].ToString());
                }
                else
                {
                    Response.Redirect("home.aspx");
                }
            }
            catch (Exception ex)
            {
                objErrorHandler.CreateLog(ex.ToString());
            }

            return result;
           
        }
        

        //protected void btnpdf_Click(object sender, EventArgs e)
        //{

        //    string WithPrice = "false";
        //    string Details = "false";

        //    if (hfpricepdf.Value == "true")
        //    {
        //        WithPrice = "true";
        //    }
        //    if (hfdetailspdf.Value == "true")
        //    {
        //        Details = "true";
        //    }
        //    string str = Construct_ST(WithPrice, Details, "cell_pdf",);
        //    //Assign Html content in a string to write in PDF 
        //    string strContent = "<html><body>" + str + "</body></html>";

        //    exportpdf(strContent);

        //}

        public void ST_PDF(string WithPrice, string Details, string cellname,string pid,string fid,string cid,string path)
        {


            try
            {
                string str = Construct_ST(WithPrice, Details, "cell_pdf", pid, fid, cid,path);
                //Assign Html content in a string to write in PDF 
                string strContent = "<html><body>" + str + "</body></html>";

                exportpdf(strContent);
                string pageurl = "ORGURL_";
                pageurl = pageurl + hfpid.Value;
                if (Session[pageurl] != null)
                {
                    Response.Redirect(Session[pageurl].ToString());
                }
                else
                {
                    Response.Redirect("home.aspx");
                }
            }
            catch(Exception ex)
            { 
                objErrorHandler.CreateLog(ex.ToString());
                
            }
        }
        private void exportpdf(string strcontent)
        {

            string user_id = string.Empty;
            try
            {
                var pechkin = Factory.Create(new GlobalConfig());
                var pdf = pechkin.Convert(new ObjectConfig()
                                        .SetLoadImages(true).SetZoomFactor(1.5)
                                        .SetPrintBackground(true)
                                        .SetScreenMediaType(true)
                                        .SetCreateExternalLinks(true), strcontent);
                string fid = Productcode;


                string familyname = string.Empty;
                if (hfFN.Value != "")
                {
                    familyname = hfFN.Value;
                    familyname = familyname.Replace(" ", "-").Replace("  ", "-").ToString().Replace("\"", "_").ToString();
                }
                else
                    familyname = fid;

                string titlecase = string.Empty;
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                titlecase = textInfo.ToTitleCase(familyname.ToLower());

                string new_filname = string.Empty;
                new_filname = titlecase;

                string p_code = Productcode;
                p_code = textInfo.ToTitleCase(Productcode.ToLower());
                p_code = p_code.Replace(" ", "-").Replace("  ", "-").Replace("\"", "_").ToString();
                if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
                {
                    user_id = HttpContext.Current.Session["USER_ID"].ToString();
                }

                Response.Clear();

                Response.ClearContent();
                Response.ClearHeaders();

                Response.ContentType = "application/pdf";
                //  Response.AddHeader("Content-Disposition", string.Format("attachment;filename="+ "Product_" + fid + "_" + user_id + ".pdf; size={0}", pdf.Length));
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename= " + p_code + "-" + new_filname + ".pdf; size={0}", pdf.Length));
                Response.BinaryWrite(pdf);

                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                objErrorHandler.CreateLog(ex.ToString());
            }


            //Document pdfDoc = new Document();

            //try
            //{
            //    PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);

            //    //Open PDF Document to write data 
            //    pdfDoc.Open();
               
            //    //string strContent = "<table border=1> <tr><td width='28%'>ORDER CODE</td><td colspan='2'>STOCK STATUS</td></tr><tr><td>CA600</td><td> INSTOCK</td></tr><tr> <td>QTY</td><td width='38%'>Cost INC GST</td><td width='34%'>Cost EX GST</td></tr><tr><tr><td>1</td><td>$5.25</td><td>$4.77</td></tr><tr><td>10</td><td>$4.72</td><td>$4.29</td></tr><tr><td>RRP</td><td>$9.95</td><td>$9.05</td></tr><tr><td>Your Price</td><td>$4.72</td><td>$4.29</td></tr></tr></table>";


            //     //Read string contents using stream reader and convert html to parsed conent 
            //    System.IO.StringReader x = new System.IO.StringReader(strcontent);
            //    var parsedHtmlElements = HTMLWorker.ParseToList(x,null);
               
            //    //Get each array values from parsed elements and add to the PDF document 
            //    foreach (var htmlElement in parsedHtmlElements)
            //        pdfDoc.Add(htmlElement as IElement);

            //    //Close your PDF 
            //    pdfDoc.Close();

            //   HttpContext.Current.Response.ContentType = "application/pdf";
            //    string fid = Productcode;   
            //    //Set default file Name as current datetime 
            //    string filename="attachment;filename="+fid+".pdf";
            //    HttpContext.Current.Response.AddHeader("content-disposition", filename);
            //    System.Web.HttpContext.Current.Response.Write(pdfDoc);

            //    HttpContext.Current.Response.Flush();
            //    HttpContext.Current.Response.End();

            //}
            //catch (Exception ex)
            //{
            //    objErrorHandler.CreateLog(ex.ToString());   
            //    //Response.Write(ex.ToString());
            //}

 
        }
     


       
      
        public string Construct_ST(string WithPrice, string Details, string cellname, string pid, string fid, string cid,string path)
        {
            string str = string.Empty;
            //objErrorHandler.CreateLog("pid_fid_cid" + pid +"_"+ fid+"_"+cid);
            try
            {
                // TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("PRODUCT_PRINT", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
                StringTemplate _stmpl_records = null;
                StringTemplateGroup _stg_records = null;
                HelperServices objHelperServices = new HelperServices();
             //  breadcrumb=  objEasyAsk.GetBreadCrumb_print();

             //string bcreplace = ""; // HttpContext.Current.Server.MapPath("ProdImages");


             //if (cellname == "cell")
             //{
             //    bcreplace = bcreplace + "images/close11.png";
             //    bcreplace = "<img alt='' style='vertical-align:middle; margin-left:5px;margin-top:-5px;' height='12px' width='14px' src= '" + bcreplace + "' />" + "<span></span> " + "<span style='font-size:12px;'>/</span>" + "<span></span> ";
             //    breadcrumb = breadcrumb.Replace(">>1", ">").Replace(">1", bcreplace);
             //}
             //else
             //{
             //    bcreplace = HttpContext.Current.Server.MapPath("ProdImages");
             //    bcreplace = bcreplace+""+ "/images/close11.png";
             //    bcreplace = "<img alt='' style='vertical-align:middle; margin-left:5px;margin-top:-5px;' height='12px' width='14px' src= '" + bcreplace + "' />" +"<span></span> " +"<span style='font-size:11px;'>/</span>"+"<span></span> ";
             //    breadcrumb = breadcrumb.Replace(">>1", ">").Replace(">1", bcreplace);
             //}

                _stg_records = new StringTemplateGroup(cellname, HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()));
                if (cellname == "cell")
                {
                    _stmpl_records = _stg_records.GetInstanceOf("PRODUCT_PRINT" + "\\" + "header");
                }
                else
                {
                    _stmpl_records = _stg_records.GetInstanceOf("PRODUCT_PRINT" + "\\" + "header_pdf");
                }
               // _stmpl_records.SetAttribute("lblbreadcrum", breadcrumb);
                _stmpl_records.SetAttribute("closebtn", HttpContext.Current.Server.MapPath("/Images/close11.png"));
                _stmpl_records.SetAttribute("Imagepath", HttpContext.Current.Server.MapPath("/Images/WesLogo.jpg"));


                if (Details == "true" || Details == "True")
                    _stmpl_records.SetAttribute("TBT_DES_SHOW", true);
                else
                    _stmpl_records.SetAttribute("TBT_DES_SHOW", false);



                DataSet DsProductDetails = new DataSet();
                EasyAsk_WES EasyAsk = new EasyAsk_WES();
                DsProductDetails = null;
                Security objSecurity=new Security();
                string EA = objSecurity.StringDeCrypt(path);
                //objErrorHandler.CreateLog(EA);   
                 DsProductDetails = EasyAsk.GetAttributeProducts("ProductPage", "", "ProductId", pid, "", "0", "0", "", EA);
            int prddet=0;
                if(DsProductDetails!=null)
             {
             prddet=DsProductDetails.Tables[0].Rows.Count;  
             }
               // objErrorHandler.CreateLog(prddet.ToString());
                //DsProductDetails = (DataSet)HttpContext.Current.Session["FamilyProduct"]; 
                breadcrumb = objEasyAsk.GetBreadCrumb_print();
                string bcreplace = string.Empty;
                if (cellname == "cell")
                {
                    bcreplace = bcreplace + "images/close11.png";
                    bcreplace = "<img alt='' style='vertical-align:middle; margin-left:5px;margin-top:-2px;' height='12px' width='14px' src= '" + bcreplace + "' />" + "<span></span> " + "<span style='font-size:12px;'>/</span>" + "<span></span> ";
                    breadcrumb = breadcrumb.Replace(">>1", ">").Replace(">1", bcreplace);
                }
                else
                {
                    //bcreplace = HttpContext.Current.Server.MapPath("ProdImages");
                    //bcreplace = bcreplace + "" + "/images/close11.png";
                    //bcreplace = "<img alt='' style='vertical-align:middle; margin-left:5px;margin-top:-5px;' height='12px' width='14px' src= '" + bcreplace + "' />" + "<span></span> " + "<span style='font-size:11px;'>/</span>" + "<span></span> ";
                    //breadcrumb = breadcrumb.Replace(">>1", ">").Replace(">1", bcreplace);
                    breadcrumb = breadcrumb.Replace(">>", ">").Replace(">", " > ");
                }

                _stmpl_records.SetAttribute("lblbreadcrum", breadcrumb);
                string strValue = _stmpl_records.ToString();
              
                if (cellname == "cell")
                {
                    str = "<html><body style='-webkit-print-color-adjust:exact;'>" + strValue + "<div style='background-color: #FFFFFF;border: 1px solid #CCCCCC;border-radius: 6px;' class='divborder' width='100%'><table width=900  border=0 style='border-color:Gray' cellspacing=0 cellpadding=0><tr>" + ST_Product_print(WithPrice, Details, cellname, pid, fid, cid, DsProductDetails,EA) + "</tr></table></div></body><//html>";
                }
                else
                {
                    str = strValue + "<table width='900px'><tr>" + ST_Product_print(WithPrice, Details, cellname, pid, fid, cid, DsProductDetails,EA) + "</tr></table>";
                   // str = strValue + ST_Product_print(WithPrice, Details, cellname, pid, fid, cid);
                }
                return str;

            }

            catch (Exception ex)
            {
                objErrorHandler.CreateLog(ex.ToString());
                return str;
            }

        }
    }
   



