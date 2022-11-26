

using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models.DatabaseModels
{
    public class DbDataContext: DbContext
    {
        public DbDataContext(DbContextOptions<DbDataContext> options):
            base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ConnectedUser> ConnectedUsers { get; set; }

    }
}
