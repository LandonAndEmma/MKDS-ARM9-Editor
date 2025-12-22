using Avalonia.Controls;
namespace ARM9Editor;
public static class DialogService
{
    public static async Task ShowErrorAsync(Window? owner, string message)
    {
        await ShowMessageAsync(owner, "Error", message);
    }
    public static async Task ShowMessageAsync(Window? owner, string title, string message)
    {
        if (owner == null)
        {
            return;
        }
        Window dialog = new()
        {
            Title = title,
            Width = 400,
            Height = 150,
            CanResize = false,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            SizeToContent = SizeToContent.Height
        };
        StackPanel stack = new()
        {
            Margin = new Avalonia.Thickness(20)
        };
        stack.Children.Add(new TextBlock
        {
            Text = message,
            TextWrapping = Avalonia.Media.TextWrapping.Wrap,
            Margin = new Avalonia.Thickness(0, 0, 0, 20)
        });
        Button button = new()
        {
            Content = "OK",
            Width = 80,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right
        };
        button.Click += (_, _) => dialog.Close();
        stack.Children.Add(button);
        dialog.Content = stack;
        await dialog.ShowDialog(owner);
    }
}