using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerProjectTracker.Models
{
    public class Container
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime Created { get; set; }
    }
}
