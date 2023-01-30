using CommunityToolkit.Mvvm.ComponentModel;

namespace JsonFormatter.ViewModels.UserControls;

public partial class JsonPresenterViewModel : ViewModelBase
{
    public JsonPresenterViewModel()
    {
        empty = true;
    }
    
    public JsonPresenterViewModel(ValueNodeViewModel json)
    {
        json.Last = true;
        this.json = json;
    }
    
    [ObservableProperty]
    private bool empty;

    [ObservableProperty]
    private ValueNodeViewModel json = new();
}
