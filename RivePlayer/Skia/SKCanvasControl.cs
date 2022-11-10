using System;
using Avalonia.Media;
using SkiaSharp;

namespace Avalonia.Controls.Skia;

/// <summary>
/// SKCanvas control.
/// </summary>
public class SKCanvasControl : Control
{
    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<SKCanvasEventArgs>? Draw;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public override void Render(DrawingContext context)
    {
        var viewPort = new Rect(Bounds.Size);
        using var clip = ClipToBounds ? context.PushClip(viewPort) : default;
        context.Custom(
            new SKCanvasDrawOperation(
                new Rect(0, 0, viewPort.Width, viewPort.Height),
                RaiseOnDraw));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="surface"></param>
    private void RaiseOnDraw(SKSurface surface)
    {
        var e = new SKCanvasEventArgs(surface);
        OnDraw(e);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    protected virtual void OnDraw(SKCanvasEventArgs e)
    {
        Draw?.Invoke(this, e);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ClipToBoundsProperty)
        {
            InvalidateVisual();
        }
    }
}
