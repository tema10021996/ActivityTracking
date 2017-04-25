using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using ActivityTracking.DomainModel;

namespace ActivityTracking.DAL.EntityFramework
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext() : base("ApplicationDB") {}

        public DbSet<UserLogin> JustUsers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Reason> Reasons { get; set;}
        public DbSet<Absence> Absenсes { get; set;}
        public static ApplicationContext Create()
        {
            return new ApplicationContext();
        }
    }
}
