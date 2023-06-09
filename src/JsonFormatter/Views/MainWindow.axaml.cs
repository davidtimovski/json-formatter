using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Threading;
using JsonFormatter.ViewModels;

namespace JsonFormatter.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        Input.Watermark = "{ \"property\": \"value\" }";
        FormatButton.Click += FormatJson;
    }
    
    private void FormatJson(object? sender, RoutedEventArgs e)
    {
        var vm = (MainWindowViewModel)DataContext!;
        vm.StartFormatting();
    
        Dispatcher.UIThread.Post(() =>
        {
            if (!vm.Format())
            {
                FlyoutBase.ShowAttachedFlyout(InputWrap);
            }
        }, DispatcherPriority.Background);
    }
    
    private void Input_OnPastingFromClipboard(object? sender, RoutedEventArgs e)
    {
        FormatJson(sender, e);
    }
}