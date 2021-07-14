using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServerProjectTracker.Models;

namespace ServerProjectTracker.AppLogic
{
   /// <summary>
   /// This logic class is used to manage access to specific projects.
   /// As a Rule, a user should never be able to change their own access level from the web application, and only an owner can grant it to others
   /// </summary>
    public class ProjectSecurityLogic
    {
        private readonly Data.ServerProjectTrackerContext _context;

        public ProjectSecurityLogic(Data.ServerProjectTrackerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This method is used to give ownership rights to the creator of a project, and only used on new projects
        /// </summary>
        /// <param name="ProjectId">The Id of the new project</param>
        /// <param name="UserId">The Id for the creator of the project</param>
        public void SetNewProjectOwner(int ProjectId, int UserId)
        {
            Project project = _context.Project.FirstOrDefault(p => p.ProjectId == ProjectId);
            Users user = _context.Users.FirstOrDefault(u => u.UserId == UserId);

            if (project == null || user == null) throw new Exception("Error: unable to find user or project");

            var userlist = _context.ProjectUsers.Where(pu => pu.ProjectId == ProjectId).ToList();

            if (userlist.Count > 0) throw new Exception("Error: Cannot set owner on non-new project");

            ProjectUsers newAccess = new ProjectUsers();
            newAccess.ProjectId = ProjectId;
            newAccess.UserId = UserId;
            newAccess.AccessLevel = 0;
            newAccess.AccessGrantedDate = DateTime.Now;

            _context.ProjectUsers.Add(newAccess);
            _context.SaveChanges();
        }

        /// <summary>
        /// Used to grant ownership rights to a new user on a project
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="UserId"></param>
        /// <param name="OwnerId"></param>
        public void GrantOwnershipAccess(int ProjectId, int UserId, int OwnerId)
        {
            if (UserId == OwnerId) throw new Exception("Error: Cannot modify own access level");
            
            Users user = _context.Users.FirstOrDefault(u => u.UserId == UserId);

            if (user == null) throw new Exception("Error: unable to find user by Id");

            var ownerAccess = DetermineAccessLevel(ProjectId, OwnerId);

            if (ownerAccess != 0) throw new Exception("Error: Provided OwnerId is not an owner and cannot grant or revoke access");

            ProjectUsers userAccess = _context.ProjectUsers.Where(pu => pu.ProjectId == ProjectId).FirstOrDefault(pu => pu.UserId == UserId);

            // Meaning they have no history of access
            if(userAccess == null)
            {
                userAccess = new ProjectUsers();
                userAccess.ProjectId = ProjectId;
                userAccess.UserId = UserId;
                userAccess.AccessLevel = 0;
                userAccess.AccessGrantedDate = DateTime.Now;
                userAccess.AccessGranterUserId = OwnerId;

                _context.ProjectUsers.Add(userAccess);
                _context.SaveChanges();
                return;
            }

            userAccess.AccessLevel = 0;
            userAccess.AccessUpdatedDate = DateTime.Now;
            userAccess.AccessGranterUserId = OwnerId;

            _context.SaveChanges();
        }

        /// <summary>
        /// Used to grant developer access rights
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="UserId"></param>
        /// <param name="OwnerId"></param>
        public void GrantDeveloperAccess(int ProjectId, int UserId, int OwnerId)
        {
            if (UserId == OwnerId) throw new Exception("Error: Cannot modify own access level");

            Users user = _context.Users.FirstOrDefault(u => u.UserId == UserId);

            if (user == null) throw new Exception("Error: unable to find user by Id");

            var ownerAccess = DetermineAccessLevel(ProjectId, OwnerId);

            if (ownerAccess != 0) throw new Exception("Error: Provided OwnerId is not an owner and cannot grant or revoke access");

            ProjectUsers userAccess = _context.ProjectUsers.Where(pu => pu.ProjectId == ProjectId).FirstOrDefault(pu => pu.UserId == UserId);

            // Meaning they have no history of access
            if (userAccess == null)
            {
                userAccess = new ProjectUsers();
                userAccess.ProjectId = ProjectId;
                userAccess.UserId = UserId;
                userAccess.AccessLevel = 1;
                userAccess.AccessGrantedDate = DateTime.Now;
                userAccess.AccessGranterUserId = OwnerId;

                _context.ProjectUsers.Add(userAccess);
                _context.SaveChanges();
                return;
            }

            userAccess.AccessLevel = 1;
            userAccess.AccessUpdatedDate = DateTime.Now;
            userAccess.AccessGranterUserId = OwnerId;

            _context.SaveChanges();
        }


        /// <summary>
        /// Used to grant viewer access level
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="UserId"></param>
        /// <param name="OwnerId"></param>
        public void GrantViewerAccess(int ProjectId, int UserId, int OwnerId)
        {
            if (UserId == OwnerId) throw new Exception("Error: Cannot modify own access level");

            Users user = _context.Users.FirstOrDefault(u => u.UserId == UserId);

            if (user == null) throw new Exception("Error: unable to find user by Id");

            var ownerAccess = DetermineAccessLevel(ProjectId, OwnerId);

            if (ownerAccess != 0) throw new Exception("Error: Provided OwnerId is not an owner and cannot grant or revoke access");

            ProjectUsers userAccess = _context.ProjectUsers.Where(pu => pu.ProjectId == ProjectId).FirstOrDefault(pu => pu.UserId == UserId);

            // Meaning they have no history of access
            if (userAccess == null)
            {
                userAccess = new ProjectUsers();
                userAccess.ProjectId = ProjectId;
                userAccess.UserId = UserId;
                userAccess.AccessLevel = 2;
                userAccess.AccessGrantedDate = DateTime.Now;
                userAccess.AccessGranterUserId = OwnerId;

                _context.ProjectUsers.Add(userAccess);
                _context.SaveChanges();
                return;
            }

            userAccess.AccessLevel = 2;
            userAccess.AccessUpdatedDate = DateTime.Now;
            userAccess.AccessGranterUserId = OwnerId;

            _context.SaveChanges();
        }

        /// <summary>
        /// Used to revoke access rights to a project
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="UserId"></param>
        /// <param name="OwnerId"></param>
        public void RevokeAccess(int ProjectId, int UserId, int OwnerId)
        {
            if (UserId == OwnerId) throw new Exception("Error: Cannot modify own access level");

            Users user = _context.Users.FirstOrDefault(u => u.UserId == UserId);

            if (user == null) throw new Exception("Error: unable to find user by Id");

            var ownerAccess = DetermineAccessLevel(ProjectId, OwnerId);

            if (ownerAccess != 0) throw new Exception("Error: Provided OwnerId is not an owner and cannot grant or revoke access");

            ProjectUsers userAccess = _context.ProjectUsers.Where(pu => pu.ProjectId == ProjectId).FirstOrDefault(pu => pu.UserId == UserId);

            // Meaning they have no history of access
            if (userAccess == null) throw new Exception("Error: User has never had access to revoke");

            userAccess.AccessLevel = -1;
            userAccess.AccessUpdatedDate = DateTime.Now;
            userAccess.AccessGranterUserId = OwnerId;

            _context.SaveChanges();
        }

        /// <summary>
        /// A User has both a per project access level, as well as their global User Access Level. 
        /// </summary>
        /// <param name="ProjectId">The Id of the project in question</param>
        /// <param name="UserId">The Id of the User in question</param>
        /// <returns>The integer value level of access, 3 or higher being no access</returns>
        public int DetermineAccessLevel(int ProjectId, int UserId)
        {
            Project project = _context.Project.FirstOrDefault(p => p.ProjectId == ProjectId);
            Users user = _context.Users.FirstOrDefault(u => u.UserId == UserId);

            if (project == null || user == null) throw new Exception("Error: unable to find user or project");

            ProjectUsers projectUser = _context.ProjectUsers.Where(pu => pu.ProjectId == ProjectId).FirstOrDefault(pu => pu.UserId == UserId);

            int accessLevel = user.UserAccessLevel;
            if(projectUser != null)
            {
                if (accessLevel > projectUser.AccessLevel && projectUser.AccessLevel >= 0) accessLevel = projectUser.AccessLevel; //If project access level is better (0 - 2) than global, but not negative
            }

            return accessLevel;
        }

        public int GetAccessLevel(int ProjectId, int UserId)
        {
            Project project = _context.Project.FirstOrDefault(p => p.ProjectId == ProjectId);
            Users user = _context.Users.FirstOrDefault(u => u.UserId == UserId);

            if (project == null || user == null) throw new Exception("Error: unable to find user or project");

            ProjectUsers projectUser = _context.ProjectUsers.Where(pu => pu.ProjectId == ProjectId).FirstOrDefault(pu => pu.UserId == UserId);

            return projectUser.AccessLevel;
        }

        /// <summary>
        /// Used to get a list of viable projects, intended for use by the tracker dashboard
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<Project> DetermineViewableProjects(int UserId)
        {
            Users user = _context.Users.FirstOrDefault(u => u.UserId == UserId);
            if (user == null) throw new Exception("Error: unable to find user");

            if(user.UserAccessLevel <= 2 && user.UserAccessLevel >= 0) //User has global view access or better
            {
                return _context.Project.ToList();
            }

            List<Project> projects = new List<Project>();

            var projectUsers = _context.ProjectUsers.Include(p => p.Project).Where(p => p.UserId == user.UserId).ToList();

            foreach (var pu in projectUsers)
            {
                if(pu.AccessLevel >= 0 && pu.AccessLevel < 3)
                    projects.Add(pu.Project);
            }

            return projects;
        }

        /// <summary>
        /// Generates a list of Access Rules for a given project for use on the project security page
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public List<ProjectAccessRule> GetAccessRules(int ProjectId)
        {
            Project project = _context.Project.FirstOrDefault(p => p.ProjectId == ProjectId);

            if (project == null) throw new Exception("Error: unable to find project");

            List<ProjectAccessRule> projectAccessRules = new List<ProjectAccessRule>();
            var projectUsers = _context.ProjectUsers.Include(p => p.User).Where(p => p.ProjectId == ProjectId).ToList();

            foreach (var pjusr in projectUsers)
            {
                var AccessGranter = _context.Users.FirstOrDefault(u => u.UserId == pjusr.AccessGranterUserId);
                string AccessString = "";

                switch (pjusr.AccessLevel)
                {
                    case (0):
                        AccessString = "Owner";
                        break;
                    case (1):
                        AccessString = "Developer";
                        break;
                    case (2):
                        AccessString = "Viewer";
                        break;
                    default:
                        AccessString = "Revoked";
                        break;
                }

                projectAccessRules.Add(new ProjectAccessRule(pjusr.User, AccessGranter, pjusr.AccessLevel, AccessString, pjusr.AccessGrantedDate, pjusr.AccessUpdatedDate));
            }

            return projectAccessRules;
        }

        /// <summary>
        /// A method for getting a list of users who could be added to a project that are not already
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public List<string> GetPotentialNewUsers(int ProjectId)
        {
            Project project = _context.Project.FirstOrDefault(p => p.ProjectId == ProjectId);

            if (project == null) throw new Exception("Error: unable to find project");

            var userList = _context.Users.ToList();
            var projectUserList = _context.ProjectUsers.Where(p => p.ProjectId == ProjectId);
            List<int> projectUserIds = new List<int>();

            foreach (var user in projectUserList)
            {
                projectUserIds.Add(user.UserId);
            }

            List<string> usernames = new List<string>();

            foreach (var user in userList)
            {
                var has = projectUserIds.FindIndex(u => u == user.UserId);
                if (has == -1) usernames.Add(user.Username);
            }

            return usernames;
        }
    }

