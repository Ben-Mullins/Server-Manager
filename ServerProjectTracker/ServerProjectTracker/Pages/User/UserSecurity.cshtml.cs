using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServerProjectTracker.AppLogic;

namespace ServerProjectTracker.Pages.User
{
    public class UserSecurityModel : PageModel
    {
        private readonly Data.ServerProjectTrackerContext _context;

        public UserSecurityModel(Data.ServerProjectTrackerContext context)
        {
            _context = context;
        }

        public int UserId { get; set; }

        public List<Models.Users> Users { get; set; }

        public IActionResult OnGet()
        {
            var userId = Session.getUserId(HttpContext.Session);
            if (userId == null) return RedirectToPage("/Index");
            var accessLevel = Session.getUserAccess(HttpContext.Session);
            ViewData["UserAccess"] = accessLevel.ToString();

            UserId = (int)userId;

            if (accessLevel != 0) return RedirectToPage("/Tracker/Index");

            Users = _context.Users.OrderBy(u => u.UserAccessLevel).ToList();

            return Page();
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("/User/UserSecurity");
        }

        public IActionResult OnPostUpAccess(int UserId)
        {
            var userId = Session.getUserId(HttpContext.Session);
            if (userId == null) return RedirectToPage("/Index");
            var accessLevel = Session.getUserAccess(HttpContext.Session);
            if (accessLevel != 0) return RedirectToPage("/Tracker/Index");
            ViewData["UserAccess"] = accessLevel.ToString();

            var security = new UserLogic(_context);
            security.ElevateAccess(UserId, (int)userId);

            return RedirectToPage("/User/UserSecurity");
        }

        public IActionResult OnPostDownAccess(int UserId)
        {
            var userId = Session.getUserId(HttpContext.Session);
            if (userId == null) return RedirectToPage("/Index");
            var accessLevel = Session.getUserAccess(HttpContext.Session);
            if (accessLevel != 0) return RedirectToPage("/Tracker/Index");
            ViewData["UserAccess"] = accessLevel.ToString();

            var security = new UserLogic(_context);
            security.ReduceAccess(UserId, (int)userId);

            return RedirectToPage("/User/UserSecurity");
        }

        public IActionResult OnPostRevokeAccess(int UserId)
        {
            var userId = Session.getUserId(HttpContext.Session);
            if (userId == null) return RedirectToPage("/Index");
            var accessLevel = Session.getUserAccess(HttpContext.Session);
            if (accessLevel != 0) return RedirectToPage("/Tracker/Index");
            ViewData["UserAccess"] = accessLevel.ToString();

            var security = new UserLogic(_context);
            security.RevokeAccess(UserId, (int)userId);

            return RedirectToPage("/User/UserSecurity");
        }
    }
}
