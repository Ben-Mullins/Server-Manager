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

        [BindProperty]
        public string ProjectStatus { get; set; }

        /// <summary>
        /// 0 - Full
        /// 1 - Partial
        /// 2 - Readonly
        /// </summary>
        [BindProperty]
        public int AccessLevel { get; set; }

        public void OnGet(int ProjectId)
        {
            //The following data is placeholder, and should be removed once we have actual project data
            Project = new Project();
            Project.ProjectId = ProjectId;
            Project.ProjectTitle = "Placeholder Title " + ProjectId;
            Project.ProjectDescription = "This is a placeholder description for our project. We are currently not hosting any live project details in this web application yet. This project does not really exist. But this description can be used to tell what a project is about, and anything about how it works and what it's for.";
            Project.ProjectLangauge = "C#";
            Project.ProjectDatabase = "MS SQL Server";
            Project.ProjectBackend = ".Net Core Framework 5.0";
            Project.ProjectTechnologyMisc = "ASP.Net Blazor";
            Project.AddedDate = new DateTime();
            Project.UpdatedDate = new DateTime();
            Project.ProjectLink = $"http://localhost/{ProjectId}";
            Project.ProjectImageLink = "/images/placeholder.jpg";
            ProjectStatus = "Active";
            AccessLevel = 0;
        }
    }
}
