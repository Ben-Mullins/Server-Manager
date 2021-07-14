using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServerProjectTracker.AppLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServerProjectTracker.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly Data.ServerProjectTrackerContext _context;

        public IndexModel(ILogger<IndexModel> logger, Data.ServerProjectTrackerContext context)
        {
            _logger = logger;
            _context = context;
        }

        [BindProperty, Required]
        public string Username { get; set; }

        [BindProperty, Required]
        public string Password { get; set; }

        [BindProperty]
        public string ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            var userId = Session.getUserId(HttpContext.Session);
            if (userId != null) return RedirectToPage("/Tracker/Index");

            ErrorMessage = "";

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string password = Encryptor.encryptPass(Password);

            var User = await _context.Users.Where(u => u.Username == Username).FirstOrDefaultAsync(u => u.Password == password);

            if (User == null)
            {
                ErrorMessage = "Either the username or password was incorrect";
                return Page();
            }

            Session.setUser(HttpContext.Session, User);

            return RedirectToPage("/Tracker/Index");
        }
    }
}
