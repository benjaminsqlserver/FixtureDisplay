using FixtureDisplay.Models;
using FixtureDisplay.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;

namespace FixtureDisplay.Components.Pages
{
    public partial class Index
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected IFixtureService FixtureService { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        private List<Team>? teams;
        private List<int>? seasons;
        private List<HistoricalResult>? historicalResults;
        private string? selectedHomeTeam;
        private string? selectedAwayTeam;
        private int? selectedSeason;
        private bool searchPerformed = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadTeams();
            await LoadSeasons();
        }

        private async Task LoadTeams()
        {
            teams = await FixtureService.GetAllTeamsAsync();
        }

        private async Task LoadSeasons()
        {
            seasons = await FixtureService.GetDistinctSeasonsAsync();
        }

        private async Task DisplayResults()
        {
            if (IsSearchEnabled())
            {
                historicalResults = await FixtureService.GetHistoricalResultsByTeamsAndSeasonAsync(
                    selectedHomeTeam!,
                    selectedAwayTeam!,
                    selectedSeason!.Value);
                searchPerformed = true;
            }
        }

        private bool IsSearchEnabled()
        {
            return !string.IsNullOrEmpty(selectedHomeTeam)
                && !string.IsNullOrEmpty(selectedAwayTeam)
                && selectedSeason.HasValue;
        }

        private void ResetPage()
        {
            NavigationManager.NavigateTo("/fixtures", forceLoad: true);
        }
    }
}