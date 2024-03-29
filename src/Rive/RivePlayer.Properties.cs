// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Metadata;
using Avalonia.Reactive;
using RiveSharp;

namespace Rive;

// XAML properies for RivePlayer.
public partial class RivePlayer
{
    static RivePlayer()
    {
        SourceProperty.Changed.Subscribe(new AnonymousObserver<AvaloniaPropertyChangedEventArgs<string>>(OnSourceNameChanged));
        ArtboardProperty.Changed.Subscribe(new AnonymousObserver<AvaloniaPropertyChangedEventArgs<string>>(OnArtboardNameChanged));
        StateMachineProperty.Changed.Subscribe(new AnonymousObserver<AvaloniaPropertyChangedEventArgs<string>>(OnStateMachineNameChanged));
        StateMachineInputCollectionProperty.Changed.Subscribe(new AnonymousObserver<AvaloniaPropertyChangedEventArgs<StateMachineInputCollection>>(OnStateMachineInputCollectionChanged));
    }
    
    // Monotonically increasing ID of the current Source. Increments everytime Source changes.
    // This token is used to cancel pending async load operations when new ones are started.
    private int _currentSourceToken = 0;

    /// <summary>
    /// Identifies the <see cref="Source"/> property.
    /// </summary>
    public static readonly StyledProperty<string> SourceProperty =
        AvaloniaProperty.Register<RivePlayer, string>(nameof(Source));

    /// <summary>
    /// Identifies the <see cref="Artboard"/> property.
    /// </summary>
    public static readonly StyledProperty<string> ArtboardProperty =
        AvaloniaProperty.Register<RivePlayer, string>(nameof(Artboard));

    /// <summary>
    /// Identifies the <see cref="StateMachine"/> property.
    /// </summary>
    public static readonly StyledProperty<string> StateMachineProperty =
        AvaloniaProperty.Register<RivePlayer, string>(nameof(StateMachine));

    /// <summary>
    /// Identifies the <see cref="StateMachineInputCollection"/> property.
    /// </summary>
    public static readonly StyledProperty<StateMachineInputCollection> StateMachineInputCollectionProperty =
        AvaloniaProperty.Register<RivePlayer, StateMachineInputCollection>(nameof(StateMachineInputCollection));

    /// <summary>
    /// URI to the `.riv` file to load into the app. Supported schemes are `http`, `https`, and `ms-appx`.
    /// </summary>
    public string Source
    {
        get => (string)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    /// <summary>
    /// Name of the Rive artboard to instantiate. If empty, the default artboard from the Rive file
    /// is loaded.
    /// will be loaded.
    /// </summary>
    public string Artboard
    {
        get => (string)GetValue(ArtboardProperty);
        set => SetValue(ArtboardProperty, value);
    }

    /// <summary>
    /// Name of the Rive state machine to instantiate from the artboard. If empty, the the default
    /// state machine is instantiated. If a state machine with the given name does not exist in the
    /// artboard, the runtime attempts to load a (deprecated) Rive animation of the same name.
    /// </summary>
    public string StateMachine
    {
        get => (string)GetValue(StateMachineProperty);
        set => SetValue(StateMachineProperty, value);
    }

    /// <summary>
    /// Holds the collection of <see cref="StateMachineInput"/>. This can be populated directly from XAML:
    ///
    ///   <rive:RivePlayer Source="ms-appx:///mystatemachine.riv">
    ///     <rive:BoolInput Target="inputNameInStateMachine" Value="True" />
    ///     <rive:NumberInput Target="inputNameInStateMachine" Value="{x:Bind ...}" />
    ///     <rive:TriggerInput Target="inputNameInStateMachine" x:Name="..." />
    ///   </rive:RivePlayer>
    ///
    /// </summary>
    [Content]
    public StateMachineInputCollection StateMachineInputCollection
    {
        get => (StateMachineInputCollection)GetValue(StateMachineInputCollectionProperty);
        set => SetValue(StateMachineInputCollectionProperty, value);
    }

    private static void OnSourceNameChanged(AvaloniaPropertyChangedEventArgs<string> obj)
    {
        var player = (RivePlayer)obj.Sender;
        var newSourceName = (string)obj.NewValue.GetValueOrDefault();
        // Clear the current Scene while we wait for the new one to load.
        player.sceneActionsQueue.Enqueue(() => player._scene = new Scene());

        ++player._currentSourceToken;  // Cancel any other active async source load operation.
        // Defer state machine inputs here until the new file is loaded.
        player._deferredSMInputsDuringAsyncSourceLoad = new List<Action>();
        player.LoadSourceFileDataAsync(newSourceName, player._currentSourceToken);
    }

    private static void OnArtboardNameChanged(AvaloniaPropertyChangedEventArgs<string> obj)
    {
        var player = (RivePlayer)obj.Sender;
        var newArtboardName = (string)obj.NewValue.GetValueOrDefault();
        player.sceneActionsQueue.Enqueue(() => player._artboardName = newArtboardName);
        if (player._deferredSMInputsDuringAsyncSourceLoad != null)
        {
            // If a file is currently loading async, it will apply the new artboard once
            // it completes. Loading a new artboard also invalidates any state machine
            // inputs that were waiting for the file load.
            player._deferredSMInputsDuringAsyncSourceLoad.Clear();
        }
        else
        {
            player.sceneActionsQueue.Enqueue(() => player.UpdateScene(SceneUpdates.Artboard));
        }
    }

    private static void OnStateMachineNameChanged(AvaloniaPropertyChangedEventArgs<string> obj)
    {
        var player = (RivePlayer)obj.Sender;
        var newStateMachineName = (string)obj.NewValue.GetValueOrDefault();
        player.sceneActionsQueue.Enqueue(() => player._stateMachineName = newStateMachineName);
        if (player._deferredSMInputsDuringAsyncSourceLoad != null)
        {
            // If a file is currently loading async, it will apply the new state machine
            // once it completes. Loading a new state machine also invalidates any state
            // machine inputs that were waiting for the file load.
            player._deferredSMInputsDuringAsyncSourceLoad.Clear();
        }
        else
        {
            player.sceneActionsQueue.Enqueue(() => player.UpdateScene(SceneUpdates.StateMachine));
        }
    }

    private static void OnStateMachineInputCollectionChanged(AvaloniaPropertyChangedEventArgs<StateMachineInputCollection> obj)
    {
        // Clear the RivePlayer on the old reference so it quits updating us.
        var oldCollection = (StateMachineInputCollection?)obj.OldValue.GetValueOrDefault();
        oldCollection?.SetRivePlayer(null);

        var newCollection = (StateMachineInputCollection)obj.NewValue.GetValueOrDefault();
        newCollection.SetRivePlayer((RivePlayer)obj.Sender);
    }
}
