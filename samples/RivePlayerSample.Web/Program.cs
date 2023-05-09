using System;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Browser;
using RivePlayerSample.Base;

[assembly:SupportedOSPlatform("browser")]

internal class Program
{
    private static async Task Main(string[] args) 
    {
        try
        {
            await BuildAvaloniaApp().StartBrowserAppAsync("out");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>();
}
