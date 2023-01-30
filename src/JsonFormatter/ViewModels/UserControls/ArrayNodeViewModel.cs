using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace JsonFormatter.ViewModels.UserControls;

public partial class ArrayNodeViewModel : ViewModelBase
{
    public ArrayNodeViewModel(List<ValueNodeViewModel> items, short nesting, string? propertyName = null)
    {
        _ = items ?? throw new ArgumentException(null, nameof(items));
        
        indentation = new Thickness(nesting * Constants.IndentationWidth, 0, 0, 0);
        if (propertyName != null)
        {
            isProperty = true;
            this.propertyName = propertyName;
        }

        if (items.Any())
        {
            items[^1].Last = true;

            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
        else
        {
            empty = collapsed = true;
        }
    }

    [RelayCommand]
    private void Collapse()
    {
        Collapsed = true;
    }

    [RelayCommand]
    private void Expand()
    {
        Collapsed = false;
    }
    
    [ObservableProperty]
    private bool empty;

    [ObservableProperty]
    private bool collapsed;

    [ObservableProperty]
    private Thickness indentation;

    [ObservableProperty]
    private bool isProperty;

    [ObservableProperty]
    private string? propertyName;

    [ObservableProperty]
    private bool last;
    
    public ObservableCollection<ValueNodeViewModel> Items { get; set; } = new();
}
