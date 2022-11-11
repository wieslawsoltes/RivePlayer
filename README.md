# RivePlayer

[![NuGet](https://img.shields.io/nuget/v/Rive.svg)](https://www.nuget.org/packages/Rive)
[![NuGet](https://img.shields.io/nuget/dt/Rive.svg)](https://www.nuget.org/packages/Rive)

A [rive](https://rive.app/) animation player control for Avalonia.

## Usage

https://www.nuget.org/packages/Rive/

```xaml
<Application.Styles>
    <!-- ... -->
    <StyleInclude Source="avares://Rive/RivePlayer.axaml" />
</Application.Styles>
```

```xaml
xmlns:rive="using:Rive"
```

```xaml
<rive:RivePlayer x:Name="RivePlayer"
                 Width="600"
                 Height="600"
                 DrawInBackground="True"
                 Source="/Assets/animated-login-screen.riv">
  <rive:BoolInput Target="isChecking"
                  Value="{Binding #IsChecking.IsChecked, Mode=OneWay}" />
  <rive:NumberInput Target="numLook"
                    Value="{Binding #NumLook.Value, Mode=OneWay}" />
  <rive:BoolInput Target="isHandsUp"
                  Value="{Binding #IsHandsUp.IsChecked, Mode=OneWay}" />
  <rive:TriggerInput x:Name="FailureTrigger"
                     Target="trigFail" />
  <rive:TriggerInput x:Name="SuccessTrigger"
                     Target="trigSuccess" />
</rive:RivePlayer>
```

```xaml
Source="/Assets/animated-login-screen.riv"
```

```xaml
Source="https://public.rive.app/community/runtime-files/2244-4463-animated-login-screen.riv"
```

## License

Based on sources from [CommunityToolkit](https://github.com/CommunityToolkit/Labs-Windows/tree/main/labs/RivePlayer)

RivePlayer is licensed under the [MIT license](LICENSE.TXT).
