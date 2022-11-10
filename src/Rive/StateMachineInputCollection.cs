// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Specialized;
using Avalonia;
using Avalonia.Collections;

namespace Rive;

/// <summary>
/// Manages a collection of StateMachineInput objects for <see cref="RivePlayer"/>. The
/// [Content] tag on RivePlayer instructs the XAML engine to automatically populate this
/// collection with nested inputs:
///
///   <rive:RivePlayer Source="...">
///       <!--  Adds this input to the StateMachineInputCollection.  -->
///       <rive:BoolInput Target=... />
///   </rive:RivePlayer>
///
/// </summary>
public class StateMachineInputCollection : AvaloniaList<AvaloniaObject>
{
    private WeakReference<RivePlayer> _rivePlayer = new WeakReference<RivePlayer>(null!);

    public StateMachineInputCollection()
    {
        CollectionChanged += InputsVectorChanged;
    }


    /// <summary>
    /// Establishes the <see cref="RivePlayer"/> whose state machine inputs this class will manage.
    /// If any given <see cref="StateMachineInput"/> was not already bound to this rive player, it also
    /// applies its current `Value` to the state machine.
    /// </summary>
    public void SetRivePlayer(RivePlayer? rivePlayer)
    {
        _rivePlayer = new WeakReference<RivePlayer>(rivePlayer!);
        foreach (StateMachineInput input in this)
        {
            input.SetRivePlayer(_rivePlayer);
        }
    }

    private void InputsVectorChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                foreach (var item in e.NewItems)
                {
                    var input = (StateMachineInput)item;
                    input.SetRivePlayer(_rivePlayer);
                }
                break;
            case NotifyCollectionChangedAction.Remove:
            case NotifyCollectionChangedAction.Move:
            case NotifyCollectionChangedAction.Replace:
            case NotifyCollectionChangedAction.Reset:
                foreach (var item in e.OldItems)
                {
                    var input = (StateMachineInput)item;
                    input.SetRivePlayer(new WeakReference<RivePlayer>(null!));
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        /* TODO:
        switch (e.Action)
        {
            case CollectionChange.ItemInserted:
            case CollectionChange.ItemChanged:
            {
                var input = (StateMachineInput)sender[(int)@event.Index];
                input.SetRivePlayer(_rivePlayer);
            }
            break;
            case CollectionChange.ItemRemoved:
            {
                var input = (StateMachineInput)sender[(int)@event.Index];
                input.SetRivePlayer(new WeakReference<RivePlayer>(null!));
                break;
            }
            case CollectionChange.Reset:
                foreach (StateMachineInput input in sender)
                {
                    input.SetRivePlayer(new WeakReference<RivePlayer>(null!));
                }
                break;
        }
        */
    }
}