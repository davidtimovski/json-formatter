using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace JsonFormatter.UserControls;

public partial class Viewer : UserControl
{
    public Viewer()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
