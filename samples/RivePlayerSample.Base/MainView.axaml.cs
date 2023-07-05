using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace RivePlayerSample.Base;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        // RivePlayer.Source = "https://public.rive.app/community/runtime-files/2244-4463-animated-login-screen.riv";
        // RivePlayer.Source = "https://public.rive.app/community/runtime-files/3466-7249-3d-cube-demo.riv";
        // RivePlayer.Source = "https://public.rive.app/community/runtime-files/3375-7092-the-pumpking-army.riv";
    }
}

