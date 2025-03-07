using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CS395SI_Spring2023_Group1.Data;
using CS395SI_Spring2023_K4S.Model;

namespace CS395SI_Spring2023_Group1.Pages.SectionForStudent
{
    public class SectionEnrollModel : PageModel
    {
        private readonly CS395SI_Spring2023_Group1.Data.CS395SI_Spring2023_Group1Context _context;

        public SectionEnrollModel(CS395SI_Spring2023_Group1.Data.CS395SI_Spring2023_Group1Context context)
        {
            _context = context;
        }

        public IList<Spring2024_Group2_Sections> Spring2024_Group2_Sections { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync(string ServiceID)
        {
            string studentEmail = HttpContext.Session.GetString("studentEmail");

            Spring2024_Group2_Sections = await _context.Spring2024_Group2_Sections
                .Where(s => s.serviceID == ServiceID)
                .ToListAsync();

            var enrolledSections = await _context.Spring2024_Group2_Schedule
                .Where(s => s.StudentEmail == studentEmail)
                .Select(s => s.SectionID)
                .ToListAsync();

            ViewData["EnrolledSections"] = enrolledSections;

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            var serviceID = Request.Form["serviceID"];
            var serviceName = Request.Form["serviceName"];
            var sectionID = Request.Form["sectionID"];

            if (!int.TryParse(sectionID, out int validSectionID))
            {
                return new JsonResult(new { success = false, message = "Invalid section ID." });
            }

            string studentEmail = HttpContext.Session.GetString("studentEmail");

            bool alreadyEnrolled = await _context.Spring2024_Group2_Schedule
                .AnyAsync(s => s.StudentEmail == studentEmail && s.SectionID == validSectionID);

            if (alreadyEnrolled)
            {
                return new JsonResult(new { success = false, message = "You are already enrolled in this course." });
            }

            var groupSchedule = new Spring2024_Group2_Schedule
            {
                ServiceID = serviceID,
                ServiceName = serviceName,
                SectionID = validSectionID,
                StartDate = DateTime.Parse(Request.Form["StartDate"]),
                EndDate = DateTime.Parse(Request.Form["EndDate"]),
                WeekDay = Request.Form["weekDay"],
                StartTime = TimeSpan.Parse(Request.Form["startTime"]),
                EndTime = TimeSpan.Parse(Request.Form["endTime"]),
                Status = "Pending",
                StudentEmail = studentEmail
            };

            _context.Spring2024_Group2_Schedule.Add(groupSchedule);
            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true, sectionID = validSectionID });
        }

    }
}
