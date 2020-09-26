using RetroChat.Shared.Models;
using System.Collections.Generic;
using System.Linq;

namespace RetroChat.Server.Services
{
    public class ChatService : IChatService
    {
        private readonly IList<ChatLine> _backlog;
        private readonly IList<User> _currentUsers;

        public ChatService()
        {
            _currentUsers = new List<User>();
            _backlog = new List<ChatLine>();
        }

        public IEnumerable<ChatLine> FetchBacklog()
        {
            return _backlog;
        }

        public void PushToBacklog(ChatLine chatLine)
        {
            if (_backlog.Count >= 100)
            {
                _backlog.RemoveAt(0);
            }
            _backlog.Add(chatLine);
        }

        public void AddUser(User user)
        {
            var check = _currentUsers.FirstOrDefault(cu => cu.Handle.Equals(user.Handle, System.StringComparison.InvariantCultureIgnoreCase));
            if (check == null)
                _currentUsers.Add(user);
        }

        public void RemoveUser(string handle)
        {
            var cu = _currentUsers.FirstOrDefault(cu => cu.Handle.Equals(handle, System.StringComparison.InvariantCultureIgnoreCase));
            if (cu != null)
                _currentUsers.Remove(cu);
        }

        public bool UserIsConnected(string handle)
        {
            return _currentUsers.Any(cu => cu.Handle.Equals(handle, System.StringComparison.InvariantCultureIgnoreCase));
        }

        public IList<User> GetCurrentUsers()
        {
            return _currentUsers;
        }

        public void ClearCurrentUsers()
        {
            _currentUsers.Clear();
        }
    }
}