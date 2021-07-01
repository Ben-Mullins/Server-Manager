using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        /// <summary>
        /// 0 - Full
        /// 1 - Partial
        /// 2 - Readonly
        /// </summary>
        [BindProperty]
        public int AccessLevel { get; set; }

        /// <summary>
        /// People who reach this page must have access level 0
        /// </summary>
        /// <param name="ProjectId"></param>
        public void OnGet(int ProjectId)
        {
            Project = new Project();
            Project.ProjectId = ProjectId;
            Project.ProjectTitle = "Placeholder Title " + ProjectId;
            Project.ProjectLink = $"http://localhost/{ProjectId}";
        }
    }
}
