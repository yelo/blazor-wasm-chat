using RetroChat.Shared.Models;
using System.Collections.Generic;

namespace RetroChat.Server.Services
{
    public interface IChatService
    {
        void AddUser(User user);

        IEnumerable<ChatLine> FetchBacklog();

        IList<User> GetCurrentUsers();

        void PushToBacklog(ChatLine chatLine);

        void RemoveUser(string handle);

        bool UserIsConnected(string handle);

        void ClearCurrentUsers();
    }
}