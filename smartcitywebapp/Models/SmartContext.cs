using SmartCityWebApp.Models;
using System.Data.Entity;

/*namespace SmartCityWebApp
{
    public class SmartContext : DbContext
    {
        public DbSet<Role> RoleDB { get; set; }
        public DbSet<User> UserDB { get; set; }
        public DbSet<Housing> HousingDB { get; set; }
        public DbSet<Notation> NotationDB { get; set; }        
        public DbSet<Message> MessageDB { get; set; }
        public DbSet<Bed> BedDB { get; set; }
        public DbSet<Locality> LocalityDB { get; set; }

        public SmartContext() : base("name=SmartConnection")
        {                
        }
                
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                    .HasRequired<Role>(r => r.Role)
                    .WithMany()
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Housing>()
                    .HasRequired<User>(u => u.Host)
                    .WithMany()
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Notation>()
                    .HasRequired<User>(u => u.Origin)
                    .WithMany()
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Notation>()
                    .HasRequired<Housing>(h => h.Housing)
                    .WithMany()
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Message>()
                    .HasRequired<User>(u => u.Sender)
                    .WithMany()
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Message>()
                    .HasRequired<User>(u => u.Reciever)
                    .WithMany()
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Message>()
                    .HasOptional<Housing>(h => h.Housing)
                    .WithMany()
                    .WillCascadeOnDelete(false);
        }
    }
}*/