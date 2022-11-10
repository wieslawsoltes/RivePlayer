using System;
using Avalonia;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using SkiaSharp;

namespace Rive;

/// <summary>
/// 
/// </summary>
internal class SKCanvasDrawOperation : ICustomDrawOperation
{
    private readonly Rect _bounds;
    private readonly Action<SKSurface> _invalidate;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bounds"></param>
    /// <param name="invalidate"></param>
    public SKCanvasDrawOperation(Rect bounds, Action<SKSurface> invalidate)
    {
        _bounds = bounds;
        _invalidate = invalidate;
    }

    /// <summary>
    /// 
    /// </summary>
    public void Dispose()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public Rect Bounds => _bounds;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    public bool HitTest(Point p) => _bounds.Contains(p);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(ICustomDrawOperation? other) => false;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public void Render(IDrawingContextImpl context)
    {
        var leaseFeature = context.GetFeature<ISkiaSharpApiLeaseFeature>();
        if (leaseFeature is null)
        {
            return;
        }
        using var lease = leaseFeature.Lease();
        var surface = lease?.SkSurface;
        
        if (surface is { })
        {
            _invalidate(surface);
        }
    }
}
