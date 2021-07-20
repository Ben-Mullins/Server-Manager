using ServerProjectTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerProjectTracker.AppLogic
{
    public class UserLogic
    {
        private readonly Data.ServerProjectTrackerContext _context;

        public UserLogic(Data.ServerProjectTrackerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// A method to elevate the global User Access Level, requires global ownership access
        /// </summary>
        /// <param name="UserId">The id for the user to modify</param>
        /// <param name="OriginId">The id for the originating user</param>
        public void ElevateAccess(int UserId, int OriginId)
        {
            if (UserId == OriginId) throw new Exception("Error: Cannot modify own access level");

            Users user = _context.Users.FirstOrDefault(u => u.UserId == UserId);
            Users origin = _context.Users.FirstOrDefault(u => u.UserId == OriginId);

            if (user == null || origin == null) throw new Exception("Error: unable to find user or origin user by Id");

            if(origin.UserAccessLevel != 0) throw new Exception("Error: Origin lacks sufficient access");

            if (user.UserAccessLevel <= 0) throw new Exception("Error: Cannot further elevate access");

            user.UserAccessLevel -= 1;
            _context.SaveChanges();
        }

        /// <summary>
        /// A method to reduce the global User Access Level, requires global ownership access
        /// </summary>
        /// <param name="UserId">The id for the user to modify</param>
        /// <param name="OriginId">The id for the originating user</param>
        public void ReduceAccess(int UserId, int OriginId)
        {
            if (UserId == OriginId) throw new Exception("Error: Cannot modify own access level");

            Users user = _context.Users.FirstOrDefault(u => u.UserId == UserId);
            Users origin = _context.Users.FirstOrDefault(u => u.UserId == OriginId);

            if (user == null || origin == null) throw new Exception("Error: unable to find user or origin user by Id");

            if (origin.UserAccessLevel != 0) throw new Exception("Error: Origin lacks sufficient access");

            if (user.UserAccessLevel >= 4) throw new Exception("Error: Cannot further reduce access, revoke access instead");

            user.UserAccessLevel += 1;
            _context.SaveChanges();
        }

        /// <summary>
        /// A method to revoke the global User Access Level, requires global ownership access
        /// </summary>
        /// <param name="UserId">The id for the user to modify</param>
        /// <param name="OriginId">The id for the originating user</param>
        public void RevokeAccess(int UserId, int OriginId)
        {
            if (UserId == OriginId) throw new Exception("Error: Cannot modify own access level");

            Users user = _context.Users.FirstOrDefault(u => u.UserId == UserId);
            Users origin = _context.Users.FirstOrDefault(u => u.UserId == OriginId);

            if (user == null || origin == null) throw new Exception("Error: unable to find user or origin user by Id");

            if (origin.UserAccessLevel != 0) throw new Exception("Error: Origin lacks sufficient access");

            user.UserAccessLevel = 5;
            _context.SaveChanges();
        }
    }
}
