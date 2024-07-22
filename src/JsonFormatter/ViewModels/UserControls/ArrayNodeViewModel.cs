using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace JsonFormatter.ViewModels.UserControls;

public partial class ArrayNodeViewModel : ObservableObject
{
    public ArrayNodeViewModel(List<ValueNodeViewModel> items, short nesting, string? propertyName = null)
    {
        _ = items ?? throw new ArgumentException(null, nameof(items));

        indentation = new Thickness(nesting * Constants.IndentationWidth, 0, 0, 0);
        if (propertyName != null)
        {
            isProperty = true;
            this.propertyName = $"{propertyName}: ";
        }

        if (items.Count == 0)
        {
            empty = collapsed = true;
        }
        else
        {
            items[^1].EndsWithComma = false;

            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        if (empty)
        {
            emptyClosingSymbol = "[],";
        }
        else
        {
            fullClosingSymbol = "],";
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
    private string? emptyClosingSymbol;

    [ObservableProperty]
    private string? fullClosingSymbol;

    public void SetLast()
    {
        if (EmptyClosingSymbol != null)
        {
            EmptyClosingSymbol = "[]";
        }

        if (FullClosingSymbol != null)
        {
            FullClosingSymbol = "]";
        }
    }

    public ObservableCollection<ValueNodeViewModel> Items { get; set; } = [];
}
