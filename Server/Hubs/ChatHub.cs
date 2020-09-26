using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using RetroChat.Server.Services;
using RetroChat.Shared.Models;
using System;
using System.Threading.Tasks;

namespace RetroChat.Server.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(IChatService chatService, ILogger<ChatHub> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        public async Task SendMessage(ChatLine chatLine)
        {
            _chatService.PushToBacklog(chatLine);
            await Clients.All.SendAsync("OnMessageRecieved", chatLine);
        }

        public async Task AnnouncePrecense(User user)
        {
            _chatService.AddUser(user);
            await Clients.All.SendAsync("OnUserAnnounce", _chatService.GetCurrentUsers());
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _chatService.ClearCurrentUsers();
            Clients.All.SendAsync("ForceAnnounce", 0);
            return base.OnDisconnectedAsync(exception);
        }
    }
}