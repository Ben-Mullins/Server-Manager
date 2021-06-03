using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ServerProjectTracker.Data;
using ServerProjectTracker.Models;

namespace ServerProjectTracker.Pages.Scaffold
{
    public class IndexModel : PageModel
    {
        private readonly ServerProjectTracker.Data.ServerProjectTrackerContext _context;

        public IndexModel(ServerProjectTracker.Data.ServerProjectTrackerContext context)
        {
            _context = context;
        }

        public IList<Project> Project { get;set; }

        public async Task OnGetAsync()
        {
            Project = await _context.Project.ToListAsync();
        }
    }
}
