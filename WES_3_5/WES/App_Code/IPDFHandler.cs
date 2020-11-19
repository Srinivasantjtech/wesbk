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
    public class IPDFHandler : IHttpHandler, IRequiresSessionState
    {

        public IPDFHandler()
        {

        }
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                bool requiresSession = false;
                HelperServices objhelper = new HelperServices();
                if (context.Handler is IRequiresSessionState)
                    requiresSession = true;

                //Console.WriteLine("inside pdf");  

                HttpRequest Request = context.Request;
                HttpResponse Response = context.Response;
                

                if ((context.Session["USER_ID"] == null) && (objhelper.CheckCredential() == false))
                {
                    string pdfurl = context.Request.Path;
                    HelperServices objHelperServices = new HelperServices();
                    objHelperServices.writelog("After for"); 
                    if (pdfurl.ToLower().Contains("wes_secure_files/") == false)
                    {
                        pdfurl = pdfurl.Replace("/media/", "/media/wes_secure_files/");
                    }
                    
                    if (pdfurl != "" && pdfurl.ToLower().Contains(".pdf") == true)
                    {
                        string filePath = context.Server.MapPath(pdfurl);
                        string fileName = System.IO.Path.GetFileName(filePath);
                        context.Response.ClearContent();
                        context.Response.ClearHeaders();
                        FileInfo pdfInfo = new FileInfo(filePath);
                        context.Response.AddHeader("Content-Length", pdfInfo.Length.ToString());
                        context.Response.ContentType = "application/pdf";
                        context.Response.TransmitFile(filePath);
                        context.Response.End();
                    }
                    else
                    {
                        context.Session["PageUrl"] = context.Request.Path;
                        Response.Redirect("/Login.aspx");
                    }
                }
                else if (context.Session["USER_ID"] == "")
                {
                    string pdfurl = context.Request.Path;
                    if (pdfurl.ToLower().Contains("wes_secure_files/") == false)
                    {
                        pdfurl = pdfurl.Replace("/media/", "/media/wes_secure_files/");
                    }
                    if (pdfurl != "" && pdfurl.ToLower().Contains(".pdf") == true)
                    {
                        string filePath = context.Server.MapPath(pdfurl);
                        string fileName = System.IO.Path.GetFileName(filePath);
                        context.Response.ClearContent();
                        context.Response.ClearHeaders();
                        FileInfo pdfInfo = new FileInfo(filePath);
                        context.Response.AddHeader("Content-Length", pdfInfo.Length.ToString());
                        context.Response.ContentType = "application/pdf";
                        context.Response.TransmitFile(filePath);
                        context.Response.End();
                    }
                    else
                    {
                        context.Session["PageUrl"] = context.Request.Path;
                        Response.Redirect("/Login.aspx");
                    }
                }
                else
                {
                    string urlRequested = context.Request.Path;

                    if (urlRequested.ToLower().Contains("attachments") == false)
                    {
                        urlRequested = "/attachments" + urlRequested;
                    }
                    if (urlRequested.ToLower().Contains("wes_secure_files/") == false)
                    {
                        urlRequested = urlRequested.Replace("/media/","/media/wes_secure_files/");
                    }
                    string filePath = context.Server.MapPath(urlRequested);

                    string fileName = System.IO.Path.GetFileName(filePath);

                    context.Response.ClearContent();

                    context.Response.ClearHeaders();

                    FileInfo pdfInfo = new FileInfo(filePath);

                    context.Response.AddHeader("Content-Length", pdfInfo.Length.ToString());

                    //assuming that the file is pdf, needs to be changed appropriately
                    //context.Response.ContentType = "application/zip";
                    context.Response.ContentType = "application/pdf";

                    context.Response.TransmitFile(filePath);

                    context.Response.End();
                }
            }
            catch
            {
               // Response.Redirect("/Login.aspx");
            
            }
            //    string fileToServe = context.Request.Path;
            //    //Log the user and the file served to the DB
            //    FileInfo pdf = new FileInfo(context.Server.MapPath(fileToServe));
            //    context.Response.ClearContent();
            //    context.Response.ContentType = "application/pdf";
            //    context.Response.AddHeader("Content-Disposition", "attachment; filename=" + pdf.Name);
            //    context.Response.AddHeader("Content-Length", pdf.Length.ToString());
            //    context.Response.TransmitFile(pdf.FullName);
            //    context.Response.Flush();
            //    context.Response.End();

            //}
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