using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using JsonFormatter.ViewModels;

namespace JsonFormatter.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        var input = this.FindControl<TextBox>("Input");
        input.Watermark = "{ \"property\": \"value\" }";
        
        var formatButton = this.FindControl<Button>("FormatButton");
        formatButton.Click += FormatJson;
    }

    private void FormatJson(object? sender, RoutedEventArgs e)
    {
        var vm = (MainWindowViewModel)DataContext!;
        vm.StartFormatting();

        Dispatcher.UIThread.Post(() =>
        {
            vm.Format();
        }, DispatcherPriority.Background);
    }

    private void Input_OnPastingFromClipboard(object? sender, RoutedEventArgs e)
    {
        FormatJson(sender, e);
    }
}