using Avalonia.Controls;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
namespace ARM9Editor;
public sealed class MainWindowViewModel : INotifyPropertyChanged
{
    private const string DefaultTitle = "Mario Kart DS ARM9 Editor";
    private const string RepoUrl = "https://github.com/LandonAndEmma/MKDS-ARM9-Editor";
    private string? _filePath;
    public event PropertyChangedEventHandler? PropertyChanged;
    public Window? Owner { get; set; }
    public ARM9Data Data { get; } = new();
    public bool IsFileLoaded => Data.IsLoaded;
    public string Status { get; private set => SetField(ref field, value); } = "Ready";
    public string Title { get; private set => SetField(ref field, value); } = DefaultTitle;
    public async Task OpenFileAsync()
    {
        try
        {
            (byte[]? data, string? path) = await FileService.OpenFileAsync(Owner);
            if (data == null || path == null)
            {
                return;
            }
            Data.Load(data);
            _filePath = path;
            string fileName = Path.GetFileName(_filePath);
            Title = $"{DefaultTitle} - {fileName}";
            Status = $"Loaded: {fileName}";
            OnPropertyChanged(nameof(IsFileLoaded));
        }
        catch (Exception ex)
        {
            await DialogService.ShowErrorAsync(Owner, ex.Message);
        }
    }
    public async Task SaveFileAsync()
    {
        if (!IsFileLoaded)
        {
            await DialogService.ShowErrorAsync(Owner, "No file loaded.");
            return;
        }
        if (string.IsNullOrEmpty(_filePath))
        {
            await SaveFileAsAsync();
            return;
        }
        try
        {
            await FileService.SaveFileAsync(_filePath, Data.ToArray());
            Status = $"Saved: {Path.GetFileName(_filePath)}";
            await DialogService.ShowMessageAsync(Owner, "Success", "File saved successfully.");
        }
        catch (Exception ex)
        {
            await DialogService.ShowErrorAsync(Owner, ex.Message);
        }
    }
    public async Task SaveFileAsAsync()
    {
        if (!IsFileLoaded)
        {
            await DialogService.ShowErrorAsync(Owner, "No file loaded.");
            return;
        }
        try
        {
            string? path = await FileService.SaveFileAsAsync(Owner);
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            await FileService.SaveFileAsync(path, Data.ToArray());
            _filePath = path;
            string fileName = Path.GetFileName(_filePath);
            Title = $"{DefaultTitle} - {fileName}";
            Status = $"Saved: {fileName}";
            await DialogService.ShowMessageAsync(Owner, "Success", "File saved successfully.");
        }
        catch (Exception ex)
        {
            await DialogService.ShowErrorAsync(Owner, ex.Message);
        }
    }
    public async Task ShowInfoAsync()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        Version? version = assembly.GetName().Version;
        var versionStr = version != null ? $"{version.Major}.{version.Minor}.{version.Build}" : "Unknown";
        AssemblyCompanyAttribute? companyAttr = assembly.GetCustomAttribute<AssemblyCompanyAttribute>();
        var message = $"Mario Kart DS ARM9 Editor\nVersion: {versionStr}\n\nEdit values in Mario Kart DS ARM9 files.\n\n";
        if (companyAttr != null)
        {
            message += $"By: {companyAttr.Company}\n";
        }
        message += "Special Thanks: Ermelber, Yami, MkDasher";
        await DialogService.ShowMessageAsync(Owner, "Info", message);
    }
    public async Task OpenRepositoryAsync()
    {
        try
        {
            _ = Process.Start(new ProcessStartInfo
            {
                FileName = RepoUrl,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            await DialogService.ShowErrorAsync(Owner, $"Cannot open URL: {ex.Message}");
        }
    }
    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (!Equals(field, value))
        {
            field = value;
            OnPropertyChanged(propertyName);
        }
    }
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}