using System;
using System.Web;
using System.IO;

/// <summary>
/// Summary description for PDFHandler
/// </summary>
/// [assembly: TagPrefix("TradingBell.Common", "WebCat")]
//[assembly: System.Reflection.AssemblyVersion("5.0")]
namespace Handlers
{
    public class PDFHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.UrlReferrer.ToString().ToLower().Contains("cataloguedownload.aspx") || context.Request.UrlReferrer.ToString().ToLower().Contains("categorylist.aspx"))
            {
                FileStream oFile = new FileStream(context.Request.PhysicalPath, FileMode.Open);
                byte[] content = new byte[oFile.Length];
                oFile.Read(content, 0, Convert.ToInt32(oFile.Length));
                oFile.Close();
                context.Response.ContentType = "application/pdf";
                context.Response.BinaryWrite(content);
                context.Response.End();
            }
            else
                context.Response.Redirect("/Login.aspx?URL=CatalogueDownload.aspx");
        }
        /// <summary>
        /// Marks the handler reusable across multiple requests
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}