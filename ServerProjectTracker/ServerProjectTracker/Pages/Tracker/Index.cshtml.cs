using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServerProjectTracker.Pages.Tracker
{
    public class IndexModel : PageModel
    {
        private readonly Data.ServerProjectTrackerContext _context;

        public IndexModel(Data.ServerProjectTrackerContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }
    }
}
