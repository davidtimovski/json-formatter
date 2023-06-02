using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using JsonFormatter.Models;

namespace JsonFormatter.UserControls;

public class LineControl : Control
{
    private readonly Dictionary<TextSegmentKind, ISolidColorBrush> _colors = new()
    {
        { TextSegmentKind.Whitespace, new SolidColorBrush(Color.FromRgb(102, 195, 204)) },
        { TextSegmentKind.Keyword, new SolidColorBrush(Color.FromRgb(102, 195, 204)) },
        { TextSegmentKind.Null, new SolidColorBrush(Color.FromRgb(188, 188, 43)) },
        { TextSegmentKind.String, new SolidColorBrush(Color.FromRgb(200, 161, 98)) },
        { TextSegmentKind.Number, new SolidColorBrush(Color.FromRgb(237, 148, 192)) },
        { TextSegmentKind.Boolean, new SolidColorBrush(Color.FromRgb(108, 149, 235)) },
    };

    static LineControl()
    {
        AffectsRender<LineControl>(LineProperty);
    }

    public static readonly StyledProperty<TextLine> LineProperty =
        AvaloniaProperty.Register<LineControl, TextLine>(nameof(TextLine));

    public TextLine Line
    {
        get => GetValue(LineProperty);
        set => SetValue(LineProperty, value);
    }

    public override void Render(DrawingContext drawingContext)
    {
        if (Line == null)
        {
            return;
        }
        
        var formattedText = new FormattedText(Line.GetText(), CultureInfo.GetCultureInfo("en-us"),
            FlowDirection.LeftToRight,
            new Typeface(FontFamily.Parse("Maven Pro", new Uri("resm:JsonFormatter.Assets.Fonts?assembly=JsonFormatter#Maven Pro"))),
            15,
            new SolidColorBrush(Color.FromRgb(102, 195, 204)));
        
        foreach (var segment in Line.Segments)
        {
            formattedText.SetForegroundBrush(_colors[segment.Kind], segment.StartPosition, segment.Length);
        }

        drawingContext.DrawText(formattedText, new Point(0, Line.Number * Constants.LineHeight));
    }
}
