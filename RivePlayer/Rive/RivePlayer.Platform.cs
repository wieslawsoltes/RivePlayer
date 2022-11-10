// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


using System;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Skia;
using Avalonia.Interactivity;


namespace CommunityToolkit.Labs.WinUI.Rive;

// This file contains platform-specific customizations of RivePlayer.
[TemplatePart(Name = SkiaSurfacePartName, Type = typeof(ContentPresenter))]
public partial class RivePlayer : TemplatedControl
{
    private const string SkiaSurfacePartName = "SkiaSurface";
    ContentPresenter? _skiaSurface;
    private bool _drawInBackground;

    /// <summary>
    /// Controls whether the RivePlayer should run its rendering and animation logic in a background
    /// thread. This cannot be modified after the control has finished loading.
    /// </summary>
    public bool DrawInBackground
    {
        get => _drawInBackground;
        set
        {
            if (_skiaSurface != null)
            {
                throw new InvalidOperationException(
                    "RivePlayer.DrawInBackground cannot be modified after the control has finished loading.");
            }
            _drawInBackground = value;
        }
    }

    /// <inheritdoc/>
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _skiaSurface = e.NameScope.Find<ContentPresenter>(SkiaSurfacePartName);
        if (_skiaSurface != null && _skiaSurface.Content == null)
        {
#if WINDOWS_UWP
            // SKSwapChainPanel performs better than SKXamlCanvas.
            var swapChainPanel = new SKSwapChainPanel
            {
                DrawInBackground = _drawInBackground
            };
            swapChainPanel.PaintSurface += OnPaintSurface;
            _skiaSurface.Content = swapChainPanel;
#else
            // WinAppSdk doesn't have SKSwapChainPanel yet.
            // SKSwapChainPanel doesn't work in WASM yet.
            var xamlCanvas = new SKCanvasControl();
            xamlCanvas.Draw += OnPaintSurface;
            _skiaSurface.Content = xamlCanvas;
#endif
            _animationTimer = new AnimationTimer(this, fps: 120);
        }
    }

    /// <summary>
    /// Schedules a repaint and a call to <see cref="PaintNextAnimationFrame"/>. Overlapping calls
    /// between frame boundaries are coalesced.
    /// </summary>
    internal void InvalidateAnimation()
    {
#if WINDOWS_UWP
        var swapChainPanel = _skiaSurface?.Content as SKSwapChainPanel;
        swapChainPanel?.Invalidate();
#else
        var xamlCanvas = _skiaSurface?.Content as SKCanvasControl;
        xamlCanvas?.InvalidateVisual();
#endif
    }

#if WINDOWS_UWP
    private void OnPaintSurface(object? sender, SKPaintGLSurfaceEventArgs e)
    {
        this.PaintNextAnimationFrame(e.Surface, e.BackendRenderTarget.Width, e.BackendRenderTarget.Height);
    }
#else
    private void OnPaintSurface(object? sender, SKCanvasEventArgs e)
    {
        //this.PaintNextAnimationFrame(e.Surface, e.Info.Width, e.Info.Height);
        this.PaintNextAnimationFrame(e.Surface, (int)Bounds.Width, (int)Bounds.Height);
    }
#endif

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        /*
#if HAS_UNO
        // XamlRoot.IsHostVisible isn't implemented in Uno.
        OnXamlRootChanged(isHostVisible:true);
#else
        this.XamlRoot.Changed += (XamlRoot xamlRoot, XamlRootChangedEventArgs a) =>
        {
            OnXamlRootChanged(xamlRoot.IsHostVisible);
        };
        OnXamlRootChanged(this.XamlRoot.IsHostVisible);
#endif
*/
    }
}
