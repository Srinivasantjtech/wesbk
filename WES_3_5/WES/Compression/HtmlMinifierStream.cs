using System.IO;
using System.Text;
using System;
using System.Text.RegularExpressions;
public class HtmlMinifierStream : Stream
{
    private static readonly Regex reg = new Regex(@"(?<=[^])\t{2,}|(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,11}(?=[<])|(?=[\n])\s{2,}|\<\!--[^\[].*?--\>", RegexOptions.Compiled);

    /// <summary>
    /// Creates a new Minify Filter for the provided stream
    /// </summary>
    public HtmlMinifierStream(Stream stream)
    {
        this._UnderlyingStream = stream;
    }

    /// <summary>
    /// The actual output stream
    /// </summary>
    private Stream _UnderlyingStream;

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
        string html = Encoding.UTF8.GetString(buffer);

        //TODO translate the HTML minifier from http://code.google.com/p/minify/ and use that
        html = reg.Replace(html, string.Empty);
        AspMinifier.Minifier min = new AspMinifier.Minifier();
        html = min.Minify(html, "html");

        //finally, write the new stream
        byte[] output = Encoding.UTF8.GetBytes(html);
        this._UnderlyingStream.Write(output, 0, output.Length);

    }

    #endregion

}