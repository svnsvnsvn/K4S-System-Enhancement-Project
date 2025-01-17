using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CS395SI_Spring2023_Group1.Data;
using CS395SI_Spring2023_K4S.Model;
using Microsoft.AspNetCore.Authorization;

namespace CS395SI_Spring2023_Group1.Pages.Registration
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly CS395SI_Spring2023_Group1.Data.CS395SI_Spring2023_Group1Context _context;

        public IndexModel(CS395SI_Spring2023_Group1.Data.CS395SI_Spring2023_Group1Context context)
        {
            _context = context;
        }

        public IList<Spring2023_Group1_Profile_Sys> Spring2023_Group1_Profile_Sys { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Spring2023_Group1_Profile_Sys != null)
            {
                Spring2023_Group1_Profile_Sys = await _context.Spring2023_Group1_Profile_Sys.ToListAsync();
            }
        }
        public IActionResult OnPost(string StudentEmail)
        {
            HttpContext.Session.SetString("studentEmail", StudentEmail);
            return RedirectToPage("../../StudentSchedule/Index");
        }
    }
}
