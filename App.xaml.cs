using DataSyncToDoClientApp.MAUI.Models;

namespace DataSyncToDoClientApp.MAUI;

public partial class App : Application
{
    public App(IDbInitializer initializer)
    {
        InitializeComponent();
        initializer.Initialize();  // Initalize DB
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            return new Window(new AppShell()) { MaximumWidth = 400 };
        }
        else
        {
            return new Window(new AppShell());
        }
    }
}