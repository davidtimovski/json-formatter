using Avalonia.Controls;
using Avalonia.Input;

namespace JsonFormatter.UserControls;

public partial class ObjectNode : UserControl
{
    public ObjectNode()
    {
        InitializeComponent();
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
