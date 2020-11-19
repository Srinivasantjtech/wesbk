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

public partial class UC_specialproducts : System.Web.UI.UserControl
{
    string stemplatepath = "";
    ErrorHandler objErrorHandler=new ErrorHandler();
    HelperDB objHelperDB = new HelperDB();
    HelperServices objHelperServices = new HelperServices();
    CategoryServices objCategoryServices = new CategoryServices();

    protected void Page_Load(object sender, EventArgs e)
    {
        stemplatepath = Server.MapPath("Templates");
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString(); 
    }
    public string ST_Specialproduct()
    {

        try
        {
            ConnectionDB objConnectionDB = new ConnectionDB();
            HelperServices objHelperServices = new HelperServices();

            TBWTemplateEngine tbwtEngine = new TBWTemplateEngine("SpecialProducts", Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString()), objConnectionDB.ConnectionString);
            //tbwtEngine.RenderHTML("Column");
            //return (tbwtEngine.RenderedHTML);
            return tbwtEngine.ST_SpecialProduct();
        }
        catch (Exception e)
        {
            objErrorHandler.ErrorMsg = e;
            objErrorHandler.CreateLog();
            return string.Empty;
        }
    }
    //public string ST_PDFDownload()
    //{
    //    StringTemplateGroup _stg_container = null;
    //    StringTemplateGroup _stg_records = null;
    //    StringTemplateGroup _stg_pages = null;
    //    StringTemplate _stmpl_container = null;
    //    StringTemplate _stmpl_records = null;
    //    StringTemplate _stmpl_pages = null;
    //    TBWDataList[] lstrecords = new TBWDataList[0];
    //    string[] filenames = null;

    //    string shtml = "";
    //    int counter = 0;

    //    if (Directory.Exists(Server.MapPath("News update")))
    //    {

    //        //string[] fileEntries = Directory.GetFiles(Server.MapPath("News update"));
    //        //lstrecords = new TBWDataList[fileEntries.Length];
    //        //filenames = new string[fileEntries.Length];
    //        //if (fileEntries.Length > 0)

    //        DataSet dsPDFCount = new DataSet();
    //        dsPDFCount = objCategoryServices.GetCatalogPDFCount(5);

    //        if (dsPDFCount != null)
    //        {
    //            foreach (DataRow rPDF in dsPDFCount.Tables[0].Rows)
    //            {
    //                lstrecords = new TBWDataList[Convert.ToInt32(rPDF["CountFiles"].ToString())];
    //            }
    //        }

    //        if (lstrecords.Length > 0)
    //        {
    //            DataSet dsCatalog = new DataSet();
    //            try
    //            {
    //                dsCatalog = objCategoryServices.GetCatalogPDFDownload(5);
    //                if (dsCatalog != null)
    //                {
    //                    foreach (DataRow rCat in dsCatalog.Tables[0].Rows)
    //                    {
    //                        string MyFile = Server.MapPath(string.Format("News update/{0}", rCat["IMAGE_FILE2"].ToString()));

    //                        _stg_records = new StringTemplateGroup("Newsupdate", stemplatepath);
    //                        _stmpl_records = _stg_records.GetInstanceOf("Newsupdate" + "\\" + "cell");
    //                        _stg_container = new StringTemplateGroup("Newsupdate", stemplatepath);
    //                        _stmpl_container = _stg_container.GetInstanceOf("Newsupdate" + "\\" + "row");

    //                        if (System.IO.File.Exists(MyFile))
    //                        {
    //                            //Modified By :indu :Added replace function to remove  catelogdowload

    //                            _stmpl_records.SetAttribute("PDF", rCat["IMAGE_FILE2"].ToString().Replace("\\", "/"));
    //                            //_stmpl_records.SetAttribute("PDF", rCat["IMAGE_FILE2"].ToString());
    //                            _stmpl_records.SetAttribute("PDFFILEDESCRIPTION", rCat["IMAGE_NAME2"].ToString());

    //                            FileInfo finfo = new FileInfo(Server.MapPath(string.Format("News update/{0}", rCat["IMAGE_FILE2"].ToString())));
    //                            long FileInBytes = finfo.Length;
    //                            long FileInKB = finfo.Length / 1024;

    //                            _stmpl_records.SetAttribute("PDF_SIZE", FileInKB + " KB");
    //                            _stmpl_records.SetAttribute("PDF_DATE", rCat["MODIFIED_DATE"].ToString());
    //                            _stmpl_container.SetAttribute("TBWDataList", _stmpl_records.ToString());
    //                            lstrecords[counter] = new TBWDataList(_stmpl_container.ToString());
    //                            counter++;
    //                        }
    //                    }
    //                }
    //            }
    //            catch (Exception e)
    //            {
    //                objErrorHandler.ErrorMsg = e;
    //                objErrorHandler.CreateLog(); 
    //            }

    //            _stg_container = new StringTemplateGroup("Newsupdate", stemplatepath);
    //            _stmpl_container = _stg_container.GetInstanceOf("Newsupdate" + "\\" + "main");
    //            _stmpl_container.SetAttribute("TBWDataList", lstrecords);
    //            shtml = _stmpl_container.ToString();
    //        }
    //        else
    //            return "<table height=\"100\"><tr><td valign=\"bottom\">No PDF Catalogue found</td></tr></table>";
    //    }
    //    else
    //        return "<table height=\"100\"><tr><td valign=\"bottom\">No PDF catalogue found</td></tr></table>";
    //    return objHelperServices.StripWhitespace(   shtml);

    //}
}
