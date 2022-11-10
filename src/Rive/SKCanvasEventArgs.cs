using SkiaSharp;

namespace Rive;

/// <summary>
/// 
/// </summary>
internal class SKCanvasEventArgs
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
