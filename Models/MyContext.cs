using Microsoft.EntityFrameworkCore;

namespace WannaBet.Models
{
    public class MyContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }
        // "users" table is represented by this DbSet "Users"
        public DbSet<User> Users { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<Reserve> Reserves { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
    }
}