using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using CommunityToolkit.Mvvm.ComponentModel;
using JsonFormatter.ViewModels.UserControls;

namespace JsonFormatter.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private int jsonNodeCount;
    
    public MainWindowViewModel()
    {
        input = string.Empty;
        formatButtonLabel = "Format";
    }

    public void StartFormatting()
    {
        jsonNodeCount = 0;
        FormatButtonDisabled = true;
        InvalidInput = false;
        FormatButtonLabel = "Formatting..";
    }
    
    public bool Format()
    {
        if (Input == string.Empty)
        {
            Json = new ValueNodeViewModel();
            EndFormatting();
            return true;
        }

        if (Input.Length > Constants.MaxInputLength)
        {
            EndFormatting();
            ErrorMessage = $"For performance/memory reasons I can't render JSON over {Constants.MaxInputLength:n0} characters. Sorry!";
            return false;
        }

        SanitizeInput();

        var valid = true;
        try
        {
            var result = JsonNode.Parse(Input);
            var vm = ConstructViewModel(result, 0);
            
            if (jsonNodeCount > Constants.MaxNodeCount)
            {
                EndFormatting();
                ErrorMessage = $"For performance/memory reasons I can't render JSON with over {Constants.MaxNodeCount:n0} nodes. Sorry!";
                return false;
            }
            
            Json = vm;
            InvalidInput = false;
            ErrorMessage = null;
        }
        catch
        {
            InvalidInput = true;
            ErrorMessage = "Invalid JSON";
            valid = false;
        }

        EndFormatting();
        return valid;
    }

    private void EndFormatting()
    {
        FormatButtonLabel = "Format";
        FormatButtonDisabled = false;
    }
    
    private void SanitizeInput()
    {
        if (Input.StartsWith("\"") && Input.EndsWith("\""))
        {
            Input = Input.Substring(1, Input.Length - 2);
        }
        else if (Input.StartsWith('\'') && Input.EndsWith('\''))
        {
            Input = Input.Substring(1, Input.Length - 2);
        }
    }

    private ValueNodeViewModel ConstructViewModel(JsonNode? value, short nesting, string? propertyName = null)
    {
        jsonNodeCount++;
        
        if (value == null)
        {
            return new ValueNodeViewModel(nesting, propertyName);
        }

        if (value is JsonArray jArray)
        {
            return ConstructViewModelFromArray(jArray, nesting, propertyName);
        }

        if (value is JsonObject jObject)
        {
            return ConstructViewModelFromObject(jObject, nesting, propertyName);
        }

        var jsonElement = value.GetValue<JsonElement>();
        switch (jsonElement.ValueKind)
        {
            case JsonValueKind.Null:
                return new ValueNodeViewModel(nesting, propertyName);
            case JsonValueKind.String:
                return new ValueNodeViewModel(value.ToString(), nesting, propertyName);
            case JsonValueKind.Number:
                return new ValueNodeViewModel(double.Parse(value.ToString()), nesting, propertyName);
            case JsonValueKind.True:
                return new ValueNodeViewModel(true, nesting, propertyName);
            case JsonValueKind.False:
                return new ValueNodeViewModel(false, nesting, propertyName);
            default:
                throw new ArgumentException("Node value type not recognized");
        }
    }

    private ValueNodeViewModel ConstructViewModelFromArray(JsonArray array, short nesting, string? propertyName = null)
    {
        var itemNesting = (short)(nesting + 1);
        var items = array.Select(x => ConstructViewModel(x, itemNesting)).ToList();
        return new ValueNodeViewModel(new ArrayNodeViewModel(items, nesting, propertyName), nesting);
    }

    private ValueNodeViewModel ConstructViewModelFromObject(JsonObject jObject, short nesting, string? propertyName = null)
    {
        var propNesting = (short)(nesting + 1);
        var properties = jObject.Select(property => ConstructViewModel(property.Value, propNesting, property.Key)).ToList();
        return new ValueNodeViewModel(new ObjectNodeViewModel(properties, nesting, propertyName), nesting);
    }
    
    [ObservableProperty]
    private bool formatButtonDisabled;

    [ObservableProperty]
    private string formatButtonLabel;

    [ObservableProperty]
    private string input;

    [ObservableProperty]
    private bool invalidInput;
    
    [ObservableProperty]
    private string? errorMessage;
    
    [ObservableProperty]
    private bool empty;

    [ObservableProperty]
    private ValueNodeViewModel json = new();
}
