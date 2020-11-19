using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text; 
using System.IO;
using TradingBell5.CatalogX;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.EasyAsk;
using TradingBell.WebCat.TemplateRender;
using TradingBell.WebCat.CommonServices;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
//using ExpertPdf.HtmlToPdf;
using System.Configuration;
using System.Globalization;
using Pechkin;
using System.Xml;
public partial class UC_family : System.Web.UI.UserControl
{
    AjaxControlToolkit.ModalPopupExtender modalPop = new AjaxControlToolkit.ModalPopupExtender();
    ConnectionDB objConnectionDB = new ConnectionDB();
    HelperDB objHelperDB = new HelperDB();
    ErrorHandler objErrorHandler = new ErrorHandler();
    HelperServices objHelperServices = new HelperServices();
    FamilyServices objFamilyServices = new FamilyServices();

   // string category_id = "1";
    string templatename = string.Empty;
    string contentvalue = string.Empty;
    string CScontentvalue = string.Empty;
    string subfamtemplate = string.Empty;
    string iRecordsPerPage = "18";
    DataSet Ds = new DataSet();
    DataSet EADs = new DataSet();
    DataSet Dsall = new DataSet();
    DataTable Famtb = new DataTable();
    DataTable SFamtb = new DataTable();
    DataTable EASubFamtb = new DataTable();
    DataTable SFamtb1 = new DataTable();
    DataSet dsPriceTableAll = new DataSet();
    DataSet DDS = null;
    TBWTemplateEngine tbwtEngine;
    FamilyServices ObjFamilyPage = new FamilyServices();
    EasyAsk_WES objEasyAsk = new EasyAsk_WES();
    string breadcrumb = string.Empty;
    string _Familyids = string.Empty;
    string _Fid = string.Empty;
    string stemplatepath = string.Empty;
    string strPDFFiles1 = HttpContext.Current.Server.MapPath("attachments");
    string strImgFiles1 = HttpContext.Current.Server.MapPath("ProdImages");
    public string DownloadST = string.Empty;
    public bool isDownload = false;
    string downloadST = string.Empty;
    bool isdownload = false;
    bool isdownload_product = false;
    public string ctname = string.Empty;
  //  private string _Package = null;
  //  private string _SkinRootPath = null;
    protected void Page_Load(object sender, EventArgs e)
    {

       // this.ModalPanel1.Visible = false;
        stemplatepath = Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());

