using System.IO;
using System.Text;
using System.Text.RegularExpressions;
public abstract class ReplaceFilterStream : Stream
{
    private readonly Stream _stream;
    #region Stream Overrides
    protected ReplaceFilterStream(Stream stream)
    {
        _stream = stream;
    }
    public override bool CanRead
    {
        get { return _stream.CanRead; }
    }
    public override bool CanSeek
    {
        get { return _stream.CanSeek; }
    }
    public override bool CanWrite
    {
        get { return _stream.CanWrite; }
    }
    public override long Length
    {
        get { return _stream.Length; }
    }
    public override long Position
    {
        get { return _stream.Position; }
        set { _stream.Position = value; }
    }
    public override void Flush()
    {
        _stream.Flush();
    }
    public override int Read(byte[] buffer, int offset, int count)
    {
        return _stream.Read(buffer, offset, count);
    }
    public override long Seek(long offset, SeekOrigin origin)
    {
        return _stream.Seek(offset, origin);
    }
    public override void SetLength(long value)
    {
        _stream.SetLength(value);
    }
    public override void Write(byte[] buffer, int offset, int count)
    {
        var text = Encoding.Default.GetString(buffer);
        text = Find(text);
        var bytes = Encoding.Default.GetBytes(text);
        _stream.Write(bytes, 0, bytes.Length);
    }
    #endregion
    #region Private Methods
    private string Find(string html)
    {
        return SubjectPattern.Replace(html, new MatchEvaluator(Found));
    }
    public string Found(Match m)
    {
        return ReplacePattern.Replace(m.Value, new MatchEvaluator(Replace));
    }
    #endregion
    public abstract Regex SubjectPattern { get; }
    public abstract Regex ReplacePattern { get; }
    public abstract string Replace(Match m);
}