using System.Globalization;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;

namespace JsonFormatter.ViewModels.UserControls;

public partial class ValueNodeViewModel : ViewModelBase
{
    public ValueNodeViewModel()
    {
    }
    
    public ValueNodeViewModel(short nesting, string? propertyName = null)
    {
        indentation = new Thickness(nesting * Constants.IndentationWidth, 0, 0, 0);
        if (propertyName != null)
        {
            isProperty = true;
            this.propertyName = $"{propertyName}: ";
        }
        isNullValue = true;
        isPrimitive = true;
    }
    
    public ValueNodeViewModel(string value, short nesting, string? propertyName = null)
    {
        indentation = new Thickness(nesting * Constants.IndentationWidth, 0, 0, 0);
        if (propertyName != null)
        {
            isProperty = true;
            this.propertyName = $"{propertyName}: ";
        }
        stringValue = $"\"{value}\"";
        isStringValue = true;
        isPrimitive = true;
    }
    
    public ValueNodeViewModel(double value, short nesting, string? propertyName = null)
    {
        indentation = new Thickness(nesting * Constants.IndentationWidth, 0, 0, 0);
        if (propertyName != null)
        {
            isProperty = true;
            this.propertyName = $"{propertyName}: ";
        }
        numberValue = value.ToString(CultureInfo.InvariantCulture);
        isNumberValue = true;
        isPrimitive = true;
    }
    
    public ValueNodeViewModel(bool value, short nesting, string? propertyName = null)
    {
        indentation = new Thickness(nesting * Constants.IndentationWidth, 0, 0, 0);
        if (propertyName != null)
        {
            isProperty = true;
            this.propertyName = $"{propertyName}: ";
        }
        booleanValue = value ? "true" : "false";
        isBooleanValue = true;
        isPrimitive = true;
    }
    
    public ValueNodeViewModel(ArrayNodeViewModel value)
    {
        value.Last = last;
        arrayValue = value;
        isArrayValue = true;
    }
    
    public ValueNodeViewModel(ObjectNodeViewModel value)
    {
        value.Last = last;
        objectValue = value;
        isObjectValue = true;
    }
    
    [ObservableProperty]
    private Thickness indentation;

    [ObservableProperty]
    private bool isProperty;

    [ObservableProperty]
    private string? propertyName;

    [ObservableProperty]
    private bool isPrimitive;
 
    [ObservableProperty]
    private bool isNullValue;

    [ObservableProperty]
    private bool isStringValue;

    [ObservableProperty]
    private string stringValue;

    [ObservableProperty]
    private bool isNumberValue;

    [ObservableProperty]
    private string numberValue;

    [ObservableProperty]
    private bool isBooleanValue;

    [ObservableProperty]
    private string booleanValue;

    [ObservableProperty]
    private bool isArrayValue;

    [ObservableProperty]
    private ArrayNodeViewModel arrayValue;

    [ObservableProperty]
    private bool isObjectValue;

    [ObservableProperty]
    private ObjectNodeViewModel objectValue;

    [ObservableProperty]
    private bool last;
}
