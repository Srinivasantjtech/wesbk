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
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
using System.IO;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
public partial class UC_CatalogueDownload : System.Web.UI.UserControl
{
    string stemplatepath = "";
    ErrorHandler objErrorHandler=new ErrorHandler();
    HelperDB objHelperDB = new HelperDB();
    HelperServices objHelperServices = new HelperServices();
    CategoryServices objCategoryServices = new CategoryServices();
    Security objSecurity = new Security();
    string WesNewsCategoryId = System.Configuration.ConfigurationManager.AppSettings["WESNEWS_CATEGORY_ID"].ToString();
    string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();
    string _Action = "";


    protected void Page_Load(object sender, EventArgs e)
    {
        stemplatepath = Server.MapPath("Templates");
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();

        if (Request.QueryString["ActionResult"] != null)
            _Action = Request.QueryString["ActionResult"].ToString();


        //DataSet tmpds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, WesNewsCategoryId, "GET_CATEGORY", HelperDB.ReturnType.RTDataSet);
        //string custom_num_f = null;
        //if (tmpds != null)
        //{
        //    custom_num_f = tmpds.Tables[0].Rows[0]["CUSTOM_NUM_FIELD3"].ToString();
        //}
    }
    public string ST_PDFDownload()
    {
        StringTemplateGroup _stg_container = null;
        StringTemplateGroup _stg_records = null;
        StringTemplateGroup _stg_pages = null;
        StringTemplate _stmpl_container = null;
        StringTemplate _stmpl_records = null;
        StringTemplate _stmpl_pages = null;
        TBWDataList[] lstrecords = new TBWDataList[0];
        string[] filenames = null;

        string shtml = "";
        int counter = 0;
        int cntoddeven = 0;
        DataRow[] dr=null;

        if (Directory.Exists(Server.MapPath("attachments")))
        {

            //string[] fileEntries = Directory.GetFiles(Server.MapPath("attachments"), "*.pdf");
            //lstrecords = new TBWDataList[fileEntries.Length];
            //filenames = new string[fileEntries.Length];
            //if (fileEntries.Length > 0)

            //DataSet dsPDFCount = new DataSet();
            //dsPDFCount = objCategoryServices.GetCatalogPDFCount(2);

            //if (dsPDFCount != null)
            //{
            //    foreach (DataRow rPDF in dsPDFCount.Tables[0].Rows)
            //    {
            //        lstrecords = new TBWDataList[Convert.ToInt32(rPDF["CountFiles"].ToString())];
            //    }
            //}

            //if (lstrecords.Length > 0)
            //{

                DataSet dsCatalog = new DataSet();
                try
                {
                    dsCatalog = objCategoryServices.GetCatalogPDFDownload(2);
                    if (dsCatalog != null)
                    {
                        if (_Action == "CATALOGUE")
                        {
                            dr = dsCatalog.Tables[0].Select("PARENT_CATEGORY<>'" + WesNewsCategoryId + "' And CUSTOM_NUM_FIELD1=1 And CUSTOM_NUM_FIELD2=0 ");
                      
                        }
                        else if (_Action == "NEWS")
                        {
                            dr = dsCatalog.Tables[0].Select("PARENT_CATEGORY='" + WesNewsCategoryId + "' And CUSTOM_NUM_FIELD1=1 And CUSTOM_NUM_FIELD2=0 ");
                            //And CUSTOM_NUM_FIELD1=1 And CUSTOM_NUM_FIELD2=0  And CUSTOM_NUM_FIELD1=1 And CUSTOM_NUM_FIELD2=0");
                        }

                        else if (_Action == "FORMS")
                        {
                            dr = dsCatalog.Tables[0].Select("CUSTOM_NUM_FIELD2=1 ");
                        }

                        if (dr.Length > 0)
                        {
                            lstrecords = new TBWDataList[dr.Length + 1];
                            dsCatalog=new DataSet();
                            if (_Action == "NEWS")
                            {
                                DataTable dtSortedTableold = dr.CopyToDataTable().Copy();


                                DataTable dtSortedTable = dtSortedTableold.AsEnumerable()
                                    .OrderByDescending(row => row.Field<DateTime>("PublishedDate"))
                                    .CopyToDataTable();
                                dsCatalog.Tables.Add(dtSortedTable);
                                //dsCatalog.Tables.Add(dr.CopyToDataTable().Copy());
                            }
                            else
                            {
                                DataTable dtSortedTableold = dr.CopyToDataTable().Copy();


                                DataTable dtSortedTable = dtSortedTableold.AsEnumerable()
                                    .OrderBy(row => row.Field<string>("IMAGE_NAME2"))
                                    .CopyToDataTable();

                                DataRow[] dr1 = dtSortedTable.Select("IMAGE_NAME2 <>''");
                                if (dr1.Length > 0)
                                {
                                    dsCatalog.Tables.Add(dr1.CopyToDataTable().Copy());
                                }

                                DataRow[] dr2 = dtSortedTable.Select("IMAGE_NAME2 =''");
                                if (dr2.Length > 0)
                                {
                                    dsCatalog.Tables[0].Merge(dr2.CopyToDataTable().Copy());
                                }
                                if (dr1.Length == 0 && dr2.Length == 0)
                                {
                                    dsCatalog.Tables.Add(dtSortedTable); 
                                }
                                }
                           // dsCatalog.Tables.Add(dr2.CopyToDataTable().Copy());
                            //dsCatalog.Tables.Add(dtSortedTable);     
                           // dsCatalog.Tables[0].DefaultView.Sort = "IMAGE_NAME2";
                          //  objHelperServices.writelog("b4 foorloop-catelogdw"); 
                            foreach (DataRow rCat in dsCatalog.Tables[0].Rows)
                            {
                                //objHelperServices.writelog("After for"); 
                                string Ebook_pdf_FileRef=System.Configuration.ConfigurationManager.AppSettings["Ebook_pdf_FileRef"].ToString();    
                                string MyFile = Server.MapPath(string.Format("attachments{0}", rCat["IMAGE_FILE2"].ToString()));
                                
                                string checkoldfiles=string.Empty;
                                if (MyFile.ToLower().Contains(Ebook_pdf_FileRef) == false)
                                {
                                    checkoldfiles = MyFile.Replace("\\media\\pdf\\", "\\media\\wes_secure_files\\pdf\\");
                                    MyFile = MyFile.Replace("\\media\\pdf\\", "\\media\\wes_secure_files\\pdf\\");
                               
                                }
                                else

                                {
                                   
                                checkoldfiles =MyFile;  
                                }
                                //objHelperServices.writelog("MyFile " + MyFile);
                               //
                                if (System.IO.File.Exists(MyFile) || rCat["IMAGE_NAME"].ToString() != "")
                                {
                                    //objHelperServices.writelog("Insite if"); 
                                    _stg_records = new StringTemplateGroup("cataloguedownload", stemplatepath);
                                    _stmpl_records = _stg_records.GetInstanceOf("cataloguedownload" + "\\" + "cell");

                                   


                                    cntoddeven++;
                                    if ((cntoddeven % 2) == 0)
                                    {
                                        //objHelperServices.writelog("Insite row1 "); 
                                        _stg_container = new StringTemplateGroup("cataloguedownload", stemplatepath);
                                        _stmpl_container = _stg_container.GetInstanceOf("cataloguedownload" + "\\" + "row1");
                                    }
                                    else
                                    {
                                        //objHelperServices.writelog("Insite row"); 
                                        _stg_container = new StringTemplateGroup("cataloguedownload", stemplatepath);
                                        _stmpl_container = _stg_container.GetInstanceOf("cataloguedownload" + "\\" + "row");
                                    }

                                    if (System.IO.File.Exists(MyFile))
                                    {
                                        //objHelperServices.writelog("inside myfile " );
                                        _stmpl_records.SetAttribute("TBT_PDF_DESCRIPTION", rCat["IMAGE_NAME2"].ToString());

                                        string[] file = rCat["IMAGE_FILE2"].ToString().Split(new string[] { @"/" }, StringSplitOptions.None);
                                       
                                        _stmpl_records.SetAttribute("PDF_FILE_NAME", file[file.Length-1].ToString());
                                        //Modified By :indu :Added replace function to remove  catelogdowload

                                       // string pdffilepath = rCat["IMAGE_FILE2"].ToString().Replace("\\", "/");

                                       // pdffilepath = objSecurity.StringEnCrypt(pdffilepath);
                                        _stmpl_records.SetAttribute("PDF", rCat["IMAGE_FILE2"].ToString().Replace("\\", "/").Replace("wes_secure_files/", "").Replace("wes_secure_files\\", ""));

                                      //  _stmpl_records.SetAttribute("PDF", pdffilepath);
                                       
                                        
                                       

                                       // FileInfo finfo = new FileInfo(Server.MapPath(string.Format("attachments{0}", rCat["IMAGE_FILE2"].ToString())));

                                        string newfile = rCat["IMAGE_FILE2"].ToString();
                                        if (newfile.ToLower().Contains(Ebook_pdf_FileRef) == false)
                                        {
                                            newfile = newfile.Replace("/media/pdf/", "/media/wes_secure_files/pdf/").Replace("\\media\\pdf\\", "\\media\\wes_secure_files\\pdf\\");
                                        }
                                        FileInfo finfo = new FileInfo(Server.MapPath(string.Format("attachments{0}", newfile)));
                                        
                                        long FileInBytes = finfo.Length;
                                        long FileInKB = finfo.Length / 1024;

                                       // objHelperServices.writelog(FileInBytes.ToString());
                                      //  objHelperServices.writelog(FileInKB.ToString());   
                                        string time = finfo.LastWriteTime.ToString("dd-MM-yyyy");

                                        _stmpl_records.SetAttribute("PDF_SIZE", FileInKB + " KB");
                                     //Modified by:indu :To sort wesnews by modified date

                                        //_stmpl_records.SetAttribute("PDF_DATE", time.Date.ToShortDateString());
                                        if (_Action == "NEWS")
                                        {
                                            if (Convert.ToDateTime(rCat["PublishedDate"].ToString()).Year == 1900)
                                            {
                                                _stmpl_records.SetAttribute("PDF_DATE", "");
                                            }
                                            else
                                            {
                                                _stmpl_records.SetAttribute("PDF_DATE", Convert.ToDateTime(rCat["PublishedDate"].ToString()).ToString("yyyy-MM-dd"));
                                            }
                                            }
                                        else
                                        {
                                           // _stmpl_records.SetAttribute("PDF_DATE", Convert.ToDateTime(rCat["MODIFIED_DATE"].ToString()).ToShortDateString());
                                           _stmpl_records.SetAttribute("PDF_DATE",time);
                                        }
                                    }
                                    else
                                    {
                                        _stmpl_records.SetAttribute("TBT_PDF_DESCRIPTION", "");
                                        _stmpl_records.SetAttribute("PDF_FILE_NAME", "");
                                    }
                                    //_stmpl_records.SetAttribute("PDF_EBOOK",);
                                    //_stmpl_records.SetAttribute("PDF_BROWSE_ONLINE", time.Date.ToString());

                                    if (_Action == "NEWS" || _Action == "CATALOGUE")
                                    {
                                        if (rCat["IMAGE_NAME"].ToString() != "")
                                        {
                                            //modified by:indu
                                            string ebookpath = objHelperServices.viewebook(rCat["IMAGE_NAME"].ToString());
                                           
                                            if (ebookpath.Contains("www."))
                                            {

                                                ebookpath = ebookpath.ToLower().Replace("attachments", ""). Replace("\\", "").Replace("http://", "").Replace("https://", "");
                                                ebookpath = "http://" + ebookpath;
                                            }
                                         
                                            _stmpl_records.SetAttribute("PDF_EBOOK", ebookpath.Replace("wes_secure_files/", "").Replace("wes_secure_files\\",""));
                                           // _stmpl_records.SetAttribute("PDF_EBOOK", rCat["IMAGE_NAME"].ToString());
                                            _stmpl_records.SetAttribute("EBOOK_DISPLAY", true);
                                        }
                                        else
                                        {
                                            _stmpl_records.SetAttribute("PDF_EBOOK", "");
                                            _stmpl_records.SetAttribute("EBOOK_DISPLAY", false);
                                        }
                                        string Ea_Path="";
                                        if (rCat["CATEGORY_PATH"].ToString().Contains("////") == true)
                                        {
                                           // objHelperServices.writelog("Inside if CATEGORY_PATH");
                                         
                                            if (rCat["CATEGORY_PATH"].ToString().EndsWith("////" + rCat["CATEGORY_NAME"].ToString()) == true)
                                            {
                                                string[] str = rCat["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                                                Ea_Path = "AllProducts////WESAUSTRALASIA";
                                                for (int i = 0; i < str.Length - 1; i++)
                                                {
                                                    //Modified by:Indu for repeatation in category 5-Oct-2016
                                                    if (Ea_Path.Contains(str[i].ToString()) == false)
                                                    {
                                                        Ea_Path = Ea_Path + "////" + str[i].ToString();
                                                    }
                                                }
                                                _stmpl_records.SetAttribute("TBT_URL_PATH", "product_list.aspx");
                                                _stmpl_records.SetAttribute("TBT_ISCAT", false);
                                            }
                                            else
                                            {

                                                Ea_Path = "AllProducts////WESAUSTRALASIA////" + rCat["CATEGORY_PATH"].ToString();
                                                _stmpl_records.SetAttribute("TBT_URL_PATH", "product_list.aspx");
                                                _stmpl_records.SetAttribute("TBT_ISCAT", false);
                                            }

                                        }
                                        else
                                        {
                                            _stmpl_records.SetAttribute("TBT_URL_PATH", "categorylist.aspx");                                            
                                            Ea_Path = "AllProducts////WESAUSTRALASIA";
                                            _stmpl_records.SetAttribute("TBT_ISCAT", true);
                                        }
                                        
                                        _stmpl_records.SetAttribute("TBT_PARENT_CATEGORY_ID",HttpUtility.UrlEncode( rCat["PARENT_CATEGORY"].ToString()));
                                        
                                        _stmpl_records.SetAttribute("TBT_CATEGORY_ID", HttpUtility.UrlEncode(rCat["CATEGORY_ID"].ToString()));
                                       
                                        _stmpl_records.SetAttribute("TBT_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(rCat["CATEGORY_NAME"].ToString()));
                                     
                                        _stmpl_records.SetAttribute("TBT_CUSTOM_NUM_FIELD3", HttpUtility.UrlEncode(rCat["CUSTOM_NUM_FIELD3"].ToString()));
                                       
                                        _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea_Path)));


                                    }
                                    if (_Action == "CATALOGUE")
                                    {
                                        _stmpl_records.SetAttribute("TBT_PDF_CATALOGUE", true);
                                        _stmpl_records.SetAttribute("TBT_PDF_NEWS", false);
                                        _stmpl_records.SetAttribute("TBT_PDF_FORMS", false);
                                    }
                                    else if (_Action == "NEWS")
                                    {
                                        _stmpl_records.SetAttribute("TBT_PDF_CATALOGUE", false);
                                        _stmpl_records.SetAttribute("TBT_PDF_NEWS", true);
                                       
                                        _stmpl_records.SetAttribute("TBT_PDF_FORMS", false);

                                    }

                                    else if (_Action == "FORMS")
                                    {
                                        _stmpl_records.SetAttribute("TBT_PDF_CATALOGUE", false);
                                        _stmpl_records.SetAttribute("TBT_PDF_NEWS", false);
                                        _stmpl_records.SetAttribute("TBT_PDF_FORMS", true);
                                    }
                                
                                   
                                    _stmpl_container.SetAttribute("TBWDataList", _stmpl_records.ToString());


                                 
                                    lstrecords[counter] = new TBWDataList(_stmpl_container.ToString());
                                    counter++;
                                }
                            }
                        }
                    }
                }
                
                catch (Exception e)
                {
                    objErrorHandler.ErrorMsg = e;
                    objErrorHandler.CreateLog();
                    objHelperServices.writelog(e.ToString()); 
                }
                try
                {
                   // objHelperServices.writelog("b4 _stg_container");
                    _stg_container = new StringTemplateGroup("cataloguedownload", stemplatepath);
                    _stmpl_container = _stg_container.GetInstanceOf("cataloguedownload" + "\\" + "main");
                    if (_Action == "CATALOGUE")
                    {
                        _stmpl_container.SetAttribute("TBT_PDF_CATALOGUE", true);
                        _stmpl_container.SetAttribute("TBT_PDF_NEWS", false);
                        _stmpl_container.SetAttribute("TBT_PDF_FORMS", false);
                    }
                    else if (_Action == "NEWS")
                    {
                        _stmpl_container.SetAttribute("TBT_PDF_CATALOGUE", false);
                        _stmpl_container.SetAttribute("TBT_PDF_NEWS", true);
                        _stmpl_container.SetAttribute("TBT_PDF_FORMS", false);
                    }

                    else if (_Action == "FORMS")
                    {
                        _stmpl_container.SetAttribute("TBT_PDF_CATALOGUE", false);
                        _stmpl_container.SetAttribute("TBT_PDF_NEWS", false);
                        _stmpl_container.SetAttribute("TBT_PDF_FORMS", true);
                    }
                    _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                    try
                    {
                        FileInfo finfo_gst = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["Modifieddate_exgst"].ToString());
                        if (finfo_gst.Exists)
                        {
                            _stmpl_container.SetAttribute("Modifieddate_exgst", finfo_gst.LastWriteTime.ToString("dd-MM-yyyy"));

                        }
                        FileInfo finfo_pricelist = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["Modifieddate_pricelist"].ToString());
                        if (finfo_pricelist.Exists)
                        {
                            _stmpl_container.SetAttribute("Modifieddate_pricelist", finfo_pricelist.LastWriteTime.ToString("dd-MM-yyyy"));

                        }

                    }
                    catch (Exception ex)
                    {
                        objErrorHandler.CreateLog(ex.ToString());   
                    }

                    shtml = _stmpl_container.ToString();
                   // objHelperServices.writelog(shtml);
                }
                catch (Exception ex)
                {
                    objHelperServices.writelog(ex.ToString());
                }

         }
        //    else
        //        return "<table height=\"100\"><tr><td valign=\"bottom\">No PDF Catalogue found</td></tr></table>";
        //}
        //else
        //    return "<table height=\"100\"><tr><td valign=\"bottom\">No PDF catalogue found</td></tr></table>";
        return objHelperServices.StripWhitespace(shtml) ;

    }
}
