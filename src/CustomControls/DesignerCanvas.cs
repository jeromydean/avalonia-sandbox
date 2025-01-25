using Avalonia;
using Avalonia.Controls;

namespace CustomControls
{
  public class DesignerCanvas : Canvas
  {
    private const double MinScale = .25d;
    private const double MaxScale = 10d;
    private const double ScalingDeltaStep = .25d;

    public static readonly StyledProperty<double> ScaleXProperty =
      AvaloniaProperty.Register<DesignerCanvas, double>(name: nameof(ScaleX), defaultValue: 1d);

    public double ScaleX
    {
      get => GetValue(ScaleXProperty);
      set => SetValue(ScaleXProperty, value);
    }

    public static readonly StyledProperty<double> ScaleYProperty =
      AvaloniaProperty.Register<DesignerCanvas, double>(name: nameof(ScaleY), defaultValue: 1d);

    public double ScaleY
    {
      get => GetValue(ScaleYProperty);
      set => SetValue(ScaleYProperty, value);
    }

    static DesignerCanvas()
    {
      AffectsRender<DesignerCanvas>(ScaleXProperty,
        ScaleYProperty);
    }

    public DesignerCanvas()
    {
      PointerWheelChanged += DesignerCanvas_PointerWheelChanged;
    }

    private void DesignerCanvas_PointerWheelChanged(object? sender, Avalonia.Input.PointerWheelEventArgs e)
    {
      if (e.KeyModifiers == Avalonia.Input.KeyModifiers.Control)
      {
        ///TODO can we zoom a little close to the current position or to the center of the control?
        //Point position = e.GetPosition(this);

        double scale = ScaleX;
        double delta = e.Delta.Y * ScalingDeltaStep;

        scale += delta;

        scale = Math.Max(scale, MinScale);
        scale = Math.Min(scale, MaxScale);

        if (ScaleX != scale)
        {
          ScaleX = ScaleY = scale;
        }

        e.Handled = true;
      }
    }
  }
}
