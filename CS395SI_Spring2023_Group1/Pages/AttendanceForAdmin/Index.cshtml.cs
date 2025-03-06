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

namespace CS395SI_Spring2023_Group1.Pages.AttendanceForAdmin
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly CS395SI_Spring2023_Group1.Data.CS395SI_Spring2023_Group1Context _context;

        public IndexModel(CS395SI_Spring2023_Group1.Data.CS395SI_Spring2023_Group1Context context)
        {
            _context = context;
        }

        public IList<Spring2025_Group3_Attendance> AttendanceRecords { get; set; } = new List<Spring2025_Group3_Attendance>();
        public string SessionTitle { get; set; } = "N/A"; 
        public string ScheduleTime { get; set; } = "N/A"; 

        [BindProperty(SupportsGet = true)]
        public int WeekOffset { get; set; } = 0; // Allows week navigation

        public DateTime WeekStart { get; set; } // Stores the first day of the selected week

        public async Task OnGetAsync()
        {
            WeekStart = DateTime.UtcNow.Date.AddDays(-(int)DateTime.UtcNow.DayOfWeek + (WeekOffset * 7));

            if (_context.Spring2025_Group3_Attendance != null)
            {
                AttendanceRecords = await _context.Spring2025_Group3_Attendance
                    .Where(a => a.CurrentDate >= WeekStart && a.CurrentDate < WeekStart.AddDays(7))
                    .OrderBy(a => a.CurrentDate)
                    .ToListAsync();

                // Fetch session title dynamically from the first attendance record
                var firstRecord = AttendanceRecords.FirstOrDefault();
                if (firstRecord != null)
                {
                    var session = await _context.Spring2023_Group1_Services
                        .Where(s => s.ServiceID == firstRecord.ServiceID)
                        .FirstOrDefaultAsync();

                    if (session != null)
                    {
                        SessionTitle = session.ServiceName;
                        ScheduleTime = "Mon. & Wed. 9:00am"; // Ideally, fetch dynamically if stored
                    }
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string email = Request.Form["email"];
            DateTime date = DateTime.Parse(Request.Form["date"]);
            string status = Request.Form["status"];

            var attendance = await _context.Spring2025_Group3_Attendance
                .FirstOrDefaultAsync(a => a.Email == email && a.CurrentDate.Date == date);

            if (attendance != null)
            {
                attendance.AttendanceStatus = status;
                _context.Update(attendance);
            }
            else
            {
                _context.Spring2025_Group3_Attendance.Add(new Spring2025_Group3_Attendance
                {
                    // AttendanceID = Guid.NewGuid().ToString(),
                    Email = email,
                    ServiceID = null,
                    SectionID = 0,
                    ScheduleID = 0,
                    CurrentDate = date,
                    AttendanceStatus = status
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index", new { WeekOffset });
        }
    }
}
