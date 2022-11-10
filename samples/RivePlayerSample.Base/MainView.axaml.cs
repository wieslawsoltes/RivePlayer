using Avalonia.Controls;
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

    protected override void OnLoaded()
    {
        base.OnLoaded();

        // RivePlayer.Source = "https://public.rive.app/community/runtime-files/2244-4463-animated-login-screen.riv";
        // RivePlayer.Source = "https://public.rive.app/community/runtime-files/3466-7249-3d-cube-demo.riv";
        // RivePlayer.Source = "https://public.rive.app/community/runtime-files/3375-7092-the-pumpking-army.riv";
    }
}

