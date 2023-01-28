using ReactiveUI;

namespace JsonFormatter.ViewModels.UserControls;

public class JsonPresenterViewModel : ViewModelBase
{
    public JsonPresenterViewModel()
    {
        empty = true;
    }
    
    public JsonPresenterViewModel(ValueNodeViewModel json)
    {
        this.json = json;
    }
    
    private bool empty;
    public bool Empty
    {
        get => empty;
        set => this.RaiseAndSetIfChanged(ref empty, value);
    }
    
    private ValueNodeViewModel json = new();
    public ValueNodeViewModel Json
    {
        get => json;
        set => this.RaiseAndSetIfChanged(ref json, value);
    }
}
