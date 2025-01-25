using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using Avalonia.Threading;
using SkiaSharp;

namespace CustomControls
{
  public class PointerPathsCanvas : Control
  {
    private class PointerPathCustomDrawOperation : ICustomDrawOperation
    {
      private IEnumerable<SKPath> _paths;

      public Rect Bounds { get; }

      public PointerPathCustomDrawOperation(Rect bounds,
        IEnumerable<SKPath> paths)
      {
        Bounds = bounds;

        //get our own copy of it so nobody can modify out from under us
        _paths = paths.ToList();
      }

      public bool HitTest(Point p)
      {
        if (Bounds.Contains(p))
          return true;

        return false;
      }
      public bool Equals(ICustomDrawOperation other) => false;

      public void Dispose()
      {
      }

      public void Render(ImmediateDrawingContext context)
      {
        ISkiaSharpApiLeaseFeature leaseFeature = context.TryGetFeature<ISkiaSharpApiLeaseFeature>();
        if (leaseFeature != null)
        {
          using (ISkiaSharpApiLease skiaLease = leaseFeature.Lease())
          {
            SKCanvas canvas = skiaLease.SkCanvas;
            {
              SKPaint pathStrokePaint = new SKPaint
              {
                Style = SKPaintStyle.Stroke,
                StrokeCap = SKStrokeCap.Round,
                Color = SKColors.Black,
                StrokeWidth = 5f
              };

              SKPaint circleFillPaint = new SKPaint
              {
                Style = SKPaintStyle.Fill,
                Color = SKColors.Black,
                StrokeWidth = 5f
              };

              
              foreach (SKPath path in _paths)
              {
                if (path != null)
                {
                  //verify we have a path and not a single point
                  if (path.PointCount > 1)
                  {
                    canvas.DrawPath(path, pathStrokePaint);
                  }
                  else
                  {
                    canvas.DrawCircle(path.Points.First(),
                      5f,
                      circleFillPaint);
                  }
                }
              }
            }
          }
        }
        else
        {
          throw (new NotImplementedException());
        }
      }
    }

    private List<SKPath> _paths = new List<SKPath>();
    private SKPath? _currentPath = null;
    private bool _isCapturing = false;

    public override void EndInit()
    {
      base.EndInit();

      PointerPressed += PointerPathCanvas_PointerPressed;
      PointerMoved += PointerPathCanvas_PointerMoved;
      PointerReleased += PointerPathCanvas_PointerReleased;
      PointerExited += PointerPathCanvas_PointerExited;
    }

    public override void Render(DrawingContext context)
    {
      context.Custom(new PointerPathCustomDrawOperation(new Rect(Bounds.Size), _paths));
      Dispatcher.UIThread.InvokeAsync(InvalidateVisual, DispatcherPriority.Background);
    }

    private void PointerPathCanvas_PointerReleased(object? sender, Avalonia.Input.PointerReleasedEventArgs e)
    {
      if (_isCapturing)
      {
        InvalidateVisual();
      }

      _isCapturing = false;
      e.Handled = true;
    }

    private void PointerPathCanvas_PointerExited(object? sender, Avalonia.Input.PointerEventArgs e)
    {
      if (_isCapturing)
      {
        InvalidateVisual();
      }

      _isCapturing = false;
      e.Handled = true;
    }

    private void PointerPathCanvas_PointerMoved(object? sender, Avalonia.Input.PointerEventArgs e)
    {
      if (_isCapturing)
      {
        _currentPath?.LineTo(e.GetPosition(this).ToSKPoint());
        InvalidateVisual();
      }
      e.Handled = true;
    }

    private void PointerPathCanvas_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
      _isCapturing = true;

      _currentPath = new SKPath();
      _currentPath.MoveTo(e.GetPosition(this).ToSKPoint());
      _paths.Add(_currentPath);

      e.Handled = true;
    }
  }
}