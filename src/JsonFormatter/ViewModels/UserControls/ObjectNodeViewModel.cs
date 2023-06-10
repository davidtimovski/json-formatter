using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace JsonFormatter.ViewModels.UserControls;

public partial class ObjectNodeViewModel : ViewModelBase
{
    public ObjectNodeViewModel(List<ValueNodeViewModel> properties, short nesting, string? propertyName = null)
    {
        _ = properties ?? throw new ArgumentException(null, nameof(properties));
        
        indentation = new Thickness(nesting * Constants.IndentationWidth, 0, 0, 0);
        if (propertyName != null)
        {
            isProperty = true;
            this.propertyName = $"{propertyName}: ";
        }

        if (properties.Any())
        {
            properties[^1].EndsWithComma = false;
            
            foreach (var property in properties)
            {
                Properties.Add(property);
            }
        }
        else
        {
            empty = collapsed = true;
        }
        
        if (empty)
        {
            emptyClosingSymbol = "{}";
        }
        else
        {
            fullClosingSymbol = "}";
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
            EmptyClosingSymbol = "{}";
        }
            
        if (FullClosingSymbol != null)
        {
            FullClosingSymbol = "}";
        }
    }
    
    public ObservableCollection<ValueNodeViewModel> Properties { get; set; } = new();
}
