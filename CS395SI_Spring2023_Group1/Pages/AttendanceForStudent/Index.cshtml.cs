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
using System.Security.Claims;

namespace CS395SI_Spring2023_Group1.Pages.AttendanceForStudent
{
    [Authorize(Roles = "Instructor")]
    public class IndexModel : PageModel
    {
        private readonly CS395SI_Spring2023_Group1Context _context;

        public IndexModel(CS395SI_Spring2023_Group1Context context)
        {
            _context = context;
        }

        public IList<Spring2025_Group3_Attendance> AttendanceRecords { get; set; } = default!;
        public string? ServiceName { get; set; }
        public string? ScheduleTime { get; set; }
        public string? StudentEmail { get; set; }

        [BindProperty(SupportsGet = true)]
        public int ScheduleID { get; set; }

        [BindProperty(SupportsGet = true)]
        public int WeekOffset { get; set; } = 0;

        public DateTime WeekStart { get; set; }
        
        public int SectionID { get; set; }
        public string? ServiceID { get; set; }
        
        public double PersonalAttendancePercentage { get; set; } = 0;
        public int TotalSessionsPresent { get; set; } = 0;
        public int TotalSessions { get; set; } = 0;
        
        public bool IsAdmin { get; set; } = false;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            ScheduleID = id;
            
            // Check if user is an admin
            IsAdmin = User.IsInRole("Admin");
            
            // Get the current user's email or use the student's email provided in the URL
            var currentUserEmail = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(currentUserEmail))
            {
                return RedirectToPage("/Account/Login");
            }
            
            // Get schedule information - regardless of whether the user is the student or an admin
            var schedule = await _context.Spring2024_Group2_Schedule
                .FirstOrDefaultAsync(s => s.ScheduleID == ScheduleID);

            if (schedule == null)
            {
                return NotFound("Schedule not found.");
            }
            
            // Set the student email based on the schedule
            StudentEmail = schedule.StudentEmail;
            
            // If the user is not an admin and not the student of this schedule, deny access
            if (!IsAdmin && currentUserEmail != StudentEmail)
            {
                return NotFound("You are not enrolled in this schedule.");
            }

            SectionID = schedule.SectionID;
            ServiceID = schedule.ServiceID;

            // Calculate the start of the current week (or offset week)
            WeekStart = DateTime.UtcNow.Date.AddDays(-(int)DateTime.UtcNow.DayOfWeek + (WeekOffset * 7));
            if (WeekStart.DayOfWeek != DayOfWeek.Monday)
            {
                WeekStart = WeekStart.AddDays(1); // Ensure we start on Monday
            }

            // Get section information
            var sectionInfo = await _context.Spring2024_Group2_Sections
                .Where(s => s.sectionID == SectionID)
                .Select(s => new
                {
                    s.serviceName,
                    s.StartDate,
                    s.EndDate,
                    s.weekDay,
                    s.startTime,
                    s.endTime
                })
                .FirstOrDefaultAsync();

            if (sectionInfo != null)
            {
                ServiceName = sectionInfo.serviceName;
                ScheduleTime = $"{sectionInfo.weekDay} | {sectionInfo.startTime} - {sectionInfo.endTime}";
            }

            // Get attendance records for the student for the current week
            AttendanceRecords = await _context.Spring2025_Group3_Attendance
                .Where(a => a.Email == StudentEmail && 
                           a.SectionID == SectionID &&
                           a.CurrentDate >= WeekStart && 
                           a.CurrentDate < WeekStart.AddDays(7))
                .OrderBy(a => a.CurrentDate)
                .ToListAsync();

            // If no records exist for this week, create placeholders but don't save them
            var placeholders = new List<Spring2025_Group3_Attendance>();
            
            if (!AttendanceRecords.Any() && sectionInfo != null)
            {
                for (int i = 0; i < 7; i++)
                {
                    var currentDate = WeekStart.AddDays(i);
                    
                    // Check if this is a day the class meets
                    if (currentDate.DayOfWeek.ToString() == sectionInfo.weekDay)
                    {
                        placeholders.Add(new Spring2025_Group3_Attendance
                        {
                            Email = StudentEmail,
                            SectionID = SectionID,
                            ServiceID = ServiceID,
                            ScheduleID = ScheduleID,
                            CurrentDate = currentDate,
                            AttendanceStatus = "Not Marked"
                        });
                    }
                }
                
                // Just use the placeholders for display, don't save to DB
                AttendanceRecords = placeholders;
            }

            // Calculate attendance percentage
            // Get all attendance records for this student in this section
            var allAttendanceRecords = await _context.Spring2025_Group3_Attendance
                .Where(a => a.Email == StudentEmail && 
                           a.SectionID == SectionID)
                .ToListAsync();

            TotalSessions = allAttendanceRecords.Count;
            TotalSessionsPresent = allAttendanceRecords.Count(a => a.AttendanceStatus == "Present");

            if (TotalSessions > 0)
            {
                PersonalAttendancePercentage = (double)TotalSessionsPresent / TotalSessions * 100;
            }

            return Page();
        }
    }
}