using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using CommunityToolkit.Mvvm.ComponentModel;
using JsonFormatter.ViewModels.UserControls;

namespace JsonFormatter.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private int jsonNodeCount;

    public MainWindowViewModel()
    {
        jsonInput = string.Empty;
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
        if (JsonInput == string.Empty)
        {
            Json = new ValueNodeViewModel();
            EndFormatting();
            return true;
        }

        if (JsonInput.Length > Constants.MaxInputLength)
        {
            EndFormatting();
            ErrorMessage =
                $"For performance/memory reasons I can't render JSON over {Constants.MaxInputLength:n0} characters. Sorry!";
            return false;
        }

        SanitizeInput();

        var valid = true;
        try
        {
            var result = JsonNode.Parse(JsonInput);
            var vm = ConstructViewModel(result, 0);
            vm.EndsWithComma = false;

            if (jsonNodeCount > Constants.MaxNodeCount)
            {
                EndFormatting();
                ErrorMessage =
                    $"For performance/memory reasons I can't render JSON with over {Constants.MaxNodeCount:n0} nodes. Sorry!";
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
        if (JsonInput.StartsWith("\"") && JsonInput.EndsWith("\""))
        {
            JsonInput = JsonInput.Substring(1, JsonInput.Length - 2);
        }
        else if (JsonInput.StartsWith('\'') && JsonInput.EndsWith('\''))
        {
            JsonInput = JsonInput.Substring(1, JsonInput.Length - 2);
        }
    }

    private ValueNodeViewModel ConstructViewModel(JsonNode? value, short nesting, string? propertyName = null)
    {
        jsonNodeCount++;

        if (value is null)
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
        return jsonElement.ValueKind switch
        {
            JsonValueKind.Null => new ValueNodeViewModel(nesting, propertyName),
            JsonValueKind.String => new ValueNodeViewModel(value.ToString(), nesting, propertyName),
            JsonValueKind.Number => new ValueNodeViewModel(double.Parse(value.ToString()), nesting, propertyName),
            JsonValueKind.True => new ValueNodeViewModel(true, nesting, propertyName),
            JsonValueKind.False => new ValueNodeViewModel(false, nesting, propertyName),
            _ => throw new ArgumentException("Node value type not recognized")
        };
    }

    private ValueNodeViewModel ConstructViewModelFromArray(JsonArray array, short nesting, string? propertyName = null)
    {
        var itemNesting = (short)(nesting + 1);
        var items = array.Select(x => ConstructViewModel(x, itemNesting)).ToList();
        return new ValueNodeViewModel(new ArrayNodeViewModel(items, nesting, propertyName), nesting);
    }

    private ValueNodeViewModel ConstructViewModelFromObject(JsonObject jObject, short nesting,
        string? propertyName = null)
    {
        var propNesting = (short)(nesting + 1);
        var properties = jObject.Select(property => ConstructViewModel(property.Value, propNesting, property.Key))
            .ToList();
        return new ValueNodeViewModel(new ObjectNodeViewModel(properties, nesting, propertyName), nesting);
    }

    [ObservableProperty]
    private bool formatButtonDisabled;

    [ObservableProperty]
    private string formatButtonLabel;

    [ObservableProperty]
    private string jsonInput;

    [ObservableProperty]
    private bool invalidInput;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private bool empty;

    [ObservableProperty]
    private ValueNodeViewModel json = new();
}