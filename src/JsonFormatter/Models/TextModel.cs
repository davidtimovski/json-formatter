using System.Collections.Generic;
using System.Globalization;

namespace JsonFormatter.Models;

public class TextModel
{
    private const string NullString = "null";
    private const string TrueString = "true";
    private const string FalseString = "false";

    public TextModel()
    {
        NewLine();
    }
    
    public LinkedList<TextLine> Lines { get; } = new();

    public void NewLine()
    {
        Lines.AddLast(new TextLine(Lines.Count));
    }
    
    public void AddIndent(short nesting)
    {
        if (nesting == 0)
        {
            return;
        }
        
        var text = new string(' ', nesting * 4);
        Lines.Last.Value.Add(text, TextSegmentKind.Whitespace);
    }
    
    public void AddWhitespace(string text)
    {
        Lines.Last.Value.Add(text, TextSegmentKind.Whitespace);
    }
    
    public void AddKeyword(string text)
    {
        Lines.Last.Value.Add(text, TextSegmentKind.Keyword);
    }
    
    public void AddNull()
    {
        Lines.Last.Value.Add(NullString, TextSegmentKind.Null);
    }
    
    public void AddString(string text)
    {
        var textWithQuotes = $"\"{text}\"";
        Lines.Last.Value.Add(textWithQuotes, TextSegmentKind.String);
    }
    
    public void AddNumber(double number)
    {
        var text = number.ToString(CultureInfo.InvariantCulture);
        Lines.Last.Value.Add(text, TextSegmentKind.Number);
    }
    
    public void AddTrue()
    {
        Lines.Last.Value.Add(TrueString, TextSegmentKind.Boolean);
    }
    
    public void AddFalse()
    {
        Lines.Last.Value.Add(FalseString, TextSegmentKind.Boolean);
    }

    public void Reset()
    {
        Lines.Clear();
    }
}
