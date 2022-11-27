using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.DatabaseModels;
using System.Text.Json;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private DbDataContext _dbDataContext;
        public ChatController(DbDataContext dbDataContext)
        {
            _dbDataContext = dbDataContext;
        }

        [HttpGet("/rooms")]
        [Authorize]
        public IActionResult GetChatRooms()
        {
            var claims = User.Claims.ToList();
            Room room = new Room();

            //Filter specific claim    
            int userId = int.Parse(claims?.FirstOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", StringComparison.OrdinalIgnoreCase))?.Value);
            List<Dictionary<string, object>> myAllRooms = new List<Dictionary<string, object>>();

            List<Dictionary<string, object>> dictionaryTest = new List<Dictionary<string, object>>();
            List<Participant> myRooms = _dbDataContext.Participants.Where(x => x.UserId == userId).ToList();
            foreach (Participant participant in myRooms)
            {
                Dictionary<string, object> myConversation = new Dictionary<string, object>();

                var requiredUserInfo = (from ep in _dbDataContext.Participants
                                        join us in _dbDataContext.Users on ep.UserId equals us.UserId
                                        where ep.UserId != participant.UserId && ep.RoomId == participant.RoomId
                                        select new
                                        {
                                            UserId = us.UserId,
                                            FirstName = us.FirstName,
                                            RoomId = ep.RoomId,
                                        }).ToList();

                var requiredMessagesInfo = (from ep in _dbDataContext.Messages
                                            join usr in _dbDataContext.Users on ep.UserId equals usr.UserId
                                            where ep.RoomId == participant.RoomId
                                            select new
                                            {
                                                UserName = usr.FirstName,
                                                Text = ep.Text,
                                                UserId = ep.UserId,
                                                RoomId = ep.RoomId,
                                                Created = ep.Created
                                            }).OrderBy(c => c.Created).ToList();
                myConversation.Add("roominfo", requiredUserInfo[0]);
                myConversation.Add("conversationinfo", requiredMessagesInfo);
                dictionaryTest.Add(myConversation);
            }

            return Ok(dictionaryTest);
        }
    }
}
