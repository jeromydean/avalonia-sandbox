using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace CustomControls
{
  public class Ruler : Control
  {
    private const int DipsPerInch = 96;
    private const double FontSize = 12d;

    public static readonly StyledProperty<Orientation> OrientationProperty =
      AvaloniaProperty.Register<Ruler, Orientation>(name: nameof(Orientation), defaultValue: Orientation.Horizontal);

    public Orientation Orientation
    {
      get => GetValue(OrientationProperty);
      set => SetValue(OrientationProperty, value);
    }

    public static readonly StyledProperty<IBrush?> BackgroundProperty =
      AvaloniaProperty.Register<Ruler, IBrush?>(name: nameof(Background));

    public IBrush? Background
    {
      get => GetValue(BackgroundProperty);
      set => SetValue(BackgroundProperty, value);
    }

    static Ruler()
    {
      AffectsRender<Ruler>(OrientationProperty,
        BackgroundProperty);
    }

    public override void Render(DrawingContext context)
    {
      Rect controlArea = new Rect(Bounds.Size);
      if (Background != null)
      {
        context.FillRectangle(Background.ToImmutable(), controlArea);
      }

      Typeface rulerTypeface = new Typeface("Arial",
        FontStyle.Normal,
        FontWeight.UltraBold,
        FontStretch.Normal);

      Pen tickPen = new Pen(SolidColorBrush.Parse("#ffffffff"),
        thickness: 1d,
        lineCap: PenLineCap.Round);

      bool isHorizontal = Orientation == Orientation.Horizontal;

      double fullTickHeight = isHorizontal ? controlArea.Height : controlArea.Width;
      double halfTickHeight = fullTickHeight * .8d;
      double quarterTickHeight = fullTickHeight * .6d;
      double sixteenthTickHeight = fullTickHeight * .2d;

      for (int tickPos = 0; tickPos <= (isHorizontal ? controlArea.Width : controlArea.Height); tickPos += 6)
      {
        double tickHeight = sixteenthTickHeight;
        if (tickPos % DipsPerInch == 0)
        {
          context.DrawText(new FormattedText((tickPos / DipsPerInch).ToString(),
            System.Globalization.CultureInfo.CurrentCulture,
            FlowDirection.LeftToRight,
            rulerTypeface,
            FontSize,
            SolidColorBrush.Parse("#ffffffff")),
            (isHorizontal ? new Point(tickPos, 0d) : new Point(0d, tickPos)));

          tickHeight = fullTickHeight;
        }
        else if (tickPos % (DipsPerInch / 2) == 0)
        {
          tickHeight = halfTickHeight;
        }
        else if (tickPos % (DipsPerInch / 4) == 0)
        {
          tickHeight = quarterTickHeight;
        }

        if (isHorizontal)
        {
          context.DrawLine(tickPen,
            new Point(tickPos, controlArea.Height),
            new Point(tickPos, (controlArea.Height - tickHeight)));
        }
        else
        {
          context.DrawLine(tickPen,
            new Point(controlArea.Width, tickPos),
            new Point((controlArea.Width - tickHeight), tickPos));
        }        
      }
    }
  }
}