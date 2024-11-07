using Microsoft.EntityFrameworkCore;

namespace RestApp.Entities
{
    public class AppDbContext : DbContext
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions <AppDbContext> options): base(options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>();

            modelBuilder.Entity<Comment>();

            modelBuilder.Entity<Post>();

            modelBuilder.Entity<Role>();
            
        }


        /*
        add-migration Init
        update-database
        remove-migration
        */
    }
}
