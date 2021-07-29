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
    public class EditProjectSecurityModel : PageModel
    {
        private readonly Data.ServerProjectTrackerContext _context;

        public EditProjectSecurityModel(Data.ServerProjectTrackerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Project Project { get; set; }

        [BindProperty]
        public int UserId { get; set; }

        [BindProperty]
        public int EditedUserId { get; set; }

        /// <summary>
        /// 0 - Full
        /// 1 - Partial
        /// 2 - Readonly
        /// </summary>
        [BindProperty]
        public int AccessLevel { get; set; }

        public List<ProjectAccessRule> ProjectAccessRules { get; set; }

        public IActionResult OnGet(int ProjectId)
        {
            var userId = Session.getUserId(HttpContext.Session);
            if (userId == null) return RedirectToPage("/Index");
            UserId = (int)userId;

            var UserAccessLevel = (int)Session.getUserAccess(HttpContext.Session);
            ViewData["UserAccess"] = UserAccessLevel.ToString();

            Project = _context.Project.FirstOrDefault(p => p.ProjectId == ProjectId);
            if (Project == null) return RedirectToPage("/Tracker/Index");

            ProjectSecurityLogic security = new ProjectSecurityLogic(_context);

            AccessLevel = security.DetermineAccessLevel(ProjectId, (int)userId);
            if (AccessLevel > 2) return RedirectToPage("/Tracker/Index");

            if (AccessLevel != 0) return RedirectToPage("/Tracker/Details", new { ProjectId });

            //Load Access Rules
            ProjectAccessRules = security.GetAccessRules(ProjectId);

            return Page();
        }

        public IActionResult OnPost(int ProjectId)
        {
            return RedirectToPage("/Tracker/EditProjectSecurity", new { ProjectId });
        }

        public IActionResult OnPostUpAccess(int ProjectId, int UserId)
        {
            var userId = Session.getUserId(HttpContext.Session);
            if (userId == null) return RedirectToPage("/Index");
            var UserAccessLevel = (int)Session.getUserAccess(HttpContext.Session);
            ViewData["UserAccess"] = UserAccessLevel.ToString();

            Project = _context.Project.FirstOrDefault(p => p.ProjectId == ProjectId);
            if (Project == null) return RedirectToPage("/Tracker/Index");

            ProjectSecurityLogic security = new ProjectSecurityLogic(_context);

            AccessLevel = security.DetermineAccessLevel(ProjectId, (int)userId);
            if (AccessLevel > 2) return RedirectToPage("/Tracker/Index");

            if (AccessLevel != 0) return RedirectToPage("/Tracker/Details", new { ProjectId });

            int currentAccess = security.GetAccessLevel(ProjectId, UserId);

            switch (currentAccess)
            {
                case (1):
                    security.GrantOwnershipAccess(ProjectId, UserId, (int)userId);
                    break;
                case (2):
                    security.GrantDeveloperAccess(ProjectId, UserId, (int)userId);
                    break;
                default:
                    break;
            }

            return RedirectToPage("/Tracker/EditProjectSecurity", new { ProjectId });
        }

        public IActionResult OnPostDownAccess(int ProjectId, int UserId)
        {
            var userId = Session.getUserId(HttpContext.Session);
            if (userId == null) return RedirectToPage("/Index");
            var UserAccessLevel = (int)Session.getUserAccess(HttpContext.Session);
            ViewData["UserAccess"] = UserAccessLevel.ToString();

            Project = _context.Project.FirstOrDefault(p => p.ProjectId == ProjectId);
            if (Project == null) return RedirectToPage("/Tracker/Index");

            ProjectSecurityLogic security = new ProjectSecurityLogic(_context);

            AccessLevel = security.DetermineAccessLevel(ProjectId, (int)userId);
            if (AccessLevel > 2) return RedirectToPage("/Tracker/Index");

            if (AccessLevel != 0) return RedirectToPage("/Tracker/Details", new { ProjectId });

            int currentAccess = security.GetAccessLevel(ProjectId, UserId);

            switch (currentAccess)
            {
                case (0):
                    security.GrantDeveloperAccess(ProjectId, UserId, (int)userId);
                    break;
                case (1):
                    security.GrantViewerAccess(ProjectId, UserId, (int)userId);
                    break;
                default:
                    break;
            }

            return RedirectToPage("/Tracker/EditProjectSecurity", new { ProjectId });
        }

        public IActionResult OnPostRevokeAccess(int ProjectId, int UserId)
        {
            var userId = Session.getUserId(HttpContext.Session);
            if (userId == null) return RedirectToPage("/Index");
            var UserAccessLevel = (int)Session.getUserAccess(HttpContext.Session);
            ViewData["UserAccess"] = UserAccessLevel.ToString();

            Project = _context.Project.FirstOrDefault(p => p.ProjectId == ProjectId);
            if (Project == null) return RedirectToPage("/Tracker/Index");

            ProjectSecurityLogic security = new ProjectSecurityLogic(_context);

            AccessLevel = security.DetermineAccessLevel(ProjectId, (int)userId);
            if (AccessLevel > 2) return RedirectToPage("/Tracker/Index");

            if (AccessLevel != 0) return RedirectToPage("/Tracker/Details", new { ProjectId });

            security.RevokeAccess(ProjectId, UserId, (int)userId);

            return RedirectToPage("/Tracker/EditProjectSecurity", new { ProjectId });
        }

        public IActionResult OnPostReturnAccess(int ProjectId, int UserId)
        {
            var userId = Session.getUserId(HttpContext.Session);
            if (userId == null) return RedirectToPage("/Index");
            var UserAccessLevel = (int)Session.getUserAccess(HttpContext.Session);
            ViewData["UserAccess"] = UserAccessLevel.ToString();

            Project = _context.Project.FirstOrDefault(p => p.ProjectId == ProjectId);
            if (Project == null) return RedirectToPage("/Tracker/Index");

            ProjectSecurityLogic security = new ProjectSecurityLogic(_context);

            AccessLevel = security.DetermineAccessLevel(ProjectId, (int)userId);
            if (AccessLevel > 2) return RedirectToPage("/Tracker/Index");

            if (AccessLevel != 0) return RedirectToPage("/Tracker/Details", new { ProjectId });

            security.GrantViewerAccess(ProjectId, UserId, (int)userId);

            return RedirectToPage("/Tracker/EditProjectSecurity", new { ProjectId });
        }
    }
}
