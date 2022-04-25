using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Radzen;
using TennisPlanner.App.Components;
using TennisPlanner.App.Services;
using TennisPlanner.Shared.Helpers;
using TennisPlanner.Shared.Models;

namespace TennisPlanner.App.Pages;

public partial class Index
{
    [Inject]
    DialogService DialogService { get; set; }

    [Inject]
    ISearchFiltersService SearchFiltersService { get; set; }

    SearchModel searchModel = new();

    protected override async Task OnInitializedAsync()
    {
        searchModel.SelectedDate = DateTime.Now;
    }

    private bool ValidateFields()
    {
        var selectedDate = searchModel.SelectedDate;
        if (selectedDate == null || rejectedDate((DateTime)selectedDate))
        {
            return false;
        }

        return true;
    }

    private async Task HandleSearch()
    {
        if (searchModel.SelectedDate == null)
        {
            LoggerService.Log(logLevel: LogLevel.Error, operationName: $"{nameof(Result)}.{nameof(this.HandleSearch)}", message: $"{nameof(searchModel.SelectedDate)} is null.");
            NotificationService.Notify(severity: NotificationSeverity.Error, summary: "A error occured.");
            return;
        }

        var searchParams = new List<KeyValuePair<string, string>>();
        searchParams.Add(new KeyValuePair<string, string>(Constants.DateTimeQueryKey, searchModel.SelectedDate?.ToString("yyyy-MM-dd")));
        searchParams.Add(new KeyValuePair<string, string>(Constants.FiltersQueryKey, SearchFiltersService.ToJson()));
        var queryString = QueryString.Create(searchParams);
        var searchUri = $"/Search{queryString.ToUriComponent()}";
        NavManager.NavigateTo(
            uri: searchUri,
            options: new NavigationOptions() { ForceLoad = false });
    }

    private async Task DisplayFiltersModal()
    {
        await DialogService.OpenAsync<SearchFiltersForm>($"Filtres de recherche",
               new Dictionary<string, object>(),
               new DialogOptions() { Width = "700px", Height = "570px", Resizable = true, Draggable = true });
    }

    void DateRender(DateRenderEventArgs args)
    {
        args.Disabled = rejectedDate(args.Date);
    }

    private bool rejectedDate(DateTime dateTime)
    {
        return dateTime.Date < DateTime.Today || dateTime.Date > DateTime.Today.AddDays(7);
    }
}
