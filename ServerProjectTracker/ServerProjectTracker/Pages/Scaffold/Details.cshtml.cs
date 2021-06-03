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
    public class DetailsModel : PageModel
    {
        private readonly ServerProjectTracker.Data.ServerProjectTrackerContext _context;

        public DetailsModel(ServerProjectTracker.Data.ServerProjectTrackerContext context)
        {
            _context = context;
        }

        public Project Project { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Project = await _context.Project.FirstOrDefaultAsync(m => m.ProjectId == id);

            if (Project == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
