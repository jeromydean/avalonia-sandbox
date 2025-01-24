using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace CustomControls
{
  public class Designer : TemplatedControl
  {
    public static readonly StyledProperty<IBrush?> HorizontalRulerBackgroundProperty =
      AvaloniaProperty.Register<Designer, IBrush?>(name: nameof(HorizontalRulerBackground));

    public IBrush? HorizontalRulerBackground
    {
      get => GetValue(HorizontalRulerBackgroundProperty);
      set => SetValue(HorizontalRulerBackgroundProperty, value);
    }

    public static readonly StyledProperty<IBrush?> VerticalRulerBackgroundProperty =
      AvaloniaProperty.Register<Designer, IBrush?>(name: nameof(VerticalRulerBackground));

    public IBrush? VerticalRulerBackground
    {
      get => GetValue(VerticalRulerBackgroundProperty);
      set => SetValue(VerticalRulerBackgroundProperty, value);
    }
    static Designer()
    {
      AffectsRender<Designer>(HorizontalRulerBackgroundProperty,
        VerticalRulerBackgroundProperty);
    }

    public override void Render(DrawingContext context)
    {
      base.Render(context);
    }
  }
}