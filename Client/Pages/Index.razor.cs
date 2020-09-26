using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using RetroChat.Client.Components;
using RetroChat.Shared.Models;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace RetroChat.Client.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject]
        protected ILogger<Index> Logger { get; set; }

        [Inject]
        protected HttpClient Http { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        private bool HandleIsAvailable { get; set; } = true;
        private string Handle { get; set; } = string.Empty;
        private User User;

        private ChatComponent ChatComponent;

        private async void InitChat(KeyboardEventArgs e)
        {
            switch (e.Code)
            {
                case "Enter":
                    await InitConnection();
                    break;

                default:
                    break;
            }
        }

        private async Task Close(MouseEventArgs e)
        {
            await ChatComponent.Reset();
            User = null;
        }

        private async void TapConnect(MouseEventArgs e)
        {
            await InitConnection();
        }

        private async Task InitConnection()
        {
            if (User == null && await ValidateHandle())
            {
                User = new User { Handle = Handle.Trim().Replace(" ", "_"), Color = Color.Red };
                Handle = string.Empty;
            }
            StateHasChanged();
        }

        private async Task<bool> ValidateHandle()
        {
            var trimmed = Handle.Trim().Replace(" ", "_");
            if (!string.IsNullOrEmpty(trimmed) && trimmed.Length <= 16)
            {
                var response = await Http.PostAsJsonAsync(NavigationManager.ToAbsoluteUri("chat/availability"), trimmed);
                var result = await response.Content.ReadFromJsonAsync<bool>();
                HandleIsAvailable = result;
                return result;
            }
            HandleIsAvailable = false;
            return false;
        }

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }
    }
}