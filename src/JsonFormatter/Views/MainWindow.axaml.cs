using Avalonia.Controls;

namespace JsonFormatter.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        var input = this.FindControl<TextBox>("Input");
        input.Watermark = "{ \"property': \"value\" }";
    }
}