using System.Web;
using System;
using System.IO;
using System.Text;
//using Yahoo.Yui.Compressor;
//using log4net;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO.Compression;
using System.Web.Caching;
using System.Net;
public class AxdResourceHandler : IHttpHandler
{
    private const int DAYS_IN_CACHE = 30;

    /// <summary>
    /// Enables processing of HTTP Web requests by a custom 
    /// HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"></see> interface.
    /// </summary>
    /// <param name="context">An <see cref="T:System.Web.HttpContext"></see> object that provides 
    /// references to the intrinsic server objects 
    /// (for example, Request, Response, Session, and Server) used to service HTTP requests.
    /// </param>
    public void ProcessRequest(HttpContext context)
    {
        string root = context.Request.Url.GetLeftPart(UriPartial.Authority);
        string path = context.Request.QueryString["path"];
        string content = string.Empty;

        if (!string.IsNullOrEmpty(path))
        {
            //if (context.Cache[path] == null)
            //{
                string[] scripts = path.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string script in scripts)
                {
                    // We only want to serve resource files for security reasons.
                    if (script.ToUpperInvariant().Contains("RESOURCE.AXD"))
                        content += RetrieveScript(root + script) + Environment.NewLine;
                }

               // content = StripWhitespace(content);
               //context.Cache.Insert(path, content, null, Cache.NoAbsoluteExpiration, new TimeSpan(DAYS_IN_CACHE, 0, 0, 0));
            //}
        }

        //content = (string)context.Cache[path];
        if (!string.IsNullOrEmpty(content))
        {
            content=JsMinifier.GetMinifiedCode(content);  
            context.Response.Write(content);
           // SetHeaders(content.GetHashCode(), context);            
           // Compress(context);
            
        }
    }

    /// <summary>
    /// Retrieves the specified remote script using a WebClient.
    /// </summary>
    /// <param name="file">The remote URL</param>
    private static string RetrieveScript(string file)
    {
        string script = null;

        try
        {
            Uri url = new Uri(file, UriKind.Absolute);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                script = reader.ReadToEnd();
            }
        }
        catch (System.Net.Sockets.SocketException)
        {
            // The remote site is currently down. Try again next time.
        }
        catch (UriFormatException)
        {
            // Only valid absolute URLs are accepted
        }

        return script;
    }

    /// <summary>
    /// Strips the whitespace from any .js file.
    /// </summary>
    private static string StripWhitespace(string body)
    {
        string[] lines = body.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        StringBuilder emptyLines = new StringBuilder();
        foreach (string line in lines)
        {
            string s = line.Trim();
            if (s.Length > 0 && !s.StartsWith("//"))
                emptyLines.AppendLine(s.Trim());
        }

        body = emptyLines.ToString();

        // remove C styles comments
        body = Regex.Replace(body, "/\\*.*?\\*/", String.Empty, RegexOptions.Compiled | RegexOptions.Singleline);
        //// trim left
        body = Regex.Replace(body, "^\\s*", String.Empty, RegexOptions.Compiled | RegexOptions.Multiline);
        //// trim right
        body = Regex.Replace(body, "\\s*[\\r\\n]", "\r\n", RegexOptions.Compiled | RegexOptions.ECMAScript);
        // remove whitespace beside of left curly braced
        body = Regex.Replace(body, "\\s*{\\s*", "{", RegexOptions.Compiled | RegexOptions.ECMAScript);
        // remove whitespace beside of coma
        body = Regex.Replace(body, "\\s*,\\s*", ",", RegexOptions.Compiled | RegexOptions.ECMAScript);
        // remove whitespace beside of semicolon
        body = Regex.Replace(body, "\\s*;\\s*", ";", RegexOptions.Compiled | RegexOptions.ECMAScript);
        // remove newline after keywords
        body = Regex.Replace(body, "\\r\\n(?<=\\b(abstract|boolean|break|byte|case|catch|char|class|const|continue|default|delete|do|double|else|extends|false|final|finally|float|for|function|goto|if|implements|import|in|instanceof|int|interface|long|native|new|null|package|private|protected|public|return|short|static|super|switch|synchronized|this|throw|throws|transient|true|try|typeof|var|void|while|with)\\r\\n)", " ", RegexOptions.Compiled | RegexOptions.ECMAScript);

        return body;
    }

    /// <summary>
    /// This will make the browser and server keep the output
    /// in its cache and thereby improve performance.
    /// </summary>
    private static void SetHeaders(int hash, HttpContext context)
    {
        context.Response.ContentType = "text/javascript";
        context.Response.Cache.VaryByHeaders["Accept-Encoding"] = true;

        context.Response.Cache.SetExpires(DateTime.Now.ToUniversalTime().AddDays(DAYS_IN_CACHE));
        context.Response.Cache.SetCacheability(HttpCacheability.Public);
        context.Response.Cache.SetMaxAge(new TimeSpan(DAYS_IN_CACHE, 0, 0, 0));
        context.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
        context.Response.Cache.SetETag("\"" + hash.ToString() + "\"");
    }

    #region Compression

    private const string GZIP = "gzip";
    private const string DEFLATE = "deflate";

    private static void Compress(HttpContext context)
    {

        //context.Response.Filter = new JavascriptMinifierStream(context.Response.Filter);

        if (context.Request.UserAgent != null && context.Request.UserAgent.Contains("MSIE 6"))
            return;

        if (IsEncodingAccepted(GZIP))
        {
            context.Response.Filter = new GZipStream(context.Response.Filter, CompressionMode.Compress);
            SetEncoding(GZIP);
        }
        else if (IsEncodingAccepted(DEFLATE))
        {
            context.Response.Filter = new DeflateStream(context.Response.Filter, CompressionMode.Compress);
            SetEncoding(DEFLATE);
        }
    }

    /// <summary>
    /// Checks the request headers to see if the specified
    /// encoding is accepted by the client.
    /// </summary>
    private static bool IsEncodingAccepted(string encoding)
    {
        return HttpContext.Current.Request.Headers["Accept-encoding"] != null && HttpContext.Current.Request.Headers["Accept-encoding"].Contains(encoding);
    }

    /// <summary>
    /// Adds the specified encoding to the response headers.
    /// </summary>
    /// <param name="encoding"></param>
    private static void SetEncoding(string encoding)
    {
        HttpContext.Current.Response.AppendHeader("Content-encoding", encoding);
    }

    #endregion

    /// <summary>
    /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"></see> instance.
    /// </summary>
    /// <value></value>
    /// <returns>true if the <see cref="T:System.Web.IHttpHandler"></see> instance is reusable; otherwise, false.</returns>
    public bool IsReusable
    {
        get { return false; }
    }

}