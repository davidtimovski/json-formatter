using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;

namespace JsonFormatter.UserControls;

public partial class ArrayNode : UserControl
{
    public ArrayNode()
    {
        InitializeComponent();
        
        // Hack, find a way to do it with MVVM
        
        // Array
        var expandButton = this.FindControl<Button>("ExpandButton");
        var collapseButton = this.FindControl<Button>("CollapseButton");
        
        var collapsedClosingBracket = this.FindControl<TextBlock>("CollapsedClosingBracket");

        expandButton.Command = ReactiveCommand.Create(() =>
        {
            var items = this.FindControl<ItemsControl>("Items");
            items.IsVisible = true;
            
            expandButton.IsVisible = false;
            collapseButton.IsVisible = true;
            collapsedClosingBracket.IsVisible = false;
            
            var closingBracket = this.FindControl<Border>("ClosingBracket");
            closingBracket.IsVisible = true;
        });
        collapseButton.Command = ReactiveCommand.Create(() =>
        {
            var items = this.FindControl<ItemsControl>("Items");
            items.IsVisible = false;
            
            collapseButton.IsVisible = false;
            expandButton.IsVisible = true;
            collapsedClosingBracket.IsVisible = true;
            
            var closingBracket = this.FindControl<Border>("ClosingBracket");
            closingBracket.IsVisible = false;
        });
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
