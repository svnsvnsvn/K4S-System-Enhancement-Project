using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CS395SI_Spring2023_Group1.Data;
using CS395SI_Spring2023_K4S.Model;

namespace CS395SI_Spring2023_Group1.Pages.StudentSchedule
{
    public class IndexModel : PageModel
    {
        private readonly CS395SI_Spring2023_Group1.Data.CS395SI_Spring2023_Group1Context _context;

        public IndexModel(CS395SI_Spring2023_Group1.Data.CS395SI_Spring2023_Group1Context context)
        {
            _context = context;
        }

        public IList<Spring2024_Group2_Schedule> Spring2024_Group2_Schedule { get;set; } = default!;

        public async Task OnGetAsync()
        {
            //if (_context.Spring2024_Group2_Schedule != null)
            //{
            //    Spring2024_Group2_Schedule = await _context.Spring2024_Group2_Schedule.ToListAsync();
            //}

            string studentEmail = HttpContext.Session.GetString("studentEmail");
            Spring2024_Group2_Schedule=await _context.Spring2024_Group2_Schedule
                .Where (s=>s.StudentEmail == studentEmail)
                .ToListAsync();
        }

                public IActionResult OnPost(string ScheduleID)
        {
            HttpContext.Session.SetString("studentEmail", ScheduleID);
            return RedirectToPage("/SectionEnroll/AvailableService");
        }
    }
}
