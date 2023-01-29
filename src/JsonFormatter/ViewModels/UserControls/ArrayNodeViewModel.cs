using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Avalonia;
using DynamicData;
using ReactiveUI;

namespace JsonFormatter.ViewModels.UserControls;

public class ArrayNodeViewModel : ViewModelBase
{
    public ArrayNodeViewModel()
    {
        OnCollapseCommand = ReactiveCommand.Create(() => { Collapsed = true; });
        OnExpandCommand = ReactiveCommand.Create(() => { Collapsed = false; });
    }
    
    public ArrayNodeViewModel(List<ValueNodeViewModel> items, short nesting, string? propertyName = null) : this()
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
            Items.AddRange(items);
        }
        else
        {
            empty = collapsed = true;
        }
    }
    
    private bool empty;
    public bool Empty
    {
        get => empty;
        set => this.RaiseAndSetIfChanged(ref empty, value);
    }

    public ObservableCollection<ValueNodeViewModel> Items { get; set; } = new();
    
    private ReactiveCommand<Unit, Unit> OnCollapseCommand { get; }
    private ReactiveCommand<Unit, Unit> OnExpandCommand { get; }
    
    private bool collapsed;
    public bool Collapsed
    {
        get => collapsed;
        set => this.RaiseAndSetIfChanged(ref collapsed, value);
    }
    
    private Thickness indentation;
    public Thickness Indentation
    {
        get => indentation;
        set => this.RaiseAndSetIfChanged(ref indentation, value);
    }

    private bool isProperty;
    public bool IsProperty
    {
        get => isProperty;
        set => this.RaiseAndSetIfChanged(ref isProperty, value);
    }
    
    private string? propertyName;
    public string? PropertyName
    {
        get => propertyName;
        set => this.RaiseAndSetIfChanged(ref propertyName, value);
    }
    
    private bool last;
    public bool Last
    {
        get => last;
        set => this.RaiseAndSetIfChanged(ref last, value);
    }
}
