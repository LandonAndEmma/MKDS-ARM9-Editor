using Avalonia.Controls;
using Avalonia.Platform.Storage;
namespace ARM9Editor;
public sealed class FileService
{
    private const int MinFileSize = 1024 * 1024;
    private static readonly FilePickerFileType BinaryFileType = new("Binary files")
    {
        Patterns = new[] { "*.bin" }
    };
    public static async Task<(byte[]? Data, string? Path)> OpenFileAsync(Window? owner)
    {
        if (owner?.StorageProvider == null)
        {
            return (null, null);
        }
        IReadOnlyList<IStorageFile> files = await owner.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open ARM9.bin File",
            AllowMultiple = false,
            FileTypeFilter = new[] { BinaryFileType }
        });
        if (files.Count == 0)
        {
            return (null, null);
        }
        IStorageFile file = files[0];
        try
        {
            await using Stream stream = await file.OpenReadAsync();
            using MemoryStream memory = new();
            await stream.CopyToAsync(memory);
            byte[] data = memory.ToArray();
            return data.Length == 0
                ? throw new InvalidDataException("File is empty.")
                : data.Length < MinFileSize
                ? throw new InvalidDataException($"File is too small. Expected at least {MinFileSize / 1024}KB.")
                : ((byte[]? Data, string? Path))(data, file.Path.LocalPath);
        }
        catch (Exception ex)
        {
            throw new IOException($"Failed to read file: {ex.Message}", ex);
        }
    }
    public static async Task SaveFileAsync(string path, byte[] data)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentException("Invalid file path.", nameof(path));
        }
        if (data == null || data.Length == 0)
        {
            throw new ArgumentException("No data to save.", nameof(data));
        }
        try
        {
            await File.WriteAllBytesAsync(path, data);
        }
        catch (Exception ex)
        {
            throw new IOException($"Failed to save file: {ex.Message}", ex);
        }
    }
    public static async Task<string?> SaveFileAsAsync(Window? owner)
    {
        if (owner?.StorageProvider == null)
        {
            return null;
        }
        IStorageFile? file = await owner.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save ARM9.bin File",
            DefaultExtension = "bin",
            FileTypeChoices = new[] { BinaryFileType }
        });
        return file?.Path.LocalPath;
    }
}