
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
public sealed class ChangeRecord
{
    public int Offset { get; set; }
    public string? OriginalValue { get; set; }
    public string? NewValue { get; set; }
    public string Type { get; set; } = "byte";
    public int? Length { get; set; }
}
public sealed class ChangesExport
{
    public List<ChangeRecord> Changes { get; set; } = [];
}
public sealed class ARM9Data
{
    private byte[]? _data;
    private byte[]? _originalData;
    private readonly Dictionary<int, byte> _byteChanges = [];
    private readonly Dictionary<string, string> _stringChanges = [];
    public bool IsLoaded => _data is { Length: > 0 };
    public int Length => _data?.Length ?? 0;
    public void Load(byte[] data)
    {
        _data = data ?? throw new ArgumentNullException(nameof(data));
        _originalData = (byte[])data.Clone();
        _byteChanges.Clear();
        _stringChanges.Clear();
    }
    public byte GetByte(int offset)
    {
        return IsLoaded && offset >= 0 && offset < _data!.Length ? _data[offset] : (byte)0;
    }
    public void SetByte(int offset, byte value)
    {
        if (IsLoaded && offset >= 0 && offset < _data!.Length)
        {
            if (_originalData![offset] != value)
            {
                _byteChanges[offset] = value;
            }
            else
            {
                _ = _byteChanges.Remove(offset);
            }
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
        string originalValue = GetOriginalString(offset, length);
        if (originalValue != value)
        {
            _stringChanges[$"{offset}:{length}"] = value;
        }
        else
        {
            _ = _stringChanges.Remove($"{offset}:{length}");
        }
    }
    private string GetOriginalString(int offset, int length)
    {
        if (_originalData == null || offset < 0 || offset + length > _originalData.Length)
        {
            return string.Empty;
        }
        ReadOnlySpan<byte> span = new(_originalData, offset, length);
        int nullIndex = span.IndexOf((byte)0);
        return System.Text.Encoding.ASCII.GetString(nullIndex >= 0 ? span[..nullIndex] : span);
    }
    public ChangesExport ExportChanges()
    {
        ChangesExport export = new();
        foreach (KeyValuePair<int, byte> kvp in _byteChanges)
        {
            export.Changes.Add(new ChangeRecord
            {
                Offset = kvp.Key,
                OriginalValue = _originalData![kvp.Key].ToString(),
                NewValue = kvp.Value.ToString(),
                Type = "byte"
            });
        }
        foreach (KeyValuePair<string, string> kvp in _stringChanges)
        {
            string[] parts = kvp.Key.Split(':');
            int offset = int.Parse(parts[0]);
            int length = int.Parse(parts[1]);
            export.Changes.Add(new ChangeRecord
            {
                Offset = offset,
                OriginalValue = GetOriginalString(offset, length),
                NewValue = kvp.Value,
                Type = "string",
                Length = length
            });
        }
        return export;
    }
    public void ImportChanges(ChangesExport import)
    {
        if (!IsLoaded)
        {
            throw new InvalidOperationException("No file loaded.");
        }
        foreach (ChangeRecord change in import.Changes)
        {
            if (change.Type == "byte" && change.NewValue != null)
            {
                if (byte.TryParse(change.NewValue, out byte value))
                {
                    SetByte(change.Offset, value);
                }
            }
            else if (change.Type == "string" && change.NewValue != null && change.Length.HasValue)
            {
                SetString(change.Offset, change.Length.Value, change.NewValue);
            }
        }
    }
    public bool HasChanges()
    {
        return _byteChanges.Count > 0 || _stringChanges.Count > 0;
    }
    public byte[] ToArray()
    {
        return _data?.ToArray() ?? Array.Empty<byte>();
    }
}