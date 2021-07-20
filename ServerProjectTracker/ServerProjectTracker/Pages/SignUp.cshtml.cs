using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ServerProjectTracker.AppLogic;
using ServerProjectTracker.Models;

namespace ServerProjectTracker.Pages
{
    public class SignUpModel : PageModel
    {
        private readonly Data.ServerProjectTrackerContext _context;

        public SignUpModel(Data.ServerProjectTrackerContext context)
        {
            _context = context;
        }

        [BindProperty, Required]
        public string Username { get; set; }

        [BindProperty, Required]
        public string Password { get; set; }

        [BindProperty, Required]
        public string ConfirmPassword { get; set; }

        [BindProperty]
        public string ConfirmError { get; set; }

        [BindProperty]
        public string UserError { get; set; }

        [BindProperty]
        public Models.Users Users { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Username == Username);
            var error = false;

            if (existingUser != null)
            {
                UserError = "That username is already in use";
                error = true;
            }
            if (ConfirmPassword.CompareTo(Password) != 0)
            {
                ConfirmError = "Passwords do not match";
                error = true;
            }

            if (error) return Page();

            Users.Username = Username;
            Users.Password = Encryptor.encryptPass(Password);
            Users.UserAccessLevel = 3;

            _context.Users.Add(Users);
            await _context.SaveChangesAsync();

            var user = _context.Users.FirstOrDefault(u => u.Username == Username);
            Session.setUser(HttpContext.Session, user);

            return RedirectToPage("/Tracker/Index");
        }
    }
}
