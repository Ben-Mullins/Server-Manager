using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            User user = _context.User.FirstOrDefault(u => u.UserId == UserId);

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
            User user = _context.User.FirstOrDefault(u => u.UserId == UserId);

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

        public void GrantDeveloperAccess(int ProjectId, int UserId, int OwnerId)
        {
            User user = _context.User.FirstOrDefault(u => u.UserId == UserId);

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

        public void GrantViewerAccess(int ProjectId, int UserId, int OwnerId)
        {
            User user = _context.User.FirstOrDefault(u => u.UserId == UserId);

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

        public void RevokeAccess(int ProjectId, int UserId, int OwnerId)
        {
            User user = _context.User.FirstOrDefault(u => u.UserId == UserId);

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
        /// <returns>The integer value level of access, 3 being no access</returns>
        public int DetermineAccessLevel(int ProjectId, int UserId)
        {
            Project project = _context.Project.FirstOrDefault(p => p.ProjectId == ProjectId);
            User user = _context.User.FirstOrDefault(u => u.UserId == UserId);

            if (project == null || user == null) throw new Exception("Error: unable to find user or project");

            ProjectUsers projectUser = _context.ProjectUsers.Where(pu => pu.ProjectId == ProjectId).FirstOrDefault(pu => pu.UserId == UserId);

            int accessLevel = user.UserAccessLevel;
            if(projectUser != null)
            {
                if (accessLevel > projectUser.AccessLevel && projectUser.AccessLevel >= 0) accessLevel = projectUser.AccessLevel; //If project access level is better (0 - 2) than global, but not negative
            }

            return accessLevel;
        }
    }
}
