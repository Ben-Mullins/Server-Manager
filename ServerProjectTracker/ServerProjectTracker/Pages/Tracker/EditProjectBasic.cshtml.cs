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
    public class EditProjectBasicModel : PageModel
    {
        private readonly Data.ServerProjectTrackerContext _context;

        public EditProjectBasicModel(Data.ServerProjectTrackerContext context)
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

        [BindProperty]
        public string ProjectImageLink { get; set; }

        /// <summary>
        /// 0 - Full
        /// 1 - Partial
        /// 2 - Readonly
        /// </summary>
        [BindProperty]
        public int AccessLevel { get; set; }

        /// <summary>
        /// People who reach this page must have at least access level 1 or lower
        /// </summary>
        /// <param name="ProjectId"></param>
        public void OnGet(int ProjectId)
        {
            Project = new Project();
            Project.ProjectId = ProjectId;
            Project.ProjectLink = $"http://localhost/{ProjectId}";
            ProjectTitle = "Placeholder Title " + ProjectId;
            ProjectDescription = "This is a placeholder description for our project. We are currently not hosting any live project details in this web application yet. This project does not really exist. But this description can be used to tell what a project is about, and anything about how it works and what it's for.";
            ProjectLangauge = "C#";
            ProjectDatabase = "MS SQL Server";
            ProjectBackend = ".Net Core Framework 5.0";
            ProjectTechnologyMisc = "ASP.Net Blazor";
            ProjectLink = $"http://localhost/{ProjectId}";
            ProjectImageLink = "/images/placeholder.jpg";
            AccessLevel = 0;
        }
    }
}
