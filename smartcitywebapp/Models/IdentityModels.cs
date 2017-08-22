using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System;

namespace SmartCityWebApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

       /* [Required]
        [StringLength(12)]
        public string PhoneNumber { get; set; }

        [Required]
        public bool IsBanned { get; set; }*/

        

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("name=SmartConnection", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Role> RoleDB { get; set; }
        public DbSet<User> UserDB { get; set; }
        public DbSet<Housing> HousingDB { get; set; }
        /*
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
        }
        */

        /*public DbSet<Notation> NotationDB { get; set; }
        public DbSet<Message> MessageDB { get; set; }
        public DbSet<Bed> BedDB { get; set; }
        public DbSet<Locality> LocalityDB { get; set; }*/

        /*        protected override void OnModelCreating(DbModelBuilder modelBuilder)
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
                }*/
    }
}