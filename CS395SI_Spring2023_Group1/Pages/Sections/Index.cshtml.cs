using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CS395SI_Spring2023_Group1.Data;
using CS395SI_Spring2023_K4S.Model;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CS395SI_Spring2023_Group1.Pages.Sections
{
    public class IndexModel : PageModel
    {
        private readonly CS395SI_Spring2023_Group1.Data.CS395SI_Spring2023_Group1Context _context;

        public IndexModel(CS395SI_Spring2023_Group1.Data.CS395SI_Spring2023_Group1Context context)
        {
            _context = context;
        }

        public IList<Spring2024_Group2_Sections> Spring2024_Group2_Sections { get; set; } = default!;
        public string ServiceID { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(string serviceID)
        {
            if (string.IsNullOrEmpty(serviceID))
            {
                return RedirectToPage("/Services/Index"); 
            }

            ServiceID = serviceID;

            Spring2024_Group2_Sections = await _context.Spring2024_Group2_Sections
                .Where(s => s.serviceID == serviceID)
                .ToListAsync();

            ServiceName = await _context.Spring2023_Group1_Services
                .Where(s => s.ServiceID == serviceID)
                .Select(s => s.ServiceName)
                .FirstOrDefaultAsync() ?? "Unknown Service";

            return Page();
        }


        public IActionResult OnPost(string? sectionID)
        {
            if (string.IsNullOrEmpty(sectionID))
            {
                TempData["ErrorMessage"] = "Section ID is required.";
                return Page(); 
            }

            HttpContext.Session.SetString("sectionID", sectionID);
            // return RedirectToPage("/AttendanceForAdmin/Index"); 
            return RedirectToPage("/AttendanceForAdmin/Index", new { sectionID });

        }

    }
}