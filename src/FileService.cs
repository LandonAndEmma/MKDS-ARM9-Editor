using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
namespace ARM9Editor;

public sealed class FileService
{
    private const int MinFileSize = 1024 * 1024;
    private static readonly FilePickerFileType BinaryFileType = new("Binary files")
    {
        Patterns = new[] { "*.bin" }
    };
    private static readonly FilePickerFileType JsonFileType = new("JSON files")
    {
        Patterns = new[] { "*.json" }
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
    [RequiresUnreferencedCode("Calls Newtonsoft.Json.JsonConvert.SerializeObject")]
    public static async Task<string?> ExportChangesAsync(Window? owner, ChangesExport changes)
    {
        if (owner?.StorageProvider == null)
        {
            return null;
        }
        IStorageFile? file = await owner.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Export Changes To JSON",
            DefaultExtension = "json",
            SuggestedFileName = "arm9_changes.json",
            FileTypeChoices = new[] { JsonFileType }
        });
        if (file == null)
        {
            return null;
        }
        try
        {
            string json = JsonConvert.SerializeObject(changes, Formatting.Indented);
            await File.WriteAllTextAsync(file.Path.LocalPath, json);
            return file.Path.LocalPath;
        }
        catch (Exception ex)
        {
            throw new IOException($"Failed to export changes: {ex.Message}", ex);
        }
    }
    [RequiresUnreferencedCode("Calls Newtonsoft.Json.JsonConvert.DeserializeObject")]
    public static async Task<ChangesExport?> ImportChangesAsync(Window? owner)
    {
        if (owner?.StorageProvider == null)
        {
            return null;
        }
        IReadOnlyList<IStorageFile> files = await owner.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Import Changes From JSON",
            AllowMultiple = false,
            FileTypeFilter = new[] { JsonFileType }
        });
        if (files.Count == 0)
        {
            return null;
        }
        IStorageFile file = files[0];
        try
        {
            string json = await File.ReadAllTextAsync(file.Path.LocalPath);
            ChangesExport? changes = JsonConvert.DeserializeObject<ChangesExport>(json);
            return changes ?? throw new InvalidDataException("Invalid JSON format.");
        }
        catch (JsonException ex)
        {
            throw new InvalidDataException($"Failed to parse JSON: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new IOException($"Failed to import changes: {ex.Message}", ex);
        }
    }
}