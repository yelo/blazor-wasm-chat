using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using RetroChat.Shared.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace RetroChat.Client.Components
{
    public partial class ChatComponent : ComponentBase
    {
        [Inject]
        protected HttpClient Http { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected ILogger<ChatComponent> Logger { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public User User { get; set; }

        private HubConnection hubConnection;
        private List<ChatLine> Backlog = new List<ChatLine>();
        private List<User> CurrentUsers = new List<User>();

        private string Message { get; set; } = string.Empty;
        private bool IsConnected => hubConnection?.State == HubConnectionState.Connected;

        public async Task Reset()
        {
            await hubConnection.StopAsync();
            Backlog.Clear();
        }

        private async Task DoSubmit(KeyboardEventArgs e)
        {
            switch (e.Code)
            {
                case "Enter":
                    await SendMessage();
                    break;
            }
        }

        private async Task SendMessage()
        {
            if (string.IsNullOrEmpty(Message.Trim()) || Message.Trim().Length > 128) return;

            var chatLine = new ChatLine
            {
                From = User,
                Message = Message
            };

            await hubConnection.SendAsync("SendMessage", chatLine);
            Message = string.Empty;
        }

        private async Task InitSignalR()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("chathub"))
                .Build();

            hubConnection.On<ChatLine>("OnMessageRecieved", (chatLine) =>
            {
                var cl = chatLine;
                Backlog.Add(cl);
                StateHasChanged();
                JSRuntime.InvokeVoidAsync("scrollToBottom", "chat-history");
            });

            hubConnection.On<List<User>>("OnUserAnnounce", (users) =>
            {
                CurrentUsers = users;
                StateHasChanged();
            });

            hubConnection.On<int>("ForceAnnounce", async _ => await AnnouncePrecence());

            await hubConnection.StartAsync();

            if (IsConnected)
            {
                await AnnouncePrecence();
            }
        }

        private async Task AnnouncePrecence()
        {
            await hubConnection.SendAsync("AnnouncePrecense", User);
        }

        protected override async Task OnInitializedAsync()
        {
            Backlog = await Http.GetFromJsonAsync<List<ChatLine>>(NavigationManager.ToAbsoluteUri("chat/backlog"));
            await InitSignalR();

            await Task.Delay(50);
            await JSRuntime.InvokeVoidAsync("scrollToBottom", "chat-history");
        }
    }
}