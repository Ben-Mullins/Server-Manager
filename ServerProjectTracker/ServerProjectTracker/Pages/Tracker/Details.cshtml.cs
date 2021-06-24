using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServerProjectTracker.Models;

namespace ServerProjectTracker.Pages.Tracker
{
    public class DetailsModel : PageModel
    {
        private readonly Data.ServerProjectTrackerContext _context;

        public DetailsModel(Data.ServerProjectTrackerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Project Project { get; set; }

        public void OnGet(int ProjectId)
        {
            //The following data is placeholder, and should be removed once we have actual project data
            Project = new Project();
            Project.ProjectTitle = "Placeholder Title " + ProjectId;
        }
    }
}
