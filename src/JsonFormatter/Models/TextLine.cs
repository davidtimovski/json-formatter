using System.Collections.Generic;
using System.Text;

namespace JsonFormatter.Models;

public class TextLine
{
    private readonly StringBuilder _stringBuilder = new();
    private int currentPosition = 0;

    public TextLine(int number)
    {
        Number = number;
    }

    public int Number { get; }
    public List<TextSegment> Segments { get; } = new();

    public void Add(string text, TextSegmentKind kind)
    {
        _stringBuilder.Append(text);
        Segments.Add(new TextSegment(currentPosition, text.Length, kind));
        currentPosition += text.Length;
    }

    public string GetText()
    {
        return _stringBuilder.ToString();
    }
}
