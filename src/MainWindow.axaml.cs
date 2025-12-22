using Avalonia.Controls;
using Avalonia.Interactivity;
namespace ARM9Editor;
public partial class MainWindow : Window
{
    private MainWindowViewModel? ViewModel => DataContext as MainWindowViewModel;
    public MainWindow()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }
    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (ViewModel != null)
        {
            ViewModel.Owner = this;
            ViewModel.PropertyChanged += (_, args) =>
            {
                if (args.PropertyName == nameof(ViewModel.IsFileLoaded))
                {
                    PopulateTabs();
                }
            };
        }
    }
    private void PopulateTabs()
    {
        TabControl? tabControl = this.FindControl<TabControl>("MainTabControl");
        if (tabControl == null || ViewModel == null || !ViewModel.IsFileLoaded)
        {
            return;
        }
        tabControl.Items.Clear();
        foreach (EditorTab tabType in Enum.GetValues<EditorTab>())
        {
            TabConfig config = ConfigurationService.Instance.GetTabConfig(tabType);
            _ = tabControl.Items.Add(new TabItem
            {
                Header = config.Header,
                Content = new EditorContent(tabType, ViewModel)
            });
        }
        tabControl.SelectedIndex = 0;
    }
    private async void OnOpenClick(object? sender, RoutedEventArgs e)
    {
        if (ViewModel != null)
        {
            await ViewModel.OpenFileAsync();
        }
    }
    private async void OnSaveClick(object? sender, RoutedEventArgs e)
    {
        if (ViewModel != null)
        {
            await ViewModel.SaveFileAsync();
        }
    }
    private async void OnSaveAsClick(object? sender, RoutedEventArgs e)
    {
        if (ViewModel != null)
        {
            await ViewModel.SaveFileAsAsync();
        }
    }
    private async void OnInfoClick(object? sender, RoutedEventArgs e)
    {
        if (ViewModel != null)
        {
            await ViewModel.ShowInfoAsync();
        }
    }
    private async void OnRepositoryClick(object? sender, RoutedEventArgs e)
    {
        if (ViewModel != null)
        {
            await ViewModel.OpenRepositoryAsync();
        }
    }
}