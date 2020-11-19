using System.Web;
using System;
using System.IO;
using System.Text;
//using Yahoo.Yui.Compressor;
//using log4net;
using System.Reflection;
public class JsResourceHandler : IHttpHandler
{
  //  private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest(HttpContext context)
    {
        String queryString = context.Request.QueryString["d"];
        String[] cssFiles = queryString.Split(',');
        StringBuilder output = new StringBuilder();
        foreach (String cssFile in cssFiles)
        {
            if (File.Exists(context.Request.MapPath(cssFile)))
            {
                TextReader reader = new StreamReader(context.Request.MapPath(cssFile));
                output.Append(reader.ReadToEnd());
            }
        }
        context.Response.Write(output.ToString());
        context.Response.ContentType = "text/javascript";
    }

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }
}