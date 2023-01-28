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
    
    private bool isPrimitive;
    public bool IsPrimitive
    {
        get => isPrimitive;
        set => this.RaiseAndSetIfChanged(ref value, isPrimitive);
    }
    
    private bool isNullValue;
    public bool IsNullValue
    {
        get => isNullValue;
        set => this.RaiseAndSetIfChanged(ref value, isNullValue);
    }
    
    private bool isStringValue;
    public bool IsStringValue
    {
        get => isStringValue;
        set => this.RaiseAndSetIfChanged(ref value, isStringValue);
    }
    
    private string stringValue;
    public string StringValue
    {
        get => stringValue;
        set => this.RaiseAndSetIfChanged(ref value, stringValue);
    }
    
    private bool isNumberValue;
    public bool IsNumberValue
    {
        get => isNumberValue;
        set => this.RaiseAndSetIfChanged(ref value, isNumberValue);
    }
    
    private string numberValue;
    public string NumberValue
    {
        get => numberValue;
        set => this.RaiseAndSetIfChanged(ref value, numberValue);
    }
    
    private bool isBooleanValue;
    public bool IsBooleanValue
    {
        get => isBooleanValue;
        set => this.RaiseAndSetIfChanged(ref value, isBooleanValue);
    }
    
    private string booleanValue;
    public string BooleanValue
    {
        get => booleanValue;
        set => this.RaiseAndSetIfChanged(ref value, booleanValue);
    }
    
    private bool isArrayValue;
    public bool IsArrayValue
    {
        get => isArrayValue;
        set => this.RaiseAndSetIfChanged(ref value, isArrayValue);
    }

    private ArrayNodeViewModel arrayValue = new();
    public ArrayNodeViewModel ArrayValue
    {
        get => arrayValue;
        set => this.RaiseAndSetIfChanged(ref value, arrayValue);
    }
    
    private bool isObjectValue;
    public bool IsObjectValue
    {
        get => isObjectValue;
        set => this.RaiseAndSetIfChanged(ref value, isObjectValue);
    }

    private ObjectNodeViewModel objectValue = new();
    public ObjectNodeViewModel ObjectValue
    {
        get => objectValue;
        set => this.RaiseAndSetIfChanged(ref value, objectValue);
    }
}
