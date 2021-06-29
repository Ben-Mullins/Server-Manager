using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public void OnGet()
        {
        }
    }
}
