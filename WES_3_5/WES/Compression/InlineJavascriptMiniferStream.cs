using System.Web;
using System.Text.RegularExpressions;
using System;
using System.Text;
using Yahoo.Yui.Compressor;
using System.IO;
//using log4net;
using System.Reflection;
public class InlineJavascriptMinifierStream : ReplaceFilterStream
{
  //  private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    // Match any <script> tag that does not have a src attribute, and include its inner content
    private static readonly Regex ScriptWithoutSrc = new Regex(@"<script(?:(?!src).)*/script>",
        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);
    // Match the content within the matched <style> tag
    private static readonly Regex ScriptContent = new Regex(@">[^>]*(.*)<",
        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);
    public InlineJavascriptMinifierStream(Stream stream)
        : base(stream)
    {
    }
    public override Regex ReplacePattern
    {
        get { return ScriptContent; }
    }
    public override Regex SubjectPattern
    {
        get { return ScriptWithoutSrc; }
    }
    public override string Replace(Match m)
    {
        if (!String.IsNullOrEmpty(m.Value))
        {
            var context = HttpContext.Current;
            string  inner = m.Value.Substring(1, m.Value.Length - 2);
            if (context == null)
            {
                return m.Value;
            }
            else
            {
                // Replace the angle brackets and minify the contents
                StringBuilder builder = new StringBuilder(">");
                try
                {
                    
                    //builder.Append(JavaScriptCompressor.Compress(inner));
                    AspMinifier.Minifier min = new AspMinifier.Minifier();
                    

                    builder.Append(JsMinifier.GetMinifiedCode(inner));
                    //builder.Append(min.Minify(inner,"js"));
                }
                catch (Exception ex)
                {
                    //_log.Error("Error minifying inline javascript in " + context.Request.Url, ex);
                    return m.Value;
                }
                builder.Append("<");
                return builder.ToString();
            }
        }
        return m.Value;
    }
}