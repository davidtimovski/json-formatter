using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using JsonFormatter.Models;

namespace JsonFormatter.ViewModels.UserControls;

public partial class ViewerViewModel : ViewModelBase
{
    public ViewerViewModel(List<TextLine> lines)
    {
        _ = lines ?? throw new ArgumentException(null, nameof(lines));

        foreach (var line in lines)
        {
            Lines.Add(line);
        }
    }

    [ObservableProperty]
    private bool empty;
    
    public ObservableCollection<TextLine> Lines { get; set; } = new();
}
