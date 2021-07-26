using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServerProjectTracker.AppLogic;
using ServerProjectTracker.Models;

namespace ServerProjectTracker.Pages.Tracker
{
    public class DetailsModel : PageModel
    {
        private readonly Data.ServerProjectTrackerContext _context;

        public DetailsModel(Data.ServerProjectTrackerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Project Project { get; set; }

        [BindProperty]
        public string ProjectStatus { get; set; }

        [BindProperty]
        public string ProjectState { get; set; }

        /// <summary>
        /// 0 - Full
        /// 1 - Partial
        /// 2 - Readonly
        /// </summary>
        [BindProperty]
        public int AccessLevel { get; set; }

        public IActionResult OnGet(int ProjectId)
        {
            var userId = Session.getUserId(HttpContext.Session);
            if (userId == null) return RedirectToPage("/Index");

            Project = _context.Project.FirstOrDefault(p => p.ProjectId == ProjectId);
            if (Project == null) return RedirectToPage("/Tracker/Index");

            ProjectSecurityLogic security = new ProjectSecurityLogic(_context);

            AccessLevel = security.DetermineAccessLevel(ProjectId, (int)userId);
            if (AccessLevel > 2) return RedirectToPage("/Tracker/Index");

            //The following data is placeholder, and should be removed once we have actual project data
            ProjectState = "running";
            ProjectStatus = "Exited (255) 21 minutes ago";

            return Page();
        }
    }
}
