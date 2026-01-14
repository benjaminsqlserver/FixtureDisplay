using FixtureDisplay.Models;
using FixtureDisplay.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System.Net.Http;

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
        private List<SunderlandFixture>? fixtures;
        private string? selectedHomeTeam;
        private string? selectedAwayTeam;
        private bool searchPerformed = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadTeams();
        }

        private async Task LoadTeams()
        {
            teams = await FixtureService.GetAllTeamsAsync();
        }

        private async Task DisplayFixtures()
        {
            if (!string.IsNullOrEmpty(selectedHomeTeam) && !string.IsNullOrEmpty(selectedAwayTeam))
            {
                fixtures = await FixtureService.GetFixturesByTeamsAsync(selectedHomeTeam, selectedAwayTeam);
                searchPerformed = true;
            }
        }

        private void ResetPage()
        {
            NavigationManager.NavigateTo("/fixtures", forceLoad: true);
        }
    }
}