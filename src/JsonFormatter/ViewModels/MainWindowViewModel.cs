using System;
using System.Linq;
using JsonFormatter.ViewModels.UserControls;
using ReactiveUI;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace JsonFormatter.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public void StartFormatting()
    {
        FormatButtonDisabled = true;
        Presenter = new JsonPresenterViewModel();
        InvalidInput = false;
        FormatButtonLabel = "Formatting..";
    }
    
    public void Format()
    {
        if (input == string.Empty)
        {
            EndFormatting();
            return;
        }

        SanitizeInput();

        try
        {
            var result = JsonNode.Parse(input);
            Presenter = new JsonPresenterViewModel(GetVm(result, 0));
            invalidInput = false;
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
        if (input.StartsWith("\"") && input.EndsWith("\""))
        {
            input = input.Substring(1, input.Length - 2);
        }
        else if (input.StartsWith('\'') && input.EndsWith('\''))
        {
            input = input.Substring(1, input.Length - 2);
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
    
    private bool formatButtonDisabled;
    public bool FormatButtonDisabled
    {
        get => formatButtonDisabled;
        set => this.RaiseAndSetIfChanged(ref formatButtonDisabled, value);
    }

    private string formatButtonLabel = "Format";
    public string FormatButtonLabel
    {
        get => formatButtonLabel;
        set => this.RaiseAndSetIfChanged(ref formatButtonLabel, value);
    }
    
    private string input = string.Empty;
    public string Input
    {
        get => input;
        set => this.RaiseAndSetIfChanged(ref input, value);
    }
    
    private bool invalidInput;
    public bool InvalidInput
    {
        get => invalidInput;
        set => this.RaiseAndSetIfChanged(ref invalidInput, value);
    }

    private JsonPresenterViewModel presenter = new();
    public JsonPresenterViewModel Presenter
    {
        get => presenter;
        set => this.RaiseAndSetIfChanged(ref presenter, value);
    }
}