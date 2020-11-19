    /// <summary>
    /// Minifies HTML before sending it to the output
    /// </summary>
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
//using Yahoo.Yui.Compressor;
using System;
//using log4net;
using System.Reflection;
using System.Web;

public class JavascriptMinifierStream : Stream
{
  //  private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    #region Constructors

    /// <summary>
    /// Creates a new Minify Filter for the provided stream
    /// </summary>
    public JavascriptMinifierStream(Stream stream)
    {
        this._UnderlyingStream = stream;
    }

    #endregion

    #region Members

    /// <summary>
    /// The actual output stream
    /// </summary>
    private Stream _UnderlyingStream;

    #endregion

    #region Inherited Members

    public override bool CanRead
    {
        get { return this._UnderlyingStream.CanRead; }
    }

    public override bool CanSeek
    {
        get { return this._UnderlyingStream.CanSeek; }
    }

    public override bool CanWrite
    {
        get { return this._UnderlyingStream.CanWrite; }
    }

    public override void Flush()
    {
        this._UnderlyingStream.Flush();
    }

    public override long Length
    {
        get { return this._UnderlyingStream.Length; }
    }

    public override long Position
    {
        get
        {
            return this._UnderlyingStream.Position;
        }
        set
        {
            this._UnderlyingStream.Position = value;
        }
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        return this._UnderlyingStream.Read(buffer, offset, count);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        return this._UnderlyingStream.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
        this._UnderlyingStream.SetLength(value);
    }


    #endregion

    #region Minify Attempt

    /// <summary>
    /// Minify output before sending it to the underlying stream
    /// </summary>
    public override void Write(byte[] buffer, int offset, int count)
    {

        //format the string
        string javaScript = Encoding.UTF8.GetString(buffer);
        try
        {
           // javaScript = JavaScriptCompressor.Compress(javaScript, true, true, false, false, -1, Encoding.UTF8, System.Globalization.CultureInfo.InvariantCulture);
            javaScript = JsMinifier.GetMinifiedCode(javaScript);
            //AspMinifier.Minifier min = new AspMinifier.Minifier();
            //javaScript = min.Minify(javaScript,"js");
           
        }
        catch (Exception ex)
        {
            //_log.Error("Error running YUI on Javascript from " + HttpContext.Current.Request.Url, ex);
        }
        //finally, write the new stream
        byte[] output = Encoding.UTF8.GetBytes(javaScript);
        this._UnderlyingStream.Write(output, 0, output.Length);

    }

    #endregion
}