namespace ARM9Editor;
public enum EditorTab
{
    Music,
    Courses,
    Weather,
    Emblems,
    Karts,
    Characters
}
public enum EditorType
{
    Byte,
    VariableString,
    FixedString
}
public sealed record TabConfig(string Header, EditorTab Type, string LeftLabel, string RightLabel);
public sealed record EditorConfig(string Name, EditorType Type, int Offset, int MaxLength = 0, int MinValue = 0, int MaxValue = 255);
public sealed class ARM9Data
{
    private byte[]? _data;
    public bool IsLoaded => _data is { Length: > 0 };
    public int Length => _data?.Length ?? 0;
    public void Load(byte[] data)
    {
        _data = data ?? throw new ArgumentNullException(nameof(data));
    }
    public byte GetByte(int offset)
    {
        return IsLoaded && offset >= 0 && offset < _data!.Length ? _data[offset] : (byte)0;
    }
    public void SetByte(int offset, byte value)
    {
        if (IsLoaded && offset >= 0 && offset < _data!.Length)
        {
            _data[offset] = value;
        }
    }
    public string GetString(int offset, int length)
    {
        if (!IsLoaded || offset < 0 || offset + length > _data!.Length)
        {
            return string.Empty;
        }
        ReadOnlySpan<byte> span = new(_data, offset, length);
        int nullIndex = span.IndexOf((byte)0);
        return System.Text.Encoding.ASCII.GetString(nullIndex >= 0 ? span[..nullIndex] : span);
    }
    public void SetString(int offset, int length, string value)
    {
        if (!IsLoaded || offset < 0 || offset + length > _data!.Length)
        {
            return;
        }
        byte[] bytes = System.Text.Encoding.ASCII.GetBytes(value);
        int copyLen = Math.Min(bytes.Length, length);
        Array.Copy(bytes, 0, _data, offset, copyLen);
        Array.Clear(_data, offset + copyLen, length - copyLen);
    }
    public byte[] ToArray()
    {
        return _data?.ToArray() ?? Array.Empty<byte>();
    }
}