namespace DataSyncToDoClientApp.MAUI.Models;

public class TodoItem : OfflineClientEntity
{
    public string Title { get; set; } = string.Empty;
    public bool IsComplete { get; set; } = false;
}
