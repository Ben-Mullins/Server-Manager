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
    public class EditProjectBasicModel : PageModel
    {
        private readonly Data.ServerProjectTrackerContext _context;

        public EditProjectBasicModel(Data.ServerProjectTrackerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Project Project { get; set; }

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
        public string ProjectImageLink { get; set; }

        [BindProperty]
        public string DockerId { get; set; }

        [BindProperty]
        public string TitleError { get; set; }

        /// <summary>
        /// 0 - Full
        /// 1 - Partial
        /// 2 - Readonly
        /// </summary>
        [BindProperty]
        public int AccessLevel { get; set; }

        /// <summary>
        /// People who reach this page must have at least access level 1 or lower
        /// </summary>
        /// <param name="ProjectId"></param>
        public IActionResult OnGet(int ProjectId)
        {
            var userId = Session.getUserId(HttpContext.Session);
            if (userId == null) return RedirectToPage("/Index");

            Project = _context.Project.FirstOrDefault(p => p.ProjectId == ProjectId);
            if (Project == null) return RedirectToPage("/Tracker/Index");

            ProjectSecurityLogic security = new ProjectSecurityLogic(_context);

            AccessLevel = security.DetermineAccessLevel(ProjectId, (int)userId);
            if (AccessLevel > 2) return RedirectToPage("/Tracker/Index");

            if (AccessLevel == 2) return RedirectToPage("/Tracker/Details", new { ProjectId });

            ProjectTitle = Project.ProjectTitle;
            ProjectDescription = Project.ProjectDescription;
            ProjectLangauge = Project.ProjectLangauge;
            ProjectDatabase = Project.ProjectDatabase;
            ProjectBackend = Project.ProjectBackend;
            ProjectTechnologyMisc = Project.ProjectTechnologyMisc;
            ProjectLink = Project.ProjectLink;
            ProjectImageLink = Project.ProjectImageLink;
            DockerId = Project.DockerId;

            return Page();
        }

        public IActionResult OnPost(int ProjectId)
        {
            // Access Level Checks
            var userId = Session.getUserId(HttpContext.Session);
            if (userId == null) return RedirectToPage("/Index");

            Project = _context.Project.FirstOrDefault(p => p.ProjectId == ProjectId);
            if (Project == null) return RedirectToPage("/Tracker/Index");

            ProjectSecurityLogic security = new ProjectSecurityLogic(_context);

            AccessLevel = security.DetermineAccessLevel(ProjectId, (int)userId);
            if (AccessLevel > 2) return RedirectToPage("/Tracker/Index");

            if (AccessLevel == 2) return RedirectToPage("/Tracker/Details", new { ProjectId });

            // Update Checks
            var project = _context.Project.Where(p => p.ProjectTitle == ProjectTitle).FirstOrDefault(p => p.ProjectId != ProjectId);
            if (project != null)
            {
                TitleError = "That Project Title is already in use";
                return Page();
            }

            // Update the Project
            ProjectLogic projector = new ProjectLogic(_context);

            if (ProjectLangauge == "") ProjectLangauge = null;
            if (ProjectDatabase == "") ProjectDatabase = null;
            if (ProjectBackend == "") ProjectBackend = null;
            if (ProjectTechnologyMisc == "") ProjectTechnologyMisc = null;
            if (ProjectLink == "") ProjectLink = null;
            if (DockerId == "") DockerId = null;

            projector.UpdateProject((int)userId, ProjectId, ProjectTitle, ProjectDescription, ProjectLangauge, ProjectDatabase, ProjectBackend, ProjectTechnologyMisc, ProjectLink, DockerId);
            return RedirectToPage("/Tracker/EditProjectBasic", new { ProjectId });
        }
    }
}
