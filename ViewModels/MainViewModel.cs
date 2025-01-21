using CommunityToolkit.Datasync.Client;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using DataSyncToDoClientApp.MAUI.Models;
using DataSyncToDoClientApp.MAUI.Services;

namespace DataSyncToDoClientApp.MAUI.ViewModels;

public partial class MainViewModel(AppDbContext context, IAlertService alertService) : ObservableRecipient
{
    [ObservableProperty]
    private bool isRefreshing = false;

    [ObservableProperty]
    private ConcurrentObservableCollection<TodoItem> items = [];

    [ObservableProperty]
    public TodoItem? itemToAdd = new();

    public async Task RefreshItemsAsync(CancellationToken cancellationToken = default)
    {
        if (IsRefreshing)
        {
            return;
        }

        try
        {
            IsRefreshing = true;
            await context.SynchronizeAsync(cancellationToken);
            List<TodoItem> items = await context.TodoItems.ToListAsync(cancellationToken);
            Items.ReplaceAll(items);
        }
        catch (Exception ex)
        {
            await alertService.ShowErrorAlertAsync("RefreshItems", ex.Message);
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    public async Task UpdateItemAsync(string itemId, CancellationToken cancellationToken = default)
    {
        try
        {
            TodoItem? item = await context.TodoItems.FindAsync([itemId]);
            if (item is not null)
            {
                item.IsComplete = !item.IsComplete;
                _ = context.TodoItems.Update(item);
                _ = Items.ReplaceIf(x => x.Id == itemId, item);
                _ = await context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            await alertService.ShowErrorAlertAsync("UpdateItem", ex.Message);
        }
    }

    [RelayCommand]
    public async Task AddItemAsync(object returnValue, CancellationToken cancellationToken = default)
    {
        try
        {
            if (ItemToAdd is null)
            {
                return;
            }
            _ = context.TodoItems.Add(ItemToAdd);
            _ = await context.SaveChangesAsync(cancellationToken);
            Items.Add(ItemToAdd);
            ItemToAdd = new();
        }
        catch (Exception ex)
        {
            await alertService.ShowErrorAlertAsync("AddItem", ex.Message);
        }
    }
}