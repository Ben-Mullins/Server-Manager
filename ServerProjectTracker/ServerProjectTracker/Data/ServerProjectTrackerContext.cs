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

        public DbSet<ServerProjectTracker.Models.Project> Project { get; set; }
    }
}
