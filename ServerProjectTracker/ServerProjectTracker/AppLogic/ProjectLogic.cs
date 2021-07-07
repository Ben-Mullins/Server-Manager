using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServerProjectTracker.Models;

namespace ServerProjectTracker.AppLogic
{
    public class ProjectLogic
    {
        private readonly Data.ServerProjectTrackerContext _context;

        public ProjectLogic(Data.ServerProjectTrackerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This is a method for creating a new project and adding it to the database. Only the Title and Description are required
        /// </summary>
        /// <param name="UserId">The id of the user who created the project</param>
        /// <param name="ProjectTitle">The title of the project</param>
        /// <param name="ProjectDescription">The description of the project</param>
        /// <param name="ProjectLangauge">The programming language of the project</param>
        /// <param name="ProjectDatabase">The database used by the project</param>
        /// <param name="ProjectBackend">The backend used by the project</param>
        /// <param name="ProjectTechnologyMisc">Any other technology used by the project</param>
        /// <param name="ProjectLink">The link for the project on the server</param>
        /// <param name="DockerId">The id of the Docker Container</param>
        public void CreateProject(int UserId, string ProjectTitle, string ProjectDescription, string ProjectLangauge = null, string ProjectDatabase = null, string ProjectBackend = null, string ProjectTechnologyMisc = null, string ProjectLink = null, string DockerId = null)
        {
            var project = _context.Project.FirstOrDefault(p => p.ProjectTitle == ProjectTitle);
            if (project != null) throw new Exception("Error: That Project Title is already in use");
            
            Project newProject = new Project();

            newProject.ProjectTitle = ProjectTitle;
            newProject.ProjectDescription = ProjectDescription;
            newProject.ProjectLangauge = ProjectLangauge;
            newProject.ProjectDatabase = ProjectDatabase;
            newProject.ProjectBackend = ProjectBackend;
            newProject.ProjectTechnologyMisc = ProjectTechnologyMisc;
            newProject.ProjectLink = ProjectLink;
            newProject.DockerId = DockerId;
            newProject.AddedDate = DateTime.Now;
            newProject.UpdatedDate = DateTime.Now;

            _context.Project.Add(newProject);
            _context.SaveChanges();

            newProject = _context.Project.FirstOrDefault(p => p.ProjectTitle == ProjectTitle);
            var security = new ProjectSecurityLogic(_context);

            security.SetNewProjectOwner(newProject.ProjectId, UserId);
        }

        public void UpdateProject(int UserId, int ProjectId, string ProjectTitle, string ProjectDescription, string ProjectLangauge = null, string ProjectDatabase = null, string ProjectBackend = null, string ProjectTechnologyMisc = null, string ProjectLink = null, string DockerId = null)
        {
            Project project = _context.Project.FirstOrDefault(p => p.ProjectId == ProjectId);

            if (project == null) throw new Exception("Error: Could not find Project by Id");

            var security = new ProjectSecurityLogic(_context);
            var access = security.DetermineAccessLevel(ProjectId, UserId);

            if (access < 0 || access > 1) throw new Exception("Error: Cannot update, User has insufficient privileges"); //User must be owner or developer

            project.ProjectTitle = ProjectTitle;
            project.ProjectDescription = ProjectDescription;
            project.ProjectLangauge = ProjectLangauge;
            project.ProjectDatabase = ProjectDatabase;
            project.ProjectBackend = ProjectBackend;
            project.ProjectTechnologyMisc = ProjectTechnologyMisc;
            project.ProjectLink = ProjectLink;
            project.DockerId = DockerId;
            project.UpdatedDate = DateTime.Now;

            _context.SaveChanges();
        }
    }
}
