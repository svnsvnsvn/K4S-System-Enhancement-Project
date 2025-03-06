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

        public async Task OnGetAsync(string ServiceID)
        {
            Spring2024_Group2_Sections=await _context.Spring2024_Group2_Sections
                .Where (s => s.serviceID == ServiceID)
                .ToListAsync();
        }

        public IActionResult OnPost()
        {
            var serviceID = Request.Form["ServiceID"];
            var serviceName = Request.Form["ServiceName"];
            var scheduleID = Request.Form["ScheduleID"];


            TimeSpan startTime = TimeSpan.Zero;
            TimeSpan endtime = TimeSpan.Zero;
            DateTime StartDate = DateTime.MinValue;
            DateTime EndDate = DateTime.MinValue;

            TimeSpan.TryParse(Request.Form["StartTime"],out startTime);
            TimeSpan.TryParse(Request.Form["EndTime"],out endtime);
            DateTime.TryParse(Request.Form["StartDate"],out StartDate);
            DateTime.TryParse(Request.Form["EndDate"], out EndDate);

            var weekDay = Request.Form["WeekDay"];
            var status = "Pending";
            string studentEmail = HttpContext.Session.GetString("studentEmail");
            var groupSchedule = new Spring2024_Group2_Schedule
            {
                ServiceID = serviceID,
              
                ServiceName = serviceName,
                StartTime = startTime,
                EndTime = endtime,
                StartDate = StartDate,
                EndDate = EndDate,
                WeekDay = weekDay,
                StudentEmail=studentEmail
            };
            _context.Spring2024_Group2_Schedule.Add(groupSchedule);
            _context.SaveChanges();

            return RedirectToAction("AvailableService/Index"); // SHOULD NOT BE REDIRECTING. SHOULD just change the textof the button
            // return null; // SHOULD NOT BE REDIRECTING. SHOULD just change the textof the button

        }
    }
}
