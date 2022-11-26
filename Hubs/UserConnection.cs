namespace WebApplication1.Hubs
{
    public class UserConnection
    {
        public int ConnectedId { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
    }
}
