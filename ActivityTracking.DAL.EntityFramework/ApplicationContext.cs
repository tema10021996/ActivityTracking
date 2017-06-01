﻿using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using ActivityTracking.DomainModel;

namespace ActivityTracking.DAL.EntityFramework
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext() : base("ApplicationDB") {}
        public DbSet<Reason> Reasons { get; set;}
        public DbSet<Absence> Absenсes { get; set;}
        public DbSet<WeekBeginningDay> WeekBeginningDay { get; set; }
        public static ApplicationContext Create()
        {
            return new ApplicationContext();
        }
    }
}
