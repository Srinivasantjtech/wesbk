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
using System.Net;
public partial class PDFDownload : System.Web.UI.Page
{
    Security objSecurity = new Security();
    ErrorHandler objErrorHandler = new ErrorHandler();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
             if (Request.QueryString["FilePath"] != null)
            {
                string pdfurldec = "";
                pdfurldec = Request.QueryString["FilePath"];
               // pdfurldec = objSecurity.StringDeCrypt(pdfurldec);

                string pdfurl = "/attachments" + pdfurldec;
                string filename = "";
                if (Request.QueryString["FileName"] != null)
                    filename = Request.QueryString["FileName"];
                string FileName = filename;

                String path = Server.MapPath(pdfurl);

                if (System.IO.File.Exists(path))
                {
                    FileInfo fileInfo = new FileInfo(FileName);
                    if (HttpContext.Current.Session["USER_NAME"] != null && Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()) > 0)
                    {
                       //correct code
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("Content-Disposition", "attachment;filename=\"" + FileName + "\"");
                        Response.TransmitFile(Server.MapPath(pdfurl));
                        Response.End();   
                    }
                    else
                    {
                        Response.Redirect("~/login.aspx", true);
                    }
                }

            }
            //else if (Request.QueryString["EbookLink"] != null)
            //{

            //    string ebooklink = "";
            //    string sitename = "";
            //    string ebooklink1 = "";
            //    if (HttpContext.Current.Session["USER_NAME"] != null && Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()) > 0)
            //    {
            //        if (HttpContext.Current.Request.Url.ToString().ToLower().Contains(".htm") == true || HttpContext.Current.Request.Url.ToString().ToLower().Contains(".html") == true)
            //        {
            //            sitename = Request.Url.GetLeftPart(UriPartial.Authority);
            //            ebooklink = sitename + "/attachments" + Request.QueryString["EbookLink"];
            //            ebooklink1 = "/attachments" + Request.QueryString["EbookLink"];
            //            Response.Clear();
            //            Response.AddHeader("content-disposition", "attachment");
            //            Response.ContentType = "text/html";
            //            Response.WriteFile(Server.MapPath(ebooklink1));
            //            Response.End();

            //        }
            //        else
            //        {
            //             ebooklink = Request.QueryString["EbookLink"];
            //            //string redirect = "<script>window.open('" + ebooklink + "','WESpopup', " + "'width=400, height=400, resizable=no,left=200,top=200');</script>";
            //            //Response.Write(redirect);
            //            //Response.Redirect(actionresult);                      
                                           
            //        }
            //    }
            //}
            //else
            //{
            //    Response.Redirect("~/login.aspx", true);
            //}
        
        }
        catch (Exception ex)
        {
            objErrorHandler.ErrorMsg = ex;
           // objErrorHandler.CreateLog();

        }

    }

  
}
