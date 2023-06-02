using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using CommunityToolkit.Mvvm.ComponentModel;
using JsonFormatter.Models;
using JsonFormatter.ViewModels.UserControls;

namespace JsonFormatter.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private int jsonNodeCount;
    private readonly TextModel _viewerModel = new();
    
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
            //var vm = ConstructViewModel(result, 0);
            
            // if (jsonNodeCount > Constants.MaxNodeCount)
            // {
            //     EndFormatting();
            //     ErrorMessage = $"For performance/memory reasons I can't render JSON with over {Constants.MaxNodeCount:n0} nodes. Sorry!";
            //     return false;
            // }
            //
            //Json = vm;
            
            ConstructViewModel2(result, 0);

            Output = new ViewerViewModel(_viewerModel.Lines.ToList());
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

    private void ConstructViewModel2(JsonNode? value, short nesting, string? propertyName = null)
    {
        jsonNodeCount++;
        
        if (value == null)
        {
            _viewerModel.AddIndent(nesting);
            _viewerModel.AddKeyword($"{propertyName}: ");
            _viewerModel.AddNull();
            return;
        }
        
        _viewerModel.AddIndent(nesting);

        if (value is JsonArray jArray)
        {
            ConstructViewModelFromArray2(jArray, nesting, propertyName);
            return;
        }

        if (value is JsonObject jObject)
        {
            ConstructViewModelFromObject2(jObject, nesting, propertyName);
            return;
        }
        
        if (propertyName != null)
        {
            _viewerModel.AddKeyword($"{propertyName}: ");
        }

        var jsonElement = value.GetValue<JsonElement>();
        switch (jsonElement.ValueKind)
        {
            case JsonValueKind.Null:
                _viewerModel.AddNull();
                break;
            case JsonValueKind.String:
                _viewerModel.AddString(value.ToString());
                break;
            case JsonValueKind.Number:
                _viewerModel.AddNumber(double.Parse(value.ToString()));
                break;
            case JsonValueKind.True:
                _viewerModel.AddTrue();
                break;
            case JsonValueKind.False:
                _viewerModel.AddFalse();
                break;
            default:
                throw new ArgumentException("Node value type not recognized");
        }
    }

    private void ConstructViewModelFromArray2(JsonArray array, short nesting, string? propertyName = null)
    {
        if (propertyName != null)
        {
            _viewerModel.AddKeyword($"{propertyName}: ");
        }
        
        _viewerModel.AddKeyword("[");
        _viewerModel.NewLine();

        var propNesting = (short)(nesting + 1);
        for (var i = 0; i < array.Count; i++)
        {
            ConstructViewModel2(array[i], propNesting);

            if (i < array.Count - 1)
            {
                _viewerModel.AddKeyword(",");
            }

            _viewerModel.NewLine();
        }
        
        _viewerModel.AddIndent(nesting);
        _viewerModel.AddKeyword("]");
    }
    
    private void ConstructViewModelFromObject2(JsonObject jObject, short nesting, string? propertyName = null)
    {
        if (propertyName != null)
        {
            _viewerModel.AddKeyword($"{propertyName}: ");
        }
        
        _viewerModel.AddKeyword("{");
        _viewerModel.NewLine();
        
        var propNesting = (short)(nesting + 1);
        using var enumerator = jObject.GetEnumerator();

        var notLast = enumerator.MoveNext();
        while (notLast)
        {
            ConstructViewModel2(enumerator.Current.Value, propNesting, enumerator.Current.Key);

            notLast = enumerator.MoveNext();        
            if (notLast)
            {
                _viewerModel.AddKeyword(",");
            }
            
            _viewerModel.NewLine();
        }

        _viewerModel.AddIndent(nesting);
        _viewerModel.AddKeyword("}");
    }
    
    [ObservableProperty]
    private bool formatButtonDisabled;

    [ObservableProperty]
    private string formatButtonLabel;

    [ObservableProperty]
    private string input;
    
    [ObservableProperty]
    private ViewerViewModel output;

    [ObservableProperty]
    private bool invalidInput;
    
    [ObservableProperty]
    private string? errorMessage;
    
    [ObservableProperty]
    private bool empty;

    [ObservableProperty]
    private ValueNodeViewModel json = new();
}
