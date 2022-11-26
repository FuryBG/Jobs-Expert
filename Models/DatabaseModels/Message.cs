using System.Numerics;

namespace WebApplication1.Models.DatabaseModels
{
    public class Message
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public string Text { get; set; }
        public string Created { get; set; } = DateTime.Now.ToFileTimeUtc().ToString();
    }
}
