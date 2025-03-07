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
        public int SectionID { get; set; } 

        [BindProperty(SupportsGet = true)]
        public int WeekOffset { get; set; } = 0; 

        public DateTime WeekStart { get; set; } 

        public async Task OnGetAsync(int sectionID)
        {
            Console.WriteLine($"Section ID received: {sectionID}");

            SectionID = sectionID;

            WeekStart = DateTime.UtcNow.Date.AddDays(-(int)DateTime.UtcNow.DayOfWeek + (WeekOffset * 7));
            if (WeekStart.DayOfWeek != DayOfWeek.Monday)
            {
                WeekStart = WeekStart.AddDays(1); 
            }

            var sectionSchedule = await _context.Spring2024_Group2_Sections
                .Where(s => s.sectionID == sectionID)
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

            if (sectionSchedule != null)
            {
                SessionTitle = $"{sectionSchedule.serviceName} (Section {sectionID})";
                ScheduleTime = $"{sectionSchedule.weekDay} | {sectionSchedule.startTime} - {sectionSchedule.endTime}";
            }



            if (_context.Spring2025_Group3_Attendance != null && _context.Spring2024_Group2_Schedule != null)
            {
                var existingAttendance = await _context.Spring2025_Group3_Attendance
                .Where(a => a.SectionID == sectionID && a.CurrentDate >= WeekStart && a.CurrentDate < WeekStart.AddDays(7))
                .ToListAsync();

                var enrolledStudents = await _context.Spring2024_Group2_Schedule
                .Where(s => s.SectionID == sectionID)
                .Select(s => new { s.StudentEmail, s.SectionID, s.ServiceID, s.ScheduleID }) 
                .ToListAsync();


                Console.WriteLine($"Found {enrolledStudents.Count} enrolled students.");


                var newAttendanceRecords = new List<Spring2025_Group3_Attendance>();
                foreach (var student in enrolledStudents)
                {

                    bool existsInProfile = await _context.Spring2023_Group1_Profile_Sys
                        .AnyAsync(p => p.Email == student.StudentEmail);

                    if (!existsInProfile)
                    {
                        Console.WriteLine($"⚠ Skipping {student.StudentEmail} - Not found in Profile table!");
                        continue; 
                    }

                    Console.WriteLine($"Processing Student: {student.StudentEmail} - ScheduleID: {student.ScheduleID}");

                    for (int i = 0; i < 7; i++)
                    {
                        var currentDate = WeekStart.AddDays(i);
                        bool recordExists = existingAttendance
                            .Any(a => a.Email == student.StudentEmail && a.CurrentDate.Date == currentDate.Date);

                        if (!recordExists)
                        {

                            Console.WriteLine($"Adding attendance for {student.StudentEmail} on {currentDate} with ScheduleID: {student.ScheduleID}");
                            newAttendanceRecords.Add(new Spring2025_Group3_Attendance
                            {
                                Email = student.StudentEmail,
                                SectionID = student.SectionID,
                                ServiceID = student.ServiceID,
                                ScheduleID = student.ScheduleID, 
                                CurrentDate = currentDate,
                                AttendanceStatus = "Not Marked"
                            });

                        }
                    }
                }

                if (newAttendanceRecords.Count > 0)
                {
                    _context.Spring2025_Group3_Attendance.AddRange(newAttendanceRecords);
                    await _context.SaveChangesAsync();

                    existingAttendance = await _context.Spring2025_Group3_Attendance
                    .Where(a => a.SectionID == sectionID && a.CurrentDate >= WeekStart && a.CurrentDate < WeekStart.AddDays(7))
                    .ToListAsync();

                }

                AttendanceRecords = await _context.Spring2025_Group3_Attendance
                    .Where(a => a.CurrentDate >= WeekStart && a.CurrentDate < WeekStart.AddDays(7) && a.SectionID == sectionID)
                    .OrderBy(a => a.CurrentDate)
                    .ToListAsync();

                var firstRecord = AttendanceRecords.FirstOrDefault();
                if (firstRecord?.ServiceID != null)
                {
                    var session = await _context.Spring2023_Group1_Services
                        .Where(s => s.ServiceID == firstRecord.ServiceID)
                        .FirstOrDefaultAsync();

                    if (session != null)
                    {
                        SessionTitle = $"{session.ServiceName} (Section {sectionID})";
                        ScheduleTime = ScheduleTime = $"{sectionSchedule.weekDay} {sectionSchedule.startTime:hh\\:mm} - {sectionSchedule.endTime:hh\\:mm} | {sectionSchedule.StartDate:MM/dd/yyyy} - {sectionSchedule.EndDate:MM/dd/yyyy}";

                    }
                }
            }
        }



        public async Task<IActionResult> OnPostAsync()
        {
            string email = Request.Form["email"];
            DateTime date = DateTime.Parse(Request.Form["date"]);
            string status = Request.Form["status"];

            var validStatuses = new List<string> { "Present", "Absent", "Late", "Not Marked" };
            if (!validStatuses.Contains(status))
            {
                return BadRequest("Invalid attendance status. Allowed values: Present, Absent, Late, Not Marked.");

            }

            var enrollment = await _context.Spring2024_Group2_Schedule
                .Where(e => e.StudentEmail == email)
                .Select(e => new { e.ServiceID, e.SectionID, e.ScheduleID })
                .FirstOrDefaultAsync();

            if (enrollment == null)
            {
                return BadRequest("Student is not enrolled in any section.");
            }

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
                    Email = email,
                    ServiceID = enrollment.ServiceID,
                    SectionID = enrollment.SectionID,
                    ScheduleID = enrollment.ScheduleID,
                    CurrentDate = date,
                    AttendanceStatus = status
                });
            }

            await _context.SaveChangesAsync();
            return new JsonResult(new { success = true, message = "Attendance updated successfully." });
        }

    }
}
