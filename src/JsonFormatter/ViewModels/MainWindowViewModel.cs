using System;
using System.Linq;
using JsonFormatter.ViewModels.UserControls;
using ReactiveUI;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace JsonFormatter.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        var input = """{"empty":[],"emptyObj":{},"nullField":null,"id":"0001","type":"donut","name":"Cake","ppu":0.55,"batters":{"batter":[{"id":"1001","type":"Regular"},{"id":"1002","type":"Chocolate"},{"id":"1003","type":"Blueberry"},{"id":"1004","type":"Devil's Food"}]},"topping":[{"id":"5001","type":"None"},{"id":"5002","type":"Glazed"},{"id":"5005","type":"Sugar"},{"id":"5007","type":"Powdered Sugar"},{"id":"5006","type":"Chocolate with Sprinkles"},{"id":"5003","type":"Chocolate"},{"id":"5004","type":"Maple"}]}""";

        if (input == null)
        {
            root = new ValueNodeViewModel(0);
            return;
        }

        var result = JsonNode.Parse(input);
        root = GetVm(result, 0);
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

    private ValueNodeViewModel root;

    public ValueNodeViewModel Root
    {
        get => root;
        set => this.RaiseAndSetIfChanged(ref value, root);
    }
}
