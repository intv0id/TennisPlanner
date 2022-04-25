using Microsoft.AspNetCore.Components;
using Radzen;
using TennisPlanner.App.Services;
using TennisPlanner.Shared.Helpers;
using TennisPlanner.Shared.Models;

namespace TennisPlanner.App.Pages;

public partial class Result
{
    public SearchResults? SearchResults { get; set; }

    private bool initialized = false;

    protected override async Task OnInitializedAsync()
    {
        if (initialized)
        {
            return;
        }

        SearchResultsDataProvider.ResultsChanged += OnResultsChanged;
        if (NavManager.TryGetQueryString<DateTime>(Constants.DateTimeQueryKey, out var dateTime))
        {
            await SearchResultsDataProvider.Init(dateTime: dateTime);
            if (NavManager.TryGetQueryString<string>(Constants.FiltersQueryKey, out var filters))
            {
                if (!SearchFiltersService.TryLoadFromJson(filters))
                {
                    NotificationService.Notify(severity: NotificationSeverity.Warning, summary: "Certains filtres de recherche n'ont pas été appliqués.");
                }
            }
        }

        initialized = true;
    }

    private void OnResultsChanged(object? sender, ResultsChangedArgs obj)
    {
        this.SearchResults = obj.NewSearchResults;
        InvokeAsync(() =>
        {
            StateHasChanged();
        });
    }

    private void goToIndex()
    {
        NavManager.NavigateTo(uri: "/", options: new NavigationOptions()
        { ForceLoad = true });
    }
}
