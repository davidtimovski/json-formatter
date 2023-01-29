using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Avalonia;
using DynamicData;
using ReactiveUI;

namespace JsonFormatter.ViewModels.UserControls;

public class ObjectNodeViewModel : ViewModelBase
{
    public ObjectNodeViewModel()
    {
        OnCollapseCommand = ReactiveCommand.Create(() => { Collapsed = true; });
        OnExpandCommand = ReactiveCommand.Create(() => { Collapsed = false; });
    }

    public ObjectNodeViewModel(List<ValueNodeViewModel> properties, short nesting, string? propertyName = null) : this()
    {
        _ = properties ?? throw new ArgumentException(null, nameof(properties));
        
        indentation = new Thickness(nesting * Constants.IndentationWidth, 0, 0, 0);
        if (propertyName != null)
        {
            isProperty = true;
            this.propertyName = propertyName;
        }

        if (properties.Any())
        {
            properties[^1].Last = true;
            Properties.AddRange(properties);
        }
        else
        {
            empty = collapsed = true;
        }
    }
    
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

    private bool empty;
    public bool Empty
    {
        get => empty;
        set => this.RaiseAndSetIfChanged(ref empty, value);
    }

    public ObservableCollection<ValueNodeViewModel> Properties { get; set; } = new();
    
    private bool last;
    public bool Last
    {
        get => last;
        set => this.RaiseAndSetIfChanged(ref last, value);
    }
}
