using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServerProjectTracker.Models;

namespace ServerProjectTracker.Data
{
    public class ServerProjectTrackerContext : DbContext
    {
        public ServerProjectTrackerContext (DbContextOptions<ServerProjectTrackerContext> options)
            : base(options)
        {
        }

        public DbSet<Project> Project { get; set; }

        public DbSet<Users> Users { get; set; }

        public DbSet<ProjectUsers> ProjectUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .HasAlternateKey(u => u.Username);
            modelBuilder.Entity<Project>()
                .HasAlternateKey(p => p.ProjectTitle);
            modelBuilder.Entity<ProjectUsers>()
                .HasKey(pu => new { pu.ProjectId, pu.UserId });
        }
    }
}
