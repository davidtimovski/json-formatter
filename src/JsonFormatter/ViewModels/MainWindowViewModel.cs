using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using CommunityToolkit.Mvvm.ComponentModel;
using JsonFormatter.ViewModels.UserControls;

namespace JsonFormatter.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        input = string.Empty;
        formatButtonLabel = "Format";
    }
    
    public void StartFormatting()
    {
        FormatButtonDisabled = true;
        Presenter = new JsonPresenterViewModel();
        InvalidInput = false;
        FormatButtonLabel = "Formatting..";
    }
    
    public void Format()
    {
        if (Input == string.Empty)
        {
            EndFormatting();
            return;
        }

        SanitizeInput();

        try
        {
            var result = JsonNode.Parse(Input);
            Presenter = new JsonPresenterViewModel(GetVm(result, 0));
            InvalidInput = false;
        }
        catch
        {
            InvalidInput = true;
        }

        EndFormatting();
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

    private ValueNodeViewModel GetVm(JsonNode? value, short nesting, string? propertyName = null)
    {
        if (value == null)
        {
            return new ValueNodeViewModel(nesting, propertyName);
        }

        if (value is JsonArray jArray)
        {
            return GetVmFromArray(jArray, nesting, propertyName);
        }

        if (value is JsonObject jObject)
        {
            return GetVmFromObject(jObject, nesting, propertyName);
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

    private ValueNodeViewModel GetVmFromArray(JsonArray array, short nesting, string? propertyName = null)
    {
        var itemNesting = (short)(nesting + 1);
        var items = array.Select(x => GetVm(x, itemNesting)).ToList();
        return new ValueNodeViewModel(new ArrayNodeViewModel(items, nesting, propertyName));
    }

    private ValueNodeViewModel GetVmFromObject(JsonObject jObject, short nesting, string? propertyName = null)
    {
        var propNesting = (short)(nesting + 1);
        var properties = jObject.Select(property => GetVm(property.Value, propNesting, property.Key)).ToList();
        return new ValueNodeViewModel(new ObjectNodeViewModel(properties, nesting, propertyName));
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
    private JsonPresenterViewModel presenter = new();
}