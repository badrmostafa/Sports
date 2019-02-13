using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace Sports.Models.Classes
{
    public class SportContext:DbContext
    {
        //ConnectionString
        public SportContext():base("SportContext")
        { }
        //DbSet<>
        public DbSet<Sport> Sports { get; set; }
        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<TypeOfSport> TypesOfSports { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<PricingPlan> PricingPlans { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Subscribe> Subscribs { get; set; }
        public DbSet<Hobby> Hobbies { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
    }
}