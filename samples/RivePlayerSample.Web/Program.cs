using System;
using System.Runtime.Versioning;
using Avalonia;
using Avalonia.Web;
using RivePlayerSample.Base;

[assembly:SupportedOSPlatform("browser")]

internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            BuildAvaloniaApp().SetupBrowserApp("out");
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
