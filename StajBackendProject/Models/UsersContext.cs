using Microsoft.EntityFrameworkCore;

namespace StajBackendProject.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions<UsersContext> options):base(options) { }
        public DbSet<Users> Users { get; set; } = null;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Users>()
                .Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Users>()
                .Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Users>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Users>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique()
                .HasFilter("[PhoneNumber] IS NOT NULL AND [PhoneNumber] <> ''");
        }
    }
}
