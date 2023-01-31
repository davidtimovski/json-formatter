using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace JsonFormatter.UserControls;

public partial class ObjectNode : UserControl
{
    public ObjectNode()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    private void SelectableTextBlock_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (sender == null)
        {
            return;
        }
        
        var textBox = (SelectableTextBlock)sender;
        textBox.SelectAll();
    }
}
