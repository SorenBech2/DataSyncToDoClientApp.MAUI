using DataSyncToDoClientApp.MAUI.Models;
using DataSyncToDoClientApp.MAUI.ViewModels;

namespace DataSyncToDoClientApp.MAUI;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;

    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        indicator.IsRunning = true;
        await _viewModel.RefreshItemsAsync();
        indicator.IsRunning = false;
    }

    private void OnListItemTapped(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is TodoItem item)
        {
            _viewModel.UpdateItemCommand.Execute(item.Id);
        }

        if (sender is CollectionView itemList)
        {
            itemList.SelectedItem = null;
        }
    }

    private async void Refresh_Clicked(object sender, EventArgs e)
    {
        indicator.IsRunning = true;
        await _viewModel.RefreshItemsAsync();
        indicator.IsRunning = false;
    }
}

