using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServerProjectTracker.Models
{
    /// <summary>
    /// The Data Class associated with the Project table in the db
    /// </summary>
    public class Project
    {
        [Required]
        public int ProjectId { get; set; }

        /// <summary>
        /// The title of the project
        /// </summary>
        [Required, MaxLength(250)]
        public string ProjectTitle { get; set; }

        /// <summary>
        /// A simple description of what the project is overall, and what it's purpose is
        /// </summary>
        [Required]
        public string ProjectDescription { get; set; }

        /// <summary>
        /// The id used by Docker to identify a container, to allow the tracker to connect a project to the docker id using the API
        /// </summary>
        public string DockerId { get; set; }

        /// <summary>
        /// The language the project is primarily written in
        /// </summary>
        [MaxLength(250)]
        public string ProjectLangauge { get; set; }

        /// <summary>
        /// The type of database the project uses
        /// </summary>
        [MaxLength(250)]
        public string ProjectDatabase { get; set; }

        /// <summary>
        /// The is the back end "Core" of the project, like the JS library, or ASP.net, etc.
        /// </summary>
        [MaxLength(250)]
        public string ProjectBackend { get; set; }

        /// <summary>
        /// This can be a catch all for any other type of specialized technology that are used in the project that are worth mentioning
        /// </summary>
        public string ProjectTechnologyMisc { get; set; }

        /// <summary>
        /// This is for storing the location of the project's link this would need to match what it is in nginx, as this should be the public url to the project home page
        /// </summary>
        public string ProjectLink { get; set; }

        /// <summary>
        /// This is for storing the link for the project image link, or the default placeholder image link. 
        /// </summary>
        public string ProjectImageLink { get; set; }

        /// <summary>
        /// The Date that the project was added
        /// </summary>
        [DataType(DataType.Date), Required]
        public DateTime AddedDate { get; set; }

        /// <summary>
        /// The date the project was last updated on the server
        /// </summary>
        [DataType(DataType.Date), Required]
        public DateTime UpdatedDate { get; set; }
    }
}
