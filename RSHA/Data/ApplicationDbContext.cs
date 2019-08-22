using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RSHA.Models;

namespace RSHA.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Requests> Requests { get; set; }
        public DbSet<ProblemTypes> ProblemTypes { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Mechanics> Mechanics { get; set; }
        public DbSet<Feedbacks> Feedbacks { get; set; }
        public DbSet<BugRapports> BugRapports { get; set; }
    }
}