    /// <summary>
    /// This is a data support class for Security logic which organizes data necessary for manipulating project security data
    /// </summary>
    public class ProjectAccessRule
    {
        /// <summary>
        /// The User to be granted access
        /// </summary>
        public Users User { get; set; }

        /// <summary>
        /// The User who granted the user their current access level, can be null
        /// </summary>
        public Users AccessGranter { get; set; }

        /// <summary>
        /// The actual numerical value of access level of the rule
        /// </summary>
        public int AccessLevel { get; set; }

        /// <summary>
        /// A string representation of the access level
        /// </summary>
        public string AccessString { get; set; }

        /// <summary>
        /// The date that original access was granted
        /// </summary>
        public DateTime AccessGrantedDate { get; set; }

        /// <summary>
        /// If access level was changed, this reflects the time of the last change
        /// </summary>
        public DateTime? AccessUpdatedDate { get; set; }

        public ProjectAccessRule(Users user, Users accessGranter, int accessLevel, string accessString, DateTime accessGranted, DateTime? accessUpdated)
        {
            User = user;
            AccessGranter = accessGranter;
            AccessLevel = accessLevel;
            AccessString = accessString;
            AccessGrantedDate = accessGranted;
            AccessUpdatedDate = accessUpdated;
        }
    }
}
