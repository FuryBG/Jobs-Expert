using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using WebApplication1.Models.DatabaseModels;
using static System.Net.Mime.MediaTypeNames;

namespace WebApplication1.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        DbDataContext _dbDataContext;
        public ChatHub(DbDataContext dbDataContext)
        {
            _dbDataContext = dbDataContext;
        }

        public override Task OnConnectedAsync()
        {
            var claims = Context.User.Claims.ToList();

            //Filter specific claim    
            int userId = int.Parse(claims?.FirstOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", StringComparison.OrdinalIgnoreCase))?.Value);
            ConnectedUser connected = _dbDataContext.ConnectedUsers.Where(x => x.Id == userId).FirstOrDefault();
            if (connected != null)
            {
                connected.connectionId = Context.ConnectionId;
                _dbDataContext.Entry(connected).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            else
            {
                _dbDataContext.ConnectedUsers.Add(new ConnectedUser() { connectionId = Context.ConnectionId, Id = (int)userId });
            }
            _dbDataContext.SaveChanges();
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var claims = Context.User.Claims.ToList();
            int userId = int.Parse(claims?.FirstOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", StringComparison.OrdinalIgnoreCase))?.Value);

            _dbDataContext.ConnectedUsers.Remove(new ConnectedUser() { connectionId = Context.ConnectionId, Id = (int)userId });
            _dbDataContext.SaveChanges();
            Console.WriteLine(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(UserConnection userConnection)
        {
            ConnectedUser connectedUser = _dbDataContext.ConnectedUsers.Where(x => x.Id == userConnection.UserId).FirstOrDefault();
            string test = Context.User.Identity.Name;
            Message newMessage = saveMessage(userConnection);
            Dictionary<string, object> rawMessage = new Dictionary<string, object>();
            rawMessage.Add("id", newMessage.Id);
            rawMessage.Add("userId", newMessage.UserId);
            rawMessage.Add("roomId", newMessage.RoomId);
            rawMessage.Add("text", newMessage.Text);
            rawMessage.Add("userName", userConnection.UserName);
            rawMessage.Add("created", newMessage.Created);
            if (connectedUser != null)
            {
                await Clients.Client(connectedUser.connectionId).SendAsync("MessageReceived", rawMessage);
            }
        }

        public Message saveMessage(UserConnection userConnection)
        {
            var claims = Context.User.Claims.ToList();
            int userId = int.Parse(claims?.FirstOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", StringComparison.OrdinalIgnoreCase))?.Value);
            Message newMessage = new Message
            {
                RoomId = userConnection.RoomId,
                Text = userConnection.Message,
                UserId = userId
            };
            _dbDataContext.Messages.Add(newMessage);
            _dbDataContext.SaveChanges();
            return (newMessage);
        }
    }
}
