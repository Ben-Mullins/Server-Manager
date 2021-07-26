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
    public class AddSecurityUserModel : PageModel
    {
        private readonly Data.ServerProjectTrackerContext _context;

        public AddSecurityUserModel(Data.ServerProjectTrackerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Project Project { get; set; }

        [BindProperty]
        public string NewUser { get; set; }

        [BindProperty]
        public int? NewAccessLevel { get; set; }

        [BindProperty]
        public string UserError { get; set; }

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

            if (AccessLevel != 0) return RedirectToPage("/Tracker/Details", new { ProjectId });

            return Page();
        }

        public IActionResult OnGetSearch(int ProjectId, string userSearch)
        {
            var userId = Session.getUserId(HttpContext.Session);
            if (userId == null) return RedirectToPage("/Index");

            ProjectSecurityLogic security = new ProjectSecurityLogic(_context);
            var Users = security.GetPotentialNewUsers(ProjectId, (int)userId);

            return new JsonResult(Users);
        }

        public IActionResult OnPost(int ProjectId)
        {
            var userId = Session.getUserId(HttpContext.Session);
            if (userId == null) return RedirectToPage("/Index");

            Project = _context.Project.FirstOrDefault(p => p.ProjectId == ProjectId);
            if (Project == null) return RedirectToPage("/Tracker/Index");

            ProjectSecurityLogic security = new ProjectSecurityLogic(_context);

            AccessLevel = security.DetermineAccessLevel(ProjectId, (int)userId);
            if (AccessLevel > 2) return RedirectToPage("/Tracker/Index");

            if (AccessLevel != 0) return RedirectToPage("/Tracker/Details", new { ProjectId });

            var viableUsers = security.GetPotentialNewUsers(ProjectId, (int)userId);
            if(viableUsers.FindIndex(u => u == NewUser) == -1)
            {
                UserError = "Invalid Username, try again.";
                return Page();
            }

            Users newUser = _context.Users.FirstOrDefault(u => u.Username == NewUser);

            switch (NewAccessLevel)
            {
                case (0):
                    security.GrantOwnershipAccess(ProjectId, newUser.UserId, (int)userId);
                    break;
                case (1):
                    security.GrantDeveloperAccess(ProjectId, newUser.UserId, (int)userId);
                    break;
                case (2):
                    security.GrantViewerAccess(ProjectId, newUser.UserId, (int)userId);
                    break;
                default:
                    break;
            }

            return RedirectToPage("/Tracker/EditProjectSecurity", new { ProjectId });
        }
    }
}
