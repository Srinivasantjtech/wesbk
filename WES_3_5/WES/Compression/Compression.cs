#region Using

using System;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.IO;
using System.IO.Compression;
using System.Web.UI;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

#endregion

/// <summary>
/// Compresses the output using standard deflate.
/// </summary>
public sealed class CompressionModule : IHttpModule
{

    #region IHttpModule Members

    /// <summary>
    /// Disposes of the resources (other than memory) used by the module 
    /// that implements <see cref="T:System.Web.IHttpModule"></see>.
    /// </summary>
    void IHttpModule.Dispose()
    {
        // Nothing to dispose; 
    }

    /// <summary>
    /// Initializes a module and prepares it to handle requests.
    /// </summary>
    /// <param name="context">An <see cref="T:System.Web.HttpApplication"></see> 
    /// that provides access to the methods, properties, and events common to 
    /// all application objects within an ASP.NET application.
    /// </param>
    void IHttpModule.Init(HttpApplication context)
    {
        // For page compression
        context.PreRequestHandlerExecute += new EventHandler(context_PreReleaseRequestState);
        context.PostRequestHandlerExecute += new EventHandler(context_PostBeginRequest);

        // For ScriptResource.axd compression
        context.BeginRequest += new EventHandler(context_BeginRequest);
        context.EndRequest += new EventHandler(context_EndRequest);
    }

    #endregion

    private const string DEFLATE = "deflate";

    #region Compress page

    /// <summary>
    /// Handles the BeginRequest event of the context control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    void context_PreReleaseRequestState(object sender, EventArgs e)
    {
        HttpApplication app = (HttpApplication)sender;
        
        if (app.Context.CurrentHandler is System.Web.UI.Page && app.Request["HTTP_X_MICROSOFTAJAX"] == null)
        {
            Page page = (Page)app.Context.CurrentHandler;
           //page.Load += new EventHandler(Page_Load_CSS_Combiner);

            if (IsEncodingAccepted(DEFLATE))
            {
                app.Response.Filter = new DeflateStream(app.Response.Filter, CompressionMode.Compress);
                SetEncoding(DEFLATE);
            }
            if (app.Response.ContentType.StartsWith("text/javascript", StringComparison.InvariantCultureIgnoreCase))
            {
                //minify the js
                app.Response.Filter = new JavascriptMinifierStream(app.Response.Filter);
            }
            else if (app.Response.ContentType.StartsWith("text/css", StringComparison.InvariantCultureIgnoreCase))
            {
                //minify the css
                app.Response.Filter = new CssMinifierStream(app.Response.Filter);
            }
            else if (app.Response.ContentType.StartsWith("text/html", StringComparison.InvariantCultureIgnoreCase))
            {
                //minify the html
                app.Response.Filter = new HtmlMinifierStream(app.Response.Filter);
               // app.Response.Filter = new InlineStyleMinifierStream(app.Response.Filter);
                //app.Response.Filter = new InlineJavascriptMinifierStream(app.Response.Filter);
                //app.Response.Filter = new WebResourceFilter(app.Response.Filter);
            }
           
        }
    }

    /// <summary>
    /// Checks the request headers to see if the specified
    /// encoding is accepted by the client.
    /// </summary>
    private static bool IsEncodingAccepted(string encoding)
    {
        HttpContext context = HttpContext.Current;
        return context.Request.Headers["Accept-encoding"] != null && context.Request.Headers["Accept-encoding"].Contains(encoding);
    }

    /// <summary>
    /// Adds the specified encoding to the response headers.
    /// </summary>
    /// <param name="encoding"></param>
    private static void SetEncoding(string encoding)
    {
        if(encoding!=null)
            HttpContext.Current.Response.AppendHeader("Content-encoding", encoding);
    }

    #endregion

    #region Compress ScriptResource.axd

    /// <summary>
    /// Handles the BeginRequest event of the context control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void context_BeginRequest(object sender, EventArgs e)
    {
        HttpApplication app = (HttpApplication)sender;
        // app.Request.Path.Contains("CssResource.axd") || app.Request.Path.Contains("JsResource.axd")
        //if (app.Request.Path.Contains("ScriptResource.axd") || app.Request.Path.Contains("CssResource.axd") )
        if (app.Request.Path.Contains("CssResource.axd") || app.Request.Path.Contains("ScriptResource.axd") || app.Request.Path.Contains("JsResource.axd"))
        {
          
            SetCachingHeaders(app);

            if (app.Context.Request.QueryString["c"] == null && (IsEncodingAccepted(DEFLATE)))
                app.CompleteRequest();
        }
        //if (app.Context.CurrentHandler is Page)
        //{
        //    app.Response.Filter = new WebResourceFilter(app.Response.Filter);
        //}
    }
    private void context_PostBeginRequest(object sender, EventArgs e)
    {
        HttpApplication app = sender as HttpApplication;
        if (app.Context.CurrentHandler is Page)
        {
            app.Response.Filter = new WebResourceFilter(app.Response.Filter);
        }
    }
    /// <summary>
    /// Handles the EndRequest event of the context control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void context_EndRequest(object sender, EventArgs e)
    {
        if ((!IsEncodingAccepted(DEFLATE)))
            return;

        HttpApplication app = (HttpApplication)sender;
        string key = app.Request.QueryString.ToString();

        if ((app.Request.Path.Contains("CssResource.axd") ||  app.Request.Path.Contains("JsResource.axd") || app.Request.Path.Contains("ScriptResource.axd")) && app.Context.Request.QueryString["c"] == null)
        {
            if (app.Application[key] == null)
            {
                AddCompressedBytesToCache(app, key);
            }

            SetEncoding((string)app.Application[key + "enc"]);
            app.Context.Response.ContentType = (string)app.Application[key + " contentType"];
            app.Context.Response.BinaryWrite((byte[])app.Application[key]);
        }
    }

