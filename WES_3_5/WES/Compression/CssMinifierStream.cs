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

public class CssMinifierStream : Stream
{
  //  private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    #region Constructors

    /// <summary>
    /// Creates a new Minify Filter for the provided stream
    /// </summary>
    public CssMinifierStream(Stream stream)
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
        string css = Encoding.UTF8.GetString(buffer);
        try
        {
            //css = CssCompressor.Compress(css);
            //css = CssMinifier.CssMinify(css);
            AspMinifier.Minifier min = new AspMinifier.Minifier();
            css = min.Minify(css, "css");            
        }
        catch (Exception ex)
        {
            //_log.Error("Error running YUI on CSS from " + HttpContext.Current.Request.Url, ex);
        }

        //finally, write the new stream
        byte[] output = Encoding.UTF8.GetBytes(css);
        this._UnderlyingStream.Write(output, 0, output.Length);

    }

    #endregion

}