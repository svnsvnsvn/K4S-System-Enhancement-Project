using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CS395SI_Spring2023_Group1.Data;
using CS395SI_Spring2023_K4S.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace CS395SI_Spring2023_Group1.Pages.StudentSchedule
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly CS395SI_Spring2023_Group1.Data.CS395SI_Spring2023_Group1Context _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(
            CS395SI_Spring2023_Group1.Data.CS395SI_Spring2023_Group1Context context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Spring2024_Group2_Schedule> Spring2024_Group2_Schedule { get; set; } = default!;
        public bool CanModifySchedule { get; set; }

        public async Task OnGetAsync()
        {
            // Set permission flag based on role
            CanModifySchedule = User.IsInRole("Admin") || !User.IsInRole("Instructor");
            
            // If instructor, show all student schedules
            if (User.IsInRole("Instructor"))
            {
                Spring2024_Group2_Schedule = await _context.Spring2024_Group2_Schedule
                    .ToListAsync();
            }
            else
            {
                // For regular users and admins, use the existing logic
                string studentEmail = HttpContext.Session.GetString("studentEmail");
                Spring2024_Group2_Schedule = await _context.Spring2024_Group2_Schedule
                    .Where(s => s.StudentEmail == studentEmail)
                    .ToListAsync();
            }
        }

        public IActionResult OnPost(string ScheduleID)
        {
            // Only allow non-instructors to perform this action
            if (User.IsInRole("Instructor"))
            {
                return Forbid();
            }
            
            HttpContext.Session.SetString("studentEmail", ScheduleID);
            return RedirectToPage("/SectionEnroll/AvailableService");
        }
    }
}