using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServerProjectTracker.AppLogic;

namespace ServerProjectTracker.Pages.User
{
    public class EditUserModel : PageModel
    {
        private readonly Data.ServerProjectTrackerContext _context;

        public EditUserModel(Data.ServerProjectTrackerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Users Users { get; set; }

        [BindProperty, Required]
        public string Firstname { get; set; }

        [BindProperty, Required]
        public string Lastname { get; set; }

        [BindProperty, Required]
        public string Password { get; set; }

        [BindProperty, Required]
        public string ConfirmPassword { get; set; }

        [BindProperty]
        public string ConfirmError { get; set; }

        public IActionResult OnGet()
        {
            var userId = Session.getUserId(HttpContext.Session);
            if (userId == null) return RedirectToPage("/Index");

            Users = _context.Users.FirstOrDefault(u => u.UserId == userId);
            Firstname = Users.Firstname;
            Lastname = Users.Lastname;

            return Page();
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("/User/EditUser");
        }

        public IActionResult OnPostBasic()
        {
            var userId = Session.getUserId(HttpContext.Session);
            if (userId == null) return RedirectToPage("/Index");

            Users = _context.Users.FirstOrDefault(u => u.UserId == userId);

            Users.Firstname = Firstname;
            Users.Lastname = Lastname;
            _context.SaveChanges();

            return RedirectToPage("/User/EditUser");
        }

        public IActionResult OnPostPass()
        {
            var userId = Session.getUserId(HttpContext.Session);
            if (userId == null) return RedirectToPage("/Index");

            Users = _context.Users.FirstOrDefault(u => u.UserId == userId);
            Firstname = Users.Firstname;
            Lastname = Users.Lastname;

            var error = false;

            if (ConfirmPassword.CompareTo(Password) != 0)
            {
                ConfirmError = "Passwords do not match";
                Password = "";
                ConfirmPassword = "";
                error = true;
            }

            if (error) return Page();

            Users.Password = Encryptor.encryptPass(Password);
            _context.SaveChanges();

            return RedirectToPage("/User/EditUser");
        }
    }
}
