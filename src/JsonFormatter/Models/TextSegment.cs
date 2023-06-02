namespace JsonFormatter.Models;

public class TextSegment
{
    public TextSegment(int startPosition, int length, TextSegmentKind kind)
    {
        StartPosition = startPosition;
        Length = length;
        Kind = kind;
    }
    
    public int StartPosition { get; }
    public int Length { get; }
    public TextSegmentKind Kind { get; }
}
