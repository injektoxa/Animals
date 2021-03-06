﻿using Microsoft.AspNet.Identity.EntityFramework;

namespace Animals.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public System.Data.Entity.DbSet<Animals.Models.Anamne> Anamnes { get; set; }

        public System.Data.Entity.DbSet<Animals.Models.Pet> Pets { get; set; }

        public System.Data.Entity.DbSet<Animals.Models.Diagnostic> Diagnostics { get; set; }

        public System.Data.Entity.DbSet<Animals.Models.Surgical_treatment> Surgical_treatment { get; set; }

        public System.Data.Entity.DbSet<Animals.Models.Treatment> Treatments { get; set; }
    }
}