using SkiaSharp;

namespace Avalonia.Controls.Skia;

/// <summary>
/// 
/// </summary>
public class SKCanvasEventArgs
{
    /// <summary>
    /// 
    /// </summary>
    public SKSurface Surface { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="surface"></param>
    internal SKCanvasEventArgs(SKSurface surface)
    {
        Surface = surface;
    }
}
