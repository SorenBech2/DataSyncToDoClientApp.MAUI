namespace DataSyncToDoClientApp.MAUI.Services;
public interface IAlertService
{
    Task ShowErrorAlertAsync(string title, string message, string cancel = "OK");
}

public class AlertService : IAlertService
{
    public Task ShowErrorAlertAsync(string title, string message, string cancel = "OK")
    {
        var mainPage = Application.Current?.Windows[0].Page;
        return mainPage?.DisplayAlert(title, message, cancel) ?? Task.CompletedTask;
    }
}
