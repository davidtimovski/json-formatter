using Avalonia.Controls;
using Avalonia.Input;

namespace JsonFormatter.UserControls;

public partial class ValueNode : UserControl
{
    public ValueNode()
    {
        InitializeComponent();
    }

    private void TextBox_OnPointerReleased(object? sender, PointerReleasedEventArgs _)
    {
        if (sender is null)
        {
            return;
        }

        var textBox = (SelectableTextBlock)sender;
        textBox.SelectAll();
    }
}
