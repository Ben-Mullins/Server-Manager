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
        /// If access level is changed after initial access being granted, the new date will be reflected here 
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime? AccessUpdatedDate { get; set; }

        /// <summary>
        /// The Id associated with the User who granted the current access level
        /// </summary>
        public int? AccessGranterUserId { get; set; }

        public virtual Project Project { get; set; }

        public virtual Users User { get; set; }
    }
}
