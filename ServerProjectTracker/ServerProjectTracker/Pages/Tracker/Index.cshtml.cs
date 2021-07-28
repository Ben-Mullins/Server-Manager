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
    public class IndexModel : PageModel
    {
        private readonly Data.ServerProjectTrackerContext _context;

        public IndexModel(Data.ServerProjectTrackerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int UserAccessLevel { get; set; }

        public List<Project> ProjectList { get; set; }

        public IActionResult OnGet()
        {
            var userId = Session.getUserId(HttpContext.Session);
            if (userId == null) return RedirectToPage("/Index");

            UserAccessLevel = (int)Session.getUserAccess(HttpContext.Session);
            ViewData["UserAccess"] = UserAccessLevel.ToString();

            ProjectSecurityLogic security = new ProjectSecurityLogic(_context);

            ProjectList = security.DetermineViewableProjects((int)userId);

            return Page();
        }
    }
}
