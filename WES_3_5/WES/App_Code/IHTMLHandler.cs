using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Text;
using System.IO;
using TradingBell.WebCat.CommonServices;

namespace WES.App_Code
{
    public class IHTMLHandler : IHttpHandler, IRequiresSessionState
    {
       
     
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                bool requiresSession = false;
                HelperServices objhelper = new HelperServices();
              
                if (context.Handler is IRequiresSessionState)
                    requiresSession = true;



                HttpRequest Request = context.Request;
                HttpResponse Response = context.Response;
                string urlRequested = context.Request.Path;
                if(urlRequested.Contains("404New.htm"))
                {
                    string filePath = context.Server.MapPath(urlRequested);

                    string fileName = System.IO.Path.GetFileName(filePath);

                    context.Response.ClearContent();

                    context.Response.ClearHeaders();

                    FileInfo pdfInfo = new FileInfo(filePath);

                    context.Response.AddHeader("Content-Length", pdfInfo.Length.ToString());

                    //assuming that the file is pdf, needs to be changed appropriately
                    //context.Response.ContentType = "application/zip";
                    context.Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Html;

                    context.Response.TransmitFile(filePath);

                    context.Response.End();
                }
                if ((context.Session["USER_ID"] == null) && (objhelper.CheckCredential() == false))
                {
                    context.Session["PageUrl"] = context.Request.Path;
                    Response.Redirect("/Login.aspx");
                }
                else if ((context.Session["USER_ID"] == ""))
                {
                    context.Session["PageUrl"] = context.Request.Path;
                    Response.Redirect("/Login.aspx");
                }
                else
                {

                   
                    if ((urlRequested.ToLower().Contains("index.html") == true) && (urlRequested.ToLower().Contains("attachments") == false))
                    {
                        urlRequested = "/attachments" + urlRequested;
                       
                       // Response.Redirect(urlRequested);
                    }
                    if (urlRequested.ToLower().Contains("attachments") == false)
                    {
                        
                        urlRequested = "/attachments" + urlRequested;
                    }
                    objhelper.writelog("b4url " + urlRequested);
                    if (urlRequested.ToLower().Contains("/media/wes_secure_files/") == false) 
                    {
                      
                        urlRequested = urlRequested.Replace("/media/", "/media/wes_secure_files/");
                        objhelper.writelog("after url " + urlRequested);
                    }
                    string filePath = System.Configuration.ConfigurationManager.AppSettings["SharedFilePath"].ToString() + urlRequested;
                   // string filePath = context.Server.MapPath(urlRequested);
                    objhelper.writelog("filepath " + filePath);   
                    string fileName = System.IO.Path.GetFileName(filePath);

                    context.Response.ClearContent();

                    context.Response.ClearHeaders();

                    FileInfo pdfInfo = new FileInfo(filePath);

                    context.Response.AddHeader("Content-Length", pdfInfo.Length.ToString());

                    //assuming that the file is pdf, needs to be changed appropriately
                    //context.Response.ContentType = "application/zip";
                    context.Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Html;

                    context.Response.TransmitFile(filePath);

                    context.Response.End();
                }

            }
            catch(Exception ex)
            {
              //  Response.Redirect("/Login.aspx");
            }
            // This handler is called whenever a file ending 
            // in .sample is requested. A file with that extension
            // does not need to exist.
            //Response.Write("<html>");
            //Response.Write("<body>");
            //Response.Write("<h1>Hello from a synchronous custom HTTP handler.</h1>");
            //Response.Write("</body>");
            //Response.Write("</html>");
        }


        public bool IsReusable
        {
            // To enable pooling, return true here.
            // This keeps the handler in memory.
            get { return false; }
        }
    }
}