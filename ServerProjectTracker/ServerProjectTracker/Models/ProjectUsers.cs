using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServerProjectTracker.Models
{
    public class ProjectUsers
    {
        [Required]
        public int ProjectId { get; set; }

        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Level 0 is Full Project Access, and is labeled "Owner", this is default for the User that creates the Project
        /// Level 1 is Partial Project Access, and is labeled "Developer"
        /// Level 2 is View Project Access, and is labeled "Viewer"
        /// A negative value indicates that access has been revoked, but was previously given to a specific user.
        /// </summary>
        [Required]
        public int AccessLevel { get; set; }

        /// <summary>
        /// This should be the date that original access was given
        /// </summary>
        [DataType(DataType.DateTime), Required]
        public DateTime AccessGrantedDate { get; set; }

        /// <summary>
        /// If the access level was changed at one point, this date would indicate when it was last changed. 
        /// </summary>
        [DataType(DataType.DateTime), Required]
        public DateTime AccessUpdatedDate { get; set; }

        public virtual Project Project { get; set; }

        public virtual User User { get; set; }
    }
}
