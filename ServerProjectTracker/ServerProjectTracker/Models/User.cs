using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServerProjectTracker.Models
{
    public class User
    {
        [Required]
        public int UserId { get; set; }
        
        /// <summary>
        /// This is the username of the user, if we link this to cas, it would be their CAS username
        /// </summary>
        [Required, MaxLength(100)]
        public string Username { get; set; }

        /// <summary>
        /// If we link to CAS we should remove this field, and replace it with however we bind the User to the CAS account.
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// The first name of the user
        /// </summary>
        [Required, MaxLength(50)]
        public string Firstname { get; set; }

        /// <summary>
        /// The last name of the user
        /// </summary>
        [Required, MaxLength(75)]
        public string Lastname { get; set; }

        /// <summary>
        /// This is to determine the type of user, whether they are an Instructor or User. 
        /// </summary>
        [Required]
        public char Usertype { get; set; }

        /// <summary>
        /// This should be a User's overall Access Level on the site. 
        /// Level 0 is Global Ownership Access, and should be reserved to Server Admins only (Full Access to all projects)
        /// Level 1 is Global Developer Access, and should be reserved for Server Admin Developers only (Partial Access to all projects)
        /// Level 2 is Global View Access, and should be reserved for Admins (Per project access level is also set for allowing better access)
        /// Level 3 is Per Project Access Only, and should be the default 
        /// </summary>
        [Required]
        public int UserAccessLevel { get; set; }
    }
}
