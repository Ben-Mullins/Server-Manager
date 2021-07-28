using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServerProjectTracker.AppLogic;
using ServerProjectTracker.Models;

namespace ServerProjectTracker.Pages.Tracker
{
    public class CreateProjectModel : PageModel
    {
        private readonly Data.ServerProjectTrackerContext _context;

        public CreateProjectModel(Data.ServerProjectTrackerContext context)
        {
            _context = context;
        }

        [BindProperty, Required, MaxLength(250)]
        public string ProjectTitle { get; set; }

        [BindProperty, Required]
        public string ProjectDescription { get; set; }

        [BindProperty, MaxLength(250)]
        public string ProjectLangauge { get; set; }

        [BindProperty, MaxLength(250)]
        public string ProjectDatabase { get; set; }

        [BindProperty, MaxLength(250)]
        public string ProjectBackend { get; set; }

        [BindProperty]
        public string ProjectTechnologyMisc { get; set; }

        [BindProperty]
        public string ProjectLink { get; set; }

        [BindProperty]
        public string DockerId { get; set; }

        [BindProperty]
        public string TitleError { get; set; }

        public IActionResult OnGet()
        {
            var userId = Session.getUserId(HttpContext.Session);
            if (userId == null) return RedirectToPage("/Index");

            var userAccessLevel = (int)Session.getUserAccess(HttpContext.Session);
            if(userAccessLevel < 0 || userAccessLevel > 3) return RedirectToPage("/Tracker/Index");
            ViewData["UserAccess"] = userAccessLevel.ToString();

            return Page();
        }

        public IActionResult OnPost()
        {
            var userId = Session.getUserId(HttpContext.Session);
            if (userId == null) return RedirectToPage("/Index");

            var accessLevel = (int)Session.getUserAccess(HttpContext.Session);
            ViewData["UserAccess"] = accessLevel.ToString();

            var project = _context.Project.FirstOrDefault(p => p.ProjectTitle == ProjectTitle);
            if (project != null)
            {
                TitleError = "That Project Title is already in use";
                return Page();
            }

            if (ProjectLangauge == "") ProjectLangauge = null;
            if (ProjectDatabase == "") ProjectDatabase = null;
            if (ProjectBackend == "") ProjectBackend = null;
            if (ProjectTechnologyMisc == "") ProjectTechnologyMisc = null;
            if (ProjectLink == "") ProjectLink = null;
            if (DockerId == "") DockerId = null;

            var projector = new ProjectLogic(_context);
            projector.CreateProject((int)userId, ProjectTitle, ProjectDescription, ProjectLangauge, ProjectDatabase, ProjectBackend, ProjectTechnologyMisc, ProjectLink, DockerId);

            return RedirectToPage("/Tracker/Index");
        }
    }
}
