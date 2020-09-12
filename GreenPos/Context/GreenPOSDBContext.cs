using GreenPOS.Entity;
using Microsoft.EntityFrameworkCore;

namespace GreenPOS.Context
{
    public class GreenPOSDBContext : DbContext
    {
        public GreenPOSDBContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Contact>().ToTable("Contact");
            modelBuilder.Entity<Notes>().ToTable("Notes");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<Screen>().ToTable("Screen");
            modelBuilder.Entity<ScreenPermission>().ToTable("ScreenPermission");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<UserRole>().ToTable("UserRole");
        }

        public DbSet<Contact> Contact { get; set; }
        public DbSet<Notes> Notes { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Screen> Screen { get; set; }
        public DbSet<ScreenPermission> ScreenPermission { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserRole> UserRole { get; set; }

        public DbSet<Quote> Quote { get; set; }

        public DbSet<Facade> Facade { get; set; }
        public DbSet<HouseDesign> HouseDesign { get; set; }
        public DbSet<Promotion> Promotion { get; set; }

        public DbSet<Inclusion> Inclusion { get; set; }
        public DbSet<Package> Package { get; set; }

        public DbSet<InclusionDetail> InclusionDetail { get; set; }

        public DbSet<Document> Document { get; set; }

    }
}
