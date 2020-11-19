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
    public class IFilesHandler: IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                bool requiresSession = false;
                HelperServices objhelper = new HelperServices();
                if (context.Handler is IRequiresSessionState)
                    requiresSession = true;


                string urlRequested = context.Request.Path;
  if ((context.Session["USER_ID"] == null) && (objhelper.CheckCredential() == false)&&(urlRequested.Contains("/media/" )==true))
                {
                    context.Session["PageUrl"] = context.Request.Path;
                    Response.Redirect("/Login.aspx");
                }
  else if ((context.Session["USER_ID"] == "") && (urlRequested.Contains("/media/") == true))
  {
      context.Session["PageUrl"] = context.Request.Path;
      Response.Redirect("/Login.aspx");
  }
  else
  {
      if ((urlRequested.ToLower().Contains(".html") == false)
                          && (urlRequested.ToLower().Contains(".htm") == false) && (urlRequested.ToLower().Contains(".pdf") == false) && (urlRequested.ToLower().Contains(".exe") == false))
      {






          urlRequested = urlRequested.Replace("\\attachments", "");
          string filePath = System.Configuration.ConfigurationManager.AppSettings["SharedFilePath"].ToString() + urlRequested;
          filePath = filePath.Replace("/mediapub/", "/wes_public_files/").Replace("/media/", "/wes_secure_files/").Replace("/", "\\");

          // or whatever folder you want to load..



          var htmlFiles = new DirectoryInfo(filePath).GetFiles("*.html");


          if (htmlFiles.Length > 0)
          {

              var firsthtmlFilename = htmlFiles[0].Name;
              filePath = filePath + firsthtmlFilename;



          }

          else
          {

              var htmFiles = new DirectoryInfo(filePath).GetFiles("*.htm");
              if (htmFiles.Length > 0)
              {
                  var firsthtmFilename = htmFiles[0].Name;
                  filePath = filePath + firsthtmFilename;


              }
          }


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
      else if (urlRequested.ToLower().Contains("html") == true || urlRequested.ToLower().Contains("htm") == true)
      {

          urlRequested = urlRequested.Replace("\\attachments", "");

          string filePath = System.Configuration.ConfigurationManager.AppSettings["SharedFilePath"].ToString() + urlRequested;
          filePath = filePath.Replace("/mediapub/", "/wes_public_files/").Replace("/media/", "/wes_secure_files/").Replace("/", "\\");

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
      else if (urlRequested.ToLower().Contains("pdf") == true)
      {
          urlRequested = urlRequested.Replace("\\attachments", "");
          string filePath = System.Configuration.ConfigurationManager.AppSettings["SharedFilePath"].ToString() + urlRequested;
          filePath = filePath.Replace("/mediapub/", "/wes_public_files/").Replace("/media/", "/wes_secure_files/").Replace("/", "\\");

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
  else if (urlRequested.ToLower().Contains(".exe") == true)
                {
                   
                    urlRequested = urlRequested.Replace("\\attachments", "");
                    string filePath = System.Configuration.ConfigurationManager.AppSettings["SharedFilePath"].ToString() + urlRequested;
                    filePath = filePath.Replace("/mp/", "/wagner_public_files/").Replace("/", "\\");

                    string fileName = System.IO.Path.GetFileName(filePath);

                    context.Response.ClearContent();

                    context.Response.ClearHeaders();

                    FileInfo pdfInfo = new FileInfo(filePath);

                    context.Response.AddHeader("Content-Length", pdfInfo.Length.ToString());

                  
                    Response.ContentType = "application/octet-stream";

                  
                    Response.WriteFile(filePath);
                    context.Response.End();

                }
  }
            }
            catch (Exception ex)
            {
                //Response.Write(ex.ToString());
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