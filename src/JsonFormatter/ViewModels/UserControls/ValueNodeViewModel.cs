using ReactiveUI;

namespace JsonFormatter.ViewModels.UserControls;

public class ValueNodeViewModel : ViewModelBase
{
    public ValueNodeViewModel()
    {
    }
    
    public ValueNodeViewModel(short nesting, string? propertyName = null)
    {
        indentation = new string(' ', nesting * Constants.IndentationSpaces);
        if (propertyName != null)
        {
            isProperty = true;
            this.propertyName = propertyName;
        }
        isNullValue = true;
        isPrimitive = true;
    }
    
    public ValueNodeViewModel(string value, short nesting, string? propertyName = null)
    {
        indentation = new string(' ', nesting * Constants.IndentationSpaces);
        if (propertyName != null)
        {
            isProperty = true;
            this.propertyName = propertyName;
        }
        stringValue = $"\"{value}\"";
        isStringValue = true;
        isPrimitive = true;
    }
    
    public ValueNodeViewModel(double value, short nesting, string? propertyName = null)
    {
        indentation = new string(' ', nesting * Constants.IndentationSpaces);
        if (propertyName != null)
        {
            isProperty = true;
            this.propertyName = propertyName;
        }
        numberValue = value.ToString();
        isNumberValue = true;
        isPrimitive = true;
    }
    
    public ValueNodeViewModel(bool value, short nesting, string? propertyName = null)
    {
        indentation = new string(' ', nesting * Constants.IndentationSpaces);
        if (propertyName != null)
        {
            isProperty = true;
            this.propertyName = propertyName;
        }
        booleanValue = value ? "true" : "false";
        isBooleanValue = true;
        isPrimitive = true;
    }
    
    public ValueNodeViewModel(ArrayNodeViewModel value)
    {
        arrayValue = value;
        isArrayValue = true;
    }
    
    public ValueNodeViewModel(ObjectNodeViewModel value)
    {
        objectValue = value;
        isObjectValue = true;
    }
    
    private string indentation = string.Empty;
    public string Indentation
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
    
    private bool isPrimitive;
    public bool IsPrimitive
    {
        get => isPrimitive;
        set => this.RaiseAndSetIfChanged(ref isPrimitive, value);
    }
    
    private bool isNullValue;
    public bool IsNullValue
    {
        get => isNullValue;
        set => this.RaiseAndSetIfChanged(ref isNullValue, value);
    }
    
    private bool isStringValue;
    public bool IsStringValue
    {
        get => isStringValue;
        set => this.RaiseAndSetIfChanged(ref isStringValue, value);
    }
    
    private string stringValue;
    public string StringValue
    {
        get => stringValue;
        set => this.RaiseAndSetIfChanged(ref stringValue, value);
    }
    
    private bool isNumberValue;
    public bool IsNumberValue
    {
        get => isNumberValue;
        set => this.RaiseAndSetIfChanged(ref isNumberValue, value);
    }
    
    private string numberValue;
    public string NumberValue
    {
        get => numberValue;
        set => this.RaiseAndSetIfChanged(ref numberValue, value);
    }
    
    private bool isBooleanValue;
    public bool IsBooleanValue
    {
        get => isBooleanValue;
        set => this.RaiseAndSetIfChanged(ref isBooleanValue, value);
    }
    
    private string booleanValue;
    public string BooleanValue
    {
        get => booleanValue;
        set => this.RaiseAndSetIfChanged(ref booleanValue, value);
    }
    
    private bool isArrayValue;
    public bool IsArrayValue
    {
        get => isArrayValue;
        set => this.RaiseAndSetIfChanged(ref isArrayValue, value);
    }

    private ArrayNodeViewModel arrayValue = new();
    public ArrayNodeViewModel ArrayValue
    {
        get => arrayValue;
        set => this.RaiseAndSetIfChanged(ref arrayValue, value);
    }
    
    private bool isObjectValue;
    public bool IsObjectValue
    {
        get => isObjectValue;
        set => this.RaiseAndSetIfChanged(ref isObjectValue, value);
    }

    private ObjectNodeViewModel objectValue = new();
    public ObjectNodeViewModel ObjectValue
    {
        get => objectValue;
        set => this.RaiseAndSetIfChanged(ref objectValue, value);
    }
}
