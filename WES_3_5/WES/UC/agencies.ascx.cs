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
public partial class UC_agencies : System.Web.UI.UserControl
{
    string stemplatepath = "";
    HelperServices objHelperServices = new HelperServices();
    protected void Page_Load(object sender, EventArgs e)
    {
        stemplatepath = Server.MapPath(ConfigurationManager.AppSettings["StringTemplatePath"].ToString());
        try
        {
            if (Request.Url.OriginalString.ToString().ToUpper().Contains("AGENCIES.ASPX"))
                stemplatepath = stemplatepath.Substring(0, stemplatepath.Substring(0, stemplatepath.Length - 1).LastIndexOf('\\')) ;
        }
        catch (Exception) { }
        Page.Title = objHelperServices.GetOptionValues("BROWSER TITLE").ToString();
    }
    public string ST_Agencies()
    { 
      /*  StringTemplateGroup _stg_container = null;
        StringTemplateGroup _stg_records = null;
        StringTemplateGroup _stg_pages = null;
        StringTemplate _stmpl_container = null;
        StringTemplate _stmpl_records = null;
        StringTemplate _stmpl_pages = null;
        TBWDataList[] lstrecords = new TBWDataList[0];
        TBWDataList[] lstrows = new TBWDataList[1];
        string[] filenames = null;
        
        string shtml = "";
        if (Directory.Exists(Server.MapPath("images/img_logos")))
        {
            string[] fileEntries = Directory.GetFiles(Server.MapPath("images/img_logos"));
            lstrecords = new TBWDataList[fileEntries.Length];
            filenames = new string[fileEntries.Length];
            _stg_records = new StringTemplateGroup("Agenices", stemplatepath);
            string _stragenices = "";
            if (fileEntries.Length > 0)
            {
                int counter = 0;
                for (int i = 0; i < fileEntries.Length; i++)
                {
                    FileInfo fi = new FileInfo(fileEntries[i]);
                    _stmpl_records = _stg_records.GetInstanceOf("cell");
                    _stmpl_records.SetAttribute("AGENCIES_LOGO", fi.Name);
                    counter++;
                    if (counter == 3)
                    {
                        lstrecords[i] = _stmpl_records;
                    }
                    else
                    {
                        lstrecords[i]= _stmpl_records;
                    }
                }
            }
            _stg_container = new StringTemplateGroup("Agenices", stemplatepath);
            _stmpl_container = _stg_container.GetInstanceOf("rows" );
            _stmpl_container.SetAttribute("TBWDataList", lstrecords);
            lstrows[0] = new TBWDataList(_stmpl_container.ToString());
            _stg_container = new StringTemplateGroup("Agenices", stemplatepath);
            _stmpl_container = _stg_container.GetInstanceOf( "main");
            _stmpl_container.SetAttribute("TBWDataList", lstrows);
            return _stmpl_container.ToString();
        }
       */
        return "";
    }
}
