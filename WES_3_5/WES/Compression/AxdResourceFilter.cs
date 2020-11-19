/// <summary>
/// Minifies HTML before sending it to the output
/// </summary>
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
//using Yahoo.Yui.Compressor;
using System;
using System.Web;
//using log4net;
using System.Reflection;

using System.IO.Compression;
using System.Web.Caching;
using System.Net;
using System.Collections.Generic;
public class WebResourceFilter : Stream
{

    public WebResourceFilter(Stream sink)
    {
        _sink = sink;
    }

    private Stream _sink;

    #region Properites

    public override bool CanRead
    {
        get { return true; }
    }

    public override bool CanSeek
    {
        get { return true; }
    }

    public override bool CanWrite
    {
        get { return true; }
    }

    public override void Flush()
    {
        _sink.Flush();
    }

    public override long Length
    {
        get { return 0; }
    }

    private long _position;
    public override long Position
    {
        get { return _position; }
        set { _position = value; }
    }

    #endregion

    #region Methods

    public override int Read(byte[] buffer, int offset, int count)
    {
        return _sink.Read(buffer, offset, count);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        return _sink.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
        _sink.SetLength(value);
    }

    public override void Close()
    {
        _sink.Close();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        byte[] data = new byte[count];
        Buffer.BlockCopy(buffer, offset, data, 0, count);
        string html = System.Text.Encoding.Default.GetString(buffer);
        int index = 0;
        List<string> list = new List<string>();

        Regex regex = new Regex("<script\\s*src=\"((?=[^\"]*(webresource.axd|scriptresource.axd))[^\"]*)\"\\s*type=\"text/javascript\"[^>]*>[^<]*(?:</script>)?", RegexOptions.IgnoreCase);
        foreach (Match match in regex.Matches(html))
        {
            if (index == 0)
                index = html.IndexOf(match.Value);

            string relative = match.Groups[1].Value;
            list.Add(relative);
            html = html.Replace(match.Value, string.Empty);
        }

        if (index > 0)
        {
            string script = "<script type=\"text/javascript\" src=\"js.axd?path={0}\"></script>";
            string path = string.Empty;
            foreach (string s in list)
            {
                path += HttpUtility.UrlEncode(s) + ",";
            }

            html = html.Insert(index, string.Format(script, path));
        }

        byte[] outdata = System.Text.Encoding.Default.GetBytes(html);
        _sink.Write(outdata, 0, outdata.GetLength(0));
    }

    #endregion

}