using Avalonia.Controls;
using Avalonia.Interactivity;
using JsonFormatter.ViewModels;
using Avalonia.Threading;
using ReactiveUI;

namespace JsonFormatter.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        var input = this.FindControl<TextBox>("Input");
        input.Watermark = "{ \"property': \"value\" }";
        
        var formatButton = this.FindControl<Button>("FormatButton");
        formatButton.Command = ReactiveCommand.Create(FormatJson);
    }

    private void FormatJson()
    {
        var vm = (MainWindowViewModel)DataContext!;
        vm.FormatButtonDisabled = true;

        Dispatcher.UIThread.Post(() =>
        {
            vm.FormatJson();
            vm.FormatButtonDisabled = false;
        }, DispatcherPriority.Background);
    }

    private void Input_OnPastingFromClipboard(object? sender, RoutedEventArgs e)
    {
        FormatJson();
    }
}