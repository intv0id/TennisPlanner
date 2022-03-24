using Microsoft.AspNetCore.Components;
using Radzen;
using TennisPlanner.Server.Helpers;
using TennisPlanner.Server.Models;
using TennisPlanner.Server.Services;
using TennisPlanner.Shared.Helpers;

namespace TennisPlanner.Server.Pages
{
    public partial class Result
    {
        public SearchResults? SearchResults { get; set; }

        protected override async Task OnInitializedAsync()
        {
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
            // TODO: redirect to 500
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
}