    /// <summary>
    /// Sets the caching headers and monitors the If-None-Match request header,
    /// to save bandwidth and CPU time.
    /// </summary>
    private static void SetCachingHeaders(HttpApplication app)
    {
        string etag = "\"" + app.Context.Request.QueryString.ToString().GetHashCode().ToString() + "\"";
        string incomingEtag = app.Request.Headers["If-None-Match"];

        app.Response.Cache.VaryByHeaders["Accept-Encoding"] = true;
        app.Response.Cache.SetOmitVaryStar(true);
        app.Response.Cache.SetExpires(DateTime.Now.AddDays(7));
        app.Response.Cache.SetCacheability(HttpCacheability.Public);
        //app.Response.Cache.SetLastModified(DateTime.Now.AddDays(-30));
        app.Response.Cache.SetETag(etag);

        if (String.Compare(incomingEtag, etag) == 0)
        {
            app.Response.StatusCode = (int)HttpStatusCode.NotModified;
            app.Response.End();
        }
    }

    /// <summary>
    /// Adds a compressed byte array into the application items.
    /// <remarks>
    /// This is done for performance reasons so it doesn't have to
    /// create an HTTP request every time it serves the ScriptResource.axd.
    /// </remarks>
    /// </summary>
    private static void AddCompressedBytesToCache(HttpApplication app, string key)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(app.Context.Request.Url.OriginalString + "&c=1");
        request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
        {
            using (MemoryStream ms = CompressResponse(response, app, key))
            {
                app.Application.Add(key, ms.ToArray());
                app.Application.Add(key + " contentType", response.ContentType);
            }
        }
    }

    /// <summary>
    /// Compresses the response stream if the browser allows it.
    /// </summary>
    private static MemoryStream CompressResponse(HttpWebResponse response, HttpApplication app, string key)
    {
        Stream responseStream = response.GetResponseStream();
        MemoryStream dataStream = new MemoryStream();
        StreamCopy(responseStream, dataStream);
        responseStream.Dispose();

        byte[] buffer = dataStream.ToArray();
        dataStream.Dispose();

        MemoryStream ms = new MemoryStream();
        Stream compress = ms;

        if (IsEncodingAccepted(DEFLATE))
        {
            compress = new DeflateStream(compress, CompressionMode.Compress);
            app.Application.Add(key + "enc", DEFLATE);
        }

        if (response.ContentType.StartsWith("text/javascript",StringComparison.InvariantCultureIgnoreCase))
        {
            //minify the js
            compress = new JavascriptMinifierStream(compress);
        }
        else if (response.ContentType.StartsWith("text/css", StringComparison.InvariantCultureIgnoreCase))
        {
            //minify the css
            compress = new CssMinifierStream(compress);
        }
        else if (response.ContentType.StartsWith("text/html", StringComparison.InvariantCultureIgnoreCase))
        {
            compress = new HtmlMinifierStream(compress);
           // compress = new WebResourceFilter(compress);
            //compress = new InlineStyleMinifierStream(compress);
            //compress = new InlineJavascriptMinifierStream(compress);

        }
        compress.Write(buffer, 0, buffer.Length);
        compress.Dispose();
        return ms;
    }

    /// <summary>
    /// Copies one stream into another.
    /// </summary>
    private static void StreamCopy(Stream input, Stream output)
    {
        byte[] buffer = new byte[2048];
        int read;
        do
        {
            read = input.Read(buffer, 0, buffer.Length);
            output.Write(buffer, 0, read);
        } while (read > 0);
    }

    #endregion

    private void Page_Load_CSS_Combiner(object sender, EventArgs e)
    {
        Page page = sender as Page;
        if (page == null) return;
        if (page.Header == null) return;
        List<String> cssHrefs = new List<String>();
        List<HtmlLink> controlsToRemove = new List<HtmlLink>();
        foreach (Control control in page.Header.Controls)
        {
            HtmlLink HtmlLink = control as HtmlLink;
            if (HtmlLink != null)
            {
                if ("stylesheet".Equals(HtmlLink.Attributes["rel"]) && "text/css".Equals(HtmlLink.Attributes["type"]) && ("all".Equals(HtmlLink.Attributes["media"]) || String.IsNullOrEmpty(HtmlLink.Attributes["media"])))
                {
                    try
                    {
                        if (VirtualPathUtility.IsAbsolute(HtmlLink.Href))
                        {
                            cssHrefs.Add(HtmlLink.Href);
                            controlsToRemove.Add(HtmlLink);
                        }
                    }
                    catch (HttpException)
                    {
                        //HtmlLink.Href is not a virtual path, so ignore this reference
                    }
                }
            }
        }
        foreach (HtmlLink control in controlsToRemove)
        {
            page.Header.Controls.Remove(control);
        }
        if (cssHrefs.Count > 0)
        {
            HtmlLink CombinedCssControl = new HtmlLink();
            CombinedCssControl.Attributes["type"] = "text/css";
            CombinedCssControl.Attributes["rel"] = "stylesheet";
            CombinedCssControl.Href = VirtualPathUtility.ToAbsolute("~/CssResource.axd") + "?d=" + HttpUtility.UrlEncode(String.Join(",", cssHrefs.ToArray()));
            page.Header.Controls.Add(CombinedCssControl);
        }
    }

}