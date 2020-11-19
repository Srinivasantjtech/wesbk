using System.Web;
using System.Text.RegularExpressions;
using System;
using System.Text;

using System.IO;
using System.Reflection;
public class InlineStyleMinifierStream : ReplaceFilterStream
{
    //private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    // Match any <style> tag that does not have a src attribute, and include its inner content
    private static readonly Regex StyleWithoutSrc = new Regex(@"<styles?(?:(?!src).)*/style>",
        RegexOptions.Compiled |RegexOptions.IgnoreCase |RegexOptions.Singleline |RegexOptions.Multiline);
    // Match the content within the matched <style> tag
    private static readonly Regex StyleContent = new Regex(@">[^>]*(.*)<",
        RegexOptions.Compiled |RegexOptions.IgnoreCase |RegexOptions.Singleline | RegexOptions.Multiline);
    public InlineStyleMinifierStream(Stream stream)
        : base(stream)
    {
    }
    public override Regex ReplacePattern
    {
        get { return StyleContent; }
    }
    public override Regex SubjectPattern
    {
        get { return StyleWithoutSrc; }
    }
    public override string Replace(Match m)
    {
        if (!String.IsNullOrEmpty(m.Value))
        {
            var context = HttpContext.Current;
            var inner = m.Value.Substring(1, m.Value.Length - 2);
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
                    //builder.Append(CssCompressor.Compress(inner));
                   // builder.Append(CssMinifier.CssMinify(inner));
                    AspMinifier.Minifier min = new AspMinifier.Minifier();
                    builder.Append(min.Minify(inner, "css"));
                }
                catch (Exception ex)
                {
                   // _log.Error("Error minifying inline CSS in " + context.Request.Url, ex);
                    return m.Value;
                }
                builder.Append("<");
                return builder.ToString();
            }
        }
        return m.Value;
    }
}