        if (!IsPostBack) 
        {
        if (HttpContext.Current.Request.QueryString["fid"] != null)
        {
           
            hffid.Value = HttpContext.Current.Request.QueryString["fid"];
           // string sessionname = "FamilyProduct_" + hffid.Value;
            HttpContext.Current.Session["pfid"] = hffid.Value;
        }
        if (HttpContext.Current.Request.QueryString["path"] != null)
        {
            hfpath.Value= HttpContext.Current.Request.QueryString["path"];
        }
        HFcnt.Value = "0";
          
        }
    }
    public string ST_FamilypageALLData()
    {

        GetFamilyAllData("");
          return "";

    }

    public string Bread_Crumbs()
    {
     
        breadcrumb = objEasyAsk.GetBreadCrumb(Server.MapPath("Templates"));
        return breadcrumb;
    }
    private void ConstructFamilyDatajson(string familyid, DataRow[] SourceFtb, DataSet tempDs, int tableinx)
    {
        try
        {
            DataTable dt = new DataTable();
           // DataSet tempDs = new DataSet();
           // tempDs = GetFamilyPageProduct(familyid, "PRODUCT"); // Get Other Attribute from Db
            if (tempDs != null && tempDs.Tables.Count > 0 && tempDs.Tables[tableinx].Rows.Count > 0)
            {
                Ds.Tables.Add(familyid);
                Ds.Tables[familyid].Columns.Add("FAMILY_ID", typeof(string));
                Ds.Tables[familyid].Columns.Add("PRODUCT_ID", typeof(string));
                Ds.Tables[familyid].Columns.Add("TWeb Image1", typeof(string));
                Ds.Tables[familyid].Columns.Add("Code", typeof(string));
                for (int i = 0; i <= tempDs.Tables[tableinx].Columns.Count - 1; i++)
                {
                    string tempdscolname = tempDs.Tables[tableinx].Columns[i].ColumnName.ToUpper();
                    if (tempdscolname != "FAMILY_ID" && tempdscolname != "PRODUCT_ID")
                    {
                        Ds.Tables[familyid].Columns.Add(tempdscolname, typeof(string));
                    }
                }


                foreach (DataRow tdr in SourceFtb)
                {
                    DataRow Dsdr = Ds.Tables[familyid].NewRow();
                    Dsdr["FAMILY_ID"] = tdr["FAMILY_ID"];
                    Dsdr["PRODUCT_ID"] = tdr["PRODUCT_ID"];
                    Dsdr["TWeb Image1"] = tdr["PRODUCT_TH_IMAGE"];
                    Dsdr["CODE"] = tdr["PRODUCT_CODE"];
                    Dsdr["COST"] = tdr["PRODUCT_PRICE"];
                    DataRow[] tempDr = tempDs.Tables[tableinx].Select("FAMILY_ID='" + familyid + "' And PRODUCT_ID='" + tdr["PRODUCT_ID"] + "'");
                    if (tempDr.Length > 0)
                    {
                        DataTable temptb = tempDr.CopyToDataTable();

                        for (int i = 0; i <= temptb.Columns.Count - 1; i++)
                        {
                            string temptbcolname = temptb.Columns[i].ColumnName.ToUpper();

                            if (temptbcolname != "FAMILY_ID" && temptbcolname != "PRODUCT_ID" && temptbcolname != "COST")
                            {
                                try
                                {
                                    Dsdr[temptbcolname] = temptb.Rows[0][temptbcolname];
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                    Ds.Tables[familyid].Rows.Add(Dsdr);
                }

            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
    }
    private void ConstructFamilyData(string familyid, DataTable SourceFtb)
    {
        try
        {
            
            DataSet tempDs = new DataSet();
            tempDs = GetFamilyPageProduct(familyid, "PRODUCT"); // Get Other Attribute from Db
            if (tempDs != null && tempDs.Tables.Count > 0)
            {
                Ds.Tables.Add(familyid);
                Ds.Tables[familyid].Columns.Add("FAMILY_ID", typeof(string));
                Ds.Tables[familyid].Columns.Add("PRODUCT_ID", typeof(string));
                Ds.Tables[familyid].Columns.Add("TWeb Image1", typeof(string));
                Ds.Tables[familyid].Columns.Add("Code", typeof(string));
                for (int i = 0; i <= tempDs.Tables[0].Columns.Count - 1; i++)
                {
                    string tempdscolname = tempDs.Tables[0].Columns[i].ColumnName.ToUpper();
                    if (tempdscolname != "FAMILY_ID" && tempdscolname != "PRODUCT_ID")
                    {
                        Ds.Tables[familyid].Columns.Add(tempdscolname, typeof(string));
                    }
                }


                foreach (DataRow tdr in SourceFtb.Rows)
                {
                    DataRow Dsdr = Ds.Tables[familyid].NewRow();
                    Dsdr["FAMILY_ID"] = tdr["FAMILY_ID"];
                    Dsdr["PRODUCT_ID"] = tdr["PRODUCT_ID"];
                    Dsdr["TWeb Image1"] = tdr["PRODUCT_TH_IMAGE"];
                    Dsdr["CODE"] = tdr["PRODUCT_CODE"];
                    Dsdr["COST"] = tdr["PRODUCT_PRICE"];
                    DataRow[] tempDr = tempDs.Tables[0].Select("FAMILY_ID='" + familyid + "' And PRODUCT_ID='" + tdr["PRODUCT_ID"] + "'");
                    if (tempDr.Length > 0)
                    {
                        DataTable temptb = tempDr.CopyToDataTable();

                        for (int i = 0; i <= temptb.Columns.Count - 1; i++)
                        {
                            string temptbcolname = temptb.Columns[i].ColumnName.ToUpper();
                            if (temptbcolname != "FAMILY_ID" && temptbcolname != "PRODUCT_ID" && temptbcolname != "COST")
                            {
                                try
                                {
                                    Dsdr[temptbcolname] = temptb.Rows[0][temptbcolname];
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                    Ds.Tables[familyid].Rows.Add(Dsdr);
                }

            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
    }
    private void ConstructSubFamilyHeader(DataSet EADs, DataTable SFtb)
    {
        try
        {
        if (SFtb!=null)
        {
            DataTable tempDs = new DataTable();  
            string image_string="";
            DataRow dRow;
            EADs.Tables.Add(EASubFamtb);
            EASubFamtb.Columns.Add("FAMILY_ID", typeof(string));
            EASubFamtb.Columns.Add("FAMILY_NAME", typeof(string));
            EASubFamtb.Columns.Add("STRING_VALUE", typeof(string));
            EASubFamtb.Columns.Add("NUMERIC_VALUE", typeof(string));
            EASubFamtb.Columns.Add("ATTRIBUTE_ID", typeof(string));
            EASubFamtb.Columns.Add("ATTRIBUTE_NAME", typeof(string));
            EASubFamtb.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
            EASubFamtb.Columns.Add("OBJECT_TYPE", typeof(string));
            EASubFamtb.Columns.Add("OBJECT_NAME", typeof(string));
            EASubFamtb.TableName = "SubFamily";

            foreach (DataRow tdr in SFtb.Rows)
            {
                DataRow[] dr=EADs.Tables["FamilyPro"].Select("FAMILY_ID='" + tdr["FAMILY_ID"] +"'");
                if (dr.Length>0)
                {
                    tempDs=dr.CopyToDataTable();

                    dRow = EASubFamtb.NewRow();
                    dRow["FAMILY_ID"] =tempDs.Rows[0]["FAMILY_ID"] ;
                    dRow["FAMILY_NAME"] = tempDs.Rows[0]["FAMILY_NAME"];
                                        
                    //------------------
                    dRow["ATTRIBUTE_ID"] = "13";
                    dRow["STRING_VALUE"] = tempDs.Rows[0]["FAMILY_SHORT_DESC"];//Family Description
                    dRow["NUMERIC_VALUE"] = "0";
                    dRow["OBJECT_TYPE"] = "NULL";
                    dRow["OBJECT_NAME"] = "NULL";
                    dRow["ATTRIBUTE_NAME"] = "SHORT_DESCRIPTION";
                    dRow["ATTRIBUTE_TYPE"] = "7";
                    EASubFamtb.Rows.Add(dRow.ItemArray);

                    //------------------
                    dRow["ATTRIBUTE_ID"] = "4";
                    dRow["STRING_VALUE"] = tempDs.Rows[0]["FAMILY_DESC"];//Family Description
                    dRow["NUMERIC_VALUE"] = "0";
                    dRow["OBJECT_TYPE"] = "NULL";
                    dRow["OBJECT_NAME"] = "NULL";
                    dRow["ATTRIBUTE_NAME"] = "DESCRIPTION";
                    dRow["ATTRIBUTE_TYPE"] = "7";
                    EASubFamtb.Rows.Add(dRow.ItemArray);



                    //------------------
                    image_string = tempDs.Rows[0]["FAMILY_TH_IMAGE"].ToString() ;
                    if(!(image_string.ToLower().Contains("noimage.gif")))  
                       image_string=image_string.ToLower().Replace("_th", "_Images_200");
                    
                    dRow["ATTRIBUTE_ID"] = "746";
                    dRow["STRING_VALUE"] = image_string;
                    dRow["NUMERIC_VALUE"] = "0";
                    dRow["OBJECT_TYPE"] = "jpg";
                    dRow["OBJECT_NAME"] = image_string;
                    dRow["ATTRIBUTE_NAME"] = "FWeb Image1";
                    dRow["ATTRIBUTE_TYPE"] = "9";
                    EASubFamtb.Rows.Add(dRow.ItemArray);

                    

                    //------------------
                    dRow["ATTRIBUTE_ID"] = "747";
                    dRow["STRING_VALUE"] = tempDs.Rows[0]["FAMILY_TH_IMAGE"];
                    dRow["NUMERIC_VALUE"] = "0";
                    dRow["OBJECT_TYPE"] = "jpg";
                    dRow["OBJECT_NAME"] = tempDs.Rows[0]["FAMILY_TH_IMAGE"];
                    dRow["ATTRIBUTE_NAME"] = "TFWeb Image1";
                    dRow["ATTRIBUTE_TYPE"] = "9";
                    EASubFamtb.Rows.Add(dRow.ItemArray);       

                }                
               
            }

            HttpContext.Current.Session["FamilyProduct"] = EADs;
        }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
    }
    private void ConstructSubFamilyHeader_print(DataSet EADs, DataTable SFtb)
    {
        try
        {
            if (SFtb != null)
            {
                DataTable tempDs = new DataTable();
                string image_string = string.Empty;
                DataRow dRow;
                EADs.Tables.Add(EASubFamtb);
                EASubFamtb.Columns.Add("FAMILY_ID", typeof(string));
                EASubFamtb.Columns.Add("FAMILY_NAME", typeof(string));
                EASubFamtb.Columns.Add("STRING_VALUE", typeof(string));
                EASubFamtb.Columns.Add("NUMERIC_VALUE", typeof(string));
                EASubFamtb.Columns.Add("ATTRIBUTE_ID", typeof(string));
                EASubFamtb.Columns.Add("ATTRIBUTE_NAME", typeof(string));
                EASubFamtb.Columns.Add("ATTRIBUTE_TYPE", typeof(string));
                EASubFamtb.Columns.Add("OBJECT_TYPE", typeof(string));
                EASubFamtb.Columns.Add("OBJECT_NAME", typeof(string));
               // EASubFamtb.TableName = "SubFamily_print";

                foreach (DataRow tdr in SFtb.Rows)
                {
                    DataRow[] dr = EADs.Tables["FamilyPro"].Select("FAMILY_ID='" + tdr["FAMILY_ID"] + "'");
                    if (dr.Length > 0)
                    {
                        tempDs = dr.CopyToDataTable();

                        dRow = EASubFamtb.NewRow();
                        dRow["FAMILY_ID"] = tempDs.Rows[0]["FAMILY_ID"];
                        dRow["FAMILY_NAME"] = tempDs.Rows[0]["FAMILY_NAME"];

                        //------------------
                        dRow["ATTRIBUTE_ID"] = "13";
                        dRow["STRING_VALUE"] = tempDs.Rows[0]["FAMILY_SHORT_DESC"];//Family Description
                        dRow["NUMERIC_VALUE"] = "0";
                        dRow["OBJECT_TYPE"] = "NULL";
                        dRow["OBJECT_NAME"] = "NULL";
                        dRow["ATTRIBUTE_NAME"] = "SHORT_DESCRIPTION";
                        dRow["ATTRIBUTE_TYPE"] = "7";
                        EASubFamtb.Rows.Add(dRow.ItemArray);

                        //------------------
                        dRow["ATTRIBUTE_ID"] = "4";
                        dRow["STRING_VALUE"] = tempDs.Rows[0]["FAMILY_DESC"];//Family Description
                        dRow["NUMERIC_VALUE"] = "0";
                        dRow["OBJECT_TYPE"] = "NULL";
                        dRow["OBJECT_NAME"] = "NULL";
                        dRow["ATTRIBUTE_NAME"] = "DESCRIPTION";
                        dRow["ATTRIBUTE_TYPE"] = "7";
                        EASubFamtb.Rows.Add(dRow.ItemArray);



                        //------------------
                        image_string = tempDs.Rows[0]["FAMILY_TH_IMAGE"].ToString();
                        if (!(image_string.ToLower().Contains("noimage.gif")))
                            image_string = image_string.ToLower().Replace("_th", "_Images_200");

                        dRow["ATTRIBUTE_ID"] = "746";
                        dRow["STRING_VALUE"] = image_string;
                        dRow["NUMERIC_VALUE"] = "0";
                        dRow["OBJECT_TYPE"] = "jpg";
                        dRow["OBJECT_NAME"] = image_string;
                        dRow["ATTRIBUTE_NAME"] = "FWeb Image1";
                        dRow["ATTRIBUTE_TYPE"] = "9";
                        EASubFamtb.Rows.Add(dRow.ItemArray);



                        //------------------
                        dRow["ATTRIBUTE_ID"] = "747";
                        dRow["STRING_VALUE"] = tempDs.Rows[0]["FAMILY_TH_IMAGE"];
                        dRow["NUMERIC_VALUE"] = "0";
                        dRow["OBJECT_TYPE"] = "jpg";
                        dRow["OBJECT_NAME"] = tempDs.Rows[0]["FAMILY_TH_IMAGE"];
                        dRow["ATTRIBUTE_NAME"] = "TFWeb Image1";
                        dRow["ATTRIBUTE_TYPE"] = "9";
                        EASubFamtb.Rows.Add(dRow.ItemArray);

                    }

                }

                HttpContext.Current.Session["FamilyProduct"] = EADs;
            }
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
    }
    public string ST_Family_Download()
    {
        try
        {
            DataSet TmpDs = EADs;
            DataSet TempEADs = Dsall;
            int tableinx = Dsall.Tables.Count - 1;

            string rtnstr = string.Empty;
            StringTemplateGroup _stg_container = null;
         //   StringTemplateGroup _stg_records = null;
           StringTemplate _stmpl_container = null;
         //   StringTemplate _stmpl_records = null;
          //  StringTemplate _stmpl_records1 = null;
         //   StringTemplate _stmpl_recordsrows = null;
            TBWDataList[] lstrecords = new TBWDataList[0];
            TBWDataList[] lstrows = new TBWDataList[0];

            StringTemplateGroup _stg_container1 = null;
            StringTemplateGroup _stg_records1 = null;
            TBWDataList1[] lstrecords1 = new TBWDataList1[0];
            TBWDataList1[] lstrows1 = new TBWDataList1[0];


            DataTable dt = new DataTable();
            DataRow[] dr = null;

            int ictrecords = 0;

            if (Request.QueryString["fid"] != null)
                _Fid = Request.QueryString["fid"].ToString();

            //_Familyids = _Fid;

            //DataSet TmpDs = (DataSet)HttpContext.Current.Session["FamilyProduct"];

            //dr = TmpDs.Tables["FamilyPro"].Select("FAMILY_ID<>'" + _Fid + "'");
            //if (dr.Length > 0)
            //{
            //    SFamtb = dr.CopyToDataTable().DefaultView.ToTable(true, "FAMILY_ID").Copy();
            //    if (SFamtb != null)
            //    {
            //        for (int i = 0; i <= SFamtb.Rows.Count - 1; i++)
            //        {
            //            _Familyids = _Familyids + "," + SFamtb.Rows[i]["FAMILY_ID"].ToString();
            //        }
            //    }
            //}

            _stg_container = new StringTemplateGroup("main", stemplatepath);

            _stmpl_container = _stg_container.GetInstanceOf("Familypage" + "\\" + "DownloadMain");

            if (_Familyids != "")
            {

                // DataSet TempEADs = GetFamilyPageProduct(_Familyids, "ATTACHMENT");
                if (TempEADs != null && TempEADs.Tables.Count > 0 && TempEADs.Tables[tableinx].Rows.Count > 0)
                {
                    //TempEADs.Tables[0].Columns.Add("FAMILY_NAME");



                    string[] strf = _Familyids.Split(new string[] { "," }, StringSplitOptions.None);
                    if (strf.Length > 0)
                    {
                        lstrecords = new TBWDataList[strf.Length];
                        for (int intfam = 0; intfam <= strf.Length - 1; intfam++)
                        {
                            dr = null;
                            dr = TempEADs.Tables[tableinx].Select("FAMILY_ID='" + strf[intfam] + "'", "Sno");
                            if (dr.Length > 0)
                            {
                                //dt = new DataTable();
                                //dt = dr.CopyToDataTable();
                                rtnstr = ST_Familypage_Download(_Fid, strf[intfam], dr);
                                if (rtnstr != "")
                                {
                                    lstrecords[ictrecords] = new TBWDataList(rtnstr.ToString());
                                    ictrecords = ictrecords + 1;
                                }

                            }
                        }





                    }
                }





            }
            _stmpl_container.SetAttribute("TBWDataList", lstrecords);
            string DownloadST_Product = ST_Product_Download(TmpDs);
            _stmpl_container.SetAttribute("PRODUCT_DOWNLOAD", DownloadST_Product);
            

            if (ictrecords > 0 || DownloadST_Product != "")
            {

                _stmpl_container.SetAttribute("DOWNLOAD_MAIL", ST_Downloads_Update(true));
                DownloadST = _stmpl_container.ToString();                
                isDownload = true;
                return "block";
            }

            _stmpl_container = _stg_container.GetInstanceOf("Familypage" + "\\" + "DownloadMain");
            _stmpl_container.SetAttribute("DOWNLOAD_MAIL", ST_Downloads_Update(false));
            DownloadST = _stmpl_container.ToString();
            isDownload = true ;


        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
        return "block";


    }


    public string ST_Familypage_Download(string pFamilyid, string Familyid, DataRow[] Adt)
    {
        StringTemplateGroup _stg_container = null;
        StringTemplateGroup _stg_records = null;
        StringTemplate _stmpl_container = null;
        StringTemplate _stmpl_records = null;

        TBWDataList[] lstrecords = new TBWDataList[0];
        TBWDataList[] lstrows = new TBWDataList[0];


        TBWDataList1[] lstrecords1 = new TBWDataList1[0];
        TBWDataList1[] lstrows1 = new TBWDataList1[0];



        long FileInKB;
        string[] file = null;
        string strfile = string.Empty;
        if (Adt != null && Adt.Length > 0)
        {




            DataSet dscat = new DataSet();


            try
            {
                _stg_records = new StringTemplateGroup("cell", stemplatepath);
                _stg_container = new StringTemplateGroup("row", stemplatepath);


                lstrecords = new TBWDataList[Adt.Length + 1];



                int ictrecords = 0;

                foreach (DataRow dr in Adt)//For Records
                {
                    strfile = dr["FAMILY_ATT_FILE"].ToString().Replace(@"\\","/");
                   

                    FileInfo Fil;
                    

                    if ((dr["FAMILY_ATT_FILE"].ToString().ToLower().Contains(".jpg")))
                        Fil = new FileInfo(strImgFiles1 + dr["FAMILY_ATT_FILE"].ToString());
                    else
                        Fil = new FileInfo(strPDFFiles1 + dr["FAMILY_ATT_FILE"].ToString());

                    if (Fil.Exists)
                    {
                        _stmpl_records = _stg_records.GetInstanceOf("Familypage" + "\\" + "DownloadCell");

                        strfile = dr["FAMILY_ATT_FILE"].ToString().Replace(@"\\", "/");
                        strfile = strfile.Replace(@"\", "/");

                        file = strfile.Split(new string[] { @"/" }, StringSplitOptions.None);
                        if (file.Length > 0)
                        {

                            _stmpl_records.SetAttribute("TBT_ATTACH_FILE_NAME", file[file.Length - 1].ToString());
                            if (file[file.Length - 1].ToString().ToLower().Contains(".jpg")==true)
                                _stmpl_records.SetAttribute("TBT_ATTACH_INIT_PATH", "prodimages");
                            else
                                _stmpl_records.SetAttribute("TBT_ATTACH_INIT_PATH", "");
                        }

                        //  FileInBytes = Fil.Length;
                        FileInKB = Fil.Length / 1024;
                        
                         
                        _stmpl_records.SetAttribute("TBT_FAMILY_ATT_DESC", dr["FAMILY_ATT_DESC"].ToString());


                        _stmpl_records.SetAttribute("TBT_ATTACH_FILE_PATH", strfile.Replace(".PDF", ".pdf"));
                        _stmpl_records.SetAttribute("TBT_ATTACH_SIZE", FileInKB.ToString() + " KB");


                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                        ictrecords++;
                    }
                }

                _stmpl_container = _stg_container.GetInstanceOf("Familypage" + "\\" + "DownloadRow");

                _stmpl_container.SetAttribute("TBT_FAMILY_NAME", Adt[0]["FAMILY_NAME"].ToString());
                if (pFamilyid.ToLower() == Familyid.ToLower())
                {
                    _stmpl_container.SetAttribute("TBT_FAMILY_HEAD", false);
                }
                else
                {
                    _stmpl_container.SetAttribute("TBT_FAMILY_HEAD", true);
                }



                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                if(ictrecords>0)
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
    public string ST_Product_Download(DataSet TmpDs)
    {
        string rtnstr = string.Empty;
       // StringTemplateGroup _stg_container = null;
       // StringTemplateGroup _stg_records = null;
       // StringTemplate _stmpl_container = null;
       // StringTemplate _stmpl_records = null;
       // StringTemplate _stmpl_records1 = null;
       // StringTemplate _stmpl_recordsrows = null;
        TBWDataList[] lstrecords = new TBWDataList[0];
        TBWDataList[] lstrows = new TBWDataList[0];

       // StringTemplateGroup _stg_container1 = null;
       // StringTemplateGroup _stg_records1 = null;
        TBWDataList1[] lstrecords1 = new TBWDataList1[0];
        TBWDataList1[] lstrows1 = new TBWDataList1[0];
        string downloadst_pro = string.Empty;


        DataTable dt = new DataTable();
        DataRow[] dr = null;

       // int ictrecords = 0;


        string DownloadST_Product = string.Empty;




        string _pid_multiple = string.Empty;
        string pcode_multiple = string.Empty;
        for (int prd = 0; prd <= TmpDs.Tables["FamilyPro"].Rows.Count - 1; prd++)
        {

            string _pid = TmpDs.Tables["FamilyPro"].Rows[prd]["PRODUCT_ID"].ToString();
            string prodcode = TmpDs.Tables["FamilyPro"].Rows[prd]["PRODUCT_CODE"].ToString() + " - Product Downloads";
            if (_pid != "")
            {
                if (_pid_multiple == string.Empty)
                {
                    _pid_multiple = _pid;
                    pcode_multiple = prodcode;
                }
                else
                {
                    _pid_multiple = _pid_multiple + "," + _pid;
                    pcode_multiple = pcode_multiple + "," + prodcode;

                }
            }

        }

        DataSet TempEADs_pid = objFamilyServices.GetFamilyPageProduct(_pid_multiple, "PRODUCT_ATTACHMENT");
        string[] pid = _pid_multiple.Split(',');
        string[] pcode = pcode_multiple.Split(',');
        for (int i = 0; i <= pid.Length - 1; i++)
        {
            DataRow[] drpid = TempEADs_pid.Tables[0].Select("PRODUCT_ID='" + pid[i] + "'");
            if (drpid.Length > 0)
            {

                rtnstr = ST_Productpage_Download(drpid, pcode[i].ToString());
                if (rtnstr != "")
                {
                    DownloadST_Product = DownloadST_Product + rtnstr;
                }
            }

        }
        return DownloadST_Product;



    }

    public string ST_Productpage_Download(DataRow[] Adt, string Protitle)
    {
        isdownload_product = false;
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
        if (Adt != null && Adt.Length > 0)
        {




            DataSet dscat = new DataSet();


            try
            {
                _stg_records = new StringTemplateGroup("cell", stemplatepath);
                _stg_container = new StringTemplateGroup("row", stemplatepath);

                //_stg_container = new StringTemplateGroup("row", stemplatepath);

               // _stg_container = new StringTemplateGroup("row", _SkinRootPath);


                lstrecords = new TBWDataList[Adt.Length + 1];


                int ictrecords = 0;

                foreach (DataRow dr in Adt)//For Records
                {
                    strfile = dr["PRODUCT_ATT_FILE"].ToString().Replace(@"\\", "/");


                    FileInfo Fil;



                    if ((dr["PRODUCT_ATT_FILE"].ToString().ToLower().Contains(".jpg")))
                        Fil = new FileInfo(strImgFiles1 + dr["PRODUCT_ATT_FILE"].ToString());
                    else
                        Fil = new FileInfo(strPDFFiles1 + dr["PRODUCT_ATT_FILE"].ToString());


                    if (Fil.Exists)
                    {
                        _stmpl_records = _stg_records.GetInstanceOf("Familypage" + "\\" + "DownloadCell_Product");

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

                _stmpl_container = _stg_container.GetInstanceOf("FamilyPage" + "\\" + "DownloadRow_Product");

                _stmpl_container.SetAttribute("TBT_PRODUCT_CODE", Protitle);
                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                if (ictrecords > 0)
                {
                    //isdownload_product = true;
                    return _stmpl_container.ToString();
                }
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
    public string ST_Familypage_print(string fid,string withprice,string withdetails,DataSet Dsfamilyproduct)
    {

        try
        {
          
            //if (Request.QueryString["type"] == null)
            //{

            //    EasyAsk.GetAttributeProducts("FamilyPage", "", "FamilyId", _fid, "", "0", "0", "");

            //}
            //else
            //{

            //    EasyAsk.GetAttributeProducts("FamilyPage", _searchstr, _type, _value, _bname, "0", "0", "");

            //}
            EasyAsk_WES EasyAsk = new EasyAsk_WES();
          // EasyAsk.GetAttributeProducts("FamilyPage", "", "FamilyId", fid, "", "0", "0", "");
          // DataSet EADs = (DataSet)HttpContext.Current.Session["FamilyProduct"];


            //DataSet EADs = new DataSet();
            DataSet EADs1 = new DataSet();
            if (Dsfamilyproduct != null)
                EADs1 = Dsfamilyproduct;
            else
            {
                EasyAsk.GetAttributeProducts("FamilyPage", "", "FamilyId", fid, "", "0", "0", "");
                EADs1 = (DataSet)HttpContext.Current.Session["FamilyProduct"];
            }

            //string sessionname = "FamilyProduct_" + hffid.Value.Trim();

            //DataSet EADs = (DataSet)HttpContext.Current.Session[sessionname];

            DataSet tempDs = new DataSet();
            DataTable Ftb = new DataTable();


            DataTable Atttbl = new DataTable();

            DataRow[] Dr = null;


            string _UserID = string.Empty;

            string _cid = string.Empty;
            string _pcr = string.Empty;


            _Fid = fid;


            if (HttpContext.Current.Session["USER_ID"].ToString() != null)
                _UserID = HttpContext.Current.Session["USER_ID"].ToString();

            _Familyids = _Fid;
            if (EADs1 != null)
            {
                // Main Family
                Dr = EADs1.Tables["FamilyPro"].Select("FAMILY_ID='" + _Fid + "'");
                if (Dr.Length > 0)
                {
                    Famtb = Dr.CopyToDataTable();
                    ConstructFamilyData(_Fid, Famtb);
                }
                // Sub Family
                Dr = null;
               // SFamtb = new DataTable();
                SFamtb1 = new DataTable();
                Dr = EADs1.Tables["FamilyPro"].Select("FAMILY_ID<>'" + _Fid + "'");
                if (Dr.Length > 0)
                {
                    SFamtb1 = Dr.CopyToDataTable().DefaultView.ToTable(true, "FAMILY_ID").Copy();
                    if (SFamtb1 != null)
                    {
                        for (int i = 0; i <= SFamtb1.Rows.Count - 1; i++)
                        {
                            Dr = EADs1.Tables["FamilyPro"].Select("FAMILY_ID='" + SFamtb1.Rows[i]["FAMILY_ID"].ToString() + "'");
                            if (Dr.Length > 0)
                            {
                                Ftb = Dr.CopyToDataTable();
                                ConstructFamilyData(SFamtb1.Rows[i]["FAMILY_ID"].ToString(), Ftb);
                            }
                            _Familyids = _Familyids + "," + SFamtb1.Rows[i]["FAMILY_ID"].ToString();
                        }

                    }

                }
                // Get Family Attribute
                tempDs = GetFamilyPageProduct(_Familyids, "ATTRIBUTE");
                if (tempDs != null)
                {
                    tempDs.Tables[0].TableName = "Attribute_pdf";
                    Ds.Tables.Add(tempDs.Tables[0].Copy());
                }
                // Get Product Price & Inventory
                tempDs = GetProductPrice(_Familyids, "", _UserID);
                if (tempDs != null)
                {
                    tempDs.Tables[0].TableName = "ProductPrice_pdf";
                    Ds.Tables.Add(tempDs.Tables[0].Copy());
                }

                // Construct Sub FamilyData
                ConstructSubFamilyHeader_print(EADs1, SFamtb1);

            }



            try
            {
                contentvalue = "";
               

                if (Ds.Tables[_Fid] != null)
                {
                    if (Ds.Tables[_Fid].Columns.Count <= 9)
                    {
                        CScontentvalue = ObjFamilyPage.GenerateHorizontalHTML_print(_Fid, Ds, withprice, withdetails);
                        CScontentvalue.Replace("<tr></tr>", "");
                    }
                    else
                    {
                    // 
                        if (Ds.Tables[_Fid].Rows.Count > 7)
                        {
                            int rowcount = Ds.Tables[_Fid].Rows.Count/7;
                            int endcount=0;
                            for (int i = 0; i <= rowcount; i++)
                            {
                                if (i == 0)
                                {
                                    CScontentvalue = ObjFamilyPage.GenerateVerticalHTML_blocks(_Fid, Ds, withprice, withdetails, 0, 7);
                                    CScontentvalue = CScontentvalue.Replace("<tr></tr>", "");
                                }
                                else
                                {
                                    if (Ds.Tables[_Fid].Rows.Count >= ((7 * i) + 7))
                                    {
                                        endcount = (7 * i) + 7;
                                    }
                                    else
                                    {
                                        endcount = Ds.Tables[_Fid].Rows.Count;
                                    }
                                    CScontentvalue = CScontentvalue + ObjFamilyPage.GenerateVerticalHTML_blocks(_Fid, Ds, withprice, withdetails, 7 * i, endcount).Replace("<tr></tr>", "");
                                
                                }
                                }
                                
                        }
                        else
                        {
                            CScontentvalue = ObjFamilyPage.GenerateVerticalHTML_printnew(_Fid, Ds, withprice, withdetails);
                          CScontentvalue=  CScontentvalue.Replace("<tr></tr>", "");
                        }
                        }
                }
                else
                {
                    CScontentvalue = ObjFamilyPage.GenerateHorizontalHTML_print(_Fid, Ds, withprice, withdetails);
                    CScontentvalue = CScontentvalue.Replace("<tr></tr>", "");
                }
                if (Famtb.Rows.Count == 1 && SFamtb1.Rows.Count == 0 && (HttpContext.Current.Request.QueryString["ProductResult"] != null && HttpContext.Current.Request.QueryString["ProductResult"].ToString().Equals("SUCCESS")))
                    {
                        Response.Redirect("productdetails.aspx?&pid=" + Famtb.Rows[0]["Product_ID"].ToString() + "&fid=" + HttpContext.Current.Request.QueryString["fid"].ToString() + "&cid=" + _cid + "&pcr=" + _pcr + "byp=2", true);
                        return "";
                    }
                else if (SFamtb1 != null)
                    {

                        foreach (DataRow DR in SFamtb1.Rows)
                        {
                            string cssubfamilycontent;
                            //cssubfamilycontent = getcstable(DR["Family_id"].ToString());
                            cssubfamilycontent = ObjFamilyPage.GenerateHorizontalHTML_print(DR["Family_id"].ToString(), Ds,withprice,withdetails);
                            if (cssubfamilycontent != "" && cssubfamilycontent.Length > 336)
                            {
                               // templatename = "CSFAMILYPAGEWITHSUBFAMILY";
                                templatename = "CSFAMILYPAGEWITHSUBFAMILY_PRINT";
                                tbwtEngine = new TBWTemplateEngine(templatename,HttpContext .Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
                                tbwtEngine.paraValue = DR["Family_id"].ToString();
                                tbwtEngine.paraFID = DR["Family_id"].ToString();
                                tbwtEngine.RenderHTML("Row");
                                subfamtemplate = subfamtemplate + tbwtEngine.RenderedHTML;
                                subfamtemplate = subfamtemplate + cssubfamilycontent;
                            }
                            else
                            {
                                cssubfamilycontent = "";
                            }
                        }
                    }
                    templatename = "CSFAMILYPAGELOGO";
                    tbwtEngine = new TBWTemplateEngine(templatename, HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
                   tbwtEngine.RenderHTML("Row");
                   if (subfamtemplate != "")
                   {
                       subfamtemplate = "<div style='width:98%;margin:0 auto; background-color: #F0F0F0; border: 1px solid #C8C8C8;color: #646464;font-size: 11px;height: 22px;margin-bottom: -10px;padding-top: 8px;text-indent: 10px;margin-top :10px;'>Related Products</div>" + subfamtemplate;
                       //CScontentvalue
                       //subfamtemplate +   + "</br>
                       //contentvalue = contentvalue + "<div>" + CScontentvalue.Replace("<tr></tr>","<tr><td height=10></td></tr>") + "</div>" + subfamtemplate;
                       contentvalue = contentvalue  + CScontentvalue.Replace("<tr></tr>", "<tr><td height=10></td></tr>") + subfamtemplate;
                           //+ tbwtEngine.RenderedHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
                   }
                   else
                   {
                       contentvalue = contentvalue +
                           "<div style='width:100%;margin-left:2px;padding-top: 10px;'><div>" + CScontentvalue.Replace("<tr></tr>", "<tr><td height=10></td></tr>") + "</div>" + subfamtemplate + "</div>";
                   }

                  
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return e.Message;
            }
        }
        catch
        { }
        //return objHelperServices.StripWhitespace(contentvalue.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>"));
        return contentvalue.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");

    }

    public DataSet ConvertXMLToDataSet(string xmlData)
    {
        StringReader stream = null;
        XmlTextReader reader = null;
        try
        {
            DataSet xmlDS = new DataSet();
            stream = new StringReader(xmlData);
            // Load the XmlTextReader from the stream
            reader = new XmlTextReader(stream);
            xmlDS.ReadXml(reader);
            return xmlDS;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (reader != null) reader.Close();
        }
    }

    public string getleftattribute(string _fid)
    {
        string x = "";
        try
        {
            HelperDB objHelperDb = new HelperDB();
            DataSet tmpds1 = objHelperDb.GetDataSetDB("exec [STP_GETFAMILY_XML] " + _fid);
           // objErrorHandler.CreateLog("exec [STP_GETFAMILY_XML] " + _fid);
            DataSet dsmrgattr = new DataSet();
            if (tmpds1 != null && tmpds1.Tables[0].Rows.Count > 0)
            {

                dsmrgattr = ConvertXMLToDataSet(tmpds1.Tables[0].Rows[0][0].ToString());

                //DataRow[] dr = dsmrgattr.Tables["LeftRowField"].Select("Merge='Checked'");
                //if (dr.Length > 0)
                //{
                 if ((dsmrgattr != null) && (dsmrgattr.Tables.Count > 0))
                    {
                        //  DataTable dt = dr.CopyToDataTable();
                        DataTable dt = dsmrgattr.Tables["LeftRowField"];
                        //   string x = string.Empty;
                        if (dt != null)
                        {
                            //   string x = string.Empty;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if( dt.Rows[i]["AttrId"]!=null)
                                {
                                     string[] a = dt.Rows[i]["AttrId"].ToString().Split(':');
                                    if (a.Length > 0)
                                    {
                                if (i == 0)
                                {
                                    // x = dt.Rows[i]["AttrId"].ToString().Replace("Attr:", "");

                                    x = a[1];
                                    }
                                else
                                {
                                    x = x + "," + a[1];
                                }
                                }



                               
                                }


                            }
                        }

                        DataTable dtsummary = dsmrgattr.Tables["SummaryField"];

                        if (dtsummary != null)
                        {
                            for (int i = 0; i < dtsummary.Rows.Count; i++)
                            {
                                if (dtsummary.Rows[i]["AttrId"] != null)
                                {
                                    string[] a = dtsummary.Rows[i]["AttrId"].ToString().Split(':');
                                    if (a.Length > 0)
                                    {
                                        if (x == "")
                                        {
                                            // x = dt.Rows[i]["AttrId"].ToString().Replace("Attr:", "");

                                            x = a[1];
                                        }
                                        else
                                        {
                                            x = x + "," + a[1];
                                        }
                                    }




                                }


                            }
                        }

                        DataTable dtrightrow = dsmrgattr.Tables["RightRowField"];

                        if (dtrightrow != null)
                        {
                            for (int i = 0; i < dtrightrow.Rows.Count; i++)
                            {
                                if (dtrightrow.Rows[i]["AttrId"] != null)
                                {
                                    string[] a = dtrightrow.Rows[i]["AttrId"].ToString().Split(':');
                                    if (a.Length > 0)
                                    {
                                        if (x == "")
                                        {
                                            // x = dt.Rows[i]["AttrId"].ToString().Replace("Attr:", "");

                                            x = a[1];
                                        }
                                        else
                                        {
                                            x = x + "," + a[1];
                                        }
                                    }




                                }


                            }
                        }
                    DataTable dtColumnField = dsmrgattr.Tables["ColumnField"];

                    if (dtColumnField != null)
                    {
                        for (int i = 0; i < dtColumnField.Rows.Count; i++)
                        {
                            if (dtColumnField.Rows[i]["AttrId"] != null)
                            {
                                string[] a = dtColumnField.Rows[i]["AttrId"].ToString().Split(':');
                                if (a.Length > 0)
                                {
                                    if (x == "")
                                    {
                                        // x = dt.Rows[i]["AttrId"].ToString().Replace("Attr:", "");

                                        x = a[1];
                                    }
                                    else
                                    {
                                        x = x + "," + a[1];
                                    }
                                }




                            }


                        }
                    }
                    ///  objErrorHandler.CreateLog("x=" + x); 
                    //   dssimilarcolumns = objHelperDb.GetDataSetDB("exec [STP_Attribute_name] '" + x + "'");
                    //objErrorHandler.CreateLog("exec [STP_Attribute_name] '" + x + "'" + "------" + _familyID);
                    //dsgetleftattr = objHelperDb.GetDataSetDB("exec [STP_Attribute_name] '" + x + "'");
                }

            }
        }
        catch (Exception ex)
        {
            objErrorHandler.CreateLog(ex.ToString());
        }

        return x;
    }
    public int GetFamilyAllData(string _Fid)
    {

        try
        {
            //have to get the id
            EADs = (DataSet)HttpContext.Current.Session["FamilyProduct"];

            DataSet tempDs = new DataSet();
            DataTable Ftb = new DataTable();


            DataTable Atttbl = new DataTable();

            DataRow[] DrMain = null;
            DataRow[] Drsub = null;
            DataRow[] Dr = null;
            string _UserID = string.Empty;

            string _cid = string.Empty;
            string _pcr = string.Empty;

            if (HttpContext.Current.Request.QueryString["fid"] != null)
                _Fid = HttpContext.Current.Request.QueryString["fid"].ToString();
            if (HttpContext.Current.Request.QueryString["cid"] != null)
                _cid = HttpContext.Current.Request.QueryString["cid"].ToString();
            if (HttpContext.Current.Request.QueryString["pcr"] != null)
                _pcr = HttpContext.Current.Request.QueryString["pcr"].ToString();

            if (HttpContext.Current.Session["USER_ID"].ToString() != null)
                _UserID = HttpContext.Current.Session["USER_ID"].ToString();


            if (_UserID == "" || _UserID == null)
                _UserID = "0";

            if (HttpContext.Current.Request.QueryString["fid"] != null)
            {
                hffid.Value = HttpContext.Current.Request.QueryString["fid"];
                HttpContext.Current.Session["pfid"] = hffid.Value;
            }
            if (HttpContext.Current.Request.QueryString["path"] != null)
            {
                //hfpath.Value = HttpContext.Current.HttpContext.Current.Request.QueryString["path"];
                hfpath.Value = HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["path"].ToString());
            }

            string ExecString = string.Empty;



            int tblinx = 0;


            _Familyids = _Fid;
            string x = "";
          
            if (EADs != null)
            {


//Modified by indu 11-Dec-2017
                //For family attribute table designer like
               // ExecString = "exec STP_TBWC_PICKFAMILYPAGEPRODUCT '" + _Fid + "','PRODUCT'";
                x = getleftattribute(_Fid);
                if (x != "")
                {
                    ExecString = "exec STP_TBWC_PICKFAMILYPAGEPRODUCT_TABLEDESIGNER '" + _Fid + "','PRODUCT','" + x + "'";

                   // objErrorHandler.CreateLog(ExecString);
                }
                else
                {
                    ExecString = "exec STP_TBWC_PICKFAMILYPAGEPRODUCT '" + _Fid + "','PRODUCT'";
                }

                DrMain = EADs.Tables["FamilyPro"].Select("FAMILY_ID='" + _Fid + "'");

                Drsub = EADs.Tables["FamilyPro"].Select("FAMILY_ID<>'" + _Fid + "'");
                if (Drsub.Length > 0)
                {
                    SFamtb = Drsub.CopyToDataTable().DefaultView.ToTable(true, "FAMILY_ID").Copy();
                    foreach (DataRow dr in SFamtb.Rows)
                    {

                        x = getleftattribute(dr["FAMILY_ID"].ToString());
                        if (x != "")
                        {
                            ExecString = ExecString + ";exec STP_TBWC_PICKFAMILYPAGEPRODUCT_TABLEDESIGNER '" + dr["FAMILY_ID"].ToString() + "','PRODUCT','" + x + "'";

                          
                        }
                        else
                        {
                            ExecString = ExecString + ";exec STP_TBWC_PICKFAMILYPAGEPRODUCT '" + dr["FAMILY_ID"].ToString() + "','PRODUCT'";
                        
                        }

                        _Familyids = _Familyids + "," + dr["FAMILY_ID"].ToString();
                        _Familyids = _Familyids.Replace(",,", ",");
                    }

                }

                //ExecString=ExecString+";exec STP_TBWC_PICKFPRODUCTPRICE '"+ _Familyids +"','','"+_UserID+"'";

                //ExecString=ExecString+";exec STP_TBWC_PICKGENERICDATA '2',"+EADs.Tables[0].Rows[0]["Family_id"].ToString()+",'2','','','GET_FAMILY_ATTRIBUTE'";
                //objErrorHandler.CreateLog(ExecString);
                ExecString = ExecString + ";exec STP_TBWC_PICKFAMILYPAGEPRODUCT '" + _Familyids + "','ATTACHMENT'";




                string tmpProds = string.Empty;
                if (Convert.ToInt32(_UserID) > 0)
                {
                    foreach (DataRow drpid in EADs.Tables["FamilyPro"].Rows)
                    {
                        tmpProds = tmpProds + drpid["PRODUCT_ID"].ToString() + ",";
                        tmpProds = tmpProds.Replace(",,", ",");
                    }
                    if (tmpProds != "")
                    {
                        tmpProds = tmpProds.Substring(0, tmpProds.Length - 1);

                        dsPriceTableAll = objHelperDB.GetProductPriceTableAll(tmpProds, Convert.ToInt32(_UserID));
                    }
                }


                Dsall = objHelperDB.GetDataSetDB(ExecString);

                if (Dsall == null && Dsall.Tables.Count <= 0)
                    return -1;


                // Main fl
                DrMain = EADs.Tables["FamilyPro"].Select("FAMILY_ID='" + _Fid + "'");
                if (DrMain.Length > 0)
                {
                    // Famtb = Dr.CopyToDataTable();
                    ConstructFamilyDatajson(_Fid, DrMain, Dsall, tblinx);
                }
                // Sub fl


                if (Drsub.Length > 0)
                {

                    if (SFamtb != null)
                    {
                        for (int i = 0; i <= SFamtb.Rows.Count - 1; i++)
                        {
                            Dr = EADs.Tables["FamilyPro"].Select("FAMILY_ID='" + SFamtb.Rows[i]["FAMILY_ID"].ToString() + "'");
                            if (Dr.Length > 0)
                            {
                                // Ftb = Dr.CopyToDataTable();
                                tblinx = tblinx + 1;
                                ConstructFamilyDatajson(SFamtb.Rows[i]["FAMILY_ID"].ToString(), Dr, Dsall, tblinx);
                            }
                            //_Familyids = _Familyids + "," + SFamtb.Rows[i]["FAMILY_ID"].ToString();
                            //_Familyids = _Familyids.Replace(",,", ",");
                        }

                    }

                }


                return 1;


            }

            return -1;
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();
            return -1;
        }
    }
    public string ST_BulkBuyPP()
    {
        StringTemplateGroup _stg_container = null;
        StringTemplate _stmpl_container = null;
        StringTemplateGroup _stg_records1 = null;
        StringTemplate _stg_container_records1 = null;
        try
        {

            _stg_container = new StringTemplateGroup("main", stemplatepath);

            _stmpl_container = _stg_container.GetInstanceOf("Familypage\\BulkBuyPP");


            _stg_records1 = new StringTemplateGroup("row", stemplatepath);
            // _stg_container_records1 = _stg_records1.GetInstanceOf("Familypage" + "\\" + "multilistitem");
            string shtml = string.Empty;
            if (HttpContext.Current.Session["prodcodedesc"] != null)
            {
                string codedescall = HttpContext.Current.Session["prodcodedesc"].ToString();
                string[] codedesc = codedescall.Split('|');
                for (int i = 0; i < codedesc.Length - 1; i++)
                {
                    _stg_container_records1 = _stg_records1.GetInstanceOf("Familypage\\multilistitem");
                    string prodcode = string.Empty;
                    prodcode = codedesc[i].Trim();
                    //_stg_container_records1.SetAttribute("TBW_LIST_VAL", prodcode);
                    //if (i == 0)
                    //    _stg_container_records1.SetAttribute("TBW_SELECTED", "SELECTED");

                    if (codedesc.Length > 2)
                    {
                        _stg_container_records1.SetAttribute("TBW_LIST_VAL", prodcode);
                        _stmpl_container.SetAttribute("TBT_CHK_PRODCOUNT", true);
                    }
                    else
                    {
                        _stg_container_records1.SetAttribute("TBW_LIST_VAL", prodcode);
                        _stg_container_records1.SetAttribute("TBW_SELECTED", "SELECTED");
                        _stmpl_container.SetAttribute("TBT_CHK_PRODCOUNT", false);
                    }


                    //if (i == 0)
                    //{
                    //    _stg_container_records1.SetAttribute("TBW_LIST_VAL", "Please Select Product");
                    //    _stg_container_records1.SetAttribute("TBW_SELECTED", "SELECTED");
                    //}
                    //else
                    //    _stg_container_records1.SetAttribute("TBW_LIST_VAL", prodcode);

                    shtml = shtml + _stg_container_records1.ToString();
                }
                if (HttpContext.Current.Session["AQ_FL_CAPTCH_IMAGE"] != null)
                {
                    _stmpl_container.SetAttribute("CAPTCHA_IMAGE", HttpContext.Current.Session["AQ_FL_CAPTCH_IMAGE"].ToString());
                    _stmpl_container.SetAttribute("CC_CODE", HttpContext.Current.Session["AQ_FL_CAPTCH_VALUE"].ToString());
                }
                // _stmpl_container.SetAttribute("TBW_DDL_VALUE", _stg_container_records1.ToString());
                _stmpl_container.SetAttribute("TBW_DDL_VALUE", shtml.ToString());
            }
            else
            {
                Response.Redirect("home.aspx");
            }
            return _stmpl_container.ToString();
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
            objErrorHandler.CreateLog();

        }
        return "";
    }
    public string ST_Downloads_Update(bool chkdwld)
    {
        StringTemplateGroup _stg_container = null;
        StringTemplate _stmpl_container = null;
        try
        {

            _stg_container = new StringTemplateGroup("main", stemplatepath);

            //if (chkdwld == false)
                _stmpl_container = _stg_container.GetInstanceOf("Familypage\\DowloadUpdate");
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

    public string ST_VPC()
    {
        string vpchref = string.Empty;
        string eapath = string.Empty;
        
         Security objSecurity = new Security();
        try
        {
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
                        vpchref = "";
                        eapath = "";
                        ctname = "";
                        vpchref = dr["Url"].ToString();
                        eapath = dr["EAPath"].ToString();
                        ctname = dr["ItemValue"].ToString();    
                    }
                }
                eapath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath));
                vpchref = vpchref + "&path=" + eapath;
            }
        }
        catch (Exception e)
        {
            objErrorHandler.ErrorMsg = e;
            objErrorHandler.CreateLog();
            return e.Message;
        }
        return vpchref;
    }


    public string Dynamic_pagination(string pagno, string _fid, string eapath, string Rawurl)
    {

        Get_Value_Breadcrum(pagno, _fid, eapath, Rawurl);
        //int rtn = 1;
        int rtn = GetFamilyAllData(_fid);
        return ST_Familypage(_fid, Rawurl);

    }
    public void Get_Value_Breadcrum(string pagno, string _fid, string eapath, string Rawurl)
    {
        EasyAsk_WES EasyAsk = new EasyAsk_WES(); 

        //if (HttpContext.Current.Session["EA"] != null)
        //{
        //    if (HttpContext.Current.Session["EA"].ToString().Contains(_fid))
        //    {

                EasyAsk.GetAttributeProducts("FamilyPage", "", "FamilyId", _fid, "", iRecordsPerPage.ToString(), pagno, "Next");
            //}
            //else
            //{

             
            //    EasyAsk.GetAttributeProducts("FamilyPage", "", "FamilyId", _fid, "", iRecordsPerPage.ToString(), pagno, "Next");
            //}
        //}

    }

    public string ST_Familypage(string fid, string Rawurl)
    {

        if (fid == "")
        {
            fid = hffid.Value;
           
                
                if (HttpContext.Current.Session[fid + "Icnt"] != null)
                {
                    itotalrecords.Value = HttpContext.Current.Session[fid + "Icnt"].ToString();
                    if (Convert.ToInt16(HttpContext.Current.Session[fid + "Icnt"].ToString()) > 1)
                    {
                        hfcheckload.Value = "1";



                    }
                    else
                    {
                        hfcheckload.Value = "0";
                    }
                }
            
        }
      


        ////have to get the id
        //DataSet EADs = (DataSet)HttpContext.Current.Session["FamilyProduct"];
        
        //DataSet tempDs = new DataSet();        
        //DataTable Ftb = new DataTable();
        

        //DataTable Atttbl = new DataTable();

        //DataRow[] Dr = null;


        string _UserID = string.Empty;

        string _cid = string.Empty;
        string _pcr = string.Empty;
        _Fid = fid;
        if (HttpContext.Current.Request.QueryString["fid"]!=null)
        _Fid=HttpContext.Current.Request.QueryString["fid"].ToString();
        if (HttpContext.Current.Request.QueryString["cid"] != null)
            _cid = HttpContext.Current.Request.QueryString["cid"].ToString();
        if (HttpContext.Current.Request.QueryString["pcr"] != null)
            _pcr = HttpContext.Current.Request.QueryString["pcr"].ToString();
        
        if ( HttpContext.Current.Session["USER_ID"].ToString()!=null)
        _UserID= HttpContext.Current.Session["USER_ID"].ToString();

        if (HttpContext.Current.Request.QueryString["fid"] != null)
        {
            hffid.Value = HttpContext.Current.Request.QueryString["fid"];
            HttpContext.Current.Session["pfid"] = hffid.Value;
        }
        if (HttpContext.Current.Request.QueryString["path"] != null)
        {
            //hfpath.Value = HttpContext.Current.Request.QueryString["path"];
            hfpath.Value = HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["path"].ToString());
        }
      //  Ds = new DataSet(); 

        _Familyids =_Fid;



        HttpContext.Current.Session["prodcodedesc"] = null;
        try
        {
            contentvalue = "";


           







            if (_Fid != null)
            {
                //CScontentvalue = getcstable(Request.QueryString["fid"].ToString());
                //by jtech
                #region comments
               
                #endregion
                //by jtech
                CScontentvalue = ObjFamilyPage.GenerateHorizontalHTMLJson(_Fid, Ds, dsPriceTableAll, EADs);

                if (Convert.ToInt16(HttpContext.Current.Session[fid + "Icnt"].ToString()) > 1)
                {
                    if (HttpContext.Current.Request.QueryString["fid"] != null)
                    {
                        CScontentvalue = CScontentvalue + " <div  class=\"lodder0\" align=\"center\"> </div>";
                    }

                }
                
               
                if (Famtb.Rows.Count == 1 && SFamtb.Rows.Count == 0 && (HttpContext.Current.Request.QueryString["ProductResult"] != null && HttpContext.Current.Request.QueryString["ProductResult"].ToString().Equals("SUCCESS")))
                {
                    Response.Redirect("productdetails.aspx?&pid=" + Famtb.Rows[0]["Product_ID"].ToString() + "&fid=" + HttpContext.Current.Request.QueryString["fid"].ToString() + "&cid=" + _cid + "&pcr=" + _pcr +"byp=2", true);
                    return "";
                }
                else if (SFamtb != null)
                {
                    string subfamproduct = string.Empty;
                    if (HttpContext.Current.Session["prodcodedesc"] != null)
                        subfamproduct = HttpContext.Current.Session["prodcodedesc"].ToString();

                    foreach (DataRow DR in SFamtb.Rows)
                    {
                        string cssubfamilycontent="";
                        //cssubfamilycontent = getcstable(DR["Family_id"].ToString());



                         bool BindToST = objHelperDB.CheckFamilyPAGE_Discontinued(DR["Family_id"].ToString());
                         if (BindToST == true)
                         {
                             cssubfamilycontent = ObjFamilyPage.GenerateHorizontalHTMLJson(DR["Family_id"].ToString(), Ds, dsPriceTableAll, EADs);
                         }
                            if (cssubfamilycontent != "" && cssubfamilycontent.Length > 336)
                        {
                            templatename = "CSFAMILYPAGEWITHSUBFAMILY";
                            tbwtEngine = new TBWTemplateEngine(templatename,HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
                            tbwtEngine.paraValue = DR["Family_id"].ToString();
                            tbwtEngine.paraFID = DR["Family_id"].ToString();
                            //tbwtEngine.RenderHTML("Row");
                            //subfamtemplate = subfamtemplate + tbwtEngine.RenderedHTML;
                            //subfamtemplate = subfamtemplate + cssubfamilycontent;
                            subfamtemplate = subfamtemplate + tbwtEngine.ST_SubFamily_Load(EADs);
                            subfamtemplate = subfamtemplate + cssubfamilycontent;
                        }
                        else
                        {
                            cssubfamilycontent = "";
                        }
                        subfamproduct = subfamproduct + HttpContext.Current.Session["prodcodedesc"].ToString();
                    }
                    HttpContext.Current.Session["prodcodedesc"] = subfamproduct;
                }
                /*templatename = "CSFAMILYPAGELOGO";
                tbwtEngine = new TBWTemplateEngine(templatename,HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
                tbwtEngine.RenderHTML("Row");
                if (subfamtemplate != "")
                    subfamtemplate = "<div class=\"title7\">Related Products</div>" + subfamtemplate;
                //contentvalue = contentvalue + "<div style=\"overflow:auto; width:780px; height:100%;\">" + CScontentvalue + subfamtemplate + "</div>" + tbwtEngine.RenderedHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
                contentvalue = contentvalue + "<div style=\" width:100%; height:100%;\"><div class=\"fpscroll\">" + CScontentvalue + "</div>" + subfamtemplate + "</div>" + tbwtEngine.RenderedHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
                */
                if (subfamtemplate != "")
                    subfamtemplate = "<div class=\"title7\">Related Products</div>" + subfamtemplate;
                //contentvalue = contentvalue + "<div style=\"overflow:auto; width:780px; height:100%;\">" + CScontentvalue + subfamtemplate + "</div>" + tbwtEngine.RenderedHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
               // contentvalue = contentvalue + "<div style=\" width:760px; height:100%;\"><div class=\"fpscroll\">" + CScontentvalue + "</div>" + subfamtemplate + "</div>"; //+ tbwtEngine.RenderedHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");
                contentvalue = contentvalue + "<div style=\" height:100%;\"><div>" + CScontentvalue + "</div>" + subfamtemplate + "</div>"; 
      
            }
        }
        catch (Exception e)
        {
            objErrorHandler.ErrorMsg =e;
            objErrorHandler.CreateLog();
            return e.Message; 
        }
        //return objHelperServices.StripWhitespace(contentvalue.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>"));
        return contentvalue.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");

    }
    // by Jtech
    //private DataSet GetDataSetX(string category_id)
    //{

    //    DataSet catid = new DataSet();
    //    SqlDataAdapter da = new SqlDataAdapter("select convert(int, option_value) as OPTION_VALUE from tbwc_store_options where option_name = 'DEFAULT CATALOG'", conStr.ConnectionString.ToString().Substring(conStr.ConnectionString.ToString().IndexOf(';') + 1));
    //    da.Fill(catid, "generictable");
    //    int[] AttributeIdList = new int[0];
    //    CatalogXfunction oProdTable = new CatalogXfunction();
    //    DataSet dds = oProdTable.WebCatalogFamily(Convert.ToInt32(catid.Tables[0].Rows[0].ItemArray[0].ToString()), 178966, AttributeIdList, conStr.ConnectionString.ToString().Substring(conStr.ConnectionString.ToString().IndexOf(';') + 1));
    //    return dds;
    //}

    //private DataSet GetDataSetFX(string familyid)
    //{
    //    DataSet prodtable = new DataSet();
    //    SqlDataAdapter da = new SqlDataAdapter("select distinct tps.PRODUCT_ID from tb_prod_specs tps,tb_prod_family tpf, tbwc_inventory ti where ti.product_id= tps.product_id and ti.product_status <> 'DISABLE' and tps.product_id=tpf.product_id and tpf.family_id=" + familyid, conStr.ConnectionString.ToString().Substring(conStr.ConnectionString.ToString().IndexOf(';') + 1));
    //    da.Fill(prodtable, "Producttable");
    //    DataSet subfamtable = new DataSet();
    //    DataTable DT = new DataTable("generictable");
    //    da = new SqlDataAdapter("select sf.family_id,sf.subfamily_id from tb_subfamily sf,tb_catalog_family cf where cf.family_id=sf.subfamily_id and cf.catalog_id=" + oHelper.GetOptionValues("DEFAULT CATALOG").ToString() + " and  sf.family_id=" + familyid, conStr.ConnectionString.ToString().Substring(conStr.ConnectionString.ToString().IndexOf(';') + 1));
    //    da.Fill(DT);
    //    prodtable.Tables.Add(DT);
    //    return prodtable;
    //}

    //private DataSet GetDataSetSFX(string familyid)
    //{
    //    DataSet prodtable = new DataSet();
    //    SqlDataAdapter da = new SqlDataAdapter("select distinct tps.PRODUCT_ID from tb_prod_specs tps,tb_prod_family tpf, tbwc_inventory ti where ti.product_id = tps.product_id and ti.product_status <> 'DISABLE' and tps.product_id=tpf.product_id and tpf.family_id=" + familyid, conStr.ConnectionString.ToString().Substring(conStr.ConnectionString.ToString().IndexOf(';') + 1));
    //    da.Fill(prodtable, "Producttable");
    //    return prodtable;
    //}

    
    private DataSet GetFamilyPageProduct(string familyid, string Option)
    {
        return objFamilyServices.GetFamilyPageProduct(familyid, Option);
    }
    private DataSet GetProductPrice(string familyids, string productids, string UserID)
    {
        DataSet ds = new DataSet();
        SqlCommand objSqlCommand;
        SqlDataAdapter da;
        //SqlConnection Gcon = new SqlConnection();
        //Gcon.ConnectionString = conStr.ConnectionString.ToString().Substring(conStr.ConnectionString.ToString().IndexOf(';') + 1);
        try
        {
            objSqlCommand = new SqlCommand("STP_TBWC_PICKFPRODUCTPRICE", objConnectionDB.GetConnection() );
            objSqlCommand.CommandType = CommandType.StoredProcedure;
            objSqlCommand.Parameters.Add("@FamilyIDs", familyids);
            objSqlCommand.Parameters.Add("@ProductIDs", productids);
            objSqlCommand.Parameters.Add("@UserID", UserID);
            da = new SqlDataAdapter(objSqlCommand);
            da.Fill(ds);
        }
         
        catch (Exception e)
        {
            objErrorHandler.ErrorMsg = e;
            objErrorHandler.CreateLog();
        }
        finally
        {
            objSqlCommand = null;
            da = null;
        }

        return ds;
    }
    //public string getcstable(string familyid)
    //{
    //    CSProductTable oCSProdTab = new CSProductTable(familyid, oHelper.GetOptionValues("DEFAULT CATALOG"));
    //    oCSProdTab.UserID = oHelper.CI(Session["USER_ID"]);
    //    oCSProdTab.DesciptionAttributeWidth = 300;
    //    oCSProdTab.DesciptionHighAttributeWidth = 180;
    //    oCSProdTab.DesciptionMidumAttributeWidth = 80;
    //    oCSProdTab.DesciptionNormalAttributeWidth = 440;
    //    oCSProdTab.ProductImgHeight = 100;
    //    oCSProdTab.ProductImgWidth = 100;
    //    oCSProdTab.IsVisibleShipping = true;
    //    return (oCSProdTab.GenerateFamilyPreview().Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">&rarr;</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>"));
    //}
    public string Generateparentfamilyhtml_print(string fid,string withprice, string withdetails,DataSet DsFamilyproduct,string cellname)
    {
        string shtml = string.Empty;
        try
        {
            contentvalue = "";
            

                templatename = "CSFAMILYPAGE";
                EasyAsk_WES EasyAsk = new EasyAsk_WES();
            DataSet dscontainer = new DataSet();

                   // EasyAsk.GetAttributeProducts("FamilyPage", "", "FamilyId", fid, "", "0", "0", "");
                  //   DataSet dscontainer = (DataSet)HttpContext.Current.Session["FamilyProduct"];
            if (DsFamilyproduct != null)
                dscontainer = DsFamilyproduct;
            else
            {
                EasyAsk.GetAttributeProducts("FamilyPage", "", "FamilyId", fid, "", "0", "0", "");
                dscontainer = (DataSet)HttpContext.Current.Session["FamilyProduct"];
            }
                
               
                //string sessionname = "FamilyProduct_" + hffid.Value.Trim();

                //DataSet dscontainer = (DataSet)HttpContext.Current.Session[sessionname];

                StringTemplate _stmpl_container = null;
                StringTemplateGroup _stg_container = null;
                string _Package = "CSFAMILYPAGE";

                foreach (DataRow dr in dscontainer.Tables[0].Rows)
                {

                    string drstatus = dr["STATUS"].ToString().ToUpper();

                    if (drstatus == "TRUE")
                    {
                        _stg_container = new StringTemplateGroup(_Package + "\\" + "main_print" + "1", HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()));
                        _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "main_print" + "1");
                    }
                    else if (drstatus == "FALSE")
                    {
                        _stg_container = new StringTemplateGroup(_Package + "\\" + "main_print", HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()));
                        _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "main_print");
                    }
                    else
                    {
                        _stg_container = new StringTemplateGroup(_Package + "\\" + "main_print" + "2", HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()));
                        _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "main_print" + "2");
                    }


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


                    
                        string _fid = fid;

                        //EasyAsk_WES EasyAsk = new EasyAsk_WES();
                        CategoryServices objCategoryServices = new CategoryServices();
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

                        }

                    }
                //if (withdetails == "true" || withdetails == "True")
                //    _stmpl_container.SetAttribute("TBT_DES_SHOW", true);
                //else
                //    _stmpl_container.SetAttribute("TBT_DES_SHOW", false);
            
                string strFile = HttpContext.Current.Server.MapPath("ProdImages");
                if (dscontainer.Tables[0].Columns.Contains("attribute_name"))
                {
                    string descall = string.Empty;
                    //string descalltrim = "";
                    string desc1 = string.Empty;
                    string descallstring = string.Empty;
                    string Att_name = string.Empty;
                    foreach (DataRow dr in dscontainer.Tables[0].Rows)
                    {
                        if (dr["ATTRIBUTE_TYPE"].ToString() == "1")
                        {
                            _stmpl_container.SetAttribute("TBT_FAMILY_NAME", dr["FAMILY_NAME"].ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;"));
                            if (cellname == "cell_pdf")
                            {
                                hfFN.Value = dr["FAMILY_NAME"].ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;");
                            }
                        }
                        desc1 = "";
                        if (dr["ATTRIBUTE_TYPE"].ToString() == "3" || dr["ATTRIBUTE_TYPE"].ToString() == "9")
                        {
                            FileInfo Fil;

                            Fil = new FileInfo(strFile + dr["STRING_VALUE"].ToString().Replace("_th", "_Images_200"));
                            if (Fil.Exists)
                            {

                                _stmpl_container.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), (dr["STRING_VALUE"].ToString().Replace("_th", "_Images_200").Replace("\n", "<br/>").Replace("\r", "&nbsp;").Replace("/", "\\")));

                            }
                            else
                            {
                                _stmpl_container.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), "/images/noimage.gif");

                            }

                        }
                        else
                        {
                            // 
                            Att_name = dr["ATTRIBUTE_NAME"].ToString().ToUpper();


                            if (Att_name == "DESCRIPTIONS" || Att_name == "FEATURES" || Att_name == "SPECIFICATION" || Att_name == "SPECIFICATIONS" || Att_name == "APPLICATIONS" || Att_name == "NOTES")
                            {
                                desc1 = dr["STRING_VALUE"].ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                                desc1 = desc1.ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;");
                            }
                            else
                                _stmpl_container.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"].ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;"));


                        }



                        if (desc1 != "")
                            descall = descall + desc1 + "<br/><br/>";


                    }

                    if (descall != "")
                    {
                        descall = descall.Trim();
                        descall = descall.Substring(0, descall.Length - 5);
                    }

                    GetFamilyMultipleImages(Convert.ToInt32(fid), _stmpl_container);

                    if (descall.Length > 1080)
                    {
                        //  _stmpl_records.SetAttribute("TBT_MORE_SHOW", true);
                        descallstring = descall.Substring(0, 1080).ToString();
                        _stmpl_container.SetAttribute("TBT_MORE", descallstring);
                        _stmpl_container.SetAttribute("TBT_MENU_ID", "2");
                        descall = descall.Substring(0, descall.Length).ToString();
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
                    {
                        _stmpl_container.SetAttribute("TBT_MORE_SHOW", true);
                    }
                    else
                    {
                        _stmpl_container.SetAttribute("TBT_MORE_SHOW", false);

                    }

                   shtml= _stmpl_container.ToString(); 


                }

            }
       
        catch
        { }
        return shtml;
            }
          

       


      
   
    public string Generateparentfamilyhtml()
    {
        try
        {
        contentvalue = "";
        if (Request.QueryString["fid"] != null && Request.QueryString["fid"].ToString() != "")
        {
            //DDS = GetDataSetFX(Request.QueryString["fid"].ToString());
            templatename = "CSFAMILYPAGE";
            tbwtEngine = new TBWTemplateEngine(templatename, Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
            if (Request.QueryString["fid"] != null)
            {
                tbwtEngine.paraValue = Request.QueryString["fid"].ToString();
                tbwtEngine.paraFID = Request.QueryString["fid"].ToString();
            }
           // tbwtEngine.RenderHTML("Row");
           // if(tbwtEngine.RenderedHTML!=null)
            //contentvalue = tbwtEngine.RenderedHTML.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");

            contentvalue = tbwtEngine.ST_Family_Load(EADs);
            if (contentvalue != null)
                contentvalue = contentvalue.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");

        }
       // return objHelperServices.StripWhitespace( contentvalue);
        return contentvalue;

    }
         
         
        catch (Exception e)
        {
            objErrorHandler.ErrorMsg = e;
            objErrorHandler.CreateLog();
            return string.Empty;  
        }

    }
    public class PdfGenerator
    {
        public static string HtmlToPdf(string pdfOutputLocation, string outputFilenamePrefix, string[] urls,
        string[] options = null,
        string pdfHtmlToPdfExePath = "C:\\Program Files\\wkhtmltopdf\\wkhtmltopdf.exe")
        {
            string urlsSeparatedBySpaces = string.Empty;
            try
            {
                //Determine inputs
                if ((urls == null) || (urls.Length == 0))
                    throw new Exception("No input URLs provided for HtmlToPdf");
                else
                    urlsSeparatedBySpaces = String.Join(" ", urls); //Concatenate URLs

                string outputFolder = pdfOutputLocation;
                string outputFilename = outputFilenamePrefix + "_" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-fff") + ".PDF"; // assemble destination PDF file name

                var p = new System.Diagnostics.Process()
                {
                    StartInfo =
                    {
                        FileName = pdfHtmlToPdfExePath,
                        Arguments = ((options == null) ? "" : String.Join(" ", options)) + " " + urlsSeparatedBySpaces + " " + outputFilename,
                        UseShellExecute = false, // needs to be false in order to redirect output
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        RedirectStandardInput = true, // redirect all 3, as it should be all 3 or none
                        WorkingDirectory = HttpContext.Current.Server.MapPath(outputFolder)
                    }
                };

                p.Start();

                // read the output here...
                var output = p.StandardOutput.ReadToEnd();
                var errorOutput = p.StandardError.ReadToEnd();

                // ...then wait n milliseconds for exit (as after exit, it can't read the output)
                p.WaitForExit(60000);

                // read the exit code, close process
                int returnCode = p.ExitCode;
                p.Close();

                // if 0 or 2, it worked so return path of pdf
                if ((returnCode == 0) || (returnCode == 2))
                    return outputFolder + outputFilename;
                else
                    throw new Exception(errorOutput);
            }
            catch (Exception exc)
            {
                throw new Exception("Problem generating PDF from HTML, URLs: " + urlsSeparatedBySpaces + ", outputFilename: " + outputFilenamePrefix, exc);
            }
        }
    }
    private void exportpdf(string strcontent)
    {

        //var pdfUrl = PdfGenerator.HtmlToPdf(pdfOutputLocation: "~/PDFs/",
        //outputFilenamePrefix: "GeneratedPDF",
        //urls: new string[] { "http://news.bbc.co.uk" });








        Document pdfDoc = new Document();

        try
        {
            PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);
            pdfDoc.Open();
            //Read string contents using stream reader and convert html to parsed conent 
            System.IO.StringReader x = new System.IO.StringReader(strcontent);

            iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
            styles.LoadStyle("imgsize", "height", "50px");
            styles.LoadStyle("imgsize", "width", "50px");
            styles.LoadStyle("divborder", "color", "red");
            styles.LoadStyle("divborder", "border", "1px");

            try
            {
                // var  parsedHtmlElements = HTMLWorker.ParseToList(x, null);
                var parsedHtmlElements = HTMLWorker.ParseToList(x, styles);
                foreach (var htmlElement in parsedHtmlElements)
                    pdfDoc.Add(htmlElement as IElement);

            }
            catch (Exception ex)
            {
                // objErrorHandler.CreateLog(strcontent);
                objErrorHandler.CreateLog(ex.ToString());
                Response.Redirect(Session["PageUrl"].ToString());
            }
            //Get each array values from parsed elements and add to the PDF document 

            //Close your PDF 
            pdfDoc.Close();

            HttpContext.Current.Response.ContentType = "application/pdf";
            string fid = Request.QueryString["fid"].ToString();
            //Set default file Name as current datetime 
            string filename = "attachment;filename=" + fid + ".pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", filename);
            System.Web.HttpContext.Current.Response.Write(pdfDoc);

            HttpContext.Current.Response.Flush();

        }
        catch (Exception ex)
        {
            objErrorHandler.CreateLog(ex.ToString());
        }


    }
    protected void pdfbtn_Click(object sender, EventArgs e)
    {





        //string urlToConvert = Session["PageUrl"].ToString();

        //string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);

        ////urlToConvert = baseUrl + urlToConvert;
        //urlToConvert = "https://wes.com.au" + urlToConvert;
        //PdfConverter pdfConverter = new PdfConverter();
        //pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4;
        //pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal;
        //pdfConverter.PdfDocumentOptions.PdfPageOrientation = PDFPageOrientation.Portrait;
        //pdfConverter.PdfDocumentInfo.AuthorName = " HTML to PDF Converter";
        //byte[] pdfBytes = pdfConverter.GetPdfBytesFromUrl(urlToConvert);

        //// send the PDF document as a response to the browser for download
        //System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
        //response.Clear();
        //response.AddHeader("Content-Type", "binary/octet-stream");
        //response.AddHeader("Content-Disposition",
        //    "attachment; filename=ConversionResult.pdf; size=" + pdfBytes.Length.ToString());
        //response.Flush();
        //response.BinaryWrite(pdfBytes);
        //response.Flush();
        //response.End();


       // string user_id = "";
        string WithPrice = chkpricepdf.Checked.ToString().ToLower();
        string Details = chkdetailpdf.Checked.ToString().ToLower();
        //string Details = "true";
        string fid = string.Empty;
        //if (Request.QueryString["fid"] != null)
        //{
        //    fid = Request.QueryString["fid"].ToString();
        //}

        if (hffid.Value != "")
            fid = hffid.Value;
        string strcontents = Construct_ST(WithPrice, Details, "cell_pdf", fid, hfpath.Value);
        string newimagepath = HttpContext.Current.Server.MapPath("prodimages");
        string newnoimage = HttpContext.Current.Server.MapPath("images/noimage.gif");
        strcontents = strcontents.Replace("prodimages", newimagepath).Replace("images/noimage.gif", newnoimage);
      //  objErrorHandler.CreateLog("constract_st2" + fid);
        //exportpdf(strcontents.Replace("\"", "'").Replace("src='/", "src='"));
        savehtmlandExportPDF(strcontents);
       // objErrorHandler.CreateLog("constract_st3" + fid);
        Response.Redirect(Session["PageUrl"].ToString());




    }



    private void savehtmlandExportPDF(string strContent)
    {

        try
        {

            //var contentOfPage = "<html>Whatever you are writing</html>";

            string fid = string.Empty;
            if (Request.QueryString["fid"] != null)
            {
                fid = Request.QueryString["fid"].ToString();
            }
            string user_id = string.Empty;
            if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
            {
                user_id = HttpContext.Current.Session["USER_ID"].ToString();
            }
            //string filename = "PDFs/Family" + fid + "_" + user_id + ".html";
            //   using (StreamWriter writer = new StreamWriter(HttpContext.Current.Server.MapPath(filename)))
            //{
            //    writer.WriteLine(strContent);
            //}
            //string filenamesouce = ConfigurationManager.AppSettings["PDF_SOURCEPATH_DOWN"] + "Family" + fid + "_" + user_id + ".html";
            //using (StreamWriter writer = new StreamWriter(filenamesouce))
            //{
            //    string content = strContent.Replace(@"/C:\WES WEBSITE\WESR","").Replace(@"C:\WES WEBSITE\WESR","").Replace(@"C:\WES WEBSITE","");
            //    content = "";
            //    content = "<html><body><table><tr><td>this is sample </tr></td></table></body></html>";

            //    writer.WriteLine(content);
            //}

            string familyname = string.Empty;
            if (hfFN.Value != "")
            {
                familyname = hfFN.Value;
                familyname = familyname.Replace(" ", "-").Replace("  ", "-").Replace("\"", "_").ToString();
            }
            else
                familyname = fid;


            string titlecase = string.Empty;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            titlecase = textInfo.ToTitleCase(familyname.ToLower());

            string new_filname = string.Empty;
            new_filname = titlecase;

            var pechkin = Factory.Create(new GlobalConfig());
            var pdf = pechkin.Convert(new ObjectConfig()
                                    .SetLoadImages(true).SetZoomFactor(1.5)
                                    .SetPrintBackground(true)
                                    .SetScreenMediaType(true)
                                    .SetCreateExternalLinks(true), strContent);


            Response.Clear();

            Response.ClearContent();
            Response.ClearHeaders();

            Response.ContentType = "application/pdf";
            // Response.AddHeader("Content-Disposition", string.Format("attachment;filename=" +"Family_" + fid + "_" + user_id + ".pdf; size={0}", pdf.Length));
            // Response.AddHeader("Content-Disposition", string.Format("attachment;filename=" + new_filname + ".pdf; size={0}", pdf.Length));
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename= " + new_filname + ".pdf; size={0}", pdf.Length));
            Response.BinaryWrite(pdf);

            Response.Flush();
            Response.End();


            ////byte[] fileContent = GeneratePDFFile();
            //// if (fileContent != null)
            //// {
            ////     try
            ////     {
            ////         //string fullpath = "~/FAMILY_PDF/Family" + fid + "_" + user_id + ".pdf";
            ////         //string pdf_filename = "Family" + fid + "_" + user_id + ".pdf";
            ////         //Response.ContentType = "application/pdf";
            ////         //Response.AppendHeader("Content-Disposition", "attachment; filename=" + pdf_filename);
            ////         //Response.TransmitFile(HttpContext.Current.Server.MapPath(fullpath));
            ////         //Response.Flush();


            ////         string fullpath = "~/FAMILY_PDF/Family" + fid + "_" + user_id + ".pdf";
            ////         string pdf_filename = "Family" + fid + "_" + user_id + ".pdf";
            ////         string despath = ConfigurationManager.AppSettings["PDF_DESPATH_DOWN"] + "Family" + fid + "_" + user_id + ".pdf";
            ////         Response.ContentType = "application/pdf";
            ////         Response.AppendHeader("Content-Disposition", "attachment; filename=" + pdf_filename);
            ////         Response.TransmitFile(despath);
            ////         Response.Flush();





            ////         //string pdf_filename =  fid + "_" + user_id + ".pdf";
            ////         //string fullpath = ConfigurationManager.AppSettings["PDF_SOURCEPATH_DOWN"].ToString() + pdf_filename;
            ////         //Response.ContentType = "application/pdf";
            ////         //Response.AppendHeader("Content-Disposition", "attachment; filename=" + pdf_filename);
            ////         //Response.TransmitFile(fullpath);
            ////         //Response.Flush();
            ////     }
            ////     finally
            ////     {
            ////        // File.Delete(HttpContext.Current.Server.MapPath("FAMILY_PDF/Family" + fid + "_" + user_id + ".pdf"));
            ////       //  File.Delete(HttpContext.Current.Server.MapPath("PDFs/Family" + fid + "_" + user_id + ".html"));
            ////         Response.End();
            ////     }

            //// }
      
        }
        catch (Exception ex)
        {
            objErrorHandler.CreateLog(strContent.ToString());
        }
      
    }

    private void savehtmlandExportPDF_Email(string strContent)
    {
        //var contentOfPage = "<html>Whatever you are writing</html>";

        string fid = string.Empty;
        if (Request.QueryString["fid"] != null)
        {
            fid = Request.QueryString["fid"].ToString();
        }
        string user_id = string.Empty;
        if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
        {
            user_id = HttpContext.Current.Session["USER_ID"].ToString();
        }
        string filename = "PDFs/FamilyEmail" + fid + "_" + user_id + ".html";
        //using (StreamWriter writer = new StreamWriter(Server.MapPath("PDFs/test.html")))
        using (StreamWriter writer = new StreamWriter(HttpContext.Current.Server.MapPath(filename)))
        {
            writer.WriteLine(strContent);
        }
        byte[] fileContent = GeneratePDFFile_Email(filename);
        if (fileContent != null)
        {
            try
            {
              //  string notes = txtnotes.Value.Trim();
              //  string emailid = txtemail.Value.Trim();
                string fullpath = "~/FAMILY_PDF/FamilyEmail" + fid + "_" + user_id + ".pdf";
                string pdf_filename = "FamilyEmail" + fid + "_" + user_id + ".pdf";
               // string result = objHelperServices.sendmail("Family Details", notes, emailid, Server.MapPath("FAMILY_PDF/FamilyEmail" + fid + "_" + user_id + ".pdf"));
             
            }
            finally
            {
                File.Delete(HttpContext.Current.Server.MapPath("FAMILY_PDF/FamilyEmail" + fid + "_" + user_id + ".pdf"));
                File.Delete(HttpContext.Current.Server.MapPath("PDFs/FamilyEmail" + fid + "_" + user_id + ".html"));
                //Response.End();
            }

        }
    }


    private byte[] GeneratePDFFile( )
    {
        //url of the page we would wanto convert.
        //we dont need to hard code this URL if we want to export current page
        //we could prepare the URL by using HTTPRequest object oo
        //string url = @"http://localhost:54630/PageExportPDF.aspx";
        // string url = @"file:///C:/Documents%20and%20Settings/jtechusers/Desktop/test.html";
        //  string url = HttpContext.Current.Server.MapPath("test.html");
        // string url = @"file:///C:/WES WEBSITE/Jtech Release/test.html";
        try
        {
            string user_id = string.Empty;
            string fid = string.Empty;
            if (Request.QueryString["fid"] != null)
            {
                fid = Request.QueryString["fid"].ToString();
            }
            if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
            {
                user_id = HttpContext.Current.Session["USER_ID"].ToString();
            }


            string filename = ConfigurationManager.AppSettings["PDF_DESPATH_DOWN"] + "family" + fid + "_" + user_id + ".pdf";

            string filenamesouce = ConfigurationManager.AppSettings["PDF_SOURCEPATH_DOWN"] + "family" + fid + "_" + user_id + ".html";
            string pdf_filename = string.Empty;
 
            pdf_filename = "Family" + fid + "_" + user_id + ".pdf";

            // string url = @"http://staging2.wesonline.com.au/" + "family" + fid + "_" + user_id + ".pdf";
           // string url = filenamesouce;
            string url = "http://staging2.wesonline.com.au/test.html";
            //Document pdfDoc1 = new Document(PageSize.A4, 10, 10, 10, 10);
            //PdfWriter.GetInstance(pdfDoc1, new FileStream(Server.MapPath("FAMILY_PDF/" + pdf_filename), FileMode.Create));
            //pdfDoc1.Open();
            //pdfDoc1.Add(new Paragraph("pdf page"));
            //pdfDoc1.Close();


            // string filepath = Path.Combine(HttpContext.Current.Server.MapPath("~/FAMILY_PDF"), pdf_filename);
            // string filepath = HttpContext.Current.Server.MapPath("~/FAMILY_PDF")+ @"\" + pdf_filename;

            //string filepath = HttpContext.Current.Server.MapPath("~/PDFs/Family" + fid + "_" + user_id + ".pdf");
            string filepath = filename; //HttpContext.Current.Server.MapPath("~/Family" + fid + "_" + user_id + ".pdf");
            //objErrorHandler.CreateLog(filepath);
            //variable to store pdf file content
            byte[] fileContent = null;

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            //set the executable location
            process.StartInfo.FileName = ConfigurationManager.AppSettings["PDF_EXE_PATH"]; //Path.Combine(HttpContext.Current.Server.MapPath("~/PDFConverter"), "wkhtmltopdf.exe");
            //set the arguments to the exectuable
          //  objErrorHandler.CreateLog(process.StartInfo.FileName);
           // objErrorHandler.CreateLog(url + " " + filename);
            // wkhtmltopdf [OPTIONS]... <input fileContent> [More input fileContents] <output fileContent>
            process.StartInfo.Arguments = url + " " + filename;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;
            //run the executable
            process.Start();

            //wait until the conversion is done
            process.WaitForExit();

            // read the exit code, close process    
            int returnCode = process.ExitCode;
           // objErrorHandler.CreateLog(returnCode.ToString());
            process.Close();

            // initialize the filestream with filepath
            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
           // objErrorHandler.CreateLog(filepath);
            fileContent = new byte[(int)fs.Length];

            //read the content
            fs.Read(fileContent, 0, (int)fs.Length);

            //close the stream
            fs.Close();

            return fileContent;




          ////  byte[] fileContent = null;

          ////  System.Diagnostics.Process process = new System.Diagnostics.Process();
          ////  process.StartInfo.UseShellExecute = false;
          ////  process.StartInfo.CreateNoWindow = true;

          ////  //set the executable location
          ////  process.StartInfo.FileName = ConfigurationManager.AppSettings["PDF_EXE_PATH"];
          ////  //set the arguments to the exectuable
          //////  objErrorHandler.CreateLog(process.StartInfo.FileName);
          //// // objErrorHandler.CreateLog(filenamesouce + " " + filename);
          ////  // wkhtmltopdf [OPTIONS]... <input fileContent> [More input fileContents] <output fileContent>
          ////  process.StartInfo.Arguments = filenamesouce + " " + filename;
          ////  process.StartInfo.RedirectStandardOutput = true;
          ////  process.StartInfo.RedirectStandardError = true;
          ////  process.StartInfo.RedirectStandardInput = true;
          ////  //run the executable
          ////  process.Start();

          ////  //wait until the conversion is done
          ////  process.WaitForExit();

          ////  // read the exit code, close process    
          ////  int returnCode = process.ExitCode;
          ////  objErrorHandler.CreateLog(returnCode.ToString());
          ////  process.Close();


          ////  FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
          ////  objErrorHandler.CreateLog(filename);
          ////  fileContent = new byte[(int)fs.Length];
          ////  //read the content
          ////  fs.Read(fileContent, 0, (int)fs.Length);
          ////  //close the stream
          ////  fs.Close();
          ////  return fileContent;





            ////////////string filename1 = @"PDFs\Family" + fid + "_" + user_id + ".html";

            ////////////  string url = HttpContext.Current.Server.MapPath(filename);

            //////////// string url = HttpContext.Current.Server.MapPath(@"PDFs\a.ht");
            ////////////  string url = HttpContext.Current.Server.MapPath(@"PDFs\Family" + fid + "_" + user_id + ".html");
            ////////////string url = @"C:\\WES WEBSITE\WESR\PDFs\Family" + fid + "_" + user_id + ".html";



           //////// // string url = @"file:///C:/WES WEBSITE/Jtech Release/test.html";

           //////// if (Request.QueryString["fid"] != null)
           //////// {
           ////////     fid = Request.QueryString["fid"].ToString();
           //////// }
           //////// string pdf_filename = string.Empty;
           //////// //if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
           //////// //{
           //////// //    user_id = HttpContext.Current.Session["USER_ID"].ToString();
           //////// //}

           //////// pdf_filename = "Family" + fid + "_" + user_id + ".pdf";

           //////// // string url = @"http://staging2.wesonline.com.au/pdfs/" +fid + "_" + user_id + ".html";
           //////// string url = "staging2.wesonline.com.au/test.html";

           //////// //Document pdfDoc1 = new Document(PageSize.A4, 10, 10, 10, 10);
           //////// //PdfWriter.GetInstance(pdfDoc1, new FileStream(Server.MapPath("FAMILY_PDF/" + pdf_filename), FileMode.Create));
           //////// //pdfDoc1.Open();
           //////// //pdfDoc1.Add(new Paragraph("pdf page"));
           //////// //pdfDoc1.Close();


           //////// // string filepath = Path.Combine(HttpContext.Current.Server.MapPath("~/FAMILY_PDF"), pdf_filename);
           //////// // string filepath = HttpContext.Current.Server.MapPath("~/FAMILY_PDF")+ @"\" + pdf_filename;

           //////// //string filepath = HttpContext.Current.Server.MapPath("~/PDFs/Family" + fid + "_" + user_id + ".pdf");
           //////// string filepath = HttpContext.Current.Server.MapPath("~/Family" + fid + "_" + user_id + ".pdf");
           //////// objErrorHandler.CreateLog(filepath);
           //////// //variable to store pdf file content
           //////// byte[] fileContent = null;

           //////// System.Diagnostics.Process process = new System.Diagnostics.Process();
           //////// process.StartInfo.UseShellExecute = false;
           //////// process.StartInfo.CreateNoWindow = true;

           //////// //set the executable location
           //////// process.StartInfo.FileName = Path.Combine(HttpContext.Current.Server.MapPath("~/PDFConverter"), "wkhtmltopdf.exe");
           //////// //set the arguments to the exectuable
           //////// objErrorHandler.CreateLog(process.StartInfo.FileName);
           //////// objErrorHandler.CreateLog(url + " " + filepath);
           //////// // wkhtmltopdf [OPTIONS]... <input fileContent> [More input fileContents] <output fileContent>
           //////// process.StartInfo.Arguments = url + " " + filepath;
           //////// process.StartInfo.RedirectStandardOutput = true;
           //////// process.StartInfo.RedirectStandardError = true;
           //////// process.StartInfo.RedirectStandardInput = true;
           //////// //run the executable
           //////// process.Start();

           //////// //wait until the conversion is done
           //////// process.WaitForExit();

           //////// // read the exit code, close process    
           //////// int returnCode = process.ExitCode;
           //////// objErrorHandler.CreateLog(returnCode.ToString());
           //////// process.Close();

           ////////// initialize the filestream with filepath
           //////// FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
           //////// objErrorHandler.CreateLog(filepath);
           //////// fileContent = new byte[(int)fs.Length];

           //////// //read the content
           //////// fs.Read(fileContent, 0, (int)fs.Length);

           //////// //close the stream
           //////// fs.Close();

           //////// return fileContent;
        }
        catch (Exception ex)
        {
            objErrorHandler.CreateLog(ex.ToString());
            return null;
        }


       
    }

    private byte[] GeneratePDFFile_Email(string filename)
    {

        string url = HttpContext.Current.Server.MapPath(filename);
        string fid = string.Empty;
        if (Request.QueryString["fid"] != null)
        {
            fid = Request.QueryString["fid"].ToString();
        }
        string pdf_filename = string.Empty;
        string user_id = string.Empty;
        if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
        {
            user_id = HttpContext.Current.Session["USER_ID"].ToString();
        }

        pdf_filename = "FamilyEmail" + fid + "_" + user_id + ".pdf";
        string filepath = Path.Combine(HttpContext.Current.Server.MapPath("~/FAMILY_PDF"), pdf_filename);
        //variable to store pdf file content
        byte[] fileContent = null;

        System.Diagnostics.Process process = new System.Diagnostics.Process();
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;

        //set the executable location
        process.StartInfo.FileName = Path.Combine(HttpContext.Current.Server.MapPath("~/PDFConverter"), "wkhtmltopdf.exe");
        //set the arguments to the exectuable
        // wkhtmltopdf [OPTIONS]... <input fileContent> [More input fileContents] <output fileContent>
        process.StartInfo.Arguments = url + " " + filepath;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.RedirectStandardInput = true;
        //run the executable
        process.Start();

        //wait until the conversion is done
        process.WaitForExit();

        // read the exit code, close process    
        int returnCode = process.ExitCode;
        process.Close();

        //initialize the filestream with filepath
        FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
        fileContent = new byte[(int)fs.Length];

        //read the content
        fs.Read(fileContent, 0, (int)fs.Length);

        //close the stream
        fs.Close();

        return fileContent;
    }
    //private byte[] GeneratePDFFile()
    //{
    //    //url of the page we would wanto convert.
    //    //we dont need to hard code this URL if we want to export current page
    //    //we could prepare the URL by using HTTPRequest object oo
    //    //string url = @"http://localhost:54630/PageExportPDF.aspx";
    //    string url = Request.Url.AbsoluteUri;

    //    //output PDF file Path
    //    string filepath = Path.Combine(Server.MapPath("~/PDF"), "test.pdf");

    //    //variable to store pdf file content
    //    byte[] fileContent = null;

    //    System.Diagnostics.Process process = new System.Diagnostics.Process();
    //    process.StartInfo.UseShellExecute = false;
    //    process.StartInfo.CreateNoWindow = true;

    //    //set the executable location
    //    process.StartInfo.FileName = Path.Combine(Server.MapPath("~/PDFConverter"), "wkhtmltopdf.exe");
    //    //set the arguments to the exectuable
    //    // wkhtmltopdf [OPTIONS]... <input fileContent> [More input fileContents] <output fileContent>
    //    process.StartInfo.Arguments = url + " " + filepath;
    //    process.StartInfo.RedirectStandardOutput = true;
    //    process.StartInfo.RedirectStandardError = true;
    //    process.StartInfo.RedirectStandardInput = true;
    //    //run the executable
    //    process.Start();

    //    //wait until the conversion is done
    //    process.WaitForExit();

    //    // read the exit code, close process    
    //    int returnCode = process.ExitCode;
    //    process.Close();

    //    //initialize the filestream with filepath
    //    FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
    //    fileContent = new byte[(int)fs.Length];

    //    //read the content
    //    fs.Read(fileContent, 0, (int)fs.Length);

    //    //close the stream
    //    fs.Close();

    //    return fileContent;
    //}
    protected void emailbtn_Click(object sender, EventArgs e)
    {

      //  Callemailpopup();
        string result = string.Empty;
       // string WithPrice = chkPriceemail.Checked.ToString();
       // string Details = chkdetailemail.Checked.ToString();



       // string Details = "true";
        string fid = string.Empty;
        //if (Request.QueryString["fid"] != null)
        //{
        //    fid = Request.QueryString["fid"].ToString();
        //}
        if (hffid.Value != "")
            fid = hffid.Value;
       // string strContent = Construct_ST(WithPrice, Details, "cell_pdf", fid,hfpath.Value);


        string newimagepath = HttpContext.Current.Server.MapPath("prodimages");
        string newnoimage = HttpContext.Current.Server.MapPath("images/noimage.gif");
       // strContent = strContent.Replace("prodimages", newimagepath).Replace("images/noimage.gif", newnoimage);

        string user_id = string.Empty;
        if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "")
        {
            user_id = HttpContext.Current.Session["USER_ID"].ToString();
        }

      //  string notes = txtnotes.Value;
      //  string emailid = txtemail.Value;
     


        try
        {
            var pechkin = Factory.Create(new GlobalConfig());
          //  var pdf = pechkin.Convert(new ObjectConfig()
                                  //  .SetLoadImages(true).SetZoomFactor(1.5)
                                 //   .SetPrintBackground(true)
                                //    .SetScreenMediaType(true)
                                //    .SetCreateExternalLinks(true), strContent);


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

            //using (FileStream file = System.IO.File.Create(Server.MapPath("EmailPdfFiles/" + "Family_" + fid + "_" + user_id + ".pdf")))
            //{
            //    file.Write(pdf, 0, pdf.Length);
            //}

            if (File.Exists(Server.MapPath("EmailPdfFiles/" + titlecase + "_" + user_id + ".pdf")))
            {
                File.Delete(Server.MapPath("EmailPdfFiles/" + titlecase + "_" + user_id + ".pdf"));
                using (FileStream file = System.IO.File.Create(Server.MapPath("EmailPdfFiles/" + titlecase + "_" + user_id + ".pdf")))
                {
                   // file.Write(pdf, 0, pdf.Length);
                }
            }
            else
            {
                using (FileStream file = System.IO.File.Create(Server.MapPath("EmailPdfFiles/" + titlecase + "_" + user_id + ".pdf")))
                {
                  //  file.Write(pdf, 0, pdf.Length);
                }
            }

            

            if (! File.Exists(Server.MapPath("EmailPdfFiles/" + titlecase + ".pdf")))
               File.Copy(Server.MapPath("EmailPdfFiles/" + titlecase + "_" + user_id + ".pdf"), Server.MapPath("EmailPdfFiles/" + titlecase + ".pdf"));

          //  result = objHelperServices.sendmail("Product Information." + " " + titlecase, notes, emailid, Server.MapPath("EmailPdfFiles/" + titlecase + ".pdf"));

           // File.Delete(Server.MapPath("EmailPdfFiles/" + "Family_" + fid + "_" + user_id + ".pdf"));

            if (File.Exists(Server.MapPath("EmailPdfFiles/" + titlecase + ".pdf")))
                File.Delete(Server.MapPath("EmailPdfFiles/" + titlecase + ".pdf"));
            if (File.Exists(Server.MapPath("EmailPdfFiles/" + titlecase + "_" + user_id + ".pdf")))
                File.Delete(Server.MapPath("EmailPdfFiles/" + titlecase +"_"+ user_id + ".pdf"));

            if (Session["PageUrl"] != null)
            {
                Response.Redirect(Session["PageUrl"].ToString());
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

            


    //    string newimagepath = HttpContext.Current.Server.MapPath("prodimages");
    //    string newnoimage = HttpContext.Current.Server.MapPath("images/noimage.gif");
    //    strContent = strContent.Replace("prodimages", newimagepath).Replace("images/noimage.gif", newnoimage).Replace("src='/","src='");
     
    //Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);

    //try
    //{
    //    Document pdfDoc1 = new Document(PageSize.A4, 10, 10, 10, 10);

    //    PdfWriter.GetInstance(pdfDoc1, new FileStream(Server.MapPath("EmailPdfFiles/" + fid + ".pdf"), FileMode.Create));
    //    pdfDoc1.Open();
    //    //Read string contents using stream reader and convert html to parsed conent 
    //    System.IO.StringReader x = new System.IO.StringReader(strContent);
    //    var parsedHtmlElements = HTMLWorker.ParseToList(x, null);

    //    //Get each array values from parsed elements and add to the PDF document 
    //    foreach (var htmlElement in parsedHtmlElements)
    //        pdfDoc1.Add(htmlElement as IElement);

    //    pdfDoc1.Close();
    //    string notes = txtnotes.Value;
    //    string emailid = txtemail.Value;
    //    string result = objHelperServices.sendmail("Family Details",notes, emailid, Server.MapPath("EmailPdfFiles/" + fid + ".pdf"));
    //   // File.Delete(Server.MapPath("EmailPdfFiles/" + fid + ".pdf"));
    //    Response.Redirect(Session["PageUrl"].ToString());

    //}
    //catch (Exception ex)

    //{
    //    objErrorHandler.CreateLog(ex.ToString());
    //    Response.Redirect(Session["PageUrl"].ToString());
    //}
    }

    //private void Callemailpopup()
    //{
    //    this.ModalPanel1.Visible = false;
    //    this.modalPop.Hide();
    //    modalPop = new AjaxControlToolkit.ModalPopupExtender();
       
    //        ModalPanel1.Visible = true;
    //        modalPop.ID = "popUp1";
    //        modalPop.PopupControlID = "ModalPanel1";
    //        modalPop.BackgroundCssClass = "modalBackground";
    //        modalPop.DropShadow = false;
    //        modalPop.TargetControlID = "btnHiddenTestPopupExtender";
    //        this.ModalPanel1.Controls.Add(modalPop);
    //        this.modalPop.Show();
       
    //}

    private void GetFamilyMultipleImages(int FamilyID, StringTemplate st)
    {
        string strfile = HttpContext.Current.Server.MapPath("ProdImages");
        HelperServices objHelperService = new HelperServices();
        DataSet oDs = new DataSet();
        //oDa.Fill(oDs, "Images");            
        DataTable dt = new DataTable();
        DataRow[] dr;
        oDs = (DataSet)HttpContext.Current.Session["FamilyProduct"];
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
                           // oPrd.MediumImage = objHelperService.SetImageFolderPath(oPrd.LargeImage.ToLower(), "_Images", "_images_200");

                            //oPrd.Thumpnail = oPrd.LargeImage.ToLower().Replace("_images", "_th50");
                            //oPrd.SmallImage = oPrd.LargeImage.ToLower().Replace("_images", "_th");
                            //oPrd.MediumImage = oPrd.LargeImage.ToLower().Replace("_images", "_images_200");
                            st.SetAttribute("TBT_MULTIIMAGES", oPrd);
                            if (firstImg)
                            {
                               // st.SetAttribute("TBT_TFWEB_IMAGE1", oPrd.MediumImage);
                                //st.SetAttribute("TBT_TFWEB_LIMAGE", oPrd.LargeImage);
                                firstImg = false;
                            }
                        }
                    }

                }
                else
                {
                   // string tempstr = "";
                   // string tempstr1 = "";
                  //  string[] tempstrs = null;
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
                              //  oPrd.MediumImage = objHelperService.SetImageFolderPath(img, "_Th", "_images_200");

                                //oPrd.Thumpnail = oPrd.LargeImage.ToLower().Replace("_images", "_th50");
                                //oPrd.SmallImage = oPrd.LargeImage.ToLower().Replace("_images", "_th");
                                //oPrd.MediumImage = oPrd.LargeImage.ToLower().Replace("_images", "_images_200");
                                st.SetAttribute("TBT_MULTIIMAGES", oPrd);
                                if (firstImg)
                                {
                                  //  st.SetAttribute("TBT_TFWEB_IMAGE1", oPrd.MediumImage);
                                   // st.SetAttribute("TBT_TFWEB_LIMAGE", oPrd.LargeImage);
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
    public string Construct_ST(string WithPrice, string Details, string cellname,string fid,string path)
    {

        StringTemplate _stmpl_records = null;
        StringTemplateGroup _stg_records = null;
        HelperServices objHelperServices = new HelperServices();
      //  breadcrumb = objEasyAsk.GetBreadCrumb_print();

        string bcreplace = string.Empty; // HttpContext.Current.Server.MapPath("ProdImages");


        //if (cellname == "cell")
        //{
        //    bcreplace = bcreplace + "images/close11.png";
        //    bcreplace = "<img alt='' style='vertical-align:middle; margin-left:5px;margin-top:-5px;' height='12px' width='14px' src= '" + bcreplace + "' />" + "" + "<span style='font-size:16px;'>/</span>";
        //    breadcrumb = breadcrumb.Replace(">>1", ">").Replace(">1", bcreplace);
        //}
        //else
        //{
        //    //bcreplace = HttpContext.Current.Server.MapPath("ProdImages");
        //    //bcreplace = bcreplace + "/images/close11.png";
        //    bcreplace = HttpContext.Current.Server.MapPath("images");
        //    bcreplace = bcreplace + "/close11.png";
        //    bcreplace = "<img alt='' style='vertical-align:middle; margin-left:5px;margin-top:-5px;' height='12px' width='14px' src= '" + bcreplace + "' />" +""+ "<span style='font-size:16px;'>/</span>";
        //    breadcrumb = breadcrumb.Replace(">>1", ">").Replace(">1", bcreplace);
        //}

        _stg_records = new StringTemplateGroup(cellname, HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()));
        if (cellname == "cell")
        {
            _stmpl_records = _stg_records.GetInstanceOf("CSFAMILYPAGE" + "\\" + "header");
          
        }
        else
        {
            _stmpl_records = _stg_records.GetInstanceOf("CSFAMILYPAGE" + "\\" + "header_pdf");
        }
       // _stmpl_records.SetAttribute("lblbreadcrum", breadcrumb);
        _stmpl_records.SetAttribute("closebtn", HttpContext.Current.Server.MapPath("/Images/close11.png"));
        _stmpl_records.SetAttribute("Imagepath", HttpContext.Current.Server.MapPath("/images/WesLogo.jpg"));






        if (Details == "true" || Details == "True")
            _stmpl_records.SetAttribute("TBT_DES_SHOW", true);
        else
            _stmpl_records.SetAttribute("TBT_DES_SHOW", false);



        //string strValue = _stmpl_records.ToString();
        //objErrorHandler.CreateLog("constract_st" + fid);

        EasyAsk_WES EasyAsk = new EasyAsk_WES();
        DataSet DsFamilyProduct = new DataSet();
        Security objSecurity = new Security();
        if (HttpContext.Current.Session["pfid"] != null)
        {
            if (HttpContext.Current.Session["pfid"].ToString() != fid)
            {
                
                //string ea = objSecurity.StringDeCrypt(Request.QueryString["path"].ToString());
                string ea = string.Empty;
                ea = HttpUtility.UrlDecode(path);
                ea = ea.Replace(" ", "+");
                ea = objSecurity.StringDeCrypt(ea);

                DsFamilyProduct = EasyAsk.GetAttributeProducts("FamilyPage", "", "FamilyId", fid, "", "0", "0", "", ea);
            }
            else
            {
               // DsFamilyProduct = (DataSet)HttpContext.Current.Session["FamilyProduct"];
                string ea1 = string.Empty;
                ea1 = HttpUtility.UrlDecode(path);
                ea1 = ea1.Replace(" ", "+");
                ea1 = objSecurity.StringDeCrypt(ea1);

                DsFamilyProduct = EasyAsk.GetAttributeProducts("FamilyPage", "", "FamilyId", fid, "", "0", "0", "", ea1);
            }
        }
        else
        {
            //Security objSecurity = new Security();
            string ea = objSecurity.StringDeCrypt(Request.QueryString["path"].ToString());
            DsFamilyProduct = EasyAsk.GetAttributeProducts("FamilyPage", "", "FamilyId", fid, "", "0", "0", "", ea);
        }
            breadcrumb = objEasyAsk.GetBreadCrumb_print();
         if (cellname == "cell")
         {
             bcreplace = bcreplace + "images/close11.png";
             bcreplace = "<img alt='' style='vertical-align:middle; margin-left:5px;margin-top:-5px;' height='12px' width='14px' src= '" + bcreplace + "' />" + " " + "<span style='font-size:16px;'>/</span>" +" ";
             breadcrumb = breadcrumb.Replace(">>1", ">").Replace(">1", bcreplace);
         }
         else
         {
            
             //bcreplace = HttpContext.Current.Server.MapPath("images");
             //bcreplace = bcreplace + "/close11.png";
             //bcreplace = "<img alt='' style='vertical-align:middle; margin-left:5px;margin-top:-5px;' height='12px' width='14px' src= '" + bcreplace + "' />" + " " + "<span style='font-size:16px;'>/</span>" +" ";
             //breadcrumb = breadcrumb.Replace(">>1", ">").Replace(">1", bcreplace);
             breadcrumb = breadcrumb.Replace(">>", ">").Replace(">", " > ");
         }

         _stmpl_records.SetAttribute("lblbreadcrum", breadcrumb);
         string strValue = _stmpl_records.ToString();
         


         string getfamilydetails = Generateparentfamilyhtml_print(fid, WithPrice, Details, DsFamilyProduct, cellname);
         string productdetails = ST_Familypage_print(fid, WithPrice, Details, DsFamilyProduct);

         string strcontents = string.Empty;

       // if(cellname == "cell")
        strcontents = "<html><body style='-webkit-print-color-adjust:exact;'>" + strValue + "<div style=\"background-color: #FFFFFF;width:950px;border: 1px solid #CCCCCC;border-radius: 6px;\" class='divborder' >" + getfamilydetails + "<div style='width:98%;margin:0 auto; border:1px solid #c8c8c8; border-left:none; height:40px; background:#f3f3f4;'><div style='width:85px; height:22px; background:#fff; margin-top:-5px; border-right:1px solid #c8c8c8; border-left:1px solid #c8c8c8; border-top:1px solid #c8c8c8; position:relative; -webkit-border-top-left-radius: 5px; -webkit-border-top-right-radius: 5px; -moz-border-radius-topleft: 5px; -moz-border-radius-topright: 5px; border-top-left-radius: 5px; border-top-right-radius: 5px; color:#2678bd; text-align:center; padding:10px; font-weight:bold; float:left; padding-top:15px;font-family: Arial;'> Products </div></div>" + productdetails + "</div></body></html>";
       // else
        //    strcontents = "<html><body style='-webkit-print-color-adjust:exact;'>" + strValue + getfamilydetails  + productdetails + "</body></html>";
        //string strcontents = "<html><body>" + strValue  + getfamilydetails +"<table style='Background-color:gray;' ><tr><td> Products </td> </tr></table>" +"</body></html>";
     //   objErrorHandler.CreateLog("constract_st" + fid);
        return strcontents.Replace("prodimages/prodimages/","prodimages/").Replace("prodimages/images/","images/");
    }

   
}
