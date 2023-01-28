using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DynamicData;
using ReactiveUI;

namespace JsonFormatter.ViewModels.UserControls;

public class ArrayNodeViewModel : ViewModelBase
{
    public ArrayNodeViewModel()
    {
    }
    
    public ArrayNodeViewModel(List<ValueNodeViewModel> items, short nesting, string? propertyName = null)
    {
        _ = items ?? throw new ArgumentException(null, nameof(items));
        
        indentation = new string(' ', nesting * Constants.IndentationSpaces);
        if (propertyName != null)
        {
            isProperty = true;
            this.propertyName = propertyName;
        }
        
        empty = !items.Any();
        Items.AddRange(items);
    }
    
    private bool empty;
    public bool Empty
    {
        get => empty;
        set => this.RaiseAndSetIfChanged(ref value, empty);
    }

    public ObservableCollection<ValueNodeViewModel> Items { get; set; } = new();
    
    
    private string indentation = "";
    public string Indentation
    {
        get => indentation;
        set => this.RaiseAndSetIfChanged(ref value, indentation);
    }

    private bool isProperty;
    public bool IsProperty
    {
        get => isProperty;
        set => this.RaiseAndSetIfChanged(ref value, isProperty);
    }
    
    private string? propertyName;
    public string? PropertyName
    {
        get => propertyName;
        set => this.RaiseAndSetIfChanged(ref value, propertyName);
    }
}
