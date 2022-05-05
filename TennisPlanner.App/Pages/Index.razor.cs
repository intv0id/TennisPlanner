using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Radzen;
using TennisPlanner.App.Components;
using TennisPlanner.App.Services;
using TennisPlanner.Shared.Helpers;
using TennisPlanner.Shared.Models;
using TennisPlanner.Shared.Services.Logging;

namespace TennisPlanner.App.Pages;

public partial class Index
{
    [Inject]
    DialogService DialogService { get; set; }
    [Inject]
    ISearchFiltersService SearchFiltersService { get; set; }
    [Inject]
    NavigationManager NavManager { get; set; }
    [Inject]
    NotificationService NotificationService{ get; set; }
    [Inject]
    ILoggerService LoggerService { get; set; }
    [Inject]
    IUserConsentService UserConsent { get; set; }

    SearchModel searchModel = new();
    public List<FiltersProfileDto>? FiltersProfiles { get; set; };

    protected override async Task OnInitializedAsync()
    {
        searchModel.SelectedDate = DateTime.Now;

        if (UserConsent.IsLocalStorageEnabled())
        {
            FiltersProfiles = await SearchFiltersService.ListSavedProfileIdsInLocalStorageAsync();
        }
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
        searchParams.Add(new KeyValuePair<string, string>(Constants.FiltersQueryKey, SearchFiltersService.ToBase64()));
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

    private async Task SearchWithProfileIdAsync(string? id)
    {
        if (id == null)
        {
            LoggerService.Log(
                logLevel: LogLevel.Error,
                operationName: $"{nameof(Index)}.{nameof(this.SearchWithProfileIdAsync)}",
                message: "Null id in search profile loading.");
            NotificationService.Notify(NotificationSeverity.Error, summary: "Erreur pendant le chargement des filtres de recherche", detail: "NullId");
        }

        if (!await SearchFiltersService.TryLoadProfileFromLocalStorageAsync(id))
        {
            LoggerService.Log(
                logLevel: LogLevel.Error,
                operationName: $"{nameof(Index)}.{nameof(this.SearchWithProfileIdAsync)}",
                message: "Couldn't load profile.");
            NotificationService.Notify(NotificationSeverity.Error, summary: "Erreur pendant le chargement des filtres de recherche", detail: "Erreur de stockage local.");
            return;
        }

        HandleSearch();
    }
}
