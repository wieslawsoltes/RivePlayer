using Avalonia.Controls;

namespace RivePlayer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();

            //this.RivePlayer.Source = "https://public.rive.app/community/runtime-files/2244-4463-animated-login-screen.riv";
            //this.RivePlayer.Source = "https://public.rive.app/community/runtime-files/3466-7249-3d-cube-demo.riv";
            //this.RivePlayer.Source = "https://public.rive.app/community/runtime-files/3375-7092-the-pumpking-army.riv";
        }
    }
}
