using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
namespace ARM9Editor;
public sealed class EditorContent : UserControl
{
    private readonly EditorTab _tabType;
    private readonly MainWindowViewModel _viewModel;
    public EditorContent(EditorTab tabType, MainWindowViewModel viewModel)
    {
        _tabType = tabType;
        _viewModel = viewModel;
        BuildContent();
    }
    private void BuildContent()
    {
        TabConfig config = ConfigurationService.Instance.GetTabConfig(_tabType);
        IReadOnlyList<EditorConfig> editors = ConfigurationService.Instance.GetEditorConfigs(_tabType);
        Grid mainGrid = new()
        {
            RowDefinitions = { new RowDefinition(GridLength.Auto), new RowDefinition(GridLength.Star) }
        };
        Grid header = CreateHeader(config.LeftLabel, config.RightLabel);
        Grid.SetRow(header, 0);
        mainGrid.Children.Add(header);
        StackPanel content = new()
        {
            Margin = new Thickness(0)
        };
        foreach (EditorConfig editor in editors)
        {
            content.Children.Add(CreateEditorRow(editor));
        }
        ScrollViewer scroll = new()
        {
            Content = content,
            VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto,
            HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Disabled
        };
        Grid.SetRow(scroll, 1);
        mainGrid.Children.Add(scroll);
        Content = mainGrid;
    }
    private Grid CreateHeader(string leftLabel, string rightLabel)
    {
        Grid grid = new()
        {
            Height = 40,
            Background = new SolidColorBrush(Color.FromArgb(255, 50, 50, 50)),
            ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(2, GridUnitType.Star) }
        };
        TextBlock left = new()
        {
            Text = leftLabel,
            FontWeight = FontWeight.Bold,
            FontSize = 14,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(15, 0)
        };
        Grid.SetColumn(left, 0);
        grid.Children.Add(left);
        TextBlock right = new()
        {
            Text = rightLabel,
            FontWeight = FontWeight.Bold,
            FontSize = 14,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(15, 0)
        };
        Grid.SetColumn(right, 1);
        grid.Children.Add(right);
        return grid;
    }
    private Border CreateEditorRow(EditorConfig config)
    {
        Grid grid = new()
        {
            MinHeight = 45,
            ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(2, GridUnitType.Star) }
        };
        TextBlock label = new()
        {
            Text = config.Name,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(15, 0)
        };
        Grid.SetColumn(label, 0);
        grid.Children.Add(label);
        Control editor = CreateEditor(config);
        Grid.SetColumn(editor, 1);
        grid.Children.Add(editor);
        return new Border
        {
            Child = grid,
            BorderThickness = new Thickness(0, 0, 0, 1),
            BorderBrush = new SolidColorBrush(Color.FromArgb(60, 128, 128, 128))
        };
    }
    private Control CreateEditor(EditorConfig config)
    {
        return config.Type switch
        {
            EditorType.Byte => CreateByteEditor(config),
            EditorType.VariableString => CreateVariableStringEditor(config),
            EditorType.FixedString => CreateFixedStringEditor(config),
            _ => new TextBlock { Text = "Unknown type" }
        };
    }
    private NumericUpDown CreateByteEditor(EditorConfig config)
    {
        NumericUpDown numeric = new()
        {
            Minimum = config.MinValue,
            Maximum = config.MaxValue,
            Value = _viewModel.Data.GetByte(config.Offset),
            Margin = new Thickness(10, 5, 15, 5),
            ShowButtonSpinner = true,
            FormatString = "0",
            HorizontalAlignment = HorizontalAlignment.Stretch
        };
        numeric.ValueChanged += (_, e) =>
        {
            if (e.NewValue.HasValue)
            {
                _viewModel.Data.SetByte(config.Offset, (byte)e.NewValue.Value);
            }
        };
        return numeric;
    }
    private TextBox CreateVariableStringEditor(EditorConfig config)
    {
        TextBox textBox = new()
        {
            Text = _viewModel.Data.GetString(config.Offset, config.MaxLength),
            Margin = new Thickness(10, 5, 15, 5),
            VerticalContentAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Stretch
        };
        textBox.LostFocus += async (_, _) =>
        {
            string text = textBox.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(text))
            {
                _viewModel.Data.SetString(config.Offset, config.MaxLength, string.Empty);
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(text, @"^[A-Za-z0-9_]+$"))
            {
                await DialogService.ShowErrorAsync(_viewModel.Owner, "Only alphanumeric characters and underscores allowed.");
                textBox.Text = _viewModel.Data.GetString(config.Offset, config.MaxLength);
                return;
            }
            if (System.Text.Encoding.UTF8.GetByteCount(text) > config.MaxLength)
            {
                await DialogService.ShowErrorAsync(_viewModel.Owner, $"Maximum length: {config.MaxLength} bytes.");
                textBox.Text = _viewModel.Data.GetString(config.Offset, config.MaxLength);
                return;
            }
            _viewModel.Data.SetString(config.Offset, config.MaxLength, text);
        };
        return textBox;
    }
    private TextBox CreateFixedStringEditor(EditorConfig config)
    {
        TextBox textBox = new()
        {
            Text = _viewModel.Data.GetString(config.Offset, config.MaxLength),
            MaxLength = config.MaxLength,
            Margin = new Thickness(10, 5, 15, 5),
            VerticalContentAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Stretch
        };
        textBox.LostFocus += async (_, _) =>
        {
            string text = textBox.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(text))
            {
                _viewModel.Data.SetString(config.Offset, config.MaxLength, string.Empty);
                return;
            }
            if (text.Length != config.MaxLength || !System.Text.RegularExpressions.Regex.IsMatch(text, @"^[A-Za-z0-9_]+$"))
            {
                await DialogService.ShowErrorAsync(_viewModel.Owner, $"Must be exactly {config.MaxLength} alphanumeric characters or underscores.");
                textBox.Text = _viewModel.Data.GetString(config.Offset, config.MaxLength);
                return;
            }
            _viewModel.Data.SetString(config.Offset, config.MaxLength, text);
        };
        return textBox;
    }
}